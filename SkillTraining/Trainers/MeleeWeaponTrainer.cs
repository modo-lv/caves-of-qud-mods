using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;
using XRL.World.Parts.Skill;
using static ModoMods.Core.Data.QudEventProperties;
using static ModoMods.Core.Data.QudSkillClasses;

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
      var skill = weapon.GetWeaponSkill();

      if (skill == null
          || !attacker.CanTrainSkills()
          // Only equipped weapons train skills
          || weapon.EquippedOn()?.ThisPartWeapon() == null
          || !defender.IsCombatant()
         ) {
        return base.FireEvent(ev);
      }

      PlayerAction? action = skill switch {
        QudSkillClasses.Axe => PlayerAction.AxeHit,
        QudSkillClasses.Cudgel => PlayerAction.CudgelHit,
        LongBlade =>
          attacker.HasSkill(LongBlade)
            ? PlayerAction.StanceHit
            : PlayerAction.LongBladeHit,
        ShortBlade => PlayerAction.ShortHit,
        null => null,
        _ => throw new Exception($"Unknown melee weapon skill: [{skill}].")
      };

      var isOffhand = attacker.GetPrimaryWeapon() != weapon;
      if (action is not null) {
        var modifier = 1m;
        if (isOffhand) modifier /= 2;
        if (isCritical) {
          if (action == PlayerAction.CudgelHit && attacker.HasSkill(QudSkillClasses.Cudgel))
            action = PlayerAction.CudgelSkilledCrit;
          modifier *= 2;
        }
        attacker.Training()?.HandleTrainingAction(
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
        attacker.Training()?.HandleTrainingAction(PlayerAction.SingleWeaponHit);
      else if (isOffhand) {
        action =
          attacker.HasSkill(MultiweaponExpertise)
            ? PlayerAction.ExpertOffhand
            : attacker.HasSkill(MultiweaponFighting)
              ? PlayerAction.ProficientOffhand
              : PlayerAction.Offhand;
        attacker.Training()?.HandleTrainingAction((PlayerAction) action);
        // Jab
        if (skill == ShortBlade && !ev.HasProperty(Flurrying) && attacker.HasSkill(ShortBlade))
          attacker.Training()?.HandleTrainingAction(PlayerAction.ShortOffhandHit);
      }

      return base.FireEvent(ev);
    }
  }
}