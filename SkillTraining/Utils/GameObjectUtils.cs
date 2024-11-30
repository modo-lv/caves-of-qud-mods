using System;
using ModoMods.Core.Data;
using XRL.World;
using XRL.World.Parts.Mutation;

namespace ModoMods.SkillTraining.Utils {
  public static class GameObjectUtils {
    /// <summary>
    /// Determines if <paramref name="obj"/> counts as a combatant for the purposes of player skill training.
    /// </summary>
    public static Boolean IsCombatant(this GameObject? obj) =>
      // Destroying inanimate objects isn't combat
      obj is { IsCreature: true }
      // Attacking self isn't combat
      && !obj.IsPlayer()
      // Creatures can also be walls (e.g. some fungi)
      && !obj.HasTag(TagNames.Wall)
      // Fists, fangs etc. count as weapons. Dreadroots are weaponless.
      && obj.HasWeapon()
      // SporePuffer mutation consumes 1000 energy every turn, regardless of whether the spores were released.
      // As a result, creatures with this mutation (some fungi) never get to do anything else. 
      && !obj.HasPart<SporePuffer>();
  }
}