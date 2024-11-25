using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  public class TinkeringTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { ExamineSuccessEvent.ID, };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      if (ev.Actor.IsPlayer()) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.ExamineSuccess);
      } 
      return base.HandleEvent(ev);
    }
  }
}