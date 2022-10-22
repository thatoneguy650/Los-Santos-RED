using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LosSantosRED.lsr.Player
{
    public class FlashlightActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private uint GameTimeStartedHoldingUmbrella;
        private string animIdleDictionary;
        private string animEnterDictionary;
        private string animEnter;
        private string animIdle;
        private string animExitDictionary;
        private string animExit;
        private int animEnterFlag = (int)(AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animExitFlag = (int)(AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private float animEnterBlendIn = 4.0f;
        private float animIdleBlendIn = 4.0f;
        private float animExitBlendIn = 4.0f;
        private float animEnterBlendOut = -4.0f;
        private float animIdleBlendOut = -4.0f;
        private bool SetEndFrame;
        private float animExitBlendOut = -4.0f;
        private Rage.Object Flashlight;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "prop_cs_police_torch";
        private Vector3 InitialDirection;
        private Vector3 LightDirection;
        private float GameplayCameraCurrentPitch;
        private float GameplayCameraCurrentHeading;
        private bool IsLightOn = false;
        private LoopedParticle Light;

        public FlashlightActivity(IActionable player, ISettingsProvideable settings) : base()
        {
            Player = player;
            Settings = settings;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Flashlight";
        public override string CancelPrompt { get; set; } = "Drop Flashlight";
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
                EntryPoint.WriteToConsole($"Flashlight Enter: {animEnter}", 5);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animEnterDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
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
            AttachFlashlightToHand();
            Idle();
        }
        private void Idle()
        {
            if (animIdle != "")
            {
                EntryPoint.WriteToConsole($"Flashlight Idle: {animIdle}", 5);
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animIdleDictionary, animIdle, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1


                if (SetEndFrame)
                {
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, animIdleDictionary, animIdle, 0.99f);
                }

                Player.ButtonPrompts.AddPrompt("FlashlightToggle", "Toggle Light", "FlashlightToggle", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 500);


                while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
                {
                    if(Player.IsVisiblyArmed)
                    {
                        IsCancelled = true;
                    }


                    //Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animIdleDictionary, animIdle);


                    HandleLight();
                    HandleButtons();

                    if (Player.ButtonPrompts.IsPressed("FlashlightToggle"))
                    {
                        IsLightOn = !IsLightOn;
                    }


                    GameFiber.Yield();
                }

                Player.ButtonPrompts.RemovePrompts("FlashlightToggle");

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
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animExitDictionary, animExit, animExitBlendIn, animExitBlendOut, -1, animExitFlag, 0, false, false, false);//-1
                    while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
                    {
                        Player.WeaponEquipment.SetUnarmed();
                        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animExitDictionary, animIdle);
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
            if (1 == 1)
            {
                GameFiber.Sleep(500);
            }
            if (Flashlight.Exists())
            {
                Flashlight.Delete();
            }

            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;

            if (Flashlight.Exists())
            {
                Flashlight.Detach();
            }
            if (Flashlight.Exists())
            {
                Flashlight.Delete();
            }
        }
        private void HandleButtons()
        {
           
        }
        private void HandleLight()
        {
            
            if (Flashlight.Exists())
            {
                Vector3 FlashlightOrigin = new Vector3(Flashlight.Position.X - Settings.SettingsManager.ActivitySettings.FlashlightExtraDistanceX - Flashlight.Model.Dimensions.X / 2, Flashlight.Position.Y - Settings.SettingsManager.ActivitySettings.FlashlightExtraDistanceY - Flashlight.Model.Dimensions.Y / 2 , Flashlight.Position.Z);// Vector3 FlashlightOrigin = new Vector3(Flashlight.Position.X, Flashlight.Position.Y, Flashlight.Position.Z);
                Vector3 FlashlightOrigin2 = new Vector3(Settings.SettingsManager.ActivitySettings.FlashlightExtraDistanceX - Flashlight.Model.Dimensions.X / 2, Settings.SettingsManager.ActivitySettings.FlashlightExtraDistanceY - Flashlight.Model.Dimensions.Y / 2, 0);// Vector3 FlashlightOrigin = new Vector3(Flashlight.Position.X, Flashlight.Position.Y, Flashlight.Position.Z);


                Vector3 FlashlightOffset = Flashlight.GetOffsetPosition(FlashlightOrigin2);

                FlashlightOrigin = FlashlightOffset;

                LightDirection = Vector3.Zero;

                GameplayCameraCurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
                GameplayCameraCurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();





                if (Settings.SettingsManager.ActivitySettings.FlashlightFollowCamera)
                {
                    LightDirection = NativeHelper.GetGameplayCameraDirection().ToNormalized();
                    LightDirection = new Vector3(LightDirection.X, LightDirection.Y, LightDirection.Z).ToNormalized();
                    Rotator directionRotator = LightDirection.ToRotator();
                    Rotator playerRotator = Game.LocalPlayer.Character.Direction.ToRotator();
                    if (directionRotator.Pitch >= Settings.SettingsManager.ActivitySettings.FlashlightPitchMax)
                    {
                        directionRotator.Pitch = Settings.SettingsManager.ActivitySettings.FlashlightPitchMax;
                    }
                    if (directionRotator.Pitch <= Settings.SettingsManager.ActivitySettings.FlashlightPitchMin)
                    {
                        directionRotator.Pitch = Settings.SettingsManager.ActivitySettings.FlashlightPitchMin;
                    }
                    float HeadingDiff = ExtensionsMethods.Extensions.GetHeadingDifference(playerRotator.Yaw, directionRotator.Yaw);
                    if(HeadingDiff >= Settings.SettingsManager.ActivitySettings.FlashlightHeadingMax || HeadingDiff <= Settings.SettingsManager.ActivitySettings.FlashlightHeadingMin)
                    { 
                        if(HeadingDiff >= 60f)
                        {
                            directionRotator.Yaw = playerRotator.Yaw + 60f;
                        }
                        else if (HeadingDiff <= -60f)
                        {
                            directionRotator.Yaw = playerRotator.Yaw - 60f;
                        }
                    }
                    LightDirection = directionRotator.ToVector();
                    
                }
                else
                {
                    LightDirection = new Vector3(Flashlight.Direction.X, Flashlight.Direction.Y, Flashlight.Direction.Z).ToNormalized() * -1.0f;
                    LightDirection = new Vector3(LightDirection.X, LightDirection.Y, Flashlight.Direction.Z).ToNormalized();
                }

                //UpdateFlashlightPosition();

                if (IsLightOn)
                {


                    //Light = new LoopedParticle("core", "ent_amb_torch_fire", Flashlight, FlashlightOffset, Rotator.Zero, 1.5f);
    

                    NativeFunction.Natives.DRAW_SHADOWED_SPOT_LIGHT(FlashlightOrigin.X, FlashlightOrigin.Y, FlashlightOrigin.Z, LightDirection.X, LightDirection.Y, LightDirection.Z, 255, 255, 255, 100f, 1.0f, 0.0f, 13.0f, 1.0f);
                }
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
                if (Settings.SettingsManager.ActivitySettings.FlashlightFollowCamera)
                {
                    Flashlight.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, LightDirection.ToRotator());
                }
                else
                {
                    Flashlight.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
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
            animEnterDictionary = "melee@holster";
            animEnter = "";


            animExitDictionary = "melee@holster";
            animExit = "";
            HandBoneName = "BONETAG_L_PH_HAND";
            SetEndFrame = false;


            animIdleBlendIn = 4.0f;
            animIdleBlendOut = -4.0f;
            PropModelName = "prop_cs_police_torch";
            HandOffset = new Vector3(0f, 0f, 0.02f);
            HandRotator = new Rotator(-40f, 0f, -250f);
            animIdleDictionary = "anim@amb@casino@hangout@ped_male@stand_withdrink@01a@base";
            animIdle = "base";




            AnimationDictionary.RequestAnimationDictionay(animEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(animIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(animExitDictionary);
        }
    }
}