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
    public class InjectActivity : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private EatingData Data;
        private bool hasGainedHP = false;
        private IIntoxicants Intoxicants;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private Rage.Object Item;
        private IActionable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private ISettingsProvideable Settings;
        private InjectItem InjectItem;
        private ConsumableRefresher ConsumableItemNeedGain;

        public InjectActivity(IActionable consumable, ISettingsProvideable settings, InjectItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            InjectItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Shooting Up";
        public override string CancelPrompt { get; set; } = "Stop Shooting Up";
        public override string ContinuePrompt { get; set; } = "Continue Shooting Up";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsUsingIllegalItem = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Continue()
        {
        }
        public override void Pause()
        {
            Cancel();//for now it just cancels
        }
        public override bool IsPaused() => false;
        public override void Start()
        {
            Setup();
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
            if (player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }
        private void AttachItemToHand()
        {
            CreateItem();
            if (Item.Exists() && !IsAttachedToHand)
            {
                Item.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Item);
            }
        }
        private void CreateItem()
        {
            if (!Item.Exists() && Data.PropModelName != "")
            {
                try
                {
                    Item = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (Item.Exists())
                {
                    Item.IsGravityDisabled = false;
                }
                else
                {
                    IsCancelled = true;
                }
            }
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            AttachItemToHand();
            Player.ActivityManager.IsPerformingActivity = true;
            if (ModItem.IsPublicUseIllegal)
            {
                Player.ActivityManager.IsUsingIllegalItem = true;
            }
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            if (Item.Exists())
            {
                Item.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsUsingIllegalItem = false;
            if (CurrentIntoxicant != null && !CurrentIntoxicant.ContinuesWithoutCurrentUse)
            {
                //EntryPoint.WriteToConsole("IngestActivity Exit, Stopping ingestion");
                Player.Intoxication.StopIngesting(CurrentIntoxicant);
            }
            GameFiber.Sleep(5000);
            if (Item.Exists())
            {
                Item.Delete();
            }
        }
        private void Idle()
        {
            ConsumableItemNeedGain = new ConsumableRefresher(Player, InjectItem, Settings);
            bool hasStartedIntoxicating = false;
            uint GameTimeLastGaveHealth = Game.GameTime;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
            {
                Player.WeaponEquipment.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 0.35f && !hasStartedIntoxicating)
                {
                    Player.Intoxication.StartIngesting(CurrentIntoxicant);
                    hasStartedIntoxicating = true;
                }
                if (AnimationTime >= 0.7f)
                {
                    ConsumableItemNeedGain.FullyConsume();
                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);//NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    break;
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Setup()
        {
            List<string> AnimIdle;
            string AnimEnter = "";
            string AnimEnterDictionary = "";
            string AnimExit = "";
            string AnimExitDictionary = "";
            string AnimIdleDictionary;

            string PropModel = "";

            string HandBoneName = "BONETAG_R_PH_HAND";
            Vector3 HandOffset = new Vector3();
            Rotator HandRotator = new Rotator();
            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                }
            }
            AnimIdleDictionary = "rcmpaparazzo1ig_4";
            AnimIdle = new List<string>() { "miranda_shooting_up" };


            if (InjectItem != null && InjectItem.IsIntoxicating && InjectItem.Intoxicant != null)
            {
                CurrentIntoxicant = InjectItem.Intoxicant;
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }

            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData("", "", AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, PropModel);
        }
    }
}