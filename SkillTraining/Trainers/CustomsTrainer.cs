using System;
using HarmonyLib;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Qud.API;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains "Customs and Folklore" skill.</summary>
  [HarmonyPatch] public class CustomsTrainer : ModPart {

    [HarmonyPrefix][HarmonyPatch(typeof(IBaseJournalEntry), nameof(IBaseJournalEntry.Reveal))]
    public static void PreReveal(ref IBaseJournalEntry __instance) {
      if (__instance.Revealed
          || __instance
            is not JournalMapNote
            and not JournalRecipeNote
            and not JournalSultanNote
            and not JournalVillageNote)
        return;

      Main.PointTracker.HandleTrainingAction(PlayerAction.SecretReveal);
    }

    /// <remarks>
    /// For some reason <see cref="ReputationChangeEvent"/> does not get fired on the player
    /// for water ritual reputation changes.
    /// </remarks>
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.ReputationChanged);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      var type = ev.GetStringParameter("Type");
      Output.DebugLog($"Reputation change type: {type}");

      PlayerAction action;
      if (type == "WaterRitualPrimaryAward")
        action = PlayerAction.FirstRitualRep;
      else if (type.StartsWith("WaterRitual"))
        action = PlayerAction.RitualRep;
      else
        return base.FireEvent(ev);

      Main.PointTracker.HandleTrainingAction(action: action);

      return base.FireEvent(ev);
    }
  }
}