using System;
using ModoMods.Core.Utils;
using XRL.World;
using XRL.World.ZoneParts;

namespace ModoMods.LootRecoil {
  [Serializable] public class HasTeleportedItems : IZonePart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"{nameof(HasTeleportedItems)} added to {this.ParentZone.DisplayName}");
    }
  }
}