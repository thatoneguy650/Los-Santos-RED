using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    private List<DispatchablePerson> LSLifeguardPeds;
    private List<DispatchablePerson> LSPDASDPeds;
    private List<DispatchablePerson> LSSDASDPeds;

    public List<DispatchablePerson> ArmyPeds { get; private set; }
    public List<DispatchablePerson> USMCPeds { get; private set; }
    public List<DispatchablePerson> USAFPeds { get; private set; }

    private List<DispatchablePerson> PrisonPeds;
    private List<DispatchablePerson> MarshalsServicePeds;
    public List<DispatchablePerson> OffDutyCops { get; private set; }
    public List<DispatchablePerson> SecurityPeds { get; private set; }

    private List<DispatchablePerson> GruppeSechsPeds;
    private List<DispatchablePerson> SecuroservPeds;
    private List<DispatchablePerson> MerryweatherSecurityPeds;
    private List<DispatchablePerson> BobcatPeds;

    public List<DispatchablePerson> CoastGuardPeds { get; private set; }
    public List<DispatchablePerson> NYSPPeds { get; private set; }
    public List<DispatchablePerson> Firefighters { get; private set; }
    public List<DispatchablePerson> EMTs { get; private set; }
    public List<DispatchablePerson> GreenEMTs { get; private set; }
    public List<DispatchablePerson> BlueEMTs { get; private set; }

    public List<DispatchablePerson> VagosPeds { get; private set; }

    public List<DispatchablePerson> FamiliesPeds { get; private set; }
    public List<DispatchablePerson> BallasPeds { get; private set; }
    public List<DispatchablePerson> MarabuntaPeds { get; private set; }
    public List<DispatchablePerson> AltruistPeds { get; private set; }

    public List<DispatchablePerson> TriadsPeds { get; private set; }
    public List<DispatchablePerson> KoreanPeds { get; private set; }
    public List<DispatchablePerson> RedneckPeds { get; private set; }
    public List<DispatchablePerson> ArmenianPeds { get; private set; }
    public List<DispatchablePerson> CartelPeds { get; private set; }
    public List<DispatchablePerson> YardiesPeds { get; private set; }

    private List<DispatchablePerson> SpanishLordsPeds;
    public List<DispatchablePerson> OtherPeds { get; private set; }
    public List<DispatchablePerson> TaxiDrivers { get; private set; }
    public List<DispatchablePerson> VendorPeds { get; private set; }
    public List<DispatchablePerson> IllicitMarketplacePeds { get; private set; }
    public List<DispatchablePerson> TellerPeds { get; private set; }
    public List<DispatchablePerson> BarPeds { get; private set; }
    public List<DispatchablePerson> HaircutPeds { get; private set; }
    public List<DispatchablePerson> BobMuletPeds { get; private set; }
    public List<DispatchablePerson> BurgerShotPeds { get; private set; }

    public List<DispatchablePerson> CluckinBellPeds { get; private set; }
    public List<DispatchablePerson> TwatPeds { get; private set; }
    public List<DispatchablePerson> GunshopPeds { get; private set; }

    public List<DispatchablePerson> RegularPeds { get; private set; }
    public List<DispatchablePerson> VehicleRacePeds { get; private set; }
    private List<DispatchablePerson> StandardCops_Old;
    private List<DispatchablePerson> FIBPeds_Old;
    private List<DispatchablePerson> NOOSEPeds_Old;
    private List<DispatchablePerson> SheriffPeds_Old;

    private List<DispatchablePerson> StandardCops_Simple;
    private List<DispatchablePerson> FIBPeds_Simple;
    private List<DispatchablePerson> NOOSEPeds_Simple;
    private List<DispatchablePerson> MarshalsServicePeds_Simple;
    private List<DispatchablePerson> PrisonPeds_Simple;
    private List<DispatchablePerson> SecurityPeds_Simple;
    private List<DispatchablePerson> GruppeSechsPeds_Simple;
    private List<DispatchablePerson> SecuroservPeds_Simple;
    private List<DispatchablePerson> BobcatPeds_Simple;
    private List<DispatchablePerson> MerryweatherSecurityPeds_Simple;

    private DispatchablePeople_LostMC DispatchablePeople_LostMC;
    private DispatchablePeople_Mafia DispatchablePeople_Mafia;
    private DispatchablePeople_Diablos DispatchablePeople_Diablos;
    private DispatchablePeople_Varrios DispatchablePeople_Varrios;
    private DispatchablePeople_AngelsOfDeath DispatchablePeople_AngelsOfDeath;
    private DispatchablePeople_UptownRiders DispatchablePeople_UptownRiders;

    private DispatchablePeople_NorthHolland DispatchablePeople_NorthHolland;
    private DispatchablePeople_Petrovic DispatchablePeople_Petrovic;
    private DispatchablePeople_Cops DispatchablePeople_Cops;
    private int optionalpropschance;
    private DispatchablePeople_AngelsOfDeath DispatchablePeople_AngelsOfDeath_LS;

    public List<string> GeneralMaleVoices { get; private set; } = new List<string>() { "A_M_M_GENERICMALE_01_WHITE_MINI_01", "A_M_M_SALTON_01_WHITE_FULL_01", "A_M_M_TOURIST_01_WHITE_MINI_01", "A_M_M_MALIBU_01_LATINO_FULL_01" };
    public List<string> GeneralFemaleVoices { get; private set; } = new List<string>() { "A_F_Y_FITNESS_01_WHITE_FULL_01", "A_F_Y_BEVHILLS_01_WHITE_FULL_01", "A_F_Y_SOUCENT_01_BLACK_FULL_01", "A_F_Y_TOURIST_01_WHITE_FULL_01" };

    public List<string> GeneralMaleCopVoices { get; private set; } = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" };

    public List<string> GeneralFemaleCopVoices { get; private set; } = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" };

    public List<DispatchablePerson> MafiaPeds { get; set; }
    public List<DispatchablePerson> LostMCPeds { get; set; }
    public List<DispatchablePerson> DiablosPeds { get; set; }
    public List<DispatchablePerson> VarriosPeds { get; set; }

    public List<DispatchablePerson> AngelsOfDeathPeds { get; set; }

    public List<DispatchablePerson> AngelsOfDeathPeds_LS { get; set; }

    public List<DispatchablePerson> UptownRidersPeds { get; set; }


    public List<DispatchablePerson> PetrovicPeds { get; set; }
    public List<DispatchablePerson> NorthHollandPeds { get; set; }

    public List<DispatchablePersonGroup> AllPeople => PeopleGroupLookup;
    public void Setup(IIssuableWeapons issuableWeapons)
    {
        foreach (DispatchablePersonGroup dpg in PeopleGroupLookup)
        {
            if (dpg.DispatchablePeople == null)
            {
                continue;
            }
            foreach (DispatchablePerson dispatchPerson in dpg.DispatchablePeople)
            {
                dispatchPerson.Setup(issuableWeapons);

            }
        }
    }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "DispatchablePeople_*.xml" : $"DispatchablePeople_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable People config: {ConfigFile.FullName}", 0);
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable People config:  {ConfigFileName}", 0);
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Dispatchable People config found, creating default", 0);
            SetupDefault();
            DefaultConfig_LosSantos2008();
            DefaultConfig_Simple();
            DefaultConfig_SunshineDream();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig();
            DefaultConfig_LibertyCity();
            DefaultConfig_LPP();
        }
        //Load Additive
        foreach (FileInfo fileInfo in LSRDirectory.GetFiles("DispatchablePeople+_*.xml").OrderByDescending(x => x.Name))
        {
            EntryPoint.WriteToConsole($"Loaded ADDITIVE Dispatchable People config  {fileInfo.FullName}", 0);
            List<DispatchablePersonGroup> additivePossibleItems = Serialization.DeserializeParams<DispatchablePersonGroup>(fileInfo.FullName);
            PeopleGroupLookup.RemoveAll(x => additivePossibleItems.Any(y=> y.DispatchablePersonGroupID == x.DispatchablePersonGroupID));
            PeopleGroupLookup.AddRange(additivePossibleItems);
        }
    }

    private void DefaultConfig_FullExpandedJurisdiction()
    {
        DispatchablePeople_EUP dispatchablePeople_EUP = new DispatchablePeople_EUP(this);
        dispatchablePeople_EUP.DefaultConfig();
    }

    public List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID)
    {
        return PeopleGroupLookup.FirstOrDefault(x => x.DispatchablePersonGroupID == dispatchablePersonGroupID)?.DispatchablePeople;
    }
    private void SetupDefault()
    {
        // Game.DisplaySubtitle(Game.LocalPlayer.Character.SeatIndex.ToString());

        DispatchablePeople_Cops = new DispatchablePeople_Cops(this);

        optionalpropschance = 20;


        List<PedPropComponent> MaleCopShortSleeveOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 8, 3),
            new PedPropComponent(1, 8, 5),
            new PedPropComponent(1, 8, 6),
            new PedPropComponent(1, 7, 0),
            new PedPropComponent(1, 2, 3),

            new PedPropComponent(6, 3, 0),
        };

        List<PedPropComponent> FemaleCopShortSleeveOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 39, 0),
            new PedPropComponent(1, 40, 0),
            new PedPropComponent(1, 11, 0),
            new PedPropComponent(1, 11, 1),
            new PedPropComponent(1, 11, 3),
            new PedPropComponent(1, 24, 0),

            new PedPropComponent(6, 20, 2),
        };

        DispatchablePerson GenericK9 = new DispatchablePerson("a_c_husky", 50, 50)
        {
            IsAnimal = true,
            DebugName = "K9_Husky",
            UnitCode = "K9",
            RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>()),
            OverrideAgencyLessLethalWeapons = true,
            OverrideAgencySideArms = true,
            OverrideAgencyLongGuns = true,
            OverrideLessLethalWeaponsID = null,
            OverrideSideArmsID = null,
            OverrideLongGunsID = null,
        };




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

            //Regular
            DispatchablePeople_Cops.CreateLSPDMPPed(60,60,0,2,true,true,false),
            DispatchablePeople_Cops.CreateLSPDMPPed(60,60,0,2,true,false,false),

            DispatchablePeople_Cops.CreateLSPDMPPed(40,40,0,2,false,true,false),
            DispatchablePeople_Cops.CreateLSPDMPPed(40,40,0,2,false,false,false),

            //Armored
            DispatchablePeople_Cops.CreateLSPDMPPed(0,60,3,6,true,true,true),
            DispatchablePeople_Cops.CreateLSPDMPPed(0,60,3,6,true,false,true),

            DispatchablePeople_Cops.CreateLSPDMPPed(0,40,3,6,false,true,true),
            DispatchablePeople_Cops.CreateLSPDMPPed(0,40,3,6,false,false,true),

            //Motorcycle
            DispatchablePeople_Cops.CreateLSPDMotorcycleMPPed(0,0,0,2,true,true),
            DispatchablePeople_Cops.CreateLSPDMotorcycleMPPed(0,0,0,2,true,false),

            DispatchablePeople_Cops.CreateLSPDMotorcycleMPPed(0,0,0,2,false,true),
            DispatchablePeople_Cops.CreateLSPDMotorcycleMPPed(0,0,0,2,false, false),


            //new DispatchablePerson("mp_f_freemode_01",40,40) {
            //    DebugName = "LSPDMPNoArmorFemale"
            //    ,RandomizeHead = true
            //    ,MaxWantedLevelSpawn = 2
            //    ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
            //    ,RequiredVariation = new PedVariation(
            //        new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0)},
            //        new List<PedPropComponent>() {  })
            //    ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9),new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
            //    ,OptionalPropChance = optionalpropschance
            //},//no body armor with hat and glasses possible
            //new DispatchablePerson("mp_f_freemode_01",0,40) {
            //    DebugName = "LSPDMPArmorFemale"
            //    ,RandomizeHead = true
            //    ,MinWantedLevelSpawn = 3
            //    ,ArmorMin = 30
            //    ,ArmorMax = 50
            //    ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
            //    ,RequiredVariation = new PedVariation(
            //        new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 3, 0), new PedComponent(11, 48, 0, 0)},
            //        new List<PedPropComponent>() {  })
            //    ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 15), new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
            //    ,OptionalPropChance = optionalpropschance
            //},//body armor, with riot helmet and glasses possible

            //new DispatchablePerson("mp_m_freemode_01",60,60) {
            //    DebugName = "LSPDMPNoArmorMale"
            //    ,RandomizeHead = true
            //    ,MaxWantedLevelSpawn = 2
            //    ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
            //    ,RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
            //        new List<PedPropComponent>() { })
            //    ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9),new PedPropComponent(1, 37, 0),new PedPropComponent(1, 38, 0),new PedPropComponent(1, 8, 3),new PedPropComponent(1, 8, 5),new PedPropComponent(1, 8, 6),new PedPropComponent(1, 7, 0),new PedPropComponent(1, 2, 3),new PedPropComponent(6, 3, 0), }
            //    ,OptionalPropChance = optionalpropschance
            //},
            //new DispatchablePerson("mp_m_freemode_01",0,60) {
            //    DebugName = "LSPDMPArmorMale"
            //    ,RandomizeHead = true
            //    ,MinWantedLevelSpawn = 3
            //    ,ArmorMin = 50
            //    ,ArmorMax = 50
            //    ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
            //    ,RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
            //        new List<PedPropComponent>() { })
            //    ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 124, 15), new PedPropComponent(1, 23, 9), new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), new PedPropComponent(6, 3, 0), }
            //    ,OptionalPropChance = optionalpropschance
            //},



            new DispatchablePerson("mp_m_freemode_01",2,2) {
                DebugName = "LSPDMPDetectiveMale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0),new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), }
                ,OptionalPropChance = optionalpropschance
            },

            new DispatchablePerson("mp_f_freemode_01",2,2) {
                DebugName = "LSPDMPDetectiveFemale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
                ,OptionalPropChance = optionalpropschance
            },


            GenericK9,



            new DispatchablePerson("mp_m_freemode_01",0,0) {
                DebugName = "LSPDMPSniperArmorMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 124, 15), new PedPropComponent(1, 23, 9) }
                ,OptionalPropChance = 10
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = 65
                ,AccuracyMax = 85
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
            new DispatchablePerson("s_m_y_pilot_01",0,0) { DebugName = "Police and LSPD Labeled Pilot",GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } }, //HAS LSPD STUFF ON HIM!!!!

        };
        SheriffPeds = new List<DispatchablePerson>() {

            GetGenericMPDetectivePed(2,2,2,true),
            GetGenericMPDetectivePed(2,2,2,false),

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
            new DispatchablePerson("s_f_y_sheriff_01",0,45) {
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
            GenericK9,
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },

        };
        NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 0,0){
                DebugName = "NOOSEDefaultMale"
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },

            //Geneirc LSPD Style Cops


            GetGenericMPCopPed(30,30,3,true, true),
            GetGenericMPCopPed(30,30,3,true, false),
            GetGenericMPCopPed(20,20,3,false, true),
            GetGenericMPCopPed(20,20,3,false, false),

            GetGenericMPDetectivePed(5,1,2,true),
            GetGenericMPDetectivePed(5,1,2,false),


            new DispatchablePerson("mp_m_freemode_01", 0,100) {
                DebugName = "NOOSEMPMaleSWAT1"
                ,MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0),new PedPropComponent(1, 23, 9)})
            },
            new DispatchablePerson("mp_m_freemode_01", 0,100) {
                DebugName = "NOOSEMPMaleSWAT2"
                ,MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0)})
            },

            new DispatchablePerson("mp_f_freemode_01", 0,50) {
                DebugName = "NOOSEMPFemaleSWAT1"
                ,MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0),new PedPropComponent(1,25,9) })
            },
            new DispatchablePerson("mp_f_freemode_01", 0,50) {
                DebugName = "NOOSEMPFemaleSWAT2"
                ,MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0)})
            },

            new DispatchablePerson("mp_m_freemode_01", 100,100, 100, 100, 100, 100, 65, 85, 400, 500, 2, 2) {
                DebugName = "NOOSEMPMaleSniper"
                ,GroupName = "Sniper"
                ,RandomizeHead = true
                ,MinWantedLevelSpawn = 3
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0)})
                    ,CombatRange = 3
                ,CombatMovement = 0
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
            GenericK9,
        };
        FIBPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){ DebugName = "FIBAgentMale",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){ DebugName = "FIBOfficeMale1",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){ DebugName = "FIBOfficeMale2",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) { DebugName = "FIBOfficeMale3",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0,0) {
                DebugName = "FIBSWATMale"
                ,GroupName = "FIBHET"
                ,AllowRandomizeBeforeVariationApplied = true
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },

            new DispatchablePerson("mp_m_freemode_01", 25, 25) {
                DebugName = "FIBMPMale1"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },//328 cool too
                    new List<PedPropComponent>() { new PedPropComponent(0, 19, 0),new PedPropComponent(1, 15, 9), })
            },
            new DispatchablePerson("mp_m_freemode_01", 75,75) {
                DebugName = "FIBMPMale2"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 117, 0),new PedPropComponent(1, 25, 4), })
            },

            new DispatchablePerson("mp_f_freemode_01", 50,50) {
                DebugName = "FIBMPFemale1"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(2, 42, 0, 0),new PedComponent(3, 18, 0, 0),new PedComponent(4, 130, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(8, 0, 0, 0),new PedComponent(9, 18, 2, 0),new PedComponent(11, 54, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 19, 0), })
            },
            new DispatchablePerson("mp_f_freemode_01", 50,50) {
                DebugName = "FIBMPFemale2"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
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
            new DispatchablePerson("s_m_y_ranger_01",60,60) {
                DebugName = "ParkRangerMale"
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}
                ,OptionalPropChance = 70
            },
            new DispatchablePerson("s_f_y_ranger_01",40,40) {
                DebugName = "ParkRangerFemale"
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }
                ,OptionalPropChance = 70
            },



            DispatchablePeople_Cops.CreateSAPRMPPed(60,60,0,3,true,true),
            DispatchablePeople_Cops.CreateSAPRMPPed(60,60,0,3,true,false),

            DispatchablePeople_Cops.CreateSAPRMPPed(40,40,0,3,false,true),
            DispatchablePeople_Cops.CreateSAPRMPPed(40,40,0,3,false,false),

        };
        DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) { DebugName = "DOAAgentMale" },
        };
        SAHPPeds = new List<DispatchablePerson>() {

            GetGenericMPDetectivePed(2,2,2,true),
            GetGenericMPDetectivePed(2,2,2,false),

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
            //Male
            //Regular
            DispatchablePeople_Cops.CreateSAHPMPPed(25,25,0,3,true,false,false,false),         
            DispatchablePeople_Cops.CreateSAHPMPPed(25,25,0,3,true,true,false,false),

            //Armored
            DispatchablePeople_Cops.CreateSAHPMPPed(0,75,3,3,true,false,false,false),
            DispatchablePeople_Cops.CreateSAHPMPPed(0,75,3,3,true,true,false,false),

            //Female
            //Regular 
            DispatchablePeople_Cops.CreateSAHPMPPed(25,25,0,3,false,false,false,false),
            DispatchablePeople_Cops.CreateSAHPMPPed(25,25,0,3,false,true,false,false),

            //Armored
            DispatchablePeople_Cops.CreateSAHPMPPed(0,75,3,3,false,false,true,false),
            DispatchablePeople_Cops.CreateSAHPMPPed(0,75,3,3,false,true,true,false),

            //Motorcycle
            //Male
            DispatchablePeople_Cops.CreateSAHPMPPed(0,0,0,2,true,false,false,true),
            DispatchablePeople_Cops.CreateSAHPMPPed(0,0,0,2,true,true,false,true),
            //Female
            DispatchablePeople_Cops.CreateSAHPMPPed(0,0,0,2,false,false,false,true),    
            DispatchablePeople_Cops.CreateSAHPMPPed(0,0,0,2,false,true,false,true),

            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
        };
        ArmyPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_armymech_01",10,10) { DebugName = "Army Mechanic", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_m_marine_01",10,10) { DebugName = "Military_BDUOnly",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_marine_02",10,0) { DebugName = "Military_Dress", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_01",10,10) { DebugName = "Military_CombatShirtOnly", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_marine_02",10,1) { DebugName = "Military_NoShirt", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_03",100,100) 
            {
                DebugName = "Military_FullGear"
                ,AccuracyMin = 25
                ,AccuracyMax = 35
                ,ShootRateMin = 450
                ,ShootRateMax = 550
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 150
                ,MaxWantedLevelSpawn = 10
                ,AllowRandomizeBeforeVariationApplied = true
                ,FiringPatternHash = -957453492//fullauto
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)})
            },
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },






        };
        USMCPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_marine_01",10,10) { DebugName = "Military_BDUOnly",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_marine_02",10,0) { DebugName = "Military_Dress", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_01",10,10) { DebugName = "Military_CombatShirtOnly", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_marine_02",10,1) { DebugName = "Military_NoShirt", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_03",100,100) 
            {
                DebugName = "Military_FullGear"
                ,AccuracyMin = 25
                ,AccuracyMax = 35
                ,ShootRateMin = 450
                ,ShootRateMax = 550
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 150
                ,MaxWantedLevelSpawn = 10
                ,AllowRandomizeBeforeVariationApplied = true
                ,FiringPatternHash = -957453492//fullauto
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)})
            },
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },            
        };
        USAFPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_marine_01",10,10) { DebugName = "Military_BDUOnly",MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_marine_02",10,0) { DebugName = "Military_Dress", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_01",10,10) { DebugName = "Military_CombatShirtOnly", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_marine_02",10,1) { DebugName = "Military_NoShirt", MaxWantedLevelSpawn = 2 },
            new DispatchablePerson("s_m_y_marine_03",100,100) 
            {
                DebugName = "Military_FullGear"
                ,AccuracyMin = 25
                ,AccuracyMax = 35
                ,ShootRateMin = 450
                ,ShootRateMax = 550
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 150
                ,MaxWantedLevelSpawn = 10
                ,AllowRandomizeBeforeVariationApplied = true
                ,FiringPatternHash = -957453492//fullauto
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)})
            },
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
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
        MarshalsServicePeds = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_freemode_01",50,0) {
                DebugName = "USMSMPMale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0),new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() {  }
                ,OptionalPropChance = 0
            },

            new DispatchablePerson("mp_f_freemode_01",50,0) {
                DebugName = "USMSMPFemale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = 2
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() {  }
                ,OptionalPropChance = 10
            },

            new DispatchablePerson("mp_m_freemode_01", 0, 50) {
                DebugName = "USMSArmorMPMale"
                ,GroupName = "Armored"
                ,AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,RandomizeHead = true
                ,MinWantedLevelSpawn = 3
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 5, 3, 0), new PedComponent(3, 139, 4, 0), new PedComponent(4, 47, 0, 0), new PedComponent(5, 0, 0, 0), new PedComponent(6, 14, 0, 0), new PedComponent(7, 125, 0, 0), new PedComponent(8, 130, 0, 0), 
                        new PedComponent(9, 16, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 18, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() {  }
                ,OptionalPropChance = 0
            },


        };
        LSPDASDPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_y_pilot_01",5,5) { DebugName = "Police and LSPD Labeled Pilot",GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } }, //HAS LSPD STUFF ON HIM!!!!
        };
        LSSDASDPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_m_pilot_02",5,5){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
        };
        OffDutyCops = new List<DispatchablePerson>()
        { 
            new DispatchablePerson("a_f_y_bevhills_03",25,0) {  OverrideAgencyLongGuns = true,GroupName = "OffDuty", OverrideLongGunsID = "",OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, },
            new DispatchablePerson("a_f_y_fitness_01",25,0) { OverrideAgencyLongGuns = true,GroupName = "OffDuty",OverrideLongGunsID = "",OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, },
            new DispatchablePerson("a_f_y_soucent_02",25,0) { OverrideAgencyLongGuns = true,GroupName = "OffDuty",OverrideLongGunsID = "",OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, },
            new DispatchablePerson("a_m_m_skidrow_01",25,0) { OverrideAgencyLongGuns = true,GroupName = "OffDuty",OverrideLongGunsID = "" ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },},
            new DispatchablePerson("a_m_y_beachvesp_01",25,0) { OverrideAgencyLongGuns = true,GroupName = "OffDuty",OverrideLongGunsID = "" ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },},
            new DispatchablePerson("a_m_y_busicas_01",25,0) { OverrideAgencyLongGuns = true, GroupName = "OffDuty",OverrideLongGunsID = "" ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },},
        };
        SecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",0,0)  { DebugName = "SecurityMale",GroupName = "ArmedSecurity" },

            //Male
            //Top - 11
            //316 = long sleeve with collar closed
            //317 = long sleeve 
            //318 = short sleeve with collar closed
            //319 = short sleeve 
            //Texture 0 - Black, 1 - Forest Green, 2 - Cream, 3- Tan, 4 - Dark Green, 5 - white, 6 - off white, 7 grey

            //129 - 0 securoserv varsity closed


            //Lower - 4
            //122 = green cargo correctional pants tucked in
            //123 = green cargo correctional pants not tucked in
            //25-0 = regular suit pants in black
            //22-1 = olive regular
            //25 = regular suit pants 0 = black, 1=Gray,2=Navy,3=TealGreenkindEMTGreen,4=Red,5=White,6=Brown,7=?

            //Undershirt - 8
            //153 = Small Radio On Shoulder and some puches works with armor
            //122 glock with holster and some pouches, works with armor
            //155 - 0 = janitor stuff, radio on side
            //154 - 0 = just keys and small radio on side

            //Armor - 9
            //11 - 1 black, same as lspd

            //Decal - 10
            //71 = goes with long sleeve
            //76 = goes with short sleeve

            //Accessoriues
            //23 7=Brown,1=navy

            //Props
            //Hats - 1
            //65 - 0 = Securoserve hat in black
            //58 0 = tan, 1 = khaki, 2 = black = regular forawrds hat

            //Cream Generic
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPCreamShortSleeveUnArmedMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 22, 1, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 319, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 1),new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity",
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPCreamLongSleeveUnArmedMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 22, 1, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 317, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 1),new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity",
            },


            //Female
            
            //Upper - 3 
            //3 = long sleeve, only hands
            //9 = short sleeve full arms

            //Top - 11
            //328 = long sleeve 
            //330 = short sleeve 
            //Texture 0 - Black, 1 - Forest Green, 2 - Cream, 3- Tan, 4 - Dark Green, 5 - white, 6 - off white, 7 grey
            //127 - 0 securoserv varsity closed

            //lower -4 
            //6 0-black,1-charcoal,2-navy = suit, others are BROKE
            //37 - 6 = bown, but needs new shoes


            //23 5-olive = suit again

            //Decal - 10
            //85 = g6 for short sleeve
            //80 = g6 for long sleeve

            //Armor - 9
            //6 - 1 black, same as lspd
  
            //Undershirt - 8
            //152 glock with holster and some pouches, works with armor
            //159 - 0 = janitor stuff, radio on side
            //160 = only glock
            //189 radio on shoulder, cop style?no baton
            //190 pouches and keys
            //191 puches keys, janitor stuff

            //accessories
            //22 = tie 7 = tan

            //Props
            //Hats - 1
            //64 - 0 = Securoserve hat in black
            //58 0 = tan, 1 = khaki, 2 = black = regular forawrds hat

            //Cream Generic
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPCreamShortSleeveUnArmedFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 23, 5, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 330, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 1), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity",
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPCreamLongSleeveUnArmedFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 23, 5, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 328, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 1), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity",
            },
        };
        GruppeSechsPeds = new List<DispatchablePerson>() {

            //
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPGreenShortSleeveArmedG6Male"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(10, 76, 0, 0), new PedComponent(11, 319, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(0, 124, 0), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPGreenLongSleeveArmedG6Male"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(10, 71, 0, 0), new PedComponent(11, 317, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(0, 124, 0), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPGreenShortSleeveUnArmedG6Male"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(10, 76, 0, 0), new PedComponent(11, 319, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                 ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPGreenLongSleeveUnArmedG6Male"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(10, 71, 0, 0), new PedComponent(11, 317, 1, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },

            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPGreenShortSleeveArmedG6Female"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0) ,new PedComponent(9, 6, 1, 0), new PedComponent(10, 85, 0, 0), new PedComponent(11, 330, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 0),new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPGreenLongSleeveArmedG6Female"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 80, 0, 0), new PedComponent(11, 328, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 0), new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPGreenShortSleeveUnArmedG6Female"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(10, 85, 0, 0), new PedComponent(11, 330, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPGreenLongSleeveUnArmedG6Female"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(10, 80, 0, 0), new PedComponent(11, 328, 1, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
        };
        BobcatPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPShortSleeveArmedBobcatMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 25, 6, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 319, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPLongSleeveArmedBobcatMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 25, 6, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 317, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPShortSleeveUnArmedBobcatMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 25, 6, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 319, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPLongSleeveUnArmedBobcatMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 25, 6, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 317, 2, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },

            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPShortSleeveArmedBobcatFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 37, 6, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0) ,new PedComponent(9, 6, 1, 0), new PedComponent(11, 330, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPLongSleeveArmedBobcatFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 37, 6, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 328, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 0), new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPShortSleeveUnArmedBobcatFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 37, 6, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 330, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPLongSleeveUnArmedBobcatFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 37, 6, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 328, 2, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
        };
        MerryweatherSecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPShortSleeveArmedMerryMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 2, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 319, 5, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPLongSleeveArmedMerryMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 2, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 317, 5, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPShortSleeveUnArmedMerryMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 2, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 319, 5, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPLongSleeveUnArmedMerryMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 2, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 153, 0, 0), new PedComponent(11, 317, 5, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() {  new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },

            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPShortSleeveArmedMerryFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 6, 2, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0) ,new PedComponent(9, 6, 1, 0), new PedComponent(11, 330, 5, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPLongSleeveArmedMerryFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 2, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 328, 5, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,GroupName = "ArmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPShortSleeveUnArmedMerryFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0) ,new PedComponent(4, 6, 2, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 330, 5, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 58, 2), new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPLongSleeveUnArmedMerryFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 2, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 189, 0, 0), new PedComponent(11, 328, 5, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("s_m_m_security_01",0,0)  { DebugName = "SecurityMale",GroupName = "ArmedSecurity" },
        };
        SecuroservPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_securoguard_01",20,20)  { DebugName = "DLCSecuroGuard 01",GroupName = "ArmedSecurity" },

            //MP_M_WeapExp_01 underground guns guy?

            new DispatchablePerson("mp_m_freemode_01",20,20) {
                DebugName = "MPBlackVarsityUnArmedSecuroMale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 154, 0, 0), new PedComponent(11, 129, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0, 65, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 5, 0) }
                ,OptionalPropChance = 25
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
            new DispatchablePerson("mp_f_freemode_01",20,20) {
                DebugName = "MPBlackVarsityUnArmedSecuroFemale"
                ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 190, 0, 0), new PedComponent(11, 127, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0, 64, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 11, 0) }
                ,OptionalPropChance = 10
                ,OverrideAgencySideArms = true
                ,OverrideAgencyLongGuns = true
                ,OverrideSideArmsID = null
                ,OverrideLongGunsID = null
                ,GroupName = "UnarmedSecurity"
            },
        };
        LSLifeguardPeds = new List<DispatchablePerson>() 
        {
            new DispatchablePerson("s_m_y_baywatch_01",50,50)  { DebugName = "LS Lifeguard Male" },
            new DispatchablePerson("s_f_y_baywatch_01",50,50)  { DebugName = "LS Lifeguard Female" },
        };
        CoastGuardPeds = new List<DispatchablePerson>() 
        {
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
        DispatchablePeople_LostMC = new DispatchablePeople_LostMC(this);
        DispatchablePeople_LostMC.Setup();
        DispatchablePeople_Mafia = new DispatchablePeople_Mafia(this);
        DispatchablePeople_Mafia.Setup();
        DispatchablePeople_Diablos = new DispatchablePeople_Diablos(this);
        DispatchablePeople_Diablos.Setup();
        DispatchablePeople_Varrios = new DispatchablePeople_Varrios(this);
        DispatchablePeople_Varrios.Setup();

        DispatchablePeople_AngelsOfDeath = new DispatchablePeople_AngelsOfDeath(this);
        DispatchablePeople_AngelsOfDeath.Setup();


        DispatchablePeople_AngelsOfDeath_LS = new DispatchablePeople_AngelsOfDeath(this);
        DispatchablePeople_AngelsOfDeath_LS.IsLosSantos = true;
        DispatchablePeople_AngelsOfDeath_LS.Setup();


        DispatchablePeople_UptownRiders = new DispatchablePeople_UptownRiders(this);
        DispatchablePeople_UptownRiders.Setup();


        DispatchablePeople_NorthHolland = new DispatchablePeople_NorthHolland(this);
        DispatchablePeople_NorthHolland.Setup();

        DispatchablePeople_Petrovic = new DispatchablePeople_Petrovic(this);
        DispatchablePeople_Petrovic.Setup();



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
        //VarriosPeds = new List<DispatchablePerson>() {
            
        //    //new DispatchablePerson("ig_ortega",20,20,5,10,400,600,0,1) { DebugName = "VarriosOrtegaMale" },
        //};
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

        YardiesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_og_boss_01",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale1" },
            new DispatchablePerson("a_m_m_soucent_03",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale2" },
            new DispatchablePerson("a_m_y_soucent_02",30,30,5,10,400,600,0,1) { DebugName = "YardiesMale3" },
        };
        //NorthHollandPeds = new List<DispatchablePerson>() {
        //    new DispatchablePerson("a_m_m_soucent_01",30,30,5,10,400,600,0,1) { DebugName = "NorthHollandMale1" },
        //    new DispatchablePerson("a_m_o_soucent_01",30,30,5,10,400,600,0,1) { DebugName = "NorthHollandMale2" },
        //    new DispatchablePerson("a_m_m_soucent_04",30,30,5,10,400,600,0,1) { DebugName = "NorthHollandMale3" },
        //};
        //PetrovicPeds = new List<DispatchablePerson>() {
        //    new DispatchablePerson("ig_russiandrunk",30,30,5,10,400,600,0,1) { DebugName = "PetrovicMale1" },
        //    new DispatchablePerson("g_m_m_armlieut_01",30,30,5,10,400,600,0,1) { DebugName = "PetrovicMale2" },
        //};
        SpanishLordsPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_mexgang_01",30,30,5,10,400,600,0,1) { DebugName = "SpanishLordsMale1" },
            new DispatchablePerson("g_m_y_mexgoon_01",30,30,5,10,400,600,0,1) { DebugName = "SpanishLordsMale2" },
            new DispatchablePerson("g_m_y_mexgoon_02",30,30,5,10,400,600,0,1) { DebugName = "SpanishLordsMale3" },
        };
        //UptownRidersPeds = new List<DispatchablePerson>() {
        //    new DispatchablePerson("a_m_m_og_boss_01",30,30,5,10,400,600,0,1) { DebugName = "UptownRiderMale1" },
        //};


        //a_m_m_soucent_01

        //Other Peds
        OtherPeds = new List<DispatchablePerson>() {

            new DispatchablePerson("u_m_y_juggernaut_01", 100,100){
                DebugName = "Juggernautmale"
                ,HasFullBodyArmor = true
                ,DisableBulletRagdoll = true
                ,DisableCriticalHits = true
                ,FiringPatternHash = -957453492//fullauto
                ,HealthMin = 700
                ,HealthMax = 700
                ,ArmorMin = 1500
                ,ArmorMax = 1500
                ,AllowRandomizeBeforeVariationApplied = true
                ,OverrideAgencySideArms = true
                ,OverrideSideArmsID = "Minigun"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "Minigun"
                ,ShootRateMin = 500
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,AlwaysHasLongGun = true
                ,CombatRange = 2
            },
        };
        IllicitMarketplacePeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("IG_DrugDealer",100,100),
            new DispatchablePerson("S_M_Y_Dealer_01",100,100),
        };
        RegularPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_bevhills_02",100,100),
            new DispatchablePerson("a_m_m_genfat_02",100,100),
            new DispatchablePerson("a_m_m_eastsa_02",100,100),
            new DispatchablePerson("a_m_m_afriamer_01",100,100),
            new DispatchablePerson("a_m_m_stlat_02",100,100),
            new DispatchablePerson("a_m_y_beachvesp_01",100,100),
            new DispatchablePerson("a_m_y_beachvesp_02",100,100),
            new DispatchablePerson("a_m_y_bevhills_01",100,100),
            new DispatchablePerson("a_m_y_eastsa_02",100,100),
            new DispatchablePerson("a_f_m_salton_01",100,100),
            new DispatchablePerson("a_f_y_business_03",100,100),
            new DispatchablePerson("a_f_m_tourist_01",100,100),
            new DispatchablePerson("a_f_o_genstreet_01",100,100),
            new DispatchablePerson("a_f_y_femaleagent",100,100),
            new DispatchablePerson("a_f_y_fitness_02",100,100),
        };

        VehicleRacePeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("ig_car3guy2",100,100),
            new DispatchablePerson("a_m_m_bevhills_02",100,100),
            new DispatchablePerson("a_m_m_genfat_02",100,100),
            new DispatchablePerson("a_m_m_eastsa_02",100,100),
            new DispatchablePerson("a_m_m_afriamer_01",100,100),
            new DispatchablePerson("a_m_m_stlat_02",100,100),
            new DispatchablePerson("a_m_y_beachvesp_01",100,100),
            new DispatchablePerson("a_m_y_beachvesp_02",100,100),
            new DispatchablePerson("a_m_y_bevhills_01",100,100),
            new DispatchablePerson("a_m_y_eastsa_02",100,100),
            new DispatchablePerson("a_f_m_salton_01",100,100),
            new DispatchablePerson("a_f_y_business_03",100,100),
            new DispatchablePerson("a_f_m_tourist_01",100,100),
            new DispatchablePerson("a_f_o_genstreet_01",100,100),
            new DispatchablePerson("a_f_y_femaleagent",100,100),
            new DispatchablePerson("a_f_y_fitness_02",100,100),
        };

        ServicePeds();
    }
    private void ServicePeds()
    {
        DispatchablePeople_Service dispatchablePeople_Service = new DispatchablePeople_Service(this);
        dispatchablePeople_Service.Setup();

        TaxiDrivers = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_socenlat_01",100,100),
        };
        VendorPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_strvend_01",100,100),
            new DispatchablePerson("s_m_m_linecook",100,100),
        };
        TellerPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_f_m_shop_high",100,100),
            new DispatchablePerson("s_f_y_airhostess_01",100,100),
            new DispatchablePerson("s_m_m_highsec_01",100,100),
        };
        BarPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_f_y_clubbar_01",100,100),
            new DispatchablePerson("s_m_y_clubbar_01",100,100),
            new DispatchablePerson("a_f_y_clubcust_01",100,100),
        };
        HaircutPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_f_m_fembarber",100,100),
        };
        BobMuletPeds = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_m_hairdress_01",100,100),
        };
        BurgerShotPeds = new List<DispatchablePerson>()
        {
            dispatchablePeople_Service.CreateBurgerShotPed(true, true),
            dispatchablePeople_Service.CreateBurgerShotPed(false, true),
            dispatchablePeople_Service.CreateBurgerShotPed(true, false),
            dispatchablePeople_Service.CreateBurgerShotPed(false, false),
        };
        CluckinBellPeds = new List<DispatchablePerson>()
        {
            dispatchablePeople_Service.CreateCluckinBellPed(true, true,false),
            dispatchablePeople_Service.CreateCluckinBellPed(false, true,false),
            dispatchablePeople_Service.CreateCluckinBellPed(true, false,false),
            dispatchablePeople_Service.CreateCluckinBellPed(false, false,false),

            dispatchablePeople_Service.CreateCluckinBellPed(true, true,true),
            dispatchablePeople_Service.CreateCluckinBellPed(false, true,true),
            dispatchablePeople_Service.CreateCluckinBellPed(true, false,true),
            dispatchablePeople_Service.CreateCluckinBellPed(false, false,true),
        };
        TwatPeds = new List<DispatchablePerson>()
        {
            dispatchablePeople_Service.CreatetwatPed(true),
            dispatchablePeople_Service.CreatetwatPed(false),
        };

        GunshopPeds = new List<DispatchablePerson>()
        { 
            new DispatchablePerson("s_m_m_ccrew_03",100,100),
            new DispatchablePerson("g_m_m_cartelgoons_01",100,100),
            new DispatchablePerson("ig_charlie_reed",100,100),
            new DispatchablePerson("ig_req_officer",100,100),
        };

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
        PeopleGroupLookup.Add(new DispatchablePersonGroup("ArmyPeds", ArmyPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("USMCPeds", USMCPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("USAFPeds", USAFPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("OffDutyCops", OffDutyCops));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LSLifeguardPeds", LSLifeguardPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LCPDPeds", StandardCops));
        //Fire
        PeopleGroupLookup.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        //EMT
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("EMTs", EMTs));
        //Security
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechsPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroservPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BobcatPeds", BobcatPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurityPeds));
        //Gangs
        PeopleGroupLookup.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("DiablosPeds", DiablosPeds));
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


        PeopleGroupLookup.Add(new DispatchablePersonGroup("NorthHollandPeds", NorthHollandPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("PetrovicPeds", PetrovicPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("SpanishLordsPeds", SpanishLordsPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("UptownRidersPeds", UptownRidersPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("AngelsOfDeathPeds", AngelsOfDeathPeds_LS));

        //Other
        PeopleGroupLookup.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));


        PeopleGroupLookup.Add(new DispatchablePersonGroup("VehicleRacePeds", VehicleRacePeds));

        PeopleGroupLookup.Add(new DispatchablePersonGroup("VendorPeds", VendorPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", IllicitMarketplacePeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("TellerPeds", TellerPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BarPeds", BarPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("HaircutPeds", HaircutPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("BobMuletPeds", BobMuletPeds));


        PeopleGroupLookup.Add(new DispatchablePersonGroup("BurgerShotPeds", BurgerShotPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("CluckinBellPeds", CluckinBellPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("TwatPeds", TwatPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("GunshopPeds", GunshopPeds));
        //

        Serialization.SerializeParams(PeopleGroupLookup, ConfigFileName);
    }
    private void DefaultConfig_LibertyCity()
    {
        List<DispatchablePersonGroup> LCPeopleGroupLookup = Extensions.DeepCopy(PeopleGroupLookup);
        int optionalpropschance = 20;
        DispatchablePerson DetectiveMale = new DispatchablePerson("mp_m_freemode_01", 2, 2)
        {
            DebugName = "MPDetectiveMale",
            GroupName = "Detective",
            RandomizeHead = true,
            MaxWantedLevelSpawn = 2,
            OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
        new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0), new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0) },
        new List<PedPropComponent>() { }),
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), },
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson DetectiveFemale = new DispatchablePerson("mp_f_freemode_01", 2, 2)
        {
            DebugName = "MPDetectiveFemale",
            GroupName = "Detective",
            RandomizeHead = true,
            MaxWantedLevelSpawn = 2,
            OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                 new List<PedComponent>() { new PedComponent(3, 3, 0, 0), new PedComponent(4, 34, 0, 0), new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0) },
                 new List<PedPropComponent>() { }),
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), },
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson K9Generic = new DispatchablePerson("a_c_husky", 50, 50)
        {
            IsAnimal = true,
            DebugName = "K9_Husky",
            UnitCode = "K9",
            RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) }, new List<PedPropComponent>()),
            OverrideAgencyLessLethalWeapons = true,
            OverrideAgencySideArms = true,
            OverrideAgencyLongGuns = true,
            OverrideLessLethalWeaponsID = null,
            OverrideSideArmsID = null,
            OverrideLongGunsID = null,
        };

        DispatchablePerson PilotGeneric = new DispatchablePerson("s_m_m_pilot_02", 0, 0) { DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) } } };
        LCPeopleGroupLookup.RemoveAll(x => x.DispatchablePersonGroupID == "LCPDPeds");
        LCPeopleGroupLookup.RemoveAll(x => x.DispatchablePersonGroupID == "ASPPeds");
        LCPeopleGroupLookup.RemoveAll(x => x.DispatchablePersonGroupID == "NOOSEPeds");

        List<DispatchablePerson> NOOSE_DLC = new List<DispatchablePerson>()
        {
            DetectiveMale,
            DetectiveFemale,
            new DispatchablePerson("ig_lcswat", 2,100) {
                DebugName = "ig_lcswat"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lcswat", 100,100) {
                DebugName = "ig_lcswat"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(0, 2, 0), new PedPropComponent(0, 3, 0), new PedPropComponent(0, 4, 0), new PedPropComponent(0, 5, 0), new PedPropComponent(0, 6, 0) }
                ,OptionalPropChance = 80
                ,RequiredVariation = new PedVariation(),
            },
        };

        List<DispatchablePerson> LCPD_DLC = new List<DispatchablePerson>() {

            DetectiveMale,
            DetectiveFemale,
            K9Generic,
            PilotGeneric,

            //Standard
            new DispatchablePerson("ig_lccop_traffic",15,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },

            new DispatchablePerson("ig_lccop_traffic",0,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 100
                ,GroupName = "MotorcycleCop"
                ,UnitCode = "Mary"
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },
            new DispatchablePerson("ig_lccop_01",45,45) {
                DebugName = "ig_lccop_01"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true//no props
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_02",45,45) {
                DebugName = "ig_lccop_02"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true//no props
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lcfatcop",25,25) {
                DebugName = "ig_lcfatcop"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },


            //Tactical
            new DispatchablePerson("ig_lccop_vest_01",0,45) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_vest_02",0,45) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_vest_02",0,0) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation()
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = 65
                ,AccuracyMax = 85
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
            new DispatchablePerson("ig_lcfatcop_vest",0,15) {
                DebugName = "ig_lcfatcop_vest"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
        };

        List<DispatchablePerson> ASP_DLC = new List<DispatchablePerson>() {
            DetectiveMale,
            DetectiveFemale,
            K9Generic,
            PilotGeneric,
            new DispatchablePerson("ig_lcstrooper",100,100) {
                DebugName = "ig_lcstrooper"
                ,MaxWantedLevelSpawn = 4
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(3, 0, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), }
                ,OptionalPropChance = 70
            },
            new DispatchablePerson("ig_lccop_traffic",0,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 100
                ,GroupName = "MotorcycleCop"
                ,UnitCode = "Mary"
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },
            new DispatchablePerson("ig_lccop_vest_02",0,0) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation()
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = 65
                ,AccuracyMax = 85
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
        };

        LCPeopleGroupLookup.Add(new DispatchablePersonGroup("LCPDPeds", LCPD_DLC));
        LCPeopleGroupLookup.Add(new DispatchablePersonGroup("ASPPeds", ASP_DLC));
        LCPeopleGroupLookup.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSE_DLC));



        List<DispatchablePerson> FDLC_DLC = new List<DispatchablePerson>() {

            new DispatchablePerson("mp_m_freemode_01",100,100) { 
                DebugName = "fdlcfreemode1Gear"
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { 
                    new PedComponent(3, 170, 0, 0),//BLACK GLOVES
                    new PedComponent(4, 120, 0, 0),//Pants
                    new PedComponent(6, 51, 0, 0),//boots
                    new PedComponent(8, 151, 0, 0),//ff gear and rebreather
                    new PedComponent(11, 314, 0, 0),//open yellow 
                })
                ,OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(3, 0, 0, 0),//no gloves
                    new PedComponent(11, 315, 0, 0),//closed? yellow 
                }
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 137, 0),new PedPropComponent(0,138,0) }//hat
                ,OptionalPropChance = 30
                ,OptionalComponentChance = 50
                ,RandomizeHead = true
            },
            new DispatchablePerson("mp_m_freemode_01",100,100) {
                DebugName = "fdlcfreemode2Gear"
                ,RequiredVariation = new PedVariation(new List<PedComponent>() {
                    new PedComponent(3, 170, 0, 0),//BLACK GLOVES
                    new PedComponent(4, 120, 0, 0),//Pants
                    new PedComponent(6, 51, 0, 0),//boots
                    new PedComponent(11, 314, 0, 0),//open yellow 
                })
                ,OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(3, 0, 0, 0),//no gloves
                    new PedComponent(11, 315, 0, 0),//closed? yellow 
                }
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 137, 0), new PedPropComponent(0, 138, 0) }
                ,OptionalPropChance = 20
                ,OptionalComponentChance = 50
                ,RandomizeHead = true
            },

            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "fdlcFfreemode1Gear"
                ,RequiredVariation = new PedVariation(new List<PedComponent>() {
                    new PedComponent(3, 215, 0, 0),//BLACK GLOVES
                    new PedComponent(4, 126, 0, 0),//Pants
                    new PedComponent(6, 55, 0, 0),//boots
                    new PedComponent(8, 187, 0, 0),//ff gear and rebreather
                    new PedComponent(11, 325, 0, 0),//open yellow 
                })
                ,OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(3, 0, 0, 0),//no gloves
                    new PedComponent(11, 326, 0, 0),//closed? yellow 
                }
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 136, 0),new PedPropComponent(0,137,0) }//hat
                ,OptionalPropChance = 30
                ,OptionalComponentChance = 50
                ,RandomizeHead = true
            },
            new DispatchablePerson("mp_f_freemode_01",100,100) {
                DebugName = "fdlcFfreemode2"
                ,RequiredVariation = new PedVariation(new List<PedComponent>() {
                    new PedComponent(3, 215, 0, 0),//BLACK GLOVES
                    new PedComponent(4, 126, 0, 0),//Pants
                    new PedComponent(6, 55, 0, 0),//boots
                    new PedComponent(11, 325, 0, 0),//open yellow 
                })
                ,OptionalComponents = new List<PedComponent>()
                {
                    new PedComponent(3, 0, 0, 0),//no gloves
                    new PedComponent(11, 326, 0, 0),//closed? yellow 
                }
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 136, 0), new PedPropComponent(0, 137, 0) }
                ,OptionalPropChance = 20
                ,OptionalComponentChance = 50
                ,RandomizeHead = true
            },

        };
        LCPeopleGroupLookup.Add(new DispatchablePersonGroup("FDLCFirePeds", FDLC_DLC));


        Serialization.SerializeParams(LCPeopleGroupLookup, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\DispatchablePeople_{StaticStrings.LibertyConfigSuffix}.xml");
    }
    private void DefaultConfig_LPP()
    {
        List<DispatchablePersonGroup> LPPPersonGroup = Extensions.DeepCopy(PeopleGroupLookup);
        int optionalpropschance = 20;
        DispatchablePerson DetectiveMale = new DispatchablePerson("mp_m_freemode_01", 2, 2)
        {
            DebugName = "MPDetectiveMale",
            GroupName = "Detective",
            RandomizeHead = true,
            MaxWantedLevelSpawn = 2,
            OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
        new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0), new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0) },
        new List<PedPropComponent>() { }),
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), },
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson DetectiveFemale = new DispatchablePerson("mp_f_freemode_01", 2, 2)
        {
            DebugName = "MPDetectiveFemale",
            GroupName = "Detective",
            RandomizeHead = true,
            MaxWantedLevelSpawn = 2,
            OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                 new List<PedComponent>() { new PedComponent(3, 3, 0, 0), new PedComponent(4, 34, 0, 0), new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0) },
                 new List<PedPropComponent>() { }),
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), },
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson K9Generic = new DispatchablePerson("a_c_husky", 50, 50)
        {
            IsAnimal = true,
            DebugName = "K9_Husky",
            UnitCode = "K9",
            RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) }, new List<PedPropComponent>()),
            OverrideAgencyLessLethalWeapons = true,
            OverrideAgencySideArms = true,
            OverrideAgencyLongGuns = true,
            OverrideLessLethalWeaponsID = null,
            OverrideSideArmsID = null,
            OverrideLongGunsID = null,
        };

        DispatchablePerson PilotGeneric = new DispatchablePerson("s_m_m_pilot_02", 0, 0) { DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) } } };
        LPPPersonGroup.RemoveAll(x => x.DispatchablePersonGroupID == "LCPDPeds");
        LPPPersonGroup.RemoveAll(x => x.DispatchablePersonGroupID == "ASPPeds");
        LPPPersonGroup.RemoveAll(x => x.DispatchablePersonGroupID == "NOOSEPeds");
        List<DispatchablePerson> NOOSE_DLC = new List<DispatchablePerson>()
        {
            DetectiveMale,
            DetectiveFemale,
            new DispatchablePerson("ig_lcswat", 2,100) {
                DebugName = "ig_lcswat"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lcswat", 100,100) {
                DebugName = "ig_lcswat"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(0, 2, 0), new PedPropComponent(0, 3, 0), new PedPropComponent(0, 4, 0), new PedPropComponent(0, 5, 0), new PedPropComponent(0, 6, 0) }
                ,OptionalPropChance = 80
                ,RequiredVariation = new PedVariation(),
            },
        };

        List<DispatchablePerson> LCPD_DLC = new List<DispatchablePerson>() {

            DetectiveMale,
            DetectiveFemale,
            K9Generic,
            PilotGeneric,

            //Standard
            new DispatchablePerson("ig_lccop_traffic",15,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },

            new DispatchablePerson("ig_lccop_traffic",0,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 100
                ,GroupName = "MotorcycleCop"
                ,UnitCode = "Mary"
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },
            new DispatchablePerson("ig_lccop_01",45,45) {
                DebugName = "ig_lccop_01"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true//no props
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_02",45,45) {
                DebugName = "ig_lccop_02"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true//no props
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lcfatcop",25,25) {
                DebugName = "ig_lcfatcop"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },


            //Tactical
            new DispatchablePerson("ig_lccop_vest_01",0,45) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_vest_02",0,45) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
            new DispatchablePerson("ig_lccop_vest_02",0,0) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation()
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = 65
                ,AccuracyMax = 85
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
            new DispatchablePerson("ig_lcfatcop_vest",0,15) {
                DebugName = "ig_lcfatcop_vest"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,MinWantedLevelSpawn = 3
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation(),
            },
        };

        List<DispatchablePerson> ASP_DLC = new List<DispatchablePerson>() {
            DetectiveMale,
            DetectiveFemale,
            K9Generic,
            PilotGeneric,
            new DispatchablePerson("ig_lcstrooper",100,100) {
                DebugName = "ig_lcstrooper"
                ,MaxWantedLevelSpawn = 4
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(3, 0, 0, 0) })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), }
                ,OptionalPropChance = 70
            },
            new DispatchablePerson("ig_lccop_traffic",0,0) {
                DebugName = "ig_lccop_traffic"
                ,MaxWantedLevelSpawn = 2
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                ,OptionalPropChance = 100
                ,GroupName = "MotorcycleCop"
                ,UnitCode = "Mary"
                ,RequiredVariation = new PedVariation(),
                //hat 0 = helmet, 1 = hat
                //glasses 0 or 1 with 0 or 1 for texture
            },
            new DispatchablePerson("ig_lccop_vest_02",0,0) {
                DebugName = "ig_lccop_vest_01"
                ,ArmorMin = 50
                ,ArmorMax = 50
                ,AllowRandomizeBeforeVariationApplied = true
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), }
                ,OptionalPropChance = 70
                ,RequiredVariation = new PedVariation()
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = 65
                ,AccuracyMax = 85
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },
        };

        LPPPersonGroup.Add(new DispatchablePersonGroup("LCPDPeds", LCPD_DLC));
        LPPPersonGroup.Add(new DispatchablePersonGroup("ASPPeds", ASP_DLC));
        LPPPersonGroup.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSE_DLC));



        List<DispatchablePerson> FDLC_DLC = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_lcfireman_01",100,100) {
                DebugName = "fdlcfreemode1Gear"
            },
        };
        LPPPersonGroup.Add(new DispatchablePersonGroup("FDLCFirePeds", FDLC_DLC));

        Serialization.SerializeParams(LPPPersonGroup, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\DispatchablePeople_{StaticStrings.LPPConfigSuffix}.xml");
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
                ,GroupName = "FIBHET"
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

        List<DispatchablePerson> LSPDASDPeds_2008 = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_y_pilot_01",0,0) { DebugName = "Police and LSPD Labeled Pilot",GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } }, //HAS LSPD STUFF ON HIM!!!!
            new DispatchablePerson("s_m_y_swat_01", 0,100){DebugName = "LSPDASDSWAT",AllowRandomizeBeforeVariationApplied = true,RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })},
        };
        List<DispatchablePerson> LSSDASDPeds_2008 = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
            new DispatchablePerson("s_m_y_swat_01", 0,100){DebugName = "LSSDASDSWAT",AllowRandomizeBeforeVariationApplied = true,RequiredVariation = new PedVariation( new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })},
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
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("ArmyPeds", ArmyPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("USMCPeds", USMCPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("USAFPeds", USAFPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("OffDutyCops", OffDutyCops));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LSLifeguardPeds", LSLifeguardPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds_2008));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds_2008));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LCPDPeds", StandardCops_Old));
        //Fire
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        //EMT
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("EMTs", EMTs));
        //Security
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechsPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroservPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BobcatPeds", BobcatPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurityPeds));

        //Gangs
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("DiablosPeds", DiablosPeds));
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

        //Other
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("VehicleRacePeds", VehicleRacePeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("VendorPeds", VendorPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", IllicitMarketplacePeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("TellerPeds", TellerPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BurgerShotPeds", BurgerShotPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("CluckinBellPeds", CluckinBellPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("TwatPeds", TwatPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("GunshopPeds", GunshopPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BarPeds", BarPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("HaircutPeds", HaircutPeds));
        PeopleGroupLookup_Old.Add(new DispatchablePersonGroup("BobMuletPeds", BobMuletPeds));
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

        FIBPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){ DebugName = "FIBAgentMale", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){ DebugName = "FIBNormalMale1", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){ DebugName = "FIBNormalMale2", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) { DebugName = "FIBNormalMale3", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0, 100) {
                DebugName = "FIBSWATDefaultMale"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },
        };
        NOOSEPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01",100,100){
                DebugName = "NOOSEDefaultMale"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },
        };
        MarshalsServicePeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_ciasec_01",0, 50){ DebugName = "USMSSimple",AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100, MaxWantedLevelSpawn = 3 },
        };
        PrisonPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100)  { DebugName = "PrisonGuardMale" },
        };
        SecurityPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100)  { DebugName = "SecurityMale" },
        };
        GruppeSechsPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_armoured_01",100,100)  { DebugName = "SecurityMale" },
            new DispatchablePerson("s_m_m_armoured_02",100,100)  { DebugName = "SecurityMale" },
        };
        SecuroservPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_securoguard_01",100,100)  { DebugName = "SecurityMale" },
        };
        BobcatPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("ig_prolsec_02",100,100){ DebugName = "SecurityMale" },
        };
        MerryweatherSecurityPeds_Simple = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100)  { DebugName = "SecurityMale" },
        };
        //
        List<DispatchablePersonGroup> PeopleGroupLookup_Simple = new List<DispatchablePersonGroup>();
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("StandardCops", StandardCops_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("ArmyPeds", ArmyPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("USMCPeds", USMCPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("USAFPeds", USAFPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds_Simple));

        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("OffDutyCops", OffDutyCops));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("LSLifeguardPeds", LSLifeguardPeds));

        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("LCPDPeds", StandardCops_Old));

        //Fire
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        //EMTs
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("EMTs", EMTs));
        //Security
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechsPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroservPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BobcatPeds", BobcatPeds_Simple));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurityPeds_Simple));
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

        //Other
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("VehicleRacePeds", VehicleRacePeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("VendorPeds", VendorPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", IllicitMarketplacePeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("TellerPeds", TellerPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BurgerShotPeds", BurgerShotPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("CluckinBellPeds", CluckinBellPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("TwatPeds", TwatPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("GunshopPeds", GunshopPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BarPeds", BarPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("HaircutPeds", HaircutPeds));
        PeopleGroupLookup_Simple.Add(new DispatchablePersonGroup("BobMuletPeds", BobMuletPeds));
        Serialization.SerializeParams(PeopleGroupLookup_Simple, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\DispatchablePeople_Simple.xml");

        Serialization.SerializeParams(PeopleGroupLookup_Simple, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\DispatchablePeople_SunshineDream.xml");
    }
    private void DefaultConfig_SunshineDream()
    {
        List<DispatchablePerson> StandardCops_SunshineDream = new List<DispatchablePerson>() {
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
        List<DispatchablePerson> FIBPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){ DebugName = "FIBAgentMale", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){ DebugName = "FIBNormalMale1", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){ DebugName = "FIBNormalMale2", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) { DebugName = "FIBNormalMale3", MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0, 100) {
                DebugName = "FIBSWATDefaultMale"
                ,GroupName = "FIBHET"
                ,AccuracyMin = 30
                ,AccuracyMax = 50
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,MinWantedLevelSpawn = 5
                ,MaxWantedLevelSpawn = 5
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },
        };
        List<DispatchablePerson> NOOSEPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01",100,100){
                DebugName = "NOOSEDefaultMale"
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,AllowRandomizeBeforeVariationApplied = true
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },
                    new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) })
            },
        };
        List<DispatchablePerson> MarshalsServicePeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_ciasec_01",0, 50){ DebugName = "USMSSimple",AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100, MaxWantedLevelSpawn = 3 },
        };
        List<DispatchablePerson> PrisonPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100)  { DebugName = "PrisonGuardMale" },
        };
        List<DispatchablePerson> SecurityPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100)  { DebugName = "SecurityMale" },
        };
        List<DispatchablePerson> GruppeSechsPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_armoured_01",100,100)  { DebugName = "SecurityMale" },
            new DispatchablePerson("s_m_m_armoured_02",100,100)  { DebugName = "SecurityMale" },
        };
        List<DispatchablePerson> SecuroservPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("mp_m_securoguard_01",100,100)  { DebugName = "SecurityMale" },
        };
        List<DispatchablePerson> BobcatPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("ig_prolsec_02",100,100){ DebugName = "SecurityMale" },
        };
        List<DispatchablePerson> MerryweatherSecurityPeds_SunshineDream = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100)  { DebugName = "SecurityMale" },
        };
        //
        List<DispatchablePersonGroup> PeopleGroupLookup_SunshineDream = new List<DispatchablePersonGroup>();
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("VCPDPeds", StandardCops_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("VDPDPeds", StandardCops_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("VCPDHeliPeds", LSSDASDPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("ArmyPeds", ArmyPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("USMCPeds", USMCPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("USAFPeds", USAFPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("OffDutyCops", OffDutyCops));

        //Fire
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        //EMTs
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("EMTs", EMTs));
        //Security
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechsPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroservPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("BobcatPeds", BobcatPeds_SunshineDream));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurityPeds_SunshineDream));
        //Gangs
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));

        //Other
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("VehicleRacePeds", VehicleRacePeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("VendorPeds", VendorPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", IllicitMarketplacePeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("TellerPeds", TellerPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("BurgerShotPeds", BurgerShotPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("CluckinBellPeds", CluckinBellPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("TwatPeds", TwatPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("GunshopPeds", GunshopPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("BarPeds", BarPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("HaircutPeds", HaircutPeds));
        PeopleGroupLookup_SunshineDream.Add(new DispatchablePersonGroup("BobMuletPeds", BobMuletPeds));
        Serialization.SerializeParams(PeopleGroupLookup_SunshineDream, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\DispatchablePeople_SunshineDream.xml");
    }
   public OptionalAppliedOverlayLogic GenericGangTattoos(bool isMale, float headPercent, float torsoPercent, float armsPercent, float legsPercent)
    {
        return GenericGangTattoos(isMale, headPercent,1, torsoPercent,1, armsPercent,1, legsPercent,1);
    }  
    public OptionalAppliedOverlayLogic GenericGangTattoos(bool isMale, float headPercent, int headLimit, float torsoPercent, int torsoLimit, float armsPercent, int armsLimit, float legsPercent, int legsLimit)
    {
        OptionalAppliedOverlayLogic toreturn = new OptionalAppliedOverlayLogic();
        if (headPercent > 0f)
        {
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_HEAD", headPercent, headLimit));
            if (isMale)
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_000_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_001_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_018_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_019_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_020_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_021_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_022_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_023_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_024_M","ZONE_HEAD","FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_027_M","ZONE_HEAD","NECK_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_025_M","ZONE_HEAD","NECK_LEFT_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_026_M","ZONE_HEAD","NECK_LEFT_FULL"),
                });
            }
            else
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_000_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_001_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_018_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_019_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_020_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_021_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_022_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_023_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_024_F", "ZONE_HEAD", "FACE"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_027_F", "ZONE_HEAD", "NECK_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_025_F", "ZONE_HEAD", "NECK_LEFT_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays", "MP_Sum2_Tat_026_F", "ZONE_HEAD", "NECK_LEFT_FULL"),
                });
            }

        }
        if (torsoPercent > 0f)
        {
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_TORSO", torsoPercent, torsoLimit));
            if (isMale)
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_038_M","ZONE_TORSO","BACK_FULL_SHORT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_007_M","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_035_M","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_037_M","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_006_M","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_036_M","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_039_M","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_057_M","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_041_M","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_042_M","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_058_M","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_003_M","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_004_M","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_040_M","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_059_M","ZONE_TORSO","CHEST_RIGHT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_044_M","ZONE_TORSO","CHEST_STOM_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_043_M","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_060_M","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_061_M","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_062_M","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_005_M","ZONE_TORSO","STOMACH_LOWER_RIGHT"),
                });
            }
            else
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_038_F","ZONE_TORSO","BACK_FULL_SHORT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_007_F","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_035_F","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_037_F","ZONE_TORSO","BACK_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_006_F","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_036_F","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_039_F","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_057_F","ZONE_TORSO","BACK_UPPER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_041_F","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_042_F","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_058_F","ZONE_TORSO","CHEST_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_003_F","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_004_F","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_040_F","ZONE_TORSO","CHEST_LEFT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_059_F","ZONE_TORSO","CHEST_RIGHT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_044_F","ZONE_TORSO","CHEST_STOM_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_043_F","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_060_F","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_061_F","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_062_F","ZONE_TORSO","STOMACH_FULL"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_005_F","ZONE_TORSO","STOMACH_LOWER_RIGHT"),
                });
            }
        }
        if (armsPercent > 0f)
        {
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_RIGHT_ARM", armsPercent, armsLimit));
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_LEFT_ARM", armsPercent, armsLimit));
            if (isMale)
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_049_M","ZONE_LEFT_ARM","ARM_LEFT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_029_M","ZONE_LEFT_ARM","ARM_LEFT_LOWER_INNER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_009_M","ZONE_LEFT_ARM","ARM_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_028_M","ZONE_LEFT_ARM","ARM_LEFT_LOWER_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_008_M","ZONE_LEFT_ARM","ARM_LEFT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_010_M","ZONE_LEFT_ARM","ARM_LEFT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_013_M","ZONE_RIGHT_ARM","ARM_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_045_M","ZONE_RIGHT_ARM","ARM_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_047_M","ZONE_RIGHT_ARM","ARM_RIGHT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_046_M","ZONE_RIGHT_ARM","ARM_RIGHT_SHOULDER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_011_M","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_012_M","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_030_M","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_031_M","ZONE_RIGHT_ARM","HAND_RIGHT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_048_M","ZONE_RIGHT_ARM","HAND_RIGHT"),
                });
            }
            else
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_049_F","ZONE_LEFT_ARM","ARM_LEFT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_029_F","ZONE_LEFT_ARM","ARM_LEFT_LOWER_INNER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_009_F","ZONE_LEFT_ARM","ARM_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_028_F","ZONE_LEFT_ARM","ARM_LEFT_LOWER_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_008_F","ZONE_LEFT_ARM","ARM_LEFT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_010_F","ZONE_LEFT_ARM","ARM_LEFT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_013_F","ZONE_RIGHT_ARM","ARM_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_045_F","ZONE_RIGHT_ARM","ARM_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_047_F","ZONE_RIGHT_ARM","ARM_RIGHT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_046_F","ZONE_RIGHT_ARM","ARM_RIGHT_SHOULDER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_011_F","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_012_F","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_030_F","ZONE_RIGHT_ARM","ARM_RIGHT_UPPER_SIDE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_031_F","ZONE_RIGHT_ARM","HAND_RIGHT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_048_F","ZONE_RIGHT_ARM","HAND_RIGHT"),
                });
            }
        }
        if (legsPercent > 0f)
        {
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_LEFT_LEG", legsPercent, legsLimit));
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_RIGHT_LEG", legsPercent, legsLimit));
            if (isMale)
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_034_M","ZONE_RIGHT_LEG","LEG_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_051_M","ZONE_RIGHT_LEG","LEG_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_050_M","ZONE_RIGHT_LEG","LEG_RIGHT_LOWER_BACK"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_052_M","ZONE_RIGHT_LEG","LEG_RIGHT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_017_M","ZONE_RIGHT_LEG","LEG_RIGHT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_033_M","ZONE_RIGHT_LEG","LEG_RIGHT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_054_M","ZONE_LEFT_LEG","LEG_LEFT_CALF"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_002_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_014_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_015_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_016_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_055_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_056_M","ZONE_LEFT_LEG","LEG_LEFT_LOWER_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_032_M","ZONE_LEFT_LEG","LEG_LEFT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_053_M","ZONE_LEFT_LEG","LEG_LEFT_UPPER_FRONT"),
                });
            }
            else
            {
                toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
                {
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_054_F","ZONE_LEFT_LEG","LEG_LEFT_CALF"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_002_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_014_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_015_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_016_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_055_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_056_F","ZONE_LEFT_LEG","LEG_LEFT_LOWER_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_032_F","ZONE_LEFT_LEG","LEG_LEFT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_053_F","ZONE_LEFT_LEG","LEG_LEFT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_034_F","ZONE_RIGHT_LEG","LEG_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_051_F","ZONE_RIGHT_LEG","LEG_RIGHT_FULL_SLEEVE"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_050_F","ZONE_RIGHT_LEG","LEG_RIGHT_LOWER_BACK"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_052_F","ZONE_RIGHT_LEG","LEG_RIGHT_LOWER_OUTER"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_017_F","ZONE_RIGHT_LEG","LEG_RIGHT_UPPER_FRONT"),
                    new OptionalAppliedOverlay("mpSum2_overlays","MP_Sum2_Tat_033_F","ZONE_RIGHT_LEG","LEG_RIGHT_UPPER_FRONT"),
                });
            }
        }
        return toreturn;
    }
    private DispatchablePerson GetGenericMPCopPed(int ambientSpawnChance, int wantedSpawnChance, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve)
    {

        DispatchablePerson ShortSleeveMale = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "NooseShortSleeveArmedMerryMale"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = maxWantedLevelSpawn
                ,OverrideVoice = GeneralMaleCopVoices
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 319, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9),new PedPropComponent(1, 37, 0),new PedPropComponent(1, 38, 0),new PedPropComponent(1, 8, 3),new PedPropComponent(1, 8, 5),new PedPropComponent(1, 8, 6),new PedPropComponent(1, 7, 0),new PedPropComponent(1, 2, 3),new PedPropComponent(6, 3, 0), }
                ,OptionalPropChance = optionalpropschance
            };
            DispatchablePerson LongSleeveMale = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "NooseLongSleeveArmedMerryMale",
                RandomizeHead = true
                ,MaxWantedLevelSpawn = maxWantedLevelSpawn
                ,OverrideVoice = GeneralMaleCopVoices
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 317, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9),new PedPropComponent(1, 37, 0),new PedPropComponent(1, 38, 0),new PedPropComponent(1, 8, 3),new PedPropComponent(1, 8, 5),new PedPropComponent(1, 8, 6),new PedPropComponent(1, 7, 0),new PedPropComponent(1, 2, 3),new PedPropComponent(6, 3, 0), }
                ,OptionalPropChance = optionalpropschance
            };


            DispatchablePerson ShortSleeveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "NooseShortSleeveArmedMerryFemale"
                , RandomizeHead = true
                ,MaxWantedLevelSpawn = maxWantedLevelSpawn
                , OverrideVoice = GeneralFemaleCopVoices
                , RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 9, 0, 0), new PedComponent(4, 6, 0, 0), new PedComponent(6, 55, 0, 0), new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 330, 0, 0) },
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
                ,OptionalPropChance = optionalpropschance
            };
            DispatchablePerson LongSleeveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "NooseLongSleeveArmedMerryFemale",
                RandomizeHead = true
                ,MaxWantedLevelSpawn = maxWantedLevelSpawn
                ,OverrideVoice = GeneralFemaleCopVoices
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 6, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 328, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
                ,OptionalPropChance = optionalpropschance
            };



        if (isMale)
        {
            if(isShortSleeve)
            {
                return ShortSleeveMale;
            }
            return LongSleeveMale;
        }
        else
        {
            if(isShortSleeve)
            {
                return ShortSleeveFemale;
            }
            return LongSleeveFemale;
        }
    }
    private DispatchablePerson GetGenericMPDetectivePed(int ambientSpawnChance, int wantedSpawnChance,int maxwantedLevelSpawn, bool isMale)
    {
            DispatchablePerson DetectiveMale  = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "MPDetectiveMale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = maxwantedLevelSpawn
                ,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02","S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0),new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0)},
                    new List<PedPropComponent>() { })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), }
                ,OptionalPropChance = optionalpropschance
            };

        DispatchablePerson DetectiveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "MPDetectiveFemale"
                ,GroupName = "Detective"
                ,RandomizeHead = true
                ,MaxWantedLevelSpawn = maxwantedLevelSpawn
                ,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0)},
                    new List<PedPropComponent>() {  })
                ,OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
                ,OptionalPropChance = optionalpropschance
            };
        if(isMale)
        {
            return DetectiveMale;
        }
        else
        {
            return DetectiveFemale;
        }
    }
}

