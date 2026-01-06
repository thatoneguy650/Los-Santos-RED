using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Linq;

[Serializable]
public class DisplayPlacement
{
    private bool isSpawned;
    private Rage.Object SpawnedProp;
    private ModItem ModItem;
    public DisplayPlacement()
    {

    }
    public DisplayPlacement(int slotID, string modItemName)
    {
        SlotID = slotID;
        ModItemName = modItemName;
    }
    public int SlotID { get; set; }
    public string ModItemName { get; set; }
    public bool IsSpawned => isSpawned;
    public void SpawnDisplay(CabinetData cabinetData, IModItems modItems)
    {
        if(ModItem == null)
        {
            ModItem = modItems.Get(ModItemName);
        }
        if(ModItem == null)
        {
            return;
        }
        SpawnDisplay(cabinetData, ModItem);        
    }
    public void SpawnDisplay(CabinetData cabinetData, ModItem modItem)
    {
        ModItem = modItem;
        if (ModItem == null || ModItem.ModelItem == null)
        {
            return;
        }
        DisplaySlot selectedSlot = cabinetData.Slots.Where(x => x.SlotID == SlotID).FirstOrDefault();
        if (selectedSlot == null)
        {
            return;
        }
        if (SpawnedProp.Exists())
        {
            SpawnedProp.Delete();
        }
        SpawnedProp = ModItem.SpawnObject(selectedSlot.Position, selectedSlot.Rotation);//SpawnedProp = new Rage.Object(modItem.ModelItemID, selectedSlot.Position, selectedSlot.Rotation);// 239.2449f);
        if (SpawnedProp.Exists())
        {
            NativeFunction.Natives.SET_ENTITY_COLLISION(SpawnedProp, false, false);
            NativeFunction.Natives.FREEZE_ENTITY_POSITION(SpawnedProp, true);
            SpawnedProp.IsPersistent = true;
            isSpawned = true;
        }
        EntryPoint.WriteToConsole($"TROPHY SPAWNED RAN  IsSpawned{isSpawned}");
        NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(modItem.ModelItemID));
    }
    public void DespawnDisplay()
    {
        if(SpawnedProp.Exists())
        {
            SpawnedProp.Delete();
        }
        isSpawned = false;
    }
}

