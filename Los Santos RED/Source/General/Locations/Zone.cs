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
    public Zone(string _GameName, string _TextName, County _ZoneCounty)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
    }
    public string DispatchUnitName { get; set; }
    public string InternalGameName { get; set; }
    public string DisplayName { get; set; }
    public County ZoneCounty { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;

}