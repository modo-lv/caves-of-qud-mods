using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using Qud.UI;
using UnityEngine.UIElements;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World.Skills;
using static ModoMods.SkillTraining.Data.ModEventNames;

namespace ModoMods.SkillTraining.Wiring {
  [HasWishCommand]
  public class ModMenu {
    static TrainingTracker Tracker => The.Player.TrainingTracker()!;

    [WishCommand("SkillTraining")]
    public static void Show() {
      var selectedIndex = 0;
      var buttons = new QudMenuItem[] {
        new QudMenuItem {
          text = "{{W|[" + ControlManager.getCommandInputFormatted(SettingsCommand) + "]}} {{y|Settings}}",
          command = "option:-2",
          hotkey = SettingsCommand,
        }
      };
      while (selectedIndex != -1) {
        var trainingList = Main.AllTrainableSkills.ToList();
        var list = trainingList.Select(entry => {
          var fullCost = CostModifier.RealCosts[entry.Key];
          var cost = CostModifier.Disabled ? fullCost : fullCost - Convert.ToInt32(Math.Floor(entry.Value));
          var locked = !Main.Player.HasSkill(entry.Key);
          var trained = locked && entry.Value >= fullCost;
          var sb = new StringBuilder();
          var color = locked ? (trained ? "&G" : "&y") : "&K";
          sb.Append(SkillToggleStatus(entry.Key) + $"{color} {entry.Key.SkillName()} ".PadRight(30, '-'));

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

          sb.Append(" " + (CostModifier.Disabled ? "/" : "\x1A"));

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
          DefaultSelected: selectedIndex,
          Buttons: buttons
        );

        switch (selectedIndex) {
          case >= 0 when The.Player?.HasSkill(trainingList[selectedIndex].Key) != false:
            Popup.ShowBlock(
              Message: "Training is not applicable to unlocked skills.",
              LogMessage: false
            );
            continue;
          case >= 0:
            SkillMenu(trainingList[selectedIndex]);
            break;
          case -2:
            SettingsMenu();
            break;
        }
      }
    }

    public static void SettingsMenu() {
      var selectedIndex = 0;
      while (selectedIndex != -1) {
        var options = new[] {
          "Reduce skill costs \x10 " + ToggleStatus(Tracker.ModifyCosts, ModOptions.ModifyCosts),
        };
        selectedIndex = Popup.PickOption(
          Title: "Skill training settings",
          Intro: "Settings for this playthrough.\n\n",
          Options: options,
          DefaultSelected: selectedIndex,
          AllowEscape: true
        );

        if (selectedIndex == 0) {
          Tracker.ModifyCosts = Tracker.ModifyCosts switch {
            null => true,
            true => false,
            _ => null
          };
          CostModifier.ResetSkillCosts();
        }
      }
    }

    public static void SkillMenu(KeyValuePair<String, Decimal> entry) {
      var selectedIndex = 0;
      while (selectedIndex != -1) {
        var options = new[] {
          "Toggle training \x10 " + SkillToggleStatus(entry.Key),
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
          break;
        }
      }
    }

    public static String SkillToggleStatus(String skillClass) {
      Tracker.Enabled.TryAdd(skillClass, null);
      var isSet = true;
      var value = Tracker.Enabled[skillClass]
                  ?? ModOptions.TrainingEnabled.Also(_ => { isSet = false; });

      return The.Player?.HasSkill(skillClass) != false
        ? "{{k|[---]}}"
        : ToggleStatus(value, isSet);
    }

    public static String ToggleStatus(Boolean? value, Boolean defaultValue) {
      return ToggleStatus(value ?? defaultValue, value != null);
    }

    public static String ToggleStatus(Boolean value, Boolean isSet) {
      var bColor = isSet ? "&y" : "&K";
      var color = isSet ? (value ? "&G" : "&R") : "";

      return bColor + "[" + color + (value ? "ON" : "OFF").PadLeft(3, ' ') + bColor + "]&y";
    }
  }
}