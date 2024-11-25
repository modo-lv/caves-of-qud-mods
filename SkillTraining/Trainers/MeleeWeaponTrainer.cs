using System;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains melee weapon skills.</summary>
  public class MeleeWeaponTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.AttackerHit);
    }

    public override Boolean FireEvent(Event ev) {
      var isCritical = ev.HasFlag("Critical");
      var skill = SkillUtils.SkillOrPower(ev.Weapon()!.GetWeaponSkill()).Class;
      
      if (ev.Attacker()?.IsPlayer() != true
          || ev.Defender()?.IsPlayer() == true
          || skill == null
          || Main.Player.HasSkill(skill)
          // Only equipped weapons train skills
          || ev.Weapon()?.EquippedOn()?.ThisPartWeapon() == null) {
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
      if (action is not null && ev.Weapon()?.IsEquippedInMainHand() == true) {
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
      else if (ev.Weapon()?.IsEquippedOnPrimary() == true)
        Main.PointTracker.HandleTrainingAction(PlayerAction.OffhandWeaponHit);

      return base.FireEvent(ev);
    }
  }
}