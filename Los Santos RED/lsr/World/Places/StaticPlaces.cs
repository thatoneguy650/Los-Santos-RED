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
    private ITimeControllable Time;
    private INameProvideable Names;
    private IPlateTypes PlateTypes;
    private IModItems ModItems;

    private IPedGroups PedGroups;
    private IJurisdictions Jurisdictions;
    private IGangTerritories GangTerritories;
    private ILocationTypes LocationTypes;
    private IOrganizations Associations;
    private IContacts Contacts;
    private IIssuableWeapons IssuableWeapons;
    private IHeads Heads;
    private IDispatchablePeople DispatchablePeople;
    private IClothesNames ClothesNames;

    public StaticPlaces(Places places, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteriors interiors, IShopMenus shopMenus, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, IZones zones, IStreets streets, IGangs gangs,
        IAgencies agencies, ITimeControllable time, INameProvideable names, IPedGroups pedGroups, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ILocationTypes locationTypes, IPlateTypes plateTypes, 
        IOrganizations associations, IContacts contacts, IModItems modItems, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, IClothesNames clothesNames)
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
        PedGroups = pedGroups;
        Jurisdictions = jurisdictions;
        GangTerritories = gangTerritories;
        LocationTypes= locationTypes;
        PlateTypes = plateTypes;
        Associations = associations;
        Contacts = contacts;
        ModItems = modItems;
        IssuableWeapons = issuableWeapons;
        Heads = heads;
        DispatchablePeople= dispatchablePeople;
        ClothesNames = clothesNames;
    }
    public void Setup(IInteractionable player, ILocationInteractable locationInteractable)
    {
        foreach (GameLocation tl in PlacesOfInterest.InteractableLocations())
        {
            tl.StoreData(ShopMenus, Agencies, Gangs,Zones, Jurisdictions, GangTerritories, Names, Crimes, PedGroups, World, Streets, LocationTypes, Settings, PlateTypes, Associations, Contacts, Interiors, locationInteractable, ModItems, Weapons, Time, PlacesOfInterest, IssuableWeapons, Heads, DispatchablePeople);
        }
        foreach (ILocationSetupable ps in PlacesOfInterest.LocationsToSetup())
        {
            ps.Setup();
        }
        foreach(Interior interior in Interiors.PossibleInteriors.AllInteriors())
        {
            interior.Setup(player, PlacesOfInterest, Settings, locationInteractable, ModItems, ClothesNames);
        }

        foreach(StoredSpawn spawnPlace in PlacesOfInterest.PossibleLocations.StoredSpawns)
        {
            spawnPlace.Setup();
        }
    }
    public void ActivateLocations()
    {
        int LocationsCalculated = 0;
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        foreach (GameLocation gl in PlacesOfInterest.AllLocations())
        {
            gl.CheckActivation(World, Interiors,Settings,Crimes,Weapons,Time);
            LocationsCalculated++;
            if (LocationsCalculated >= 7)//7)//7//20//50//20//5
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
    public void Update()
    {
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        int updated = 0;
        foreach (GameLocation gl in Places.ActiveLocations.ToList())
        {
            gl.Update(Time);
            updated++;
            if (updated >= 15)//5
            { 
                GameFiber.Yield();
                updated = 0;
            }
            if (!EntryPoint.ModController.IsRunning)
            {
                break;
            }
        }     
    }
    public void Dispose()
    {
        foreach (GameLocation loc in Places.ActiveLocations.ToList())
        {
            loc.Deactivate(true);
        }
        foreach (GameLocation gl in PlacesOfInterest.AllLocations())
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
            if(gl.IsActivated)
            {
                gl.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
            }
           // gl.IsBlipEnabled = setEnabled;

            //if (setEnabled)
            //{
             //   gl.ActivateBlip(Time, World);
            //}
            //else
            //{
            //    gl.DeactivateBlip();
            //}
            //gl.IsEnabled = setEnabled;
            //EntryPoint.WriteToConsole($"SetGangLocationActive {iD} setEnabled:{setEnabled}");
        }
    }
    public void AddAllBlips()
    {
        foreach (GameLocation basicLocation in PlacesOfInterest.AllLocations())
        {
            if(!basicLocation.IsActivated && basicLocation.IsEnabled && basicLocation.IsBlipEnabled)
            {
                basicLocation.ActivateBlip(Time, World);
            }
        }
    }

    public void Reset()
    {
        foreach (GameLocation basicLocation in PlacesOfInterest.AllLocations())
        {
            basicLocation.Reset();
        }
    }
    public void DebugDeactivateAllLocations()
    {
        foreach (GameLocation loc in Places.ActiveLocations.ToList())
        {
            loc.Deactivate(true);
        }
    }
}

