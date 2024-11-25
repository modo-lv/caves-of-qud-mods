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
      { SkillClasses.Swimming,            0.15m }, // 100
      // @formatter:on
    };


    public static Decimal SwimmingTrainingRate =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_SwimmingTrainingPercentage"))
        .AsPercentage();
  }
}