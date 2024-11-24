using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public class TinkeringTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      ExamineSuccessEvent.ID,
    };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      if (ev.Actor.IsPlayer()) {
        Req.PointTracker.AddPoints(SkillClasses.Tinkering, ModOptions.TinkeringTrainingRate);
      } 
      return base.HandleEvent(ev);
    }
  }
}