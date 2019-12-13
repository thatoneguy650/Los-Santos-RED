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
    private static readonly List<Entity> CreatedEntities = new List<Entity>();
    private static readonly Random rnd;
   // private static Vehicle NewsChopper;
   // private static List<GTANewsReporter> Reporters = new List<GTANewsReporter>();
   // private static uint K9Interval;
    private static uint RandomCopInterval;
    private static uint CleanupCopInterval;
    public static bool IsRunning { get; set; } = true;
    static PoliceSpawning()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        MainLoop();
    }
    private static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();
                    //if (Settings.SpawnPoliceK9 && Game.GameTime > K9Interval + 5555) // was 2000
                    //{
                    //    if (Game.LocalPlayer.WantedLevel > 0 && 1==0 && !InstantAction.PlayerInVehicle && PoliceScanning.K9Peds.Count < 3)
                    //        CreateK9();
                    //    MoveK9s();
                    //    K9Interval = Game.GameTime;
                    //}


                    if (Settings.SpawnRandomPolice && Game.GameTime > RandomCopInterval + 2000)
                    {
                        if (PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)// && Game.LocalPlayer.WantedLevel == 0)
                            SpawnRandomCop();
                        RandomCopInterval = Game.GameTime;
                    }   
                    else if(Game.GameTime > CleanupCopInterval + 5000)
                    {
                        RemoveFarAwayRandomlySpawnedCops();
                        CleanupCopInterval = Game.GameTime;
                    }

                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        LocalWriteToLog("PoliceSpawningTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }

        });
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
            Vector3 SpawnLocation = Vector3.Zero;

            SpawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around2D(750f, 1500f));

            if (SpawnLocation == Vector3.Zero)
                return;

            if (SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) <= 250f)
                return;

            if (PoliceScanning.CopPeds.Any(x => x.CopPed.DistanceTo2D(SpawnLocation) <= 500f))
                return;


            Zone ZoneName = Zones.GetZoneAtLocation(SpawnLocation);
            string StreetName = PlayerLocation.GetCurrentStreet(SpawnLocation);
            Street MyGTAStreet = Streets.GetStreetFromName(StreetName);

            if (ZoneName == null || (MyGTAStreet != null && MyGTAStreet.isFreeway && rnd.Next(1, 11) <= 4))
            {
                SpawnCop(Agencies.SAHP, SpawnLocation);
            }
            else if (ZoneName.MainZoneAgency != null && ZoneName.SecondaryZoneAgencies.Any())
            {
                int Value = rnd.Next(1, 11);
                if (Value <= 7)
                    SpawnCop(ZoneName.MainZoneAgency, SpawnLocation);
                else
                    SpawnCop(ZoneName.SecondaryZoneAgencies.PickRandom(), SpawnLocation);
            }
            else if (ZoneName.MainZoneAgency != null && !ZoneName.SecondaryZoneAgencies.Any())
            {
                SpawnCop(ZoneName.MainZoneAgency, SpawnLocation);
            }
            else
            {
                SpawnCop(Agencies.LSPD, SpawnLocation);
            }
        }
        catch (Exception e)
        {
            LocalWriteToLog("SpawnRandomCop",e.Message + " : " + e.StackTrace);
        }

    }
    public static void RemoveFarAwayRandomlySpawnedCops()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && x.WasRandomSpawn))
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
            if (Cop.DistanceToPlayer >= 250f && Cop.CopPed.IsInAnyVehicle(false))//750f
            {
                if (Cop.CopPed.CurrentVehicle.Health <= 800 || Cop.CopPed.CurrentVehicle.EngineHealth <= 800)
                {
                    if (Cop.AssignedAgency == Agencies.LSPD || Cop.AssignedAgency == Agencies.LSSD)//Only repiar naturally spawning cars so they appear to have just been another spawned car
                    {
                        Cop.CopPed.CurrentVehicle.Repair();
                    }
                }
                else if (Cop.CopPed.CurrentVehicle.Health <= 600 || Cop.CopPed.CurrentVehicle.EngineHealth <= 600 || Cop.CopPed.CurrentVehicle.IsUpsideDown)
                {
                    Police.DeleteCop(Cop);
                }
            }
        }
    }
    public static void SpawnCop(Agency _Agency, Vector3 SpawnLocation)
    {
        if (SpawnLocation == null)
            return;
        if (_Agency == null)
            return;
        bool isBikeCop = rnd.Next(1, 11) <= 9; //90% chance Bike Cop
        Ped Cop = SpawnCopPed(_Agency, SpawnLocation, isBikeCop);
        GameFiber.Yield();
        if (Cop == null)
            return;
        CreatedEntities.Add(Cop);
        Vehicle CopCar = SpawnCopCruiser(_Agency, SpawnLocation, isBikeCop);
        GameFiber.Yield();
        if (CopCar == null)
        {
            if(Cop.Exists())
                Cop.Delete();
            return;
        }
        CreatedEntities.Add(CopCar);
        Cop.WarpIntoVehicle(CopCar, -1);
        Cop.IsPersistent = true;
        CopCar.IsPersistent = true;
        Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
        //Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections);
        GTACop MyNewCop = new GTACop(Cop, false, Cop.Health, _Agency);
        Police.IssueCopPistol(MyNewCop);
        MyNewCop.WasRandomSpawn = true;
        MyNewCop.WasMarkedNonPersistent = true;
        MyNewCop.WasRandomSpawnDriver = true;
        MyNewCop.IsBikeCop = isBikeCop;

        if (Settings.SpawnedRandomPoliceHaveBlip)
        {
            Blip myBlip = Cop.AttachBlip();
            myBlip.Color = _Agency.AgencyColor;
            myBlip.Scale = 0.6f;
        }

        PoliceScanning.CopPeds.Add(MyNewCop);
        GameFiber.Yield();

        bool AddPartner = rnd.Next(1, 11) <= 5;
        if (AddPartner && !isBikeCop)
        {
            Ped PartnerCop = SpawnCopPed(_Agency, SpawnLocation, false);
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

        //LocalWriteToLog("SpawnCop", string.Format("CopSpawned: Handled {0},Agency{1},AddedPartner{2}", Cop.Handle, _Agency.Initials, AddPartner));
    }
    public static Ped SpawnCopPed(Agency _Agency,Vector3 SpawnLocation, bool IsBikeCop)
    {
        bool isMale = true;
        if (_Agency == null)
            return null;
        if (_Agency.CopModels.Any(x => !x.isMale))
            isMale = rnd.Next(1, 11) <= 7; //70% chance Male

        Agency.ModelInformation MyInfo = _Agency.CopModels.Where(x => x.isMale == isMale && x.UseForRandomSpawn).PickRandom();
        Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 10f);
        Ped Cop = new Ped(MyInfo.ModelName, SafeSpawnLocation, 0f);
        if (!Cop.Exists())
            return null;

        NativeFunction.CallByName<bool>("SET_PED_AS_COP", Cop, true);
        Cop.RandomizeVariation();
        if (IsBikeCop)
        {
            Cop.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 0, 0, 0);
        }
        else
        {
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 1, 0, 0);
        }
        if (_Agency == Agencies.LSSD || _Agency == Agencies.LSPD)
        {
            if (isMale && rnd.Next(1, 11) <= 4) //40% Chance of Vest
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
            if (!Police.IsNightTime)
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses
        }

        return Cop;
    }
    public static Vehicle SpawnCopCruiser(Agency _Agency, Vector3 SpawnLocation, bool IsBikeCop)
    {
        string CarModel;
        int RandomValue = rnd.Next(1, 20);
        if (_Agency == Agencies.LSPD)
        {
            if (RandomValue <= 5)
                CarModel = "police3";
            else if (RandomValue <= 10)
                CarModel = "police2";
            else if (RandomValue <= 12)
                CarModel = "police4";
            else if (RandomValue <= 15)
                CarModel = "fbi2";
            else
                CarModel = "police";
        }
        else if (_Agency == Agencies.LSSD)
        {
            if (RandomValue <= 10)
                CarModel = "sheriff2";
            else
                CarModel = "sheriff";
        }
        else if (_Agency == Agencies.DOA)
        {
            CarModel = "police4";
        }
        else if (_Agency == Agencies.SAPR)
        {
            CarModel = "pranger";
        }
        else if (_Agency == Agencies.FIB)
        {
            if (RandomValue <= 10)
                CarModel = "fbi";
            else
                CarModel = "fbi2";
        }
        else if (_Agency == Agencies.IAA)
        {
            CarModel = "police4";
        }
        else if (_Agency == Agencies.SAHP)
        {
            if (IsBikeCop)
                CarModel = "policeb";
            else
                CarModel = "police4";
        }
        else if (_Agency == Agencies.PRISEC || _Agency == Agencies.LSPA)
        {
            CarModel = "police4";
        }
        else if (_Agency == Agencies.SASPA)
        {
            if (RandomValue <= 6)
                CarModel = "police4";
            else if (RandomValue <= 16)
                CarModel = "policet";
            else if (RandomValue <= 18)
                CarModel = "fbi2";
            else
                CarModel = "fbi";

            //CarModel = "policet";
        }
        else//fall back to unmarked, goes with everyone
        {
            CarModel = "police4";
        }

        Vehicle CopCar = new Vehicle(CarModel, SpawnLocation, 0f);
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
            GTACop ClosestDriver = PoliceScanning.CopPeds.Where(x => x.CopPed.IsInAnyVehicle(false) && !x.isInHelicopter && x.CopPed.CurrentVehicle.Driver == x.CopPed && x.CopPed.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (ClosestDriver != null)
            {
                Ped Doggo = new Ped("a_c_shepherd", ClosestDriver.CopPed.GetOffsetPosition(new Vector3(0f, -10f, 0f)), 180);
                Doggo.WarpIntoVehicle(ClosestDriver.CopPed.CurrentVehicle, 1);
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
        if (!Cop.CopPed.IsInAnyVehicle(false) || Cop.CopPed.IsOnBike || Cop.CopPed.IsInBoat || Cop.CopPed.IsInHelicopter)
            return;
        if (Cop.CopPed.CurrentVehicle.IsSeatFree(1))
            DoggoCop.CopPed.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 1);
        else
            DoggoCop.CopPed.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 2);
        LocalWriteToLog("PutK9InCar", String.Format("K9 {0}, put in Car", DoggoCop.CopPed.Handle));
    }
    public static void MoveK9s()
    {
        foreach (GTACop K9 in PoliceScanning.K9Peds)
        {
            if (K9.CopPed.IsInAnyVehicle(false))
            {
                GTACop ClosestDriver = PoliceScanning.CopPeds.Where(x => x.CopPed.IsInAnyVehicle(false) && !x.isInHelicopter && x.CopPed.CurrentVehicle.Driver == x.CopPed && x.CopPed.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                if (ClosestDriver != null)
                {
                    PutK9InCar(K9, ClosestDriver);
                }
            }
        }
    }
    //public static void RemoveAllRandomlySpawnedCops()
    //{
    //    foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && x.WasRandomSpawn))
    //    {
    //        if (Cop.DistanceToPlayer >= 250f)
    //        {
    //            Police.MarkNonPersistent(Cop);
    //            break;
    //        }
    //    }
    //}
    //public static void CleanupPolice()
    //{
    //    if (InstantAction.PlayerWantedLevel > 0)
    //        return;
    //    foreach(Entity myEnt in CreatedEntities)
    //    {
    //        if(myEnt.Exists())
    //        {
    //            if(myEnt.DistanceTo2D(Game.LocalPlayer.Character) >= 350f)
    //            {
    //                if(myEnt is Vehicle)
    //                {

    //                }
    //                else if (myEnt is Ped)
    //                {

    //                }
    //            }
    //        }
    //    }
    //}
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

