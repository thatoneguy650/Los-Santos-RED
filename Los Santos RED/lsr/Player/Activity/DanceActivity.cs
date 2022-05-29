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
        private uint GameTimeStartedGesturing;
        private DanceData DanceData;
        private int AnimationFlag = 50;
        private float AnimationBlendOutTime = -1.0f;
        private IRadioStations RadioStations;
        private RadioStation RadioStation;
        private ISettingsProvideable Settings;
        private MenuPool MenuPool;
        private UIMenu DanceMenu;
        private IDances Dances;
        private UIMenuListScrollerItem<DanceData> DanceScrollerMenu;

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
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.IsDancing = false;
        }
        public override void Pause()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.IsDancing = false;
        }
        public override void Continue()
        {

        }
        public override void Start()
        {
            EntryPoint.WriteToConsole($"Gesture Start: {DanceData.Name}", 5);
            GameFiber GestureWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                Enter();
            }, "DanceActivity");
        }
        private void Enter()
        {

            RadioStation = RadioStations.GetDanceStation();

            if (RadioStation != null)
            {
                NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(true);
                NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(true);
                NativeFunction.Natives.SET_RADIO_TO_STATION_NAME(RadioStation.InternalName);
                NativeFunction.Natives.SET_RADIO_STATION_MUSIC_ONLY(RadioStation.InternalName, true);
            }

            Player.SetUnarmed();
            Player.IsPerformingActivity = true;
            Player.IsDancing = true;
          
            if (DanceData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = true;
            }
            if (DanceData.AnimationEnter != "")
            {
                EntryPoint.WriteToConsole($"Gesture Enter: {DanceData.AnimationEnter}", 5);
                GameTimeStartedGesturing = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationEnter, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                while (Player.CanPerformActivities && !IsCancelled)
                {
                    Player.SetUnarmed();
                    //float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationEnter);
                    //if (AnimationTime >= 1.0f)
                    //{
                    //    break;
                    //}
                    if(Player.IsMoveControlPressed)
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
            if (DanceData.AnimationName != "" && !IsCancelled)
            {
                Player.ButtonPrompts.AddPrompt("DanceActivity","Pick Dance","PickDance", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
                Player.ButtonPrompts.AddPrompt("DanceActivity", "Random Dance", "RandomDance", Settings.SettingsManager.KeySettings.InteractCancel, 2);



                string PlayingAnimation = DanceData.AnimationName;
                string PlayingDictionary = DanceData.AnimationDictionary;
                EntryPoint.WriteToConsole($"Gesture Idle: {DanceData.AnimationName}", 5);
                GameTimeStartedGesturing = Game.GameTime;

                if (!Player.IsMoveControlPressed)
                {
                    Game.DisplayNotification($"Dance Name: ~r~{DanceData.Name}~s~");
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationName, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                }
                while (Player.CanPerformActivities && !IsCancelled)
                {
                    Player.SetUnarmed();
                    //float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationName);
                    //if (AnimationTime >= 1.0f)
                    //{
                    //    break;
                    //}
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                        break;
                    }


                    if(PlayingAnimation != DanceData.AnimationName || PlayingDictionary != DanceData.AnimationDictionary)//changed the dance, restart it baby!
                    {
                        AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationName, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                        PlayingAnimation = DanceData.AnimationName;
                        PlayingDictionary = DanceData.AnimationDictionary;
                        EntryPoint.WriteToConsole("Dancing, Changed Dance!");
                        Game.DisplayNotification($"Dance Name: ~r~{DanceData.Name}~s~");



                    }



                    if(Player.ButtonPrompts.IsPressed("PickDance"))
                    {
                        DanceMenu.Visible = true;
                    }
                    if (Player.ButtonPrompts.IsPressed("RandomDance"))
                    {
                        DanceData newDanceData = Dances.GetRandomDance();
                        if(newDanceData != null)
                        {
                            DanceData = newDanceData;
                            GameFiber.Sleep(200);
                        }
                    }
                    //button pop up to start menu, when selected, change the animation name



                    MenuPool.ProcessMenus();

                    GameFiber.Yield();
                }
                Player.ButtonPrompts.RemovePrompts("DanceActivity");
            }

            Exit();
        }
        private void Exit()
        {
            try
            {
                if (DanceData.AnimationExit != "" && !IsCancelled)
                {
                    EntryPoint.WriteToConsole($"Gesture Exit: {DanceData.AnimationExit}", 5);
                    GameTimeStartedGesturing = Game.GameTime;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                    while (Player.CanPerformActivities && !IsCancelled)
                    {
                        Player.SetUnarmed();
                        //float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit);
                        //if (AnimationTime >= 1.0f)
                        //{
                        //    break;
                        //}
                        if (Player.IsMoveControlPressed)
                        {
                            IsCancelled = true;
                            break;
                        }
                        GameFiber.Yield();
                    }
                }
                if (DanceData.IsInsulting)
                {
                    Player.IsMakingInsultingGesture = false;
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            Player.IsPerformingActivity = false;
            Player.IsDancing = false;


            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

            if (RadioStation != null)
            {
                NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(false);
                NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(false);
                NativeFunction.Natives.SET_RADIO_STATION_MUSIC_ONLY(RadioStation.InternalName, false);
            }
        }
        private void Setup()
        {
            AnimationFlag = 1;
            AnimationBlendOutTime = -4.0f;
            EntryPoint.WriteToConsole($"Gesture Setup AnimationDictionary: {DanceData.AnimationDictionary} AnimationEnter: {DanceData.AnimationEnter} AnimationName: {DanceData.AnimationName} AnimationExit: {DanceData.AnimationExit}", 5);
            AnimationDictionary.RequestAnimationDictionay(DanceData.AnimationDictionary);

            MenuPool = new MenuPool();
            DanceMenu = new UIMenu("Dances", "Select a Dance");
            DanceMenu.RemoveBanner();
            MenuPool.Add(DanceMenu);
            DanceMenu.OnItemSelect += OnDanceMenuSelect;
            DanceScrollerMenu = new UIMenuListScrollerItem<DanceData>("Dances","Select a new dance",Dances.DanceLookups);
            DanceMenu.AddItem(DanceScrollerMenu);



            //
           // DanceMenu.Visible = true;


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
    }
}