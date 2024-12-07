using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ModoMods.Core.Data;
using ModoMods.Core.Utils;
using ModoMods.SkillTraining.Data;
using ModoMods.SkillTraining.Utils;
using XRL;
using XRL.World;

namespace ModoMods.SkillTraining.Trainers {
  /// <summary>Trains "Deft Throwing" skill.</summary>
  /// <remarks>Attached to the player to watch for equipment changes.</remarks>
  public class DeftThrowingTrainer : ModPart {

    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      
      // Wire up items equipped before trainer was attached.  
      this.ParentObject.GetEquippedObjects()
        .Where(item => item.IsEquippedAsThrownWeapon())
        .Do(item => item.RequirePart<ItemThrownDetector>());
    }

    public override ISet<Int32> WantEventIds => new HashSet<Int32> { EquipperEquippedEvent.ID };

    /// <summary>Attaches to the equipped item.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon() && ev.Actor.CanTrainSkills())
        ev.Item.RequirePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }

  /// <remarks>
  /// Attached to a thrown weapon when it is equipped, to listen for the throwing event.
  /// </remarks>
  public class ItemThrownDetector : ModPart {
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, QudEventNames.ThrownProjectileHit);
      base.Register(obj, reg);
    }

    /// <summary>Attach hit detection to the intended target.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID != QudEventNames.ThrownProjectileHit)
        return base.FireEvent(ev);

      var defender = ev.Defender()?.OnlyIf(it =>
        it.IsCombatant() && it == ev.GetGameObjectParameter("ApparentTarget")
      );
      if (ev.Attacker().CanTrainSkills() && defender != null) {
        ev.Attacker().TrainingTracker()?.HandleTrainingAction(PlayerAction.ThrownWeaponHit);
      }

      return base.FireEvent(ev);
    }

    public override ISet<Int32> WantEventIds => new HashSet<Int32> { AfterThrownEvent.ID };

    /// <summary>Removes this detector from the weapon after it's been thrown.</summary>
    /// <remarks>
    /// Normally this isn't necessary, but something about enemies picking up the thrown weapons
    /// with this detector attached causes random(?) <see cref="NullReferenceException"/>s.
    /// </remarks>
    public override Boolean HandleEvent(AfterThrownEvent ev) {
      ev.Item.UnregisterPartEvent(this, QudEventNames.ThrownProjectileHit);
      ev.Item.RemovePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }
}