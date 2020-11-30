﻿using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class AmbientPlateManager
{
    private static readonly string ConfigFileName = "Plugins\\LosSantosRED\\PlateTypes.xml";
    private static List<PlateType> PlateTypes = new List<PlateType>();
    private static List<AmbientVehicle> AmbientVehicles = new List<AmbientVehicle>();
    private static bool UseVanillaConfig = false;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        AmbientVehicles = new List<AmbientVehicle>();
        PlateTypes = new List<PlateType>();
        ReadConfig();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdateVehiclePlates();
        }
    }
    public static void UpdateCurrentVehiclePlate()
    {
        if (Game.LocalPlayer.Character.CurrentVehicle != null)
        {
            AmbientVehicle Mine = AmbientVehicles.FirstOrDefault(x => x.CivVehicle.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle);
            if (Mine != null)
            {
                Mine.UpdatePlate();
            }
        }
    }
    private static void UpdateVehiclePlates()
    {
        int VehiclesUpdated = 0;
        AmbientVehicles.RemoveAll(x => !x.CivVehicle.VehicleEnt.Exists());
        foreach (VehicleExt MyCar in VehicleManager.CivilianVehicles.Where(x => x.VehicleEnt.Exists()))
        {
            if (!AmbientVehicles.Any(x => x.CivVehicle.VehicleEnt.Handle == MyCar.VehicleEnt.Handle))
            {
                AmbientVehicle AmbCar = new AmbientVehicle(MyCar);
                AmbCar.UpdatePlate();
                AmbientVehicles.Add(AmbCar);
                VehiclesUpdated++;
            }
            if (VehiclesUpdated > 5)
            {
                break;
            }
        }
    }
    private static PlateType GetPlateType(int CurrentIndex)
    {
        return PlateTypes.FirstOrDefault(x => x.Index == CurrentIndex);
    }
    private static PlateType GetRandomPlateType()
    {
        if (!PlateTypes.Any())
            return null;

        List<PlateType> ToPickFrom = PlateTypes.Where(x => x.CanSpawn).ToList();
        int Total = ToPickFrom.Sum(x => x.SpawnChance);
        int RandomPick = General.MyRand.Next(0, Total);
        foreach (PlateType Type in ToPickFrom)
        {
            int SpawnChance = Type.SpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Type;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    private static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            PlateTypes = General.DeserializeParams<PlateType>(ConfigFileName);
        }
        else
        {
            if (UseVanillaConfig)
            {
                DefaultConfig();
            }
            else
            {
                CustomConfig();
            }
            General.SerializeParams(PlateTypes, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {
        PlateTypes.Add(new PlateType(0, "Red on White California", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(1, "Yellow on Black California", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(2, "Yellow on Blue California", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(3, "Classic California", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(4, "Exempt California", "San Andreas", 0, "1ABC234") { CanOverwrite = false });
    }
    private static void CustomConfig()
    {
        DefaultConfig();
        PlateTypes.Add(new PlateType(5, "New York 1", "Liberty", 0, "ABC-1234"));
        PlateTypes.Add(new PlateType(6, "Florida 1", "Vice City", 0, "123 4AB"));
        PlateTypes.Add(new PlateType(7, "New York 2", "Liberty", 0, "ABC-1234"));
        PlateTypes.Add(new PlateType(8, "New York 3", "Liberty", 0, "ABC-1234"));
        PlateTypes.Add(new PlateType(9, "Sprunk Logo", "None", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(10, "Patriots?", "None", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(11, "Los Santos Shrimps", "San Andreas", 1, "1ABC234"));
        PlateTypes.Add(new PlateType(12, "Nevada San Andreas Mashup", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(13, "North Carolina", "North Volucrina", 3, "ABC-1234"));
        PlateTypes.Add(new PlateType(14, "New Jersey", "Alderney", 3, "D12-ABC"));
        PlateTypes.Add(new PlateType(15, "Nevada", "Robada", 3, "123-A45"));
        PlateTypes.Add(new PlateType(16, "Illinois", "Lincoln", 3, "AB 12345"));
        PlateTypes.Add(new PlateType(17, "Our Florida", "Miami", 3, "123 4AB"));
        PlateTypes.Add(new PlateType(18, "Florida 1", "Miami", 0, "123 4AB"));
        PlateTypes.Add(new PlateType(19, "Florida 2", "Miami", 0, "123 4AB"));
        PlateTypes.Add(new PlateType(20, "Arizona", "Hareona", 3));
        PlateTypes.Add(new PlateType(21, "North Dakota New", "North Yankton", 3, "123 ABC"));
        PlateTypes.Add(new PlateType(22, "South Dakota New", "South Yankton", 3, "0A1 234"));
        PlateTypes.Add(new PlateType(23, "South Carolina", "South Volucrina", 3, "ABC 123"));
        PlateTypes.Add(new PlateType(24, "Firefighter California", "San Andras", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(25, "Texas 1", "Alamo", 1, "ABC-1234"));
        PlateTypes.Add(new PlateType(26, "Texas 2", "Alamo", 1, "ABC-1234"));
        PlateTypes.Add(new PlateType(27, "Texas 3", "Alamo", 1, "ABC-1234"));
        PlateTypes.Add(new PlateType(28, "Idaho", "Cataldo", 3, "A 123456"));
        PlateTypes.Add(new PlateType(29, "Louisiana", "Maraisiana", 3, "123 ABC"));
        PlateTypes.Add(new PlateType(30, "Oregon", "Cascadia", 3, "123 ABC"));
        PlateTypes.Add(new PlateType(31, "Corvette Plate", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(32, "Nothing", "Liberty City", 0));
        PlateTypes.Add(new PlateType(33, "Nothing", "Liberty City", 0));
        PlateTypes.Add(new PlateType(34, "Nothing", "Liberty City", 0));
        PlateTypes.Add(new PlateType(35, "Montana", "Colina", 3, "0-12345A"));
        PlateTypes.Add(new PlateType(36, "Colorado", "Coloverdo", 3, "ABC-D12"));
        PlateTypes.Add(new PlateType(37, "Washington", "Jefferson", 3, "ABC1234"));
        PlateTypes.Add(new PlateType(38, "Washington DC", "Jefferson CD", 3, "AB-1234"));
        PlateTypes.Add(new PlateType(39, "Wisconsin", "Meskousin", 3, "ABC-1234"));
        PlateTypes.Add(new PlateType(40, "Black on Yellow California", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(41, "Nothing", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(42, "Minnesota", "Minnewa", 3, "123-ABC"));
        PlateTypes.Add(new PlateType(43, "Michigan", "Misquakewan", 3, "ABC 1234"));
        PlateTypes.Add(new PlateType(44, "Nothing", "Carcer City", 0));
        PlateTypes.Add(new PlateType(45, "Alaska", "Tanadux", 3, "ABC 123"));
        PlateTypes.Add(new PlateType(46, "Hawaii", "Haiateaa", 3, "ABC 123"));
        PlateTypes.Add(new PlateType(47, "Nothing", "San Andreas", 0, "1ABC234"));
        PlateTypes.Add(new PlateType(48, "Nothing", "None", 0));
        PlateTypes.Add(new PlateType(49, "Nothing", "None", 0));
        PlateTypes.Add(new PlateType(50, "Nothing", "None", 0));
        PlateTypes.Add(new PlateType(51, "Nothing", "None", 0));
        PlateTypes.Add(new PlateType(52, "Nothing", "None", 0));
    }
    private class AmbientVehicle
    {
        public VehicleExt CivVehicle { get; set; }
        public bool CanUpdatePlate
        {
            get
            {
                VehicleClass CurrentClass = (VehicleClass)CivVehicle.ClassInt();
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
        public AmbientVehicle(VehicleExt vehicle)
        {
            CivVehicle = vehicle;
        }     
        public void UpdatePlate()
        {
            PlateType CurrentType = GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CivVehicle.VehicleEnt));
            string CurrentPlateNumber = CivVehicle.VehicleEnt.LicensePlate;
            if (General.RandomPercent(10) && CurrentType != null && CurrentType.CanOverwrite && CanUpdatePlate)
            {
                PlateType NewType = GetRandomPlateType();
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        CivVehicle.VehicleEnt.LicensePlate = NewPlateNumber;
                        CivVehicle.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        CivVehicle.CarPlate.PlateNumber = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CivVehicle.VehicleEnt, NewType.Index);
                    CivVehicle.OriginalLicensePlate.PlateType = NewType.Index;
                    CivVehicle.CarPlate.PlateType = NewType.Index;
                }
            }
        }

    }
}

