using System;
using XRL.UI;

namespace Modo.SkillTraining.Wiring {
  public static class ModOptions {
    public static Int32 MeleeTrainingPercentage =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MeleeTrainingPercentage"));
    public static Int32 ThrownTrainingPercentage =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_ThrownTrainingPercentage"));
    public static Int32 MissileTrainingPercentage =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MissileTrainingPercentage"));
  }
}