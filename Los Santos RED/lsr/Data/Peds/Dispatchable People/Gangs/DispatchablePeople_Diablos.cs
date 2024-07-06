using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_Diablos
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
    public DispatchablePeople_Diablos(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void Setup()
    {
        DispatchablePeople.DiablosPeds = new List<DispatchablePerson>() { };
        DefaultPeds();
    }
    private void DefaultPeds()
    {
        DispatchablePerson DiablosMale1 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "DiablosMale1",
            //OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true, 45f, 0f, 0f, 0f),
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                     new PedComponent(2, 1, 4, 0),
                     new PedComponent(3, 18, 0, 0),
                     new PedComponent(4, 1, 8, 0),
                     new PedComponent(6, 7, 0, 0),
                     new PedComponent(7, 0, 0, 0),
                     new PedComponent(8, 15, 0, 0),
                     new PedComponent(11, 224, 0, 0) }, new List<PedPropComponent>() { new PedPropComponent(0, 14, 3), })
        };
        DispatchablePeople.DiablosPeds.Add(DiablosMale1);
        DispatchablePerson DiablosMale2 = new DispatchablePerson("mp_m_freemode_01", defaultAmbient, defaultWanted, defaultAccuracyMin, defaultAccuracyMax, defaultShootRateMin, defaultShootRateMax, defaultCombatAbilityMin, defaultCombatAbilityMax)
        {
            RandomizeHead = true,
            OverrideVoice = DefaultVoicesMale,
            DebugName = "DiablosMale2",
            //OptionalAppliedOverlayLogic = DispatchablePeople.GenericGangTattoos(true, 45f, 0f, 0f, 0f),
            RequiredVariation = new PedVariation(new List<PedComponent>() {
                    new PedComponent(2, 4, 0, 0),
                    new PedComponent(3, 18, 0, 0),
                     new PedComponent(4, 9, 0, 0),
                     new PedComponent(6, 7, 0, 0),
                     new PedComponent(7, 0, 0, 0),
                     new PedComponent(8, 15, 0, 0),
                     new PedComponent(11, 224, 0, 0) }, new List<PedPropComponent>() { new PedPropComponent(0, 14, 2), })
        };
        DispatchablePeople.DiablosPeds.Add(DiablosMale2);
       
    }


}

