using System;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using XRL;
using XRL.World;

namespace ModoMods.ItemRecoiler {
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static void Init(GameObject player) {
      if (!player.HasPart<Recoiler>()
          || player.GetPart<Recoiler>()?.Storage == null
          && !player.Inventory.HasObject(IrBlueprintNames.Storage)) {
        Output.DebugLog($"[{player}] does not appear to own a [{IrBlueprintNames.Storage}], placing in inventory...");
        player.Inventory.AddObject(IrBlueprintNames.Storage);
      }
      player.RequirePart<Recoiler>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) { Init(player); }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
  }
}