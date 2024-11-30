using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Physic skill.</summary>
  [HarmonyPatch]
  public class PhysicTrainer : ModPart {
    /// <remarks>
    /// Since bandage application does not trigger any reliable event for detecting success/failure,
    /// this trainer uses Harmony patching instead of part attachment. 
    /// </remarks>
    [HarmonyPostfix][HarmonyPatch(typeof(BandageMedication), nameof(BandageMedication.PerformBandaging))]
    public static void PostBandage(ref Boolean __result) {
      if (!__result) return;
      Main.PointTracker.HandleTrainingAction(PlayerAction.Bandage);
    }

    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      AfterConsumeEvent.ID,
      EffectRemovedEvent.ID,
    };

    /// <summary>Handles recovering from any negative health effect.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Effect.ID.IsIn(EffectIds.HealthNegative))
        Main.PointTracker.HandleTrainingAction(PlayerAction.Recover);
      return base.HandleEvent(ev);
    }
    
    /// <summary>Handles tonic injection.</summary>
    public override Boolean HandleEvent(AfterConsumeEvent ev) {
      if (ev.Inject && ev.Actor.IsPlayer() && ev.Voluntary)
        Main.PointTracker.HandleTrainingAction(PlayerAction.Inject);
      return base.HandleEvent(ev);
    }
  }
}