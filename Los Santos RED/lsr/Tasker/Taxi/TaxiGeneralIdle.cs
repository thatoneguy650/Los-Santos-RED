using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiGeneralIdle : GeneralIdle
{

    public TaxiGeneralIdle(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool checkPassengers, bool checkSiren, bool forceStandardScenarios) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, checkPassengers, checkSiren, forceStandardScenarios)
    {

    }
    protected override void SetVehicleState()
    {
        if (Ped.IsDriver && Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            bool isLightOn = NativeFunction.Natives.IS_TAXI_LIGHT_ON<bool>(Ped.Pedestrian.CurrentVehicle);
            if (isLightOn && Ped.Pedestrian.CurrentVehicle.PassengerCount > 0)
            {
                //EntryPoint.WriteToConsole($"{Ped.Handle} TURNING TAXI LIGHT OFF");
                NativeFunction.Natives.SET_TAXI_LIGHTS(Ped.Pedestrian.CurrentVehicle, false);
            }
            else if(!isLightOn)
            {
                //EntryPoint.WriteToConsole($"{Ped.Handle} TURNING TAXI LIGHT ON");
                NativeFunction.Natives.SET_TAXI_LIGHTS(Ped.Pedestrian.CurrentVehicle, true);
            }
        }
    }
}

