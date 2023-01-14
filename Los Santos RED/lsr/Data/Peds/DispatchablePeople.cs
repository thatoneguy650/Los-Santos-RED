using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople : IDispatchablePeople
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\DispatchablePeople.xml";
    private List<DispatchablePersonGroup> PeopleGroupLookup = new List<DispatchablePersonGroup>();
    private List<DispatchablePerson> StandardCops;
    private List<DispatchablePerson> SheriffPeds;
    private List<DispatchablePerson> NOOSEPeds;
    private List<DispatchablePerson> FIBPeds;
    private List<DispatchablePerson> ParkRangers;
    private List<DispatchablePerson> DOAPeds;
    private List<DispatchablePerson> SAHPPeds;
    private List<DispatchablePerson> MilitaryPeds;
    private List<DispatchablePerson> PrisonPeds;
    private List<DispatchablePerson> SecurityPeds;
    private List<DispatchablePerson> CoastGuardPeds;
    private List<DispatchablePerson> NYSPPeds;
    private List<DispatchablePerson> Firefighters;
    private List<DispatchablePerson> EMTs;
    private List<DispatchablePerson> GreenEMTs;
    private List<DispatchablePerson> BlueEMTs;
    private List<DispatchablePerson> LostMCPeds;
    private List<DispatchablePerson> VagosPeds;
    private List<DispatchablePerson> FamiliesPeds;
    private List<DispatchablePerson> BallasPeds;
    private List<DispatchablePerson> MarabuntaPeds;
    private List<DispatchablePerson> AltruistPeds;
    private List<DispatchablePerson> VarriosPeds;
    private List<DispatchablePerson> TriadsPeds;
    private List<DispatchablePerson> KoreanPeds;
    private List<DispatchablePerson> RedneckPeds;
    private List<DispatchablePerson> ArmenianPeds;
    private List<DispatchablePerson> CartelPeds;
    private List<DispatchablePerson> MafiaPeds;
    private List<DispatchablePerson> YardiesPeds;
    private List<DispatchablePerson> StandardCops_Old;
    private List<DispatchablePerson> FIBPeds_Old;
    private List<DispatchablePerson> NOOSEPeds_Old;
    private List<DispatchablePerson> SheriffPeds_Old;
    private List<DispatchablePerson> StandardCops_Simple;
    private List<DispatchablePerson> PrisonPeds_Simple;
    private List<DispatchablePerson> SecurityPeds_Simple;
    public List<DispatchablePersonGroup> AllPeople => PeopleGroupLookup;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("DispatchablePeople*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable People config: {ConfigFile.FullName}", 0);
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable People config  {ConfigFile.FullName}", 0);
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Dispatchable People config found, creating default", 0);
            SetupDefault();
            DefaultConfig_LosSantos2008();
            DefaultConfig_Simple();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig();
        }
    }
    public List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID)
    {
        return PeopleGroupLookup.FirstOrDefault(x => x.DispatchablePersonGroupID == dispatchablePersonGroupID)?.DispatchablePeople;
    }
    private void SetupDefault()
    {
        //Cops
        StandardCops = new List<DispatchablePerson>() {
            new DispatchablePerson("s_f_y_cop_01",0,0) { 
                DebugName = "LSPDDefaultFemale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 10 
            },//new PedPropComponent(0, 0, 0)//Hat,//new PedPropComponent(1, 0, 0)//Glasses
            new DispatchablePerson("s_m_y_cop_01",0,0) { 
                DebugName = "LSPDDefaultMale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }
                ,OptionalPropChance = 10 
            },

            new DispatchablePerson("mp_f_freemode_01",40,40) { 
                DebugName = "LSPDMPNoArmorFemale"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 45, 0), new PedPropComponent(1, 25, 9) }
                ,OptionalPropChance = 10
            },//no body armor with hat and glasses possible
            new DispatchablePerson("mp_f_freemode_01",0,40) { 
                DebugName = "LSPDMPArmorFemale"
                ,RandomizeHead = true
                ,MinWantedLevelSpawn = 3
                ,ArmorMin = 30
                ,ArmorMax = 50
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 3, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 15), new PedPropComponent(1, 25, 9) }
                ,OptionalPropChance = 10
            },//body armor, with riot helmet and glasses possible

            new DispatchablePerson("mp_m_freemode_01",60,60) { 
                DebugName = "LSPDMPNoArmorMale"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 46, 0), new PedPropComponent(1, 23, 9) }
                ,OptionalPropChance = 10
            },
            new DispatchablePerson("mp_m_freemode_01",0,60) { 
                DebugName = "LSPDMPArmorMale"
                ,RandomizeHead = true
                ,MinWantedLevelSpawn = 3
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 124, 15), new PedPropComponent(1, 23, 9) }
                ,OptionalPropChance = 10
            },



            new DispatchablePerson("mp_m_freemode_01",2,2) {
                DebugName = "LSPDMPDetectiveMale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0),new PedComponent(6, 10, 0, 0), new PedComponent(7, 125, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() {  }
                ,OptionalPropChance = 0
            },

            new DispatchablePerson("mp_f_freemode_01",2,2) {
                DebugName = "LSPDMPDetectiveFemale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 29, 0, 0), new PedComponent(7, 95, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() {  }
                ,OptionalPropChance = 10
            },


        };
        SheriffPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_sheriff_01",60,60) {
                DebugName = "LSSDDefaultNoArmorMale"
                ,MaxWantedLevelSpawn = 2
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70 
            },//filled duty belt
            new DispatchablePerson("s_m_y_sheriff_01",0,75) {
                DebugName = "LSSDDefaultArmorMale"
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 2, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 95 
            },//filled duty belt},//vest

            new DispatchablePerson("s_f_y_sheriff_01",40,40) {
                DebugName = "LSSDDefaultNoArmorFemale"
                ,MaxWantedLevelSpawn = 2
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0) }
                ,OptionalPropChance = 70 
            },
            new DispatchablePerson("s_f_y_sheriff_01",0,25) {
                DebugName = "LSSDDefaultArmorFemale"
                ,MaxWantedLevelSpawn = 3
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0) }
                ,OptionalPropChance = 95 
            },
        };
        NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 0,0){
                DebugName = "NOOSEDefaultMale"
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) 
            },

            new DispatchablePerson("mp_m_freemode_01", 100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "NOOSEMPMale1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0),new PedPropComponent(1, 23, 9)}) 
            },          
            new DispatchablePerson("mp_m_freemode_01", 100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "NOOSEMPMale2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0)}) 
            },

            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "NOOSEMPFemale1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0),new PedPropComponent(1,25,9) }) 
            },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "NOOSEMPFemale2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0)}) 
            },
        };
        FIBPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){ DebugName = "FIBAgentMale",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){ DebugName = "FIBOfficeMale1",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){ DebugName = "FIBOfficeMale2",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) { DebugName = "FIBOfficeMale3",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0,0) {
                DebugName = "FIBSWATMale"
                ,GroupName = "FIBHRT"
                ,AllowRandomizeBeforeVariationApplied = true
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) 
            },

            new DispatchablePerson("mp_m_freemode_01", 25, 25, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "FIBMPMale1"
                ,GroupName = "FIBHRT"
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },//328 cool too
                    new List<PedPropComponent>() { new PedPropComponent(0, 19, 0),new PedPropComponent(1, 15, 9), }) 
            },
            new DispatchablePerson("mp_m_freemode_01", 75,75, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "FIBMPMale2"
                ,GroupName = "FIBHRT"
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 117, 0),new PedPropComponent(1, 25, 4), }) 
            },

            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "FIBMPFemale1"
                ,GroupName = "FIBHRT"
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(2, 42, 0, 0),new PedComponent(3, 18, 0, 0),new PedComponent(4, 130, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(8, 0, 0, 0),new PedComponent(9, 18, 2, 0),new PedComponent(11, 54, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 19, 0), }) 
            },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "FIBMPFemale2"
                ,GroupName = "FIBHRT"
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(2, 42, 0, 0),new PedComponent(3, 18, 0, 0),new PedComponent(4, 130, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(8, 0, 0, 0),new PedComponent(9, 18, 2, 0),new PedComponent(11, 54, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 116, 0),new PedPropComponent(1, 27, 4), }) 
            },
        };
        ParkRangers = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_ranger_01",75,75) {
                DebugName = "ParkRangerMale"
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}
                ,OptionalPropChance = 70
            },
            new DispatchablePerson("s_f_y_ranger_01",25,25) {
                DebugName = "ParkRangerFemale"
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }
                ,OptionalPropChance = 70 
            },
        };
        DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) { DebugName = "DOAAgentMale" },
        };
        SAHPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){
                DebugName = "SAHPCarMale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,GroupName = "StandardSAHP"
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }, OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { 
                    new PedComponent(4, 1, 0, 0) }, 
                    new List<PedPropComponent>() ) 
            },
            new DispatchablePerson("s_m_y_hwaycop_01",0,0){
                DebugName = "SAHPMotorcycleMale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,RequiredHelmetType = 1024
                ,GroupName = "MotorcycleCop"
                ,UnitCode = "Mary"
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }, OptionalPropChance = 90
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(4, 0, 0, 0) }, 
                    new List<PedPropComponent>() ) 
            },
        };
        MilitaryPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_armymech_01",25,0) { DebugName = "ARMYNormalMale1" },
            new DispatchablePerson("s_m_m_marine_01",50,0) { DebugName = "ARMYNormalMale2" },
            new DispatchablePerson("s_m_m_marine_02",0,0) { DebugName = "ARMYNormalMale3" },
            new DispatchablePerson("s_m_y_marine_03",100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { 
                DebugName = "ARMYSoldierMale"
                ,MaxWantedLevelSpawn = 10
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)}) 
            },
            new DispatchablePerson("s_m_y_pilot_01",0,0) { DebugName = "ARMYPilotMale1" },
            new DispatchablePerson("s_m_m_pilot_02",0,0) { DebugName = "ARMYPilotMale2" },
        };
        PrisonPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",0,0)  { DebugName = "PrisonGuardMale" },


            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeTanPrisonShortSleeveGuardMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(10, 72, 0, 0), new PedComponent(11, 319, 3, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0) }
                ,OptionalComponentChance = 35
            },

            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeForestGreeenShortSleevePrisonGuardMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(10, 72, 0, 0), new PedComponent(11, 319, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0) }
                ,OptionalComponentChance = 35
            },



            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeTanPrisonLongSleeveGuardMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(10, 72, 0, 0), new PedComponent(11, 317, 3, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0) }
                ,OptionalComponentChance = 35
            },

            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeForestGreeenPrisonLongSleeveGuardMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(10, 72, 0, 0), new PedComponent(11, 317, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0)}
                ,OptionalComponentChance = 35
            },


                        //Male
            //Top 317 = long sleeve 316 is collar closed
            //319 = short sleeve 318 is collar closed
            //Textur 0 - Black, 1 - Forest Green, 2 - Cream, 3- Tan, 4 - Dark Green, 5 - white, 6 - off white, 7 grey

            //Lower
            //122 = pants tucked in
            //1234 = pants not tucked in


            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeTanPrisonShortSleeveGuardFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(10, 81, 0, 0), new PedComponent(11, 330, 3, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }
                ,OptionalComponentChance = 35
            },

            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeForestGreenPrisonShortSleeveGuardFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(10, 81, 0, 0), new PedComponent(11, 330, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }
                ,OptionalComponentChance = 35
            },


            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeTanPrisonLongSleeveGuardFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(10, 81, 0, 0), new PedComponent(11, 328, 3, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }
                ,OptionalComponentChance = 35
            },

            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeForestGreenPrisonLongSleeveGuardFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(10, 81, 0, 0), new PedComponent(11, 328, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                ,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }
                ,OptionalComponentChance = 35
            },

        };
        SecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",0,0)  { DebugName = "SecurityMale" },


            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeCreamSecurityShortSleeveMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 71, 0, 0), new PedComponent(11, 319, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                //,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0) }//tucked in pants
                //,OptionalComponentChance = 35
            },

            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "FreemodeCreamSecurityLongSleeveMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 122, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 71, 0, 0), new PedComponent(11, 317, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25

                //,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 123, 0, 0) }//tucked in pants
                //,OptionalComponentChance = 35
            },

                        //Male
            //Top 317 = long sleeve 316 is collar closed
            //319 = short sleeve 318 is collar closed
            //Textur 0 - Black, 1 - Forest Green, 2 - Cream, 3- Tan, 4 - Dark Green, 5 - white, 6 - off white, 7 grey

            //Lower
            //122 = pants tucked in
            //1234 = pants not tucked in
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeCreamSecurityShortSleeveFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 80, 0, 0), new PedComponent(11, 330, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                //,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }//tucked in pants
                //,OptionalComponentChance = 35
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "FreemodeCreamSecurityLongSleeveFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 128, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 80, 0, 0), new PedComponent(11, 328, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10

                //,OptionalComponents = new List<PedComponent>() { new PedComponent(4, 129, 0, 0) }//tucked in pants
                //,OptionalComponentChance = 35
            },

        };
        CoastGuardPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_uscg_01",100,100)  { DebugName = "CoastGuardMale" },
        };
        NYSPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_snowcop_01",100,100)  { DebugName = "NYSPDefaultMale" },
        };
        Firefighters = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_fireman_01",100,100)  { DebugName = "FireFighterMale" },
        };
        EMTs = new List<DispatchablePerson>() { 
            new DispatchablePerson("s_m_m_paramedic_01", 0, 0) { DebugName = "EMTMaleDefault" },
        };

        GreenEMTs = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_freemode_01",100,100) {
                DebugName = "EMTMaleGreen1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 85, 0, 0),
                        new PedComponent(4, 96, 1, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 127, 0, 0),
                        new PedComponent(8, 129, 0, 0),
                        new PedComponent(10, 58, 0, 0),
                        new PedComponent(11, 250, 1, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 122, 1) })
            },
            new DispatchablePerson("mp_m_freemode_01",100,100) {
                DebugName = "EMTMaleGreen2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 85, 0, 0),
                        new PedComponent(4, 96, 1, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 126, 0, 0),
                        new PedComponent(8, 129, 0, 0),
                        new PedComponent(10, 58, 0, 0),
                        new PedComponent(11, 250, 1, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 122, 1) })
            },
            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "EMTFemaleGreen1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 98, 0, 0),
                        new PedComponent(4, 99, 1, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(7, 97, 0, 0),
                        new PedComponent(8, 159, 0, 0),
                        new PedComponent(10, 66, 0, 0),//or 66 65 is the one with chest icon, 66 has only side
                        new PedComponent(11, 258, 1, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 121, 1) })
            },
            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "EMTFemaleGreen2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 98, 0, 0),
                        new PedComponent(4, 99, 1, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(7, 96, 0, 0),
                        new PedComponent(8, 159, 0, 0),
                        new PedComponent(10, 66, 0, 0),//or 66 65 is the one with chest icon, 66 has only side
                        new PedComponent(11, 258, 1, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 121, 1) })
            },

            new DispatchablePerson("s_m_m_paramedic_01",0,0)   { DebugName = "EMTMaleDefault" },

        };
        BlueEMTs = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_freemode_01",100,100) {
                DebugName = "EMTMaleBlue1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 85, 0, 0),
                        new PedComponent(4, 96, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 127, 0, 0),
                        new PedComponent(8, 129, 0, 0),
                        new PedComponent(10, 58, 0, 0),
                        new PedComponent(11, 250, 0, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 122, 0) })
            },
            new DispatchablePerson("mp_m_freemode_01",100,100) {
                DebugName = "EMTMaleBlue2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 85, 0, 0),
                        new PedComponent(4, 96, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 126, 0, 0),
                        new PedComponent(8, 129, 0, 0),
                        new PedComponent(10, 58, 0, 0),
                        new PedComponent(11, 250, 0, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 122, 0) })
            },
            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "EMTFemaleBlue1"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 98, 0, 0),
                        new PedComponent(4, 99, 0, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(7, 97, 0, 0),
                        new PedComponent(8, 159, 0, 0),
                        new PedComponent(10, 66, 0, 0),//or 66
                        new PedComponent(11, 258, 0, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 121, 0) })
            },
            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "EMTFemaleBlue2"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(3, 98, 0, 0),
                        new PedComponent(4, 99, 0, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(7, 96, 0, 0),
                        new PedComponent(8, 159, 0, 0),
                        new PedComponent(10, 66, 0, 0),//or 66
                        new PedComponent(11, 258, 0, 0)
                    },
                    new List<PedPropComponent>() { new PedPropComponent(0, 121, 0) })
            },
            new DispatchablePerson("s_m_m_paramedic_01",0,0)   { DebugName = "EMTMaleDefault" },
        };

        //Gangs
        LostMCPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_lost_01",30,30,5,10,400,600,0,1) { DebugName = "LOSTMale1" },
            new DispatchablePerson("g_m_y_lost_02",30,30,5,10,400,600,0,1) { DebugName = "LOSTMale2" },
            new DispatchablePerson("g_m_y_lost_03",30,30,5,10,400,600,0,1) { DebugName = "LOSTMale3" },
            //new DispatchablePerson("ig_clay",30,30,5,10,400,600,0,1) { DebugName = "LOSTClay" },
            new DispatchablePerson("g_f_y_lost_01",10,10,5,10,400,600,0,1) { DebugName = "LOSTFemale1" },
        };
        VagosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_mexgoon_01",30,30,5,10,400,600,0,1) { DebugName = "VagosMale1" },
            new DispatchablePerson("g_m_y_mexgoon_02",30,30,5,10,400,600,0,1) { DebugName = "VagosMale2" },
            new DispatchablePerson("g_m_y_mexgoon_03",30,30,5,10,400,600,0,1) { DebugName = "VagosMale3" },
            new DispatchablePerson("g_f_y_vagos_01",10,10,5,10,400,600,0,1) { DebugName = "VagosFemale1" },
        };
        FamiliesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_famca_01",30,30,5,10,400,600,0,1) { DebugName = "FamiliesMale1" },
            new DispatchablePerson("g_m_y_famdnf_01",30,30,5,10,400,600,0,1) { DebugName = "FamiliesMale2" },
            new DispatchablePerson("g_m_y_famfor_01",30,30,5,10,400,600,0,1) { DebugName = "FamiliesMale3" },
            new DispatchablePerson("g_f_y_families_01",10,10,5,10,400,600,0,1) { DebugName = "FamiliesFemale1" },
        };
        BallasPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_ballasout_01",30,30,5,10,400,600,0,1) { DebugName = "BallasMale1" },
            new DispatchablePerson("g_m_y_ballaeast_01",30,30,5,10,400,600,0,1) { DebugName = "BallasMale2" },
            new DispatchablePerson("g_m_y_ballaorig_01",30,30,5,10,400,600,0,1) { DebugName = "BallasMale3" },
            new DispatchablePerson("g_f_y_ballas_01",10,10,5,10,400,600,0,1) { DebugName = "BallasFemale1" },
        };
        MarabuntaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_salvaboss_01",30,30,5,10,400,600,0,1) { DebugName = "MarabuntaMale1" },
            new DispatchablePerson("g_m_y_salvagoon_01",30,30,5,10,400,600,0,1) { DebugName = "MarabuntaMale2" },
            new DispatchablePerson("g_m_y_salvagoon_02",30,30,5,10,400,600,0,1) { DebugName = "MarabuntaMale3" },
            new DispatchablePerson("g_m_y_salvagoon_03",10,10,5,10,400,600,0,1) { DebugName = "MarabuntaMale4" },
        };
        AltruistPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_acult_01",30,30,5,10,400,600,0,1) { DebugName = "CultMale1" },
            new DispatchablePerson("a_m_o_acult_01",30,30,5,10,400,600,0,1) { DebugName = "CultMale2" },
            new DispatchablePerson("a_m_o_acult_02",30,30,5,10,400,600,0,1) { DebugName = "CultMale3" },
            new DispatchablePerson("a_m_y_acult_01",10,10,5,10,400,600,0,1) { DebugName = "CultMale4" },
            new DispatchablePerson("a_m_y_acult_02",10,10,5,10,400,600,0,1) { DebugName = "CultMale5" },
            new DispatchablePerson("a_f_m_fatcult_01",10,10,5,10,400,600,0,1) { DebugName = "CultFemale1" },
        };
        VarriosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_azteca_01",100,100,5,10,400,600,0,1) { DebugName = "VarriosMale" },
            //new DispatchablePerson("ig_ortega",20,20,5,10,400,600,0,1) { DebugName = "VarriosOrtegaMale" },
        };
        TriadsPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_chigoon_01",33,33,5,10,400,600,0,1) { DebugName = "TriadMale1" },
            new DispatchablePerson("g_m_m_chigoon_02",33,33,5,10,400,600,0,1) { DebugName = "TriadMale2" },
            new DispatchablePerson("g_m_m_korboss_01",33,33,5,10,400,600,0,1) { DebugName = "TriadMale3" },
            //new DispatchablePerson("ig_hao",33,33,5,10,400,600,0,1) { DebugName = "TriadHaoMale1" },
        };
        KoreanPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_korean_01",33,33,5,10,400,600,0,1) { DebugName = "KoreanMale1" },
            new DispatchablePerson("g_m_y_korean_02",33,33,5,10,400,600,0,1) { DebugName = "KoreanMale2" },
            new DispatchablePerson("g_m_y_korlieut_01",33,33,5,10,400,600,0,1) { DebugName = "KoreanMale3" },
        };
        RedneckPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1) { DebugName = "RedneckMale1" },
            new DispatchablePerson("a_m_m_hillbilly_02",30,30,5,10,400,600,0,1) { DebugName = "RedneckMale2" },
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1) { DebugName = "RedneckMale3" },
            new DispatchablePerson("a_m_m_hillbilly_02",10,10,5,10,400,600,0,1) { DebugName = "RedneckMale4" },
        };
        ArmenianPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_armboss_01",30,30,5,10,400,600,0,1) { DebugName = "ArmenianMale1" },
            new DispatchablePerson("g_m_m_armgoon_01",30,30,5,10,400,600,0,1) { DebugName = "ArmenianMale2" },
            new DispatchablePerson("g_m_m_armlieut_01",30,30,5,10,400,600,0,1) { DebugName = "ArmenianMale3" },
            new DispatchablePerson("g_m_y_armgoon_02",10,10,5,10,400,600,0,1) { DebugName = "ArmenianMale4" },
        };
        CartelPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_mexboss_01",30,30,5,10,400,600,0,1) { DebugName = "CartelMale1" },
            new DispatchablePerson("g_m_m_mexboss_02",30,30,5,10,400,600,0,1) { DebugName = "CartelMale2" },
            new DispatchablePerson("g_m_y_mexgang_01",30,30,5,10,400,600,0,1) { DebugName = "CartelMale3" },
            new DispatchablePerson("a_m_y_mexthug_01",30,30,5,10,400,600,0,1) { DebugName = "CartelMale4" },
        };
        MafiaPeds = new List<DispatchablePerson>() {
            //Dark Grey Unbuttoned Suit with Red Tie and buttoned undershirt
            new DispatchablePerson("mp_m_freemode_01",100,100,5,10,400,600,0,1) { 
                RandomizeHead = true
                 ,RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 4, 0, 0),
                     new PedComponent(4, 10, 0, 0),
                     new PedComponent(6, 10, 0, 0),
                     new PedComponent(7, 21, 2, 0),
                     new PedComponent(8, 10, 0, 0),
                     new PedComponent(11, 4, 0, 0) },new List<PedPropComponent>() { })
                 ,OptionalComponents = new List<PedComponent>() {
                    new PedComponent(7, 21, 1, 0),//7-21 is a tie in lotsa colors
                    new PedComponent(7, 21, 2, 0),
                    new PedComponent(7, 21, 3, 0),
                    new PedComponent(7, 21, 4, 0),
                    new PedComponent(7, 21, 5, 0),
                    new PedComponent(7, 21, 6, 0),
                    new PedComponent(7, 21, 7, 0),
                    new PedComponent(7, 21, 8, 0),
                    new PedComponent(7, 21, 9, 0),
                    new PedComponent(7, 21, 10, 0),
                    new PedComponent(7, 21, 11, 0),
                    new PedComponent(7, 21, 12, 0),

                    new PedComponent(8, 31, 0, 0),//undershirt in a few colors, buttoned
                    new PedComponent(8, 31, 1, 0),
                    new PedComponent(8, 31, 2, 0),
                    new PedComponent(8, 31, 3, 0),
                    new PedComponent(8, 31, 4, 0),
                    new PedComponent(8, 31, 5, 0),
                    new PedComponent(8, 31, 6, 0),
                    new PedComponent(8, 31, 7, 0),
                    new PedComponent(8, 31, 8, 0),
                    new PedComponent(8, 31, 9, 0),
                    new PedComponent(8, 31, 10, 0),
                    new PedComponent(8, 31, 11, 0),
                    new PedComponent(8, 31, 12, 0),
                    new PedComponent(8, 31, 13, 0),
                    new PedComponent(8, 31, 14, 0),
                    new PedComponent(8, 31, 15, 0),

                    new PedComponent(11, 10, 0, 0),//closed jacket
                 }
                 ,OptionalComponentChance = 80,
            },
            //Dark Grey Unbuttoned Suit with unbuttoned undershirt
            new DispatchablePerson("mp_m_freemode_01",100,100,5,10,400,600,0,1) { 
                RandomizeHead = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 4, 0, 0),
                     new PedComponent(4, 10, 0, 0),
                     new PedComponent(6, 10, 0, 0),
                     new PedComponent(7, 0, 0, 0),
                     new PedComponent(8, 11, 0, 0),
                     new PedComponent(11, 4, 0, 0) },new List<PedPropComponent>() { })
                ,OptionalComponents = new List<PedComponent>() {
                    new PedComponent(8, 11, 0, 0),//undershirt in a few colors, unbuttoned
                    new PedComponent(8, 11, 1, 0),
                    new PedComponent(8, 11, 2, 0),
                    new PedComponent(8, 11, 3, 0),
                    new PedComponent(8, 11, 4, 0),
                    new PedComponent(8, 11, 5, 0),
                    new PedComponent(8, 11, 6, 0),
                    new PedComponent(8, 11, 7, 0),
                    new PedComponent(8, 11, 8, 0),
                    new PedComponent(8, 11, 9, 0),
                    new PedComponent(8, 11, 10, 0),
                    new PedComponent(8, 11, 11, 0),
                    new PedComponent(8, 11, 12, 0),
                    new PedComponent(8, 11, 13, 0),
                    new PedComponent(8, 11, 14, 0),
                    new PedComponent(8, 11, 15, 0),

                    new PedComponent(11, 10, 0, 0),//closed jacket
                }
                ,OptionalComponentChance = 80,
             }, 
        };
        YardiesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_og_boss_01",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale1" },
            new DispatchablePerson("a_m_o_soucent_01",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale2" },
            new DispatchablePerson("a_m_y_soucent_02",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale3" },
        };

        ////  ,CustomPropAttachments = new List<CustomPropAttachment>() { new CustomPropAttachment("prop_holster_01", "BONETAG_PELVIS", new Vector3(-0.04f, 0f, 0.19f), new Rotator(-90f, -90f, 0f)) { SpawnChance = 100f } }

        //LSPD//
        //s_m_y_cop_01
        //new PedPropComponent(0, 0, 0) Goofy Hat
        //new PedPropComponent(1, 0, 0) Spy Glasses Forward
        //new PedPropComponent(1, 1, 0) Spy Glasses Backwards
        //new PedPropComponent(1, 2, 0) Spy Glasses On Head
        //new PedPropComponent(1, 3, 0) Aviators

        //s_f_y_cop_01
        //new PedPropComponent(0, 0, 0) Goofy Hat
        //new PedPropComponent(1, 0, 0) Aviators


        //LSSD//
        //s_m_y_sheriff_01
        //new PedPropComponent(0, 0, 0) Flat Hat
        //new PedPropComponent(0, 1, 0) Cowboy Hat
        //new PedPropComponent(1, 0, 0) Aviators
        //new PedPropComponent(1, 1, 0) Spy Glasses Forwards

        //s_f_y_sheriff_01
        //new PedPropComponent(0, 0, 0) Flat Hat
        //new PedPropComponent(0, 1, 0) Cowboy Hat
        //new PedPropComponent(1, 0, 0) Aviators


        //SAHP//
        //s_m_y_hwaycop_01
        //new PedPropComponent(0, 0, 0) Helmet
        //new PedPropComponent(1, 0, 0) Spy Glasses Forward
        //new PedPropComponent(1, 1, 0) Aviators

        //Park Rangers//
        //s_m_y_ranger_01
        //new PedPropComponent(0, 0, 0) Flat Hat
        //new PedPropComponent(0, 1, 0) Baseball Hat
        //new PedPropComponent(1, 0, 0) Aviators

        //s_f_y_ranger_01
        //new PedPropComponent(1, 0, 0) Aviators

        //ARMY//
        //s_m_y_marine_03
        //new PedPropComponent(1, 0, 0) ESS Glasses
    }
    private void DefaultConfig()
    {
        //Cops
        PeopleGroupLookup.Add(new DispatchablePersonGroup("StandardCops", StandardCops));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("EMTs", EMTs));

        //Gangs
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

        Serialization.SerializeParams(PeopleGroupLookup, ConfigFileName);
    }
    private void DefaultConfig_LosSantos2008()
    {
        //2008
        StandardCops_Old = new List<DispatchablePerson>() {
            new DispatchablePerson("s_f_y_cop_01",40,40) { 
                DebugName = "LSPDDefaultOldFemale"
                ,AllowRandomizeBeforeVariationApplied = true
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } 
            },//new PedPropComponent(0, 0, 0)//Hat,//new PedPropComponent(1, 0, 0)//Glasses
            new DispatchablePerson("s_m_y_cop_01",60,60) {
                DebugName = "LSPDDefaultOldMale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) }) 
            },
        };
        FIBPeds_Old = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){ DebugName = "FIBAgentMale", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){ DebugName = "FIBNormalMale1", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){ DebugName = "FIBNormalMale2", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) { DebugName = "FIBNormalMale3", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0,100) {
                DebugName = "FIBSWATOldMale"
                ,GroupName = "FIBHRT"
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) 
            },
        };
        NOOSEPeds_Old = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 100,100){
                DebugName = "NOOSEDefaultMale"
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },
        };
        SheriffPeds_Old = new List<DispatchablePerson>() {
            new DispatchablePerson("s_f_y_sheriff_01",35,35) {
                DebugName = "LSSDDefaultOldFemale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0) }
                ,OptionalPropChance = 70 
            },
            new DispatchablePerson("s_m_y_sheriff_01",65,65) {
                DebugName = "LSSDDefaultOldMale"
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70 
            },//filled duty belt
        };

        List<DispatchablePersonGroup> PeopleGroupLookup_Old = new List<DispatchablePersonGroup>();
        //Cops
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("StandardCops", StandardCops_Old));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds_Old));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds_Old));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_Old));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));

        //Gangs
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

        Serialization.SerializeParams(PeopleGroupLookup_Old, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\DispatchablePeople_LosSantos2008.xml");
    }
    private void DefaultConfig_Simple()
    {
        StandardCops_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_f_y_cop_01",40,40) {
                DebugName = "LSPDDefaultOldFemale"
                ,MaxWantedLevelSpawn = 2
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } 
            },//new PedPropComponent(0, 0, 0)//Hat,//new PedPropComponent(1, 0, 0)//Glasses
            new DispatchablePerson("s_m_y_cop_01",60,60) {
                DebugName = "LSPDDefaultOldNoArmorMale"
                ,MaxWantedLevelSpawn = 2
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) }) 
            },
            new DispatchablePerson("s_m_y_cop_01",0,60) {
                DebugName = "LSPDDefaultOldArmorMale"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,EmptyHolster = new PedComponent(9,0,0,0)
                ,FullHolster = new PedComponent(9,1,0,0)
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }//Glasses only?
                ,OptionalPropChance = 80
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { 
                    new PedComponent(9, 2, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 1, 1) }) 
            },//vest, no hat
        };

        PrisonPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100)  { DebugName = "PrisonGuardMale" },
        };
        SecurityPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100)  { DebugName = "SecurityMale" },
        };

        //
        List<DispatchablePersonGroup> PeopleGroupLookup_Simple = new List<DispatchablePersonGroup>();
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("StandardCops", StandardCops_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds_Old));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_Old));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("EMTs", EMTs));


        //Gangs
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));

        Serialization.SerializeParams(PeopleGroupLookup_Simple, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\DispatchablePeople_Simple.xml");
    }
    private void DefaultConfig_FullExpandedJurisdiction()
    {
        List<DispatchablePersonGroup> PeopleConfig_EUP = new List<DispatchablePersonGroup>();

        //Cops
        List<DispatchablePerson> StandardCops_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },

            //LSPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0), 
                        new PedComponent(5, 52, 0, 0), 
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0), 
                        new PedComponent(9, 0, 0, 0), 
                        new PedComponent(10, 0, 0, 0), 
                        new PedComponent(11, 200, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents = 
                new List<PedComponent>() { 
                    new PedComponent(10, 12, 0, 0), //Ranks
                    new PedComponent(10, 12, 1, 0),
                    new PedComponent(10, 12, 2, 0), 
                    new PedComponent(10, 12, 3, 0), 
                    new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },

            //LSPD Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 12, 0, 0), //Ranks
                    new PedComponent(10, 12, 1, 0),
                    new PedComponent(10, 12, 2, 0),
                    new PedComponent(10, 12, 3, 0),
                    new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },

            //LSPD Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 15, 0, 0), //Ranks
                    new PedComponent(10, 15, 1, 0),
                    new PedComponent(10, 15, 2, 0),
                    new PedComponent(10, 15, 3, 0),
                    new PedComponent(10, 44, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 14, 0, 0), //Ranks
                    new PedComponent(10, 14, 1, 0), //Ranks
                    new PedComponent(10, 14, 2, 0),
                    new PedComponent(10, 14, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                } },

            //LSPD Jacket
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 29, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 28, 0, 0), 
                } },
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 31, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 30, 0, 0), //Ranks
                } },

            //LSPD Raincoat
            new DispatchablePerson("mp_m_freemode_01",1,1) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 28, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 187, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",1,1) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 30, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 189, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //LSPD SWAT
            new DispatchablePerson("mp_m_freemode_01",0,20) { RandomizeHead = true,MinWantedLevelSpawn = 3,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 179, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 16, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,150,0),new PedPropComponent(1,21,0)  }),
            },
            new DispatchablePerson("mp_f_freemode_01",0,20) { RandomizeHead = true,MinWantedLevelSpawn = 3,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 215, 0, 0),
                        new PedComponent(4, 30, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 18, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,149,0),new PedPropComponent(1,22,0)  }),
             },

            //LSPD Detective Suit
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 68, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 38, 3, 0),
                        new PedComponent(8, 178, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 4, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 68, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 0, 0, 0),
                        new PedComponent(8, 64, 0, 0),
                        new PedComponent(9, 26, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 24, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //LSPD Detective
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 10, 3, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 6, 3, 0),
                        new PedComponent(8, 88, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 349, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 0, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 26, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 27, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //LSPD Detective Windbraker
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 12, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 59, 9, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 179, 0, 0),
                        new PedComponent(9, 17, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 325, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 59, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 39, 0, 0),
                        new PedComponent(9, 21, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 318, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //Metro (little more tactical uniforms)
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male METRO Div Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,101,1),
              new PedComponent(3,4,0),
              new PedComponent(10,12,0),
              new PedComponent(8,94,0),
              new PedComponent(4,86,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male METRO Div Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,102,1),
              new PedComponent(3,11,0),
              new PedComponent(10,15,0),
              new PedComponent(8,94,0),
              new PedComponent(4,86,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female METRO Div Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,92,1),
              new PedComponent(3,3,0),
              new PedComponent(10,11,0),
              new PedComponent(8,101,0),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female METRO Div Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,93,1),
              new PedComponent(3,14,0),
              new PedComponent(10,14,0),
              new PedComponent(8,101,0),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },

            //Utility (Tactical looking, but no armor)
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,209,0),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,212,0),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,225,0),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,226,0),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }) },

            //Motor Units (Motorcycle cops, we dont have that yet...)
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Motor Unit Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,200,1),
            new PedComponent(3,20,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Motor Unit Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,193,1),
            new PedComponent(3,20,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Motor Unit Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,190,1),
            new PedComponent(3,26,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Motor Unit Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,202,1),
            new PedComponent(3,23,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Motor Unit Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,195,1),
            new PedComponent(3,23,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Motor Unit Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,192,1),
            new PedComponent(3,28,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },

            //Traffic Utility (kinda tactical looking, but no armor)
            new DispatchablePerson("mp_m_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Traffic Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,209,15),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Traffic Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,212,15),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Traffic Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,225,15),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Traffic Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,226,15),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }) },

            //Beach Detail (Still in uniform, just with polo and baseball hat)
            new DispatchablePerson("mp_m_freemode_01",2,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Beach Detail>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,135,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,93,2),
          new PedComponent(3,0,0),
          new PedComponent(10,0,0),
          new PedComponent(8,94,0),
          new PedComponent(4,86,12),
          new PedComponent(6,2,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }) },
            new DispatchablePerson("mp_f_freemode_01",2,0) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Beach Detail>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,84,2),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,2,0),
              new PedComponent(4,89,12),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },

            //Detective (with armor)            
            new DispatchablePerson("mp_m_freemode_01",1,1) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Armor Protection>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,3,0), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,1),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,0),
              new PedComponent(4,10,4),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",1,1) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Raid Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,0),
              new PedComponent(3,12,0),
              new PedComponent(10,0,0),
              new PedComponent(8,179,1),
              new PedComponent(4,10,4),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,17,0),
              new PedComponent(5,59,0),
               }) },

        };
        List<DispatchablePerson> LSIAPDPeds_FEJ = new List<DispatchablePerson>() {
            //LSIA Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                } },

            //LSIA Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 44, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            //LSIA Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 14, 7, 0), //Ranks
                    new PedComponent(10, 14, 8, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            //Utility
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,209,8),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,2),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,65,9),
               }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {

              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,212,8),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,2),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,65,9),
               }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA ESU Utility>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,209,9),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,12),
              new PedComponent(6,51,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,65,10),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,225,8),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,2),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,9),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,226,8),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,2),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,9),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA ESU Utility>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,225,9),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,10),
               }) },

            //Polos
            new DispatchablePerson("mp_m_freemode_01",2,2) { DebugName = "<Male LSIA Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,14),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,12),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",2,2) {DebugName = "<Female LSIA Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,335,14),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,16,0),
              new PedComponent(5,48,0),
               }) },

            //Jacket
            new DispatchablePerson("mp_m_freemode_01",1,1) { DebugName = "<Male LSIA Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,52,0),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,56,1),
              new PedComponent(4,35,0),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,28,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",1,1) {DebugName = "<Female LSIA Jacket>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,175,0),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,33,0),
              new PedComponent(4,34,0),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,30,0),
              new PedComponent(5,48,0),
               }) },

        };
        List<DispatchablePerson> RHPDCops_FEJ = new List<DispatchablePerson>() {
            //RHPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                } },

            //RHPD Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            //RHPD Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            new DispatchablePerson("mp_m_freemode_01", 5,5){
    MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Jacket>", RandomizeHead = true,
    OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
    RequiredVariation = new PedVariation(
        new List<PedPropComponent>(){

        },
        new List<PedComponent>(){
            new PedComponent(1, 101, 0),
            new PedComponent(11, 143, 0),
            new PedComponent(3, 4, 0),
            new PedComponent(10, 0, 0),
            new PedComponent(8, 56, 1),
            new PedComponent(4, 35, 0),
            new PedComponent(6, 51, 0),
            new PedComponent(7, 8, 0),
            new PedComponent(9, 28, 0),
            new PedComponent(5, 0, 0),
        })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 187, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 72, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 311, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Bicycle Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 49, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 311, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 12, 2),
                        new PedComponent(6, 2, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 209, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 6),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 212, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 6),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Suit>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 178, 13),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 38, 2),
                        new PedComponent(9, 38, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 68, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 1,1){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Detective>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 349, 13),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 10, 2),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Armor Protection>", RandomizeHead = true,MinWantedLevelSpawn = 3,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 13),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 0),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 0, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 50){
                DebugName = "<Male RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 150, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 52, 4),
                        new PedComponent(11, 220, 11),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 6),
                        new PedComponent(5, 48, 0),
                    })},

            new DispatchablePerson("mp_f_freemode_01", 5,5){
            MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Utility Class B>", RandomizeHead = true,
            OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                new List<PedPropComponent>(){

                },
                new List<PedComponent>(){
                    new PedComponent(1, 0, 0),
                    new PedComponent(11, 225, 6),
                    new PedComponent(3, 3, 0),
                    new PedComponent(10, 0, 0),
                    new PedComponent(8, 101, 1),
                    new PedComponent(4, 89, 12),
                    new PedComponent(6, 25, 0),
                    new PedComponent(7, 8, 0),
                    new PedComponent(9, 16, 0),
                    new PedComponent(5, 65, 6),
                })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 6),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 5,5){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 335, 16),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 140, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 189, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Bicycle Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 47, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 335, 16),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 14, 2),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Detective>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 1, 14),
                        new PedComponent(3, 1, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 171, 18),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 40, 0),
                        new PedComponent(5, 0, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Detective (vest)>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 281, 18),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 161, 0),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 40, 0),
                        new PedComponent(5, 0, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0,50){
                DebugName = "<Female RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 11),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 6),
                        new PedComponent(5, 48, 0),
                    })},


        };
        List<DispatchablePerson> DPPDCops_FEJ = new List<DispatchablePerson>() {
            //DPPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                } },

            //DPPD Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            //DPPD Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                } },

            // Jacket
            new DispatchablePerson("mp_m_freemode_01", 1, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 143, 5),
                                                         new PedComponent(3, 4, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 56, 1),
                                                         new PedComponent(4, 35, 0),
                                                         new PedComponent(6, 51, 0),
                                                         new PedComponent(7, 8, 0),
                                                         new PedComponent(9, 28, 0),
                                                         new PedComponent(5, 0, 0),
                                                     })},
            new DispatchablePerson("mp_f_freemode_01", 1, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 140, 5),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 33, 1),
                                                            new PedComponent(4, 34, 0),
                                                            new PedComponent(6, 52, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 30, 0),
                                                            new PedComponent(5, 48, 0),
                                                        })},

            // Polo
            new DispatchablePerson("mp_m_freemode_01", 10, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 311, 17),
                                                            new PedComponent(3, 0, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 12),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 48, 0),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 10, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 335, 17),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 12),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 16, 0),
                                                            new PedComponent(5, 48, 0),
                                                        })},

            // Utility
            new DispatchablePerson("mp_m_freemode_01", 2, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 209, 7),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 2),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 65, 7),
                                                        })},
            new DispatchablePerson("mp_m_freemode_01", 2, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 212, 7),
                                                            new PedComponent(3, 11, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 2),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 65, 7),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 2, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 225, 7),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 2),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 15, 0),
                                                            new PedComponent(5, 65, 7),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 2, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 226, 7),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 2),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 15, 0),
                                                            new PedComponent(5, 65, 7),
                                                        })},

            //SWAT
            new DispatchablePerson("mp_m_freemode_01", 0, 50){
                DebugName = "<Male DPPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 150, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 38, 0),
                        new PedComponent(11, 220, 10),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 7),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 50){
                DebugName = "<Female DPPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 149, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 38, 0),
                        new PedComponent(11, 230, 10),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 7),
                        new PedComponent(5, 48, 0),
                    })},

        };
        List<DispatchablePerson> SheriffPeds_FEJ = new List<DispatchablePerson>() {

            new DispatchablePerson("s_m_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },

            //LSSD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps = 
                new List<PedPropComponent>() { 
                    new PedPropComponent(0,13,0) 
                },
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps =
                new List<PedPropComponent>() {
                    new PedPropComponent(0,13,0)
                },
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 10, 3, 0),
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },

            //LSSD Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 10, 3, 0),
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },

            //LSSD Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps =
                new List<PedPropComponent>() {
                    new PedPropComponent(0,13,0)
                },
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 1, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 2, 0, 0),
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 1, 0, 0), //Ranks
                    new PedComponent(10, 9, 0, 0),
                    new PedComponent(10, 2, 0, 0),
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },

            // Jackets
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male LSSD Coat>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 265, 1),
                                                         new PedComponent(3, 4, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 38, 1),
                                                         new PedComponent(4, 25, 0),
                                                         new PedComponent(6, 51, 0),
                                                         new PedComponent(7, 8, 0),
                                                         new PedComponent(9, 14, 0),
                                                         new PedComponent(5, 64, 0),
                                                     })},
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Coat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 5, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 41, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 1),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 30, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 25, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 1),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 279, 0),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 51, 1),
                                                            new PedComponent(4, 41, 1),
                                                            new PedComponent(6, 52, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 30, 1),
                                                            new PedComponent(5, 48, 0),
                                                        })},
            new DispatchablePerson("mp_m_freemode_01", 1, 0){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 187, 1),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 38, 1),
                                                            new PedComponent(4, 25, 0),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 28, 1),
                                                            new PedComponent(5, 53, 0),
                                                        })},

            // Polo
            new DispatchablePerson("mp_m_freemode_01", 3, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Male LSSD Polo>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 135, 22),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 13),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 3, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Female LSSD Polo>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 134, 22),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 13),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 31, 1),
                        new PedComponent(4, 89, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    })},

            // Swat
            new DispatchablePerson("mp_m_freemode_01", 0, 20){
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4,
                DebugName = "<Male LSSD SWAT Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 150, 1),
                        new PedPropComponent(1, 23, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 2),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 1),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 20){
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4,
                DebugName = "<Female LSSD SWAT Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 2),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 1),
                        new PedComponent(5, 48, 0),
                    })},

            // Detective
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Armor Protection>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(6, 32, 1),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 292, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 4),
                        new PedComponent(4, 22, 3),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 23, 1),
                        new PedComponent(5, 0, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Raid Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 325, 1),
                                                            new PedComponent(3, 12, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 179, 15),
                                                            new PedComponent(4, 22, 3),
                                                            new PedComponent(6, 20, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 17, 3),
                                                            new PedComponent(5, 60, 1),
                                                        })},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Gang Detail Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 325, 1),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 17),
                                                            new PedComponent(4, 4, 1),
                                                            new PedComponent(6, 63, 4),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 17, 3),
                                                            new PedComponent(5, 60, 1),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Armor Protection>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 366, 12),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 161, 4),
                                                            new PedComponent(4, 3, 4),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 25, 1),
                                                            new PedComponent(5, 0, 0),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Raid Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 318, 1),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 217, 12),
                                                            new PedComponent(4, 3, 63),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 21, 0),
                                                            new PedComponent(5, 60, 1),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Plain Clothes>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 73, 1),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 14, 0),
                                                            new PedComponent(4, 5, 9),
                                                            new PedComponent(6, 62, 23),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 25, 1),
                                                            new PedComponent(5, 68, 0),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Gang Detail Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 318, 1),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 57, 1),
                                                            new PedComponent(4, 5, 9),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 21, 3),
                                                            new PedComponent(5, 60, 1),
                                                        })},
        };
        List<DispatchablePerson> BCSheriffPeds_FEJ = new List<DispatchablePerson>() {
            // BCSO Class A
            new DispatchablePerson("mp_m_freemode_01", 20, 20){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 200, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = new List<PedPropComponent>(){new PedPropComponent(0, 13,
                                                                                  1)},
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 11, 1, 0),  // Ranks
                        new PedComponent(10, 11, 4, 0), new PedComponent(10, 45, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},
            new DispatchablePerson("mp_f_freemode_01", 20, 20){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 202, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = new List<PedPropComponent>(){new PedPropComponent(0, 13,
                                                                                  1)},
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 10, 1, 0),  // Ranks
                        new PedComponent(10, 10, 4, 0), new PedComponent(10, 53, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},

            // BCSO Class B
            new DispatchablePerson("mp_m_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 193, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 11, 1, 0),  // Ranks
                        new PedComponent(10, 11, 4, 0), new PedComponent(10, 45, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},
            new DispatchablePerson("mp_f_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 195, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 10, 1, 0),  // Ranks
                        new PedComponent(10, 10, 4, 0), new PedComponent(10, 53, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},

            // BCSO Class C
            new DispatchablePerson("mp_m_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 190, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 15, 7, 0),  // Ranks
                        new PedComponent(10, 15, 8, 0), new PedComponent(10, 44, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},
            new DispatchablePerson("mp_f_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 192, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 14, 7, 0),  // Ranks
                        new PedComponent(10, 14, 8, 0), new PedComponent(10, 52, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},

            // Detective (with armor (male))
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3, DebugName = "<Male BCSO Armor Protection>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(6, 32, 1),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 107, 0),
                        new PedComponent(11, 292, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 132, 5),
                        new PedComponent(4, 22, 5),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 119, 1),
                        new PedComponent(9, 23, 4),
                        new PedComponent(5, 29, 8),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3, DebugName = "<Female BCSO Suit>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 6, 15),
                                                         new PedComponent(3, 5, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 20, 0),
                                                         new PedComponent(4, 23, 5),
                                                         new PedComponent(6, 13, 0),
                                                         new PedComponent(7, 0, 0),
                                                         new PedComponent(9, 0, 0),
                                                         new PedComponent(5, 60, 0),
                                                     })},

            // SWAT
            new DispatchablePerson("mp_m_freemode_01", 0, 20){
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, DebugName = "<Male BCSO SWAT Uniform>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 150, 1),
                        new PedPropComponent(1, 21, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 3),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 20){
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, DebugName = "<Female BCSO SWAT Uniform>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 3),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 0),
                        new PedComponent(5, 48, 0),
                    })},
        };
        List<DispatchablePerson> NOOSEPeds_FEJ = new List<DispatchablePerson>() {
            //PIA Peds
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 10),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 10),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 5),
                        new PedComponent(5, 48, 0),
                    })},

            //PIA TRU 
            new DispatchablePerson("mp_m_freemode_01",0,50) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, DebugName = "<Male PIA TRU Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,39,1),
              new PedPropComponent(1,25,4),//new PedPropComponent(1,26,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),//new PedComponent(1,56,1),//new PedComponent(1,122,0),
              new PedComponent(11,220,8),
              new PedComponent(3,141,19),
              new PedComponent(10,0,0),
              new PedComponent(8,15,0),
              new PedComponent(4,37,2),
              new PedComponent(6,25,0),
              new PedComponent(7,110,0),
              new PedComponent(9,25,5),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",0,50) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,DebugName = "<Female PIA TRU Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,38,1),
              new PedPropComponent(1,27,2),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),//new PedComponent(1,55,0),
              new PedComponent(11,230,8),
              new PedComponent(3,174,19),
              new PedComponent(10,0,0),
              new PedComponent(8,14,0),
              new PedComponent(4,36,2),
              new PedComponent(6,25,0),
              new PedComponent(7,81,0),
              new PedComponent(9,27,5),
              new PedComponent(5,48,0),
               }) },
        };
        List<DispatchablePerson> FIBPeds_FEJ = new List<DispatchablePerson>() {

            //Suits
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 2, DebugName = "<Male FIB Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,31,1), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,293,0),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,16,0),
              new PedComponent(4,10,0),
              new PedComponent(6,10,0),
              new PedComponent(7,38,8),
              new PedComponent(9,22,0),
              new PedComponent(5,28,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",20,20) {MaxWantedLevelSpawn = 2, DebugName = "<Female FIB Agent>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,27,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,9,0),
              new PedComponent(4,3,0),
              new PedComponent(6,29,0),
              new PedComponent(7,0,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }) },

            //Suits (with armor)
            new DispatchablePerson("mp_m_freemode_01",0,40) { MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,0),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,6),
              new PedComponent(4,10,0),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",0,40) { MaxWantedLevelSpawn = 3, DebugName = "<Female FIB Response>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,366,0),
              new PedComponent(3,7,0),
              new PedComponent(10,0,0),
              new PedComponent(8,161,6),
              new PedComponent(4,3,0),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }) },

            //Vanilla Like (male only)
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Field Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,31,2), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,88,0),
              new PedComponent(4,47,0),
              new PedComponent(6,25,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Field Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,3),
              new PedComponent(3,6,0),
              new PedComponent(10,0,0),
              new PedComponent(8,28,3),
              new PedComponent(4,47,0),
              new PedComponent(6,25,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }) },

            //SWAT
            new DispatchablePerson("mp_m_freemode_01",0,50) { GroupName = "FIBHRT",MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, DebugName = "<Male FIB SWAT Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,39,0),
              new PedPropComponent(1,21,0),
              }, new List<PedComponent>() {
              new PedComponent(1,122,0),
              new PedComponent(11,220,5),
              new PedComponent(3,179,0),
              new PedComponent(10,0,0),
              new PedComponent(8,116,0),
              new PedComponent(4,31,4),
              new PedComponent(6,35,0),
              new PedComponent(7,110,0),
              new PedComponent(9,25,3),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",0,50) {GroupName = "FIBHRT",MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, DebugName = "<Female FIB SWAT Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,38,0),
              new PedPropComponent(1,22,0),
              }, new List<PedComponent>() {
              new PedComponent(1,122,0),
              new PedComponent(11,230,5),
              new PedComponent(3,215,0),
              new PedComponent(10,0,0),
              new PedComponent(8,14,0),
              new PedComponent(4,30,4),
              new PedComponent(6,36,0),
              new PedComponent(7,81,0),
              new PedComponent(9,27,3),
              new PedComponent(5,48,0),
               }) },

            };
        List<DispatchablePerson> ParkRangers_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_ranger_01",0,0) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}, OptionalPropChance = 70},
            new DispatchablePerson("s_f_y_ranger_01",0,0) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }, OptionalPropChance = 70 },

            //Rangers
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,4,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,200,5),
          new PedComponent(3,4,0),
          new PedComponent(10,0,0),
          new PedComponent(8,49,1),
          new PedComponent(4,86,8),
          new PedComponent(6,51,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,36,0),
           }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,4,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,193,5),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,13,0),
              new PedComponent(5,36,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,190,5),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,13,0),
              new PedComponent(5,36,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,135,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,9),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
             new PedPropComponent(0,135,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,21,0),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,28,4),
              new PedComponent(5,48,0),
             }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,4,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,202,5),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,195,5),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,192,5),
              new PedComponent(3,9,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,134,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,335,9),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Jacket>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,134,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,19,0),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,10),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,30,4),
              new PedComponent(5,48,0),
               }) },

        };
        List<DispatchablePerson> DOAPeds_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",0,0),
            new DispatchablePerson("mp_m_freemode_01",10,0) { DebugName = "<Male DOA Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,37,2), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,4),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,88,0),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",15,0) {DebugName = "<Female DOA Agent>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,27,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,7,0),
              new PedComponent(4,3,2),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,50,0),
              new PedComponent(5,0,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",0,50) { DebugName = "<Male DOA Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,37,2), }, new List<PedComponent>() {
              new PedComponent(1,121,0),
              new PedComponent(11,292,4),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,8),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_m_freemode_01",25,0) { DebugName = "<Male DOA Windbreaker>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,4),
              new PedComponent(3,12,0),
              new PedComponent(10,0,0),
              new PedComponent(8,179,4),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,0,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",50,50) {DebugName = "<Female DOA Windbreaker>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,318,4),
              new PedComponent(3,7,0),
              new PedComponent(10,0,0),
              new PedComponent(8,39,0),
              new PedComponent(4,3,2),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,50,0),
              new PedComponent(5,0,0),
               }) },
        };
        List<DispatchablePerson> SAHPPeds_FEJ = new List<DispatchablePerson>() {

            //SAHP Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,31,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,30,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Class B
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 14, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Class C
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Motorcycle Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Motorcycle Class B
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Motorcycle Class C
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } },

            //SAHP Jacket
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 19, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 19, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,31,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 28, 2, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 31, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 30, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,30,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 30, 2, 0), //Ranks
                } },

            //SAHP SWAT
            new DispatchablePerson("mp_m_freemode_01",0,0) { RandomizeHead = true,MinWantedLevelSpawn = 99,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 52, 3, 0),
                        new PedComponent(3, 141,18, 0),
                        new PedComponent(4, 37, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 35, 1, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 25, 2, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 7, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,39,1)  }),
            },
            new DispatchablePerson("mp_f_freemode_01",0,0) { RandomizeHead = true,MinWantedLevelSpawn = 99,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 52, 3, 0),
                        new PedComponent(3, 174, 18, 0),
                        new PedComponent(4, 36, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 27, 2, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 7, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,38,1),new PedPropComponent(1,25,0)  }),
             },

        };
        List<DispatchablePerson> MilitaryPeds_FEJ = new List<DispatchablePerson>() {
            //Vanilla Peds
            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_m_pilot_02",0,0),
            new DispatchablePerson("s_m_y_pilot_01",0,0),
            new DispatchablePerson("s_m_y_marine_03",100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)}) },
        
            ////EUP
            //new DispatchablePerson("mp_m_freemode_01", 0, 70, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { MinWantedLevelSpawn = 6, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
            //    RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {
            //            new PedComponent(1, 0, 0, 0),
            //            new PedComponent(3, 141, 19, 0),
            //            new PedComponent(4, 37, 2, 0),
            //            new PedComponent(5, 48, 0, 0),
            //            new PedComponent(6, 35, 0, 0),
            //            new PedComponent(7, 42, 0, 0),
            //            new PedComponent(8, 15, 0, 0),
            //            new PedComponent(9, 15, 0, 0),
            //            new PedComponent(10, 0, 0, 0),
            //            new PedComponent(11, 220, 13, 0)},//13?//6?//listed as 25, but that texture is BROKE
            //        new List<PedPropComponent>() { new PedPropComponent(0,39,1),new PedPropComponent(1,23,0)  }),
            //},
            //new DispatchablePerson("mp_f_freemode_01", 0, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { MinWantedLevelSpawn = 6, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
            //    RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {
            //            new PedComponent(1, 0, 0, 0),
            //            new PedComponent(3, 174,19, 0),
            //            new PedComponent(4, 36, 2, 0),
            //            new PedComponent(5, 48, 0, 0),
            //            new PedComponent(6, 36, 0, 0),
            //            new PedComponent(7, 29, 0, 0),
            //            new PedComponent(8, 14, 0, 0),
            //            new PedComponent(9, 17, 0, 0),
            //            new PedComponent(10, 0, 0, 0),
            //            new PedComponent(11, 230, 25, 0)},//listed as 25, but that texture is BROKE
            //        new List<PedPropComponent>()  {new PedPropComponent(0,38,1),new PedPropComponent(1,25,0)   }),
            //},

        };
        List<DispatchablePerson> PrisonPeds_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",0,0),

            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,200,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,154,0),
            new PedComponent(4,25,2),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,193,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,190,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,139,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,311,8),
            new PedComponent(3,0,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,48,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,202,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,190,0),
            new PedComponent(4,41,0),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,195,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,192,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,138,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,335,8),
            new PedComponent(3,14,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,25,0),
            new PedComponent(7,0,0),
            new PedComponent(9,16,0),
            new PedComponent(5,48,0),
            }) },

            //With Armor
            new DispatchablePerson("mp_m_freemode_01",0,25) { MinWantedLevelSpawn = 3,DebugName = "<Male SASPA Armed Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,193,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,8,0),
            new PedComponent(9,20,8),
            new PedComponent(5,48,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",0,25) { MinWantedLevelSpawn = 3,DebugName = "<Male SASPA Armed Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,190,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,8,0),
            new PedComponent(9,20,8),
            new PedComponent(5,48,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,25) { MinWantedLevelSpawn = 3, DebugName = "<Female SASPA Armed Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,195,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,51,1),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,8,0),
            new PedComponent(9,23,8),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,25) { MinWantedLevelSpawn = 3, DebugName = "<Female SASPA Armed Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,192,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,51,1),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,8,0),
            new PedComponent(9,23,8),
            new PedComponent(5,33,0),
            }) },

        };
        List<DispatchablePerson> LSPPPeds_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
            DebugName = "<Male LSPP Class A>", RandomizeHead = true,
            OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                new List<PedPropComponent>(){

                },
                new List<PedComponent>(){
                    new PedComponent(1, 0, 0),
                    new PedComponent(11, 200, 8),
                    new PedComponent(3, 4, 0),
                    new PedComponent(10, 0, 0),
                    new PedComponent(8, 56, 1),
                    new PedComponent(4, 35, 0),
                    new PedComponent(6, 51, 0),
                    new PedComponent(7, 8, 0),
                    new PedComponent(9, 0, 0),
                    new PedComponent(5, 32, 0),
                })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 8),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 135, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 8),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 212, 10),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 8),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 8),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 8),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 10),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 8),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 15),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 8),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 8),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 8),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 8),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Harbor Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 135, 21),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 87, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 8, 1),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Harbor Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 134, 21),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 90, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 9, 1),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 20, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                DebugName = "<Male LSPP Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 32, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 174, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                DebugName = "<Female LSPP Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 189, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Plain Clothes>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 2, 9),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 10, 5),
                        new PedComponent(6, 3, 1),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 41, 0),
                        new PedComponent(5, 0, 0),
                    })},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Plain clothes>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 14, 8),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                    })},
        };
        List<DispatchablePerson> LSPDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",100,100) { GroupName = "Pilot", DebugName = "<Male LSPD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,79,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,108,0),
            new PedComponent(3,16,0),
            new PedComponent(10,0,0),
            new PedComponent(8,67,0),
            new PedComponent(4,64,0),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,48,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",100,100) { GroupName = "Pilot", DebugName = "<Female LSPD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,78,1),
            new PedPropComponent(1,13,0),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,99,0),
            new PedComponent(3,17,0),
            new PedComponent(10,0,0),
            new PedComponent(8,49,0),
            new PedComponent(4,66,0),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,48,0),
            }) },
        };
        List<DispatchablePerson> LSSDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",0,0) { DebugName = "<Male LSSD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,79,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,108,1),
          new PedComponent(3,96,0),
          new PedComponent(10,0,0),
          new PedComponent(8,43,1),
          new PedComponent(4,64,1),
          new PedComponent(6,24,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) {DebugName = "<Female LSSD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,78,0),
          new PedPropComponent(1,13,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,99,1),
          new PedComponent(3,36,0),
          new PedComponent(10,0,0),
          new PedComponent(8,30,0),
          new PedComponent(4,66,1),
          new PedComponent(6,24,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }) },
        };

        //Cops
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("StandardCops", StandardCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSIAPDPeds", LSIAPDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RHPDCops", RHPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DPPDCops", DPPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCSheriffPeds", BCSheriffPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPPPeds", LSPPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds_FEJ));

        //Gangs
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\DispatchablePeople_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchablePeople_EUP.xml");

    }
}

