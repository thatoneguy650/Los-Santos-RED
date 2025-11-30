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
    private ITimeControllable Time;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private uint GameTimeLastReportedCamera;

    private RestrictedArea CurrentRestrictedArea;
    private Interior CurrentRestrictedInterior;
    public bool IsTrespassing { get; private set; }
    public bool IsCivilianReactableTrespassing { get; private set; }
    public bool IsTrespassingOnMilitaryBase { get; private set; }
    public RestrictedAreaManager(IRestrictedAreaManagable player, ILocationInteractable locationInteractable, IEntityProvideable world, ISettingsProvideable settings, ITimeControllable time)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        World = world;
        Settings = settings;
        Time = time;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Update()
    {

        if (EntryPoint.IsLSPDFRIntegrationEnabled)
        {
            return;
        }

        foreach (GameLocation gl in World.Places.ActiveLocations.ToList())
        {
            if(gl.RestrictedAreas != null && gl.RestrictedAreas.RestrictedAreasList != null && gl.RestrictedAreas.RestrictedAreasList.Any())
            {
                gl.RestrictedAreas.Update(LocationInteractable, World);
                GameFiber.Yield();
            }
            if(gl.RestrictedAreas != null && gl.RestrictedAreas.VanillaRestrictedAreas != null && gl.RestrictedAreas.VanillaRestrictedAreas.Any())
            {
                gl.RestrictedAreas.UpdateVanilla(LocationInteractable, World);
                GameFiber.Yield();
            }
        }
        IsTrespassing = false;
        IsTrespassingOnMilitaryBase = false;
        IsCivilianReactableTrespassing = false;
        if (Player.Violations.CanEnterRestrictedAreas)
        {        
            return;
        }
        UpdateInteriorRestrictions();
        UpdateLocationRestrictions();
    }
    private void UpdateInteriorRestrictions()
    {
        if (!Player.CurrentLocation.IsInside)
        {
            return;
        }
        bool IsOpen = Player.CurrentLocation.CurrentInterior.GameLocation?.IsOpen(Time.CurrentHour) == true;

        if (Player.CurrentLocation.CurrentInterior.IsRestricted || (Player.CurrentLocation.CurrentInterior.IsTrespassingWhenClosed && !IsOpen))
        {


            IsTrespassing = true;
            if (Player.CurrentLocation.CurrentInterior.IsCivilianReactableRestricted || (Player.CurrentLocation.CurrentInterior.IsTrespassingWhenClosed && !IsOpen))
            {
                IsCivilianReactableTrespassing = true;
            }
        }
    }
    private void UpdateLocationRestrictions()
    {
        if (EntryPoint.IsLSPDFRIntegrationEnabled)
        {
            return;
        }
        GameLocation restrictedLocation = World.Places.ActiveLocations.Where(x => x.RestrictedAreas != null && (x.RestrictedAreas.IsPlayerViolating() || x.RestrictedAreas.IsPlayerViolatingVanilla())).FirstOrDefault();
        if (restrictedLocation == null)
        {
            return;
        }
        RestrictedArea CurrentRestrictedArea = restrictedLocation.RestrictedAreas.RestrictedAreasList.Where(x => x.IsPlayerViolating).FirstOrDefault();
        if (CurrentRestrictedArea != null)
        {
            if (CurrentRestrictedArea.CanSeeOnCameras == true && Game.GameTime - GameTimeLastReportedCamera >= 20000)
            {
                Player.OnSeenInRestrictedAreaOnCamera(CurrentRestrictedArea.IsCivilianReactableRestricted);
                GameTimeLastReportedCamera = Game.GameTime;
            }
            IsTrespassing = true;
            if(CurrentRestrictedArea.IsCivilianReactableRestricted)
            {
                IsCivilianReactableTrespassing = true;
            }
        }
        VanillaRestrictedArea CurrentVanillaRestrictedArea = restrictedLocation.RestrictedAreas.VanillaRestrictedAreas.Where(x => x.IsPlayerViolating).FirstOrDefault();
        if (CurrentVanillaRestrictedArea != null)
        {
            IsTrespassing = true;
        }
        if(IsTrespassing && restrictedLocation.GetType() == typeof(MilitaryBase))
        {
            IsTrespassingOnMilitaryBase = true;
        }
    }
}

