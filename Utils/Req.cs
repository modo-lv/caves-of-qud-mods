using System;
using XRL;
using XRL.World;

namespace SkillTraining.Utils {
  /// <summary>
  /// Wrappers with better error messages for accessing nullable, but required properties. 
  /// </summary>
  public static class Req {
    public static GameObject Player => The.Player
                                       ?? throw new NullReferenceException("[The.Player] is null");
  }
}