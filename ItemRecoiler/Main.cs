using System;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Parts;
using ModoMods.ItemRecoiler.Wiring;
using XRL;
using XRL.Wish;
using XRL.World;

namespace ModoMods.ItemRecoiler {
  [HasCallAfterGameLoaded][PlayerMutator][HasWishCommand]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    /// <summary>Attaches all the necessary mod parts to the main player object.</summary>
    public static void Init(GameObject player) {
      player.RequirePart<ModCommands>();
      player.RequirePart<StartupProvider>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) {
      Output.DebugLog("Game started, wiring up the mod...");
      Init(player);
    }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() {
      Output.DebugLog("Game loaded, verifying all the mod wiring...");
      Init(Player);
    }
  }
}