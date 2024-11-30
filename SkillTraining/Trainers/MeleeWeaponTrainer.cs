using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains melee weapon skills.</summary>
  public class MeleeWeaponTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, EventNames.AttackerHit);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.AttackerHit)
        return base.FireEvent(ev);
      
      var isCritical = ev.HasFlag("Critical");
      var skill = SkillUtils.SkillOrPower(ev.Weapon()!.GetWeaponSkill()).Class;
      
      if (ev.Attacker()?.IsPlayer() != true
          || ev.Attacker()?.HasEffect<Dominated>() != false
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

      var isOffhand = ev.Weapon()?.IsEquippedInMainHand() != true;
      if (action is not null) {
        var modifier = 1m;
        if (isOffhand) modifier /= 2;
        if (isCritical) modifier *= 2;
        Main.PointTracker.HandleTrainingAction(
          (PlayerAction) action,
          amountModifier: modifier
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
      else if (isOffhand)
        Main.PointTracker.HandleTrainingAction(PlayerAction.OffhandWeaponHit);

      return base.FireEvent(ev);
    }
  }
}