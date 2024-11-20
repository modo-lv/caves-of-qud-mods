using System;
using SkillTraining.Parts;
using XRL;
using XRL.World;

namespace SkillTraining.Utils {
  /// <summary>
  /// Wrappers with better error messages for accessing nullable, but required properties. 
  /// </summary>
  public static class Req {
    public static GameObject Player => The.Player
                                       ?? throw new NullReferenceException("[The.Player] is null");

    public static PointTracker GetPointTracker(this GameObject player) {
      player.TryGetPart<PointTracker>(out var part);
      return part ?? throw new NullReferenceException($"[{player}] does not have [{nameof(PointTracker)}] part.");
    }
  }
}