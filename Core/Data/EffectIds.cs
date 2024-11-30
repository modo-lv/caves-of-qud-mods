using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World.Effects;

namespace ModoMods.Core.Data {
  /// <summary>Known in-game effect IDs.</summary>
  public static class EffectIds {
    public static readonly Guid Bleeding = new Bleeding().ID;
    public static readonly Guid Poisoned = new Poisoned().ID;
    public static readonly Guid Illness = new Ill().ID;
    public static readonly ISet<Guid> Diseases = new HashSet<Guid> {
      new Glotrot().ID,
      new Ironshank().ID,
      new Monochrome().ID,
    };
    public static readonly Guid Lost = new Lost().ID;


    public static readonly ISet<Guid> HealthNegative = 
      new HashSet<Guid> { Bleeding, Poisoned, Illness }.Concat(Diseases).ToHashSet();
  }
}