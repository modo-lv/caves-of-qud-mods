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
  public class CookingTrainer : ModPart {
    /// <summary>Listen for the <see cref="QudEventNames.CookedAt"/> event.</summary>
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, QudEventNames.CookedAt);
    }
    
    /// <summary>Handle the <see cref="QudEventNames.CookedAt"/> event.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID == QudEventNames.CookedAt)
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.Cook);
      return base.FireEvent(ev);
    }
    
    /// <remarks>
    /// "Tasty" random meals don't trigger the <see cref="QudEventNames.CookedAt"/> event,
    /// and must be detected by their effects.
    /// </remarks>
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EffectAppliedEvent.ID };
    
    /// <summary>Handle the tasty cooking event.</summary>
    public override Boolean HandleEvent(EffectAppliedEvent ev) {
      if (ev.Effect is BasicCookingEffect)
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.CookTasty);
      return base.HandleEvent(ev);
    }
  }
  
  // ReSharper disable UnusedType.Global, UnusedMember.Local, InconsistentNaming
  /// <remarks>Harmony patches for methods don't raise events.</remarks>
  [HarmonyPatch] public static class CookingTrainerPatches {
    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Harvestable), nameof(Harvestable.AttemptHarvest))]
    private static void AfterHarvest(ref Boolean __result, ref GameObject who) {
      if (__result && who.CanTrainSkills())
        who.Training()?.HandleTrainingAction(PlayerAction.Harvest);
    }

    /// <summary>Training points from harvesting plants.</summary>
    [HarmonyPostfix][HarmonyPatch(typeof(Butcherable), nameof(Butcherable.AttemptButcher))]
    private static void AfterButcher(ref Boolean __result, ref GameObject Actor) {
      if (__result && Actor.CanTrainSkills())
        Actor.Training()?.HandleTrainingAction(PlayerAction.Butcher);
    }

    [HarmonyPostfix][HarmonyPatch(typeof(Campfire), nameof(Campfire.CookFromIngredients))]
    private static void AfterIngredients(ref Boolean __result, ref Boolean random) {
      if (__result && !random)
        The.Player.Training()?.HandleTrainingAction(PlayerAction.CookIngredients);
    }
  }
}