using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_Petrovic_LPP
{
    private DispatchablePeople DispatchablePeople;

    public DispatchablePeople_Petrovic_LPP(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.PetrovicPeds_LPP = new List<DispatchablePerson>() { };
        LCPPeds();  
    }
    private void LCPPeds()
    {

        DispatchablePerson PetrovicLCPPMale1 = new DispatchablePerson("IG_Russian_Goon_01", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "PetrovicMale1" };
        DispatchablePeople.PetrovicPeds_LPP.Add(PetrovicLCPPMale1);
        DispatchablePerson PetrovicLCPPMale2 = new DispatchablePerson("IG_Russian_Goon_02", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "PetrovicMale2" };
        DispatchablePeople.PetrovicPeds_LPP.Add(PetrovicLCPPMale2);
        DispatchablePerson PetrovicLCPPMale3 = new DispatchablePerson("IG_Russian_Goon_03", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "PetrovicMale3" };
        DispatchablePeople.PetrovicPeds_LPP.Add(PetrovicLCPPMale3);
        DispatchablePerson PetrovicLCPPMale4 = new DispatchablePerson("IG_Russian_Goon_04", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "PetrovicMale4" };
        DispatchablePeople.PetrovicPeds_LPP.Add(PetrovicLCPPMale4);
        DispatchablePerson PetrovicLCPPMale5 = new DispatchablePerson("IG_Russian_Goon_05", 30, 30, 5, 10, 400, 600, 0, 1) { DebugName = "PetrovicMale5" };
        DispatchablePeople.PetrovicPeds_LPP.Add(PetrovicLCPPMale5);

    }
}

