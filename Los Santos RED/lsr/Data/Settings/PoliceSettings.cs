using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings
{
    public bool SpawnPoliceK9 = true;
    public bool SpawnRandomPolice = true;
    public int SpawnAmbientPoliceLimit = 10;
    public bool SpawnedAmbientPoliceHaveBlip = true;
    public bool WantedMusicDisable = true;
    public bool ShowPoliceRadarBlips = false;
    public bool IssuePoliceHeavyWeapons = true;
    public int PoliceKilledSurrenderLimit = 2;
    public bool OverridePoliceAccuracy = true;
    public int PoliceGeneralAccuracy = 10;
    public int PoliceTazerAccuracy = 30;
    public int PoliceHeavyAccuracy = 10;
    public bool WantedLevelIncreasesOverTime = true;
    public uint WantedLevelIncreaseTime = 240000;
    public uint WantedLevelInceaseOverTimeLimit = 3;
    public int PoliceBribeWantedLevelScale = 500;
    public int PoliceBailWantedLevelScale = 750;
    public int HospitalFee = 5000;
    public float LastWantedCenterSize = 400f;
    public int PoliceRecentlySeenTime = 17000;//15000//10000
    public bool AllowPoliceWeaponVariations = true;
    public bool DispatchAudio = true;
    public bool DispatchAudioOnlyHighPriority = false;
    public int DispatchAudioVolume = 5;
    public bool DispatchSubtitles = false;
    public bool DispatchNotifications = true;
    public bool DisableAmbientScanner = true;

    public bool DebugShowPoliceTask = true;

    public int HelicopterLimit = 2;
    public int BoatLimit = 2;

    public PoliceSettings()
    {

    }
}