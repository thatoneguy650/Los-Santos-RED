using LosSantosRED.lsr.Interface;
using Mod;
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
    private bool IsRunningInteriorUpdate;
    private bool IsActive;
    private List<GameLocation> InteriorUpdateLocations = new List<GameLocation>();

    public InteriorManager(IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IInteriorManageable player)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = player;
    }
    public void Setup()
    {

    }
    public void Update()
    {
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
            else
            {
                gameLocation.Interior.RemoveButtonPrompts();
            }

        }
        InteriorUpdateLocations.RemoveAll(x => !x.IsActivated || !x.IsNearby || x.DistanceToPlayer >= 50f);
    }
    public void Dispose()
    {

    }
    private void StartInteriorChecking()
    {
        if(IsRunningInteriorUpdate)
        {
            return;
        }
        GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole($"Interior StartInteriorChecking");
                IsRunningInteriorUpdate = true;
                while (IsActive && EntryPoint.ModController.IsRunning)
                {
                    foreach(GameLocation gameLocation in InteriorUpdateLocations.ToList())
                    {
                        gameLocation.Interior.UpdateInteracts();
                    }
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
}

