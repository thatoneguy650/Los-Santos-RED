using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;


public class MansionInterior : ResidenceInterior
{
    //private string CurrentMansionValueEntitySet;
    //private List<string> defaultEntities = new List<string>() { "SET_BASE_VAULT_00", "SET_BASE_VAULT_01", "SET_BASE_VAULT_02", "SET_BASE_VAULT_03", 
    //    "SET_BASE_VAULT_04", "SET_BASE_VAULT_05", "SET_BASE_VAULT_06", "SET_BASE_VAULT_07", "SET_BASE_VAULT_08", "SET_BASE_VAULT_09" };
    private List<int> LinkedInteriorIDs = new List<int>();
    private int MoneyInteriorID;

    public List<MoneyEntitySet> MoneyEntitySets = new List<MoneyEntitySet>();
    public Vector3 MoneyInteriorCoords { get; set; }
    [XmlIgnore]
    private readonly Dictionary<int, HashSet<string>> ActivatedSetsByInterior = new Dictionary<int, HashSet<string>>();
    public MansionInterior(int iD, string name) : base(iD, name)
    {
    }

    public MansionInterior()
    {

    }
    public override void OnPlayerLoadedSave()
    {
        MatchVaultToStoredCash();
        base.OnPlayerLoadedSave();
    }
    public override bool CheckMatchingIDs(int internalId)
    {
        return internalId == InternalID || LinkedInteriorIDs.Contains(internalId);
    }
    public override void OnStoredCashChanged(int storedCash)
    {
        if(InteriorSets == null)
        {
            return;
        }
        if(!InteriorSets.Exists(x => x == "SET_BASE_VAULT_00"))
        {
            return;
        }
        if(!IsActive)
        {
            return;
        }
        SetEntitySet(storedCash);
    }
    private void MatchVaultToStoredCash()
    {
        EntryPoint.WriteToConsole("MatchVaultToStoredCash LOADED START");
        if (InteriorSets == null)
        {
            return;
        }
        if(Residence == null)
        {
            return;
        }
        if (!InteriorSets.Exists(x => x == "SET_BASE_VAULT_00"))
        {
            return;
        }
        SetEntitySet(Residence.CashStorage.StoredCash);
    }
    private void SetEntitySet(int Cash)
    {
        foreach (MoneyEntitySet moneyEntitySet in MoneyEntitySets)
        {
            moneyEntitySet.SetStatus(MoneyInteriorID, Cash);
        }
        NativeFunction.Natives.REFRESH_INTERIOR(MoneyInteriorID);
    }
  







    public override void Load(bool isOpen)
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                // 1. Resolve initial interior
                InternalID = InternalInteriorCoordinates != Vector3.Zero
                    ? NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                        InternalInteriorCoordinates.X,
                        InternalInteriorCoordinates.Y,
                        InternalInteriorCoordinates.Z)
                    : LocalID;

                // 2. Request ALL IPLs FIRST
                foreach (string ipl in RequestIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) &&
                        !NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                    {
                        NativeFunction.Natives.REQUEST_IPL(ipl);
                    }
                    GameFiber.Yield();
                }

                GameFiber.Yield();

                // 3. Re-resolve interior AFTER IPLs
                if (InternalInteriorCoordinates != Vector3.Zero)
                {
                    int resolved = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                        InternalInteriorCoordinates.X,
                        InternalInteriorCoordinates.Y,
                        InternalInteriorCoordinates.Z);

                    if (resolved != 0)
                        InternalID = resolved;
                }

                if (InternalID == 0 || !NativeFunction.Natives.IS_VALID_INTERIOR<bool>(InternalID))
                {
                    EntryPoint.WriteToConsole($"Invalid Interior ID for {Name}");
                    return;
                }

                // 4. FORCE CLEAR
                ForceClearInterior(InternalID);

                foreach (Vector3 coord in LinkedInteriorCoords ?? Enumerable.Empty<Vector3>())
                {
                    int linkedID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                        coord.X, coord.Y, coord.Z);

                    if (linkedID != 0)
                        ForceClearInterior(linkedID);
                }

                if (MoneyInteriorCoords != Vector3.Zero)
                {
                    MoneyInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(MoneyInteriorCoords.X, MoneyInteriorCoords.Y, MoneyInteriorCoords.Z);
                }
                else
                {
                    MoneyInteriorID = InternalID;
                }

                GameFiber.Yield();

                // 5. Pin / activate interior
                if (NeedsActivation)
                {
                    NativeFunction.Natives.PIN_INTERIOR_IN_MEMORY(InternalID);
                    NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, true);

                    if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                        NativeFunction.Natives.CAP_INTERIOR(InternalID, false);

                    GameFiber.Yield();
                }

                // 6. Remove IPLs (if any)
                foreach (string ipl in RemoveIPLs)
                {
                    if (!string.IsNullOrEmpty(ipl) &&
                        NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                    {
                        NativeFunction.Natives.REMOVE_IPL(ipl);
                    }
                    GameFiber.Yield();
                }

                // 7. Activate entity sets
                foreach (string interiorSet in InteriorSets)
                {
                    if (string.IsNullOrEmpty(interiorSet))
                        continue;

                    if (TryActivateEntitySetWithVerify(InternalID, interiorSet, 0))
                        TrackActivatedSet(InternalID, interiorSet);

                    if (interiorSet.StartsWith("SET_WALLPAPER_", StringComparison.OrdinalIgnoreCase)
                        && InteriorWallpaperColor != -1)
                    {
                        NativeFunction.Natives.SET_INTERIOR_ENTITY_SET_TINT_INDEX(
                            InternalID, interiorSet, InteriorWallpaperColor);
                    }
                    else if (InteriorTintColor != -1)
                    {
                        SetInteriorColorTint(interiorSet, InteriorTintColor);
                    }

                    GameFiber.Yield();
                }

                LinkedInteriorIDs = new List<int>();
                // 8. Linked interiors (same sets, no re-clear)
                foreach (Vector3 coord in LinkedInteriorCoords ?? Enumerable.Empty<Vector3>())
                {
                    int linkedID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                        coord.X, coord.Y, coord.Z);

                    if (linkedID == 0)
                        continue;

                    LinkedInteriorIDs.Add(linkedID);

                    foreach (string interiorSet in InteriorSets)
                    {
                        if (string.IsNullOrEmpty(interiorSet))
                            continue;
                        if (TryActivateEntitySetWithVerify(linkedID, interiorSet, 0))
                            TrackActivatedSet(linkedID, interiorSet);

                        if (interiorSet.StartsWith("SET_WALLPAPER_", StringComparison.OrdinalIgnoreCase)
                            && InteriorWallpaperColor != -1)
                        {
                            NativeFunction.Natives.SET_INTERIOR_ENTITY_SET_TINT_INDEX(
                                linkedID, interiorSet, InteriorWallpaperColor);
                        }
                    }

                    NativeFunction.Natives.REFRESH_INTERIOR(linkedID);
                    GameFiber.Yield();
                }

                // 9. Optional style activation
                if (InteriorSetStyleID != -1)
                {
                    foreach (string styleName in GetEntitySetNamePatterns(InteriorSetStyleID))
                    {
                        if (TryActivateEntitySetWithVerify(InternalID, styleName, 0))
                            break;
                    }
                    GameFiber.Yield();
                }
                // 10. Doors / interactions
                LoadDoors(isOpen, false);
                // Final Refresh moved to end to ensure doors are in correct state

                // 11. Final refresh
                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);

                GameFiber.Yield(); // yield to all interior markers to register properly

                foreach (InteriorInteract ii in AllInteractPoints ?? Enumerable.Empty<InteriorInteract>())
                    ii.OnInteriorLoaded();


                

                IsActive = true;
                MatchVaultToStoredCash();
                EntryPoint.WriteToConsole($"Load Interior {Name} isOpen={isOpen}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(
                    $"Load Exception: {ex.Message}\n{ex.StackTrace}", 0);
            }
        }, "Load Interior");
    }
    public override void Unload()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                // Remove all spawned props immediately
                RemoveSpawnedProps();

                // Force-clear main interior (brute-force sanitation)
                ForceClearInterior(InternalID);

                // Deactivate any sets tracked by this script (redundant but safe)
                DeactivateTrackedSets();

                // Unpin and deactivate the interior
                NativeFunction.Natives.UNPIN_INTERIOR(InternalID);
                NativeFunction.Natives.SET_INTERIOR_ACTIVE(InternalID, false);
                if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(InternalID))
                    NativeFunction.Natives.CAP_INTERIOR(InternalID, true);

                GameFiber.Yield();

                // Remove requested IPLs
                foreach (string ipl in RequestIPLs ?? Enumerable.Empty<string>())
                {
                    if (!string.IsNullOrEmpty(ipl) && NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                        NativeFunction.Natives.REMOVE_IPL(ipl);
                    GameFiber.Yield();
                }

                // Reapply removed IPLs if needed (rare, but maintains consistency)
                foreach (string ipl in RemoveIPLs ?? Enumerable.Empty<string>())
                {
                    if (!string.IsNullOrEmpty(ipl) && !NativeFunction.Natives.IS_IPL_ACTIVE<bool>(ipl))
                        NativeFunction.Natives.REQUEST_IPL(ipl);
                    GameFiber.Yield();
                }

                // Clear linked interiors
                foreach (Vector3 coord in LinkedInteriorCoords ?? Enumerable.Empty<Vector3>())
                {
                    int linkedID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(coord.X, coord.Y, coord.Z);
                    if (linkedID == 0) continue;

                    ForceClearInterior(linkedID);
                    GameFiber.Yield();
                }

                // Deactivate doors
                foreach (InteriorDoor door in Doors ?? Enumerable.Empty<InteriorDoor>())
                {
                    door.LockDoor();
                    door.Deactivate();
                    GameFiber.Yield();
                }

                // Handle disabled interiors
                if (DisabledInteriorCoords != Vector3.Zero)
                {
                    DisabledInteriorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(
                        DisabledInteriorCoords.X, DisabledInteriorCoords.Y, DisabledInteriorCoords.Z);

                    if (DisabledInteriorID != 0)
                    {
                        ForceClearInterior(DisabledInteriorID);
                        NativeFunction.Natives.DISABLE_INTERIOR(DisabledInteriorID, true);
                        NativeFunction.Natives.REFRESH_INTERIOR(DisabledInteriorID);
                        GameFiber.Yield();
                    }
                }
                // Final refresh and alarm cleanup
                NativeFunction.Natives.REFRESH_INTERIOR(InternalID);
                TurnOffAlarm();


                if (AllInteractPoints != null)
                {
                    foreach (InteriorInteract ii in AllInteractPoints)
                    {
                        ii.OnInteriorUnloaded();
                    }
                }

                IsActive = false;

                GameFiber.Yield();
                EntryPoint.WriteToConsole($"Unload Interior {Name}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"Unload Exception: {ex.Message}\n{ex.StackTrace}", 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Unload Interiors");
    }
    // Helper to generate possible entity set names for a given style ID
    private IEnumerable<string> GetEntitySetNamePatterns(int styleID)
    {
        yield return $"entity_set_style_{styleID}";
        yield return $"set_style_{styleID}";
        yield return $"set_style0{styleID}";
        yield return $"style_{styleID}";
        yield return $"style{styleID}";
    }
    // Activate an entity set and verify it activated
    // returns true if activated successfully
    private bool TryActivateEntitySetWithVerify(int interiorID, string setName, int maxTicksToWait = 30) // 0.5 seconds at 16ms per tick
    {
        if (string.IsNullOrEmpty(setName)) return false;
        try
        {
            NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(interiorID, setName);
        }
        catch
        {
            // swallow - native may ignore invalid names
            return false;
        }
        int ticks = 0;
        while (!NativeFunction.Natives.IS_INTERIOR_ENTITY_SET_ACTIVE<bool>(interiorID, setName) && ticks < maxTicksToWait)
        {
            NativeFunction.Natives.REFRESH_INTERIOR(interiorID);
            GameFiber.Yield();
            ticks++;
        }
        return NativeFunction.Natives.IS_INTERIOR_ENTITY_SET_ACTIVE<bool>(interiorID, setName);
    }
    private void TrackActivatedSet(int interiorId, string setName)
    {
        if (interiorId == 0 || string.IsNullOrEmpty(setName))
            return;

        if (!ActivatedSetsByInterior.TryGetValue(interiorId, out var setList))
        {
            setList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ActivatedSetsByInterior[interiorId] = setList;
        }

        setList.Add(setName);
    }
    private void DeactivateTrackedSets()
    {
        foreach (var kvp in ActivatedSetsByInterior)
        {
            int interiorId = kvp.Key;

            foreach (string setName in kvp.Value)
            {
                try
                {
                    NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(interiorId, setName);
                }
                catch { }
            }

            NativeFunction.Natives.REFRESH_INTERIOR(interiorId);
        }

        ActivatedSetsByInterior.Clear();
    }
    private void ForceClearInterior(int interiorId)
    {
        if (interiorId == 0 || !NativeFunction.Natives.IS_VALID_INTERIOR<bool>(interiorId))
            return;

        string[] prefixes =
        {
        "SET_STYLE",
        "SET_WALLPAPER",
        "entity_set_",
        "entity_set_style",
        "set_style",
        "style"
        };
        // Numeric suffixes (0–12)
        for (int i = 0; i <= 12; i++)
        {
            foreach (string p in prefixes)
            {
                TryDeactivate(interiorId, $"{p}_{i}");
                TryDeactivate(interiorId, $"{p}{i}");
            }
        }

        // Letter suffixes (A–Z)
        for (char c = 'A'; c <= 'Z'; c++)
        {
            foreach (string p in prefixes)
            {
                TryDeactivate(interiorId, $"{p}_{c}");
                TryDeactivate(interiorId, $"{p}{c}");
            }
        }

        // Bare prefixes (many legacy DLCs use exact names)
        foreach (string p in prefixes)
        {
            TryDeactivate(interiorId, p);
        }
    }
    private void TryDeactivate(int interiorId, string setName)
    {
        try
        {
            if (NativeFunction.Natives.IS_INTERIOR_ENTITY_SET_ACTIVE<bool>(interiorId, setName))
            {
                NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(interiorId, setName);
            }
        }
        catch
        {
            // Silent by design — invalid names are expected
        }
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        InteriorTintColor = -1;
        InteriorSetStyleID = -1;
        InteriorWallpaperColor = -1;
    }
}

