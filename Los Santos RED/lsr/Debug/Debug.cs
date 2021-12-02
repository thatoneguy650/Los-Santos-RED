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
    private Camera StoreCam;
    private Camera InterpolationCamera;
    private Agencies Agencies;
    private Weapons Weapons;

    public Debug(PlateTypes plateTypes, Mod.World world, Mod.Player targetable, IStreets streets, Dispatcher dispatcher, Zones zones, Crimes crimes, ModController modController, Settings settings, Tasker tasker, Mod.Time time,Agencies agencies, Weapons weapons)
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
        //foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 90f))
        //{
        //    Color Color = Color.Yellow;
        //    if (ped.CurrentlyViolatingWantedLevel == 0)
        //    {
        //        Color = Color.Green;
        //    }
        //    else if (ped.CurrentlyViolatingWantedLevel == 1)
        //    {
        //        Color = Color.Yellow;
        //    }
        //    else if (ped.CurrentlyViolatingWantedLevel == 2)
        //    {
        //        Color = Color.Orange;
        //    }
        //    else if (ped.CurrentlyViolatingWantedLevel > 2)
        //    {
        //        Color = Color.Red;
        //    }
        //    Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, Color);
        //    Color Color2 = Color.Yellow;
        //    if (ped.WantedLevel == 0)
        //    {
        //        Color2 = Color.Green;
        //    }
        //    else if (ped.WantedLevel == 1)
        //    {
        //        Color2 = Color.Yellow;
        //    }
        //    else if (ped.WantedLevel == 2)
        //    {
        //        Color2 = Color.Orange;
        //    }
        //    else if (ped.WantedLevel > 2)
        //    {
        //        Color2 = Color.Red;
        //    }
        //    Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 2.5f), Vector3.Zero, Rotator.Zero, 1f, Color2);
        //    Color Color3 = Color.Yellow;
        //    if (ped.HasSeenPlayerCommitCrime)
        //    {
        //        Color3 = Color.Red;
        //    }
        //    else if (ped.CanRecognizePlayer)
        //    {
        //        Color3 = Color.Orange;
        //    }
        //    else if (ped.CanSeePlayer)
        //    {
        //        Color3 = Color.Green;
        //    }
        //    Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Rotator.Zero, 1f, Color3);
        //}
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
    }
    private void DebugNumpad0()
    {
        Game.LocalPlayer.IsInvincible = true;
        Game.DisplayNotification("IsInvincible = True");
    }
    private void DebugNumpad1()
    {
        Game.LocalPlayer.IsInvincible = false;
        Game.DisplayNotification("IsInvincible = False");
    }
    private void DebugNumpad2()
    {
        Dispatcher.SpawnCop(Game.LocalPlayer.Character.GetOffsetPositionFront(10f));
    }
    private void DebugNumpad3()
    {
        WriteCivilianAndCopState();
    }
    private void DebugNumpad4()
    {
        IssueWeapons(Weapons);
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
    }
    private void DebugNumpad5()
    {
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

        SpawnGunAttackers();
    }
    private void DebugNumpad6()
    {
        int TotalEntities = 0;
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES ===============================", 2);
        foreach (Entity ent in EntryPoint.SpawnedEntities)
        {
            if(ent.Exists())
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"SPAWNED ENTITY STILL EXISTS {ent.Handle} {ent.GetType()} {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
            }
        }
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        TotalEntities = 0;

        List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES ===============================", 2);
        foreach (Entity ent in AllEntities)
        {
            if (ent.Exists() && ent.IsPersistent)
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"PERSISTENT ENTITY STILL EXISTS {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 2);
            }
        }
        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES =============================== TOTAL: {TotalEntities}", 2);

        WriteCopState();

        //SpawnModelChecker();
        //Vector3 pos = Game.LocalPlayer.Character.Position;
        //float Heading = Game.LocalPlayer.Character.Heading;
        //string text1 = NativeHelper.GetKeyboardInput("");
        //string text2 = NativeHelper.GetKeyboardInput("");
        //WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void DebugNumpad7()
    {
        EntryPoint.WriteToConsole($"FASTFORWARD 8 HOURS {Time.CurrentTime}", 5);
        Time.FastForward(8);
      //  HighlightStoreWithCamera();



      //GameFiber.Sleep(5000);
      //  ReturnToGameplay();
        //EntryPoint.WriteToConsole($"CURRENT TIME (PRE) {Time.CurrentTime}", 5);     
        //Time.FastForward(8);
        //GameFiber.Sleep(5000);
        //EntryPoint.WriteToConsole($"CURRENT TIME (POST) {Time.CurrentTime}", 5);

        //ConsumableSubstance toadd = ConsumableSubstances.Consumables.PickRandom();
        //if(toadd != null)
        //{
        //    Player.AddToInventory(toadd,1);
        //    EntryPoint.WriteToConsole($"ADDED {toadd.Name} {toadd.Type}", 5);
        //}

        //if (Player.CurrentLookedAtPed != null)
        //{
        //    Player.CurrentLookedAtPed.MerchantType = MerchantType.HotDog;
        //}
        //Dispatcher.SpawnHelicopterCop(Game.LocalPlayer.Character.GetOffsetPositionFront(10f));
    }
    public void DebugNumpad8()
    {
        EntryPoint.WriteToConsole($"FASTFORWARD TO 11 AM TOMORROW {Time.CurrentTime}", 5);

        Time.FastForward(new DateTime(Time.CurrentYear,Time.CurrentMonth,Time.CurrentDay + 1,11,0,0));

        // SetRadarZoomeFor20Seconds(1000f);
        //Player.OnSuspectEluded();
        //Player.SetWantedLevel(0, "Clear Wanted Debug", true);
    }
    private void DebugNumpad9()
    {
        int CurrentWanted = Player.WantedLevel;
        if(CurrentWanted <= 5)
        {
            CurrentWanted++;
            Player.SetWantedLevel(CurrentWanted, "Increase Wanted", true);
        }
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
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        StoreCam.Position = CameraPos;
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        StoreCam.Rotation = HowToRotate;
        if (!InterpolationCamera.Exists())
        {
            InterpolationCamera = new Camera(false);
        }
        InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
        InterpolationCamera.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, InterpolationCamera, 1500, true, true);

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
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(InterpolationCamera, StoreCam, 1500, true, true);
        GameFiber.Sleep(1500);
        InterpolationCamera.Active = false;
        if (StoreCam.Exists())
        {
            StoreCam.Delete();
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
            EntryPoint.WriteToConsole($"Handle {ped.Pedestrian.Handle}-{ped.DistanceToPlayer} {ped.Pedestrian.Model.Name} WantedLevel = {ped.WantedLevel} IsDeadlyChase = {ped.IsDeadlyChase} IsBusted {ped.IsBusted} IsArrested {ped.IsArrested} ViolationWantedLevel = {ped.CurrentlyViolatingWantedLevel} Weapon {currentWeapon} Reason {ped.ViolationWantedLevelReason} Stunned {ped.Pedestrian.IsStunned} GroupName {ped.PedGroup?.InternalName} Task {ped.CurrentTask?.Name}-{ped.CurrentTask?.SubTaskName} WasEverSetPersistent:{ped.WasEverSetPersistent} Call:{ped.WillCallPolice} Fight:{ped.WillFight} NewGroup:{ped.Pedestrian.RelationshipGroup.Name} NativeGroup:{RG}", 5);
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
            EntryPoint.WriteToConsole($"Num6: Cop {cop.Pedestrian.Handle}-{cop.DistanceToPlayer} {cop.Pedestrian.Model.Name} Weapons: {cop.CopDebugString} Task: {cop.CurrentTask?.Name}-{cop.CurrentTask?.SubTaskName} Target:{cop.CurrentTask?.OtherTarget?.Handle} Vehicle {VehString} {combat} {Weapon} {VehicleWeapon} HasLoggedDeath {cop.HasLoggedDeath} WasModSpawned {cop.WasModSpawned}", 5);
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

