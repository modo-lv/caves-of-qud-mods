using System;
using System.Linq;

namespace Modo.SkillTraining.Internal {
  public static class StringUtils {
    public static String Repeat(this String value, Int32 count) =>
      Enumerable.Repeat(value, count)
        .Unless(it => it.IsNullOrEmpty())
        ?.Aggregate((a, b) => a + b)
      ?? "";
  }
}