using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  public class AcrobaticsTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      // Melee
      obj.RegisterPartEvent(this, QudEventNames.DefenderAfterAttackMissed);
      // Missiles
      obj.RegisterPartEvent(this, QudEventNames.WeaponGetDefenderDV);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      // Melee
      if (ev.ID == QudEventNames.DefenderAfterAttackMissed)
        ev.Defender().Training()?.HandleTrainingAction(PlayerAction.DodgeMelee);

      // Missiles
      // Technically the defender DV detection event gets sent to the projectile after defender,
      // but at least in the vanilla game projectiles don't seem to be using it do modify the DV,
      // so this should work well enough.
      // The whole approach is fragile, but without a reliable `DefenderMissileMissed` event,
      // this is the only way.
      if (ev.ID == QudEventNames.WeaponGetDefenderDV && ev.Defender().CanTrainSkills()) {
        var dv = ev.GetIntParameter("DV");
        if (ev.Defender()?.HasSkill(QudSkillClasses.SwiftReflexes) == false)
          dv -= 5;
        if (ev.Defender()?.IsMobile() == false)
          dv = -100;
        if (ev.GetIntParameter("Result") <= dv) {
          ev.Defender().Training()?.HandleTrainingAction(PlayerAction.DodgeMissile);
        }
      }

      return base.FireEvent(ev);
    }
  }
}