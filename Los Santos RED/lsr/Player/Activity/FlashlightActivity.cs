using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LosSantosRED.lsr.Player
{
    public class FlashlightActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private uint GameTimeStartedHoldingUmbrella;
        private string animBaseDictionary;
        private string animEnterDictionary;
        private string animEnter;
        private string animBase;
        private string animExitDictionary;
        private string animExit;
        private float FakeEmissiveRadius;
        private float FakeEmissiveFallOff;
        private float EmissiveDistance;
        private float EmissiveBrightness;
        private float EmissiveHardness;
        private float EmissiveRadius;
        private float EmissiveFallOff;
        private bool LightFollowsCamera;
        private bool AllowPropRotation;
        private bool UseFakeEmissive;
        private float PitchMax;
        private float PitchMin;
        private float HeadingMax;
        private float HeadingMin;
        private float FakeEmissiveDistance;
        private float FakeEmissiveBrightness;
        private float FakeEmissiveHardness;
        private List<Tuple<string,string>> animIdles;
        private int animEnterFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animBaseFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);


        private int animExitFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private float animEnterBlendIn = 8.0f;
        private float animBaseBlendIn = 8.0f;
        private float animIdleBlendIn = 8.0f;
        private float animExitBlendIn = 8.0f;
        private float animEnterBlendOut = -8.0f;
        private float animBaseBlendOut = -8.0f;
        private float animIdleBlendOut = -8.0f;
        private float animExitBlendOut = -4.0f;
        private Rage.Object Flashlight;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "prop_cs_police_torch";
        private Vector3 InitialDirection;
        private Rotator LightRotator;
        private Vector3 LightDirection;
        private float GameplayCameraCurrentPitch;
        private float GameplayCameraCurrentHeading;
        private bool IsLightOn = true;
        private LoopedParticle Light;

        private string PlayingDictionary = "";
        private string PlayingAnimation = "";
        private Vector3 FlashlightOrigin;
        private Vector3 FlashlightOriginExtended;
        private bool IsSearching;
        private Vector3 FakeEmissiveDirection;
        private bool isSearching;
        private bool isLightOn;

        private float ExtraDistanceX;
        private float ExtraDistanceY;
        private float ExtraDistanceZ;
        private float FakeEmissiveExtraDistanceX;
        private float FakeEmissiveExtraDistanceY;
        private float FakeEmissiveExtraDistanceZ;
        private float ExtraRotation;
        private float NonCameraExtra;
        private float PitchModifier;

        private FlashlightItem FlashlightItem;
        private bool hasStartedAnimation;

        public FlashlightActivity(IActionable player, ISettingsProvideable settings, FlashlightItem flashlightItem) : base()
        {
            Player = player;
            Settings = settings;
            FlashlightItem = flashlightItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Flashlight";
        public override string CancelPrompt { get; set; } = "Put Away Flashlight";
        public override string ContinuePrompt { get; set; } = "Continue Flashlight";
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
           // EntryPoint.WriteToConsole($"Flashlight Start");
            GameFiber FlashlightWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    Enter();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "FlashlightActivity");
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }







        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            
            if (animEnter != "")
            {
                AttachFlashlightToHand();
                //EntryPoint.WriteToConsole($"Flashlight Enter: {animEnter}");
                PlayingDictionary = animEnterDictionary;
                PlayingAnimation = animEnter;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animEnterDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animEnterDictionary, animEnter);
                    if (AnimationTime >= 1.0f)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
            }
            Idle();
        }
        private void Idle()
        {
            if (animBase != "")
            {
                AttachFlashlightToHand();
                //EntryPoint.WriteToConsole($"Flashlight Idle: {animBase}");
                GameTimeStartedHoldingUmbrella = Game.GameTime;

                PlayingDictionary = animBaseDictionary;
                PlayingAnimation = animBase;
                hasStartedAnimation = false;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animBaseDictionary, animBase, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1

                Player.ButtonPrompts.AddPrompt("Flashlight", "Toggle Light", "FlashlightToggle", GameControl.Attack, 10);

                if (FlashlightItem.CanSearch)
                {
                    Player.ButtonPrompts.AddPrompt("Flashlight", "Search", "FlashlightPlayAnimation", GameControl.Aim, 12);
                }


                while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled)
                {
                    if(Player.IsVisiblyArmed)
                    {
                        IsCancelled = true;
                    }
                    if(isSearching != IsSearching)
                    {
                        if (IsSearching)
                        {
                            Player.ButtonPrompts.RemovePrompt("FlashlightPlayAnimation");
                            Player.ButtonPrompts.AddPrompt("Flashlight", "Stop Search", "FlashlightStopAnimation", GameControl.Aim, 12);
                        }
                        else
                        {
                            Player.ButtonPrompts.RemovePrompt("FlashlightStopAnimation");
                            Player.ButtonPrompts.AddPrompt("Flashlight", "Search", "FlashlightPlayAnimation", GameControl.Aim, 12);
                        }
                        isSearching = IsSearching;
                    }

                    if(isLightOn != IsLightOn)
                    {
                        if (IsLightOn)
                        {
                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Press", "DLC_SECURITY_BUTTON_PRESS_SOUNDS", 0);
                        }
                        else
                        {
                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Press", "DLC_SECURITY_BUTTON_PRESS_SOUNDS", 0);
                        }
                        isLightOn = IsLightOn;
                    }

                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
                    if (AnimationTime >= 1.0f)
                    {
                        IsSearching = false;
                    }
                    if (AnimationTime > 0.0f)
                    {
                        hasStartedAnimation = true;
                    }
                    if (AnimationTime == 0.0f && hasStartedAnimation)
                    {
                        IsCancelled = true;
                    }
                    HandleLight();
                    HandleButtons();
                    GameFiber.Yield();
                }

                Player.ButtonPrompts.RemovePrompts("Flashlight");

            }
            Exit();
        }
        private void Exit()
        {
            try
            {
                if (animExit != "")
                {
                    //EntryPoint.WriteToConsole($"Flashlight Exit: {animExit}");
                    GameTimeStartedHoldingUmbrella = Game.GameTime;
                    PlayingDictionary = animExitDictionary;
                    PlayingAnimation = animExit;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animExitDictionary, animExit, animExitBlendIn, animExitBlendOut, -1, animExitFlag, 0, false, false, false);//-1
                    while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled)
                    {
                        Player.WeaponEquipment.SetUnarmed();
                        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animExitDictionary, animBase);
                        if(AnimationTime >= 1.0f)
                        {
                            break;
                        }
                        GameFiber.Yield();
                    }
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                //Need to Delete the umbrella too
            }
            catch
            {
                Game.DisplayNotification("FAIL");
            }
            //if (1 == 1)
            //{
            //    GameFiber.Sleep(500);
            //}
            if (Flashlight.Exists())
            {
                Flashlight.Delete();
            }
            Player.ButtonPrompts.RemovePrompts("Flashlight");
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;

            //if (Flashlight.Exists())
            //{
            //    Flashlight.Detach();
            //}
            //if (Flashlight.Exists())
            //{
            //    Flashlight.Delete();
            //}
        }
        private void HandleButtons()
        {

            Game.DisableControlAction(0, GameControl.Attack, true);// false);
            Game.DisableControlAction(0, GameControl.Attack2, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);// false);


            Game.DisableControlAction(0, GameControl.Aim, true);// false);
            Game.DisableControlAction(0, GameControl.VehicleAim, true);// false);
            Game.DisableControlAction(0, GameControl.AccurateAim, true);// false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);// false);

            if (Player.IsShowingActionWheel)
            {
                return;
            }

            if (Player.ButtonPrompts.IsPressed("FlashlightToggle"))
            {
                IsLightOn = !IsLightOn;
                //EntryPoint.WriteToConsole($"FlashlightToggle Toggled to {IsLightOn}");
            }
            if (Player.ButtonPrompts.IsPressed("FlashlightPlayAnimation"))
            {
                if (animIdles.Any())
                {
                    Tuple<string, string> idlePicked = animIdles.PickRandom();
                    if (idlePicked != null)
                    {
                        IsSearching = true;
                        StartIdleAnimation(idlePicked.Item1, idlePicked.Item2);
                    }
                }

                //EntryPoint.WriteToConsole($"FlashlightPlayAnimation");
            }
            if (Player.ButtonPrompts.IsPressed("FlashlightStopAnimation"))
            {
                IsSearching = false;
                StartBaseAnimation();
            } 

            //EntryPoint.WriteToConsole($"FlashlightPlayAnimation");
        }
        private void StartBaseAnimation()
        {
            hasStartedAnimation = false;
            PlayingDictionary = animBaseDictionary;
            PlayingAnimation = animBase;
            //EntryPoint.WriteToConsole($"FlashlightPlayAnimation SET BASE {PlayingDictionary} {PlayingAnimation}");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1
        }
        private void StartIdleAnimation(string dict, string anim)
        {
            hasStartedAnimation = false;
            PlayingDictionary = dict;
            PlayingAnimation = anim;
            //EntryPoint.WriteToConsole($"FlashlightPlayAnimation New Idle {PlayingDictionary} {PlayingAnimation}");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1
        }
        private void HandleLight()
        {          
            if (Flashlight.Exists())
            {
                GetLightDirection();
                UpdateFlashlightPosition();
                DrawLights();
            }
        }
        private void AttachFlashlightToHand()
        {
            CreateFlashlight();
            if (Flashlight.Exists() && !IsAttachedToHand)
            {
                Flashlight.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Flashlight);
                InitialDirection = Flashlight.Direction;
            }
        }
        private void UpdateFlashlightPosition()
        {

            if (Flashlight.Exists() && IsAttachedToHand)
            {
                if (AllowPropRotation && LightFollowsCamera)
                {
                    Flashlight.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, LightRotator);
                }
            }
        }
        private void CreateFlashlight()
        {
            if (!Flashlight.Exists() && PropModelName != "")
            {
                try
                {
                    Flashlight = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Flashlight.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void Setup()
        {
            HandBoneName = "BONETAG_L_PH_HAND";

            animEnterDictionary = "amb@world_human_security_shine_torch@male@enter";
            animEnter = "enter";
            animBaseDictionary = "amb@world_human_security_shine_torch@male@base";
            animBase = "base";
            animIdles = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_a"),//Right to left above and below
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_b"),//does a box, faster
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_c"),//Does a right to left mostly down
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_b","idle_d"),//Does a box around, mostly above
                    new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_b","idle_e"),// right to left in middle
                };
            animExitDictionary = "amb@world_human_security_shine_torch@male@exit";
            animExit = "exit";


            animBaseBlendIn = 1.0f;
            animBaseBlendOut = -1.0f;

            animIdleBlendIn = 1.0f;
            animIdleBlendOut = -1.0f;

            animExitBlendIn = 1.0f;
            animExitBlendOut = -1.0f;

            animEnterBlendIn = 1.0f;
            animEnterBlendOut = -1.0f;




            EmissiveDistance = Settings.SettingsManager.FlashlightSettings.EmissiveDistance;
            EmissiveBrightness = Settings.SettingsManager.FlashlightSettings.EmissiveBrightness;
            EmissiveHardness = Settings.SettingsManager.FlashlightSettings.EmissiveHardness;
            EmissiveRadius = Settings.SettingsManager.FlashlightSettings.EmissiveRadius;
            EmissiveFallOff = Settings.SettingsManager.FlashlightSettings.EmissiveFallOff;

            LightFollowsCamera = Settings.SettingsManager.FlashlightSettings.LightFollowsCamera;
            AllowPropRotation = Settings.SettingsManager.FlashlightSettings.AllowPropRotation;
            UseFakeEmissive = Settings.SettingsManager.FlashlightSettings.UseFakeEmissive;
            PitchMax = Settings.SettingsManager.FlashlightSettings.PitchMax;
            PitchMin = Settings.SettingsManager.FlashlightSettings.PitchMin;
            HeadingMax = Settings.SettingsManager.FlashlightSettings.HeadingMax;
            HeadingMin = Settings.SettingsManager.FlashlightSettings.HeadingMin;


            FakeEmissiveDistance = Settings.SettingsManager.FlashlightSettings.FakeEmissiveDistance;
            FakeEmissiveBrightness = Settings.SettingsManager.FlashlightSettings.FakeEmissiveBrightness;
            FakeEmissiveHardness = Settings.SettingsManager.FlashlightSettings.FakeEmissiveHardness;
            FakeEmissiveRadius = Settings.SettingsManager.FlashlightSettings.FakeEmissiveRadius;
            FakeEmissiveFallOff = Settings.SettingsManager.FlashlightSettings.FakeEmissiveFallOff;


            if (FlashlightItem != null && FlashlightItem.ModelItem != null)
            {
                PropModelName = FlashlightItem.ModelItem.ModelName;
                PropAttachment handAttachment = FlashlightItem.ModelItem.Attachments.Where(x => x.Name == "LeftHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }
                PropAttachment extraDistance = FlashlightItem.ModelItem.Attachments.Where(x => x.Name == "ExtraDistance").FirstOrDefault();
                if (extraDistance != null)
                {
                    ExtraDistanceX = extraDistance.Attachment.X;
                    ExtraDistanceY = extraDistance.Attachment.Y;
                    ExtraDistanceZ = extraDistance.Attachment.Z;
                }
                PropAttachment emissiveExtraDistance = FlashlightItem.ModelItem.Attachments.Where(x => x.Name == "EmissiveExtraDistance").FirstOrDefault();
                if (emissiveExtraDistance != null)
                {
                    FakeEmissiveExtraDistanceX = emissiveExtraDistance.Attachment.X;
                    FakeEmissiveExtraDistanceY = emissiveExtraDistance.Attachment.Y;
                    FakeEmissiveExtraDistanceZ = emissiveExtraDistance.Attachment.Z;
                }
                PropAttachment extraAttachment = FlashlightItem.ModelItem.Attachments.Where(x => x.Name == "FrontRotation").FirstOrDefault();
                if (extraAttachment != null)
                {
                    ExtraRotation = extraAttachment.Attachment.X;
                    PitchModifier = extraAttachment.Attachment.Y;
                    NonCameraExtra = extraAttachment.Attachment.Z;
                }


                if(FlashlightItem.IsCellphone)
                {
                    animEnterDictionary = "cellphone@";
                    animEnter = "cellphone_photo_ent";
                    animBaseDictionary = "cellphone@";
                    animBase = "cellphone_photo_idle";
                    animIdles = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("cellphone@","cellphone_photo_idle"),//Right to left above and below
                    };
                    animExitDictionary = "cellphone@";
                    animExit = "cellphone_photo_exit";
                }



                EmissiveDistance = FlashlightItem.EmissiveDistance;
                EmissiveBrightness = FlashlightItem.EmissiveBrightness;
                EmissiveHardness = FlashlightItem.EmissiveHardness;
                EmissiveRadius = FlashlightItem.EmissiveRadius;
                EmissiveFallOff = FlashlightItem.EmissiveFallOff;

                LightFollowsCamera = FlashlightItem.LightFollowsCamera;
                AllowPropRotation = FlashlightItem.AllowPropRotation;
                UseFakeEmissive = FlashlightItem.UseFakeEmissive;
                PitchMax = FlashlightItem.PitchMax;
                PitchMin = FlashlightItem.PitchMin;
                HeadingMax = FlashlightItem.HeadingMax;
                HeadingMin = FlashlightItem.HeadingMin;


                FakeEmissiveDistance = FlashlightItem.FakeEmissiveDistance;
                FakeEmissiveBrightness = FlashlightItem.FakeEmissiveBrightness;
                FakeEmissiveHardness = FlashlightItem.FakeEmissiveHardness;
                FakeEmissiveRadius = FlashlightItem.FakeEmissiveRadius;
                FakeEmissiveFallOff = FlashlightItem.FakeEmissiveFallOff;




            }
            else
            {
                if (PropModelName == "prop_tool_torch")
                {
                    HandOffset = new Vector3(0.12f, -0.02f, -0.08f);
                    HandRotator = new Rotator(0f, 0f, -100f);

                    ExtraDistanceX = 0.1f;
                    ExtraDistanceY = 0.35f;

                    FakeEmissiveExtraDistanceX = 0.0f;
                    FakeEmissiveExtraDistanceY = 0.1f;
                    FakeEmissiveExtraDistanceZ = 0.0f;
                    ExtraRotation = -90f;
                    PitchModifier = 1.0f;
                    NonCameraExtra = 1.0f;
                }
                else
                {
                    PropModelName = "prop_cs_police_torch";
                    HandOffset = new Vector3(0f, 0.002f, 0.002f);
                    HandRotator = new Rotator(-180f, -130f, -100f);

                    ExtraDistanceX = 0.0f;
                    ExtraDistanceY = -0.05f;

                    FakeEmissiveExtraDistanceX = -0.07f;
                    FakeEmissiveExtraDistanceY = -0.2f;
                    FakeEmissiveExtraDistanceZ = 0.0f;
                    ExtraRotation = 90f;
                    NonCameraExtra = -1.0f;
                    PitchModifier = -1.0f;
                }
            }






            /*
             * 
             * prop_phone_ing == michael (iphgone)
             * prop_phone_ing_02 == trevor (windows phone)
             * prop_phone_ing_03 == franklin (android)
             * cellphone@ cellphone_photo_ent
cellphone@ cellphone_photo_exit
cellphone@ cellphone_photo_idle*/


            AnimationDictionary.RequestAnimationDictionay(animEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(animBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(animExitDictionary);
            foreach(string idleDict in animIdles.GroupBy(x=> x.Item1).Select(y=> y.Key))
            {
                AnimationDictionary.RequestAnimationDictionay(idleDict);
            }
        }
        private void GetLightDirection()
        {

            GetFlashlightOffsets();

            LightDirection = Vector3.Zero;
            GameplayCameraCurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
            GameplayCameraCurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();


            bool FollowCamera = LightFollowsCamera;
            if(Settings.SettingsManager.FlashlightSettings.LightFollowsPropDuringSearch && IsSearching)
            {
                FollowCamera = false;
            }
            if (FollowCamera)
            {
                LightDirection = NativeHelper.GetGameplayCameraDirection().ToNormalized();
                LightDirection = new Vector3(LightDirection.X, LightDirection.Y, LightDirection.Z).ToNormalized();
                Rotator directionRotator = LightDirection.ToRotator();
                Rotator playerRotator = Game.LocalPlayer.Character.Direction.ToRotator();
                if (directionRotator.Pitch >= PitchMax)
                {
                    directionRotator.Pitch = PitchMax;
                }
                if (directionRotator.Pitch <= PitchMin)
                {
                    directionRotator.Pitch = PitchMin;
                }
                float HeadingDiff = Extensions.GetHeadingDifference(playerRotator.Yaw, directionRotator.Yaw);


                if (HeadingDiff >= 120f || HeadingDiff <= -120f)
                {
                    LightDirection = new Vector3(Flashlight.Direction.X, Flashlight.Direction.Y, Flashlight.Direction.Z).ToNormalized() * NonCameraExtra;
                    return;
                }


                if (HeadingDiff >= HeadingMax || HeadingDiff <= HeadingMin)
                {

                    if (HeadingDiff >= HeadingMax)
                    {
                        directionRotator.Yaw = playerRotator.Yaw + HeadingMax;
                    }
                    else if (HeadingDiff <= HeadingMin)
                    {
                        directionRotator.Yaw = playerRotator.Yaw + HeadingMin;
                    }
                }
                LightRotator = new Rotator(directionRotator.Pitch * PitchModifier, directionRotator.Roll, directionRotator.Yaw - playerRotator.Yaw + ExtraRotation).ToNormalized();
                //LightRotator = new Rotator(directionRotator.Pitch * -1.0f,directionRotator.Roll,directionRotator.Yaw - playerRotator.Yaw + FlashlightHeadingMax + FlashlightRotationExtra).ToNormalized();
                LightDirection = directionRotator.ToVector();

            }
            else
            {
                LightDirection = new Vector3(Flashlight.Direction.X, Flashlight.Direction.Y, Flashlight.Direction.Z).ToNormalized() * NonCameraExtra;
                //LightDirection = new Vector3(LightDirection.X, LightDirection.Y, Flashlight.Direction.Z).ToNormalized();
            }


           // Game.DisplaySubtitle($"{LightRotator} -- {Game.LocalPlayer.Character.Direction.ToRotator()}");
        }
        private void GetFlashlightOffsets()
        {
            Vector3 FlashlightFrontOffsetAmount = new Vector3(
                                                                Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceX + ExtraDistanceX - Flashlight.Model.Dimensions.X / 2,
                                                                Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceY + ExtraDistanceY - Flashlight.Model.Dimensions.Y / 2,
                                                                Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceZ + ExtraDistanceZ - 0);
            FlashlightOrigin = Flashlight.GetOffsetPosition(FlashlightFrontOffsetAmount);


            Vector3 FlashlightFrontOffsetAmountExtended = new Vector3(
                                                    Settings.SettingsManager.FlashlightSettings.DebugFakeEmissiveExtraDistanceX + FakeEmissiveExtraDistanceX + FlashlightFrontOffsetAmount.X,
                                                    Settings.SettingsManager.FlashlightSettings.DebugFakeEmissiveExtraDistanceY + FakeEmissiveExtraDistanceY + FlashlightFrontOffsetAmount.Y,
                                                    Settings.SettingsManager.FlashlightSettings.DebugFakeEmissiveExtraDistanceZ + FakeEmissiveExtraDistanceZ + FlashlightFrontOffsetAmount.Z);
            FlashlightOriginExtended = Flashlight.GetOffsetPosition(FlashlightFrontOffsetAmountExtended);
        }
        private void DrawLights()
        {
            if (IsLightOn)
            {

                NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, LightDirection.X, LightDirection.Y, LightDirection.Z, 255, 255, 255,
                        EmissiveDistance,
                        EmissiveBrightness,
                        EmissiveHardness,
                        EmissiveRadius,
                        EmissiveFallOff
                    );
                
                //NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, LightDirection.X, LightDirection.Y, LightDirection.Z, 255, 255, 255, 100f, 1.0f, 0.0f, 13.0f, 1.0f);
                if (UseFakeEmissive)
                {
                    if (Settings.SettingsManager.FlashlightSettings.ShowDebugMarkerAtEmissiveTip)
                    {
                        Rage.Debug.DrawSphereDebug(FlashlightOriginExtended, 0.1f, Color.Yellow);
                        Rage.Debug.DrawSphereDebug(FlashlightOrigin, 0.1f, Color.Red);
                    }

                    FakeEmissiveDirection = (FlashlightOrigin - FlashlightOriginExtended).ToNormalized();
                    NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOriginExtended.X, FlashlightOriginExtended.Y, FlashlightOriginExtended.Z, FakeEmissiveDirection.X, FakeEmissiveDirection.Y, FakeEmissiveDirection.Z, 255, 255, 255, 
                        FakeEmissiveDistance, 
                        FakeEmissiveBrightness,
                        FakeEmissiveHardness,
                        FakeEmissiveRadius,
                        FakeEmissiveFallOff
                        );
                    //NativeFunction.Natives.DRAW_SPOT_LIGHT(LightDirection.X, LightDirection.Y, LightDirection.Z, FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, 255, 255, 255, Settings.SettingsManager.FlashlightSettings.FlashlightFakeEmissiveDistance, 1.0f, 0.0f, 13.0f, 1.0f);
                }
            }
        }

    }
}