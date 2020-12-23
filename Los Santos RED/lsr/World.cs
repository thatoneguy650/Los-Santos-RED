using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Drawing;

namespace LosSantosRED.lsr
{
    public class World
    {
        private Civilians Civilians = new Civilians();
        private List<Blip> CreatedBlips;
        private Dispatcher Dispatcher = new Dispatcher();
        private Police Police = new Police();
        private Scanner Scanner = new Scanner();
        private Spawner Spawner = new Spawner();
        private Tasking Tasking = new Tasking();
        private Time Time = new Time();
        private Vehicles Vehicles = new Vehicles();
        public World()
        {
        }
        public float ActiveDistance
        {
            get
            {
                return Police.ActiveDistance;
            }
        }
        public bool AnyPoliceCanHearPlayer => Police.AnyCanHearPlayer;
        public bool AnyPoliceCanRecognizePlayer => Police.AnyCanRecognizePlayer;
        public bool AnyPoliceCanSeePlayer => Police.AnyCanSeePlayer;
        public bool AnyPoliceRecentlySeenPlayer => Police.AnyRecentlySeenPlayer;
        public bool AnyPoliceSeenPlayerCurrentWanted => Police.AnySeenPlayerCurrentWanted;
        public string CurrentTime
        {
            get
            {
                return Time.CurrentTime;
            }
        }
        public bool IsNight
        {
            get
            {
                return Time.IsNight;
            }
        }
        public Pedestrians Pedestrians { get; private set; } = new Pedestrians();//soon you will be 
        public int PersistentCivilians
        {
            get
            {
                return Civilians.PersistentCount;
            }
        }
        public Vector3 PlacePoliceLastSeenPlayer => Police.PlaceLastSeenPlayer;
        public int PoliceBoatsCount
        {
            get
            {
                return Vehicles.PoliceBoatsCount;
            }
        }
        public int PoliceHelicoptersCount
        {
            get
            {
                return Vehicles.PoliceHelicoptersCount;
            }
        }
        public void AbortScanner()
        {
            Scanner.Abort();
        }
        public void AddBlip(Blip myBlip)
        {
            CreatedBlips.Add(myBlip);
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
        public void ClearPolice()
        {
            Pedestrians.ClearPolice();
            Vehicles.ClearPolice();
        }
        public void CreateLocationBlips()
        {
            CreatedBlips = new List<Blip>();
            foreach (GameLocation MyLocation in Mod.DataMart.Places.GetAllPlaces())
            {
                MapBlip myBlip = new MapBlip(MyLocation.LocationPosition, MyLocation.Name, MyLocation.Type);
                myBlip.Create();
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
            Civilians.Tick();
        }
        public void UpdatePolice()
        {
            Police.Tick();
        }
        public void UpdatePoliceSpeech()
        {
            Police.SpeechTick();
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
        private class MapBlip
        {
            public MapBlip(Vector3 LocationPosition, string Name, LocationType Type)
            {
                this.LocationPosition = LocationPosition;
                this.Name = Name;
                this.Type = Type;
            }
            public Vector3 LocationPosition { get; }
            public string Name { get; }
            public LocationType Type { get; }
            private BlipSprite Icon
            {
                get
                {
                    if (Type == LocationType.Hospital)
                    {
                        return BlipSprite.Hospital;
                    }
                    else if (Type == LocationType.Police)
                    {
                        return BlipSprite.PoliceStation;
                    }
                    else if (Type == LocationType.ConvenienceStore)
                    {
                        return BlipSprite.CriminalHoldups;
                    }
                    else if (Type == LocationType.GasStation)
                    {
                        return BlipSprite.JerryCan;
                    }
                    else
                    {
                        return BlipSprite.Objective;
                    }
                }
            }
            public void Create()
            {
                Blip MyLocationBlip = new Blip(LocationPosition)
                {
                    Name = Name
                };
                MyLocationBlip.Sprite = Icon;
                MyLocationBlip.Color = Color.White;
                NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
            }
        }
    }
}