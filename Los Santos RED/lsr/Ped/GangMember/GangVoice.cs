using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangVoice
{
    private PedExt GangMember;
    private bool IsInFiber = false;
    private ISettingsProvideable Settings;

    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP", "GENERIC_HOWS_IT_GOING" };
    private readonly List<string> FleeingSpeech = new List<string> { "GENERIC_SHOCKED_MED", "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_MED", "GENERIC_FRUSTRATED_HIGH" };
    private readonly List<string> LowCombatSpeech = new List<string> {  "GENERIC_CURSE_MED", "GENERIC_INSULT_MED", "GENERIC_FRUSTRATED_MED","GENERIC_FRUSTRATED_HIGH", "GENERIC_WHATEVER" };
    private readonly List<string> HighCombatSpeech = new List<string> { "CHALLENGE_THREATEN", "GENERIC_CURSE_HIGH", "GENERIC_WAR_CRY", "GENERIC_FUCK_YOU", "GENERIC_INSULT_HIGH", "GENERIC_INSULT_MED" };//,"GENERIC_FRIGHTENED_HIGH" };

    private uint GameTimeLastSpoke;

    private int TimeBetweenSpeaking;
    private bool Spoke;
    private bool IsInSevereCombat => GangMember.WantedLevel >= 2 || GangMember.PedViolations.CurrentlyViolatingWantedLevel >= 2 || (GangMember.Pedestrian.Exists() && GangMember.Pedestrian.IsInCombat);
    private bool IsInLowCombat => GangMember.WantedLevel == 1 || GangMember.PedViolations.CurrentlyViolatingWantedLevel == 1;
    private bool IsFleeing => GangMember.IsCowering || (GangMember.Pedestrian.Exists() && GangMember.Pedestrian.IsFleeing);
    public GangVoice(PedExt gangMember, ISettingsProvideable settings)
    {
        GangMember = gangMember;
        Settings = settings;
        TimeBetweenSpeaking = 25000 + RandomItems.GetRandomNumberInt(0, 13000);
    }

    public bool IsSpeechTimedOut => Game.GameTime - GameTimeLastSpoke < TimeBetweenSpeaking;
    public bool CanSpeak => !GangMember.IsUnconscious && !IsSpeechTimedOut && GangMember.DistanceToPlayer <= 50f && !GangMember.IsBeingHeldAsHostage;
    public void Speak(IPoliceRespondable currentPlayer)
    {
        if (!CanSpeak || !GangMember.Pedestrian.Exists())
        {
            return;
        }
        if(IsInSevereCombat)
        {
            SaySevereCombatSpeech();
        }
        else if(IsInLowCombat)
        {
            SayLowCombatSpeech();
        }
        else if (IsFleeing)
        {
            SayFleeingSpeech();
        }
        else
        {
            SayIdleSpeech();
        }
        GameTimeLastSpoke = Game.GameTime;
    }

    private void SayIdleSpeech()
    {
        if (RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.IdleSpeakPercentage))
        {
            GangMember.PlaySpeech(IdleSpeech, false, false);
        }
        TimeBetweenSpeaking = Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_General_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_General_Randomizer_Min, Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_General_Randomizer_Max);
    }

    private void SaySevereCombatSpeech()
    {
        if (RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.SevereCombatSpeakPercentage))
        {
            GangMember.PlaySpeech(HighCombatSpeech, false, true);
        }
        TimeBetweenSpeaking = Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_HighCombat_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_HighCombat_Randomizer_Min, Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_HighCombat_Randomizer_Max);
    }
    private void SayLowCombatSpeech()
    {
        if (RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.LowCombatSpeakPercentage))
        {
            GangMember.PlaySpeech(LowCombatSpeech, false, true);
        }
        TimeBetweenSpeaking = Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_LowCombat_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_LowCombat_Randomizer_Min, Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_LowCombat_Randomizer_Max);
    }
    private void SayFleeingSpeech()
    {
        if (RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.FleeingSpeakPercentage))
        {
            GangMember.PlaySpeech(LowCombatSpeech, false, true);
        }
        TimeBetweenSpeaking = Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_Fleeing_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_Fleeing_Randomizer_Min, Settings.SettingsManager.GangSettings.TimeBetweenGangSpeak_Fleeing_Randomizer_Max);
    }
    public void ResetSpeech()
    {
        GameTimeLastSpoke = 0;
    }
}

