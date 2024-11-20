using System.Text.RegularExpressions;
using SkillTraining.Utils;
using XRL.Wish;

namespace SkillTraining.Wiring {

  [HasWishCommand]
  public class Wishes {
    [WishCommand(Regex = "SkillTraining(:.+)?")]
    public static void Handle(Match match) {
      Output.DebugLog($"Wish received: {match.Groups[1]}");
      switch (match.Groups[1].Value) {
        default: Output.Alert(Req.Player.GetPointTracker().ToString()); break;
      }
    }
  }
  
}