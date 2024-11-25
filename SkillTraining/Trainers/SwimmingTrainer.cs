using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Effects;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains Swimming skill.</summary>
  /// <remarks>
  /// Attached to the player and adds training points at the end of every swimming movement.
  /// </remarks>
  public class SwimmingTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { EnteredCellEvent.ID };

    public override Boolean HandleEvent(EnteredCellEvent ev) {
      if (ev.Object.IsPlayer()
          && !Main.Player.OnWorldMap()
          && !Main.Player.HasSkill(SkillClasses.Swimming)
          && Main.Player.HasEffect<Swimming>()) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.Swim);
      }
      return base.HandleEvent(ev);
    }
  }
}