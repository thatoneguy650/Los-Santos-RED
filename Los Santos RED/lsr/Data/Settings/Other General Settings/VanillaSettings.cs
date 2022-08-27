using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VanillaSettings : ISettingsDefaultable
{


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
    public float BlockGangScenariosDistance { get; set; }
    [Description("Terminates all vanilla shops (LS Customs, Ammunation, Tattoo, Vending Machines) and allows DLC vehicles to be spawned without an external trailer. CANNOT BE RE-ENABLED, REQUIRES GAME RESTART.")]
    public bool TerminateVanillaShops { get; set; }

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
        TerminateRandomEvents = false;
        BlockGangScenarios = true;
        BlockGangScenariosDistance = 200f;
        BlockVanillaPoliceCarGenerators = true;
        BlockVanillaPoliceScenarios = true;
        TerminateVanillaShops = false;
    }

}