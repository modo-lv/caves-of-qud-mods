using System;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil.Data;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World;

namespace ModoMods.LootRecoil {
  [HasCallAfterGameLoaded][PlayerMutator][HasWishCommand]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static void Init(GameObject player) {
      if (!player.HasPart<Recoiler>()
          || player.GetPart<Recoiler>()?.Escrow == null
          && !player.Inventory.HasObject(LrBlueprintNames.Storage)) {
        Output.DebugLog($"[{player}] does not appear to own a [{LrBlueprintNames.Storage}], placing in inventory...");
        player.Inventory.AddObject(LrBlueprintNames.Storage);
      }
      player.RequirePart<Recoiler>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) { Init(player); }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }


    [WishCommand("q")]
    public static void Open() {
      TradeUI.ShowTradeScreen(Player.RequirePart<Recoiler>()!.Escrow, 0.0f, TradeUI.TradeScreenMode.Container);
    }
  }
}