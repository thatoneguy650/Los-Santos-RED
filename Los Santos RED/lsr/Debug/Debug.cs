using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
    public Debug(PlateTypes plateTypes, Mod.World world, Mod.Player targetable, IStreets streets, Dispatcher dispatcher, Zones zones, Crimes crimes)
    {
        PlateTypes = plateTypes;
        World = world;
        Player = targetable;
        Streets = streets;
        Dispatcher = dispatcher;
        Zones = zones;
        Crimes = crimes;
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

        //foreach(Cop cop in World.PoliceList.Where(x=> x.Pedestrian.Exists()))
        //{
        //    DrawColoredArrowTaskStatus(cop);
        //    DrawColoredArrowAlertness(cop);
        //}
        //foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 75f))
        //{
        //    Color Color = Color.Yellow;
        //    if(!ped.CanBeTasked)
        //    {
        //        Color = Color.Purple;
        //    }
        //    else if(ped.CurrentTask != null)
        //    {
        //        Color = Color.Black;
        //    }
        //    else if(ped.HasSeenPlayerCommitCrime)
        //    {
        //        Color = Color.Orange;
        //    }
        //    else if (ped.CanRecognizePlayer)
        //    {
        //        Color = Color.Green;
        //    }
        //    else if (ped.CanSeePlayer)
        //    {
        //        Color = Color.White;
        //    }
        //    else
        //    {
        //        Color = Color.Red;
        //    }
        //    Rage.Debug.DrawArrowDebug(ped.Pedestrian.Position + new Vector3(0f,0f,2f), Vector3.Zero, Rotator.Zero, 1f, Color);
       // }
    }
    private void DebugNumpad0()
    {
        MakeNonInvincible();
    }
    private void DebugNumpad1()
    {
        MakeInvincible();
    }
    private void DebugNumpad2()
    {
        Player.SetWantedLevel(0, "RESETTING DEBUG!", true);
    }
    private void DebugNumpad3()
    {
        Player.SetWantedLevel(2, "SETTING DEBUG!", true);
    }
    private void DebugNumpad4()
    {
        if (DebugPed.Exists())
        {
            DebugPed.Delete();
        }
        //Vector3 Pos = Game.LocalPlayer.Character.GetOffsetPositionFront(3f);
        ////Cop = new Ped("s_m_y_cop_01", Game.LocalPlayer.Character.GetOffsetPositionFront(3f), Game.LocalPlayer.Character.Heading);
        //Cop = NativeFunction.Natives.CREATE_PED<Ped>(6, Game.GetHashKey("s_m_y_cop_01"), Pos.X, Pos.Y, Pos.Z, Game.LocalPlayer.Character.Heading, false, false);
        //Cop.RelationshipGroup = RelationshipGroup.Cop;
    }
    private void DebugNumpad5()
    {
        Player.DebugScanner.Reset();
        if (RandomItems.RandomPercent(50))
        {
            Player.DebugScanner.OnWantedSearchMode();
            EntryPoint.WriteToConsole("Announcing OnWantedSearchMode", 3);
        }
        else
        {
            Player.DebugScanner.OnAppliedWantedStats();
            EntryPoint.WriteToConsole("Announcing OnAppliedWantedStats", 3);
        }
       // Dispatcher.SpawnRegularRoadblock();
        //SpawnRegularRoadblock();


        //VehicleMissionFlag++;
        //EntryPoint.WriteToConsole($"VehicleMissionFlag {VehicleMissionFlag}",4);
        //DebugPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(3f), Game.LocalPlayer.Character.Heading);
        //DebugPed.Inventory.GiveNewWeapon(new WeaponAsset("weapon_pistol"), 60, true);
        //DebugPed.Tasks.FightAgainst(Game.LocalPlayer.Character);


        //EntryPoint.WriteToConsole("HandleRespawn RUN");
        //Game.HandleRespawn();
    }
    private void DebugNumpad6()
    {
        Player.DebugScanner.Reset();
        Crime ToAnnounce = Crimes.CrimeList.PickRandom();
        if(ToAnnounce != null)
        {
            Player.DebugScanner.AnnounceCrime(ToAnnounce, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position, true));
            EntryPoint.WriteToConsole($"Announcing {ToAnnounce.Name}", 3);
        }

        //Player.PrintCriminalHistory();
        //EntryPoint.WriteToConsole("-------------------------------", 3);
        //EntryPoint.WriteToConsole($" CurrentVehicle.Vehicle.Handle: {Player.CriminalHistoryDebug.ToString()}", 3);
        //EntryPoint.WriteToConsole("-------------------------------", 3);
        //if(VehicleMissionFlag > 0)
        //{
        //    VehicleMissionFlag--;
        //}
        //EntryPoint.WriteToConsole($"VehicleMissionFlag {VehicleMissionFlag}", 4);
    }
    private void DebugNumpad7()
    {
        EntryPoint.WriteToConsole("SCANNER ABORT", 3);
        Player.DebugScanner.Abort();
        //if(Game.LocalPlayer.Character.Inventory.EquippedWeaponObject != null && Player.CurrentWeapon != null)
        //{
        //    EntryPoint.WriteToConsole($" Weapon Dimensions: Model {Player.CurrentWeapon.ModelName}, CurrentWeaponIsOneHanded{Player.CurrentWeaponIsOneHanded}, X: {Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.X}, Y: {Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.Y}, Z: {Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.Z}", 3);

        //}


        if (Player.CurrentVehicle != null)
        {
            EntryPoint.WriteToConsole("-------------------------------", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.Vehicle.Handle: {Player.CurrentVehicle.Vehicle.Handle}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.IsStolen: {Player.CurrentVehicle.IsStolen}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.WasReportedStolen: {Player.CurrentVehicle.WasReportedStolen}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.CopsRecognizeAsStolen: {Player.CurrentVehicle.CopsRecognizeAsStolen}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.NeedsToBeReportedStolen: {Player.CurrentVehicle.NeedsToBeReportedStolen}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.GameTimeToReportStolen: {Player.CurrentVehicle.GameTimeToReportStolen}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.IsWanted: {Player.CurrentVehicle.CarPlate.IsWanted}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.PlateNumber: {Player.CurrentVehicle.CarPlate.PlateNumber}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.IsWanted: {Player.CurrentVehicle.OriginalLicensePlate.IsWanted}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.PlateNumber: {Player.CurrentVehicle.OriginalLicensePlate.PlateNumber}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.HasOriginalPlate: {Player.CurrentVehicle.HasOriginalPlate}", 3);
            EntryPoint.WriteToConsole($" CurrentVehicle.HasBeenDescribedByDispatch: {Player.CurrentVehicle.HasBeenDescribedByDispatch}", 3);
            EntryPoint.WriteToConsole("-------------------------------", 3);
        }


        //if(Player.CurrentVehicle != null)
        //{
        //    UpdatePlate(Player.CurrentVehicle);
        //}

        // SpawnInteractiveChaser(1f);
    }
    public void DebugNumpad8()
    {
        //SpawnInteractiveChaser(5f);


        EntryPoint.WriteToConsole("===================================", 4);
        foreach (Cop cop in World.PoliceList.OrderBy(x => x.DistanceToPlayer))
        {
            //int rel1 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(Game.LocalPlayer.Character, cop.Pedestrian);
            //int rel2 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(cop.Pedestrian, Game.LocalPlayer.Character);

            if (cop.Pedestrian.CurrentVehicle.Exists())
            {
                //int MissionType = NativeFunction.Natives.GET_ACTIVE_VEHICLE_MISSION_TYPE<int>(cop.Pedestrian.CurrentVehicle);
                //EntryPoint.WriteToConsole(cop.DebugString + $"MissionType {MissionType}", 4);
                EntryPoint.WriteToConsole(cop.DebugString + $" WasModSpawned: {cop.WasModSpawned}", 4);
            }


        }
        EntryPoint.WriteToConsole("===================================", 4);
    }
    private void DebugNumpad9()
    {
        MakeSober();
        TerminateMod();
    }
    private void TerminateMod()
    {
        if (DebugPed.Exists())
        {
            DebugPed.Delete();
        }
        EntryPoint.ModController.Dispose();
        Game.LocalPlayer.WantedLevel = 0;
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.DisplayNotification("Instant Action Deactivated");
    }
    public void UpdatePlate(VehicleExt vehicleExt)//this might need to come out of here.... along with the two bools
    {
        vehicleExt.HasUpdatedPlateType = true;
        PlateType CurrentType = PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle));
        string CurrentPlateNumber = vehicleExt.Vehicle.LicensePlate;
        Zone CurrentZone = Zones.GetZone(vehicleExt.Vehicle.Position);


        /*
         * 
         *TEMP HERE UNTIL I DECIDE
         * 
         * 
         * */
        if (CurrentZone != null && CurrentZone.State != "San Andreas")//change the plates based on state
        {
            PlateType NewType = PlateTypes.GetPlateType(CurrentZone.State);

            if (NewType != null)
            {
                EntryPoint.WriteToConsole($"Zone State: {CurrentZone.State} Plate State {NewType.State} Index {NewType.Index} Index+1 {NewType.Index + 1}",3);
                string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                if (NewPlateNumber != "")
                {
                    vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
                    vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                    vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
                }
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle, NewType.Index);
                vehicleExt.OriginalLicensePlate.PlateType = NewType.Index;
                vehicleExt.CarPlate.PlateType = NewType.Index;
                EntryPoint.WriteToConsole(string.Format("Update Plater Updated {0} {1}", vehicleExt.Vehicle.Model.Name, NewType.Index),3);
            }
        }
        else
        {
            if (RandomItems.RandomPercent(10) && CurrentType != null && CurrentType.CanOverwrite && vehicleExt.CanUpdatePlate)
            {
                PlateType NewType = PlateTypes.GetRandomPlateType();
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
                        vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle, NewType.Index + 1);
                    vehicleExt.OriginalLicensePlate.PlateType = NewType.Index;
                    vehicleExt.CarPlate.PlateType = NewType.Index;
                    EntryPoint.WriteToConsole(string.Format("UpdatePlate Updated {0} {1}", vehicleExt.Vehicle.Model.Name, NewType.Index),3);
                }
            }
        }

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
    private void SpawnRegularRoadblock()
    {
        //Vector3 Position = Player.Character.GetOffsetPositionFront(50f);
        //Street ForwardStreet = Streets.GetStreet(Position);
        //if (ForwardStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name)
        //{
        //    EntryPoint.WriteToConsole("ROADBLOCK: Street Matches", 3);
        //    if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
        //    {
        //        Roadblock rb = new Roadblock(Player, World, new Agency(),"policet", CenterPosition) ;
        //        rb.SpawnRoadblock();
        //        EntryPoint.WriteToConsole("ROADBLOCK: Spawned", 3);
        //    }
        //}
        //else
        //{
        //    EntryPoint.WriteToConsole("ROADBLOCK: Street DOES NOT Matche", 3);
        //}

    }
    private void SpawnRoadblock()
    {


        Vector3 Position = Player.Character.GetOffsetPositionFront(20f);
        if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
        {


            float VehicleHeading = Heading;// - 90f;
            float AdaptedVehicleHeading = VehicleHeading;

            if(VehicleHeading < 0)
            {
                AdaptedVehicleHeading += 360f;
            }
            EntryPoint.WriteToConsole($"ROADBLOCK: VehicleHeading {VehicleHeading} AdaptedVehicleHeading {AdaptedVehicleHeading}", 3);
            Vector3 FrontVector = new Vector3((float)Math.Cos(VehicleHeading * Math.PI / 180), (float)Math.Sin(VehicleHeading * Math.PI / 180), 0);
            Vector3 AdaptedFrontVector = new Vector3((float)Math.Cos(AdaptedVehicleHeading * Math.PI / 180), (float)Math.Sin(AdaptedVehicleHeading * Math.PI / 180), 0);


            Vector3 spawnpos = CenterPosition + (FrontVector * 2f);
            Vector3 AdapatedSpawnPos = CenterPosition + (AdaptedFrontVector * 2f);
            while (!Game.IsKeyDownRightNow(Keys.E))
            {
                Game.DisplayHelp("Press E to delete roadblock");


                Rage.Debug.DrawArrowDebug(new Vector3(AdapatedSpawnPos.X, AdapatedSpawnPos.Y, AdapatedSpawnPos.Z + 1f), AdaptedFrontVector, Rotator.Zero, 1f, Color.Green);
                Rage.Debug.DrawArrowDebug(new Vector3(CenterPosition.X, CenterPosition.Y, CenterPosition.Z + 1f), AdaptedFrontVector, Rotator.Zero, 1f, Color.Purple);



                Rage.Debug.DrawArrowDebug(spawnpos, FrontVector, Rotator.Zero, 1f, Color.White);        
                Rage.Debug.DrawArrowDebug(CenterPosition, FrontVector, Rotator.Zero, 1f, Color.Red);
                GameFiber.Yield();


            }
        }

        //Vector3 Position = Player.Character.GetOffsetPositionFront(50f);
        //Street ForwardStreet = Streets.GetStreet(Position);
        //if(ForwardStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name)
        //{
        //    EntryPoint.WriteToConsole("ROADBLOCK: Street Matches",3);
        //    if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
        //    {
        //        Roadblock rb = new Roadblock("policet", CenterPosition);
        //        rb.SpawnRoadblock();
        //        EntryPoint.WriteToConsole("ROADBLOCK: Spawned", 3);
        //    }
        //}
        //else
        //{
        //    EntryPoint.WriteToConsole("ROADBLOCK: Street DOES NOT Matche", 3);
        //}















        //if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Player.Position.X, Player.Position.Y, Player.Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
        //{
        //    string ModelName = "policet";          
        //    Model Model = new Model(ModelName);
        //    double DesiredHeading = Heading - 90f;
        //    var angle = DesiredHeading * Math.PI / 180;
        //    Vector3 FrontVector = new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0);

        //    bool SpawnWorked = true;
        //    while(SpawnWorked)
        //    {
        //        AddRoadblockCar();
        //    }

        //    Vector3 FrontPos = CenterPosition + (FrontVector * (Model.Dimensions.Length() + 2f));

        //    List<Vector3> Positions = new List<Vector3>();

        //    Positions.Add(CenterPosition);
        //    Positions.Add(FrontPos);


        //    GameFiber.StartNew(delegate
        //    {
        //        try
        //        {
        //            while (!Game.IsKeyDownRightNow(Keys.E))
        //            {
        //                Game.DisplayHelp("Press E to delete roadblock");
        //                foreach(Vector3 Position in Positions)
        //                {
        //                    Rage.Debug.DrawArrowDebug(new Vector3(Position.X, Position.Y, Position.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.White);
        //                }
        //                GameFiber.Sleep(25);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
        //        }
        //    }, "DebugLoop2");

        //}
    }
    //private Vector3 AddRoadblockCar(Vector3 InitialPosition, bool AddInFront)
    //{
    //    return Vector3.Zero;
    //}
    private void SpawnInteractiveChaser(float Distance)
    {
        Ped newped = new Ped("a_f_m_business_02", Game.LocalPlayer.Character.GetOffsetPositionFront(2f), Game.LocalPlayer.Character.Heading);
        Vehicle car = new Vehicle("police", Game.LocalPlayer.Character.GetOffsetPositionFront(-8f), Game.LocalPlayer.Character.Heading);
        if (newped.Exists() && car.Exists())
        {

            car.IsCollisionProof = true;
            //  car.IsCollisionEnabled = false;

            //  NativeFunction.Natives.SET_ENTITY_COLLISION(car, false, false);

            newped.WarpIntoVehicle(car, -1);
            GameFiber.StartNew(delegate
            {
                try
                {

                    PedExt Coolio = new PedExt(newped, false, false, false, "Test1", null);
                    Coolio.Update(Player, Player.Position);
                    Coolio.CurrentTask = new Chase(Coolio, Player, Distance, VehicleMissionFlag);
                    Coolio.CurrentTask.Start();

                    //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", newped, 100f);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", newped, 8f);
                    car.AttachBlip();

                    uint GameTimeColliisonCheck = Game.GameTime;
                    while (newped.Exists() && newped.IsAlive && !Game.IsKeyDownRightNow(Keys.E))
                    {
                        Game.DisplayHelp("Press E to delete chaser");



                        if (Coolio.DistanceToPlayer <= 20f)
                        {
                            car.IsCollisionProof = false;
                        }
                        else
                        {
                            car.IsCollisionProof = true;
                        }


                        Game.DisplayNotification($"{Coolio.CurrentTask?.Name} AND {Coolio.CurrentTask?.SubTaskName}");

                        //if (Game.GameTime - GameTimeColliisonCheck >= 1000)
                        //{


                        //    VehicleExt ClosestCar = World.CivilianVehicleList.Where(x => x.Vehicle.Handle != car.Handle).OrderBy(x => car.DistanceTo2D(x.Vehicle)).FirstOrDefault();
                        //    if (ClosestCar != null && ClosestCar.Vehicle.Exists())
                        //    {
                        //        car.CollisionIgnoredEntity = ClosestCar.Vehicle;
                        //    }
                        //}
                        // Rage.World.GetClosestEntity(car.Position, 10f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);

                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 4, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 8, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 16, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 32, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 4194304, true);

                        //262144 - Take shortest path (Removes most pathing limits, the driver even goes on dirtroads)
                        //4194304 - Ignore roads (Uses local pathing, only works within 200~ meters around the player)

                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 4, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 8, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 16, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 32, true);
                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 512, true);
                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 262144, true);

                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 4194304, true);



                        Coolio.CurrentTask.Update();
                        //GameFiber.Sleep(250);
                        GameFiber.Sleep(25);
                        //GameFiber.Yield();
                    }
                    if (newped.Exists())
                    {
                        newped.Delete();
                    }
                    if (car.Exists())
                    {
                        car.Delete();
                    }
                }
                catch (Exception e)
                {
                    if (newped.Exists())
                    {
                        newped.Delete();
                    }
                    if (car.Exists())
                    {
                        car.Delete();
                    }
                    //EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace);
                }
            }, "DebugLoop2");
            //List<Vehicle> CreatedCars = new List<Vehicle>();
            //Vehicle MiddleCar = new Vehicle(ModelName, CenterPosition, Heading - 90f);
            //GameFiber.Yield();
            //CreatedCars.Add(MiddleCar);
            //Vector3 FrontPos = MiddleCar.GetOffsetPositionFront(MiddleCar.Model.Dimensions.Length() + 2f);//car.ForwardVector * 3.0f;// (car.Model.Dimensions.Length() / 2) * car.ForwardVector;
            //Vector3 RearPos = MiddleCar.GetOffsetPositionFront((-1.0f * MiddleCar.Model.Dimensions.Length()) + -2f);
            //Vehicle FrontCar = new Vehicle(ModelName, FrontPos, Heading - 90f);
            //GameFiber.Yield();
            //CreatedCars.Add(FrontCar);
            //Vehicle RearCar = new Vehicle(ModelName, RearPos, Heading - 90f);
            //GameFiber.Yield();
            //CreatedCars.Add(RearCar);
            //if (MiddleCar.Exists())
            //{
            //    GameFiber.StartNew(delegate
            //    {
            //        try
            //        {
            //            uint GameTimeColliisonCheck = Game.GameTime;
            //            while (MiddleCar.Exists() && !Game.IsKeyDownRightNow(Keys.E))
            //            {
            //                Game.DisplayHelp("Press E to delete roadblock");

            //                Rage.Debug.DrawArrowDebug(new Vector3(RearPos.X, RearPos.Y, RearPos.Z + 1f), Vector3.Zero, Rotator.Zero, 1f, Color.White);

            //                GameFiber.Sleep(25);
            //            }
            //            foreach (Vehicle car in CreatedCars)
            //            {
            //                if (car.Exists())
            //                {
            //                    car.Delete();
            //                }
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            foreach (Vehicle car in CreatedCars)
            //            {
            //                if (car.Exists())
            //                {
            //                    car.Delete();
            //                }
            //            }
            //            EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
            //        }
            //    }, "DebugLoop2");

            //}
        }
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

}

