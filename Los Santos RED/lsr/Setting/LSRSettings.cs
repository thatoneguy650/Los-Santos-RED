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
public class LSRSettings
{
    public GeneralSettings General = new GeneralSettings();
    public UISettings UI = new UISettings();
    public KeySettings KeyBinding = new KeySettings();
    public TrafficSettings TrafficViolations = new TrafficSettings();
    public PoliceSettings Police = new PoliceSettings();
}
