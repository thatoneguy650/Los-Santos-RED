using ExtensionsMethods;
using LosSantosRED.lsr;
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


public class VehicleFuel
{
    private bool NearGasPumps;
    private uint GameTimeLastCheckedFuel;
    private float CurrentFuelLevel;
    public bool CanPumpFuel { get; private set; }
    public string FuelUIText
    {
        get
        {
            return string.Format(" Fuel {0}", (Game.LocalPlayer.Character.CurrentVehicle.FuelLevel / 100f).ToString("P2"));
        }
    }   
    public void Tick()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsCar)
        {
            CurrentFuelLevel = Game.LocalPlayer.Character.CurrentVehicle.FuelLevel;
            if (Mod.Player.VehicleEngine.IsEngineRunning)
            {
                EngineRunningTick();
            }
            else
            {
                EngineOffTick();
            }
        }
        else
        {
            CanPumpFuel = false;
        }
        
    }
    public void PumpFuel()
    {
        if (Game.LocalPlayer.Character.GetCash() >= 1 && Game.LocalPlayer.Character.CurrentVehicle.FuelLevel < 100f)
        {
            Game.LocalPlayer.Character.GiveCash(-1);
            if (CurrentFuelLevel + 1f <= 100f)
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = CurrentFuelLevel + 1f;
            else
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = 100f;
        }
    }
    private void EngineRunningTick()
    {
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            if (CurrentFuelLevel <= 0)
            {
                Mod.Player.VehicleEngine.TurnOffEngine();
            }
            else
            {
                float AmountToSubtract = 0.001f + Game.LocalPlayer.Character.CurrentVehicle.Speed * 0.0001f;
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = CurrentFuelLevel - AmountToSubtract;
            }
            GameTimeLastCheckedFuel = Game.GameTime;
        }
        NearGasPumps = false;
        CanPumpFuel = false;
    }
    private void EngineOffTick()
    {
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
            if (!CanPumpFuel)
            {
                Game.DisplayHelp("Hold ~INPUT_CONTEXT~ to refuel", 5000);
            }
            CanPumpFuel = true;
        }
        else
        {
            CanPumpFuel = false;
        }

        //if (CanPumpFuel && Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.SurrenderKey))
        //{
        //    PumpFuel();
        //}
    }
}

