using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  /// <remarks>Trains Bow and Rifle skills.</remarks>
  public class BowAndRifleTrainer : IMissileWeaponTrainer {
    public override void HandleDefenderHit(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Decimal modifier,
      Boolean isCritical
    ) {
      var critModifier = modifier * (isCritical ? 2 : 1);
      if (defender.HasEffect<RifleMark>()) {
        attacker.Training()?.HandleTrainingAction(
          action: isCritical ? PlayerAction.MarkedCriticalHit : PlayerAction.MarkedHit,
          amountModifier: modifier
        );
      } else {
        attacker.Training()?.HandleTrainingAction(PlayerAction.BowOrRifleHit, critModifier);
      }
    }
  }
}