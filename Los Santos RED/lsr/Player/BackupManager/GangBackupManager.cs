using ExtensionsMethods;
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
        foreach (GangBackup gangBackup in ActiveBackup.ToList())
        {
            gangBackup.Update();
        }
        ActiveBackup.RemoveAll(x => !x.IsValid || !x.IsActive);
    }
    public void Reset()
    {
        foreach (GangBackup gangBackup in ActiveBackup.ToList())
        {
            gangBackup.Cancel();
        }
        ActiveBackup.Clear();
        Player.GangRequisitionManager?.ReleaseAllSquads();
        EntryPoint.WriteToConsole("GangBackupManager Reset");
    }
    public void Dispose()
    {
        foreach (GangBackup gangBackup in ActiveBackup.ToList())
        {
            gangBackup.Dispose();
        }
        ActiveBackup.Clear();
    }
    public void RequestBackup(Gang gang, int requestedMemberCount, string requiredVehicleModel)
    {
        if (gang == null)
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, NO GANG");      
            return;
        }
        // Rank cap, cooldown and goodwill, all at the one entry point every backup
        // request passes through. Upstream's hard-coded 7 stays put downstream as a
        // ceiling this never reaches, so none of those four literals need editing.
        if (Player.GangRequisitionManager != null)
        {
            int authorizedCount = requestedMemberCount;
            string requisitionRefusal = Player.GangRequisitionManager.AuthorizeBackup(gang, ref authorizedCount);
            if (requisitionRefusal != null)
            {
                Player.CellPhone.AddPhoneResponse(gang.Contact.Name, gang.Contact.IconName, requisitionRefusal);
                return;
            }
            requestedMemberCount = authorizedCount;
        }
        GangBackup currentBackup = ActiveBackup.FirstOrDefault(x => x.RequestedGang.ID == gang.ID);
        if(currentBackup == null)
        {
            AddNewBackup(gang, requestedMemberCount, requiredVehicleModel);
            return;
        }
        UpdateBackup(currentBackup, requestedMemberCount, requiredVehicleModel);
    }

    private void UpdateBackup(GangBackup currentBackup, int requestedMemberCount, string requiredVehicleModel)
    {
        if(currentBackup == null)
        {
            return;
        }
        if(currentBackup.MemberCount >= 7)
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, ALREADY ACTIVE BACKUP");
            List<string> tooManyReplies = new List<string>() {
                "Already got some guys out there.",
                "I already sent the guys.",
                "Don't you have some guys already there?."
                };
            Player.CellPhone.AddPhoneResponse(currentBackup.RequestedGang.Contact.Name, currentBackup.RequestedGang.Contact.IconName, tooManyReplies.PickRandom());
            return;
        }
        int CurrentBackupMembers = currentBackup.MemberCount;
        if (CurrentBackupMembers + requestedMemberCount > 7)
        {
            EntryPoint.WriteToConsole($"CurrentBackupMembers + requestedMemberCount > 7 prerequestedMemberCount{requestedMemberCount}");
            requestedMemberCount = 7 - CurrentBackupMembers;
        }
        List<string> positiveReplies = new List<string>() {

                "Got some more guys on the way, hang on.",
                "Sending some more shooters to your location.",
                "Some more guys are on the way.",
            };
        Player.CellPhone.AddPhoneResponse(currentBackup.RequestedGang.Contact.Name, currentBackup.RequestedGang.Contact.IconName, positiveReplies.PickRandom());
        currentBackup.AddMoreMembers(requestedMemberCount, requiredVehicleModel);
    }

    private bool AddNewBackup(Gang gang, int requestedMemberCount, string requiredVehicleModel)
    {
        List<string> failReplies = new List<string>() {
                "Can't spare anyone now.",
                "Nobody around, sorry.",
                "You're going to have to deal with it on your own."
            };
        GangBackup gangBackup = new GangBackup(World, Player, gang, requestedMemberCount, requiredVehicleModel);
        gangBackup.Setup();
        if (!gangBackup.IsActive)
        {
            EntryPoint.WriteToConsole($"RequestBackup FAIL, NOT ACTIVE");
            // Nobody came, so nobody is paid for. AuthorizeBackup charged before the
            // dispatcher was asked, which is the only place the charge can live, so the
            // failure path has to hand it back.
            Player.GangRequisitionManager?.RefundBackup(gang, requestedMemberCount);
            Player.CellPhone.AddPhoneResponse(gang.Contact.Name, gang.Contact.IconName, failReplies.PickRandom());
            return false;
        }
        ActiveBackup.Add(gangBackup);
        EntryPoint.WriteToConsole("GangBackupManager RequestBackup Active backup Added");
        List<string> positiveReplies = new List<string>() {

                "Got some guys on the way, hang on.",
                "Sending some shooters to your location.",
                "Some guys are on the way.",
            };
        Player.CellPhone.AddPhoneResponse(gang.Contact.Name, gang.Contact.IconName, positiveReplies.PickRandom());
        return true;
    }
}

