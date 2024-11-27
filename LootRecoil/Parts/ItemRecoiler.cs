using System;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil;
using ModoMods.LootRecoil.Data;
using XRL.UI;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  [Serializable] public class ItemRecoiler : ProgrammableRecoiler {
    public Cell? Programmed;

    public override void ProgrammedForLocation(Zone zone, Cell cell) {
      base.ProgrammedForLocation(zone, cell);
      if (this.Programmed == null) {
        cell.AddObject(
          GameObject.CreateUnmodified(LrBlueprintNames.Receiver)
        );
      } else {
        var oldChest = this.Programmed.FindObject(LrBlueprintNames.Receiver);
        var total = 0;
        oldChest?.Inventory.Objects.ToList().ForEach(item => {
          this.Programmed.AddObject(item);
          total += item.Count;
        });
        if (total > 0) {
          Output.DebugLog($"{total} item(s) removed from the chest to move it to the new location.");
        }
        oldChest?.ZoneTeleport(zone.ZoneID, cell.X, cell.Y);
      }
      this.Programmed = cell;
    }
  }
}