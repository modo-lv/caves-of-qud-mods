using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Physic skill.</summary>
  [HarmonyPatch]
  public class PhysicTrainer : ModPart {
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    [HarmonyPostfix][HarmonyPatch(typeof(BandageMedication), nameof(BandageMedication.PerformBandaging))]
    public static void AfterBandage(ref Boolean __result, ref GameObject Actor) {
      if (__result && Actor.CanTrainSkills())
        Actor.Training()?.HandleTrainingAction(PlayerAction.Bandage);
    }

    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      AfterConsumeEvent.ID,
      EffectRemovedEvent.ID,
    };

    /// <summary>Handles recovering from any negative health effect.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      if (ev.Effect.GetType().IsOneOf(QudEffectTypes.PhysicalNegative))
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.Recover);
      return base.HandleEvent(ev);
    }
    
    /// <summary>Handles tonic injection.</summary>
    public override Boolean HandleEvent(AfterConsumeEvent ev) {
      if (ev.Inject && ev.Actor.IsPlayer() && ev.Voluntary)
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.Inject);
      return base.HandleEvent(ev);
    }
  }
}