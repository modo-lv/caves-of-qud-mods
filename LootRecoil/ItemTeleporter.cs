using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.LootRecoil;
using ModoMods.LootRecoil.Data;
using XRL.UI;
using XRL.World.Parts;

// ReSharper disable once CheckNamespace
namespace XRL.World.Parts {
  public class ItemTeleporter : Teleporter {
    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command != CommandNames.ActivateTeleporter)
        return true;

      if (this.AttemptItemTeleport()) {
        ev.RequestInterfaceExit();
      }

      return true;
    }
    
    

    public Boolean AttemptItemTeleport() {
      var Actor = Main.Player;
      if (this.DestinationZone.IsNullOrEmpty())
        return Actor.Fail("Nothing happens.");

      if (!this.UsableInCombat && Actor.AreHostilesNearby())
        return Actor == this.ParentObject
          ? Actor.Fail("You can't recoil with hostiles nearby!")
          : Actor.Fail("You can't use " + this.ParentObject.t() + " with hostiles nearby!");

      Actor.PlayWorldOrUISound(this.Sound);
      Output.Message("You activate the item recoiler.");

      var transmitter = GameObject.CreateUnmodified(LrBlueprintNames.Transmitter);
      TradeUI.ShowTradeScreen(transmitter, 0.0f, TradeUI.TradeScreenMode.Container);
      var total = 0;
      var zone = The.ZoneManager.GetZone(this.DestinationZone);
      while (!transmitter.Inventory.Objects.IsNullOrEmpty()) {
        var item = transmitter.Inventory.GetFirstObject();
        if (item.Blueprint == LrBlueprintNames.Recoiler) {
          Main.Player.Inventory.AddObject(item);
          continue;
        }
        if (item.ZoneTeleport(
              this.DestinationZone,
              this.DestinationX,
              this.DestinationY,
              null,
              this.ParentObject,
              Actor)) {
          item.RequirePart<IsTeleportedItem>();
          zone.RequirePart<HasTeleportedItems>();
          total += item.Count;
        }
      }
      Output.Message(total + " item(s) recoiled to {{Y|" + zone.DisplayName + "}}.");
      RecoiledVacuum.AttemptCleaning(zone);
      return true;
    }
  }
}