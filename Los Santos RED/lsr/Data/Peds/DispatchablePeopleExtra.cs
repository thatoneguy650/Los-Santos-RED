//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//internal class DispatchablePeopleExtra
//{
//    public void Test()
//    {
//        List<DispatchablePerson> Unused1_FEJ = new List<DispatchablePerson>()
//        {
          
            
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 1),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 38, 13),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Leather Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 38, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 1),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 38, 13),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Business casual>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 3, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 293, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 10, 13),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 0),
//                        new PedComponent(3, 88, 1),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 1),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 10, 13),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 3, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 0),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Raid Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 0),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 1),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 0),
//                        new PedComponent(5, 59, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 230, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 5),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 63, 3),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Raid Jacket GED>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 5),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 63, 3),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 0),
//                        new PedComponent(5, 59, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Plain Clothes GED>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 208, 5),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 113, 0),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 525, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 7, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 1),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 64, 1),
//                        new PedComponent(4, 3, 3),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 26, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Business casual>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 28, 2),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 24, 5),
//                        new PedComponent(4, 3, 3),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 26, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 0),
//                        new PedComponent(3, 105, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 0),
//                        new PedComponent(4, 3, 3),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 26, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 366, 1),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 0),
//                        new PedComponent(4, 3, 3),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 26, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Raid Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 0),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 217, 1),
//                        new PedComponent(4, 3, 3),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 21, 0),
//                        new PedComponent(5, 59, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 262, 0),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 5, 0),
//                        new PedComponent(6, 62, 20),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 26, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Raid Jacket GED>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 0),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 57, 2),
//                        new PedComponent(4, 5, 0),
//                        new PedComponent(6, 62, 20),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 21, 0),
//                        new PedComponent(5, 59, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Plain Clothes GED>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 73, 2),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 79, 0),
//                        new PedComponent(4, 5, 0),
//                        new PedComponent(6, 62, 20),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 7, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 49, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 93, 2),
//                        new PedComponent(3, 19, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 0),
//                        new PedComponent(4, 12, 2),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 47, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 84, 2),
//                        new PedComponent(3, 31, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 14, 2),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Dirtbike patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 16, 0),
//                        new PedPropComponent(1, 25, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 152, 0),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 55, 0),
//                        new PedComponent(4, 67, 11),
//                        new PedComponent(6, 47, 3),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 18, 7),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Dirtbike Unit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 16, 0),
//                        new PedPropComponent(1, 27, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 0),
//                        new PedComponent(3, 18, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 32, 0),
//                        new PedComponent(4, 69, 11),
//                        new PedComponent(6, 48, 3),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 22, 7),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 79, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 108, 0),
//                        new PedComponent(3, 16, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 67, 0),
//                        new PedComponent(4, 64, 0),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 78, 1),
//                        new PedPropComponent(1, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 99, 0),
//                        new PedComponent(3, 17, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 0),
//                        new PedComponent(4, 66, 0),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 101, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 12, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 74, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 102, 0),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 15, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 74, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 92, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 11, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 74, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 93, 0),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 14, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 74, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male METRO Div Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 101, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 12, 0),
//                        new PedComponent(8, 94, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male METRO Div Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 102, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 15, 0),
//                        new PedComponent(8, 94, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female METRO Div Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 92, 1),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 11, 0),
//                        new PedComponent(8, 101, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female METRO Div Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 93, 1),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 14, 0),
//                        new PedComponent(8, 101, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPD SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 0),
//                        new PedPropComponent(1, 21, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 185, 0),
//                        new PedComponent(11, 220, 0),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 31, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPD SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 0),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 185, 0),
//                        new PedComponent(11, 230, 0),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 48, 0),
//                        new PedComponent(4, 30, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 18, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 2),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 2),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 265, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 64, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 5, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 274, 1),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 64, 1),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 30, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 279, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 25, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 1),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 1),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 41, 1),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 2),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 2),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 2),
//                        new PedComponent(3, 26, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 1),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 2),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 2),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 2),
//                        new PedComponent(3, 28, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 53, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 13),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 13),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 1),
//                        new PedComponent(4, 89, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 49, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 94, 2),
//                        new PedComponent(3, 19, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 12, 1),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 47, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 85, 2),
//                        new PedComponent(3, 31, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 14, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(1, 23, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 94, 2),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 87, 0),
//                        new PedComponent(4, 87, 7),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 8, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(1, 25, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 85, 2),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 65, 0),
//                        new PedComponent(4, 90, 7),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 9, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 1),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 15),
//                        new PedComponent(4, 22, 3),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 23, 1),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Business Casual>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 16),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 22, 3),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 23, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 1),
//                        new PedComponent(3, 93, 1),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 15),
//                        new PedComponent(4, 22, 3),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 23, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 15),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 4),
//                        new PedComponent(4, 22, 3),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 23, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Raid Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 1),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 15),
//                        new PedComponent(4, 22, 3),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 3),
//                        new PedComponent(5, 60, 1),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 3, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 208, 16),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 63, 4),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 23, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Gang Detail Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 17),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 63, 4),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 3),
//                        new PedComponent(5, 60, 1),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Gang Detail Armor>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 73, 18),
//                        new PedComponent(3, 151, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 115, 1),
//                        new PedComponent(4, 4, 1),
//                        new PedComponent(6, 63, 4),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 7, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 3),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 5),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Business Casual>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 357, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 1),
//                        new PedComponent(3, 105, 1),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 5),
//                        new PedComponent(4, 3, 63),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 366, 12),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 4),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Raid Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 1),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 217, 12),
//                        new PedComponent(4, 3, 63),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 21, 0),
//                        new PedComponent(5, 60, 1),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 73, 1),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 5, 9),
//                        new PedComponent(6, 62, 23),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Gang Detail Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 1),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 57, 1),
//                        new PedComponent(4, 5, 9),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 21, 3),
//                        new PedComponent(5, 60, 1),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Gang Detail Armor>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 73, 1),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 81, 1),
//                        new PedComponent(4, 5, 9),
//                        new PedComponent(6, 62, 23),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 29, 4),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 11, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 87, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 1),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 1, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 87, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 1),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 1),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 10, 0),
//                        new PedComponent(8, 31, 1),
//                        new PedComponent(4, 90, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 1),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 22),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 1),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 1, 0),
//                        new PedComponent(8, 31, 1),
//                        new PedComponent(4, 90, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 1),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Dirtbike patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 16, 1),
//                        new PedPropComponent(1, 25, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 152, 1),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 55, 0),
//                        new PedComponent(4, 67, 10),
//                        new PedComponent(6, 47, 3),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 18, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Dirtbike Unit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 16, 1),
//                        new PedPropComponent(1, 27, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 1),
//                        new PedComponent(3, 18, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 32, 0),
//                        new PedComponent(4, 69, 10),
//                        new PedComponent(6, 48, 3),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 22, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 1),
//                        new PedPropComponent(1, 23, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 220, 2),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 35, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 1),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 230, 2),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 1),
//                        new PedComponent(6, 36, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSSD Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 79, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 108, 1),
//                        new PedComponent(3, 96, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 43, 1),
//                        new PedComponent(4, 64, 1),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSSD Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 78, 0),
//                        new PedPropComponent(1, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 99, 1),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 30, 0),
//                        new PedComponent(4, 66, 1),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 31, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 4),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 30, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 41, 2),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 41, 2),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 4),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 41, 2),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 265, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 64, 4),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 19, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 30, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 41, 2),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 25, 1),
//                        new PedComponent(6, 15, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 2),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 41, 2),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 2),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 4),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 32, 0),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 4),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 32, 0),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 4),
//                        new PedComponent(3, 26, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 32, 0),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 4),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 31, 0),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 4),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 31, 0),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 4),
//                        new PedComponent(3, 28, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 31, 0),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 55, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 4),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 89, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 4),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 89, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 4),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 4),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 89, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male CVS Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 7),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 3),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female CVS Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 7),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 89, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 3),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male CVS Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 86, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 3),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female CVS Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 89, 11),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 3),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 87, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 5),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 87, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 90, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 20),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 5),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 90, 11),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Riot Gear>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 125, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 185, 0),
//                        new PedComponent(11, 150, 5),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 53, 0),
//                        new PedComponent(4, 125, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 5, 1),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Riot Gear>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 124, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 185, 0),
//                        new PedComponent(11, 147, 5),
//                        new PedComponent(3, 39, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 27, 1),
//                        new PedComponent(4, 131, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 3, 1),
//                        new PedComponent(5, 65, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 39, 1),
//                        new PedPropComponent(1, 21, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 3),
//                        new PedComponent(11, 220, 7),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 37, 1),
//                        new PedComponent(6, 35, 1),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 38, 1),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 3),
//                        new PedComponent(11, 230, 7),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 36, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 2),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 79, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 108, 3),
//                        new PedComponent(3, 96, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 48, 0),
//                        new PedComponent(4, 64, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 78, 0),
//                        new PedPropComponent(1, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 99, 3),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 29, 0),
//                        new PedComponent(4, 66, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 0),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 14),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 35, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Inspector>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 15),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 35, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Inspector (Vest)>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 14),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 10),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 35, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 3),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 13),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 39, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 366, 14),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 0),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 39, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Inspector>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 27, 4),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 7, 0),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 39, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SAHP Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 8),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 14),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 8),
//                        new PedComponent(5, 29, 9),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SAHP Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 8),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 13),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 39, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 4, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 4, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 5),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger K-9>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 13),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 14),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger K-9>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 13),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 14),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 9),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 8, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 265, 3),
//                        new PedComponent(3, 38, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 64, 6),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 21, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 4),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Ranger Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 4),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 6),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 6),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 6),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 19),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 10),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 2, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 265, 4),
//                        new PedComponent(3, 38, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 64, 9),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 19),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 21, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASP Lifeguard Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 19),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 49, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 4, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 5),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 9),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 8, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 274, 3),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 8),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 64, 6),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 18),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 19, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 4),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Ranger Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 4),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 4, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 6),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 6),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 6),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 19),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 10),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 19),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 19, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 5, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 274, 4),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 64, 9),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASP Lifeguard Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 31, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 5),
//                        new PedComponent(5, 36, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Uniform Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 13, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Uniform Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Uniform Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 3),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 13, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 3),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 265, 2),
//                        new PedComponent(3, 38, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 64, 3),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 8, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 274, 2),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 64, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 30, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 279, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 6),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 65, 1),
//                        new PedComponent(4, 22, 8),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 34, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 3, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 1),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 3),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 3),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 3),
//                        new PedComponent(3, 26, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 32, 2),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 3),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 3),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 3),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 3),
//                        new PedComponent(3, 28, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 5),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 6),
//                        new PedComponent(4, 22, 5),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 12, 15),
//                        new PedComponent(9, 23, 4),
//                        new PedComponent(5, 29, 8),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 6, 15),
//                        new PedComponent(3, 5, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 20, 0),
//                        new PedComponent(4, 23, 5),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 60, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Detective>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 6),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 16, 1),
//                        new PedComponent(4, 22, 5),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 12, 15),
//                        new PedComponent(9, 23, 4),
//                        new PedComponent(5, 29, 8),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 107, 0),
//                        new PedComponent(11, 292, 6),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 132, 5),
//                        new PedComponent(4, 22, 5),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 119, 1),
//                        new PedComponent(9, 23, 4),
//                        new PedComponent(5, 29, 8),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 307, 2),
//                        new PedComponent(3, 88, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 6),
//                        new PedComponent(4, 22, 5),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 12, 15),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 60, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 294, 2),
//                        new PedComponent(3, 6, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 20, 0),
//                        new PedComponent(4, 23, 5),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 60, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 9, 10),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 9, 0),
//                        new PedComponent(4, 5, 11),
//                        new PedComponent(6, 3, 9),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 60, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Raid Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 4),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 325, 2),
//                        new PedComponent(3, 152, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 102, 17),
//                        new PedComponent(4, 0, 4),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 3),
//                        new PedComponent(5, 60, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Task Force>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 73, 18),
//                        new PedComponent(3, 151, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 115, 0),
//                        new PedComponent(4, 0, 4),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 7, 1),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 49, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 94, 1),
//                        new PedComponent(3, 19, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 12, 1),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Bicycle Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 47, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 85, 1),
//                        new PedComponent(3, 31, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 14, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(1, 15, 9),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 94, 1),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 87, 0),
//                        new PedComponent(4, 87, 14),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 8, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(1, 25, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 85, 1),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 65, 0),
//                        new PedComponent(4, 90, 14),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 9, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 23),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 166, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 11, 1),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 87, 14),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 23),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 183, 0),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 15, 7),
//                        new PedComponent(8, 38, 0),
//                        new PedComponent(4, 87, 14),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO K-9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 23),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 163, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 10, 1),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 90, 14),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO K-9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 23),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 185, 0),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 14, 7),
//                        new PedComponent(8, 51, 0),
//                        new PedComponent(4, 90, 14),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 1),
//                        new PedPropComponent(1, 21, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 220, 3),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 1),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 230, 3),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 79, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 108, 2),
//                        new PedComponent(3, 96, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 43, 2),
//                        new PedComponent(4, 64, 1),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Pilot Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 78, 2),
//                        new PedPropComponent(1, 13, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 99, 2),
//                        new PedComponent(3, 36, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 30, 2),
//                        new PedComponent(4, 66, 1),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Vintage Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 33, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 15),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 44, 0),
//                        new PedComponent(4, 10, 15),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 56, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Vintage Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 33, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 16),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 44, 0),
//                        new PedComponent(4, 10, 15),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 56, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Vintage Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 33, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 16),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 44, 0),
//                        new PedComponent(4, 10, 15),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 56, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Vintage Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 15),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 52, 0),
//                        new PedComponent(4, 3, 14),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 33, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Vintage Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 16),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 52, 0),
//                        new PedComponent(4, 3, 14),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 33, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Vintage Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 16),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 52, 0),
//                        new PedComponent(4, 3, 14),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 33, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Vintage Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 30, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 266, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 44, 0),
//                        new PedComponent(4, 10, 15),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 56, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 64, 18),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Vintage Coat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 32, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 275, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 52, 0),
//                        new PedComponent(4, 3, 14),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 33, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 64, 18),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male BCSO Vintage Motorcycle>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 4),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 16),
//                        new PedComponent(3, 19, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 44, 0),
//                        new PedComponent(4, 32, 3),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 56, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female BCSO Vintage Motorcycle>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 4),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 16),
//                        new PedComponent(3, 31, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 52, 0),
//                        new PedComponent(4, 31, 3),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 33, 1),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 54, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 10, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 0),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 38, 8),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 28, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 31, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 293, 0),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 16, 0),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 38, 8),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 28, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Response>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 0),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 6),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Response>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 366, 0),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 6),
//                        new PedComponent(4, 3, 0),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 27, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 9, 0),
//                        new PedComponent(4, 3, 0),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 307, 3),
//                        new PedComponent(3, 88, 1),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 0),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 38, 8),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 28, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female Crime Scene Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 294, 3),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 16),
//                        new PedComponent(4, 3, 0),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 31, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 47, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Field Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 3),
//                        new PedComponent(3, 6, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 28, 3),
//                        new PedComponent(4, 47, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 22, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Task Force>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 7),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 311, 1),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 116, 0),
//                        new PedComponent(4, 47, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 7, 2),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Task Force>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 7),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 335, 0),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 163, 8),
//                        new PedComponent(4, 54, 3),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 24, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 11),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 40, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 11),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 39, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 11),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 39, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 39, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police K9 Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 12),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 39, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 20, 9),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB Police K9 Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 12),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 39, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 97, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 20, 9),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Police Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 11),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 48, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Police Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 11),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 47, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Police Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 11),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 47, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 34, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB Police Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 47, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 39, 0),
//                        new PedPropComponent(1, 21, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 220, 5),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 116, 0),
//                        new PedComponent(4, 31, 4),
//                        new PedComponent(6, 35, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 3),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB SWAT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 38, 0),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 230, 5),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 4),
//                        new PedComponent(6, 36, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 3),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male FIB HRT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 39, 1),
//                        new PedPropComponent(1, 21, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 4),
//                        new PedComponent(11, 220, 6),
//                        new PedComponent(3, 141, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 37, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 8),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female FIB HRT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 38, 1),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 4),
//                        new PedComponent(11, 230, 6),
//                        new PedComponent(3, 174, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 36, 2),
//                        new PedComponent(6, 36, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 8),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 1),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 4),
//                        new PedComponent(4, 10, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DOA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 4),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 39, 0),
//                        new PedComponent(4, 3, 2),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 50, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 37, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 4),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 10, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DOA Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 27, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 7, 0),
//                        new PedComponent(4, 3, 2),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 50, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Response>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 37, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 292, 4),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 8),
//                        new PedComponent(4, 10, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 4),
//                        new PedComponent(4, 10, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DOA Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 4),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 39, 0),
//                        new PedComponent(4, 3, 2),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 50, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 2, 4),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 4, 4),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA Task Force>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 10),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 121, 0),
//                        new PedComponent(11, 2, 4),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 133, 10),
//                        new PedComponent(4, 4, 4),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DOA SRT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 0),
//                        new PedPropComponent(1, 26, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 220, 4),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 103, 0),
//                        new PedComponent(4, 31, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 4),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DOA SRT Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 0),
//                        new PedPropComponent(1, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 230, 4),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 4),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USMS Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 4),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 13, 0),
//                        new PedComponent(9, 42, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USMS Marshal>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 42, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USMS Response>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 11),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 42, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USMS Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 7),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 4),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 17, 7),
//                        new PedComponent(5, 62, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USMS Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 14),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 7),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 133, 13),
//                        new PedComponent(4, 86, 6),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 77, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female USMS Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 6),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 64, 0),
//                        new PedComponent(4, 3, 1),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 44, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female USMS Marshal>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 221, 0),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 7, 0),
//                        new PedComponent(4, 3, 1),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 44, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female USMS Response>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 332, 0),
//                        new PedComponent(3, 1, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 11),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 44, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 10),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 10),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 10),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 10),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 10),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 10),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 35, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA TRU Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 39, 1),
//                        new PedPropComponent(1, 26, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 220, 8),
//                        new PedComponent(3, 141, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 37, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA TRU Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 38, 1),
//                        new PedPropComponent(1, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 230, 8),
//                        new PedComponent(3, 174, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 36, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male Border Patrol Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 3),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 10),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 4),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male Border Patrol Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 10),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 4),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male Border Patrol Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 4),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 8),
//                        new PedComponent(5, 56, 6),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female Border Patrol Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 3),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 10),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 4),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female Border Patrol Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 10),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 4),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female Border Patrol Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 4),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 7),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 8),
//                        new PedComponent(5, 61, 6),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male NOOSE SEP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 15),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male NOOSE SEP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 15),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male NOOSE SEP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 17),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 3),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 5),
//                        new PedComponent(5, 56, 4),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male NOOSE SEP Riot Gear>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 0),
//                        new PedPropComponent(1, 26, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 150, 6),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 125, 6),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female NOOSE SEP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 15),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 34, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female NOOSE SEP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 15),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 34, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female NOOSE SEP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 17),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 3),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 5),
//                        new PedComponent(5, 61, 4),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female NOOSE SEP Riot Gear>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 0),
//                        new PedPropComponent(1, 23, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 147, 6),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 131, 7),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 34, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SEP TRU Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 39, 1),
//                        new PedPropComponent(1, 26, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 220, 9),
//                        new PedComponent(3, 141, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 37, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SEP TRU Uniform>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 38, 1),
//                        new PedPropComponent(1, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 122, 0),
//                        new PedComponent(11, 230, 9),
//                        new PedComponent(3, 174, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 36, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 5),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male Juggernaut>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 93, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 0),
//                        new PedComponent(11, 186, 0),
//                        new PedComponent(3, 110, 3),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 97, 0),
//                        new PedComponent(4, 84, 0),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female Juggernaut>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 92, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 0),
//                        new PedComponent(11, 188, 0),
//                        new PedComponent(3, 127, 3),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 105, 0),
//                        new PedComponent(4, 86, 0),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 0),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 13, 0),
//                        new PedComponent(9, 43, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 3),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 39, 2),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Special Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 0),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 10, 4),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 43, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Special Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 27, 1),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 7, 0),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 2),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 47, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 43, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 2),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 7, 0),
//                        new PedComponent(4, 54, 3),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male PIA Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 325, 5),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 28, 3),
//                        new PedComponent(4, 47, 0),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 43, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female PIA Windbreaker>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 318, 5),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 2),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 45, 0),
//                        new PedComponent(5, 28, 3),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male IAA Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 242, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 10, 3),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 22, 8),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female IAA Field Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 14, 1),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 9, 0),
//                        new PedComponent(4, 3, 1),
//                        new PedComponent(6, 50, 0),
//                        new PedComponent(7, 98, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male IAA Agent>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 4),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 16, 0),
//                        new PedComponent(4, 10, 2),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 22, 8),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male IAA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 0),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 179, 1),
//                        new PedComponent(4, 10, 0),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 13, 0),
//                        new PedComponent(9, 22, 8),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female IAA Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 24, 1),
//                        new PedComponent(3, 7, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 104, 4),
//                        new PedComponent(4, 3, 1),
//                        new PedComponent(6, 50, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 24, 8),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USAF SF Fatigues>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 63, 0),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 21, 0),
//                        new PedComponent(8, 55, 0),
//                        new PedComponent(4, 87, 0),
//                        new PedComponent(6, 35, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male USAF Combat Fatigues>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 220, 13),
//                        new PedComponent(3, 154, 19),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 87, 0),
//                        new PedComponent(6, 35, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 7, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female USAF Police>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 56, 0),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 20, 0),
//                        new PedComponent(8, 32, 0),
//                        new PedComponent(4, 90, 0),
//                        new PedComponent(6, 36, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female USAF Combat Fatigues>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 28, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 230, 25),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 2, 0),
//                        new PedComponent(4, 90, 0),
//                        new PedComponent(6, 36, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 29, 0),
//                        new PedComponent(5, 20, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 154, 0),
//                        new PedComponent(4, 25, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 45, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 9),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 45, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 139, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 8),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 45, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Armed Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 20, 8),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Armed Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 9),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 20, 8),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 139, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 149, 1),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 86, 10),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 2),
//                        new PedComponent(5, 56, 7),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA Jumpsuit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 2),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 39, 0),
//                        new PedComponent(4, 87, 10),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 5),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male SASPA CRT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 125, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 0),
//                        new PedComponent(11, 150, 0),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 39, 0),
//                        new PedComponent(4, 125, 5),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 5, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 9),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 190, 0),
//                        new PedComponent(4, 41, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 9),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 9),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 138, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 8),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 37, 0),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Armed Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 9),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 23, 8),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Armed Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 9),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 23, 8),
//                        new PedComponent(5, 33, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 146, 1),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 51, 1),
//                        new PedComponent(4, 89, 10),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 2),
//                        new PedComponent(5, 61, 7),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA Jumpsuit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 2),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 3, 0),
//                        new PedComponent(4, 90, 10),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 5),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female SASPA CRT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 124, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 0),
//                        new PedComponent(11, 147, 0),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 3, 0),
//                        new PedComponent(4, 131, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 3, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 7),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 7),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 7),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 14),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 8),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 9),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 8),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 14, 0),
//                        new PedComponent(5, 65, 9),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA ESU Utility>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 65, 10),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 7),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 7),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 7),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 8),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 9),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 8),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 9),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA ESU Utility>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 9),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 10),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 14),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 52, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 7),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 31, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 175, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 7),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSIA Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 62, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 0),
//                        new PedComponent(4, 0, 12),
//                        new PedComponent(6, 1, 0),
//                        new PedComponent(7, 0, 0),
//                        new PedComponent(9, 40, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSIA Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 160, 0),
//                        new PedComponent(3, 1, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 172, 0),
//                        new PedComponent(4, 0, 0),
//                        new PedComponent(6, 49, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 42, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 8),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 8),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 8),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 13, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 15),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 10),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 8),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 10),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 8),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 8),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 8),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 8),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 10),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 8),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 10),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 8),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 15),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 8),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 32, 1),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 8),
//                        new PedComponent(3, 20, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 32, 1),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 8),
//                        new PedComponent(3, 26, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 32, 1),
//                        new PedComponent(6, 13, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Motorcycle Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 8),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Motorcycle Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 8),
//                        new PedComponent(3, 23, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Motorcycle Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 17, 5),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 8),
//                        new PedComponent(3, 28, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 34, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 135, 21),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 15),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 87, 0),
//                        new PedComponent(4, 87, 2),
//                        new PedComponent(6, 24, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 8, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Harbor Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 134, 21),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 15),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 65, 0),
//                        new PedComponent(4, 90, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 1, 0),
//                        new PedComponent(9, 9, 1),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 20, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 6),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 32, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 174, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 6),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 0),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male LSPP Plain Clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 2, 9),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 10, 5),
//                        new PedComponent(6, 3, 1),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 41, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female LSPP Plain clothes>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 14, 8),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 3, 1),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 43, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 200, 13),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 193, 13),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 190, 13),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 143, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 187, 8),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 311, 16),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Bicycle Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 49, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 311, 16),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 12, 2),
//                        new PedComponent(6, 2, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 209, 6),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 6),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 212, 6),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 6),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Suit>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 4, 0),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 178, 13),
//                        new PedComponent(4, 22, 0),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 38, 2),
//                        new PedComponent(9, 38, 0),
//                        new PedComponent(9, 38, 0),
//                        new PedComponent(5, 68, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Detective>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 349, 13),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 88, 0),
//                        new PedComponent(4, 22, 0),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 38, 0),
//                        new PedComponent(5, 10, 2),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 348, 13),
//                        new PedComponent(3, 12, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 0),
//                        new PedComponent(4, 22, 0),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 38, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male RHPD SWAT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 52, 4),
//                        new PedComponent(11, 220, 11),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 6),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 202, 13),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 195, 13),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 192, 13),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 72, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 6),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 65, 6),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 6),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 65, 6),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 335, 16),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 140, 0),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 189, 8),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Bicycle Patrol>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 47, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 101, 0),
//                        new PedComponent(11, 335, 16),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 14, 2),
//                        new PedComponent(6, 10, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Detective>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 1, 14),
//                        new PedComponent(3, 1, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 171, 18),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 40, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD Detective (vest)>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 281, 18),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 161, 0),
//                        new PedComponent(4, 3, 4),
//                        new PedComponent(6, 29, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 40, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female RHPD SWAT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 1),
//                        new PedPropComponent(1, 22, 0),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 230, 11),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 6),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 200, 14),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 193, 14),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 190, 14),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 143, 5),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 187, 9),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 56, 1),
//                        new PedComponent(4, 35, 0),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 28, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 311, 17),
//                        new PedComponent(3, 0, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 12),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
            
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 209, 7),
//                        new PedComponent(3, 4, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 7),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 212, 7),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 94, 1),
//                        new PedComponent(4, 86, 2),
//                        new PedComponent(6, 51, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 37, 0),
//                        new PedComponent(5, 65, 7),
//                })
//            },
            
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD Armor Protection>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(6, 4, 2),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 292, 1),
//                        new PedComponent(3, 11, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 131, 0),
//                        new PedComponent(4, 22, 1),
//                        new PedComponent(6, 20, 0),
//                        new PedComponent(7, 6, 0),
//                        new PedComponent(9, 39, 0),
//                        new PedComponent(5, 0, 0),
//                })
//            },
//            new DispatchablePerson("mp_m_freemode_01", 0, 0)
//            {
//                DebugName = "<Male DPPD SWAT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_M_Y_COP_01_WHITE_FULL_01",
//                    "S_M_Y_COP_01_WHITE_FULL_02",
//                    "S_M_Y_COP_01_BLACK_FULL_01",
//                    "S_M_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 150, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 38, 0),
//                        new PedComponent(11, 220, 10),
//                        new PedComponent(3, 179, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 15, 0),
//                        new PedComponent(4, 31, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 110, 0),
//                        new PedComponent(9, 25, 7),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Class A>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 202, 14),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 195, 14),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 192, 14),
//                        new PedComponent(3, 9, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 0, 0),
//                        new PedComponent(5, 73, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Utility Class B>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 225, 7),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 7),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Utility Class C>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 226, 7),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 2),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 15, 0),
//                        new PedComponent(5, 65, 7),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Polo>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 335, 17),
//                        new PedComponent(3, 14, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 101, 1),
//                        new PedComponent(4, 89, 12),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 16, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Jacket>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 140, 5),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD Raincoat>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                { }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 0, 0),
//                        new PedComponent(11, 189, 9),
//                        new PedComponent(3, 3, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 33, 1),
//                        new PedComponent(4, 34, 0),
//                        new PedComponent(6, 52, 0),
//                        new PedComponent(7, 8, 0),
//                        new PedComponent(9, 30, 0),
//                        new PedComponent(5, 48, 0),
//                })
//            },
            
//            new DispatchablePerson("mp_f_freemode_01", 0, 0)
//            {
//                DebugName = "<Female DPPD SWAT>",
//                RandomizeHead = true,
//                OverrideVoice = new List<string>()
//                {
//                    "S_F_Y_COP_01_WHITE_FULL_01",
//                    "S_F_Y_COP_01_WHITE_FULL_02",
//                    "S_F_Y_COP_01_BLACK_FULL_01",
//                    "S_F_Y_COP_01_BLACK_FULL_02"
//                },
//                RequiredVariation = new PedVariation(new List<PedPropComponent>()
//                {
//                    new PedPropComponent(0, 149, 1),
//                }, new List<PedComponent>()
//                {
//                    new PedComponent(1, 38, 0),
//                        new PedComponent(11, 230, 10),
//                        new PedComponent(3, 215, 0),
//                        new PedComponent(10, 0, 0),
//                        new PedComponent(8, 14, 0),
//                        new PedComponent(4, 30, 1),
//                        new PedComponent(6, 25, 0),
//                        new PedComponent(7, 81, 0),
//                        new PedComponent(9, 27, 7),
//                        new PedComponent(5, 48, 0),
//                })
//            },
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            


            










//        };
//    }
//}

