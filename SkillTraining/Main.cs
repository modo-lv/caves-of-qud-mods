using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Parts;
using XRL;
using XRL.World;

namespace Modo.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="TrainingTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded] [PlayerMutator]
  public class Main : IPlayerMutator {
    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog(
        $"Game loaded, ensuring that [{nameof(TrainingTracker)}] is attached to [{Req.Player}]..."
      );
      Req.Player.RequirePart<TrainingTracker>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog($"New game started, attaching [{nameof(TrainingTracker)}] to [{player}]...");
      player.RequirePart<TrainingTracker>();
    }
  }
}