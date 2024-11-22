using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
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
      var skill = 
        SkillFactory.Factory.SkillByClass.GetValueOrDefault(ev.Weapon.GetWeaponSkill())?.Class;
      var percentage = ModOptions.MeleeTrainingPercentage;
      if (ev.Attacker != Req.Player
          || skill == null
          || percentage < 1
          || Req.Player.HasSkill(skill)
          // Only equipped weapons train skills
          || ev.Weapon.EquippedOn()?.ThisPartWeapon() == null) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"Successful hit on [{ev.Defender}] with [{ev.Weapon}] equipped in the main hand.");
      var amount = Math.Round(percentage / new Decimal(100), 2);
      var singleWeapon = true;
      The.Player.ForeachEquippedObject(obj => {
        if (singleWeapon && obj.EquippedOn().ThisPartWeapon() != null && !obj.IsEquippedOnPrimary())
          singleWeapon = false;
      });

      // Main hand weapon skill
      if (ev.Weapon.IsEquippedInMainHand())
        Req.PointTracker.AddPoints(skill, amount);
      // Single / offhand weapon
      if (singleWeapon) {
        Req.PointTracker.AddPoints(SkillClasses.SingleWeaponFighting, amount / 2);
      } else if (!ev.Weapon.IsEquippedOnPrimary()) {
        Req.PointTracker.AddPoints(SkillClasses.MultiweaponFighting, amount * 2);
      }

      return base.HandleEvent(ev);
    }
  }
}