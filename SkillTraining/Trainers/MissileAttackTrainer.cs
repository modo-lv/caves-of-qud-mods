using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;
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
      if (ev.ApparentTarget?.IsCreature == true 
          && ev.ApparentTarget?.IsPlayer() == false
          && ev.Actor?.HasEffect<Dominated>() == false)
        ev.ApparentTarget.RequirePart<MissileAttackTrainer>();
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      if (ev.Launcher == null) // No launcher means hit with a thrown weapon, which is a separate trainer
      {
        return base.HandleEvent(ev);
      }
      var skillClass = ev.Launcher.GetPart<MissileWeapon>().Skill;
      var action = skillClass switch {
        "Bow" => PlayerAction.BowHit,
        SkillClasses.Pistol => PlayerAction.PistolHit,
        "Rifle" => PlayerAction.RifleHit,
        SkillClasses.HeavyWeapon => PlayerAction.HeavyWeaponHit,
        _ => throw new Exception($"Unknown missile weapon skill: [{skillClass}].")
      };
      if (skillClass is "Rifle" or "Bow")
        skillClass = SkillClasses.BowAndRifle;
      var skill = SkillUtils.SkillByClass(skillClass)?.Class;

      if (this.ParentObject != ev.Defender)
        throw new Exception($"Defender missile hit event fired on [{this.ParentObject}], " +
                            $"but defender is [{ev.Defender}].");

      if (skill == null
          || !this.ParentObject.IsCreature
          || !ev.Attacker.IsPlayer()
          || ev.Attacker?.HasEffect<Dominated>() != false
          || this.ParentObject.IsPlayer()
          || ev.AimedAt != this.ParentObject) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"[{this.ParentObject}] hit with [{ev.Launcher}].");
      Main.PointTracker.HandleTrainingAction(action);

      return base.HandleEvent(ev);
    }
  }
}