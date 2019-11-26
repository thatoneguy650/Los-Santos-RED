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
        LSPD = new Agency("~b~", "LSPD", "Los Santos Police Deptartment", new List<string>() { "s_m_y_cop_01", "s_f_y_cop_01", "s_m_y_swat_01" }, System.Drawing.Color.Blue);
        LSSD = new Agency("~r~", "LSSD", "Los Santos County Sheriff", new List<string>() { "s_m_y_sheriff_01", "s_f_y_sheriff_01" }, System.Drawing.Color.Red);
        SAPR = new Agency("~g~", "SAPR", "San Andreas Park Ranger", new List<string>() { "s_m_y_ranger_01", "s_f_y_ranger_01" }, System.Drawing.Color.Green);
        DOA = new Agency("~p~", "DOA", "Drug Observation Agency", new List<string>() { "u_m_m_doa_01" }, System.Drawing.Color.Purple);
        FIB = new Agency("~p~", "FIB", "Federal Investigation Bureau", new List<string>() { "s_m_m_fibsec_01" }, System.Drawing.Color.Purple);
        IAA = new Agency("~p~", "IAA", "International Affairs Agency", new List<string>() { "s_m_m_ciasec_01" }, System.Drawing.Color.Purple);
        SAHP = new Agency("~y~", "SAHP", "San Andreas Highway Patrol", new List<string>() { "s_m_y_hwaycop_01" }, System.Drawing.Color.Yellow);
        SASPA = new Agency("~o~", "SASPA", "San Andreas State Prison Authority", new List<string>() { "s_m_m_prisguard_01" }, System.Drawing.Color.Orange);
        ARMY = new Agency("~u~", "ARMY", "Army", new List<string>() { "s_m_y_armymech_01", "s_m_m_marine_01", "s_m_m_marine_02", "s_m_y_marine_01", "s_m_y_marine_02", "s_m_y_marine_03", "s_m_m_pilot_02", "s_m_y_pilot_01" }, System.Drawing.Color.Black);
        UNK = new Agency("~s~", "UNK", "Unknown Agency", new List<string>() { "" }, System.Drawing.Color.White);
        PRISEC = new Agency("~s~", "PRISEC", "Private Security", new List<string>() { "s_m_m_security_01" }, System.Drawing.Color.White);
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
    public static Agency GetAgencyFromPed(Ped Cop)
    {
        if (!Cop.isPoliceArmy())
            return Agencies.UNK;
        if (Cop.isArmy())
            return Agencies.ARMY;
        else if (Cop.isPolice())
        {
            Agency ToReturn = Agencies.AgenciesList.Where(x => x.Models.Contains(Cop.Model.Name.ToLower())).FirstOrDefault();
            if (ToReturn == null)
                return Agencies.LSPD;
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
    public Color AgencyColor = Color.White;
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
}
