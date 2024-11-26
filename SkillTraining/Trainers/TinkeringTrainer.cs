using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  public class TinkeringTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { ExamineSuccessEvent.ID, };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      if (ev.Actor.IsPlayer()) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.ExamineSuccess);
      } 
      return base.HandleEvent(ev);
    }
  }
}