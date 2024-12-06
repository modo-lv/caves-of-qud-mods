using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Tinkering;

namespace ModoMods.SkillTraining.Trainers {
  [HarmonyPatch] public class TinkeringTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { ExamineSuccessEvent.ID, };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      ev.Actor.TrainingTracker()?.HandleTrainingAction(PlayerAction.ExamineSuccess);
      return base.HandleEvent(ev);
    }

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [HarmonyPostfix][HarmonyPatch(typeof(Garbage), nameof(Garbage.AttemptRifle))]
    public static void AfterTrashRifle(ref Boolean __result, ref GameObject Actor) {
      if (__result && Actor.CanTrainSkills()) {
        The.Player.TrainingTracker()?.HandleTrainingAction(PlayerAction.RifleTrashSuccess);
      }
    }

    [HarmonyPostfix][HarmonyPatch(typeof(Disassembly), nameof(Disassembly.End))]
    public static void AfterDisassemble(ref Disassembly __instance) {
      The.Player.TrainingTracker()?.HandleTrainingAction(
        action: PlayerAction.DisassembleBit,
        amountModifier: __instance.BitsDone.Length
      );
    }
  }
}