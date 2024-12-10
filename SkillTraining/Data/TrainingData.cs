using System;
using System.Collections.Generic;
using ModoMods.SkillTraining.Utils;
using static ModoMods.SkillTraining.Data.PlayerAction;
using static ModoMods.Core.Data.QudSkillClasses;

namespace ModoMods.SkillTraining.Data {
  public struct TrainingData {
    public readonly String SkillClass;
    public readonly Decimal DefaultAmount;

    public TrainingData(String skillClass, Decimal defaultAmount) {
      this.SkillClass = skillClass;
      this.DefaultAmount = defaultAmount;
    }

    public static readonly IDictionary<PlayerAction, TrainingData> Data =
      new Dictionary<PlayerAction, TrainingData> {
        //@formatter:off
        #region Acrobatics
        { DodgeMelee,        new TrainingData(Spry,                0.10m) }, // 100
        { DodgeMissile,      new TrainingData(Acrobatics,          1.00m) }, // 75
        #endregion
        
        #region Axe
        { Cleave,            new TrainingData(AxeCleave,           1.00m) }, // 150
        { ChargedCleave,     new TrainingData(ChargingStrike,      0.50m) }, // 100
        #endregion
        
        #region Cudgel
        { ChargedStrike,     new TrainingData(ChargingStrike,      0.50m) }, // 100
        #endregion
        
        #region Customs and Folklore
        { RitualFirstRep,    new TrainingData(CustomsAndFolklore,  1.00m) },
        { RitualRep,         new TrainingData(CustomsAndFolklore,  0.25m) },
        { JournalReveal,     new TrainingData(CustomsAndFolklore,  0.25m) },
        { RifleTrash,        new TrainingData(TrashDivining,       0.25m) }, // 150
        #endregion 

        #region Endurance
        { EnduranceSprint,   new TrainingData(Longstrider,         0.25m) }, // 250
        { SufferDaze,        new TrainingData(Endurance,           1.00m) }, // 100
        { SufferStun,        new TrainingData(Endurance,           1.00m) }, // 100
        { SufferPoison,      new TrainingData(Endurance,           0.25m) }, // 100
        { Swim,              new TrainingData(Swimming,            0.15m) }, // 100
        { EndurePoison,      new TrainingData(PoisonTolerance,     0.25m) }, // 100
        { ExtremeTemp,       new TrainingData(Weathered,           1.00m) }, // 200
        { Juicing,           new TrainingData(Juicer,              1.00m) }, // 150
        #endregion

        #region Melee weapons
        { AxeHit,            new TrainingData(Axe,                 0.10m) },
        { CudgelHit,         new TrainingData(Cudgel,              0.10m) },
        { LongBladeHit,      new TrainingData(LongBlade,           0.20m) },
        { ShortBladeHit,     new TrainingData(ShortBlade,          0.20m) },
         
        { SingleWeaponHit,   new TrainingData(SingleWeaponFighting,0.10m) },
        { OffhandWeaponHit,  new TrainingData(MultiweaponFighting, 1.00m) },
        #endregion
        
        #region Missile combat
        { BowOrRifleHit,     new TrainingData(BowAndRifle,         0.50m) },
        { PistolHit,         new TrainingData(Pistol,              0.50m) },
        { HeavyWeaponHit,    new TrainingData(HeavyWeapon,         0.50m) },
        #endregion
        
        #region Cooking and Gathering
        { Cook,              new TrainingData(CookingAndGathering, 0.25m) }, // 100
        { CookTasty,         new TrainingData(CookingAndGathering, 1.00m) }, 
        { Harvest,           new TrainingData(CookingAndGathering, 0.15m) },
        { Butcher,           new TrainingData(CookingAndGathering, 0.75m) },
        { CookIngredients,   new TrainingData(Spicer,              0.75m) }, // 50
        #endregion
        
 
        #region Physic
        { Bandage,           new TrainingData(Physic,              1.00m) },
        { Recover,           new TrainingData(Physic,              1.00m) },
        { Inject,            new TrainingData(Physic,              1.00m) },
        #endregion
        
        #region Self-Discipline
        { DisciplineSprint,  new TrainingData(Conatus,             0.25m) }, // 150
        { SufferTerror,      new TrainingData(Lionheart,           0.35m) }, // 100
        { SufferConfusion,   new TrainingData(IronMind,            0.75m) }, // 100
        #endregion
         
        #region Tactics
        { DangerSprint,      new TrainingData(Tactics,             0.75m) }, // 100
        { ThrownWeaponHit,   new TrainingData(DeftThrowing,        0.75m) },
        #endregion
         
        #region Tinkering 
        { ExamineSuccess,    new TrainingData(Tinkering,           1.00m) },
        { RifleTrashSuccess, new TrainingData(Tinkering,           0.25m) },
        { DisassembleBit,    new TrainingData(Tinkering,           0.25m) },
        #endregion 
         
        { ShieldBlock,       new TrainingData(Shield,              0.25m) },
         
        { TradeItem,         new TrainingData(SnakeOiler,          0.01m) },
         
        #region Wayfaring
        { SufferLost,        new TrainingData(Wayfaring,           0.25m) },
        { RecoverLost,       new TrainingData(Wayfaring,           0.75m) },
        { WorldMapMove,      new TrainingData(Wayfaring,           0.15m) },
        #endregion
        
        //@formatter:on
      };


    public static TrainingData For(PlayerAction action) =>
      Data.GetOr(action, () =>
        throw new Exception($"No training data for player action [{action}].")
      );
  }
}