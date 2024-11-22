using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SkillTraining.Internal;
using UnityEngine;
using XRL.World;
using XRL.World.Skills;

namespace SkillTraining.Parts {
  /// <summary> Main component that tracks training points for each trainable skill. </summary>
  public class PointTracker : IPart {

    /// <summary>Player's training points in each skill.</summary>
    public IReadOnlyDictionary<String, Decimal> Points { get; protected set; } = null!;
    /// <inheritdoc cref="Points"/>
    [SerializeField]
    protected readonly IDictionary<String, Decimal> _Points = new Dictionary<String, Decimal>();
    protected void _OnPointUpdate() { this.Points = new ReadOnlyDictionary<String, Decimal>(this._Points); }


    public PointTracker() { this._OnPointUpdate(); }


    /// <summary>Increases training point value for a skill.</summary>
    public void AddPoints(String skill, Decimal amount) {
      if (!this._Points.ContainsKey(skill))
        this._Points[skill] = 0;
      this._Points[skill] += amount;
      Output.DebugLog(
        $"Training points for [{skill.SkillName()}] increased by {amount} to {this.Points[skill]} total."
      );
      this._OnPointUpdate();
    }

    /// <summary>Completely removes a skill from the training registry.</summary>
    public void RemoveSkill(String skill) {
      this._Points.Remove(skill);
      this._OnPointUpdate();
    }

    /// <summary>Called by the game whenever the part is created and attached to an object.</summary>
    /// <remarks>
    /// Will not be called on game load if the player already had this part when it was saved,
    /// only first-time instantiations trigger this.
    /// </remarks>
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.DebugLog($"[{nameof(PointTracker)}] part created and added to [{this.ParentObject}].");
    }

    public override Boolean WantEvent(Int32 id, Int32 cascade) =>
      base.WantEvent(id, cascade)
      || id == BeforeMeleeAttackEvent.ID;

    public override Boolean HandleEvent(BeforeMeleeAttackEvent ev) {
      // Attach the melee tracker to the target creature.
      if (ev.Target.IsCreature)
        ev.Target.RequirePart<MeleeAttackTracker>();
      return base.HandleEvent(ev);
    }

    public override String ToString() {
      var list =
        this.Points
          .Select(entry => {
            var total = SkillFactory.Factory.SkillList[entry.Key.SkillName()].Cost; 
            return $"\t {SkillFactory.GetSkillOrPowerName(entry.Key)}: " +
                   $"{{{{Y|{entry.Value}}}}} / {total}";
          })
          .Unless(it => it.IsNullOrEmpty())
          ?.Aggregate((a, b) => $"{a}\n{b}")
        ?? "  (none)";

      return $"Skill training points\n{list}";
    }
  }
}