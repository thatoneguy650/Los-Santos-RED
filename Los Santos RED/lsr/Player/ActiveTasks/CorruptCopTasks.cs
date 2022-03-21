using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CorruptCopTasks
{

    private ITaskAssignable Player;
    private ITimeReportable Time;
    private IGangs Gangs;
    private PlayerTasks PlayerTasks;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;
    private PlayerTask CurrentTask;

    public CorruptCopTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Gangs = gangs;
        PlayerTasks = playerTasks;
        PlacesOfInterest = placesOfInterest;
        ActiveDrops = activeDrops;
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void GangHitWork()
    {
        if (PlayerTasks.CanStartNewTask(EntryPoint.OfficerFriendlyContactName))
        {
            string ContactName = EntryPoint.OfficerFriendlyContactName;
            string ContactIcon = Player.CellPhone.ContactList.FirstOrDefault(x => x.Name == ContactName)?.IconName;
            Gang TargetGang = null;
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().PickRandom();
            }
            if (TargetGang != null)
            {
                int MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin, Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax).Round(500);
                if (MoneyToRecieve <= 0)
                {
                    MoneyToRecieve = 500;
                }
                int MembersToKill = RandomItems.GetRandomNumberInt(1, 3);
                List<string> Replies;
                if (MembersToKill == 1)
                {
                    Replies = new List<string>() {
                    $"Some of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ might have ambushed some cops? How about you return the favor. Get rid of one of them to send a message. ${MoneyToRecieve} on completion",
                    $"The {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are trying to make some moves that we disagree with. Give them a funeral to think it over. ${MoneyToRecieve}",
                    $"Need you to disappear a {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ member without asking a lot of questions. ${MoneyToRecieve} when you are done.",
                    };
                }
                else
                {
                    Replies = new List<string>() {
                    $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ cocksuckers need to be put in their place. I want at least {MembersToKill} bodies in the street. ${MoneyToRecieve}",
                    $"You like money and killing people right? ${MoneyToRecieve} to get rid of {MembersToKill} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. A no brainer.",
                    $"Need some spectacle in the streets to keep our budget. Need you to waste {MembersToKill} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. ${MoneyToRecieve}",
                     };
                }
                Player.CellPhone.AddPhoneResponse(ContactName, Replies.PickRandom());
                GangReputation gr = Player.GangRelationships.GetReputation(TargetGang);
                int CurrentKilledMembers = gr.MembersKilled;
                EntryPoint.WriteToConsole($"You are hired to kill starting kill = {CurrentKilledMembers} MembersToKill {MembersToKill}!");
                PlayerTasks.AddTask(ContactName, MoneyToRecieve, 2000, 0, -500, 7);
                CurrentTask = PlayerTasks.GetTask(ContactName);
                GameFiber PayoffFiber = GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        if (CurrentTask == null || !CurrentTask.IsActive)
                        {
                            EntryPoint.WriteToConsole($"Task Inactive for {ContactName}");
                            break;
                        }
                        if (Player.GangRelationships.GetReputation(TargetGang)?.MembersKilled > CurrentKilledMembers + MembersToKill - 1)
                        {
                            CurrentTask.IsReadyForPayment = true;
                            EntryPoint.WriteToConsole($"You killed a member so it is now ready for payment!");
                            break;
                        }
                        GameFiber.Sleep(1000);
                    }
                    if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
                    {
                        DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
                        if (myDrop != null)
                        {
                            myDrop.SetupDrop(MoneyToRecieve, false);
                            ActiveDrops.Add(myDrop);
                            Replies = new List<string>() {
                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.StreetAddress}, its {myDrop.Description}.",
                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.StreetAddress}.",
                            };

                            Player.CellPhone.AddScheduledText(ContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 2);
                            while (true)
                            {
                                if (CurrentTask == null || !CurrentTask.IsActive)
                                {
                                    EntryPoint.WriteToConsole($"Task Inactive for {ContactName}");
                                    break;
                                }
                                if (myDrop.InteractionComplete)
                                {
                                    EntryPoint.WriteToConsole($"Picked up money for Gang Hit for {ContactName}");
                                    break;
                                }
                                GameFiber.Sleep(1000);
                            }
                            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
                            {
                                PlayerTasks.CompleteTask(ContactName);
                            }
                        }
                        else
                        {
                            Replies = new List<string>() {
                            $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
                            $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
                            $"Sending your payment of ${MoneyToRecieve}",
                            $"Sending ${MoneyToRecieve}",
                            $"Heard you were done. We owe you ${MoneyToRecieve}",
                            };
                            Player.CellPhone.AddScheduledText(ContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 0);
                            PlayerTasks.CompleteTask(ContactName);
                        }
                    }
                    else if (CurrentTask != null && CurrentTask.IsActive)
                    {
                        PlayerTasks.CancelTask(ContactName);
                    }

                }, "PayoffFiber");
            }
            else
            {
                GentlyAbortTaskCreation();
            }
        }
    }
    private void GentlyAbortTaskCreation()
    {
        List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
        Player.CellPhone.AddPhoneResponse(EntryPoint.OfficerFriendlyContactName, Replies.PickRandom());
    }

}

