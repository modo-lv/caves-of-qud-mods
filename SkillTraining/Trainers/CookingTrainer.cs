using System;
using HarmonyLib;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Cooking and Gathering" skill.</summary>
  [HarmonyPatch]
  public class CookingTrainer : ModPart {
    /// <remarks>
    /// Since harvesting can be done by anyone, the actor has to be checked for being the player,
    /// and the result remembered for the post-harvest check.
    /// </remarks>
    [HarmonyPrefix][HarmonyPatch(typeof(Harvestable), nameof(Harvestable.AttemptHarvest))]
    private static void PreHarvest(GameObject who, out Boolean __state) {
      __state = who.IsPlayer();
    }

    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Harvestable), nameof(Harvestable.AttemptHarvest))]
    private static void PostHarvest(ref Boolean __result, ref Boolean __state) {
      if (__state && __result) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.Harvest);
      }
    }

    /// <remarks>
    /// Since butchering can be done by anyone, the actor has to be checked for being the player,
    /// and the result remembered for the post-butcher check.
    /// </remarks>
    [HarmonyPrefix][HarmonyPatch(typeof(Butcherable), nameof(Butcherable.AttemptButcher))]
    private static void PreButcher(GameObject Actor, out Boolean __state) {
      __state = Actor.IsPlayer();
    }

    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Butcherable), nameof(Butcherable.AttemptButcher))]
    private static void PostButcher(ref Boolean __result, ref Boolean __state) {
      if (__state && __result) {
        Main.PointTracker.HandleTrainingAction(PlayerAction.Butcher);
      }
    }

    /// <summary>Listen for the <see cref="EventNames.CookedAt"/> event.</summary>
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.CookedAt);
    }

    /// <summary>Handle the <see cref="EventNames.CookedAt"/> event.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID == EventNames.CookedAt)
        Main.PointTracker.HandleTrainingAction(PlayerAction.Cook);
      return base.FireEvent(ev);
    }

    /// <remarks>
    /// "Tasty" random meals don't trigger the <see cref="EventNames.CookedAt"/> event,
    /// and must be detected by their effects.
    /// </remarks>
    public override Set<Int32> WantEventIds => new Set<Int32> { EffectAppliedEvent.ID };

    /// <summary>Handle the tasty cooking event.</summary>
    public override Boolean HandleEvent(EffectAppliedEvent ev) {
      if (ev.Actor.IsPlayer() && ev.Effect.GetType().IsSubclassOf(typeof(BasicCookingEffect)))
        Main.PointTracker.HandleTrainingAction(PlayerAction.CookTasty);
      return base.HandleEvent(ev);
    }
  }
}