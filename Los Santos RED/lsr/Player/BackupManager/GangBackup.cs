using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangBackup
{
    private IEntityProvideable World;
    private IGangBackupable Player;
    private List<GangMember> BackupMembers = new List<GangMember>();
    public GangBackup(IEntityProvideable world, IGangBackupable player, Gang gang)
    {
        World = world;
        Player = player;
        RequestedGang = gang;
    }
    public Gang RequestedGang { get; set; }
    public bool IsActive { get; set; }
    public bool IsValid { get; set; } = true;
    public void Setup()
    {
        IsActive = false;
        EntryPoint.WriteToConsole("Gang Backup IS ACTIVE SET TO FALSE SETUP");
        if (GetBackupMembers())//if there is an existing one
        {
            IsActive = true;
            return;
        }
        SpawnMembers();
        if (GetBackupMembers())//if there is an existing one
        {
            IsActive = true;
            return;
        }
        EntryPoint.WriteToConsole("GangBackup SETUP FAIL NO DISPATCH");
    }

    private void SpawnMembers()
    {
        EntryPoint.WriteToConsole("GangBackup SETUP FORCING GANG SPAWN");
        Player.Dispatcher.DispatchGangBackup(RequestedGang);// Dispatcher.ForceTaxiSpawn(RequestedFirm);
    }

    private bool GetBackupMembers()
    {
        bool foundOne = false;
        foreach(GangMember gangmember in World.Pedestrians.GangMemberList.Where(x => x.Gang != null && x.Gang.ID == RequestedGang.ID && (x.IsInVehicle || x.DistanceToPlayer <= 55f) && x.WasModSpawned))
        {
            gangmember.IsBackupSquad = true;
            foundOne = true;
            BackupMembers.Add(gangmember);
        }
        return foundOne;
    }

    public void Update()
    {
        bool hasValidMembers = false;
        foreach (GangMember gangMember in BackupMembers)
        {
            if(gangMember.IsDead || gangMember.IsUnconscious || !gangMember.Pedestrian.Exists())
            {
                gangMember.IsAddedToPlayerGroup = false;
                gangMember.IsBackupSquad = false;
                Player.GroupManager.Remove(gangMember);
            }
            else
            {
                hasValidMembers = true;
                if (!gangMember.IsAddedToPlayerGroup && gangMember.DistanceToPlayer <= 40f)
                {
                    Player.GroupManager.Add(gangMember);
                    gangMember.IsAddedToPlayerGroup = true;
                }
            }
        }
        if(!hasValidMembers)
        {
            EntryPoint.WriteToConsole("Back Backup no valid members, setting inactive");
            IsValid = false;
        }
    }
    public void Cancel()
    {
        foreach(GangMember gangMember in BackupMembers)
        {
            gangMember.IsBackupSquad = false;
            gangMember.IsAddedToPlayerGroup = false;
            Player.GroupManager.Remove(gangMember);
        }
    }
    public void Dispose()
    {
        foreach (GangMember gangMember in BackupMembers)
        {
            gangMember.IsBackupSquad = false;
            gangMember.IsAddedToPlayerGroup = false;
            Player.GroupManager.Remove(gangMember);
        }
    }
}

