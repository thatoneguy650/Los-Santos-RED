using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
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
        private ISettingsProvideable Settings;
        private int Health = 1000;
        private bool IsOnFire;

        public Vehicle Vehicle { get; set; } = null;
        public Vector3 PlaceOriginallyEntered { get; set; }
        public Radio Radio { get; set; }
        public Indicators Indicators { get; set; }
       public Engine Engine { get; set; }
        public FuelTank FuelTank { get; set; }  
        public Color DescriptionColor { get; set; }
        public LicensePlate CarPlate { get; set; }
        public LicensePlate OriginalLicensePlate { get; set; }
        public bool WasModSpawned { get; set; } = false;
        public bool ManuallyRolledDriverWindowDown { get; set; }
        public bool HasBeenDescribedByDispatch { get; set; }
        public bool WasAlarmed { get; set; }
        public bool IsStolen { get; set; } = false;//true;
        public bool HasAutoSetRadio { get; set; } = false;
       // public bool OwnedByPlayer { get; set; }
        public bool WasReportedStolen { get; set; }
        public bool HasUpdatedPlateType { get; set; }
        public bool AreAllWindowsIntact { get; set; }
        public uint Handle { get; private set; }
        public bool AddedToReportedStolenQueue { get; set; }
        public bool NeedsToBeReportedStolen
        {
            get
            {
                if (HasBeenEnteredByPlayer && IsStolen && !WasReportedStolen && Game.GameTime > GameTimeToReportStolen)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public uint GameTimeToReportStolen
        {
            get
            {
                if (WasAlarmed && GameTimeEntered > 0)
                {
                    return GameTimeEntered + Settings.SettingsManager.PlayerSettings.AlarmedCarTimeToReportStolen;//60000;// was 100000, 60 second for testing
                }
                else if (GameTimeEntered > 0)
                {
                    return GameTimeEntered + Settings.SettingsManager.PlayerSettings.NonAlarmedCarTimeToReportStolen;// 120000;//was 600000, 120 seconds for testing
                }
                else
                {
                    return 0;
                }
            }
        }
        public bool ColorMatchesDescription => Vehicle.PrimaryColor == DescriptionColor;
        public bool HasOriginalPlate => CarPlate != null && CarPlate.PlateNumber == OriginalLicensePlate.PlateNumber;
        public bool IsWanted => CopsRecognizeAsStolen || (CarPlate != null && CarPlate.IsWanted);
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
                    if (CarPlate != null && CarPlate.IsWanted)
                    {
                        return true;
                    }
                    //else if (WasReportedStolen && ColorMatchesDescription)//turned off for now, if you have this you need to chnage the license plate AND the color (maybe good for hard mode, more realistic)
                    //{
                    //    return true;
                    //}
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
        public bool HasBeenEnteredByPlayer => GameTimeEntered != 0;
        //public VehicleExt(Vehicle vehicle, uint gameTimeEntered) : this(vehicle)
        //{
        //    GameTimeEntered = gameTimeEntered;
        //    PlaceOriginallyEntered = Vehicle.Position;
        //}
        public VehicleExt(Vehicle vehicle, ISettingsProvideable settings)
        {
            Vehicle = vehicle;
            Settings = settings;
            if (Vehicle.Exists())
            {
                Handle = vehicle.Handle;
                DescriptionColor = Vehicle.PrimaryColor;
                CarPlate = new LicensePlate(Vehicle.LicensePlate, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Vehicle), false);
                OriginalLicensePlate = CarPlate;
                if (Settings.SettingsManager.PlayerSettings.UseCustomFuelSystem)
                {
                    Vehicle.FuelLevel = (float)(8f + RandomItems.MyRand.NextDouble() * (100f - 8f));//RandomItems.MyRand.Next(8, 100);
                }
                Health = Vehicle.Health;
            }
            Radio = new Radio(this);
            Indicators = new Indicators(this);
            FuelTank = new FuelTank(this);
            Engine = new Engine(this, Settings);
        }
        public void SetAsEntered()
        {
            if (GameTimeEntered == 0)
            {
                GameTimeEntered = Game.GameTime;
                PlaceOriginallyEntered = Vehicle.Position;
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
        public void SetNotWanted()
        {
            IsStolen = false;
            WasReportedStolen = false;
            if(CarPlate != null)
            {
                CarPlate.IsWanted = false;
            }
            if(OriginalLicensePlate != null)
            {
                OriginalLicensePlate.IsWanted = false;
            }
        }
        public void Update(IDriveable driver)
        {
            if (IsCar)
            {
                Engine.Update(driver);




                if(Settings.SettingsManager.PlayerSettings.KeepRadioAutoTuned)
                {
                    Radio.Update(Settings.SettingsManager.PlayerSettings.AutoTuneRadioStation);
                }
                else if (Settings.SettingsManager.PlayerSettings.AutoTuneRadioOnEntry && !HasAutoSetRadio)
                {
                    Radio.Update(Settings.SettingsManager.PlayerSettings.AutoTuneRadioStation);
                }
                //else
                //{
                //    Radio.Update("NONE");
                //}         




                if (Settings.SettingsManager.PlayerSettings.AllowSetIndicatorState)
                {
                    Indicators.Update();
                }
                if (Settings.SettingsManager.PlayerSettings.UseCustomFuelSystem)
                {
                    FuelTank.Update();
                }
                if(Vehicle.Exists())
                {
                    if(Health > Vehicle.Health)
                    {
                        GameFiber.Yield();
                        OnHealthDecreased(driver); 
                    }
                    else
                    {
                        Health = Vehicle.Health;
                    }
                    bool onFire = Vehicle.IsOnFire;
                    if (IsOnFire != onFire)
                    {
                        if(onFire)
                        {
                            GameFiber.Yield();
                            driver.OnVehicleStartedFire();
                        }
                        IsOnFire = onFire;
                    }
                }
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
                //EntryPoint.WriteToConsole($"AttemptToLock Vehicle: {Vehicle.Handle}");
                if (!Vehicle.HasOccupants)
                {
                    if (Vehicle.SetLock((VehicleLockStatus)7) && !Vehicle.IsEngineOn)
                    {
                        HasAttemptedToLock = true;
                        Vehicle.MustBeHotwired = true;
                        //EntryPoint.WriteToConsole($"AttemptToLock! Locked & Hotwired Vehicle: {Vehicle.Handle}");
                    }
                }
            }
        }
        public void UpgradePerformance()//should be an inherited class? VehicleExt and CopCar? For now itll stay in here 
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
        public void UpdateLivery(Agency AssignedAgency)
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
                    //EntryPoint.WriteToConsole(string.Format("ChangeLivery! No Match for Vehicle {0} for {1}", Vehicle.Model.Name, AssignedAgency.Initials));
                    if(Vehicle.IsPersistent)
                    {
                        EntryPoint.PersistentVehiclesDeleted++;
                    }
                    Vehicle.Delete();
                }
                return;
            }
            if (MyVehicle.RequiredLiveries != null && MyVehicle.RequiredLiveries.Any())
            {
                int NewLiveryNumber = MyVehicle.RequiredLiveries.PickRandom();
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
        public void SetRadioStation(string stationName) => Radio.SetRadioStation(stationName);
        public bool IsCar
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Vehicle?.Model.Hash);//this appears to crash?
            }
        }
        public bool WasSpawnedEmpty { get; set; } = false;
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
        public string IsSuspicious(bool IsNight)
        {
            List<string> SuspiciousReasons = new List<string>();
            bool LightsOn;
            bool HighbeamsOn;
            if (!Vehicle.Exists())
            {
                return "";
            }
            if (Vehicle.Health <= 300 || (Vehicle.EngineHealth <= 300 && Engine.IsRunning))//can only see smoke and shit if its running
            {
                SuspiciousReasons.Add("General Appearance");
            }
            if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", Vehicle))
            {
                SuspiciousReasons.Add("Broken Windows");
            }
            foreach (VehicleDoor myDoor in Vehicle.GetDoors())
            {
                if (myDoor.IsDamaged)
                {
                    SuspiciousReasons.Add("Damaged Doors");
                    break;
                }
            }
            if (IsNight && Engine.IsRunning)
            {
                if (IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle))
                {
                    SuspiciousReasons.Add("Damaged Lights");
                }
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", Vehicle, &LightsOn, &HighbeamsOn);
                }
                if (!LightsOn)
                {
                    SuspiciousReasons.Add("No Lights");
                }
                if (HighbeamsOn)
                {
                    SuspiciousReasons.Add("Highbeams");
                }
            }
            if (Vehicle.LicensePlate == "        ")
            {
                SuspiciousReasons.Add("No Plate");
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 0, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            else if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 1, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            else if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 2, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            else if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 3, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            else if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 4, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            else if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 5, false))
            {
                SuspiciousReasons.Add("Busted Tires");
            }
            return string.Join(", ", SuspiciousReasons);
        }
        private void OnHealthDecreased(IDriveable driver)
        {
            int Damage = Health - Vehicle.Health;
            bool Collided = NativeFunction.Natives.HAS_ENTITY_COLLIDED_WITH_ANYTHING<bool>(Vehicle);
            driver.OnVehicleHealthDecreased(Damage, Collided);
            //EntryPoint.WriteToConsole($"PLAYER EVENT Vehicle Crashed Damage {Damage} Collided {Collided}", 5);
            Health = Vehicle.Health;
        }
    }
}
