using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player
{
    public class ScenarioActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IIntoxicatable Player;
        private Mod.Player player;
        public ScenarioActivity(IIntoxicatable consumable) : base()
        {
            Player = consumable;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => $"IsPerformingActivity: {Player.ActivityManager.IsPerformingActivity}";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Activity";
        public override string CancelPrompt { get; set; } = "Stop Activity";
        public override string ContinuePrompt { get; set; } = "Continue Activity";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
        }
        public override void Pause()
        {
            Cancel();//for now it just cancels
        }
        public override bool IsPaused() => false;
        public override void Continue()
        {

        }
        public override void Start()
        {
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Enter();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "DrinkingWatcher");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended && !player.ActivityManager.IsResting)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Scenario");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;

            if (Player.ClosestScenario != null && Player.ClosestScenario.InternalName.ToUpper().Contains("SEAT"))
            {
                NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP(Player.Character, Player.Position.X, Player.Position.Y, Player.Position.Z, 2f, -1);
            }
            else
            {
                NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Player.Character, Player.Position.X, Player.Position.Y, Player.Position.Z, 2f, -1);
            }
            Idle();
        }
        private void Exit()
        {
            try
            {


                //NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Player.Character, 0, 0, 0);
                // NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Player.Character, 0, 0, 0);
                // NativeFunction.Natives.TASK_AGITATED_ACTION(Player.Character, Player.Character);
                //Player.Character.Tasks.Clear();
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            Player.ActivityManager.IsPerformingActivity = false;
        }
        private void Idle()
        {
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && !Player.IsMoveControlPressed)
            {
                Player.WeaponEquipment.SetUnarmed();
                GameFiber.Yield();
            }
            Exit();
        }
    }
}