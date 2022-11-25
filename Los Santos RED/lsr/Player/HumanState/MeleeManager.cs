using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MeleeManager
{
    private uint GameTimeLastSetMeleeModifier;
    private IMeleeManageable Player;
    private ISettingsProvideable Settings;
    private float currentSetModifer = 1.0f;

    private float MeleeModifier => Settings.SettingsManager.NeedsSettings.ApplyNeeds && Settings.SettingsManager.NeedsSettings.AllowMeleeDamageDecrease ? Player.HumanState.MinimumNeedPercent <= 25f ? 0.25f : Player.HumanState.MinimumNeedPercent <= 50f ? 0.5f : 1.0f : 1.0f;
    private float TotalMeleeModifier => MeleeModifier * Settings.SettingsManager.PlayerOtherSettings.MeleeDamageModifier;
    public MeleeManager(IMeleeManageable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (TotalMeleeModifier != 1.0f && (GameTimeLastSetMeleeModifier == 0 || Game.GameTime - GameTimeLastSetMeleeModifier >= 5000))
        {
            currentSetModifer = TotalMeleeModifier;
            NativeFunction.Natives.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER(Game.LocalPlayer, TotalMeleeModifier, true);
            GameTimeLastSetMeleeModifier = Game.GameTime;
        }
    }

    public void Dispose()
    {
        NativeFunction.Natives.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER(Game.LocalPlayer, 1.0f, true);
    }
}

