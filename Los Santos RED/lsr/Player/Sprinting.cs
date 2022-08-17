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
    private string SprintDisabledReason;
    private uint GameTimeStartedSprinting;
    private uint GameTimeStoppedSprinting;
    private bool isSprinting = false;
    private float CurrentStamina = 50f;
    private uint GameTimeLastUpdatedSprint = 0;
    private ISprintable Player;
    private ISettingsProvideable Settings;
    private bool IsTimeToUpdateSprint => Game.GameTime - GameTimeLastUpdatedSprint >= 250;
    private uint TimeSprinting => isSprinting ? Game.GameTime - GameTimeStartedSprinting : 0;
    private uint TimeNotSprinting => !isSprinting ? Game.GameTime - GameTimeStoppedSprinting : 0;
    public bool IsSprinting => isSprinting;
    public float Stamina => CurrentStamina;
    public float StaminaPercentage => CurrentStamina / Settings.SettingsManager.SprintSettings.MaxStamina;
    public bool InfiniteStamina { get; set; }
    public bool TurboSpeed { get; set; }
    public bool CanSprint { get; set; }
    public bool CanRegainStamina { get; set; }
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
            if (CanSprint)
            {
                GameTimeStartedSprinting = Game.GameTime;
                isSprinting = true;
            }
            else if(SprintDisabledReason != "")
            {
                Game.DisplayHelp($"Unable to Sprint ({SprintDisabledReason})");
            }
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
        SetCanSprint();
        if (IsTimeToUpdateSprint)
        {
            if (CanRegainStamina)
            {
                SetStamina();
            }
            ToggleSprint();
            GameTimeLastUpdatedSprint = Game.GameTime;
        }
        SetMoveSpeed();
    }
    private void SetStamina()
    {
        if (isSprinting && !InfiniteStamina)
        {
            CurrentStamina -= Settings.SettingsManager.SprintSettings.DrainRate;
            if (CurrentStamina < 0)
            {
                CurrentStamina = 0;
            }
        }
        else
        {
            CurrentStamina += Settings.SettingsManager.SprintSettings.RecoverRate;
            if (CurrentStamina > Settings.SettingsManager.SprintSettings.MaxStamina)
            {
                CurrentStamina = Settings.SettingsManager.SprintSettings.MaxStamina;
            }
        }
    }
    private void ToggleSprint()
    {
        if (CanSprint)
        {
            if (isSprinting && CurrentStamina == 0)
            {
                Stop();
            }
            if (IsSprinting && Player.Character.Speed <= 2.0f)
            {
                Stop();
            }
        }
        else
        {
            if (IsSprinting)
            {
                Stop();
            }
        }
    }
    private void SetMoveSpeed()
    {
        if (isSprinting)
        {
            float MoveSpeed = Settings.SettingsManager.SprintSettings.MoveSpeedOverride;
            if (TurboSpeed)
            {
                MoveSpeed += 1.5f;
            }
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, MoveSpeed);
        }
    }
    private void SetCanSprint()
    {
        if (Settings.SettingsManager.NeedsSettings.ApplyNeeds && Player.HumanState.HasPressingNeeds)
        {
            SprintDisabledReason = "Needs";
            CanSprint = false;
            CanRegainStamina = false;
        }
        else if (Settings.SettingsManager.DamageSettings.AllowInjuryEffects && Player.Injuries.IsInjured && Player.Injuries.IsSeverlyInjured)
        {
            SprintDisabledReason = "Injured";
            CanSprint = false;
            CanRegainStamina = false;
        }
        else
        {
            SprintDisabledReason = "";
            CanSprint = true;
            CanRegainStamina = true;
        }
    }
}

