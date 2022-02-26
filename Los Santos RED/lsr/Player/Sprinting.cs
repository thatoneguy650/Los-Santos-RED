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
    private uint GameTimeLastUpdatedSprint = 0;
    private ISprintable Player;
    private ISettingsProvideable Settings;
    private uint TimeSprinting => isSprinting ? Game.GameTime - GameTimeStartedSprinting : 0;
    private uint TimeNotSprinting => !isSprinting ? Game.GameTime - GameTimeStoppedSprinting : 0;
    public bool IsSprinting => isSprinting;
    public float Stamina => CurrentStamina;
    public float StaminaPercentage => CurrentStamina / Settings.SettingsManager.SprintSettings.MaxStamina;
    public bool InfiniteStamina { get; set; }
    public bool TurboSpeed { get; set; }
    public Sprinting(ISprintable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
        CurrentStamina = Settings.SettingsManager.SprintSettings.MaxStamina;
    }
    public void Start()
    {
        if (!isSprinting && CurrentStamina > Settings.SettingsManager.SprintSettings.MinStaminaToStart && Player.Character.Speed >= 2.0f)
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
    public void Dispose()
    {
        InfiniteStamina = false;
        TurboSpeed = false;
    }
    public void Update()
    {
        if (Game.GameTime - GameTimeLastUpdatedSprint >= 250)
        {
            if (isSprinting && !InfiniteStamina)
            {
                if (CurrentStamina >= Settings.SettingsManager.SprintSettings.DrainRate)
                {
                    CurrentStamina -= Settings.SettingsManager.SprintSettings.DrainRate;
                }
            }
            else
            {
                if (CurrentStamina <= Settings.SettingsManager.SprintSettings.MaxStamina - Settings.SettingsManager.SprintSettings.RecoverRate)
                {
                    CurrentStamina += Settings.SettingsManager.SprintSettings.RecoverRate;
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
            float MoveSpeed = Settings.SettingsManager.SprintSettings.MoveSpeedOverride;
            if(TurboSpeed)
            {
                MoveSpeed += 1.5f;
            }
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, MoveSpeed);
        }
    }


}

