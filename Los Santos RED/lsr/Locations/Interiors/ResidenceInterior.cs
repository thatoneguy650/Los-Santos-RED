using Rage;
using Rage.Native;
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
    public List<TrophyInteract> TrophyInteracts { get; set; } = new List<TrophyInteract>();

    [XmlIgnore]
    public Dictionary<int, int> PlacedTrophies { get; set; } = new Dictionary<int, int>();

    [XmlIgnore]
    public Dictionary<int, Rage.Object> SpawnedTrophies { get; set; } = new Dictionary<int, Rage.Object>();

    public List<TrophyPlacement> SavedPlacedTrophies { get; set; } = new List<TrophyPlacement>();
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
            AllInteracts.AddRange(TrophyInteracts);
            return AllInteracts;
        }
    }
    public ResidenceInterior()
    {
       
    }
    public ResidenceInterior(int iD, string name) : base(iD, name)
    {

    }
    protected override void LoadDoors(bool isOpen, bool reLockForcedEntry)
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
        foreach (TrophyInteract test in TrophyInteracts)
        {
            test.TrophyableLocation = newResidence;
        }
    }
    public override void AddLocation(PossibleInteriors interiorList)
    {
        interiorList.ResidenceInteriors.Add(this);
    }
    public new void Load(bool isOpen)
    {
        base.Load(isOpen);
        SpawnPlacedTrophies();
    }

    private void SpawnPlacedTrophies()
    {
        if (PlacedTrophies == null || PlacedTrophies.Count == 0)
        {
            return;
        }
        if (!TrophyInteract.CabinetDatasByInterior.TryGetValue(InternalID, out CabinetData cabinetData) || cabinetData == null)
        {
            return;
        }
        if (cabinetData.Slots == null || cabinetData.Slots.Count == 0)
        {
            return;
        }
        float trophyHeading = cabinetData.TrophyHeading;
        foreach (TrophySlot trophySlot in cabinetData.Slots)
        {
            if (!PlacedTrophies.TryGetValue(trophySlot.SlotID, out int trophyID) || trophyID == 0)
            {
                continue;
            }
            if (!TrophyInteract.TrophyRegistry.TryGetValue(trophyID, out TrophyDefinition trophyDefinition))
            {
                continue;
            }
            uint modelHash = Game.GetHashKey(trophyDefinition.ModelName);
            NativeFunction.Natives.REQUEST_MODEL(modelHash);
            while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
            {
                GameFiber.Yield();
            }
            Rage.Object trophyObject = new Rage.Object(modelHash, trophySlot.Position, trophyHeading);
            if (trophyObject.Exists())
            {
                int entityHandle = (int)trophyObject.Handle.Value;
                NativeFunction.Natives.SET_ENTITY_COLLISION(entityHandle, false, false);
                NativeFunction.Natives.FREEZE_ENTITY_POSITION(entityHandle, true);
                trophyObject.IsPersistent = true;
                SpawnedTrophies[trophySlot.SlotID] = trophyObject;
                SpawnedProps.Add(trophyObject);
            }
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(modelHash);
        }
    }
}

