using System;
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

    public override Set<Int32> WantEventIds => new Set<Int32> { EquipperEquippedEvent.ID };

    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon())
        ev.Item.RequirePart<DeftThrowingTrainer>();
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
            target.RequirePart<DeftThrowingTrainer>().Weapon = this.ParentObject;
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