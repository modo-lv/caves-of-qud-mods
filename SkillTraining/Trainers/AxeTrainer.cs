using HarmonyLib;
using JetBrains.Annotations;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts.Skill;

namespace ModoMods.SkillTraining.Trainers {
  /// <remarks>
  /// This should have been an <see cref="IPart"/>,
  /// but <see cref="Axe_Cleave.PerformCleave"/> produces no events. 
  /// </remarks>
  [HarmonyPatch][UsedImplicitly] public class AxeTrainer {
    [HarmonyPostfix][HarmonyPatch(typeof(Axe_Cleave), nameof(Axe_Cleave.PerformCleave))]
    // ReSharper disable once InconsistentNaming
    public static void AfterCleave(GameObject Attacker) {
      Attacker.Training()?.HandleTrainingAction(PlayerAction.Cleave);
    }
  }
}