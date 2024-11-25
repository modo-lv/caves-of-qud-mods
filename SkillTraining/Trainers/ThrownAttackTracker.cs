﻿using System;
using Modo.SkillTraining.Data;
using Modo.SkillTraining.Utils;
using Wintellect.PowerCollections;
using XRL;
using XRL.World;

namespace Modo.SkillTraining.Trainers {
  /// <summary>Trains thrown weapon skill.</summary>
  /// <remarks>
  /// Attached to the player to watch for equipment changes.
  /// When a throwing weapon is equipped, attaches to that item to listen for the throwing event.
  /// When the weapon is thrown, the tracker attaches itself to the intended target,
  /// to listen for the damage taken event.
  /// </remarks>
  public class ThrownAttackTracker : ModPart {
    public GameObject? Weapon;

    public override Set<Int32> WantEventIds => new Set<Int32> { EquipperEquippedEvent.ID };

    /// <summary>Thrown weapon attack training.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<ThrownAttackTracker>();
      return base.HandleEvent(ev);
    }

    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, EventNames.BeforeThrown);
      obj.RegisterPartEvent(this, EventNames.TakeDamage);
      base.Register(obj, reg);
    }

    public override Boolean FireEvent(Event ev) {
      switch (ev.ID) {
        case EventNames.BeforeThrown: {
          // Attach this tracker to the target creature, to detect when it gets hit.
          var target = ev.GetParameter("ApparentTarget") as GameObject;
          if (target?.IsCreature == true)
            target.RequirePart<ThrownAttackTracker>().Weapon = this.ParentObject;
          break;
        }
        case EventNames.TakeDamage // Taking damage means the hit was successful.
          when !Main.Player.HasSkill(SkillClasses.DeftThrowing)
               && ev.Attacker() == Main.Player
               && ev.Defender()?.IsCreature == true:
          Output.DebugLog($"[{ev.Defender()}] hit with [{this.Weapon}].");
          Main.PointTracker.HandleTrainingAction(PlayerAction.ThrownWeaponHit);
          break;
      }

      return base.FireEvent(ev);
    }
  }
}