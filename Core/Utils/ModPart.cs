using System;
using System.Collections.Generic;
using XRL.World.Parts;

namespace ModoMods.Core.Utils {
  public abstract class ModPart : IPlayerPart {
    /// <summary>
    /// Outputs a debug log message whenever a new instance of this part is attached to a game object.
    /// </summary>
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"New [{this.GetType().Name}] attached to [{this.ParentObject}].");
    }

    /// <summary>
    /// Outputs a debug log message whenever this instance is removed from a game object.
    /// </summary>
    public override void Remove() {
      Output.DebugLog($"Removed [{this.GetType().Name}] from [{this.ParentObject}].");
      base.Remove();
    }

    /// <summary>Determines which min-event IDs to listen for.</summary>
    public virtual ISet<Int32> WantEventIds => new HashSet<Int32>();

    /// <summary>
    /// Listens for all event IDs contained in <see cref="WantEventIds"/>.
    /// </summary>
    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || this.WantEventIds.Contains(id);
  }
}