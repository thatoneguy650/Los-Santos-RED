using LosSantosRED.lsr.Helper;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;


public class Interiors_LibertyPP
{
    private Interiors LSInteriors;
    private PossibleInteriors LibertyCityInteriors;

    public Interiors_LibertyPP(Interiors interiors)
    {
        LSInteriors = interiors;
    }

    public void DefaultConfig()
    {
        LibertyCityInteriors = new PossibleInteriors();

        Banks();
        Bars();
        Bowling();
        Businesses();
        Cafes();
        Clubs();
        GangDens();
        GeneralInteriors();
        Hotels();
        Residence();
        Restaurants();
        Stations();
        Stores();

        PossibleInteriors lppInteriors = LibertyCityInteriors.Copy();

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
                IsTrespassingWhenClosed = true,
                //SearchLocations = new List<Vector3>(){new Vector3(217.8289f, 27.17734f, 15.40732f),new Vector3(236.4546f, 23.64848f, 15.40731f),new Vector3(232.7605f, 19.77981f, 8.907292f)},
                IsWeaponRestricted = true,Doors =  new List<InteriorDoor>() {

                    new InteriorDoor(3863802474,new Vector3(5160.267f, 3719.198f, 15.64099f)) { LockWhenClosed = true, InteractPostion = new Vector3(5159.714f, -3720.703f, 15.40793f), InteractHeader =  268.4283f },//Left 1
                    new InteriorDoor(866127123,new Vector3(5160.267f, -3722.2f, 15.64099f)) { LockWhenClosed = true, InteractPostion = new Vector3(5159.714f, -3720.703f, 15.40793f), InteractHeader =  268.4283f },//Left 2

                    new InteriorDoor(3863802474,new Vector3(5160.267f, -3723.186f, 15.64099f)) { LockWhenClosed = true, InteractPostion = new Vector3(5159.672f, -3724.748f, 15.40793f), InteractHeader =  273.206f },//Right 1
                    new InteriorDoor(866127123,new Vector3(5160.267f, -3726.188f, 15.64099f)) { LockWhenClosed = true, InteractPostion = new Vector3(5159.672f, -3724.748f, 15.40793f), InteractHeader =  273.206f },//Right 2
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("bolDrawer1",new Vector3(5181.127f, -3724.557f, 15.40724f), 127.8818f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("bolDrawer2",new Vector3(5178.438f, -3720.24f, 15.40724f), 124.4948f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("bolDrawer3",new Vector3(5175.608f, -3716.002f, 15.40724f), 126.7362f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bolVault1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5170.938f, -3727.942f, 8.907451f),360f-270.6115f,-.4f), //1
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5167.293f, -3727.923f, 8.907451f),360f-87.8942f,-.4f), //2
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5167.32f, -3729.148f, 8.907451f),360f-88.48374f,-.4f),  //3
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5167.285f, -3730.907f, 8.907451f),360f-91.22388f,-.4f), //4
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5167.184f, -3732.33f, 8.907451f),360f-91.82407f,-.4f), //5
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5168.667f, -3733.277f, 8.907451f),360f-182.9932f,-.4f),  //6
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
                     new MoneyTheftInteract("BOLSmallSafe",new Vector3(5170.348f, -3733.344f, 8.907451f), 269.3103f,"Steal from Safe")//only here for example
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
                        Position = NativeHelper.GetOffsetPosition(new Vector3(5170.341f, -3732.385f, 8.907451f),360f-269.0598f,-.4f),  //9
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
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        //Bars
        {
            new Interior(54018, "Lucky Winkles Bar")
        {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4754.003f, -2805.612f, 10.39499f), 178.2276f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(4114682006,new Vector3(4747.633f, -2799.764f, 10.64464f)) { LockWhenClosed = true, InteractPostion = new Vector3(4746.291f, -2800.276f, 10.35966f), InteractHeader =  227.6646f },
                },
        },
            new Interior(80386, "Steinway Beer Garden")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6331.574f, -2515.158f, 35.51511f), 1.348893f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(261592072,new Vector3(6337.857f, -2520.987f, 35.76452f)) { LockWhenClosed = true, InteractPostion = new Vector3(6339.181f, -2520.392f, 35.39922f), InteractHeader =  53.52141f },
                },
            },
            new Interior(164866, "Comrades Bar")
             {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6123.848f, -3746.413f, 15.48475f), 358.3104f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(387699963,new Vector3(6116.591f, -3745.581f, 15.73469f)) { LockWhenClosed = true, InteractPostion = new Vector3(6115.893f, -3746.93f, 15.31808f), InteractHeader =  269.8689f },//front
                    new InteriorDoor(387699963,new Vector3(6135.839f, -3747.724f, 15.73484f)) { LockWhenClosed = true, InteractPostion = new Vector3(6136.479f, -3749.041f, 15.48583f), InteractHeader =  89.2451f },//rear
                    new InteriorDoor(387699963,new Vector3(6131.428f, -3749.222f, 15.73488f)) { LockWhenClosed = false },//inside, leaving unlocked
                },
            },
        });
    }
    private void Bowling()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Bowling
            new Interior(38146, "Memory Lanes")//27394
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6390.766f, -3930.662f, 16.4449f), 134.424f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6389.152f, -3910.837f, 16.84486f), 96.29797f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(6390.102f, -3911.347f, 16.84486f), 184.7186f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3477273845, new Vector3(6385.566f, -3934.577f, 16.70292f)) { LockWhenClosed = true, InteractPostion = new Vector3(6387.097f, -3935.072f, 16.44081f), InteractHeader =  359.2093f },//Left
                    new InteriorDoor(2464449593, new Vector3(6388.566f, -3934.577f, 16.70292f)) { LockWhenClosed = true, InteractPostion = new Vector3(6387.097f, -3935.072f, 16.44081f), InteractHeader =  359.2093f },//Right
                },
            }, //Firefly Island, Broker
            new Interior(92162, "Memory Lanes")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {

                   new MoneyTheftInteract("CashRegister1",new Vector3(6390.794f, -3930.686f, 16.4449f), 135.172f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6389.124f, -3910.854f, 16.84486f), 96.50331f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(6390.102f, -3911.385f, 16.84486f), 186.001f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3477273845, new Vector3(4614.224f, -3179.836f, 5.075127f)) { LockWhenClosed = true, InteractPostion = new Vector3(-334.9954f, 571.699f, 4.812612f), InteractHeader =  85.92f },//Left
                    new InteriorDoor(2464449593, new Vector3(4614.224f, -3176.836f, 5.075127f)) { LockWhenClosed = true, InteractPostion = new Vector3(-334.9954f, 571.699f, 4.812612f), InteractHeader =  85.92f },//Right
                },
            }, //Golden Pier, Westminster, Algonquin
        });
    }
    private void Businesses()
    {
        LibertyCityInteriors.BusinessInteriors.AddRange(new List<BusinessInterior>()
        {
            //Laundromats
            new BusinessInterior(37634, "Laundromat")
            {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("LaundromatStandard1",new Vector3(6271.416f, -2605.745f, 38.71414f), 179.2222f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(6273.699f, -2626.862f, 42.43004f),
                        CameraDirection = new Vector3(-0.3726866f, 0.8860272f, -0.2757909f),
                        CameraRotation = new Rotator(-16.00915f, 4.441104E-06f, 22.81292f)
                    },
                },
            //    Doors = new List<InteriorDoor>()
            //    {
            //        new InteriorDoor(4062767778, new Vector3(1320.801f, 1129.155f, 38.95517f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear_door
            //        new InteriorDoor(1049657988, new Vector3(1317.837f, 1146.426f, 38.96228f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },//rear_door
            //    },

            }, // Harrison Street, East Island City, Dukes.
            new BusinessInterior(150530, "Laundromat")
            {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("LaundromatStandard2",new Vector3(4146.854f, -2115.822f, 13.72556f), 177.9467f ,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(4148.242f, -2136.135f, 15.65503f),
                        CameraDirection = new Vector3(-0.3089745f, 0.9195166f, -0.2429485f),
                        CameraRotation = new Rotator(-14.06063f, 1.012165E-05f, 18.5733f)
                    },
                },
            }, // Hubbard Avenue, Alderney City, Alderney.
            new BusinessInterior(158466, "Laundromat")
            {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("LaundromatStandard2",new Vector3(6216.696f, -3578.553f, 20.21691f), 99.26854f,"Interact")
                    {
                        UseNavmesh =false,
                        CameraPosition = new Vector3(6196.794f, -3584.113f, 23.00935f),
                        CameraDirection = new Vector3(0.7721155f, 0.5827978f, -0.2533466f),
                        CameraRotation = new Rotator(-14.67564f, 3.265498E-05f, -52.95426f)
                    },
                },
            }, // Oneida Avenue, Hove Beach, Broker.
        });
    }
    private void Cafes()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Diner - Cafe
            new Interior(101634, "69th Street Diner")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6076.702f, -3743.14f, 15.88297f), 2.342958f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1285262331, new Vector3(6073.336f, -3738.095f, 16.15447f)) { LockWhenClosed = true, InteractPostion = new Vector3(6071.963f, -3737.404f, 15.88346f), InteractHeader =  179.4005f },//front
                    new InteriorDoor(3232592925,new Vector3(6079.913f, -3741.698f, 16.14832f)){ LockWhenClosed = true, InteractPostion = new Vector3(6080.514f, -3740.345f, 15.88315f), InteractHeader =  90.53715f },//side
                },
            },
            new Interior(121346,"Homebrew Cafe")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                Doors = new List<InteriorDoor>()
                {// 1713.114 Y:547.4352 Z:25.46173
                    new InteriorDoor(1542565804,new Vector3(6663.111f, -3202.563f, 25.46174f)) { LockWhenClosed = true, InteractPostion = new Vector3(6663.716f, -3201.22f, 25.15536f), InteractHeader =  89.3888f },
                },// mlo coords 14.60596f, -9.044677f, -2.845501f
            },
            //Internet Cafe
            new Interior(50178, "tw@ - Broker")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6156.243f, -3424.086f, 24.19375f), 198.2033f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(149395267,new Vector3(6163.554f, -3426.709f, 24.44369f)) { LockWhenClosed = true, InteractPostion = new Vector3(6163.793f, -3425.188f, 23.94148f), InteractHeader =  110.7947f },
                },
            },
            new Interior(166914, "tw@ - Bercham")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(3610.003f, -2798.179f, 25.44385f), 333.1547f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(149395267,new Vector3(3606.741f, -2791.144f, 25.6938f)) { LockWhenClosed = true, InteractPostion = new Vector3(3605.61f, -2791.997f, 25.44485f), InteractHeader =  239.1357f },
                },
            },
            new Interior(66562, "tw@ - North Holland")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4854.961f, -1867.716f, 12.9132f), 5.688653f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
            Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(149395267,new Vector3(4848.791f, -1862.992f, 13.16315f)) { LockWhenClosed = true, InteractPostion = new Vector3(4848.162f, -1864.421f, 12.93218f), InteractHeader =  272.0392f },
                },
            },
            //Superstar Cafe
            new Interior(71682, "Superstar Cafe - The Triangle")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4954.248f, -3203.228f, 15.70811f), 358.9371f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(149395267, new Vector3(4952.427f, -3215.935f, 14.95663f)) { LockWhenClosed = true, InteractPostion = new Vector3(4953.825f, -3216.634f, 14.70763f), InteractHeader =  2.453143f },
                },
            },
            new Interior(133890, "Superstar Cafe - Lancaster")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(5202.569f, -2274.862f, 15.64894f), 91.12072f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(149395267, new Vector3(5215.251f, -2276.726f, 14.89747f)) { LockWhenClosed = true, InteractPostion = new Vector3(5215.852f, -2275.279f, 14.66417f), InteractHeader =  89.95391f },
                },
            },
        });
    }
    private void Clubs()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Clubs
            new Interior(130818, "Bahama Mamas")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4792.932f, -2848.032f, 5.669321f), 0.9953489f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(4796.354f, -2844.621f, 5.669316f), 75.03561f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(4800.179f, -2817.002f, 6.169299f), 179.1182f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister4",new Vector3(4792.75f, -2816.978f, 6.169302f), 178.8593f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("OfficeDrawer",new Vector3(4814.029f, -2821.568f, 6.1693f), 90.4517f,"Rob")
                   {
                        CashMinAmount = 2500,
                        CashMaxAmount = 5000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 500,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
            },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(725112888, new Vector3(4789.874f, -2858.713f, 14.65745f)) { LockWhenClosed = true, InteractPostion = new Vector3(4788.51f, -2859.265f, 14.40985f), InteractHeader =  2.245278f },
                },
            },
            new Interior(12034, "Hercules")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4755.429f, -2901.592f, 11.71165f), 268.4433f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
            },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1316667213, new Vector3(4747.892f, -2898.61f, 11.96499f)) { LockWhenClosed = true, InteractPostion = new Vector3(4747.268f, -2899.94f, 11.71259f), InteractHeader =  269.6075f },
                },
            },
            new Interior(126210, "Maisonette 9")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4705.901f, -3097.282f, 7.683853f), 179.3201f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(4704.072f, -3097.301f, 7.683853f), 181.8977f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("OfficeDrawer",new Vector3(4716.355f, -3109.481f, 9.858822f), 268.3863f,"Rob")
                   {
                        CashMinAmount = 2500,
                        CashMaxAmount = 10000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 500,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
            },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(4119540397, new Vector3(4720.208f, -3103.662f, 10.14345f)) { LockWhenClosed = true, InteractPostion = new Vector3(4720.956f, -3102.299f, 9.859115f), InteractHeader =  90.58852f },
                },
            },
            //Strip Club
            new Interior(113666, "The Triangle Club")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6378.61f, -1552.915f, 17.72205f), 315.8536f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6377.956f, -1552.309f, 17.72205f), 314.7484f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("OfficeDrawer",new Vector3(6388.311f, -1560.575f, 17.72195f), 312.867f,"Rob")
                   {
                        CashMinAmount = 2500,
                        CashMaxAmount = 5000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 500,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(358597415, new Vector3(6386.133f, -1545.739f, 17.97499f)) { LockWhenClosed = true, InteractPostion = new Vector3(6385.672f, -1544.248f, 17.48218f), InteractHeader =  138.2046f },//main
                    new InteriorDoor(2881168431, new Vector3(6331.678f, -1586.206f, 16.97499f)) { LockWhenClosed = true, InteractPostion = new Vector3(6332.205f, -1587.66f, 16.71487f), InteractHeader =  316.3538f },//rear
                },
            }, //Northern Gardens, Bohan
            new Interior(67586, "Honkers Gentlemen's Club")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(3616.392f, -3243.243f, 10.01033f), 358.7121f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(3625.39f, -3243.279f, 10.01033f), 358.7303f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(3625.343f, -3247.949f, 10.01034f), 181.2999f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister4",new Vector3(3612.327f, -3244.988f, 10.01033f), 86.42935f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
            },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2881168431, new Vector3(3608.07f, -3230.456f, 10.26201f)) { LockWhenClosed = true, InteractPostion = new Vector3(3607.568f, -3229.108f, 10.01727f), InteractHeader =  272.6192f },
                },
            }, //Tudor, Alderney 
        });
    }
    private void GangDens()
    {
        LibertyCityInteriors.GangDenInteriors.AddRange(new List<GangDenInterior>()
        {

            new GangDenInterior(398593,"LOST M.C. Clubhouse")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1643309849,new Vector3(3473.45f, -2902.721f, 26.41543f)) // front door
                    {
                        LockWhenClosed = true, CanBeForcedOpenByPlayer = false,
                    },
                    new InteriorDoor(1643309849,new Vector3(3461.776f, -2914.469f, 26.58377f)) // side door
                    {
                        LockWhenClosed = true, CanBeForcedOpenByPlayer = false,
                    },
                    new InteriorDoor(4233865074,new Vector3(3467.053f, -2902.548f, 26.59354f))  // inside door
                    {
                        LockWhenClosed = false,
                    },
                    new InteriorDoor(807349477,new Vector3(3458.663f, -2930.226f, 42.65924f))  // roof door
                    {
                        LockWhenClosed = true, CanBeForcedOpenByPlayer = false,
                    },
                },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("lostclubhouseStandard1",new Vector3(3472.254f, -2909.05f, 26.34175f), 240.7863f,"Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3469.961f, -2909.845f, 28.30681f),
                        CameraDirection = new Vector3(0.984137f, 0.1675837f, -0.05822409f),
                        CameraRotation = new Rotator(-3.337882f, -1.721139E-05f, -80.33609f)
                    },

                    new ToiletInteract("lostclubhouseToiletl1",new Vector3(3473.675f, -2915.979f, 26.34175f), 152.7634f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3472.876f, -2917.777f, 27.22837f),
                        CameraDirection = new Vector3(0.5204769f, 0.7612571f, -0.3867705f),
                        CameraRotation = new Rotator(-22.7537f, -3.7033E-06f, -34.36069f),
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("lostclubhouseRest1", new Vector3(3468.464f, -2922.389f, 26.57759f), 87.98154f,"Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3466.316f, -2921.234f, 27.52811f),
                        CameraDirection = new Vector3(0.7403796f, -0.5172898f, -0.4292426f),
                        CameraRotation = new Rotator(-25.41951f, 1.890575E-06f, -124.9413f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    },
                },
            },
            new GangDenInterior(555777,"Angels Of Death Clubhouse")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1292729623,new Vector3(4565.156f, -2049.616f, 6.348628f)) // front door l
                    {
                        LockWhenClosed = true, CanBeForcedOpenByPlayer = false,
                    },
                    new InteriorDoor(566666890,new Vector3(4561.536f, -2049.603f, 6.348628f)) // front door r
                    {
                        LockWhenClosed = true, CanBeForcedOpenByPlayer = false,
                    },
                    new InteriorDoor(807349477,new Vector3(4570.098f, -2052.992f, 6.351658f)) // inside door 
                    {
                        LockWhenClosed = false, CanBeForcedOpenByPlayer = false,
                    },
                    new InteriorDoor(384356690,new Vector3(4564.622f, -2062.965f, 6.356998f)) // inside door 
                    {
                        LockWhenClosed = false, CanBeForcedOpenByPlayer = false,
                    },
                },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("aodclubhouseStandard1",new Vector3(4566.753f, -2057.878f, 6.099845f), 90.69704f,"Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4567.76f, -2054.997f, 7.594522f),
                        CameraDirection = new Vector3(-0.3271181f, -0.915049f, -0.2359642f),
                        CameraRotation = new Rotator(-13.64847f, -7.467957E-06f, 160.3287f)
                    },

                    new ToiletInteract("aodclubhouseToiletl1",new Vector3(4560.809f, -2071.146f, 7.500951f), 357.0692f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4561.576f, -2069.895f, 8.350286f),
                        CameraDirection = new Vector3(-0.4779177f, -0.7564526f, -0.4465133f),
                        CameraRotation = new Rotator(-26.5202f, 2.862524E-06f, 147.7158f),
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("aodclubhouseRest1", new Vector3(4557.133f, -2062.265f, 10.553f), 265.8946f,"Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4559.034f, -2064.471f, 11.23346f),
                        CameraDirection = new Vector3(-0.5590795f, 0.7525795f, -0.3479284f),
                        CameraRotation = new Rotator(-20.36066f, 2.823081E-05f, 36.60808f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    },
                },
            },
        });
    }
    private void GeneralInteriors()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //new Interior(76290,"Perestroika")

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
        });
    }
    private void Hotels()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Hotel
            new Interior(3330, "The Majestic Hotel Lobby"),
            new Interior(41218, "The Majestic Hotel Elevator Floor"),
            new Interior(148994, "The Majestic Hotel Room"),

        });
    }
    private void Residence()
    {
        LibertyCityInteriors.ResidenceInteriors.AddRange(new List<ResidenceInterior>()
        {
            new ResidenceInterior(152578,"Playboy X Penthouse") {
                IsTeleportEntry = true,
                ClearPositions = new List<Vector3>()
                {
                    new Vector3(4765.792f, -1788.05225f, 38.51976f),
                    new Vector3(4766.047f, -1789.26416f, 38.51976f),
                    new Vector3(4763.99072f, -1801.84521f, 38.03947f),
                    new Vector3(4764.2627f, -1803.29224f, 38.0385628f),
                    new Vector3(4764.47754f, -1804.47021f, 38.0395f),
                    new Vector3(4765.106f, -1803.13025f, 38.03937f),
                    new Vector3(4766.18555f, -1804.26624f, 38.03947f),
                    new Vector3(4765.961f, -1803.29224f, 38.0392f),
                },
                InteriorEgressPosition = new Vector3(4761.712f, -1788.61816f, 38.9610519f),
                InteriorEgressHeading = 179.2488f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("playboyaptExit1",new Vector3(4761.712f, -1788.61816f, 38.9610519f), 0.1088863f ,"Exit") ,
                    new StandardInteriorInteract("playboyaptStandard1",new Vector3(4763.38037f, -1793.98022f, 38.9662628f), 182.3361f,"Interact")
                    {
                        CameraPosition = new Vector3(4766.6875f, -1791.70215f, 40.3259926f),
                        CameraDirection = new Vector3(-0.7304603f, -0.637247f, -0.24565f),
                        CameraRotation = new Rotator(-14.22025f, 2.642284E-06f, 131.1012f)
                    },
                    new CraftInteriorInteract("Stove",new Vector3(4769.8916f, -1787.07117f, 38.96627f), 1.711152f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(4769.45459f, -1788.83716f, 39.8832321f),
                        CameraDirection = new Vector3(0.44477f, 0.8541482f, -0.2694632f),
                        CameraRotation = new Rotator(-15.63233f, 7.535821E-06f, -27.50679f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("playboyaptInventory1",new Vector3(4771.602f, -1787.38721f, 38.96627f), 353.7243f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(4767.112f, -1788.24023f, 39.98292f),
                        CameraDirection = new Vector3(0.7164276f, 0.5529848f, -0.4253695f),
                        CameraRotation = new Rotator(-25.17406f, 0f, -52.33673f)
    },
                    new InventoryInteract("playboyaptInventory2",new Vector3(4769.531f, -1789.6842f, 38.96627f), 4.749637f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(4766.93945f, -1790.74121f, 40.1288223f),
                        CameraDirection = new Vector3(0.8029016f, 0.4488999f, -0.3922219f),
                        CameraRotation = new Rotator(-23.09282f, -9.281453E-06f, -60.79057f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("playboyaptChange1",new Vector3(4753.241f, -1797.13623f, 38.9663f), 272.0037f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(4756.12744f, -1796.93018f, 39.76433f),
                        CameraDirection = new Vector3(-0.9620676f, -0.02033964f, -0.2720518f),
                        CameraRotation = new Rotator(-15.7864f, 9.870523E-06f, 91.21114f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("playboyaptRest1", new Vector3(4757.998f, -1793.24219f, 38.9663f), 184.5695f,"Sleep")
                    {
                        CameraPosition = new Vector3(4754.94873f, -1793.35913f, 40.3194427f),
                        CameraDirection = new Vector3(0.6897783f, -0.520592f, -0.5031797f),
                        CameraRotation = new Rotator(-30.21059f, -2.667481E-05f, -127.0427f)
                    },
                },
            },
            new ResidenceInterior(142850,"Studio Apartment") {
                IsTeleportEntry = true,
                ClearPositions = new List<Vector3>()
                {
                    new Vector3(5289.847f, -2405.52832f, 44.0535431f),
                    new Vector3(5289.509f, -2403.53516f, 44.0535431f),
                },
                InteriorEgressPosition = new Vector3(5284.05762f, -2404.50317f, 45.04579f),
                InteriorEgressHeading = 267.9149f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("studioaptExit1",new Vector3(5284.05762f, -2404.50317f, 45.04579f), 91.86633f ,"Exit") ,
                    new StandardInteriorInteract("studioaptStandard1",new Vector3(5287.75146f, -2404.08521f, 45.04591f), 270.5879f,"Interact")
                    {
                        CameraPosition = new Vector3(5285.68945f, -2402.15137f, 46.22877f),
                        CameraDirection = new Vector3(0.8424098f, -0.4465208f, -0.301604f),
                        CameraRotation = new Rotator(-17.55397f, -4.477364E-06f, -117.9259f)
                    },
                    new CraftInteriorInteract("Stove",new Vector3(5282.12646f, -2395.71216f, 45.04581f), 93.00458f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5283.76953f, -2396.288f, 46.1488533f),
                        CameraDirection = new Vector3(-0.875756f, 0.2937503f, -0.3830956f),
                    CameraRotation = new Rotator(-22.52556f, 3.74337E-05f, 71.45727f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("studioaptInventory1",new Vector3(5282.354f, -2398.19214f, 45.04592f), 92.57986f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(5283.65869f, -2399.91919f, 45.94081f),
                        CameraDirection = new Vector3(-0.5748014f, 0.7399729f, -0.3493471f),
                        CameraRotation = new Rotator(-20.44739f, 3.189145E-05f, 37.8396f)
                },
                    new InventoryInteract("studioaptInventory2",new Vector3(5287.41748f, -2394.267f, 45.04591f), 3.660645f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(5285.326f, -2395.30713f, 46.00545f),
                        CameraDirection = new Vector3(0.7699414f, 0.520547f, -0.3690811f),
                        CameraRotation = new Rotator(-21.65896f, -2.939621E-05f, -55.9379f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("studioaptChange1",new Vector3(5286.292f, -2413.42725f, 45.04591f), 356.4514f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(5286.17f, -2410.30615f, 45.87831f),
                        CameraDirection = new Vector3(-0.03027195f, -0.9652488f, -0.2595734f),
                        CameraRotation = new Rotator(-15.04475f, 0f, 178.2037f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("studioaptRest1", new Vector3(5290.90234f, -2413.54517f, 45.089f), 359.6932f,"Sleep")
                    {
                        CameraPosition = new Vector3(5294.2915f, -2413.06519f, 46.44382f),
                        CameraDirection = new Vector3(-0.7603666f, 0.4343084f, -0.4829274f),
                        CameraRotation = new Rotator(-28.87677f, 9.750054E-06f, 60.26573f)
                    },
                },
            },
            new ResidenceInterior(39170, "South Bohan Apartment")
            {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(5791.15332f, -1851.01318f, 17.4906235f),
                InteriorEgressHeading = 0.9092442f,
                ClearPositions = new List<Vector3>()
                {
                    new Vector3(5792.80664f, -1847.24023f, 16.4639435f),
                    new Vector3(5786.12549f, -1846.25415f, 16.4760838f),
                    new Vector3(5786.81445f, -1845.08618f, 16.4760838f)
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(261592072, new Vector3(5785.202f, -1855.971f, 11.785f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5784.897f, -1855.763f, 11.48749f),
                        InteractHeader = 270.2112f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("southbohanStandard1", new Vector3(5789.16455f, -1847.80713f, 17.4711227f), 27.44588f, "Interact")
                    {
                        CameraPosition = new Vector3(5789.237f, -1850.52014f, 18.4632339f),
                        CameraDirection = new Vector3(-0.04744998f, 0.9517307f, -0.3032443f),
                        CameraRotation = new Rotator(-17.65257f, 0f, 2.854204f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5797.03467f, -1850.24524f, 17.4711342f), 179.6671f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5796.598f, -1848.54919f, 18.5158138f),
                        CameraDirection = new Vector3(0.002083805f, -0.9392081f, -0.343342f),
                        CameraRotation = new Rotator(-20.08062f, 2.485638E-08f, -179.8729f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("southbohanRest1", new Vector3(5787.504f, -1844.66614f, 17.4711342f), 267.3164f, "Sleep")
                    {
                        CameraPosition = new Vector3(5787.87842f, -1847.41418f, 18.6954842f),
                        CameraDirection = new Vector3(0.4276391f, 0.7953158f, -0.4296482f),
                        CameraRotation = new Rotator(-25.44524f, 1.701881E-05f, -28.26679f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("southbohanInventory1", new Vector3(5797.309f, -1847.81213f, 17.4711342f), 271.6566f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        CameraPosition = new Vector3(5795.745f, -1845.67419f, 18.6528931f),
                        CameraDirection = new Vector3(0.5751348f, -0.6988618f, -0.4252202f),
                        CameraRotation = new Rotator(-25.1646f, -1.980935E-05f, -140.547f)
                    },
                    new InventoryInteract("southbohanInventory2", new Vector3(5788.647f, -1850.21021f, 17.4711342f), 179.147f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        CameraPosition = new Vector3(5790.80664f, -1848.59924f, 18.6786137f),
                        CameraDirection = new Vector3(-0.6846707f, -0.5983276f, -0.4162093f),
                        CameraRotation = new Rotator(-24.59549f, 4.694836E-06f, 131.1499f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("southbohanChange1", new Vector3(5791.36572f, -1843.9502f, 17.4711342f), 180.2521f, "Change Outfit")
                    {
                        CameraPosition = new Vector3(5791.50244f, -1846.83618f, 18.320364f),
                        CameraDirection = new Vector3(0.02624621f, 0.9549722f, -0.2955321f),
                        CameraRotation = new Rotator(-17.18945f, 2.792789E-08f, -1.574306f)
                    }
                }
            },//South Bohan Safehouse
            new ResidenceInterior(46850, "Broker Apartment")
            {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(6080.821f, -3757.16528f, 19.4021645f),
                InteriorEgressHeading = 1.34244f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2181386400, new Vector3(6084.177f, -3760.533f, 15.40443f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(6084.77f, -3761.947f, 15.19345f),
                        InteractHeader = 89.08341f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("brokeraptStandard1", new Vector3(6080.401f, -3754.08032f, 19.4021645f), 1.176553f, "Interact")
                    {
                        CameraPosition = new Vector3(6078.172f, -3757.21875f, 20.6965141f),
                        CameraDirection = new Vector3(0.5709521f, 0.787036f, -0.2336407f),
                        CameraRotation = new Rotator(-13.51151f, 4.390381E-06f, -35.95886f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(6074.832f, -3755.33936f, 19.4108734f), 2.826868f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(6075.299f, -3757.01831f, 20.5027428f),
                        CameraDirection = new Vector3(-0.01129704f, 0.9287074f, -0.3706412f),
                        CameraRotation = new Rotator(-21.75517f, -5.745287E-08f, 0.6969265f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("brokeraptRest1", new Vector3(6079.05371f, -3752.82422f, 19.402174f), 357.6328f, "Sleep")
                    {
                        CameraPosition = new Vector3(6081.93359f, -3752.51929f, 20.6411343f),
                        CameraDirection = new Vector3(-0.7985814f, 0.3459656f, -0.4925197f),
                        CameraRotation = new Rotator(-29.50633f, 9.810093E-07f, 66.57652f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("brokeraptInventory1", new Vector3(6075.941f, -3756.85278f, 19.4108829f), 181.8482f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        CameraPosition = new Vector3(6078.467f, -3755.47974f, 20.8128338f),
                        CameraDirection = new Vector3(-0.7816852f, -0.4300944f, -0.4516492f),
                        CameraRotation = new Rotator(-26.84955f, 7.655484E-06f, 118.8202f)
                    },
                    new InventoryInteract("brokeraptInventory2", new Vector3(6082.68164f, -3752.13452f, 19.402174f), 189.5905f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        CameraPosition = new Vector3(6079.76855f, -3751.99121f, 20.2630138f),
                        CameraDirection = new Vector3(0.9117068f, -0.2928823f, -0.2881157f),
                        CameraRotation = new Rotator(-16.74518f, -4.457903E-07f, -107.8094f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("brokeraptChange1", new Vector3(6081.422f, -3749.22021f, 19.402174f), 182.1172f, "Change Outfit")
                    {
                        CameraPosition = new Vector3(6081.424f, -3751.874f, 20.2533035f),
                        CameraDirection = new Vector3(0.04927624f, 0.9553418f, -0.2913655f),
                        CameraRotation = new Rotator(-16.93973f, -3.346865E-07f, -2.952682f)
                    }
                }
            }, // Broker Safehouse
            new ResidenceInterior(111874, "145 Mahesh Avenue")
            {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(4218.69336f, -2371.82813f, 19.0011234f),
                InteriorEgressHeading = 356.884f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2124429686, new Vector3(4224.403f, -2362.897f, 14.23296f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(4225.711f, -2362.341f, 13.81779f),
                        InteractHeader = 182.0731f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("alderenyaptStandard1", new Vector3(4220.10254f, -2367.933f, 19.0388336f), 313.5496f, "Interact")
                    {
                        CameraPosition = new Vector3(4219.06f, -2370.34131f, 20.2933426f),
                        CameraDirection = new Vector3(0.3834104f, 0.8622515f, -0.3309363f),
                        CameraRotation = new Rotator(-19.32561f, -1.311893E-05f, -23.97289f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(4226.307f, -2364.93018f, 19.0011444f), 270.7166f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(4224.605f, -2365.27734f, 20.0443439f),
                        CameraDirection = new Vector3(0.9395224f, -0.001058639f, -0.3424857f),
                        CameraRotation = new Rotator(-20.02839f, 5.910299E-07f, -90.06456f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("alderenyaptRest1", new Vector3(4218.95f, -2364.2832f, 19.0011234f), 91.93905f, "Sleep")
                    {
                        CameraPosition = new Vector3(4219.809f, -2367.412f, 20.2434444f),
                        CameraDirection = new Vector3(-0.5588966f, 0.7283394f, -0.3964294f),
                        CameraRotation = new Rotator(-23.35515f, -1.022968E-05f, 37.50109f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("alderenyaptInventory1", new Vector3(4224.99658f, -2365.44629f, 19.0011444f), 211.9569f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        CameraPosition = new Vector3(4223.3457f, -2365.05029f, 19.8982544f),
                        CameraDirection = new Vector3(0.8892203f, -0.2438023f, -0.3871017f),
                        CameraRotation = new Rotator(-22.77428f, 0f, -105.3323f)
                    },
                    new InventoryInteract("alderenyaptInventory2", new Vector3(4222.3335f, -2368.62524f, 19.0012226f), 271.1751f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        CameraPosition = new Vector3(4220.93164f, -2366.09521f, 19.9939842f),
                        CameraDirection = new Vector3(0.4645696f, -0.8410434f, -0.2771662f),
                        CameraRotation = new Rotator(-16.09114f, 1.777174E-06f, -151.0849f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("alderenyaptChange1", new Vector3(4217.58057f, -2367.00635f, 19.0011234f), 270.7104f, "Change Outfit")
                    {
                        CameraPosition = new Vector3(4220.204f, -2367.0293f, 19.913393f),
                        CameraDirection = new Vector3(-0.9393548f, 0.03328265f, -0.341328f),
                        CameraRotation = new Rotator(-19.95781f, 1.703107E-06f, 87.97078f)
                    }
                }
            },//Alderney Safehouse
            new ResidenceInterior(1538, "370 Galveston Ave")
            {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(4749.047f, -1866.66418f, 16.4905739f),
                InteriorEgressHeading = 264.2994f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855, new Vector3(4748.421f, -1865.912f, 16.70837f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(4747.839f, -1867.286f, 16.23608f),
                        InteractHeader = 275.0539f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GalvestonStandard1", new Vector3(4752.1416f, -1861.91724f, 16.4740639f), 89.00453f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4752.91064f, -1859.31421f, 17.5844231f),
                        CameraDirection = new Vector3(-0.498876f, -0.802644f, -0.3269332f),
                        CameraRotation = new Rotator(-19.08274f, 5.420512E-06f, 148.1374f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(4752.41064f, -1856.44324f, 16.4830742f), 2.4541f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(4752.8667f, -1858.14221f, 17.4300842f),
                        CameraDirection = new Vector3(-0.008799502f, 0.9579368f, -0.2868444f),
                        CameraRotation = new Rotator(-16.66913f, -1.392539E-08f, 0.5262979f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GalvestonRest1", new Vector3(4755.51563f, -1858.76819f, 16.4563427f), 268.4165f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4755.07373f, -1860.42517f, 17.7559738f),
                        CameraDirection = new Vector3(0.6740843f, 0.4977706f, -0.5457424f),
                        CameraRotation = new Rotator(-33.07541f, 2.343424E-05f, -53.55637f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GalvestonInventory1", new Vector3(4753.678f, -1857.07019f, 16.4830742f), 269.4172f, "Access Items")
                    {
                        UseNavmesh = false,
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        CameraPosition = new Vector3(4753.06348f, -1859.4502f, 17.4691734f),
                        CameraDirection = new Vector3(0.4479685f, 0.8236551f, -0.3477305f),
                        CameraRotation = new Rotator(-20.34856f, -5.463601E-06f, -28.54083f)
                    },
                    new InventoryInteract("GalvestonInventory2", new Vector3(4754.27637f, -1863.87317f, 16.4577427f), 181.9774f, "Access Weapons/Cash")
                    {
                        UseNavmesh = false,
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        CameraPosition = new Vector3(4751.89746f, -1863.34717f, 17.614233f),
                        CameraDirection = new Vector3(0.8676881f, -0.3632791f, -0.3393311f),
                        CameraRotation = new Rotator(-19.83613f, -4.447367E-05f, -112.7178f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GalvestonChange1", new Vector3(4758.954f, -1860.71521f, 16.4567642f), 86.94769f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4756.088f, -1860.76221f, 17.0922642f),
                        CameraDirection = new Vector3(0.9676532f, -0.035804f, -0.2497307f),
                        CameraRotation = new Rotator(-14.46157f, 4.243231E-06f, -92.11903f)
                    }
                }
            }, // Luis Apartment
            new ResidenceInterior(100610, "Middle Park Penthouse")
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(5178.508f, -2458.86816f, 62.8008232f),
                InteriorEgressHeading = 358.9589f,
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract("mppentExit1", new Vector3(5178.512f, -2458.71436f, 62.8008232f), 179.6859f, "Exit")
                    {
                        UseNavmesh = false
                    },
                    new StandardInteriorInteract("mppentStandard1", new Vector3(5164.259f, -2463.55615f, 61.9053421f), 91.19791f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5164.14844f, -2463.496f, 62.83546f),
                        CameraDirection = new Vector3(-0.9422437f, -0.0005598353f, -0.3349276f),
                        CameraRotation = new Rotator(-19.56813f, -2.131829E-05f, 90.03404f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5172.111f, -2475.0332f, 62.52026f), 90.07284f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5173.67529f, -2474.62622f, 63.5728226f),
                        CameraDirection = new Vector3(-0.9374399f, 0.007484701f, -0.3480669f),
                        CameraRotation = new Rotator(-20.36912f, -2.547174E-06f, 89.54255f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("mppentRest1", new Vector3(5163.19043f, -2466.46533f, 61.90533f), 180.8321f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5164.824f, -2464.724f, 63.4448929f),
                        CameraDirection = new Vector3(-0.3359595f, -0.7726297f, -0.5386786f),
                        CameraRotation = new Rotator(-32.59373f, 3.040102E-06f, 156.4993f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("mppentInventory1", new Vector3(5172.58838f, -2471.88428f, 62.52026f), 270.6129f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5171.29834f, -2474.58936f, 63.41704f),
                        CameraDirection = new Vector3(0.6445473f, 0.710347f, -0.2827825f),
                        CameraRotation = new Rotator(-16.42634f, 7.120833E-06f, -42.21965f)
                    },
                    new InventoryInteract("mppentInventory2", new Vector3(5158.18f, -2460.61133f, 62.211853f), 91.67359f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5160.0376f, -2458.3042f, 63.77415f),
                        CameraDirection = new Vector3(-0.7813568f, -0.5301833f, -0.3292222f),
                        CameraRotation = new Rotator(-19.22157f, 4.520897E-06f, 124.1585f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("mppentChange1", new Vector3(5178.46436f, -2468.70117f, 62.5202522f), 83.09912f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5175.0874f, -2468.871f, 63.17467f),
                        CameraDirection = new Vector3(0.9882383f, -0.01377646f, -0.1522997f),
                        CameraRotation = new Rotator(-8.76022f, 8.63851E-07f, -90.79868f)
                    }
                }
            },// Yusuf's Penthouse
            new ResidenceInterior(113154, "Studio Apartment")
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(5992.53174f, -3108.51465f, 29.237524f),
                InteriorEgressHeading = 240.6072f,
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract("studaptExit1", new Vector3(5992.53174f, -3108.51465f, 29.237524f), 58.48744f, "Exit")
                    {
                        UseNavmesh = false
                    },
                    new StandardInteriorInteract("studaptStandard1", new Vector3(5995.984f, -3112.34424f, 29.2375336f), 146.6657f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5995.166f, -3109.48633f, 30.5135632f),
                        CameraDirection = new Vector3(-0.0530019f, -0.951273f, -0.3037606f),
                        CameraRotation = new Rotator(-17.68361f, -1.702621E-05f, 176.811f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(6004.96875f, -3109.07813f, 29.4059734f), 241.9506f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(6003.17773f, -3108.64355f, 30.3389034f),
                        CameraDirection = new Vector3(0.8549131f, -0.4375533f, -0.2786946f),
                        CameraRotation = new Rotator(-16.18231f, 0f, -117.1038f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("studaptRest1", new Vector3(5996.109f, -3116.05176f, 29.237524f), 240.7971f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5995.89063f, -3113.64478f, 31.0693436f),
                        CameraDirection = new Vector3(0.1860044f, -0.8695814f, -0.4574172f),
                        CameraRotation = new Rotator(-27.22057f, -2.928314E-05f, -167.9263f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("studaptInventory1", new Vector3(6003.331f, -3109.728f, 29.4059734f), 150.1994f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6001.6875f, -3108.046f, 30.3496742f),
                        CameraDirection = new Vector3(0.5024242f, -0.790361f, -0.3505701f),
                        CameraRotation = new Rotator(-20.52219f, -1.549769E-05f, -147.5563f)
                    },
                    new InventoryInteract("studaptInventory2", new Vector3(5997.356f, -3122.66016f, 29.237524f), 244.1518f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5995.22656f, -3123.684f, 30.4508743f),
                        CameraDirection = new Vector3(0.84171f, 0.2752596f, -0.464496f),
                        CameraRotation = new Rotator(-27.67761f, -3.374312E-06f, -71.891f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("studaptChange1", new Vector3(5989.43066f, -3112.40747f, 29.2375431f), 199.779f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5990.36865f, -3114.892f, 30.0002632f),
                        CameraDirection = new Vector3(-0.2841462f, 0.9256303f, -0.2499389f),
                        CameraRotation = new Rotator(-14.4739f, 4.408796E-07f, 17.06523f)
                    }
                }
            },// Brucie's Studio Apartment
            new ResidenceInterior(216578, "Single Room")
            {
                IsTeleportEntry = false,
                InteriorEgressPosition = new Vector3(5055.579f, -1877.20923f, 32.79669f),
                InteriorEgressHeading = 87.70292f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(3727781305, new Vector3(5049.668f, -1872.032f, 21.05347f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5049.313f, -1873.395f, 20.8012f),
                        InteractHeader = 270.1318f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("singrmaptStandard1", new Vector3(5049.86133f, -1873.08521f, 32.79673f), 95.84647f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5050.76172f, -1872.33423f, 34.22689f),
                        CameraDirection = new Vector3(-0.8189111f, -0.3715491f, -0.4374197f),
                        CameraRotation = new Rotator(-25.93936f, 7.595365E-06f, 114.4043f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("singrmaptRest1", new Vector3(5052.254f, -1874.84924f, 32.79673f), 359.6346f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5050.392f, -1876.0332f, 34.2433319f),
                        CameraDirection = new Vector3(0.5939716f, 0.6753681f, -0.4371221f),
                        CameraRotation = new Rotator(-25.92041f, 1.898536E-06f, -41.33093f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("singrmaptInventory2", new Vector3(5052.315f, -1876.01624f, 32.80104f), 281.446f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5051.50244f, -1878.21924f, 33.9929f),
                        CameraDirection = new Vector3(0.5709561f, 0.710955f, -0.4105511f),
                        CameraRotation = new Rotator(-24.23946f, -7.490577E-06f, -38.76736f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("singrmaptChange1", new Vector3(5057.53174f, -1877.31116f, 32.79668f), 84.24583f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5054.269f, -1877.31018f, 33.54731f),
                        CameraDirection = new Vector3(0.9709671f, -0.04076166f, -0.2357147f),
                        CameraRotation = new Rotator(-13.63375f, -6.039883E-06f, -92.40389f)
                    }
                }
            }, // Single Room Apartment
            new ResidenceInterior(38402, "Apartment 204")
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(4647.22852f, -1998.96118f, 97.53441f),
                InteriorEgressHeading = 165.7377f,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(106751028, new Vector3(4640.167f, -2006.507f, 97.78454f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(4641.525f, -2006.296f, 101.5324f),
                        InteractHeader = 175.7224f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract("westaptExit1", new Vector3(4647.22852f, -1998.96118f, 97.53441f), 342.735f, "Exit"),
                    new StandardInteriorInteract("westaptStandard1", new Vector3(4643.2876f, -2013.25024f, 97.56591f), 257.585f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4641.39063f, -2014.78613f, 98.67214f),
                        CameraDirection = new Vector3(0.7836698f, 0.5190888f, -0.3411867f),
                        CameraRotation = new Rotator(-19.94919f, -2.724822E-06f, -56.48022f)
                    },
                    new ToiletInteract("westaptToilet1", new Vector3(4640.06f, -2017.93518f, 97.54931f), 166.5865f, "Use Toilet")
                    {
                        UseNavmesh = false,
                        IsStanding = false,
                        CameraPosition = new Vector3(4642.263f, -2017.22314f, 98.8478546f),
                        CameraDirection = new Vector3(-0.8049945f, -0.3821636f, -0.4538005f),
                        CameraRotation = new Rotator(-26.98779f, 1.916217E-05f, 115.3956f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(4637.866f, -2024.01917f, 97.5345f), 351.0685f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(4638.05273f, -2025.62122f, 98.6309738f),
                        CameraDirection = new Vector3(0.1515174f, 0.9152469f, -0.3733171f),
                        CameraRotation = new Rotator(-21.92034f, -2.300771E-07f, -9.399954f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("westaptRest1", new Vector3(4645.428f, -2010.86218f, 97.7415848f), 167.1969f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4642.871f, -2012.22217f, 98.8282852f),
                        CameraDirection = new Vector3(0.831084f, 0.3339785f, -0.4446996f),
                        CameraRotation = new Rotator(-26.40412f, 3.336246E-05f, -68.10683f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("westaptInventory2", new Vector3(4641.546f, -2025.16614f, 97.5345f), 351.6466f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4642.992f, -2026.71021f, 98.84838f),
                        CameraDirection = new Vector3(-0.4048215f, 0.8305874f, -0.3824187f),
                        CameraRotation = new Rotator(-22.48358f, 1.848016E-05f, 25.98422f)
                    },
                    new InventoryInteract("westaptInventory2", new Vector3(4636.572f, -2026.3042f, 97.5345f), 113.1772f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4637.514f, -2023.91821f, 98.765625f),
                        CameraDirection = new Vector3(-0.6273283f, -0.6858361f, -0.368901f),
                        CameraRotation = new Rotator(-21.64785f, 1.010417E-05f, 137.5511f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("westaptChange1", new Vector3(4638.95557f, -2020.8772f, 97.57541f), 256.2012f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(4641.93164f, -2021.40417f, 98.47262f),
                        CameraDirection = new Vector3(-0.9225153f, 0.2407122f, -0.3017003f),
                        CameraRotation = new Rotator(-17.55976f, -8.955016E-07f, 75.37587f)
                    }
                }
            }, // Top of Westminister Towers
            new ResidenceInterior(133890, "720 Savannah Ave")
            {
                IsTeleportEntry = false,
                ClearPositions = new List<Vector3>()
                {
                    new Vector3(6553.841f, -2730.25928f, 32.1130829f),
                    new Vector3(6553.856f, -2729.393f,   32.1130829f),
                    new Vector3(6554.68457f, -2728.4082f, 32.1130829f),
                    new Vector3(6555.572f, -2729.1062f,  32.1130829f),
                    new Vector3(6555.481f, -2729.96826f, 32.1130829f)
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2124429686, new Vector3(6571.136f, -2727.106f, 33.36455f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(6571.72f, -2725.763f, 33.11459f),
                        InteractHeader = 91.04817f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("SavannahStandard1", new Vector3(6565.25f, -2730.78125f, 33.12474f), 89.94624f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6566.777f, -2729.35f, 33.89661f),
                        CameraDirection = new Vector3(-0.9473535f, -0.3191137f, -0.02622674f),
                        CameraRotation = new Rotator(-1.502854f, 1.120963E-05f, 108.616f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(6552.528f, -2732.28613f, 33.1132f), 179.8432f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(6552.124f, -2730.622f, 34.1268921f),
                        CameraDirection = new Vector3(-0.02492519f, -0.9452553f, -0.3253785f),
                        CameraRotation = new Rotator(-18.98851f, -5.643165E-08f, 178.4895f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("SavannahRest1", new Vector3(6561.35449f, -2731.602f, 33.13322f), 178.4906f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6564.001f, -2730.392f, 34.30717f),
                        CameraDirection = new Vector3(-0.7372941f, -0.5458307f, -0.3980783f),
                        CameraRotation = new Rotator(-23.4581f, -9.306941E-07f, 126.5131f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("SavannahInventory1", new Vector3(6557.274f, -2732.15527f, 33.1244125f), 180.423f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6555.44971f, -2730.75024f, 34.61238f),
                        CameraDirection = new Vector3(0.5750204f, -0.6332822f, -0.5179819f),
                        CameraRotation = new Rotator(-31.19698f, 4.990542E-06f, -137.7605f)
                    },
                    new InventoryInteract("SavannahInventory2", new Vector3(6560.39453f, -2729.55713f, 33.1244125f), 5.4308f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6562.653f, -2730.58716f, 34.20805f),
                        CameraDirection = new Vector3(-0.755771f, 0.5409938f, -0.368966f),
                        CameraRotation = new Rotator(-21.65186f, 1.194162E-05f, 54.40427f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("SavannahChange1", new Vector3(6569.734f, -2731.38428f, 33.1099f), 83.40388f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(6566.65771f, -2731.41919f, 33.93484f),
                        CameraDirection = new Vector3(0.9594702f, -0.04636775f, -0.2779693f),
                        CameraRotation = new Rotator(-16.13904f, -1.322092E-05f, -92.76675f)
                    }
                }
            },
            new ResidenceInterior(113666, "15 Cariboo Ave")
            {
                IsTeleportEntry = false,
                ClearPositions = new List<Vector3>()
                {
                    new Vector3(3947.29883f, -1715.76624f, 25.074213f),
                    new Vector3(3949.1958f,  -1714.81213f, 25.074213f),
                    new Vector3(3948.109f,   -1711.51819f, 25.074213f),
                    new Vector3(3944.69287f, -1706.15222f, 25.074213f),
                    new Vector3(3942.34985f, -1700.05029f, 25.074213f),
                    new Vector3(3944.13672f, -1699.37524f, 25.074213f),
                    new Vector3(3946.37476f, -1698.66211f, 25.074213f)
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855, new Vector3(3940.908f, -1716.545f, 26.31433f))// front door
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(3942.314f, -1717.058f, 25.93524f),
                        InteractHeader = 3.393853f
                    },
                    new InteriorDoor(2842627855, new Vector3(3942.417f, -1696.47f, 26.31433f)) // rear door
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(3941.031f, -1696.033f, 25.93525f),
                        InteractHeader = 180.048f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("CaribooStandard1", new Vector3(3941.92188f, -1712.35522f, 26.0636635f), 357.4294f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3943.60986f, -1713.59717f, 27.0955734f),
                        CameraDirection = new Vector3(-0.726667f, 0.6419539f, -0.2446429f),
                        CameraRotation = new Rotator(-14.16073f, 1.849113E-05f, 48.5419f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(3937.80884f, -1697.94727f, 26.0636635f), 3.002773f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(3938.22363f, -1699.30322f, 27.166584f),
                        CameraDirection = new Vector3(-0.02082958f, 0.9259633f, -0.3770387f),
                        CameraRotation = new Rotator(-22.15037f, -5.761282E-08f, 1.288653f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("CaribooRest1", new Vector3(3947.29688f, -1711.35425f, 26.0636539f), 359.507f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3944.88574f, -1711.98315f, 27.1787128f),
                        CameraDirection = new Vector3(0.8418663f, 0.3707662f, -0.3921651f),
                        CameraRotation = new Rotator(-23.08929f, 1.206557E-05f, -66.23083f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("CaribooInventory1", new Vector3(3935.84473f, -1700.68115f, 26.0636635f), 142.2518f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3935.31787f, -1699.0332f, 27.0118027f),
                        CameraDirection = new Vector3(0.003686385f, -0.9422784f, -0.3348102f),
                        CameraRotation = new Rotator(-19.561f, 8.919094E-07f, -179.7758f)
                    },
                    new InventoryInteract("CaribooInventory2", new Vector3(3940.11475f, -1698.57227f, 26.0636539f), 18.70444f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3941.72485f, -1698.80225f, 27.2863541f),
                        CameraDirection = new Vector3(-0.7413149f, 0.4254399f, -0.5190886f),
                        CameraRotation = new Rotator(-31.27114f, -7.991138E-06f, 60.14853f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("CaribooChange1", new Vector3(3934.83472f, -1704.65515f, 26.0636539f), 268.2375f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3937.53882f, -1704.56213f, 26.9111443f),
                        CameraDirection = new Vector3(-0.9615784f, 0.0235971f, -0.2735143f),
                        CameraRotation = new Rotator(-15.87349f, -1.414645E-06f, 88.59425f)
                    }
                }
            },
            new ResidenceInterior(107522, "13 Emery Street")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2842627855, new Vector3(3234.252f, -3317.34f, 7.456365f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(3235.646f, -3316.877f, 7.168783f),
                        InteractHeader = 180.4152f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("EmeryStandard1", new Vector3(3234.232f, -3329.045f, 7.237668f), 92.47269f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3234.89575f, -3329.12427f, 8.754951f),
                        CameraDirection = new Vector3(-0.9860452f, 0.005240227f, -0.1663955f),
                        CameraRotation = new Rotator(-9.578314f, 8.793732E-08f, 89.69551f)
                    },
                    new ToiletInteract("EmeryToilet1", new Vector3(3237.526f, -3322.844f, 7.206337f), 180.4569f, "Use Toilet")
                    {
                        UseNavmesh = false,
                        IsStanding = false,
                        CameraPosition = new Vector3(3237.399f, -3319.43579f, 8.564277f),
                        CameraDirection = new Vector3(-0.04256547f, -0.9398201f, -0.339008f),
                        CameraRotation = new Rotator(-19.81645f, -2.212064E-06f, 177.4068f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(3233.7627f, -3322.278f, 7.206261f), 91.27723f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(3235.31079f, -3321.944f, 8.32634449f),
                        CameraDirection = new Vector3(-0.9214435f, 0.03472181f, -0.3869577f),
                        CameraRotation = new Rotator(-22.76533f, 2.893449E-07f, 87.842f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("EmeryRest1", new Vector3(3234.08374f, -3332.99634f, 11.5784721f), 148.7325f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3234.11865f, -3329.57227f, 12.9507627f),
                        CameraDirection = new Vector3(-0.09780149f, -0.8892375f, -0.4468687f),
                        CameraRotation = new Rotator(-26.54296f, 0f, 173.7236f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("EmeryInventory1", new Vector3(3233.75586f, -3319.371f, 7.206264f), 91.91442f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3235.08887f, -3321.02466f, 8.115811f),
                        CameraDirection = new Vector3(-0.5772347f, 0.733659f, -0.3585311f),
                        CameraRotation = new Rotator(-21.01002f, -2.194983E-05f, 38.19528f)
                    },
                    new InventoryInteract("EmeryInventory2", new Vector3(3237.17188f, -3318.73047f, 11.3706827f), 306.6408f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3233.7627f, -3319.23926f, 12.6116428f),
                        CameraDirection = new Vector3(0.9585665f, 0.04204297f, -0.2817494f),
                        CameraRotation = new Rotator(-16.36464f, -2.280169E-06f, -87.4886f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("EmeryChange1", new Vector3(3241.27881f, -3330.177f, 11.3706827f), 127.5932f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(3238.79785f, -3332.20264f, 11.9489422f),
                        CameraDirection = new Vector3(0.7738792f, 0.5986655f, -0.2066656f),
                        CameraRotation = new Rotator(-11.92702f, 4.363058E-06f, -52.27481f)
                    }
                }
            },

            new ResidenceInterior(19202, "GGJ Projects 2, Apt 1a")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537, new Vector3(5061.102f, -1755.245f, 23.0294f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5062.504f, -1755.861f, 22.77757f),
                        InteractHeader = 138.2051f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GGJP2Apt1aStandard1", new Vector3(5054.14453f, -1763.15918f, 22.8013535f), 43.4018f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5057.05957f, -1765.67017f, 24.4191341f),
                        CameraDirection = new Vector3(-0.666633f, 0.7076033f, -0.2343035f),
                        CameraRotation = new Rotator(-13.55057f, 0f, 43.29234f)
                    },
                    new ToiletInteract("GGJP2Apt1aToilet1", new Vector3(5056.81055f, -1754.74219f, 22.7770443f), 45.99881f, "Use Toilet")
                    {
                        UseNavmesh = false,
                        IsStanding = false,
                        CameraPosition = new Vector3(5059.95752f, -1755.29016f, 23.780344f),
                        CameraDirection = new Vector3(-0.911991f, 0.2589934f, -0.3181114f),
                        CameraRotation = new Rotator(-18.54875f, 4.502772E-07f, 74.14616f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5057.771f, -1764.87122f, 22.7770729f), 317.0496f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5056.734f, -1766.24524f, 23.8981228f),
                        CameraDirection = new Vector3(0.7054998f, 0.5933805f, -0.3875173f),
                        CameraRotation = new Rotator(-22.80011f, -1.85228E-06f, -49.93354f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP2Apt1aRest1", new Vector3(5059.30957f, -1762.07922f, 23.2146339f), 313.3011f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5063.328f, -1759.83215f, 24.3495827f),
                        CameraDirection = new Vector3(-0.8406711f, -0.3858223f, -0.3800175f),
                        CameraRotation = new Rotator(-22.33477f, -2.584453E-05f, 114.6525f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP2Apt1aInventory1", new Vector3(5056.402f, -1766.76013f, 22.7770729f), 226.3212f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5057.19727f, -1765.17114f, 23.739584f),
                        CameraDirection = new Vector3(-0.3236664f, -0.8267594f, -0.4601185f),
                        CameraRotation = new Rotator(-27.39475f, -9.61611E-07f, 158.6203f)
                    },
                    new InventoryInteract("GGJP2Apt1aInventory2", new Vector3(5062.0166f, -1760.13721f, 22.7779636f), 316.3353f, "Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5059.874f, -1759.71021f, 23.7930927f),
                        CameraDirection = new Vector3(0.9209683f, -0.1605224f, -0.3550352f),
                        CameraRotation = new Rotator(-20.7956f, -5.936258E-06f, -99.88718f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP2Apt1aChange1", new Vector3(5054.05371f, -1756.84924f, 22.7770443f), 227.6901f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5056.26074f, -1759.03821f, 23.2715836f),
                        CameraDirection = new Vector3(-0.66719f, 0.7376567f, -0.1046303f),
                        CameraRotation = new Rotator(-6.005866f, 1.781358E-05f, 42.12118f)
                    }
                }
            },
            new ResidenceInterior(71426, "GGJ Projects 2, Apt 1b")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537, new Vector3(5066.089f, -1744.931f, 23.02524f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5067.376f, -1744.271f, 22.80794f),
                        InteractHeader = 46.11092f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GGJP2Apt1aStandard1", new Vector3(5053.47363f, -1730.46216f, 22.7772942f), 223.0466f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5051.872f, -1728.88721f, 23.5862541f),
                        CameraDirection = new Vector3(0.6932488f, -0.7203354f, -0.02286982f),
                        CameraRotation = new Rotator(-1.310459f, -1.601245E-07f, -136.0977f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5056.23926f, -1727.94214f, 22.7817726f), 135.6283f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5057.05273f, -1726.38916f, 23.8535538f),
                        CameraDirection = new Vector3(-0.6293289f, -0.6892445f, -0.3590086f),
                        CameraRotation = new Rotator(-21.03932f, -1.829513E-06f, 137.6017f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP2Apt1bRest1", new Vector3(5062.42139f, -1733.35217f, 22.7943535f), 225.3645f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5062.826f, -1731.66821f, 23.6597538f),
                        CameraDirection = new Vector3(-0.04321926f, -0.9654846f, -0.2568493f),
                        CameraRotation = new Rotator(-14.88319f, 1.656395E-07f, 177.4369f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP2Apt1bInventory1", new Vector3(5056.244f, -1726.78418f, 22.7811241f), 351.8874f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5057.92139f, -1728.28015f, 23.6077728f),
                        CameraDirection = new Vector3(-0.6425394f, 0.7501521f, -0.1562533f),
                        CameraRotation = new Rotator(-8.989489f, -1.772001E-05f, 40.58156f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP2Apt1bChange1", new Vector3(5056.422f, -1737.31323f, 22.7772827f), 46.15971f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5054.08252f, -1735.06018f, 23.3384743f),
                        CameraDirection = new Vector3(0.6704726f, -0.7317045f, -0.1227802f),
                        CameraRotation = new Rotator(-7.052581f, -6.452118E-07f, -137.5005f)
                    }
                }
            },
            new ResidenceInterior(140290, "GGJ Projects 3, Apt 1a")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537, new Vector3(5097.607f, -1786.904f, 23.02957f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5097.813f, -1787.306f, 22.77759f),
                        InteractHeader = 44.46087f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GGJPApt1aStandard1", new Vector3(5094.188f, -1784.52124f, 22.7651043f), 44.33807f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5095.56f, -1785.95215f, 23.7388935f),
                        CameraDirection = new Vector3(-0.684735f, 0.7193161f, -0.1171429f),
                        CameraRotation = new Rotator(-6.727238f, -1.934308E-06f, 43.58912f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5087.76758f, -1783.49524f, 22.7650337f), 225.0439f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5086.403f, -1782.65723f, 23.8464031f),
                        CameraDirection = new Vector3(0.6388482f, -0.6774699f, -0.3645648f),
                        CameraRotation = new Rotator(-21.3808f, 0f, -136.6806f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1aRest1", new Vector3(5092.063f, -1777.10718f, 22.7650242f), 44.32032f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5090.55273f, -1780.07214f, 23.5414238f),
                        CameraDirection = new Vector3(0.2148467f, 0.9572635f, -0.1936171f),
                        CameraRotation = new Rotator(-11.16395f, -2.393163E-06f, -12.64975f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1aInventory1", new Vector3(5086.12f, -1782.21021f, 22.7650337f), 131.7994f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5087.82275f, -1782.76917f, 23.5460529f),
                        CameraDirection = new Vector3(-0.8908563f, 0.2511279f, -0.3785628f),
                        CameraRotation = new Rotator(-22.24469f, -1.199152E-05f, 74.25713f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1aChange1", new Vector3(5088.579f, -1778.67114f, 22.7650242f), 229.4285f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5090.78027f, -1780.51123f, 23.2281227f),
                        CameraDirection = new Vector3(-0.6969023f, 0.7025919f, -0.1438463f),
                        CameraRotation = new Rotator(-8.270475f, -3.882358E-06f, 44.76707f)
                    }
                }
            },
            new ResidenceInterior(90114, "GGJ Projects 3, Apt 1b")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537, new Vector3(5107.948f, -1791.86f, 23.02745f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5108.46f, -1793.094f, 22.80729f),
                        InteractHeader = 315.6514f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GGJP3Apt1bStandard1", new Vector3(5120.38037f, -1787.01721f, 22.7669144f), 135.921f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5121.99268f, -1786.60718f, 23.5033436f),
                        CameraDirection = new Vector3(-0.9412518f, -0.3331325f, -0.05538824f),
                        CameraRotation = new Rotator(-3.175137f, 1.175744E-06f, 109.4901f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5125.19775f, -1780.9552f, 22.7668839f), 316.8826f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5124.633f, -1782.5022f, 23.8731327f),
                        CameraDirection = new Vector3(0.5384172f, 0.7526636f, -0.3789519f),
                        CameraRotation = new Rotator(-22.26878f, -6.458082E-06f, -35.578f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1bRest1", new Vector3(5120.617f, -1777.55017f, 22.9024334f), 0.947769f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5122.525f, -1780.57825f, 24.1103039f),
                        CameraDirection = new Vector3(-0.3994308f, 0.8487614f, -0.3464955f),
                        CameraRotation = new Rotator(-20.27311f, -7.281251E-06f, 25.20188f),
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("amb@world_human_bum_slumped@male@laying_on_left_side@base", "base", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("get_up@directional@movement@from_knees@standard", "getup_l_0", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1bInventory1", new Vector3(5125.994f, -1782.08618f, 22.7668934f), 278.1388f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5124.16553f, -1783.63916f, 23.5643845f),
                        CameraDirection = new Vector3(0.8634548f, 0.4911701f, -0.1148818f),
                        CameraRotation = new Rotator(-6.596808f, -4.29732E-07f, -60.36692f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1bChange1", new Vector3(5114.705f, -1782.90125f, 22.7668629f), 313.8317f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5116.64063f, -1780.94922f, 23.3228245f),
                        CameraDirection = new Vector3(-0.7270162f, -0.6586128f, -0.1941046f),
                        CameraRotation = new Rotator(-11.19242f, 2.610979E-06f, 132.1738f)
                    }
                }
            },
            new ResidenceInterior(62210, "GGJ Projects 3, Apt 1a")
            {
                IsTeleportEntry = false,
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(1033979537, new Vector3(5109.512f, -1800.952f, 23.02957f))
                    {
                        LockWhenClosed = true,
                        InteractPostion = new Vector3(5109.213f, -1800.412f, 22.77766f),
                        InteractHeader = 227.0408f
                    }
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new StandardInteriorInteract("GGJP3Apt1cStandard1", new Vector3(5113.42773f, -1803.91724f, 22.7652245f), 222.8452f, "Interact")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5111.69141f, -1802.26819f, 23.487133f),
                        CameraDirection = new Vector3(0.6831025f, -0.7196007f, -0.1246827f),
                        CameraRotation = new Rotator(-7.16243f, -6.453662E-07f, -136.4905f)
                    },
                    new CraftInteriorInteract("Stove", new Vector3(5119.03125f, -1804.21118f, 22.7650242f), 46.76653f, "Stove")
                    {
                        CraftingFlag = "Stove",
                        AutoCamera = false,
                        CameraPosition = new Vector3(5120.508f, -1805.08215f, 23.8798733f),
                        CameraDirection = new Vector3(-0.6436245f, 0.6620793f, -0.3839252f),
                        CameraRotation = new Rotator(-22.57703f, 0f, 44.19024f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("GGJP3Apt1cRest1", new Vector3(5114.7793f, -1810.62524f, 22.7650242f), 225.0215f, "Sleep")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5116.469f, -1808.28821f, 23.7018642f),
                        CameraDirection = new Vector3(-0.345157f, -0.8860653f, -0.3094429f),
                        CameraRotation = new Rotator(-18.02566f, -4.938128E-06f, 158.7171f),
                        StartAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getin_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        LoopAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_sleep_loop_couch", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        EndAnimations = new List<AnimationBundle>
                        {
                            new AnimationBundle("savecouch@", "t_getout_couch", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" }
                        },
                        UseDefaultAnimations = false
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("GGJP3Apt1cInventory1", new Vector3(5120.83936f, -1805.60425f, 22.7650242f), 314.4303f, "Access Items")
                    {
                        CanAccessItems = true,
                        CanAccessWeapons = true,
                        CanAccessCash = true,
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5119.08154f, -1804.74622f, 23.6565838f),
                        CameraDirection = new Vector3(0.8260796f, -0.4026572f, -0.3942837f),
                        CameraRotation = new Rotator(-23.22131f, -2.229686E-05f, -115.9861f)
                    }
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("GGJP3Apt1cChange1", new Vector3(5118.06738f, -1808.9082f, 22.7650242f), 43.79533f, "Change Outfit")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(5116.07568f, -1807.07117f, 23.279974f),
                        CameraDirection = new Vector3(0.6770111f, -0.7154508f, -0.1725864f),
                        CameraRotation = new Rotator(-9.938233f, -6.06746E-06f, -136.5813f)
                    }
                }
            }

        });
    }
    private void Restaurants()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //BurgerShot
            new Interior(126722, "Burger Shot")
        {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6826.206f, -3029.144f, 25.212f), 269.8009f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6826.163f, -3031.522f, 25.212f), 266.6409f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(6826.201f, -3033.86f, 25.212f), 268.8137f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(6840.523f, -3031.698f, 25.46795f)) { LockWhenClosed = true, InteractPostion = new Vector3(6841.112f, -3030.166f, 25.21254f), InteractHeader =  90.819f },
                    new InteriorDoor(1050821746,new Vector3(6840.522f, -3028.689f, 25.46795f)){ LockWhenClosed = true, InteractPostion = new Vector3(6841.112f, -3030.166f, 25.21254f), InteractHeader =  90.819f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(6830.787f, -3022.293f, 25.46941f)){ LockWhenClosed = true, InteractPostion = new Vector3(6829.254f, -3021.711f, 25.21105f), InteractHeader =  180.5584f },
                    new InteriorDoor(1050821746,new Vector3(6828.765f, -3024.501f, 24.2556f)){ LockWhenClosed = true, InteractPostion = new Vector3(6829.254f, -3021.711f, 25.21105f), InteractHeader =  180.5584f },
                },
        },
            new Interior(156674, "Burger Shot")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(5637.095f, -1753.564f, 16.31569f), 30.39394f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(5639.155f, -1752.351f, 16.31569f), 29.43141f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(5641.108f, -1751.21f, 16.31569f), 28.83416f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(5632.152f, -1739.85f, 16.57166f)) { LockWhenClosed = true, InteractPostion = new Vector3(5630.608f, -1740.146f, 16.27608f), InteractHeader =  210.8455f },
                    new InteriorDoor(1050821746,new Vector3(5629.546f, -1741.356f, 16.57166f)){ LockWhenClosed = true, InteractPostion = new Vector3(5630.608f, -1740.146f, 16.27608f), InteractHeader =  210.8455f  },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(5628.875f, -1752.985f, 16.57312f)){ LockWhenClosed = true, InteractPostion = new Vector3(5629.218f, -1754.518f, 16.17559f), InteractHeader =  301.5448f },
                    new InteriorDoor(1050821746,new Vector3(5630.38f, -1755.591f, 16.57312f)){ LockWhenClosed = true, InteractPostion = new Vector3(5629.218f, -1754.518f, 16.17559f), InteractHeader =  301.5448f },
                },
            },
            new Interior(112642, "Burger Shot - Star Junction")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(5017.145f, -2969.855f, 14.82007f), 89.36915f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(5017.159f, -2967.47f, 14.82007f), 87.70749f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(5017.149f, -2965.22f, 14.82007f), 91.50613f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(5002.817f, -2967.288f, 15.07604f)) { LockWhenClosed = true, InteractPostion = new Vector3(5002.304f, -2968.818f, 14.77771f), InteractHeader =  271.3166f },
                    new InteriorDoor(1050821746,new Vector3(5002.818f, -2970.297f, 15.07604f)){ LockWhenClosed = true, InteractPostion = new Vector3(5002.304f, -2968.818f, 14.77771f), InteractHeader =  271.3166f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(5012.554f, -2976.693f, 15.0775f)){ LockWhenClosed = true, InteractPostion = new Vector3(5014.082f, -2977.239f, 14.81536f), InteractHeader =  359.9872f },
                    new InteriorDoor(1050821746,new Vector3(5015.563f, -2976.692f, 15.0775f)){ LockWhenClosed = true, InteractPostion = new Vector3(5014.082f, -2977.239f, 14.81536f), InteractHeader =  359.9872f },
                },
            },
            new Interior(109570, "Burger Shot - North Holland")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4761.305f, -2063.22f, 13.04701f), 89.57156f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(4761.3f, -2060.822f, 13.04701f), 89.65251f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(4761.319f, -2058.55f, 13.04701f), 85.69505f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(4746.997f, -2060.628f, 13.30298f)) { LockWhenClosed = true, InteractPostion = new Vector3(4746.506f, -2062.107f, 13.04964f), InteractHeader =  269.6174f },
                    new InteriorDoor(1050821746,new Vector3(4746.998f, -2063.637f, 13.30298f)){ LockWhenClosed = true, InteractPostion = new Vector3(4746.506f, -2062.107f, 13.04964f), InteractHeader =  269.6174f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(4756.733f, -2070.033f, 13.30444f)){ LockWhenClosed = true, InteractPostion = new Vector3(4758.286f, -2070.557f, 13.04167f), InteractHeader =  359.0448f },
                    new InteriorDoor(1050821746,new Vector3(4759.743f, -2070.032f, 13.30444f)){ LockWhenClosed = true, InteractPostion = new Vector3(4758.286f, -2070.557f, 13.04167f), InteractHeader =  359.0448f },
                },
            },
            new Interior(134402, "Burger Shot - Bohan")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6298.258f, -1672.324f, 16.90752f), 45.87393f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6299.972f, -1670.646f, 16.90753f), 44.89492f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(6301.518f, -1668.984f, 16.90753f), 46.71372f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(6289.873f, -1660.468f, 17.16348f)) { LockWhenClosed = true, InteractPostion = new Vector3(6288.396f, -1661.147f, 16.89834f), InteractHeader =  226.0569f },
                    new InteriorDoor(1050821746,new Vector3(6287.765f, -1662.616f, 17.16348f)){ LockWhenClosed = true, InteractPostion = new Vector3(6288.396f, -1661.147f, 16.89834f), InteractHeader =  226.0569f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(6290.234f, -1674.0f, 17.16494f)){ LockWhenClosed = true, InteractPostion = new Vector3(6290.967f, -1675.443f, 16.90903f), InteractHeader =  317.8639f },
                    new InteriorDoor(1050821746,new Vector3(6292.383f, -1676.107f, 17.16494f)){ LockWhenClosed = true, InteractPostion = new Vector3(6290.967f, -1675.443f, 16.90903f), InteractHeader =  317.8639f },
                },
            },
            new Interior(105986, "Burger Shot - The Meat Quarter")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4569.465f, -3123.683f, 4.811165f), 358.4242f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(4571.874f, -3123.717f, 4.811165f), 356.4503f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(4574.167f, -3123.688f, 4.811165f), 359.7217f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },
                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(4572.065f, -3109.38f, 5.06713f)) { LockWhenClosed = true, InteractPostion = new Vector3(4570.549f, -3108.904f, 4.813244f), InteractHeader =  182.0755f },
                    new InteriorDoor(1050821746,new Vector3(4569.056f, -3109.381f, 5.06713f)){ LockWhenClosed = true, InteractPostion = new Vector3(4570.549f, -3108.904f, 4.813244f), InteractHeader =  182.0755f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(4562.66f, -3119.117f, 5.06859f)){ LockWhenClosed = true, InteractPostion = new Vector3(4562.167f, -3120.62f, 4.812753f), InteractHeader =  272.5801f },
                    new InteriorDoor(1050821746,new Vector3(4562.661f, -3122.126f, 5.06859f)){ LockWhenClosed = true, InteractPostion = new Vector3(4562.167f, -3120.62f, 4.812753f), InteractHeader =  272.5801f },
                },
            },
            new Interior(59650, "Burger Shot - Alderney")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(4183.211f, -1628.357f, 24.31401f), 178.8505f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(4180.802f, -1628.367f, 24.31399f), 177.9693f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(4178.541f, -1628.319f, 24.31399f), 177.7901f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },

                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(3024662465, new Vector3(4180.655f, -1642.646f, 24.56996f)) { LockWhenClosed = true, InteractPostion = new Vector3(4182.205f, -1643.287f, 24.01462f), InteractHeader =  359.8446f },
                    new InteriorDoor(1050821746,new Vector3(4183.664f, -1642.645f, 24.56996f)){ LockWhenClosed = true, InteractPostion = new Vector3(4182.205f, -1643.287f, 24.01462f), InteractHeader =  359.8446f },

                    //SIDE
                    new InteriorDoor(3024662465,new Vector3(4190.06f, -1632.909f, 24.57142f)){ LockWhenClosed = true, InteractPostion = new Vector3(4190.603f, -1631.357f, 24.13132f), InteractHeader =  91.97084f },
                    new InteriorDoor(1050821746,new Vector3(4190.06f, -1629.9f, 24.57142f)){ LockWhenClosed = true, InteractPostion = new Vector3(4190.603f, -1631.357f, 24.13132f), InteractHeader =  91.97084f },
                },
            },
            //CluckingBell
            new Interior(143874, "Cluckin' Bell - Dukes")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6370.939f, -2895.904f, 25.10332f), 359.2544f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6372.551f, -2895.928f, 25.10328f), 359.2594f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(6374.223f, -2895.918f, 25.1033f), 358.4255f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister4",new Vector3(6375.829f, -2895.923f, 25.10331f), 358.8919f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
             },

                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(903328250, new Vector3(6380.992f, -2881.459f, 25.34808f)) { LockWhenClosed = true, InteractPostion = new Vector3(6379.465f, -2881.021f, 25.09674f), InteractHeader =  180.3512f },
                    new InteriorDoor(2862357381,new Vector3(6377.992f, -2881.459f, 25.34808f)){ LockWhenClosed = true, InteractPostion = new Vector3(6379.465f, -2881.021f, 25.09674f), InteractHeader =  180.3512f },

                },
            },
            new Interior(124162, "Cluckin' Bell - The Triangle")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(5066.804f, -3188.044f, 14.80308f), 101.0844f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(5066.365f, -3186.497f, 14.80304f), 104.1104f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister3",new Vector3(5065.957f, -3184.908f, 14.80306f), 105.7112f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister4",new Vector3(5065.556f, -3183.327f, 14.80309f), 104.344f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
            },

                Doors = new List<InteriorDoor>()
                { 
                    //FRONT
                    new InteriorDoor(903328250, new Vector3(5050.246f, -3182.093f, 15.04785f)) { LockWhenClosed = true, InteractPostion = new Vector3(5050.146f, -3183.737f, 14.76415f), InteractHeader =  281.1935f },
                    new InteriorDoor(2862357381,new Vector3(5051.022f, -3184.991f, 15.04785f)){ LockWhenClosed = true, InteractPostion = new Vector3(5050.146f, -3183.737f, 14.76415f), InteractHeader =  281.1935f },

                },
            },
            //Restaurants
            new Interior(78338, "Perestroika")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true,
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6135.582f, -3529.857f, 18.27529f), 90.9241f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("CashRegister2",new Vector3(6135.545f, -3525.244f, 18.27313f), 91.08496f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   new MoneyTheftInteract("Drawer1",new Vector3(6139.457f, -3505.638f, 15.85712f), 180.0058f,"Rob Drawer")
                   {
                        CashMinAmount = 500,
                        CashMaxAmount = 3000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 500,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                   },
                Doors = new List<InteriorDoor>()
                {
                   new InteriorDoor(2762578800,new Vector3(6144.174f, -3545.455f, 20.12719f)) { LockWhenClosed = true, InteractPostion = new Vector3(6145.713f, -3545.995f, 19.84102f), InteractHeader =  2.96353f },//cabaret_door_l 
                   new InteriorDoor(1985756882,new Vector3(6147.174f, -3545.455f, 20.12719f)) { LockWhenClosed = true, InteractPostion = new Vector3(6145.713f, -3545.995f, 19.84102f), InteractHeader =  2.96353f },//cabaret_door_r 
                   new InteriorDoor(3971243973,new Vector3(6150.814f, -3546.269f, 20.14879f)) { LockWhenClosed = false },//inside - coat room
                   new InteriorDoor(3971243973,new Vector3(6147.118f, -3540.366f, 20.14879f)) { LockWhenClosed = false },//inside
                   new InteriorDoor(3971243973,new Vector3(6144.119f, -3540.366f, 20.14879f)) { LockWhenClosed = false },//inside
                   new InteriorDoor(3971243973,new Vector3(6135.199f, -3517.483f, 18.51647f)) { LockWhenClosed = false },//inside stage door - to office
                   new InteriorDoor(3971243973,new Vector3(6141.786f, -3506.881f, 16.10702f)) { LockWhenClosed = true },//inside officer
                   new InteriorDoor(2881168431,new Vector3(6156.352f, -3516.486f, 22.01963f)) { LockWhenClosed = true, InteractPostion = new Vector3(6156.887f, -3514.947f, 21.76736f), InteractHeader = 89.67247f },//rear_door
                   new InteriorDoor(2881168431,new Vector3(6156.352f, -3513.482f, 22.01963f)) { LockWhenClosed = true, InteractPostion = new Vector3(6156.887f, -3514.947f, 21.76736f), InteractHeader = 89.67247f },//rear_door
                },
            },
            new Interior(85506, "Mr Fuk's - Alderney City")
            {
                IsTrespassingWhenClosed = true,
                IsWeaponRestricted = true, },
             });
    }
    private void Stations()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
        //Fire Stations
        new Interior(160514, "Broker Fire Station") { },
        new Interior(176898, "Bohan Fire Station") { },
        });
    }
    private void Stores()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Gun Stores
            new Interior(108290, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(6245.646f, -3170.169f, 34.49496f)) { LockWhenClosed = true, InteractPostion = new Vector3(6244.992f, -3168.911f, 34.08046f), InteractHeader =  271.0405f },
                },
            },// GunStore - Downtown Broker
            new Interior(92674, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(5261.213f, -3599.389f, 11.41268f)) { LockWhenClosed = true, InteractPostion = new Vector3(5260.685f, -3598.026f, 11.16249f), InteractHeader =  268.6977f },
                },
            }, // GunStore - Chinatown, Algonquin
            new Interior(101890, "Underground Guns")
            {
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(807349477, new Vector3(3855.926f, -2947.357f, 14.86614f)) { LockWhenClosed = true, InteractPostion = new Vector3(3854.629f, -2947.912f, 14.62142f), InteractHeader =  1.594016f },
                },
            },// GunStore -  Port Tudor, Alderney

            // Clothing Store
            new Interior(120834, "Russian Store")
            {
                InteractPoints = new List < InteriorInteract > ()
                {
                   new MoneyTheftInteract("CashRegister1",new Vector3(6077.243f, -3702.948f, 15.84804f), 7.241336f,"Rob")
                   {
                        CashMinAmount = 250,
                        CashMaxAmount = 1000,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                   },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(377940039, new Vector3(6080.48f, -3700.686f, 16.11983f )) { LockWhenClosed = true, InteractPostion = new Vector3(6081.085f, -3699.182f, 15.86015f), InteractHeader =  86.96169f },
                    new InteriorDoor(3762420618, new Vector3(6080.48f, -3697.683f, 16.11983f)) { LockWhenClosed = true, InteractPostion = new Vector3(6081.085f, -3699.182f, 15.86015f), InteractHeader =  86.96169f },
                },
            },
        });
    }

}
