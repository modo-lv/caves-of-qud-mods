using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  public class TinkeringTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      ExamineSuccessEvent.ID,
    };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      if (ev.Actor.IsPlayer()) {
        Main.PointTracker.AddPoints(SkillClasses.Tinkering, Settings.TinkeringTrainingRate);
      } 
      return base.HandleEvent(ev);
    }
  }
}