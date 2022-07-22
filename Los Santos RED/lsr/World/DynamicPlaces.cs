using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DynamicPlaces
{
    private Places Places;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IInteriors Interiors;
    private IShopMenus ShopMenus;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private IWeapons Weapons;

    private List<VendingMachine> ActiveVendingMachines = new List<VendingMachine>();
    private List<string> VendingMachinesModelNames = new List<string>();
    private List<uint> VendingMachinessModelHashes = new List<uint>();
    private List<GasPump> ActiveGasPumps = new List<GasPump>();
    private List<string> GasPumpsModelNames = new List<string>();
    private List<uint> GasPumpsModelHashes = new List<uint>();

    public DynamicPlaces(Places places, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteriors interiors, IShopMenus shopMenus, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons)
    {
        Places = places;
        PlacesOfInterest = placesOfInterest;
        World = world;
        Interiors = interiors;
        ShopMenus = shopMenus;
        Settings = settings;
        Crimes = crimes;
        Weapons = weapons;
    }

    public void Setup()
    {
        VendingMachinesModelNames = new List<string>() { "prop_vend_soda_01","prop_vend_soda_02","prop_vend_coffe_01","prop_vend_condom_01","prop_vend_fags_01","prop_vend_snak_01","prop_vend_water_01"};

        VendingMachinessModelHashes = new List<uint>() {0x3b21c5e7,0x426a547c,0x418f055a};

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
    }
    public void Dispose()
    {

    }
    public void ActivateLocations()
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
                Places.ActiveInteractableLocations.Add(newVend);
                ActiveVendingMachines.Add(newVend);
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
                GasStation ClosestStation = (GasStation)Places.ActiveInteractableLocations.Where(x => x.GetType() == typeof(GasStation)).OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault();//maybe store anyothe list of stations?
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
                Places.ActiveInteractableLocations.Add(newGasPump);
                ActiveGasPumps.Add(newGasPump);
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
                if (Places.ActiveInteractableLocations.Contains(gl))
                {
                    Places.ActiveInteractableLocations.Remove(gl);
                }
                if (ActiveVendingMachines.Contains(gl))
                {
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
                if (Places.ActiveInteractableLocations.Contains(gl))
                {
                    Places.ActiveInteractableLocations.Remove(gl);
                }
                if (ActiveGasPumps.Contains(gl))
                {
                    ActiveGasPumps.Remove(gl);
                    gl.Dispose();
                    GameFiber.Yield();
                }
            }
        }
    }


}

