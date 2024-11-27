using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
//using LosSantosRED.lsr.Util.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;


namespace Mod
{
    public class World : IEntityLoggable, IEntityProvideable
    {
        private int totalWantedLevel;
        private IZones Zones;
        private IJurisdictions Jurisdictions;
        private ISettingsProvideable Settings;
        private ICrimes Crimes;
        private IWeapons Weapons;     
        private ITimeControllable Time;
        private IInteriors Interiors;
        private IShopMenus ShopMenus;
        private IGangTerritories GangTerritories;
        private IGangs Gangs;
        private IStreets Streets;
        private IPlacesOfInterest PlacesOfInterest;
        private List<Blip> CreatedBlips = new List<Blip>();
        private Blip TotalWantedBlip;
        private float CurrentSpawnMultiplier;
        private bool isSettingDensity;

        public World(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups,
            IWeapons weapons, ICrimes crimes, ITimeControllable time, IShopMenus shopMenus, IInteriors interiors, IAudioPlayable audio, IGangs gangs, IGangTerritories gangTerritories, IStreets streets, IModItems modItems, IPedGroups pedGroups, ILocationTypes locationTypes,
            IOrganizations associations, IContacts contacts, ModDataFileManager modDataFileManager)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            Jurisdictions = jurisdictions;
            Settings = settings;
            Weapons = weapons;
            Crimes = crimes;
            Time = time;
            Interiors = interiors;
            ShopMenus = shopMenus;
            Gangs = gangs;
            GangTerritories = gangTerritories;
            Streets = streets;
            ModDataFileManager = modDataFileManager;
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups, weapons, crimes, shopMenus, Gangs, GangTerritories, this);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes, modItems, this, associations);
            Places = new Places(this, zones, jurisdictions, settings, placesOfInterest, weapons, crimes, time, shopMenus, interiors, gangs, gangTerritories, streets, agencies, names, pedGroups, locationTypes, plateTypes, associations, contacts, ModDataFileManager.ModItems, modDataFileManager.IssueableWeapons, modDataFileManager.Heads, modDataFileManager.DispatchablePeople, modDataFileManager.ClothesNames);
            SpawnErrors = new List<SpawnError>();
        }
        public bool IsMPMapLoaded { get; private set; }
        public bool IsZombieApocalypse { get; set; } = false;
        public Vehicles Vehicles { get; private set; }
        public Pedestrians Pedestrians { get; private set; }
        public Places Places { get; private set; }
        public int CitizenWantedLevel { get; set; }
        public int TotalWantedLevel { get; set; } = 0;
        public Vector3 PoliceBackupPoint { get; set; }
        public bool AnyFiresNearPlayer { get; private set; }
        public List<SpawnError> SpawnErrors { get; private set; }
        public ModDataFileManager ModDataFileManager { get; private set; }
        public ILocationInteractable LocationInteractable { get; private set; }
        public bool IsFEJInstalled { get; private set; }
        public bool IsFMTInstalled { get; private set; }
        public string DebugString => "";
        public void Setup(IInteractionable player, ILocationInteractable locationInteractable)
        {
            DetermineMap();
            Pedestrians.Setup();
            LocationInteractable = locationInteractable;
            Places.Setup(player, locationInteractable);
            Vehicles.Setup();
            AddBlipsToMap();
            SetMemoryItems();
            CheckSpecialCircumstances();
        }

        private void CheckSpecialCircumstances()
        {
            IsFEJInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfej"));
            EntryPoint.WriteToConsole($"FEJ Installed: {IsFEJInstalled}",0);

            IsFMTInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfmt"));
            EntryPoint.WriteToConsole($"FMT Installed: {IsFMTInstalled}", 0);

            //if (Settings.SettingsManager.WorldSettings.SetMissionFlagOn)
            //{
            //    NativeFunction.Natives.SET_MINIGAME_IN_PROGRESS(true);
            //}
        }

        private void SetMemoryItems()
        {
            if (Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles)
            {
                NativeMemory.SetMPGlobals();
            }
        }

        public void Update()
        {
            if(Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels)
            {
                SetDensity();
            }
            if (Settings.SettingsManager.WorldSettings.AllowPoliceBackupBlip)
            {
                if (PoliceBackupPoint == Vector3.Zero)
                {
                    if (TotalWantedBlip.Exists())
                    {
                        TotalWantedBlip.Delete();
                    }
                }
                else
                {
                    if (!TotalWantedBlip.Exists())
                    {
                        CreateTotalWantedBlip();
                    }
                    else
                    {
                        TotalWantedBlip.Position = PoliceBackupPoint;
                    }
                }
            }
            else
            {
                if (TotalWantedBlip.Exists())
                {
                    TotalWantedBlip.Delete();
                }
            }
            if(TotalWantedLevel != totalWantedLevel)
            {
                OnTotalWantedLevelChanged();
            }
            if (Settings.SettingsManager.WorldSettings.AllowSettingDistantSirens)
            {
                NativeFunction.Natives.DISTANT_COP_CAR_SIRENS(false);
            }
            int numFires = NativeFunction.Natives.GET_NUMBER_OF_FIRES_IN_RANGE<int>(Game.LocalPlayer.Character.Position, 150f);
            AnyFiresNearPlayer = numFires > 0;
        }
        public void Dispose()
        {
            Places.Dispose();
            Pedestrians.Dispose();
            Vehicles.Dispose();
            RemoveBlips();
            if (Settings.SettingsManager.WorldSettings.SetMissionFlagOn)
            {
                NativeFunction.Natives.SET_MINIGAME_IN_PROGRESS(false);
            }
        }
        public void ClearSpawned(bool includeCivilians)
        {
            Pedestrians.ClearSpawned();
            Vehicles.ClearSpawned(includeCivilians);
        }
        public void LoadMPMap()
        {
            if (!IsMPMapLoaded)
            {
                Game.FadeScreenOut(1500, true);
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(1);
                NativeFunction.Natives.x0888C3502DBBEEF5();// ON_ENTER_MP();
                Game.FadeScreenIn(1500, true);
                IsMPMapLoaded = true;
            }
        }
        public void LoadSPMap()
        {
            if (IsMPMapLoaded)
            {
                Game.FadeScreenOut(1500, true);
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(0);
                NativeFunction.Natives.xD7C10C4A637992C9();// ON_ENTER_SP();
                Game.FadeScreenIn(1500, true);
                IsMPMapLoaded = false;
            }
        }
        public void AddBlip(Blip myBlip)
        {
            if (myBlip.Exists())
            {
                CreatedBlips.Add(myBlip);
            }
        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
        }
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                {
                    MyBlip.Delete();
                }
            }
            if (TotalWantedBlip.Exists())
            {
                TotalWantedBlip.Delete();
            }
        }
        public void SetDensity()
        {
            CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.DefaultSpawnMultiplier;// 1.0f;
            if (TotalWantedLevel >= 10)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted10Multiplier;
            }
            else if (TotalWantedLevel >= 9)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted9Multiplier;
            }
            else if (TotalWantedLevel >= 8)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted8Multiplier;
            }
            else if (TotalWantedLevel >= 7)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted7Multiplier;
            }
            else if (TotalWantedLevel >= 6)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted6Multiplier;
            }
            else if (TotalWantedLevel == 5)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted5Multiplier;
            }
            else if(TotalWantedLevel == 4)
            {
                CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted4Multiplier;
            }
            if (CurrentSpawnMultiplier != 1.0f && !isSettingDensity)
            {
                isSettingDensity = true;
                EntryPoint.WriteToConsole($"World - START Setting Population Density {CurrentSpawnMultiplier}");
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (CurrentSpawnMultiplier != 1.0f && EntryPoint.ModController?.IsRunning == true)
                        {
                            NativeFunction.Natives.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            GameFiber.Yield();
                        }
                        isSettingDensity = false;
                        //EntryPoint.WriteToConsoleTestLong($"World - DONE Setting Population Density {CurrentSpawnMultiplier}");
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, $"Density Runner");
            }
        }
        private void DetermineMap()
        {
            string iplName = "bkr_bi_hw1_13_int";
            NativeFunction.Natives.REQUEST_IPL(iplName);
            GameFiber.Sleep(100);
            IsMPMapLoaded = NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName);
            EntryPoint.WriteToConsole($"MP Map Loaded: {IsMPMapLoaded}");
        }
        private void CreateTotalWantedBlip()
        {
            TotalWantedBlip = new Blip(PoliceBackupPoint, 50f)
            {
                Name = "Police Requesting Assistance",
                Color = Color.Purple,
                Alpha = 0.25f
            };
            EntryPoint.WriteToConsole($"TOTAL WANTED BLIP CREATED");
            if (TotalWantedBlip.Exists())
            {
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Police Requesting Assistance");
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(TotalWantedBlip);
                NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE((uint)TotalWantedBlip.Handle, true);
            }
        }
        private void OnTotalWantedLevelChanged()
        {
            if(TotalWantedLevel == 0)
            {
                OnTotalWantedLevelRemoved();
            }
            else if(totalWantedLevel == 0)
            {
                OnTotalWantedLevelAdded();
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong($"OnTotalWantedLevelChanged {TotalWantedLevel}");
            }
            totalWantedLevel = TotalWantedLevel;
        }
        private void OnTotalWantedLevelRemoved()
        {
            if (Settings.SettingsManager.WorldSettings.AllowSettingDistantSirens)
            {
                NativeFunction.Natives.DISTANT_COP_CAR_SIRENS(false);
                //EntryPoint.WriteToConsoleTestLong($"OnTotalWantedLevelRemoved Distant Sirens Removed");
            }
        }
        private void OnTotalWantedLevelAdded()
        {
            //EntryPoint.WriteToConsoleTestLong($"OnTotalWantedLevelAdded {TotalWantedLevel}");
        }

        //public void StoreEntities()
        //{
        //    //EntryPoint.ModController.AllObjects = Rage.World.GetAllObjects().ToList();
        //    //GameFiber.Yield();
        //    //EntryPoint.ModController.AllPeds = Rage.World.GetAllPeds().ToList();
        //    //GameFiber.Yield();
        //   // EntryPoint.ModController.AllVehicles = Rage.World.GetAllVehicles().ToList();
        //}
    }
}