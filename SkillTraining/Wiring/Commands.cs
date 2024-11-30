using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Wiring {
  /// <summary>Reacts to keyboard shortcut</summary>
  public class Commands : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { CommandEvent.ID };

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != ModEventNames.OverviewCommand)
        return base.HandleEvent(ev);

      Wishes.Overview();
      return true;
    }
  }
}