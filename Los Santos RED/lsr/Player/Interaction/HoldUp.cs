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
    private uint GameTimeStartedIntimidating;
    private uint GameTimeStoppedTargetting;
    private bool IsTargetting;
    private IInteractionable Player;
    private PedExt Target;
    private bool ForcedCower;
    private bool IsActivelyOrdering;
    private ISettingsProvideable Settings;
    public HoldUp(IInteractionable player, PedExt target, ISettingsProvideable settings)
    {
        Player = player;
        Target = target;
        Settings = settings;
    }
    public override string DebugString => $"HoldingUp {Target.Pedestrian.Handle} IsIntimidated {IsTargetIntimidated} TargetMugged {Target.HasBeenMugged}";
    private bool IsTargetIntimidated => GameTimeStartedIntimidating != 0 && Game.GameTime - GameTimeStartedIntimidating >= 1500;
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

        Target.HatesPlayer = true;
        if (Target.GetType() == typeof(GangMember))
        {
            GangMember gm = (GangMember)Target;
            Player.GangRelationships.ChangeReputation(gm.Gang, -500, true);
        }


        //Target.TimesInsultedByPlayer += 5;
        GameTimeStartedIntimidating = Game.GameTime;
        GameTimeStoppedTargetting = 0;
        int TimeToWait = RandomItems.MyRand.Next(1500, 2500);
        IsTargetting = true;
        while ((IsTargetting || Game.GameTime - GameTimeStoppedTargetting <= TimeToWait) && !ForcedCower && Target.DistanceToPlayer <= 10f && Target.Pedestrian.IsAlive && !Target.Pedestrian.IsRagdoll && !Target.Pedestrian.IsStunned && Player.IsAliveAndFree && !Player.Character.IsStunned && !Player.Character.IsRagdoll && NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {
            if (Player.CurrentTargetedPed?.Pedestrian.Handle == Target.Pedestrian.Handle)
            {
                if (!IsTargetting)
                {
                    IsTargetting = true;
                    GameTimeStartedIntimidating = Game.GameTime;
                    GameTimeStoppedTargetting = 0;
                }
                if (IsTargetting && IsTargetIntimidated)
                {
                    if (!Target.HasBeenMugged && !Player.ButtonPrompts.Any(x => x.Identifier == "DemandCash"))
                    {
                        Player.ButtonPrompts.Add(new ButtonPrompt("Demand Cash", "HoldUp", "DemandCash", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                    }
                    if (!Player.ButtonPrompts.Any(x => x.Identifier == "ForceDown"))
                    {
                        Player.ButtonPrompts.Add(new ButtonPrompt("Force Down", "HoldUp", "ForceDown", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2));
                    }
                }
                else
                {
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "HoldUp");
                }
                if (Player.ButtonPrompts.Any(x => x.Identifier == "DemandCash" && x.IsPressedNow) && IsTargetIntimidated && !Target.HasBeenMugged)//demand cash?
                {
                    Target.HasBeenMugged = true;
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "HoldUp");
                    CreateMoneyDrop();
                }
                if (Player.ButtonPrompts.Any(x => x.Identifier == "ForceDown" && x.IsPressedNow) && IsTargetIntimidated && !ForcedCower)//demand cash?
                {
                    ForcedCower = true;
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "HoldUp");
                    ForceCower();
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
    private void CleanUp()
    {
        Player.ButtonPrompts.RemoveAll(x => x.Group == "HoldUp");
        if (Target != null && Target.Pedestrian.Exists())
        {         
            //if (Target.WillFight)
            //{
            //    Target.Pedestrian.Inventory.GiveNewWeapon("weapon_pistol", 60, true);
            //    Target.Pedestrian.Tasks.FightAgainst(Player.Character, -1);
            //}
            //else if (!ForcedCower)
            //{
            //    Target.Pedestrian.Tasks.Flee(Player.Character, 100f, -1);
            //}
            //else
            //{
                Target.Pedestrian.BlockPermanentEvents = false;
                Target.CanBeTasked = true;
           // }
        }
        Player.IsHoldingUp = false;
    }
    private void CreateMoneyDrop()
    {
        IsActivelyOrdering = true;
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY" }, true);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        GameFiber.Sleep(2000);
        NativeFunction.CallByName<bool>("SET_PED_MONEY", Target.Pedestrian, 0);
        Vector3 MoneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);
        NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, RandomItems.MyRand.Next(15, 100), 1, false, true);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        IsActivelyOrdering = false;
    }
    private void ForceCower()
    {
        IsActivelyOrdering = true;
        SayAvailableAmbient(Player.Character, new List<string>() { "GUN_DRAW", "CHALLENGE_THREATEN" }, true);
        //Target.Pedestrian.Tasks.Cower(-1);
        NativeFunction.Natives.TASK_COWER(Target.Pedestrian, -1);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        GameFiber.Sleep(2000);
        IsActivelyOrdering = false;
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
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
        {
            if (ToSpeak.Handle == Player.Character.Handle && Player.CharacterModelIsFreeMode)
            {
                ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
            }
            else
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
            }
            //ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
            GameFiber.Sleep(100);
            if (ToSpeak.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
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
    private void Setup()
    {
        Player.IsHoldingUp = true;
        Target.CanBeTasked = false;
        GameTimeStartedHoldingUp = Game.GameTime;
        Target.HasSpokenWithPlayer = true;
        Target.Pedestrian.BlockPermanentEvents = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        EnterHandsUp();
    }
}