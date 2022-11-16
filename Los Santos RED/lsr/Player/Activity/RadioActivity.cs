using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class RadioActivity : DynamicActivity
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

        private RadioItem RadioItem;
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

        public RadioActivity(IActionable player, ISettingsProvideable settings, RadioItem binocularItem) : base()
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
        public override string PausePrompt { get; set; } = "Pause Radio";
        public override string CancelPrompt { get; set; } = "Put Away Radio";
        public override string ContinuePrompt { get; set; } = "Continue Radio";


        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
        }
        public override void Pause()
        {

        }
        public override bool IsPaused() => false;
        public override void Continue()
        {

        }
        public override void Start()
        {
            EntryPoint.WriteToConsole($"Radio Start", 5);
            GameFiber BinocWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "RadioWatcher");
        }
        private void Enter()
        {
            Setup();
            if (!IsCancelled)
            {
                AttachItemToHand();
                TakeOutItem();
                Tick();
            }
        }
        private void Tick()
        {
            AddPrompts();
            StartGeneralIdle();
            while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
            {
                GeneralTick();
                StatusTick();
                InputTick();
                GameFiber.Yield();
            }
            Exit();
        }
        private void TakeOutItem()
        {
            if (!IsCancelled)
            {
                EntryPoint.WriteToConsole("Take Out Radio Start");
                RemovePrompts();
                PlayingDictionary = animTakeOutDictionary;
                PlayingAnimation = animTakeOut;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole("Take Out Radio End");
            }
        }
        private void PutAwayItem()
        {
            if (!IsCancelled)
            {
                EntryPoint.WriteToConsole("Put Away Radio Start");
                RemovePrompts();
                PlayingDictionary = animPutAwayDictionary;
                PlayingAnimation = animPutAway;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole("Put Away Radio End");
            }
        }
        private void RaiseRadio()
        {
            if (!IsCancelled)
            {
                EntryPoint.WriteToConsole("Raise Radio Start");
                IsRaising = true;
                IsLowering = false;
                RemovePrompts();
                PlayingDictionary = animRaiseDictionary;
                PlayingAnimation = animRaise;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                IsRaised = true;
                IsRaising = false;
                AddPrompts();
                EntryPoint.WriteToConsole("Raise Radio End");
            }
        }
        private void LowerRadio(bool restartIdle)
        {
            if (!IsCancelled)
            {
                EntryPoint.WriteToConsole("Lower Radio Start");
                IsLowering = true;
                IsRaising = false;
                RemovePrompts();
                PlayingDictionary = animLowerDictionary;
                PlayingAnimation = animLower;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowerBlendIn, animLowerBlendOut, -1, animLowerFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                IsRaised = false;
                IsLowering = false;
                if (restartIdle)
                {
                    StartGeneralIdle();
                    AddPrompts();
                }
                EntryPoint.WriteToConsole("Lower Radio End");
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
            if (!IsCancelled && IsRaised)
            {
                LowerRadio(false);
            }
            if(!IsCancelled)
            {
                PutAwayItem();
            }
            Dispose(true);
        }
        private void StartGeneralIdle()
        {
            EntryPoint.WriteToConsole("Radio Start General Idle");
            PlayingDictionary = animBaseDictionary;
            PlayingAnimation = animBase;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowBlendIn, animLowBlendOut, -1, animLowFlag, 0, false, false, false);//-1      
        }
        private void GeneralTick()
        {
            Player.WeaponEquipment.SetUnarmed();
            CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
        }
        private void StatusTick()
        {
            CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
        }
        private void InputTick()
        {
            DisableControls();
            if (Player.ButtonPrompts.IsPressed("RadioLower"))
            {
                LowerRadio(true);
            }
            if (Player.ButtonPrompts.IsPressed("RadioRaise"))
            {
                RaiseRadio();
            }
            if (Player.ButtonPrompts.IsPressed("RadioAttach"))
            {
                RemovePrompts();
                PutAwayItem();
                AttachItemToBelt();


                Player.ActivityManager.AttachedItems.Add(new AttachedItem(RadioItem, rageObject));
                Dispose(false);

                //AddPrompts();
            }
            if(Player.ButtonPrompts.IsPressed("RadioDetach"))
            {
                RemovePrompts();
                AttachItemToHand();
                TakeOutItem();
                AddPrompts();
            }
        }

        private void AttachItemToHand()
        {
            CreateBinoculars();
            if (rageObject.Exists() && !IsAttachedToHand)
            {         
                rageObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                IsAttachedToBelt = false;
                Player.AttachedProp.Add(rageObject);
                InitialDirection = rageObject.Direction;
            }
        }
        private void AttachItemToBelt()
        {
            if (rageObject.Exists() && !IsAttachedToBelt)
            {
                rageObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, BeltBoneName), BeltOffset, BeltRotator);
                IsAttachedToHand = false;
                IsAttachedToBelt = true;
                Player.AttachedProp.Add(rageObject);

            }
        }
        private void CreateBinoculars()
        {
            if (!rageObject.Exists() && PropModelName != "")
            {
                try
                {
                    rageObject = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
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
                if (IsRaised)
                {
                    Player.ButtonPrompts.AddPrompt("Radio", "Lower", "RadioLower", GameControl.Aim, 15);
                }
                else
                {
                    Player.ButtonPrompts.AddPrompt("Radio", "Raise", "RadioRaise", GameControl.Aim, 15);
                }
            }

#if DEBUG

            if (IsAttachedToHand && !IsRaised)
            {
                Player.ButtonPrompts.AddPrompt("Radio", "Attach", "RadioAttach", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 12);
            }
            else if(IsAttachedToBelt)
            {
                Player.ButtonPrompts.AddPrompt("Radio", "Detach", "RadioDetach", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 12);
            }
#endif
        }
        private void RemovePrompts()
        {
            Player.ButtonPrompts.RemovePrompts("Radio");
        }
        private void DisableControls()
        {
            Game.DisableControlAction(0, GameControl.Attack, false);
            Game.DisableControlAction(0, GameControl.Attack2, false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, false);


            Game.DisableControlAction(0, GameControl.Aim, false);
            Game.DisableControlAction(0, GameControl.VehicleAim, false);
            Game.DisableControlAction(0, GameControl.AccurateAim, false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, false);


            Game.DisableControlAction(0, GameControl.WeaponWheelNext, false);
            Game.DisableControlAction(0, GameControl.WeaponWheelPrev, false);

            Game.DisableControlAction(0, GameControl.WeaponWheelUpDown, false);
            Game.DisableControlAction(0, GameControl.WeaponWheelLeftRight, false);


            Game.DisableControlAction(0, GameControl.SelectWeapon, false);

            Game.DisableControlAction(0, GameControl.SelectNextWeapon, false);
            Game.DisableControlAction(0, GameControl.SelectPrevWeapon, false);


            Game.DisableControlAction(0, GameControl.NextWeapon, false);
            Game.DisableControlAction(0, GameControl.PrevWeapon, false);
        }

        private void Dispose(bool deleteObject)
        {
            if (Settings.SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem)
            {
                Player.Scanner.Abort();
            }
            RemovePrompts();
            if (rageObject.Exists() && deleteObject)
            {
                rageObject.Delete();
            }
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
        }

    }
}