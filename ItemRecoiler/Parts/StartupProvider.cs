using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Wiring;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.ItemRecoiler.Parts {
  /// <summary>
  /// Provide palyer 
  /// </summary>
  [Serializable] public class StartupProvider : ModPart {
    public Boolean Provided;

    public override ISet<Int32> WantEventIds => new HashSet<Int32> { BeforeTakeActionEvent.ID };

    public override Boolean HandleEvent(BeforeTakeActionEvent ev) {
      var player = this.ParentObject;
      if (ModOptions.GiveOnStartup && !this.Provided) {
        var recoiler = GameObject.CreateUnmodified(ModBlueprintNames.Recoiler);
        {
          var cell = GameObject.CreateUnmodified("Solar Cell");
          cell.RemovePart<Examiner>();
          cell.AddPart<ModMetered>();
          recoiler.GetPart<EnergyCellSocket>().SetCell(cell);
        }
        var text = "You feel a slight spacetime disturbance in your immediate vicinity, " +
                   $"and quickly discover {recoiler.a}{recoiler.BaseDisplayName} in your inventory " +
                   "that wasn't there before.";
        Output.Alert(text);
        player.Inventory.AddObject(recoiler);
        this.Provided = true;
      }
      return base.HandleEvent(ev);
    }
  }
}