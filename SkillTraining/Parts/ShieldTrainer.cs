using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public class ShieldTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      AfterShieldBlockEvent.ID,
    };

    public override Boolean HandleEvent(AfterShieldBlockEvent ev) {
      if (ev.Defender.IsPlayer()) {
        Req.TrainingTracker.AddPoints(SkillClasses.Shield, ModOptions.ShieldOilerTrainingRate);
      }
      return base.HandleEvent(ev);
    }
  }
}