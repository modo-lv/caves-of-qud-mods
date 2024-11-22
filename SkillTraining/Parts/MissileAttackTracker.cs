using System;
using System.Collections.Generic;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Skills;

namespace Modo.SkillTraining.Parts {
  public class MissileAttackTracker : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"[{nameof(MissileAttackTracker)}] created and added to [{this.ParentObject}].");
    }

    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || id == DefenderMissileHitEvent.ID;

    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      var skillClass = ev.Launcher.GetPart<MissileWeapon>().Skill;
      if (skillClass is "Rifle" or "Bow")
        skillClass = SkillClasses.BowAndRifle;
      var skill = SkillUtils.SkillByClass(skillClass)?.Class;
      
      if (skill == null
          || !ev.Defender.IsCreature
          || !ev.Attacker.IsPlayer()
          || ev.AimedAt != this.ParentObject) {
        return base.HandleEvent(ev);
      }

      Output.DebugLog($"[{this.ParentObject}] hit with [{ev.Launcher}].");
      Req.PointTracker.AddPoints(
        skill,
        ModOptions.MissileTrainingPercentage / new Decimal(100)
      );

      return base.HandleEvent(ev);
    }
  }
}