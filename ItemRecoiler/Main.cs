using System;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
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
      if (ModOptions.GiveOnStartup
          && !player.Inventory.HasObject(IrBlueprintNames.Recoiler)
          && !player.HasPart<IrActivationCommand>()) {
        var recoiler = GameObject.CreateUnmodified(IrBlueprintNames.Recoiler);
        var text = "You feel a slight spacetime disturbance in your immediate vicinity, " +
                   $"and quickly discover {recoiler.a}{recoiler.DisplayName} in your inventory " +
                   "that wasn't there before.";
        player.Inventory.AddObject(recoiler);
        Output.Alert(text);
      }
      player.RequirePart<IrActivationCommand>();
    }

    /// <summary>New game.</summary>
    public void mutate(GameObject player) { Init(player); }

    /// <summary>Load game.</summary>
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
  }
}