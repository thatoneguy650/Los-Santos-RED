using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VehicleInformation
{
    public string ModelName { get; set; }
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinOccupants { get; set; } = 1;
    public int MaxOccupants { get; set; } = 2;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 5;
    public List<string> AllowedPedModels { get; set; } = new List<string>();//only ped models can spawn in this, if emptyt any ambient spawn can
    public List<int> Liveries { get; set; } = new List<int>();
    public bool IsCar
    {
        get
        {
            return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Game.GetHashKey(ModelName));
        }
    }
    public bool IsMotorcycle
    {
        get
        {
            return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BIKE", Game.GetHashKey(ModelName));
        }
    }
    public bool IsHelicopter
    {
        get
        {
            return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_HELI", Game.GetHashKey(ModelName));
        }
    }
    public bool IsBoat
    {
        get
        {
            return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BOAT", Game.GetHashKey(ModelName));
        }
    }
    public bool CanSpawnWanted
    {
        get
        {
            if (WantedSpawnChance > 0)
                return true;
            else
                return false;
        }
    }
    public bool CanSpawnAmbient
    {
        get
        {
            if (AmbientSpawnChance > 0)
                return true;
            else
                return false;
        }
    }
    public bool CanCurrentlySpawn
    {
        get
        {
            if (IsHelicopter && Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.IsHelicopter) >= Mod.DataMart.Settings.SettingsManager.Police.HelicopterLimit)
            {
                return false;
            }
            else if (IsBoat && Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.IsBoat) >= Mod.DataMart.Settings.SettingsManager.Police.BoatLimit)
            {
                return false;
            }

            if (Mod.Player.IsWanted)
            {
                if (Mod.Player.WantedLevel >= MinWantedLevelSpawn && Mod.Player.WantedLevel <= MaxWantedLevelSpawn)
                    return CanSpawnWanted;
                else
                    return false;
            }
            else
                return CanSpawnAmbient;
        }
    }
    public int CurrentSpawnChance
    {
        get
        {
            if (!CanCurrentlySpawn)
                return 0;
            if (Mod.Player.IsWanted)
            {
                if (Mod.Player.WantedLevel >= MinWantedLevelSpawn && Mod.Player.WantedLevel <= MaxWantedLevelSpawn)
                    return WantedSpawnChance;
                else
                    return 0;
            }
            else
                return AmbientSpawnChance;
        }
    }
    public VehicleInformation()
    {

    }
    public VehicleInformation(string modelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = modelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
}