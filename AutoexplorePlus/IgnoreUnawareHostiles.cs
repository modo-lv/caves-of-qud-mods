using System;
using System.Collections.Generic;
using HarmonyLib;
using ModoMods.Core.Utils;
using XRL.World;

namespace ModoMods.AutoexplorePlus {
  [HarmonyPatch] public class IgnoreUnawareHostiles : ModPart {
    public override Int32 Priority => PRIORITY_VERY_LOW;

    public override ISet<Int32> WantEventIds => new HashSet<Int32> { ExtraHostilePerceptionEvent.ID };

    public override Boolean HandleEvent(ExtraHostilePerceptionEvent ev) {
      if (ev.Actor == this.ParentObject
          && ev.Actor.IsPlayer()
          && ev is { Hostile: not null }
          && ev.Hostile.Brain.Target != ev.Actor
          && ev.Hostile.DistanceTo(this.ParentObject) > 2 
         ) {
        Output.Message(
          $"Hostile [{ev.Hostile.DisplayName}] detected ({ev.PerceiveVerb}), " +
          $"but is not targeting player, ignoring."
        );
        ev.Hostile = null;
        ev.PerceiveVerb = null;
      }
      return base.HandleEvent(ev);
    }
    
  }
}