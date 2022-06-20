using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class DispatchablePeople : IDispatchablePeople
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\DispatchablePeople.xml";
    private List<DispatchablePersonGroup> PeopleGroupLookup = new List<DispatchablePersonGroup>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("DispatchablePeople*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            PeopleGroupLookup = Serialization.DeserializeParams<DispatchablePersonGroup>(ConfigFileName);
        }
        else
        {
            Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
            DefaultConfig();
            DefaultConfig_EUPBasic();
            Serialization.SerializeParams(PeopleGroupLookup, ConfigFileName);
        }
    }
    public List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID)
    {
        return PeopleGroupLookup.FirstOrDefault(x => x.DispatchablePersonGroupID == dispatchablePersonGroupID)?.DispatchablePeople;
    }
    private void DefaultConfig()
    {
        List<DispatchablePersonGroup> PeopleConfig_Default = new List<DispatchablePersonGroup>();

        //Cops
        List<DispatchablePerson> StandardCops = new List<DispatchablePerson>() {

        //s_m_y_cop_01
        //new PedPropComponent(0, 0, 0) Goofy Hat
        //new PedPropComponent(1, 0, 0) Spy Glasses Forward
        //new PedPropComponent(1, 1, 0) Spy Glasses Backwards
        //new PedPropComponent(1, 2, 0) Spy Glasses On Head
        //new PedPropComponent(1, 3, 0) Aviators

            //s_f_y_cop_01
            //new PedPropComponent(0, 0, 0) Goofy Hat
            //new PedPropComponent(1, 0, 0) Aviators

            new DispatchablePerson("s_f_y_cop_01",0,0) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } },//new PedPropComponent(0, 0, 0)//Hat,//new PedPropComponent(1, 0, 0)//Glasses
            new DispatchablePerson("mp_f_freemode_01",40,40) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  }), 
                OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 45, 0), new PedPropComponent(1, 25, 9) } },//no body armor with hat and glasses possible
            new DispatchablePerson("mp_f_freemode_01",0,40) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 3, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  }),
                OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 123, 15), new PedPropComponent(1, 25, 9) }},//body armor, with riot helmet and glasses possible


            new DispatchablePerson("mp_m_freemode_01",30,30) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_HWAYCOP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0, 46, 0),new PedPropComponent(1,23,9) }) },//no body armor with glasses and goofy hat


            new DispatchablePerson("s_m_y_cop_01",30,30) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) }) },//filled duty belt
            new DispatchablePerson("s_m_y_cop_01",0,60) { MinWantedLevelSpawn = 3
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0), new PedPropComponent(1, 2, 0), new PedPropComponent(1, 3, 0) }//Glasses only?
                , OptionalPropChance = 80
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 2, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 1, 1) }) },//vest, no hat
        };
        List<DispatchablePerson> SheriffPeds = new List<DispatchablePerson>() {
            


        //s_m_y_sheriff_01
        //new PedPropComponent(0, 0, 0) Flat Hat
        //new PedPropComponent(0, 1, 0) Cowboy Hat
        //new PedPropComponent(1, 0, 0) Aviators
        //new PedPropComponent(1, 1, 0) Spy Glasses Forwards


        //s_f_y_sheriff_01
        //new PedPropComponent(0, 0, 0) Flat Hat
        //new PedPropComponent(0, 1, 0) Cowboy Hat
        //new PedPropComponent(1, 0, 0) Aviators

            new DispatchablePerson("s_f_y_sheriff_01",35,35) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0) },OptionalPropChance = 70 },
            new DispatchablePerson("s_f_y_sheriff_01",0,15) { MinWantedLevelSpawn = 3
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0) },OptionalPropChance = 70 },
            new DispatchablePerson("s_m_y_sheriff_01",65,65) { MaxWantedLevelSpawn = 2, AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) },OptionalPropChance = 70 },//filled duty belt
            new DispatchablePerson("s_m_y_sheriff_01",0,85) { MinWantedLevelSpawn = 3
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 2, 0, 0) })
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) },OptionalPropChance = 95 },//filled duty belt},//vest
        };
        List<DispatchablePerson> NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 0,0){ RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) },

                        //old school SWAT
            new DispatchablePerson("mp_m_freemode_01", 100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0),new PedPropComponent(1, 23, 9)}) },
                        new DispatchablePerson("mp_m_freemode_01", 100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0),new PedComponent(4, 121, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 2, 0, 0),new PedComponent(10, 70, 0, 0),new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0)}) },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_03",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0),new PedPropComponent(1,25,9) }) },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_04",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0)}) },

        };
        List<DispatchablePerson> FIBPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 0,0) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) },


            //No helmet new school swat
            new DispatchablePerson("mp_m_freemode_01", 25, 25, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },//328 cool too
                new List<PedPropComponent>() { new PedPropComponent(0, 19, 0),new PedPropComponent(1, 15, 9), }) },
            new DispatchablePerson("mp_m_freemode_01", 75,75, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 46, 0, 0),new PedComponent(3, 17, 0, 0),new PedComponent(4, 129, 1, 0),new PedComponent(6, 24, 0, 0),new PedComponent(7, 148, 12, 0),new PedComponent(8, 130, 0, 0),new PedComponent(9, 15, 2, 0),new PedComponent(11, 54, 0, 0), },
                new List<PedPropComponent>() { new PedPropComponent(0, 117, 0),new PedPropComponent(1, 25, 4), }) },


            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(2, 42, 0, 0),new PedComponent(3, 18, 0, 0),new PedComponent(4, 130, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(8, 0, 0, 0),new PedComponent(9, 18, 2, 0),new PedComponent(11, 54, 3, 0), },
                new List<PedPropComponent>() { new PedPropComponent(0, 19, 0), }) },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(2, 42, 0, 0),new PedComponent(3, 18, 0, 0),new PedComponent(4, 130, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(8, 0, 0, 0),new PedComponent(9, 18, 2, 0),new PedComponent(11, 54, 3, 0), },
                new List<PedPropComponent>() { new PedPropComponent(0, 116, 0),new PedPropComponent(1, 27, 4), }) },



            };
        List<DispatchablePerson> ParkRangers = new List<DispatchablePerson>() {
            //Park Rangers
            //s_m_y_ranger_01
                //new PedPropComponent(0, 0, 0) Flat Hat
            //new PedPropComponent(0, 1, 0) Baseball Hat
            //new PedPropComponent(1, 0, 0) Aviators

            //s_f_y_ranger_01
            //new PedPropComponent(1, 0, 0) Aviators

            new DispatchablePerson("s_m_y_ranger_01",75,75) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}, OptionalPropChance = 70},
            new DispatchablePerson("s_f_y_ranger_01",25,25) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }, OptionalPropChance = 70 }
        
        };
        List<DispatchablePerson> DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) };
        List<DispatchablePerson> SAHPPeds = new List<DispatchablePerson>() {

            //Highway Patrol
            //s_m_y_hwaycop_01
            //new PedPropComponent(0, 0, 0) Helmet
            //new PedPropComponent(1, 0, 0) Spy Glasses Forward
            //new PedPropComponent(1, 1, 0) Aviators
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){ AllowRandomizeBeforeVariationApplied = true,GroupName = "StandardSAHP"
            , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }, OptionalPropChance = 70
            , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(4, 1, 0, 0) }, new List<PedPropComponent>() ) },
            new DispatchablePerson("s_m_y_hwaycop_01",0,0){ RequiredHelmetType = 1024, GroupName = "MotorcycleCop",UnitCode = "Mary", AllowRandomizeBeforeVariationApplied = true
            , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }, OptionalPropChance = 90
            , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(4, 0, 0, 0) }, new List<PedPropComponent>() ) },};
        List<DispatchablePerson> MilitaryPeds = new List<DispatchablePerson>() {
            //s_m_y_marine_03
            //new PedPropComponent(1, 0, 0) ESS Glasses

            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_03",100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0), new PedPropComponent(1, 0, 0)}) },
            new DispatchablePerson("s_m_m_pilot_02",0,0),
            new DispatchablePerson("s_m_y_pilot_01",0,0) };
        List<DispatchablePerson> PrisonPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100) };
        List<DispatchablePerson> SecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100) };
        List<DispatchablePerson> CoastGuardPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_uscg_01",100,100) };
        List<DispatchablePerson> NYSPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_snowcop_01",100,100) };
        List<DispatchablePerson> Firefighters = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_fireman_01",100,100) };
        List<DispatchablePerson> EMTs = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_paramedic_01",100,100) };

        PeopleConfig_Default.Add(new DispatchablePersonGroup("StandardCops", StandardCops));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("EMTs", EMTs));

        //Gangs
        List<DispatchablePerson> LostMCPEds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_lost_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_lost_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_lost_03",30,30,5,10,400,600,0,1),
            new DispatchablePerson("ig_clay",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_lost_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> VagosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_mexgoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgoon_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgoon_03",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_vagos_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> FamiliesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_famca_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_famdnf_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_famfor_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_families_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> BallasPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_ballasout_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_ballaeast_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_ballaorig_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_ballas_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> MarabuntaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_salvaboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_03",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> AltruistPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_acult_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_acult_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_acult_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_acult_01",10,10,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_acult_02",10,10,5,10,400,600,0,1),
            new DispatchablePerson("a_f_m_fatcult_01",10,10,5,10,400,600,0,1), };
        List<DispatchablePerson> VarriosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_azteca_01",100,100,5,10,400,600,0,1),
            new DispatchablePerson("ig_ortega",20,20,5,10,400,600,0,1), };
        List<DispatchablePerson> TriadsPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_chigoon_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_chigoon_02",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_korboss_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("ig_hao",33,33,5,10,400,600,0,1),  };
        List<DispatchablePerson> KoreanPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_korean_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_korean_02",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_korlieut_01",33,33,5,10,400,600,0,1) };
        List<DispatchablePerson> RedneckPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_02",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> ArmenianPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_armboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_armgoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_armlieut_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_armgoon_02",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> CartelPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_mexboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_mexboss_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgang_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_mexthug_01",30,30,5,10,400,600,0,1), };
        List<DispatchablePerson> MafiaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>() {  }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 1, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 2, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("u_m_m_jewelsec_01",30,30,5,10,400,600,0,1),
             new DispatchablePerson("u_m_m_aldinapoli",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(4, 0, 0, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
                                                                                                                                                                                                   };
        List<DispatchablePerson> YardiesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_og_boss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_soucent_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_soucent_02",30,30,5,10,400,600,0,1),};

        PeopleConfig_Default.Add(new DispatchablePersonGroup("LostMCPEds", LostMCPEds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        PeopleConfig_Default.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

       
        PeopleGroupLookup = PeopleConfig_Default;
    }
    private void DefaultConfig_EUPBasic()
    {
        List<DispatchablePersonGroup> PeopleConfig_EUP = new List<DispatchablePersonGroup>();


        //noose is ugly, miulitary is ugly
        //need to add the jacket and detective for RHPD and DPPD
        //Need to add the utility noose PIA peds that have armor? meh, what about lspd peds with armor for 3 stars?
        //lspd swat is cool looking so is bc and ls sheriffs, SAHP IS NOT!
        //need to do simple park rangers and saspsa as well
        //Cops
        List<DispatchablePerson> StandardCops = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",0,0),
            new DispatchablePerson("s_f_y_cop_01",0,0),

            //LSPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",0,20) { RandomizeHead = true,MinWantedLevelSpawn = 3,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",0,20) { RandomizeHead = true,MinWantedLevelSpawn = 3,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
        };
        List<DispatchablePerson> LSIAPDPeds = new List<DispatchablePerson>() {
            //LSIA Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
        };
        List<DispatchablePerson> RHPDCops = new List<DispatchablePerson>() {
            //RHPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
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
        };
        List<DispatchablePerson> DPPDCops = new List<DispatchablePerson>() {
            //DPPD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
        };
        List<DispatchablePerson> SheriffPeds = new List<DispatchablePerson>() {

            new DispatchablePerson("s_m_y_sheriff_01",0,0),
            new DispatchablePerson("s_f_y_sheriff_01",0,0),

            //LSSD Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
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
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
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
        };
        List<DispatchablePerson> BCSheriffPeds = new List<DispatchablePerson>() {
            //BCSO Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps =
                new List<PedPropComponent>() {
                    new PedPropComponent(0,13,1)
                },
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps =
                new List<PedPropComponent>() {
                    new PedPropComponent(0,13,1)
                },
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0),
                    new PedComponent(10, 53, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },

            //BCSO Class B
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0),
                    new PedComponent(10, 53, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },

            //BCSO Class C
            new DispatchablePerson("mp_m_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 22, 8, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 15, 7, 0), //Ranks
                    new PedComponent(10, 15, 8, 0),
                    new PedComponent(10, 44, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },
            new DispatchablePerson("mp_f_freemode_01",15,15) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 3, 7, 0),
                        new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 3, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 14, 7, 0), //Ranks
                    new PedComponent(10, 14, 8, 0),
                    new PedComponent(10, 52, 7, 0),

                    new PedComponent(5, 54, 1, 0),//watches
                    new PedComponent(5, 54, 2, 0),//watches

                } },
        };
        List<DispatchablePerson> NOOSEPeds = new List<DispatchablePerson>() {
            //NOOSE TRU SEP THESE ARE BOTH UGLY AS FUCK!
            new DispatchablePerson("mp_m_freemode_01",0,25) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 122, 0, 0),
                        new PedComponent(3, 141, 19, 0),
                        new PedComponent(4, 37, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 25, 5, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 9, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,39,1),new PedPropComponent(1,26,0)  }),
            },
            new DispatchablePerson("mp_f_freemode_01",0,25) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 122, 0, 0),
                        new PedComponent(3, 174,19, 0),
                        new PedComponent(4, 36, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 27, 5, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 9, 0)},
                    new List<PedPropComponent>()  {new PedPropComponent(0,38,1),new PedPropComponent(1,28,0)   }),
            },

            //NOOSE TRU PIA
            new DispatchablePerson("mp_m_freemode_01",0,25) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 122, 0, 0),
                        new PedComponent(3, 141, 19, 0),
                        new PedComponent(4, 37, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 25, 5, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 8, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,39,1),new PedPropComponent(1,26,0)  }),
            },
            new DispatchablePerson("mp_f_freemode_01",0,25) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 122, 0, 0),
                        new PedComponent(3, 174,19, 0),
                        new PedComponent(4, 36, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 27, 5, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 8, 0)},
                    new List<PedPropComponent>()  {new PedPropComponent(0,38,1),new PedPropComponent(1,28,0)   }),
            },
        };
        List<DispatchablePerson> FIBPeds = new List<DispatchablePerson>() {

            //FIB Special Agent Suit
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 38, 3, 0),
                        new PedComponent(8, 178, 0, 0),
                        new PedComponent(9, 22, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 4, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 68, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 0, 0, 0),
                        new PedComponent(8, 64, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 24, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //FIB Special Agent
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 38, 3, 0),
                        new PedComponent(8, 16, 0, 0),
                        new PedComponent(9, 22, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 349, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 0, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 27, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //FIB Windbraker
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 58, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 38, 3, 0),
                        new PedComponent(8, 178, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 307, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 39, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 318, 3, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //FIB Police Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 54, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 37, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 40, 0, 0),
                        new PedComponent(11, 200, 11, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 2, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 48, 0, 0),
                        new PedComponent(11, 202, 11, 0)},
                    new List<PedPropComponent>() {  }),
            },

            //FIB Police Class B
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 86, 12, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 54, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 37, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 39, 0, 0),
                        new PedComponent(11, 193, 11, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 89, 12, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 2, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 47, 0, 0),
                        new PedComponent(11, 195, 11, 0)},
                    new List<PedPropComponent>() {  }),
            },

            //FIB Police Class C
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 86, 12, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 54, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 37, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 39, 0, 0),
                        new PedComponent(11, 190, 11, 0)},
                    new List<PedPropComponent>() {  }),
             },
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",MaxWantedLevelSpawn = 3,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 89, 12, 0),
                        new PedComponent(5, 34, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 2, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 47, 0, 0),
                        new PedComponent(11, 192, 11, 0)},
                    new List<PedPropComponent>() {  }),
            },

            //FIB SWAT (Base)
            new DispatchablePerson("mp_m_freemode_01", 50, 50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 141, 0, 0),
                        new PedComponent(4, 31, 4, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 35, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 25, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 5, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,39,0),new PedPropComponent(1,23,0)  }),
            },
            new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 174,0, 0),
                        new PedComponent(4, 30, 4, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 36, 1, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 27, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 5, 0)},
                    new List<PedPropComponent>()  {new PedPropComponent(0,38,0),new PedPropComponent(1,28,0)   }),
            },

            ////FIB SWAT HRT
            //new DispatchablePerson("mp_m_freemode_01", 50, 50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_01",
            //    RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {
            //            new PedComponent(1, 0, 0, 0),
            //            new PedComponent(3, 141, 19, 0),
            //            new PedComponent(4, 37, 2, 0),
            //            new PedComponent(5, 48, 0, 0),
            //            new PedComponent(6, 35, 0, 0),
            //            new PedComponent(7, 110, 0, 0),
            //            new PedComponent(8, 15, 0, 0),
            //            new PedComponent(9, 27, 8, 0),
            //            new PedComponent(10, 0, 0, 0),
            //            new PedComponent(11, 220, 6, 0)},
            //        new List<PedPropComponent>() { new PedPropComponent(0,39,1),new PedPropComponent(1,23,0)  }),
            //},
            //new DispatchablePerson("mp_f_freemode_01", 50,50, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
            //    RequiredVariation = new PedVariation(
            //        new List<PedComponent>() {
            //            new PedComponent(1, 0, 0, 0),
            //            new PedComponent(3, 174,19, 0),
            //            new PedComponent(4, 36, 2, 0),
            //            new PedComponent(5, 48, 0, 0),
            //            new PedComponent(6, 36, 1, 0),
            //            new PedComponent(7, 81, 0, 0),
            //            new PedComponent(8, 15, 0, 0),
            //            new PedComponent(9, 27, 8, 0),
            //            new PedComponent(10, 0, 0, 0),
            //            new PedComponent(11, 230, 6, 0)},
            //        new List<PedPropComponent>()  {new PedPropComponent(0,38,1),new PedPropComponent(1,25,0)   }),
            //},

            };
        List<DispatchablePerson> ParkRangers = new List<DispatchablePerson>() {
            //Park Rangers
            //s_m_y_ranger_01
                //new PedPropComponent(0, 0, 0) Flat Hat
            //new PedPropComponent(0, 1, 0) Baseball Hat
            //new PedPropComponent(1, 0, 0) Aviators

            //s_f_y_ranger_01
            //new PedPropComponent(1, 0, 0) Aviators

            new DispatchablePerson("s_m_y_ranger_01",75,75) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}, OptionalPropChance = 70},
            new DispatchablePerson("s_f_y_ranger_01",25,25) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }, OptionalPropChance = 70 }

        };
        List<DispatchablePerson> DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) };
        List<DispatchablePerson> SAHPPeds = new List<DispatchablePerson>() {

            //SAHP Class A
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01", GroupName = "MotorcycleCop",UnitCode = "Mary",
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
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_m_freemode_01",0,0) { RandomizeHead = true,MinWantedLevelSpawn = 99,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",GroupName = "StandardSAHP",
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
            new DispatchablePerson("mp_f_freemode_01",0,0) { RandomizeHead = true,MinWantedLevelSpawn = 99,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",GroupName = "StandardSAHP",
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
        List<DispatchablePerson> MilitaryPeds = new List<DispatchablePerson>() {
            //Vanilla Peds
            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_m_pilot_02",0,0),
            new DispatchablePerson("s_m_y_pilot_01",0,0),
        
            //EUP
            new DispatchablePerson("mp_m_freemode_01", 0, 70, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { MinWantedLevelSpawn = 6, RandomizeHead = true,OverrideVoice = "S_M_Y_SWAT_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 141, 19, 0),
                        new PedComponent(4, 37, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 35, 0, 0),
                        new PedComponent(7, 42, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 15, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 13, 0)},//13?//6?//listed as 25, but that texture is BROKE
                    new List<PedPropComponent>() { new PedPropComponent(0,39,1),new PedPropComponent(1,23,0)  }),
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { MinWantedLevelSpawn = 6, RandomizeHead = true,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 174,19, 0),
                        new PedComponent(4, 36, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 36, 0, 0),
                        new PedComponent(7, 29, 0, 0),
                        new PedComponent(8, 14, 0, 0),
                        new PedComponent(9, 17, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 25, 0)},//listed as 25, but that texture is BROKE
                    new List<PedPropComponent>()  {new PedPropComponent(0,38,1),new PedPropComponent(1,25,0)   }),
            },

        };
        List<DispatchablePerson> PrisonPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100) };
        List<DispatchablePerson> SecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100) };
        List<DispatchablePerson> CoastGuardPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_uscg_01",100,100) };
        List<DispatchablePerson> NYSPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_snowcop_01",100,100) };
        List<DispatchablePerson> Firefighters = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_fireman_01",100,100) };
        List<DispatchablePerson> EMTs = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_paramedic_01",100,100) };

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("StandardCops", StandardCops));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSIAPDPeds", LSIAPDPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RHPDCops", RHPDCops));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DPPDCops", DPPDCops));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCSheriffPeds", BCSheriffPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("EMTs", EMTs));

        //Gangs
        List<DispatchablePerson> LostMCPEds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_lost_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_lost_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_lost_03",30,30,5,10,400,600,0,1),
            new DispatchablePerson("ig_clay",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_lost_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> VagosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_mexgoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgoon_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgoon_03",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_vagos_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> FamiliesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_famca_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_famdnf_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_famfor_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_families_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> BallasPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_ballasout_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_ballaeast_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_ballaorig_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_f_y_ballas_01",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> MarabuntaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_salvaboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_salvagoon_03",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> AltruistPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_acult_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_acult_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_acult_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_acult_01",10,10,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_acult_02",10,10,5,10,400,600,0,1),
            new DispatchablePerson("a_f_m_fatcult_01",10,10,5,10,400,600,0,1), };
        List<DispatchablePerson> VarriosPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_azteca_01",100,100,5,10,400,600,0,1),
            new DispatchablePerson("ig_ortega",20,20,5,10,400,600,0,1), };
        List<DispatchablePerson> TriadsPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_chigoon_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_chigoon_02",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_korboss_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("ig_hao",33,33,5,10,400,600,0,1),  };
        List<DispatchablePerson> KoreanPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_y_korean_01",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_korean_02",33,33,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_korlieut_01",33,33,5,10,400,600,0,1) };
        List<DispatchablePerson> RedneckPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_m_hillbilly_02",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> ArmenianPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_armboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_armgoon_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_armlieut_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_armgoon_02",10,10,5,10,400,600,0,1) };
        List<DispatchablePerson> CartelPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("g_m_m_mexboss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_m_mexboss_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("g_m_y_mexgang_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_mexthug_01",30,30,5,10,400,600,0,1), };
        List<DispatchablePerson> MafiaPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>() {  }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 1, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_01",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 2, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
            new DispatchablePerson("s_m_m_highsec_02",30,30,5,10,400,600,0,1),
            new DispatchablePerson("u_m_m_jewelsec_01",30,30,5,10,400,600,0,1),
             new DispatchablePerson("u_m_m_aldinapoli",30,30,5,10,400,600,0,1) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(4, 0, 0, 0) },new List<PedPropComponent>() { }) },//not good, bad heads
                                                                                                                                                                                                   };
        List<DispatchablePerson> YardiesPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("a_m_m_og_boss_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_o_soucent_01",30,30,5,10,400,600,0,1),
            new DispatchablePerson("a_m_y_soucent_02",30,30,5,10,400,600,0,1),};

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LostMCPEds", LostMCPEds));
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

        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\DispatchablePeople_Gresk.xml");

    }

}

