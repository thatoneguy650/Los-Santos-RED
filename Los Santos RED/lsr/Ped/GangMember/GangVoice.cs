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



    //{ "DRAW_GUN", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };  



    private readonly List<string> LowCombatSpeech = new List<string> {  "GENERIC_CURSE_MED", "GENERIC_INSULT_MED", "GENERIC_FRUSTRATED_MED","GENERIC_FRUSTRATED_HIGH", "GENERIC_WHATEVER" };
    private readonly List<string> HighCombatSpeech = new List<string> { "CHALLENGE_THREATEN", "GENERIC_CURSE_HIGH", "GENERIC_WAR_CRY", "GENERIC_FUCK_YOU", "GENERIC_INSULT_HIGH", "GENERIC_INSULT_MED" };//,"GENERIC_FRIGHTENED_HIGH" };




    //{ "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_SHOCKED_HIGH", "GENERIC_WAR_CRY", "PINNED_DOWN", "GENERIC_INSULT_HIGH", "GET_HIM" };

    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE", "COMBAT_TAUNT" };
    private readonly List<string> DangerousUnarmedSpeech = new List<string> { "DRAW_GUN", "COP_SEES_WEAPON", "COP_SEES_GUN", "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "GET_HIM" };
    private readonly List<string> InVehiclePlayerOnFootMegaPhone = new List<string> { "STOP_ON_FOOT_MEGAPHONE", "COP_ARRIVAL_ANNOUNCE_MEGAPHONE" };
    private readonly List<string> InVehiclePlayerInVehicleMegaPhone = new List<string> { "CHASE_VEHICLE_MEGAPHONE", "STOP_VEHICLE_CAR_MEGAPHONE", "STOP_VEHICLE_CAR_WARNING_MEGAPHONE", "STOP_VEHICLE_GENERIC_MEGAPHONE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE_MEGAPHONE", "COMBAT_TAUNT" };
    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE", "ARREST_PLAYER", "CRIMINAL_APPREHENDED" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "SUSPECT_KILLED_REPORT" };

    private uint GameTimeLastSpoke;

    private int TimeBetweenSpeaking;
    private bool Spoke;

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
        if(GangMember.WantedLevel >= 2 || GangMember.PedViolations.CurrentlyViolatingWantedLevel >= 2 || GangMember.Pedestrian.IsInCombat)
        {
            GangMember.PlaySpeech(HighCombatSpeech, GangMember.IsInVehicle, true);
            TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Max);
        }
        else if(GangMember.WantedLevel == 1 || GangMember.PedViolations.CurrentlyViolatingWantedLevel == 1)
        {
            GangMember.PlaySpeech(LowCombatSpeech, GangMember.IsInVehicle, true);
            TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Max);
        }
        GameTimeLastSpoke = Game.GameTime;
    }
    public void ResetSpeech()
    {
        GameTimeLastSpoke = 0;
    }
}

