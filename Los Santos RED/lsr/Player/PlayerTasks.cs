using ExtensionsMethods;
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
    public List<PlayerTask> PlayerTaskList { get; set; } = new List<PlayerTask>();
    public PlayerTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        PlayerTaskList.RemoveAll(x => !x.IsActive);
        foreach(PlayerTask pt in PlayerTaskList)
        {
            if(pt.CanExpire && DateTime.Compare(pt.ExpireTime, Time.CurrentDateTime) < 0)
            {
                pt.IsActive = false;
                pt.IsReadyForPayment = false;
                //expired?
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
    }
    public void CompletedTask(string contactName)
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
            EntryPoint.WriteToConsole($"Task Completed for {contactName}");
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
            EntryPoint.WriteToConsole($"Task Failed for {contactName}");
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
    public void RemoveTask(string contactName)
    {
        PlayerTaskList.RemoveAll(x => x.ContactName == contactName);
    }
    public PlayerTask GetTask(string contactName)
    {
        return PlayerTaskList.FirstOrDefault(x => x.ContactName == contactName);
    }
    public void PayoffGangToFriendly(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
            if (gr != null)
            {
                int CostToBuy = Player.GangRelationships.CostToPayoffGang(ActiveGang);
                if (CostToBuy <= 500)
                {
                    Player.GangRelationships.SetReputation(ActiveGang, 500, false);
                    List<string> Replies = new List<string>() {
                $"${Math.Abs(CostToBuy)}? Don't worry about it",
                $"We can forget about the ${Math.Abs(CostToBuy)}, not worth my time.",
                };
                    Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
                }
                else
                {
                    int RepToFriendly = 500 - gr.ReputationLevel;
                    CreateDeadDrop(ActiveGang, CostToBuy, RepToFriendly, true);
                }
            }
        }
    }
    public void PayoffGangToNeutral(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
            if (gr != null)
            {
                int CostToBuy = Player.GangRelationships.CostToPayoffGang(ActiveGang);
                int RepToNeutral = 0 - gr.ReputationLevel;
                CreateDeadDrop(ActiveGang, CostToBuy, RepToNeutral, true);
            }
        }
    }
    public void PayoffDebt(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
            if (gr != null)
            {
                CreateDebtDeadDrop(ActiveGang, gr.PlayerDebt);
            }
        }
    }


    public void GangPickupWork(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
            if (myDrop != null && myDen != null)
            {
                int PaymentAmount = RandomItems.GetRandomNumberInt(ActiveGang.PickupPaymentMin, ActiveGang.PickupPaymentMax).Round(100);
                int MoneyToPickup = PaymentAmount * 10;
                float TenPercent = (float)MoneyToPickup / 10;

                int MoneyToRecieve = (int)TenPercent;

                if (MoneyToRecieve <= 0)
                {
                    MoneyToRecieve = 500;
                }
                MoneyToRecieve = MoneyToRecieve.Round(10);
                myDrop.SetGang(ActiveGang, MoneyToPickup, false);
                ActiveDrops.Add(myDrop);
                myDen.ExpectedMoney = MoneyToPickup;

                AddTask(ActiveGang.ContactName, MoneyToRecieve,500, -1 * MoneyToPickup, -1000);

                List<string> Replies = new List<string>() {
                    $"Pickup ${MoneyToPickup} from {myDrop.StreetAddress}, its {myDrop.Description}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You get 10% on completion",
                    $"Go get ${MoneyToPickup} from {myDrop.Description}, address is {myDrop.StreetAddress}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. 10% to you when you drop it off",
                    $"Make a pickup of ${MoneyToPickup} from {myDrop.Description} on {myDrop.StreetAddress}. Take it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You'll get 10% when I get my money.",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());

                GameFiber DeadDropFiber = GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        PlayerTask CurrentTask = GetTask(ActiveGang.ContactName);
                        if (CurrentTask == null || !CurrentTask.IsActive)
                        {
                            EntryPoint.WriteToConsole($"Task Inactive for {ActiveGang.ContactName}");
                            break;
                        }
                        if(myDrop.InteractionComplete)
                        {
                            myDrop.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 3);
                        }
                        if (myDrop.InteractionComplete && !myDrop.IsNearby)
                        {
                            EntryPoint.WriteToConsole($"You did the pickup, so the den is now active!");
                            CurrentTask.IsReadyForPayment = true;

                            List<string> Replies2 = new List<string>() {
                                "Take the money to the designated place.",
                                "Now bring me the money, don't get lost",
                                "Remeber that is MY MONEY you are just holding it. Drop it off where we agreed.",
                                "Drop the money off at the designated place",
                                "Take the money where it needs to go",
                                "Bring the stuff back to us. Don't take long.",  };
                            Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies2.PickRandom(), 2);

                            break;
                        }
                        GameFiber.Sleep(1000);
                    }
                    //GetTask(ActiveGang.ContactName).IsReadyForPayment = true;
                }, "DeadDropFiber");
            }
            else
            {
                List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
            }
        }
    }
    public void GangTheftWork(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            bool IsCancelled = false;
            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);


            Gang TargetGang = null;
            if (ActiveGang.EnemyGangs != null && ActiveGang.EnemyGangs.Any())
            {
                TargetGang = Gangs.GetGang(ActiveGang.EnemyGangs.PickRandom());
            }
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().Where(x => x.ID != ActiveGang.ID).PickRandom();
            }
            if (myDen != null && TargetGang != null)
            {
                
                int MoneyToRecieve = RandomItems.GetRandomNumberInt(ActiveGang.TheftPaymentMin, ActiveGang.TheftPaymentMax).Round(500);

                if (MoneyToRecieve <= 0)
                {
                    MoneyToRecieve = 500;
                }

                DispatchableVehicle toget = TargetGang.GetRandomVehicle(0, false, false, true);
                if (toget != null)
                {
                    string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(toget.ModelName));
                    string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(toget.ModelName));


                    List<string> Replies = new List<string>() {
                    $"Go steal a ~p~{MakeName} {ModelName}~s~ from those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ assholes. Once you are done come back to {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} on completion",
                    $"Go get me a ~p~{MakeName} {ModelName}~s~ with {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ gang colors. Bring it back to the {ActiveGang.DenName} on {myDen.StreetAddress}. Payment ${MoneyToRecieve}",


                   // $"Go get ${MoneyToPickup} from {myDrop.Description}, address is {myDrop.StreetAddress}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} to get it done",
                   // $"Make a pickup of ${MoneyToPickup} from {myDrop.Description} on {myDrop.StreetAddress}. Take it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You'll get ${MoneyToRecieve} once its done.",
                    };
                    Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
                    //CustomiFruit.Close();

                    AddTask(ActiveGang.ContactName,MoneyToRecieve,1000,0,-500);
                    Gang toWatchGang = ActiveGang;
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        //float VehicleSpawnPercentage = Settings.SettingsManager.GangSettings.VehicleSpawnPercentage;
                        //Settings.SettingsManager.GangSettings.VehicleSpawnPercentage = 100f;
                        while (true)
                        {
                            PlayerTask CurrentTask = GetTask(ActiveGang.ContactName);
                            if (CurrentTask == null || !CurrentTask.IsActive)
                            {
                                EntryPoint.WriteToConsole($"Task Inactive for {ActiveGang.ContactName}");
                                break;
                            }
                            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Model.Name.ToLower() == toget.ModelName.ToLower() && Player.CurrentVehicle.WasModSpawned && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == TargetGang.ID)
                            {
                                EntryPoint.WriteToConsole($"You got in a {toget.ModelName.ToLower()} so the den is now ACTIVE!");
                                Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {ActiveGang.DenName} on {myDen.StreetAddress} to collect the ${MoneyToRecieve}",
                                $"Word got around that you are done with that thing for us, Come back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"Get back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"{myDen.StreetAddress} for ${MoneyToRecieve}",
                                $"Heard you were done, see you at the {ActiveGang.DenName} on {myDen.StreetAddress}. We owe you ${MoneyToRecieve}",
                                };
                                //CompletedTask(ActiveGang.ContactName);
                                Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom(), 2);

                                CurrentTask.IsReadyForPayment = true;
                                break;
                            }
                            GameFiber.Sleep(1000);
                        }
                        //Settings.SettingsManager.GangSettings.VehicleSpawnPercentage = VehicleSpawnPercentage;
                    }, "PayoffFiber");

                }
                else
                {
                    IsCancelled = true;
                }
            }
            else
            {
                IsCancelled = true;
            }

            if (IsCancelled)
            {
                List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
            }
        }
    }
    public void GangHitWork(Gang ActiveGang)
    {
        if (HasTask(ActiveGang?.ContactName))
        {
            AlreadyHasTasks(ActiveGang?.ContactName, ActiveGang?.ContactIcon);
        }
        else
        {
            bool IsCancelled = false;
            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
            Gang TargetGang = null;
            if (ActiveGang.EnemyGangs != null && ActiveGang.EnemyGangs.Any())
            {
                TargetGang = Gangs.GetGang(ActiveGang.EnemyGangs.PickRandom());
            }
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().Where(x => x.ID != ActiveGang.ID).PickRandom();
            }
            if (myDen != null && TargetGang != null)
            {
                
                int MoneyToRecieve = RandomItems.GetRandomNumberInt(ActiveGang.HitPaymentMin, ActiveGang.HitPaymentMax).Round(500);

                if (MoneyToRecieve <= 0)
                {
                    MoneyToRecieve = 500;
                }

                DispatchableVehicle toget = TargetGang.GetRandomVehicle(0, false, false, true);
                if (toget != null)
                {
                    string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(toget.ModelName));
                    string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(toget.ModelName));
                    List<string> Replies = new List<string>() {
                    $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ think they can fuck with me? Go give one of those pricks a dirt nap. Once you are done come back to the {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} to you",
                   $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ decided to make some moves against us. Let them know we don't approve with some gentle encouragement on a member. When you are finished, get back to the {ActiveGang.DenName} on {myDen.StreetAddress}. I'll have ${MoneyToRecieve} waiting for you.",
                   $"Go find one of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ pricks. Make sure he won't ever talk to anyone again. Come back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                    };
                    Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
                    //CustomiFruit.Close();
                    GangReputation gr = Player.GangRelationships.GetReputation(TargetGang);
                    int CurrentKilledMembers = gr.MembersKilled;
                    EntryPoint.WriteToConsole($"You are hired to kill starting kill = {CurrentKilledMembers}!");
                    AddTask(ActiveGang.ContactName, MoneyToRecieve, 2000, 0, -500);
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        while (true)
                        {
                            PlayerTask CurrentTask = GetTask(ActiveGang.ContactName);
                            if (CurrentTask == null || !CurrentTask.IsActive)
                            {
                                EntryPoint.WriteToConsole($"Task Inactive for {ActiveGang.ContactName}");
                                break;
                            }

                            //EntryPoint.WriteToConsole($"GANG HIT WORK - {ActiveGang.ShortName} Killed: {Player.GangRelationships.GetReputation(ActiveGang)?.MembersKilled} Goal Killed: {CurrentKilledMembers}");

                            if (Player.GangRelationships.GetReputation(TargetGang)?.MembersKilled > CurrentKilledMembers)
                            {
                                CurrentTask.IsReadyForPayment = true;
                                EntryPoint.WriteToConsole($"You killed a member so it is now ready for payment!");

                                Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {ActiveGang.DenName} on {myDen.StreetAddress} to collect the ${MoneyToRecieve}",
                                $"Word got around that you are done with that thing for us, Come back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"Get back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"{myDen.StreetAddress} for ${MoneyToRecieve}",
                                $"Heard you were done, see you at the {ActiveGang.DenName} on {myDen.StreetAddress}. We owe you ${MoneyToRecieve}",
                                };
                                //CompletedTask(ActiveGang.ContactName);
                                Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom(), 2);

                                break;
                            }
                            GameFiber.Sleep(1000);
                        }
                    }, "PayoffFiber");

                }
                else
                {
                    IsCancelled = true;
                }
            }
            else
            {
                IsCancelled = true;
            }

            if (IsCancelled)
            {
                List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
            }
        }
    }
    public void GangCancel(Gang ActiveGang)
    {
        PlayerTask currentAssignment = GetTask(ActiveGang.ContactName);
        if (currentAssignment != null)
        {
            FailTask(ActiveGang.ContactName);
            List<string> Replies = new List<string>() {
                    "I knew you were reliable",
                    "You really fucked me on this one",
                    "You are very helpful",
                    "This is a great time to fuck me like this prick",
                    "Whatever prick",
                    "Sorry I stuck my neck out for you",
                    };
            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
        }
    }
    private void CreateDeadDrop(Gang ActiveGang, int CostToBuy, int RepToSet, bool isDropOff)
    {
        DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
        if (myDrop != null)
        {
            AddTask(ActiveGang.ContactName, 0 , RepToSet, -1 * CostToBuy, -200);
            myDrop.SetGang(ActiveGang, CostToBuy, isDropOff);
            ActiveDrops.Add(myDrop);
            List<string> Replies = new List<string>() {
                $"Drop ${CostToBuy} on {myDrop.StreetAddress}, its {myDrop.Description}. My guy won't pick it up if you are around.",
                $"Place ${CostToBuy} in {myDrop.Description}, address is {myDrop.StreetAddress}. Don't hang around either, drop it off and leave.",
                $"Drop off ${CostToBuy} to {myDrop.Description} on {myDrop.StreetAddress}. Once you drop the cash off, get out of the area.",
                };

            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());

            GameFiber DeadDropFiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    PlayerTask CurrentTask = GetTask(ActiveGang.ContactName);
                    if (CurrentTask == null || !CurrentTask.IsActive)
                    {
                        EntryPoint.WriteToConsole($"Task Inactive for {ActiveGang.ContactName}");
                        break;
                    }
                    if (myDrop.InteractionComplete)
                    {
                        myDrop.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 3);
                    }
                    if (myDrop.InteractionComplete && !myDrop.IsNearby)
                    {
                        if (myDrop.IsDropOff)
                        {
                            if (Player.GangRelationships.IsHostile(ActiveGang))// CurrentTask.RepAmountOnCompletion <= 0)
                            {
                                Replies = new List<string>() {
                                "I guess we can forget about that shit.",
                                "No problem man, all is forgiven",
                                "That shit before? Forget about it.",
                                "We are square",
                                "You are off the hit list",
                                "This doesn't make us friends prick, just associates",

                                };
                            }
                            else
                            {
                                Replies = new List<string>() {
                                "Nice to get some respect from you finally, give us a call soon",
                                "Well this certainly smooths things over, come by to discuss things",
                                "I always liked you",
                                "Thanks for that, I'll remember it",
                                "Ah you got me my favorite thing! I owe you a thing or two",
                                };
                            }
                            CompletedTask(ActiveGang.ContactName);
                            Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom(), 0);
                            break;
                        }
                        else
                        {
                            List<string> Replies2 = new List<string>() {
                            "Take the money to the designated place.",
                            "Now bring me the money, don't get lost",
                            "Remeber that is MY MONEY you are just holding it. Drop it off where we agreed.",
                            "Drop the money off at the designated place",
                            "Take the money where it needs to go",
                            "Bring the stuff back to us. Don't take long.",  };
                            Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies2.PickRandom(), 0);
                            CurrentTask.IsReadyForPayment = true;
                            break;//need to pick up the cash for this one!
                        }
                    }
                    GameFiber.Sleep(1000);
                }
                //GetTask(ActiveGang.ContactName).IsReadyForPayment = true;
            }, "DeadDropFiber");


        }
    }

    private void CreateDebtDeadDrop(Gang ActiveGang, int CostToBuy)
    {
        DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
        if (myDrop != null)
        {
            AddTask(ActiveGang.ContactName, 0, 0, 0, 0);
            myDrop.SetGang(ActiveGang, CostToBuy, true);
            ActiveDrops.Add(myDrop);
            List<string> Replies = new List<string>() {
                $"Drop ${CostToBuy} on {myDrop.StreetAddress}, its {myDrop.Description}. My guy won't pick it up if you are around.",
                $"Place ${CostToBuy} in {myDrop.Description}, address is {myDrop.StreetAddress}. Don't hang around either, drop it off and leave.",
                $"Drop off ${CostToBuy} to {myDrop.Description} on {myDrop.StreetAddress}. Once you drop the cash off, get out of the area.",
                };

            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());

            GameFiber DeadDropFiber = GameFiber.StartNew(delegate
            {
                while (true)
                {
                    PlayerTask CurrentTask = GetTask(ActiveGang.ContactName);
                    if (CurrentTask == null || !CurrentTask.IsActive)
                    {
                        EntryPoint.WriteToConsole($"Task Inactive for {ActiveGang.ContactName}");
                        break;
                    }
                    if (myDrop.InteractionComplete)
                    {
                        myDrop.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 3);
                    }
                    if (myDrop.InteractionComplete && !myDrop.IsNearby)
                    {
                        GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
                        Replies = new List<string>() {
                        "I guess we are even now",
                        "Consider your debt paid",
                        "Debt wiped",
                        "You can stop looking over your shoulder now",
                        "We are square",
                        };
                        CompletedTask(ActiveGang.ContactName);
                        Player.CellPhone.AddScheduledText(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom(), 0);
                        break;
                        
                    }
                    GameFiber.Sleep(1000);
                }
                //GetTask(ActiveGang.ContactName).IsReadyForPayment = true;
            }, "DeadDropFiber");


        }
    }

    private void AlreadyHasTasks(string contactName, string contactIcon)
    {
        List<string> Replies = new List<string>() {
                    $"Aren't you already taking care of that thing for us?",
                    $"Didn't we already give you something to do?",
                    $"Finish your task before you call us again prick.",
                    $"Get going on that thing, stop calling me",
                    $"I alredy told you what to do, stop calling me.",
                    $"You already have an item, stop with the calls.",

                    };
        Player.CellPhone.AddPhoneResponse(contactName, contactIcon, Replies.PickRandom());
    }

}

