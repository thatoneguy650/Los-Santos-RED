using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerVoice
{
    public IPlayerVoiceable Player;
    private ISettingsProvideable Settings;
    private ISpeeches Speeches;
    private uint GameTimeLastSpoke;
    private uint TimeBetweenSpeakingRandomizer;
    //private uint GameTimeBetweenSpeaking;
    private List<string> GenericFightPossibilities;
    private List<string> GenericCrashPossibilities;
    private List<string> GenericWonPossibilities;
    private List<string> GenericPoliceFightPossibilities;

    private bool CanSpeak => Player.IsAliveAndFree && !Player.IsIncapacitated && Game.GameTime - GameTimeLastSpoke >= (GameTimeBetweenSpeaking + TimeBetweenSpeakingRandomizer);
    private uint GameTimeBetweenSpeaking
    {
        get
        {
            if(Player.WantedLevel >= 3)
            {
                return Settings.SettingsManager.PlayerSpeechSettings.TimeBetweenSpeech_HighWanted;
            }
            else if(Player.IsWanted)
            {
                return Settings.SettingsManager.PlayerSpeechSettings.TimeBetweenSpeech_LowWanted;
            }
            else
            {
                return Settings.SettingsManager.PlayerSpeechSettings.TimeBetweenSpeech;
            }
        }
    }
    public bool IsSpeaking { get; private set; }
    public PlayerVoice(IPlayerVoiceable player, ISettingsProvideable settings, ISpeeches speeches)
    {
        Player = player;
        Settings = settings;
        Speeches = speeches;
    }
    public void Setup()
    {
        GenericFightPossibilities = new List<string>() { "FIGHT", "FIGHT_RUN", "GENERIC_CURSE_HIGH", "GENERIC_CURSE_MED", "PROVOKE_GENERIC", "CHALLENGE_THREATEN" };
        GenericCrashPossibilities = new List<string>(){"BUMP","CRASH_CAR","CRASH_GENERIC","GENERIC_CURSE_HIGH","GENERIC_CURSE_MED","GENERIC_SHOCKED_MED","CHALLENGE_THREATEN"};
        GenericWonPossibilities = new List<string>(){ "WON_DISPUTE","GENERIC_WHATEVER" };
        GenericPoliceFightPossibilities = new List<string>(){"CHASED_BY_POLICE","FIGHT","FIGHT_RUN","GENERIC_CURSE_HIGH","GENERIC_CURSE_MED","GENERIC_SHOCKED_MED","PROVOKE_GENERIC","WON_DISPUTE","CHALLENGE_THREATEN"};
    }
    public void Update()
    {

    }
    public void Dispose()
    {

    }
    public void OnWantedActiveMode()
    {
        SayAvailableAmbient(GenericPoliceFightPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnWantedActiveModePercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnWantedActiveMode");
    }
    public void OnWantedSearchMode()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnWantedSearchModePercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnWantedSearchMode");
    }
    public void OnBecameWanted()
    {
        SayAvailableAmbient(GenericPoliceFightPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnBecameWantedPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnBecameWanted");
    }
    public void OnLostWanted()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnLostWantedPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnLostWanted");
    }
    public void OnSuspectEluded()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnSuspectEludedPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnSuspectEluded");
    }
    public void OnCrashedCar()
    {
        SayAvailableAmbient(GenericCrashPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnCrashedCarPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnCrashedCar");
    }
    public void OnShotGun()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnShotGunPercentage);    
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnShotGun");
    }
    public void OnKilledCop()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnKilledCopPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnKilledCop");
    }
    public void OnKilledCivilian()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, Settings.SettingsManager.PlayerSpeechSettings.OnKilledCivilianPercentage);
        //EntryPoint.WriteToConsoleTestLong("Player Voice OnKilledCivilian");
    }
    private bool SayAvailableAmbient(List<string> Possibilities, bool WaitForComplete, float percentage)
    {
        bool Spoke = false;
        if (Settings.SettingsManager.PlayerSpeechSettings.EnableSpeech && CanSpeak && RandomItems.RandomPercent(percentage))
        {
            foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()).Take(3))
            {
                string voiceName = null;
                if (Player.CharacterModelIsFreeMode)
                {
                    voiceName = Player.FreeModeVoice;
                }
                //bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(Player.Character, AmbientSpeech, false);
                Player.Character.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, SpeechModifier.Force | SpeechModifier.AllowRepeat);
                GameFiber.Sleep(300);
                if (Player.Character.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                //EntryPoint.WriteToConsole($"PlayerVoice: {Player.Character.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            while (Player.Character.IsAnySpeechPlaying && WaitForComplete)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (Spoke)
            {
                GameTimeLastSpoke = Game.GameTime;
                TimeBetweenSpeakingRandomizer = RandomItems.GetRandomNumber(Settings.SettingsManager.PlayerSpeechSettings.TimeBetweenSpeechRandomizer_Min, Settings.SettingsManager.PlayerSpeechSettings.TimeBetweenSpeechRandomizer_Max);
            }
        }
        return Spoke;
    }

 
}

