using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
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
        ev.Object.Training()?.HandleTrainingAction(PlayerAction.Swim);
      if (ev.Object.HasEffect<Running>())
        ev.Object.Training()?.HandleTrainingAction(PlayerAction.EnduranceSprint);
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ParentObject.HasEffect<Dazed>())
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.SufferDaze);
      if (this.ParentObject.HasEffect<Stun>())
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.SufferStun);
      if (this.ParentObject.HasEffect<Poisoned>()) {
        this.ParentObject.Training()?.HandleTrainingAction(
          this.ParentObject.HasSkill(QudSkillClasses.Endurance)
            ? PlayerAction.EndurePoison
            : PlayerAction.SufferPoison
        );
      }
      return base.HandleEvent(ev);
    }
  }
}