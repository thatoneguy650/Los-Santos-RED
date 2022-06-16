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
            DefaultConfig_Freemode();
            Serialization.SerializeParams(PeopleGroupLookup, ConfigFileName);
        }
    }
    public List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID)
    {
        return PeopleGroupLookup.FirstOrDefault(x => x.DispatchablePersonGroupID == dispatchablePersonGroupID)?.DispatchablePeople;
    }
    private void DefaultConfig_Freemode()
    {
        List<DispatchablePersonGroup> PeopleConfig_FullExtended = new List<DispatchablePersonGroup>();

        //Cops
        List<DispatchablePerson> StandardCops = new List<DispatchablePerson>() {
        //male cop
                new DispatchablePerson("s_m_y_cop_01",0,0),
               new DispatchablePerson("s_f_y_cop_01",0,0),


               //male rank decals 1-3
                //,new PedComponent(10, 8, 1, 0)
                //,new PedComponent(10, 8, 2, 0)
                //,new PedComponent(10, 8, 3, 0)

            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 1, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() {  }) },//no body armor 
            new DispatchablePerson("mp_m_freemode_01",2,2) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_COP_01_BLACK_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 3, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,46,0) }) },//no body armor with goofy hat
            new DispatchablePerson("mp_m_freemode_01",20,20) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_M_Y_HWAYCOP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(1,23,9) }) },//no body armor with glasses


             new DispatchablePerson("mp_m_freemode_01",0,50) { RandomizeHead = true,MinWantedLevelSpawn = 2,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_HWAYCOP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0),new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>()) },//body armor
             new DispatchablePerson("mp_m_freemode_01",0,45) { RandomizeHead = true,MinWantedLevelSpawn = 2,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0),new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 3, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(1,23,9) }) },//body armor with glasses
             new DispatchablePerson("mp_m_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {new PedComponent(4, 35, 0, 0),new PedComponent(6, 25, 0, 0),new PedComponent(8, 58, 0, 0),new PedComponent(9, 11, 1, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,124,15) }) },//body armor with riot hat

        //female cop
        
               //male rank decals 1-3
                //,new PedComponent(10, 7, 1, 0)
                //,new PedComponent(10, 7, 2, 0)
                //,new PedComponent(10, 7, 3, 0)

            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  })},//no body armor
            new DispatchablePerson("mp_f_freemode_01",2,2) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 2, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,45,0) })},//no body armor with goofy hat
            new DispatchablePerson("mp_f_freemode_01",20,20) { RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(1,25,9) })},//no body armor with glasses

            new DispatchablePerson("mp_f_freemode_01",0,70) { RandomizeHead = true,MinWantedLevelSpawn = 2,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 3, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() {  })},//body armor
            new DispatchablePerson("mp_f_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 2, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,123,15) })},//body armor with riot hat
            new DispatchablePerson("mp_f_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 14, 0, 0) ,new PedComponent(4, 34, 0, 0) ,new PedComponent(6, 55, 0, 0) ,new PedComponent(8, 35, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(10, 7, 3, 0), new PedComponent(11, 48, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,123,15),new PedPropComponent(1,25,9) })},//body armor with riot hat and glasses
        };
        List<DispatchablePerson> SheriffPeds = new List<DispatchablePerson>() {

            new DispatchablePerson("s_m_y_sheriff_01",0,0),
            new DispatchablePerson("s_f_y_sheriff_01",0,0),

            new DispatchablePerson("mp_m_freemode_01",75,75) { RandomizeHead = true,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_SHERIFF_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 38, 0, 0), new PedComponent(3, 11, 0, 0), new PedComponent(4, 47, 1, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 58, 0, 0), new PedComponent(9, 7, 2, 0), new PedComponent(11, 319, 3, 0), },
                    new List<PedPropComponent>() {   }) },//body armor
            new DispatchablePerson("mp_m_freemode_01",75,75) { RandomizeHead = true,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_SHERIFF_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 38, 0, 0), new PedComponent(3, 11, 0, 0), new PedComponent(4, 47, 1, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 58, 0, 0), new PedComponent(9, 7, 2, 0), new PedComponent(11, 319, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0,58,0)  }) },//body armor with hat
            new DispatchablePerson("mp_m_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_M_Y_HWAYCOP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 38, 0, 0), new PedComponent(3, 11, 0, 0), new PedComponent(4, 47, 1, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 58, 0, 0), new PedComponent(9, 7, 2, 0), new PedComponent(11, 319, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 124, 3)  }) },//body armor with helmet

            new DispatchablePerson("mp_f_freemode_01",75,75) { RandomizeHead = true,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 42, 0, 0),new PedComponent(3, 14, 0, 0),new PedComponent(4, 49, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(7, 2, 0, 0),new PedComponent(8, 35, 0, 0),new PedComponent(9, 6, 2, 0),new PedComponent(11, 330, 3, 0), },
                    new List<PedPropComponent>() {  }) },//body armor
            new DispatchablePerson("mp_f_freemode_01",75,75) { RandomizeHead = true,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_BLACK_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 42, 0, 0),new PedComponent(3, 14, 0, 0),new PedComponent(4, 49, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(7, 2, 0, 0),new PedComponent(8, 35, 0, 0),new PedComponent(9, 6, 2, 0),new PedComponent(11, 330, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 58, 0), }) },//body armor with hat
             new DispatchablePerson("mp_f_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_01",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 42, 0, 0),new PedComponent(3, 14, 0, 0),new PedComponent(4, 49, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(7, 2, 0, 0),new PedComponent(8, 35, 0, 0),new PedComponent(9, 6, 2, 0),new PedComponent(11, 330, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 123, 3), }) },//body armor with helmet    
            new DispatchablePerson("mp_f_freemode_01",0,5) { RandomizeHead = true,MinWantedLevelSpawn = 3,ArmorMin = 50,ArmorMax = 50,OverrideVoice = "S_F_Y_COP_01_WHITE_FULL_02",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 42, 0, 0),new PedComponent(3, 14, 0, 0),new PedComponent(4, 49, 1, 0),new PedComponent(6, 55, 0, 0),new PedComponent(7, 2, 0, 0),new PedComponent(8, 35, 0, 0),new PedComponent(9, 6, 2, 0),new PedComponent(11, 330, 3, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 123, 3),new PedPropComponent(1,25,9) }) },//body armor with helmet        and glasees    
        };
        List<DispatchablePerson> NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 0,0),
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
            //new DispatchablePerson("s_m_y_swat_01", 5,30) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) }
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
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){ AllowRandomizeBeforeVariationApplied = true
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

        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("StandardCops", StandardCops));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("EMTs", EMTs));

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

        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("LostMCPEds", LostMCPEds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        PeopleConfig_FullExtended.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

        Serialization.SerializeParams(PeopleConfig_FullExtended, "Plugins\\LosSantosRED\\AlternateConfigs\\DispatchablePeople_FullExtended.xml");

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
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){ AllowRandomizeBeforeVariationApplied = true
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

}

