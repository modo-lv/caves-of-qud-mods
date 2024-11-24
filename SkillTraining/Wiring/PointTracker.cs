using System;
using System.Collections.Generic;
using System.Linq;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Trainers;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;
using Skills = XRL.World.Parts.Skills;

namespace Modo.SkillTraining.Wiring {
  /// <summary>Main component that tracks training points for trainable skills.</summary>
  [Serializable] public class PointTracker : ModPart {
    /// <inheritdoc cref="Points"/>
    public Dictionary<String, Decimal> Points = new Dictionary<String, Decimal> {
      { SkillClasses.Axe, 0 },
      { SkillClasses.BowAndRifle, 0 },
      { SkillClasses.Cudgel, 0 },
      { SkillClasses.CookingAndGathering, 0 },
      { SkillClasses.CustomsAndFolklore, 0 },
      { SkillClasses.DeftThrowing, 0 },
      { SkillClasses.HeavyWeapon, 0 },
      { SkillClasses.LongBlade, 0 },
      { SkillClasses.MultiweaponFighting, 0 },
      { SkillClasses.Pistol, 0 },
      { SkillClasses.Shield, 0 },
      { SkillClasses.ShortBlade, 0 },
      { SkillClasses.SingleWeaponFighting, 0 },
      { SkillClasses.SnakeOiler, 0 },
      { SkillClasses.Swimming, 0 },
      { SkillClasses.Wayfaring, 0 },
    };

    private Boolean _disabledLogged = false;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      EquipperEquippedEvent.ID,
      BeforeFireMissileWeaponsEvent.ID,
    };


    /// <summary>Thrown weapon attack training.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<ThrownAttackTracker>();
      return base.HandleEvent(ev);
    }

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget?.IsCreature == true)
        ev.ApparentTarget.RequirePart<MissileAttackTrainer>();
      return base.HandleEvent(ev);
    }


    /// <summary>Process a known training action.</summary>
    public void HandleTrainingAction(PlayerAction action, Decimal amountModifier = 1m) {
      switch (ModOptions.TrainingEnabled) {
        case true when this._disabledLogged:
          this._disabledLogged = false;
          break;
        case false when this._disabledLogged:
          return;
        case false: {
          Output.Log(
            "Skill training has been disabled in game options, no points will be earned or skills unlocked."
          );
          this._disabledLogged = true;
          return;
        }
      }

      Output.DebugLog($"Player action: [{action}].");
      this.AddPoints(
        TrainingData.For(action).SkillClass,
        TrainingData.For(action).DefaultAmount * amountModifier
      );
      this.UnlockCompletedSkills();
    }

    /// <summary>Increases training point value for a skill (if applicable).</summary>
    public void AddPoints(String skillClass, Decimal amount) {
      var skill = SkillUtils.SkillOrPower(skillClass);
      if (amount > 0 && !Main.Player.HasSkill(skillClass)) {
        this.Points.TryAdd(skillClass, 0);
        if (amount > skill.Cost)
          this.Points[skillClass] = amount;
        else
          this.Points[skillClass] += amount;
        Output.DebugLog($"[{skillClass.SkillName()}] + {amount} = {this.Points[skillClass]}");
      }
      this.UnlockCompletedSkills();
    }

    /// <summary>Checks all trainable skills and unlocks those whos training is complete.</summary>
    private void UnlockCompletedSkills() {
      (
        from entry in Main.Player.RequirePart<PointTracker>().Points
        where SkillUtils.SkillOrPower(entry.Key)!.Cost <= entry.Value
        select entry.Key
      ).ToList().ForEach(unlocked => {
        var canUnlock = true;
        // Special case - Tactful has a minimum stat requirement
        if (unlocked == SkillClasses.CustomsAndFolklore) {
          canUnlock = SkillUtils.PowerByClass(SkillClasses.Tactful)!.MeetsAttributeMinimum(Main.Player);
        }
        if (!canUnlock)
          return;

        Output.Alert("{{Y|" + unlocked.SkillName() + "}} skill unlocked through practical training!");
        Main.Player.GetPart<Skills>().AddSkill(unlocked);
        Output.Log($"[{unlocked}] added to [{Main.Player}].");
        this.ResetPoints(unlocked);
      });
    }

    /// <summary>Resets training points back to 0.</summary>
    public void ResetPoints(String skillClass) {
      this.Points[skillClass] = 0;
      Output.Log($"[{skillClass}] training reset to 0.");
    }
  }
}