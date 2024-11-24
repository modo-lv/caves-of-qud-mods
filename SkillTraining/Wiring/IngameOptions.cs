using System;
using System.Collections.Generic;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using XRL.World;

namespace Modo.SkillTraining.Wiring {
  /// <summary>Per-game settings, configurable for each playthrough individually.</summary>
  [Serializable] public class IngameOptions : ModPart {
    public IDictionary<PlayerAction, Decimal> CustomRates = new Dictionary<PlayerAction, Decimal>();
    public Boolean UseCustomRates = false;

    /// <summary>Returns the training rate (customized or default) for a player action.</summary>
    public Decimal TrainingRateFor(PlayerAction playerAction) {
      if (this.UseCustomRates && this.CustomRates.TryGetValue(playerAction, out var customRate))
        return customRate;
      return TrainingData.For(playerAction).DefaultAmount;
    }
  }
}