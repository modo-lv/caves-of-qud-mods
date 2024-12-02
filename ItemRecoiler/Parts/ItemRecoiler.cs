using System;
using System.Linq;
using System.Text;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Utils;
using static ModoMods.ItemRecoiler.Data.PropertyNames;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  /// <summary>Recoiler item functionality</summary>
  [Serializable] public class ItemRecoiler : ProgrammableRecoiler {
    /// <summary>Cached zone name for displaying the full item name.</summary>
    /// <remarks>
    /// Dynamically reading zone data while it's frozen causes a
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
      var chest = chestZone.FindObject(it => it.GetBooleanProperty(IsItemReceiver));
      if (chest == null) {
        var receiver = GameObject.CreateUnmodified("Chest");
        receiver.SetBooleanProperty(IsItemReceiver, true);
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
          "A landing beacon for {{itemrecoiler|items recoiling}} through the ether.";
        cell.AddObject(receiver);
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

    public override Boolean HandleEvent(GetInventoryActionsEvent ev) {
      if (this.Teleporter?.DestinationZone != null && this.IsObjectActivePartSubject(The.Player))
        ev.AddAction(Name: "FindImprint", Display: "find imprint", EventNames.FindImprintCommand, Key: 'f');
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
        var parasang = ZoneManager.instance.GetZone(this.Teleporter.DestinationZone);
        var x = parasang.X switch { 0 => "west", 2 => "east", _ => "" };
        var y = parasang.Y switch { 0 => "north", 2 => "south", _ => "" };
        sb.Append("Parasang {{B|" + parasang.wX + ":" + parasang.wY + "}} ");
        sb.Append("(" + parasang.DisplayName + "), ");
        sb.Append("{{B|" + $"{(x != "" || y != "" ? x+y : "center")}" + "}} region.");
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