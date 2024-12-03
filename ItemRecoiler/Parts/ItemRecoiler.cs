using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Parts;
using ModoMods.ItemRecoiler.Utils;
using ModoMods.ItemRecoiler.Wiring;
using static ModoMods.ItemRecoiler.Data.PropertyNames;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  /// <summary>Recoiler item functionality</summary>
  [Serializable] public class ItemRecoiler : ProgrammableRecoiler {
    /// <summary>Cached zone name for displaying the full item name.</summary>
    /// <remarks>
    /// Dynamically reading zone data while it's frozen can cause
    /// <c>SerializationReader::TryGetShared not on game context!</c>
    /// exception, so we need to cache the name when it is set/changed.
    /// It's also more performant, as we don't have to look up zone data every time the item is displayed. 
    /// </remarks>
    public String? ImprintedZoneName;

    public ITeleporter? Teleporter => this.ParentObject.GetPartDescendedFrom<ITeleporter>();

    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || id == GetInventoryActionsEvent.ID
      || id == InventoryActionEvent.ID;

    /// <summary>Adds/updates the item receiver chest.</summary>
    public override void ProgrammedForLocation(Zone zone, Cell cell) {
      var chestZone = this.ParentObject.GetPartDescendedFrom<ITeleporter>()?.DestinationZone?.Let(it =>
        ZoneManager.instance.GetZone(it)
      ) ?? zone;
      base.ProgrammedForLocation(zone, cell);
      this.ImprintedZoneName = zone.BaseDisplayName;
      var chest = chestZone.FindObject(it =>
        it.GetStringProperty(LinkedReceiver) == this.ParentObject.ID
      );
      if (chest == null && ModOptions.CreateReceivers) {
        var receiver = GameObject.CreateUnmodified("Chest");
        receiver.SetStringProperty(LinkedReceiver, this.ParentObject.ID);
        receiver.DisplayName = "{{itemrecoiler|recoiled item}} receiver";
        receiver.SetDetailColor('M');
        receiver.SetForegroundColor('y');
        receiver.RequirePart<Physics>().Also(it => {
          it.Takeable = false;
          it.Solid = false;
        });
        receiver.RequirePart<HologramMaterial>().Also(it => {
          it.ColorStrings = "&Y,&m,&y,&M";
          it.DetailColors = "y,Y,m,m";
        });
        receiver.SetStringProperty("OverlayColor", "&m^k");
        receiver.RequirePart<HologramInvulnerability>();
        receiver.RequirePart<Commerce>().Value = 0.0;
        receiver.RequirePart<Description>().Short =
          $"A landing beacon for items recoiled by\n{this.ParentObject.DisplayName}.";
        receiver.RequirePart<ReceiverErasure>();
        cell.AddObject(receiver);
      } else {
        var total = 0;
        chest?.Inventory.Objects.ToList().ForEach(item => {
          chest.CurrentCell.AddObject(item);
          total += item.Count;
        });
        if (total > 0)
          Output.DebugLog($"{total} item(s) removed from the chest before moving it to the new location.");
        chest?.ZoneTeleport(zone.ZoneID, cell.X, cell.Y);
      }
    }

    public override Boolean HandleEvent(GetInventoryActionsEvent ev) {
      if (this.Teleporter?.DestinationZone.IsNullOrEmpty() == false
          && this.IsObjectActivePartSubject(The.Player)
         ) {
        ev.AddAction(Name: "FindImprint", Display: "find imprint", EventNames.FindImprintCommand, Key: 'f');
      }
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command == EventNames.FindImprintCommand && this.Teleporter?.DestinationZone != null) {
        var sb = new StringBuilder();
        sb.Append(
          $"You hold the {this.ParentObject.BaseDisplayName} to your forehead for a moment. " +
          "Hazy disparate scraps of geospatial awareness surface slowly and " +
          "assemble themselves into a clear sense of location:\n\n"
        );
        var zone = ZoneManager.instance.GetZone(this.Teleporter.DestinationZone);
        var zoneName = zone.DisplayName;
        var x = zone.X switch { 0 => "west", 2 => "east", _ => "" };
        var y = zone.Y switch { 0 => "north", 2 => "south", _ => "" };
        var match = Regex.Match(zoneName, ",\\s*([^,]+)$").OnlyIf(it => it.Success);
        var depth = match?.Groups[1].Value;
        if (match != null)
          zoneName = zoneName.Replace(match.Groups[0].Value, "");
        sb.Append("Parasang {{B|" + zone.wX + ":" + zone.wY + "}}");
        sb.Append(" ({{W|" + zoneName + "}})");
        sb.Append(", {{B|" + $"{(x != "" || y != "" ? x + y : "center")}" + "}} region");
        if (depth != null)
          sb.Append(", {{B|" + depth + "}}");
        sb.Append(".");
        Output.Alert(sb.ToString());
      }
      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(GetDisplayNameEvent ev) {
      if (this.ImprintedZoneName != null)
        ev.AddBase(this.ImprintedZoneName, orderAdjust: -1);
      return base.HandleEvent(ev);
    }
  }
}