﻿using ExtensionsMethods;
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
        private string AnimIdleDictionary;
        private string AnimIdle;
        private int HandBoneID;
        private Vector3 HandOffset;
        private Rotator HandRotator;
        private Rage.Object Bottle;
        private bool IsAttachedToHand;
        private IIntoxicatable Player;
        private IntoxicatingEffect IntoxicatingEffect;
        private string PropModel;
        private float CurrentAnimationTime;
        private string DebugLocation;
        private bool IsCancelControlPressed => Game.IsControlPressed(0, GameControl.Sprint) || Game.IsControlPressed(0, GameControl.Jump);// || Game.IsControlPressed(0, GameControl.VehicleExit);
        public DrinkingActivity(IIntoxicatable consumable) : base()
        {
            Player = consumable;
        }
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsConsuming} I: {Player.IntoxicatedIntensity} L: {DebugLocation} AT {Math.Round(CurrentAnimationTime,2)}";
        private void Setup()
        {
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                AnimIdleDictionary = "amb@world_human_drinking@beer@male@idle_a";
                AnimIdle = new List<string>() { "idle_c", "Idle_a" }.PickRandom();
                HandBoneID = 57005;
                HandOffset = new Vector3(0.12f, 0.0f, -0.06f);
                HandRotator = new Rotator(-77.0f, 23.0f, 0.0f);
            }
            else
            {
                AnimIdleDictionary = "amb@world_human_drinking@beer@female@idle_a";
                AnimIdle = new List<string>() { "idle_c", "Idle_a" }.PickRandom();
                HandBoneID = 57005;
                HandOffset = new Vector3(0.12f, 0.0f, -0.06f);
                HandRotator = new Rotator(-77.0f, 23.0f, 0.0f);
            }
            PropModel = new List<string>() { "prop_cs_beer_bot_40oz", "prop_cs_beer_bot_40oz_02", "prop_cs_beer_bot_40oz_03" }.PickRandom(); ;
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
        }
        public override void Start()
        {
            Setup();
            IntoxicatingEffect = new IntoxicatingEffect(Player, 5.0f, 25000, 60000, "Drunk");//25000
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
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimIdleDictionary, AnimIdle, 8.0f, -8.0f, -1, 49, 0, false, false, false);//-1
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