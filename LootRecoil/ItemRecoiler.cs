using System;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil;
using ModoMods.LootRecoil.Data;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  [Serializable] public class ItemRecoiler : ProgrammableRecoiler {
    public Cell? Programmed;

    public override void ProgrammedForLocation(Zone zone, Cell cell) {
      Output.DebugLog("Imprinted");
      base.ProgrammedForLocation(zone, cell);
      if (this.Programmed == null) {
        cell.AddObject(
          GameObject.CreateUnmodified(LrBlueprintNames.Receiver)
        );
      } else {
        var oldChest = this.Programmed.FindObject(LrBlueprintNames.Receiver);
        oldChest?.ZoneTeleport(zone.ZoneID, cell.X, cell.Y);
      }
      this.Programmed = cell;
    }
  }
}