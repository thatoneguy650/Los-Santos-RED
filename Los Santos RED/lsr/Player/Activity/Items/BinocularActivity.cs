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
        private IActionable Player;
        private ISettingsProvideable Settings;
        private Camera ZoomedCamera;

        private bool IsCancelled;
        private bool IsZoomedIn = false;
        private bool isZoomedIn = false;
        private bool IsAttachedToHand;

        private string PlayingDictionary = "";
        private string PlayingAnimation = "";
        private float CurrentAnimationTime = 0.0f;
        private bool hasStartedAnimation;

        private BinocularsItem BinocularItem;
        private float ExtraDistanceX;
        private float ExtraDistanceY;
        private float ExtraDistanceZ;
        private Rage.Object Binoculars;
        private string PropModelName = "prop_binoc_01";
        private Vector3 BinocularsOrigin;
        private Vector3 lastCameraDirection;
        private float CurrentFOV;
        private int FovOrder = 0;
        private int binocularsScaleformID;

        private string HandBoneName = "BONETAG_R_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private Vector3 InitialDirection;

        private string animLowerDictionary;
        private string animLower;
        private float animLowerBlendIn = 4.0f;
        private float animLowerBlendOut = -4.0f;
        private int animLowerFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private string animLowDictionary;
        private string animLowStationary;
        private string animLowWalking;
        private string animLowRunning;
        private float animLowBlendIn = 4.0f;
        private float animLowBlendOut = -4.0f;
        private int animLowFlag = (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private string animRaiseDictionary;
        private string animRaise;
        private float animRaiseBlendIn = 4.0f;
        private float animRaiseBlendOut = -4.0f;
        private int animRaiseFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private List<Tuple<string, string>> animRaisedIdles;
        private float animRaisedIdleBlendIn = 8.0f;
        private float animRaisedIdleBlendOut = -8.0f;
        private int animRaisedIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

        private bool IsBinocsRaised = false;
        private bool IsRaisingBinocs = false;
        private bool IsLoweringBinocs = false;
        private bool isNightVision;
        private bool IsNightVision;
        private bool IsThermalVision;
        private bool isThermalVision;
        private int VisionOrder = 0;

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
        public override bool IsUpperBodyOnly { get; set; } = true;
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
           // EntryPoint.WriteToConsole($"Binoculars Start");
            GameFiber BinocWatcher = GameFiber.StartNew(delegate
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
            }, "BinocWatcher");
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
            Setup();
            if (!IsCancelled)
            {
                AttachBinocularsToHand();
                Tick();
            }
        }
        private void Tick()
        {
            AddPrompts();
            LowIdle();
            while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled)
            {
                if (!IsBinocsRaised)
                {
                    LowIdle();
                }
                GeneralTick();
                StatusTick();
                InputTick();
                ZoomTick();   
                GameFiber.Yield();
            }
            Exit();
        }
        private void RaiseBinoculars()
        {
            if (!IsCancelled)
            {
                //EntryPoint.WriteToConsoleTestLong("Raise Binocs Start");
                IsRaisingBinocs = true;
                IsLoweringBinocs = false;
                IsNightVision = false;
                IsThermalVision = false;
                RemovePrompts();
                PlayingDictionary = animRaiseDictionary;
                PlayingAnimation = animRaise;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animRaiseBlendIn, animRaiseBlendOut, -1, animRaiseFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                IsBinocsRaised = true;
                IsRaisingBinocs = false;
                IsZoomedIn = true;
                ResetFOV();
                AddPrompts();
                //EntryPoint.WriteToConsoleTestLong("Raise Binocs End");
            }
        }
        private void LowerBinoculars(bool restartIdle)
        {
            if (!IsCancelled)
            {
                //EntryPoint.WriteToConsoleTestLong("Lower Binocs Start");
                IsLoweringBinocs = true;
                IsRaisingBinocs = false;
                IsZoomedIn = false;
                IsNightVision = false;
                IsThermalVision = false;
                RemovePrompts();
                PlayingDictionary = animLowerDictionary;
                PlayingAnimation = animLower;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowerBlendIn, animLowerBlendOut, -1, animLowerFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitesBase && Player.IsOnFoot && !IsCancelled && CurrentAnimationTime < 1.0f)
                {
                    DisableControls();
                    GeneralTick();
                    GameFiber.Yield();
                }
                IsBinocsRaised = false;
                IsLoweringBinocs = false;
                ResetFOV();
                if (restartIdle)
                {
                    LowIdle();
                    AddPrompts();
                }
                //EntryPoint.WriteToConsoleTestLong("Lower Binocs End");
            }
        }
        private void Exit()
        {
            if (!IsCancelled && IsBinocsRaised)
            {
                LowerBinoculars(false);
            }
            Dispose();
        }
        private void LowIdle()
        {
            PlayingDictionary = animLowDictionary;
            bool restartAnimation = false;
            if (Player.Character.Speed >= 3.0f)
            {
                if (PlayingAnimation != animLowRunning)
                {
                    restartAnimation = true;
                    PlayingAnimation = animLowRunning;
                }
            }
            else if (Player.Character.Speed >= 1.5f)
            {
                if (PlayingAnimation != animLowWalking)
                {
                    restartAnimation = true;
                    PlayingAnimation = animLowWalking;
                }
            }
            else
            {
                if (PlayingAnimation != animLowStationary)
                {
                    restartAnimation = true;
                    PlayingAnimation = animLowStationary;
                }
            }
            if (restartAnimation)
            {
                //EntryPoint.WriteToConsoleTestLong($"Idle Low Start {PlayingDictionary}    ANIM: {PlayingAnimation}");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDictionary, PlayingAnimation, animLowBlendIn, animLowBlendOut, -1, animLowFlag, 0, false, false, false);//-1
            }
        }
        private void GeneralTick()
        {
            Player.WeaponEquipment.SetUnarmed();
            CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
        }
        private void StatusTick()
        {
            if (isZoomedIn != IsZoomedIn)
            {
                OnIsZoomedInChanged();
            }
            if(isNightVision != IsNightVision)
            {
                OnIsNightVisionChanged();
            }
            if(isThermalVision != IsThermalVision)
            {
                OnIsHeatVisionChanged();
            }
            CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDictionary, PlayingAnimation);
        }
        private void InputTick()
        {
            DisableControls();

            if(Player.IsShowingActionWheel)
            {
                return;
            }

            if (Player.ButtonPrompts.IsPressed("BinocularsLower"))
            {
                LowerBinoculars(true);
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsRaise"))
            {
                RaiseBinoculars();
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsZoomIn"))
            {
                IncreaseZoom();
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsZoomOut"))
            {
                DecreaseZoom();
            }
            if (Player.ButtonPrompts.IsPressed("BinocularsVision"))
            {
                ChangeVisionMode();
            }
        }
        private void ResetFOV()
        {
            FovOrder = 0;
            CurrentFOV = BinocularItem.MaxFOV;
        }
        private void IncreaseZoom()
        {
            FovOrder++;
            if (FovOrder > 2)
            {
                FovOrder = 2;
            }
            ChangeZoomLevel();
        }
        private void DecreaseZoom()
        {
            FovOrder--;
            if (FovOrder < 0)
            {
                FovOrder = 0;
            }
            ChangeZoomLevel();
        }
        private void ChangeZoomLevel()
        {
            //FovOrder++;
            //if(FovOrder > 2)
            //{
            //    FovOrder = 0;
            //}
            if(FovOrder == 0)
            {
                CurrentFOV = BinocularItem.MaxFOV;
            }
            else if (FovOrder == 1)
            {
                CurrentFOV = BinocularItem.MidFOV;
            }
            else if (FovOrder == 2)
            {
                CurrentFOV = BinocularItem.MinFOV;
            }
        }
        private void ChangeVisionMode()
        {
            VisionOrder++;
            int Max = 0;
            if(BinocularItem.HasThermalVision)
            {
                Max = 2;
            }
            else if(BinocularItem.HasNightVision)
            {
                Max = 1;
            }

            if(VisionOrder > Max)
            {
                VisionOrder = 0;
            }
            if(VisionOrder == 0)
            {
                IsNightVision = false;
                IsThermalVision = false;
            }
            else if(VisionOrder == 1)
            {
                if(BinocularItem.HasNightVision)
                {
                    IsNightVision = true;
                }
                else
                {
                    IsNightVision = false;
                }
                IsThermalVision = false;
            }
            else if (VisionOrder == 2)
            {
                if (BinocularItem.HasThermalVision)
                {
                    IsThermalVision = true;
                }
                else
                {
                    IsThermalVision = false;
                }
                IsNightVision = false;
            }
        }
        private void ZoomTick()
        {
            if (ZoomedCamera.Exists() && IsZoomedIn)
            {
                GetBinocularLocation();
                ZoomedCamera.Position = BinocularsOrigin;
                ZoomedCamera.Direction = GameplayCameraDirection();// NativeHelper.GetGameplayCameraDirection();
                //ZoomedCamera.FOV = CurrentFOV;// Settings.SettingsManager.ActivitySettings.BinocFOV;

                NativeFunction.Natives.SET_CAM_FOV((uint)ZoomedCamera.Handle, CurrentFOV);

                if (Settings.SettingsManager.BinocularSettings.NearDOF != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_NEAR_DOF((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.NearDOF);
                }
                if (Settings.SettingsManager.BinocularSettings.FarDOF != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_FAR_DOF((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.FarDOF);
                }
                if (Settings.SettingsManager.BinocularSettings.DOFStrength != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_DOF_STRENGTH((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.DOFStrength);
                }
                if (Settings.SettingsManager.BinocularSettings.MotionBlur != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_MOTION_BLUR_STRENGTH((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.MotionBlur);
                }
                if (Settings.SettingsManager.BinocularSettings.DrawScaleform)
                {
                    NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_FULLSCREEN(binocularsScaleformID, 255, 255, 255, 255, 0);
                }
                NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
            }
            if(!IsZoomedIn && Settings.SettingsManager.BinocularSettings.DrawMarkers)
            {
                GetBinocularLocation();
                Rage.Debug.DrawSphereDebug(BinocularsOrigin, 0.1f, Color.Yellow);
            }
        }
        private void OnIsZoomedInChanged()
        {
            if (IsZoomedIn)
            {
                CreateZoomedCamera();
            }
            else
            {
                ReturnToGameplayerCamera();
            }
            isZoomedIn = IsZoomedIn;
        }
        private void OnIsNightVisionChanged()
        {
            NativeFunction.Natives.SET_SEETHROUGH(false);
            NativeFunction.Natives.SET_NIGHTVISION(IsNightVision);
            isNightVision = IsNightVision;
        }
        private void OnIsHeatVisionChanged()
        {
            NativeFunction.Natives.SET_NIGHTVISION(false);
            NativeFunction.Natives.SET_SEETHROUGH(IsThermalVision);
            isThermalVision = IsThermalVision;
        }
        private void CreateZoomedCamera()
        {
            if(!ZoomedCamera.Exists() && IsZoomedIn)
            {
                ZoomedCamera = new Camera(false);
                GetBinocularLocation();
                ZoomedCamera.Position = BinocularsOrigin;
                NativeFunction.Natives.SET_CAM_FOV((uint)ZoomedCamera.Handle, CurrentFOV);
                //ZoomedCamera.FOV = CurrentFOV;//Settings.SettingsManager.ActivitySettings.BinocFOV;
                Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
                ZoomedCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
                ZoomedCamera.Direction = GameplayCameraDirection();// NativeHelper.GetGameplayCameraDirection();
                if (Settings.SettingsManager.BinocularSettings.NearDOF != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_NEAR_DOF((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.NearDOF);
                }
                if (Settings.SettingsManager.BinocularSettings.FarDOF != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_FAR_DOF((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.FarDOF);
                }
                if (Settings.SettingsManager.BinocularSettings.DOFStrength != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_DOF_STRENGTH((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.DOFStrength);
                }
                if (Settings.SettingsManager.BinocularSettings.MotionBlur != 0.0f)
                {
                    NativeFunction.Natives.SET_CAM_MOTION_BLUR_STRENGTH((uint)ZoomedCamera.Handle, Settings.SettingsManager.BinocularSettings.MotionBlur);
                }
                ZoomedCamera.Active = true;
            }
        }
        private void ReturnToGameplayerCamera()
        {
            if (ZoomedCamera.Exists())
            {
                ZoomedCamera.Active = false;
                ZoomedCamera.Delete();
            }
            if(NativeFunction.Natives.GET_USINGNIGHTVISION<bool>())
            {
                NativeFunction.Natives.SET_NIGHTVISION(false);
            }
            IsNightVision = false;
            IsThermalVision = false;
            if (NativeFunction.Natives.GET_USINGSEETHROUGH<bool>())
            {
                NativeFunction.Natives.SET_SEETHROUGH(false);
            }
        }
        private Vector3 GameplayCameraDirection()
        {
            Vector3 cameraDirection = NativeHelper.GetGameplayCameraDirection().ToNormalized();
            cameraDirection = new Vector3(cameraDirection.X, cameraDirection.Y, cameraDirection.Z).ToNormalized();
            Rotator directionRotator = cameraDirection.ToRotator();
            Rotator playerRotator = Game.LocalPlayer.Character.Direction.ToRotator();
            //if (directionRotator.Pitch >= 15f)
            //{
            //    directionRotator.Pitch = 15f;
            //}
            //if (directionRotator.Pitch <= -15f)
            //{
            //    directionRotator.Pitch = -15f;
            //}
            float HeadingDiff = Extensions.GetHeadingDifference(playerRotator.Yaw, directionRotator.Yaw);
            if (HeadingDiff >= 60f || HeadingDiff <= -60f)
            {
                if(lastCameraDirection == Vector3.Zero)
                {
                    cameraDirection = new Rotator(directionRotator.Pitch, playerRotator.Roll, playerRotator.Yaw).ToNormalized().ToVector(); //lastCameraDirection;//cameraDirection = new Vector3(Binoculars.Direction.X, Binoculars.Direction.Y, Binoculars.Direction.Z).ToNormalized();
                }
                else
                {
                    cameraDirection = new Rotator(directionRotator.Pitch, lastCameraDirection.ToRotator().Roll, lastCameraDirection.ToRotator().Yaw).ToNormalized().ToVector(); //lastCameraDirection;//cameraDirection = new Vector3(Binoculars.Direction.X, Binoculars.Direction.Y, Binoculars.Direction.Z).ToNormalized();
                }


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
        private void AttachBinocularsToHand()
        {
            CreateBinoculars();
            if (Binoculars.Exists() && !IsAttachedToHand)
            {
                Binoculars.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Binoculars);
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
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Binoculars.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void AddPrompts()
        {
            if(IsBinocsRaised)
            {
                Player.ButtonPrompts.AddPrompt("Binoculars", "Zoom In", "BinocularsZoomIn", GameControl.WeaponWheelPrev, 10);
                Player.ButtonPrompts.AddPrompt("Binoculars", "Zoom Out", "BinocularsZoomOut", GameControl.WeaponWheelNext, 15);
                Player.ButtonPrompts.AddPrompt("Binoculars", "Vision Mode", "BinocularsVision", GameControl.Attack, 20);
                Player.ButtonPrompts.AddPrompt("Binoculars", "Lower", "BinocularsLower", GameControl.Aim, 30);
            }
            else
            {
                Player.ButtonPrompts.AddPrompt("Binoculars", "Look Through", "BinocularsRaise", GameControl.Aim, 12);
            }
        }
        private void RemovePrompts()
        {
            Player.ButtonPrompts.RemovePrompts("Binoculars");
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


            Game.DisableControlAction(0, GameControl.WeaponWheelNext, true);// false);
            Game.DisableControlAction(0, GameControl.WeaponWheelPrev, true);// false);

            Game.DisableControlAction(0, GameControl.WeaponWheelUpDown, true);// false);
            Game.DisableControlAction(0, GameControl.WeaponWheelLeftRight, true);// false);


            Game.DisableControlAction(0, GameControl.SelectWeapon, true);// false);

            Game.DisableControlAction(0, GameControl.SelectNextWeapon, true);// false);
            Game.DisableControlAction(0, GameControl.SelectPrevWeapon, true);// false);


            Game.DisableControlAction(0, GameControl.NextWeapon, true);// false);
            Game.DisableControlAction(0, GameControl.PrevWeapon, true);//false);
        }
        private void Setup()
        {
            HandBoneName = "BONETAG_R_PH_HAND";

            if (Player.IsMale || 1==1)
            {
                animLowDictionary = "amb@world_human_binoculars@male@base";
                animLowStationary = "idle";
                animLowWalking = "walk";
                animLowRunning = "run";

                animRaiseDictionary = "amb@world_human_binoculars@male@enter";
                animRaise = "enter";//

                animRaisedIdles = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_a"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_b"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_a","idle_c"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_d"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_e"),
                        new Tuple<string, string>("amb@world_human_binoculars@male@idle_b","idle_f"),
                    };

                animLowerDictionary = "amb@world_human_binoculars@male@exit";
                animLower = "exit";
            }
            else
            {
                animLowDictionary = "amb@world_human_binoculars@female@base";
                animLowStationary = "idle";
                animLowWalking = "walk";
                animLowRunning = "run";

                animRaiseDictionary = "amb@world_human_binoculars@female@enter";
                animRaise = "enter";//

                animRaisedIdles = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_a"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_b"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_a","idle_c"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_d"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_e"),
                        new Tuple<string, string>("amb@world_human_binoculars@female@idle_b","idle_f"),
                    };

                animLowerDictionary = "amb@world_human_binoculars@female@exit";
                animLower = "exit";
            }

            CurrentFOV = 55f;
            if (BinocularItem != null && BinocularItem.ModelItem != null)
            {
                PropModelName = BinocularItem.ModelItem.ModelName;
                PropAttachment handAttachment = BinocularItem.ModelItem.Attachments.Where(x => x.Name == "RightHand").FirstOrDefault();
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

            AnimationDictionary.RequestAnimationDictionay(animLowDictionary);
            AnimationDictionary.RequestAnimationDictionay(animRaiseDictionary);
            AnimationDictionary.RequestAnimationDictionay(animLowerDictionary);
            foreach (string idleDict in animRaisedIdles.GroupBy(x => x.Item1).Select(y => y.Key))
            {
                AnimationDictionary.RequestAnimationDictionay(idleDict);
            }

            binocularsScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("BINOCULARS");
            while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(binocularsScaleformID))
            {
                GameFiber.Yield();
            }



            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
        }
        private void Dispose()
        {
            ReturnToGameplayerCamera();
            RemovePrompts();
            if (Binoculars.Exists())
            {
                Binoculars.Delete();
            }
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;

            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        }

        /*
         * 
         * amb@world_human_binoculars@male@enter enter//go up from idle to base
amb@world_human_binoculars@male@exit exit//go down from base to idle
         * amb@world_human_binoculars@male@base base //Up Idle
         * amb@world_human_binoculars@male@base idle //low standing
        amb@world_human_binoculars@male@base run//low running
        amb@world_human_binoculars@male@base walk*///low walking
    }
}