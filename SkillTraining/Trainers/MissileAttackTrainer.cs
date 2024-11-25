using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Parts;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains missile weapon skills.</summary>
  /// <remarks>
  /// Attached first to the player to listen for missile attack start.
  /// Once a missile attack is started, also attaches to the intended target,
  /// to listen for missile hit event.
  /// </remarks>
  public class MissileAttackTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      BeforeFireMissileWeaponsEvent.ID,
      DefenderMissileHitEvent.ID,
    };

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget?.IsCreature == true && ev.ApparentTarget?.IsPlayer() == false)
        ev.ApparentTarget.RequirePart<MissileAttackTrainer>();
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
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

      if (Main.Player.HasSkill(skill)
          || skill == null
          || !this.ParentObject.IsCreature
          || !ev.Attacker.IsPlayer()
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