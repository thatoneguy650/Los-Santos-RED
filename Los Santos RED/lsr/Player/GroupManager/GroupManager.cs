using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
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
    private IWeapons Weapons;
    private ITargetable Targetable;
    public List<PedExt> CurrentGroupMembers { get; private set; } = new List<PedExt>();
    public int PlayerGroup { get; private set; }
    public int MemberCount => CurrentGroupMembers.Count();
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
        CurrentGroupMembers = new List<PedExt>();
    }
    public void Update()
    {
        CurrentGroupMembers.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
        foreach(PedExt groupMember in CurrentGroupMembers)
        {
            if(groupMember.Pedestrian.Exists() && groupMember.CurrentTask != null)
            {
                groupMember.CurrentTask.Update();
            }
        }
    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        Disband();
    }
    public bool Add(PedExt groupMember)
    {
        if (groupMember != null && groupMember.Pedestrian.Exists() && CurrentGroupMembers.Count <= maxMembers-1)
        {
            if (!CurrentGroupMembers.Any(x => x.Handle == groupMember.Handle))
            {
                PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
                CurrentGroupMembers.Add(groupMember);
                OnBecameGroupMember(groupMember);
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
            OnLeftGroup(groupMember);
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
        OnLeftGroup(groupMember);

    }
    public bool IsMember(PedExt groupMember)
    {
        return CurrentGroupMembers.Contains(groupMember);
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
    public void ResetStatus(PedExt groupMember)
    {
        if (groupMember.CurrentTask != null)
        {
            groupMember.CurrentTask.Stop();
            groupMember.CurrentTask = null;
        }
        if (groupMember.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_ALERTNESS(groupMember.Pedestrian, 3);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(groupMember.Pedestrian, 0, false);
            NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(groupMember.Pedestrian, PlayerGroup);
            NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
            NativeFunction.Natives.CLEAR_PED_TASKS(groupMember.Pedestrian);
        }
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
    private void OnBecameGroupMember(PedExt groupMember)
    {
        groupMember.IsGroupMember = true;
        groupMember.CanBeTasked = false;
        groupMember.CanBeAmbientTasked = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(groupMember.Pedestrian);
        //groupMember.Pedestrian.KeepTasks = true;
        groupMember.Pedestrian.BlockPermanentEvents = false;
        //groupMember.Pedestrian.IsInvincible = true;
        NativeFunction.Natives.SET_PED_ALERTNESS(groupMember.Pedestrian, 3);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(groupMember.Pedestrian, 0, false);
        NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(groupMember.Pedestrian, PlayerGroup);
        NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
        //groupMember.Pedestrian.KeepTasks = true;
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(groupMember.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
       // NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(groupMember.Pedestrian, 100f, 0);//TR
       // NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_IN_AREA(groupMember.Pedestrian, groupMember.Pedestrian.Position.X,groupMember.Pedestrian.Position.Y,groupMember.Pedestrian.Position.Z, 5000f, 0);//TR
       // groupMember.Pedestrian.KeepTasks = true;
    }
    private void OnLeftGroup(PedExt groupMember)
    {
        groupMember.IsGroupMember = false;
        groupMember.CanBeTasked = true;
        groupMember.CanBeAmbientTasked = true;
    }
    public void SetFollow(PedExt mi)
    {
        if (mi.CurrentTask != null)
        {
            mi.CurrentTask.Stop();
            mi.CurrentTask = null;
        }
        mi.CurrentTask = new GeneralFollow(mi, mi, Targetable, World, new List<VehicleExt>() { mi.AssignedVehicle }, null, Settings);
        mi.CurrentTask.Start();
    }
}

