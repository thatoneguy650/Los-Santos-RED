using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PoliceSpawningManager
{
    private static uint GameTimeLastRemovedCop;
    private static uint GameTimeLastSpawnedCop;
    private static List<Vehicle> CreatedPoliceVehicles;
    private static List<Entity> CreatedEntities;
    private static VehicleInformation CurrentVehicleInfo;
    public static bool IsRunning { get; set; }
    public static bool RecentlySpawnedCop
    {
        get
        {
            if (GameTimeLastSpawnedCop == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSpawnedCop <= 10000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyRemovedCop
    {
        get
        {
            if (GameTimeLastRemovedCop == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRemovedCop <= 10000)
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        CreatedPoliceVehicles = new List<Vehicle>();
        CreatedEntities = new List<Entity>();
        IsRunning = true;
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
    public static void Tick()
    {
        if (IsRunning)
        {
            RepairOrRemoveDamagedVehicles();
            RemoveAbandonedVehicles();

            if(PedSwapManager.RecentlyTakenOver || RespawnManager.RecentlySurrenderedToPolice)
            {
                RemoveDisallowedPeds();
            }
        }
    }
    public static bool SpawnGTACop(Agency _Agency, Vector3 SpawnLocation, float Heading, VehicleInformation MyCarInfo, bool CanSpawnOnFoot)
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
                VehicleManager.PoliceVehicles.Add(CopCar.VehicleEnt);
                List<string> RequiredPedModels = new List<string>();
                if (CurrentVehicleInfo != null && CurrentVehicleInfo.AllowedPedModels.Any())
                {
                    RequiredPedModels = CurrentVehicleInfo.AllowedPedModels;
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
                PoliceEquipmentManager.IssueWeapons(MyNewCop);
                MyNewCop.WasModSpawned = true;
                MyNewCop.WasMarkedNonPersistent = true;
                MyNewCop.WasSpawnedAsDriver = true;

                //MyNewCop.IsBikeCop = MyCarInfo.IsMotorcycle;
                MyNewCop.GameTimeSpawned = Game.GameTime;
                Debugging.WriteToLog("PoliceSpawning", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.VehicleEnt.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

                if (SettingsManager.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
                {
                    Blip myBlip = Cop.AttachBlip();
                    myBlip.Color = _Agency.AgencyColor;
                    myBlip.Scale = 0.6f;
                    Mod.Map.AddBlip(myBlip);
                }
                Mod.PedManager.Cops.Add(MyNewCop);

                if (CurrentVehicleInfo != null)
                {
                    int OccupantsToAdd = RandomItems.MyRand.Next(CurrentVehicleInfo.MinOccupants, CurrentVehicleInfo.MaxOccupants + 1) - 1;
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
                                PoliceEquipmentManager.IssueWeapons(MyNewPartnerCop);
                                MyNewPartnerCop.WasModSpawned = true;
                                MyNewPartnerCop.WasMarkedNonPersistent = true;

                                if (SettingsManager.MySettings.Police.SpawnedAmbientPoliceHaveBlip && PartnerCop.Exists())
                                {
                                    Blip myBlip = PartnerCop.AttachBlip();
                                    myBlip.Color = _Agency.AgencyColor;
                                    myBlip.Scale = 0.6f;
                                    Mod.Map.AddBlip(myBlip);
                                }
                                Mod.PedManager.Cops.Add(MyNewPartnerCop);
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
    public static void UpdateLivery(Vehicle CopCar, Agency AssignedAgency)
    {
        VehicleInformation MyVehicle = null;
        if (AssignedAgency != null && AssignedAgency.Vehicles != null && CopCar.Exists())
        {
            MyVehicle = AssignedAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
        }
        if (MyVehicle == null)
        {
            if (CopCar.Exists())
            {
                Debugging.WriteToLog("ChangeLivery", string.Format("No Match for Vehicle {0} for {1}", CopCar.Model.Name, AssignedAgency.Initials));
                CopCar.Delete();
            }
            return;
        }
        if (MyVehicle.Liveries != null && MyVehicle.Liveries.Any())
        {
            //Debugging.WriteToLog("ChangeLivery", string.Format("Agency {0}, {1}, {2}", AssignedAgency.Initials, CopCar.Model.Name,string.Join(",", MyVehicle.Liveries.Select(x => x.ToString()))));
            int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
            NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        }
        CopCar.LicensePlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    }
    public static void UpdateLivery(Vehicle CopCar)
    {
        Agency AssignedAgency = AgencyManager.GetAgency(CopCar);
        UpdateLivery(CopCar, AssignedAgency);
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
                    Cop PassengerCop = Mod.PedManager.Cops.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
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
        foreach (Cop Cop in Mod.PedManager.Cops.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer >= 100f && x.Pedestrian.IsInAnyVehicle(false)))//was 175f
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
    private static void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
            return;
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }
    private static void RemoveDisallowedPeds()
    {
        foreach(Cop myCop in Mod.PedManager.Cops.Where(x => !x.AssignedAgency.CanSpawn))
        {
            DeleteCop(myCop);
        }
    }
    private static Ped SpawnCopPed(Agency _Agency,Vector3 SpawnLocation, bool IsBike, List<string> RequiredModels)
    {
        if (_Agency == null)
            return null;

        PedestrianInformation MyInfo = _Agency.GetRandomPed(RequiredModels);

        if(MyInfo == null)
            return null;

        Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 1f);//+5f
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

        if (Cop.IsMale && RandomItems.MyRand.Next(1, 11) <= 4) //40% Chance of Vest
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
        if (!Mod.Player.IsNightTime)
            NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses

        if (MyInfo.RequiredVariation != null)
        {
            MyInfo.RequiredVariation.ReplacePedComponentVariation(Cop);
        }


        return Cop;
    }
    private static VehicleExt SpawnCopVehicle(Agency _Agency, VehicleInformation MyCarInfo, Vector3 SpawnLocation,float Heading)
    {
        string ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, Heading);
        UpdateLivery(CopCar, _Agency);
        GameFiber.Yield();
        if (CopCar.Exists())
        {
            VehicleExt ToReturn = new VehicleExt(CopCar, 0, false, false, false, null);
            if (CopCar.Exists())
            {
                UpgradeCruiser(CopCar);
                CurrentVehicleInfo = MyCarInfo;
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
    private class RandomPoliceSpawn
    {
        public Vector3 SpawnLocation;
        public float Heading;
        public Zone ZoneAtLocation;
        public Street StreetAtSpawn;
        public RandomPoliceSpawn(Vector3 _SpawnLocation, float _Heading, Zone _ZoneAtLocation, Street _StreetAtSpawn)
        {
            SpawnLocation = _SpawnLocation;
            ZoneAtLocation = _ZoneAtLocation;
            StreetAtSpawn = _StreetAtSpawn;
            Heading = _Heading;
        }
    }

}


