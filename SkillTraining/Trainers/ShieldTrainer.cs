using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Shield skill.</summary>
  public class ShieldTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { AfterShieldBlockEvent.ID };

    public override Boolean HandleEvent(AfterShieldBlockEvent ev) {
      if (!ev.Defender.IsPlayer()) return base.HandleEvent(ev);
      
      Main.PointTracker.HandleTrainingAction(PlayerAction.ShieldBlock);
      return base.HandleEvent(ev);
    }
  }
}