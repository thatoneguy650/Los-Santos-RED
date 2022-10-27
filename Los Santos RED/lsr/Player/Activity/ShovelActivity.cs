using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class ShovelActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private uint GameTimeStartedHolding;
        private string animDictionary;
        private string animEnter;
        private string animIdle;
        private string animExit;
        private int animEnterFlag = (int)(AnimationFlags.StayInEndFrame );
        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame);
        private int animExitFlag = (int)(AnimationFlags.StayInEndFrame);
        private float animEnterBlendIn = 1.0f;
        private float animIdleBlendIn = 1.0f;
        private float animExitBlendIn = 1.0f;
        private float animEnterBlendOut = -1.0f;
        private float animIdleBlendOut = -1.0f;
        private float animExitBlendOut = -1.0f;
        private Rage.Object Shovel;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_R_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "prop_tool_shovel";
        private bool hasStartedAnimation;
        private ISettingsProvideable Settings;
        private float animationStopTIme;
        private Vector3 StartDiggingPosition;
        private Vector3 HoleLocation;
        private bool hasGroundAtStartDiggingPosition;
        private bool hasGroundAtHole;
        private CameraControl CameraControl;
        private ICameraControllable CameraControllable;
        private Rage.Object DirtPile;

        public ShovelActivity(IActionable player, ModItem modItem, ISettingsProvideable settings, ICameraControllable cameraControllable) : base()
        {
            Player = player;
            ModItem = modItem;
            Settings = settings;
            CameraControllable = cameraControllable;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Shovel";
        public override string CancelPrompt { get; set; } = "Put Away Shovel";
        public override string ContinuePrompt { get; set; } = "Continue Shovel";
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
            EntryPoint.WriteToConsole($"Shovel Start", 5);
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                Enter();
            }, "ShovelActivity");
        }
        private void Enter()
        {
            // Game.FadeScreenOut(500, true);

            if(Settings.SettingsManager.ActivitySettings.ShovelFadeOut)
            {
                Game.FadeScreenOut(500, true);
            }

            if (Settings.SettingsManager.ActivitySettings.ShovelUseAltCamera)
            {
                CameraControl = new CameraControl(CameraControllable);
                CameraControl.Setup();
                CameraControl.TransitionHighlightEntity(Player.Character, false, Settings.SettingsManager.ActivitySettings.ShovelCameraOffsetX, Settings.SettingsManager.ActivitySettings.ShovelCameraOffsetY, Settings.SettingsManager.ActivitySettings.ShovelCameraOffsetZ);
            }


            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            AttachShovelToHand();
            GetHoleLocations();


            CreateDirt();

            if (animEnter != "")
            {
                GameTimeStartedHolding = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1


                if (Settings.SettingsManager.ActivitySettings.ShovelFadeOut)
                {
                    GameFiber.Sleep(1000);
                    Game.FadeScreenIn(500, false);
                }
                    while (Player.ActivityManager.CanPerformActivities && !IsCancelled && Game.GameTime - GameTimeStartedHolding <= 25000)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animEnter);
                    if (AnimationTime >= Settings.SettingsManager.ActivitySettings.ShovelAnimationStopTime)
                    {
                        break;
                    }

                    if (Settings.SettingsManager.ActivitySettings.ShovelDebugDrawMarkers)
                    {
                        if (hasGroundAtStartDiggingPosition)
                        {
                            Rage.Debug.DrawArrowDebug(StartDiggingPosition, Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
                        }

                        if (hasGroundAtHole)
                        {
                            Rage.Debug.DrawArrowDebug(HoleLocation, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
                        }
                    }
                    GameFiber.Yield();
                }
            }
            if (Game.IsScreenFadedOut)
            {
                Game.FadeScreenIn(1000, true);
            }
            Idle();
        }
        private void GetHoleLocations()
        {
            StartDiggingPosition = Player.Character.GetOffsetPosition(new Vector3(Settings.SettingsManager.ActivitySettings.ShovelStartOffsetX, Settings.SettingsManager.ActivitySettings.ShovelStartOffsetY, 0.5f));
            float StartDiggingGroundZ;
            hasGroundAtStartDiggingPosition = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(StartDiggingPosition.X, StartDiggingPosition.Y, StartDiggingPosition.Z, out StartDiggingGroundZ, true, false);
            if(hasGroundAtStartDiggingPosition)
            {
                StartDiggingPosition = new Vector3(StartDiggingPosition.X, StartDiggingPosition.Y, StartDiggingGroundZ);
            }


            HoleLocation = Player.Character.GetOffsetPosition(new Vector3(Settings.SettingsManager.ActivitySettings.ShovelHoleOffsetX, Settings.SettingsManager.ActivitySettings.ShovelHoleOffsetY, 0.5f));
            float HoleGroundZ;
            hasGroundAtHole = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(HoleLocation.X, HoleLocation.Y, HoleLocation.Z, out HoleGroundZ, true, false);
            if (hasGroundAtHole)
            {
                HoleLocation = new Vector3(HoleLocation.X, HoleLocation.Y, HoleGroundZ);





            }
        }
        private void Idle()
        {
            Exit();
        }
        private void Exit()
        {

            if (Settings.SettingsManager.ActivitySettings.ShovelUseAltCamera)
            {
                CameraControl?.TransitionToGameplayCam(false);
            }

            if (Shovel.Exists())
            {
                Shovel.Delete();
            }

            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;


            GameFiber.Sleep(10000);
            if (DirtPile.Exists())
            {
                DirtPile.Delete();
            }

        }

        private void AttachShovelToHand()
        {
            CreateShovel();
            if (Shovel.Exists() && !IsAttachedToHand)
            {
                Shovel.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Shovel;
            }
        }
        private void CreateShovel()
        {
            if (!Shovel.Exists() && PropModelName != "")
            {
                try
                {
                    Shovel = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Shovel.Exists())
                {
                    IsCancelled = true;
                }
            }
        }//prop_pile_dirt_01
        private void CreateDirt()
        {
            if (!DirtPile.Exists())
            {
                try
                {
                    DirtPile = new Rage.Object("proc_sml_stones02", HoleLocation);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
            }
        }//prop_pile_dirt_01
        private void Setup()
        {
            animDictionary = "switch@trevor@digging";
            animEnter = "001433_01_trvs_26_digging_exit";
            animIdle = "";
            animExit = "";
            HandBoneName = "BONETAG_R_PH_HAND";
            animIdleBlendIn = 4.0f;
            animIdleBlendOut = -4.0f;
            PropModelName = "prop_tool_shovel";
            HandOffset = new Vector3(0.005f, 0.006f, -0.048f);
            HandRotator = new Rotator(3f, -183f, 0f);
            if (ModItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                PropAttachment handAttachment = ModItem.ModelItem.Attachments.Where(x => x.Name == "RightHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }
            }
            
            AnimationDictionary.RequestAnimationDictionay(animDictionary);
        }
    }
}