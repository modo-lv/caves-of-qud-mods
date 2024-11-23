using System;
using Modo.SkillTraining.Internal;
using XRL.UI;

namespace Modo.SkillTraining.Wiring {
  public static class ModOptions {
    public static Decimal CookingTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_CookingTrainingPercentage"))
        .AsPercentage();
    public static Decimal CustomsTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_CustomsTrainingPercentage"))
        .AsPercentage();
    public static Decimal MeleeTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MeleeTrainingPercentage"))
        .AsPercentage();
    public static Decimal ThrownTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_ThrownTrainingPercentage"))
        .AsPercentage();
    public static Decimal MissileTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_MissileTrainingPercentage"))
        .AsPercentage();
    public static Decimal PhysicTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_PhysicTrainingPercentage"))
        .AsPercentage();
    public static Decimal SwimmingTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_SwimmingTrainingPercentage"))
        .AsPercentage();
    public static Decimal WayfaringTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_WayfaringTrainingPercentage"))
        .AsPercentage();
  }
}