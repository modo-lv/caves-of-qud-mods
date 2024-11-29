using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using XRL.World;

namespace ModoMods.ItemRecoiler.Parts {
  /// <summary>Reacts to keyboard shortcut</summary>
  public class IrActivationCommand : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { CommandEvent.ID, };

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != IrEventNames.TransmitCommand)
        return base.HandleEvent(ev);

      var recoiler = Main.Player.Inventory.FindObjectByBlueprint(IrBlueprintNames.Recoiler);
      if (recoiler != null)
        recoiler.Twiddle();
      else
        Output.Message("You don't have an item recoiler");

      return base.HandleEvent(ev);
    }
  }
}