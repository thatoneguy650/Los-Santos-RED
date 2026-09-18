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
        private IJurisdictions Jurisdictions;
        private ISettingsProvideable Settings;
        private ICrimes Crimes;
        private IWeapons Weapons;
        private ITimeControllable Time;
        private IInteriors Interiors;
        private IShopMenus ShopMenus;
        private IGangs Gangs;
        private IStreets Streets;
        private IPlacesOfInterest PlacesOfInterest;
        private List<Blip> CreatedBlips = new List<Blip>();
        private Blip TotalWantedBlip;
        private float CurrentSpawnMultiplier;
        private bool isSettingDensity;
        private bool isTrafficDisabled;

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
        public IZones Zones { get; set; }
        public IGangTerritories GangTerritories { get; set; }
        public int CitizenWantedLevel { get; set; }
        public int TotalWantedLevel { get; set; } = 0;
        public Vector3 PoliceBackupPoint { get; set; }
        public bool AnyFiresNearPlayer { get; private set; }
        public List<SpawnError> SpawnErrors { get; private set; }
        public ModDataFileManager ModDataFileManager { get; private set; }
        public ILocationInteractable LocationInteractable { get; private set; }
        public bool IsFEJInstalled { get; private set; }
        public bool IsFMTInstalled { get; private set; }
        public bool IsFEWInstalled { get; private set; }
        public bool IsFMLPInstalled { get; private set; }

        public bool IsFERSInstalled { get; private set; }
        public bool IsEUPInstalled { get; private set; }
        public bool IsEUPSUPInstalled { get; private set; }
        public string DebugString => "";


        public bool IsTrafficDisabled => isTrafficDisabled;
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
            EntryPoint.WriteToConsole($"FEJ Installed: {IsFEJInstalled}", 0);

            IsFMTInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfmt"));
            EntryPoint.WriteToConsole($"FMT Installed: {IsFMTInstalled}", 0);

            IsFEWInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfew"));
            EntryPoint.WriteToConsole($"FEW Installed: {IsFEWInstalled}", 0);

            IsFMLPInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfmlp"));
            EntryPoint.WriteToConsole($"FMLP Installed: {IsFMLPInstalled}", 0);

            IsFERSInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("greskfers"));
            EntryPoint.WriteToConsole($"FERS Installed: {IsFERSInstalled}", 0);

            IsEUPInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("eup"));
            EntryPoint.WriteToConsole($"EUP Installed: {IsEUPInstalled}", 0);
            IsEUPSUPInstalled = NativeFunction.Natives.IS_DLC_PRESENT<bool>(Game.GetHashKey("sup"));
            EntryPoint.WriteToConsole($"EUP:S&P Installed: {IsEUPSUPInstalled}", 0);
            //if (Settings.SettingsManager.WorldSettings.SetMissionFlagOn)
            //{
            //    NativeFunction.Natives.SET_MINIGAME_IN_PROGRESS(true);
            //}
        }
        private void SetMemoryItems()
        {
            if (Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles)
            {
                if (EntryPoint.IsEnhancedVersion)
                {
                    NativeMemoryEnhanced.SetMPGlobals();
                }
                else
                {
                    NativeMemory.SetMPGlobals();
                }
            }
        }
        public void Update()
        {

            SetDensity();

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
            if (TotalWantedLevel != totalWantedLevel)
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
                LoadMansionIPLs();
                LoadWorldIPLs();
                Game.FadeScreenIn(1500, true);
                IsMPMapLoaded = true;
            }
        }
        public void LoadSPMap()
        {
            if (IsMPMapLoaded)
            {
                Game.FadeScreenOut(1500, true);
                UnloadMansionIPLs();
                UnloadWorldIPLs();
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
            if (Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels)
            {
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
                else if (TotalWantedLevel == 4)
                {
                    CurrentSpawnMultiplier = Settings.SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels_Wanted4Multiplier;
                }
            }
            if (isTrafficDisabled)
            {
                CurrentSpawnMultiplier = 0.0f;
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
                            if (CurrentSpawnMultiplier == 0.0f)
                            {
                                NativeFunction.Natives.SET_AMBIENT_VEHICLE_RANGE_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            }
                            NativeFunction.Natives.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            NativeFunction.Natives.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
                            GameFiber.Yield();
                        }
                        isSettingDensity = false;
                        EntryPoint.WriteToConsole($"World - DONE Setting Population Density {CurrentSpawnMultiplier}");
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        //EntryPoint.ModController.CrashUnload();
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
            if (TotalWantedLevel == 0)
            {
                OnTotalWantedLevelRemoved();
            }
            else if (totalWantedLevel == 0)
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
        public void SetTrafficDisabled()
        {
            isTrafficDisabled = true;
            Vehicles.ClearPolice();
            Pedestrians.ClearPolice();
        }
        public void SetTrafficEnabled()
        {
            isTrafficDisabled = false;
        }
        private void LoadMansionIPLs()
        {
            foreach (string ipl in mansionMPIPLs)
            {
                NativeFunction.Natives.REQUEST_IPL(ipl);
            }
            foreach (string ipl in mansionSPIPLs)
            {
                NativeFunction.Natives.REMOVE_IPL(ipl);
            }
        }
        private void UnloadMansionIPLs()
        {
            foreach (string ipl in mansionMPIPLs)
            {
                NativeFunction.Natives.REMOVE_IPL(ipl);
            }
            foreach (string ipl in mansionSPIPLs)
            {
                NativeFunction.Natives.REQUEST_IPL(ipl);
            }
        }
        private List<string> mansionMPIPLs = new List<string>()
        {
        // VineWood Mansion
        "m25_2_ch2_04_mansion_interior_a",
        "apa_ch2_04_mansion_shared",
        "apa_ch2_04_mansion_private",
        "apa_ch2_04_mansion_railings_p",
        "apa_ch2_04_mansion_grass",
        "apa_ch2_04_mansion_shared_distantlights",
        "apa_ch2_04_mansion_shared_lodlights",
        // Richman Mansion
        "m25_2_ch1_06e_mansion_interior_a",
        "hei_ch1_06e_mansion_shared",
        "hei_ch1_06f_mansion_shared",
        "hei_ch1_06e_mansion_private",
        "hei_ch1_06e_mansion_railings_p",
        "hei_ch1_roads_mansion",
        "hei_ch1_06e_mansion_shared_distantlights",
        "hei_ch1_06e_mansion_shared_lodlights",
        // Tongva Mansion
        "m25_2_ch1_09_mansion_interior_a",
        "hei_ch1_09_mansion_shared",
        "hei_ch1_09_mansion_private",
        "hei_ch1_09_mansion_railings_p",
        "m25_2_mansion_props",
        "hei_ch1_09_mansion_shared_distantlights",
        "hei_ch1_09_mansion_shared_lodlights",
        };
        private List<string> mansionSPIPLs = new List<string>()
        {
        // VineWood Mansion
        "apa_ch2_04_mansion_original",
        "apa_ch2_04_props_original",
        // Richman Mansion
        "hei_ch1_06e_mansion_original",
        "hei_ch1_06f_mansion_original",
        "hei_ch1_06e_props_original",
        // Tongva Mansion
        "hei_ch1_roads_original",
        "hei_ch1_09_mansion_original",
        "hei_ch1_09_props_original"
        };


        private void LoadWorldIPLs()
        {
            foreach (string ipl in worldMPIPLs)
            {
                NativeFunction.Natives.REQUEST_IPL(ipl);
            }
        }
        private void UnloadWorldIPLs()
        {
            foreach (string ipl in worldMPIPLs)
            {
                NativeFunction.Natives.REMOVE_IPL(ipl);
            }
        }

        //Load Map Fixes and Building Exteriors.
        private List<string> worldMPIPLs = new List<string>()
        {
        // Base game
            // Vagos Garage Door fix - Put here to keep loaded since the door is loaded/unloaded when using the den.
                        "bkr_bi_id1_23_door",
                    // Grave Hole fix -282.4638f, 2835.845f, 55.91446f
                        "lr_cs6_08_grave_closed",

                       // "ch1_02_open", // Sniper Mission interior test
                // Cayo 
                    // base
                        //"h4_ch2_mansion_final",


                // Gunrunning DLC
                    // Yacht: -1363.724, 6734.108, 2.44598
                        //"gr_heist_yacht2",
                        //"gr_heist_yacht2_bar",
                        //"gr_heist_yacht2_bar_lod",
                        //"gr_heist_yacht2_bedrm",
                        //"gr_heist_yacht2_bedrm_lod",
                        //"gr_heist_yacht2_bridge",
                        //"gr_heist_yacht2_bridge_lod",
                        //"gr_heist_yacht2_enginrm",
                        //"gr_heist_yacht2_enginrm_lod",
                        //"gr_heist_yacht2_lod",
                        //"gr_heist_yacht2_lounge",
                        //"gr_heist_yacht2_lounge_lod",
                        //"gr_heist_yacht2_slod",

                // Hiests DLC
                // Heist Yacht: -2043.974,-1031.582, 11.981
                        //"hei_yacht_heist",
                        //"hei_yacht_heist_Bar",
                        //"hei_yacht_heist_Bedrm",
                        //"hei_yacht_heist_Bridge",
                        //"hei_yacht_heist_DistantLights",
                        //"hei_yacht_heist_enginrm",
                        //"hei_yacht_heist_LODLights",
                        //"hei_yacht_heist_Lounge",

                // Heist Carrier: 3082.3117 -4717.1191 15.2622
                        //"hei_carrier",
                        //"hei_carrier_distantlights",
                        //"hei_Carrier_int1",
                        //"hei_Carrier_int2",
                        //"hei_Carrier_int3",
                        //"hei_Carrier_int4",
                        //"hei_Carrier_int5",
                        //"hei_Carrier_int6",
                        //"hei_carrier_lodlights",
                        //"hei_carrier_slod",


                // Tuner DLC
                    // Los Santos Car Meet: -2000.0, 1113.211, -25.36243
                        "tr_tuner_meetup",
                        "tr_tuner_race_line",

                    // Tuner Shop Exteriors
                        "tr_tuner_shop_burton",
                        "tr_tuner_shop_mesa",
                        "tr_tuner_shop_mission",
                        "tr_tuner_shop_rancho",
                        "tr_tuner_shop_strawberry",

                // Drug Wars DLC
                    // base
                        "xm3_collision_fixes",
                        "xm3_sum2_fix",
                        "xm3_security_fix",

                    // Freakshop Exterior (warehouse)
                        "xm3_warehouse",
                        "xm3_warehouse_grnd",

                    // Eclipse Boulevard Garage: 519.2477, -2618.788, -50.000
                        "xm3_garage_fix",

                    //Train crash: 2630.595, 1458.144, 25.3669
                    //"xm3_train_crash",

                    // Bunker Exteriors
                        //"gr_case0_bunkerclosed", // Desert: 848.6175, 2996.567, 45.81612
                        //"gr_case1_bunkerclosed", // SmokeTree: 2126.785, 3335.04, 48.21422
                        //"gr_case2_bunkerclosed", // Scrapyard: 2493.654, 3140.399, 51.28789
                        //"gr_case3_bunkerclosed", // Oilfields: 481.0465, 2995.135, 43.96672
                        //"gr_case4_bunkerclosed", // RatonCanyon: -391.3216, 4363.728, 58.65862
                        //"gr_case5_bunkerclosed", // Grapeseed: 1823.961, 4708.14, 42.4991
                        //"gr_case6_bunkerclosed", // Farmhouse: 1570.372, 2254.549, 78.89397
                        //"gr_case7_bunkerclosed", // Paletto: -783.0755, 5934.686, 24.31475
                        //"gr_case9_bunkerclosed", // Route68: 24.43542, 2959.705, 58.35517
                        //"gr_case10_bunkerclosed", // Zancudo: -3058.714, 3329.19, 12.5844
                        //"gr_case11_bunkerclosed", // Great Ocean Highway: -3180.466, 1374.192, 19.9597

                // Mercenaries DLC
                    //base
                        "m23_1_legacy_fixes",
                        "m23_2_legacy_fixes",
                // Chop Shop DLC
                    // base
                        "m23_2_acp_collision_fixes_01",
                        "m23_2_acp_collision_fixes_02",
                        "m23_2_tug_collision",
                        "m23_2_hei_yacht_collision_fixes",
                        "m23_2_vinewood_garage",

                    // lifeguard Exterior
                        "m23_2_lifeguard_access",

                    // Salvage Yard Exteriors
                        "m23_2_sp1_03_reds",
                        "m23_2_sc1_03_reds",
                        "m23_2_id2_04_reds",
                        "m23_2_cs1_05_reds",
                        "m23_2_cs4_11_reds",

                // Bounties DLC
                    // base
                        "m24_1_legacyfixes",
                        "m24_1_pizzasigns",

                    // Bail Office Exteriors
                        "m24_1_bailoffice_davis",
                        "m24_1_bailoffice_delperro",
                        "m24_1_bailoffice_missionrow",
                        "m24_1_bailoffice_paletobay",
                        "m24_1_bailoffice_vinewood",

                    // Aircraft carrier: -3208.03, 3954.54, 14.0
                        //"m24_1_carrier",
                        //"m24_1_carrier_int1",
                        //"m24_1_carrier_int2",
                        //"m24_1_carrier_int3",
                        //"m24_1_carrier_int4",
                        //"m24_1_carrier_int5",
                        //"m24_1_carrier_int6",
                        //"m24_1_carrier_ladders",

                // Agents DLC
                    // base
                        "m24_2_legacy_fixes",
                        "m24_2_mp2024_02_additions",

                    // Garment Factory Exterior  752.31, -997.24, -47.0
                        "m24_2_garment_factory",

                    // Hangar door: -2632.43, 2963.23, 8.5
                        "m24_2_prop_m42_hangerdoor_02a",

                // Money Fronts DLC
                    // base
                        "m25_1_legacy_fixes",
                        "m25_2_legacy_fixes",
                        "m25_1_mp2025_01_additions",
                        "m25_1_bobcat",
                        "m25_1_garage",
                        "m25_1_quikpharma",

                    // Hand's On Car Wash Exterior
                        "m25_1_carwash",

                    // Offices - Smoke on Water and Higgins Heli Exteriors
                        "m25_1_helitours",
                        "m25_1_smokeonthewater",

                // Kortz Centre DLC
                    // base
                        "m26_1_mp2026_01_additions_critical_0",
                    // Museum Exterior
                        "m26_1_mp2026_01_additions_exterior",
                        "m26_1_mp2026_01_additions_exterior_cctv",

                        "m26_1_mp2026_01_additions_kortz_lowerbarriers_up",
                    //  "m26_1_mp2026_01_additions_kortz_lowerbarriers",

        };
    }
}