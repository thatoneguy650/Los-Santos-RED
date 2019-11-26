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
    private static Random rnd;
    public static bool IsRunning { get; set; } = true;
    public static List<string> DeadlyChaseSpeech { get; set; }
    public static List<string> UnarmedChaseSpeech { get; set; }
    public static List<string> CautiousChaseSpeech { get; set; }
    public static List<string> ArrestedWaitSpeech { get; set; }
    public static List<string> PlayerDeadSpeech { get; set; }
    public static bool SaidManDown { get; set; } = true;
    static PoliceSpeech()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        SetupSpeech();
        MainLoop();
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                CheckSpeech();
                GameFiber.Yield();
            }
        });
    }
    private static void SetupSpeech()
    {
        DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN", "COMBAT_TAUNT", "FIGHT", "GENERIC_INSULT", "GENERIC_WAR_CRY", "GET_HIM", "REQUEST_BACKUP", "REQUEST_NOOSE", "SHOOTOUT_OPEN_FIRE" };
        UnarmedChaseSpeech = new List<string> { "FOOT_CHASE", "FOOT_CHASE_AGGRESIVE", "FOOT_CHASE_LOSING", "FOOT_CHASE_RESPONSE", "GET_HIM", "SUSPECT_SPOTTED" };
        CautiousChaseSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL" };
        ArrestedWaitSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL","SURROUNDED" };

        PlayerDeadSpeech = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE" };
    }
    private static void CheckSpeech()
    {
        try
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CanSpeak && x.DistanceToPlayer <= 45f && x.CopPed.Exists() && !x.CopPed.IsDead))
            {
                //if (rnd.Next(0, 100) <= 10)
                //    return;

                if (Cop.isTasked)
                {
                    if (InstantAction.isBusted && Cop.DistanceToPlayer <= 20f)
                    {
                        Cop.CopPed.PlayAmbientSpeech("ARREST_PLAYER");
                       // InstantAction.WriteToLog("CheckSpeech", "ARREST_PLAYER");
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase)
                    {
                        string Speech = UnarmedChaseSpeech.PickRandom();
                        Cop.CopPed.PlayAmbientSpeech(Speech);
                       // InstantAction.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.CautiousChase)
                    {
                        string Speech = CautiousChaseSpeech.PickRandom();
                        Cop.CopPed.PlayAmbientSpeech(Speech);
                       // InstantAction.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait)
                    {
                        string Speech = ArrestedWaitSpeech.PickRandom();
                        Cop.CopPed.PlayAmbientSpeech(Speech);
                       // InstantAction.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                    {
                        string Speech = DeadlyChaseSpeech.PickRandom();
                        Cop.CopPed.PlayAmbientSpeech(Speech);
                        //InstantAction.WriteToLog("CheckSpeech", Speech);
                    }
                    else //Normal State
                    {
                        if(Cop.DistanceToPlayer <= 4f)
                        {
                            Cop.CopPed.PlayAmbientSpeech("CRIMINAL_WARNING");
                            //InstantAction.WriteToLog("CheckSpeech", "CRIMINAL_WARNING");
                        }
                    }
                }
                else
                {
                    if(InstantAction.isDead && Cop.DistanceToPlayer <= 20f)
                    {
                        string Speech = PlayerDeadSpeech.PickRandom();
                        Cop.CopPed.PlayAmbientSpeech(Speech);
                        //InstantAction.WriteToLog("CheckSpeech", Speech);
                    }
                }
                Cop.GameTimeLastSpoke = Game.GameTime - (uint)rnd.Next(500,1000);
            }
           
        }
        catch (Exception e)
        {
            Game.Console.Print(e.Message);
        }
    }

}

