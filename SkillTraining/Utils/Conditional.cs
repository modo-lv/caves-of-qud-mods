using System;

namespace SkillTraining.Utils {
  public static class Conditional {
    /// <summary>
    /// Return <paramref name="value"/> if <paramref name="predicate"/> returns <c>false</c>,
    /// or <c>null</c> if it returns <c>true</c>.
    /// </summary>
    public static T? Unless<T>(this T? value, Predicate<T?> predicate) where T : class => 
      predicate(value) ? null : value;
  }
}