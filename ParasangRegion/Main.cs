using System;
using HarmonyLib;
using XRL;
using XRL.World;

namespace ModoMods.ParasangRegion {
  [HarmonyPatch]
  public class Main {
    [HarmonyPostfix][HarmonyPatch(typeof(Zone), nameof(Zone.DisplayName), MethodType.Getter)]
    public static void DisplayName(ref Zone __instance, ref String __result) {
      if (The.Player?.OnWorldMap() != false)
        return;
      var x = __instance.X switch { 0 => "W", 1 => "", 2 => "E", _ => "?" };
      var y = __instance.Y switch { 0 => "N", 1 => "", 2 => "S", _ => "?" };
      var text = y + x;
      if (text == "") text = "C";
      __result += " ({{C|" + text + "}})";
    }
  }
}