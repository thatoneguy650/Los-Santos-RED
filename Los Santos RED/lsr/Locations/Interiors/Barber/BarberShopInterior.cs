using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class BarberShopInterior : Interior
{
    protected BarberShop barberShop;
    public BarberShop BarberShop => barberShop;
    public List<HaircutInteract> HaircutInteracts { get; set; } = new List<HaircutInteract>();
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
        foreach (HaircutInteract test in HaircutInteracts)
        {
            test.BarberShop = newBarber;
            if (newBarber != null)
            {
                EntryPoint.WriteToConsole($"SET BARBER SHOP FOR {newBarber.Name}");
            }
            //test.TotalCash = RandomItems.GetRandomNumberInt(Bank.DrawerCashMin, Bank.DrawerCashMax);
        }
    }
}

