using System;
using System.Collections.Generic;
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
    private List<ISettingsDefaultable> AllDefaultableSettings;
    public SettingsManager()
    {

    }
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
    public KeySettings KeySettings { get; set; } = new KeySettings();


    [Category("Player")]
    public NeedsSettings NeedsSettings { get; set; } = new NeedsSettings();


    [Category("Player")]
    public PlayerOtherSettings PlayerOtherSettings { get; set; } = new PlayerOtherSettings();
    [Category("Player")]
    public CellphoneSettings CellphoneSettings { get; set; } = new CellphoneSettings();
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
    public TaskSettings TaskSettings { get; set; } = new TaskSettings();
    [Category("World")]
    public TimeSettings TimeSettings { get; set; } = new TimeSettings();
    [Category("World")]
    public WeatherReportingSettings WeatherReportingSettings { get; set; } = new WeatherReportingSettings();
    [Category("UI")]
    public UIGeneralSettings UIGeneralSettings { get; set; } = new UIGeneralSettings();
    [Category("UI")]
    public LSRHUDSettings LSRHUDSettings { get; set; } = new LSRHUDSettings();
    [Category("UI")]
    public BarDisplaySettings BarDisplaySettings { get; set; } = new BarDisplaySettings();
    [Category("UI")]
    public ActionWheelSettings ActionWheelSettings { get; set; } = new ActionWheelSettings();
    public VanillaSettings VanillaSettings { get; set; } = new VanillaSettings();
    public DebugSettings DebugSettings { get; set; } = new DebugSettings();
    public void Setup()
    {
        AllDefaultableSettings = new List<ISettingsDefaultable>()
        {
            RespawnSettings, VehicleSettings, PedSwapSettings, ActivitySettings, SprintSettings, ViolationSettings, RecoilSettings, SwaySettings, SelectorSettings, InvestigationSettings, CriminalHistorySettings, ScannerSettings, KeySettings, PlayerOtherSettings, CellphoneSettings,
            PoliceSettings,GangSettings,CivilianSettings, EMSSettings,FireSettings, DamageSettings, WorldSettings, TaskSettings, TimeSettings, WeatherReportingSettings, VanillaSettings,DebugSettings,
            UIGeneralSettings,LSRHUDSettings,BarDisplaySettings,ActionWheelSettings, NeedsSettings

        };


//#if DEBUG
//        PoliceSettings.ManageDispatching = false;
//        EMSSettings.ManageDispatching = false;
//        GangSettings.ManageDispatching = false;
//#endif
    }
    public void SetDefault()
    {
        foreach(ISettingsDefaultable settingsDefaultable in AllDefaultableSettings)
        {
            settingsDefaultable.SetDefault();
        }
    }


}
