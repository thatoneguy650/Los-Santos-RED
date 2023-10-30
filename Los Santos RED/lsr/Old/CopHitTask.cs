//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;

//namespace LosSantosRED.lsr.Player.ActiveTasks
//{
//    public class CopHitTask : IPlayerTask
//    {
//        private ITaskAssignable Player;
//        private ITimeReportable Time;
//        private IGangs Gangs;
//        private IWeapons Weapons;
//        private PlayerTasks PlayerTasks;
//        private IPlacesOfInterest PlacesOfInterest;
//        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
//        private ISettingsProvideable Settings;
//        private IEntityProvideable World;
//        private ICrimes Crimes;
//        private PlayerTask CurrentTask;
//        private int MoneyToRecieve;
//        private DeadDrop myDrop;
//        private INameProvideable Names;
//        private bool TargetCopIsMale;
//        private string TargetCopName;
//        private Residence TargetCopResidence;
//        private Vector3 TargetCopSpawnPosition;
//        private float TargetCopSpawnHeading;
//        private readonly List<string> FemaleTargetCopPossibleModels = new List<string>() { "a_f_o_ktown_01", "a_f_o_soucent_01","a_f_o_genstreet_01", "a_f_o_soucent_02", "a_f_y_bevhills_01", "a_f_y_bevhills_02","a_f_y_business_01", "a_f_y_business_02", "a_f_y_business_03", "a_f_y_business_04",
//            "a_f_y_genhot_01", "a_f_y_fitness_01", "a_f_m_business_02", "a_f_y_clubcust_01", "a_f_y_femaleagent","a_f_y_eastsa_03","a_f_y_hiker_01","a_f_y_hipster_01","a_f_y_hipster_04","a_f_y_skater_01","a_f_y_soucent_03","a_f_y_tennis_01","a_f_y_vinewood_01","a_f_y_tourist_02" };

//        private readonly List<string> MaleTargetCopPossibleModels = new List<string>() { "a_m_m_afriamer_01", "a_m_m_beach_01","a_m_m_bevhills_01", "a_m_m_bevhills_02", "a_m_m_business_01", "a_m_m_fatlatin_01","a_m_m_genfat_01", "a_m_m_malibu_01", "a_m_m_ktown_01", "a_m_m_mexcntry_01",
//            "a_m_m_soucent_01", "a_m_m_soucent_02", "a_m_m_tourist_01", "a_m_y_bevhills_01", "a_m_y_bevhills_02","a_m_y_beachvesp_01","a_m_y_business_02","a_m_y_business_01","a_m_y_business_03","a_m_y_clubcust_01","a_m_y_genstreet_01","a_m_y_genstreet_02","a_m_y_hipster_01","a_m_y_hipster_03","a_m_y_ktown_02","a_m_y_polynesian_01","a_m_y_soucent_02" };

//        private bool HasSpawnPosition => TargetCopSpawnPosition != Vector3.Zero;
//        private int SpawnPositionCellX;
//        private int SpawnPositionCellY;
//        private bool IsTargetCopSpawned;
//        private int GameTimeToWaitBeforeComplications;
//        private string TargetCopModel;

//        private PedExt TargetCop;
//        private PedVariation TargetCopVariation;
//        private bool HasAddedComplications;
//        private bool WillAddComplications;
//        private CorruptCopContact Contact;

//        private bool IsPlayerFarFromTargetCop => TargetCop != null && TargetCop.Pedestrian.Exists() && TargetCop.Pedestrian.DistanceTo2D(Player.Character) >= 850f;
//        private bool IsPlayerNearTargetCopSpawn => SpawnPositionCellX != -1 && SpawnPositionCellY != -1 && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, SpawnPositionCellX, SpawnPositionCellY, 5);
//        public CopHitTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, 
//            INameProvideable names, IWeapons weapons, CorruptCopContact corruptCopContact)
//        {
//            Player = player;
//            Time = time;
//            Gangs = gangs;
//            PlayerTasks = playerTasks;
//            PlacesOfInterest = placesOfInterest;
//            ActiveDrops = activeDrops;
//            Settings = settings;
//            World = world;
//            Crimes = crimes;
//            Names = names;
//            Weapons = weapons;
//            Contact = corruptCopContact;
//        }
//        public void Setup()
//        {

//        }
//        public void Dispose()
//        {
//            if (TargetCop != null && TargetCop.Pedestrian.Exists())
//            {
//                Blip attachedBlip = TargetCop.Pedestrian.GetAttachedBlip();
//                if (attachedBlip.Exists())
//                {
//                    attachedBlip.Delete();
//                }
//                TargetCop.Pedestrian.IsPersistent = false;
//                TargetCop.Pedestrian.Delete();
//            }
//        }
//        public void Start(CorruptCopContact contact)
//        {
//            Contact = contact;
//            if (Contact == null)
//            {
//                return;
//            }
//            if (PlayerTasks.CanStartNewTask(Contact.Name))
//            {
//                GetPedInformation();
//                if (HasSpawnPosition)
//                {
//                    GetPayment();
//                    SendInitialInstructionsMessage();
//                    AddTask();
//                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
//                    {
//                        try
//                        {
//                            Loop();
//                            FinishTask();
//                        }
//                        catch (Exception ex)
//                        {
//                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                            EntryPoint.ModController.CrashUnload();
//                        }
//                    }, "PayoffFiber");
//                }
//                else
//                {
//                    SendTaskAbortMessage();
//                }
//            }
//            else
//            {
//                SendTaskAbortMessage();
//            }
//        }
//        private void GetPedInformation()
//        {
//            TargetCopIsMale = RandomItems.RandomPercent(60);
//            TargetCopName = Names.GetRandomName(TargetCopIsMale);
//            TargetCopResidence = PlacesOfInterest.PossibleLocations.Residences.Where(x => !x.IsOwnedOrRented).PickRandom();
//            TargetCopVariation = null;
//            if (TargetCopIsMale)
//            {
//                TargetCopModel = MaleTargetCopPossibleModels.PickRandom();
//            }
//            else
//            {
//                TargetCopModel = FemaleTargetCopPossibleModels.PickRandom();
//            }
//            if (TargetCopResidence != null)
//            {
//                TargetCopSpawnPosition = TargetCopResidence.EntrancePosition;
//                TargetCopSpawnHeading = TargetCopResidence.EntranceHeading;
//                SpawnPositionCellX = (int)(TargetCopSpawnPosition.X / EntryPoint.CellSize);
//                SpawnPositionCellY = (int)(TargetCopSpawnPosition.Y / EntryPoint.CellSize);
//            }
//            else
//            {
//                TargetCopSpawnPosition = Vector3.Zero;
//                SpawnPositionCellX = -1;
//                SpawnPositionCellY = -1;
//            }
//        }
//        private void Loop()
//        {
//            while (true)
//            {
//                if (CurrentTask == null || !CurrentTask.IsActive)
//                {
//                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {Contact.Name}");
//                    break;
//                }
//                if (!IsTargetCopSpawned && IsPlayerNearTargetCopSpawn)
//                {
//                    IsTargetCopSpawned = SpawnTargetCop();
//                }
//                if (IsTargetCopSpawned && IsPlayerFarFromTargetCop)
//                {
//                    DespawnTargetCop();
//                }
//                else if (IsTargetCopSpawned && TargetCop != null && TargetCop.Pedestrian.Exists() && TargetCop.Pedestrian.IsDead)
//                {
//                    CurrentTask.OnReadyForPayment(true);
//                    break;
//                }
//                if (IsTargetCopSpawned && TargetCop != null && !TargetCop.Pedestrian.Exists())//somehow it got removed, set it as despawned
//                {
//                    DespawnTargetCop();
//                }
//                GameFiber.Sleep(1000);
//            }
//        }
//        private void FinishTask()
//        {
//            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
//            {
//                GameFiber.Sleep(RandomItems.GetRandomNumberInt(10000, 25000));
//                SetCompleted();
//            }
//            else
//            {
//                Dispose();
//            }
//        }
//        private void SetCompleted()
//        {
//            //EntryPoint.WriteToConsoleTestLong("Witness Elimination COMPLETED");
//            SendCompletedMessage();
//            PlayerTasks.CompleteTask(Contact, true);
//        }
//        private void StartDeadDropPayment()
//        {
//            myDrop = PlacesOfInterest.GetUsableDeadDrop(World.IsMPMapLoaded);
//            if (myDrop != null)
//            {
//                myDrop.SetupDrop(MoneyToRecieve, false);
//                ActiveDrops.Add(myDrop);
//                SendDeadDropStartMessage();
//                while (true)
//                {
//                    if (CurrentTask == null || !CurrentTask.IsActive)
//                    {
//                        //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {Contact.Name}");
//                        break;
//                    }
//                    if (myDrop.InteractionComplete)
//                    {
//                        Game.DisplayHelp($"{Contact.Name} Money Picked Up");
//                        //EntryPoint.WriteToConsoleTestLong($"Picked up money for Gang Hit for {Contact.Name}");
//                        break;
//                    }
//                    GameFiber.Sleep(1000);
//                }
//                if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
//                {
//                    PlayerTasks.CompleteTask(Contact, true);
//                }
//            }
//            else
//            {
//                SendQuickPaymentMessage();
//                PlayerTasks.CompleteTask(Contact, true);
//            }
//        }
//        private void AddTask()
//        {
//            //EntryPoint.WriteToConsoleTestLong($"You are hired to kill a witness!");
//            PlayerTasks.AddTask(Contact, MoneyToRecieve, 2000, 0, -500, 7, "Witness Elimination");
//            CurrentTask = PlayerTasks.GetTask(Contact.Name);
//            IsTargetCopSpawned = false;

//            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
//            HasAddedComplications = false;
//            WillAddComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationComplicationsPercentage);


//        }
//        private bool SpawnTargetCop()
//        {
//            if (TargetCopSpawnPosition != Vector3.Zero)
//            {
//                Ped ped = new Ped(TargetCopModel, TargetCopSpawnPosition, TargetCopSpawnHeading);
//                GameFiber.Yield();
//                if (ped.Exists())
//                {
//                    Blip myBlip = ped.AttachBlip();
//                    EntryPoint.WriteToConsole($"PEDEXT BLIP CREATED");
//                    myBlip.Color = Color.DarkRed;
//                    myBlip.Scale = 0.3f;
//                    string GroupName = "Man";
//                    if (!TargetCopIsMale)
//                    {
//                        GroupName = "Woman";
//                    }
//                    TargetCop = new PedExt(ped, Settings, Crimes, Weapons, TargetCopName, GroupName, World);
//                    World.Pedestrians.AddEntity(TargetCop);
//                    TargetCop.WasEverSetPersistent = true;
//                    TargetCop.CanBeAmbientTasked = true;
//                    TargetCop.CanBeTasked = true;
//                    TargetCop.WasModSpawned = true;
//                    if (TargetCopVariation == null)
//                    {
//                        TargetCop.Pedestrian.RandomizeVariation();
//                        TargetCopVariation = NativeHelper.GetPedVariation(TargetCop.Pedestrian);
//                    }
//                    else
//                    {
//                        TargetCopVariation.ApplyToPed(TargetCop.Pedestrian);
//                    }
//                    SendWitnessSpawnedMessage();

//                    //EntryPoint.WriteToConsoleTestLong("Witness Elimination SPAWNED WITNESS");

//                    return true;
//                }
//            }
//            return false;
//        }
//        private void SendWitnessSpawnedMessage()
//        {

//        }
//        private void DespawnTargetCop()
//        {
//            if (TargetCop != null && TargetCop.Pedestrian.Exists())
//            {
//                TargetCop.Pedestrian.Delete();
//                //EntryPoint.WriteToConsoleTestLong("Witness Elimination DESPAWNED WITNESS");
//            }
//            IsTargetCopSpawned = false;
//        }
//        private void GetPayment()
//        {
//            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin, Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax).Round(500);
//            if (MoneyToRecieve <= 0)
//            {
//                MoneyToRecieve = 500;
//            }
//        }
//        private void SendTaskAbortMessage()
//        {
//            List<string> Replies = new List<string>() {
//                    "Nothing yet, I'll let you know",
//                    "I've got nothing for you yet",
//                    "Give me a few days",
//                    "Not a lot to be done right now",
//                    "We will let you know when you can do something for us",
//                    "Check back later.",
//                    };
//            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
//        }
//        private void SendInitialInstructionsMessage()
//        {
//            List<string> Replies = new List<string>() {
//                    $"Got a witness that needs to disappear. Address is ~p~{TargetCopResidence.FullStreetAddress}~s~. Name ~y~{TargetCopName}~s~. ${MoneyToRecieve}",
//                    $"Get to ~p~{TargetCopResidence.FullStreetAddress}~s~ and get rid of ~y~{TargetCopName}~s~. ${MoneyToRecieve} on complation",
//                    $"We need to you shut this guy up before he squeals. He's at ~p~{TargetCopResidence.FullStreetAddress}~s~. The name is ~y~{TargetCopName}~s~. Payment of ${MoneyToRecieve}",
//                    $"~y~{TargetCopName}~s~ is at ~p~{TargetCopResidence.FullStreetAddress}~s~. You know what to do. ${MoneyToRecieve}",
//                    $"Need you to make sure ~y~{TargetCopName}~s~ doesn't make it to the deposition, they live at ~p~{TargetCopResidence.FullStreetAddress}~s~. ${MoneyToRecieve}",
//                     };

//            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
//        }
//        private void SendQuickPaymentMessage()
//        {
//            List<string> Replies = new List<string>() {
//                            $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
//                            $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
//                            $"Sending your payment of ${MoneyToRecieve}",
//                            $"Sending ${MoneyToRecieve}",
//                            $"Heard you were done. We owe you ${MoneyToRecieve}",
//                            };
//            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 0, false);
//        }
//        private void SendDeadDropStartMessage()
//        {
//            List<string> Replies = new List<string>() {
//                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.FullStreetAddress}, its {myDrop.Description}.",
//                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.FullStreetAddress}.",
//                            };

//            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
//        }
//        private void SendCompletedMessage()
//        {
//            List<string> Replies = new List<string>() {
//                        $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
//                        $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
//                        $"Sending your payment of ${MoneyToRecieve}",
//                        $"Sending ${MoneyToRecieve}",
//                        $"Heard you were done. We owe you ${MoneyToRecieve}",
//                        };
//            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 0, false);
//        }
//    }

//}
