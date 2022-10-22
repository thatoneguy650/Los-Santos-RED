using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;

namespace LosSantosRED.lsr.Player
{
    public class UmbrellaActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private uint GameTimeStartedHoldingUmbrella;
        private string animDictionary;
        private string animEnter;
        private string animIdle;
        private string animExit;
        private int animEnterFlag = (int)(AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animExitFlag = (int)(AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private float animEnterBlendIn = 4.0f;
        private float animIdleBlendIn = 4.0f;
        private float animExitBlendIn = 4.0f;
        private float animEnterBlendOut = -4.0f;
        private float animIdleBlendOut = -4.0f;
        private bool SetEndFrame;
        private float animExitBlendOut = -4.0f;
        private Rage.Object Umbrella;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "p_amb_brolly_01";
        public UmbrellaActivity(IActionable player) : base()
        {
            Player = player;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Umbrella";
        public override string CancelPrompt { get; set; } = "Drop Umbrella";
        public override string ContinuePrompt { get; set; } = "Continue Umbrella";
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
            EntryPoint.WriteToConsole($"Umbrella Start", 5);
            GameFiber UmbrellaWatcher = GameFiber.StartNew(delegate
            {
                Player.ActivityManager.UmbrellaTimes++;
                Setup();
                Enter();
            }, "UmbrellaActivity");
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            AttachUmbrellaToHand();
            if (animEnter != "")
            {
                EntryPoint.WriteToConsole($"Umbrella Enter: {animEnter}", 5);
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivities && !IsCancelled && Game.GameTime - GameTimeStartedHoldingUmbrella <= 5000)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animEnter);
                    GameFiber.Yield();
                }
            }
            Idle();
        }
        private void Idle()
        {
            if (animIdle != "")
            {
                EntryPoint.WriteToConsole($"Umbrella Idle: {animIdle}", 5);
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animIdle, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1


                if(SetEndFrame)
                {
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, animDictionary, animIdle, 0.99f);
                }

                while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animIdle);
                    GameFiber.Yield();
                }
            }
            Exit();
        }
        private void Exit()
        {
            try
            {
                if (animExit != "")
                {
                    EntryPoint.WriteToConsole($"Umbrella Exit: {animExit}", 5);
                    GameTimeStartedHoldingUmbrella = Game.GameTime;
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animExit, animExitBlendIn, animExitBlendOut, -1, animExitFlag, 0, false, false, false);//-1
                    while (Player.ActivityManager.CanPerformActivities && !IsCancelled)
                    {
                        Player.WeaponEquipment.SetUnarmed();
                        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animIdle);
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
            if (Umbrella.Exists())
            {
                Umbrella.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
            if (1==1)
            {
                GameFiber.Sleep(5000);
            }
            if (Umbrella.Exists())
            {
                Umbrella.Delete();
            }
            
        }

        private void AttachUmbrellaToHand()
        {
            CreateUmbrella();
            if (Umbrella.Exists() && !IsAttachedToHand)
            {
                Umbrella.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Umbrella;
            }
        }
        private void CreateUmbrella()
        {
            if (!Umbrella.Exists() && PropModelName != "")
            {
                try
                {
                    Umbrella = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Umbrella.Exists())
                {
                    IsCancelled = true;
                }
            }
        }


        private void Setup()
        {
            if(Player.ActivityManager.UmbrellaTimes > 1)
            {
                Player.ActivityManager.UmbrellaTimes = 0;
            }
            animDictionary = "";
            animEnter = "";
            animIdle = "";
            animExit = "";

            HandBoneName = "BONETAG_L_PH_HAND";
            SetEndFrame = false;
            animIdleBlendIn = 4.0f;
            animIdleBlendOut = -4.0f;
            PropModelName = "p_amb_brolly_01_s";
            HandOffset = new Vector3(0.005f, 0.045f, 0f);
            HandRotator = new Rotator(0f, -180f, 0f);
            //if (Player.ActivityManager.UmbrellaTimes == 0)
            //{
            //    animDictionary = "doors@";
            //    animIdle = "door_sweep_l_hand_medium";
            //}
            //else if (Player.ActivityManager.UmbrellaTimes == 1)
            //{
                animDictionary = "anim@amb@casino@hangout@ped_male@stand_withdrink@01a@base";
                animIdle = "base";
                HandOffset = new Vector3(-0.01f, 0.01f, 0.05f);
                HandRotator = new Rotator(0f, -40f, 0f);
            //}
            AnimationDictionary.RequestAnimationDictionay(animDictionary);
        }
    }
}