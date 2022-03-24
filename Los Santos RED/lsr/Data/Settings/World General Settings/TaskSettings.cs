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

    [Description("Minimum payment amount for the Cop Hit task from Officer Friendly")]
    public int OfficerFriendlyCopHitPaymentMin { get; set; }
    [Description("Maximum payment amount for the Cop Hit task from Officer Friendly")]
    public int OfficerFriendlyCopHitPaymentMax { get; set; }




    [Description("Minimum payment amount for the Witness Elimination task from Officer Friendly")]
    public int OfficerFriendlyWitnessEliminationPaymentMin { get; set; }
    [Description("Maximum payment amount for the Witness Elimination task from Officer Friendly")]
    public int OfficerFriendlyWitnessEliminationPaymentMax { get; set; }






    [Description("Complications Percent Gun Pickup task from Underground Guns")]
    public float UndergroundGunsGunPickupComplicationsPercentage { get; set; }

    [Description("Minimum payment amount for the Gun Pickup task from Underground Guns")]
    public int UndergroundGunsGunPickupPaymentMin { get; set; }
    [Description("Maximum payment amount for the Gun Pickup task from Underground Guns")]
    public int UndergroundGunsGunPickupPaymentMax { get; set; }



    public TaskSettings()
    {
        SetDefault();
#if DEBUG

        UndergroundGunsGunPickupComplicationsPercentage = 100f;
#else

#endif
    }
    public void SetDefault()
    {
        OfficerFriendlyGangHitPaymentMin = 10000;
        OfficerFriendlyGangHitPaymentMax = 35000;


        OfficerFriendlyCopHitPaymentMin = 20000;
        OfficerFriendlyCopHitPaymentMax = 45000;


        OfficerFriendlyWitnessEliminationPaymentMin = 10000;
        OfficerFriendlyWitnessEliminationPaymentMax = 20000;

        UndergroundGunsGunPickupPaymentMin = 5000;
        UndergroundGunsGunPickupPaymentMax = 10000;


        UndergroundGunsGunPickupComplicationsPercentage = 30f;



    }

}