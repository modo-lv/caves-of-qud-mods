using System;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Trainers;
using ModoMods.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="PointTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static PointTracker PointTracker => Player.RequirePart<PointTracker>();

    /// <summary>Attaches all the necessary mod parts to the main player object.</summary>
    public static void Init(GameObject player) {
      player.RequirePart<PointTracker>();
      player.RequirePart<Commands>();

      player.RequirePart<MeleeWeaponTrainer>();
      player.RequirePart<MissileAttackTrainer>();

      player.RequirePart<CookingTrainer>();
      player.RequirePart<CustomsTrainer>();
      player.RequirePart<DeftThrowingTrainer>();
      player.RequirePart<PhysicTrainer>();
      player.RequirePart<ShieldTrainer>();
      player.RequirePart<SnakeOilerTrainer>();
      player.RequirePart<SwimmingTrainer>();
      player.RequirePart<TinkeringTrainer>();
      player.RequirePart<WayfaringTrainer>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog("Game started, wiring up the mod...");
      Init(player);
    }

    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog("Game loaded, verifying all the mod wiring...");
      Init(Player);
    }
  }
}