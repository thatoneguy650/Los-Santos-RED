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
    public Zone(string _GameName, string _TextName, County _ZoneCounty,string state,bool isSpecificLocation, eLocationEconomy economy, eLocationType type)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
        State = state;
        IsSpecificLocation = isSpecificLocation;
        Economy = economy;
        Type = type;     
    }
    public Zone(string _GameName, string _TextName, County _ZoneCounty,Vector2[] boundaries, string state, bool isSpecificLocation, eLocationEconomy economy, eLocationType type)
    {
        InternalGameName = _GameName;
        Boundaries = boundaries;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
        State = state;
        IsSpecificLocation = isSpecificLocation;
        Economy = economy;
        Type = type;
    }
    public string DispatchUnitName { get; set; }
    public string InternalGameName { get; set; }
    public string DisplayName { get; set; }
    public County ZoneCounty { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;
    public Vector2[] Boundaries { get; set; }
    public string State { get; set; }
    public string FullDisplayName//move this somewhere else....
    {
        get
        {
            string CountyName = ZoneCounty.ToString();
            if (ZoneCounty == County.BlaineCounty)
                CountyName = "Blaine County";
            else if (ZoneCounty == County.VenturaCounty)
                CountyName = "Ventura County";
            else if (ZoneCounty == County.MajesticCounty)
                CountyName = "Majestic County";
            else if (ZoneCounty == County.CityOfLosSantos)
                CountyName = "City of Los Santos";
            else if (ZoneCounty == County.LosSantosCounty)
                CountyName = "Los Santos County";
            else if (ZoneCounty == County.Crook)
                CountyName = "Crook County";
            else if (ZoneCounty == County.NorthYankton)
                CountyName = "North Yankton";
            else if (ZoneCounty == County.Vice)
                CountyName = "Vice County";
            else if (ZoneCounty == County.Alderney)
                CountyName = "Alderney";
            else if (ZoneCounty == County.LibertyCity)
                CountyName = "Liberty City";
            return DisplayName + ", " + CountyName;
        }
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


    public bool IsLowPop => Type == eLocationType.Rural || Type == eLocationType.Wilderness;
    public bool IsSpecificLocation { get; set; } = false;
    public eLocationEconomy Economy { get; set; } = eLocationEconomy.Middle;
    public eLocationType Type { get; set; } = eLocationType.Rural;

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



}