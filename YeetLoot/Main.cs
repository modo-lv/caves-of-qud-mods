using System;
using System.Linq;
using ModoMods.Core.Utils;
using Newtonsoft.Json;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World;

namespace ModoMods.LootYeet {
  [HasCallAfterGameLoaded][PlayerMutator][HasWishCommand]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static GameObject? Chest =>
      Player.RequirePart<Yeeter>().Chest;

    public static void Init(GameObject player) {
      if (!player.HasPart<Yeeter>()) {
        var chest = GameObject.CreateUnmodified("ModoMods_LootYeet_Chest");
        player.Inventory.AddObject(chest);
      }
      player.RequirePart<Yeeter>();
    }

    public void mutate(GameObject player) { Init(player); }
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
    
    
    [WishCommand("")]
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