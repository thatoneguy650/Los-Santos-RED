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
    public static bool CanPumpFuel { get;private set; }
    public static bool IsRunning { get; set; }
    public static string FuelUIText { get; private set; }
    
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    } 
    public static void FuelTick()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsCar)
        {
            float CurrentFuelLevel = Game.LocalPlayer.Character.CurrentVehicle.FuelLevel;
            if (VehicleEngine.IsEngineRunning)
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
            else
            {
                Location ClosestGasStation = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.GasStation);
                NearGasPumps = false;
                if (ClosestGasStation != null && Game.LocalPlayer.Character.Position.DistanceTo2D(ClosestGasStation.LocationPosition) <= 50f)
                {
                    if(ClosestGasStation.GasPumps.Any(x => Game.LocalPlayer.Character.Position.DistanceTo2D(new Vector3(x.X,x.Y,x.Z)) <= 4f))
                    {
                        NearGasPumps = true;
                    }
                }
                if(NearGasPumps && Game.LocalPlayer.Character.CurrentVehicle.FuelLevel <= 100f)
                {
                    if(!CanPumpFuel)
                    {
                        Game.DisplayHelp("Press E to Refuel");
                    }
                    CanPumpFuel = true;
                }
                else
                {
                    CanPumpFuel = false;
                }

                if (CanPumpFuel && Game.IsKeyDownRightNow(LosSantosRED.MySettings.KeyBinding.SurrenderKey))
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
            FuelUIText = string.Format(" Fuel {0} {1} Near Pumps {2}", (Game.LocalPlayer.Character.CurrentVehicle.FuelLevel / 100f).ToString("P2"), VehicleEngine.LimitThrottle ? "Limit: On" : "", NearGasPumps);
        }
    }
   
}

