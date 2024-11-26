using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using XRL.UI;
using XRL.World;
using GameObject = XRL.World.GameObject;

namespace ModoMods.ItemRecoiler {
  /// <summary>Provides item recoil functionality.</summary>
  /// <remarks>
  /// Actually sending items to distant zones isn't possible as game objects get serialized when the player
  /// moves too far away. Instead, we use an abstract chest object attached to the player that the player
  /// has no direct access to.
  /// 
  /// When the player drops the recoil receiver, the zone ID of the location is recorded and item recoiling
  /// enabled. "Recoiled" items are stored in <see cref="Escrow"/>, and, once the player returns to the zone,
  /// automatically transferred to the receiver chest at the end of the turn.
  /// </remarks>
  [Serializable]
  public class Recoiler : ModPart {
    private GameObject? _storage;
    public GameObject Escrow => this._storage ??= GameObject.CreateUnmodified("Chest");

    public String? ZoneId;

    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      CommandEvent.ID,
      GenericCommandEvent.ID,
      CommandTakeActionEvent.ID,
      EndTurnEvent.ID
    };

    public override Boolean HandleEvent(EndTurnEvent ev) {
      if (this.ZoneId == null
          && Main.Player.CurrentZone.HasObject(IrBlueprintNames.Storage)) {
        this.ZoneId = Main.Player.CurrentZone.ZoneID;
        Output.DebugLog($"Found receiver chest in zone [{this.ZoneId}], recoiling enabled.");
      }

      if (Main.Player.CurrentZone.ZoneID == this.ZoneId
          && !Main.Player.CurrentZone.HasObject(IrBlueprintNames.Storage)) {
        this.ZoneId = null;
        Output.DebugLog($"Recoil receiver in [{Main.Player.CurrentZone.ZoneID}] removed, recoiling disabled.");
      }
      
      if (Main.Player.CurrentZone.ZoneID == this.ZoneId
          && !this.Escrow.Inventory.Objects.IsNullOrEmpty()) {
        var chest = Main.Player.CurrentZone.FindObject(IrBlueprintNames.Storage);
        var container = Main.Player.Inventory;
        if (chest == null) {
          Output.Alert(
            "Recoiled item receiver seems to have disappeared. " +
            "Placing recoiled items back in the inventory."
          );
        } else {
          container = chest.Inventory;
        }
        while (!this.Escrow.Inventory.Objects.IsNullOrEmpty()) {
          var item = this.Escrow.Inventory.GetFirstObject();
          container.AddObject(item);
        }
      }
      return base.HandleEvent(ev);
    }
    
    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != IrEventNames.TransmitCommand)
        return base.HandleEvent(ev);
      if (this.ZoneId == null) {
        Popup.PickOption(
          Title: "No recoil receiver",
          Intro: "Items can only be recoiled to a manually placed recoil storage chest.\n\n" +
                 "If you don't have one in your inventory either, wish for " +
                 "{{Y|" + IrBlueprintNames.Transmitter + "}} (and don't forget to pick it up!).",
          AllowEscape: true,
          Options: Array.Empty<String>()
        );
        return base.HandleEvent(ev);
      }

      var transmitter = GameObject.CreateUnmodified(IrBlueprintNames.Transmitter);
      TradeUI.ShowTradeScreen(transmitter, 0.0f, TradeUI.TradeScreenMode.Container);
      var total = 0;
      while (!transmitter.Inventory.Objects.IsNullOrEmpty()) {
        var item = transmitter.Inventory.GetFirstObject();
        this.Escrow.Inventory.AddObject(item);
        total += item.Count;
      }
      Output.Message(total + " item(s) recoiled.");

      return base.HandleEvent(ev);
    }
  }
}