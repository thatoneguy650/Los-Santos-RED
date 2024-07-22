using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;


public class GroupManager
{
    private int maxMembers => Settings.SettingsManager.GroupSettings.MaxGroupMembers;// 15;// Settings.SettingsManager.PlayerOtherSettings.UseVanillaGroup ? 7 : 15;
    private IGroupManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangs Gangs;
    private IWeapons Weapons;
    private ITargetable Targetable;
    private bool IsSetCombatSpacing = false;
    private int selectedMode = 0;
    public List<GroupMember> CurrentGroupMembers { get; private set; } = new List<GroupMember>();
    public int PlayerGroup { get; private set; }
    public int MemberCount => CurrentGroupMembers.Count();
    public float GroupFollowDistance { get; set; } = 3.0f;
    public bool BlockPermanentEvents { get; set; } = true;
    public bool IsSetFollow { get; set; } = false;
    public bool IsSetCombat { get; set; } = false;
    public bool RideInPlayerVehicleIfPossible { get; set; } = true;
    public bool AlwaysArmed { get; set; } = false;
    public bool NeverArmed { get; set; } = false;
    public bool AutoArmed { get; set; } = true;

    public GroupManager(IGroupManageable player, ITargetable targetable, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, IWeapons weapons)
    {
        Player = player;
        Settings = settings;
        World = world;
        Gangs = gangs;
        Weapons = weapons;
        Targetable = targetable;
    }
    public void Setup()
    {
        CurrentGroupMembers = new List<GroupMember>();
    }
    public void Update()
    {
        CurrentGroupMembers.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists() || x.PedExt.Pedestrian.IsDead);
        for (int i = CurrentGroupMembers.Count - 1; i >= 0; i--)
        {
            GroupMember sc = CurrentGroupMembers[i];
            if(sc != null && sc.PedExt != null && sc.PedExt.IsBusted)
            {
                Remove(sc.PedExt);
                //EntryPoint.WriteToConsoleTestLong("Remove Group Member (Busted)");
            }
        }
    }
    public GroupMember GetMember(PedExt toget)
    {
        return CurrentGroupMembers.Where(x => x.PedExt != null && x.PedExt.Handle == toget.Handle).FirstOrDefault();
    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        Disband();
    }
    public GroupMember Add(PedExt groupMember)
    {
        if(groupMember == null || !groupMember.Pedestrian.Exists())
        {
            Game.DisplayHelp("Cannot Recruit: Member Invalid");
            return null;
        }
        if (CurrentGroupMembers.Count > maxMembers - 1)
        {
            Game.DisplayHelp("Cannot Recruit: Too Many Members");
            return null;
        }
        if (CurrentGroupMembers.Any(x => x.PedExt != null && x.PedExt.Handle == groupMember.Handle))
        {
            Game.DisplayHelp("Cannot Recruit: Already Member");
            return null;
        }
        GroupMember newMember = new GroupMember(groupMember, CurrentGroupMembers.Count + 1, Targetable, World, Settings, this, Weapons);
        CurrentGroupMembers.Add(newMember);
        newMember.OnBecameGroupMember();
        return newMember;
    }
    public void Disband()
    {
        //PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        foreach (GroupMember groupMember in CurrentGroupMembers)
        {
            groupMember.OnLeftGroup();
        }
        CurrentGroupMembers.Clear();
    }
    public void Remove(GroupMember toRemove)
    {
        if (toRemove == null || toRemove.PedExt == null)
        {
            return;
        }
        CurrentGroupMembers.RemoveAll(x => x.PedExt.Handle == toRemove.PedExt.Handle);
        toRemove.OnLeftGroup();
    }
    public void Remove(PedExt toRemove)
    {
        GroupMember groupMember = CurrentGroupMembers.FirstOrDefault(x => x.PedExt.Handle == toRemove.Handle);
        if(groupMember == null)
        {
            return;
        }
        Remove(groupMember);
    }
    public bool IsMember(PedExt groupMember)
    {
        return CurrentGroupMembers.Any(x=> x.PedExt.Handle == groupMember.Handle);
    }
    public void TryRecruitLookedAtPed()
    {
        Add(Player.CurrentLookedAtPed);
    }
    public void UpdateAllTasking()
    {
        foreach (GroupMember groupMember in CurrentGroupMembers)
        {
            groupMember.UpdateTasking(BlockPermanentEvents);
        }
    }

    public void ToggleForceTasking()
    {
        BlockPermanentEvents = !BlockPermanentEvents;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Force Tasking {(BlockPermanentEvents ? "Enabled" : "Disabled")}");
    }

    public void SetInvincible()
    {
        foreach (GroupMember groupMember in CurrentGroupMembers)
        {
            if(groupMember.PedExt != null && groupMember.PedExt.Pedestrian.Exists())
            {
                groupMember.PedExt.Pedestrian.IsInvincible = true;
            }
        }
        Game.DisplaySubtitle("SET GROUP INVINCIBLE");
    }

    public void OnSetAutoTasking()
    {
        IsSetFollow = false;
        IsSetCombat = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Set Auto Tasking");
    }

    public void OnSetCombatTasking()
    {
        IsSetCombat = true;
        IsSetFollow = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Set Combat");
    }

    public void OnSetNonCombatTasking()
    {
        IsSetFollow = true;
        IsSetCombat = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Set Follow");
    }
    public void OnToggleUsePlayerCar()
    {
        RideInPlayerVehicleIfPossible = !RideInPlayerVehicleIfPossible;
        UpdateAllTasking();
        Game.DisplaySubtitle($"RideInPlayerVehicleIfPossible {(RideInPlayerVehicleIfPossible ? "Enabled" : "Disabled")}");
    }
    public void OnSetAutoArmed()
    {
        AlwaysArmed = false;
        NeverArmed = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Auto Armed Enabled");
        //PlayToggleSound();
    }
    public void OnSetAlwaysArmed()
    {
        AlwaysArmed = true;
        NeverArmed = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Always Armed Enabled");
        //PlayToggleSound();
    }
    public void OnSetNeverArmed()
    {
        NeverArmed = true;
        AlwaysArmed = false;
        UpdateAllTasking();
        Game.DisplaySubtitle($"Never Armed Enabled");
        //PlayToggleSound();
    }

    public void ToggleMode()
    {
        if(MemberCount == 0)
        {
            return;
        }
        selectedMode++;
        if(selectedMode >= 3)
        {
            selectedMode = 0;
        }
        if(selectedMode == 0)
        {
            OnSetAutoTasking();      
        }
        else if (selectedMode == 1)
        {
            OnSetNonCombatTasking();       
        }
        else
        {
            OnSetCombatTasking();
        }
        PlayToggleSound();
    }
    private void PlayToggleSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }
}

