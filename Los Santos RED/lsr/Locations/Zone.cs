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

    [XmlIgnore]
    public ShopMenuGroupContainer DealerMenus { get; set; }
    [XmlIgnore]
    public ShopMenuGroupContainer CustomerMenus { get; set; }

    public string DealerMenuContainerID { get; set; }
    public string CustomerMenuContainerID { get; set; }

    public bool IsLowPop => Type == eLocationType.Rural || Type == eLocationType.Wilderness;
    public void StoreData(IGangTerritories gangTerritories, IJurisdictions jurisdictions, IShopMenus shopMenus)
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
        DealerMenus = shopMenus.GetSpecificGroupContainer(DealerMenuContainerID);
        CustomerMenus = shopMenus.GetSpecificGroupContainer(CustomerMenuContainerID);
    }
    public ShopMenu GetIllicitMenu(ISettingsProvideable Settings, IShopMenus ShopMenus)
    {
        float dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentageMiddleZones;
        float customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageMiddleZones;
        string DealerMenuContainerID = StaticStrings.MiddleAreaDrugDealerMenuGroupID;
        string CustomerMenuContainerID = StaticStrings.MiddleAreaDrugCustomerMenuGroupID;

        if (Economy == eLocationEconomy.Rich)
        {
            dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentageRichZones;
            customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageRichZones;
            DealerMenuContainerID = StaticStrings.RichAreaDrugDealerMenuGroupID;
            CustomerMenuContainerID = StaticStrings.RichAreaDrugCustomerMenuGroupID;
        }
        else if (Economy == eLocationEconomy.Poor)
        {
            dealerPercentage = Settings.SettingsManager.CivilianSettings.DrugDealerPercentagePoorZones;
            customerPercentage = Settings.SettingsManager.CivilianSettings.DrugCustomerPercentagePoorZones;
            DealerMenuContainerID = StaticStrings.PoorAreaDrugDealerMenuGroupID;
            CustomerMenuContainerID = StaticStrings.PoorAreaDrugCustomerMenuGroupID;
        }
        if (RandomItems.RandomPercent(dealerPercentage))
        {
            if(DealerMenus != null)
            {
                string lookingForID = DealerMenus.GetRandomWeightedShopMenuGroupID();
                //EntryPoint.WriteToConsole($"ZONE {DisplayName} GETTING ZONE SPECIFIC DEALER MENU ID: {lookingForID}");
                return ShopMenus.GetWeightedRandomMenuFromGroup(lookingForID);
            }
            else
            {
               // EntryPoint.WriteToConsole($"ZONE {DisplayName} GETTING GENERIC DEALER MENU DealerMenuContainerID: {DealerMenuContainerID}");
                return ShopMenus.GetWeightedRandomMenuFromContainer(DealerMenuContainerID);//  GetRandomMenuIDByEconomy(true);
            }
        }
        else if (RandomItems.RandomPercent(customerPercentage))
        {
            if (CustomerMenus != null)
            {
                string lookingForID = CustomerMenus.GetRandomWeightedShopMenuGroupID();
               // EntryPoint.WriteToConsole($"ZONE {DisplayName} GETTING ZONE SPECIFIC CUSTOMER MENU ID: {lookingForID}");
                return ShopMenus.GetWeightedRandomMenuFromGroup(lookingForID);
            }
            else
            {
                //EntryPoint.WriteToConsole($"ZONE {DisplayName} GETTING GENERIC CUSTOMER MENU CustomerMenuContainerID: {CustomerMenuContainerID}");
                return ShopMenus.GetWeightedRandomMenuFromContainer(CustomerMenuContainerID);
            }
        }
        return null;
    }


}