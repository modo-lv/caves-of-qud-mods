using System;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Deft Throwing" skill.</summary>
  /// <remarks>
  /// Attached to the player to watch for equipment changes.
  /// When a throwing weapon is equipped, attaches to that item to listen for the throwing event.
  /// When the weapon is thrown, the tracker attaches itself to the intended target,
  /// to listen for the damage taken event.
  /// </remarks>
  public class DeftThrowingTrainer : ModPart {
    public GameObject? Weapon;

    public override Set<Int32> WantEventIds => new Set<Int32> {
      EquipperEquippedEvent.ID,
      DefenderMissileHitEvent.ID,
    };

    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<DeftThrowingTrainer>();
      return base.HandleEvent(ev);
    }

    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      base.Register(obj, reg);
    }
    
    public override Boolean HandleEvent(DefenderMissileHitEvent ev) {
      if (ev.Launcher != null
          || ev.Attacker?.IsPlayer() != true
          || ev.Defender?.IsCreature != true)
        return base.HandleEvent(ev);
      
      Output.DebugLog($"[{ev.Defender}] hit with [{this.Weapon}].");
      Main.PointTracker.HandleTrainingAction(PlayerAction.ThrownWeaponHit);
      
      return base.HandleEvent(ev);
    }

    public override Boolean FireEvent(Event ev) {
      if (ev.ID != EventNames.BeforeThrown)
        return base.FireEvent(ev);
      
      // Attach this tracker to the target creature, to detect when it gets hit.
      var target = ev.GetParameter("ApparentTarget") as GameObject;
      if (target?.IsCreature == true)
        target.RequirePart<DeftThrowingTrainer>().Weapon = this.ParentObject;

      return base.FireEvent(ev);
    }
  }

}