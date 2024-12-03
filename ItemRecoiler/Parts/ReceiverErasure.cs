using System;
using System.Collections.Generic;
using System.Linq;
using ModoMods.Core.Utils;
using XRL.World;

namespace ModoMods.ItemRecoiler.Parts {
  public class ReceiverErasure : ModPart {
    public const String EraseCommand = "ModoMods_ItemRecoiler_EraseReceiver";

    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      GetInventoryActionsEvent.ID,
      InventoryActionEvent.ID,
    };

    public override Boolean HandleEvent(GetInventoryActionsEvent ev) {
      ev.AddAction(Name:"Erase", Display:"turn off", Key:'u', Command:EraseCommand);
      return base.HandleEvent(ev);
    }
    
    public override Boolean HandleEvent(InventoryActionEvent ev) {
      if (ev.Command == EraseCommand) {
        this.ParentObject
          .Inventory.Objects.ToList()
          .ForEach(item => this.ParentObject.CurrentCell.AddObject(item));
        if (this.ParentObject.Inventory.Objects.IsNullOrEmpty()) {
          ThePlayer.ShowSuccess(
            "You pick up the tiny semi-organic e-beam diffractor, and " +
            "mush it into vaguely unpleasant goop in your fist. " +
            "Not even an afterimage of the hologram remains."
          );
          this.ParentObject.Obliterate(Silent: true);
        } else
          Output.Alert(
            $"Could not remove all items from {this.ParentObject.BaseDisplayName}. " +
            "Please remove manually and try again."
          );
      }
      return base.HandleEvent(ev);
    }
  }
}