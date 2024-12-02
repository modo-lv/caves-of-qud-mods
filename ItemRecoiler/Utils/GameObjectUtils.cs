using System;
using XRL.World;

namespace ModoMods.ItemRecoiler.Utils {
  public static class GameObjectUtils {
    /// <summary>Stores a boolean value as a 1/0 integer property on a game object.</summary>
    public static void SetBooleanProperty(this GameObject gameObject, String name, Boolean value) {
      gameObject.SetIntProperty(name, value ? 1 : 0);
    }
    /// <summary>
    /// Fetches a game object's integer property and converts it to a boolean (0/*).
    /// </summary>
    public static Boolean GetBooleanProperty(this GameObject gameObject, String name) {
      return gameObject.GetIntProperty(name) != 0;
    }
  }
}