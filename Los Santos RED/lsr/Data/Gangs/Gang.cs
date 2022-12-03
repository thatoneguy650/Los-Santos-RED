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
public class Gang
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
    public Color Color => Color.FromName(ColorString);
    public string ColorString { get; set; } = "White";
    public bool CanSpawnAnywhere { get; set; } = false;
    public string ColorInitials
    {
        get
        {
            return ColorPrefix + ShortName;
        }
    }
    public string ColorPrefix { get; set; } = "~s~";
    public string FullName { get; set; } = "Unknown";
    public string ShortName { get; set; } = "Unk";
    public bool HasMotorcycles => Vehicles.Any(x => x.IsMotorcycle);
    public string ID { get; set; } = "UNK";
    public string LicensePlatePrefix { get; set; } = "";
    public uint MaxWantedLevelSpawn { get; set; } = 6;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public int SpawnLimit { get; set; } = 10;
    public bool SpawnsOnHighway { get; set; } = false;
    public string ContactName { get; set; } = "Unknown";
    public string ContactIcon { get; set; }
    public string DealerMenuGroup { get; set; } = "";
    public string DenName { get; set; } = "Den";
    public uint GameTimeToRecoverAmbientRep { get; set; } = 5000;
    public float PercentageWithLongGuns { get; set; } = 5f;
    public float PercentageWithSidearms { get; set; } = 40f;
    public float PercentageWithMelee { get; set; } = 50f;
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
    public float FightPercentage { get; set; } = 80f;
    public float FightPolicePercentage { get; set; } = 50f;
    public float DrugDealerPercentage { get; set; } = 40f;
    public int AmbientMemberMoneyMin { get; set; } = 500;
    public int AmbientMemberMoneyMax { get; set; } = 1200;


    public int DealerMemberMoneyMin { get; set; } = 1500;
    public int DealerMemberMoneyMax { get; set; } = 5000;


    public float VehicleSpawnPercentage { get; set; } = 60f;
    public int CostToPayoffGangScalar { get; set; } = 5;
    public bool RemoveRepOnWantedInTerritory { get; set; } = true;
    public int RemoveRepoOnWantedInTerritoryScalar { get; set; } = 5;
    public bool AddAmbientRep { get; set; } = true;
    public int MinimumRep { get; set; } = -2000;
    public int MaximumRep { get; set; } = 2000;
    public int StartingRep { get; set; } = 200;
    public int NeutralRepLevel { get; set; } = 0;
    public int FriendlyRepLevel { get; set; } = 500;
    public float PercentageTrustingOfPlayer { get; set; } = 60f;
    public List<string> EnemyGangs = new List<string>();
    public bool IsFedUpWithPlayer { get; set; } = false;

    public int MemberKickUpDays { get; set; } = 7;
    public int MemberKickUpAmount { get; set; } = 2000;
    public int MemberKickUpMissLimit { get; set; } = 3;




    [XmlIgnore]
    public bool HasWantedMembers { get; set; }


    [XmlIgnore]
    public List<RandomHeadData> PossibleHeads { get; set; } = new List<RandomHeadData>();
    public string HeadDataGroupID { get; set; }
    [XmlIgnore]
    public List<DispatchablePerson> Personnel { get; set; } = new List<DispatchablePerson>();
    public string PersonnelID { get; set; }
    [XmlIgnore]
    public List<IssuableWeapon> MeleeWeapons { get; set; } = new List<IssuableWeapon>();
    public string MeleeWeaponsID { get; set; }
    [XmlIgnore]
    public List<IssuableWeapon> SideArms { get; set; } = new List<IssuableWeapon>();
    public string SideArmsID { get; set; }
    [XmlIgnore]
    public List<IssuableWeapon> LongGuns { get; set; } = new List<IssuableWeapon>();
    public string LongGunsID { get; set; }
    [XmlIgnore]
    public List<DispatchableVehicle> Vehicles { get; set; } = new List<DispatchableVehicle>();
    public string VehiclesID { get; set; }
    public string MemberName { get; set; }
    public bool CanSpawn(int wantedLevel) => wantedLevel >= MinWantedLevelSpawn && wantedLevel <= MaxWantedLevelSpawn;
    public DispatchablePerson GetRandomPed(int wantedLevel, string RequiredPedGroup)// List<string> RequiredModels)
    {
        if (Personnel == null || !Personnel.Any())
            return null;

        List<DispatchablePerson> ToPickFrom = Personnel.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if (RequiredPedGroup != "")
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