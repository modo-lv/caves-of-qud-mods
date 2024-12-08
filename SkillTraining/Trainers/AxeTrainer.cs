using HarmonyLib;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts.Skill;

namespace ModoMods.SkillTraining.Trainers {
  [HarmonyPatch] public class AxeTrainer {
    // ReSharper disable once InconsistentNaming
    [HarmonyPostfix][HarmonyPatch(typeof(Axe_Cleave), nameof(Axe_Cleave.PerformCleave))]
    public static void AfterCleave(GameObject Attacker) {
      Attacker.TrainingTracker()?.HandleTrainingAction(PlayerAction.Cleave);
    }
  }
}