using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IntimidationManager
{
    private IIntimidationManageable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;

   // private Mod.Player ItelliTest;
    private uint GameTimeLastYelled;
    private uint GameTimeLastPrinted;

    private bool RecentlyYelled => GameTimeLastYelled != 0 && Game.GameTime - GameTimeLastYelled <= Settings.SettingsManager.PlayerOtherSettings.RecentlyYellingIntimidationTime;
    public float IntimidationPercent { get; private set; }
    public IntimidationManager(IIntimidationManageable player, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        UpdateIntimidationLevel();
    }
    public void Reset()
    {

    }
    public void Dispose()
    {

    }
    private void UpdateIntimidationLevel()
    {
        float boostLevel = 0f;
        if(Player.CurrentLocation.IsInside)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.InsideIntimidationBoost;
        }
        if(Player.RecentlyShot)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.RecentlyShotIntimidationBoost;
        }
        if(Player.IsVisiblyArmed)
        {
            if(Player.WeaponEquipment.CurrentWeapon != null)
            {
                boostLevel += (Settings.SettingsManager.PlayerOtherSettings.WeaponLevelBoostScalar * Player.WeaponEquipment.CurrentWeapon.WeaponLevel);
            }
            else
            {
                boostLevel += Settings.SettingsManager.PlayerOtherSettings.VisiblyArmedIntimidationBoost;
            }
        }
        if(Player.Violations.DamageViolations.RecentlyKilledCivilian)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.KilledCivilianIntimidationBoost;
        }
        if (Player.Violations.DamageViolations.CountRecentCivilianMurderVictimWithoutSecurity >= 3)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.MurderRampageIntimidationBoost;
        }
        if (Player.Violations.DamageViolations.RecentlyKilledCop)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.KilledCopIntimidationBoost;
        }
        if(RecentlyYelled)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.RecentlyYelledIntimidationBoost;
        }
        if(Player.WantedLevel >= 2)
        {
            boostLevel += Settings.SettingsManager.PlayerOtherSettings.PlayerWantedIntimidationBoost;
        }
        IntimidationPercent = boostLevel.Clamp(0f,100f);

#if DEBUG
        if(Game.GameTime - GameTimeLastPrinted >= 2000)
        {
            EntryPoint.WriteToConsole($"IntimidationPercent: {IntimidationPercent} boostLevel{boostLevel}");
            GameTimeLastPrinted = Game.GameTime;
        }
        
#endif
    }
    public void YellGetDown()
    {
        if(!Player.IsVisiblyArmed || Player.IsInVehicle)
        {
            return;
        }
        EntryPoint.WriteToConsole("IM YELL GET DOWN RAN");
        Player.PlayerVoice.YellGetDown();
        GameTimeLastYelled = Game.GameTime;
    }
}

