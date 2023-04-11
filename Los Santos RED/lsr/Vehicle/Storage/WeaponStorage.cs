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
    public void Reset()
    {
        StoredWeapons.Clear();
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
    public void CreateInteractionMenu(MenuPool menuPool, UIMenu parentMenu, IWeapons weapons, IModItems modItems)
    {
        Weapons = weapons;
        //Weapons Menu
        UIMenu WeaponsHeaderMenu = menuPool.AddSubMenu(parentMenu, "Stored Weapons");
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].Description = "Manage Stored Weapons. Place items within storage, or retreive them for use.";
        WeaponsHeaderMenu.SetBannerType(EntryPoint.LSRedColor);


        //Remove Weapons
        UIMenu RemoveWeaponsSubMenu = menuPool.AddSubMenu(WeaponsHeaderMenu, "Take Weapons");
        WeaponsHeaderMenu.MenuItems[WeaponsHeaderMenu.MenuItems.Count() - 1].Description = "Take stored weapons. Remove the weapon from its local storage and add it to the player's current weapon inventory.";
        RemoveWeaponsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        foreach (StoredWeapon storedWeapon in StoredWeapons)
        {
            string weaponName = GetWeaponName((uint)storedWeapon.WeaponHash, modItems);
            UIMenuItem removeWeapon = new UIMenuItem($"Take {weaponName}", $"Take {weaponName}");
            removeWeapon.Activated += (menu, item) =>
            {
                RemoveFromStorage(storedWeapon, Weapons);
                removeWeapon.Enabled = false;
                //RemoveWeaponsSubMenu.Visible = false;
            };
            RemoveWeaponsSubMenu.AddItem(removeWeapon);
        }
        //Place Weapons
        UIMenu PlaceWeaponsSubMenu = menuPool.AddSubMenu(WeaponsHeaderMenu, "Deposit Weapons");
        WeaponsHeaderMenu.MenuItems[WeaponsHeaderMenu.MenuItems.Count() - 1].Description = "Deposit stored weapons. Remove the weapon from the player's current weapon inventory and add it to the local storage.";
        PlaceWeaponsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        foreach (WeaponDescriptor wd in Game.LocalPlayer.Character.Inventory.Weapons)
        {
            if((int)wd.Hash == -72657034)//parachute?
            {
                continue;
            }
            string weaponName = GetWeaponName((uint)wd.Hash, modItems);
            UIMenuItem addWeapon = new UIMenuItem($"Deposit {weaponName}", $"Deposit {weaponName}");
            addWeapon.Activated += (menu, item) =>
            {
                PlaceInStorage((uint)wd.Hash, Weapons);
                addWeapon.Enabled = false;
                //PlaceWeaponsSubMenu.Visible = false;
            };
            PlaceWeaponsSubMenu.AddItem(addWeapon);
        }
    }
    private string GetWeaponName(uint hash, IModItems modItems)
    {
        WeaponInformation wi = Weapons.GetWeapon((uint)hash);
        if (wi == null)
        {
            return hash.ToString();
        }
        WeaponItem modItem = modItems.GetWeapon(wi.ModelName);
        if(modItem == null)
        {
            modItem = modItems.GetWeapon(wi.Hash);
        }
        if(modItem == null)
        {
            return wi.ModelName;
        }
        return modItem.Name;

    }
}

