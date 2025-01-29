using System;
using XRL.UI;

namespace ModoMods.ParasangRegion.Wiring {
  /// <summary>
  /// Global mod options, configured in the game's main Options screen and applying to all characters.
  /// </summary>
  public static class ModOptions {
    public static Boolean RequireMindsCompass =>
      Options.GetOption("Option_ModoMods_ParasangRegion_RequireMindsCompass")
        ?.EqualsNoCase("yes") == true;
    
    public static Boolean HideWhenLost =>
      Options.GetOption("Option_ModoMods_ParasangRegion_HideWhenLost")
        ?.EqualsNoCase("yes") == true;
  }
}