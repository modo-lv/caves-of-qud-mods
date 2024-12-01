using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains Self-Discipline skills.</summary>
  [HarmonyPatch]
  public class SelfDisciplineTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EffectRemovedEvent.ID };

    /// <summary>Handles recovering from any negative health effect.</summary>
    public override Boolean HandleEvent(EffectRemovedEvent ev) {
      switch (ev.Effect) {
        case Terrified:
          Main.PointTracker.HandleTrainingAction(PlayerAction.RecoverTerror);
          break;
        case Confused:
          Main.PointTracker.HandleTrainingAction(PlayerAction.RecoverConfusion);
          break;
      }
      return base.HandleEvent(ev);
    }
  }
}