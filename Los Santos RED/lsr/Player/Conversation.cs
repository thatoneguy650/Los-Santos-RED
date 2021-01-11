using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Conversation
{
    private bool HasGreeted;
    private PedExt Ped;
    private Mod.Player Player;
    private bool IsConversationNegative;
    private int ScaleformID;
    public Conversation(Mod.Player player, PedExt ped)
    {
        Player = player;
        Ped = ped;
    }
    private bool CanContinueConversation => Player.IsConversing && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 10f && !Ped.Pedestrian.IsFleeing && Ped.Pedestrian.IsAlive && !Game.IsKeyDown(System.Windows.Forms.Keys.H);
    public void Start()
    {
        Player.IsConversing = true;
        NativeFunction.CallByName<bool>("SET_GAMEPLAY_PED_HINT", Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
        Game.Console.Print($"Conversation Started");
        GameFiber.StartNew(delegate
        {
            Setup();
            
            Tick();
            Dispose();
        }, "Conversation");
    }
    private void CheckInput()
    {
        if (Game.IsKeyDown(System.Windows.Forms.Keys.O))
        {
            Positive();
        }
        else if (Game.IsKeyDown(System.Windows.Forms.Keys.L))
        {
            Negative();
        }
    }
    private void GetAttention()
    {
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "OVER_THERE", "GENERIC_HI" });
        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HOWS_IT_GOING", "OVER_THERE", "GENERIC_HI" });
    }
    private void Positive()
    {
        IsConversationNegative = false;
        if (HasGreeted)
        {
            SaySmallTalk(Player.Character, false);
            SaySmallTalk(Ped.Pedestrian, true);
        }
        else
        {
            SayGreeting(Player.Character, true);
            SayGreeting(Ped.Pedestrian, true);
        }
        GameFiber.Sleep(1000);
    }
    private void Negative()
    {
        IsConversationNegative = true;
        if (!HasGreeted)
        {
            SayInsult(Player.Character, false);
            SayInsult(Ped.Pedestrian, false);
        }
        else
        {
            SayInsult(Player.Character, true);
            SayInsult(Ped.Pedestrian, true);
        }
        GameFiber.Sleep(1000);
    }
    private void DisplayPrompt()
    {
        


        



        if (!HasGreeted)
        {
            Game.DisplayHelp("Press H to Stop Talking~n~Press O to Greet~n~Press L to Insult");
        }
        else
        {
            Game.DisplayHelp("Press H to Stop Talking~n~Press O for Positive~n~Press L for Negative");
        }
    }
    public void Dispose()
    {
        Player.IsConversing = false;
        Ped.Pedestrian.BlockPermanentEvents = false;
        Ped.Pedestrian.Tasks.Clear();
        Player.Character.Tasks.Clear();
        NativeFunction.CallByName<bool>("STOP_GAMEPLAY_HINT", true);
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        Game.Console.Print($"CONVERSATION Can {ToSpeak.Handle} Say {Speech}? {CanSay}");
        return CanSay;
    }
    private void SaySmallTalk(Ped ToReply, bool IsReply)
    {
        SayAvailableAmbient(ToReply, new List<string>() { "CHAT_STATE", "CHAT_RESP", "GENERIC_WHATEVER" });
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities)
        {
            ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
            GameFiber.Sleep(100);
            if (ToSpeak.IsAnySpeechPlaying)
            {
                Spoke = true;
                break;
            }
        }
        GameFiber.Sleep(100);
        while (ToSpeak.IsAnySpeechPlaying)
        {
            Spoke = true;
            GameFiber.Yield();
        }
        return Spoke;
    }
    private void SayGreeting(Ped ToReply, bool Positively)
    {
        if (Positively)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_HI", "GENERIC_WHATEVER" });
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "PROVOKE_GENERIC", "GENERIC_INSULT_MED", "GENERIC_WHATEVER" });
        }
        HasGreeted = true;
    }
    private void SayInsult(Ped ToReply, bool IsSevere)
    {
        if (IsSevere)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_HIGH", "GENERIC_INSULT_MED", "GENERIC_WHATEVER" });
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_MED", "GENERIC_WHATEVER" });
        }
        HasGreeted = true;
    }
    private void Setup()
    {






        //Ped.Pedestrian.Tasks.Clear();
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Player.Character, Ped.Pedestrian, -1, 0, 2);
        Ped.Pedestrian.BlockPermanentEvents = true;
        uint GameTimeStartedConversing = Game.GameTime;
        GameFiber.Sleep(500);
        GetAttention();
    }
    private void Tick()
    {
        while (CanContinueConversation)
        {
            DisplayPrompt();
            CheckInput();
            GameFiber.Yield();
        }
        GameFiber.Sleep(1000);
    }
}

