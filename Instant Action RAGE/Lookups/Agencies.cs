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
        AgenciesList = new List<Agency>();

        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Department", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false), new Agency.ModelInformation("s_m_y_swat_01", true,false) }, Color.Blue,true);
        LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_sheriff_01", true), new Agency.ModelInformation("s_f_y_sheriff_01",false) }, Color.Red, true);
        SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_ranger_01", true), new Agency.ModelInformation("s_f_y_ranger_01",false) }, Color.Green, true);
        DOA = new Agency("~p~", "DOA", "Drug Observation Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("u_m_m_doa_01", true) }, Color.Purple, true);
        FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_fibsec_01", true) }, Color.Purple, true);
        IAA = new Agency("~p~", "IAA", "International Affairs Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_ciasec_01", true) }, Color.Purple, true);
        SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_hwaycop_01", true) }, Color.Yellow, true);
        SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_prisguard_01", true) }, Color.Orange, true);
        ARMY = new Agency("~u~", "ARMY", "Army", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_armymech_01", true), new Agency.ModelInformation("s_m_m_marine_01", true), new Agency.ModelInformation("s_m_m_marine_02",true), new Agency.ModelInformation("s_m_y_marine_01",true)
                , new Agency.ModelInformation("s_m_y_marine_02",true), new Agency.ModelInformation("s_m_y_marine_03",true), new Agency.ModelInformation("s_m_m_pilot_02",true), new Agency.ModelInformation("s_m_y_pilot_01",true) }, Color.Black, true);
        UNK = new Agency("~s~", "UNK", "Unknown Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("",true) }, Color.White, true);
        PRISEC = new Agency("~HUD_COLOUR_ORANGELIGHT~", "PRISEC", "Private Security", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_security_01", true) }, Color.White, true);
        LSPA = new Agency("~HUD_COLOUR_PURPLEDARK~", "LSPA", "Port Authority of Los Santos", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_security_01", true) }, Color.LightGray, true);
        LSIAPD = new Agency("~HUD_COLOUR_PURPLELIGHT~", "LSIAPD", "Los Santos International Airport Police Department", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) }, Color.LightBlue, true);
        BCSO = new Agency("~HUD_COLOUR_REDDARK~", "BCSO", "Blaine County Sheriffs Office", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_sheriff_01", true), new Agency.ModelInformation("s_f_y_sheriff_01", false) }, Color.Gray,true);

        VPPD = new Agency("~HUD_COLOUR_BLUEDARK~", "VPD", "Vespucci Police Department", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) }, Color.Blue, true);
        VWPD = new Agency("~HUD_COLOUR_BLUE~", "VWPD", "Vinewood Police Department", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) }, Color.Blue, true);
        RHPD = new Agency("~HUD_COLOUR_BLUELIGHT~", "RHPD", "Rockford Hills Police Department", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) }, Color.Blue, true);
        SACG = new Agency("~o~", "SACG", "San Andreas Coast Guard", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false) }, Color.Blue, true);

        LSPD.UsesLSPDVehicles = true;
        LSSD.UsesLSSDVehicles = true;
        VPPD.UsesLSPDVehicles = true;
        VWPD.UsesLSPDVehicles = true;
        RHPD.UsesLSPDVehicles = true;

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
        if (!Cop.isPoliceArmy())
            return UNK;
        if (Cop.isArmy())
            return ARMY;
        else if (Cop.isPolice())
        {
            Agency ToReturn;
            if (Cop.Model.Name.ToLower() == "s_m_y_cop_01" || Cop.Model.Name.ToLower() == "s_f_y_cop_01")//Swat depends on unit insignias
            {
                ToReturn = GetAgencyFromVanillaCop(Cop);
            }
            else if (Cop.Model.Name.ToLower() == "s_m_y_sheriff_01" || Cop.Model.Name.ToLower() == "s_f_y_sheriff_01")//Swat depends on unit insignias
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
        if (PedZone != null)
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
        if (PedZone != null)
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
    public Color AgencyColor = Color.White;
    public bool IsVanilla = false;
    public bool CanCheckTrafficViolations = true;
    public bool UsesLSPDVehicles = false;
    public bool UsesLSSDVehicles = false;
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
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
    }
}
