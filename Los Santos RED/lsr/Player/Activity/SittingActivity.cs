using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class SittingActivity : DynamicActivity
    {
        private string PlayingAnim;
        private string PlayingDict;
        private SittingData Data;

        private SittingData EatingData;
        private SittingData DrinkingData;

        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private ISettingsProvideable Settings;
        private bool IsActivelySitting = false;
        private Vector3 StartingPosition;
        private int PlayerScene;
        private List<uint> SeatModels = new List<uint>() { 0x6ba514ac, 0x7facd66f, 0xc0a6cbcd, 0x534bc1bc, 0xa55359b8 };
        private Entity ClosestEntity;
        private IModItems ModItems;
        private Rage.Object Prop;
        private bool IsConsuming = false;
        //private ModItem ConsumeableModItem;
        public SittingActivity(IIntoxicatable consumable, ISettingsProvideable settings, IModItems modItems) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItems = modItems;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsPerformingActivity} I: {Player.IntoxicatedIntensity}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
        }
        public override void Continue()
        {
        }
        public override void Start()
        {
            EntryPoint.WriteToConsole("Sitting Activity Started", 5);
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "Sitting");
        }
        private void Enter()
        {
            EntryPoint.WriteToConsole("Sitting Activity Enter", 5);
            Player.SetUnarmed();
            Player.IsPerformingActivity = true;
            PlayingDict = Data.AnimEnterDictionary;
            PlayingAnim = Data.AnimEnter;
            bool Continue = false;
            List<Rage.Object> Objects = World.GetAllObjects().ToList();
            float ClosestDistance = 999f;
            foreach(Rage.Object obj in Objects)
            {
                if(obj.Exists() && obj.Model.Name.ToLower().Contains("chair") || obj.Model.Name.ToLower().Contains("bench") || obj.Model.Name.ToLower().Contains("seat") || SeatModels.Contains(obj.Model.Hash))
                {
                    float DistanceToObject = obj.DistanceTo2D(Game.LocalPlayer.Character.Position);
                    if (DistanceToObject <= 5f && DistanceToObject <= ClosestDistance)
                    {
                        ClosestEntity = obj;
                        ClosestDistance = DistanceToObject;
                    }
                }
            }
            if (ClosestEntity.Exists())
            {
                EntryPoint.WriteToConsole($"Sitting Closest = {ClosestEntity.Model.Name}", 5);
                ClosestEntity.IsPositionFrozen = true;
                Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
                DesiredPos = new Vector3(DesiredPos.X, DesiredPos.Y, Game.LocalPlayer.Character.Position.Z);
                float DesiredHeading = Math.Abs(ClosestEntity.Heading + 180f);
                float ObjectHeading = ClosestEntity.Heading;
                if(ClosestEntity.Heading >= 180f)
                {
                    DesiredHeading = ClosestEntity.Heading - 180f;
                }
                else
                {
                    DesiredHeading = ClosestEntity.Heading + 180f;
                }

                NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, DesiredPos.X, DesiredPos.Y, DesiredPos.Z, 1.0f, -1, DesiredHeading, 0.2f);
                uint GameTimeStartedSitting = Game.GameTime;
                float heading = Game.LocalPlayer.Character.Heading;
                bool IsFacingDirection = false;
                bool IsCloseEnough = false;
                while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough)
                {
                    IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(DesiredPos) < 0.2f;
                    GameFiber.Yield();
                }
                GameTimeStartedSitting = Game.GameTime;
                while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection)
                {
                    heading = Game.LocalPlayer.Character.Heading;
                    if (Math.Abs(Extensions.GetHeadingDifference(heading, DesiredHeading)) <= 2.0f)
                    {
                        IsFacingDirection = true;
                        EntryPoint.WriteToConsole($"Sitting FACING TRUE {Game.LocalPlayer.Character.DistanceTo(DesiredPos)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading} {ObjectHeading}", 5);
                    }
                    GameFiber.Yield();
                }
                if (IsCloseEnough && IsFacingDirection)
                {
                    Continue = true;
                    EntryPoint.WriteToConsole($"Sitting IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPos)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading} {ObjectHeading}", 5);
                }
                else
                {
                    EntryPoint.WriteToConsole($"Sitting NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPos)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading} {ObjectHeading}", 5);
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"Sitting nothing CLOSE!", 5);
            }
            if(!Continue)
            {
                Player.IsPerformingActivity = false;
                if(ClosestEntity.Exists())
                {
                    ClosestEntity.IsPositionFrozen = false;
                }
                return;
            }
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
                Player.SetUnarmed();
                GameFiber.Yield();
            }
            if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) >= 0.95f)
            {
                IsActivelySitting = true;
            }
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
            EntryPoint.WriteToConsole("Sitting Activity Idle", 5);
            StartNewBaseScene();
            float AnimationTime;
            while (Player.CanPerformActivities && !IsCancelled)
            {
                AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                if(AnimationTime >= 1.0f)
                {
                    if(ModItem != null)
                    {
                        if(IsConsuming)
                        {

                            EntryPoint.WriteToConsole("Sitting Activity Consumed Item, Removing", 5);
                            ModItem = null;
                            IsConsuming = false;
                            if (Prop.Exists())
                            {
                                Prop.Delete();
                                IsAttachedToHand = false;
                            }
                        }


                        if(ModItem?.Type == eConsumableType.Drink)
                        {
                            IsConsuming = true;
                            StartNewDrinkingScene();
                        }
                        else if (ModItem?.Type == eConsumableType.Eat)
                        {
                            IsConsuming = true;
                            StartNewEatingScene();
                        }
                        else
                        {
                            IsConsuming = false;
                            StartNewIdleScene();
                        }
                    }
                    else
                    {
                        IsConsuming = false;
                        StartNewIdleScene();
                    }
                }
                if(Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }
                Player.SetUnarmed();
                GameFiber.Yield();
            }
            Exit();
        }
        private void Exit()
        {
            EntryPoint.WriteToConsole("Sitting Activity Exit", 5);
            IsConsuming = false;
            IsAttachedToHand = false;
            if (Prop.Exists())
            {
                Prop.Detach();
                IsAttachedToHand = false;
            }

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
                    Player.SetUnarmed();
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole("Sitting Activity Exit 2", 5);
            }
            EntryPoint.WriteToConsole("Sitting Activity Exit 3", 5);
            AnimationDictionary.RequestAnimationDictionay("ped");
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);

            if (ClosestEntity.Exists())
            {
                ClosestEntity.IsPositionFrozen = false;
            }


            GameFiber.Yield();
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.IsPerformingActivity = false;

            GameFiber.Sleep(5000);
            if (Prop.Exists())
            {
                Prop.Delete();
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


            EntryPoint.WriteToConsole($"Sitting Activity Started New Idle {PlayingAnim}", 5);
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
            EntryPoint.WriteToConsole($"Sitting Activity Started New Base {PlayingAnim}", 5);
        }
        private void StartNewEatingScene()
        {
            if (Prop.Exists())
            {
                Prop.Delete();
                IsAttachedToHand = false;
            }
            if(ModItem != null)
            {
                AttachItemToHand();
            }
            PlayingDict = EatingData.AnimIdleDictionary;
            PlayingAnim = EatingData.AnimIdle.PickRandom();
            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
            NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            EntryPoint.WriteToConsole($"Sitting Activity Started New Eating {PlayingAnim}", 5);
        }
        private void StartNewDrinkingScene()
        {
            if (Prop.Exists())
            {
                Prop.Delete();
                IsAttachedToHand = false;
            }
            if (ModItem != null)
            {

                AttachItemToHand();
            }

            PlayingDict = DrinkingData.AnimIdleDictionary;
            PlayingAnim = DrinkingData.AnimIdle.PickRandom();

            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
            NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
            NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            EntryPoint.WriteToConsole($"Sitting Activity Started New Drinking {PlayingAnim}", 5);
        }
        private void AttachItemToHand()
        {
            EntryPoint.WriteToConsole($"Sitting Activity AttachItemToHand Start", 5);
            CreateProp();
            EntryPoint.WriteToConsole($"Sitting Activity AttachItemToHand 2", 5);
            if (Prop.Exists() && !IsAttachedToHand)
            {
                EntryPoint.WriteToConsole($"Sitting Activity AttachItemToHand Start", 5);
                Prop.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, ModItem.ModelItem.AttachBoneIndex), ModItem.ModelItem.AttachOffset, ModItem.ModelItem.AttachRotation);
                IsAttachedToHand = true;
                EntryPoint.WriteToConsole($"Sitting Activity Attached Prop TO Hand", 5);
            }
        }
        private void CreateProp()
        {
            if (ModItem.ModelItem.ModelName != "")
            {
                try
                {
                    //Vector3 position = Player.Character.GetOffsetPositionUp(50f);
                    //Model modelToCreate = new Model(Game.GetHashKey(Data.PropModelName));
                    //modelToCreate.LoadAndWait();
                    //Food = NativeFunction.Natives.CREATE_OBJECT<Rage.Object>(Game.GetHashKey(Data.PropModelName), position.X, position.Y, position.Z, 0f);
                    Prop = new Rage.Object(ModItem.ModelItem.ModelName, Player.Character.GetOffsetPositionUp(50f));
                    GameFiber.Yield();
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {ModItem.ModelItem.ModelName}");
                }
                if (Prop.Exists())
                {
                    Prop.IsGravityDisabled = false;
                    EntryPoint.WriteToConsole($"Sitting Activity AttachItemToHand Prop Exists", 5);
                }
                else
                {
                    IsCancelled = true;
                }
            }
        }
        private void Setup()
        {
            EntryPoint.WriteToConsole("Sitting Activity SETUP RAN", 5);
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

            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUPO MALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair@male@generic@base";
                AnimEnter = "enter_forward";
                AnimEnterDictionary = "amb@prop_human_seat_chair@male@generic@enter";
                AnimExit = "exit_forward";
                AnimExitDictionary = "amb@prop_human_seat_chair@male@generic@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair@male@generic@idle_a";
                AnimIdle2 = new List<string>() { "idle_d", "idle_e" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair@male@generic@idle_b";
            }
            else
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUP FEMALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair@female@legs_crossed@base";
                AnimEnter = "enter_fwd";
                AnimEnterDictionary = "amb@prop_human_seat_chair@female@legs_crossed@enter";
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
            EntryPoint.WriteToConsole("Sitting Activity Loaded Dicts", 5);
            Data = new SittingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, AnimIdle2, AnimIdleDictionary2);
            EntryPoint.WriteToConsole("Sitting Activity Data Created", 5);

            SetupEating();
            SetupDrinking();
        }
        private void SetupEating()
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
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUPO MALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair_food@male@base";
                AnimEnter = "base";
                AnimEnterDictionary = "amb@prop_human_seat_chair_food@male@base";
                AnimExit = "exit";
                AnimExitDictionary = "amb@prop_human_seat_chair_food@male@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair_food@male@idle_a";
                AnimIdle2 = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair_food@male@idle_a";
            }
            else
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUP FEMALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair_food@female@base";
                AnimEnter = "base";
                AnimEnterDictionary = "amb@prop_human_seat_chair_food@female@base";
                AnimExit = "exit";
                AnimExitDictionary = "amb@prop_human_seat_chair_food@female@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair_food@female@idle_a";
                AnimIdle2 = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair_food@female@idle_a";
            }
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 0", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 1", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 2", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 3", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary2);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 4", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 5", 5);

            EntryPoint.WriteToConsole("Sitting Activity Loaded Dicts", 5);
            EatingData = new SittingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, AnimIdle2, AnimIdleDictionary2);
            EntryPoint.WriteToConsole("Sitting Activity Data Created", 5);
        }
        private void SetupDrinking()
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
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUPO MALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair_drink@male@generic@base";
                AnimEnter = "base";
                AnimEnterDictionary = "amb@prop_human_seat_chair_drink@male@generic@base";
                AnimExit = "exit";
                AnimExitDictionary = "amb@prop_human_seat_chair_drink@male@generic@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                AnimIdleDictionary = "amb@prop_human_seat_chair_drink@male@generic@idle_a";
                AnimIdle2 = new List<string>() { "idle_d", "idle_e", "idle_f" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair_drink@male@generic@idle_b";
            }
            else
            {
                EntryPoint.WriteToConsole("Sitting Activity SETUP FEMALE", 5);
                AnimBase = "base";
                AnimBaseDictionary = "amb@prop_human_seat_chair_drink@female@generic@base";
                AnimEnter = "enter";
                AnimEnterDictionary = "amb@prop_human_seat_chair_drink@female@generic@enter";
                AnimExit = "exit_fwd";
                AnimExitDictionary = "amb@prop_human_seat_chair_drink@female@generic@exit";
                AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c", "idle_d" };
                AnimIdleDictionary = "amb@prop_human_seat_chair_drink@female@generic@idle_a";
                AnimIdle2 = new List<string>() { "idle_a", "idle_b", "idle_c", "idle_d" };
                AnimIdleDictionary2 = "amb@prop_human_seat_chair_drink@female@generic@idle_a";
            }
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 0", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 1", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 2", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 3", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary2);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 4", 5);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            EntryPoint.WriteToConsole("Sitting Activity Eating Loaded Dicts 5", 5);
            EntryPoint.WriteToConsole("Sitting Activity Loaded Dicts", 5);
            DrinkingData = new SittingData(AnimBase, AnimBaseDictionary, AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, AnimIdle2, AnimIdleDictionary2);
            EntryPoint.WriteToConsole("Sitting Activity Data Created", 5);
        }
    }
}