using System;
using System.ComponentModel;
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
    [Category("Player")]
    public RespawnSettings RespawnSettings { get; set; } = new RespawnSettings();
    [Category("Player")]
    public VehicleSettings VehicleSettings { get; set; } = new VehicleSettings();
    [Category("Player")]
    public PedSwapSettings PedSwapSettings { get; set; } = new PedSwapSettings();
    [Category("Player")]
    public ActivitySettings ActivitySettings { get; set; } = new ActivitySettings();
    [Category("Player")]
    public SprintSettings SprintSettings { get; set; } = new SprintSettings();
    [Category("Player")]
    public ViolationSettings ViolationSettings { get; set; } = new ViolationSettings();
    [Category("Player")]
    public RecoilSettings RecoilSettings { get; set; } = new RecoilSettings();
    [Category("Player")]
    public SwaySettings SwaySettings { get; set; } = new SwaySettings();
    [Category("Player")]
    public SelectorSettings SelectorSettings { get; set; } = new SelectorSettings();
    [Category("Player")]
    public InvestigationSettings InvestigationSettings { get; set; } = new InvestigationSettings();
    [Category("Player")]
    public CriminalHistorySettings CriminalHistorySettings { get; set; } = new CriminalHistorySettings();
    [Category("Player")]
    public ScannerSettings ScannerSettings { get; set; } = new ScannerSettings();
    [Category("Player")]
    public UISettings UISettings { get; set; } = new UISettings();
    [Category("Player")]
    public KeySettings KeySettings { get; set; } = new KeySettings();
    [Category("Player")]
    public PlayerOtherSettings PlayerOtherSettings { get; set; } = new PlayerOtherSettings();
    [Category("World")]
    public PoliceSettings PoliceSettings { get; set; } = new PoliceSettings();
    [Category("World")]
    public GangSettings GangSettings { get; set; } = new GangSettings();
    [Category("World")]
    public CivilianSettings CivilianSettings { get; set; } = new CivilianSettings();
    [Category("World")]
    public EMSSettings EMSSettings { get; set; } = new EMSSettings();
    [Category("World")]
    public FireSettings FireSettings { get; set; } = new FireSettings();
    [Category("World")]
    public DamageSettings DamageSettings { get; set; } = new DamageSettings();
    [Category("World")]
    public WorldSettings WorldSettings { get; set; } = new WorldSettings();
    [Category("World")]
    public TimeSettings TimeSettings { get; set; } = new TimeSettings();
    [Category("World")]
    public WeatherReportingSettings WeatherReportingSettings { get; set; } = new WeatherReportingSettings();
    public VanillaSettings VanillaSettings { get; set; } = new VanillaSettings();
    public DebugSettings DebugSettings { get; set; } = new DebugSettings();

}
