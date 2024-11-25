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


  }
}