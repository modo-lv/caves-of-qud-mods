using System;
using System.Reflection;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Trainers;
using ModoMods.SkillTraining.Wiring;
using XRL;
using XRL.World;
using XRL.World.Skills;

namespace ModoMods.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="TrainingTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    public static TrainingTracker TrainingTracker => Player.RequirePart<TrainingTracker>();

    /// <summary>Attaches all the necessary mod parts to the main player object.</summary>
    public static void Register(GameObject gameObject) {
      gameObject.RequirePart<TrainingTracker>();
      gameObject.RequirePart<ModCommands>();

      gameObject.RequirePart<MeleeWeaponTrainer>();
      gameObject.RequirePart<MissileAttackTrainer>();

      gameObject.RequirePart<CookingTrainer>();
      gameObject.RequirePart<CustomsTrainer>();
      gameObject.RequirePart<DeftThrowingTrainer>();
      gameObject.RequirePart<DodgeTrainer>();
      gameObject.RequirePart<PhysicTrainer>();
      gameObject.RequirePart<SelfDisciplineTrainer>();
      gameObject.RequirePart<ShieldTrainer>();
      gameObject.RequirePart<SnakeOilerTrainer>();
      gameObject.RequirePart<SwimmingTrainer>();
      gameObject.RequirePart<TinkeringTrainer>();
      gameObject.RequirePart<WayfaringTrainer>();
      
      CostModifier.ResetSkills();
    }

    /// <summary>Remove all training-related parts form a game object.</summary>
    public static void Unregister(GameObject? gameObject) {
      gameObject?.RemovePart<TrainingTracker>();
      gameObject?.RemovePart<ModCommands>();

      gameObject?.RemovePart<MeleeWeaponTrainer>();
      gameObject?.RemovePart<MissileAttackTrainer>();

      gameObject?.RemovePart<CookingTrainer>();
      gameObject?.RemovePart<CustomsTrainer>();
      gameObject?.RemovePart<DodgeTrainer>();
      gameObject?.RemovePart<DeftThrowingTrainer>();
      gameObject?.RemovePart<PhysicTrainer>();
      gameObject?.RemovePart<SelfDisciplineTrainer>();
      gameObject?.RemovePart<ShieldTrainer>();
      gameObject?.RemovePart<SnakeOilerTrainer>();
      gameObject?.RemovePart<SwimmingTrainer>();
      gameObject?.RemovePart<TinkeringTrainer>();
      gameObject?.RemovePart<WayfaringTrainer>();
      
      CostModifier.ResetSkills();
    }

    public void mutate(GameObject player) {
      Output.DebugLog("Game started, wiring up the training parts...");
      Register(player);
    }

    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog("Game loaded, checking the training part wiring...");
      Register(Player);
    }
  }
}