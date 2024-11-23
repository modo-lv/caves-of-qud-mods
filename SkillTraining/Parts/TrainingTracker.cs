using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using UnityEngine;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Skills;
using Skills = XRL.World.Parts.Skills;

namespace Modo.SkillTraining.Parts {
  /// <summary>Main component that tracks training points for trainable skills.</summary>
  [Serializable]
  public class TrainingTracker : ModPart {
    /// <inheritdoc cref="Points"/>
    public Dictionary<String, Decimal> Points = null!;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      BeforeMeleeAttackEvent.ID,
      EquipperEquippedEvent.ID,
      BeforeFireMissileWeaponsEvent.ID,
    };

    public override void FinalizeRead(SerializationReader Reader) {
      base.FinalizeRead(Reader);
      if (this.Points.IsNullOrEmpty()) {
        Output.DebugLog("No training points loaded, starting from scratch.");
        this.Points = new Dictionary<String, Decimal> {
          { SkillClasses.Axe, 0 },
          { SkillClasses.BowAndRifle, 0 },
          { SkillClasses.Cudgel, 0 },
          { SkillClasses.DeftThrowing, 0 },
          { SkillClasses.HeavyWeapons, 0 },
          { SkillClasses.LongBlade, 0 },
          { SkillClasses.MultiweaponFighting, 0 },
          { SkillClasses.Pistol, 0 },
          { SkillClasses.ShortBlade, 0 },
          { SkillClasses.SingleWeaponFighting, 0 },
        };
      }
    }


    /// <summary>Melee weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeMeleeAttackEvent ev) {
      if (ev.Target.IsCreature)
        ev.Target.RequirePart<MeleeWeaponTrainer>();
      return base.HandleEvent(ev);
    }

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


    /// <summary>Increases training point value for a skill.</summary>
    public void AddPoints(String skillClass, Decimal amount) {
      if (!this.Points.ContainsKey(skillClass))
        this.Points[skillClass] = 0;
      this.Points[skillClass] += amount;

      Output.DebugLog($"[{skillClass.SkillName()}] + {amount} = {this.Points[skillClass]}");
      (
        from entry in Req.Player.RequirePart<TrainingTracker>().Points
        where SkillUtils.SkillOrPower(entry.Key)!.Cost <= entry.Value
        select entry.Key
      ).ToList().ForEach(skill => {
        Output.Alert($"You have unlocked {{{{Y|{skill.SkillName()}}}}} through practical training!");
        Req.TrainingTracker.RemoveSkill(skill);
        Req.Player.GetPart<Skills>().AddSkill(skill);
        Output.Log($"[{skill}] added to [{Req.Player}], training points removed.");
      });
    }

    /// <summary>Completely removes a skill from the training registry.</summary>
    public void RemoveSkill(String? skill) {
      if (skill == null) return;
      this.Points.Remove(skill);
    }


    /// <summary>
    /// Outputs the 
    /// </summary>
    public override String ToString() {
      var list =
        this.Points
          .Select(entry => {
            var name = $"○ {SkillFactory.GetSkillOrPowerName(entry.Key)} ";
            var value = "{{Y|" + $"{entry.Value:##0.00;;0}" + "}}";
            var current =
              "{{k|" + ("}}" + value).PadLeft(14, '0');
            var target =
              "{{k|" +
              ("}}" + SkillUtils.SkillOrPower(entry.Key)?.Cost).PadLeft(5, '0');

            return $"{(name + "{{K|").PadRight(34, '-') + "}}"} " +
                   $"{current} / {target}";

          })
          .Aggregate((a, b) => $"{a}\n{b}");

      return $"{{{{C|Skill training points}}}}\n\n{list}";
    }
  }
}