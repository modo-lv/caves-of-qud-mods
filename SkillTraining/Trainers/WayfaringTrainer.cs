using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Effects;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains "Wayfaring" skill.</summary>
  public class WayfaringTrainer : ModPart {
    /// <summary>Number of turns spent travelling the world map.</summary>
    private Int32 _travelTurns;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      EndTurnEvent.ID,
      EffectRemovedEvent.ID,
    };

    /// <summary>Travel.</summary>
    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (!Main.Player.OnWorldMap()) return base.HandleEvent(ev);
      this._travelTurns = (this._travelTurns + 1) % 300;
      if (this._travelTurns == 0)
        Main.PointTracker.HandleTrainingAction(PlayerAction.WorldMapMove);
      return base.HandleEvent(ev);
    }
    
    /// <summary>Regain bearings.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Actor.IsPlayer() && ev.Effect.GetType().IsSubclassOf(typeof(Lost)))
        Main.PointTracker.HandleTrainingAction(PlayerAction.RegainBearings);
      return base.HandleEvent(ev);
    }
  }
}