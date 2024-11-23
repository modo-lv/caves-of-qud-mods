using Modo.SkillTraining.Internal;
using XRL.Wish;

namespace Modo.SkillTraining.Wiring {
  [HasWishCommand] public class Wishes {
    [WishCommand("SkillTraining")] public static void Handle() {
      Output.Alert(Req.TrainingTracker.ToString());
    }
  }
}