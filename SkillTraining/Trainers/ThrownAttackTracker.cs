using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
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
          // Attach this tracker to the target creature, to detect when it gets hit.
          var target = ev.GetParameter("ApparentTarget") as GameObject;
          if (target?.IsCreature == true)
            target.RequirePart<ThrownAttackTracker>().Weapon = this.ParentObject;
          break;
        }
        case EventNames.TakeDamage // Taking damage means the hit was successful.
          when !Main.Player.HasSkill(SkillClasses.DeftThrowing)
               && ev.GetParameter("Attacker") == Main.Player
               && (ev.GetParameter("Defender") as GameObject)?.IsCreature == true:
          Output.DebugLog($"[{ev.GetParameter("Defender")}] hit with [{this.Weapon}].");
          Main.PointTracker.AddPoints(SkillClasses.DeftThrowing, Settings.ThrownTrainingRate);
          break;
      }

      return base.FireEvent(ev);
    }
  }
}