using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StaticPlaces
{
    private Places Places;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IInteriors Interiors;
    private IShopMenus ShopMenus;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private IWeapons Weapons;
    private IZones Zones;
    private IStreets Streets;
    private IGangs Gangs;
    private IAgencies Agencies;
    private ITimeReportable Time;
    private INameProvideable Names;
    public StaticPlaces(Places places, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteriors interiors, IShopMenus shopMenus, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, IZones zones, IStreets streets, IGangs gangs, IAgencies agencies, ITimeReportable time, INameProvideable names)
    {
        Places = places;
        PlacesOfInterest = placesOfInterest;
        World = world;
        Interiors = interiors;
        ShopMenus = shopMenus;
        Settings = settings;
        Crimes = crimes;
        Weapons = weapons;
        Zones = zones;
        Streets = streets;
        Gangs = gangs;
        Agencies = agencies;
        Time = time;
        Names = names;
    }
    public void Setup()
    {
        //need to combine these
        foreach (BasicLocation basicLocation in PlacesOfInterest.AllLocations())
        {
            basicLocation.StoreData(Zones, Streets);
        }
        foreach (InteractableLocation tl in PlacesOfInterest.InteractableLocations())
        {
            tl.StoreData(ShopMenus, Agencies);
        }
        foreach (ILocationGangAssignable tl in PlacesOfInterest.GangAssignableLocations())
        {
           
            tl.StoreData(Gangs, ShopMenus);
        }
        foreach (ILocationSetupable ps in PlacesOfInterest.LocationsToSetup())
        {
            ps.Setup(Crimes,Names);
        }
    }
    public void ActivateLocations()
    {
        int LocationsCalculated = 0;
        if (EntryPoint.ModController.IsRunning)
        {
            foreach (BasicLocation gl in PlacesOfInterest.AllLocations())
            {
                if (gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5) && gl.IsEnabled && gl.IsCorrectMap(World.IsMPMapLoaded) && gl.CanActivate)// ((World.IsMPMapLoaded && gl.IsOnMPMap) || (!World.IsMPMapLoaded && gl.IsOnSPMap)))
                {
                    if (!gl.IsActivated)
                    {
                        gl.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
                        GameFiber.Yield();
                    }
                }
                else
                {
                    if (gl.IsActivated)
                    {
                        gl.Deactivate();
                        GameFiber.Yield();
                    }
                }
                if (Settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap)
                {
                    if (!gl.IsActivated && gl.IsEnabled && gl.CanActivate && gl.IsBlipEnabled && !gl.Blip.Exists() && gl.IsSameState(EntryPoint.FocusZone?.State) && gl.IsCorrectMap(World.IsMPMapLoaded))//(EntryPoint.FocusZone == null || EntryPoint.FocusZone.State == gl.StateLocation))
                    {
                        gl.ActivateBlip(Time, World);
                    }
                    else if (!gl.IsEnabled && gl.Blip.Exists())
                    {
                        gl.DeactivateBlip();
                    }
                    else if (gl.IsEnabled && gl.IsBlipEnabled && gl.Blip.Exists() && !gl.IsSameState(EntryPoint.FocusZone?.State))
                    {
                        gl.DeactivateBlip();
                    }
                    else
                    {
                        if (gl.IsEnabled && gl.IsBlipEnabled)
                        {
                            gl.UpdateBlip(Time);
                        }
                    }
                }
                else
                {
                    if (!gl.IsActivated && gl.Blip.Exists())
                    {
                        gl.DeactivateBlip();
                    }
                }
                LocationsCalculated++;
                if (LocationsCalculated >= 7)//20//50//20//5
                {
                    LocationsCalculated = 0;
                    GameFiber.Yield();
                }
                if (!EntryPoint.ModController.IsRunning)
                {
                    break;
                }
            }
        }
    }
    public void Update()
    {
        if (EntryPoint.ModController.IsRunning)
        {
            int updated = 0;
            foreach (BasicLocation gl in Places.ActiveLocations.ToList())
            {
                gl.Update(Time);
                updated++;
                if (updated >= 5)
                { 
                    GameFiber.Yield();
                    updated = 0;
                }
            }
        }
    }
    public void Dispose()
    {
        foreach (BasicLocation loc in Places.ActiveLocations.ToList())
        {
            loc.Deactivate();
        }
        foreach (BasicLocation gl in PlacesOfInterest.AllLocations())
        {
            gl.DeactivateBlip();
        }
    }
    public void ActivateLocation(ILocationRespawnable respawnableLocation)
    {
        if (!respawnableLocation.IsActivated)
        {
            respawnableLocation.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
        }   
    }
    public void SetGangLocationActive(string iD, bool setEnabled)
    {
        foreach (GangDen gl in PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.AssociatedGang?.ID == iD))
        {
            gl.IsAvailableForPlayer = setEnabled;
            gl.IsBlipEnabled = setEnabled;

            if (setEnabled)
            {
                gl.ActivateBlip(Time, World);
            }
            else
            {
                gl.DeactivateBlip();
            }
            //gl.IsEnabled = setEnabled;
        }
    }
    public void AddAllBlips()
    {
        foreach (BasicLocation basicLocation in PlacesOfInterest.AllLocations())
        {
            if(!basicLocation.IsActivated && basicLocation.IsEnabled)
            {
                basicLocation.ActivateBlip(Time, World);
            }
        }
    }
}

