using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL;
using XRL.UI;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Snake Oiler" skill.</summary>
  /// <remarks>
  /// Attached to player to listen for trade start events.
  /// When a trade is started, also attaches to the trader, to wait for "take object" event. 
  /// </remarks>
  public class SnakeOilerTrainer : ModPart {
    public override Set<Int32> WantEventIds => new Set<Int32> { StartTradeEvent.ID };

    public override Boolean HandleEvent(StartTradeEvent ev) {
      // Party member trading doesn't count
      if (ev.Trader.IsPlayerLed()) {
        ev.Trader.RemovePart<SnakeOilerTrainer>();
        return base.HandleEvent(ev);
      }
      // Non-creatures are containers (which trigger the same events).
      if (ev.Trader.IsCreature)
        ev.Trader.RequirePart<SnakeOilerTrainer>();
      return base.HandleEvent(ev);
    }

    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.CommandTakeObject);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      var item = ev.GetGameObjectParameter("Object");
      
      if (ev.ID != EventNames.CommandTakeObject
          || TradeUI.ScreenMode != TradeUI.TradeScreenMode.Trade
          || !this.ParentObject.IsCreature
          // Getting items doesn't train selling them
          || this.ParentObject.IsPlayer()
          || item == null
          || item.ValueEach <= 1)
        return base.FireEvent(ev);
      
      var itemCount = item.Stacker?.Number ?? 1m;
      var bonus = Math.Round(new Decimal(item.Value) / 1000, 2);
      Output.DebugLog($"Sold {itemCount} item(s) worth {item.Value}, bonus: {bonus}.");
      bonus *= 1m / TrainingData.For(PlayerAction.TradeItem).DefaultAmount;
      Main.PointTracker.HandleTrainingAction(PlayerAction.TradeItem, itemCount + bonus);

      return base.FireEvent(ev);
    }
  }
}