using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.ItemRecoiler.Wiring {
  /// <summary>Reacts to keyboard shortcut</summary>
  public class QuickActivateCommand : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { CommandEvent.ID, };

    public override Boolean HandleEvent(CommandEvent ev) {
      if (ev.Command != EventNames.TransmitCommand)
        return base.HandleEvent(ev);

      var recoilers = Main.Player.Inventory.GetObjects(it => it.Blueprint == ModBlueprintNames.Recoiler);
      if (recoilers.Count > 1) {
        Main.Player.ParticleText(
          "You have multiple item recoilers. Quick activation not available.",
          Color: 'w', juiceDuration: 3f
        );
      } else if (recoilers[0] != null) {
        var teleporter = recoilers[0].GetPartDescendedFrom<ITeleporter>();
        if (teleporter.DestinationZone.IsNullOrEmpty())
          recoilers[0].Twiddle();
        else
          teleporter.HandleEvent(
            new InventoryActionEvent { Command = QudCommands.ActivateTeleporter }
          );
      } else
        Main.Player.ParticleText("You don't have an item recoiler.", 'o');

      return base.HandleEvent(ev);
    }
  }
}