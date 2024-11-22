using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AiUnity.Common.Extensions;
using Modo.SkillTraining.Internal;
using UnityEngine;
using XRL.World;
using XRL.World.Anatomy;
using XRL.World.Skills;
using Skills = XRL.World.Parts.Skills;

namespace Modo.SkillTraining.Parts {
  /// <summary> Main component that tracks training points for each trainable skill. </summary>
  public class PointTracker : IPart {

    /// <summary>Player's training points in each skill.</summary>
    public IReadOnlyDictionary<String, Decimal> Points { get; protected set; } = null!;
    /// <inheritdoc cref="Points"/>
    [SerializeField]
    protected readonly IDictionary<String, Decimal> _Points = new Dictionary<String, Decimal>();
    protected void _OnPointUpdate() {
      this.Points = new ReadOnlyDictionary<String, Decimal>(this._Points);
    }


    public PointTracker() { this._OnPointUpdate(); }


    /// <summary>Increases training point value for a skill.</summary>
    public void AddPoints(String skillClass, Decimal amount) {
      if (!this._Points.ContainsKey(skillClass))
        this._Points[skillClass] = 0;
      this._Points[skillClass] += amount;
      
      Output.DebugLog($"[{skillClass.SkillName()}] + {amount} = {this.Points[skillClass]}");
      (
        from entry in Req.Player.RequirePart<PointTracker>().Points
        where SkillUtils.SkillOrPower(entry.Key)!.Cost <= entry.Value 
        select entry.Key
      ).ToList().ForEach(skill => {
        Output.Alert($"You have unlocked {{{{Y|{skill.SkillName()}}}}} through practical training!");
        Req.PointTracker.RemoveSkill(skill);
        Req.Player.GetPart<Skills>().AddSkill(skill);
        Output.Log($"[{skill}] added to [{Req.Player}], training points removed.");
      });
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
      || id == BeforeMeleeAttackEvent.ID
      || id == BeforeFireMissileWeaponsEvent.ID
      || id == EquipperEquippedEvent.ID;

    /// <summary>Melee weapon attack tracking.</summary>
    public override Boolean HandleEvent(BeforeMeleeAttackEvent ev) {
      if (ev.Target.IsCreature)
        ev.Target.RequirePart<MeleeAttackTracker>();
      return base.HandleEvent(ev);
    }

    /// <summary>Missile weapon attack tracking.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget?.IsCreature == true)
        ev.ApparentTarget.RequirePart<MissileAttackTracker>();
      return base.HandleEvent(ev);
    }

    /// <summary>Thrown weapon attack tracking.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon()) {
        ev.Item.RequirePart<ThrownAttackTracker>();
      }
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