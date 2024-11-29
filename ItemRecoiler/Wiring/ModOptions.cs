using System;
using XRL.UI;

namespace ModoMods.ItemRecoiler.Wiring {
  /// <summary>
  /// Global mod options, configured in the game's main Options screen and applying to all characters.
  /// </summary>
  public static class ModOptions {
    public static Boolean GiveOnStartup =>
      Options.GetOption("Option_ModoMods_ItemRecoiler_GiveOnStartup")?.EqualsNoCase("yes") ?? false;
  }
}