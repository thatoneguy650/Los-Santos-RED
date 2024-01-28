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
  //  public bool CanRadioIn => !Cop.IsUnconscious && !IsRadioTimedOut && Cop.DistanceToPlayer <= 50f && !Cop.IsInVehicle && !Cop.RecentlyGotOutOfVehicle && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsSwimming && Cop.Pedestrian.Speed <= 0.25f && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && !Cop.Pedestrian.IsInWrithe && !Cop.Pedestrian.IsGettingIntoVehicle && !Cop.Pedestrian.IsInAnyVehicle(true) && !Cop.Pedestrian.IsInAnyVehicle(false);

    public bool CanRadioIn => !Cop.IsUnconscious && Cop.DistanceToPlayer <= 100f && !Cop.IsInVehicle && !Cop.RecentlyGotOutOfVehicle && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsSwimming && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && !Cop.Pedestrian.IsInWrithe && !Cop.Pedestrian.IsGettingIntoVehicle && !Cop.Pedestrian.IsInAnyVehicle(true) && !Cop.Pedestrian.IsInAnyVehicle(false);


    public bool CanSpeak => !Cop.IsUnconscious && !IsSpeechTimedOut && Cop.DistanceToPlayer <= 50f && !Cop.IsBeingHeldAsHostage;
    public void Speak(IPoliceRespondable currentPlayer)
    {    
        if(Game.GameTime - GameTimeLastForcedRadioSpeech >= 5000 && Cop.Pedestrian.Exists() && (NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Cop.Pedestrian, "random@arrests", "generic_radio_chatter", 3) || NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Cop.Pedestrian, "random@arrests", "radio_chatter", 3)))
        {
            Cop.PlaySpeech(new List<string>() { "SETTLE_DOWN", "CRIMINAL_APPREHENDED", "ARREST_PLAYER" }, false, false);
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
        if(!CanSpeak)
        {
            return;
        }  
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
        if (currentPlayer.IsWanted && Cop.RecentlySeenPlayer)
        {
            if (Cop.IsInVehicle)
            {
                if (Cop.IsDriver)
                {
                    if (currentPlayer.IsInVehicle)
                    {
                        Cop.PlaySpeech(InVehiclePlayerInVehicleMegaPhone, false, false);
                    }
                    else
                    {
                        Cop.PlaySpeech(InVehiclePlayerOnFootMegaPhone, false, false);
                    }
                }
            }
            else
            {
                if (currentPlayer.IsBusted)
                {
                    if(currentPlayer.WantedLevel == 1)
                    {
                        Cop.PlaySpeech(IdleSpeech, Cop.IsInVehicle,false);
                    }
                    else
                    {
                        Cop.PlaySpeech(SuspectBusted, Cop.IsInVehicle,false);
                    }
                        
                }
                else if (currentPlayer.IsDead)
                {
                    Cop.PlaySpeech(SuspectDown, Cop.IsInVehicle,false);
                }
                else
                {
                    if (currentPlayer.PoliceResponse.IsDeadlyChase)
                    {
                        if (currentPlayer.PoliceResponse.IsWeaponsFree)
                        {
                            Cop.PlaySpeech(WeaponsFreeSpeech, Cop.IsInVehicle,true);
                        }
                        else
                        {
                            Cop.PlaySpeech(DeadlyChaseSpeech, Cop.IsInVehicle,true);
                        }
                    }
                    else if(currentPlayer.IsDangerouslyArmed)
                    {
                        Cop.PlaySpeech(DangerousUnarmedSpeech, Cop.IsInVehicle,true);
                    }
                    else
                    {
                        Cop.PlaySpeech(UnarmedChaseSpeech, Cop.IsInVehicle,true);
                    }
                }
            }
        }
        GameTimeLastSpoke = Game.GameTime;     
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
                        Cop.PlaySpeech(InVehiclePlayerInVehicleMegaPhone, false,false);
                    }
                    else
                    {
                        Cop.PlaySpeech(InVehiclePlayerOnFootMegaPhone, false,false);
                    }
                }
                else
                {
                    if (Cop.CurrentTask.OtherTarget.IsBusted)
                    {
                        Cop.PlaySpeech(SuspectBusted, Cop.IsInVehicle,false);
                    }
                    else
                    {
                        if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
                        {
                            Cop.PlaySpeech(DeadlyChaseSpeech, Cop.IsInVehicle,true);
                        }
                        else
                        {
                            Cop.PlaySpeech(UnarmedChaseSpeech, Cop.IsInVehicle,true);
                        }
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    public void ResetSpeech()
    {
        GameTimeLastSpoke = 0;
        GameTimeLastRadioed = 0;
    }
    public void RadioInWanted(IPoliceRespondable currentPlayer)
    {
        if(!Cop.Pedestrian.Exists() || !CanRadioIn || currentPlayer.IsBusted || currentPlayer.IsDead)
        {
            return;
        }
        if(!AnimationDictionary.RequestAnimationDictionayResult("random@arrests"))
        {
            EntryPoint.WriteToConsole("RadioInWanted FAIL COULD NOT LOAD ANIMATION DICTIONARY");
            return;
        }
        ResetSpeech();
        if (Settings.SettingsManager.PoliceSettings.AllowRadioInAnimation && Cop.CanPlayRadioInAnimation)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_enter", 2.0f, -2.0f, 1000, 16 | 32, 0, false, false, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_chatter", 2.0f, -2.0f, 2000, 16 | 32, 0, false, false, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_exit", 2.0f, -2.0f, 1000, 16 | 32, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        Cop.CurrentTask = null;
        SpeakToPlayer(currentPlayer);
        //Cop.ClearTasks(true);
    }
}

