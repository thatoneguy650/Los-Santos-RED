using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
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
using static DispatchScannerFiles;

namespace LSR.Vehicles
{
    public class VehicleExt
    {
        private VehicleClass vehicleClass;
        private uint GameTimeEntered = 0;
        private bool HasAttemptedToLock;
        private ISettingsProvideable Settings;
        private int Health = 1000;      
        private bool IsOnFire;


        public VehicleClass VehicleClass => vehicleClass;
        public string VehicleModelName { get; private set; }
        public bool HasShowHotwireLockPrompt { get; set; } = false;
        public bool IsPolice { get; set; } = false;
        public bool IsEMT { get; set; } = false;
        public bool IsFire { get; set; } = false;
        public Blip AttachedBlip { get; set; }
        public bool IsHotWireLocked { get; set; } = false;


        public bool IsDisabled { get; set; } = false;

        public Vehicle Vehicle { get; set; } = null;
        public Vector3 PlaceOriginallyEntered { get; set; }
        public Radio Radio { get; set; }
        public Indicators Indicators { get; set; }
       public Engine Engine { get; set; }
        public FuelTank FuelTank { get; set; }  
        public Color DescriptionColor { get; set; }
        public LicensePlate CarPlate { get; set; }
        public LicensePlate OriginalLicensePlate { get; set; }
        public Gang AssociatedGang { get; set; }
        public uint HasExistedFor => Game.GameTime - GameTimeSpawned;
        public uint GameTimeSpawned { get; set; }
        public bool WasModSpawned { get; set; } = false;
        public bool ManuallyRolledDriverWindowDown { get; set; }
        public bool HasBeenDescribedByDispatch { get; set; }
        public bool WasAlarmed { get; set; }
        public bool IsStolen { get; set; } = false;
        public bool HasAutoSetRadio { get; set; } = false;
        public bool WasReportedStolen { get; set; }
        public bool HasUpdatedPlateType { get; set; }
        public bool AreAllWindowsIntact { get; set; }
        public uint Handle { get; private set; }
        public int FuelTankCapacity { get; private set; } = 20;
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
        public uint GameTimeToReportStolen { get; set; } = 0;
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
                if (vehicleClass == VehicleClass.Compact || vehicleClass == VehicleClass.Coupe || vehicleClass == VehicleClass.Muscle || vehicleClass == VehicleClass.OffRoad || vehicleClass == VehicleClass.Sedan || vehicleClass == VehicleClass.Sport
                    || vehicleClass == VehicleClass.SportClassic || vehicleClass == VehicleClass.Super || vehicleClass == VehicleClass.SUV || vehicleClass == VehicleClass.Van)
                {
                    return true;
                }
                return false;
            }
        }
        public bool RequiresFuel
        {
            get
            {
                if (vehicleClass == VehicleClass.Compact || vehicleClass == VehicleClass.Coupe || vehicleClass == VehicleClass.Muscle || vehicleClass == VehicleClass.OffRoad || vehicleClass == VehicleClass.Sedan || vehicleClass == VehicleClass.Sport 
                    || vehicleClass == VehicleClass.SportClassic || vehicleClass == VehicleClass.Super || vehicleClass == VehicleClass.SUV || vehicleClass == VehicleClass.Van || vehicleClass == VehicleClass.Motorcycle 
                    || vehicleClass == VehicleClass.Emergency || vehicleClass == VehicleClass.Industrial || vehicleClass == VehicleClass.Utility || vehicleClass == VehicleClass.Boat || vehicleClass == VehicleClass.Helicopter 
                    || vehicleClass == VehicleClass.Plane || vehicleClass == VehicleClass.Service || vehicleClass == VehicleClass.Military || vehicleClass == VehicleClass.Commercial)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsAircraft
        {
            get
            {
                if (vehicleClass == VehicleClass.Helicopter || vehicleClass == VehicleClass.Plane)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsCar { get; private set; }
        private void GetFuelTankCapacity()
        {
            if (vehicleClass == VehicleClass.Compact) // "Compact":
                FuelTankCapacity = 18;
            else if (vehicleClass == VehicleClass.Sedan) // "Sedan":
                FuelTankCapacity = 20;
            else if (vehicleClass == VehicleClass.SUV) // "SUV":
                FuelTankCapacity = 30;
            else if (vehicleClass == VehicleClass.Coupe) //
                FuelTankCapacity = 20;
            else if (vehicleClass == VehicleClass.Muscle) //
                FuelTankCapacity = 25;
            else if (vehicleClass == VehicleClass.SportClassic) //
                FuelTankCapacity = 25;
            else if (vehicleClass == VehicleClass.Sport) //
                FuelTankCapacity = 20;
            else if (vehicleClass == VehicleClass.Super) //
                FuelTankCapacity = 20;
            else if (vehicleClass == VehicleClass.Motorcycle) //
                FuelTankCapacity = 5;
            else if (vehicleClass == VehicleClass.OffRoad) //
                FuelTankCapacity = 35;
            else if (vehicleClass == VehicleClass.Industrial) //
                FuelTankCapacity = 50;
            else if (vehicleClass == VehicleClass.Utility) //
                FuelTankCapacity = 35;
            else if (vehicleClass == VehicleClass.Van) //
                FuelTankCapacity = 30;
            else if (vehicleClass == VehicleClass.Cycle) //
                FuelTankCapacity = 0;
            else if (vehicleClass == VehicleClass.Boat) //
                FuelTankCapacity = 40;
            else if (vehicleClass == VehicleClass.Helicopter) //
                FuelTankCapacity = 100;
            else if (vehicleClass == VehicleClass.Plane) //
                FuelTankCapacity = 100;
            else if (vehicleClass == VehicleClass.Service) //
                FuelTankCapacity = 40;
            else if (vehicleClass == VehicleClass.Emergency) //
                FuelTankCapacity = 30;
            else if (vehicleClass == VehicleClass.Military) //
                FuelTankCapacity = 60;
            else if (vehicleClass == VehicleClass.Commercial) //
                FuelTankCapacity = 40;
            else if (vehicleClass == VehicleClass.Rail) //
                FuelTankCapacity = 100;
            else
                FuelTankCapacity = 20;
        }
        public bool HasBeenEnteredByPlayer => GameTimeEntered != 0;
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

                Health = Vehicle.Health;
                VehicleModelName = vehicle.Model.Name;
                GameTimeSpawned = Game.GameTime;
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
                if (WasAlarmed)
                {
                    GameTimeToReportStolen = GameTimeEntered + RandomItems.GetRandomNumber(Settings.SettingsManager.VehicleSettings.AlarmedCarTimeToReportStolenMin, Settings.SettingsManager.VehicleSettings.AlarmedCarTimeToReportStolenMax);//IF it is stolen, this is when it would trigger, alarmed cars get called in sooner
                }
                else
                {
                    GameTimeToReportStolen = GameTimeEntered + RandomItems.GetRandomNumber(Settings.SettingsManager.VehicleSettings.NonAlarmedCarTimeToReportStolenMin, Settings.SettingsManager.VehicleSettings.NonAlarmedCarTimeToReportStolenMax);//IF it is stolen, this is when it would trigger
                }
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
                return NativeHelper.VehicleMakeName(Vehicle.Model.Hash);
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
                return NativeHelper.VehicleModelName(Vehicle.Model.Hash);
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



        public string ClassName()
        {
            int ClassInt = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", Vehicle);
            switch (ClassInt)
            {
                case 0:
                    return "Compact";
                case 1:
                    return "Sedan";
                case 2:
                    return "SUV";
                case 3:
                    return "Coupe";
                case 4:
                    return "Muscle";
                case 5:
                    return "Sports Classic";
                case 6:
                    return "Sports Car";
                case 7:
                    return "Super";
                case 8:
                    return "Motorcycle";
                case 9:
                    return "Off Road";
                case 10:
                    return "Industrial";
                case 11:
                    return "Utility";
                case 12:
                    return "Van";
                case 13:
                    return "Bicycle";
                case 14:
                    return "Boat";
                case 15:
                    return "Helicopter";
                case 16:
                    return "Plane";
                case 17:
                    return "Service";
                case 18:
                    return "Emergency";
                case 19:
                    return "Military";
                case 20:
                    return "Commercial";
                case 21:
                    return "Train";
                default:
                    return "";
            }
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
        public string FullName(bool withColor)
        {
            string VehicleName = "";
            Color carColor = VehicleColor();
            string Make = MakeName();
            string Model = ModelName();
            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
            string ColorizedColorName = carColor.Name;
            if (Make != "" && Model != "")
            {
                VehicleName = $"{Make} {Model}";
            }
            else if (Make == "")
            {
                VehicleName = $"{Model}";
            }
            else if (Model == "")
            {
                VehicleName = $"{Make}";
            }
            else
            {
                VehicleName = $"Unknown";
            }
            if (carColor.ToString() != "" && withColor)
            {
                ColorizedColorName = $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
                VehicleName = $"{ColorizedColorName} " + VehicleName;
            }
            return VehicleName;
        }
        public string FullDescription()
        {
            string description = "";
            description += "~n~~s~";
            if (MakeName() != "")
            {
                description += $"~n~Manufacturer: ~b~{MakeName()}~s~";
            }
            if (ModelName() != "")
            {
                description += $"~n~Model: ~g~{ModelName()}~s~";
            }
            if (ClassName() != "")
            {
                description += $"~n~Class: ~p~{ClassName()}~s~";
            }
            return description;
        }
        public void Update(IDriveable driver)
        {
            if (Vehicle.Exists())
            {
                if (!Settings.SettingsManager.VehicleSettings.AllowSetEngineStateOnlyCars || IsCar)
                {
                    Engine.Update(driver);
                }
                if (IsCar)
                {
                    if (Settings.SettingsManager.VehicleSettings.AllowSetIndicatorState)
                    {
                        Indicators.Update();
                    }
                    if (Vehicle.Exists())
                    {
                        if (Health > Vehicle.Health)
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
                            if (onFire)
                            {
                                GameFiber.Yield();
                                driver.OnVehicleStartedFire();
                            }
                            IsOnFire = onFire;
                        }
                    }
                }
                if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem && RequiresFuel)
                {
                    FuelTank.Update();
                    //GameFiber.Yield();//TR Removed 5
                }
                if (Settings.SettingsManager.VehicleSettings.KeepRadioAutoTuned)
                {
                    Radio.Update(Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation);
                    //GameFiber.Yield();//TR Removed 5
                }
                else if (Settings.SettingsManager.VehicleSettings.AutoTuneRadioOnEntry && !HasAutoSetRadio)
                {
                    Radio.Update(Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation);
                    //GameFiber.Yield();//TR Removed 5
                }
                GameFiber.Yield();//TR Added 5
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
                GameFiber.Yield();
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
            GameFiber.Yield();
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

        public bool WasSpawnedEmpty { get; set; } = false;
        public bool OwnedByPlayer { get; internal set; }

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

        public void Setup()
        {
            if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem)
            {
                if (RequiresFuel)
                {
                    Vehicle.FuelLevel = RandomItems.GetRandomNumber(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin, Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax);// (float)(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin + RandomItems.MyRand.NextDouble() * (settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax - Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin));//RandomItems.MyRand.Next(8, 100);
                }
            }
            vehicleClass = (VehicleClass)ClassInt();
            if (Vehicle.Exists())
            {
                IsCar = NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Vehicle?.Model.Hash);
            }
            else
            {
                IsCar = false;
            }
            GetFuelTankCapacity();

        }
        public void UpdatePlateType(bool force, IZones Zones, IPlateTypes PlateTypes)//this might need to come out of here.... along with the two bools
        {
            if (!Vehicle.Exists())
            {
                return;
            }
            HasUpdatedPlateType = true;
            PlateType CurrentType = PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Vehicle));
            Zone CurrentZone = Zones.GetZone(Vehicle.Position);
            PlateType NewType = null;
            if (force)
            {
                NewType = PlateTypes.GetRandomPlateType();
            }
            else if (CurrentZone != null && CurrentZone.State != "San Andreas")//change the plates based on state
            {
                NewType = PlateTypes.GetPlateType(CurrentZone.State);
            }
            else
            {
                if (RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehiclePlatesPercent) && CurrentType != null && CurrentType.CanOverwrite && CanUpdatePlate)
                {
                    NewType = PlateTypes.GetRandomPlateType();
                }
            }
            if (NewType != null)
            {
                string NewPlateNumber;
                if (Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent))
                {
                    NewPlateNumber = PlateTypes.GetRandomVanityPlateText();
                }
                else
                {
                    NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                }
                if (NewPlateNumber != "")
                {
                    Vehicle.LicensePlate = NewPlateNumber;
                    OriginalLicensePlate.PlateNumber = NewPlateNumber;
                    CarPlate.PlateNumber = NewPlateNumber;
                }
                else
                {
                    NewPlateNumber = RandomItems.RandomString(8);
                    Vehicle.LicensePlate = NewPlateNumber;
                    OriginalLicensePlate.PlateNumber = NewPlateNumber;
                    CarPlate.PlateNumber = NewPlateNumber;
                }
                if (NewType.Index <= NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES"))
                {
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Vehicle, NewType.Index);
                    OriginalLicensePlate.PlateType = NewType.Index;
                    CarPlate.PlateType = NewType.Index;
                }
            }
            else
            {
                string NewPlateNumber;
                if (Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent))
                {
                    NewPlateNumber = PlateTypes.GetRandomVanityPlateText();
                    if (NewPlateNumber != "")
                    {
                        Vehicle.LicensePlate = NewPlateNumber;
                        OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        CarPlate.PlateNumber = NewPlateNumber;
                    }
                }
            }
        }



    }
}
