using System;
using HarmonyLib;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL.World.Parts;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Modo.SkillTraining.Parts {
  /// <summary>Trains Physic skill.</summary>
  /// <remarks>
  /// Since bandage application does not trigger any reliable event for detecting success/failure,
  /// this trainer uses Harmony patching instead of part attachment. 
  /// </remarks>
  [HarmonyPatch(typeof(BandageMedication), nameof(BandageMedication.PerformBandaging))]
  public class PhysicTrainer {
    public static void Postfix(ref Boolean __result) {
      if (!__result)
        return;
      Output.DebugLog("Bandage applied.");
      Req.TrainingTracker.AddPoints(SkillClasses.Physic, ModOptions.PhysicTrainingRate);
    }
  }
}