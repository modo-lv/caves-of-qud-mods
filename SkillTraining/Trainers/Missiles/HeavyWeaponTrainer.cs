using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using static ModoMods.SkillTraining.Data.PlayerAction;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  public class HeavyWeaponTrainer : IMissileWeaponTrainer {
    public override ISet<Int32> WantEventIds => base.WantEventIds.Concat(new[] {
      EnteredCellEvent.ID,
    }).ToHashSet();

    public override void HandleDefenderHit(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Decimal modifier,
      Boolean isCritical
    ) {
      attacker.Training()
        ?.HandleTrainingAction(HeavyWeaponHit, modifier * (isCritical ? 2 : 1));
    }

    /// <summary>Carrying a heavy weapon.</summary>
    public override Boolean HandleEvent(EnteredCellEvent ev) {
      if (ev.Actor == this.ParentObject) {
        if (ev.Actor.HasHeavyWeaponEquipped())
          ev.Actor.Training()?.HandleTrainingAction(CarryHeavyWeapon);
      }
      return base.HandleEvent(ev);
    }
  }
}