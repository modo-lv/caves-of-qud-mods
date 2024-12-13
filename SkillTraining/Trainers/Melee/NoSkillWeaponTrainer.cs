using System;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Melee {
  public class NoSkillWeaponTrainer : IMeleeWeaponTrainer {
    public override String? WeaponSkill => null;

    public override void HandleAfterAttack(
      GameObject attacker, 
      GameObject defender, 
      GameObject weapon,
      Boolean isCritical,
      Boolean isOffhand, 
      Boolean isSingle
      ) {
      base.HandleAfterAttack(attacker, defender, weapon, isCritical, isOffhand, isSingle);
    }
  }
}