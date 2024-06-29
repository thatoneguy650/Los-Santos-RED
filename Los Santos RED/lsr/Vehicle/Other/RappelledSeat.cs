using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RappelledSeat
{
    public uint GameTimeLastRappelled { get; private set; }
    public int SeatRappelledFrom { get; private set; }
    public PedExt PedExt { get; private set; }
    public RappelledSeat()
    {
    }
    public RappelledSeat(PedExt pedExt)
    {

        PedExt = pedExt;
    }

    public void AddRappelled(uint gameTimeLastRappelled, int seatRappelledFrom)
    {
        GameTimeLastRappelled = gameTimeLastRappelled;
        SeatRappelledFrom = seatRappelledFrom;
    }


}

