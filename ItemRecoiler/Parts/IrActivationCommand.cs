﻿using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.ItemRecoiler.Parts {
  /// <summary>Reacts to keyboard shortcut</summary>
  public class IrActivationCommand : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { CommandEvent.ID, };

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != IrEventNames.TransmitCommand)
        return base.HandleEvent(ev);

      var recoiler = Main.Player.Inventory.FindObjectByBlueprint(IrBlueprintNames.Recoiler);
      if (recoiler != null) {
        var teleporter = recoiler.GetPartDescendedFrom<ITeleporter>();
        if (teleporter.DestinationZone.IsNullOrEmpty())
          recoiler.Twiddle();
        else
          teleporter.HandleEvent(
            new InventoryActionEvent { Command = CommandNames.ActivateTeleporter }
          );
      } else
        Main.Player.ParticleText("You don't have an item recoiler.", 'o');

      return base.HandleEvent(ev);
    }
  }
}