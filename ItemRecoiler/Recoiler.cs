using System;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using Wintellect.PowerCollections;
using XRL.UI;
using XRL.World;
using EventNames = ModoMods.Core.Data.EventNames;
using GameObject = XRL.World.GameObject;

namespace ModoMods.ItemRecoiler {
  [Serializable]
  public class Recoiler : ModPart {
    public GameObject? Storage;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      CommandEvent.ID,
      GenericCommandEvent.ID,
      CommandTakeActionEvent.ID,
      InventoryActionEvent.ID,
    };

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (!ev.Actor.IsPlayer()
          || ev.Command != EventNames.CommandDropObject
          || ev.Item.Blueprint != IrBlueprintNames.Storage)
        return base.HandleEvent(ev);

      this.Storage = ev.Item;
      Output.DebugLog($"[{ev.Actor}] placed [{ev.Item.Blueprint}], recoiled items should go to it.");

      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != IrEventNames.TransmitCommand)
        return base.HandleEvent(ev);
      if (this.Storage == null) {
        Popup.PickOption(
          Title: "No recoil chest",
          Intro: "Items can only be recoiled to a manually placed recoil storage chest.\n\n" +
                 "If you don't have one in your inventory either, wish for " +
                 "{{Y|" + IrBlueprintNames.Transmitter + "}} (and don't forget to pick it up!).",
          AllowEscape: true
        );
        return base.HandleEvent(ev);
      }
      if (this.Storage.IsInvalid()) {
        Popup.PickOption(
          Title: "Recoil chest invalid",
          Intro: "Something has gone wrong with the recoil chest placed in the world.\n\n" +
                 "Wish for {{Y|" + IrBlueprintNames.Transmitter + "}} to get a new one and place that.",
          AllowEscape: true
        );
        return base.HandleEvent(ev);
      }

      var transmitter = GameObject.CreateUnmodified(IrBlueprintNames.Transmitter);
      TradeUI.ShowTradeScreen(transmitter, 0.0f, TradeUI.TradeScreenMode.Container);
      while (!transmitter.Inventory.Objects.IsNullOrEmpty()) {
        this.Storage.Inventory.AddObject(transmitter.Inventory.GetFirstObject());
      }
      
      return base.HandleEvent(ev);
    }
  }
}