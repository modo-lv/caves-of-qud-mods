using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Qud.UI;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World.Skills;

namespace ModoMods.SkillTraining.Wiring {
  [HasWishCommand]
  public class ModMenu {
    static TrainingTracker Tracker => The.Player.TrainingTracker()!;

    [WishCommand("SkillTraining")]
    public static void Show() {
      var selectedIndex = 0;
      while (selectedIndex != -1) {
        var trainingList = Main.AllTrainableSkills.ToList();
        var list = trainingList.Select(entry => {
          var fullCost = CostModifier.RealCosts[entry.Key];
          var cost = fullCost - Convert.ToInt32(entry.Value);
          var locked = !Main.Player.HasSkill(entry.Key);
          var trained = locked && entry.Value >= fullCost;
          var sb = new StringBuilder();
          var color = locked ? (trained ? "&G" : "&y") : "&K";
          sb.Append(OnOff(entry.Key) + $"{color} {entry.Key.SkillName()} ".PadRight(30, '-'));

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
        }).ToList();

        selectedIndex = Popup.PickOption(
          Title: "Skill training",
          Options: list,
          AllowEscape: true,
          DefaultSelected: selectedIndex
        );

        if (selectedIndex >= 0) {
          if (The.Player?.HasSkill(trainingList[selectedIndex].Key) != false) {
            Popup.ShowBlock(
              Message: "Training is not applicable to unlocked skills.",
              LogMessage: false
            );
            continue;
          }
          SubMenu(trainingList[selectedIndex]);
        }
      }
    }

    public static void SubMenu(KeyValuePair<String, Decimal> entry) {
      var selectedIndex = 0;
      while (selectedIndex != -1) {
        var options = new[] {
          "Toggle training \x10 " + OnOff(entry.Key),
          "Modify points \x10 ",
        };
        selectedIndex = Popup.PickOption(
          Title: entry.Key.SkillName(),
          Options: options,
          DefaultSelected: selectedIndex,
          AllowEscape: true
        );

        if (selectedIndex == 0) {
          Tracker.Enabled[entry.Key] = Tracker.Enabled[entry.Key] switch {
            null => true,
            true => false,
            _ => null
          };
        } else if (selectedIndex == 1) {
          var newValueInput = Popup.AskString(
            Message: "Set training points for {{Y|" + entry.Key.SkillName() + "}}:",
            Default: $"{entry.Value}",
            MaxLength: 6,
            RestrictChars: "0123456789."
          );
          if (newValueInput == $"{entry.Value}" || newValueInput.IsNullOrEmpty())
            continue;

          if (!Decimal.TryParse(newValueInput, out var newValue)) {
            Output.Alert("{{W|" + newValueInput + "}} isn't a valid decimal number.");
            continue;
          }

          Output.Message(
            "Changing {{Y|" + entry.Key.SkillName() + "}} skill training points to " +
            "{{B|" + newValue + "}}..."
          );

          Tracker.SetPoints(entry.Key, newValue);
        }
      }
    }

    public static String OnOff(String skillClass) {
      Tracker.Enabled.TryAdd(skillClass, null);
      var isSet = true;
      var value = Tracker.Enabled[skillClass] ?? ModOptions.TrainingEnabled.Also(_ => {
        isSet = false;
      });

      if (The.Player?.HasSkill(skillClass) != false) {
        return "{{k|[---]}}";
      }

      var bColor = isSet ? "&y" : "&K";
      var color = isSet ? (value ? "&G" : "&R") : "";

      return bColor + "[" + color + (value ? "ON" : "OFF").PadLeft(3, ' ') + bColor + "]&y";
    }
  }
}