using System.Linq;
using ModoMods.SkillGone.Utils;
using XRL;
using XRL.UI;
using XRL.Wish;
using XRL.World.Skills;

namespace ModoMods.SkillGone {
  [HasWishCommand] public class Main {
    [WishCommand("SkillGone")] public static void SkillGone() {
      var choice = 0;
      while (true) {
        var combined = SkillFactory.GetSkills()
          .SelectMany(skill => skill.PowerList.Select(it => it.Generic).Prepend(skill.Generic))
          .Where(it => The.Player.HasSkill(it.Entry.Class))
          .ToList();


        choice = Popup.PickOption(
          Title: "Forget a skill or power\n",
          Intro: "No skills or powers known.".OnlyIf(_ => combined.Count < 1),
          Options: combined.Select(it =>
            "\a ".OnlyIf(_ => it.Entry is PowerEntry) +
            "{{G|" + it.DisplayName + "}}" +
            $" [{it.Entry.Cost} sp]"
          ).ToList(),
          DefaultSelected: choice,
          AllowEscape: true
        );
        if (choice == -1)
          break;
        var target = combined[choice];
        var skillPower = target.Entry is PowerEntry ? "power" : "skill";

        DialogResult? refundChoice = null;
        if (target.Entry.Cost > 0) {
          refundChoice = Popup.ShowYesNoCancel(
            Message: "You are about to forget the {{G|" + target.DisplayName + "}} " + skillPower + ".\n\n" +
                     "Refund the {{B|" + target.Entry.Cost + "}} skill points?",
            AllowEscape: true,
            defaultResult: DialogResult.Cancel
          );
          if (refundChoice != DialogResult.Yes && refundChoice != DialogResult.No)
            continue;
        }

        var refund = refundChoice == DialogResult.Yes;

        The.Player.RemoveSkill(target.Name);
        if (refund)
          The.Player.GetStat("SP").Bonus += target.Entry.Cost;
        Popup.Show(
          "{{G|" + target.DisplayName + "}} " + skillPower + " forgotten." +
          (" {{B|+" + target.Entry.Cost + "}} skill points.").OnlyIf(_ => refund)
        );
        if (choice == combined.Count - 1) {
          choice--;
        }
      }
    }
  }
}