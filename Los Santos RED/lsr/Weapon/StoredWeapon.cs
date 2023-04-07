
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StoredWeapon
{
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
}

