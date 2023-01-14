using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CopVoice
{
    private PedExt Cop;
    private bool IsInFiber = false;
    private ISettingsProvideable Settings;

    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE", "COMBAT_TAUNT" };
    private readonly List<string> DangerousUnarmedSpeech = new List<string> { "DRAW_GUN", "COP_SEES_WEAPON", "COP_SEES_GUN", "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "GET_HIM" };
    private readonly List<string> InVehiclePlayerOnFootMegaPhone = new List<string> { "STOP_ON_FOOT_MEGAPHONE", "COP_ARRIVAL_ANNOUNCE_MEGAPHONE" };

    private readonly List<string> InVehiclePlayerInVehicleMegaPhone = new List<string> { "CHASE_VEHICLE_MEGAPHONE", "STOP_VEHICLE_CAR_MEGAPHONE", "STOP_VEHICLE_CAR_WARNING_MEGAPHONE", "STOP_VEHICLE_GENERIC_MEGAPHONE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE_MEGAPHONE", "COMBAT_TAUNT" };



    private readonly List<string> DeadlyChaseSpeech = new List<string> { "COVER_YOU", "COVER_ME", "DRAW_GUN", "COP_SEES_WEAPON", "COP_SEES_GUN", "GET_HIM", "REQUEST_NOOSE" };
    //{ "DRAW_GUN", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };  
    private readonly List<string> WeaponsFreeSpeech = new List<string> { "CHALLENGE_THREATEN", "FIGHT", "GENERIC_CURSE_HIGH", "GENERIC_WAR_CRY", "OFFICER_DOWN", "SHOOTOUT_OPEN_FIRE", "PINNED_DOWN", "TAKE_COVER" };//,"GENERIC_FRIGHTENED_HIGH" };
    //{ "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_SHOCKED_HIGH", "GENERIC_WAR_CRY", "PINNED_DOWN", "GENERIC_INSULT_HIGH", "GET_HIM" };

    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE", "ARREST_PLAYER","CRIMINAL_APPREHENDED" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "SUSPECT_KILLED_REPORT" };

    private uint GameTimeLastRadioed;
    private uint GameTimeLastSpoke;

    private int TimeBetweenSpeaking;
    private int TimeBetweenRadioIn;
    private uint GameTimeLastForcedRadioSpeech;
    private bool Spoke;

    public CopVoice(PedExt cop, string modelName, ISettingsProvideable settings)
    {
        Cop = cop;
        Settings = settings;
        TimeBetweenRadioIn = 10000 + RandomItems.GetRandomNumberInt(0, 15000);
        TimeBetweenSpeaking = 25000 + RandomItems.GetRandomNumberInt(0, 13000);
    }
    public bool IsRadioTimedOut => Game.GameTime - GameTimeLastRadioed < TimeBetweenRadioIn;
    public bool IsSpeechTimedOut => Game.GameTime - GameTimeLastSpoke < TimeBetweenSpeaking;
    public bool CanRadioIn => !Cop.IsUnconscious && !IsRadioTimedOut && Cop.DistanceToPlayer <= 50f && !Cop.IsInVehicle && !Cop.RecentlyGotOutOfVehicle && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsSwimming && Cop.Pedestrian.Speed <= 0.25f && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && !Cop.Pedestrian.IsInWrithe && !Cop.Pedestrian.IsGettingIntoVehicle && !Cop.Pedestrian.IsInAnyVehicle(true) && !Cop.Pedestrian.IsInAnyVehicle(false);
    public bool CanSpeak => !Cop.IsUnconscious && !IsSpeechTimedOut && Cop.DistanceToPlayer <= 50f;
    public void Speak(IPoliceRespondable currentPlayer)
    {    
        if(Game.GameTime - GameTimeLastForcedRadioSpeech >= 5000 && Cop.Pedestrian.Exists() && (NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Cop.Pedestrian, "random@arrests", "generic_radio_chatter", 3) || NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Cop.Pedestrian, "random@arrests", "radio_chatter", 3)))
        {
            PlaySpeech(new List<string>() { "SETTLE_DOWN", "CRIMINAL_APPREHENDED", "ARREST_PLAYER" }, false, false);
            GameTimeLastForcedRadioSpeech = Game.GameTime;
        }
        else if (Cop.CurrentTask != null && Cop.CurrentTask.OtherTarget != null && Cop.CurrentTask.OtherTarget.Pedestrian.Exists() && Cop.CurrentTask.OtherTarget.Pedestrian.IsAlive)
        {
            if (Cop.CurrentTask.OtherTarget.WantedLevel > currentPlayer.WantedLevel || Cop.CurrentTask.OtherTarget.IsDeadlyChase && currentPlayer.PoliceResponse.IsDeadlyChase)
            {
                SpeakToTarget();
            }
            else
            {
                SpeakToPlayer(currentPlayer);
            }
        }
        else
        {
            SpeakToPlayer(currentPlayer);
        }
    }
    private void SpeakToPlayer(IPoliceRespondable currentPlayer)
    {
        if (CanSpeak)
        {
            if(currentPlayer.IsDangerouslyArmed && Cop.DistanceToPlayer <= 50f)
            {
                TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Armed_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Armed_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Armed_Randomizer_Max);
            }
            else if (currentPlayer.WantedLevel <= 3)
            {
                TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_General_Randomizer_Max);
            }
            else if (currentPlayer.PoliceResponse.IsWeaponsFree)
            {
                TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_WeaponsFree_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max);
            }
            else if (currentPlayer.PoliceResponse.IsDeadlyChase)
            {
                TimeBetweenSpeaking = Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Deadly_Min + RandomItems.GetRandomNumberInt(Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Deadly_Randomizer_Min, Settings.SettingsManager.PoliceSpeechSettings.TimeBetweenCopSpeak_Deadly_Randomizer_Max);
            }
            if (currentPlayer.IsWanted && Cop.CanSeePlayer)
            {
                if (Cop.IsInVehicle)
                {
                    if (currentPlayer.IsInVehicle)
                    {
                        PlaySpeech(InVehiclePlayerInVehicleMegaPhone, false,false);
                    }
                    else 
                    {
                        PlaySpeech(InVehiclePlayerOnFootMegaPhone, false,false);
                    }
                }
                else
                {
                    if (currentPlayer.IsBusted)
                    {
                        if(currentPlayer.WantedLevel == 1)
                        {
                            PlaySpeech(IdleSpeech, Cop.IsInVehicle,false);
                        }
                        else
                        {
                            PlaySpeech(SuspectBusted, Cop.IsInVehicle,false);
                        }
                        
                    }
                    else if (currentPlayer.IsDead)
                    {
                        PlaySpeech(SuspectDown, Cop.IsInVehicle,false);
                    }
                    else
                    {
                        if (currentPlayer.PoliceResponse.IsDeadlyChase)
                        {
                            if (currentPlayer.PoliceResponse.IsWeaponsFree)
                            {
                                PlaySpeech(WeaponsFreeSpeech, Cop.IsInVehicle,true);
                            }
                            else
                            {
                                PlaySpeech(DeadlyChaseSpeech, Cop.IsInVehicle,true);
                            }
                        }
                        else if(currentPlayer.IsDangerouslyArmed)
                        {
                            PlaySpeech(DangerousUnarmedSpeech, Cop.IsInVehicle,true);
                        }
                        else
                        {
                            PlaySpeech(UnarmedChaseSpeech, Cop.IsInVehicle,true);
                        }
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    private void SpeakToTarget()
    {
        if (CanSpeak)
        {
            if (Cop.CurrentTask.OtherTarget.WantedLevel <= 3)
            {
                TimeBetweenSpeaking = 25000 + RandomItems.GetRandomNumberInt(0, 13000);
            }
            else if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
            {
                TimeBetweenSpeaking = 18000 + RandomItems.GetRandomNumberInt(0, 7000);
            }
            if (Cop.CurrentTask.OtherTarget.IsWanted)
            {
                if (Cop.IsInVehicle)
                {
                    if (Cop.CurrentTask.OtherTarget.IsInVehicle)
                    {
                        PlaySpeech(InVehiclePlayerInVehicleMegaPhone, false,false);
                    }
                    else
                    {
                        PlaySpeech(InVehiclePlayerOnFootMegaPhone, false,false);
                    }
                }
                else
                {
                    if (Cop.CurrentTask.OtherTarget.IsBusted)
                    {
                        PlaySpeech(SuspectBusted, Cop.IsInVehicle,false);
                    }
                    else
                    {
                        if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
                        {
                            PlaySpeech(DeadlyChaseSpeech, Cop.IsInVehicle,true);
                        }
                        else
                        {
                            PlaySpeech(UnarmedChaseSpeech, Cop.IsInVehicle,true);
                        }
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    private void PlaySpeech(List<string> Possibilities, bool useMegaphone, bool isShouted)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()).Take(2))
        {
            string voiceName = null;
            bool IsOverWrittingVoice = false;
            if (Cop.VoiceName != "")
            {
                voiceName = Cop.VoiceName;
                IsOverWrittingVoice = true;
            }
            bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(Cop.Pedestrian, AmbientSpeech, false);
            SpeechModifier speechModifier = SpeechModifier.Force;
            if (useMegaphone)
            {
                speechModifier = SpeechModifier.ForceMegaphone;
            }
            else if (isShouted)
            {
                speechModifier = SpeechModifier.ForceShouted;
            }

            if(IsOverWrittingVoice)
            {
                Cop.Pedestrian.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, speechModifier);
            }
            else
            {
                Cop.Pedestrian.PlayAmbientSpeech(AmbientSpeech, useMegaphone);
            }
            GameFiber.Sleep(300);//100
            if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {Cop.Pedestrian.Handle} voiceName {voiceName} Attempting {AmbientSpeech}, Result: {Spoke} IsOverWrittingVoice {IsOverWrittingVoice}", 5);
            if (Spoke)
            {
                break;
            }
        }
    }
    public void ResetSpeech()
    {
        GameTimeLastSpoke = 0;
        GameTimeLastRadioed = 0;
    }
    //public void YellInPain()
    //{
    //    if (CanYell)
    //    {
    //        if (RandomItems.RandomPercent(80))
    //        {
    //            List<int> PossibleYells = new List<int>() { 6, 7, 8 };
    //            int YellType = PossibleYells.PickRandom();
    //            NativeFunction.Natives.PLAY_PAIN(Cop.Pedestrian, YellType, 0, 0);

    //            EntryPoint.WriteToConsole($"YELL IN PAIN {Cop.Pedestrian.Handle} YellType {YellType}");
    //        }
    //        else
    //        {
    //            PlaySpeech("GENERIC_FRIGHTENED_HIGH", Cop.IsInVehicle);
    //            EntryPoint.WriteToConsole($"CRY SPEECH FOR PAIN {Cop.Pedestrian.Handle}");
    //        }

    //        GameTimeLastYelled = Game.GameTime;
    //    }
    //}
}

