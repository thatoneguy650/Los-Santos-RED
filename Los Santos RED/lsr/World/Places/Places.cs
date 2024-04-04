using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
//using LosSantosRED.lsr.Util.Locations;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Places
{
    private IZones Zones;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private IInteriors Interiors;
    private IShopMenus ShopMenus;
    private IGangTerritories GangTerritories;
    private IGangs Gangs;
    private IStreets Streets;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IAgencies Agencies;
    private IOrganizations Associations;
    private IContacts Contacts;
    private IModItems ModItems;
    public Places(IEntityProvideable world, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IWeapons weapons, ICrimes crimes, ITimeControllable time, IShopMenus shopMenus,
        IInteriors interiors, IGangs gangs, IGangTerritories gangTerritories, IStreets streets, IAgencies agencies, INameProvideable names, IPedGroups pedGroups, ILocationTypes locationTypes, IPlateTypes plateTypes, 
        IOrganizations associations, IContacts contacts, IModItems modItems, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, IClothesNames clothesNames)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Settings = settings;
        Weapons = weapons;
        Crimes = crimes;
        Time = time;
        Interiors = interiors;
        ShopMenus = shopMenus;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Streets = streets;
        Agencies = agencies;
        Associations = associations;
        Contacts = contacts;
        ModItems = modItems;
        DynamicPlaces = new DynamicPlaces(this, PlacesOfInterest, World, Interiors, ShopMenus, Settings, Crimes, Weapons, Time);
        StaticPlaces = new StaticPlaces(this, PlacesOfInterest, World, Interiors, ShopMenus, Settings, Crimes, Weapons, Zones,Streets,Gangs,Agencies, Time, names, pedGroups, Jurisdictions, 
            GangTerritories, locationTypes, plateTypes, Associations, Contacts, ModItems, issuableWeapons,heads,dispatchablePeople, clothesNames);
    }
    public List<GameLocation> ActiveLocations { get; private set; } = new List<GameLocation>();
    public DynamicPlaces DynamicPlaces { get; private set; }
    public StaticPlaces StaticPlaces { get; private set; }
    public void Setup(IInteractionable player, ILocationInteractable locationInteractable)
    {
        foreach (Zone zone in Zones.ZoneList)
        {
            zone.StoreData(GangTerritories, Jurisdictions, ShopMenus);
            GameFiber.Yield();
        }
        StaticPlaces.Setup(player, locationInteractable);
        DynamicPlaces.Setup();
    }
    public void Dispose()
    {
        StaticPlaces.Dispose();
        DynamicPlaces.Dispose();
    }
    public void ActivateLocations()
    {

        StaticPlaces.ActivateLocations();
        GameFiber.Yield();
        DynamicPlaces.ActivateLocations();
    }
    public void UpdateLocations()
    {
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        int updated = 0;
        foreach (GameLocation gl in ActiveLocations.ToList())
        {
            gl.Update(Time);
            updated++;
            if (updated >= 5)//15)//5
            {
                GameFiber.Yield();
                updated = 0;
            }
            if (!EntryPoint.ModController.IsRunning)
            {
                break;
            }
        }
        //EntryPoint.WriteToConsole($"UPDATE LOCATIONS RAN {updated}");
    }
    public void Reset()
    {
        StaticPlaces.Reset();
    }


}
