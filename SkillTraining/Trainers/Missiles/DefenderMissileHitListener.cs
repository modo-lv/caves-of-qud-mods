using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Parts;

namespace ModoMods.SkillTraining.Trainers.Missiles {
  /// <summary>Reacts to missile hits on targeted enemies.</summary>
  public class DefenderMissileHitListener : ModPart {
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

      if (attacker.CanTrainSkills()
          && ev.GetGameObjectParameter("AimedAt") == defender
          && defender.IsCombatant()) {
        var multiplier = 1m / launcher.GetPart<MissileWeapon>()?.ShotsPerAction ?? 1m;

        IMissileWeaponTrainer trainer = skillClass switch {
          // AFAICT there are no weapons using "Bow" skill anymore, but the game still has checks for both.
          "Bow" or "Rifle" => this.ParentObject.RequirePart<BowAndRifleTrainer>(),
          "Pistol" => this.ParentObject.RequirePart<PistolTrainer>(),
          "HeavyWeapons" => this.ParentObject.RequirePart<HeavyWeaponTrainer>(),
          _ => throw new Exception($"Unknown missile weapon skill: [{skillClass}].")
        };
        trainer.HandleDefenderHit(attacker, defender, launcher, multiplier, ev.HasFlag("Critical"));
      }

      return base.FireEvent(ev);
    }
  }
}