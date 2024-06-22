using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PoliceGeneralInvestigate : GeneralInvestigate
{
    public PoliceGeneralInvestigate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, 
        IWeaponIssuable weaponIssuable, bool shouldSearchArea) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, weaponIssuable, shouldSearchArea)
    {

    }
    private bool IsRespondingCode3 => (Player.Investigation.IsActive && Player.Investigation.InvestigationWantedLevel > 1) || Ped.PedAlerts.IsAlerted || World.CitizenWantedLevel > 1;
    protected override bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot;
    protected override bool ForceSetArmed => IsRespondingCode3;
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
        NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
        NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
        if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
        {
            NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
        }
    }
    protected override void GetLocations()
    {
        if (Ped.PedAlerts.IsAlerted)
        {
            PlaceToDriveTo = Ped.PedAlerts.AlertedPoint;
            PlaceToWalkTo = Ped.PedAlerts.AlertedPoint;
        }
        else if (Player.Investigation.IsActive)
        {
            if(Player.Investigation.StreetPosition != Vector3.Zero)
            {
                PlaceToDriveTo = Player.Investigation.StreetPosition;
            }
            else
            {
                PlaceToDriveTo = Player.Investigation.Position;
            }

            if(Player.Investigation.Interior != null && Player.Investigation.Interior.SearchLocations != null && Player.Investigation.Interior.SearchLocations.Any())
            {
                PlaceToWalkTo = Player.Investigation.Interior.SearchLocations.FirstOrDefault();
                SearchPoints = Player.Investigation.Interior.SearchLocations.ToList();
            }
            else
            {
                PlaceToWalkTo = Player.Investigation.Position;
            }

        }
        else
        {
            PlaceToDriveTo = World.PoliceBackupPoint;
            PlaceToWalkTo = World.PoliceBackupPoint;
        }
    }
    public override void OnLocationReached()
    {
        Player.Investigation.OnPoliceArrived();
        Ped.GameTimeReachedInvestigationPosition = Game.GameTime;
        HasReachedLocatePosition = true;
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} Police Located HasReachedLocatePosition");
    }
    public override void OnFinalSearchLocationReached()
    {
        Player.Investigation.OnPoliceInvestigatedArea();
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} POLICE HAS SEARCHED LOCATION ON FOOT");
    }
}

