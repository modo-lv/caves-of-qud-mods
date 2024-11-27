using System;
using XRL.World.Parts;

namespace ModoMods.LootRecoil.Data {
  public abstract class LrBlueprintNames {
    /// <summary>
    /// The modified <see cref="ProgrammableRecoiler"/> that players activate to recoil items.
    /// </summary>
    public const String Recoiler = "ModoMods:LootRecoil:Recoiler";
    
    /// <summary>The chest where recoiled items are sent to.</summary>
    public const String Receiver = "ModoMods:LootRecoil:Receiver";
    
    /// <summary>The temporary chest that does the actual teleporting.</summary>
    public const String Transmitter = "ModoMods:LootRecoil:Transmitter";
  }
}