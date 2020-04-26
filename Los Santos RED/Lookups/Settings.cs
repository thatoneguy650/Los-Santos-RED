using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

[Serializable()]
public class Settings
{
    public GeneralSettings General = new GeneralSettings();
    public UISettings UI = new UISettings();
    public KeySettings KeyBinding = new KeySettings();
    public TrafficSettings TrafficViolations = new TrafficSettings();
    public PoliceSettings Police = new PoliceSettings();
    public string MainCharacterToAliasModelName
    {
        get
        {
            if (General.MainCharacterToAlias == "Michael")
                return "player_zero";
            else if (General.MainCharacterToAlias == "Franklin")
                return "player_one";
            else if (General.MainCharacterToAlias == "Trevor")
                return "player_two";
            else
                return "player_zero";
        }
    }
    public int IntReplacePlayerWithPedCharacter
    {
        get
        {
            switch (General.MainCharacterToAlias)
            {
                case "Michael":
                    return 0;
                case "Franklin":
                    return 1;
                case "Trevor":
                    return 2;
            }
            return 1;
        }
    }    
}

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
    public uint WantedLevelIncreaseTime = 180000;
    public uint WantedLevelInceaseOverTimeLimit = 4;
    public int PoliceBribeWantedLevelScale = 500;
    public int PoliceBailWantedLevelScale = 750;
    public int HospitalFee = 5000;
    public float LastWantedCenterSize = 400f;
    public int PoliceRecentlySeenTime = 25000;//15000//10000
    public bool AllowPoliceWeaponVariations = true;
    public bool DispatchAudio = true;
    public bool DispatchAudioOnlyHighPriority = false;
    public float DispatchAudioVolume = 0.4f;
    public bool DispatchSubtitles = false;
    public bool DispatchNotifications = true;
    public bool DisableAmbientScanner = true;
    public PoliceSettings()
    {

    }
}

public class GeneralSettings
{
    public bool AliasPedAsMainCharacter = true;
    public string MainCharacterToAlias = "Michael";
    public bool Keanu = true;
    public int UndieLimit = 0;
    public bool Debug = false; 
    public bool PedTakeoverSetRandomMoney = true;
    public int PedTakeoverRandomMoneyMin = 500;
    public int PedTakeoverRandomMoneyMax = 5000;
    public bool AlwaysShowCash = true;
    public bool Logging = true;
    public bool AllowDeathMenus = true;
    public bool AlwaysShowRadar = true;
    public bool AlwaysShowHUD = true;
    public GeneralSettings()
    {

    }

}

public class UISettings
{
    public bool Enabled = true;
    public float PositionX = 0.92f;
    public float PositionY = 0.16f;
    public float Spacing = 0.02f;
    public float Scale = 0.35f;

    public UISettings()
    {

    }
}

public class KeySettings
{
    public Keys MenuKey = Keys.F10;
    public Keys SurrenderKey = Keys.E;
    public Keys DropWeaponKey = Keys.G;
    public KeySettings()
    {

    }
}

public class TrafficSettings
{
    public bool Enabled = true;
    public bool ExemptCode3 = true;
    public bool Speeding = true;
    public float SpeedingOverLimitThreshold = 25f;
    public bool HitPed = true;
    public bool HitVehicle = true;
    public bool DrivingAgainstTraffic = true;
    public bool DrivingOnPavement = true;
    public bool NotRoadworthy = true;
    public bool RunningRedLight = true;

    public TrafficSettings()
    {

    }
}
