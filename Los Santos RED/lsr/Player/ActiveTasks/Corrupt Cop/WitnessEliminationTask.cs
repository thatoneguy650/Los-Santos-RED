using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class WitnessEliminationTask : IPlayerTask
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
        private IShopMenus ShopMenus;
        private PlayerTask CurrentTask;
        private int MoneyToRecieve;
        private DeadDrop myDrop;
        private IWeapons Weapons;
        private INameProvideable Names;
        private bool WitnessIsMale;
        private string WitnessName;
        private bool WitnessIsAtHome;
        private GameLocation WitnessLocation;
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
        private int GameTimeToWaitBeforeComplications;
        private string WitnessModel;

        private PedExt Witness;
        private PedVariation WitnessVariation;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private object pedHeadshotHandle;
        private bool WitnessIsCustomer;
        private bool WillFlee;
        private bool WillFight;
        private ShopMenu WitnessShopMenu;
        private WeaponInformation WitnessWeapon;
        private CorruptCopContact Contact;

        private bool IsPlayerFarFromWitness => Witness != null && Witness.Pedestrian.Exists() && Witness.Pedestrian.DistanceTo2D(Player.Character) >= 850f;
        private bool IsPlayerNearWitnessSpawn => SpawnPositionCellX != -1 && SpawnPositionCellY != -1 && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, SpawnPositionCellX, SpawnPositionCellY, 6);
        public WitnessEliminationTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, 
            ICrimes crimes, INameProvideable names,IWeapons weapons, IShopMenus shopMenus, CorruptCopContact corruptCopContact)
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
            Weapons = weapons;
            ShopMenus = shopMenus;
            Contact = corruptCopContact;
        }
        public void Setup()
        {
            
        }
        public void Dispose()
        {
            if(Witness != null && Witness.Pedestrian.Exists())
            {
                Witness.DeleteBlip();
                Witness.Pedestrian.IsPersistent = false;
                Witness.Pedestrian.Delete();
            }
            if (WitnessLocation != null)
            {
                WitnessLocation.IsPlayerInterestedInLocation = false;
            }
        }
        public void Start(CorruptCopContact contact)
        {
            Contact = contact;
            if (Contact == null)
            {
                return;
            }
            if (PlayerTasks.CanStartNewTask(Contact.Name))
            {
                GetPedInformation();
                if (HasSpawnPosition)
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
        private void GetPedInformation()
        {
            WitnessIsMale = RandomItems.RandomPercent(60);
            WitnessName = Names.GetRandomName(WitnessIsMale);
            WitnessIsAtHome = RandomItems.RandomPercent(30);
            if (WitnessIsAtHome)
            {
                WitnessLocation = PlacesOfInterest.PossibleLocations.Residences.Where(x => !x.IsOwnedOrRented && x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            }
            else
            {
                WitnessLocation = PlacesOfInterest.PossibleLocations.WitnessTaskLocations().Where(x => x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            }
            
            WitnessVariation = null;
            if (WitnessIsMale)
            {
                WitnessModel = MaleWitnessPossibleModels.Where(x => Player.ModelName.ToLower() != x.ToLower()).PickRandom();
            }
            else
            {
                WitnessModel = FemaleWitnessPossibleModels.Where(x => Player.ModelName.ToLower() != x.ToLower()).PickRandom();
            }
            if (WitnessLocation != null)
            {
                WitnessSpawnPosition = WitnessLocation.EntrancePosition;
                WitnessSpawnHeading = WitnessLocation.EntranceHeading;
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
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {Contact.Name}");
                    break;
                }
                if(!IsWitnessSpawned && IsPlayerNearWitnessSpawn)
                {
                    IsWitnessSpawned = SpawnWitness();
                }
                if(IsWitnessSpawned && IsPlayerFarFromWitness)
                {
                    DespawnWitness();
                    if(Witness.HasSeenPlayerCommitCrime)
                    {
                        //EntryPoint.WriteToConsoleTestLong("Witness Elimination WITNESS FLED");
                        Game.DisplayHelp($"{Contact.Name} The witness fled");
                        break;
                    }
                }
                else if(IsWitnessSpawned && Witness != null && Witness.Pedestrian.Exists() && Witness.Pedestrian.IsDead)
                {
                    Witness.Pedestrian.IsPersistent = false;
                    Witness.DeleteBlip();
                    //EntryPoint.WriteToConsoleTestLong("Witness Elimination WITNESS WAS KILLED");
                    CurrentTask.OnReadyForPayment(true);
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
            if (WitnessLocation != null)
            {
                WitnessLocation.IsPlayerInterestedInLocation = false;
            }
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                //GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 10000));
                
                StartDeadDropPayment();//sets u teh whole dead drop thingamajic
            }
            else if (CurrentTask != null && CurrentTask.IsActive)
            {
                //GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 10000));
                SetFailed();
            }
            else
            {
                Dispose();
            }
        }
        private void SetCompleted()
        {
            //EntryPoint.WriteToConsoleTestLong("Witness Elimination COMPLETED");
            
            PlayerTasks.CompleteTask(Contact, true);

            SendCompletedMessage();
        }
        private void SetFailed()
        {
            //EntryPoint.WriteToConsoleTestLong("Witness Elimination FAILED");
            SendFailMessage();
            PlayerTasks.FailTask(Contact);
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
                        //EntryPoint.WriteToConsoleTestLong($"Picked up money for Witness Elimination for {Contact.Name}");
                        Game.DisplayHelp($"{Contact.Name} Money Picked Up");
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
            //EntryPoint.WriteToConsoleTestLong($"You are hired to kill a witness!");
            PlayerTasks.AddTask(Contact, 0, 2000, 0, -500, 7,"Witness Elimination");
            CurrentTask = PlayerTasks.GetTask(Contact.Name);
            IsWitnessSpawned = false;
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationComplicationsPercentage);
            WillFlee = false;
            WillFight = false;
            if(WillAddComplications)
            {
                if(RandomItems.RandomPercent(50))
                {
                    WillFlee = true;
                }
                else
                {
                    WillFight = true;
                }
            }
            WitnessWeapon = null;
            if (RandomItems.RandomPercent(40))
            {
                Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
            else
            {
                if (RandomItems.RandomPercent(50))
                {
                    Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
                }
                else
                {
                    if (RandomItems.RandomPercent(50))
                    {
                        Weapons.GetRandomRegularWeapon(WeaponCategory.AR);
                    }
                    else
                    {
                        Weapons.GetRandomRegularWeapon(WeaponCategory.Shotgun);
                    }
                }
            }
            WitnessShopMenu = null;
            WitnessIsCustomer = RandomItems.RandomPercent(30f);
            if (WitnessIsCustomer)
            {
                WitnessShopMenu = ShopMenus.GetRandomDrugCustomerMenu();
            }

            if(WitnessLocation != null)
            {
                WitnessLocation.IsPlayerInterestedInLocation = true;
            }
        }
        private bool SpawnWitness()
        {
            if (WitnessSpawnPosition != Vector3.Zero)
            {
                World.Pedestrians.CleanupAmbient();
                Ped ped = new Ped(WitnessModel, WitnessSpawnPosition, WitnessSpawnHeading);
                GameFiber.Yield();
                NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(WitnessModel));
                if (ped.Exists())
                {
                    string GroupName = "Man";
                    if(!WitnessIsMale)
                    {
                        GroupName = "Woman";
                    }  
                    Witness = new PedExt(ped, Settings, Crimes, Weapons, WitnessName, GroupName, World);
                    if (Settings.SettingsManager.TaskSettings.ShowEntityBlips)
                    {
                        Witness.AddBlip();
                    }
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
                    pedHeadshotHandle = NativeFunction.Natives.REGISTER_PEDHEADSHOT<int>(ped);
                    if (WitnessIsCustomer)
                    {
                        Witness.SetupTransactionItems(WitnessShopMenu, false);
                    }
                    if(WillAddComplications)
                    {
                        ped.RelationshipGroup = RelationshipGroup.HatesPlayer;
                        if (WillFlee)//flee
                        {
                            Witness.WillCallPolice = true;
                            Witness.WillCallPoliceIntense = true;
                            Witness.WillFight = false;
                            Witness.WillFightPolice = false;
                            Witness.WillAlwaysFightPolice = false;
                            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(ped, (int)eCombatAttributes.BF_AlwaysFlee, true);
                            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(ped, 2, true);
                            //EntryPoint.WriteToConsoleTestLong("WITNESS ELIMINATION, THE WITNESS WITH FLEE FROM YOU");
                        }
                        else if (WillFight)
                        {
                            Witness.WillFight = true;
                            Witness.WillCallPolice = false;
                            Witness.WillCallPoliceIntense = false;
                            Witness.WillFightPolice = true;
                            Witness.WillAlwaysFightPolice = true;
                            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(ped, (int)eCombatAttributes.BF_AlwaysFight, true);
                            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(ped, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(ped, 0, false);
                            
                            if (WitnessWeapon != null)
                            {
                                NativeFunction.Natives.GIVE_WEAPON_TO_PED(ped, (uint)WitnessWeapon.Hash, WitnessWeapon.AmmoAmount, false, false);
                            }
                            //EntryPoint.WriteToConsoleTestLong("WITNESS ELIMINATION, THE WITNESS WITH FIGHT YOU");
                        }
                        //they either know and flee, or know and fight     
                    }
                    GameFiber.Sleep(1000);
                    SendWitnessSpawnedMessage();
                    return true;
                }
            }
            return false;
        }
        private void SendWitnessSpawnedMessage()
        {
            List<string> Replies;
            string LookingForItem = "";
            if (WitnessIsCustomer)
            {
                MenuItem myMenuItem = Witness.ShopMenu?.Items.Where(x => x.NumberOfItemsToPurchaseFromPlayer > 0 && x.IsIllicilt).PickRandom();
                if(myMenuItem != null)
                {
                    LookingForItem = myMenuItem.ModItemName;
                }
            }
            if (NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
            {
                Replies = new List<string>() {
                    $"Picture of ~y~{WitnessName}~s~ attached. I heard they were still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"Sent you a picture of ~y~{WitnessName}~s~. They should still be around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"~y~{WitnessName}~s~. They are still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"The name is ~y~{WitnessName}~s~, pic attached. They are loitering around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"Remember, ~y~{WitnessName}~s~ is the name. I also sent a picture. I got word they are still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                     };
                string PickedReply = Replies.PickRandom();
                if(WitnessIsCustomer && LookingForItem != "")
                {
                    List<string> ItemReplies = new List<string>() {
                    $" They are probably looking for ~p~{LookingForItem}~s~.",
                    $" They like ~p~{LookingForItem}~s~.",
                    $" Will be looking to buy ~p~{LookingForItem}~s~.",
                    $" They are interested in ~p~{LookingForItem}~s~.",
                    $" The target likes ~p~{LookingForItem}~s~.",
                     };
                    PickedReply += ItemReplies.PickRandom();
                }

                if(WillFight || WillFlee)
                {
                    PickedReply += " ~s~The target might have gotten wind, be careful.";
                }
                string str = NativeFunction.Natives.GET_PEDHEADSHOT_TXD_STRING<string>(pedHeadshotHandle);
                EntryPoint.WriteToConsole($"WITNESS ELIM SENT PICTURE MESSAGE {str}");
                Player.CellPhone.AddCustomScheduledText(Contact, PickedReply, Time.CurrentDateTime, str, true);
            }
            else
            {
                Replies = new List<string>() {
                    $"~y~{WitnessName}~s~. I heard they were still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"~y~{WitnessName}~s~. They should still be around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"~y~{WitnessName}~s~. They are still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"The name is ~y~{WitnessName}~s~. They are loitering around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                    $"Remember, ~y~{WitnessName}~s~ is the name. I got word they are still around ~p~{WitnessLocation.Name} {WitnessLocation.FullStreetAddress}~s~.",
                     };
                string PickedReply = Replies.PickRandom();
                if (WitnessIsCustomer && LookingForItem != "")
                {
                    List<string> ItemReplies = new List<string>() {
                    $" They are probably looking for ~p~{LookingForItem}~s~.",
                    $" They like ~p~{LookingForItem}~s~.",
                    $" Will be looking to buy ~p~{LookingForItem}~s~.",
                    $" They are interested in ~p~{LookingForItem}~s~.",
                    $" The target likes ~p~{LookingForItem}~s~.",
                     };
                    PickedReply += ItemReplies.PickRandom();
                }

                if (WillFight || WillFlee)
                {
                    PickedReply += " ~s~The target might have gotten wind, be careful.";
                }
                EntryPoint.WriteToConsole("WITNESS ELIM SENT REGULAR MESSAGE");
                Player.CellPhone.AddCustomScheduledText(Contact, PickedReply, Time.CurrentDateTime,null, true);
            }
        }
        private void DespawnWitness()
        {
            if (Witness != null && Witness.Pedestrian.Exists())
            {
                Witness.DeleteBlip();
                Witness.Pedestrian.Delete();
                //EntryPoint.WriteToConsoleTestLong("Witness Elimination DESPAWNED WITNESS");
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
            Player.CellPhone.AddPhoneResponse(Contact.Name, Replies.PickRandom());
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies;
            if (WitnessIsAtHome)
            {
                Replies = new List<string>() {
                    $"Got a witness that needs to disappear. Home address is ~p~{WitnessLocation.FullStreetAddress}~s~. Name ~y~{WitnessName}~s~. ${MoneyToRecieve}",
                    $"Get to the house at ~p~{WitnessLocation.FullStreetAddress}~s~ and get rid of ~y~{WitnessName}~s~. ${MoneyToRecieve} on complation",
                    $"We need to you shut this guy up before he squeals. He lives at ~p~{WitnessLocation.FullStreetAddress}~s~. The name is ~y~{WitnessName}~s~. Payment of ${MoneyToRecieve}",
                    $"~y~{WitnessName}~s~ is living at ~p~{WitnessLocation.FullStreetAddress}~s~. They should be home. You know what to do. ${MoneyToRecieve}",
                    $"Need you to make sure ~y~{WitnessName}~s~ doesn't make it to the deposition, they live at ~p~{WitnessLocation.FullStreetAddress}~s~. ${MoneyToRecieve}",
                     };
            }
            else
            {
                Replies = new List<string>() {
                    $"Got a witness that needs to disappear. They hang around ~p~{WitnessLocation.Name}~s~. Address is ~p~{WitnessLocation.FullStreetAddress}~s~. Name ~y~{WitnessName}~s~. ${MoneyToRecieve}",
                    $"Get to ~p~{WitnessLocation.Name}~s~ on ~p~{WitnessLocation.FullStreetAddress}~s~ and get rid of ~y~{WitnessName}~s~. ${MoneyToRecieve} on complation",
                    $"We need to you shut this guy up before he squeals. He's at ~p~{WitnessLocation.Name}~s~ ~p~{WitnessLocation.FullStreetAddress}~s~. The name is ~y~{WitnessName}~s~. Payment of ${MoneyToRecieve}",
                    $"~y~{WitnessName}~s~ is at ~p~{WitnessLocation.Name}~s~, address is ~p~{WitnessLocation.FullStreetAddress}~s~. You know what to do. ${MoneyToRecieve}",
                    $"Need you to make sure ~y~{WitnessName}~s~ doesn't make it to the deposition, they are currently at ~p~{WitnessLocation.Name}~s~ on ~p~{WitnessLocation.FullStreetAddress}~s~. ${MoneyToRecieve}",
                     };
            }
            
            Player.CellPhone.AddPhoneResponse(Contact.Name, Replies.PickRandom());
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
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
        }
        private void SendDeadDropStartMessage()
        {
            List<string> Replies = new List<string>() {
                            $"Pickup your payment of ${MoneyToRecieve} from {myDrop.FullStreetAddress}, its {myDrop.Description}.",
                            $"Go get your payment of ${MoneyToRecieve} from {myDrop.Description}, address is {myDrop.FullStreetAddress}.",
                            };

            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
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
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
        }
        private void SendFailMessage()
        {
            List<string> Replies = new List<string>() {
                        $"You spooked them, they are gone. Thank for nothing.",
                        $"How could you let them get away?",
                        $"They weren't supposed to show up in court. Useless.",
                        $"How did you fuck this up so bad, they are squealing everything",
                        $"Since you fucked that up, they went right to the cops.",
                        };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
        }
    }

}
