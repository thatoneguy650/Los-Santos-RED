using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Linq;

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
        private int animEnterFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animIdleFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private int animExitFlag = (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
        private float animEnterBlendIn = 1.0f;
        private float animIdleBlendIn = 1.0f;
        private float animExitBlendIn = 1.0f;
        private float animEnterBlendOut = -1.0f;
        private float animIdleBlendOut = -1.0f;
        private float animExitBlendOut = -1.0f;
        private Rage.Object Umbrella;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "p_amb_brolly_01";
        private bool hasStartedAnimation;

        public UmbrellaActivity(IActionable player, ModItem modItem) : base()
        {
            Player = player;
            ModItem = modItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Umbrella";
        public override string CancelPrompt { get; set; } = "Put Away Umbrella";
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
            //EntryPoint.WriteToConsole($"Umbrella Start");
            GameFiber UmbrellaWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    Enter();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "UmbrellaActivity");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            Player.ActivityManager.IsPerformingActivity = true;
            AttachUmbrellaToHand();
            if (animEnter != "")
            {
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && Game.GameTime - GameTimeStartedHoldingUmbrella <= 5000)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animEnter);
                    if (AnimationTime >= 1.0f)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
            }
            Idle();
        }
        private void Idle()
        {
            if (animIdle != "")
            {
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.Natives.SET_PED_CAN_PLAY_AMBIENT_ANIMS(Player.Character, false);


                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animIdle, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animIdle);

                    if(AnimationTime > 0.0f)
                    {
                        hasStartedAnimation = true;
                    }
                    if (AnimationTime == 0.0 && hasStartedAnimation)
                    {
                        IsCancelled = true;
                    }
                    GameFiber.Yield();

                   //Game.DisplaySubtitle(AnimationTime.ToString());
                }

                NativeFunction.Natives.SET_PED_CAN_PLAY_AMBIENT_ANIMS(Player.Character, true);

            }
            Exit();
        }
        private void Exit()
        {
            if (animExit != "")
            {
                GameTimeStartedHoldingUmbrella = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animExit, animExitBlendIn, animExitBlendOut, -1, animExitFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animExit);
                    if (AnimationTime >= 1.0f)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
            }
            if (Umbrella.Exists())
            {
                Umbrella.Delete();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;            
        }

        private void AttachUmbrellaToHand()
        {
            CreateUmbrella();
            if (Umbrella.Exists() && !IsAttachedToHand)
            {
                Umbrella.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Umbrella);
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
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Umbrella.Exists())
                {
                    IsCancelled = true;
                }
            }
        }
        private void Setup()
        {
            animDictionary = "";
            animEnter = "";
            animIdle = "";
            animExit = "";
            HandBoneName = "BONETAG_L_PH_HAND";
            animIdleBlendIn = 4.0f;
            animIdleBlendOut = -4.0f;
            PropModelName = "p_amb_brolly_01_s";
            HandOffset = new Vector3(0.005f, 0.045f, 0f);
            HandRotator = new Rotator(0f, -180f, 0f);
            animDictionary = "anim@amb@casino@hangout@ped_male@stand_withdrink@01a@base";
            animIdle = "base";
            HandOffset = new Vector3(-0.01f, 0.01f, 0.05f);
            HandRotator = new Rotator(0f, -40f, 0f);
            if(ModItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                PropAttachment handAttachment = ModItem.ModelItem.Attachments.Where(x => x.Name == "LeftHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }
            }
            AnimationDictionary.RequestAnimationDictionay(animDictionary);
        }
    }
}