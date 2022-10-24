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
        private float FakeEmissiveExtraDistanceX;
        private float FakeEmissiveExtraDistanceY;
        private float FakeEmissiveExtraDistanceZ;
        private float ExtraRotation;
        private float NonCameraExtra;
        private float PitchModifier;

        public FlashlightActivity(IActionable player, ISettingsProvideable settings, string propName) : base()
        {
            Player = player;
            Settings = settings;
            PropModelName = propName;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
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
            EntryPoint.WriteToConsole($"Flashlight Start", 5);
            GameFiber FlashlightWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                Enter();
            }, "FlashlightActivity");
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            
            if (animEnter != "")
            {
                AttachFlashlightToHand();
                EntryPoint.WriteToConsole($"Flashlight Enter: {animEnter}", 5);
                PlayingDictionary = animEnterDictionary;
                PlayingAnimation = animEnter;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animEnterDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
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
                EntryPoint.WriteToConsole($"Flashlight Idle: {animBase}", 5);
                GameTimeStartedHoldingUmbrella = Game.GameTime;

                PlayingDictionary = animBaseDictionary;
                PlayingAnimation = animBase;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animBaseDictionary, animBase, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1

                Player.ButtonPrompts.AddPrompt("Flashlight", "Toggle Light", "FlashlightToggle", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
                Player.ButtonPrompts.AddPrompt("Flashlight", "Search", "FlashlightPlayAnimation", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);

                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
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
                            Player.ButtonPrompts.AddPrompt("Flashlight", "Stop Search", "FlashlightStopAnimation", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
                        }
                        else
                        {
                            Player.ButtonPrompts.RemovePrompt("FlashlightStopAnimation");
                            Player.ButtonPrompts.AddPrompt("Flashlight", "Search", "FlashlightPlayAnimation", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
                        }
                        isSearching = IsSearching;
                    }
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
                    if (AnimationTime >= 1.0f)
                    {
                        IsSearching = false;
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
                    EntryPoint.WriteToConsole($"Flashlight Exit: {animExit}", 5);
                    GameTimeStartedHoldingUmbrella = Game.GameTime;
                    PlayingDictionary = animExitDictionary;
                    PlayingAnimation = animExit;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animExitDictionary, animExit, animExitBlendIn, animExitBlendOut, -1, animExitFlag, 0, false, false, false);//-1
                    while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
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
            if (Player.ButtonPrompts.IsPressed("FlashlightToggle"))
            {
                IsLightOn = !IsLightOn;
                EntryPoint.WriteToConsole($"FlashlightToggle Toggled to {IsLightOn}");
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

                EntryPoint.WriteToConsole($"FlashlightPlayAnimation");
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
            PlayingDictionary = animBaseDictionary;
            PlayingAnimation = animBase;
            EntryPoint.WriteToConsole($"FlashlightPlayAnimation SET BASE {PlayingDictionary} {PlayingAnimation}");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1
        }
        private void StartIdleAnimation(string dict, string anim)
        {
            PlayingDictionary = dict;
            PlayingAnimation = anim;
            EntryPoint.WriteToConsole($"FlashlightPlayAnimation New Idle {PlayingDictionary} {PlayingAnimation}");
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
                Player.AttachedProp = Flashlight;
                InitialDirection = Flashlight.Direction;
            }
        }
        private void UpdateFlashlightPosition()
        {

            if (Flashlight.Exists() && IsAttachedToHand)
            {
                if (Settings.SettingsManager.FlashlightSettings.AllowPropRotation && Settings.SettingsManager.FlashlightSettings.LightFollowsCamera)
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
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
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
            animExitDictionary = "amb@world_human_security_shine_torch@male@exit";
            animExit = "exit";
            animBaseDictionary = "amb@world_human_security_shine_torch@male@base";
            animBase = "base";

            animBaseBlendIn = 1.0f;
            animBaseBlendOut = -1.0f;

            animIdleBlendIn = 1.0f;
            animIdleBlendOut = -1.0f;

            animExitBlendIn = 1.0f;
            animExitBlendOut = -1.0f;

            animEnterBlendIn = 1.0f;
            animEnterBlendOut = -1.0f;

            //PropModelName = "prop_cs_police_torch";
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

            animIdles = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_a"),//Right to left above and below
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_b"),//does a box, faster
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_a","idle_c"),//Does a right to left mostly down
                    //new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_b","idle_d"),//Does a box around, mostly above
                    new Tuple<string, string>("amb@world_human_security_shine_torch@male@idle_b","idle_e"),// right to left in middle
                };

            //PropModelName = Settings.SettingsManager.FlashlightSettings.PropName;
            if (PropModelName == "prop_tool_torch")
            {
                HandOffset = new Vector3(0.12f, -0.02f, -0.08f);
                HandRotator = new Rotator(0f, 0f, -100f);
                //ExtraDistanceX = 0.1f;
                //ExtraDistanceY = 0.3f;

                ExtraDistanceX = 0.1f;
                ExtraDistanceY = 0.35f;

                FakeEmissiveExtraDistanceX = 0.0f;
                FakeEmissiveExtraDistanceY = 0.1f;
                FakeEmissiveExtraDistanceZ = 0.0f;
                ExtraRotation = -90f;
                PitchModifier = 1.0f;
                NonCameraExtra = 1.0f;
            }


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


            bool FollowCamera = Settings.SettingsManager.FlashlightSettings.LightFollowsCamera;
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
                if (directionRotator.Pitch >= Settings.SettingsManager.FlashlightSettings.PitchMax)
                {
                    directionRotator.Pitch = Settings.SettingsManager.FlashlightSettings.PitchMax;
                }
                if (directionRotator.Pitch <= Settings.SettingsManager.FlashlightSettings.PitchMin)
                {
                    directionRotator.Pitch = Settings.SettingsManager.FlashlightSettings.PitchMin;
                }
                float HeadingDiff = ExtensionsMethods.Extensions.GetHeadingDifference(playerRotator.Yaw, directionRotator.Yaw);
                if (HeadingDiff >= Settings.SettingsManager.FlashlightSettings.HeadingMax || HeadingDiff <= Settings.SettingsManager.FlashlightSettings.HeadingMin)
                {
                    if (HeadingDiff >= Settings.SettingsManager.FlashlightSettings.HeadingMax)
                    {
                        directionRotator.Yaw = playerRotator.Yaw + Settings.SettingsManager.FlashlightSettings.HeadingMax;
                    }
                    else if (HeadingDiff <= Settings.SettingsManager.FlashlightSettings.HeadingMin)
                    {
                        directionRotator.Yaw = playerRotator.Yaw + Settings.SettingsManager.FlashlightSettings.HeadingMin;
                    }
                }
                LightRotator = new Rotator(directionRotator.Pitch * PitchModifier, directionRotator.Roll, directionRotator.Yaw - playerRotator.Yaw + ExtraRotation).ToNormalized();
                //LightRotator = new Rotator(directionRotator.Pitch * -1.0f,directionRotator.Roll,directionRotator.Yaw - playerRotator.Yaw + Settings.SettingsManager.FlashlightSettings.FlashlightHeadingMax + Settings.SettingsManager.FlashlightSettings.FlashlightRotationExtra).ToNormalized();
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
                                                                ExtraDistanceX - Flashlight.Model.Dimensions.X / 2,
                                                                ExtraDistanceY - Flashlight.Model.Dimensions.Y / 2,
                                                                0);
            FlashlightOrigin = Flashlight.GetOffsetPosition(FlashlightFrontOffsetAmount);


            Vector3 FlashlightFrontOffsetAmountExtended = new Vector3(
                                                    FakeEmissiveExtraDistanceX + FlashlightFrontOffsetAmount.X,
                                                    FakeEmissiveExtraDistanceY + FlashlightFrontOffsetAmount.Y,
                                                    FakeEmissiveExtraDistanceZ + FlashlightFrontOffsetAmount.Z);
            FlashlightOriginExtended = Flashlight.GetOffsetPosition(FlashlightFrontOffsetAmountExtended);
        }
        private void DrawLights()
        {
            if (IsLightOn)
            {

                NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, LightDirection.X, LightDirection.Y, LightDirection.Z, 255, 255, 255,
                        Settings.SettingsManager.FlashlightSettings.EmissiveDistance,
                        Settings.SettingsManager.FlashlightSettings.EmissiveBrightness,
                        Settings.SettingsManager.FlashlightSettings.EmissiveHardness,
                        Settings.SettingsManager.FlashlightSettings.EmissiveRadius,
                        Settings.SettingsManager.FlashlightSettings.EmissiveFallOff
                    );
                
                //NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, LightDirection.X, LightDirection.Y, LightDirection.Z, 255, 255, 255, 100f, 1.0f, 0.0f, 13.0f, 1.0f);
                if (Settings.SettingsManager.FlashlightSettings.UseFakeEmissive)
                {
                    if (Settings.SettingsManager.FlashlightSettings.ShowDebugMarkerAtEmissiveTip)
                    {
                        Rage.Debug.DrawSphereDebug(FlashlightOriginExtended, 0.1f, Color.Yellow);
                        Rage.Debug.DrawSphereDebug(FlashlightOrigin, 0.1f, Color.Red);
                    }

                    FakeEmissiveDirection = (FlashlightOrigin - FlashlightOriginExtended).ToNormalized();
                    NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOriginExtended.X, FlashlightOriginExtended.Y, FlashlightOriginExtended.Z, FakeEmissiveDirection.X, FakeEmissiveDirection.Y, FakeEmissiveDirection.Z, 255, 255, 255, 
                        Settings.SettingsManager.FlashlightSettings.FakeEmissiveDistance, 
                        Settings.SettingsManager.FlashlightSettings.FakeEmissiveBrightness, 
                        Settings.SettingsManager.FlashlightSettings.FakeEmissiveHardness,
                        Settings.SettingsManager.FlashlightSettings.FakeEmissiveRadius,
                        Settings.SettingsManager.FlashlightSettings.FakeEmissiveFallOff
                        );
                    //NativeFunction.Natives.DRAW_SPOT_LIGHT(LightDirection.X, LightDirection.Y, LightDirection.Z, FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, 255, 255, 255, Settings.SettingsManager.FlashlightSettings.FlashlightFakeEmissiveDistance, 1.0f, 0.0f, 13.0f, 1.0f);
                }
            }
        }

    }
}