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
    public string ColorPrefix { get; set; } = "~s~";
    public string Initials { get; set; } = "UNK";
    public string FullName { get; set; } = "Unknown";
    public List<PedestrianInformation> CopModels { get; set; } = new List<PedestrianInformation>();
    public List<VehicleInformation> Vehicles { get; set; } = new List<VehicleInformation>();
    public string AgencyColorString { get; set; } = "White";
    public Classification AgencyClassification { get; set; } = Classification.Other;
    public string LicensePlatePrefix { get; set; } = "";
    public bool SpawnsOnHighway { get; set; } = false;
    public bool CanSpawnAnywhere { get; set; } = false;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public uint MaxWantedLevelSpawn { get; set; } = 5;
    public int SpawnLimit { get; set; } = 99;
    public List<IssuedWeapon> IssuedWeapons { get; set; } = new List<IssuedWeapon>();
    public bool CanSpawn
    {
        get
        {
            if (Mod.Player.WantedLevel >= MinWantedLevelSpawn && Mod.Player.WantedLevel <= MaxWantedLevelSpawn)
            {
                if (Mod.PedManager.Cops.Count(x => x.AssignedAgency == this) < SpawnLimit)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
    public bool HasMotorcycles
    {
        get
        {
            return Vehicles.Any(x => x.IsMotorcycle);
        }
    }
    public bool HasSpawnableHelicopters
    {
        get
        {
            return Vehicles.Any(x => x.IsHelicopter && x.CanCurrentlySpawn);
        }
    }
    public Color AgencyColor
    {
        get
        {
            return Color.FromName(AgencyColorString);
        }
    }
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
    }
    public bool CanCheckTrafficViolations
    {
        get
        {
            if (AgencyClassification == Classification.Police || AgencyClassification == Classification.Federal || AgencyClassification == Classification.Sheriff || AgencyClassification == Classification.State)
                return true;
            else
                return false;
        }
    }

    public VehicleInformation GetVehicleInfo(Vehicle CopCar)
    {
        return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
    }
    public VehicleInformation GetRandomVehicle()
    {
        if (Vehicles == null || !Vehicles.Any())
            return null;

        List<VehicleInformation> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn).ToList();
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        // Debugging.WriteToLog("GetRandomVehicle", string.Format("Total Chance {0}, Items {1}", Total, string.Join(",",ToPickFrom.Select( x => x.ModelName + " " + x.CanCurrentlySpawn + "  " + x.CurrentSpawnChance))));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (VehicleInformation Vehicle in ToPickFrom)
        {
            int SpawnChance = Vehicle.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Vehicle;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public PedestrianInformation GetRandomPed(List<string> RequiredModels)
    {
        if (CopModels == null || !CopModels.Any())
            return null;

        List<PedestrianInformation> ToPickFrom = CopModels.Where(x => Mod.Player.WantedLevel >= x.MinWantedLevelSpawn && Mod.Player.WantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if (RequiredModels != null && RequiredModels.Any())
        {
            ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        //Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (PedestrianInformation Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public Agency()
    {

    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, string _AgencyColorString, Classification _AgencyClassification, List<PedestrianInformation> _CopModels, List<VehicleInformation> _Vehicles, string _LicensePlatePrefix, List<IssuedWeapon> _IssuedWeapons)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColorString = _AgencyColorString;
        Vehicles = _Vehicles;
        AgencyClassification = _AgencyClassification;
        LicensePlatePrefix = _LicensePlatePrefix;
        IssuedWeapons = _IssuedWeapons;
    }

}