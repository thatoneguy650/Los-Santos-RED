using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class ResidenceInterior : Interior
{
    protected Residence residence;
    public Residence Residence => residence;
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
    public ResidenceInterior()
    {
       
    }
    public ResidenceInterior(int iD, string name) : base(iD, name)
    {

    }
    protected override void LoadDoors(bool isOpen)
    {
        if (isOpen && Residence != null && Residence.IsOwnedOrRented)
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
    public void SetResidence(Residence newResidence)
    {
        residence = newResidence;
        foreach (RestInteract test in RestInteracts)
        {
            test.RestableLocation = newResidence;
        }
        foreach (InventoryInteract test in InventoryInteracts)
        {
            test.InventoryableLocation = newResidence;
        }
        foreach (OutfitInteract test in OutfitInteracts)
        {
            test.OutfitableLocation = newResidence;
        }
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.ResidenceInteriors.Add(this);
    }
}

