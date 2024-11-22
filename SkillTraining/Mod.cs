using SkillTraining.Internal;
using SkillTraining.Parts;
using XRL;
using XRL.World;

namespace SkillTraining {
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Mod : IPlayerMutator {
    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog($"Game loaded, ensuring that [{nameof(PointTracker)}] part is attached to [{Req.Player}]...");
      Req.Player.RequirePart<PointTracker>();
    }
    
    public void mutate(GameObject player) {
      Output.DebugLog($"New game started, attaching [{nameof(PointTracker)}] part to [{player}]...");
      player.RequirePart<PointTracker>();
    }
  }
}