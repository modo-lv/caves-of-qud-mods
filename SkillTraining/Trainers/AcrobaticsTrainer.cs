using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Skill;
using static ModoMods.Core.Data.QudEventNames;

namespace ModoMods.SkillTraining.Trainers {
  public class AcrobaticsTrainer : ModPart {
    public override ISet<String> RegisterEventIds => new HashSet<String> {
      DefenderAfterAttackMissed, // Melee
      WeaponGetDefenderDV, // Missiles
      BeforeCooldownActivatedAbility, // Juke
    };

    public override Boolean FireEvent(Event ev) {
      if (ev.ID == BeforeCooldownActivatedAbility) {
        var ability = ev.GetParameter<ActivatedAbilityEntry>("AbilityEntry");
        if (ability?.Command == Tactics_Juke.COMMAND_NAME)
          this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.Juked);
      }

      // Melee
      if (ev.ID == DefenderAfterAttackMissed)
        ev.Defender().Training()?.HandleTrainingAction(PlayerAction.DodgeMelee);

      // Missiles
      // The defender DV detection event gets sent to the projectile as well (after defender),
      // but at least in the vanilla game projectiles don't seem to be using it do modify the DV,
      // so this should work well enough.
      // The whole approach is fragile, but without a reliable `DefenderMissileMissed` event,
      // this is the only way.
      if (ev.ID == WeaponGetDefenderDV && ev.Defender().CanTrainSkills()) {
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