using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

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
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsPerformingActivity} I: {Player.IntoxicatedIntensity}";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Eating";
        public override string CancelPrompt { get; set; } = "Stop Eating";
        public override string ContinuePrompt { get; set; } = "Continue Eating";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
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
                //Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, "BONETAG_L_PH_HAND"), Data.HandOffset, Data.HandRotator);
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
                    if (Food.Exists())
                    {
                        Food.IsGravityDisabled = false;
                    }
                    else
                    {
                        IsCancelled = true;
                    }
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
            }
        }
        private void Enter()
        {
            Player.SetUnarmed();
            AttachFoodToHand();
            Player.IsPerformingActivity = true;
            Idle();
        }

        private void Idle()
        {
            StartNewIdleAnimation();
            EntryPoint.WriteToConsole($"Eating Activity Playing {PlayingDict} {PlayingAnim}", 5);
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f)
                {
                    if (TimesAte >= 5 && GivenFullHealth && GivenFullHunger && GivenFullSleep && GivenFullThirst) // || Player.Character.Health == Player.Character.MaxHealth))
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
            Player.IsPerformingActivity = false;
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
            return true;
            if (Game.GameTime - GameTimeLastCheckedAnimation >= 500)
            {
                if (PrevAnimationTime == AnimationTime)
                {
                    EntryPoint.WriteToConsole("Animation Issues Detected, Cancelling");
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
                if (ModItem.ChangesHealth)
                {
                    if (ModItem.HealthChangeAmount > 0 && HealthGiven < ModItem.HealthChangeAmount)
                    {
                        HealthGiven++;
                        Player.ChangeHealth(1);
                    }
                    else if (ModItem.HealthChangeAmount < 0 && HealthGiven > ModItem.HealthChangeAmount)
                    {
                        HealthGiven--;
                        Player.ChangeHealth(-1);  
                    }
                    //Player.HumanState.Hunger.Change(3.0f, true);
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
            int HandBoneID = 57005;
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;
            string PropModel = "";

            //if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            //{
            //    AnimIdleDictionary = "amb@code_human_wander_eating_donut@male@idle_a";
            //    AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            //    AnimBase = "base";
            //    AnimBaseDictionary = "amb@code_human_wander_eating_donut@male@base";
            //    AnimEnter = "static";
            //    AnimEnterDictionary = "amb@code_human_wander_eating_donut@male@base";

            //    if (Player.IsSitting || Player.IsInVehicle)
            //    {
            //        AnimIdleDictionary = "amb@world_human_seat_wall_eating@male@both_hands@idle_a";
            //        AnimIdle = new List<string>() { "Idle_c" };
            //    }
            //}
            //else
            //{
            //    AnimIdleDictionary = "amb@code_human_wander_eating_donut@female@idle_a";
            //    AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            //    AnimBase = "base";
            //    AnimBaseDictionary = "amb@code_human_wander_eating_donut@female@base";
            //    AnimEnter = "static";
            //    AnimEnterDictionary = "amb@code_human_wander_eating_donut@female@base";

            //    if (Player.IsSitting || Player.IsInVehicle)
            //    {
            //        AnimIdleDictionary = "amb@world_human_seat_wall_eating@female@sandwich_right_hand@idle_a";
            //        AnimIdle = new List<string>() { "idle_a" };
            //    }
            //}
            if (ModItem != null && ModItem.ModelItem != null)
            {
                //HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffsetOverride;
                HandRotator = ModItem.ModelItem.AttachRotationOverride;
                PropModel = ModItem.ModelItem.ModelName;
            }




            //works, but need to redo all the food attachments
            //HandBoneID = 18905;


            HandOffset = Vector3.Zero;
            HandRotator = Rotator.Zero;


            AnimIdleDictionary = "mp_player_inteat@burger";
            AnimIdle = new List<string>() { "mp_player_int_eat_burger" };
            AnimBase = "mp_player_int_eat_burger_enter";
            AnimBaseDictionary = "mp_player_inteat@burger";
            AnimEnter = "mp_player_int_eat_burger_enter";
            AnimEnterDictionary = "mp_player_inteat@burger";


            if (Settings.SettingsManager.PlayerOtherSettings.OverwriteHandOffset)
            {
                HandOffset = new Vector3(Settings.SettingsManager.PlayerOtherSettings.HandOffsetX, Settings.SettingsManager.PlayerOtherSettings.HandOffsetY, Settings.SettingsManager.PlayerOtherSettings.HandOffsetZ);
                HandRotator = new Rotator(Settings.SettingsManager.PlayerOtherSettings.HandRotateX, Settings.SettingsManager.PlayerOtherSettings.HandRotateY, Settings.SettingsManager.PlayerOtherSettings.HandRotateZ);
            }



            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, PropModel);
        }
    }
}