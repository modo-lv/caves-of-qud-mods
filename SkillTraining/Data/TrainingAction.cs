using System;
using System.Collections.Generic;
using static Modo.SkillTraining.Data.PlayerAction;
using static Modo.SkillTraining.Data.SkillClasses;

namespace Modo.SkillTraining.Data {
  public struct TrainingAction {
    public readonly String SkillClass;
    public readonly Decimal DefaultAmount;

    public TrainingAction(String skillClass, Decimal defaultAmount) {
      this.SkillClass = skillClass;
      this.DefaultAmount = defaultAmount;
    }

    public static readonly IDictionary<PlayerAction, TrainingAction> Data =
      new Dictionary<PlayerAction, TrainingAction> {
        //@formatter:off
        { Cook,      new TrainingAction(CookingAndGathering, 0.30m) },
        { CookTasty, new TrainingAction(CookingAndGathering, 0.60m) },
        { Harvest,   new TrainingAction(CookingAndGathering, 0.01m) },
        { Butcher,   new TrainingAction(CookingAndGathering, 0.10m) },
        //@formatter:on
      };
  }
}