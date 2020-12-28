using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class Spawner : ISpawner
{
    private IWorldLogger World;
    public Spawner(IWorldLogger world)
    {
        World = world;
    }
    public void Delete(Cop Cop)
    {
        if (Cop != null && Cop.Pedestrian.Exists())
        {
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
                {
                    Cop.Pedestrian.CurrentVehicle.Delete();
                }
            }
            RemoveBlip(Cop.Pedestrian);
            if (Cop.Pedestrian.Exists())
            {
                Game.Console.Print(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                Cop.Pedestrian.Delete();
            }
        }
    }
    public void Spawn(PoliceSpawn policeSpawn)
    {
        try
        {
            SpawnTask spawnTask = new SpawnTask(policeSpawn.Agency, policeSpawn.InitialPosition, policeSpawn.StreetPosition, policeSpawn.Heading, policeSpawn.WantedLevel, policeSpawn.CanSpawnHelicopter, policeSpawn.CanSpawnBoat, policeSpawn.AddBlip);
            spawnTask.AttemptSpawn();
            spawnTask.CreatedCops.ForEach(x => World.AddEntity(x));
            spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
        }
        catch (Exception ex)
        {
            Game.Console.Print("SpawnCop " + ex.Message + " : " + ex.StackTrace);
        }
    }
    private void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
        {
            return;
        }
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private class SpawnTask
    {
        private DispatchableOfficer OfficerType;
        private Agency Agency;
        private int WantedLevel;
        private bool CanSpawnHeli;
        private bool CanSpawnBoat;
        private bool AddBlip;
        private Vector3 InitialPosition;
        private Vector3 StreetPosition;
        private float Heading;
        private VehicleExt Vehicle;
        private DispatchableVehicle VehicleType;
        public SpawnTask(Agency agency, Vector3 initialPosition, Vector3 streetPosition, float heading, int wantedLevel, bool canSpawnHeli, bool canSpawnBoat, bool addBlip)
        {
            Agency = agency;
            WantedLevel = wantedLevel;
            CanSpawnHeli = canSpawnHeli;
            CanSpawnBoat = canSpawnBoat;
            AddBlip = addBlip;
            InitialPosition = initialPosition;
            StreetPosition = streetPosition;
            Heading = heading;
        }
        public List<Cop> CreatedCops { get; private set; } = new List<Cop>();
        public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
        private Vector3 Position
        {
            get
            {
                if (VehicleType.IsHelicopter)
                {
                    return InitialPosition + new Vector3(0f, 0f, 250f);
                }
                else if (VehicleType.IsBoat)
                {
                    return InitialPosition;
                }
                else
                {
                    return StreetPosition;
                }
            }
        }
        public void AttemptSpawn()
        {
            if (Agency != null)
            {
                VehicleType = Agency.GetRandomVehicle(WantedLevel, CanSpawnHeli, CanSpawnBoat);
                OfficerType = Agency.GetRandomPed(WantedLevel, VehicleType.RequiredPassengerModels);
                Vehicle = CreateVehicle();
                if (Vehicle != null && Vehicle.Vehicle.Exists())
                {
                    Cop Cop = CreateCop();
                    if (Cop != null && Cop.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                    {
                        Cop.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, -1);
                        int OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
                        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                        {
                            Cop PassengerCop = CreateCop();
                            if (PassengerCop != null && PassengerCop.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                            {
                                PassengerCop.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, OccupantIndex - 1);
                            }
                        }
                    }
                }
            }
        }
        private Cop CreateCop()
        {
            Ped CopPed = new Ped(OfficerType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), Heading);
            GameFiber.Yield();
            if (CopPed.Exists())
            {
                NativeFunction.CallByName<bool>("SET_PED_AS_COP", CopPed, true);
                CopPed.RandomizeVariation();
                if (VehicleType.IsMotorcycle)
                {
                    CopPed.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
                    NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", CopPed, 4, 0, 0, 0);
                }
                else
                {
                    NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", CopPed, 4, 1, 0, 0);
                }
                if (OfficerType.RequiredVariation != null)
                {
                    OfficerType.RequiredVariation.ReplacePedComponentVariation(CopPed);
                }
                GameFiber.Yield();
                CopPed.IsPersistent = true;
                Cop PrimaryCop = new Cop(CopPed, CopPed.Health, Agency, true);
                PrimaryCop.IssueWeapons();
                CopPed.IsPersistent = true;
                if (AddBlip && CopPed.Exists())
                {
                    Blip myBlip = CopPed.AttachBlip();
                    myBlip.Color = Agency.AgencyColor;
                    myBlip.Scale = 0.6f;
                }
                CreatedCops.Add(PrimaryCop);
                return PrimaryCop;
            }
            return null;
        }
        private VehicleExt CreateVehicle()
        {
            Vehicle copcar = new Vehicle(VehicleType.ModelName, Position, Heading);
            GameFiber.Yield();
            if (copcar.Exists())
            {
                VehicleExt CopVehicle = new VehicleExt(copcar, true);
                if (copcar.Exists())
                {
                    copcar.IsPersistent = true;
                    CopVehicle.UpdateCopCarLivery(Agency);
                    CopVehicle.UpgradeCopCarPerformance();
                    CreatedVehicles.Add(CopVehicle);
                    GameFiber.Yield();
                    return CopVehicle;
                }
            }
            return null;
        }
    }
}


