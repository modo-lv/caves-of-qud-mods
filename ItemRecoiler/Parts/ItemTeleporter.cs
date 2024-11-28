using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Parts;
using XRL.UI;
using XRL.World.Parts;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  /// <summary>Teleporter part for recoiling items.</summary>
  public class ItemTeleporter : Teleporter {
    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || id == InventoryActionEvent.ID;

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command == CommandNames.ActivateTeleporter && this.AttemptItemTeleport()) {
        ev.RequestInterfaceExit();
        return true;
      }
      return false;
    }

    /// <summary>A simplified version of <see cref="ITeleporter.AttemptTeleport"/></summary>
    public Boolean AttemptItemTeleport() {
      if (this.DestinationZone.IsNullOrEmpty())
        return Main.Player.Fail("Nothing happens.");

      if (!this.UsableInCombat && Main.Player.AreHostilesNearby())
        return Main.Player == this.ParentObject
          ? Main.Player.Fail("You can't recoil with hostiles nearby!")
          : Main.Player.Fail("You can't use " + this.ParentObject.t() + " with hostiles nearby!");

      Main.Player.PlayWorldOrUISound(this.Sound);
      Output.Message($"You activate the {this.ParentObject}.");

      // Teleport items
      var transmitter = GameObject.CreateUnmodified(IrBlueprintNames.Transmitter);
      TradeUI.ShowTradeScreen(transmitter, 0.0f, TradeUI.TradeScreenMode.Container);
      var total = 0;
      var zone = The.ZoneManager.GetZone(this.DestinationZone);
      var chest = zone.FindObject(IrBlueprintNames.Receiver);
      while (!transmitter.Inventory.Objects.IsNullOrEmpty()) {
        var item = transmitter.Inventory.GetFirstObject();
        if (item.Blueprint == IrBlueprintNames.Recoiler) {
          Main.Player.Inventory.AddObject(item);
          continue;
        }
        if (item.ZoneTeleport(
              this.DestinationZone,
              this.DestinationX,
              this.DestinationY,
              null,
              this.ParentObject,
              Main.Player)) {
          chest?.Inventory.AddObject(item);
          total += item.Count;
        }
      }
      Output.Message(total + " item(s) recoiled to " + zone.DisplayName + ".");

      return true;
    }
  }
}