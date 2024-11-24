using System;
using System.Collections.Generic;
using System.Linq;

namespace Modo.SkillTraining.Utils {
  public static class DictionaryUtils {
    public static TValue? GetOr<TKey, TValue>(
      this IDictionary<TKey, TValue> dictionary,
      TKey key,
      Func<TValue> fallback
    ) =>
      dictionary.ContainsKey(key)
        ? dictionary[key]
        : fallback();

    /// <summary>
    /// Parameter-less conversion of any collection of key-value pairs to a dictionary. 
    /// </summary>
    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> pairs
    ) =>
      pairs.ToDictionary(it => it.Key, it => it.Value);
  }
}