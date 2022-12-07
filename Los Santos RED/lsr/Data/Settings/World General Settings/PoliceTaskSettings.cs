using System.ComponentModel;

public class PoliceTaskSettings : ISettingsDefaultable
{

    [Description("Allows tasking of ambient police pedestrians in the world. (Not recommended to disable)")]
    public bool ManageTasking { get; set; }


    [Description("Allow cops to use the drive by sight driving flag when locating.")]
    public bool AllowDriveBySightDuringLocate { get; set; }
    [Description("Distance to allow drive by sight during locate.")]
    public float DriveBySightDuringLocateDistance { get; set; }
    [Description("Allow cops to use the drive by sight driving flag when investigating.")]
    public bool AllowDriveBySightDuringInvestigate { get; set; }
    [Description("Distance to allow drive by sight during investigate.")]
    public float DriveBySightDuringInvestigateDistance { get; set; }

    [Description("Allow cops to use the drive by sight driving flag when chasing.")]
    public bool AllowDriveBySightDuringChase { get; set; }
    [Description("Distance to allow drive by sight during chase.")]
    public float DriveBySightDuringChaseDistance { get; set; }


    [Description("Enable or disable chase assists to allow the police to better keep up with the player")]
    public bool AllowChaseAssists { get; set; }
    [Description("Enable or disable clearing or offscreen non-mission vehicles that are blocking police vehicles")]
    public bool AllowFrontVehicleClearAssist { get; set; }
    [Description("Enable or disable collision proffing for police vehicles")]
    public bool AllowReducedCollisionPenaltyAssist { get; set; }
    [Description("Enable or disable increased power for police vehicles")]
    public bool AllowPowerAssist { get; set; }


    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 1 star.")]
    public int InvestigationRespondingOfficers_Wanted1 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 2 star.")]
    public int InvestigationRespondingOfficers_Wanted2 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 3 star.")]
    public int InvestigationRespondingOfficers_Wanted3 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 4 star.")]
    public int InvestigationRespondingOfficers_Wanted4 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 5 star.")]
    public int InvestigationRespondingOfficers_Wanted5 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 6 star.")]
    public int InvestigationRespondingOfficers_Wanted6 { get; set; }

    [Description("Percentage of cops that have an idea of your location during search mode.")]
    public float SixthSensePercentage { get; set; }
    [Description("Percentage of cops in a helicopter that have an idea of your location during search mode.")]
    public float SixthSenseHelicopterPercentage { get; set; }
    [Description("Percentage of cops that have an idea of your location during search mode and are already close to you when it starts.")]
    public float SixthSensePercentageClose { get; set; }
    [Description("Percentage of search mode that cops will be able to use their sixth sense. A value of 0.7 means they would be able to use their sixth sense powers for the first 30% of search mode (1.0 is none, 0.0 is the entire search mode).")]
    public float SixthSenseSearchModeLimitPercentage { get; set; }


    [Description("Enables or disables siege mode when you are in a building the police saw you enter. Allows police to keep an active search mode when they cannot directly see you.")]
    public bool AllowSiegeMode { get; set; }
    [Description("When in siege mode, how close the entry team will attempt to get to the player.")]
    public float SiegeGotoDistance { get; set; }
    [Description("When in siege mode, how close the entry team will attempt to get before aiming at the player.")]
    public float SiegeAimDistance { get; set; }
    [Description("Percentage of cops that will be set to siege mode. If in siege mode, the cop will be forced to attempt entry. If not in siege mode cops may or may not attempt entry depending on vanilla AI.")]
    public float SiegePercentage { get; set; }
    [Description("Driver aggressiveness modifer. Min 0.0 Max 1.0")]
    public float DriverAggressiveness { get; set; }
    [Description("Driver ability modifer. Min 0.0 Max 1.0")]
    public float DriverAbility { get; set; }
    [Description("Driver racing modifer. Min 0.0 Max 1.0")]
    public float DriverRacing { get; set; }
    [Description("If enabled, police can respond without a civilian report.")]
    public bool AllowRespondingWithoutCallIn { get; set; }
    [Description("If enabled, LSR set the siren state for any vehicle an AI Cop is in.")]
    public bool AllowSettingSirenState { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during vehicle chase.")]
    public bool BlockEventsDuringVehicleChase { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during chase.")]
    public bool BlockEventsDuringChase { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during investigate.")]
    public bool BlockEventsDuringInvestigate { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during vehicleinvestigate.")]
    public bool BlockEventsDuringVehicleInvestigate { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during locate.")]
    public bool BlockEventsDuringLocate { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during vehicle locate.")]
    public bool BlockEventsDuringVehicleLocate { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during kill.")]
    public bool BlockEventsDuringKill { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during idle.")]
    public bool BlockEventsDuringIdle { get; set; }
    [Description("If enabled, LSR will not totally control Ped AI during AI chase.")]
    public bool BlockEventsDuringAIChase { get; set; }
    [Description("If enabled, the ped will be given the steer around native when driving.")]
    public bool SetSteerAround { get; set; }
    [Description("Time (in ms) between target updates during tasking. CURRENTLY DISABLED.")]
    public uint TargetUpdateTime { get; set; }



    public bool EnableCombatAttributeCanInvestigate { get; set; }
    public bool EnableCombatAttributeDisableEntryReactions { get; set; }
    public bool EnableCombatAttributeCanFlank { get; set; }
    public bool EnableCombatAttributeCanChaseOnFoot { get; set; }
    public bool OverrrideTargetLossResponse { get; set; }
    public int OverrrideTargetLossResponseValue { get; set; }
    public bool EnableConfigFlagAlwaysSeeAproachingVehicles { get; set; }
    public bool EnableConfigFlagDiveFromApproachingVehicles { get; set; }
    public bool AllowMinorReactions { get; set; }

    public PoliceTaskSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {
        ManageTasking = true;

        AllowChaseAssists = true;
        AllowFrontVehicleClearAssist = true;
        AllowReducedCollisionPenaltyAssist = true;
        AllowPowerAssist = true;

        AllowDriveBySightDuringChase = true;
        DriveBySightDuringChaseDistance = 150f;
        AllowDriveBySightDuringInvestigate = true;
        DriveBySightDuringInvestigateDistance = 150f;
        AllowDriveBySightDuringLocate = true;
        DriveBySightDuringLocateDistance = 150f;

    


        InvestigationRespondingOfficers_Wanted1 = 2;
        InvestigationRespondingOfficers_Wanted2 = 4;
        InvestigationRespondingOfficers_Wanted3 = 6;
        InvestigationRespondingOfficers_Wanted4 = 8;
        InvestigationRespondingOfficers_Wanted5 = 10;
        InvestigationRespondingOfficers_Wanted6 = 14;




        SixthSensePercentage = 70f;
        SixthSenseHelicopterPercentage = 90f;
        SixthSenseSearchModeLimitPercentage = 0.7f;



        AllowSiegeMode = true;
        SiegeGotoDistance = 8f;
        SiegeAimDistance = 15f;
        SiegePercentage = 80f;




        AllowRespondingWithoutCallIn = true;




        AllowSettingSirenState = true;

        BlockEventsDuringVehicleChase = true;
        BlockEventsDuringChase = true;
        BlockEventsDuringInvestigate = true;
        BlockEventsDuringVehicleInvestigate = true;
        BlockEventsDuringLocate = true;
        BlockEventsDuringVehicleLocate = true;
        BlockEventsDuringKill = true;
        BlockEventsDuringIdle = true;
        BlockEventsDuringAIChase = true;

        DriverAggressiveness = 0.75f;
        DriverAbility = 1.0f;
        DriverRacing = 0.0f;

        SetSteerAround = false;

        TargetUpdateTime = 1000;


        EnableCombatAttributeCanInvestigate = true;
        EnableCombatAttributeDisableEntryReactions = true;
        EnableCombatAttributeCanFlank = true;
        EnableCombatAttributeCanChaseOnFoot = true;

        OverrrideTargetLossResponse = true;
        OverrrideTargetLossResponseValue = 2;


        EnableConfigFlagAlwaysSeeAproachingVehicles = true;
        EnableConfigFlagDiveFromApproachingVehicles = true;
        AllowMinorReactions = true;

    }
}