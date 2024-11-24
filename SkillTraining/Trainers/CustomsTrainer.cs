using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  public class CustomsTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.ReputationChanged);
    }

    public override Boolean FireEvent(Event ev) {
      if (Main.Player.HasSkill(SkillClasses.CustomsAndFolklore)
          || ev.GetStringParameter("Type") != "WaterRitualPrimaryAward")
        return base.FireEvent(ev);

      Output.DebugLog("Water ritual reputation change.");
      Main.PointTracker.AddPoints(SkillClasses.CustomsAndFolklore, Settings.CustomsTrainingRate);
      return base.FireEvent(ev);
    }
  }
}