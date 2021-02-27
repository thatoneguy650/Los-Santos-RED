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
    public CarBreakIn(ICarStealable player, Vehicle targetVehicle)
    { 
        Player = player;
        TargetVehicle = targetVehicle;
    }
    public void BreakIn()
    {
        try
        {
            Player.IsCarJacking = true;
            WereWindowsIntact = NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle);
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                while(Game.LocalPlayer.Character.IsGettingIntoVehicle)
                {
                    if(!HasBrokenWindow && WereWindowsIntact && !NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle))
                    {
                        HasBrokenWindow = true;
                        TargetVehicle.AlarmTimeLeft = new TimeSpan(0, 0, 30);
                    }
                    GameFiber.Yield();
                }
                Player.IsCarJacking = false;
            }, "CarBreakIn");
        }
        catch (Exception e)
        {
            Player.IsCarJacking = false;
            EntryPoint.WriteToConsole("CarBreakIn" + e.Message + e.StackTrace, 0);
        }
    }
}

