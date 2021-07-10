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
    public GeneralSettings General { get; set; } = new GeneralSettings();
    public UISettings UI { get; set; } = new UISettings();
    public KeySettings KeyBinding { get; set; } = new KeySettings();
    public TrafficSettings TrafficViolations { get; set; } = new TrafficSettings();
    public PoliceSettings Police { get; set; } = new PoliceSettings();
    public RespawnSettings RespawnSettings { get; set; } = new RespawnSettings();
    public PlayerSettings PlayerSettings { get; set; } = new PlayerSettings();
}
