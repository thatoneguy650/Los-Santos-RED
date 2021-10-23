using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
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
        public override string DebugString => $"IsPerformingActivity: {Player.IsPerformingActivity}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
        }
        public override void Continue()
        {

        }
        public override void Start()
        {
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
        }
        private void Enter()
        {
            Player.SetUnarmed();
            Player.IsPerformingActivity = true;

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
                Player.Character.Tasks.Clear();
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            Player.IsPerformingActivity = false;
        }
        private void Idle()
        {
            while (Player.CanPerformActivities && !IsCancelled && !Player.IsMoveControlPressed)
            {
                Player.SetUnarmed();
                GameFiber.Yield();
            }
            Exit();
        }
    }
}