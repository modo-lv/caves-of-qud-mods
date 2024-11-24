using System;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Modo.SkillTraining.Wiring;
using XRL;
using XRL.UI;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public class SnakeOilerTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.CommandRemoveObject);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.CommandRemoveObject || TradeUI.ScreenMode != TradeUI.TradeScreenMode.Trade)
        return base.FireEvent(ev);
      
      Output.DebugLog("Successful trade.");
      Req.PointTracker.AddPoints(SkillClasses.SnakeOiler, ModOptions.SnakeOilerTrainingRate);
      return base.FireEvent(ev);
    }
  }
}