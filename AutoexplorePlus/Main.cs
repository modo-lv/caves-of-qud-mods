#nullable enable
using XRL;
using XRL.World;

namespace ModoMods.AutoexplorePlus {
  [HasCallAfterGameLoaded]
  public class Main : IPlayerMutator {
    public static void Register(GameObject? player) {
        player?.RequirePart<IgnoreUnawareHostiles>();
    }

    [CallAfterGameLoaded] public static void AfterLoad() { Register(The.Player); }
    public void mutate(GameObject player) { Register(The.Player); }
  }
}