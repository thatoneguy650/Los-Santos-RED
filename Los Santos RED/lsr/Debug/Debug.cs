using Blackjack;
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using LSR.Vehicles;
using Microsoft.VisualBasic.Logging;
using NAudio.Gui;
using Rage;
using Rage.Native;
using Roulette;
//using RNUIExamples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;


//using System.Windows.Media;
//using System.Windows.Media;
//using System.Windows.Media;
using System.Xml.Linq;
//using System.Windows.Media;
//using System.Windows.Media;
//using System.Windows.Media;

public class Debug
{
    private Crimes Crimes;
    private int PlateIndex;
    private Vector3 StoredPosition;
    private PlateTypes PlateTypes;
    private Mod.World World;
    private Mod.Player Player;
    private Ped DebugPed;
    private IStreets Streets;
    private int VehicleMissionFlag = 1;
    private Dispatcher Dispatcher;
    private Zones Zones;
    private ModController ModController;
    private Settings Settings;
    private Ped RageTargetPed;
    private Mod.Tasker Tasker;
    private List<InteriorPosition> InteriorPositions = new List<InteriorPosition>();
    private List<InteriorPosition> MPInteriorPositions = new List<InteriorPosition>();
    private uint GameTimeLastScannedForStands;
    private Mod.Time Time;
    private Camera CharCam;
    private Camera InterpolationCamera;
    private Agencies Agencies;
    private Weapons Weapons;
    private Camera FreeCam;
    private float FreeCamScale = 1.0f;
    private List<string> Timecycles;
    private int TimecycleIndex = 0;
    private string TimecycleName = "";
    private IModItems ModItems;
    private bool boolFlipper;
    private WeatherReporting Weather;
    private PlacesOfInterest PlacesOfInterest;
    private Interiors Interiors;
    private Gangs Gangs;
    private bool Started1 = false;
    private Input Input;
    private ShopMenus ShopMenus;
    private int TextSoundID;
    private int HangUpSoundID;
    private Vector3 Offset;
    private Rotator Rotation;
    private bool isPrecise;
    private bool IsDisplaying;
    private bool isClearingPeds;
    private ModDataFileManager ModDataFileManager;
    private bool isRunning;
    private uint GameTimeLastAttached;
    private bool Test;
    private bool OnOff1;
    private bool IsOn = true;
    private bool isDoorLocked;
    private bool DoOne;
    private bool IsBigMapActive = false;
    private string CurrentDictionary;
    private string CurrentAnimation;
    private bool isShowingTunnel;
    private Rage.Object chairProp;
    private Cop LastPed;
    private Rage.Object PumpHandleProp;

    public Debug(PlateTypes plateTypes, Mod.World world, Mod.Player targetable, IStreets streets, Dispatcher dispatcher, Zones zones, Crimes crimes, ModController modController, Settings settings, Mod.Tasker tasker, Mod.Time time, Agencies agencies, Weapons weapons, ModItems modItems, WeatherReporting weather, PlacesOfInterest placesOfInterest, Interiors interiors, Gangs gangs, Input input, ShopMenus shopMenus, ModDataFileManager modDataFileManager)
    {
        PlateTypes = plateTypes;
        World = world;
        Player = targetable;
        Streets = streets;
        Dispatcher = dispatcher;
        Zones = zones;
        Crimes = crimes;
        ModController = modController;
        Settings = settings;
        Tasker = tasker;
        Time = time;
        Agencies = agencies;
        Weapons = weapons;
        ModItems = modItems;
        Weather = weather;
        PlacesOfInterest = placesOfInterest;
        Interiors = interiors;
        Gangs = gangs;
        Input = input;
        ShopMenus = shopMenus;

        ModDataFileManager = modDataFileManager;

    }
    public void Dispose()
    {
        if (DebugPed.Exists())
        {
            DebugPed.Delete();
        }
    }
    public void Update()
    {
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            DebugNumpad0();
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {
            DebugNumpad1();
        }
        if (Game.IsKeyDown(Keys.NumPad2))
        {
            DebugNumpad2();
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {
            DebugNumpad3();
        }
        if (Game.IsKeyDown(Keys.NumPad4))
        {
            DebugNumpad4();
        }
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            DebugNumpad5();
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            DebugNumpad6();
        }
        if (Game.IsKeyDown(Keys.NumPad7))
        {
            DebugNumpad7();
        }
        if (Game.IsKeyDown(Keys.NumPad8))
        {
            DebugNumpad8();
        }
        if (Game.IsKeyDown(Keys.NumPad9))
        {
            DebugNumpad9();
        }

        if (Settings.SettingsManager.DebugSettings.ShowPoliceTaskArrows)
        {
            foreach (Cop cop in World.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists()))
            {
                DrawColoredArrowTaskStatus(cop);
                DrawColoredArrowAlertness(cop);
            }
        }
        if (Settings.SettingsManager.DebugSettings.ShowCivilianTaskArrows)
        {
            foreach (PedExt ped in World.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 45f))
            {
                Color Color = Color.Yellow;
                if (!ped.CanBeTasked)
                {
                    Color = Color.Purple;
                }
                else if (ped.CurrentTask != null)
                {
                    Color = Color.Black;
                }
                else if (ped.HasSeenPlayerCommitCrime)
                {
                    Color = Color.Orange;
                }
                else if (ped.CanRecognizePlayer)
                {
                    Color = Color.Green;
                }
                else if (ped.CanSeePlayer)
                {
                    Color = Color.White;
                }
                else
                {
                    Color = Color.Red;
                }
                Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color);
            }
        }
        if (Settings.SettingsManager.DebugSettings.ShowCivilianPerceptionArrows)
        {
            EntryPoint.WriteToConsole("DRAW DEBUG ARROWS");
            List<PedExt> AllPeds = World.Pedestrians.CivilianList;
            AllPeds.AddRange(World.Pedestrians.GangMemberList);
            AllPeds.AddRange(World.Pedestrians.MerchantList);
            AllPeds.AddRange(World.Pedestrians.TellerList);
            foreach (PedExt ped in AllPeds.Where(x => x.Pedestrian.Exists()))/// && x.DistanceToPlayer <= 250f))// && NativeHelper.IsNearby(EntryPoint.FocusCellX,EntryPoint.FocusCellY,x.CellX,x.CellY,4)))// x.DistanceToPlayer <= 150f))
            {
                Color Color3 = Color.Yellow;
                if (ped.CanRecognizePlayer)
                {
                    Color3 = Color.Orange;
                }
                else if (ped.CanSeePlayer)
                {
                    Color3 = Color.Green;
                }
                else
                {
                    Color3 = Color.Black;
                }
                Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color3);
            }

            foreach (Cop cop in World.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists()))
            {
                Color Color4 = Color.Yellow;
                if (cop.CanRecognizePlayer)
                {
                    Color4 = Color.Orange;
                }
                else if (cop.CanSeePlayer)
                {
                    Color4 = Color.Green;
                }
                Rage.Debug.DrawArrowDebug(cop.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color4);
            }
        }


        if (Settings.SettingsManager.DebugSettings.ShowTrafficArrows)
        {
            foreach (PedExt ped in World.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists() && x.IsInVehicle))/// && x.DistanceToPlayer <= 250f))// && NativeHelper.IsNearby(EntryPoint.FocusCellX,EntryPoint.FocusCellY,x.CellX,x.CellY,4)))// x.DistanceToPlayer <= 150f))
            {
                Color Color3 = Color.Yellow;
                if (ped.IsWaitingAtTrafficLight)
                {
                    Color3 = Color.Purple;
                }
                Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color3);
            }

        }


        //foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
        //{
        //    Color Color3 = Color.Yellow;
        //    if (cop.HasSeenPlayerCommitCrime)
        //    {
        //        Color3 = Color.Red;
        //    }
        //    else if (cop.CanRecognizePlayer)
        //    {
        //        Color3 = Color.Orange;
        //    }
        //    else if (cop.CanSeePlayer)
        //    {
        //        Color3 = Color.Green;
        //    }
        //    Rage.Debug.DrawArrowDebug(cop.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color3);
        //}
        //foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
        //{
        //    Color Color;
        //    if (cop.CurrentTask == null)
        //    {
        //        Color = Color.Black;
        //    }
        //    else if (cop.CurrentTask?.Name == "Investigate")
        //    {
        //        Color = Color.White;
        //    }
        //    else if (cop.CurrentTask?.Name == "Idle")
        //    {
        //        Color = Color.Orange;
        //    }
        //    else if (cop.CurrentTask?.Name == "Locate")
        //    {
        //        Color = Color.Green;
        //    }
        //    else if (cop.CurrentTask?.Name == "Chase")
        //    {
        //        Color = Color.Blue;
        //    }
        //    else if (cop.CurrentTask?.Name == "Kill")
        //    {
        //        Color = Color.Red;
        //    }
        //    else if (cop.CurrentTask?.Name == "ApprehendOther")
        //    {
        //        Color = Color.Pink;
        //    }
        //    else
        //    {
        //        Color = Color.Yellow;
        //    }
        //    Rage.Debug.DrawArrowDebug(cop.Pedestrian.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color);
        //}

        //if (Player.CurrentLocation?.ClosestRoadNode != Vector3.Zero)
        //{
        //    if (Player.CurrentLocation.NodeString != "")
        //    {
        //        Game.DisplaySubtitle(Player.CurrentLocation.NodeString);
        //    }
        //    Rage.Debug.DrawArrowDebug(Player.CurrentLocation.ClosestRoadNode + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
        //}


        //Vector3 position = Game.LocalPlayer.Character.Position;
        //Vector3 outPos;
        //NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, 1, out outPos, 1, 0x40400000, 0);
        //if(outPos != Vector3.Zero)
        //{
        //    Rage.Debug.DrawArrowDebug(outPos + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Red);
        //}
        //int standsfound = 0;
        //if (Game.GameTime - GameTimeLastScannedForStands >= 1000)
        //{

        //    Rage.Object[] objects = Rage.World.GetAllObjects();
        //    foreach (Rage.Object obj in objects)
        //    {
        //        if (obj.Exists())
        //        {
        //            //if(obj.DistanceTo2D(Game.LocalPlayer.Character) <= 20f)
        //            //{
        //            //    EntryPoint.WriteToConsole($"Scanned for Stands MODEL {obj.Model.Name.ToLower()}", 5);

        //            //}
        //            if (obj.Model.Hash == Game.GetHashKey("prop_hotdogstand_01") || obj.Model.Hash == Game.GetHashKey("prop_burgerstand_01"))
        //            {
        //                //if(!obj.GetAttachedBlip().Exists())
        //                //{
        //                //    obj.AttachBlip();
        //                //}
        //                standsfound++;
        //            }
        //        }
        //    }
        //    if(standsfound > 0 )
        //    {
        //        Game.DisplayNotification($"Stand Found {standsfound}");
        //    }
        //    EntryPoint.WriteToConsole($"Scanned for Stands Found {standsfound}", 5);
        //    GameTimeLastScannedForStands = Game.GameTime;
        //}
        //bool runningRed = NativeFunction.Natives.GET_IS_PLAYER_DRIVING_WRECKLESS<bool>(Game.LocalPlayer, 1);
        //Game.DisplaySubtitle($"Running Red: {runningRed}");

    }
    public void Setup()
    {
        InteriorPositions = new List<InteriorPosition>()
        {
            new InteriorPosition("10 Car",new Vector3(229.9559f, -981.7928f, -99.66071f))//works
            ,new InteriorPosition("Low End Apartment",new Vector3(261.4586f, -998.8196f, -99.00863f))//works
            ,new InteriorPosition("4 Integrity Way, Apt 30",new Vector3(-35.31277f, -580.4199f, 88.71221f))//works
            ,new InteriorPosition("Del Perro Heights, Apt 4",new Vector3(-1468.14f, -541.815f, 73.4442f))//works
            ,new InteriorPosition("Del Perro Heights, Apt 7",new Vector3(-1477.14f, -538.7499f, 55.5264f))//works
            ,new InteriorPosition("Eclipse Towers, Apt 3",new Vector3(-773.407f, 341.766f, 211.397f))//works
            ,new InteriorPosition("CharCreator",new Vector3(402.5164f, -1002.847f, -99.2587f))//works
            ,new InteriorPosition("Mission Carpark",new Vector3(405.9228f, -954.1149f, -99.6627f))//works
            ,new InteriorPosition("Torture Room",new Vector3(136.5146f, -2203.149f, 7.30914f))//works
            ,new InteriorPosition("Omega's Garage",new Vector3(2331.344f, 2574.073f, 46.68137f))//works
            ,new InteriorPosition("Motel",new Vector3(152.2605f, -1004.471f, -98.99999f))//works
            ,new InteriorPosition("Lester's House",new Vector3(1273.9f, -1719.305f, 54.77141f))//works
            ,new InteriorPosition("FBI Top Floor",new Vector3(134.5835f, -749.339f, 258.152f))//works
            ,new InteriorPosition("FBI Floor 47",new Vector3(134.5835f, -766.486f, 234.152f))//works
            ,new InteriorPosition("FBI Floor 49",new Vector3(134.635f, -765.831f, 242.152f))//works
            ,new InteriorPosition("IAA Office",new Vector3(117.22f, -620.938f, 206.1398f))//works
   
        };
        MPInteriorPositions = new List<InteriorPosition>()
        {
            new InteriorPosition("2 Car",new Vector3(173.2903f, -1003.6f, -99.65707f))//doesnt work
            ,new InteriorPosition("6 Car",new Vector3(197.8153f, -1002.293f, -99.65749f))//doesnt work
            ,new InteriorPosition("Medium End Apartment",new Vector3(347.2686f, -999.2955f, -99.19622f))//doesnt work
            ,new InteriorPosition("4 Integrity Way, Apt 28",new Vector3(-18.07856f, -583.6725f, 79.46569f))//doesnt work
            ,new InteriorPosition("Richard Majestic, Apt 2",new Vector3(-915.811f, -379.432f, 113.6748f))//doesnt work
            ,new InteriorPosition("Tinsel Towers, Apt 42",new Vector3(-614.86f, 40.6783f, 97.60007f))//doesnt work
            ,new InteriorPosition("3655 Wild Oats Drive",new Vector3(-169.286f, 486.4938f, 137.4436f))//doesnt work
            ,new InteriorPosition("2044 North Conker Avenue",new Vector3(340.9412f, 437.1798f, 149.3925f))//doesnt work
            ,new InteriorPosition("2045 North Conker Avenue",new Vector3(373.023f, 416.105f, 145.7006f))//doesnt work
            ,new InteriorPosition("2862 Hillcrest Avenue",new Vector3(-676.127f, 588.612f, 145.1698f))//doesnt work
            ,new InteriorPosition("2868 Hillcrest Avenue",new Vector3(-763.107f, 615.906f, 144.1401f))//doesnt work
            ,new InteriorPosition("2874 Hillcrest Avenue",new Vector3(-857.798f, 682.563f, 152.6529f))//doesnt work
            ,new InteriorPosition("2677 Whispymound Drive",new Vector3(120.5f, 549.952f, 184.097f))//doesnt work
            ,new InteriorPosition("2133 Mad Wayne Thunder",new Vector3(-1288f, 440.748f, 97.69459f))//doesnt work
            ,new InteriorPosition("Bunker Interior",new Vector3(899.5518f,-3246.038f, -98.04907f))//doesnt work
            ,new InteriorPosition("Solomon's Office",new Vector3(-1005.84f, -478.92f, 50.02733f))//doesnt work
            ,new InteriorPosition("Psychiatrist's Office",new Vector3(-1908.024f, -573.4244f, 19.09722f))//doesnt work
            ,new InteriorPosition("Movie Theatre",new Vector3(-1427.299f, -245.1012f, 16.8039f))//doesnt work
            ,new InteriorPosition("Madrazos Ranch",new Vector3(1399f, 1150f, 115f))//doesnt work
            //,new InteriorPosition("Life Invader Office",new Vector3(-1044.193f, -236.9535f, 37.96496f))//doesnt work//wrong coords? doesnt even work in MP
            ,new InteriorPosition("Smuggler's Run Hangar",new Vector3(-1266.802f, -3014.837f, -49.000f))//doesnt work
            ,new InteriorPosition("Avenger Interior",new Vector3(520.0f, 4750.0f, -70.0f))//doesnt work
            ,new InteriorPosition("Facility",new Vector3(345.0041f, 4842.001f, -59.9997f))//doesnt work
            ,new InteriorPosition("Server Farm",new Vector3(2168.0f, 2920.0f, -84.0f))//doesnt work
            ,new InteriorPosition("Submarine",new Vector3(514.33f, 4886.18f, -62.59f))//doesnt work
            ,new InteriorPosition("IAA Facility",new Vector3(2147.91f, 2921.0f, -61.9f))//doesnt work
            ,new InteriorPosition("Nightclub",new Vector3(-1604.664f, -3012.583f, -78.000f))//doesnt work
            ,new InteriorPosition("Nightclub Warehouse",new Vector3(-1505.783f, -3012.587f, -80.000f))//doesnt work
            ,new InteriorPosition("Terrorbyte Interior",new Vector3(-1421.015f, -3012.587f, -80.000f))//doesnt work
        };
        SetupTimecycles();

    }


    private void OneLiners()
    {
        // if (Game.LocalPlayer.Character.CurrentVehicle.Exists()) { NativeFunction.Natives.SET_VEHICLE_LIVERY(Game.LocalPlayer.Character.CurrentVehicle, 17);  }//Set Livery
    }

    private void DebugNumpad0()
    {
        Game.LocalPlayer.IsInvincible = true;
        Game.DisplayNotification("IsInvincible = True");
    }
    private void DebugNumpad1()
    {
        //ModController.DebugCoreRunning = !ModController.DebugCoreRunning;
        //Game.DisplayNotification($"ModController.DebugCoreRunning {ModController.DebugCoreRunning}");
        //GameFiber.Sleep(500);


        Game.LocalPlayer.IsInvincible = false;
        Game.DisplayNotification("IsInvincible = False");
    }
    private void DebugNumpad2()
    {
        //ModController.DebugSecondaryRunning = !ModController.DebugSecondaryRunning;
        //Game.DisplayNotification($"ModController.DebugSecondaryRunning {ModController.DebugSecondaryRunning}");
        //GameFiber.Sleep(500);
        //Dispatcher.DebugSpawnCop();


        // Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth - 50;



        //PedExt toControl = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        // if (toControl == null)
        // {
        //     return;
        // }
        // NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP(toControl.Pedestrian, toControl.Pedestrian.Position.X, toControl.Pedestrian.Position.Y, toControl.Pedestrian.Position.Z, 10f, 0);
        // GameFiber.Sleep(1000);
        // Game.DisplaySubtitle("RAN ONE");


        ///Burglar_Bell

        int alarmSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();

        Vector3 Coords = Game.LocalPlayer.Character.Position;
        uint GameTimeStarted = Game.GameTime;
        while(!NativeFunction.Natives.REQUEST_SCRIPT_AUDIO_BANK<bool>("Alarms",false,-1) && Game.GameTime - GameTimeStarted <= 2000)
        {
            GameFiber.Yield();
        }

        NativeFunction.Natives.PLAY_SOUND_FROM_COORD(alarmSoundID, "Burglar_Bell", Coords.X, Coords.Y, Coords.Z, "Generic_Alarms", false, 0, false);


        //NativeFunction.Natives.PLAY_SOUND_FROM_COORD(alarmSoundID, "ALARMS_KLAXON_03_CLOSE", Coords.X, Coords.Y, Coords.Z, "", false,0,false);

        GameFiber.Sleep(5000);

        NativeFunction.Natives.STOP_SOUND(alarmSoundID);

        NativeFunction.Natives.RELEASE_SOUND_ID(alarmSoundID);

    }
    private void DebugNumpad3()
    {
        WriteCivilianAndCopState();

    }
    public enum DoorState
    {
        Unknown = -1,
        Unlocked = 0,
        Locked = 1,
        LockedUntilOutOfArea = 2,
        UnlockedUntilOutOfArea = 3,
        LockedThisFrame = 4,
        OpenThisFrame = 5,
        ClosedThisFrame = 6
    }

    public int FindDoor(Vector3 position, ulong model)
    {
        int ret;
        unsafe
        {
            NativeFunction.CallByName<bool>("DOOR_SYSTEM_FIND_EXISTING_DOOR", position.X, position.Y, position.Z, model, &ret);
        }
        return ret;
    }
    private void DebugNumpad4()
    {



        foreach(VehicleExt vehicle in World.Vehicles.NonServiceVehicles)
        {
            EntryPoint.WriteToConsole($"NonServiceVehicles: {vehicle.Handle} {vehicle.IsMotorcycle}");
        }


        //int trackID = NativeFunction.Natives.GET_AUDIBLE_MUSIC_TRACK_TEXT_ID<int>();


        //string artistNameLabel = $"{trackID}A";
        //string songNameLabel = $"{trackID}S";


        //string artistName = NativeFunction.Natives.GET_FILENAME_FOR_AUDIO_CONVERSATION<string>(artistNameLabel);
        //string songName = NativeFunction.Natives.GET_FILENAME_FOR_AUDIO_CONVERSATION<string>(songNameLabel);


        //Game.DisplaySubtitle($"{artistName} - {songName}  {artistNameLabel}-{songNameLabel}  {Game.GetLocalizedString(trackID.ToString())} {Game.GetLocalizedString(songNameLabel)}");
        //GameFiber.Sleep(500);
        //NativeFunction.Natives.FREEZE_RADIO_STATION("RADIO_19_USER");
        ;

        //NativeFunction.Natives.SKIP_RADIO_FORWARD();
        //GameFiber.Sleep(500);

        //if(Player.ClosestInteractableLocation == null)
        //{
        //    EntryPoint.WriteToConsole($"No locations");
        //    return;
        //}
        //EntryPoint.WriteToConsole($"{Player.ClosestInteractableLocation.Name} {Player.ClosestInteractableLocation.CanCurrentlyInteract(Player)} {Player.ClosestInteractableLocation.ButtonPromptText}");


        //Player.GroupManager.SetInvincible();


        //if (PumpHandleProp.Exists())
        //{
        //    PumpHandleProp.Delete();
        //    return;
        //}

        //try
        //{
        //    PumpHandleProp = new Rage.Object("prop_cs_fuel_nozle", Player.Character.GetOffsetPositionUp(50f));// new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
        //}
        //catch (Exception ex)
        //{

        //}
        //if (!PumpHandleProp.Exists())
        //{

        //    return;
        //}

        //VehicleExt vehicleExt = Player.CurrentLookedAtVehicle;
        //if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        //{
        //    return;
        //}
        //string vehicleBoneName = string.IsNullOrEmpty(Settings.SettingsManager.DebugSettings.DebugLastBone) ? "wheel_lr" : Settings.SettingsManager.DebugSettings.DebugLastBone;
        //Vector3 VehicleOffset = new Vector3(Settings.SettingsManager.DebugSettings.DebugLastX, Settings.SettingsManager.DebugSettings.DebugLastY, Settings.SettingsManager.DebugSettings.DebugLastZ);
        //Rotator VehicleRotation = new Rotator(Settings.SettingsManager.DebugSettings.DebugRotate1, Settings.SettingsManager.DebugSettings.DebugRotate2, Settings.SettingsManager.DebugSettings.DebugRotate3);
        //PumpHandleProp.AttachTo(vehicleExt.Vehicle, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", vehicleExt.Vehicle, vehicleBoneName), VehicleOffset, VehicleRotation);

        //GameFiber.Sleep(5000);

        //if (PumpHandleProp.Exists())
        //{
        //    PumpHandleProp.Delete();
        //    return;
        //}

        ////TASK_INVESTIGATE_COORDS
        //if (LastPed != null && LastPed.Pedestrian.Exists())
        //{
        //    LastPed.CanBeAmbientTasked = true;
        //    LastPed.CanBeTasked = true;
        //    LastPed.WeaponInventory.ShouldAutoSetWeaponState = true;
        //    NativeFunction.Natives.SET_PED_USING_ACTION_MODE(LastPed.Pedestrian, false, -1, "DEFAULT_ACTION");
        //}
        //Cop Ped = World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && !x.IsInVehicle && !x.IsDead).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //if (Ped != null)
        //{
        //    LastPed = Ped;
        //    Ped.CanBeAmbientTasked = false;
        //    Ped.CanBeTasked = false;

        //}
        //Ped.Pedestrian.Tasks.ClearImmediately();
        //Ped.WeaponInventory.SetDeadly(true);
        //Ped.WeaponInventory.ShouldAutoSetWeaponState = false;
        //Vector3 SearchCoords = Player.Character.GetOffsetPositionFront(50f);






        ////AnimationDictionary.RequestAnimationDictionay("amb@code_human_police_investigate@base");


        ////NativeFunction.Natives.TASK_WANDER_SPECIFIC(Ped.Pedestrian, "amb@code_human_police_investigate@base","base", 0);

        //NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Ped.Pedestrian, true, -1, "DEFAULT_ACTION");


        //NativeFunction.Natives.TASK_AGITATED_ACTION_CONFRONT_RESPONSE(Ped.Pedestrian, Player.Character);

        //unsafe
        //{
        //    int lol = 0;
        //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
        //    if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
        //    {
        //        EntryPoint.WriteToConsole("LOCATE SET TO LEAVE VEHICLE AND WANDER");
        //        NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
        //        NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
        //    }
        //    Vector3 RandomPlaceOnFoot = SearchCoords.Around2D(2f);
        //    NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 1.5f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
        //    NativeFunction.CallByName<bool>("TASK_WANDER_STANDARD", 0, 0, 0);
        //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
        //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
        //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
        //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        //}



        //amb@code_human_police_investigate@base

        //Vector3 Corner1 = new Vector3(984.8781f, -1777.009f, 31.19557f);
        //Vector3 Corner2 = new Vector3(919.2766f, -1919.417f, 40.12272f);
        /*			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", joaat("prop_vend_soda_01"), 100, 10f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend2", joaat("prop_vend_soda_02"), 100, 10f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", joaat("sf_prop_sf_vend_drink_01a"), 100, 10f, -1, 8);
        			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("ob_cashregister", joaat("prop_till_01"), 100, 100f, -1, 9);
         
         			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", joaat("prop_atm_01"), 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", joaat("prop_atm_02"), 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 904538042, 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", joaat("prop_atm_03"), 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -171012495, 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", joaat("prop_fleeca_atm"), 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 807411288, 100, 4f, -1, 9);
			BRAIN::REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -639162137, 100, 4f, -1, 8);
         */



        // NativeFunction.Natives.SET_PED_NON_CREATION_AREA(Corner1.X, Corner1.Y, Corner1.Z, Corner2.X, Corner2.Y, Corner2.Z);

        //       if(LastPed != null && LastPed.Pedestrian.Exists())
        //       {
        //           LastPed.Pedestrian.IsPersistent = false;
        //       }
        //       PedExt Ped = World.Pedestrians.Citizens.Where(x=> x.Pedestrian.Exists() && x.IsInVehicle && x.IsDriver && !x.IsDead).OrderBy(x=> x.DistanceToPlayer).FirstOrDefault();



        //       if (Ped != null)
        //       {
        //           LastPed = Ped;
        //           Ped.Pedestrian.IsPersistent = true;
        //           Ped.Pedestrian.BlockPermanentEvents = true;
        //           Ped.Pedestrian.KeepTasks = true;
        //           NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);

        //           //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, true);
        //           //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, true);
        //           //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);


        //           /*VEHICLE_CHASE_CANT_BLOCK						= 1,
        //VEHICLE_CHASE_CANT_BLOCK_FROM_PURSUE			= 2,
        //VEHICLE_CHASE_CANT_PURSUE						= 4,
        //VEHICLE_CHASE_CANT_RAM							= 8,
        //VEHICLE_CHASE_CANT_SPIN_OUT						= 16,
        //VEHICLE_CHASE_CANT_MAKE_AGGRESSIVE_MOVE			= 32,
        //VEHICLE_CHASE_CANT_CRUISE_IN_FRONT_DURING_BLOCK	= 64,
        //VEHICLE_CHASE_USE_CONTINUOUS_RAM				= 128,
        //VEHICLE_CHASE_CANT_PULL_ALONGSIDE				= 256,
        //VEHICLE_CHASE_CANT_PULL_ALONGSIDE_INFRONT		= 512*/

        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 1, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 2, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 4, false);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 8, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 16, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 32, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 64, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 128, false);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 256, true);
        //           NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, 512, true);

        //           NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);


        //           GameFiber.Sleep(1000);
        //       }


        //if(!isShowingTunnel)
        //{
        //    Game.dis
        //    isShowingTunnel = true;
        //}
        //else
        //{
        //    isShowingTunnel = false;
        //}


        //GameFiber.StartNew(delegate
        //{
        //    uint GameTimeStarted = Game.GameTime;
        //    while (!Game.IsKeyDownRightNow(Keys.Space))
        //    {
        //        Game.DisplaySubtitle($"IsInTunnel {Player.CurrentLocation.IsInTunnel} PossiblyInTunnel {Player.CurrentLocation.PossiblyInTunnel} IsInside {Player.CurrentLocation.IsInside} SPACE TO STOP");
        //        GameFiber.Yield();
        //    }

        //}, "Run Debug Logic");


        //if(Game.TimeScale >= 0.1f)
        //{
        //    Game.TimeScale -= 0.05f;
        //}
        //GameFiber.Sleep(500);

        //CurrentDictionary = NativeHelper.GetKeyboardInput("dict");
        //CurrentAnimation = NativeHelper.GetKeyboardInput("anim");
        //Game.DisplaySubtitle($"Updated Anims CurrentDict:{CurrentDictionary} CurrentAnimation:{CurrentAnimation}");

        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", Game.GetHashKey("prop_vend_soda_01"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend2", Game.GetHashKey("prop_vend_soda_02"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", Game.GetHashKey("sf_prop_sf_vend_drink_01a"), 0, 0.01f, -1, 8);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_cashregister", Game.GetHashKey("prop_till_01"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_01"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_02"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 904538042, 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_03"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -171012495, 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_fleeca_atm"), 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 807411288, 0, 0.01f, -1, 9);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -639162137, 0, 0.01f, -1, 8);




        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", Game.GetHashKey("prop_vend_soda_01"), 100, 10f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend2", Game.GetHashKey("prop_vend_soda_02"), 100, 10f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_vend1", Game.GetHashKey("sf_prop_sf_vend_drink_01a"), 100, 10f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("ob_cashregister", Game.GetHashKey("prop_till_01"), 100, 100f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_01"), 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_02"), 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 904538042, 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_atm_03"), 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -171012495, 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", Game.GetHashKey("prop_fleeca_atm"), 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", 807411288, 100, 4f, -1, 1024);
        //NativeFunction.Natives.REGISTER_OBJECT_SCRIPT_BRAIN("atm_trigger", -639162137, 100, 4f, -1, 1024);

        //GameLocation location = World.Places.ActiveLocations.Where(x => x.IsActivated).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //if (location == null)
        //{
        //    return;
        //}
        //Game.DisplaySubtitle($"Found Location {location.Name}");
        //SpawnLocation spawnLocation = new SpawnLocation(location.EntrancePosition);
        //spawnLocation.GetClosestStreet(false);
        //spawnLocation.GetClosestSideOfRoad();

        //GameFiber.StartNew(delegate
        //{
        //    uint GameTimeStarted = Game.GameTime;
        //    while (Game.GameTime - GameTimeStarted <= 20000)
        //    {
        //        if(spawnLocation.HasSideOfRoadPosition)
        //        {
        //            Rage.Debug.DrawArrowDebug(spawnLocation.StreetPosition + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color.Red);
        //        }
        //        else if (spawnLocation.HasStreetPosition)
        //        {
        //            Rage.Debug.DrawArrowDebug(spawnLocation.StreetPosition + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color.White);
        //        }
        //        GameFiber.Yield();
        //    }

        //}, "Run Debug Logic");




        //NativeFunction.Natives.DISABLE_SCRIPT_BRAIN_SET(1024);
        //foreach(PedExt pedExt in World.Pedestrians.PedExts)
        //{
        //    if(pedExt.WasModSpawned)
        //    {
        //        EntryPoint.WriteToConsole($"CLEARING TASKS FOR {pedExt.Handle}");
        //        pedExt.ClearTasks(true);
        //    }
        //}


        //foreach(VehicleExt veh in Player.VehicleOwnership.OwnedVehicles.Where(x=>x.Vehicle.Exists()))
        //{
        //    EntryPoint.WriteToConsole($"{veh.Vehicle.Model.Name} {veh.IsImpounded} {veh.ImpoundedLocation} {veh.TimesImpounded} {veh.DateTimeImpounded}");
        //}

        //LighterItem lighterItem = Player.Inventory.ItemsList.Where(x=> x.ModItem != null).Select(x=> x.ModItem).OfType<LighterItem>().ToList().FirstOrDefault();
        //if (lighterItem == null)
        //{
        //    Game.DisplayHelp($"Need a ~r~Lighter~s~ to use");
        //   // return false;
        //}
        //else
        //{
        //    Game.DisplayHelp($"FOUND{lighterItem.Name}");
        //}
        //NativeFunction.Natives.SET_BIGMAP_ACTIVE(!IsBigMapActive, false);
        //Game.DisplaySubtitle($"IsBigMapActive:{IsBigMapActive}");
        //GameFiber.Sleep(1000);
        //IsBigMapActive = !IsBigMapActive;

        //GarageDoor = new InteriorDoor(3082692265,new Vector3(5.644455f,0.1074037f, 2.158299f)) },

        // int doorID = FindDoor(new Vector3(-1355.819f, -754.4543f, 23.49588f), 3082692265);



        // bool isRegistered = NativeFunction.Natives.IS_DOOR_REGISTERED_WITH_SYSTEM<bool>(doorID);

        //// GameFiber.Sleep(500);
        // bool isClosed = NativeFunction.Natives.IS_DOOR_CLOSED<bool>(doorID);

        // int doorState = NativeFunction.Natives.DOOR_SYSTEM_GET_DOOR_STATE<int>(doorID);
        // string helpText = $"{doorID} isRegistered{isRegistered} isClosed{isClosed} doorState{doorState}";
        // Game.DisplayHelp(helpText);
        // EntryPoint.WriteToConsole(helpText);


        // //NativeFunction.Natives.ADD_DOOR_TO_SYSTEM(doorID, 3082692265, new Vector3(-1355.819f, -754.4543f, 23.49588f), true, true, false);


        // NativeFunction.Natives.DOOR_SYSTEM_SET_DOOR_STATE(doorID, 0, false, false);

        //K9Test();
        // Player.CellPhone.OpenBurner();
        // GameFiber.Sleep(1000);

        // return;

        //HighlightDoorsAndProps();


        //VehicleExt myCar = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 100f);
        //if (myCar != null && myCar.Vehicle.Exists())
        //{
        //    NativeFunction.Natives.SET_VEHICLE_USE_PLAYER_LIGHT_SETTINGS(myCar.Vehicle, true);
        //    Game.DisplayHelp("Set Light State");
        //}
        //// return;
        // Player.ResetScannerDebug();
        //  Player.AddCrime(Crimes.CrimeList.PickRandom(), false, Game.LocalPlayer.Character.Position, null, null, false, true, false);

        //Player.ResetScannerDebug();
        //Player.ScannerPlayDebug();
        // NativeFunction.Natives.x48608C3464F58AB4(0f, 50f, 0f);
        //RelationshipGroup myRG = Game.LocalPlayer.Character.RelationshipGroup;
        //foreach (Gang gang in Gangs.AllGangs)
        //{
        //    RelationshipGroup gangRG = new RelationshipGroup(gang.ID);
        //    int Rel1 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_GROUPS<int>(myRG.Hash, gangRG.Hash);
        //    int Rel2 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_GROUPS<int>(gangRG.Hash, myRG.Hash);
        //    EntryPoint.WriteToConsole($"Gang {gang.FullName} Rel1 {Rel1} Rel2 {Rel2}", 5);

        //}
        //Game.DisplayNotification($"Interior ID {Player.CurrentLocation?.CurrentInterior?.ID}");

        //PedExt myPed = World.Pedestrians.Citizens.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();

        //if (myPed != null)
        //{


        //    // myPed.IsDealingDrugs = true;



        //    //if(RandomItems.RandomPercent(50))
        //    //{
        //    myPed.ShopMenu = ShopMenus.GetRandomDrugCustomerMenu();
        //    //}
        //    //else
        //    //{
        //    //    myPed.ShopMenu = ShopMenus.GetRandomDrugCustomerMenu();
        //    //}

        //    //}
        //}
        //if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        //{
        //    Player.CurrentVehicle.HasUpdatedPlateType = true;


        //    int CurrentPlateStyleIndex = NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.CurrentVehicle.Vehicle);
        //    //EntryPoint.WriteToConsoleTestLong($"Plate 1: CurrentPlateStyleIndex {CurrentPlateStyleIndex}");

        //    if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int newPlateStyleIndex))
        //    {



        //        PlateType CurrentType = PlateTypes.GetPlateType(CurrentPlateStyleIndex);
        //        if (CurrentType != null && Player.CurrentVehicle.CanUpdatePlate)
        //        {
        //            //EntryPoint.WriteToConsoleTestLong($"Plate 2: newPlateStyleIndex {newPlateStyleIndex}");
        //            PlateType NewType = PlateTypes.GetPlateType(newPlateStyleIndex);//PlateTypes.GetRandomPlateType();
        //            if (NewType != null)
        //            {
        //                //EntryPoint.WriteToConsoleTestLong("Plate 3");
        //                string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
        //                if (NewPlateNumber != "")
        //                {
        //                    //EntryPoint.WriteToConsoleTestLong("Plate 4");
        //                    Player.CurrentVehicle.Vehicle.LicensePlate = NewPlateNumber;
        //                    Player.CurrentVehicle.OriginalLicensePlate.PlateNumber = NewPlateNumber;
        //                    Player.CurrentVehicle.CarPlate.PlateNumber = NewPlateNumber;
        //                }
        //                if (NewType.Index <= NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES"))
        //                {
        //                    //EntryPoint.WriteToConsoleTestLong($"OldPlateType {CurrentType.Index} {CurrentType.State} {CurrentType.Description}");
        //                    int test = NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES");
        //                    //EntryPoint.WriteToConsoleTestLong($"Total Plates: {test} NewPlateType {NewType.Index} {NewType.State} {NewType.Description}");
        //                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.CurrentVehicle.Vehicle, NewType.Index);
        //                    Player.CurrentVehicle.OriginalLicensePlate.PlateType = NewType.Index;
        //                    Player.CurrentVehicle.CarPlate.PlateType = NewType.Index;
        //                }
        //                // //EntryPoint.WriteToConsole("UpdatePlate", string.Format("Updated {0} {1}", Vehicle.Model.Name, NewType.Index));
        //            }
        //        }
        //    }
        //}

        //SpawnNoGunAttackers();

        //SpawnGunAttackers();

        //  NativeFunction.Natives.x759E13EBC1C15C5A(50f);

        //ModController.DebugQuaternaryRunning = !ModController.DebugQuaternaryRunning;
        //Game.DisplayNotification($"ModController.DebugQuaternaryRunning {ModController.DebugQuaternaryRunning}");
        ////GameFiber.Sleep(500);
        //float ClosestDistance = 999f;
        //GameLocation gameLocation = null;
        //foreach(GameLocation gl in PlacesOfInterest.GetAllPlaces())
        //{
        //    float distance = gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character);
        //    if (distance < ClosestDistance)
        //    {
        //        gameLocation = gl;
        //        ClosestDistance = distance;
        //    }
        //}
        //if(gameLocation != null)
        //{
        //    EntryPoint.WriteToConsole($"Debug Recycling Location {gameLocation.Name}", 5);
        //    gameLocation.Setup(Interiors,Settings,Crimes,Weapons);
        //    GameFiber.Sleep(2500);
        //    gameLocation.Dispose();
        //}





        //if (Player.IsDriver && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())// Game.LocalPlayer.Character.CurrentVehicle.Exists() && )
        //{
        //    bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(Player.CurrentVehicle.Vehicle, 0);
        //    if (isValid)
        //    {
        //        float DoorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(Player.CurrentVehicle.Vehicle, 0);

        //        if (DoorAngle > 0.0f)
        //        {
        //            string toPlay = "";
        //            int TimeToWait = 250;
        //            if (DoorAngle >= 0.7)
        //            {
        //                toPlay = "d_close_in";
        //                TimeToWait = 500;
        //            }
        //            else
        //            {
        //                toPlay = "d_close_in_near";
        //            }
        //            EntryPoint.WriteToConsole($"Player Event: Closing Door Manually Angle {DoorAngle} Dict veh@std@ds@enter_exit Animation {toPlay}", 5);
        //            AnimationDictionary.RequestAnimationDictionay("veh@std@ds@enter_exit");
        //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "veh@std@ds@enter_exit", toPlay, 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1      
        //            GameFiber DoorWatcher = GameFiber.StartNew(delegate
        //            {
        //                GameFiber.Sleep(TimeToWait);
        //                if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
        //                {
        //                    NativeFunction.Natives.SET_VEHICLE_DOOR_SHUT(Game.LocalPlayer.Character.CurrentVehicle, 0, false);
        //                    GameFiber.Sleep(250);
        //                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        //                }
        //                else
        //                {
        //                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        //                }
        //            }, "DoorWatcher");
        //        }
        //    }
        //}


        //mp_doorbell
        //open_door
        //AnimationDictionary.RequestAnimationDictionay("switch@michael@biking_with_jimmy");
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "switch@michael@biking_with_jimmy", "exit_right_door", 8.0f, -8.0f, -1, 50, 0, false, false, false);//-1
        //Player.AddToInventory(ModItems.Get("Hot Dog"), 4);
        //Player.AddToInventory(ModItems.Get("Can of eCola"), 4);
        //Player.AddToInventory(ModItems.Get("Redwood Regular"), 4);
        //Player.AddToInventory(ModItems.Get("Alco Patch"), 4);

        //Player.AddToInventory(ModItems.Get("Equanox"), 4);
        //Player.AddToInventory(ModItems.Get("Screwdriver"), 1);
        //Player.AddToInventory(ModItems.Get("DIC Lighter"), 1);
        //Weather.DebugPlayReport();


        //Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
        //if (ClosestEntity.Exists())
        //{


        //    Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
        //    DesiredPos = new Vector3(DesiredPos.X, DesiredPos.Y, Game.LocalPlayer.Character.Position.Z);
        //    float DesiredHeading = Math.Abs(ClosestEntity.Heading + 180f);
        //    float ObjectHeading = ClosestEntity.Heading;
        //    if (ClosestEntity.Heading >= 180f)
        //    {
        //        DesiredHeading = ClosestEntity.Heading - 180f;
        //    }
        //    else
        //    {
        //        DesiredHeading = ClosestEntity.Heading + 180f;
        //    }




        //    EntryPoint.WriteToConsole($"Sitting Closest = {ClosestEntity.Model.Name}", 5);
        //    EntryPoint.WriteToConsole($"Sitting Activity ClosestSittableEntity X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z}", 5);


        //    if (ClosestEntity.Model.Dimensions.X >= 2f)
        //    {

        //    }

        //    uint GameTimeStartedDisplaying = Game.GameTime;
        //    while (Game.GameTime - GameTimeStartedDisplaying <= 3000)
        //    {

        //        Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
        //        GameFiber.Yield();
        //    }

        //  }
        //// Player.ScannerPlayDebug();

        //SetInRandomInterior();
        //BrowseTimecycles();

        // if (Game.LocalPlayer.Character.CurrentVehicle.Exists()) { NativeFunction.Natives.SET_VEHICLE_LIVERY(Game.LocalPlayer.Character.CurrentVehicle, 17);  }

        // Dispatcher.RemoveRoadblock();
        //if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        //{
        //    int TotalLiveries = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.CurrentVehicle.Vehicle);
        //    if (TotalLiveries > -1)
        //    {
        //        int LiveryNumber = RandomItems.GetRandomNumberInt(0, TotalLiveries-1);
        //        NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.CurrentVehicle.Vehicle, LiveryNumber);
        //    }
        //if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        //{
        //    int TotalLiveries = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.CurrentVehicle.Vehicle);
        //    if (TotalLiveries > -1)
        //    {
        //        int LiveryNumber = RandomItems.GetRandomNumberInt(0, TotalLiveries-1);
        //        NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.CurrentVehicle.Vehicle, LiveryNumber);
        //    }
        //}
        //NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Game.LocalPlayer.Character, 3, 1, 0, 0);

        //EntryPoint.WriteToConsole($"Game.IsPaused {Game.IsPaused}", 5);
        // IssueWeapons(Weapons);
        //set in garage
        //Game.LocalPlayer.Character.Position = new Vector3(229.9559f, -981.7928f, -99.66071f);
        //Model characterModel = new Model(0xB779A091);
        //characterModel.LoadAndWait();
        //Vector3 Position = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        //NativeFunction.Natives.CREATE_VEHICLE<Vehicle>(0xB779A091, Position.X, Position.Y, Position.Z, 0f, false, false);
        // Player.AddCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "HitPedWithCar"), false, Game.LocalPlayer.Character.Position, null, null, RandomItems.RandomPercent(25), true, true);
        //Game.LocalPlayer.IsInvincible = true;
        //Game.DisplayNotification("IsInvincible = True");
        //foreach (InteriorPosition ip in MPInteriorPositions)
        //{
        //    if (ip != null)
        //    {
        //        EntryPoint.WriteToConsole($"Player Set In {ip.Name}", 5);
        //        Game.DisplayNotification(ip.Name);
        //        Game.LocalPlayer.Character.Position = ip.Position;
        //        GameFiber.Sleep(1000);
        //        string toWrite = $",new Interior({Player.CurrentLocation.CurrentInterior.ID}, \"{ip.Name}\")";
        //        toWrite += " { IsMPOnly = true }";
        //        WriteToLogLocations(toWrite);

        //        GameFiber.Sleep(2000);
        //    }
        //}
        //Game.LocalPlayer.IsInvincible = false;
        //Game.DisplayNotification("IsInvincible = False");
        // SpawnNoGunAttackers();
        //SpawnBus();
    }


   

    private void DebugNumpad5()
{
        CanineUnit k9 = World.Pedestrians.PoliceCanines.Where(x => x.Pedestrian.Exists()).OrderBy(x=> x.DistanceToPlayer).FirstOrDefault();
        if(k9 == null)
        {
            return;
        }
        if(!k9.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.Natives.PLAY_ANIMAL_VOCALIZATION(k9.Pedestrian, 2, "BARK");
        Game.DisplaySubtitle("Bark Played");
        GameFiber.Sleep(500);

        //NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(2,(int)GameControl.VehicleNextRadioTrack, 1.0f);
        //GameFiber.Sleep(100);

        //foreach(ContactRelationship test in Player.RelationshipManager.ContactRelationships)
        //{
        //    EntryPoint.WriteToConsole($"ContactRelationship {test.ContactName} HasPhoneContact:{test.HasPhoneContact} ReputationLevel:{test.ReputationLevel} PlayerDebt:{test.PlayerDebt}");
        //}
        //PhoneContact cool = ModDataFileManager.Contacts.GetContactData(StaticStrings.UndergroundGunsContactName);





        //Player.CellPhone.AddScheduledText(cool, "this is a test text", 0, false);

        //GameFiber DoorWatcher = GameFiber.StartNew(delegate
        //{

        //    while (true)
        //    {
        //        BlackJackGame blackJackGameInternal = new BlackJackGame(Player, Settings, false, null, new BlackJackGameRules());
        //        blackJackGameInternal.StartRound();

        //        if(!blackJackGameInternal.IsActive)
        //        {
        //            break;   
        //        }
        //        GameFiber.Yield();
        //    }
        //}, "DoorWatcher");


        //GameFiber DoorWatcher = GameFiber.StartNew(delegate
        //{

        //    while (true)
        //    {
        //        try
        //        {


        //            RouletteGame rouletteGame = new RouletteGame(Player, Settings, ModDataFileManager.PlacesOfInterest.PossibleLocations.GamblingDens.FirstOrDefault(), new RouletteGameRules());
        //            rouletteGame.Setup();
        //            rouletteGame.StartRound();

        //            if (!rouletteGame.IsActive)
        //            {
        //                break;
        //            }
        //            GameFiber.Yield();
        //        }
        //        catch(Exception ex)
        //        {
        //            Game.DisplaySubtitle(ex.ToString());
        //        }
        //    }
        //}, "DoorWatcher");

        //if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int seatIndex) && Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
        //{
        //    NativeFunction.Natives.TASK_WARP_PED_INTO_VEHICLE(Player.Character, Player.InterestedVehicle.Vehicle, seatIndex);
        //}

        //GameFiber DoorWatcher = GameFiber.StartNew(delegate
        //{
        //    Vector3 Position = Game.LocalPlayer.Character.GetOffsetPositionFront(5f);
        //    float Heading = Game.LocalPlayer.Character.Heading;
        //    uint GameTimeStarted = Game.GameTime;
        //    Color color = Color.Green;
        //    while (Game.GameTime - GameTimeStarted <= 5000)
        //    {

        //        //bool isObscured = NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(Position.X, Position.Y, Position.Z, Settings.SettingsManager.DebugSettings.ObscuredX, Settings.SettingsManager.DebugSettings.ObscuredY, Settings.SettingsManager.DebugSettings.ObscuredZ, Game.LocalPlayer.Character);
        //        Vector3 MinCoords = NativeHelper.GetOffsetPosition(Position, Heading, -1.0f * Settings.SettingsManager.DebugSettings.ObscuredX);
        //        MinCoords = NativeHelper.GetOffsetPosition(MinCoords, Heading -90, -1.0f * Settings.SettingsManager.DebugSettings.ObscuredX);


        //        Vector3 MaxCoords = NativeHelper.GetOffsetPosition(Position, Heading, 1.0f * Settings.SettingsManager.DebugSettings.ObscuredX);
        //        MaxCoords = NativeHelper.GetOffsetPosition(MinCoords, Heading + 90, 1.0f * Settings.SettingsManager.DebugSettings.ObscuredX);

        //        bool isObscured = NativeFunction.Natives.IS_AREA_OCCUPIED<bool>(MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z, true,true,true,true,false,0,true);
        //        //NativeFunction.Natives.DRAW_DEBUG_BOX(MinCoords.X, MinCoords.Y, MinCoords.Z, MaxCoords.X, MaxCoords.Y, MaxCoords.Z, 0, 0, 255, 200);

        //        if (isObscured)
        //        {
        //            color = Color.Red;
        //        }
        //        else
        //        {
        //            color = Color.Green;
        //        }

        //        Rage.Debug.DrawArrowDebug(Position, Vector3.Zero, Rotator.Zero, 1f, color);

        //        GameFiber.Yield();
        //    }
        //}, "DoorWatcher");


        // GameFiber.Sleep(500);


        //TASK_WARP_PED_INTO_VEHICLE
        //bool isGround = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, 1000f, out float GroundZ, true, false);

        //Game.DisplaySubtitle($"GroundZ: {GroundZ}");
        //GameFiber.Sleep(500);
        //string dictionary = "savem_default@";
        //string animation = "m_getin_l";

        //if(!string.IsNullOrEmpty(CurrentDictionary))
        //{
        //    dictionary = CurrentDictionary;
        //}
        //if(!string.IsNullOrEmpty(CurrentAnimation))
        //{
        //    animation = CurrentAnimation;
        //}
        //Vector3 startingPos = Game.LocalPlayer.Character.Position;
        //float startingHeading = Game.LocalPlayer.Character.Heading;

        //AnimationDictionary.RequestAnimationDictionay(dictionary);
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 0, false, false, false);//-1
        //GameFiber.Sleep(1000);

        //while(!Game.IsKeyDownRightNow(Keys.O))
        //{
        //    Game.DisplayHelp("PRESS O To Cancel");
        //    GameFiber.Yield();
        //}
        //EntryPoint.WriteToConsole($"startingPos: new Vector3({startingPos.X}f, {startingPos.Y}f, {startingPos.Z}f), startingHeading: {startingHeading}f");
        //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

        //Game.DisplaySubtitle($" TotalCars: {Rage.World.EnumerateVehicles().Count()} Vehicle Capacity {Rage.World.VehicleCapacity}");
        //Game.DisplaySubtitle($"DOOR LOCK SET TO {isDoorLocked} ");

        //Vector3 Pos1 = new Vector3(413.364f, -1620.034f, 28.34158f);
        ////Vector3 Pos2 = new Vector3(410.4831f, -1617.619f, 28.34158f);

        //if (isDoorLocked)
        //{

        //    NativeFunction.Natives.x9B12F9A24FABEDB0(2811495845, Pos1.X, Pos1.Y, Pos1.Z, true, 1.0f);
        //}
        //else
        //{
        //    NativeFunction.Natives.x9B12F9A24FABEDB0(2811495845, Pos1.X, Pos1.Y, Pos1.Z, false, 0, 1.0f);
        //}
        //isDoorLocked = !isDoorLocked;    
        //GameFiber.Sleep(1000);


        //PhoneTest();
        //ShuffleTest();
        //OffsetGarbage();

        //AnimationTester();

        //return;
        //GameFiber.StartNew(delegate
        //{
        //    VehicleExt ClosestVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 5f);
        //    Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionRight(10f).Around2D(10f));
        //    GameFiber.Yield();
        //    bool freezePos = false;
        //    AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");

        //    if (ClosestVehicle!= null && ClosestVehicle.Vehicle.Exists() && coolguy.Exists())
        //    {
        //        //coolguy.Kill();
        //        //NativeFunction.Natives.SET_PED_TO_RAGDOLL(coolguy, -1, -1, 0, false, false, false);
        //       // coolguy.IsPersistent = true;
        //       // coolguy.BlockPermanentEvents = true;
        //        while (ClosestVehicle.Vehicle.Exists() && !Game.IsKeyDownRightNow(Keys.O) && coolguy.Exists())
        //        {
        //            bool hasBoot = ClosestVehicle.Vehicle.HasBone("boot");
        //            Vector3 BootPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
        //            Vector3 RootPosition = ClosestVehicle.Vehicle.GetBonePosition(0);


        //            float YOffset = -1 * BootPosition.DistanceTo2D(RootPosition);
        //            float ZOffset = BootPosition.Z - RootPosition.Z;// BootPosition.DistanceTo2D(RootPosition);





        //            int bootBoneIndex = ClosestVehicle.Vehicle.GetBoneIndex("boot");

        //            Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPositionUp(Settings.SettingsManager.DragSettings.LoadBodyZOffset);
        //            Vector3 FinalPosition = NativeHelper.GetOffsetPosition(NativeHelper.GetOffsetPosition(BootPosition, ClosestVehicle.Vehicle.Heading, Settings.SettingsManager.DragSettings.LoadBodyXOffset), ClosestVehicle.Vehicle.Heading - 90f, Settings.SettingsManager.DragSettings.LoadBodyYOffset);
        //            FinalPosition.Z = AboveVehiclePosition.Z;

        //            Rage.Debug.DrawArrowDebug(BootPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);
        //            Rage.Debug.DrawArrowDebug(AboveVehiclePosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Black);
        //            Rage.Debug.DrawArrowDebug(FinalPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Green);

        //            Rage.Debug.DrawArrowDebug(RootPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Purple);

        //            Rage.Debug.DrawArrowDebug(RootPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);

        //            if (Game.IsKeyDownRightNow(Keys.I))
        //            {
        //                freezePos = !freezePos;
        //                if(freezePos)
        //                {
        //                    NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(coolguy, ClosestVehicle.Vehicle, Settings.SettingsManager.DragSettings.BoneIndex, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyXOffset, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyYOffset + YOffset, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyZOffset + ZOffset, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyXRotation, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyYRotation, 
        //                        Settings.SettingsManager.DragSettings.LoadBodyZRotation, 
        //                        false, false, false, Settings.SettingsManager.DragSettings.UseBasicAttachIfPed, Settings.SettingsManager.DragSettings.Euler, Settings.SettingsManager.DragSettings.OffsetIsRelative, false);
        //                    //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(coolguy, ClosestVehicle.Vehicle, bootBoneIndex, FinalPosition.X, FinalPosition.Y, FinalPosition.Z, 0.0f, 0.0f, 0.0f, false, false, false, false, 2, false, false);
        //                    GameFiber.Wait(100);
        //                    NativeFunction.Natives.TASK_PLAY_ANIM(coolguy, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2, 0, false, false, false);
        //                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(coolguy, "timetable@floyd@cryingonbed@base", "base", 1.0f);
        //                    Game.DisplaySubtitle("ATTACHED");
        //                }
        //                else
        //                {
        //                    coolguy.Detach();
        //                    Game.DisplaySubtitle("DETACHED");
        //                }


        //                //if(!freezePos)
        //                //{
        //                //    coolguy.BlockPermanentEvents = true;
        //                //    coolguy.IsRagdoll = true;
        //                //    NativeFunction.Natives.SET_PED_TO_RAGDOLL(coolguy, -1, -1, 0, false, false, false);
        //                //}
        //                GameFiber.Sleep(500);
        //            }
        //            //if(freezePos)
        //            //{
        //            //    coolguy.Position = FinalPosition;
        //            //}
        //            Game.DisplayHelp("PRESS O TO CANCEL, I to Drop");
        //            GameFiber.Yield();
        //        }

        //    }


        //    if (coolguy.Exists())
        //    {
        //        coolguy.Delete();
        //    }



        //}, "Run Debug Logic");




        //Player.CellPhone.CloseBurner();
        //GameFiber.Sleep(1000);
        //   SpawnGunAttackers();

        //GameFiber.StartNew(delegate
        //{
        //    uint GameTimeStarted = Game.GameTime;
        //    while (Game.GameTime - GameTimeStarted <= 5000)
        //    {
        //        Player.IsDoingSuspiciousActivity = true;
        //        GameFiber.Sleep(1000);
        //    }
        //    Player.IsDoingSuspiciousActivity = false;
        //}, "Run Debug Logic");


        //Gang myGang = Gangs.AllGangs.PickRandom();
        //Player.CellPhone.AddContact(myGang, true);

        //PedExt myPed = World.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();

        //if (myPed != null && myPed.Pedestrian.Exists())
        //{
        //    myPed.Pedestrian.Health = 125;
        //    //if(RandomItems.RandomPercent(50))
        //    //{
        //    //    myPed.Pedestrian.Kill();
        //    //}

        //    //else
        //    //{
        //    //    NativeFunction.Natives.TASK_WRITHE(myPed.Pedestrian, Player.Character, -1, false);
        //    //}
        //}

        //SpawnNoGunAttackers();


        //Player.CellPhone.AddScheduledContact("Officer Friendly", "CHAR_BLANK_ENTRY", "", Time.CurrentDateTime.AddMinutes(2));

        //Gang myGang = Gangs.AllGangs.PickRandom();
        //Player.GangRelationships.SetReputation(myGang, 2000, true);

        //SpawnNoGunAttackers();
        //RelationshipGroup myRG = Game.LocalPlayer.Character.RelationshipGroup;
        //foreach (Gang gang in Gangs.AllGangs)
        //{
        //    int randomnum = RandomItems.GetRandomNumberInt(-200, 600);
        //    Player.SetReputation(gang, randomnum);
        //}




        //World.IsZombieApocalypse = !World.IsZombieApocalypse;
        //Game.DisplayNotification($"World.IsZombieApocalypse {World.IsZombieApocalypse} {Game.GameTime}");
        //GameFiber.Sleep(500);
        //NativeFunction.Natives.x48608C3464F58AB4(25f,25f,0f);


        //Player.SetHeadblend();

        //List<string> ShopPeds = new List<string>() { "s_m_y_ammucity_01", "s_m_m_ammucountry", "u_m_y_tattoo_01", "s_f_y_shop_low", "s_f_y_shop_mid", "s_f_m_shop_high", "s_m_m_autoshop_01", "s_m_m_autoshop_02" };
        //foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists()))
        //{
        //    if (ped.Pedestrian.Exists() && ShopPeds.Contains(ped.Pedestrian.Model.Name.ToLower()))
        //    {
        //        EntryPoint.WriteToConsole($"DEBUG Deleted Ped {ped.Pedestrian.Handle} {ped.Pedestrian.Model.Name}", 5);
        //        ped.Pedestrian.Delete();
        //    }
        //}
        //        //}
        //if (ShopPeds.Contains(ModelName))
        //        {
        //            return false;
        //        }


        //}

        //if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
        //{
        //    float PreAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(Game.LocalPlayer.Character.CurrentVehicle, 0);
        //    NativeFunction.Natives.SET_VEHICLE_DOOR_OPEN(Game.LocalPlayer.Character.CurrentVehicle, 0, true, true);
        //    float PostANgle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(Game.LocalPlayer.Character.CurrentVehicle, 0);
        //    EntryPoint.WriteToConsole($"PreAngle {PreAngle} PostANgle {PostANgle}", 5);   
        //    //NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2.0f, 1, 0);
        //}



        //ModController.DebugQuinaryRunning = !ModController.DebugQuinaryRunning;
        //Game.DisplayNotification($"ModController.DebugQuinaryRunning {ModController.DebugQuinaryRunning}");
        //GameFiber.Sleep(500);







        //List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList();
        //float ClosestDistance = 999f;
        //foreach (Rage.Object obj in Objects)
        //{
        //    if (obj.Exists())// && obj.Model.Name.ToLower().Contains("chair") || obj.Model.Name.ToLower().Contains("bench") || obj.Model.Name.ToLower().Contains("seat") || obj.Model.Name.ToLower().Contains("chr") || SeatModels.Contains(obj.Model.Hash))
        //    {
        //        string modelName = obj.Model.Name.ToLower();
        //        float DistanceToObject = obj.DistanceTo(Game.LocalPlayer.Character.Position);
        //        if (modelName.Contains("chair") || modelName.Contains("sofa") || modelName.Contains("couch") || modelName.Contains("bench") || modelName.Contains("seat") || modelName.Contains("chr"))
        //        {

        //            if (DistanceToObject <= 5f && DistanceToObject >= 0.5f && DistanceToObject <= ClosestDistance)
        //            {
        //                ClosestDistance = DistanceToObject;
        //            }

        //        }
        //        if (DistanceToObject <= 5f)
        //        {
        //            EntryPoint.WriteToConsole($"PROP HUNT: Found {modelName} Hash: {obj.Model.Hash} X: {obj.Model.Dimensions.X} Y: {obj.Model.Dimensions.Y} {DistanceToObject}", 5);
        //        }
        //    }
        //}

















        //Player.Inventory.Add(ModItems.Get("Hot Dog"), 4);
        //Player.Inventory.Add(ModItems.Get("Can of eCola"), 4);
        //Player.Inventory.Add(ModItems.Get("Redwood Regular"), 4);
        //Player.Inventory.Add(ModItems.Get("Alco Patch"), 4);

        //Player.Inventory.Add(ModItems.Get("Equanox"), 4);

        //SetInRandomInterior();

        // Dispatcher.SpawnRoadblock();


        //EntryPoint.WriteToConsole("Zone STRING : " + GetInternalZoneString(Game.LocalPlayer.Character.Position), 5);
        //Player.ResetScannerDebug();

        //Crime toPlay = Crimes.CrimeList.Where(x => x.CanBeReportedByCivilians).PickRandom();
        //CrimeSceneDescription toAnnounce = new CrimeSceneDescription(false, false, Game.LocalPlayer.Character.Position);
        //Player.PlayDispatchDebug(toPlay, toAnnounce);
        //Freecam();
        //Ped completelynewnameAsd = new Ped("S_M_M_GENTRANSPORT", Player.Character.GetOffsetPositionFront(3f), Game.LocalPlayer.Character.Heading); //new Ped(Player.Character.Position.Around2D(5f));//new Ped("a_f_y_smartcaspat_01", Player.Character.Position.Around2D(5f), Game.LocalPlayer.Character.Heading);//S_M_M_GENTRANSPORT
        //GameFiber.Yield();
        //if (!completelynewnameAsd.Exists())
        //{
        //    return;
        //}
        //completelynewnameAsd.Model.LoadAndWait();
        //string tempModelName = completelynewnameAsd.Model.Name;
        //completelynewnameAsd.RandomizeVariation();
        //EntryPoint.WriteToConsole($"SpawnModelChecker! 1 {tempModelName} {completelynewnameAsd.Position}", 5);
        //GameFiber.Sleep(5000);
        //if (completelynewnameAsd.Exists())
        //{
        //    tempModelName = completelynewnameAsd.Model.Name;
        //    EntryPoint.WriteToConsole($"SpawnModelChecker! 2 {tempModelName} {completelynewnameAsd.Position}", 5);
        //}
        //GameFiber.Sleep(500);
        //if (completelynewnameAsd.Exists())
        //{
        //    completelynewnameAsd.Delete();
        //}


        //SpawnItemInFrom();
        //Model characterModel = new Model(0xB779A091);
        //characterModel.LoadAndWait();
        //Vector3 Position = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        //NativeFunction.Natives.CREATE_VEHICLE<Vehicle>(1127131465, Position.X, Position.Y, Position.Z, 0f, false, false);

        //Game.LocalPlayer.Character.Health = RandomItems.MyRand.Next(5, 90);
        //SpawnModelChecker2();
        //string text1 = NativeHelper.GetKeyboardInput("");
        //string toWrite = $"new Interior({Player.CurrentLocation?.CurrentInterior?.ID}, \"{text1}\"),";
        //WriteToLogInteriors(toWrite);


        //SetInRandomInterior();
        //Game.LocalPlayer.IsInvincible = true;
        //Game.DisplayNotification("IsInvincible = True");
        //foreach (InteriorPosition ip in InteriorPositions)
        //{
        //    if (ip != null)
        //    {
        //        EntryPoint.WriteToConsole($"Player Set In {ip.Name}", 5);
        //        Game.DisplayNotification(ip.Name);
        //        Game.LocalPlayer.Character.Position = ip.Position;
        //        GameFiber.Sleep(1000);
        //        string toWrite = $",new Interior({Player.CurrentLocation.CurrentInterior.ID}, \"{ip.Name}\")";
        //        WriteToLog(toWrite);

        //        GameFiber.Sleep(2000);
        //    }
        //}
        //Game.LocalPlayer.IsInvincible = false;
        //Game.DisplayNotification("IsInvincible = False");

        //SpawnGunAttackers();
    }

    private void OffsetGarbage()
{




VehicleExt chosenVehicle = Player.ActivityManager.GetInterestedVehicle();
if(chosenVehicle == null || !chosenVehicle.Vehicle.Exists())
{
    return;
}
Vector3 DoorTogglePosition = Vector3.Zero;
float DoorToggleHeading = 0f;




if (RandomItems.RandomPercent(50f))//is trunk
{
    float length = chosenVehicle.Vehicle.Model.Dimensions.Y;
    DoorTogglePosition = chosenVehicle.Vehicle.Position;
    DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, chosenVehicle.Vehicle.Heading + Settings.SettingsManager.DoorToggleSettings.TrunkHeading, (-1 * length/2) + Settings.SettingsManager.DoorToggleSettings.TrunkOffset);
    DoorToggleHeading = chosenVehicle.Vehicle.Heading;
}
else
{
    float length = chosenVehicle.Vehicle.Model.Dimensions.Y;
    DoorTogglePosition = chosenVehicle.Vehicle.Position;
    DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, chosenVehicle.Vehicle.Heading + Settings.SettingsManager.DoorToggleSettings.HoodHeading, (length / 2) + Settings.SettingsManager.DoorToggleSettings.HoodOffset);
    DoorToggleHeading = chosenVehicle.Vehicle.Heading - 180f;
}



GameFiber.StartNew(delegate
{
    while (chosenVehicle.Vehicle.Exists() && !Game.IsKeyDownRightNow(Keys.O))
    {
        Game.DisplayHelp($"O to Cancel");
        Rage.Debug.DrawArrowDebug(DoorTogglePosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
        GameFiber.Yield();
    }
}, "Run Debug Logic");






}

    private void DebugNumpad6()
    {







        
        World.Places.StaticPlaces.DebugDeactivateAllLocations();
        Game.DisplaySubtitle("DeactivatedLocations");
        GameFiber.Sleep(2000);


        ////X:124.8145 Y:-747.5364 Z:242.152
        ////[4 / 4 / 2024 5:09:09 PM.832] Player ped heading: 248.4043


        ////Player Holding Pos new Vector3(123.7631f, -745.737f, 242.152f);   216.5336f;
        ////BARBER HOLDING POS new Vector3(122.921f, -748.2304f, 242.152f), 305.9934f


        //AnimationDictionary.RequestAnimationDictionay("misshair_shop@hair_dressers");
        ////Game.LocalPlayer.Character.Position = new Vector3(-277.76483154297f, 6224.8930664063f, 31.135352325439f);

        //Vector3 posI = new Vector3(124.8145f, -747.5364f, 242.152f);
        //float heading = 239.2449f;
        //Vector3 anim_pos = new Vector3(0.0f, 0.0f, -2.6f * 57.29578f - Settings.SettingsManager.DebugSettings.BarberRotationYaw);
        //if(chairProp.Exists())
        //{
        //    chairProp.Delete();
        //}

        //chairProp = new Rage.Object("vw_prop_casino_track_chair_01", posI, 360f-239.2449f);
        //if(chairProp.Exists())
        //{
        //    NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(chairProp);
        //}

        //Vector3 posINew = NativeHelper.GetOffsetPosition(posI, heading + Settings.SettingsManager.DebugSettings.BarberHeadingXOffset, Settings.SettingsManager.DebugSettings.BarberXOffset);


        //posINew = NativeHelper.GetOffsetPosition(posINew, heading + Settings.SettingsManager.DebugSettings.BarberHeadingYOffset, Settings.SettingsManager.DebugSettings.BarberYOffset);

        //posINew = new Vector3(posINew.X, posINew.Y, posINew.Z - Settings.SettingsManager.DebugSettings.BarberZOffset);

        //EntryPoint.WriteToConsole($"barberPosition = new Vector3({posINew.X}f,{posINew.Y}f,{posINew.Z}f);");
        //EntryPoint.WriteToConsole($"barberrotation = new Vector3({anim_pos.X}f,{anim_pos.Y}f,{anim_pos.Z}f);");

        //NativeFunction.Natives.TASK_PLAY_ANIM_ADVANCED(Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_enterchair", posINew.X, posINew.Y, posINew.Z, anim_pos.X, anim_pos.Y, anim_pos.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        //GameFiber.Sleep(3000);


















        //foreach(HeadOverlayData ho in Player.CurrentModelVariation.HeadOverlays)
        //{
        //    EntryPoint.WriteToConsole($"OverlayID:{ho.OverlayID} Index:{ho.Index} Opacity:{ho.Opacity} PriColor:{ho.PrimaryColor} SecColor:{ho.SecondaryColor} ColorType:{ho.ColorType}");
        //}
        //if (Player.CurrentVehicle != null)
        //{
        //    //Player.CurrentVehicle.Engine.SetState(false);
        //    //Player.CurrentVehicle.Vehicle.MustBeHotwired = true;
        //    //Player.CurrentVehicle.IsHotWireLocked = true;

        //    EntryPoint.WriteToConsole(
        //        $"IsOnMotorcycle:{Player.IsOnMotorcycle} " +
        //        $"IsOnBicycle:{Player.IsOnBicycle} " +
        //        $"CurrentVehicle.IsMotorcycle:{Player.CurrentVehicle.IsMotorcycle} " +
        //        $"CurrentVehicle.VehicleClass:{Player.CurrentVehicle.VehicleClass} " +
        //        $"CurrentVehicle.IsCar:{Player.CurrentVehicle.IsCar} " +
        //        $"IsHotWireLocked:{Player.CurrentVehicle.IsHotWireLocked} IsDisabled{Player.CurrentVehicle.IsDisabled} Engine.CanToggle{Player.CurrentVehicle.Engine.CanToggle} MustBeHotwired:{Player.CurrentVehicle.Vehicle.MustBeHotwired}"

        //        );
        //}

        //if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int seatIndex) && Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
        //{
        //    uint GameTimeStarted = Game.GameTime;
        //    while (Game.GameTime - GameTimeStarted <= 2000)
        //    {
        //        NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME<bool>(0, (int)GameControl.Enter, 1.0f);
        //        GameFiber.Yield();
        //    }

        //    //NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, Player.InterestedVehicle.Vehicle, -1, seatIndex, 1f, (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_JACK_ANYONE);
        //}
        // GameFiber.Sleep(500);

        //int interiorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(347.2686f, -999.2955f, -99.19622f);

        //if (interiorID != 0)
        //{
        //    NativeFunction.Natives.DISABLE_INTERIOR(interiorID, true);
        //    NativeFunction.Natives.REMOVE_IPL("Medium End Apartment");
        //    NativeFunction.Natives.SET_INTERIOR_ACTIVE(interiorID, false);


        //    Game.DisplaySubtitle($"SET INACTIVE interiorID {interiorID}");
        //    GameFiber.Sleep(500);
        //}

        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("lost_mc", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("LOST_MC", true);


        //foreach(GangDen gangden in ModDataFileManager.PlacesOfInterest.PossibleLocations.GangDens)
        //{
        //    EntryPoint.WriteToConsole($"{gangden.Name} ISMPMAP:{World.IsMPMapLoaded} ISCORRECTMAP{gangden.IsCorrectMap(World.IsMPMapLoaded)}");
        //}



        //PedExt chosen = World.Pedestrians.Civilians.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //if(chosen == null || !chosen.Pedestrian.Exists())
        //{
        //    return;
        //}

        //int pedHeadshotHandle = NativeFunction.Natives.REGISTER_PEDHEADSHOT<int>(chosen.Pedestrian);
        //GameFiber.Sleep(2000);
        //string str = NativeFunction.Natives.GET_PEDHEADSHOT_TXD_STRING<string>(pedHeadshotHandle);
        //NativeHelper.DisplayNotificationCustom(str, str, "Test", "~g~Text Received~s~", "Test 1", NotificationIconTypes.ChatBox, false);
        //string AliveSeatAnimationDictionaryName = "veh@std@ps@enter_exit";
        //string AliveSeatAnimationName = "dead_fall_out";


        //AliveSeatAnimationDictionaryName = NativeHelper.GetKeyboardInput("random@crash_rescue@car_death@std_car");
        //AliveSeatAnimationName = NativeHelper.GetKeyboardInput("loop");


        //AnimationDictionary.RequestAnimationDictionayResult(AliveSeatAnimationDictionaryName);

        //NativeFunction.Natives.TASK_PLAY_ANIM(Game.LocalPlayer.Character, AliveSeatAnimationDictionaryName, AliveSeatAnimationName, 1000.0f, -1000.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
        //NativeFunction.Natives.SET_ANIM_RATE(Game.LocalPlayer.Character, 0.0f, 2, false);

        //        List<string> CoolStuff = new List<string>() {

        //        "manhat09_stream7",
        //"manhat09_stream6",
        //"manhat09_stream5",
        //"manhat09_stream4",
        //"manhat09_stream3",
        //"manhat09_stream2",
        //"manhat09_stream1",
        //"manhat09_stream0",
        //"manhat09_strbig0",
        //"manhat09_lod",
        //"manhat09",

        //        };



        //        if (IsOn)
        //        {
        //            foreach (string ipl in CoolStuff)
        //            {
        //                NativeFunction.Natives.REMOVE_IPL(ipl);
        //            }
        //            Game.DisplaySubtitle("IPLS REMOVED");
        //        }
        //        else
        //        {
        //            foreach (string ipl in CoolStuff)
        //            {
        //                NativeFunction.Natives.REQUEST_IPL(ipl);
        //            }
        //            Game.DisplaySubtitle("IPLS REQUESTED");
        //        }
        //        IsOn = !IsOn;
        //        //Vector3 position = Game.LocalPlayer.Character.Position;
        //        //bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, out Vector3 outPos, 0, 3.0f, 0f);
        //        //Vector3 ClosestNode = outPos;




        //        //int StreetHash = 0;
        //        //int CrossingHash = 0;
        //        //string CurrentStreetName;
        //        //unsafe
        //        //{
        //        //    NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", ClosestNode.X, ClosestNode.Y, ClosestNode.Z, &StreetHash, &CrossingHash);
        //        //}
        //        //string StreetName = string.Empty;
        //        //if (StreetHash != 0)
        //        //{
        //        //    unsafe
        //        //    {
        //        //        IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
        //        //        StreetName = Marshal.PtrToStringAnsi(ptr);
        //        //    }
        //        //    CurrentStreetName = StreetName;
        //        //    GameFiber.Yield();
        //        //}
        //        //else
        //        //{
        //        //    CurrentStreetName = "";
        //        //}


        //        //Game.DisplaySubtitle($"StreetHash {StreetHash} CurrentStreetName {CurrentStreetName} StreetName {StreetName}");
        //        GameFiber.Sleep(200);



        //        //Game.DisplaySubtitle("Disabling LS");
        //        //LSMapDisabler lSMapDisabler = new LSMapDisabler();
        //        //lSMapDisabler.DisableLS();
        //        //GameFiber.Sleep(500);
        //        //Game.DisplaySubtitle("LS Disabled");
        //        //ShockTest();
        //        //DateTime currentOffsetDateTime = new DateTime(2020, Time.CurrentDateTime.Month, Time.CurrentDateTime.Day, Time.CurrentDateTime.Hour, Time.CurrentDateTime.Minute, Time.CurrentDateTime.Second);
        //        //WeatherForecast closestForecast = ModDataFileManager.WeatherForecasts.WeatherForecastList.OrderBy(x => Math.Abs(x.DateTime.Ticks - currentOffsetDateTime.Ticks)).ThenBy(x => x.DateTime).FirstOrDefault();//WeatherForecasts.WeatherForecastList.OrderBy(x => (x.DateTime - currentOffsetDateTime).Duration()).ThenBy(y=>y.DateTime).FirstOrDefault();
        //        //if (closestForecast != null)
        //        //{
        //        //    Game.DisplaySubtitle($"DEBUG Time is {Time.CurrentDateTime} and the closest forcast is {closestForecast.DateTime} {closestForecast.AirTemperature} F {closestForecast.Description} ");
        //        //}


        //        //Player.Scanner.DebugPlayDispatch();

        //        //SpawnWithQuat();
        //HighlightProp();
        //        //SetFlags();
        //        //if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        //        //{
        //        //    BusRide MyBusRide = new BusRide(Player, Player.CurrentVehicle.Vehicle, World, PlacesOfInterest, Settings);
        //        //    MyBusRide.Start();
        //        //}


        //        //ModController.DebugNonPriorityRunning = !ModController.DebugNonPriorityRunning;
        //        //Game.DisplayNotification($"ModController.DebugNonPriorityRunning {ModController.DebugNonPriorityRunning}");
        //        //GameFiber.Sleep(500);



        //        //int TotalEntities = 0;
        //        //EntryPoint.WriteToConsole($"SPAWNED ENTITIES ===============================", 2);
        //        //foreach (Entity ent in EntryPoint.SpawnedEntities)
        //        //{
        //        //    if (ent.Exists())
        //        //    {
        //        //        TotalEntities++;
        //        //        EntryPoint.WriteToConsole($"SPAWNED ENTITY STILL EXISTS {ent.Handle} {ent.GetType()} {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
        //        //    }
        //        //}
        //        //EntryPoint.WriteToConsole($"SPAWNED ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        //        //TotalEntities = 0;

        //        //List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        //        //EntryPoint.WriteToConsole($"PERSISTENT ENTITIES ===============================", 2);
        //        //foreach (Entity ent in AllEntities)
        //        //{
        //        //    if (ent.Exists() && ent.IsPersistent)
        //        //    {
        //        //        TotalEntities++;
        //        //        EntryPoint.WriteToConsole($"PERSISTENT ENTITY STILL EXISTS {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
        //        //    }
        //        //}
        //        //EntryPoint.WriteToConsole($"PERSISTENT ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        //        //WriteCopState();

        //        //SpawnModelChecker();
        //        //Vector3 pos = Game.LocalPlayer.Character.Position;
        //        //float Heading = Game.LocalPlayer.Character.Heading;
        //        //string text1 = NativeHelper.GetKeyboardInput("");
        //        //string text2 = NativeHelper.GetKeyboardInput("");
        //        //WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void DebugNumpad7()
    {



        //BarberXOffset = 1.1f;

        //BarberYOffset = 0.3f;

        //BarberZOffset = 0.2f;

        //BarberHeadingXOffset = 0f;

        //BarberHeadingYOffset = -90f;

        //BarberRotationYaw = 50f;

        //barberPosition = new Vector3(-278.348f,6225.873f,30.93535f);
        //barberrotation = new Vector3(0f, 0f, -498.969f);
        //barberrotationMAYBE>? = new Vector3(0f, 0f, -138.969f);


        //TaskPlayAnimAdvanced(p:ped(), anim, "player_enterchair", data.posI, data.anim_pos, 1000.0, -1000.0, -1, 5642, 0.0, 2, 1)

        //AnimationDictionary.RequestAnimationDictionay("misshair_shop@hair_dressers");
        ////Game.LocalPlayer.Character.Position = new Vector3(-277.76483154297f, 6224.8930664063f, 31.135352325439f);

        //Vector3 posI = new Vector3(-35.654655456543f, -154.29956054688f, 57.82421875f - 1.50f);
        //float heading = 65.38319f;
        //Vector3 anim_pos = new Vector3(0.0f, 0.0f, -2.6f * 57.29578f - 50f);


        //Vector3 posINew = NativeHelper.GetOffsetPosition(posI, heading + Settings.SettingsManager.DebugSettings.BarberHeadingXOffset, Settings.SettingsManager.DebugSettings.BarberXOffset);


        //posINew = NativeHelper.GetOffsetPosition(posINew, heading + Settings.SettingsManager.DebugSettings.BarberHeadingYOffset, Settings.SettingsManager.DebugSettings.BarberYOffset);

        //posINew = new Vector3(posINew.X, posINew.Y, posINew.Z - Settings.SettingsManager.DebugSettings.BarberZOffset);

        //EntryPoint.WriteToConsole($"barberPosition = new Vector3({posINew.X}f,{posINew.Y}f,{posINew.Z}f);");
        //EntryPoint.WriteToConsole($"barberrotation = new Vector3({anim_pos.X}f,{anim_pos.Y}f,{anim_pos.Z}f);");

        //NativeFunction.Natives.TASK_PLAY_ANIM_ADVANCED(Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_enterchair", posINew.X, posINew.Y, posINew.Z, anim_pos.X, anim_pos.Y, anim_pos.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        //GameFiber.Sleep(3000);
        //if(Game.TimeScale == 1.0f)
        //{
        //    Game.TimeScale = Settings.SettingsManager.DebugSettings.SlowMoScaleTime;
        //}
        //else
        //{
        //    Game.TimeScale = 1.0f;
        //}
        //GameFiber.Sleep(500);

        //if (Game.TimeScale >= 0.4f)
        //{
        //    Game.TimeScale = 0.3f;
        //}
        //else if (Game.TimeScale >= 0.1f)
        //{
        //    Game.TimeScale -= 0.05f;
        //}
        //GameFiber.Sleep(500);


        //while (!Game.IsKeyDownRightNow(Keys.Space))
        //{
        //    if (Player.StreetPlacePoliceShouldSearchForPlayer != Vector3.Zero)
        //    {
        //        Rage.Debug.DrawArrowDebug(Player.StreetPlacePoliceShouldSearchForPlayer, Vector3.Zero, Rotator.Zero, 1f, Color.Blue);
        //    }
        //    if (Player.PlacePoliceShouldSearchForPlayer != Vector3.Zero)
        //    {
        //        Rage.Debug.DrawArrowDebug(Player.StreetPlacePoliceShouldSearchForPlayer, Vector3.Zero, Rotator.Zero, 1f, Color.Purple);
        //    }
        //    if (Player.StreetPlacePoliceLastSeenPlayer != Vector3.Zero)
        //    {
        //        Rage.Debug.DrawArrowDebug(Player.StreetPlacePoliceShouldSearchForPlayer, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
        //    }
        //    if (Player.PlacePoliceLastSeenPlayer != Vector3.Zero)
        //    {
        //        Rage.Debug.DrawArrowDebug(Player.StreetPlacePoliceShouldSearchForPlayer, Vector3.Zero, Rotator.Zero, 1f, Color.Orange);
        //    }
        //    Game.DisplaySubtitle($"PRESS SPACE TO CANCEL");
        //    GameFiber.Yield();
        //}





        //World.Vehicles.StopAllTrains();

        //Player.PoliceResponse.TrainStopper.StopAllTrains();
        //Vector3 Position = new Vector3(833.577f, -1258.954f, 26.34347f);
        //bool isObscured = false;
        //while (!Game.IsKeyDownRightNow(Keys.Space))
        //{
        //    Rage.Debug.DrawArrowDebug(Position, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
        //    isObscured = NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(Position.X, Position.Y, Position.Z, Settings.SettingsManager.DebugSettings.ObscuredX, Settings.SettingsManager.DebugSettings.ObscuredY, Settings.SettingsManager.DebugSettings.ObscuredZ, 0);// 0.5f, 2f, 1f, 0))//NativeFunction.Natives.IS_POSITION_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 0.1f, false, true, false, false, false, false, false))
        //    Game.DisplayHelp($"Press SPACE to Stop");

        //    float DistancetoPos = Game.LocalPlayer.Character.Position.DistanceTo(Position);
        //    Game.DisplaySubtitle($"isObscured: {isObscured} DistancetoPos {Math.Round(DistancetoPos,2)}");
        //    GameFiber.Yield();
        //}



        //

        //if (int.TryParse(NativeHelper.GetKeyboardInput("4"), out int newPlateStyleIndex))
        //{
        //    var MyPtr = Game.GetScriptGlobalVariableAddress(newPlateStyleIndex); //the script id for respawn_controller
        //    Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
        //    Game.TerminateAllScriptsWithName("respawn_controller");
        //    Game.DisplaySubtitle($"SET {newPlateStyleIndex}");
        //}

        //Camera StoreCam = Camera.RenderingCamera;

        //if (StoreCam.Exists())
        //{
        //    EntryPoint.WriteToConsole($"RENDERING CAMERA EXISTS {StoreCam.Position}");
        //}




        //int interiorID = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(347.2686f, -999.2955f, -99.19622f);

        //if (interiorID != 0)
        //{
        //    NativeFunction.Natives.DISABLE_INTERIOR(interiorID, false);
        //    NativeFunction.Natives.REQUEST_IPL("Medium End Apartment");
        //    NativeFunction.Natives.SET_INTERIOR_ACTIVE(interiorID, true);


        //    Game.DisplaySubtitle($"SET ACTIVE interiorID {interiorID}");
        //    GameFiber.Sleep(500);
        ////}
        //while (!Game.IsKeyDown(Keys.W))
        //{
        //    if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
        //    {
        //        NativeFunction.Natives.SET_TAXI_LIGHTS(Game.LocalPlayer.Character.CurrentVehicle, true);
        //    }
        //    GameFiber.Yield();
        //}
        //Game.DisplayHelp("DONE");



        //  string PlayingDict = "timetable@trevor@on_the_toilet";
        //  string PlayingAnim = "trevonlav_baseloop";


        //  AnimationDictionary.RequestAnimationDictionay(PlayingDict);


        //  Vector3 Position = Game.LocalPlayer.Character.Position;
        //  float Heading = Game.LocalPlayer.Character.Heading;


        ////  Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(Settings.SettingsManager.DebugSettings.SynchedSceneOffsetX, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetY, Settings.SettingsManager.DebugSettings.SynchedSceneOffsetZ)); //new Vector3(Position.X, Position.Y, Position.Z);

        //  int PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", Position.X, Position.Y, Game.LocalPlayer.Character.Position.Z + Settings.SettingsManager.DebugSettings.SynchedSceneOffsetZ, 0.0f, 0.0f, Heading, 2);//270f //old
        //  NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
        //  NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, PlayingDict, PlayingAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
        //  NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
        //  while(!Game.IsKeyDown(Keys.W))
        //  {
        //      GameFiber.Yield();
        //  }
        //  Game.LocalPlayer.Character.Tasks.Clear();
        //ShowBodyLoadPosition();
        //ShowCarChangePosition();

        //if(Player.InterestedVehicle == null)
        //{
        //    return;
        //}
        //GetLicensePlateChangePosition(Player.InterestedVehicle.Vehicle);

        //Vehicle SpawnedVehicle = null;
        //Vector3 Position = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        //try
        //{
        //    SpawnedVehicle = SpawnedVehicle = new Vehicle("taxi", Position, 0f);//NativeFunction.Natives.CREATE_VEHICLE<Vehicle>(Game.GetHashKey("taxi"), Position.X, Position.Y, Position.Z,0f, false, false, false);//   new Vehicle("taxi", Position, 0f) { IsPersistent = false };
        //    if (!SpawnedVehicle.Exists())
        //    {
        //        Game.DisplaySubtitle($"SPAWNED VEHICLE DOESNT EXIST? {Game.GameTime}");
        //        return;
        //    }
        //    GameFiber.Sleep(2000);
        //    if (SpawnedVehicle.Exists())
        //    {
        //        SpawnedVehicle.Delete();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Game.DisplaySubtitle($"SPAWN ERROR {Game.GameTime}");
        //    EntryPoint.WriteToConsole($"DebugNumpad7: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING TAXI", 0);
        //    if (SpawnedVehicle.Exists())
        //    {
        //        EntryPoint.WriteToConsole($"DebugNumpad7: ERROR DELETED VEHICLE ACTUALLY STILL GOT THE HANDLE", 0);
        //        SpawnedVehicle.Delete();
        //    }
        //    EntryPoint.ModController.AddSpawnError(new SpawnError(Game.GetHashKey("taxi"), Position, Game.GameTime));
        //    GameFiber.Yield();
        //    return;
        //}








        //Game.DisplaySubtitle(Player.GPSManager.GetGPSRoutePosition().ToString());
        // NativeFunction.Natives.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE(4163212883, -355.3892f, -51.06768f, 49.31105f, false,1.0f,true);



        //Vector3 source = Game.LocalPlayer.Character.Position;
        //Vector3 target = new Vector3(source.X, source.Y, source.Z + 1.0f);
        //if (!NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(source.X, source.Y, source.Z, out float GroundZ, true, false))
        //{
        //    return;
        //    //position = new Vector3(position.X, position.Y, GroundZ);
        //}
        //target = new Vector3(source.X, source.Y, GroundZ - 1.0f);
        //int ShapeTestResultID = NativeFunction.Natives.START_SHAPE_TEST_CAPSULE<int>(source.X, source.Y, source.Z, target.X, target.Y, target.Z, 1.0f, 1, Game.LocalPlayer.Character, 7);
        //if(ShapeTestResultID == 0)
        //{
        //    return;
        //}
        //Vector3 hitPositionArg;
        //bool hitSomethingArg;
        //int materialHashArg;
        //int entityHandleArg;
        //Vector3 surfaceNormalArg;
        //int Result = 0;
        //unsafe
        //{
        //    Result = NativeFunction.CallByName<int>("GET_SHAPE_TEST_RESULT_INCLUDING_MATERIAL", ShapeTestResultID, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &materialHashArg, &entityHandleArg);
        //}
        //if(Result == 0)
        //{ 
        //    return;
        //}

        //bool DidHit = hitSomethingArg;
        //Vector3 HitPosition = hitPositionArg;
        //Vector3 SurfaceNormal = surfaceNormalArg;
        //int MaterialHash = materialHashArg;
        //Game.DisplaySubtitle($"{DidHit} {HitPosition} {SurfaceNormal} {(MaterialHash)MaterialHash}");
        //EntryPoint.WriteToConsole($"{DidHit} {HitPosition} {SurfaceNormal} {MaterialHash} {(MaterialHash)MaterialHash}");










        //GET_CLOSEST_OBJECT_OF_TYPE

        //prop_cctv_pole_04, X: 411.6299 Y: -1619.302 Z: 28.30813,, 574160586

        //Vector3 Coordinates = Vector3.Zero;


        //if(DoOne)
        //{
        //    Coordinates = new Vector3(411.6299f, -1619.302f, 28.30813f);
        //}
        //else
        //{
        //    Coordinates = new Vector3(409.8848f, -1660.358f, 28.25814f);
        //}






        //Rage.Object myObject = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Coordinates.X, Coordinates.Y, Coordinates.Z,10f,Game.GetHashKey("prop_cctv_pole_04"),false,false,false);

        //if(myObject.Exists())
        //{
        //    Game.DisplaySubtitle($"{(DoOne ? 1 : 2)}  {myObject.Handle} {myObject.Health} {myObject.MaxHealth}");
        //}

        //DoOne = !DoOne;

        //PedExt closestPed = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //if(closestPed == null || !closestPed.Pedestrian.Exists())
        //{
        //    EntryPoint.WriteToConsole("ERROR DEBUG7d");
        //    return;
        //}

        ////DEFAULT,BASE,COP,EMPTY,GANG,FAMILY,PLAYER,Security, MEDIC,FIREMAN
        //string descisionMakerName = NativeHelper.GetKeyboardInput("DEFAULT");
        //if(!string.IsNullOrEmpty(descisionMakerName))
        //{
        //    NativeFunction.Natives.SET_DECISION_MAKER(closestPed.Pedestrian, Game.GetHashKey(descisionMakerName));
        //    Game.DisplaySubtitle($"SET_DECISION_MAKER {descisionMakerName}");
        //}






        //unsafe
        //{
        //    ANIM_DATA aNIM_DATA = new ANIM_DATA();
        //    aNIM_DATA.type = 0;
        //    aNIM_DATA.dictionary0 = "random@arrests";
        //    aNIM_DATA.anim0 = "radio_chatter";
        //    aNIM_DATA.phase0 = 0.0f;
        //    aNIM_DATA.weight0 = 1.0f;


        //    aNIM_DATA.dictionary1 = "";
        //    aNIM_DATA.anim1 = "";
        //    aNIM_DATA.phase1 = 0.0f;
        //    aNIM_DATA.weight1 = 0.0f;

        //    aNIM_DATA.dictionary2 = "";
        //    aNIM_DATA.anim2 = "";
        //    aNIM_DATA.phase2 = 0.0f;
        //    aNIM_DATA.weight2 = 0.0f;

        //    aNIM_DATA.filter = (int)Game.GetHashKey("BONEMASK_ARMONLY_L");
        //    aNIM_DATA.blendInDelta = 8.0f;
        //    aNIM_DATA.blendOutDelta = -8.0f;
        //    aNIM_DATA.timeToPlay = -1;
        //    aNIM_DATA.flags = 16 | 32;
        //    aNIM_DATA.ikFlags = 0;

        //    IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(aNIM_DATA)); //Marshal.AllocHGlobal(1024);
        //    Marshal.StructureToPtr(aNIM_DATA, intPtr, true);
        //    long* f = (long*)intPtr.ToInt64();
        //    NativeFunction.CallByName<bool>("TASK_SCRIPTED_ANIMATION", closestCop.Pedestrian, &f, null, null, 8.0f, -8.0f);
        //}

        //unsafe
        //{
        //    int lol = 0;
        //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
        //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_enter", 2.0f, -2.0f, 1000, 16 | 32, 0, false, "BONEMASK_ARMONLY_L", false);
        //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_chatter", 2.0f, -2.0f, 2000, 16 | 32, 0, false, "BONEMASK_ARMONLY_L", false);
        //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_exit", 2.0f, -2.0f, 1000, 16 | 32, 0, false, "BONEMASK_ARMONLY_L", false);
        //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
        //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
        //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", closestCop.Pedestrian, lol);
        //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        //}


        //TunOffInterior();

        //ResetCops();

        //if (!Test)
        //{
        //    //NativeFunction.Natives.ADD_PED_DECORATION(Game.LocalPlayer.Character, "multiplayer_overlays", "FM_Tat_F_003");
        //    NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(Game.LocalPlayer.Character, Game.GetHashKey("multiplayer_overlays"), Game.GetHashKey("FM_Tat_F_003"));
        //    Game.DisplaySubtitle("added DECOR");
        //}
        //else
        //{
        //    NativeFunction.Natives.CLEAR_PED_DECORATIONS(Game.LocalPlayer.Character);
        //    Game.DisplaySubtitle("removed DECOR");
        //}
        //Test = !Test;
        //DoUiCustomzierFont();
        //ParticleTest1();
        //string AudioFilePath = Ringtones.STTHOMAS.FileName;// string.Format("Plugins\\LosSantosRED\\audio\\{0}", "gta4_cellphone\\STTHOMAS.wav");
        //NAudioPlayer nAudio = new NAudioPlayer(Settings);
        //nAudio.Play(AudioFilePath, false, false);
        ////while (nAudio.IsAudioPlaying)
        ////{
        ////    GameFiber.Yield();
        ////}
        //Game.DisplaySubtitle("Audio Finished");

    }



    public void DebugNumpad8()
{


        if (!Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips)
        {
            Settings.SettingsManager.GangSettings.ShowSpawnedBlip = true;
            Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = true;
            Settings.SettingsManager.EMSSettings.ShowSpawnedBlips = true;
            Settings.SettingsManager.FireSettings.ShowSpawnedBlips = true;
            Settings.SettingsManager.TaxiSettings.ShowSpawnedBlip = true;
            Settings.SettingsManager.SecuritySettings.ShowSpawnedBlips = true;
        }
        else
        {
            Settings.SettingsManager.GangSettings.ShowSpawnedBlip = false;
            Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = false;
            Settings.SettingsManager.EMSSettings.ShowSpawnedBlips = false;
            Settings.SettingsManager.FireSettings.ShowSpawnedBlips = false;
            Settings.SettingsManager.TaxiSettings.ShowSpawnedBlip = false;
            Settings.SettingsManager.SecuritySettings.ShowSpawnedBlips = false;
        }
        Game.DisplaySubtitle($"Toggled Blips Enabled:{Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips}");
        GameFiber.Sleep(500);


        // if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        // {
        //     EntryPoint.WriteToConsole($"modelName: {Player.CurrentVehicle.Vehicle.Model.Name.ToLower()} modelHAsh{Player.CurrentVehicle.Vehicle.Model.Hash} modSpawned{Player.CurrentVehicle.WasModSpawned} gang{Player.CurrentVehicle.AssociatedGang?.ID}");
        // }
        // EntryPoint.WriteToConsole($"CustomPhoneOS:{Player.CellPhone.CustomPhoneOS} CustomPhoneType:{Player.CellPhone.CustomPhoneType} PhoneOS{Player.CellPhone.PhoneOS} PhoneType{Player.CellPhone.PhoneType}");



        //foreach(ContactRelationship cr in Player.RelationshipManager.ContactRelationships)
        // {
        //     EntryPoint.WriteToConsole($"{cr.ContactName} {cr.ReputationLevel} {cr.TotalMoneySpent} {cr.Stuff}");
        // }

        //foreach(GangDen gangDen in ModDataFileManager.PlacesOfInterest.PossibleLocations.GangDens)
        // {
        //     EntryPoint.WriteToConsole($" {gangDen.AssociatedGang?.ShortName} hasBLip = {gangDen.Blip.Exists()} hasTerrBlip = {gangDen.TerritoryBlip.Exists()}");
        // }

        // string dictionaryName = NativeHelper.GetKeyboardInput("veh@std@ds@base");
        // string animName = NativeHelper.GetKeyboardInput("change_station");
        // Player.ActivityManager.DebugPlayVehicleAnim(dictionaryName, animName);


        //GameFiber.StartNew(delegate
        //{
        //    while (!Game.IsKeyDownRightNow(Keys.Z))
        //    {
        //        if(Game.IsControlJustReleased(0, GameControl.Attack))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.Attack:{Game.IsControlJustReleased(0, GameControl.Attack)}");
        //        }
        //        if(NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.Attack (Disabled):{NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 24)}");
        //        }
        //        if(Game.IsControlJustReleased(0, GameControl.VehicleAttack))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleAttack:{Game.IsControlJustReleased(0, GameControl.VehicleAttack)}");
        //        }
        //        if(NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleAttack (Disabled):{NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 69)}");
        //        }


        //        if (Game.IsControlJustReleased(0, GameControl.VehicleFlyMouseControlOverride))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleFlyMouseControlOverride:{Game.IsControlJustReleased(0, GameControl.VehicleFlyMouseControlOverride)}");
        //        }
        //        if (NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 122))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleFlyMouseControlOverride (Disabled):{NativeFunction.Natives.x305C8DCD79DA8B0F<bool>(0, 122)}");
        //        }



        //        if (Game.IsControlJustPressed(0, GameControl.Aim))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.Aim:{Game.IsControlJustPressed(0, GameControl.Aim)}");
        //        }
        //        if(NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.Aim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25)}");
        //        }
        //        if(Game.IsControlJustPressed(0, GameControl.VehicleAim))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleAim:{Game.IsControlJustPressed(0, GameControl.VehicleAim)}");
        //        }
        //        if(NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68))
        //        {
        //            EntryPoint.WriteToConsole($"GameControl.VehicleAim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68)}");
        //        }



        //        Game.DisplayHelp($"Press Z to Exit");
        //        GameFiber.Yield();
        //    }


        //}, "Run Debug Logic");
        //GameFiber.Sleep(1000);


        //EntryPoint.WriteToConsole($"GameControl.Attack:{Game.IsControlJustPressed(0, GameControl.Attack)}");
        //EntryPoint.WriteToConsole($"GameControl.Attack (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 24)}");
        //EntryPoint.WriteToConsole($"GameControl.VehicleAttack:{Game.IsControlJustPressed(0, GameControl.VehicleAttack)}");
        //EntryPoint.WriteToConsole($"GameControl.VehicleAttack (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 69)}");


        //EntryPoint.WriteToConsole($"GameControl.Aim:{Game.IsControlJustPressed(0, GameControl.Aim)}");
        //EntryPoint.WriteToConsole($"GameControl.Aim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25)}");
        //EntryPoint.WriteToConsole($"GameControl.VehicleAim:{Game.IsControlJustPressed(0, GameControl.VehicleAim)}");
        //EntryPoint.WriteToConsole($"GameControl.VehicleAim (Disabled):{NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 68)}");



        //NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25)

        //&&  && Player.CurrentVehicle.Vehicle.Model.Name.ToLower() == VehicleToSteal.ModelName.ToLower() && Player.CurrentVehicle.WasModSpawned && Player.CurrentVehicle.AssociatedGang != null && Player.CurrentVehicle.AssociatedGang.ID == TargetGang.ID;



        //Rage.Object doorEntity = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(145.2892f, -1041.0303f, 29.3679f, 3.0f, 4163212883, true, false, true);
        //if(!doorEntity.Exists())
        //{
        //    return;
        //}
        //NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity,false);

        //int x = 0;
        //while (x < 200)
        //{
        //    if(doorEntity.Exists())
        //    {
        //        doorEntity.Rotation = new Rotator(0f, 0f, doorEntity.Rotation.Yaw - 0.5f);
        //    }
        //    x++;
        //    GameFiber.Yield();
        //}

        //GameFiber.Sleep(5000);
        //if (!doorEntity.Exists())
        //{
        //    return;
        //}
        //NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, true);
        //NativeFunction.Natives.x9B12F9A24FABEDB0(4163212883, 145.2892f, -1041.0303f, 29.3679f, false, 1.0f);






        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("City_Banks", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Countryside_Banks", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("AMMUNATION", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("YellowJackInn", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VANGELICO", true);
        //GameFiber.StartNew(delegate
        //{
        //    Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        //    while (!Game.IsKeyDownRightNow(Keys.Z))
        //    {
        //        //Rage.Debug.DrawArrowDebug(PlayerPos, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
        //        //bool isExplosion = NativeFunction.Natives.IS_EXPLOSION_IN_SPHERE<bool>(-1, PlayerPos.X,PlayerPos.Y,PlayerPos.Z,20f);
        //        bool IsCurrentWeaponSilenced = Player.Character.IsCurrentWeaponSilenced;
        //        bool IS_PED_CURRENT_WEAPON_SILENCED = NativeFunction.Natives.IS_PED_CURRENT_WEAPON_SILENCED<bool>(Game.LocalPlayer.Character);
        //        Game.DisplaySubtitle($"IsCurrentWeaponSilenced:{IsCurrentWeaponSilenced} IS_PED_CURRENT_WEAPON_SILENCED:{IS_PED_CURRENT_WEAPON_SILENCED}");
        //        Game.DisplayHelp($"Press Z to Exit");
        //        GameFiber.Yield();
        //    }


        //}, "Run Debug Logic");

        //GameFiber.StartNew(delegate
        //{
        //    float CamPitch = 0f;
        //    float VehiclePitch = 0f;
        //    while (!Game.IsKeyDownRightNow(Keys.Z))
        //    {
        //        CamPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        //        if (Player.Character.CurrentVehicle.Exists())
        //        {
        //            VehiclePitch = Player.Character.CurrentVehicle.Rotation.Pitch;
        //        }
        //        else
        //        {
        //            VehiclePitch = 0f;
        //        }
        //        Game.DisplaySubtitle($"CamPitch:{Math.Round(CamPitch,4)} VehiclePitch:{Math.Round(VehiclePitch, 4)}");
        //        Game.DisplayHelp($"Press Z to Exit");
        //        GameFiber.Yield();
        //    }


        //}, "Run Debug Logic");


        // IS_EXPLOSION_IN_AREA

        //DoCops();
        // Test222();
        //SpawnLocation taxiSpawn = new SpawnLocation(Game.LocalPlayer.Character.Position);
        //taxiSpawn.GetClosestStreet(true);
        //DispatchableVehicle taxiVehicle = new DispatchableVehicle("taxi", 100, 100);
        //DispatchablePerson taxiPed = new DispatchablePerson("a_m_m_socenlat_01", 100, 100);
        //if (taxiSpawn.StreetPosition != null)
        //{
        //    CivilianSpawnTask civilianSpawnTask = new CivilianSpawnTask(taxiSpawn, taxiVehicle, taxiPed, false, false, true, Settings, Crimes, Weapons, ModDataFileManager.Names, World);
        //    civilianSpawnTask.AllowAnySpawn = true;
        //    civilianSpawnTask.AllowBuddySpawn = false;
        //    civilianSpawnTask.AttemptSpawn();
        //    civilianSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
        //    civilianSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.None));
        //    PedExt taxiDriver = civilianSpawnTask.CreatedPeople.FirstOrDefault();
        //    if (taxiDriver != null && taxiDriver.Pedestrian.Exists() && taxiDriver.Pedestrian.CurrentVehicle.Exists())
        //    {
        //        taxiDriver.CanBeTasked = true;
        //        taxiDriver.CanBeAmbientTasked = true;
        //        NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(taxiDriver.Pedestrian, taxiDriver.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);
        //    }
        //}


        // WeaponTest1();

        //try
        //{
        //    string pickupName = "PICKUP_WEAPON_PISTOL";



        //    List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList();
        //    foreach (Rage.Object obj in Objects)
        //    {
        //        if (obj.Exists() && obj.DistanceTo2D(Game.LocalPlayer.Character) <= 10f)
        //        {

        //            bool isPickup = false;// NativeFunction.Natives.xFC481C641EBBD27D<bool>(obj);

        //            EntryPoint.WriteToConsole($"{obj.Model.Name} {obj.Model.Hash} isPickup {isPickup}");



        //        }
        //    }

        //    //W_PI_PISTOL
        //    NativeFunction.Natives.SET_LOCAL_PLAYER_CAN_COLLECT_PORTABLE_PICKUPS(false);

        //    //NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(453432689, false);
        //    GameFiber.StartNew(delegate
        //    {
        //        while (!Game.IsKeyDownRightNow(Keys.P))
        //        {
        //            NativeFunction.Natives.SET_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_OF_TYPE(Game.LocalPlayer, 4189041807, false);

        //            NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(1467525553, false);
        //            // NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(2395771146, false);


        //            bool hasStuff = false;
        //            if (NativeFunction.Natives.DOES_PICKUP_OF_TYPE_EXIST_IN_AREA<bool>(4189041807, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 10f))
        //            {
        //                hasStuff = true;
        //            }


        //            Game.DisplayHelp($"Press P to Stop Does Pickup Exist {hasStuff}");

        //            GameFiber.Yield();
        //        }

        //        NativeFunction.Natives.SET_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_OF_TYPE(Game.LocalPlayer, 4189041807, true);
        //        NativeFunction.Natives.SET_LOCAL_PLAYER_CAN_COLLECT_PORTABLE_PICKUPS(true);
        //        //NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(453432689, true);

        //        NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(1467525553, true);
        //        // NativeFunction.Natives.SET_LOCAL_PLAYER_PERMITTED_TO_COLLECT_PICKUPS_WITH_MODEL(2395771146, true);

        //    }, "Run Debug Logic");





        //}
        //catch (Exception ex)
        //{
        //    Game.DisplayNotification("Shit CRASHES!!!");
        //    EntryPoint.WriteToConsole($"{ex.Message} {ex.StackTrace}");
        //}

        //PICKUP_WEAPON_PISTOL
        //try
        //{


        //            GameFiber.StartNew(delegate
        //            {
        //                while (!Game.IsKeyDownRightNow(Keys.P))
        //                {
        //                    bool highway = NativeFunction.Natives.GET_IS_PLAYER_DRIVING_ON_HIGHWAY<bool>(Game.LocalPlayer);
        //                    bool wreckless = NativeFunction.Natives.GET_IS_PLAYER_DRIVING_WRECKLESS<bool>(Game.LocalPlayer,2);
        //                    //Game.DisplayHelp($"Press P to Stop~n~FW: {rn.ForwardLanes} BW: {rn.BackwardsLanes} ~n~WIDTH: {rn.Width} POS: {rn.RoadPosition}");


        //                        Game.DisplayHelp($"Press P to Stop~n~Hi:{highway}~n~Wr:{wreckless}");

        //                    GameFiber.Yield();
        //                }
        //            }, "Run Debug Logic");





        //}
        //catch (Exception ex)
        //{
        //    Game.DisplayNotification("Shit CRASHES!!!");
        //}




        //try
        //{

        //    Vector3 position = Game.LocalPlayer.Character.Position;//Game.LocalPlayer.Character.Position;
        //    Vector3 outPos;
        //    float outHeading;


        //    if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(position.X, position.Y, position.Z, out outPos, out outHeading, 1, 3.0f, 0))
        //    {
        //        RoadNode rn = new RoadNode(outPos, outHeading);
        //        rn.MajorRoadsOnly = true;
        //        rn.GetRodeNodeProperties();
        //        if (rn.HasRoad)
        //        {
        //            EntryPoint.WriteToConsole($"Road Node Properties {rn.Position} {rn.Heading} FW: {rn.ForwardLanes} BW: {rn.BackwardsLanes} WIDTH: {rn.Width} POS: {rn.RoadPosition}");
        //            GameFiber.StartNew(delegate
        //            {
        //                while (!Game.IsKeyDownRightNow(Keys.P))
        //                {
        //                    Rage.Debug.DrawArrowDebug(outPos + new Vector3(0f, 0f, 0.5f), new Vector3(rn.Heading,0f,0f), Rotator.Zero, 1f, Color.White);
        //                    Game.DisplayHelp($"Press P to Stop~n~FW: {rn.ForwardLanes} BW: {rn.BackwardsLanes} ~n~WIDTH: {rn.Width} POS: {rn.RoadPosition}");
        //                    GameFiber.Yield();
        //                }
        //            }, "Run Debug Logic");
        //        }
        //        else
        //        {
        //            Game.DisplayHelp($"No Road FOund");
        //        }

        //    }



        //}
        //catch (Exception ex)
        //{
        //    Game.DisplayNotification("Shit CRASHES!!!");
        //}









        //string AudioFilePath = Ringtones.STTHOMAS.FileName;// string.Format("Plugins\\LosSantosRED\\audio\\{0}", "gta4_cellphone\\STTHOMAS.wav");
        //NAudioPlayer nAudio = new NAudioPlayer(Settings);
        //nAudio.Play(AudioFilePath, false, false);

        //string AudioFilePath = Ringtones.STTHOMAS.FileName;//string.Format("Plugins\\LosSantosRED\\audio\\{0}", "gta4_cellphone\\STTHOMAS.wav");
        //NAudioPlayer nAudio = new NAudioPlayer(Settings);
        //nAudio.Play(AudioFilePath, false, false);
        //while(nAudio.IsAudioPlaying)
        //{
        //    GameFiber.Yield();
        //}
        //Game.DisplaySubtitle("Audio Finished");





    }
    private void DebugNumpad9()
{
World.Pedestrians.ClearSpawned();
World.Vehicles.ClearSpawned(true);


if (Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching)
{
    Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching = false;
    Settings.SettingsManager.EMSSettings.ManageDispatching = false;
    Settings.SettingsManager.GangSettings.ManageDispatching = false;
    Settings.SettingsManager.FireSettings.ManageDispatching = false;
    Settings.SettingsManager.SecuritySettings.ManageDispatching = false;
    Settings.SettingsManager.TaxiSettings.ManageDispatching = false;
    Settings.SettingsManager.CivilianSettings.ManageDispatching = false;
    Game.DisplaySubtitle("Dispatching Disabled");
}
else
{
    Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching = true;
    Settings.SettingsManager.EMSSettings.ManageDispatching = true;
    Settings.SettingsManager.GangSettings.ManageDispatching = true;
    Settings.SettingsManager.FireSettings.ManageDispatching = true;
    Settings.SettingsManager.SecuritySettings.ManageDispatching = true;
    Settings.SettingsManager.TaxiSettings.ManageDispatching = true;
    Settings.SettingsManager.CivilianSettings.ManageDispatching = true;
    Dispatcher.DebugResetLocations();
    Game.DisplaySubtitle("Dispatching Enabled");
}



//Cop cop = World.Pedestrians.PoliceList.Where(x=> x.DistanceToPlayer <= 25f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
//if(cop == null || !cop.Pedestrian.Exists())
//{
//    return;
//}
//cop.Pedestrian.BlockPermanentEvents = true;
//cop.Pedestrian.KeepTasks = true;
//NativeFunction.Natives.CLEAR_PED_TASKS(cop.Pedestrian);
//NativeFunction.Natives.TASK_GOTO_ENTITY_OFFSET(cop.Pedestrian, Player.Character, -1, 1.0f, -180f, 1.0f, 0);


//NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD_ADVANCED(cop.Pedestrian,1.0f,-1,0.25f,4 | 16);
    //PED_INDEX PedIndex, VECTOR VecCoors, FLOAT MoveBlendRatio, INT Time , FLOAT Radius, ENAV_SCRIPT_FLAGS iNavFlags, NAVDATA navDataStruct, FLOAT FinalHeading = DEFAULT_NAVMESH_FINAL_HEADING ) = "0x72f317bc03266125"

GameFiber.Sleep(1000);
//OpenDoors();
//foreach (ModItem modItem in ModItems.AllItems())
//{
//    if (modItem.Name == "Marijuana" || modItem.Name == "Cocaine" || modItem.Name == "Heroin" || modItem.Name == "Methamphetamine" || modItem.Name == "Crack" || modItem.Name == "SPANK" || modItem.Name == "Toilet Cleaner")
//    {
//        EntryPoint.WriteToConsole($"ITEM {modItem.Name} CHECKING");
//        List<Zone> foundDealer = Zones.GetZoneByItem(modItem, ShopMenus, true);
//        List<Zone> foundCustomer = Zones.GetZoneByItem(modItem, ShopMenus, false);

//        if(foundDealer != null)
//        {
//            EntryPoint.WriteToConsole($"ITEM {modItem.Name} DEALER AREA(S) FOUND {string.Join(",", foundDealer.Select(x=>x.DisplayName))}");
//        }
//        if (foundCustomer != null)
//        {
//            EntryPoint.WriteToConsole($"ITEM {modItem.Name} CUSTOMER AREA(S) FOUND {string.Join(",", foundCustomer.Select(x => x.DisplayName))}");
//        }
//    }
//}
//CarChanePos();
// SpawnAttachedRagdoll();

// EntryPoint.WriteToConsole($"HandsAreUp: {Player.Surrendering.HandsAreUp}");

//Player.CellPhone.CloseBurner();

//PrintRelationships();
//AlertMessage();
//SetPropAttachment();
//DisplaySprite();
//DisableAllSpawning();
//Player.CellPhone.AddScamText();
}


    private void DoCops()
    {
    Vector3 PlaceToDriveTo = Game.LocalPlayer.Character.Position;
    foreach (Cop cop in World.Pedestrians.Police)
    {
        if(cop.Pedestrian.Exists() && cop.Pedestrian.CurrentVehicle.Exists())
        {
            cop.Pedestrian.BlockPermanentEvents = true;
            cop.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(cop.Pedestrian, cop.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 70f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
            EntryPoint.WriteToConsole($"COP {cop.Handle} SET TO DRIVE TO PLAYER");
        }
    }
    }


    private void ResetCops()
    {
    foreach (Cop cop in World.Pedestrians.Police)
    {
        if (cop.Pedestrian.Exists())
        {
            cop.ClearTasks(true);
            EntryPoint.WriteToConsole($"COP {cop.Handle} CLEARED");
        }
    }
    }
    private void PhoneTest()
    {

    GameFiber.StartNew(delegate
    {
        Ped coolPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f), 0f);
        GameFiber.Yield();
        if (coolPed.Exists())
        {
            coolPed.BlockPermanentEvents = true;
            coolPed.KeepTasks = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 50f, 10000, true,true);//100f
                NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 1500f, -1, true, true);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", coolPed, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        Game.DisplayHelp("PRESS Z TO CANCEL");
        while (coolPed.Exists() && !Game.IsKeyDownRightNow(Keys.Z) && ModController.IsRunning)
        {
            GameFiber.Sleep(25);
        }
        if (coolPed.Exists())
        {
            coolPed.Delete();
        }

    }, "Run Debug Logic");







    }
    private void ShockTest()
    {

    GameFiber.StartNew(delegate
    {
        Ped coolPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f), 0f);
        GameFiber.Yield();
        if (coolPed.Exists())
        {
            coolPed.BlockPermanentEvents = true;
            coolPed.KeepTasks = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_SHOCKING_EVENT_BACK_AWAY", 0, 0);//100f
                NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", coolPed, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        Game.DisplayHelp("PRESS Z TO CANCEL");
        while (coolPed.Exists() && !Game.IsKeyDownRightNow(Keys.Z) && ModController.IsRunning)
        {
            GameFiber.Sleep(25);
        }
        if (coolPed.Exists())
        {
            coolPed.Delete();
        }

    }, "Run Debug Logic");







    }
    private void ShuffleTest()
    {
    //spawn car, put me in, put them in passenger, kill them, shuffle seat, see what happens?
    //TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT
    bool isPassenger = RandomItems.RandomPercent(50f);
    int playerseat = isPassenger ? 0 : - 1;
    int pedseat = isPassenger ? -1: 0;

    VehicleExt chosenVehicle = Player.ActivityManager.GetInterestedVehicle();
    if (chosenVehicle == null || !chosenVehicle.Vehicle.Exists())
    {
        return;
    }
    if(!Player.IsInVehicle)
    {
        Player.Character.WarpIntoVehicle(chosenVehicle.Vehicle, playerseat);
    }
    Ped randomPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f));
    GameFiber.Yield();
    if (!randomPed.Exists())
    {
        return;
    }
    randomPed.WarpIntoVehicle(chosenVehicle.Vehicle, pedseat);
    randomPed.Kill();
    GameFiber.Sleep(1000);
    if (chosenVehicle == null || !chosenVehicle.Vehicle.Exists() || !randomPed.Exists())
    {
        return;
    }
    NativeFunction.Natives.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT(Game.LocalPlayer.Character, chosenVehicle.Vehicle, false);
    }
    private void K9Test()
    {
    //spawn K9 give him attack me tasks, set him to not do the pre attack thiungo, wait until press button, despawn

    GameFiber.StartNew(delegate
    {
        Ped attackDog = new Ped("a_c_shepherd",Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f),0f);
        GameFiber.Yield();
        if (attackDog.Exists())
        {
            attackDog.BlockPermanentEvents = true;
            attackDog.KeepTasks = true;
            // NativeFunction.Natives.SET_PED_CONFIG_FLAG(attackDog, 281, false);//Can Writhe
            //NativeFunction.Natives.SET_PED_DIES_WHEN_INJURED(attackDog, false);
            //attackDog.Tasks.FightAgainst(Game.LocalPlayer.Character);
            VehicleExt chosenVehicle = Player.ActivityManager.GetInterestedVehicle();
            if(chosenVehicle != null && chosenVehicle.Vehicle.Exists())
            {
                attackDog.WarpIntoVehicle(chosenVehicle.Vehicle, 1);
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, attackDog.CurrentVehicle, 27, 1000);
                    NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, attackDog.CurrentVehicle, (int)(eEnter_Exit_Vehicle_Flags.ECF_WARP_PED | eEnter_Exit_Vehicle_Flags.ECF_DONT_CLOSE_DOOR));// 256);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", attackDog, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, 134217728, 16);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", attackDog, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
        }
        Game.DisplayHelp("PRESS Z TO CANCEL");
        while (attackDog.Exists() && !Game.IsKeyDownRightNow(Keys.Z) && ModController.IsRunning)
        {
            GameFiber.Sleep(25);
        }
        if (attackDog.Exists())
        {
            attackDog.Delete();
        }

    }, "Run Debug Logic");



    }


    private void OpenDoors()
    {
    Interior int1 = Interiors.GetInteriorByLocalID(-103);
    if(int1 == null)
    {
        return;
    }
    foreach (InteriorDoor door in int1.Doors)
    {
        door.UnLockDoor();
        //NativeFunction.Natives.x9B12F9A24FABEDB0(door.ModelHash, door.Position.X, door.Position.Y, door.Position.Z, false, 0.0f, 50.0f);
        //door.IsLocked = false;
    }
    }
    private void SpawnAttachedRagdoll()
    {
    //GameFiber.StartNew(delegate
    //{
    //    Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(2f).Around2D(2f));
    //    Rage.Object leftHandObject = new Rage.Object("ng_proc_cigarette01a", Game.LocalPlayer.Character.GetOffsetPositionFront(2f).Around2D(2f));
    //    GameFiber.Yield();
    //    if (coolguy.Exists())
    //    {
    //        coolguy.BlockPermanentEvents = true;
    //        coolguy.KeepTasks = true;
    //        coolguy.Kill();
    //        GameFiber.Sleep(500);
    //        if (coolguy.Exists() && leftHandObject.Exists())
    //        {
    //            AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");
    //            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "combat@drag_ped@", "injured_drag_plyr", 2.0f, -2.0f, -1, (int)AnimationFlags.Loop, 0, false, false, false);

    //            NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(coolguy, Player.Character, false);
    //            leftHandObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, "BONETAG_PELVIS"), Vector3.Zero, Rotator.Zero);
    //            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY(coolguy, leftHandObject, 
    //                NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", coolguy, "BONETAG_SPINE3"), //bone 1
    //                NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", coolguy, "BONETAG_SPINE3"),// bone 2
    //                Settings.SettingsManager.DragSettings.Attach1X, Settings.SettingsManager.DragSettings.Attach1Y, Settings.SettingsManager.DragSettings.Attach1Z,
    //                Settings.SettingsManager.DragSettings.Attach2X, Settings.SettingsManager.DragSettings.Attach2Y, Settings.SettingsManager.DragSettings.Attach2Z,
    //                Settings.SettingsManager.DragSettings.Attach3X, Settings.SettingsManager.DragSettings.Attach3Y, Settings.SettingsManager.DragSettings.Attach3Z,
    //                100000.0f,//break force
    //                Settings.SettingsManager.DragSettings.FixedRotation, //fixed rotation
    //                true, //DoInitialWarp
    //                false, //collision
    //                false, //teleport
    //                1 //RotationORder
    //                );

    //            //"BONETAG_SPINE3"
    //            //"BONETAG_PELVIS"
    //            //0.1,0.3,-0.1
    //            //0,0,0
    //            //180.90,0
    //        }
    //    }
    //    while (coolguy.Exists() && leftHandObject.Exists() && !Game.IsKeyDownRightNow(Keys.Q) && ModController.IsRunning)
    //    {
    //        Game.DisplayHelp("Press Q to Stop");
    //        GameFiber.Yield();
    //    }
    //    if (coolguy.Exists())
    //    {
    //        coolguy.Delete();
    //    }
    //    if (leftHandObject.Exists())
    //    {
    //        leftHandObject.Delete();
    //    }
    //}, "Run Debug Logic");
    }


    private void ShowBodyLoadPosition()
    {
        //GameFiber.Sleep(200);
        //VehicleExt TargetVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, 10f);//GetTargetVehicle();
        //if (TargetVehicle != null && TargetVehicle.Vehicle.Exists())//make sure we found a vehicle to change the plates of
        //{
        //    TargetVehicle.OpenDoor(5, true);
        //    Vector3 ChangeSpot = GetBodyLoadPosition(TargetVehicle.Vehicle);
        //    GameFiber.StartNew(delegate
        //    {
        //        while (!Game.IsKeyDownRightNow(Keys.Space))
        //        {

        //            if (ChangeSpot != Vector3.Zero)
        //            {
        //                Rage.Debug.DrawArrowDebug(ChangeSpot, Vector3.Zero, Rotator.Zero, 1f, Color.White);
        //                Rage.Debug.DrawArrowDebug(new Vector3(ChangeSpot.X, ChangeSpot.Y, ChangeSpot.Z + 2.0f), Vector3.Zero, Rotator.Zero, 1f, Color.Red);
        //            }
        //            Game.DisplayHelp($"Press SPACE to Stop");
        //            GameFiber.Yield();
        //        }
        //    }, "Run Debug Logic");
        //}
    }
    //private Vector3 GetBodyLoadPosition(Vehicle VehicleToChange)
    //{
    //    if (!VehicleToChange.Exists())
    //    {
    //        return Vector3.Zero;
    //    }
    //    float halfLength = VehicleToChange.Model.Dimensions.Y / 2.0f;
    //    halfLength += Settings.SettingsManager.DragSettings.LoadBodyYOffset;
    //    //y = -.75
    //    //z = 0.1

    //    Vector3 almostFinal = VehicleToChange.GetOffsetPositionFront(-1.0f * halfLength);

    //    return new Vector3(almostFinal.X + Settings.SettingsManager.DragSettings.LoadBodyXOffset, almostFinal.Y, almostFinal.Z + Settings.SettingsManager.DragSettings.LoadBodyZOffset);
    //}


    private void ShowCarChangePosition()
    {
        GameFiber.Sleep(200);
        VehicleExt TargetVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, 10f);//GetTargetVehicle();
        if (TargetVehicle != null && TargetVehicle.Vehicle.Exists())//make sure we found a vehicle to change the plates of
        {
            Vector3 ChangeSpot = GetLicensePlateChangePosition(TargetVehicle.Vehicle);
            Vector3 NewChangeSpot = GetLicensePlateChangePositionNew(TargetVehicle.Vehicle);
            GameFiber.StartNew(delegate
            {
                while (!Game.IsKeyDownRightNow(Keys.Space))
                {

                    if (ChangeSpot != Vector3.Zero)
                    {
                        Rage.Debug.DrawArrowDebug(ChangeSpot, Vector3.Zero, Rotator.Zero, 1f, Color.White);
                    }
                    if (NewChangeSpot != Vector3.Zero)
                    {
                        Rage.Debug.DrawArrowDebug(NewChangeSpot, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
                    }
                    Game.DisplayHelp($"Press SPACE to Stop");
                    GameFiber.Yield();
                }
            }, "Run Debug Logic");
        }
    }


    private Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    {
        if(!VehicleToChange.Exists())
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: NO VEHICLE");
            return Vector3.Zero;
        }
        Vector3 Position;
        Vector3 Right;
        Vector3 Forward;
        Vector3 Up;
        bool isFrontPlate = false;

        if (VehicleToChange.HasBone("numberplate"))
        {
            float HeadingRotation = -90f;
            EntryPoint.WriteToConsole("PLATE THEFT BONE: numberplate");
            Position = VehicleToChange.GetBonePosition("numberplate");
            if (Position.DistanceTo2D(VehicleToChange.GetOffsetPositionFront(2f)) < Position.DistanceTo2D(VehicleToChange.GetOffsetPositionFront(-2f)))
            {
                EntryPoint.WriteToConsole("PLATE THEFT BONE: numberplate IS FRONT PLATE");
                HeadingRotation = 90f;
            }
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading + HeadingRotation, 1.5f);
            return SpawnPosition;
        }
        else if (VehicleToChange.IsBike)
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: IS BIKE");
            return VehicleToChange.GetOffsetPositionFront(-1.5f);
        }
        else if (VehicleToChange.HasBone("bumper_r"))
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: bumper_r");
            Position = VehicleToChange.GetBonePosition("bumper_r");
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, 1.5f);
            SpawnPosition = NativeHelper.GetOffsetPosition(SpawnPosition, VehicleToChange.Heading, VehicleToChange.Model.Dimensions.X / 2);
            return SpawnPosition;
        }
        else
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: NO BONE FOUND DISABLING");
            return Vector3.Zero;
        }
    }


    private Vector3 GetLicensePlateChangePositionNew(Vehicle VehicleToChange)
    {
        if (!VehicleToChange.Exists())
        {
            return Vector3.Zero;
        }
        float halfLength = VehicleToChange.Model.Dimensions.Y / 2.0f;
        halfLength += 1.0f;
        return VehicleToChange.GetOffsetPositionFront(-1.0f * halfLength);
    }



    //private Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    //{
    //Vector3 Position;
    //Vector3 Right;
    //Vector3 Forward;
    //Vector3 Up;

    //if (VehicleToChange.HasBone("numberplate"))
    //{
    //    //EntryPoint.WriteToConsoleTestLong("PLATE THEFT BONE: numberplate");
    //    Position = VehicleToChange.GetBonePosition("numberplate");


    //    Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, -1 * Settings.SettingsManager.ActivitySettings.PlateTheftFloat);
    //    return SpawnPosition;

    //    VehicleToChange.GetBoneAxes("numberplate", out Right, out Forward, out Up);
    //    return Vector3.Add(Forward * -1.0f * Settings.SettingsManager.ActivitySettings.PlateTheftFloat, Position);
    //}
    //else if (VehicleToChange.HasBone("boot"))
    //{
    //    //EntryPoint.WriteToConsoleTestLong("PLATE THEFT BONE: boot");
    //    Position = VehicleToChange.GetBonePosition("boot");

    //    Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading -90, -1 * Settings.SettingsManager.ActivitySettings.PlateTheftFloat);
    //    return SpawnPosition;

    //    VehicleToChange.GetBoneAxes("boot", out Right, out Forward, out Up);
    //    return Vector3.Add(Forward * -1.75f * Settings.SettingsManager.ActivitySettings.PlateTheftFloat, Position);//return Vector3.Add(Forward * -1.75f, Position);
    //}
    //else if (VehicleToChange.IsBike)
    //{
    //    //EntryPoint.WriteToConsoleTestLong("PLATE THEFT BONE: IS BIKE");
    //    return VehicleToChange.GetOffsetPositionFront(-1.5f);
    //}
    //else if (VehicleToChange.HasBone("bumper_r"))
    //{
    //    //EntryPoint.WriteToConsoleTestLong("PLATE THEFT BONE: bumper_r");
    //    Position = VehicleToChange.GetBonePosition("bumper_r");


    //    Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, -1 * Settings.SettingsManager.ActivitySettings.PlateTheftFloat);
    //    return SpawnPosition;


    //    VehicleToChange.GetBoneAxes("bumper_r", out Right, out Forward, out Up);
    //    Position = Vector3.Add(Forward * -1.0f * Settings.SettingsManager.ActivitySettings.PlateTheftFloat, Position);
    //    return Vector3.Add(Right * 0.25f, Position);
    //}
    //else
    //{
    //    return Vector3.Zero;
    //}
    //}
    private void DoUiCustomzierFont()
{

GameFiber.Sleep(500);
GameFiber.StartNew(delegate
{
    while (!Game.IsKeyDownRightNow(Keys.Space))
    {
        DisplayName();
        Game.DisplayHelp($"Press SPACE to Stop");
        GameFiber.Yield();
    }

}, "Run Debug Logic");


}
private void DisplayName()
{
//AffiliationCenterX = 0.92f;
//AffiliationCenterY = 0.575f;





DisplayTextOnScreen("Franklin Clinton",
    Settings.SettingsManager.PedSwapSettings.NamePositionY,
    Settings.SettingsManager.PedSwapSettings.NamePositionX,
    Settings.SettingsManager.PedSwapSettings.NameScale,
    Color.FromName(Settings.SettingsManager.PedSwapSettings.NameColor),
    (GTAFont)Settings.SettingsManager.PedSwapSettings.NameFont,
    (GTATextJustification)Settings.SettingsManager.PedSwapSettings.NameJustificationID,
    false);

string AffiliationName = "Unaffiliated";

DisplayTextOnScreen(AffiliationName,
    Settings.SettingsManager.PedSwapSettings.AffiliationPositionY,
    Settings.SettingsManager.PedSwapSettings.AffiliationPositionX,
    Settings.SettingsManager.PedSwapSettings.AffiliationScale,
    Color.FromName(Settings.SettingsManager.PedSwapSettings.AffiliationColor),
    (GTAFont)Settings.SettingsManager.PedSwapSettings.AffiliationFont,
    (GTATextJustification)Settings.SettingsManager.PedSwapSettings.AffiliationJustificationID,
    false);
}
private void DisplayTextOnScreen(string TextToShow, float Y, float X, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
{
DisplayTextOnScreen(TextToShow, Y, X, Scale, TextColor, Font, Justification, outline, 255);
}
private void DisplayTextOnScreen(string TextToShow, float Y, float X, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
{
try
{
    if (TextToShow == "" || alpha == 0 || TextToShow is null)
    {
        return;
    }
    NativeFunction.Natives.SET_TEXT_FONT((int)Font);
    NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
    NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

    NativeFunction.Natives.SetTextJustification((int)Justification);

    NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

    if (outline)
    {
        NativeFunction.Natives.SET_TEXT_OUTLINE(true);


        NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
    }
    NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
    //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
    //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
    if (Justification == GTATextJustification.Right)
    {
        NativeFunction.Natives.SET_TEXT_WRAP(0f, X);
    }
    else
    {
        NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
    }
    NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
    //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
    NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
    NativeFunction.Natives.xCD015E5BB0D96A57(X, Y);
}
catch (Exception ex)
{
    EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
}
//return;
}
private void PrintRelationships()
{
foreach (PedExt ped in World.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.WasModSpawned).ThenBy(x => x.DistanceToPlayer))
{
    uint currentWeapon;
    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);


    if (currentWeapon != 0 && currentWeapon != 2725352035)
    {

        uint coolTest = NativeFunction.Natives.GET_CURRENT_PED_WEAPON_ENTITY_INDEX<uint>(ped.Pedestrian, 0);
        bool EntityIndexIsValid = false;
        if (coolTest != 0)
        {
            EntityIndexIsValid = true;
        }

        bool isWeaponReadyToShoot = NativeFunction.Natives.IS_PED_WEAPON_READY_TO_SHOOT<bool>(ped.Pedestrian);

        EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} Weapon {currentWeapon} EntityIndexIsValid {EntityIndexIsValid} isWeaponReadyToShoot {isWeaponReadyToShoot}", 5);
    }
}

bool test = NativeFunction.Natives.IS_VEHICLE_A_CONVERTIBLE<bool>(Game.LocalPlayer.Character.CurrentVehicle, false); Game.DisplaySubtitle(test.ToString());

}

private void DisableAllSpawning()
{
Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching = false;
Settings.SettingsManager.GangSettings.ManageDispatching = false;
Settings.SettingsManager.EMSSettings.ManageDispatching = false;

Settings.SettingsManager.PoliceTaskSettings.ManageTasking = false;
Settings.SettingsManager.GangSettings.ManageTasking = false;
Settings.SettingsManager.EMSSettings.ManageTasking = false;



bool isClearingPeds = false;
GameFiber.Yield();

GameFiber.StartNew(delegate
{
    if (!isClearingPeds)
    {
        isClearingPeds = true;
    }
    float CurrentSpawnMultiplier = 0f;
    while (isClearingPeds && ModController.IsRunning && !Game.IsKeyDownRightNow(Keys.O))
    {

        NativeFunction.Natives.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
        NativeFunction.Natives.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
        NativeFunction.Natives.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
        NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);
        NativeFunction.Natives.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME(CurrentSpawnMultiplier);


        GameFiber.Yield();
    }
}, "Run Debug Logic");
}
private void Something()
{

GameFiber.StartNew(delegate
{
    if (!isClearingPeds)
    {
        isClearingPeds = true;
    }
    float CurrentSpawnMultiplier = 0f;
    while (isClearingPeds && ModController.IsRunning && !Game.IsKeyDownRightNow(Keys.O))
    {

        NativeFunction.Natives.SET_LAW_PEDS_CAN_ATTACK_NON_WANTED_PLAYER_THIS_FRAME(Game.LocalPlayer);


        GameFiber.Yield();
    }
}, "Run Debug Logic");


}
private void DisplaySprite()
{



string dictionaryName = NativeHelper.GetKeyboardInput("shared");
if (dictionaryName != "")
{
    NativeFunction.Natives.REQUEST_STREAMED_TEXTURE_DICT(dictionaryName, false);

    string textureName = NativeHelper.GetKeyboardInput("newstar_32");

    if (textureName != "")
    {
        GameFiber.StartNew(delegate
        {
            uint GameTimeStarted = Game.GameTime;
            while (!NativeFunction.Natives.HAS_STREAMED_TEXTURE_DICT_LOADED<bool>(dictionaryName) || Game.GameTime - GameTimeStarted <= 500)
            {
                GameFiber.Yield();
            }

            if (NativeFunction.Natives.HAS_STREAMED_TEXTURE_DICT_LOADED<bool>(dictionaryName))
            {
                while (Game.GameTime - GameTimeStarted <= 3000)
                {

                    NativeFunction.Natives.DRAW_SPRITE(dictionaryName, textureName, 0.5f, 0.5f, 0.3f, 0.3f, 0f, 255, 255, 255, 255, false, false);
                    NativeFunction.Natives.DRAW_SPRITE(dictionaryName, textureName, 0.7f, 0.7f, 0.3f, 0.3f, 0f, 255, 255, 255, 100, false, false);
                    GameFiber.Yield();
                }
            }

        }, "Run Debug Logic");

        GameFiber.Sleep(1000);//so it doesnt start another
    }

}


}






public enum PathnodeFlags
{
Slow = 1,
Two = 2,
Intersection = 4,
Eight = 8, SlowTraffic = 12, ThirtyTwo = 32, Freeway = 64, FourWayIntersection = 128, BigIntersectionLeft = 512
}
[Flags]
public enum ENodeFlag
{
None = 0,

// 0000 0001
// not sure for what this bit stands 
Unknown = 1,

// 0000 0010
Unused = 2,

// 0000 0100
IsAlley = 4,

// 0000 1000
IsGravelRoad = 8,

// 0001 0000
IsBackroad = 16,

// 0010 0000
IsOnWater = 32,

// 0100 0000
IsPedCrossway = 64,

// 1000 0000
IsJunction = 128,

// 0001 0000 0000
LeftTurnNoReturn = 256,

// 0010 0000 0000
RightTurnNoReturn = 512,

// 0000 0001 0000 0000 0000
IsOffRoad = 4096,

// 0000 0010 0000 0000 0000
NoRightTurn = 8192,

// 0000 0100 0000 0000 0000
NoBigVehicles = 16384,
}



[Flags]
public enum eVehicleNodeProperties
{

VNP_OFF_ROAD				= 1,					// node has been flagged as 'off road', suitable only for 4x4 vehicles, etc
VNP_ON_PLAYERS_ROAD			= 2,					// node has been dynamically marked as somewhere ahead, possibly on (or near to) the player's current road
VNP_NO_BIG_VEHICLES			= 4,					// node has been marked as not suitable for big vehicles
VNP_SWITCHED_OFF			= 8,					// node is switched off for ambient population
VNP_TUNNEL_OR_INTERIOR		= 16,					// node is in a tunnel or an interior
VNP_LEADS_TO_DEAD_END		= 32,					// node is, or leads to, a dead end
VNP_HIGHWAY					= 64,					// node is marked as highway
VNP_JUNCTION				= 128,					// node qualifies as junction
VNP_TRAFFIC_LIGHT			= 256,					// node's special function is traffic-light
VNP_GIVE_WAY				= 512,					// node's special function is give-way	
VNP_WATER					= 1024					// node is water/boat
}


private void SetPropAttachment()
{
string PropName = NativeHelper.GetKeyboardInput("prop_holster_01");
try
{
    Rage.Object SmokedItem = new Rage.Object(Game.GetHashKey(PropName), Player.Character.GetOffsetPositionUp(50f));
    GameFiber.Yield();

    string headBoneName = "BONETAG_HEAD";
    string handRBoneName = "BONETAG_R_PH_HAND";
    string handLBoneName = "BONETAG_L_PH_HAND";

    string boneName = headBoneName;



    string thighRBoneName = "BONETAG_R_THIGH";
    string thighLBoneName = "BONETAG_L_THIGH";
    string pelvisBoneName = "BONETAG_PELVIS";
    string spineRootBoneName = "BONETAG_SPINE_ROOT";
    string spineBoneName = "BONETAG_SPINE";
    string wantedBone = NativeHelper.GetKeyboardInput("Head");
    if (wantedBone == "RHand")
    {
        boneName = handRBoneName;
    }
    else if (wantedBone == "LHand")
    {
        boneName = handLBoneName;
    }
    else if (wantedBone == "LThigh")
    {
        boneName = thighLBoneName;
    }
    else if (wantedBone == "RThigh")
    {
        boneName = thighRBoneName;
    }
    else if (wantedBone == "Pelvis")
    {
        boneName = pelvisBoneName;
    }
    else if (wantedBone == "SpineRoot")
    {
        boneName = spineRootBoneName;
    }
    else if (wantedBone == "Spine")
    {
        boneName = spineBoneName;
    }
    else
    {
        boneName = wantedBone;
    }



    uint GameTimeLastAttached = 0;
    Offset = new Vector3();
    Rotation = new Rotator();
    isPrecise = false;
    if (SmokedItem.Exists())
    {
        //Specific for umbrella

        //AnimationDictionary.RequestAnimationDictionay("doors@");
        // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "doors@", "door_sweep_l_hand_medium", 4.0f, -4.0f, -1, (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1

        //string dictionary = "amb@world_human_binoculars@male@base";
        //string animation = "base";



        string dictionary = NativeHelper.GetKeyboardInput("move_strafe@melee_small_weapon_fps");
        string animation = NativeHelper.GetKeyboardInput("idle");

        AnimationDictionary.RequestAnimationDictionay(dictionary);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1


        isRunning = true;

        AttachItem(SmokedItem, boneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));



        GameFiber.StartNew(delegate
        {
            while (!Game.IsKeyDownRightNow(Keys.Space) && SmokedItem.Exists())
            {
                if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                {
                    AttachItem(SmokedItem, boneName, Offset, Rotation);
                    GameTimeLastAttached = Game.GameTime;
                }


                if (Game.IsKeyDown(Keys.B))
                {
                    //EntryPoint.WriteToConsoleTestLong($"Item {PropName} Attached to  {boneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                    GameFiber.Sleep(500);
                }
                if (Game.IsKeyDown(Keys.N))
                {
                    isPrecise = !isPrecise;
                    GameFiber.Sleep(500);
                }
                if (Game.IsKeyDown(Keys.D0))
                {
                    isRunning = !isRunning;
                    NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
                    GameFiber.Sleep(500);
                }
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, dictionary, animation);
                Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise} ~n~Press 0 Pause{isRunning}");
                Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                //Game.DisplaySubtitle($"Current Animation Time {AnimationTime}");
                GameFiber.Yield();
            }

            if (SmokedItem.Exists())
            {
                SmokedItem.Delete();
            }
        }, "Run Debug Logic");
    }
}
catch(Exception e)
{
    Game.DisplayNotification("ERROR DEBUG");
}
}
private void AttachItem(Rage.Object SmokedItem, string boneName, Vector3 offset, Rotator rotator)
{
if (SmokedItem.Exists())
{
    SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, boneName), offset, rotator);

}
}
private bool CheckAttachmentkeys()
{

float adderOffset = isPrecise ? 0.001f : 0.01f;
float rotatorOFfset = isPrecise ? 1.0f : 10f;
if (Game.IsKeyDownRightNow(Keys.T))//X UP?
{
    Offset = new Vector3(Offset.X + adderOffset, Offset.Y, Offset.Z);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.G))//X Down?
{
    Offset = new Vector3(Offset.X - adderOffset, Offset.Y, Offset.Z);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.Y))//Y UP?
{
    Offset = new Vector3(Offset.X, Offset.Y + adderOffset, Offset.Z);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.H))//Y Down?
{
    Offset = new Vector3(Offset.X, Offset.Y - adderOffset, Offset.Z);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.U))//Z Up?
{
    Offset = new Vector3(Offset.X, Offset.Y, Offset.Z + adderOffset);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.J))//Z Down?
{
    Offset = new Vector3(Offset.X, Offset.Y, Offset.Z - adderOffset);
    return true;
}

else if (Game.IsKeyDownRightNow(Keys.I))//XR Up?
{
    Rotation = new Rotator(Rotation.Pitch + rotatorOFfset, Rotation.Roll, Rotation.Yaw);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.K))//XR Down?
{
    Rotation = new Rotator(Rotation.Pitch - rotatorOFfset, Rotation.Roll, Rotation.Yaw);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.O))//YR Up?
{
    Rotation = new Rotator(Rotation.Pitch, Rotation.Roll + rotatorOFfset, Rotation.Yaw);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.L) || Game.IsKeyDownRightNow(Keys.OemPeriod))//YR Down?
{
    Rotation = new Rotator(Rotation.Pitch, Rotation.Roll - rotatorOFfset, Rotation.Yaw);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.P))//ZR Up?
{
    Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw + rotatorOFfset);
    return true;
}
else if (Game.IsKeyDownRightNow(Keys.OemSemicolon))//ZR Down?
{
    Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw - rotatorOFfset);
    return true;
}
return false;
}
private void PlayTextReceivedSound()
{

TextSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSoundID, "Text_Arrive_Tone", "Phone_SoundSet_Default", 0);

GameFiber.Sleep(1000);
NativeFunction.Natives.RELEASE_SOUND_ID(TextSoundID);
}
private void PlayPhoneResponseSound()
{

HangUpSoundID = NativeFunction.Natives.GET_SOUND_ID<int>();
NativeFunction.Natives.PLAY_SOUND_FRONTEND(HangUpSoundID, "Hang_Up", "Phone_SoundSet_Default", 0);


GameFiber.Sleep(1000);

NativeFunction.Natives.RELEASE_SOUND_ID(HangUpSoundID);

}


private void WeaponTest1()
{
//shovel replacing baseball bat?
string propName = NativeHelper.GetKeyboardInput("gr_prop_gr_hammer_01");
string weaponHashText = NativeHelper.GetKeyboardInput("1317494643");
if (uint.TryParse(weaponHashText, out uint WeaponHash))
{
    NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, WeaponHash, 200, false, false);
    NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Game.LocalPlayer.Character, WeaponHash, true);
    if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
    {
        Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = false;
    }
    //BONETAG_R_PH_HAND new Vector3(-0.03f,-0.2769999f,-0.06200002f),new Rotator(20f, -101f, 81f)
    Rage.Object weaponObject = null;
    try
    {
        weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
        string HandBoneName = "BONETAG_R_PH_HAND";
        Offset = new Vector3(0f, 0f, 0f);
        Rotation = new Rotator(0f, 0f, 0f);
        if (weaponObject.Exists())
        {
            AttachItem(weaponObject, HandBoneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));

            GameFiber.StartNew(delegate
            {
                while (!Game.IsKeyDownRightNow(Keys.Space))
                {
                    uint currentWeapon;
                    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Game.LocalPlayer.Character, out currentWeapon, true);
                    if (currentWeapon != WeaponHash)
                    {
                        break;
                    }
                    if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                    {
                        AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                        GameTimeLastAttached = Game.GameTime;
                    }
                    if (Game.IsKeyDown(Keys.B))
                    {
                        //EntryPoint.WriteToConsoleTestLong($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                        GameFiber.Sleep(500);
                    }
                    if (Game.IsKeyDown(Keys.N))
                    {
                        isPrecise = !isPrecise;
                        GameFiber.Sleep(500);
                    }
                    Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                    Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}");
                    GameFiber.Yield();
                }
                if (weaponObject.Exists())
                {
                    weaponObject.Delete();
                }
                if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
                {
                    Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = true;
                }
            }, "Run Debug Logic");
        }
    }
    catch (Exception ex)
    {
        //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
    }
}
}

private void ParticleTest1()
{
//shovel replacing baseball bat?
string propName = NativeHelper.GetKeyboardInput("p_cs_lighter_01");
string particleGroupName = NativeHelper.GetKeyboardInput("core");
string particleName = NativeHelper.GetKeyboardInput("veh_sub_leak");
Rage.Object weaponObject = null;
try
{
    weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
    string HandBoneName = "BONETAG_L_PH_HAND";

    Offset = new Vector3(0.0f, 0.0f, 0.0f);
    Rotation = new Rotator(0f, 0f, 0f);


    Vector3 CoolOffset = new Vector3(0.13f, 0.02f, 0.02f);
    Rotator CoolRotation = new Rotator(-93f, 40f, 0f);
    if (weaponObject.Exists())
    {
        string dictionary = "anim@amb@casino@hangout@ped_male@stand_withdrink@01a@base";
        string animation = "base";

        AnimationDictionary.RequestAnimationDictionay(dictionary);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1



        AttachItem(weaponObject, HandBoneName, CoolOffset, CoolRotation);
        LoopedParticle particle = new LoopedParticle(particleGroupName, particleName, weaponObject, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, 1.5f);
        GameFiber.StartNew(delegate
        {
            while (!Game.IsKeyDownRightNow(Keys.Space))
            {
                if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                {
                    particle.Stop();
                    particle = new LoopedParticle(particleGroupName, particleName, weaponObject, Offset, Rotation, 1.5f);
                    //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                    GameTimeLastAttached = Game.GameTime;
                }
                if (Game.IsKeyDown(Keys.B))
                {
                    //EntryPoint.WriteToConsoleTestLong($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                    GameFiber.Sleep(500);
                }
                if (Game.IsKeyDown(Keys.N))
                {
                    isPrecise = !isPrecise;
                    GameFiber.Sleep(500);
                }
                Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}");
                GameFiber.Yield();
            }
            if (weaponObject.Exists())
            {
                weaponObject.Delete();
            }
            if(particle != null)
            {
                particle.Stop();
            }
        }, "Run Debug Logic");
    }
}
catch (Exception ex)
{
    //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
}

}


//private void AlertMessage()
//{
//    PopUpWarning popUpWarning = new PopUpWarning("exit to windows","Are you sure you want to exit to the desktop","",Player.ButtonPrompts, Settings);
//    popUpWarning.Setup();
//    popUpWarning.Show();
//    GameFiber.StartNew(delegate
//    {
//        while (!popUpWarning.IsAnswered)
//        {
//            GameFiber.Yield();
//        }
//        EntryPoint.WriteToConsole($"Pop Up Warning Exit Result IsAccepted{popUpWarning.IsAccepted} IsRejected{popUpWarning.IsRejected}");
//    }, "Run Debug Logic");


//}
private void ScriptedAnimation()
{
//unsafe
//{
//    long* Var31 = null;
//    long* Var32 = null;

//    Var31[4] = 1065353216;
//    Var31[5] = 1065353216;
//    Var31[9] = 1065353216;
//    Var31[10] = 1065353216;
//    Var31[14] = 1065353216;
//    Var31[17] = 1065353216;
//    Var31[18] = 1065353216;
//    Var31[19] = 1065353216;

//    Var32[4] = 1065353216;
//    Var32[5] = 1065353216;
//    Var32[9] = 1065353216;
//    Var32[10] = 1065353216;
//    Var32[14] = 1065353216;
//    Var32[15] = 1065353216;
//    Var32[17] = 1065353216;
//    Var32[18] = 1065353216;
//    Var32[19] = 1065353216;

//    Var31 = (long*)1L;
//    Var31[1] = "";
//    Var31[4] = 1065353216;
//    Var31[4] = 1065353216;
//    Var31[4] = 1065353216;
//    Var31[4] = 1065353216;


//    NativeFunction.CallByName<int>("TASK_SCRIPTED_ANIMATION", Game.LocalPlayer.Character, &Var31, &Var32, &Var32, 0.0f, 0.25f);
//    //NativeFunction.Natives.TASK_SCRIPTED_ANIMATION(Game.LocalPlayer.Character,);
//}
}

private void HighlightObject()
{
//Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
//if (ClosestEntity.Exists())
//{


//    Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
//    DesiredPos = new Vector3(DesiredPos.X, DesiredPos.Y, Game.LocalPlayer.Character.Position.Z);
//    float DesiredHeading = Math.Abs(ClosestEntity.Heading + 180f);
//    float ObjectHeading = ClosestEntity.Heading;
//    if (ClosestEntity.Heading >= 180f)
//    {
//        DesiredHeading = ClosestEntity.Heading - 180f;
//    }
//    else
//    {
//        DesiredHeading = ClosestEntity.Heading + 180f;
//    }




//    EntryPoint.WriteToConsole($"Sitting Closest = {ClosestEntity.Model.Name}", 5);
//    EntryPoint.WriteToConsole($"Sitting Activity ClosestSittableEntity X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z}", 5);


//    if (ClosestEntity.Model.Dimensions.X >= 2f)
//    {

//    }

//    uint GameTimeStartedDisplaying = Game.GameTime;
//    while (Game.GameTime - GameTimeStartedDisplaying <= 3000)
//    {

//        Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
//        GameFiber.Yield();
//    }

//  }
}

//private void AddGPSRoute()
//{
//    if(Player.CurrentGPSBlip.Exists())
//    {
//        Player.CurrentGPSBlip.Delete();
//    }
//    GameLocation coolPlace = PlacesOfInterest.GetAllPlaces().PickRandom();
//    if(coolPlace != null)
//    {
//        Game.DisplayHelp($"Adding GPS To {coolPlace.Name}");
//        Blip MyLocationBlip = new Blip(coolPlace.EntrancePosition)
//        {
//            Name = coolPlace.Name
//        };
//        if (MyLocationBlip.Exists())
//        {
//            MyLocationBlip.Color = Color.Yellow;
//            NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE(MyLocationBlip, false);
//            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
//            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(coolPlace.Name);
//            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);
//            NativeFunction.Natives.SET_BLIP_ROUTE(MyLocationBlip, true);
//            Player.CurrentGPSBlip = MyLocationBlip;
//            World.AddEntity(MyLocationBlip);
//        }
//    }
//}


private void SetFlags()
{
GameFiber.StartNew(delegate
{




    bool setMission = false; ;

    while (!Game.IsKeyDown(Keys.P))
    {
        NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(0f);
        Game.DisplayHelp($"Press P to Stop~n~Press O to Mission");



        if(Game.IsKeyDown(Keys.O) && !setMission)
        {
            NativeFunction.Natives.SET_MISSION_FLAG(true);
            Game.DisplaySubtitle("SET MISSION FLAG TO TRUE");
            setMission = true;
        }

        GameFiber.Yield();
    }

    if(setMission)
    {
        NativeFunction.Natives.SET_MISSION_FLAG(false);
    }
    //if (myCar.Exists())
    //{
    //    myCar.Delete();
    //}
    //if (busDriver.Exists())
    //{
    //    busDriver.Delete();
    //}
    //NativeFunction.Natives.SET_ENTITY_AS_MISSION_ENTITY(myCar, false, 1);

}, "Run Debug Logic");
}
private void SpawnBus()
{

GameFiber.StartNew(delegate
{





    Vector3 busSpot = Game.LocalPlayer.Character.GetOffsetPositionFront(30f);
    Vehicle myCar = new Vehicle("bus", busSpot);

    GameFiber.Yield();
    if (myCar.Exists())
    {
        myCar.IsPersistent = false;
        myCar.IsEngineOn = true;
        Ped busDriver = new Ped("S_M_M_GENTRANSPORT",Game.LocalPlayer.Character.GetOffsetPositionFront(15f),0f);
        GameFiber.Yield();
        if (busDriver.Exists())
        {
            busDriver.IsPersistent = false;
            busDriver.BlockPermanentEvents = false;
            busDriver.KeepTasks = false;
            busDriver.WarpIntoVehicle(myCar, -1);
        }
        Game.DisplayHelp($"person spawned normally");
        //while (!Game.IsKeyDown(Keys.O))
        //{
        //    GameFiber.Yield();
        //}
        //if (myCar.Exists())
        //{
        //    myCar.Delete();
        //}
        //if (busDriver.Exists())
        //{
        //    busDriver.Delete();
        //}
        //NativeFunction.Natives.SET_ENTITY_AS_MISSION_ENTITY(myCar, false, 1);
    }
}, "Run Debug Logic");
}
private void NodeChekcer()
{


try
{
    GameFiber.StartNew(delegate
    {
        bool isShowing = false;
        while (!Game.IsKeyDownRightNow(Keys.P))
        {
            Vector3 position = Game.LocalPlayer.Character.Position;
            int NodeID = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_ID<int>(position.X, position.Y, position.Z, 1, 1, 30f, 30f);
            Vector3 newPos;
            NativeFunction.Natives.GET_VEHICLE_NODE_POSITION(NodeID, out newPos);


            Rage.Debug.DrawArrowDebug(newPos + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color.White);

            Game.DisplayHelp($"Press P to Stop");
            GameFiber.Yield();
        }
        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
    }, "Run Debug Logic");
}
catch (Exception ex)
{
    Game.DisplayNotification("Shit CRASHES!!!");
}



}
private void HighlightDoorsAndProps()
{
try
{
    GameFiber.StartNew(delegate
    {
        bool isShowing = false;
        while (!Game.IsKeyDownRightNow(Keys.P))
        {
            Game.DisplayHelp($"Press P to Stop~n~Press J to Show~n~Press K To Store");


            if (Game.IsKeyDownRightNow(Keys.J))
            {
                Entity Target = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
                // Entity Target = Game.LocalPlayer.GetFreeAimingTarget();
                if(Target.Exists())
                {
                    string Text = $"Object Name: {Target.Model.Name} Hash: {Target.Model.Hash} new Vector3({Target.Position.X}f,{Target.Position.Y}f,{Target.Position.Z}f), {Target.Heading}f";
                    Game.DisplayNotification(Text);
                    EntryPoint.WriteToConsole(Text);

                    GameFiber.StartNew(delegate
                    {
                        uint GameTimeStarted = Game.GameTime;
                        while(Target.Exists() && Game.GameTime - GameTimeStarted <= 5000 && !isShowing)
                        {
                            Rage.Debug.DrawArrowDebug(Target.Position + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
                            GameFiber.Yield();
                        }

                    }, "Run Debug Logic");






                }
            }
            else if (Game.IsKeyDownRightNow(Keys.K))
            {
                Entity Target = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
                //Entity Target = Game.LocalPlayer.GetFreeAimingTarget();
                if (Target.Exists())
                {
                    string text1 = NativeHelper.GetKeyboardInput("Description");
                    string name = "Dead Drop";
                    string description = "Literally";
                    string Text = $"Object Name: {Target.Model.Name} Hash: {Target.Model.Hash} new Vector3({Target.Position.X}f,{Target.Position.Y}f,{Target.Position.Z}f), {Target.Heading}f, ";

                    EntryPoint.WriteToConsole(Text);

                    string Text2 = $"new DeadDrop(new Vector3({Target.Position.X}f,{Target.Position.Y}f,{Target.Position.Z}f), {Target.Heading}f, \"{name}\", \"{text1}\" )";// { OpenTime = 0, CloseTime = 24, IsEnabled = false },";
                    Text2 += " { OpenTime = 0,CloseTime = 24, IsEnabled = false },";


                    WriteToPropLocations(Text2);
                }
                GameFiber.Sleep(500);
            }
            GameFiber.Yield();
        }
        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
    }, "Run Debug Logic");
}
catch (Exception ex)
{
    Game.DisplayNotification("Shit CRASHES!!!");
}
}
private void WriteToPropLocations(String TextToLog)
{
StringBuilder sb = new StringBuilder();
sb.Append(TextToLog + System.Environment.NewLine);
File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredPropLocations.txt", sb.ToString());
sb.Clear();
}
private void contacttest()
{
//Player.AddContact("Vagos Boss",ContactIcon.MP_MexBoss);
//Player.AddContact("LOST MC Boss", ContactIcon.MP_BikerBoss);
//try
//{
//    GameFiber.StartNew(delegate
//    {
//        // Custom phone creation
//        _iFruit = new CustomiFruit();

//        // Phone customization (totally optional)
//        /*
//        _iFruit.CenterButtonColor = System.Drawing.Color.Orange;
//        _iFruit.LeftButtonColor = System.Drawing.Color.LimeGreen;
//        _iFruit.RightButtonColor = System.Drawing.Color.Purple;
//        _iFruit.CenterButtonIcon = SoftKeyIcon.Fire;
//        _iFruit.LeftButtonIcon = SoftKeyIcon.Police;
//        _iFruit.RightButtonIcon = SoftKeyIcon.Website;
//        */

            //        // New contact (wait 4 seconds (4000ms) before picking up the phone)
            //        iFruitContact contactA = new iFruitContact("Unknown Gang Boss", 40);
            //        contactA.Answered += ContactAnswered;   // Linking the Answered event with our function
            //        contactA.DialTimeout = 4000;            // Delay before answering
            //        contactA.Active = true;                 // true = the contact is available and will answer the phone
            //        contactA.Icon = ContactIcon.MP_MexBoss;      // Contact's icon
            //        _iFruit.Contacts.Add(contactA);         // Add the contact to the phone

            //        // New contact (wait 4 seconds before displaying "Busy...")
            //        iFruitContact contactB = new iFruitContact("Families Boss", 41);
            //        contactB.DialTimeout = 4000;
            //        contactB.Active = false;                // false = the contact is busy
            //        contactB.Icon = ContactIcon.Blocked;
            //        contactB.Bold = true;                   // Set the contact name in bold
            //        _iFruit.Contacts.Add(contactB);



            //        while (!Game.IsKeyDownRightNow(Keys.P))
            //        {
            //            Game.DisplayHelp($"Press P to Stop");
            //            _iFruit.Update();

            //            GameFiber.Yield();
            //        }

            //    }, "Run Debug Logic");
            //}
            //catch (Exception ex)
            //{
            //    Game.DisplayNotification("Shit CRASHES!!!");
            //}





        }

        //private void ContactAnswered(iFruitContact contact)
        //{
        //    // The contact has answered, we can execute our code
        //    Game.DisplayNotification("The contact has answered.");

        //    // We need to close the phone at a moment.
        //    // We can close it as soon as the contact pick up calling _iFruit.Close().
        //    // Here, we will close the phone in 5 seconds (5000ms).
        //    _iFruit.Close(5000);
        //}

        //private void CreatePointChecker()
        //{

        //    Vector3 CoolPos = Game.LocalPlayer.Character.Position.Around2D(10f);
        //    Color coolColor = Color.Yellow;
        //    GameFiber.StartNew(delegate
        //    {
        //        while (!Game.IsKeyDown(Keys.O))
        //        {
        //            if(Extensions.PointIsInFrontOfPed(Game.LocalPlayer.Character,CoolPos))
        //            {
        //                coolColor = Color.Red;
        //            }
        //            else
        //            {
        //                coolColor = Color.Yellow;
        //            }
        //            float Result = Extensions.GetDotVectorResult(Game.LocalPlayer.Character, CoolPos);
        //            Game.DisplayHelp($"Press O to Stop GetDotVectorResult {Result}");
        //            Rage.Debug.DrawArrowDebug(CoolPos, Vector3.Zero, Rotator.Zero, 1f, coolColor);

        //            GameFiber.Yield();
        //        }

        //    }, "Run Debug Logic");
        //}
        private void BrowseTimecycles()
    {
        try
        {
            GameFiber.StartNew(delegate
            {

                while (!Game.IsKeyDownRightNow(Keys.P))
                {
                    Game.DisplayHelp($"Press P to Stop J/K To Toggle Current {TimecycleName}");


                    if (Game.IsKeyDownRightNow(Keys.J))
                    {
                        if (TimecycleIndex <= 0)
                        {
                            TimecycleIndex = 0;
                        }
                        else
                        {
                            TimecycleIndex--;
                        }
                        TimecycleName = Timecycles[TimecycleIndex];
                        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
                        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", TimecycleName);
                        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", 1.0f);
                        GameFiber.Sleep(500);
                    }
                    if (Game.IsKeyDownRightNow(Keys.K))
                    {
                        if (TimecycleIndex > Timecycles.Count() - 1)
                        {
                            TimecycleIndex = 0;
                        }
                        else
                        {
                            TimecycleIndex++;
                        }

                        TimecycleName = Timecycles[TimecycleIndex];
                        NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
                        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", TimecycleName);
                        NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", 1.0f);
                        GameFiber.Sleep(500);
                    }
                    GameFiber.Sleep(25);
                }
                NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            }, "Run Debug Logic");
        }
        catch(Exception ex)
        {
            Game.DisplayNotification("Shit CRASHES!!!");
        }

    }
    private string GetInternalZoneString(Vector3 ZonePosition)
    {
        IntPtr ptr = Rage.Native.NativeFunction.Natives.GET_NAME_OF_ZONE<IntPtr>(ZonePosition.X, ZonePosition.Y, ZonePosition.Z);
        return Marshal.PtrToStringAnsi(ptr);
    }
    private Vector3 RotationToDirection(Rotator rotation)
    {
        double retZ = rotation.Yaw * 0.01745329f;
        double retX = rotation.Pitch * 0.01745329f;
        double absX = Math.Abs(Math.Cos(retX));
        return new Vector3((float)-(Math.Sin(retZ) * absX), (float)(Math.Cos(retZ) * absX), (float)Math.Sin(retX));
    }
    private Vector3 RotationToDirection(Vector3 rotation)
    {
        double retZ = rotation.Z * 0.01745329f;
        double retX = rotation.X * 0.01745329f;
        double absX = Math.Abs(Math.Cos(retX));
        return new Vector3((float)-(Math.Sin(retZ) * absX), (float)(Math.Cos(retZ) * absX), (float)Math.Sin(retX));
    }
    private void PedCameraStuff()
    {
        Game.FadeScreenOut(2500, true);
        GameFiber.Sleep(500);
        Game.LocalPlayer.Character.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
        CharCam = new Camera(false);
        CharCam.Position = new Vector3(402.8473f, -998.3224f, -98.00025f);//new Vector3(402.9562f, -1000.557f, -99.00404f);
        //CharCam.Rotation = //new Rotator(-4.537422f, 0.05429313f, -0.983484f);
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        CharCam.Rotation = new Rotator(r.X, r.Y, r.Z);
        Vector3 ToLookAt = new Vector3(402.8473f, -996.3224f, -99.00025f);
        Vector3 _direction = (ToLookAt - CharCam.Position).ToNormalized();
        CharCam.Direction = _direction;
        CharCam.Active = true;
        Ped Model = new Ped("S_M_M_GENTRANSPORT", new Vector3(402.8473f, -996.3224f, -99.00025f), 182.7549f);
        GameFiber.Yield();
        if (Model.Exists())
        {
            Model.IsPersistent = true;
            Model.IsVisible = true;
            EntryPoint.WriteToConsole($"Model Exists {Model.Model.Name} {Model.Position}", 5);
            Model.BlockPermanentEvents = true;
        }

        //uint GameTimeStarted = Game.GameTime;
        //while (Game.GameTime - GameTimeStarted <= 2000)
        //{
        //    if (Model.Exists())
        //    {
        //        EntryPoint.WriteToConsole($"Model Exists BEFORE {Model.Model.Name} {Model.Position}", 5);
        //    }
        //    else
        //    {
        //        EntryPoint.WriteToConsole($"Model DOES NOT Exist BEFORE {Model.Model.Name} {Model.Position}", 5);
        //    }
        //    GameFiber.Sleep(200);
        //}

        // GameFiber.Sleep(500);
        Game.FadeScreenIn(1500, true);

        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= 5000)
        {
            if (Model.Exists())
            {
                EntryPoint.WriteToConsole($"Model Exists AFTER {Model.Model.Name} {Model.Position}", 5);
            }
            else
            {
                EntryPoint.WriteToConsole($"Model DOES NOT Exist AFTER {Model.Model.Name} {Model.Position}", 5);
            }
            GameFiber.Sleep(200);
        }
        CharCam.Active = false;

        if (Model.Exists())
        {
            EntryPoint.WriteToConsole($"Model Exists DELETING {Model.Model.Name} {Model.Position}", 5);
            Model.Delete();
        }
        else
        {
            EntryPoint.WriteToConsole($"Model DOES NOT Exist DELETING {Model.Model.Name} {Model.Position}", 5);
        }
    }
    private void PedSettingStuff()
    {
        Ped myPed = new Ped("S_M_M_GENTRANSPORT", Game.LocalPlayer.Character.GetOffsetPositionFront(5f),0f);
        GameFiber.Yield();
        if (myPed.Exists())
        {
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(myPed, ComponentNumber);
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(myPed, ComponentNumber, DrawableNumber);
                    for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
                    {
                        bool IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(myPed, ComponentNumber, DrawableNumber, TextureNumber);
                        if(IsValid)
                        {
                            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(myPed, ComponentNumber, DrawableNumber, TextureNumber, 0);
                            EntryPoint.WriteToConsole($"PedSettingStuff Is Valid Variation {ComponentNumber} {DrawableNumber} {TextureNumber}", 5);
                            GameFiber.Sleep(1000);
                            if(!myPed.Exists())
                            {
                                return;
                            }
                        }
                    }
                }

            }
        }
        GameFiber.Sleep(5000);
        if(myPed.Exists())
        {
            myPed.Delete();
        }
        //NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(myPed, ComponentID, DrawableID);
    }
    private void SpawnItemInFrom()
    {
        GameFiber.StartNew(delegate
        {
            string ModelName = NativeHelper.GetKeyboardInput("");
            if (ModelName != "")
            {
                Rage.Object myobject = new Rage.Object(ModelName, Game.LocalPlayer.Character.GetOffsetPositionFront(5f));
                GameFiber.Yield();
                while (myobject.Exists() && !Game.IsKeyDownRightNow(Keys.P))
                {
                    myobject.IsGravityDisabled = true;
                    Game.DisplayHelp($"Model {ModelName} Spawned! Press P to Delete");
                    GameFiber.Sleep(25);
                }
                if (myobject.Exists())
                {
                    myobject.Delete();
                }
            }
        }, "Run Debug Logic");
    }
    private void HighlightStoreWithCamera()
    {

        Vector3 CameraPos = new Vector3(-216.1503f, -54.80959f, 59.33761f);
        Rotator HowToRotate = new Rotator(-11.99999f, 0f, -15.95173f);
        if (!CharCam.Exists())
        {
            CharCam = new Camera(false);
        }
        CharCam.Position = CameraPos;
        CharCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CharCam.Rotation = HowToRotate;
        if (!InterpolationCamera.Exists())
        {
            InterpolationCamera = new Camera(false);
        }
        InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
        InterpolationCamera.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(CharCam, InterpolationCamera, 1500, true, true);

    }
    private void ReturnToGameplay()
    {
        if (!InterpolationCamera.Exists())
        {
            InterpolationCamera = new Camera(false);
        }
        InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
        InterpolationCamera.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(InterpolationCamera, CharCam, 1500, true, true);
        GameFiber.Sleep(1500);
        InterpolationCamera.Active = false;
        if (CharCam.Exists())
        {
            CharCam.Delete();
        }
        if (InterpolationCamera.Exists())
        {
            InterpolationCamera.Delete();
        }
    }
    private void SetRadarZoomeFor20Seconds(float distance)
    {

            GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
            {
                uint GameTimeStartedRadarZoom = Game.GameTime;
                while (Game.GameTime - GameTimeStartedRadarZoom <= 20000)
                {
                    NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(distance);
                    GameFiber.Yield();
                }
                

            }, "SetRadarZoomeFor20Seconds");
        
    }
    private void SetInRandomInterior()
    {
      InteriorPosition mypos =  InteriorPositions.PickRandom();
        if (mypos != null)
        {
            EntryPoint.WriteToConsole($"Player Set In {mypos.Name}", 5);
            Game.LocalPlayer.Character.Position = mypos.Position;
        }
    }
    private void WriteToLogLocations(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "Locations.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogInteriors(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "Interiors.txt", sb.ToString());
        sb.Clear();
    }
    private void DrawDebugArrowsOnPeds()
    {
        Vector3 Position = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 31086));
        Rage.Debug.DrawArrowDebug(Position, Vector3.Zero, Rotator.Zero, 1f, Color.White);
        Vector3 Position2 = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005));
        Rage.Debug.DrawArrowDebug(Position2, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
    }
    private void DrawColoredArrowTaskStatus(PedExt PedToDraw)
    {
        Color Color = Color.White;
        TaskStatus taskStatus = PedToDraw.Pedestrian.Tasks.CurrentTaskStatus;
        if (taskStatus == TaskStatus.InProgress)
        {
            Color = Color.Green;
        }
        else if (taskStatus == TaskStatus.Interrupted)
        {
            Color = Color.Red;
        }
        else if (taskStatus == TaskStatus.None)
        {
            Color = Color.White;
        }
        else if (taskStatus == TaskStatus.NoTask)
        {
            Color = Color.Blue;
        }
        else if (taskStatus == TaskStatus.Preparing)
        {
            Color = Color.Purple;
        }
        else if (taskStatus == TaskStatus.Unknown)
        {
            Color = Color.Yellow;
        }

        Rage.Debug.DrawArrowDebug(PedToDraw.Pedestrian.Position, Vector3.Zero, Rotator.Zero, 1f, Color);
    }
    private void DrawColoredArrowAlertness(PedExt PedToDraw)
    {
        Color Color = Color.White;
        int Alertness = NativeFunction.Natives.GET_PED_ALERTNESS<int>(PedToDraw.Pedestrian);
        if (Alertness == 0)
        {
            Color = Color.Green;
        }
        else if (Alertness == 1)
        {
            Color = Color.Red;
        }
        else if (Alertness == 2)
        {
            Color = Color.White;
        }
        else if (Alertness == 3)
        {
            Color = Color.Blue;
        }
        else
        {
            Color = Color.Yellow;
        }
        Rage.Debug.DrawArrowDebug(new Vector3(PedToDraw.Pedestrian.Position.X, PedToDraw.Pedestrian.Position.Y, PedToDraw.Pedestrian.Position.Z + 1f), Vector3.Zero, Rotator.Zero, 1f, Color);
    }
    private void MakeNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        EntryPoint.WriteToConsole("KeyDown: You are NOT invicible", 4);
        Game.DisplaySubtitle("Invincibility Off");
    }
    private void MakeInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        EntryPoint.WriteToConsole("KeyDown: You are invicible", 4);
        Game.DisplaySubtitle("Invincibility On");
    }
    private void MakeDrunk()
    {
        NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, true);
        if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>("move_m@drunk@verydrunk"))
        {
            NativeFunction.Natives.REQUEST_ANIM_SET<bool>("move_m@drunk@verydrunk");
        }
        NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character, "move_m@drunk@verydrunk", 0x3E800000);
        NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        NativeFunction.Natives.SET_TIMECYCLE_MODIFIER<int>("Drunk");
        NativeFunction.Natives.SET_TIMECYCLE_MODIFIER_STRENGTH<int>(1.1f);
        NativeFunction.Natives.x80C8B1846639BB19(1);
        NativeFunction.Natives.SHAKE_GAMEPLAY_CAM<int>("DRUNK_SHAKE", 5.0f);
        //EntryPoint.WriteToConsole("Player Made Drunk");
    }
    private void MakeSober()
    {
        NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
        NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        //EntryPoint.WriteToConsole("Player Made Sober");
    }
    public void LoadNorthYankton()
    {
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_01");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01c");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01c_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01d");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01d_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01e");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01e_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01f");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01f_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01g");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01h");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01h_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01i");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01i_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01j");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01j_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01k");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01k_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01z");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01z_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_02");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue02");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue02_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_03");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_grv_dug");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_grv_dug_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_grv_torch");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_04");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04_cover");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_end");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_start");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_start_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_05");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_06");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_int");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_int_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_pannel");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_pannel_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_m2_door");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_m2_door_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_occl_00");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_occl");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_rd");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerd");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerdb");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerd_lod");
    }
    public void UnLoadNorthYankton()
    {
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_01");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01c");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01c_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01d");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01d_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01e");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01e_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01f");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01f_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01g");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01h");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01h_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01i");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01i_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01j");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01j_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01k");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01k_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01z");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01z_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_02");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue02");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue02_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_03");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_grv_dug");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_grv_dug_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_grv_torch");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_04");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04_cover");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_end");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_start");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_start_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_05");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_06");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_int");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_int_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_pannel");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_pannel_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_m2_door");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_m2_door_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_occl_00");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_occl");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_rd");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerd");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerdb");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerd_lod");
    }
    private void SpawnModelChecker()
    {
        Ped completelynewnameAsd = new Ped("S_M_M_GENTRANSPORT", Player.Character.Position.Around2D(5f), Game.LocalPlayer.Character.Heading); //new Ped(Player.Character.Position.Around2D(5f));//new Ped("a_f_y_smartcaspat_01", Player.Character.Position.Around2D(5f), Game.LocalPlayer.Character.Heading);//S_M_M_GENTRANSPORT
        GameFiber.Yield();
        if (!completelynewnameAsd.Exists())
        {
            return;
        }
        completelynewnameAsd.Model.LoadAndWait();
        string tempModelName = completelynewnameAsd.Model.Name;
        completelynewnameAsd.RandomizeVariation();
        EntryPoint.WriteToConsole($"SpawnModelChecker! 1 {tempModelName}", 5);
        GameFiber.Sleep(5000);
        if(completelynewnameAsd.Exists())
        {
            tempModelName = completelynewnameAsd.Model.Name;
            EntryPoint.WriteToConsole($"SpawnModelChecker! 2 {tempModelName}", 5);
        }
        GameFiber.Sleep(500);
        if(completelynewnameAsd.Exists())
        {
            completelynewnameAsd.Delete();
        }
    }
    private void SpawnModelChecker2()
    {
        Ped completelynewnameAsd = new Ped("S_M_M_GENTRANSPORT", Player.Character.Position.Around2D(5f), Game.LocalPlayer.Character.Heading);//S_M_M_GENTRANSPORT
        GameFiber.Yield();
        if (!completelynewnameAsd.Exists())
        {
            return;
        }
        completelynewnameAsd.Model.LoadAndWait();
        string tempModelName = completelynewnameAsd.Model.Name;
        completelynewnameAsd.RandomizeVariation();
        EntryPoint.WriteToConsole($"SpawnModelChecker! 1 {tempModelName}", 5);
        GameFiber.Sleep(5000);
        if (completelynewnameAsd.Exists())
        {
            tempModelName = completelynewnameAsd.Model.Name;
            EntryPoint.WriteToConsole($"SpawnModelChecker! 2 {tempModelName}", 5);
        }
        GameFiber.Sleep(500);


        unsafe
        {
            var PedPtr = (ulong)completelynewnameAsd.MemoryAddress;
            ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
            *((ulong*)(SkinPtr + 0x18)) = 411102470;
        }
        GameFiber.Sleep(5000);
        if (completelynewnameAsd.Exists())
        {
            tempModelName = completelynewnameAsd.Model.Name;
            EntryPoint.WriteToConsole($"SpawnModelChecker! 3 {tempModelName}", 5);
        }
        GameFiber.Sleep(500);
        if (completelynewnameAsd.Exists())
        {
            completelynewnameAsd.Delete();
        }
        //411102470
    }
    private void SpawnAttackHeli1()
    {

        GameFiber.StartNew(delegate
        {





            Vector3 HeliSpot = Game.LocalPlayer.Character.GetOffsetPositionFront(30f);
            Vehicle myCar = new Vehicle("valkyrie", new Vector3(HeliSpot.X, HeliSpot.Y, HeliSpot.Z + 100f));

            GameFiber.Yield();
            if (myCar.Exists())
            {
                myCar.IsEngineOn = true;
                Ped pilot = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(15f));
                GameFiber.Yield();
                if (pilot.Exists())
                {
                    pilot.BlockPermanentEvents = true;
                    pilot.KeepTasks = true;
                    pilot.WarpIntoVehicle(myCar, -1);
                    pilot.Tasks.ChaseWithHelicopter(Game.LocalPlayer.Character, new Vector3(0f, 30f, 50f));
                    myCar.IsEngineOn = true;
                }
                Ped myPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f));
                GameFiber.Yield();
                if (myPed.Exists())
                {
                    myPed.WarpIntoVehicle(myCar, 1);
                    myPed.BlockPermanentEvents = true;
                    myPed.KeepTasks = true;

                    NativeFunction.Natives.TASK_COMBAT_PED(myPed, Player.Character, 0, 16);
                    myPed.RelationshipGroup = RelationshipGroup.HatesPlayer;
                    uint valTurret = NativeFunction.Natives.GET_HASH_KEY<uint>("VEHICLE_WEAPON_TURRET_VALKYRIE");
                    NativeFunction.Natives.SET_CURRENT_PED_VEHICLE_WEAPON(myPed, valTurret);



                    if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(myPed, 0, Player.CurrentVehicle.Vehicle, 0f, 0f, 0f, 0f, 0f);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(myPed, Player.Character, 0, 0f, 0f, 0f, 0f, 0f);
                    }
                }
                Game.DisplayHelp($"person spawned normally");
                while (!Game.IsKeyDown(Keys.O))
                {
                    GameFiber.Yield();
                }
                if (myCar.Exists())
                {
                    myCar.Delete();
                }
                if (myPed.Exists())
                {
                    myPed.Delete();
                }
                if (pilot.Exists())
                {
                    pilot.Delete();
                }
                //NativeFunction.Natives.SET_ENTITY_AS_MISSION_ENTITY(myCar, false, 1);
            }

        }, "Run Debug Logic");
    }
    private void SpawnAttackHeli()
    {

        GameFiber.StartNew(delegate
        {
            




            Vector3 HeliSpot = Game.LocalPlayer.Character.GetOffsetPositionFront(30f);
            Vehicle myCar = new Vehicle("polmav", new Vector3(HeliSpot.X, HeliSpot.Y, HeliSpot.Z + 100f));

            GameFiber.Yield();
            if (myCar.Exists())
            {
                myCar.IsEngineOn = true;
                Ped pilot = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(15f));
                GameFiber.Yield();
                if (pilot.Exists())
                {
                    pilot.IsPersistent = true;
                    pilot.BlockPermanentEvents = true;
                    pilot.KeepTasks = true;
                    pilot.WarpIntoVehicle(myCar, -1);
                    //pilot.Tasks.ChaseWithHelicopter(Game.LocalPlayer.Character, new Vector3(0f, 30f, 50f));
                    pilot.RelationshipGroup = RelationshipGroup.HatesPlayer;
                    pilot.Tasks.ChaseWithHelicopter(Game.LocalPlayer.Character, new Vector3(25f, 0f, 25f));//Right+Left-,Forawrd+Backwards-,Up+,Down-
                    //Vector3 pedPos = Player.Character.Position;
                    //if (Player.Character.CurrentVehicle.Exists())
                    //{
                    //    NativeFunction.Natives.TASK_HELI_MISSION(pilot, pilot.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    //}
                    //else
                    //{
                    //    NativeFunction.Natives.TASK_HELI_MISSION(pilot, pilot.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    //}
                    myCar.IsEngineOn = true;
                }
                Ped myPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f));
                GameFiber.Yield();
                if (myPed.Exists())
                {
                    myPed.IsPersistent = true;
                    myPed.WarpIntoVehicle(myCar, 1);
                    myPed.BlockPermanentEvents = true;
                    myPed.KeepTasks = true;
                    NativeFunction.Natives.GIVE_WEAPON_TO_PED(myPed, (uint)0x394F415C, 200, false, false);
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", myPed, 0x394F415C, true);
                    NativeFunction.Natives.TASK_COMBAT_PED(myPed, Player.Character, 0, 16);
                    myPed.RelationshipGroup = RelationshipGroup.HatesPlayer;
                }
                Game.DisplayHelp($"person spawned normally");
                while (!Game.IsKeyDown(Keys.O))
                {
                    GameFiber.Yield();
                }
                if (myCar.Exists())
                {
                    myCar.Delete();
                }
                if (myPed.Exists())
                {
                    myPed.Delete();
                }
                if (pilot.Exists())
                {
                    pilot.Delete();
                }
                //NativeFunction.Natives.SET_ENTITY_AS_MISSION_ENTITY(myCar, false, 1);
            }
        }, "Run Debug Logic");
    }
    private void SpawnGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionRight(10f).Around2D(10f));
            GameFiber.Yield();
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;

                coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);

                //if (RandomItems.RandomPercent(30))
                //{
                //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                //}
                //else if (RandomItems.RandomPercent(30))
                //{
                //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                //}
                coolguy.Tasks.FightAgainst(Game.LocalPlayer.Character);
            }
            while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P))
            {
                Game.DisplayHelp($"Attackers Spawned! Press P to Delete O to Flee");


                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    coolguy.Tasks.Clear();
                    coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                }
                GameFiber.Sleep(25);
            }
            if (coolguy.Exists())
            {
                coolguy.Delete();
            }
        }, "Run Debug Logic");
    }
    private void SpawnNoGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f));
            GameFiber.Yield();
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;
                NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", coolguy, 281, true);//Can Writhe
                NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", coolguy, false);

                if (RandomItems.RandomPercent(30))
                {
                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                }
                else if (RandomItems.RandomPercent(30))
                {
                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Knife, 1, true);
                }
                coolguy.Tasks.FightAgainst(Game.LocalPlayer.Character);
            }
            while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P) && ModController.IsRunning)
            {
               // Game.DisplayHelp($"Attackers Spawned! Press P to Delete O to Flee");


                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    coolguy.Tasks.Clear();
                    coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                }
                GameFiber.Sleep(25);
            }
            if (coolguy.Exists())
            {
                coolguy.Delete();
            }

        }, "Run Debug Logic");
    }
    private void WriteCivilianAndCopState()
    {

        EntryPoint.WriteToConsole($"PLAYER EVENT: INVESTIGATION START Police:{Player.Investigation.RequiresPolice} EMS;{Player.Investigation.RequiresEMS} Fire:{Player.Investigation.RequiresFirefighters}");

        EntryPoint.WriteToConsole($"============================================ PLAYER HANDLE {Game.LocalPlayer.Character.Handle} GameModel:{Game.LocalPlayer.Character.Model.Name} LSRModel:{Player.ModelName} FM?:{Player.CharacterModelIsFreeMode} PC?:{Player.CharacterModelIsPrimaryCharacter} voice:{Player.FreeModeVoice} RG: {Game.LocalPlayer.Character.RelationshipGroup.Name} DRG: {RelationshipGroup.Player.Name}", 5);
        //EntryPoint.WriteToConsole($"============================================ VEHICLES START", 5);
        //foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(Game.LocalPlayer.Character)))
        //{
        //    EntryPoint.WriteToConsole($"veh {veh.Vehicle.Handle} {veh.Vehicle.Model.Name} IsCar {veh.Vehicle.IsCar} Engine.IsRunning {veh.Engine.IsRunning} IsDriveable {veh.Vehicle.IsDriveable} IsLockedForPlayer {veh.Vehicle.IsLockedForPlayer(Game.LocalPlayer)} Gang? {veh.AssociatedGang?.ShortName} WasModSpawned {veh.WasModSpawned}", 5);
        //}
        //EntryPoint.WriteToConsole($"============================================ VEHICLES END", 5);
        EntryPoint.WriteToConsole($"============================================ CIVIES START", 5);
        foreach (PedExt ped in World.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
            EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} CanSeePlayer{ped.CanSeePlayer} MENU? {ped.HasMenu} IsUnconscious:{ped.IsUnconscious} Alive:{ped.Pedestrian.IsAlive} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} WillFightPolice {ped.WillFightPolice} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ CIVIES END", 5);
        EntryPoint.WriteToConsole($"============================================ SECURITY START", 5);
        foreach (SecurityGuard ped in World.Pedestrians.SecurityGuardList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
            string weaponinventorystring = "";
            if(ped.WeaponInventory.IsSetLessLethal)
            { 
                weaponinventorystring = "IsSetLessLethal";
            }
            else if (ped.WeaponInventory.IsSetDeadly)
            {
                weaponinventorystring = "IsSetDeadly";
            }
            else if (ped.WeaponInventory.IsSetUnarmed)
            {
                weaponinventorystring = "IsSetUnarmed";
            }
            else if (ped.WeaponInventory.IsSetDefault)
            {
                weaponinventorystring = "IsSetDefault";
            }
            if(ped.WeaponInventory.HasPistol)
            {
                weaponinventorystring += " Has Pistol";
            }
            if (ped.WeaponInventory.LongGun != null)
            {
                weaponinventorystring += " Has Long Gun";
            }
            string Text = $"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} CanSeePlayer{ped.CanSeePlayer} {ped.AssignedAgency?.ID} IsUnconscious:{ped.IsUnconscious} " +
                $"Alive:{ped.Pedestrian.IsAlive} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} " +
                $"IsInVehicle {ped.IsInVehicle} ReactionTier: {ped.PedReactions.ReactionTier} WeaponSet {weaponinventorystring} DebugWeaponState {ped.WeaponInventory.DebugWeaponState}" +
                $"Weapon {currentWeapon} Stunned {ped.Pedestrian.IsStunned} WasEverSetPersistent:{ped.WasEverSetPersistent} " +
                $"Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG} HasTaser {ped.HasTaser} SpawnRequirement {ped.LocationTaskRequirements.TaskRequirements}";

            if (ped.CurrentTask?.OtherTarget?.Pedestrian.Exists() == true)
            {
                Text += $" TASK Target:{ped.CurrentTask.OtherTarget.Pedestrian.Handle}";

            }



            EntryPoint.WriteToConsole(Text);


        }
        EntryPoint.WriteToConsole($"============================================ SECURITY END", 5);
        EntryPoint.WriteToConsole($"============================================ EMT START", 5);
        foreach (EMT ped in World.Pedestrians.EMTList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);







            string Text = $"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} {ped.AssignedAgency?.ID} CanSeePlayer{ped.CanSeePlayer} MENU? {ped.HasMenu} " +
                $"IsUnconscious:{ped.IsUnconscious} Alive:{ped.Pedestrian.IsAlive} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  " +
                $"PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} " +
                $"IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} " +
                $"Stunned {ped.Pedestrian.IsStunned} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} WasModSpawned:{ped.WasModSpawned} CanBeTasked:{ped.CanBeTasked} CanBeAmbientTasked:{ped.CanBeAmbientTasked} " +
                $"NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG} IsRespondingToInvestigation {ped.IsRespondingToInvestigation}";
            if (ped.CurrentTask?.OtherTarget?.Pedestrian.Exists() == true)
            {
                Text +=$" TASK Target:{ped.CurrentTask.OtherTarget.Pedestrian.Handle}";

            }

            EntryPoint.WriteToConsole(Text);



        }
        EntryPoint.WriteToConsole($"============================================ EMT END", 5);

        EntryPoint.WriteToConsole($"============================================ FIRE START", 5);
        foreach (Firefighter ped in World.Pedestrians.FirefighterList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);







            string Text = $"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} {ped.AssignedAgency?.ID} CanSeePlayer{ped.CanSeePlayer} MENU? {ped.HasMenu} " +
                $"IsUnconscious:{ped.IsUnconscious} Alive:{ped.Pedestrian.IsAlive} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  " +
                $"PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} " +
                $"IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} " +
                $"Stunned {ped.Pedestrian.IsStunned} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} WasModSpawned:{ped.WasModSpawned} CanBeTasked:{ped.CanBeTasked} CanBeAmbientTasked:{ped.CanBeAmbientTasked} " +
                $"NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG} IsRespondingToInvestigation {ped.IsRespondingToInvestigation}";

            if (ped.CurrentTask?.OtherTarget?.Pedestrian.Exists() == true)
            {
                Text += $" TASK Target:{ped.CurrentTask.OtherTarget.Pedestrian.Handle}";

            }

            EntryPoint.WriteToConsole(Text);



        }
        EntryPoint.WriteToConsole($"============================================ FIRE END", 5);

        EntryPoint.WriteToConsole($"============================================ GANGS START", 5);
        foreach (GangMember ped in World.Pedestrians.GangMemberList.Where(x => x.Pedestrian.Exists()).OrderBy(x=>x.WasModSpawned).ThenBy(x => x.DistanceToPlayer))
        { 
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);

            string weaponinventorystring = "";
            if (ped.WeaponInventory.IsSetLessLethal)
            {
                weaponinventorystring = "IsSetLessLethal";
            }
            else if (ped.WeaponInventory.IsSetDeadly)
            {
                weaponinventorystring = "IsSetDeadly";
            }
            else if (ped.WeaponInventory.IsSetUnarmed)
            {
                weaponinventorystring = "IsSetUnarmed";
            }
            else if (ped.WeaponInventory.IsSetDefault)
            {
                weaponinventorystring = "IsSetDefault";
            }

            if (ped.WeaponInventory.HasPistol)
            {
                weaponinventorystring += " Has Pistol";
            }
            if (ped.WeaponInventory.LongGun != null)
            {
                weaponinventorystring += " Has Long Gun";
            }
            weaponinventorystring += $" HasHeavyWeaponOnPerson {ped.WeaponInventory.HasHeavyWeaponOnPerson}";

            EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} CanSeePlayer{ped.CanSeePlayer} MENU? {ped.HasMenu} IsUnconscious:{ped.IsUnconscious} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} WasModSpawned {ped.WasModSpawned} Gang: {ped.Gang.ID} CanBeAmbientTasked {ped.CanBeAmbientTasked} CanBeTasked {ped.CanBeTasked} ", 5);
            EntryPoint.WriteToConsole($"     weaponinventorystring {weaponinventorystring}  SpawnRequirement {ped.LocationTaskRequirements.TaskRequirements} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} WorstObservedCrime {ped.PedViolations.WorstObservedCrime?.Name} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} Stunned {ped.Pedestrian.IsStunned}  WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG} CanBeTasked:{ped.CanBeTasked} CanBeAmbientTasked:{ped.CanBeAmbientTasked}", 5);

            // SpawnRequirement {ped.LocationTaskRequirements.TaskRequirements}";







        }
        EntryPoint.WriteToConsole($"============================================ GANGS END", 5);
        //EntryPoint.WriteToConsole($"============================================ ZOMBIES START", 5);
        //foreach (PedExt ped in World.ZombieList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 200f).OrderBy(x => x.DistanceToPlayer))
        //{
        //    uint currentWeapon;
        //    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
        //    uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
        //    EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} IsZombie {ped.IsZombie} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.ViolationWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} GroupName {ped.PedGroup?.InternalName} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG}", 5);
        //}
        //EntryPoint.WriteToConsole($"============================================ ZOMBIES END", 5);
        EntryPoint.WriteToConsole($"============================================ SERVICE START", 5);
        foreach (PedExt ped in World.Pedestrians.ServiceWorkers.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
            EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} " +
                $"HasWeapon: {ped.HasWeapon}  CanSeePlayer{ped.CanSeePlayer} MENU? {ped.HasMenu} IsUnconscious:{ped.IsUnconscious} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} " +
                $"WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} " +
                $"Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} " +
                $"Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG} WasModSpawned:{ped.WasModSpawned} CanConverse:{ped.CanConverse} Tasked{ped.CanBeTasked} AmbientTasked{ped.CanBeAmbientTasked}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ SERVICE END", 5);
        EntryPoint.WriteToConsole($"============================================ DEAD START", 5);
        foreach (PedExt ped in World.Pedestrians.DeadPeds.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
            EntryPoint.WriteToConsole($"DEAD !!! Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} {ped.Name} ${ped.Money} MENU? {ped.HasMenu} IsDead:{ped.Pedestrian.IsDead} LoggedDeath:{ped.CurrentHealthState.HasLoggedDeath} IsUnconscious:{ped.IsUnconscious} Task: {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} OtherCrimes {ped.OtherCrimesWitnessed.Count()}  PlayerCrimes {ped.PlayerCrimesWitnessed.Count()} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} IsInVehicle {ped.IsInVehicle} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.PedViolations.CurrentlyViolatingWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ DEAD END", 5);
        EntryPoint.WriteToConsole($"============================================ COPS START", 5);
        EntryPoint.WriteToConsole($"============================================ ClosestCop Handle {Player.ClosestCopToPlayer?.Handle}", 5);
        foreach (Cop cop in World.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists()).OrderBy(x=>x.DistanceToPlayer))
        {
            string VehString = "";
            string combat = "";
            if (cop.IsInVehicle && cop.Pedestrian.CurrentVehicle.Exists())
            {
                VehString = cop.Pedestrian.CurrentVehicle.Model.Name;
            }
            if (cop.Pedestrian.CombatTarget.Exists())
            {
                combat = " Combat: " + cop.Pedestrian.CombatTarget.Handle.ToString();
            }
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(cop.Pedestrian, out currentWeapon, true);
            string Weapon = $" Weapon: {currentWeapon}";
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(cop.Pedestrian, out currentVehicleWeapon);
            string VehicleWeapon = $" VehicleWeapon: Has {hasVehicleWeapon} : {currentVehicleWeapon}";
            uint coolHandle = 0;

            string weaponinventorystring = "";
            if (cop.WeaponInventory.IsSetLessLethal)
            {
                weaponinventorystring = "IsSetLessLethal";
            }
            else if (cop.WeaponInventory.IsSetDeadly)
            {
                weaponinventorystring = "IsSetDeadly";
            }
            else if (cop.WeaponInventory.IsSetUnarmed)
            {
                weaponinventorystring = "IsSetUnarmed";
            }
            else if (cop.WeaponInventory.IsSetDefault)
            {
                weaponinventorystring = "IsSetDefault";
            }

            if (cop.WeaponInventory.HasPistol)
            {
                weaponinventorystring += " Has Pistol";
            }
            if (cop.WeaponInventory.LongGun != null)
            {
                weaponinventorystring += " Has Long Gun";
            }
            weaponinventorystring += $" HasHeavyWeaponOnPerson {cop.WeaponInventory.HasHeavyWeaponOnPerson}";

            bool canSeePlayer = cop.CanSeePlayer;

            string retardedcops = $"IsDriver:{cop.IsDriver}";


            if (cop.CurrentTask?.OtherTarget?.Pedestrian.Exists() == true)
            {
                EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, cop.CellX, cop.CellY)}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} SeePlayer:{canSeePlayer} Name:{cop.Name} {cop.GroupName} ${cop.Money} " +
                    $"CanSeePlayer{cop.CanSeePlayer} weaponhash {currentWeapon} IsUnconscious:{cop.IsUnconscious} IsMale:{cop.Pedestrian.IsMale} " +
                    $"TaskStatus:{cop.Pedestrian.Tasks.CurrentTaskStatus} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} " +
                    $"Target:{cop.CurrentTask.OtherTarget.Pedestrian.Handle} IsRespondingToInvestigation {cop.IsRespondingToInvestigation} ");
                    EntryPoint.WriteToConsole($"IsRespondingToCitizenWanted {cop.IsRespondingToCitizenWanted} IsInVehicle {cop.IsInVehicle} Vehicle  {VehString} {combat} WEapon: {Weapon} {VehicleWeapon} " +
                    $"HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} RGotIn:{cop.RecentlyGotInVehicle} RGotOut:{cop.RecentlyGotOutOfVehicle} " +
                $"WeaponSet {weaponinventorystring} DebugWeaponState {cop.WeaponInventory.DebugWeaponState} {retardedcops} CanBeTasked:{cop.CanBeTasked} CanBeAmbientTasked:{cop.CanBeAmbientTasked}", 5);
            }

            else
            {
                EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}({cop.Handle})-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, cop.CellX, cop.CellY)}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} {cop.AssignedAgency?.ShortName} SeePlayer:{canSeePlayer} Name:{cop.Name} {cop.GroupName} ${cop.Money} " +
                    $"weaponhash {currentWeapon} IsUnconscious:{cop.IsUnconscious} IsMale:{cop.Pedestrian.IsMale} " +
                    $"TaskStatus:{cop.Pedestrian.Tasks.CurrentTaskStatus} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} " +
                    $"Target:{0} IsRespondingToInvestigation {cop.IsRespondingToInvestigation} ");
                EntryPoint.WriteToConsole($"IsRespondingToCitizenWanted {cop.IsRespondingToCitizenWanted} IsInVehicle {cop.IsInVehicle} Vehicle  {VehString} {combat} weapon: {Weapon} {VehicleWeapon} " +
                    $"HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} IsMarshalMember{cop.IsMarshalTaskForceMember} RGotIn:{cop.RecentlyGotInVehicle} RGotOut:{cop.RecentlyGotOutOfVehicle} WeaponSet {weaponinventorystring} DebugWeaponState {cop.WeaponInventory.DebugWeaponState} {retardedcops} CanBeTasked:{cop.CanBeTasked} CanBeAmbientTasked:{cop.CanBeAmbientTasked}", 5);
            }
            
        }
        EntryPoint.WriteToConsole($"============================================ COPS END", 5);
        EntryPoint.WriteToConsole($"============================================ K9 START", 5);
        foreach (Cop cop in World.Pedestrians.PoliceCanineList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            string VehString = "";
            string combat = "";
            if (cop.IsInVehicle && cop.Pedestrian.CurrentVehicle.Exists())
            {
                VehString = cop.Pedestrian.CurrentVehicle.Model.Name;
            }
            if (cop.Pedestrian.CombatTarget.Exists())
            {
                combat = " Combat: " + cop.Pedestrian.CombatTarget.Handle.ToString();
            }
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(cop.Pedestrian, out currentWeapon, true);
            string Weapon = $" Weapon: {currentWeapon}";
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(cop.Pedestrian, out currentVehicleWeapon);
            string VehicleWeapon = $" VehicleWeapon: Has {hasVehicleWeapon} : {currentVehicleWeapon}";
            uint coolHandle = 0;

            string weaponinventorystring = "";
            if (cop.WeaponInventory.IsSetLessLethal)
            {
                weaponinventorystring = "IsSetLessLethal";
            }
            else if (cop.WeaponInventory.IsSetDeadly)
            {
                weaponinventorystring = "IsSetDeadly";
            }
            else if (cop.WeaponInventory.IsSetUnarmed)
            {
                weaponinventorystring = "IsSetUnarmed";
            }
            else if (cop.WeaponInventory.IsSetDefault)
            {
                weaponinventorystring = "IsSetDefault";
            }

            if (cop.WeaponInventory.HasPistol)
            {
                weaponinventorystring += " Has Pistol";
            }
            if (cop.WeaponInventory.LongGun != null)
            {
                weaponinventorystring += " Has Long Gun";
            }


            if (cop.CurrentTask?.OtherTarget?.Pedestrian.Exists() == true)
            {
                EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, cop.CellX, cop.CellY)}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} Name:{cop.Name} {cop.GroupName} weaponhash {currentWeapon} IsUnconscious:{cop.IsUnconscious} IsMale:{cop.Pedestrian.IsMale} " +
                    $"TaskStatus:{cop.Pedestrian.Tasks.CurrentTaskStatus} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{cop.CurrentTask.OtherTarget.Pedestrian.Handle} IsRespondingToInvestigation {cop.IsRespondingToInvestigation} IsRespondingToCitizenWanted {cop.IsRespondingToCitizenWanted} IsInVehicle {cop.IsInVehicle} Vehicle  {VehString} {combat} WEapon: {Weapon} {VehicleWeapon} " +
                    $"HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} RGotIn:{cop.RecentlyGotInVehicle} RGotOut:{cop.RecentlyGotOutOfVehicle} WeaponSet {weaponinventorystring} DebugWeaponState {cop.WeaponInventory.DebugWeaponState}", 5);

            }

            else
            {
                EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, cop.CellX, cop.CellY)}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} Name:{cop.Name} {cop.GroupName} weaponhash {currentWeapon} IsUnconscious:{cop.IsUnconscious} IsMale:{cop.Pedestrian.IsMale} " +
                    $"TaskStatus:{cop.Pedestrian.Tasks.CurrentTaskStatus} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{0} IsRespondingToInvestigation {cop.IsRespondingToInvestigation} IsRespondingToCitizenWanted {cop.IsRespondingToCitizenWanted} IsInVehicle {cop.IsInVehicle} Vehicle  {VehString} {combat} weapon: {Weapon} {VehicleWeapon} " +
                    $"HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} RGotIn:{cop.RecentlyGotInVehicle} RGotOut:{cop.RecentlyGotOutOfVehicle} WeaponSet {weaponinventorystring} DebugWeaponState {cop.WeaponInventory.DebugWeaponState}", 5);
            }

        }
        EntryPoint.WriteToConsole($"============================================ K9 END", 5);


        EntryPoint.WriteToConsole($"============================================ POLICE VEHICLE START", 5);
        foreach(PoliceVehicleExt vehicleExt in World.Vehicles.PoliceVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ POLICE VEHICLE END", 5);

        EntryPoint.WriteToConsole($"============================================ EMS VEHICLE START", 5);
        foreach (EMSVehicleExt vehicleExt in World.Vehicles.EMSVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ EMS VEHICLE END", 5);

        EntryPoint.WriteToConsole($"============================================ FIRE VEHICLE START", 5);
        foreach (FireVehicleExt vehicleExt in World.Vehicles.FireVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ FIRE VEHICLE END", 5);


        EntryPoint.WriteToConsole($"============================================ SECURITY VEHICLE START", 5);
        foreach (SecurityVehicleExt vehicleExt in World.Vehicles.SecurityVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ SECURITY VEHICLE END", 5);

        EntryPoint.WriteToConsole($"============================================ GANG VEHICLE START", 5);
        foreach (GangVehicleExt vehicleExt in World.Vehicles.GangVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ GANG VEHICLE END", 5);

        EntryPoint.WriteToConsole($"============================================ TAXI VEHICLE START", 5);
        foreach (TaxiVehicleExt vehicleExt in World.Vehicles.TaxiVehicles)
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ TAXI VEHICLE END", 5);
        EntryPoint.WriteToConsole($"============================================ CIVILIAN VEHICLE START", 5);
        foreach (VehicleExt vehicleExt in World.Vehicles.CivilianVehicles.Where(x=> x.WasModSpawned))
        {
            EntryPoint.WriteToConsole($"{vehicleExt.GetDebugString()}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ CIVILIAN VEHICLE END", 5);

        EntryPoint.WriteToConsole($"============================================", 5);
        EntryPoint.WriteToConsole($"============================================", 5);
        EntryPoint.WriteToConsole($"Player.CurrentLookedAtPed?.Handle: {Player.CurrentLookedAtPed?.Handle}", 5);
        EntryPoint.WriteToConsole($"Player.CurrentLookedAtPed?.CanConverse: {Player.CurrentLookedAtPed?.CanConverse}", 5);
        EntryPoint.WriteToConsole($"DisplayablePlayer.CurrentLocation.CurrentInterior?.Name{Player.CurrentLocation?.CurrentInterior?.Name}", 5);
        EntryPoint.WriteToConsole($"Player.ActivityManager.CanConverseWithLookedAtPed{Player.ActivityManager.CanConverseWithLookedAtPed}", 5);
        //
        //DisplayablePlayer.CurrentLocation.CurrentInterior?.Name
        //Player.CurrentLookedAtPed
    }
    //private void WriteCopState()
    //{
    //    EntryPoint.WriteToConsole($"============================================ POLICE VEHICLES START", 2);
    //    foreach (VehicleExt veh in World.Vehicles.PoliceVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(Game.LocalPlayer.Character)))
    //    {
    //        EntryPoint.WriteToConsole($"veh {veh.Vehicle.Handle} {veh.Vehicle.Model.Name} IsPersistent {veh.Vehicle.IsPersistent} Position: {veh.Vehicle.Position}", 2);
    //    }
    //    EntryPoint.WriteToConsole($"============================================ POLICE VEHICLES END", 2);
    //    EntryPoint.WriteToConsole($"============================================ COPS START", 2);
    //    foreach (Cop cop in World.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists()))
    //    {
    //        string VehString = "";
    //        string combat = "";
    //        if (cop.IsInVehicle && cop.Pedestrian.CurrentVehicle.Exists())
    //        {
    //            VehString = cop.Pedestrian.CurrentVehicle.Model.Name;
    //        }
    //        if (cop.Pedestrian.CombatTarget.Exists())
    //        {
    //            combat = " Combat: " + cop.Pedestrian.CombatTarget.Handle.ToString();
    //        }
    //        uint currentWeapon;
    //        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(cop.Pedestrian, out currentWeapon, true);
    //        string Weapon = $" Weapon: {currentWeapon}";
    //        uint currentVehicleWeapon;
    //        bool hasVehicleWeapon = false;
    //        hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(cop.Pedestrian, out currentVehicleWeapon);
    //        string VehicleWeapon = $" VehicleWeapon: Has {hasVehicleWeapon} : {currentVehicleWeapon}";
    //        EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} {cop.AssignedAgency?.ShortName} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{cop.CurrentTask?.OtherTarget?.Handle} Vehicle {VehString} {combat} {Weapon} {VehicleWeapon} HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} Position: {cop.Pedestrian.Position} IsMarshalMember{cop.IsMarshalTaskForceMember}", 2);
    //    }
    //    EntryPoint.WriteToConsole($"============================================ COPS END", 2);
    //}
    private void SetIndex()
    {
        if (PlateIndex < 0)
        {
            return;
        }
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            Vehicle Car = Game.LocalPlayer.Character.CurrentVehicle;
            PlateType NewType = PlateTypes.GetPlateType(PlateIndex);
            if (NewType != null)
            {
                string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                if (NewPlateNumber != "")
                {
                    Car.LicensePlate = NewPlateNumber;
                }
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Car, NewType.Index);
                Game.DisplaySubtitle($" PlateIndex: {PlateIndex}, Index: {NewType.Index}, State: {NewType.StateID}, Description: {NewType.Description}");
            }
            else
            {
                Game.DisplaySubtitle($" PlateIndex: {PlateIndex} None Found");
            }
        }
    }
    //private void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded, bool StayStanding)
    //{
    //    //SubTaskName = "SetArrested";
    //    GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
    //    {
    //        AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
    //        AnimationDictionary.RequestAnimationDictionay("busted");
    //        AnimationDictionary.RequestAnimationDictionay("ped");

    //        if (!PedToArrest.Exists())
    //        {
    //            return;
    //        }

    //        while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
    //        {
    //            GameFiber.Yield();
    //        }

    //        if (!PedToArrest.Exists())
    //        {
    //            return;
    //        }


    //        if (PedToArrest.IsInAnyVehicle(false))
    //        {
    //            Vehicle oldVehicle = PedToArrest.CurrentVehicle;
    //            if (PedToArrest.Exists() && oldVehicle.Exists())
    //            {
    //                NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
    //                GameFiber.Wait(2500);
    //            }
    //        }

    //        NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
    //        //Ped.SetWantedLevel(0);

    //        if (StayStanding)
    //        {
    //            if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
    //            {
    //                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
    //            }
    //        }
    //        else
    //        {
    //            if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
    //            {
    //                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
    //                GameFiber.Wait(6000);

    //                if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
    //                {
    //                    return;
    //                }

    //                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
    //            }
    //        }
    //        PedToArrest.KeepTasks = true;

    //        if (MarkAsNoLongerNeeded)
    //        {
    //            PedToArrest.IsPersistent = false;
    //        }
    //        //GameTimeFinishedArrestedAnimation = Game.GameTime;
    //        //PlayedArrestAnimation = true;
    //        //EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);

    //    }, "SetArrestedAnimation");
    //}
    //public void UnSetArrestedAnimation(Ped PedToArrest)
    //{
    //    // SubTaskName = "UnSetArrested";
    //    GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
    //    {
    //        AnimationDictionary.RequestAnimationDictionay("random@arrests");
    //        AnimationDictionary.RequestAnimationDictionay("busted");
    //        AnimationDictionary.RequestAnimationDictionay("ped");
    //        AnimationDictionary.RequestAnimationDictionay("mp_arresting");
    //        if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3))
    //        {
    //            //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
    //            //GameFiber.Wait(1000);


    //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "hands_up_2_idle", 4.0f, -4.0f, -1, 4096, 0, 0, 1, 0);




    //            //while (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", PedToArrest, "random@arrests", "kneeling_arrest_escape") < 1.0f)
    //            //{
    //            //    GameFiber.Yield();
    //            //}





    //            GameFiber.Wait(2000);//1250

    //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 4.0f, -4.0f, -1, 50, 0, 0, 1, 0);
    //            GameFiber.Wait(7000);//1250
    //            NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
    //        }
    //        else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
    //        {
    //            NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
    //        }
    //        //PlayedUnArrestAnimation = true;
    //        //EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
    //    }, "UnSetArrestedAnimation");
    //}
    private void SetPlayerOffset()
    {
        ulong ModelHash = 0;
        if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Michael")
        {
            ModelHash = 225514697;
        }
        else if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Franklin")
        {
            ModelHash = 2602752943;
        }
        else if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Trevor")
        {
            ModelHash = 2608926626;
        }
        if (ModelHash != 0)
        {
            //bigbruh in discord, supplied the below, seems to work just fine
            unsafe
            {
                var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
                ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
                *((ulong*)(SkinPtr + 0x18)) = ModelHash;
            }
        }



        //unsafe
        //{
        //    var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
        //    ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
        //    *((ulong*)(SkinPtr + 0x18)) = (ulong)225514697;
        //}
    }
    //public void IssueWeapons(IWeapons weapons)
    //{
    //    IssuableWeapon Sidearm = Agencies.GetAgency("NOOSE").GetRandomWeapon(true, weapons);
    //    IssuableWeapon LongGun = Agencies.GetAgency("NOOSE").GetRandomWeapon(false, weapons);
    //    if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)WeaponHash.StunGun, false))
    //    {
    //        NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)WeaponHash.StunGun, 100, false, false);
    //    }
    //    if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)Sidearm.GetHash(), false))
    //    {
    //        NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)Sidearm.GetHash(), 200, false, false);
    //        Sidearm.ApplyVariation(Game.LocalPlayer.Character);
    //    }
    //    if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)LongGun.GetHash(), false))
    //    {
    //        NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)LongGun.GetHash(), 200, false, false);
    //        LongGun.ApplyVariation(Game.LocalPlayer.Character);
    //    }
    //   // NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Game.LocalPlayer.Character, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
    //  //  NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Game.LocalPlayer.Character, 2, true);//can do drivebys    
    //}
    private void SetupTimecycles()
    {
        Timecycles = new List<string>()
        { "AirRaceBoost01"
,"AirRaceBoost02"
,"AmbientPUSH"
,"AP1_01_B_IntRefRange"
,"AP1_01_C_NoFog"
,"ArenaEMP"
,"ArenaEMP_Blend"
,"ArenaWheelPurple01"
,"ArenaWheelPurple02"
,"Bank_HLWD"
,"Barry1_Stoned"
,"BarryFadeOut"
,"baseTONEMAPPING"
,"BeastIntro01"
,"BeastIntro02"
,"BeastLaunch01"
,"BeastLaunch02"
,"BikerFilter"
,"BikerForm01"
,"BikerFormFlash"
,"Bikers"
,"BikersSPLASH"
,"blackNwhite"
,"BlackOut"
,"BleepYellow01"
,"BleepYellow02"
,"Bloom"
,"BloomLight"
,"BloomMid"
,"BombCam01"
,"BombCamFlash"
,"Broken_camera_fuzz"
,"buggy_shack"
,"buildingTOP"
,"BulletTimeDark"
,"BulletTimeLight"
,"CAMERA_BW"
,"CAMERA_secuirity"
,"CAMERA_secuirity_FUZZ"
,"canyon_mission"
,"carMOD_underpass"
,"carpark"
,"carpark_dt1_02"
,"carpark_dt1_03"
,"Carpark_MP_exit"
,"cashdepot"
,"cashdepotEMERGENCY"
,"casino_brightroom"
,"casino_mainfloor"
,"casino_mainWhiteFloor"
,"casino_managementlobby"
,"casino_managementOff"
,"casino_managersoffice"
,"CasinoBathrooms"
,"cBank_back"
,"cBank_front"
,"ch2_tunnel_whitelight"
,"CH3_06_water"
,"CHOP"
,"cinema"
,"cinema_001"
,"cops"
,"CopsSPLASH"
,"crane_cam"
,"crane_cam_cinematic"
,"CrossLine01"
,"CrossLine02"
,"CS1_railwayB_tunnel"
,"CS3_rail_tunnel"
,"CUSTOM_streetlight"
,"damage"
,"DeadlineNeon01"
,"default"
,"DefaultColorCode"
,"dlc_casino_carpark"
,"DLC_Casino_Garage"
,"DONT_overide_sunpos"
,"Dont_tazeme_bro"
,"dont_tazeme_bro_b"
,"downtown_FIB_cascades_opt"
,"DrivingFocusDark"
,"DrivingFocusLight"
,"Drone_FishEye_Lens"
,"DRUG_2_drive"
,"Drug_deadman"
,"Drug_deadman_blend"
,"drug_drive_blend01"
,"drug_drive_blend02"
,"drug_flying_01"
,"drug_flying_02"
,"drug_flying_base"
,"DRUG_gas_huffin"
,"drug_wobbly"
,"Drunk"
,"dying"
,"eatra_bouncelight_beach"
,"epsilion"
,"exile1_exit"
,"exile1_plane"
,"ExplosionJosh"
,"EXT_FULLAmbientmult_art"
,"ext_int_extlight_large"
,"EXTRA_bouncelight"
,"eyeINtheSKY"
,"Facebook_NEW"
,"facebook_serveroom"
,"FIB_5"
,"FIB_6"
,"FIB_A"
,"FIB_B"
,"FIB_interview"
,"FIB_interview_optimise"
,"FinaleBank"
,"FinaleBankexit"
,"FinaleBankMid"
,"fireDEPT"
,"FORdoron_delete"
,"Forest"
,"fp_vig_black"
,"fp_vig_blue"
,"fp_vig_brown"
,"fp_vig_gray"
,"fp_vig_green"
,"fp_vig_red"
,"FrankilinsHOUSEhills"
,"frankilnsAUNTS_new"
,"frankilnsAUNTS_SUNdir"
,"FRANKLIN"
,"FranklinColorCode"
,"FranklinColorCodeBasic"
,"FranklinColorCodeBright"
,"FullAmbientmult_interior"
,"gallery_refmod"
,"garage"
,"gen_bank"
,"glasses_black"
,"Glasses_BlackOut"
,"glasses_blue"
,"glasses_brown"
,"glasses_Darkblue"
,"glasses_green"
,"glasses_orange"
,"glasses_pink"
,"glasses_purple"
,"glasses_red"
,"glasses_Scuba"
,"glasses_VISOR"
,"glasses_yellow"
,"gorge_reflection_gpu"
,"gorge_reflectionoffset"
,"gorge_reflectionoffset2"
,"graveyard_shootout"
,"grdlc_int_02"
,"grdlc_int_02_trailer_cave"
,"gunclub"
,"gunclubrange"
,"gunshop"
,"gunstore"
,"half_direct"
,"hangar_lightsmod"
,"Hanger_INTmods"
,"heathaze"
,"heist_boat"
,"heist_boat_engineRoom"
,"heist_boat_norain"
,"helicamfirst"
,"heliGunCam"
,"Hicksbar"
,"HicksbarNEW"
,"hillstunnel"
,"Hint_cam"
,"hitped"
,"hud_def_blur"
,"hud_def_blur_switch"
,"hud_def_colorgrade"
,"hud_def_desat_cold"
,"hud_def_desat_cold_kill"
,"hud_def_desat_Franklin"
,"hud_def_desat_Michael"
,"hud_def_desat_Neutral"
,"hud_def_desat_switch"
,"hud_def_desat_Trevor"
,"hud_def_desatcrunch"
,"hud_def_flash"
,"hud_def_focus"
,"hud_def_Franklin"
,"hud_def_lensdistortion"
,"hud_def_lensdistortion_subtle"
,"hud_def_Michael"
,"hud_def_Trevor"
,"ImpExp_Interior_01"
,"IMpExt_Interior_02"
,"IMpExt_Interior_02_stair_cage"
,"InchOrange01"
,"InchOrange02"
,"InchPickup01"
,"InchPickup02"
,"InchPurple01"
,"InchPurple02"
,"INT_FullAmbientmult"
,"INT_FULLAmbientmult_art"
,"INT_FULLAmbientmult_both"
,"INT_garage"
,"INT_mall"
,"INT_NO_fogALPHA"
,"INT_NoAmbientmult"
,"INT_NoAmbientmult_art"
,"INT_NoAmbientmult_both"
,"INT_NOdirectLight"
,"INT_nowaterREF"
,"INT_posh_hairdresser"
,"INT_smshop"
,"INT_smshop_indoor_bloom"
,"INT_smshop_inMOD"
,"INT_smshop_outdoor_bloom"
,"INT_streetlighting"
,"INT_trailer_cinema"
,"id1_11_tunnel"
,"impexp_interior_01_lift"
,"int_amb_mult_large"
,"int_arena_01"
,"int_arena_Mod"
,"int_arena_Mod_garage"
,"int_arena_VIP"
,"int_Barber1"
,"int_carmod_small"
,"int_carrier_control"
,"int_carrier_control_2"
,"int_carrier_hanger"
,"int_carrier_rear"
,"int_carrier_stair"
,"int_carshowroom"
,"int_chopshop"
,"int_clean_extlight_large"
,"int_clean_extlight_none"
,"int_clean_extlight_small"
,"int_ClothesHi"
,"int_clotheslow_large"
,"int_cluckinfactory_none"
,"int_cluckinfactory_small"
,"int_ControlTower_none"
,"int_ControlTower_small"
,"int_dockcontrol_small"
,"int_extlght_sm_cntrst"
,"int_extlight_large"
,"int_extlight_large_fog"
,"int_extlight_none"
,"int_extlight_none_dark"
,"int_extlight_none_dark_fog"
,"int_extlight_none_fog"
,"int_extlight_small"
,"int_extlight_small_clipped"
,"int_extlight_small_fog"
,"int_Farmhouse_none"
,"int_Farmhouse_small"
,"int_FranklinAunt_small"
,"int_GasStation"
,"int_hanger_none"
,"int_hanger_small"
,"int_Hospital_Blue"
,"int_Hospital_BlueB"
,"int_hospital_dark"
,"int_Hospital_DM"
,"int_hospital_small"
,"int_Hospital2_DM"
,"int_lesters"
,"int_Lost_none"
,"int_Lost_small"
,"int_methlab_small"
,"int_motelroom"
,"int_office_Lobby"
,"int_office_LobbyHall"
,"int_tattoo"
,"int_tattoo_B"
,"int_tunnel_none_dark"
,"interior_WATER_lighting"
,"introblue"
,"jewel_gas"
,"jewel_optim"
,"jewelry_entrance"
,"jewelry_entrance_INT"
,"jewelry_entrance_INT_fog"
,"Kifflom"
,"KT_underpass"
,"lab_none"
,"lab_none_dark"
,"lab_none_dark_fog"
,"lab_none_dark_OVR"
,"lab_none_exit"
,"lab_none_exit_OVR"
,"LectroDark"
,"LectroLight"
,"LIGHTSreduceFALLOFF"
,"li"
,"LifeInvaderLOD"
,"lightning"
,"lightning_cloud"
,"lightning_strong"
,"lightning_weak"
,"LightPollutionHills"
,"lightpolution"
,"LODmult_global_reduce"
,"LODmult_global_reduce_NOHD"
,"LODmult_HD_orphan_LOD_reduce"
,"LODmult_HD_orphan_reduce"
,"LODmult_LOD_reduce"
,"LODmult_SLOD1_reduce"
,"LODmult_SLOD2_reduce"
,"LODmult_SLOD3_reduce"
,"lodscaler"
,"LostTimeDark"
,"LostTimeFlash"
,"LostTimeLight"
,"maxlodscaler"
,"metro"
,"METRO_platform"
,"METRO_Tunnels"
,"METRO_Tunnels_entrance"
,"MichaelColorCode"
,"MichaelColorCodeBasic"
,"MichaelColorCodeBright"
,"MichaelsDarkroom"
,"MichaelsDirectional"
,"MichaelsNODirectional"
,"micheal"
,"micheals_lightsOFF"
,"michealspliff"
,"michealspliff_blend"
,"michealspliff_blend02"
,"militarybase_nightlight"
,"mineshaft"
,"morebloom"
,"morgue_dark"
,"morgue_dark_ovr"
,"Mp_apart_mid"
,"MP_Arena_theme_atlantis"
,"MP_Arena_theme_evening"
,"MP_Arena_theme_hell"
,"MP_Arena_theme_midday"
,"MP_Arena_theme_morning"
,"MP_Arena_theme_night"
,"MP_Arena_theme_saccharine"
,"MP_Arena_theme_sandstorm"
,"MP_Arena_theme_scifi_night"
,"MP_Arena_theme_storm"
,"MP_Arena_theme_toxic"
,"MP_Arena_VIP"
,"mp_battle_int01"
,"mp_battle_int01_dancefloor"
,"mp_battle_int01_dancefloor_OFF"
,"mp_battle_int01_entry"
,"mp_battle_int01_garage"
,"mp_battle_int01_office"
,"mp_battle_int02"
,"mp_battle_int03"
,"mp_battle_int03_tint1"
,"mp_battle_int03_tint2"
,"mp_battle_int03_tint3"
,"mp_battle_int03_tint4"
,"mp_battle_int03_tint5"
,"mp_battle_int03_tint6"
,"mp_battle_int03_tint7"
,"mp_battle_int03_tint8"
,"mp_battle_int03_tint9"
,"mp_bkr_int01_garage"
,"mp_bkr_int01_small_rooms"
,"mp_bkr_int01_transition"
,"mp_bkr_int02_garage"
,"mp_bkr_int02_hangout"
,"mp_bkr_int02_small_rooms"
,"mp_bkr_ware01"
,"mp_bkr_ware02_dry"
,"mp_bkr_ware02_standard"
,"mp_bkr_ware02_upgrade"
,"mp_bkr_ware03_basic"
,"mp_bkr_ware03_upgrade"
,"mp_bkr_ware04"
,"mp_bkr_ware05"
,"MP_Bull_tost"
,"MP_Bull_tost_blend"
,"MP_casino_apartment_bar"
,"MP_casino_apartment_barPARTY"
,"MP_casino_apartment_barPARTY_0"
,"MP_casino_apartment_barPARTY_01"
,"MP_casino_apartment_barPARTY_2"
,"MP_casino_apartment_Bath"
,"MP_casino_apartment_changing"
,"MP_casino_apartment_cinema"
,"MP_casino_apartment_colour0"
,"MP_casino_apartment_colour1"
,"MP_casino_apartment_colour2"
,"MP_casino_apartment_exec"
,"MP_casino_apartment_lobby"
,"MP_casino_apartment_lounge"
,"MP_casino_apartment_MBed"
,"MP_casino_apartment_office"
,"MP_casino_apartment_spa"
,"MP_corona_heist"
,"MP_corona_heist_blend"
,"MP_corona_heist_BW"
,"MP_corona_heist_BW_night"
,"MP_corona_heist_DOF"
,"MP_corona_heist_night"
,"MP_corona_heist_night_blend"
,"MP_corona_selection"
,"MP_corona_switch"
,"MP_corona_tournament"
,"MP_corona_tournament_DOF"
,"MP_death_grade"
,"MP_death_grade_blend01"
,"MP_death_grade_blend02"
,"MP_deathfail_night"
,"mp_exec_office_01"
,"mp_exec_office_02"
,"mp_exec_office_03"
,"mp_exec_office_03_blue"
,"mp_exec_office_03C"
,"mp_exec_office_04"
,"mp_exec_office_05"
,"mp_exec_office_06"
,"mp_exec_warehouse_01"
,"MP_Garage_L"
,"mp_gr_int01_black"
,"mp_gr_int01_grey"
,"mp_gr_int01_white"
,"MP_H_01_Bathroom"
,"MP_H_01_Bedroom"
,"MP_H_01_New"
,"MP_H_01_New_Bathroom"
,"MP_H_01_New_Bedroom"
,"MP_H_01_New_Study"
,"MP_H_01_Study"
,"MP_H_02"
,"MP_H_04"
,"mp_h_05"
,"MP_H_06"
,"mp_h_07"
,"mp_h_08"
,"MP_heli_cam"
,"mp_imx_intwaremed"
,"mp_imx_intwaremed_office"
,"mp_imx_mod_int_01"
,"MP_intro_logo"
,"MP_job_end_night"
,"MP_job_load"
,"MP_job_load_01"
,"MP_job_load_02"
,"MP_job_lose"
,"MP_job_preload"
,"MP_job_preload_blend"
,"MP_job_preload_night"
,"MP_job_win"
,"MP_Killstreak"
,"MP_Killstreak_blend"
,"mp_lad_day"
,"mp_lad_judgment"
,"mp_lad_night"
,"MP_Loser"
,"MP_Loser_blend"
,"MP_lowgarage"
,"MP_MedGarage"
,"mp_nightshark_shield_fp"
,"MP_Powerplay"
,"MP_Powerplay_blend"
,"MP_race_finish"
,"MP_select"
,"mp_smg_int01_han"
,"mp_smg_int01_han_blue"
,"mp_smg_int01_han_red"
,"mp_smg_int01_han_yellow"
,"Mp_Stilts"
,"Mp_Stilts_gym"
,"Mp_Stilts_gym2"
,"Mp_Stilts2"
,"Mp_Stilts2_bath"
,"MP_Studio_Lo"
,"mp_x17dlc_base"
,"mp_x17dlc_base_dark"
,"mp_x17dlc_base_darkest"
,"mp_x17dlc_facility"
,"mp_x17dlc_facility_conference"
,"mp_x17dlc_facility2"
,"mp_x17dlc_in_sub"
,"mp_x17dlc_in_sub_no_reflection"
,"mp_x17dlc_int_01"
,"mp_x17dlc_int_01_tint1"
,"mp_x17dlc_int_01_tint2"
,"mp_x17dlc_int_01_tint3"
,"mp_x17dlc_int_01_tint4"
,"mp_x17dlc_int_01_tint5"
,"mp_x17dlc_int_01_tint6"
,"mp_x17dlc_int_01_tint7"
,"mp_x17dlc_int_01_tint8"
,"mp_x17dlc_int_01_tint9"
,"mp_x17dlc_int_02"
,"mp_x17dlc_int_02_hangar"
,"mp_x17dlc_int_02_outdoor_intro_camera"
,"mp_x17dlc_int_02_tint1"
,"mp_x17dlc_int_02_tint2"
,"mp_x17dlc_int_02_tint3"
,"mp_x17dlc_int_02_tint4"
,"mp_x17dlc_int_02_tint5"
,"mp_x17dlc_int_02_tint6"
,"mp_x17dlc_int_02_tint7"
,"mp_x17dlc_int_02_tint8"
,"mp_x17dlc_int_02_tint9"
,"mp_x17dlc_int_02_vehicle_avenger_camera"
,"mp_x17dlc_int_02_vehicle_workshop_camera"
,"mp_x17dlc_int_02_weapon_avenger_camera"
,"mp_x17dlc_int_silo"
,"mp_x17dlc_int_silo_escape"
,"mp_x17dlc_lab"
,"mp_x17dlc_lab_loading_bay"
,"MPApart_H_01"
,"MPApart_H_01_gym"
,"MPApartHigh"
,"MPApartHigh_palnning"
,"mugShot"
,"mugShot_lineup"
,"Multipayer_spectatorCam"
,"multiplayer_ped_fight"
,"nervousRON_fog"
,"NeutralColorCode"
,"NeutralColorCodeBasic"
,"NeutralColorCodeBright"
,"NeutralColorCodeLight"
,"NEW_abattoir"
,"new_bank"
,"NEW_jewel"
,"NEW_jewel_EXIT"
,"NEW_lesters"
,"new_MP_Garage_L"
,"NEW_ornate_bank"
,"NEW_ornate_bank_entrance"
,"NEW_ornate_bank_office"
,"NEW_ornate_bank_safe"
,"New_sewers"
,"NEW_shrinksOffice"
,"NEW_station_unfinished"
,"new_stripper_changing"
,"NEW_trevorstrailer"
,"NEW_tunnels"
,"NEW_tunnels_ditch"
,"new_tunnels_entrance"
,"NEW_tunnels_hole"
,"NEW_yellowtunnels"
,"NewMicheal"
,"NewMicheal_night"
,"NewMicheal_upstairs"
,"NewMichealgirly"
,"NewMichealstoilet"
,"NewMichealupstairs"
,"NewMod"
,"nextgen"
,"NG_blackout"
,"NG_deathfail_BW_base"
,"NG_deathfail_BW_blend01"
,"NG_deathfail_BW_blend02"
,"NG_filmic01"
,"NG_filmic02"
,"NG_filmic03"
,"NG_filmic04"
,"NG_filmic05"
,"NG_filmic06"
,"NG_filmic07"
,"NG_filmic08"
,"NG_filmic09"
,"NG_filmic10"
,"NG_filmic11"
,"NG_filmic12"
,"NG_filmic13"
,"NG_filmic14"
,"NG_filmic15"
,"NG_filmic16"
,"NG_filmic17"
,"NG_filmic18"
,"NG_filmic19"
,"NG_filmic20"
,"NG_filmic21"
,"NG_filmic22"
,"NG_filmic23"
,"NG_filmic24"
,"NG_filmic25"
,"NG_filmnoir_BW01"
,"NG_filmnoir_BW02"
,"NG_first"
,"nightvision"
,"NO_coronas"
,"NO_fog_alpha"
,"NO_streetAmbient"
,"NO_weather"
,"NoAmbientmult"
,"NoAmbientmult_interior"
,"NOdirectLight"
,"NoPedLight"
,"NOrain"
,"OrbitalCannon"
,"overwater"
,"Paleto"
,"paleto_nightlight"
,"paleto_opt"
,"PennedInDark"
,"PennedInLight"
,"PERSHING_water_reflect"
,"phone_cam"
,"phone_cam1"
,"phone_cam10"
,"phone_cam11"
,"phone_cam12"
,"phone_cam13"
,"phone_cam2"
,"phone_cam3"
,"phone_cam3_REMOVED"
,"phone_cam4"
,"phone_cam5"
,"phone_cam6"
,"phone_cam7"
,"phone_cam8"
,"phone_cam8_REMOVED"
,"phone_cam9"
,"plane_inside_mode"
,"player_transition"
,"player_transition_no_scanlines"
,"player_transition_scanlines"
,"PlayerSwitchNeutralFlash"
,"PlayerSwitchPulse"
,"plaza_carpark"
,"PoliceStation"
,"PoliceStationDark"
,"polluted"
,"poolsidewaterreflection2"
,"PORT_heist_underwater"
,"powerplant_nightlight"
,"powerstation"
,"PPFilter"
,"PPGreen01"
,"PPGreen02"
,"PPOrange01"
,"PPOrange02"
,"PPPink01"
,"PPPink02"
,"PPPurple01"
,"PPPurple02"
,"prison_nightlight"
,"projector"
,"prologue"
,"prologue_ending_fog"
,"prologue_ext_art_amb"
,"prologue_reflection_opt"
,"prologue_shootout"
,"Prologue_shootout_opt"
,"pulse"
,"RaceTurboDark"
,"RaceTurboFlash"
,"RaceTurboLight"
,"ranch"
,"REDMIST"
,"REDMIST_blend"
,"ReduceDrawDistance"
,"ReduceDrawDistanceMAP"
,"ReduceDrawDistanceMission"
,"reducelightingcost"
,"ReduceSSAO"
,"reducewaterREF"
,"refit"
,"reflection_correct_ambient"
,"RemixDrone"
,"RemoteSniper"
,"resvoire_reflection"
,"rply_brightness"
,"rply_brightness_neg"
,"rply_contrast"
,"rply_contrast_neg"
,"rply_motionblur"
,"rply_saturation"
,"rply_saturation_neg"
,"rply_vignette"
,"rply_vignette_neg"
,"SALTONSEA"
,"sandyshore_nightlight"
,"SAWMILL"
,"scanline_cam"
,"scanline_cam_cheap"
,"scope_zoom_in"
,"scope_zoom_out"
,"secret_camera"
,"services_nightlight"
,"shades_pink"
,"shades_yellow"
,"SheriffStation"
,"ship_explosion_underwater"
,"ship_explosion_underwater"
,"ship_lighting"
,"Shop247"
,"Shop247_none"
,"sleeping"
,"SmugglerCheckpoint01"
,"SmugglerCheckpoint02"
,"SmugglerFlash"
,"Sniper"
,"SP1_03_drawDistance"
,"spectator1"
,"spectator10"
,"spectator2"
,"spectator3"
,"spectator4"
,"spectator5"
,"spectator6"
,"spectator7"
,"spectator8"
,"spectator9"
,"StadLobby"
,"stc_coroners"
,"stc_deviant_bedroom"
,"stc_deviant_lounge"
,"stc_franklinsHouse"
,"stc_trevors"
,"stoned"
,"stoned_aliens"
,"stoned_cutscene"
,"stoned_monkeys"
,"StreetLighting"
,"StreetLightingJunction"
,"StreetLightingtraffic"
,"STRIP_changing"
,"STRIP_nofog"
,"STRIP_office"
,"STRIP_stage"
,"StuntFastDark"
,"StuntFastLight"
,"StuntSlowDark"
,"StuntSlowLight"
,"subBASE_water_ref"
,"sunglasses"
,"superDARK"
,"switch_cam_1"
,"switch_cam_2"
,"telescope"
,"TinyGreen01"
,"TinyGreen02"
,"TinyPink01"
,"TinyPink02"
,"TinyRacerMoBlur"
,"torpedo"
,"traffic_skycam"
,"trailer_explosion_optimise"
,"TransformFlash"
,"TransformRaceFlash"
,"TREVOR"
,"TrevorColorCode"
,"TrevorColorCodeBasic"
,"TrevorColorCodeBright"
,"Trevors_room"
,"trevorspliff"
,"trevorspliff_blend"
,"trevorspliff_blend02"
,"Tunnel"
,"tunnel_entrance"
,"tunnel_entrance_INT"
,"TUNNEL_green"
,"TUNNEL_green_ext"
,"Tunnel_green1"
,"tunnel_id1_11"
,"TUNNEL_orange"
,"TUNNEL_orange_exterior"
,"TUNNEL_white"
,"TUNNEL_yellow"
,"TUNNEL_yellow_ext"
,"ufo"
,"ufo_deathray"
,"underwater"
,"underwater_deep"
,"underwater_deep_clear"
,"v_abattoir"
,"V_Abattoir_Cold"
,"v_bahama"
,"v_cashdepot"
,"V_CIA_Facility"
,"v_dark"
,"V_FIB_IT3"
,"V_FIB_IT3_alt"
,"V_FIB_IT3_alt5"
,"V_FIB_stairs"
,"v_foundry"
,"v_janitor"
,"v_jewel2"
,"v_metro"
,"V_Metro_station"
,"V_Metro2"
,"v_michael"
,"v_michael_lounge"
,"V_Office_smoke"
,"V_Office_smoke_ext"
,"V_Office_smoke_Fire"
,"v_recycle"
,"V_recycle_dark"
,"V_recycle_light"
,"V_recycle_mainroom"
,"v_rockclub"
,"V_Solomons"
,"V_strip_nofog"
,"V_strip_office"
,"v_strip3"
,"v_strpchangerm"
,"v_sweat"
,"v_sweat_entrance"
,"v_sweat_NoDirLight"
,"v_torture"
,"Vagos"
,"vagos_extlight_small"
,"VAGOS_new_garage"
,"VAGOS_new_hangout"
,"VagosSPLASH"
,"VC_tunnel_entrance"
,"vehicle_subint"
,"venice_canal_tunnel"
,"vespucci_garage"
,"VolticBlur"
,"VolticFlash"
,"VolticGold"
,"warehouse"
,"WAREHOUSE"
,"WarpCheckpoint"
,"WATER _lab_cooling"
,"WATER_CH2_06_01_03"
,"WATER_CH2_06_02"
,"WATER_CH2_06_04"
,"WATER_cove"
,"WATER_hills"
,"WATER_ID2_21"
,"WATER_lab"
,"WATER_lab_cooling"
,"WATER_militaryPOOP"
,"WATER_muddy"
,"WATER_port"
,"WATER_REF_malibu"
,"WATER_refmap_high"
,"WATER_refmap_hollywoodlake"
,"WATER_refmap_low"
,"WATER_refmap_med"
,"WATER_refmap_off"
,"WATER_refmap_poolside"
,"WATER_refmap_silverlake"
,"WATER_refmap_venice"
,"WATER_refmap_verylow"
,"WATER_resevoir"
,"WATER_RichmanStuntJump"
,"WATER_river"
,"WATER_salton"
,"WATER_salton_bottom"
,"WATER_shore"
,"WATER_silty"
,"WATER_silverlake"
,"WeaponUpgrade"
,"whitenightlighting"
,"WhiteOut"};
    }
    private void AnimationTester()
    {
        string dictionary = NativeHelper.GetKeyboardInput("veh@std@ds@enter_exit");
        string animation = NativeHelper.GetKeyboardInput("d_close_out");
        AnimationDictionary.RequestAnimationDictionay(dictionary);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 8.0f, -8.0f, -1, (int)(AnimationFlags.Loop), 0, false, false, false);//-1
        GameFiber.Sleep(5000);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }

    private void ArrestScene()
    {


        GameFiber.StartNew(delegate
        {
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(0.5f));
            GameFiber.Yield();

            GameFiber.Sleep(1000);
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;





                string Dictionary =  "mp_arrest_paired";
                string PlayerAnimation = "cop_p1_rf_right_0";
                string DriverAnimation = "crook_p1_right";


                Dictionary = "mp_arresting";
                PlayerAnimation = "arrest_on_floor_back_right_a";
                DriverAnimation = "arrest_on_floor_back_right_b";

                Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
                float PlayerHeading = Game.LocalPlayer.Character.Heading;



                int PlayerScene;
                int DriverScene;
                AnimationDictionary.RequestAnimationDictionay(Dictionary);


                //coolguy.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(1f);
                coolguy.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(0.5f);
                coolguy.Heading = PlayerHeading;


                Vector3 DriverPos = coolguy.Position;
                float DriverHeading = coolguy.Heading;


                PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 0.0f, 0.0f, PlayerHeading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Dictionary, PlayerAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                DriverScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverPos.X, DriverPos.Y, DriverPos.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", DriverScene, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", coolguy, DriverScene, Dictionary, DriverAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", DriverScene, 0.0f);
                while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) < 1.0f && coolguy.Exists())
                {
                    //float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                    GameFiber.Yield();
                }
                //EntryPoint.WriteToConsoleTestLong("Arrest Test First Phase Over");
                //if (coolguy.Exists())
                //{
                //    //PlayerAnimation = "cop_p2_back_right";
                //    //DriverAnimation = "crook_p2_back_right";
                //    PlayerAnimation = "cop_p2_back_right";
                //    DriverAnimation = "crook_p2_back_right";
                //    PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 0.0f, 0.0f, PlayerHeading, 2);//270f //old
                //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                //    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Dictionary, PlayerAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                //    DriverScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverPos.X, DriverPos.Y, DriverPos.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", DriverScene, false);
                //    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", coolguy, DriverScene, Dictionary, DriverAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", DriverScene, 0.0f);
                //    while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) < 1.0f && coolguy.Exists())
                //    {
                //        //float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                //        GameFiber.Yield();
                //    }
                //    EntryPoint.WriteToConsole("Arrest Test Second Phase Over");
                //    //if (coolguy.Exists())
                //    //{
                //    //    PlayerAnimation = "cop_p3_fwd";
                //    //    DriverAnimation = "crook_p3";
                //    //    PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 0.0f, 0.0f, PlayerHeading, 2);//270f //old
                //    //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
                //    //    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Dictionary, PlayerAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                //    //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
                //    //    DriverScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverPos.X, DriverPos.Y, DriverPos.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                //    //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", DriverScene, false);
                //    //    NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", coolguy, DriverScene, Dictionary, DriverAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                //    //    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", DriverScene, 0.0f);
                //    //    while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) < 1.0f && coolguy.Exists())
                //    //    {
                //    //        //float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
                //    //        GameFiber.Yield();
                //    //    }
                //    //    EntryPoint.WriteToConsole("Arrest Test Third Phase Over");
                //    //}
                //}

            }

            if (coolguy.Exists())
            {
                coolguy.Delete();
            }
            Game.LocalPlayer.Character.Tasks.Clear();

        }, "Run Debug Logic");






    }


    private void HighlightProp()
    {

        Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 10f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
        if (ClosestEntity.Exists())
        {
            Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
            EntryPoint.WriteToConsole($"Closest Object = {ClosestEntity.Model.Name} {ClosestEntity.Model.Hash}", 5);
            EntryPoint.WriteToConsole($"Closest Object X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z} new Vector3({ClosestEntity.Position.X}f,{ClosestEntity.Position.Y}f,{ClosestEntity.Position.Z}f)", 5);
            uint GameTimeStartedDisplaying = Game.GameTime;
            while (Game.GameTime - GameTimeStartedDisplaying <= 5000)
            {
                Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
                GameFiber.Yield();
            }

        }
    }

    private void SpawnWithQuat()
    {
        Quaternion qt = new Quaternion(new Vector3(0f, 0f, 0.9473041f), 0.3203356f);
        Vector3 tospawn = new Vector3(407.7554f, -984.2084f, 29.89806f);

        tospawn = new Vector3(427.5909f, -1027.707f, 29.22805f);
        qt = new Quaternion(tospawn + new Vector3(0f, 0f, -0.7058839f), 0.7083276f);
        

        //EntryPoint.WriteToConsoleTestLong($"Angle {qt.Angle}");

       // qt.Normalize();
        //qt.Invert();
        //qt.Invert();


        Vector3 Rotations = GetEulerAngles(qt);


        float DegreeAngle = qt.Angle * 180f / (float)Math.PI;
        //EntryPoint.WriteToConsoleTestLong($"Angle degree {DegreeAngle} Rotations {Rotations}");
        Vehicle imcool = new Vehicle("police2", tospawn, Rotations.Z);

        // imcool.Orientation = qt;


        


        GameFiber.Sleep(500);
        if(imcool.Exists())
        {
            //EntryPoint.WriteToConsoleTestLong($"imcool {imcool.Heading}");
        }


        GameFiber.Sleep(10000);
        if(imcool.Exists())
        {
            imcool.Delete();
        }
    }
    private Vector3 GetEulerAngles(Quaternion q)
    {
        var x = q.X;
        var y = q.Y;
        var z = q.Z;
        var w = q.W;
        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;
        var ls = xx + yy + zz + ww;
        var st = x * w - y * z;
        var sv = ls * 0.499f;
        var rd = 180.0f / (float)Math.PI;
        if (st > sv)
        {
            return new Vector3(90, (float)Math.Atan2(y, x) * 2.0f * rd, 0);
        }
        else if (st < -sv)
        {
            return new Vector3(-90, (float)Math.Atan2(y, x) * -2.0f * rd, 0);
        }
        else
        {
            return new Vector3(
                (float)Math.Asin(2.0f * st) * rd,
                (float)Math.Atan2(2.0f * (y * w + x * z), 1.0f - 2.0f * (xx + yy)) * rd,
                (float)Math.Atan2(2.0f * (x * y + z * w), 1.0f - 2.0f * (xx + zz)) * rd
                );
        }
    }
    public Vector3 ToEulerAngles(Quaternion q)
    {
        Vector3 angles = new Vector3();

        // roll / x
        double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
        double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch / y
        double sinp = 2 * (q.W * q.Y - q.Z * q.X);
        if (Math.Abs(sinp) >= 1)
        {
            if(sinp < 0)
            {
                angles.Y = -1.0f * (float)Math.PI / 2;
            }
            else
            {
                angles.Y = (float)Math.PI / 2;
            }


            //angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
        }
        else
        {
            angles.Y = (float)Math.Asin(sinp);
        }

        // yaw / z
        double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
        double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

        return angles;
    }

    private void Test222()
    {
        //seems to work when in cover and using ANY WEAPON, need to make sure its just the ones we want!
        //"cover@move@ai@base@1h" two handed pistol, held with both hands, does some transition stuffo
        //"cover@move@ai@base@2h" two handed long gun, held with both hands
        string animSet = "cover@move@ai@base@1h";
        if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(animSet))
        {
            NativeFunction.Natives.REQUEST_ANIM_SET<bool>(animSet);
            GameFiber.Sleep(200);
        }

        if (!OnOff1)
        {
            NativeFunction.Natives.SET_PED_MOTION_IN_COVER_CLIPSET_OVERRIDE(Game.LocalPlayer.Character, animSet);
            Game.DisplaySubtitle("SET CLIPSET");
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_MOTION_IN_COVER_CLIPSET_OVERRIDE(Game.LocalPlayer.Character);
            Game.DisplaySubtitle("RESET CLIPSET");
        }
        OnOff1 = !OnOff1;
    }
    public class InteriorPosition
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public InteriorPosition()
        {

        }

        public InteriorPosition(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }
    }
    enum eTaskTypeIndex
    {
 //    CTaskHandsUp = 0,
	//CTaskClimbLadder = 1,
	//CTaskExitVehicle = 2,
	//CTaskCombatRoll = 3,
	//CTaskAimGunOnFoot = 4,
	//CTaskMovePlayer = 5,
	//CTaskPlayerOnFoot = 6,
	//CTaskWeapon = 8,
	//CTaskPlayerWeapon = 9,
	//CTaskPlayerIdles = 10,
	//CTaskAimGun = 12,
	//CTaskComplex = 12,
	//CTaskFSMClone = 12,
	//CTaskMotionBase = 12,
	//CTaskMove = 12,
	//CTaskMoveBase = 12,
	//CTaskNMBehaviour = 12,
	//CTaskNavBase = 12,
	//CTaskScenario = 12,
	//CTaskSearchBase = 12,
	//CTaskSearchInVehicleBase = 12,
	//CTaskShockingEvent = 12,
	//CTaskTrainBase = 12,
	//CTaskVehicleFSM = 12,
	//CTaskVehicleGoTo = 12,
	//CTaskVehicleMissionBase = 12,
	//CTaskVehicleTempAction = 12,
	//CTaskPause = 14,
	//CTaskDoNothing = 15,
	//CTaskGetUp = 16,
	//CTaskGetUpAndStandStill = 17,
	//CTaskFallOver = 18,
	//CTaskFallAndGetUp = 19,
	//CTaskCrawl = 20,
	//CTaskComplexOnFire = 25,
	//CTaskDamageElectric = 26,
	//CTaskTriggerLookAt = 28,
	//CTaskClearLookAt = 29,
	//CTaskSetCharDecisionMaker = 30,
	//CTaskSetPedDefensiveArea = 31,
	//CTaskUseSequence = 32,
	//CTaskMoveStandStill = 34,
	//CTaskComplexControlMovement = 35,
	//CTaskMoveSequence = 36,
	//CTaskAmbientClips = 38,
	//CTaskMoveInAir = 39,
	//CTaskNetworkClone = 40,
	//CTaskUseClimbOnRoute = 41,
	//CTaskUseDropDownOnRoute = 42,
	//CTaskUseLadderOnRoute = 43,
	//CTaskSetBlockingOfNonTemporaryEvents = 44,
	//CTaskForceMotionState = 45,
	//CTaskSlopeScramble = 46,
	//CTaskGoToAndClimbLadder = 47,
	//CTaskClimbLadderFully = 48,
	//CTaskRappel = 49,
	//CTaskVault = 50,
	//CTaskDropDown = 51,
	//CTaskAffectSecondaryBehaviour = 52,
	//CTaskAmbientLookAtEvent = 53,
	//CTaskOpenDoor = 54,
	//CTaskShovePed = 55,
	//CTaskSwapWeapon = 56,
	//CTaskGeneralSweep = 57,
	CTaskPolice = 58,
	CTaskPoliceOrderResponse = 59,
	CTaskPursueCriminal = 60,
	CTaskArrestPed = 62,
	CTaskArrestPed2 = 63,
	CTaskBusted = 64,


	//CTaskFirePatrol = 65,
	//CTaskHeliOrderResponse = 66,
	//CTaskHeliPassengerRappel = 67,
	//CTaskAmbulancePatrol = 68,
	//CTaskPoliceWantedResponse = 69,
	//CTaskSwat = 70,
	//CTaskSwatWantedResponse = 72,
	//CTaskSwatOrderResponse = 73,
	//CTaskSwatGoToStagingArea = 74,
	//CTaskSwatFollowInLine = 75,
	//CTaskWitness = 76,
	//CTaskGangPatrol = 77,
	//CTaskArmy = 78,
	//CTaskShockingEventWatch = 80,
	//CTaskShockingEventGoto = 82,
	//CTaskShockingEventHurryAway = 83,
	//CTaskShockingEventReactToAircraft = 84,
	//CTaskShockingEventReact = 85,
	//CTaskShockingEventBackAway = 86,
	//CTaskShockingPoliceInvestigate = 87,
	//CTaskShockingEventStopAndStare = 88,
	//CTaskShockingNiceCarPicture = 89,
	//CTaskShockingEventThreatResponse = 90,
	//CTaskTakeOffHelmet = 92,
	//CTaskCarReactToVehicleCollision = 93,
	//CTaskCarReactToVehicleCollisionGetOut = 95,
	//CTaskDyingDead = 97,
	//CTaskWanderingScenario = 100,
	//CTaskWanderingInRadiusScenario = 101,
	//CTaskMoveBetweenPointsScenario = 103,
	//CTaskChatScenario = 104,
	//CTaskCowerScenario = 106,
	//CTaskDeadBodyScenario = 107,
	//CTaskSayAudio = 114,
	//CTaskWaitForSteppingOut = 116,
	//CTaskCoupleScenario = 117,
	//CTaskUseScenario = 118,
	//CTaskUseVehicleScenario = 119,
	//CTaskUnalerted = 120,
	//CTaskStealVehicle = 121,
	//CTaskReactToPursuit = 122,
	//CTaskHitWall = 125,
	//CTaskCower = 126,
	//CTaskCrouch = 127,
	//CTaskMelee = 128,
	//CTaskMoveMeleeMovement = 129,
	//CTaskMeleeActionResult = 130,
	//CTaskMeleeUpperbodyAnims = 131,
	//CTaskMoVEScripted = 133,
	//CTaskScriptedAnimation = 134,
	//CTaskSynchronizedScene = 135,
	//CTaskComplexEvasiveStep = 137,
	//CTaskWalkRoundCarWhileWandering = 138,
	//CTaskComplexStuckInAir = 140,
	//CTaskWalkRoundEntity = 141,
	//CTaskMoveWalkRoundVehicle = 142,
	//CTaskReactToGunAimedAt = 144,
	//CTaskDuckAndCover = 146,
	//CTaskAggressiveRubberneck = 147,
	//CTaskInVehicleBasic = 150,
	//CTaskCarDriveWander = 151,
	//CTaskLeaveAnyCar = 152,
	//CTaskComplexGetOffBoat = 153,
	//CTaskCarSetTempAction = 155,
	//CTaskBringVehicleToHalt = 156,
	//CTaskCarDrive = 157,
	//CTaskPlayerDrive = 159,
	//CTaskEnterVehicle = 160,
	//CTaskEnterVehicleAlign = 161,
	//CTaskOpenVehicleDoorFromOutside = 162,
	//CTaskEnterVehicleSeat = 163,
	//CTaskCloseVehicleDoorFromInside = 164,
	//CTaskInVehicleSeatShuffle = 165,
	//CTaskExitVehicleSeat = 167,
	//CTaskCloseVehicleDoorFromOutside = 168,
	//CTaskControlVehicle = 169,
	//CTaskMotionInAutomobile = 170,
	//CTaskMotionOnBicycle = 171,
	//CTaskMotionOnBicycleController = 172,
	//CTaskMotionInVehicle = 173,
	//CTaskMotionInTurret = 174,
	//CTaskReactToBeingJacked = 175,
	//CTaskReactToBeingAskedToLeaveVehicle = 176,
	//CTaskTryToGrabVehicleDoor = 177,
	//CTaskGetOnTrain = 178,
	//CTaskGetOffTrain = 179,
	//CTaskRideTrain = 180,
	//CTaskMountThrowProjectile = 190,
	//CTaskGoToCarDoorAndStandStill = 195,
	//CTaskMoveGoToVehicleDoor = 196,
	//CTaskSetPedInVehicle = 197,
	//CTaskSetPedOutOfVehicle = 198,
	//CTaskVehicleMountedWeapon = 199,
	//CTaskVehicleGun = 200,
	//CTaskVehicleProjectile = 201,
	//CTaskSmashCarWindow = 204,
	//CTaskMoveGoToPoint = 205,
	//CTaskMoveAchieveHeading = 206,
	//CTaskMoveFaceTarget = 207,
	//CTaskComplexGoToPointAndStandStillTimed = 208,
	//CTaskMoveGoToPointAndStandStill = 208,
	//CTaskMoveFollowPointRoute = 209,
	//CTaskMoveSeekEntity_CEntitySeekPosCalculatorStandard = 210,
	//CTaskMoveSeekEntity_CEntitySeekPosCalculatorLastNavMeshIntersection = 211,
	//CTaskMoveSeekEntity_CEntitySeekPosCalculatorLastNavMeshIntersection2 = 212,
	//CTaskMoveSeekEntity_CEntitySeekPosCalculatorXYOffsetFixed = 213,
	//CTaskMoveSeekEntity_CEntitySeekPosCalculatorXYOffsetFixed2 = 214,
	//CTaskExhaustedFlee = 215,
	//CTaskGrowlAndFlee = 216,
	//CTaskScenarioFlee = 217,
	//CTaskSmartFlee = 218,
	//CTaskFlyAway = 219,
	//CTaskWalkAway = 220,
	//CTaskWander = 221,
	//CTaskWanderInArea = 222,
	//CTaskFollowLeaderInFormation = 223,
	//CTaskGoToPointAnyMeans = 224,
	//CTaskTurnToFaceEntityOrCoord = 225,
	//CTaskFollowLeaderAnyMeans = 226,
	//CTaskFlyToPoint = 228,
	//CTaskFlyingWander = 229,
	//CTaskGoToPointAiming = 230,
	//CTaskGoToScenario = 231,
	//CTaskSeekEntityAiming = 233,
	//CTaskSlideToCoord = 234,
	//CTaskSwimmingWander = 235,
	//CTaskMoveTrackingEntity = 237,
	//CTaskMoveFollowNavMesh = 238,
	//CTaskMoveGoToPointOnRoute = 239,
	//CTaskEscapeBlast = 240,
	//CTaskMoveWander = 241,
	//CTaskMoveBeInFormation = 242,
	//CTaskMoveCrowdAroundLocation = 243,
	//CTaskMoveCrossRoadAtTrafficLights = 244,
	//CTaskMoveWaitForTraffic = 245,
	//CTaskMoveGoToPointStandStillAchieveHeading = 246,
	//CTaskMoveGetOntoMainNavMesh = 251,
	//CTaskMoveSlideToCoord = 252,
	//CTaskMoveGoToPointRelativeToEntityAndStandStill = 253,
	//CTaskHelicopterStrafe = 254,
	//CTaskGetOutOfWater = 256,
	//CTaskMoveFollowEntityOffset = 259,
	//CTaskFollowWaypointRecording = 261,
	//CTaskMotionPed = 264,
	//CTaskMotionPedLowLod = 265,
	//CTaskHumanLocomotion = 268,
	//CTaskMotionBasicLocomotionLowLod = 269,
	//CTaskMotionStrafing = 270,
	//CTaskMotionTennis = 271,
	//CTaskMotionAiming = 272,
	//CTaskBirdLocomotion = 273,
	//CTaskFlightlessBirdLocomotion = 274,
	//CTaskFishLocomotion = 278,
	//CTaskQuadLocomotion = 279,
	//CTaskMotionDiving = 280,
	//CTaskMotionSwimming = 281,
	//CTaskMotionParachuting = 282,
	//CTaskMotionDrunk = 283,
	//CTaskRepositionMove = 284,
	//CTaskMotionAimingTransition = 285,
	//CTaskThrowProjectile = 286,
	//CTaskCover = 287,
	//CTaskMotionInCover = 288,
	//CTaskAimAndThrowProjectile = 289,
	CTaskGun = 290,
	CTaskAimFromGround = 291,
	CTaskAimGunVehicleDriveBy = 295,
	CTaskAimGunScripted = 296,
	CTaskReloadGun = 298,
	//CTaskWeaponBlocked = 299,
	//CTaskEnterCover = 300,
	//CTaskExitCover = 301,
	//CTaskAimGunFromCoverIntro = 302,
	//CTaskAimGunFromCoverOutro = 303,
	//CTaskAimGunBlindFire = 304,
	CTaskCombatClosestTargetInArea = 307,
	CTaskCombatAdditionalTask = 308,
	CTaskInCover = 309,
	CTaskAimSweep = 313,
	//CTaskSharkCircle = 319,
	//CTaskSharkAttack = 320,
	CTaskAgitated = 321,
	CTaskAgitatedAction = 322,
	CTaskConfront = 323,
	//CTaskIntimidate = 324,
	//CTaskShove = 325,
	//CTaskShoved = 326,
	//CTaskCrouchToggle = 328,
	//CTaskRevive = 329,
	//CTaskParachute = 335,
	//CTaskParachuteObject = 336,
	//CTaskTakeOffPedVariation = 337,
	//CTaskCombatSeekCover = 340,
	//CTaskCombatFlank = 342,
	//CTaskCombat = 343,
	//CTaskCombatMounted = 344,
	//CTaskMoveCircle = 345,
	//CTaskMoveCombatMounted = 346,
	//CTaskSearch = 347,
	//CTaskSearchOnFoot = 348,
	//CTaskSearchInAutomobile = 349,
	//CTaskSearchInBoat = 350,
	//CTaskSearchInHeli = 351,
	//CTaskThreatResponse = 352,
	//CTaskInvestigate = 353,
	//CTaskStandGuardFSM = 354,
	//CTaskPatrol = 355,
	CTaskShootAtTarget = 356,
	//CTaskSetAndGuardArea = 357,
	//CTaskStandGuard = 358,
	//CTaskSeparate = 359,
	//CTaskStayInCover = 360,
	//CTaskVehicleCombat = 361,
	//CTaskVehiclePersuit = 362,
	//CTaskVehicleChase = 363,
	//CTaskDraggingToSafety = 364,
	//CTaskDraggedToSafety = 365,
	//CTaskVariedAimPose = 366,
	//CTaskMoveWithinAttackWindow = 367,
	//CTaskMoveWithinDefensiveArea = 368,
	//CTaskShootOutTire = 369,
	//CTaskShellShocked = 370,
	//CTaskBoatChase = 371,
	//CTaskBoatCombat = 372,
	//CTaskBoatStrafe = 373,
	//CTaskHeliChase = 374,
	//CTaskHeliCombat = 375,
	//CTaskSubmarineCombat = 376,
	//CTaskSubmarineChase = 377,
	//CTaskPlaneChase = 378,
	//CTaskTargetUnreachable = 379,
	//CTaskTargetUnreachableInInterior = 380,
	//CTaskTargetUnreachableInExterior = 381,
	//CTaskStealthKill = 382,
	//CTaskWrithe = 383,
	//CTaskAdvance = 384,
	//CTaskCharge = 385,
	//CTaskMoveToTacticalPoint = 386,
	//CTaskToHurtTransit = 387,
	//CTaskAnimatedHitByExplosion = 388,
	//CTaskNMRelax = 389,
	//CTaskNMPose = 391,
	//CTaskNMBrace = 392,
	//CTaskNMBuoyancy = 393,
	//CTaskNMInjuredOnGround = 394,
	//CTaskNMShot = 395,
	//CTaskNMHighFall = 396,
	//CTaskNMBalance = 397,
	//CTaskNMElectrocute = 398,
	//CTaskNMPrototype = 399,
	//CTaskNMExplosion = 400,
	//CTaskNMOnFire = 401,
	//CTaskNMScriptControl = 402,
	//CTaskNMJumpRollFromRoadVehicle = 403,
	//CTaskNMFlinch = 404,
	//CTaskNMSit = 405,
	//CTaskNMFallDown = 406,
	//CTaskBlendFromNM = 407,
	//CTaskNMControl = 408,
	//CTaskNMDangle = 409,
	//CTaskNMGenericAttach = 412,
	//CTaskNMDraggingToSafety = 414,
	//CTaskNMThroughWindscreen = 415,
	//CTaskNMRiverRapids = 416,
	//CTaskNMSimple = 417,
	//CTaskRageRagdoll = 418,
	//CTaskJumpVault = 421,
	//CTaskJump = 422,
	//CTaskFall = 423,
	//CTaskReactAimWeapon = 425,
	//CTaskChat = 426,
	//CTaskMobilePhone = 427,
	//CTaskReactToDeadPed = 428,
	//CTaskSearchForUnknownThreat = 430,
	//CTaskBomb = 432,
	//CTaskDetonator = 433,
	//CTaskAnimatedAttach = 435,
	//CTaskCutScene = 441,
	//CTaskReactToExplosion = 442,
	//CTaskReactToImminentExplosion = 443,
	//CTaskDiveToGround = 444,
	//CTaskReactAndFlee = 445,
	//CTaskSidestep = 446,
	//CTaskCallPolice = 447,
	//CTaskReactInDirection = 448,
	//CTaskReactToBuddyShot = 449,
	//CTaskVehicleGoToAutomobileNew = 454,
	//CTaskVehicleGoToPlane = 455,
	//CTaskVehicleGoToHelicopter = 456,
	//CTaskVehicleGoToSubmarine = 457,
	//CTaskVehicleGoToBoat = 458,
	//CTaskVehicleGoToPointAutomobile = 459,
	//CTaskVehicleGoToPointWithAvoidanceAutomobile = 460,
	//CTaskVehiclePursue = 461,
	//CTaskVehicleRam = 462,
	//CTaskVehicleSpinOut = 463,
	//CTaskVehicleApproach = 464,
	//CTaskVehicleThreePointTurn = 465,
	//CTaskVehicleDeadDriver = 466,
	//CTaskVehicleCruiseNew = 467,
	//CTaskVehicleCruiseBoat = 468,
	//CTaskVehicleStop = 469,
	//CTaskVehiclePullOver = 470,
	//CTaskVehiclePassengerExit = 471,
	//CTaskVehicleFlee = 472,
	//CTaskVehicleFleeAirborne = 473,
	//CTaskVehicleFleeBoat = 474,
	//CTaskVehicleFollowRecording = 475,
	//CTaskVehicleFollow = 476,
	//CTaskVehicleBlock = 477,
	//CTaskVehicleBlockCruiseInFront = 478,
	//CTaskVehicleBlockBrakeInFront = 479,
	//CTaskVehicleBlockBackAndForth = 478,
	//CTaskVehicleCrash = 481,
	//CTaskVehicleLand = 482,
	//CTaskVehicleLandPlane = 483,
	//CTaskVehicleHover = 484,
	//CTaskVehicleAttack = 485,
	//CTaskVehicleAttackTank = 486,
	//CTaskVehicleCircle = 487,
	//CTaskVehiclePoliceBehaviour = 488,
	//CTaskVehiclePoliceBehaviourHelicopter = 489,
	//CTaskVehiclePoliceBehaviourBoat = 490,
	//CTaskVehicleEscort = 491,
	//CTaskVehicleHeliProtect = 492,
	//CTaskVehiclePlayerDriveAutomobile = 494,
	//CTaskVehiclePlayerDriveBike = 495,
	//CTaskVehiclePlayerDriveBoat = 496,
	//CTaskVehiclePlayerDriveSubmarine = 497,
	//CTaskVehiclePlayerDriveSubmarineCar = 498,
	//CTaskVehiclePlayerDriveAmphibiousAutomobile = 499,
	//CTaskVehiclePlayerDrivePlane = 500,
	//CTaskVehiclePlayerDriveHeli = 501,
	//CTaskVehiclePlayerDriveAutogyro = 502,
	//CTaskVehiclePlayerDriveDiggerArm = 503,
	//CTaskVehiclePlayerDriveTrain = 504,
	//CTaskVehiclePlaneChase = 505,
	//CTaskVehicleNoDriver = 506,
	//CTaskVehicleAnimation = 507,
	//CTaskVehicleConvertibleRoof = 508,
	//CTaskVehicleParkNew = 509,
	//CTaskVehicleFollowWaypointRecording = 510,
	//CTaskVehicleGoToNavmesh = 511,
	//CTaskVehicleReactToCopSiren = 512,
	//CTaskVehicleGotoLongRange = 513,
	//CTaskVehicleWait = 514,
	//CTaskVehicleReverse = 515,
	//CTaskVehicleBrake = 516,
	//CTaskVehicleHandBrake = 517,
	//CTaskVehicleTurn = 518,
	//CTaskVehicleGoForward = 519,
	//CTaskVehicleSwerve = 520,
	//CTaskVehicleFlyDirection = 521,
	//CTaskVehicleHeadonCollision = 522,
	//CTaskVehicleBoostUseSteeringAngle = 523,
	//CTaskVehicleShotTire = 524,
	//CTaskVehicleBurnout = 525,
	//CTaskVehicleRevEngine = 526,
	//CTaskVehicleSurfaceInSubmarine = 527,
	//CTaskVehiclePullAlongside = 528,
	//CTaskVehicleTransformToSubmarine = 529,
	//CTaskAnimatedFallback = 530
};
}

