using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

    public TaskSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        OfficerFriendlyGangHitPaymentMin = 10000;
        OfficerFriendlyGangHitPaymentMax = 35000;
        OfficerFriendlyGangHitComplicationsPercentage = 10f;

        OfficerFriendlyCopHitPaymentMin = 20000;
        OfficerFriendlyCopHitPaymentMax = 45000;
        OfficerFriendlyCopHitComplicationsPercentage = 5f;

        OfficerFriendlyWitnessEliminationPaymentMin = 10000;
        OfficerFriendlyWitnessEliminationPaymentMax = 20000;
        OfficerFriendlyWitnessEliminationComplicationsPercentage = 30f;

        UndergroundGunsGunPickupPaymentMin = 5000;
        UndergroundGunsGunPickupPaymentMax = 10000;
        UndergroundGunsGunPickupComplicationsPercentage = 15f;

        ShowEntityBlips = false;
        DisplayHelpPrompts = true;




    }

}