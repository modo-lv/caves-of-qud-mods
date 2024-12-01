using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Self-Discipline skills.</summary>
  public class SelfDisciplineTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EffectRemovedEvent.ID };

    /// <summary>Handles recovering from any negative health effect.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      switch (ev.Effect) {
        case Terrified:
          this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.RecoverTerror);
          break;
        case Confused:
          this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.RecoverConfusion);
          break;
      }
      return base.HandleEvent(ev);
    }
  }
}