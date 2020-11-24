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
    private static List<string> RegularChaseSpeech;
    private static List<SpeakingCop> SpeakingCops;
    public static bool IsRunning { get; set; }
    static PoliceSpeech()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        IsRunning = true;
        SetupLists();
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            try
            {
                AddSpeakingCops();
                CheckSpeech();
            }
            catch (Exception e)
            {
                Debugging.WriteToLog(e.Message, e.StackTrace);
            }
        }
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void SetupLists()
    {
        DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_INSULT", "GENERIC_WAR_CRY", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
        UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED" };
        CautiousChaseSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
        RegularChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED", "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
        AmbientSpeech = new List<string> { "CHAT_STATE", "CHAT_RESP" };
        ArrestedWaitSpeech = new List<string> { "WON_DISPUTE" };
        PlayerDeadSpeech = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE", "SUSPECT_KILLED_REPORT" };
        SpeakingCops = new List<SpeakingCop>();
    }
    private static void AddSpeakingCops()
    {
        PedList.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());
        SpeakingCops.RemoveAll(x => !x.AssignedCop.Pedestrian.Exists());
        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists()))
        {
            if (!SpeakingCops.Any(x => x.AssignedCop.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                SpeakingCops.Add(new SpeakingCop(Cop));
            }
        }
    }
    private static void CheckSpeech()
    {
        foreach (SpeakingCop Cop in SpeakingCops.Where(x => x.AssignedCop.Pedestrian.Exists() && !x.AssignedCop.Pedestrian.IsDead))
        {
            if (Cop.CanSpeak && Cop.AssignedCop.DistanceToPlayer <= 45f)
            {
                Cop.Speak();
            }
            if (Cop.CanRadioIn && Cop.AssignedCop.CanSeePlayer && Crimes.IsViolatingAnyCrimes)
            {
                Cop.RadioIn();
            }
        }
    }
    private class SpeakingCop
    {
        private uint GameTimeLastSpoke;
        private uint GameTimeLastRadioed;

        public SpeakingCop(Cop assignedCop)
        {
            AssignedCop = assignedCop;
        }

        public Cop AssignedCop { get; set; }
        public bool IsSpeechTimedOut
        {
            get
            {
                if (GameTimeLastSpoke == 0)
                    return false;
                else if (Game.GameTime - GameTimeLastSpoke >= 15000)
                    return false;
                else
                    return true;
            }
        }
        public bool CanSpeak
        {
            get
            {
                if (IsSpeechTimedOut)
                    return false;
                else
                    return true;
            }
        }
        public bool IsRadioTimedOut
        {
            get
            {
                if (GameTimeLastRadioed == 0)
                    return false;
                else if (Game.GameTime - GameTimeLastRadioed >= 15000)
                    return false;
                else
                    return true;
            }
        }
        public bool CanRadioIn
        {
            get
            {
                if (IsRadioTimedOut)
                    return false;
                else if (!AssignedCop.Pedestrian.IsInAnyVehicle(false) && !AssignedCop.Pedestrian.IsSwimming && !AssignedCop.Pedestrian.IsInCover && !AssignedCop.Pedestrian.IsGoingIntoCover && !AssignedCop.Pedestrian.IsShooting && !AssignedCop.Pedestrian.IsInWrithe)
                    return true;
                else
                    return false;
            }
        }
        public void Speak()
        {
            if (PlayerState.IsBusted && AssignedCop.DistanceToPlayer <= 20f)
            {
                AssignedCop.Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
            }
            else if (PedDamage.RecentlyKilledCop)
            {
                AssignedCop.Pedestrian.PlayAmbientSpeech("OFFICER_DOWN");
            }
            else if (PlayerState.IsWanted && !WantedLevelScript.IsDeadlyChase)
            {
                AssignedCop.Pedestrian.PlayAmbientSpeech(RegularChaseSpeech.PickRandom());
            }
            else if (PlayerState.IsNotWanted && Respawn.RecentlyBribedPolice)
            {
                AssignedCop.Pedestrian.PlayAmbientSpeech(AmbientSpeech.PickRandom());
            }
            else if (WantedLevelScript.IsDeadlyChase)
            {
                AssignedCop.Pedestrian.PlayAmbientSpeech(DeadlyChaseSpeech.PickRandom());
            }
            else //Normal State
            {
                if (AssignedCop.DistanceToPlayer <= 4f)
                {
                    AssignedCop.Pedestrian.PlayAmbientSpeech("CRIMINAL_WARNING");
                }
            }
            GameTimeLastSpoke = Game.GameTime;
        }
        public void RadioIn()
        {
    
            string AnimationToPlay = "generic_radio_enter";
            GTAWeapon CurrentGun = General.GetCurrentWeapon(AssignedCop.Pedestrian);
            if (CurrentGun != null && CurrentGun.IsOneHanded)
                AnimationToPlay = "radio_enter";

            Speak();

            General.RequestAnimationDictionay("random@arrests");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", AssignedCop.Pedestrian, "random@arrests", AnimationToPlay, 2.0f, -2.0f, -1, 52, 0, false, false, false);
            GameTimeLastRadioed = Game.GameTime;

        }
    }

}

