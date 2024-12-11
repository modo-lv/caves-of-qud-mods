using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  /// <remarks>Trains Bow and Rifle skills.</remarks>
  public class BowAndRifleTrainer : IMissileWeaponTrainer {
    public override void HandleDefenderHit(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Decimal multiplier,
      Boolean isCritical
    ) {
     attacker.Training()?
       .HandleTrainingAction(PlayerAction.BowOrRifleHit, multiplier * (isCritical ? 2 : 1));
    }
  }
}