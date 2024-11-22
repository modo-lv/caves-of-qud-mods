using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public class ThrownAttackTracker : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"[{nameof(ThrownAttackTracker)}] created and added to [{this.ParentObject}].");
    }

    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      obj.RegisterPartEvent(this, EventNames.TakeDamage);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID == EventNames.BeforeThrown) {
        // Attack this tracker to the target creature, to detect when it gets hit.
        var target = ev.GetParameter("ApparentTarget") as GameObject;
        if (target?.IsCreature == true) {
          target.RequirePart<ThrownAttackTracker>();
        }
      } else if (ev.ID == EventNames.TakeDamage
                 || ev.GetParameter("Attacker") == Req.Player
                 || (ev.GetParameter("Defender") as GameObject)?.IsCreature == true) {
        // Taking damage means the hit was successful.
        Output.DebugLog($"Thrown hit on [{ev.GetParameter("Defender")}] successful.");
        Req.Player.RequirePart<PointTracker>().AddPoints(
          SkillClasses.DeftThrowing,
          ModOptions.ThrownTrainingPercentage / new Decimal(100)
        );
      }

      return base.FireEvent(ev);
    }
  }
}