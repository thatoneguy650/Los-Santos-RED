using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VehicleSettings : ISettingsDefaultable
{
    [Description("Minimum time that must elapse before an alarmed car is reported stolen by the owner. A random number is chosen between the minimum and maximum values. Assumes the owner eventually comes back and figures out they are missing a car. Alarmed owners will be more likely to find it faster.")]
    public uint AlarmedCarTimeToReportStolenMin { get; set; }
    [Description("Maximum time that must elapse before an alarmed car is reported stolen by the owner. A random number is chosen between the minimum and maximum values. Assumes the owner eventually comes back and figures out they are missing a car. Alarmed owners will be more likely to find it faster.")]
    public uint AlarmedCarTimeToReportStolenMax { get; set; }
    [Description("Minimum time that must elapse before a non-alarmed car is reported stolen by the owner. A random number is chosen between the minimum and maximum values. Assumes the owner eventually comes back and figures out they are missing a car.")]
    public uint NonAlarmedCarTimeToReportStolenMin { get; set; }
    [Description("Maximum time that must elapse before a non-alarmed car is reported stolen by the owner. A random number is chosen between the minimum and maximum values. Assumes the owner eventually comes back and figures out they are missing a car.")]
    public uint NonAlarmedCarTimeToReportStolenMax { get; set; }
    [Description("Keep the entry for AutoTuneRadioStation tuned no matter what when you are in the car. Only recommened for recording/streaming.")]
    public bool KeepRadioAutoTuned { get; set; }
    [Description("Set the AutoTuneRadioStation tuned on entry the first time you get in a car. Useful for setting a favorite station better than vanilla.")]
    public bool AutoTuneRadioOnEntry { get; set; }
    [Description("Station the previous two options tune to. NONE also works for no tuning.")]
    public string AutoTuneRadioStation { get; set; }
    [Description("Disable the character automatically turning over the engine on entry. Used with the ignition hot key (Shift + Z Default)")]
    public bool DisableAutoEngineStart { get; set; }
    [Description("Set a random amount of fuel on each vehicle and calculates consumption rates on use.")]
    public bool UseCustomFuelSystem { get; set; }
    [Description("Allow the mod to set the engine as enabled or disabled to work with the ignition and fuel systems")]
    public bool AllowSetEngineState { get; set; }
    [Description("Add additional damage to the engine on collision.")]
    public bool ScaleEngineDamage { get; set; }
    [Description("Multiplier for the aditional damage. Ex. a 30 damage collision would be a 90 damamge collision at a ScaleEngineDamageMultiplier of 3.0")]
    public float ScaleEngineDamageMultiplier { get; set; }
    [Description("Allow the mod to set the indicator state.")]
    public bool AllowSetIndicatorState { get; set; }
    [Description("Force first person mode when you select vehicle duck and are in a vehicle. Used to simulate the lack of vision you would have in real life combined with the fact that bullets cannot go through doors in vanilla GTA.")]
    public bool ForceFirstPersonOnVehicleDuck { get; set; }
    [Description("Set mobile radio when you are in a police vehicle to simulate having a regular radio in police vehicles. Does not work in military or swat vehicles.")]
    public bool AllowRadioInPoliceVehicles { get; set; }
    [Description("Apply a slight HP decrease when you use your arm to break a car window.")]
    public bool InjureOnWindowBreak { get; set; }
    [Description("Require the screwdriver item to be in your inventory before you can pick the locks of any vehicles.")]
    public bool RequireScrewdriverForLockPickEntry { get; set; }
    [Description("Require the screwdriver item to be in your inventory before you can hotwire any vehicles.")]
    public bool RequireScrewdriverForHotwire { get; set; }
    [Description("Allows mission (persistent) vehicles to be locked by the game automatically. If disabled, only ambient vehicles will be locked by the mod.")]
    public bool AllowLockMissionVehicles { get;  set; }
    [Description("Stops the player ped from automatically putting on a helmet when on a motorcycle.")]
    public bool DisableAutoHelmet { get; set; }
    [Description("Stops the auto rollover assistance feature")]
    public bool DisableRolloverFlip { get; set; }

    public VehicleSettings()
    {
        SetDefault();
#if DEBUG
        AutoTuneRadioStation = "RADIO_19_USER";
        AutoTuneRadioOnEntry = true;
        UseCustomFuelSystem = true;
#endif
    }
    public void SetDefault()
    {
        AlarmedCarTimeToReportStolenMin = 60000;
        AlarmedCarTimeToReportStolenMax = 140000;
        NonAlarmedCarTimeToReportStolenMin = 500000;
        NonAlarmedCarTimeToReportStolenMax= 700000;
        KeepRadioAutoTuned = false;
        AutoTuneRadioOnEntry = false;
        AutoTuneRadioStation = "NONE";
        DisableAutoEngineStart = true;
        UseCustomFuelSystem = false;
        AllowSetEngineState = true;
        ScaleEngineDamage = true;
        ScaleEngineDamageMultiplier = 3.0f;
        AllowSetIndicatorState = true;
        ForceFirstPersonOnVehicleDuck = true;
        AllowRadioInPoliceVehicles = true;
        InjureOnWindowBreak = true;
        RequireScrewdriverForLockPickEntry = false;
        RequireScrewdriverForHotwire = false;
        AllowLockMissionVehicles = false;
        DisableAutoHelmet = true;
        DisableRolloverFlip = true;
    }
}