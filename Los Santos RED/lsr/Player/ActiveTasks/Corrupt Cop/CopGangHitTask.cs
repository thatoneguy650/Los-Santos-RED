using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class CopGangHitTask : IPlayerTask
    {
        private ITaskAssignable Player;
        private ITimeReportable Time;
        private IGangs Gangs;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private PlayerTask CurrentTask;
        private int MoneyToRecieve;
        private Gang TargetGang;
        private int KillRequirement;
        private GangReputation CurrentGangReputation;
        private int CurrentKilledMembers;
        private DeadDrop myDrop;
        private CorruptCopContact Contact;

        public CopGangHitTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes,
            CorruptCopContact corruptCopContact, Gang targetGang, int killRequirement)
        {
            Player = player;
            Time = time;
            Gangs = gangs;
            PlayerTasks = playerTasks;
            PlacesOfInterest = placesOfInterest;
            ActiveDrops = activeDrops;
            Settings = settings;
            World = world;
            Crimes = crimes;
            Contact = corruptCopContact;
            TargetGang = targetGang;
            KillRequirement = killRequirement;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {

        }
        public void Start(CorruptCopContact contact)
        {
            Contact = contact;
            if(Contact == null)
            {
                return;
            }
            if (PlayerTasks.CanStartNewTask(Contact.Name))
            {
                GetGang();
                if (TargetGang != null)
                {
                    GetPayment();
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        try
                        {
                            Loop();
                            FinishTask();
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "PayoffFiber");
                }
                else
                {
                    SendTaskAbortMessage();
                }
            }
        }
        private void Loop()
        {
            while (true)
            {
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {Contact.Name}");
                    break;
                }
                if (Player.RelationshipManager.GangRelationships.GetReputation(TargetGang)?.MembersKilled > CurrentKilledMembers + KillRequirement - 1)
                {
                    CurrentTask.OnReadyForPayment(true);
                    //EntryPoint.WriteToConsoleTestLong($"You killed a member so it is now ready for payment!");
                    break;
                }
                GameFiber.Sleep(1000);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                StartDeadDropPayment();
            }
        }
        private void StartDeadDropPayment()
        {
            myDrop = PlacesOfInterest.GetUsableDeadDrop(World.IsMPMapLoaded, Player.CurrentLocation);
            if (myDrop != null)
            {
                myDrop.SetupDrop(MoneyToRecieve, false);
                ActiveDrops.Add(myDrop);
                SendDeadDropStartMessage();
                while (true)
                {
                    if (CurrentTask == null || !CurrentTask.IsActive)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {Contact.Name}");
                        break;
                    }
                    if (myDrop.InteractionComplete)
                    {
                        Game.DisplayHelp($"{Contact.Name} Money Picked Up");
                        //EntryPoint.WriteToConsoleTestLong($"Picked up money for Gang Hit for {Contact.Name}");
                        break;
                    }
                    GameFiber.Sleep(1000);
                }
                if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
                {
                    PlayerTasks.CompleteTask(Contact, true);
                }
                myDrop?.Reset();
                myDrop?.Deactivate(true);
            }
            else
            {    
                PlayerTasks.CompleteTask(Contact, true);
                SendQuickPaymentMessage();
            }
        }   
        private void AddTask()
        {
            CurrentGangReputation = Player.RelationshipManager.GangRelationships.GetReputation(TargetGang);
            CurrentKilledMembers = CurrentGangReputation.MembersKilled;
            //EntryPoint.WriteToConsoleTestLong($"You are hired to kill starting kill = {CurrentKilledMembers} MembersToKill {MembersToKill}!");
            PlayerTasks.AddTask(Contact, 0, 2000, 0, -500, 7,"Gang Hit");//money is receieved at the dead drop
            CurrentTask = PlayerTasks.GetTask(Contact.Name);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin, Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax).Round(500) * KillRequirement;
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void GetGang()
        {
            //TargetGang = null;
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().PickRandom();
            }
        }
        private void SendTaskAbortMessage()
        {
            List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies;
            if (KillRequirement == 1)
            {
                Replies = new List<string>() {
                    $"Some of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ decided to mess with our rackets. Get rid of one of them to send a message. ${MoneyToRecieve} on completion",
                    $"The {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ thought it was a good idea to try and take out a cop. Make sure one of them ends up dead. ${MoneyToRecieve}",
                    $"Need you to disappear a {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ member without asking a lot of questions. ${MoneyToRecieve} when you are done.",
                    };
            }
            else
            {
                Replies = new List<string>() {
                    $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ cocksuckers need to be put in their place. I want at least {KillRequirement} bodies in the street. ${MoneyToRecieve}",
                    $"You like money and killing people right? ${MoneyToRecieve} to get rid of {KillRequirement} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. A no brainer.",
                    $"Need some spectacle in the streets to keep our budget. Need you to waste {KillRequirement} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. ${MoneyToRecieve}",
                     };
            }
            Player.CellPhone.AddPhoneResponse(Contact.Name,Contact.IconName, Replies.PickRandom());
        }
        private void SendQuickPaymentMessage()
        {
            List<string> Replies = new List<string>() {
                            $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
                            $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
                            $"Sending your payment of ${MoneyToRecieve}",
                            $"Sending ${MoneyToRecieve}",
                            $"Heard you were done. We owe you ${MoneyToRecieve}",
                            };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 0, false);
        }
        private void SendDeadDropStartMessage()
        {
            List<string> Replies = new List<string>() {
                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.FullStreetAddress}, its {myDrop.Description}.",
                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.FullStreetAddress}.",
                            };

            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
        }
    }
}
