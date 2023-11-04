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
    private int FoundMembers = 0;
    private int MinMembers;
    private IEntityProvideable World;
    private IGangBackupable Player;
    private List<GangMember> BackupMembers = new List<GangMember>();
    public GangBackup(IEntityProvideable world, IGangBackupable player, Gang gang, int minMembers)
    {
        World = world;
        Player = player;
        RequestedGang = gang;
        MinMembers = minMembers;
    }
    public Gang RequestedGang { get; set; }
    public bool IsActive { get; set; }
    public bool IsValid { get; set; } = true;
    public void Setup()
    {
        IsActive = false;
        EntryPoint.WriteToConsole("Gang Backup IS ACTIVE SET TO FALSE SETUP");

        FoundMembers = GetBackupMembers();
        if (FoundMembers >= MinMembers)//if there is an existing one
        {
            IsActive = true;
            return;
        }
        SpawnMembers();
        FoundMembers = GetBackupMembers();
        if (FoundMembers > 0)//if there is an existing one
        {
            IsActive = true;
            return;
        }
        EntryPoint.WriteToConsole("GangBackup SETUP FAIL NO DISPATCH");
    }

    private void SpawnMembers()
    {
        EntryPoint.WriteToConsole($"GangBackup SETUP FORCING GANG SPAWN {MinMembers - FoundMembers}");
        Player.Dispatcher.DispatchGangBackup(RequestedGang, MinMembers - FoundMembers);// Dispatcher.ForceTaxiSpawn(RequestedFirm);
    }

    private int GetBackupMembers()
    {
        int membersFound = 0;
        foreach(GangMember gangmember in World.Pedestrians.GangMemberList.Where(x => x.Gang != null && x.Gang.ID == RequestedGang.ID && (x.IsInVehicle || x.DistanceToPlayer <= 55f) && x.WasModSpawned))
        {
            gangmember.IsBackupSquad = true;
            membersFound++;
            BackupMembers.Add(gangmember);
        }
        return membersFound;
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
                EntryPoint.WriteToConsole("REMOVED GROUP MEMBER");
            }
            else
            {
                hasValidMembers = true;
                if (!gangMember.IsAddedToPlayerGroup && gangMember.DistanceToPlayer <= 20f)
                {
                    Player.GroupManager.Add(gangMember);
                    Player.GroupManager.ResetStatus(gangMember);
                    GameFiber.Sleep(500);
                    Player.GroupManager.SetFollow(gangMember);
                    //Player.GroupManager.SetFollow(gangMember);
                    gangMember.IsAddedToPlayerGroup = true;
                    EntryPoint.WriteToConsole("ADDED GROUP MEMBER");
                    //gangMember.IsBackupSquad = false;
                }
            }
        }
        BackupMembers.RemoveAll(x => !x.IsBackupSquad && !x.IsAddedToPlayerGroup);
        if (!hasValidMembers)
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

