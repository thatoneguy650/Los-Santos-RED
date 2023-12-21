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
        private Rage.Object PipeObject;
        private Rage.Object LighterObject;


        private int LeftHandBoneID = 18905;
        private Vector3 LighterOffset = new Vector3(0.13f, 0.02f, 0.02f);
        private Rotator LighterRotator = new Rotator(-93f, 40f, 0f);
        private int HandBoneID;
        private int MouthBoneID;
        private ConsumableRefresher ConsumableItemNeedGain;
        private Vector3 LighterHandOffset;
        private Rotator LighterHandRotator;
        private string LighterHandBoneName;
        private string LighterPropModelName;

        private PipeItem PipeItem;
        private LighterItem LighterItem;
        private ConsumableItem ConsumableItem;

        public PipeSmokingActivity(IActionable consumable, ISettingsProvideable settings, PipeItem pipeItem, LighterItem lighterItem, ConsumableItem consumableItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = pipeItem;
            PipeItem = pipeItem;
            LighterItem = lighterItem;
            ConsumableItem = consumableItem;
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
            Player.ActivityManager.IsUsingIllegalItem = false;
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
            Player.ActivityManager.IsUsingIllegalItem = false;
        }
        public override bool IsPaused() => isPaused;
        public override void Start()
        {
            isPaused = false;
            if (!Setup())
            {
                Game.DisplayHelp("Cannot Start Activity");
                return;
            }
            GameFiber SmokingWatcher = GameFiber.StartNew(delegate
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
            }, "SmokingWatcher");
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
        private void AttachSmokedItemToHand()
        {
            CreateSmokedItem();
            if (PipeObject.Exists())
            {
                PipeObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsSmokedItemAttachedToMouth = false;
                Player.AttachedProp.Add(PipeObject);
            }
            if(LighterObject.Exists())
            {
                LighterObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, LighterHandBoneName), LighterHandOffset, LighterHandRotator); //LighterObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, LeftHandBoneID), LighterOffset, LighterRotator); //LighterObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, LighterHandBoneName), LighterHandOffset, LighterHandRotator);
                Player.AttachedProp.Add(LighterObject);
            }
        }
        private void CreateSmokedItem()
        {
            if (!PipeObject.Exists())
            {
                try
                {
                    PipeObject = new Rage.Object(Game.GetHashKey(Data.PropModelName), Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
            }
            if (!LighterObject.Exists())
            {
                try
                {
                    LighterObject = new Rage.Object(Game.GetHashKey(LighterItem.ModelItem?.ModelName), Player.Character.GetOffsetPositionUp(60f));                 
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
            if (ModItem.IsPublicUseIllegal)
            {
                Player.ActivityManager.IsUsingIllegalItem = true;
            }
            Idle();
        }
        private void Exit()
        {
            if (PipeObject.Exists())
            {
                PipeObject.Detach();
            }
            if (LighterObject.Exists())
            {
                LighterObject.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.IsUsingIllegalItem = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            GameFiber.Sleep(5000);
            if (PipeObject.Exists())
            {
                PipeObject.Delete();
            }
            if (LighterObject.Exists())
            {
                LighterObject.Delete();
            }
        }
        private void Idle()
        {
            ConsumableItemNeedGain = new ConsumableRefresher(Player, ConsumableItem, Settings);
            AttachSmokedItemToHand();
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, 50, 0, false, false, false);
            IsActivelySmoking = true;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && !isPaused)
            {
                Player.WeaponEquipment.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, 50, 0, false, false, false);
                }
                UpdatePosition();
                UpdateSmoke();
                ConsumableItemNeedGain.Update();
                GameFiber.Yield();
            }
            Exit();        
        }
     
        private bool Setup()
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



            if (PipeItem == null || LighterItem == null || ConsumableItem == null)
            {
                return false;
            }
            HandOffset = new Vector3();
            HandRotator = new Rotator();
            HandBoneName = "BONETAG_R_PH_HAND";
            MouthOffset = new Vector3();
            MouthRotator = new Rotator();
            MouthBoneName = "";
            if (PipeItem != null && PipeItem.ModelItem != null)
            {
                PropModelName = PipeItem.ModelItem.ModelName;
                PropAttachment pa = PipeItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                }
            }
            if (LighterItem != null && LighterItem.ModelItem != null)
            {
                LighterPropModelName = LighterItem.ModelItem.ModelName;
                PropAttachment pa = LighterItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "LeftHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    LighterHandOffset = pa.Attachment;
                    LighterHandRotator = pa.Rotation;
                    LighterHandBoneName = pa.BoneName;
                }
            }
            if (ConsumableItem != null && ConsumableItem.IsIntoxicating && ConsumableItem.Intoxicant != null)
            {
                CurrentIntoxicant = ConsumableItem.Intoxicant;
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }


            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SmokingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, MouthBoneName, MouthOffset, MouthRotator, PropModelName);
            return true;
        }
        private void UpdatePosition()
        {
            DistanceBetweenHandAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, HandBoneID)).DistanceTo(NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)));
            if (DistanceBetweenHandAndFace <= MinDistanceBetweenHandAndFace)
            {
                MinDistanceBetweenHandAndFace = DistanceBetweenHandAndFace;
            }
            if (PipeObject.Exists())
            {
                DistanceBetweenSmokedItemAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)).DistanceTo(PipeObject);
                if (DistanceBetweenSmokedItemAndFace <= MinDistanceBetweenSmokedItemAndFace)
                {
                    MinDistanceBetweenSmokedItemAndFace = DistanceBetweenSmokedItemAndFace;
                }
            }
            if (DistanceBetweenSmokedItemAndFace <= 0.77f && PipeObject.Exists())
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
                    Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", PipeObject, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, 1.5f);
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