using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PoliceGeneralLocate : GeneralLocate
{
    public override void Start()
    {
        base.Start();
    }
    public PoliceGeneralLocate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, IWeaponIssuable weaponIssuable, bool hasSixthSense) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, weaponIssuable, hasSixthSense)
    {

    }

    protected override bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot && Player.PoliceLastSeenOnFoot && Player.IsNearbyPlacePoliceShouldSearchForPlayer;
    protected override void UpdateVehicleState()
    {
        if (!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
        {
            return;
        }
        if (Settings.SettingsManager.WorldSettings.AllowSettingSirenState && Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
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
        PlaceToDriveTo = HasSixthSense ? Player.StreetPlacePoliceShouldSearchForPlayer : Player.StreetPlacePoliceLastSeenPlayer;
        PlaceToWalkTo = HasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer; 
    }

    public override void OnLocationReached()
    {
        if (!Ped.IsInVehicle)
        {
            HasReachedLocatePosition = true;
        }
        else
        {
            if (!(HasSixthSense && Player.SearchMode.IsInStartOfSearchMode))
            {
                HasReachedLocatePosition = true;
            }
        }
    }
}

