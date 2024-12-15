using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DispatchablePeople_Cops
{
    private DispatchablePeople DispatchablePeople;
    private List<PedPropComponent> MaleShortSleeveOptions;
    private List<PedPropComponent> FemaleShortSleeveOptions;
    private int optionalpropschance = 40;
    private List<string> GeneralMaleCopVoices = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" };
    private List<string> GeneralFemaleCopVoices = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" };


    private List<string> HPMaleCopVoices = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" };
    


    private List<PedPropComponent> ShortSleeveMaleOptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9), new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), new PedPropComponent(6, 3, 0), };
    private List<PedPropComponent> LongSleeveMaleOptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9), new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), new PedPropComponent(6, 3, 0), };
    private List<PedPropComponent> ShortSleeveFemaleOptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), };
    private List<PedPropComponent> LongSleeveFemaleOptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), };


    public DispatchablePeople_Cops(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }

    public void Setup()
    {

    }
    public DispatchablePerson CreateLSPDMPPed(int ambientSpawnChance, int wantedSpawnChance, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve, bool withArmor)
    {
        DispatchablePerson toReturn;
        if (isMale)
        {
            if (isShortSleeve)
            {
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"LSPDShortSleeveMaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                                new PedComponent(3, 11, 0, 0),
                                new PedComponent(4, 35, 0, 0),
                                new PedComponent(6, 25, 0, 0),
                                new PedComponent(8, 58, 0, 0),
                                new PedComponent(9, 11, 1, 0),
                                new PedComponent(11, 319, 0, 0),
                                new PedComponent(10,214,0)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = ShortSleeveMaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 229, 1),
                    NoHelmetPercentage = 0,
                };
            }
            else
            {
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"LSPDLongSleeveMaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                            new PedComponent(3, 1, 0, 0),
                            new PedComponent(4, 35, 0, 0),
                            new PedComponent(6, 25, 0, 0),
                            new PedComponent(8, 58, 0, 0),
                            new PedComponent(9, 11, 1, 0),
                            new PedComponent(11, 317, 0, 0),
                            new PedComponent(10,211,0)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = LongSleeveMaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 229, 1),
                    NoHelmetPercentage = 0,
                };
            }
        }
        else
        {
            if (isShortSleeve)
            {
                toReturn = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"LSPDShortSleeveFemaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                   new List<PedComponent>() {
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(8, 35, 0, 0),
                        new PedComponent(9, 6, 1, 0),
                        new PedComponent(11, 330, 0, 0),
                        new PedComponent(10,227,0)
                   },
                   new List<PedPropComponent>() { }),
                    OptionalProps = ShortSleeveFemaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 228, 1),
                    NoHelmetPercentage = 0,
                };
            }
            else
            {
                toReturn = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"LSPDLongSleeveFemaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                    new PedComponent(3, 3, 0, 0),
                    new PedComponent(4, 34, 0, 0),
                    new PedComponent(6, 55, 0, 0),
                    new PedComponent(8, 35, 0, 0),
                    new PedComponent(9, 6, 1, 0),
                    new PedComponent(11, 328, 0, 0),
                    new PedComponent(10,230,0)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = LongSleeveFemaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 228, 1),
                    NoHelmetPercentage = 0,
                };
            }
        }

        if (!withArmor)
        {
            toReturn.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 9);
        }
        return toReturn;
    }
    public DispatchablePerson CreateSAHPMPPed(int ambientSpawnChance, int wantedSpawnChance, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve, bool withArmor, bool withBoots)
    {
        DispatchablePerson toReturn;
        if (isMale)
        {

            //Male Boots
            //new PedComponent(4, 205, 0, 0),
            //new PedComponent(6, 103, 0, 0),


            //Male No Boots
            //new PedComponent(4, 143, 2, 0),
            //new PedComponent(6, 25, 0, 0),


            //Full Gear
            //new PedComponent(8, 58, 0, 0),
            //Smaller Gear
            //new PedComponent(8, 153, 0, 0),

            //Helmet
            //new PedPropComponent(0, 229, 0);

            //also has 1 is is darker helmet color

            if (isShortSleeve)
            {
                //HAS BOOTS
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"SAHPShortSleeveMaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = HPMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                                new PedComponent(3, 11, 0, 0),
                                new PedComponent(4, 205, 0, 0),
                                new PedComponent(6, 103, 0, 0),
                                new PedComponent(8, 153, 0, 0),
                                new PedComponent(9, 11, 0, 0),
                                new PedComponent(11, 319, 2, 0),
                                new PedComponent(10,214,1)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = ShortSleeveMaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 229, 0),
                    NoHelmetPercentage = 0,
                };
            }
            else
            {
                //HAS BOOTS
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"SAHPLongSleeveMaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = HPMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                            new PedComponent(3, 1, 0, 0),
                            new PedComponent(4, 205, 0, 0),
                            new PedComponent(6, 103, 0, 0),
                            new PedComponent(8, 153, 0, 0),
                            new PedComponent(9, 11, 0, 0),
                            new PedComponent(11, 317, 2, 0),
                            new PedComponent(10,211,1)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = LongSleeveMaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 229, 0),
                    NoHelmetPercentage = 0,
                };
            }
            if(!withBoots)
            {
                toReturn.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 4 || x.ComponentID == 6 || x.ComponentID == 8);
                toReturn.RequiredVariation.Components.Add(new PedComponent(4, 143, 2, 0));
                toReturn.RequiredVariation.Components.Add(new PedComponent(6, 25, 0, 0));
                toReturn.RequiredVariation.Components.Add(new PedComponent(8, 58, 0, 0));
            }
        }
        else
        {

            //FeMale Boots
            //new PedComponent(4, 220, 0, 0),
            //new PedComponent(6, 107, 0, 0),


            //FeMale No Boots
            //new PedComponent(4, 41, 0, 0),
            //new PedComponent(6, 55, 0, 0),


            //Full Gear
            //new PedComponent(8, 35, 0, 0),
            //Smaller Gear
            //new PedComponent(8, 189, 0, 0),

            //Helmet
            //228
            //decla 227-230
            //also has 1 is is darker helmet color

            if (isShortSleeve)
            {
                toReturn = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"SAHPShortSleeveFemaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                   new List<PedComponent>() {
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 220, 0, 0),
                        new PedComponent(6, 107, 0, 0),
                        new PedComponent(8, 189, 0, 0),
                        new PedComponent(9, 6, 0, 0),
                        new PedComponent(11, 330, 2, 0),
                        new PedComponent(10,230,1)
                   },
                   new List<PedPropComponent>() { }),
                    OptionalProps = ShortSleeveFemaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 228, 0),
                    NoHelmetPercentage = 0,
                };
            }
            else
            {
                toReturn = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"SAHPLongSleeveFemaleNew",
                    RandomizeHead = true,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                    new PedComponent(3, 3, 0, 0),
                    new PedComponent(4, 220, 0, 0),
                    new PedComponent(6, 107, 0, 0),
                    new PedComponent(8, 189, 0, 0),
                    new PedComponent(9, 6, 0, 0),
                    new PedComponent(11, 328, 2, 0),
                    new PedComponent(10, 227, 1)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = LongSleeveFemaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 228,0),
                    NoHelmetPercentage = 0,
                };
            }
            if (!withBoots)
            {
                toReturn.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 4 || x.ComponentID == 6 || x.ComponentID == 8);
                toReturn.RequiredVariation.Components.Add(new PedComponent(4, 41, 0, 0));
                toReturn.RequiredVariation.Components.Add(new PedComponent(6, 55, 0, 0));
                toReturn.RequiredVariation.Components.Add(new PedComponent(8, 35, 0, 0));
            }
        }

        if (!withArmor)
        {
            toReturn.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 9);
        }
        return toReturn;
    }


}

