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
    private IWorld World;
    private IPlayer Player;
    private string Animation = "std_force_entry_ds";
    private int DoorIndex = 0;
    private int WaitTime = 1750;
    private VehicleLockStatus OriginalLockStatus;
    private Rage.Object Screwdriver;
    private Vehicle TargetVehicle;
    private int SeatTryingToEnter;
    private bool WereWindowsIntact;
    private bool HasBrokenWindow;
    public CarBreakIn(IWorld world, IPlayer player, Vehicle targetVehicle, int seatTryingToEnter)
    {
        World = world;
        Player = player;
        TargetVehicle = targetVehicle;
        SeatTryingToEnter = seatTryingToEnter;
    }

    public void BreakIn()
    {
        try
        {
            WereWindowsIntact = NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle);
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                while(Game.LocalPlayer.Character.IsGettingIntoVehicle && !HasBrokenWindow)
                {
                    if(WereWindowsIntact && !NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", TargetVehicle))
                    {
                        Player.SetSmashedWindow();
                        HasBrokenWindow = true;
                        TargetVehicle.AlarmTimeLeft = new TimeSpan(0, 0, 30);
                        Game.Console.Print($"Vehicle {TargetVehicle.Handle} You Smashed the Windows");
                    }
                    GameFiber.Yield();
                }

            }, "CarBreakIn");
        }
        catch (Exception e)
        {
            Player.IsLockPicking = false;
            Game.Console.Print("CarBreakIn" + e.Message + e.StackTrace);
        }
    }
}

