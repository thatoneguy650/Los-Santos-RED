using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class HoldUp : Interaction
{
    private uint GameTimeStartedMugging;
    private IInteractionable Player;
    private PedExt Target;
    private uint GameTimeStartedIntimidating;
    private uint GameTimeStoppedTargetting;
    private bool IsTargetting;
    private bool IsTargetIntimidated => GameTimeStartedIntimidating != 0 && Game.GameTime - GameTimeStartedIntimidating >= 1500;
    public HoldUp(IInteractionable player, PedExt target)
    {
        Player = player;
        Target = target;
    }
    public override string Prompt
    {
        get
        {
            if (IsTargetting && IsTargetIntimidated && !Target.HasBeenMugged)
            {
                return $"Press ~{Keys.O.GetInstructionalId()}~ to Demand Cash";
            }
            else
            {
                return $"";
            }
        }
    }
    public override void Dispose()
    {
        CleanUp();
    }
    public override void Start()
    {
        if (Target.CanBeTasked)
        {
            GameFiber.StartNew(delegate
            {
                Setup();
            });
        }
    }
    private void CheckIntimidation()
    {
        GameTimeStartedIntimidating = Game.GameTime;
        GameTimeStoppedTargetting = 0;
        int TimeToWait = RandomItems.MyRand.Next(1500, 2500);
        IsTargetting = true;
        while ((IsTargetting || Game.GameTime - GameTimeStoppedTargetting <= TimeToWait) && Target.DistanceToPlayer <= 10f)
        {
            if (Player.CurrentTargetedPed?.Pedestrian.Handle == Target.Pedestrian.Handle)//!Game.LocalPlayer.IsFreeAiming && !NativeFunction.CallByName<bool>("IS_PLAYER_TARGETTING_ANYTHING", Game.LocalPlayer))
            {
                if (!IsTargetting)
                {
                    IsTargetting = true;
                    GameTimeStartedIntimidating = Game.GameTime;
                    GameTimeStoppedTargetting = 0;
                }
                if (Game.IsKeyDown(Keys.O) && IsTargetIntimidated && !Target.HasBeenMugged)//demand cash?
                {
                    Target.HasBeenMugged = true;
                    
                    CreateMoneyDrop();
                }
            }
            else
            {
                if (IsTargetting)
                {
                    IsTargetting = false;
                    GameTimeStartedIntimidating = 0;
                    GameTimeStoppedTargetting = Game.GameTime;
                }
            }
            GameFiber.Yield();
        }
        CleanUp();
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
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
    private void CleanUp()
    {
        Target.Pedestrian.BlockPermanentEvents = false;
        Target.Pedestrian.Tasks.Flee(Player.Character, 100f, -1);
        Target.CanBeTasked = true;
        Player.IsMugging = false;
    }
    private void CreateMoneyDrop()
    {
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY" }, true);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        GameFiber.Sleep(2000);
        //while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "mp_safehousevagos@", "package_dropoff", 3))
        //{
        //    GameFiber.Yield();
        //}
        NativeFunction.CallByName<bool>("SET_PED_MONEY", Target.Pedestrian, 0);
        Vector3 MoneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);
        NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, RandomItems.MyRand.Next(15, 100), 1, false, true);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        
    }
    private void EnterHandsUp()
    {

        SayAvailableAmbient(Player.Character, new List<string>() { "GUN_DRAW", "CHALLENGE_THREATEN" }, false);
        GameFiber.Sleep(750);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, true);
        while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1) && Game.GameTime - GameTimeStartedMugging <= 2500)
        {
            GameFiber.Sleep(100);
        }
        if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {
            CleanUp();
        }
        else
        {
            CheckIntimidation();
        }
    }
    private void Setup()
    {
        Player.IsMugging = true;
        Target.CanBeTasked = false;
        GameTimeStartedMugging = Game.GameTime;
        Target.HasSpokenWithPlayer = true;
        Target.IsFedUpWithPlayer = true;
        Target.Pedestrian.BlockPermanentEvents = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        EnterHandsUp();
    }
}