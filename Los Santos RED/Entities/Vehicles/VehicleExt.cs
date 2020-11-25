using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleExt
{
    private uint GameTimeEntered = 0;

    public Vehicle VehicleEnt { get; set; } = null;
    public Ped PreviousOwner { get; set; } = null;
    public Color DescriptionColor { get; set; }
    public LicensePlate CarPlate { get; set; }
    public LicensePlate OriginalLicensePlate { get; set; }
    public bool ManuallyRolledDriverWindowDown { get; set; }
    public Vector3 PositionOriginallyEntered { get; set; } = Vector3.Zero;
    public bool HasBeenDescribedByDispatch { get; set; }
    public bool WasAlarmed { get; set; }
    public bool WasJacked { get; set; }
    public bool IsStolen { get; set; }
    public bool WasReportedStolen { get; set; }
    public bool NeedsToBeReportedStolen
    {
        get
        {
            if (!WasReportedStolen && Game.GameTime > GameTimeToReportStolen && GameTimeEntered > 0)
                return true;
            else
                return false;
        }
    }
    public uint GameTimeToReportStolen
    {
        get
        {
            if (WasAlarmed && GameTimeEntered > 0)
                return GameTimeEntered + 100000;
            else if (GameTimeEntered > 0)
                return GameTimeEntered + 600000;
            else
                return 0;
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
    public bool HasOriginalPlate
    {
        get
        {
            if(CarPlate.PlateNumber == OriginalLicensePlate.PlateNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool CopsRecognizeAsStolen
    {
        get
        {
            if(!IsStolen)
                return false;
            else
            {
                if (CarPlate.IsWanted)
                    return true;
                else if (WasReportedStolen && ColorMatchesDescription)
                    return true;
                else
                    return false;
            }
        }
    }
    public VehicleExt(Vehicle _Vehicle,uint _GameTimeEntered,bool _WasJacked, bool _WasAlarmed, Ped _PrevIousOwner, bool _IsStolen, LicensePlate _CarPlate)
    {
        VehicleEnt = _Vehicle;
        GameTimeEntered = _GameTimeEntered;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        PreviousOwner = _PrevIousOwner;
        IsStolen = _IsStolen;

        DescriptionColor = _Vehicle.PrimaryColor;
        CarPlate = _CarPlate;
        OriginalLicensePlate = _CarPlate;

        if (VehicleEnt.Exists())
            PositionOriginallyEntered = VehicleEnt.Position;
        else
            PositionOriginallyEntered = Game.LocalPlayer.Character.Position;

        _Vehicle.FuelLevel = General.MyRand.Next(25, 100);

        //Debugging.WriteToLog("GTAVehicle", string.Format("Vehicle Created: Handle {0},GTEntered,{1},GTReportStolen {2},WasJacked {3},WasAlarmed {4},IsStolen {5},WatchLastOwner {6}", VehicleEnt.Handle, GameTimeEntered, GameTimeToReportStolen, WasJacked,WasAlarmed, IsStolen, PreviousOwner != null));
    }
    public void SetAsEntered()
    {
        if (GameTimeEntered == 0)
            GameTimeEntered = Game.GameTime;
    }

}

