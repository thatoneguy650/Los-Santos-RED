using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

[Serializable()]
public class Organization : IPlatePrefixable, IGeneratesDispatchables
{
    public Organization()
    {
    }
    public Organization(string _ColorPrefix, string _ID, string _shortName, string _FullName, string _AgencyColorString, string _DispatchablePeropleGroupID, string _DispatchableVehicleGroupID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string groupName)
    {
        ColorPrefix = _ColorPrefix;
        ID = _ID;
        FullName = _FullName;
        ShortName = _shortName;
        LessLethalWeaponsID = meleeWeaponsID;
        PersonnelID = _DispatchablePeropleGroupID;
        ColorString = _AgencyColorString;
        VehiclesID = _DispatchableVehicleGroupID;
        LicensePlatePrefix = _LicensePlatePrefix;
        SideArmsID = sideArmsID;
        LongGunsID = longGunsID;
        MemberName = groupName;
    }
    public string ID { get; set; } = "UNK";
    public string FullName { get; set; } = "Unknown";
    public string ShortName { get; set; } = "Unk";
    public string MemberName { get; set; } = "Employee";
    public string Description { get; set; } = "";
    public string BannerImagePath { get; set; } = "";
    public string ContactName { get; set; }
    public string ColorPrefix { get; set; } = "~s~";
    public string ColorString { get; set; } = "White";
    public string LicensePlatePrefix { get; set; } = "";
    public string HeadDataGroupID { get; set; }
    public string PersonnelID { get; set; }
    public string LessLethalWeaponsID { get; set; }
    public string SideArmsID { get; set; }
    public string LongGunsID { get; set; }
    public string VehiclesID { get; set; }


    [XmlIgnore]
    public List<RandomHeadData> PossibleHeads { get; set; } = new List<RandomHeadData>();
    [XmlIgnore]
    public List<DispatchablePerson> Personnel { get; set; } = new List<DispatchablePerson>();
    [XmlIgnore]
    public List<IssuableWeapon> SideArms { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> LongGuns { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<DispatchableVehicle> Vehicles { get; set; } = new List<DispatchableVehicle>();
    [XmlIgnore]
    public List<IssuableWeapon> LessLethalWeapons { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public PhoneContact PhoneContact { get; set; }
    [XmlIgnore]
    public Texture BannerImage { get; set; }


    public bool HasBannerImage => BannerImagePath != "";
    public string ColorInitials => ColorPrefix + ShortName;
    public Color Color => Color.FromName(ColorString);
    public bool CanSpawn(int wantedLevel) => true;
    public DispatchablePerson GetSpecificPed(Ped ped)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any() || !ped.Exists())
        {
            return null;
        }
        List<DispatchablePerson> ToPickFrom = new List<DispatchablePerson>();
        if (NativeHelper.IsStringHash(ped.Model.Name, out uint modelHash))//is not actualy a model name
        {
            ToPickFrom = Personnel.Where(b => Game.GetHashKey(b.ModelName.ToLower()) == modelHash).ToList();
        }
        if (!ToPickFrom.Any())
        {
            ToPickFrom = Personnel.Where(b => b.ModelName.ToLower() == ped.Model.Name.ToLower()).ToList();
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }

    public DispatchablePerson GetRandomPed(int wantedLevel, string RequiredPedGroup) => GetRandomPed(wantedLevel, RequiredPedGroup, false);
    public DispatchablePerson GetRandomPed(int wantedLevel, string RequiredPedGroup, bool forceAnimal)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any())
            return null;

        List<DispatchablePerson> ToPickFrom = Personnel.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn && x.IsAnimal == forceAnimal).ToList();
        if (RequiredPedGroup != "" && !string.IsNullOrEmpty(RequiredPedGroup))
        {
            ToPickFrom = ToPickFrom.Where(x => x.GroupName == RequiredPedGroup).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchablePerson dispatchablePErson in ToPickFrom)
        {
            int SpawnChance = dispatchablePErson.CurrentSpawnChance(wantedLevel);
            if (RandomPick < SpawnChance)
            {
                return dispatchablePErson;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public DispatchableVehicle GetRandomVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings)
    {
        if (Vehicles == null || !Vehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && !x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
        if (includeBoats)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsBoat).ToList());
        }
        if (includeHelicopters)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsHelicopter).ToList());
        }
        if (includeMotorcycles)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsMotorcycle).ToList());
        }
        if (requiredGroup != "" && !string.IsNullOrEmpty(requiredGroup))
        {
            ToPickFrom = ToPickFrom.Where(x => x.GroupName == requiredGroup).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchableVehicle Vehicle in ToPickFrom)
        {
            int SpawnChance = Vehicle.CurrentSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles);
            if (RandomPick < SpawnChance)
            {
                return Vehicle;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public IssuableWeapon GetRandomWeapon(bool isSidearm, IWeapons weapons)
    {
        List<IssuableWeapon> PossibleWeapons;
        if (isSidearm)
        {
            PossibleWeapons = SideArms;
        }
        else
        {
            PossibleWeapons = LongGuns;
        }
        if (PossibleWeapons != null && PossibleWeapons.Any())
        {
            int Total = PossibleWeapons.Sum(x => x.SpawnChance);
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (IssuableWeapon weapon in PossibleWeapons)
            {
                int SpawnChance = weapon.SpawnChance;
                if (RandomPick < SpawnChance)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(weapon.ModelName);
                    if (WeaponLookup != null)
                    {

                        weapon.SetIssued(Game.GetHashKey(weapon.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                        return weapon;
                    }
                }
                RandomPick -= SpawnChance;
            }
            if (PossibleWeapons.Any())
            {
                return PossibleWeapons.PickRandom();
            }
        }
        return null;
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons)
    {
        List<IssuableWeapon> PossibleWeapons = LessLethalWeapons;
        if (PossibleWeapons != null && PossibleWeapons.Any())
        {
            int Total = PossibleWeapons.Sum(x => x.SpawnChance);
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (IssuableWeapon weapon in PossibleWeapons)
            {
                int SpawnChance = weapon.SpawnChance;
                if (RandomPick < SpawnChance)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(weapon.ModelName);
                    if (WeaponLookup != null)
                    {
                        weapon.SetIssued(Game.GetHashKey(weapon.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                        return weapon;
                    }
                }
                RandomPick -= SpawnChance;
            }
            if (PossibleWeapons.Any())
            {
                return PossibleWeapons.PickRandom();
            }
        }
        return null;
    }
    public DispatchableVehicle GetVehicleInfo(Vehicle vehicle) => Vehicles.Where(x => x.ModelName.ToLower() == vehicle.Model.Name.ToLower()).FirstOrDefault();
    public override string ToString()
    {
        return ID;
    } 
}