using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil.Data;
using XRL.World;

namespace ModoMods.LootRecoil {
  public class RecoiledVacuum : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EnteringZoneEvent.ID };

    public override Boolean HandleEvent(EnteringZoneEvent ev) {
      var zone = Main.Player.CurrentCell.ParentZone;
      AttemptCleaning(zone);

      return base.HandleEvent(ev);
    }

    public static void AttemptCleaning(Zone zone) {
      zone.TryGetPart(out HasTeleportedItems hasItems);
      if (hasItems != null) {
        var chest = zone.FindObject(LrBlueprintNames.Receiver);
        var total = 0;
        zone.FindObjects(it => it.HasPart<IsTeleportedItem>()).ForEach(item => {
          if (chest != null) {
            chest.Inventory.AddObject(item);
            total++;
          }
          item.RemovePart<IsTeleportedItem>();
        });
        zone.RemovePart<HasTeleportedItems>();
        Output.DebugLog($"Moved {total} teleported item(s) to the receiver.");
      }
    }
  }
}