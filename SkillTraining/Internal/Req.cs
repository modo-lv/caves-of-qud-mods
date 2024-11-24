using System;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Internal {
  /// <summary>
  /// Wrappers with better error messages for accessing nullable, but required properties. 
  /// </summary>
  public static class Req {

    public static PointTracker PointTracker {
      get {
        Player.TryGetPart<PointTracker>(out var part);
        return part
               ?? throw new NullReferenceException(
                 $"[{Player}] does not have [{nameof(SkillTraining.PointTracker)}] part."
               );
      }
    }
  }
}