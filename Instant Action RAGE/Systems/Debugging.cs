using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Debugging
{

    public static bool ShowCopTaskStatus = false;
    public static List<GameFiber> GameFibers = new List<GameFiber>();

    public static bool IsRunning { get; set; } = true;
    public static void Initialize()
    {
        MainLoop();
    }
    public static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();
                    DebugLoop();
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        WriteToLog("DebuggingTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void DebugLoop()
    {

        string MyGameFibers = string.Join(";", GameFibers.Where(x => x.IsAlive).GroupBy(g => g.Name).Select(group => group.Key + ":" + group.Count()));//new { GF = group.Key,Cnt = group.Count() }));//.ToArray());

       // string MyGameFibers = string.Join(";", GameFibers.Where(x => x.IsAlive).Select(x => x.Name).Distinct().ToArray());

        //GameFibers.
        string DebugString = string.Format("Fibers: {0} LastWeapon: {1}", MyGameFibers, InstantAction.LastWeapon);
        UI.Text(DebugString, 0.86f, 0.16f, 0.35f, false, Color.White, UI.eFont.FontChaletComprimeCologne);
        //if (DispatchAudioGameFibersRunning)
        //    DebugString = DebugString + " DA!";

        //if (InstantActionGameFibersRunning)
        //    DebugString = DebugString + " IA!";


        string TextToShow = "Police State: " + Police.TempCurrentPoliceTickRunning;
        if (Police.PlayerIsPersonOfInterest)
            TextToShow = TextToShow + " + POI";

        if(Police.PlayerLastSeenInVehicle)
            TextToShow = TextToShow + " + LS:Vehicle";
        else
            TextToShow = TextToShow + " + LS:Foot";

        UI.Text(TextToShow, 0.84f, 0.16f, 0.35f, false, Color.White, UI.eFont.FontChaletComprimeCologne);

        if (Smoking.CurrentIdleAnimation != null)
        {
            string Animation = string.Format("Anim: {0}, Time: {1}, NearMouth: {2}", Smoking.CurrentIdleAnimation.Animation, Smoking.CurrentPuffingAnimationTime, Smoking.CurrentPuffingAnimationNearMouth);
            UI.Text(Animation, 0.82f, 0.16f, 0.35f, false, Color.White, UI.eFont.FontChaletComprimeCologne);
        }


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
        if (ShowCopTaskStatus)
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
            {
                if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Green);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Purple);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.White);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Orange);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Red);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Black);
                else
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
            }
            //if (Game.LocalPlayer.WantedLevel > 0)
            //{
            //    Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            //    Rage.Debug.DrawArrowDebug(new Vector3(CurrentWantedLevelPosition.X, CurrentWantedLevelPosition.Y, CurrentWantedLevelPosition.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Blue);
            //}
        }

    }
    private static void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = 100;
        Debugging.WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = 100;
        Debugging.WriteToLog("KeyDown", "You are invicible");
    }
    private static void DebugCopReset()
    {
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Game.LocalPlayer.WantedLevel = 0;
        Tasking.UntaskAll(true);


        foreach (GTACop Cop in PoliceScanning.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.CurrentVehicle.Delete();
            Cop.CopPed.Delete();
        }


        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        foreach (Ped dog in closestPed)
        {
            dog.Delete();
        }

        Game.TimeScale = 1f;
        InstantAction.IsBusted = false;
        InstantAction.BeingArrested = false;
        NativeFunction.Natives.xB4EDDC19532BFB85();


        PoliceSpawning.Dispose();
    }
    private static void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private static void DebugNumpad1()
    {
        DebugInvincible();
    }
    private static void DebugNumpad2()
    {
        if (Game.LocalPlayer.WantedLevel > 0)
            Game.LocalPlayer.WantedLevel = 0;
        else
            Game.LocalPlayer.WantedLevel = 2;
    }
    private static void DebugNumpad3()
    {
        DebugCopReset();
    }
    private static void DebugNumpad4()
    {
        Settings.Logging = true;

        //TestStreetCall();


        //PoliceScanning.RemoveAllCreatedEntities();


        foreach(Location loc in Locations.GetAllLocationsOfType(Location.LocationType.Police))
        {
            WriteToLog("", loc.ToString());
        }


        //Tasking.RetaskAllRandomSpawns();
        return;



        PoliceSpawning.SpawnCop(Agencies.SAHP, Game.LocalPlayer.Character.GetOffsetPositionFront(10f));




        GTACop MyCop = PoliceScanning.CopPeds.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (MyCop != null)
        {

            MyCop.CopPed.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f);



            Tasking.AddItemToQueue(new PoliceTask(MyCop, PoliceTask.Task.RandomSpawnIdle));

            return;




            MyCop.CopPed.Tasks.StandStill(-1);

            Ped PedToMove = Game.LocalPlayer.Character;
            Vector3 PositionToMoveTo = MyCop.CopPed.GetOffsetPositionFront(1f);

            // 
            bool Continue = true;
            bool isPlayer = true;
            //Vector3 Resultant = Vector3.Subtract(PositionToMoveTo, MyCop.CopPed.Position);
            //float DesiredHeading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);





            //NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

            Game.LocalPlayer.Character.Tasks.GoToOffsetFromEntity(MyCop.CopPed, -1, 1f, 0f, 2f);


            while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.2f))
            {
                GameFiber.Yield();
                if (isPlayer && Extensions.IsMoveControlPressed())
                {
                    Continue = false;
                    break;
                }
            }
            if (!Continue)
            {
                PedToMove.Tasks.Clear();
            }



            GameFiber.Sleep(1000);

            //Respawning.BribeAnimation(MyCop.CopPed, Game.LocalPlayer.Character);
            //MyCop.CopPed.Tasks.LeaveVehicle(MyCop.CopPed.CurrentVehicle, LeaveVehicleFlags.None);
            //GameFiber.Sleep(4000);
            //PoliceScanning.Untask(MyCop);

            GameFiber.Sleep(15000);

            //PoliceScanning.RandomSpawnIdle(MyCop);

            if (MyCop.CopPed.CurrentVehicle.Exists())
                MyCop.CopPed.CurrentVehicle.Delete();
            if (MyCop.CopPed.LastVehicle.Exists())
                MyCop.CopPed.LastVehicle.Delete();
            MyCop.CopPed.Delete();

        }






        //GameFiber.StartNew(delegate
        //{
        //    VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass != VehicleLookup.VehicleClass.Utility).PickRandom();
        //    Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //    Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //    PoliceScanning.CreatedEntities.Add(MyCar);
        //    PoliceScanning.CreatedEntities.Add(Driver);
        //    Driver.WarpIntoVehicle(MyCar, -1);
        //    uint GameTimeStarted = Game.GameTime;
        //    //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
        //    //  GameFiber.Yield();

        //    Debugging.WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
        //    Debugging.WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

        //    // CarJackPedWithWeapon(MyCar, Driver, -1);
        //    while(Game.GameTime - GameTimeStarted <= 20000)
        //    {
        //        //Text(myLookup.VehicleClass.ToString(), 0.5f, 0.5f, 0.75f, true, Color.Black);
        //        GameFiber.Yield();
        //    }

        //    if (Driver.Exists())
        //        Driver.Delete();

        //    if (MyCar.Exists())
        //        MyCar.Delete();

        //});

        //GameFiber.StartNew(delegate
        //{
        //    VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass == VehicleLookup.VehicleClass.Coupe || x.VehicleClass == VehicleLookup.VehicleClass.Sedan || x.VehicleClass == VehicleLookup.VehicleClass.Sports || x.VehicleClass == VehicleLookup.VehicleClass.SUV || x.VehicleClass == VehicleLookup.VehicleClass.Compact).PickRandom();
        //    Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //    Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //    PoliceScanning.CreatedEntities.Add(MyCar);
        //    PoliceScanning.CreatedEntities.Add(Driver);
        //    Driver.WarpIntoVehicle(MyCar, -1);
        //    uint GameTimeStarted = Game.GameTime;
        //    //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
        //    //  GameFiber.Yield();

        //    Debugging.WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
        //    Debugging.WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

        //    // CarJackPedWithWeapon(MyCar, Driver, -1);
        //    GameFiber.Sleep(20000);
        //    if (Driver.Exists())
        //        Driver.Delete();

        //    if (MyCar.Exists())
        //        MyCar.Delete();

        //});






        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;
        //}






        //DispatchAudioSystem.AbortAllAudio();







        //Vehicle MyCar = new Vehicle("gauntlet", Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //Ped Driver = new Ped("u_m_y_hippie_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //Driver.BlockPermanentEvents = true;

        //Driver.WarpIntoVehicle(MyCar, -1);

        ////uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}
        //GameFiber.Sleep(3000);

        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 11816);
        //Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);



        //uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}



        //Driver.Position = DriverSeatCoordinates;

        //GameFiber.Sleep(3000);

        //Driver.WarpIntoVehicle(MyCar, -1);

        //GameFiber.Sleep(3000);

        ////GameFiber.Sleep(3000);

        //if (MyCar.Exists())
        //    MyCar.Delete();

        //if (Driver.Exists())
        //    Driver.Delete();



        //foreach (DroppedWeapon MyOldGuns in DroppedWeapons)
        //{

        //    Debugging.WriteToLog("WeaponInventoryChanged", string.Format("Dropped Gun {0},OldAmmo: {1}", MyOldGuns.Weapon.Hash, MyOldGuns.Ammo));

        //}




        //List<string> Bones = new List<string> { "SKEL_ROOT", "skel_root", "SKEL_Pelvis", "SKEL_PELVIS", "skel_pelvis", "SKEL_Spine_Root", "SKEL_SPINE_ROOT", "skel_spine_root", "SKEL_Spine0","SKEL_SPINE0","skel_spine0" };


        //foreach(string Stuff in Bones)
        //{
        //    if(Game.LocalPlayer.Character.HasBone(Stuff))
        //    {
        //        Debugging.WriteToLog("Bones", string.Format("I have bone: {0}", Stuff));
        //    }
        //}


        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 0);
        //Vector3 MyPosition = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Game.LocalPlayer.Character, BoneIndexSpine, 0f, 0f, 0f);
        // Debugging.WriteToLog("Bones", string.Format("Spine Bone?: {0}", MyPosition));

        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;




        //    //Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, ClosestVehicle, 0);
        //    //Vector3 CarPosition = ClosestVehicle.Position;
        //    //float DesiredHeading = ClosestVehicle.Heading - 90f;
        //    ////NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Game.LocalPlayer.Character, GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z, DesiredHeading, 3000);

        //    //uint GameTimeStarted = Game.GameTime;

        //    //while (Game.GameTime - GameTimeStarted <= 10000)
        //    //{
        //    //    Rage.Debug.DrawArrowDebug(new Vector3(GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
        //    //    GameFiber.Yield();
        //    //}


        //   // GameFiber.Sleep(3000);

        //}






    }
    private static void DebugNumpad5()
    {

        Settings.Logging = true;

        Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        float DistanceToPlayer = Game.LocalPlayer.Character.DistanceTo2D(CurrentWantedLevelPosition);
        float DistanceToPlacePlayerLastSeen = Game.LocalPlayer.Character.DistanceTo2D(Police.PlacePlayerLastSeen);
        WriteToLog("WantedLevel", string.Format("CenterPosition: {0},DistanceToPlayer: {1},PlacePlayerLastSeen: {2},DistanceToPlacePlayerLastSeen: {3}", CurrentWantedLevelPosition, DistanceToPlayer, Police.PlacePlayerLastSeen, DistanceToPlacePlayerLastSeen));

        //WriteToLog("WantedLevel2", string.Format("LastWantedCenterPosition: {0}", Police.LastWantedCenterPosition));

        // DispatchAudioSystem.ReportSuspiciousActivity();



        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return;
        DispatchAudio.ReportFelonySpeeding(InstantAction.GetPlayersCurrentTrackedVehicle(), 110f);
        GTAVehicle VehicleDescription = InstantAction.TrackedVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();
        Vehicle myCar = VehicleDescription.VehicleEnt;

        //if (myCar.Health <= 500 || myCar.EngineHealth <= 300)
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: Health: {0},Engine Health {1}", myCar.Health, myCar.EngineHealth));

        //if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar))
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: ARE_ALL_VEHICLE_WINDOWS_INTACT: {0}", NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar)));

        VehicleDoor[] CarDoors = myCar.GetDoors();

        foreach (VehicleDoor myDoor in CarDoors)
        {
            //if (myDoor.IsDamaged)
            Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: door {0} Is Damaged: {1}", myDoor.Index, myDoor.IsDamaged));
        }


        bool LightsOn;
        bool HighbeamsOn;

        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar, &LightsOn, &HighbeamsOn);
        }

        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IsStolen: {0},IsRoadWorthy: {1}, .CarPlate.IsWanted: {2},ColorMatchesDescription: {3},MatchesOriginalDescription: {4}", VehicleDescription.IsStolen, myCar.IsRoadWorthy(), VehicleDescription.CarPlate.IsWanted, VehicleDescription.ColorMatchesDescription, VehicleDescription.MatchesOriginalDescription));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: Night: {0},LightsOn: {1}, HighbeamsOn: {2},RightHeadlightDamaged: {3},LeftHeadlightDmaaged: {4}", Police.IsNightTime, LightsOn, HighbeamsOn, NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar), NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar)));

        //ReportGrandTheftAuto();
        // ReportSuspiciousVehicle(VehicleDescription);


        // Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: CurrentLicensePlateWanted: {0},IsStolen:{1},IsWanted:{2}", VehicleDescription.CurrentLicensePlateWanted,VehicleDescription.IsStolen,VehicleDescription.IsWanted));


        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 0: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 0, false)));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 1: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 1, false)));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 2: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 2, false)));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 3: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 3, false)));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 4: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 4, false)));
        Debugging.WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 5: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 5, false)));


        //ReportStolenVehicle(GetPlayersCurrentTrackedVehicle());

        Debugging.WriteToLog("Civilians", string.Format("Total Civilians: {0}", PoliceScanning.Civilians.Count()));






        //if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
        //    return;

        //GTAVehicle VehicleDescription = EnteredVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();

        //Color BaseColor = GetBaseColor(VehicleDescription.OriginalColor);
        //ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
        //VehicleInfo VehicleInformation = InstantAction.GetVehicleInfo(VehicleDescription);
        //string ManufacturerScannerFile;
        //if (VehicleInformation != null)
        //{
        //    ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
        //    Debugging.WriteToLog("", string.Format("Name: {0},Manufac {1}, ModelScanner {2},Color {3}", VehicleInformation.Name, VehicleInformation.Manufacturer, VehicleInformation.ModelScannerFile, LookupColor.BaseColor));
        //}
        //else
        //{
        //    Debugging.WriteToLog("", string.Format("Hash: {0},Name {1}", VehicleDescription.VehicleEnt.Model.Hash, VehicleDescription.VehicleEnt.Model.Name));
        //}




        //VehicleInfo ToReturn = VehicleLookup.Vehicles.Where(x => x.Name.ToUpper() == "CAVALCADE2").FirstOrDefault();
        //Debugging.WriteToLog("", string.Format("CAVALCADE2: Hash: {0},Name {1}", ToReturn.Hash, ToReturn.Name));

        //ReportStolenVehicle(VehicleDescription);

        //Rage.Object camera = new Rage.Object("prop_ing_camera_01", Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront * 2));



        //Rage.Object FoodBag = new Rage.Object("prop_tool_screwdvr01", Game.LocalPlayer.Character.GetOffsetPositionFront(2f));

        //int BoneIndex = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);



        ////FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1f, -0.1f, -0.1f), new Rotator(120.0f, 0.0f, 0.0f));
        //FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        ////camera.AttachTo(Game.LocalPlayer.Character, 28252, Vector3.Zero, Rotator.Zero);
        //GameFiber.Sleep(5000);

        //FoodBag.Delete();
        //camera.Delete();
    }
    private static void DebugNumpad6()
    {
        try
        {
            RespawnStopper.IsRunning = false;
            NativeFunction.CallByName<bool>("REQUEST_SCRIPT", "respawn_controller");


            NativeFunction.CallByName<bool>("REQUEST_SCRIPT", "selector");



            Game.HandleRespawn();
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);


            GameFiber.Sleep(5000);
            RespawnStopper.IsRunning = true;

            //Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);




            /*

            // Smoking.startPTFX("core", "ent_dst_concrete_large");

            //Smoking.Start();
            //if(!Smoking.PlayersCurrentCigarette.Exists())
            //{
            //    return;
            //}

            string PTFX = "core";
            string FX = "ent_anim_cig_exhale_mth_car";

            if (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
            {
                NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
                while (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
                    GameFiber.Sleep(50);
            }


            //NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
            //GameFiber.Sleep(200);
            NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, PTFX);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", FX, offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 1.0f, false, false, false);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 1f, false, false, false);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_ENTITY", FX, Smoking.PlayersCurrentCigarette, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.0f, false, false, false);
            Debugging.WriteToLog("DebugNumpad6", "StartLoop");
            int Particle = 0;

                Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 2.0f, false, false, false);
                GameFiber.Sleep(2500);
            
            NativeFunction.CallByName<int>("STOP_PARTICLE_FX_LOOPED", Particle, true);
            Debugging.WriteToLog("DebugNumpad6", "StopLoop");


            //uint TimeStarted = Game.GameTime;
            //GameFiber.StartNew(delegate
            //{
            //    while (Game.GameTime - TimeStarted <= 5000)
            //    {
            //        NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 2.0f, false, false, false);
            //       // NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", "ent_dst_concrete_large", offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 10.0f, false, false, false);
            //        GameFiber.Yield();
            //    }
            //});


            //GTAWeapon CurrentWeapon = Weapons.Where(x => x.Hash == (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash).First();

            //if (CurrentWeapon != null)
            //{
            //    WeaponVariation.WeaponComponent myComponent = WeaponComponentsLookup.Where(x => x.BaseWeapon == CurrentWeapon.Name && x.Name == "Suppressor").FirstOrDefault();
            //    if (myComponent == null)
            //    {
            //        Debugging.WriteToLog("DebugNumpad6", "No Component Found");
            //        return;
            //    }

            //    WeaponVariation Cool = new WeaponVariation(0);
            //    Cool.Components.Add(myComponent);

            //    ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)CurrentWeapon.Hash, Cool);
            //}

            //WeaponVariation DroppedGunVariation = GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            //foreach (WeaponVariation.WeaponComponent Comp in DroppedGunVariation.Components)
            //{
            //    Debugging.WriteToLog("GetWeaponVariation", string.Format("Name: {0},HashKey: {1},Hash: {2}", Comp.Name, Comp.HashKey, Comp.Hash));
            //}
            //Debugging.WriteToLog("GetWeaponVariation", string.Format("Tint: {0}", DroppedGunVariation.Tint));
            */
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("DebugApplyPoliceVariation", e.Message);
        }
    }
    private static void DebugNumpad7()
    {
        Settings.Logging = true;
        //Settings.Debug = true;
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsAlive))
        {
            Debugging.WriteToLog("Debug", string.Format("Cop: {0},Model.Name:{1},isTasked: {2},canSeePlayer: {3},DistanceToPlayer: {4},HurtByPlayer: {5},IssuedHeavyWeapon {6},TaskIsQueued: {7},TaskType: {8},WasRandomSpawn: {9},TaskFiber: {10},CurrentTaskStatus: {11},Agency: {12}",
                    Cop.CopPed.Handle, Cop.CopPed.Model.Name, Cop.isTasked, Cop.canSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.CopPed.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials));
        }
    }
    private static void DebugNumpad8()
    {
        if (PlayerLocation.PlayerCurrentStreet == null)
        {
            WriteToLog("PlayerCurrentStreet", "No STreet");
        }
        else
        {
            WriteToLog("PlayerCurrentStreet", PlayerLocation.PlayerCurrentStreet.Name);

        }

        Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false);


        // Smoking.Start();
    }
    private static void DebugNumpad9()
    {
        Game.DisplayNotification("Instant Action Deactivated");
        InstantAction.Dispose();
    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        // if (!Logging)
        //     return;
        //if (ProcedureString != "GetCarjackingAnimations")
        //    return;
        //StringBuilder sb = new StringBuilder();
        //sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        //File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        //sb.Clear();


        if (ProcedureString == "Error")
        {
            Game.DisplayNotification("Instant Action has Crashed and needs to be restarted");
        }

        if (Settings.Logging)
            Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }

}

