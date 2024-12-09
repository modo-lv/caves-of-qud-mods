using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Parts.Skill;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>
  /// Generic melee weapon training is handled by <see cref="MeleeWeaponTrainer"/>,
  /// this handles axe-specific stuff.
  /// </summary>
  public class AxeTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      AfterAddSkillEvent.ID,
    };

    public override ISet<String> RegisterEventIds => new HashSet<String> {
      QudEventNames.AttackerGetDefenderDV,
      QudEventNames.AttackerAfterAttack,
    };

    public override Boolean HandleEvent(AfterAddSkillEvent ev) {
      if (ev.Skill is Axe_Cleave) {
        Output.DebugLog($"Gained [{ev.Skill}], re-applying [{this}] to ensure correct event order.");
        this.ParentObject.Also(player => {
          player.RemovePart<AxeTrainer>();
          player.RequirePart<AxeTrainer>();
        });
      }
      return base.HandleEvent(ev);
    }

    private Int32 _cleavedBeforeAttack;
    public override Boolean FireEvent(Event ev) {
      if (ev.ID == QudEventNames.AttackerGetDefenderDV) {
        this._cleavedBeforeAttack =
          ev.Defender()!.GetEffectDescendedFrom<IShatterEffect>()?.GetPenalty() ?? 0;
      }
      if (ev.ID == QudEventNames.AttackerAfterAttack && ev.IsChargeAttack()) {
        var cleavedAfterAttack = ev.Defender()!.GetEffectDescendedFrom<IShatterEffect>()?.GetPenalty() ?? 0;
        if (cleavedAfterAttack > this._cleavedBeforeAttack)
          ev.Attacker().Training()?.HandleTrainingAction(PlayerAction.ChargedCleave);
      }
      return base.FireEvent(ev);
    }
  }
}