using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_NorthHolland_LPP
{
    private DispatchablePeople DispatchablePeople;

    public DispatchablePeople_NorthHolland_LPP(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.NorthHollandPeds_LPP = new List<DispatchablePerson>() { };
        LCPPeds();
    }
    private void LCPPeds()
    {
        DispatchablePerson HustlerMale1 = new DispatchablePerson("G_M_Y_Hustler_01", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale1" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale1);
        DispatchablePerson HustlerMale2 = new DispatchablePerson("G_M_Y_Hustler_02", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale2" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale2);
        DispatchablePerson HustlerMale3 = new DispatchablePerson("G_M_Y_Hustler_03", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale3" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale3);
        DispatchablePerson HustlerMale4 = new DispatchablePerson("G_M_Y_Hustler_04", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale4" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale4);
        DispatchablePerson HustlerMale5 = new DispatchablePerson("G_M_Y_Hustler_05", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale5" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale5);
        DispatchablePerson HustlerMale6 = new DispatchablePerson("G_M_Y_FatHustler_01", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "HustlerMale6" };
        DispatchablePeople.NorthHollandPeds_LPP.Add(HustlerMale6);

    }
}

