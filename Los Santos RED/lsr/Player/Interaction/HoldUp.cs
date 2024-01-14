using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
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
    private bool Fleed;
    private IModItems ModItems;
    private ICellphones Cellphones;
    private bool isForcedCower = false;
    public HoldUp(IInteractionable player, PedExt target, ISettingsProvideable settings, IModItems modItems, ICellphones cellphones)
    {
        Player = player;
        Target = target;
        Settings = settings;
        ModItems = modItems;
        Cellphones = cellphones;
    }
    public override string DebugString => $"HoldingUp {Target.Pedestrian.Handle} IsIntimidated {IsTargetIntimidated} TargetMugged {Target.HasBeenMugged}";
    private bool IsTargetIntimidated => GameTimeStartedIntimidating != 0 && Game.GameTime - GameTimeStartedIntimidating >= 1000;
    public override bool CanPerformActivities { get; set; } = false;
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
                if (Target.Pedestrian.Exists() && (!Target.IsInVehicle || Target.Pedestrian.Speed <= 5f))
                {
                    Setup();
                }
            });
        }
    }
    private void Setup()
    {
        //EntryPoint.WriteToConsoleTestLong($"Hold Up Started Target.IsInVehicle {Target.IsInVehicle}");
        Target.CanBeTasked = false;     
        Target.PlayerKnownsName = true;
        Target.Pedestrian.BlockPermanentEvents = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        EnterHandsUp();
    }
    private void EnterHandsUp()
    {
        SayAvailableAmbient(Player.Character, new List<string>() { "GUN_DRAW", "CHALLENGE_THREATEN", "CHALLENGE_ACCEPTED_GENERIC" }, false);
        Player.ActivityManager.IsHoldingUp = true;
        GameTimeStartedHoldingUp = Game.GameTime;
        if (Target.IsInVehicle && Target.Pedestrian.CurrentVehicle.Exists())
        {
            Player.IsCarJacking = true;
            //EntryPoint.WriteToConsoleTestLong($"Hold Up EnterHandsUp Target.IsInVehicle {Target.IsInVehicle}");
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", 0, Target.Pedestrian.CurrentVehicle, 256);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1250);
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Target.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            while (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1) && Game.GameTime - GameTimeStartedHoldingUp <= 5000)
            {
                GameFiber.Sleep(100);
            }
            Player.IsCarJacking = false;
        }
        else
        {
            //EntryPoint.WriteToConsoleTestLong($"Hold Up EnterHandsUp Target.IsInVehicle {Target.IsInVehicle}");
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
            while (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1) && Game.GameTime - GameTimeStartedHoldingUp <= 5000)
            {
                GameFiber.Sleep(100);
            }
        }
        if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {
            CleanUp();
        }
        else
        {
            SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG", "GENERIC_FRIGHTENED_HIGH", "GENERIC_FRIGHTENED_MED", "GENERIC_SHOCKED_HIGH", "GENERIC_SHOCKED_MED" }, false);
            CheckIntimidation();
        }
    }
    private void CheckIntimidation()
    {
        Target.HatesPlayer = true;
        if (Target.GetType() == typeof(GangMember))
        {
            GangMember gm = (GangMember)Target;
            Player.RelationshipManager.GangRelationships.ChangeReputation(gm.Gang, -500, true);
        }
        //Target.TimesInsultedByPlayer += 5;
        GameTimeStartedIntimidating = Game.GameTime;
        GameTimeStoppedTargetting = 0;
        int TimeToWait = 500;// RandomItems.MyRand.Next(500, 1000);
        IsTargetting = true;
        while ((IsTargetting || Game.GameTime - GameTimeStoppedTargetting <= TimeToWait || Player.IsAiming) && !ForcedCower && Target.DistanceToPlayer <= 10f && Target.Pedestrian.IsAlive && !Target.Pedestrian.IsRagdoll && !Target.Pedestrian.IsStunned && Player.IsAliveAndFree && !Player.Character.IsStunned && !Player.Character.IsRagdoll && NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
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
                    if (!Target.HasBeenMugged && !Player.ButtonPrompts.HasPrompt("DemandCash")) //if (!Target.HasBeenMugged && !Player.ButtonPromptList.Any(x => x.Identifier == "DemandCash"))
                    {
                        Player.ButtonPrompts.AddPrompt("HoldUp", "Demand Cash/Items", "DemandCash", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);//Player.ButtonPromptList.Add(new ButtonPrompt("Demand Cash/Items", "HoldUp", "DemandCash", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                    }
                    if (!Player.ButtonPrompts.HasPrompt("ForceDown")) //if (!Player.ButtonPromptList.Any(x => x.Identifier == "ForceDown"))
                    {
                        Player.ButtonPrompts.AddPrompt("HoldUp", "Force Down", "ForceDown", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);//Player.ButtonPromptList.Add(new ButtonPrompt("Force Down", "HoldUp", "ForceDown", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2));
                    }
                    if (!Player.ButtonPrompts.HasPrompt("Flee")) //if (!Player.ButtonPromptList.Any(x => x.Identifier == "Flee"))
                    {
                        Player.ButtonPrompts.AddPrompt("HoldUp", "Force Flee", "Flee", Settings.SettingsManager.KeySettings.InteractCancel, 3);//Player.ButtonPromptList.Add(new ButtonPrompt("Force Flee", "HoldUp", "Flee", Settings.SettingsManager.KeySettings.InteractCancel, 3));
                    }
                }
                else
                {
                    Player.ButtonPrompts.RemovePrompts("HoldUp");//Player.ButtonPromptList.RemoveAll(x => x.Group == "HoldUp");
                }
                if (Player.ButtonPrompts.IsPressed("DemandCash") && IsTargetIntimidated && !Target.HasBeenMugged)//demand cash?//if (Player.ButtonPromptList.Any(x => x.Identifier == "DemandCash" && x.IsPressedNow) && IsTargetIntimidated && !Target.HasBeenMugged)//demand cash?
                {
                    Target.HasBeenMugged = true;
                    Target.WillCallPolice = false;
                    Target.WillCallPoliceIntense = false;
                    Player.ButtonPrompts.RemovePrompts("HoldUp");//Player.ButtonPromptList.RemoveAll(x => x.Group == "HoldUp");
                    CreateMoneyDrop();
                }
                if (Player.ButtonPrompts.IsPressed("ForceDown") && IsTargetIntimidated && !ForcedCower)//demand cash?//if (Player.ButtonPromptList.Any(x => x.Identifier == "ForceDown" && x.IsPressedNow) && IsTargetIntimidated && !ForcedCower)//demand cash?
                {
                    ForcedCower = true;
                    Target.WillCallPolice = false;
                    Target.WillCallPoliceIntense = false;
                    Player.ButtonPrompts.RemovePrompts("HoldUp");//Player.ButtonPromptList.RemoveAll(x => x.Group == "HoldUp");
                    ForceCower();
                }
                if (Player.ButtonPrompts.IsPressed("Flee") && IsTargetIntimidated && !Fleed)//demand cash?//if (Player.ButtonPromptList.Any(x => x.Identifier == "Flee" && x.IsPressedNow) && IsTargetIntimidated && !Fleed)//demand cash?
                {
                    Fleed = true;
                    Player.ButtonPrompts.RemovePrompts("HoldUp");//Player.ButtonPromptList.RemoveAll(x => x.Group == "HoldUp");
                    FuckOff();
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
        Player.ActivityManager.IsHoldingUp = false;
        Player.IsCarJacking = false;
        CleanUp();
    }
    private void CleanUp()
    {
        Player.ButtonPrompts.RemovePrompts("HoldUp");
        if (Target != null && Target.Pedestrian.Exists())
        {         
            Target.Pedestrian.BlockPermanentEvents = false;
            Target.CanBeTasked = true;
            if (!isForcedCower)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Target.Pedestrian);
            }
        }
        Player.ActivityManager.IsHoldingUp = false;
        Player.IsCarJacking = false;
    }
    private void CreateMoneyDrop()
    {

        IsActivelyOrdering = true;
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY" }, true);
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        GameFiber.Sleep(2000);
        if(!Target.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.CallByName<bool>("SET_PED_MONEY", Target.Pedestrian, 0);
        uint modelHash = Game.GetHashKey("PICKUP_MONEY_WALLET");
        Vector3 MoneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);

        if (Target.IsGangMember)
        {
            modelHash = Game.GetHashKey("PICKUP_MONEY_VARIABLE");
            int MoneyPickupCreated = 0;
            int PickupsToCreate = Math.DivRem(Target.Money, 500, out int Remainder);
            if (Remainder > 0)
            {
                PickupsToCreate++;
            }
            for (int i = 0;i< PickupsToCreate; i++)
            {
                int MoneyToDrop = Target.Money - MoneyPickupCreated;
                if(MoneyToDrop >= 500)
                {
                    MoneyToDrop = 500;
                }
                MoneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);
                NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", modelHash, MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, MoneyToDrop, 1, false, true);
                MoneyPickupCreated += MoneyToDrop;
            }
            //EntryPoint.WriteToConsoleTestLong($"Mugging Calculated PickupsToCreate (Gang) {PickupsToCreate} Target.Money {Target.Money} Remainder {Remainder}");
        }
        else
        {       
            if (Target.IsMerchant)
            {
                modelHash = Game.GetHashKey("PICKUP_MONEY_DEP_BAG");
            }
            NativeFunction.Natives.CREATE_AMBIENT_PICKUP(modelHash, MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, Target.Money, 1, false, true); //NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, Target.Money, 1, false, true);
        }
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);

        string ItemsFound;





        if(RandomItems.RandomPercent(Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomItems))
        {
            Target.PedInventory.AddRandomItems(ModItems, false, true);
        }
        ItemsFound = Target.LootInventory(Player, ModItems, Cellphones);








        string Description = "";
        if (ItemsFound != "")
        {  
            Description += $"Items Stolen:";
            Description += ItemsFound;
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Ped Mugged", $"~y~{Target.Name}", Description);
            EntryPoint.WriteToConsole($"MUGGING:{Target.Name} {Description}");
        }
        IsActivelyOrdering = false;
    }
    private void ForceCower()
    {
        IsActivelyOrdering = true;
        SayAvailableAmbient(Player.Character, new List<string>() { "GUN_DRAW", "CHALLENGE_THREATEN" }, true);
        //Target.Pedestrian.Tasks.Cower(-1);
        NativeFunction.Natives.TASK_COWER(Target.Pedestrian, -1);
        Target.WillCower = true;
        isForcedCower = true;
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        GameFiber.Sleep(2000);
        IsActivelyOrdering = false;
    }
    private void FuckOff()
    {
        IsActivelyOrdering = true;
        SayAvailableAmbient(Player.Character, new List<string>() { "GUN_DRAW", "CHALLENGE_THREATEN" }, true);
        NativeFunction.Natives.TASK_SMART_FLEE_PED(Target.Pedestrian, Player.Character, 500f, -1, false, false);
        SayAvailableAmbient(Target.Pedestrian, new List<string>() { "GUN_BEG" }, false);
        //Target.WillCower = false;
        GameFiber.Sleep(2000);
        IsActivelyOrdering = false;
        isForcedCower = false;
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
        {
            if (ToSpeak.Handle == Player.Character.Handle)
            {
                if (Player.CharacterModelIsFreeMode)
                {
                    ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
                }
                else
                {
                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                }
            }
            else
            {
                if (Target.VoiceName != "")
                {
                    ToSpeak.PlayAmbientSpeech(Target.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
                }
                else
                {
                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                }
            }
            GameFiber.Sleep(300);
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
        uint GameTimeStarted = Game.GameTime;
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete && Game.GameTime - GameTimeStarted <= 10000)
        {
            Spoke = true;
            GameFiber.Yield();
        }
        return Spoke;
    }

}