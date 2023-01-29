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
    public class ItemDroppingActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private uint GameTimeStartedGesturing;
        private string DropItemDictionary;
        private string DropItemAnimation;
       

        public ItemDroppingActivity(IActionable player, ISettingsProvideable settings, ModItem modItem) : base()
        {
            Player = player;
            Settings = settings;
            ModItem = modItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Item Drop";
        public override string CancelPrompt { get; set; } = "Stop Item Drop";
        public override string ContinuePrompt { get; set; } = "Continue Item Drop";

        public bool CompletedAnimation { get; private set; }
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
            EntryPoint.WriteToConsole($"Item Drop Start", 5);
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
            }, "ItemDropActivity");
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitesBase)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Drop Item");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            PerformDrop();
        }
        private void PerformDrop()
        {
            EntryPoint.WriteToConsole($"Drop Item Perform", 5);
            GameTimeStartedGesturing = Game.GameTime;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, DropItemDictionary, DropItemAnimation, 8.0f, -8.0f, -1, 56, 0, false, false, false);//-1
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && Game.GameTime - GameTimeStartedGesturing <= 5000)
            {
                Player.WeaponEquipment.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DropItemDictionary, DropItemAnimation);
                if (AnimationTime >= 1.0f)
                {
                    CompletedAnimation = true;
                    break;
                }
                GameFiber.Yield();
            }     
            Exit();
        }
        private void Exit()
        {
            if(CompletedAnimation)
            {
                Player.Inventory.Remove(ModItem);
            }
            Player.ActivityManager.IsPerformingActivity = false;
        }
        private void Setup()
        {
            DropItemDictionary = "pickup_object";
            DropItemAnimation = "pickup_low";
            AnimationDictionary.RequestAnimationDictionay(DropItemDictionary);
        }
    }
}
