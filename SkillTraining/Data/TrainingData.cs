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
        { Cook,      new TrainingData(CookingAndGathering, 0.30m) },
        { CookTasty, new TrainingData(CookingAndGathering, 0.60m) },
        { Harvest,   new TrainingData(CookingAndGathering, 0.01m) },
        { Butcher,   new TrainingData(CookingAndGathering, 0.10m) },
        //@formatter:on
      };


    public static TrainingData For(PlayerAction action) =>
      Data.GetOr(action, () =>
        throw new Exception($"No training data for player action [{action}].")
      );
  }
}