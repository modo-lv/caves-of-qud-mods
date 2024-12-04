namespace ModoMods.SkillTraining.Data {
  /// <summary>Player actions used in training skills.</summary>
  public enum PlayerAction {
    DodgeMelee,
    
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
    #endregion
    
    #region Self-Discipline
    RecoverTerror,
    RecoverConfusion,
    #endregion
    
    ShieldBlock,
    
    Swim,
    
    ThrownWeaponHit,
    
    TradeItem,
    
    #region Wayfaring
    RegainBearings,
    WorldMapMove,
    #endregion
  }
}