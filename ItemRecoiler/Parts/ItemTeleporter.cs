using System;
using System.Linq;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Utils;
using XRL.UI;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  /// <summary>Teleporter part for recoiling items.</summary>
  public class ItemTeleporter : Teleporter {
    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || id == InventoryActionEvent.ID;

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command == QudCommands.ActivateTeleporter) {
        if (this.AttemptItemTeleport())
          ev.RequestInterfaceExit();
        return true;
      }
      return base.HandleEvent(ev);
    }

    /// <summary>A simplified version of <see cref="ITeleporter.AttemptTeleport"/></summary>
    public Boolean AttemptItemTeleport() {
      if (this.DestinationZone.IsNullOrEmpty())
        return Main.Player.Fail("Nothing happens.");

      if (!this.UsableInCombat && Main.Player.AreHostilesNearby())
        return Main.Player == this.ParentObject
          ? Main.Player.Fail("You can't recoil with hostiles nearby!")
          : Main.Player.Fail("You can't use " + this.ParentObject.t() + " with hostiles nearby!");

      // Teleport items
      var transmitter = this.ParentObject.DeepCopy().Also(it => it.RequirePart<Inventory>());
      TradeUI.ShowTradeScreen(transmitter, 0.0f, TradeUI.TradeScreenMode.Container);

      var chargeNeeded = transmitter.Inventory.Objects.Sum(o => o.Weight);
      var actualCharge = this.ParentObject.QueryCharge();
      if (chargeNeeded > actualCharge) {
        if (actualCharge > 0)
          Popup.ShowFail(
            this.ParentObject.Does("hum") + " for a moment, then powers down. " +
            this.ParentObject.Does("don't", Pronoun: true) + " have enough charge to recoil " +
            "all the items ({{Y|" + actualCharge + "}}/{{Y|" + chargeNeeded + "}})."
          );
        else
          Popup.ShowFail(this.ParentObject.Does("don't") + " have any charge left.");

        Main.Player.Inventory.AddObject(transmitter.Inventory.Objects.ToList());

        return true;
      }
      var total = 0;
      var chargeSpent = 0;
      var zone = The.ZoneManager.GetZone(this.DestinationZone);
      var chest = zone.FindObject(it => it.GetBooleanProperty(PropertyNames.IsItemReceiver));
      while (!transmitter.Inventory.Objects.IsNullOrEmpty()) {
        var item = transmitter.Inventory.GetFirstObject();
        if (item.Blueprint == ModBlueprintNames.Recoiler) {
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
          this.ParentObject.UseCharge(item.Weight);
          chargeSpent += item.Weight;
        }
      }
      transmitter.Obliterate(Silent: true);
      if (total > 0) {
        Main.Player.PlayWorldOrUISound(this.Sound);
        Output.Message("{{Y|" + total + "}} item(s) recoiled to {{Y|" + zone.DisplayName + "}}.");
        Output.DebugLog($"[{this.ParentObject}] charge - {chargeSpent} = {this.ParentObject.QueryCharge()}.");
      }

      return true;
    }
  }
}