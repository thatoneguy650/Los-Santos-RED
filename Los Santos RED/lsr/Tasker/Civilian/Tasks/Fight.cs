using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Fight : ComplexTask
{
    WeaponInformation ToIssue;
    ITargetable Player;
    public Fight(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue) : base(player, ped, 5000)
    {
        Name = "Fight";
        SubTaskName = "";
        ToIssue = toIssue;
        Player = player;
    }
    public override void Start()
    {
        if(!Ped.Pedestrian.Exists())
        {
            return;
        }

        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Ped.Pedestrian, 0, false);
        if (ToIssue != null)
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Ped.Pedestrian, (uint)ToIssue.Hash, ToIssue.AmmoAmount, false, false);
        }
        if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 0, 16);
        }
        else
        {
            if (Ped.IsGangMember)
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 75f, 0);//TR
            }
            else
            {
                NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, 0, 16);
            }
        }
        NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
        NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
        GameTimeLastRan = Game.GameTime;
        
    }
    public override void Update()
    {
        if(Ped.Pedestrian.Exists() && Ped.IsDriver)
        {
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
}

