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


public class TheftViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IZones Zones;
    private IGangTerritories GangTerritories;
    public bool IsRobbingBank { get; set; }
    public TheftViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, IZones zones, IGangTerritories gangTerritories)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
        Zones = zones;
        GangTerritories = gangTerritories;
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
        if (Player.IsWanted && Player.IsInVehicle && Player.IsInAirVehicle)
        {
            Violations.AddViolating(StaticStrings.GotInAirVehicleDuringChaseCrimeID);//.IsCurrentlyViolating = true;
        }
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen && Player.IsInVehicle && Player.IsDriver)
        {
            Violations.AddViolating(StaticStrings.DrivingStolenVehicleCrimeID);//.IsCurrentlyViolating = true;
        }
        if (Player.IsInVehicle && Player.IsDriver && Player.CurrentVehicle != null && Player.AnyPoliceCanRecognizePlayer && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.IsPolice) // && Player.CurrentVehicle.Vehicle.IsPoliceVehicle)
        {
            Violations.AddViolating(StaticStrings.DrivingStolenVehicleCrimeID);//.IsCurrentlyViolating = true;
        }
        if (Player.ActivityManager.IsHoldingUp)
        {
            Violations.AddViolating(StaticStrings.MuggingCrimeID);//.IsCurrentlyViolating = true;
        }
        if (Player.IsBreakingIntoCar)
        {
            Violations.AddViolating(StaticStrings.GrandTheftAutoCrimeID);//.IsCurrentlyViolating = true;
        }
        if (Player.IsChangingLicensePlates)
        {
            Violations.AddViolating(StaticStrings.ChangingPlatesCrimeID);//.IsCurrentlyViolating = true;
        }
        if (IsRobbingBank)
        {
            Violations.AddViolating(StaticStrings.BankRobberyCrimeID);//.IsCurrentlyViolating = true;
        }

        CheckStolenVehicles();
    }
    private void CheckStolenVehicles()
    {
        foreach(VehicleExt stolenCar in Player.TrackedVehicles.Where(x => x.NeedsToBeReportedStolen))
        {
            stolenCar.SetReportedStolen();
        }
    }
    public void AddCarJacked(PedExt myPed)
    {
        if(myPed == null)
        {
            return;
        }
        myPed.OnCarjackedByPlayer(Player,Zones,GangTerritories);
    }

}

