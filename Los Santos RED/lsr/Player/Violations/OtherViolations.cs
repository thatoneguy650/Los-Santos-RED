using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


public class OtherViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private IInteractionable Interactionable;
    public OtherViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world, IInteractionable interactionable)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
        World = world;
        Interactionable = interactionable;
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
    //public void AddFoundBody()
    //{
    //    Violations.AddViolating(StaticStrings.SuspiciousVehicleCrimeID);
    //}
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
        if (!Player.ActivityManager.IsUrinatingDefectingOnToilet && Player.ActivityManager.IsUrinatingDefecting && (Player.CurrentLocation.IsInside || Player.CurrentLocation.CurrentZone?.Type != eLocationType.Wilderness))// && (Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Rich || Player.CurrentLocation.CurrentZone?.Economy == eLocationEconomy.Middle))
        {
            Violations.AddViolating(StaticStrings.IndecentExposureCrimeID);
        }
        if (!Violations.CanBodyInteract && Player.ActivityManager.IsLootingBody)
        {
            Violations.AddViolating(StaticStrings.MuggingCrimeID);
        }
        if (Player.ActivityManager.IsUsingIllegalItem)
        {
            Violations.AddViolating(StaticStrings.DrugPossessionCrimeID);
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
        if(Player.IsInVehicle && Player.CurrentVehicle != null && Player.IsDriver)
        {
            SuspiciousBodyUpdate();
        }
    }
    private void SuspiciousBodyUpdate()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.IsDriver)
        {
            return;
        }
        if(Player.CurrentVehicle.VehicleBodyManager.CheckSuspicious())
        {
            Violations.AddViolating(StaticStrings.SuspiciousVehicleCrimeID);
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
        if (Player.ActivityManager.IsBuryingBody)
        {
            Violations.AddViolating(StaticStrings.BuryingABody);
        }
        CheckVehicleInvasion();
    }
    private void CheckVehicleInvasion()
    {
        if(!Player.IsInVehicle || Player.CurrentVehicle == null || Player.IsDriver)
        {
            return;
        }
        if(Player.IsCop)
        {
            return;
        }
        if(Player.CurrentVehicle.IsTaxi)
        {
            return;
        }
        if (Player.TimeInCurrentVehicle >= 5000)
        {
            return;
        }
        if (Player.VehicleOwnership.OwnedVehicles.Any(x=> x.Handle == Player.CurrentVehicle.Handle))
        {
            return;
        }
        if(Player.CurrentVehicle.Vehicle.Exists() && Player.LastFriendlyVehicle.Exists() && Player.CurrentVehicle.Vehicle.Handle == Player.LastFriendlyVehicle.Handle)
        {
            return;
        }
        if((Player.IsEMT || Player.IsFireFighter || Player.IsSecurityGuard) && Player.CurrentVehicle.IsService)
        {
            return;
        }
        if(Player.RelationshipManager.GangRelationships.CurrentGang != null && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == Player.RelationshipManager.GangRelationships.CurrentGang.ID)
        {
            return;
        }
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if(!Player.CurrentVehicle.Vehicle.Occupants.Any(x=> x.Exists() && x.Handle != Player.Character.Handle))
        {
            return;
        }
        Violations.AddViolating(StaticStrings.VehicleInvasionCrimeID);
    }
    private void TrespassingUpdate()
    {
        if (!Violations.CanEnterRestrictedAreas && Player.IsWanted && Player.CurrentLocation != null && Player.CurrentLocation.CurrentZone != null && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted && Player.CurrentLocation.GameTimeInZone >= 15000 && (Player.WantedLevel >= 2 || Player.PoliceResponse.IsDeadlyChase))
        {
            Violations.AddViolating(StaticStrings.TrespessingOnGovtPropertyCrimeID);
        }
        if(Player.RestrictedAreaManager.IsTrespassing && !Violations.CanEnterRestrictedAreas)
        {
            if (Player.RestrictedAreaManager.IsCivilianReactableTrespassing)
            {
                Violations.AddViolating(StaticStrings.CivilianTrespessingCrimeID);
            }
            else
            {
                Violations.AddViolating(StaticStrings.TrespessingCrimeID);
            }
        }
        if (Player.RestrictedAreaManager.IsTrespassingOnMilitaryBase && !Violations.CanEnterRestrictedAreas)
        {
            Violations.AddViolating(StaticStrings.TrespassingOnMilitaryBaseCrimeID);
        }
    }
    public bool AddFoundIllegalItem()
    {
        Violations.AddViolatingAndObserved(StaticStrings.DrugPossessionCrimeID);
        return true;
    }
}

