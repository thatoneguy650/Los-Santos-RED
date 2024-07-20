using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GroupMember
{
    private ITargetable Targetable;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private GroupManager GroupManager;
    private RelationshipGroup previousPedGroup;

    public GroupMember(PedExt pedExt, int index, ITargetable player, IEntityProvideable world, ISettingsProvideable settings, GroupManager groupManager, IWeapons weapons)
    {
        PedExt = pedExt;
        Index = index;
        Targetable = player;
        World = world;
        Settings = settings;
        GroupManager = groupManager;
        Weapons = weapons;
    }

    public PedExt PedExt { get; set; }
    public int Index { get; set; }
    public bool SetForceTasking { get; set; } = false;
    public bool SetFollowIfPossible { get; set; } = false;
    public bool SetCombatIfPossible { get; set; } = false;
    public bool RideInPlayerVehicleIfPossible { get; set; } = true;
    public bool AlwaysArmed { get; set; } = false;
    public bool NeverArmed { get; set; } = false;
    public void UpdateTasking(bool SetForceTasking)
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.Pedestrian.BlockPermanentEvents = SetForceTasking;
        SetFollow();
    }
    public void SetFollow()
    {
        if (PedExt.IsBusted)
        {
            return;
        }
        if (PedExt.CurrentTask != null)
        {
            PedExt.CurrentTask.Stop();
            PedExt.CurrentTask = null;
        }
        GangMember gm = null;
        if (PedExt.GetType() == typeof(GangMember))
        {
            gm = (GangMember)PedExt;
        }
        PedExt.CurrentTask = new GeneralFollow(PedExt, PedExt, Targetable, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, null, Settings, GroupManager, gm);
        PedExt.CurrentTask.Start();
    }
    public void OnBecameGroupMember()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        if (!PedExt.WasModSpawned)
        {
            PedExt.WasEverSetPersistent = true;
            previousPedGroup = PedExt.Pedestrian.RelationshipGroup;
            PedExt.Pedestrian.IsPersistent = true;
            PedExt.Pedestrian.RelationshipGroup = previousPedGroup;
        }
        PedExt.IsGroupMember = true;
        PedExt.CanBeTasked = false;
        PedExt.CanBeAmbientTasked = false;
        PedExt.CurrentTask = null;
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        PedExt.Pedestrian.BlockPermanentEvents = false;
        NativeFunction.Natives.SET_PED_ALERTNESS(PedExt.Pedestrian, 3);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(PedExt.Pedestrian, 0, false);
       // AddInternal(PedExt);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_CanDoDrivebys, true);
        if(Settings.SettingsManager.GroupSettings.EnableIncreasedGroupHealth)
        {
            PedExt.Pedestrian.MaxHealth = Settings.SettingsManager.GroupSettings.IncreasedHealthMax;
            PedExt.Pedestrian.Health = RandomItems.GetRandomNumberInt(Settings.SettingsManager.GroupSettings.IncreasedHealthMin, Settings.SettingsManager.GroupSettings.IncreasedHealthMax);
        }
        if(Settings.SettingsManager.GroupSettings.EnableAutoArmor)
        {
            PedExt.Pedestrian.Armor = RandomItems.GetRandomNumberInt(Settings.SettingsManager.GroupSettings.AutoArmorMin, Settings.SettingsManager.GroupSettings.AutoArmorMax);
        }
        if(Settings.SettingsManager.GroupSettings.AlwaysSetSpecialist)
        {
            SetSpecialist();
        }
        //Set combat defensive
        NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(PedExt.Pedestrian, 1);
        uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(PedExt.Pedestrian, 0);
        WeaponInformation wi = Weapons.GetWeapon(bestWeapon);
        string weaponString = "Unarmed";
        if (wi != null)
        {
            weaponString = $"Weapon: {wi.Category}";
        }
        Game.DisplayHelp($"Recruited {PedExt.FormattedName} {weaponString}");
        SetFollow();
    }
    public void OnLeftGroup()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.IsGroupMember = false;
        PedExt.CanBeTasked = true;
        PedExt.CanBeAmbientTasked = true;
        //ResetStatus(groupMember, true);
        if (PedExt.IsBusted)
        {
            return;
        }
        if (PedExt.CurrentTask != null)
        {
            PedExt.CurrentTask.Stop();
            PedExt.CurrentTask = null;
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        if (!PedExt.WasModSpawned)
        {
            PedExt.Pedestrian.KeepTasks = true;
            PedExt.Pedestrian.IsPersistent = false;
            PedExt.Pedestrian.RelationshipGroup = previousPedGroup;
        }
    }
    public void SetViolent()
    {
        PedExt.WillCallPolice = false;
        PedExt.WillCallPoliceIntense = false;
        PedExt.WillFight = true;
        PedExt.WillFightPolice = true;
        PedExt.WillAlwaysFightPolice = true;
    }
    public void SetPassive()
    {
        PedExt.WillCallPolice = false;
        PedExt.WillCallPoliceIntense = false;
        PedExt.WillFight = false;
        PedExt.WillFightPolice = false;
        PedExt.WillAlwaysFightPolice = false;
    }
    public void SetSpecialist()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.Pedestrian.Accuracy = 95;
        NativeFunction.Natives.SET_PED_SHOOT_RATE(PedExt.Pedestrian, 700);
        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(PedExt.Pedestrian, 2);
        //NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(PedExt.Pedestrian, 1);
    }
    public void ResetStatus(bool clearTasks)
    {
        if (PedExt.IsBusted)
        {
            return;
        }
        if (PedExt.CurrentTask != null)
        {
            if (clearTasks)
            {
                PedExt.CurrentTask.Stop();
            }
            PedExt.CurrentTask = null;
        }
        if (PedExt.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_ALERTNESS(PedExt.Pedestrian, 3);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(PedExt.Pedestrian, 0, false);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
            if (clearTasks)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
            }
        }
    }
    public void GiveCurrentWeapon()
    {
        if (Targetable.WeaponEquipment.CurrentWeapon != null && PedExt.Pedestrian.Exists())
        {
            WeaponVariation WeaponToTransferVariations = Weapons.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            uint WeaponToTransferHash = (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
            if (WeaponToTransferHash != 0 && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(PedExt.Pedestrian, WeaponToTransferHash, false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(PedExt.Pedestrian, WeaponToTransferHash, 200, false, false);
                WeaponInformation weaponToTransferInfo = Weapons.GetWeapon(WeaponToTransferHash);
                if (weaponToTransferInfo != null)
                {
                    weaponToTransferInfo.ApplyWeaponVariation(PedExt.Pedestrian, WeaponToTransferVariations);
                }
                NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(Game.LocalPlayer.Character, WeaponToTransferHash);
            }
        }
    }

}

