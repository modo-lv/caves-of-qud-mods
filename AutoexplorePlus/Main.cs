using XRL;
using XRL.World;
using XRL.World.Parts.Mutation;

namespace ModoMods.AutoexplorePlus {
  [HasCallAfterGameLoaded]
  public class Main : IPlayerMutator {
    public static void Register(GameObject? player) {
      if (player?.HasPart<HeightenedHearing>() == true || player?.HasPart<HeightenedSmell>() == true)
        player.RequirePart<IgnoreUnawareHostiles>();
    }

    [CallAfterGameLoaded] public static void AfterLoad() { Register(The.Player); }
    public void mutate(GameObject player) { Register(The.Player); }
  }
}