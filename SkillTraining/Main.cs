using System;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Trainers;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="PointTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    /// <summary>A non-null reference to the main player body.</summary>
    /// <exception cref="NullReferenceException"></exception>
    public static GameObject Player => The.Player ?? throw new NullReferenceException("[The.Player] is null");


    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog($"Game loaded, ensuring that training parts are attached to [{Player}]...");
      Player.RequirePart<CookingTrainer>();
      Player.RequirePart<CustomsTrainer>();
      Player.RequirePart<PointTracker>();
      Player.RequirePart<ShieldTrainer>();
      Player.RequirePart<SnakeOilerTrainer>();
      Player.RequirePart<SwimmingTrainer>();
      Player.RequirePart<TinkeringTrainer>();
      Player.RequirePart<WayfaringTrainer>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog($"New game started, attaching training parts to [{player}]...");
      player.RequirePart<CookingTrainer>();
      player.RequirePart<CustomsTrainer>();
      player.RequirePart<PointTracker>();
      player.RequirePart<ShieldTrainer>();
      player.RequirePart<SnakeOilerTrainer>();
      player.RequirePart<SwimmingTrainer>();
      player.RequirePart<TinkeringTrainer>();
      player.RequirePart<WayfaringTrainer>();
    }
  }
}