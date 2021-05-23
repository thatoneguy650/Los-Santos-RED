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
    private readonly List<string> DeadlyChaseSpeech = new List<string> { "DRAW_GUN", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
    private readonly List<string> SuspectBusted = new List<string> { "WON_DISPUTE", "ARREST_PLAYER" };
    private readonly List<string> SuspectDown = new List<string> { "SUSPECT_KILLED", "SUSPECT_KILLED_REPORT" };
    private readonly List<string> UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "SUSPECT_SPOTTED" };
    private readonly List<string> IdleSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
    private readonly List<string> AngrySpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_SHOCKED_HIGH", "GENERIC_WAR_CRY", "PINNED_DOWN", "GENERIC_INSULT_HIGH", "GET_HIM" };

    private uint GameTimeLastRadioed;
    private uint GameTimeLastSpoke;

    public Voice(Cop cop)
    {
        Cop = cop;
    }

    public bool IsRadioTimedOut => GameTimeLastRadioed != 0 && Game.GameTime - GameTimeLastRadioed < 60000;
    public bool IsSpeechTimedOut => GameTimeLastSpoke != 0 && Game.GameTime - GameTimeLastSpoke < 25000;
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
        if (CanSpeak)
        {
            if (currentPlayer.IsWanted)
            {
                if (currentPlayer.IsBusted)
                {
                    Cop.Pedestrian.PlayAmbientSpeech(SuspectBusted.PickRandom(),Cop.IsInVehicle);
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
                            Cop.Pedestrian.PlayAmbientSpeech(AngrySpeech.PickRandom(), Cop.IsInVehicle);
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
            else
            {
                if (!Cop.IsInVehicle)
                {
                    Cop.Pedestrian.PlayAmbientSpeech(IdleSpeech.PickRandom(), false);
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
    }
}

