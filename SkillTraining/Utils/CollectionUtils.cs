using System;
using System.Collections.Generic;
using System.Linq;

namespace ModoMods.SkillTraining.Utils {
  public static class CollectionUtils {
    /// <summary>Determines if the receiver is present in a collection.</summary>
    public static Boolean IsOneOf<T>(this T item, IEnumerable<T> collection) => 
      collection.Contains(item);
  }
}