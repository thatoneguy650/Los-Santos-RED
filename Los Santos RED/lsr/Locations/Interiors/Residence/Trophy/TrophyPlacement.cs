using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TrophyPlacement
{
    private bool isSpawned;
    private Rage.Object SpawnedTrophy;
    public TrophyPlacement()
    {

    }
    public TrophyPlacement(int slotID, string trophyModelName)
    {
        SlotID = slotID;
        TrophyModelName = trophyModelName;
    }
    public int SlotID { get; set; }
    public string TrophyModelName { get; set; }
    public bool IsSpawned => isSpawned;
    public void SpawnTrophy(CabinetData cabinetData)
    {
        TrophySlot selectedSlot = cabinetData.Slots.Where(x => x.SlotID == SlotID).FirstOrDefault();
        if(selectedSlot == null)
        {
            return;
        }
        if (SpawnedTrophy.Exists())
        {
            SpawnedTrophy.Delete();
        }
        SpawnedTrophy = new Rage.Object(TrophyModelName, selectedSlot.Position, selectedSlot.Rotation);// 239.2449f);
        if (SpawnedTrophy.Exists())
        {
            NativeFunction.Natives.SET_ENTITY_COLLISION(SpawnedTrophy, false, false);
            NativeFunction.Natives.FREEZE_ENTITY_POSITION(SpawnedTrophy, true);
            SpawnedTrophy.IsPersistent = true;
            isSpawned = true;
        }
        EntryPoint.WriteToConsole($"TROPHY SPAWNED RAN  IsSpawned{isSpawned}");
        NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(TrophyModelName));
        
    }
    public void SpawnPreviewTrophy(CabinetData cabinetData, string modelName)
    {
        TrophySlot selectedSlot = cabinetData.Slots.Where(x => x.SlotID == SlotID).FirstOrDefault();
        if (selectedSlot == null)
        {
            return;
        }
        if (SpawnedTrophy.Exists())
        {
            SpawnedTrophy.Delete();
        }
        SpawnedTrophy = new Rage.Object(modelName, selectedSlot.Position, selectedSlot.Rotation);// 239.2449f);
        if (SpawnedTrophy.Exists())
        {
            NativeFunction.Natives.SET_ENTITY_COLLISION(SpawnedTrophy, false, false);
            NativeFunction.Natives.FREEZE_ENTITY_POSITION(SpawnedTrophy, true);
            SpawnedTrophy.IsPersistent = true;
            isSpawned = true;
        }
        EntryPoint.WriteToConsole($"TROPHY SPAWNED RAN  IsSpawned{isSpawned}");
        NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(modelName));

    }
    public void DespawnTrophy()
    {
        if(SpawnedTrophy.Exists())
        {
            SpawnedTrophy.Delete();
        }
        isSpawned = false;
    }
}

