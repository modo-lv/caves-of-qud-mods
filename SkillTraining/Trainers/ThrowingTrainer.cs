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
  public class ThrowingTrainer : ModPart {
    /// <remarks>
    /// Makes sure that currently equipped item is wired up as well,
    /// in case it was equipped before trainer attachment or without raising an event. 
    /// </remarks>
    public override void AddedAfterCreation() {
      base.AddedAfterCreation();
      this.ParentObject.GetEquippedObjects()
        .Where(item => item.IsEquippedAsThrownWeapon())
        .Do(item => item.RequirePart<ItemThrownDetector>());
    }
    
    public override ISet<Int32> WantEventIds => new HashSet<Int32> {
      EquipperEquippedEvent.ID,
    };

    /// <summary>Attaches to the equipped item.</summary>
    public override Boolean HandleEvent(EquipperEquippedEvent ev) {
      if (ev.Item.IsEquippedAsThrownWeapon() && ev.Actor.CanTrainSkills())
        ev.Item.RequirePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }

  /// <remarks>Attached to a thrown weapon when it is equipped, to listen for the throw hit event.</remarks>
  public class ItemThrownDetector : ModPart {
    
    public override ISet<Int32> WantEventIds => new HashSet<Int32> { AfterThrownEvent.ID };
    
    public override void Register(GameObject obj, IEventRegistrar reg) {
      obj.RegisterPartEvent(this, QudEventNames.ThrownProjectileHit);
      base.Register(obj, reg);
    }
    

    /// <summary>Reward training points for successful hit with the thrown wepaon.</summary>
    public override Boolean FireEvent(Event ev) {
      if (ev.ID != QudEventNames.ThrownProjectileHit)
        return base.FireEvent(ev);

      var defender = ev.Defender()?.OnlyIf(it =>
        it.IsCombatant() && it == ev.GetGameObjectParameter("ApparentTarget")
      );
      if (ev.Attacker().CanTrainSkills() && defender != null) {
        ev.Attacker().Training()?.HandleTrainingAction(PlayerAction.ThrownWeaponHit);
      }

      return base.FireEvent(ev);
    }


    /// <summary>Removes this detector from the weapon after it's been thrown.</summary>
    /// <remarks>
    /// This was added due to some <see cref="NullReferenceException"/>s, but that was a few rewrites ago.
    /// TODO: Check if things aren't working OK without manual removal now.
    /// </remarks>
    public override Boolean HandleEvent(AfterThrownEvent ev) {
      ev.Item.UnregisterPartEvent(this, QudEventNames.ThrownProjectileHit);
      ev.Item.RemovePart<ItemThrownDetector>();
      return base.HandleEvent(ev);
    }
  }
}