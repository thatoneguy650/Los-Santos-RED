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

namespace LosSantosRED.lsr.Player
{
    public class SittingActivity : DynamicActivity
    {
        private string PlayingAnim;
        private string PlayingDict;
        private SittingData Data;
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private bool IsActivelySitting = false;
        private Vector3 StartingPosition;
        private int PlayerScene;
        private Rage.Object ClosestSittableEntity;
        private Entity PossibleCollisionTable;
        private Vector3 StoredPlayerPosition;
        private float StoredPlayerHeading;
        private bool FindSittingProp = false;
        private bool EnterForward = true;
        private Vector3 SeatEntryPosition;
        private float SeatEntryHeading;
        private ISeats Seats;
        private float SeatOffset;
        private CameraControl CameraControl;
        private ICameraControllable CameraControllable;
        private List<Rage.Object> CollisionObjects = new List<Rage.Object>();
        private uint GameTimeLastGotHealth;
        private bool HasStartedPoopAnim;

        private bool UseMaleAnimations => Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale || IsSittingOnToilet;
        public SittingActivity(IActionable player, ISettingsProvideable settings, bool findSittingProp, bool enterForward, ISeats seats, ICameraControllable cameraControllable) : base()
        {
            Player = player;
            Settings = settings;
            FindSittingProp = findSittingProp;
            EnterForward = enterForward;
            Seats = seats;
            CameraControllable = cameraControllable;
        }
        public bool IsSittingOnToilet { get; set; } = false;
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = false;
        public override bool IsUpperBodyOnly { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Activity";
        public override string CancelPrompt { get; set; } = "Stop Activity";
        public override string ContinuePrompt { get; set; } = "Continue Activity";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.ActivityManager.IsSitting = false;
            Player.ActivityManager.IsPerformingActivity = false;
            if (IsSittingOnToilet)
            {
                Player.ActivityManager.IsUrinatingDefecting = false;
                Player.ActivityManager.IsUrinatingDefectingOnToilet = false;
            }
            //Player.IsPerformingActivity = false;
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
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    //EntryPoint.WriteToConsoleTestLong("Sitting Activity Enter");
                    Enter();
                    //EntryPoint.WriteToConsoleTestLong("Sitting Activity Final");
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Sitting");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended && !player.ActivityManager.IsResting)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Sit");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsSitting = true;
            if (IsSittingOnToilet)
            {
                Player.ActivityManager.IsUrinatingDefecting = true;
                Player.ActivityManager.IsUrinatingDefectingOnToilet = true;
            }
            if (FindSittingProp)
            {
                if (!GetSittableProp() || !GetSeatCoordinates() || !MoveToSeatCoordinates())
                {
                    Player.ActivityManager.IsSitting = false;
                    if (IsSittingOnToilet)
                    {
                        Player.ActivityManager.IsUrinatingDefecting = false;
                        Player.ActivityManager.IsUrinatingDefectingOnToilet = false;
                    }
                    Player.ActivityManager.IsPerformingActivity = false;
                    if (ClosestSittableEntity.Exists())
                    {
                        ClosestSittableEntity.IsPositionFrozen = false;
                    }
                    return;
                }
            }
            else
            {
                SeatEntryPosition = Player.Character.Position;
                SeatEntryHeading = Player.Character.Heading;
            }
            //GetPossibleBlockingProp();
            if (Settings.SettingsManager.ActivitySettings.UseAltCameraWhenSitting && !Settings.SettingsManager.ActivitySettings.TeleportWhenSitting)
            {
                CameraControl = new CameraControl(CameraControllable);
                CameraControl.Setup();
                CameraControl.TransitionHighlightEntity(Player.Character, false);
            }
            SitDown();
            if (IsActivelySitting)
            {
                Idle();
            }
            else
            {
                Exit();
            }
        }
        private void Idle()
        {
            //EntryPoint.WriteToConsoleTestLong("Sitting Activity Idle Start");
            Player.ActivityManager.IsPerformingActivity = false;
            StartNewBaseScene();
            if (IsSittingOnToilet)
            {
                StartToiletAnimation();
            }
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
            {
                if (IsSittingOnToilet)
                {
                    LoopToilet();
                }
                else
                {
                    LoopRegular();
                }
                //if(IsSittingOnToilet && Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Z) && !HasStartedPoopAnim)
                //{
                //    EntryPoint.WriteToConsole("STARTING POOP ANIM");
                //    HasStartedPoopAnim = true;
                //    StartToiletAnimation();
                //}
                GameFiber.Yield();
            }
            //EntryPoint.WriteToConsoleTestLong("Sitting Activity Idle End");
            Exit();
        }

        private void StartToiletAnimation()
        {
            string dictionary = "timetable@trevor@on_the_toilet";
            string anim = new List<string>() { "trevonlav_backedup", "trevonlav_struggleloop", "trevonlav_midwife" }.PickRandom();
            if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
            {
                return;
            }
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY | eAnimationFlags.AF_LOOPING), 0, false, false, false);
        }

        private void LoopRegular()
        {
            float AnimationTime;
            AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
            if (AnimationTime >= 1.0f && !Player.ActivityManager.IsPerformingActivity)
            {
                StartNewIdleScene();
            }
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            Player.WeaponEquipment.SetUnarmed();
        }
        private void LoopToilet()
        {
            float AnimationTime;
            AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
            if (AnimationTime >= 1.0f && !Player.ActivityManager.IsPerformingActivity)
            {
                StartNewIdleScene();
            }
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (Game.GameTime - GameTimeLastGotHealth >= 3000)
            {
                Player.ActivityManager.PlaySpecificFacialAnimations(Player.IsMale ? new List<string>() { "effort_1", "effort_2", "effort_3", }.PickRandom() : "effort_1");
                //Player.HealthManager.ChangeHealth(1);
                GameTimeLastGotHealth = Game.GameTime;
            }   
            Player.WeaponEquipment.SetUnarmed();
        }
        private void Exit()
        {
            //EntryPoint.WriteToConsoleTestLong("Sitting Activity Exit Start");
            Player.ActivityManager.PauseCurrentActivity();
            if (Settings.SettingsManager.ActivitySettings.TeleportWhenSitting)
            {
                Game.FadeScreenOut(500, true);
                Player.Character.Position = StoredPlayerPosition;
                Player.Character.Heading = StoredPlayerHeading;
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Game.FadeScreenIn(500, true);
                Player.ActivityManager.IsSitting = false;
                if (IsSittingOnToilet)
                {
                    Player.ActivityManager.IsUrinatingDefecting = false;
                    Player.ActivityManager.IsUrinatingDefectingOnToilet = false;
                }
                Player.ActivityManager.IsPerformingActivity = false;
            }
            else
            {
                if (Settings.SettingsManager.ActivitySettings.UseAltCameraWhenSitting)
                {
                    CameraControl?.TransitionToGameplayCam(false);
                }
                if (IsActivelySitting)
                {
                    PlayingDict = Data.AnimExitDictionary;
                    PlayingAnim = Data.AnimExit;
                    Vector3 Position = Game.LocalPlayer.Character.Position;
                    float Heading = Game.LocalPlayer.Character.Heading;
                    uint GameTimeStartedExiting = Game.GameTime;
                    PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                    float AnimationTime = 0f;
                    AnimationWatcher aw = new AnimationWatcher();
                    while (AnimationTime < 1.0f && Game.GameTime - GameTimeStartedExiting <= 5000)
                    {
                        AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                        if(!aw.IsAnimationRunning(AnimationTime))
                        {
                            break;
                        }
                        Player.WeaponEquipment.SetUnarmed();
                        GameFiber.Yield();
                    }
                }
                AnimationDictionary.RequestAnimationDictionay("ped");
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                if (ClosestSittableEntity.Exists())
                {
                    ClosestSittableEntity.IsPositionFrozen = false;
                }
                GameFiber.Yield();
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Player.ActivityManager.IsSitting = false;
                if (IsSittingOnToilet)
                {
                    Player.ActivityManager.IsUrinatingDefecting = false;
                    Player.ActivityManager.IsUrinatingDefectingOnToilet = false;
                }
                Player.ActivityManager.IsPerformingActivity = false;
                GameFiber.Sleep(5000);
            }

            foreach(Rage.Object obj in CollisionObjects)
            {
                if (obj.Exists())
                {
                    NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(obj, obj, false);
                    //obj.CollisionIgnoredEntity = null;
                    //obj.IsCollisionEnabled = true;
                    //obj.NeedsCollision = true;
                    //EntryPoint.WriteToConsoleTestLong("RESET COLLISION FOR ONE PROP, SITTING");
                }
            }
            //EntryPoint.WriteToConsoleTestLong("Sitting Activity Exit End");
        }
        private void SitDown()
        {
            PlayingDict = Data.AnimEnterDictionary;
            PlayingAnim = Data.AnimEnter;
            StartingPosition = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", StartingPosition.X, StartingPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
            NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Data.AnimEnterDictionary, Data.AnimEnter, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            float AnimationTime = 0f;
            while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && AnimationTime < 1.0f)
            {
                AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                if (Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }
                Player.WeaponEquipment.SetUnarmed();
                GameFiber.Yield();
            }
            if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) >= 0.95f)
            {
                IsActivelySitting = true;
            }
        }
        private bool GetSittableProp()
        {
            float ClosestDistance = 999f;
            if (Player.CurrentLookedAtObject.Exists())
            {
                string modelName = Player.CurrentLookedAtObject.Model.Name.ToLower();
                SeatModel seatModel = Seats.GetSeatModel(Player.CurrentLookedAtObject);
                if (seatModel != null || NativeHelper.IsSittableModel(modelName))
                {
                    float DistanceToObject = Player.CurrentLookedAtObject.DistanceTo(Game.LocalPlayer.Character.Position);
                    if (DistanceToObject <= 5f && DistanceToObject >= 0.5f && DistanceToObject <= ClosestDistance)//
                    {
                        ClosestSittableEntity = Player.CurrentLookedAtObject;
                        ClosestDistance = DistanceToObject;
                        return ClosestSittableEntity.Exists();
                    }
                }
            }


            List<Rage.Object> Objects = World.GetAllObjects().ToList(); //EntryPoint.ModController.AllObjects.ToList();// World.GetAllObjects().ToList();
            foreach (Rage.Object obj in Objects)
            {
                if (obj.Exists())
                {
                    string modelName = obj.Model.Name.ToLower();
                    uint hash = obj.Model.Hash;
                    SeatModel seatModel = Seats.GetSeatModel(obj);
                    if (seatModel != null || NativeHelper.IsSittableModel(modelName))
                    {
                        float DistanceToObject = obj.DistanceTo(Game.LocalPlayer.Character.Position);
                        if (DistanceToObject <= 5f && DistanceToObject >= 0.5f && DistanceToObject <= ClosestDistance)//
                        {
                            ClosestSittableEntity = obj;
                            ClosestDistance = DistanceToObject;
                        }
                    }
                }
            }
            return ClosestSittableEntity.Exists();
        }
        private bool GetSeatCoordinates()
        {
            if (ClosestSittableEntity.Exists())
            {
                ClosestSittableEntity.IsPositionFrozen = true;
                SeatOffset = Seats.GetOffSet(ClosestSittableEntity);
                Vector3 DesiredPos = ClosestSittableEntity.GetOffsetPositionFront(SeatOffset);
                float entranceZPosition = DesiredPos.Z;
                NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(DesiredPos.X, DesiredPos.Y, DesiredPos.Z, out entranceZPosition, false);
                DesiredPos = new Vector3(DesiredPos.X, DesiredPos.Y, entranceZPosition);
                float DesiredHeading;// = Math.Abs(ClosestSittableEntity.Heading + 180f);
                if (ClosestSittableEntity.Heading >= 180f)
                {
                    DesiredHeading = ClosestSittableEntity.Heading - 180f;
                }
                else
                {
                    DesiredHeading = ClosestSittableEntity.Heading + 180f;
                }
                float ModelWidth = ClosestSittableEntity.Model.Dimensions.X;
                if (ModelWidth >= 1.2f)
                {
                    int SeatNumber = (int)(ModelWidth / 0.7f);
                    if(SeatNumber > 1)
                    {
                        List<Vector3> PossibleSeatPositions = new List<Vector3>();
                        float FirstPosition = ModelWidth / (float)SeatNumber;    
                        for (int i = 0; i < SeatNumber; i++)
                        {
                            float XPosition = (FirstPosition * (i + 1)) - 0.6f;
                            Vector3 SeatPosition = ClosestSittableEntity.GetOffsetPosition(new Vector3((-1 * ModelWidth / 2) + XPosition,SeatOffset, 0f));
                            PossibleSeatPositions.Add(SeatPosition);
                        }
                        float ClosestDistance = 99f;
                        Vector3 ClosestPosition = Vector3.Zero;
                        foreach(Vector3 possiblePos in PossibleSeatPositions)
                        {
                            float distanceto = Player.Character.Position.DistanceTo2D(possiblePos);
                            if(distanceto < ClosestDistance)
                            {
                                ClosestPosition = possiblePos;
                                ClosestDistance = distanceto;
                            }
                        }
                        SeatEntryPosition = ClosestPosition;
                        SeatEntryHeading = DesiredHeading;
                        return true;
                    }
                }
                SeatEntryPosition = DesiredPos;
                SeatEntryHeading = DesiredHeading;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool MoveToSeatCoordinates()
        {
            if (Settings.SettingsManager.ActivitySettings.TeleportWhenSitting)
            {
                StoredPlayerPosition = Player.Character.Position;
                StoredPlayerHeading = Player.Character.Heading;
                Game.FadeScreenOut(500, true);
                Player.Character.Position = SeatEntryPosition;
                Player.Character.Heading = SeatEntryHeading;
                Game.FadeScreenIn(500, true);
                return true;
            }
            else
            {
                bool IsFacingDirection = false;
                bool IsCloseEnough = false;
                NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, SeatEntryPosition.X, SeatEntryPosition.Y, SeatEntryPosition.Z, 1.0f, -1, SeatEntryHeading, Settings.SettingsManager.ActivitySettings.SittingSlideDistance);

                //NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, SeatEntryPosition.X, SeatEntryPosition.Y, SeatEntryPosition.Z, 1.0f, -1, Settings.SettingsManager.ActivitySettings.SittingSlideDistance, 0, SeatEntryHeading);


                uint SeatHash = 0;
                string SeatName = "";
                if(ClosestSittableEntity.Exists())
                {
                    SeatHash = ClosestSittableEntity.Model.Hash;
                    SeatName = ClosestSittableEntity.Model.Name;
                }
                //EntryPoint.WriteToConsoleTestLong($"SeatOffset {SeatOffset} SeatHash {SeatHash} SeatName {SeatName}");
                uint GameTimeGotClose = 0;
                SetCollisionDisabled();

                while (!IsCancelled)
                {
                    float SeatDistance = Game.LocalPlayer.Character.DistanceTo2D(SeatEntryPosition);
                    float HeadingDifference = Math.Abs(Extensions.GetHeadingDifference(Game.LocalPlayer.Character.Heading, SeatEntryHeading));
                    if (SeatDistance <= 0.2f && HeadingDifference <= 0.5f)
                    {
                        IsCloseEnough = true;
                        IsFacingDirection = true;
                        EntryPoint.WriteToConsole("SITTING BREAK FOR CLOSE");
                        break;
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                    }
                    if(SeatDistance <= Settings.SettingsManager.ActivitySettings.ForceSitDistance && GameTimeGotClose == 0)
                    {
                        GameTimeGotClose = Game.GameTime;
                        EntryPoint.WriteToConsole("SITTING GAMETIMEGOT CLOSE");
                    }
                    if(GameTimeGotClose != 0 && Game.GameTime - GameTimeGotClose >= Settings.SettingsManager.ActivitySettings.ForceSitTimeOut && !IsCloseEnough && Settings.SettingsManager.ActivitySettings.ForceSitWhenClose)
                    {
                        Game.LocalPlayer.Character.Position = SeatEntryPosition;
                        Game.LocalPlayer.Character.Heading = SeatEntryHeading;
                        EntryPoint.WriteToConsole("SITTING FORCING POS");
                    }





//#if DEBUG
//                    Game.DisplaySubtitle($"SeatOffset {SeatOffset} SeatHash {SeatHash} SeatName {SeatName} SeatDistance {SeatDistance} HeadingDifference {HeadingDifference} ");
//                    Rage.Debug.DrawArrowDebug(SeatEntryPosition, Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
//#endif
                    GameFiber.Yield();
                }
                GameFiber.Sleep(250);
                if (IsCloseEnough && IsFacingDirection && !IsCancelled)
                {
                    return true;
                }
                else
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    return false;
                }
            }
        }

        private void SetCollisionDisabled()
        {
           CollisionObjects = new List<Rage.Object>();
            List<Rage.Object> Objects = World.GetAllObjects().ToList(); //EntryPoint.ModController.AllObjects.ToList();//World.GetAllObjects().ToList();
            foreach (Rage.Object obj in Objects)
            {
                if (obj.Exists() && ClosestSittableEntity.Exists())
                {
                    string modelName = obj.Model.Name.ToLower();
                    uint hash = obj.Model.Hash;
                    if (obj.Handle != ClosestSittableEntity.Handle)
                    {
                        float DistanceToObject = obj.DistanceTo(SeatEntryPosition);
                        if (DistanceToObject <= 3f)//
                        {
                            CollisionObjects.Add(obj);
                            NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(obj, Player.Character, true);

                            //obj.CollisionIgnoredEntity = Game.LocalPlayer.Character;
                        }
                    }
                }
            }
        }

        private void StartNewIdleScene()
        {
            if (RandomItems.RandomPercent(50))
            {
                PlayingDict = Data.AnimIdleDictionary;
                PlayingAnim = Data.AnimIdle.PickRandom();
            }
            else
            {
                PlayingDict = Data.AnimIdleDictionary2;
                PlayingAnim = Data.AnimIdle2.PickRandom();
            }

            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;


            //if (IsSittingOnToilet)
            //{
            //    Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(Settings.SettingsManager.DebugSettings.SynchedSceneOffsetX, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetY, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetZ)); //new Vector3(Position.X, Position.Y, Position.Z);
            //}

            PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
            NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
        }
        private void StartNewBaseScene()
        {
            PlayingDict = Data.AnimBaseDictionary;
            PlayingAnim = Data.AnimBase;
            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;

            //if(IsSittingOnToilet)
            //{
            //    Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(Settings.SettingsManager.DebugSettings.SynchedSceneOffsetX, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetY, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetZ)); //new Vector3(Position.X, Position.Y, Position.Z);
            //}

            PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
            NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
        }
        private void Setup()
        {
            string AnimBase;
            string AnimBaseDictionary;
            string AnimEnter;
            string AnimEnterDictionary;
            string AnimExit;
            string AnimExitDictionary;
            List<string> AnimIdle;
            string AnimIdleDictionary;
            List<string> AnimIdle2;
            string AnimIdleDictionary2;
            if (UseMaleAnimations)
            {
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair@male@generic@base";
                if (EnterForward)
                {   
                    AnimEnter = "enter_forward";
                    AnimEnterDictionary = "amb@prop_human_seat_chair@male@generic@enter";
                }
                else
                {
                    AnimEnter = "enter_back";
                    AnimEnterDictionary = "amb@prop_human_seat_chair@male@generic@react_aggressive";
                }        
                AnimExit = "exit_forward";
                AnimExitDictionary = "amb@prop_human_seat_chair@male@generic@exit";


                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair@male@generic@idle_a";
                AnimIdle2 = new List<string>() { "idle_d", "idle_e" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair@male@generic@idle_b";



            }
            else
            {
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair@female@legs_crossed@base";
                if(EnterForward)
                {
                    AnimEnter = "enter_fwd";
                    AnimEnterDictionary = "amb@prop_human_seat_chair@female@legs_crossed@enter";
                }
                else
                {
                    AnimEnter = "enter_back";
                    AnimEnterDictionary = "amb@prop_human_seat_chair@female@legs_crossed@react_coward";
                } 
                AnimExit = "exit_fwd";
                AnimExitDictionary = "amb@prop_human_seat_chair@female@legs_crossed@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair@female@legs_crossed@idle_a";
                AnimIdle2 = new List<string>() { "idle_d", "idle_e" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair@female@legs_crossed@idle_b";
            }


            //if (IsSittingOnToilet)
            //{
            //    AnimBase = "trevonlav_baseloop";
            //    AnimBaseDictionary = "timetable@trevor@on_the_toilet";
            //    AnimIdle = new List<string>() { "trevonlav_comehere", "trevonlav_backedup", "trevonlav_midwife", "trevonlav_struggleloop" };
            //    AnimIdleDictionary = "timetable@trevor@on_the_toilet";
            //    AnimIdle2 = new List<string>() { "trevonlav_comehere", "trevonlav_backedup", "trevonlav_midwife", "trevonlav_struggleloop" };
            //    AnimIdleDictionary2 = "timetable@trevor@on_the_toilet";
            //    AnimExit = "trev_on_toilet_exit";
            //    AnimExitDictionary = "switch@trevor@on_toilet";
            //}


            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary2);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SittingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, AnimIdle2, AnimIdleDictionary2);
        }
    }
}