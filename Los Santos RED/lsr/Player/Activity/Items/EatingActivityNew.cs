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
    public class EatingActivityNew : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private EatingData Data;
        private Rage.Object Food;
        private IIntoxicants Intoxicants;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IActionable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private ISettingsProvideable Settings;
        private int TimesAte;
        private FoodItem FoodItem;
        private ConsumableRefresher ConsumableItemNeedGain;
        private AnimationWatcher AnimationWatcher;
        private bool isPlayingBase;

        public EatingActivityNew(IActionable consumable, ISettingsProvideable settings, FoodItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
            FoodItem = modItem;
        }
        public override string DebugString => $"";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Eating";
        public override string CancelPrompt { get; set; } = "Stop Eating";
        public override string ContinuePrompt { get; set; } = "Continue Eating";
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
            Cancel();//for now it just cancels
        }
        public override bool IsPaused() => false;
        public override void Start()
        {
            //EntryPoint.WriteToConsole("EatingActivity START");
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
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
            AttachFoodToHand();
            Player.ActivityManager.IsPerformingActivity = true;
            Idle();
        }
        private void Idle()
        {
            uint GameTimeBetweenBites = RandomItems.GetRandomNumber(1500, 2500);
            uint GameTimeLastChangedIdle = Game.GameTime;
            bool IsFinishedWithBite = false;
            bool FinishMeal = false;
            StartNewIdleAnimation();
            TimesAte++;
            isPlayingBase = false;
            ConsumableItemNeedGain = new ConsumableRefresher(Player, FoodItem, Settings) { IsIntervalBased = Settings.SettingsManager.ActivitySettings.IntervalBasedConsumption };
            ConsumableItemNeedGain.Update();
            Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);

            uint GameTimeStartedBite = 0;
            uint comboBite = 0; // variable name is trash sorry
            uint BitesAllowed = Settings.SettingsManager.ActivitySettings.BitesAllowed;
            while (Player.ActivityManager.CanPerformActivitiesMiddle && !IsCancelled)
            {
                Player.WeaponEquipment.SetUnarmed();
                DisableControls();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f || IsFinishedWithBite)
                {
                    if (!IsFinishedWithBite)
                    {
                        IsFinishedWithBite = true;
                        StartBaseAnimation();
                    }
                    if (ConsumableItemNeedGain.IsFinished)
                    {
                        IsCancelled = true;
                    }
                    else if (comboBite > 0 && comboBite != BitesAllowed)
                    {
                        ConsumableItemNeedGain.Update();
                        TimesAte++;
                        comboBite++;
                        StartNewIdleAnimation();
                        Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);

                        GameTimeStartedBite = Game.GameTime;
                        IsFinishedWithBite = false;
                        Player.ButtonPrompts.RemovePrompts("EatingActivity");
                    }
                    else if (FinishMeal)
                    {
                        ConsumableItemNeedGain.Update();
                        TimesAte++;
                        StartNewIdleAnimation();
                        Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);

                        GameTimeStartedBite = Game.GameTime;
                        IsFinishedWithBite = false;
                        Player.ButtonPrompts.RemovePrompts("EatingActivity");
                    }
                    else if (IsFinishedWithBite && Player.ButtonPrompts.IsPressed("EatingFinish") && (Game.GameTime - GameTimeStartedBite >= 1500 || AnimationTime >= 1.0f))
                    {
                        FinishMeal = true;
                        ConsumableItemNeedGain.Update();

                        Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);


                        TimesAte++;
                        StartNewIdleAnimation();
                        IsFinishedWithBite = false;

                        GameTimeStartedBite = Game.GameTime;
                        Player.ButtonPrompts.RemovePrompts("EatingActivity");
                        //EntryPoint.WriteToConsole($"New Drinking Idle {PlayingAnim} TimesDrank {TimesDrank}");
                    }
                    else if (IsFinishedWithBite && Player.ButtonPrompts.IsPressed("EatingMultipleBites") && (Game.GameTime - GameTimeStartedBite >= 1500 || AnimationTime >= 1.0f))
                    {
                        comboBite = 0;
                        ConsumableItemNeedGain.Update();

                        Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);


                        TimesAte++;
                        comboBite++;
                        StartNewIdleAnimation();
                        IsFinishedWithBite = false;

                        GameTimeStartedBite = Game.GameTime;
                        Player.ButtonPrompts.RemovePrompts("EatingActivity");
                        //EntryPoint.WriteToConsole($"New Drinking Idle {PlayingAnim} TimesDrank {TimesDrank}");
                    }
                    else if (IsFinishedWithBite && Player.ButtonPrompts.IsPressed("EatingTakeBite") && (Game.GameTime - GameTimeStartedBite >= 1500 || AnimationTime >= 1.0f))
                    {
                        ConsumableItemNeedGain.Update();

                        Player.Intoxication.AddIntervalConsumption(CurrentIntoxicant);


                        TimesAte++;
                        StartNewIdleAnimation();
                        IsFinishedWithBite = false;

                        GameTimeStartedBite = Game.GameTime;
                        Player.ButtonPrompts.RemovePrompts("EatingActivity");
                        //EntryPoint.WriteToConsole($"New Drinking Idle {PlayingAnim} TimesDrank {TimesDrank}");
                    }
                }


                if (isPlayingBase && AnimationTime >= Settings.SettingsManager.ActivitySettings.EatAnimBaseEndingPercentage)// 0.5f)
                {
                    NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, PlayingDict, PlayingAnim, 0.0f);
                }
                if (IsFinishedWithBite)
                {
                    Player.ButtonPrompts.AddPrompt("EatingActivity", "Take Bite", "EatingTakeBite", GameControl.Attack, 10);
                    Player.ButtonPrompts.AddPrompt("EatingActivity", $"Take {BitesAllowed} Bites", "EatingMultipleBites", GameControl.Aim, 10);
                    Player.ButtonPrompts.AddPrompt("EatingActivity", "Finish Food", "EatingFinish", GameControl.Detonate, 10);
                }
                else
                {
                    Player.ButtonPrompts.RemovePrompts("EatingActivity");
                }
                bool isAnimRunning = AnimationWatcher.IsAnimationRunning(AnimationTime);
                if (!isAnimRunning && !IsFinishedWithBite)
                {
                    IsCancelled = true;
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void DisableControls()
        {
            Game.DisableControlAction(0, GameControl.Attack, true);// false);
            Game.DisableControlAction(0, GameControl.Attack2, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);// false);
            Game.DisableControlAction(0, GameControl.Detonate, true);// false);


            Game.DisableControlAction(0, GameControl.Aim, true);// false);
            Game.DisableControlAction(0, GameControl.VehicleAim, true);// false);
            Game.DisableControlAction(0, GameControl.AccurateAim, true);// false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);// false);
        }
        private void Exit()
        {
            Player.ButtonPrompts.RemovePrompts("EatingActivity");
            if (Food.Exists())
            {
                Food.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            if (ModItem?.ModelItem?.CleanupItemImmediately == false)
            {
                GameFiber.Sleep(5000);
            }
            if (Food.Exists())
            {
                Food.Delete();
            }
        }
        private void AttachFoodToHand()
        {
            CreateFood();
            if (Food.Exists() && !IsAttachedToHand)
            {
                Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Food);
            }
        }
        private void CreateFood()
        {
            if (!Food.Exists() && Data.PropModelName != "")
            {
                try
                {
                    Food = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Food.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void StartNewIdleAnimation()
        {
            isPlayingBase = false;
            AnimationWatcher.Reset();
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Settings.SettingsManager.ActivitySettings.BlendInIdleEat * 1.0f, Settings.SettingsManager.ActivitySettings.BlendOutIdleEat * -1.0f, -1, (int)(AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly | AnimationFlags.StayInEndFrame), 0, false, false, false);
        }
        private void StartBaseAnimation()
        {
            isPlayingBase = true;
            AnimationWatcher.Reset();
            PlayingDict = Data.AnimBaseDictionary;
            PlayingAnim = Data.AnimBase;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Settings.SettingsManager.ActivitySettings.BlendInBaseEat * 1.0f, Settings.SettingsManager.ActivitySettings.BlendOutBaseEat * -1.0f, -1, (int)(AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly | AnimationFlags.StayInEndFrame), 0, false, false, false);
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

            string HandBoneName = "BONETAG_L_PH_HAND";
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;

            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "LeftHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                    //EntryPoint.WriteToConsoleTestLong($"Eating Activity Found Attachment {HandOffset} {HandRotator} {HandBoneName}");
                }
            }
            AnimIdleDictionary = "mp_player_inteat@burger";
            AnimIdle = new List<string>() { "mp_player_int_eat_burger" };
            AnimBase = "mp_player_int_eat_burger_enter";
            AnimBaseDictionary = "mp_player_inteat@burger";
            AnimEnter = "mp_player_int_eat_burger_enter";
            AnimEnterDictionary = "mp_player_inteat@burger";

            if (FoodItem != null && FoodItem.IsIntoxicating && FoodItem.Intoxicant != null)
            {
                CurrentIntoxicant = FoodItem.Intoxicant;
                Player.Intoxication.StartIngestingIntervalBased(CurrentIntoxicant, FoodItem.IntoxicationPerInterval);
            }

            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, PropModel);
            AnimationWatcher = new AnimationWatcher();
        }
    }
}
