using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable()]
public class Zone
{

    public Zone()
    {

    }
    public Zone(string _GameName, string _TextName, string _CountyID,string state,bool isSpecificLocation, eLocationEconomy economy, eLocationType type)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        CountyID = _CountyID;
        State = state;
        IsSpecificLocation = isSpecificLocation;
        Economy = economy;
        Type = type;     
    }
    public Zone(string _GameName, string _TextName, string _CountyID, Vector2[] boundaries, string state, bool isSpecificLocation, eLocationEconomy economy, eLocationType type)
    {
        InternalGameName = _GameName;
        Boundaries = boundaries;
        DisplayName = _TextName;
        CountyID = _CountyID;
        State = state;
        IsSpecificLocation = isSpecificLocation;
        Economy = economy;
        Type = type;
    }


    public string InternalGameName { get; set; }
    public string DisplayName { get; set; }
    public string CountyID { get; set; }
    public string State { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;
    public bool IsSpecificLocation { get; set; } = false;
    public eLocationEconomy Economy { get; set; } = eLocationEconomy.Middle;
    public eLocationType Type { get; set; } = eLocationType.Rural;
    public Vector2[] Boundaries { get; set; }
    public string FullDisplayName(ICounties counties)
    {
        GameCounty myCounty = counties.GetCounty(CountyID);
        if(myCounty != null)
        {
            return DisplayName + ", " + myCounty.CountyName;
        }
        return DisplayName;
    }
    [XmlIgnore]
    public string AssignedLEAgencyInitials { get; set; }
    [XmlIgnore]
    public string AssignedSecondLEAgencyInitials { get; set; }
    [XmlIgnore]
    public string AssignedGangInitials { get; set; }
    [XmlIgnore]
    public List<Gang> Gangs { get; set; }
    [XmlIgnore]
    public List<Agency> Agencies { get; set; }
    public List<ZoneMenu> DealerMenus { get; set; }
    public List<ZoneMenu> CustomerMenus { get; set; }

    public bool IsLowPop => Type == eLocationType.Rural || Type == eLocationType.Wilderness;
    public void StoreData(IGangTerritories gangTerritories, IJurisdictions jurisdictions)
    {
        Gangs = new List<Gang>();
        List<Gang> GangStuff = gangTerritories.GetGangs(InternalGameName, 0);
        if (GangStuff != null)
        {
            Gangs.AddRange(GangStuff);
        }
        Agencies = new List<Agency>();
        List<Agency> LEAgency = jurisdictions.GetAgencies(InternalGameName, 0, ResponseType.LawEnforcement);
        if (LEAgency != null)
        {
            Agencies.AddRange(LEAgency);
        }
        List<Agency> EMSAgencies = jurisdictions.GetAgencies(InternalGameName, 0, ResponseType.EMS);
        if (EMSAgencies != null)
        {
            Agencies.AddRange(EMSAgencies);
        }
        List<Agency> FireAgencies = jurisdictions.GetAgencies(InternalGameName, 0, ResponseType.Fire);
        if (FireAgencies != null)
        {
            Agencies.AddRange(FireAgencies);
        }
        AssignedLEAgencyInitials = jurisdictions.GetMainAgency(InternalGameName, ResponseType.LawEnforcement)?.ColorInitials;
        Gang mainGang = gangTerritories.GetMainGang(InternalGameName);
        if (mainGang != null)
        {
            AssignedGangInitials = mainGang.ColorInitials;
        }
        else
        {
            AssignedGangInitials = "";
        }
        Agency secondaryAgency = jurisdictions.GetNthAgency(InternalGameName, ResponseType.LawEnforcement, 2);
        if (secondaryAgency != null)
        {
            AssignedSecondLEAgencyInitials = secondaryAgency.ColorInitials;
        }
        else
        {
            AssignedSecondLEAgencyInitials = "";
        }
    }

    private string GetRandomMenuID(List<ZoneMenu> toCheck)
    {
        if (toCheck == null || !toCheck.Any())
        {
            return null;
        }
        List<ZoneMenu> ToPickFrom = toCheck.ToList();
        int Total = ToPickFrom.Sum(x => x.SelectChance);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (ZoneMenu zoneMenu in ToPickFrom)
        {
            int SpawnChance = zoneMenu.SelectChance;
            if (RandomPick < SpawnChance)
            {
                return zoneMenu.MenuGroupID;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom()?.MenuGroupID;
        }
        return null;
    }
    private string GetRandomMenuIDByEconomy(bool isDealer)
    {
        if (Economy == eLocationEconomy.Poor)
        {
            if(isDealer)
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.ToiletCleanerDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.MarijuanaDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.CokeDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.CrackDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.HeroinDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.SPANKDealerMenuGroupID, 15),
                });
            }
            else
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineCustomerMenuGroupID, 10),
                    new ZoneMenu(StaticStrings.ToiletCleanerCustomerMenuGroupID, 10),
                    new ZoneMenu(StaticStrings.MarijuanaCustomerMenuGroupID, 10),
                    new ZoneMenu(StaticStrings.CokeCustomerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.CrackCustomerMenuGroupID, 10),
                    new ZoneMenu(StaticStrings.HeroinCustomerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.SPANKCustomerMenuGroupID, 10),
                });
            }
        }
        else if (Economy == eLocationEconomy.Middle)
        {
            if (isDealer)
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.ToiletCleanerDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.MarijuanaDealerMenuGroupID, 40),
                    new ZoneMenu(StaticStrings.CokeDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.CrackDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.HeroinDealerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.SPANKDealerMenuGroupID, 5),
                });
            }
            else
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineCustomerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.ToiletCleanerCustomerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.MarijuanaCustomerMenuGroupID, 40),
                    new ZoneMenu(StaticStrings.CokeCustomerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.CrackCustomerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.HeroinCustomerMenuGroupID, 5),
                    new ZoneMenu(StaticStrings.SPANKCustomerMenuGroupID, 5),
                });
            }
        }
        else if (Economy == eLocationEconomy.Rich)
        {
            if (isDealer)
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineDealerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.ToiletCleanerDealerMenuGroupID, 1),
                    new ZoneMenu(StaticStrings.MarijuanaDealerMenuGroupID, 30),
                    new ZoneMenu(StaticStrings.CokeDealerMenuGroupID, 20),
                    new ZoneMenu(StaticStrings.CrackDealerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.HeroinDealerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.SPANKDealerMenuGroupID, 1),
                });
            }
            else
            {
                return GetRandomMenuID(new List<ZoneMenu>() {
                    new ZoneMenu(StaticStrings.MethamphetamineCustomerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.ToiletCleanerCustomerMenuGroupID, 1),
                    new ZoneMenu(StaticStrings.MarijuanaCustomerMenuGroupID, 30),
                    new ZoneMenu(StaticStrings.CokeCustomerMenuGroupID, 20),
                    new ZoneMenu(StaticStrings.CrackCustomerMenuGroupID, 2),
                    new ZoneMenu(StaticStrings.HeroinCustomerMenuGroupID, 15),
                    new ZoneMenu(StaticStrings.SPANKCustomerMenuGroupID, 1),
                });
            }
        }
        return null;
    }

    public ShopMenu GetIllicitMenu(ISettingsProvideable Settings, IShopMenus ShopMenus)
    {
        float dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentageMiddleZones;
        float customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageMiddleZones;
        if(Economy == eLocationEconomy.Rich)
        {
            dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentageRichZones;
            customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageRichZones;
        }
        else if (Economy == eLocationEconomy.Poor)
        {
            dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentagePoorZones;
            customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentagePoorZones;
        }
        if (RandomItems.RandomPercent(dealerPercentage))
        {
            string dealerMenu = GetRandomMenuID(DealerMenus);
            if(string.IsNullOrEmpty(dealerMenu))
            {
                dealerMenu = GetRandomMenuIDByEconomy(true);
            }
            return ShopMenus.GetWeightedRandomMenuFromGroup(dealerMenu);
        }
        else if (RandomItems.RandomPercent(customerPercentage))
        {
            string customerMenu = GetRandomMenuID(CustomerMenus);
            if (string.IsNullOrEmpty(customerMenu))
            {
                customerMenu = GetRandomMenuIDByEconomy(false);  
            }
            return ShopMenus.GetWeightedRandomMenuFromGroup(customerMenu);
        }
        return null;
    }


}