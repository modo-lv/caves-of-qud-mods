using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Class responsible for modifying skill prices depending on training.</summary>
  public static class CostModifier {
    public static Boolean Disabled => !(The.Player.Training()?.ModifyCosts ?? ModOptions.ModifyCosts);

    public static readonly IDictionary<String, Int32> RealCosts =
      SkillFactory.GetSkills().ToDictionary(it => it.Class, it => it.Cost).Also(list => {
        SkillFactory.GetPowers().ForEach(power => list.TryAdd(power.Class, power.Cost));
      });

    public static void UpdateCost(String skillClass, TrainingTracker training) {
      if (Disabled) return;
      // At the moment we treat multiple instances of the same skill class as a single skill.
      // There is only one such case at the moment (`Cudgel_ChargingStrike`),
      // and the game treats them as a single skill.
      SkillUtils.SkillsOrPowers(skillClass).Do(skill => {
        var reduction = Convert.ToInt32(Math.Floor(training.GetPoints(skillClass)));
        skill.Cost = RealCosts[skillClass] - reduction;
        Output.DebugLog($"Skill [{skillClass}] cost - {reduction} = {skill.Cost}.");
      });
    }

    public static void ResetSkillCosts() {
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
      if (Disabled) return;
      The.Player.Training()?.Also(tracker => {
        foreach (var skill in Main.AllTrainableSkills)
          UpdateCost(skill.Key, tracker);
      });
    }
  }
}