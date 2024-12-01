using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Shield skill.</summary>
  public class ShieldTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { AfterShieldBlockEvent.ID };

    public override Boolean HandleEvent(AfterShieldBlockEvent ev) {
      if (ev.Defender.CanTrainSkills())
        ev.Defender.TrainingTracker()?.HandleTrainingAction(PlayerAction.ShieldBlock);
      return base.HandleEvent(ev);
    }
  }
}