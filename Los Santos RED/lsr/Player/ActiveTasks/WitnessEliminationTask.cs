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
    public class WitnessEliminationTask
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
        private DeadDrop myDrop;
        private INameProvideable Names;
        private bool WitnessIsMale;
        private string WitnessName;
        private Residence WitnessResidence;
        private Vector3 WitnessSpawnPosition;
        private float WitnessSpawnHeading;
        private readonly List<string> FemaleWitnessPossibleModels = new List<string>() { "a_f_o_ktown_01", "a_f_o_soucent_01","a_f_o_genstreet_01", "a_f_o_soucent_02", "a_f_y_bevhills_01", "a_f_y_bevhills_02","a_f_y_business_01", "a_f_y_business_02", "a_f_y_business_03", "a_f_y_business_04", 
            "a_f_y_genhot_01", "a_f_y_fitness_01", "a_f_m_business_02", "a_f_y_clubcust_01", "a_f_y_femaleagent","a_f_y_eastsa_03","a_f_y_hiker_01","a_f_y_hipster_01","a_f_y_hipster_04","a_f_y_skater_01","a_f_y_soucent_03","a_f_y_tennis_01","a_f_y_vinewood_01","a_f_y_tourist_02" };

        private readonly List<string> MaleWitnessPossibleModels = new List<string>() { "a_m_m_afriamer_01", "a_m_m_beach_01","a_m_m_bevhills_01", "a_m_m_bevhills_02", "a_m_m_business_01", "a_m_m_fatlatin_01","a_m_m_genfat_01", "a_m_m_malibu_01", "a_m_m_ktown_01", "a_m_m_mexcntry_01",
            "a_m_m_soucent_01", "a_m_m_soucent_02", "a_m_m_tourist_01", "a_m_y_bevhills_01", "a_m_y_bevhills_02","a_m_y_beachvesp_01","a_m_y_business_02","a_m_y_business_01","a_m_y_business_03","a_m_y_clubcust_01","a_m_y_genstreet_01","a_m_y_genstreet_02","a_m_y_hipster_01","a_m_y_hipster_03","a_m_y_ktown_02","a_m_y_polynesian_01","a_m_y_soucent_02" };

        private bool HasSpawnPosition => WitnessSpawnPosition != Vector3.Zero;
        private int SpawnPositionCellX;
        private int SpawnPositionCellY;
        private bool IsWitnessSpawned;
        private string WitnessModel;

        private PedExt Witness;
        private PedVariation WitnessVariation;

        private bool IsPlayerFarFromWitness => Witness != null && Witness.Pedestrian.Exists() && Witness.Pedestrian.DistanceTo2D(Player.Character) >= 850f;
        private bool IsPlayerNearWitnessSpawn => SpawnPositionCellX != -1 && SpawnPositionCellY != -1 && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, SpawnPositionCellX, SpawnPositionCellY, 5);
        public WitnessEliminationTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, INameProvideable names)
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
            Names = names;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            if(Witness != null && Witness.Pedestrian.Exists())
            {
                Blip attachedBlip = Witness.Pedestrian.GetAttachedBlip();
                if (attachedBlip.Exists())
                {
                    attachedBlip.Delete();
                }
                Witness.Pedestrian.IsPersistent = false;
                Witness.Pedestrian.Delete();
            }
        }
        public void Start()
        {
            if (PlayerTasks.CanStartNewTask(EntryPoint.OfficerFriendlyContactName))
            {
                GetPedInformation();
                if (HasSpawnPosition)
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
        private void GetPedInformation()
        {
            WitnessIsMale = RandomItems.RandomPercent(60);
            WitnessName = Names.GetRandomName(WitnessIsMale);
            WitnessResidence = PlacesOfInterest.PossibleLocations.Residences.Where(x => !x.IsOwnedOrRented).PickRandom();
            WitnessVariation = null;
            if (WitnessIsMale)
            {
                WitnessModel = MaleWitnessPossibleModels.PickRandom();
            }
            else
            {
                WitnessModel = FemaleWitnessPossibleModels.PickRandom();
            }
            if (WitnessResidence != null)
            {
                WitnessSpawnPosition = WitnessResidence.EntrancePosition;
                WitnessSpawnHeading = WitnessResidence.EntranceHeading;
                SpawnPositionCellX = (int)(WitnessSpawnPosition.X / EntryPoint.CellSize);
                SpawnPositionCellY = (int)(WitnessSpawnPosition.Y / EntryPoint.CellSize);
            }
            else
            {
                WitnessSpawnPosition = Vector3.Zero;
                SpawnPositionCellX = -1;
                SpawnPositionCellY = -1;
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
                if(!IsWitnessSpawned && IsPlayerNearWitnessSpawn)
                {
                    IsWitnessSpawned = SpawnWitness();
                }
                if(IsWitnessSpawned && IsPlayerFarFromWitness)
                {
                    DespawnWitness();
                }
                else if(IsWitnessSpawned && Witness != null && Witness.Pedestrian.Exists() && Witness.Pedestrian.IsDead)
                {
                    CurrentTask.IsReadyForPayment = true;
                    EntryPoint.WriteToConsole("Witness Elimination WITNESS WAS KILLED");
                    break;
                }
                if(IsWitnessSpawned && Witness != null && !Witness.Pedestrian.Exists())//somehow it got removed, set it as despawned
                {
                    DespawnWitness();
                }
                GameFiber.Sleep(1000);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                GameFiber.Sleep(RandomItems.GetRandomNumberInt(10000, 25000));
                SetCompleted();
            }
            else
            {
                Dispose();
            }
        }
        private void SetCompleted()
        {
            EntryPoint.WriteToConsole("Witness Elimination COMPLETED");
            SendCompletedMessage();
            PlayerTasks.CompleteTask(EntryPoint.OfficerFriendlyContactName);
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
                    PlayerTasks.CompleteTask(EntryPoint.OfficerFriendlyContactName);
                }
            }
            else
            {
                SendQuickPaymentMessage();
                PlayerTasks.CompleteTask(EntryPoint.OfficerFriendlyContactName);
            }
        }
        private void AddTask()
        {
            EntryPoint.WriteToConsole($"You are hired to kill a witness!");
            PlayerTasks.AddTask(EntryPoint.OfficerFriendlyContactName, MoneyToRecieve, 2000, 0, -500, 7);
            CurrentTask = PlayerTasks.GetTask(EntryPoint.OfficerFriendlyContactName);
            IsWitnessSpawned = false;
        }
        private bool SpawnWitness()
        {
            if (WitnessSpawnPosition != Vector3.Zero)
            {
                Ped ped = new Ped(WitnessModel, WitnessSpawnPosition, WitnessSpawnHeading);
                GameFiber.Yield();
                if (ped.Exists())
                {
                    Blip myBlip = ped.AttachBlip();
                    myBlip.Color = Color.DarkRed;
                    myBlip.Scale = 0.3f;

                    Witness = new PedExt(ped, Settings, Crimes, null, WitnessName, "Witness");
                    World.Pedestrians.AddEntity(Witness);
                    Witness.WasEverSetPersistent = true;
                    Witness.CanBeAmbientTasked = true;
                    Witness.CanBeTasked = true;
                    Witness.WasModSpawned = true;
                    if(WitnessVariation == null)
                    {
                        Witness.Pedestrian.RandomizeVariation();
                        WitnessVariation = NativeHelper.GetPedVariation(Witness.Pedestrian);
                    }
                    else
                    {
                        WitnessVariation.ApplyToPed(Witness.Pedestrian);
                    }
                    SendWitnessSpawnedMessage();

                    EntryPoint.WriteToConsole("Witness Elimination SPAWNED WITNESS");

                    return true;
                }
            }
            return false;
        }
        private void SendWitnessSpawnedMessage()
        {

        }
        private void DespawnWitness()
        {
            if (Witness != null && Witness.Pedestrian.Exists())
            {
                Witness.Pedestrian.Delete();
                EntryPoint.WriteToConsole("Witness Elimination DESPAWNED WITNESS");
            }
            IsWitnessSpawned = false;
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin, Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
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
            List<string> Replies = new List<string>() {
                    $"Got a witness that needs to disappear. Address is ~p~{WitnessResidence.StreetAddress}~s~. Name ~y~{WitnessName}~s~. ${MoneyToRecieve}",
                    $"Get to ~p~{WitnessResidence.StreetAddress}~s~ and get rid of ~y~{WitnessName}~s~. ${MoneyToRecieve} on complation",
                    $"We need to you shut this guy up before he squeals. He's at ~p~{WitnessResidence.StreetAddress}~s~. The name is ~y~{WitnessName}~s~. Payment of ${MoneyToRecieve}",
                    $"~y~{WitnessName}~s~ is at ~p~{WitnessResidence.StreetAddress}~s~. You know what to do. ${MoneyToRecieve}",
                    $"Need you to make sure ~y~{WitnessName}~s~ doesn't make it to the deposition, they live at ~p~{WitnessResidence.StreetAddress}~s~. ${MoneyToRecieve}",
                     };
            
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
                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.StreetAddress}, its {myDrop.Description}.",
                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.StreetAddress}.",
                            };

            Player.CellPhone.AddScheduledText(EntryPoint.OfficerFriendlyContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 2);
        }
        private void SendCompletedMessage()
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
    }

}
