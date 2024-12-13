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
    public const String Tumble = "Acrobatics_Tumble";
    
    // Axe tree
    public const String Axe = "Axe";
    public const String AxeCleave = "Axe_Cleave";

    // Bow and rifle tree
    public const String BowAndRifle = "Rifles"; // 100
    public const String SureFire = "Rifle_SureFire"; // 150
    public const String WoundingFire = "Rifle_WoundingFire"; // 150

    // Cooking tree
    public const String CookingAndGathering = "CookingAndGathering";
    public const String Spicer = "CookingAndGathering_Spicer";

    // Cudgel tree
    public const String Cudgel = "Cudgel";
    public const String Bludgeon = "Cudgel_Bludgeon"; // 150

    // Customs and folklore tree
    public const String CustomsAndFolklore = "Customs"; // 150
    public const String Tactful = "Customs_Tactful";
    public const String TrashDivining = "Customs_TrashDivining"; // 150
    
    // Endurance tree
    public const String Endurance = "Endurance";
    public const String Longstrider = "Endurance_Longstrider";
    public const String Swimming = "Endurance_Swimming";
    public const String PoisonTolerance = "Endurance_PoisonTolerance";
    public const String Weathered = "Endurance_Weathered";
    public const String Juicer = "Endurance_Juicer"; // 150

    // Heavy Weapon tree
    public const String HeavyWeapon = "HeavyWeapons"; // 100
    public const String Tank = "HeavyWeapons_Tank"; // 150

    // Multiweapon tree
    public const String MultiweaponFighting = "Multiweapon_Fighting"; // 150
    public const String MultiweaponExpertise = "Multiweapon_Expertise"; // 150
    public const String MultiweaponMastery = "Multiweapon_Mastery"; // 200

    // Long Blade tree
    public const String LongBlade = "LongBlades";
    public const String DuelingStance = "LongBladesDuelingStance";
    
    // Persuasion tree
    public const String SnakeOiler = "Persuasion_SnakeOiler";

    // Physic tree
    public const String Physic = "Physic"; // 50
    public const String Nostrums = "Physic_Nostrums"; // 100
    public const String Apothecary = "Physic_Apothecary"; // 100
    
    // Pistol tree
    public const String Pistol = "Pistol"; // 100
    public const String Akimbo = "Pistol_Akimbo"; // 150
    public const String WeakSpotter = "Pistol_WeakSpotter"; // 150
    public const String SlingAndRun = "Pistol_SlingAndRun"; // 50
    public const String DeadShot = "Pistol_DeadShot"; // 200
    public const String EmptyClips = "Pistol_EmptyTheClips"; // 300
    public const String FastestGun = "Pistol_FastestGun"; // 400
    
    // Self-Discipline tree
    public const String FastingWay = "Discipline_FastingWay";
    public const String Lionheart = "Discipline_Lionheart";
    public const String IronMind = "Discipline_IronMind";
    public const String Conatus = "Discipline_Conatus";

    // Shield tree
    public const String Shield = "Shield";
    public const String SwiftBlocking = "Shield_SwiftBlocking";
    public const String DeftBlocking = "Shield_DeftBlocking";
    public const String ShieldWall = "Shield_ShieldWall";
    
    // Short Blade tree
    public const String ShortBlade = "ShortBlades";
    public const String Jab = "ShortBlades_Jab";

    // Single weapon tree
    public const String SingleWeaponFighting = "SingleWeaponFighting";

    // Tactics tree
    public const String Tactics = "Tactics";
    public const String DeftThrowing = "Tactics_Throwing";
    public const String Juke = "Tactics_Juke";
    
    // Tinkering tree
    public const String Tinkering = "Tinkering"; // 100
    
    // Wayfaring tree
    public const String Wayfaring = "Survival"; // 100
  }
}