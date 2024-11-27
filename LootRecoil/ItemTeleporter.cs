using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil;
using XRL.UI;
using XRL.World.Parts;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  public class ItemTeleporter : Teleporter {
    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command != CommandNames.ActivateTeleporter)
        return true;

      this.AttemptItemTeleport(Main.Player.Inventory.Objects.Take(1));

      return true;
    }

    public Boolean AttemptItemTeleport(IEnumerable<GameObject> items) {
      var Actor = Main.Player;
      if (this.DestinationZone.IsNullOrEmpty())
        return Actor.Fail("Nothing happens.");

      if (!this.UsableInCombat && Actor.AreHostilesNearby())
        return Actor == this.ParentObject
          ? Actor.Fail("You can't recoil with hostiles nearby!")
          : Actor.Fail("You can't use " + this.ParentObject.t() + " with hostiles nearby!");

      Actor.PlayWorldOrUISound(this.Sound);
      Output.Message("You activate the item recoiler.");

      var num2 = items.First().ZoneTeleport(
        this.DestinationZone,
        this.DestinationX,
        this.DestinationY,
        null,
        this.ParentObject,
        Actor
      ) ? 1 : 0;
      if (num2 == 0)
        return false;
      this.LastTurnUsed = The.CurrentTurn;
      return true;
    }
  }
}