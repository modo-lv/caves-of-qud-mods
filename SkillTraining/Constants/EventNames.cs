using System;

namespace Modo.SkillTraining.Constants {
  public static class EventNames {
    /// <summary>Sent to the defender at the end of a melee attack.</summary>
    public const String DefenderAfterAttack = "DefenderAfterAttack";

    /// <summary>Fired on the thrown weapon at the start of throwing it.</summary>
    public const String BeforeThrown = "BeforeThrown";
    
    /// <summary>Fired on the defender when an attack hits.</summary>
    public const String TakeDamage = "TakeDamage";
  }
}