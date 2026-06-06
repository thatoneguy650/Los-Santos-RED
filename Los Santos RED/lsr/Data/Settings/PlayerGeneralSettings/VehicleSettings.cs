using LosSantosRED.lsr;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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
    [Description("Minimum fuel in cars. 0 is minimum")]
    public float CustomFuelSystemFuelMin { get; set; }
    [Description("Maximum fuel in cars. 65 is maximum.")]
    public float CustomFuelSystemFuelMax { get; set; }
    [Description("Scalar amount for the custom fuel system. Default is 1.0. A Value of 2.0 would consume fuel twice as fast as default. A Value of 0.5 would consume fuel half as fast as default.")]
    public float CustomFuelSystemFuelConsumptionScalar { get; set; }
    [Description("Allow the mod to set the engine as enabled or disabled to work with the ignition and fuel systems")]
    public bool AllowSetEngineState { get; set; }
    [Description("Allow the mod to set the engine as enabled or disabled to work with the ignition and fuel systems, but only for cars (requires AllowSetEngineState)")]
    public bool AllowSetEngineStateOnlyCars { get; set; }
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
    [Description("Allows vehicles to be locked by the game automatically.")]
    public bool AllowLockVehicles { get; set; }
    [Description("Percentage of available vehicles that will be locked by LSR. Maximum 100")]
    public float LockVehiclePercentage { get; set; }

    [Description("Allows mission (persistent) vehicles to be locked by the game automatically. If disabled, only ambient vehicles will be locked by the mod.")]
    public bool AllowLockMissionVehicles { get;  set; }
    [Description("Stops the player ped from automatically putting on a helmet when on a motorcycle.")]
    public bool DisableAutoHelmet { get; set; }
    [Description("Stops the auto rollover assistance feature")]
    public bool DisableRolloverFlip { get; set; }
    [Description("If enabled, the player light rendering (dynamic shadows, more emissive) will be applied to ALL vehicles in the world. Light won't do the weird transition when getting out of a vehicle. Only applies within about 75 meters.")]
    public bool UseBetterLightStateOnAI { get; set; }
    [Description("If enabled, the player will not be able to fly aircraft until they have a pilots license.")]
    public bool DisableAircraftWithoutLicense { get; set; }
    [Description("Limit of vehicle health before the vehicle is considered damaged/non-roadworthy.")]
    public int NonRoadworthyVehicleHealthLimit { get; set; }
    [Description("Limit of vehicle engine health before the vehicle is considered damaged/non-roadworthy.")]
    public int NonRoadworthyEngineHealthLimit { get; set; }
    [Description("If enabled, any broken windows will mark the vehicle as damaged/non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckDamagedWindows { get; set; }
    [Description("If enabled, any damaged doors will mark the vehicle as damaged/non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckDamagedDoors { get; set; }
    [Description("If enabled, any broken headlights will mark the vehicle as damaged/non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckDamagedHeadlights { get; set; }
    [Description("If enabled, any burst tires will mark the vehicle as damaged/non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckDamagedTires { get; set; }
    [Description("If enabled, not having lights on at night will mark the vehicle as non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckNoHeadlights { get; set; }
    [Description("If enabled, not having a license plate will mark the vehicle as non-roadworthy.")]
    public bool NonRoadworthyVehicleCheckNoPlate { get; set; }
    [Description("If enabled, vehicles will be refuelled when repairing at Pay n Spray.")]
    public bool RefuelVehicleAfterPayNSprayRepair { get; set; }
    public bool AttachOwnedVehicleBlips { get; set; }


    [Description("If enabled, the player will perform an animation when manipulating vehicle controls (engine off, opening/closing door, rolling windows up/down, etc.")]
    public bool PlayControlAnimations { get; set; }
    public bool AutoHotwire { get; set; }
    public bool InjureOnVehicleCrash { get; set; }
    public float VehicleCrashInjureScalar { get; set; }
    public int VehicleCrashInjureMinVehicleDamageTrigger { get; set; }
    public float VehicleCrashInjureRandomizePercentage { get; set; }
    public bool FuelUsesAnimationsAndProps { get; set; }
    public int PlayerEnteredPersistantVehicleLimit { get; set; }
    public float PlayerEnteredPersistantVehicleDistanceLimit { get; set; }





    public int LockpickRegularTotalPinsMin { get; set; }
    public int LockpickRegularTotalPinsMax { get; set; }
    public int LockpickRegularTotalPinStepsMin { get; set; }
    public int LockpickRegularTotalPinStepsMax { get; set; }
    public float LockpickRegularZoneWidthMin { get; set; }
    public float LockpickRegularZoneWidthMax { get; set; }
    public float LockpickRegularFillSpeedMin { get; set; }
    public float LockpickRegularFillSpeedMax { get; set; }


    public int LockpickHighEndTotalPinsMin { get; set; }
    public int LockpickHighEndTotalPinsMax { get; set; }
    public int LockpickHighEndTotalPinStepsMin { get; set; }
    public int LockpickHighEndTotalPinStepsMax { get; set; }
    public float LockpickHighEndZoneWidthMin { get; set; }
    public float LockpickHighEndZoneWidthMax { get; set; }
    public float LockpickHighEndFillSpeedMin { get; set; }
    public float LockpickHighEndFillSpeedMax { get; set; }

    public int LockpickPoliceTotalPinsMin { get; set; }
    public int LockpickPoliceTotalPinsMax { get; set; }
    public int LockpickPoliceTotalPinStepsMin { get; set; }
    public int LockpickPoliceTotalPinStepsMax { get; set; }
    public float LockpickPoliceZoneWidthMin { get; set; }
    public float LockpickPoliceZoneWidthMax { get; set; }
    public float LockpickPoliceFillSpeedMin { get; set; }
    public float LockpickPoliceFillSpeedMax { get; set; }


    public int HotwireRegularTotalPinsMin { get; set; }
    public int HotwireRegularTotalPinsMax { get; set; }
    public int HotwireRegularTotalPinStepsMin { get; set; }
    public int HotwireRegularTotalPinStepsMax { get; set; }
    public float HotwireRegularZoneWidthMin { get; set; }
    public float HotwireRegularZoneWidthMax { get; set; }
    public float HotwireRegularFillSpeedMin { get; set; }
    public float HotwireRegularFillSpeedMax { get; set; }


    public int HotwireHighEndTotalPinsMin { get; set; }
    public int HotwireHighEndTotalPinsMax { get; set; }
    public int HotwireHighEndTotalPinStepsMin { get; set; }
    public int HotwireHighEndTotalPinStepsMax { get; set; }
    public float HotwireHighEndZoneWidthMin { get; set; }
    public float HotwireHighEndZoneWidthMax { get; set; }
    public float HotwireHighEndFillSpeedMin { get; set; }
    public float HotwireHighEndFillSpeedMax { get; set; }

    public int HotwirePoliceTotalPinsMin { get; set; }
    public int HotwirePoliceTotalPinsMax { get; set; }
    public int HotwirePoliceTotalPinStepsMin { get; set; }
    public int HotwirePoliceTotalPinStepsMax { get; set; }
    public float HotwirePoliceZoneWidthMin { get; set; }
    public float HotwirePoliceZoneWidthMax { get; set; }
    public float HotwirePoliceFillSpeedMin { get; set; }
    public float HotwirePoliceFillSpeedMax { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public VehicleSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        AlarmedCarTimeToReportStolenMin = 30000;// 40000;// 60000;
        AlarmedCarTimeToReportStolenMax = 50000; // 100000;// 140000;
        NonAlarmedCarTimeToReportStolenMin = 100000;// 200000;// 500000;
        NonAlarmedCarTimeToReportStolenMax = 200000;// 400000;// 700000;
        KeepRadioAutoTuned = false;
        AutoTuneRadioOnEntry = false;
        AutoTuneRadioStation = "NONE";
        DisableAutoEngineStart = true;
        UseCustomFuelSystem = true;
        CustomFuelSystemFuelMin = 10f;
        CustomFuelSystemFuelMax = 65f;
        CustomFuelSystemFuelConsumptionScalar = 3.0f;
        AllowSetEngineState = true;
        AllowSetEngineStateOnlyCars = false;

        ScaleEngineDamage = true;



        ScaleEngineDamageMultiplier = 3.0f;
        AllowSetIndicatorState = true;
        ForceFirstPersonOnVehicleDuck = true;
        AllowRadioInPoliceVehicles = true;
        InjureOnWindowBreak = true;
        RequireScrewdriverForLockPickEntry = false;
        RequireScrewdriverForHotwire = false;


        AllowLockVehicles = true;
        LockVehiclePercentage = 90f;

        AllowLockMissionVehicles = false;
        DisableAutoHelmet = true;
        DisableRolloverFlip = true;
        UseBetterLightStateOnAI = false;
        DisableAircraftWithoutLicense = false;

        NonRoadworthyVehicleHealthLimit = 300;
        NonRoadworthyEngineHealthLimit = 300;
        NonRoadworthyVehicleCheckDamagedWindows = true;
        NonRoadworthyVehicleCheckDamagedDoors = true;
        NonRoadworthyVehicleCheckDamagedHeadlights = true;
        NonRoadworthyVehicleCheckDamagedTires = true;
        NonRoadworthyVehicleCheckNoHeadlights = true;
        NonRoadworthyVehicleCheckNoPlate = true;
        RefuelVehicleAfterPayNSprayRepair = true;
        AttachOwnedVehicleBlips = true;
        PlayControlAnimations = true;
        AutoHotwire = false;

        InjureOnVehicleCrash = true;

        VehicleCrashInjureScalar = 0.1f;// 0.35f;

        VehicleCrashInjureRandomizePercentage = 0.3f;

        VehicleCrashInjureMinVehicleDamageTrigger = 55;

        FuelUsesAnimationsAndProps = true;

        PlayerEnteredPersistantVehicleLimit = 3;
        PlayerEnteredPersistantVehicleDistanceLimit = 500f;




        LockpickRegularTotalPinsMin = 1;
        LockpickRegularTotalPinsMax = 4;
        LockpickRegularTotalPinStepsMin = 2;
        LockpickRegularTotalPinStepsMax = 4;
        LockpickRegularZoneWidthMin = 10f;
        LockpickRegularZoneWidthMax = 30f;
        LockpickRegularFillSpeedMin = 0.75f;
        LockpickRegularFillSpeedMax = 1.25f;


        LockpickHighEndTotalPinsMin = 3;
        LockpickHighEndTotalPinsMax = 6;
        LockpickHighEndTotalPinStepsMin = 3;
        LockpickHighEndTotalPinStepsMax = 5;
        LockpickHighEndZoneWidthMin = 5f;
        LockpickHighEndZoneWidthMax = 15f;
        LockpickHighEndFillSpeedMin = 0.5f;
        LockpickHighEndFillSpeedMax = 0.75f;
       

        LockpickPoliceTotalPinsMin = 4;
        LockpickPoliceTotalPinsMax = 6;
        LockpickPoliceTotalPinStepsMin = 4;
        LockpickPoliceTotalPinStepsMax = 6;
        LockpickPoliceZoneWidthMin = 10f;
        LockpickPoliceZoneWidthMax = 30f;
        LockpickPoliceFillSpeedMin = 0.75f;
        LockpickPoliceFillSpeedMax = 1.25f;




        HotwireRegularTotalPinsMin = 1;
        HotwireRegularTotalPinsMax = 2;
        HotwireRegularTotalPinStepsMin = 2;
        HotwireRegularTotalPinStepsMax = 3;
        HotwireRegularZoneWidthMin = 20f;
        HotwireRegularZoneWidthMax = 30f;
        HotwireRegularFillSpeedMin = 0.75f;
        HotwireRegularFillSpeedMax = 1.25f;


        HotwireHighEndTotalPinsMin = 3;
        HotwireHighEndTotalPinsMax = 4;
        HotwireHighEndTotalPinStepsMin = 2;
        HotwireHighEndTotalPinStepsMax = 4;
        HotwireHighEndZoneWidthMin = 10f;
        HotwireHighEndZoneWidthMax = 15f;
        HotwireHighEndFillSpeedMin = 0.75f;
        HotwireHighEndFillSpeedMax = 1.00f;


        HotwirePoliceTotalPinsMin = 3;
        HotwirePoliceTotalPinsMax = 5;
        HotwirePoliceTotalPinStepsMin = 3;
        HotwirePoliceTotalPinStepsMax = 5;
        HotwirePoliceZoneWidthMin = 10f;
        HotwirePoliceZoneWidthMax = 30f;
        HotwirePoliceFillSpeedMin = 0.75f;
        HotwirePoliceFillSpeedMax = 1.25f;


    }
}