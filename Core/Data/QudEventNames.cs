using System;
using System.Diagnostics.CodeAnalysis;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.Core.Data {
  // ReSharper disable InconsistentNaming
  public static class QudEventNames {
    /// <summary>Fired on the attacker as soon as the attack is calculated to hit.</summary>
    public const String AttackerHit = "AttackerHit";

    /// <summary>Fired on the attacker (and defender) after an attack has successfully hit.</summary>
    /// <seealso cref="Combat.MeleeAttackWithWeaponInternal"/>
    public const String AttackerAfterAttack = "AttackerAfterAttack";

    /// <summary>Earliest event fired on the attacker during melee attack calculations.</summary>
    public const String AttackerGetDefenderDV = "AttackerGetDefenderDV"; 

    /// <summary>Fired on the player when dropping an item to the ground.</summary>
    public const String CommandDropObject = "CommandDropObject";
    
    /// <summary>Fired on the player when moving items from inventory to trader or container.</summary>
    public const String CommandRemoveObject = "CommandRemoveObject";
    
    /// <summary>Fired on a trader/container when items are moved to it, once for each stack.</summary>
    public const String CommandTakeObject = "CommandTakeObject";

    /// <summary>Fired on the defender just after a melee attack has missed.</summary>
    public const String DefenderAfterAttackMissed = "DefenderAfterAttackMissed";
    
    /// <summary>Fired on the defender when calculating whether a missile weapon attack will hit.</summary>
    public const String WeaponGetDefenderDV = "WeaponGetDefenderDV";
    
    /// <summary>Fired on the thrown weapon once it hits something.</summary>
    /// <seealso cref="GameObject.PerformThrow"/>
    public const String ThrownProjectileHit = "ThrownProjectileHit";
    
    /// <summary>Fired on the defender when an attack hits.</summary>
    public const String TakeDamage = "TakeDamage";

    /// <summary>Fired on the player after successfully cooking and eating a meal.</summary>
    public const String CookedAt = "CookedAt";

    /// <summary>Fired on the missile weapon when it hits something with an attack.</summary>
    public const String DefenderProjectileHit = "DefenderProjectileHit";

    /// <summary>Fired on the player after a reputation change has occured.</summary>
    public const String ReputationChanged = "ReputationChanged";
  }
}