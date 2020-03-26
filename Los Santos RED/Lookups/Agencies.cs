using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Agencies
{
    
    public static List<Agency> AgenciesList { get; set; }
    public static Agency LSPD;
    public static Agency LSSD;
    public static Agency SAPR;
    public static Agency DOA;
    public static Agency FIB;
    public static Agency IAA;
    public static Agency SAHP;
    public static Agency SASPA;
    public static Agency ARMY;
    public static Agency UNK;
    public static Agency PRISEC;
    public static Agency LSPA;
    public static Agency LSIAPD;
    public static Agency BCSO;
    public static Agency VPPD;
    public static Agency LSPDVW;
    public static Agency LSPDELS;
    public static Agency LSPDCH;
    public static Agency RHPD;
    public static Agency SACG;
    public static Agency NOOSE;

    public static Agency LSSDVW;
    public static Agency LSSDCH;
    public static void Initialize()
    {
        List<Agency.ModelInformation> StandardCops = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) };
        List<Agency.ModelInformation> ParkRangers = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_ranger_01", true), new Agency.ModelInformation("s_f_y_ranger_01", false) };
        List<Agency.ModelInformation> SheriffPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_sheriff_01", true), new Agency.ModelInformation("s_f_y_sheriff_01", false) };
        List<Agency.ModelInformation> SWAT = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_swat_01", true, false) };
        List<Agency.ModelInformation> DOAPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("u_m_m_doa_01", true) };
        List<Agency.ModelInformation> IAAPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_fibsec_01", true) };
        List<Agency.ModelInformation> SAHPPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_hwaycop_01", true) };
        List<Agency.ModelInformation> MilitaryPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_armymech_01", true), new Agency.ModelInformation("s_m_m_marine_01", true), new Agency.ModelInformation("s_m_m_marine_02", true), new Agency.ModelInformation("s_m_y_marine_01", true), new Agency.ModelInformation("s_m_y_marine_02", true), new Agency.ModelInformation("s_m_y_marine_03", true), new Agency.ModelInformation("s_m_m_pilot_02", true), new Agency.ModelInformation("s_m_y_pilot_01", true) };
        List<Agency.ModelInformation> FIBPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_fibsec_01", true) };
        List<Agency.ModelInformation> PrisonPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_prisguard_01", true) };
        List<Agency.ModelInformation> SecurityPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_security_01", true) };
        List<Agency.ModelInformation> CoastGuardPeds = new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_uscg_01", true) };
        
        List<Agency.VehicleInformation> UnmarkedVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("police4", true, true, 100) };
        List<Agency.VehicleInformation> ParkRangerVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("pranger", true, true, 100) };
        List<Agency.VehicleInformation> FIBVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("fbi", true, true, 70), new Agency.VehicleInformation("fbi2", true, true, 30) };
        //List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("policeb", true, true, 70,true), new Agency.VehicleInformation("police4", true, true, 30) };
        List<Agency.VehicleInformation> PrisonVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("policet", true, true, 70), new Agency.VehicleInformation("police4", true, true, 30) };

        List<Agency.VehicleInformation> LSPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 5,false,new List<int>() { 0,1 }),
            new Agency.VehicleInformation("police2", true, true, 55, false,new List<int>() { 0,1 }),
            new Agency.VehicleInformation("police3", true, true, 3, false,new List<int>() { 0,1 }),
            new Agency.VehicleInformation("pscout", true, true, 30, false,new List<int>() { 0,1 }),
            new Agency.VehicleInformation("police4", true, true, 3),
            new Agency.VehicleInformation("fbi2", true, true, 4) };

        List<Agency.VehicleInformation> VWPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 5,false,new List<int>() { 2,3 }),
            new Agency.VehicleInformation("police2", true, true, 5,false,new List<int>() { 2,3 }),
            new Agency.VehicleInformation("police3", true, true, 5,false,new List<int>() { 2,3 }),
            new Agency.VehicleInformation("pscout", true, true, 85, false, new List<int>() { 2,3 }) };

        List<Agency.VehicleInformation> ChumashLSPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 45,false,new List<int>() { 4,5 }),
            new Agency.VehicleInformation("police2", true, true, 45,false,new List<int>() { 4,5 }),
            new Agency.VehicleInformation("police3", true, true, 5,false,new List<int>() { 4,5 }),
            new Agency.VehicleInformation("pscout", true, true, 5, false, new List<int>() { 4,5 }) };

        List<Agency.VehicleInformation> EastLSPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 95,false,new List<int>() { 6,7 }),
            //new Agency.VehicleInformation("police2", true, true, 30,false,new List<int>() { 6,7 }),
            new Agency.VehicleInformation("police3", true, true, 5,false,new List<int>() { 6,7 }),
           // new Agency.VehicleInformation("pscout", true, true, 35, false, new List<int>() { 6,7 })

        };

        List<Agency.VehicleInformation> RHPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 30,false,new List<int>() { 8,9 }),
            new Agency.VehicleInformation("police2", true, true, 30,false,new List<int>() { 8,9 }),
            new Agency.VehicleInformation("police3", true, true, 5,false,new List<int>() { 8,9 }),
            new Agency.VehicleInformation("pscout", true, true, 35, false,new List<int>() { 8,9 })};

        List<Agency.VehicleInformation> VPPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", true, true, 30,false,new List<int>() { 10,11 }),
            new Agency.VehicleInformation("police2", true, true, 30,false,new List<int>() { 10,11 }),
            new Agency.VehicleInformation("police3", true, true, 10,false,new List<int>() { 10,11 }),
            new Agency.VehicleInformation("pscout", true, true, 35, false,new List<int>() { 10,11 })};

        List<Agency.VehicleInformation> SAHPVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policeb", true, true, 35, true),
            new Agency.VehicleInformation("police4", true, true, 5),
            new Agency.VehicleInformation("police", true, true, 30,false, new List<int>() { 12,13 }),
            new Agency.VehicleInformation("pscout", true, true, 30,false, new List<int>() { 12,13 })};

        List<Agency.VehicleInformation> LSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", true, true, 10, false, new List<int> { 0, 1, 2, 3 }),
            new Agency.VehicleInformation("sheriff2", true, true, 90, false, new List<int> { 0, 1 }) };

        List<Agency.VehicleInformation> BCSOVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", true, true, 10, false, new List<int> { 4,5,6 }),
            new Agency.VehicleInformation("sheriff2", true, true, 90, false, new List<int> { 2,3 })};

        List<Agency.VehicleInformation> VWHillsLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", true, true, 10, false, new List<int> { 7,8 }),
            new Agency.VehicleInformation("sheriff2", true, true, 90, false, new List<int> { 4,5 }) };

        List<Agency.VehicleInformation> ChumashLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", true, true, 10, false, new List<int> { 9,10}),
            new Agency.VehicleInformation("sheriff2", true, true, 90, false, new List<int> { 6,7 }) };



        //List<Agency.VehicleInformation> LSPDVehiclesVanilla = new List<Agency.VehicleInformation>() {
        //    new Agency.VehicleInformation("police", true, true, 25,false,new List<int>() { 0,1,2,3,4,5 }),
        //    new Agency.VehicleInformation("police2", true, true, 25, false,new List<int>() { 0,1,2,3,4,5,6,7 }),
        //    new Agency.VehicleInformation("police3", true, true, 25, false,new List<int>() { 0,1,2,3,4,5,6,7 }),
        //    new Agency.VehicleInformation("police4", true, true, 10),
        //    new Agency.VehicleInformation("fbi2", true, true, 15) };

        //List<Agency.VehicleInformation> LSSDVehiclesVanilla = new List<Agency.VehicleInformation>() {
        //    new Agency.VehicleInformation("sheriff", true, true, 50, false, new List<int> { 0, 1, 2, 3 }),
        //    new Agency.VehicleInformation("sheriff2", true, true, 50, false, new List<int> { 0, 1, 2, 3 }) };

        AgenciesList = new List<Agency>();
        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Department", Color.Blue,Agency.Classification.Police,true, true, StandardCops, LSPDVehicles,"LS ");
        SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", Color.Yellow, Agency.Classification.Police, true, true, SAHPPeds, SAHPVehicles,"HP ");
        LSIAPD = new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", Color.LightBlue, Agency.Classification.Police, true, true, StandardCops, LSPDVehicles, "LSA ");
        VPPD = new Agency("~b~", "VPPD", "Vespucci Police Department", Color.DarkBlue, Agency.Classification.Police, false, true, StandardCops, VPPDVehicles, "VP ");   
        RHPD = new Agency("~b~", "RHPD", "Rockford Hills Police Department", Color.LightBlue, Agency.Classification.Police, false, true, StandardCops, RHPDVehicles, "RH ");

        LSPDVW = new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", Color.Blue, Agency.Classification.Police, false, true, StandardCops, VWPDVehicles, "LSV ");
        LSPDELS = new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", Color.Blue, Agency.Classification.Police, false, true, StandardCops, EastLSPDVehicles, "LSE ");
        LSPDCH = new Agency("~b~", "LSPD-CH", "Los Santos Police - Chumash Division", Color.Blue, Agency.Classification.Police, false, true, StandardCops, ChumashLSPDVehicles, "LSC ");

        LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", Color.Red, Agency.Classification.Sheriff,true, true, SheriffPeds, LSSDVehicles,"LSCS ");
        BCSO = new Agency("~r~", "BCSO", "Blaine County Sheriffs Office", Color.DarkRed, Agency.Classification.Sheriff, false, true, SheriffPeds, BCSOVehicles, "BCS ");

        LSSDVW = new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", Color.Red, Agency.Classification.Sheriff, false, true, SheriffPeds, VWHillsLSSDVehicles, "LSCS ");
        LSSDCH = new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", Color.Red, Agency.Classification.Sheriff, false, true, SheriffPeds, ChumashLSSDVehicles, "LSCS ");

        SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", Color.Green, Agency.Classification.Federal,true, true, ParkRangers, ParkRangerVehicles,"");
        DOA = new Agency("~p~", "DOA", "Drug Observation Agency", Color.Purple, Agency.Classification.Federal,true, true, DOAPeds,UnmarkedVehicles, "DOA ");
        FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", Color.Purple, Agency.Classification.Federal,true, true,FIBPeds,FIBVehicles, "FIB ");
        IAA = new Agency("~p~", "IAA", "International Affairs Agency", Color.Purple, Agency.Classification.Federal,true, false,IAAPeds,UnmarkedVehicles, "IAA ");
        ARMY = new Agency("~u~", "ARMY", "Army", Color.Black, Agency.Classification.Federal, true, false, MilitaryPeds, null, "");
        NOOSE = new Agency("~r~", "NOOSE", "National Office of Security Enforcement", Color.DarkRed, Agency.Classification.Federal, true, false, SWAT, FIBVehicles, "");

        PRISEC = new Agency("~o~", "PRISEC", "Private Security", Color.White, Agency.Classification.Security, true, true, SecurityPeds, UnmarkedVehicles, "");
        LSPA = new Agency("~p~", "LSPA", "Port Authority of Los Santos", Color.LightGray, Agency.Classification.Security, true, true, SecurityPeds, UnmarkedVehicles, "LSPA ");

        SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", Color.Orange, Agency.Classification.Other,true, true,PrisonPeds,PrisonVehicles, "SASPA ");
        UNK = new Agency("~s~", "UNK", "Unknown Agency", Color.White, Agency.Classification.Other, true, false, null, null, "");
        SACG = new Agency("~o~", "SACG", "San Andreas Coast Guard", Color.DarkOrange, Agency.Classification.Other, true, false, CoastGuardPeds, UnmarkedVehicles, "SACG ");

        AgenciesList.Add(LSPD);
        AgenciesList.Add(SAHP);
        AgenciesList.Add(LSIAPD);
        AgenciesList.Add(VPPD);
        AgenciesList.Add(RHPD);

        AgenciesList.Add(LSPDVW);
        AgenciesList.Add(LSPDELS);
        AgenciesList.Add(LSPDCH);

        AgenciesList.Add(LSSD);
        AgenciesList.Add(BCSO);
        AgenciesList.Add(LSSDVW);
        AgenciesList.Add(LSSDCH);

        AgenciesList.Add(SAPR);
        AgenciesList.Add(DOA);
        AgenciesList.Add(FIB);
        AgenciesList.Add(IAA);
        AgenciesList.Add(ARMY);
        AgenciesList.Add(NOOSE);

        AgenciesList.Add(PRISEC);
        AgenciesList.Add(LSPA);

        AgenciesList.Add(SASPA);
        AgenciesList.Add(UNK);
        AgenciesList.Add(SACG);

    }
    public static void Dispose()
    {

    }
    public static Agency GetAgencyFromPed(Ped Cop)
    {
        if (!Cop.IsPoliceArmy())
            return UNK;
        if (Cop.IsArmy())
            return ARMY;
        else if (Cop.IsPolice())
        {
            Agency ToReturn;
            if (Cop.Model.Name.ToLower() == "s_m_y_swat_01")//Swat depends on unit insignias
             {
                ToReturn = GetAgencyFromSwat(Cop);
            }
            else if (Cop.Model.Name.ToLower() == "s_m_m_security_01")//Security depends on where they are
            {
                ToReturn = GetAgencyFromSecurity(Cop);
            }
            else
            {
                ToReturn = GetPedAgencyFromZone(Cop);
            }
            return ToReturn;
        }
        else
            return null;
    }
    private static Agency GetPedAgencyFromZone(Ped Cop)
    {
        Zone ZoneFound = Zones.GetZoneAtLocation(Cop.Position);
        Agency ZoneAgency = LSPD;
        if (ZoneFound != null)
        {
            foreach (ZoneAgency MyAgency in ZoneFound.ZoneAgencies)
            {
                if (MyAgency.AssiciatedAgency.CopModels != null && MyAgency.AssiciatedAgency.CopModels.Any())
                {
                    if (MyAgency.AssiciatedAgency.CopModels.Any(x => x.ModelName == Cop.Model.Name.ToLower()))
                    {
                        ZoneAgency = MyAgency.AssiciatedAgency;
                        break;
                    }
                }
            }
        }
        return ZoneAgency;
    }
    public static void ChangeLivery(Vehicle CopCar, Agency AssignedAgency)
    {
        Agency.VehicleInformation MyVehicle = null;
        if (AssignedAgency != null && AssignedAgency.Vehicles != null)
        {
            MyVehicle = AssignedAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
        }
        if (MyVehicle == null || MyVehicle.Liveries == null || !MyVehicle.Liveries.Any())
        {
            ChangeToDefaultLivery(CopCar);
            return;
        }
        int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
        NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        CopCar.LicensePlate = AssignedAgency.LicensePlatePrefix + LosSantosRED.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    }
    public static void CheckandChangeLivery(Vehicle CopCar)
    {
        Zone ZoneFound = Zones.GetZoneAtLocation(CopCar.Position);
        Agency.VehicleInformation MyVehicle = null;
        Agency ZoneAgency = null;
        if (ZoneFound != null)
        {
            foreach (ZoneAgency MyAgency in ZoneFound.ZoneAgencies)
            {
                if (MyAgency.AssiciatedAgency.Vehicles != null && MyAgency.AssiciatedAgency.Vehicles.Any())
                {
                    if (MyAgency.AssiciatedAgency.Vehicles.Any(x => x.ModelName == CopCar.Model.Name.ToLower()))
                    {
                        ZoneAgency = MyAgency.AssiciatedAgency;
                        break;
                    }
                }
            }
            if (ZoneAgency != null && ZoneAgency.Vehicles != null)
            {
                MyVehicle = ZoneAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
            }
        }
        if (MyVehicle == null || MyVehicle.Liveries == null || !MyVehicle.Liveries.Any())
        {
            ChangeToDefaultLivery(CopCar);
            return;
        }

        int LiveryNumber = NativeFunction.CallByName<int>("GET_VEHICLE_LIVERY", CopCar);
        int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
        NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        if(ZoneAgency != null)
        {
            CopCar.LicensePlate = ZoneAgency.LicensePlatePrefix + LosSantosRED.RandomString(8 - ZoneAgency.LicensePlatePrefix.Length);
        }
        
    }

    public static void ChangeToDefaultLivery(Vehicle CopCar)
    {
        Agency.VehicleInformation MyVehicle = LSPD.GetVehicleInfo(CopCar);
        if (MyVehicle == null)
            MyVehicle = LSSD.GetVehicleInfo(CopCar);

        if (MyVehicle == null)
            return;

        int LiveryNumber = NativeFunction.CallByName<int>("GET_VEHICLE_LIVERY", CopCar);
        int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
        NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
    }
    private static Agency GetAgencyFromSwat(Ped Cop)
    {
        return NOOSE;
    }
    private static Agency GetAgencyFromSecurity(Ped Cop)
    {
        Zone PedZone = Zones.GetZoneAtLocation(Cop.Position);
        if (PedZone != null && PedZone.MainZoneAgency.AgencyClassification == Agency.Classification.Security)//only other that uses security peds
        {
            return PedZone.MainZoneAgency;
        }
        else
        {
            return PRISEC;
        }
    }
 }
public class Agency
{
    public string ColorPrefix = "~s~";
    public string Initials;
    public string FullName;
    public List<ModelInformation> CopModels;
    public List<VehicleInformation> Vehicles;
    public Color AgencyColor = Color.White;
    public bool IsVanilla = false;
    public Classification AgencyClassification;
    public bool CanSpawnAmbient = false;
    public string LicensePlatePrefix;
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
        Security = 3,
        Other = 4,
    }
    public VehicleInformation GetVehicleInfo(Vehicle CopCar)
    {
        return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
    }
    public VehicleInformation GetRandomVehicle(bool IsMotorcycle)
    {
        if (Vehicles == null || !Vehicles.Any())
            return null;
        int Total = Vehicles.Where(x => x.IsMotorcycle == IsMotorcycle).Sum(x => x.SpawnChance);
        int RandomPick = LosSantosRED.MyRand.Next(0, Total);
        foreach (VehicleInformation Vehicle in Vehicles.Where(x => x.IsMotorcycle == IsMotorcycle))
        {
            if (RandomPick < Vehicle.SpawnChance)
            {
                return Vehicle;
            }
            RandomPick -= Vehicle.SpawnChance;
        }
        return null;
    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, Color _AgencyColor, Classification _AgencyClassification, bool _IsVanilla,bool _CanSpawnAmbient, List<ModelInformation> _CopModels, List<VehicleInformation> _Vehicles,string _LicensePlatePrefix)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColor = _AgencyColor;
        IsVanilla = _IsVanilla;
        Vehicles = _Vehicles;
        AgencyClassification = _AgencyClassification;
        CanSpawnAmbient = _CanSpawnAmbient;
        LicensePlatePrefix = _LicensePlatePrefix;
    }
    public class ModelInformation
    {
        public string ModelName;
        public bool isMale = true;
        public bool UseForRandomSpawn = true;
        public bool IsVanilla = true;

        public ModelInformation(string _ModelName,bool _isMale)
        {
            ModelName = _ModelName;
            isMale = _isMale;
        }
        public ModelInformation(string _ModelName, bool _isMale,bool _UseForRandomSpawn)
        {
            ModelName = _ModelName;
            isMale = _isMale;
            UseForRandomSpawn = _UseForRandomSpawn;
        }
    }
    public class VehicleInformation
    {
        public string ModelName;
        public bool UseForRandomSpawn = true;
        public bool IsVanilla = true;
        public int SpawnChance;
        public bool IsMotorcycle = false;
        public List<int> Liveries = new List<int>();
        public VehicleInformation(string modelName)
        {
            ModelName = modelName;
        }
        public VehicleInformation(string modelName, bool useForRandomSpawn) : this(modelName)
        {
            UseForRandomSpawn = useForRandomSpawn;
        }
        public VehicleInformation(string modelName, bool useForRandomSpawn, bool isVanilla) : this(modelName, useForRandomSpawn)
        {
            IsVanilla = isVanilla;
        }
        public VehicleInformation(string modelName, bool useForRandomSpawn, bool isVanilla, int spawnChance) : this(modelName, useForRandomSpawn, isVanilla)
        {
            SpawnChance = spawnChance;
        }
        public VehicleInformation(string modelName, bool useForRandomSpawn, bool isVanilla, int spawnChance, bool isMotorcycle) : this(modelName, useForRandomSpawn, isVanilla, spawnChance)
        {
            IsMotorcycle = isMotorcycle;
        }
        public VehicleInformation(string modelName, bool useForRandomSpawn, bool isVanilla, int spawnChance, bool isMotorcycle, List<int> _Liveries) : this(modelName, useForRandomSpawn, isVanilla, spawnChance)
        {
            IsMotorcycle = isMotorcycle;
            Liveries = _Liveries;
        }
    }
}

