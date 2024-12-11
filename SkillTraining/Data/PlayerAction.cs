namespace ModoMods.SkillTraining.Data {
  /// <summary>Player actions that train skills.</summary>
  public enum PlayerAction {
    #region Acrobatics
    DodgeMelee,
    DodgeMissile,
    #endregion
    
    #region Axe
    Cleave,
    ChargedCleave,
    #endregion
    
    #region Cudgel
    ChargedStrike,
    #endregion
    
    #region Customs and Folklore
    RitualFirstRep,
    RitualRep,
    JournalReveal,
    RifleTrash,
    #endregion

    #region Endurance
    EnduranceSprint,
    SufferStun,
    SufferDaze,
    SufferPoison,
    Swim,
    EndurePoison,
    ExtremeTemp,
    Juicing,
    #endregion
    
    #region Long Blade
    LongBladeHit,
    StanceHit,
    #endregion
    
    #region Multiweapon Fighting
    Offhand,
    ProficientOffhand,
    ExpertOffhand,
    #endregion
    
    #region Melee weapons
    AxeHit,
    CudgelHit,
    ShortBladeHit,
    SingleWeaponHit,
    #endregion
    
    #region Missile weapons
    BowOrRifleHit,
    HeavyWeaponHit,
    CarryHeavyWeapon,
    #endregion
    
    #region Cooking and Gathering
    Cook,
    /// <summary>Meal cooked with manually selected ingredients (no recipe).</summary>
    CookIngredients,
    CookTasty,
    Harvest,
    Butcher,
    #endregion
    
    #region Physic
    Bandage,
    Recover,
    Inject,
    #endregion
    
    #region Pistol
    PistolHit,
    AlternatePistolHit,
    CriticalPistolHit,
    SprintingPistolHit,
    #endregion
    
    #region Tinkering
    ExamineSuccess,
    RifleTrashSuccess,
    DisassembleBit,
    #endregion
    
    #region Self-Discipline
    DisciplineSprint,
    SufferFamine,
    SufferTerror,
    SufferConfusion,
    #endregion
    
    ShieldBlock,
    
    #region Tactics
    DangerSprint,
    ThrownWeaponHit,
    #endregion
    
    TradeItem,
    
    #region Wayfaring
    SufferLost,
    RecoverLost,
    WorldMapMove,
    #endregion
  }
}