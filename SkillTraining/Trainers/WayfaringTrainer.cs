using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
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
      if (!Main.Player.OnWorldMap() || Main.Player.HasEffect<Dominated>()) 
        return base.HandleEvent(ev);
      this._travelTurns = (this._travelTurns + 1) % 300;
      if (this._travelTurns == 0)
        Main.PointTracker.HandleTrainingAction(PlayerAction.WorldMapMove);
      return base.HandleEvent(ev);
    }
    
    /// <summary>Regain bearings.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Actor?.IsPlayer() == true
          && ev.Actor?.HasEffect<Dominated>() == false
          && ev.Effect?.GetType().IsSubclassOf(typeof(Lost)) == true)
        Main.PointTracker.HandleTrainingAction(PlayerAction.RegainBearings);
      return base.HandleEvent(ev);
    }
  }
}