using LosSantosRED.lsr.Interface;
using Microsoft.VisualBasic.ApplicationServices;
using Mod;
using Rage;
using Rage.Native;
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
    private ITimeControllable Time;

    private List<VendingMachine> ActiveVendingMachines = new List<VendingMachine>();
    private List<string> VendingMachinesModelNames = new List<string>();
    private List<uint> VendingMachinessModelHashes = new List<uint>();

    private List<GasPump> ActiveGasPumps = new List<GasPump>();
    private List<string> GasPumpsModelNames = new List<string>();
    private List<uint> GasPumpsModelHashes = new List<uint>();


    private List<ATMMachine> ActiveATMMachines = new List<ATMMachine>();
    private List<string> ATMModelNames = new List<string>();
    private List<uint> ATMModelHashes = new List<uint>();

    private List<CashRegister> ActiveCashRegisters = new List<CashRegister>();
    private List<string> CashRegisterModelNames = new List<string>();
    private List<uint> CashRegisterModelHashes = new List<uint>();

    public DynamicPlaces(Places places, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteriors interiors, IShopMenus shopMenus, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeControllable time)
    {
        Places = places;
        PlacesOfInterest = placesOfInterest;
        World = world;
        Interiors = interiors;
        ShopMenus = shopMenus;
        Settings = settings;
        Crimes = crimes;
        Weapons = weapons;
        Time = time;
    }

    public void Setup()
    {
        VendingMachinesModelNames = new List<string>() { "prop_vend_soda_01","prop_vend_soda_02","prop_vend_coffe_01","prop_vend_condom_01","prop_vend_fags_01","prop_vend_snak_01","prop_vend_water_01"};
        VendingMachinessModelHashes = new List<uint>() 
        {
            0x3b21c5e7,
            0x426a547c,
            0x418f055a
        };
        GasPumpsModelNames = new List<string>()
        { 
            "prop_gas_pump_1a", //ron pump
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
            0x64ff4c0e, ///globe new


            0x0f3b57cf//lc globe
        };
        ATMModelNames = new List<string>() 
        {
            "prop_atm_01", 
            "prop_fleeca_atm",
            "prop_atm_02", 
            "prop_atm_03" 
        };
        ATMModelHashes = new List<uint>()
        {
            3424098598,//prop_atm_01
            3168729781, //prop_atm_02
            2930269768, //prop_atm_03
            506770882, //prop_fleeca_atm
        };
        //prop_fleeca_atm FLEECA
        //prop_atm_01 standalone ATM
        //prop_atm_02 LOM BANK
        //prop_atm_03 generic
        CashRegisterModelNames = new List<string>()
        { 
            "p_till_01_s", 
            "prop_till_01_dam", 
            "prop_till_01",
            "prop_till_02", 
            "prop_till_03" 
        };
        CashRegisterModelHashes = new List<uint>()
        {
            892543765,
            3940037152, 
            303280717, 
            534367705,
            759654580,
        };
    }
    public void Dispose()
    {

    }

    public void OnLookedAtObject(Rage.Object currentLookedAtObject)
    {
        if (!Settings.SettingsManager.WorldSettings.CreateObjectLocationsFromScanning)
        { 
            CheckObject(currentLookedAtObject);
        }
    }


    public void ActivateLocations()
    {
        if (EntryPoint.ModController.IsRunning)
        {
            if(Settings.SettingsManager.WorldSettings.CreateObjectLocationsFromScanning)
            { 
                List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList(); //EntryPoint.ModController.AllObjects.ToList();//Rage.World.GetAllObjects().ToList();
                GameFiber.Yield();
                int checkedObjects = 0;
                foreach (Rage.Object obj in Objects)
                {
                    CheckObject(obj);
                    checkedObjects++;
                    if (checkedObjects > 20)//10
                    {
                        GameFiber.Yield();
                        checkedObjects = 0;
                    }
                    if (!EntryPoint.ModController.IsRunning)
                    {
                        break;
                    }
                }
                GameFiber.Yield();
            }
            RemoveInactiveVendingMachines();
            GameFiber.Yield();
            RemoveInactiveGasPumps();
            GameFiber.Yield();
            RemoveInactiveATM();
            GameFiber.Yield();
            RemoveInactiveCashRegisters();
        }
    }
    private void CheckObject(Rage.Object obj)
    {
        if (obj.Exists() && !NativeFunction.Natives.HAS_OBJECT_BEEN_BROKEN<bool>(obj, false))
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
                ActivateGasPump(obj, modelName, position, heading, true);
                GameFiber.Yield();
            }
            else if (ATMModelNames.Contains(modelName) || ATMModelHashes.Contains(hash))
            {
                ActivateATMMachine(obj, modelName, position, heading);
                GameFiber.Yield();
            }
            else if (CashRegisterModelNames.Contains(modelName) || CashRegisterModelHashes.Contains(hash))
            {
                ActiveCashRegister(obj, modelName, position, heading);
                GameFiber.Yield();
            }
        }
    }
    private void ActiveCashRegister(Rage.Object obj, string modelName, Vector3 position, float heading)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo > 50f)
        {
            return;
        }
        if (ActiveCashRegisters.Any(x => x.EntrancePosition.DistanceTo2D(obj.GetOffsetPositionFront(0.5f)) <= 0.2f))
        {
            return;
        }
        //ShopMenu vendingMenu = ShopMenus.GetVendingMenu(modelName);
        Vector3 EntrancePos = obj.GetOffsetPositionFront(0.5f);

        GameLocation closestLocation = Places.ActiveLocations.OrderBy(x => x.EntrancePosition.DistanceTo2D(obj.GetOffsetPositionFront(0.5f))).FirstOrDefault();//maybe store anyothe list of stations?
        int newRegisterCash = RandomItems.GetRandomNumberInt(200, 1200); //3500;
        if(closestLocation != null)
        {
            newRegisterCash = RandomItems.GetRandomNumberInt(closestLocation.RegisterCashMin, closestLocation.RegisterCashMax);
        }
        CashRegister newVend = new CashRegister(EntrancePos, heading, "Cash Register", "Cash Register", "", obj, newRegisterCash) { OpenTime = 0, CloseTime = 24 };
        newVend.CanInteractWhenWanted = true;
        newVend.StoreData(ShopMenus, World.ModDataFileManager.Agencies, World.ModDataFileManager.Gangs, World.ModDataFileManager.Zones, World.ModDataFileManager.Jurisdictions, World.ModDataFileManager.GangTerritories, World.ModDataFileManager.Names,
    World.ModDataFileManager.Crimes, World.ModDataFileManager.RelationshipGroups, World, World.ModDataFileManager.Streets, World.ModDataFileManager.LocationTypes, Settings, World.ModDataFileManager.PlateTypes, World.ModDataFileManager.Organizations, World.ModDataFileManager.Contacts, Interiors, World.LocationInteractable, World.ModDataFileManager.ModItems, World.ModDataFileManager.Weapons, Time, PlacesOfInterest, World.ModDataFileManager.IssueableWeapons, World.ModDataFileManager.Heads, World.ModDataFileManager.DispatchablePeople);

        newVend.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
        ActiveCashRegisters.Add(newVend);

        EntryPoint.WriteToConsole($"Activate CashRegister {newVend.Name} {newVend.EntrancePosition}");

    }
    private void ActivateVendingMachine(Rage.Object obj, string modelName, Vector3 position, float heading)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo > 50f)
        {
            return;
        }
        if (ActiveVendingMachines.Any(x => x.EntrancePosition.DistanceTo2D(obj.GetOffsetPositionFront(0.5f)) <= 0.2f))
        {
            return;
        }

        ShopMenu vendingMenu = ShopMenus.GetVendingMenu(modelName);
        Vector3 EntrancePos = obj.GetOffsetPositionFront(0.5f);
        VendingMachine newVend = new VendingMachine(EntrancePos, heading, vendingMenu.Name, vendingMenu.Name, vendingMenu.ID, obj) { Menu = vendingMenu, OpenTime = 0, CloseTime = 24 };
        newVend.CanInteractWhenWanted = true;

        newVend.StoreData(ShopMenus, World.ModDataFileManager.Agencies, World.ModDataFileManager.Gangs, World.ModDataFileManager.Zones, World.ModDataFileManager.Jurisdictions, World.ModDataFileManager.GangTerritories, World.ModDataFileManager.Names,
            World.ModDataFileManager.Crimes, World.ModDataFileManager.RelationshipGroups, World, World.ModDataFileManager.Streets, World.ModDataFileManager.LocationTypes, Settings, World.ModDataFileManager.PlateTypes, World.ModDataFileManager.Organizations, World.ModDataFileManager.Contacts, Interiors, World.LocationInteractable, World.ModDataFileManager.ModItems, World.ModDataFileManager.Weapons, Time, PlacesOfInterest, World.ModDataFileManager.IssueableWeapons, World.ModDataFileManager.Heads, World.ModDataFileManager.DispatchablePeople);


        newVend.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
        ActiveVendingMachines.Add(newVend);

        EntryPoint.WriteToConsole($"Activate Vending {newVend.Name} {newVend.EntrancePosition}");



    }
    private void ActivateGasPump(Rage.Object obj, string modelName, Vector3 position, float heading, bool IsDoubleSided)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo > 50f)
        {
            return;
        }
        if (ActiveGasPumps.Any(x => x.EntrancePosition.DistanceTo2D(obj.Position) <= 0.2f))
        {
            return;
        }
        //GasStation ClosestStation = (GasStation)Places.ActiveLocations.Where(x => x.GetType() == typeof(GasStation)).OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault();//maybe store anyothe list of stations?
        IGasPumpable gasPumpable = Places.ActiveLocations.OfType<IGasPumpable>().ToList().OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault();
        Vector3 EntrancePos = obj.Position;
        GasPump newGasPump;
        if (gasPumpable != null)
        {
            newGasPump = new GasPump(EntrancePos, heading, gasPumpable.Name, gasPumpable.Description, "None", obj, gasPumpable) { BannerImagePath = gasPumpable.BannerImagePath, OpenTime = 0, CloseTime = 24 };
        }
        else
        {
            newGasPump = new GasPump(EntrancePos, heading, "Gas Pump", "Gas Pump", "None", obj, null) { OpenTime = 0, CloseTime = 24 };
        }
        newGasPump.CanInteractWhenWanted = true;
        newGasPump.StoreData(ShopMenus,World.ModDataFileManager.Agencies,World.ModDataFileManager.Gangs, World.ModDataFileManager.Zones,World.ModDataFileManager.Jurisdictions, World.ModDataFileManager.GangTerritories, World.ModDataFileManager.Names, 
            World.ModDataFileManager.Crimes,World.ModDataFileManager.RelationshipGroups, World, World.ModDataFileManager.Streets, World.ModDataFileManager.LocationTypes, Settings, World.ModDataFileManager.PlateTypes,World.ModDataFileManager.Organizations,World.ModDataFileManager.Contacts,Interiors,World.LocationInteractable, World.ModDataFileManager.ModItems, World.ModDataFileManager.Weapons, Time, PlacesOfInterest, World.ModDataFileManager.IssueableWeapons, World.ModDataFileManager.Heads, World.ModDataFileManager.DispatchablePeople);
        newGasPump.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
        ActiveGasPumps.Add(newGasPump);
        EntryPoint.WriteToConsole($"Activate GasPump {newGasPump.Name} {newGasPump.EntrancePosition}");
    }

    private void ActivateATMMachine(Rage.Object obj, string modelName, Vector3 position, float heading)
    {
        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        if (distanceTo > 50f)
        {
            return;
        }
        if (ActiveATMMachines.Any(x => x.EntrancePosition.DistanceTo2D(obj.Position) <= 0.4f))
        {
            return;
        }
        Bank closestBank = PlacesOfInterest.PossibleLocations.Banks.Where(x => x.IsEnabled).OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault(); ;// (Bank)Places.ActiveLocations.Where(x => x.GetType() == typeof(Bank)).OrderBy(x => x.EntrancePosition.DistanceTo2D(obj)).FirstOrDefault();//maybe store anyothe list of stations?
        Vector3 EntrancePos = obj.Position;
        ATMMachine newATMMachine;
        if (closestBank != null)
        {
            newATMMachine = new ATMMachine(EntrancePos, heading, closestBank.Name, closestBank.Description, "None", obj, closestBank) { BannerImagePath = closestBank.BannerImagePath, OpenTime = 0, CloseTime = 24 };
        }
        else
        {
            newATMMachine = new ATMMachine(EntrancePos, heading, "ATM", "ATM", "None", obj, null) { OpenTime = 0, CloseTime = 24 };
        }
        newATMMachine.CanInteractWhenWanted = true;
        newATMMachine.StoreData(ShopMenus, World.ModDataFileManager.Agencies, World.ModDataFileManager.Gangs, World.ModDataFileManager.Zones, World.ModDataFileManager.Jurisdictions, World.ModDataFileManager.GangTerritories, World.ModDataFileManager.Names,
            World.ModDataFileManager.Crimes, World.ModDataFileManager.RelationshipGroups, World, World.ModDataFileManager.Streets, World.ModDataFileManager.LocationTypes, Settings, World.ModDataFileManager.PlateTypes, World.ModDataFileManager.Organizations, World.ModDataFileManager.Contacts, Interiors, World.LocationInteractable, World.ModDataFileManager.ModItems, World.ModDataFileManager.Weapons, Time, PlacesOfInterest, World.ModDataFileManager.IssueableWeapons, World.ModDataFileManager.Heads, World.ModDataFileManager.DispatchablePeople);

        newATMMachine.Activate(Interiors, Settings, Crimes, Weapons, Time, World);
        ActiveATMMachines.Add(newATMMachine);


        EntryPoint.WriteToConsole($"Activate ATM {newATMMachine.Name} {newATMMachine.EntrancePosition}");
    }
    private void RemoveInactiveVendingMachines()
    {
        for (int i = ActiveVendingMachines.Count - 1; i >= 0; i--)
        {
            VendingMachine gl = ActiveVendingMachines[i];
            if (gl.DistanceToPlayer >= 100f || !gl.MachineProp.Exists() || NativeFunction.Natives.HAS_OBJECT_BEEN_BROKEN<bool>(gl.MachineProp, false))
            {
                if(gl.IsActivated)
                {
                    gl.Deactivate(true);
                    if (ActiveVendingMachines.Contains(gl))
                    {
                        ActiveVendingMachines.Remove(gl);
                    }
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
            if (gl.DistanceToPlayer >= 100f || !gl.PumpProp.Exists() || NativeFunction.Natives.HAS_OBJECT_BEEN_BROKEN<bool>(gl.PumpProp, false))
            {
                if(gl.IsActivated)
                {
                    gl.Deactivate(true);
                    if (ActiveGasPumps.Contains(gl))
                    {
                        ActiveGasPumps.Remove(gl);
                    }
                    GameFiber.Yield();
                }
            }
        }
    }
    private void RemoveInactiveATM()
    {
        for (int i = ActiveATMMachines.Count - 1; i >= 0; i--)
        {
            ATMMachine gl = ActiveATMMachines[i];
            if(gl.DistanceToPlayer < 100f && gl.ATMObject.Exists() && !NativeFunction.Natives.HAS_OBJECT_BEEN_BROKEN<bool>(gl.ATMObject, false))
            {
                continue;
            }
            if (gl.IsActivated)
            {
                gl.Deactivate(true);
                if (ActiveATMMachines.Contains(gl))
                {
                    ActiveATMMachines.Remove(gl);
                }
                GameFiber.Yield();
            }      
        }
    }
    private void RemoveInactiveCashRegisters()
    {
        for (int i = ActiveCashRegisters.Count - 1; i >= 0; i--)
        {
            CashRegister gl = ActiveCashRegisters[i];
            if (gl.DistanceToPlayer < 100f && gl.RegisterProp.Exists() && !NativeFunction.Natives.HAS_OBJECT_BEEN_BROKEN<bool>(gl.RegisterProp, false))
            {
                continue;
            }
            if (gl.IsActivated)
            {
                gl.Deactivate(true);
                if (ActiveCashRegisters.Contains(gl))
                {
                    ActiveCashRegisters.Remove(gl);
                }
                GameFiber.Yield();
            }
        }
    }


}

