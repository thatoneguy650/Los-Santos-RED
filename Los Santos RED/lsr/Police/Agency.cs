using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Agency
{
    public Agency()
    {

    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, string _AgencyColorString, Classification _AgencyClassification, List<DispatchableOfficer> _CopModels, List<DispatchableVehicle> _Vehicles, string _LicensePlatePrefix, List<IssuableWeapon> sideArms, List<IssuableWeapon> longGuns)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColorString = _AgencyColorString;
        Vehicles = _Vehicles;
        AgencyClassification = _AgencyClassification;
        LicensePlatePrefix = _LicensePlatePrefix;
        SideArms = sideArms;
        LongGuns = longGuns;
    }
    public Classification AgencyClassification { get; set; } = Classification.Other;
    public Color AgencyColor
    {
        get
        {
            return Color.FromName(AgencyColorString);
        }
    }
    public string AgencyColorString { get; set; } = "White";
    public bool CanCheckTrafficViolations
    {
        get
        {
            if (AgencyClassification == Classification.Police || AgencyClassification == Classification.Federal || AgencyClassification == Classification.Sheriff || AgencyClassification == Classification.State)
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
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
    }
    public string ColorPrefix { get; set; } = "~s~";
    public List<DispatchableOfficer> CopModels { get; set; } = new List<DispatchableOfficer>();
    public string FullName { get; set; } = "Unknown";
    public bool HasMotorcycles
    {
        get
        {
            return Vehicles.Any(x => x.IsMotorcycle);
        }
    }
    public string Initials { get; set; } = "UNK";
    public string LicensePlatePrefix { get; set; } = "";
    public uint MaxWantedLevelSpawn { get; set; } = 5;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public int SpawnLimit { get; set; } = 99;
    public bool SpawnsOnHighway { get; set; } = false;
    public List<DispatchableVehicle> Vehicles { get; set; } = new List<DispatchableVehicle>();
    public List<IssuableWeapon> SideArms { get; set; } = new List<IssuableWeapon>();
    public List<IssuableWeapon> LongGuns { get; set; } = new List<IssuableWeapon>();
    public bool CanSpawn(int WantedLevel)
    {
        if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public DispatchableOfficer GetRandomPed(int WantedLevel, List<string> RequiredModels)
    {
        if (CopModels == null || !CopModels.Any())
            return null;

        List<DispatchableOfficer> ToPickFrom = CopModels.Where(x => WantedLevel >= x.MinWantedLevelSpawn && WantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if (RequiredModels != null && RequiredModels.Any())
        {
            ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(WantedLevel));
        //Mod.Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchableOfficer Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance(WantedLevel);
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public DispatchableVehicle GetRandomVehicle(int WantedLevel, bool includeHelicopters, bool includeBoats)
    {
        if (Vehicles != null && Vehicles.Any())
        {
            List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(WantedLevel) && !x.IsHelicopter && !x.IsBoat).ToList();
            if (includeBoats)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(WantedLevel) && x.IsBoat).ToList());
            }
            if (includeHelicopters)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(WantedLevel) && x.IsHelicopter).ToList());
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(WantedLevel));
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (DispatchableVehicle Vehicle in ToPickFrom)
            {
                int SpawnChance = Vehicle.CurrentSpawnChance(WantedLevel);
                if (RandomPick < SpawnChance)
                {
                    return Vehicle;
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public IssuableWeapon GetRandomWeapon(bool isSidearm)
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
        weaponToIssue.SetIssued(Game.GetHashKey(weaponToIssue.ModelName), null);
        return weaponToIssue;
    }
    public DispatchableVehicle GetVehicleInfo(Vehicle CopCar)
    {
        return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
    }
    public bool HasSpawnableHelicopters(int WantedLevel)
    {
        return Vehicles.Any(x => x.IsHelicopter && x.CanCurrentlySpawn(WantedLevel));
    }
}