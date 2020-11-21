using ExtensionsMethods;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

internal static class PoliceSpeech
{
    private static readonly Random rnd;
    private static List<string> DeadlyChaseSpeech;
    private static List<string> UnarmedChaseSpeech;
    private static List<string> CautiousChaseSpeech;
    private static List<string> ArrestedWaitSpeech;
    private static List<string> PlayerDeadSpeech;
    private static List<string> AmbientSpeech;
    public static bool IsRunning { get; set; }
    static PoliceSpeech()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        IsRunning = true;
        DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_INSULT", "GENERIC_WAR_CRY", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
        UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED" };
        CautiousChaseSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
        AmbientSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };

        ArrestedWaitSpeech = new List<string> { "WON_DISPUTE" };

        PlayerDeadSpeech = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE", "SUSPECT_KILLED_REPORT" };
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            try
            {
                foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead))
                {
                    if (Cop.CanSpeak && Cop.DistanceToPlayer <= 45f)
                    {
                        if (PlayerState.IsBusted && Cop.DistanceToPlayer <= 20f)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
                        }
                        else if (PedWounds.RecentlyKilledCop)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech("OFFICER_DOWN");
                        }
                        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.UnarmedChase)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(UnarmedChaseSpeech.PickRandom());
                        }
                        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.CautiousChase)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(CautiousChaseSpeech.PickRandom());
                        }
                        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.ArrestedWait)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(ArrestedWaitSpeech.PickRandom());
                        }
                        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.Normal && Respawn.RecentlyBribedPolice)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(AmbientSpeech.PickRandom());
                        }
                        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom());
                        }
                        else //Normal State
                        {
                            if (Cop.DistanceToPlayer <= 4f)
                            {
                                Cop.Pedestrian.PlayAmbientSpeech("CRIMINAL_WARNING");
                            }
                        } 
                        Cop.GameTimeLastSpoke = Game.GameTime;
                    }
                    if (Cop.CanSeePlayer && Crimes.IsViolatingAnyCrimes)
                    {
                        RadioIn(Cop);
                    }
                }
            }
            catch (Exception e)
            {
                Debugging.WriteToLog(e.Message, e.StackTrace);
            }
        }
    }
    private static void RadioIn(Cop Cop)
    {
        if (!Cop.Pedestrian.IsInAnyVehicle(false) && !Cop.Pedestrian.IsSwimming && !Cop.Pedestrian.IsInCover && !Cop.Pedestrian.IsGoingIntoCover && !Cop.Pedestrian.IsShooting && Cop.CanRadio)
        {
            Cop.Pedestrian.PlayAmbientSpeech(CautiousChaseSpeech.PickRandom());
            Cop.GameTimeLastSpoke = Game.GameTime - (uint)rnd.Next(500, 1000);
            General.RequestAnimationDictionay("random@arrests");
            string AnimationToPlay = "generic_radio_enter";
            GTAWeapon CurrentGun = General.GetCurrentWeapon(Cop.Pedestrian);
            if (CurrentGun != null && CurrentGun.IsOneHanded)
                AnimationToPlay = "radio_enter";
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Cop.Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            Cop.GameTimeLastRadioed = Game.GameTime;
        }
    }

}

