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
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EndTurnEvent.ID,
      EnterCellEvent.ID,
    };

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ParentObject.HasEffect<Confused>())
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.SufferConfusion);
      if (this.ParentObject.HasEffect<Terrified>())
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.SufferTerror);
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(EnterCellEvent ev) {
      if (this.ParentObject.HasEffect<Running>())
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.DisciplineSprint);
      return base.HandleEvent(ev);
    }
  }
}