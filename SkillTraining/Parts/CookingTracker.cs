using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains cooking and gathering skill.</summary>
  /// <remarks>
  /// Gets attached to the player to track cooking events and increase training as appropriate.
  /// </remarks>
  public class CookingTracker : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.CookedAt);
    }

    public override Boolean FireEvent(Event ev) {
      Req.TrainingTracker.AddPoints(SkillClasses.CookingAndGathering, ModOptions.CookingTrainingRate);
      return base.FireEvent(ev);
    }
  }
}