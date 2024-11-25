using System;
using ModoMods.Core.Utils;
using XRL;
using XRL.World;
using static ModoMods.LootYeet.Data.BlueprintNames;

namespace ModoMods.LootYeet {
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static void Init(GameObject player) {
      if (!player.HasPart<Yeeter>()
          || player.GetPart<Yeeter>()?.Chest == null
          && !player.Inventory.HasObject(YeetLootChest)) {
        Output.DebugLog($"[{player}] does not appear to own a [{YeetLootChest}], placing in inventory...");
        player.Inventory.AddObject(YeetLootChest);
      }
      player.RequirePart<Yeeter>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) { Init(player); }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
  }
}