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
public class SettingsManager
{
    public GeneralSettings GeneralSettings { get; set; } = new GeneralSettings();
    public UISettings UISettings { get; set; } = new UISettings();
    public KeySettings KeySettings { get; set; } = new KeySettings();
    public PoliceSettings PoliceSettings { get; set; } = new PoliceSettings();
    public RespawnSettings RespawnSettings { get; set; } = new RespawnSettings();
    public PlayerSettings PlayerSettings { get; set; } = new PlayerSettings();
}
