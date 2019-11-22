using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

static class Settings
{
    public static bool AliasPedAsMainCharacter = true;
    public static string MainCharacterToAlias = "Michael";

    public static int UndieLimit = 0;
    public static bool Debug = false;
    public static Keys MenuKey = Keys.F10;
    public static Keys SurrenderKey = Keys.E;
    public static Keys DropWeaponKey = Keys.G;
    public static float DispatchAudioVolume = 0.4f;

    public static bool TrafficViolations = true;
    public static bool TrafficViolationsExemptCode3 = true;
    public static bool TrafficViolationsSpeeding = true;
    public static float TrafficViolationsSpeedingOverLimitThreshold = 25f;

    public static bool TrafficViolationsHitPed = true;
    public static bool TrafficViolationsHitVehicle = true;
    public static bool TrafficViolationsDrivingAgainstTraffic = true;
    public static bool TrafficViolationsDrivingOnPavement = true;
    public static bool TrafficViolationsNotRoadworthy = true;

    public static bool TrafficViolationsUI = true;
    public static bool TrafficViolationsUIOnlyWhenSpeeding = false;


    public static bool SpawnPoliceK9 = true;
    public static bool SpawnRandomPolice = true;
    public static int SpawnRandomPoliceLimit = 5;
    public static bool SpawnedRandomPoliceHaveBlip = true;

    public static bool IssuePoliceHeavyWeapons = true;

    public static int PoliceKilledSurrenderLimit = 5;

    public static bool OverridePoliceAccuracy = true;
    public static int PoliceGeneralAccuracy = 10;
    public static int PoliceTazerAccuracy = 30;
    public static int PoliceHeavyAccuracy = 10;

    public static bool SpawnNewsChopper = true;
    public static bool WantedLevelIncreasesOverTime = true;
    public static uint WantedLevelIncreaseTime = 180000;
    public static uint WantedLevelInceaseOverTimeLimit = 4;

    public static int PoliceBribeWantedLevelScale = 500;
    public static int PoliceBailWantedLevelScale = 750;
    public static int HospitalFee = 5000;

    public static bool PedTakeoverSetRandomMoney = true;
    public static int PedTakeoverRandomMoneyMin = 500;
    public static int PedTakeoverRandomMoneyMax = 5000;
    public static bool DispatchAudio = true;
    public static bool DispatchAudioOnlyHighPriority = false;
    public static int PoliceRecentlySeenTime = 10000;
    public static bool DisableAmbientScanner = true;
    public static bool WantedMusicDisable = true;
    public static bool AlwaysShowHUD = true;
    public static bool ShowPoliceRadarBlips = false;
    public static bool AlwaysShowCash = true;
    public static bool Keanu = true;
    public static bool AllowPoliceWeaponVariations = true;
    public static bool TrafficInfoUI = true;
    public static float TrafficInfoUIPositionX = 0.92f;
    public static float TrafficInfoUIPositionY = 0.16f;
    public static float TrafficInfoUISpacing = 0.02f;
    public static float TrafficInfoUIScale = 0.35f;
    public static bool DispatchSubtitles = false;
    public static bool Logging = true;

    public static string MainCharacterToAliasModelName
    {
        get
        {
            if (MainCharacterToAlias == "Michael")
                return "player_zero";
            else if (MainCharacterToAlias == "Franklin")
                return "player_one";
            else if (MainCharacterToAlias == "Trevor")
                return "player_two";
            else
                return "player_zero";
        }
    }
    public static int IntReplacePlayerWithPedCharacter
    {
        get
        {
            switch (MainCharacterToAlias)
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
    public static void Initialize()
    {
        ReadSettings();
    }
    public static void ReadSettings()
    {
        try
        {
            if (File.Exists("Plugins\\InstantAction\\InstantAction.xml"))
            { 
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("Plugins\\InstantAction\\InstantAction.xml");
                XmlElement documentElement = xmlDocument.DocumentElement;
                FieldInfo[] myFields = Type.GetType("Settings", false).GetFields();          
                foreach(XmlNode myNode in documentElement.ChildNodes)
                {
                    FieldInfo MyField = myFields.Where(x => x.Name == myNode.Name).FirstOrDefault();
                    if (MyField == null)
                        continue;
                    if (MyField.FieldType == typeof(bool))
                    {
                        MyField.SetValue(null, bool.Parse(myNode.InnerText));
                    }
                    else if (MyField.FieldType == typeof(int))
                    {
                        MyField.SetValue(null, int.Parse(myNode.InnerText));
                    }
                    else if (MyField.FieldType == typeof(float))
                    {
                        MyField.SetValue(null, float.Parse(myNode.InnerText));
                    }
                    else if (MyField.FieldType == typeof(Keys))
                    {
                        MyField.SetValue(null, (Keys)Enum.Parse(typeof(Keys), myNode.InnerText, true));
                    }
                    else
                    {
                        MyField.SetValue(null, myNode.InnerText);
                    }
                }

            }
            else
            {
                CreateSettingsFile();
            }
        }
        catch (Exception e)
        {
            InstantAction.WriteToLog("ReadSettings", e.Message.ToString());
        }
    }
    public static void WriteSettings()
    {
        try
        {
            if (File.Exists("Plugins\\InstantAction\\InstantAction.xml"))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("Plugins\\InstantAction\\InstantAction.xml");

                foreach (FieldInfo fi in Type.GetType("Settings", false).GetFields())
                {
                    XmlNode xmlNode = xmlDocument.SelectSingleNode("//" + fi.Name);
                    xmlNode.InnerText = fi.GetValue(null).ToString();
                }
                xmlDocument.Save("Plugins\\InstantAction\\InstantAction.xml");
            }
            else
            {
                CreateSettingsFile();
            }
        }
        catch (Exception e2)
        {
            InstantAction.WriteToLog("WriteSettings", e2.Message);
        }
    }
    public static void CreateSettingsFile()
    {
        XmlDocument xmlDocument = new XmlDocument();
        XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement root = xmlDocument.DocumentElement;
        xmlDocument.InsertBefore(xmlDeclaration, root);

        XmlElement element1 = xmlDocument.CreateElement(string.Empty, "InstantAction", string.Empty);
        xmlDocument.AppendChild(element1);
        foreach (FieldInfo fi in Type.GetType("Settings", false).GetFields())
        {
            XmlElement NewElement = xmlDocument.CreateElement(string.Empty, fi.Name, string.Empty);
            NewElement.AppendChild(xmlDocument.CreateTextNode(fi.GetValue(null).ToString()));
            xmlDocument.DocumentElement.AppendChild(NewElement);
        }
        xmlDocument.Save("Plugins\\InstantAction\\InstantAction.xml");
    }
}

