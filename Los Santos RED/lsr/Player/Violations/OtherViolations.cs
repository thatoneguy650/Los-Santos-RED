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
        ViolentUpdate();
        TrespassingUpdate();
        SuspiciousUpdate();
        ResistingArrestUpdate();
        DealingUpdate();
        NonViolentUpdate();
    }

    private void NonViolentUpdate()
    {
        if (Player.Intoxication.IsIntoxicated && Player.Intoxication.CurrentIntensity >= 2.0f && !Player.IsInVehicle)
        {
            Violations.AddViolating(StaticStrings.PublicIntoxicationCrimeID);
        }
        if (Player.RecentlyFedUpCop)
        {
            Violations.AddViolating(StaticStrings.InsultingOfficerCrimeID);
        }
        if (Player.IsBeingANuisance)
        {
            Violations.AddViolating(StaticStrings.PublicNuisanceCrimeID);
        }
        if (Player.IsSleeping && Player.IsSleepingOutside && !Player.CurrentLocation.IsInside && Player.CurrentLocation.CurrentZone?.Type != eLocationType.Wilderness && (Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Rich || Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Middle))
        {
            Violations.AddViolating(StaticStrings.PublicVagrancyCrimeID);
        }
        if (!Violations.CanBodyInteract && Player.ActivityManager.IsLootingBody)
        {
            Violations.AddViolating(StaticStrings.MuggingCrimeID);
        }
    }
    private void DealingUpdate()
    {
        if (Player.IsDealingDrugs)
        {
            Violations.AddViolating(StaticStrings.DealingDrugsCrimeID);
        }
        if (Player.IsDealingIllegalGuns)
        {
            Violations.AddViolating(StaticStrings.DealingGunsCrimeID);
        }
    }
    private void ResistingArrestUpdate()
    {
        if (Player.IsNotWanted || Player.Surrendering.HandsAreUp)
        {
            return;
        }
        if (Player.RecentlyResistedArrest)
        {
            Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
            return;
        }
        if (Player.IsInVehicle)
        {
            ResistingArrestVehicleUpdate();
        }
        else
        {
            ResistingArrestOnFootUpdate();
        }    
    }
    private void ResistingArrestVehicleUpdate()
    {
        bool isResistingArrest;
        if (Player.VehicleSpeedMPH >= 65f)
        {
            isResistingArrest = Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime;//kept going or took off
        }
        else if (Player.HasBeenMovingFast)
        {
            isResistingArrest = Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestMediumTriggerTime;
        }
        else
        {
            isResistingArrest = Player.HasBeenMoving && Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime;
        }
        if(isResistingArrest)
        {
            Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
        }
    }
    private void ResistingArrestOnFootUpdate()
    {
        bool isResistingArrest;
        if(!Player.Character.Exists())
        {
            return;
        }
        if (Player.HasBeenMovingFast)
        {
            isResistingArrest = Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime;//kept going or took off
        }
        else
        {
            isResistingArrest = Player.HasBeenMoving && Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime;
        }    
        if (isResistingArrest)
        {
            Violations.AddViolating(StaticStrings.ResistingArrestCrimeID);
        }
    }
    private void SuspiciousUpdate()
    {
        if (Player.Investigation.IsSuspicious || Player.IsDoingSuspiciousActivity)//when near investigation blip with description
        {
            Violations.AddViolating(StaticStrings.SuspiciousActivityCrimeID);
        }
    }
    private void ViolentUpdate()
    {
        if (Player.ActivityManager.IsCommitingSuicide)
        {
            Violations.AddViolating(StaticStrings.AttemptingSuicideCrimeID);
        }
        if (Player.ActivityManager.IsHoldingHostage)
        {
            Violations.AddViolating(StaticStrings.KidnappingCrimeID);
        }
    }

    private void TrespassingUpdate()
    {
        if (!Violations.CanEnterRestrictedAreas && Player.IsWanted && Player.CurrentLocation != null && Player.CurrentLocation.CurrentZone != null && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted && Player.CurrentLocation.GameTimeInZone >= 15000 && (Player.WantedLevel >= 3 || Player.PoliceResponse.IsDeadlyChase))
        {
            Violations.AddViolating(StaticStrings.TrespessingOnGovtPropertyCrimeID);
        }
        if(Player.RestrictedAreaManager.IsTrespassing && !Violations.CanEnterRestrictedAreas)
        {
            Violations.AddViolating(StaticStrings.TrespessingCrimeID);
        }
    }
    public bool AddFoundIllegalItem()
    {
        Violations.AddViolatingAndObserved(StaticStrings.DealingDrugsCrimeID);
        return true;
    }
}

