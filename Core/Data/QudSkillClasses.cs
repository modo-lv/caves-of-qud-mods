using System;

namespace ModoMods.Core.Data {
  /// <summary>Known skill class names.</summary>
  public static class QudSkillClasses {
    // Special case: same skill in both axe and cudgel trees.
    public const String ChargingStrike = "Cudgel_ChargingStrike";
    
    // Acrobatics tree
    public const String Acrobatics = "Acrobatics";
    public const String SwiftReflexes = "Acrobatics_SwiftReflexes";
    public const String Spry = "Acrobatics_Dodge";
    
    // Axe tree
    public const String Axe = "Axe";
    public const String AxeCleave = "Axe_Cleave";

    // Bow and rifle tree
    public const String BowAndRifle = "Rifles"; // 100

    // Cooking tree
    public const String CookingAndGathering = "CookingAndGathering";
    public const String Spicer = "CookingAndGathering_Spicer";

    // Customs and folklore tree
    public const String CustomsAndFolklore = "Customs"; // 150
    public const String Tactful = "Customs_Tactful";
    public const String TrashDivining = "Customs_TrashDivining"; // 150

    // Cudgel tree
    public const String Cudgel = "Cudgel";
    
    // Endurance tree
    public const String Endurance = "Endurance";
    public const String Longstrider = "Endurance_Longstrider";
    public const String Swimming = "Endurance_Swimming";
    public const String PoisonTolerance = "Endurance_PoisonTolerance";

    // Heavy Weapon tree
    public const String HeavyWeapon = "HeavyWeapons"; // 100

    // Multiweapon tree
    public const String MultiweaponFighting = "Multiweapon_Fighting"; // 150

    // Long Blade tree
    public const String LongBlade = "LongBlades";
    
    // Persuasion tree
    public const String SnakeOiler = "Persuasion_SnakeOiler";

    // Pistol tree
    public const String Pistol = "Pistol"; // 100
    
    // Physic tree
    public const String Physic = "Physic"; // 50
    
    // Self-Discipline tree
    public const String FastingWay = "Discipline_FastingWay";
    public const String Lionheart = "Discipline_Lionheart";
    public const String IronMind = "Discipline_IronMind";
    public const String Conatus = "Discipline_Conatus";

    // Shield tree
    public const String Shield = "Shield";

    // Short Blade tree
    public const String ShortBlade = "ShortBlades";

    // Single weapon tree
    public const String SingleWeaponFighting = "SingleWeaponFighting";

    // Tactics tree
    public const String Tactics = "Tactics";
    public const String DeftThrowing = "Tactics_Throwing";
    
    // Tinkering tree
    public const String Tinkering = "Tinkering"; // 100
    
    // Wayfaring tree
    public const String Wayfaring = "Survival"; // 100
  }
}