using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
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
    public StaticPlaces(Places places, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteriors interiors, IShopMenus shopMenus, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, IZones zones, IStreets streets, IGangs gangs, IAgencies agencies, ITimeReportable time)
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
    }
    public void Setup()
    {
        foreach (BasicLocation basicLocation in PlacesOfInterest.AllLocations())
        {
            basicLocation.StoreData(Zones, Streets);
        }
        foreach (ILocationGangAssignable tl in PlacesOfInterest.GangAssignableLocations())
        {
            tl.StoreData(Gangs,ShopMenus);
        }
        foreach (InteractableLocation tl in PlacesOfInterest.InteractableLocations())
        {
            tl.StoreData(ShopMenus);
        }
        foreach (ILocationAgencyAssignable ps in PlacesOfInterest.AgencyAssignableLocations())
        {
            ps.StoreData(Agencies);
        }
    }
    public void ActivateLocations()
    {
        int LocationsCalculated = 0;
        foreach (BasicLocation gl in PlacesOfInterest.AllLocations())
        {
            if (gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5) && gl.IsEnabled)
            {
                if (!gl.IsActivated)
                {
                    //EntryPoint.WriteToConsole($"{gl.Name} ACTIVATE 1");
                    gl.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
                    GameFiber.Yield();
                }
            }
            else
            {
                if(gl.IsActivated)
                {
                    //EntryPoint.WriteToConsole($"{gl.Name} DEACTIVATE 1");
                    gl.Deactivate();
                    GameFiber.Yield();
                }
            }

            if(Settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap)
            {
                if (!gl.IsActivated && gl.IsEnabled && gl.IsBlipEnabled && !gl.Blip.Exists())
                {
                    //EntryPoint.WriteToConsole($"{gl.Name} ACTIVATE BLIP 1");
                    gl.ActivateBlip(Time, World);
                }
                else if (!gl.IsEnabled && gl.Blip.Exists())
                {
                    //EntryPoint.WriteToConsole($"{gl.Name} DEACTIVATE BLIP 1");
                    gl.DeactivateBlip();
                }
                else
                {
                    if (gl.IsEnabled && gl.IsBlipEnabled)
                    {
                        //EntryPoint.WriteToConsole($"{gl.Name} UPDATE BLIP 1");
                        gl.UpdateBlip(Time);
                    }
                }
            }
            else
            {
                if (!gl.IsActivated && gl.Blip.Exists())
                {
                    //EntryPoint.WriteToConsole($"{gl.Name} DEACTIVATE BLIP 2");
                    gl.DeactivateBlip();
                }
            }

            LocationsCalculated++;
            if (LocationsCalculated >= 7)//20//50//20//5
            {
                LocationsCalculated = 0;
                GameFiber.Yield();
            }

        }
    }
    public void Update()
    {
        foreach (BasicLocation gl in Places.ActiveLocations.ToList())
        {
            gl.Update(Time);
            GameFiber.Yield();
        }
    }
    public void Dispose()
    {
        foreach (BasicLocation loc in Places.ActiveLocations.ToList())
        {
            loc.Deactivate();
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
            gl.IsEnabled = setEnabled;
            //if (setEnabled)
            //{
            //    if (!gl.IsActivated)
            //    {
            //        EntryPoint.WriteToConsole("SetGangLocationActive I AM ACTIVATING ");
            //        gl.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
            //    }
            //}
            //else
            //{
            //    if (gl.IsActivated)
            //    {
            //        EntryPoint.WriteToConsole("SetGangLocationActive I AM DEACTIVATING ");
            //        gl.Deactivate();
            //    }
            //}

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

