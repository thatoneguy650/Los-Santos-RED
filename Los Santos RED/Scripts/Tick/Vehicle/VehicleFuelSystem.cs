using ExtensionsMethods;
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


internal static class VehicleFuelSystem
{
    private static bool NearGasPumps;
    private static uint GameTimeLastCheckedFuel;
    private static float CurrentFuelLevel;
    public static bool CanPumpFuel { get;private set; }
    public static bool IsRunning { get; set; }
    public static string FuelUIText
    {
        get
        {
            return string.Format(" Fuel {0}", (Game.LocalPlayer.Character.CurrentVehicle.FuelLevel / 100f).ToString("P2"));
        }
    }
    
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    } 
    public static void Tick()
    {
        if (IsRunning)
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsCar)
            {
                CurrentFuelLevel = Game.LocalPlayer.Character.CurrentVehicle.FuelLevel;
                if (VehicleEngine.IsEngineRunning)
                {
                    EngineRunningTick();
                }
                else
                {
                    EngineOffTick();
                }
            }
        }
    }
    private static void EngineRunningTick()
    {
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            if (CurrentFuelLevel <= 0)
            {
                VehicleEngine.TurnOffEngine();
            }
            else
            {
                float AmountToSubtract = 0.001f + Game.LocalPlayer.Character.CurrentVehicle.Speed * 0.0001f;
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = CurrentFuelLevel - AmountToSubtract;
            }
            GameTimeLastCheckedFuel = Game.GameTime;
        }
        NearGasPumps = false;
    }
   private static void EngineOffTick()
    {
        GameLocation ClosestGasStation = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, LocationType.GasStation);
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

        if (CanPumpFuel && Game.IsKeyDownRightNow(General.MySettings.KeyBinding.SurrenderKey))
        {
            PumpFuel();
        }
    }
    private static void PumpFuel()
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
}

