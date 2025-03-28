using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class BusinessInterior : Interior
{
    protected Business business;
    public Business Business => business;
    public List<RestInteract> RestInteracts { get; set; } = new List<RestInteract>();
    public List<InventoryInteract> InventoryInteracts { get; set; } = new List<InventoryInteract>();
    public List<OutfitInteract> OutfitInteracts { get; set; } = new List<OutfitInteract>();
    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(RestInteracts);
            AllInteracts.AddRange(InventoryInteracts);
            AllInteracts.AddRange(OutfitInteracts);
            return AllInteracts;
        }
    }
    public BusinessInterior()
    {

    }
    public BusinessInterior(int iD, string name) : base(iD, name)
    {

    }
    protected override void LoadDoors(bool isOpen)
    {
        if (Business != null && Business.IsOwned)
        {
            foreach (InteriorDoor door in Doors)
            {
                door.UnLockDoor();
            }
        }
        else
        {
            foreach (InteriorDoor door in Doors.Where(x => x.LockWhenClosed))
            {
                door.LockDoor();
            }
        }
    }
    public void SetBusiness(Business newBusiness)
    {
        business = newBusiness;
        foreach (InventoryInteract test in InventoryInteracts)
        {
            test.InventoryableLocation = newBusiness;
        }
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.BusinessInteriors.Add(this);
    }
}

