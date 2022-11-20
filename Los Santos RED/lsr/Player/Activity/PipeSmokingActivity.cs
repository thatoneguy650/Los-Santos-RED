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
    public class PipeSmokingActivity : DynamicActivity//this one needs lotsa work!!!
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
        private IActionable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private bool PrevHandByFace = false;
        private ISettingsProvideable Settings;
        private bool ShouldContinue;
        private LoopedParticle Smoke;
        private Rage.Object SmokedItem;
        private Rage.Object LighterItem;
        private PipeSmokeItem PipeSmokeItem;

        private int LeftHandBoneID = 18905;
        private Vector3 LighterOffset = new Vector3(0.13f,0.02f,0.02f);
        private Rotator LighterRotator = new Rotator(-93f,40f,0f);
        private int HandBoneID;
        private int MouthBoneID;

        public PipeSmokingActivity(IActionable consumable, bool isPot, ISettingsProvideable settings) : base()
        {
            Player = consumable;
            IsPot = isPot;
            Settings = settings;
        }
        public PipeSmokingActivity(IActionable consumable, ISettingsProvideable settings, PipeSmokeItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            PipeSmokeItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"IsAttachedToMouth: {IsSmokedItemAttachedToMouth} IsLit: {IsSmokedItemLit} HandByFace: {IsHandByFace} H&F: {Math.Round(DistanceBetweenHandAndFace, 3)}, {Math.Round(MinDistanceBetweenHandAndFace, 3)}";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Smoking";
        public override string CancelPrompt { get; set; } = "Stop Smoking";
        public override string ContinuePrompt { get; set; } = "Continue Smoking";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Continue()
        {
            if (Player.ActivityManager.CanPerformActivitiesExtended)
            {
                Setup();
                ShouldContinue = true;
            }
        }
        public override void Pause()
        {
            isPaused = true;
            Player.ActivityManager.IsPerformingActivity = false;
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
        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitesBase)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }
        private void AttachSmokedItemToHand()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists())
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsSmokedItemAttachedToMouth = false;
                Player.AttachedProp.Add(SmokedItem);
            }
            if(LighterItem.Exists())
            {
                LighterItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, LeftHandBoneID), LighterOffset, LighterRotator);
                Player.AttachedProp.Add(LighterItem);
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
                    InventoryItem LighterInventoryItem = Player.Inventory.Get(typeof(LighterItem));//LighterInventoryItem = Player.Inventory.Items.Where(x => x.ModItem?.ToolType == ToolTypes.Lighter).FirstOrDefault();
                    if (LighterInventoryItem != null)
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
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
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
            Player.ActivityManager.IsPerformingActivity = false;
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
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, 50, 0, false, false, false);
            IsActivelySmoking = true;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && !isPaused)
            {
                Player.WeaponEquipment.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    //if (!hasGainedHP)//get health once you finish it once, but you can still continue drinking, might chnage it to a duration based
                    //{
                    //    Player.ChangeHealth(ModItem.MaxHealthChangeAmount);
                    //    hasGainedHP = true;
                    //}
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, 50, 0, false, false, false);
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

            HandBoneID = 57005;
            MouthBoneID = 17188;

            string HandBoneName;
            Vector3 HandOffset = new Vector3();// new Vector3(0.13f, 0.04f, -0.01f);
            Rotator HandRotator = new Rotator();// new Rotator(-92f, 0f, 0f);
            string AnimBase;
            string AnimBaseDictionary;
            string AnimEnter;
            string AnimEnterDictionary;
            string AnimExit;
            string AnimExitDictionary;
            List<string> AnimIdle;
            string AnimIdleDictionary;
            string MouthBoneName;
            Vector3 MouthOffset;
            Rotator MouthRotator;
            string PropModelName = "";
            
            AnimBaseDictionary = "timetable@ron@smoking_meth";
            AnimBase = "";
            AnimIdleDictionary = "timetable@ron@smoking_meth";
            AnimIdle = new List<string> { "ig_4_idle_a" };//"idle_a", "idle_b", these are kinda bad
            AnimEnterDictionary = "timetable@ron@smoking_meth";
            AnimEnter = "";
            AnimExitDictionary = "timetable@ron@smoking_meth";
            AnimExit = "";
            HasLightingAnimation = false;     



            HandOffset = new Vector3();
            HandRotator = new Rotator();
            HandBoneName = "BONETAG_R_PH_HAND";
            MouthOffset = new Vector3();
            MouthRotator = new Rotator();
            MouthBoneName = "";
            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                }
            }




            if (ModItem != null && PipeSmokeItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(PipeSmokeItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SmokingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, MouthBoneName, MouthOffset, MouthRotator, PropModelName);
        }
        private void UpdatePosition()
        {
            DistanceBetweenHandAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, HandBoneID)).DistanceTo(NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)));
            if (DistanceBetweenHandAndFace <= MinDistanceBetweenHandAndFace)
            {
                MinDistanceBetweenHandAndFace = DistanceBetweenHandAndFace;
            }
            if (SmokedItem.Exists())
            {
                DistanceBetweenSmokedItemAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)).DistanceTo(SmokedItem);
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