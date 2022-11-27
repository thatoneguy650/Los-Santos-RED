using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
[Serializable]
public class DispatchableVehicle
{
    public string DebugName { get; set; }
    public string ModelName { get; set; }
    public string RequiredPedGroup { get; set; } = "";
    public int MinOccupants { get; set; } = 1;
    public int MaxOccupants { get; set; } = 2;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 6;
    public int RequiredPrimaryColorID { get; set; } = -1;
    public int RequiredSecondaryColorID { get; set; } = -1;
    public List<int> RequiredLiveries { get; set; } = new List<int>();
    public List<DispatchableVehicleExtra> VehicleExtras { get; set; } = new List<DispatchableVehicleExtra>();
    public bool IsBoat => NativeFunction.Natives.IS_THIS_MODEL_A_BOAT<bool>(Game.GetHashKey(ModelName));
    public bool IsCar => NativeFunction.Natives.IS_THIS_MODEL_A_CAR<bool>(Game.GetHashKey(ModelName));
    public bool IsHelicopter => NativeFunction.Natives.IS_THIS_MODEL_A_HELI<bool>(Game.GetHashKey(ModelName));
    public bool IsMotorcycle => NativeFunction.Natives.IS_THIS_MODEL_A_BIKE<bool>(Game.GetHashKey(ModelName));
    public bool CanCurrentlySpawn(int WantedLevel) => CurrentSpawnChance(WantedLevel) > 0;
    public int CurrentSpawnChance(int WantedLevel)
    {
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return AmbientSpawnChance;
        }
    }
    public DispatchableVehicle()
    {
    }
    public DispatchableVehicle(string modelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = modelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }

}