using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player
{
    public class UrinatingActivity : DynamicActivity
    {
        private string PlayingAnim;
        private string PlayingDict;
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;

        public UrinatingActivity(IActionable player, ISettingsProvideable settings) : base()
        {
            Player = player;
            Settings = settings;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = false;
        public override bool IsUpperBodyOnly { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Activity";
        public override string CancelPrompt { get; set; } = "Stop Activity";
        public override string ContinuePrompt { get; set; } = "Continue Activity";
        public override void Cancel()
        {
            IsCancelled = true;
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
            }, "Laying");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Urinate");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            PlayingDict = "missbigscore1switch_trevor_piss";
            PlayingAnim = "piss_loop";
            if(!Player.IsMale)
            {
                PlayingDict = "missfbi3ig_0";
                PlayingAnim = "shit_loop_trev";
            }
            AnimationDictionary.RequestAnimationDictionay(PlayingDict);
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayingDict, PlayingAnim, 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_LOOPING), 0, false, false, false);
            LoopedParticle particle = null;
            if (Player.IsMale)
            {
                particle = new LoopedParticle("core", "ent_amb_peeing", Player.Character, PedBoneId.Pelvis, new Vector3(0.0f, 0.27f, -0.21f), new Rotator(-90f, 0f, 0f), 1.5f);
            }
            else
            {
                particle = new LoopedParticle("core", "ent_amb_peeing", Player.Character, PedBoneId.Pelvis, new Vector3(0.0f, -0.07f, -0.54f), new Rotator(-180f, 0f, 0f), 1.5f);
            }
            Player.ActivityManager.IsUrinatingDefecting = true;
            uint GameTimeLastPlayedAnim = Game.GameTime;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
            {
                if (Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }
                if (!Player.IsAliveAndFree)
                {
                    IsCancelled = true;
                }
                if (Game.GameTime - GameTimeLastPlayedAnim >= 3000)
                {
                    Player.ActivityManager.PlaySpecificFacialAnimations(Player.IsMale ? new List<string>() { "mood_happy_1", }.PickRandom() : "effort_1");
                    GameTimeLastPlayedAnim = Game.GameTime;
                }
                GameFiber.Yield();
            }
            particle.Stop();
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsUrinatingDefecting = false;
        }
    }
}