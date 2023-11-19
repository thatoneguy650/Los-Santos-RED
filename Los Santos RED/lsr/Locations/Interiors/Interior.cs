using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[Serializable()]
public class Interior
{
   // private bool IsMenuInteracting = false;
    private bool IsInside;
    protected IInteractionable Player;
    protected GameLocation InteractableLocation;
    protected ISettingsProvideable Settings;
    private bool IsActive = false;
    private bool IsRunningInteriorUpdate = false;
    public Interior()
    {

    }
    public Interior(int iD, string name, List<string> requestIPLs)
    {
        Name = name;
        LocalID = iD;
        RequestIPLs = requestIPLs;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs)
    {
        Name = name;
        LocalID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs, List<string> interiorSets)
    {
        Name = name;
        LocalID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
        InteriorSets = interiorSets;
    }
    public Interior(int iD, string name, List<string> requestIPLs, List<string> removeIPLs, List<InteriorDoor> interiorDoors)
    {
        Name = name;
        LocalID = iD;
        RequestIPLs = requestIPLs;
        RemoveIPLs = removeIPLs;
        Doors = interiorDoors;
    }
    public Interior(int iD, string name)
    {
        LocalID = iD;
        Name = name;
    }
    [XmlIgnore]
    public int InternalID { get; private set; }
    [XmlIgnore]
    public int DisabledInteriorID { get; private set; }
    [XmlIgnore]
    public bool IsMenuInteracting { get; set; }
    public int LocalID { get; set; }
    public Vector3 InternalInteriorCoordinates { get; set; }
    public string Name { get; set; }
    public bool IsMPOnly { get; set; } = false;
    public bool IsSPOnly { get; set; } = false;
    public bool IsTeleportEntry { get; set; } = false;
    public Vector3 DisabledInteriorCoords { get; set; } = Vector3.Zero;
    public List<InteriorDoor> Doors { get; set; } = new List<InteriorDoor>();
    public List<string> RequestIPLs { get; set; } = new List<string>();
    public List<string> RemoveIPLs { get; set; } = new List<string>();
    public List<string> InteriorSets { get; set; } = new List<string>();
    public Vector3 InteriorEgressPosition { get; set; }
    public float InteriorEgressHeading { get; set; }
    public bool NeedsActivation { get; set; } = false;
    public bool IsRestricted { get; set; } = false;
    public bool IsWeaponRestricted { get; set; } = false;
    public List<InteriorInteract> InteractPoints { get; set; } = new List<InteriorInteract>();


    public virtual void Setup(IInteractionable player, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Settings = settings;
        Player = player;
        if(!IsTeleportEntry)
        {
            InteractableLocation = placesOfInterest.AllLocations().Where(x=> x.InteriorID == LocalID).FirstOrDefault();
        }
        foreach (InteriorInteract interiorInteract in InteractPoints)
        {
            interiorInteract.Setup();
        }
    }

    public void DebugOpenDoors()
    {
        foreach (InteriorDoor door in Doors)
        {
            door.UnLockDoor();
            EntryPoint.WriteToConsole($"INTERIOR: {Name} {door.ModelHash} {door.Position} UNLOCKED");
        }
    }
    public void Load(bool isOpen)
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                if (InternalInteriorCoordinates != Vector3.Zero)
                {
                    InternalID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(InternalInteriorCoordinates.X, InternalInteriorCoordinates.Y, InternalInteriorCoordinates.Z);
                }
                else
                {
                    InternalID = LocalID;
                }
                if(NeedsActivation)
                {
                    NativeFunction.Natives.PIN_INTERIOR_IN_MEMORY(InternalID);
                    NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, true);
                    if(NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                    {
                        NativeFunction.Natives.CAP_INTERIOR(InternalID, false);
                    }
                }
                foreach (string iplName in RequestIPLs)
                {
                    NativeFunction.Natives.REQUEST_IPL(iplName);
                    GameFiber.Yield();
                }
                foreach (string iplName in RemoveIPLs)
                {
                    NativeFunction.Natives.REMOVE_IPL(iplName);
                    GameFiber.Yield();
                }
                foreach (string interiorSet in InteriorSets)
                {
                    NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(InternalID, interiorSet);
                    GameFiber.Yield();
                }
                if (isOpen)
                {
                    foreach (InteriorDoor door in Doors)
                    {
                        door.UnLockDoor();
                    }
                }
                else
                {
                    foreach (InteriorDoor door in Doors.Where(x=> x.LockWhenClosed))
                    {
                        door.LockDoor();
                    }
                }
                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                    NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, false);
                    NativeFunction.Natives.CAP_INTERIOR(DisabledInteriorID, false);
                    NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                    GameFiber.Yield();
                }
                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                IsActive = true;
                GameFiber.Yield();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            }
        }, "Load Interior");
    }
    public void Unload()
    {
        GameFiber.StartNew(delegate
            {
                try
                {
                    if (NeedsActivation)
                    {
                        NativeFunction.Natives.UNPIN_INTERIOR(InternalID);
                        NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, false);
                        if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                        {
                            NativeFunction.Natives.CAP_INTERIOR(InternalID, true);
                        }
                    }
                    foreach (string iplName in RequestIPLs)
                    {
                        NativeFunction.Natives.REMOVE_IPL(iplName);
                        GameFiber.Yield();
                    }
                    foreach (string iplName in RemoveIPLs)
                    {
                        NativeFunction.Natives.REQUEST_IPL(iplName);
                        GameFiber.Yield();
                    }
                    foreach (string interiorSet in InteriorSets)
                    {
                        NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, interiorSet);
                        GameFiber.Yield();
                    }
                    foreach (InteriorDoor door in Doors)
                    {
                        door.LockDoor();
                        //NativeFunction.Natives.x9B12F9A24FABEDB0(door.ModelHash, door.Position.X, door.Position.Y, door.Position.Z, true, 0.0f, 50.0f); //NativeFunction.Natives.x9B12F9A24FABEDB0(door.ModelHash, door.Position.X, door.Position.Y, door.Position.Z, true, door.Rotation.Pitch, door.Rotation.Roll, door.Rotation.Yaw);
                        //door.IsLocked = true;
                        door.Deactivate();
                        GameFiber.Yield();
                    }
                    if (DisabledInteriorCoords != Vector3.Zero)
                    {
                        DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                        NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, true);
                        NativeFunction.Natives.CAP_INTERIOR(DisabledInteriorID, true);
                        NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                        GameFiber.Yield();
                    }
                    NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                    //SetInactive();
                    IsActive = false;
                    GameFiber.Yield();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Unload Interiors");
    }
    public void Update()
    {
        foreach (InteriorDoor door in Doors.Where(x=>x.ForceRotateOpen && !x.HasBeenForceRotatedOpen))
        {
            EntryPoint.WriteToConsole("ATTEMPTING TO FORCE ROTATE OPEN DOOR THAT WASNT THERE");
            door.UnLockDoor();
        }
        if(IsTeleportEntry)
        {
            return;
        }
        if(InteractPoints== null || !InteractPoints.Any() || InteractableLocation == null)
        {
            return;
        }

        if(!IsRunningInteriorUpdate && InteractableLocation.DistanceToPlayer <= 100f)
        {
            OnBecameClose();
        }
    }
    public virtual void Teleport(IInteractionable player, GameLocation interactableLocation, LocationCamera locationCamera)
    {
        InteractableLocation = interactableLocation;
        Player = player;
        if (InteractableLocation.Interior != null && InteractableLocation.Interior.IsTeleportEntry)
        {
            Game.FadeScreenOut(1500, true);
            Player.Character.Position = InteractableLocation.Interior.InteriorEgressPosition;
            Player.Character.Heading = InteractableLocation.Interior.InteriorEgressHeading;
            IsInside = true;
            IsMenuInteracting = false;
            locationCamera?.StopImmediately();
            GameFiber.Sleep(1000);
            Game.FadeScreenIn(1500, true);
            OnWentInside();
        }
    }


    public void OnBecameClose()
    {
        if(IsTeleportEntry || IsRunningInteriorUpdate)
        {
            return;
        }
        GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole($"Interior OnBecameClose {Name}");
                IsRunningInteriorUpdate = true;
                IsMenuInteracting = false;
                while (IsActive && EntryPoint.ModController.IsRunning)
                {
                    if (InteractableLocation.DistanceToPlayer <= 100f)
                    {
                        InsideLoopNew();
                    }
                    GameFiber.Yield();
                }
                IsRunningInteriorUpdate = false;
                EntryPoint.WriteToConsole($"Interior StoppedBecomingClose {Name}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }

        }, "Interact");
    }


    private void OnWentInside()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsInside && EntryPoint.ModController.IsRunning)
                {
                    InsideLoopNew();
                    GameFiber.Yield();
                }
                Player.ActivityManager.IsInteractingWithLocation = false;
                InteractableLocation.CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }

        }, "Interact");
    }
    public virtual void InsideLoopNew()
    {
        foreach(InteriorInteract interiorInteract in InteractPoints)
        {
            interiorInteract.Update(Player,Settings,InteractableLocation, this);
        }
    }
    public void Exit()
    {
        IsMenuInteracting = false;
        RemoveButtonPrompts();
        TeleportOut();
    }
    private void SetInactive()
    {
        if (IsInside)
        {
            RemoveButtonPrompts();
            IsInside = false;
        }
    }
    public void ForceExitPlayer(IInteractionable player, GameLocation interactableLocation)
    {
        InteractableLocation = interactableLocation;
        Player = player;
        if (!IsInside)
        {
            return;
        }
        RemoveButtonPrompts();
        TeleportOut();
    }
    public virtual void RemoveButtonPrompts()
    {
        EntryPoint.WriteToConsole("INTERIOR RemoveButtonPrompts RAN");
        foreach (InteriorInteract interiorInteract in InteractPoints)
        {
            interiorInteract.RemovePrompt();
        }
    }
    protected virtual void TeleportOut()
    {
        //Do exit cam here from the main location?
        IsInside = false;
        if (InteractableLocation != null)
        {
            Game.FadeScreenOut(1500, true);
            Player.Character.Position = InteractableLocation.EntrancePosition;
            Player.Character.Heading = InteractableLocation.EntranceHeading;
            Player.Character.IsVisible = false;
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1000, true);
            InteractableLocation.DoExitCamera();
            //GameFiber.Sleep(1000);
        }
    }
}
