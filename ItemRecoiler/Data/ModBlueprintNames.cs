using System;
using XRL.World.Parts;

namespace ModoMods.ItemRecoiler.Data {
  public abstract class ModBlueprintNames {
    /// <summary>
    /// The modified <see cref="ProgrammableRecoiler"/> that players activate to recoil items.
    /// </summary>
    public const String Recoiler = "ModoMods_ItemRecoiler_Recoiler";
    
    /// <summary>The chest where recoiled items are sent to.</summary>
    public const String Receiver = "ModoMods_ItemRecoiler_Receiver"; }
}