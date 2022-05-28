using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
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

        public DanceActivity(IActionable player, DanceData danceData, IRadioStations radioStations) : base()
        {
            Player = player;
            DanceData = danceData;
            RadioStations = radioStations;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
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
            RadioStation = RadioStations.RadioStationList.Where(x => x.Name != "OFF" && x.Name != "NONE").PickRandom();
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
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationEnter);
                    if (AnimationTime >= 1.0f)
                    {
                        break;
                    }
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
            if (DanceData.AnimationName != "")
            {
                EntryPoint.WriteToConsole($"Gesture Idle: {DanceData.AnimationName}", 5);
                GameTimeStartedGesturing = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationName, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                while (Player.CanPerformActivities && !IsCancelled)
                {
                    Player.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationName);
                    if (AnimationTime >= 1.0f)
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
            Exit();
        }
        private void Exit()
        {
            try
            {
                if (DanceData.AnimationExit != "")
                {
                    EntryPoint.WriteToConsole($"Gesture Exit: {DanceData.AnimationExit}", 5);
                    GameTimeStartedGesturing = Game.GameTime;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                    while (Player.CanPerformActivities && !IsCancelled)
                    {
                        Player.SetUnarmed();
                        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DanceData.AnimationDictionary, DanceData.AnimationExit);
                        if (AnimationTime >= 1.0f)
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
        }
    }
}