using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public static partial class Agencies
{
    [Serializable()]
    public class Agency
    {
        public string ColorPrefix { get; set; } = "~s~";
        public string Initials { get; set; } = "UNK";
        public string FullName { get; set; } = "Unknown";
        public List<ModelInformation> CopModels { get; set; } = new List<ModelInformation>();
        public List<VehicleInformation> Vehicles { get; set; } = new List<VehicleInformation>();
        public string AgencyColorString { get; set; } = "White";
        public Classification AgencyClassification { get; set; } = Classification.Other;
        public string LicensePlatePrefix { get; set; } = "";
        public bool SpawnsOnHighway { get; set; } = false;
        public bool CanSpawnAnywhere { get; set; } = false;
        public uint MinWantedLevelSpawn { get; set; } = 0;
        public uint MaxWantedLevelSpawn { get; set; } = 5;
        public int SpawnLimit { get; set; } = 99;
        public List<IssuedWeapon> IssuedWeapons { get; set; } = new List<IssuedWeapon>();
        public List<ZoneJurisdiction> Jurisdiction { get; set; } = new List<ZoneJurisdiction>();
        public bool CanSpawn
        {
            get
            {
                if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                {
                    if (PedList.CopPeds.Count(x => x.AssignedAgency == this) < SpawnLimit)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }
        public bool HasMotorcycles
        {
            get
            {
                return Vehicles.Any(x => x.IsMotorcycle);
            }
        }
        public bool HasSpawnableHelicopters
        {
            get
            {
                return Vehicles.Any(x => x.IsHelicopter && x.CanCurrentlySpawn);
            }
        }
        public Color AgencyColor
        {
            get
            {
                return Color.FromName(AgencyColorString);
            }
        }
        public string ColoredInitials
        {
            get
            {
                return ColorPrefix + Initials;
            }
        }
        public bool CanCheckTrafficViolations
        {
            get
            {
                if (AgencyClassification == Classification.Police || AgencyClassification == Classification.Federal || AgencyClassification == Classification.Sheriff)
                    return true;
                else
                    return false;
            }
        }
        public enum Classification
        {
            Police = 0,
            Sheriff = 1,
            Federal = 2,
            State = 3,
            Security = 4,
            Military = 5,
            Other = 6,
        }
        public VehicleInformation GetVehicleInfo(Vehicle CopCar)
        {
            return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
        }
        public VehicleInformation GetRandomVehicle()
        {
            if (Vehicles == null || !Vehicles.Any())
                return null;

            List<VehicleInformation> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn).ToList();
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
            // Debugging.WriteToLog("GetRandomVehicle", string.Format("Total Chance {0}, Items {1}", Total, string.Join(",",ToPickFrom.Select( x => x.ModelName + " " + x.CanCurrentlySpawn + "  " + x.CurrentSpawnChance))));
            int RandomPick = General.MyRand.Next(0, Total);
            foreach (VehicleInformation Vehicle in ToPickFrom)
            {
                int SpawnChance = Vehicle.CurrentSpawnChance;
                if (RandomPick < SpawnChance)
                {
                    return Vehicle;
                }
                RandomPick -= SpawnChance;
            }
            return null;
        }
        public ModelInformation GetRandomPed(List<string> RequiredModels)
        {
            if (CopModels == null || !CopModels.Any())
                return null;

            List<ModelInformation> ToPickFrom = CopModels.Where(x => PlayerState.WantedLevel >= x.MinWantedLevelSpawn && PlayerState.WantedLevel <= x.MaxWantedLevelSpawn).ToList();
            if (RequiredModels != null && RequiredModels.Any())
            {
                ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
            //Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
            int RandomPick = General.MyRand.Next(0, Total);
            foreach (ModelInformation Cop in ToPickFrom)
            {
                int SpawnChance = Cop.CurrentSpawnChance;
                if (RandomPick < SpawnChance)
                {
                    return Cop;
                }
                RandomPick -= SpawnChance;
            }
            return null;
        }
        public Agency()
        {

        }
        public Agency(string _ColorPrefix, string _Initials, string _FullName, string _AgencyColorString, Classification _AgencyClassification,List<ZoneJurisdiction> _Jurisdictions, List<ModelInformation> _CopModels, List<VehicleInformation> _Vehicles, string _LicensePlatePrefix, List<IssuedWeapon> _IssuedWeapons)
        {
            ColorPrefix = _ColorPrefix;
            Initials = _Initials;
            FullName = _FullName;
            CopModels = _CopModels;
            AgencyColorString = _AgencyColorString;
            Vehicles = _Vehicles;
            AgencyClassification = _AgencyClassification;
            LicensePlatePrefix = _LicensePlatePrefix;
            IssuedWeapons = _IssuedWeapons;
            Jurisdiction = _Jurisdictions;
        }
        public class ModelInformation
        {
            public string ModelName;
            public int AmbientSpawnChance = 0;
            public int WantedSpawnChance = 0;
            public int MinWantedLevelSpawn = 0;
            public int MaxWantedLevelSpawn = 5;
            public PedVariation RequiredVariation;
            public bool CanCurrentlySpawn
            {
                get
                {
                    if (PlayerState.IsWanted)
                    {
                        if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                            return WantedSpawnChance > 0;
                        else
                            return false;
                    }
                    else
                        return AmbientSpawnChance > 0;
                }
            }
            public int CurrentSpawnChance
            {
                get
                {
                    if (PlayerState.IsWanted)
                    {
                        if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                            return WantedSpawnChance;
                        else
                            return 0;
                    }
                    else
                        return AmbientSpawnChance;
                }
            }
            public ModelInformation()
            {

            }
            public ModelInformation(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
            {
                ModelName = _ModelName;
                AmbientSpawnChance = ambientSpawnChance;
                WantedSpawnChance = wantedSpawnChance;
            }
        }
        public class VehicleInformation
        {
            public string ModelName;
            public int AmbientSpawnChance = 0;
            public int WantedSpawnChance = 0;
            public bool IsCar
            {
                get
                {
                    return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Game.GetHashKey(ModelName));
                }
            }
            public bool IsMotorcycle
            {
                get
                {
                    return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BIKE", Game.GetHashKey(ModelName));
                }
            }
            public bool IsHelicopter
            {
                get
                {
                    return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_HELI", Game.GetHashKey(ModelName));
                }
            }
            public bool IsBoat
            {
                get
                {
                    return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BOAT", Game.GetHashKey(ModelName));
                }
            }
            public int MinOccupants = 1;
            public int MaxOccupants = 2;
            public int MinWantedLevelSpawn = 0;
            public int MaxWantedLevelSpawn = 5;
            public List<string> AllowedPedModels = new List<string>();//only ped models can spawn in this, if emptyt any ambient spawn can
            public List<int> Liveries = new List<int>();
            public bool CanSpawnWanted
            {
                get
                {
                    if (WantedSpawnChance > 0)
                        return true;
                    else
                        return false;
                }
            }
            public bool CanSpawnAmbient
            {
                get
                {
                    if (AmbientSpawnChance > 0)
                        return true;
                    else
                        return false;
                }
            }
            public bool CanCurrentlySpawn
            {
                get
                {
                    if (IsHelicopter && PedList.PoliceVehicles.Count(x => x.IsHelicopter) >= General.MySettings.Police.HelicopterLimit)
                    {
                        return false;
                    }
                    else if (IsBoat && PedList.PoliceVehicles.Count(x => x.IsBoat) >= General.MySettings.Police.BoatLimit)
                    {
                        return false;
                    }

                    if (PlayerState.IsWanted)
                    {
                        if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                            return CanSpawnWanted;
                        else
                            return false;
                    }
                    else
                        return CanSpawnAmbient;
                }
            }
            public int CurrentSpawnChance
            {
                get
                {
                    if (!CanCurrentlySpawn)
                        return 0;
                    if (PlayerState.IsWanted)
                    {
                        if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                            return WantedSpawnChance;
                        else
                            return 0;
                    }
                    else
                        return AmbientSpawnChance;
                }
            }
            public VehicleInformation()
            {

            }
            public VehicleInformation(string modelName, int ambientSpawnChance, int wantedSpawnChance)
            {
                ModelName = modelName;
                AmbientSpawnChance = ambientSpawnChance;
                WantedSpawnChance = wantedSpawnChance;
            }
        }
        public class IssuedWeapon
        {
            public string ModelName;
            public bool IsPistol = false;
            public GTAWeapon.WeaponVariation MyVariation = new GTAWeapon.WeaponVariation();
            public IssuedWeapon()
            {

            }
            public IssuedWeapon(string _ModelName, bool _IsPistol, GTAWeapon.WeaponVariation _MyVariation)
            {
                ModelName = _ModelName;
                IsPistol = _IsPistol;
                MyVariation = _MyVariation;
            }

        }
        public class ZoneJurisdiction
        {
            public string ZoneInternalGameName;
            public int Priority;
            public int AmbientSpawnChance = 0;
            public int WantedSpawnChance = 0;
            public ZoneJurisdiction()
            {

            }
            public ZoneJurisdiction(string associatedAgencyName, int priority, int ambientSpawnChance, int wantedSpawnChance)
            {
                ZoneInternalGameName = associatedAgencyName;
                Priority = priority;
                AmbientSpawnChance = ambientSpawnChance;
                WantedSpawnChance = wantedSpawnChance;
            }
            public bool CanCurrentlySpawn
            {
                get
                {
                    if (PlayerState.IsWanted)
                        return WantedSpawnChance > 0;
                    else
                        return AmbientSpawnChance > 0;
                }
            }
            public int CurrentSpawnChance
            {
                get
                {
                    if (PlayerState.IsWanted)
                        return WantedSpawnChance;
                    else
                        return AmbientSpawnChance;
                }
            }
        }
    }
}


