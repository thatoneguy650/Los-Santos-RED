using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class WeaponDropping
{
    private static List<WeaponExt> DroppedWeapons;
    private static bool DroppingWeapon;
    private static int PrevCountWeapons;
    private static int WeaponCount;
    public static bool IsRunning { get; set; }
   
    public static void Initialize()
    {
        DroppedWeapons = new List<WeaponExt>();
        DroppingWeapon = false;
        PrevCountWeapons = 1;
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
            if (PrevCountWeapons != WeaponCount)
                WeaponInventoryChanged(WeaponCount);

            if (Game.IsKeyDownRightNow(General.MySettings.KeyBinding.DropWeaponKey) && !DroppingWeapon && !PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsConsideredArmed())
            {
                DropWeapon();
            }
        }
    }
    public static void ResetWeaponCount()
    {
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        PrevCountWeapons = WeaponCount;
    }
    private static void DropWeapon()
    {
        DroppingWeapon = true;
        GameFiber DropWeapon = GameFiber.StartNew(delegate
        {
            DropWeaponAnimation();
            if (Game.LocalPlayer.Character.IsRunning)
                GameFiber.Sleep(500);
            else
                GameFiber.Sleep(250);

            int CurrentWeaponAmmo = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo;
            int AmmoToDrop = 0;
            if (CurrentWeaponAmmo > 60)
                AmmoToDrop = 60;
            else if (CurrentWeaponAmmo == 0)
                AmmoToDrop = 0;
            else
                AmmoToDrop = CurrentWeaponAmmo;

            NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, CurrentWeaponAmmo - AmmoToDrop);

            GTAWeapon.WeaponVariation DroppedGunVariation = General.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            DroppedWeapons.Add(new WeaponExt(Game.LocalPlayer.Character.Inventory.EquippedWeapon, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), DroppedGunVariation, AmmoToDrop));

            NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            Debugging.WriteToLog("DroppingWeapon", string.Format("Dropped your gun, Ammo {0}", AmmoToDrop));

            GameFiber.Sleep(1000);
            DroppingWeapon = false;
        }, "DropWeapon");
        Debugging.GameFibers.Add(DropWeapon);
    }
    private static void WeaponInventoryChanged(int weaponCount)
    {
        if (weaponCount > PrevCountWeapons) //Added Weapon
        {
            WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (WeaponExt MyOldGuns in DroppedWeapons)
            {
                if (PlayerWeapons.Contains(MyOldGuns.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(MyOldGuns.CoordinatedDropped) <= 2f)
                {
                    Debugging.WriteToLog("WeaponInventoryChanged", string.Format("Just picked up an old weapon {0},OldAmmo: {1}", MyOldGuns.Weapon.Hash, MyOldGuns.Ammo));
                    General.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Variation);


                    //NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo + MyOldGuns.Ammo);
                    NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Ammo + 1);
                }
            }
            DroppedWeapons.RemoveAll(x => PlayerWeapons.Contains(x.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(x.CoordinatedDropped) <= 2f);
        }
        else //Lost Weapon
        {

        }
        Debugging.WriteToLog("WeaponInventoryChanged", string.Format("Previous Weapon Count {0}, Current {1}, Total Dropped Weapons {2}", PrevCountWeapons, weaponCount, DroppedWeapons.Count()));
        PrevCountWeapons = weaponCount;
    }
    private static void DropWeaponAnimation()
    {
        GameFiber DropWeaponAnimation = GameFiber.StartNew(delegate
        {
            General.RequestAnimationDictionay("pickup_object");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);
        }, "DropWeaponAnimation");
        Debugging.GameFibers.Add(DropWeaponAnimation);
    }
}

