using System;
using SkillTraining.Utils;
using XRL.World;

namespace SkillTraining.Parts {
  [Serializable] 
  public class PointTracker : IPart {
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.Log($"[{nameof(PointTracker)}] part created and added to [{this.ParentObject}].");
    }
  }
}