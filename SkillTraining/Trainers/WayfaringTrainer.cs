using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Wayfaring" skill.</summary>
  public class WayfaringTrainer : ModPart {
    /// <summary>Number of turns spent travelling the world map.</summary>
    private Int32 _travelTurns;

    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EndTurnEvent.ID,
      EffectRemovedEvent.ID,
    };

    /// <summary>Travel.</summary>
    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ParentObject.OnWorldMap() && this.ParentObject.CanTrainSkills()) {
        this._travelTurns = (this._travelTurns + 1) % 300;
        if (this._travelTurns == 0)
          this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.WorldMapMove);
      }
      return base.HandleEvent(ev);
    }
    
    /// <summary>Regain bearings.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Effect is Lost && this.ParentObject.CanTrainSkills())
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.RegainBearings);
      return base.HandleEvent(ev);
    }
  }
}