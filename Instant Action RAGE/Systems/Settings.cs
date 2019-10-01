using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

static class Settings
{
    public static bool AutoRespawn = false;
    public static bool BetterChasesAllowBustOportunity = true;
    public static bool ReplacePlayerWithPed = true;
    public static string ReplacePlayerWithPedCharacter = "Michael";
    public static int IntReplacePlayerWithPedCharacter
    {
        get
        {
            switch (ReplacePlayerWithPedCharacter)
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
    public static bool ReplacePlayerWithPedRandomMoney = true;
    public static bool PoliceEnhancements = true;
    public static int UndieLimit = 0;
    public static bool Debug = false;
    public static bool RandomEvents = false;
    public static Keys MenuKey = Keys.F10;
    public static Keys BetterChasesSurrenderKey = Keys.E;
    public static Keys DropWeaponKey = Keys.G;
    public static float DispatchAudioVolume = 0.4f;

    public static void Initialize()
    {
        ReadSettings();
    }
    public static string PrintSettings()
    {
        return "Settings:\n" +
               $"AutoRespawn: {AutoRespawn}\n" +
               $"ReplacePlayerWithPed: {ReplacePlayerWithPed}\n" +
               $"ReplacePlayerWithPedCharacter: {ReplacePlayerWithPedCharacter}\n" +
               $"ReplacePlayerWithPedRandomMoney: {ReplacePlayerWithPedRandomMoney}\n" +
               $"UndieLimit: {UndieLimit}\n" +
               $"Debug: {Debug}\n" +
               $"MenuKey: {MenuKey}\n" +
               $"DropWeaponKey: {DropWeaponKey}";
    }
    public static void ReadSettings()
    {
        if (File.Exists("Plugins\\InstantAction\\InstantAction.xml"))
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("Plugins\\InstantAction\\InstantAction.xml");
                XmlElement documentElement = xmlDocument.DocumentElement;
                MenuKey = (Keys)Enum.Parse(typeof(Keys), documentElement.SelectSingleNode("//MenuKey").InnerText, true);
                BetterChasesSurrenderKey = (Keys)Enum.Parse(typeof(Keys), documentElement.SelectSingleNode("//BustKey").InnerText, true);
                DropWeaponKey = (Keys)Enum.Parse(typeof(Keys), documentElement.SelectSingleNode("//DropWeaponKey").InnerText, true);
                AutoRespawn = bool.Parse(documentElement.SelectSingleNode("//AutoRespawn").InnerText);
                ReplacePlayerWithPed = bool.Parse(documentElement.SelectSingleNode("//ReplacePlayerWithPed").InnerText);
                ReplacePlayerWithPedCharacter = documentElement.SelectSingleNode("//ReplacePlayerWithPedCharacter").InnerText;
                ReplacePlayerWithPedRandomMoney = bool.Parse(documentElement.SelectSingleNode("//ReplacePlayerWithPedRandomMoney").InnerText);
                Debug = bool.Parse(documentElement.SelectSingleNode("//Debug").InnerText);
                RandomEvents = bool.Parse(documentElement.SelectSingleNode("//RandomEvents").InnerText);
                UndieLimit = int.Parse(documentElement.SelectSingleNode("//UndieLimit").InnerText);
            }
            catch (Exception e)
            {
                WriteToLog("ReadSettings", e.Message.ToString());
            }
        }
    }
    public static void WriteSettings(String Setting, String Value)
    {
        try
        {
            if (File.Exists("Plugins\\InstantAction\\InstantAction.xml"))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("Plugins\\InstantAction\\InstantAction.xml");
                XmlNode xmlNode = xmlDocument.SelectSingleNode("//" + Setting);
                xmlNode.InnerText = Value;
                xmlDocument.Save("Plugins\\InstantAction\\InstantAction.xml");
            }
            else
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = xmlDocument.DocumentElement;
                xmlDocument.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = xmlDocument.CreateElement(string.Empty, "InstantAction", string.Empty);
                xmlDocument.AppendChild(element1);

                XmlElement MenuKeyElement = xmlDocument.CreateElement(string.Empty, "MenuKey", string.Empty);
                MenuKeyElement.AppendChild(xmlDocument.CreateTextNode(MenuKey.ToString()));
                xmlDocument.DocumentElement.AppendChild(MenuKeyElement);


                XmlElement DropWeaponKeyElement = xmlDocument.CreateElement(string.Empty, "DropWeaponKey", string.Empty);
                DropWeaponKeyElement.AppendChild(xmlDocument.CreateTextNode(DropWeaponKey.ToString()));
                xmlDocument.DocumentElement.AppendChild(DropWeaponKeyElement);

                XmlElement AutoRespawnElement = xmlDocument.CreateElement(string.Empty, "AutoRespawn", string.Empty);
                AutoRespawnElement.AppendChild(xmlDocument.CreateTextNode(AutoRespawn.ToString()));
                xmlDocument.DocumentElement.AppendChild(AutoRespawnElement);

                XmlElement ReplacePlayerWithPedElement = xmlDocument.CreateElement(string.Empty, "ReplacePlayerWithPed", string.Empty);
                ReplacePlayerWithPedElement.AppendChild(xmlDocument.CreateTextNode(ReplacePlayerWithPed.ToString()));
                xmlDocument.DocumentElement.AppendChild(ReplacePlayerWithPedElement);

                XmlElement ReplacePlayerWithPedCharacterElement = xmlDocument.CreateElement(string.Empty, "ReplacePlayerWithPedCharacter", string.Empty);
                ReplacePlayerWithPedCharacterElement.AppendChild(xmlDocument.CreateTextNode(ReplacePlayerWithPedCharacter.ToString()));
                xmlDocument.DocumentElement.AppendChild(ReplacePlayerWithPedCharacterElement);

                XmlElement ReplacePlayerWithPedRandomMoneyElement = xmlDocument.CreateElement(string.Empty, "ReplacePlayerWithPedRandomMoney", string.Empty);
                ReplacePlayerWithPedRandomMoneyElement.AppendChild(xmlDocument.CreateTextNode(ReplacePlayerWithPedRandomMoney.ToString()));
                xmlDocument.DocumentElement.AppendChild(ReplacePlayerWithPedRandomMoneyElement);


                XmlElement DebugElement = xmlDocument.CreateElement(string.Empty, "Debug", string.Empty);
                DebugElement.AppendChild(xmlDocument.CreateTextNode(Debug.ToString()));
                xmlDocument.DocumentElement.AppendChild(DebugElement);


                XmlElement UndieLimitElement = xmlDocument.CreateElement(string.Empty, "UndieLimit", string.Empty);
                UndieLimitElement.AppendChild(xmlDocument.CreateTextNode(UndieLimit.ToString()));
                xmlDocument.DocumentElement.AppendChild(UndieLimitElement);

                xmlDocument.Save("scripts\\InstantAction\\InstantAction.xml");

            }
        }
        catch (Exception e2)
        {
            WriteToLog("WriteSettings", e2.Message);
        }
    }
    private static void WriteToLog(String ProcedureString, String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        sb.Clear();
    }
}

