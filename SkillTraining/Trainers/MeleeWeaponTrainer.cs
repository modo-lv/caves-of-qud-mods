using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Modo.SkillTraining.Wiring;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains melee weapon skills.</summary>
  public class MeleeWeaponTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.AttackerHit);
    }

    public override Boolean FireEvent(Event ev) {
      var weapon = ev.GetGameObjectParameter("Weapon");
      var attacker = ev.GetGameObjectParameter("Attacker");
      var defender = ev.GetGameObjectParameter("Defender");
      var isCritical = ev.HasFlag("Critical");
      var skill = SkillUtils.SkillOrPower(weapon.GetWeaponSkill())?.Class;
      
      if (!attacker.IsPlayer()
          || defender.IsPlayer()
          || skill == null
          || Main.Player.HasSkill(skill)
          // Only equipped weapons train skills
          || weapon.EquippedOn()?.ThisPartWeapon() == null) {
        return base.FireEvent(ev);
      }
      
      PlayerAction? action = skill switch {
        SkillClasses.Axe => PlayerAction.AxeHit,
        SkillClasses.Cudgel => PlayerAction.CudgelHit,
        SkillClasses.LongBlade => PlayerAction.LongBladeHit,
        SkillClasses.ShortBlade => PlayerAction.ShortBladeHit,
        null => null,
        _ => throw new Exception($"Unknown melee weapon skill: [{skill}].")
      };

      // Weapon skill
      if (action is not null && weapon.IsEquippedInMainHand()) {
        Main.PointTracker.HandleTrainingAction(
          (PlayerAction) action,
          amountModifier: isCritical ? 2m : 1m
        );
      }
      
      var singleWeapon = true;
      The.Player.ForeachEquippedObject(obj => {
        if (singleWeapon && obj.EquippedOn().ThisPartWeapon() != null && !obj.IsEquippedOnPrimary())
          singleWeapon = false;
      });

      // Single/multi fighting.
      if (singleWeapon)
        Main.PointTracker.HandleTrainingAction(PlayerAction.SingleWeaponHit);
      else if (!weapon.IsEquippedOnPrimary())
        Main.PointTracker.HandleTrainingAction(PlayerAction.OffhandWeaponHit);

      return base.FireEvent(ev);
    }
  }
}