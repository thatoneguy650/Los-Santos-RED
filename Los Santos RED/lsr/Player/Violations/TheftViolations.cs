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

        if (Player.CurrentVehicle != null && Player.CurrentVehicle.IsImpounded && Player.AnyPoliceCanRecognizePlayer && Player.IsInVehicle && Player.IsDriver)
        {
            Violations.AddViolating(StaticStrings.DrivingStolenVehicleCrimeID);//.IsCurrentlyViolating = true;
        }

        if (Player.IsInVehicle && Player.IsDriver && Player.CurrentVehicle != null && Player.AnyPoliceCanRecognizePlayer && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.IsPoliceVehicle)
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

        CheckStolenVehicles();

    }

    private void CheckStolenVehicles()
    {
        foreach(VehicleExt stolenCar in Player.TrackedVehicles.Where(x => x.NeedsToBeReportedStolen))
        {
            stolenCar.WasReportedStolen = true;
            if(stolenCar.CarPlate != null && stolenCar.OriginalLicensePlate != null && stolenCar.CarPlate.PlateNumber == stolenCar.OriginalLicensePlate.PlateNumber)
            {
                stolenCar.CarPlate.IsWanted = true;
            }
            Game.DisplayHelp("Vehicle Reported Stolen");
        }
    }

    public void AddCarJacked(PedExt myPed)
    {
        if(myPed == null)
        {
            return;
        }
        myPed.OnCarjackedByPlayer(Player,Zones,GangTerritories);


        //if (myPed.IsGangMember)
        //{
        //    if (myPed.GetType() == typeof(GangMember))
        //    {
        //        GangMember gm = (GangMember)myPed;
        //        AddCarjackedGang(gm);
        //        //AddAttackedGang(gm, true);
        //    }
        //}
        //myPed.HasBeenCarJackedByPlayer = true;
    }
    //private void AddCarjackedGang(GangMember gm)
    //{
    //    if(gm == null)
    //    {
    //        return;
    //    }
    //    int RepToRemove = -2500;
    //    GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(gm.Gang);//.MembersKilled++;
    //    if (gr != null)
    //    {
    //        gr.MembersCarJacked++;
    //        //EntryPoint.WriteToConsole($"VIOLATIONS: Carjacking GangMemeber {gm.Gang.ShortName} {gr.MembersCarJacked}", 5);
    //        if (gm.Pedestrian.Exists())
    //        {
    //            Zone KillingZone = Zones.GetZone(gm.Pedestrian.Position);
    //            if (KillingZone != null)
    //            {
    //                List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(gm.Gang.ID);
    //                if (totalTerritories.Any(x => x.ZoneInternalGameName == KillingZone.InternalGameName))
    //                {
    //                    RepToRemove -= 2500;
    //                    gr.MembersCarJackedInTerritory++;
    //                    //EntryPoint.WriteToConsole($"VIOLATIONS: Carjacking GangMemeber {gm.Gang.ShortName} On Own Turf {gr.MembersCarJackedInTerritory}", 5);
    //                }
    //            }
    //        }
    //    }
    //    Player.RelationshipManager.GangRelationships.ChangeReputation(gm.Gang, RepToRemove, true);
    //    Player.RelationshipManager.GangRelationships.AddAttacked(gm.Gang);
    //}
}

