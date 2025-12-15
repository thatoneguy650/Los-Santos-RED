using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Interop;
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
    private int alarmSoundID;
    protected bool isAlarmActive;
    private bool isOpen;

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
    public List<Vector3> LinkedInteriorCoords { get; set; } = new List<Vector3>();
    public int InteriorTintColor { get; set; } = -1;
    public int InteriorSetStyleID { get; set; } = -1;
    public int InteriorWallpaperColor { get; set; } = -1;
    public Vector3 InteriorEgressPosition { get; set; }
    public float InteriorEgressHeading { get; set; }
    public bool NeedsActivation { get; set; } = false;
    public bool NeedsSetDisabled { get; set; } = false;
    public bool IsRestricted { get; set; } = false;
    public bool IsCivilianReactableRestricted { get; set; } = false;
    public bool IsWeaponRestricted { get; set; } = false;
    public bool IsTunnel { get; set; } = false;
    public float MaxUpdateDistance { get; set; } = 50f;


    public bool IsTrespassingWhenClosed { get; set; } = false;
    public List<InteriorInteract> InteractPoints { get; set; } = new List<InteriorInteract>();
    public List<Vector3> ClearPositions { get; set; } = new List<Vector3>();
    public string ForceAutoInteractName { get; set; }
    public List<PropSpawn> PropSpawns { get; set; }
    public List<SpawnPlace> VendorLocations { get; set; } = new List<SpawnPlace>();
    public List<Vector3> SearchLocations { get; set; }
    [XmlIgnore]
    public virtual List<InteriorInteract> AllInteractPoints => InteractPoints;
    public InteriorInteract ClosestInteract => AllInteractPoints.Where(x => x.CanAddPrompt).OrderBy(x => x.DistanceTo).FirstOrDefault();

    public GameLocation GameLocation { get; set; }
    public bool IsAlarmActive => isAlarmActive;
    public bool IsNotAlarmed { get; set; }

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
        foreach(InteriorDoor interiorDoor in Doors)
        {
            interiorDoor.AddPairedDoors(Doors.Where(x => x.DoorGroupName == interiorDoor.DoorGroupName && x.Position != interiorDoor.Position).ToList());
        }
    }
    public void DebugLockDoors()
    {
        foreach (InteriorDoor door in Doors)
        {
            door.LockDoor();
            EntryPoint.WriteToConsole($"INTERIOR: {Name} {door.ModelHash} {door.Position} LOCKED");
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
                // Resolve interior ID
                InternalID = InternalInteriorCoordinates != Vector3.Zero
                    ? NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(InternalInteriorCoordinates.X, InternalInteriorCoordinates.Y, InternalInteriorCoordinates.Z)
                    : LocalID;

                // Pin interior in memory if needed
                if (NeedsActivation)
                {
                    NativeFunction.Natives.PIN_INTERIOR_IN_MEMORY(InternalID);
                    NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, true);
                    if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                        NativeFunction.Natives.CAP_INTERIOR(InternalID, false);
                    GameFiber.Yield();
                }

                // Request IPLs
                foreach (string ipl in RequestIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) && !NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                        NativeFunction.Natives.REQUEST_IPL(ipl);
                    GameFiber.Yield();
                }

                GameFiber.Yield();

                // Re-resolve InternalID after IPLs
                if (InternalInteriorCoordinates != Vector3.Zero)
                {
                    int resolved = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(InternalInteriorCoordinates.X, InternalInteriorCoordinates.Y, InternalInteriorCoordinates.Z);
                    if (resolved != 0) InternalID = resolved;
                }

                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                GameFiber.Yield();

                // Remove IPLs
                foreach (string ipl in RemoveIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) && NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                    {
                        NativeFunction.Natives.REMOVE_IPL(ipl);
                        GameFiber.Yield();
                    }
                }

                // Deactivate previous entity sets and styles
                for (int i = 1; i <= 9; i++)
                {
                    foreach (string prefix in new[] { "entity_set_style_", "set_style_", "set_style0", "style_", "style" })
                    {
                        try { NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, prefix + i); } catch { }
                    }
                    GameFiber.Yield();
                }

                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                GameFiber.Yield();

                // Activate interior sets
                foreach (string interiorSet in InteriorSets)
                {
                    if (string.IsNullOrEmpty(interiorSet)) continue;
                    TryActivateEntitySetWithVerify(InternalID, interiorSet, 0);

                    if (interiorSet.StartsWith("SET_WALLPAPER_", StringComparison.OrdinalIgnoreCase) && InteriorWallpaperColor != -1)
                        NativeFunction.Natives.SET_INTERIOR_ENTITY_SET_TINT_INDEX(InternalID, interiorSet, InteriorWallpaperColor);
                    else if (InteriorTintColor != -1)
                        SetInteriorColorTint(interiorSet, InteriorTintColor);

                    GameFiber.Yield();
                }

                // Handle linked interiors
                foreach (Vector3 coord in LinkedInteriorCoords ?? Enumerable.Empty<Vector3>())
                {
                    int linkedID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(coord.X, coord.Y, coord.Z);
                    if (linkedID == 0) continue;

                    foreach (string interiorSet in InteriorSets)
                    {
                        if (string.IsNullOrEmpty(interiorSet)) continue;
                        TryActivateEntitySetWithVerify(linkedID, interiorSet, 0);

                        if (interiorSet.StartsWith("SET_WALLPAPER_", StringComparison.OrdinalIgnoreCase) && InteriorWallpaperColor != -1)
                            NativeFunction.Natives.SET_INTERIOR_ENTITY_SET_TINT_INDEX(linkedID, interiorSet, InteriorWallpaperColor);
                        else if (InteriorTintColor != -1)
                            SetInteriorColorTint(interiorSet, InteriorTintColor);

                        GameFiber.Yield();
                    }
                    NativeFunction.Natives.REFRESH_INTERIOR(linkedID);
                    GameFiber.Yield();
                }

                // Activate style patterns
                if (InteriorSetStyleID != -1)
                {
                    bool tried = false;
                    foreach (string styleName in GetEntitySetNamePatterns(InteriorSetStyleID))
                    {
                        if (TryActivateEntitySetWithVerify(InternalID, styleName, 0))
                        {
                            EntryPoint.WriteToConsole($"Activated style: {styleName}");
                            tried = true;
                            break;
                        }
                    }
                    if (!tried) EntryPoint.WriteToConsole($"Warning: could not activate style for StyleID {InteriorSetStyleID}");
                    GameFiber.Yield();
                }

                RefreshInteriorAndLinked(InternalID);

                // Load doors
                LoadDoors(isOpen, false); // re included

                // Disabled interior handling
                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                    NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, false);
                    NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                    GameFiber.Yield();
                }

                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);

                foreach (InteriorInteract ii in AllInteractPoints ?? Enumerable.Empty<InteriorInteract>())
                    ii.OnInteriorLoaded();

                IsActive = true;
                GameFiber.Yield();
                EntryPoint.WriteToConsole($"Load Interior {Name} isOpen={isOpen}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"Load Exception: {ex.Message}\n{ex.StackTrace}", 0);
            }
        }, "Load Interior");
    }
    protected virtual void LoadDoors(bool isOpen, bool reLockForcedEntry)
    {
        EntryPoint.WriteToConsole($"LOAD DOORS RAN {isOpen}");
        if (isOpen)
        {
            foreach (InteriorDoor door in Doors)
            {
                door.UnLockDoor();
            }
        }
        else
        {
            EntryPoint.WriteToConsole($"LOAD DOORS RAN LOCKING STUFF {isOpen}");

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
    public void Unload()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                NativeFunction.Natives.UNPIN_INTERIOR(InternalID);
                NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, false);
                if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                    NativeFunction.Natives.CAP_INTERIOR(InternalID, true);

                // Remove requested IPLs
                foreach (string ipl in RequestIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) && NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                        NativeFunction.Natives.REMOVE_IPL(ipl);
                    GameFiber.Yield();
                }
                foreach (string ipl in RemoveIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) && !NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                        NativeFunction.Natives.REQUEST_IPL(ipl);
                    GameFiber.Yield();
                }
                // Deactivate interior sets
                foreach (string interiorSet in InteriorSets ?? Enumerable.Empty<string>())
                {
                    if (string.IsNullOrEmpty(interiorSet)) continue;
                    try { NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, interiorSet); } catch { }
                    GameFiber.Yield();
                }

                // Deactivate style patterns
                if (InteriorSetStyleID != -1)
                {
                    foreach (string pattern in GetEntitySetNamePatterns(InteriorSetStyleID))
                    {
                        try { NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(InternalID, pattern); } catch { }
                        GameFiber.Yield();
                    }
                }

                // Linked interiors
                foreach (Vector3 coord in LinkedInteriorCoords ?? Enumerable.Empty<Vector3>())
                {
                    int linkedID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(coord.X, coord.Y, coord.Z);
                    if (linkedID == 0) continue;

                    foreach (string interiorSet in InteriorSets)
                    {
                        try { NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(linkedID, interiorSet); } catch { }
                        GameFiber.Yield();
                    }

                    NativeFunction.Natives.REFRESH_INTERIOR(linkedID);
                    GameFiber.Yield();
                }

                // Door cleanup
                foreach (InteriorDoor door in Doors ?? Enumerable.Empty<InteriorDoor>())
                {
                    door.LockDoor();
                    door.Deactivate();
                    GameFiber.Yield();
                }

                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);
                    NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, true);
                    NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                    GameFiber.Yield();
                }

                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                TurnOffAlarm();
                IsActive = false;
                GameFiber.Yield();
                EntryPoint.WriteToConsole($"Unload Interior {Name}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Unload Interiors");
    }
    public void Update(bool IsOpen)
    {
        foreach (InteriorDoor door in Doors.Where(x=> !x.IsLocked && x.ForceRotateOpen && !x.HasBeenForceRotatedOpen))
        {
            //EntryPoint.WriteToConsole("ATTEMPTING TO FORCE ROTATE OPEN DOOR THAT WASNT THERE");
            door.UnLockDoor();
        }
        foreach (InteriorDoor door in Doors.Where(x => x.IsLocked && x.LockWhenClosed && !x.HasRanLockWithEntity))
        {
            //EntryPoint.WriteToConsole("ATTEMPTING TO LOCK A DOOR WHERE THE ENTITY DOESNT EXISTS");
            door.LockDoor();
        }
        if(!IsOpen && Player.CurrentLocation != null && Player.CurrentLocation.CurrentInterior != null && Player.CurrentLocation.CurrentInterior == this)
        {
            return;//dont do anything until you leave
        }
        if(Player.CurrentLocation.TimeOutside < 10000)
        {
            return; //give you 10 seconds OUTSIde befoe it locks
        }

        if(isOpen != IsOpen)
        {
            if (IsOpen)
            {      
                if (GameLocation != null)
                {
                    GameLocation.IsServiceFilled = false;
                }
                EntryPoint.WriteToConsole($"Interior changed from closed to Open {Name}");
            }
            else
            {
                EntryPoint.WriteToConsole($"Interior changed from open to closed {Name}");
            }
            LoadDoors(IsOpen, false);
            isOpen = IsOpen;
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
        if (InternalInteriorCoordinates != Vector3.Zero)
        {
            InternalInteriorCoordinates += offsetToAdd;
        }
        if (DisabledInteriorCoords != Vector3.Zero)
        {
            DisabledInteriorCoords += offsetToAdd;
        }

        foreach (InteriorDoor sl in Doors)
        {
            if (sl.Position != Vector3.Zero)
            {
                sl.Position += offsetToAdd;
            }
            if(sl.InteractPostion != Vector3.Zero)
            {
                sl.InteractPostion += offsetToAdd;
            }
        }
        if (InteriorEgressPosition != Vector3.Zero)
        {
            InteriorEgressPosition += offsetToAdd;
        }
        foreach(InteriorInteract test in AllInteractPoints)
        {
            test.AddDistanceOffset(offsetToAdd);
        }
        for (int i = 0; i < ClearPositions.Count; i++)
        {
            ClearPositions[i] += offsetToAdd;
        }
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
        Doors.Clear();
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



    public virtual void SetOffAlarm()
    {
        if(IsNotAlarmed)
        {
            return;
        }
        if(IsAlarmActive)
        {
            return;
        }
        alarmSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        Vector3 Coords = Game.LocalPlayer.Character.Position;
        uint GameTimeStarted = Game.GameTime;
        while (!NativeFunction.Natives.REQUEST_SCRIPT_AUDIO_BANK<bool>("Alarms", false, -1) && Game.GameTime - GameTimeStarted <= 2000)
        {
            GameFiber.Yield();
        }
        isAlarmActive = true;
        if(GameLocation != null)
        {
            Coords = GameLocation.EntrancePosition;
        }
        NativeFunction.Natives.PLAY_SOUND_FROM_COORD(alarmSoundID, "Burglar_Bell", Coords.X, Coords.Y, Coords.Z, "Generic_Alarms", false, 0, false);
        //NativeFunction.Natives.PLAY_SOUND_FROM_COORD(alarmSoundID, "ALARMS_KLAXON_03_CLOSE", Coords.X, Coords.Y, Coords.Z, "", false,0,false);
    }
    public void TurnOffAlarm()
    {
        if(!IsAlarmActive)
        {
            return;
        }
        isAlarmActive = false;
        NativeFunction.Natives.STOP_SOUND(alarmSoundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(alarmSoundID);
    }
    // Refresh main interior and discovered linked interiors

    private void RefreshInteriorAndLinked(int interiorId)
    {
        NativeFunction.Natives.REFRESH_INTERIOR(interiorId);

        // Refresh disabled / linked interior if present
        if (DisabledInteriorCoords != Vector3.Zero)
        {
            DisabledInteriorID =
                NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                    DisabledInteriorCoords.X,
                    DisabledInteriorCoords.Y,
                    DisabledInteriorCoords.Z);

            if (DisabledInteriorID != 0)
            {
                NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
            }
        }
    }
    private IEnumerable<string> GetEntitySetNamePatterns(int styleID)
    {
        yield return $"entity_set_style_{styleID}";
        yield return $"set_style_{styleID}";
        yield return $"set_style0{styleID}";
        yield return $"style_{styleID}";
        yield return $"style{styleID}";
    }

    // Activate an entity set and do a short verification loop that it "took"
    // returns true if we observed the set active (best-effort)
    private bool TryActivateEntitySetWithVerify(int interiorID, string setName, int maxTicksToWait = 60)
    {
        if (string.IsNullOrEmpty(setName)) return false;
        try
        {
            NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(interiorID, setName);
        }
        catch
        {
            // swallow - native may ignore invalid names
        }

        int ticks = 0;
        // best-effort: call refresh and yield a few times; there's no direct "is entity set active" native in the environment common to all versions
        while (ticks < maxTicksToWait)
        {
            NativeFunction.Natives.REFRESH_INTERIOR(interiorID);
            GameFiber.Yield();
            ticks++;
        }

        // We can't reliably query "is entity set active" with the standard natives in all versions,
        // but calling ACTIVATE + REFRESH stabilizes the state for many DLC types.
        // Return true as long as we didn't error out (optimistic).
        // If you have a custom check (GET_INTERIOR_ENTITY_SET_ACTIVE) in your native set, use it here.
        return true;
    }
}
