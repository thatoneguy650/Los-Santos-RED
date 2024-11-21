using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_NorthHolland
{
    private DispatchablePeople DispatchablePeople;
    private int optionalComponentDefault = 80;
    private int optionaPropDefault = 50;
    private int defaultAmbient = 25;
    private int defaultWanted = 25;

    private int defaultFemaleAmbient = 5;
    private int defaultFemaleWanted = 5;

    private int defaultAccuracyMin = 5;
    private int defaultAccuracyMax = 10;

    private int defaultShootRateMin = 400;
    private int defaultShootRateMax = 600;

    private int defaultCombatAbilityMin = 0;
    private int defaultCombatAbilityMax = 1;

    private List<string> DefaultVoicesMale = new List<string>() {
        "A_M_M_SOUCENT_01_BLACK_FULL_01",
        "A_M_M_SOUCENT_02_BLACK_FULL_01",
        "A_M_M_SOUCENT_03_BLACK_FULL_01",
        "A_M_M_SOUCENT_04_BLACK_FULL_01",
    };
    private List<string> DefaultVoicesFemale = new List<string>() {
        "A_F_M_SOUCENT_01_BLACK_FULL_01",
        "A_F_M_SOUCENT_02_BLACK_FULL_01",
        "A_F_O_SOUCENT_01_BLACK_FULL_01",
        "A_F_O_SOUCENT_02_BLACK_FULL_01",
    };
    public DispatchablePeople_NorthHolland(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.NorthHollandPeds = new List<DispatchablePerson>() { };
        MaleFreemodePeds();
        FemaleFreemodePeds();
    }
    private void FemaleFreemodePeds()
    {
        PedPropComponent DefaultFemaleHelmet = new PedPropComponent(1, 182, 0);
        float NoHelmetPercentage = 90f;

        DispatchablePerson NHHFemale1 = new DispatchablePerson("mp_f_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustlerFemale1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            OverrideHelmet = DefaultFemaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 79, 0, 0),
                new PedComponent(3, 1, 0, 0),
                new PedComponent(4, 87, 5, 0),
                new PedComponent(6, 11, 2, 0),
                new PedComponent(8, 86, 12, 0),
                new PedComponent(11, 193, 1, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(4,4,0,0),
                new PedComponent(4,4,2,0),
                new PedComponent(4,4,3,0),
                new PedComponent(4,4,8,0),
                new PedComponent(4,4,9,0),
                new PedComponent(4,4,12,0),
                new PedComponent(4,4,13,0),
                new PedComponent(4,30,0,0),
                new PedComponent(4,30,1,0),
                new PedComponent(4,30,2,0),
                new PedComponent(4,87,0,0),
                new PedComponent(4,87,3,0),
                new PedComponent(4,87,8,0),
                new PedComponent(4,87,9,0),
                new PedComponent(4,87,11,0),
                new PedComponent(4,87,12,0),
                new PedComponent(6,10,1,0),
                new PedComponent(6,11,3,0),
                new PedComponent(6,49,0,0),
                new PedComponent(6,50,1,0),
                new PedComponent(6,96,4,0),
                new PedComponent(6,96,5,0),
                new PedComponent(6,96,6,0),
                new PedComponent(6,96,11,0),
                new PedComponent(6,96,14,0),
                new PedComponent(6,97,4,0),
                new PedComponent(6,97,5,0),
                new PedComponent(6,97,6,0),
                new PedComponent(6,97,11,0),
                new PedComponent(6,97,14,0),
                new PedComponent(8,26,0,0),
                new PedComponent(8,26,1,0),
                new PedComponent(8,26,2,0),
                new PedComponent(8,86,0,0),
                new PedComponent(8,86,1,0),
                new PedComponent(8,86,18,0),
                new PedComponent(8,86,23,0),
                new PedComponent(8,86,24,0),
                new PedComponent(11,193,2,0),
                new PedComponent(11,193,5,0),
                new PedComponent(11,193,7,0),
                new PedComponent(11,193,9,0),
                new PedComponent(11,193,14,0),
                new PedComponent(11,193,15,0),
                new PedComponent(11,193,16,0),
                new PedComponent(11,193,20,0),
                new PedComponent(11,193,21,0),
                new PedComponent(11,193,24,0),
                new PedComponent(11,193,16,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(1,16,1),
                new PedPropComponent(1,16,4),
                new PedPropComponent(1,16,8),
                new PedPropComponent(1,16,9),
                new PedPropComponent(2,13,0),
                new PedPropComponent(2,14,0),
                new PedPropComponent(2,15,0),
                new PedPropComponent(2,16,0),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHFemale1);

    }
    private void MaleFreemodePeds()
    {
        PedPropComponent DefaultMaleHelmet = new PedPropComponent(1, 182, 0);
        float NoHelmetPercentage = 90f;

        DispatchablePerson NHHMale1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 50, 0, 0),
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 4, 0, 0),
                new PedComponent(6, 12, 6, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 115, 0, 0),
                new PedComponent(10, 0, 0, 0),
                new PedComponent(11, 7, 2, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(4,0,0,0),
                new PedComponent(4,0,4,0),
                new PedComponent(4,0,6,0),
                new PedComponent(4,0,8,0),
                new PedComponent(4,0,9,0),
                new PedComponent(4,0,12,0),
                new PedComponent(4,1,0,0),
                new PedComponent(4,1,1,0),
                new PedComponent(4,1,5,0),
                new PedComponent(4,1,8,0),
                new PedComponent(4,1,11,0),
                new PedComponent(4,1,12,0),
                new PedComponent(4,8,0,0),
                new PedComponent(4,9,7,0),
                new PedComponent(4,63,0,0),
                new PedComponent(4,126,0,0),
                new PedComponent(4,129,1,0),
                new PedComponent(4,129,3,0),
                new PedComponent(4,143,0,0),
                new PedComponent(6,0,10,0),
                new PedComponent(6,1,0,0),
                new PedComponent(6,1,1,0),
                new PedComponent(6,1,2,0),
                new PedComponent(6,1,14,0),
                new PedComponent(6,12,0,0),
                new PedComponent(6,12,1,0),
                new PedComponent(6,12,3,0),
                new PedComponent(6,12,6,0),
                new PedComponent(6,12,12,0),
                new PedComponent(6,32,0,0),
                new PedComponent(6,32,1,0),
                new PedComponent(6,57,6,0),
                new PedComponent(6,57,10,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,17,0),
                new PedComponent(6,75,20,0),
                new PedComponent(6,75,22,0),
                new PedComponent(6,75,23,0),
                new PedComponent(6,75,25,0),
                new PedComponent(6,116,0,0),
                new PedComponent(6,116,1,0),
                new PedComponent(6,117,14,0),
                new PedComponent(7,50,0,0),
                new PedComponent(7,50,1,0),
                new PedComponent(7,53,0,0),
                new PedComponent(7,53,1,0),
                new PedComponent(7,88,0,0),
                new PedComponent(7,88,1,0),
                new PedComponent(7,89,0,0),
                new PedComponent(7,89,1,0),
                new PedComponent(7,163,0,0),
                new PedComponent(7,163,2,0),
                new PedComponent(7,189,0,0),
                new PedComponent(7,189,2,0),
                new PedComponent(8,0,0,0),
                new PedComponent(8,0,1,0),
                new PedComponent(8,0,2,0),
                new PedComponent(8,9,1,0),
                new PedComponent(8,9,2,0),
                new PedComponent(8,9,11,0),
                new PedComponent(8,23,0,0),
                new PedComponent(8,23,1,0),
                new PedComponent(8,76,0,0),
                new PedComponent(8,76,1,0),
                new PedComponent(8,135,5,0),
                new PedComponent(8,135,7,0),
                new PedComponent(8,135,9,0),
                new PedComponent(8,135,11,0),
                new PedComponent(8,135,14,0),
                new PedComponent(8,135,19,0),
                new PedComponent(8,187,3,0),
                new PedComponent(11,7,1,0),
                new PedComponent(11,7,14,0),
                new PedComponent(11,74,7,0),
                new PedComponent(11,74,9,0),
                new PedComponent(11,88,5,0),
                new PedComponent(11,88,6,0),
                new PedComponent(11,167,3,0),
                new PedComponent(11,167,7,0),
                new PedComponent(11,191,7,0),
                new PedComponent(11,191,16,0),
                new PedComponent(11,191,21,0),
                new PedComponent(11,191,24,0),
                new PedComponent(11,230,3,0),
                new PedComponent(11,230,4,0),
                new PedComponent(11,261,8,0),
                new PedComponent(11,261,9,0),
                new PedComponent(11,269,2,0),
                new PedComponent(11,269,14,0),
                new PedComponent(11,309,6,0),
                new PedComponent(11,309,10,0),
                new PedComponent(11,390,0,0),
                new PedComponent(11,390,1,0),
                new PedComponent(11,390,2,0),
                new PedComponent(11,390,3,0),
                new PedComponent(11,390,8,0),
                new PedComponent(11,390,19,0),
                new PedComponent(11,391,1,0),

            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(1,3,5),
                new PedPropComponent(1,5,0),
                new PedPropComponent(1,5,5),
                new PedPropComponent(1,10,0),
                new PedPropComponent(1,18,5),
                new PedPropComponent(1,18,0),
                new PedPropComponent(1,18,5),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale1);

        DispatchablePerson NHHMale2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler2",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 0, 0, 0),
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 0, 1, 0),
                new PedComponent(6, 32, 1, 0),
                new PedComponent(8, 135, 11, 0),
                new PedComponent(11, 167, 3, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(4,0,0,0),
                new PedComponent(4,0,4,0),
                new PedComponent(4,0,6,0),
                new PedComponent(4,0,8,0),
                new PedComponent(4,0,9,0),
                new PedComponent(4,0,12,0),
                new PedComponent(4,1,0,0),
                new PedComponent(4,1,1,0),
                new PedComponent(4,1,5,0),
                new PedComponent(4,1,8,0),
                new PedComponent(4,1,11,0),
                new PedComponent(4,1,12,0),
                new PedComponent(4,8,0,0),
                new PedComponent(4,9,7,0),
                new PedComponent(4,63,0,0),
                new PedComponent(4,126,0,0),
                new PedComponent(4,129,1,0),
                new PedComponent(4,129,3,0),
                new PedComponent(4,143,0,0),
                new PedComponent(6,0,10,0),
                new PedComponent(6,1,0,0),
                new PedComponent(6,1,1,0),
                new PedComponent(6,1,2,0),
                new PedComponent(6,1,14,0),
                new PedComponent(6,12,0,0),
                new PedComponent(6,12,1,0),
                new PedComponent(6,12,3,0),
                new PedComponent(6,12,6,0),
                new PedComponent(6,12,12,0),
                new PedComponent(6,32,0,0),
                new PedComponent(6,32,1,0),
                new PedComponent(6,57,6,0),
                new PedComponent(6,57,10,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,17,0),
                new PedComponent(6,75,20,0),
                new PedComponent(6,75,22,0),
                new PedComponent(6,75,23,0),
                new PedComponent(6,75,25,0),
                new PedComponent(6,116,0,0),
                new PedComponent(6,116,1,0),
                new PedComponent(6,117,14,0),
                new PedComponent(7,50,0,0),
                new PedComponent(7,50,1,0),
                new PedComponent(7,53,0,0),
                new PedComponent(7,53,1,0),
                new PedComponent(7,88,0,0),
                new PedComponent(7,88,1,0),
                new PedComponent(7,89,0,0),
                new PedComponent(7,89,1,0),
                new PedComponent(7,163,0,0),
                new PedComponent(7,163,2,0),
                new PedComponent(7,189,0,0),
                new PedComponent(7,189,2,0),
                new PedComponent(8,0,0,0),
                new PedComponent(8,0,1,0),
                new PedComponent(8,0,2,0),
                new PedComponent(8,9,1,0),
                new PedComponent(8,9,2,0),
                new PedComponent(8,9,11,0),
                new PedComponent(8,23,0,0),
                new PedComponent(8,23,1,0),
                new PedComponent(8,76,0,0),
                new PedComponent(8,76,1,0),
                new PedComponent(8,135,5,0),
                new PedComponent(8,135,7,0),
                new PedComponent(8,135,9,0),
                new PedComponent(8,135,11,0),
                new PedComponent(8,135,14,0),
                new PedComponent(8,135,19,0),
                new PedComponent(8,187,3,0),
                new PedComponent(11,7,1,0),
                new PedComponent(11,7,14,0),
                new PedComponent(11,74,7,0),
                new PedComponent(11,74,9,0),
                new PedComponent(11,88,5,0),
                new PedComponent(11,88,6,0),
                new PedComponent(11,167,3,0),
                new PedComponent(11,167,7,0),
                new PedComponent(11,191,7,0),
                new PedComponent(11,191,16,0),
                new PedComponent(11,191,21,0),
                new PedComponent(11,191,24,0),
                new PedComponent(11,230,3,0),
                new PedComponent(11,230,4,0),
                new PedComponent(11,261,8,0),
                new PedComponent(11,261,9,0),
                new PedComponent(11,269,2,0),
                new PedComponent(11,269,14,0),
                new PedComponent(11,309,6,0),
                new PedComponent(11,309,10,0),
                new PedComponent(11,390,0,0),
                new PedComponent(11,390,1,0),
                new PedComponent(11,390,2,0),
                new PedComponent(11,390,3,0),
                new PedComponent(11,390,8,0),
                new PedComponent(11,390,19,0),
                new PedComponent(11,391,1,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(0,55,1),
                new PedPropComponent(0,55,9),
                new PedPropComponent(0,55,10),
                new PedPropComponent(0,96,0),
                new PedPropComponent(0,96,4),
                new PedPropComponent(0,96,6),
                new PedPropComponent(0,143,0),
                new PedPropComponent(0,143,2),
                new PedPropComponent(1,5,0),
                new PedPropComponent(1,5,9),
                new PedPropComponent(1,3,5),
                new PedPropComponent(1,5,0),
                new PedPropComponent(1,5,5),
                new PedPropComponent(1,10,0),
                new PedPropComponent(1,18,5),
                new PedPropComponent(1,18,0),
                new PedPropComponent(1,18,5),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale2);


        DispatchablePerson NHHMale3 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler3",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 75, 0, 0),
                new PedComponent(3, 1, 0, 0),
                new PedComponent(4, 26, 0, 0),
                new PedComponent(6, 32, 1, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 16, 0, 0),
                new PedComponent(11, 74, 8, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(4,0,0,0),
                new PedComponent(4,0,4,0),
                new PedComponent(4,0,6,0),
                new PedComponent(4,0,8,0),
                new PedComponent(4,0,9,0),
                new PedComponent(4,0,12,0),
                new PedComponent(4,1,0,0),
                new PedComponent(4,1,1,0),
                new PedComponent(4,1,5,0),
                new PedComponent(4,1,8,0),
                new PedComponent(4,1,11,0),
                new PedComponent(4,1,12,0),
                new PedComponent(4,8,0,0),
                new PedComponent(4,9,7,0),
                new PedComponent(4,63,0,0),
                new PedComponent(4,126,0,0),
                new PedComponent(4,129,1,0),
                new PedComponent(4,129,3,0),
                new PedComponent(4,143,0,0),
                new PedComponent(6,0,10,0),
                new PedComponent(6,1,0,0),
                new PedComponent(6,1,1,0),
                new PedComponent(6,1,2,0),
                new PedComponent(6,1,14,0),
                new PedComponent(6,12,0,0),
                new PedComponent(6,12,1,0),
                new PedComponent(6,12,3,0),
                new PedComponent(6,12,6,0),
                new PedComponent(6,12,12,0),
                new PedComponent(6,32,0,0),
                new PedComponent(6,32,1,0),
                new PedComponent(6,57,6,0),
                new PedComponent(6,57,10,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,15,0),
                new PedComponent(6,75,17,0),
                new PedComponent(6,75,20,0),
                new PedComponent(6,75,22,0),
                new PedComponent(6,75,23,0),
                new PedComponent(6,75,25,0),
                new PedComponent(6,116,0,0),
                new PedComponent(6,116,1,0),
                new PedComponent(6,117,14,0),
                new PedComponent(7,50,0,0),
                new PedComponent(7,50,1,0),
                new PedComponent(7,53,0,0),
                new PedComponent(7,53,1,0),
                new PedComponent(7,88,0,0),
                new PedComponent(7,88,1,0),
                new PedComponent(7,89,0,0),
                new PedComponent(7,89,1,0),
                new PedComponent(7,163,0,0),
                new PedComponent(7,163,2,0),
                new PedComponent(7,189,0,0),
                new PedComponent(7,189,2,0),
                new PedComponent(8,0,0,0),
                new PedComponent(8,0,1,0),
                new PedComponent(8,0,2,0),
                new PedComponent(8,9,1,0),
                new PedComponent(8,9,2,0),
                new PedComponent(8,9,11,0),
                new PedComponent(8,23,0,0),
                new PedComponent(8,23,1,0),
                new PedComponent(8,76,0,0),
                new PedComponent(8,76,1,0),
                new PedComponent(8,135,5,0),
                new PedComponent(8,135,7,0),
                new PedComponent(8,135,9,0),
                new PedComponent(8,135,11,0),
                new PedComponent(8,135,14,0),
                new PedComponent(8,135,19,0),
                new PedComponent(8,187,3,0),
                new PedComponent(11,7,1,0),
                new PedComponent(11,7,14,0),
                new PedComponent(11,74,7,0),
                new PedComponent(11,74,9,0),
                new PedComponent(11,88,5,0),
                new PedComponent(11,88,6,0),
                new PedComponent(11,167,3,0),
                new PedComponent(11,167,7,0),
                new PedComponent(11,191,7,0),
                new PedComponent(11,191,16,0),
                new PedComponent(11,191,21,0),
                new PedComponent(11,191,24,0),
                new PedComponent(11,230,3,0),
                new PedComponent(11,230,4,0),
                new PedComponent(11,261,8,0),
                new PedComponent(11,261,9,0),
                new PedComponent(11,269,2,0),
                new PedComponent(11,269,14,0),
                new PedComponent(11,309,6,0),
                new PedComponent(11,309,10,0),
                new PedComponent(11,390,0,0),
                new PedComponent(11,390,1,0),
                new PedComponent(11,390,2,0),
                new PedComponent(11,390,3,0),
                new PedComponent(11,390,8,0),
                new PedComponent(11,390,19,0),
                new PedComponent(11,391,1,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(0,55,1),
                new PedPropComponent(0,55,9),
                new PedPropComponent(0,55,10),
                new PedPropComponent(0,96,0),
                new PedPropComponent(0,96,4),
                new PedPropComponent(0,96,6),
                new PedPropComponent(0,143,0),
                new PedPropComponent(0,143,2),
                new PedPropComponent(1,5,0),
                new PedPropComponent(1,5,9),
                new PedPropComponent(1,3,5),
                new PedPropComponent(1,5,0),
                new PedPropComponent(1,5,5),
                new PedPropComponent(1,10,0),
                new PedPropComponent(1,18,5),
                new PedPropComponent(1,18,0),
                new PedPropComponent(1,18,5),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale3);
    }
    private void MaleFreemodePeds_Old()
    {
        PedPropComponent DefaultMaleHelmet = new PedPropComponent(1, 182, 0);
        float NoHelmetPercentage = 90f;

        DispatchablePerson NHHMale1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 50, 0, 0),
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 5, 0, 0),
                new PedComponent(6, 99, 9, 0),
                new PedComponent(7, 53, 0, 0),
                new PedComponent(8, 115, 0, 0),
                new PedComponent(11, 3, 5, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 5, 3, 0),
                new PedComponent(6, 99, 17, 0),
                new PedComponent(7, 163, 2, 0),
                new PedComponent(11, 3, 3, 0),
                new PedComponent(4, 72, 5, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(0,120,4),
                new PedPropComponent(0,120,5),
                new PedPropComponent(0,120,4),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale1);

        DispatchablePerson NHHMale2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler2",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 0, 0, 0),
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 0, 1, 0),
                new PedComponent(6, 148, 0, 0),
                new PedComponent(8, 135, 11, 0),
                new PedComponent(11, 167, 0, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(11, 167, 2, 0),
                new PedComponent(11, 167, 3, 0),
                new PedComponent(8, 135, 9, 0),
                new PedComponent(8, 135, 14, 0),
                new PedComponent(4, 0, 8, 0),
                new PedComponent(4, 0, 12, 0),
                new PedComponent(6, 116, 0, 0),
                new PedComponent(6, 116, 12, 0),
                new PedComponent(7, 53, 1, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(0,55,0),
                new PedPropComponent(0,55,1),
                new PedPropComponent(0,96,1),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale2);


        DispatchablePerson NHHMale3 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "NHHustler3",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 75, 0, 0),
                new PedComponent(3, 1, 0, 0),
                new PedComponent(4, 26, 0, 0),
                new PedComponent(6, 32, 1, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 15, 0, 0),
                new PedComponent(11, 307, 9, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(7, 17, 0, 0),
                new PedComponent(4, 26, 4, 0),
                new PedComponent(6, 32, 5, 0),
                new PedComponent(7, 53, 0, 0),
                new PedComponent(11, 307, 6, 0),
                new PedComponent(4, 55, 0, 0),
                new PedComponent(6, 32, 0, 0),
                new PedComponent(7, 163, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalProps = new List<PedPropComponent>()
            {
                new PedPropComponent(0,55,9),
                new PedPropComponent(0,143,2),
                new PedPropComponent(0,5,0),
                new PedPropComponent(0,5,9),
            },
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.NorthHollandPeds.Add(NHHMale3);
    }
}

