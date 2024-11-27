using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil.Data;
using XRL.World;

namespace ModoMods.LootRecoil.Parts {
  public class RecoiledVacuum : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EnteringZoneEvent.ID };

    public override Boolean HandleEvent(EnteringZoneEvent ev) {
      var zone = Main.Player.CurrentCell.ParentZone;
      AttemptCleaning(zone);

      return base.HandleEvent(ev);
    }

    public static void AttemptCleaning(Zone zone) {
    }
  }
}