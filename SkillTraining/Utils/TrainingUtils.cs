using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Wiring;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts.Mutation;

namespace ModoMods.SkillTraining.Utils {
  public static class TrainingUtils {
    /// <summary>Training point tracker associated with a game object.</summary>
    /// <remarks>
    /// If the object is capable of skill training, but doesn't have all the necessary parts,
    /// those will be created and attached. 
    /// </remarks>
    public static TrainingTracker? TrainingTracker(this GameObject? gameObject) {
      if (gameObject == null || !gameObject.CanTrainSkills()) {
        // Just in case we have somehow attached training parts to something that can't actually train.
        if (gameObject != null && !gameObject.HasEffect<Dominated>()) {
          Output.DebugLog(
            $"Tried to access training tracker on [{gameObject}], which isn't capable of training. " +
            "Removing training parts (if any) instead."
          );
          Main.Unregister(gameObject);
        }
        return null;
      }
      return gameObject.RequirePart<TrainingTracker>();
    }
    
    /// <summary>Determines if this game object is one that is capable of skill training.</summary>
    public static Boolean CanTrainSkills(this GameObject? gameObject) =>
      gameObject != null
      && gameObject.IsPlayer()
      && !gameObject.HasEffect<Dominated>();

    /// <summary>
    /// Determines if this game object counts as a combatant for the purposes of player skill training.
    /// </summary>
    public static Boolean IsCombatant(this GameObject? gameObject) =>
      // Destroying inanimate objects isn't combat
      gameObject is { IsCreature: true }
      // Attacking self isn't combat
      && !gameObject.IsPlayer()
      // Some walls are also creatures (fungi), but they don't fight back.
      && !gameObject.HasTag(QudTagNames.Wall)
      // Fists, fangs etc. count as weapons. Dreadroots are weaponless.
      && gameObject.HasWeapon()
      // SporePuffer mutation consumes 1000 energy every turn, regardless of whether the spores were released.
      // As a result, creatures with this mutation (some fungi) can never engage in (direct) combat. 
      && !gameObject.HasPart<SporePuffer>();
  }
}