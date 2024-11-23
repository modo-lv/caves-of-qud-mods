using System;
using System.Linq;
using System.Text.RegularExpressions;
using Modo.SkillTraining.Internal;
using XRL.UI;
using XRL.Wish;
using XRL.World.Skills;

namespace Modo.SkillTraining.Wiring {
  [HasWishCommand] public class Wishes {
    [WishCommand("SkillTraining")]
    public static void Handle() {
      var choice = 0;
      while (true) {
        choice = Popup.PickOption(
          Title: "Skill Training mod menu",
          AllowEscape: true,
          DefaultSelected: choice,
          Options: new[] {
            "Training progress",
            "Untrain a skill",
            "Unlearn a skill (lose skill points)",
            "Unlearn a skill (refund skill points)",
          }
        );
        switch (choice) {
          case 0: Output.Alert(Req.TrainingTracker.ToString()); break;
          case 1: Untrain(); break;
          case 2: Unlearn(false); break;
          case 3: Unlearn(true); break;
          default: return;
        }
      }
    }

    public static void Untrain() {

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