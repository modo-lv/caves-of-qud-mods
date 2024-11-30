using System;
using System.Collections.Generic;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using XRL;
using XRL.World;
using XRL.World.Effects;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Deft Throwing" skill.</summary>
  /// <remarks>Attached to the player to watch for equipment changes.</remarks>
  public class DeftThrowingTrainer : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EquipperEquippedEvent.ID };

    /// <summary>Attaches to the equipped item.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Actor?.IsPlayer() == true
          && ev.Actor?.HasEffect<Dominated>() == false
          && ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }


  /// <remarks>
  /// Attached to a thrown weapon when it is equipped, to listen for the throwing event.
  /// </remarks>
  public class ItemThrownDetector : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { AfterThrownEvent.ID };
    
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      base.Register(obj, reg);
    }

    /// <summary>Attach hit detection to the intended target.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.BeforeThrown 
          || ev.GetGameObjectParameter("Thrower")?.HasEffect<Dominated>() != false)
        return base.FireEvent(ev);

      // Attach this tracker to the target creature, to detect when it gets hit.
      var target = ev.GetGameObjectParameter("ApparentTarget");
      if (target?.IsCreature == true)
        target.RequirePart<ThrownHitDetector>();

      return base.FireEvent(ev);
    }

    /// <summary>Removes this detector from the weapon after it's been thrown.</summary>
    /// <remarks>
    /// Normally this isn't necessary, but something about enemies picking up the thrown weapons
    /// with this detector attached causes random(?) <see cref="NullReferenceException"/>s.
    /// </remarks>
    public override Boolean HandleEvent(AfterThrownEvent ev) {
      ev.Item.RemovePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }

  /// <remarks>
  /// Attached to the intended target when the weapon is thrown, to listen for the "hit landed" event.
  /// </remarks>
  public class ThrownHitDetector : ModPart {
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { DefenderMissileHitEvent.ID };

    /// <summary>Processes a successful hit.</summary>
    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      if (ev.Launcher != null
          || ev.Attacker?.IsPlayer() != true
          || ev.Attacker?.HasEffect<Dominated>() != false
          || ev.Defender?.IsCreature != true)
        return base.HandleEvent(ev);

      Output.DebugLog($"[{ev.Defender}] hit with with a thrown weapon.");
      Main.PointTracker.HandleTrainingAction(PlayerAction.ThrownWeaponHit);

      return base.HandleEvent(ev);
    }

  }

}