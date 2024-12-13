using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Melee {
  public class LongBladeTrainer : IMeleeWeaponTrainer {
    public override void HandleAfterAttack(GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Boolean isCritical, Boolean isOffhand, Boolean isSingle) {
      attacker.Training()?.HandleTrainingAction(PlayerAction.LongBladeHit, isCritical ? 2 : 1);
    }
  }
}