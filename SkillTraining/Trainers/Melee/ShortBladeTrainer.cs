using System;
using ModoMods.Core.Data;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Melee {
  public class ShortBladeTrainer : IMeleeWeaponTrainer {
    public override String? WeaponSkill => QudSkillClasses.ShortBlade;

    public override void HandleAfterAttack(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Boolean isCritical,
      Boolean isOffhand,
      Boolean isSingle
    ) {
      attacker.Training()?.HandleTrainingAction(PlayerAction.ShortHit, isCritical ? 2 : 1);
      base.HandleAfterAttack(attacker, defender, weapon, isCritical, isOffhand, isSingle);
    }
  }
}