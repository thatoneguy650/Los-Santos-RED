using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponDropping
{
    private List<DroppedWeapon> DroppedWeapons = new List<DroppedWeapon>();
    private bool DroppingWeapon;
    private int PrevCountWeapons = 1;
    private int WeaponCount = 1;
    private int CurrentWeaponAmmo;

    public WeaponDropping()
    {
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        PrevCountWeapons = WeaponCount;
    }

    public bool CanDropWeapon
    {
        get
        {
            if (!DroppingWeapon && !Mod.Player.IsInVehicle && Mod.Player.IsConsideredArmed)//was player one
                return true;
            else
                return false;
        }
    }
    public int AmmoToDrop
    {
        get
        {
            CurrentWeaponAmmo = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo;
            if (CurrentWeaponAmmo > 60)
                return 60;
            else if (CurrentWeaponAmmo == 0)
                return 0;
            else
                return CurrentWeaponAmmo;
        }
    }
    public void Tick()
    {
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        if (PrevCountWeapons != WeaponCount)
            WeaponInventoryChanged(WeaponCount);
    }
    public void DropWeapon()
    {
        DroppingWeapon = true;
        GameFiber DropWeapon = GameFiber.StartNew(delegate
        {
            DropWeaponAnimation();
            NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, CurrentWeaponAmmo - AmmoToDrop);

            WeaponVariation DroppedGunVariation = Mod.DataMart.Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            DroppedWeapons.Add(new DroppedWeapon(Game.LocalPlayer.Character.Inventory.EquippedWeapon, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), DroppedGunVariation, AmmoToDrop));

            NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);

            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);

            GameFiber.Sleep(1000);
            DroppingWeapon = false;
        }, "DropWeapon");
        Mod.Debug.GameFibers.Add(DropWeapon);
    }
    private void WeaponInventoryChanged(int weaponCount)
    {
        if (weaponCount > PrevCountWeapons) //Added Weapon
        {
            WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (DroppedWeapon MyOldGuns in DroppedWeapons)
            {
                if (PlayerWeapons.Contains(MyOldGuns.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(MyOldGuns.CoordinatedDropped) <= 2f)
                {
                    MyOldGuns.Variation.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash);
                    NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Ammo + 1);
                }
            }
            DroppedWeapons.RemoveAll(x => PlayerWeapons.Contains(x.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(x.CoordinatedDropped) <= 2f);
        }
        PrevCountWeapons = weaponCount;
    }
    private void DropWeaponAnimation()
    {
        GameFiber DropWeaponAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary AnimDictionary = new AnimationDictionary("pickup_object");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);
        }, "DropWeaponAnimation");
        Mod.Debug.GameFibers.Add(DropWeaponAnimation);

        if (Game.LocalPlayer.Character.IsRunning)
            GameFiber.Sleep(500);
        else
            GameFiber.Sleep(250);
    }
}

