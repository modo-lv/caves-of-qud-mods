using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Cooking and Gathering" skill.</summary>
  [HarmonyPatch]
  public class CookingTrainer : ModPart {
    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Harvestable), nameof(Harvestable.AttemptHarvest))]
    private static void AfterHarvest(ref Boolean __result, ref GameObject who) {
      if (__result && who.CanTrainSkills())
        who.TrainingTracker()?.HandleTrainingAction(PlayerAction.Harvest);
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Butcherable), nameof(Butcherable.AttemptButcher))]
    private static void AfterButcher(ref Boolean __result, ref GameObject Actor) {
      if (__result && Actor.CanTrainSkills())
        Actor.TrainingTracker()?.HandleTrainingAction(PlayerAction.Butcher);
    }

    /// <summary>Listen for the <see cref="EventNames.CookedAt"/> event.</summary>
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.CookedAt);
    }

    /// <summary>Handle the <see cref="EventNames.CookedAt"/> event.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID == EventNames.CookedAt)
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.Cook);
      return base.FireEvent(ev);
    }

    /// <remarks>
    /// "Tasty" random meals don't trigger the <see cref="EventNames.CookedAt"/> event,
    /// and must be detected by their effects.
    /// </remarks>
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EffectAppliedEvent.ID };

    /// <summary>Handle the tasty cooking event.</summary>
    public override Boolean HandleEvent(EffectAppliedEvent ev) {
      if (ev.Effect is BasicCookingEffect)
        this.ParentObject.TrainingTracker()?.HandleTrainingAction(PlayerAction.CookTasty);
      return base.HandleEvent(ev);
    }
  }
}