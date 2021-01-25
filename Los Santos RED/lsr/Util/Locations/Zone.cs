using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Zone
{

    public Zone()
    {

    }
    public Zone(string _GameName, string _TextName, County _ZoneCounty, string state)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
        State = state;
    }
    public Zone(string _GameName, string _TextName, County _ZoneCounty,Vector2[] boundaries, string state)
    {
        InternalGameName = _GameName;
        Boundaries = boundaries;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
        State = state;
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
            string CountyName = "San Andreas";
            if (ZoneCounty == County.BlaineCounty)
                CountyName = "Blaine County";
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
            return DisplayName + ", " + CountyName;
        }
    }
    public string AssignedAgencyInitials { get; set; }

}