using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GroupManager
{
    private int maxMembers = 7;
    private IGroupManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangs Gangs;
    public List<PedExt> CurrentGroupMembers { get; private set; } = new List<PedExt>();
    public int PlayerGroup { get; private set; }
    public int MemberCount => CurrentGroupMembers.Count();
    public GroupManager(IGroupManageable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs)
    {
        Player = player;
        Settings = settings;
        World = world;
        Gangs = gangs;
    }
    public void Setup()
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
        CurrentGroupMembers = new List<PedExt>();
    }
    public void Update()
    {
        CurrentGroupMembers.RemoveAll(x => !x.Pedestrian.Exists());
    }
    public void Dispose()
    {

    }
    public bool Add(PedExt groupMember)
    {
        if (groupMember != null && groupMember.Pedestrian.Exists() && CurrentGroupMembers.Count <= maxMembers-1)
        {

            if (!CurrentGroupMembers.Any(x => x.Handle == groupMember.Handle))
            {
                PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
                CurrentGroupMembers.Add(groupMember);
                groupMember.CanBeTasked = false;
                groupMember.CanBeAmbientTasked = false;

                NativeFunction.Natives.CLEAR_PED_TASKS(groupMember.Pedestrian);
                groupMember.Pedestrian.KeepTasks = true;


                //groupMember.Pedestrian.KeepTasks = true;
                //groupMember.Pedestrian.IsPersistent = true;
                NativeFunction.Natives.SET_PED_ALERTNESS(groupMember.Pedestrian, 3);
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(groupMember.Pedestrian, 0, false);

                NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(groupMember.Pedestrian, PlayerGroup);
                NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);

                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(groupMember.Pedestrian, 5000000, 0);//TR
                return true;
            }
        }
        return false;
    }
    public void Disband()
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        foreach (PedExt groupMember in CurrentGroupMembers)
        {
            if (groupMember.Pedestrian.Exists())
            {
                if (NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(groupMember.Pedestrian, PlayerGroup))
                {
                    NativeFunction.Natives.REMOVE_PED_FROM_GROUP(groupMember.Pedestrian);
                }
            }
            groupMember.CanBeTasked = true;
            groupMember.CanBeAmbientTasked = true;
        }
        CurrentGroupMembers.Clear();
    }
    public void Remove(PedExt groupMember)
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        if (NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(groupMember.Pedestrian, PlayerGroup))
        {
            NativeFunction.Natives.REMOVE_PED_FROM_GROUP(groupMember.Pedestrian);
        }
        if(CurrentGroupMembers.Contains(groupMember))
        {
            CurrentGroupMembers.Remove(groupMember);
        }
        groupMember.CanBeTasked = true;
        groupMember.CanBeAmbientTasked = true;
    }
    public bool IsMember(PedExt groupMember)
    {
        return CurrentGroupMembers.Contains(groupMember);
    }
    public void TryRecruitLookedAtPed()
    {
        if(Add(Player.CurrentLookedAtPed))
        {
            Game.DisplayHelp($"Recruited {Player.CurrentLookedAtPed.FormattedName}");
        }
        else
        {
            Game.DisplayHelp("Cannot Recruit Member");
        }
    }

}

