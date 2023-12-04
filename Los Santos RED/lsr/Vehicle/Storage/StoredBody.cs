using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RAGENativeUI.Elements.UIMenuStatsPanel;


public class StoredBody
{
    private ISettingsProvideable Settings;
    private IVehicleSeatAndDoorLookup VehicleSeatDoorData;
    private IEntityProvideable World;
    private bool PedCanBeTasked;
    private bool PedCanBeAmbientTasked;
    private VehicleItem VehicleItem;
    private readonly string TrunkAnimationDictionaryName = "timetable@floyd@cryingonbed@base";
    private readonly string TrunkAnimationName = "base";

    private readonly string AliveSeatAnimationDictionaryName = "random@crash_rescue@car_death@std_car";//"veh @std@ps@enter_exit";
    private readonly string AliveSeatAnimationName = "loop";//"dead_fall_out";
    public bool IsAttachedToVehicle { get; set; } = true;
    public StoredBody(PedExt pedExt, VehicleDoorSeatData vehicleDoorSeatData, VehicleExt vehicleExt, ISettingsProvideable settings, IEntityProvideable world)
    {
        PedExt = pedExt;
        VehicleDoorSeatData = vehicleDoorSeatData;
        VehicleExt = vehicleExt;
        Settings = settings;
        World = world;
    }
    public PedExt PedExt { get; private set; }
    public VehicleExt VehicleExt { get; private set; }
    public VehicleDoorSeatData VehicleDoorSeatData { get; private set; }
    public bool WasEjected { get; private set; }
    public bool Load(bool withFade)
    {
        if (VehicleDoorSeatData == null)
        {
            return false;
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        if (withFade)
        {
            VehicleExt.OpenDoorLoose(VehicleDoorSeatData.DoorID, true);
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        if(withFade && Settings.SettingsManager.PedLoadingSettings.FadeOut && !Game.IsScreenFadedOut)
        {
            Game.FadeScreenOut(500, true);
        }
        SetupPed();
        SetupVehicle();
        bool Loaded;
        if (VehicleDoorSeatData.SeatBone == "boot")
        {
            Loaded = LoadInTrunk();
        }
        else
        {
            Loaded = LoadIntoSeat();
        }
        if(!Loaded) 
        {
            ResetPed();
        }
        CleanupPed();
        if (withFade && Settings.SettingsManager.PedLoadingSettings.FadeOut)
        {
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
        }
        return Loaded;
    }

    private void SetupVehicle()
    {
        VehicleItem = World?.ModDataFileManager.ModItems.GetVehicle(VehicleExt.Vehicle.Model.Hash);
    }

    public bool OnVehicleCrashed()
    {
        if (VehicleDoorSeatData == null || VehicleDoorSeatData.SeatBone != "boot" || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        EntryPoint.WriteToConsole("EJECTING BODY FROM TRUNK");
        VehicleExt.OpenDoor(VehicleDoorSeatData.DoorID, false);
        PedExt.Pedestrian.Detach();
        ResetPed();
        VehicleExt.VehicleBodyManager.OnEjectedBody();
        PedExt.IsLoadedInTrunk = false;
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);
        NativeFunction.Natives.SET_PED_TO_RAGDOLL(PedExt.Pedestrian, -1, -1, 0, false, false, false);
        WasEjected = true;
        return true;
    }
    private void ResetPed()
    {
        if (!PedExt.Pedestrian.Exists())
        {
            return;
        }
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        else
        {
            PedExt.IsUnconscious = true;
        }
    }
    private void CleanupPed()
    {
        if (PedCanBeTasked)
        {
            PedExt.CanBeTasked = true;
        }
        if (PedCanBeAmbientTasked)
        {
            PedExt.CanBeAmbientTasked = true;
        }
    }
    private bool LoadIntoSeat()
    {
        if(!AnimationDictionary.RequestAnimationDictionayResult(AliveSeatAnimationDictionaryName))
        { 
            return false; 
        }
        bool isLoaded = SetPedIntoSeat();
        return isLoaded;
    }
    private bool SetPedIntoSeat()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        int seat = VehicleDoorSeatData.SeatID;
        if(!VehicleExt.Vehicle.IsSeatFree(seat))
        {
            return false;
        }
        PedExt.Pedestrian.WarpIntoVehicle(VehicleExt.Vehicle, seat); 
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        else
        {
            PlaySeatAnimation();
        }
        IsAttachedToVehicle = true;
        return true;
    }
    private bool LoadInTrunk()
    {
        if (!AnimationDictionary.RequestAnimationDictionayResult(TrunkAnimationDictionaryName))
        {
            return false;
        }
        bool isLoaded = DetermineAttachType();
        return isLoaded;
    }
    private bool DetermineAttachType()
    {
        if(VehicleItem?.OverrideCannotLoadBodiesInRear == true)
        {
            EntryPoint.WriteToConsole("STORED BODY VEHICLE ITEM OverrideCannotLoadBodiesInRear IS TRUE");
            return false;
        }
        else if(VehicleItem?.OverrideLoadBodiesInBed == true)
        {
            EntryPoint.WriteToConsole("STORED BODY VEHICLE ITEM OverrideLoadBodiesInBed IS TRUE");
            return PlaceInBed();
        }
        else if(VehicleExt.VehicleClass == VehicleClass.Van)
        {
            return PlaceInBed();
        }
        else if (!(VehicleExt.VehicleClass == VehicleClass.Motorcycle || VehicleExt.VehicleClass == VehicleClass.Industrial || VehicleExt.VehicleClass == VehicleClass.Utility || VehicleExt.VehicleClass == VehicleClass.Cycle || VehicleExt.VehicleClass == VehicleClass.Boat || VehicleExt.VehicleClass == VehicleClass.Helicopter || VehicleExt.VehicleClass == VehicleClass.Plane || VehicleExt.VehicleClass == VehicleClass.Rail))
        {
            AttachToTrunk();
            if(!IsAttachedToVehicle)
            {
                return false;
            }
            return PlayTrunkAnimation();
        }
        return false;
    }
    private bool PlaceInBed()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists() || !VehicleExt.Vehicle.HasBone("boot"))
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }

        float OffsetX = Settings.SettingsManager.PedLoadingSettings.DefaultBedLoadXOffset;
        float OffsetY = Settings.SettingsManager.PedLoadingSettings.DefaultBedLoadYOffset;
        float OffsetZ = Settings.SettingsManager.PedLoadingSettings.DefaultBedLoadZOffset;


        if (VehicleItem != null && VehicleItem.BedLoadOffsetOverride != Vector3.Zero)
        {
            OffsetX = VehicleItem.BedLoadOffsetOverride.X;
            OffsetY = VehicleItem.BedLoadOffsetOverride.Y;
            OffsetZ = VehicleItem.BedLoadOffsetOverride.Z;
        }


        float halfLength = VehicleExt.Vehicle.Model.Dimensions.Y / 2.0f;
        halfLength += OffsetY;
        Vector3 almostFinal = VehicleExt.Vehicle.GetOffsetPositionFront(-1.0f * halfLength);
        Vector3 BedPositon = new Vector3(almostFinal.X + OffsetX, almostFinal.Y, almostFinal.Z + OffsetZ);
        PedExt.Pedestrian.Position = BedPositon;

        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        else
        {
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(PedExt.Pedestrian, -1, -1, 0, false, false, false);
        }
        IsAttachedToVehicle = false;
        return true;
    }
    private bool PlayTrunkAnimation()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(PedExt.Pedestrian, TrunkAnimationDictionaryName, TrunkAnimationName, 1000.0f, -1000.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_FORCE_START | eAnimationFlags.AF_NOT_INTERRUPTABLE), 0, false, false, false);
        NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(PedExt.Pedestrian, TrunkAnimationDictionaryName, TrunkAnimationName, 1.0f);
        return true;
    }
    private bool PlaySeatAnimation()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        PedExt.Pedestrian.BlockPermanentEvents = true;
        NativeFunction.Natives.TASK_PLAY_ANIM(PedExt.Pedestrian, AliveSeatAnimationDictionaryName, AliveSeatAnimationName, 1000.0f, -1000.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
        NativeFunction.Natives.SET_ANIM_RATE(PedExt.Pedestrian, 0.0f,2,false);
        return true;
    }
    private void AttachToTrunk()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }



        float OffsetX = Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachXOffset;
        float OffsetY = Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachYOffset;
        float OffsetZ = Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachZOffset;


        if (VehicleItem != null && VehicleItem.OverrideTrunkAttachment && VehicleItem.TrunkAttachOffsetOverride != Vector3.Zero)
        {
            OffsetX = VehicleItem.TrunkAttachOffsetOverride.X;
            OffsetY = VehicleItem.TrunkAttachOffsetOverride.Y;
            OffsetZ = VehicleItem.TrunkAttachOffsetOverride.Z;
        }




        float halfLength = VehicleExt.Vehicle.Model.Dimensions.Y / 2.0f;
        halfLength += OffsetY;
        float YOffset = -1.0f * halfLength;
        float ZOffset = OffsetZ;
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(PedExt.Pedestrian, VehicleExt.Vehicle, Settings.SettingsManager.PedLoadingSettings.BoneIndex,
            OffsetX,
            YOffset,
            ZOffset,
            Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachXRotation,
            Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachRotation,
            Settings.SettingsManager.PedLoadingSettings.DefaultTrunkAttachZRotation,
            false, false, false, Settings.SettingsManager.PedLoadingSettings.UseBasicAttachIfPed, Settings.SettingsManager.PedLoadingSettings.Euler, Settings.SettingsManager.PedLoadingSettings.OffsetIsRelative, false);

        IsAttachedToVehicle = true;
        GameFiber.Wait(100);
    }
    private void SetupPed()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedCanBeTasked = PedExt.CanBeTasked;
        PedCanBeAmbientTasked = PedExt.CanBeAmbientTasked;
        PedExt.CanBeTasked = false;
        PedExt.CanBeAmbientTasked = false;

        PedExt.Pedestrian.Detach();

        PedExt.CurrentHealthState.ResurrectPed();
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.Pedestrian.KeepTasks = true;


        NativeFunction.Natives.DISABLE_PED_PAIN_AUDIO(PedExt.Pedestrian, true);

    }
    public bool Unload()
    {
        //Move to Unload Position and open the door if it exists?
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        //VehicleExt.OpenDoor(VehicleDoorSeatData.DoorID, true);
        VehicleExt.OpenDoorLoose(VehicleDoorSeatData.DoorID, true);
        if (Settings.SettingsManager.PedLoadingSettings.FadeOut)
        {
            Game.FadeScreenOut(500, true);
        }
        PedExt.Pedestrian.Detach();
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);

        Vector3 finalPos = VehicleDoorSeatData.GetDoorOffset(VehicleExt.Vehicle, Settings);
        if(finalPos == Vector3.Zero)
        {
            PedExt.Pedestrian.Position = Game.LocalPlayer.Character.Position;
        }
        else
        {
            PedExt.Pedestrian.Position = finalPos;
        }
        PedExt.IsLoadedInTrunk = false;
        if (PedExt.IsUnconscious)
        { 
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(PedExt.Pedestrian, -1, -1, 0, false, false, false);
            NativeFunction.Natives.DISABLE_PED_PAIN_AUDIO(PedExt.Pedestrian, false);
        }
        if (Settings.SettingsManager.PedLoadingSettings.FadeOut)
        {
            GameFiber.Sleep(1000);
            Game.FadeScreenIn(500, true);
        }
        return true;
    }
    public bool IsValid()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"REMOVING INVALID STORED BODY PED DOESNT EXIST");
            return false;
        }
        else if (VehicleDoorSeatData == null)
        {
            EntryPoint.WriteToConsole($"REMOVING INVALID STORED BODY NO DATA");
            return false;
        }
        else if (VehicleDoorSeatData.SeatID >= -1 && (!PedExt.Pedestrian.CurrentVehicle.Exists() || PedExt.Pedestrian.SeatIndex != VehicleDoorSeatData.SeatID))
        {
            EntryPoint.WriteToConsole($"REMOVING INVALID STORED BODY CUrrent:{PedExt.Pedestrian.SeatIndex} Stored:{VehicleDoorSeatData.SeatID}");
            return false;
        }
        return true;
    }

    public void ReAttach()
    {
        if (VehicleDoorSeatData == null)
        {
            return;
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        SetupPed();
        SetupVehicle();
        bool Loaded;
        if (VehicleDoorSeatData.SeatBone == "boot")
        {
            Loaded = LoadInTrunk();
        }
        else
        {
            Loaded = LoadIntoSeat();
        }
        if (!Loaded)
        {
            ResetPed();
        }
        CleanupPed();
    }
}

