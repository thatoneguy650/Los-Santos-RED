using System.Collections.Generic;
using System.Xml.Serialization;

public class ClothingShopInterior : Interior
{
    protected ClothingShop clothingShop;
    public ClothingShop ClothingShop => clothingShop;
    public List<TryOnInteract> TryOnInteracts { get; set; } = new List<TryOnInteract>();
    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(TryOnInteracts);
            return AllInteracts;
        }
    }
    public ClothingShopInterior()
    {

    }
    public ClothingShopInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetClothingShop(ClothingShop newClothingShop)
    {
        clothingShop = newClothingShop;
        foreach (TryOnInteract test in TryOnInteracts)
        {
            test.ClothingShop = newClothingShop;
            if (newClothingShop != null)
            {
                EntryPoint.WriteToConsole($"SET CLOTHING SHOP FOR {newClothingShop.Name}");
            }
            //test.TotalCash = RandomItems.GetRandomNumberInt(Bank.DrawerCashMin, Bank.DrawerCashMax);
        }
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.ClothingShopInteriors.Add(this);
    }

}

