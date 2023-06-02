//using ExtensionsMethods;
//using LosSantosRED.lsr.Interface;
//using LosSantosRED.lsr.Player;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//public class Loot_Old : DynamicActivity
//{
//    private uint GameTimeStartedConversing;
//    private bool IsActivelyConversing;
//    private bool IsTasked;
//    private bool IsBlockedEvents;
//    private PedExt Ped;
//    private IInteractionable Player;
//    private bool CancelledConversation;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private dynamic pedHeadshotHandle;
//    private IModItems ModItems;
//    private bool IsCancelled;
//    private WeaponInformation LastWeapon;

//    //private WeaponInformation previousWeapon;

//    public Loot_Old(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems)
//    {
//        Player = player;
//        Ped = ped;
//        Settings = settings;
//        Crimes = crimes;
//        ModItems = modItems;
//    }
//    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
//   public override ModItem ModItem { get; set; }
//    public override bool CanPause { get; set; } = false;
//    public override bool CanCancel { get; set; } = false;
//    public override bool IsUpperBodyOnly { get; set; } = false;
//    public override string PausePrompt { get; set; } = "Pause Activity";
//    public override string CancelPrompt { get; set; } = "Stop Activity";
//    public override string ContinuePrompt { get; set; } = "Continue Activity";
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsoleTestLong($"Looting Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");

//            Player.ActivityManager.IsLootingBody = true;
//           // NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
//            GameFiber.StartNew(delegate
//            {
//                try
//                {
//                    LootBody();
//                    Cancel();
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                    EntryPoint.ModController.CrashUnload();
//                }
//            }, "Conversation");
//        }
//    }
//    public override bool CanPerform(IActionable player)
//    {
//        if (!player.ActivityManager.CanLootLookedAtPed)
//        {
//            Game.DisplayHelp($"Cannot loot ped");
//            return false;
//        }
//        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
//        {
//            return true;
//        }
//        Game.DisplayHelp($"Cannot Loot");
//        return false;
//    }
//    private void LootBody()
//    {
//        //EntryPoint.WriteToConsoleTestLong("Looting Body");
//        if (MoveToBody())
//        {
//            bool hasCompletedTasks = DoLootAnimation();
//            if(hasCompletedTasks)
//            {
//                FinishLoot();
//                PlayAnimation("amb@medic@standing@tendtodead@exit", "exit");
//            }
//            if(LastWeapon != null)
//            {
//                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon.Hash, true);
//            }
//        }
//    }
//    private void FinishLoot()
//    {
//        bool hasAddedItem = false;
//        bool hasAddedCash = false;
//        string ItemsFound = "";
//        int CashAdded = 0;
//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.HasBeenLooted = true;
//            if (RandomItems.RandomPercent(Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomItems))
//            {
//                Ped.PedInventory.AddRandomItems(ModItems);
//            }
//            ItemsFound = Ped.LootInventory(Player);
//            hasAddedItem = ItemsFound != "";
//            if (Ped.Money > 0)//dead peds already drop it, truned off dropping for now
//            {
//                Player.BankAccounts.GiveMoney(Ped.Money);
//                CashAdded = Ped.Money;
//                Ped.Money = 0;
//                if (Ped.Pedestrian.Exists())
//                { 
//                    Ped.Pedestrian.Money = 0;
//                }
//                hasAddedCash = true;
//            }
//        }
//        string Description = "";
//        if (hasAddedCash)
//        {
//            Description += $"Cash Stolen: ~n~~g~${CashAdded}~s~";
//        }
//        if (hasAddedItem)
//        {
//            if(hasAddedCash)
//            {
//                Description += $"~n~Items Stolen:";
//                Description += ItemsFound;
//            }
//            else
//            {
//                Description += $"Items Stolen:";
//                Description += ItemsFound;
//            }
//        }
//        if(!hasAddedCash && !hasAddedItem)
//        {
//            Description = "Nothing Found";
//        }
//        if (NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
//        {
//            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(pedHeadshotHandle);
//            Game.DisplayNotification(str, str, "~r~Ped Searched", $"~y~{Ped.Name}", Description);
//        }
//        else
//        {
//            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Ped Searched", $"~y~{Ped.Name}", Description);
//        }
//    }
//    private bool MoveToBody()
//    {
//        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
//        Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
//        float DesiredHeading = Game.LocalPlayer.Character.Heading;
//        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, 1.0f, -1, DesiredHeading, 0.2f);
//        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Player.Character, Ped.Pedestrian, -1, 1.75f, 0.75f, 1073741824, 1); //Original and works ok
//        uint GameTimeStartedMovingToBody = Game.GameTime;
//        float heading = Game.LocalPlayer.Character.Heading;
//        bool IsFacingDirection = true;
//        bool IsCloseEnough = false;
//        while (Game.GameTime - GameTimeStartedMovingToBody <= 5000 && !IsCloseEnough && !IsCancelled)
//        {
//            if (Player.IsMoveControlPressed)
//            {
//                IsCancelled = true;
//            }
//            if(!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
//            {
//                IsCancelled = true;
//                break;
//            }
//            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(Ped.Pedestrian) <= 1.85f;


//           // Rage.Debug.DrawArrowDebug(DesiredPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);


//            GameFiber.Yield();
//        }


//        Vector3 PedRoot = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
//        //var Heading = (PedRoot - Player.Character.Position);

//        float calcHeading = (float)GetHeading(Player.Character.Position,PedRoot);
//        float calcHeading2 = (float)CalculeAngle(PedRoot,Player.Character.Position);

//        DesiredHeading = calcHeading2;

//        //EntryPoint.WriteToConsole($"calcHeading 1 {calcHeading} calcHeading2  {calcHeading2}");


//        //calcHeading = -(90 - calcHeading);
//        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);


//        //EntryPoint.WriteToConsole($"calcHeading 2 {calcHeading} calcHeading2 {calcHeading2}");

//        //NativeFunction.Natives.TASK_ACHIEVE_HEADING(Player.Character, calcHeading2, -1);//1200





//        GameFiber.Sleep(1000);
//        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
//        {
//           // EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
//            return true;
//        }
//        else
//        {
//            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//            //EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
//            return false;
//        }
//    }
//    private double GetHeading(Vector3 a, Vector3 b)
//    {
//        double x = b.X - a.X;
//        double y = b.Y - a.Y;
//        return 270 - Math.Atan2(y, x) * (180 / Math.PI);
//    }


//    private double CalculeAngle(Vector3 start, Vector3 arrival)
//    {
//        var deltaX = Math.Pow((arrival.X - start.X), 2);
//        var deltaY = Math.Pow((arrival.Y - start.Y), 2);

//        var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
//        var angle = (radian * (180 / Math.PI) + 360) % 360;

//        return angle;
//    }


//    private bool DoLootAnimation()
//    {
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
//        if (Player.WeaponEquipment.CurrentWeapon != null)
//        {
//            LastWeapon = Player.WeaponEquipment.CurrentWeapon;
//        }
//        else
//        {
//            LastWeapon = null;
//        }

//        Player.WeaponEquipment.SetUnarmed();
//        List<string> IdleToPlay = new List<string>() { "idle_a" , "idle_b" , "idle_c" };
//        if(PlayAnimation("amb@medic@standing@tendtodead@enter", "enter") && PlayAnimation("amb@medic@standing@tendtodead@idle_a", IdleToPlay.PickRandom()))
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    private bool PlayAnimation(string dictionary, string animation)
//    {
//        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
//        uint GameTimeStartedLootAnimation = Game.GameTime;
//        float AnimationTime = 0.0f;
//        while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedLootAnimation <= 10000)
//        {
//            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
//            if (Player.IsMoveControlPressed)
//            {
//                IsCancelled = true;
//            }
//            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
//            {
//                IsCancelled = true;
//                break;
//            }
//            GameFiber.Yield();
//        }
//        if(!IsCancelled && AnimationTime >= 1.0f)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    public override void Continue()
//    {

//    }
//    public override void Cancel()
//    {

//       // Player.SetPlayerToLastWeapon();

//        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//        Player.ActivityManager.IsLootingBody = false;
//        Player.ActivityManager.IsPerformingActivity = false;
//        //NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
//    }
//    public override void Pause()
//    {
//        Cancel();
//    }
//    public override bool IsPaused() => false;
//    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
//    {
//        bool Spoke = false;
//        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
//        {
//            if (isPlayer)
//            {
//                if (Player.CharacterModelIsFreeMode)
//                {
//                    ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
//                }
//                else
//                {
//                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
//                }
//            }
//            else
//            {
//                if (Ped.VoiceName != "")
//                {
//                    ToSpeak.PlayAmbientSpeech(Ped.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
//                }
//                else
//                {
//                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
//                }
//            }

//            GameFiber.Sleep(300);//100
//            if (ToSpeak.IsAnySpeechPlaying)
//            {
//                Spoke = true;
//            }
//            //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
//            if (Spoke)
//            {
//                break;
//            }
//        }
//        GameFiber.Sleep(100);
//        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete)
//        {
//            Spoke = true;
//            GameFiber.Yield();
//        }
//        if (!Spoke)
//        {
//            Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
//        }  
//        return Spoke;
//    }
//}