using System;
using System.Linq;
using HarmonyLib;
using Qud.UI;
using XRL;
using XRL.World;

namespace ModoMods.ParasangRegion {
  [HarmonyPatch]
  public class Main {
    [HarmonyPostfix]
    [HarmonyPatch(typeof(XRLGame), nameof(XRLGame.StartQuest))]
    [HarmonyPatch(MethodType.Normal, typeof(Quest), typeof(String), typeof(String), typeof(String))]
    // ReSharper disable once InconsistentNaming
    public static void AfterStartQuest(Quest Quest) {
      // Quest giver locations get stored in the save file by the game, so set those to unmodified name.
      Quest.QuestGiverLocationName = The.ZoneManager.GetZoneDisplayName(Quest.QuestGiverLocationZoneID);
    }
    
    [HarmonyPostfix][HarmonyPatch(typeof(Zone), nameof(Zone.DisplayName), MethodType.Getter)]
    public static void DisplayName(ref Zone __instance, ref String __result) {
      var x = __instance.X switch { -1 => "?", 0 => "W", 1 => "", 2 => "E", _ => $"{__instance.X}" };
      var y = __instance.Y switch { -1 => "?", 0 => "N", 1 => "", 2 => "S", _ => $"{__instance.Y}" };
      var text = y + x;
      if (text == "??" && The.Player?.OnWorldMap() == true) { return; }
      if (text == "") text = "C";
      __result += " ({{C|" + text + "}})";
    }
  }
}