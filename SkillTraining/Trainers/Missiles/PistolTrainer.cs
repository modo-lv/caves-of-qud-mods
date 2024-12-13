using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;
using static ModoMods.Core.Data.QudSkillClasses;
using static ModoMods.SkillTraining.Data.PlayerAction;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  public class PistolTrainer : IMissileWeaponTrainer {
    public override ISet<Int32> WantEventIds => base.WantEventIds.Concat(new[] {
      BeforeFireMissileWeaponsEvent.ID,
      EnteredCellEvent.ID,
    }).ToHashSet();

    /// <summary>Has the shooter moved while sprinting since the last shot?</summary>
    public Boolean SprintMoved;

    /// <summary>Detects sprint-moving.</summary>
    public override Boolean HandleEvent(EnteredCellEvent ev) {
      if (ev.Actor == this.ParentObject && ev.Actor.HasEffect<Running>())
        this.SprintMoved = true;
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      ev.MissileWeapons.ForEach(mw => mw.ParentObject.RequirePart<ShotCompleteTracker>());
      return base.HandleEvent(ev);
    }

    /// <summary>Clears sprint-run flag after a shot has been fired.</summary>
    public class ShotCompleteTracker : ModPart {
      public override ISet<Int32> WantEventIds => new HashSet<Int32> { ShotCompleteEvent.ID };
      public override Boolean HandleEvent(ShotCompleteEvent ev) {
        if (ev.Actor.TryGetPart<PistolTrainer>(out var pistol))
          pistol.SprintMoved = false;
        return base.HandleEvent(ev);
      }
    }

    public override void HandleDefenderHit(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Decimal modifier,
      Boolean isCritical
    ) {
      var critMultiplier = modifier * (isCritical ? 2 : 1);

      // Sprinting
      if (attacker.HasEffect<Running>() && this.SprintMoved)
        attacker.Training()?.HandleTrainingAction(SprintingPistolHit, critMultiplier);

      // Critical
      if (isCritical)
        attacker.Training()?.HandleTrainingAction(PistolNativeCrit, modifier);

      // Multiple one-handed weapons (pistols) equipped
      Boolean skilled;
      if (attacker.GetMissileWeapons(w => !w.GetPart<Physics>().UsesTwoSlots)?.Count > 1) {
        attacker.Training()?.HandleTrainingAction(AlternatePistolHit, critMultiplier);
        skilled = attacker.HasSkill(Akimbo);
      } else {
        attacker.Training()?.HandleTrainingAction(PistolHit, critMultiplier);
        skilled = attacker.HasSkill(Pistol);
      }

      if (skilled)
        attacker.Training()?.HandleTrainingAction(
          attacker.HasSkill(EmptyClips) ? PistolFastertHit : PistolFastHit,
          critMultiplier
        );
    }
  }
}