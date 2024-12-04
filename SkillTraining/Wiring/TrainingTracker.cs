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
    /// <inheritdoc cref="Points"/>
    public IDictionary<String, Decimal> Points = new Dictionary<String, Decimal>();

    private Boolean _disabledLogged;

    /// <summary>Process a known training action.</summary>
    public void HandleTrainingAction(PlayerAction action, Decimal amountModifier = 1m) {
      switch (ModOptions.TrainingEnabled) {
        case true when this._disabledLogged:
          this._disabledLogged = false;
          break;
        case false when this._disabledLogged:
          return;
        case false: {
          Output.Log(
            "Skill training has been disabled in game options, no points will be earned or skills unlocked."
          );
          this._disabledLogged = true;
          return;
        }
      }

      var amount =
        Math.Max(0.01m, Math.Min(1.0m, TrainingData.For(action).DefaultAmount * amountModifier));
      Output.DebugLog($"Player action: [{action}].");
      this.AddPoints(TrainingData.For(action).SkillClass, amount);
    }

    /// <summary>Increases training point value for a skill (if applicable).</summary>
    public void AddPoints(String skillClass, Decimal amount) {
      amount = Math.Round(amount, 2);
      var skill = SkillUtils.SkillOrPower(skillClass);
      if (amount > 0 && !Main.Player.HasSkill(skillClass)) {
        this.Points.TryAdd(skillClass, 0);
        if (amount > skill.Cost)
          this.Points[skillClass] = amount;
        else
          this.Points[skillClass] += amount;

        if (ModOptions.ShowTraining)
          Output.Message(
            "{{training|Training}}: {{Y|" + skillClass.SkillName() + "}} " +
            $"+ {amount} = " + "{{Y|" + this.Points[skillClass] + "}}."
          );
        Output.DebugLog(
          $"[{skillClass.SkillName()}] + {amount} = {this.Points[skillClass]}",
          inGame: false
        );
      }
      CostModifier.UpdateCost(skillClass, this);
      this.UnlockCompletedSkills();
    }

    /// <summary>Checks all trainable skills and unlocks those whos training is complete.</summary>
    private void UnlockCompletedSkills() {
      (
        from entry in Main.Player.RequirePart<TrainingTracker>().Points
        where CostModifier.RealCosts[entry.Key] <= entry.Value
        select entry.Key
      ).ToList().ForEach(unlocked => {
        var canUnlock = true;
        // Special case - Tactful has a minimum stat requirement
        if (unlocked == SkillClasses.CustomsAndFolklore) {
          canUnlock = SkillUtils.PowerByClass(SkillClasses.Tactful)!.MeetsAttributeMinimum(Main.Player);
        }
        if (!canUnlock)
          return;

        Output.Alert("{{Y|" + unlocked.SkillName() + "}} skill unlocked through practical training!");
        Main.Player.GetPart<Skills>().AddSkill(unlocked);
        Output.DebugLog($"[{unlocked}] added to [{Main.Player}].");
        this.ResetPoints(unlocked);
      });
    }

    /// <summary>Resets training points back to 0.</summary>
    public void ResetPoints(String skillClass) {
      this.Points[skillClass] = 0;
      Output.Log($"[{skillClass}] training reset to 0.");
    }
  }
}