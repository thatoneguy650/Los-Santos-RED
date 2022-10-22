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
    public class EatingActivity : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private EatingData Data;
        private Rage.Object Food;
        private IIntoxicants Intoxicants;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private ISettingsProvideable Settings;
        private uint GameTimeLastGivenHealth;
        private int HealthGiven;
        private int TimesAte;
        private uint GameTimeLastGivenNeeds;
        private float HungerGiven;
        private float ThirstGiven;
        private int SleepGiven;
        private bool GivenFullHealth;
        private bool GivenFullHunger;
        private bool GivenFullThirst;
        private bool GivenFullSleep;
        private float PrevAnimationTime;
        private uint GameTimeLastCheckedAnimation;

        public EatingActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
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
            EntryPoint.WriteToConsole("EatingActivity START", 5);
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
        }
        private void AttachFoodToHand()
        {
            CreateFood();
            if (Food.Exists() && !IsAttachedToHand)
            {
                Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Food;
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
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if(!Food.Exists())
                {
                    IsCancelled = true;
                }
                //try
                //{
                //    if (Food.Exists())
                //    {
                //        Food.IsGravityDisabled = false;
                //    }
                //    else
                //    {
                //        IsCancelled = true;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    EntryPoint.WriteToConsole($"Error Setting Model Gravity {ex.Message} {ex.StackTrace}");
                //}
            }
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
            StartNewIdleAnimation();
            EntryPoint.WriteToConsole($"Eating Activity Playing {PlayingDict} {PlayingAnim}", 5);
            while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
            {
                Player.WeaponEquipment.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f)
                {
                    bool isFinished = Settings.SettingsManager.NeedsSettings.ApplyNeeds ? GivenFullHunger && GivenFullSleep && GivenFullThirst : GivenFullHealth;
                    if (TimesAte >= 5 && isFinished) // || Player.Character.Health == Player.Character.MaxHealth))
                    {
                        if (Food.Exists())
                        {
                            Food.Delete();
                        }
                        IsCancelled = true;
                    }
                    else
                    {
                        TimesAte++;
                        StartNewIdleAnimation();
                    }
                }
                if (!IsAnimationRunning(AnimationTime))
                {
                    IsCancelled = true;
                }
                UpdateHealthGain();
                UpdateNeeds();
                GameFiber.Yield();
            }
            Exit();
        }


        private void Exit()
        {
            if (Food.Exists())
            {
                Food.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            if (ModItem?.CleanupItemImmediately == false)
            {
                GameFiber.Sleep(5000);
            }
            if (Food.Exists())
            {
                Food.Delete();
            }
        }

        private void StartNewIdleAnimation()
        {
            GameTimeLastCheckedAnimation = Game.GameTime;
            PrevAnimationTime = 0.0f;
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -2.0f, -1, 50, 0, false, false, false);
        }
        private bool IsAnimationRunning(float AnimationTime)
        {
            //return NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, PlayingDict, PlayingAnim, 3);



            //return true;
            if (Game.GameTime - GameTimeLastCheckedAnimation >= 500)
            {
                if (PrevAnimationTime == AnimationTime)
                {
                    //EntryPoint.WriteToConsole("Animation Issues Detected, Cancelling");
                    return false;
                }
                PrevAnimationTime = AnimationTime;
                GameTimeLastCheckedAnimation = Game.GameTime;
            }
            return true;
        }

        private void UpdateHealthGain()
        {
            if (Game.GameTime - GameTimeLastGivenHealth >= 1000)
            {
                if (ModItem.ChangesHealth && !Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                {
                    if (ModItem.HealthChangeAmount > 0 && HealthGiven < ModItem.HealthChangeAmount)
                    {
                        HealthGiven++;
                        Player.HealthManager.ChangeHealth(1);
                    }
                    else if (ModItem.HealthChangeAmount < 0 && HealthGiven > ModItem.HealthChangeAmount)
                    {
                        HealthGiven--;
                        Player.HealthManager.ChangeHealth(-1);  
                    }
                }

                if(HealthGiven == ModItem.HealthChangeAmount)
                {
                    GivenFullHealth = true;
                }

                GameTimeLastGivenHealth = Game.GameTime;
            }
        }
        private void UpdateNeeds()
        {
            if (Game.GameTime - GameTimeLastGivenNeeds >= 1000)
            {
                if(ModItem.ChangesNeeds)
                {
                    if(ModItem.ChangesHunger)
                    {
                        if(ModItem.HungerChangeAmount < 0.0f)
                        {
                            if(HungerGiven > ModItem.HungerChangeAmount)
                            {
                                Player.HumanState.Hunger.Change(-1.0f, true);
                                HungerGiven--;
                            }
                            else
                            {
                                GivenFullHunger = true;
                            }
                        }
                        else
                        {
                            if(HungerGiven < ModItem.HungerChangeAmount)
                            {
                                Player.HumanState.Hunger.Change(1.0f, true);
                                HungerGiven++;
                            }
                            else
                            {
                                GivenFullHunger = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullHunger = true;
                    }
                    if (ModItem.ChangesThirst)
                    {
                        if (ModItem.ThirstChangeAmount < 0.0f)
                        {
                            if (ThirstGiven > ModItem.ThirstChangeAmount)
                            {
                                Player.HumanState.Thirst.Change(-1.0f, true);
                                ThirstGiven--;
                            }
                            else
                            {
                                GivenFullThirst = true;
                            }
                        }
                        else
                        {
                            if (ThirstGiven < ModItem.ThirstChangeAmount)
                            {
                                Player.HumanState.Thirst.Change(1.0f, true);
                                ThirstGiven++;
                            }
                            else
                            {
                                GivenFullThirst = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullThirst = true;
                    }
                    if (ModItem.ChangesSleep)
                    {
                        if (ModItem.SleepChangeAmount < 0.0f)
                        {
                            if (SleepGiven > ModItem.SleepChangeAmount)
                            {
                                Player.HumanState.Sleep.Change(-1.0f, true);
                                SleepGiven--;
                            }
                            else
                            {
                                GivenFullSleep = true;
                            }
                        }
                        else
                        {
                            if (SleepGiven < ModItem.SleepChangeAmount)
                            {
                                Player.HumanState.Sleep.Change(1.0f, true);
                                SleepGiven++;
                            }
                            else
                            {
                                GivenFullSleep = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullSleep = true;
                    }
                }
                GameTimeLastGivenNeeds = Game.GameTime;
            }
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

            string HandBoneName =  "BONETAG_L_PH_HAND";
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
                    EntryPoint.WriteToConsole($"Eating Activity Found Attachment {HandOffset} {HandRotator} {HandBoneName}");
                }
            }
            //if (Settings.SettingsManager.PlayerOtherSettings.OverwriteHandOffset)
            //{
            //    HandOffset = new Vector3(Settings.SettingsManager.PlayerOtherSettings.HandOffsetX, Settings.SettingsManager.PlayerOtherSettings.HandOffsetY, Settings.SettingsManager.PlayerOtherSettings.HandOffsetZ);
            //    HandRotator = new Rotator(Settings.SettingsManager.PlayerOtherSettings.HandRotateX, Settings.SettingsManager.PlayerOtherSettings.HandRotateY, Settings.SettingsManager.PlayerOtherSettings.HandRotateZ);
            //}

            AnimIdleDictionary = "mp_player_inteat@burger";
            AnimIdle = new List<string>() { "mp_player_int_eat_burger" };
            AnimBase = "mp_player_int_eat_burger_enter";
            AnimBaseDictionary = "mp_player_inteat@burger";
            AnimEnter = "mp_player_int_eat_burger_enter";
            AnimEnterDictionary = "mp_player_inteat@burger";

            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, PropModel);
        }
    }
}