using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PoliceSpawning
{
    private static List<Vehicle> CreatedPoliceVehicles;
    private static List<Entity> CreatedEntities;
    // private static Vehicle NewsChopper;
    // private static List<GTANewsReporter> Reporters = new List<GTANewsReporter>();
    // private static uint K9Interval;
    //private static uint RandomCopInterval;
    //private static uint CleanupCopInterval;
    //private static uint K9Interval;
    private static RandomPoliceSpawn NextPoliceSpawn;

    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        //RandomCopInterval = 0;
        //CleanupCopInterval = 0;
        //K9Interval = 0;
        IsRunning = true;
        MainLoop();
    }
    private static void MainLoop()
    {
        //var stopwatch = new Stopwatch();
        //GameFiber.StartNew(delegate
        //{
        //    try
        //    {
        //        while (IsRunning)
        //        {
        //            stopwatch.Start();
        //            //if (Settings.SpawnPoliceK9 && Game.GameTime > K9Interval + 5555) // was 2000
        //            //{
        //            //    if (Game.LocalPlayer.WantedLevel > 0 && !InstantAction.PlayerInVehicle && PoliceScanning.K9Peds.Count < 3)
        //            //        CreateK9();
        //            //    MoveK9s();
        //            //    K9Interval = Game.GameTime;
        //            //}

        //            if (InstantAction.PlayerWantedLevel == 0 || Police.PlayerHasBeenWantedFor >= 15000)
        //            {
        //                if (Settings.SpawnRandomPolice && Game.GameTime > RandomCopInterval + 2000)
        //                {
        //                    if (PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)// && Game.LocalPlayer.WantedLevel == 0)
        //                        SpawnRandomCop();
        //                    RandomCopInterval = Game.GameTime;
        //                }   
        //                else if(Game.GameTime > CleanupCopInterval + 5000)
        //                {
        //                    RemoveFarAwayRandomlySpawnedCops();
        //                    CleanupCopInterval = Game.GameTime;
        //                }
        //            }
        //            stopwatch.Stop();
        //            if (stopwatch.ElapsedMilliseconds >= 16)
        //                LocalWriteToLog("PoliceSpawningTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
        //            stopwatch.Reset();
        //            GameFiber.Yield();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        InstantAction.Dispose();
        //        Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        //    }

        //});
    }
    public static void Dispose()
    {
        IsRunning = false;
        foreach (Entity ent in CreatedEntities)
        {
            if (ent.Exists())
            {
                Blip myBlip = ent.GetAttachedBlip();
                if (myBlip.Exists())
                {
                    myBlip.Delete();
                }
                ent.Delete();
            }   
        }
        CreatedEntities.Clear();
    }
    public static void SpawnRandomCop()
    {
        try
        {
            if (NextPoliceSpawn == null)
            {
                GetRandomSpawnLocation();
                return;
            }

            if (NextPoliceSpawn.IsFreeway && InstantAction.MyRand.Next(1, 11) <= 4)
            {
                SpawnCop(Agencies.SAHP, NextPoliceSpawn.SpawnLocation);
            }
            else if (NextPoliceSpawn.ZoneAtLocation.MainZoneAgency != null && NextPoliceSpawn.ZoneAtLocation.SecondaryZoneAgencies != null && NextPoliceSpawn.ZoneAtLocation.SecondaryZoneAgencies.Any())
            {
                int Value = InstantAction.MyRand.Next(1, 11);
                if (Value <= 7)
                    SpawnCop(NextPoliceSpawn.ZoneAtLocation.MainZoneAgency, NextPoliceSpawn.SpawnLocation);
                else
                    SpawnCop(NextPoliceSpawn.ZoneAtLocation.SecondaryZoneAgencies.PickRandom(), NextPoliceSpawn.SpawnLocation);
            }
            else if (NextPoliceSpawn.ZoneAtLocation.MainZoneAgency != null && NextPoliceSpawn.ZoneAtLocation.SecondaryZoneAgencies == null && !NextPoliceSpawn.ZoneAtLocation.SecondaryZoneAgencies.Any())
            {
                SpawnCop(NextPoliceSpawn.ZoneAtLocation.MainZoneAgency, NextPoliceSpawn.SpawnLocation);
            }
            else
            {
                SpawnCop(Agencies.LSPD, NextPoliceSpawn.SpawnLocation);
            }

            NextPoliceSpawn = null;
        }
        catch (Exception e)
        {
            LocalWriteToLog("SpawnRandomCop",e.Message + " : " + e.StackTrace);
        }

    }
    public static void GetRandomSpawnLocation()
    {
        NextPoliceSpawn = null;

        Vector3 SpawnLocation = Vector3.Zero;
        SpawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around2D(750f, 1500f));

        if (SpawnLocation == Vector3.Zero)
            return;

        if (SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)//250f
            return;

        if (PoliceScanning.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(SpawnLocation) <= 350f))//500f
            return;

        Zone ZoneName = Zones.GetZoneAtLocation(SpawnLocation);
        if (ZoneName == Zones.OCEANA)
            return;

        string StreetName = PlayerLocation.GetCurrentStreet(SpawnLocation);
        Street MyGTAStreet = Streets.GetStreetFromName(StreetName);

        NextPoliceSpawn = new RandomPoliceSpawn(SpawnLocation, ZoneName, MyGTAStreet != null && MyGTAStreet.isFreeway);
    }
    public static void RemoveFarAwayRandomlySpawnedCops()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.WasRandomSpawn))
        {
            if (Cop.DistanceToPlayer >= 2000f)//2000f
            {
                Police.DeleteCop(Cop);
            }
            else if (Cop.WasMarkedNonPersistent && Cop.DistanceToPlayer >= 1750f)//1750f
            {
                Police.MarkNonPersistent(Cop);
                break;
            }
            if (Cop.DistanceToPlayer >= 250f && Cop.Pedestrian.IsInAnyVehicle(false))//750f
            {
                if (Cop.Pedestrian.CurrentVehicle.Health <= 800 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 800)
                {
                    if (Cop.AssignedAgency == Agencies.LSPD || Cop.AssignedAgency == Agencies.LSSD)//Only repiar naturally spawning cars so they appear to have just been another spawned car
                    {
                        Cop.Pedestrian.CurrentVehicle.Repair();
                    }
                }
                else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
                {
                    Police.DeleteCop(Cop);
                }
            }
        }
        foreach(Vehicle PoliceCar in CreatedPoliceVehicles.Where(x => x.Exists() && x.IsEmpty))//cleanup abandoned police cars, either cop dies or he gets marked non persisitent
        {
            if(PoliceCar.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
            {
                PoliceCar.Delete();
            }
        }
        CreatedPoliceVehicles.RemoveAll(x => !x.Exists());
    }
    public static void SpawnCop(Agency _Agency, Vector3 SpawnLocation)
    {
        if (SpawnLocation == null)
            return;
        if (_Agency == null)
            return;

        var SpawnCop = SpawnCopPed(_Agency, SpawnLocation);
        Ped Cop = SpawnCop.Item1;
        bool isBike = SpawnCop.Item2;
        GameFiber.Yield();
        if (Cop == null)
            return;
        CreatedEntities.Add(Cop);
        Vehicle CopCar;
        if (isBike)
            CopCar = SpawnCopMotorcycle(_Agency, SpawnLocation);
        else
            CopCar = SpawnCopCruiser(_Agency, SpawnLocation);

        GameFiber.Yield();
        if (CopCar == null)
        {
            if(Cop.Exists())
                Cop.Delete();
            return;
        }
        CreatedPoliceVehicles.Add(CopCar);
        CreatedEntities.Add(CopCar);
        Cop.WarpIntoVehicle(CopCar, -1);
        Cop.IsPersistent = true;
        CopCar.IsPersistent = true;
        Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
        GTACop MyNewCop = new GTACop(Cop, false, Cop.Health, _Agency);
        Police.IssueCopPistol(MyNewCop);
        MyNewCop.WasRandomSpawn = true;
        MyNewCop.WasMarkedNonPersistent = true;
        MyNewCop.WasRandomSpawnDriver = true;
        MyNewCop.IsBikeCop = isBike;

        if (Settings.SpawnedRandomPoliceHaveBlip && Cop.Exists())
        {
            Blip myBlip = Cop.AttachBlip();
            myBlip.Color = _Agency.AgencyColor;
            myBlip.Scale = 0.6f;
            Police.CreatedBlips.Add(myBlip);
        }

        PoliceScanning.CopPeds.Add(MyNewCop);
        GameFiber.Yield();

        bool AddPartner = InstantAction.MyRand.Next(1, 11) <= 5;
        if (AddPartner && !isBike)
        {
            var PartnerSpawn = SpawnCopPed(_Agency, SpawnLocation);
            Ped PartnerCop = PartnerSpawn.Item1;
            GameFiber.Yield();
            if (PartnerCop == null)
                return;
            CreatedEntities.Add(PartnerCop);
            if(!CopCar.Exists())
            {
                if (PartnerCop.Exists())
                    PartnerCop.Delete();
                return;
            }
            PartnerCop.WarpIntoVehicle(CopCar, 0);
            PartnerCop.IsPersistent = true;
            GTACop MyNewPartnerCop = new GTACop(PartnerCop, false, PartnerCop.Health, _Agency);
            Police.IssueCopPistol(MyNewPartnerCop);
            MyNewPartnerCop.WasRandomSpawn = true;
            MyNewPartnerCop.WasMarkedNonPersistent = true;
            PoliceScanning.CopPeds.Add(MyNewPartnerCop);
        }
    }
    public static (Ped,bool) SpawnCopPed(Agency _Agency,Vector3 SpawnLocation)
    {
        bool isMale = true;
        bool isBike = false;
        if (_Agency == null)
            return (null,false);
        if (_Agency.CopModels.Any(x => !x.isMale))
            isMale = InstantAction.MyRand.Next(1, 11) <= 7; //70% chance Male

        if (_Agency == Agencies.SAHP)
            isBike = InstantAction.MyRand.Next(1, 11) <= 5; //50% bike cop for SAHP

        Agency.ModelInformation MyInfo = _Agency.CopModels.Where(x => x.isMale == isMale && x.UseForRandomSpawn).PickRandom();
        Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 10f);
        Ped Cop = new Ped(MyInfo.ModelName, SafeSpawnLocation, 0f);
        if (!Cop.Exists())
            return (null, false);

        NativeFunction.CallByName<bool>("SET_PED_AS_COP", Cop, true);
        Cop.RandomizeVariation();
        if (isBike)
        {
            Cop.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 0, 0, 0);
        }
        else
        {
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 1, 0, 0);
        }
        if (_Agency == Agencies.LSSD || _Agency == Agencies.LSPD || _Agency == Agencies.BCSO || _Agency == Agencies.LSIAPD)
        {
            if (isMale && InstantAction.MyRand.Next(1, 11) <= 4) //40% Chance of Vest
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
            if (!Police.IsNightTime)
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses
        }

        return (Cop,isBike);
    }
    public static Vehicle SpawnCopCruiser(Agency _Agency, Vector3 SpawnLocation)
    {
        string ModelName = "police4";
        Agency.VehicleInformation MyCar = _Agency.GetRandomVehicle(false);
        if (MyCar != null)
            ModelName = MyCar.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        GameFiber.Yield();
        if(CopCar.Exists())
        {
            UpgradeCruiser(CopCar);
            return CopCar;
        }
        else
        {
            return null;
        }
        
    }
    public static Vehicle SpawnCopMotorcycle(Agency _Agency, Vector3 SpawnLocation)
    {  
        string ModelName = "policeb";
        Agency.VehicleInformation MyVehicle = _Agency.GetRandomVehicle(true);
        if (MyVehicle != null)
            ModelName = MyVehicle.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        GameFiber.Yield();
        if (CopCar.Exists())
        {
            return CopCar;
        }
        else
        {
            return null;
        }
    }
    public static void UpgradeCruiser(Vehicle CopCruiser)
    {
        if (!CopCruiser.Exists())
            return;
        NativeFunction.CallByName<bool>("SET_VEHICLE_MOD_KIT", CopCruiser, 0);//Required to work
        NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", CopCruiser, 11, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", CopCruiser, 11) - 1, true);//Engine
        NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", CopCruiser, 12, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", CopCruiser, 12) - 1, true);//Brakes
        NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", CopCruiser, 13, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", CopCruiser, 13) - 1, true);//Tranny
        NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", CopCruiser, 15, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", CopCruiser, 15) - 1, true);//Suspension

        NativeFunction.CallByName<bool>("SET_VEHICLE_WINDOW_TINT", CopCruiser, 1);
    }
    public static void CreateK9()
    {
        try
        {
            GTACop ClosestDriver = PoliceScanning.CopPeds.Where(x => x.Pedestrian.IsInAnyVehicle(false) && !x.isInHelicopter && x.Pedestrian.CurrentVehicle.Driver == x.Pedestrian && x.Pedestrian.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (ClosestDriver != null)
            {
                Ped Doggo = new Ped("a_c_shepherd", ClosestDriver.Pedestrian.GetOffsetPosition(new Vector3(0f, -10f, 0f)), 180);
                Doggo.WarpIntoVehicle(ClosestDriver.Pedestrian.CurrentVehicle, 1);
                CreatedEntities.Add(Doggo);
                Doggo.BlockPermanentEvents = true;
                Doggo.IsPersistent = false;
                Doggo.RelationshipGroup = "COPDOGS";
                Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "COP", Relationship.Like);
                Game.SetRelationshipBetweenRelationshipGroups("COP", "COPDOGS", Relationship.Like);
                //Doggo.Health = 50;
                Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "PLAYER", Relationship.Hate);
                Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "COPDOGS", Relationship.Hate);
                GTACop DoggoCop = new GTACop(Doggo, false, Doggo.Health, ClosestDriver.AssignedAgency);
                PoliceScanning.K9Peds.Add(DoggoCop);
                Tasking.TaskK9(DoggoCop);
                LocalWriteToLog("CreateK9", String.Format("Created K9 ", Doggo.Handle));
            }
        }
        catch (Exception e)
        {
            LocalWriteToLog("CreateK9", e.Message);
        }
    }  
    private static void PutK9InCar(GTACop DoggoCop, GTACop Cop)
    {
        if (!Cop.Pedestrian.IsInAnyVehicle(false) || Cop.Pedestrian.IsOnBike || Cop.Pedestrian.IsInBoat || Cop.Pedestrian.IsInHelicopter)
            return;
        if (Cop.Pedestrian.CurrentVehicle.IsSeatFree(1))
            DoggoCop.Pedestrian.WarpIntoVehicle(Cop.Pedestrian.CurrentVehicle, 1);
        else
            DoggoCop.Pedestrian.WarpIntoVehicle(Cop.Pedestrian.CurrentVehicle, 2);
        LocalWriteToLog("PutK9InCar", String.Format("K9 {0}, put in Car", DoggoCop.Pedestrian.Handle));
    }
    public static void MoveK9s()
    {
        foreach (GTACop K9 in PoliceScanning.K9Peds)
        {
            if (K9.Pedestrian.IsInAnyVehicle(false))
            {
                GTACop ClosestDriver = PoliceScanning.CopPeds.Where(x => x.Pedestrian.IsInAnyVehicle(false) && !x.isInHelicopter && x.Pedestrian.CurrentVehicle.Driver == x.Pedestrian && x.Pedestrian.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                if (ClosestDriver != null)
                {
                    PutK9InCar(K9, ClosestDriver);
                }
            }
        }
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.PoliceSpawningLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
    //News Spawning
    //public static void SpawnNewsChopper()
    //{

    //    Ped NewsPilot = new Ped("s_m_m_pilot_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 400f)), 0f);
    //    CreatedEntities.Add(NewsPilot);
    //    Ped CameraMan = new Ped("ig_beverly", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 410f)), 0f);
    //    CreatedEntities.Add(CameraMan);
    //    Ped Assistant = new Ped("s_m_y_grip_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 420f)), 0f);
    //    CreatedEntities.Add(Assistant);
    //    NewsChopper = new Vehicle("maverick", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 500f)), NewsPilot.Heading);
    //    CreatedEntities.Add(NewsChopper);
    //    NewsPilot.WarpIntoVehicle(NewsChopper, -1);
    //    CameraMan.WarpIntoVehicle(NewsChopper, 1);
    //    Assistant.WarpIntoVehicle(NewsChopper, 2);
    //    NewsPilot.BlockPermanentEvents = true;
    //    CameraMan.BlockPermanentEvents = true;
    //    Assistant.BlockPermanentEvents = true;
    //    NativeFunction.CallByName<bool>("TASK_HELI_CHASE", NewsPilot, Game.LocalPlayer.Character, 25f, 25f, 40f);
    //    Reporters.Add(new GTANewsReporter(NewsPilot, false, NewsPilot.Health));
    //    Reporters.Add(new GTANewsReporter(CameraMan, false, CameraMan.Health));
    //    Reporters.Add(new GTANewsReporter(Assistant, false, Assistant.Health));
    //    NewsPilot.KeepTasks = true;
    //    LocalWriteToLog("SpawnNewsChopper", "News Chopper Spawned");
    //}
    //public static void SpawnNewsVan()
    //{

    //    Ped CameraMan = new Ped("ig_beverly", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 5f, 0f)), 0f);
    //    CreatedEntities.Add(CameraMan);
    //    Ped Assistant = new Ped("s_m_y_grip_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 5f, 0f)), 0f);
    //    CreatedEntities.Add(Assistant);




    //    //Rage.Object camera = new Rage.Object("prop_ing_camera_01", CameraMan.GetOffsetPosition(Vector3.RelativeTop * 30));
    //    //CameraMan.Tasks.PlayAnimation("anim@mp_player_intupperphotography", "idle_a_fp", 8.0F, AnimationFlags.Loop);

    //    //camera.AttachTo(CameraMan, 28252, Vector3.Zero, Rotator.Zero);

    //    //camera.Heading = CameraMan.Heading - 180;
    //    //camera.Position = CameraMan.GetOffsetPosition(Vector3.RelativeTop * 0.0f + Vector3.RelativeFront * 0.33f);
    //    //camera.IsPositionFrozen = true;


    //    //Vector3 SpawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around2D(50f));
    //    //Vehicle NewsVan = new Vehicle("rumpo", SpawnLocation, Assistant.Heading);
    //    //NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", NewsVan, 0);
    //    //NewsVan.PrimaryColor = Color.Gray;
    //    // CameraMan.WarpIntoVehicle(NewsVan, 0);
    //    //Assistant.WarpIntoVehicle(NewsVan, -1);
    //    CameraMan.BlockPermanentEvents = true;
    //    Assistant.BlockPermanentEvents = true;



    //    // NativeFunction.CallByName<bool>("TASK_VEHICLE_ESCORT",
    //    //NativeFunction.Natives.xFC545A9F0626E3B6(Assistant, NewsVan,Game.LocalPlayer.Character,40.0f, 262144, 10.0f);

    //    //Assistant.Tasks.ChaseWithGroundVehicle(Game.LocalPlayer.Character);

    //    //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Assistant, 100f);
    //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Assistant, 8f);
    //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Assistant, 32, true);

    //    //Assistant.Tasks.FollowToOffsetFromEntity(Game.LocalPlayer.Character, new Vector3(0f, -20f, 0f));
    //    Assistant.KeepTasks = true;

    //    //NewsTeam.Add(NewsVan);
    //    //NewsTeam.Add(CameraMan);
    //    //NewsTeam.Add(Assistant);
    //}
    //public static void DeleteNewsTeam()
    //{
    //    foreach (GTANewsReporter Reporter in Reporters)
    //    {
    //        if (Reporter.ReporterPed.Exists())
    //            Reporter.ReporterPed.Delete();
    //    }
    //    Reporters.Clear();
    //    if (NewsChopper.Exists())
    //        NewsChopper.Delete();
    //    LocalWriteToLog("DeleteNewsTeam", "News Team Deleted");
    //}
}
public class RandomPoliceSpawn
{
    public Vector3 SpawnLocation;
    public Zone ZoneAtLocation;
    public bool IsFreeway;
    public RandomPoliceSpawn(Vector3 _SpawnLocation,Zone _ZoneAtLocation,bool _IsFreeway)
    {
        SpawnLocation = _SpawnLocation;
        ZoneAtLocation = _ZoneAtLocation;
        IsFreeway = _IsFreeway;
    }
}

