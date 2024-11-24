using System;
using System.Collections.Generic;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using XRL.UI;

namespace Modo.SkillTraining.Wiring {
  /// <summary>
  /// Global mod options, configured in the game's main Options screen and applying to all characters.
  /// </summary>
  public static class ModOptions {
    public static Boolean TrainingEnabled =>
      Options.GetOption("Option_Modo_SkillTraining_Enabled")?.EqualsNoCase("yes") ?? false;

    public static IDictionary<String, Decimal> Defaults = new Dictionary<String, Decimal> {
      // @formatter:off
      { SkillClasses.BowAndRifle,         0.25m }, // 100
      { SkillClasses.CustomsAndFolklore,  1.00m }, // 150
      { SkillClasses.HeavyWeapon,         0.25m }, // 100
      { SkillClasses.MultiweaponFighting, 0.30m }, // 150
      { SkillClasses.Physic,              1.00m }, //  50
      { SkillClasses.Pistol,              0.25m }, // 100
      { SkillClasses.Shield,              0.50m }, // 100

      { SkillClasses.SnakeOiler,          1.00m }, // 150
      { SkillClasses.Swimming,            0.15m }, // 100
      // @formatter:on
    };


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
    public static Decimal ShieldOilerTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_ShieldTrainingPercentage"))
        .AsPercentage();
    public static Decimal SnakeOilerTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_SnakeOilerTrainingPercentage"))
        .AsPercentage();
    public static Decimal SwimmingTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_SwimmingTrainingPercentage"))
        .AsPercentage();
    public static Decimal TinkeringTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_TinkeringTrainingPercentage"))
        .AsPercentage();
    public static Decimal WayfaringTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_WayfaringTrainingPercentage"))
        .AsPercentage();
  }
}