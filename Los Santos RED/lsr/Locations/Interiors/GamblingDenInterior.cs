using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

public class GamblingDenInterior : Interior
{
    protected GamblingDen gamblingDen;
    public GamblingDen GamblingDen => gamblingDen;
    public List<RestInteract> RestInteracts { get; set; } = new List<RestInteract>();


    public List<GamblingInteract> GamblingInteracts { get; set; } = new List<GamblingInteract>();

    [XmlIgnore]
    public override List<InteriorInteract> AllInteractPoints
    {
        get
        {
            List<InteriorInteract> AllInteracts = new List<InteriorInteract>();
            AllInteracts.AddRange(InteractPoints);
            AllInteracts.AddRange(RestInteracts);
            AllInteracts.AddRange(GamblingInteracts);
            return AllInteracts;
        }
    }
    public GamblingDenInterior()
    {

    }
    public GamblingDenInterior(int iD, string name) : base(iD, name)
    {

    }
    public void SetGamblingDen(GamblingDen newGamblingDen)
    {
        gamblingDen = newGamblingDen;
        foreach (RestInteract test in RestInteracts)
        {
            test.RestableLocation = newGamblingDen;
        }
        foreach (GamblingInteract test in GamblingInteracts)
        {
            test.GamblingDen = newGamblingDen;
        }
    }
    protected override void LoadDoors(bool isOpen, bool reLockForcedEntry)
    {
        if (isOpen && GamblingDen != null && GamblingDen.IsAvailableForPlayer())
        {
            foreach (InteriorDoor door in Doors)
            {
                door.UnLockDoor();
            }
        }
        else
        {
            if (reLockForcedEntry)
            {
                foreach (InteriorDoor door in Doors.Where(x => x.LockWhenClosed))
                {
                    door.LockDoor();
                }
            }
            else
            {
                foreach (InteriorDoor door in Doors.Where(x => x.LockWhenClosed && !x.HasBeenForcedOpen))
                {
                    door.LockDoor();
                }
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
        interiorList.GamblingDenInteriors.Add(this);
    }
}