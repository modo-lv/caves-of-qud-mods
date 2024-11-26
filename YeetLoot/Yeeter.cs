using System;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.LootYeet.Data;
using Newtonsoft.Json;
using Wintellect.PowerCollections;
using XRL;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;
using EventNames = ModoMods.Core.Data.EventNames;
using GameObject = XRL.World.GameObject;

namespace ModoMods.LootYeet {
  [Serializable]
  public class Yeeter : ModPart {
    public GameObject? Chest;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      CommandEvent.ID,
      GenericCommandEvent.ID,
      CommandTakeActionEvent.ID,
      InventoryActionEvent.ID,
    };

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (!ev.Actor.IsPlayer()
          || ev.Command != EventNames.CommandDropObject
          || ev.Item.Blueprint != BlueprintNames.YeetLootChest)
        return base.HandleEvent(ev);

      this.Chest = ev.Item;
      Output.DebugLog($"[{ev.Actor}] dropped [{ev.Item.Blueprint}], loot yeeting now possible.");

      return base.HandleEvent(ev);
    }

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != YlEventNames.YeetLoot)
        return base.HandleEvent(ev);
      if (this.Chest == null) {
        Popup.PickOption(
          Title: "No loot chest",
          Intro: "Items can only be yeeted to a loot chest that's been placed somewhere.\n\n" +
                 "If you don't have the chest in your inventory either, wish for " +
                 "{{Y|" + BlueprintNames.YeetLootChest + "}} to correct this.",
          AllowEscape: true
        );
        return base.HandleEvent(ev);
      }
      if (this.Chest.IsInvalid()) {
        Popup.PickOption(
          Title: "Loot chest invalid",
          Intro: "Something has gone wrong with the chest placed in the world.\n\n" +
                 "Wish for {{Y|" + BlueprintNames.YeetLootChest + "}} to get a new one and use that.",
          AllowEscape: true
        );
        return base.HandleEvent(ev);
      }

      var temp = GameObject.CreateUnmodified("Chest");
      temp.DisplayName = "extradimensional yeeter";
      TradeUI.ShowTradeScreen(temp, 0.0f, TradeUI.TradeScreenMode.Container);
      while (!temp.Inventory.Objects.IsNullOrEmpty()) {
        this.Chest.Inventory.AddObject(temp.Inventory.GetFirstObject());
      }
      
      return base.HandleEvent(ev);
    }
  }
}