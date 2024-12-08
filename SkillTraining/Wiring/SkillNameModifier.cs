using System;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Appends an indicator to trainable skill names.</summary>
  [HarmonyPatch] public class SkillNameModifier {
    public static Boolean Modified;

    [HarmonyPrefix][HarmonyPatch(typeof(SkillFactory), nameof(SkillFactory.Factory), MethodType.Getter)]
    // ReSharper disable once InconsistentNaming
    public static void BeforeFactory(ref SkillFactory? ____Factory) {
      if (____Factory == null) {
        Output.DebugLog("SkillFactory is null, reload imminent, resetting name modifier...");
        Modified = false;
      }
    }

    [HarmonyPostfix][HarmonyPatch(typeof(SkillFactory), nameof(SkillFactory.Factory), MethodType.Getter)]
    public static void Names(ref SkillFactory __result) {
      if (!Modified) {
        Output.DebugLog("Skills (re)loaded, adding training indicators:");
        foreach (var key in Main.AllTrainableSkills.Keys) {
          __result.GetFirstEntry(key)?.Also(it => {
            it.Name += "+";
            Output.DebugLog("> " + it.Name);
          });
        }
        Modified = true;
      }
    }
  }
}