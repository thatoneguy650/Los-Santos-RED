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
        private bool UseMaleAnimations => Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale;
        public SittingActivity(IActionable player, ISettingsProvideable settings, bool findSittingProp, bool enterForward, ISeats seats) : base()
        {
            Player = player;
            Settings = settings;
            FindSittingProp = findSittingProp;
            EnterForward = enterForward;
            Seats = seats;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = false;
        public override string PausePrompt { get; set; } = "Pause Activity";
        public override string CancelPrompt { get; set; } = "Stop Activity";
        public override string ContinuePrompt { get; set; } = "Continue Activity";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsSitting = false;
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
                Enter();
            }, "Sitting");
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.IsSitting = true;
            if (FindSittingProp)
            {
                if (!GetSittableProp() || !GetSeatCoordinates() || !MoveToSeatCoordinates())
                {
                    Player.IsSitting = false;
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
            StartNewBaseScene();
            float AnimationTime;
            while (Player.CanPerformActivities && !IsCancelled)
            {
                AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                if(AnimationTime >= 1.0f && !Player.IsPerformingActivity)
                {
                    StartNewIdleScene();
                }
                if(Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }
                Player.WeaponEquipment.SetUnarmed();
                if (PossibleCollisionTable.Exists() && ClosestSittableEntity.Exists() && PossibleCollisionTable.Handle != ClosestSittableEntity.Handle)
                {
                    NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Player.Character, PossibleCollisionTable, true);
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Exit()
        {
            Player.PauseCurrentActivity();
            if (Settings.SettingsManager.ActivitySettings.TeleportWhenSitting)
            {
                Game.FadeScreenOut(500, true);
                Player.Character.Position = StoredPlayerPosition;
                Player.Character.Heading = StoredPlayerHeading;
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Game.FadeScreenIn(500, true);
                Player.IsSitting = false;         
            }
            else
            {
                if (IsActivelySitting)
                {
                    PlayingDict = Data.AnimExitDictionary;
                    PlayingAnim = Data.AnimExit;
                    Vector3 Position = Game.LocalPlayer.Character.Position;
                    float Heading = Game.LocalPlayer.Character.Heading;
                    PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                    float AnimationTime = 0f;
                    while (AnimationTime < 1.0f)
                    {
                        AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
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
                Player.IsSitting = false;
                GameFiber.Sleep(5000);
                if (PossibleCollisionTable.Exists() && ClosestSittableEntity.Exists() && PossibleCollisionTable.Handle != ClosestSittableEntity.Handle)
                {
                    PossibleCollisionTable.IsPositionFrozen = false;
                }
            }
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
            while (Player.CanPerformActivities && !IsCancelled && AnimationTime < 1.0f)
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
            List<Rage.Object> Objects = World.GetAllObjects().ToList();
            float ClosestDistance = 999f;
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
                float offset = Seats.GetOffSet(ClosestSittableEntity);
                Vector3 DesiredPos = ClosestSittableEntity.GetOffsetPositionFront(offset);
                DesiredPos = new Vector3(DesiredPos.X, DesiredPos.Y, Game.LocalPlayer.Character.Position.Z);
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
                            Vector3 SeatPosition = ClosestSittableEntity.GetOffsetPosition(new Vector3((-1 * ModelWidth / 2) + XPosition,offset, 0f));
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
                NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, SeatEntryPosition.X, SeatEntryPosition.Y, SeatEntryPosition.Z, 1.0f, -1, SeatEntryHeading, 0.2f);
                uint GameTimeStartedSitting = Game.GameTime;
                float heading = Game.LocalPlayer.Character.Heading;
                bool IsFacingDirection = false;
                bool IsCloseEnough = false;
                while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
                {
                    if (PossibleCollisionTable.Exists() && PossibleCollisionTable.Handle != ClosestSittableEntity.Handle)
                    {
                        NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Player.Character, PossibleCollisionTable, true);
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                    }
                    IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(SeatEntryPosition) < 0.2f;
//#if DEBUG
//                    Rage.Debug.DrawArrowDebug(SeatEntryPosition + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
//#endif
                    GameFiber.Yield();
                }
                GameFiber.Sleep(250);
                GameTimeStartedSitting = Game.GameTime;
                while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
                {
                    if (PossibleCollisionTable.Exists() && PossibleCollisionTable.Handle != ClosestSittableEntity.Handle)
                    {
                        NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Player.Character, PossibleCollisionTable, true);
                    }
                    heading = Game.LocalPlayer.Character.Heading;
                    if (Math.Abs(Extensions.GetHeadingDifference(heading, SeatEntryHeading)) <= 0.5f)//0.5f)
                    {
                        IsFacingDirection = true;
                    }
                    if (Player.IsMoveControlPressed)
                    {
                        IsCancelled = true;
                    }
//#if DEBUG
//                    Rage.Debug.DrawArrowDebug(SeatEntryPosition + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
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
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary2);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new SittingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, AnimIdle2, AnimIdleDictionary2);
        }
    }
}