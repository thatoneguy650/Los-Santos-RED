using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LosSantosRED.lsr.Player
{
    public class GestureActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;      
        private uint GameTimeStartedGesturing;
        private GestureData GestureData;
        private int AnimationFlag = 50;
        private float AnimationBlendOutTime = -1.0f;

        public GestureActivity(IActionable player, GestureData gestureData) : base()
        {
            Player = player;
            GestureData = gestureData;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Gesture";
        public override string CancelPrompt { get; set; } = "Stop Gesture";
        public override string ContinuePrompt { get; set; } = "Continue Gesture";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
        }
        public override void Pause()
        {

        }
        public override bool IsPaused() => false;
        public override void Continue()
        {

        }
        public override void Start()
        {
            //EntryPoint.WriteToConsole($"Gesture Start: {GestureData.Name}");
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
            }, "GestureActivity");
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Gesture");
            return false;
        }






        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;       
            if(GestureData.IsInsulting)
            {
                Player.IsMakingInsultingGesture = true;
            }
            if (GestureData.AnimationEnter != "")
            {
                //EntryPoint.WriteToConsole($"Gesture Enter: {GestureData.AnimationEnter}");
                GameTimeStartedGesturing = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationEnter, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && Game.GameTime - GameTimeStartedGesturing <= 5000)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationEnter);
                    if (AnimationTime >= 1.0f)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
            } 
            Idle();
        }
        private void Idle()
        {
            if (GestureData.AnimationName != "")
            {
                EntryPoint.WriteToConsole($"Gesture Idle: {GestureData.AnimationName}", 5);
                GameTimeStartedGesturing = Game.GameTime;
                int idleAnimationFlag = GetIdleAnimationFlag();
                //EntryPoint.WriteToConsole($"idleAnimationFlag {idleAnimationFlag} IsWholeBody{GestureData.IsWholeBody} SetRepeat{GestureData.SetRepeat}");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationName, 4.0f, AnimationBlendOutTime, -1, idleAnimationFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && (GestureData.SetRepeat || Game.GameTime - GameTimeStartedGesturing <= 5000))
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationName);
                    if (AnimationTime >= 1.0f && !GestureData.SetRepeat)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
            }
            Exit();
        }
        private int GetIdleAnimationFlag()
        {
            int idleAnimationFlag = 0;
            if (!GestureData.IsWholeBody)
            {
                idleAnimationFlag = idleAnimationFlag | (int)AnimationFlags.UpperBodyOnly | (int)AnimationFlags.SecondaryTask;
            }

            if (GestureData.SetRepeat)
            {
                idleAnimationFlag = idleAnimationFlag | (int)AnimationFlags.Loop;
            }
            else
            {
                idleAnimationFlag = idleAnimationFlag | (int)AnimationFlags.StayInEndFrame;
            }
            return idleAnimationFlag;
        }
        private void Exit()
        {
            try
            {
                if (GestureData.AnimationExit != "")
                {
                    //EntryPoint.WriteToConsole($"Gesture Exit: {GestureData.AnimationExit}");
                    GameTimeStartedGesturing = Game.GameTime;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationExit, 4.0f, AnimationBlendOutTime, -1, AnimationFlag, 0, false, false, false);//-1
                    while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && Game.GameTime - GameTimeStartedGesturing <= 5000)
                    {
                        Player.WeaponEquipment.SetUnarmed();
                        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, GestureData.AnimationDictionary, GestureData.AnimationExit);
                        if (AnimationTime >= 1.0f)
                        {
                            break;
                        }
                        GameFiber.Yield();
                    }
                }
                if(GestureData.AnimationEnter != "" || GestureData.IsWholeBody)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                }
                else
                {
                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                }
                if (GestureData.IsInsulting)
                {
                    Player.IsMakingInsultingGesture = false;
                }
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            Player.ActivityManager.IsPerformingActivity = false;
        }
        private void Setup()
        {
            if (GestureData.AnimationDictionary == "")//auto detect
            {
                if (Player.IsMale)
                {
                    if (Player.IsInVehicle)
                    {
                        GestureData.AnimationDictionary = "gestures@m@car@std@casual@ds";
                    }
                    else
                    {
                        if (Player.ActivityManager.IsSitting)
                        {
                            GestureData.AnimationDictionary = "gestures@m@sitting@generic@casual";
                        }
                        else
                        {
                            GestureData.AnimationDictionary = "gestures@m@standing@casual";
                        }
                    }
                }
                else
                {
                    if (Player.IsInVehicle)
                    {
                        GestureData.AnimationDictionary = "gestures@m@car@std@casual@ds";
                    }
                    else
                    {
                        if (Player.ActivityManager.IsSitting)
                        {
                            GestureData.AnimationDictionary = "gestures@m@sitting@generic@casual";
                        }
                        else
                        {
                            GestureData.AnimationDictionary = "gestures@f@standing@casual";
                        }
                    }
                }
            }
            if(GestureData.AnimationEnter != "")
            {
                AnimationFlag = 2;
                AnimationBlendOutTime = -4.0f;
            }
            //EntryPoint.WriteToConsole($"Gesture Setup AnimationDictionary: {GestureData.AnimationDictionary} AnimationEnter: {GestureData.AnimationEnter} AnimationName: {GestureData.AnimationName} AnimationExit: {GestureData.AnimationExit}");
            AnimationDictionary.RequestAnimationDictionay(GestureData.AnimationDictionary);
        }
    }
}




/*
 * 
 * 
 * 
 * anim@mp_player_intselfieblow_kiss enter
anim@mp_player_intselfieblow_kiss exit
anim@mp_player_intselfieblow_kiss idle_a
anim@mp_player_intselfiedock enter
anim@mp_player_intselfiedock exit
anim@mp_player_intselfiedock idle_a
anim@mp_player_intselfiejazz_hands enter
anim@mp_player_intselfiejazz_hands exit
anim@mp_player_intselfiejazz_hands idle_a
anim@mp_player_intselfiethe_bird enter
anim@mp_player_intselfiethe_bird exit
anim@mp_player_intselfiethe_bird idle_a
anim@mp_player_intselfiethumbs_up enter
anim@mp_player_intselfiethumbs_up exit
anim@mp_player_intselfiethumbs_up idle_a
anim@mp_player_intselfiewank enter
anim@mp_player_intselfiewank exit
anim@mp_player_intselfiewank idle_a



mp_player_intfinger mp_player_int_finger
mp_player_introck mp_player_int_rock
mp_player_intsalute mp_player_int_salute
mp_player_intwank mp_player_int_wank

 * */