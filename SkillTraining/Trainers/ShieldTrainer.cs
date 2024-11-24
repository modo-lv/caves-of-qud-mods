using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  public class ShieldTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      AfterShieldBlockEvent.ID,
    };

    public override Boolean HandleEvent(AfterShieldBlockEvent ev) {
      if (ev.Defender.IsPlayer()) {
        Main.PointTracker.AddPoints(SkillClasses.Shield, Settings.ShieldOilerTrainingRate);
      }
      return base.HandleEvent(ev);
    }
  }
}