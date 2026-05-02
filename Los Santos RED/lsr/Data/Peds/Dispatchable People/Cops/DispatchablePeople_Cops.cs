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
    public DispatchablePerson CreateLSPDMPPed(int ambientSpawnChance, int wantedSpawnChance, int minwantedLevelSpawn, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve, bool withArmor)
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
        else
        {
            toReturn.ArmorMin = 50;
            toReturn.ArmorMax = 50;
        }
        return toReturn;
    }
    public DispatchablePerson CreateLSPDMotorcycleMPPed(int ambientSpawnChance, int wantedSpawnChance, int minwantedLevelSpawn, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve)
    {
        DispatchablePerson toReturn;
        if (isMale)
        {
            if (isShortSleeve)
            {
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"LSPDShortSleeveMaleNewMotorcycle",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                                new PedComponent(3, 11, 0, 0),
                                new PedComponent(4, 130, 1, 0),
                                new PedComponent(6, 103, 0, 0),
                                new PedComponent(8, 153, 0, 0),
                                new PedComponent(11, 319, 8, 0),
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
                    DebugName = $"LSPDLongSleeveMaleNewMotorcycle",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                            new PedComponent(3, 1, 0, 0),
                            new PedComponent(4, 130, 1, 0),
                            new PedComponent(6, 103, 0, 0),
                            new PedComponent(8, 153, 0, 0),
                            new PedComponent(11, 317, 8, 0),
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
                    DebugName = $"LSPDShortSleeveFemaleNewMotorcycle",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                   new List<PedComponent>() {
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 136, 1, 0),
                        new PedComponent(6, 107, 0, 0),
                        new PedComponent(8, 189, 0, 0),
                        new PedComponent(11, 330, 8, 0),
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
                    DebugName = $"LSPDLongSleeveFemaleNewMotorcycle",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                    new PedComponent(3, 3, 0, 0),
                    new PedComponent(4, 136, 1, 0),
                    new PedComponent(6, 107, 0, 0),
                    new PedComponent(8, 189, 0, 0),
                    new PedComponent(11, 328, 8, 0),
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

        toReturn.GroupName = "MotorcycleCop";
        toReturn.UnitCode = "Mary";

        return toReturn;
    }
    public DispatchablePerson CreateSAHPMPPed(int ambientSpawnChance, int wantedSpawnChance, int minwantedLevelSpawn, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve, bool withArmor, bool withBoots)
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = HPMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                                new PedComponent(3, 11, 0, 0),
                                new PedComponent(4, 205, 0, 0),//202 from plebmaster
                                new PedComponent(6, 103, 0, 0),
                                new PedComponent(8, 153, 0, 0),
                                new PedComponent(9, 11, 0, 0),
                                new PedComponent(11, 319, 2, 0),
                                new PedComponent(10,214,1)//207-210 from plebmaster
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = HPMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                            new PedComponent(3, 1, 0, 0),
                            new PedComponent(4, 205, 0, 0),//217 from plebmaster
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
            if (!withBoots)
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
                    MinWantedLevelSpawn = minwantedLevelSpawn,
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
                    OverrideHelmet = new PedPropComponent(0, 228, 0),
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
        if (withBoots)
        {
            toReturn.GroupName = "MotorcycleCop";
            toReturn.UnitCode = "Mary";
        }
        else
        {
            toReturn.GroupName = "StandardSAHP";
        }
        if (!withArmor)
        {
            toReturn.RequiredVariation.Components.RemoveAll(x => x.ComponentID == 9);
        }
        else
        {
            toReturn.ArmorMin = 50;
            toReturn.ArmorMax = 50;
        }
        return toReturn;
    }
    public DispatchablePerson CreateSAPRMPPed(int ambientSpawnChance, int wantedSpawnChance,int minwantedLevelSpawn, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve)
    {
        DispatchablePerson toReturn;
        if (isMale)
        {
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
                toReturn = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
                {
                    DebugName = $"SAPRShortSleeveMaleNew",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                                new PedComponent(3, 11, 0, 0),
                                new PedComponent(4, 129, 5, 0),
                                new PedComponent(6, 25, 0, 0),
                                new PedComponent(8, 153, 0, 0),
                                new PedComponent(11, 319, 3, 0),
                                new PedComponent(10,214,2)
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
                    DebugName = $"SAPRLongSleeveMaleNew",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralMaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                            new PedComponent(3, 1, 0, 0),
                            new PedComponent(4, 129, 5, 0),
                            new PedComponent(6, 25, 0, 0),
                            new PedComponent(8, 153, 0, 0),
                            new PedComponent(11, 317, 3, 0),
                            new PedComponent(10,211,2)
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
                    DebugName = $"SAPRShortSleeveFemaleNew",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                   new List<PedComponent>() {
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 135, 5, 0),//new PedComponent(4, 128, 0, 0),//new PedComponent(4, 41, 0, 0),
                        new PedComponent(6, 55, 0, 0),
                        new PedComponent(8, 189, 0, 0),
                        new PedComponent(11, 330, 3, 0),
                        new PedComponent(10,230,2)
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
                    DebugName = $"SAPRLongSleeveFemaleNew",
                    RandomizeHead = true,
                    MinWantedLevelSpawn = minwantedLevelSpawn,
                    MaxWantedLevelSpawn = maxWantedLevelSpawn,
                    OverrideVoice = GeneralFemaleCopVoices,
                    RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                    new PedComponent(3, 3, 0, 0),
                    new PedComponent(4, 135, 5, 0),//new PedComponent(4, 128, 0, 0),//new PedComponent(4, 41, 0, 0),
                    new PedComponent(6, 55, 0, 0),
                    new PedComponent(8, 189, 0, 0),
                    new PedComponent(11, 328, 3, 0),
                    new PedComponent(10, 227, 2)
                    },
                    new List<PedPropComponent>() { }),
                    OptionalProps = LongSleeveFemaleOptionalProps,
                    OptionalPropChance = optionalpropschance,
                    OverrideHelmet = new PedPropComponent(0, 228, 1),
                    NoHelmetPercentage = 0,
                };
            }
        }
        return toReturn;
    }

    public DispatchablePerson GetGenericMPCopPed(int ambientSpawnChance, int wantedSpawnChance, int maxWantedLevelSpawn, bool isMale, bool isShortSleeve)
    {

        DispatchablePerson ShortSleeveMale = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "NooseShortSleeveArmedMerryMale"
                ,
            RandomizeHead = true
                ,
            MaxWantedLevelSpawn = maxWantedLevelSpawn
                ,
            OverrideVoice = GeneralMaleCopVoices
                ,
            RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 11, 0, 0), new PedComponent(4, 10, 0, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 319, 0, 0) },
                    new List<PedPropComponent>() { })
                ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9), new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), new PedPropComponent(6, 3, 0), }
                ,
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson LongSleeveMale = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "NooseLongSleeveArmedMerryMale",
            RandomizeHead = true
            ,
            MaxWantedLevelSpawn = maxWantedLevelSpawn
            ,
            OverrideVoice = GeneralMaleCopVoices
            ,
            RequiredVariation = new PedVariation(
                new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 10, 0, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 122, 0, 0), new PedComponent(9, 11, 1, 0), new PedComponent(11, 317, 0, 0) },
                new List<PedPropComponent>() { })
            ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 23, 9), new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), new PedPropComponent(6, 3, 0), }
            ,
            OptionalPropChance = optionalpropschance
        };


        DispatchablePerson ShortSleeveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "NooseShortSleeveArmedMerryFemale"
            ,
            RandomizeHead = true
            ,
            MaxWantedLevelSpawn = maxWantedLevelSpawn
            ,
            OverrideVoice = GeneralFemaleCopVoices
            ,
            RequiredVariation = new PedVariation(
                new List<PedComponent>() { new PedComponent(3, 9, 0, 0), new PedComponent(4, 6, 0, 0), new PedComponent(6, 55, 0, 0), new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 330, 0, 0) },
                new List<PedPropComponent>() { })
            ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
            ,
            OptionalPropChance = optionalpropschance
        };
        DispatchablePerson LongSleeveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "NooseLongSleeveArmedMerryFemale",
            RandomizeHead = true
            ,
            MaxWantedLevelSpawn = maxWantedLevelSpawn
            ,
            OverrideVoice = GeneralFemaleCopVoices
            ,
            RequiredVariation = new PedVariation(
                new List<PedComponent>() { new PedComponent(3, 3, 0, 0), new PedComponent(4, 6, 0, 0), new PedComponent(6, 55, 0, 0), new PedComponent(8, 152, 0, 0), new PedComponent(9, 6, 1, 0), new PedComponent(11, 328, 0, 0) },
                new List<PedPropComponent>() { })
            ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 25, 9), new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
            ,
            OptionalPropChance = optionalpropschance
        };



        if (isMale)
        {
            if (isShortSleeve)
            {
                return ShortSleeveMale;
            }
            return LongSleeveMale;
        }
        else
        {
            if (isShortSleeve)
            {
                return ShortSleeveFemale;
            }
            return LongSleeveFemale;
        }
    }
    public DispatchablePerson GetGenericMPDetectivePed(int ambientSpawnChance, int wantedSpawnChance, int maxwantedLevelSpawn, bool isMale)
    {
        DispatchablePerson DetectiveMale = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "MPDetectiveMale"
            ,
            GroupName = "Detective"
            ,
            RandomizeHead = true
            ,
            MaxWantedLevelSpawn = maxwantedLevelSpawn
            ,
            OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02", "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }
            ,
            RequiredVariation = new PedVariation(
                new List<PedComponent>() { new PedComponent(3, 1, 0, 0), new PedComponent(4, 35, 0, 0), new PedComponent(6, 10, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 130, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 348, 0, 0) },
                new List<PedPropComponent>() { })
            ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 37, 0), new PedPropComponent(1, 38, 0), new PedPropComponent(1, 8, 3), new PedPropComponent(1, 8, 5), new PedPropComponent(1, 8, 6), new PedPropComponent(1, 7, 0), new PedPropComponent(1, 2, 3), }
            ,
            OptionalPropChance = optionalpropschance
        };

        DispatchablePerson DetectiveFemale = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance)
        {
            DebugName = "MPDetectiveFemale"
                ,
            GroupName = "Detective"
                ,
            RandomizeHead = true
                ,
            MaxWantedLevelSpawn = maxwantedLevelSpawn
                ,
            OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }
                ,
            RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 3, 0, 0), new PedComponent(4, 34, 0, 0), new PedComponent(6, 29, 0, 0), new PedComponent(7, 0, 0, 0), new PedComponent(8, 160, 0, 0), new PedComponent(10, 0, 0, 0), new PedComponent(11, 366, 0, 0) },
                    new List<PedPropComponent>() { })
                ,
            OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 39, 0), new PedPropComponent(1, 40, 0), new PedPropComponent(1, 11, 0), new PedPropComponent(1, 11, 1), new PedPropComponent(1, 11, 3), new PedPropComponent(1, 24, 0), new PedPropComponent(6, 20, 2), }
                ,
            OptionalPropChance = optionalpropschance
        };
        if (isMale)
        {
            return DetectiveMale;
        }
        else
        {
            return DetectiveFemale;
        }
    }



    public DispatchablePerson GetGenericSWATMPCopPed(int ambientSpawnChance, int wantedSpawnChance, int maxwantedLevelSpawn, bool isMale, int Style, string groupName)
    {
        DispatchablePerson male1 = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance) {
            DebugName = "MPMaleSWAT1"
                , MinWantedLevelSpawn = 3
                , AccuracyMin = 25
                , AccuracyMax = 40
                , ShootRateMin = 400
                , ShootRateMax = 500
                , CombatAbilityMin = 1
                , CombatAbilityMax = 2
                , HealthMin = 100
                , HealthMax = 100
                , ArmorMin = 100
                , ArmorMax = 100
                ,GroupName = groupName
                , MaxWantedLevelSpawn = maxwantedLevelSpawn
                , RandomizeHead = true
                , OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                , RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 17, 0, 0), new PedComponent(4, 121, 0, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 2, 0, 0), new PedComponent(10, 70, 0, 0), new PedComponent(11, 320, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 141, 0), new PedPropComponent(1, 23, 9) })
        };
        DispatchablePerson male2 = new DispatchablePerson("mp_m_freemode_01", ambientSpawnChance, wantedSpawnChance) {
            DebugName = "MPMaleSWAT2"
            , MinWantedLevelSpawn = 3
            , AccuracyMin = 25
            , AccuracyMax = 40
            , ShootRateMin = 400
            , ShootRateMax = 500
            , CombatAbilityMin = 1
            , CombatAbilityMax = 2
            , HealthMin = 100
            , HealthMax = 100
            , ArmorMin = 100
            , ArmorMax = 100
                            ,
            MaxWantedLevelSpawn = maxwantedLevelSpawn
            ,
            GroupName = groupName
            , RandomizeHead = true
            , OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
            , RequiredVariation = new PedVariation(
                new List<PedComponent>() { new PedComponent(3, 17, 0, 0), new PedComponent(4, 121, 0, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 2, 0, 0), new PedComponent(10, 70, 0, 0), new PedComponent(11, 320, 0, 0), },
                new List<PedPropComponent>() { new PedPropComponent(0, 141, 0) })
        };

        DispatchablePerson female1 = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance) {
            DebugName = "MPFemaleSWAT1"
                , MinWantedLevelSpawn = 3
                , AccuracyMin = 25
                , AccuracyMax = 40
                , ShootRateMin = 400
                , ShootRateMax = 500
                , CombatAbilityMin = 1
                , CombatAbilityMax = 2
                , HealthMin = 100
                , HealthMax = 100
                , ArmorMin = 100
                , ArmorMax = 100
                                ,
            MaxWantedLevelSpawn = maxwantedLevelSpawn
                , RandomizeHead = true
                ,
            GroupName = groupName
                , OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                , RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0), new PedComponent(4, 127, 0, 0), new PedComponent(6, 24, 0, 0), new PedComponent(8, 9, 0, 0), new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0), new PedPropComponent(1, 25, 9) })
        };
        DispatchablePerson female2 = new DispatchablePerson("mp_f_freemode_01", ambientSpawnChance, wantedSpawnChance) {
                DebugName = "MPFemaleSWAT2"
                ,MinWantedLevelSpawn = 3
                ,AccuracyMin = 25
                ,AccuracyMax = 40
                ,ShootRateMin = 400
                ,ShootRateMax = 500
                ,CombatAbilityMin = 1
                ,CombatAbilityMax = 2
                ,HealthMin = 100
                ,HealthMax = 100
                ,ArmorMin = 100
                ,ArmorMax = 100
                                ,
            MaxWantedLevelSpawn = maxwantedLevelSpawn
                ,
            
            GroupName = groupName
            ,RandomizeHead = true
                ,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" }
                ,RequiredVariation = new PedVariation(
                    new List<PedComponent>() { new PedComponent(3, 18, 0, 0),new PedComponent(4, 127, 0, 0),new PedComponent(6, 24, 0, 0),new PedComponent(8, 9, 0, 0),new PedComponent(10, 79, 0, 0), new PedComponent(11, 331, 0, 0), },
                    new List<PedPropComponent>() { new PedPropComponent(0, 140, 0)})
            };
        if(isMale)
        {
            if(Style == 1)
            {
                return male1;
            }
            else
            {
                return male2;
            }
        }
        else
        {
            if (Style == 1)
            {
                return female1;
            }
            else
            {
                return female2;
            }
        }
    }

}

