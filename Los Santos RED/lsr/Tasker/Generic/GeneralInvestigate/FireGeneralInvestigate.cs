using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FireGeneralInvestigate : GeneralInvestigate
{
    public FireGeneralInvestigate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable, bool shouldSearchArea) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, weaponIssuable, shouldSearchArea)
    {

    }
    private bool IsRespondingCode3 => Player.Investigation.IsActive;
    protected override bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter;
    protected override bool ForceSetArmed => false;
    protected override void UpdateVehicleState()
    {
        if (!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
        {
            return;
        }
        if (Settings.SettingsManager.WorldSettings.AllowSettingSirenState && Ped.IsDriver && Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren)
        {
            if (IsRespondingCode3)
            {
                if (!Ped.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
                    Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
            }
            else
            {
                if (Ped.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    Ped.Pedestrian.CurrentVehicle.IsSirenSilent = true;
                }
            }
        }
    }
    protected override void GetLocations()
    {
        if (Player.Investigation.IsActive)
        {
            if (Player.Investigation.StreetPosition != Vector3.Zero)
            {
                PlaceToDriveTo = Player.Investigation.StreetPosition;
            }
            else
            {
                PlaceToDriveTo = Player.Investigation.Position;
            }
            PlaceToWalkTo = Player.Investigation.Position;
        }
        else if (Ped.PedAlerts.IsAlerted)
        {
            PlaceToDriveTo = Ped.PedAlerts.AlertedPoint;
            PlaceToWalkTo = Ped.PedAlerts.AlertedPoint;
        }
    }
    public override void OnLocationReached()
    {
        Player.Investigation.OnFireFightersArrived();
        Ped.GameTimeReachedInvestigationPosition = Game.GameTime;
        HasReachedLocatePosition = true;
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} FIRE Located HasReachedLocatePosition");
    }
}

