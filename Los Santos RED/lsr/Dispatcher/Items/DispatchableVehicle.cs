using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[Serializable]
public class DispatchableVehicle
{

    private bool isBoat;
    private bool isHelicopter;
    private bool isMotorcycle;
    private bool isPlane;
    private bool isCar;
    private PlateType FinalPlateType;

    public string DebugName { get; set; }
    public string ModelName { get; set; }
    public string RequiredPedGroup { get; set; } = "";
    public string GroupName { get; set; } = "";

    public int MinOccupants { get; set; } = 1;
    public int MaxOccupants { get; set; } = 2;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 6;



    public int AdditionalSpawnChanceOffRoad { get; set; } = 0;

    public List<int> ForceStayInSeats { get; set; }
    public List<int> CaninePossibleSeats { get; set; }
    public int RequiredPrimaryColorID { get; set; } = -1;
    public int RequiredSecondaryColorID { get; set; } = -1;



    public int RequiredInteriorColorID { get; set; } = -1;
    public int RequiredDashColorID { get; set; } = -1;

    public List<int> RequiredLiveries { get; set; } = new List<int>();
    public List<DispatchableVehicleExtra> VehicleExtras { get; set; } = new List<DispatchableVehicleExtra>();
    public List<DispatchableVehicleMod> VehicleMods { get; set; } = new List<DispatchableVehicleMod>();
    public List<int> OptionalColors { get; set; }
    public float MaxRandomDirtLevel { get; set; } = 5.0f;
    public int ForcedPlateType { get; set; } = -1;
    public VehicleVariation RequiredVariation { get; set; }
    public bool RequiresDLC { get; set; } = false;
    public bool IsBoat => isBoat;// NativeFunction.Natives.IS_THIS_MODEL_A_BOAT<bool>(Game.GetHashKey(ModelName));
    public bool IsCar => isCar;// NativeFunction.Natives.IS_THIS_MODEL_A_CAR<bool>(Game.GetHashKey(ModelName));
    public bool IsHelicopter => isHelicopter;// NativeFunction.Natives.IS_THIS_MODEL_A_HELI<bool>(Game.GetHashKey(ModelName));
    public bool IsPlane => isPlane;// NativeFunction.Natives.IS_THIS_MODEL_A_PLANE<bool>(Game.GetHashKey(ModelName));
    public bool IsMotorcycle => isMotorcycle;// NativeFunction.Natives.IS_THIS_MODEL_A_BIKE<bool>(Game.GetHashKey(ModelName));

    public PlateType OutputFinalPlateType => FinalPlateType;

    public int FirstPassengerIndex { get; set; } = 0;
    public List<SpawnAdjustmentAmount> SpawnAdjustmentAmounts { get; set; }
    public bool RequiredGroupIsDriverOnly { get; set; }
    public bool MatchDashColorToBaseColor { get; set; } = false;
    public bool MatchInteriorColorToBaseColor { get; set; } = false;



    public List<string> RequestedPlateTypes { get; set; }

    public string GetDescription()
    {
        string description = "";

        description += $"DebugName: {DebugName}";
        description += $"~n~ModelName: {ModelName}";
        description += $"~n~RequiredPedGroup: {RequiredPedGroup}";
        description += $"~n~MinOccupants: {MinOccupants} MaxOccupants: {MaxOccupants}";
        description += $"~n~AmbientSpawnChance: {AmbientSpawnChance} WantedSpawnChance: {WantedSpawnChance}";
        description += $"~n~MinWantedLevelSpawn: {MinWantedLevelSpawn} MaxWantedLevelSpawn: {MaxWantedLevelSpawn}";
        description += $"~n~RequiresDLC: {RequiresDLC}";
        return description;
    }
    public void Setup(IPlateTypes plateTypes)
    {
        isBoat = NativeFunction.Natives.IS_THIS_MODEL_A_BOAT<bool>(Game.GetHashKey(ModelName));
        isCar = NativeFunction.Natives.IS_THIS_MODEL_A_CAR<bool>(Game.GetHashKey(ModelName));
        isHelicopter = NativeFunction.Natives.IS_THIS_MODEL_A_HELI<bool>(Game.GetHashKey(ModelName));
        isPlane = NativeFunction.Natives.IS_THIS_MODEL_A_PLANE<bool>(Game.GetHashKey(ModelName));
        isMotorcycle = NativeFunction.Natives.IS_THIS_MODEL_A_BIKE<bool>(Game.GetHashKey(ModelName));
#if DEBUG
        if(isMotorcycle)
        {
            EntryPoint.WriteToConsole($"{DebugName} {ModelName} is motorcycle");
        }
        if (isHelicopter)
        {
            EntryPoint.WriteToConsole($"{DebugName} {ModelName} is heli");
        }
        if (isPlane)
        {
            EntryPoint.WriteToConsole($"{DebugName} {ModelName} is plane");
        }
        if (isBoat)
        {
            EntryPoint.WriteToConsole($"{DebugName} {ModelName} is boat");
        }
        //if (isCar)
        //{
        //    EntryPoint.WriteToConsole($"{DebugName} {ModelName} is car");
        //}
#endif
        if (RequestedPlateTypes != null)
        {
            foreach (string plateTypeName in RequestedPlateTypes)
            {
                FinalPlateType = plateTypes.GetPlateByDescription(plateTypeName);
                if (FinalPlateType != null)
                {
                    break;
                }
            }
        }

    }

    public bool CanCurrentlyAdjustedSpawn(int WantedLevel, bool allowDLC, eSpawnAdjustment eSpawnAdjustment) => CurrentAdjustedSpawnChance(WantedLevel, allowDLC, eSpawnAdjustment) > 0;
    public int CurrentAdjustedSpawnChance(int WantedLevel, bool allowDLC, eSpawnAdjustment eSpawnAdjustment)
    {
        if (RequiresDLC && !allowDLC)
        {
            return 0;
        }
        int adjustmentAmount = 0;
        if (SpawnAdjustmentAmounts != null && SpawnAdjustmentAmounts.Any())
        {
            foreach(SpawnAdjustmentAmount spawnAdjustmentAmount in SpawnAdjustmentAmounts)
            {
                if(eSpawnAdjustment.HasFlag(spawnAdjustmentAmount.eSpawnAdjustment))
                {
                    adjustmentAmount += spawnAdjustmentAmount.PercentAmount;
                   // EntryPoint.WriteToConsole($"CurrentAdjustedSpawnChance eSpawnAdjustment {eSpawnAdjustment}, spawnAdjustmentAmount enum:{spawnAdjustmentAmount.eSpawnAdjustment} value:{spawnAdjustmentAmount.PercentAmount}");
                }
            }
        }
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance + adjustmentAmount;
            }
            else
            {
                return 0 + adjustmentAmount;
            }
        }
        else
        {
            return AmbientSpawnChance + adjustmentAmount;
        }
    }




    public bool CanCurrentlySpawn(int WantedLevel, bool allowDLC) => CurrentSpawnChance(WantedLevel, allowDLC) > 0;
    public int CurrentSpawnChance(int WantedLevel, bool allowDLC)
    {
        if(RequiresDLC && !allowDLC)
        {
            return 0;
        }
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


    public void SetVehicleExtPermanentStats(VehicleExt vehicleExt, bool SetPersistent)
    {
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (SetPersistent)
        {
            vehicleExt.Vehicle.IsPersistent = true;
            EntryPoint.PersistentVehiclesCreated++;
        }
        else
        {
            vehicleExt.Vehicle.IsPersistent = false;
        }
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (RequiredLiveries != null && RequiredLiveries.Any())
        {
            NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", vehicleExt.Vehicle, RequiredLiveries.PickRandom());
        }
        GameFiber.Yield();
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (VehicleExtras != null && VehicleExtras.Any())
        {
            foreach (DispatchableVehicleExtra extra in VehicleExtras.OrderBy(x => x.ExtraID).ThenBy(x => x.IsOn))
            {
                if (NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(vehicleExt.Vehicle, extra.ExtraID))
                {
                    int toSet = extra.IsOn ? 0 : 1;
                    if (RandomItems.RandomPercent(extra.Percentage))
                    {
                        NativeFunction.Natives.SET_VEHICLE_EXTRA(vehicleExt.Vehicle, extra.ExtraID, toSet);
                    }
                }
                GameFiber.Yield();
                if (!vehicleExt.Vehicle.Exists())
                {
                    break;
                }
            }
        }
        GameFiber.Yield();
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (VehicleMods != null && VehicleMods.Any())
        { 
            //EntryPoint.WriteToConsole("SETTING VEHICLE MODS");
            NativeFunction.Natives.SET_VEHICLE_MOD_KIT(vehicleExt.Vehicle, 0);
            foreach (DispatchableVehicleMod dispatchableVehicleMod in VehicleMods)
            {
               // EntryPoint.WriteToConsole($"VEHICLE MODS: ID: {dispatchableVehicleMod.ModID}");
                if (RandomItems.RandomPercent(dispatchableVehicleMod.Percentage) && dispatchableVehicleMod.DispatchableVehicleModValues != null)
                {
                    DispatchableVehicleModValue value = dispatchableVehicleMod.PickValue();//.DispatchableVehicleModValues.PickValue();
                    if (value != null)
                    {
                       // EntryPoint.WriteToConsole($"VEHICLE MODS: ID: {dispatchableVehicleMod.ModID} VALUE: {value.Value}");
                        NativeFunction.Natives.SET_VEHICLE_MOD(vehicleExt.Vehicle, dispatchableVehicleMod.ModID, value.Value, false);
                        GameFiber.Yield();
                        if(!vehicleExt.Vehicle.Exists())
                        {
                            break;
                        }
                    }
                }
            }
        }
        GameFiber.Yield();
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (FinalPlateType == null && ForcedPlateType != -1)
        {
            string plateNumber = NativeHelper.GenerateNewLicensePlateNumber("12ABC345");
            vehicleExt.Vehicle.LicensePlate = plateNumber;
            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(vehicleExt.Vehicle, ForcedPlateType);
        }

        if(FinalPlateType != null)
        {
            string plateNumber = FinalPlateType.GenerateNewLicensePlateNumber();
            vehicleExt.Vehicle.LicensePlate = plateNumber;
            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(vehicleExt.Vehicle, FinalPlateType.Index);
        }

        GameFiber.Yield();
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (OptionalColors != null && OptionalColors.Any())
        {
            int chosenColor = OptionalColors.PickRandom();
            NativeFunction.Natives.SET_VEHICLE_COLOURS(vehicleExt.Vehicle, chosenColor, chosenColor);
            if (MatchInteriorColorToBaseColor)
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(vehicleExt.Vehicle, chosenColor);
            }
            if (MatchDashColorToBaseColor)
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(vehicleExt.Vehicle, chosenColor);
            }
        }
        if (RequiredPrimaryColorID != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_COLOURS(vehicleExt.Vehicle, RequiredPrimaryColorID, RequiredSecondaryColorID == -1 ? RequiredPrimaryColorID : RequiredSecondaryColorID);
        }
        if (RequiredInteriorColorID != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(vehicleExt.Vehicle, RequiredInteriorColorID);
        }
        if (RequiredDashColorID != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(vehicleExt.Vehicle, RequiredDashColorID);
        }
        GameFiber.Yield();
        if (!vehicleExt.Vehicle.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(vehicleExt.Vehicle, RandomItems.GetRandomNumber(0.0f, MaxRandomDirtLevel.Clamp(0.0f,15.0f)));
        RequiredVariation?.Apply(vehicleExt);
        GameFiber.Yield();
    }


    //public void UpgradePerformance(VehicleExt vehicleExt)//should be an inherited class? VehicleExt and CopCar? For now itll stay in here 
    //{
    //    if (vehicleExt.Vehicle.Exists() || vehicleExt.Vehicle.IsHelicopter)
    //    {
    //        return;
    //    }
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD_KIT", vehicleExt.Vehicle, 0);//Required to work
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 11, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 11) - 1, true);//Engine
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 12, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 12) - 1, true);//Brakes
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 13, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 13) - 1, true);//Tranny
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 15, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 15) - 1, true);//Suspension
    //}
    //public void UpdateLivery(VehicleExt vehicleExt,Agency AssignedAgency)
    //{
    //    if (AssignedAgency == null)
    //    {
    //        return;
    //    }
    //    vehicleExt.Vehicle.LicensePlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    //    if (RequiredLiveries == null || !RequiredLiveries.Any())
    //    {
    //        return;
    //    }
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", vehicleExt.Vehicle, RequiredLiveries.PickRandom());
    //}

}