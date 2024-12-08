using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains melee weapon skills.</summary>
  public class MeleeWeaponTrainer : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      base.Register(obj, reg);
      obj.RegisterPartEvent(this, QudEventNames.AttackerAfterAttack);
    }

    public override Boolean FireEvent(Event ev) {
      var attacker = ev.Attacker().OnlyIf(it => it == this.ParentObject);
      var weapon = ev.Weapon();
      var defender = ev.Defender();
      if (ev.ID != QudEventNames.AttackerAfterAttack
          || ev.GetIntParameter("Penetrations") == 0
          || weapon == null || attacker == null || defender == null)
        return base.FireEvent(ev);

      var isCritical = ev.HasFlag("Critical");
      var skill = SkillUtils.SkillOrPower(weapon.GetWeaponSkill()).Class;

      if (skill == null
          || !attacker.CanTrainSkills()
          || attacker.HasSkill(skill)
          // Only equipped weapons train skills
          || weapon.EquippedOn()?.ThisPartWeapon() == null
          || !defender.IsCombatant()
         ) {
        return base.FireEvent(ev);
      }

      PlayerAction? action = skill switch {
        QudSkillClasses.Axe => PlayerAction.AxeHit,
        QudSkillClasses.Cudgel => PlayerAction.CudgelHit,
        QudSkillClasses.LongBlade => PlayerAction.LongBladeHit,
        QudSkillClasses.ShortBlade => PlayerAction.ShortBladeHit,
        null => null,
        _ => throw new Exception($"Unknown melee weapon skill: [{skill}].")
      };

      var isOffhand = attacker.GetPrimaryWeapon() != weapon;
      if (action is not null) {
        var modifier = 1m;
        if (isOffhand) modifier /= 2;
        if (isCritical) modifier *= 2;
        attacker.TrainingTracker()?.HandleTrainingAction(
          (PlayerAction) action,
          amountModifier: modifier
        );
      }

      var singleWeapon = true;
      attacker.ForeachEquippedObject(obj => {
        if (singleWeapon && obj.EquippedOn().ThisPartWeapon() != null && !obj.IsEquippedOnPrimary())
          singleWeapon = false;
      });

      // Single/multi fighting.
      if (singleWeapon)
        attacker.TrainingTracker()?.HandleTrainingAction(PlayerAction.SingleWeaponHit);
      else if (isOffhand)
        attacker.TrainingTracker()?.HandleTrainingAction(PlayerAction.OffhandWeaponHit);

      return base.FireEvent(ev);
    }
  }
}