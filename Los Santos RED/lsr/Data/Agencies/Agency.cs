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
using static DispatchScannerFiles;

[Serializable()]
public class Agency : IPlatePrefixable, IGeneratesDispatchables
{
    private int beatNumber = 0;

    public Agency()
    {
    }
    public Agency(string _ColorPrefix, string _ID, string _shortName, string _FullName, string _AgencyColorString, Classification _AgencyClassification, string _DispatchablePeropleGroupID, string _DispatchableVehicleGroupID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string groupName)
    {
        ColorPrefix = _ColorPrefix;
        ID = _ID;
        FullName = _FullName;
        ShortName = _shortName;
        LessLethalWeaponsID = meleeWeaponsID;
        PersonnelID = _DispatchablePeropleGroupID;
        ColorString = _AgencyColorString;
        VehiclesID = _DispatchableVehicleGroupID;
        Classification = _AgencyClassification;
        LicensePlatePrefix = _LicensePlatePrefix;
        SideArmsID = sideArmsID;
        LongGunsID = longGunsID;
        MemberName = groupName;
    }
    public string ID { get; set; } = "UNK";
    public string FullName { get; set; } = "Unknown";
    public string ShortName { get; set; } = "Unk";
    public string MemberName { get; set; } = "Cop";
    public Classification Classification { get; set; } = Classification.Other;
    public int Division { get; set; } = -1;
    public string ColorPrefix { get; set; } = "~s~";
    public string ColorString { get; set; } = "White";
    public string LicensePlatePrefix { get; set; } = "";
    public int SpawnLimit { get; set; } = 99;
    public bool CanSpawnAnywhere { get; set; } = false;
    public bool SpawnsOnHighway { get; set; } = false;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public uint MaxWantedLevelSpawn { get; set; } = 6;
    public bool CanDispatchOffDuty { get; set; } = true;
    public string HeadDataGroupID { get; set; }
    public string PersonnelID { get; set; }
    public string LessLethalWeaponsID { get; set; }
    public string SideArmsID { get; set; }
    public string LongGunsID { get; set; }
    public string VehiclesID { get; set; }

    public float OffDutyDispatchPercent { get; set; } = 0f;
    public string OffDutyPersonnelID { get; set; } //OffDutyCops
    public string OffDutyVehiclesID { get; set; }//OffDutyCopVehicles

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
    public List<DispatchablePerson> OffDutyPersonnel { get; set; } = new List<DispatchablePerson>();
    [XmlIgnore]
    public List<DispatchableVehicle> OffDutyVehicles { get; set; } = new List<DispatchableVehicle>();



    [XmlIgnore]
    public bool HasDispatchableHelicopters { get; set; } = false;
    [XmlIgnore]
    public bool HasDispatchableBoats { get; set; } = false;




    public string ColorInitials => ColorPrefix + ShortName;
    public Color Color => Color.FromName(ColorString);
    public ResponseType ResponseType => Classification == Classification.EMS ? ResponseType.EMS : Classification == Classification.Fire ? ResponseType.Fire : Classification == Classification.Security ? ResponseType.Security : ResponseType.LawEnforcement;
    public bool CanSpawn(int wantedLevel) => wantedLevel >= MinWantedLevelSpawn && wantedLevel <= MaxWantedLevelSpawn;
    public DispatchablePerson GetSpecificPed(Ped ped)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any() || !ped.Exists())
        {
            return null;
        }
        //uint modelHash;
        //var hex = ped.Model.Name.ToLower();
        //if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
        //{
        //    hex = hex.Substring(2);
        //}
        //bool parsedSuccessfully = uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out modelHash);
        List<DispatchablePerson> ToPickFrom = new List<DispatchablePerson>();
        if (NativeHelper.IsStringHash(ped.Model.Name, out uint modelHash))//is not actualy a model name
        {
            ToPickFrom = Personnel.Where(b => Game.GetHashKey(b.ModelName.ToLower()) == modelHash).ToList();
        }
        if(!ToPickFrom.Any())
        {
            ToPickFrom = Personnel.Where(b => b.ModelName.ToLower() == ped.Model.Name.ToLower()).ToList();
        }     
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public DispatchablePerson GetRandomOffDutyPed(int wantedLevel, string RequiredPedGroup)
    {
        if (OffDutyPersonnel == null || !OffDutyPersonnel.Any())
        {
            EntryPoint.WriteToConsole("OFF DUTY NO PERSONNEL 1!");
            return null;
        }
        List<DispatchablePerson> ToPickFrom = OffDutyPersonnel.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn && x.IsAnimal == false).ToList();

        if(!ToPickFrom.Any())
        {
            EntryPoint.WriteToConsole("OFF DUTY NO PERSONNEL 2!");
        }

        if (RequiredPedGroup != "" && !string.IsNullOrEmpty(RequiredPedGroup))
        {
            ToPickFrom = ToPickFrom.Where(x => x.GroupName == RequiredPedGroup).ToList();
        }


        if (!ToPickFrom.Any())
        {
            EntryPoint.WriteToConsole("OFF DUTY NO PERSONNEL AFTER GROUP!");
        }

        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel));
        int RandomPick = RandomItems.MyRand.Next(wantedLevel, Total);
        foreach (DispatchablePerson Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance(wantedLevel);
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }

    public DispatchableVehicle GetRandomOffDutyVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings)
    {
        if (OffDutyVehicles == null || !OffDutyVehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = OffDutyVehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && !x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
        if (includeBoats)
        {
            ToPickFrom.AddRange(OffDutyVehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsBoat).ToList());
        }
        if (includeHelicopters)
        {
            ToPickFrom.AddRange(OffDutyVehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsHelicopter).ToList());
        }
        if (includeMotorcycles)
        {
            ToPickFrom.AddRange(OffDutyVehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsMotorcycle).ToList());
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
        foreach (DispatchablePerson Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance(wantedLevel);
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            //EntryPoint.WriteToConsole($"RequiredPedGroup: {RequiredPedGroup} TOPICKFROM {string.Join(",",ToPickFrom.ToList())}");
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public DispatchableVehicle GetRandomHeliVehicle(int wantedLevel, string requiredGroup, ISettingsProvideable settings)
    {
        if (Vehicles == null || !Vehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
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
    public DispatchableVehicle GetRandomVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings)
    {
        return GetRandomVehicle(wantedLevel, includeHelicopters, includeBoats, includeMotorcycles, requiredGroup, settings, false);
    }
    public DispatchableVehicle GetRandomVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings, bool forceGroup)
    {
        if(Vehicles == null || !Vehicles.Any())
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
            if (forceGroup)
            {
                //EntryPoint.WriteToConsole($"FORCE GROUP RAN FOR AGENCY {requiredGroup}");

                ToPickFrom = Vehicles.Where(x => x.GroupName == requiredGroup).ToList();
                if(!ToPickFrom.Any(x=> x.CurrentSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) > 0))
                {
                    return ToPickFrom.PickRandom();
                }
            }
            else
            {
                ToPickFrom = ToPickFrom.Where(x => x.GroupName == requiredGroup).ToList();
            }
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
    public DispatchableVehicle GetRandomAdjustedVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings, eSpawnAdjustment eSpawnAdjustment)
    {
        if (Vehicles == null || !Vehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlyAdjustedSpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment) && !x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
        if (includeBoats)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlyAdjustedSpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment) && x.IsBoat).ToList());
        }
        if (includeHelicopters)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlyAdjustedSpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment) && x.IsHelicopter).ToList());
        }
        if (includeMotorcycles)
        {
            ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlyAdjustedSpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment) && x.IsMotorcycle).ToList());
        }
        if (requiredGroup != "" && !string.IsNullOrEmpty(requiredGroup))
        {
            ToPickFrom = ToPickFrom.Where(x => x.GroupName == requiredGroup).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentAdjustedSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchableVehicle Vehicle in ToPickFrom)
        {
            int SpawnChance = Vehicle.CurrentAdjustedSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles, eSpawnAdjustment);
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
    public int GetNextBeatNumber()
    {
        beatNumber++;
        if(beatNumber > 24)
        {
            beatNumber = 1;
        }
        return beatNumber;
    }
    public DispatchableVehicle GetRandomWaterVehicle(int wantedLevel, string requiredGroup, ISettingsProvideable settings)
    {
        if (Vehicles == null || !Vehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && (x.IsBoat || x.IsHelicopter)).ToList();
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
    public DispatchableVehicle GetRandomCanineVehicle(int wantedLevel, string requiredGroup, ISettingsProvideable settings)
    {
        if (Vehicles == null || !Vehicles.Any())
        {
            return null;
        }
        List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles) && x.CaninePossibleSeats != null && x.CaninePossibleSeats.Any()).ToList();
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
    public ShopMenu GenerateMenu(IModItems modItems)
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        //Convert Dispatchable Vehicles to ShopMenu
        if (Vehicles != null)
        {
            foreach (DispatchableVehicle dv in Vehicles)
            {
                VehicleItem vehicleItem = modItems.GetVehicle(dv.ModelName);
                if (vehicleItem == null)
                {
                    continue;
                }
                EntryPoint.WriteToConsole($"ADDED {dv.ModelName} TO MENU");
                MenuItem existingMenuItem = new MenuItem(vehicleItem.Name, 0, -1) { ModItem = vehicleItem };
                existingMenuItem.SetFree();
                menuItems.Add(existingMenuItem);
            }
        }
        //Convert Issuable Weapons to ShopMenu
        List<IssuableWeapon> AllWeapons = new List<IssuableWeapon>();
        if (LessLethalWeapons != null)
        {
            AllWeapons.AddRange(LessLethalWeapons);
        }
        if (SideArms != null)
        {
            AllWeapons.AddRange(SideArms);
        }
        if (LongGuns != null)
        {
            AllWeapons.AddRange(LongGuns);
        }
        foreach (IssuableWeapon issuableWeapon in AllWeapons)
        {
            WeaponItem weaponItem = modItems.GetWeapon(issuableWeapon.ModelName);
            if (weaponItem == null)
            {
                continue;
            }
            MenuItem existingMenuItem = menuItems.FirstOrDefault(x => x.ModItemName == weaponItem.Name);
            if (existingMenuItem == null)
            {
                existingMenuItem = new MenuItem(weaponItem.Name, 0, -1) { SubPrice = 0, ModItem = weaponItem };
                existingMenuItem.SetFree();
                menuItems.Add(existingMenuItem);
            }
            foreach (WeaponComponent stuff in issuableWeapon.Variation?.Components)
            {
                if (!existingMenuItem.Extras.Any(x => x.ExtraName == stuff.Name))
                {
                    existingMenuItem.Extras.Add(new MenuItemExtra(stuff.Name, 0));
                }
            }
            EntryPoint.WriteToConsole($"ADDED {issuableWeapon.ModelName} TO MENU");
        }
        return new ShopMenu(ID + "Menu", ID + "Menu", menuItems);    
    }

    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        LessLethalWeapons = issuableWeapons.GetWeaponData(LessLethalWeaponsID);
        LongGuns = issuableWeapons.GetWeaponData(LongGunsID);
        SideArms = issuableWeapons.GetWeaponData(SideArmsID);
        Personnel = dispatchablePeople.GetPersonData(PersonnelID);
        Vehicles = dispatchableVehicles.GetVehicleData(VehiclesID);
        OffDutyPersonnel = dispatchablePeople.GetPersonData(OffDutyPersonnelID);
        OffDutyVehicles = dispatchableVehicles.GetVehicleData(OffDutyVehiclesID);
        PossibleHeads = heads.GetHeadData(HeadDataGroupID);
        //HasDispatchableBoats = Vehicles != null && Vehicles.Any(x => x.IsBoat);
        //HasDispatchableHelicopters = Vehicles != null && Vehicles.Any(x => x.IsHelicopter);
    }
}