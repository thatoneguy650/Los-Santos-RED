using LosSantosRED.lsr.Interface;
using Mod;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InteriorManager
{
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private IInteriorManageable Player;
    private IInteractionable Interactionable;
    private ILocationInteractable LocationInteractable;

    private bool IsRunningInteriorUpdate;
    private bool IsActive;
    private List<GameLocation> InteriorUpdateLocations = new List<GameLocation>();


    private uint GameTimeLastUpdatedDistances;
    private InteriorInteract PrevClosestInteriorInteract = null;
    private InteriorInteract ClosestInteriorInteract = null;
    private Interior ClosestInterior = null;
    private GameLocation ClosestLocation = null;
    private float closestDistance = 999f;
    private Interior TeleportInterior;
    private bool IsUpdatingSingleLocation;
    public bool IsInsideTeleportInterior { get; private set; } = false;

    public GameLocation CurrentTeleportInteriorLocation { get; private set; }

    public InteriorManager(IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IInteriorManageable player, IInteractionable interactionable, ILocationInteractable locationInteractable)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = player;
        Interactionable = interactionable;
        LocationInteractable = locationInteractable;
    }
    public void Setup()
    {
        IsActive = true;
    }
    public void Reset()
    {
        IsUpdatingSingleLocation = false;
        IsInsideTeleportInterior = false;
        CurrentTeleportInteriorLocation = null;
        InteriorUpdateLocations.Clear();
        IsActive = true;
    }
    public void Update()
    {
        if (IsUpdatingSingleLocation)
        {
            if(CurrentTeleportInteriorLocation != null && CurrentTeleportInteriorLocation.Interior != null && CurrentTeleportInteriorLocation.Interior.InteriorEgressPosition.DistanceTo(Player.Character) >= CurrentTeleportInteriorLocation.InteriorMaxUpdateDistance)// 50f)
            {
                EntryPoint.WriteToConsole($"YOU MOVED AWAY FROM {CurrentTeleportInteriorLocation.Name} TURNING OFF INTERIOR UPDATE");
                OnTeleportedOutside(CurrentTeleportInteriorLocation);
            }
            return;
        }
        foreach(GameLocation gameLocation in World.Places.ActiveLocations.ToList())
        {
            if (!gameLocation.HasInterior || gameLocation.Interior == null || gameLocation.Interior.IsTeleportEntry)
            {
                continue;
            }
            if (gameLocation.Interior.AllInteractPoints == null || !gameLocation.Interior.AllInteractPoints.Any())
            {
                continue;
            }
            if(gameLocation.IsActivated && gameLocation.DistanceToPlayer <= gameLocation.InteriorMaxUpdateDistance)// 50f)
            {
                if(!InteriorUpdateLocations.Any(x=> x.Interior?.LocalID == gameLocation.Interior?.LocalID))
                {
                    InteriorUpdateLocations.Add(gameLocation);
                }
                StartInteriorChecking();
            }
        }
        InteriorUpdateLocations.RemoveAll(x => !x.IsActivated || !x.IsNearby || x.DistanceToPlayer >= x.InteriorMaxUpdateDistance);// 50f);
    }
    public void OnStoppedInteracting()
    {
        GameTimeLastUpdatedDistances = 0;
        UpdateClosestInteract();
        if (ClosestInteriorInteract != null && ClosestInteriorInteract.ShouldAddPrompt && !Interactionable.ButtonPrompts.HasPrompt(ClosestInteriorInteract.ButtonPromptText))
        {
            OnClosestInteractChanged();
            EntryPoint.WriteToConsole($"UpdateClosestInteract NO PROMPT, READDING");
        }
    }
    public void Dispose()
    {
        IsUpdatingSingleLocation = false;
        IsInsideTeleportInterior = false;
        CurrentTeleportInteriorLocation = null;
        InteriorUpdateLocations.Clear();
        IsActive = false;
    }
    private void StartInteriorChecking()
    {
        if(IsRunningInteriorUpdate)
        {
            return;
        }
        GameFiber.Yield();
        GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole($"Interior StartInteriorChecking");
                IsRunningInteriorUpdate = true;
                while (IsActive && EntryPoint.ModController.IsRunning && InteriorUpdateLocations.Any())
                {
                    UpdateClosestInteract();
                    ClosestInteriorInteract?.UpdateActivated(Interactionable, Settings, ClosestLocation, ClosestInterior, LocationInteractable);
                    HandleMarkers();
                    GameFiber.Yield();
                }
                IsRunningInteriorUpdate = false;
                EntryPoint.WriteToConsole($"Interior StartInteriorChecking STOP");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }

        }, "Interact");
    }

    private void HandleMarkers()
    {
        if (!Settings.SettingsManager.WorldSettings.ShowMarkersInInteriors)
        {
            return;
        }
        foreach(GameLocation location in InteriorUpdateLocations)
        {
            if(location.Interior.IsMenuInteracting)
            {
                continue;
            }
            foreach(InteriorInteract interiorInteract in location.Interior.AllInteractPoints)
            {
                interiorInteract.DisplayMarker(Settings.SettingsManager.WorldSettings.InteriorMarkerType, Settings.SettingsManager.WorldSettings.InteriorMarkerZOffset, Settings.SettingsManager.WorldSettings.InteriorMarkerScale);
            }
        }
    }
    private void UpdateClosestInteract()
    {
        if(Game.GameTime - GameTimeLastUpdatedDistances < 500)
        {
            return;
        }
        closestDistance = 999f;
        ClosestInteriorInteract = null;
        ClosestInterior = null;
        ClosestLocation = null;
        foreach (GameLocation gameLocation in InteriorUpdateLocations.ToList())
        {
            gameLocation.Interior.UpdateInteractDistances();
            InteriorInteract closestIIForLocation = gameLocation.Interior.ClosestInteract;
            if (closestIIForLocation == null)
            {
                continue;
            }
            if (closestIIForLocation.DistanceTo < closestDistance)
            {
                ClosestInteriorInteract = closestIIForLocation;
                ClosestInterior = gameLocation.Interior;
                ClosestLocation = gameLocation;
                closestDistance = closestIIForLocation.DistanceTo;
            }
        }
        GameTimeLastUpdatedDistances = Game.GameTime;
        if(PrevClosestInteriorInteract == null && ClosestInteriorInteract != null)
        {
            OnClosestInteractChanged();
            EntryPoint.WriteToConsole($"UpdateClosestInteract CHANGED FROM NULL TO {ClosestInteriorInteract.Position}");
        }
        else if (PrevClosestInteriorInteract != null && ClosestInteriorInteract != null && PrevClosestInteriorInteract != ClosestInteriorInteract)
        {
            OnClosestInteractChanged();
            EntryPoint.WriteToConsole($"UpdateClosestInteract CHANGED FROM {PrevClosestInteriorInteract.Position} TO {ClosestInteriorInteract.Position}");
        }
        else if (PrevClosestInteriorInteract != null && ClosestInteriorInteract == null)
        {
            OnClosestInteractChanged();
            EntryPoint.WriteToConsole($"UpdateClosestInteract CHANGED FROM {PrevClosestInteriorInteract.Position} TO NULL");
        }
        else if (ClosestInteriorInteract != null && ClosestInteriorInteract.ShouldAddPrompt && !Interactionable.ButtonPrompts.HasPrompt(ClosestInteriorInteract.ButtonPromptText))
        {
            OnClosestInteractChanged();
            EntryPoint.WriteToConsole($"UpdateClosestInteract NO PROMPT, READDING");
        }
        PrevClosestInteriorInteract = ClosestInteriorInteract;
       // GameFiber.Yield();
    }

    private void OnClosestInteractChanged()
    {
        if(PrevClosestInteriorInteract != null)
        {
            PrevClosestInteriorInteract.RemovePrompt();
        }
        if(ClosestInteriorInteract != null)
        {
            ClosestInteriorInteract.UpdateActivated(Interactionable, Settings, ClosestLocation, ClosestInterior, LocationInteractable);
            ClosestInteriorInteract.AddPrompt();
        }
    }

    public void OnTeleportedInside(GameLocation gameLocation)
    {
        IsUpdatingSingleLocation = true;
        IsInsideTeleportInterior = true;
        CurrentTeleportInteriorLocation = gameLocation;
        InteriorUpdateLocations.Clear();
        InteriorUpdateLocations.Add(gameLocation);
        StartInteriorChecking();
    }
    public void OnTeleportedOutside(GameLocation gameLocation)
    {
        IsUpdatingSingleLocation = false;
        IsInsideTeleportInterior = false;
        CurrentTeleportInteriorLocation = null;
        InteriorUpdateLocations.Clear();
    }

    public void OnStartedInteriorInteract()
    {
        Player.IsSetDisabledControls = true;
    }

    public void OnEndedInteriorInteract()
    {
        Player.IsSetDisabledControls = false;
    }
}

