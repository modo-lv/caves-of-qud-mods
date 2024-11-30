using System;
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

    public static void Init(GameObject player) {
      player.RequirePart<Commands>();
      player.RequirePart<StartupProvider>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) { Init(player); }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
  }
}