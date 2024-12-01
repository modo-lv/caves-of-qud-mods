using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World.Effects;

namespace ModoMods.Core.Data {
  /// <summary>Known in-game effect IDs.</summary>
  public static class EffectTypes {
    public static readonly Int32 Terrified = new Terrified().GetEffectType();
    public static readonly Int32 Confused = new Confused().GetEffectType();
    
    public static readonly Int32 Bleeding = new Bleeding().GetEffectType();
    public static readonly Int32 Poisoned = new Poisoned().GetEffectType();
    public static readonly Int32 Illness = new Ill().GetEffectType();
    public static readonly ISet<Int32> Diseases = new HashSet<Int32> {
      new Glotrot().GetEffectType(),
      new Ironshank().GetEffectType(),
      new Monochrome().GetEffectType(),
    };

    public static readonly Int32 BasicCooking = new BasicCookingEffect().GetEffectType();
    
    public static readonly Int32 Lost = new Lost().GetEffectType();


    public static readonly ISet<Int32> PhysicalNegative = 
      new HashSet<Int32> { Bleeding, Poisoned, Illness }.Concat(Diseases).ToHashSet();
  }
}