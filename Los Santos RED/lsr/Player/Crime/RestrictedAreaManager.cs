using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RestrictedAreaManager 
{
    private IRestrictedAreaManagable Player;
    private ILocationInteractable LocationInteractable;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private uint GameTimeLastReportedCamera;

    public RestrictedArea CurrentRestrictedArea { get; private set; }

    public RestrictedAreaManager(IRestrictedAreaManagable player, ILocationInteractable locationInteractable, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        World = world;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }

    public void Update()
    {








        foreach (GameLocation gl in World.Places.ActiveLocations.ToList())
        {
            if(gl.RestrictedAreas == null || gl.RestrictedAreas.RestrictedAreasList == null || !gl.RestrictedAreas.RestrictedAreasList.Any())
            {
                continue;
            }
            gl.RestrictedAreas.Update(LocationInteractable, World);
            GameFiber.Yield();
        }

        GameLocation restrictedLocation = World.Places.ActiveLocations.Where(x => x.RestrictedAreas != null && x.RestrictedAreas.IsPlayerViolating()).FirstOrDefault();
        if (!Player.Violations.CanEnterRestrictedAreas && restrictedLocation != null)
        {
            CurrentRestrictedArea = restrictedLocation.RestrictedAreas.RestrictedAreasList.Where(x => x.IsPlayerViolating).FirstOrDefault();
            if (CurrentRestrictedArea?.CanSeeOnCameras == true && Game.GameTime - GameTimeLastReportedCamera >= 20000)
            {
                Player.OnSeenInRestrictedAreaOnCamera();
                GameTimeLastReportedCamera = Game.GameTime;
            }
        }
        else
        {
            CurrentRestrictedArea = null;
        }



    }
}

