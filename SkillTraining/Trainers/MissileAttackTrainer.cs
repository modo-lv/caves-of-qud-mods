using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains missile weapon skills.</summary>
  /// <remarks>
  /// Attached first to the player to listen for missile attack start.
  /// Once a missile attack is started, also attaches to the intended target,
  /// to listen for missile hit event.
  /// </remarks>
  public class MissileAttackTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      BeforeFireMissileWeaponsEvent.ID,
      DefenderMissileHitEvent.ID,
    };

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget.IsCombatant() && ev.Actor.CanTrainSkills())
        ev.ApparentTarget.RequirePart<MissileAttackTrainer>();
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      // No launcher means hit with a thrown weapon, which is a separate trainer
      if (ev.Launcher == null)
        return base.HandleEvent(ev);
     
      if (this.ParentObject != ev.Defender)
        throw new Exception($"Defender missile hit event fired on [{this.ParentObject}], " +
                            $"but defender is [{ev.Defender}].");
      
      var skillClass = ev.Launcher.GetPart<MissileWeapon>().Skill;
      var action = skillClass switch {
        // AFAICT there are no weapons using "Bow" skill anymore, but the game has checks for it.
        "Bow" => PlayerAction.BowOrRifleHit,
        SkillClasses.Pistol => PlayerAction.PistolHit,
        "Rifle" => PlayerAction.BowOrRifleHit,
        SkillClasses.HeavyWeapon => PlayerAction.HeavyWeaponHit,
        _ => throw new Exception($"Unknown missile weapon skill: [{skillClass}].")
      };
      if (skillClass is "Rifle" or "Bow")
        skillClass = SkillClasses.BowAndRifle;
      var skill = SkillUtils.SkillByClass(skillClass)?.Class;

      if (skill != null
          && ev.Attacker.CanTrainSkills()
          && ev.AimedAt == ev.Defender
          && ev.Defender.IsCombatant()
         ) {
        ev.Attacker.TrainingTracker()?.HandleTrainingAction(action);
      }

      return base.HandleEvent(ev);
    }
  }
}