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


public class FuelTank
{
    private bool NearGasPumps;
    private uint GameTimeLastCheckedFuel;
    private float CurrentLevel;
    public bool CanPump { get; private set; }
    public string UIText
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
            CurrentLevel = Game.LocalPlayer.Character.CurrentVehicle.FuelLevel;
            if (Mod.Player.CurrentVehicle != null && Mod.Player.CurrentVehicle.Engine.IsRunning)
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
            CanPump = false;
        }
        
    }
    public void PumpFuel()
    {
        if (Game.LocalPlayer.Character.GetCash() >= 1 && Game.LocalPlayer.Character.CurrentVehicle.FuelLevel < 100f)
        {
            Game.LocalPlayer.Character.GiveCash(-1);
            if (CurrentLevel + 1f <= 100f)
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = CurrentLevel + 1f;
            else
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = 100f;
        }
    }
    private void EngineRunningTick()
    {
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            if (CurrentLevel <= 0)
            {
                if (Mod.Player.CurrentVehicle != null)
                {
                    Mod.Player.CurrentVehicle.ToggleEngine(Game.LocalPlayer.Character, false, false);
                }
            }
            else
            {
                float AmountToSubtract = 0.001f + Game.LocalPlayer.Character.CurrentVehicle.Speed * 0.0001f;
                Game.LocalPlayer.Character.CurrentVehicle.FuelLevel = CurrentLevel - AmountToSubtract;
            }
            GameTimeLastCheckedFuel = Game.GameTime;
        }
        NearGasPumps = false;
        CanPump = false;
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

