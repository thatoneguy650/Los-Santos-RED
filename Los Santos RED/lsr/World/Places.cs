using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Util.Locations;
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
    private ITimeReportable Time;
    private IInteriors Interiors;
    private IShopMenus ShopMenus;
    private IGangTerritories GangTerritories;
    private IGangs Gangs;
    private IStreets Streets;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private List<VendingMachine> ActiveVendingMachines = new List<VendingMachine>();


    private List<GasStation> ActiveGasStations = new List<GasStation>();

    private List<string> VendingMachinesModelNames = new List<string>();
    private List<uint> VendingMachinessModelHashes = new List<uint>();

    private List<GasPump> ActiveGasPumps = new List<GasPump>();

    private List<string> GasPumpsModelNames = new List<string>();
    private List<uint> GasPumpsModelHashes = new List<uint>();

    public Places(IEntityProvideable world, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IWeapons weapons, ICrimes crimes, ITimeReportable time, IShopMenus shopMenus, IInteriors interiors, IGangs gangs, IGangTerritories gangTerritories, IStreets streets)
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
    }
    public List<InteractableLocation> ActiveInteractableLocations { get; private set; } = new List<InteractableLocation>();
    public List<BasicLocation> ActiveBasicLocations { get; private set; } = new List<BasicLocation>();
    public void Setup()
    {
        foreach (Zone zone in Zones.ZoneList)
        {
            zone.Gangs = new List<Gang>();
            List<Gang> GangStuff = GangTerritories.GetGangs(zone.InternalGameName, 0);
            if(GangStuff != null)
            {
                zone.Gangs.AddRange(GangStuff);
            }
            
            zone.Agencies = new List<Agency>();
            List<Agency> LEAgency = Jurisdictions.GetAgencies(zone.InternalGameName, 0, ResponseType.LawEnforcement);
            if (LEAgency != null)
            {
                zone.Agencies.AddRange(LEAgency);
            }
            List<Agency> EMSAgencies = Jurisdictions.GetAgencies(zone.InternalGameName, 0, ResponseType.EMS);
            if (EMSAgencies != null)
            {
                zone.Agencies.AddRange(EMSAgencies);
            }
            List<Agency> FireAgencies = Jurisdictions.GetAgencies(zone.InternalGameName, 0, ResponseType.Fire);
            if (FireAgencies != null)
            {
                zone.Agencies.AddRange(FireAgencies);
            }

            zone.AssignedLEAgencyInitials = Jurisdictions.GetMainAgency(zone.InternalGameName, ResponseType.LawEnforcement)?.ColorInitials;
            Gang mainGang = GangTerritories.GetMainGang(zone.InternalGameName);
            if (mainGang != null)
            {
                zone.AssignedGangInitials = mainGang.ColorInitials;
            }
            else
            {
                zone.AssignedGangInitials = "";
            }
            Agency secondaryAgency = Jurisdictions.GetNthAgency(zone.InternalGameName, ResponseType.LawEnforcement, 2);
            if (secondaryAgency != null)
            {
                zone.AssignedSecondLEAgencyInitials = secondaryAgency.ColorInitials;
            }
            else
            {
                zone.AssignedSecondLEAgencyInitials = "";
            }
            GameFiber.Yield();
        }

        VendingMachinesModelNames = new List<string>()
            { "prop_vend_soda_01","prop_vend_soda_02","prop_vend_coffe_01","prop_vend_condom_01","prop_vend_fags_01","prop_vend_snak_01","prop_vend_water_01"};

        VendingMachinessModelHashes = new List<uint>()
            {0x3b21c5e7,0x426a547c,0x418f055a};


        GasPumpsModelNames = new List<string>()
        { "prop_gas_pump_1a", //ron pump
        "prop_gas_pump_1b",// globe oil
        "prop_gas_pump_1c",// ltd
        "prop_gas_pump_1d",// xero
        "prop_gas_pump_old2",//old xero
        "prop_gas_pump_old3",//old LTD
        };

        GasPumpsModelHashes = new List<uint>()
        {
            0x7339e883,//ltd new
            0xf62c2b4b, //ltd old
            0x4fd621bc, //ron new
            0x885c12c7, //xero new
            0xe40106f5, //xero old
            0xe469f8b3, //globe old
            0x64ff4c0e ///globe new
        };


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
            basicLocation.StreetAddress = LocationName;
        }
        foreach(GangDen tl in PlacesOfInterest.PossibleLocations.GangDens)
        {
            tl.Menu = ShopMenus.GetMenu(tl.MenuID);
            tl.AssociatedGang = Gangs.GetGang(tl.GangID);
            tl.ButtonPromptText = $"Enter {tl.AssociatedGang?.ShortName} {tl.AssociatedGang?.DenName}";
        }
        foreach (InteractableLocation tl in PlacesOfInterest.GetAllInteractableLocations())
        {
            tl.Menu = ShopMenus.GetMenu(tl.MenuID);
        }
    }
    public void Dispose()
    {
        foreach (InteractableLocation loc in ActiveInteractableLocations)
        {
            loc.Dispose();
        }
        foreach (BasicLocation loc in ActiveBasicLocations)
        {
            loc.Dispose();
        }
    }
    public void ActiveNearLocations()
    {
        int LocationsCalculated = 0;
        foreach (InteractableLocation gl in PlacesOfInterest.GetAllInteractableLocations())
        {
            if (gl.IsOpen(Time.CurrentHour) && gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5) && gl.IsEnabled)// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
            {
                if (!ActiveInteractableLocations.Contains(gl))
                {
                    ActiveInteractableLocations.Add(gl);
                    gl.Setup(Interiors, Settings, Crimes, Weapons);


                    World.Pedestrians.AddEntity(gl.Merchant);
                    World.AddBlip(gl.Blip);
                    GameFiber.Yield();
                }
            }
            else
            {
                if (ActiveInteractableLocations.Contains(gl))
                {
                    ActiveInteractableLocations.Remove(gl);
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
                if (!ActiveBasicLocations.Contains(gl))
                {
                    ActiveBasicLocations.Add(gl);
                    gl.Setup(Interiors, Settings, Crimes, Weapons);
                    World.AddBlip(gl.Blip);
                    GameFiber.Yield();
                }
            }
            else
            {
                if (ActiveBasicLocations.Contains(gl))
                {
                    ActiveBasicLocations.Remove(gl);
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

        GameFiber.Yield();
        UpdateGeneratedLocations();
    }
    public void UpdateNearLocations()
    {
        foreach (InteractableLocation gl in ActiveInteractableLocations.ToList())
        {
            gl.Update();
            GameFiber.Yield();
        }
    }
    private void UpdateGeneratedLocations()
    {
        List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList();
        foreach (Rage.Object obj in Objects)
        {
            if (obj.Exists())
            {
                string modelName = obj.Model.Name.ToLower();
                Vector3 position = obj.Position;
                float heading = obj.Heading;
                uint hash = obj.Model.Hash;
                if (VendingMachinesModelNames.Contains(modelName) || VendingMachinessModelHashes.Contains(hash))
                {
                    ActivateVendingMachine(obj, modelName, position, heading);
                    GameFiber.Yield();
                }
                else if (GasPumpsModelNames.Contains(modelName) || GasPumpsModelHashes.Contains(hash))
                {
                    ActivateGasPump(obj, modelName, position, heading);
                    GameFiber.Yield();
                }
            }
        }
        GameFiber.Yield();
        RemoveInactiveVendingMachines();
        RemoveInactiveGasPumps();
    }
    private void ActivateVendingMachine(Rage.Object obj, string modelName, Vector3 position, float heading)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo <= 50f)
        {
            if (!ActiveVendingMachines.Any(x => x.EntrancePosition.DistanceTo2D(obj.Position) <= 0.2f))
            {
                ShopMenu vendingMenu = ShopMenus.GetVendingMenu(modelName);
                VendingMachine newVend = new VendingMachine(position, heading, vendingMenu.Name, vendingMenu.Name, vendingMenu.ID, obj) { Menu = vendingMenu };
                newVend.CanInteractWhenWanted = true;
                newVend.Setup(Interiors, Settings, Crimes, Weapons);
                World.AddBlip(newVend.Blip);
                ActiveInteractableLocations.Add(newVend);
                ActiveVendingMachines.Add(newVend);
                EntryPoint.WriteToConsole($"Nearby Vending {vendingMenu.Name} ADDED Props FOUND {modelName}", 5);
            }
        }
    }
    private void ActivateGasPump(Rage.Object obj, string modelName, Vector3 position, float heading)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo <= 50f)
        {
            if (!ActiveGasPumps.Any(x => x.EntrancePosition.DistanceTo2D(obj.Position) <= 0.2f))
            {
                //ShopMenu vendingMenu = ShopMenus.GetVendingMenu(modelName);
                GasStation ClosestStation = (GasStation)ActiveInteractableLocations.Where(x => x.GetType() == typeof(GasStation)).OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault();//maybe store anyothe list of stations?
                GasPump newGasPump;
                if (ClosestStation != null)
                {
                    newGasPump = new GasPump(position, heading, ClosestStation.Name, ClosestStation.Description, "None", obj, ClosestStation) { BannerImagePath = ClosestStation.BannerImagePath };
                }
                else
                {
                    newGasPump = new GasPump(position, heading, "Gas Pump", "Gas Pump", "None", obj, null) { };
                }
                newGasPump.CanInteractWhenWanted = true;
                newGasPump.Setup(Interiors, Settings, Crimes, Weapons);
                World.AddBlip(newGasPump.Blip);
                ActiveInteractableLocations.Add(newGasPump);
                ActiveGasPumps.Add(newGasPump);
                EntryPoint.WriteToConsole($"Nearby gas pump ADDED Props FOUND {modelName}", 5);
            }
        }
    }
    private void RemoveInactiveVendingMachines()
    {
        for (int i = ActiveVendingMachines.Count - 1; i >= 0; i--)
        {
            VendingMachine gl = ActiveVendingMachines[i];
            if (gl.DistanceToPlayer >= 100f)
            {
                if (ActiveInteractableLocations.Contains(gl))
                {
                    ActiveInteractableLocations.Remove(gl);
                }
                if (ActiveVendingMachines.Contains(gl))
                {
                    EntryPoint.WriteToConsole($"Nearby Vending {gl.Name} REMOVED", 5);
                    ActiveVendingMachines.Remove(gl);
                    gl.Dispose();
                    GameFiber.Yield();
                }
            }
        }
    }
    private void RemoveInactiveGasPumps()
    {
        for (int i = ActiveGasPumps.Count - 1; i >= 0; i--)
        {
            GasPump gl = ActiveGasPumps[i];
            if (gl.DistanceToPlayer >= 100f)
            {
                if (ActiveInteractableLocations.Contains(gl))
                {
                    ActiveInteractableLocations.Remove(gl);
                }
                if (ActiveGasPumps.Contains(gl))
                {
                    EntryPoint.WriteToConsole($"Nearby gas pump {gl.Name} REMOVED", 5);
                    ActiveGasPumps.Remove(gl);
                    gl.Dispose();
                    GameFiber.Yield();
                }
            }
        }
    }

    public void ActivateBasicLocation(BasicLocation gl)
    {
        if (!ActiveBasicLocations.Contains(gl))
        {
            ActiveBasicLocations.Add(gl);
            gl.Setup(Interiors, Settings, Crimes, Weapons);
            World.AddBlip(gl.Blip);
            GameFiber.Yield();
        }    
    }
    public void SetGangLocationActive(string iD, bool v)
    {
        foreach (GangDen gl in PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.AssociatedGang?.ID == iD))
        {
            EntryPoint.WriteToConsole($"Enabled Den {gl.Name}");
            gl.IsEnabled = v;
        }
    }
}
