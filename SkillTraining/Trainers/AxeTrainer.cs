using HarmonyLib;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts.Skill;

namespace ModoMods.SkillTraining.Trainers {
  /// <remarks>
  /// This should have been an <see cref="IPart"/>,
  /// but <see cref="Axe_Cleave.PerformCleave"/> produces no events. 
  /// </remarks>
  // ReSharper disable once UnusedType.Global
  [HarmonyPatch] public class AxeTrainer {
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    [HarmonyPostfix][HarmonyPatch(typeof(Axe_Cleave), nameof(Axe_Cleave.PerformCleave))]
    public static void AfterCleave(GameObject Attacker) {
      Attacker.Training()?.HandleTrainingAction(PlayerAction.Cleave);
    }
  }
}