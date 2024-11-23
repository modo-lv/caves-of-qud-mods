using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;
using XRL.World.Effects;

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains cooking and gathering skill.</summary>
  /// <remarks>
  /// Gets attached to the player to track cooking events and increase training as appropriate.
  /// </remarks>
  public class CookingTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> {
      EffectAppliedEvent.ID
    };

    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.CookedAt);
    }

    public override Boolean HandleEvent(EffectAppliedEvent ev) {
      // "Tasty" random meals don't trigger the "CookedAt" event, and must be detected by their effects
      if (ev.Effect.GetType().IsSubclassOf(typeof(BasicCookingEffect)))
        IncreasePoints(multiplier: 2);
      return base.HandleEvent(ev);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID == EventNames.CookedAt)
        IncreasePoints();
      return base.FireEvent(ev);
    }

    public static void IncreasePoints(Byte multiplier = 1) {
      Output.DebugLog("Meal consumed.");
      Req.TrainingTracker.AddPoints(
        SkillClasses.CookingAndGathering,
        ModOptions.CookingTrainingRate * multiplier
      );
    }
  }
}