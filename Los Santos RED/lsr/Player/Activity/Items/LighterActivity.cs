using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class LighterActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private uint GameTimeStartedHolding;
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
        private Rage.Object Lighter;
        private bool IsAttachedToHand;
        private string HandBoneName = "BONETAG_L_PH_HAND";
        private Vector3 HandOffset = new Vector3();
        private Rotator HandRotator = new Rotator();
        private string PropModelName = "p_amb_brolly_01";
        private bool hasStartedAnimation;
        private LighterItem LighterItem;
        private Vector3 FlameOffset;
        private Rotator FlameRotator;
        private LoopedParticle LighterFlameParticle;


        private bool IsLit = false;
        private bool isLit = false;
        public LighterActivity(IActionable player, LighterItem lighterItem) : base()
        {
            Player = player;
            ModItem = lighterItem;
            LighterItem = lighterItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Lighter";
        public override string CancelPrompt { get; set; } = "Put Away Lighter";
        public override string ContinuePrompt { get; set; } = "Continue Lighter";
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
            //EntryPoint.WriteToConsole($"Lighter Start");
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
            }, "LighterActivity");
        }
        public override bool CanPerform(IActionable player)
        {
            if (player.ActivityManager.CanPerformActivitiesExtended)
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
            AttachToHand();
            if (animEnter != "")
            {
                GameTimeStartedHolding = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animEnter, animEnterBlendIn, animEnterBlendOut, -1, animEnterFlag, 0, false, false, false);//-1
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && Game.GameTime - GameTimeStartedHolding <= 5000)
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
                GameTimeStartedHolding = Game.GameTime;
                NativeFunction.Natives.SET_PED_CAN_PLAY_AMBIENT_ANIMS(Player.Character, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDictionary, animIdle, animIdleBlendIn, animIdleBlendOut, -1, animIdleFlag, 0, false, false, false);//-1
                Player.ButtonPrompts.AddPrompt("Lighter", "Toggle", "LighterToggle", GameControl.Attack, 10);
                while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
                {
                    ControlTick();
                    Player.WeaponEquipment.SetUnarmed();
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDictionary, animIdle);
                    if (AnimationTime > 0.0f)
                    {
                        hasStartedAnimation = true;
                    }
                    if (AnimationTime == 0.0 && hasStartedAnimation)
                    {
                        IsCancelled = true;
                    }
                    GameFiber.Yield();
                }
                Player.ButtonPrompts.RemovePrompts("Lighter");
                NativeFunction.Natives.SET_PED_CAN_PLAY_AMBIENT_ANIMS(Player.Character, true);
            }
            Exit();
        }

        private void ControlTick()
        {
            DisableControls();

            if (Player.IsShowingActionWheel)
            {
                return;
            }

            if (Player.ButtonPrompts.IsPressed("LighterToggle"))
            {
                IsLit = !IsLit;
                OnIsLitChanged();
            }
        }

        private void Exit()
        {
            if (animExit != "")
            {
                GameTimeStartedHolding = Game.GameTime;
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
            if (Lighter.Exists())
            {
                Lighter.Delete();
            }
            if(LighterFlameParticle != null)
            {
                LighterFlameParticle.Stop();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.ActivityManager.IsPerformingActivity = false;
        }
        private void AttachToHand()
        {
            CreateLighter();
            if (Lighter.Exists() && !IsAttachedToHand)
            {
                Lighter.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp.Add(Lighter);
            }
        }
        private void CreateLighter()
        {
            if (!Lighter.Exists() && PropModelName != "")
            {
                try
                {
                    Lighter = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception ex)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (!Lighter.Exists())
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
            HandBoneName = "BONETAG_R_PH_HAND";
            animIdleBlendIn = 4.0f;
            animIdleBlendOut = -4.0f;
            animDictionary = "move_strafe@melee_small_weapon_fps";// "anim @amb@casino@hangout@ped_male@stand_withdrink@01a@base";
            animIdle = "idle";// "base";
            HandOffset = new Vector3(0.0f, 0.0f, 0.0f);
            HandRotator = new Rotator(0f, 0f, 0f);
            FlameOffset = new Vector3(0.0f, 0.0f, 0.0f);
            FlameRotator = new Rotator(0f, 0f, 0f);



            if (ModItem != null)
            {
                PropModelName = ModItem.ModelItem.ModelName;
                PropAttachment handAttachment = ModItem.ModelItem.Attachments.Where(x => x.Name == "RightHand").FirstOrDefault();
                if (handAttachment != null)
                {
                    HandBoneName = handAttachment.BoneName;
                    HandOffset = handAttachment.Attachment;
                    HandRotator = handAttachment.Rotation;
                }


                PropAttachment flamesAttachment = ModItem.ModelItem.Attachments.Where(x => x.Name == "Flames").FirstOrDefault();
                if (flamesAttachment != null)
                {
                    FlameOffset = flamesAttachment.Attachment;
                    FlameRotator = flamesAttachment.Rotation;
                }

            }
            AnimationDictionary.RequestAnimationDictionay(animDictionary);
        }

        private void OnIsLitChanged()
        {
            if(IsLit)
            {
                if (Lighter.Exists())
                {
                    LighterFlameParticle = new LoopedParticle("scr_safehouse", "scr_sh_lighter_flame", Lighter, FlameOffset, FlameRotator, 1.0f);
                }
            }
            else
            {
                if(LighterFlameParticle != null)
                {
                    LighterFlameParticle.Stop();
                }
            }
            isLit = IsLit;
        }
        private void DisableControls()
        {
            Game.DisableControlAction(0, GameControl.Attack, true);// false);
            Game.DisableControlAction(0, GameControl.Attack2, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);// false);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);// false);


            Game.DisableControlAction(0, GameControl.Aim, true);// false);
            Game.DisableControlAction(0, GameControl.VehicleAim, true);// false);
            Game.DisableControlAction(0, GameControl.AccurateAim, true);// false);
            Game.DisableControlAction(0, GameControl.VehiclePassengerAim, true);// false);

        }

    }
}