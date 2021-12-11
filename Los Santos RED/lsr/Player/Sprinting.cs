using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Sprinting
{
    private uint GameTimeStartedSprinting;
    private uint GameTimeStoppedSprinting;
    private bool isSprinting = false;
    private float CurrentStamina = 50f;
    private float MaxStamina = 50f;
    private float StaminaStartMin = 10f;
    private uint GameTimeLastUpdatedSprint = 0;
    private uint TimeSprinting => isSprinting ? Game.GameTime - GameTimeStartedSprinting : 0;
    private uint TimeNotSprinting => !isSprinting ? Game.GameTime - GameTimeStoppedSprinting : 0;

    public bool IsSprinting => isSprinting;
    public float Stamina => CurrentStamina;
    public float StaminaPercentage => CurrentStamina / MaxStamina;

    private ISprintable Player;
    private ISettingsProvideable Settings;

    public Sprinting(ISprintable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
        MaxStamina = Settings.SettingsManager.PlayerSettings.Sprint_MaxStamina;
        CurrentStamina = Settings.SettingsManager.PlayerSettings.Sprint_MaxStamina;
        StaminaStartMin = Settings.SettingsManager.PlayerSettings.Sprint_MinStaminaToStart;
    }

    public void Start()
    {
        if (!isSprinting && CurrentStamina > StaminaStartMin && Player.Character.Speed >= 2.0f)
        {
            GameTimeStartedSprinting = Game.GameTime;
            isSprinting = true;
        }
    }
    public void Stop()
    {
        if (isSprinting)
        {
            GameTimeStoppedSprinting = Game.GameTime;
            isSprinting = false;
        }
    }
    public void Update()
    {
        if (Game.GameTime - GameTimeLastUpdatedSprint >= 250)
        {
            if (isSprinting)
            {
                if (CurrentStamina >= 1.0f)
                {
                    CurrentStamina--;
                }
            }
            else
            {
                if (CurrentStamina <= MaxStamina - 1.0f)
                {
                    CurrentStamina++;
                }
            }
            if (isSprinting && CurrentStamina == 0)
            {
                Stop();
            }
            if(IsSprinting && Player.Character.Speed <= 2.0f)
            {
                Stop();
            }
            GameTimeLastUpdatedSprint = Game.GameTime;
        }
        if (isSprinting)
        {
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, 5.0f);
        }
    }

}

