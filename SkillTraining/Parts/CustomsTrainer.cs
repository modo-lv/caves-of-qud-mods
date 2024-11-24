using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public class CustomsTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.ReputationChanged);
    }

    public override Boolean FireEvent(Event ev) {
      if (Req.Player.HasSkill(SkillClasses.CustomsAndFolklore)
          || ev.GetStringParameter("Type") != "WaterRitualPrimaryAward")
        return base.FireEvent(ev);

      Output.DebugLog("Water ritual reputation change.");
      Req.PointTracker.AddPoints(SkillClasses.CustomsAndFolklore, ModOptions.CustomsTrainingRate);
      return base.FireEvent(ev);
    }
  }
}