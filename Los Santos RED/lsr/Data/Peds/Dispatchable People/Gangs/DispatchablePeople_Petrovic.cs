using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_Petrovic
{
    private DispatchablePeople DispatchablePeople;
    private int optionalComponentDefault = 80;
    private int optionaPropDefault = 30;
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
        "G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_01",
        "G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_02",
        "G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_01",
        "G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_02",
    };
    public DispatchablePeople_Petrovic(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.PetrovicPeds = new List<DispatchablePerson>() { };
        MaleFreemodePeds();
    }
    private void MaleFreemodePeds()
    {
        PedPropComponent DefaultMaleHelmet = new PedPropComponent(1, 182, 0);
        float NoHelmetPercentage = 90f;


        DispatchablePerson PetMale1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "Petrovic1",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 40, 0, 0),
                new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 25, 0, 0),
                new PedComponent(6, 21, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 11, 1, 0),
                new PedComponent(11, 3, 2, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(2, 40, 0, 0),
                new PedComponent(11, 3, 11, 0),
                new PedComponent(11, 59, 1, 0),
                new PedComponent(11, 59, 2, 0),
                new PedComponent(8, 11, 14, 0),
                new PedComponent(6, 20, 0, 0),
                new PedComponent(6,21,2,0),
                new PedComponent(4,25,1,0),
                new PedComponent(4,25,6,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.PetrovicPeds.Add(PetMale1);

        DispatchablePerson PetMale2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "Petrovic2",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 31, 0, 0),
                new PedComponent(3, 6, 0, 0),
                new PedComponent(4, 126, 0, 0),
                new PedComponent(6, 12, 0, 0),
                new PedComponent(8, 15, 0, 0),
                new PedComponent(11, 141, 4, 0),//366 = OPEN
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(11, 141, 0, 0),
                new PedComponent(11, 141, 1, 0),
                new PedComponent(11, 141, 2, 0),
                new PedComponent(11, 141, 5, 0),
                new PedComponent(11, 141, 6, 0),
                new PedComponent(11, 141, 7, 0),
                new PedComponent(11, 141, 8, 0),
                new PedComponent(11, 141, 9, 0),
                new PedComponent(11, 141, 10, 0),
                new PedComponent(4,141,0,0),
                new PedComponent(6,10,0,0),
                new PedComponent(6,12,6,0),
                new PedComponent(4,8,0,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.PetrovicPeds.Add(PetMale2);


        DispatchablePerson PetMale3 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "Petrovic3",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 5, 0, 0),
                new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 0, 1, 0),
                new PedComponent(6, 54, 0, 0),
                new PedComponent(8, 72, 0, 0),
                new PedComponent(11, 70, 1, 0),//366 = OPEN
            }, new List<PedPropComponent>() { new PedPropComponent(1,18,1) }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(11,338,0,0),
                new PedComponent(8,9,2,0),
                new PedComponent(4,37,1,0),
                new PedComponent(6,14,11,0),
                new PedComponent(8,9,0,0),
                new PedComponent(4,10,0,0),
                new PedComponent(6,14,0,0),
                new PedComponent(8,75,2,0),
                new PedComponent(4,143,0,0),
                new PedComponent(6,111,1,0),
                new PedComponent(11,338,2,0),
                new PedComponent(8,72,3,0),
                new PedComponent(4,7,0,0),
                new PedComponent(6,66,0,0),
                new PedComponent(11,70,2,0),
                new PedComponent(8,72,3,0),
                new PedComponent(4,7,0,0),
                new PedComponent(6,66,0,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.PetrovicPeds.Add(PetMale3);


        DispatchablePerson PetMale4 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            DebugName = "Petrovic4",
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            OverrideHelmet = DefaultMaleHelmet,
            NoHelmetPercentage = NoHelmetPercentage,
            RequiredVariation = new PedVariation(new List<PedComponent>()
            {
                new PedComponent(2, 9, 0, 0),
                new PedComponent(3, 14, 0, 0),
                new PedComponent(7, 189, 1, 0),
                new PedComponent(6, 75, 17, 0),
                new PedComponent(8, 29, 7, 0),
                new PedComponent(11, 338, 0, 0),//366 = OPEN
            }, new List<PedPropComponent>() { new PedPropComponent(1, 56, 6) }),
            OptionalComponents = new List<PedComponent>()
            {
                new PedComponent(11,338,5,0),
                new PedComponent(8,1,1,0),
                new PedComponent(6,42,1,0),
                new PedComponent(4,0,10,0),
                new PedComponent(2,9,0,0),
                new PedComponent(11,338,4,0),
                new PedComponent(8,29,15,0),
                new PedComponent(6,42,1,0),
                new PedComponent(4,0,1,0),
            },
            OptionalComponentChance = optionalComponentDefault,
            OptionalPropChance = optionaPropDefault,
        };
        DispatchablePeople.PetrovicPeds.Add(PetMale4);
    }
}

