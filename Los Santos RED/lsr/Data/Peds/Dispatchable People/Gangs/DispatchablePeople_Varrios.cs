using System.Collections.Generic;


public class DispatchablePeople_Varrios
{
    private DispatchablePeople DispatchablePeople;
    private int optionalComponentDefault = 80;

    private int defaultVanilla = 65;

    private int defaultFemale = 10;

    private int defaultAmbient = 25;
    private int defaultWanted = 25;

    private int defaultAccuracyMin = 5;
    private int defaultAccuracyMax = 10;

    private int defaultShootRateMin = 400;
    private int defaultShootRateMax = 600;

    private int defaultCombatAbilityMin = 0;
    private int defaultCombatAbilityMax = 1;

    private List<string> DefaultVoicesMale = new List<string>() { "G_M_Y_MEXGOON_01_LATINO_MINI_01","G_M_Y_MEXGOON_01_LATINO_MINI_02","G_M_Y_MEXGOON_02_LATINO_MINI_01","G_M_Y_MEXGOON_02_LATINO_MINI_02","G_M_Y_MEXGOON_03_LATINO_MINI_01","G_M_Y_MEXGOON_03_LATINO_MINI_02","G_M_Y_POLOGOON_01_LATINO_MINI_01","G_M_Y_POLOGOON_01_LATINO_MINI_02", };
    private List<string> DefaultVoicesFemale = new List<string>() { "G_F_Y_VAGOS_01_LATINO_MINI_01","G_F_Y_VAGOS_01_LATINO_MINI_02","G_F_Y_VAGOS_01_LATINO_MINI_03","G_F_Y_VAGOS_01_LATINO_MINI_04", };
    public DispatchablePeople_Varrios(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.VarriosPeds = new List<DispatchablePerson>() { };
        DefaultPeds();
    }
    private void DefaultPeds()
    {
        DispatchablePerson VarriosVanillaMale1 = new DispatchablePerson("g_m_y_azteca_01", defaultVanilla, defaultVanilla, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax) { DebugName = "VarriosMale" };
        DispatchablePeople.VarriosPeds.Add(VarriosVanillaMale1);
        DispatchablePerson VarriosMale1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "VarriosMale1",
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 11, 0, 0),
                     new PedComponent(4, 1, 0, 0),//Jeans
                     new PedComponent(6, 12, 0, 0),//Boots
                     new PedComponent(8, 76, 0, 0),//white t shirt
                     new PedComponent(11, 346, 24, 0)//Button Up
            
            }, new List<PedPropComponent>() {}),
            OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true, 0f, 0f, 85f, 0f),
            OptionalComponents = new List<PedComponent>()
            {
                //Pants
                new PedComponent(4, 1, 1, 0),
                new PedComponent(4, 1, 2, 0),
                new PedComponent(4, 1, 3, 0),
                new PedComponent(4, 1, 4, 0),
                new PedComponent(4, 1, 5, 0),
                new PedComponent(4, 1, 6, 0),

                new PedComponent(6, 12, 6, 0),
                new PedComponent(6, 12, 7, 0),
                new PedComponent(6, 12, 8, 0),
                new PedComponent(6, 12, 10, 0),
            },
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.VarriosPeds.Add(VarriosMale1);
        DispatchablePerson VarriosMale2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "VarriosMale2",
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 11, 0, 0),
                     new PedComponent(4, 62, 0, 0),//Work Shorts
                     new PedComponent(6, 9, 0, 0),//Sneakers with socks
                     new PedComponent(8, 15, 0, 0),//no shirt
                     new PedComponent(11, 82, 6, 0),//Loose polo
            }, new List<PedPropComponent>() { new PedPropComponent(6,14,2) }),
            OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true,0f,0f,85f,0f),
            OptionalComponents = new List<PedComponent>()
            {
                //Pants
                new PedComponent(4, 62, 1, 0),
                new PedComponent(4, 62, 2, 0),
                new PedComponent(4, 62, 3, 0),

                //Shoes
                new PedComponent(6, 9, 1, 0),
                new PedComponent(6, 9, 2, 0),
            },
            OptionalPropChance = 60,
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.VarriosPeds.Add(VarriosMale2);

        DispatchablePerson VarriosFemale1 = new DispatchablePerson("mp_f_freemode_01", defaultFemale, defaultFemale, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            DebugName = "VarriosFemale1",
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 15, 0, 0),//Full Torso
                     new PedComponent(4, 45, 0, 0),//Baggy Cargo pants
                     new PedComponent(6, 3, 0, 0),//Chucktaylors
                     new PedComponent(8, 15, 0, 0),//no shirt
                     new PedComponent(11, 74, 0, 0),//Cropped Tank
            }, new List<PedPropComponent>() { }),
            OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true, 12f,1, 95f,3, 85f,2, 0f,0),
            OptionalComponents = new List<PedComponent>()
            {
                //Pants
                new PedComponent(4, 45, 1, 0),
                new PedComponent(4, 45, 2, 0),
                new PedComponent(4, 45, 3, 0),

                new PedComponent(4, 74, 0, 0),//Distressed tight jeans
                new PedComponent(4, 74, 1, 0),//Distressed tight jeans
                new PedComponent(4, 74, 2, 0),//Distressed tight jeans
                new PedComponent(4, 74, 3, 0),//Distressed tight jeans
                new PedComponent(4, 74, 4, 0),//Distressed tight jeans
                new PedComponent(4, 74, 5, 0),//Distressed tight jeans

                //Top
                new PedComponent(11, 74, 1, 0),//Cropped Tank
                new PedComponent(11, 74, 2, 0),//Cropped Tank

                //Shoes
                new PedComponent(6, 3, 3, 0),//Chucktaylors
                new PedComponent(6, 3, 14, 0),//Chucktaylors

            },
            OptionalPropChance = 60,
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.VarriosPeds.Add(VarriosFemale1);
        DispatchablePerson VarriosFemale2 = new DispatchablePerson("mp_f_freemode_01", defaultFemale, defaultFemale, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesFemale,
            DebugName = "VarriosFemale2",
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(3, 6, 0, 0),//Full Torso
                     new PedComponent(4, 45, 0, 0),//Baggy Cargo pants
                     new PedComponent(6, 3, 0, 0),//Chucktaylors
                     new PedComponent(8, 5, 0, 0),//cropped tank
                     new PedComponent(11, 120, 9, 0),//top buttoned shit
            }, new List<PedPropComponent>() {  }),
            OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true, 12f, 1, 95f, 3, 0f, 0, 0f, 0),
            OptionalComponents = new List<PedComponent>()
            {
                //Pants
                new PedComponent(4, 45, 1, 0),
                new PedComponent(4, 45, 2, 0),
                new PedComponent(4, 45, 3, 0),

                new PedComponent(4, 74, 0, 0),//Distressed tight jeans
                new PedComponent(4, 74, 1, 0),//Distressed tight jeans
                new PedComponent(4, 74, 2, 0),//Distressed tight jeans
                new PedComponent(4, 74, 3, 0),//Distressed tight jeans
                new PedComponent(4, 74, 4, 0),//Distressed tight jeans
                new PedComponent(4, 74, 5, 0),//Distressed tight jeans

                //Undershirt
                new PedComponent(8, 5, 1, 0),//cropped tank
                new PedComponent(8, 5, 7, 0),//cropped tank

                //Top
                new PedComponent(11, 120, 0, 0),//top buttoned shit
                new PedComponent(11, 120, 6, 0),//top buttoned shit

                //Shoes
                new PedComponent(6, 3, 3, 0),//Chucktaylors
                new PedComponent(6, 3, 14, 0),//Chucktaylors
            },
            OptionalPropChance = 60,
            OptionalComponentChance = optionalComponentDefault,
        };
        DispatchablePeople.VarriosPeds.Add(VarriosFemale2);
    }

}

