using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Modifies skill points gained on level up, depending on configuration.</summary>
  public class LevelUpModifier : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { GetLevelUpPointsEvent.ID };

    public override Boolean HandleEvent(GetLevelUpPointsEvent ev) {
      // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
      switch (this.ParentObject.TrainingTracker()?.LevelUpSkillPoints) {
        case LevelUpSkillPoints.Reduced: ev.SkillPoints /= 2; break;
        case LevelUpSkillPoints.None: ev.SkillPoints = 0; break;
      }
      return base.HandleEvent(ev);
    }
  }
}