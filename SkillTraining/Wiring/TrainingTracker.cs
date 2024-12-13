using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Skills = XRL.World.Parts.Skills;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Main component that tracks training points for trainable skills.</summary>
  [Serializable] public class TrainingTracker : ModPart {
    public IDictionary<String?, Decimal> Points = new Dictionary<String?, Decimal>();
    /// <summary>
    /// Tracks on/off toggles for each skill. 
    /// </summary>
    public IDictionary<String?, Boolean?> Enabled = new Dictionary<String?, Boolean?>();

    public Boolean? ModifyCostsOverride;
    public LevelUpSkillPoints? LevelUpSkillPointsOverride;

    public Boolean ModifyCosts =>
      this.ModifyCostsOverride ?? ModOptions.ModifyCosts;
    public LevelUpSkillPoints LevelUpSkillPoints =>
      this.LevelUpSkillPointsOverride ?? ModOptions.LevelUpSkillPoints;

    /// <summary>Process a known training action.</summary>
    public void HandleTrainingAction(PlayerAction action, Decimal amountModifier = 1m) {
      var trainingData = TrainingData.For(action);
      if (!(this.Enabled.GetOr(trainingData.SkillClass, default(Boolean?)) ?? ModOptions.TrainingEnabled))
        return;
      var amount =
        Math.Max(0.01m, trainingData.DefaultAmount * amountModifier);
      Output.DebugLog($"Player action: [{action}].");
      this.AddPoints(trainingData.SkillClass, amount);
    }

    public Decimal GetPoints(String? skillClass) {
      this.Points.TryAdd(skillClass, 0);
      return this.Points[skillClass];
    }

    /// <summary>Increases training point value for a skill (if applicable).</summary>
    public void AddPoints(String? skillClass, Decimal amount) {
      amount = Math.Round(amount, 2);
      if (this.SetPoints(skillClass, this.GetPoints(skillClass) + amount, false)) {
        if (ModOptions.ShowTraining)
          Output.Message(
            "{{training|Training}}: {{Y|" + skillClass.SkillName() + "}} " +
            $"+ {amount} = " + "{{Y|" + this.GetPoints(skillClass) + "}}."
          );
        else
          Output.DebugLog(
            $"[{skillClass.SkillName()}] + {amount} = {this.GetPoints(skillClass)}",
            inGame: false
          );
        CostModifier.UpdateCost(skillClass, this);
        this.UnlockCompletedSkills();
        if (this.GetPoints(skillClass) >= CostModifier.RealCosts[skillClass]
            && !this.ParentObject.HasSkill(skillClass)
           ) {
          Output.Alert("{{O|" + skillClass.SkillName() + "}} skill has been fully trained, " +
                       "but unlocking requirements for it aren't met yet.");
        }
      }
    }

    public Boolean SetPoints(String? skillClass, Decimal newValue, Boolean postProcess = true) {
      if (this.ParentObject.HasSkill(skillClass))
        return false;
      newValue = Math.Min(newValue, CostModifier.RealCosts[skillClass]);
      this.Points[skillClass] = newValue;
      if (postProcess) {
        CostModifier.UpdateCost(skillClass, this);
        this.UnlockCompletedSkills();
      }
      return true;
    }

    /// <summary>Checks all trainable skills and unlocks those whose training is complete.</summary>
    private void UnlockCompletedSkills() {
      (
        from entry in this.Points
        where CostModifier.RealCosts[entry.Key] <= entry.Value
        select entry.Key
      ).ToList().ForEach(completed => {
        if (SkillUtils.PowerByClass(completed)?.MeetsAttributeMinimum(this.ParentObject) == false)
          return;

        Output.Alert("{{G|" + completed.SkillName() + "}} skill unlocked through practical training!");
        this.ParentObject.GetPart<Skills>().AddSkill(completed);
        Output.DebugLog($"[{completed}] added to [{this.ParentObject}].");
        this.ResetPoints(completed);
      });
    }

    /// <summary>Resets training points back to 0.</summary>
    public void ResetPoints(String? skillClass) {
      this.Points[skillClass] = 0;
      Output.DebugLog($"[{skillClass}] training reset to 0.");
    }
  }
}