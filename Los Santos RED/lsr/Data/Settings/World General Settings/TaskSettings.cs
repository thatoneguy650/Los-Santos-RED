using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class TaskSettings : ISettingsDefaultable
{







    [Description("Minimum payment amount for the Gang Hit task from Officer Friendly")]
    public int OfficerFriendlyGangHitPaymentMin { get; set; }
    [Description("Maximum payment amount for the Gang Hit task from Officer Friendly")]
    public int OfficerFriendlyGangHitPaymentMax { get; set; }
    [Description("Complications Percent for the Gang Hit task from Officer Friendly")]
    public float OfficerFriendlyGangHitComplicationsPercentage { get; set; }





    [Description("Minimum payment amount for the Cop Hit task from Officer Friendly")]
    public int OfficerFriendlyCopHitPaymentMin { get; set; }
    [Description("Maximum payment amount for the Cop Hit task from Officer Friendly")]
    public int OfficerFriendlyCopHitPaymentMax { get; set; }
    [Description("Complications Percent for the Cop Hit task from Officer Friendly")]
    public float OfficerFriendlyCopHitComplicationsPercentage { get; set; }



    [Description("Minimum payment amount for the Witness Elimination task from Officer Friendly")]
    public int OfficerFriendlyWitnessEliminationPaymentMin { get; set; }
    [Description("Maximum payment amount for the Witness Elimination task from Officer Friendly")]
    public int OfficerFriendlyWitnessEliminationPaymentMax { get; set; }
    [Description("Complications Percent for the Witness Elimination task from Officer Friendly")]
    public float OfficerFriendlyWitnessEliminationComplicationsPercentage { get; set; }








    [Description("Minimum payment amount for the Gun Pickup task from Underground Guns")]
    public int UndergroundGunsGunPickupPaymentMin { get; set; }
    [Description("Maximum payment amount for the Gun Pickup task from Underground Guns")]
    public int UndergroundGunsGunPickupPaymentMax { get; set; }
    [Description("Complications Percent for the Gun Pickup task from Underground Guns")]
    public float UndergroundGunsGunPickupComplicationsPercentage { get; set; }

    [Description("Show blips on entities that are related to the task.")]
    public bool ShowEntityBlips { get; set; }
    [Description("Show help text pop ups on task status changes.")]
    public bool DisplayHelpPrompts { get; set; }
    public int VehicleExporterTransferPaymentMin { get; set; }
    public int VehicleExporterTransferPaymentMax { get; set; }
    public float VehicleExporterTransferComplicationsPercentage { get; set; }
    [Description("Percent that you will extort stores in Enemy Territory")]
    public float GangRacketeeringExtortionPercentage { get; set; }
    [Description("Complications Percent of stores calling for enemy backup during extortion.")]
    public float GangRacketeeringExtortionComplicationsPercentage { get; set; }
    [Description("Complications Percent of stores calling for police backup during racketeering.")]
    public float GangRacketeeringComplicationsPercentage { get; set; }
    [Description("Percent of torching stores in Enemy Territory")]
    public float GangArsonEnemyTurfPercentage { get; set; }

    public TaskSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

        OfficerFriendlyWitnessEliminationPaymentMin = 1500;// 10000;
        OfficerFriendlyWitnessEliminationPaymentMax = 2500;// 20000;
        OfficerFriendlyWitnessEliminationComplicationsPercentage = 45f;

        OfficerFriendlyGangHitPaymentMin = 2000;// 10000;
        OfficerFriendlyGangHitPaymentMax = 3000;// 15000;
        OfficerFriendlyGangHitComplicationsPercentage = 10f;

        OfficerFriendlyCopHitPaymentMin = 3500;// 15000;
        OfficerFriendlyCopHitPaymentMax = 4500;// 20000;
        OfficerFriendlyCopHitComplicationsPercentage = 25f;


        UndergroundGunsGunPickupPaymentMin = 2000;// 5000;
        UndergroundGunsGunPickupPaymentMax = 4000;// 10000;
        UndergroundGunsGunPickupComplicationsPercentage = 15f;

        ShowEntityBlips = true;
        DisplayHelpPrompts = true;

        VehicleExporterTransferPaymentMin = 1500;// 2000;
        VehicleExporterTransferPaymentMax = 2500;// 5000;
        VehicleExporterTransferComplicationsPercentage = 25f;
        GangRacketeeringExtortionPercentage = 25f;
        GangRacketeeringExtortionComplicationsPercentage = 10f;
        GangRacketeeringComplicationsPercentage = 5f;
        GangArsonEnemyTurfPercentage = 5f;
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

}