using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Wiring {
  [HasWishCommand] public class Wishes {
    [WishCommand("SkillTraining")]
    public static void Handle() {
      var choice = 0;
      while (true) {
        choice = Popup.PickOption(
          Title: "Skill Training",
          AllowEscape: true,
          DefaultSelected: choice,
          Options: new[] {
            "\t Progress overview",
            "\t Modify skill training \x10",
          }
        );
        switch (choice) {
          case 0: Overview(); break;
          case 1: ModifyTraining(); break;
          default: return;
        }
      }
    }

    public static void Overview() {
      var output = TrainingData.Data.Values
        .Select(it => it.SkillClass)
        .Distinct()
        .OrderBy(it => it.SkillName())
        .ToDictionary(it => it, it => The.Player.TrainingTracker()!.Points.GetOr(it, () => 0m))
        .Select(entry => {
          var fullCost = CostModifier.RealCosts[entry.Key];
          var cost = fullCost - Convert.ToInt32(entry.Value);
          var locked = !Main.Player.HasSkill(entry.Key);
          var trained = locked && entry.Value >= fullCost;
          var sb = new StringBuilder();
          var color = locked ? (trained ? "&G" : "&y") : "&K";
          sb.Append($"{color}○ {SkillFactory.GetSkillOrPowerName(entry.Key)} ".PadRight(30, '-'));

          // Current points
          sb.Append(" ");
          var value = $"{(locked ? (trained ? "&G" : "&Y") : "&K")}{entry.Value:##0.00;;0}{color}";
          var pad = 10;
          if (entry.Value == 0) {
            sb.Append("{{k|000.0}}" + value);
          } else if (value.Length < pad) {
            sb.Append("{{k|" + ("}}" + value).PadLeft(pad + 2, '0'));
          } else {
            sb.Append(value);
          }

          sb.Append(" /");

          // Cost
          sb.Append(" ");
          value = $"{color}{cost}";
          pad = 5;
          if (value.Length < pad)
            sb.Append("{{k|" + ("}}" + value).PadLeft(pad + 2, '0'));
          else {
            sb.Append(value);
          }

          Output.DebugLog(sb.ToString());
          return sb.ToString();
        })
        .Aggregate((a, b) => $"{a}\n{b}");

      // Regular `Show` doesn't display the title, so use a "choice" without any options instead. 
      Popup.PickOption(
        Title: "Skill training progress",
        Intro: output,
        Options: new List<String>(0),
        AllowEscape: true
      );
    }

    public static void ModifyTraining() {
      var choice = 0;
      while (true) {
        var list = TrainingData.Data.Values
          .Select(it => it.SkillClass)
          .Distinct()
          .OrderBy(it => it.SkillName())
          .ToDictionary(it => it, it => The.Player.TrainingTracker()!.Points.GetOr(it, () => 0m))
          .Where(it => !Main.Player.HasSkill(it.Key))
          .ToList();

        choice = Popup.PickOption(
          Title: "Modify skill training",
          Intro: "Training points can only be modified for skills that haven't been unlocked yet.",
          Options: list
            .Select(it => "{{Y|" + it.Key.SkillName() + "}}" + $" [{it.Value} tp]")
            .ToList(),
          DefaultSelected: choice,
          AllowEscape: true
        );
        if (choice == -1)
          break;
        var target = list[choice];
        var newValueInput = Popup.AskString(
          Message: "Training points for {{Y|" + target.Key + "}} (unlocked skills will remain at 0):",
          Default: $"{target.Value}",
          MaxLength: 6,
          RestrictChars: "0123456789."
        );
        if (newValueInput == $"{target.Value}" || newValueInput.IsNullOrEmpty())
          continue;

        if (!Decimal.TryParse(newValueInput, out var newValue)) {
          Output.Alert("Failed to convert {{W|" + newValueInput + "}} to a decimal number.");
          continue;
        }

        Output.Message(
          "Setting {{Y|" + target.Key.SkillName() + "}} skill training points to " +
          "{{W|" + newValue + "}}..."
        );

        if (newValue > target.Value)
          The.Player.TrainingTracker()!.AddPoints(target.Key, newValue - target.Value);
        else
          The.Player.TrainingTracker()!.Points[target.Key] = newValue;
      }
    }
  }
}