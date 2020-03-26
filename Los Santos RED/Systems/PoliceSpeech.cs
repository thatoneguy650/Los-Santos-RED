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

        ArrestedWaitSpeech = new List<string> { "DRAW_GUN", "GET_HIM", "COP_ARRIVAL_ANNOUNCE", "MOVE_IN", "MOVE_IN_PERSONAL", "SURROUNDED" };

        PlayerDeadSpeech = new List<string> { "SUSPECT_KILLED", "WON_DISPUTE" };
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void CheckSpeech()
    {
        try
        {
            foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.CanSpeak && x.DistanceToPlayer <= 45f && x.Pedestrian.Exists() && !x.Pedestrian.IsDead))
            {
                if (Cop.IsTasked)
                {
                    if (LosSantosRED.IsBusted && Cop.DistanceToPlayer <= 20f)
                    {
                        Cop.Pedestrian.PlayAmbientSpeech("ARREST_PLAYER");
                        Debugging.WriteToLog("CheckSpeech", "ARREST_PLAYER");
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase)
                    {
                        string Speech = UnarmedChaseSpeech.PickRandom();
                        Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        Debugging.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.CautiousChase)
                    {
                        string Speech = CautiousChaseSpeech.PickRandom();
                        Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        Debugging.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait)
                    {
                        //string Speech = ArrestedWaitSpeech.PickRandom();
                        //Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        //LocalWriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.Normal && Respawning.RecentlyBribedPolice)
                    {
                        string Speech = AmbientSpeech.PickRandom();
                        Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        Debugging.WriteToLog("CheckSpeech", Speech);
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                    {
                        string Speech = DeadlyChaseSpeech.PickRandom();
                        Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        Debugging.WriteToLog("CheckSpeech", Speech);
                    }
                    else //Normal State
                    {
                        if(Cop.DistanceToPlayer <= 4f)
                        {
                            Cop.Pedestrian.PlayAmbientSpeech("CRIMINAL_WARNING");
                            Debugging.WriteToLog("CheckSpeech", "CRIMINAL_WARNING");
                        }
                    }
                }
                else
                {
                    if(LosSantosRED.IsDead && Cop.DistanceToPlayer <= 20f)
                    {
                        string Speech = PlayerDeadSpeech.PickRandom();
                        Cop.Pedestrian.PlayAmbientSpeech(Speech);
                        Debugging.WriteToLog("CheckSpeech", Speech);
                    }
                }
                Cop.GameTimeLastSpoke = Game.GameTime - (uint)rnd.Next(500,1000);
            }          
        }
        catch (Exception e)
        {
            Debugging.WriteToLog(e.Message,e.StackTrace);
        }
    }

}

