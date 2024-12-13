using System;
using System.Collections.Generic;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using static ModoMods.Core.Data.QudEventNames;
using static ModoMods.Core.Data.QudSkillClasses;
using static ModoMods.SkillTraining.Data.PlayerAction;

namespace ModoMods.SkillTraining.Trainers.Melee {
  /// <summary>Trains melee weapon skills.</summary>
  public abstract class IMeleeWeaponTrainer : ModPart {
    public abstract String WeaponSkill { get; }
    
    public virtual void HandleAfterAttack(
      GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Boolean isCritical,
      Boolean isOffhand,
      Boolean isSingle
    ) {
      // Single/multi fighting.
      if (isSingle)
        attacker.Training()?.HandleTrainingAction(SingleWeaponHit);
      else if (isOffhand) {
        var action =
          attacker.HasSkill(MultiweaponExpertise)
            ? ExpertOffhand
            : attacker.HasSkill(MultiweaponFighting)
              ? SkilledOffhand
              : Offhand;
        attacker.Training()?.HandleTrainingAction(action);
      }
    }

    public override ISet<String> RegisterEventIds => new HashSet<String> {
      AttackerHit
    };

    public override Boolean FireEvent(Event ev) {
      var attacker = ev.Attacker().OnlyIf(it => it == this.ParentObject);
      var weapon = ev.Weapon();
      var defender = ev.Defender();
      var isCritical = ev.HasFlag("Critical");
      var skill = weapon?.GetWeaponSkill();
      if (ev.ID == AttackerHit) {
        if (ev.GetIntParameter("Penetrations") == 0
            || weapon == null || attacker == null || defender == null || skill == null
            || !attacker.CanTrainSkills()
            || !defender.IsCombatant()
            || weapon.EquippedOn()?.ThisPartWeapon() == null
           ) {
          return base.FireEvent(ev);
        }

        var isOffhand = attacker.GetPrimaryWeapon() != weapon;
        var isSingle = true;
        attacker.ForeachEquippedObject(obj => {
          if (isSingle && obj.EquippedOn().ThisPartWeapon() != null && !obj.IsEquippedOnPrimary())
            isSingle = false;
        });

        if (skill == this.WeaponSkill)
          this.HandleAfterAttack(attacker, defender, weapon, isCritical, isOffhand, isSingle);
      }
      return base.FireEvent(ev);

      /*

      if (action is not null) {
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

      return base.FireEvent(ev);*/
    }
  }
}