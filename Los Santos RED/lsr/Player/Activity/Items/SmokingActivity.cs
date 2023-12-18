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
    public class SmokingActivity : DynamicActivity
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
        private uint GameTimeLastGivenHealth;
        private int HealthGiven;
        private int MouthBoneID;
        private int HandBoneID;
        private SmokeItem SmokeItem;
        private ConsumableRefresher ConsumableItemNeedGain;
        private uint GameTimeStartedSmoking;
        private Vector3 ParticleOffset;
        private Rotator ParticleRotation;
        private uint Duration;

        public SmokingActivity(IActionable consumable, ISettingsProvideable settings, SmokeItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            SmokeItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"IsAttachedToMouth: {IsSmokedItemAttachedToMouth} IsLit: {IsSmokedItemLit} HandByFace: {IsHandByFace} H&F: {Math.Round(DistanceBetweenHandAndFace, 3)}, {Math.Round(MinDistanceBetweenHandAndFace, 3)}";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = true;
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
                Player.ActivityManager.PausedActivites.Remove(this);
                Player.ActivityManager.PausedActivites.RemoveAll(x=> x.ModItem != null && ModItem != null && x.ModItem.Name == ModItem.Name);
            }
        }
        public override void Pause()
        {
            isPaused = true;
            Player.ActivityManager.IsPerformingActivity = false;
            Player.ActivityManager.AddPausedActivity(this);
        }
        public override bool IsPaused() => isPaused;
        public override void Start()
        {
            isPaused = false;
            Setup();
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
            if (SmokedItem.Exists() && IsSmokedItemAttachedToMouth)
            {
                //SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsSmokedItemAttachedToMouth = false;
                Player.AttachedProp.Add(SmokedItem);
            }
        }
        private void AttachSmokedItemToMouth()
        {
            CreateSmokedItem();
            if (SmokedItem.Exists() && !IsSmokedItemAttachedToMouth)
            {
                SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Data.MouthBoneName), Data.MouthOffset, Data.MouthRotator);
                IsSmokedItemAttachedToMouth = true;
                Player.AttachedProp.Add(SmokedItem);
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
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            ConsumableItemNeedGain = new ConsumableRefresher(Player, SmokeItem, Settings);
            GameTimeStartedSmoking = Game.GameTime;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !isPaused && !IsCancelled && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter) < 1.0f)//NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Player.Character, AnimEnterDictionary, AnimEnter, 1))// && CurrentAnimationTime < 1.0f)
            {
                Player.WeaponEquipment.SetUnarmed();
                UpdatePosition();
                UpdateSmoke();
                if (PrevHandByFace != IsHandByFace)
                {
                    if (IsHandByFace)
                    {
                        if (!IsSmokedItemAttachedToMouth && !IsSmokedItemLit)
                        {
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
                    PrevHandByFace = IsHandByFace;
                }
                GameFiber.Yield();
            }
            GameFiber.Sleep(100);
            if(IsCancelled)
            {
                Exit();//drop the cigarette (with animation if possible)
            }
            else if (!Player.ActivityManager.CanPerformActivitiesExtended || isPaused)
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
            //EntryPoint.WriteToConsole("SmokingActivity Exit Start");
            if (IsActivelySmoking && Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimExitDictionary, Data.AnimExit, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                uint GameTimeStartedExitAnimation = Game.GameTime;
                while (Game.GameTime - GameTimeStartedExitAnimation <= 5000 && Player.ActivityManager.CanPerformActivitiesExtended && NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, Data.AnimExitDictionary, Data.AnimExit) < 1.0f)
                {
                    Player.WeaponEquipment.SetUnarmed();
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
            Player.ActivityManager.IsPerformingActivity = false;
            isPaused = false;
            IsCancelled = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            EntryPoint.WriteToConsole("SmokingActivity Exit End");
            Player.ActivityManager.PausedActivites.Remove(this);
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
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && !isPaused && Game.GameTime - GameTimeStartedSmoking <= Duration)
            {
                Player.WeaponEquipment.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                }
                UpdatePosition();
                UpdateSmoke();
                ConsumableItemNeedGain.Update();
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
            Pause();
            AttachSmokedItemToMouth();
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            IsActivelySmoking = false;
            while (!IsCancelled && !ShouldContinue && Game.GameTime - GameTimeStartedSmoking <= Duration)//two minutes and your ciggy burns out
            {
                UpdatePosition();
                UpdateSmoke();
                ConsumableItemNeedGain.Update();
                GameFiber.Yield();
            }
            if (ShouldContinue && !IsCancelled)
            {
                ShouldContinue = false;
                IsActivelySmoking = true;
                isPaused = false;
                Player.ActivityManager.IsPerformingActivity = true;
                Idle();
            }
            else
            {
                Exit();
            }
        }
        private void UpdateHealthGain()
        {
            if (Game.GameTime - GameTimeLastGivenHealth >= 15000 && !Settings.SettingsManager.NeedsSettings.ApplyNeeds)
            {
                if (SmokeItem.ChangesHealth && !Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                {
                    if (SmokeItem.HealthChangeAmount > 0 && HealthGiven < SmokeItem.HealthChangeAmount)
                    {
                        HealthGiven++;
                        Player.HealthManager.ChangeHealth(1);
                    }
                    else if (SmokeItem.HealthChangeAmount < 0 && HealthGiven > SmokeItem.HealthChangeAmount)
                    {
                        HealthGiven--;
                        Player.HealthManager.ChangeHealth(-1);
                    }
                }
                GameTimeLastGivenHealth = Game.GameTime;
            }
        }
        private void Setup()
        {
            HandBoneID = 57005;
            MouthBoneID = 17188;

            string AnimBase;
            string AnimBaseDictionary;
            string AnimEnter;
            string AnimEnterDictionary;
            string AnimExit;
            string AnimExitDictionary;
            List<string> AnimIdle;
            string AnimIdleDictionary;
            Duration = 120000;
            
            string HandBoneName = "BONETAG_R_PH_HAND";
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;


            string MouthBoneName = "BONETAG_HEAD";
            Vector3 MouthOffset = Vector3.Zero;
            Rotator MouthRotator = Rotator.Zero;


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
                }
                else
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                    AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                }
                AnimBase = "base";
                AnimEnter = "enter";
                AnimExit = "exit";
                MouthBoneID = 31086;
                //MouthOffset = new Vector3(-0.007f, 0.13f, 0.01f);
                //MouthRotator = new Rotator(0.0f, -175f, 91f);
            }
            else if (Player.IsMale)
            {
                if (RandomItems.RandomPercent(50))
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_b@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_b@idle_a";
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_b@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_b@exit";
                }
                else
                {
                    AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                    AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                    AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                    AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                }
                AnimBase = "base";
                AnimIdle = new List<string> { "idle_a", "idle_b", "idle_c" };
                AnimEnter = "enter";
                AnimExit = "exit";
                MouthBoneID = 17188;
                //MouthOffset = new Vector3(0.046f, 0.015f, 0.014f);
                //MouthRotator = new Rotator(0.0f, -180f, 0f);
                //MouthOffset = new Vector3(-0.007f, 0.13f, 0.01f);
                //MouthRotator = new Rotator(0.0f, -175f, 91f);
            }
            else
            {
                //isFemalePed = true;
                AnimBaseDictionary = "amb@world_human_smoking@female@idle_a";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_smoking@female@idle_a";
                AnimIdle = new List<string> { "idle_c" };//"idle_a", "idle_b", these are kinda bad
                AnimEnterDictionary = "amb@world_human_smoking@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_smoking@female@exit";
                AnimExit = "exit";
                MouthBoneID = 17188;
                //MouthOffset = new Vector3(-0.007f, 0.13f, 0.01f);
                //MouthRotator = new Rotator(0.0f, -175f, 91f);
                //MouthOffset = new Vector3(-0.02f, 0.1f, 0.01f);
                //MouthRotator = new Rotator(0f, 0f, -80f);
            }
            if (Player.ActivityManager.IsSitting)
            {
                HasLightingAnimation = false;
                AnimBaseDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                AnimBase = "enter";
                AnimIdleDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                AnimIdle = new List<string> { "idle_a" };//"idle_a", "idle_b", these are kinda bad
                AnimEnterDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";///amb@code_human_in_car_mp_actions@smoke@std@ps@base idle_c
                AnimExit = "exit";
                HasLightingAnimation = false;

            }
            else if (Player.IsInVehicle)
            {
                if (Player.IsDriver)
                {
                    HasLightingAnimation = false;
                    AnimBaseDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ds@base";
                    AnimBase = "enter";
                    AnimIdleDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ds@base";
                    AnimIdle = new List<string> { "idle_a" };//"idle_a", "idle_b", these are kinda bad
                    AnimEnterDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ds@base";
                    AnimEnter = "enter";
                    AnimExitDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ds@base";///amb@code_human_in_car_mp_actions@smoke@std@ps@base idle_c
                    AnimExit = "exit";
                    HasLightingAnimation = false;
                }
                else
                {
                    HasLightingAnimation = false;
                    AnimBaseDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                    AnimBase = "enter";
                    AnimIdleDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                    AnimIdle = new List<string> { "idle_a" };//"idle_a", "idle_b", these are kinda bad
                    AnimEnterDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";
                    AnimEnter = "enter";
                    AnimExitDictionary = "amb@code_human_in_car_mp_actions@smoke@std@ps@base";///amb@code_human_in_car_mp_actions@smoke@std@ps@base idle_c
                    AnimExit = "exit";
                    HasLightingAnimation = false;
                }
            }
            ParticleOffset = new Vector3(-0.07f, 0.0f, 0f);
            ParticleRotation = Rotator.Zero;
            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                Duration = SmokeItem.Duration;
                if(Duration == 0)
                {
                    Duration = 120000;
                }


                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender) && x.IsMP == Player.CharacterModelIsFreeMode);
                if (pa == null)
                {
                    pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                }
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                    //EntryPoint.WriteToConsoleTestLong($"Smoking Activity Found Attachment (Hand) {HandOffset} {HandRotator} {HandBoneName}");
                }
                PropAttachment pa2 = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "Head" && (x.Gender == "U" || x.Gender == Player.Gender) && x.IsMP == Player.CharacterModelIsFreeMode);
                if(pa2 == null)
                {
                    pa2 = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "Head" && (x.Gender == "U" || x.Gender == Player.Gender));
                }
                if (pa2 != null)
                {
                    MouthOffset = pa2.Attachment;
                    MouthRotator = pa2.Rotation;
                    MouthBoneName = pa2.BoneName;
                    //EntryPoint.WriteToConsoleTestLong($"Smoking Activity Found Attachment (Mouth) {MouthOffset} {MouthRotator} {MouthBoneName}");
                }
                PropAttachment pa3 = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "Particle");
                if (pa3 != null)
                {
                    ParticleOffset = pa3.Attachment;
                    ParticleRotation = pa3.Rotation;
                    //EntryPoint.WriteToConsoleTestLong($"Smoking Activity Found Attachment (Mouth) {MouthOffset} {MouthRotator} {MouthBoneName}");
                }
            }
            if (SmokeItem != null && SmokeItem.IsIntoxicating && SmokeItem.Intoxicant != null)
            {
                CurrentIntoxicant = SmokeItem.Intoxicant;
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
            if (Player.ActivityManager.IsSitting || Player.IsInVehicle)
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
            if (Player.ActivityManager.IsSitting || Player.IsInVehicle)
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
                        Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", SmokedItem, ParticleOffset, ParticleRotation, 1.5f);
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
                            Smoke = new LoopedParticle("core", "ent_anim_cig_smoke", SmokedItem, ParticleOffset, ParticleRotation, 1.5f);
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