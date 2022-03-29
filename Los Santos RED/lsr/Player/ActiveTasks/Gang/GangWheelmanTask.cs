using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangWheelmanTask
    {
        private ITaskAssignable Player;
        private ITimeControllable Time;
        private IGangs Gangs;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;

        private IWeapons Weapons;
        private INameProvideable Names;
        private IPedGroups PedGroups;
        private IShopMenus ShopMenus;
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private BasicLocation RobberyLocation;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private bool HasAddedComplications;
        private bool WillAddComplications;

        private int HoursToRobbery;
        private DateTime RobberyTime;
        private bool hasStartedGetaway;
        private bool hasSpawnedRobbers;
        private bool hasStartedRobbery;
        private dynamic PlayerGroup;
        private RelationshipGroup RobberRelationshipGroup;



        private List<GangMember> SpawnedRobbers = new List<GangMember>();

        private bool HasLocations => RobberyLocation != null && HiringGangDen != null;
        public GangWheelmanTask(ITaskAssignable player, ITimeControllable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus)
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
            Weapons = weapons;
            Names = names;
            PedGroups = pedGroups;
            ShopMenus = shopMenus;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            CleanupRobbers();
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetRobberyInformation();
                if (HasLocations)
                {
                    GetPayment();
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
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    EntryPoint.WriteToConsole($"Task Inactive for {HiringGang.ContactName}");
                    break;
                }
                if(!hasStartedRobbery)
                {
                    PreRobberyLoop();
                }
                else
                {
                    if(!PostRobberyLoop())//falsse is a break condition?
                    {
                        break;
                    }
                }
                GameFiber.Sleep(250);
            }
        }
        private void PreRobberyLoop()
        {
            float distanceTo = Player.Character.DistanceTo2D(RobberyLocation.EntrancePosition);
            if (DateTime.Compare(Time.CurrentDateTime, RobberyTime) >= 0)
            {
                hasStartedRobbery = true;
                if (distanceTo <= 50f)
                {
                    Game.FadeScreenOut(1500, true);
                    hasSpawnedRobbers = SpawnRobbers();
                    GameFiber.Sleep(1000);
                    Game.FadeScreenIn(1500, true);
                    if(hasSpawnedRobbers)
                    {
                        Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "ArmedRobbery"), false, Player.Character.Position, Player.CurrentVehicle, null, true, true, true);
                    }
                }
                Player.ButtonPrompts.RemovePrompts("RobberyStart");
            }
            else 
            {
                if (distanceTo <= 50f && Player.Character.Speed <= 0.25f && !Time.IsFastForwarding)
                {
                    Player.ButtonPrompts.AddPrompt("RobberyStart", "Start Robbery", "RobberyStart", Settings.SettingsManager.KeySettings.InteractCancel, 99);
                    if (Player.ButtonPrompts.IsHeld("RobberyStart"))
                    {
                        EntryPoint.WriteToConsole("RobberyStart You pressed fastforward to time");
                        Time.FastForward(RobberyTime);
                    }
                }
                else
                {
                    Player.ButtonPrompts.RemovePrompts("RobberyStart");
                }          
            }
        }
        private bool PostRobberyLoop()
        {
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            if (!hasSpawnedRobbers)
            {
                EntryPoint.WriteToConsole($"Failed as you werent close enough of the robbers didnt spawn!");
                return false;
            }
            if (hasSpawnedRobbers && !AreRobbersNormal())
            {
                return false;
            }
            if(!Player.IsAliveAndFree)
            {
                EntryPoint.WriteToConsole($"You got busted or died!");
                return false;
            }
            if (Player.IsNotWanted && !Player.Investigation.IsActive)
            {
                CurrentTask.IsReadyForPayment = true;
                EntryPoint.WriteToConsole($"You lost the cops so it is now ready for payment!");
                return false;
            }
            return true;
        }

        private bool AreRobbersNormal()
        {
            foreach(GangMember gm in SpawnedRobbers)
            {
                if(!gm.Pedestrian.Exists())
                {
                    EntryPoint.WriteToConsole($"Failed A robber got despawned soemhow!");
                    return false;
                }
                else if(gm.IsBusted)
                {
                    EntryPoint.WriteToConsole($"Failed The robber got caught!");
                    return false;
                }
                else if (gm.Pedestrian.IsDead)
                {
                    EntryPoint.WriteToConsole($"Failed A robber died!");
                    return false;
                }
                else if (gm.Pedestrian.DistanceTo2D(Player.Character) >= 250f)
                {
                    EntryPoint.WriteToConsole($"Failed A robber got left!");
                    return false;
                }
            }
            return true;
        }

        private bool SpawnRobbers()
        {
            bool spawnedOneRobber = false;
            int RobbersToSpawn = RandomItems.GetRandomNumberInt(1,3);
            for (int i = 0; i < RobbersToSpawn; i++)
            {
                if(SpawnRobber())
                {
                    spawnedOneRobber = true;
                }
            }
            return spawnedOneRobber;
        }
        private bool SpawnRobber()
        {
            if (RobberyLocation.EntrancePosition != Vector3.Zero)
            {
                DispatchablePerson RobberAccompliceInfo = HiringGang.Personnel.Where(x => x.CanCurrentlySpawn(0)).PickRandom();
                if (RobberAccompliceInfo != null)
                {
                    Vector3 ToSpawn = NativeHelper.GetOffsetPosition(RobberyLocation.EntrancePosition, RobberyLocation.EntranceHeading, 3f);
                    SpawnLocation toSpawn = new SpawnLocation(ToSpawn);
                    SpawnTask gmSpawn = new SpawnTask(HiringGang, toSpawn, null, RobberAccompliceInfo, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, false, Crimes, PedGroups, ShopMenus, World);
                    gmSpawn.AllowAnySpawn = true;
                    gmSpawn.AllowBuddySpawn = false;
                    gmSpawn.AttemptSpawn();
                    GangMember RobberAccomplice = (GangMember)gmSpawn.CreatedPeople.FirstOrDefault();
                    if(RobberAccomplice != null && RobberAccomplice.Pedestrian.Exists())
                    {
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(RobberAccomplice.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(RobberAccomplice.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(RobberAccomplice.Pedestrian, 0, false);
                        RobberAccomplice.WeaponInventory.IssueWeapons(Weapons, true, true, true);
                        RobberAccomplice.CanBeTasked = false;
                        RobberAccomplice.CanBeAmbientTasked = false;
                        RobberRelationshipGroup = new RelationshipGroup("ROBBERS");
                        RobberAccomplice.Pedestrian.RelationshipGroup = RobberRelationshipGroup;
                        RelationshipGroup.Cop.SetRelationshipWith(RobberRelationshipGroup, Relationship.Hate);
                        RobberRelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                        PlayerGroup = NativeFunction.Natives.GET_PLAYER_GROUP<int>(Game.LocalPlayer);
                        NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(RobberAccomplice.Pedestrian, PlayerGroup);
                        NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, PlayerGroup);
                        NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(RobberAccomplice.Pedestrian, 500000, 0);//TR
                        RobberAccomplice.Pedestrian.KeepTasks = true;
                        return true;
                    }
                } 
            }
            return false;
        }
        private void SetFailed()
        {
            EntryPoint.WriteToConsole("Gang Wheelman FAILED");
            CleanupRobbers();

            SendFailMessage();
            PlayerTasks.FailTask(HiringGang.ContactName);
        }
        private void SetCompleted()
        {
            CleanupRobbers();
            GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
            SendMoneyPickupMessage();
        }
        private void CleanupRobbers()
        {
            foreach (GangMember RobberAccomplice in SpawnedRobbers)
            {
                if (RobberAccomplice != null && RobberAccomplice.Pedestrian.Exists())
                {
                    RobberAccomplice.Pedestrian.IsPersistent = false;
                    Blip attachedBlip = RobberAccomplice.Pedestrian.GetAttachedBlip();
                    if (attachedBlip.Exists())
                    {
                        attachedBlip.Delete();
                    }
                    RobberAccomplice.CanBeTasked = true;
                    RobberAccomplice.CanBeAmbientTasked = true;
                }
            }
        }
        private void FinishTask()
        {
            Player.ButtonPrompts.RemovePrompts("RobberyStart");
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                SetCompleted();
            }
            if (CurrentTask != null && CurrentTask.IsActive)
            {
                GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
                SetFailed();
            }
            else
            {
                Dispose();
            }
        }
        private void GetRobberyInformation()
        {
            List<BasicLocation> PossibleSpots = new List<BasicLocation>();
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Banks);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.BeautyShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.ConvenienceStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Dispensaries);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.GasStations);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.HardwareStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.HeadShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.LiquorStores);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.PawnShops);
            PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Pharmacies);
            //PossibleSpots.AddRange(PlacesOfInterest.PossibleLocations.Restaurants);
            RobberyLocation = PossibleSpots.PickRandom();
            HiringGangDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == HiringGang.ID);
            HoursToRobbery = RandomItems.GetRandomNumberInt(8, 12);
            RobberyTime = Time.CurrentDateTime.AddHours(HoursToRobbery);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.WheelmanPaymentMin, HiringGang.WheelmanPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void AddTask()
        {
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);

            hasStartedGetaway = false;
            hasSpawnedRobbers = false;
            SpawnedRobbers.Clear();

            EntryPoint.WriteToConsole($"You are hired to wheelman!");
            PlayerTasks.AddTask(HiringGang.ContactName, MoneyToRecieve, 2000, 2000, -500, 7, "Gang Wheelman");
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                    $"We need a wheelman for a score that is going down. Location is the {RobberyLocation.Name} {RobberyLocation.StreetAddress} in {HoursToRobbery} hours. Once you are done come back to the {HiringGang.DenName} on {HiringGangDen.StreetAddress}. ${MoneyToRecieve} to you",
                    $"Get a fast car and head to the {RobberyLocation.Name} {RobberyLocation.StreetAddress}. It will go down in {HoursToRobbery} hours. When you are finished, get back to the {HiringGang.DenName} on {HiringGangDen.StreetAddress}. I'll have ${MoneyToRecieve} waiting for you.",
                   $"We need a driver for a job that we got planned. Get to the {RobberyLocation.Name} {RobberyLocation.StreetAddress}. Be there in {HoursToRobbery} hours. Afterwards, come back to the {HiringGang.DenName} on {HiringGangDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom());
        }
        private void SendMoneyPickupMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {HiringGang.DenName} on {HiringGangDen.StreetAddress} to collect the ${MoneyToRecieve}",
                                $"Word got around that you are done with that thing for us, Come back to the {HiringGang.DenName} on {HiringGangDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"Get back to the {HiringGang.DenName} on {HiringGangDen.StreetAddress} for your payment of ${MoneyToRecieve}",
                                $"{HiringGangDen.StreetAddress} for ${MoneyToRecieve}",
                                $"Heard you were done, see you at the {HiringGang.DenName} on {HiringGangDen.StreetAddress}. We owe you ${MoneyToRecieve}",
                                };
            Player.CellPhone.AddScheduledText(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom(), 2);
        }
        private void SendFailMessage()
        {
            List<string> Replies = new List<string>() {
                        $"You fucked that up pretty bad.",
                        $"Do you enjoy pissing me off? The whole job is ruined.",
                        $"You completely fucked up the job",
                        };
            Player.CellPhone.AddScheduledText(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom(), 0);
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
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, Replies.PickRandom());
        }
    }
}
