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
    private uint GameTimeBetweenSpeaking;
    private List<string> GenericFightPossibilities;
    private List<string> GenericCrashPossibilities;
    private List<string> GenericWonPossibilities;
    private List<string> GenericPoliceFightPossibilities;

    private bool CanSpeak => Player.IsAliveAndFree && !Player.IsIncapacitated && Game.GameTime - GameTimeLastSpoke >= GameTimeBetweenSpeaking;

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
        GenericWonPossibilities = new List<string>(){"WON_DISPUTE","GENERIC_WHATEVER"};
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
        SayAvailableAmbient(GenericPoliceFightPossibilities, false, 80f);
        EntryPoint.WriteToConsole("Player Voice OnWantedActiveMode");
    }
    public void OnWantedSearchMode()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, 30f);
        EntryPoint.WriteToConsole("Player Voice OnWantedSearchMode");
    }
    public void OnBecameWanted()
    {
        SayAvailableAmbient(GenericPoliceFightPossibilities, false, 100f);
        EntryPoint.WriteToConsole("Player Voice OnBecameWanted");
    }
    public void OnLostWanted()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, 100f);
        EntryPoint.WriteToConsole("Player Voice OnLostWanted");
    }
    public void OnSuspectEluded()
    {
        SayAvailableAmbient(GenericWonPossibilities, false, 30f);
        EntryPoint.WriteToConsole("Player Voice OnSuspectEluded");
    }
    public void OnCrashedCar()
    {
        SayAvailableAmbient(GenericCrashPossibilities, false, 50f);
        EntryPoint.WriteToConsole("Player Voice OnCrashedCar");
    }
    public void OnShotGun()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, 30f);    
        EntryPoint.WriteToConsole("Player Voice OnShotGun");
    }
    public void OnKilledCop()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, 80f);
        EntryPoint.WriteToConsole("Player Voice OnKilledCop");
    }
    public void OnKilledCivilian()
    {
        SayAvailableAmbient(GenericFightPossibilities, false, 80f);
        EntryPoint.WriteToConsole("Player Voice OnKilledCivilian");
    }
    private bool SayAvailableAmbient(List<string> Possibilities, bool WaitForComplete, float percentage)
    {
        bool Spoke = false;
        if (CanSpeak && RandomItems.RandomPercent(percentage))
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
                EntryPoint.WriteToConsole($"PlayerVoice: {Player.Character.Handle} Attempting {AmbientSpeech}, Result: {Spoke}", 5);
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
            }
        }
        return Spoke;
    }

 
}

