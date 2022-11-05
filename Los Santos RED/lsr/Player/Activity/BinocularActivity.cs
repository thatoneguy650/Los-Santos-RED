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
    public class BinocularActivity : DynamicActivity
    {
        private Camera ZoomedCamera;
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

        private List<Tuple<string, string>> animIdles;
        private int animEnterFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animBaseFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);


        private int animExitFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private float animEnterBlendIn = 8.0f;
        private float animBaseBlendIn = 8.0f;
        private float animIdleBlendIn = 8.0f;
        private float animExitBlendIn = 8.0f;
        private float animEnterBlendOut = -8.0f;
        private float MinFOV;
        private float MidFOV;
        private float MaxFOV;
        private float animBaseBlendOut = -8.0f;
        private float animIdleBlendOut = -8.0f;
        private float animExitBlendOut = -4.0f;
        private Rage.Object Binoculars;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "prop_cs_police_torch";
        private Vector3 InitialDirection;


        private string PlayingDictionary = "";
        private string PlayingAnimation = "";
        private bool IsSearching;
        private bool isSearching;


        private BinocularsItem BinocularItem;
        private bool hasStartedAnimation;

        private bool IsZoomedIn = false;
        private bool isZoomedIn = false;
        private dynamic globalScaleformID;
        private float ExtraDistanceX;
        private float ExtraDistanceY;
        private float ExtraDistanceZ;
        private Vector3 BinocularsOrigin;
        private Vector3 lastCameraDirection;

        private float CurrentFOV;

        public BinocularActivity(IActionable player, ISettingsProvideable settings, BinocularsItem binocularItem) : base()
        {
            Player = player;
            Settings = settings;
            BinocularItem = binocularItem;
            ModItem = binocularItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Binoculars";
        public override string CancelPrompt { get; set; } = "Put Away Binoculars";
        public override string ContinuePrompt { get; set; } = "Continue Binoculars";
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
            EntryPoint.WriteToConsole($"Binoculars Start", 5);
            GameFiber BinocWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                Enter();
            }, "BinocWatcher");
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;

            if (animEnter != "")
            {
                AttachBinocularsToHand();
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
                EntryPoint.WriteToConsole($"Binoculars Idle: {animBase}", 5);
                GameTimeStartedHoldingUmbrella = Game.GameTime;

                PlayingDictionary = animBaseDictionary;
                PlayingAnimation = animBase;
                hasStartedAnimation = false;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animBaseDictionary, animBase, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1
                Player.ButtonPrompts.AddPrompt("Binoculars", "Toggle Zoom", "BinocularsToggle", GameControl.Attack, 10);
                if (BinocularItem.CanSearch)
                {
                    Player.ButtonPrompts.AddPrompt("Binoculars", "Search", "BinocularsPlayAnimation", GameControl.Aim, 12);
                }
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
                {
                    if (Player.IsVisiblyArmed)
                    {
                        IsCancelled = true;
                    }
                    if (isSearching != IsSearching)
                    {
                        if (IsSearching)
                        {
                            Player.ButtonPrompts.RemovePrompt("BinocularsPlayAnimation");
                            Player.ButtonPrompts.AddPrompt("Binoculars", "Stop Search", "BinocularsStopAnimation", GameControl.Aim, 12);
                        }
                        else
                        {
                            Player.ButtonPrompts.RemovePrompt("BinocularsStopAnimation");
                            Player.ButtonPrompts.AddPrompt("Binoculars", "Search", "BinocularsPlayAnimation", GameControl.Aim, 12);
                        }
                        isSearching = IsSearching;
                    }

                    if (isZoomedIn != IsZoomedIn)
                    {
                        if (IsZoomedIn)
                        {
                            CreateZoomedCamera();
                           // NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Press", "DLC_SECURITY_BUTTON_PRESS_SOUNDS", 0);
                        }
                        else
                        {
                            ReturnToGameplayerCamera();
                            //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Press", "DLC_SECURITY_BUTTON_PRESS_SOUNDS", 0);
                        }
                        isZoomedIn = IsZoomedIn;
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
                    HandleScaleform();
                    HandleZoom();
                    HandleButtons();
                    GameFiber.Yield();
                }

                Player.ButtonPrompts.RemovePrompts("Binoculars");

            }
            Exit();
        }
        private void ReturnToGameplayerCamera()
        {
            if (ZoomedCamera.Exists())
            {
                ZoomedCamera.Active = false;
                ZoomedCamera.Delete();
            }

            
        }
        private void HandleZoom()
        {
            if (ZoomedCamera.Exists() && IsZoomedIn)
            {
                GetBinocularLocation();
                ZoomedCamera.Position = BinocularsOrigin;
                ZoomedCamera.Direction = GameplayCameraDirection();// NativeHelper.GetGameplayCameraDirection();
                ZoomedCamera.FOV = Settings.SettingsManager.ActivitySettings.BinocFOV;
            }
            if(!IsZoomedIn && Settings.SettingsManager.ActivitySettings.BinocDebugDrawMarkers)
            {
                GetBinocularLocation();
                Rage.Debug.DrawSphereDebug(BinocularsOrigin, 0.1f, Color.Yellow);
            }
        }
        private void CreateZoomedCamera()
        {
            if(!ZoomedCamera.Exists() && IsZoomedIn)
            {
                ZoomedCamera = new Camera(false);
                GetBinocularLocation();
                ZoomedCamera.Position = BinocularsOrigin;
                ZoomedCamera.FOV = Settings.SettingsManager.ActivitySettings.BinocFOV;
                Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
                ZoomedCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
                ZoomedCamera.Direction = GameplayCameraDirection();// NativeHelper.GetGameplayCameraDirection();
                ZoomedCamera.Active = true;
            }

        }
        private Vector3 GameplayCameraDirection()
        {
            Vector3 cameraDirection = NativeHelper.GetGameplayCameraDirection().ToNormalized();
            cameraDirection = new Vector3(cameraDirection.X, cameraDirection.Y, cameraDirection.Z).ToNormalized();
            Rotator directionRotator = cameraDirection.ToRotator();
            Rotator playerRotator = Game.LocalPlayer.Character.Direction.ToRotator();
            if (directionRotator.Pitch >= 15f)
            {
                directionRotator.Pitch = 15f;
            }
            if (directionRotator.Pitch <= -15f)
            {
                directionRotator.Pitch = -15f;
            }
            float HeadingDiff = Extensions.GetHeadingDifference(playerRotator.Yaw, directionRotator.Yaw);
            if (HeadingDiff >= 60f || HeadingDiff <= -60f)
            {
                cameraDirection = lastCameraDirection;//cameraDirection = new Vector3(Binoculars.Direction.X, Binoculars.Direction.Y, Binoculars.Direction.Z).ToNormalized();
            }
            else
            {
                lastCameraDirection = cameraDirection;
            }
            return cameraDirection;
        }
        private void GetBinocularLocation()
        {
            Vector3 FlashlightFrontOffsetAmount = new Vector3(
                                                    Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceX + ExtraDistanceX - Binoculars.Model.Dimensions.X / 2,
                                                    Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceY + ExtraDistanceY - Binoculars.Model.Dimensions.Y / 2,
                                                    Settings.SettingsManager.FlashlightSettings.DebugExtraDistanceZ + ExtraDistanceZ - 0);
            BinocularsOrigin = Binoculars.GetOffsetPosition(FlashlightFrontOffsetAmount);
        }
        private void HandleScaleform()
        {
            if(IsZoomedIn)
            {
                NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_FULLSCREEN(globalScaleformID, 255, 255, 255, 255, 0);
            }
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
                        if (AnimationTime >= 1.0f)
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
            if (Binoculars.Exists())
            {
                Binoculars.Delete();
            }
            Player.ButtonPrompts.RemovePrompts("Binoculars");
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

            Game.DisableControlAction(0, GameControl.Attack, false);
            Game.DisableControlAction(0, GameControl.Attack2, false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, false);


            Game.DisableControlAction(0, GameControl.Aim, false);
            Game.DisableControlAction(0, GameControl.VehicleAim, false);
            Game.DisableControlAction(0, GameControl.AccurateAim, false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, false);


            if (Player.ButtonPrompts.IsPressed("BinocularsToggle"))
            {
                IsZoomedIn = !IsZoomedIn;
                EntryPoint.WriteToConsole($"BinocularsToggle Toggled to {IsZoomedIn}");
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsPlayAnimation"))
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

                EntryPoint.WriteToConsole($"BinocularsPlayAnimation");
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsStopAnimation"))
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
            EntryPoint.WriteToConsole($"PlayAnimation SET BASE {PlayingDictionary} {PlayingAnimation}");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animBaseBlendIn, animBaseBlendOut, -1, animBaseFlag, 0, false, false, false);//-1
        }
        private void StartIdleAnimation(string dict, string anim)
        {
            hasStartedAnimation = false;
            PlayingDictionary = dict;
            PlayingAnimation = anim;
            EntryPoint.WriteToConsole($"PlayAnimation New Idle {PlayingDictionary} {PlayingAnimation}");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1
        }
        private void AttachBinocularsToHand()
        {
            CreateBinoculars();
            if (Binoculars.Exists() && !IsAttachedToHand)
            {
                Binoculars.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Binoculars;
                InitialDirection = Binoculars.Direction;
            }
        }
        private void CreateBinoculars()
        {
            if (!Binoculars.Exists() && PropModelName != "")
            {
                try
                {
                    Binoculars = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Binoculars.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void Setup()
        {
            HandBoneName = "BONETAG_L_PH_HAND";


            animBaseBlendIn = 8.0f;
            animBaseBlendOut = -8.0f;

            animIdleBlendIn = 8.0f;
            animIdleBlendOut = -8.0f;

            animExitBlendIn = 8.0f;
            animExitBlendOut = -8.0f;

            animEnterBlendIn = 8.0f;
            animEnterBlendOut = -8.0f;

            MinFOV = 50f;
            MidFOV = 35f;
            MaxFOV = 10f;

            if (Player.IsMale)
            {
                animEnterDictionary = "amb@world_human_binoculars@male@enter";
                animEnter = "enter";

                animBaseDictionary = "amb@world_human_binoculars@male@base";
                animBase = "base";
                animIdles = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_a"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_b"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_c"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_d"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_e"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_f"),
                    };
                animExitDictionary = "amb@world_human_binoculars@male@exit";
                animExit = "exit";


            }
            else
            {
                animEnterDictionary = "amb@world_human_binoculars@female@enter";
                animEnter = "enter";

                animBaseDictionary = "amb@world_human_binoculars@female@base";
                animBase = "base";
                animIdles = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_a"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_b"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_c"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_d"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_e"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_f"),
                    };
                animExitDictionary = "amb@world_human_binoculars@female@exit";
                animExit = "exit";
            }

            CurrentFOV = 55f;

            if (BinocularItem != null && BinocularItem.ModelItem != null)
            {
                PropModelName = BinocularItem.ModelItem.ModelName;
                PropAttachment handAttachment = BinocularItem.ModelItem.Attachments.Where(x => x.Name == "LeftHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }


                PropAttachment extraDistance = BinocularItem.ModelItem.Attachments.Where(x => x.Name == "ExtraDistance").FirstOrDefault();
                if (extraDistance != null)
                {
                    ExtraDistanceX = extraDistance.Attachment.X;
                    ExtraDistanceY = extraDistance.Attachment.Y;
                    ExtraDistanceZ = extraDistance.Attachment.Z;
                }

                CurrentFOV = BinocularItem.MaxFOV;

            }

            AnimationDictionary.RequestAnimationDictionay(animEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(animBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(animExitDictionary);
            foreach (string idleDict in animIdles.GroupBy(x => x.Item1).Select(y => y.Key))
            {
                AnimationDictionary.RequestAnimationDictionay(idleDict);
            }


            globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("BINOCULARS");
            while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
            {
                GameFiber.Yield();
            }



        }
    }
}