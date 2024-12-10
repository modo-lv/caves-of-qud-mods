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
  /// Detects charged cleave and daze/stun, and trains Charged Strike.
  /// </summary>
  public class ChargedStrikeTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      AfterAddSkillEvent.ID,
    };

    public override ISet<String> RegisterEventIds => new HashSet<String> {
      QudEventNames.AttackerGetDefenderDV,
      QudEventNames.AttackerAfterAttack,
    };

    public override Boolean HandleEvent(AfterAddSkillEvent ev) {
      if (ev.Skill is Axe_Cleave or Cudgel_Bludgeon) {
        Output.DebugLog($"Gained [{ev.Skill}], re-applying [{this}] to ensure correct event order.");
        this.ParentObject.Also(player => {
          player.RemovePart<ChargedStrikeTrainer>();
          player.RequirePart<ChargedStrikeTrainer>();
        });
      }
      return base.HandleEvent(ev);
    }

    private Int32 _cleavedBeforeAttack;
    private Boolean _dazedBeforeAttack;
    private Boolean _stunnedBeforeAttack;
    public override Boolean FireEvent(Event ev) {
      if (ev.ID == QudEventNames.AttackerGetDefenderDV) {
        // Axe
        this._cleavedBeforeAttack =
          ev.Defender()!.GetEffectDescendedFrom<IShatterEffect>()?.GetPenalty() ?? 0;
        // Cudgel
        this._dazedBeforeAttack = ev.Defender()?.HasEffect<Dazed>() == true;
        this._stunnedBeforeAttack = ev.Defender()?.HasEffect<Stun>() == true;
      }
      if (ev.ID == QudEventNames.AttackerAfterAttack && ev.IsChargeAttack()) {
        // Axe
        var cleavedAfterAttack = ev.Defender()!.GetEffectDescendedFrom<IShatterEffect>()?.GetPenalty() ?? 0;
        if (cleavedAfterAttack > this._cleavedBeforeAttack)
          ev.Attacker().Training()?.HandleTrainingAction(PlayerAction.ChargedCleave);
        
        // Cudgel
        var defenderGotDazed =
          !this._dazedBeforeAttack && ev.Defender()?.HasEffect<Dazed>() == true;
        var defenderGotStunned =
          this._dazedBeforeAttack && !this._stunnedBeforeAttack && ev.Defender()?.HasEffect<Stun>() == true;
        if (defenderGotDazed || defenderGotStunned)
          ev.Attacker().Training()?.HandleTrainingAction(PlayerAction.ChargedStrike);
      }
      return base.FireEvent(ev);
    }
  }
}