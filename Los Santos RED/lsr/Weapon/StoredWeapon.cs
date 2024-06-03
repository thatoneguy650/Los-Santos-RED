
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class StoredWeapon
{
    private UIMenu weaponStorageSubMenu;
    private UIMenuItem weaponStorageSubMenuItem;
    private UIMenuItem takeWeaponMenu;
    private UIMenuItem giveWeaponMenu;
    private WeaponInformation WeaponInformation;
    private WeaponItem WeaponItem;
    private string DisplayName => WeaponItem != null ? WeaponItem.Name : WeaponInformation != null ? WeaponInformation.ModelName : WeaponHash.ToString();
    private string DisplayDesription => WeaponItem != null ? WeaponItem.GetTypeDescription() : "";
    public uint WeaponHash { get; set; }
    public Vector3 CoordinatedDropped { get; set; }
    public WeaponVariation Variation { get; set; }
    public int Ammo { get; set; }
    public StoredWeapon(uint _WeaponHash, Vector3 _CoordinatedDropped, WeaponVariation _Variation,int _Ammo)
    {
        WeaponHash = _WeaponHash;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
        Ammo = _Ammo;
    }

    public StoredWeapon()
    {
    }

    public void GiveToPlayer(IWeapons weapons)
    {
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(WeaponHash, 0, false);
        if (Game.LocalPlayer.Character.Inventory.Weapons.Contains(WeaponHash))
        {
            WeaponInformation Gun2 = weapons.GetWeapon((uint)WeaponHash);
            if (Gun2 != null)
            {
                Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, Variation);
            }
        }
        NativeFunction.Natives.SET_PED_AMMO(Game.LocalPlayer.Character, (uint)WeaponHash, 0, false);
        NativeFunction.Natives.ADD_AMMO_TO_PED(Game.LocalPlayer.Character, (uint)WeaponHash, Ammo);
    }

    public virtual void CreateManagementMenu(IInteractionable player, MenuPool menuPool, WeaponStorage weaponStorage, UIMenu headerMenu, IWeapons weapons, IModItems modItems, bool withAnimations, bool removeBanner)
    {
        WeaponInformation = weapons.GetWeapon((uint)WeaponHash);
        WeaponItem = WeaponInformation != null ? modItems.GetWeapon(WeaponInformation.ModelName) : modItems.GetWeapon(WeaponHash);


        weaponStorageSubMenu = menuPool.AddSubMenu(headerMenu, DisplayName);
        weaponStorageSubMenuItem = headerMenu.MenuItems[headerMenu.MenuItems.Count() - 1];
        weaponStorageSubMenuItem.Description = DisplayDesription;
        if (removeBanner)
        {
            weaponStorageSubMenu.RemoveBanner();
        }
        else
        {
            weaponStorageSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }

        takeWeaponMenu = new UIMenuItem("Take", "");
        takeWeaponMenu.Activated += (sender, selectedItem) =>
        {
            weaponStorage.RemoveFromStorage(this, weapons);
            if (withAnimations)
            {
                player.ActivityManager.PerformItemAnimation(WeaponItem, true);
            }
            UpdateInventoryScrollers(player, weaponStorage);
        };
        weaponStorageSubMenu.AddItem(takeWeaponMenu);

        giveWeaponMenu = new UIMenuItem("Deposit", "");
        giveWeaponMenu.Activated += (sender, selectedItem) =>
        {
            weaponStorage.PlaceInStorage(this, weapons);
            if (withAnimations)
            {
                player.ActivityManager.PerformItemAnimation(WeaponItem, false);
            }
            UpdateInventoryScrollers(player, weaponStorage);
        };
        weaponStorageSubMenu.AddItem(giveWeaponMenu);
        UpdateInventoryScrollers(player, weaponStorage);
    }

    private void UpdateInventoryScrollers(IInteractionable player, WeaponStorage weaponStorage)
    {
        int totalInStorage = weaponStorage.StoredWeapons.Count(x => x.WeaponHash == WeaponHash);
        int totalOnPerson = Game.LocalPlayer.Character.Inventory.Weapons.Count(x => (uint)x.Hash == WeaponHash);//should always be 1
        takeWeaponMenu.Enabled = totalInStorage > 0;
        giveWeaponMenu.Enabled = totalOnPerson > 0;

        string descriptionToUse = $"{DisplayDesription}" +
            $"~n~{totalInStorage} (s) ~o~Stored~s~" +
            $"~n~{totalOnPerson} (s) In ~y~Player Weapons~s~."; ;

        takeWeaponMenu.Description = descriptionToUse;
        giveWeaponMenu.Description = descriptionToUse;

        weaponStorageSubMenuItem.RightLabel = $"{totalInStorage} Stored";
        weaponStorageSubMenuItem.Description = descriptionToUse;
    }
}

