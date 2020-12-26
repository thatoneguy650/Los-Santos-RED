using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//Needs some refactoring
public class Spawner : ISpawner
{
    private IWorldLogger World;
    private readonly List<Entity> CreatedEntities = new List<Entity>();
    public Spawner(IWorldLogger world, IDataMart dataMart)
    {
        World = world;
    }
    public void Dispose()
    {
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
    public void SpawnCop(Agency agency, Vector3 position, float heading, VehicleInformation vehicleInfo, int WantedLevel, bool WithBlip)
    {
        try
        {
            DesiredSpawn DS = new DesiredSpawn(agency, position, heading, vehicleInfo, World, WantedLevel, WithBlip);
            DS.SpawnCops();
        }
        catch(Exception ex)
        {
            Game.Console.Print("SpawnCop" + ex.Message + " : " + ex.StackTrace);
        }
    }
    public void DeleteCop(Cop Cop)
    {
        if (Cop == null)
        {
            return;
        }
        if (!Cop.Pedestrian.Exists())
        {
            return;
        }
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
        Cop.WasMarkedNonPersistent = false;
    }
    public void MarkNonPersistent(Cop Cop)
    {
        if (!Cop.Pedestrian.Exists())
        {
            return;
        }
        RemoveBlip(Cop.Pedestrian);
        if (Cop.Pedestrian.IsInAnyVehicle(false))
        {
            if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                {
                    if (Passenger.Exists())
                    {
                        Passenger.IsPersistent = false;
                    }
                }
            }
            Cop.Pedestrian.CurrentVehicle.IsPersistent = false;
        }
        Cop.Pedestrian.IsPersistent = false;
        Cop.WasMarkedNonPersistent = false;
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
    private class SpawnLocation
    {
        public Vector3 Position;
        public float Heading;
        public Zone ZoneAtLocation;
        public Street StreetAtSpawn;
        public SpawnLocation(Vector3 _SpawnLocation, float _Heading, Zone _ZoneAtLocation, Street _StreetAtSpawn)
        {
            Position = _SpawnLocation;
            ZoneAtLocation = _ZoneAtLocation;
            StreetAtSpawn = _StreetAtSpawn;
            Heading = _Heading;
        }
    }
    private class DesiredSpawn//shit name w/e
    {
        private IWorldLogger World;
        private Agency Agency;
        private Vector3 SpawnLocation;
        private float Heading;
        private VehicleInformation VehicleInformation;
        private int WantedLevel;
        private bool HasBlip;
        public DesiredSpawn(Agency agency, Vector3 spawnLocation, float heading, VehicleInformation vehicleInformation, IWorldLogger world, int wantedLevel,bool hasBlip)
        {
            Agency = agency;
            SpawnLocation = spawnLocation;
            Heading = heading;
            VehicleInformation = vehicleInformation;
            World = world;
            WantedLevel = wantedLevel;
            HasBlip = hasBlip;
        }
        public void SpawnCops()
        {
            if (Agency == null)// || !Agency.CanSpawn)
            {
                return;
            }
            VehicleExt CopCar = SpawnCopVehicle();
            GameFiber.Yield();
            if (CopCar == null || !CopCar.Vehicle.Exists())
            {
                return;
            }
            World.AddToList(CopCar);
            Ped Cop = SpawnCopPed(WantedLevel);
            GameFiber.Yield();
            if (Cop == null || !Cop.Exists() || !CopCar.Vehicle.Exists())
            {
                return;
            }
            Cop.WarpIntoVehicle(CopCar.Vehicle, -1);
            Cop.IsPersistent = true;
            CopCar.Vehicle.IsPersistent = true;
            Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
            Cop MyNewCop = new Cop(Cop, Cop.Health, Agency, true);
            MyNewCop.WasMarkedNonPersistent = true;
            MyNewCop.WasSpawnedAsDriver = true;
            
            if (HasBlip && Cop.Exists())
            {
                Blip myBlip = Cop.AttachBlip();
                myBlip.Color = Agency.AgencyColor;
                myBlip.Scale = 0.6f;
                World.AddBlip(myBlip);
            }


            World.AddCop(MyNewCop);
            //Game.Console.Print("PoliceSpawning", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.Vehicle.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));
            if (VehicleInformation != null)
            {
                int OccupantsToAdd = RandomItems.MyRand.Next(VehicleInformation.MinOccupants, VehicleInformation.MaxOccupants + 1) - 1;
                for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                {
                    Ped PartnerCop = SpawnCopPed(WantedLevel);
                    GameFiber.Yield();
                    if (PartnerCop != null)
                    {
                        //CreatedEntities.Add(PartnerCop);
                        if (!CopCar.Vehicle.Exists())
                        {
                            if (PartnerCop.Exists())
                            {
                                PartnerCop.Delete();
                            }
                        }
                        else
                        {
                            PartnerCop.WarpIntoVehicle(CopCar.Vehicle, OccupantIndex - 1);
                            PartnerCop.IsPersistent = true;
                            Cop MyNewPartnerCop = new Cop(PartnerCop, PartnerCop.Health, Agency, true);
                            MyNewPartnerCop.WasMarkedNonPersistent = true;
                            if (HasBlip && PartnerCop.Exists())
                            {
                                Blip myBlip = PartnerCop.AttachBlip();
                                myBlip.Color = Agency.AgencyColor;
                                myBlip.Scale = 0.6f;
                                World.AddBlip(myBlip);
                            }
                            World.AddCop(MyNewPartnerCop);
                            //Game.Console.Print("PoliceSpawning", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.Vehicle.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
                        }
                    }
                }

            }
        }
        private Ped SpawnCopPed(int WantedLevel)
        {
            if (Agency == null)
            {
                return null;
            }
            List<string> RequiredModels = new List<string>();
            if (VehicleInformation != null && VehicleInformation.AllowedPedModels.Any())
            {
                RequiredModels = VehicleInformation.AllowedPedModels;
            }
            PedestrianInformation MyInfo = Agency.GetRandomPed(RequiredModels, WantedLevel);
            if (MyInfo == null)
            {
                return null;
            }
            Vector3 SafeSpawnLocation = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z + 1f);//+5f
            Ped Cop = new Ped(MyInfo.ModelName, SafeSpawnLocation, 0f);
            if (!Cop.Exists())
            {
                return null;
            }
            NativeFunction.CallByName<bool>("SET_PED_AS_COP", Cop, true);
            Cop.RandomizeVariation();
            if (VehicleInformation.IsMotorcycle)
            {
                Cop.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 0, 0, 0);
            }
            else
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 4, 1, 0, 0);
            }
            if (MyInfo.RequiredVariation != null)
            {
                MyInfo.RequiredVariation.ReplacePedComponentVariation(Cop);
            }
            return Cop;
        }
        private VehicleExt SpawnCopVehicle()
        {
            Vehicle CopCar = new Vehicle(VehicleInformation.ModelName, SpawnLocation, Heading);
            GameFiber.Yield();
            if (CopCar.Exists())
            {
                VehicleExt ToReturn = new VehicleExt(CopCar, true);
                if (CopCar.Exists())
                {
                    ToReturn.UpdateCopCarLivery(Agency);
                    ToReturn.UpgradeCopCarPerformance();
                    World.AddToList(ToReturn);
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
    }
}


