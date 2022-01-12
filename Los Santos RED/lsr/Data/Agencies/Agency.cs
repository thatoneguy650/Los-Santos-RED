using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

[Serializable()]
public class Agency
{
    public Agency()
    {
    }
    public Agency(string _ColorPrefix, string _ID, string _FullName, string _AgencyColorString, Classification _AgencyClassification, List<DispatchablePerson> _CopModels, List<DispatchableVehicle> _Vehicles, string _LicensePlatePrefix, List<IssuableWeapon> sideArms, List<IssuableWeapon> longGuns)
    {
        ColorPrefix = _ColorPrefix;
        ID = _ID;
        FullName = _FullName;
        Personnel = _CopModels;
        ColorString = _AgencyColorString;
        Vehicles = _Vehicles;
        Classification = _AgencyClassification;
        LicensePlatePrefix = _LicensePlatePrefix;
        SideArms = sideArms;
        LongGuns = longGuns;   
    }
    public Classification Classification { get; set; } = Classification.Other;
    public ResponseType ResponseType => Classification == Classification.EMS ? ResponseType.EMS : Classification == Classification.Fire ? ResponseType.Fire : ResponseType.LawEnforcement;
    public Color Color => Color.FromName(ColorString);
    public string ColorString { get; set; } = "White";
    public bool CanCheckTrafficViolations
    {
        get
        {
            if (Classification == Classification.Police || Classification == Classification.Federal || Classification == Classification.Sheriff || Classification == Classification.State)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool CanSpawnAnywhere { get; set; } = false;
    public string ColorInitials
    {
        get
        {
            return ColorPrefix + ID;
        }
    }
    public string ColorPrefix { get; set; } = "~s~";
    public string FullName { get; set; } = "Unknown";
    public bool HasMotorcycles => Vehicles.Any(x => x.IsMotorcycle);
    public string ID { get; set; } = "UNK";
    public string LicensePlatePrefix { get; set; } = "";
    public uint MaxWantedLevelSpawn { get; set; } = 6;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public int SpawnLimit { get; set; } = 99;
    public bool SpawnsOnHighway { get; set; } = false;


    public int AccuracyMin { get; set; } = 40;
    public int AccuracyMax { get; set; } = 40;


    public int ShootRateMin { get; set; } = 500;
    public int ShootRateMax { get; set; } = 500;


    public int CombatAbilityMin { get; set; } = 1;//0 - poor, 1- average, 2 - professional
    public int CombatAbilityMax { get; set; } = 2;//0 - poor, 1- average, 2 - professional





    public List<DispatchablePerson> Personnel { get; set; } = new List<DispatchablePerson>();
    public List<IssuableWeapon> SideArms { get; set; } = new List<IssuableWeapon>();
    public List<IssuableWeapon> LongGuns { get; set; } = new List<IssuableWeapon>();
    public List<DispatchableVehicle> Vehicles { get; set; } = new List<DispatchableVehicle>();
    public bool CanSpawn(int wantedLevel) => wantedLevel >= MinWantedLevelSpawn && wantedLevel <= MaxWantedLevelSpawn;
    public DispatchablePerson GetRandomPed(int wantedLevel, List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any())
            return null;

        List<DispatchablePerson> ToPickFrom = Personnel.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if (RequiredModels != null && RequiredModels.Any())
        {
            ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel));
        //Mod.Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchablePerson Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance(wantedLevel);
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public DispatchableVehicle GetRandomVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles)
    {
        if (Vehicles != null && Vehicles.Any())
        {
            List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel) && !x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
            if (includeBoats)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel) && x.IsBoat).ToList());
            }
            if (includeHelicopters)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel) && x.IsHelicopter).ToList());
            }
            if (includeMotorcycles)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel) && x.IsMotorcycle).ToList());
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel));
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (DispatchableVehicle Vehicle in ToPickFrom)
            {
                int SpawnChance = Vehicle.CurrentSpawnChance(wantedLevel);
                if (RandomPick < SpawnChance)
                {
                    return Vehicle;
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public IssuableWeapon GetRandomWeapon(bool isSidearm, IWeapons weapons)
    {
        IssuableWeapon weaponToIssue;
        if (isSidearm)
        {
            weaponToIssue = SideArms.PickRandom();
        }
        else
        {
            weaponToIssue = LongGuns.PickRandom();
        }
        WeaponInformation WeaponLookup = weapons.GetWeapon(weaponToIssue.ModelName);
        weaponToIssue.SetIssued(Game.GetHashKey(weaponToIssue.ModelName), WeaponLookup.PossibleComponents);
        return weaponToIssue;
    }
    public DispatchableVehicle GetVehicleInfo(Vehicle vehicle) => Vehicles.Where(x => x.ModelName.ToLower() == vehicle.Model.Name.ToLower()).FirstOrDefault();
    public bool HasSpawnableHelicopters(int wantedLevel) => Vehicles.Any(x => x.IsHelicopter && x.CanCurrentlySpawn(wantedLevel));
}