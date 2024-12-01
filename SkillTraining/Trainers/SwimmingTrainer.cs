using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Swimming skill.</summary>
  /// <remarks>
  /// Attached to the player and adds training points at the end of every swimming movement.
  /// </remarks>
  public class SwimmingTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EnteredCellEvent.ID };

    public override Boolean HandleEvent(EnteredCellEvent ev) {
      if (ev.Object.HasEffect<Swimming>() && ev.Object.CanTrainSkills())
        ev.Object.TrainingTracker()?.HandleTrainingAction(PlayerAction.Swim);
      return base.HandleEvent(ev);
    }
  }
}