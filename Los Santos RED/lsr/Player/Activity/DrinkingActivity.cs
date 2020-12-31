using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    public class DrinkingActivity : ConsumeActivity
    {
        private string AnimBaseDictionary;
        private string AnimBase;
        private string AnimIdleDictionary;
        private string AnimIdle;
        private string AnimEnterDictionary;
        private string AnimEnter;
        private string AnimExitDictionary;
        private string AnimExit;
        private int HandBoneID;
        private Vector3 HandOffset;
        private Rotator HandRotator;
        private Rage.Object Bottle;
        private bool IsAttachedToHand;
        private IConsumableIntoxicatable Player;
        private IntoxicatingEffect IntoxicatingEffect;
        private string PropModel;
        private float CurrentAnimationTime;
        private string DebugLocation;
        private bool IsCancelControlPressed => Game.IsControlPressed(0, GameControl.Sprint) || Game.IsControlPressed(0, GameControl.Jump);// || Game.IsControlPressed(0, GameControl.VehicleExit);
        public DrinkingActivity(IConsumableIntoxicatable consumable) : base()
        {
            Player = consumable;
        }
        public override string DebugString => $"IsIntoxicated {Player.IsIntoxicated} IsConsuming: {Player.IsConsuming} Intensity: {Player.IntoxicatedIntensity} Loop: {DebugLocation}";
        private void Setup()
        {
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two")
            {
                AnimBaseDictionary = "amb@world_human_drinking@beer@male@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_drinking@beer@male@idle_a";
                AnimIdle = "idle_b";
                AnimEnterDictionary = "amb@world_human_drinking@beer@male@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@male@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.1f, -0.13f, -0.05f);
                HandRotator = new Rotator(-83.07f, 9.0f, 0.0f);
            }
            else if (Player.IsMale)
            {
                AnimBaseDictionary = "amb@world_human_drinking@beer@male@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_drinking@beer@male@idle_a";
                AnimIdle = "idle_b";
                AnimEnterDictionary = "amb@world_human_drinking@beer@male@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@male@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.1f, -0.13f, -0.05f);
                HandRotator = new Rotator(-83.07f, 9.0f, 0.0f);
            }
            else
            {
                AnimBaseDictionary = "amb@world_human_drinking@beer@female@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_drinking@beer@female@idle_a";
                AnimIdle = "idle_b";
                AnimEnterDictionary = "amb@world_human_drinking@beer@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@female@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.1f, -0.13f, -0.05f);
                HandRotator = new Rotator(-83.07f, 9.0f, 0.0f);
            }
            PropModel = "prop_beer_bottle";
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
        }
        public override void Start()
        {
            Setup();
            IntoxicatingEffect = new IntoxicatingEffect(Player, 5.0f, 5000, 60000);
            IntoxicatingEffect.Start();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Drink();
            }, "DrinkingWatcher");
        }
        private void AttachBottleToHand()
        {
            CreateBottle();
            if (Bottle.Exists() && !IsAttachedToHand)
            {
                Bottle.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, HandBoneID), HandOffset, HandRotator);
                IsAttachedToHand = true;
            }
        }
        private void CreateBottle()
        {
            if (!Bottle.Exists())
            {
                Bottle = new Rage.Object(PropModel, Player.Character.GetOffsetPositionUp(50f));
                Bottle.IsGravityDisabled = false;
            }
        }
        private void Drink()
        {
            DebugLocation = "Drink";
            AttachBottleToHand();
            Player.IsConsuming = true;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimEnterDictionary, AnimEnter, 8.0f, -8.0f, -1, 50, 0, false, false, false);//-1
            while (!IsCancelControlPressed)// && CurrentAnimationTime < 1.0f)
            {
                CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, AnimEnterDictionary, AnimEnter);
                GameFiber.Yield();
            }
            if (IsCancelControlPressed)
            {
                Stop();
            }
            else
            {
                Idle();
            }
        }
        private void Idle()
        {
            DebugLocation = "Idle";
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimBaseDictionary, AnimIdle, 8.0f, -8.0f, -1, 49, 0, false, false, false);//49
            while (!IsCancelControlPressed)
            {
                GameFiber.Yield();
            }
            Stop();
        }
        private void Stop()
        {
            DebugLocation = "Stop";
            if (Bottle.Exists())
            {
                Bottle.Detach();
            }
            Player.Character.Tasks.Clear();
            Player.IsConsuming = false;
            GameFiber.Sleep(5000);
            if (Bottle.Exists())
            {
                Bottle.Delete();
            }
        }
    }
}
