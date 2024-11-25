using System;
using XRL;
using XRL.World;

namespace ModoMods.LootYeet {
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static GameObject? Chest =>
      Player.RequirePart<Yeeter>().Chest;

    public static void Init(GameObject player) {
        /*
        var chest = GameObject.CreateUnmodified("ModoMods_LootYeet_Chest");
        player.Inventory.AddObject(chest);
    */
        player.RequirePart<Yeeter>();
    }

    public void mutate(GameObject player) { Init(player); }
    [CallAfterGameLoaded] public static void OnGameLoaded() { Init(Player); }
  }
}