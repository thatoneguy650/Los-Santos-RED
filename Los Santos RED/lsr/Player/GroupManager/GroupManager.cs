using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;


public class GroupManager
{
    private int maxMembers = 7;
    private IGroupManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangs Gangs;
    private IWeapons Weapons;
    private ITargetable Targetable;
    private bool IsSetCombatSpacing = false;
    public List<GroupMember> CurrentGroupMembers { get; private set; } = new List<GroupMember>();
    public int PlayerGroup { get; private set; }
    public int MemberCount => CurrentGroupMembers.Count();

    public float GroupFollowDistance { get; set; } = 3.0f;

    public bool SetForceTasking { get; set; } = false;
    public bool SetFollowIfPossible { get; set; } = true;
    public bool SetCombatIfPossible { get; set; } = false;
    public bool RideInPlayerVehicleIfPossible { get; set; } = true; 
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
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
        CurrentGroupMembers = new List<GroupMember>();
    }
    public void Update()
    {
        CurrentGroupMembers.RemoveAll(x => x.PedExt == null || !x.PedExt.Pedestrian.Exists() || x.PedExt.Pedestrian.IsDead);
        bool isAnyInCombat = false;
        for (int i = CurrentGroupMembers.Count - 1; i >= 0; i--)
        {
            GroupMember sc = CurrentGroupMembers[i];
            if(sc != null && sc.PedExt != null && sc.PedExt.IsBusted)
            {
                Remove(sc.PedExt);
                //EntryPoint.WriteToConsoleTestLong("Remove Group Member (Busted)");
            }
            if(sc != null && (Player.IsWanted || sc.PedExt.IsWanted || (sc.PedExt.Pedestrian.Exists() && sc.PedExt.Pedestrian.IsInCombat)))
            {
                isAnyInCombat = true;
            }
        }
        if(!Settings.SettingsManager.PlayerOtherSettings.UseVanillaGroup)
        {
            return;
        }
        if(isAnyInCombat && !IsSetCombatSpacing)
        {
            NativeFunction.Natives.SET_GROUP_FORMATION_SPACING(PlayerGroup, 15f, -1.0f, 30f);
            IsSetCombatSpacing = true;
            EntryPoint.WriteToConsole("GROUP SET COMBAT SPACING");
        }
        else if(!isAnyInCombat && IsSetCombatSpacing)
        {
            NativeFunction.Natives.RESET_GROUP_FORMATION_DEFAULT_SPACING(PlayerGroup);
            IsSetCombatSpacing = false;
            EntryPoint.WriteToConsole("GROUP SET COMBAT REGULAR");
        }
    }
    public void AddInternal(PedExt groupMember)
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        if (Settings.SettingsManager.PlayerOtherSettings.UseVanillaGroup && groupMember != null && groupMember.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(groupMember.Pedestrian, PlayerGroup);
        }
        EntryPoint.WriteToConsole("Added Ped To GTA Group");
    }
    public void RemoveInternal(PedExt groupMember)
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        if (Settings.SettingsManager.PlayerOtherSettings.UseVanillaGroup && groupMember.Pedestrian.Exists())
        {
            if (NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(groupMember.Pedestrian, PlayerGroup))
            {
                NativeFunction.Natives.REMOVE_PED_FROM_GROUP(groupMember.Pedestrian);
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
    public void Add(PedExt groupMember)
    {
        if(groupMember == null || !groupMember.Pedestrian.Exists())
        {
            Game.DisplayHelp("Cannot Recruit: Member Invalid");
            return;
        }
        if (CurrentGroupMembers.Count > maxMembers - 1)
        {
            Game.DisplayHelp("Cannot Recruit: Too Many Members");
            return;
        }
        if (CurrentGroupMembers.Any(x => x.PedExt != null && x.PedExt.Handle == groupMember.Handle))
        {
            Game.DisplayHelp("Cannot Recruit: Already Member");
            return;
        }
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        CurrentGroupMembers.Add(new GroupMember(groupMember, CurrentGroupMembers.Count+1));
        OnBecameGroupMember(groupMember);    
        if(CurrentGroupMembers.Count() == 1)
        {
            OnStartedGroup();
        }
    }
    private void OnStartedGroup()
    {
        if (Settings.SettingsManager.PlayerOtherSettings.UseVanillaGroup)
        {
            NativeFunction.Natives.SET_GROUP_FORMATION(PlayerGroup, 0);//LOOSE FORMATION
            NativeFunction.Natives.SET_GROUP_FORMATION_SPACING(PlayerGroup, 5f, -1.0f, 15f);
        }
    }
    public void Disband()
    {
        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
        foreach (GroupMember groupMember in CurrentGroupMembers)
        {
            RemoveInternal(groupMember.PedExt);
            OnLeftGroup(groupMember.PedExt);
        }
        CurrentGroupMembers.Clear();
    }
    public void Remove(PedExt groupMember)
    {
        RemoveInternal(groupMember);
        if(CurrentGroupMembers.Any(x => x.PedExt.Handle == groupMember.Handle))
        {
            CurrentGroupMembers.RemoveAll(x => x.PedExt.Handle == groupMember.Handle);
        }
        OnLeftGroup(groupMember);

    }
    public bool IsMember(PedExt groupMember)
    {
        return CurrentGroupMembers.Any(x=> x.PedExt.Handle == groupMember.Handle);
    }
    public void GiveCurrentWeapon(PedExt groupMember)
    {
        if(Player.WeaponEquipment.CurrentWeapon != null && groupMember.Pedestrian.Exists())
        {
            WeaponVariation WeaponToTransferVariations = Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            uint WeaponToTransferHash = (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
            if (WeaponToTransferHash != 0 && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(groupMember.Pedestrian, WeaponToTransferHash, false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(groupMember.Pedestrian, WeaponToTransferHash, 200, false, false);
                WeaponInformation weaponToTransferInfo = Weapons.GetWeapon(WeaponToTransferHash);
                if (weaponToTransferInfo != null)
                {
                    weaponToTransferInfo.ApplyWeaponVariation(groupMember.Pedestrian, WeaponToTransferVariations);
                }
                NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(Game.LocalPlayer.Character, WeaponToTransferHash);
            }
        }
    }
    public void ResetStatus(PedExt groupMember, bool clearTasks)
    {
        if(groupMember.IsBusted)
        {
            return;
        }
        if (groupMember.CurrentTask != null)
        {
            if (clearTasks)
            {
                groupMember.CurrentTask.Stop();
            }
            groupMember.CurrentTask = null;
        }
        if (groupMember.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_ALERTNESS(groupMember.Pedestrian, 3);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(groupMember.Pedestrian, 0, false);
            AddInternal(groupMember);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
            if (clearTasks)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(groupMember.Pedestrian);
            }
        }
    }
    public void TryRecruitLookedAtPed()
    {
        Add(Player.CurrentLookedAtPed);
    }
    private void OnBecameGroupMember(PedExt groupMember)
    {
        groupMember.IsGroupMember = true;
        groupMember.CanBeTasked = false;
        groupMember.CanBeAmbientTasked = false;
        groupMember.CurrentTask = null;
        NativeFunction.Natives.CLEAR_PED_TASKS(groupMember.Pedestrian);
        groupMember.Pedestrian.BlockPermanentEvents = false;
        NativeFunction.Natives.SET_PED_ALERTNESS(groupMember.Pedestrian, 3);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(groupMember.Pedestrian, 0, false);
        AddInternal(groupMember);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanDoDrivebys, true);
        uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(groupMember.Pedestrian, 0);
        WeaponInformation wi = Weapons.GetWeapon(bestWeapon);
        string weaponString = "Unarmed";
        if (wi != null)
        {
            weaponString = $"Weapon: {wi.Category}";
        }
        Game.DisplayHelp($"Recruited {groupMember.FormattedName} {weaponString}");
        SetFollow(groupMember);
    }
    private void OnLeftGroup(PedExt groupMember)
    {
        groupMember.IsGroupMember = false;
        groupMember.CanBeTasked = true;
        groupMember.CanBeAmbientTasked = true;
        ResetStatus(groupMember, true);
    }
    public void SetViolent(PedExt groupMember)
    {
        groupMember.WillCallPolice = false;
        groupMember.WillCallPoliceIntense = false;
        groupMember.WillFight = true;
        groupMember.WillFightPolice = true;
        groupMember.WillAlwaysFightPolice = true;
    }
    public void SetPassive(PedExt groupMember)
    {
        groupMember.WillCallPolice = false;
        groupMember.WillCallPoliceIntense = false;
        groupMember.WillFight = false;
        groupMember.WillFightPolice = false;
        groupMember.WillAlwaysFightPolice = false;
    }
    public void SetSpecialist(PedExt groupMember)
    {
        if (groupMember == null || !groupMember.Pedestrian.Exists())
        {
            return;
        }
        groupMember.Pedestrian.Accuracy = 85;
        NativeFunction.Natives.SET_PED_SHOOT_RATE(groupMember.Pedestrian, 300);
        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(groupMember.Pedestrian, 2);
        NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(groupMember.Pedestrian, 1);
    }
    public void SetFollow(PedExt mi)
    {
        if (mi.IsBusted)
        {
            return;
        }
        if (mi.CurrentTask != null)
        {
            mi.CurrentTask.Stop();
            mi.CurrentTask = null;
        }
        GangMember gm = null;
        if(mi.GetType() == typeof(GangMember))
        {
            gm = (GangMember)mi;
        }
        mi.CurrentTask = new GeneralFollow(mi, mi, Targetable, World, new List<VehicleExt>() { mi.AssignedVehicle }, null, Settings, this, gm);
        mi.CurrentTask.Start();
    }

    public void UpdateTasking()
    {
        foreach (GroupMember groupMember in CurrentGroupMembers)
        {
            if(groupMember.PedExt == null || !groupMember.PedExt.Pedestrian.Exists())
            {
                continue;
            }
            groupMember.PedExt.Pedestrian.BlockPermanentEvents = SetForceTasking;
            SetFollow(groupMember.PedExt);
        }
    }
}

