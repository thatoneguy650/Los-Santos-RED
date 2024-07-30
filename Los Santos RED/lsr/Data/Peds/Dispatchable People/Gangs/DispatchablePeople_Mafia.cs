using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_Mafia
{
    private DispatchablePeople DispatchablePeople;
    private int optionalComponentDefault = 80;
    private int defaultAmbient = 25;
    private int defaultWanted = 25;

    private int defaultAccuracyMin = 5;
    private int defaultAccuracyMax = 10;

    private int defaultShootRateMin = 400;
    private int defaultShootRateMax = 600;

    private int defaultCombatAbilityMin = 0;
    private int defaultCombatAbilityMax = 1;

    private List<string> DefaultVoicesMale = new List<string>() { "A_M_Y_BUSINESS_01_WHITE_FULL_01", "A_M_Y_BUSINESS_02_WHITE_FULL_01", "A_M_M_SKATER_01_WHITE_FULL_01" };
    public DispatchablePeople_Mafia(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.MafiaPeds = new List<DispatchablePerson>() { };
        DefaultPeds();
    }
    private void DefaultPeds()
    {
        DispatchablePerson MafiaGen1 = new DispatchablePerson("mp_m_freemode_01", 35, 35, 7, 12, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "MafiaSuitGen1",
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 10, 0, 0),
                new PedComponent(7, 21, 2, 0),
                new PedComponent(8, 10, 0, 0),
                new PedComponent(11, 4, 0, 0) }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {
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
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen1);
        DispatchablePerson MafiaGen2 = new DispatchablePerson("mp_m_freemode_01", 35, 35, 7, 12, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "MafiaSuitGen2",
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                     new PedComponent(3, 4, 0, 0),
                     new PedComponent(4, 10, 0, 0),
                     new PedComponent(6, 10, 0, 0),
                     new PedComponent(7, 0, 0, 0),
                     new PedComponent(8, 11, 0, 0),
                     new PedComponent(11, 4, 0, 0) }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {
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
            },
            OptionalComponentChance = 80,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen2);
        DispatchablePerson MafiaGen3 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeShirtPulledUp",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 11, 0, 0),//5,3
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 21, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 0, 0, 0),//0
                new PedComponent(11, 346, 25, 0),
                new PedComponent(2, 10, 0, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {
                //Undershirts
                new PedComponent(8, 0, 1, 0),
                new PedComponent(8, 0, 2, 0),

                //Jackets/Shirts
                new PedComponent(11, 346, 0, 0),
                new PedComponent(11, 346, 1, 0),
                new PedComponent(11, 346, 2, 0),
                new PedComponent(11, 346, 3, 0),
                new PedComponent(11, 346, 4, 0),
                new PedComponent(11, 346, 5, 0),
                new PedComponent(11, 346, 18, 0),
                new PedComponent(11, 346, 20, 0),
                new PedComponent(11, 346, 21, 0),
                new PedComponent(11, 346, 22, 0),
                new PedComponent(11, 346, 23, 0),

                //Pants
                new PedComponent(4, 10, 1, 0),
                new PedComponent(4, 10, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen3);
        DispatchablePerson MafiaGen4 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeNoJacket",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 5, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 21, 0, 0),
                new PedComponent(7, 17, 0, 0),
                new PedComponent(8, 15, 0, 0),
                new PedComponent(11, 5, 0, 0),
                new PedComponent(2, 10, 0, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {
                //Chain
                new PedComponent(7, 17, 1, 0),
                new PedComponent(7, 17, 2, 0),

                //Pants
                new PedComponent(4, 10, 1, 0),
                new PedComponent(4, 10, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen4);
        DispatchablePerson MafiaGen5 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeTracksuitTop",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 6, 0, 0),//3,3
                new PedComponent(4, 13, 0, 0),
                new PedComponent(6, 30, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 15, 0, 0),
                new PedComponent(11, 113, 3, 0),
                new PedComponent(2, 19, 0, 0),

            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {
                //Chain
                new PedComponent(7, 17, 0, 0),
                new PedComponent(7, 17, 1, 0),
                new PedComponent(7, 17, 2, 0),
                //Tracksuit
                new PedComponent(11, 113, 0, 0),
                new PedComponent(11, 113, 1, 0),
                new PedComponent(11, 113, 2, 0),

                new PedComponent(11, 141, 0, 0),
                new PedComponent(11, 141, 1, 0),
                new PedComponent(11, 141, 2, 0),
                new PedComponent(11, 141, 3, 0),
                new PedComponent(11, 141, 4, 0),
                new PedComponent(11, 141, 5, 0),
                new PedComponent(11, 141, 6, 0),
                new PedComponent(11, 141, 7, 0),
                new PedComponent(11, 141, 8, 0),
                new PedComponent(11, 141, 9, 0),
                new PedComponent(11, 141, 10, 0),

                //Pants
                new PedComponent(4, 13, 1, 0),
                new PedComponent(4, 13, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen5);
        DispatchablePerson MafiaGen6 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeLeatherJacket",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 1, 0, 0),
                new PedComponent(4, 13, 0, 0),
                new PedComponent(6, 10, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 10, 14, 0),
                new PedComponent(11, 508, 0, 0),//38
                new PedComponent(2, 19, 0, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {                  
                //Jacket
                new PedComponent(11, 508, 1, 0),//38
                new PedComponent(11, 508, 2, 0),//38
                new PedComponent(11, 508, 3, 0),//38
                new PedComponent(11, 508, 4, 0),//38

                //Pants
                new PedComponent(4, 13, 1, 0),
                new PedComponent(4, 13, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen6);
        DispatchablePerson MafiaGen7 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeOpenJacket",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 12, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 36, 3, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 63, 5, 0),
                new PedComponent(11, 59, 2, 0),
                new PedComponent(2, 10, 0, 0),
            }, new List<PedPropComponent>() { }),
            OptionalComponents = new List<PedComponent>() 
            {                  
                //Jacket
                new PedComponent(11, 59, 0, 0),
                new PedComponent(11, 59, 1, 0),

                new PedComponent(8, 63, 0, 0),
                new PedComponent(8, 63, 1, 0),
                new PedComponent(8, 63, 2, 0),
                new PedComponent(8, 63, 3, 0),
                new PedComponent(8, 63, 4, 0),

                //Pants
                new PedComponent(4, 10, 1, 0),
                new PedComponent(4, 10, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen7);
        DispatchablePerson MafiaGen8 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            DebugName = "GTA4MafiaRemadeOpenShirt2",
            OverrideVoice = DefaultVoicesMale,
            RequiredVariation = new PedVariation(new List<PedComponent>() 
            {
                new PedComponent(3, 0, 0, 0),//5
                new PedComponent(4, 13, 0, 0),
                new PedComponent(6, 111, 1, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 0, 0, 0),//5
                new PedComponent(11, 429, 14, 0),
                new PedComponent(2, 10, 0, 0),
            }, new List<PedPropComponent>() { })
            ,OptionalComponents = new List<PedComponent>() 
            {                  
                //Jacket
                new PedComponent(11, 429, 9, 0),
                new PedComponent(11, 429, 15, 0),

                new PedComponent(11, 428, 0, 0),
                new PedComponent(11, 428, 1, 0),
                new PedComponent(11, 428, 2, 0),
                new PedComponent(11, 428, 3, 0),
                new PedComponent(11, 428, 4, 0),
                new PedComponent(11, 428, 5, 0),

                //Pants
                new PedComponent(4, 13, 1, 0),
                new PedComponent(4, 13, 2, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.MafiaPeds.Add(MafiaGen8);
    }
}

