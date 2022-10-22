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
            Violations.AddViolating("AttemptingSuicide");
        }
        if (Player.IsWanted && Player.CurrentLocation != null && Player.CurrentLocation.CurrentZone != null && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted && Player.CurrentLocation.GameTimeInZone >= 15000 && (Player.WantedLevel >= 3 || Player.PoliceResponse.IsDeadlyChase))
        {
            Violations.AddViolating("TrespessingOnGovtProperty");
        }
        if (Player.Investigation.IsSuspicious)
        {
            Violations.AddViolating("SuspiciousActivity");
        }
        if (Player.IsDoingSuspiciousActivity)
        {
            Violations.AddViolating("SuspiciousActivity");
        }
        if (Player.IsWanted && !Player.Surrendering.HandsAreUp)
        {
            if (Player.IsInVehicle)
            {
                if (Player.VehicleSpeedMPH >= 65f)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime)//kept going or took off
                    {
                        Violations.AddViolating("ResistingArrest");
                    }
                }
                else if (Player.HasBeenMovingFast)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestMediumTriggerTime)
                    {
                        Violations.AddViolating("ResistingArrest");
                    }
                }
                else
                {
                    if (Player.HasBeenMoving && Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime)
                    {
                        Violations.AddViolating("ResistingArrest");
                    }
                }
            }
            else
            {
                if (Player.Character.Exists() && Player.HasBeenMovingFast)
                {
                    if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime)//kept going or took off
                    {
                        Violations.AddViolating("ResistingArrest");
                    }
                }
                else
                {
                    if (Player.Character.Exists() && Player.HasBeenMoving)
                    {
                        if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime)
                        {
                            Violations.AddViolating("ResistingArrest");
                        }
                    }
                }
            }
        }
        if (Player.RecentlyResistedArrest)
        {
            Violations.AddViolating("ResistingArrest");
        }
        if (Player.Intoxication.IsIntoxicated && Player.Intoxication.CurrentIntensity >= 2.0f && !Player.IsInVehicle)
        {
            Violations.AddViolating("PublicIntoxication");
        }
        if (Player.RecentlyFedUpCop)
        {
            Violations.AddViolating("InsultingOfficer");
        }
        if (Player.IsDealingDrugs)
        {
            Violations.AddViolating("DealingDrugs");
        }
        if (Player.IsDealingIllegalGuns)
        {
            Violations.AddViolating("DealingGuns");
        }
        if (Player.ActivityManager.IsHoldingHostage)
        {
            Violations.AddViolating("Kidnapping");
        }
        if (Player.IsBeingANuisance)
        {
            Violations.AddViolating("PublicNuisance");
        }
        if (Player.IsSleeping && !Player.CurrentLocation.IsInside && Player.CurrentLocation.CurrentZone?.Type != eLocationType.Wilderness && (Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Rich || Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Middle))
        {
            Violations.AddViolating("PublicVagrancy");
        }
        if (Player.ActivityManager.IsLootingBody)
        {
            Violations.AddViolating("Mugging");
        }
    }
}

