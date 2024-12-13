using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts.Skill;
using static ModoMods.Core.Data.QudEventNames;
using static ModoMods.SkillTraining.Data.PlayerAction;

namespace ModoMods.SkillTraining.Trainers.Melee {
  public class AxeTrainer : IMeleeWeaponTrainer {
    public override String? WeaponSkill => QudSkillClasses.Axe;

    public override ISet<String> RegisterEventIds => base.RegisterEventIds.Concat(new HashSet<String> {
      AttackerHit,
    }).ToHashSet();

    public override Boolean FireEvent(Event ev) {
      if (ev.ID == AttackerHit) {
        Output.Log(ev.GetStringParameter("Properties"));
        ev.Defender()?.RequirePart<CleaveDetector>().Also(cleave =>
          cleave.IsCharging = ev.IsChargeAttack()
        );
      }
      return base.FireEvent(ev);
    }

    public override void HandleAfterAttack(GameObject attacker,
      GameObject defender,
      GameObject weapon,
      Boolean isCritical,
      Boolean isOffhand,
      Boolean isSingle
    ) {
      attacker.Training()?.HandleTrainingAction(AxeHit, isCritical ? 2 : 1);
      base.HandleAfterAttack(attacker, defender, weapon, isCritical, isOffhand, isSingle);
    }
  }

  public class CleaveDetector : ModPart {
    public Boolean IsCharging;
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EffectAppliedEvent.ID };
    public override Boolean HandleEvent(EffectAppliedEvent ev) {
      if (ev.Effect is ShatterArmor cleave) {
        cleave.Owner.Training()?.HandleTrainingAction(CleaveHit);
        if (this.IsCharging)
          cleave.Owner.Training()?.HandleTrainingAction(ChargedCleaveHit);
        this.IsCharging = false;
      }
      return base.HandleEvent(ev);
    }
  }

  // ReSharper disable InconsistentNaming, UnusedMember.Global
  [HarmonyPatch] public static class DismemberDetector {
    [HarmonyPostfix][HarmonyPatch(typeof(Axe_Dismember), nameof(Axe_Dismember.Dismember))]
    public static void AfterDismember(ref Boolean __result, GameObject Attacker) {
      if (__result && Attacker.CanTrainSkills()) {
        if (Attacker.HasSkill(QudSkillClasses.Decapitate))
          Attacker.Training()?.HandleTrainingAction(DecapitateHit);
        else if (Attacker.HasSkill(QudSkillClasses.Dismember))
          Attacker.Training()?.HandleTrainingAction(SkilledDismemberHit);
        else
          Attacker.Training()?.HandleTrainingAction(DismemberHit);
      }
    }
  }
}