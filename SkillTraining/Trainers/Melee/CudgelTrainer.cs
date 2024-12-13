using System;
using ModoMods.Core.Data;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Melee {
  public class CudgelTrainer : IMeleeWeaponTrainer {
    public override String? WeaponSkill => QudSkillClasses.Cudgel;

    public override void HandleAfterAttack(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Boolean isCritical,
      Boolean isOffhand, 
      Boolean isSingle
    ) {
      attacker.Training()?.HandleTrainingAction(PlayerAction.CudgelHit, isCritical ? 2 : 1);
      base.HandleAfterAttack(attacker, defender, weapon, isCritical, isOffhand, isSingle);
    }
  }
}