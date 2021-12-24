using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player
{
    public class SmokingActivity : DynamicActivity
    {
        private string PlayingAnim;
        private string PlayingDict;
        private SmokingData Data;
        private float DistanceBetweenHandAndFace = 999f;
        private float DistanceBetweenSmokedItemAndFace = 999f;
        private uint GameTimeToStartSmoke;
        private uint GameTimeToStopSmoke;
        private IntoxicatingEffect IntoxicatingEffect;
        private bool IsActivelySmoking;
        private bool IsCancelled;
        private bool IsEmittingSmoke;
        private bool IsHandByFace;
        private bool IsPot;
        private bool IsSmokedItemAttachedToMouth;
        private bool IsSmokedItemLit;
        private bool IsSmokedItemNearMouth;
        private float MinDistanceBetweenHandAndFace = 999f;
        private float MinDistanceBetweenSmokedItemAndFace = 999f;
        private IIntoxicatable Player;
        private bool PrevHandByFace = false;
        private bool ShouldContinue;
        private LoopedParticle Smoke;
        private Rage.Object SmokedItem;
        private ISettingsProvideable Settings;
        private IIntoxicants Intoxicants;
        private Intoxicant CurrentIntoxicant;
        private bool IsPaused = false;
        private bool HasLightingAnimation = true;
        private bool hasGainedHP = false;

        public SmokingActivity(IIntoxicatable consumable, bool isPot, ISettingsProvideable settings) : base()
        {
            Player = consumable;
            IsPot = isPot;
            Settings = settings;
        }
        public SmokingActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => $"IsAttachedToMouth: {IsSmokedItemAttachedToMouth} IsLit: {IsSmokedItemLit} HandByFace: {IsHandByFace} H&F: {Math.Round(DistanceBetweenHandAndFace, 3)}, {Math.Round(MinDistanceBetweenHandAndFace, 3)}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
        }
        public override void Pause()
        {
            IsPaused = true;
            Player.IsPerformingActivity = false;
        }
        public override void Continue()
        {
            if (Player.CanPerformActivities)
            {
                Setup();
                ShouldContinue = true;
            }
        }
        public override void Start()
        {
            IsPaused = false;
            Setup();
            GameFiber SmokingWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "SmokingWatcher");
        }
        private void AttachSmokedItemToHand()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists() && IsSmokedItemAttachedToMouth)
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                IsSmokedItemAttachedToMouth = false;
                Player.AttachedProp = SmokedItem;
            }
        }
        private void AttachSmokedItemToMouth()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists() && !IsSmokedItemAttachedToMouth)
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.MouthBoneID), Data.MouthOffset, Data.MouthRotator);
                IsSmokedItemAttachedToMouth = true;
                Player.AttachedProp = SmokedItem;
                if (!HasLightingAnimation)
                {
                    IsActivelySmoking = true;
                    IsSmokedItemLit = true;
                }
            }
        }
        private void CreateSmokedItem()
        {
            if (!SmokedItem.Exists())
            {
                try
                {
                    SmokedItem = new Rage.Object(Game.GetHashKey(Data.PropModelName), Player.Character.GetOffsetPositionUp(50f));
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
            Player.IsPerformingActivity = true;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            EntryPoint.WriteToConsole($"Smoking Activity Playing {Data.AnimEnterDictionary} {Data.AnimEnter}", 5);
            while (Player.CanPerformActivities && !IsPaused && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter) < 1.0f)//NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Player.Character, AnimEnterDictionary, AnimEnter, 1))// && CurrentAnimationTime < 1.0f)
            {
                Player.SetUnarmed();
                UpdatePosition();
                UpdateSmoke();
                if (PrevHandByFace != IsHandByFace)
                {
                    if (IsHandByFace)
                    {
                        if (!IsSmokedItemAttachedToMouth && !IsSmokedItemLit)
                        {
                            
                            AttachSmokedItemToMouth();
                            EntryPoint.WriteToConsole($"Smoking Activity IsAttachedToMouth {IsSmokedItemAttachedToMouth} IsLit {IsSmokedItemLit} HandByFace {IsHandByFace} {DistanceBetweenHandAndFace}", 5);
                        }
                        else if (IsSmokedItemAttachedToMouth && !IsSmokedItemLit)
                        {
                            IsActivelySmoking = true;
                            IsSmokedItemLit = true;
                            EntryPoint.WriteToConsole($"Smoking Activity Light Cigarette {IsSmokedItemAttachedToMouth} IsLit {IsSmokedItemLit} HandByFace {IsHandByFace} {DistanceBetweenHandAndFace}", 5);
                        }
                        else if (IsSmokedItemAttachedToMouth && IsSmokedItemLit)
                        {
                            AttachSmokedItemToHand();
                            EntryPoint.WriteToConsole($"Smoking Activity AttachSmokedItemToHand {IsSmokedItemAttachedToMouth} IsLit {IsSmokedItemLit} HandByFace {IsHandByFace} {DistanceBetweenHandAndFace}", 5);
                        }
                    }
                    EntryPoint.WriteToConsole($"HandByFace Changed To {IsHandByFace}",5);
                    PrevHandByFace = IsHandByFace;
                }
                GameFiber.Yield();
            }
            GameFiber.Sleep(100);
            if (!Player.CanPerformActivities || IsPaused)
            {
                if (IsSmokedItemLit && IsSmokedItemNearMouth)
                {
                    InactiveIdle();//attach to mouth and let it inactively smoke
                }
                else
                {
                    Exit();//drop the cigarette (with animation if possible)
                }
            }
            else
            {
                Idle();//Start puffing from the cigarette
            }
        }
        private void Idle()
        {
            EntryPoint.WriteToConsole("SmokingActivity Idle Start", 5);
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            EntryPoint.WriteToConsole($"Smoking Activity Playing {PlayingDict} {PlayingAnim}", 5);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
            while (Player.CanPerformActivities && !IsCancelled && !IsPaused)
            {
                Player.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    if (!hasGainedHP)//get health once you finish it once, but you can still continue drinking, might chnage it to a duration based
                    {
                        Player.AddHealth(ModItem.HealthGained);
                        hasGainedHP = true;
                    }
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                }
                UpdatePosition();
                UpdateSmoke();
                if (IsHandByFace)
                {
                    AttachSmokedItemToHand();
                }
                GameFiber.Yield();
            }
            EntryPoint.WriteToConsole("SmokingActivity Idle End", 5);
            if (IsSmokedItemNearMouth)
            {
                InactiveIdle();
            }
            else
            {
                Exit();
            }
        }
        private void InactiveIdle()
        {
            EntryPoint.WriteToConsole("SmokingActivity InactiveIdle Start", 5);
            AttachSmokedItemToMouth();
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            IsActivelySmoking = false;
            uint GameTimeStartedIdle = Game.GameTime;
            while (!IsCancelled && !ShouldContinue && Game.GameTime - GameTimeStartedIdle <= 120000)//two minutes and your ciggy burns out
            {
                UpdatePosition();
                UpdateSmoke();
                GameFiber.Yield();
            }
            if (ShouldContinue && !IsCancelled)
            {
                ShouldContinue = false;
                IsActivelySmoking = true;
                IsPaused = false;
                Player.IsPerformingActivity = true;
                Idle();
            }
            else
            {
                Exit();
            }
        }
        private void Exit()
        {
            EntryPoint.WriteToConsole("SmokingActivity Exit Start", 5);
            if (IsActivelySmoking && Player.CanPerformActivities)
            {
                EntryPoint.WriteToConsole($"Smoking Activity Playing {Data.AnimExitDictionary} {Data.AnimExit}", 5);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimExitDictionary, Data.AnimExit, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                uint GameTimeStartedExitAnimation = Game.GameTime;
                while (Game.GameTime - GameTimeStartedExitAnimation <= 5000 && Player.CanPerformActivities && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimExitDictionary, Data.AnimExit) < 1.0f)
                {
                    Player.SetUnarmed();
                    if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimExitDictionary, Data.AnimExit) >= 0.8f && SmokedItem.Exists())
                    {
                        SmokedItem.Detach();
                    }
                    GameFiber.Yield();
                }
            }
            if (SmokedItem.Exists())
            {
                SmokedItem.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
            EntryPoint.WriteToConsole("SmokingActivity Exit End", 5);
            GameFiber.Sleep(5000);
            if (SmokedItem.Exists())
            {
                SmokedItem.Delete();
            }
        }
        private void Setup()
        {
            int HandBoneID;
            Vector3 HandOffset;
            Rotator HandRotator;
            string AnimBase;
            string AnimBaseDictionary;
            string AnimEnter;
            string AnimEnterDictionary;
            string AnimExit;
            string AnimExitDictionary;
            List<string> AnimIdle;
            string AnimIdleDictionary;
            int MouthBoneID;
            Vector3 MouthOffset;
            Rotator MouthRotator;
            string PropModelName = "ng_proc_cigarette01a";
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two")
            {
                if (RandomItems.RandomPercent(50))
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_b@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_b@idle_a";
                    AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_b@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_b@exit";
                    HandOffset = new Vector3(0.141f, 0.03f, -0.033f);
                    HandRotator = new Rotator(0.0f, -168f, -84f);
                }
                else
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                    AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                    HandOffset = new Vector3(0.1640f, 0.019f, 0.0f);
                    HandRotator = new Rotator(0.49f, 79f, 79f);
                }
                AnimBase = "base";
                AnimEnter = "enter";
                AnimExit = "exit";
                HandBoneID = 57005;
                MouthBoneID = 31086;
                MouthOffset = new Vector3(-0.007f, 0.13f, 0.01f);
                MouthRotator = new Rotator(0.0f, -175f, 91f);
            }
            else if (Player.IsMale)
            {
                if (RandomItems.RandomPercent(50))
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_b@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_b@idle_a";
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_b@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_b@exit";
                    HandOffset = new Vector3(0.141f, 0.03f, -0.033f);
                    HandRotator = new Rotator(0.0f, -168f, -84f);
                }
                else
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                    HandOffset = new Vector3(0.14f, 0.03f, 0.0f);
                    HandRotator = new Rotator(0.49f, 79f, 79f);
                }
                AnimBase = "base";
                AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };
                AnimEnter = "enter";
                AnimExit = "exit";
                HandBoneID = 57005;
                MouthBoneID = 17188;
                MouthOffset = new Vector3(0.046f, 0.015f, 0.014f);
                MouthRotator = new Rotator(0.0f, -180f, 0f);
            }
            else
            {
                AnimBaseDictionary = "amb@world_human_smoking@female@idle_a";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_smoking@female@idle_a";
                AnimIdle = new List<string> { "idle_c" };//"idle_a", "idle_b", these are kinda bad
                AnimEnterDictionary = "amb@world_human_smoking@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_smoking@female@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.14f, 0.01f, 0.0f);
                HandRotator = new Rotator(0.49f, 79f, 79f);
                MouthBoneID = 17188;
                MouthOffset = new Vector3(0.046f, 0.015f, 0.014f);
                MouthRotator = new Rotator(0.0f, -180f, 0f);
            }

            if (Player.IsSitting || Player.IsInVehicle)
            {
                AnimBaseDictionary = "amb@incar@male@smoking@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@incar@male@smoking@idle_a";
                AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };//"idle_a", "idle_b", these are kinda bad
                AnimEnterDictionary = "amb@incar@male@smoking@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@incar@male@smoking@exit";
                AnimExit = "exit";
                HasLightingAnimation = false;
            }

            if(ModItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                //if (ModItem.IsIntoxicating)
                //{
                //    IntoxicatingEffect = new IntoxicatingEffect(Player, Settings.SettingsManager.ActivitySettings.Marijuana_MaxEffectAllowed, Settings.SettingsManager.ActivitySettings.Marijuana_TimeToReachEachIntoxicatedLevel, Settings.SettingsManager.ActivitySettings.Marijuana_TimeToReachEachSoberLevel, Settings.SettingsManager.ActivitySettings.Marijuana_Overlay);
                //    IntoxicatingEffect.Start();
                //}
            }
            else
            {
                if (IsPot)
                {
                    PropModelName = Settings.SettingsManager.ActivitySettings.Marijuana_PossibleProps.PickRandom();
                    IntoxicatingEffect = new IntoxicatingEffect(Player, Settings.SettingsManager.ActivitySettings.Marijuana_MaxEffectAllowed, Settings.SettingsManager.ActivitySettings.Marijuana_TimeToReachEachIntoxicatedLevel, Settings.SettingsManager.ActivitySettings.Marijuana_TimeToReachEachSoberLevel, Settings.SettingsManager.ActivitySettings.Marijuana_Overlay);
                    IntoxicatingEffect.Start();
                }
                else
                {
                    PropModelName = Settings.SettingsManager.ActivitySettings.Cigarette_PossibleProps.PickRandom();
                }
            }

            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.StartIngesting(CurrentIntoxicant);
            }

            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SmokingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, MouthBoneID, MouthOffset, MouthRotator, PropModelName);
        }
        private void UpdatePosition()
        {
            DistanceBetweenHandAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, Data.HandBoneID)).DistanceTo(NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, Data.MouthBoneID)));
            if (DistanceBetweenHandAndFace <= MinDistanceBetweenHandAndFace)
            {
                MinDistanceBetweenHandAndFace = DistanceBetweenHandAndFace;
            }
            if (SmokedItem.Exists())
            {
                DistanceBetweenSmokedItemAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, Data.MouthBoneID)).DistanceTo(SmokedItem);
                if (DistanceBetweenSmokedItemAndFace <= MinDistanceBetweenSmokedItemAndFace)
                {
                    MinDistanceBetweenSmokedItemAndFace = DistanceBetweenSmokedItemAndFace;
                }
            }
            if (Player.IsSitting || Player.IsInVehicle)
            {
                if (DistanceBetweenSmokedItemAndFace <= 0.19f && SmokedItem.Exists())
                {
                    IsSmokedItemNearMouth = true;
                }
                else
                {
                    IsSmokedItemNearMouth = false;
                }
            }
            else
            {
                if (DistanceBetweenSmokedItemAndFace <= 0.17f && SmokedItem.Exists())
                {
                    IsSmokedItemNearMouth = true;
                }
                else
                {
                    IsSmokedItemNearMouth = false;
                }
            }
            if(Player.IsSitting || Player.IsInVehicle)
            {
                if (DistanceBetweenHandAndFace <= 0.27f)//0.2f
                {
                    IsHandByFace = true;
                }
                else
                {
                    IsHandByFace = false;
                }
            }
            else
            {
                if (DistanceBetweenHandAndFace <= 0.25f)//0.2f
                {
                    IsHandByFace = true;
                }
                else
                {
                    IsHandByFace = false;
                }
            }


           // EntryPoint.WriteToConsole($"Smoking Activity: DistanceBetweenHandAndFace {DistanceBetweenHandAndFace} IsHandByFace {IsHandByFace}", 5);
        }
        private void UpdateSmoke()
        {
            if (IsSmokedItemLit)
            {
                if (IsActivelySmoking)
                {
                    if (IsSmokedItemNearMouth && IsSmokedItemLit && !IsEmittingSmoke)
                    {
                        Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", SmokedItem, new Vector3(-0.07f, 0.0f, 0f), Rotator.Zero, 1.5f);
                        IsEmittingSmoke = true;
                    }
                    if (!IsSmokedItemNearMouth && IsEmittingSmoke && Smoke != null)
                    {
                        Smoke.Stop();
                        IsEmittingSmoke = false;
                    }
                }
                else
                {
                    if (!IsEmittingSmoke)
                    {
                        if (GameTimeToStopSmoke <= Game.GameTime && Game.GameTime >= GameTimeToStartSmoke)
                        {
                            IsEmittingSmoke = true;
                            Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", SmokedItem, new Vector3(-0.07f, 0.0f, 0f), Rotator.Zero, 1.5f);
                            GameTimeToStopSmoke = Game.GameTime + (uint)RandomItems.MyRand.Next(1200, 1500);
                        }
                    }
                    else
                    {
                        if (Game.GameTime >= GameTimeToStopSmoke)
                        {
                            IsEmittingSmoke = false;
                            Smoke.Stop();
                            GameTimeToStartSmoke = Game.GameTime + (uint)RandomItems.MyRand.Next(3500, 5000);
                        }
                    }
                }
            }
        }
    }
}