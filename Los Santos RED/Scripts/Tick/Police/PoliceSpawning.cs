using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PoliceSpawning
{
    private static uint GameTimeLastRemovedCop;
    private static uint GameTimeLastSpawnedCop;
    private static List<Vehicle> CreatedPoliceVehicles;
    private static List<Entity> CreatedEntities;
    //private static RandomPoliceSpawn StreetSpawn;
    //private static RandomPoliceSpawn WaterSpawn;
    //private static RandomPoliceSpawn AirSpawn;
    //private static uint GameTimeBetweenSpawning = 6000;
    private enum DispatchType
    {
        PoliceAutomobile = 1,
        PoliceHelicopter = 2,
        FireDepartment = 3,
        SwatAutomobile = 4,
        AmbulanceDepartment = 5,
        PoliceRiders = 6,
        PoliceVehicleRequest = 7,
        PoliceRoadBlock = 8,
        PoliceAutomobileWaitPulledOver = 9,
        PoliceAutomobileWaitCruising = 10,
        Gangs = 11,
        SwatHelicopter = 12,
        PoliceBoat = 13,
        ArmyVehicle = 14,
        BikerBackup = 15
    };
    public static bool IsRunning { get; set; }
    //public static float MinDistanceToSpawn
    //{
    //    get
    //    {
    //        if (PlayerState.IsWanted)
    //            return 400f - (PlayerState.WantedLevel * -40);
    //        else if (Investigation.InInvestigationMode)
    //            return Investigation.InvestigationDistance / 2;
    //        else
    //            return 350f;//450f;//750f
    //    }
    //}
    //public static float MaxDistanceToSpawn
    //{
    //    get
    //    {
    //        if (PlayerState.IsWanted)
    //            return 550f;
    //        else if (Investigation.InInvestigationMode)
    //            return Investigation.InvestigationDistance;
    //        else
    //            return 900f;//1250f//1500f
    //    }
    //}
    //public static float DistanceToDelete
    //{
    //    get
    //    {
    //        if (PlayerState.IsWanted)
    //            return 550f;//700f//550f
    //        else
    //            return 1500f;
    //    }
    //}
    //public static bool AllowClosePoliceSpawns
    //{
    //    get
    //    {
    //        if (PlayerState.IsWanted)
    //            return true;
    //        else
    //            return false;
    //    }
    //}
    //public static bool CanSpawnCop
    //{
    //    get
    //    {
    //        if (PlayerState.IsNotWanted)
    //        {
    //            if (PedList.TotalSpawnedCops < General.MySettings.Police.SpawnAmbientPoliceLimit)
    //            {
    //                if (Game.GameTime - GameTimeLastSpawnedCop >= GameTimeBetweenSpawning)
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //                return false;
    //        }
    //        else
    //        {
    //            if (PedList.TotalSpawnedCops < General.MySettings.Police.SpawnAmbientPoliceLimit + ExtraCopSpawnLimit)
    //            {
    //                if (Game.GameTime - GameTimeLastSpawnedCop >= GameTimeBetweenSpawning - (PlayerState.WantedLevel * -1000))
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //                return false;
    //        }
    //    }
    //}
    //public static bool CanRemoveCop
    //{
    //    get
    //    {
    //        if (PlayerState.IsNotWanted)
    //        {
    //            if (PedList.TotalSpawnedCops >= General.MySettings.Police.SpawnAmbientPoliceLimit)
    //            {
    //                if (Game.GameTime - GameTimeLastRemovedCop >= GameTimeBetweenSpawning)
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //                return false;
    //        }
    //        else
    //        {
    //            if (PedList.TotalSpawnedCops >= General.MySettings.Police.SpawnAmbientPoliceLimit + ExtraCopSpawnLimit)
    //            {
    //                if (Game.GameTime - GameTimeLastRemovedCop >= GameTimeBetweenSpawning - (PlayerState.WantedLevel * -1000))
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //                return false;
    //        }
    //    }
    //}
    //public static int ExtraCopSpawnLimit
    //{
    //    get
    //    {
    //        int CurrentWantedLevel = PlayerState.WantedLevel;
    //        if (CurrentWantedLevel == 1)//set as parameters
    //            return 0;
    //        else if (CurrentWantedLevel == 2)
    //            return 2;
    //        else if (CurrentWantedLevel == 3)
    //            return 6;
    //        else if (CurrentWantedLevel == 4)
    //            return 10;
    //        else if (CurrentWantedLevel == 5)
    //            return 12;
    //        else
    //            return 0;
    //    }
    //}
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        IsRunning = true;
    }
    public static void StopVanillaDispatch()
    {
        if (IsRunning)
        {
            SetDispatchService(false);
        }
    }
    public static void CheckRemove()
    {
        if (IsRunning)
        {
            RepairOrRemoveDamagedVehicles();
            RemoveAbandonedVehicles();


            if(PedSwap.RecentlyTakenOver || Respawn.RecentlySurrendered)
            {
                RemoveDisallowedPeds();
            }
        }
    }
    public static void UpgradeCruiser(Vehicle CopCruiser)
    {
        if (!CopCruiser.Exists())
            return;
        if (!CopCruiser.IsHelicopter)
        {
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
    }

   public static void SpawnTempHeliCopter()
    {
        Agency NOOSE = Agencies.AgenciesList.Where(x => x.Initials == "NOOSE").FirstOrDefault();
        Agency.VehicleInformation Heli = NOOSE.Vehicles.Where(x => x.ModelName == "annihilator").FirstOrDefault();

        SpawnGTACop(NOOSE, Game.LocalPlayer.Character.GetOffsetPositionFront(10f) + new Vector3(0f, 0f, 100f), 0f, Heli,false);



        foreach(Cop MyCop in PedList.CopPeds.Where(x => x.IsInHelicopter))
        {
            if(!MyCop.Pedestrian.IsDriver())
            {
                NativeFunction.CallByName<bool>("CONTROL_MOUNTED_WEAPON", MyCop.Pedestrian);
            }
        }
        
        

    }
    private static void RemoveAbandonedVehicles()
    {
        foreach (Vehicle PoliceCar in CreatedPoliceVehicles.Where(x => x.Exists()))//cleanup abandoned police cars, either cop dies or he gets marked non persisitent
        {
            if (PoliceCar.IsEmpty)
            {
                if (PoliceCar.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                {
                    PoliceCar.Delete();
                }
            }
        }
        CreatedPoliceVehicles.RemoveAll(x => !x.Exists());
    }
    private static void RepairOrRemoveDamagedVehicles()
    {
        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer >= 175f && x.Pedestrian.IsInAnyVehicle(false)))
        {
            if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth < 1000f)
            {
                Cop.Pedestrian.CurrentVehicle.Repair();
            }
            else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
            {
                DeleteCop(Cop);
                Debugging.WriteToLog("PoliceSpawning", string.Format("Cop GaveUp Delete: {0}", Cop.Pedestrian.Handle));
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

        SetDispatchService(true);
    }
    public static void DeleteCop(Cop Cop)
    {
        if (Cop == null)
            return;
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
            Debugging.WriteToLog("PoliceSpawning", string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle,Cop.DistanceToPlayer,Cop.AssignedAgency.Initials));
            Cop.Pedestrian.Delete();
        }
        Cop.WasMarkedNonPersistent = false;
        GameTimeLastRemovedCop = Game.GameTime;
    }
    public static void MarkNonPersistent(Cop Cop)
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
                    Cop PassengerCop = PedList.CopPeds.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
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
    public static void RemoveDisallowedPeds()
    {
        foreach(Cop myCop in PedList.CopPeds.Where(x => !x.AssignedAgency.CanSpawn))
        {
            DeleteCop(myCop);
        }
    }
    public static bool SpawnGTACop(Agency _Agency, Vector3 SpawnLocation, float Heading,Agency.VehicleInformation MyCarInfo,bool CanSpawnOnFoot)
    {
        if (_Agency == null)
            return false;

        if (!_Agency.CanSpawn)
            return false;

        if (CanSpawnOnFoot)
        {
            Ped Cop = SpawnCopPed(_Agency, SpawnLocation, false, null);
            if (Cop == null || !Cop.Exists())
                return false;
            else
                return true;
        }
        else
        {
            VehicleExt CopCar;
            CopCar = SpawnCopVehicle(_Agency, MyCarInfo, SpawnLocation, Heading);
            GameFiber.Yield();

            if (CopCar != null && CopCar.VehicleEnt.Exists())
            {
                PedList.PoliceVehicles.Add(CopCar.VehicleEnt);
                List<string> RequiredPedModels = new List<string>();
                if (CopCar.ExtendedAgencyVehicleInformation != null && CopCar.ExtendedAgencyVehicleInformation.AllowedPedModels.Any())
                {
                    RequiredPedModels = CopCar.ExtendedAgencyVehicleInformation.AllowedPedModels;
                }

                Ped Cop = SpawnCopPed(_Agency, SpawnLocation, MyCarInfo.IsMotorcycle, RequiredPedModels);
                GameFiber.Yield();
                if (Cop == null || !Cop.Exists())
                    return false;
                CreatedEntities.Add(Cop);
                CreatedPoliceVehicles.Add(CopCar.VehicleEnt);
                CreatedEntities.Add(CopCar.VehicleEnt);
                Cop.WarpIntoVehicle(CopCar.VehicleEnt, -1);
                Cop.IsPersistent = true;
                CopCar.VehicleEnt.IsPersistent = true;
                Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                Cop MyNewCop = new Cop(Cop, Cop.Health, _Agency);
                MyNewCop.IssuePistol();
                MyNewCop.WasModSpawned = true;
                MyNewCop.WasMarkedNonPersistent = true;
                MyNewCop.WasRandomSpawnDriver = true;
                MyNewCop.IsBikeCop = MyCarInfo.IsMotorcycle;
                MyNewCop.GameTimeSpawned = Game.GameTime;
                Debugging.WriteToLog("PoliceSpawning", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.VehicleEnt.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

                if (General.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
                {
                    Blip myBlip = Cop.AttachBlip();
                    myBlip.Color = _Agency.AgencyColor;
                    myBlip.Scale = 0.6f;
                    General.CreatedBlips.Add(myBlip);
                }
                PedList.CopPeds.Add(MyNewCop);

                if (CopCar.ExtendedAgencyVehicleInformation != null)
                {
                    int OccupantsToAdd = General.MyRand.Next(CopCar.ExtendedAgencyVehicleInformation.MinOccupants, CopCar.ExtendedAgencyVehicleInformation.MaxOccupants + 1) - 1;
                    for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                    {
                        Ped PartnerCop = SpawnCopPed(_Agency, SpawnLocation, false, null);
                        GameFiber.Yield();
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
                                PartnerCop.WarpIntoVehicle(CopCar.VehicleEnt, OccupantIndex - 1);
                                PartnerCop.IsPersistent = true;
                                Cop MyNewPartnerCop = new Cop(PartnerCop, PartnerCop.Health, _Agency);
                                MyNewPartnerCop.IssuePistol();
                                MyNewPartnerCop.WasModSpawned = true;
                                MyNewPartnerCop.WasMarkedNonPersistent = true;
                                if (General.MySettings.Police.SpawnedAmbientPoliceHaveBlip && PartnerCop.Exists())
                                {
                                    Blip myBlip = PartnerCop.AttachBlip();
                                    myBlip.Color = _Agency.AgencyColor;
                                    myBlip.Scale = 0.6f;
                                    General.CreatedBlips.Add(myBlip);
                                }
                                PedList.CopPeds.Add(MyNewPartnerCop);
                                MyNewPartnerCop.GameTimeSpawned = Game.GameTime;
                                Debugging.WriteToLog("PoliceSpawning", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.VehicleEnt.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
                            }
                        }
                    }
                }
                GameTimeLastSpawnedCop = Game.GameTime;
                return true;
            }
            return false;
        }
    }
    private static Ped SpawnCopPed(Agency _Agency,Vector3 SpawnLocation, bool IsBike, List<string> RequiredModels)
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

        if (Cop.IsMale && General.MyRand.Next(1, 11) <= 4) //40% Chance of Vest
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
        if (!Police.IsNightTime)
            NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses

        if (MyInfo.RequiredVariation != null)
            General.ReplacePedComponentVariation(Cop, MyInfo.RequiredVariation);


        return Cop;
    }
    private static VehicleExt SpawnCopVehicle(Agency _Agency, Agency.VehicleInformation MyCarInfo, Vector3 SpawnLocation,float Heading)
    {
        string ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, Heading);
        Agencies.ChangeLivery(CopCar, _Agency);
        GameFiber.Yield();
        if (CopCar.Exists())
        {
            VehicleExt ToReturn = new VehicleExt(CopCar, 0, false, false, null, false, null) { ExtendedAgencyVehicleInformation = MyCarInfo };
            if (CopCar.Exists())
            {
                UpgradeCruiser(CopCar);
                return ToReturn;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    private static void SetDispatchService(bool ValueToSet)
    {
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceVehicleRequest, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRiders, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRoadBlock, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitCruising, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitPulledOver, ValueToSet);
    }
    //private static void RemoveCop()
    //{
    //    //Cop ToDelete = PedList.CopPeds.Where(x => x.DistanceToPlayer >= 200f && x.CanBeDeleted && x.CountNearbyCops >= 3).OrderByDescending(y => y.DistanceToPlayer).FirstOrDefault();

    //    float DistanceToDelete = 1500f;

    //    if (PlayerState.IsWanted)
    //        DistanceToDelete = 550f;


    //    DeleteCop(PedList.CopPeds.Where(x => x.DistanceToPlayer >= DistanceToDelete && x.CanBeDeleted).OrderByDescending(y => y.DistanceToPlayer).FirstOrDefault());

    //    if(PlayerState.IsWanted)// && Game.GameTime - GameTimeLastRemovedCop >= (PlayerState.WantedLevel * -2000) + 15000)
    //    {
    //        //bool First = false;
    //        foreach (Cop MyCop in PedList.CopPeds.Where(x => x.DistanceToPlayer >= 250f && x.CanBeDeleted).OrderByDescending(y => y.DistanceToPlayer))
    //        {
    //            if (MyCop.DistanceToPlayer >= 800f)
    //                DeleteCop(MyCop);
    //            else if (MyCop.CountNearbyCops >= 3)
    //                DeleteCop(MyCop);
    //        }
    //        //DeleteCop(PedList.CopPeds.Where(x => x.DistanceToPlayer >= 175f && x.CanBeDeleted).OrderByDescending(y => y.DistanceToPlayer).FirstOrDefault());
    //    }
    //}
    //public static void CheckRemove()
    //{
    //    if (IsRunning)
    //    {
    //        if (CanRemoveCop)
    //        {

    //        }



    //        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.CanBeDeleted))
    //        {
    //            Vector3 CurrentLocation = Cop.Pedestrian.Position;
    //            if (Cop.DistanceToPlayer >= DistanceToDelete)//2000f
    //            {
    //                DeleteCop(Cop);
    //                Debugging.WriteToLog("SpawnCop", string.Format("Cop Distance Delete: {0}", Cop.Pedestrian.Handle));
    //            }
    //            //else if (PlayerState.IsWanted && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsDriver() && !Cop.Pedestrian.IsInHelicopter)
    //            //{
    //            //    if (Cop.DistanceToPlayer >= 175f && Cop.TimeBehindPlayer >= 20000)
    //            //    {
    //            //        DeleteCop(Cop);
    //            //        Debugging.WriteToLog("SpawnCop", string.Format("Cop Behind Delete: {0}", Cop.Pedestrian.Handle));
    //            //    }
    //            //    else if (Cop.DistanceToPlayer >= 175f && (Cop.EverSeenPlayer || Cop.ClosestDistanceToPlayer <= 50f) && Cop.CountNearbyCops >= 3)
    //            //    {
    //            //        DeleteCop(Cop);
    //            //        Debugging.WriteToLog("SpawnCop", string.Format("Cop Nearby 1 Delete: {0}", Cop.Pedestrian.Handle));
    //            //    }
    //            //    else if (Cop.DistanceToPlayer >= 250f && Cop.CountNearbyCops >= 2)
    //            //    {
    //            //        DeleteCop(Cop);
    //            //        Debugging.WriteToLog("SpawnCop", string.Format("Cop Nearby 2 Delete: {0}", Cop.Pedestrian.Handle));
    //            //    }
    //            //}

    //            if (Cop.DistanceToPlayer >= 175f && Cop.Pedestrian.IsInAnyVehicle(false))//250f
    //            {
    //                if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth < 1000f)
    //                {
    //                    Cop.Pedestrian.CurrentVehicle.Repair();
    //                }
    //                else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
    //                {
    //                    DeleteCop(Cop);
    //                    Debugging.WriteToLog("SpawnCop", string.Format("Cop GaveUp Delete: {0}", Cop.Pedestrian.Handle));
    //                }
    //            }
    //        }
    //        foreach (Vehicle PoliceCar in CreatedPoliceVehicles.Where(x => x.Exists()))//cleanup abandoned police cars, either cop dies or he gets marked non persisitent
    //        {
    //            if (PoliceCar.IsEmpty)
    //            {
    //                if (PoliceCar.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
    //                {
    //                    PoliceCar.Delete();
    //                }
    //            }
    //        }
    //        CreatedPoliceVehicles.RemoveAll(x => !x.Exists());
    //    }
    //}
    //private static void GetPoliceSpawn()
    //{
    //    StreetSpawn = null;
    //    WaterSpawn = null;
    //    AirSpawn = null;

    //    float DistanceFrom = MinDistanceToSpawn;
    //    float DistanceTo = MaxDistanceToSpawn;
    //    Vector3 SpawnLocation = Vector3.Zero;
    //    float Heading = 0f;
    //    Vector3 InitialPosition = Vector3.Zero;
    //    if (PlayerState.IsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
    //    {
    //        InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(350f).Around2D(DistanceFrom, DistanceTo);//put it out front to aid the cops
    //    }
    //    else
    //    {
    //        InitialPosition = Game.LocalPlayer.Character.Position.Around2D(DistanceFrom, DistanceTo);
    //    }
    //    Zone InitialZoneName = Zones.GetZoneAtLocation(InitialPosition);

    //    //Water
    //    if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out float height))
    //    {
    //        WaterSpawn = new RandomPoliceSpawn(new Vector3(InitialPosition.X, InitialPosition.Y, InitialPosition.Z + height), Heading, InitialZoneName, null);
    //        Debugging.WriteToLog("GetPoliceSpawn", string.Format("Water Spawn at {0}", InitialPosition));
    //    }
    //    else
    //    {
    //        WaterSpawn = null;
    //    }

    //    //Air
    //    AirSpawn = new RandomPoliceSpawn(new Vector3(InitialPosition.X, InitialPosition.Y, InitialPosition.Z + 250f), Heading, InitialZoneName, null);


    //    //Street
    //    General.GetStreetPositionandHeading(InitialPosition, out SpawnLocation, out Heading, true);
    //    if (SpawnLocation == Vector3.Zero || SpawnLocation.DistanceTo2D(Game.LocalPlayer.Character) < 150f)//was >?
    //    {
    //        StreetSpawn = null;
    //    }
    //    else
    //    {
    //        if (AllowClosePoliceSpawns)
    //        {
    //            if (PedList.CopPeds.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(SpawnLocation) <= 200f))//150f
    //                StreetSpawn = null;
    //        }
    //        else
    //        {
    //            if (PedList.CopPeds.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(SpawnLocation) <= 500f))//500f
    //                StreetSpawn = null;
    //        }
    //    }

    //    Zone ZoneName = Zones.GetZoneAtLocation(SpawnLocation);
    //    if (ZoneName == null)
    //        StreetSpawn = null;

    //   // string StreetName = Streets.GetCurrentStreetName(SpawnLocation);
    //    Street MyGTAStreet = Streets.GetCurrentStreet(SpawnLocation);

    //    StreetSpawn = new RandomPoliceSpawn(SpawnLocation, Heading, ZoneName, MyGTAStreet);
    //}


    //private static Agency GetAgencyToSpawn()
    //{








    //    Agency ToSpawn = null;
    //    string DebugName = "";
    //    if (PlayerState.WantedLevel == 5)
    //    {
    //        DebugName = "ARMY";
    //        ToSpawn = Agencies.RandomArmyAgency;
    //    }
    //    else if (WaterSpawn != null && General.RandomPercent(10) && PedList.PoliceVehicles.Count(x => x.IsBoat) < General.MySettings.Police.BoatLimit)
    //    {
    //        DebugName = "WATER";
    //        ToSpawn = Jurisdiction.RandomAgencyAtZone(WaterSpawn.ZoneAtLocation.InternalGameName);
    //    }
    //    else if (AirSpawn != null && General.RandomPercent(10) && PedList.PoliceVehicles.Count(x => x.IsHelicopter) < General.MySettings.Police.HelicopterLimit)
    //    {
    //        DebugName = "HELI";
    //        ToSpawn = Jurisdiction.AirAgencyAtZone(AirSpawn.ZoneAtLocation.InternalGameName);
    //    }
    //    else if (StreetSpawn != null)
    //    {
    //        if ((PlayerState.IsNotWanted && General.RandomPercent(3)) || (PlayerState.WantedLevel >= 3 || General.RandomPercent(2 * PlayerState.WantedLevel)))
    //        {
    //            DebugName = "FEDERAL";
    //            ToSpawn = Agencies.RandomFederalAgency;
    //        }
    //        else
    //        {
    //            if (StreetSpawn.StreetAtSpawn != null && StreetSpawn.StreetAtSpawn.IsHighway && General.RandomPercent(10))
    //            {
    //                DebugName = "HIGHWAY";
    //                ToSpawn = Agencies.RandomHighwayAgency;
    //            }
    //            else
    //            {
    //                DebugName = "REGULAR";
    //                ToSpawn = Jurisdiction.RandomAgencyAtZone(StreetSpawn.ZoneAtLocation.InternalGameName);
    //            }
    //        }
    //    }
    //    //Debugging.WriteToLog("SpawnCop", string.Format("Attempting to spawn {0}, {1}", ToSpawn.Initials, DebugName));
    //    return ToSpawn;
    //}


    //public static void SpawnCop()
    //{
    //    try
    //    {
    //        GetPoliceSpawn();
    //        Agency AgencyToSpawn = GetAgencyToSpawn();
    //        SpawnGTACop(AgencyToSpawn, StreetSpawn.Heading);
    //    }
    //    catch (Exception e)
    //    {
    //        Debugging.WriteToLog("SpawnActiveChaseCopError", e.Message + " : " + e.StackTrace);
    //    }
    //}





    //public static void SpawnRoadblock(Vector3 InitialPosition)
    //{
    //    Vector3 SpawnLocation;
    //    float Heading;
    //    General.GetStreetPositionandHeading(InitialPosition, out SpawnLocation, out Heading, false);
    //    Heading -= 90f;


    //    Zone MyZone = Zones.GetZoneAtLocation(SpawnLocation);
    //    Agency AgencyToSpawn = MyZone.GetRandomAgency();
    //    GTAVehicle Cool = null;
    //    if (AgencyToSpawn != null)
    //        Cool = SpawnCopVehicle(AgencyToSpawn, AgencyToSpawn.GetRandomVehicle(false, false), SpawnLocation,Heading);

    //    uint GameTimeStartedSleeping = Game.GameTime;
    //    while(Game.GameTime - GameTimeStartedSleeping <= 5000)
    //    {


    //        GameFiber.Yield();
    //    }


    //    if (Cool != null && Cool.VehicleEnt.Exists())
    //    {
    //        Cool.VehicleEnt.Delete();
    //    }

    //}


    //public static void DebugRoadblock(Vector3 PositionNear)
    //{
    //    List<RandomPoliceSpawn> Nodes = new List<RandomPoliceSpawn>();
    //    Vector3 pos = PositionNear;
    //    Vector3 outPos;
    //    float heading;
    //    float val;
    //    for (int i = 1; i < 40; i++)
    //    {
    //        unsafe
    //        {
    //            NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
    //        }
    //        bool LocalIsObscured = false;
    //        //if (NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
    //        //{
    //        //    LocalIsObscured = true;

    //        //}
    //        if (NativeFunction.CallByName<bool>("IS_POSITION_OCCUPIED", outPos.X, outPos.Y, outPos.Z, 4f, 0, 1, 0, 0, 0, 0, 0))
    //        {
    //            LocalIsObscured = true;

    //        }




    //        Nodes.Add(new RandomPoliceSpawn(outPos, heading, null, null) { IsObscured = LocalIsObscured });
    //    }

    //    uint GameTimeStartedSleeping = Game.GameTime;
    //    while (Game.GameTime - GameTimeStartedSleeping <= 10000)
    //    {
    //        foreach(RandomPoliceSpawn MyNode in Nodes)
    //        {
    //            System.Drawing.Color ColorToPick = System.Drawing.Color.Yellow;
    //            if(MyNode.IsObscured)
    //                ColorToPick = System.Drawing.Color.Red;
    //            Rage.Debug.DrawArrowDebug(new Vector3(MyNode.SpawnLocation.X, MyNode.SpawnLocation.Y, MyNode.SpawnLocation.Z), new Vector3(0.5f), Rotator.Zero, 1f, ColorToPick);
    //        }

    //        GameFiber.Yield();
    //    }



    //}
}

public class RandomPoliceSpawn
{
    public Vector3 SpawnLocation;
    public float Heading;
    public Zone ZoneAtLocation;
    public Street StreetAtSpawn;
    public bool IsObscured;
    public RandomPoliceSpawn(Vector3 _SpawnLocation,float _Heading, Zone _ZoneAtLocation, Street _StreetAtSpawn)
    {
        SpawnLocation = _SpawnLocation;
        ZoneAtLocation = _ZoneAtLocation;
        StreetAtSpawn = _StreetAtSpawn;
        Heading = _Heading;
    }
}

