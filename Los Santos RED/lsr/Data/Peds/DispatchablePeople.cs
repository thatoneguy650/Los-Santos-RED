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
            DefaultConfig_Simple();
            DefaultConfig();
        }
    }
    public List<DispatchablePerson> GetPersonData(string dispatchablePersonGroupID)
    {
        return PeopleGroupLookup.FirstOrDefault(x => x.DispatchablePersonGroupID == dispatchablePersonGroupID)?.DispatchablePeople;
    }
    private void DefaultConfig()
    {
        PeopleGroupLookup = new List<DispatchablePersonGroup>();

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
            new DispatchablePerson("s_m_y_ranger_01",75,75),
            new DispatchablePerson("s_f_y_ranger_01",25,25) };
        List<DispatchablePerson> DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) };
        List<DispatchablePerson> SAHPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){ AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(4, 1, 0, 0) },
                new List<PedPropComponent>() ) },
            new DispatchablePerson("s_m_y_hwaycop_01",0,0){ RequiredHelmetType = 1024, GroupName = "MotorcycleCop",UnitCode = "Mary", AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(4, 0, 0, 0) },
                new List<PedPropComponent>() ) },};
        List<DispatchablePerson> MilitaryPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_03",100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
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
        PeopleGroupLookup.Add(new DispatchablePersonGroup("EMTs", EMTs));

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

        PeopleGroupLookup.Add(new DispatchablePersonGroup("LostMCPEds", LostMCPEds));
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
    private void DefaultConfig_Simple()
    {
        List<DispatchablePersonGroup> SimplePeopleGroupLookup = new List<DispatchablePersonGroup>();

        //Cops
        List<DispatchablePerson> StandardCops = new List<DispatchablePerson>() {

            new DispatchablePerson("s_f_y_cop_01",40,40) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } },//new PedPropComponent(0, 0, 0)//Hat,//new PedPropComponent(1, 0, 0)//Glasses
            new DispatchablePerson("s_m_y_cop_01",60,60) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) }) },//filled duty belt
            new DispatchablePerson("s_m_y_cop_01",0,100) { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) }//Glasses only?
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 2, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 1, 1) }) },//vest, no hat
        };
        List<DispatchablePerson> SheriffPeds = new List<DispatchablePerson>() {
            
            new DispatchablePerson("s_f_y_sheriff_01",35,35) { MaxWantedLevelSpawn = 2
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } },
            new DispatchablePerson("s_m_y_sheriff_01",65,65) { MaxWantedLevelSpawn = 2, AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 1, 0, 0) })
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } },//filled duty belt
            new DispatchablePerson("s_m_y_sheriff_01",0,100) { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3
                , AllowRandomizeBeforeVariationApplied = true
                , RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(9, 2, 0, 0) })
                , OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0), new PedPropComponent(1, 0, 0), new PedPropComponent(1, 1, 0) } },//filled duty belt},//vest
        };
        List<DispatchablePerson> NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 100,100){ RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) }
        };
        List<DispatchablePerson> FIBPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 5,30) { GroupName = "FIBHRT", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1, 0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) }
            };
        List<DispatchablePerson> ParkRangers = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_ranger_01",75,75),
            new DispatchablePerson("s_f_y_ranger_01",25,25) };
        List<DispatchablePerson> DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) };
        List<DispatchablePerson> SAHPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_hwaycop_01",100,100){ AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(4, 1, 0, 0) },
                new List<PedPropComponent>() ) },
            new DispatchablePerson("s_m_y_hwaycop_01",0,0){ RequiredHelmetType = 1024, GroupName = "MotorcycleCop",UnitCode = "Mary", AllowRandomizeBeforeVariationApplied = true, RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(4, 0, 0, 0) },
                new List<PedPropComponent>() ) },};
        List<DispatchablePerson> MilitaryPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_03",100,100, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
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

        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("StandardCops", StandardCops));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("NOOSEPeds", NOOSEPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("ParkRangers", ParkRangers));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("MilitaryPeds", MilitaryPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("SecurityPeds", SecurityPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuardPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("NYSPPeds", NYSPPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("Firefighters", Firefighters));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("EMTs", EMTs));

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

        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("LostMCPEds", LostMCPEds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("VagosPeds", VagosPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("FamiliesPeds", FamiliesPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("BallasPeds", BallasPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("MarabuntaPeds", MarabuntaPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("AltruistPeds", AltruistPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("VarriosPeds", VarriosPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("TriadsPeds", TriadsPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("KoreanPeds", KoreanPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("RedneckPeds", RedneckPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("ArmenianPeds", ArmenianPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("CartelPeds", CartelPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("MafiaPeds", MafiaPeds));
        SimplePeopleGroupLookup.Add(new DispatchablePersonGroup("YardiesPeds", YardiesPeds));

        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
        Serialization.SerializeParams(SimplePeopleGroupLookup, "Plugins\\LosSantosRED\\AlternateConfigs\\DispatchablePeople_Simple.xml");

    }

}

