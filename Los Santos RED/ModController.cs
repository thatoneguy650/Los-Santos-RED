using LosSantosRED.lsr.Data;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace LosSantosRED.lsr
{
    //dont really like how this is written, need an entire refactor?
    public class ModController
    {
        private readonly Stopwatch TickStopWatch = new Stopwatch();
        private Agencies Agencies;
        private Civilians Civilians;
        private List<ModTask> CoreTasks;
        private Crimes Crimes;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private MovingAverage FPS;
        private GameSaves GameSaves;
        private Input Input;
        private Jurisdictions Jurisdictions;
        private string LastRanCoreTask;
        private string LastRanQuaternaryTask;
        private string LastRanQuinaryTask;
        private string LastRanSecondaryTask;
        private string LastRanTertiaryTask;
        private Names Names;
        private PedSwap PedSwap;
        private PlacesOfInterest PlacesOfInterest;
        private PlateTypes PlateTypes;
        private Mod.Player Player;
        private Police Police;
        private string PrevLastRanCoreTask;
        private string PrevLastRanQuaternaryTask;
        private string PrevLastRanSecondaryTask;
        private string PrevLastRanTertiaryTask;
        private List<ModTask> QuaternaryTasks;
        private List<ModTask> QuinaryTasks;
        private List<ModTask> NonPriorityTasks;
        private RadioStations RadioStations;
        private PedGroups RelationshipGroups;
        private Scenarios Scenarios;
        private List<ModTask> SecondaryTasks;
        private Settings Settings;
        private Streets Streets;
        private StreetScannerAudio StreetScannerAudio;
        private Tasker Tasker;
        private List<ModTask> TertiaryTasks;
        private Mod.Time Time;
        private MovingAverage TimeMA;
        private UI UI;
        private VanillaManager VanillaManager;
        private VehicleScannerAudio VehicleScannerAudio;
        private WavAudio WavAudio;
        private Weapons Weapons;
        private Mod.World World;
        private Zones Zones;
        private ZoneScannerAudio ZoneScannerAudio;
        private Interiors Interiors;
        private ModItems ModItems;
        private ShopMenus ShopMenus;
        private Intoxicants Intoxicants;
        private Weather Weather;
        private string PrevLastRanQuinaryTask;
        private string PrevLastRanNonPriorityTask;
        private string LastRanNonPriorityTask;
        private Gangs Gangs;
        private GangTerritories GangTerritories;

        public ModController()
        {
        }
        public bool DebugCoreRunning { get; set; } = true;
        public bool DebugInputRunning { get; set; } = true;
        public bool DebugSecondaryRunning { get; set; } = true;
        public bool DebugTertiaryRunning { get; set; } = true;
        public bool DebugQuaternaryRunning { get; set; } = true;
        public bool DebugQuinaryRunning { get; set; } = true;
        public bool DebugUIRunning { get; set; } = true;
        public bool DebugNonPriorityRunning { get; set; } = true;




        public bool IsRunning { get; private set; }


        public void Dispose()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            Dispatcher.Dispose();
            VanillaManager.Dispose();
            UI.Dispose();
            Time.Dispose();
            Weather.Dispose();
            Debug.Dispose();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~Deactivated");
        }
        public void NewPlayer(string modelName, bool isMale)//gotta go
        {
            Player.Reset(true, true, true, true, true, true, true);
            Player.SetDemographics(modelName, isMale, GetName(modelName, Names.GetRandomName(isMale)), RandomItems.MyRand.Next(Settings.SettingsManager.PedSwapSettings.RandomMoneyMin, Settings.SettingsManager.PedSwapSettings.RandomMoneyMax));
        }
        public void NewPlayer(string modelName, bool isMale,string playerName, int moneyToSpawnWith)//gotta go
        {
            Player.Reset(true, true, true, true, true, true, true);
            Player.SetDemographics(modelName, isMale, playerName, moneyToSpawnWith);
        }
        public void ReloadSettingsFromFile()
        {
            Settings.ReadConfig();
        }
        public void SaveSettingsToFile()
        {
            Settings.SerializeAllSettings();
        }
        public void Start()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            ReadDataFiles();
            WavAudio = new WavAudio();
            GameFiber.Yield();
            Time = new Mod.Time(Settings);
            GameFiber.Yield();
            World = new Mod.World(Agencies, Zones, Jurisdictions, Settings, PlacesOfInterest, PlateTypes, Names, RelationshipGroups, Weapons, Crimes, Time, ShopMenus, Interiors, WavAudio, Gangs, GangTerritories);
            World.Setup();
            GameFiber.Yield();
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, GetName(Game.LocalPlayer.Character.Model.Name, Names.GetRandomName(Game.LocalPlayer.Character.IsMale)), World, Time, Streets, Zones, Settings, Weapons, RadioStations, Scenarios, Crimes, WavAudio, PlacesOfInterest, Interiors, ModItems, Intoxicants, Gangs, Jurisdictions);
            Player.Setup();
            GameFiber.Yield();
            Police = new Police(World, Player, Player, Settings);
            GameFiber.Yield();
            Civilians = new Civilians(World, Player, Player, Settings);
            GameFiber.Yield();
            PedSwap = new PedSwap(Time, Player, Settings, World, Weapons, Crimes, Names);
            GameFiber.Yield();
            Tasker = new Tasker(World, Player, Weapons, Settings, PlacesOfInterest);
            Tasker.Setup();
            GameFiber.Yield();
            UI = new UI(Player, Settings, Jurisdictions, PedSwap, PlacesOfInterest, Player, Player, Player, Weapons, RadioStations, GameSaves, World, Player, Player, Tasker, Player, ModItems, Time, Player, Gangs);
            UI.Setup();
            GameFiber.Yield();
            Input = new Input(Player, Settings, UI, PedSwap);
            GameFiber.Yield();
            Dispatcher = new Dispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes, RelationshipGroups,Gangs,GangTerritories, ShopMenus);
            GameFiber.Yield();
            VanillaManager = new VanillaManager(Settings);
            GameFiber.Yield();
            Weather = new Weather(WavAudio, Settings, Time, Player);
            Weather.Setup();
            GameFiber.Yield();
            Debug = new Debug(PlateTypes, World, Player, Streets, Dispatcher,Zones,Crimes, this, Settings, Tasker, Time, Agencies, Weapons, ModItems, Weather, PlacesOfInterest, Interiors, Gangs);
            Debug.Setup();
            GameFiber.Yield();
            World.AddBlipsToMap();
            GameFiber.Yield();
            PedSwap.Setup();
            GameFiber.Yield();

            GameSave CurrentSave = GameSaves.GetSave(Player);
            if (CurrentSave != null)
            {
                CurrentSave.Load(Weapons, PedSwap, Player, Settings, World);
                GameFiber.Yield();
            }

            GameFiber.Yield();
            SetupModTasks();
            GameFiber.Yield();
            StartCoreLogic();
            GameFiber.Yield();
            StartSecondaryLogic();
            GameFiber.Yield();
            StartTertiaryLogic();
            GameFiber.Yield();
            StartQuaternaryLogic();
            GameFiber.Yield();
            StartQuinaryLogic();
            GameFiber.Yield();

            StartNonPriorityLogic();
            GameFiber.Yield();
            StartUILogic();
            GameFiber.Yield();
            StartInputLogic();
            GameFiber.Yield();
#if DEBUG
            StartDebugLogic();
            GameFiber.Yield();
#endif
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Has Loaded Successfully");
        }
        private string GetName(string modelBeforeSpoof, string defaultName)//gotta get outta here
        {
            if (modelBeforeSpoof.ToLower() == "player_zero")
            {
                return "Michael De Santa";
            }
            else if (modelBeforeSpoof.ToLower() == "player_one")
            {
                return "Franklin Clinton";
            }
            else if (modelBeforeSpoof.ToLower() == "player_two")
            {
                return "Trevor Philips";
            }
            else
            {
                return defaultName;
            }
        }
        private void ReadDataFiles()
        {
            Settings = new Settings();
            Settings.ReadConfig();
            GameFiber.Yield();


            ModItems = new ModItems();
            ModItems.ReadConfig();
            GameFiber.Yield();


            ShopMenus = new ShopMenus();
            ShopMenus.ReadConfig();
            GameFiber.Yield();



            Zones = new Zones();
            Zones.ReadConfig();
            GameFiber.Yield();
            PlateTypes = new PlateTypes();
            PlateTypes.ReadConfig();
            GameFiber.Yield();
            Streets = new Streets();
            Streets.ReadConfig();
            GameFiber.Yield();
            Weapons = new Weapons();
            Weapons.ReadConfig();
            GameFiber.Yield();
            Names = new Names();
            Names.ReadConfig();
            GameFiber.Yield();
            PlacesOfInterest = new PlacesOfInterest(ShopMenus);
            PlacesOfInterest.ReadConfig();
            GameFiber.Yield();
            VehicleScannerAudio = new VehicleScannerAudio();
            VehicleScannerAudio.ReadConfig();
            GameFiber.Yield();
            ZoneScannerAudio = new ZoneScannerAudio();
            ZoneScannerAudio.ReadConfig();
            GameFiber.Yield();
            StreetScannerAudio = new StreetScannerAudio();
            StreetScannerAudio.ReadConfig();
            GameFiber.Yield();
            Agencies = new Agencies();
            Agencies.ReadConfig();
            GameFiber.Yield();

            Gangs = new Gangs();
            Gangs.ReadConfig();
            GameFiber.Yield();


            Jurisdictions = new Jurisdictions(Agencies);
            Jurisdictions.ReadConfig();
            GameFiber.Yield();

            GangTerritories = new GangTerritories(Gangs);
            GangTerritories.ReadConfig();
            GameFiber.Yield();

            RadioStations = new RadioStations();
            RadioStations.ReadConfig();
            GameFiber.Yield();
            RelationshipGroups = new PedGroups();
            RelationshipGroups.ReadConfig();
            GameFiber.Yield();
            Scenarios = new Scenarios();
            GameFiber.Yield();
            Crimes = new Crimes();
            Crimes.ReadConfig();
            GameFiber.Yield();
            GameSaves = new GameSaves();
            GameSaves.ReadConfig();
            GameFiber.Yield();

            Interiors = new Interiors();
            Interiors.ReadConfig();

            Intoxicants = new Intoxicants();
            Intoxicants.ReadConfig();

            GameFiber.Yield();

        }
        private void SetupModTasks()
        {
            CoreTasks = new List<ModTask>()
            {
                  new ModTask(100, "Player.Update", Player.Update, 0),//1

                 new ModTask(250, "UI.Update", UI.Update, 1),//500

            };
            SecondaryTasks = new List<ModTask>()
            {
                new ModTask(250, "Player.Violations.Update", Player.ViolationsUpdate, 0),
                new ModTask(250, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 1),
                new ModTask(250, "Player.Investigation.Update", Player.Investigation.Update, 2),
                new ModTask(250, "Player.SearchModeUpdate", Player.SearchModeUpdate, 3),
                
                new ModTask(250, "Player.TrafficViolationsUpdate", Player.TrafficViolationsUpdate, 4),
                new ModTask(250, "Player.LocationUpdate", Player.LocationUpdate, 5),
                new ModTask(250, "Player.ArrestWarrantUpdate",Player.ArrestWarrantUpdate, 6),//these were all 500





              //  new ModTask(500, "Civilians.Update", Civilians.Update, 7),//250
               // new ModTask(500, "Police.Update", Police.Update,7),//added yields//cant get 300 ms updates in here anyways
               // new ModTask(500, "Player.Intoxication.Update", Player.IntoxicationUpdate, 10),//added yields//cant get 300 ms updates in here anyways






            };

            TertiaryTasks = new List<ModTask>()
            {
              
                new ModTask(1000, "World.PrunePedestrians", World.PrunePedestrians, 0),
                new ModTask(500, "World.CreateNewPedestrians", World.CreateNewPedestrians, 1), //very bad performance//500, need to up this somehow, we are stuck around 1250 to 1500, maybe just up the times?
                new ModTask(1000, "World.PruneVehicles", World.PruneVehicles, 2),//500
                new ModTask(1000, "World.CreateNewVehicles", World.CreateNewVehicles, 3), //very bad performance
                new ModTask(1000, "World.CleanUpVehicles", World.CleanUpVehicles, 4),
                new ModTask(1000, "World.UpdateVehiclePlates", World.UpdateVehiclePlates, 5),
                new ModTask(1500, "Player.ScannerUpdate", Player.ScannerUpdate, 6),//500
                new ModTask(2000, "VanillaManager.Tick", VanillaManager.Tick, 7),//1000

                //new ModTask(2000, "World.ActiveNearLocations", World.ActiveNearLocations, 8),//1000

                //new ModTask(4000, "Weather.Update", Weather.Update, 9),//1000

                //new ModTask(1500, "Dispatcher.Recall", Dispatcher.Recall, 8),
                //new ModTask(1500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 9),

            };

            QuaternaryTasks = new List<ModTask>()
            {

                new ModTask(1500, "Dispatcher.Recall", Dispatcher.Recall, 0),
                new ModTask(1500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 1),
                new ModTask(500, "Tasker.UpdatePolice", Tasker.UpdatePolice, 2),
                new ModTask(500, "Tasker.UpdateCivilians", Tasker.UpdateCivilians, 3),

                //new ModTask(500, "Police.Update", Police.Update, 6),
            };

            QuinaryTasks = new List<ModTask>()
            {
                new ModTask(250, "Police.Update", Police.Update,0),
                new ModTask(250, "Civilians.Update", Civilians.Update, 1),//500//250
               // new ModTask(500, "Police.Update", Police.Update, 1),//added yields//cant get 300 ms updates in here anyways
            };

            NonPriorityTasks = new List<ModTask>()
            {
                new ModTask(2000, "World.ActiveNearLocations", World.ActiveNearLocations, 0),//1000
                new ModTask(4000, "Weather.Update", Weather.Update, 1),//1000
                new ModTask(500, "World.UpdateNear", World.UpdateNearLocations, 2),//1000


                new ModTask(2000, "Player.GangRelationshipsUpdate", Player.GangRelationshipsUpdate, 3),//might become a priority...

                //new ModTask(500, "Police.Update", Police.Update, 1),//added yields//cant get 300 ms updates in here anyways
            };


            FPS = new MovingAverage();
            TimeMA = new MovingAverage();
        }
        private void StartCoreLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        TickStopWatch.Start();
                        if (DebugCoreRunning)
                        {
                            PrevLastRanCoreTask = LastRanCoreTask;
                            ModTask coreTask = CoreTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                            if (coreTask != null && DebugCoreRunning)
                            {
                                LastRanCoreTask = coreTask.DebugName + "-" + Player.UpdateState + $": TimeBetweenRuns: {Game.GameTime - coreTask.GameTimeLastRan}";
                                coreTask.Run();
                            }
                        }
                        FPS.ComputeAverage(Game.FrameRate);
                        TickStopWatch.Reset();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Core Logic");
            GameFiber.Yield();
        }
        private void StartDebugLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Debug.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
                    Dispose();
                }
            }, "Run Debug Logic");
        }
        private void StartInputLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if (DebugInputRunning)
                        {
                            Input.Update();
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Input Logic");
        }
        private void StartSecondaryLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    int CurrentSecondaryTask = 0;
                    while (IsRunning)
                    {
                        if (DebugSecondaryRunning)
                        {
                            if (CurrentSecondaryTask > SecondaryTasks.Count)
                            {
                                CurrentSecondaryTask = 0;
                            }
                            PrevLastRanSecondaryTask = LastRanSecondaryTask;
                            ModTask firstSecondaryTask = SecondaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentSecondaryTask).FirstOrDefault();
                            if (firstSecondaryTask != null)
                            {
                                LastRanSecondaryTask = firstSecondaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstSecondaryTask.GameTimeLastRan}";
                                firstSecondaryTask.Run();
                                CurrentSecondaryTask++;
                            }
                            else
                            {
                                ModTask alternateSecondaryTask = SecondaryTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if (alternateSecondaryTask != null)
                                {
                                    LastRanSecondaryTask = alternateSecondaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateSecondaryTask.GameTimeLastRan}";
                                    alternateSecondaryTask.Run();
                                }
                                else
                                {
                                    LastRanSecondaryTask = "NONE";//nothing to run at all this tick, everything is on time
                                }
                            }      
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Secondary Logic");
            GameFiber.Yield();
        }
        private void StartTertiaryLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    int CurrentTertiaryTask = 0;
                    while (IsRunning)
                    {
                        if (DebugTertiaryRunning)
                        {
                            if (CurrentTertiaryTask > TertiaryTasks.Count)
                            {
                                CurrentTertiaryTask = 0;
                            }
                            PrevLastRanTertiaryTask = LastRanTertiaryTask;
                            ModTask firstTertiary = TertiaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentTertiaryTask).FirstOrDefault();
                            if (firstTertiary != null)
                            {
                                LastRanTertiaryTask = firstTertiary.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstTertiary.GameTimeLastRan}";
                                firstTertiary.Run();
                                CurrentTertiaryTask++;
                            }
                            else
                            {
                                ModTask alternateTertiaryTask = TertiaryTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if (alternateTertiaryTask != null)
                                {
                                    LastRanTertiaryTask = alternateTertiaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateTertiaryTask.GameTimeLastRan}";
                                    alternateTertiaryTask.Run();
                                }
                                else
                                {
                                    LastRanTertiaryTask = "NONE";//nothing to run at all this tick, everything is on time
                                }
                            }
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Secondary Logic");
            GameFiber.Yield();
        }
        private void StartQuaternaryLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    int CurrentQuaternaryTask = 0;
                    while (IsRunning)
                    {
                        if (DebugQuaternaryRunning)
                        {
                            if (CurrentQuaternaryTask > QuaternaryTasks.Count)
                            {
                                CurrentQuaternaryTask = 0;
                            }
                            PrevLastRanQuaternaryTask = LastRanQuaternaryTask;
                            ModTask firstQuaternary = QuaternaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentQuaternaryTask).FirstOrDefault();
                            if (firstQuaternary != null)
                            {
                                LastRanQuaternaryTask = firstQuaternary.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstQuaternary.GameTimeLastRan}";
                                firstQuaternary.Run();
                                CurrentQuaternaryTask++;
                            }
                            else
                            {
                                ModTask alternateQuaternaryTask = QuaternaryTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if (alternateQuaternaryTask != null)
                                {
                                    LastRanQuaternaryTask = alternateQuaternaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateQuaternaryTask.GameTimeLastRan}";
                                    alternateQuaternaryTask.Run();
                                }
                                else
                                {
                                    LastRanQuaternaryTask = "NONE";//nothing to run at all this tick, everything is on time
                                }
                            }
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Secondary Logic");
            GameFiber.Yield();
        }
        private void StartQuinaryLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    int CurrentQuinaryTask = 0;
                    while (IsRunning)
                    {
                        if (DebugQuinaryRunning)
                        {
                            if (CurrentQuinaryTask > QuinaryTasks.Count)
                            {
                                CurrentQuinaryTask = 0;
                            }
                            PrevLastRanQuinaryTask = LastRanQuinaryTask;
                            ModTask firstquiaryTask = QuinaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentQuinaryTask).FirstOrDefault();
                            if (firstquiaryTask != null)
                            {
                                LastRanQuinaryTask = firstquiaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstquiaryTask.GameTimeLastRan}";
                                firstquiaryTask.Run();
                                CurrentQuinaryTask++;
                            }
                            else
                            {
                                ModTask alternateQuinaryTask = QuinaryTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if (alternateQuinaryTask != null)
                                {
                                    LastRanQuinaryTask = alternateQuinaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateQuinaryTask.GameTimeLastRan}";
                                    alternateQuinaryTask.Run();
                                }
                                else
                                {
                                    LastRanQuinaryTask = "NONE";//nothing to run at all this tick, everything is on time
                                }
                            }
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Quinary Logic");
            GameFiber.Yield();
        }
        private void StartNonPriorityLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    int CurrentNonPriorityTask = 0;
                    while (IsRunning)
                    {
                        if (DebugNonPriorityRunning)
                        {
                            if (CurrentNonPriorityTask > NonPriorityTasks.Count)
                            {
                                CurrentNonPriorityTask = 0;
                            }
                            PrevLastRanNonPriorityTask = LastRanQuinaryTask;
                            ModTask firstnonPriorityTask = NonPriorityTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentNonPriorityTask).FirstOrDefault();
                            if (firstnonPriorityTask != null)
                            {
                                LastRanNonPriorityTask = firstnonPriorityTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstnonPriorityTask.GameTimeLastRan}";
                                firstnonPriorityTask.Run();
                                CurrentNonPriorityTask++;
                            }
                            else
                            {
                                ModTask alternateNonPriorityTask = NonPriorityTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if (alternateNonPriorityTask != null)
                                {
                                    LastRanNonPriorityTask = alternateNonPriorityTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateNonPriorityTask.GameTimeLastRan}";
                                    alternateNonPriorityTask.Run();
                                }
                                else
                                {
                                    LastRanNonPriorityTask = "NONE";//nothing to run at all this tick, everything is on time
                                }
                            }
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Quinary Logic");
            GameFiber.Yield();
        }
        private void StartUILogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if(DebugUIRunning)
                        {
                            UI.Tick();
                        }
                        
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run UI Logic");

            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if (DebugUIRunning)
                        {
                            UI.Tick2();
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run UI Logic 2");

            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if (DebugUIRunning)
                        {
                            UI.Tick3();
                            GameFiber.Yield();
                            Time.Tick();//this was below before, but shouldnt be any different
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run UI Logic 3");
        }
        private class ModTask
        {
            public string DebugName;
            public uint GameTimeLastRan = 0;
            public uint Interval = 500;
            public uint IntervalMissLength;
            public bool RanThisTick = false;
            public int RunGroup;
            public int RunOrder;
            public Action TickToRun;
            public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup, int _RunOrder)
            {
                GameTimeLastRan = 0;
                Interval = _Interval;
                IntervalMissLength = Interval * 2;
                DebugName = _DebugName;
                TickToRun = _TickToRun;
                RunGroup = _RunGroup;
                RunOrder = _RunOrder;
            }
            public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunOrder)
            {
                GameTimeLastRan = 0;
                Interval = _Interval;
                IntervalMissLength = Interval * 2;
                DebugName = _DebugName;
                TickToRun = _TickToRun;
                RunOrder = _RunOrder;
            }
            public MovingAverage AverageTimeBetweenRuns { get; set; } = new MovingAverage();
            public bool MissedInterval => Interval != 0 && Game.GameTime - GameTimeLastRan >= IntervalMissLength;
            public bool RunningBehind => Interval != 0 && Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2);
            public bool ShouldRun => GameTimeLastRan == 0 || Game.GameTime - GameTimeLastRan > Interval;
            public void Run()
            {
                AverageTimeBetweenRuns.ComputeAverage(Game.GameTime - GameTimeLastRan);
                TickToRun();
                GameTimeLastRan = Game.GameTime;
                RanThisTick = true;
            }
        }
        private class MovingAverage
        {
            private float sampleAccumulator;
            private Queue<float> samples = new Queue<float>();
            private int windowSize = 16;
            public float Average { get; private set; }

            /// <summary>
            /// Computes a new windowed average each time a new sample arrives
            /// </summary>
            /// <param name="newSample"></param>
            public void ComputeAverage(float newSample)
            {
                sampleAccumulator += newSample;
                samples.Enqueue(newSample);

                if (samples.Count > windowSize)
                {
                    sampleAccumulator -= samples.Dequeue();
                }

                Average = sampleAccumulator / samples.Count;
            }
        }
    }
}