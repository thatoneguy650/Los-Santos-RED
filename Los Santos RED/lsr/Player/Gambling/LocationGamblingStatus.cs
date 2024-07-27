using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LocationGamblingStatus
{
    public LocationGamblingStatus()
    {

    }
    public LocationGamblingStatus(GamblingDen gameLocation, List<GamblingIncident> gamblingWinIncidents)
    {
        GameLocation = gameLocation;
        GamblingIncidents = gamblingWinIncidents;
    }
    public GamblingDen GameLocation { get; set; }
    public List<GamblingIncident> GamblingIncidents { get; set; }
    public int TotalWon => GamblingIncidents == null ? 0 : GamblingIncidents.Sum(x => x.AmountWon);
    public void UpdateTime(ITimeReportable time)
    {
        GamblingIncidents.RemoveAll(x => DateTime.Compare(time.CurrentDateTime, x.DateTimeWon.AddHours(GameLocation.WinLimitResetHours)) > 0);
    }
}

