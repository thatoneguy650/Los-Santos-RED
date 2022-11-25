using LosSantosRED.lsr.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class ScannerDispatches //: IDispatches
{
    private readonly string ScannerDispatchesConfigFileName = "Plugins\\LosSantosRED\\ScannerDispatches.xml";
    private List<DispatchReportStaticData> DispatchDataList = new List<DispatchReportStaticData>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("ScannerDispatches*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Scanner Dispatches config  {ConfigFile.FullName}", 0);
            DispatchDataList = Serialization.DeserializeParams<DispatchReportStaticData>(ConfigFile.FullName);
        }
        else if (File.Exists(ScannerDispatchesConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Scanner Dispatches config  {ScannerDispatchesConfigFileName}", 0);
            DispatchDataList = Serialization.DeserializeParams<DispatchReportStaticData>(ScannerDispatchesConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Scanner Dispatches config found, creating default", 0);
            DefaultConfig();
        }
    }

    public AudioSet AttentionAllUnits()
    {
        return null;
    }

    public string RadioEndBeep()
    {
        return "";
    }

    public string RadioStartBeep()
    {
        return "";
    }

    public DispatchReportStaticData GetStaticData(string dispatchID)
    {
        return null;
    }


    private void DefaultConfig()
    {
        DispatchDataList = new List<DispatchReportStaticData>();
        DispatchReportStaticData OfficerDown = new DispatchReportStaticData()
        {
            Name = "Officer Down",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_officer_down.AcriticalsituationOfficerdown.FileName },"a critical situation, officer down"),
                new AudioSet(new List<string>() { crime_officer_down.AnofferdownpossiblyKIA.FileName },"an officer down, possibly KIA"),
                new AudioSet(new List<string>() { crime_officer_down.Anofficerdown.FileName },"an officer down"),
                new AudioSet(new List<string>() { crime_officer_down.Anofficerdownconditionunknown.FileName },"an officer down, condition unknown"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
            MainMultiAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_officers_down.Multipleofficersdown.FileName },"multiple officers down"),
                new AudioSet(new List<string>() { crime_officers_down.Severalofficersdown.FileName },"several officers down"),
            },
        };

        DispatchReportStaticData OfficerMIA = new DispatchReportStaticData()
        {
            Name = "Officer MIA",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,

            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                //new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName },"we have an officer down, possibly KIA"),
                //new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName },"we have an officer down, condition unknown"),
                new AudioSet(new List<string>() { crime_officer_down.AnofferdownpossiblyKIA.FileName },"an officer down, possibly KIA"),
                new AudioSet(new List<string>() { crime_officer_down.Anofficerdownconditionunknown.FileName },"an officer down, condition unknown"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
        };

        DispatchDataList.Add(OfficerDown);
        DispatchDataList.Add(OfficerMIA);
        Serialization.SerializeParams(DispatchDataList, ScannerDispatchesConfigFileName);
    }




}

