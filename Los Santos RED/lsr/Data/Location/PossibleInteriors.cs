using System;
using System.Collections.Generic;


public class PossibleInteriors
{


    public PossibleInteriors()
    {

    }
    public List<Interior> GeneralInteriors { get; private set; } = new List<Interior>();
    public List<ResidenceInterior> ResidenceInteriors { get; private set; } = new List<ResidenceInterior>();
    public List<GangDenInterior> GangDenInteriors { get; private set; } = new List<GangDenInterior>();
    public List<BankInterior> BankInteriors { get; private set; } = new List<BankInterior>();
    public List<BarberShopInterior> BarberShopInteriors { get; private set; } = new List<BarberShopInterior>();
    public List<Interior> AllInteriors()
    {
        List<Interior> Allinteriors = new List<Interior>();
        Allinteriors.AddRange(GeneralInteriors);
        Allinteriors.AddRange(ResidenceInteriors);
        Allinteriors.AddRange(GangDenInteriors);
        Allinteriors.AddRange(BankInteriors);
        Allinteriors.AddRange(BarberShopInteriors);
        return Allinteriors;
    }
}

