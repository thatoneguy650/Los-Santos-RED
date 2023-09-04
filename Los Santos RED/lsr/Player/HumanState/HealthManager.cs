using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HealthManager
{
    private uint GameTimeLastCheckedRegen = 0;
    private IHealthManageable Player;
    private ISettingsProvideable Settings;
    private uint GameTimeLastRegenedHealth;
    private uint GameTimeLastDrainedHealth;
    private bool IsTimeToUpdate => Game.GameTime - GameTimeLastCheckedRegen >= 1000;
    public bool RecentlyRegenedHealth => GameTimeLastRegenedHealth != 0 && Game.GameTime - GameTimeLastRegenedHealth <= 5000;
    public bool RecentlyDrainedHealth => GameTimeLastDrainedHealth != 0 && Game.GameTime - GameTimeLastDrainedHealth <= 5000;

    public bool IsMaxHealth => Player.Character.Exists() && Player.Character.Health == Player.Character.MaxHealth;

    public HealthManager(IHealthManageable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (IsTimeToUpdate)
        {
            if (Settings.SettingsManager.NeedsSettings.AllowHealthRegen && Player.HumanState.HasNeedsManaged)
            {
                if (Game.GameTime - GameTimeLastRegenedHealth >= Settings.SettingsManager.NeedsSettings.HealthRegenInterval)
                {
                    if (Player.Character.Health < Player.Character.MaxHealth)
                    {
                        ChangeHealth(Math.Abs(Settings.SettingsManager.NeedsSettings.HealthRegenAmount));
                        //EntryPoint.WriteToConsoleTestLong($"Health Manager Added Health (Needs) Max Health: {Player.Character.MaxHealth} Current Health: {Player.Character.Health}");
                        GameTimeLastRegenedHealth = Game.GameTime;
                    }
                }
            }
            if (Settings.SettingsManager.NeedsSettings.AllowHealthDrain && Player.HumanState.HasPressingNeeds)
            {
                if (Game.GameTime - GameTimeLastDrainedHealth >= Settings.SettingsManager.NeedsSettings.HealthDrainInterval)
                { 
                    if (Player.Character.Health >= Settings.SettingsManager.NeedsSettings.HealthDrainMinHealth)//minimum value to stop it from killing people tons
                    {
                        ChangeHealth(-1 * Math.Abs(Settings.SettingsManager.NeedsSettings.HealthDrainAmount));
                        //EntryPoint.WriteToConsoleTestLong($"Health Manager Drained Health (Needs) Max Health: {Player.Character.MaxHealth} Current Health: {Player.Character.Health}");
                        GameTimeLastDrainedHealth = Game.GameTime;
                    }
                }
            }
            GameTimeLastCheckedRegen = Game.GameTime;
        }
    }
    public void Dispose()
    {

    }
    public void ChangeHealth(int ToAdd)
    {
        if (ToAdd > 0)
        {
            if (Player.Character.Health < Player.Character.MaxHealth)
            {
                if (Player.Character.MaxHealth - Player.Character.Health < ToAdd)
                {
                    ToAdd = Player.Character.MaxHealth - Player.Character.Health;
                }
                Player.Character.Health += ToAdd;
                //EntryPoint.WriteToConsole($"PLAYER EVENT: Added Health {ToAdd}");
            }
        }
        else if (ToAdd < 0)
        {
            if (Player.Character.Health > 0 && Player.Character.Health + ToAdd >= 0)
            {
                Player.Character.Health += ToAdd;
            }
            else
            {
                Player.Character.Health = 0;
            }
        }
    }
    public void SetHealth(int value)
    {
        //EntryPoint.WriteToConsole($"Set Health, Value {value}");
        if(value > Player.Character.MaxHealth)
        {
            Player.Character.Health = Player.Character.MaxHealth;
        }
        else
        {
            if(value == 0)
            {
                Game.LocalPlayer.Character.Kill();
                //EntryPoint.WriteToConsole("Set health to 0, killing player");
            }
            else
            {
                Player.Character.Health = value;
            }
            
        }
    }
}

