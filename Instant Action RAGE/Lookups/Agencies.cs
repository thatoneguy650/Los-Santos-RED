using ExtensionsMethods;
using Rage;
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
    public static void Initialize()
    {
        AgenciesList = new List<Agency>();
        //LSPD = new Agency("~b~", "LSPD", "Los Santos Police Deptartment", new List<string>() { "s_m_y_cop_01", "s_f_y_cop_01", "s_m_y_swat_01" }, Color.Blue);
        //LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", new List<string>() { "s_m_y_sheriff_01", "s_f_y_sheriff_01" }, Color.Red);
        //SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", new List<string>() { "s_m_y_ranger_01", "s_f_y_ranger_01" }, Color.Green);
        //DOA = new Agency("~q~", "DOA", "Drug Observation Agency", new List<string>() { "u_m_m_doa_01" }, Color.DeepPink);
        //FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", new List<string>() { "s_m_m_fibsec_01" }, Color.Purple);
        //IAA = new Agency("~p~", "IAA", "International Affairs Agency", new List<string>() { "s_m_m_ciasec_01" }, Color.Purple);
        //SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", new List<string>() { "s_m_y_hwaycop_01" }, Color.Yellow);
        //SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", new List<string>() { "s_m_m_prisguard_01" }, Color.Orange);
        //ARMY = new Agency("~u~", "ARMY", "Army", new List<string>() { "s_m_y_armymech_01", "s_m_m_marine_01", "s_m_m_marine_02", "s_m_y_marine_01", "s_m_y_marine_02", "s_m_y_marine_03", "s_m_m_pilot_02", "s_m_y_pilot_01" }, Color.Black);
        //UNK = new Agency("~s~", "UNK", "Unknown Agency", new List<string>() { "" }, Color.White);
        //PRISEC = new Agency("~s~", "PRISEC", "Private Security", new List<string>() { "s_m_m_security_01" }, Color.White);


        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Deptartment", new List<Agency.ModelInformation>() { new Agency.ModelInformation("s_m_y_cop_01", true), new Agency.ModelInformation("s_f_y_cop_01", false), new Agency.ModelInformation("s_m_y_swat_01", true) }, Color.Blue);
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
            //Agency ToReturn = AgenciesList.Where(x => x.CopModels.SelectMany(b => b.ModelName).Where(c => c.//Where(x => x.ModelName == Cop.Model.Name.ToLower()).ToList()).FirstOrDefault();//.Include(Cop.Model.Name.ToLower())).FirstOrDefault();
           // Agency ToReturn = AgenciesList.SelectMany(x => x.CopModels).SelectMany(c => c.ModelName == Cop.Model.Name.ToLower());

            Agency ToReturn = AgenciesList.Where(p => p.CopModels.Any(c => c.ModelName == Cop.Model.Name.ToLower())).FirstOrDefault();


            if (ToReturn == null)
                return LSPD;
            else
                return ToReturn;
        }
        else
            return null;
    }

}
public class Agency
{
    public string ColorPrefix = "~s~";
    public string Initials;
    public string FullName;
    public List<string> Models;
    public List<ModelInformation> CopModels;
    public Color AgencyColor = Color.White;
    private static Random rnd;

    static Agency()
    {
        rnd = new Random();
    }
    public Agency()
    {

    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, List<string> _Models, Color _AgencyColor)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        Models = _Models;
        AgencyColor = _AgencyColor;
    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, List<ModelInformation> _CopModels, Color _AgencyColor)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColor = _AgencyColor;
    }

    //public static ModelInformation GetRandomModel(List<ModelInformation> brokers, int totalWeight)
    //{
    //    // totalWeight is the sum of all brokers' weight
    //    int randomNumber = rnd.Next(0, totalWeight);

    //    ModelInformation selectedBroker = null;
    //    foreach (ModelInformation broker in brokers)
    //    {
    //        if (randomNumber <= broker.Weight)
    //        {
    //            selectedBroker = broker;
    //            break;
    //        }
    //        randomNumber = randomNumber - broker.Weight;
    //    }
    //    return selectedBroker;
    //}



    public class ModelInformation
    {
        public string ModelName;
        public bool isMale = true;
        public ModelInformation(string _ModelName,bool _isMale)
        {
            ModelName = _ModelName;
            isMale = _isMale;
        }
    }
}
