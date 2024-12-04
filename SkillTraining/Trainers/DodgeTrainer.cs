using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  public class DodgeTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, QudEventNames.DefenderAfterAttackMissed);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID == QudEventNames.DefenderAfterAttackMissed)
        ev.Defender().TrainingTracker()?.HandleTrainingAction(PlayerAction.DodgeMelee);
      return base.FireEvent(ev);
    }
  }
}