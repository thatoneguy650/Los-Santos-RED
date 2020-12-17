﻿using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PoliceSpawning
{
    private uint GameTimeLastRemovedCop;
    private uint GameTimeLastSpawnedCop;
    private List<Vehicle> CreatedPoliceVehicles = new List<Vehicle>();
    private List<Entity> CreatedEntities = new List<Entity>();
    private VehicleInformation CurrentVehicleInfo;
    public bool RecentlySpawnedCop
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
    public bool RecentlyRemovedCop
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
    public void Tick()
    {
        RepairOrRemoveDamagedVehicles();
        RemoveAbandonedVehicles();

        if(Mod.Player.PedSwap.RecentlyTakenOver || Mod.Player.Respawning.RecentlySurrenderedToPolice)
        {
            RemoveDisallowedPeds();
        }   
    }
    public bool SpawnGTACop(Agency _Agency, Vector3 SpawnLocation, float Heading, VehicleInformation MyCarInfo, bool CanSpawnOnFoot)
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
                Mod.World.Vehicles.PoliceVehicles.Add(CopCar);
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
                //Mod.World.PoliceEquipmentManager.IssueWeapons(MyNewCop);
                MyNewCop.WasModSpawned = true;
                MyNewCop.WasMarkedNonPersistent = true;
                MyNewCop.WasSpawnedAsDriver = true;

                //MyNewCop.IsBikeCop = MyCarInfo.IsMotorcycle;
                MyNewCop.GameTimeSpawned = Game.GameTime;
                Mod.Debug.WriteToLog("PoliceSpawning", string.Format("Attempting to Spawn: {0}, Vehicle: {1}, PedModel: {2}, PedHandle: {3}, Color: {4}", _Agency.Initials, CopCar.Vehicle.Model.Name, Cop.Model.Name, Cop.Handle, _Agency.AgencyColor));

                if (Mod.DataMart.Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && Cop.Exists())
                {
                    Blip myBlip = Cop.AttachBlip();
                    myBlip.Color = _Agency.AgencyColor;
                    myBlip.Scale = 0.6f;
                    Mod.World.AddBlip(myBlip);
                }
                Mod.World.Pedestrians.Cops.Add(MyNewCop);

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
                                Mod.World.Pedestrians.Cops.Add(MyNewPartnerCop);
                                MyNewPartnerCop.GameTimeSpawned = Game.GameTime;
                                Mod.Debug.WriteToLog("PoliceSpawning", string.Format("        Attempting to Spawn Partner{0}: Agency: {1}, Vehicle: {2}, PedModel: {3}, PedHandle: {4}", OccupantIndex, _Agency.Initials, CopCar.Vehicle.Model.Name, PartnerCop.Model.Name, PartnerCop.Handle));
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
    public void UpgradeCruiser(Vehicle CopCruiser)
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
    public void UpdateLivery(Vehicle CopCar, Agency AssignedAgency)
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
                Mod.Debug.WriteToLog("ChangeLivery", string.Format("No Match for Vehicle {0} for {1}", CopCar.Model.Name, AssignedAgency.Initials));
                CopCar.Delete();
            }
            return;
        }
        if (MyVehicle.Liveries != null && MyVehicle.Liveries.Any())
        {
            //Mod.Debugging.WriteToLog("ChangeLivery", string.Format("Agency {0}, {1}, {2}", AssignedAgency.Initials, CopCar.Model.Name,string.Join(",", MyVehicle.Liveries.Select(x => x.ToString()))));
            int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
            NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        }
        CopCar.LicensePlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    }
    public void UpdateLivery(Vehicle CopCar)
    {
        Agency AssignedAgency = Mod.DataMart.Agencies.GetAgency(CopCar);
        UpdateLivery(CopCar, AssignedAgency);
    }
    public void DeleteCop(Cop Cop)
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
        GameTimeLastRemovedCop = Game.GameTime;
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
                    Cop PassengerCop = Mod.World.Pedestrians.Cops.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
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
    private void RemoveAbandonedVehicles()
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
    private void RepairOrRemoveDamagedVehicles()
    {
        foreach (Cop Cop in Mod.World.Pedestrians.Cops.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer >= 100f && x.Pedestrian.IsInAnyVehicle(false)))//was 175f
        {
            if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth < 1000f)
            {
                Cop.Pedestrian.CurrentVehicle.Repair();
            }
            else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
            {
                DeleteCop(Cop);
                Mod.Debug.WriteToLog("PoliceSpawning", string.Format("Cop GaveUp Delete: {0}", Cop.Pedestrian.Handle));
            }
        }
    }
    private void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
            return;
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }
    private void RemoveDisallowedPeds()
    {
        foreach(Cop myCop in Mod.World.Pedestrians.Cops.Where(x => !x.AssignedAgency.CanSpawn))
        {
            DeleteCop(myCop);
        }
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
        UpdateLivery(CopCar, _Agency);
        GameFiber.Yield();
        if (CopCar.Exists())
        {
            VehicleExt ToReturn = new VehicleExt(CopCar);
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

