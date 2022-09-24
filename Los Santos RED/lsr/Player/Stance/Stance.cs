using LosSantosRED.lsr.Interface;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Stance
{
    private IStanceable Player;
    private ISettingsProvideable Settings;
    public bool IsCrouched { get; set; }
    public Stance(IStanceable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Update()
    {
        if(IsCrouched && Settings.SettingsManager.ActivitySettings.CrouchingAdjustsMovementSpeed && !Player.IsAiming)
        {
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, Settings.SettingsManager.ActivitySettings.CrouchMovementSpeedOverride);
        }
    }
    public void Crouch()
    {
        string CrouchSet = "move_ped_crouched";
        string StrafeCrouchSet = "move_ped_crouched_strafing";
        if (!Player.IsInVehicle)
        {
            if (IsCrouched)
            {
                NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET(Player.Character, 0.5f);
                NativeFunction.Natives.RESET_PED_STRAFE_CLIPSET(Player.Character);
                NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Player.Character);
                IsCrouched = false;
            }
            else
            {
                if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(CrouchSet))
                {
                    NativeFunction.Natives.REQUEST_ANIM_SET(CrouchSet);
                }

                if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(StrafeCrouchSet))
                {
                    NativeFunction.Natives.REQUEST_ANIM_SET(StrafeCrouchSet);
                }
                SetActionMode(false);
                NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET(Player.Character, CrouchSet, 0.5f);
                NativeFunction.Natives.SET_PED_STRAFE_CLIPSET(Player.Character, StrafeCrouchSet);
                NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, "Ballistic");
                IsCrouched = true;
            }
        }
    }
    public void ToggleActionMode()
    {
        bool isUsingActionMode = NativeFunction.Natives.IS_PED_USING_ACTION_MODE<bool>(Player.Character);
        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Player.Character, !isUsingActionMode, -1, "DEFAULT_ACTION");
    }
    public void SetActionMode(bool enabled)
    {
        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Player.Character, enabled, -1, "DEFAULT_ACTION");
    }
    public void ToggleStealthMode()
    {
        bool isUsingStealthMode = NativeFunction.Natives.GET_PED_STEALTH_MOVEMENT<bool>(Player.Character);
        NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Player.Character, !isUsingStealthMode, "DEFAULT_ACTION");
    }
    public void SetStealthMode(bool enabled)
    {
        NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Player.Character, enabled, "DEFAULT_ACTION");
    }
}

