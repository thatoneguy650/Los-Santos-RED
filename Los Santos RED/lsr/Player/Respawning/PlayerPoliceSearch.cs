using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerPoliceSearch
{
    private IRespawnable Player;
    private ITimeReportable Time;
    private IModItems ModItems;
    private VehicleExt CarToSearch;
    private IWeapons Weapons;
    public PlayerPoliceSearch(IRespawnable player, ITimeReportable time, IModItems modItems, VehicleExt carToSearch, IWeapons weapons)
    {
        Player = player;
        Time = time;
        ModItems = modItems;
        CarToSearch = carToSearch;
        Weapons = weapons;
    }
    public bool FoundIllegalDrugs { get; private set; }
    public bool FoundIllegalWeapons { get; private set; }
    public bool FoundIllegalItems { get; private set; }


    public bool FoundVehicleIllegalDrugs { get; private set; }
    public bool FoundVehicleIllegalWeapons { get; private set; }
    public bool FoundVehicleIllegalItems { get; private set; }
    public bool FoundVehicleStoredBody { get; private set; }

    public bool DidItemsSearch { get; private set; }
    public bool DidWeaponSearch { get; private set; }

    public bool DidVehicleItemsSearch { get; private set; }
    public bool DidVehicleWeaponSearch { get; private set; }


    public bool DidVehicleBodySearch { get; private set; }

    public bool AlwaysFind { get; set; }
    public void DoWeaponSearch()
    {
        bool hasCCW = Player.Licenses.HasValidCCWLicense(Time);
        DidWeaponSearch = true;
        List<WeaponInformation> IllegalWeapons = Player.WeaponEquipment.GetIllegalWeapons(hasCCW);
        WeaponInformation worstWeapon = IllegalWeapons.OrderByDescending(x => x.WeaponLevel).FirstOrDefault();
        if (worstWeapon == null)
        {
            return;
        }
        foreach (WeaponInformation weapon in IllegalWeapons)
        {
            int TimesToCheck = 1;
            WeaponItem wi = ModItems.PossibleItems.WeaponItems.FirstOrDefault(x => x.ModelName == weapon.ModelName);
            if (wi == null)
            {
                EntryPoint.WriteToConsole($"SEARCH WEAPON {weapon.ModelName} DID NOT FIND WEAPON, CONTINUING");
                continue;
            }
            if (weapon.Category == WeaponCategory.Throwable)
            {
                TimesToCheck = NativeFunction.Natives.GET_AMMO_IN_PED_WEAPON<int>(Player.Character, weapon.Hash);
                TimesToCheck.Clamp(1, 10);
            }
            EntryPoint.WriteToConsole($"SEARCH WEAPON {weapon.ModelName} FOUND MODITEM %:{wi.PoliceFindDuringPlayerSearchPercentage} TimesToCheck {TimesToCheck}");
            if (RandomItems.RandomPercent(wi.PoliceFindDuringPlayerSearchPercentage * TimesToCheck))
            {
                Player.Violations.WeaponViolations.AddFoundWeapon(worstWeapon, hasCCW);
                FoundIllegalWeapons = true;
                FoundIllegalItems = true;
                Player.WeaponEquipment.RemoveIllegalWeapons(hasCCW);
                //EntryPoint.WriteToConsoleTestLong($"SEARCH WEAPON {weapon.ModelName} PERCENTAGE MET, WEAPONS FOUND");
                break;
            }
        }
    }
    public void DoItemSearch()
    {
        DidItemsSearch = true;
        List<ModItem> IllegalItems = Player.Inventory.GetIllicitItems();
        foreach (ModItem modItem in IllegalItems)
        {
            int ItemsOwned = 1;
            InventoryItem ii = Player.Inventory.Get(modItem);
            if (ii != null)
            {
                ItemsOwned = ii.Amount;
            }
            EntryPoint.WriteToConsole($"SEARCH WEAPON {modItem.Name} %:{modItem.PoliceFindDuringPlayerSearchPercentage} ItemsOwned {ItemsOwned} Total%{modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned}");
            if (RandomItems.RandomPercent(modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned))
            {
                Player.Violations.OtherViolations.AddFoundIllegalItem();
                FoundIllegalDrugs = true;
                FoundIllegalItems = true;
                Player.Inventory.RemoveIllicitInventoryItems();
                EntryPoint.WriteToConsole($"SEARCH ITEMS {modItem.Name} PERCENTAGE MET, ITEMS FOUND");
                break;
            }
        }
    }
    public void DoVehicleItemSearch()
    {
        DidVehicleItemsSearch = true;
        if(CarToSearch == null)
        {
            return;
        }
        List<ModItem> IllegalItems = CarToSearch.SimpleInventory.GetIllegalItems();
        foreach (ModItem modItem in IllegalItems)
        {
            int ItemsOwned = 1;
            InventoryItem ii = CarToSearch.SimpleInventory.Get(modItem);
            if (ii != null)
            {
                ItemsOwned = ii.Amount;
            }
            //EntryPoint.WriteToConsoleTestLong($"SEARCH WEAPON {modItem.Name} %:{modItem.PoliceFindDuringPlayerSearchPercentage} ItemsOwned {ItemsOwned} Total%{modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned}");
            if (RandomItems.RandomPercent(modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned))
            {
                Player.Violations.OtherViolations.AddFoundIllegalItem();
                FoundVehicleIllegalDrugs = true;
                FoundVehicleIllegalItems = true;
                CarToSearch.SimpleInventory.RemoveIllegalItems();
                //EntryPoint.WriteToConsoleTestLong($"SEARCH ITEMS {modItem.Name} PERCENTAGE MET, ITEMS FOUND");
                break;
            }
        }
    }
    public void DoVehicleWeaponSearch()
    {
        bool hasCCW = Player.Licenses.HasValidCCWLicense(Time);
        DidVehicleWeaponSearch = true;
        if (CarToSearch == null)
        {
            return;
        }
        List<WeaponInformation> IllegalWeapons = CarToSearch.WeaponStorage.GetIllegalWeapons(hasCCW, Weapons);
        WeaponInformation worstWeapon = IllegalWeapons.OrderByDescending(x => x.WeaponLevel).FirstOrDefault();
        if (worstWeapon == null)
        {
            return;
        }
        foreach (WeaponInformation weapon in IllegalWeapons)
        {
            int TimesToCheck = 1;
            WeaponItem wi = ModItems.PossibleItems.WeaponItems.FirstOrDefault(x => x.ModelName == weapon.ModelName);
            if (wi == null)
            {
                //EntryPoint.WriteToConsoleTestLong($"SEARCH WEAPON {weapon.ModelName} DID NOT FIND WEAPON, CONTINUING");
                continue;
            }
            if (weapon.Category == WeaponCategory.Throwable)
            {
                TimesToCheck = NativeFunction.Natives.GET_AMMO_IN_PED_WEAPON<int>(Player.Character, weapon.Hash);
                TimesToCheck.Clamp(1, 10);
            }
            //EntryPoint.WriteToConsoleTestLong($"SEARCH WEAPON {weapon.ModelName} FOUND MODITEM %:{wi.PoliceFindDuringPlayerSearchPercentage} TimesToCheck {TimesToCheck}");
            if (RandomItems.RandomPercent(wi.PoliceFindDuringPlayerSearchPercentage * TimesToCheck))
            {
                Player.Violations.WeaponViolations.AddFoundWeapon(worstWeapon, hasCCW);
                FoundVehicleIllegalWeapons = true;
                FoundVehicleIllegalItems = true;
                CarToSearch.WeaponStorage.RemoveIllegalWeapons(hasCCW, Weapons);
                //EntryPoint.WriteToConsoleTestLong($"SEARCH WEAPON {weapon.ModelName} PERCENTAGE MET, WEAPONS FOUND");
                break;
            }
        }
    }

    public void DoVehicleBodySearch()
    {
        DidVehicleBodySearch = true;
        if (CarToSearch == null)
        {
            return;
        }
        if(CarToSearch.VehicleBodyManager.StoredBodies.Any())
        {
            FoundVehicleStoredBody = true;
            FoundVehicleIllegalItems = true;
            Player.Violations.DamageViolations.AddKilledCivilian();
        }
    }
}

