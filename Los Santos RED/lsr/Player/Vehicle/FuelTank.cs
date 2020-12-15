using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class FuelTank
{
    private VehicleExt VehicleExt;
    private bool NearGasPumps;
    private uint GameTimeLastCheckedFuel;
    private float CurrentLevel;

    public FuelTank(VehicleExt vehicleToMonitor)
    {
        VehicleExt = vehicleToMonitor;
    }

    public bool CanPump { get; private set; }
    public string UIText
    {
        get
        {
           return string.Format(" Fuel {0}", (CurrentLevel / 100f).ToString("P2"));
        }
    }   
    public void Update()
    {
        CurrentLevel = VehicleExt.Vehicle.FuelLevel;
        if (VehicleExt != null && VehicleExt.Engine.IsRunning)
        {
            EngineRunningTick();
        }
        else
        {
            EngineOffTick();
        }     
    }
    public void PumpFuel()
    {
        if (VehicleExt.Vehicle.FuelLevel < 100f)
        {
            if (CurrentLevel + 1f <= 100f)
            {
                VehicleExt.Vehicle.FuelLevel = CurrentLevel + 1f;
            }
            else
            {
                VehicleExt.Vehicle.FuelLevel = 100f;
            }
        }
    }
    private void EngineRunningTick()
    {
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            if (CurrentLevel <= 0)
            {
                if (VehicleExt != null)
                {
                    VehicleExt.Engine.Toggle(false);
                }
            }
            else
            {
                float AmountToSubtract = 0.001f + VehicleExt.Vehicle.Speed * 0.0001f;
                VehicleExt.Vehicle.FuelLevel = CurrentLevel - AmountToSubtract;
            }
            GameTimeLastCheckedFuel = Game.GameTime;
        }
        NearGasPumps = false;
        CanPump = false;
    }
    private void EngineOffTick()
    {

        CanPump = false;


        return;
        //needs reorganizing twith payment and peds

        GameLocation ClosestGasStation = Mod.DataMart.Places.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.GasStation);
        NearGasPumps = false;
        if (ClosestGasStation != null && Game.LocalPlayer.Character.Position.DistanceTo2D(ClosestGasStation.LocationPosition) <= 50f)
        {
            if (ClosestGasStation.GasPumps.Any(x => Game.LocalPlayer.Character.Position.DistanceTo2D(new Vector3(x.X, x.Y, x.Z)) <= 4f))
            {
                NearGasPumps = true;
            }
        }
        if (NearGasPumps && Game.LocalPlayer.Character.CurrentVehicle.FuelLevel <= 100f)
        {
            if (!CanPump)
            {
                Game.DisplayHelp("Hold ~INPUT_CONTEXT~ to refuel", 5000);
            }
            CanPump = true;
        }
        else
        {
            CanPump = false;
        }

        //if (CanPumpFuel && Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.SurrenderKey))
        //{
        //    PumpFuel();
        //}
    }
}

