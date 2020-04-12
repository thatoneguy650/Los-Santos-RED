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
    private static RandomPoliceSpawn NextPoliceSpawn;
    private static RandomPoliceSpawn NextPoliceInvestigationSpawn;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        IsRunning = true;
    }
    public static void RandomCopTick()
    {
        if (PedScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < LosSantosRED.MySettings.Police.SpawnRandomPoliceLimit)
        {
            SpawnRandomCop();
        }
        if(Police.PoliceInInvestigationMode && !PedScanning.CopPeds.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Police.InvestigationPosition) <= Police.InvestigationDistance && x.Pedestrian.IsDriver()) && PedScanning.CopPeds.Where(x => x.WasInvestigationSpawn).Count() < 4)
        {
            SpawnInvestigatingCop();
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
    public static void SpawnRandomCop()
    {
        try
        {
            if (NextPoliceSpawn == null)
            {
                if(LosSantosRED.PlayerIsWanted)
                    NextPoliceSpawn = GetPoliceSpawn(250f,750f,true);
                else
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
                AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.ZoneAgencies.Where(x => !x.IsMain).PickRandom().AssociatedAgency();

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
        LosSantosRED.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position.Around2D(DistanceFrom, DistanceTo), out SpawnLocation, out Heading);

        if (SpawnLocation == Vector3.Zero)
            return null;

        if (SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) <= 150f)//250f
            return null;

        if (!AllowClosePoliceSpawns)
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
        float DeleteDistance = 2000;
        float NonPersistDistance = 1750f;

        //if (LosSantosRED.PlayerIsWanted)
        //    return;//dont do this for now?


        if(LosSantosRED.PlayerIsWanted)
        {
            DeleteDistance = 900f;//1250f;
            NonPersistDistance = 850f;//1000f;//was 550f
        }
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.WasRandomSpawn))
        {
            if (Cop.DistanceToPlayer >= DeleteDistance)//2000f
            {
                Police.DeleteCop(Cop);
            }
            else if (Cop.WasMarkedNonPersistent && Cop.DistanceToPlayer >= NonPersistDistance)//1750f
            {
                Police.MarkNonPersistent(Cop);
                break;
            }
            if (Cop.DistanceToPlayer >= 125f && Cop.Pedestrian.IsInAnyVehicle(false))//250f
            {
                if (Cop.Pedestrian.CurrentVehicle.Health <= 800 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 800)
                {
                    //if (Cop.AssignedAgency == Agencies.LSPD || Cop.AssignedAgency == Agencies.LSSD)//Only repiar naturally spawning cars so they appear to have just been another spawned car
                    //{
                        Cop.Pedestrian.CurrentVehicle.Repair();
                    //}
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
    public static GTACop SpawnCop(Agency _Agency, Vector3 SpawnLocation)
    {
        if (SpawnLocation == null)
            return null;
        if (_Agency == null)
            return null;

        var SpawnCop = SpawnCopPed(_Agency, SpawnLocation);
        Ped Cop = SpawnCop.Item1;
        bool isBike = SpawnCop.Item2;
        //GameFiber.Yield();
        if (Cop == null)
            return null;

        CreatedEntities.Add(Cop);
        Vehicle CopCar;
        if (isBike)
            CopCar = SpawnCopMotorcycle(_Agency, SpawnLocation);
        else
            CopCar = SpawnCopCruiser(_Agency, SpawnLocation);


        if (CopCar == null)
        {
            if (Cop.Exists())
                Cop.Delete();
        }
        else
        {
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

            if (LosSantosRED.MySettings.Police.SpawnedRandomPoliceHaveBlip && Cop.Exists())
            {
                Blip myBlip = Cop.AttachBlip();
                myBlip.Color = _Agency.AgencyColor;
                myBlip.Scale = 0.6f;
                Police.CreatedBlips.Add(myBlip);
            }

            PedScanning.CopPeds.Add(MyNewCop);

            bool AddPartner = LosSantosRED.MyRand.Next(1, 11) <= 5;
            if (AddPartner && !isBike)
            {
                var PartnerSpawn = SpawnCopPed(_Agency, SpawnLocation);
                Ped PartnerCop = PartnerSpawn.Item1;
                if (PartnerCop != null)
                {
                    CreatedEntities.Add(PartnerCop);
                    if (!CopCar.Exists())
                    {
                        if (PartnerCop.Exists())
                            PartnerCop.Delete();
                    }
                    else
                    {
                        PartnerCop.WarpIntoVehicle(CopCar, 0);
                        PartnerCop.IsPersistent = true;
                        GTACop MyNewPartnerCop = new GTACop(PartnerCop, false, PartnerCop.Health, _Agency);
                        Police.IssueCopPistol(MyNewPartnerCop);
                        MyNewPartnerCop.WasRandomSpawn = true;
                        MyNewPartnerCop.WasMarkedNonPersistent = true;
                        PedScanning.CopPeds.Add(MyNewPartnerCop);
                    }
                }
            }
            return MyNewCop;
        }
        return null;
    }
    public static (Ped,bool) SpawnCopPed(Agency _Agency,Vector3 SpawnLocation)
    {
        bool isMale = true;
        bool isBike = false;
        if (_Agency == null)
            return (null,false);
        if (_Agency.CopModels.Any(x => !x.isMale))
            isMale = LosSantosRED.MyRand.Next(1, 11) <= 7; //70% chance Male

        if (_Agency.HasBikeOfficers)
            isBike = LosSantosRED.MyRand.Next(1, 11) <= 5; //50% bike cop for SAHP

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
        //if (_Agency == Agencies.LSSD || _Agency == Agencies.LSPD || _Agency == Agencies.BCSO || _Agency == Agencies.LSIAPD)
        //{
            if (isMale && LosSantosRED.MyRand.Next(1, 11) <= 4) //40% Chance of Vest
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
            if (!Police.IsNightTime)
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses
       // }

        return (Cop,isBike);
    }
    public static Vehicle SpawnCopCruiser(Agency _Agency, Vector3 SpawnLocation)
    {
        string ModelName = "police4";
        Agency.VehicleInformation MyCar = _Agency.GetRandomVehicle(false);
        if (MyCar != null)
            ModelName = MyCar.ModelName;

        Debugging.WriteToLog("SpawnCopCruiser", string.Format("Trying to spawn: {0}, For: {1}",ModelName, _Agency.FullName));
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, 0f);
        Agencies.ChangeLivery(CopCar, _Agency);
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
                Tasking.TaskK9(DoggoCop);
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

