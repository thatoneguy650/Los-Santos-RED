using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Voice
{
    private Cop Cop;
    private bool IsInFiber = false;

    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE", "COMBAT_TAUNT" };

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
    public Voice(Cop cop, string modelName)
    {
        Cop = cop;
        TimeBetweenRadioIn = 10000 + RandomItems.GetRandomNumberInt(0, 15000);
        TimeBetweenSpeaking = 25000 + RandomItems.GetRandomNumberInt(0, 13000);
    }
    public bool IsRadioTimedOut => Game.GameTime - GameTimeLastRadioed < TimeBetweenRadioIn;
    public bool IsSpeechTimedOut => Game.GameTime - GameTimeLastSpoke < TimeBetweenSpeaking;
    public bool CanRadioIn => !Cop.IsUnconscious && !IsRadioTimedOut && Cop.DistanceToPlayer <= 50f && !Cop.IsInVehicle && !Cop.RecentlyGotOutOfVehicle && !Cop.Pedestrian.IsSwimming && Cop.Pedestrian.Speed <= 0.25f && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && !Cop.Pedestrian.IsInWrithe && !Cop.Pedestrian.IsGettingIntoVehicle && !Cop.Pedestrian.IsInAnyVehicle(true) && !Cop.Pedestrian.IsInAnyVehicle(false);
    public bool CanSpeak => !Cop.IsUnconscious && !IsSpeechTimedOut && Cop.DistanceToPlayer <= 50f;

    public void RadioIn(IPoliceRespondable currentPlayer)
    {
        if (CanRadioIn && !IsInFiber && ((Cop.CurrentTask?.OtherTarget?.IsBusted == true && Cop.CurrentTask?.OtherTarget?.ArrestingPedHandle == Cop.Handle) || (Cop.CurrentTask?.OtherTarget == null && currentPlayer.IsBusted && currentPlayer.ArrestingCop.Handle == Cop.Handle)))
        {
            TimeBetweenRadioIn = 10000 + RandomItems.GetRandomNumberInt(0, 25000);
            GameTimeLastRadioed = Game.GameTime;

            if(Cop.Pedestrian.Exists() )
            {

            }

            GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
            {
                IsInFiber = true;
                AnimationDictionary.RequestAnimationDictionay("random@arrests");
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Cop.Pedestrian, "random@arrests", "radio_enter", 2.0f, -2.0f, -1, 0, 0, false, false, false);
                GameFiber.Wait(1000);
                if (Cop.Pedestrian.Exists() && ((Cop.CurrentTask?.OtherTarget?.IsBusted == true && Cop.CurrentTask?.OtherTarget?.ArrestingPedHandle == Cop.Handle) || (Cop.CurrentTask?.OtherTarget == null && currentPlayer.IsBusted)))
                {
                    PlaySpeech(new List<string>() { "SETTLE_DOWN", "CRIMINAL_APPREHENDED", "ARREST_PLAYER" }.PickRandom(), false);
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Cop.Pedestrian, "random@arrests", "radio_chatter", 2.0f, -2.0f, -1, 0, 0, false, false, false);
                    GameFiber.Wait(2000);
                }
                
                if (Cop.Pedestrian.Exists() && ((Cop.CurrentTask?.OtherTarget?.IsBusted == true && Cop.CurrentTask?.OtherTarget?.ArrestingPedHandle == Cop.Handle) || (Cop.CurrentTask?.OtherTarget == null && currentPlayer.IsBusted)))
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Cop.Pedestrian, "random@arrests", "radio_exit", 2.0f, -2.0f, -1, 0, 0, false, false, false);
                    GameFiber.Wait(1000);
                }
                IsInFiber = false;
                Cop.CurrentTask?.ReTask();//will this work to reset the task stuff?
                GameTimeLastRadioed = Game.GameTime;
            }, "SetArrestedAnimation");
        }
    }
    public void Speak(IPoliceRespondable currentPlayer)
    {
        if (Cop.CurrentTask != null && Cop.CurrentTask.OtherTarget != null && Cop.CurrentTask.OtherTarget.Pedestrian.Exists() && Cop.CurrentTask.OtherTarget.Pedestrian.IsAlive)
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
            if (currentPlayer.WantedLevel <= 3)
            {
                TimeBetweenSpeaking = 25000 + RandomItems.GetRandomNumberInt(0, 13000);
            }
            else if (currentPlayer.PoliceResponse.IsWeaponsFree)
            {
                TimeBetweenSpeaking = 10000 + RandomItems.GetRandomNumberInt(0, 4000);
            }
            else if (currentPlayer.PoliceResponse.IsDeadlyChase)
            {
                TimeBetweenSpeaking = 18000 + RandomItems.GetRandomNumberInt(0, 7000);
            }
            if (currentPlayer.IsWanted)
            {
                if (Cop.IsInVehicle)
                {
                    if (currentPlayer.IsInVehicle)
                    {
                        PlaySpeech(InVehiclePlayerInVehicleMegaPhone.PickRandom(), false);
                    }
                    else 
                    {
                        PlaySpeech(InVehiclePlayerOnFootMegaPhone.PickRandom(), false);
                    }
                }
                else
                {
                    if (currentPlayer.IsBusted)
                    {
                        PlaySpeech(SuspectBusted.PickRandom(), Cop.IsInVehicle);
                    }
                    else if (currentPlayer.IsDead)
                    {
                        PlaySpeech(SuspectDown.PickRandom(), Cop.IsInVehicle);
                    }
                    else
                    {
                        if (currentPlayer.PoliceResponse.IsDeadlyChase)
                        {
                            if (currentPlayer.PoliceResponse.IsWeaponsFree)
                            {
                                PlaySpeech(WeaponsFreeSpeech.PickRandom(), Cop.IsInVehicle);
                            }
                            else
                            {
                                PlaySpeech(DeadlyChaseSpeech.PickRandom(), Cop.IsInVehicle);
                            }
                        }
                        else
                        {
                            PlaySpeech(UnarmedChaseSpeech.PickRandom(), Cop.IsInVehicle);
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
                        PlaySpeech(InVehiclePlayerInVehicleMegaPhone.PickRandom(), false);
                    }
                    else
                    {
                        PlaySpeech(InVehiclePlayerOnFootMegaPhone.PickRandom(), false);
                    }
                }
                else
                {
                    if (Cop.CurrentTask.OtherTarget.IsBusted)
                    {
                        PlaySpeech(SuspectBusted.PickRandom(), Cop.IsInVehicle);
                    }
                    else
                    {
                        if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
                        {
                            PlaySpeech(DeadlyChaseSpeech.PickRandom(), Cop.IsInVehicle);
                        }
                        else
                        {
                            PlaySpeech(UnarmedChaseSpeech.PickRandom(), Cop.IsInVehicle);
                        }
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    private void PlaySpeech(string speechName,bool useMegaphone)
    {
        if(Cop.VoiceName != "")// isFreeMode)
        {
            if(useMegaphone)
            {
                Cop.Pedestrian.PlayAmbientSpeech(Cop.VoiceName, speechName, 0, SpeechModifier.ForceMegaphone);
                
            }
            else
            {
                Cop.Pedestrian.PlayAmbientSpeech(Cop.VoiceName, speechName, 0, SpeechModifier.Force);
            }
            EntryPoint.WriteToConsole($"FREEMODE COP SPEAK {Cop.Pedestrian.Handle} freeModeVoice {Cop.VoiceName} speechName {speechName}");
        }
        else
        {
            Cop.Pedestrian.PlayAmbientSpeech(speechName, useMegaphone);
            EntryPoint.WriteToConsole($"REGULAR COP SPEAK {Cop.Pedestrian.Handle} freeModeVoice {Cop.VoiceName} speechName {speechName}");
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

