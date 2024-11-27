using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Deft Throwing" skill.</summary>
  /// <remarks>
  /// Attached to the player to watch for equipment changes.
  /// When a throwing weapon is equipped, attaches to that item to listen for the throwing event.
  /// When the weapon is thrown, attaches itself to the intended target, to listen for the "hit landed" event.
  /// </remarks>
  public class DeftThrowingTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EquipperEquippedEvent.ID,
      DefenderMissileHitEvent.ID,
      AfterThrownEvent.ID,
    };
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      base.Register(obj, reg);
    }

    /// <summary>Attaches to the equipped item.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Actor?.IsPlayer() == true && ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<DeftThrowingTrainer>();
      return base.HandleEvent(ev);
    }
    
    /// <summary>Attaches to the intended target.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.BeforeThrown)
        return base.FireEvent(ev);

      // Attach this tracker to the target creature, to detect when it gets hit.
      var target = ev.GetGameObjectParameter("ApparentTarget");
      if (target?.IsCreature == true)
        target.RequirePart<DeftThrowingTrainer>();

      return base.FireEvent(ev);
    }
    
    /// <summary>Processes a successful hit.</summary>
    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      if (ev.Launcher != null
          || ev.Attacker?.IsPlayer() != true
          || ev.Defender?.IsCreature != true)
        return base.HandleEvent(ev);

      Output.DebugLog($"[{ev.Defender}] hit with with a thrown weapon.");
      Main.PointTracker.HandleTrainingAction(PlayerAction.ThrownWeaponHit);

      return base.HandleEvent(ev);
    }

    /// <summary>Removes this trainer from the weapon after it's been thrown.</summary>
    /// <remarks>
    /// Normally this isn't necessary, but something about enemies picking up the thrown weapons
    /// with this trainer attached causes random(?) <see cref="NullReferenceException"/>s.
    /// </remarks>
    public override Boolean HandleEvent(AfterThrownEvent ev) {
      ev.Item.RemovePart<DeftThrowingTrainer>();
      return base.HandleEvent(ev);
    }
  }

}