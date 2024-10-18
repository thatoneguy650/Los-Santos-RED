using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class VanillaSettings : ISettingsDefaultable
{

    [Description("Terminates the entire vanilla respawn system. REQUIRED FOR NORMAL MOD FUNCTIONS. May cause issues with Taxi dispatch.")]
    public bool TerminateRespawn { get; set; }
    [Description("Global ID for the vanilla respawn system. Requires a mod restart after changing. Currently 5, was changed from 4 in the Chop Shop Update. If you don't know what you are doing dont change this number.")]
    public int TerminateRespawnGlobalID { get; set; }
    public bool TerminateDispatch { get; set; }
    public bool TerminateHealthRecharge { get; set; }
    public bool TerminateWantedMusic { get; set; }
    public bool TerminateScanner { get; set; }
    public bool TerminateScenarioCops { get; set; }
    public bool SupressRandomPoliceEvents { get; set; }
    [Description("Terminates all vanilla shops (LS Customs, Ammunation, Tattoo, Vending Machines). No longer needed to stop r* DLC car despawning. CANNOT BE RE-ENABLED, REQUIRES GAME RESTART.")]
    public bool TerminateVanillaShops { get; set; }
    [Description("Terminates all vanilla blips. CANNOT BE RE-ENABLED, REQUIRES GAME RESTART.")]
    public bool TerminateVanillaBlips { get; set; }
    [Description("Terminates the vanilla character select system (Michael, Franklin, Trevor Wheel). WILL DISABLE THE ROCKSTAR EDITOR WHICH CANNOT BE RE-ENABLED WITHOUT A GAME RESTART.")]
    public bool TerminateSelector { get; set; }
    public bool SupressVanillaCopCrimes { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
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
        SupressRandomPoliceEvents = true;
        TerminateVanillaShops = false;
        TerminateVanillaBlips = false;
        TerminateSelector = false;
        SupressVanillaCopCrimes = true;
        TerminateRespawnGlobalID = 5;
    }

}