using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains missile weapon skills.</summary>
  /// <remarks>
  /// Attached to the player to listen for missile attack start, and attach the hit tracker to the target.
  /// </remarks>
  public class MissileAttackTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { BeforeFireMissileWeaponsEvent.ID };

    /// <summary>Missile weapon attack training.</summary>
    public override Boolean HandleEvent(BeforeFireMissileWeaponsEvent ev) {
      if (ev.ApparentTarget.IsCombatant() && ev.Actor.CanTrainSkills())
        ev.ApparentTarget.RequirePart<MissileHitTracker>();
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
        "Bow" => PlayerAction.BowOrRifleHit,
        SkillClasses.Pistol => PlayerAction.PistolHit,
        "Rifle" => PlayerAction.BowOrRifleHit,
        SkillClasses.HeavyWeapon => PlayerAction.HeavyWeaponHit,
        _ => throw new Exception($"Unknown missile weapon skill: [{skillClass}].")
      };

      if (attacker.CanTrainSkills()
          && ev.GetGameObjectParameter("AimedAt") == defender
          && defender.IsCombatant()
         ) {
        var multiplier = 1m / launcher.GetPart<MissileWeapon>()?.ShotsPerAction ?? 1m;
        attacker.TrainingTracker()?.HandleTrainingAction(action, multiplier);
      }

      return base.FireEvent(ev);
    }
  }
}