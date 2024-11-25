using System;
using HarmonyLib;
using Modo.SkillTraining.Data;
using XRL.World.Parts;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains Physic skill.</summary>
  /// <remarks>
  /// Since bandage application does not trigger any reliable event for detecting success/failure,
  /// this trainer uses Harmony patching instead of part attachment. 
  /// </remarks>
  [HarmonyPatch]
  public class PhysicTrainer {
    [HarmonyPostfix][HarmonyPatch(typeof(BandageMedication), nameof(BandageMedication.PerformBandaging))]
    public static void PostBandage(ref Boolean __result) {
      if (!__result) return;
      Main.PointTracker.HandleTrainingAction(PlayerAction.Bandage);
    }
  }
}