using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;

[Serializable()]
public class Gang : IPlatePrefixable, IGeneratesDispatchables
{
    public Gang()
    {
    }
    public Gang(string _ID, string _FullName, string _ShortName, string _MemberName)
    {
        ID = _ID;
        FullName = _FullName;
        ShortName = _ShortName;
        ContactName = _ShortName;
        ContactIcon = "CHAR_DEFAULT";
        MemberName = _MemberName;
    }
    public Gang(string _ColorPrefix, string _ID, string _FullName, string _ShortName, string _AgencyColorString, string peopleID, string vehiclesID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string _MemberName)
    {
        ColorPrefix = _ColorPrefix;
        ID = _ID;
        FullName = _FullName;
        ShortName = _ShortName;
        PersonnelID = peopleID;
        ColorString = _AgencyColorString;
        VehiclesID = vehiclesID;
        LicensePlatePrefix = _LicensePlatePrefix;
        MeleeWeaponsID = meleeWeaponsID;
        SideArmsID = sideArmsID;
        LongGunsID = longGunsID;
        ContactName = _ShortName;
        ContactIcon = "CHAR_DEFAULT";
        MemberName = _MemberName;
    }
    public Gang(string _ColorPrefix, string _ID, string _FullName, string _ShortName, string _AgencyColorString, string peopleID, string vehiclesID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string _ContactName, string contactIcon, string _MemberName)
    {
        ColorPrefix = _ColorPrefix;
        ID = _ID;
        FullName = _FullName;
        ShortName = _ShortName;
        PersonnelID = peopleID;
        ColorString = _AgencyColorString;
        VehiclesID = vehiclesID;
        LicensePlatePrefix = _LicensePlatePrefix;
        MeleeWeaponsID = meleeWeaponsID;
        SideArmsID = sideArmsID;
        LongGunsID = longGunsID;
        ContactName = _ContactName;
        ContactIcon = contactIcon;
        MemberName = _MemberName;
    }
    public string ID { get; set; } = "UNK";
    public string FullName { get; set; } = "Unknown";
    public string ShortName { get; set; } = "Unk";
    public string MemberName { get; set; } = "Gang Member";
    public string ContactName { get; set; } = "Unknown";
    public string ContactIcon { get; set; }
    public string DenName { get; set; } = "Den";
    public string ColorPrefix { get; set; } = "~s~";
    public string ColorString { get; set; } = "White";
    public string LicensePlatePrefix { get; set; } = "";
    public int SpawnLimit { get; set; } = 10;
    public bool CanSpawnAnywhere { get; set; } = false;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public uint MaxWantedLevelSpawn { get; set; } = 6;
    public string HeadDataGroupID { get; set; }
    public string PersonnelID { get; set; }
    public string MeleeWeaponsID { get; set; }
    public string SideArmsID { get; set; }
    public string LongGunsID { get; set; }
    public string VehiclesID { get; set; }
    public int MinimumRep { get; set; } = -2000;
    public int MaximumRep { get; set; } = 2000;
    public int StartingRep { get; set; } = 200;
    public int NeutralRepLevel { get; set; } = 0;
    public int FriendlyRepLevel { get; set; } = 500;
    public int MemberOfferRepLevel { get; set; } = 2000;
    public int HitSquadRep { get; set; } = -1800;
    public bool AddAmbientRep { get; set; } = true;
    public uint GameTimeToRecoverAmbientRep { get; set; } = 5000;
    public int PickupPaymentMin { get; set; } = 200;
    public int PickupPaymentMax { get; set; } = 1000;
    public int TheftPaymentMin { get; set; } = 1000;
    public int TheftPaymentMax { get; set; } = 5000;
    public int HitPaymentMin { get; set; } = 10000;
    public int HitPaymentMax { get; set; } = 30000;
    public int DeliveryPaymentMin { get; set; } = 1000;
    public int DeliveryPaymentMax { get; set; } = 4000;
    public int WheelmanPaymentMin { get; set; } = 15000;
    public int WheelmanPaymentMax { get; set; } = 35000;
    public float FightPercentage { get; set; } = 90f;
    public float FightPolicePercentage { get; set; } = 60f;
    public float DrugDealerPercentage { get; set; } = 40f;
    public string DealerMenuGroup { get; set; } = "";
    public int AmbientMemberMoneyMin { get; set; } = 500;
    public int AmbientMemberMoneyMax { get; set; } = 1200;
    public int DealerMemberMoneyMin { get; set; } = 1500;
    public int DealerMemberMoneyMax { get; set; } = 5000;
    public int CostToPayoffGangScalar { get; set; } = 5;
    public bool RemoveRepOnWantedInTerritory { get; set; } = true;
    public int RemoveRepOnWantedInTerritoryScalar { get; set; } = 5;
    public float PercentageTrustingOfPlayer { get; set; } = 60f;
    public float PercentageWithLongGuns { get; set; } = 5f;
    public float PercentageWithSidearms { get; set; } = 40f;
    public float PercentageWithMelee { get; set; } = 50f;
    public float VehicleSpawnPercentage { get; set; } = 60f;
    public float PedestrianSpawnPercentageAroundDen { get; set; } = 80f;
    public int MemberKickUpDays { get; set; } = 7;
    public int MemberKickUpAmount { get; set; } = 2000;
    public int MemberKickUpMissLimit { get; set; } = 3;
    public GangClassification GangClassification { get; set; } = GangClassification.Generic;
    public List<string> EnemyGangs { get; set; } = new List<string>();
    [XmlIgnore]
    public List<RandomHeadData> PossibleHeads { get; set; } = new List<RandomHeadData>();
    [XmlIgnore]
    public List<DispatchablePerson> Personnel { get; set; } = new List<DispatchablePerson>(); 
    [XmlIgnore]
    public List<IssuableWeapon> MeleeWeapons { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> SideArms { get; set; } = new List<IssuableWeapon>();  
    [XmlIgnore]
    public List<IssuableWeapon> LongGuns { get; set; } = new List<IssuableWeapon>();  
    [XmlIgnore]
    public List<DispatchableVehicle> Vehicles { get; set; } = new List<DispatchableVehicle>();
    [XmlIgnore]
    public bool IsFedUpWithPlayer { get; set; } = false;
    [XmlIgnore]
    public bool HasWantedMembers { get; set; }
    public Color Color => Color.FromName(ColorString);
    public string ColorInitials => ColorPrefix + ShortName;
    public bool CanSpawn(int wantedLevel) => wantedLevel >= MinWantedLevelSpawn && wantedLevel <= MaxWantedLevelSpawn;
    public DispatchablePerson GetRandomPed(int wantedLevel, string RequiredPedGroup)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any())
            return null;

        List<DispatchablePerson> ToPickFrom = Personnel.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn).ToList();
        //if (RequiredPedGroup != "")
        //{
        //    ToPickFrom = ToPickFrom.Where(x => x.GroupName == RequiredPedGroup).ToList();
        //}

        if (RequiredPedGroup != "" && !string.IsNullOrEmpty(RequiredPedGroup))
        {
            ToPickFrom = ToPickFrom.Where(x => x.GroupName == RequiredPedGroup).ToList();
        }


        //if (RequiredModels != null && RequiredModels.Any())
        //{
        //    ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        //}
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
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public DispatchablePerson GetSpecificPed(Ped ped)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any() || !ped.Exists())
        {
            return null;
        }
        List<DispatchablePerson> ToPickFrom = Personnel.Where(b => b.ModelName.ToLower() == ped.Model.Name.ToLower()).ToList();
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom();
        }
        return null;
    }
    public DispatchableVehicle GetRandomVehicle(int wantedLevel, bool includeHelicopters, bool includeBoats, bool includeMotorcycles, string requiredGroup, ISettingsProvideable settings)
    {
        if (Vehicles != null && Vehicles.Any())
        {
            List<DispatchableVehicle> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch) && !x.IsHelicopter && !x.IsBoat && !x.IsMotorcycle).ToList();
            if (includeBoats)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch) && x.IsBoat).ToList());
            }
            if (includeHelicopters)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch) && x.IsHelicopter).ToList());
            }
            if (includeMotorcycles)
            {
                ToPickFrom.AddRange(Vehicles.Where(x => x.CanCurrentlySpawn(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch) && x.IsMotorcycle).ToList());
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch));
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (DispatchableVehicle Vehicle in ToPickFrom)
            {
                int SpawnChance = Vehicle.CurrentSpawnChance(wantedLevel, settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesToDispatch);
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
        List<IssuableWeapon> PossibleWeapons = MeleeWeapons;
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
    //public IssuableWeapon GetRandomWeapon(bool isSidearm, IWeapons weapons)
    //{
    //    IssuableWeapon weaponToIssue;
    //    if (isSidearm)
    //    {
    //        weaponToIssue = SideArms.PickRandom();
    //    }
    //    else
    //    {
    //        weaponToIssue = LongGuns.PickRandom();
    //    }if (weaponToIssue != null)
    //    {
    //        WeaponInformation WeaponLookup = weapons.GetWeapon(weaponToIssue.ModelName);
    //        weaponToIssue.SetIssued(Game.GetHashKey(weaponToIssue.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
    //        return weaponToIssue;
    //    }
    //    return null;
    //}
    //public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons)
    //{
    //    IssuableWeapon weaponToIssue = MeleeWeapons.PickRandom();
    //    if (weaponToIssue != null)
    //    {
    //        WeaponInformation WeaponLookup = weapons.GetWeapon(weaponToIssue.ModelName);
    //        weaponToIssue.SetIssued(Game.GetHashKey(weaponToIssue.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
    //        return weaponToIssue;
    //    }
    //    return null;
    //}
    public DispatchableVehicle GetVehicleInfo(Vehicle vehicle) => Vehicles.Where(x => x.ModelName.ToLower() == vehicle.Model.Name.ToLower()).FirstOrDefault();






    public override string ToString()
    {
        return ShortName.ToString();
    }
}