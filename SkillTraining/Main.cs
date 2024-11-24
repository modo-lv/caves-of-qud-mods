using System;
using Modo.SkillTraining.Trainers;
using Modo.SkillTraining.Utils;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="PointTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player => 
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static PointTracker PointTracker => Player.RequirePart<PointTracker>();
    
    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog($"Game loaded, ensuring that training parts are attached to [{Player}]...");
      Player.RequirePart<PointTracker>();
      
      Player.RequirePart<CookingTrainer>();
      Player.RequirePart<CustomsTrainer>();
      Player.RequirePart<MeleeWeaponTrainer>();
      Player.RequirePart<PointTracker>();
      Player.RequirePart<ShieldTrainer>();
      Player.RequirePart<SnakeOilerTrainer>();
      Player.RequirePart<SwimmingTrainer>();
      Player.RequirePart<TinkeringTrainer>();
      Player.RequirePart<WayfaringTrainer>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog($"New game started, attaching training parts to [{player}]...");
      player.RequirePart<PointTracker>();

      player.RequirePart<CookingTrainer>();
      player.RequirePart<CustomsTrainer>();
      player.RequirePart<MeleeWeaponTrainer>();
      player.RequirePart<PointTracker>();
      player.RequirePart<ShieldTrainer>();
      player.RequirePart<SnakeOilerTrainer>();
      player.RequirePart<SwimmingTrainer>();
      player.RequirePart<TinkeringTrainer>();
      player.RequirePart<WayfaringTrainer>();
    }
  }
}