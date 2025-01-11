using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[Serializable()]
public class Interior
{
    private bool IsInside;
    protected IInteractionable Player;
    protected ILocationInteractable LocationInteractable;
    protected GameLocation InteractableLocation;
    protected ISettingsProvideable Settings;
    private bool IsActive = false;
    private bool IsRunningInteriorUpdate = false;
    protected List<Rage.Object> SpawnedProps = new List<Rage.Object>();

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
    public bool IsTeleportEntry { get; set; } = false;
    public Vector3 DisabledInteriorCoords { get; set; } = Vector3.Zero;
    public List<InteriorDoor> Doors { get; set; } = new List<InteriorDoor>();
    public List<string> RequestIPLs { get; set; } = new List<string>();
    public List<string> RemoveIPLs { get; set; } = new List<string>();
    public List<string> InteriorSets { get; set; } = new List<string>();
    public int InteriorTintColor { get; set; } = -1;
    public int InteriorSetStyleID { get; set; } = -1;
    public Vector3 InteriorEgressPosition { get; set; }
    public float InteriorEgressHeading { get; set; }
    public bool NeedsActivation { get; set; } = false;
    public bool NeedsSetDisabled { get; set; } = false;
    public bool IsRestricted { get; set; } = false;
    public bool IsCivilianReactableRestricted { get; set; } = false;
    public bool IsWeaponRestricted { get; set; } = false;
    public bool IsTunnel { get; set; } = false;
    public float MaxUpdateDistance { get; set; } = 50f;
    public List<InteriorInteract> InteractPoints { get; set; } = new List<InteriorInteract>();
    public List<Vector3> ClearPositions { get; set; } = new List<Vector3>();
    public string ForceAutoInteractName { get; set; }
    public List<PropSpawn> PropSpawns { get; set; }
    public List<SpawnPlace> VendorLocations { get; set; } = new List<SpawnPlace>();
    public List<Vector3> SearchLocations { get; set; }
    [XmlIgnore]
    public virtual List<InteriorInteract> AllInteractPoints => InteractPoints;
    public InteriorInteract ClosestInteract => AllInteractPoints.Where(x => x.CanAddPrompt).OrderBy(x => x.DistanceTo).FirstOrDefault();
    public virtual void Setup(IInteractionable player, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ILocationInteractable locationInteractable, IModItems modItems, IClothesNames clothesNames)
    {
        Settings = settings;
        Player = player;
        LocationInteractable = locationInteractable;
        if (!IsTeleportEntry)
        {
            InteractableLocation = placesOfInterest.AllLocations().Where(x=> x.InteriorID == LocalID).FirstOrDefault();
        }
        foreach (InteriorInteract interiorInteract in AllInteractPoints)//InteractPoints)
        {
            interiorInteract.Setup(modItems, clothesNames);
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
                if(NeedsSetDisabled)
                {
                    NativeFunction.Natives.DISABLE_INTERIOR(InternalID, false);
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
                    if (!NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName))
                    {
                        NativeFunction.Natives.REQUEST_IPL(iplName);
                    }
                    GameFiber.Yield();
                }
                foreach (string iplName in RemoveIPLs)
                {
                    if (NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName))
                    {
                        NativeFunction.Natives.REMOVE_IPL(iplName);
                    }
                    GameFiber.Yield();
                }
                // Deactivate the current entity set style before activating the new one ( If one is loaded )
                if (InteriorSetStyleID != -1)
                {
                    string previousEntitySetStyle = $"entity_set_style_{InteriorSetStyleID}";
                    EntryPoint.WriteToConsole($"Deactivating previous entity set style: {previousEntitySetStyle}");
                    NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, previousEntitySetStyle);
                    GameFiber.Yield();
                }
                foreach (string interiorSet in InteriorSets)
                {
                    NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(InternalID, interiorSet);
                    if (InteriorTintColor != -1)
                    {
                        SetInteriorColorTint(interiorSet, InteriorTintColor);  // Apply the tint color to each interior set
                    }
                    GameFiber.Yield();
                }
                // Activate new entity set style
                if (InteriorSetStyleID != -1)
                {
                    string newEntitySetStyle = $"entity_set_style_{InteriorSetStyleID}";
                    EntryPoint.WriteToConsole($"Activating new entity set style: {newEntitySetStyle}");
                    NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(InternalID, newEntitySetStyle);
                    GameFiber.Yield();
                }
                LoadDoors(isOpen);
                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                    NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, false);
                    NativeFunction.Natives.CAP_INTERIOR(DisabledInteriorID, false);
                    NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                    GameFiber.Yield();
                }
                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                if (AllInteractPoints != null)
                {
                    foreach (InteriorInteract ii in AllInteractPoints)
                    {
                        ii.OnInteriorLoaded();
                    }
                }
                IsActive = true;
                GameFiber.Yield();
                EntryPoint.WriteToConsole($"Load Interior {Name} isOpen{isOpen}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            }
        }, "Load Interior");
    }
    protected virtual void LoadDoors(bool isOpen)
    {
        if (isOpen)
        {
            foreach (InteriorDoor door in Doors)
            {
                door.UnLockDoor();
            }
        }
        else
        {
            foreach (InteriorDoor door in Doors.Where(x => x.LockWhenClosed))
            {
                door.LockDoor();
            }
        }
    }
    public void Unload()
    {
        GameFiber.StartNew(delegate
            {
                try
                {
                    if (NeedsSetDisabled)
                    {
                        NativeFunction.Natives.DISABLE_INTERIOR(InternalID, true);
                    }
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
                    // Deactivate the current entity set style if active
                    if (InteriorSetStyleID != -1)
                    {
                        string entitySetStyle = $"entity_set_style_{InteriorSetStyleID}";
                        EntryPoint.WriteToConsole($"Deactivating entity set style on unload: {entitySetStyle}");
                        NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, entitySetStyle);
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

                    EntryPoint.WriteToConsole($"Unload Interior {Name}");
                    //new Vector3(-19.51501f, -597.6929f, 94.02557f)
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
        //if(IsTeleportEntry)
        //{
        //    return;
        //}
        //if(InteractPoints== null || !InteractPoints.Any() || InteractableLocation == null)
        //{
        //    return;
        //}

        //if(!IsRunningInteriorUpdate && InteractableLocation.DistanceToPlayer <= 100f)
        //{
        //    OnBecameClose();
        //}
    }
    public virtual void Teleport(IInteractionable player, GameLocation interactableLocation, LocationCamera locationCamera)
    {
        InteractableLocation = interactableLocation;
        Player = player;
        if (InteractableLocation.Interior != null && InteractableLocation.Interior.IsTeleportEntry)
        {
            Game.FadeScreenOut(1500, true);
            InteractableLocation.Interior.Load(true);
            Player.Character.Position = InteractableLocation.Interior.InteriorEgressPosition;
            Player.Character.Heading = InteractableLocation.Interior.InteriorEgressHeading;
            IsInside = true;
            IsMenuInteracting = false;
            locationCamera?.StopImmediately(true);
            Player.InteriorManager.OnTeleportedInside(InteractableLocation);
            GameFiber.Sleep(500);
            RemoveExistingPeds();
            if(VendorLocations != null && VendorLocations.Any())
            {
                SpawnInteriorVendors();
            }
            if (PropSpawns != null && PropSpawns.Any())
            {
                SpawnInteriorProps();
            }
            if (!string.IsNullOrEmpty(ForceAutoInteractName))
            {
                GameFiber.Sleep(250);
                Player.ActivityManager.IsInteractingWithLocation = true;
                DoAutoInteract();
                Exit();
                Player.ActivityManager.IsInteractingWithLocation = false;
            }
            else
            {
                Game.FadeScreenIn(1500, true);
            }
        }
    }
    private void DoAutoInteract()
    {
        InteriorInteract ii = AllInteractPoints.FirstOrDefault(x => x.Name == ForceAutoInteractName);
        if(ii == null)
        {
            return;
        }
        ii.SetupFake(Player, Settings, InteractableLocation, LocationInteractable);
        ii.OnInteract();
    }
    public void SetInteriorColorTint(string entitySetName, int InteriorColorStyle)
    {
        if (InternalID != 0) // Ensure the interior instance index is valid
        {
            NativeFunction.Natives.SET_INTERIOR_ENTITY_SET_TINT_INDEX(InternalID, entitySetName, InteriorColorStyle);
        }
        else
        {
            // Handle the case where the interior instance index is not valid
            EntryPoint.WriteToConsole("Error: Interior instance index is not valid.");
        }
    }
    private void SpawnInteriorProps()
    {
        foreach(PropSpawn ps in PropSpawns)
        {
            Rage.Object newProp = new Rage.Object(ps.ModelName, ps.SpawnPlace.Position, ps.SpawnPlace.Heading);// 239.2449f);
            if (newProp.Exists())
            {
                SpawnedProps.Add(newProp);
                if (ps.PlaceOnGround)
                {
                    NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(newProp);
                }
            }
        }
    }
    private void SpawnInteriorVendors()
    {
        Player.Dispatcher.LocationDispatcher.SpawnInteriorServiceWorker(InteractableLocation);
        //GameFiber.Sleep(1000);
    }
    protected virtual void RemoveExistingPeds()
    {
        foreach(Vector3 position in ClearPositions)
        {
            NativeFunction.Natives.CLEAR_AREA(position.X, position.Y, position.Z, 5f, true, false, false, false);
        }
    }
    public virtual void UpdateInteractDistances()
    {
        foreach (InteriorInteract interiorInteract in AllInteractPoints)
        {
            interiorInteract.UpdateDistances(Player);
        }
    }
    public void Exit()
    {
        IsMenuInteracting = false;
        RemoveButtonPrompts();
        TeleportOut();
    }

    public virtual void AddDistanceOffset(Vector3 offsetToAdd)
    {
        InternalInteriorCoordinates += offsetToAdd;
        DisabledInteriorCoords += offsetToAdd;

        foreach (InteriorDoor sl in Doors)
        {
            if (sl.Position != Vector3.Zero)
            {
                sl.Position += offsetToAdd;
            }
        }
        InteriorEgressPosition += offsetToAdd;
        foreach(InteriorInteract test in AllInteractPoints)
        {
            test.AddDistanceOffset(offsetToAdd);
        }
        ClearPositions.ForEach(x => x += offsetToAdd);
        if (PropSpawns != null)
        {
            foreach (PropSpawn test in PropSpawns)
            {
                test.SpawnPlace.AddDistanceOffset(offsetToAdd);
            }
        }
        if (VendorLocations != null)
        {
            foreach (var test2 in VendorLocations)
            {
                test2.AddDistanceOffset(offsetToAdd);
            }
        }
        if (SearchLocations != null)
        {
            SearchLocations.ForEach(x => x += offsetToAdd);
        }
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

            if (!string.IsNullOrEmpty(ForceAutoInteractName))
            {
                Game.FadeScreenOut(500, false);
            }
            else
            {
                Game.FadeScreenOut(1500, true);
            }


            Player.Character.Position = InteractableLocation.EntrancePosition;
            Player.Character.Heading = InteractableLocation.EntranceHeading;
            Player.Character.IsVisible = false;
            if (VendorLocations != null && VendorLocations.Any())
            {
                InteractableLocation.AttemptVendorDespawn();
            }
            RemoveSpawnedProps();
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1000, true);
            InteractableLocation.DoExitCamera(false);
            Player.InteriorManager.OnTeleportedOutside(InteractableLocation);
            //GameFiber.Sleep(1000);
        }
    }
    public virtual void OnCarryingWeaponInside(IViolateable player)
    {
        if(IsWeaponRestricted)
        {
            player.Violations.AddViolating(StaticStrings.ArmedRobberyCrimeID);
        }
    }
    private void RemoveSpawnedProps()
    {
        foreach(Rage.Object prop in SpawnedProps)
        {
            if(prop.Exists())
            {
                prop.Delete();
            }
        }
        SpawnedProps.Clear();
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        InteriorTintColor = -1;
        InteriorSetStyleID = -1;
    }

    public virtual void AddLocation(PossibleInteriors lppInteriors)
    {
        lppInteriors.GeneralInteriors.Add(this);
    }
}
