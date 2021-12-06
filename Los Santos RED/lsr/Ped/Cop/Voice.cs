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

    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED", "COP_ARRIVAL_ANNOUNCE", "COMBAT_TAUNT" };
    private readonly List<string> DeadlyChaseSpeech = new List<string> { "COVER_YOU", "COVER_ME", "DRAW_GUN", "COP_SEES_WEAPON", "COP_SEES_GUN", "GET_HIM", "REQUEST_NOOSE" };
    //{ "DRAW_GUN", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };  
    private readonly List<string> WeaponsFreeSpeech = new List<string> { "CHALLENGE_THREATEN", "FIGHT","GENERIC_CURSE_HIGH","GENERIC_FRIGHTENED_HIGH","GENERIC_WAR_CRY","OFFICER_DOWN", "SHOOTOUT_OPEN_FIRE", "PINNED_DOWN", "TAKE_COVER" };
    //{ "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_SHOCKED_HIGH", "GENERIC_WAR_CRY", "PINNED_DOWN", "GENERIC_INSULT_HIGH", "GET_HIM" };

    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE", "ARREST_PLAYER","CRIMINAL_APPREHENDED" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "SUSPECT_KILLED_REPORT" };

    private uint GameTimeLastRadioed;
    private uint GameTimeLastSpoke;

    public Voice(Cop cop)
    {
        Cop = cop;
    }
    public bool IsRadioTimedOut => GameTimeLastRadioed != 0 && Game.GameTime - GameTimeLastRadioed < 60000;
    public bool IsSpeechTimedOut => GameTimeLastSpoke != 0 && Game.GameTime - GameTimeLastSpoke < TimeBetweenSpeaking;
    public int TimeBetweenSpeaking { get; private set; }
    public bool CanRadioIn => !IsRadioTimedOut && Cop.DistanceToPlayer <= 50f && !Cop.IsInVehicle && !Cop.RecentlyGotOutOfVehicle && !Cop.Pedestrian.IsSwimming && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && !Cop.Pedestrian.IsInWrithe && !Cop.Pedestrian.IsGettingIntoVehicle && !Cop.Pedestrian.IsInAnyVehicle(true) && !Cop.Pedestrian.IsInAnyVehicle(false);
    public bool CanSpeak => !IsSpeechTimedOut && Cop.DistanceToPlayer <= 50f;
    public void RadioIn(IPoliceRespondable currentPlayer)
    {
        if (CanRadioIn && currentPlayer.IsWanted)
        {
            string AnimationToPlay = "generic_radio_enter";
            //WeaponInformation CurrentGun = DataMart.Instance.Weapons.GetCurrentWeapon(Pedestrian);
            //if (CurrentGun != null && CurrentGun.IsOneHanded)
            //    AnimationToPlay = "radio_enter";
            Speak(currentPlayer);
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Cop.Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;
        }
    }
    public void Speak(IPoliceRespondable currentPlayer)
    {
        if(Cop.CurrentTask != null && Cop.CurrentTask.OtherTarget != null && Cop.CurrentTask.OtherTarget.Pedestrian.Exists() && Cop.CurrentTask.OtherTarget.Pedestrian.IsAlive)
        {
            if(Cop.CurrentTask.OtherTarget.WantedLevel > currentPlayer.WantedLevel || Cop.CurrentTask.OtherTarget.IsDeadlyChase && currentPlayer.PoliceResponse.IsDeadlyChase)
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
        if (currentPlayer.WantedLevel <= 3)
        {
            TimeBetweenSpeaking = 25000;
        }
        else if (currentPlayer.PoliceResponse.IsWeaponsFree)
        {
            TimeBetweenSpeaking = 10000;
        }
        else if (currentPlayer.PoliceResponse.IsDeadlyChase)
        {
            TimeBetweenSpeaking = 18000;
        }
        if (CanSpeak)
        {
            if (currentPlayer.IsWanted)
            {
                if (currentPlayer.IsBusted)
                {
                    Cop.Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom(), Cop.IsInVehicle);
                }
                else if (currentPlayer.IsDead)
                {
                    Cop.Pedestrian.PlayAmbientSpeech(SuspectDown.PickRandom(), Cop.IsInVehicle);
                }
                else
                {
                    if (currentPlayer.PoliceResponse.IsDeadlyChase)
                    {
                        if (currentPlayer.PoliceResponse.IsWeaponsFree)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(WeaponsFreeSpeech.PickRandom(), Cop.IsInVehicle);
                        }
                        else
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom(), Cop.IsInVehicle);
                        }
                    }
                    else
                    {
                        Cop.Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom(), Cop.IsInVehicle);
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
    private void SpeakToTarget()
    {
        if (Cop.CurrentTask.OtherTarget.WantedLevel <= 3)
        {
            TimeBetweenSpeaking = 25000;
        }
        else if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
        {
            TimeBetweenSpeaking = 18000;
        }
        if (CanSpeak)
        {
            if (Cop.CurrentTask.OtherTarget.IsWanted)
            {
                if (Cop.CurrentTask.OtherTarget.IsBusted)
                {
                    Cop.Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom(), Cop.IsInVehicle);
                }
                else
                {
                    if (Cop.CurrentTask.OtherTarget.IsDeadlyChase)
                    {
                        Cop.Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom(), Cop.IsInVehicle);
                    }
                    else
                    {
                        Cop.Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom(), Cop.IsInVehicle);
                    }
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
}

