using System;
using System.Collections.Generic;
using XRL;
using XRL.World.Skills;

namespace Modo.SkillTraining.Internal {
  public static class SkillUtils {
    /// <summary>
    /// Gets the display name (<c>"Long Blades"</c>) of a skill class (<c>"LongBlades"</c>).
    /// </summary>
    /// <remarks>
    /// Weirdly, <see cref="SkillFactory.SkillList"/> is also indexed by the display name,
    /// rather than skill class.
    /// </remarks>
    public static String SkillName(this String skillClass) =>
      SkillFactory.GetSkillOrPowerName(skillClass);

    /// <summary>Fetches data for a skill.</summary>
    public static SkillEntry? SkillByClass(String className) =>
      SkillFactory.Factory.SkillByClass.GetValueOrDefault(className);

    /// <summary>Fetches data for a power.</summary>
    public static PowerEntry? PowerByClass(String className) =>
      SkillFactory.Factory.PowersByClass.GetValueOrDefault(className);

    /// <summary>Fetch the data for a skill/power.</summary>
    public static IBaseSkillEntry SkillOrPower(String className) {
      return SkillByClass(className)?.Generic?.Entry ?? PowerByClass(className)?.Generic?.Entry
        ?? throw new ArgumentException($"Could not find any skill by class name [{className}].");
    }
  }
}