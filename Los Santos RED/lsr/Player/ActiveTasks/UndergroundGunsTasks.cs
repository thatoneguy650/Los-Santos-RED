using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class UndergroundGunsTasks
{

    private ITaskAssignable Player;
    private ITimeReportable Time;
    private IGangs Gangs;
    private PlayerTasks PlayerTasks;
    private IPlacesOfInterest PlacesOfInterest;
    private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
    private ISettingsProvideable Settings;


    private PlayerTask CurrentTask;

    private Vehicle SpawnedVehicle = null;

    public UndergroundGunsTasks(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings)
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
        if (SpawnedVehicle.Exists())
        {
            Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
            if (attachedBlip.Exists())
            {
                attachedBlip.Delete();
            }
            SpawnedVehicle.IsPersistent = false;
        }
    }
    public void GunPickupWork()
    {
        if (PlayerTasks.CanStartNewTask(EntryPoint.UndergroundGunsContactName))
        {
            GunStore DropOffStore = PlacesOfInterest.PossibleLocations.GunStores.Where(x => x.ContactName == EntryPoint.UndergroundGunsContactName && x.IsEnabled).PickRandom();
            GunStore PickUpStore = null;
            if (DropOffStore != null)
            {
                PickUpStore = PlacesOfInterest.PossibleLocations.GunStores.Where(x => x.ContactName == EntryPoint.UndergroundGunsContactName && x.Name != DropOffStore.Name && x.ParkingSpot != Vector3.Zero).PickRandom();
            }
            if (DropOffStore != null && PickUpStore != null)
            {
                int MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMin, Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMax).Round(500);
                if (MoneyToRecieve <= 0)
                {
                    MoneyToRecieve = 500;
                }
                List<string> Replies;
                Replies = new List<string>() {
                $"Need you to pickup some guns from our shop on {PickUpStore.StreetAddress} and bring them to the shop on {DropOffStore.StreetAddress}. They are loaded in the ~p~Burrito Van~s~ out front. ${MoneyToRecieve} when you are done",
                    };



                Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
                EntryPoint.WriteToConsole($"Starting Underground Guns Pickup Guns Task");
                PlayerTasks.AddTask(EntryPoint.UndergroundGunsContactName, MoneyToRecieve, 2000, 0, -500, 7);
                CurrentTask = PlayerTasks.GetTask(EntryPoint.UndergroundGunsContactName);

                bool hasSpawnedCar = false;

                GameFiber PayoffFiber = GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        if (CurrentTask == null || !CurrentTask.IsActive)
                        {
                            EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.UndergroundGunsContactName}");
                            break;
                        }
                        if (!hasSpawnedCar && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, PickUpStore.CellX, PickUpStore.CellY, 5))
                        {
                            hasSpawnedCar = SpawnVehicle(PickUpStore);
                        }
                        if (hasSpawnedCar && (!SpawnedVehicle.Exists() || SpawnedVehicle.Health <= 300 || SpawnedVehicle.EngineHealth <= 300))
                        {
                            EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.UndergroundGunsContactName}");
                            break;
                        }
                        if (hasSpawnedCar && SpawnedVehicle.Exists() && SpawnedVehicle.Driver?.Handle == Player.Character.Handle && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, DropOffStore.CellX, DropOffStore.CellY, 2) && Player.Character.Speed <= 1.0f)
                        {
                            SpawnedVehicle.IsPersistent = false;
                            CurrentTask.IsReadyForPayment = true;
                            EntryPoint.WriteToConsole($"You ARRIVED! so it is now ready for payment!");
                            break;
                        }
                        GameFiber.Sleep(1000);
                    }
                    if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
                    {
                        Replies = new List<string>() {
                        $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
                        $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
                        $"Sending your payment of ${MoneyToRecieve}",
                        $"Sending ${MoneyToRecieve}",
                        $"Heard you were done. We owe you ${MoneyToRecieve}",
                        };
                        Player.CellPhone.AddScheduledText(EntryPoint.UndergroundGunsContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 0);
                        PlayerTasks.CompleteTask(EntryPoint.UndergroundGunsContactName);
                    }
                    else if (CurrentTask != null && CurrentTask.IsActive)
                    {
                        if(SpawnedVehicle.Exists())
                        {
                            Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
                            if (attachedBlip.Exists())
                            {
                                attachedBlip.Delete();
                            }
                            SpawnedVehicle.IsPersistent = false;
                        }
                        PlayerTasks.CancelTask(EntryPoint.UndergroundGunsContactName);
                    }
                    else
                    {
                        if (SpawnedVehicle.Exists())
                        {
                            Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
                            if (attachedBlip.Exists())
                            {
                                attachedBlip.Delete();
                            }
                            SpawnedVehicle.IsPersistent = false;
                        }
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
        Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
    }
    private bool SpawnVehicle(GunStore PickUpStore)
    {

        SpawnLocation SpawnLocation = new SpawnLocation(PickUpStore.EntrancePosition);


        SpawnLocation.StreetPosition = PickUpStore.ParkingSpot;
        SpawnLocation.Heading = PickUpStore.ParkingHeading;


       // SpawnLocation.GetClosestStreet();

        if (SpawnLocation.StreetPosition != Vector3.Zero)
        {
            SpawnedVehicle = new Vehicle("burrito", SpawnLocation.StreetPosition, SpawnLocation.Heading);
            GameFiber.Yield();
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.PrimaryColor = System.Drawing.Color.Black;
                SpawnedVehicle.IsPersistent = true;
                Blip myBlip = SpawnedVehicle.AttachBlip();
                myBlip.Color = Color.DarkRed;
                myBlip.Scale = 0.3f;

                Player.LastFriendlyVehicle = null;
                Player.LastFriendlyVehicle = SpawnedVehicle;



                List<string> PickupMessage = new List<string>() {
                        $"The Black ~p~Burrito Van~s~ is parked out front, plate number is {SpawnedVehicle.LicensePlate}. Keys should be in it.",
                        };
                Player.CellPhone.AddScheduledText(EntryPoint.UndergroundGunsContactName, "CHAR_BLANK_ENTRY", PickupMessage.PickRandom(), 0);


                return true;
            }
        }
        return false;
    }


}
