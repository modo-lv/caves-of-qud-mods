using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Qud.API;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Customs and Folklore" skill.</summary>
  public class CustomsTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { SecretVisibilityChangedEvent.ID };
    public override Boolean HandleEvent(SecretVisibilityChangedEvent ev) {
      // Accomplishments are achievements, and general notes are those that a player writes by hand. 
      if (ev.Entry.Revealed && ev.Entry is not JournalAccomplishment and not JournalGeneralNote) {
        this.ParentObject.Training()?.HandleTrainingAction(PlayerAction.JournalReveal);
      }
      return base.HandleEvent(ev);
    }

    /// <remarks>
    /// For some reason <see cref="ReputationChangeEvent"/> does not get fired on the player
    /// for water ritual reputation changes.
    /// </remarks>
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, QudEventNames.ReputationChanged);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != QudEventNames.ReputationChanged)
        return base.FireEvent(ev);

      if (ev.Actor().CanTrainSkills()) {
        var type = ev.GetStringParameter("Type");
        Output.DebugLog($"Reputation change type: {type}");

        PlayerAction action;
        if (type == "WaterRitualPrimaryAward")
          action = PlayerAction.RitualFirstRep;
        else if (type.StartsWith("WaterRitual"))
          action = PlayerAction.RitualRep;
        else
          return base.FireEvent(ev);

        this.ParentObject.Training()?.HandleTrainingAction(action);
      }

      return base.FireEvent(ev);
    }
  }
}