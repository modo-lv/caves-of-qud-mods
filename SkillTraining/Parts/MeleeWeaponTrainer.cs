using System;
using System.Collections.Generic;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;
using XRL.World.Skills;

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains melee weapon skills.</summary>
  /// <remarks>
  /// Gets attached to the target object to validate successful hits and increase training as appropriate.
  /// </remarks>
  public class MeleeWeaponTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { DefendMeleeHitEvent.ID };

    /// <summary>
    /// Handle getting hit (before any damage calculations) and increase the training points accordingly.
    /// </summary>
    public override Boolean HandleEvent(DefendMeleeHitEvent ev) {
      var skill = SkillUtils.SkillOrPower(ev.Weapon.GetWeaponSkill())?.Class;

      if (Req.Player.HasSkill(skill)) {
        Req.TrainingTracker.RemoveSkill(skill);
        return base.HandleEvent(ev);
      }
      if (ev.Attacker != Req.Player
          || skill == null
          || ModOptions.MeleeTrainingRate <= 0
          // Only equipped weapons train skills
          || ev.Weapon.EquippedOn()?.ThisPartWeapon() == null) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"[{ev.Defender}] hit with [{ev.Weapon}].");
      var singleWeapon = true;
      The.Player.ForeachEquippedObject(obj => {
        if (singleWeapon && obj.EquippedOn().ThisPartWeapon() != null && !obj.IsEquippedOnPrimary())
          singleWeapon = false;
      });

      // Main hand weapon skill
      if (ev.Weapon.IsEquippedInMainHand())
        Req.TrainingTracker.AddPoints(skill, ModOptions.MeleeTrainingRate);
      // Single / offhand weapon
      if (singleWeapon) {
        Req.TrainingTracker.AddPoints(
          SkillClasses.SingleWeaponFighting,
          Math.Round(ModOptions.MeleeTrainingRate / 2, 2)
        );
      } else if (!ev.Weapon.IsEquippedOnPrimary()) {
        Req.TrainingTracker.AddPoints(
          SkillClasses.MultiweaponFighting,
          Math.Round(ModOptions.MeleeTrainingRate * 2, 2)
        );
      }

      return base.HandleEvent(ev);
    }
  }
}