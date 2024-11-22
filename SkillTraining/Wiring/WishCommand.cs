using System.Text.RegularExpressions;
using Modo.SkillTraining.Internal;
using XRL.Wish;

namespace Modo.SkillTraining.Wiring {

  [HasWishCommand]
  public class Wishes {
    [WishCommand(Regex = "SkillTraining(:.+)?")]
    public static void Handle(Match match) {
      Output.DebugLog($"Wish received: {match.Groups[1]}");
      switch (match.Groups[1].Value) {
        default: Output.Alert(Req.PointTracker.ToString()); break;
      }
    }
  }
  
}