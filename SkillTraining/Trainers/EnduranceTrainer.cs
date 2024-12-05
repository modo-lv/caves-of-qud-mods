using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Endurance skills.</summary>
  public class EnduranceTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EnterCellEvent.ID,
      EndTurnEvent.ID,
    };

    public override Boolean HandleEvent(EnterCellEvent ev) {
      if (ev.Object.HasEffect<Swimming>())
        ev.Object.TrainingTracker()?.HandleTrainingAction(PlayerAction.Swim);
      if (ev.Object.HasEffect<Running>())
        ev.Object.TrainingTracker()?.HandleTrainingAction(PlayerAction.Sprinting);
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ParentObject.HasEffect<Dazed>())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.SufferDaze);
      if (this.ParentObject.HasEffect<Stun>())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.SufferStun);
      if (this.ParentObject.HasEffect<Poisoned>())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.SufferPoison);
      return base.HandleEvent(ev);
    }
  }
}