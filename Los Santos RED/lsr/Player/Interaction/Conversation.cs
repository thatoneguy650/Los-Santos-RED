using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RAGENativeUI;
using LosSantosRED.lsr.Player;

public class Conversation : Interaction
{
    private uint GameTimeStartedConversing;
    private bool TargetCancelledConversation;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private PedExt Ped;
    private IInteractionable Player;
    public Conversation(IInteractionable player, PedExt ped)
    {
        Player = player;
        Ped = ped;
    }
    public override string Prompt => CanContinueConversation && !IsActivelyConversing ? $"Press ~{Keys.O.GetInstructionalId()}~ for Positive Response~n~Press ~{Keys.L.GetInstructionalId()}~ for Negative Response" : "";
    private bool CanContinueConversation => Player.IsConversing && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 7f && !Ped.Pedestrian.IsFleeing && Ped.Pedestrian.IsAlive;
    public override void Dispose()
    {
        Player.IsConversing = false;
        if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
        {
            Ped.Pedestrian.Tasks.Clear();
        }
        NativeFunction.CallByName<bool>("STOP_GAMEPLAY_HINT", true);
    }
    public override void Start()
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
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        Game.Console.Print($"CONVERSATION Can {ToSpeak.Handle} Say {Speech}? {CanSay}");
        return CanSay;
    }
    private void CheckInput()
    {
        if (Game.IsKeyDown(Keys.O))
        {
            Positive();
        }
        else if (Game.IsKeyDown(Keys.L))
        {
            Negative();
        }
    }
    private void Negative()
    {
        IsActivelyConversing = true;
        SayInsult(Player.Character, true);
        SayInsult(Ped.Pedestrian, true);
        Ped.HasBeenInsultedByPlayer = true;
        if (Ped.HasBeenInsultedByPlayer)
        {
            TargetCancelledConversation = true;
        }
        GameFiber.Sleep(1000);
        IsActivelyConversing = false;
    }
    private void Positive()
    {
        IsActivelyConversing = true;
        SaySmallTalk(Player.Character, false);
        SaySmallTalk(Ped.Pedestrian, true);
        GameFiber.Sleep(1000);
        IsActivelyConversing = false;
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x=> RandomItems.MyRand.Next()))
        {
            ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
            GameFiber.Sleep(100);
            if (ToSpeak.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            Game.Console.Print($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
            if (Spoke)
            {
                break;
            }
        }
        GameFiber.Sleep(100);
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete)
        {
            Spoke = true;
            GameFiber.Yield();
        }
        return Spoke;
    }
    private void SayInsult(Ped ToReply, bool IsSevere)
    {
        if (IsSevere)
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_HIGH"}, true);
        }
        else
        {
            SayAvailableAmbient(ToReply, new List<string>() { "GENERIC_INSULT_MED" }, true);
        }
        //GENERIC_INSULT_MED works on player
        //GENERIC_INSULT_MED works on peds?
    }
    private void SaySmallTalk(Ped ToReply, bool IsReply)
    {
        //Main Character?
        //CULT_TALK
        //PED_RANT_RESP

        SayAvailableAmbient(ToReply, new List<string>() { "PED_RANT_RESP", "CULT_TALK", "PED_RANT_01", "PHONE_CONV1_CHAT1" }, true);
        //CHAT_STATE does not work on most?
        //CHAT_RESP On Main and Character mostly say whatever?
        //GENERIC_WHATEVER main is basically and insult
    }
    private void Setup()
    {
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false);
        GameFiber.Sleep(500);
        if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
        {
            IsTasked = false;
        }
        else
        {
            IsTasked = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }

        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 2000);
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Ped.Pedestrian, -1, 0, 2);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        GameTimeStartedConversing = Game.GameTime;
        GameFiber.Sleep(500);
        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, true);
    }
    private void Tick()
    {
        while (CanContinueConversation)
        {
            CheckInput();
            if(TargetCancelledConversation)
            {
                Dispose();
                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_INSULT_MED", "GENERIC_WHATEVER" }, true);
                break;
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(1000);
    }
}

