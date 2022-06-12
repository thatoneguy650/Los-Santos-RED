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
    public class PipeSmokingActivity : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private SmokingData Data;
        private float DistanceBetweenHandAndFace = 999f;
        private float DistanceBetweenSmokedItemAndFace = 999f;
        private uint GameTimeToStartSmoke;
        private uint GameTimeToStopSmoke;
        private bool hasGainedHP = false;
        private bool HasLightingAnimation = true;
        private IIntoxicants Intoxicants;
        private bool IsActivelySmoking;
        private bool IsCancelled;
        private bool IsEmittingSmoke;
        private bool IsHandByFace;
        private bool isPaused = false;
        private bool IsPot;
        private bool IsSmokedItemAttachedToMouth;
        private bool IsSmokedItemLit;
        private bool IsSmokedItemNearMouth;
        private float MinDistanceBetweenHandAndFace = 999f;
        private float MinDistanceBetweenSmokedItemAndFace = 999f;
        private IIntoxicatable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private bool PrevHandByFace = false;
        private ISettingsProvideable Settings;
        private bool ShouldContinue;
        private LoopedParticle Smoke;
        private Rage.Object SmokedItem;
        private Rage.Object LighterItem;


        private int LeftHandBoneID = 18905;
        private Vector3 LighterOffset = new Vector3(0.13f,0.02f,0.02f);
        private Rotator LighterRotator = new Rotator(-93f,40f,0f);

        public PipeSmokingActivity(IIntoxicatable consumable, bool isPot, ISettingsProvideable settings) : base()
        {
            Player = consumable;
            IsPot = isPot;
            Settings = settings;
        }
        public PipeSmokingActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"IsAttachedToMouth: {IsSmokedItemAttachedToMouth} IsLit: {IsSmokedItemLit} HandByFace: {IsHandByFace} H&F: {Math.Round(DistanceBetweenHandAndFace, 3)}, {Math.Round(MinDistanceBetweenHandAndFace, 3)}";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Continue()
        {
            if (Player.CanPerformActivities)
            {
                Setup();
                ShouldContinue = true;
            }
        }
        public override void Pause()
        {
            isPaused = true;
            Player.IsPerformingActivity = false;
        }
        public override bool IsPaused() => isPaused;
        public override void Start()
        {
            isPaused = false;
            Setup();
            GameFiber SmokingWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "SmokingWatcher");
        }
        private void AttachSmokedItemToHand()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists())
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                IsSmokedItemAttachedToMouth = false;
                Player.AttachedProp = SmokedItem;
            }
            if(LighterItem.Exists())
            {
                LighterItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, LeftHandBoneID), LighterOffset, LighterRotator);
                Player.AttachedProp = LighterItem;
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
            if (!LighterItem.Exists())
            {
                try
                {
                    InventoryItem LighterInventoryItem = Player.Inventory.Items.Where(x => x.ModItem?.ToolType == ToolTypes.Lighter).FirstOrDefault();
                    if(LighterInventoryItem != null)
                    {
                        LighterItem = new Rage.Object(Game.GetHashKey(LighterInventoryItem.ModItem.ModelItem?.ModelName), Player.Character.GetOffsetPositionUp(60f));
                    }
                    else
                    {
                        LighterItem = new Rage.Object(Game.GetHashKey("p_cs_lighter_01"), Player.Character.GetOffsetPositionUp(60f));
                    }


                    
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop p_cs_lighter_01");
                }
            }
        }
        private void Enter()
        {
            Player.SetUnarmed();
            Player.IsPerformingActivity = true;
            Idle();
        }
        private void Exit()
        {
            EntryPoint.WriteToConsole("SmokingActivity Exit Start", 5);
            if (SmokedItem.Exists())
            {
                SmokedItem.Detach();
            }
            if (LighterItem.Exists())
            {
                LighterItem.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            EntryPoint.WriteToConsole("SmokingActivity Exit End", 5);
            GameFiber.Sleep(5000);
            if (SmokedItem.Exists())
            {
                SmokedItem.Delete();
            }
            if (LighterItem.Exists())
            {
                LighterItem.Delete();
            }
        }
        private void Idle()
        {
            AttachSmokedItemToHand();
            EntryPoint.WriteToConsole("SmokingActivity Idle Start", 5);
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            EntryPoint.WriteToConsole($"Smoking Activity Playing {PlayingDict} {PlayingAnim}", 5);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
            IsActivelySmoking = true;
            while (Player.CanPerformActivities && !IsCancelled && !isPaused)
            {
                Player.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    //if (!hasGainedHP)//get health once you finish it once, but you can still continue drinking, might chnage it to a duration based
                    //{
                    //    Player.ChangeHealth(ModItem.MaxHealthChangeAmount);
                    //    hasGainedHP = true;
                    //}
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                }
                UpdatePosition();
                UpdateSmoke();
                GameFiber.Yield();
            }
            EntryPoint.WriteToConsole("SmokingActivity Idle End", 5);
            Exit();
            
        }
     
        private void Setup()
        {
            int HandBoneID = 57005;
            Vector3 HandOffset = new Vector3(0.13f, 0.04f, -0.01f);
            Rotator HandRotator = new Rotator(-92f, 0f, 0f);
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
            
            AnimBaseDictionary = "timetable@ron@smoking_meth";
            AnimBase = "";
            AnimIdleDictionary = "timetable@ron@smoking_meth";
            AnimIdle = new List<string> { "ig_4_idle_a" };//"idle_a", "idle_b", these are kinda bad
            AnimEnterDictionary = "timetable@ron@smoking_meth";
            AnimEnter = "";
            AnimExitDictionary = "timetable@ron@smoking_meth";
            AnimExit = "";
            HasLightingAnimation = false;     

            if (ModItem != null && ModItem.ModelItem != null)
            {
                HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffset;
                HandRotator = ModItem.ModelItem.AttachRotation;
                PropModelName = ModItem.ModelItem.ModelName;
            }
            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SmokingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, 0, Vector3.Zero, Rotator.Zero, PropModelName);
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
            if (DistanceBetweenSmokedItemAndFace <= 0.77f && SmokedItem.Exists())
            {
                IsSmokedItemNearMouth = true;
            }
            else
            {
                IsSmokedItemNearMouth = false;
            }    
            if (DistanceBetweenHandAndFace <= 0.64f)//0.2f
            {
                IsHandByFace = true;
            }
            else
            {
                IsHandByFace = false;
            }
            Player.DebugLine4 = $"Smoking Activity: DistanceBetweenHandAndFace {Math.Round(DistanceBetweenHandAndFace,2,MidpointRounding.AwayFromZero)} DistanceBetweenSmokedItemAndFace {Math.Round(DistanceBetweenSmokedItemAndFace, 2, MidpointRounding.AwayFromZero)} IsHandByFace {IsHandByFace} IsSmokedItemNearMouth {IsSmokedItemNearMouth}";
            // EntryPoint.WriteToConsole($"Smoking Activity: DistanceBetweenHandAndFace {DistanceBetweenHandAndFace} IsHandByFace {IsHandByFace}", 5);
        }
        private void UpdateSmoke()
        {
            if (IsActivelySmoking)
            {
                if (IsSmokedItemNearMouth && !IsEmittingSmoke)
                {
                    Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", SmokedItem, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, 1.5f);
                    IsEmittingSmoke = true;
                }
                if (!IsSmokedItemNearMouth && IsEmittingSmoke && Smoke != null)
                {
                    Smoke.Stop();
                    IsEmittingSmoke = false;
                }
            }
        }
    }
}