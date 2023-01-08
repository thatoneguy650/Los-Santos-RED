using System.ComponentModel;

public class PoliceSettings : ISettingsDefaultable
{

    [Description("Show the vanilla police blip")]
    public bool ShowVanillaBlips { get; set; }
    [Description("Enable or disable the non-vanilla wanted system. (Not recommended to disable)")]
    public bool UseFakeWantedLevelSystem { get; set; }
    [Description("If enabled, only LSR will be able to set the wanted level.")]
    public bool TakeExclusiveControlOverWantedLevel { get; set; }
    [Description("If enabled, one star wanted levels not set by the mod will be ignored.")]
    public bool TakeExclusiveControlOverWantedLevelOneStarAndBelow { get; set; }
    [Description("If enabled, any observed crime that results in deadly chase will automatically increase the wanted level to 3.")]
    public bool DeadlyChaseRequiresThreeStars { get; set; }
    [Description("Maximum wanted level allowed. Maximum = 6.")]
    public int MaxWantedLevel { get; set; }
    [Description("Enable or Disable accuracy override")]
    public bool OverrideAccuracy { get; set; }
    [Description("Enable or disable health override")]
    public bool OverrideHealth { get; set; }
    [Description("Enable or disable armor override")]
    public bool OverrideArmor { get; set; }
    [Description("Enable or disable auto load setting. (Not recommended to disable)")]
    public bool ManageLoadout { get; set; }
    [Description("Distance (in meters) that police can see.")]
    public float SightDistance { get; set; }
    [Description("Distance (in meters) that police can hear gunshots.")]
    public float GunshotHearingDistance { get; set; }
    [Description("Additional distance (in meters) that police can see when in a helicopter.")]
    public float SightDistance_Helicopter { get; set; }
    [Description("Additional distance (in meters) that police can see when in a helicopter and you are wanted.")]
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; }
    [Description("Enable or disable cops knowing your position when you fire a weapon within their hearing distance. Realistic and useful for more cover based shootouts.")]
    public bool KnowsShootingSourceLocation { get; set; }
    [Description("Distance (in meters) that the police will instantly recognize you if you are seen.")]
    public float AutoRecognizeDistance { get; set; }
    [Description("Distance (in meters) that police will instance recongize you even if technically unseen. Useful to stop cops from being obvivious when you are directly behind them. Set to 0 to disable.")]
    public float AlwaysRecognizeDistance { get; set; }
    [Description("Time (in ms) that you will still be considered seen after police have lost sight.")]
    public int RecentlySeenTime { get; set; }
    [Description("Distance (in meters) police need to be within to bust the player.")]
    public float BustDistance { get; set; }
    [Description("Enable or disable dropping your current weapon when busted and armed.")]
    public bool DropWeaponWhenBusted { get; set; }
    [Description("Fine amount when caught at 1 star with no other serious crimes.")]
    public int GeneralFineAmount { get; set; }
    [Description("Additional fine amount when you are caught at one star and are driving without a valid license.")]
    public int DrivingWithoutLicenseFineAmount { get; set; }
    [Description("Additional fine amount added when you fail at talking your way out of a ticket.")]
    public int TalkFailFineAmount { get; set; }
    [Description("Enable or disable wanted level increasing by killing police.")]
    public bool WantedLevelIncreasesByKillingPolice { get; set; }
    [Description("Minimum police killed before being given a four star wanted level.")]
    public int KillLimit_Wanted4 { get; set; }
    [Description("Minimum police killed before being given a five star wanted level.")]
    public int KillLimit_Wanted5 { get; set; }
    [Description("Minimum police killed before being given a six star wanted level.")]
    public int KillLimit_Wanted6 { get; set; }
    [Description("Enable or diable wanted level increasing over time.")]
    public bool WantedLevelIncreasesOverTime { get; set; }
    [Description("Time (in ms) at wanted level 1 required to increase wanted level to 2.")]
    public uint WantedLevelIncreaseTime_FromWanted1 { get; set; }
    [Description("Time (in ms) at wanted level 2 required to increase wanted level to 3.")]
    public uint WantedLevelIncreaseTime_FromWanted2 { get; set; }
    [Description("Time (in ms) at wanted level 3 required to increase wanted level to 4.")]
    public uint WantedLevelIncreaseTime_FromWanted3 { get; set; }
    [Description("Time (in ms) at wanted level 4 required to increase wanted level to 5.")]
    public uint WantedLevelIncreaseTime_FromWanted4 { get; set; }
    [Description("Time (in ms) at wanted level 5 required to increase wanted level to 6 (maximum).")]
    public uint WantedLevelIncreaseTime_FromWanted5 { get; set; }


    [Description("Time in millisecond that each wanted level adds to the search time. Ex. SearchTimeMultiplier of 30000 at 2 Stars would take 60 seconds to expire. At 4 stars, 120 seconds.")]
    public uint SearchTimeMultiplier { get; set; }


    public bool ForceDefaultWeaponAnimations { get; set; }
    public bool AllowPoliceToCallEMTsOnBodies { get; set; }

    public PoliceSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {
        GeneralFineAmount = 500;
        TalkFailFineAmount = 500;
        DrivingWithoutLicenseFineAmount = 1000;



        TakeExclusiveControlOverWantedLevel = true;
        TakeExclusiveControlOverWantedLevelOneStarAndBelow = false;

        ShowVanillaBlips = false;
        OverrideAccuracy = true;
        AutoRecognizeDistance = 15f;
        AlwaysRecognizeDistance = 7f;
        RecentlySeenTime = 15000;
  

        ManageLoadout = true;
        BustDistance = 4f;//5f;
        OverrideHealth = true;

        WantedLevelIncreasesByKillingPolice = true;
        KillLimit_Wanted4 = 5;
        KillLimit_Wanted5 = 10;
        KillLimit_Wanted6 = 20;

        WantedLevelIncreasesOverTime = true;
        WantedLevelIncreaseTime_FromWanted1 = 90000;//1.5 min
        WantedLevelIncreaseTime_FromWanted2 = 180000;//3 mins
        WantedLevelIncreaseTime_FromWanted3 = 270000;//4.5 mins
        WantedLevelIncreaseTime_FromWanted4 = 360000;//6 minutes
        WantedLevelIncreaseTime_FromWanted5 = 600000;//10 minutes

        SightDistance = 90f;//70f;
        GunshotHearingDistance = 125f;
        SightDistance_Helicopter = 175f;
        SightDistance_Helicopter_AdditionalAtWanted = 100f;

        DeadlyChaseRequiresThreeStars = true;
        MaxWantedLevel = 6;

        KnowsShootingSourceLocation = true;
        UseFakeWantedLevelSystem = true;

        DropWeaponWhenBusted = false;

        ForceDefaultWeaponAnimations = true;




#if DEBUG
        DropWeaponWhenBusted = true;
#endif



        SearchTimeMultiplier = 20000;

        AllowPoliceToCallEMTsOnBodies = true;
    }
}