using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangGeneralLocate : GeneralLocate
{

    public GangGeneralLocate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, IWeaponIssuable weaponIssuable) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, weaponIssuable, false)
    {

    }

    protected override bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot;
    protected override void UpdateVehicleState()
    {
        if (!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
        {
            return;
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
        if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            PlaceToDriveTo = OtherTarget.Pedestrian.Position;// Player.StreetPlacePoliceShouldSearchForPlayer;
            PlaceToWalkTo = OtherTarget.Pedestrian.Position; //Player.PlacePoliceShouldSearchForPlayer;
        }
        else
        {
            PlaceToDriveTo = Player.Character.Position;
            PlaceToWalkTo = Player.Character.Position;
        }
    }
}
