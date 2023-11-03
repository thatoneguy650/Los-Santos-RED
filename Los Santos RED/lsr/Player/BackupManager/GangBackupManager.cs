using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangBackupManager
{
    public bool RequestBackup(Gang gang)
    {
        if (gang == null)
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, NO TAXI FIRM");
            return false;
        }
        if (ActiveRides.Any(x => x.RequestedFirm.ID == taxiFirm.ID))
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, ALREADY ACTIVE RIDE");
            return false;
        }
        TaxiRide taxiRide = new TaxiRide(World, Player, taxiFirm, Player.Position);
        taxiRide.Setup();
        if (!taxiRide.IsActive)
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, NOT ACTIVE");
            return false;
        }
        ActiveRides.Add(taxiRide);
        EntryPoint.WriteToConsole("TaxiManager RequestService Active Ride Added");
        return true;
    }
}

