using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LSR.Vehicles;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Mod;
using LosSantosRED.lsr.Interface;

public class CarBreakIn
{
    private ICarStealable Player;
    private Vehicle TargetVehicle;
    private bool WereWindowsIntact;
    private bool HasBrokenWindow;
    private ISettingsProvideable Settings;
    private int SeatTryingToEnter;
    public CarBreakIn(ICarStealable player, Vehicle targetVehicle, ISettingsProvideable settings, int seatTryingToEnter)
    { 
        Player = player;
        TargetVehicle = targetVehicle;
        Settings = settings;
        SeatTryingToEnter = seatTryingToEnter;
    }
    public void BreakIn()
    {
        try
        {
            //EntryPoint.WriteToConsole("PLAYER EVENT: CarBreakIn Start");
            Player.IsCarJacking = true;
            WereWindowsIntact = NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle);
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                try
                {
                    GameFiber.Yield();
                    while (Game.LocalPlayer.Character.IsGettingIntoVehicle)
                    {
                        if (!HasBrokenWindow && WereWindowsIntact && !NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle))
                        {
                            if (Settings.SettingsManager.VehicleSettings.InjureOnWindowBreak)
                            {
                                Player.Character.Health -= 5;
                            }
                            HasBrokenWindow = true;
                            TargetVehicle.AlarmTimeLeft = new TimeSpan(0, 0, 30);
                        }
                        GameFiber.Yield();
                    }
                    Player.IsCarJacking = false;
                    //EntryPoint.WriteToConsole("PLAYER EVENT: CarBreakIn End");
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "CarBreakIn");
        }
        catch (Exception e)
        {
            Player.IsCarJacking = false;
            EntryPoint.WriteToConsole("CarBreakIn" + e.Message + e.StackTrace, 0);
        }
    }
}

