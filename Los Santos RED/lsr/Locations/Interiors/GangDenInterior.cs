using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class GangDenInterior : Interior
{
    protected GangDen gangDen;
    public GangDen GangDen => gangDen;
    public List<RestInteract> RestInteracts { get; set; } = new List<RestInteract>();
    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(RestInteracts);
            return AllInteracts;
        }
    }
    public GangDenInterior()
    {

    }
    public GangDenInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetGangDen(GangDen newGangDen)
    {
        gangDen = newGangDen;
        foreach (RestInteract test in RestInteracts)
        {
            test.RestableLocation = newGangDen;
        }
    }
    protected override void LoadDoors(bool isOpen)
    {
        if (isOpen && GangDen != null && GangDen.IsAvailableForPlayer)
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
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach (RestInteract bdi in RestInteracts)
        {
            bdi.AddDistanceOffset(offsetToAdd);
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.GangDenInteriors.Add(this);
    }
}