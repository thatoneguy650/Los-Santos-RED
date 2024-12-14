using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

namespace LSR.Vehicles
{
    public class VehicleExt
    {
        protected VehicleClass vehicleClass;
        protected uint GameTimeEntered = 0;
        protected bool HasAttemptedToLock;
        protected ISettingsProvideable Settings;
        protected int Health = 1000;
        protected bool IsOnFire;
        protected uint GameTimeBecameEmpty;
        protected bool HasAddedRandomItems = false;
        protected bool HasAddedRandomWeapons = false;
        protected bool HasAddedRandomCash = false;
        protected uint GameTimeLastAddedSonarBlip;
        private uint GameTimeLastDamagedByPlayer;
        private uint GameTimeLastUpdatedDamage;
        public bool HasHadPedsRappelOrParachute { get; private set; }
        public uint GameTimeLastHadPedsRappelOrParachute { get; private set; }
        //private List<int> EjectedSeats = new List<int>();

        public List<RappelledSeat> RappelledSeats = new List<RappelledSeat>();


        public DispatchableVehicle DispatchableVehicle { get; set; }
        public VehicleInteractionMenu VehicleInteractionMenu { get; set; }
        public SimpleInventory SimpleInventory { get; private set; }
        public CashStorage CashStorage { get; private set; }
        public VehicleClass VehicleClass => vehicleClass;
        public string VehicleModelName { get; private set; }
        public bool HasShowHotwireLockPrompt { get; set; } = false;
        public bool IsPolice { get; set; } = false;
        public bool IsEMT { get; set; } = false;
        public bool IsFire { get; set; } = false;
        public bool IsGang { get; set; } = false;
        public bool IsService => IsPolice || IsEMT || IsFire;
        public Blip AttachedBlip { get; set; }
        public bool IsHotWireLocked { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public bool IsImpounded { get; set; }
        public DateTime DateTimeImpounded { get; set; }
        public int TimesImpounded { get; set; }
        public string ImpoundedLocation { get; set; }
        public Vehicle Vehicle { get; set; } = null;
        public Vector3 PlaceOriginallyEntered { get; set; }
        public virtual bool IsTaxi => false;
        public bool RecentlyDamagedByPlayer => GameTimeLastDamagedByPlayer != 0 && Game.GameTime - GameTimeLastDamagedByPlayer <= 5000;
        public Radio Radio { get; set; }
        public Indicators Indicators { get; set; }
        public Engine Engine { get; set; }
        public FuelTank FuelTank { get; set; }
        public Windows Windows { get; set; }
        public Doors Doors { get; set; }
        public VehicleBodyManager VehicleBodyManager { get; private set; }
        public WeaponStorage WeaponStorage { get; private set; }
        public Color DescriptionColor { get; set; }
        public LicensePlate CarPlate { get; set; }
        public LicensePlate OriginalLicensePlate { get; set; }
        public Gang AssociatedGang { get; set; }
        public Agency AssociatedAgency { get; set; }
        public SonarBlip SonarBlip { get; set; }
        public DistanceChecker DistanceChecker { get; private set; }
        public virtual Color BlipColor => AssociatedAgency != null ? AssociatedAgency.Color : AssociatedGang != null ? AssociatedGang.Color : Color.White;
        public virtual float BlipSize => AssociatedAgency != null ? 0.6f : 0.25f;
        public uint HasExistedFor => GameTimeSpawned == 0 ? 0 : Game.GameTime - GameTimeSpawned;
        public uint HasBeenEmptyFor => GameTimeBecameEmpty == 0 ? 0 : Game.GameTime - GameTimeBecameEmpty;
        public uint GameTimeSpawned { get; set; }
        public Vector3 PositionSpawned { get; set; } = Vector3.Zero;
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
        public bool CanBeConsideredStolen { get; set; } = true;
        public int FuelTankCapacity { get; private set; } = 20;
        public bool AddedToReportedStolenQueue { get; set; }
        public bool CanBeExported { get; set; } = true;
        public bool CanHavePlateRandomlyUpdated { get; set; } = true;
        public bool NeedsToBeReportedStolen
        {
            get
            {
                if(!WasReportedStolen)
                {
                    return false;
                }
                if (HasBeenEnteredByPlayer && IsStolen && Game.GameTime > GameTimeToReportStolen)
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
        public bool IsWanted => CarPlate != null && CarPlate.IsWanted;// CopsRecognizeAsStolen || (CarPlate != null && CarPlate.IsWanted);
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
                    if (IsWanted)//CarPlate != null && CarPlate.IsWanted)
                    {
                        return true;
                    }
                    //else if (WasReportedStolen && ColorMatchesDescription)//turned off for now, if you have this you need to change the license plate AND the color (maybe good for hard mode, more realistic)
                    //{
                    //    return true;
                    //}
                    else if (Vehicle.Exists() && Vehicle.IsAlarmSounding)
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
        public virtual bool CanUpdatePlate
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

        public bool IsAircraft => IsPlane || IsHeli;
        //public bool IsAircraft
        //{
        //    get
        //    {
        //        if (vehicleClass == VehicleClass.Helicopter || vehicleClass == VehicleClass.Plane)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        //public bool IsBoat
        //{
        //    get
        //    {
        //        if (vehicleClass == VehicleClass.Boat)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        public bool IsCar { get; private set; }
        public bool IsBicycle { get; private set; } = false;
        public bool IsMotorcycle { get; private set; } = false;
        public bool IsQuad { get; private set; } = false;
        public bool IsJetSki { get; private set; } = false;
        public bool IsBoat { get; private set; } = false;
        public bool IsHeli { get; private set; } = false;
        public bool IsPlane { get; private set; } = false;
        public bool CanPerformActivitiesInside => !IsBicycle && !IsMotorcycle && !IsJetSki && !IsQuad;
        public bool IsRandomlyLocked { get; set; } = false;
        public bool UsePlayerAnimations => !IsMotorcycle && !IsBicycle && !IsJetSki && !IsQuad;// VehicleClass != VehicleClass.Motorcycle && VehicleClass != VehicleClass.Cycle;
        public int TimesPassengersRefilled { get; set; }
        public bool HasSpecialPassengerEntry
        {
            get
            {
                return false;//no longer needed? TURN OFF THE FLAG_HAS_REAR_SEAT_ACTIVITIES WHILE KEEPING THE SAME LAYOUT
                if (!Vehicle.Exists())
                {
                    return false;
                }
                string modelName = Vehicle.Model.Name;
                if (modelName == "0xd227bdbb" || modelName == "caddy3")
                {
                    return false;// true;
                }
                return false;
            }
        }
            

        public bool CanBeHotwired => !IsMotorcycle && !IsBicycle && !IsAircraft && !IsBoat && !IsJetSki && !IsQuad;
        public bool IsFreeEntry => IsMotorcycle || IsBicycle || IsAircraft || IsBoat || IsJetSki || IsQuad;
        public virtual bool CanHaveRandomCash { get; set; } = true;
        public virtual float PercentageToGetRandomWeapons => Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomWeapons;
        public virtual bool CanHaveRandomWeapons { get; set; } = true;
        public virtual bool CanHaveRandomItems { get; set; } = true;
        public virtual bool CanRandomlyHaveIllegalItems { get; set; } = true;
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
        public int PoliceBlipID => vehicleClass == VehicleClass.Helicopter ? 15 : 3;
        private void GetOwnedBlipID()
        {
            if(vehicleClass == VehicleClass.Helicopter)
            {
                OwnedBlipID = 64; //radar_helicopter
            }
            else if (vehicleClass == VehicleClass.Plane)
            {
                OwnedBlipID = 307; //radar_plane_drop
            }
            else if (IsMotorcycle)
            {
                OwnedBlipID = 226; //radar_gang_vehicle_bikers
            }
            else if (IsBicycle)
            {
                OwnedBlipID = 226; //radar_gang_vehicle_bikers
            }
            else if (vehicleClass == VehicleClass.Service)
            {
                OwnedBlipID = 198; //radar_taxi
            }
            else if (vehicleClass == VehicleClass.Industrial)
            {
                OwnedBlipID = 477; //radar_truck
            }
            else if (vehicleClass == VehicleClass.Boat)
            {
                OwnedBlipID = 427; //radar_player_boat
            }
            else if (vehicleClass == VehicleClass.Emergency && IsPolice)
            {
                OwnedBlipID = 56; //radar_cop_patrol
            }
            else
            {
                OwnedBlipID = 326;//getawaycar?
            }
            
        }
        public int OwnedBlipID { get; private set; } = 326;//getawaycar?
        public bool HasBeenEnteredByPlayer => GameTimeEntered != 0;
        public VehicleExt(Vehicle vehicle, ISettingsProvideable settings)
        {
            Vehicle = vehicle;
            Settings = settings;
            if (Vehicle.Exists())
            {
                Handle = vehicle.Handle;
                DescriptionColor = Vehicle.PrimaryColor;
                CarPlate = new LicensePlate(Vehicle.LicensePlate, NativeFunction.Natives.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX<int>(Vehicle), false);
                OriginalLicensePlate = CarPlate;
                Health = Vehicle.Health;
                VehicleModelName = vehicle.Model.Name;
                GameTimeSpawned = Game.GameTime;
                PositionSpawned = vehicle.Position;
            }
            else
            {
                CarPlate = new LicensePlate("UNKNOWN", 0, false);
                OriginalLicensePlate = CarPlate;
            }
            Radio = new Radio(this);
            Indicators = new Indicators(this);
            FuelTank = new FuelTank(this, Settings);
            Engine = new Engine(this, Settings);
            Windows = new Windows(this, settings);
            Doors = new Doors(this, settings);
            VehicleBodyManager = new VehicleBodyManager(this, Settings);
            VehicleInteractionMenu = new VehicleInteractionMenu(this);
            WeaponStorage = new WeaponStorage(Settings);
            SimpleInventory = new SimpleInventory(Settings);
            CashStorage = new CashStorage();
            SonarBlip = new SonarBlip(this, Settings);
            DistanceChecker = new DistanceChecker(Settings);
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

                if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem && RequiresFuel)
                {
                    Vehicle.FuelLevel = RandomItems.GetRandomNumber(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin, Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax);// (float)(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin + RandomItems.MyRand.NextDouble() * (settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax - Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin));//RandomItems.MyRand.Next(8, 100);  
                }
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
            if (CarPlate != null)
            {
                CarPlate.IsWanted = false;
            }
            if (OriginalLicensePlate != null)
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
        public bool CanLoadBodies
        {
            get
            {
                if (VehicleClass == VehicleClass.Motorcycle || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Utility || VehicleClass == VehicleClass.Cycle || VehicleClass == VehicleClass.Boat ||
                    VehicleClass == VehicleClass.Helicopter || VehicleClass == VehicleClass.Plane || VehicleClass == VehicleClass.Rail || VehicleClass == VehicleClass.Service)
                {
                    return false;
                }
                return true;
            }
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
        public string GetRegularDescription(bool isOwned)
        {
            string vehicleString = "";
            string VehicleName = FullName(false);
            if(isOwned && IsImpounded)
            {
                vehicleString += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~r~Impounded~s~";
            }
            else if (isOwned)
            {
                vehicleString += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Owned~s~";
            }
            else if (!IsStolen)
            {
                vehicleString += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Unknown~s~";
            }
            else
            {
                vehicleString += $"Vehicle: ~r~{VehicleName}~n~~s~Status: ~r~Stolen~s~";
            }
            if (CarPlate != null && CarPlate.IsWanted)
            {
                vehicleString += $"~n~Plate: ~r~{CarPlate.PlateNumber} ~r~(Wanted)~s~";
            }
            else
            {
                vehicleString += $"~n~Plate: ~p~{CarPlate.PlateNumber} ~s~";
            }
            return vehicleString;
        }
        public string GetCarName()
        {
            if (!Vehicle.Exists())
            {
                return "";
            }
            string MakeName = NativeHelper.VehicleMakeName(Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(Vehicle.Model.Hash);
            string CarName = (MakeName + " " + ModelName).Trim();
            return CarName;
        }
        public string GetCarDescription()
        {
            if(!Vehicle.Exists())
            {
                return "";
            }
            string MakeName = NativeHelper.VehicleMakeName(Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(Vehicle.Model.Hash);
            string ClassName = NativeHelper.VehicleClassName(Vehicle.Model.Hash);
            string CarDescription = "";
            if (MakeName != "")
            {
                CarDescription += $"~n~Manufacturer: ~b~{MakeName}~s~";
            }
            if (ModelName != "")
            {
                CarDescription += $"~n~Model: ~g~{ModelName}~s~";
            }
            if (ClassName != "")
            {
                CarDescription += $"~n~Class: ~p~{ClassName}~s~";
            }
            return CarDescription;
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

                VehicleBodyManager.UpdateData();
                GameFiber.Yield();//TR Added 5
            }
        }
        public void OnPoliceSeenCar()
        {
            if (Vehicle.Exists() && DescriptionColor != Vehicle.PrimaryColor)
            {
                DescriptionColor = Vehicle.PrimaryColor;
            }
            if (IsStolen && WasReportedStolen && CarPlate != null && !CarPlate.IsWanted)
            {
                CarPlate.IsWanted = true;
            }
            //if (IsStolen && !WasReportedStolen)
            //{
            //    WasReportedStolen = true;
            //}
        }
        public void AttemptToLock()
        {
            //in here also check for owner?
            //maybe call this onentrycar lock status?
            //in here check who the owner is, if it is a regular car, if it can be locked, etc.
            if(HasAttemptedToLock || Vehicle.HasOccupants)
            {
                return;
            }
            if (Vehicle.SetLock((VehicleLockStatus)7))// && !Vehicle.IsEngineOn)
            {
                HasAttemptedToLock = true;
                Vehicle.MustBeHotwired = true;
                EntryPoint.WriteToConsole($"AttemptToLock! Locked & Hotwired Vehicle: {Vehicle.Handle}");
            }
        }
        public void UpgradePerformance()//should be an inherited class? VehicleExt and CopCar? For now itll stay in here 
        {
            if (!Vehicle.Exists() || Vehicle.IsHelicopter)
            {
                return;
            }
            NativeFunction.CallByName<bool>("SET_VEHICLE_MOD_KIT", Vehicle, 0);//Required to work
            NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 11, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 11) - 1, true);//Engine
            NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 12, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 12) - 1, true);//Brakes
            NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 13, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 13) - 1, true);//Tranny
            NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", Vehicle, 15, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", Vehicle, 15) - 1, true);//Suspension
        }
        public void UpdatePlatePrefix(IPlatePrefixable AssignedAgency)
        {
            if (AssignedAgency == null || string.IsNullOrEmpty(AssignedAgency.LicensePlatePrefix))
            {
                return;
            }
            if(DispatchableVehicle != null && DispatchableVehicle.OutputFinalPlateType != null && DispatchableVehicle.OutputFinalPlateType.DisablePrefix)
            {
                return;
            }
            string newPlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
            Vehicle.LicensePlate = newPlate;
            CarPlate.PlateNumber = newPlate;
            //if (MyVehicle == null || MyVehicle.RequiredLiveries == null || !MyVehicle.RequiredLiveries.Any())
            //{
            //    return;
            //}
            //NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", Vehicle, MyVehicle.RequiredLiveries.PickRandom());
        }
        public void SetRandomPlate()
        {
            string randomPlate = RandomItems.RandomString(8);
            OriginalLicensePlate = new LicensePlate(randomPlate, 0, false);
            CarPlate = new LicensePlate(randomPlate, 0, false);
            EntryPoint.WriteToConsole($"SET PLATE {randomPlate}");
            if(!Vehicle.Exists())
            { 
                return; 
            }
            Vehicle.LicensePlate = randomPlate;
            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX<int>(Vehicle, 0);
            EntryPoint.WriteToConsole($"SET PLATE FINISH {randomPlate}");
        }
        public void SetRandomColor()
        {
            List<int> PossibleColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 49, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 88, 89,
                90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 111, 112, 125, 137, 141, 142, 143, 145, 146, 150, };
            int color = PossibleColors.PickRandom();
            NativeFunction.Natives.SET_VEHICLE_COLOURS(Vehicle, color, color);
        }
        public bool WasSpawnedEmpty { get; set; } = false;
        public bool IsOwnedByPlayer { get; internal set; }
        public bool AllowVanityPlates { get; set; } = true;
        public bool WasCrushed { get; set; }
        public bool IsAlwaysOpenForPlayer { get; set; } = false;
        public bool CanAlwaysRollOver => VehicleClass == VehicleClass.Motorcycle || VehicleClass == VehicleClass.Cycle;
        public virtual bool HasSonarBlip => true;
        public bool IsManualCleanup { get; internal set; }


        public virtual bool CanNeverUpdatePlate => false;
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
            if(!Vehicle.Exists())
            {
                return;
            }
            if (!(Game.GameTime - GameTimeLastUpdatedDamage >= 1000))
            {
                Health = Vehicle.Health;
                return;
            }
            GameTimeLastUpdatedDamage = Game.GameTime;
            int Damage = Health - Vehicle.Health;
            bool Collided = NativeFunction.Natives.HAS_ENTITY_COLLIDED_WITH_ANYTHING<bool>(Vehicle);
            driver.OnVehicleHealthDecreased(Damage, Collided);
            EntryPoint.WriteToConsole($"PLAYER EVENT Vehicle Crashed Damage {Damage} Collided {Collided}", 5);
            Health = Vehicle.Health;
        }
        public void Setup()
        {
            if(!Vehicle.Exists())
            {
                return;
            }
            SetupClassAndCategory();
            //if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem && RequiresFuel)
            //{
            //    Vehicle.FuelLevel = RandomItems.GetRandomNumber(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin, Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax);// (float)(Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin + RandomItems.MyRand.NextDouble() * (settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax - Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMin));//RandomItems.MyRand.Next(8, 100);  
            //}
            GetFuelTankCapacity();
            IsRandomlyLocked = RandomItems.RandomPercent(Settings.SettingsManager.VehicleSettings.LockVehiclePercentage);
            
        }
        public void ForcePlateType(string text, int index)
        {
            if (!Vehicle.Exists() || string.IsNullOrEmpty(text) || index < 0)
            {
                return;
            }
            HasUpdatedPlateType = true;      
            string cleanedText = text.Left(8);
            Vehicle.LicensePlate = cleanedText;
            OriginalLicensePlate.PlateNumber = cleanedText;
            CarPlate.PlateNumber = cleanedText;       
            if (index <= NativeFunction.Natives.GET_NUMBER_OF_VEHICLE_NUMBER_PLATES<int>())
            {
                NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(Vehicle, index);
                OriginalLicensePlate.PlateType = index;
                CarPlate.PlateType = index;
            }
        }
        public void UpdatePlateType(bool force, IZones Zones, IPlateTypes PlateTypes, bool allowVanity, bool forceState)//this might need to come out of here.... along with the two bools
        {
            if (!Vehicle.Exists())
            {
                return;
            }
            HasUpdatedPlateType = true;
            PlateType CurrentType = PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Vehicle));
            Zone CurrentZone = Zones.GetZone(Vehicle.Position);
            PlateType NewType = null;

            if(forceState)
            {
                NewType = PlateTypes.GetRandomInStatePlate(CurrentZone.StateID);
            }
            else if (force)
            {
                NewType = PlateTypes.GetRandomPlateType();
                //EntryPoint.WriteToConsole($"UPDATE PLATE TYPE FORCE {NewType?.StateID}");
            }
            else if (CanHavePlateRandomlyUpdated && CurrentZone != null && CurrentZone.StateID != StaticStrings.SanAndreasStateID && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.OutOfStateRandomVehiclePlatesPercent))//change the plates based on state
            {
                NewType = PlateTypes.GetRandomInStatePlate(CurrentZone.StateID);
            }
            else
            {
                if (CanHavePlateRandomlyUpdated && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehiclePlatesPercent) && CurrentType != null && CurrentType.CanOverwrite && CanUpdatePlate)
                {
                    NewType = PlateTypes.GetRandomPlateType();
                }
            }
            if (NewType != null)
            {
                //EntryPoint.WriteToConsole($"UPDATE PLATE TYPE FORCE NEW TYPE IS NOT NULL {NewType?.StateID}");
                string NewPlateNumber;
                if (allowVanity && Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent))
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
                if (Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent) && allowVanity && AllowVanityPlates)
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
        public void ResetBecameEmpty()
        {
            GameTimeBecameEmpty = 0;

        }
        public void SetBecameEmpty()
        {
            if (GameTimeBecameEmpty == 0)
            {
                GameTimeBecameEmpty = Game.GameTime;
            }
        }
        private void SetupClassAndCategory()
        {
            vehicleClass = (VehicleClass)ClassInt();
            IsCar = NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Vehicle?.Model.Hash);
            bool isModelBike = NativeFunction.Natives.IS_THIS_MODEL_A_BIKE<bool>((uint)Vehicle?.Model.Hash);
            bool isModelBicycle = NativeFunction.Natives.IS_THIS_MODEL_A_BICYCLE<bool>((uint)Vehicle?.Model.Hash);
            bool isModelQuad = NativeFunction.Natives.IS_THIS_MODEL_A_QUADBIKE<bool>((uint)Vehicle?.Model.Hash);
            bool isModelJetSki = NativeFunction.Natives.IS_THIS_MODEL_A_JETSKI<bool>((uint)Vehicle?.Model.Hash);
            bool isModelBoat = NativeFunction.Natives.IS_THIS_MODEL_A_BOAT<bool>((uint)Vehicle?.Model.Hash);

            bool isModelPlane = NativeFunction.Natives.IS_THIS_MODEL_A_PLANE<bool>((uint)Vehicle?.Model.Hash);
            bool isModelHeli = NativeFunction.Natives.IS_THIS_MODEL_A_HELI<bool>((uint)Vehicle?.Model.Hash);

            IsBicycle = isModelBicycle && isModelBike;
            IsMotorcycle = !isModelBicycle && isModelBike;
            IsQuad = isModelQuad;
            IsJetSki = isModelJetSki;
            IsBoat = isModelBoat;
            IsPlane = isModelPlane;
            IsHeli = isModelHeli;
            GetOwnedBlipID();
        }
        public void RemoveBlip()
        {
            if (!AttachedBlip.Exists() || !Vehicle.Exists())
            {
                return;
            }
            AttachedBlip.Delete();
            AttachedBlip = null;
        }
        public void AddOwnershipBlip()
        {
            if (AttachedBlip.Exists() || !Vehicle.Exists())
            {
                return;
            }
            AttachedBlip = Vehicle.AttachBlip();
            AttachedBlip.Sprite = (BlipSprite)OwnedBlipID;
            AttachedBlip.Color = Color.Red;
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)AttachedBlip.Handle, true);
            EntryPoint.WriteToConsole($"OWNERSHIP BLIP CREATED");
            //EntryPoint.WriteToConsole($"PLAYER EVENT: AddOwnershipBlip", 5);
        }
        public void AddOwnership()
        {
            IsOwnedByPlayer = true;
            if (!Vehicle.Exists())
            {
                return;
            }
            Vehicle.IsStolen = false;
            Vehicle.IsPersistent = true;
        }
        public void RemoveOwnership()
        {
            IsOwnedByPlayer = false;
            if (!Vehicle.Exists())
            {
                return;
            }
            Vehicle.IsPersistent = false;
        }
        public float GetLightEmissive(Vehicle vehicle, LightID index)//from +Vincent in discord
        {
            unsafe
            {
                IntPtr v = vehicle.MemoryAddress;
                IntPtr drawHandler = *(IntPtr*)((IntPtr)v + 0x48);
                if (drawHandler == IntPtr.Zero)
                {
                    return -1f;
                }

                IntPtr customShaderEffect = *(IntPtr*)(drawHandler + 0x20);
                if (customShaderEffect == IntPtr.Zero)
                {
                    return -1f;
                }

                float* lightEmissives = (float*)(customShaderEffect + 0x20);
                return lightEmissives[(int)index];
            }
        }
        public VehicleDoorSeatData GetClosestPedStorageBone(IInteractionable Player, float maxDistance, IVehicleSeatAndDoorLookup vehicleSeatDoorData)
        {
            if(!Vehicle.Exists() || !CanLoadBodies || vehicleSeatDoorData == null) 
            {
                return null;
            }
            float closestBoneDistance = 999f;
            VehicleDoorSeatData boneToReturn = null;
            foreach(VehicleDoorSeatData vdsd in vehicleSeatDoorData.VehicleDoorSeatDataList)
            {
                Vector3 bonePositon = Vector3.Zero;
                if (vdsd.SeatID == -2)//is trunk
                {
                    if (Vehicle.HasBone(vdsd.SeatBone))
                    {
                        bonePositon = Vehicle.GetBonePosition(vdsd.SeatBone);
                    }
                    else
                    {
                        float halfLength = Vehicle.Model.Dimensions.Y / 2.0f;
                        halfLength += 1.0f;
                        bonePositon = Vehicle.GetOffsetPositionFront(-1.0f * halfLength);
                    }
                }
                else
                {
                    if (!Vehicle.HasBone(vdsd.SeatBone))
                    {
                        continue;
                    }
                    bonePositon = Vehicle.GetBonePosition(vdsd.SeatBone);
                }
                float currentBoneDistance = Player.Character.DistanceTo2D(bonePositon);
                if(currentBoneDistance <= maxDistance && currentBoneDistance < closestBoneDistance)
                {
                    boneToReturn = vdsd;
                    closestBoneDistance = currentBoneDistance;
                }
            }
            return boneToReturn;
        }
        public void PutPedInTrunk(IInteractionable Player, PedExt pedExt)
        {

        }
        public enum LightID
        {
            defaultlight = 0,
            headlight_l = 1,
            headlight_r = 2,
            taillight_l = 3,
            taillight_r = 4,
            indicator_lf = 5,
            indicator_rf = 6,
            indicator_lr = 7,
            indicator_rr = 8,
            brakelight_l = 9,
            brakelight_r = 10,
            brakelight_m = 11,
            reversinglight_l = 12,
            reversinglight_r = 13,
            extralight_1 = 14,
            extralight_2 = 15,
            extralight_3 = 16,
            extralight_4 = 17
        }
        public void OpenDoor(int doorID, bool wait)
        {
            if (!Vehicle.Exists())
            {
                return;
            }
            if (!Vehicle.Doors[doorID].IsValid())
            {
                return;
            }
            if (!Vehicle.Doors[doorID].IsFullyOpen)
            {
                Vehicle.Doors[doorID].Open(false, false);
                if (wait)
                {
                    GameFiber.Wait(750);
                }
            }
        }
        public void OpenDoorLoose(int doorID, bool wait)
        {
            if (!Vehicle.Exists())
            {
                return;
            }
            if (!Vehicle.Doors[doorID].IsValid())
            {
                return;
            }
            if (!Vehicle.Doors[doorID].IsFullyOpen)
            {
                Vehicle.Doors[doorID].Open(false, false);
                if (wait)
                {
                    GameFiber.Wait(750);
                }
                Vehicle.Doors[doorID].Open(true, false);
            }
        }
        public void CloseDoor(int doorID)
        {
            if (!Vehicle.Exists())
            {
                return;
            }
            if (!Vehicle.Doors[doorID].IsValid())
            {
                return;
            }
            Vehicle.Doors[doorID].Close(false);
        }
        public void CreateDoorMenu(MenuPool menuPool, UIMenu vehicleInteractMenu)
        {

        }
        public void ResetItems()
        {
            SimpleInventory.Reset();
            WeaponStorage.Reset();
        }
        public virtual void HandleRandomItems(IModItems modItems)
        {
            if(HasAddedRandomItems)
            {
                return;
            }
            if(!CanHaveRandomItems)
            {
                HasAddedRandomItems = true;
                return;
            }
            if (RandomItems.RandomPercent(Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomItems))
            {
                SimpleInventory.AddRandomItems(modItems, true, CanRandomlyHaveIllegalItems);
            }
            HasAddedRandomItems = true;
        }
        public virtual void HandleRandomWeapons(IModItems modItems, IWeapons weapons)
        {
            if (HasAddedRandomWeapons)
            {
                EntryPoint.WriteToConsole("ALREADY ADDED RANDOM WEAPONS");
                return;
            }
            if (!CanHaveRandomWeapons)
            {
                HasAddedRandomWeapons = true;
                EntryPoint.WriteToConsole("CANT HAVE RANDOM WEAPONS");
                return;
            }
            if (RandomItems.RandomPercent(PercentageToGetRandomWeapons))
            {
                EntryPoint.WriteToConsole("ATTEMPT ADD RANDOM WEAPON");
                WeaponStorage.AddRandomWeapons(modItems, weapons);
            }
            HasAddedRandomWeapons = true;
        }
        public void HandleRandomCash()
        {
            if (HasAddedRandomCash)
            {
                EntryPoint.WriteToConsole("ALREADY ADDED RANDOM CASH");
                return;
            }
            if (!CanHaveRandomCash)
            {
                HasAddedRandomCash = true;
                EntryPoint.WriteToConsole("CANT HAVE RANDOM CASH");
                return;
            }
            if (RandomItems.RandomPercent(Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomCash))
            {
                EntryPoint.WriteToConsole("ATTEMPT ADD RANDOM CASH");
                CashStorage.StoredCash = RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.RandomCashMin, Settings.SettingsManager.PlayerOtherSettings.RandomCashMax);
            }
            HasAddedRandomCash = true;
        }
        public void SetImpounded(ITimeReportable time, string locationName, bool hasValidCCW, IWeapons weapons)
        {
            IsImpounded = true;
            DateTimeImpounded = time.CurrentDateTime;
            TimesImpounded++;
            ImpoundedLocation = locationName;
            SimpleInventory.OnImpounded();
            WeaponStorage.OnImpounded(hasValidCCW, weapons);
            if(Vehicle.Exists())
            {
                Vehicle.IsEngineOn = false;
                NativeFunction.Natives.SET_VEHICLE_DOORS_SHUT(Vehicle, true);
            }
        }
        private void UnSetImpounded()
        {
            IsImpounded = false;
            TimesImpounded = 0;
            ImpoundedLocation = "";
        }
        public void AddToImpoundMenu(ILocationAreaRestrictable location, UIMenu impoundSubMenu, ILocationInteractable player, ITimeReportable time)
        {
            int DaysImpounded = (time.CurrentDateTime - DateTimeImpounded).Days;
            int DailyFee = Settings.SettingsManager.RespawnSettings.ImpoundVehiclesDailyFee;
            int StolenExtraFee = Settings.SettingsManager.RespawnSettings.ImpoundVehiclesStolenPenalty;
            int TimeStolen = TimesImpounded - 1;
            int ExtraFee = TimeStolen * StolenExtraFee;
            int fee = DaysImpounded * DailyFee;
            fee += ExtraFee;
            string vehicleName = FullName(false);
            UIMenuItem impoundMenuItem = new UIMenuItem(vehicleName, $"Pay the impound fee and be granted your {vehicleName}.~n~Date Impounded: ~p~{DateTimeImpounded}~s~~n~Impounded Days: ~y~{DaysImpounded}~s~~n~Daily Fee: ~r~${DailyFee}~s~~n~Extra Fee: ~r~${ExtraFee}~s~~n~Total: ~r~${fee}~s~") { RightLabel = $"${fee}" };
            impoundMenuItem.Activated += (sender, selectedItem) =>
            {
                if(player.BankAccounts.GetMoney(true) <= fee)
                {
                    new GTANotification(location.Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction.").Display();
                    NativeHelper.PlayErrorSound();
                    return;
                }
                player.BankAccounts.GiveMoney(-1 * fee, true);
                UnSetImpounded();
                new GTANotification(location.Name, "~g~Payment Accepted", $"Please collect your vehicle from the lot.").Display();
                location.RestrictedAreas?.RemoveImpoundRestrictions();
                NativeHelper.PlaySuccessSound();
                sender.Visible = false;
            };
            impoundSubMenu.AddItem(impoundMenuItem);
        }
        public void AddRegularBlip()
        {
            if (AttachedBlip.Exists() || !Vehicle.Exists())
            {
                return;
            }
            AttachedBlip = Vehicle.AttachBlip();
            AttachedBlip.Scale = 0.25f;
            AttachedBlip.Sprite = (BlipSprite)225;
            AttachedBlip.Color = Color.Blue;
            EntryPoint.WriteToConsole($"VEWHICLE BLIP CREATED");
            //EntryPoint.WriteToConsole($"PLAYER EVENT: AddOwnershipBlip", 5);
        }
        public void AddBlip()
        {
            if (AttachedBlip.Exists() || !Vehicle.Exists())
            {
                return;
            }
            AttachedBlip = Vehicle.AttachBlip();
            AttachedBlip.Scale = BlipSize;
            AttachedBlip.Color = BlipColor;
            EntryPoint.WriteToConsole($"VEHICLE BLIP CREATED");
        }
        public void FullyDelete()
        {
            SonarBlip.Dispose();
            if (AttachedBlip.Exists())
            {
                AttachedBlip.Delete();
                AttachedBlip = null;
            }
            if (!Vehicle.Exists())
            {
                return;
            }
            Vehicle.Delete();
        }
        public bool IsDamaged()
        {
            return IsDamaged(100, 100);
        }
        public bool IsDamaged(int BodyDamageLimit, int EngineDamageLimit)
        {
            if (!Vehicle.Exists())
            {
                return false;
            }
            if (Vehicle.Health <= BodyDamageLimit || Vehicle.EngineHealth <= EngineDamageLimit)//can only see smoke and shit if its running
            {
                return true;
            }
            if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", Vehicle))
            {
                return true;
            }
            foreach (VehicleDoor myDoor in Vehicle.GetDoors())
            {
                if (myDoor.IsDamaged)
                {
                    return true;
                }
            }
            if (IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 0, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 1, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 2, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 3, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 4, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 5, false))
            {
                return true;
            }  
            return false;
        }
        public bool IsVisiblyDamaged(ITimeReportable Time)
        {
            if (!Vehicle.Exists())
            {
                return false;
            }
            if (Vehicle.Health <= Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleHealthLimit || (Vehicle.EngineHealth <= Settings.SettingsManager.VehicleSettings.NonRoadworthyEngineHealthLimit && Engine.IsRunning))//can only see smoke and shit if its running
            {
                return true;
            }
            if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedWindows && !NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", Vehicle))
            {
                return true;
            }
            if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedDoors)
            {
                foreach (VehicleDoor myDoor in Vehicle.GetDoors())
                {
                    if (myDoor.IsDamaged)
                    {
                        return true;
                    }
                }
            }
            if (Time.IsNight && Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedHeadlights)
            {
                if (IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle))
                {
                    return true;
                }
            }
            if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedTires)
            {
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 0, false))
                {
                    return true;
                }
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 1, false))
                {
                    return true;
                }
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 2, false))
                {
                    return true;
                }
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 3, false))
                {
                    return true;
                }
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 4, false))
                {
                    return true;
                }
                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", Vehicle, 5, false))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsRoadWorthy(ITimeReportable Time)
        {
            bool LightsOn;
            bool HighbeamsOn;
            if (Time.IsNight && Engine.IsRunning)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", Vehicle, &LightsOn, &HighbeamsOn);
                }
                if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckNoHeadlights && !LightsOn)
                {
                    return false;
                }
                if (IsCar && Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedHeadlights && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", Vehicle))
                {
                    return false;
                }
            }
            if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckNoPlate && Vehicle.LicensePlate == "        ")
            {
                return false;
            }
            return true;
        }
        public void CreateDoorInteractionMenu(IInteractionable player, MenuPool menuPool, UIMenu doorAccessHeaderMenu, IVehicleSeatAndDoorLookup vehicleSeatDoorData)
        {
            List<VehicleDoorSeatData> stuff = vehicleSeatDoorData.VehicleDoorSeatDataList.ToList();
            stuff.Add(new VehicleDoorSeatData("Hood (Bonnet)","",4,2));
            stuff.Add(new VehicleDoorSeatData("Other", "", 6, 2));
            foreach (VehicleDoorSeatData door in stuff)
            {
                UIMenuListScrollerItem<string> uIMenuListScrollerItem = new UIMenuListScrollerItem<string>(door.SeatName, $"Open or close {door.SeatName}",new List<string>() { "Open","Close" });
                uIMenuListScrollerItem.Activated += (sender, e) =>
                {
                    if (door == null || player.IsInVehicle)
                    {
                        return;
                    }
                    player.ActivityManager.SetDoor(door.DoorID, uIMenuListScrollerItem.SelectedItem == "Open" ? true : false, false, this);
                };
                doorAccessHeaderMenu.AddItem(uIMenuListScrollerItem);
            }
        }
        public void CreateWindowInteractionMenu(IInteractionable player, MenuPool menuPool, UIMenu windowAccessHeaderMenu, IVehicleSeatAndDoorLookup vehicleSeatDoorData)
        {
            List<Tuple<int, string>> stuff = new List<Tuple<int, string>>() { 
                new Tuple<int, string>(0, "Driver"),
                new Tuple<int, string>(1, "Passenger"),
                new Tuple<int, string>(2, "Rear Driver"),
                new Tuple<int, string>(3, "Rear Passenger"),
                new Tuple<int, string>(4, "Middle Driver"),
                new Tuple<int, string>(5, "Middle Passenger"),
                new Tuple<int, string>(6, "Front Windshield"),
                new Tuple<int, string>(7, "Rear Windshield"),
                };
            foreach (Tuple<int, string> window in stuff)
            {
                UIMenuListScrollerItem<string> uIMenuListScrollerItem = new UIMenuListScrollerItem<string>(window.Item2, $"Open or close {window.Item2}", new List<string>() { "Roll Down", "Roll Up" });
                uIMenuListScrollerItem.Activated += (sender, e) =>
                {
                    if (window == null || !player.IsInVehicle)
                    {
                        return;
                    }
                    player.ActivityManager.SetWindowState(window.Item1, uIMenuListScrollerItem.SelectedItem == "Roll Up");
                };
                windowAccessHeaderMenu.AddItem(uIMenuListScrollerItem);
            }
        }
        public virtual void UpdateInteractPrompts(IButtonPromptable player)
        {
            if (!Vehicle.Exists() || (!HasBeenEnteredByPlayer && !IsOwnedByPlayer) || VehicleInteractionMenu.IsShowingMenu || Vehicle.Speed >= 0.5f)
            {
                player.ButtonPrompts.RemovePrompts("VehicleInteract");
               // EntryPoint.WriteToConsole("UpdateInteractPrompts BASE REMOVE 1");
                return;
            }
            if ((!Settings.SettingsManager.UIGeneralSettings.ShowVehicleInteractionPromptInVehicle && player.IsInVehicle) || player.ActivityManager.IsPerformingActivity)
            {
                player.ButtonPrompts.RemovePrompts("VehicleInteract");
               //EntryPoint.WriteToConsole("UpdateInteractPrompts BASE REMOVE 2");
                return;
            }
            if (!player.ButtonPrompts.HasPrompt($"VehicleInteract"))
            {
                Action action = () => { player.ShowVehicleInteractMenu(true); };
                player.ButtonPrompts.AttemptAddPrompt("VehicleInteract", "Vehicle Interact", $"VehicleInteract", Settings.SettingsManager.KeySettings.VehicleInteractModifier, Settings.SettingsManager.KeySettings.VehicleInteract, 999, action);
               // EntryPoint.WriteToConsole("UpdateInteractPrompts BASE ADD PROMPT 1");
            }
        }
        public void UpdateHotwirePrompt(IButtonPromptable player)
        {
            if (!Vehicle.Exists() || (!HasBeenEnteredByPlayer && !IsOwnedByPlayer) || !IsHotWireLocked || VehicleInteractionMenu.IsShowingMenu || Vehicle.Speed >= 0.5f)
            {
                player.ButtonPrompts.RemovePrompts("VehicleHotwire");
                 EntryPoint.WriteToConsole("UpdateHotwirePrompt BASE REMOVE 1");
                return;
            }
            if (player.ActivityManager.IsPerformingActivity)
            {
                player.ButtonPrompts.RemovePrompts("VehicleHotwire");
                EntryPoint.WriteToConsole("UpdateHotwirePrompt BASE REMOVE 2");
                return;
            }
            if (!player.ButtonPrompts.HasPrompt($"VehicleHotwire"))
            {
                Action action = () => { player.ActivityManager.HotwireVehicle(); };
                player.ButtonPrompts.AttemptAddPrompt("VehicleHotwire", "Hotwire", $"VehicleHotwire", GameControl.VehicleAccelerate, 900, action);
                 EntryPoint.WriteToConsole("UpdateHotwirePrompt BASE ADD PROMPT 1");
            }
        }
        public virtual void AddVehicleToList(IEntityProvideable world)
        {
            world.Vehicles.AddCivilian(this);
        }
        public virtual string GetDebugString()
        {
            string vehicleText = $"{Handle}";
            if (Vehicle.Exists())
            {
                vehicleText += $" {Vehicle.Model.Name} WasModSpawned: {WasModSpawned}";
            }
            return vehicleText;
        }
        public void SetReportedStolen()
        {
            if(WasReportedStolen)
            {
                return;
            }
            WasReportedStolen = true;
            if (OriginalLicensePlate != null)
            {
                OriginalLicensePlate.IsWanted = true;
            }
            if (CarPlate != null && OriginalLicensePlate != null && CarPlate.PlateNumber == OriginalLicensePlate.PlateNumber)
            {
                CarPlate.IsWanted = true;
                Game.DisplaySubtitle($"Vehicle Reported Stolen with plate {CarPlate.PlateNumber}");
            }
            else if (OriginalLicensePlate != null)
            {
                Game.DisplaySubtitle($"Vehicle Reported Stolen with plate {OriginalLicensePlate.PlateNumber}");
            }
            else
            {
                Game.DisplaySubtitle($"Vehicle Reported Stolen");
            }
        }
        public virtual void OnDamagedByPlayer(IInteractionable player, IEntityProvideable world)
        {
            if(!Vehicle.Exists())
            {
                return;
            }
            string DamageString = $"I AM VEHICLE {Handle} AND I WAS JUST DAMAGED BY THE PLAYER";
            EntryPoint.WriteToConsole(DamageString);
            //Game.DisplaySubtitle(DamageString);
            NativeFunction.Natives.CLEAR_ENTITY_LAST_DAMAGE_ENTITY(Vehicle);
            PedExt Driver = null;
            if (Vehicle.Driver.Exists())
            {
                Driver = world.Pedestrians.GetPedExt(Vehicle.Driver.Handle);
            }
            if(Driver != null)
            {
                Driver.OnPlayerDamagedCarOnFoot(player);
            }
            GameTimeLastDamagedByPlayer = Game.GameTime;
        }
        public bool CheckPlayerDamage(IInteractionable player, IEntityProvideable world)
        {
            if (!Vehicle.Exists() || RecentlyDamagedByPlayer)
            {
                return false;
            }
            if (NativeFunction.Natives.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY<bool>(Vehicle, player.Character, false))
            {
                OnDamagedByPlayer(player,world);
                return true;
            }
            return false;
        }

        public void AddRappelled(PedExt ped, int seatIndex)
        {
            RappelledSeat rs = new RappelledSeat(ped);
            rs.AddRappelled(Game.GameTime, seatIndex);
            RappelledSeats.Add(rs);
            HasHadPedsRappelOrParachute = true;
            GameTimeLastHadPedsRappelOrParachute = Game.GameTime;
            EntryPoint.WriteToConsole($"VEHICLE MARKED AS RAPPELLED FROM SEAT {seatIndex}");
        }
    }
}
