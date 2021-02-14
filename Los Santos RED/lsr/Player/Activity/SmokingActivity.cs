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
        private bool PrevHandByFace;
        private bool ShouldContinue;
        private LoopedParticle Smoke;
        private Rage.Object SmokedItem;
        public SmokingActivity(IIntoxicatable consumable, bool isPot) : base()
        {
            Player = consumable;
            IsPot = isPot;
        }
        public override string DebugString => $"IsAttachedToMouth: {IsSmokedItemAttachedToMouth} IsLit: {IsSmokedItemLit} HandByFace: {IsHandByFace} H&F: {Math.Round(DistanceBetweenHandAndFace, 3)}, {Math.Round(MinDistanceBetweenHandAndFace, 3)}";
        public override void Cancel()
        {
            IsCancelled = true;
        }
        public override void Continue()
        {
            if (Player.CanPerformActivities)
            {
                ShouldContinue = true;
            }
        }
        public override void Start()
        {
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
            }
        }
        private void AttachSmokedItemToMouth()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists() && !IsSmokedItemAttachedToMouth)
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.MouthBoneID), Data.MouthOffset, Data.MouthRotator);
                IsSmokedItemAttachedToMouth = true;
            }
        }
        private void CreateSmokedItem()
        {
            if (!SmokedItem.Exists())
            {
                SmokedItem = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
            }
        }
        private void Enter()
        {
            Player.SetUnarmed();
            Player.IsPerformingActivity = true;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            GameFiber.Sleep(500);
            while (Player.CanPerformActivities && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter) < 1.0f)//NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Player.Character, AnimEnterDictionary, AnimEnter, 1))// && CurrentAnimationTime < 1.0f)
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
                            //Game.Console.Print($"IsAttachedToMouth {IsSmokedItemAttachedToMouth} IsLit {IsSmokedItemLit} HandByFace {IsHandByFace} {DistanceBetweenHandAndFace}"); 
                            AttachSmokedItemToMouth();
                        }
                        else if (IsSmokedItemAttachedToMouth && !IsSmokedItemLit)
                        {
                            IsActivelySmoking = true;
                            IsSmokedItemLit = true;
                        }
                        else if (IsSmokedItemAttachedToMouth && IsSmokedItemLit)
                        {
                            AttachSmokedItemToHand();
                        }
                    }
                    //Game.Console.Print($"HandByFace Changed To {IsHandByFace}");
                    PrevHandByFace = IsHandByFace;
                }
                GameFiber.Yield();
            }
            GameFiber.Sleep(100);
            if (!Player.CanPerformActivities)
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
        private void Exit()
        {
            if (IsActivelySmoking && Player.CanPerformActivities)
            {
                //Game.Console.Print($"Stop with Animation");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimExitDictionary, Data.AnimExit, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                while (Player.CanPerformActivities && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimExitDictionary, Data.AnimExit) < 1.0f)
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
            Player.Character.Tasks.Clear();
            Player.IsPerformingActivity = false;
            GameFiber.Sleep(5000);
            if (SmokedItem.Exists())
            {
                SmokedItem.Delete();
            }
        }
        private void Idle()
        {
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                    //Game.Console.Print($"New Smoking Idle {PlayingAnim}");
                }
                UpdatePosition();
                UpdateSmoke();
                if (IsHandByFace)
                {
                    AttachSmokedItemToHand();
                }
                GameFiber.Yield();
            }
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
            AttachSmokedItemToMouth();
            Player.Character.Tasks.Clear();
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
                Idle();
            }
            else
            {
                Exit();
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
            string PropModelName;
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
                AnimIdle = new List<string> {  "idle_c" };//"idle_a", "idle_b", these are kinda bad
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
            if (IsPot)
            {
                PropModelName = "p_amb_joint_01";
            }
            else
            {
                PropModelName = "ng_proc_cigarette01a";
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SmokingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, MouthBoneID, MouthOffset, MouthRotator, PropModelName);

            if (IsPot)
            {
                IntoxicatingEffect = new IntoxicatingEffect(Player, 3.0f, 60000, 60000, "drug_wobbly");//2.5,25000//smoking was new IntoxicatingEffect(Player, 0.5f, 25000, 60000, "Bloom");//1.2,25000
                IntoxicatingEffect.Start();
            }
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
            if (DistanceBetweenSmokedItemAndFace <= 0.17f && SmokedItem.Exists())
            {
                IsSmokedItemNearMouth = true;
            }
            else
            {
                IsSmokedItemNearMouth = false;
            }
            if (DistanceBetweenHandAndFace <= 0.25f)//0.2f
            {
                IsHandByFace = true;
            }
            else
            {
                IsHandByFace = false;
            }
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