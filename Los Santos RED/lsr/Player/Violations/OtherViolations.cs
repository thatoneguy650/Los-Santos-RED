using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OtherViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;

    public OtherViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Reset()
    {

    }
    public void Update()
    {
        if (Player.ActivityManager.IsCommitingSuicide)
        {
            Violations.AddViolating(StaticStrings.AttemptingSuicideCrimeID);
        }
        if (Player.IsWanted && Player.CurrentLocation != null && Player.CurrentLocation.CurrentZone != null && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted && Player.CurrentLocation.GameTimeInZone >= 15000 && (Player.WantedLevel >= 3 || Player.PoliceResponse.IsDeadlyChase))
        {
            Violations.AddViolating(StaticStrings.TrespessingOnGovtPropertyCrimeID);
        }
        if (Player.Investigation.IsSuspicious)
        {
            Violations.AddViolating(StaticStrings.SuspiciousActivityCrimeID);
        }
        if (Player.IsDoingSuspiciousActivity)
        {
            Violations.AddViolating(StaticStrings.SuspiciousActivityCrimeID);
        }
        if (Player.IsWanted && !Player.Surrendering.HandsAreUp)
        {
            if (Player.IsInVehicle)
            {
                if (Player.VehicleSpeedMPH >= 65f)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime)//kept going or took off
                    {
                        Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
                    }
                }
                else if (Player.HasBeenMovingFast)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestMediumTriggerTime)
                    {
                        Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
                    }
                }
                else
                {
                    if (Player.HasBeenMoving && Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime)
                    {
                        Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
                    }
                }
            }
            else
            {
                if (Player.Character.Exists() && Player.HasBeenMovingFast)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime)//kept going or took off
                    {
                        Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
                    }
                }
                else
                {
                    if (Player.Character.Exists() && Player.HasBeenMoving)
                    {
                        if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime)
                        {
                            Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
                        }
                    }
                }
            }
        }
        if (Player.RecentlyResistedArrest)
        {
            Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
        }
        if (Player.Intoxication.IsIntoxicated && Player.Intoxication.CurrentIntensity >= 2.0f && !Player.IsInVehicle)
        {
            Violations.AddViolating(StaticStrings.PublicIntoxicationCrimeID);
        }
        if (Player.RecentlyFedUpCop)
        {
            Violations.AddViolating(StaticStrings.InsultingOfficerCrimeID);
        }
        if (Player.IsDealingDrugs)
        {
            Violations.AddViolating(StaticStrings.DealingDrugsCrimeID);
        }
        if (Player.IsDealingIllegalGuns)
        {
            Violations.AddViolating(StaticStrings.DealingGunsCrimeID);
        }
        if (Player.ActivityManager.IsHoldingHostage)
        {
            Violations.AddViolating(StaticStrings.KidnappingCrimeID);
        }
        if (Player.IsBeingANuisance)
        {
            Violations.AddViolating(StaticStrings.PublicNuisanceCrimeID);
        }
        if (Player.IsSleeping && Player.IsSleepingOutside && !Player.CurrentLocation.IsInside && Player.CurrentLocation.CurrentZone?.Type != eLocationType.Wilderness && (Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Rich || Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Middle))
        {
            Violations.AddViolating(StaticStrings.PublicVagrancyCrimeID);
        }
        if (Player.ActivityManager.IsLootingBody)
        {
            Violations.AddViolating(StaticStrings.MuggingCrimeID);
        }
    }
}

