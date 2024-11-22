using System;
using System.Linq;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;
using XRL.World.Skills;
using Event = XRL.World.Event;
using GameObject = XRL.World.GameObject;
using Skills = XRL.World.Parts.Skills;

namespace Modo.SkillTraining.Parts {
  /// <summary>Tracks melee attacks for purposes of weapon skill point calculations.</summary>
  /// <remarks>
  /// Temporarily attached to the target at the start of the player's melee attack, and removed afterward.
  /// </remarks>
  public class MeleeAttackTracker : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"New [{nameof(MeleeAttackTracker)}] attached to [{this.ParentObject}].");
    }

    public override Boolean WantEvent(Int32 id, Int32 cascade) => base.WantEvent(id, cascade)
                                                                  || id == DefendMeleeHitEvent.ID;

    /// <summary>
    /// Handle getting hit (before any damage calculations) and increase the training points accordingly.
    /// </summary>
    public override Boolean HandleEvent(DefendMeleeHitEvent ev) {
      var skill = ev.Weapon.GetWeaponSkill();
      var percentage = ModOptions.WeaponTrainingPercentage;
      if (ev.Attacker != The.Player
          || percentage < 1
          || The.Player.HasSkill(skill)
          || !ev.Weapon.IsEquippedInMainHand()) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"Successful hit on [{ev.Defender}] with [{ev.Weapon}] equipped in the main hand.");
      var points = Math.Round(percentage / new Decimal(100), 2);
      Req.PointTracker.AddPoints(skill, points);

      return base.HandleEvent(ev);
    }

    public override void Register(GameObject obj, IEventRegistrar registrar) {
      base.Register(obj, registrar);
      obj.RegisterPartEvent(this, EventNames.DefenderAfterAttack);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.DefenderAfterAttack)
        return base.FireEvent(ev);

      Output.DebugLog($"Removing [{nameof(MeleeAttackTracker)}] from [{this.ParentObject}]...");
      this.ParentObject.RemovePart<MeleeAttackTracker>();

      (
        from entry in Req.Player.RequirePart<PointTracker>().Points
        select entry.Key
        into skill
        where SkillFactory.Factory.SkillList[skill.SkillName()].Cost <= Req.PointTracker.Points[skill]
        select skill
      ).ToList().ForEach(skill => {
        Output.Alert(
          $"You have unlocked the {{{{Y|{skill.SkillName()}}}}} skill tree through practical training!");
        Req.PointTracker.RemoveSkill(skill);
        Req.Player.GetPart<Skills>().AddSkill(skill);
        Output.Log($"[{skill}] added to [{Req.Player}], training points removed.");
      });

      return base.FireEvent(ev);
    }
  }
}