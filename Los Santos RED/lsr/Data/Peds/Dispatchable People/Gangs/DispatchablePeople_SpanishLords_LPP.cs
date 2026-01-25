using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_SpanishLords_LPP
{
    private DispatchablePeople DispatchablePeople;

    public DispatchablePeople_SpanishLords_LPP(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.SpanishLordsPeds_LPP = new List<DispatchablePerson>() { };
        LCPPeds();
    }
    private void LCPPeds()
    {

        DispatchablePerson SpanishLordsLCPPMale1 = new DispatchablePerson("G_M_Y_PR", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "SpanishMale1" };
        DispatchablePeople.SpanishLordsPeds_LPP.Add(SpanishLordsLCPPMale1);

    }
}

