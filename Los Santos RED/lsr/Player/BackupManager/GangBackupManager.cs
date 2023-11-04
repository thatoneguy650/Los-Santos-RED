using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangBackupManager
{
    private IEntityProvideable World;
    private IGangBackupable Player;

    public GangBackupManager(IEntityProvideable world, IGangBackupable player)
    {
        World = world;
        Player = player;
    }
    public List<GangBackup> ActiveBackup { get; private set; } = new List<GangBackup>();
    public void Setup()
    {

    }
    public void Update()
    {
        foreach (GangBackup gangBackup in ActiveBackup)
        {
            gangBackup.Update();
        }
        ActiveBackup.RemoveAll(x => !x.IsValid || !x.IsActive);
    }
    public void Reset()
    {
        foreach (GangBackup gangBackup in ActiveBackup)
        {
            gangBackup.Cancel();
        }
        ActiveBackup.Clear();
        EntryPoint.WriteToConsole("GangBackupManager Reset");
    }
    public void Dispose()
    {
        foreach (GangBackup gangBackup in ActiveBackup)
        {
            gangBackup.Dispose();
        }
        ActiveBackup.Clear();
    }
    public bool RequestBackup(Gang gang, int minMembers)
    {
        if (gang == null)
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, NO GANG");
            return false;
        }
        if (ActiveBackup.Any(x => x.RequestedGang.ID == gang.ID))
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, ALREADY ACTIVE BACKUP");
            return false;
        }
        GangBackup gangBackup = new GangBackup(World, Player, gang, minMembers);
        gangBackup.Setup();
        if (!gangBackup.IsActive)
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, NOT ACTIVE");
            return false;
        }
        ActiveBackup.Add(gangBackup);
        EntryPoint.WriteToConsole("GangBackupManager RequestBackup Active backup Added");
        return true;
    }
}

