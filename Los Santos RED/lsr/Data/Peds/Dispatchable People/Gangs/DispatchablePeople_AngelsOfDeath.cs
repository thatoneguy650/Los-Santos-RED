using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_AngelsOfDeath
{
    private DispatchablePeople DispatchablePeople;
    private int optionalComponentDefault = 80;
    private int defaultAmbient = 15;
    private int defaultWanted = 15;

    private int defaultFemaleAmbient = 5;
    private int defaultFemaleWanted = 5;

    private int defaultAccuracyMin = 5;
    private int defaultAccuracyMax = 10;

    private int defaultShootRateMin = 400;
    private int defaultShootRateMax = 600;

    private int defaultCombatAbilityMin = 0;
    private int defaultCombatAbilityMax = 1;

    private List<string> DefaultVoicesMale = new List<string>() {
        "G_M_Y_LOST_01_BLACK_FULL_01",
        "G_M_Y_LOST_01_BLACK_FULL_02",
        "G_M_Y_LOST_01_BLACK_MINI_01",
        "G_M_Y_LOST_01_BLACK_MINI_02",
        "G_M_Y_LOST_01_BLACK_MINI_03",
        "G_M_Y_LOST_01_WHITE_FULL_01",
        "G_M_Y_LOST_01_WHITE_MINI_01",
        "G_M_Y_LOST_01_WHITE_MINI_02",
        "G_M_Y_LOST_02_LATINO_FULL_01",
        "G_M_Y_LOST_02_LATINO_FULL_02",
        "G_M_Y_LOST_02_LATINO_MINI_01",
        "G_M_Y_LOST_02_LATINO_MINI_02",
        "G_M_Y_LOST_02_LATINO_MINI_03",
        "G_M_Y_LOST_02_WHITE_FULL_01",
        "G_M_Y_LOST_02_WHITE_MINI_01",
        "G_M_Y_LOST_02_WHITE_MINI_02",
        "G_M_Y_LOST_03_WHITE_FULL_01",
        "G_M_Y_LOST_03_WHITE_MINI_02",
        "G_M_Y_LOST_03_WHITE_MINI_03",
       // "A_M_Y_BUSINESS_01_WHITE_FULL_01", "A_M_Y_BUSINESS_02_WHITE_FULL_01", "A_M_M_SKATER_01_WHITE_FULL_01"
    };
    private List<string> DefaultVoicesFemale = new List<string>() { "A_F_Y_TOURIST_01_WHITE_FULL_01", "A_F_Y_TOURIST_01_WHITE_MINI_01", "A_F_Y_TOURIST_02_WHITE_MINI_01", "A_F_Y_VINEWOOD_01_WHITE_MINI_01",
        "A_F_Y_TOURIST_01_BLACK_FULL_01", "A_F_Y_TOURIST_01_BLACK_MINI_01","A_F_Y_TOURIST_01_LATINO_FULL_01","A_F_Y_TOURIST_01_LATINO_MINI_01" };
    public DispatchablePeople_AngelsOfDeath(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.AngelsOfDeathPeds = new List<DispatchablePerson>() { };
        DefaultPeds();
        MaleFreemodePeds();
        FemaleFreemodePeds();
    }



    private void DefaultPeds()
    {
        DispatchablePeople.AngelsOfDeathPeds.AddRange(new List<DispatchablePerson>()
        {
            //new DispatchablePerson("g_m_y_lost_01",10,10,defaultAccuracyMin,defaultAccuracyMax,defaultShootRateMin,defaultShootRateMax,defaultCombatAbilityMin,defaultCombatAbilityMax) { DebugName = "LOSTMaleVanilla1" },
            //new DispatchablePerson("g_m_y_lost_02",10,10,defaultAccuracyMin,defaultAccuracyMax,defaultShootRateMin,defaultShootRateMax,defaultCombatAbilityMin,defaultCombatAbilityMax) { DebugName = "LOSTMaleVanilla2" },
            //new DispatchablePerson("g_m_y_lost_03",10,10,defaultAccuracyMin,defaultAccuracyMax,defaultShootRateMin,defaultShootRateMax,defaultCombatAbilityMin,defaultCombatAbilityMax) { DebugName = "LOSTMaleVanilla3" },
            //new DispatchablePerson("g_f_y_lost_01",2,2,defaultAccuracyMin,defaultAccuracyMax,defaultShootRateMin,defaultShootRateMax,defaultCombatAbilityMin,defaultCombatAbilityMax) { DebugName = "LOSTFemaleVanilla1" },
        });
    }
    private void MaleFreemodePeds()
    {
        //Same Pants and Boots Below
        PedPropComponent DefaultMaleHelmet = new PedPropComponent(1, 85, 0);
        float NoHelmetPercentage = 70f;
        DispatchablePerson GeneralMaleBiker1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient + 5, defaultWanted + 5, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeMaleGen1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 20, 0, 0),
                new PedComponent(3, 0, 0, 0),
                new PedComponent(4, 74, 0, 0),
                new PedComponent(6, 50, 0, 0),
                new PedComponent(8, 81, 0, 0),
                new PedComponent(11, 366, 0, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),

            OptionalAppliedOverlayLogic = BothArmTattoos(true, true),

            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 366, 1, 0),//366 = OPEN

                //Undershirt
                new PedComponent(8, 81, 1, 0),//SHirt GOES UP
                new PedComponent(8, 81, 2, 0),

                //Boots
                new PedComponent(6, 50, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 53, 0, 0),
                new PedComponent(6, 53, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 27, 0, 0),

                //Pants
                new PedComponent(4, 74, 1, 0),//Padded
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 72, 0, 0),//Regular
                new PedComponent(4, 72, 1, 0),
                new PedComponent(4, 72, 2, 0),
                new PedComponent(4, 72, 3, 0),
                new PedComponent(4, 72, 4, 0),
                new PedComponent(4, 72, 5, 0),

                new PedComponent(4, 82, 0, 0),
                new PedComponent(4, 82, 1, 0),
                new PedComponent(4, 82, 2, 0),
                new PedComponent(4, 82, 3, 0),
                new PedComponent(4, 82, 4, 0),
                new PedComponent(4, 82, 5, 0),
                new PedComponent(4, 82, 6, 0),
                new PedComponent(4, 82, 7, 0),
                new PedComponent(4, 82, 8, 0),
                new PedComponent(4, 82, 9, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralMaleBiker1);

        DispatchablePerson GeneralMaleBiker2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeMaleGen2",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 20, 0, 0),
                new PedComponent(3, 5, 0, 0),
                new PedComponent(4, 74, 0, 0),
                new PedComponent(6, 50, 0, 0),
                new PedComponent(8, 5, 0, 0),
                new PedComponent(11, 365, 0, 0),//365 = CLOSED
            }, new List<PedPropComponent>() { }),
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalAppliedOverlayLogic = FullTattos(false, true),
            OptionalComponents = new List<PedComponent>()
            { 
                //Top
                new PedComponent(11, 365, 1, 0),//365 = CLOSED CUT
                 
                //Undershirt
                new PedComponent(8, 5, 1, 0),
                new PedComponent(8, 5, 2, 0),

                //Boots
                new PedComponent(6, 50, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 53, 0, 0),
                new PedComponent(6, 53, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 27, 0, 0),

                //Pants
                new PedComponent(4, 74, 1, 0),//Padded
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 72, 0, 0),//Regular
                new PedComponent(4, 72, 1, 0),
                new PedComponent(4, 72, 2, 0),
                new PedComponent(4, 72, 3, 0),
                new PedComponent(4, 72, 4, 0),
                new PedComponent(4, 72, 5, 0),

                new PedComponent(4, 82, 0, 0),
                new PedComponent(4, 82, 1, 0),
                new PedComponent(4, 82, 2, 0),
                new PedComponent(4, 82, 3, 0),
                new PedComponent(4, 82, 4, 0),
                new PedComponent(4, 82, 5, 0),
                new PedComponent(4, 82, 6, 0),
                new PedComponent(4, 82, 7, 0),
                new PedComponent(4, 82, 8, 0),
                new PedComponent(4, 82, 9, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralMaleBiker2);

        DispatchablePerson GeneralMaleBiker3 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeMaleGen3",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 20, 0, 0),
                new PedComponent(3, 5, 0, 0),
                new PedComponent(4, 74, 0, 0),
                new PedComponent(6, 50, 0, 0),
                new PedComponent(8, 2, 2, 0),
                new PedComponent(11, 367, 0, 0),//Ugly weird vest thing
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = FullTattos(false, true),
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 367, 1, 0),
                new PedComponent(11, 367, 2, 0),
                new PedComponent(11, 367, 3, 0),
                new PedComponent(11, 367, 4, 0),
                new PedComponent(11, 367, 5, 0),
                new PedComponent(11, 367, 6, 0),
                new PedComponent(11, 367, 7, 0),

                //Undershirt
                new PedComponent(8, 2, 1, 0),
                new PedComponent(8, 2, 4, 0),

                //Boots
                new PedComponent(6, 50, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 53, 0, 0),
                new PedComponent(6, 53, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 27, 0, 0),

                //Pants
                new PedComponent(4, 74, 1, 0),//Padded
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 72, 0, 0),//Regular
                new PedComponent(4, 72, 1, 0),
                new PedComponent(4, 72, 2, 0),
                new PedComponent(4, 72, 3, 0),
                new PedComponent(4, 72, 4, 0),
                new PedComponent(4, 72, 5, 0),

                new PedComponent(4, 82, 0, 0),
                new PedComponent(4, 82, 1, 0),
                new PedComponent(4, 82, 2, 0),
                new PedComponent(4, 82, 3, 0),
                new PedComponent(4, 82, 4, 0),
                new PedComponent(4, 82, 5, 0),
                new PedComponent(4, 82, 6, 0),
                new PedComponent(4, 82, 7, 0),
                new PedComponent(4, 82, 8, 0),
                new PedComponent(4, 82, 9, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralMaleBiker3);

        DispatchablePerson GeneralMaleBiker4 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeMaleGen4",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 20, 0, 0),
                new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 74, 0, 0),
                new PedComponent(6, 50, 0, 0),
                new PedComponent(8, 2, 2, 0),
                new PedComponent(11, 368, 0, 0),//Ugly weird vest thing
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 368, 1, 0),
                new PedComponent(11, 368, 2, 0),
                new PedComponent(11, 368, 3, 0),
                new PedComponent(11, 368, 4, 0),
                new PedComponent(11, 368, 5, 0),
                new PedComponent(11, 368, 6, 0),
                new PedComponent(11, 368, 7, 0),

                //Undershirt
                new PedComponent(8, 2, 1, 0),
                new PedComponent(8, 2, 4, 0),

                //Boots
                new PedComponent(6, 50, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 53, 0, 0),
                new PedComponent(6, 53, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 27, 0, 0),

                //Pants
                new PedComponent(4, 74, 1, 0),//Padded
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 72, 0, 0),//Regular
                new PedComponent(4, 72, 1, 0),
                new PedComponent(4, 72, 2, 0),
                new PedComponent(4, 72, 3, 0),
                new PedComponent(4, 72, 4, 0),
                new PedComponent(4, 72, 5, 0),

                new PedComponent(4, 82, 0, 0),
                new PedComponent(4, 82, 1, 0),
                new PedComponent(4, 82, 2, 0),
                new PedComponent(4, 82, 3, 0),
                new PedComponent(4, 82, 4, 0),
                new PedComponent(4, 82, 5, 0),
                new PedComponent(4, 82, 6, 0),
                new PedComponent(4, 82, 7, 0),
                new PedComponent(4, 82, 8, 0),
                new PedComponent(4, 82, 9, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralMaleBiker4);

        DispatchablePerson GeneralMaleBiker5 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient + 5, defaultWanted + 5, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeMaleGen5",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 0, 0, 0),
                new PedComponent(4, 74, 0, 0),
                new PedComponent(6, 50, 0, 0),
                new PedComponent(8, 15, 0, 0),
                new PedComponent(11, 0, 0, 0),//t shirt
            }, new List<PedPropComponent>() { }),

            OptionalAppliedOverlayLogic = BothArmTattoos(true, true),

            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 0, 1, 0),
                new PedComponent(11, 0, 2, 0),

                //Boots
                new PedComponent(6, 50, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 53, 0, 0),
                new PedComponent(6, 53, 1, 0),
                new PedComponent(6, 50, 2, 0),
                new PedComponent(6, 50, 3, 0),
                new PedComponent(6, 50, 4, 0),
                new PedComponent(6, 50, 5, 0),

                new PedComponent(6, 27, 0, 0),

                //Pants
                new PedComponent(4, 74, 1, 0),//Padded
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 72, 0, 0),//Regular
                new PedComponent(4, 72, 1, 0),
                new PedComponent(4, 72, 2, 0),
                new PedComponent(4, 72, 3, 0),
                new PedComponent(4, 72, 4, 0),
                new PedComponent(4, 72, 5, 0),

                new PedComponent(4, 82, 0, 0),
                new PedComponent(4, 82, 1, 0),
                new PedComponent(4, 82, 2, 0),
                new PedComponent(4, 82, 3, 0),
                new PedComponent(4, 82, 4, 0),
                new PedComponent(4, 82, 5, 0),
                new PedComponent(4, 82, 6, 0),
                new PedComponent(4, 82, 7, 0),
                new PedComponent(4, 82, 8, 0),
                new PedComponent(4, 82, 9, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.AngelsOfDeathPeds.Add(GeneralMaleBiker5);
    }

    private void FemaleFreemodePeds()
    {
        PedPropComponent DefaultFemaleHelmet = new PedPropComponent(1, 84, 0);
        float NoHelmetPercentage = 80f;

        //384 = closed, 385 = open
        DispatchablePerson GeneralFemaleBiker1 = new DispatchablePerson("mp_f_freemode_01", defaultFemaleAmbient + 5, defaultFemaleWanted + 5, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeFemaleGen1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 0, 0, 0),//Torso
                new PedComponent(4, 77, 0, 0),//Pants
                new PedComponent(6, 51, 0, 0),//Shoes
                new PedComponent(8, 86, 22, 0),//Undershirt
                new PedComponent(11, 385, 0, 0),//Top
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = BothArmTattoos(true, false),
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 385, 1, 0),

                //Undershirt
                new PedComponent(8, 86, 23, 0),
                new PedComponent(8, 86, 24, 0),

                //Boots
                new PedComponent(6, 51, 1, 0),
                new PedComponent(6, 51, 2, 0),
                new PedComponent(6, 51, 3, 0),
                new PedComponent(6, 51, 4, 0),
                new PedComponent(6, 51, 5, 0),

                //Pants
                new PedComponent(4, 77, 1, 0),

                new PedComponent(4, 73, 0, 0),
                new PedComponent(4, 73, 1, 0),
                new PedComponent(4, 73, 2, 0),
                new PedComponent(4, 73, 3, 0),
                new PedComponent(4, 73, 4, 0),
                new PedComponent(4, 73, 5, 0),

                new PedComponent(4, 74, 0, 0),
                new PedComponent(4, 74, 1, 0),
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 75, 0, 0),
                new PedComponent(4, 75, 1, 0),
                new PedComponent(4, 75, 2, 0),

                new PedComponent(4, 76, 0, 0),
                new PedComponent(4, 76, 1, 0),
                new PedComponent(4, 76, 2, 0),

                new PedComponent(4, 77, 1, 0),
                new PedComponent(4, 77, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralFemaleBiker1);

        DispatchablePerson GeneralFemaleBiker2 = new DispatchablePerson("mp_f_freemode_01", defaultFemaleAmbient, defaultFemaleWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeFemaleGen2",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 0, 0, 0),//Torso
                new PedComponent(4, 77, 0, 0),//Pants
                new PedComponent(6, 51, 0, 0),//Shoes
                new PedComponent(8, 86, 22, 0),//Undershirt
                new PedComponent(11, 384, 0, 0),//Top
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = FullTattos(false, false),
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 384, 1, 0),
                new PedComponent(11, 386, 0, 0),
                new PedComponent(11, 386, 1, 0),
                new PedComponent(11, 386, 2, 0),
                new PedComponent(11, 386, 3, 0),
                new PedComponent(11, 386, 4, 0),
                new PedComponent(11, 386, 5, 0),
                new PedComponent(11, 386, 6, 0),
                new PedComponent(11, 386, 7, 0),

                //Undershirt
                new PedComponent(8, 86, 23, 0),
                new PedComponent(8, 86, 24, 0),

                //Boots
                new PedComponent(6, 51, 1, 0),
                new PedComponent(6, 51, 2, 0),
                new PedComponent(6, 51, 3, 0),
                new PedComponent(6, 51, 4, 0),
                new PedComponent(6, 51, 5, 0),

                //Pants
                new PedComponent(4, 77, 1, 0),

                new PedComponent(4, 73, 0, 0),
                new PedComponent(4, 73, 1, 0),
                new PedComponent(4, 73, 2, 0),
                new PedComponent(4, 73, 3, 0),
                new PedComponent(4, 73, 4, 0),
                new PedComponent(4, 73, 5, 0),

                new PedComponent(4, 74, 0, 0),
                new PedComponent(4, 74, 1, 0),
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 75, 0, 0),
                new PedComponent(4, 75, 1, 0),
                new PedComponent(4, 75, 2, 0),

                new PedComponent(4, 76, 0, 0),
                new PedComponent(4, 76, 1, 0),
                new PedComponent(4, 76, 2, 0),

                new PedComponent(4, 77, 1, 0),
                new PedComponent(4, 77, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralFemaleBiker2);

        DispatchablePerson GeneralFemaleBiker3 = new DispatchablePerson("mp_f_freemode_01", defaultFemaleAmbient, defaultFemaleWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeFemaleGen3",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 1, 0, 0),//Torso
                new PedComponent(4, 77, 0, 0),//Pants
                new PedComponent(6, 51, 0, 0),//Shoes
                new PedComponent(8, 55, 0, 0),//Undershirt
                new PedComponent(11, 387, 0, 0),//Top
            }, new List<PedPropComponent>() { }),
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 387, 1, 0),
                new PedComponent(11, 387, 2, 0),
                new PedComponent(11, 387, 3, 0),
                new PedComponent(11, 387, 4, 0),
                new PedComponent(11, 387, 5, 0),
                new PedComponent(11, 387, 6, 0),
                new PedComponent(11, 387, 7, 0),

                //Undershirt
                new PedComponent(8, 55, 1, 0),

                //Boots
                new PedComponent(6, 51, 1, 0),
                new PedComponent(6, 51, 2, 0),
                new PedComponent(6, 51, 3, 0),
                new PedComponent(6, 51, 4, 0),
                new PedComponent(6, 51, 5, 0),

                //Pants
                new PedComponent(4, 77, 1, 0),

                new PedComponent(4, 73, 0, 0),
                new PedComponent(4, 73, 1, 0),
                new PedComponent(4, 73, 2, 0),
                new PedComponent(4, 73, 3, 0),
                new PedComponent(4, 73, 4, 0),
                new PedComponent(4, 73, 5, 0),

                new PedComponent(4, 74, 0, 0),
                new PedComponent(4, 74, 1, 0),
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 75, 0, 0),
                new PedComponent(4, 75, 1, 0),
                new PedComponent(4, 75, 2, 0),

                new PedComponent(4, 76, 0, 0),
                new PedComponent(4, 76, 1, 0),
                new PedComponent(4, 76, 2, 0),

                new PedComponent(4, 77, 1, 0),
                new PedComponent(4, 77, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralFemaleBiker3);

        DispatchablePerson GeneralFemaleBiker4 = new DispatchablePerson("mp_f_freemode_01", defaultFemaleAmbient, defaultFemaleWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeFemaleGen4",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 15, 0, 0),//Torso
                new PedComponent(4, 77, 0, 0),//Pants
                new PedComponent(6, 51, 0, 0),//Shoes
                new PedComponent(8, 17, 0, 0),//Undershirt
                new PedComponent(11, 385, 0, 0),//Top
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = FullTattos(false, false),
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 385, 1, 0),

                //Undershirt
                new PedComponent(8, 17, 1, 0),
                new PedComponent(8, 17, 2, 0),
                new PedComponent(8, 17, 3, 0),
                new PedComponent(8, 17, 4, 0),
                new PedComponent(8, 17, 5, 0),
                new PedComponent(8, 17, 6, 0),
                new PedComponent(8, 17, 7, 0),
                new PedComponent(8, 17, 8, 0),
                new PedComponent(8, 17, 9, 0),
                new PedComponent(8, 17, 10, 0),
                new PedComponent(8, 17, 11, 0),

                //Boots
                new PedComponent(6, 51, 1, 0),
                new PedComponent(6, 51, 2, 0),
                new PedComponent(6, 51, 3, 0),
                new PedComponent(6, 51, 4, 0),
                new PedComponent(6, 51, 5, 0),

                //Pants
                new PedComponent(4, 77, 1, 0),

                new PedComponent(4, 73, 0, 0),
                new PedComponent(4, 73, 1, 0),
                new PedComponent(4, 73, 2, 0),
                new PedComponent(4, 73, 3, 0),
                new PedComponent(4, 73, 4, 0),
                new PedComponent(4, 73, 5, 0),

                new PedComponent(4, 74, 0, 0),
                new PedComponent(4, 74, 1, 0),
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 75, 0, 0),
                new PedComponent(4, 75, 1, 0),
                new PedComponent(4, 75, 2, 0),

                new PedComponent(4, 76, 0, 0),
                new PedComponent(4, 76, 1, 0),
                new PedComponent(4, 76, 2, 0),

                new PedComponent(4, 77, 1, 0),
                new PedComponent(4, 77, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        //DispatchablePeople.AngelsOfDeathPeds.Add(GeneralFemaleBiker4);

        DispatchablePerson GeneralFemaleBiker5 = new DispatchablePerson("mp_f_freemode_01", defaultFemaleAmbient + 5, defaultFemaleWanted + 5, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "LostFreemodeFemaleGen5",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(3, 14, 0, 0),//Torso
                new PedComponent(4, 77, 0, 0),//Pants
                new PedComponent(6, 51, 0, 0),//Shoes
                new PedComponent(8, 15, 0, 0),//Undershirt
                new PedComponent(11, 286, 0, 0),//Top
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = BothArmTattoos(true, false),
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            OptionalComponents = new List<PedComponent>()
            {
                //Top
                new PedComponent(11, 286, 1, 0),

                //Boots
                new PedComponent(6, 51, 1, 0),
                new PedComponent(6, 51, 2, 0),
                new PedComponent(6, 51, 3, 0),
                new PedComponent(6, 51, 4, 0),
                new PedComponent(6, 51, 5, 0),

                //Pants
                new PedComponent(4, 77, 1, 0),

                new PedComponent(4, 73, 0, 0),
                new PedComponent(4, 73, 1, 0),
                new PedComponent(4, 73, 2, 0),
                new PedComponent(4, 73, 3, 0),
                new PedComponent(4, 73, 4, 0),
                new PedComponent(4, 73, 5, 0),

                new PedComponent(4, 74, 0, 0),
                new PedComponent(4, 74, 1, 0),
                new PedComponent(4, 74, 2, 0),
                new PedComponent(4, 74, 3, 0),
                new PedComponent(4, 74, 4, 0),
                new PedComponent(4, 74, 5, 0),

                new PedComponent(4, 75, 0, 0),
                new PedComponent(4, 75, 1, 0),
                new PedComponent(4, 75, 2, 0),

                new PedComponent(4, 76, 0, 0),
                new PedComponent(4, 76, 1, 0),
                new PedComponent(4, 76, 2, 0),

                new PedComponent(4, 77, 1, 0),
                new PedComponent(4, 77, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.AngelsOfDeathPeds.Add(GeneralFemaleBiker5);
    }
    private OptionalAppliedOverlayLogic TShirtOverlays(bool isMale)
    {
        OptionalAppliedOverlayLogic toreturn = new OptionalAppliedOverlayLogic();

        toreturn.AppliedOverlayZonePercentages = new List<AppliedOverlayZonePercentage>() {

        new AppliedOverlayZonePercentage("ZONE_TORSO",100f,1),
        };

        //28 - 31, 34, 35 BIKE TEEs = good for full sHIRT
        if (isMale)
        {
            toreturn.OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_Biker_Tee_000_M","ZONE_TORSO"),//Ones that say liberty
                new OptionalAppliedOverlay("mpBiker_overlays","MP_Biker_Tee_001_M","ZONE_TORSO"),//2 and 3 say los santos

            };
        }
        else
        {
            toreturn.OptionalAppliedOverlays = new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_Biker_Tee_000_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_Biker_Tee_001_F","ZONE_TORSO"),

            };
        }

        return toreturn;
    }
    private OptionalAppliedOverlayLogic FullTattos(bool withTshirt, bool isMale)
    {
        OptionalAppliedOverlayLogic toreturn = BothArmTattoos(withTshirt, isMale);

        toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_HEAD", 7f, 1));

        if (!withTshirt)
        {
            toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_TORSO", 85f, 1));
        }

        if (isMale)
        {
            toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_000_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_001_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_003_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_005_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_006_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_008_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_009_M","ZONE_HEAD"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_010_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_011_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_013_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_017_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_018_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_019_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_021_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_023_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_026_M","ZONE_TORSO"),

                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_029_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_030_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_031_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_032_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_034_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_038_M","ZONE_HEAD"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_039_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_041_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_043_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_051_M","ZONE_HEAD"),
            });
        }
        else
        {
            toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_000_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_001_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_003_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_005_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_006_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_008_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_009_F","ZONE_HEAD"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_010_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_011_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_013_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_017_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_018_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_019_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_021_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_023_M","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_026_F","ZONE_TORSO"),

                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_029_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_030_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_031_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_032_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_034_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_038_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_039_F","ZONE_HEAD"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_041_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_043_F","ZONE_TORSO"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_051_F","ZONE_HEAD"),
            });
        }
        return toreturn;
    }
    private OptionalAppliedOverlayLogic BothArmTattoos(bool withTshirt, bool isMale)
    {
        OptionalAppliedOverlayLogic toreturn = new OptionalAppliedOverlayLogic();
        if (withTshirt)
        {
            toreturn = TShirtOverlays(isMale);
        }
        toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_RIGHT_ARM", 85f, 1));
        toreturn.AppliedOverlayZonePercentages.Add(new AppliedOverlayZonePercentage("ZONE_LEFT_ARM", 85f, 1));

        if (isMale)
        {
            toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_007_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_014_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_016_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_020_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_024_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_025_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_033_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_042_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_046_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_047_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_049_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_054_M","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_012_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_035_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_045_M","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_053_M","ZONE_LEFT_ARM"),
            });
        }
        else
        {
            toreturn.OptionalAppliedOverlays.AddRange(new List<OptionalAppliedOverlay>()
            {
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_007_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_012_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_014_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_016_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_020_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_024_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_025_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_033_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_035_F","ZONE_LEFT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_042_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_045_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_046_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_047_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_049_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_054_F","ZONE_RIGHT_ARM"),
                new OptionalAppliedOverlay("mpBiker_overlays","MP_MP_Biker_Tat_053_F","ZONE_LEFT_ARM"),
            });
        }
        return toreturn;
    }
}

