using LosSantosRED.lsr.Interface;
using Mod;
using NAudio.Wave;
using Rage;
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
    public void Update()
    {

        if (IsUpdatingSingleLocation)
        {
            return;
        }
        foreach(GameLocation gameLocation in World.Places.ActiveLocations.ToList())
        {
            if(!gameLocation.HasInterior || gameLocation.Interior == null || gameLocation.Interior.IsTeleportEntry)
            {
                continue;
            }
            if (gameLocation.Interior.InteractPoints == null || !gameLocation.Interior.InteractPoints.Any())
            {
                continue;
            }
            if(gameLocation.IsActivated && gameLocation.DistanceToPlayer <= 50f)
            {
                if(!InteriorUpdateLocations.Any(x=> x.Interior?.LocalID == gameLocation.Interior?.LocalID))
                {
                    InteriorUpdateLocations.Add(gameLocation);
                }
                StartInteriorChecking();
            }
            //else
            //{
            //    gameLocation.Interior.RemoveButtonPrompts();
            //}

        }
        InteriorUpdateLocations.RemoveAll(x => !x.IsActivated || !x.IsNearby || x.DistanceToPlayer >= 50f);
    }
    public void Dispose()
    {
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
        PrevClosestInteriorInteract = ClosestInteriorInteract;
        GameFiber.Yield();
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
        InteriorUpdateLocations.Clear();
        InteriorUpdateLocations.Add(gameLocation);
        StartInteriorChecking();
    }
    public void OnTeleportedOutside(GameLocation gameLocation)
    {
        IsUpdatingSingleLocation = false;
        InteriorUpdateLocations.Clear();
    }
}

