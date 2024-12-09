using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Trainers;
using ModoMods.SkillTraining.Utils;
using ModoMods.SkillTraining.Wiring;
using XRL;
using XRL.World;
using XRL.World.Parts.Skill;
using XRL.World.Skills;
using ModCommands = ModoMods.SkillTraining.Wiring.ModCommands;

namespace ModoMods.SkillTraining {
  /// <summary>Main "entry point" for the mod functionality.</summary>
  /// <remarks>Attaches the <see cref="TrainingTracker"/> part to the player object.</remarks>
  [HasCallAfterGameLoaded][PlayerMutator]
  public class Main : IPlayerMutator {
    public static GameObject Player =>
      The.Player ?? throw new NullReferenceException("[The.Player] is null.");

    /// <summary>
    /// All trainable skills and their current training points (or -1 if called without a current player).
    /// </summary>
    public static IDictionary<String, Decimal> AllTrainableSkills =>
      TrainingData.Data.Values
        .Select(it => it.SkillClass)
        .Distinct()
        .ToDictionary(it => it, it => The.Player.Training()?.GetPoints(it) ?? -1m);


    /// <summary>Attaches all the necessary mod parts to the main player object.</summary>
    public static void Register(GameObject gameObject) {
      gameObject.RequirePart<TrainingTracker>();
      gameObject.RequirePart<ModCommands>();
      gameObject.RequirePart<LevelUpModifier>();

      gameObject.RequirePart<MeleeWeaponTrainer>();
      gameObject.RequirePart<MissileAttackTrainer>();

      gameObject.RequirePart<AxeTrainer>();
      gameObject.RequirePart<CookingTrainer>();
      gameObject.RequirePart<CustomsTrainer>();
      gameObject.RequirePart<ThrowingTrainer>();
      gameObject.RequirePart<DodgeTrainer>();
      gameObject.RequirePart<EnduranceTrainer>();
      gameObject.RequirePart<PhysicTrainer>();
      gameObject.RequirePart<SelfDisciplineTrainer>();
      gameObject.RequirePart<ShieldTrainer>();
      gameObject.RequirePart<SnakeOilerTrainer>();
      gameObject.RequirePart<TacticsTrainer>();
      gameObject.RequirePart<TinkeringTrainer>();
      gameObject.RequirePart<WayfaringTrainer>();
    }

    /// <summary>Remove all training-related parts form a game object.</summary>
    public static void Unregister(GameObject? gameObject) {
      gameObject?.RemovePart<TrainingTracker>();
      gameObject?.RemovePart<ModCommands>();
      gameObject?.RequirePart<LevelUpModifier>();

      gameObject?.RemovePart<MeleeWeaponTrainer>();
      gameObject?.RemovePart<MissileAttackTrainer>();

      gameObject?.RemovePart<AxeTrainer>();
      gameObject?.RemovePart<CookingTrainer>();
      gameObject?.RemovePart<CustomsTrainer>();
      gameObject?.RemovePart<ThrowingTrainer>();
      gameObject?.RemovePart<DodgeTrainer>();
      gameObject?.RemovePart<EnduranceTrainer>();
      gameObject?.RemovePart<PhysicTrainer>();
      gameObject?.RemovePart<SelfDisciplineTrainer>();
      gameObject?.RemovePart<ShieldTrainer>();
      gameObject?.RemovePart<SnakeOilerTrainer>();
      gameObject?.RemovePart<TacticsTrainer>();
      gameObject?.RemovePart<TinkeringTrainer>();
      gameObject?.RemovePart<WayfaringTrainer>();
    }

    public void mutate(GameObject player) {
      Output.DebugLog("Game started, wiring up the training parts...");
      CostModifier.ResetSkillCosts();
      Register(player);
    }

    [CallAfterGameLoaded]
    public static void OnGameLoaded() {
      Output.DebugLog("Game loaded, checking the training part wiring...");
      CostModifier.ResetSkillCosts();
      Register(Player);
    }
  }
}