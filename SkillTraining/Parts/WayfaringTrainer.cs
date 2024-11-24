using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Effects;

namespace Modo.SkillTraining.Parts {
  public class WayfaringTrainer : ModPart {
    private Int32 _turnsRemaining = 150;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      EndTurnEvent.ID,
      EffectRemovedEvent.ID,
    };

    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Effect.GetEffectType() == new Lost().GetEffectType()) {
        Req.PointTracker.AddPoints(
          SkillClasses.Wayfaring,
          ModOptions.WayfaringTrainingRate * 5
        );
      }
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (Req.Player.OnWorldMap()) {
        this._turnsRemaining--;
        if (this._turnsRemaining == 0) {
          this._turnsRemaining = 300;
          Req.PointTracker.AddPoints(SkillClasses.Wayfaring, ModOptions.WayfaringTrainingRate);
        }
      }
      return base.HandleEvent(ev);
    }
  }
}