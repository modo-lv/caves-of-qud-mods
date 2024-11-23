using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains thrown weapon skill.</summary>
  /// <remarks>
  /// Gets attached to the throwing weapon to listen to its thrown event, and when that triggers,
  /// attach to the target object to validate successful hits and increase training as appropriate.
  /// </remarks>
  public class ThrownAttackTracker : ModPart {
    public GameObject? Weapon;
    
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      obj.RegisterPartEvent(this, EventNames.TakeDamage);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      switch (ev.ID) {
        case EventNames.BeforeThrown: {
          // Attack this tracker to the target creature, to detect when it gets hit.
          var target = ev.GetParameter("ApparentTarget") as GameObject;
          if (target?.IsCreature == true)
            target.RequirePart<ThrownAttackTracker>().Weapon = this.ParentObject;
          break;
        }
        case EventNames.TakeDamage // Taking damage means the hit was successful.
          when !Req.Player.HasSkill(SkillClasses.DeftThrowing)
               && ev.GetParameter("Attacker") == Req.Player
               && (ev.GetParameter("Defender") as GameObject)?.IsCreature == true:
          Output.DebugLog($"[{ev.GetParameter("Defender")}] hit with [{this.Weapon}].");
          Req.TrainingTracker.AddPoints(SkillClasses.DeftThrowing, ModOptions.ThrownTrainingRate);
          break;
      }

      return base.FireEvent(ev);
    }
  }
}