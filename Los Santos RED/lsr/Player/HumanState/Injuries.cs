using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Injuries
{
    private IIntoxicatable Player;
    private ISettingsProvideable Settings;
    public Injuries(IIntoxicatable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    private uint GameTimeStartedSwerving;
    private uint GameTimeToStopSwerving;
    private uint GameTimeUntilNextSwerve;
    private float SteeringBias;
    private string CurrentClipset;
    private string OverLayEffect;
    private bool IsPrimary;
    public bool IsInjured { get; private set; }
    public string DebugString { get; set; }
    public bool IsSeverlyInjured => CurrentIntensity > 1.5f;
    private string ClipsetAtCurrentIntensity
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return "NONE";
            }
            else if (CurrentIntensity >= 3)
            {
                return "move_m@drunk@verydrunk";//"move_m @drunk@verydrunk";
            }
            else if (CurrentIntensity >= 2)
            {
                return "move_m@injured"; //"move_m@drunk@moderatedrunk";
            }
            else
            {
                return "move_m@injured"; //"move_m@drunk@slightlydrunk";
            }
        }
    }
    private float SwerveAtCurrentIntensity
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 0.1f;
            }
            else if (CurrentIntensity >= 2)
            {
                return 0.5f;
            }
            else if (CurrentIntensity >= 3)
            {
                return 0.75f;
            }
            else
            {
                return 0.1f;
            }
        }
    }
    private uint SwerveMinLength
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 1000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 2000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 3000;
            }
            else
            {
                return 1000;
            }
        }
    }
    private uint SwerveMaxLength
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 2000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 3500;
            }
            else if (CurrentIntensity >= 3)
            {
                return 5000;
            }
            else
            {
                return 2000;
            }
        }
    }
    private uint SwerveMinDelay
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 10000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 5000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 2500;
            }
            else
            {
                return 10000;
            }
        }
    }
    private uint SwerveMaxDelay
    {
        get
        {
            if (CurrentIntensity < 1.5)
            {
                return 15000;
            }
            else if (CurrentIntensity >= 2)
            {
                return 10000;
            }
            else if (CurrentIntensity >= 3)
            {
                return 5000;
            }
            else
            {
                return 15000;
            }
        }
    }
    public bool IsSwerving { get; private set; }
    public float CurrentIntensity { get; private set; }
    public void Reset()
    {
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        NativeFunction.Natives.RESET_PED_VISIBLE_DAMAGE(Game.LocalPlayer.Character);
        if (IsInjured)
        {
            SetHealthy(true);
        }
        if(IsInjured)
        {
            Restart();
        }
    }
    public void Dispose()
    {
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        NativeFunction.Natives.RESET_PED_VISIBLE_DAMAGE(Game.LocalPlayer.Character);
        if (IsInjured)
        {
            SetHealthy(true);
        }
    }
    public void Restart()
    {
        Update(IsPrimary);
        if (IsPrimary && CurrentIntensity >= 0.25f)// 0.25f)
        {
            SetInjured();
            Update(IsPrimary);
        }
    }
    public void Update(bool isPrimary)
    {
        if (Settings.SettingsManager.DamageSettings.AllowInjuryEffects)
        {
            IsPrimary = isPrimary;
            int Health = Player.Character.Health;// - 100;
            int MaxHealth = Player.Character.MaxHealth;// - 100;
            if(Health > 99)
            {
                Health = Health - 100;
            }
            if (MaxHealth > 99)
            {
                MaxHealth = MaxHealth - 100;
            }

            if (MaxHealth < Health || Health <= 0 || MaxHealth < 0)
            {
                return;
            }
            if (Health < 0)
            {
                Health = 0;
            }
            if (MaxHealth < 0)
            {
                MaxHealth = 1;
            }
            if (Health <= MaxHealth - Settings.SettingsManager.DamageSettings.InjuryEffectHealthLostStart)
            {
                OverLayEffect = "dying";
                float Percentage = (float)Health / (float)(MaxHealth- Settings.SettingsManager.DamageSettings.InjuryEffectHealthLostStart);
                Percentage = 1f - Percentage;
                Percentage *= 5f;
                Percentage *= Settings.SettingsManager.DamageSettings.InjuryEffectIntensityModifier;
                CurrentIntensity = Percentage;
                UpdateInjuredStatus();
            }
            else
            {
                if (IsInjured)
                {
                    SetHealthy(true);
                }
            }
        }
    }
    private void UpdateInjuredStatus()
    {
        if (!IsInjured && IsPrimary && CurrentIntensity >= 0.1f)// 0.25f)
        {
            SetInjured();
        }
        else if (IsInjured && CurrentIntensity <= 0.1f)//0.25f)
        {
            SetHealthy(true);
        }
        if (IsInjured && IsPrimary)
        {
            if (CurrentClipset != ClipsetAtCurrentIntensity && ClipsetAtCurrentIntensity != "NONE")
            {
                CurrentClipset = ClipsetAtCurrentIntensity;
                Player.ClipsetManager.SetMovementClipset(CurrentClipset);
            }
            NativeFunction.CallByName<int>("SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE", CurrentIntensity);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
            if (Player.IsInVehicle)
            {
                UpdateSwerving();
            }
        }
    }
    private void UpdateSwerving()
    {
        if (Game.GameTime >= GameTimeUntilNextSwerve)
        {
            GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);
            if (!IsSwerving && Player.IsDriver)
            {
                IsSwerving = true;
                GameTimeStartedSwerving = Game.GameTime;
                GameTimeToStopSwerving = Game.GameTime + RandomItems.GetRandomNumber(SwerveMinLength, SwerveMaxLength);
                SteeringBias = RandomItems.GetRandomNumber(-1f * SwerveAtCurrentIntensity, SwerveAtCurrentIntensity);
            }
        }
        if (IsSwerving && Game.GameTime > GameTimeToStopSwerving)
        {
            IsSwerving = false;
            SteeringBias = 0f;
        }
        if (Player.IsDriver && IsSwerving && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            NativeFunction.Natives.SET_VEHICLE_STEER_BIAS(Player.CurrentVehicle.Vehicle, SteeringBias);
        }
    }
    private void SetInjured()
    {
        IsInjured = true;
        CurrentClipset = ClipsetAtCurrentIntensity;
        NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
        if (CurrentClipset != "NONE" && !Player.ActivityManager.IsSitting && !Player.IsInVehicle)
        {
            Player.ClipsetManager.SetMovementClipset(CurrentClipset);
        }
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", OverLayEffect);
        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", CurrentIntensity / 5.0f);
        NativeFunction.Natives.x80C8B1846639BB19(1);
        NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", CurrentIntensity);
        GameTimeUntilNextSwerve = Game.GameTime + RandomItems.GetRandomNumber(15000, 30000);
    }
    private void SetHealthy(bool ResetClipset)
    {
        IsInjured = false;
        if (!Player.Intoxication.IsIntoxicated)
        {
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);
            if (ResetClipset)
            {
                Player.ClipsetManager.ResetMovementClipset();
            }
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            NativeFunction.Natives.x80C8B1846639BB19(0);
            NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
        }
    }
}

