namespace ModoMods.SkillTraining.Data {
  /// <summary>Player actions that train skills.</summary>
  public enum PlayerAction {
    #region Acrobatics
    DodgeMelee,
    DodgeMissile,
    #endregion
    
    #region Axe
    Cleave,
    #endregion
    
    #region Endurance
    EnduranceSprint,
    SufferStun,
    SufferDaze,
    SufferPoison,
    Swim,
    #endregion
    
    #region Melee weapons
    AxeHit,
    CudgelHit,
    LongBladeHit,
    ShortBladeHit,
    SingleWeaponHit,
    OffhandWeaponHit,
    #endregion
    
    #region Missile combat
    BowOrRifleHit,
    PistolHit,
    HeavyWeaponHit,
    #endregion
    
    #region Cooking and Gathering
    Cook,
    /// <summary>Meal cooked with manually selected ingredients (no recipe).</summary>
    CookIngredients,
    CookTasty,
    Harvest,
    Butcher,
    #endregion
    
    #region Customs and Folklore
    RitualFirstRep,
    RitualRep,
    JournalReveal,
    #endregion
    
    #region Physic
    Bandage,
    Recover,
    Inject,
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