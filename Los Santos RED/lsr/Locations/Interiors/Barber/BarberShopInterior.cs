using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class BarberShopInterior : Interior
{
    protected BarberShop barberShop;
    public BarberShop BarberShop => barberShop;
    public List<SalonInteract> HaircutInteracts { get; set; } = new List<SalonInteract>();
    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(HaircutInteracts);
            return AllInteracts;
        }
    }
    public BarberShopInterior()
    {

    }
    public BarberShopInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetBarberShop(BarberShop newBarber)
    {
        barberShop = newBarber;
        foreach (SalonInteract test in HaircutInteracts)
        {
            test.BarberShop = newBarber;
            if (newBarber != null)
            {
                EntryPoint.WriteToConsole($"SET BARBER SHOP FOR {newBarber.Name}");
            }
            //test.TotalCash = RandomItems.GetRandomNumberInt(Bank.DrawerCashMin, Bank.DrawerCashMax);
        }
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.BarberShopInteriors.Add(this);
    }

}

