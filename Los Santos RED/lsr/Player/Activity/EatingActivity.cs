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
                Food.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
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
        private void Idle()
        {
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            EntryPoint.WriteToConsole($"Eating Activity Playing {PlayingDict} {PlayingAnim}", 5);
            //while (Player.CanPerformActivities && !IsCancelled)
            //{
            //    Player.SetUnarmed();
            //    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            //    UpdateHealthGain();
            //    if (AnimationTime >= 0.9f)
            //    {
            //        if (Food.Exists())
            //        {
            //            Food.Delete();
            //        }
            //    }
            //    if (AnimationTime >= 1.0f)
            //    {
            //        break;
            //    }
            //    GameFiber.Yield();
            //}
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f)
                {
                    if (TimesAte >= 1 && (HealthGiven == ModItem.HealthChangeAmount || Player.Character.Health == Player.Character.MaxHealth))
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
                        PlayingDict = Data.AnimIdleDictionary;
                        PlayingAnim = Data.AnimIdle.PickRandom();
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                        EntryPoint.WriteToConsole($"New Eating Idle {PlayingAnim} TimesAte {TimesAte} HealthGiven {HealthGiven}", 5);
                    }
                }
                UpdateHealthGain();
                GameFiber.Yield();
            }
            Exit();
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
                }
                GameTimeLastGivenHealth = Game.GameTime;
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

            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                AnimIdleDictionary = "amb@code_human_wander_eating_donut@male@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
                AnimBase = "base";
                AnimBaseDictionary = "amb@code_human_wander_eating_donut@male@base";
                AnimEnter = "static";
                AnimEnterDictionary = "amb@code_human_wander_eating_donut@male@base";

                if (Player.IsSitting || Player.IsInVehicle)
                {
                    AnimIdleDictionary = "amb@world_human_seat_wall_eating@male@both_hands@idle_a";
                    AnimIdle = new List<string>() { "Idle_c" };
                }
            }
            else
            {
                AnimIdleDictionary = "amb@code_human_wander_eating_donut@female@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
                AnimBase = "base";
                AnimBaseDictionary = "amb@code_human_wander_eating_donut@female@base";
                AnimEnter = "static";
                AnimEnterDictionary = "amb@code_human_wander_eating_donut@female@base";

                if (Player.IsSitting || Player.IsInVehicle)
                {
                    AnimIdleDictionary = "amb@world_human_seat_wall_eating@female@sandwich_right_hand@idle_a";
                    AnimIdle = new List<string>() { "idle_a" };
                }
            }
            if (ModItem != null && ModItem.ModelItem != null)
            {
                HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffset;
                HandRotator = ModItem.ModelItem.AttachRotation;
                PropModel = ModItem.ModelItem.ModelName;
            }

            //works, but need to redo all the food attachments
            //HandBoneID = 18905;
            //AnimIdleDictionary = "mp_player_inteat@burger";
            //AnimIdle = new List<string>() { "mp_player_int_eat_burger" };
            //AnimBase = "mp_player_int_eat_burger_enter";
            //AnimBaseDictionary = "mp_player_inteat@burger";
            //AnimEnter = "mp_player_int_eat_burger_enter";
            //AnimEnterDictionary = "mp_player_inteat@burger";






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