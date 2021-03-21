using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

//need some rethinking
public class WeaponDropping
{
    private int CurrentWeaponAmmo;
    private List<DroppedWeapon> DroppedWeapons = new List<DroppedWeapon>();
    private bool DroppingWeapon;
    private IWeaponDroppable Player;
    private int PrevCountWeapons = 1;
    private int WeaponCount = 1;
    private IWeapons Weapons;
    public WeaponDropping(IWeaponDroppable currentPlayer, IWeapons weapons)
    {
        Player = currentPlayer;
        Weapons = weapons;
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        PrevCountWeapons = WeaponCount;
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
    public bool CanDropWeapon => !DroppingWeapon && !Player.IsInVehicle && Player.IsVisiblyArmed;
    public void DropWeapon()
    {
        DroppingWeapon = true;
        GameFiber DropWeapon = GameFiber.StartNew(delegate
        {
            DropWeaponAnimation();
            NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, CurrentWeaponAmmo - AmmoToDrop);
            WeaponVariation DroppedGunVariation = Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            DroppedWeapons.Add(new DroppedWeapon(Game.LocalPlayer.Character.Inventory.EquippedWeapon, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), DroppedGunVariation, AmmoToDrop));
            NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            }

            GameFiber.Sleep(1000);
            DroppingWeapon = false;
        }, "DropWeapon");
    }
    public void Update()
    {
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        if (PrevCountWeapons != WeaponCount)
        {
            WeaponInventoryChanged(WeaponCount);
        }
    }
    private void DropWeaponAnimation()
    {
        GameFiber DropWeaponAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("pickup_object");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);
        }, "DropWeaponAnimation");

        if (Game.LocalPlayer.Character.IsRunning)
        {
            GameFiber.Sleep(500);
        }
        else
        {
            GameFiber.Sleep(250);
        }
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
                    WeaponInformation Gun2 = Weapons.GetWeapon((uint)MyOldGuns.Weapon.Hash);
                    if (Gun2 != null)
                    {
                        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Variation);
                    }
                    NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Ammo + 1);
                }
            }
            DroppedWeapons.RemoveAll(x => PlayerWeapons.Contains(x.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(x.CoordinatedDropped) <= 2f);
        }
        PrevCountWeapons = weaponCount;
    }
}