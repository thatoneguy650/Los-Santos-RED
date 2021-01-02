using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class VehicleExt
    {
        private uint GameTimeEntered = 0;
        private bool HasAttemptedToLock;
        public Vehicle Vehicle { get; set; } = null;
        public Radio Radio { get; set; }
        public Indicators Indicators { get; set; }
        public FuelTank FuelTank { get; set; }  
        public Color DescriptionColor { get; set; }
        public LicensePlate CarPlate { get; set; }
        public LicensePlate OriginalLicensePlate { get; set; }
        public bool WasModSpawned { get; set; }
        public bool ManuallyRolledDriverWindowDown { get; set; }
        public bool HasBeenDescribedByDispatch { get; set; }
        public bool WasAlarmed { get; set; }
        public bool IsStolen { get; set; } = true;
        public bool OwnedByPlayer { get; set; }
        public bool WasReportedStolen { get; set; }
        public bool HasUpdatedPlateType { get; set; }
        public bool AreAllWindowsIntact { get; set; }
        private bool CanToggleEngine
        {
            get
            {
                if (Vehicle.Speed > 2f)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
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
                if (Vehicle.PrimaryColor == DescriptionColor)
                    return true;
                else
                    return false;
            }
        }
        public bool HasOriginalPlate
        {
            get
            {
                if (CarPlate.PlateNumber == OriginalLicensePlate.PlateNumber)
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
                if (!IsStolen)
                {
                    return false;
                }
                else
                {
                    if (CarPlate.IsWanted)
                    {
                        return true;
                    }
                    else if (WasReportedStolen && ColorMatchesDescription)
                    {
                        return true;
                    }
                    else if (Vehicle.IsAlarmSounding)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public bool CanUpdatePlate
        {
            get
            {
                VehicleClass CurrentClass = (VehicleClass)ClassInt();
                if (CurrentClass == VehicleClass.Compact)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.Coupe)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.Muscle)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.OffRoad)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.Sedan)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.Sport)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.SportClassic)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.Super)
                {
                    return true;
                }
                else if (CurrentClass == VehicleClass.SUV)
                {
                    return true;
                }
                return false;
            }
        }
        public bool HasBeenEnteredByPlayer
        {
            get
            {
                if(GameTimeEntered == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public VehicleExt(Vehicle vehicle, uint gameTimeEntered) : this(vehicle)
        {
            GameTimeEntered = gameTimeEntered;
        }
        public VehicleExt(Vehicle vehicle)
        {
            Vehicle = vehicle;
            if (Vehicle.Exists())
            {
                DescriptionColor = Vehicle.PrimaryColor;
                CarPlate = new LicensePlate(Vehicle.LicensePlate, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Vehicle), false);
                OriginalLicensePlate = CarPlate;
                Vehicle.FuelLevel = (float)(8f + RandomItems.MyRand.NextDouble() * (100f - 8f));//RandomItems.MyRand.Next(8, 100);
            }
            Radio = new Radio(this);
            Indicators = new Indicators(this);
            FuelTank = new FuelTank(this);
        }
        public VehicleExt(Vehicle vehicle,bool wasModSpawned) : this(vehicle)
        {
            WasModSpawned = wasModSpawned;
        }
        public void SetAsEntered()
        {
            if (GameTimeEntered == 0)
            {
                GameTimeEntered = Game.GameTime;
            }
        }
        public Color VehicleColor()
        {
            if (Vehicle.Exists())
            {
                Color BaseColor = GetBaseColor(DescriptionColor);
                return BaseColor;
            }
            else
            {
                return Color.White;
            }
        }
        public string MakeName()
        {
            if (Vehicle.Exists())
            {
                string MakeName;
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByHash<IntPtr>(0xF7AF4F159FF99F97, Vehicle.Model.Hash);
                    MakeName = Marshal.PtrToStringAnsi(ptr);
                }
                unsafe
                {
                    IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, MakeName);
                    MakeName = Marshal.PtrToStringAnsi(ptr2);
                }
                if (MakeName == "CARNOTFOUND" || MakeName == "NULL")
                    return "";
                else
                    return MakeName;
            }
            else
            {
                return "";
            }

        }
        public string ModelName()
        {
            if (Vehicle.Exists())
            {
                string ModelName;
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", Vehicle.Model.Hash);
                    ModelName = Marshal.PtrToStringAnsi(ptr);
                }
                unsafe
                {
                    IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
                    ModelName = Marshal.PtrToStringAnsi(ptr2);
                }
                if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
                    return "";
                else
                    return ModelName;
            }
            else
            {
                return "";
            }
        }
        public int ClassInt()
        {
            int ClassInt = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", Vehicle);
            return ClassInt;
        }
        public void Update()
        {
            if (IsCar)
            {
                Radio.Update();
                Indicators.Update();
                FuelTank.Update();
            }
        }
        public void UpdateDescription()
        {
            if (Vehicle.Exists() && DescriptionColor != Vehicle.PrimaryColor)
            {
                DescriptionColor = Vehicle.PrimaryColor;
            }
            if (CarPlate != null && !CarPlate.IsWanted)
            {
                CarPlate.IsWanted = true;
            }
            if (IsStolen && !WasReportedStolen)
            {
                WasReportedStolen = true;
            }
        }
        public void AttemptToLock()
        {
            //in here also check for owner?
            //maybe call this onentrycar lock status?
            //in here check who the owner is, if it is a regular car, if it can be locked, etc.

            if (!HasAttemptedToLock)
            {
                //Game.Console.Print($"AttemptToLock Vehicle: {Vehicle.Handle}");
                if (!Vehicle.HasOccupants)
                {
                    if (Vehicle.SetLock((VehicleLockStatus)7) && !Vehicle.IsEngineOn)
                    {
                        HasAttemptedToLock = true;
                        Vehicle.MustBeHotwired = true;
                        //Game.Console.Print($"AttemptToLock! Locked & Hotwired Vehicle: {Vehicle.Handle}");
                    }
                }
            }
        }
        public void UpgradeCopCarPerformance()//should be an inherited class? VehicleExt and CopCar? For now itll stay in here 
        {
            if (Vehicle.Exists() && !Vehicle.IsHelicopter && Vehicle.IsPoliceVehicle)
            {
                NativeFunction.CallByName<bool>("SET_VEHICLE_MOD_KIT", Vehicle, 0);//Required to work
                NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 11, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 11) - 1, true);//Engine
                NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 12, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 12) - 1, true);//Brakes
                NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 13, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 13) - 1, true);//Tranny
                NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 15, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 15) - 1, true);//Suspension
            }
        }
        public void UpdateCopCarLivery(Agency AssignedAgency)
        {
            DispatchableVehicle MyVehicle = null;
            if (AssignedAgency != null && AssignedAgency.Vehicles != null && Vehicle.Exists())
            {
                MyVehicle = AssignedAgency.Vehicles.Where(x => x.ModelName.ToLower() == Vehicle.Model.Name.ToLower()).FirstOrDefault();
            }
            if (MyVehicle == null)
            {
                if (Vehicle.Exists())
                {
                    //Game.Console.Print(string.Format("ChangeLivery! No Match for Vehicle {0} for {1}", Vehicle.Model.Name, AssignedAgency.Initials));
                    Vehicle.Delete();
                }
                return;
            }
            if (MyVehicle.Liveries != null && MyVehicle.Liveries.Any())
            {
                int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
                NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", Vehicle, NewLiveryNumber);
            }
            Vehicle.LicensePlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
        }
        public void SetDriverWindow(bool RollDown)
        {
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_WINDOW_INTACT", Game.LocalPlayer.Character.CurrentVehicle, 0))
            {
                if (RollDown)
                {
                    NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                    ManuallyRolledDriverWindowDown = true;
                }
                else
                {
                    ManuallyRolledDriverWindowDown = false;
                    NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                }
            }
            else if (!RollDown)
            {
                if (Vehicle != null && ManuallyRolledDriverWindowDown)
                {
                    ManuallyRolledDriverWindowDown = false;
                    NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                }
            }
        }
        public bool IsCar
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Vehicle.Model.Hash);
            }
        }
        private int ClosestColor(List<Color> colors, Color target)
        {
            var colorDiffs = colors.Select(n => ColorDiff(n, target)).Min(n => n);
            return colors.FindIndex(n => ColorDiff(n, target) == colorDiffs);
        }
        private Color GetBaseColor(Color PrimaryColor)
        {
            List<Color> BaseColorList = new List<Color>
        {
            Color.Red,
            Color.Aqua,
            Color.Beige,
            Color.Black,
            Color.Blue,
            Color.Brown,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.DarkGray,
            Color.DarkOrange,
            Color.DarkRed,
            Color.Gold,
            Color.Green,
            Color.Gray,
            Color.LightBlue,
            Color.Maroon,
            Color.Orange,
            Color.Pink,
            Color.Purple,
            Color.Silver,
            Color.White,
            Color.Yellow
        };

            Color MyColor = PrimaryColor;

            int Index = ClosestColor(BaseColorList, MyColor);
            return BaseColorList[Index];
        }
        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                    + (c1.G - c2.G) * (c1.G - c2.G)
                                    + (c1.B - c2.B) * (c1.B - c2.B));
        }
    }
}