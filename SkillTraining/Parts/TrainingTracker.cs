using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modo.SkillTraining.Constants;
using Modo.SkillTraining.Internal;
using Wintellect.PowerCollections;
using XRL.World;
using XRL.World.Skills;
using Skills = XRL.World.Parts.Skills;

namespace Modo.SkillTraining.Parts {
  /// <summary>Main component that tracks training points for trainable skills.</summary>
  [Serializable]
  public class TrainingTracker : ModPart {
    /// <inheritdoc cref="Points"/>
    public Dictionary<String, Decimal> Points = null!;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      BeforeMeleeAttackEvent.ID,
      EquipperEquippedEvent.ID,
      BeforeFireMissileWeaponsEvent.ID,
      GetCookingActionsEvent.ID,
    };

    public override void FinalizeRead(SerializationReader Reader) {
      base.FinalizeRead(Reader);
      if (this.Points.IsNullOrEmpty()) {
        Output.DebugLog("No training points loaded, starting from scratch.");
        this.Points = new Dictionary<String, Decimal> {
          { SkillClasses.Axe, 0 },
          { SkillClasses.BowAndRifle, 0 },
          { SkillClasses.Cudgel, 0 },
          { SkillClasses.CookingAndGathering, 0 },
          { SkillClasses.DeftThrowing, 0 },
          { SkillClasses.HeavyWeapons, 0 },
          { SkillClasses.LongBlade, 0 },
          { SkillClasses.MultiweaponFighting, 0 },
          { SkillClasses.Pistol, 0 },
          { SkillClasses.ShortBlade, 0 },
          { SkillClasses.SingleWeaponFighting, 0 },
        };
      }
    }


    /// <summary>Melee weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeMeleeAttackEvent ev) {
      if (ev.Target.IsCreature)
        ev.Target.RequirePart<MeleeWeaponTrainer>();
      return base.HandleEvent(ev);
    }

    /// <summary>Thrown weapon attack training.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<ThrownAttackTracker>();
      return base.HandleEvent(ev);
    }

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget?.IsCreature == true)
        ev.ApparentTarget.RequirePart<MissileAttackTrainer>();
      return base.HandleEvent(ev);
    }

    /// <summary>Cooking training.</summary>
    public override Boolean HandleEvent(GetCookingActionsEvent ev) {
      if (ev.Actor.IsPlayer())
        Req.Player.RequirePart<CookingTracker>();
      return base.HandleEvent(ev);
    }


    /// <summary>Increases training point value for a skill.</summary>
    public void AddPoints(String skillClass, Decimal amount) {
      if (amount > 0 && !Req.Player.HasSkill(skillClass)) {
        this.Points.TryAdd(skillClass, 0);
        this.Points[skillClass] += amount;
        Output.DebugLog($"[{skillClass.SkillName()}] + {amount} = {this.Points[skillClass]}");
      }
      (
        from entry in Req.Player.RequirePart<TrainingTracker>().Points
        where SkillUtils.SkillOrPower(entry.Key)!.Cost <= entry.Value
        select entry.Key
      ).ToList().ForEach(skill => {
        Output.Alert($"You have unlocked {{{{Y|{skill.SkillName()}}}}} through practical training!");
        Req.Player.GetPart<Skills>().AddSkill(skill);
        Output.Log($"[{skill}] added to [{Req.Player}], training points removed.");
      });
    }

    /// <summary>Outputs the current training point overview, formatted for in-game display.</summary>
    public override String ToString() {
      var list =
        this.Points
          .OrderBy(e => e.Key)
          .Select(entry => {
            var cost = SkillUtils.SkillOrPower(entry.Key)!.Cost;
            var active = entry.Value < cost;
            var sb = new StringBuilder();
            sb.Append(active ? "&y" : "&K");
            sb.Append($"○ {SkillFactory.GetSkillOrPowerName(entry.Key)} ".PadRight(34, '-'));

            // Current points
            sb.Append(" ");
            var value = $"{entry.Value:##0.00}";
            var pad = 6;
            if (active) {
              value = "{{Y|" + value + "}}";
              pad += 6;
            }
            if (entry.Value == 0) {
              sb.Append("{{k|000.0}}{{Y|0}}");
            } else if (value.Length < pad) {
              sb.Append("{{k|" + ("}}" + value).PadLeft(pad + 2, '0'));
            } else {
              sb.Append(value);
            }

            sb.Append(" /");

            // Cost
            sb.Append(" ");
            value = cost.ToString();
            pad = 3;
            if (value.Length < pad)
              sb.Append("{{k|" + ("}}" + value).PadLeft(pad + 2, '0'));
            else {
              sb.Append(value);
            }

            Output.DebugLog(sb.ToString());
            return sb.ToString();
          })
          .Aggregate((a, b) => $"{a}\n{b}");

      return $"{{{{C|Skill training points}}}}\n\n{list}";
    }
  }
}