using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class ApplyEquipmentActivity : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private EatingData Data;
        private Rage.Object EquipmentObject;
        private IIntoxicants Intoxicants;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IActionable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private ISettingsProvideable Settings;
        private int TimesPlayedAnimation;
        private EquipmentItem EquipmentItem;
        private ConsumableRefresher ConsumableItemNeedGain;
        private AnimationWatcher AnimationWatcher;

        public ApplyEquipmentActivity(IActionable consumable, ISettingsProvideable settings, EquipmentItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
            EquipmentItem = modItem;
        }
        public override string DebugString => $"";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Applying";
        public override string CancelPrompt { get; set; } = "Stop Applying";
        public override string ContinuePrompt { get; set; } = "Continue Applying";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Continue()
        {
        }
        public override void Pause()
        {
            Cancel();
        }
        public override bool IsPaused() => false;
        public override void Start()
        {
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "ApplyingWatcher");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitiesMiddle)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            AttachEquipmentItemToHand();
            Player.ActivityManager.IsPerformingActivity = true;
            Idle();
        }
        private void Idle()
        {
            bool IsFinishedWithAnimation = false;
            StartNewIdleAnimation();
            ConsumableItemNeedGain = new ConsumableRefresher(Player, EquipmentItem, Settings);
            ConsumableItemNeedGain.SpeedMultiplier = 3.0f;
            while (Player.ActivityManager.CanPerformActivitiesMiddle && !IsCancelled)
            {
                Player.WeaponEquipment.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (TimesPlayedAnimation >= 1 && ConsumableItemNeedGain.IsFinished)
                {
                    IsCancelled = true;
                }
                else if (AnimationTime >= 0.7f)
                {
                    TimesPlayedAnimation++;
                    StartNewIdleAnimation();
                    IsFinishedWithAnimation = false;
                    //EntryPoint.WriteToConsole("PLAYED EQUIPMENT APPLY ANIMATION, STARTING ANOTHER");
                }
                bool isAnimRunning = AnimationWatcher.IsAnimationRunning(AnimationTime);
                if (!isAnimRunning && !IsFinishedWithAnimation)
                {
                    IsCancelled = true;
                }
                ConsumableItemNeedGain.Update();
                GameFiber.Yield();
            }
            Exit();
        }
        private void Exit()
        {
            if (EquipmentObject.Exists())
            {
                EquipmentObject.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            if (ModItem?.ModelItem?.CleanupItemImmediately == false)
            {
                GameFiber.Sleep(5000);
            }
            if (EquipmentObject.Exists())
            {
                EquipmentObject.Delete();
            }
        }
        private void AttachEquipmentItemToHand()
        {
            CreateEquipment();
            if (EquipmentObject.Exists() && !IsAttachedToHand)
            {
                EquipmentObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(EquipmentObject);
            }
        }
        private void CreateEquipment()
        {
            if (!EquipmentObject.Exists() && Data.PropModelName != "")
            {
                try
                {
                    EquipmentObject = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!EquipmentObject.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void StartNewIdleAnimation()
        {
            AnimationWatcher.Reset();
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -2.0f, -1, 50, 0.25f, false, false, false);
        }
        private void Setup()
        {
            List<string> AnimIdle;
            string AnimBase = "";
            string AnimBaseDictionary = "";
            string AnimEnter = "";
            string AnimEnterDictionary = "";
            string AnimExit = "";
            string AnimExitDictionary = "";
            string AnimIdleDictionary;
            string PropModel = "";
            string HandBoneName = "BONETAG_R_PH_HAND";
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;
            if (EquipmentItem != null && EquipmentItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                    //EntryPoint.WriteToConsoleTestLong($"Eating Activity Found Attachment {HandOffset} {HandRotator} {HandBoneName}");
                }
            }
            AnimIdleDictionary = "mp_safehouseshower@male@";
            AnimIdle = new List<string>() { "male_shower_undress_&_turn_on_water" };



            if (EquipmentItem != null && EquipmentItem.IsIntoxicating && EquipmentItem.Intoxicant != null)
            {
                CurrentIntoxicant = EquipmentItem.Intoxicant;
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, PropModel);
            AnimationWatcher = new AnimationWatcher();
        }
    }
}