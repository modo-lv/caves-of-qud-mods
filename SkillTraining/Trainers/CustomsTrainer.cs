using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Qud.API;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Skill;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Customs and Folklore" skills.</summary>
  public class CustomsTrainer : ModPart {
    
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      SecretVisibilityChangedEvent.ID,
      AfterReputationChangeEvent.ID,
    };

    /// <summary>Rewards training points for discovering secrets.</summary>
    public override Boolean HandleEvent(SecretVisibilityChangedEvent ev) {
      // Accomplishments are achievements, and general notes are those that a player writes by hand. 
      if (ev.Entry.Revealed && ev.Entry is not JournalAccomplishment and not JournalGeneralNote)
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.JournalReveal);
      return base.HandleEvent(ev);
    }

    /// <summary>Rewards training points for reputation changes during water rituals.</summary>
    public override Boolean HandleEvent(AfterReputationChangeEvent ev) {
      if (this.ParentObject.CanTrainSkills()) {
        Output.DebugLog($"Reputation change type: {ev.Type}");

        PlayerAction action;
        if (ev.Type == "WaterRitualPrimaryAward")
          action = PlayerAction.RitualFirstRep;
        else if (ev.Type.StartsWith("WaterRitual"))
          action = PlayerAction.RitualRep;
        else
          return base.HandleEvent(ev);

        this.ParentObject.Training()?.HandleTrainingAction(action);
      }
      return base.HandleEvent(ev);
    }
  }

  [HarmonyPatch] public static class TrashRifle {
    // ReSharper disable InconsistentNaming, UnusedMember.Global
    [HarmonyPostfix][HarmonyPatch(typeof(Garbage), nameof(Garbage.AttemptRifle))]
    public static void AfterTrashRifle(ref GameObject Actor) {
      if (Actor.HasPart<Tinkering_Scavenger>() && Actor.CanTrainSkills())
        The.Player.Training()?.HandleTrainingAction(PlayerAction.RifleTrash);
    }
  }
}