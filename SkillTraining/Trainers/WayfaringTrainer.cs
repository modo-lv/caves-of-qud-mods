using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Effects;

namespace Modo.SkillTraining.Trainers {
  public class WayfaringTrainer : ModPart {
    private Int32 _turnsTraveled;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      EndTurnEvent.ID,
      EffectRemovedEvent.ID,
    };

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (!Main.Player.OnWorldMap()) return base.HandleEvent(ev);
      
      this._turnsTraveled = (this._turnsTraveled + 1) % 300;
      if (this._turnsTraveled == 0) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.WorldMapMove);
      }
      return base.HandleEvent(ev);
    }
    
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Effect.GetEffectType() == new Lost().GetEffectType()) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.RegainBearings);
      }
      return base.HandleEvent(ev);
    }
  }
}