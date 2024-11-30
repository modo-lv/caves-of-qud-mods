using System;
using System.Collections.Generic;
using ModoMods.SkillTraining.Utils;
using static ModoMods.SkillTraining.Data.PlayerAction;
using static ModoMods.SkillTraining.Data.SkillClasses;

namespace ModoMods.SkillTraining.Data {
  public struct TrainingData {
    public readonly String SkillClass;
    public readonly Decimal DefaultAmount;

    public TrainingData(String skillClass, Decimal defaultAmount) {
      this.SkillClass = skillClass;
      this.DefaultAmount = defaultAmount;
    }

    public static readonly IDictionary<PlayerAction, TrainingData> Data =
      new Dictionary<PlayerAction, TrainingData> {
        //@formatter:off
        
        #region Melee combat
        { AxeHit,            new TrainingData(Axe,                 0.05m) },
        { CudgelHit,         new TrainingData(Cudgel,              0.05m) },
        { LongBladeHit,      new TrainingData(LongBlade,           0.10m) },
        { ShortBladeHit,     new TrainingData(ShortBlade,          0.10m) },
         
        { SingleWeaponHit,   new TrainingData(SingleWeaponFighting,0.02m) },
        { OffhandWeaponHit,  new TrainingData(MultiweaponFighting, 0.05m) },
        #endregion
        
        #region Missile combat
        { BowOrRifleHit,     new TrainingData(BowAndRifle,         0.50m) },
        { PistolHit,         new TrainingData(Pistol,              0.50m) },
        { HeavyWeaponHit,    new TrainingData(HeavyWeapon,         0.50m) },
        #endregion
        
        #region Cooking and Gathering
        { Cook,              new TrainingData(CookingAndGathering, 0.33m) },
        { CookTasty,         new TrainingData(CookingAndGathering, 0.66m) },
        { Harvest,           new TrainingData(CookingAndGathering, 0.05m) },
        { Butcher,           new TrainingData(CookingAndGathering, 0.50m) },
        #endregion
        
        #region Customs and Folklore
        { FirstRitualRep,    new TrainingData(CustomsAndFolklore,  1.00m) },
        { RitualRep,         new TrainingData(CustomsAndFolklore,  0.25m) },
        { SecretReveal,      new TrainingData(CustomsAndFolklore,  0.25m) },
        #endregion 
 
        { Bandage,           new TrainingData(Physic,              1.00m) },
         
        #region Tinkering 
        { ExamineSuccess,    new TrainingData(Tinkering,           1.00m) },
        { RifleTrashSuccess, new TrainingData(Tinkering,           0.25m) },
        #endregion 
         
        { ShieldBlock,       new TrainingData(Shield,              0.25m) },
         
        { ThrownWeaponHit,   new TrainingData(DeftThrowing,        1.00m) },
         
        { TradeItem,         new TrainingData(SnakeOiler,          0.01m) },
         
        { Swim,              new TrainingData(Swimming,            0.15m) },

        #region Wayfaring
        { WorldMapMove,      new TrainingData(Wayfaring,           0.15m) },
        { RegainBearings,    new TrainingData(Wayfaring,           1.00m) },
        #endregion
        
        //@formatter:on
      };


    public static TrainingData For(PlayerAction action) =>
      Data.GetOr(action, () =>
        throw new Exception($"No training data for player action [{action}].")
      );
  }
}