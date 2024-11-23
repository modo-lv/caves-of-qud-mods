using Modo.SkillTraining.Internal;
using XRL.World;

namespace Modo.SkillTraining.Parts {
  public abstract class ModPart : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"New [{this.GetType().Name}] attached to [{this.ParentObject.GetType().Name}].");
    }
  }
}