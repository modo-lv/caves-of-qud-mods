using System;
using System.Linq;
using HarmonyLib;
using ModoMods.Core.Utils;
using Newtonsoft.Json;
using UnityEngine;
using Wintellect.PowerCollections;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World;
using GameObject = XRL.World.GameObject;

namespace ModoMods.LootYeet {
  [HarmonyPatch][HasWishCommand]
  public class Yeeter : ModPart {
    [SerializeField] public GameObject? Chest;
    public override Set<Int32> WantEventIds => new Set<Int32> { InventoryActionEvent.ID };

    public override Boolean HandleEvent(InventoryActionEvent ev) {
      Output.DebugLog(ev.Actor);
      Output.DebugLog(ev.Command);
      Output.DebugLog(ev.Item.Blueprint);

      if (ev.Actor.IsPlayer()
          && ev.Command == "CommandDropObject"
          && ev.Item.Blueprint == "ModoMods_LootYeet_Chest") {

        Output.DebugLog($"[{ev.Actor}] dropped [{ev.Item.Blueprint}]");

        this.Chest = ev.Item;
      }
      return base.HandleEvent(ev);
    }

    [WishCommand("w")]
    public static void YeetItems() {
      if (Main.Chest == null) {
        Output.Alert("Chest has not been dropped.");
        return;
      }
      var items = Main.Player.GetInventoryAndEquipmentReadonly().ToList();
      var options = items
        .Select(it => it.GetDisplayName())
        .ToList();
      var choices = Popup.PickSeveral(
        Options: options,
        AllowEscape: true
      );
      Output.DebugLog(JsonConvert.SerializeObject(choices));
      foreach (var (index, amount) in choices) {
        Output.DebugLog($"Choice {index}, amount {amount}, item {items[index]}.");
        items[index].ForceUnequip();
        if (!items[index].IsInGraveyard())
          Main.Chest.Inventory.AddObject(items[index]);
      }
    }
  }
}