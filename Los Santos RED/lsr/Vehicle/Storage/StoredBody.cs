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
    private bool PedCanBeTasked;
    private bool PedCanBeAmbientTasked;

    public StoredBody(PedExt pedExt, string storedBone, VehicleExt vehicleExt, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        StoredBone = storedBone;
        VehicleExt = vehicleExt;
        Settings = settings;
    }

    public PedExt PedExt { get; private set; }
    public VehicleExt VehicleExt { get; private set; }
    public string StoredBone { get; private set; }
    public bool Load()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        OpenDoor();
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        if(Settings.SettingsManager.DebugSettings.Drag_FadeOut)
        {
            Game.FadeScreenOut(500, true);
        }
        SetupPed();
        bool Loaded = false;
        if (StoredBone == "boot")
        {
            SetupTrunkLoad();
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
        if (Settings.SettingsManager.DebugSettings.Drag_FadeOut)
        {
            Game.FadeScreenIn(500, true);
        }
        return Loaded;
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
        bool isLoaded = SetPedIntoSeat();
        CloseDoor();
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
        int seat = GetSeatIDFromSeat();
        EntryPoint.WriteToConsole($"SetPedIntoSeat StoredBone{StoredBone} seatid{seat}");
        if(seat == -1)
        {
            return false;
        }
        PedExt.Pedestrian.WarpIntoVehicle(VehicleExt.Vehicle, seat);
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        return true;
    }

    private bool LoadInTrunk()
    {
        SetupTrunkLoad();
        bool isLoaded = DetermineAttachType();
        CloseDoor();
        return isLoaded;
    }

    private bool DetermineAttachType()
    {
        if(VehicleExt.VehicleClass == VehicleClass.Van)
        {
            return PlaceInBed();
        }
        else if (!(VehicleExt.VehicleClass == VehicleClass.Motorcycle || VehicleExt.VehicleClass == VehicleClass.Industrial || VehicleExt.VehicleClass == VehicleClass.Utility || VehicleExt.VehicleClass == VehicleClass.Cycle || VehicleExt.VehicleClass == VehicleClass.Boat || VehicleExt.VehicleClass == VehicleClass.Helicopter || VehicleExt.VehicleClass == VehicleClass.Plane || VehicleExt.VehicleClass == VehicleClass.Rail))
        {
            AttachToTrunk();
            return PlayTrunkAnimation();
        }
        return false;
    }
    private bool PlaceInBed()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return false;
        }
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        Vector3 BootPosition = VehicleExt.Vehicle.GetBonePosition("boot");
        Vector3 BedPositon = new Vector3(BootPosition.X + Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset, BootPosition.Y + Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset, BootPosition.Z + 0.2f + Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset);
        PedExt.Pedestrian.Position = BedPositon;
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        else
        {
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(PedExt.Pedestrian, -1, -1, 0, false, false, false);
        }
        return true;
    }

    private bool PlayTrunkAnimation()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(PedExt.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2 | 8, 0, false, false, false);
        NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(PedExt.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 1.0f);
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
        bool hasBoot = VehicleExt.Vehicle.HasBone("boot");
        Vector3 BootPosition = VehicleExt.Vehicle.GetBonePosition("boot");
        Vector3 RootPosition = VehicleExt.Vehicle.GetBonePosition(0);
        float YOffset = -1 * BootPosition.DistanceTo2D(RootPosition);
        float ZOffset = BootPosition.Z - RootPosition.Z;
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(PedExt.Pedestrian, VehicleExt.Vehicle, Settings.SettingsManager.DebugSettings.Draw_BoneIndex,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset + YOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset + ZOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyXRotation,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyYRotation,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyZRotation,
            false, false, false, Settings.SettingsManager.DebugSettings.Drag_UseBasicAttachIfPed, Settings.SettingsManager.DebugSettings.Drag_Euler, Settings.SettingsManager.DebugSettings.Drag_OffsetIsRelative, false);
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
        PedExt.CurrentHealthState.ResurrectPed();
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);
        PedExt.Pedestrian.BlockPermanentEvents = true;
        PedExt.Pedestrian.KeepTasks = true;
    }
    private void SetupTrunkLoad()
    {
        AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");
    }

    private void OpenDoor()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        int doorID = GetDoorIDFromSeat();
        if(!VehicleExt.Vehicle.Doors[doorID].IsValid())
        {
            return;
        }
        if (!VehicleExt.Vehicle.Doors[doorID].IsFullyOpen)
        {
            VehicleExt.Vehicle.Doors[doorID].Open(false, false);
            GameFiber.Wait(750);
        }

    }
    private void CloseDoor()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        int doorID = GetDoorIDFromSeat();
        if (!VehicleExt.Vehicle.Doors[doorID].IsValid())
        {
            return;
        }
        VehicleExt.Vehicle.Doors[doorID].Close(false);
    }
    private int GetDoorIDFromSeat()
    {
        if (StoredBone == "boot")
        {
            return 5;
        }
        else if (StoredBone == "seat_dside_f")
        {
            return 0;
        }
        else if (StoredBone == "seat_pside_f")
        {
            return 1;
        }
        else if (StoredBone == "seat_dside_r")
        {
            return 2;
        }
        else if (StoredBone == "seat_pside_r")
        {
            return 3;
        }
        return 5;
    }
    private int GetSeatIDFromSeat()
    {
        if (StoredBone == "boot")
        {
            return -2;
        }
        else if (StoredBone == "seat_dside_f")
        {
            return -1;
        }
        else if (StoredBone == "seat_pside_f")
        {
            return 0;
        }
        else if (StoredBone == "seat_dside_r")
        {
            return 1;
        }
        else if (StoredBone == "seat_pside_r")
        {
            return 2;
        }
        return -2;
    }
    public bool Unload()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return false;
        }
        PedExt.Pedestrian.Detach();
        if (PedExt.IsDead)
        {
            PedExt.Pedestrian.Kill();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(PedExt.Pedestrian);
        PedExt.Pedestrian.Position = Game.LocalPlayer.Character.Position;
        NativeFunction.Natives.SET_PED_TO_RAGDOLL(PedExt.Pedestrian, -1, -1, 0, false, false, false);
        return false;
    }



    //private void LoadBodyInCar()
    //{
    //    EntryPoint.WriteToConsole("LoadBodyInCarStarted");
    //    if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists() || !ClosestVehicle.Vehicle.Doors[5].IsValid())
    //    {
    //        return;
    //    }
    //    if (Settings.SettingsManager.DebugSettings.Drag_FadeOut)
    //    {
    //        Game.FadeScreenOut(500, true);
    //    }
    //    AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");
    //    Ped.Pedestrian.Detach();
    //    Ped.CurrentHealthState.ResurrectPed();
    //    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
    //    NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);
    //    Ped.Pedestrian.BlockPermanentEvents = true;
    //    GameFiber.Sleep(100);
    //    if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
    //    {
    //        return;
    //    }
    //    if (!ClosestVehicle.Vehicle.Doors[5].IsFullyOpen)
    //    {
    //        ClosestVehicle.Vehicle.Doors[5].Open(false, false);
    //        GameFiber.Wait(750);
    //    }
    //    if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
    //    {
    //        return;
    //    }
    //    bool hasBoot = ClosestVehicle.Vehicle.HasBone("boot");
    //    Vector3 BootPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
    //    Vector3 RootPosition = ClosestVehicle.Vehicle.GetBonePosition(0);

    //    float YOffset = -1 * BootPosition.DistanceTo2D(RootPosition);
    //    float ZOffset = BootPosition.Z - RootPosition.Z;
    //    NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, Settings.SettingsManager.DebugSettings.Draw_BoneIndex,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset + YOffset,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset + ZOffset,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyXRotation,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyYRotation,
    //        Settings.SettingsManager.DebugSettings.Draw_LoadBodyZRotation,
    //        false, false, false, Settings.SettingsManager.DebugSettings.Drag_UseBasicAttachIfPed, Settings.SettingsManager.DebugSettings.Drag_Euler, Settings.SettingsManager.DebugSettings.Drag_OffsetIsRelative, false);
    //    IsAttached = false;
    //    GameFiber.Wait(100);
    //    if (Ped.Pedestrian.Exists())
    //    {
    //        EntryPoint.WriteToConsole("ANIMATION RAN");
    //        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2, 0, false, false, false);
    //        NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 1.0f);
    //    }
    //    if (Settings.SettingsManager.DebugSettings.Drag_FadeOut)
    //    {
    //        Game.FadeScreenIn(500, true);
    //    }
    //    if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
    //    {
    //        return;
    //    }
    //    EntryPoint.WriteToConsole("GOT TO CLOSE TRUNK");
    //    GameFiber.Wait(1000);
    //    if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Settings.SettingsManager.DebugSettings.Drag_AutoCloseTrunk)
    //    {
    //        ClosestVehicle.Vehicle.Doors[5].Close(false);
    //    }
    //    bool freezePos = false;
    //}


}

