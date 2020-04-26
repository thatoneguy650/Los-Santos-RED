using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PoliceSpawning
{
    private static List<Vehicle> CreatedPoliceVehicles;
    private static List<Entity> CreatedEntities;
    private static RandomPoliceSpawn NextPoliceSpawn;
    private static RandomPoliceSpawn NextPoliceInvestigationSpawn;
    private static uint GameTimeLastSpawnedActiveChaseCop;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        IsRunning = true;
    }
    public static bool CanSpawnActiveChaseCop
    {
        get
        {
            if (GameTimeLastSpawnedActiveChaseCop == 0)
                return true;
            else if (Game.GameTime - GameTimeLastSpawnedActiveChaseCop >= 1000)//500 for start? set to setting later
                return true;
            else
                return false;
        }
    }
    public static int ExtraCopSpawnLimit
    {
        get
        {
            int CurrentWantedLevel = LosSantosRED.PlayerWantedLevel;
            if (CurrentWantedLevel == 1)//set as parameters
                return 0;
            else if (CurrentWantedLevel == 2)
                return 2;
            else if (CurrentWantedLevel == 3)
                return 6;
            else if (CurrentWantedLevel == 4)
                return 10;
            else if (CurrentWantedLevel == 5)
                return 12;
            else
                return 0;
        }
    }
    public static void RandomCopTick()
    {
        if (LosSantosRED.PlayerIsNotWanted)
        {
            if (PedScanning.CopPeds.Where(x => x.WasModSpawned).Count() < LosSantosRED.MySettings.Police.SpawnAmbientPoliceLimit)
            {
                SpawnAmbientCop();
            }
            else if (Police.PoliceInInvestigationMode && !PedScanning.CopPeds.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Police.InvestigationPosition) <= Police.InvestigationDistance && x.Pedestrian.IsDriver()) && PedScanning.CopPeds.Where(x => x.WasInvestigationSpawn).Count() < 4)
            {
                SpawnInvestigatingCop();
            }
        }
        else
        {
            if (CanSpawnActiveChaseCop && PedScanning.CopPeds.Where(x => x.WasModSpawned).Count() < LosSantosRED.MySettings.Police.SpawnAmbientPoliceLimit + ExtraCopSpawnLimit)
            {
                SpawnActiveChaseCop();
            }
        }
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
    public static void SpawnActiveChaseCop()
    {
        try
        {
            if (NextPoliceSpawn == null)
            {
                float LowerLimit = 340f - (LosSantosRED.PlayerWantedLevel * - 40);//(1) 300f - (5)140f
                float UpperLimit = 550f;
                NextPoliceSpawn = GetPoliceSpawn(LowerLimit, UpperLimit, true);
                return;
            }
            int RandomValue = LosSantosRED.MyRand.Next(1, 11);
            Agency AgencyToSpawn;

            if(NextPoliceSpawn.ZoneAtLocation.ZoneAgencies == null || !NextPoliceSpawn.ZoneAtLocation.ZoneAgencies.Any())
            {
                Debugging.WriteToLog("SpawnActiveChaseCop", string.Format("No Agencies At: {0}", NextPoliceSpawn.ZoneAtLocation.TextName));
                NextPoliceSpawn = null;
                return;
            }

            Debugging.WriteToLog("SpawnActiveChaseCop", string.Format("Possible Agencies: {0}",string.Join(",",NextPoliceSpawn.ZoneAtLocation.ZoneAgencies.Where(x => x.AssociatedAgency != null).Select(x => x.AssociatedAgency.Initials + " " + x.CanCurrentlySpawn + " " + x.CurrentSpawnChance))));


            //bool SpawnMainZoneAgency = LosSantosRED.RandomPercent(100 - ((LosSantosRED.PlayerWantedLevel - 1) * 25));//LosSantosRED.RandomPercent(100 - ((LosSantosRED.PlayerWantedLevel - 1) * 10));//(1) 100% - (5) 60%
            //if (SpawnMainZoneAgency && NextPoliceSpawn.ZoneAtLocation.MainZoneAgency.AvailableForSpawning)
            //{
            //    AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.MainZoneAgency;
            //}
            //else
            //{
                AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.GetRandomAgency();
           // }

            if (AgencyToSpawn != null)
                SpawnCop(AgencyToSpawn, NextPoliceSpawn.SpawnLocation);

            NextPoliceSpawn = null;
            GameTimeLastSpawnedActiveChaseCop = Game.GameTime;

            Debugging.WriteToLog("SpawnActiveChaseCop", "Finished Spawning Cop");
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("SpawnActiveChaseCopError", e.Message + " : " + e.StackTrace);
        }
    }
    public static void SpawnAmbientCop()
    {
        try
        {
            if (NextPoliceSpawn == null)
            {
                NextPoliceSpawn = GetPoliceSpawn(750f, 1500f, false);
                return;
            }
            int RandomValue = LosSantosRED.MyRand.Next(1, 11);
            Agency AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.MainZoneAgency;
            if (NextPoliceSpawn.IsFreeway && Agencies.AgenciesList.Any(x => x.SpawnsOnHighway) && RandomValue <= 4)
                AgencyToSpawn = Agencies.AgenciesList.Where(x => x.SpawnsOnHighway).FirstOrDefault();
            else if (RandomValue <= 9 && NextPoliceSpawn.ZoneAtLocation.HasAgencies)
                AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.MainZoneAgency;
            else if (NextPoliceSpawn.ZoneAtLocation.HasSecondaryAgencies)
                AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.SecondaryAgencies.PickRandom().AssociatedAgency;

            if (AgencyToSpawn != null)
                SpawnCop(AgencyToSpawn, NextPoliceSpawn.SpawnLocation);

            NextPoliceSpawn = null;

            Debugging.WriteToLog("SpawnRandomCop", "Finished Spawning Cop");
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("SpawnRandomCopError",e.Message + " : " + e.StackTrace);
        }

    }
    public static void SpawnInvestigatingCop()
    {
        try
        { 
            if(NextPoliceInvestigationSpawn == null)
            {
                NextPoliceInvestigationSpawn = GetPoliceSpawn(Police.InvestigationDistance/2, Police.InvestigationDistance, true);
                return;
            }
            Agency AgencyToSpawn = NextPoliceInvestigationSpawn.ZoneAtLocation.MainZoneAgency;
            GTACop ClosestCop = SpawnCop(AgencyToSpawn, NextPoliceInvestigationSpawn.SpawnLocation);

            ClosestCop.WasInvestigationSpawn = true;
            Debugging.WriteToLog("SpawnInvestigatingCop", "Attempting to Spawn Cop");
            NextPoliceInvestigationSpawn = null;                    
            if (ClosestCop == null)
                return;
            Tasking.AddItemToQueue(new CopTask(ClosestCop, Tasking.AssignableTasks.TaskInvestigateCrime));
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("SpawnRandomCopError", e.Message + " : " + e.StackTrace);
        }

    }
    public static float GetClosestVehicleNodeHeading(this Vector3 v3)
    {
        float outHeading;
        Vector3 outPosition;

        NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING(v3.X, v3.Y, v3.Z, out outPosition, out outHeading, 12, 0x40400000, 0);

        return outHeading;
    }
    public static RandomPoliceSpawn GetPoliceSpawn(float DistanceFrom, float DistanceTo, bool AllowClosePoliceSpawns)
    {
        Vector3 SpawnLocation = Vector3.Zero;
        float Heading = 0f;
        Vector3 InitialPosition = Vector3.Zero;
        if(LosSantosRED.PlayerIsWanted)
        {
            InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(200f).Around2D(DistanceFrom, DistanceTo);
        }
        else
        {
            InitialPosition = Game.LocalPlayer.Character.Position.Around2D(DistanceFrom, DistanceTo);
        }

        LosSantosRED.GetStreetPositionandHeading(InitialPosition, out SpawnLocation, out Heading);

        if (SpawnLocation == Vector3.Zero)
            return null;

        if (SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) <= 150f)//250f
            return null;

        if (AllowClosePoliceSpawns)
        {
            if (PedScanning.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(SpawnLocation) <= 150f))
                return null;
        }
        else
        {
            if (PedScanning.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(SpawnLocation) <= 500f))//500f
                return null;
        }

        Zone ZoneName = Zones.GetZoneAtLocation(SpawnLocation);
        if (ZoneName == null || ZoneName.GameName == "OCEANA")
            return null;

        string StreetName = PlayerLocation.GetCurrentStreet(SpawnLocation);
        Street MyGTAStreet = Streets.GetStreetFromName(StreetName);

        return new RandomPoliceSpawn(SpawnLocation, ZoneName, MyGTAStreet != null && MyGTAStreet.isFreeway);
    }
    public static void RemoveFarAwayRandomlySpawnedCops()
    {
        float DeleteDistance = 1000f;//2000
        float NonPersistDistance = 800f;//1750f

        //if (LosSantosRED.PlayerIsWanted)
        //    return;//dont do this for now?


        if(LosSantosRED.PlayerIsWanted)
        {
            DeleteDistance = 850f;//1250f;
            NonPersistDistance = 750f;//1000f;//was 550f
        }
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.WasModSpawned && x.HasBeenSpawnedFor >= 20000))
        {
            Vector3 CurrentLocation = Cop.Pedestrian.Position;
            if (Cop.DistanceToPlayer >= DeleteDistance)//2000f
            {
                Police.DeleteCop(Cop);
            }
            else if (Cop.WasMarkedNonPersistent && Cop.DistanceToPlayer >= NonPersistDistance)//1750f
            {
                Police.MarkNonPersistent(Cop);
                break;
            }
            else if (Cop.DistanceToPlayer >= 200f && LosSantosRED.PlayerIsWanted && Cop.Pedestrian.IsDriver() && !Cop.Pedestrian.IsInHelicopter && Cop.EverSeenPlayer)
            {
                Police.DeleteCop(Cop);
                //if (PedScanning.CopPeds.Any(x => x.Pedestrian.Exists() && x.Pedestrian.IsDriver() && x.EverSeenPlayer && Cop.Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 50f))
                //{
                //    Debugging.WriteToLog("Deleting Close Cop", string.Format("Cop: {0}", Cop.Pedestrian.Handle));
                //    Police.DeleteCop(Cop);
                //    break;
                //}
            }

            if (Cop.DistanceToPlayer >= 125f && Cop.Pedestrian.IsInAnyVehicle(false))//250f
            {
                if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 950)
                {
                    Cop.Pedestrian.CurrentVehicle.Repair();
                }
                else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
                {
                    Police.DeleteCop(Cop);
                }
            }
        }
        foreach(Vehicle PoliceCar in CreatedPoliceVehicles.Where(x => x.Exists()))//cleanup abandoned police cars, either cop dies or he gets marked non persisitent
        {
            if(PoliceCar.IsEmpty)
            {
                if (PoliceCar.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                {
                    PoliceCar.Delete();
                }
            }
        }
        CreatedPoliceVehicles.RemoveAll(x => !x.Exists());
    }
    public static GTACop SpawnCop(Agency _Agency, Vector3 SpawnLocation)
    {
        if (SpawnLocation == null)
            return null;
        if (_Agency == null)
            return null;

        if (!_Agency.CanSpawn)
            return null;

        GTAVehicle CopCar;
        bool IsBike = false;
        bool IsHelicopter = false;
        if (_Agency.HasMotorcycles)
            IsBike = LosSantosRED.RandomPercent(50); //50% bike cop for SAHP

        if (_Agency.HasSpawnableHelicopters && CreatedPoliceVehicles.Count(x => x.IsHelicopter) <= 1)
        {
            if (_Agency.Vehicles.Any(x => !x.IsHelicopter && x.CanCurrentlySpawn))
                IsHelicopter = LosSantosRED.RandomPercent(70); //50% bike cop for SAHP
            else
                IsHelicopter = true;
        }

        Debugging.WriteToLog("SpawnCop", string.Format("Agency: {0}, HasHelicopter: {1}, ISHelicopter: {2}", _Agency.Initials,_Agency.HasSpawnableHelicopters,IsHelicopter));

        if (IsBike)
            CopCar = SpawnCopMotorcycle(_Agency, SpawnLocation);
        else if(IsHelicopter)
            CopCar = SpawnCopHelicopter(_Agency, new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 250f));//spawn it in the air
        else
            CopCar = SpawnCopCruiser(_Agency, SpawnLocation);

        if (CopCar == null)
        {

        }
        else
        {
            List<string> RequiredPedModels = new List<string>();
            if (CopCar.ExtendedAgencyVehicleInformation != null && CopCar.ExtendedAgencyVehicleInformation.AllowedPedModels.Any())
            {
                RequiredPedModels = CopCar.ExtendedAgencyVehicleInformation.AllowedPedModels;
            }

            Ped Cop = SpawnCopPed(_Agency, SpawnLocation, IsBike, RequiredPedModels);
            if (Cop == null)
                return null;
            CreatedEntities.Add(Cop);
            CreatedPoliceVehicles.Add(CopCar.VehicleEnt);
            CreatedEntities.Add(CopCar.VehicleEnt);
            Cop.WarpIntoVehicle(CopCar.VehicleEnt, -1);
            Cop.IsPersistent = true;
            CopCar.VehicleEnt.IsPersistent = true;
            Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
            GTACop MyNewCop = new GTACop(Cop, false, Cop.Health, _Agency);
            Police.IssueCopPistol(MyNewCop);
            MyNewCop.WasModSpawned = true;
            MyNewCop.WasMarkedNonPersistent = true;
            MyNewCop.WasRandomSpawnDriver = true;
            MyNewCop.IsBikeCop = IsBike;
            MyNewCop.GameTimeSpawned = Game.GameTime;
            Debugging.WriteToLog("SpawnCop", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.VehicleEnt.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

            if (LosSantosRED.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
            {
                Blip myBlip = Cop.AttachBlip();
                myBlip.Color = _Agency.AgencyColor;
                myBlip.Scale = 0.6f;
                Police.CreatedBlips.Add(myBlip);
            }
            PedScanning.CopPeds.Add(MyNewCop);

            if (CopCar.ExtendedAgencyVehicleInformation != null)
            {
                int OccupantsToAdd = LosSantosRED.MyRand.Next(CopCar.ExtendedAgencyVehicleInformation.MinOccupants, CopCar.ExtendedAgencyVehicleInformation.MaxOccupants + 1) - 1;
                for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                {
                    Ped PartnerCop = SpawnCopPed(_Agency, SpawnLocation, false, null);
                    if (PartnerCop != null)
                    {
                        CreatedEntities.Add(PartnerCop);
                        if (!CopCar.VehicleEnt.Exists())
                        {
                            if (PartnerCop.Exists())
                                PartnerCop.Delete();
                        }
                        else
                        {
                            PartnerCop.WarpIntoVehicle(CopCar.VehicleEnt, OccupantIndex-1);
                            PartnerCop.IsPersistent = true;
                            GTACop MyNewPartnerCop = new GTACop(PartnerCop, false, PartnerCop.Health, _Agency);
                            Police.IssueCopPistol(MyNewPartnerCop);
                            MyNewPartnerCop.WasModSpawned = true;
                            MyNewPartnerCop.WasMarkedNonPersistent = true;
                            PedScanning.CopPeds.Add(MyNewPartnerCop);
                            MyNewPartnerCop.GameTimeSpawned = Game.GameTime;
                            Debugging.WriteToLog("SpawnCop", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.VehicleEnt.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
                        }
                    }
                }
            }
            return MyNewCop;
        }
        return null;
    }
    public static Ped SpawnCopPed(Agency _Agency,Vector3 SpawnLocation, bool IsBike, List<string> RequiredModels)
    {
        if (_Agency == null)
            return null;



        Agency.ModelInformation MyInfo = _Agency.GetRandomPed(RequiredModels);

        if(MyInfo == null)
            return null;

        Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 10f);
        Ped Cop = new Ped(MyInfo.ModelName, SafeSpawnLocation, 0f);
        if (!Cop.Exists())
            return null;

        NativeFunction.CallByName<bool>("SET_PED_AS_COP", Cop, true);
        Cop.RandomizeVariation();
        if (IsBike)
        {
            Cop.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 0, 0, 0);
        }
        else
        {
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 1, 0, 0);
        }

        if (MyInfo.IsMale && LosSantosRED.MyRand.Next(1, 11) <= 4) //40% Chance of Vest
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
        if (!Police.IsNightTime)
            NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses

        return Cop;
    }
    public static GTAVehicle SpawnCopCruiser(Agency _Agency, Vector3 SpawnLocation)
    {
        string ModelName = "police4";
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(false,false);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopCruiser", string.Format("Could not find Auto Info for {0}", _Agency.Initials));
            return null;
        }

        ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        Agencies.ChangeLivery(CopCar, _Agency);
        GameFiber.Yield();

        GTAVehicle ToReturn = new GTAVehicle(CopCar, 0, false, false, null, false, null) { ExtendedAgencyVehicleInformation = MyCarInfo };
        if(CopCar.Exists())
        {
            UpgradeCruiser(CopCar);
            return ToReturn;
        }
        else
        {
            return null;
        }    
    }
    public static GTAVehicle SpawnCopMotorcycle(Agency _Agency, Vector3 SpawnLocation)
    {  
        string ModelName = "policeb";
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(true,false);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopMotorcycle", string.Format("Could not find Bike Info for {0}", _Agency.Initials));
            return null;
        }
        ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        GameFiber.Yield();

        GTAVehicle ToReturn = new GTAVehicle(CopCar, 0, false, false, null, false, null) { ExtendedAgencyVehicleInformation = MyCarInfo };

        if (CopCar.Exists())
        {
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    public static GTAVehicle SpawnCopHelicopter(Agency _Agency, Vector3 SpawnLocation)
    {
        string ModelName = "polmav";
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(false,true);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopHelicopter", string.Format("Could not find Heli Info for {0}", _Agency.Initials));
            return null;
        }
        ModelName = MyCarInfo.ModelName;
        //Vector3 AirSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 250f);
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        Agencies.ChangeLivery(CopCar, _Agency);
        GameFiber.Yield();

        GTAVehicle ToReturn = new GTAVehicle(CopCar, 0, false, false, null, false, null) { ExtendedAgencyVehicleInformation = MyCarInfo };

        if (CopCar.Exists())
        {
            return ToReturn;
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

        if (NativeFunction.CallByName<bool>("DOES_EXTRA_EXIST", CopCruiser, 1) && LosSantosRED.MyRand.Next(1,11) <= 9)//rarely do we want slicktop
        {
            NativeFunction.CallByName<bool>("SET_VEHICLE_EXTRA", CopCruiser, 1, false);//make sure the siren is there
        }

        // NativeFunction.CallByName<bool>("SET_VEHICLE_WINDOW_TINT", CopCruiser, 1);
    }
    public static void CreateK9()
    {
        try
        {
            GTACop ClosestDriver = PedScanning.CopPeds.Where(x => x.Pedestrian.IsInAnyVehicle(false) && !x.IsInHelicopter && x.Pedestrian.CurrentVehicle.Driver == x.Pedestrian && x.Pedestrian.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
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
                PedScanning.K9Peds.Add(DoggoCop);
                //Tasking.TaskK9(DoggoCop);
                Debugging.WriteToLog("CreateK9", String.Format("Created K9 ", Doggo.Handle));
            }
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("CreateK9", e.Message);
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
        Debugging.WriteToLog("PutK9InCar", String.Format("K9 {0}, put in Car", DoggoCop.Pedestrian.Handle));
    }
    public static void MoveK9s()
    {
        foreach (GTACop K9 in PedScanning.K9Peds)
        {
            if (K9.Pedestrian.IsInAnyVehicle(false))
            {
                GTACop ClosestDriver = PedScanning.CopPeds.Where(x => x.Pedestrian.IsInAnyVehicle(false) && !x.IsInHelicopter && x.Pedestrian.CurrentVehicle.Driver == x.Pedestrian && x.Pedestrian.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                if (ClosestDriver != null)
                {
                    PutK9InCar(K9, ClosestDriver);
                }
            }
        }
    }
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

