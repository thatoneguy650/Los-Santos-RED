using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClipsetManager
{
    private IClipsetManageable Player;
    private ISettingsProvideable Settings;
    private bool hasOverriderCoverClipset = false;
    private string SetCoverClipset;
    private string RequiredCoverClipset;

    private readonly string BallisticWeaponAnimOverride = "Ballistic";
    private readonly string DefaultWeaponAnimOverride = "Default";
    private readonly string FranklinWeaponAnimOverride = "Franklin";
    private readonly string GangWeaponAnimOverride = "Gang";
    private readonly string MichaelWeaponAnimOverride = "Michael";
    private readonly string FreemodeFWeaponAnimOverride = "MP_F_Freemode";
    private readonly string TrevorWeaponAnimOverride = "Trevor";
    private readonly string HillbillyWeaponAnimOverride = "Hillbilly";
    private readonly string Gang1HWeaponAnimOverride = "Gang1H";

    public ClipsetManager(IClipsetManageable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
 
    public void Update()
    {
        if (!Settings.SettingsManager.PlayerOtherSettings.AllowSetCharacterClipsets)
        {
            return;
        }
        UpdateCoverClipset();
    }
    private void Reset()
    {

    }
    public void Dispose()
    {
        ResetMotionInCoverClipset(true);
    }
    private void UpdateCoverClipset()
    {
        DetermineCoverClipset();
        if (RequiredCoverClipset == "")
        {
            ResetMotionInCoverClipset(false);
            return;
        }
        else if(RequiredCoverClipset != SetCoverClipset)
        {
            ApplyCoverClipset();
        }
    }
    private void ApplyCoverClipset()
    {
        //EntryPoint.WriteToConsoleTestLong($"CLIPSET MANAGER: RequiredCoverClipset{RequiredCoverClipset}");
        SetMotionInCoverClipset(RequiredCoverClipset);
        hasOverriderCoverClipset = true;
        SetCoverClipset = RequiredCoverClipset;
    }
    private void DetermineCoverClipset()
    {
        if(!Settings.SettingsManager.PlayerOtherSettings.AutoSetClipsets)
        {
            return;
        }
        if(Player.WeaponEquipment.CurrentWeapon == null)
        {
            RequiredCoverClipset = "";
        }
        else if(Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Pistol)
        {
            RequiredCoverClipset = "cover@move@ai@base@1h";
        }
        //else if (Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.AR || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.LMG)
        //{
        //    RequiredCoverClipset = "cover@move@ai@base@2h";
        //}
        else
        {
            RequiredCoverClipset = "";
        }
    }
    private void LoadClipset(string clipSet)
    {
        if (!NativeFunction.Natives.HAS_CLIP_SET_LOADED<bool>(clipSet))
        {
            NativeFunction.Natives.REQUEST_CLIP_SET<bool>(clipSet);
            int ticks = 0;
            while (!NativeFunction.Natives.HAS_CLIP_SET_LOADED<bool>(clipSet) && EntryPoint.ModController.IsRunning)
            {
                ticks++;
                if(ticks > 10)
                {
                    break;
                }
                GameFiber.Yield();
            }
        }
        //if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(animSet))
        //{
        //    NativeFunction.Natives.REQUEST_ANIM_SET<bool>(animSet);
        //    while(!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(animSet) && EntryPoint.ModController.IsRunning)
        //    {
        //        GameFiber.Yield();
        //    }        
        //}
    }

    private void SetMotionInCoverClipset(string currentClipset)
    {
        LoadClipset(currentClipset);
        NativeFunction.Natives.SET_PED_MOTION_IN_COVER_CLIPSET_OVERRIDE(Game.LocalPlayer.Character, currentClipset);
    }
    private void ResetMotionInCoverClipset(bool force)
    {
        if(!force && !hasOverriderCoverClipset)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong($"CLIPSET MANAGER: COVER RESET");
        NativeFunction.Natives.CLEAR_PED_MOTION_IN_COVER_CLIPSET_OVERRIDE(Player.Character);
        hasOverriderCoverClipset = false;
        SetCoverClipset = "";
        RequiredCoverClipset = "";
    }

    public void SetMovementClipset(string currentClipset)
    {
        LoadClipset(currentClipset);
        NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET(Player.Character, currentClipset, 0.25f);// 0x3E800000);
    }
    public void ResetMovementClipset()
    {
        //EntryPoint.WriteToConsoleTestLong($"CLIPSET MANAGER: MOVEMENT RESET");
        NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET(Player.Character, 0.25f);// 0.5f);
    }

    public void SetStrafeClipset(string currentClipset)
    {
        LoadClipset(currentClipset);
        NativeFunction.Natives.SET_PED_STRAFE_CLIPSET(Player.Character, currentClipset);
    }
    public void ResetStrafeClipset()
    {
        //EntryPoint.WriteToConsoleTestLong($"CLIPSET MANAGER: STRAFE RESET");
        NativeFunction.Natives.RESET_PED_STRAFE_CLIPSET(Player.Character);
    }

    public void SetWeaponAnimationOverride(string weaponAnimation)
    {
        NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey(weaponAnimation));
    }
    public void ResetWeaponAnimationOverride()
    {
        NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey(DefaultWeaponAnimOverride));
    }

    public void ResetWeaponMovementClipset()
    {
        //EntryPoint.WriteToConsoleTestLong($"CLIPSET MANAGER: WEAPON MOVEMENT RESET");
        NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Player.Character);
    }
}