using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Util.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mod
{
    public class World
    {
        private static readonly Lazy<World> lazy =
        new Lazy<World>(() => new World());

        public static World Instance { get { return lazy.Value; } }

        private World()
        {
        }
        private Civilians Civilians = new Civilians();
        private List<Blip> CreatedBlips;
        private Dispatcher Dispatcher = new Dispatcher();
        private Pedestrians Pedestrians = new Pedestrians();
        private Police Police = new Police();
        private Scanner Scanner;// = new Scanner();
        private Spawner Spawner = new Spawner();
        private Tasking Tasking = new Tasking();
        private Time Time = new Time();
        private Vehicles Vehicles = new Vehicles();

        public void Start()
        {
            Civilians = new Civilians();
            Dispatcher = new Dispatcher();
            Pedestrians = new Pedestrians();
            Police = new Police();
            Scanner = new Scanner(AudioPlayer);
            Spawner = new Spawner();
            Tasking = new Tasking();
            Time = new Time();
            Vehicles = new Vehicles();
        }

        public IAudioPlayer AudioPlayer;
        public float ActiveDistance => Police.ActiveDistance;
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyCopsNearPlayer => Pedestrians.AnyCopsNearPlayer;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;
        public bool AnyPoliceCanHearPlayer => Police.AnyCanHearPlayer;
        public bool AnyPoliceCanRecognizePlayer => Police.AnyCanRecognizePlayer;
        public bool AnyPoliceCanSeePlayer => Police.AnyCanSeePlayer;
        public bool AnyPoliceRecentlySeenPlayer => Police.AnyRecentlySeenPlayer;
        public bool AnyPoliceSeenPlayerCurrentWanted => Police.AnySeenPlayerCurrentWanted;
        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public string CurrentTime => Time.CurrentTime;
        public bool IsNight => Time.IsNight;
        public int PersistentCivilians => Civilians.PersistentCount;
        public Vector3 PlacePoliceLastSeenPlayer => Police.PlaceLastSeenPlayer;
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public bool ShouldBustPlayer => Pedestrians.ShouldBustPlayer;
        public int TotalSpawnedCops => Pedestrians.TotalSpawnedCops;
        public void AbortScanner()
        {
            Scanner.Abort();
        }
        public void AddBlip(Blip myBlip)
        {
            CreatedBlips.Add(myBlip);
        }
        public void AddCop(Cop myNewCop)
        {
            Pedestrians.Police.Add(myNewCop);
        }
        public void AddTaskablePeds()
        {
            Tasking.AddTaskablePeds();
        }
        public void AddToList(VehicleExt toReturn)
        {
            Vehicles.AddToList(toReturn);
        }
        public void AnnounceCrime(Crime crimeAssociated, PoliceScannerCallIn reportInformation)
        {
            Scanner.AnnounceCrime(crimeAssociated, reportInformation);
        }
        public bool AnyCopsNearPosition(Vector3 position, float radius)
        {
            return Pedestrians.AnyCopsNearPosition(position, radius);
        }
        public void ClearPolice()
        {
            Pedestrians.ClearPolice();
            Vehicles.ClearPolice();
        }
        public int CountNearbyCops(Ped pedestrian)
        {
            return Pedestrians.CountNearbyCops(pedestrian);
        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
            foreach (GameLocation MyLocation in DataMart.Instance.Places.GetAllPlaces())
            {
                MapBlip myBlip = new MapBlip(MyLocation.LocationPosition, MyLocation.Name, MyLocation.Type);
                myBlip.AddToMap();
            }
        }
        public void Delete(Cop cop)
        {
            Spawner.DeleteCop(cop);
        }
        public void Dispatch()
        {
            Dispatcher.Dispatch();
        }
        public void Dispose()
        {
            Time.Dispose();
            Dispatcher.Dispose();
            Spawner.Dispose();
            RemoveBlips();
            ClearPolice();
        }
        public PedExt GetCivilian(uint handle)
        {
            return Pedestrians.GetCivilian(handle);
        }
        public VehicleExt GetVehicle(Vehicle vehicleTryingToEnter)
        {
            return Vehicles.GetVehicle(vehicleTryingToEnter);
        }
        public void MarkNonPersistent(Cop cop)
        {
            Spawner.MarkNonPersistent(cop);
        }
        public void PauseTime()
        {
            Time.PauseTime();
        }
        public void PrintTasksDEBUG()
        {
            Tasking.PrintActivities();
        }
        public void PrunePedestrians()
        {
            Pedestrians.Prune();
        }
        public void PruneVehicles()
        {
            Vehicles.CleanLists();
        }
        public void Recall()
        {
            Dispatcher.Recall();
        }
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                    MyBlip.Delete();
            }
        }
        public void ResetPolice()
        {
            Police.Reset();
        }
        public void ResetScanner()
        {
            Scanner.Reset();
        }
        public void ResetWitnessedCrimes()
        {
            Civilians.ResetWitnessedCrimes();
        }
        public void ScaneForPedestrians()
        {
            Pedestrians.Scan();
        }
        public void ScanForVehicles()
        {
            Vehicles.Scan();
        }
        public void SpawnCop(Agency agency, Vector3 position, float heading, VehicleInformation vehicleInformation)
        {
            Spawner.SpawnCop(agency, position, heading, vehicleInformation);
        }
        public void TaskCivilians()
        {
            Tasking.TaskCivilians();
        }
        public void TaskCops()
        {
            Tasking.TaskCops();
        }
        public void UnPauseTime()
        {
            Time.UnpauseTime();
        }
        public void UpdateCivilians()
        {
            Civilians.Update();
        }
        public void UpdatePolice()
        {
            Police.Update();
        }
        public void UpdateScanner()
        {
            Scanner.Tick();
        }
        public void UpdateTime()
        {
            Time.Tick();
        }
        public void UpdateVehiclePlates()
        {
            Vehicles.UpdatePlates();
        }
        public void VehiclesTick()
        {
            Vehicles.Tick();
        }
    }
}