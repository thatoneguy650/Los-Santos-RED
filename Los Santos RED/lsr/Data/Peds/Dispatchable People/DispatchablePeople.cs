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

    private List<DispatchablePerson> ArmyPeds;
    private List<DispatchablePerson> USMCPeds;
    private List<DispatchablePerson> USAFPeds;

    private List<DispatchablePerson> PrisonPeds;
    private List<DispatchablePerson> MarshalsServicePeds;
    private List<DispatchablePerson> OffDutyCops;
    private List<DispatchablePerson> SecurityPeds;
    private List<DispatchablePerson> GruppeSechsPeds;
    private List<DispatchablePerson> SecuroservPeds;
    private List<DispatchablePerson> MerryweatherSecurityPeds;
    private List<DispatchablePerson> BobcatPeds;

    private List<DispatchablePerson> CoastGuardPeds;
    private List<DispatchablePerson> NYSPPeds;
    private List<DispatchablePerson> Firefighters;
    private List<DispatchablePerson> EMTs;
    private List<DispatchablePerson> GreenEMTs;
    private List<DispatchablePerson> BlueEMTs;

    private List<DispatchablePerson> VagosPeds;

    private List<DispatchablePerson> FamiliesPeds;
    private List<DispatchablePerson> BallasPeds;
    private List<DispatchablePerson> MarabuntaPeds;
    private List<DispatchablePerson> AltruistPeds;

    private List<DispatchablePerson> TriadsPeds;
    private List<DispatchablePerson> KoreanPeds;
    private List<DispatchablePerson> RedneckPeds;
    private List<DispatchablePerson> ArmenianPeds;
    private List<DispatchablePerson> CartelPeds;
    private List<DispatchablePerson> YardiesPeds;
    //private List<DispatchablePerson> NorthHollandPeds;
    //private List<DispatchablePerson> PetrovicPeds;
    private List<DispatchablePerson> SpanishLordsPeds;
    private List<DispatchablePerson> OtherPeds;
    private List<DispatchablePerson> TaxiDrivers;
    private List<DispatchablePerson> VendorPeds;
    private List<DispatchablePerson> IllicitMarketplacePeds;
    private List<DispatchablePerson> TellerPeds;
    private List<DispatchablePerson> BarPeds;
    private List<DispatchablePerson> HaircutPeds;
    private List<DispatchablePerson> BobMuletPeds;
    private List<DispatchablePerson> BurgerShotPeds;

    public List<DispatchablePerson> CluckinBellPeds { get; private set; }
    public List<DispatchablePerson> TwatPeds { get; private set; }
    public List<DispatchablePerson> GunshopPeds { get; private set; }

    private List<DispatchablePerson> RegularPeds;

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


    public List<string> GeneralMaleVoices { get; private set; } = new List<string>() { "A_M_M_GENERICMALE_01_WHITE_MINI_01", "A_M_M_SALTON_01_WHITE_FULL_01", "A_M_M_TOURIST_01_WHITE_MINI_01", "A_M_M_MALIBU_01_LATINO_FULL_01" };
    public List<string> GeneralFemaleVoices { get; private set; } = new List<string>() { "A_F_Y_FITNESS_01_WHITE_FULL_01", "A_F_Y_BEVHILLS_01_WHITE_FULL_01", "A_F_Y_SOUCENT_01_BLACK_FULL_01", "A_F_Y_TOURIST_01_WHITE_FULL_01" };

    public List<string> GeneralMaleCopVoices { get; private set; } = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" };

    public List<string> GeneralFemaleCopVoices { get; private set; } = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" };

    public List<DispatchablePerson> MafiaPeds { get; set; }
    public List<DispatchablePerson> LostMCPeds { get; set; }
    public List<DispatchablePerson> DiablosPeds { get; set; }
    public List<DispatchablePerson> VarriosPeds { get; set; }

    public List<DispatchablePerson> AngelsOfDeathPeds { get; set; }
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
            DefaultConfig_SunshineDream();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig();
            DefaultConfig_LibertyCity();
            DefaultConfig_LPP();
        }
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
        PeopleGroupLookup.Add(new DispatchablePersonGroup("AngelsOfDeathPeds", AngelsOfDeathPeds));

        //Other
        PeopleGroupLookup.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleGroupLookup.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));
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
    private void DefaultConfig_FullExpandedJurisdiction()
    {
        List<DispatchablePersonGroup> PeopleConfig_EUP = new List<DispatchablePersonGroup>();

        int optionalpropschance = 20;

        List<PedPropComponent> MaleCopOptionalProps = new List<PedPropComponent>() { 
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 8, 3),
            new PedPropComponent(1, 8, 5),
            new PedPropComponent(1, 8, 6),
            new PedPropComponent(1, 7, 0),
            new PedPropComponent(1, 2, 3),

        };

        List<PedPropComponent> FemaleCopOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 39, 0),
            new PedPropComponent(1, 40, 0),
            new PedPropComponent(1, 11, 0),
            new PedPropComponent(1, 11, 1),
            new PedPropComponent(1, 11, 3),
            new PedPropComponent(1, 24, 0),
        };

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

        //Cops
        List<DispatchablePerson> StandardCops_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },

            //LSPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Class A>",  MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Class A>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",25,25) { DebugName = "<Male LSPD Class B>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = 20,
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
            new DispatchablePerson("mp_f_freemode_01",25,25) { DebugName = "<Female LSPD Class B>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",55,55) { DebugName = "<Male LSPD Class C>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = 20,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 11, 0, 0),//new PedComponent(3, 4, 0, 0),//Peter Badoingy — Today at 8:01 AM Fixed an armless cop in the FEJ dispatchable people on line 773 under ped Component ID 3 i changed the DrawableID from 4 to 11, he now has elbows and forearms.
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
            new DispatchablePerson("mp_f_freemode_01",55,55) { DebugName = "<Female LSPD Class C>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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

            //LSPD Jacket MALE IS MISSING CHEST?
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Jacket>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 19, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 29, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 28, 0, 0),
                } },
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Jacket>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",1,1) { DebugName = "<Male LSPD Raincoat>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
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
            new DispatchablePerson("mp_f_freemode_01",1,1) { DebugName = "<Female LSPD Raincoat>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
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
            new DispatchablePerson("mp_m_freemode_01",0,20) {
                AccuracyMin = 25
                ,AccuracyMax = 30
                ,GroupName = "SWAT"
                ,DebugName = "<Male LSPD SWAT>"
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                RandomizeHead = true,
                MinWantedLevelSpawn = 3,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
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
            new DispatchablePerson("mp_f_freemode_01",0,20) {
                AccuracyMin = 25
                ,AccuracyMax = 30
                ,GroupName = "SWAT"
                ,DebugName = "<Female LSPD SWAT>"
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                RandomizeHead = true,
                MinWantedLevelSpawn = 3,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Detective Suit>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Detective Suit>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Male LSPD Detective>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Female LSPD Detective>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Male LSPD Detective Windbreaker>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 12, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 59, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 179, 0, 0),
                        new PedComponent(9, 17, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 325, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Female LSPD Detective Windbreaker>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
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

            //Utility (Tactical looking,looks like metro but in LIGHT BLACK, but no armor)
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",1,1) { OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,GroupName = "Detective", ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Armor Protection>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",1,1) { OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,GroupName = "Detective", ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Raid Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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

            //Animals
            new DispatchablePerson("a_c_husky",50,50) {
                IsAnimal = true,
                DebugName = "K9_Shepherd",
                UnitCode = "K9",
                RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>()),
                OverrideAgencyLessLethalWeapons = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideLessLethalWeaponsID = null,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
            },

            //Snipers
            new DispatchablePerson("mp_m_freemode_01",0,0) { DebugName = "<Male METRO Div Class B Sniper>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
               })
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

            //Dirt Bike
            new DispatchablePerson("mp_m_freemode_01",0,0) 
            { 
                DebugName = "<Male LSPD Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,16,0),
                        new PedPropComponent(1,25,0),
                    }, 
                    new List<PedComponent>() 
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,152,0),
                        new PedComponent(3,179,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,55,0),
                        new PedComponent(4,67,11),
                        new PedComponent(6,47,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,18,7),
                        new PedComponent(5,48,0),
                    })
            },
            new DispatchablePerson("mp_f_freemode_01",0,0) 
            {
                DebugName = "<Female LSPD Dirtbike Unit>", 
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                      new PedPropComponent(0,16,0),
                      new PedPropComponent(1,27,0),
                    }, 
                    new List<PedComponent>() {
                      new PedComponent(1,0,0),
                      new PedComponent(11,149,0),
                      new PedComponent(3,18,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,32,0),
                      new PedComponent(4,69,11),
                      new PedComponent(6,48,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,22,7),
                      new PedComponent(5,48,0),
                    }) 
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) 
            { 
                DebugName = "<Male LSPD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,49,0),
                    }, 
                    new List<PedComponent>() 
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,93,2),
                      new PedComponent(3,19,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,94,0),
                      new PedComponent(4,12,2),
                      new PedComponent(6,2,0),
                      new PedComponent(7,8,0),
                      new PedComponent(9,0,0),
                      new PedComponent(5,48,0),
                    }) 
            },
            new DispatchablePerson("mp_f_freemode_01",0,0) 
            {
                DebugName = "<Female LSPD Bicycle Uniform>", 
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,47,0),
                    }, 
                    new List<PedComponent>() 
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,84,2),
                        new PedComponent(3,31,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,2,0),
                        new PedComponent(4,14,2),
                        new PedComponent(6,10,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,0,0),
                        new PedComponent(5,48,0),
                    }) 
                },
        };
        List<DispatchablePerson> LSIAPDPeds_FEJ = new List<DispatchablePerson>() {
            //LSIA Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",2,2) { OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,DebugName = "<Male LSIA Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",2,2) {OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,DebugName = "<Female LSIA Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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

            //Animals
            new DispatchablePerson("a_c_husky",50,50) {
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
            },
            new DispatchablePerson("a_c_retriever",50,50) {
                IsAnimal = true,
                DebugName = "K9_Retriever",
                UnitCode = "K9",
                OverrideAgencyLessLethalWeapons = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideLessLethalWeaponsID = null,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
            },
            //Snipers
            new DispatchablePerson("mp_m_freemode_01",0,0) { DebugName = "<Male LSIA Utility Class B Sniper>",RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
               })
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
            }
        };
        List<DispatchablePerson> RHPDCops_FEJ = new List<DispatchablePerson>() {
            //RHPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
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
    OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Jacket>", RandomizeHead = true,
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
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Polo>", RandomizeHead = true,
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
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class B>", RandomizeHead = true,
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
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class C>", RandomizeHead = true,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Suit>", RandomizeHead = true,
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
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Detective>", RandomizeHead = true,
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
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Armor Protection>", RandomizeHead = true,MinWantedLevelSpawn = 3,
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
                ArmorMin = 50,ArmorMax = 50,GroupName = "SWAT",DebugName = "<Male RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
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
            OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Utility Class B>", RandomizeHead = true,
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
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Polo>", RandomizeHead = true,
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
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Jacket>", RandomizeHead = true,
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
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, GroupName = "Detective", DebugName = "<Female RHPD Detective>", RandomizeHead = true,
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
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, GroupName = "Detective", DebugName = "<Female RHPD Detective (vest)>", RandomizeHead = true,
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
                ArmorMin = 50,ArmorMax = 50,GroupName = "SWAT", DebugName = "<Female RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
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

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0,0){
                DebugName = "<Male RHPD Utility Class C Sniper>", RandomizeHead = true,
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
                    })
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

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) { 
                DebugName = "<Male RHPD Bicycle Patrol>",
                GroupName = "Bicycle",
                RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {



  new PedPropComponent(0,49,0),


  }, new List<PedComponent>() {
  new PedComponent(1,101,0),
  new PedComponent(11,311,16),
  new PedComponent(3,0,0),
  new PedComponent(10,0,0),
  new PedComponent(8,94,1),
  new PedComponent(4,12,2),
  new PedComponent(6,2,0),
  new PedComponent(7,8,0),
  new PedComponent(9,0,0),
  new PedComponent(5,48,0),
   }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { 
                DebugName = "<Female RHPD Bicycle Patrol>", GroupName = "Bicycle", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {



              new PedPropComponent(0,47,0),


              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,335,16),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,33,1),
              new PedComponent(4,14,2),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) },
        };
        List<DispatchablePerson> DPPDCops_FEJ = new List<DispatchablePerson>() {
            //DPPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",65,65) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",65,65) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01", 15, 0){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Jacket>", RandomizeHead = true,
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
            new DispatchablePerson("mp_f_freemode_01", 15, 0){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Jacket>", RandomizeHead = true,
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
            new DispatchablePerson("mp_m_freemode_01", 35, 5){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Polo>", RandomizeHead = true,
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
            new DispatchablePerson("mp_f_freemode_01", 35, 5){
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Polo>", RandomizeHead = true,
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
            new DispatchablePerson("mp_m_freemode_01", 2, 25){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class B>", RandomizeHead = true,
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
            new DispatchablePerson("mp_m_freemode_01", 2, 25){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class C>", RandomizeHead = true,
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
            new DispatchablePerson("mp_f_freemode_01", 2, 25){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class B>", RandomizeHead = true,
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
            new DispatchablePerson("mp_f_freemode_01", 2, 25){
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class C>", RandomizeHead = true,
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
                ArmorMin = 50,ArmorMax = 50,GroupName = "SWAT",
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
                ArmorMin = 50,ArmorMax = 50,GroupName = "SWAT",
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

            //Bike
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male DPPD Bicycle Patrol>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 49, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 17),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 12, 2),
                        new PedComponent(6, 2, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female DPPD Bicycle Patrol>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 47, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 17),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 14, 2),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },

            //Detective
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male DPPD Suit>",
                GroupName = "Detective",
                RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 1),
                        new PedComponent(4, 22, 1),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 39, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male DPPD Detective>",
                GroupName = "Detective",
                RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(6, 4, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 292, 1),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 1),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 39, 0),
                        new PedComponent(5, 0, 0),
                })
            },

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male DPPD Utility Class C>", RandomizeHead = true,
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
                                                        })            
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
            ,AccuracyMax = 85},

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) { 
                DebugName = "<Male DPPD Bicycle Patrol>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
                new PedPropComponent(0,49,0),
                }, new List<PedComponent>() {
                new PedComponent(1,0,0),
                new PedComponent(11,311,17),
                new PedComponent(3,0,0),
                new PedComponent(10,0,0),
                new PedComponent(8,94,1),
                new PedComponent(4,12,2),
                new PedComponent(6,2,0),
                new PedComponent(7,8,0),
                new PedComponent(9,37,0),
                new PedComponent(5,48,0),
                }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) {
                DebugName = "<Female DPPD Bicycle Patrol>", 
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
                new PedPropComponent(0,47,0),
                }, new List<PedComponent>() {
                new PedComponent(1,0,0),
                new PedComponent(11,335,17),
                new PedComponent(3,14,0),
                new PedComponent(10,0,0),
                new PedComponent(8,33,1),
                new PedComponent(4,14,2),
                new PedComponent(6,10,0),
                new PedComponent(7,8,0),
                new PedComponent(9,0,0),
                new PedComponent(5,48,0),
                }) },

        };


        List<PedPropComponent> MaleSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        MaleSheriffOptionalProps.AddRange(MaleCopOptionalProps);
        List<PedPropComponent> MaleSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        MaleSheriffShortSleeveOptionalProps.AddRange(MaleCopShortSleeveOptionalProps);


        List<PedPropComponent> FemaleSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        FemaleSheriffOptionalProps.AddRange(FemaleCopOptionalProps);
        List<PedPropComponent> FemaleSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        FemaleSheriffShortSleeveOptionalProps.AddRange(FemaleCopShortSleeveOptionalProps);


        List<DispatchablePerson> SheriffPeds_FEJ = new List<DispatchablePerson>() {

            new DispatchablePerson("s_m_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },

            //LSSD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
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
            OptionalProps = MaleSheriffOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
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
            OptionalProps = FemaleSheriffOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01",25,25) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
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
            new DispatchablePerson("mp_f_freemode_01",25,25) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
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
            new DispatchablePerson("mp_m_freemode_01",55,55) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
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
            OptionalProps = MaleSheriffShortSleeveOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 1, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 2, 0, 0),
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",55,55) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
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
            OptionalProps = FemaleSheriffShortSleeveOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01", 15, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Male LSSD Polo>",
                OptionalProps = MaleCopShortSleeveOptionalProps,
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
            new DispatchablePerson("mp_f_freemode_01", 15, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Female LSSD Polo>",
                OptionalProps = FemaleCopShortSleeveOptionalProps,
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
                MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                DebugName = "<Male LSSD SWAT Uniform>",
                GroupName = "SWAT",
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
                MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                DebugName = "<Female LSSD SWAT Uniform>",
                GroupName = "SWAT",
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Armor Protection>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Raid Jacket>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Gang Detail Jacket>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Armor Protection>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Raid Jacket>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 318, 1),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 217, 12),
                                                            new PedComponent(4, 3, 6),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 21, 0),
                                                            new PedComponent(5, 60, 1),
                                                        })},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Plain Clothes>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
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
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Gang Detail Jacket>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
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

            //Motor Units
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
                {
                    GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                    {
                        new PedPropComponent(0, 17, 2),
                    }, new List < PedComponent > ()
                    {
                        new PedComponent(1, 0, 0),
                            new PedComponent(11, 200, 2),
                            new PedComponent(3, 20, 0),
                            new PedComponent(10, 0, 0),
                            new PedComponent(8, 38, 1),
                            new PedComponent(4, 32, 2),
                            new PedComponent(6, 13, 0),
                            new PedComponent(7, 8, 0),
                            new PedComponent(9, 0, 0),
                            new PedComponent(5, 53, 0),
                    })
                },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 2),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 2),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 2),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 2),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 2),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                })
            },

            //Dirtbike
            new DispatchablePerson("mp_m_freemode_01",0,0) 
            { 
                DebugName = "<Male LSSD Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,25,0),
                    }, 
                    new List<PedComponent>() 
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,152,1),
                      new PedComponent(3,179,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,55,0),
                      new PedComponent(4,67,10),
                      new PedComponent(6,47,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,18,1),
                      new PedComponent(5,48,0),
                    }
                ) 
            },
            new DispatchablePerson("mp_f_freemode_01",0,0) 
            {
                DebugName = "<Female LSSD Dirtbike Unit>", 
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation
                ( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,27,0),
                    }, 
                    new List<PedComponent>() 
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,149,1),
                        new PedComponent(3,18,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,32,0),
                        new PedComponent(4,69,10),
                        new PedComponent(6,48,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,22,1),
                        new PedComponent(5,48,0),
                    }
                ) 
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) 
            { 
                DebugName = "<Male LSSD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,49,0),
                    }, 
                    new List<PedComponent>() 
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,94,2),
                        new PedComponent(3,19,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,37,0),
                        new PedComponent(4,12,1),
                        new PedComponent(6,2,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,14,0),
                        new PedComponent(5,48,0),
                    }
                    ) 
            },
            new DispatchablePerson("mp_f_freemode_01",0,0) 
            {
                DebugName = "<Female LSSD Bicycle Uniform>", 
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() 
                    {
                        new PedPropComponent(0,47,0),
                    }, 
                    new List<PedComponent>() 
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,85,2),
                        new PedComponent(3,31,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,2,0),
                        new PedComponent(4,14,3),
                        new PedComponent(6,10,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,16,0),
                        new PedComponent(5,48,0),
                    }) 
            },


        };


        List<PedPropComponent> MaleBCSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        MaleBCSheriffOptionalProps.AddRange(MaleCopOptionalProps);
        List<PedPropComponent> MaleBCSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        MaleBCSheriffShortSleeveOptionalProps.AddRange(MaleCopShortSleeveOptionalProps);


        List<PedPropComponent> FemaleBCSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        FemaleBCSheriffOptionalProps.AddRange(FemaleCopOptionalProps);
        List<PedPropComponent> FemaleBCSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        FemaleBCSheriffShortSleeveOptionalProps.AddRange(FemaleCopShortSleeveOptionalProps);


        List<DispatchablePerson> BCSheriffPeds_FEJ = new List<DispatchablePerson>() {
            // BCSO Class A
            new DispatchablePerson("mp_m_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 200, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = MaleBCSheriffOptionalProps,
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
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 202, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = FemaleBCSheriffOptionalProps,
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 10, 1, 0),  // Ranks
                        new PedComponent(10, 10, 4, 0), new PedComponent(10, 53, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    }},

            // BCSO Class B
            new DispatchablePerson("mp_m_freemode_01", 25, 25){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
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

            new DispatchablePerson("mp_f_freemode_01", 25, 25){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
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
            new DispatchablePerson("mp_f_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
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
                ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male BCSO Armor Protection>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
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
                ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Female BCSO Suit>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
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
                AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,GroupName = "SWAT"
                ,MinWantedLevelSpawn = 3, DebugName = "<Male BCSO SWAT Uniform>",
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
                AccuracyMin = 25
                ,AccuracyMax = 30
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 0
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,GroupName = "SWA"
                ,MinWantedLevelSpawn = 3, DebugName = "<Female BCSO SWAT Uniform>",
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

            //Motorcycle
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
    {
        GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
        {
            new PedPropComponent(0, 17, 3),
        }, new List < PedComponent > ()
        {
            new PedComponent(1, 0, 0),
                new PedComponent(11, 200, 3),
                new PedComponent(3, 20, 0),
                new PedComponent(10, 0, 0),
                new PedComponent(8, 38, 0),
                new PedComponent(4, 32, 2),
                new PedComponent(6, 13, 0),
                new PedComponent(7, 8, 0),
                new PedComponent(9, 0, 0),
                new PedComponent(5, 54, 0),
        })
    },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 3),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 0),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 3),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 0),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 3),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 3),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 3),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                })
            },

            //Dirtbike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male BCSO Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,152,0),
                      new PedComponent(3,179,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,55,0),
                      new PedComponent(4,67,10),
                      new PedComponent(6,47,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,18,1),
                      new PedComponent(5,48,0),
                    }
                )
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female BCSO Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,149,0),
                        new PedComponent(3,18,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,32,0),
                        new PedComponent(4,69,11),
                        new PedComponent(6,48,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,22,0),
                        new PedComponent(5,48,0),
                    }
                )
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) {
                DebugName = "<Male BCSO Bicycle Uniform>",
                RandomizeHead = true,
                               GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
 


              new PedPropComponent(0,49,0),


              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,94,1),
              new PedComponent(3,19,0),
              new PedComponent(10,0,0),
              new PedComponent(8,37,0),
              new PedComponent(4,12,1),
              new PedComponent(6,2,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) {
                DebugName = "<Female BCSO Bicycle Uniform>", 
                RandomizeHead = true,
                                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {

              new PedPropComponent(0,47,0),


              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,85,1),
              new PedComponent(3,31,0),
              new PedComponent(10,0,0),
              new PedComponent(8,2,0),
              new PedComponent(4,14,3),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,16,0),
              new PedComponent(5,48,0),
               }) },

            //Pilot
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
        };

        List<DispatchablePerson> NOOSEPIAPeds_FEJ = new List<DispatchablePerson>() {
            //Class A
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
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
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
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
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 20, 20){
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
            new DispatchablePerson("mp_f_freemode_01", 20, 20){
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
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 35, 35){
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
            new DispatchablePerson("mp_f_freemode_01", 35, 35){
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
            //Jacket
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
            new DispatchablePerson("mp_m_freemode_01",0,50) { 
                MinWantedLevelSpawn = 4, 
                MaxWantedLevelSpawn = 4,
                AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                DebugName = "<Male PIA TRU Uniform>",GroupName = "SWAT", RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,50) {
                MinWantedLevelSpawn = 4, 
                MaxWantedLevelSpawn = 4
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                GroupName = "SWAT",
                DebugName = "<Female PIA TRU Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            //Suit
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Suit>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 13, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Suit>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 24, 3),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 2),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            //Special Agent
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Special Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 0),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Special Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 27, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Field Agent
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Field Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 47, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Field Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 54, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Windbreaker
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Windbreaker>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 5),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 28, 3),
                        new PedComponent(4, 47, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Windbreaker>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 318, 5),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 104, 2),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 28, 3),
                })
            },
        };
        List<DispatchablePerson> NOOSESEPPeds_FEJ = new List<DispatchablePerson>()
        {
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 10, 35) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 35) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 15),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            //Jacket
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 135, 17),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 56, 4),
                })
            },   
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 134, 17),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 5),
                        new PedComponent(5, 61, 4),
                })
            },
            //RIOT
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Riot Gear>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 150, 0),
                        new PedPropComponent(1, 26, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 150, 6),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 125, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Riot Gear>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 149, 0),
                        new PedPropComponent(1, 23, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 147, 6),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 131, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            //TRU
            new DispatchablePerson("mp_m_freemode_01", 0, 50) {
                MinWantedLevelSpawn = 4, 
                MaxWantedLevelSpawn = 4,
                AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,GroupName = "SWAT"
                ,DebugName = "<Male SEP TRU Uniform>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 39, 1),
                        new PedPropComponent(1, 26, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 220, 9),
                        new PedComponent(3, 141, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 37, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 5),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 50) {
                MinWantedLevelSpawn = 4, 
                MaxWantedLevelSpawn = 4,
                AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                ,GroupName = "SWAT"
                ,DebugName = "<Female SEP TRU Uniform>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 38, 1),
                        new PedPropComponent(1, 28, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 230, 9),
                        new PedComponent(3, 174, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 36, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 5),
                        new PedComponent(5, 48, 0),
                })
            },
            //Juggernaut - ewwww
            new DispatchablePerson("mp_m_freemode_01", 0, 0) {
                HasFullBodyArmor = true
                ,DisableBulletRagdoll = true
                ,DisableCriticalHits = true
                ,FiringPatternHash = -957453492//fullauto
                ,HealthMin = 200
                ,HealthMax = 200
                ,ArmorMin = 500
                ,ArmorMax = 500
                ,AllowRandomizeBeforeVariationApplied = true
                ,OverrideAgencySideArms = true
                ,OverrideSideArmsID = "Minigun"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "Minigun"
                ,ShootRateMin = 500
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,MinWantedLevelSpawn = 4
                ,MaxWantedLevelSpawn = 4,
                DebugName = "<Male Juggernaut>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                    new PedPropComponent(0, 93, 0),

                }, new List < PedComponent > () {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 186, 0),
                        new PedComponent(3, 110, 3),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 97, 0),
                        new PedComponent(4, 84, 0),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0) {
                HasFullBodyArmor = true
                ,DisableBulletRagdoll = true
                ,DisableCriticalHits = true
                ,FiringPatternHash = -957453492//fullauto
                ,HealthMin = 200
                ,HealthMax = 200
                ,ArmorMin = 500
                ,ArmorMax = 500
                ,AllowRandomizeBeforeVariationApplied = true
                ,OverrideAgencySideArms = true
                ,OverrideSideArmsID = "Minigun"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "Minigun"
                ,ShootRateMin = 500
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,MinWantedLevelSpawn = 4
                ,MaxWantedLevelSpawn = 4,
                DebugName = "<Female Juggernaut>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 92, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 188, 0),
                        new PedComponent(3, 127, 3),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 105, 0),
                        new PedComponent(4, 86, 0),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0, 0) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Class C Sniper>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                })
            ,GroupName = "Sniper"
            ,OverrideAgencyLongGuns = true
            ,OverrideLongGunsID = "GoodSniperLongGuns"
            ,AlwaysHasLongGun = true
            ,CombatRange = 3
            ,CombatMovement = 0
            ,AccuracyMin = 65
            ,AccuracyMax = 85
            },
        };
        List<DispatchablePerson> BorderPatrolPeds_FEJ = new List<DispatchablePerson>()
        {
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male Border Patrol Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 212, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female Border Patrol Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                })
            },
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 15, 15) {
                DebugName = "<Male Border Patrol Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15) {
                DebugName = "<Female Border Patrol Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                })
            },
            //Jacket
            new DispatchablePerson("mp_m_freemode_01", 15, 15) {
                DebugName = "<Male Border Patrol Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 8),
                        new PedComponent(5, 56, 6),
                })
            },         
            new DispatchablePerson("mp_f_freemode_01", 15, 15) {
                DebugName = "<Female Border Patrol Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 7),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 8),
                        new PedComponent(5, 61, 6),
                })
            },
        };

        List<DispatchablePerson> MarshalsServicePeds_FEJ = new List<DispatchablePerson>()
        {
            //Suit
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male USMS Suit>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 4),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 13, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female USMS Suit>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 24, 6),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 64, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Regular
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male USMS Marshal>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female USMS Marshal>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 221, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Response - Has Armor
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Response>",MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 11),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Female USMS Response>",MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 332, 0),
                        new PedComponent(3, 1, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 161, 11),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Windbreaker has armor under
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Windbreaker>", MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 7),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 4),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 17, 7),
                        new PedComponent(5, 62, 0),
                })
            },
            //Field Agent - Has Armor
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Field Agent>", MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 135, 14),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 7),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 133, 13),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 77, 0),
                })
            },
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
            new DispatchablePerson("mp_m_freemode_01",0,40) { ArmorMin = 50,ArmorMax = 50, MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,40) { ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Female FIB Response>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",0,50) {
                AccuracyMin = 30
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                GroupName = "FIBHET",
                MinWantedLevelSpawn = 5, 
                MaxWantedLevelSpawn = 5, 
                DebugName = "<Male FIB SWAT Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",0,50) {
                AccuracyMin = 30
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100,
                GroupName = "FIBHET",
                MinWantedLevelSpawn = 5, 
                MaxWantedLevelSpawn = 5, 
                DebugName = "<Female FIB SWAT Uniform>", 
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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

            new DispatchablePerson("mp_m_freemode_01",0,0) { 
                ArmorMin = 50,
                ArmorMax = 50,
                MinWantedLevelSpawn = 5, 
                MaxWantedLevelSpawn = 5, 
                DebugName = "<Male FIB SWAT Uniform Sniper>",
                RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }, 
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
               })           
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
        List<DispatchablePerson> SanAndreasStateParksRangers_FEJ = new List<DispatchablePerson>() {
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
            new DispatchablePerson("mp_m_freemode_01",10,5) { DebugName = "<Male DOA Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",15,5) {DebugName = "<Female DOA Agent>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",0,50) { ArmorMin = 50,ArmorMax = 50,DebugName = "<Male DOA Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopOptionalProps,
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01",25,25) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopOptionalProps,
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
            new DispatchablePerson("mp_f_freemode_01",25,25) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01",55,55) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopShortSleeveOptionalProps,
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
            new DispatchablePerson("mp_f_freemode_01",55,55) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopShortSleeveOptionalProps,
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 2,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 2,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_m_freemode_01",0,20) { 
                RandomizeHead = true,MinWantedLevelSpawn = 3, 
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",0,20) { RandomizeHead = true,MinWantedLevelSpawn = 3, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
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

            //Dirt Bike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male SAHP Dirtbike patrol>",
                RandomizeHead = true,
                MaxWantedLevelSpawn = 2,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,0),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,152,0),
                        new PedComponent(3,179,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,55,0),
                        new PedComponent(4,67,11),
                        new PedComponent(6,47,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,21,2),
                        new PedComponent(5,48,0),
                    })
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female SAHP Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                MaxWantedLevelSpawn = 2,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                      new PedPropComponent(0,16,0),
                      new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>() {
                      new PedComponent(1,0,0),
                      new PedComponent(11,149,0),
                      new PedComponent(3,18,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,32,0),
                      new PedComponent(4,69,11),
                      new PedComponent(6,48,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,19,2),
                      new PedComponent(5,48,0),
                    })
            },

            new DispatchablePerson("mp_m_freemode_01",0,0) 
            { 
                DebugName = "<Male SAHP Pilot Uniform>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() {



              new PedPropComponent(0,79,0),


              }, 
                    new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,108,3),
              new PedComponent(3,96,0),
              new PedComponent(10,0,0),
              new PedComponent(8,48,0),
              new PedComponent(4,64,2),
              new PedComponent(6,24,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }) 
            },

            new DispatchablePerson("mp_f_freemode_01",0,0) 
            {
                DebugName = "<Female SAHP Pilot Uniform>", 
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, 
                RequiredVariation = new PedVariation( 
                    new List<PedPropComponent>() {



  new PedPropComponent(0,78,0),
  new PedPropComponent(1,13,0),

  }, 
                    new List<PedComponent>() {
  new PedComponent(1,0,0),
  new PedComponent(11,99,3),
  new PedComponent(3,36,0),
  new PedComponent(10,0,0),
  new PedComponent(8,29,0),
  new PedComponent(4,66,2),
  new PedComponent(6,24,0),
  new PedComponent(7,8,0),
  new PedComponent(9,0,0),
  new PedComponent(5,48,0),
   }) 
            },
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
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
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
            new DispatchablePerson("mp_m_freemode_01", 20, 20){
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
            new DispatchablePerson("mp_m_freemode_01", 55, 55){
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
            new DispatchablePerson("mp_m_freemode_01", 5, 15){
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
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
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
            new DispatchablePerson("mp_f_freemode_01", 20, 20){
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
            new DispatchablePerson("mp_f_freemode_01", 55, 55){
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
            new DispatchablePerson("mp_f_freemode_01", 10, 15){
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
            new DispatchablePerson("mp_f_freemode_01", 10, 20){
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
                DebugName = "<Male LSPP Plain Clothes>", GroupName = "Detective", RandomizeHead = true,
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
                DebugName = "<Female LSPP Plain clothes>",GroupName = "Detective",  RandomizeHead = true,
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
            //motorcycle
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
    {
        GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
        {
            new PedPropComponent(0, 17, 5),
        }, new List < PedComponent > ()
        {
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
        })
    },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
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
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
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
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
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
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
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
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
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
                })
            },
        };
        List<DispatchablePerson> LSPDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Pilot", DebugName = "<Male LSPD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Pilot", DebugName = "<Female LSPD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            //LSPD SWAT
            new DispatchablePerson("mp_m_freemode_01",40,40) { 
                ArmorMin = 50,
                ArmorMax = 50,
                DebugName = "<Male LSPD SWAT Uniform ASD>",
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
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
            new DispatchablePerson("mp_f_freemode_01",40,40) { 
                ArmorMin = 50,
                ArmorMax = 50,
                DebugName = "<Female LSPD SWAT Uniform ASD>",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
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
        };
        List<DispatchablePerson> LSSDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Pilot",DebugName = "<Male LSSD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            new DispatchablePerson("mp_f_freemode_01",5,5)  { GroupName = "Pilot",DebugName = "<Female LSSD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
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
            // Swat
            new DispatchablePerson("mp_m_freemode_01", 40, 40){
                ArmorMin = 50,
                ArmorMax = 50,
                DebugName = "<Male LSSD SWAT Uniform ASD>",
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
            new DispatchablePerson("mp_f_freemode_01", 40, 40){
                ArmorMin = 50,
                ArmorMax = 50,
                DebugName = "<Female LSSD SWAT Uniform ASD>",
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
        };
        List<DispatchablePerson> NYSP_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_m_snowcop_01",0,0)  { DebugName = "NYSPDefaultMale" },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 13, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 16),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 17),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male NYSP Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 17),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 5),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 13),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 12),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Parka>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 16, 0),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 33, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 26, 5),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 13, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 16),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 17),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female NYSP Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 17),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 5),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 12),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Parka>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 44, 0),
                        new PedComponent(3, 49, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 32, 0),
                        new PedComponent(6, 53, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        List<DispatchablePerson> LSDRP1_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 20),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 20),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class C>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 20),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSRDP Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 16),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 20),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 20),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class C>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 20),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 70, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSDRP Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 10),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 16),
                })
            },
        };//only this is filled out in the base one
        List<DispatchablePerson> GameWarden_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 18),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 18),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 18),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 11),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Utility>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 13),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male Game Warden Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 15),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male Game Warden Raincoat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 4),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 18),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 18),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 18),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 80, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female Game Warden Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 6),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 15),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Utility>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 12),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 13),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 11),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        List<DispatchablePerson> USNPSParkRangers_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 19),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Polo>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 11),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 12),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger K-9>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 11),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 12),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 78, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger Raincoat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 4),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 19),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Ranger Polo>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 12),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LE Park Ranger K-9>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 11),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 12),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LE Park Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 7),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 78, 0),
                })
            },

            //NON LE BELOW
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 19),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 19),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
        };


        //NEW EUP ADDON STUFF
        List<DispatchablePerson> LSFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male LSFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 21),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 15, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female LSFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 15, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 6),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 7),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 6),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 7),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 7),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 21),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 1),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 57, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 65, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 64, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSFD Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 0),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> LSFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 0),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 0),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 0),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 0),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };


        List<DispatchablePerson> LSCoFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male LSCoFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female LSCoFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 2),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 1),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 58, 1),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 66, 1),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 4),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 8, 4),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 4),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 9, 4),
                        new PedComponent(5, 48, 0),
                })
            },
        };

        List<DispatchablePerson> LSCoFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSCoFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 3),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSCoFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 3),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSCoFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 2),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSCoFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 2),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSCoFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSCoFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        List <DispatchablePerson> BCFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 18, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male BCFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 18, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female BCFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 5),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 5),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 0),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 58, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 0),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 66, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 2),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 2),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 5),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 8, 3),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 5),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 9, 3),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        List<DispatchablePerson> BCFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male BCFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 4),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male BCFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 4),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female BCFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 4),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female BCFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 4),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female BCFD EMT T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        List <DispatchablePerson> SanFire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Sanfire Brush Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 80, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 250, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 86, 23),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Brush Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 79, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 258, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 89, 23),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 7),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire LEO Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire LEO Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire LEO Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire LEO Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 25),
                })
            },
        };
        List<DispatchablePerson> Lifeguards_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male Lifeguard Bathsuit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 15, 0),
                        new PedComponent(3, 15, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female Lifeguard Swimsuit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 11, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 18, 0),
                        new PedComponent(4, 17, 4),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male Lifeguard Clothes>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 22, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 41, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 15, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female Lifeguard Clothes>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 23, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male ATV Patrol>",
                RandomizeHead = true,
                GroupName = "ATV",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 22, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 41, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 15, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female ATV Patrol>",
                RandomizeHead = true,
                GroupName = "ATV",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 23, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 46, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Nautical Patrol>",
                RandomizeHead = true,
                GroupName = "Boat",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 15, 0),
                        new PedComponent(3, 15, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Nautical Patrol>",
                RandomizeHead = true,
                GroupName = "Boat",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 11, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 18, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 47, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        List<DispatchablePerson> SanAndreasMedicalServices_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 6),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 6),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 24),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 6),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 6),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SAMS Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 0),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 59, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female SAMS Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 0),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 67, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male SAMS Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female SAMS Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 1),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male CLSMD Scrubs>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 2),
                        new PedComponent(11, 32, 3),
                        new PedComponent(3, 85, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 45, 5),
                        new PedComponent(6, 42, 2),
                        new PedComponent(7, 127, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female CLSMD Scrubs>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 2),
                        new PedComponent(11, 31, 3),
                        new PedComponent(3, 109, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 47, 5),
                        new PedComponent(6, 10, 1),
                        new PedComponent(7, 97, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        List<DispatchablePerson> CoastGuard_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Fatigues>",//just plain grey.blue BDU
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Fatigues>",//just plain grey.blue BDU
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 3),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 54, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 50, 50)
            {
                DebugName = "<Male U.S Coast Guard>",//BDU LifeVest anbd Helmet, boater
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 0),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 41, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 46, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 50, 50)
            {
                DebugName = "<Female U.S Coast Guard>",//BDU LifeVest anbd Helmet, boater
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 38, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 54, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 48, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Swat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 150, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 220, 12),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 16, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG SWAT>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 149, 0),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 12),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 18, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Onepiece>",//Orange One Piece SUit without flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 0),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 155, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Onepiece>",//Orange One Piece SUit without flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 0),
                        new PedPropComponent(1, 27, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 152, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Swimmer>",//Orange One Piece SUit with flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 1),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 155, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 0),
                        new PedComponent(6, 67, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Swimmer>",//Orange One Piece SUit with flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 1),
                        new PedPropComponent(1, 27, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 152, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 0),
                        new PedComponent(6, 70, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Jumpsuit>",//Looks like a pilot outfit
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 10),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 38, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Jumpsuit>",//Looks like a pilot outfit
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 10),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 38, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 155, 2),
                        new PedComponent(3, 16, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 152, 2),
                        new PedComponent(3, 17, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        //Security
        List<DispatchablePerson> MerryweatherSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 50, 50)
            {
                DebugName = "<Male Merryweather Security Guard>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 50, 50)
            {
                DebugName = "<Female Merryweather Security Guard>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> GruppeSechSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 41, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 0),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 40, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 0),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },

            //Armored
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Divison Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 41, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 75, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Divison Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 1),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 73, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Division Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 1),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 73, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 40, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 84, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 1),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 82, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 1),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 82, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                })
            },

        };
        List<DispatchablePerson> BobcatSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 13),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 13),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 4),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 11),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 13),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 13),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 23),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 3, 12),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 4),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 3, 12),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                })
            },
        };
        List<DispatchablePerson> SecuroServ_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class A>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class B>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class C>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class A>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class B>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class C>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
        };
        List<DispatchablePerson> LockNLoadSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male L&L Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male L&L Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male L&L Armored Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 2),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male L&L Armored Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 2),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female L&L Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female L&L Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 15),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored L&L Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 2),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored L&L Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 2),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 20, 0),
                })
            },
        };
        List<DispatchablePerson> ChuffSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Chuff Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 14),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Chuff Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Chuff Armored Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 14),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 1),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Chuff Armored Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 1),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Chuff Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 14),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Chuff Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 14),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Chuff Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 14),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 1),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Chuff Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 1),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                })
            },
        };


        //Cops
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("StandardCops", StandardCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSIAPDPeds", LSIAPDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RHPDCops", RHPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DPPDCops", DPPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCSheriffPeds", BCSheriffPeds_FEJ));



        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSEPIAPeds", NOOSEPIAPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSESEPPeds", NOOSESEPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BorderPatrolPeds", BorderPatrolPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("OffDutyCops", OffDutyCops));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkRangers", SanAndreasStateParksRangers_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USNPSParkRangers", USNPSParkRangers_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSDPRParkRangers", LSDRP1_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SADFWParkRangers", GameWarden_FEJ));


        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSLifeguardPeds", LSLifeguardPeds));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ArmyPeds", ArmyPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USMCPeds", USMCPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USAFPeds", USAFPeds));




        PeopleConfig_EUP.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_FEJ));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NYSPPeds", NYSP_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BlueEMTs", BlueEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GreenEMTs", GreenEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPPPeds", LSPPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds_FEJ));


        //Gangs
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LostMCPeds", LostMCPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DiablosPeds", DiablosPeds));
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

        //Other
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("OtherPeds", OtherPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("TaxiDrivers", TaxiDrivers));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RegularPeds", RegularPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("VendorPeds", VendorPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", IllicitMarketplacePeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("TellerPeds", TellerPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BurgerShotPeds", BurgerShotPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CluckinBellPeds", CluckinBellPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("TwatPeds", TwatPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GunshopPeds", GunshopPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BarPeds", BarPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("HaircutPeds", HaircutPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BobMuletPeds", BobMuletPeds));

        //NEW
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSFDPeds", LSFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSFDEMTPeds", LSFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSCOFDPeds", LSCoFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSCOFDEMTPeds", LSCoFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCFDPeds", BCFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCFDEMTPeds", BCFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAMSEMTPeds", SanAndreasMedicalServices_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SanFirePeds", SanFire_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSLifeguardPeds", Lifeguards_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuard_FEJ));

        //Security
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));//used for lsspp in generic
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BobcatPeds", BobcatSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroServ_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LNLPeds", LockNLoadSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ChuffPeds", ChuffSecurity_FEJ));

        DefaultConfig_FullExpandedJurisdictionEXTRA(PeopleConfig_EUP);

        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\DispatchablePeople_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchablePeople_EUP.xml");

    }
    //SOme stuff I have no use for now, but maybe others want to use it?
    private void DefaultConfig_FullExpandedJurisdictionEXTRA(List<DispatchablePersonGroup> PeopleConfig_EUP)
    {
        List<DispatchablePerson> SearchAndRescue_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSSD SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 6),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 145, 0),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSSD SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 143, 0),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 7),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSSD SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 7),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 64, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSSD SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 7),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCSO SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 7),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 145, 3),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 3),
                        new PedComponent(5, 19, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCSO SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 143, 3),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 3),
                        new PedComponent(5, 19, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCSO SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 7),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 8),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 64, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 3),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCSO SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 7),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 8),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD USAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 8),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 171, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 8),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD SAR>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 24),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD SAR>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 2),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 24),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
        };
        List<DispatchablePerson> NationalParkService_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male NPS SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 2),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 25),
                        new PedComponent(3, 171, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 9, 0),
                })
            },
        };
        List<DispatchablePerson> MilitaryUnused_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male National Guard Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 22, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 87, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female National Guard Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 21, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 90, 0),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male U.S. Army Combat Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 39, 1),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 25),
                        new PedComponent(3, 141, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 37, 2),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female U.S. Army Combat Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 38, 1),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 25),
                        new PedComponent(3, 174, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 36, 2),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 17, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USAF Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 2, 1),
                        new PedComponent(4, 87, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAF Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 22, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 90, 0),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USAF Pilot Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 38, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 48, 0),
                        new PedComponent(3, 16, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 30, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 33, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAF Pilot Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 37, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 41, 0),
                        new PedComponent(3, 17, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 29, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> ParkingEnforcement_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 5),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 5),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 3),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking Enforcement Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 3),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> GenericSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Security Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 266, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Security Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 275, 8),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                })
            },

            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male VIP Security>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 121, 0),
                        new PedComponent(11, 10, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 178, 0),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 38, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Bouncer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 121, 0),
                        new PedComponent(11, 18, 3),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> MerryweatherPMC_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather Enforcer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                        new PedPropComponent(1, 23, 0),
                        new PedPropComponent(2, 1, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 21),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 4),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Merryweather Enforcer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 22),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 89, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 8, 4),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather PMC-I>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Merryweather PMC-I>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 90, 1),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather PMC-II>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 87, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Infiltration Operative>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 117, 0),
                        new PedPropComponent(1, 25, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 220, 21),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 9, 3),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Infiltrator Operative>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 116, 0),
                        new PedPropComponent(1, 27, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 230, 22),
                        new PedComponent(3, 18, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 6, 3),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        List<DispatchablePerson> ServiceAndTransit_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Sanitation Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 71, 0),
                        new PedComponent(3, 64, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 53, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanitation Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 67, 0),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 55, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Waste Collector>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 56, 0),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 59, 1),
                        new PedComponent(4, 36, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Waste Collector>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 50, 0),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 36, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Janitor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 65, 1),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 155, 0),
                        new PedComponent(4, 38, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Janitor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 59, 1),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 191, 0),
                        new PedComponent(4, 38, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 94, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Bugstars Overalls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 139, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 65, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 66, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 38, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Mechanic Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 1),
                        new PedComponent(3, 88, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Highway Clearance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 76, 19),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 3),
                        new PedComponent(3, 99, 7),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 3),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Towing Technician>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 2),
                        new PedComponent(3, 100, 6),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Mechanic Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 1),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Highway Clearance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 3),
                        new PedComponent(3, 117, 7),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 3),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Towing Technician>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 2),
                        new PedComponent(3, 190, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Private Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 25, 0),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 56, 0),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 89, 0),
                        new PedComponent(4, 0, 10),
                        new PedComponent(6, 12, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 89, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Private Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 53, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 49, 0),
                        new PedComponent(3, 72, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 0),
                        new PedComponent(4, 4, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 6),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Public Worker>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                        new PedPropComponent(1, 15, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 2, 5),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 90, 0),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 51, 3),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 89, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Public Worker>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 0),
                        new PedComponent(3, 83, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 4, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 5),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Dockworker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 8),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 364, 0),
                        new PedComponent(3, 195, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 182, 0),
                        new PedComponent(4, 135, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Dockworker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 8),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 383, 0),
                        new PedComponent(3, 240, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 220, 0),
                        new PedComponent(4, 142, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Public Worker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Public Worker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 2),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PostOp Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 9),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female PostOp Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 9),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PostOp Bermudas>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 12, 8),
                        new PedComponent(6, 12, 12),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Post Up polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male GoPostal Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female GoPostal Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 8),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 99, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male GoPostal Bermudas>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 12, 9),
                        new PedComponent(6, 12, 12),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female GoPostal polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 1),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 99, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Bus Driver>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 1),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Operator>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 6),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 2),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Maintenance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 154, 0),
                        new PedComponent(4, 13, 7),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Bus Driver>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 10),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Operator>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 11),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Maintenance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 12),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 191, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> Coroner_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Los Santos Coroner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 307, 20),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 28, 3),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 101, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Los Santos Coroner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 294, 19),
                        new PedComponent(3, 104, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 23, 1),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 101, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> MedicalGeneric_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Biohazard Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 57, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 67, 3),
                        new PedComponent(3, 88, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 62, 3),
                        new PedComponent(4, 40, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PPE Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 0),
                        new PedComponent(11, 67, 0),
                        new PedComponent(3, 88, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 61, 0),
                        new PedComponent(4, 40, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Biohazard Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 57, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 61, 3),
                        new PedComponent(3, 101, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 43, 3),
                        new PedComponent(4, 40, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female PPE Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 0),
                        new PedComponent(11, 61, 0),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 40, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Patient>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 104, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 29, 0),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Patient>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 95, 0),
                        new PedComponent(3, 8, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 17, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> Prisoner_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Prisoner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 25, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 45, 4),
                        new PedComponent(6, 42, 1),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Prisoner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 31, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 47, 4),
                        new PedComponent(6, 98, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SearchAndRescue_FEJ", SearchAndRescue_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NationalParkService_FEJ", NationalParkService_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MilitaryUnused_FEJ", MilitaryUnused_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkingEnforcement_FEJ", ParkingEnforcement_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GenericSecurity_FEJ", GenericSecurity_FEJ));    
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MerryweatherPMC_FEJ", MerryweatherPMC_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ServiceAndTransit_FEJ", ServiceAndTransit_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Coroner_FEJ", Coroner_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MedicalGeneric_FEJ", MedicalGeneric_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Prisoner_FEJ", Prisoner_FEJ));
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

