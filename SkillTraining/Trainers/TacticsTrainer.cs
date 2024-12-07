using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  public class TacticsTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EnterCellEvent.ID };

    public override Boolean HandleEvent(EnterCellEvent ev) {
      if (ev.Actor == this.ParentObject
          && ev.Actor.HasEffect<Running>() 
          && ev.Actor.AreViableHostilesAdjacent()
        ) {
        ev.Actor.TrainingTracker()?.HandleTrainingAction(PlayerAction.DangerSprint);
      }
      return base.HandleEvent(ev);
    }
  }
}