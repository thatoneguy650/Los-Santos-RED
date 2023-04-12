using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//need some rethinking
public class WeaponDropping
{
    private int CurrentWeaponAmmo;
    private List<StoredWeapon> DroppedWeapons = new List<StoredWeapon>();
    private bool DroppingWeapon;
    private IWeaponDroppable Player;
    private int PrevCountWeapons = 1;
    private int WeaponCount = 1;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;


    private List<SuppressedPickup> Suppressed = new List<SuppressedPickup>();
   // private uint GameTimeLastDroppedWeapon;
    public WeaponDropping(IWeaponDroppable currentPlayer, IWeapons weapons, ISettingsProvideable settings)
    {
        Player = currentPlayer;
        Weapons = weapons;
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        PrevCountWeapons = WeaponCount;
        Settings = settings;
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
    public bool CanDropWeapon => !DroppingWeapon && !Player.IsInVehicle && Player.IsVisiblyArmed && Player.ActivityManager.CanPerformActivitiesExtended;
    public void Dispose()
    {
        Suppressed.ForEach(x => x.Unsuppress());
    }
    public void DropWeapon()
    {
        if (CanDropWeapon)
        {
            DroppingWeapon = true;
            GameFiber DropWeapon = GameFiber.StartNew(delegate
            {
                try
                {
                    DropWeaponAnimation();
                    if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
                    {


                        // NativeFunction.Natives.SET_PED_AMMO(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0, false);



                        NativeFunction.Natives.ADD_AMMO_TO_PED(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, -1 * Game.LocalPlayer.Character.Inventory.EquippedWeapon.MagazineSize);




                        // NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, CurrentWeaponAmmo - AmmoToDrop);





                        DroppedWeapons.Add(new StoredWeapon((uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash), Game.LocalPlayer.Character.Inventory.EquippedWeapon.MagazineSize));


                        DisableWeaponPickup((uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);


                        NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);
                        //NativeFunction.Natives.SET_PED_DROPS_WEAPON(Game.LocalPlayer.Character);





                        if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
                        }
                        GameFiber.Sleep(1000);
                    }
                    DroppingWeapon = false;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "DropWeapon");
        }
    }
    public void DisableWeaponPickup(uint weaponHash)
    {
        if (!Settings.SettingsManager.PlayerOtherSettings.WeaponDroppingSupressPickups)
        {
            return;
        }   
        uint modelHash = NativeFunction.Natives.GET_WEAPONTYPE_MODEL<uint>((uint)weaponHash);
        if (modelHash != 0 && !Suppressed.Any(x => x.ModelHash == modelHash))
        {
            SuppressedPickup sp = new SuppressedPickup(modelHash, Game.GameTime, Settings);
            if (!Suppressed.Any(x => x.ModelHash == sp.ModelHash))
            {
                Suppressed.Add(sp);
                sp.Supress();
                //EntryPoint.WriteToConsoleTestLong($"Started Supressing");
            }
        }    
    }
    public void Update()
    {
        WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        if (PrevCountWeapons != WeaponCount && !Player.IsBusted)
        {
            WeaponInventoryChanged(WeaponCount);
        }
        if (Player.IsAliveAndFree && !Player.IsIncapacitated)
        {
            foreach (SuppressedPickup sp in Suppressed)
            {
                if (sp.ShouldUnsupress)
                {
                    sp.Unsuppress();
                    //EntryPoint.WriteToConsoleTestLong($"Stopped Supressing {sp.ModelHash}");
                }
            }
            Suppressed.RemoveAll(x => x.ShouldUnsupress);
        }
    }
    private void DropWeaponAnimation()
    {
        GameFiber DropWeaponAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                AnimationDictionary.RequestAnimationDictionay("pickup_object");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
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
            foreach (StoredWeapon MyOldGuns in DroppedWeapons)
            {
                if (PlayerWeapons.Contains(MyOldGuns.WeaponHash) && Game.LocalPlayer.Character.Position.DistanceTo2D(MyOldGuns.CoordinatedDropped) <= 2f)
                {
                    WeaponInformation Gun2 = Weapons.GetWeapon((uint)MyOldGuns.WeaponHash);
                    if (Gun2 != null)
                    {
                        Gun2.ApplyWeaponVariation(Game.LocalPlayer.Character, MyOldGuns.Variation);
                    }
                    // NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Ammo + 1);

                    NativeFunction.Natives.ADD_AMMO_TO_PED(Game.LocalPlayer.Character, (uint)MyOldGuns.WeaponHash, MyOldGuns.Ammo + 1);


                }
            }
            DroppedWeapons.RemoveAll(x => PlayerWeapons.Contains(x.WeaponHash) && Game.LocalPlayer.Character.Position.DistanceTo2D(x.CoordinatedDropped) <= 2f);
        }
        PrevCountWeapons = weaponCount;
    }

    private class SuppressedPickup
    {
        private ISettingsProvideable Settings;
        public SuppressedPickup(uint modelHash, uint gameTimeDropped, ISettingsProvideable settings)
        {
            ModelHash = modelHash;
            GameTimeDropped = gameTimeDropped;
            Settings = settings;
        }

        public uint GameTimeDropped { get; set; }
        public uint ModelHash { get; set; }
        public bool ShouldUnsupress => Game.GameTime - GameTimeDropped >= Settings.SettingsManager.PlayerOtherSettings.WeaponDroppingTimeToSuppress;
        public void Unsuppress()
        {
            NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(ModelHash, true);
        }
        public void Supress()
        {
            NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(ModelHash, false);
        }
    }
}