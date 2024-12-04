using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Class responsible for modifying skill prices depending on training.</summary>
  public static class CostModifier {
    public static Boolean Disabled => !ModOptions.ModifyCosts;

    public static readonly IDictionary<String, Int32> RealCosts =
      SkillFactory.GetSkills().ToDictionary(it => it.Class, it => it.Cost).Also(list => {
        SkillFactory.GetPowers().ForEach(power => list.TryAdd(power.Class, power.Cost));
      });

    public static void UpdateCost(String skillClass, TrainingTracker training) {
      if (Disabled) return;
      SkillFactory.GetSkills().FirstOrDefault(it => it.Class == skillClass)?.Also(skill => {
        var reduction = Convert.ToInt32(Math.Floor(training.Points.GetOr(skillClass, () => 0m)));
        skill.Cost = RealCosts[skillClass] - reduction;
        Output.DebugLog($"Skill [{skillClass}] cost - {reduction} = {skill.Cost}.");
      });
    }

    public static void ResetSkills() {
      if (Disabled) return;
      var total = 0;
      SkillFactory.GetSkills().ForEach(it => {
        it.Cost = RealCosts[it.Class];
        total++;
      });
      SkillFactory.GetPowers().ForEach(it => {
        it.Cost = RealCosts[it.Class];
        total++;
      });
      Output.DebugLog($"{total} skill and power costs reset to defaults.");
    }
  }
}