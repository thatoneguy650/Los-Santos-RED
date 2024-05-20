using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class RadarDetectorActivity : DynamicActivity
    {
        private IActionable Player;
        private ISettingsProvideable Settings;

        private bool IsCancelled;
        private bool IsAttachedToHand;
        private bool IsAttachedToBelt;
        private string PlayingDictionary = "";
        private string PlayingAnimation = "";
        private float CurrentAnimationTime = 0.0f;
        private bool hasStartedAnimation;

        private RadarDetectorItem RadioItem;
        private Rage.Object rageObject;
        private string PropModelName = "prop_binoc_01";

        private string HandBoneName = "BONETAG_R_PH_HAND";
        private string animTakeOutDictionary;
        private string animTakeOut;
        private string animPutAwayDictionary;
        private string animPutAway;
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private Vector3 InitialDirection;

        private string animLowerDictionary;
        private string animLower;
        private float animLowerBlendIn = 2.0f;
        private float animLowerBlendOut = -2.0f;
        private int animLowerFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private string animBaseDictionary;
        private string animBase;
        private string animLowWalking;
        private string animLowRunning;
        private float animLowBlendIn = 2.0f;
        private float animLowBlendOut = -2.0f;
        private int animLowFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private string animRaiseDictionary;
        private string animRaise;
        private float animRaiseBlendIn = 2.0f;
        private float animRaiseBlendOut = -2.0f;
        private int animRaiseFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private List<Tuple<string, string>> animRaisedIdles;
        private float animRaisedIdleBlendIn = 1.0f;
        private float animRaisedIdleBlendOut = -1.0f;
        private int animRaisedIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);



        private bool IsRaised = false;
        private bool IsRaising = false;
        private bool IsLowering = false;

        private string BeltBoneName;
        private Vector3 BeltOffset;
        private Rotator BeltRotator;
        private bool ShouldContinue;
        private bool isPaused;

        public RadarDetectorActivity(IActionable player, ISettingsProvideable settings, RadarDetectorItem binocularItem) : base()
        {
            Player = player;
            Settings = settings;
            RadioItem = binocularItem;
            ModItem = binocularItem;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Activity";
        public override string CancelPrompt { get; set; } = "Put Away RadarDetector";
        public override string ContinuePrompt { get; set; } = "Continue Activie";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
           // Player.ActivityManager.HasScannerOut = false;
        }
        public override void Pause()
        {
            //Player.ActivityManager.IsPerformingActivity = false;
            //Player.ActivityManager.AddPausedActivity(this);
            //RemovePrompts();
            //if (IsRaised)
            //{
            //    LowerRadio(false);
            //}
            //PutAwayItem();
            //AttachItemToBelt();
            //Dispose(false);
            //isPaused = true;
        }
        public override bool IsPaused() => isPaused;
        public override void Continue()
        {
            //Player.ActivityManager.PausedActivites.Remove(this);
            //Player.ActivityManager.PausedActivites.RemoveAll(x => x.ModItem != null && ModItem != null && x.ModItem.Name == ModItem.Name);
            //isPaused = false;
            //Start();
        }
        public override void Start()
        {
            //EntryPoint.WriteToConsole($"Radio Start");
            GameFiber BinocWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    if (!IsCancelled)
                    {
                        AttachItemToHand();
                        TakeOutItem();
                        AddPrompts();
                        StartGeneralIdle();
                        Tick();
                        Exit();
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "RadarDetectorWatcher");
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
        private void Tick()
        {
            while (Player.ActivityManager.CanPerformActivitiesMiddle && !IsCancelled && !isPaused)
            {
                GeneralTick();
                InputTick();
                GameFiber.Yield();
            }
        }
        private void Setup()
        {
            HandBoneName = "BONETAG_R_PH_HAND";
            BeltBoneName = "BONETAG_PELVIS";

            animTakeOutDictionary = "cellphone@";
            animTakeOut = "cellphone_text_in";

            animBaseDictionary = "cellphone@";
            animBase = "cellphone_text_read_base";

            animRaiseDictionary = "cellphone@";
            animRaise = "cellphone_text_to_call";//

            animLowerDictionary = "cellphone@";
            animLower = "cellphone_call_to_text";

            animPutAwayDictionary = "cellphone@";
            animPutAway = "cellphone_text_out";

            if (RadioItem != null && RadioItem.ModelItem != null)
            {
                PropModelName = RadioItem.ModelItem.ModelName;
                PropAttachment handAttachment = RadioItem.ModelItem.Attachments.Where(x => x.Name == "RightHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }


                PropAttachment beltAttachment = RadioItem.ModelItem.Attachments.Where(x => x.Name == "Belt").FirstOrDefault();
                if (beltAttachment != null)
                {
                    BeltBoneName = beltAttachment.BoneName;
                    BeltOffset = beltAttachment.Attachment;
                    BeltRotator = beltAttachment.Rotation;
                }

            }
            AnimationDictionary.RequestAnimationDictionay(animTakeOutDictionary);
            AnimationDictionary.RequestAnimationDictionay(animPutAwayDictionary);
            AnimationDictionary.RequestAnimationDictionay(animBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(animRaiseDictionary);
            AnimationDictionary.RequestAnimationDictionay(animLowerDictionary);

            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
        }
        private void Exit()
        {
            if (!isPaused)
            {
                if (!IsCancelled)
                {
                    PutAwayItem();
                }
                Dispose(true);
            }
        }
        private void StartGeneralIdle()
        {
            //EntryPoint.WriteToConsoleTestLong("Radio Start General Idle");
            PlayingDictionary = animBaseDictionary;
            PlayingAnimation = animBase;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowBlendIn, animLowBlendOut, -1, animLowFlag, 0, false, false, false);//-1      
        }
        private void GeneralTick()
        {
            if (!isPaused)
            {
                Player.WeaponEquipment.SetUnarmed();
                CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
            }
        }
        private void StatusTick()
        {
            if (!isPaused)
            {
                CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
            }
        }
        private void InputTick()
        {
            if (isPaused)
            {
                RemovePrompts();
            }
            else
            {
                DisableControls();
                if (Player.IsShowingActionWheel)
                {
                    return;
                }
                if (Player.ButtonPrompts.IsPressed("RadarDetectorEnable"))
                {
                    RemovePrompts();
                    Player.RadarDetector.SetState(true);
                    Game.DisplaySubtitle("Radar Detector Enabled");
                    GameFiber.Sleep(1000);
                    AddPrompts();
                    //LowerRadio(true);
                }
                if (Player.ButtonPrompts.IsPressed("RadarDetectorDisable"))
                {
                    RemovePrompts();
                    Player.RadarDetector.SetState(false);
                    Game.DisplaySubtitle("Radar Detector Disabled");
                    GameFiber.Sleep(1000);
                    AddPrompts();
                    //RaiseRadio();
                }
            }
        }
        private void AttachItemToHand()
        {
            CreateRadio();
            if (rageObject.Exists())
            {
                rageObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                IsAttachedToBelt = false;
                Player.AttachedProp.Add(rageObject);
                InitialDirection = rageObject.Direction;
            }
        }
        private void CreateRadio()
        {
            if (!rageObject.Exists() && PropModelName != "")
            {
                try
                {
                    rageObject = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!rageObject.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void AddPrompts()
        {
            if (IsAttachedToHand)
            {
                if (!Player.RadarDetector.IsTurnedOn)
                {
                    Player.ButtonPrompts.AddPrompt("RadarDetector", "Enable", "RadarDetectorEnable", GameControl.Attack, 15);
                }
                else
                {
                    Player.ButtonPrompts.AddPrompt("RadarDetector", "Disable", "RadarDetectorDisable", GameControl.Attack, 15);
                }
            }
        }
        private void RemovePrompts()
        {
            Player.ButtonPrompts.RemovePrompts("RadarDetector");
        }
        private void DisableControls()
        {
            Game.DisableControlAction(0, GameControl.Attack, true);// false);
            Game.DisableControlAction(0, GameControl.Attack2, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);// false);


            Game.DisableControlAction(0, GameControl.Aim, true);// false);
            Game.DisableControlAction(0, GameControl.VehicleAim, true);// false);
            Game.DisableControlAction(0, GameControl.AccurateAim, true);// false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);// false);


            Game.DisableControlAction(0, GameControl.VehicleAttack, true);// false);
            Game.DisableControlAction(0, GameControl.VehicleAttack2, true);// false);

            Game.DisableControlAction(0, GameControl.WeaponWheelNext, true);// false);
            Game.DisableControlAction(0, GameControl.WeaponWheelPrev, true);// false);

            Game.DisableControlAction(0, GameControl.WeaponWheelUpDown, true);// false);
            Game.DisableControlAction(0, GameControl.WeaponWheelLeftRight, true);// false);


            Game.DisableControlAction(0, GameControl.SelectWeapon, true);// false);

            Game.DisableControlAction(0, GameControl.SelectNextWeapon, true);// false);
            Game.DisableControlAction(0, GameControl.SelectPrevWeapon, true);// false);


            Game.DisableControlAction(0, GameControl.NextWeapon, true);// false);
            Game.DisableControlAction(0, GameControl.PrevWeapon, true);// false);
        }
        private void Dispose(bool deleteObject)
        {
            //if (Settings.SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem)
            //{
            //    Player.Scanner.Abort();
            //}
            RemovePrompts();
            if (rageObject.Exists() && deleteObject)
            {
                //Player.ActivityManager.HasScannerOut = false;
                rageObject.Delete();
            }
            //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;

        }
        private void TakeOutItem()
        {
            if (!IsCancelled)
            {
                //EntryPoint.WriteToConsoleTestLong("Take Out Radio Start");
                RemovePrompts();
                PlayingDictionary = animTakeOutDictionary;
                PlayingAnimation = animTakeOut;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                AnimationWatcher aw = new AnimationWatcher();
                while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    if (!aw.IsAnimationRunning(CurrentAnimationTime))
                    {
                        //EntryPoint.WriteToConsoleTestLong($"Take Out Radio Error, Animation Not Running {CurrentAnimationTime}");
                        break;
                    }
                    GameFiber.Yield();
                }
                //Player.Scanner.ScannerBoostLevel = 1;
                //Player.ActivityManager.HasScannerOut = true;
                //EntryPoint.WriteToConsoleTestLong("Take Out Radio End");
            }
        }
        private void PutAwayItem()
        {
            if (!IsCancelled)
            {
                //EntryPoint.WriteToConsoleTestLong("Put Away Radio Start");
                RemovePrompts();
                PlayingDictionary = animPutAwayDictionary;
                PlayingAnimation = animPutAway;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                AnimationWatcher aw = new AnimationWatcher();
                while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    if (!aw.IsAnimationRunning(CurrentAnimationTime))
                    {
                        //EntryPoint.WriteToConsoleTestLong("Put Away Radio Error, Animation Not Running {CurrentAnimationTime}");
                        break;
                    }
                    GameFiber.Yield();
                }
               // Player.Scanner.ScannerBoostLevel = 0;
                //EntryPoint.WriteToConsoleTestLong("Put Away Radio End");
            }
        }
        //private void RaiseRadio()
        //{
        //    if (!IsCancelled)
        //    {
        //        //EntryPoint.WriteToConsoleTestLong("Raise Radio Start");
        //        IsRaising = true;
        //        IsLowering = false;
        //        RemovePrompts();
        //        PlayingDictionary = animRaiseDictionary;
        //        PlayingAnimation = animRaise;
        //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
        //        AnimationWatcher aw = new AnimationWatcher();
        //        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && CurrentAnimationTime < 1.0f)
        //        {
        //            DisableControls();
        //            GeneralTick();
        //            if (!aw.IsAnimationRunning(CurrentAnimationTime))
        //            {
        //                //EntryPoint.WriteToConsoleTestLong($"Raise Radio Error, Animation Not Running {CurrentAnimationTime}");
        //                break;
        //            }
        //            GameFiber.Yield();
        //        }
        //        IsRaised = true;
        //        IsRaising = false;
        //        //Player.Scanner.ScannerBoostLevel = 2;
        //        AddPrompts();
        //        //EntryPoint.WriteToConsoleTestLong("Raise Radio End");
        //    }
        //}
        //private void LowerRadio(bool restartIdle)
        //{
        //    if (!IsCancelled)
        //    {
        //        //EntryPoint.WriteToConsoleTestLong("Lower Radio Start");
        //        IsLowering = true;
        //        IsRaising = false;
        //        RemovePrompts();
        //        PlayingDictionary = animLowerDictionary;
        //        PlayingAnimation = animLower;
        //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowerBlendIn, animLowerBlendOut, -1, animLowerFlag, 0, false, false, false);//-1
        //        AnimationWatcher aw = new AnimationWatcher();
        //        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && CurrentAnimationTime < 1.0f)
        //        {
        //            DisableControls();
        //            GeneralTick();
        //            if (!aw.IsAnimationRunning(CurrentAnimationTime))
        //            {
        //                //EntryPoint.WriteToConsoleTestLong($"Lower Radio Error, Animation Not Running {CurrentAnimationTime}");
        //                break;
        //            }
        //            GameFiber.Yield();
        //        }
        //        IsRaised = false;
        //        IsLowering = false;
        //        //Player.Scanner.ScannerBoostLevel = 1;
        //        if (restartIdle)
        //        {
        //            StartGeneralIdle();
        //            AddPrompts();
        //        }
        //        //EntryPoint.WriteToConsoleTestLong("Lower Radio End");
        //    }
        //}



    }
}