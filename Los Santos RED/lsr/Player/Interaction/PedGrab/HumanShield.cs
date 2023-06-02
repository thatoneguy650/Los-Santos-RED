using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class HumanShield : PedGrab
{
    protected override bool IsPlayerValid => Player.IsAliveAndFree && !Player.IsIncapacitated && Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.CanPistolSuicide;
    public HumanShield(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IEntityProvideable world) : base(player,ped,settings,crimes,modItems, world)
    {
        FailMessage = "Cannot Take Hostage";
    }

    protected override void SetupSpecific()
    {
        AttachOffset = new Vector3(Settings.SettingsManager.ActivitySettings.HumanShieldAttachX, Settings.SettingsManager.ActivitySettings.HumanShieldAttachY, Settings.SettingsManager.ActivitySettings.HumanShieldAttachZ);
        PlayerIdleDictionary = "anim@gangops@hostage@";
        PlayerIdleAnimation = "perp_idle";
        PedIdleDictionary = "anim@gangops@hostage@";
        PedIdleAnimation = "victim_idle";
        PlayerIdleFlags = 1 | 8 | 16 | 32;//maybe 8?
        if (!AnimationDictionary.RequestAnimationDictionayResult("anim@gangops@hostage@"))
        {
            EntryPoint.WriteToConsole("ERROR GRAB SHIELD COULD NOT LOAD ANIMATION DICTIONARY 1");
        }
    }
    protected override void SetupPed()
    {
        Ped.IsBeingHeldAsHostage = true;
        base.SetupPed();
    }
    protected override void SetupPlayer()
    {
        Player.ActivityManager.IsHoldingHostage = true;
        base.SetupPlayer();
    }
    protected override void ResetPed()
    {
        Ped.IsBeingHeldAsHostage = false;
        base.ResetPed();
    }
    protected override void ResetPlayer()
    {
        Player.ActivityManager.IsHoldingHostage = false;
        base.ResetPlayer();
    }
    protected override void SetupPrompts()
    {
        Player.ButtonPrompts.RemovePrompts("Grab");
        if (!Player.ButtonPrompts.HasPrompt("Execute"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Execute", "Execute", GameControl.Attack, 1);
        }
        if (!Player.ButtonPrompts.HasPrompt("Release"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Release", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
        }
    }
    protected override bool IsPromptPressed()
    {
        if (Player.IsShowingActionWheel)
        {
            return false;
        }
        if (Player.ButtonPrompts.IsPressed("Execute")) 
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            ExecuteHostage();
            return true;
        }
        else if (Player.ButtonPrompts.IsPressed("Release")) //else if (Player.ButtonPromptList.Any(x => x.Identifier == "Release" && x.IsPressedNow))//demand cash?
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            ReleaseHostage();
            return true;
        }
        return false;
    }
    private void ReleaseHostage()
    {
        if (Ped.Pedestrian.Exists())
        {
            //Ped.Pedestrian.Detach();
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
            GameFiber.Sleep(500);
            if (Ped.Pedestrian.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_success", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1

                uint GameTimeReleased = Game.GameTime;
                float AnimationTime = 0f;
                while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
                {
                    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
                    GameFiber.Yield();
                }
            }
        }
    }
    private void ExecuteHostage()
    {
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.Detach();
            Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
            if (Ped.Pedestrian.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
            }
            GameFiber.Wait(250);
            if (Ped.Pedestrian.Exists())
            {
                NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                if (Ped.Pedestrian.Exists())
                {
                    Ped.Pedestrian.Kill();
                }
            }

        }
    }
}