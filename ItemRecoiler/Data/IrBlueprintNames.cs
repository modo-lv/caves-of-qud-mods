using System;
using XRL.World.Parts;

namespace ModoMods.ItemRecoiler.Data {
  public abstract class IrBlueprintNames {
    /// <summary>
    /// The modified <see cref="ProgrammableRecoiler"/> that players activate to recoil items.
    /// </summary>
    public const String Recoiler = "ModoMods:ItemRecoiler:Recoiler";
    
    /// <summary>The chest where recoiled items are sent to.</summary>
    public const String Receiver = "ModoMods:ItemRecoiler:Receiver";
    
    /// <summary>The temporary chest that does the actual teleporting.</summary>
    public const String Transmitter = "ModoMods:ItemRecoiler:Transmitter";
  }
}