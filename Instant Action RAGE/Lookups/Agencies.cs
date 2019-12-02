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
    public static List<Agency> AgenciesList { get; private set; } = new List<Agency>();
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
    public static void Initialize()
    {
        AgenciesList = new List<Agency>();

        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Deptartment", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false), new Agency.ModelInformation("s_m_y_swat_01", true,false) }, Color.Blue);
        LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_sheriff_01", true), new Agency.ModelInformation("s_f_y_sheriff_01",false) }, Color.Red);
        SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_ranger_01", true), new Agency.ModelInformation("s_f_y_ranger_01",false) }, Color.Green);
        DOA = new Agency("~q~", "DOA", "Drug Observation Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("u_m_m_doa_01", true) }, Color.DeepPink);
        FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_fibsec_01", true) }, Color.Purple);
        IAA = new Agency("~p~", "IAA", "International Affairs Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_ciasec_01", true) }, Color.Purple);
        SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_hwaycop_01", true) }, Color.Yellow);
        SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_prisguard_01", true) }, Color.Orange);
        ARMY = new Agency("~u~", "ARMY", "Army", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_armymech_01", true), new Agency.ModelInformation("s_m_m_marine_01", true), new Agency.ModelInformation("s_m_m_marine_02",true), new Agency.ModelInformation("s_m_y_marine_01",true)
                , new Agency.ModelInformation("s_m_y_marine_02",true), new Agency.ModelInformation("s_m_y_marine_03",true), new Agency.ModelInformation("s_m_m_pilot_02",true), new Agency.ModelInformation("s_m_y_pilot_01",true) }, Color.Black);
        UNK = new Agency("~s~", "UNK", "Unknown Agency", new List<Agency.ModelInformation>() { new Agency.ModelInformation("",true) }, Color.White);
        PRISEC = new Agency("~s~", "PRISEC", "Private Security", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_security_01", true) }, Color.White);
        LSPA = new Agency("~c~", "LSPA", "Port Authority of Los Santos", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_m_security_01", true) }, Color.LightGray);

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
    }
    public static void Dispose()
    {

    }
    public static Agency GetAgencyFromPed(Ped Cop)
    {
        if (!Cop.isPoliceArmy())
            return UNK;
        if (Cop.isArmy())
            return ARMY;
        else if (Cop.isPolice())
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
                ToReturn = AgenciesList.Where(p => p.CopModels.Any(c => c.ModelName == Cop.Model.Name.ToLower())).FirstOrDefault();
            }
            if (ToReturn == null)
                return LSPD;
            else
                return ToReturn;
        }
        else
            return null;
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
        if (PedZone != null && PedZone.MainZoneAgency == LSPA)
        {
            return LSPA;
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
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, List<ModelInformation> _CopModels, Color _AgencyColor)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColor = _AgencyColor;
    }
    public class ModelInformation
    {
        public string ModelName;
        public bool isMale = true;
        public bool UseForRandomSpawn = true;
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
}
