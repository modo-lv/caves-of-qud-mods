using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SkillTraining.Utils;
using UnityEngine;
using XRL.World;

namespace SkillTraining.Parts {
  public class PointTracker : IPart {
    /// <summary>Player's training points in each skill.</summary>
    public IReadOnlyDictionary<String, Decimal> Points { get; protected set; } = null!;
    /// <inheritdoc cref="Points"/>
    [SerializeField] protected readonly IDictionary<String, Decimal> _Points = new Dictionary<String, Decimal>();
    protected void _OnPointUpdate() { this.Points = new ReadOnlyDictionary<String, Decimal>(this._Points); }


    public PointTracker() { this._OnPointUpdate(); }


    /// <summary>Increases training point value for a skill.</summary>
    public void AddPoints(String skill, Decimal points) {
      if (!this._Points.ContainsKey(skill))
        this._Points[skill] = 0;
      this._Points[skill] += points;
      Output.Log($"Training points for [{skill}] increased by {points} to {this.Points[skill]} total.");
      this._OnPointUpdate();
    }


    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      Output.Log($"[{nameof(PointTracker)}] part created and added to [{this.ParentObject}].");
    }


    public override String ToString() {
      var list =
        this.Points
          .Select(entry => $"\t {entry.Key}: {entry.Value}")
          .Unless(it => it.IsNullOrEmpty())
          ?.Aggregate((a, b) => $"{a}\n{b}")
        ?? "  (none)";

      return $"Skill training points:\n{list}";
    }
  }
}