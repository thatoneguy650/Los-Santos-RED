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
    public class DanceActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private uint GameTimeStartedDancing;
        private DanceData DanceData;
        //private int AnimationFlag = 50;
        private float AnimationBlendOutTime = -4.0f;
        private IRadioStations RadioStations;
        private RadioStation RadioStation;
        private ISettingsProvideable Settings;
        private MenuPool MenuPool;
        private UIMenu DanceMenu;
        private IDances Dances;
        private UIMenuListScrollerItem<DanceData> DanceScrollerMenu;
        private string PlayingAnimation;
        private string PlayingDictionary;
        private int AnimationFlagRepeat => 1;
        private int AnimationFlagNormal => 0;
        private bool DisplayedDanceName;
        public DanceActivity(IActionable player, DanceData danceData, IRadioStations radioStations, ISettingsProvideable settings, IDances dances) : base()
        {
            Player = player;
            DanceData = danceData;
            RadioStations = radioStations;
            Settings = settings;
            Dances = dances;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Dancing";
        public override string CancelPrompt { get; set; } = "Stop Dancing";
        public override string ContinuePrompt { get; set; } = "Continue Dancing";

        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsDancing = false;
        }
        public override void Pause()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsDancing = false;
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
            }, "DanceActivity");
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && !player.ActivityManager.IsResting && player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: Dance");
            return false;
        }






        private void Enter()
        {
            DisplayedDanceName = false;
            if (DanceData.AnimationEnter != "")
            {
                AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);
                //EntryPoint.WriteToConsole($"Dance Enter: {DanceData.AnimationEnter}");
                GameTimeStartedDancing = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationEnter, 4.0f, AnimationBlendOutTime, -1, AnimationFlagNormal, 0, false, false, false);//-1

                if (!DisplayedDanceName)
                {
                    Game.DisplayNotification($"Dance Name: ~r~{DanceData.Name}~s~");
                    DisplayedDanceName = true;
                }


                //if (DanceData.FacialAnimationEnter != "")
                //{
                //    NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, DanceData.FacialAnimationEnter, DanceData.AnimationDictionary);
                //}
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationEnter);
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
            if (DanceData.AnimationIdle != "" && !IsCancelled)
            {
                Player.ButtonPrompts.AddPrompt("DanceActivity","Pick Dance","PickDance", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
                Player.ButtonPrompts.AddPrompt("DanceActivity", "Random Dance", "RandomDance", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
                //Player.ButtonPrompts.AddPrompt("DanceActivity", "Stop Dancing", "StopDance", Settings.SettingsManager.KeySettings.InteractCancel, 3);
                PlayingAnimation = DanceData.AnimationIdle;
                PlayingDictionary = DanceData.AnimationDictionary;
               // EntryPoint.WriteToConsole($"Dance Idle: {DanceData.AnimationIdle}");
                GameTimeStartedDancing = Game.GameTime;

                if (!Player.IsMoveControlPressed)
                {
                    AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);
                    if (!DisplayedDanceName)
                    {
                        Game.DisplayNotification($"Dance Name: ~r~{DanceData.Name}~s~");
                        DisplayedDanceName = true;
                    }
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationIdle, 4.0f, AnimationBlendOutTime, -1, AnimationFlagRepeat, 0, false, false, false);//-1

                    if(DanceData.FacialAnimationIdle != "")
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, DanceData.FacialAnimationIdle, DanceData.AnimationDictionary);
                    }
                }
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationIdle);
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
                    if(PlayingAnimation != DanceData.AnimationIdle || PlayingDictionary != DanceData.AnimationDictionary)//changed the dance, restart it baby!
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
                    if(Player.ButtonPrompts.IsPressed("PickDance"))
                    {
                        DanceMenu.Visible = true;
                    }
                    if (Player.ButtonPrompts.IsPressed("RandomDance"))
                    {
                        SetRandomDanceData();
                    }
                    //if (Player.ButtonPrompts.IsPressed("StopDance"))
                    //{
                    //    Player.ButtonPrompts.RemovePrompts("DanceActivity");
                    //    shouldStop = true;
                    //    shouldExit = true;
                    //}
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }

                //EntryPoint.WriteToConsoleTestLong($" CanPerformActivitiesExtended{Player.ActivityManager.CanPerformActivitiesExtended} IsCancelled {IsCancelled} IsMoveControlPressed {Player.IsMoveControlPressed}");

                Player.ButtonPrompts.RemovePrompts("DanceActivity");
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
            if (DanceData.AnimationExit != "" && !IsCancelled)
            {
                //EntryPoint.WriteToConsole($"Dance Exit: {DanceData.AnimationExit}");
                GameTimeStartedDancing = Game.GameTime;
                AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit, 4.0f, AnimationBlendOutTime, -1, AnimationFlagNormal, 0, false, false, false);//-1
                if (DanceData.FacialAnimationExit != "")
                {
                    NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, DanceData.FacialAnimationExit, DanceData.AnimationDictionary);
                }
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit);
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
            //EntryPoint.WriteToConsole($"Gesture Setup AnimationDictionary: {DanceData.AnimationDictionary} AnimationEnter: {DanceData.AnimationEnter} AnimationName: {DanceData.AnimationIdle} AnimationExit: {DanceData.AnimationExit}");
            AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);

            MenuPool = new MenuPool();
            DanceMenu = new UIMenu("Dances", "Select a Dance");
            DanceMenu.RemoveBanner();
            MenuPool.Add(DanceMenu);
            DanceMenu.OnItemSelect += OnDanceMenuSelect;
            DanceScrollerMenu = new UIMenuListScrollerItem<DanceData>("Dances","Select a new dance",Dances.DanceLookups);
            DanceMenu.AddItem(DanceScrollerMenu);

            //below was in Enter()
            RadioStation = RadioStations.GetDanceStation();

            if (RadioStation != null)
            {
                NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(true);
                NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(true);
                NativeFunction.Natives.SET_RADIO_TO_STATION_NAME(RadioStation.InternalName);
                NativeFunction.Natives.SET_RADIO_STATION_MUSIC_ONLY(RadioStation.InternalName, true);
            }

            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            Player.ActivityManager.IsDancing = true;

            if (DanceData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = true;
            }
            
        }
        private void UnSetup()
        {
            if (DanceData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = false;
            }
            //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsDancing = false;


            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_FACIAL_IDLE_ANIM_OVERRIDE(Player.Character);

            if (RadioStation != null)
            {
                NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(false);
                NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(false);
                NativeFunction.Natives.SET_RADIO_STATION_MUSIC_ONLY(RadioStation.InternalName, false);
            }
            DisplayedDanceName = false;
        }
        private void OnDanceMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if(selectedItem == DanceScrollerMenu)
            {
                DanceData newDanceData = DanceScrollerMenu.SelectedItem;
                if(newDanceData != null)
                {
                    DanceData = newDanceData;
                }
                DanceMenu.Visible = false;
            }
        }
        private void SetRandomDanceData()
        {
            DanceData newDanceData = Dances.GetRandomDance();
            if (newDanceData != null)
            {
                DanceData = newDanceData;
                GameFiber.Sleep(200);
            }
        }
    }
}