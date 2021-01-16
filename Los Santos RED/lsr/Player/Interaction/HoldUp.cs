using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class HoldUp : Interaction
{
    private uint GameTimeStartedHoldingUp;
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
    public override string DebugString => $"HoldingUp {Target.Pedestrian.Handle} IsIntimidated {IsTargetIntimidated} TargetMugged {Target.HasBeenMugged}";
    public override string Prompt
    {
        get
        {
            if (IsTargetting && IsTargetIntimidated && !Target.HasBeenMugged)
            {
                return $"Press ~{Keys.E.GetInstructionalId()}~ to Demand Cash";
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
        while ((IsTargetting || Game.GameTime - GameTimeStoppedTargetting <= TimeToWait) && Target.DistanceToPlayer <= 10f && Target.Pedestrian.IsAlive && !Target.Pedestrian.IsRagdoll && !Target.Pedestrian.IsStunned && Player.IsAliveAndFree && !Player.Character.IsStunned && !Player.Character.IsRagdoll && NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {
            if (Player.CurrentTargetedPed?.Pedestrian.Handle == Target.Pedestrian.Handle)//!Game.LocalPlayer.IsFreeAiming && !NativeFunction.CallByName<bool>("IS_PLAYER_TARGETTING_ANYTHING", Game.LocalPlayer))
            {
                if (!IsTargetting)
                {
                    IsTargetting = true;
                    GameTimeStartedIntimidating = Game.GameTime;
                    GameTimeStoppedTargetting = 0;
                }
                if (Game.IsKeyDown(Keys.E) && IsTargetIntimidated && !Target.HasBeenMugged)//demand cash?
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
        if (Target != null && Target.Pedestrian.Exists())
        {
            Target.Pedestrian.BlockPermanentEvents = false;
            if(Target.WillFight)
            {
                Target.Pedestrian.Inventory.GiveNewWeapon("weapon_pistol", 60, true);
                Target.Pedestrian.Tasks.FightAgainst(Player.Character, -1);
            }
            else
            {
                Target.Pedestrian.Tasks.Flee(Player.Character, 100f, -1);
            }
            Target.CanBeTasked = true;
        }
        Player.IsHoldingUp = false;
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
        GameFiber.Sleep(500);
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1250);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Target.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        while (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1) && Game.GameTime - GameTimeStartedHoldingUp <= 7000)
        {
            GameFiber.Sleep(100);
            
        }       
        if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {
            CleanUp();
        }
        else
        {
            SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GENERIC_FRIGHTENED_HIGH", "GENERIC_FRIGHTENED_MED" }, true);
            CheckIntimidation();
        }
    }
    private void Setup()
    {
        Player.IsHoldingUp = true;
        Target.CanBeTasked = false;
        GameTimeStartedHoldingUp = Game.GameTime;
        Target.HasSpokenWithPlayer = true;
        Target.IsFedUpWithPlayer = true;
        Target.Pedestrian.BlockPermanentEvents = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        EnterHandsUp();
    }
}