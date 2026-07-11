using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class CheerActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private uint GameTimeStartedCheering;
        private DanceData CheerData;
        private float AnimationBlendOutTime = -4.0f;
        private ISettingsProvideable Settings;
 
        private IDances Dances;
        private UIMenuListScrollerItem<DanceData> DanceScrollerMenu;
        private string PlayingAnimation;
        private string PlayingDictionary;
        private int AnimationFlagRepeat => 1;
        private int AnimationFlagNormal => 0;
        private bool DisplayedDanceName;
        public CheerActivity(IActionable player, ISettingsProvideable settings, IDances dances) : base()
        {
            Player = player;
            Settings = settings;
            Dances = dances;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Cheering";
        public override string CancelPrompt { get; set; } = "Stop Cheering";
        public override string ContinuePrompt { get; set; } = "Continue Cheering";

        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsCheering = false;
        }
        public override void Pause()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsCheering = false;
        }
        public override bool IsPaused() => false;
        public override void Continue()
        {

        }
        public override void Start()
        {
            //EntryPoint.WriteToConsole($"Dance Start: {DanceData.Name}");
            GameFiber GestureWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    Enter();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "CheerActivity");
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && !player.ActivityManager.IsResting && player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: Cheer");
            return false;
        }






        private void Enter()
        {
            DisplayedDanceName = false;
            if (CheerData.AnimationEnter != "")
            {
                AnimationDictionary.RequestAnimationDictionay(CheerData.AnimationDictionary);
                //EntryPoint.WriteToConsole($"Dance Enter: {DanceData.AnimationEnter}");
                GameTimeStartedCheering = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationEnter, 4.0f, AnimationBlendOutTime, -1, AnimationFlagNormal, 0, false, false, false);//-1

                //if (!DisplayedDanceName)
                //{
                //    Game.DisplayNotification($"Dance Name: ~r~{CheerData.Name}~s~");
                //    DisplayedDanceName = true;
                //}


                //if (DanceData.FacialAnimationEnter != "")
                //{
                //    NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, DanceData.FacialAnimationEnter, DanceData.AnimationDictionary);
                //}
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationEnter);
                    if (AnimationTime >= 0.99f)
                    {
                        break;
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                        break;
                    }
                    GameFiber.Yield();
                }
            }
            Idle();
        }
        private void Idle()
        {
            bool shouldExit = true;
            bool shouldStop = false;
            if (CheerData.AnimationIdle != "" && !IsCancelled)
            {
                //Player.ButtonPrompts.AddPrompt("CheerActivity", "Pick Dance", "PickDance", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
                Player.ButtonPrompts.AddPrompt("CheerActivity", "Random Cheer", "RandomCheer", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
                Player.ButtonPrompts.AddPrompt("CheerActivity", "Stop Cheering", "StopCheering", Settings.SettingsManager.KeySettings.InteractCancel, 3);
                PlayingAnimation = CheerData.AnimationIdle;
                PlayingDictionary = CheerData.AnimationDictionary;
                // EntryPoint.WriteToConsole($"Dance Idle: {DanceData.AnimationIdle}");
                GameTimeStartedCheering = Game.GameTime;

                if (!Player.IsMoveControlPressed)
                {
                    AnimationDictionary.RequestAnimationDictionay(CheerData.AnimationDictionary);
                    //if (!DisplayedDanceName)
                    //{
                    //    Game.DisplayNotification($"Dance Name: ~r~{CheerData.Name}~s~");
                    //    DisplayedDanceName = true;
                    //}
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationIdle, 4.0f, AnimationBlendOutTime, -1, AnimationFlagRepeat, 0, false, false, false);//-1

                    if (CheerData.FacialAnimationIdle != "")
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, CheerData.FacialAnimationIdle, CheerData.AnimationDictionary);
                    }
                }
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationIdle);
                    if (AnimationTime >= 0.99f && shouldStop)
                    {
                        //EntryPoint.WriteToConsoleTestLong("EXIT ANIM OVER AND SHOULDSTOP");
                        break;
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        //EntryPoint.WriteToConsoleTestLong("EXIT IsMoveControlPressed");
                        IsCancelled = true;
                        break;
                    }
                    if (PlayingAnimation != CheerData.AnimationIdle || PlayingDictionary != CheerData.AnimationDictionary)//changed the dance, restart it baby!
                    {
                        //EntryPoint.WriteToConsoleTestLong("EXIT NOT PLAYING ANIM");
                        shouldExit = false;
                        break;

                        //AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);
                        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationIdle, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                        //PlayingAnimation = DanceData.AnimationIdle;
                        //PlayingDictionary = DanceData.AnimationDictionary;
                        //EntryPoint.WriteToConsole("Dancing, Changed Dance!");
                        //Game.DisplayNotification($"Dance Name: ~r~{DanceData.Name}~s~");
                    }
                    if (Player.ButtonPrompts.IsPressed("RandomCheer"))
                    {
                        SetRandomCheer();
                    }
                    if (Player.ButtonPrompts.IsPressed("StopCheering"))
                    {
                        Player.ButtonPrompts.RemovePrompts("CheerActivity");
                        shouldStop = true;
                        shouldExit = true;
                        IsCancelled = true;
                    }
                    GameFiber.Yield();
                }
                //EntryPoint.WriteToConsoleTestLong($" CanPerformActivitiesExtended{Player.ActivityManager.CanPerformActivitiesExtended} IsCancelled {IsCancelled} IsMoveControlPressed {Player.IsMoveControlPressed}");
                Player.ButtonPrompts.RemovePrompts("CheerActivity");
            }
            if (shouldExit || IsCancelled)
            {
                Exit();
            }
            else
            {
                Enter();
            }
        }
        private void Exit()
        {
            if (CheerData.AnimationExit != "" && !IsCancelled)
            {
                GameTimeStartedCheering = Game.GameTime;
                AnimationDictionary.RequestAnimationDictionay(CheerData.AnimationDictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationExit, 4.0f, AnimationBlendOutTime, -1, AnimationFlagNormal, 0, false, false, false);//-1
                if (CheerData.FacialAnimationExit != "")
                {
                    NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, CheerData.FacialAnimationExit, CheerData.AnimationDictionary);
                }
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, CheerData.AnimationDictionary, CheerData.AnimationExit);
                    if (AnimationTime >= 0.99f)
                    {
                        break;
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                        break;
                    }
                    GameFiber.Yield();
                }
            }

            UnSetup();
        }
        private void Setup()
        {
            //AnimationFlag = 0;
            AnimationBlendOutTime = -4.0f;
            CheerData = Dances.GetRandomDance(); //new DanceData("Podium Dancers (F) 01", "anim@amb@nightclub@dancers@podium_dancers@", "hi_dance_facedj_17_v2_female^2");
            AnimationDictionary.RequestAnimationDictionay(CheerData.AnimationDictionary);          
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            Player.ActivityManager.IsCheering = true;
            if (CheerData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = true;
            }
        }
        private void UnSetup()
        {
            if (CheerData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = false;
            }

            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsCheering = false;

            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_FACIAL_IDLE_ANIM_OVERRIDE(Player.Character);

            DisplayedDanceName = false;
        }


        private void SetRandomCheer()
        {
            DanceData newDanceData = Dances.GetRandomDance();
            if (newDanceData != null)
            {
                CheerData = newDanceData;
                GameFiber.Sleep(200);
            }
        }
    }
}