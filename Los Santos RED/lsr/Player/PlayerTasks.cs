using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerTasks
{
    private ITaskAssignable Player;
    private ITimeReportable Time;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;
    private List<PlayerTask> LastContactTask = new List<PlayerTask>();
    public GangTasks GangTasks { get; private set; }
    public CorruptCopTasks CorruptCopTasks { get; private set; }
    public UndergroundGunsTasks UndergroundGunsTasks { get; private set; }
    public List<PlayerTask> PlayerTaskList { get; set; } = new List<PlayerTask>();
    public PlayerTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        GangTasks = new GangTasks(Player,Time,Gangs,this,PlacesOfInterest, ActiveDrops, Settings);
        CorruptCopTasks = new CorruptCopTasks(Player, Time, Gangs, this, PlacesOfInterest, ActiveDrops, Settings);
        UndergroundGunsTasks = new UndergroundGunsTasks(Player, Time, Gangs, this, PlacesOfInterest, ActiveDrops, Settings);
    }
    public void Setup()
    {
        GangTasks.Setup();
        CorruptCopTasks.Setup();
        UndergroundGunsTasks.Setup();
    }
    public void Update()
    {
        PlayerTaskList.RemoveAll(x => !x.IsActive);
        foreach(PlayerTask pt in PlayerTaskList.ToList())
        {
            if(pt != null && pt.CanExpire && DateTime.Compare(pt.ExpireTime, Time.CurrentDateTime) < 0)
            {
                ExpireTask(pt);
            }
        }
    }
    public void Clear()
    {
        PlayerTaskList.Clear();
    }
    public void Dispose()
    {
        PlayerTaskList.Clear();
        GangTasks.Dispose();
        CorruptCopTasks.Dispose();
        UndergroundGunsTasks.Dispose();
    }
    public void CancelTask(string contactName)
    {
        PlayerTask currentAssignment = GetTask(contactName);
        if (currentAssignment != null)
        {
            FailTask(contactName);
            List<string> Replies = new List<string>() {
                    "I knew you were reliable",
                    "You really fucked me on this one",
                    "You are very helpful",
                    "This is a great time to fuck me like this prick",
                    "Whatever prick",
                    "Sorry I stuck my neck out for you",
                    };
            Player.CellPhone.AddPhoneResponse(contactName, Replies.PickRandom());
        }
    }
    public void CompleteTask(string contactName)
    {
        PlayerTask myTask = PlayerTaskList.FirstOrDefault(x => x.ContactName == contactName && x.IsActive);
        if(myTask != null)
        {
            Gang myGang = Gangs.GetGangByContact(contactName);
            if (myGang != null)
            {
                if (myTask.RepAmountOnCompletion != 0)
                {
                    Player.GangRelationships.ChangeReputation(myGang, myTask.RepAmountOnCompletion, false);
                }
                Player.GangRelationships.SetDebt(myGang, 0);
            }
            if(myTask.PaymentAmountOnCompletion != 0)
            {
                Player.GiveMoney(myTask.PaymentAmountOnCompletion);
            }
            myTask.IsActive = false;
            myTask.IsReadyForPayment = false;
            myTask.WasCompleted = true;
            myTask.CompletionTime = Time.CurrentDateTime;
            EntryPoint.WriteToConsole($"Task Completed for {contactName}");
            LastContactTask.RemoveAll(x => x.ContactName == contactName);
            LastContactTask.Add(myTask);
        }
        PlayerTaskList.RemoveAll(x => x.ContactName == contactName);
    }
    public void FailTask(string contactName)
    {
        PlayerTask myTask = PlayerTaskList.FirstOrDefault(x => x.ContactName == contactName && x.IsActive);
        if (myTask != null)
        {
            Gang myGang = Gangs.GetGangByContact(contactName);
            if (myGang != null)
            {
                if (myTask.RepAmountOnFail != 0)
                {
                    Player.GangRelationships.ChangeReputation(myGang, myTask.RepAmountOnFail, false);
                }
                if (myTask.DebtAmountOnFail != 0)
                {
                    Player.GangRelationships.AddDebt(myGang, myTask.DebtAmountOnFail);
                }
            }
            myTask.IsActive = false;
            myTask.IsReadyForPayment = false;
            myTask.WasFailed = true;
            myTask.FailedTime = Time.CurrentDateTime;
            EntryPoint.WriteToConsole($"Task Failed for {contactName}");
            LastContactTask.RemoveAll(x => x.ContactName == contactName);
            LastContactTask.Add(myTask);
        }
        PlayerTaskList.RemoveAll(x => x.ContactName == contactName);
    }
    public bool HasTask(string contactName)
    {
        return PlayerTaskList.Any(x => x.ContactName.ToLower() == contactName.ToLower() && x.IsActive);
    }
    public void AddTask(string contactName, int moneyOnCompletion, int repOnCompletion, int debtOnFail, int repOnFail)
    {
        if (!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true) { PaymentAmountOnCompletion = moneyOnCompletion, RepAmountOnCompletion = repOnCompletion, DebtAmountOnFail = debtOnFail, RepAmountOnFail = repOnFail });
        }
    }
    public void AddTask(string contactName, int moneyOnCompletion, int repOnCompletion, int debtOnFail, int repOnFail, int daysToComplete)
    {
        if (!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true) { PaymentAmountOnCompletion = moneyOnCompletion, RepAmountOnCompletion = repOnCompletion, DebtAmountOnFail = debtOnFail, RepAmountOnFail = repOnFail, CanExpire = true, ExpireTime = Time.CurrentDateTime.AddDays(daysToComplete) });
        }
    }
    public void RemoveTask(string contactName)
    {
        PlayerTaskList.RemoveAll(x => x.ContactName == contactName);
    }
    public PlayerTask GetTask(string contactName)
    {
        return PlayerTaskList.FirstOrDefault(x => x.ContactName == contactName);
    }
    public bool CanStartNewTask(string contactName)
    {
        if(HasTask(contactName))
        {
            ReplyAlreadyHasTask(contactName);
            return false;
        }
        if(RecentlyEndedTask(contactName))
        {
            ReplyRecentlyEndedTask(contactName);
            return false;
        }
        return true;
    }
    private bool RecentlyEndedTask(string contactName)
    {
        PlayerTask lastTask = LastContactTask.FirstOrDefault(x => x.ContactName.ToLower() == contactName.ToLower());
        if(lastTask == null)
        {
            return false;
        }
        else
        {
            if(lastTask.WasCompleted)
            {
                if (DateTime.Compare(lastTask.CompletionTime.AddDays(Settings.SettingsManager.PlayerOtherSettings.DaysBetweenTasksWhenCompleted), Time.CurrentDateTime) < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (lastTask.WasFailed)
            {
                if (DateTime.Compare(lastTask.FailedTime.AddDays(Settings.SettingsManager.PlayerOtherSettings.DaysBetweenTasksWhenFailed), Time.CurrentDateTime) < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
    private void ReplyAlreadyHasTask(string contactName)
    {
        List<string> Replies = new List<string>() {
                    $"Aren't you already taking care of that thing for us?",
                    $"Didn't we already give you something to do?",
                    $"Finish your task before you call us again prick.",
                    $"Get going on that thing, stop calling me",
                    $"I alredy told you what to do, stop calling me.",
                    $"You already have an item, stop with the calls.",

                    };
        Player.CellPhone.AddPhoneResponse(contactName, Replies.PickRandom());
    }
    private void ReplyRecentlyEndedTask(string contactName)
    {
        List<string> Replies = new List<string>() {
                    $"Let the heat die down for a bit. Give me a call tomorrow.",
                    $"Didn't you just get done with that thing? Give us some time.",
                    $"You should lay low for a bit after that thing. Call us in a few.",

                    };
        Player.CellPhone.AddPhoneResponse(contactName, Replies.PickRandom());
    }
    private void ExpireTask(PlayerTask pt)
    {
        if (pt != null)
        {
            FailTask(pt.ContactName);
            List<string> Replies = new List<string>() {
                    "You were supposed to take care of that thing this century, forget about it.",
                    "That thing we talked about? Time expired.",
                    "Too late on that thing, forget about it.",
                    "Needed you to get to that thing quicker",
                    "Forget about the work item, I had someone else take care of it.",
                    "Are you always this slow? I'll get someone else to handle things.",
                    };
            iFruitContact ifc = Player.CellPhone.ContactList.FirstOrDefault(x => x.Name == pt.ContactName);

            Player.CellPhone.AddPhoneResponse(pt.ContactName, ifc?.IconName, Replies.PickRandom());
        }
    }
}
