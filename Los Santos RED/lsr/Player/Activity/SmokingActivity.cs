using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;

namespace LosSantosRED.lsr.Player
{
    public class SmokingActivity : ConsumeActivity
    {
        private bool IsPot;
        private bool DoneIdleLoop;
        private string DebugLocation;
        private float DistanceBetweenHandAndFace;
        private float DistanceBetweenJointAndFace;
        private uint GameTimeLastEmittedSmoke;
        private bool HandByFace;
        private IntoxicatingEffect IntoxicatingEffect;
        private bool IsActivelySmoking;
        private bool IsAttachedToMouth;
        private bool IsEmittingSmoke;
        private Rage.Object Joint;
        private bool JointNearMouth;
        private bool JointIsLit;
        private IConsumableIntoxicatable Player;
        private LoopedParticle PotSmoke;
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
        private int MouthBoneID;
        private Vector3 MouthOffset;
        private Rotator MouthRotator;
        private string JointProp;
        private float CurrentAnimationTime = 0.0f;
        private bool IsCancelControlPressed => Game.IsControlPressed(0, GameControl.Sprint) || Game.IsControlPressed(0, GameControl.Jump);// || Game.IsControlPressed(0, GameControl.VehicleExit);
        public SmokingActivity(IConsumableIntoxicatable consumable, bool isPot) : base()
        {
            Player = consumable;
            IsPot = isPot;
        }
        public override string DebugString => $"IsIntoxicated {Player.IsIntoxicated} IsConsuming: {Player.IsConsuming} Intensity: {Player.IntoxicatedIntensity} Loop: {DebugLocation}, Emitting: {IsEmittingSmoke}, InMouth: {JointNearMouth}, HandByFace: {HandByFace}";// + IntoxicatingEffect != null ? IntoxicatingEffect.DebugString : "";
        public override void Start()
        {
            Setup();
            if(IsPot)
            {
                IntoxicatingEffect = new IntoxicatingEffect(Player, 2.5f, 10000, 60000);
                IntoxicatingEffect.Start();
            }
            else
            {
                IntoxicatingEffect = new IntoxicatingEffect(Player, 1.1f, 10000, 60000);
                IntoxicatingEffect.Start();
            }
            GameFiber SmokingWatcher = GameFiber.StartNew(delegate
            {
                LightJoint();
            }, "SmokingWatcher");
        }
        private void Setup()
        {
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two")
            {
                AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                AnimIdle = "idle_b";
                AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.1640f, 0.019f, 0.0f);
                HandRotator = new Rotator(0.49f, 79f, 79f);
                MouthBoneID = 31086;
                MouthOffset = new Vector3(-0.007f, 0.13f, 0.01f);
                MouthRotator = new Rotator(0.0f, -175f, 91f);
            }
            else if (Player.IsMale)
            {
                AnimBaseDictionary = "amb@world_human_smoking@male@male_a@base";
                AnimBase = "base";
                AnimIdleDictionary = "amb@world_human_smoking@male@male_a@idle_a";
                AnimIdle = "idle_b";
                AnimEnterDictionary = "amb@world_human_smoking@male@male_a@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_smoking@male@male_a@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.14f, 0.03f, 0.0f);
                HandRotator = new Rotator(0.49f, 79f, 79f);
                MouthBoneID = 17188;
                MouthOffset = new Vector3(0.046f, 0.015f, 0.014f);
                MouthRotator = new Rotator(0.0f, -180f, 0f);
            }
            else
            {
                AnimBaseDictionary = "amb@world_human_smoking@female@idle_a";
                AnimBase = "idle_c";
                AnimIdleDictionary = "amb@world_human_smoking@female@idle_a";
                AnimIdle = "idle_c";
                AnimEnterDictionary = "amb@world_human_smoking@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_smoking@female@exit";
                AnimExit = "exit";
                HandBoneID = 57005;
                HandOffset = new Vector3(0.14f, 0.01f, 0.0f);
                HandRotator = new Rotator(0.49f, 79f, 79f);
                MouthBoneID = 17188;
                MouthOffset = new Vector3(0.046f, 0.015f, 0.014f);
                MouthRotator = new Rotator(0.0f, -180f, 0f);
            }
            if(IsPot)
            {
                JointProp = "p_amb_joint_01";
            }
            else
            {
                JointProp = "ng_proc_cigarette01a";
            }
            AnimationDictionary.RequestAnimationDictionay(AnimBaseDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
        }
        private void AttachJointToHand()
        {
            CreateJoint();
            if (Joint.Exists() && IsAttachedToMouth)
            {
                Joint.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, HandBoneID), HandOffset, HandRotator);
                IsAttachedToMouth = false;
            }
        }
        private void AttachJointToMouth()
        {
            CreateJoint();
            if (Joint.Exists() && !IsAttachedToMouth)
            {
                Joint.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, MouthBoneID), MouthOffset, MouthRotator);
                IsAttachedToMouth = true;
            }
        }
        private void CreateJoint()
        {
            if (!Joint.Exists())
            {
                Joint = new Rage.Object(JointProp, Player.Character.GetOffsetPositionUp(50f));
            }
        }
        private void LightJoint()
        {
            DebugLocation = "Light";
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimEnterDictionary, AnimEnter, 8.0f, -8.0f, -1, 50, 0, false, false, false);//-1
            while (!IsCancelControlPressed && CurrentAnimationTime < 1.0f)
            {
                UpdatePosition();
                UpdateSmoke();
                CurrentAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, AnimEnterDictionary, AnimEnter);
                if (CurrentAnimationTime >= 0.18f)
                {
                    Player.IsConsuming = true;
                    AttachJointToMouth();
                }
                if (CurrentAnimationTime >= 0.55f)
                {
                    IsActivelySmoking = true;
                    JointIsLit = true;
                }
                if (CurrentAnimationTime >= 0.72f)
                {
                    AttachJointToHand();
                }
                GameFiber.Yield();
            }
            if (IsCancelControlPressed)
            {
                Stop();
            }
            else
            {
                Puff();
            }
        }
        private void Puff()
        {
            DebugLocation = "Puff";
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, AnimBaseDictionary, AnimBase, 8.0f, -8.0f, -1, 49, 0, false, false, false);//49
            while (!IsCancelControlPressed)
            {
                UpdatePosition();
                UpdateSmoke();
                if (HandByFace)
                {
                    AttachJointToHand();
                }
                GameFiber.Yield();
            }
            if (JointNearMouth)
            {
                Idle();
            }
            else
            {
                Stop();
            }
        }
        private void Idle()
        {
            DebugLocation = "Idle";
            AttachJointToMouth();
            Player.Character.Tasks.Clear();
            IsActivelySmoking = false;      
            while (!Game.IsKeyDownRightNow(System.Windows.Forms.Keys.E) && !Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Q))
            {
                UpdatePosition();
                UpdateSmoke();
                Game.DisplayHelp("Press E to stop smoking ~n~Press Q to continue smoking");
                GameFiber.Yield();
            }
            if(Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Q))
            {
                IsActivelySmoking = true;
                Puff();
            }
            else
            {
                Stop();
            }
        }
        private void Stop()
        {
            DebugLocation = "Stop";
            HandByFace = false;
            IsActivelySmoking = false;
            JointIsLit = false;
            IsAttachedToMouth = false;
            if (Joint.Exists())
            {
                Joint.Detach();
            }
            Player.Character.Tasks.Clear();
            Player.IsConsuming = false;
            GameFiber.Sleep(5000);
            if (Joint.Exists())
            {
                Joint.Delete();
            }
        }
        private void UpdatePosition()
        {
            if (Joint.Exists())
            {
                DistanceBetweenJointAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)).DistanceTo2D(Joint);
                DistanceBetweenHandAndFace = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, HandBoneID)).DistanceTo2D(NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, MouthBoneID)));
            }
            if (DistanceBetweenJointAndFace <= 0.17f && Joint.Exists())
            {
                JointNearMouth = true;
            }
            else
            {
                JointNearMouth = false;
            }
            if (DistanceBetweenHandAndFace <= 0.2f)
            {
                HandByFace = true;
            }
            else
            {
                HandByFace = false;
            }
        }
        private void UpdateSmoke()
        {
            if (JointIsLit)
            {
                if (IsActivelySmoking)
                {
                    if (JointNearMouth && JointIsLit && !IsEmittingSmoke)
                    {
                        PotSmoke = new LoopedParticle("core", "ent_anim_cig_smoke", Joint, new Vector3(-0.07f, 0.0f, 0f), Rotator.Zero, 1.5f);
                        IsEmittingSmoke = true;
                    }
                    if (!JointNearMouth && IsEmittingSmoke && PotSmoke != null)
                    {
                        PotSmoke.Stop();
                        IsEmittingSmoke = false;
                    }
                }
                else
                {
                    if (Game.GameTime - GameTimeLastEmittedSmoke >= 500 && IsEmittingSmoke)
                    {
                        IsEmittingSmoke = false;
                        PotSmoke.Stop();
                    }
                    else if (Game.GameTime - GameTimeLastEmittedSmoke >= 1500 && !IsEmittingSmoke)
                    {
                        IsEmittingSmoke = true;
                        PotSmoke = new LoopedParticle("core", "ent_anim_cig_smoke", Joint, new Vector3(-0.07f, 0.0f, 0f), Rotator.Zero, 1.5f);
                        GameTimeLastEmittedSmoke = Game.GameTime;
                    }
                }
            }
        }
    }
}