using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTAVehicle
{
    private static Random rnd;

    public Vehicle VehicleEnt = null;
    public uint GameTimeEntered = 0;
    public bool WillBeReportedStolen = false;
    public bool WasReportedStolen = false;
    public bool CopWillRecognize = false;
    public bool WasAlarmed = false;
    public bool WasJacked = false;
    public uint GameTimeToReportStolen;
    public uint ReportInterval = 30000;
    public Ped PreviousOwner = null;
    public bool PreviousOwnerDied = false;
    public bool IsPlayersVehicle = false;
    public bool IsStolen = false;
    public bool QuedeReportedStolen = false;
    public bool WasSpottedAbandoned = false;
    public bool CopsCanSee = false;
    public Color DescriptionColor;
    public GTALicensePlate CarPlate;
    public GTALicensePlate OriginalLicensePlate;
    public bool ManuallyRolledDriverWindowDown = false;
    public Vector3 PositionOriginallyEntered = Vector3.Zero;
    public bool PlayerHasEntered
    {
        get
        {
            if (GameTimeEntered == 0)
                return false;
            else
                return true;
        }
    }
    public bool ShouldReportStolen
    {
        get
        {
            if (WasReportedStolen || QuedeReportedStolen)
                return false;
            else if (WillBeReportedStolen && Game.GameTime >= GameTimeToReportStolen && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == VehicleEnt.Handle)
                return true;
            else
                return false;
        }
    }
    public bool MatchesOriginalDescription
    {
        get
        {
            if (VehicleEnt.PrimaryColor == DescriptionColor && CarPlate.IsWanted)//VehicleEnt.LicensePlate == Desc)
                return true;
            else
                return false;
        }
    }
    public bool ColorMatchesDescription
    {
        get
        {
            if (VehicleEnt.PrimaryColor == DescriptionColor)
                return true;
            else
                return false;
        }
    }
    static GTAVehicle()
    {
        rnd = new Random();
    }
    public void WatchForDeath(Ped Pedestrian)
    {
        GameFiber WatchForDeath = GameFiber.StartNew(delegate
        {
            uint GameTimeStolen = Game.GameTime;
            while(Pedestrian.Exists())
            {
                Pedestrian.IsPersistent = true;
                WillBeReportedStolen = false;

                if (Pedestrian.IsDead)
                {
                    WillBeReportedStolen = false;
                    PreviousOwnerDied = true;
                    Pedestrian.IsPersistent = false;
                    //Debugging.WriteToLog("StolenVehicles", string.Format("PreviousOwnerDied {0},WillBeReportedStolen {1}", PreviousOwnerDied, WillBeReportedStolen));
                    break;
                }
                else if(Game.GameTime - GameTimeStolen > 15000 && !Pedestrian.IsRagdoll)
                {
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Pedestrian, 10000);
                    Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");
                    GameFiber.Sleep(5000);
                    if (Pedestrian.Exists() && !Pedestrian.IsDead && !Pedestrian.IsRagdoll)
                    {
                        WillBeReportedStolen = true;
                        Pedestrian.IsPersistent = false;
                    }
                    Debugging.WriteToLog("StolenVehicles", string.Format("WillBeReportedStolen {0}", WillBeReportedStolen));
                    break;
                }
                
                GameFiber.Yield();
            }
            Debugging.WriteToLog("StolenVehicles", string.Format("PreviousOwnerDisappeared? Died {0},WillBeReportedStolen {1}", PreviousOwnerDied, WillBeReportedStolen));
        }, "WatchForDeath");
        Debugging.GameFibers.Add(WatchForDeath);
    }
    public GTAVehicle(Vehicle _Vehicle, bool _IsPlayersVehicle, bool _IsStolen, GTALicensePlate _CarPlate)
    {
        VehicleEnt = _Vehicle;
        IsStolen = _IsStolen;
        IsPlayersVehicle = _IsPlayersVehicle;
        CarPlate = _CarPlate;
        OriginalLicensePlate = _CarPlate;
        if (VehicleEnt.Exists())
            PositionOriginallyEntered = VehicleEnt.Position;
        else
            PositionOriginallyEntered = Game.LocalPlayer.Character.Position;
    }
    public GTAVehicle(Vehicle _Vehicle,uint _GameTimeEntered,bool _WasJacked, bool _WasAlarmed, Ped _PrevIousOwner, bool _IsPlayersVehicle, bool _IsStolen, GTALicensePlate _CarPlate)
    {
        VehicleEnt = _Vehicle;
        GameTimeEntered = _GameTimeEntered;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        PreviousOwner = _PrevIousOwner;
        IsStolen = _IsStolen;
        IsPlayersVehicle = _IsPlayersVehicle;

        DescriptionColor = _Vehicle.PrimaryColor;
        CarPlate = _CarPlate;
        OriginalLicensePlate = _CarPlate;

        if (IsPlayersVehicle)
            IsStolen = false;

        if (IsStolen)
            WillBeReportedStolen = true;



        if (IsStolen && WillBeReportedStolen && PreviousOwner != null && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
        {
            if (PreviousOwner.IsPoliceArmy())
            {
                Debugging.WriteToLog("StolenVehicles", "Previous Owner is Cop reported immediately");
                WillBeReportedStolen = true;
            }
            else
            {
                Debugging.WriteToLog("StolenVehicles", "Previous Owner is alive, will watch for death");
                WatchForDeath(PreviousOwner);
            }
        }

        if (WasJacked)
            GameTimeToReportStolen = GameTimeEntered + 15000;
        else if (WasAlarmed)
            GameTimeToReportStolen = GameTimeEntered + 100000;
        else
            GameTimeToReportStolen = GameTimeEntered + 600000;

        if (VehicleEnt.Exists())
            PositionOriginallyEntered = VehicleEnt.Position;
        else
            PositionOriginallyEntered = Game.LocalPlayer.Character.Position;

        Debugging.WriteToLog("GTAVehicle", string.Format("Vehicle Created: Handle {0},GTEntered,{1},GTReportStolen {2},WasJacked {3},WasAlarmed {4},IsStolen {5},WillBeRptdStoln {6},WatchLastOwner {7}", VehicleEnt.Handle, GameTimeEntered, GameTimeToReportStolen, WasJacked,WasAlarmed, IsStolen, WillBeReportedStolen, PreviousOwner != null));
    }
    public GTAVehicle(Vehicle _Vehicle, bool _WasJacked, bool _WasAlarmed, Ped _PrevIousOwner, bool _IsPlayersVehicle, bool _IsStolen, GTALicensePlate _CarPlate)
    {
        VehicleEnt = _Vehicle;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        PreviousOwner = _PrevIousOwner;
        IsStolen = _IsStolen;
        IsPlayersVehicle = _IsPlayersVehicle;

        DescriptionColor = _Vehicle.PrimaryColor;
        CarPlate = _CarPlate;
        OriginalLicensePlate = _CarPlate;

        if (VehicleEnt.Exists())
            PositionOriginallyEntered = VehicleEnt.Position;
        else
            PositionOriginallyEntered = Game.LocalPlayer.Character.Position;
    }

}

