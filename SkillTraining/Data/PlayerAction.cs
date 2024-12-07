namespace ModoMods.SkillTraining.Data {
  /// <summary>Player actions used in training skills.</summary>
  public enum PlayerAction {
    DodgeMelee,
    DodgeMissile,
    
    #region Endurance
    Sprinting,
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
    RegainBearings,
    WorldMapMove,
    #endregion
  }
}