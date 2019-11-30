using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Smoking
{
    private static SmokingAnimation StandardCigaretteMale;
    private static SmokingAnimation StandardCigaretteFemale;
    private static SmokingAnimation StandardCigarettePlayerZero;
    private static Random rnd;
    private static int BreathingInterval = 3500;
    private static CigaretteAnimation CurrentAnimation = CigaretteAnimation.None;

    //Temp ppublic
    public static SmokingAnimation CurrentSmokingAnimation;
    public static SmokingAnimation.IdleAnimation CurrentIdleAnimation;
    public static float CurrentPuffingAnimationTime;
    public static bool CurrentPuffingAnimationNearMouth;


    private static uint GameTimeLastEmitSmoke;
    private static LoopedParticle SmokingParticle;
    private static bool EmittingSmoke = false;


    public static bool IsRunning { get; set; } = true;
    public static Rage.Object PlayersCurrentCigarette { get; set; } = null;
    public static bool PlayersCurrentCigaretteIsLit { get; set; } = false;
    public static CigarettePosition CurrentAttachedPosition { get; set; } = CigarettePosition.None;
    static Smoking()
    {
        rnd = new Random();
    }
    public enum CigarettePosition
    {
        None = -1,
        Hand = 0,
        Mouth = 1,
    }
    public enum CigaretteAnimation
    {
        None = -1,
        Start = 0,
        Puffing = 1,
        Exit = 2,
        ExitStayInMouth = 3,
    }
    public static void Initialize()
    {
        Setup();
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
        if(SmokingParticle != null && SmokingParticle.IsValid())
            SmokingParticle.Stop();
        if (PlayersCurrentCigarette != null && PlayersCurrentCigarette.Exists())
            PlayersCurrentCigarette.Delete();
    }
    private static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    if (InstantAction.IsDead || Game.LocalPlayer.Character.IsRagdoll)
                    {
                        RemoveCigarette();
                        return;
                    }
                    UpdateAnimations();
                    CheckEmitSmoke();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void CheckEmitSmoke()
    {
        if (CurrentAttachedPosition == CigarettePosition.Hand)
        {
            if (Game.GameTime - GameTimeLastEmitSmoke >= 200 && PlayersCurrentCigarette.Exists() && PlayersCurrentCigaretteIsLit && CurrentPuffingAnimationNearMouth)
            {
                CreateSmoke(CurrentIdleAnimation.DragTime);
                GameTimeLastEmitSmoke = Game.GameTime;
            }
        }
        else if (CurrentAttachedPosition == CigarettePosition.Mouth)
        {
            if (Game.GameTime - GameTimeLastEmitSmoke >= BreathingInterval && PlayersCurrentCigarette.Exists() && PlayersCurrentCigaretteIsLit && CurrentPuffingAnimationTime == 0f)
            {
                CreateSmoke(rnd.Next(550, 950));
                BreathingInterval = rnd.Next(4500, 6500);
                GameTimeLastEmitSmoke = Game.GameTime;
            }
        }
    }
    private static void UpdateAnimations()
    {
        if(CurrentIdleAnimation == null)
        {
            CurrentPuffingAnimationTime = 0f;
            CurrentPuffingAnimationNearMouth = false;
            return;
        }

        if (CurrentAnimation == CigaretteAnimation.Puffing)
        {
            CurrentPuffingAnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, CurrentIdleAnimation.Dictionary, CurrentIdleAnimation.Animation);
        }
        else
        {
            CurrentPuffingAnimationTime = 0f;
        }
        
        if (CurrentPuffingAnimationTime >= CurrentIdleAnimation.PuffigAnimationPercentages[0].Item1 && CurrentPuffingAnimationTime <= CurrentIdleAnimation.PuffigAnimationPercentages[0].Item2)
            CurrentPuffingAnimationNearMouth = true;
        else if (CurrentPuffingAnimationTime >= CurrentIdleAnimation.PuffigAnimationPercentages[1].Item1 && CurrentPuffingAnimationTime <= CurrentIdleAnimation.PuffigAnimationPercentages[1].Item2)//0.725f && 0.75f
            CurrentPuffingAnimationNearMouth = true;
        else
            CurrentPuffingAnimationNearMouth = false;
    }
    public static bool CancelSmoking
    {
        get
        {
            if (Game.IsControlPressed(0, GameControl.Aim) || Game.IsControlPressed(0, GameControl.Sprint) || Game.IsControlPressed(0, GameControl.Enter))
                return true;
            else
                return false;
        }
    }
    public static void Start()
    {
        if (CurrentAnimation == CigaretteAnimation.Puffing || CurrentAnimation == CigaretteAnimation.Start)
            return;
        GameFiber SmokingStart = GameFiber.StartNew(delegate
        {
            Debugging.WriteToLog("Smoking", string.Format("StartedSmoking {0}", true));
            SetPedUnarmed(Game.LocalPlayer.Character, false);
            bool Cancel = false;
            CurrentSmokingAnimation = GetSmokingAnimationForPed(Game.LocalPlayer.Character);
            if (!PlayersCurrentCigarette.Exists())
            {
                Debugging.WriteToLog("Smoking", string.Format("PlayersCurrentCigarette.Exists {0}", false));
                PlayersCurrentCigarette = new Rage.Object(CurrentSmokingAnimation.PropName, Game.LocalPlayer.Character.GetOffsetPositionUp(50f));
                if (CurrentAttachedPosition != CigarettePosition.Mouth)
                {
                    Debugging.WriteToLog("Smoking", string.Format("CurrentAttachedPosition {0}", CurrentAttachedPosition));
                    Cancel = !PutCigaretteInMouth(false);
                }
            }
            if (Cancel)
            {
                Debugging.WriteToLog("Smoking", string.Format("Cancel after PutInMouth {0}", Cancel));
                return;        
            }
            Cancel = !StartPuffingCigarette();
            if (Cancel)
            {
                Debugging.WriteToLog("Smoking", string.Format("Cancel after StartPuffing {0}", Cancel));
                Stop();
                return;
            }
            while (!Game.IsControlPressed(0, GameControl.Aim) && !Game.IsControlPressed(0, GameControl.Enter) && !Game.IsControlPressed(0, GameControl.Sprint))
            {
                GameFiber.Sleep(50);
            }
            Debugging.WriteToLog("Smoking", string.Format("Cancel after waiting after puffing {0}", Cancel));
            Stop();
        }, "SmokingStart");
        Debugging.GameFibers.Add(SmokingStart);
    }
    public static void Stop()
    {
        if (!PlayersCurrentCigarette.Exists())
            return;

        StopPuffingCigarette(false);
    }
    public static void StopWithAnimation()
    {
        if (!PlayersCurrentCigarette.Exists())
            return;

        SetPedUnarmed(Game.LocalPlayer.Character, false);

        bool CigInHand = false;
        if(CurrentAnimation != CigaretteAnimation.Puffing)
        {
            CigInHand = StartPuffingCigarette();
        }
        if (CigInHand)
        {
            while (CurrentPuffingAnimationTime <= 0.57f)
                GameFiber.Yield();
        }
        StopPuffingCigarette(false);
    }
    public static bool PutCigaretteInMouth(bool ClearTasks)
    {
        SetPedUnarmed(Game.LocalPlayer.Character, false);
        InstantAction.RequestAnimationDictionay(CurrentSmokingAnimation.EnterAnimationDictionary);
        CurrentAnimation = CigaretteAnimation.Start;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, CurrentSmokingAnimation.EnterAnimationDictionary, CurrentSmokingAnimation.EnterAnimation, 4.0f, -4.0f, -1, 48, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 2500 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        if(CancelSmoking)
        {
            Debugging.WriteToLog("PutCigaretteInMouth", string.Format("FirstCancel {0}", CancelSmoking));
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            return false;
        }
        AttachCigaretteToPedLowerLip(Game.LocalPlayer.Character);
        GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 7500 && !CancelSmoking)
        {
            if(Game.GameTime - GameTimeStartedAnimation >= 3000)
                PlayersCurrentCigaretteIsLit = true;
            GameFiber.Yield();
        }
        if (CancelSmoking)
        {
            Debugging.WriteToLog("PutCigaretteInMouth", string.Format("SecondCanel {0}", CancelSmoking));
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            if(!PlayersCurrentCigaretteIsLit)
                RemoveCigarette();
            return false;
        }
        if (ClearTasks)
        {
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        PlayersCurrentCigaretteIsLit = true;
        Debugging.WriteToLog("PutCigaretteInMouth", string.Format("Return {0}", true));
        return true;
    }
    public static bool StartPuffingCigarette()
    {
        CurrentIdleAnimation = CurrentSmokingAnimation.IdleAnimations.PickRandom();
        Debugging.WriteToLog("CurrentIdleAnimation", string.Format("CurrentIdleAnimation.Animation {0}", CurrentIdleAnimation.Animation));
        InstantAction.RequestAnimationDictionay(CurrentIdleAnimation.Dictionary);
        CurrentAnimation = CigaretteAnimation.Puffing;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, CurrentIdleAnimation.Dictionary, CurrentIdleAnimation.Animation, 4.0f, -4.0f, -1, 49, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (!CurrentPuffingAnimationNearMouth && !CancelSmoking) //while (Game.GameTime - GameTimeStartedAnimation <= 2000 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        if (CancelSmoking)
        {
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            return false;
        }
        AttachCigaretteToPedRightHand(Game.LocalPlayer.Character);
        return true;
    }
    public static void StopPuffingCigarette(bool PlayAnimation)
    {
        if (PlayAnimation)
        {
            InstantAction.RequestAnimationDictionay(CurrentSmokingAnimation.ExitAnimationDictionary);
            CurrentAnimation = CigaretteAnimation.Exit;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, CurrentSmokingAnimation.ExitAnimationDictionary, CurrentSmokingAnimation.ExitAnimation, 4.0f, -4.0f, -1, 48, 0, false, false, false);
            uint GameTimeStartedAnimation = Game.GameTime;
            while (Game.GameTime - GameTimeStartedAnimation <= 1500 && !CancelSmoking)
            {
                GameFiber.Yield();
            }
        }
        if(CurrentPuffingAnimationNearMouth)
        {
            AttachCigaretteToPedLowerLip(Game.LocalPlayer.Character);
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        else if(PlayersCurrentCigaretteIsLit && CurrentAttachedPosition == CigarettePosition.Mouth)
        {
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        else
        {
            RemoveCigarette();
        }

        //CurrentAnimation = CigaretteAnimation.None;
        //CurrentAttachedPosition = CigarettePosition.None;
        //PlayersCurrentCigarette.Detach();    
        //Game.LocalPlayer.Character.Tasks.Clear();
        //PlayersCurrentCigaretteIsLit = false;
        //GameFiber.Sleep(5000);
        //if (PlayersCurrentCigarette.Exists())
        //{
        //    PlayersCurrentCigarette.Delete();
        //    PlayersCurrentCigarette = null;
        //}
    }
    public static void AttachCigaretteToPedRightHand(Ped Pedestrian)
    {
        if (!PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette = new Rage.Object(CurrentSmokingAnimation.PropName, Pedestrian.GetOffsetPositionUp(50f));
            InstantAction.CreatedObjects.Add(PlayersCurrentCigarette);
        }
        //Right Hand
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, CurrentSmokingAnimation.HandBoneToAttach);//58868 SKEL_R_Finger20 //57005 right hand
        PlayersCurrentCigarette.AttachTo(Pedestrian, BoneIndexRightHand, CurrentSmokingAnimation.HandAttachPosition, CurrentSmokingAnimation.HandAttachRotation);
        //Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.03f, -0.02f, 0.01f), new Rotator(0f, 0f, 90f));//Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.14f, 0.03f, 0.0f), new Rotator(0.49f, 79f, 79f));//Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.13f, 0.02f, 0.0f), new Rotator(0.49f, 79f, 79f));
        CurrentAttachedPosition = CigarettePosition.Hand;
    }
    public static void AttachCigaretteToPedLowerLip(Ped Pedestrian)
    {
        if (!PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette = new Rage.Object(CurrentSmokingAnimation.PropName, Pedestrian.GetOffsetPositionUp(50f));
            InstantAction.CreatedObjects.Add(PlayersCurrentCigarette);
        }
        int BoneIndexLowerLip = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, CurrentSmokingAnimation.MouthBoneToAttach);//17188 ll root--//20623 LL reg
        PlayersCurrentCigarette.AttachTo(Pedestrian, BoneIndexLowerLip, CurrentSmokingAnimation.MouthAttachPosition, CurrentSmokingAnimation.MouthAttachRotation); //Cigarette.AttachTo(Pedestrian, BoneIndexLowerLip, new Vector3(0.03f, 0.01f, 0.0f), new Rotator(0.0f, -127f, 0f)); //Cigarette.AttachTo(Pedestrian, BoneIndexLowerLip, new Vector3(0.02f, 0.0f, 0.0f), new Rotator(0.0f, -58f, -164f));//.03,.01 -127f
        CurrentAttachedPosition = CigarettePosition.Mouth;
    }
    public static void CreateSmoke(int DelayAfter)
    {
        if (EmittingSmoke)
            return;
        try
        {
            GameFiber CreateSmoke = GameFiber.StartNew(delegate
            {
                EmittingSmoke = true;
                string Dictionary = "core";//"scr_mp_cig";
                string FX = "ent_anim_cig_smoke";//"ent_amb_cig_smoke_linger";// "ent_anim_cig_exhale_mth_car";
                SmokingParticle = new LoopedParticle(Dictionary, FX, PlayersCurrentCigarette, new Vector3(-0.07f, 0.0f, 0f), Rotator.Zero, 1.5f);
                GameFiber.Sleep(DelayAfter);
                SmokingParticle.Stop();
                EmittingSmoke = false;
            });
            Debugging.GameFibers.Add(CreateSmoke);
        }
        catch(Exception e)
        {
            Debugging.WriteToLog("Smoking", e.Message + " : " + e.StackTrace);
        }
    }
    public static void RemoveCigarette()
    {
        if (!PlayersCurrentCigarette.Exists())
            return;
        CurrentAnimation = CigaretteAnimation.None;
        CurrentAttachedPosition = CigarettePosition.None;
        PlayersCurrentCigarette.Detach();
        if (Game.LocalPlayer.Character.IsAlive)
        {
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        PlayersCurrentCigaretteIsLit = false;
        GameFiber.Sleep(5000);
        if (PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette.Delete();
            PlayersCurrentCigarette = null;
        }
    }
    public static void SetPedUnarmed(Ped Pedestrian, bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
        }
        if (SetCantChange)
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
    }
    private static SmokingAnimation GetSmokingAnimationForPed(Ped MyPed)
    {
        if (MyPed.isMainCharacter())
            return StandardCigarettePlayerZero;
        else if (MyPed.IsMale)
            return StandardCigaretteMale;
        else if (!MyPed.IsMale)
            return StandardCigaretteFemale;
        else
            return StandardCigaretteMale;
    }
    private static void Setup()
    {
        List<SmokingAnimation.IdleAnimation> MaleIdles = new List<SmokingAnimation.IdleAnimation>() { new SmokingAnimation.IdleAnimation("amb@world_human_smoking@male@male_a@idle_a", "idle_b", new List<Tuple<float, float>>() { new Tuple<float, float>(0.1f, 0.2f), new Tuple<float, float>(0.7f, 0.8f) },1500)
                                                                                                 ,new SmokingAnimation.IdleAnimation("amb@world_human_smoking@male@male_a@idle_a", "idle_c", new List<Tuple<float, float>>() { new Tuple<float, float>(0.1f, 0.2f), new Tuple<float, float>(0.7f, 0.8f) },1500) };
        StandardCigaretteMale = new SmokingAnimation("ng_proc_cigarette01a", "amb@world_human_smoking@male@male_a@enter", "enter", "amb@world_human_smoking@male@male_a@idle_a", MaleIdles, "amb@world_human_smoking@male@male_a@exit", "exit",new Vector3(-0.07f, 0.0f, 0f), 1.5f, 17188, new Vector3(0.046f, 0.015f, 0.014f), new Rotator(0.0f, -180f, 0f), 57005, new Vector3(0.14f, 0.03f, 0.0f), new Rotator(0.49f, 79f, 79f));
        StandardCigarettePlayerZero = new SmokingAnimation("ng_proc_cigarette01a", "amb@world_human_smoking@male@male_a@enter", "enter", "amb@world_human_smoking@male@male_a@idle_a", MaleIdles, "amb@world_human_smoking@male@male_a@exit", "exit", new Vector3(-0.07f, 0.0f, 0f), 1.5f, 31086, new Vector3(-0.007f, 0.13f, 0.01f), new Rotator(0.0f, -175f, 91f), 57005, new Vector3(0.1640f, 0.019f, 0.0f), new Rotator(0.49f, 79f, 79f));

        List<SmokingAnimation.IdleAnimation> FemaleIdles = new List<SmokingAnimation.IdleAnimation>() { new SmokingAnimation.IdleAnimation("amb@world_human_smoking@female@idle_a", "idle_a", new List<Tuple<float, float>>() { new Tuple<float, float>(0.3f, 0.7f) },1500)
                                                                                                        ,new SmokingAnimation.IdleAnimation("amb@world_human_smoking@female@idle_a", "idle_b", new List<Tuple<float, float>>() { new Tuple<float, float>(0.6f, 0.7f) },1500)
                                                                                                 ,new SmokingAnimation.IdleAnimation("amb@world_human_smoking@female@idle_a", "idle_c", new List<Tuple<float, float>>() { new Tuple<float, float>(0.1f, 0.19f) },500) };
        StandardCigaretteFemale = new SmokingAnimation("ng_proc_cigarette01a", "amb@world_human_smoking@female@enter", "enter", "amb@world_human_smoking@male@male_a@idle_a", FemaleIdles, "amb@world_human_smoking@female@exit", "exit", new Vector3(-0.07f, 0.0f, 0f), 1.5f, 17188, new Vector3(0.046f, 0.015f, 0.014f), new Rotator(0.0f, -180f, 0f), 57005, new Vector3(0.14f, 0.01f, 0.0f), new Rotator(0.49f, 79f, 79f));
    }
    public class SmokingAnimation//temp puiblic
    {
        public SmokingAnimation(string propName, string enterAnimationDictionary, string enterAnimation, string idleAnimationDictionary, List<IdleAnimation> idleAnimations, string exitAnimationDictionary, string exitAnimation, Vector3 smokePosition, float smokeScale, int mouthBoneToAttach, Vector3 mouthAttachPosition, Rotator mouthAttachRotation, int handBoneToAttach, Vector3 handAttachPosition, Rotator handAttachRotation)
        {
            PropName = propName;
            EnterAnimationDictionary = enterAnimationDictionary;
            EnterAnimation = enterAnimation;
            IdleAnimationDictionary = idleAnimationDictionary;
            IdleAnimations = idleAnimations;
            ExitAnimationDictionary = exitAnimationDictionary;
            ExitAnimation = exitAnimation;
            SmokePosition = smokePosition;
            SmokeScale = smokeScale;
            MouthBoneToAttach = mouthBoneToAttach;
            MouthAttachPosition = mouthAttachPosition;
            MouthAttachRotation = mouthAttachRotation;
            HandBoneToAttach = handBoneToAttach;
            HandAttachPosition = handAttachPosition;
            HandAttachRotation = handAttachRotation;
        }

        public string PropName { get; set; }// = "ng_proc_cigarette01a";
        public string EnterAnimationDictionary { get; set; }
        public string EnterAnimation { get; set; }
        public string IdleAnimationDictionary { get; set; }
        public List<IdleAnimation> IdleAnimations { get; set; } = new List<IdleAnimation>();
        public string ExitAnimationDictionary { get; set; }
        public string ExitAnimation { get; set; }
        public Vector3 SmokePosition { get; set; }
        public float SmokeScale { get; set; }
        public int MouthBoneToAttach { get; set; }
        public Vector3 MouthAttachPosition { get; set; }
        public Rotator MouthAttachRotation { get; set; }
        public int HandBoneToAttach { get; set; }
        public Vector3 HandAttachPosition { get; set; }
        public Rotator HandAttachRotation { get; set; }

        public class IdleAnimation
        {
            public IdleAnimation(string dictionary, string animation, List<Tuple<float,float>> puffigAnimationPercentages,int dragTime)
            {
                Dictionary = dictionary;
                Animation = animation;
                PuffigAnimationPercentages = puffigAnimationPercentages;
                DragTime = dragTime;
            }

            public string Dictionary { get; set; }
            public string Animation { get; set; }
            public List<Tuple<float,float>> PuffigAnimationPercentages { get; set; }
            public int DragTime { get; set; }
        }

    }

    internal class LoopedParticle// : IHandleable
    {
        public string AssetName { get; }
        public string ParticleName { get; }

        public PoolHandle Handle { get; }

        public LoopedParticle(string assetName, string particleName, Ped ped, PedBoneId bone, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedOnPedBone<uint>(particleName,
                                                                                 ped,
                                                                                 offset.X, offset.Y, offset.Z,
                                                                                 rotation.Pitch, rotation.Roll, rotation.Yaw,
                                                                                 ped.GetBoneIndex(bone),
                                                                                 scale,
                                                                                 false, false, false);
        }

        public LoopedParticle(string assetName, string particleName, Entity entity, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedOnEntity<uint>(particleName,
                                                                                entity,
                                                                                offset.X, offset.Y, offset.Z,
                                                                                rotation.Pitch, rotation.Roll, rotation.Yaw,
                                                                                scale,
                                                                                false, false, false);
        }

        public LoopedParticle(string assetName, string particleName, Entity entity, int boneIndex, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.xC6EB449E33977F0B<uint>(particleName,
                                                                    entity,
                                                                    offset.X, offset.Y, offset.Z,
                                                                    rotation.Pitch, rotation.Roll, rotation.Yaw,
                                                                    boneIndex,
                                                                    scale,
                                                                    false, false, false); // _START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE
        }

        public LoopedParticle(string assetName, string particleName, Entity entity, string boneName, Vector3 offset, Rotator rotation, float scale)
            : this(assetName, particleName, entity, entity.GetBoneIndex(boneName), offset, rotation, scale)
        {
        }

        public LoopedParticle(string assetName, string particleName, Vector3 position, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedAtCoord<uint>(particleName,
                                                                               position.X, position.Y, position.Z,
                                                                               rotation.Pitch, rotation.Roll, rotation.Yaw,
                                                                               scale,
                                                                               false, false, false, false);
        }

        private void LoadAsset()
        {
            NativeFunction.Natives.RequestNamedPtfxAsset(AssetName);
            int waitCounter = 10;
            while (!NativeFunction.Natives.HasNamedPtfxAssetLoaded<bool>(AssetName) && waitCounter > 0)
            {
                GameFiber.Sleep(10);
                waitCounter--;
            }

            NativeFunction.Natives.x6C38AF3693A69A91(AssetName); // _SET_PTFX_ASSET_NEXT_CALL
        }

        public void SetOffsets(Vector3 offset, Rotator rotation)
        {
            NativeFunction.Natives.SetParticleFxLoopedOffsets(Handle.Value,
                                                              offset.X, offset.Y, offset.Z,
                                                              rotation.Pitch, rotation.Roll, rotation.Yaw);
        }

        public void SetColor(Color color)
        {
            NativeFunction.Natives.SetParticleFxLoopedColour(Handle.Value, color.R / 255f, color.G / 255f, color.B / 255f, false);
            NativeFunction.Natives.SetParticleFxLoopedAlpha(Handle.Value, color.A / 255f);
        }

        public void SetScale(float scale)
        {
            NativeFunction.Natives.SetParticleFxLoopedScale(Handle.Value, scale);
        }

        public void SetRange(float range)
        {
            NativeFunction.Natives.xDCB194B85EF7B541(Handle.Value, range); // _SET_PARTICLE_FX_LOOPED_RANGE
        }

        public bool IsValid()
        {
            return NativeFunction.Natives.DoesParticleFxLoopedExist<bool>(Handle.Value);
        }

        public void Stop()
        {
            NativeFunction.Natives.StopParticleFxLooped(Handle.Value, false);
        }
    }
}

