using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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
    private Tasker Tasker;
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

    public Debug(PlateTypes plateTypes, Mod.World world, Mod.Player targetable, IStreets streets, Dispatcher dispatcher, Zones zones, Crimes crimes, ModController modController, Settings settings, Tasker tasker, Mod.Time time,Agencies agencies, Weapons weapons, ModItems modItems)
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
            foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
            {
                DrawColoredArrowTaskStatus(cop);
                DrawColoredArrowAlertness(cop);
            }
        }
        if (Settings.SettingsManager.DebugSettings.ShowCivilianTaskArrows)
        {
            foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 45f))
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
            foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists()))/// && x.DistanceToPlayer <= 250f))// && NativeHelper.IsNearby(EntryPoint.FocusCellX,EntryPoint.FocusCellY,x.CellX,x.CellY,4)))// x.DistanceToPlayer <= 150f))
            {
                Color Color3 = Color.Yellow;
                if (ped.HasSeenPlayerCommitCrime)
                {
                    Color3 = Color.Red;
                }
                else if (ped.CanRecognizePlayer)
                {
                    Color3 = Color.Orange;
                }
                else if (ped.CanSeePlayer)
                {
                    Color3 = Color.Green;
                }
                Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color3);
            }

            foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
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

    }
    public void Setup()
    {
        InteriorPositions = new List<InteriorPosition>()
        {
            new InteriorPosition("10 Car",new Vector3(229.9559f, -981.7928f, -99.66071f))//works
            ,new InteriorPosition("Low End Apartment",new Vector3(261.4586f, -998.8196f, -99.00863f))//works
            ,new InteriorPosition("4 Integrity Way, Apt 30",new Vector3(-35.31277f, -580.4199f, 88.71221f))//works
            ,new InteriorPosition("Dell Perro Heights, Apt 4",new Vector3(-1468.14f, -541.815f, 73.4442f))//works
            ,new InteriorPosition("Dell Perro Heights, Apt 7",new Vector3(-1477.14f, -538.7499f, 55.5264f))//works
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
    private void DebugNumpad0()
    {
        Game.LocalPlayer.IsInvincible = true;
        Game.DisplayNotification("IsInvincible = True");
    }
    private void DebugNumpad1()
    {
        ModController.DebugCoreRunning = !ModController.DebugCoreRunning;
        Game.DisplayNotification($"ModController.DebugCoreRunning {ModController.DebugCoreRunning}");
        GameFiber.Sleep(500);


        //Game.LocalPlayer.IsInvincible = false;
        //Game.DisplayNotification("IsInvincible = False");
    }
    private void DebugNumpad2()
    {
        ModController.DebugSecondaryRunning = !ModController.DebugSecondaryRunning;
        Game.DisplayNotification($"ModController.DebugSecondaryRunning {ModController.DebugSecondaryRunning}");
        GameFiber.Sleep(500);
        //Dispatcher.DebugSpawnCop();
    }
    private void DebugNumpad3()
    {
        ModController.DebugTertiaryRunning = !ModController.DebugTertiaryRunning;
        Game.DisplayNotification($"ModController.DebugTertiaryRunning {ModController.DebugTertiaryRunning}");
        GameFiber.Sleep(500);

        WriteCivilianAndCopState();
    }
    private void DebugNumpad4()
    {
        ModController.DebugQuaternaryRunning = !ModController.DebugQuaternaryRunning;
        Game.DisplayNotification($"ModController.DebugQuaternaryRunning {ModController.DebugQuaternaryRunning}");
        GameFiber.Sleep(500);



        //mp_doorbell
        //open_door
        //AnimationDictionary.RequestAnimationDictionay("switch@michael@biking_with_jimmy");
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "switch@michael@biking_with_jimmy", "exit_right_door", 8.0f, -8.0f, -1, 50, 0, false, false, false);//-1
        Player.AddToInventory(ModItems.Get("Hot Dog"), 4);
        Player.AddToInventory(ModItems.Get("Can of eCola"), 4);
        Player.AddToInventory(ModItems.Get("Redwood Regular"), 4);
        Player.AddToInventory(ModItems.Get("Alco Patch"), 4);

        Player.AddToInventory(ModItems.Get("Equanox"), 4);



        //Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.Position, 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
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

        //}
        // Player.ScannerPlayDebug();

        //SetInRandomInterior();
        //BrowseTimecycles();


        // Dispatcher.RemoveRoadblock();
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
        ModController.DebugQuinaryRunning = !ModController.DebugQuinaryRunning;
        Game.DisplayNotification($"ModController.DebugQuinaryRunning {ModController.DebugQuinaryRunning}");
        GameFiber.Sleep(500);


        Player.AddToInventory(ModItems.Get("Hot Dog"), 4);
        Player.AddToInventory(ModItems.Get("Can of eCola"), 4);
        Player.AddToInventory(ModItems.Get("Redwood Regular"), 4);
        Player.AddToInventory(ModItems.Get("Alco Patch"), 4);

        Player.AddToInventory(ModItems.Get("Equanox"), 4);

        //World.DebugPlayWeather();


        // Dispatcher.SpawnRoadblock();


        //EntryPoint.WriteToConsole("Zone STRING : " + GetInternalZoneString(Game.LocalPlayer.Character.Position),5);
        //  Player.ResetScannerDebug();

        //Crime toPlay = Crimes.CrimeList.Where(x => x.CanBeReportedByCivilians).PickRandom();
        //CrimeSceneDescription toAnnounce = new CrimeSceneDescription(false,false,Game.LocalPlayer.Character.Position);
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
    private void DebugNumpad6()
    {

        ModController.DebugNonPriorityRunning = !ModController.DebugNonPriorityRunning;
        Game.DisplayNotification($"ModController.DebugNonPriorityRunning {ModController.DebugNonPriorityRunning}");
        GameFiber.Sleep(500);



        //int TotalEntities = 0;
        //EntryPoint.WriteToConsole($"SPAWNED ENTITIES ===============================", 2);
        //foreach (Entity ent in EntryPoint.SpawnedEntities)
        //{
        //    if(ent.Exists())
        //    {
        //        TotalEntities++;
        //        EntryPoint.WriteToConsole($"SPAWNED ENTITY STILL EXISTS {ent.Handle} {ent.GetType()} {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
        //    }
        //}
        //EntryPoint.WriteToConsole($"SPAWNED ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        //TotalEntities = 0;

        //List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        //EntryPoint.WriteToConsole($"PERSISTENT ENTITIES ===============================", 2);
        //foreach (Entity ent in AllEntities)
        //{
        //    if (ent.Exists() && ent.IsPersistent)
        //    {
        //        TotalEntities++;
        //        EntryPoint.WriteToConsole($"PERSISTENT ENTITY STILL EXISTS {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
        //    }
        //}
        //EntryPoint.WriteToConsole($"PERSISTENT ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        //WriteCopState();

        //SpawnModelChecker();
        //Vector3 pos = Game.LocalPlayer.Character.Position;
        //float Heading = Game.LocalPlayer.Character.Heading;
        //string text1 = NativeHelper.GetKeyboardInput("");
        //string text2 = NativeHelper.GetKeyboardInput("");
        //WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void DebugNumpad7()
    {
        ModController.DebugUIRunning = !ModController.DebugUIRunning;
        Game.DisplayNotification($"ModController.DebugUIRunning {ModController.DebugUIRunning}");
        GameFiber.Sleep(500);
        //NativeFunction.Natives.PLAY_POLICE_REPORT("LAMAR_1_POLICE_LOST", 0.0f);
        //EntryPoint.WriteToConsole($"PLAY_POLICE_REPORT(LAMAR_1_POLICE_LOST", 5);
        //Dispatcher.RemoveRoadblock();
        //CharCam = new Camera(true);
        //CharCam.Active = false;
        //Game.LocalPlayer.Character.Position = new Vector3(815.8774f, -1290.531f, 26.28391f);
        ////PedSettingStuff();
        //PedCameraStuff();
    }
    public void DebugNumpad8()
    {
        ModController.DebugInputRunning = !ModController.DebugInputRunning;
        Game.DisplayNotification($"ModController.DebugInputRunning {ModController.DebugInputRunning}");
        GameFiber.Sleep(500);


        //if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        //    {
        //    boolFlipper = !boolFlipper;
        //    NativeFunction.Natives.xF3365489E0DD50F9(Player.CurrentVehicle.Vehicle, boolFlipper);
        //    EntryPoint.WriteToConsole($"xF3365489E0DD50F9 {boolFlipper}", 5);
        //}

        //EntryPoint.WriteToConsole($"FASTFORWARD TO 11 AM TOMORROW {Time.CurrentTime}", 5);

        //Time.FastForward(new DateTime(Time.CurrentYear,Time.CurrentMonth,Time.CurrentDay + 1,11,0,0));

        //List<MenuItem> WeedDealerMenu = new List<MenuItem>() {
        //    new MenuItem("Gram of Schwag",6, 1),
        //    new MenuItem("Gram of Mids",9, 3),
        //    new MenuItem("Gram of Dank",12, 4),
        //    new MenuItem("Joint",3, 1)};

        //if (Player.CurrentLookedAtPed != null)
        //{
        //    Player.CurrentLookedAtPed.TransactionMenu = WeedDealerMenu;
        //}

        // SetRadarZoomeFor20Seconds(1000f);
        //Player.OnSuspectEluded();
        //Player.SetWantedLevel(0, "Clear Wanted Debug", true);
    }
    private void DebugNumpad9()
    {
        ModController.DebugNonPriorityRunning = !ModController.DebugNonPriorityRunning;
        Game.DisplayNotification($"ModController.DebugNonPriorityRunning {ModController.DebugNonPriorityRunning}");
        GameFiber.Sleep(500);
        //int CurrentWanted = Player.WantedLevel;
        //if(CurrentWanted <= 5)
        //{
        //    CurrentWanted++;
        //    Player.SetWantedLevel(CurrentWanted, "Increase Wanted", true);
        //}
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
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(-20f).Around2D(10f));
            GameFiber.Yield();
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;

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
        //EntryPoint.WriteToConsole($"============================================ TRACKED VEHICLES START", 5);
        //foreach (VehicleExt veh in World.CivilianVehicleList.Where(x => x.Vehicle.Exists()).OrderBy(x=> x.Vehicle.DistanceTo2D(Game.LocalPlayer.Character)))
        //{
        //    EntryPoint.WriteToConsole($"veh {veh.Vehicle.Handle} {veh.Vehicle.Model.Name} IsCar {veh.Vehicle.IsCar} Engine.IsRunning {veh.Engine.IsRunning} IsDriveable {veh.Vehicle.IsDriveable} IsLockedForPlayer {veh.Vehicle.IsLockedForPlayer(Game.LocalPlayer)}", 5);
        //}
        //EntryPoint.WriteToConsole($"============================================ TRACKED VEHICLES END", 5);
        EntryPoint.WriteToConsole($"============================================ CIVIES START", 5);
        foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 200f).OrderBy(x => x.DistanceToPlayer))
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(ped.Pedestrian, out currentWeapon, true);
            uint RG = NativeFunction.Natives.GET_PED_RELATIONSHIP_GROUP_HASH<uint>(ped.Pedestrian);
            EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer}-Cells:{NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ped.CellX, ped.CellY)} {ped.Pedestrian.Model.Name} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.ViolationWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} GroupName {ped.PedGroup?.InternalName} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ CIVIES END", 5);
        EntryPoint.WriteToConsole($"============================================ COPS START", 5);
        foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
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
            EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} TaskStatus:{cop.Pedestrian.Tasks.CurrentTaskStatus} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{cop.CurrentTask?.OtherTarget?.Handle} Vehicle {VehString} {combat} {Weapon} {VehicleWeapon} HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned}", 5);
        }
        EntryPoint.WriteToConsole($"============================================ COPS END", 5);
    }
    private void WriteCopState()
    {
        EntryPoint.WriteToConsole($"============================================ POLICE VEHICLES START", 2);
        foreach (VehicleExt veh in World.PoliceVehicleList.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(Game.LocalPlayer.Character)))
        {
            EntryPoint.WriteToConsole($"veh {veh.Vehicle.Handle} {veh.Vehicle.Model.Name} IsPersistent {veh.Vehicle.IsPersistent} Position: {veh.Vehicle.Position}", 2);
        }
        EntryPoint.WriteToConsole($"============================================ POLICE VEHICLES END", 2);
        EntryPoint.WriteToConsole($"============================================ COPS START", 2);
        foreach (Cop cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
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
            EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{cop.CurrentTask?.OtherTarget?.Handle} Vehicle {VehString} {combat} {Weapon} {VehicleWeapon} HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned} Position: {cop.Pedestrian.Position}", 2);
        }
        EntryPoint.WriteToConsole($"============================================ COPS END", 2);
    }
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
                Game.DisplaySubtitle($" PlateIndex: {PlateIndex}, Index: {NewType.Index}, State: {NewType.State}, Description: {NewType.Description}");
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
    public void IssueWeapons(IWeapons weapons)
    {
        IssuableWeapon Sidearm = Agencies.GetAgency("NOOSE").GetRandomWeapon(true, weapons);
        IssuableWeapon LongGun = Agencies.GetAgency("NOOSE").GetRandomWeapon(false, weapons);
        if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)WeaponHash.StunGun, false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)WeaponHash.StunGun, 100, false, false);
        }
        if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)Sidearm.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)Sidearm.GetHash(), 200, false, false);
            Sidearm.ApplyVariation(Game.LocalPlayer.Character);
        }
        if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Game.LocalPlayer.Character, (uint)LongGun.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, (uint)LongGun.GetHash(), 200, false, false);
            LongGun.ApplyVariation(Game.LocalPlayer.Character);
        }
       // NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Game.LocalPlayer.Character, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
      //  NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Game.LocalPlayer.Character, 2, true);//can do drivebys    
    }
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

