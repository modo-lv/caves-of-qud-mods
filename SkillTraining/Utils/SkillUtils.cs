using System;
using System.Collections.Generic;
using System.Linq;
using XRL;
using XRL.World.Parts.Skill;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Utils {
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
    public static SkillEntry? SkillByClass(String? className) =>
      SkillFactory.Factory.SkillByClass.GetValueOrDefault(className);

    /// <summary>Fetches data for a power.</summary>
    public static PowerEntry? PowerByClass(String className) =>
      SkillFactory.Factory.PowersByClass.GetValueOrDefault(className);

    /// <summary>Fetch the data for a skill/power.</summary>
    public static IBaseSkillEntry SkillOrPower(String className) {
      return SkillByClass(className)?.Generic?.Entry ?? PowerByClass(className)?.Generic?.Entry
        ?? throw new ArgumentException($"Could not find any skill by class name [{className}].");
    }

    /// <summary>Fetches all skill/power instances of a given class.</summary>
    /// <remarks>
    /// At the moment, the only such case is <see cref="Cudgel_ChargingStrike"/>,
    /// which shows up in both Axe and Cudgel trees. But while all detections etc. treat it as a single skill
    /// (including unlocking), it is actually two different instances, each with its own cost.
    /// </remarks>
    public static IEnumerable<IBaseSkillEntry> SkillsOrPowers(String className) {
      return SkillFactory.Factory.SkillList.Values
        .SelectMany(skill => new[] { (IBaseSkillEntry) skill }.Concat(skill.Powers.Values))
        .Where(it => it.Class == className);
    }
  }
}