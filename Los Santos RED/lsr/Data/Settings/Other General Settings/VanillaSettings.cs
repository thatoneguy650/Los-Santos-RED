using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VanillaSettings : ISettingsDefaultable
{

    [Description("Terminates the entire vanilla respawn system. REQUIRED FOR NORMAL MOD FUNCTIONS. May cause issues with Taxi dispatch.")]
    public bool TerminateRespawn { get; set; }
    public bool TerminateDispatch { get; set; }
    public bool TerminateHealthRecharge { get; set; }
    public bool TerminateWantedMusic { get; set; }
    public bool TerminateScanner { get; set; }
    public bool TerminateScenarioCops { get; set; }
    public bool SuppressVanillaGangPeds { get; set; }
    public bool TerminateScenarioPeds { get; set; }
    public bool TerminateRandomEvents { get; set; }
    public bool BlockGangScenarios { get; set; }
    public bool BlockVanillaPoliceCarGenerators { get; set; }
    public bool BlockVanillaPoliceScenarios { get; set; }
    public float BlockGangScenariosAroundDensDistance { get; set; }
    [Description("Terminates all vanilla shops (LS Customs, Ammunation, Tattoo, Vending Machines) and allows DLC vehicles to be spawned without an external trailer. CANNOT BE RE-ENABLED, REQUIRES GAME RESTART.")]
    public bool TerminateVanillaShops { get; set; }
    [Description("Terminates all vanilla blips. CANNOT BE RE-ENABLED, REQUIRES GAME RESTART.")]
    public bool TerminateVanillaBlips { get; set; }
    [Description("Terminates the vanilla character select system (Michael, Franklin, Trevor Wheel). WILL DISABLE THE ROCKSTAR EDITOR WHICH CANNOT BE RE-ENABLED WITHOUT A GAME RESTART.")]
    public bool TerminateSelector { get; set; }

    public VanillaSettings()
    {

        SetDefault();
    }
    public void SetDefault()
    {
        TerminateRespawn = true;
        TerminateDispatch = true;
        TerminateHealthRecharge = true;
        TerminateWantedMusic = true;
        TerminateScanner = true;
        TerminateScenarioCops = true;
        SuppressVanillaGangPeds = false;
        TerminateScenarioPeds = false;//gets rid of them ALLLLLLLLL, not driving gang members tho
        TerminateRandomEvents = true;
        BlockGangScenarios = true;
        BlockGangScenariosAroundDensDistance = 200f;
        BlockVanillaPoliceCarGenerators = true;
        BlockVanillaPoliceScenarios = true;
        TerminateVanillaShops = false;
        TerminateVanillaBlips = false;
        TerminateSelector = false;
    }

}