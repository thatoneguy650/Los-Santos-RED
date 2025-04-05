using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;


public class Interiors_Liberty
{
    private Interiors LSInteriors;
    private PossibleInteriors LibertyCityInteriors;

    public Interiors_Liberty(Interiors interiors)
    {
        LSInteriors = interiors;
    }

    public void DefaultConfig()
    {
        LibertyCityInteriors = new PossibleInteriors();
        GeneralInteriors();
        Banks();
        Bars();
        Residence();
        Restaurants();

        Serialization.SerializeParam(LibertyCityInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Interiors_{StaticStrings.LibertyConfigSuffix}.xml");


        //PossibleInteriors lcAboveInteriors = LibertyCityInteriors.Copy();
        //foreach(Interior interior in lcAboveInteriors.AllInteriors())
        //{
        //    interior.AddDistanceOffset(new Vector3(0f, 0f, 200f));
        //}
        //Serialization.SerializeParam(lcAboveInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Variations\\CenteredAbove\\Interiors_{StaticStrings.LibertyConfigSuffix}CenteredAbove.xml");

        //PossibleInteriors lcEastInteriors = LibertyCityInteriors.Copy();
        //foreach (Interior interior in lcEastInteriors.AllInteriors())
        //{
        //    interior.AddDistanceOffset(new Vector3(4949.959f, -1184.845f, -0.000109f)); 
        //}
        //Serialization.SerializeParam(lcEastInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Variations\\East\\Interiors_{StaticStrings.LibertyConfigSuffix}East.xml");
        //LCPP 4949.947f, -3750.0441f, -0.000197f / 4950.0030f, -3749.9990f, 0.0000f

        PossibleInteriors lppInteriors = LibertyCityInteriors.Copy();
        foreach (Interior interior in lppInteriors.AllInteriors())
        {
            interior.AddDistanceOffset(new Vector3(4949.947f, -3750.0441f, -0.000197f));
        }

        foreach (Interior intloc in LSInteriors.PossibleInteriors.AllInteriors())
        {
            intloc.AddLocation(lppInteriors);
        }


        Serialization.SerializeParam(lppInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Interiors_{StaticStrings.LPPConfigSuffix}.xml");

    }

    private void Banks()
    {
        List<TheftInteractItem> SafetyDepositBoxStealItems = new List<TheftInteractItem>() {
                            new TheftInteractItem() {
                                ModItemName = "Marked Cash Stack",//2500 at forger
                                Percentage = 150,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Gold Ring",//125
                                Percentage = 55,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Silver Ring",//13
                                Percentage = 35,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Fake Gold Ring",//worht 2 dollars
                                Percentage = 7,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Fake Silver Ring",//worth 1 dollar
                                Percentage = 5,
                            },
                        };
        int SafetyDepositBoxStealMinItems = 1;
        int SafetyDepositBoxStealMaxItems = 10;


        List<TheftInteractItem> SafetyDepositBoxLargeStealItems = new List<TheftInteractItem>() {
                            new TheftInteractItem() {
                                ModItemName = "Marked Cash Stack",//2500 at forger
                                MaxItems = 3,
                                Percentage = 400,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Gold Ring",//125
                                MaxItems = 4,
                                Percentage = 55,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Silver Ring",//13
                                MaxItems = 5,
                                Percentage = 35,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Fake Gold Ring",//worht 2 dollars
                                Percentage = 2,
                            },
                            new TheftInteractItem() {
                                ModItemName = "Fake Silver Ring",//worth 1 dollar
                                Percentage = 2,
                            },
                        };

        int SafetyDepositBoxStealMinLargeItems = 1;
        int SafetyDepositBoxStealMaxLargeItems = 15;

        LibertyCityInteriors.BankInteriors.AddRange(new List<BankInterior>()
        {
            new BankInterior(25858,"Bank Of Liberty") {
                SearchLocations = new List<Vector3>(){new Vector3(217.8289f, 27.17734f, 15.40732f),new Vector3(236.4546f, 23.64848f, 15.40731f),new Vector3(232.7605f, 19.77981f, 8.907292f)},
                IsWeaponRestricted = true,Doors =  new List<InteriorDoor>() {

                    new InteriorDoor(3863802474,new Vector3(210.2651f, 30.79712f, 15.64099f)) { LockWhenClosed = true },//Left 1
                    new InteriorDoor(866127123,new Vector3(210.2651f, 23.80737f, 15.64099f)) { LockWhenClosed = true },//Left 2

                    new InteriorDoor(3863802474,new Vector3(210.2651f, 26.80957f, 15.64099f)) { LockWhenClosed = true },//Right 1
                    new InteriorDoor(866127123,new Vector3(210.2651f, 27.79492f, 15.64099f)) { LockWhenClosed = true },//Right 2
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("bolDrawer1",new Vector3(231.1804f, 25.4906f, 15.40732f), 120.5218f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("bolDrawer2",new Vector3(228.4528f, 29.75732f, 15.40731f), 122.7199f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("bolDrawer3",new Vector3(225.3774f, 33.99172f, 15.40731f), 124.7775f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(220.8931f, 22.25491f, 8.907288f),360f-274.3875f,-.4f), //1
                        Heading = 274.3875f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(217.3838f, 21.82025f, 8.90728f),360f-91.61941f,-.4f), //2
                        Heading = 91.61941f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault3",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(217.2888f, 20.62871f, 8.90728f),360f-90.78756f,-.4f),  //3
                        Heading = 90.78756f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault4",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(217.3019f, 18.76634f, 8.90728f),360f-91.83313f,-.4f), //4
                        Heading = 91.83313f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault5",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(217.2644f, 17.38053f, 8.90728f),360f-89.10315f,-.4f), //5
                        Heading = 89.10315f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault6",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(218.5396f, 16.75767f, 8.907285f),360f-180.9633f,-.4f),  //6
                        Heading = 180.9633f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),

                    },
//                    new ItemTheftInteract() {
//                        PossibleItems = SafetyDepositBoxLargeStealItems,
//                        MinItems = SafetyDepositBoxStealMinLargeItems,
//                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
//                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
//                        Name = "bolVault7",
//                        Position = NativeHelper.GetOffsetPosition(new Vector3(219.7897f, 16.7446f, 8.90729f),360f-181.6117f,-.4f),  //7
//                        Heading = 181.6117f,
//                        ButtonPromptText = "Rob",
//                        UseNavmesh = false,
//
//                        HasPreInteractRequirement = true,
//                        ItemUsePreInteract = new DrillUsePreInteract(),
//
 //                   },
 //                   new ItemTheftInteract() {
 //                       PossibleItems = SafetyDepositBoxLargeStealItems,
 //                       MinItems = SafetyDepositBoxStealMinLargeItems,
 //                       MaxItems = SafetyDepositBoxStealMaxLargeItems,
 //                       ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
 //                       Name = "bolVaultSmSafe",
 //                       Position = NativeHelper.GetOffsetPosition(new Vector3(220.3102f, 16.82383f, 8.907287f),360f-268.7439f,-.4f),  //8
 //                       Heading = 268.7439f,
 //                       ButtonPromptText = "Rob",
 //                       UseNavmesh = false,
 //
 //                       HasPreInteractRequirement = true,
 //                       ItemUsePreInteract = new DrillUsePreInteract(),
 //
 //                   },
                     new MoneyTheftInteract("BOLSmallSafe",new Vector3(220.3102f, 16.82383f, 8.907287f), 268.7439f,"Steal from Safe")//only here for example
                    {
                        CashMinAmount = 5000,
                        CashMaxAmount = 10000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 1000,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        IntroAnimationDictionary = "anim@amb@business@weed@weed_inspecting_lo_med_hi@",
                        IntroAnimation = "weed_spraybottle_stand_kneeling_01_inspector",
                        LoopAnimationDictionary = "anim@amb@business@weed@weed_inspecting_lo_med_hi@",
                        LoopAnimation = "weed_spraybottle_stand_kneeling_01_inspector",
                    },


                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolInnerVaultLaSafe",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(220.2592f, 18.05546f, 8.907285f),360f-268.8512f,-.4f),  //9
                        Heading = 268.8512f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,

                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
        });
    }



    private void Bars()
    {

    }


    private void GeneralInteriors()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {

            //Bars
            new Interior(54018, "Lucky Winkles Bar")
            {
                            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(4114682006,new Vector3(-202.3672f, 950.2361f, 10.64464f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(80386, "Steinway Beer Garden")
            {
                            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(261592072,new Vector3(1387.857f, 1229.011f, 35.76453f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(164866, "Comrades Bar")
            {
                            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(387699963,new Vector3(1166.586f, 4.418087f, 15.73469f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//front
                    new InteriorDoor(387699963,new Vector3(1185.835f, 2.27493f, 15.73483f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear
                    new InteriorDoor(387699963,new Vector3(1181.423f, 0.7771521f, 15.73488f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                },
            },

            //Bowling
            new Interior(38146, "Memory Lanes")//27394
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3477273845, new Vector3(1435.569f, -184.5796f, 16.70291f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//Left
                    new InteriorDoor(2464449593, new Vector3(1438.569f, -184.5796f, 16.70291f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//Right
                },
            }, //Firefly Island, Broker
            new Interior(92162, "Memory Lanes")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3477273845, new Vector3(-335.7781f, 570.1655f, 5.075134f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//Left
                    new InteriorDoor(2464449593, new Vector3(-335.7781f, 573.1655f, 5.075134f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//Right
                },
            }, //Golden Pier, Westminster, Algonquin

            //Clubs
            new Interior(130818, "Bahama Mamas")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(725112888, new Vector3(-160.1238f, 891.2871f, 14.65746f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(12034, "Hercules")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1316667213, new Vector3(-202.1077f, 851.3904f, 11.95598f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(126210, "Maisonette 9")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(4119540397, new Vector3(-229.8753f, 646.3377f, 10.14345f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },

            //Diner - Cafe
            new Interior(101634, "69th Street Diner")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1285262331, new Vector3(1123.339f, 11.90283f, 16.15447f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//front
                    new InteriorDoor(3232592925,new Vector3(1129.916f, 0.764942f, 16.14832f)){ NeedsDefaultUnlock = true, LockWhenClosed = true },//side
                },


            },
            new Interior(121346,"Homebrew Cafe")
            {
                            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1542565804,new Vector3(14.60596f, -9.044677f, -2.845501f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },

            //FastFood
            //BS
            new Interior(126722, "Burger Shot")
            {

                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(1890.526f, 718.3006f, 25.46795f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                    new InteriorDoor(1050821746,new Vector3(1890.525f, 721.31f, 25.46795f)){ NeedsDefaultUnlock = true, LockWhenClosed = true },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(1880.79f, 727.7058f, 25.4694f)){ NeedsDefaultUnlock = true,LockWhenClosed = true },
                    new InteriorDoor(1050821746,new Vector3(1877.78f, 727.7051f, 25.4694f)){ NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },
            new Interior(156674, "Burger Shot"),
            new Interior(112642, "Burger Shot - Star Junction"),
            new Interior(109570, "Burger Shot - North Holland"),
            new Interior(134402, "Burger Shot - Bohan"),
            new Interior(105986, "Burger Shot - The Meat Quarter"),
            new Interior(59650, "Burger Shot - Alderney"),
            //CB
            new Interior(143874, "Cluckin' Bell - Dukes"),
            new Interior(124162, "Cluckin' Bell - The Triangle"),

            //Fire Stations
            new Interior(160514,"Broker Fire Station"){  },
            new Interior(176898, "Bohan Fire Station"){  },

            //Gun Stores
            new Interior(108290, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(1295.643f, 579.8269f, 34.49496f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },// GunStore - Downtown Broker
            new Interior(92674, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(311.2178f, 150.6089f, 11.41267f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            }, // GunStore - Chinatown, Algonquin
            new Interior(101890, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(-1094.076f, 802.6436f, 14.86613f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            },// GunStore -  Port Tudor, Alderney

            //Hotel
            new Interior(3330, "The Majestic Hotel Lobby"),
            new Interior(41218, "The Majestic Hotel Elevator Floor"),
            new Interior(148994, "The Majestic Hotel Room"),



            //Internet Cafe
            new Interior(50178, "tw@ - Broker"){  },
            new Interior(166914, "tw@ - Bercham"),
            new Interior(66562, "tw@ - North Holland"),

            //Laundromats
            new Interior(138498, "Laundromat") { }, // Harrison Street, East Island City, Dukes.
            new Interior(134146, "Laundromat") { }, // Hubbard Avenue, Alderney City, Alderney.
            new Interior(141826, "Laundromat") { }, // Oneida Avenue, Hove Beach, Broker.

            //Restaurants
            new Interior(78338, "Perestroika")
            {
                             Doors = new List<InteriorDoor>()
                {
                   new InteriorDoor(2762578800,new Vector3(1194.172f, 204.5459f, 20.12718f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//cabaret_door_l 
                   new InteriorDoor(1985756882,new Vector3(1197.172f, 204.5459f, 20.12718f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//cabaret_door_r 
                   new InteriorDoor(3971243973,new Vector3(1194.117f, 209.6353f, 20.14878f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                   new InteriorDoor(3971243973,new Vector3(1197.115f, 209.6353f, 20.14878f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                   new InteriorDoor(3971243973,new Vector3(1200.812f, 203.7319f, 20.12878f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                   new InteriorDoor(3971243973,new Vector3(1185.197f, 232.5177f, 18.51646f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                   new InteriorDoor(3971243973,new Vector3(1191.784f, 243.1203f, 16.10701f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//inside
                   new InteriorDoor(2881168431,new Vector3(1206.35f, 236.5193f, 22.01962f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear_door
                   new InteriorDoor(2881168431,new Vector3(1206.35f, 233.5154f, 22.01962f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear_door
                },



            },
            new Interior(85506, "Mr Fuk's - Alderney City") { },

            //new Interior(76290,"Perestroika")




            //Strip Clubs
            new Interior(113666, "The Triangle Club ")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(358597415, new Vector3(1436.133f, 2204.258f, 17.97499f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//main
                    new InteriorDoor(2881168431, new Vector3(1381.678f, 2163.791f, 16.97499f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear
                },
            }, //Northern Gardens, Bohan
            new Interior(67586, "Honkers Gentlemen's Club")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2881168431, new Vector3(-1341.933f, 519.5435f, 10.26201f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                },
            }, //Tudor, Alderney 



            // ???
            new Interior(151554,"Beechwood Apts 2"),
            new Interior(119042,"Beechwood Apts 1"),
            new Interior(35330, "Beechwood Apts 3"),
            new Interior(37634, "Beechwood Apts 5"),
            //


            new Interior(88834, "Old House - Firefly Island"),
            new Interior(158978, "Drug Den - Schlotter Broker"),
            new Interior(54274, "Scummy Apartment"),


            new Interior(24578, "JJ China Limited"),

            new Interior(16642, "Goldberg Ligner & Shyster Offices"),
            new Interior(139266, "The Libertonian"),



            new Interior(172034, "Sprunk Factory - Bohan Industrial"),
            new Interior(88578, "Lompoc Avenue Warehouse"), // Industrial, Bohan



            //burger shot beechwoodd city
            //3024662465,new Vector3(1890.526f, 718.3006f, 25.46795f)

        });


    }

    private void Residence()
    {


        LibertyCityInteriors.ResidenceInteriors.AddRange(new List<ResidenceInterior>()
        {
            new ResidenceInterior(152578,"Playboy X Penthouse") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-188.2347f, 1961.426f, 38.96125f),
                InteriorEgressHeading = 179.2488f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("playboyaptExit1",new Vector3(-188.2347f, 1961.426f, 38.96125f), 0.1088863f ,"Exit") ,
                    new StandardInteriorInteract("playboyaptStandard1",new Vector3(-186.5664f, 1956.064f, 38.96646f), 182.3361f,"Interact")
                    {
                        CameraPosition = new Vector3(-183.2593f, 1958.342f, 40.32619f),
                        CameraDirection = new Vector3(-0.7304603f, -0.637247f, -0.24565f),
                        CameraRotation = new Rotator(-14.22025f, 2.642284E-06f, 131.1012f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("playboyaptInventory1",new Vector3(-181.0498f, 1962.895f, 38.96647f), 0.9463751f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-182.835f, 1961.804f, 39.98312f),
                        CameraDirection = new Vector3(0.7164276f, 0.5529848f, -0.4253695f),
                        CameraRotation = new Rotator(-25.17406f, 0f, -52.33673f)
    },
                    new InventoryInteract("playboyaptInventory2",new Vector3(-180.4159f, 1960.36f, 38.96647f), 4.749637f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-183.0071f, 1959.303f, 40.12902f),
                        CameraDirection = new Vector3(0.8029016f, 0.4488999f, -0.3922219f),
                        CameraRotation = new Rotator(-23.09282f, -9.281453E-06f, -60.79057f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("playboyaptChange1",new Vector3(-196.7056f, 1952.908f, 38.9665f), 272.0037f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-193.8193f, 1953.114f, 39.76453f),
                        CameraDirection = new Vector3(-0.9620676f, -0.02033964f, -0.2720518f),
                        CameraRotation = new Rotator(-15.7864f, 9.870523E-06f, 91.21114f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("playboyaptRest1", new Vector3(-191.9487f, 1956.802f, 38.9665f), 184.5695f,"Sleep")
                    {
                        CameraPosition = new Vector3(-194.9979f, 1956.685f, 40.31964f),
                        CameraDirection = new Vector3(0.6897783f, -0.520592f, -0.5031797f),
                        CameraRotation = new Rotator(-30.21059f, -2.667481E-05f, -127.0427f)
                    },
                },
            },
            new ResidenceInterior(142850,"Studio Apartment") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(333.9883f, 1345.539f, 45.0461f),
                InteriorEgressHeading = 267.9149f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("studioaptExit1",new Vector3(333.9883f, 1345.539f, 45.0461f), 91.86633f ,"Exit") ,
                    new StandardInteriorInteract("studioaptStandard1",new Vector3(337.8049f, 1345.959f, 45.04611f), 270.5879f,"Interact")
                    {
                        CameraPosition = new Vector3(335.7429f, 1347.893f, 46.22897f),
                        CameraDirection = new Vector3(0.8424098f, -0.4465208f, -0.301604f),
                        CameraRotation = new Rotator(-17.55397f, -4.477364E-06f, -117.9259f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("studioaptInventory1",new Vector3(332.4071f, 1351.852f, 45.04612f), 92.57986f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(333.7118f, 1350.125f, 45.94101f),
                        CameraDirection = new Vector3(-0.5748014f, 0.7399729f, -0.3493471f),
                        CameraRotation = new Rotator(-20.44739f, 3.189145E-05f, 37.8396f)
                },
                    new InventoryInteract("studioaptInventory2",new Vector3(337.4707f, 1355.777f, 45.04611f), 3.660645f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(335.3792f, 1354.737f, 46.00565f),
                        CameraDirection = new Vector3(0.7699414f, 0.520547f, -0.3690811f),
                        CameraRotation = new Rotator(-21.65896f, -2.939621E-05f, -55.9379f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("studioaptChange1",new Vector3(336.3454f, 1336.617f, 45.04611f), 356.4514f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(336.2229f, 1339.738f, 45.87851f),
                        CameraDirection = new Vector3(-0.03027195f, -0.9652488f, -0.2595734f),
                        CameraRotation = new Rotator(-15.04475f, 0f, 178.2037f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("studioaptRest1", new Vector3(340.9554f, 1336.499f, 45.0892f), 359.6932f,"Sleep")
                    {
                        CameraPosition = new Vector3(344.3446f, 1336.979f, 46.44402f),
                        CameraDirection = new Vector3(-0.7603666f, 0.4343084f, -0.4829274f),
                        CameraRotation = new Rotator(-28.87677f, 9.750054E-06f, 60.26573f)
                    },
                },
            },
            new ResidenceInterior(39170,"South Bohan Apartment") {// Door is non functioning,Can walk through/can cause interior issues
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(841.2067f, 1899.031f, 17.49082f),
                InteriorEgressHeading = 0.9092442f,
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("southbohanStandard1",new Vector3(839.2178f, 1902.237f, 17.47132f), 27.44588f,"Interact")
                    {
                        CameraPosition = new Vector3(839.2902f, 1899.524f, 18.46343f),
                        CameraDirection = new Vector3(-0.04744998f, 0.9517307f, -0.3032443f),
                        CameraRotation = new Rotator(-17.65257f, 0f, 2.854204f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(261592072,new Vector3(835.2046f, 1894.023f, 11.785f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },//90.81429f)
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("southbohanInventory1",new Vector3(847.3621f, 1902.232f, 17.47133f), 271.6566f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(845.7986f, 1904.37f, 18.65309f),
                        CameraDirection = new Vector3(0.5751348f, -0.6988618f, -0.4252202f),
                        CameraRotation = new Rotator(-25.1646f, -1.980935E-05f, -140.547f)
                },
                    new InventoryInteract("southbohanInventory2",new Vector3(838.7004f, 1899.834f, 17.47133f), 179.147f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(840.86f, 1901.445f, 18.67881f),
                        CameraDirection = new Vector3(-0.6846707f, -0.5983276f, -0.4162093f),
                        CameraRotation = new Rotator(-24.59549f, 4.694836E-06f, 131.1499f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("southbohanChange1",new Vector3(841.4188f, 1906.094f, 17.47133f), 180.2521f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(841.5556f, 1903.208f, 18.32056f),
                        CameraDirection = new Vector3(0.02624621f, 0.9549722f, -0.2955321f),
                        CameraRotation = new Rotator(-17.18945f, 2.792789E-08f, -1.574306f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("southbohanRest1", new Vector3(837.5573f, 1905.378f, 17.47133f), 267.3164f,"Sleep")
                    {
                        CameraPosition = new Vector3(837.9316f, 1902.63f, 18.69568f),
                        CameraDirection = new Vector3(0.4276391f, 0.7953158f, -0.4296482f),
                        CameraRotation = new Rotator(-25.44524f, 1.701881E-05f, -28.26679f)
                    },
                },
            },//South Bohan Safehouse
            new ResidenceInterior(46850,"Broker Apartment") {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(1130.874f, -7.121155f, 19.40236f),
                InteriorEgressHeading = 1.34244f,
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("brokeraptStandard1",new Vector3(1130.454f, -4.036095f, 19.40236f), 1.176553f,"Interact")
                    {
                        CameraPosition = new Vector3(1128.225f, -7.174483f, 20.69671f),
                        CameraDirection = new Vector3(0.5709521f, 0.787036f, -0.2336407f),
                        CameraRotation = new Rotator(-13.51151f, 4.390381E-06f, -35.95886f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2181386400,new Vector3(1134.176f, -10.53613f, 15.40443f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },//90.81429f)
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("brokeraptInventory1",new Vector3(1125.994f, -6.80858f, 19.41108f), 181.8482f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(1128.52f, -5.435515f, 20.81303f),
                        CameraDirection = new Vector3(-0.7816852f, -0.4300944f, -0.4516492f),
                        CameraRotation = new Rotator(-26.84955f, 7.655484E-06f, 118.8202f)
                },
                    new InventoryInteract("brokeraptInventory2",new Vector3(1132.735f, -2.0904f, 19.40237f), 189.5905f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(1129.822f, -1.947136f, 20.26321f),
                        CameraDirection = new Vector3(0.9117068f, -0.2928823f, -0.2881157f),
                        CameraRotation = new Rotator(-16.74518f, -4.457903E-07f, -107.8094f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("brokeraptChange1",new Vector3(1131.475f, 0.8239648f, 19.40237f), 182.1172f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(1131.477f, -1.829818f, 20.2535f),
                        CameraDirection = new Vector3(0.04927624f, 0.9553418f, -0.2913655f),
                        CameraRotation = new Rotator(-16.93973f, -3.346865E-07f, -2.952682f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("brokeraptRest1", new Vector3(1129.107f, -2.780038f, 19.40237f), 357.6328f,"Sleep")
                    {
                        CameraPosition = new Vector3(1131.987f, -2.475091f, 20.64133f),
                        CameraDirection = new Vector3(-0.7985814f, 0.3459656f, -0.4925197f),
                        CameraRotation = new Rotator(-29.50633f, 9.810093E-07f, 66.57652f)
                    },
                },
            }, // Broker Safehouse
            new ResidenceInterior(111874,"145 Mahesh Avenue") {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(-731.2532f, 1378.216f, 19.00132f),
                InteriorEgressHeading = 356.884f,
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("alderenyaptStandard1",new Vector3(-729.8444f, 1382.111f, 19.03903f), 313.5496f,"Interact")
                    {
                        CameraPosition = new Vector3(-730.8866f, 1379.703f, 20.29354f),
                        CameraDirection = new Vector3(0.3834104f, 0.8622515f, -0.3309363f),
                        CameraRotation = new Rotator(-19.32561f, -1.311893E-05f, -23.97289f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2124429686,new Vector3(-725.5925f, 1387.102f, 14.23296f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },//90.81429f)
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("alderenyaptInventory1",new Vector3(-724.3544f, 1384.774f, 19.00132f), 177.3981f ,"Access Items")
                    {
                        CameraPosition = new Vector3(-726.6008f, 1384.994f, 19.89845f),
                        CameraDirection = new Vector3(0.8892203f, -0.2438023f, -0.3871017f),
                        CameraRotation = new Rotator(-22.77428f, 0f, -105.3323f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("alderenyaptChange1",new Vector3(-732.3664f, 1383.038f, 19.00132f), 270.7104f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-729.7426f, 1383.015f, 19.91359f),
                        CameraDirection = new Vector3(-0.9393548f, 0.03328265f, -0.341328f),
                        CameraRotation = new Rotator(-19.95781f, 1.703107E-06f, 87.97078f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("alderenyaptRest1", new Vector3(-730.9968f, 1385.761f, 19.00132f), 91.93905f ,"Sleep")
                    {
                        CameraPosition = new Vector3(-730.1377f, 1382.632f, 20.24364f),
                        CameraDirection = new Vector3(-0.5588966f, 0.7283394f, -0.3964294f),
                        CameraRotation = new Rotator(-23.35515f, -1.022968E-05f, 37.50109f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },//Alderney Safehouse
            new ResidenceInterior(1538,"370 Galveston Ave") { // new Interior(1538, "e2_luis"),
                InteriorEgressPosition = new Vector3(-200.8998f, 1883.38f, 16.49077f),
                InteriorEgressHeading = 264.2994f,
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GalvestonStandard1",new Vector3(-197.8051f, 1888.127f, 16.47426f), 89.00453f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-197.0363f, 1890.73f, 17.58462f),
                        CameraDirection = new Vector3(-0.498876f, -0.802644f, -0.3269332f),
                        CameraRotation = new Rotator(-19.08274f, 5.420512E-06f, 148.1374f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855,new Vector3(-201.5826f, 1884.088f, 16.70837f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },//90.81429f)
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GalvestonInventory1",new Vector3(-196.2684f, 1892.974f, 16.48327f), 269.4172f ,"Access Items")
                    {
                        UseNavmesh =false,
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-196.8831f, 1890.594f, 17.46937f),
                        CameraDirection = new Vector3(0.4479685f, 0.8236551f, -0.3477305f),
                        CameraRotation = new Rotator(-20.34856f, -5.463601E-06f, -28.54083f)
                },
                    new InventoryInteract("GalvestonInventory2",new Vector3(-195.6702f, 1886.171f, 16.45794f), 181.9774f ,"Access Weapons/Cash")
                    {
                        UseNavmesh =false,
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-198.0491f, 1886.697f, 17.61443f),
                        CameraDirection = new Vector3(0.8676881f, -0.3632791f, -0.3393311f),
                        CameraRotation = new Rotator(-19.83613f, -4.447367E-05f, -112.7178f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GalvestonChange1",new Vector3(-190.9929f, 1889.329f, 16.45696f), 86.94769f ,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-193.8589f, 1889.282f, 17.09246f),
                        CameraDirection = new Vector3(0.9676532f, -0.035804f, -0.2497307f),
                        CameraRotation = new Rotator(-14.46157f, 4.243231E-06f, -92.11903f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GalvestonRest1", new Vector3(-194.4311f, 1891.276f, 16.45654f), 268.4165f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-194.873f, 1889.619f, 17.75617f),
                        CameraDirection = new Vector3(0.6740843f, 0.4977706f, -0.5457424f),
                        CameraRotation = new Rotator(-33.07541f, 2.343424E-05f, -53.55637f)
                    },
                },
            },  // Luis Apartment
            new ResidenceInterior(100610,"Middle Park Penthouse") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(228.561f, 1291.176f, 62.80102f),
                InteriorEgressHeading = 358.9589f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("mppentExit1",new Vector3(228.5653f, 1291.33f, 62.80102f), 179.6859f ,"Exit")
                    {
                        UseNavmesh =false,
                    } ,
                    new StandardInteriorInteract("mppentStandard1",new Vector3(214.3122f, 1286.488f, 61.90554f), 91.19791f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(214.2015f, 1286.548f, 62.83566f),
                        CameraDirection = new Vector3(-0.9422437f, -0.0005598353f, -0.3349276f),
                        CameraRotation = new Rotator(-19.56813f, -2.131829E-05f, 90.03404f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("mppentInventory1",new Vector3(222.6417f, 1278.16f, 62.52046f), 270.6129f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(221.3514f, 1275.455f, 63.41724f),
                        CameraDirection = new Vector3(0.6445473f, 0.710347f, -0.2827825f),
                        CameraRotation = new Rotator(-16.42634f, 7.120833E-06f, -42.21965f)
    },
                    new InventoryInteract("mppentInventory2",new Vector3(208.2335f, 1289.433f, 62.21205f), 91.67359f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(210.0907f, 1291.74f, 63.77435f),
                        CameraDirection = new Vector3(-0.7813568f, -0.5301833f, -0.3292222f),
                        CameraRotation = new Rotator(-19.22157f, 4.520897E-06f, 124.1585f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("mppentChange1",new Vector3(228.5174f, 1281.343f, 62.52045f), 83.09912f ,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(225.1406f, 1281.173f, 63.17487f),
                        CameraDirection = new Vector3(0.9882383f, -0.01377646f, -0.1522997f),
                        CameraRotation = new Rotator(-8.76022f, 8.63851E-07f, -90.79868f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("mppentRest1", new Vector3(213.2439f, 1283.579f, 61.90553f), 180.8321f ,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(214.8775f, 1285.32f, 63.44509f),
                        CameraDirection = new Vector3(-0.3359595f, -0.7726297f, -0.5386786f),
                        CameraRotation = new Rotator(-32.59373f, 3.040102E-06f, 156.4993f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            }, // Yusuf's Penthouse
            new ResidenceInterior(113154,"Studio Apartment") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(1042.585f, 641.5295f, 29.23772f),
                InteriorEgressHeading = 240.6072f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("studaptExit1",new Vector3(1042.585f, 641.5295f, 29.23772f), 58.48744f,"Exit")
                    {
                        UseNavmesh =false,
                    } ,
                    new StandardInteriorInteract("studaptStandard1",new Vector3(1046.037f, 637.7001f, 29.23773f), 146.6657f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1045.219f, 640.5579f, 30.51376f),
                        CameraDirection = new Vector3(-0.0530019f, -0.951273f, -0.3037606f),
                        CameraRotation = new Rotator(-17.68361f, -1.702621E-05f, 176.811f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("studaptInventory1",new Vector3(1054.01f, 640.391f, 29.40617f), 199.4356f,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1051.865f, 641.0962f, 30.56939f),
                        CameraDirection = new Vector3(0.752804f, -0.5008591f, -0.4271138f),
                        CameraRotation = new Rotator(-25.28453f, -1.605196E-05f, -123.6368f)
    },
                    new InventoryInteract("studaptInventory2",new Vector3(1047.409f, 627.384f, 29.23772f), 244.1518f,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1045.28f, 626.3602f, 30.45107f),
                        CameraDirection = new Vector3(0.84171f, 0.2752596f, -0.464496f),
                        CameraRotation = new Rotator(-27.67761f, -3.374312E-06f, -71.891f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("studaptChange1",new Vector3(1039.484f, 637.6368f, 29.23774f), 199.779f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1040.422f, 635.152f, 30.00046f),
                        CameraDirection = new Vector3(-0.2841462f, 0.9256303f, -0.2499389f),
                        CameraRotation = new Rotator(-14.4739f, 4.408796E-07f, 17.06523f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("studaptRest1", new Vector3(1046.162f, 633.9925f, 29.23772f), 240.7971f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1045.944f, 636.3994f, 31.06954f),
                        CameraDirection = new Vector3(0.1860044f, -0.8695814f, -0.4574172f),
                        CameraRotation = new Rotator(-27.22057f, -2.928314E-05f, -167.9263f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            }, // Brucie's Studio Apartment
            new ResidenceInterior(216578,"Single Room") {
                InteriorEgressPosition = new Vector3(105.6322f, 1872.835f, 32.79689f),
                InteriorEgressHeading = 87.70292f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3727781305,new Vector3(99.66792f, 1877.964f, 21.05347f)) { LockWhenClosed = true },
                },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("singrmaptStandard1",new Vector3(99.91463f, 1876.959f, 32.79693f), 95.84647f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(100.815f, 1877.71f, 34.22709f),
                        CameraDirection = new Vector3(-0.8189111f, -0.3715491f, -0.4374197f),
                        CameraRotation = new Rotator(-25.93936f, 7.595365E-06f, 114.4043f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("singrmaptInventory2",new Vector3(102.3684f, 1874.028f, 32.80124f), 281.446f,"Access Items")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(101.5556f, 1871.825f, 33.9931f),
                        CameraDirection = new Vector3(0.5709561f, 0.710955f, -0.4105511f),
                        CameraRotation = new Rotator(-24.23946f, -7.490577E-06f, -38.76736f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("singrmaptChange1",new Vector3(107.585f, 1872.733f, 32.79688f), 84.24583f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(104.3221f, 1872.734f, 33.54751f),
                        CameraDirection = new Vector3(0.9709671f, -0.04076166f, -0.2357147f),
                        CameraRotation = new Rotator(-13.63375f, -6.039883E-06f, -92.40389f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("singrmaptRest1", new Vector3(102.307f, 1875.195f, 32.79693f), 359.6346f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(100.4455f, 1874.011f, 34.24353f),
                        CameraDirection = new Vector3(0.5939716f, 0.6753681f, -0.4371221f),
                        CameraRotation = new Rotator(-25.92041f, 1.898536E-06f, -41.33093f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            }, // Single Room Apartment
            new ResidenceInterior(38402,"Apartment 204") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-302.7182f, 1751.083f, 97.53461f),
                InteriorEgressHeading = 165.7377f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(106751028,new Vector3(-309.8293f, 1743.489f, 97.78454f)) { LockWhenClosed = true },
                },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("westaptExit1",new Vector3(-302.7182f, 1751.083f, 97.53461f), 342.735f,"Exit") ,
                    new StandardInteriorInteract("westaptStandard1",new Vector3(-306.659f, 1736.794f, 97.56611f), 257.585f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-308.5562f, 1735.258f, 98.67234f),
                        CameraDirection = new Vector3(0.7836698f, 0.5190888f, -0.3411867f),
                        CameraRotation = new Rotator(-19.94919f, -2.724822E-06f, -56.48022f)
                    },
                    new ToiletInteract("westaptToilet1",new Vector3(-309.8867f, 1732.109f, 97.54951f), 166.5865f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-307.6836f, 1732.821f, 98.84805f),
                        CameraDirection = new Vector3(-0.8049945f, -0.3821636f, -0.4538005f),
                        CameraRotation = new Rotator(-26.98779f, 1.916217E-05f, 115.3956f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("westaptInventory2",new Vector3(-310.9941f, 1725.605f, 97.53469f), 347.688f,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-309.448f, 1724.043f, 98.69557f),
                        CameraDirection = new Vector3(-0.4117949f, 0.7958871f, -0.4438342f),
                        CameraRotation = new Rotator(-26.34878f, 2.477167E-05f, 27.35721f)
                    },
                    new InventoryInteract("westaptInventory2",new Vector3(-313.3752f, 1723.74f, 97.5347f), 113.1772f,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-312.4327f, 1726.126f, 98.76582f),
                        CameraDirection = new Vector3(-0.6273283f, -0.6858361f, -0.368901f),
                        CameraRotation = new Rotator(-21.64785f, 1.010417E-05f, 137.5511f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("westaptChange1",new Vector3(-310.9914f, 1729.167f, 97.57561f), 256.2012f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-308.0149f, 1728.64f, 98.47282f),
                        CameraDirection = new Vector3(-0.9225153f, 0.2407122f, -0.3017003f),
                        CameraRotation = new Rotator(-17.55976f, -8.955016E-07f, 75.37587f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("westaptRest1", new Vector3(-304.5185f, 1739.182f, 97.74178f), 167.1969f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-307.0757f, 1737.822f, 98.82848f),
                        CameraDirection = new Vector3(0.831084f, 0.3339785f, -0.4446996f),
                        CameraRotation = new Rotator(-26.40412f, 3.336246E-05f, -68.10683f),
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            }, // Top of Westminister Towers
            new ResidenceInterior(133890,"720 Savannah Ave") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("SavannahStandard1",new Vector3(1615.303f, 1019.263f, 33.12494f), 89.94624f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1616.83f, 1020.694f, 33.89681f),
                        CameraDirection = new Vector3(-0.9473535f, -0.3191137f, -0.02622674f),
                        CameraRotation = new Rotator(-1.502854f, 1.120963E-05f, 108.616f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2124429686,new Vector3(1621.135f, 1022.895f, 33.36455f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("SavannahInventory1",new Vector3(1607.327f, 1017.889f, 33.12461f), 180.423f,"Access Items")
                    {
                        UseNavmesh =false,
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(1605.503f, 1019.294f, 34.61258f),
                        CameraDirection = new Vector3(0.5750204f, -0.6332822f, -0.5179819f),
                        CameraRotation = new Rotator(-31.19698f, 4.990542E-06f, -137.7605f)
                },
                    new InventoryInteract("SavannahInventory2",new Vector3(1602.387f, 1017.987f, 33.11329f), 131.7048f,"Access Weapons/Cash")
                    {
                        UseNavmesh =false,
                        CanAccessItems = false,
                        CameraPosition = new Vector3(1602.124f, 1020.257f, 34.44051f),
                        CameraDirection = new Vector3(-0.1358425f, -0.8715345f, -0.4711415f),
                        CameraRotation = new Rotator(-28.10842f, -1.161521E-05f, 171.1408f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("SavannahChange1",new Vector3(1619.787f, 1018.66f, 33.1101f), 83.40388f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1616.711f, 1018.625f, 33.93504f),
                        CameraDirection = new Vector3(0.9594702f, -0.04636775f, -0.2779693f),
                        CameraRotation = new Rotator(-16.13904f, -1.322092E-05f, -92.76675f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("SavannahRest1", new Vector3(1611.408f, 1018.442f, 33.13342f), 178.4906f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(1614.054f, 1019.652f, 34.30737f),
                        CameraDirection = new Vector3(-0.7372941f, -0.5458307f, -0.3980783f),
                        CameraRotation = new Rotator(-23.4581f, -9.306941E-07f, 126.5131f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(113666,"15 Cariboo Ave") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("CaribooStandard1",new Vector3(-1008.025f, 2037.689f, 26.06386f), 357.4294f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1006.337f, 2036.447f, 27.09577f),
                        CameraDirection = new Vector3(-0.726667f, 0.6419539f, -0.2446429f),
                        CameraRotation = new Rotator(-14.16073f, 1.849113E-05f, 48.5419f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855,new Vector3(-1007.584f, 2053.532f, 26.31434f)) { LockWhenClosed = true },
                    new InteriorDoor(2842627855,new Vector3(-1009.093f, 2033.457f, 26.31434f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("CaribooInventory1",new Vector3(-1014.102f, 2049.363f, 26.06386f), 142.2518f,"Access Items")
                    {
                        UseNavmesh =false,
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-1014.629f, 2051.011f, 27.012f),
                        CameraDirection = new Vector3(0.003686385f, -0.9422784f, -0.3348102f),
                        CameraRotation = new Rotator(-19.561f, 8.919094E-07f, -179.7758f)
                },
                    new InventoryInteract("CaribooInventory2",new Vector3(-1009.832f, 2051.472f, 26.06385f), 18.70444f,"Access Weapons/Cash")
                    {
                        UseNavmesh =false,
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-1008.222f, 2051.242f, 27.28655f),
                        CameraDirection = new Vector3(-0.7413149f, 0.4254399f, -0.5190886f),
                        CameraRotation = new Rotator(-31.27114f, -7.991138E-06f, 60.14853f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("CaribooChange1",new Vector3(-1015.112f, 2045.389f, 26.06385f), 268.2375f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1012.408f, 2045.482f, 26.91134f),
                        CameraDirection = new Vector3(-0.9615784f, 0.0235971f, -0.2735143f),
                        CameraRotation = new Rotator(-15.87349f, -1.414645E-06f, 88.59425f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("CaribooRest1", new Vector3(-1002.65f, 2038.69f, 26.06385f), 359.507f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1005.061f, 2038.061f, 27.17891f),
                        CameraDirection = new Vector3(0.8418663f, 0.3707662f, -0.3921651f),
                        CameraRotation = new Rotator(-23.08929f, 1.206557E-05f, -66.23083f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(107522,"13 Emery Street") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("EmeryStandard1",new Vector3(-1715.715f, 420.9994f, 7.237865f), 92.47269f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1715.051f, 420.92f, 8.755148f),
                        CameraDirection = new Vector3(-0.9860452f, 0.005240227f, -0.1663955f),
                        CameraRotation = new Rotator(-9.578314f, 8.793732E-08f, 89.69551f)
                    },
                    new ToiletInteract("EmeryToilet1",new Vector3(-1712.421f, 427.2001f, 7.206534f), 180.4569f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-1712.548f, 430.6083f, 8.564474f),
                        CameraDirection = new Vector3(-0.04256547f, -0.9398201f, -0.339008f),
                        CameraRotation = new Rotator(-19.81645f, -2.212064E-06f, 177.4068f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855,new Vector3(-1715.748f, 432.6584f, 7.456361f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("EmeryInventory1",new Vector3(-1715.959f, 428.5735f, 7.206457f), 88.65854f,"Access Items")
                    {
                        UseNavmesh =false,
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-1715.614f, 430.8f, 8.335403f),
                        CameraDirection = new Vector3(-0.4809253f, -0.7953263f, -0.3690081f),
                        CameraRotation = new Rotator(-21.65446f, 2.755809E-06f, 148.8391f)
                },
                    new InventoryInteract("EmeryInventory2",new Vector3(-1712.775f, 431.3138f, 11.37088f), 306.6408f,"Access Weapons/Cash")
                    {
                        UseNavmesh =false,
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-1716.184f, 430.8048f, 12.61184f),
                        CameraDirection = new Vector3(0.9585665f, 0.04204297f, -0.2817494f),
                        CameraRotation = new Rotator(-16.36464f, -2.280169E-06f, -87.4886f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("EmeryChange1",new Vector3(-1708.668f, 419.8671f, 11.37088f), 127.5932f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1711.149f, 417.8415f, 11.94914f),
                        CameraDirection = new Vector3(0.7738792f, 0.5986655f, -0.2066656f),
                        CameraRotation = new Rotator(-11.92702f, 4.363058E-06f, -52.27481f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("EmeryRest1", new Vector3(-1715.863f, 417.0478f, 11.57867f), 148.7325f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(-1715.828f, 420.472f, 12.95096f),
                        CameraDirection = new Vector3(-0.09780149f, -0.8892375f, -0.4468687f),
                        CameraRotation = new Rotator(-26.54296f, 0f, 173.7236f),
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },

            new ResidenceInterior(19202,"GGJ Projects 2, Apt 1a") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GGJP2Apt1aStandard1",new Vector3(104.1977f, 1986.885f, 22.80155f), 43.4018f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(107.113f, 1984.374f, 24.41933f),
                        CameraDirection = new Vector3(-0.666633f, 0.7076033f, -0.2343035f),
                        CameraRotation = new Rotator(-13.55057f, 0f, 43.29234f)
                    },
                    new ToiletInteract("GGJP2Apt1aToilet1",new Vector3(106.8638f, 1995.302f, 22.77724f), 45.99881f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(110.0107f, 1994.754f, 23.78054f),
                        CameraDirection = new Vector3(-0.911991f, 0.2589934f, -0.3181114f),
                        CameraRotation = new Rotator(-18.54875f, 4.502772E-07f, 74.14616f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537,new Vector3(111.0986f, 1994.754f, 23.0294f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP2Apt1aInventory1",new Vector3(107.6104f, 1984.648f, 22.77727f), 275.4927f,"Access Items")
                    {
                        UseNavmesh =false,
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(105.5166f, 1982.919f, 23.83658f),
                        CameraDirection = new Vector3(0.8047788f, 0.4365766f, -0.4021593f),
                        CameraRotation = new Rotator(-23.71323f, 1.119006E-05f, -61.52104f)
                },
                    new InventoryInteract("GGJP2Apt1aInventory2",new Vector3(112.0697f, 1989.907f, 22.77816f), 316.3353f,"Access Weapons/Cash")
                    {
                        UseNavmesh =false,
                        CanAccessItems = false,
                        CameraPosition = new Vector3(109.927f, 1990.334f, 23.79329f),
                        CameraDirection = new Vector3(0.9209683f, -0.1605224f, -0.3550352f),
                        CameraRotation = new Rotator(-20.7956f, -5.936258E-06f, -99.88718f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP2Apt1aChange1",new Vector3(104.1069f, 1993.195f, 22.77724f), 227.6901f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(106.3141f, 1991.006f, 23.27178f),
                        CameraDirection = new Vector3(-0.6670195f, 0.7376567f, -0.1046303f),
                        CameraRotation = new Rotator(-6.005866f, 1.781358E-05f, 42.12118f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP2Apt1aRest1", new Vector3(109.3626f, 1987.965f, 23.21483f), 313.3011f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(113.3811f, 1990.212f, 24.34978f),
                        CameraDirection = new Vector3(-0.8406711f, -0.3858223f, -0.3800175f),
                        CameraRotation = new Rotator(-22.33477f, -2.584453E-05f, 114.6525f),
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(71426,"GGJ Projects 2, Apt 1b") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GGJP2Apt1aStandard1",new Vector3(103.5268f, 2019.582f, 22.77749f), 223.0466f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(101.9251f, 2021.157f, 23.58645f),
                        CameraDirection = new Vector3(0.6932488f, -0.7203354f, -0.02286982f),
                        CameraRotation = new Rotator(-1.310459f, -1.601245E-07f, -136.0977f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537,new Vector3(116.0862f, 2005.068f, 23.02524f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP2Apt1bInventory1",new Vector3(106.2975f, 2023.26f, 22.78132f), 351.8874f,"Access Items")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(107.9747f, 2021.764f, 23.60797f),
                        CameraDirection = new Vector3(-0.6425394f, 0.7501521f, -0.1562533f),
                        CameraRotation = new Rotator(-8.989489f, -1.772001E-05f, 40.58156f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP2Apt1bChange1",new Vector3(106.4752f, 2012.731f, 22.77748f), 46.15971f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(104.1357f, 2014.984f, 23.33867f),
                        CameraDirection = new Vector3(0.6704726f, -0.7317045f, -0.1227802f),
                        CameraRotation = new Rotator(-7.052581f, -6.452118E-07f, -137.5005f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP2Apt1bRest1", new Vector3(112.4748f, 2016.692f, 22.79455f), 225.3645f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(112.8796f, 2018.376f, 23.65995f),
                        CameraDirection = new Vector3(-0.04321926f, -0.9654846f, -0.2568493f),
                        CameraRotation = new Rotator(-14.88319f, 1.656395E-07f, 177.4369f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(140290,"GGJ Projects 3, Apt 1a") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GGJPApt1aStandard1",new Vector3(144.2413f, 1965.523f, 22.7653f), 44.33807f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(145.6135f, 1964.092f, 23.73909f),
                        CameraDirection = new Vector3(-0.684735f, 0.7193161f, -0.1171429f),
                        CameraRotation = new Rotator(-6.727238f, -1.934308E-06f, 43.58912f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537,new Vector3(146.4752f, 1963.102f, 21.77833f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1aInventory1",new Vector3(137.6043f, 1966.823f, 22.76522f), 178.5916f,"Access Items")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(135.8469f, 1968.431f, 23.56095f),
                        CameraDirection = new Vector3(0.4838185f, -0.8510178f, -0.2041775f),
                        CameraRotation = new Rotator(-11.78136f, -2.616439E-06f, -150.381f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1aChange1",new Vector3(138.6325f, 1971.373f, 22.76522f), 229.4285f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(140.8333f, 1969.533f, 23.22832f),
                        CameraDirection = new Vector3(-0.6969023f, 0.7025919f, -0.1438463f),
                        CameraRotation = new Rotator(-8.270475f, -3.882358E-06f, 44.76707f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1aRest1", new Vector3(142.116f, 1972.937f, 22.76522f), 44.32032f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(140.606f, 1969.972f, 23.54162f),
                        CameraDirection = new Vector3(0.2148467f, 0.9572635f, -0.1936171f),
                        CameraRotation = new Rotator(-11.16395f, -2.393163E-06f, -12.64975f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(90114,"GGJ Projects 3, Apt 1b") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GGJP3Apt1bStandard1",new Vector3(170.4337f, 1963.027f, 22.76711f), 135.921f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(172.0459f, 1963.437f, 23.50354f),
                        CameraDirection = new Vector3(-0.9412518f, -0.3331325f, -0.05538824f),
                        CameraRotation = new Rotator(-3.175137f, 1.175744E-06f, 109.4901f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537,new Vector3(157.9027f, 1958.24f, 23.01703f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1bInventory1",new Vector3(176.0475f, 1967.958f, 22.76709f), 278.1388f,"Access Items")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(174.2188f, 1966.405f, 23.56458f),
                        CameraDirection = new Vector3(0.8634548f, 0.4911701f, -0.1148818f),
                        CameraRotation = new Rotator(-6.596808f, -4.29732E-07f, -60.36692f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1bChange1",new Vector3(164.7583f, 1967.143f, 22.76706f), 313.8317f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(166.6939f, 1969.095f, 23.32302f),
                        CameraDirection = new Vector3(-0.7270162f, -0.6586128f, -0.1941046f),
                        CameraRotation = new Rotator(-11.19242f, 2.610979E-06f, 132.1738f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1bRest1", new Vector3(170.6703f, 1972.494f, 22.90263f), 0.947769f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(172.5779f, 1969.466f, 24.1105f),
                        CameraDirection = new Vector3(-0.3994308f, 0.8487614f, -0.3464955f),
                        CameraRotation = new Rotator(-20.27311f, -7.281251E-06f, 25.20188f),
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },
            new ResidenceInterior(62210,"GGJ Projects 3, Apt 1a") {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("GGJP3Apt1cStandard1",new Vector3(163.4809f, 1946.127f, 22.76542f), 222.8452f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(161.7444f, 1947.776f, 23.48733f),
                        CameraDirection = new Vector3(0.6831025f, -0.7196007f, -0.1246827f),
                        CameraRotation = new Rotator(-7.16243f, -6.453662E-07f, -136.4905f)
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537,new Vector3(159.4342f, 1949.132f, 23.01737f)) { LockWhenClosed = true },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1cInventory1",new Vector3(169.5598f, 1945.746f, 22.76522f), 7.653904f,"Access Items")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(171.4033f, 1944.333f, 23.53524f),
                        CameraDirection = new Vector3(-0.6420149f, 0.7355039f, -0.2164509f),
                        CameraRotation = new Rotator(-12.50066f, 4.809778E-06f, 41.11742f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1cChange1",new Vector3(168.1206f, 1941.136f, 22.76522f), 43.79533f,"Change Outfit")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(166.1289f, 1942.973f, 23.28017f),
                        CameraDirection = new Vector3(0.6770111f, -0.7154508f, -0.1725864f),
                        CameraRotation = new Rotator(-9.938233f, -6.06746E-06f, -136.5813f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1cRest1", new Vector3(164.8323f, 1939.419f, 22.76522f), 225.0215f,"Sleep")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(166.5226f, 1941.756f, 23.70206f),
                        CameraDirection = new Vector3(-0.345157f, -0.8860653f, -0.3094429f),
                        CameraRotation = new Rotator(-18.02566f, -4.938128E-06f, 158.7171f),
                        StartAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false,
                    },
                },
            },

        });


    }


    private void Restaurants()
    {


    }

}
