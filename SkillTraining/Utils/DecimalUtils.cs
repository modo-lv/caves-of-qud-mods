using System;

namespace Modo.SkillTraining.Utils {
  public static class DecimalUtils {
    /// <summary>
    /// Interprets an integer as percentage points out of 100, and converts it to the corresponding decimal.
    /// </summary>
    public static Decimal AsPercentage(this Int32 value, Byte decimals = 2) =>
      Math.Round(value / new Decimal(100), decimals); 
  }
}