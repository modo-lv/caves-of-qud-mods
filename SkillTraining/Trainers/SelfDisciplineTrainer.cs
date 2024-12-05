using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Self-Discipline skills.</summary>
  public class SelfDisciplineTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EndTurnEvent.ID };

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ParentObject.HasEffect<Confused>())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.SufferConfusion);
      if (this.ParentObject.HasEffect<Terrified>())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.SufferTerror);
      return base.HandleEvent(ev);
    }
  }
}