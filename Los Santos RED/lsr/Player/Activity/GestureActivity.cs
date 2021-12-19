using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player
{
    public class GestureActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private string AnimDictionary;
        private string Anim;
        private string GestureName;
        public GestureActivity(IActionable player, string gestureName) : base()
        {
            Player = player;
            GestureName = gestureName;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
        }
        public override void Pause()
        {

        }
        public override void Continue()
        {

        }
        public override void Start()
        {
            EntryPoint.WriteToConsole($"Gesture Start: {GestureName}", 5);
            GameFiber GestureWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                Enter();
            }, "GestureActivity");
        }
        private void Enter()
        {
            Player.SetUnarmed();
            Player.IsPerformingActivity = true;

            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimDictionary, Anim, 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            try
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character); //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            Player.IsPerformingActivity = false;
        }
        private void Idle()
        {
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, AnimDictionary, Anim);
                if (AnimationTime >= 1.0f)
                {
                    break;
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Setup()
        {
            if (Player.IsMale)
            {
                if (Player.IsInVehicle)
                {
                    AnimDictionary = "gestures@m@car@std@casual@ds";
                }
                else
                {
                    if (Player.IsSitting)
                    {
                        AnimDictionary = "gestures@m@sitting@generic@casual";
                    }
                    else
                    {
                        AnimDictionary = "gestures@m@standing@casual";
                    }
                }
            }
            else
            {
                if (Player.IsInVehicle)
                {
                    AnimDictionary = "gestures@m@car@std@casual@ds";
                }
                else
                {
                    if (Player.IsSitting)
                    {
                        AnimDictionary = "gestures@m@sitting@generic@casual";
                    }
                    else
                    {
                        AnimDictionary = "gestures@f@standing@casual";
                    }
                }
            }
            Anim = GestureName;
            AnimationDictionary.RequestAnimationDictionay(AnimDictionary);
        }
    }
}