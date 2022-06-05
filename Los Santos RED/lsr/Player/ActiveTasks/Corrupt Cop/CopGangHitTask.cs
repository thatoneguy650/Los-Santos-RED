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
    public class CopGangHitTask
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
        private int MembersToKill;
        private GangReputation CurrentGangReputation;
        private int CurrentKilledMembers;
        private DeadDrop myDrop;

        public CopGangHitTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes)
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
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            //if (SpawnedVehicle.Exists())
            //{
            //    Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
            //    if (attachedBlip.Exists())
            //    {
            //        attachedBlip.Delete();
            //    }
            //    SpawnedVehicle.IsPersistent = false;
            //    SpawnedVehicle.Delete();
            //}
            //if (GunProp.Exists())
            //{
            //    GunProp.Delete();
            //}
        }
        public void Start()
        {
            if (PlayerTasks.CanStartNewTask(EntryPoint.OfficerFriendlyContactName))
            {
                GetGang();
                if (TargetGang != null)
                {
                    GetPayment();
                    MembersToKill = RandomItems.GetRandomNumberInt(1, 3);
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        Loop();
                        FinishTask();
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
                    EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.OfficerFriendlyContactName}");
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
            myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
            if (myDrop != null)
            {
                myDrop.SetupDrop(MoneyToRecieve, false);
                ActiveDrops.Add(myDrop);
                SendDeadDropStartMessage();
                while (true)
                {
                    if (CurrentTask == null || !CurrentTask.IsActive)
                    {
                        EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.OfficerFriendlyContactName}");
                        break;
                    }
                    if (myDrop.InteractionComplete)
                    {
                        EntryPoint.WriteToConsole($"Picked up money for Gang Hit for {EntryPoint.OfficerFriendlyContactName}");
                        break;
                    }
                    GameFiber.Sleep(1000);
                }
                if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
                {
                    PlayerTasks.CompleteTask(EntryPoint.OfficerFriendlyContactName, true);
                }
            }
            else
            {
                SendQuickPaymentMessage();
                PlayerTasks.CompleteTask(EntryPoint.OfficerFriendlyContactName, true);
            }
        }   
        private void AddTask()
        {
            CurrentGangReputation = Player.GangRelationships.GetReputation(TargetGang);
            CurrentKilledMembers = CurrentGangReputation.MembersKilled;
            EntryPoint.WriteToConsole($"You are hired to kill starting kill = {CurrentKilledMembers} MembersToKill {MembersToKill}!");
            PlayerTasks.AddTask(EntryPoint.OfficerFriendlyContactName, MoneyToRecieve, 2000, 0, -500, 7,"Gang Hit");
            CurrentTask = PlayerTasks.GetTask(EntryPoint.OfficerFriendlyContactName);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin, Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void GetGang()
        {
            TargetGang = null;
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
            Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies;
            if (MembersToKill == 1)
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
                    $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ cocksuckers need to be put in their place. I want at least {MembersToKill} bodies in the street. ${MoneyToRecieve}",
                    $"You like money and killing people right? ${MoneyToRecieve} to get rid of {MembersToKill} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. A no brainer.",
                    $"Need some spectacle in the streets to keep our budget. Need you to waste {MembersToKill} {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ members. ${MoneyToRecieve}",
                     };
            }
            Player.CellPhone.AddPhoneResponse(EntryPoint.OfficerFriendlyContactName, Replies.PickRandom());
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
            Player.CellPhone.AddScheduledText(EntryPoint.OfficerFriendlyContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 0);
        }
        private void SendDeadDropStartMessage()
        {
            List<string> Replies = new List<string>() {
                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.FullStreetAddress}, its {myDrop.Description}.",
                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.FullStreetAddress}.",
                            };

            Player.CellPhone.AddScheduledText(EntryPoint.OfficerFriendlyContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 2);
        }
    }
}
