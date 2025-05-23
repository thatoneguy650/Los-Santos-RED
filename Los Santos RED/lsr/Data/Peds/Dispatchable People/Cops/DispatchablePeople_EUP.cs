﻿using LosSantosRED.lsr.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchablePeople_EUP
{
    private DispatchablePeople DispatchablePeople;
    private int optionalpropschance;
    private List<PedPropComponent> MaleCopOptionalProps;
    private List<PedPropComponent> FemaleCopOptionalProps;
    private List<PedPropComponent> MaleCopShortSleeveOptionalProps;
    private List<PedPropComponent> FemaleCopShortSleeveOptionalProps;
    private List<PedPropComponent> MaleSheriffOptionalProps;
    private List<PedPropComponent> MaleSheriffShortSleeveOptionalProps;
    private List<PedPropComponent> FemaleSheriffOptionalProps;
    private List<PedPropComponent> FemaleSheriffShortSleeveOptionalProps;
    private List<PedPropComponent> MaleBCSheriffOptionalProps;
    private List<PedPropComponent> MaleBCSheriffShortSleeveOptionalProps;
    private List<PedPropComponent> FemaleBCSheriffOptionalProps;
    private List<PedPropComponent> FemaleBCSheriffShortSleeveOptionalProps;
    private List<DispatchablePerson> CoastGuard_FEJ;
    private List<DispatchablePerson> LSFD_Fire_FEJ;
    private List<DispatchablePerson> LSFD_EMT_FEJ;
    private List<DispatchablePerson> LSCoFD_Fire_FEJ;
    private List<DispatchablePerson> LSCoFD_EMT_FEJ;
    private List<DispatchablePerson> BCFD_Fire_FEJ;
    private List<DispatchablePerson> BCFD_EMT_FEJ;
    private List<DispatchablePerson> SanFire_FEJ;
    private List<DispatchablePerson> Lifeguards_FEJ;
    private List<DispatchablePerson> SanAndreasMedicalServices_FEJ;
    private List<DispatchablePerson> MerryweatherSecurity_FEJ;
    private List<DispatchablePerson> GruppeSechSecurity_FEJ;
    private List<DispatchablePerson> BobcatSecurity_FEJ;
    private List<DispatchablePerson> SecuroServ_FEJ;
    private List<DispatchablePerson> LockNLoadSecurity_FEJ;
    private List<DispatchablePerson> ChuffSecurity_FEJ;
    private List<DispatchablePerson> USNPSParkRangers_FEJ;
    private List<DispatchablePerson> GameWarden_FEJ;
    private List<DispatchablePerson> LSDRP1_FEJ;
    private List<DispatchablePerson> NYSP_FEJ;
    private List<DispatchablePerson> LSSDASDPeds_FEJ;
    private List<DispatchablePerson> LSPDASDPeds_FEJ;
    private List<DispatchablePerson> LSPPPeds_FEJ;
    private List<DispatchablePerson> PrisonPeds_FEJ;
    private List<DispatchablePerson> SAHPPeds_FEJ;
    private List<DispatchablePerson> DOAPeds_FEJ;
    private List<DispatchablePerson> SanAndreasStateParksRangers_FEJ;
    private List<DispatchablePerson> FIBPeds_FEJ;
    private List<DispatchablePerson> MarshalsServicePeds_FEJ;
    private List<DispatchablePerson> BorderPatrolPeds_FEJ;
    private List<DispatchablePerson> NOOSESEPPeds_FEJ;
    private List<DispatchablePerson> NOOSEPIAPeds_FEJ;
    private List<DispatchablePerson> BCSheriffPeds_FEJ;
    private List<DispatchablePerson> SheriffPeds_FEJ;
    private List<DispatchablePerson> DPPDCops_FEJ;
    private List<DispatchablePerson> RHPDCops_FEJ;
    private List<DispatchablePerson> LSIAPDPeds_FEJ;
    private List<DispatchablePerson> StandardCops_FEJ;
    private int swatAccuracyMin = 25;
    private int swatAccuracyMax = 30;
    private int swatShootRateMin = 400;
    private int swatShootRateMax = 500;
    private int swatCombatAbilityMin = 0;
    private int swatCombatAbilityMax = 2;
    private int swatHealthMin = 100;
    private int swatHealthMax = 100;
    private int swatArmorMin = 100;
    private int swatArmorMax = 100;


    private int fibAccuracyMin = 30;
    private int fibAccuracyMax = 40;
    private int fibShootRateMin = 400;
    private int fibShootRateMax = 500;
    private int fibCombatAbilityMin = 2;
    private int fibCombatAbilityMax = 2;
    private int fibHealthMin = 100;
    private int fibHealthMax = 100;
    private int fibArmorMin = 100;
    private int fibArmorMax = 100;

    private int sniperAccuracyMin = 65;
    private int sniperAccuracyMax = 85;

    private int nooseAccuracyMin = 25;
    private int nooseAccuracyMax = 40;
    private int nooseShootRateMin = 400;
    private int nooseShootRateMax = 500;
    private int nooseCombatAbilityMin = 1;
    private int nooseCombatAbilityMax = 2;
    private int nooseHealthMin = 100;
    private int nooseHealthMax = 100;
    private int nooseArmorMin = 100;
    private int nooseArmorMax = 100;



    public DispatchablePeople_EUP(DispatchablePeople dispatchablePeople)
    {
        DispatchablePeople = dispatchablePeople;
    }
    public void DefaultConfig()
    {
        List<DispatchablePersonGroup> PeopleConfig_EUP = new List<DispatchablePersonGroup>();
        SetupOptionalProps();

        SetupLSPD();//Swat, Pilot, Detective
        SetupLSIAPD();
        SetupRHPD();//Swat
        SetupDPPD();//Swat
        SetupLSSD();//Swat
        SetupBCSO();//Swat
        SetupNOOSEPIA();//TRU
        SetupNOOSESEP();//TRU
        SetupNOOSEBP();
        SetupUSMS();//Armored
        SetupFIB();//Swat
        SetupSASPR();
        SetupDOA();//Armored
        SetupSAHP();//SWAT
        SetupSASPA();
        SetupLSPP();
        SetupLSPDASD();//Swat
        SetupLSSDASD();//Swat
        SetupNYSP();
        SetupLSDRP();
        SetupGameWarden();
        SetupUSNPS();
        SetupFireEMS();
        SetupUSCG();//Swat
        SetupSecurity();

        //Cops
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("StandardCops", StandardCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSIAPDPeds", LSIAPDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("RHPDCops", RHPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DPPDCops", DPPDCops_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SheriffPeds", SheriffPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCSheriffPeds", BCSheriffPeds_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSEPIAPeds", NOOSEPIAPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NOOSESEPPeds", NOOSESEPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BorderPatrolPeds", BorderPatrolPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MarshalsServicePeds", MarshalsServicePeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("OffDutyCops", DispatchablePeople.OffDutyCops));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("FIBPeds", FIBPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkRangers", SanAndreasStateParksRangers_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USNPSParkRangers", USNPSParkRangers_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSDPRParkRangers", LSDRP1_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SADFWParkRangers", GameWarden_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("DOAPeds", DOAPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAHPPeds", SAHPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ArmyPeds", DispatchablePeople.ArmyPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USMCPeds", DispatchablePeople.USMCPeds));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("USAFPeds", DispatchablePeople.USAFPeds));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("PrisonPeds", PrisonPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NYSPPeds", NYSP_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Firefighters", DispatchablePeople.Firefighters));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BlueEMTs", DispatchablePeople.BlueEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GreenEMTs", DispatchablePeople.GreenEMTs));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPPPeds", LSPPPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSPDASDPeds", LSPDASDPeds_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSSDASDPeds", LSSDASDPeds_FEJ));

        ////Gangs
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("LostMCPeds", DispatchablePeople.LostMCPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("VagosPeds", DispatchablePeople.VagosPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("DiablosPeds", DispatchablePeople.DiablosPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("FamiliesPeds", DispatchablePeople.FamiliesPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("BallasPeds", DispatchablePeople.BallasPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("MarabuntaPeds", DispatchablePeople.MarabuntaPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("AltruistPeds", DispatchablePeople.AltruistPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("VarriosPeds", DispatchablePeople.VarriosPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("TriadsPeds", DispatchablePeople.TriadsPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("KoreanPeds", DispatchablePeople.KoreanPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("RedneckPeds", DispatchablePeople.RedneckPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("ArmenianPeds", DispatchablePeople.ArmenianPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("CartelPeds", DispatchablePeople.CartelPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("MafiaPeds", DispatchablePeople.MafiaPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("YardiesPeds", DispatchablePeople.YardiesPeds));

        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("AngelsOfDeathPeds", DispatchablePeople.AngelsOfDeathPeds_LS));

        ////Other
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("OtherPeds", DispatchablePeople.OtherPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("TaxiDrivers", DispatchablePeople.TaxiDrivers));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("RegularPeds", DispatchablePeople.RegularPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("VehicleRacePeds", DispatchablePeople.VehicleRacePeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("VendorPeds", DispatchablePeople.VendorPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("IllicitMarketplacePeds", DispatchablePeople.IllicitMarketplacePeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("TellerPeds", DispatchablePeople.TellerPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("BurgerShotPeds", DispatchablePeople.BurgerShotPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("CluckinBellPeds", DispatchablePeople.CluckinBellPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("TwatPeds", DispatchablePeople.TwatPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("GunshopPeds", DispatchablePeople.GunshopPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("BarPeds", DispatchablePeople.BarPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("HaircutPeds", DispatchablePeople.HaircutPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("BobMuletPeds", DispatchablePeople.BobMuletPeds));
        //PeopleConfig_EUP.Add(new DispatchablePersonGroup("WeazelPeds", DispatchablePeople.WeazelPeds));

        //NEW
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSFDPeds", LSFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSFDEMTPeds", LSFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSCOFDPeds", LSCoFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSCOFDEMTPeds", LSCoFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCFDPeds", BCFD_Fire_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BCFDEMTPeds", BCFD_EMT_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SAMSEMTPeds", SanAndreasMedicalServices_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SanFirePeds", SanFire_FEJ));

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LSLifeguardPeds", Lifeguards_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("CoastGuardPeds", CoastGuard_FEJ));

        //Security
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecurityPeds", DispatchablePeople.SecurityPeds));//used for lsspp in generic
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GruppeSechsPeds", GruppeSechSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MerryweatherSecurityPeds", MerryweatherSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("BobcatPeds", BobcatSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SecuroservPeds", SecuroServ_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("LNLPeds", LockNLoadSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ChuffPeds", ChuffSecurity_FEJ));

        DefaultConfigEXTRA(PeopleConfig_EUP);

        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\DispatchablePeople+_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(PeopleConfig_EUP, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchablePeople+_EUP.xml");
    }
    private void SetupUSCG()
    {
        CoastGuard_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Fatigues>",//just plain grey.blue BDU
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Fatigues>",//just plain grey.blue BDU
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 3),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 54, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 50, 50)
            {
                DebugName = "<Male U.S Coast Guard>",//BDU LifeVest anbd Helmet, boater
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 0),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 41, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 46, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 50, 50)
            {
                DebugName = "<Female U.S Coast Guard>",//BDU LifeVest anbd Helmet, boater
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 38, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 54, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 48, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Swat>",
                RandomizeHead = true

                ,AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,

                GroupName = "SWAT",


                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 150, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 220, 12),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 16, 1),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG SWAT>",
                RandomizeHead = true,

                 AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                 GroupName = "SWAT",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 149, 0),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 12),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 18, 1),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Onepiece>",//Orange One Piece SUit without flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 0),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 155, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Onepiece>",//Orange One Piece SUit without flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 0),
                        new PedPropComponent(1, 27, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 152, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Swimmer>",//Orange One Piece SUit with flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 1),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 155, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 0),
                        new PedComponent(6, 67, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Swimmer>",//Orange One Piece SUit with flippers
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 1),
                        new PedPropComponent(1, 27, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 152, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 0),
                        new PedComponent(6, 70, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Jumpsuit>",//Looks like a pilot outfit
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 10),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 38, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Jumpsuit>",//Looks like a pilot outfit
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 10),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 38, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USCG Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 155, 2),
                        new PedComponent(3, 16, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 70, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USCG Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 152, 2),
                        new PedComponent(3, 17, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 72, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

    }
    private void SetupFireEMS()
    {
        //NEW EUP ADDON STUFF
        LSFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male LSFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 21),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 15, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female LSFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 15, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 6),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 7),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 6),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 7),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 7),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 57, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 21),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 1),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 57, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 65, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 78, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 64, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSFD Pilot>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 0),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        LSFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 0),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 0),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 0),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 0),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 57, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };


        LSCoFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male LSCoFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 1),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female LSCoFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 3),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 1),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 16, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 2),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSCoFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSCoFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 1),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 58, 1),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 66, 1),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 4),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 8, 4),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 4),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 9, 4),
                        new PedComponent(5, 48, 0),
                })
            },
        };

        LSCoFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSCoFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 3),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male LSCoFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 3),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 1),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSCoFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 2),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female LSCoFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 2),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 37, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSCoFD EMT Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSCoFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        BCFD_Fire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 18, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 315, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 18, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 100, 100)
            {
                DebugName = "<Male BCFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 45, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 314, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 18, 0),
                        new PedComponent(8, 151, 0),
                        new PedComponent(4, 120, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Bunker Pants>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 12, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 326, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 100, 100)
            {
                DebugName = "<Female BCFD PPE Gear>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 44, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 17, 0),
                        new PedComponent(8, 187, 0),
                        new PedComponent(4, 126, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 118, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 5),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 5),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 1),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 0),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 58, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCFD Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 0),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 66, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male BCFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 2),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female BCFD Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 2),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 5),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 8, 3),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCFD Water Rescue>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 5),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 9, 3),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        BCFD_EMT_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male BCFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 4),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male BCFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 4),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female BCFD EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 4),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female BCFD EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 4),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 2),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female BCFD EMT T-Shirt>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 23),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        SanFire_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Sanfire Brush Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 80, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 250, 0),
                        new PedComponent(3, 96, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 86, 23),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Brush Turnouts>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 79, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 258, 0),
                        new PedComponent(3, 111, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 89, 23),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire Fireman Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 7),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire LEO Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 75, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SanFire LEO Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 74, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 11),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 38, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire LEO Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 26, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 25),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanfire LEO Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 25, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 11),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 25),
                })
            },
        };
        Lifeguards_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male Lifeguard Bathsuit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 15, 0),
                        new PedComponent(3, 15, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female Lifeguard Swimsuit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 11, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 18, 0),
                        new PedComponent(4, 17, 4),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 25, 25)
            {
                DebugName = "<Male Lifeguard Clothes>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 22, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 41, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 15, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 25, 25)
            {
                DebugName = "<Female Lifeguard Clothes>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 23, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male ATV Patrol>",
                RandomizeHead = true,
                GroupName = "ATV",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 22, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 41, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 15, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female ATV Patrol>",
                RandomizeHead = true,
                GroupName = "ATV",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 23, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 46, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Nautical Patrol>",
                RandomizeHead = true,
                GroupName = "Boat",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 15, 0),
                        new PedComponent(3, 15, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 6, 3),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Nautical Patrol>",
                RandomizeHead = true,
                GroupName = "Boat",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 11, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 18, 0),
                        new PedComponent(4, 10, 3),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 19, 0),
                        new PedComponent(9, 47, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        SanAndreasMedicalServices_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 6),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 6),
                        new PedComponent(3, 92, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male SAMS Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 24),
                        new PedComponent(3, 85, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 30, 0),
                        new PedComponent(9, 29, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 6),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female SAMS EMT Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 6),
                        new PedComponent(3, 109, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 14, 1),
                        new PedComponent(9, 33, 0),
                        new PedComponent(5, 71, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male SAMS Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 249, 0),
                        new PedComponent(3, 31, 0),
                        new PedComponent(10, 59, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 126, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female SAMS Winter Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 5, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 257, 0),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 67, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 96, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male SAMS Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 101, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female SAMS Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 6, 0),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 1),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male CLSMD Scrubs>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 2),
                        new PedComponent(11, 32, 3),
                        new PedComponent(3, 85, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 45, 5),
                        new PedComponent(6, 42, 2),
                        new PedComponent(7, 127, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female CLSMD Scrubs>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 2),
                        new PedComponent(11, 31, 3),
                        new PedComponent(3, 109, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 47, 5),
                        new PedComponent(6, 10, 1),
                        new PedComponent(7, 97, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
    }
    private void SetupSecurity()
    {
        MerryweatherSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 50, 50)
            {
                DebugName = "<Male Merryweather Security Guard>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 50, 50)
            {
                DebugName = "<Female Merryweather Security Guard>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
        GruppeSechSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 41, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Gruppe Sechs Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 0),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 40, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Gruppe Sechs Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 0),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //Armored
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Divison Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 41, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 75, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Divison Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 1),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 73, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Division Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 1),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 73, 0),
                        new PedComponent(8, 20, 3),
                        new PedComponent(4, 25, 6),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 40, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 84, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 66, 2),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 1),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 82, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Divison Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 1),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 82, 0),
                        new PedComponent(8, 19, 3),
                        new PedComponent(4, 3, 15),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

        };
        BobcatSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 13),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Bobcat Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 13),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Armored Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 4),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 0),
                        new PedComponent(4, 48, 2),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class A>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 11),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 13),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 13),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 23),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Bobcat Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 3, 12),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Bobcat Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 5),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 4),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 0),
                        new PedComponent(4, 3, 12),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 2),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
        SecuroServ_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class A>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class B>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Securoserv Class C>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 0),
                        new PedComponent(4, 25, 3),
                        new PedComponent(6, 97, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class A>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class B>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Securoserv Class C>",
                RandomizeHead = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
                GroupName = "UnarmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 3, 0),
                        new PedComponent(4, 41, 3),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 75, 0),
                })
            },
        };
        LockNLoadSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male L&L Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male L&L Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male L&L Armored Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 2),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male L&L Armored Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 2),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female L&L Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female L&L Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 15),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 22),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored L&L Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 2),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored L&L Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 2),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 4),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
        ChuffSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Chuff Security Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 14),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 15, 15)
            {
                DebugName = "<Male Chuff Security Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Chuff Armored Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 14),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 1),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Chuff Armored Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 20, 1),
                        new PedComponent(4, 48, 3),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Chuff Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 14),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15)
            {
                DebugName = "<Female Chuff Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 14),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 24),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Chuff Class B>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 14),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 1),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Armored Chuff Class C>",
                RandomizeHead = true,
                GroupName = "ArmedSecurity",
                ArmorMin = 50,
                ArmorMax = 50,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 14),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 19, 1),
                        new PedComponent(4, 3, 11),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 2, 3),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
    }
    private void SetupUSNPS()
    {
        USNPSParkRangers_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 19),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LE Park Ranger Polo>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 11),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 12),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger K-9>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 11),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 65, 12),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 7),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 78, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LE Park Ranger Raincoat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 4),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 19),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 79, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LE Ranger Polo>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 12),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LE Park Ranger K-9>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 11),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 12),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LE Park Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 7),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 78, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //NON LE BELOW
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 19),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 19),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 19),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Park Ranger Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 19),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 30, 0),
                })
            },
        };
    }
    private void SetupGameWarden()
    {
        GameWarden_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 18),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 18),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 18),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 11),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male Game Warden Utility>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 13),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male Game Warden Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 15),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male Game Warden Raincoat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 135, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 4),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 18),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 18),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 18),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 80, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female Game Warden Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 6),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 15),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Utility>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 12),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 13),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female Game Warden Polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 11),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
    }
    private void SetupLSDRP()
    {
        LSDRP1_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 4, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 20),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 20),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 5, 5)
            {
                DebugName = "<Male LSDRP Ranger Class C>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 20),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male LSRDP Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 16),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 20),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 20),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 5, 5)
            {
                DebugName = "<Female LSDRP Ranger Class C>",
                RandomizeHead = true,
                GroupName = "OffRoad",
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 20),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 27, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 70, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 1, 0)
            {
                DebugName = "<Female LSDRP Ranger Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 10),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 8),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 16),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };//only this is filled out in the base one
    }
    private void SetupNYSP()
    {
        NYSP_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("s_m_m_snowcop_01",0,0)  { DebugName = "NYSPDefaultMale" },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 13, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 16),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 17),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male NYSP Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 17),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 265, 5),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 13),
                        new PedComponent(8, 44, 1),
                        new PedComponent(4, 25, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 64, 12),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 30, 30)
            {
                DebugName = "<Male NYSP Parka>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 16, 0),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 33, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 26, 5),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 13, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 16),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 17),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female NYSP Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 17),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 76, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 5),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 52, 1),
                        new PedComponent(4, 3, 13),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 12),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30)
            {
                DebugName = "<Female NYSP Parka>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 8, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 44, 0),
                        new PedComponent(3, 49, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 32, 0),
                        new PedComponent(6, 53, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 0, 0),
                })
            },
        };
    }
    private void SetupLSSDASD()
    {
        LSSDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Pilot",DebugName = "<Male LSSD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,79,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,108,1),
          new PedComponent(3,96,0),
          new PedComponent(10,0,0),
          new PedComponent(8,43,1),
          new PedComponent(4,64,1),
          new PedComponent(6,24,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5)  { GroupName = "Pilot",DebugName = "<Female LSSD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,78,0),
          new PedPropComponent(1,13,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,99,1),
          new PedComponent(3,36,0),
          new PedComponent(10,0,0),
          new PedComponent(8,30,0),
          new PedComponent(4,66,1),
          new PedComponent(6,24,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            // Swat
            new DispatchablePerson("mp_m_freemode_01", 40, 40){

                DebugName = "<Male LSSD SWAT Uniform ASD>",
                RandomizeHead = true


                ,AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,


                GroupName = "SWAT",

                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 150, 1),
                        new PedPropComponent(1, 23, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 2),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 1),
                        new PedComponent(5, 48, 0),
                    }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)},
            new DispatchablePerson("mp_f_freemode_01", 40, 40){
                DebugName = "<Female LSSD SWAT Uniform ASD>",
                RandomizeHead = true
                ,AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,

                GroupName = "SWAT",

                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 2),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 1),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)},
        };
    }
    private void SetupLSPDASD()
    {
        LSPDASDPeds_FEJ = new List<DispatchablePerson>()
        {
            //Pilot
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Pilot", DebugName = "<Male LSPD Pilot Uniform>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,79,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,108,0),
            new PedComponent(3,16,0),
            new PedComponent(10,0,0),
            new PedComponent(8,67,0),
            new PedComponent(4,64,0),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,48,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Pilot", DebugName = "<Female LSPD Pilot Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,78,1),
            new PedPropComponent(1,13,0),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,99,0),
            new PedComponent(3,17,0),
            new PedComponent(10,0,0),
            new PedComponent(8,49,0),
            new PedComponent(4,66,0),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,48,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            //LSPD SWAT
            new DispatchablePerson("mp_m_freemode_01",40,40) {
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                GroupName = "SWAT",
                ShrinkHeadForMask = true,

                DebugName = "<Male LSPD SWAT Uniform ASD>",
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 179, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 16, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,150,0),new PedPropComponent(1,21,0)  }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)
            },
            new DispatchablePerson("mp_f_freemode_01",40,40) {
                DebugName = "<Female LSPD SWAT Uniform ASD>",
                RandomizeHead = true,

                                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,

                GroupName = "SWAT",
                ShrinkHeadForMask = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 215, 0, 0),
                        new PedComponent(4, 30, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 18, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,149,0),new PedPropComponent(1,22,0)  }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)
             },
        };
    }
    private void SetupLSPP()
    {
        LSPPPeds_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
            DebugName = "<Male LSPP Class A>", RandomizeHead = true,
            OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                new List<PedPropComponent>(){

                },
                new List<PedComponent>(){
                    new PedComponent(1, 0, 0),
                    new PedComponent(11, 200, 8),
                    new PedComponent(3, 4, 0),
                    new PedComponent(10, 0, 0),
                    new PedComponent(8, 56, 1),
                    new PedComponent(4, 35, 0),
                    new PedComponent(6, 51, 0),
                    new PedComponent(7, 8, 0),
                    new PedComponent(9, 0, 0),
                    new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 20, 20){
                DebugName = "<Male LSPP Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 55, 55){
                DebugName = "<Male LSPP Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 8),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 135, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 8),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 5, 15){
                DebugName = "<Male LSPP Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 212, 10),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 8),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
                DebugName = "<Female LSPP Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 20, 20){
                DebugName = "<Female LSPP Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 55, 55){
                DebugName = "<Female LSPP Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 8),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 15){
                DebugName = "<Female LSPP Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 8),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 20){
                DebugName = "<Female LSPP Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 10),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 65, 8),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 15),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 8),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 8),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Motorcycle Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 8),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Motorcycle Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 17, 5),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 8),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Harbor Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 135, 21),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 87, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 8, 1),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Harbor Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 134, 21),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 15),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 90, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 9, 1),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)},
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                DebugName = "<Male LSPP Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 20, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                DebugName = "<Male LSPP Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 187, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 32, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                DebugName = "<Female LSPP Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 174, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                DebugName = "<Female LSPP Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 189, 6),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male LSPP Plain Clothes>", GroupName = "Detective", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 2, 9),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 10, 5),
                        new PedComponent(6, 3, 1),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 41, 0),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                DebugName = "<Female LSPP Plain clothes>",GroupName = "Detective",  RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 14, 8),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            //motorcycle
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
    {
        GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
        {
            new PedPropComponent(0, 17, 5),
        }, new List < PedComponent > ()
        {
            new PedComponent(1, 0, 0),
                new PedComponent(11, 200, 8),
                new PedComponent(3, 20, 0),
                new PedComponent(10, 0, 0),
                new PedComponent(8, 56, 1),
                new PedComponent(4, 32, 1),
                new PedComponent(6, 13, 0),
                new PedComponent(7, 8, 0),
                new PedComponent(9, 0, 0),
                new PedComponent(5, 32, 0),
        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
    },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 8),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSPP Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 8),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 32, 1),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 8),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSPP Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 5),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 8),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 32, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
    }
    private void SetupSASPA()
    {
        PrisonPeds_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",0,0),

            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,200,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,154,0),
            new PedComponent(4,25,2),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,193,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,190,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_m_freemode_01",15,15) { DebugName = "<Male SASPA Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,139,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,311,8),
            new PedComponent(3,0,0),
            new PedComponent(10,0,0),
            new PedComponent(8,45,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,48,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,202,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,190,0),
            new PedComponent(4,41,0),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,0,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,195,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,192,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,0,0),
            new PedComponent(9,14,0),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",15,15) {DebugName = "<Female SASPA Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,138,1),
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,335,8),
            new PedComponent(3,14,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,89,10),
            new PedComponent(6,25,0),
            new PedComponent(7,0,0),
            new PedComponent(9,16,0),
            new PedComponent(5,48,0),
            }) },

            //With Armor
            new DispatchablePerson("mp_m_freemode_01",0,25) { MinWantedLevelSpawn = 3,DebugName = "<Male SASPA Armed Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,193,9),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,8,0),
            new PedComponent(9,20,8),
            new PedComponent(5,48,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",0,25) { MinWantedLevelSpawn = 3,DebugName = "<Male SASPA Armed Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,190,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,37,0),
            new PedComponent(4,86,10),
            new PedComponent(6,51,0),
            new PedComponent(7,8,0),
            new PedComponent(9,20,8),
            new PedComponent(5,48,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,25) { MinWantedLevelSpawn = 3, DebugName = "<Female SASPA Armed Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,195,9),
            new PedComponent(3,3,0),
            new PedComponent(10,0,0),
            new PedComponent(8,51,1),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,8,0),
            new PedComponent(9,23,8),
            new PedComponent(5,33,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,25) { MinWantedLevelSpawn = 3, DebugName = "<Female SASPA Armed Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,0,0),
            new PedComponent(11,192,9),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,51,1),
            new PedComponent(4,89,10),
            new PedComponent(6,52,0),
            new PedComponent(7,8,0),
            new PedComponent(9,23,8),
            new PedComponent(5,33,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

        };
    }
    private void SetupSAHP()
    {
        SAHPPeds_FEJ = new List<DispatchablePerson>() {

            //SAHP Class A
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,31,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,30,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Class B
            new DispatchablePerson("mp_m_freemode_01",25,25) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 14, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                } ,
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01",25,25) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Class C
            new DispatchablePerson("mp_m_freemode_01",55,55) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = MaleCopShortSleeveOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",55,55) { MaxWantedLevelSpawn = 3,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 4, 0)},
                    new List<PedPropComponent>() { }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Motorcycle Class A
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Motorcycle Class B
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Motorcycle Class C
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 32, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 13, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 2, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, GroupName = "MotorcycleCop",UnitCode = "Mary",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 55, 0, 0),
                        new PedComponent(6, 34, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 4, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,17,0) }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 55, 1, 0),//watches
                    new PedComponent(5, 55, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP Jacket
            new DispatchablePerson("mp_m_freemode_01",15,15) { MaxWantedLevelSpawn = 2,RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_HWAYCOP_01_WHITE_FULL_01", "S_M_Y_HWAYCOP_01_WHITE_FULL_02", "S_M_Y_HWAYCOP_01_BLACK_FULL_01", "S_M_Y_HWAYCOP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 15, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 53, 0, 0),
                        new PedComponent(9, 19, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 19, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,31,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 28, 2, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",15,15) { MaxWantedLevelSpawn = 2,RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 2, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 27, 1, 0),
                        new PedComponent(9, 31, 3, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 30, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,30,0)  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 30, 2, 0), //Ranks
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //SAHP SWAT
            new DispatchablePerson("mp_m_freemode_01",0,20) {
                RandomizeHead = true,MinWantedLevelSpawn = 4,



                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,

                OverrideAgencySideArms = true,
                OverrideSideArmsID = "TacticalSidearms",
                OverrideAgencyLongGuns = true,
                OverrideLongGunsID = "TacticalLongGuns",



                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 52, 3, 0),
                        new PedComponent(3, 141,18, 0),
                        new PedComponent(4, 37, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 35, 1, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 25, 2, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 7, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,39,1)  }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,20) {



                               AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,

                OverrideAgencySideArms = true,
                OverrideSideArmsID = "TacticalSidearms",
                OverrideAgencyLongGuns = true,
                OverrideLongGunsID = "TacticalLongGuns",

                RandomizeHead = true,MinWantedLevelSpawn = 4, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },GroupName = "StandardSAHP",
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 52, 3, 0),
                        new PedComponent(3, 174, 18, 0),
                        new PedComponent(4, 36, 1, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 27, 2, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 7, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,38,1),new PedPropComponent(1,25,0)  }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)
             },

            //Dirt Bike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male SAHP Dirtbike patrol>",
                RandomizeHead = true,
                MaxWantedLevelSpawn = 2,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,0),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,152,0),
                        new PedComponent(3,179,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,55,0),
                        new PedComponent(4,67,11),
                        new PedComponent(6,47,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,21,2),
                        new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female SAHP Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                MaxWantedLevelSpawn = 2,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                      new PedPropComponent(0,16,0),
                      new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>() {
                      new PedComponent(1,0,0),
                      new PedComponent(11,149,0),
                      new PedComponent(3,18,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,32,0),
                      new PedComponent(4,69,11),
                      new PedComponent(6,48,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,19,2),
                      new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },

            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male SAHP Pilot Uniform>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>() {



              new PedPropComponent(0,79,0),


              },
                    new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,108,3),
              new PedComponent(3,96,0),
              new PedComponent(10,0,0),
              new PedComponent(8,48,0),
              new PedComponent(4,64,2),
              new PedComponent(6,24,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female SAHP Pilot Uniform>",
                RandomizeHead = true,
                GroupName = "Pilot",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>() {



  new PedPropComponent(0,78,0),
  new PedPropComponent(1,13,0),

  },
                    new List<PedComponent>() {
  new PedComponent(1,0,0),
  new PedComponent(11,99,3),
  new PedComponent(3,36,0),
  new PedComponent(10,0,0),
  new PedComponent(8,29,0),
  new PedComponent(4,66,2),
  new PedComponent(6,24,0),
  new PedComponent(7,8,0),
  new PedComponent(9,0,0),
  new PedComponent(5,48,0),
   }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
    }
    private void SetupDOA()
    {
        DOAPeds_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",0,0),
            new DispatchablePerson("mp_m_freemode_01",10,5) { DebugName = "<Male DOA Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,37,2), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,4),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,88,0),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_f_freemode_01",15,5) {DebugName = "<Female DOA Agent>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,27,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,7,0),
              new PedComponent(4,3,2),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,50,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_m_freemode_01",0,50) { ArmorMin = 50,ArmorMax = 50,DebugName = "<Male DOA Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,37,2), }, new List<PedComponent>() {
              new PedComponent(1,121,0),
              new PedComponent(11,292,4),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,8),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_m_freemode_01",25,0) { DebugName = "<Male DOA Windbreaker>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,4),
              new PedComponent(3,12,0),
              new PedComponent(10,0,0),
              new PedComponent(8,179,4),
              new PedComponent(4,10,3),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,45,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_f_freemode_01",50,50) {DebugName = "<Female DOA Windbreaker>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,318,4),
              new PedComponent(3,7,0),
              new PedComponent(10,0,0),
              new PedComponent(8,39,0),
              new PedComponent(4,3,2),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,50,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
        };
    }
    private void SetupSASPR()
    {
        SanAndreasStateParksRangers_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_ranger_01",0,0) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 0, 0), new PedPropComponent(0, 1, 0),new PedPropComponent(1, 0, 0)}, OptionalPropChance = 70},
            new DispatchablePerson("s_f_y_ranger_01",0,0) { OptionalProps = new List<PedPropComponent>() { new PedPropComponent(1, 0, 0) }, OptionalPropChance = 70 },

            //Rangers
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,4,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,200,5),
          new PedComponent(3,4,0),
          new PedComponent(10,0,0),
          new PedComponent(8,49,1),
          new PedComponent(4,86,8),
          new PedComponent(6,51,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,36,0),
           }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,4,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,193,5),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,13,0),
              new PedComponent(5,36,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,190,5),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,13,0),
              new PedComponent(5,36,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,135,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,9),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male SASP Ranger Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
             new PedPropComponent(0,135,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,21,0),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,49,1),
              new PedComponent(4,86,8),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,28,4),
              new PedComponent(5,48,0),
             }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,4,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,202,5),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,195,5),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,192,5),
              new PedComponent(3,9,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,36,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,134,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,335,9),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,8),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female SASP Ranger Jacket>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,134,18),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,19,0),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,31,0),
              new PedComponent(4,89,10),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,30,4),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

        };
    }
    private void SetupFIB()
    {
        FIBPeds_FEJ = new List<DispatchablePerson>() {

            //Suits
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 2, DebugName = "<Male FIB Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,31,1), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,293,0),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,16,0),
              new PedComponent(4,10,0),
              new PedComponent(6,10,0),
              new PedComponent(7,38,8),
              new PedComponent(9,22,0),
              new PedComponent(5,28,0),
               }),
            FullHolster = new PedComponent(8,16,0),
            EmptyHolster = new PedComponent(8,18,0)  },
            new DispatchablePerson("mp_f_freemode_01",20,20) {MaxWantedLevelSpawn = 2, DebugName = "<Female FIB Agent>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,27,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,9,0),
              new PedComponent(4,3,0),
              new PedComponent(6,29,0),
              new PedComponent(7,0,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(8,9,0),
            EmptyHolster = new PedComponent(8,10,0)  },

            //Suits (with armor)
            new DispatchablePerson("mp_m_freemode_01",0,40) { ArmorMin = 50,ArmorMax = 50, MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Response>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,0),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,6),
              new PedComponent(4,10,0),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_f_freemode_01",0,40) { ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Female FIB Response>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,366,0),
              new PedComponent(3,7,0),
              new PedComponent(10,0,0),
              new PedComponent(8,161,6),
              new PedComponent(4,3,0),
              new PedComponent(6,29,0),
              new PedComponent(7,6,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },

            //Vanilla Like (male only)
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Field Agent>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,31,2), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,0),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,88,0),
              new PedComponent(4,47,0),
              new PedComponent(6,25,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_m_freemode_01",20,20) { MaxWantedLevelSpawn = 3, DebugName = "<Male FIB Field Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,3),
              new PedComponent(3,6,0),
              new PedComponent(10,0,0),
              new PedComponent(8,28,3),
              new PedComponent(4,47,0),
              new PedComponent(6,25,0),
              new PedComponent(7,6,0),
              new PedComponent(9,22,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },

            //SWAT
            new DispatchablePerson("mp_m_freemode_01",0,50) {
                AccuracyMin = fibAccuracyMin
                ,AccuracyMax = fibAccuracyMax
                ,ShootRateMin = fibShootRateMin
                ,ShootRateMax = fibShootRateMax
                ,CombatAbilityMin = fibCombatAbilityMin
                ,CombatAbilityMax = fibCombatAbilityMax
                ,HealthMin = fibHealthMin
                ,HealthMax = fibHealthMax
                ,ArmorMin = fibArmorMin
                ,ArmorMax = fibArmorMax,





                GroupName = "FIBHET",
                MinWantedLevelSpawn = 5,
                MaxWantedLevelSpawn = 5,
                DebugName = "<Male FIB SWAT Uniform>",//@SWAT@
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,39,0),
              new PedPropComponent(1,21,0),
              }, new List<PedComponent>() {
              new PedComponent(1,122,0),
              new PedComponent(11,220,5),
              new PedComponent(3,179,0),
              new PedComponent(10,0,0),
              new PedComponent(8,116,0),
              new PedComponent(4,31,4),
              new PedComponent(6,35,0),
              new PedComponent(7,110,0),
              new PedComponent(9,25,3),
              new PedComponent(5,48,0),
               }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0) },
            new DispatchablePerson("mp_f_freemode_01",0,50) {
                AccuracyMin = fibAccuracyMin
                ,AccuracyMax = fibAccuracyMax
                ,ShootRateMin = fibShootRateMin
                ,ShootRateMax = fibShootRateMax
                ,CombatAbilityMin = fibCombatAbilityMin
                ,CombatAbilityMax = fibCombatAbilityMax
                ,HealthMin = fibHealthMin
                ,HealthMax = fibHealthMax
                ,ArmorMin = fibArmorMin
                ,ArmorMax = fibArmorMax,
                GroupName = "FIBHET",
                MinWantedLevelSpawn = 5,
                MaxWantedLevelSpawn = 5,
                DebugName = "<Female FIB SWAT Uniform>",//@SWAT@
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,38,0),
              new PedPropComponent(1,22,0),
              }, new List<PedComponent>() {
              new PedComponent(1,122,0),
              new PedComponent(11,230,5),
              new PedComponent(3,215,0),
              new PedComponent(10,0,0),
              new PedComponent(8,14,0),
              new PedComponent(4,30,4),
              new PedComponent(6,36,0),
              new PedComponent(7,81,0),
              new PedComponent(9,27,3),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0) },

            new DispatchablePerson("mp_m_freemode_01",0,0) {
                ArmorMin = 50,
                ArmorMax = 50,
                MinWantedLevelSpawn = 5,
                MaxWantedLevelSpawn = 5,
                DebugName = "<Male FIB SWAT Uniform Sniper>",//@SNIPER@@SWAT@
                RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,39,0),
              new PedPropComponent(1,21,0),
              }, new List<PedComponent>() {
              new PedComponent(1,122,0),
              new PedComponent(11,220,5),
              new PedComponent(3,179,0),
              new PedComponent(10,0,0),
              new PedComponent(8,116,0),
              new PedComponent(4,31,4),
              new PedComponent(6,35,0),
              new PedComponent(7,110,0),
              new PedComponent(9,25,3),
              new PedComponent(5,48,0),
               }),
                FullHolster = new PedComponent(7,110,0),
                EmptyHolster = new PedComponent(7,109,0)
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,CombatAbilityMin = fibCombatAbilityMin
                ,CombatAbilityMax = fibCombatAbilityMax
                ,CombatRange = 3
                ,CombatMovement = 0
                ,AccuracyMin = sniperAccuracyMin
                ,AccuracyMax = sniperAccuracyMax
                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            },

            };
    }
    private void SetupUSMS()
    {
        MarshalsServicePeds_FEJ = new List<DispatchablePerson>()
        {
            //Suit
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male USMS Suit>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 4),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 13, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female USMS Suit>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 24, 6),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 64, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            //Regular
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male USMS Marshal>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female USMS Marshal>", MaxWantedLevelSpawn = 2, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 221, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 1),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            //Response - Has Armor
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Response>",MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 11),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 42, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Female USMS Response>",MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 332, 0),
                        new PedComponent(3, 1, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 161, 11),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 44, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            //Windbreaker has armor under
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Windbreaker>", MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 7),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 4),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 17, 7),
                        new PedComponent(5, 62, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            //Field Agent - Has Armor
            new DispatchablePerson("mp_m_freemode_01", 30, 30, 100, 100, 100, 100, 30, 50, 400, 500, 2, 2) {
                DebugName = "<Male USMS Field Agent>", MinWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 135, 14),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 7),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 133, 13),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 77, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
        };
    }
    private void SetupNOOSEBP()
    {
        BorderPatrolPeds_FEJ = new List<DispatchablePerson>()
        {
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 30, 30) {
                DebugName = "<Male Border Patrol Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 212, 3),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 30, 30) {
                DebugName = "<Female Border Patrol Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 3),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 15, 15) {
                DebugName = "<Male Border Patrol Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 209, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15) {
                DebugName = "<Female Border Patrol Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 225, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 10),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 65, 4),
                }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            //Jacket
            new DispatchablePerson("mp_m_freemode_01", 15, 15) {
                DebugName = "<Male Border Patrol Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 4),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 8),
                        new PedComponent(5, 56, 6),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 15, 15) {
                DebugName = "<Female Border Patrol Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 4),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 7),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 8),
                        new PedComponent(5, 61, 6),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
        };
    }
    private void SetupNOOSESEP()
    {
        NOOSESEPPeds_FEJ = new List<DispatchablePerson>()
        {
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 15),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Class B>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 15),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 10, 35) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 35) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Class C>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 15),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            //Jacket
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 135, 17),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 3),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 56, 4),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Jacket>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 134, 17),
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 3),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 5),
                        new PedComponent(5, 61, 4),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            //RIOT
            new DispatchablePerson("mp_m_freemode_01", 10, 10) {
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Male NOOSE SEP Riot Gear>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 150, 0),
                        new PedPropComponent(1, 26, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 150, 6),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 125, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 10, 10) {
                MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, ArmorMin = 50, ArmorMax = 50, CombatAbilityMin = 1, CombatAbilityMax = 2,
                DebugName = "<Female NOOSE SEP Riot Gear>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 149, 0),
                        new PedPropComponent(1, 23, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 147, 6),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 131, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 34, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            //TRU
            new DispatchablePerson("mp_m_freemode_01", 0, 50) {
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4,
                AccuracyMin = nooseAccuracyMin
                ,AccuracyMax = nooseAccuracyMax
                ,ShootRateMin = nooseShootRateMin
                ,ShootRateMax = nooseShootRateMax
                ,CombatAbilityMin = nooseCombatAbilityMin
                ,CombatAbilityMax = nooseCombatAbilityMax
                ,HealthMin = nooseHealthMin
                ,HealthMax = nooseHealthMax
                ,ArmorMin = nooseArmorMin
                ,ArmorMax = nooseArmorMax




                ,GroupName = "SWAT"
                ,DebugName = "<Male SEP TRU Uniform>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 39, 1),
                        new PedPropComponent(1, 26, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 220, 9),
                        new PedComponent(3, 141, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 37, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 5),
                        new PedComponent(5, 48, 0),
                }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 50) {
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4,
               AccuracyMin = nooseAccuracyMin
                ,AccuracyMax = nooseAccuracyMax
                ,ShootRateMin = nooseShootRateMin
                ,ShootRateMax = nooseShootRateMax
                ,CombatAbilityMin = nooseCombatAbilityMin
                ,CombatAbilityMax = nooseCombatAbilityMax
                ,HealthMin = nooseHealthMin
                ,HealthMax = nooseHealthMax
                ,ArmorMin = nooseArmorMin
                ,ArmorMax = nooseArmorMax
                ,GroupName = "SWAT"
                ,DebugName = "<Female SEP TRU Uniform>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 38, 1),
                        new PedPropComponent(1, 28, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 122, 0),
                        new PedComponent(11, 230, 9),
                        new PedComponent(3, 174, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 36, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 5),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)
            },
            //Juggernaut - ewwww
            new DispatchablePerson("mp_m_freemode_01", 0, 0) {
                HasFullBodyArmor = true
                ,DisableBulletRagdoll = true
                ,DisableCriticalHits = true
                ,FiringPatternHash = -957453492//fullauto
                ,HealthMin = 200
                ,HealthMax = 200
                ,ArmorMin = 500
                ,ArmorMax = 500
                ,AllowRandomizeBeforeVariationApplied = true
                ,OverrideAgencySideArms = true
                ,OverrideSideArmsID = "Minigun"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "Minigun"
                ,ShootRateMin = 500
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,MinWantedLevelSpawn = 4
                ,MaxWantedLevelSpawn = 4,
                DebugName = "<Male Juggernaut>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                    new PedPropComponent(0, 93, 0),

                }, new List < PedComponent > () {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 186, 0),
                        new PedComponent(3, 110, 3),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 97, 0),
                        new PedComponent(4, 84, 0),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0) {
                HasFullBodyArmor = true
                ,DisableBulletRagdoll = true
                ,DisableCriticalHits = true
                ,FiringPatternHash = -957453492//fullauto
                ,HealthMin = 200
                ,HealthMax = 200
                ,ArmorMin = 500
                ,ArmorMax = 500
                ,AllowRandomizeBeforeVariationApplied = true
                ,OverrideAgencySideArms = true
                ,OverrideSideArmsID = "Minigun"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "Minigun"
                ,ShootRateMin = 500
                ,ShootRateMax = 500
                ,CombatAbilityMin = 2
                ,CombatAbilityMax = 2
                ,MinWantedLevelSpawn = 4
                ,MaxWantedLevelSpawn = 4,
                DebugName = "<Female Juggernaut>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                    new PedPropComponent(0, 92, 0),
                }, new List < PedComponent > () {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 188, 0),
                        new PedComponent(3, 127, 3),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 105, 0),
                        new PedComponent(4, 86, 0),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0, 0) {

                DebugName = "<Male NOOSE SEP Class C Sniper>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 20, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            ,GroupName = "Sniper"
            ,OverrideAgencyLongGuns = true
            ,OverrideLongGunsID = "GoodSniperLongGuns"
            ,AlwaysHasLongGun = true
            ,CombatRange = 3
            ,CombatMovement = 0


            ,AccuracyMin = sniperAccuracyMin
                ,AccuracyMax = sniperAccuracyMax
                ,ShootRateMin = nooseShootRateMin
                ,ShootRateMax = nooseShootRateMax
                ,CombatAbilityMin = nooseCombatAbilityMin
                ,CombatAbilityMax = nooseCombatAbilityMax
                ,HealthMin = nooseHealthMin
                ,HealthMax = nooseHealthMax
                ,ArmorMin = nooseArmorMin
                ,ArmorMax = nooseArmorMax


            },
        };
    }
    private void SetupNOOSEPIA()
    {
        NOOSEPIAPeds_FEJ = new List<DispatchablePerson>() {
            //Class A
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 200, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class A>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            //Class B
            new DispatchablePerson("mp_m_freemode_01", 20, 20){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 87, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)},
            new DispatchablePerson("mp_f_freemode_01", 20, 20){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 10),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)},
            //Class C
            new DispatchablePerson("mp_m_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 10),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 13, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 10),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 35, 0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)},
            //Jacket
            new DispatchablePerson("mp_m_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Male PIA Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 149, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 37, 0),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 5),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 10, 10){
                MaxWantedLevelSpawn = 3, DebugName = "<Female PIA Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 146, 2),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 2, 0),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 5),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            //PIA TRU 
            new DispatchablePerson("mp_m_freemode_01",0,50) {
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4,
                AccuracyMin = nooseAccuracyMin
                ,AccuracyMax = nooseAccuracyMax
                ,ShootRateMin = nooseShootRateMin
                ,ShootRateMax = nooseShootRateMax
                ,CombatAbilityMin = nooseCombatAbilityMin
                ,CombatAbilityMax = nooseCombatAbilityMax
                ,HealthMin = nooseHealthMin
                ,HealthMax = nooseHealthMax
                ,ArmorMin = nooseArmorMin
                ,ArmorMax = nooseArmorMax,
                DebugName = "<Male PIA TRU Uniform>",GroupName = "SWAT", RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,39,1),
              new PedPropComponent(1,25,4),//new PedPropComponent(1,26,0),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),//new PedComponent(1,56,1),//new PedComponent(1,122,0),
              new PedComponent(11,220,8),
              new PedComponent(3,141,19),
              new PedComponent(10,0,0),
              new PedComponent(8,15,0),
              new PedComponent(4,37,2),
              new PedComponent(6,25,0),
              new PedComponent(7,110,0),
              new PedComponent(9,25,5),
              new PedComponent(5,48,0),
               }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0) },
            new DispatchablePerson("mp_f_freemode_01",0,50) {
                MinWantedLevelSpawn = 4,
                MaxWantedLevelSpawn = 4
                ,AccuracyMin = nooseAccuracyMin
                ,AccuracyMax = nooseAccuracyMax
                ,ShootRateMin = nooseShootRateMin
                ,ShootRateMax = nooseShootRateMax
                ,CombatAbilityMin = nooseCombatAbilityMin
                ,CombatAbilityMax = nooseCombatAbilityMax
                ,HealthMin = nooseHealthMin
                ,HealthMax = nooseHealthMax
                ,ArmorMin = nooseArmorMin
                ,ArmorMax = nooseArmorMax,
                GroupName = "SWAT",
                DebugName = "<Female PIA TRU Uniform>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(0,38,1),
              new PedPropComponent(1,27,2),
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),//new PedComponent(1,55,0),
              new PedComponent(11,230,8),
              new PedComponent(3,174,19),
              new PedComponent(10,0,0),
              new PedComponent(8,14,0),
              new PedComponent(4,36,2),
              new PedComponent(6,25,0),
              new PedComponent(7,81,0),
              new PedComponent(9,27,5),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0) },
            //Suit
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Suit>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 4),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 13, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Suit>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 24, 3),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 39, 2),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            //Special Agent
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Special Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 0),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Special Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 27, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            //Field Agent
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Field Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {

                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 47, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Field Agent>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 7, 0),
                        new PedComponent(4, 54, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            //Windbreaker
            new DispatchablePerson("mp_m_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Male PIA Windbreaker>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 325, 5),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 28, 3),
                        new PedComponent(4, 47, 0),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 43, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 2, 2) {
                MaxWantedLevelSpawn = 3,DebugName = "<Female PIA Windbreaker>", RandomizeHead = true, OverrideVoice = new List < string > () {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                }, RequiredVariation = new PedVariation(new List < PedPropComponent > () {
                }, new List < PedComponent > () {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 318, 5),
                        new PedComponent(3, 7, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 104, 2),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 45, 0),
                        new PedComponent(5, 28, 3),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },
        };
    }
    private void SetupBCSO()
    {
        BCSheriffPeds_FEJ = new List<DispatchablePerson>() {
            // BCSO Class A
            new DispatchablePerson("mp_m_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 200, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = MaleBCSheriffOptionalProps,
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 11, 1, 0),  // Ranks
                        new PedComponent(10, 11, 4, 0), new PedComponent(10, 45, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 15, 15){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 202, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalProps = FemaleBCSheriffOptionalProps,
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 10, 1, 0),  // Ranks
                        new PedComponent(10, 10, 4, 0), new PedComponent(10, 53, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // BCSO Class B
            new DispatchablePerson("mp_m_freemode_01", 25, 25){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 193, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 11, 1, 0),  // Ranks
                        new PedComponent(10, 11, 4, 0), new PedComponent(10, 45, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            new DispatchablePerson("mp_f_freemode_01", 25, 25){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 195, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 10, 1, 0),  // Ranks
                        new PedComponent(10, 10, 4, 0), new PedComponent(10, 53, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // BCSO Class C
            new DispatchablePerson("mp_m_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 22, 8, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 51, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 38, 0, 0), new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 190, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 15, 7, 0),  // Ranks
                        new PedComponent(10, 15, 8, 0), new PedComponent(10, 44, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 35, 35){
                MaxWantedLevelSpawn = 3, RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0, 0), new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 3, 7, 0), new PedComponent(5, 54, 0, 0),
                        new PedComponent(6, 52, 0, 0), new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 0, 0), new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0), new PedComponent(11, 192, 3, 0)},
                    new List<PedPropComponent>(){}),
                OptionalComponents =
                    new List<PedComponent>(){
                        new PedComponent(10, 14, 7, 0),  // Ranks
                        new PedComponent(10, 14, 8, 0), new PedComponent(10, 52, 7, 0),

                        new PedComponent(5, 54, 1, 0),  // watches
                        new PedComponent(5, 54, 2, 0),  // watches

                    },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // Detective (with armor (male))
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male BCSO Armor Protection>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(6, 32, 1),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 107, 0),
                        new PedComponent(11, 292, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 132, 5),
                        new PedComponent(4, 22, 5),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 119, 1),
                        new PedComponent(9, 23, 4),
                        new PedComponent(5, 29, 8),
                    }),
FullHolster = new PedComponent(7,119,1),
EmptyHolster = new PedComponent(7,120,1)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Female BCSO Suit>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 6, 15),
                                                         new PedComponent(3, 5, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 20, 0),
                                                         new PedComponent(4, 23, 5),
                                                         new PedComponent(6, 13, 0),
                                                         new PedComponent(7, 0, 0),
                                                         new PedComponent(9, 0, 0),
                                                         new PedComponent(5, 60, 0),
                                                     })},

            // SWAT
            new DispatchablePerson("mp_m_freemode_01", 0, 20){
                //AccuracyMin = 25
                //,AccuracyMax = 30
                //,ShootRateMin = 400
                //,ShootRateMax = 500
                //,CombatAbilityMin = 0
                //,CombatAbilityMax = 2
                //,HealthMin = 100
                //,HealthMax = 100
                //,ArmorMin = 100
                //,ArmorMax = 100


               AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax


                ,GroupName = "SWAT"
                ,MinWantedLevelSpawn = 4, DebugName = "<Male BCSO SWAT Uniform>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 150, 1),
                        new PedPropComponent(1, 21, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 3),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 0),
                        new PedComponent(5, 48, 0),
                    }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 20){
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax
                ,GroupName = "SWAT"
                ,MinWantedLevelSpawn = 4, DebugName = "<Female BCSO SWAT Uniform>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 3),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)},

            //Motorcycle
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
    {
        GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
        {
            new PedPropComponent(0, 17, 3),
        }, new List < PedComponent > ()
        {
            new PedComponent(1, 0, 0),
                new PedComponent(11, 200, 3),
                new PedComponent(3, 20, 0),
                new PedComponent(10, 0, 0),
                new PedComponent(8, 38, 0),
                new PedComponent(4, 32, 2),
                new PedComponent(6, 13, 0),
                new PedComponent(7, 8, 0),
                new PedComponent(9, 0, 0),
                new PedComponent(5, 54, 0),
        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
    },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 3),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 0),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male BCSO Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 3),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 0),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 3),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 3),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female BCSO Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 3),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 3),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 54, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //Dirtbike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male BCSO Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,152,0),
                      new PedComponent(3,179,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,55,0),
                      new PedComponent(4,67,10),
                      new PedComponent(6,47,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,18,1),
                      new PedComponent(5,48,0),
                    }
                ),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female BCSO Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,149,0),
                        new PedComponent(3,18,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,32,0),
                        new PedComponent(4,69,11),
                        new PedComponent(6,48,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,22,0),
                        new PedComponent(5,48,0),
                    }
                ),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) {
                DebugName = "<Male BCSO Bicycle Uniform>",
                RandomizeHead = true,
                               GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {



              new PedPropComponent(0,49,0),


              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,94,1),
              new PedComponent(3,19,0),
              new PedComponent(10,0,0),
              new PedComponent(8,37,0),
              new PedComponent(4,12,1),
              new PedComponent(6,2,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) {
                DebugName = "<Female BCSO Bicycle Uniform>",
                RandomizeHead = true,
                                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {

              new PedPropComponent(0,47,0),


              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,85,1),
              new PedComponent(3,31,0),
              new PedComponent(10,0,0),
              new PedComponent(8,2,0),
              new PedComponent(4,14,3),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,16,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Pilot
            new DispatchablePerson("s_m_m_pilot_02",0,0){ DebugName = "Generic Pilot", GroupName = "Pilot", RequiredVariation = new PedVariation() { Props = new List<PedPropComponent>() { new PedPropComponent(0,0,0) } } },
        };
    }
    private void SetupLSSD()
    {
        SheriffPeds_FEJ = new List<DispatchablePerson>() {

            new DispatchablePerson("s_m_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_sheriff_01",0,0) {MaxWantedLevelSpawn = 3 },

            //LSSD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(7,8,0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps = MaleSheriffOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 45, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps = FemaleSheriffOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 10, 3, 0),
                    new PedComponent(10, 53, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSSD Class B
            new DispatchablePerson("mp_m_freemode_01",25,25) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7,8,0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",25,25) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 10, 3, 0),
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSSD Class C
            new DispatchablePerson("mp_m_freemode_01",55,55) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 25, 0, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7,8,0),
                        new PedComponent(8, 38, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps = MaleSheriffShortSleeveOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 1, 0, 0), //Ranks
                    new PedComponent(10, 10, 2, 0),
                    new PedComponent(10, 2, 0, 0),
                    new PedComponent(10, 44, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",55,55) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 41, 1, 0),
                        new PedComponent(5, 53, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 51, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 2, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalProps = FemaleSheriffShortSleeveOptionalProps,
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 1, 0, 0), //Ranks
                    new PedComponent(10, 9, 0, 0),
                    new PedComponent(10, 2, 0, 0),
                    new PedComponent(10, 52, 6, 0),

                    new PedComponent(5, 53, 1, 0),//watches
                    new PedComponent(5, 53, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            // Jackets
            new DispatchablePerson("mp_m_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3, DebugName = "<Male LSSD Coat>",
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 265, 1),
                                                         new PedComponent(3, 4, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 38, 1),
                                                         new PedComponent(4, 25, 0),
                                                         new PedComponent(6, 51, 0),
                                                         new PedComponent(7, 8, 0),
                                                         new PedComponent(9, 14, 0),
                                                         new PedComponent(5, 64, 0),
                                                     }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 5, 5){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Coat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 5, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 274, 1),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 41, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 64, 1),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 30, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 25, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 1),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,DebugName = "<Female LSSD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 279, 0),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 51, 1),
                                                            new PedComponent(4, 41, 1),
                                                            new PedComponent(6, 52, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 30, 1),
                                                            new PedComponent(5, 48, 0),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 1, 0){
                MaxWantedLevelSpawn = 3,DebugName = "<Male LSSD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 187, 1),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 38, 1),
                                                            new PedComponent(4, 25, 0),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 28, 1),
                                                            new PedComponent(5, 53, 0),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // Polo
            new DispatchablePerson("mp_m_freemode_01", 15, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Male LSSD Polo>",
                OptionalProps = MaleCopShortSleeveOptionalProps,
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 135, 22),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 13),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 53, 0),
                        new PedComponent(4, 86, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 15, 3){
                MaxWantedLevelSpawn = 3, DebugName = "<Female LSSD Polo>",
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 134, 22),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 13),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 31, 1),
                        new PedComponent(4, 89, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // Swat
            new DispatchablePerson("mp_m_freemode_01", 0, 20){
                MinWantedLevelSpawn = 3
                ,AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                DebugName = "<Male LSSD SWAT Uniform>",
                GroupName = "SWAT",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 150, 1),
                        new PedPropComponent(1, 23, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 2),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 1),
                        new PedComponent(5, 48, 0),
                    }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 20){
                MinWantedLevelSpawn = 3
                ,AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                DebugName = "<Female LSSD SWAT Uniform>",
                GroupName = "SWAT",
                RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 2),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 1),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)},

            // Detective
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Armor Protection>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){
                        new PedPropComponent(6, 32, 1),
                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 292, 15),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 4),
                        new PedComponent(4, 22, 3),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 23, 1),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Raid Jacket>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 325, 1),
                                                            new PedComponent(3, 12, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 179, 15),
                                                            new PedComponent(4, 22, 3),
                                                            new PedComponent(6, 20, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 17, 3),
                                                            new PedComponent(5, 60, 1),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_m_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Male LSSD Gang Detail Jacket>", RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 325, 1),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 17),
                                                            new PedComponent(4, 4, 1),
                                                            new PedComponent(6, 63, 4),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 17, 3),
                                                            new PedComponent(5, 60, 1),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Armor Protection>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 366, 12),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 161, 4),
                                                            new PedComponent(4, 3, 4),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 25, 1),
                                                            new PedComponent(5, 0, 0),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Raid Jacket>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 318, 1),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 217, 12),
                                                            new PedComponent(4, 3, 6),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 21, 0),
                                                            new PedComponent(5, 60, 1),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Plain Clothes>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 73, 1),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 14, 0),
                                                            new PedComponent(4, 5, 9),
                                                            new PedComponent(6, 62, 23),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 25, 1),
                                                            new PedComponent(5, 68, 0),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 1, 1){
                MaxWantedLevelSpawn = 3,GroupName = "Detective", DebugName = "<Female LSSD Gang Detail Jacket>", RandomizeHead = true,
                OptionalProps = FemaleCopOptionalProps,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 318, 1),
                                                            new PedComponent(3, 7, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 57, 1),
                                                            new PedComponent(4, 5, 9),
                                                            new PedComponent(6, 29, 0),
                                                            new PedComponent(7, 6, 0),
                                                            new PedComponent(9, 21, 3),
                                                            new PedComponent(5, 60, 1),
                                                        }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},

            //Motor Units
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
                {
                    GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                    {
                        new PedPropComponent(0, 17, 2),
                    }, new List < PedComponent > ()
                    {
                        new PedComponent(1, 0, 0),
                            new PedComponent(11, 200, 2),
                            new PedComponent(3, 20, 0),
                            new PedComponent(10, 0, 0),
                            new PedComponent(8, 38, 1),
                            new PedComponent(4, 32, 2),
                            new PedComponent(6, 13, 0),
                            new PedComponent(7, 8, 0),
                            new PedComponent(9, 0, 0),
                            new PedComponent(5, 53, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
                },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 193, 2),
                        new PedComponent(3, 20, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Male LSSD Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_M_Y_SHERIFF_01_WHITE_FULL_01", "S_M_Y_SHERIFF_01_WHITE_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 190, 2),
                        new PedComponent(3, 26, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 38, 1),
                        new PedComponent(4, 32, 2),
                        new PedComponent(6, 13, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class A>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 202, 2),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class B>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 195, 2),
                        new PedComponent(3, 23, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                GroupName = "MotorcycleCop",UnitCode = "Mary",DebugName = "<Female LSSD Motorcycle Class C>", RandomizeHead = true, OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation(new List < PedPropComponent > ()
                {
                    new PedPropComponent(0, 17, 2),
                }, new List < PedComponent > ()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 192, 2),
                        new PedComponent(3, 28, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 51, 1),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 53, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //Dirtbike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male LSSD Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,152,1),
                      new PedComponent(3,179,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,55,0),
                      new PedComponent(4,67,10),
                      new PedComponent(6,47,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,18,1),
                      new PedComponent(5,48,0),
                    }
                ),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female LSSD Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation
                (
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,1),
                        new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,149,1),
                        new PedComponent(3,18,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,32,0),
                        new PedComponent(4,69,10),
                        new PedComponent(6,48,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,22,1),
                        new PedComponent(5,48,0),
                    }
                ),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male LSSD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,49,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,94,2),
                        new PedComponent(3,19,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,37,0),
                        new PedComponent(4,12,1),
                        new PedComponent(6,2,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,14,0),
                        new PedComponent(5,48,0),
                    }
                    ),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female LSSD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,47,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,85,2),
                        new PedComponent(3,31,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,2,0),
                        new PedComponent(4,14,3),
                        new PedComponent(6,10,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,16,0),
                        new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },


        };
    }
    private void SetupDPPD()
    {
        DPPDCops_FEJ = new List<DispatchablePerson>() {
            //DPPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //DPPD Class B
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //DPPD Class C
            new DispatchablePerson("mp_m_freemode_01",65,65) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",65,65) { MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 73, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 14, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            // Jacket
            new DispatchablePerson("mp_m_freemode_01", 15, 0){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                     new List<PedComponent>(){
                                                         new PedComponent(1, 0, 0),
                                                         new PedComponent(11, 143, 5),
                                                         new PedComponent(3, 4, 0),
                                                         new PedComponent(10, 0, 0),
                                                         new PedComponent(8, 56, 1),
                                                         new PedComponent(4, 35, 0),
                                                         new PedComponent(6, 51, 0),
                                                         new PedComponent(7, 8, 0),
                                                         new PedComponent(9, 28, 0),
                                                         new PedComponent(5, 0, 0),
                                                     }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 15, 0){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 140, 5),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 33, 1),
                                                            new PedComponent(4, 34, 0),
                                                            new PedComponent(6, 52, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 30, 0),
                                                            new PedComponent(5, 48, 0),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // Polo
            new DispatchablePerson("mp_m_freemode_01", 35, 5){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 311, 17),
                                                            new PedComponent(3, 0, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 12),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 48, 0),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 35, 5){
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 335, 17),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 12),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 16, 0),
                                                            new PedComponent(5, 48, 0),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            // Utility
            new DispatchablePerson("mp_m_freemode_01", 2, 25){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 209, 7),
                                                            new PedComponent(3, 4, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 2),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 65, 7),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 2, 25){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male DPPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 212, 7),
                                                            new PedComponent(3, 11, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 2),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 65, 7),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 2, 25){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 225, 7),
                                                            new PedComponent(3, 3, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 2),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 15, 0),
                                                            new PedComponent(5, 65, 7),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 2, 25){
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female DPPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 226, 7),
                                                            new PedComponent(3, 14, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 101, 1),
                                                            new PedComponent(4, 89, 2),
                                                            new PedComponent(6, 25, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 15, 0),
                                                            new PedComponent(5, 65, 7),
                                                        })},

            //SWAT
            new DispatchablePerson("mp_m_freemode_01", 0, 50){
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,GroupName = "SWAT",
                DebugName = "<Male DPPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 150, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 38, 0),
                        new PedComponent(11, 220, 10),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 7),
                        new PedComponent(5, 48, 0),
                    }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 50){
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,GroupName = "SWAT",
                DebugName = "<Female DPPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 149, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 38, 0),
                        new PedComponent(11, 230, 10),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 7),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)},

            //Bike
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male DPPD Bicycle Patrol>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 49, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 17),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 12, 2),
                        new PedComponent(6, 2, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female DPPD Bicycle Patrol>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 47, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 335, 17),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 14, 2),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //Detective
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male DPPD Suit>",
                GroupName = "Detective",
                RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 179, 1),
                        new PedComponent(4, 22, 1),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 39, 0),
                        new PedComponent(5, 68, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 1, 0)
            {
                DebugName = "<Male DPPD Detective>",
                GroupName = "Detective",
                RandomizeHead = true,
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(6, 4, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 292, 1),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 1),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 39, 0),
                        new PedComponent(5, 0, 0),
                }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)
            },

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                DebugName = "<Male DPPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(new List<PedPropComponent>(){},
                                                        new List<PedComponent>(){
                                                            new PedComponent(1, 0, 0),
                                                            new PedComponent(11, 212, 7),
                                                            new PedComponent(3, 11, 0),
                                                            new PedComponent(10, 0, 0),
                                                            new PedComponent(8, 94, 1),
                                                            new PedComponent(4, 86, 2),
                                                            new PedComponent(6, 51, 0),
                                                            new PedComponent(7, 8, 0),
                                                            new PedComponent(9, 37, 0),
                                                            new PedComponent(5, 65, 7),
                                                        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            ,GroupName = "Sniper"
            ,OverrideAgencyLongGuns = true
            ,OverrideLongGunsID = "GoodSniperLongGuns"
            ,AlwaysHasLongGun = true
            ,AccuracyMin = sniperAccuracyMin
                ,AccuracyMax = sniperAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax
            ,CombatRange = 3
            ,CombatMovement = 0},

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) {
                DebugName = "<Male DPPD Bicycle Patrol>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
                new PedPropComponent(0,49,0),
                }, new List<PedComponent>() {
                new PedComponent(1,0,0),
                new PedComponent(11,311,17),
                new PedComponent(3,0,0),
                new PedComponent(10,0,0),
                new PedComponent(8,94,1),
                new PedComponent(4,12,2),
                new PedComponent(6,2,0),
                new PedComponent(7,8,0),
                new PedComponent(9,37,0),
                new PedComponent(5,48,0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01",0,0) {
                DebugName = "<Female DPPD Bicycle Patrol>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {
                new PedPropComponent(0,47,0),
                }, new List<PedComponent>() {
                new PedComponent(1,0,0),
                new PedComponent(11,335,17),
                new PedComponent(3,14,0),
                new PedComponent(10,0,0),
                new PedComponent(8,33,1),
                new PedComponent(4,14,2),
                new PedComponent(6,10,0),
                new PedComponent(7,8,0),
                new PedComponent(9,0,0),
                new PedComponent(5,48,0),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

        };
    }
    private void SetupRHPD()
    {
        RHPDCops_FEJ = new List<DispatchablePerson>() {
            //RHPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 45, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //RHPD Class B
            new DispatchablePerson("mp_m_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 5, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 5, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //RHPD Class C
            new DispatchablePerson("mp_m_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 11, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 7, 0, 0), //Ranks
                    new PedComponent(10, 44, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 72, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 13, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 6, 0, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            new DispatchablePerson("mp_m_freemode_01", 5,5){
    OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Jacket>", RandomizeHead = true,
    OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
    RequiredVariation = new PedVariation(
        new List<PedPropComponent>(){

        },
        new List<PedComponent>(){
            new PedComponent(1, 101, 0),
            new PedComponent(11, 143, 0),
            new PedComponent(3, 4, 0),
            new PedComponent(10, 0, 0),
            new PedComponent(8, 56, 1),
            new PedComponent(4, 35, 0),
            new PedComponent(6, 51, 0),
            new PedComponent(7, 8, 0),
            new PedComponent(9, 28, 0),
            new PedComponent(5, 0, 0),
        }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 187, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 1),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 28, 0),
                        new PedComponent(5, 72, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 311, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Bicycle Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 49, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 311, 16),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 12, 2),
                        new PedComponent(6, 2, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class B>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 209, 6),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 6),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Male RHPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 212, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 6),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_m_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Suit>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 4, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 178, 13),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 38, 2),
                        new PedComponent(9, 38, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 68, 0),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 1,1){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Detective>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 349, 13),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 10, 2),
                    })},
            new DispatchablePerson("mp_m_freemode_01", 5,5){
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3,GroupName = "Detective",  DebugName = "<Male RHPD Armor Protection>", RandomizeHead = true,MinWantedLevelSpawn = 3,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 348, 13),
                        new PedComponent(3, 12, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 131, 0),
                        new PedComponent(4, 22, 0),
                        new PedComponent(6, 20, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 38, 0),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,1),
            EmptyHolster = new PedComponent(7,5,1)},
            new DispatchablePerson("mp_m_freemode_01", 0, 50){
                ArmorMin = 50,ArmorMax = 50,GroupName = "SWAT",DebugName = "<Male RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 150, 1),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 52, 4),
                        new PedComponent(11, 220, 11),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 25, 6),
                        new PedComponent(5, 48, 0),
                    }),
FullHolster = new PedComponent(7,110,0),
EmptyHolster = new PedComponent(7,109,0)},

            new DispatchablePerson("mp_f_freemode_01", 5,5){
            OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Utility Class B>", RandomizeHead = true,
            OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
            RequiredVariation = new PedVariation(
                new List<PedPropComponent>(){

                },
                new List<PedComponent>(){
                    new PedComponent(1, 0, 0),
                    new PedComponent(11, 225, 6),
                    new PedComponent(3, 3, 0),
                    new PedComponent(10, 0, 0),
                    new PedComponent(8, 101, 1),
                    new PedComponent(4, 89, 12),
                    new PedComponent(6, 25, 0),
                    new PedComponent(7, 8, 0),
                    new PedComponent(9, 16, 0),
                    new PedComponent(5, 65, 6),
                }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Utility Class C>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 226, 6),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 65, 6),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 5,5){
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Polo>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 335, 16),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 101, 1),
                        new PedComponent(4, 89, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Jacket>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 140, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Raincoat>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 189, 8),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 30, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 0, 0){
                MaxWantedLevelSpawn = 3, DebugName = "<Female RHPD Bicycle Patrol>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 47, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 335, 16),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 33, 1),
                        new PedComponent(4, 14, 2),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, GroupName = "Detective", DebugName = "<Female RHPD Detective>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 1, 14),
                        new PedComponent(3, 1, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 171, 18),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 40, 0),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 1,1){
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,MaxWantedLevelSpawn = 3, GroupName = "Detective", DebugName = "<Female RHPD Detective (vest)>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 281, 18),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 161, 0),
                        new PedComponent(4, 3, 4),
                        new PedComponent(6, 29, 0),
                        new PedComponent(7, 6, 0),
                        new PedComponent(9, 40, 0),
                        new PedComponent(5, 0, 0),
                    }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0)},
            new DispatchablePerson("mp_f_freemode_01", 0,50){
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,GroupName = "SWAT", DebugName = "<Female RHPD SWAT>", RandomizeHead = true,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                        new PedPropComponent(0, 149, 1),
                        new PedPropComponent(1, 22, 0),

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 11),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 27, 6),
                        new PedComponent(5, 48, 0),
                    }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)},

            //Sniper
            new DispatchablePerson("mp_m_freemode_01", 0,0){
                DebugName = "<Male RHPD Utility Class C Sniper>", RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>(){

                    },
                    new List<PedComponent>(){
                        new PedComponent(1, 101, 0),
                        new PedComponent(11, 212, 6),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 94, 1),
                        new PedComponent(4, 86, 12),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 37, 0),
                        new PedComponent(5, 65, 6),
                    })
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true
                ,AccuracyMin = sniperAccuracyMin
                ,AccuracyMax = sniperAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax
                ,CombatRange = 3
                ,CombatMovement = 0

                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0) {
                DebugName = "<Male RHPD Bicycle Patrol>",
                GroupName = "Bicycle",
                RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation( new List<PedPropComponent>() {



                  new PedPropComponent(0,49,0),


                  }, new List<PedComponent>() {
                  new PedComponent(1,101,0),
                  new PedComponent(11,311,16),
                  new PedComponent(3,0,0),
                  new PedComponent(10,0,0),
                  new PedComponent(8,94,1),
                  new PedComponent(4,12,2),
                  new PedComponent(6,2,0),
                  new PedComponent(7,8,0),
                  new PedComponent(9,0,0),
                  new PedComponent(5,48,0),
                   }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) {
                DebugName = "<Female RHPD Bicycle Patrol>", GroupName = "Bicycle", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {



              new PedPropComponent(0,47,0),


              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,335,16),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,33,1),
              new PedComponent(4,14,2),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
        };
    }
    private void SetupLSIAPD()
    {
        LSIAPDPeds_FEJ = new List<DispatchablePerson>() {
            //LSIA Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0), //Ranks
                    new PedComponent(10, 53, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSIA Class B
            new DispatchablePerson("mp_m_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 13, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 44, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",25,25) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 10, 1, 0), //Ranks
                    new PedComponent(10, 10, 4, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSIA Class C
            new DispatchablePerson("mp_m_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 4, 0),
                    new PedComponent(10, 45, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",55,55) { RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 31, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 1, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 7, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 14, 7, 0), //Ranks
                    new PedComponent(10, 14, 8, 0), //Ranks
                    new PedComponent(10, 52, 6, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Utility
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,209,8),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,2),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,65,9),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {

              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,212,8),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,2),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,65,9),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",10,10) { DebugName = "<Male LSIA ESU Utility>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,209,9),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,12),
              new PedComponent(6,51,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,65,10),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,225,8),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,2),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,9),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,226,8),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,2),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,9),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",10,10) {DebugName = "<Female LSIA ESU Utility>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,225,9),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,15,0),
              new PedComponent(5,65,10),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },

            //Polos
            new DispatchablePerson("mp_m_freemode_01",2,2) { OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,DebugName = "<Male LSIA Polo>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,311,14),
              new PedComponent(3,0,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,12),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",2,2) {OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,DebugName = "<Female LSIA Polo>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,335,14),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,101,1),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,8,0),
              new PedComponent(9,16,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Jacket
            new DispatchablePerson("mp_m_freemode_01",1,1) { DebugName = "<Male LSIA Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,52,0),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,56,1),
              new PedComponent(4,35,0),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,28,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",1,1) {DebugName = "<Female LSIA Jacket>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,175,0),
              new PedComponent(3,3,0),
              new PedComponent(10,0,0),
              new PedComponent(8,33,0),
              new PedComponent(4,34,0),
              new PedComponent(6,52,0),
              new PedComponent(7,8,0),
              new PedComponent(9,30,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Animals
            new DispatchablePerson("a_c_husky",50,50) {
                IsAnimal = true,
                DebugName = "K9_Husky",
                UnitCode = "K9",
                RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>()),
                OverrideAgencyLessLethalWeapons = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideLessLethalWeaponsID = null,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
            },
            new DispatchablePerson("a_c_retriever",50,50) {
                IsAnimal = true,
                DebugName = "K9_Retriever",
                UnitCode = "K9",
                OverrideAgencyLessLethalWeapons = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideLessLethalWeaponsID = null,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
            },
            //Snipers
            new DispatchablePerson("mp_m_freemode_01",0,0) { DebugName = "<Male LSIA Utility Class B Sniper>",RandomizeHead = true,
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,209,8),
              new PedComponent(3,4,0),
              new PedComponent(10,0,0),
              new PedComponent(8,94,1),
              new PedComponent(4,86,2),
              new PedComponent(6,51,0),
              new PedComponent(7,8,0),
              new PedComponent(9,14,0),
              new PedComponent(5,65,9),
               })
            ,GroupName = "Sniper"
            ,OverrideAgencyLongGuns = true
            ,OverrideLongGunsID = "GoodSniperLongGuns"
            ,AlwaysHasLongGun = true
            ,ArmorMin = 50
            ,ArmorMax = 50
            ,CombatAbilityMin = 2
            ,CombatAbilityMax = 2
            ,CombatRange = 3
            ,CombatMovement = 0
            ,AccuracyMin = 65
            ,AccuracyMax = 85
            ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) }
            ,
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            }
        };
    }
    private void SetupLSPD()
    {
        //Cops
        StandardCops_FEJ = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_f_y_cop_01",0,0) { MaxWantedLevelSpawn = 3 },

            //LSPD Class A
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Class A>",  MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },

                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 12, 0, 0), //Ranks
                    new PedComponent(10, 12, 1, 0),
                    new PedComponent(10, 12, 2, 0),
                    new PedComponent(10, 12, 3, 0),
                    //new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Class A>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 202, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    //new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)},

            //LSPD Class B
            new DispatchablePerson("mp_m_freemode_01",25,25) { DebugName = "<Male LSPD Class B>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = 20,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 193, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 12, 0, 0), //Ranks
                    new PedComponent(10, 12, 1, 0),
                    new PedComponent(10, 12, 2, 0),
                    new PedComponent(10, 12, 3, 0),
                    //new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",25,25) { DebugName = "<Female LSPD Class B>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 195, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 11, 0, 0), //Ranks
                    new PedComponent(10, 11, 1, 0), //Ranks
                    new PedComponent(10, 11, 2, 0),
                    new PedComponent(10, 11, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    //new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSPD Class C
            new DispatchablePerson("mp_m_freemode_01",55,55) { DebugName = "<Male LSPD Class C>", MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                                OptionalProps = MaleCopShortSleeveOptionalProps,
                OptionalPropChance = 20,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 11, 0, 0),//new PedComponent(3, 4, 0, 0),//Peter Badoingy — Today at 8:01 AM Fixed an armless cop in the FEJ dispatchable people on line 773 under ped Component ID 3 i changed the DrawableID from 4 to 11, he now has elbows and forearms.
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 190, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 15, 0, 0), //Ranks
                    new PedComponent(10, 15, 1, 0),
                    new PedComponent(10, 15, 2, 0),
                    new PedComponent(10, 15, 3, 0),
                    new PedComponent(10, 44, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",55,55) { DebugName = "<Female LSPD Class C>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopShortSleeveOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 9, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 0, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 192, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(10, 14, 0, 0), //Ranks
                    new PedComponent(10, 14, 1, 0), //Ranks
                    new PedComponent(10, 14, 2, 0),
                    new PedComponent(10, 14, 3, 0),
                    new PedComponent(10, 53, 0, 0),
                    //new PedComponent(10, 45, 0, 0),

                    new PedComponent(5, 52, 1, 0),//watches
                    new PedComponent(5, 52, 2, 0),//watches

                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSPD Jacket MALE IS MISSING CHEST?
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Jacket>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 19, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 29, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 28, 0, 0),
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Jacket>",MaxWantedLevelSpawn = 3, RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 31, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 200, 0, 0)},
                    new List<PedPropComponent>() {  }),
            OptionalComponents =
                new List<PedComponent>() {
                    new PedComponent(9, 30, 0, 0), //Ranks
                },
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //LSPD Raincoat
            new DispatchablePerson("mp_m_freemode_01",1,1) { DebugName = "<Male LSPD Raincoat>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 35, 0, 0),
                        new PedComponent(5, 52, 0, 0),
                        new PedComponent(6, 51, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 56, 0, 0),
                        new PedComponent(9, 28, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 187, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01",1,1) { DebugName = "<Female LSPD Raincoat>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 101, 0, 0),
                        new PedComponent(3, 3, 0, 0),
                        new PedComponent(4, 34, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 52, 0, 0),
                        new PedComponent(7, 8, 0, 0),
                        new PedComponent(8, 33, 0, 0),
                        new PedComponent(9, 30, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 189, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
             },

            //LSPD SWAT
            new DispatchablePerson("mp_m_freemode_01",0,20) {
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,GroupName = "SWAT"
                ,DebugName = "<Male LSPD SWAT>"
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                RandomizeHead = true,
                MinWantedLevelSpawn = 4,
                OverrideAgencySideArms = true,
                OverrideSideArmsID = "TacticalSidearms",
                OverrideAgencyLongGuns = true,
                OverrideLongGunsID = "TacticalLongGuns",
                ShrinkHeadForMask = true,

                OverrideVoice = new List<string>() { "S_M_Y_SWAT_01_WHITE_FULL_01", "S_M_Y_SWAT_01_WHITE_FULL_02", "S_M_Y_SWAT_01_WHITE_FULL_03", "S_M_Y_SWAT_01_WHITE_FULL_04" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 179, 0, 0),
                        new PedComponent(4, 31, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 110, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 16, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 220, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,150,0),new PedPropComponent(1,21,0)  })
                ,
            FullHolster = new PedComponent(7,110,0),
            EmptyHolster = new PedComponent(7,109,0)//110 has some gold chain too
            },
            new DispatchablePerson("mp_f_freemode_01",0,20) {
                AccuracyMin = swatAccuracyMin
                ,AccuracyMax = swatAccuracyMax
                ,GroupName = "SWAT"
                ,DebugName = "<Female LSPD SWAT>"
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax,
                OverrideAgencySideArms = true,
                OverrideSideArmsID = "TacticalSidearms",
                OverrideAgencyLongGuns = true,
                OverrideLongGunsID = "TacticalLongGuns",
                ShrinkHeadForMask = true,
                RandomizeHead = true,
                MinWantedLevelSpawn = 4,
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 185, 0, 0),
                        new PedComponent(3, 215, 0, 0),
                        new PedComponent(4, 30, 0, 0),
                        new PedComponent(5, 48, 0, 0),
                        new PedComponent(6, 25, 0, 0),
                        new PedComponent(7, 81, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 18, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 230, 0, 0)},
                    new List<PedPropComponent>() { new PedPropComponent(0,149,0),new PedPropComponent(1,22,0)  }),
            FullHolster = new PedComponent(7,81,0),
            EmptyHolster = new PedComponent(7,80,0)//82 kinda works?
             },

            //LSPD Detective Suit, suit covers holster
            new DispatchablePerson("mp_m_freemode_01",5,5) { DebugName = "<Male LSPD Detective Suit>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 68, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 38, 3, 0),
                        new PedComponent(8, 178, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 4, 0, 0)},
                    new List<PedPropComponent>() {  }),
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { DebugName = "<Female LSPD Detective Suit>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 68, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 0, 0, 0),
                        new PedComponent(8, 64, 0, 0),
                        new PedComponent(9, 26, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 24, 0, 0)},
                    new List<PedPropComponent>() {  }),
             },

            //LSPD Detective
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Male LSPD Detective>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 4, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 10, 3, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 6, 2, 0),
                        new PedComponent(8, 88, 0, 0),
                        new PedComponent(9, 24, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 349, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,6,1),
            EmptyHolster = new PedComponent(7,5,1)
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Female LSPD Detective>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 0, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 0, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 15, 0, 0),
                        new PedComponent(9, 26, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 27, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,6,1),
            EmptyHolster = new PedComponent(7,5,1)
             },

            //LSPD Detective Windbraker
            new DispatchablePerson("mp_m_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Male LSPD Detective Windbreaker>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 12, 0, 0),
                        new PedComponent(4, 10, 0, 0),
                        new PedComponent(5, 59, 0, 0),
                        new PedComponent(6, 10, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 179, 0, 0),
                        new PedComponent(9, 17, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 325, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,6,1),
            EmptyHolster = new PedComponent(7,5,1)
            },
            new DispatchablePerson("mp_f_freemode_01",5,5) { GroupName = "Detective", DebugName = "<Female LSPD Detective Windbreaker>",RandomizeHead = true,MaxWantedLevelSpawn = 2,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,
                RequiredVariation = new PedVariation(
                    new List<PedComponent>() {
                        new PedComponent(1, 0, 0, 0),
                        new PedComponent(3, 7, 0, 0),
                        new PedComponent(4, 3, 0, 0),
                        new PedComponent(5, 59, 0, 0),
                        new PedComponent(6, 29, 0, 0),
                        new PedComponent(7, 6, 0, 0),
                        new PedComponent(8, 39, 0, 0),
                        new PedComponent(9, 21, 0, 0),
                        new PedComponent(10, 0, 0, 0),
                        new PedComponent(11, 318, 0, 0)},
                    new List<PedPropComponent>() {  }),
            FullHolster = new PedComponent(7,6,1),
            EmptyHolster = new PedComponent(7,5,1)
             },

            //Metro (little more tactical uniforms)
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male METRO Div Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,101,1),
              new PedComponent(3,4,0),
              new PedComponent(10,12,0),
              new PedComponent(8,94,0),
              new PedComponent(4,86,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male METRO Div Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,102,1),
              new PedComponent(3,11,0),
              new PedComponent(10,15,0),
              new PedComponent(8,94,0),
              new PedComponent(4,86,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female METRO Div Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,92,1),
              new PedComponent(3,3,0),
              new PedComponent(10,11,0),
              new PedComponent(8,101,0),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female METRO Div Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,93,1),
              new PedComponent(3,14,0),
              new PedComponent(10,14,0),
              new PedComponent(8,101,0),
              new PedComponent(4,89,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0) },

            //Utility (Tactical looking,looks like metro but in LIGHT BLACK, but no armor)
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,209,0),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",5,5) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,212,0),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,225,0),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",5,5) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,226,0),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Motor Units (Motorcycle cops, we dont have that yet...)
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class A>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,200,1),
            new PedComponent(3,20,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,193,1),
            new PedComponent(3,20,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Male LSPD Motor Unit Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,190,1),
            new PedComponent(3,26,0),
            new PedComponent(10,0,0),
            new PedComponent(8,56,0),
            new PedComponent(4,32,1),
            new PedComponent(6,13,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class A>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,202,1),
            new PedComponent(3,23,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,195,1),
            new PedComponent(3,23,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",0,0) { MaxWantedLevelSpawn = 3,GroupName = "MotorcycleCop",UnitCode = "Mary", DebugName = "<Female LSPD Motor Unit Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            new PedPropComponent(0,17,1),
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,192,1),
            new PedComponent(3,28,0),
            new PedComponent(10,0,0),
            new PedComponent(8,33,0),
            new PedComponent(4,31,1),
            new PedComponent(6,34,0),
            new PedComponent(7,8,0),
            new PedComponent(9,0,0),
            new PedComponent(5,52,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Traffic Utility (kinda tactical looking, but no armor)
            new DispatchablePerson("mp_m_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Traffic Utility Class B>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,209,15),
            new PedComponent(3,4,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_m_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Traffic Utility Class C>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,212,15),
            new PedComponent(3,11,0),
            new PedComponent(10,0,0),
            new PedComponent(8,94,0),
            new PedComponent(4,86,2),
            new PedComponent(6,24,0),
            new PedComponent(7,8,0),
            new PedComponent(9,37,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Traffic Utility Class B>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,225,15),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",1,0) { MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Traffic Utility Class C>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
            }, new List<PedComponent>() {
            new PedComponent(1,101,0),
            new PedComponent(11,226,15),
            new PedComponent(3,9,0),
            new PedComponent(10,0,0),
            new PedComponent(8,101,0),
            new PedComponent(4,89,2),
            new PedComponent(6,25,0),
            new PedComponent(7,8,0),
            new PedComponent(9,15,0),
            new PedComponent(5,63,0),
            }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Beach Detail (Still in uniform, just with polo and baseball hat)
            new DispatchablePerson("mp_m_freemode_01",2,0) { MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Beach Detail>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
          new PedPropComponent(0,135,0),
          }, new List<PedComponent>() {
          new PedComponent(1,0,0),
          new PedComponent(11,93,2),
          new PedComponent(3,0,0),
          new PedComponent(10,0,0),
          new PedComponent(8,94,0),
          new PedComponent(4,86,12),
          new PedComponent(6,2,0),
          new PedComponent(7,8,0),
          new PedComponent(9,0,0),
          new PedComponent(5,48,0),
           }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },
            new DispatchablePerson("mp_f_freemode_01",2,0) {MaxWantedLevelSpawn = 3, DebugName = "<Female LSPD Beach Detail>", RandomizeHead = true,OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,84,2),
              new PedComponent(3,14,0),
              new PedComponent(10,0,0),
              new PedComponent(8,2,0),
              new PedComponent(4,89,12),
              new PedComponent(6,10,0),
              new PedComponent(7,8,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0) },

            //Detective (with armor)            
            new DispatchablePerson("mp_m_freemode_01",1,1) { OptionalProps = MaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,GroupName = "Detective", ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Armor Protection>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              new PedPropComponent(6,3,0), }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,292,1),
              new PedComponent(3,11,0),
              new PedComponent(10,0,0),
              new PedComponent(8,131,0),
              new PedComponent(4,10,4),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,24,0),
              new PedComponent(5,0,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },
            new DispatchablePerson("mp_m_freemode_01",1,1) { OptionalProps = FemaleCopOptionalProps,
                OptionalPropChance = optionalpropschance,GroupName = "Detective", ArmorMin = 50,ArmorMax = 50,MaxWantedLevelSpawn = 3, DebugName = "<Male LSPD Raid Jacket>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,0,0),
              new PedComponent(11,325,0),
              new PedComponent(3,12,0),
              new PedComponent(10,0,0),
              new PedComponent(8,179,1),
              new PedComponent(4,10,4),
              new PedComponent(6,10,0),
              new PedComponent(7,6,0),
              new PedComponent(9,17,0),
              new PedComponent(5,59,0),
               }),
            FullHolster = new PedComponent(7,6,0),
            EmptyHolster = new PedComponent(7,5,0) },



            //Snipers
            new DispatchablePerson("mp_m_freemode_01",0,0) { DebugName = "<Male METRO Div Class B Sniper>",RandomizeHead = true,OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" }, RequiredVariation = new PedVariation( new List<PedPropComponent>() {
              }, new List<PedComponent>() {
              new PedComponent(1,101,0),
              new PedComponent(11,101,1),
              new PedComponent(3,4,0),
              new PedComponent(10,12,0),
              new PedComponent(8,94,0),
              new PedComponent(4,86,12),
              new PedComponent(6,25,0),
              new PedComponent(7,1,0),
              new PedComponent(9,0,0),
              new PedComponent(5,48,0),
               })
                ,GroupName = "Sniper"
                ,OverrideAgencyLongGuns = true
                ,OverrideLongGunsID = "GoodSniperLongGuns"
                ,AlwaysHasLongGun = true

                ,CombatRange = 3
                ,CombatMovement = 0



                ,AccuracyMin = sniperAccuracyMin
                ,AccuracyMax = sniperAccuracyMax
                ,ShootRateMin = swatShootRateMin
                ,ShootRateMax = swatShootRateMax
                ,CombatAbilityMin = swatCombatAbilityMin
                ,CombatAbilityMax = swatCombatAbilityMax
                ,HealthMin = swatHealthMin
                ,HealthMax = swatHealthMax
                ,ArmorMin = swatArmorMin
                ,ArmorMax = swatArmorMax

                ,CombatAttributesToSet = new List<CombatAttributeToSet>() { new CombatAttributeToSet(21,false) },
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },

            //Animals
            new DispatchablePerson("a_c_husky",50,50) {
                IsAnimal = true,
                DebugName = "K9_Shepherd",
                UnitCode = "K9",
                RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(0, 0, 0, 0) },new List<PedPropComponent>()),
                OverrideAgencyLessLethalWeapons = true,
                OverrideAgencySideArms = true,
                OverrideAgencyLongGuns = true,
                OverrideLessLethalWeaponsID = null,
                OverrideSideArmsID = null,
                OverrideLongGunsID = null,
            },

            //Dirt Bike
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male LSPD Dirtbike patrol>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,16,0),
                        new PedPropComponent(1,25,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,152,0),
                        new PedComponent(3,179,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,55,0),
                        new PedComponent(4,67,11),
                        new PedComponent(6,47,3),
                        new PedComponent(7,1,0),
                        new PedComponent(9,18,7),
                        new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female LSPD Dirtbike Unit>",
                RandomizeHead = true,
                GroupName = "DirtBike",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                      new PedPropComponent(0,16,0),
                      new PedPropComponent(1,27,0),
                    },
                    new List<PedComponent>() {
                      new PedComponent(1,0,0),
                      new PedComponent(11,149,0),
                      new PedComponent(3,18,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,32,0),
                      new PedComponent(4,69,11),
                      new PedComponent(6,48,3),
                      new PedComponent(7,1,0),
                      new PedComponent(9,22,7),
                      new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,1,0),
            EmptyHolster = new PedComponent(7,3,0)
            },

            //Bicycle
            new DispatchablePerson("mp_m_freemode_01",0,0)
            {
                DebugName = "<Male LSPD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_M_Y_COP_01_WHITE_FULL_01", "S_M_Y_COP_01_WHITE_FULL_02", "S_M_Y_COP_01_BLACK_FULL_01", "S_M_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,49,0),
                    },
                    new List<PedComponent>()
                    {
                      new PedComponent(1,0,0),
                      new PedComponent(11,93,2),
                      new PedComponent(3,19,0),
                      new PedComponent(10,0,0),
                      new PedComponent(8,94,0),
                      new PedComponent(4,12,2),
                      new PedComponent(6,2,0),
                      new PedComponent(7,8,0),
                      new PedComponent(9,0,0),
                      new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
            },
            new DispatchablePerson("mp_f_freemode_01",0,0)
            {
                DebugName = "<Female LSPD Bicycle Uniform>",
                RandomizeHead = true,
                GroupName = "Bicycle",
                OverrideVoice = new List<string>() { "S_F_Y_COP_01_WHITE_FULL_01", "S_F_Y_COP_01_WHITE_FULL_02", "S_F_Y_COP_01_BLACK_FULL_01", "S_F_Y_COP_01_BLACK_FULL_02" },
                RequiredVariation = new PedVariation(
                    new List<PedPropComponent>()
                    {
                        new PedPropComponent(0,47,0),
                    },
                    new List<PedComponent>()
                    {
                        new PedComponent(1,0,0),
                        new PedComponent(11,84,2),
                        new PedComponent(3,31,0),
                        new PedComponent(10,0,0),
                        new PedComponent(8,2,0),
                        new PedComponent(4,14,2),
                        new PedComponent(6,10,0),
                        new PedComponent(7,8,0),
                        new PedComponent(9,0,0),
                        new PedComponent(5,48,0),
                    }),
            FullHolster = new PedComponent(7,8,0),
            EmptyHolster = new PedComponent(7,2,0)
                },
        };
    }
    private void SetupOptionalProps()
    {
        optionalpropschance = 20;

        MaleCopOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 8, 3),
            new PedPropComponent(1, 8, 5),
            new PedPropComponent(1, 8, 6),
            new PedPropComponent(1, 7, 0),
            new PedPropComponent(1, 2, 3),

        };

        FemaleCopOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 39, 0),
            new PedPropComponent(1, 40, 0),
            new PedPropComponent(1, 11, 0),
            new PedPropComponent(1, 11, 1),
            new PedPropComponent(1, 11, 3),
            new PedPropComponent(1, 24, 0),
        };

        MaleCopShortSleeveOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 37, 0),
            new PedPropComponent(1, 38, 0),
            new PedPropComponent(1, 8, 3),
            new PedPropComponent(1, 8, 5),
            new PedPropComponent(1, 8, 6),
            new PedPropComponent(1, 7, 0),
            new PedPropComponent(1, 2, 3),

            new PedPropComponent(6, 3, 0),
        };

        FemaleCopShortSleeveOptionalProps = new List<PedPropComponent>() {
            new PedPropComponent(1, 39, 0),
            new PedPropComponent(1, 40, 0),
            new PedPropComponent(1, 11, 0),
            new PedPropComponent(1, 11, 1),
            new PedPropComponent(1, 11, 3),
            new PedPropComponent(1, 24, 0),

            new PedPropComponent(6, 20, 2),
        };


        MaleSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        MaleSheriffOptionalProps.AddRange(MaleCopOptionalProps);
        MaleSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        MaleSheriffShortSleeveOptionalProps.AddRange(MaleCopShortSleeveOptionalProps);


        FemaleSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        FemaleSheriffOptionalProps.AddRange(FemaleCopOptionalProps);
        FemaleSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 0) };
        FemaleSheriffShortSleeveOptionalProps.AddRange(FemaleCopShortSleeveOptionalProps);

        MaleBCSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        MaleBCSheriffOptionalProps.AddRange(MaleCopOptionalProps);
        MaleBCSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        MaleBCSheriffShortSleeveOptionalProps.AddRange(MaleCopShortSleeveOptionalProps);


        FemaleBCSheriffOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        FemaleBCSheriffOptionalProps.AddRange(FemaleCopOptionalProps);
        FemaleBCSheriffShortSleeveOptionalProps = new List<PedPropComponent>() { new PedPropComponent(0, 13, 1) };
        FemaleBCSheriffShortSleeveOptionalProps.AddRange(FemaleCopShortSleeveOptionalProps);
    }
    //SOme stuff I have no use for now, but maybe others want to use it?
    private void DefaultConfigEXTRA(List<DispatchablePersonGroup> PeopleConfig_EUP)
    {
        List<DispatchablePerson> SearchAndRescue_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSSD SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 6),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 145, 0),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSSD SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 143, 0),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 7),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSSD SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 7),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 64, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSSD SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 7),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCSO SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 7),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 145, 3),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 3),
                        new PedComponent(5, 19, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCSO SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 6),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 143, 3),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 3),
                        new PedComponent(5, 19, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male BCSO SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 7),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 108, 8),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 64, 1),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 3),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female BCSO SAR Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 7),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 99, 8),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 66, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 50, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSFD USAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 8),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 14),
                        new PedComponent(3, 171, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 120, 1),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 8),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 224, 22),
                        new PedComponent(3, 44, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 126, 1),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LSCoFD SAR>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 24),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 68, 0),
                        new PedComponent(4, 87, 12),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 20, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 9, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LSCoFD SAR>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 88, 2),
                        new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 24),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 48, 0),
                        new PedComponent(4, 90, 12),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 1, 2),
                        new PedComponent(5, 9, 0),
                })
            },
        };
        List<DispatchablePerson> NationalParkService_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male NPS SAR Tech>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 89, 2),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 311, 25),
                        new PedComponent(3, 171, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 87, 7),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 9, 0),
                })
            },
        };
        List<DispatchablePerson> MilitaryUnused_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male National Guard Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 22, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 87, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female National Guard Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 21, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 90, 0),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male U.S. Army Combat Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 39, 1),
                        new PedPropComponent(1, 23, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 25),
                        new PedComponent(3, 141, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 37, 2),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 15, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female U.S. Army Combat Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 38, 1),
                        new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 25),
                        new PedComponent(3, 174, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 36, 2),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 17, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USAF Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 37, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 23, 0),
                        new PedComponent(8, 2, 1),
                        new PedComponent(4, 87, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAF Fatigues>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 28, 2),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 112, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 22, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 90, 0),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male USAF Pilot Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 38, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 48, 0),
                        new PedComponent(3, 16, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 30, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 33, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female USAF Pilot Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 37, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 41, 0),
                        new PedComponent(3, 17, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 29, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> ParkingEnforcement_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 215, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 214, 5),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 5),
                        new PedComponent(3, 11, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 21, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class A>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 219, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class B>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 218, 5),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking enforcement Class C>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 5),
                        new PedComponent(3, 9, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 55, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 4),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Parking Enforcement Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 151, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 129, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 12, 3),
                        new PedComponent(5, 66, 4),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Parking Enforcement Jacket>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 148, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 159, 0),
                        new PedComponent(4, 34, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 11, 3),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> GenericSecurity_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Security Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 2, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 266, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 25, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 14, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Security Coat>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 275, 8),
                        new PedComponent(3, 36, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 8, 0),
                        new PedComponent(4, 3, 9),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 8, 0),
                        new PedComponent(9, 16, 0),
                        new PedComponent(5, 48, 0),
                })
            },

            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male VIP Security>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 121, 0),
                        new PedComponent(11, 10, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 178, 0),
                        new PedComponent(4, 10, 0),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 38, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Bouncer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 121, 0),
                        new PedComponent(11, 18, 3),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 22, 4),
                        new PedComponent(6, 10, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> MerryweatherPMC_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 86, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather Enforcer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                        new PedPropComponent(1, 23, 0),
                        new PedPropComponent(2, 1, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 220, 21),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 4),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Merryweather Enforcer>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 4),
                        new PedPropComponent(1, 25, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 230, 22),
                        new PedComponent(3, 215, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 89, 6),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 8, 4),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather PMC-I>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 2),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 88, 0),
                        new PedComponent(4, 86, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Merryweather PMC-I>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 2),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 65, 0),
                        new PedComponent(4, 90, 1),
                        new PedComponent(6, 36, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Merryweather PMC-II>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 10, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 18, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 87, 0),
                        new PedComponent(4, 87, 1),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 1, 0),
                        new PedComponent(9, 6, 1),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Infiltration Operative>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 117, 0),
                        new PedPropComponent(1, 25, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 220, 21),
                        new PedComponent(3, 179, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 31, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 110, 0),
                        new PedComponent(9, 9, 3),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Infiltrator Operative>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 116, 0),
                        new PedPropComponent(1, 27, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 52, 0),
                        new PedComponent(11, 230, 22),
                        new PedComponent(3, 18, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 30, 2),
                        new PedComponent(6, 25, 0),
                        new PedComponent(7, 81, 0),
                        new PedComponent(9, 6, 3),
                        new PedComponent(5, 48, 0),
                })
            },
        };
        List<DispatchablePerson> ServiceAndTransit_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Sanitation Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 71, 0),
                        new PedComponent(3, 64, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 53, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Sanitation Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 13, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 67, 0),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 55, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Waste Collector>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 56, 0),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 59, 1),
                        new PedComponent(4, 36, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Waste Collector>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 50, 0),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 36, 0),
                        new PedComponent(4, 35, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Janitor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 65, 1),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 155, 0),
                        new PedComponent(4, 38, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Janitor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 59, 1),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 191, 0),
                        new PedComponent(4, 38, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 94, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Bugstars Overalls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 139, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 65, 0),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 66, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 38, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Mechanic Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 1),
                        new PedComponent(3, 88, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 1),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Highway Clearance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 76, 19),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 3),
                        new PedComponent(3, 99, 7),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 3),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Towing Technician>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 77, 6),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 2),
                        new PedComponent(3, 100, 6),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Mechanic Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 1),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Highway Clearance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 3),
                        new PedComponent(3, 117, 7),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 3),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Towing Technician>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 2),
                        new PedComponent(3, 190, 19),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Private Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 25, 0),
                        new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 56, 0),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 89, 0),
                        new PedComponent(4, 0, 10),
                        new PedComponent(6, 12, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 89, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Private Contractor>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 53, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 49, 0),
                        new PedComponent(3, 72, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 56, 0),
                        new PedComponent(4, 4, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 6),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Public Worker>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                        new PedPropComponent(1, 15, 9),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 2, 5),
                        new PedComponent(3, 63, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 90, 0),
                        new PedComponent(4, 86, 6),
                        new PedComponent(6, 51, 3),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 89, 1),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Public Worker>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 73, 0),
                        new PedComponent(3, 83, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 54, 0),
                        new PedComponent(4, 4, 1),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 19, 5),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Dockworker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 8),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 364, 0),
                        new PedComponent(3, 195, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 182, 0),
                        new PedComponent(4, 135, 0),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Dockworker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 8),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 383, 0),
                        new PedComponent(3, 240, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 220, 0),
                        new PedComponent(4, 142, 0),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Public Worker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 66, 2),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 27, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Public Worker Coveralls>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 60, 1),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 60, 2),
                        new PedComponent(3, 75, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 39, 2),
                        new PedComponent(6, 26, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PostOp Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 9),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 4),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female PostOp Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 9),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PostOp Bermudas>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 12, 8),
                        new PedComponent(6, 12, 12),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Post Up polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 3),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male GoPostal Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 8),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 5),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 1),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female GoPostal Uniform>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 8),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 99, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male GoPostal Bermudas>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 45, 1),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 12, 9),
                        new PedComponent(6, 12, 12),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female GoPostal polo>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 1),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 99, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Bus Driver>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 10),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 8),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 1),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Operator>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 11),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 13, 6),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 2),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male LS Transit Maintenance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 213, 12),
                        new PedComponent(3, 4, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 154, 0),
                        new PedComponent(4, 13, 7),
                        new PedComponent(6, 51, 0),
                        new PedComponent(7, 16, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Bus Driver>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 10),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 1),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Operator>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 11),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 52, 0),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female LS Transit Maintenance>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 217, 12),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 191, 0),
                        new PedComponent(4, 52, 2),
                        new PedComponent(6, 52, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 52, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> Coroner_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Los Santos Coroner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 307, 20),
                        new PedComponent(3, 86, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 28, 3),
                        new PedComponent(4, 10, 4),
                        new PedComponent(6, 101, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Los Santos Coroner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 294, 19),
                        new PedComponent(3, 104, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 23, 1),
                        new PedComponent(4, 105, 0),
                        new PedComponent(6, 101, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> MedicalGeneric_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Biohazard Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 57, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 67, 3),
                        new PedComponent(3, 88, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 62, 3),
                        new PedComponent(4, 40, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male PPE Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 21, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 0),
                        new PedComponent(11, 67, 0),
                        new PedComponent(3, 88, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 61, 0),
                        new PedComponent(4, 40, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Biohazard Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(0, 57, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 175, 0),
                        new PedComponent(11, 61, 3),
                        new PedComponent(3, 101, 1),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 43, 3),
                        new PedComponent(4, 40, 3),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female PPE Kit>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                {
                    new PedPropComponent(1, 22, 0),
                }, new List<PedComponent>()
                {
                    new PedComponent(1, 11, 0),
                        new PedComponent(11, 61, 0),
                        new PedComponent(3, 101, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 42, 0),
                        new PedComponent(4, 40, 0),
                        new PedComponent(6, 24, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 48, 0),
                })
            },
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Patient>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 104, 0),
                        new PedComponent(3, 3, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 29, 0),
                        new PedComponent(6, 34, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Patient>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 95, 0),
                        new PedComponent(3, 8, 0),
                        new PedComponent(10, 0, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 17, 0),
                        new PedComponent(6, 35, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };
        List<DispatchablePerson> Prisoner_FEJ = new List<DispatchablePerson>()
        {
            new DispatchablePerson("mp_m_freemode_01", 0, 0)
            {
                DebugName = "<Male Prisoner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_M_Y_COP_01_WHITE_FULL_01",
                    "S_M_Y_COP_01_WHITE_FULL_02",
                    "S_M_Y_COP_01_BLACK_FULL_01",
                    "S_M_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 32, 0),
                        new PedComponent(3, 0, 0),
                        new PedComponent(10, 25, 0),
                        new PedComponent(8, 15, 0),
                        new PedComponent(4, 45, 4),
                        new PedComponent(6, 42, 1),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
            new DispatchablePerson("mp_f_freemode_01", 0, 0)
            {
                DebugName = "<Female Prisoner>",
                RandomizeHead = true,
                OverrideVoice = new List<string>()
                {
                    "S_F_Y_COP_01_WHITE_FULL_01",
                    "S_F_Y_COP_01_WHITE_FULL_02",
                    "S_F_Y_COP_01_BLACK_FULL_01",
                    "S_F_Y_COP_01_BLACK_FULL_02"
                },
                RequiredVariation = new PedVariation(new List<PedPropComponent>()
                { }, new List<PedComponent>()
                {
                    new PedComponent(1, 0, 0),
                        new PedComponent(11, 31, 0),
                        new PedComponent(3, 14, 0),
                        new PedComponent(10, 24, 0),
                        new PedComponent(8, 14, 0),
                        new PedComponent(4, 47, 4),
                        new PedComponent(6, 98, 0),
                        new PedComponent(7, 0, 0),
                        new PedComponent(9, 0, 0),
                        new PedComponent(5, 0, 0),
                })
            },
        };

        PeopleConfig_EUP.Add(new DispatchablePersonGroup("SearchAndRescue_FEJ", SearchAndRescue_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("NationalParkService_FEJ", NationalParkService_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MilitaryUnused_FEJ", MilitaryUnused_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ParkingEnforcement_FEJ", ParkingEnforcement_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("GenericSecurity_FEJ", GenericSecurity_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MerryweatherPMC_FEJ", MerryweatherPMC_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("ServiceAndTransit_FEJ", ServiceAndTransit_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Coroner_FEJ", Coroner_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("MedicalGeneric_FEJ", MedicalGeneric_FEJ));
        PeopleConfig_EUP.Add(new DispatchablePersonGroup("Prisoner_FEJ", Prisoner_FEJ));
    }

}

