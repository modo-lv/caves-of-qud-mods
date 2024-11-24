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
      Output.DebugLog($"Game loaded, ensuring that training parts are attached to [{Req.Player}]...");
      Req.Player.RequirePart<CookingTrainer>();
      Req.Player.RequirePart<CustomsTrainer>();
      Req.Player.RequirePart<TrainingTracker>();
      Req.Player.RequirePart<SnakeOilerTrainer>();
      Req.Player.RequirePart<SwimmingTrainer>();
      Req.Player.RequirePart<WayfaringTrainer>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog($"New game started, attaching training parts to [{player}]...");
      player.RequirePart<CookingTrainer>();
      player.RequirePart<CustomsTrainer>();
      player.RequirePart<TrainingTracker>();
      player.RequirePart<SnakeOilerTrainer>();
      player.RequirePart<SwimmingTrainer>();
      player.RequirePart<WayfaringTrainer>();
    }
  }
}