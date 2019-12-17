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
    public static Agency VWPD;
    public static Agency RHPD;
    public static Agency SACG;
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
        
        List<Agency.VehicleInformation> StandardPoliceVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("police", true, true, 25), new Agency.VehicleInformation("police2", true, true, 25), new Agency.VehicleInformation("police3", true, true, 25), new Agency.VehicleInformation("police4", true, true, 10), new Agency.VehicleInformation("fbi2", true, true, 15) };
        List<Agency.VehicleInformation> StandardSheriffVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("sheriff", true, true, 50), new Agency.VehicleInformation("sheriff2", true, true, 50) };
        List<Agency.VehicleInformation> UnmarkedVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("police4", true, true, 100) };
        List<Agency.VehicleInformation> ParkRangerVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("pranger", true, true, 100) };
        List<Agency.VehicleInformation> FIBVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("fbi", true, true, 70), new Agency.VehicleInformation("fbi2", true, true, 30) };
        List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("policeb", true, true, 70,true), new Agency.VehicleInformation("police4", true, true, 30) };
        List<Agency.VehicleInformation> PrisonVehicles = new List<Agency.VehicleInformation>() { new Agency.VehicleInformation("policet", true, true, 70), new Agency.VehicleInformation("police4", true, true, 30) };


        AgenciesList = new List<Agency>();
        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Department", Color.Blue, true, StandardCops, StandardPoliceVehicles);
        LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", Color.Red, true, SheriffPeds, StandardSheriffVehicles);
        SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", Color.Green, true, ParkRangers, ParkRangerVehicles);
        DOA = new Agency("~p~", "DOA", "Drug Observation Agency", Color.Purple, true, DOAPeds,UnmarkedVehicles);
        FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", Color.Purple, true,FIBPeds,FIBVehicles);
        IAA = new Agency("~p~", "IAA", "International Affairs Agency", Color.Purple, true,IAAPeds,UnmarkedVehicles);
        SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", Color.Yellow, true, SAHPPeds, HighwayPatrolVehicles);
        SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", Color.Orange, true,PrisonPeds,PrisonVehicles);
        ARMY = new Agency("~u~", "ARMY", "Army", Color.Black, true,MilitaryPeds,null);
        UNK = new Agency("~s~", "UNK", "Unknown Agency", Color.White, true);
        PRISEC = new Agency("~HUD_COLOUR_ORANGELIGHT~", "PRISEC", "Private Security", Color.White, true, SecurityPeds,UnmarkedVehicles);
        LSPA = new Agency("~HUD_COLOUR_PURPLEDARK~", "LSPA", "Port Authority of Los Santos", Color.LightGray, true, SecurityPeds,UnmarkedVehicles);
        LSIAPD = new Agency("~HUD_COLOUR_PURPLELIGHT~", "LSIAPD", "Los Santos International Airport Police Department", Color.LightBlue, true, StandardCops, StandardPoliceVehicles);
        BCSO = new Agency("~HUD_COLOUR_REDDARK~", "BCSO", "Blaine County Sheriffs Office", Color.DarkRed, true, SheriffPeds, StandardSheriffVehicles);
        VPPD = new Agency("~HUD_COLOUR_BLUEDARK~", "VPPD", "Vespucci Police Department", Color.DarkBlue, true, StandardCops, StandardPoliceVehicles);
        VWPD = new Agency("~HUD_COLOUR_BLUE~", "VWPD", "Vinewood Police Department", Color.Blue, true, StandardCops, StandardPoliceVehicles);
        RHPD = new Agency("~HUD_COLOUR_BLUELIGHT~", "RHPD", "Rockford Hills Police Department", Color.LightBlue, true, StandardCops, StandardPoliceVehicles);
        SACG = new Agency("~o~", "SACG", "San Andreas Coast Guard", Color.DarkOrange, true, CoastGuardPeds,UnmarkedVehicles);

        PRISEC.CanCheckTrafficViolations = false;
        UNK.CanCheckTrafficViolations = false;

        AgenciesList.Add(LSPD);
        AgenciesList.Add(LSSD);
        AgenciesList.Add(SAPR);
        AgenciesList.Add(DOA);
        AgenciesList.Add(FIB);
        AgenciesList.Add(IAA);
        AgenciesList.Add(SAHP);
        AgenciesList.Add(SASPA);
        AgenciesList.Add(ARMY);
        AgenciesList.Add(UNK);
        AgenciesList.Add(PRISEC);
        AgenciesList.Add(LSPA);
        AgenciesList.Add(LSIAPD);
        AgenciesList.Add(BCSO);
        AgenciesList.Add(VPPD);
        AgenciesList.Add(VWPD);
        AgenciesList.Add(RHPD);
        AgenciesList.Add(SACG);
    }
    public static void Dispose()
    {

    }
    public static Agency GetAgencyFromPed(Ped Cop,bool OnlyVanilla)
    {
        if (!Cop.IsPoliceArmy())
            return UNK;
        if (Cop.IsArmy())
            return ARMY;
        else if (Cop.IsPolice())
        {
            Agency ToReturn;
            if (Cop.Model.Name.ToLower() == "s_m_y_cop_01" || Cop.Model.Name.ToLower() == "s_f_y_cop_01")//Pick based on zone
            {
                ToReturn = GetAgencyFromVanillaCop(Cop);
            }
            else if (Cop.Model.Name.ToLower() == "s_m_y_sheriff_01" || Cop.Model.Name.ToLower() == "s_f_y_sheriff_01")//pick based on zone
            {
                ToReturn = GetAgencyFromVanillaSheriff(Cop);
            }
            else if (Cop.Model.Name.ToLower() == "s_m_y_swat_01")//Swat depends on unit insignias
             {
                ToReturn = GetAgencyFromSwat(Cop);
            }
            else if (Cop.Model.Name.ToLower() == "s_m_m_security_01")//Security depends on where they are
            {
                ToReturn = GetAgencyFromSecurity(Cop);
            }
            else
            {
                ToReturn = AgenciesList.Where(p => p.IsVanilla == OnlyVanilla && p.CopModels.Any(c => c.ModelName == Cop.Model.Name.ToLower())).FirstOrDefault();
            }
            if (ToReturn == null)
                return LSPD;
            else
                return ToReturn;
        }
        else
            return null;
    }
    private static Agency GetAgencyFromVanillaSheriff(Ped Cop)
    {
        Zone PedZone = Zones.GetZoneAtLocation(Cop.Position);
        if (PedZone != null && PedZone.MainZoneAgency == BCSO)
        {
            return PedZone.MainZoneAgency;
        }
        else
        {
            return LSSD;
        }
    }
    private static Agency GetAgencyFromVanillaCop(Ped Cop)
    {
        Zone PedZone = Zones.GetZoneAtLocation(Cop.Position);
        if (PedZone != null && PedZone.MainZoneAgency.UsesLSPDVehicles)
        {
            return PedZone.MainZoneAgency;
        }
        else
        {
            return LSPD;
        }
    }
    private static Agency GetAgencyFromSwat(Ped Cop)
    {
        if(InstantAction.PlayerWantedLevel >= 5)
        {
            NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 10, 0, 1, 0);//Set them as FIB
        }
        int TextureVariation = NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", Cop, 10);
        if (TextureVariation == 0)
            return LSPD;
        else
            return FIB;
    }
    private static Agency GetAgencyFromSecurity(Ped Cop)
    {
        Zone PedZone = Zones.GetZoneAtLocation(Cop.Position);
        if (PedZone != null)
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
    public bool CanCheckTrafficViolations = true;
    public bool UsesLSPDVehicles = false;
    public bool UsesLSSDVehicles = false;
    private static readonly Random rnd;
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
    }
    public VehicleInformation GetRandomVehicle(bool IsMotorcycle)
    {
        int Total = Vehicles.Where(x => x.IsMotorcycle == IsMotorcycle).Sum(x => x.SpawnChance);
        int RandomPick = InstantAction.rnd.Next(0, Total);
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
    public Agency(string _ColorPrefix, string _Initials, string _FullName, Color _AgencyColor, bool _IsVanilla)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        AgencyColor = _AgencyColor;
        IsVanilla = _IsVanilla;
    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, List<ModelInformation> _CopModels, Color _AgencyColor,bool _IsVanilla)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColor = _AgencyColor;
        IsVanilla = _IsVanilla;
    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, Color _AgencyColor, bool _IsVanilla, List<ModelInformation> _CopModels, List<VehicleInformation> _Vehicles)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColor = _AgencyColor;
        IsVanilla = _IsVanilla;
        Vehicles = _Vehicles;
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
    }
}
