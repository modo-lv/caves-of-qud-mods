using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts;
using static ModoMods.Core.Data.QudSkillClasses;
using static ModoMods.SkillTraining.Data.PlayerAction;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains missile weapon skills.</summary>
  /// <remarks>
  /// Attached to the player to listen for missile attack start, and attach the hit tracker to the target.
  /// </remarks>
  public class MissileWeaponTrainer : ModPart {
    public Boolean SprintedSinceShot;
    
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EnteredCellEvent.ID,
      BeforeFireMissileWeaponsEvent.ID,
    };

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget.IsCombatant() && ev.Actor.CanTrainSkills()) {
        ev.ApparentTarget.RequirePart<MissileHitTracker>();
        ev.MissileWeapons.ForEach(mw => mw.ParentObject.RequirePart<ShotCompleteTracker>());
      }
      return base.HandleEvent(ev);
    }

    /// <summary>Carrying a heavy weapon.</summary>
    public override Boolean HandleEvent(EnteredCellEvent ev) {
      if (ev.Actor == this.ParentObject) {
        if (ev.Actor.HasEffect<Running>())
          this.SprintedSinceShot = true;
        if (ev.Actor.HasHeavyWeaponEquipped())
          ev.Actor.Training()?.HandleTrainingAction(CarryHeavyWeapon);
      }
      return base.HandleEvent(ev);
    }
  }

  public class ShotCompleteTracker : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      ShotCompleteEvent.ID
    };

    public override Boolean HandleEvent(ShotCompleteEvent ev) {
      if (ev.Actor.CanTrainSkills())
        ev.Actor.GetPart<MissileWeaponTrainer>().SprintedSinceShot = false;
      return base.HandleEvent(ev);
    }
  }

  /// <summary>Tracks missile hits on targeted enemies.</summary>
  public class MissileHitTracker : ModPart {
    public override void Register(GameObject gameObject, IEventRegistrar registrar) {
      gameObject.RegisterPartEvent(this, QudEventNames.DefenderProjectileHit);
      base.Register(gameObject, registrar);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != QudEventNames.DefenderProjectileHit)
        return base.FireEvent(ev);

      var launcher = ev.GetGameObjectParameter("Launcher");
      var skillClass = ev.GetStringParameter("Skill");
      var attacker = ev.Attacker()?.OnlyIf(it => it.CanTrainSkills());
      var defender = ev.Defender()?.OnlyIf(it => it.IsCombatant());

      if (launcher == null || attacker == null || defender == null || skillClass == null)
        return base.FireEvent(ev);

      var action = skillClass switch {
        // AFAICT there are no weapons using "Bow" skill anymore, but the game has checks for it.
        "Bow" => BowOrRifleHit,
        Pistol => PistolHit,
        "Rifle" => BowOrRifleHit,
        HeavyWeapon => HeavyWeaponHit,
        _ => throw new Exception($"Unknown missile weapon skill: [{skillClass}].")
      };

      if (attacker.CanTrainSkills()
          && ev.GetGameObjectParameter("AimedAt") == defender
          && defender.IsCombatant()
         ) {
        var multiplier = 1m / launcher.GetPart<MissileWeapon>()?.ShotsPerAction ?? 1m;

        if (action == PistolHit) {
          // Sprinting
          if (attacker.HasEffect<Running>() && attacker.GetPart<MissileWeaponTrainer>().SprintedSinceShot)
            attacker.Training()?.HandleTrainingAction(SprintingPistolHit);
          // Critical
          if (ev.HasFlag("Critical")) {
            attacker.Training()?.HandleTrainingAction(PistolNativeCrit);
          }
          // Multiple one-handed weapons (pistols) equipped
          Boolean advanced; 
          if (attacker.GetMissileWeapons(w => !w.GetPart<Physics>().UsesTwoSlots)?.Count > 1) {
            advanced = attacker.HasSkill(Akimbo);
            action = AlternatePistolHit;
          } else {
            advanced = attacker.HasSkill(Pistol);
          }

          if (advanced)
            action = attacker.HasSkill(EmptyClips) ? PistolFastertHit : PistolFastHit;
        }
        
        if (ev.HasFlag("Critical")) {
          multiplier *= 2;
        }

        attacker.Training()?.HandleTrainingAction(action, multiplier);
      }

      return base.FireEvent(ev);
    }
  }
}