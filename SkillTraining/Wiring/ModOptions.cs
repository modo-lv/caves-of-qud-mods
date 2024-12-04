using System;
using XRL.UI;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>
  /// Global mod options, configured in the game's main Options screen and applying to all characters.
  /// </summary>
  public static class ModOptions {
    public static Boolean TrainingEnabled =>
      Options.GetOption("Option_ModoMods_SkillTraining_Enabled")?.EqualsNoCase("yes") ?? false;
    
    public static Boolean ShowTraining =>
      Options.GetOption("Option_ModoMods_SkillTraining_ShowTraining")?.EqualsNoCase("yes") ?? false;
    
    public static Boolean ModifyCosts =>
      Options.GetOption("Option_ModoMods_SkillTraining_ModifyCosts")?.EqualsNoCase("yes") ?? false;
  }
}