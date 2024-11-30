using System;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Utils;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  /// <summary>Recoiler item functionality</summary>
  [Serializable] public class ItemRecoiler : ProgrammableRecoiler {
    /// <summary>Cached zone name for displaying the full item name.</summary>
    /// <remarks>
    /// Dynamically reading zone data while it's frozen causes a
    /// <c>SerializationReader::TryGetShared not on game context!</c>
    /// exception, so we need to cache the name when it is set/chagned.
    /// It's also more performant, as we don't have to look up zone data every time the item is displayed. 
    /// </remarks>
    public String? ImprintedZoneName;

    /// <summary>Adds/updates the item receiver chest.</summary>
    public override void ProgrammedForLocation(Zone zone, Cell cell) {
      var chestZone = this.ParentObject.GetPartDescendedFrom<ITeleporter>()?.DestinationZone?.Let(it =>
        ZoneManager.instance.GetZone(it)
      ) ?? zone;
      var chest = chestZone.FindObject(ModBlueprintNames.Receiver);
      if (chest == null) {
        cell.AddObject(
          GameObject.CreateUnmodified(ModBlueprintNames.Receiver)
        );
      } else {
        var total = 0;
        chest.Inventory.Objects.ToList().ForEach(item => {
          chest.CurrentCell.AddObject(item);
          total += item.Count;
        });
        if (total > 0)
          Output.DebugLog($"{total} item(s) removed from the chest before moving it to the new location.");
        chest.ZoneTeleport(zone.ZoneID, cell.X, cell.Y);
      }
      this.ImprintedZoneName = zone.BaseDisplayName;
      base.ProgrammedForLocation(zone, cell);
    }

    public override Boolean HandleEvent(GetDisplayNameEvent ev) {
      if (this.ImprintedZoneName != null)
        ev.AddBase(this.ImprintedZoneName, orderAdjust: -1);
      return base.HandleEvent(ev);
    }
  }
}