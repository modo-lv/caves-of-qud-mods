using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World.Effects;

namespace ModoMods.Core.Data {
  /// <summary>Known in-game effect IDs.</summary>
  public static class QudEffectTypes {
    public static readonly Type Terrified = typeof(Terrified);
    public static readonly Type Confused = typeof(Confused);
    
    public static readonly Type Bleeding = typeof(Bleeding);
    public static readonly Type Poisoned = typeof(Poisoned);
    public static readonly Type Illness = typeof(Ill);
    public static readonly ISet<Type> Diseases = new HashSet<Type> {
      typeof(Glotrot),
      typeof(Ironshank),
      typeof(Monochrome),
    };

    public static readonly Type BasicCooking = typeof(BasicCookingEffect);
    
    public static readonly Type Lost = typeof(Lost);


    public static readonly ISet<Type> PhysicalNegative = 
      new HashSet<Type> { Bleeding, Poisoned, Illness }.Concat(Diseases).ToHashSet();
  }
}