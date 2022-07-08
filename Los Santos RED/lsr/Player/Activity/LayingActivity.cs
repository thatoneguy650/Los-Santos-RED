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
    public class LayingActivity : DynamicActivity
    {
        private string PlayingAnim;
        private string PlayingDict;
        private LayingData Data;
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private bool IsActivelyLayingDown = false;
        private Vector3 StartingPosition;
        private int PlayerScene;
        private Entity ClosestLayableEntity;
        private Entity PossibleCollisionTable;
        private Vector3 StoredPlayerPosition;
        private float StoredPlayerHeading;
        private bool FindLayingDownProp = false;
        private bool isUsingVehicleAnimations;
        private float EnterBlendIn;
        private float EnterBlendOut;
        private float MaxExit = 1.0f;
        private uint GameTimeLastDidThing;

        public LayingActivity(IActionable player, ISettingsProvideable settings, bool findSittingProp) : base()
        {
            Player = player;
            Settings = settings;
            FindLayingDownProp = findSittingProp;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = false;
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsLayingDown = false;
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
            EntryPoint.WriteToConsole("Laying Activity Started", 5);
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "Laying");
        }
        private void Enter()
        {
            EntryPoint.WriteToConsole("Laying Activity Enter", 5);
            Player.SetUnarmed();
            Player.IsLayingDown = true;

            if (FindLayingDownProp)
            {
                GetLayableProp();
                if (!MoveToSeatCoordinates())
                {
                    Player.IsLayingDown = false;
                    if (ClosestLayableEntity.Exists())
                    {
                        ClosestLayableEntity.IsPositionFrozen = false;
                    }
                    return;
                }
            }
            LayDown();
            if (IsActivelyLayingDown)
            {
                Idle();
            }
            else
            {
                Exit();
            }
        }
        private void LayDown()
        {
            PlayingDict = Data.AnimEnterDictionary;
            PlayingAnim = Data.AnimEnter;
            StartingPosition = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            if (isUsingVehicleAnimations)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Data.AnimEnterBlendIn, Data.AnimEnterBlendOut, -1, Data.AnimEnterFlag, 0, false, false, false);//-1
            }
            else
            {
                PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", StartingPosition.X, StartingPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Data.AnimEnterDictionary, Data.AnimEnter, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            }
            float AnimationTime = 0f;
            while (Player.CanPerformActivities && !IsCancelled && AnimationTime < 1.0f)
            {
                if (isUsingVehicleAnimations)
                {
                    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                }
                else
                {
                    AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                }
                if (Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }


                if(Game.GameTime - GameTimeLastDidThing >= 1000)
                {
                    EntryPoint.WriteToConsole($"LAYING ANIMATION TIME {AnimationTime} isUsingVehicleAnimations {isUsingVehicleAnimations}");
                    GameTimeLastDidThing = Game.GameTime;
                }
                Player.SetUnarmed();
                GameFiber.Yield();
            }
            if (AnimationTime >= 0.2f)
            {
                IsActivelyLayingDown = true;
            }
        }
        private void Idle()
        {
            EntryPoint.WriteToConsole("Laying Activity Idle", 5);
            StartNewBaseScene();
            float AnimationTime;

            Player.IsResting = true;
            Player.IsSleeping = true;

            while (Player.CanPerformActivities && !IsCancelled)
            {
                if (isUsingVehicleAnimations)
                {
                    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                }
                else
                {
                    AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                }
                //if (AnimationTime >= 1.0f && !Player.IsPerformingActivity)
                //{
                //    StartNewIdleScene();
                //}
                if (Player.IsMoveControlPressed)
                {
                    IsCancelled = true;
                }
                Player.SetUnarmed();
                GameFiber.Yield();
            }



            Player.IsResting = false;
            Player.IsSleeping = false;

            Exit();
        }
        private void Exit()
        {
            EntryPoint.WriteToConsole("Sitting Activity Exit", 5);
            Player.PauseCurrentActivity();

            if (1==0)
            {
                Game.FadeScreenOut(500, true);
                Player.Character.Position = StoredPlayerPosition;
                Player.Character.Heading = StoredPlayerHeading;
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Game.FadeScreenIn(500, true);
                Player.IsLayingDown = false;

            }
            else
            {

                if (IsActivelyLayingDown && Data.AnimExitDictionary != "")
                {
                    PlayingDict = Data.AnimExitDictionary;
                    PlayingAnim = Data.AnimExit;
                    Vector3 Position = Game.LocalPlayer.Character.Position;
                    float Heading = Game.LocalPlayer.Character.Heading;
                    if (isUsingVehicleAnimations)
                    {
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Data.AnimExitBlendIn, Data.AnimExitBlendOut, -1, Data.AnimExitFlag, 0, false, false, false);//-1
                    }
                    else
                    {
                        PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                    }
                    float AnimationTime = 0f;
                    while (AnimationTime < MaxExit)
                    {
                        if (isUsingVehicleAnimations)
                        {
                            AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                        }
                        else
                        {
                            AnimationTime = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                        }
                        Player.SetUnarmed();
                        GameFiber.Yield();
                    }
                    EntryPoint.WriteToConsole("Laying Activity Exit 2", 5);
                }
                EntryPoint.WriteToConsole("Laying Activity Exit 3", 5);
                if (!isUsingVehicleAnimations)
                {
                    AnimationDictionary.RequestAnimationDictionay("ped");
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                }
                GameFiber.Yield();
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Player.IsLayingDown = false;
            }
        }

        private void GetLayableProp()
        {

        }
        private bool MoveToSeatCoordinates()
        {      
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
        private void StartNewIdleScene()
        {
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            if (isUsingVehicleAnimations)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Data.AnimIdletBlendIn, Data.AnimIdleBlendOut, -1, Data.AnimIdleFlag, 0, false, false, false);//-1
            }
            else
            {
                PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            }
            EntryPoint.WriteToConsole($"Sitting Activity Started New Idle {PlayingAnim}", 5);
        }
        private void StartNewBaseScene()
        {
            PlayingDict = Data.AnimBaseDictionary;
            PlayingAnim = Data.AnimBase;
            Vector3 Position = Game.LocalPlayer.Character.Position;
            float Heading = Game.LocalPlayer.Character.Heading;
            if (isUsingVehicleAnimations)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, Data.AnimBaseBlendIn, Data.AnimBaseBlendOut, -1, Data.AnimBaseFlag, 0, false, false, false);//-1
            }
            else
            {
                PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, Heading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
            }
            EntryPoint.WriteToConsole($"Sitting Activity Started New Base {PlayingAnim}", 5);
        }
        private void Setup()
        {
            EntryPoint.WriteToConsole("Sitting Activity SETUP RAN", 5);
            Data = new LayingData();
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                EntryPoint.WriteToConsole("Laying Activity SETUPO MALE", 5);
                Data.AnimBase = "f_getin_l_bighouse";
                Data.AnimBaseDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimEnter = "f_getin_l_bighouse";
                Data.AnimEnterDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimExit = "f_getout_l_bighouse";
                Data.AnimExitDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimIdle = new List<string>() { "f_sleep_l_loop_bighouse" };
                Data.AnimIdleDictionary = "anim@mp_bedmid@left_var_01";
            }
            else
            {
                EntryPoint.WriteToConsole("Laying Activity SETUP FEMALE", 5);
                Data.AnimBase = "f_getin_l_bighouse";
                Data.AnimBaseDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimEnter = "f_getin_l_bighouse";
                Data.AnimEnterDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimExit = "f_getout_l_bighouse";
                Data.AnimExitDictionary = "anim@mp_bedmid@left_var_01";
                Data.AnimIdle = new List<string>() { "f_sleep_l_loop_bighouse" };
                Data.AnimIdleDictionary = "anim@mp_bedmid@left_var_01";
            }

            isUsingVehicleAnimations = false;
            if (!Player.IsInVehicle)
            {
                Data.AnimBase = "base";
                Data.AnimBaseDictionary = "amb@world_human_bum_slumped@male@laying_on_left_side@base";
                Data.AnimBaseBlendIn = 1.0f;
                Data.AnimBaseBlendOut = -1.0f;
                Data.AnimBaseFlag = 2;

                Data.AnimEnter = "base";
                Data.AnimEnterDictionary = "amb@world_human_bum_slumped@male@laying_on_left_side@base";
                Data.AnimEnterBlendIn = 1.0f;
                Data.AnimEnterBlendOut = -1.0f;
                Data.AnimEnterFlag = 2;

                Data.AnimExit = "forward";
                Data.AnimExitDictionary = "amb@world_human_bum_slumped@male@laying_on_left_side@flee";
                Data.AnimExitFlag = 0;
                Data.AnimEnterBlendOut = -1.0f;

                MaxExit = 0.95f;
                Data.AnimIdle = new List<string>() { "idle_a", "idle_b", "idle_c" };
                Data.AnimIdleDictionary = "amb@world_human_bum_slumped@male@laying_on_left_side@idle_a";
                Data.AnimIdleFlag = 1;
                isUsingVehicleAnimations = true;
            }
            else
            {
                Data.AnimBase = "base_premier_michael";
                Data.AnimBaseDictionary = "switch@michael@sleep_in_car";
                Data.AnimBaseFlag = 50;
                Data.AnimBaseBlendIn = 1.0f;
                Data.AnimBaseBlendOut = -1.0f;

                Data.AnimEnter = "base_premier_michael";
                Data.AnimEnterDictionary = "switch@michael@sleep_in_car";
                Data.AnimEnterFlag = 50;
                Data.AnimEnterBlendIn = 1.0f;
                Data.AnimEnterBlendOut = -1.0f;

                Data.AnimIdle = new List<string>() { "base_premier_michael" };
                Data.AnimIdleDictionary = "switch@michael@sleep_in_car";
                Data.AnimIdleFlag = 50;
                isUsingVehicleAnimations = true;
            }

            AnimationDictionary.RequestAnimationDictionay(Data.AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(Data.AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(Data.AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(Data.AnimExitDictionary);

            EntryPoint.WriteToConsole("Laying Activity Data Created", 5);
        }
    }
}