using System;

namespace ModoMods.SkillTraining.Utils {
  public static class ScopeFunctions {
    public static TOut Let<TIn, TOut>(this TIn receiver, Func<TIn, TOut> func) {
      return func(receiver);
    }
    
    public static T Also<T>(this T receiver, Action<T> action) {
      action(receiver);
      return receiver;
    }
  }
}