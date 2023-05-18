using LosSantosRED.lsr.Interface;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Stance
{
    private readonly string CrouchSet = "move_ped_crouched";
    private readonly string StrafeCrouchSet = "move_ped_crouched_strafing";
    private IStanceable Player;
    private ISettingsProvideable Settings;
    public bool IsCrouched { get; private set; }
    public bool IsInStealthMode { get; private set; }
    public bool IsBeingStealthy => IsCrouched || IsInStealthMode;

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
        if (!Player.IsInVehicle)
        {
            if (IsCrouched)
            {
                Player.ClipsetManager.ResetMovementClipset();
                Player.ClipsetManager.ResetStrafeClipset();
                Player.ClipsetManager.ResetWeaponMovementClipset();
                Player.ClipsetManager.ResetWeaponAnimationOverride();
                IsCrouched = false;
            }
            else
            {
                SetActionMode(false);
                Player.ClipsetManager.SetMovementClipset(CrouchSet);
                Player.ClipsetManager.SetStrafeClipset(StrafeCrouchSet);
                Player.ClipsetManager.SetWeaponAnimationOverride("Ballistic");
                IsCrouched = true;
            }
        }
    }
    public void ToggleActionMode()
    {
        bool isUsingActionMode = NativeFunction.Natives.IS_PED_USING_ACTION_MODE<bool>(Player.Character);
        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Player.Character, !isUsingActionMode, -1, "DEFAULT_ACTION");
        if(!isUsingActionMode)
        {
            IsInStealthMode = false;
        }
    }
    public void SetActionMode(bool enabled)
    {
        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Player.Character, enabled, -1, "DEFAULT_ACTION");
        IsInStealthMode = false;
    }
    public void ToggleStealthMode()
    {
        IsInStealthMode = NativeFunction.Natives.GET_PED_STEALTH_MOVEMENT<bool>(Player.Character);
        NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Player.Character, !IsInStealthMode, "DEFAULT_ACTION");
        IsInStealthMode = !IsInStealthMode;
    }
    public void SetStealthMode(bool enabled)
    {
        NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Player.Character, enabled, "DEFAULT_ACTION");
        IsInStealthMode = enabled;
    }
}

