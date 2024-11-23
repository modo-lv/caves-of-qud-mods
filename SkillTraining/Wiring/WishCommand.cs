using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modo.SkillTraining.Internal;
using XRL.UI;
using XRL.Wish;
using XRL.World.Parts;
using XRL.World.Skills;

namespace Modo.SkillTraining.Wiring {
  [HasWishCommand] public class Wishes {
    [WishCommand("SkillTraining")]
    public static void Handle() {
      var choice = 0;
      while (true) {
        choice = Popup.PickOption(
          Title: "Skill Training menu",
          AllowEscape: true,
          DefaultSelected: choice,
          Options: new[] {
            "Overview",
            "Modify skill training >",
            "Unlearn skills (lose skill points) >",
            "Unlearn skills (refund skill points) >",
          }
        );
        switch (choice) {
          case 0: Overview(); break;
          case 1: ModifyTraining(); break;
          case 2: Unlearn(false); break;
          case 3: Unlearn(true); break;
          default: return;
        }
      }
    }

    public static void Overview() {
      var output = Req.TrainingTracker.Points
        .OrderBy(e => e.Key.SkillName())
        .Select(entry => {
          var cost = SkillUtils.SkillOrPower(entry.Key)!.Cost;
          var locked = !Req.Player.HasSkill(entry.Key);
          var trained = locked && entry.Value >= cost;
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
        var list = Req.TrainingTracker.Points
          .Where(it => !Req.Player.HasSkill(it.Key))
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
          Req.TrainingTracker.AddPoints(target.Key, newValue - target.Value);
        else
          Req.TrainingTracker.Points[target.Key] = newValue;
      }
    }

    public static void Unlearn(Boolean refund) {
      var choice = 0;
      while (true) {
        var combined = SkillFactory.GetSkills()
          .SelectMany(skill => skill.PowerList.Select(it => it.Generic).Prepend(skill.Generic))
          .Where(it => Req.Player.HasSkill(it.Entry.Class))
          .ToList();

        var intro =
          "Choose a skill to unlearn. "
          + "{{"
          + $"{(refund ? "G" : "R")}|Skill points {(refund ? "will" : "won't")} be refunded."
          + "}}\n";
        if (combined.Count < 1)
          intro = "No skills learned.";

        choice = Popup.PickOption(
          Title: "Unlearn a skill",
          Intro: intro,
          Options: combined.Select(it =>
            "* ".If(_ => it.Entry is PowerEntry)
            + "{{Y|" + it.DisplayName + "}}"
            + $" [{it.Entry.Cost} sp]"
          ).ToList(),
          DefaultSelected: choice,
          AllowEscape: true
        );
        if (choice == -1)
          break;
        var target = combined[choice];
        if (!refund && target.Entry.Cost > 0) {
          var confirm = Popup.ShowYesNo(
            Message: "You will lose the {{Y|" + target.DisplayName + "}} skill, " +
                     "along with the {{Y|" + target.Entry.Cost + "}} points spent on it. Are you sure?",
            defaultResult: DialogResult.No
          );
          if (confirm != DialogResult.Yes) {
            continue;
          }
        }

        Req.Player.RemoveSkill(target.Name);
        if (refund)
          Req.Player.GetStat("SP").Bonus += target.Entry.Cost;
        Output.Alert(
          "{{Y|" + target.DisplayName + "}} unlearned."
          + (" {{G|" + target.Entry.Cost + "}} skill points refunded.").If(_ => refund)
        );
        if (choice == combined.Count - 1) {
          choice--;
        }
      }
    }
  }
}