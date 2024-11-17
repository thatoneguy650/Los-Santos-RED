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
    public DispatchablePeople_NorthHolland(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.NorthHollandPeds = new List<DispatchablePerson>() { };
        MaleFreemodePeds();
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

