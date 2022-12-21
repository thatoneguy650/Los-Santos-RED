using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponEquipment
{
    private bool isActive = true;
    private IItemEquipable Player;
    private WeaponDropping WeaponDropping;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private WeaponSelector WeaponSelector;
    private WeaponSway WeaponSway;
    private WeaponRecoil WeaponRecoil;
    private int FramesSinceShot = 0;
    public SelectorOptions CurrentSelectorSetting => WeaponSelector.CurrentSelectorSetting;
    public WeaponInformation CurrentWeapon { get; private set; }
    public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
    public WeaponHash CurrentWeaponHash { get; set; }
    public bool CurrentWeaponIsOneHanded { get; private set; }
    public short CurrentWeaponMagazineSize { get; private set; }
    public WeaponInformation CurrentSeenWeapon => !Player.IsInVehicle ? CurrentWeapon : null;
    public WeaponHash LastWeaponHash { get; private set; }
    public bool IsDangerouslyArmed => Player.IsVisiblyArmed && CurrentWeapon != null && CurrentWeapon.Category != WeaponCategory.Melee;
    public WeaponEquipment(IItemEquipable player, IWeaponDroppable weaponDroppable, IWeapons weapons, ISettingsProvideable settings, IWeaponSwayable weaponSwayable, IWeaponRecoilable weaponRecoilable, IWeaponSelectable weaponSelectable)
    {
        Player = player;
        Weapons = weapons;
        Settings = settings;
        WeaponDropping = new WeaponDropping(weaponDroppable, Weapons, Settings);
        WeaponSway = new WeaponSway(weaponSwayable, Settings);
        WeaponRecoil = new WeaponRecoil(weaponRecoilable, Settings);
        WeaponSelector = new WeaponSelector(weaponSelectable, Settings);
    }
    public void Setup()
    {
        isActive = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                while (isActive)
                {
                    if (Game.LocalPlayer.Character.IsShooting)
                    {
                        FramesSinceShot = 0;
                        WeaponRecoil.Update();
                        Player.SetShot();
                    }
                    else
                    {
                        if (FramesSinceShot < Settings.SettingsManager.SwaySettings.FramesBetweenRecoil)
                        {
                            FramesSinceShot++;
                        }
                        if (FramesSinceShot < Settings.SettingsManager.SwaySettings.FramesBetweenRecoil)
                        {
                            WeaponSway.Reset();
                        }
                    }
                    if (FramesSinceShot >= Settings.SettingsManager.SwaySettings.FramesBetweenRecoil && (Game.LocalPlayer.IsFreeAiming || Game.LocalPlayer.Character.IsAiming))
                    {
                        WeaponSway.Update();
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "IsShootingChecker");
        GameFiber.StartNew(delegate
        {
            try
            {
                while (isActive)
                {
                    WeaponSelector.Update();
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "IsShootingChecker2");
    }
    public void Dispose()
    {
        WeaponDropping.Dispose();
        isActive = false;
    }
    public void Update()
    {
        UpdateVisiblyArmed();
        UpdateData();
        if (Settings.SettingsManager.PlayerOtherSettings.AllowWeaponDropping)
        {
            WeaponDropping.Update();
        }
    }
    public void Reset()
    {
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
    }
    public void DropWeapon() => WeaponDropping.DropWeapon();
    public void DisableWeaponPickup(uint weaponHash) => WeaponDropping.DisableWeaponPickup(weaponHash);
    public void SetUnarmed()
    {
        if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
        }
    }
    public void SetLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
            //EntryPoint.WriteToConsole("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
        }
    }
    public void ToggleSelector() => WeaponSelector.ToggleSelector();
    private void UpdateData()
    {
        WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;

        if (PlayerCurrentWeapon != null)
        {
            CurrentWeaponHash = PlayerCurrentWeapon.Hash;
            CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
           // GameFiber.Yield();
            if (CurrentWeapon != null && PlayerCurrentWeapon != null && CurrentWeapon.Category != WeaponCategory.Melee)
            {
                CurrentWeaponMagazineSize = PlayerCurrentWeapon.MagazineSize;
            }
            else
            {
                CurrentWeaponMagazineSize = 0;
            }
        }
        else
        {
            CurrentWeaponMagazineSize = 0;
            CurrentWeaponHash = 0;
            CurrentWeapon = null;
        }
        if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject != null)
        {
            CurrentWeaponIsOneHanded = Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.X <= 0.4f;
        }
        else
        {
            CurrentWeaponIsOneHanded = false;
        }
        if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
        {
            LastWeaponHash = PlayerCurrentWeapon.Hash;
        }
    }
    private void UpdateVisiblyArmed()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
        {
            Player.IsVisiblyArmed = false;
        }
        else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
        {
            Player.IsVisiblyArmed = false;
        }
        else if(Player.ActivityManager.IsUsingToolAsWeapon)
        {
            Player.IsVisiblyArmed = false;
        }
        else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2725352035
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)966099553
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x787F0BB//weapon_snowball
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x060EC506//weapon_fireextinguisher
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x34A67B97//weapon_petrolcan
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0xBA536372//weapon_hazardcan
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x8BB05FD7//weapon_flashlight
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x23C9F95C//weapon_ball

            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2508868239//bat

            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)4192643659//bottle
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2227010557//crowbar
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)1141786504//golf club
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)1317494643//hammer

            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x94117305//pool cue
            || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x19044EE0//wrench

            )//weapon_ball

        {
            Player.IsVisiblyArmed = false;
        }
        else if (!NativeFunction.Natives.IS_PLAYER_CONTROL_ON<bool>(Game.LocalPlayer))
        {
            Player.IsVisiblyArmed = false;
        }
        else
        {
            Player.IsVisiblyArmed = true;
        }
    }


}

