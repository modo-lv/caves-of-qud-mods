using System;
using System.Collections.Generic;
using Modo.SkillTraining.Utils;
using static Modo.SkillTraining.Data.PlayerAction;
using static Modo.SkillTraining.Data.SkillClasses;

namespace Modo.SkillTraining.Data {
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
        
        #region Cooking and Gathering
        { Cook,             new TrainingData(CookingAndGathering, 0.33m) },
        { CookTasty,        new TrainingData(CookingAndGathering, 0.66m) },
        { Harvest,          new TrainingData(CookingAndGathering, 0.01m) },
        { Butcher,          new TrainingData(CookingAndGathering, 0.10m) },
        #endregion
        
        #region Melee combat
        { AxeHit,           new TrainingData(Axe,                 0.01m) },
        { CudgelHit,        new TrainingData(Cudgel,              0.01m) },
        { LongBladeHit,     new TrainingData(LongBlade,           0.02m) },
        { ShortBladeHit,    new TrainingData(ShortBlade,          0.02m) },
        
        { SingleWeaponHit,  new TrainingData(SingleWeaponFighting,0.01m) },
        { OffhandWeaponHit, new TrainingData(MultiweaponFighting, 0.02m) },
        #endregion
        
        //@formatter:on
      };


    public static TrainingData For(PlayerAction action) =>
      Data.GetOr(action, () =>
        throw new Exception($"No training data for player action [{action}].")
      );
  }
}