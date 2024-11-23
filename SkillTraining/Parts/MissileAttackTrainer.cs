using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Parts;

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains missile weapon skills.</summary>
  /// <remarks>
  /// Gets attached to the target object to validate successful hits and increase training as appropriate.
  /// </remarks>
  public class MissileAttackTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { DefenderMissileHitEvent.ID };

    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      var skillClass = ev.Launcher.GetPart<MissileWeapon>().Skill;
      if (skillClass is "Rifle" or "Bow")
        skillClass = SkillClasses.BowAndRifle;
      var skill = SkillUtils.SkillByClass(skillClass)?.Class;

      if (Req.Player.HasSkill(skill)
          || skill == null
          || !ev.Defender.IsCreature
          || !ev.Attacker.IsPlayer()
          || ev.AimedAt != this.ParentObject) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"[{this.ParentObject}] hit with [{ev.Launcher}].");
      Req.TrainingTracker.AddPoints(skill, ModOptions.MissileTrainingRate);

      return base.HandleEvent(ev);
    }
  }
}