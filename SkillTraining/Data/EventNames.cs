using System;

namespace Modo.SkillTraining.Data {
  public static class EventNames {
    /// <summary>Fired on the attacker as soon as the attack is calculated to hit.</summary>
    public const String AttackerHit = "AttackerHit";

    /// <summary>Fired on a trader/container when items are moved to it, once for each stack.</summary>
    public const String CommandTakeObject = "CommandTakeObject";
    
    /// <summary>Fired on the thrown weapon at the start of throwing it.</summary>
    public const String BeforeThrown = "BeforeThrown";
    
    /// <summary>Fired on the defender when an attack hits.</summary>
    public const String TakeDamage = "TakeDamage";

    /// <summary>Fired on the player after successfully cooking and eating a meal.</summary>
    public const String CookedAt = "CookedAt";

    /// <summary>Fired on the player after a reputation change has occured.</summary>
    public const String ReputationChanged = "ReputationChanged";

    /// <summary>Fired on the player when moving items from inventory to trader or container.</summary>
    public const String CommandRemoveObject = "CommandRemoveObject";
  }
}