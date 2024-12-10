using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using Qud.API;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;

namespace ModoMods.AutoexplorePlus {
  [HarmonyPatch] public class IgnoreUnawareHostiles : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { ExtraHostilePerceptionEvent.ID };

    public override Boolean HandleEvent(ExtraHostilePerceptionEvent ev) {
      if (ev.Actor == this.ParentObject
          && ev.Actor.IsPlayer()
          && ev is { Hostile: not null, PerceiveVerb: "hear" }
          && ev.Hostile.Brain.Target != ev.Actor
         ) {
        Output.DebugLog($"Hostile [{ev.Hostile.DisplayName}] heard, but is not targeting player, ignoring.");
        ev.Hostile = null;
        ev.PerceiveVerb = null;
      }
      return base.HandleEvent(ev);
    }

    [HarmonyPostfix]
    [HarmonyPatch(
      typeof(Mutations),
      nameof(Mutations.AddMutation),
      argumentTypes: new[] { typeof(BaseMutation), typeof(Int32), typeof(Boolean) }
    )]
    public static void AfterMutation(ref Mutations __instance, BaseMutation NewMutation) {
      if (__instance.ParentObject.IsPlayer() && NewMutation is HeightenedHearing) {
        Output.DebugLog(
          $"[{nameof(HeightenedHearing)}] mutation added, " +
          $"making sure [{nameof(IgnoreUnawareHostiles)}] part is attached after it..."
        );
        __instance.ParentObject.RemovePart<IgnoreUnawareHostiles>();
        __instance.ParentObject.RequirePart<IgnoreUnawareHostiles>();
      }
    }
  }
}