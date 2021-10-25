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
    public PlayerSettings PlayerSettings { get; set; } = new PlayerSettings();
    public RespawnSettings RespawnSettings { get; set; } = new RespawnSettings();
    public UISettings UISettings { get; set; } = new UISettings();
    public KeySettings KeySettings { get; set; } = new KeySettings();
    public PoliceSettings PoliceSettings { get; set; } = new PoliceSettings();
    public EMSSettings EMSSettings { get; set; } = new EMSSettings();
    public FireSettings FireSettings { get; set; } = new FireSettings();
    public DamageSettings DamageSettings { get; set; } = new DamageSettings();
    public PedSwapSettings PedSwapSettings { get; set; } = new PedSwapSettings();
    public CivilianSettings CivilianSettings { get; set; } = new CivilianSettings();
    public ActivitySettings ActivitySettings { get; set; } = new ActivitySettings();
    public WorldSettings WorldSettings { get; set; } = new WorldSettings();
    public TimeSettings TimeSettings { get; set; } = new TimeSettings();
    public VanillaSettings VanillaSettings { get; set; } = new VanillaSettings();
    public DebugSettings DebugSettings { get; set; } = new DebugSettings();
}
