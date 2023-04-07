using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponStorage
{
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    public List<StoredWeapon> StoredWeapons { get; set; } = new List<StoredWeapon>();

    public WeaponStorage(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void PlaceInStorage(uint hash, IWeapons weapons)
    {
        Weapons = weapons;
        foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons.Where(x=> (uint)x.Hash == hash))
        {
            StoredWeapons.Add(new StoredWeapon((uint)wd.Hash, Vector3.Zero, Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)wd.Hash), wd.Ammo));
        }
        foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons.Where(x => (uint)x.Hash == hash))
        {
            NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(Game.LocalPlayer.Character, (uint)wd.Hash);
        }
    }
    public void RemoveFromStorage(StoredWeapon storedWeapon, IWeapons weapons)
    {
        Weapons = weapons;
        if (storedWeapon == null)
        {
            return;
        }
        if(!StoredWeapons.Contains(storedWeapon))
        {
            return;
        }
        StoredWeapons.Remove(storedWeapon);
        storedWeapon.GiveToPlayer(Weapons);
    }
    public void CreateInteractionMenu(MenuPool menuPool, UIMenu VehicleInteractMenu, IWeapons weapons, IModItems modItems)
    {
        Weapons = weapons;
        //Weapons Menu
        UIMenu WeaponsHeaderMenu = menuPool.AddSubMenu(VehicleInteractMenu, "Weapons");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Manage Weapons.";
        WeaponsHeaderMenu.SetBannerType(EntryPoint.LSRedColor);


        //Remove Weapons
        UIMenu RemoveWeaponsSubMenu = menuPool.AddSubMenu(WeaponsHeaderMenu, "Remove Weapons");
        WeaponsHeaderMenu.MenuItems[WeaponsHeaderMenu.MenuItems.Count() - 1].Description = "Remove stored weapons.";
        RemoveWeaponsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        foreach (StoredWeapon storedWeapon in StoredWeapons)
        {
            string weaponName = GetWeaponName((uint)storedWeapon.WeaponHash, modItems);
            UIMenuItem removeWeapon = new UIMenuItem($"Remove {weaponName}", $"Remove {weaponName}");
            removeWeapon.Activated += (menu, item) =>
            {
                RemoveFromStorage(storedWeapon, Weapons);
                RemoveWeaponsSubMenu.Visible = false;
            };
            RemoveWeaponsSubMenu.AddItem(removeWeapon);
        }
        //Place Weapons
        UIMenu PlaceWeaponsSubMenu = menuPool.AddSubMenu(WeaponsHeaderMenu, "Add Weapons");
        WeaponsHeaderMenu.MenuItems[WeaponsHeaderMenu.MenuItems.Count() - 1].Description = "Add stored weapons.";
        PlaceWeaponsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
        {
            string weaponName = GetWeaponName((uint)wd.Hash, modItems);
            UIMenuItem addWeapon = new UIMenuItem($"Add {weaponName}", $"Add {weaponName}");
            addWeapon.Activated += (menu, item) =>
            {
                PlaceInStorage((uint)wd.Hash, Weapons);
                PlaceWeaponsSubMenu.Visible = false;
            };
            PlaceWeaponsSubMenu.AddItem(addWeapon);
        }
    }
    private string GetWeaponName(uint hash, IModItems modItems)
    {
        string weaponName = hash.ToString();
        WeaponInformation wi = Weapons.GetWeapon((uint)hash);
        if (wi != null)
        {
            weaponName = wi.ModelName;
            WeaponItem modItem = modItems.GetWeapon(wi.ModelName);
            if (modItem == null)
            {
                modItem = modItems.GetWeapon(wi.Hash);
            }
            if (modItem != null)
            {
                weaponName = modItem.Name;
            }
        }
        return weaponName;
    }
}

