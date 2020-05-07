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
    public static bool IsRunning { get; set; }
    public static float MinDistanceToSpawn
    {
        get
        {
            if (LosSantosRED.PlayerIsWanted)
                return 400f - (LosSantosRED.PlayerWantedLevel * -40);
            else if (Police.PoliceInInvestigationMode)
                return Police.InvestigationDistance / 2;
            else
                return 350f;//450f;//750f
        }
    }
    public static float MaxDistanceToSpawn
    {
        get
        {
            if (LosSantosRED.PlayerIsWanted)
                return 550f;
            else if (Police.PoliceInInvestigationMode)
                return Police.InvestigationDistance;
            else
                return 900f;//1250f//1500f
        }
    }
    public static float DistanceToDelete
    {
        get
        {
            if (LosSantosRED.PlayerIsWanted)
                return 550f;
            else
                return 1500f;
        }
    }
    public static bool AllowClosePoliceSpawns
    {
        get
        {
            if (LosSantosRED.PlayerIsWanted)
                return true;
            else
                return false;
        }
    }
    public static bool CanSpawnCop
    {
        get
        {
            if (LosSantosRED.PlayerIsNotWanted)
            {
                if (PedList.TotalSpawnedCops < LosSantosRED.MySettings.Police.SpawnAmbientPoliceLimit)
                    return true;
                else
                    return false;
            }
            else
            {
                if (PedList.TotalSpawnedCops < LosSantosRED.MySettings.Police.SpawnAmbientPoliceLimit + ExtraCopSpawnLimit)
                    return true;
                else
                    return false;
            }
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
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        IsRunning = true;
    }
    public static void PoliceSpawningTick()
    {
        if (NextPoliceSpawn == null)
        {
            NextPoliceSpawn = GetPoliceSpawn();
            return;
        }
        else if (CanSpawnCop)
        {
            SpawnCop();
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
    public static void DebugSetPoliceSpawn(Vector3 PositionToSet)
    {
        Zone ZoneName = Zones.GetZoneAtLocation(PositionToSet);
        if (ZoneName == null || ZoneName.GameName == "OCEANA")
            return;

        string StreetName = PlayerLocation.GetCurrentStreet(PositionToSet);
        Street MyGTAStreet = Streets.GetStreetFromName(StreetName);

        NextPoliceSpawn = new RandomPoliceSpawn(PositionToSet,0f, ZoneName, MyGTAStreet);
    }
    public static void SpawnCop()
    {
        try
        {
            Agency AgencyToSpawn;
            if(LosSantosRED.PlayerWantedLevel == 5 && LosSantosRED.RandomPercent(30))
            {
                AgencyToSpawn = Agencies.GetRandomArmyAgency();
            }
            else if (NextPoliceSpawn.StreetAtSpawn != null && NextPoliceSpawn.StreetAtSpawn.IsHighway && LosSantosRED.RandomPercent(10))
            {
                AgencyToSpawn = Agencies.GetRandomHighwayAgency();
            }
            else
            {
                AgencyToSpawn = NextPoliceSpawn.ZoneAtLocation.GetRandomAgency();

            }
            if (AgencyToSpawn != null)
                SpawnGTACop(AgencyToSpawn, NextPoliceSpawn.SpawnLocation, NextPoliceSpawn.Heading);


            NextPoliceSpawn = null;
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("SpawnActiveChaseCopError", e.Message + " : " + e.StackTrace);
        }
    }
    public static RandomPoliceSpawn GetPoliceSpawn()
    {
        float DistanceFrom = MinDistanceToSpawn;
        float DistanceTo = MaxDistanceToSpawn;
        Vector3 SpawnLocation = Vector3.Zero;
        float Heading = 0f;
        Vector3 InitialPosition = Vector3.Zero;
        if(LosSantosRED.PlayerIsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(350f).Around2D(DistanceFrom, DistanceTo);//put it out front to aid the cops
        }
        else
        {
            InitialPosition = Game.LocalPlayer.Character.Position.Around2D(DistanceFrom, DistanceTo);
        }

        LosSantosRED.GetStreetPositionandHeading(InitialPosition, out SpawnLocation, out Heading, true);

        if (SpawnLocation == Vector3.Zero)
            return null;

        if (SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) <= 150f)//250f
            return null;

        if (AllowClosePoliceSpawns)
        {
            if (PedList.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(SpawnLocation) <= 150f))
                return null;
        }
        else
        {
            if (PedList.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(SpawnLocation) <= 500f))//500f
                return null;
        }

        Zone ZoneName = Zones.GetZoneAtLocation(SpawnLocation);
        if (ZoneName == null || ZoneName.GameName == "OCEANA")
            return null;

        string StreetName = PlayerLocation.GetCurrentStreet(SpawnLocation);
        Street MyGTAStreet = Streets.GetStreetFromName(StreetName);

        return new RandomPoliceSpawn(SpawnLocation,Heading, ZoneName, MyGTAStreet);
    }
    public static void RemoveCops()
    {
        foreach (GTACop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.CanBeDeleted))
        {
            Vector3 CurrentLocation = Cop.Pedestrian.Position;
            if (Cop.DistanceToPlayer >= DistanceToDelete)//2000f
            {
                DeleteCop(Cop);
            }
            else if (Cop.DistanceToPlayer >= 200f && LosSantosRED.PlayerIsWanted && Cop.Pedestrian.IsDriver() && !Cop.Pedestrian.IsInHelicopter && (Cop.EverSeenPlayer || Cop.ClosestDistanceToPlayer <= 50f))
            {
                DeleteCop(Cop);
            }

            if (Cop.DistanceToPlayer >= 125f && Cop.Pedestrian.IsInAnyVehicle(false))//250f
            {
                if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth < 1000f)
                {
                    Cop.Pedestrian.CurrentVehicle.Repair();
                }
                else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
                {
                    DeleteCop(Cop);
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
    public static void DeleteCop(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        if (Cop.Pedestrian.IsInAnyVehicle(false))
        {
            if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                {
                    RemoveBlip(Passenger);
                    Passenger.Delete();
                }
            }
            if (Cop.Pedestrian.Exists() && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle != null)
                Cop.Pedestrian.CurrentVehicle.Delete();
        }
        RemoveBlip(Cop.Pedestrian);

        if (Cop.Pedestrian.Exists())
        {
            Cop.Pedestrian.Delete();
        }
        Cop.WasMarkedNonPersistent = false;
        //LocalWriteToLog("SpawnCop", string.Format("Cop Deleted: Handled {0}", Cop.CopPed.Handle));
    }
    public static void MarkNonPersistent(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        RemoveBlip(Cop.Pedestrian);
        if (Cop.Pedestrian.IsInAnyVehicle(false))
        {
            if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                {
                    GTACop PassengerCop = PedList.CopPeds.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
                    if (PassengerCop != null)
                    {
                        PassengerCop.Pedestrian.IsPersistent = false;
                        PassengerCop.WasMarkedNonPersistent = false;
                    }
                }
            }
            Cop.Pedestrian.CurrentVehicle.IsPersistent = false;
        }
        Cop.Pedestrian.IsPersistent = false;
        Cop.WasMarkedNonPersistent = false;
        //LocalWriteToLog("SpawnCop", string.Format("CopMarkedNonPersistant: Handled {0}", Cop.CopPed.Handle));
    }
    public static void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
            return;
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }
    public static GTACop SpawnGTACop(Agency _Agency, Vector3 SpawnLocation,float Heading)
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

        if (_Agency.HasSpawnableHelicopters && !CreatedPoliceVehicles.Any(x => x.IsHelicopter))
        {
            if (_Agency.Vehicles.Any(x => !x.IsHelicopter && x.CanCurrentlySpawn))
                IsHelicopter = LosSantosRED.RandomPercent(70); //50% bike cop for SAHP
            else
                IsHelicopter = true;
        }

        //Debugging.WriteToLog("SpawnCop", string.Format("Agency: {0}, HasHelicopter: {1}, ISHelicopter: {2}", _Agency.Initials,_Agency.HasSpawnableHelicopters,IsHelicopter));

        if (IsBike)
            CopCar = SpawnCopMotorcycle(_Agency, SpawnLocation, Heading);
        else if(IsHelicopter)
            CopCar = SpawnCopHelicopter(_Agency, new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 250f));//spawn it in the air
        else
            CopCar = SpawnCopCruiser(_Agency, SpawnLocation, Heading);

        if (CopCar == null)
        {

        }
        else
        {
            PedList.PoliceVehicles.Add(CopCar.VehicleEnt);
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
            MyNewCop.IssuePistol();
            MyNewCop.WasModSpawned = true;
            MyNewCop.WasMarkedNonPersistent = true;
            MyNewCop.WasRandomSpawnDriver = true;
            MyNewCop.IsBikeCop = IsBike;
            MyNewCop.GameTimeSpawned = Game.GameTime;
            //Debugging.WriteToLog("SpawnCop", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.VehicleEnt.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

            if (LosSantosRED.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
            {
                Blip myBlip = Cop.AttachBlip();
                myBlip.Color = _Agency.AgencyColor;
                myBlip.Scale = 0.6f;
                Police.CreatedBlips.Add(myBlip);
            }
            PedList.CopPeds.Add(MyNewCop);

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
                            MyNewPartnerCop.IssuePistol();
                            MyNewPartnerCop.WasModSpawned = true;
                            MyNewPartnerCop.WasMarkedNonPersistent = true;
                            PedList.CopPeds.Add(MyNewPartnerCop);
                            MyNewPartnerCop.GameTimeSpawned = Game.GameTime;
                            //Debugging.WriteToLog("SpawnCop", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.VehicleEnt.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
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

        Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 5f);
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

        if (MyInfo.RequiredVariation != null)
            LosSantosRED.ReplacePedComponentVariation(Cop, MyInfo.RequiredVariation);


        return Cop;
    }
    public static GTAVehicle SpawnCopCruiser(Agency _Agency, Vector3 SpawnLocation,float Heading)
    {
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(false,false);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopCruiser", string.Format("Could not find Auto Info for {0}", _Agency.Initials));
            return null;
        }

        string ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, Heading);
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
    public static GTAVehicle SpawnCopMotorcycle(Agency _Agency, Vector3 SpawnLocation, float Heading)
    {  
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(true,false);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopMotorcycle", string.Format("Could not find Bike Info for {0}", _Agency.Initials));
            return null;
        }
        string ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, Heading);
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
        Agency.VehicleInformation MyCarInfo = _Agency.GetRandomVehicle(false,true);
        if (MyCarInfo == null)
        {
            Debugging.WriteToLog("SpawnCopHelicopter", string.Format("Could not find Heli Info for {0}", _Agency.Initials));
            return null;
        }
        string ModelName = MyCarInfo.ModelName;
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

        //if (NativeFunction.CallByName<bool>("DOES_EXTRA_EXIST", CopCruiser, 1) && LosSantosRED.MyRand.Next(1,11) <= 9)//rarely do we want slicktop
        //{
        //    NativeFunction.CallByName<bool>("SET_VEHICLE_EXTRA", CopCruiser, 1, false);//make sure the siren is there
        //}

        // NativeFunction.CallByName<bool>("SET_VEHICLE_WINDOW_TINT", CopCruiser, 1);
    }

    public static void SpawnRoadblock(Vector3 InitialPosition)
    {
        Vector3 SpawnLocation;
        float Heading;
        LosSantosRED.GetStreetPositionandHeading(InitialPosition, out SpawnLocation, out Heading, false);
        Heading -= 90f;


        Zone MyZone = Zones.GetZoneAtLocation(SpawnLocation);
        Agency AgencyToSpawn = MyZone.GetRandomAgency();
        GTAVehicle Cool = null;
        if (AgencyToSpawn != null)
            Cool = SpawnCopCruiser(AgencyToSpawn, SpawnLocation,Heading);

        uint GameTimeStartedSleeping = Game.GameTime;
        while(Game.GameTime - GameTimeStartedSleeping <= 5000)
        {

            
            GameFiber.Yield();
        }


        if (Cool != null && Cool.VehicleEnt.Exists())
        {
            Cool.VehicleEnt.Delete();
        }

    }


    public static void DebugRoadblock(Vector3 PositionNear)
    {
        List<RandomPoliceSpawn> Nodes = new List<RandomPoliceSpawn>();
        Vector3 pos = PositionNear;
        Vector3 outPos;
        float heading;
        float val;
        for (int i = 1; i < 40; i++)
        {
            unsafe
            {
                NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
            }
            bool LocalIsObscured = false;
            //if (NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
            //{
            //    LocalIsObscured = true;
                
            //}
            if (NativeFunction.CallByName<bool>("IS_POSITION_OCCUPIED", outPos.X, outPos.Y, outPos.Z, 4f, 0, 1, 0, 0, 0, 0, 0))
            {
                LocalIsObscured = true;

            }

            


            Nodes.Add(new RandomPoliceSpawn(outPos, heading, null, null) { IsObscured = LocalIsObscured });
        }

        uint GameTimeStartedSleeping = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSleeping <= 10000)
        {
            foreach(RandomPoliceSpawn MyNode in Nodes)
            {
                System.Drawing.Color ColorToPick = System.Drawing.Color.Yellow;
                if(MyNode.IsObscured)
                    ColorToPick = System.Drawing.Color.Red;
                Rage.Debug.DrawArrowDebug(new Vector3(MyNode.SpawnLocation.X, MyNode.SpawnLocation.Y, MyNode.SpawnLocation.Z), new Vector3(0.5f), Rotator.Zero, 1f, ColorToPick);
            }
            
            GameFiber.Yield();
        }



    }
}

public class RandomPoliceSpawn
{
    public Vector3 SpawnLocation;
    public float Heading;
    public Zone ZoneAtLocation;
    public Street StreetAtSpawn;
    public bool IsObscured;
    public RandomPoliceSpawn(Vector3 _SpawnLocation,float Heading, Zone _ZoneAtLocation, Street _StreetAtSpawn)
    {
        SpawnLocation = _SpawnLocation;
        ZoneAtLocation = _ZoneAtLocation;
        StreetAtSpawn = _StreetAtSpawn;
    }
}

