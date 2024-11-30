using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  [HarmonyPatch]
  public class TinkeringTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { ExamineSuccessEvent.ID, };

    public override Boolean HandleEvent(ExamineSuccessEvent ev) {
      if (ev.Actor.IsPlayer()) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.ExamineSuccess);
      } 
      return base.HandleEvent(ev);
    }


    [HarmonyPostfix][HarmonyPatch(typeof(Garbage), nameof(Garbage.AttemptRifle))]
    public static void AfterRifle(ref Boolean __result) {
      if (__result) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.RifleTrashSuccess);
      }
    }
  }
}