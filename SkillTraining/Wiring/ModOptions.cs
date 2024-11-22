using System;
using XRL.UI;

namespace SkillTraining.Wiring {
  public static class ModOptions {
    public static Int32 WeaponTrainingPercentage =>
      Convert.ToInt32(Options.GetOption("Option_Modo_SkillTraining_WeaponTrainingPercentage"));
  }
}