using System;
using SkillTraining.Utils;
using XRL;
using XRL.World;

namespace SkillTraining.Parts {
  /// <summary> Tracks melee attacks for purposes of skill training point accrual. </summary>
  /// <remarks>
  /// Temporarily attached to the target at the start of the player's melee attack, to track whether
  /// </remarks>
  public class MeleeAttackTracker : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"New [{nameof(MeleeAttackTracker)}] attached to [{this.ParentObject}].");
    }

    public override Boolean WantEvent(Int32 id, Int32 cascade) => base.WantEvent(id, cascade)
                                                                  || id == EndTurnEvent.ID
                                                                  || id == DefendMeleeHitEvent.ID;

    public override Boolean HandleEvent(DefendMeleeHitEvent ev) {
      var skill = "";
      var weaponCount = 0;

      if (ev.Attacker == The.Player) {
        skill = ev.Weapon.GetWeaponSkill();
        The.Player.ForeachEquippedObject(obj => {
          if (obj.EquippedOn().ThisPartWeapon() != null)
            weaponCount++;
        });
      }

      if (weaponCount > 0) {
        var points = Math.Round(new Decimal(1) / 5 / weaponCount, 2);
        Output.DebugLog($"Successful hit on [{this.ParentObject}] with 1 of {weaponCount} equipped weapon(s).");
        The.Player.RequirePart<PointTracker>().AddPoints(skill, points);
      }
      else {
        Output.DebugLog("Attack without equipped weapons, no training points to award.");
      }
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(EndTurnEvent ev) {
      Output.DebugLog($"Removing [{nameof(MeleeAttackTracker)}] from [{this.ParentObject}]...");
      this.ParentObject.RemovePart<MeleeAttackTracker>();
      return base.HandleEvent(ev);
    }
  }
}