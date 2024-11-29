using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.ItemRecoiler.Data;
using ModoMods.ItemRecoiler.Wiring;
using XRL.World;

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
        var recoiler = GameObject.CreateUnmodified(IrBlueprintNames.Recoiler);
        var text = "You feel a slight spacetime disturbance in your immediate vicinity, " +
                   $"and quickly discover {recoiler.a}{recoiler.DisplayName} in your inventory " +
                   "that wasn't there before.";
        Output.Alert(text);
        player.Inventory.AddObject(recoiler);
        this.Provided = true;
      }
      return base.HandleEvent(ev);
    }
  }
}