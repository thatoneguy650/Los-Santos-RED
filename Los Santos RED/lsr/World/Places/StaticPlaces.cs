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
        foreach (BasicLocation basicLocation in PlacesOfInterest.GetAllLocations())
        {
            Zone placeZone = Zones.GetZone(basicLocation.EntrancePosition);
            string betweener = "";
            string zoneString = "";
            if (placeZone != null)
            {
                if (placeZone.IsSpecificLocation)
                {
                    betweener = $"near";
                }
                else
                {
                    betweener = $"in";
                }
                zoneString = $"~p~{placeZone.DisplayName}~s~";
            }
            string streetName = Streets.GetStreetNames(basicLocation.EntrancePosition);
            string StreetNumber = "";
            if (streetName == "")
            {
                betweener = "";
            }
            else
            {
                StreetNumber = NativeHelper.CellToStreetNumber(basicLocation.CellX, basicLocation.CellY);
            }
            string LocationName = $"{StreetNumber} {streetName} {betweener} {zoneString}".Trim();
            string ShortLocationName = $"{StreetNumber} {streetName}".Trim();
            basicLocation.FullStreetAddress = LocationName;
            basicLocation.StreetAddress = ShortLocationName;
            basicLocation.ZoneName = zoneString;
            basicLocation.CellX = (int)(basicLocation.EntrancePosition.X / EntryPoint.CellSize);
            basicLocation.CellY = (int)(basicLocation.EntrancePosition.Y / EntryPoint.CellSize);
        }
        foreach (GangDen tl in PlacesOfInterest.PossibleLocations.GangDens)
        {
            tl.Menu = ShopMenus.GetMenu(tl.MenuID);
            tl.AssociatedGang = Gangs.GetGang(tl.GangID);
            tl.ButtonPromptText = $"Enter {tl.AssociatedGang?.ShortName} {tl.AssociatedGang?.DenName}";
        }
        foreach (InteractableLocation tl in PlacesOfInterest.GetAllInteractableLocations())
        {
            tl.Menu = ShopMenus.GetMenu(tl.MenuID);
        }
        foreach (PoliceStation ps in PlacesOfInterest.PossibleLocations.PoliceStations)
        {
            if (ps.AssignedAgencyID != null)
            {
                ps.AssignedAgency = Agencies.GetAgency(ps.AssignedAgencyID);
            }
        }
        foreach (Hospital ps in PlacesOfInterest.PossibleLocations.Hospitals)
        {
            if (ps.AssignedAgencyID != null)
            {
                ps.AssignedAgency = Agencies.GetAgency(ps.AssignedAgencyID);
            }
        }
        foreach (FireStation ps in PlacesOfInterest.PossibleLocations.FireStations)
        {
            if (ps.AssignedAgencyID != null)
            {
                ps.AssignedAgency = Agencies.GetAgency(ps.AssignedAgencyID);
            }
        }
    }
    public void ActivateLocations()
    {
        int LocationsCalculated = 0;
        foreach (InteractableLocation gl in PlacesOfInterest.GetAllInteractableLocations())
        {
            if (gl.IsOpen(Time.CurrentHour) && gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5) && gl.IsEnabled)// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
            {
                if (!Places.ActiveInteractableLocations.Contains(gl))
                {
                    Places.ActiveInteractableLocations.Add(gl);
                    gl.Setup(Interiors, Settings, Crimes, Weapons);
                    World.Pedestrians.AddEntity(gl.Merchant);
                    World.AddBlip(gl.Blip);
                    GameFiber.Yield();
                }
            }
            else
            {
                if (Places.ActiveInteractableLocations.Contains(gl))
                {
                    Places.ActiveInteractableLocations.Remove(gl);
                    gl.Dispose();
                    GameFiber.Yield();
                }
            }
            LocationsCalculated++;
            if (LocationsCalculated >= 20)//50//20//5
            {
                LocationsCalculated = 0;
                GameFiber.Yield();
            }
        }
        LocationsCalculated = 0;
        foreach (BasicLocation gl in PlacesOfInterest.GetAllBasicLocations())
        {
            if (gl.IsOpen(Time.CurrentHour) && gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5) && gl.IsEnabled)// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
            {
                if (!Places.ActiveBasicLocations.Contains(gl))
                {
                    Places.ActiveBasicLocations.Add(gl);
                    gl.Setup(Interiors, Settings, Crimes, Weapons);
                    World.AddBlip(gl.Blip);
                    GameFiber.Yield();
                }
            }
            else
            {
                if (Places.ActiveBasicLocations.Contains(gl))
                {
                    Places.ActiveBasicLocations.Remove(gl);
                    gl.Dispose();
                    GameFiber.Yield();
                }
            }
            LocationsCalculated++;
            if (LocationsCalculated >= 20)//50//20//5
            {
                LocationsCalculated = 0;
                GameFiber.Yield();
            }
        }
    }
    public void UpdateLocations()
    {
        foreach (InteractableLocation gl in Places.ActiveInteractableLocations.ToList())
        {
            gl.Update();
            GameFiber.Yield();
        }
        foreach (BasicLocation gl in Places.ActiveBasicLocations.ToList())
        {
            gl.Update();
            GameFiber.Yield();
        }
    }
    public void Dispose()
    {
        foreach (InteractableLocation loc in Places.ActiveInteractableLocations)
        {
            loc.Dispose();
        }
        foreach (BasicLocation loc in Places.ActiveBasicLocations)
        {
            loc.Dispose();
        }
    }

    public void ActivateBasicLocation(BasicLocation gl)
    {
        if (!Places.ActiveBasicLocations.Contains(gl))
        {
            Places.ActiveBasicLocations.Add(gl);
            gl.Setup(Interiors, Settings, Crimes, Weapons);
            World.AddBlip(gl.Blip);
            GameFiber.Yield();
        }
    }
    public void SetGangLocationActive(string iD, bool v)
    {
        foreach (GangDen gl in PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.AssociatedGang?.ID == iD))
        {
            gl.IsEnabled = v;
        }
    }

}

