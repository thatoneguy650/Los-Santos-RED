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

    public List<PlayerTask> PlayerTaskList { get; set; } = new List<PlayerTask>();
    public PlayerTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Time = time;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
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
            myTask.IsActive = false;
            myTask.IsReadyForPayment = false;
        }
    }
    public bool HasTask(string contactName)
    {
        return PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive);
    }
    public void AddTask(string contactName, DateTime expireTime)
    {
        if (!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true) { ExpireTime = expireTime });
        }
    }
    public void AddTask(string contactName)
    {
        if(!PlayerTaskList.Any(x => x.ContactName == contactName && x.IsActive))
        {
            PlayerTaskList.Add(new PlayerTask(contactName, true));
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
            CreateDeadDrop(ActiveGang, CostToBuy, 500, true);
        }
    }
    public void PayoffGangToNeutral(Gang ActiveGang)
    {
        int CostToBuy = Player.GangRelationships.CostToPayoffGang(ActiveGang);
        CreateDeadDrop(ActiveGang,CostToBuy, 0, true);

    }
    public void GangPickupWork(Gang ActiveGang)
    {
        DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
        if (myDrop != null && myDen != null)
        {
            AddTask(ActiveGang.ContactName);
            int MoneyToPickup = RandomItems.GetRandomNumberInt(2000, 10000).Round(500);
            float TenPercent = (float)MoneyToPickup / 10;

            int MoneyToRecieve = (int)TenPercent;

            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
            MoneyToRecieve = MoneyToRecieve.Round(10);
            myDrop.SetGang(ActiveGang, MoneyToPickup, 0, false);
            ActiveDrops.Add(myDrop);
            myDen.ExpectedMoney = MoneyToPickup;
            myDen.RepOnDropOff = 500;
            myDen.MoneyOnDropOff = MoneyToRecieve;

            List<string> Replies = new List<string>() {
                    $"Pickup ${MoneyToPickup} from {myDrop.StreetAddress}, its {myDrop.Description}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You get 10% on completion",
                    $"Go get ${MoneyToPickup} from {myDrop.Description}, address is {myDrop.StreetAddress}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. 10% to you when you drop it off",
                    $"Make a pickup of ${MoneyToPickup} from {myDrop.Description} on {myDrop.StreetAddress}. Take it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You'll get 10% when I get my money.",
                    };
            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
            //CustomiFruit.Close();
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
            //CustomiFruit.Close();
        }

    }
    public void GangTheftWork(Gang ActiveGang)
    {
        //DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
        bool IsCancelled = false;
        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
        Gang TargetGang = Gangs.GetAllGangs().Where(x => x.ID != ActiveGang.ID).PickRandom();
        if (myDen != null && TargetGang != null)
        {
            AddTask(ActiveGang.ContactName);
            int MoneyToRecieve = RandomItems.GetRandomNumberInt(1000, 10000).Round(500);

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
                    $"Go steal a ~p~{MakeName} {ModelName}~s~ from {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~. Once you are done come back to {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} on completion",



                   // $"Go get ${MoneyToPickup} from {myDrop.Description}, address is {myDrop.StreetAddress}. Bring it to the {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} to get it done",
                   // $"Make a pickup of ${MoneyToPickup} from {myDrop.Description} on {myDrop.StreetAddress}. Take it to the {ActiveGang.DenName} on {myDen.StreetAddress}. You'll get ${MoneyToRecieve} once its done.",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
                //CustomiFruit.Close();


                Gang toWatchGang = ActiveGang;
                GameFiber PayoffFiber = GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Model.Name.ToLower() == toget.ModelName.ToLower() && Player.CurrentVehicle.WasModSpawned && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == toWatchGang.ID)
                        {
                            EntryPoint.WriteToConsole($"You got in a {toget.ModelName.ToLower()} so the den is now ACTIVE!");
                            break;
                        }
                        GameFiber.Yield();
                    }
                    myDen.RepOnDropOff = 1000;
                    myDen.MoneyOnDropOff = MoneyToRecieve;
                    GetTask(toWatchGang.ContactName).IsReadyForPayment = true;

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
            //CustomiFruit.Close(2000);
        }

    }
    public void GangHitWork(Gang ActiveGang)
    {
        bool IsCancelled = false;
        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
        Gang TargetGang = Gangs.GetAllGangs().Where(x => x.ID != ActiveGang.ID).PickRandom();
        if (myDen != null && TargetGang != null)
        {
            AddTask(ActiveGang.ContactName);
            int MoneyToRecieve = RandomItems.GetRandomNumberInt(10000, 30000).Round(500);

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
                    $"The {TargetGang.ShortName} thinks they can fuck with me? Go give one of those pricks a dirt nap. Once you are done come back to the {ActiveGang.DenName} on {myDen.StreetAddress}. ${MoneyToRecieve} to you",
                   $"The {TargetGang.ShortName} decided to make some moves against us. Let them know we don't approve with some gentle encouragement on a member. When you are finished, get back to the {ActiveGang.DenName} on {myDen.StreetAddress}. I'll have ${MoneyToRecieve} waiting for you.",
                   $"Go find one of those pricks from the {TargetGang.ShortName}. Make sure he won't ever talk to anyone again. Come back to the {ActiveGang.DenName} on {myDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                    };
                Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
                //CustomiFruit.Close();
                GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
                int CurrentKilledMembers = gr.MembersKilled;
                GameFiber PayoffFiber = GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        if (gr.MembersKilled > CurrentKilledMembers)
                        {
                            EntryPoint.WriteToConsole($"You killed a member so the den is now ACTIVE!");
                            break;
                        }
                        GameFiber.Yield();
                    }
                    myDen.RepOnDropOff = 2000;
                    myDen.MoneyOnDropOff = MoneyToRecieve;
                    GetTask(ActiveGang.ContactName).IsReadyForPayment = true;
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
            //CustomiFruit.Close();
        }

    }
    public void GangCancel(Gang ActiveGang)
    {
        RemoveTask(ActiveGang.ContactName);
        Player.GangRelationships.ChangeReputation(ActiveGang, -200, false);
        List<string> Replies = new List<string>() {
                    "I knew you were reliable",
                    "You really fucked me on this one",
                    "You are very helpful",
                    "This is a great time to fuck me like this prick",
                    "Whatever prick",
                    "Sorry I stuck my neck out for you",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
        //CustomiFruit.Close();
    }
    private void CreateDeadDrop(Gang ActiveGang, int CostToBuy, int RepToSet, bool isDropOff)
    {
        DeadDrop myDrop = PlacesOfInterest.PossibleLocations.DeadDrops.Where(x => !x.IsEnabled).PickRandom();
        if (myDrop != null)
        {
            AddTask(ActiveGang.ContactName);
            myDrop.SetGang(ActiveGang, CostToBuy, RepToSet, isDropOff);
            ActiveDrops.Add(myDrop);
            List<string> Replies = new List<string>() {
                    $"Drop ${CostToBuy} on {myDrop.StreetAddress}, its {myDrop.Description}. My guy won't pick it up if you are around.",
                    $"Place ${CostToBuy} in {myDrop.Description}, address is {myDrop.StreetAddress}. Don't hang around either, drop it off and leave.",
                    $"Drop off ${CostToBuy} to {myDrop.Description} on {myDrop.StreetAddress}. Once you drop the cash off, get out of the area.",
                    };

            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
            //CustomiFruit.Close();
        }
        else
        {
            //CustomiFruit.Close(500);
        }
    }



}

