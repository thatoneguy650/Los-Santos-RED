using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//Needs some refactoring
public class Spawner
{
    private readonly List<Vehicle> CreatedPoliceVehicles = new List<Vehicle>();
    private readonly List<Entity> CreatedEntities = new List<Entity>();
    private VehicleInformation CurrentVehicleInfo;
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
    public bool SpawnCop(Agency _Agency, Vector3 SpawnLocation, float Heading, VehicleInformation MyCarInfo, bool CanSpawnOnFoot)
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

            if (CopCar != null && CopCar.Vehicle.Exists())
            {

                Mod.World.Vehicles.AddToList(CopCar);




                List<string> RequiredPedModels = new List<string>();
                if (CurrentVehicleInfo != null && CurrentVehicleInfo.AllowedPedModels.Any())
                {
                    RequiredPedModels = CurrentVehicleInfo.AllowedPedModels;
                }

                Ped Cop = SpawnCopPed(_Agency, SpawnLocation, MyCarInfo.IsMotorcycle, RequiredPedModels);
                GameFiber.Yield();
                if (Cop == null || !Cop.Exists() || !CopCar.Vehicle.Exists())
                    return false;
                CreatedEntities.Add(Cop);
                CreatedPoliceVehicles.Add(CopCar.Vehicle);
                CreatedEntities.Add(CopCar.Vehicle);
                Cop.WarpIntoVehicle(CopCar.Vehicle, -1);
                Cop.IsPersistent = true;
                CopCar.Vehicle.IsPersistent = true;
                Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                Cop MyNewCop = new Cop(Cop, Cop.Health, _Agency);
                MyNewCop.Loadout.IssueWeapons();
                MyNewCop.WasModSpawned = true;
                MyNewCop.WasMarkedNonPersistent = true;
                MyNewCop.WasSpawnedAsDriver = true;
                MyNewCop.GameTimeSpawned = Game.GameTime;
                Mod.Debug.WriteToLog("PoliceSpawning", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.Vehicle.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

                if (Mod.DataMart.Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
                {
                    Blip myBlip = Cop.AttachBlip();
                    myBlip.Color = _Agency.AgencyColor;
                    myBlip.Scale = 0.6f;
                    Mod.World.AddBlip(myBlip);
                }
                Mod.World.Pedestrians.Police.Add(MyNewCop);

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
                            if (!CopCar.Vehicle.Exists())
                            {
                                if (PartnerCop.Exists())
                                    PartnerCop.Delete();
                            }
                            else
                            {
                                PartnerCop.WarpIntoVehicle(CopCar.Vehicle, OccupantIndex - 1);
                                PartnerCop.IsPersistent = true;
                                Cop MyNewPartnerCop = new Cop(PartnerCop, PartnerCop.Health, _Agency);
                                //Mod.World.PoliceEquipmentManager.IssueWeapons(MyNewPartnerCop);
                                MyNewPartnerCop.Loadout.IssueWeapons();
                                MyNewPartnerCop.WasModSpawned = true;
                                MyNewPartnerCop.WasMarkedNonPersistent = true;

                                if (Mod.DataMart.Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && PartnerCop.Exists())
                                {
                                    Blip myBlip = PartnerCop.AttachBlip();
                                    myBlip.Color = _Agency.AgencyColor;
                                    myBlip.Scale = 0.6f;
                                    Mod.World.AddBlip(myBlip);
                                }
                                Mod.World.Pedestrians.Police.Add(MyNewPartnerCop);
                                MyNewPartnerCop.GameTimeSpawned = Game.GameTime;
                                Mod.Debug.WriteToLog("PoliceSpawning", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.Vehicle.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
    public void Delete(Cop Cop)
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
            Mod.Debug.WriteToLog("PoliceSpawning", string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle,Cop.DistanceToPlayer,Cop.AssignedAgency.Initials));
            Cop.Pedestrian.Delete();
        }
        Cop.WasMarkedNonPersistent = false;
    }
    public void MarkNonPersistent(Cop Cop)
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
                    Cop PassengerCop = Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
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

    private void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
            return;
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }

    private Ped SpawnCopPed(Agency _Agency,Vector3 SpawnLocation, bool IsBike, List<string> RequiredModels)
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
        if (!Mod.World.IsNightTime)
            NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses

        if (MyInfo.RequiredVariation != null)
        {
            MyInfo.RequiredVariation.ReplacePedComponentVariation(Cop);
        }


        return Cop;
    }
    private VehicleExt SpawnCopVehicle(Agency _Agency, VehicleInformation MyCarInfo, Vector3 SpawnLocation,float Heading)
    {
        string ModelName = MyCarInfo.ModelName;
        Vehicle CopCar = new Vehicle(ModelName, SpawnLocation, Heading);
        GameFiber.Yield();
        if (CopCar.Exists())
        {
            VehicleExt ToReturn = new VehicleExt(CopCar,true);
            if (CopCar.Exists())
            {
                ToReturn.UpdateCopCarLivery(_Agency);
                ToReturn.UpgradeCopCarPerformance();
                CurrentVehicleInfo = MyCarInfo;
                Mod.World.Vehicles.AddToList(ToReturn);
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


