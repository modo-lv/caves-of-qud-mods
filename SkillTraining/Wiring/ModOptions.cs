using System;
using Modo.SkillTraining.Internal;
using XRL.UI;

namespace Modo.SkillTraining.Wiring {
  public static class ModOptions {
    public static Decimal MeleeTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MeleeTrainingPercentage"))
        .AsPercentage();
    public static Decimal ThrownTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_ThrownTrainingPercentage"))
        .AsPercentage();
    public static Decimal MissileTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MissileTrainingPercentage"))
        .AsPercentage();
  }
}