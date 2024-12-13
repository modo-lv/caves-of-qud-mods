using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  /// <summary>Base class for ranged weapon skill trainers.</summary>
  public abstract class IMissileWeaponTrainer : ModPart {
    /// <summary>Called for every shot that is eligible for training.</summary>
    public abstract void HandleDefenderHit(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Decimal modifier,
      Boolean isCritical
    );
    
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      BeforeFireMissileWeaponsEvent.ID,
    };
    
    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget.IsCombatant() && ev.Actor == this.ParentObject && ev.Actor.CanTrainSkills())
        ev.ApparentTarget.RequirePart<DefenderMissileHitListener>();
      return base.HandleEvent(ev);
    }
  }
}