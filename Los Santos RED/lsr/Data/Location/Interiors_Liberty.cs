using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    private void GeneralInteriors()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            new Interior(76290,"Perestroika") { },
            new Interior(138498, "Laundromat"){  },
            new Interior(160514,"Broker Fire Station"){  },
            new Interior(176898, "Bohan Fire Station"){  },


            new Interior(27394, "Memory Lanes"){  },
            new Interior(151554, "Beechwood Apts 2"),
            new Interior(119042, "Beechwood Apts 1"),
            new Interior(35330, "Beechwood Apts 3"),
            new Interior(118786, "Homebrew Cafe"){  },
            new Interior(37634, "Beechwood Apts 5"),
            
            new Interior(24578, "JJ China Limited"),

            new Interior(124418, "Burger Shot")
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

            new Interior(143874, "Cluckin' Bell - Dukes"),
            new Interior(124162, "Cluckin' Bell - The Triangle"),

            new Interior(50178, "tw@ - Broker"){  },
            new Interior(166914, "tw@ - Bercham"),
            new Interior(66562, "tw@ - North Holland"),


            new Interior(139266, "The Libertonian"),





            //burger shot beechwoodd city
            //3024662465,new Vector3(1890.526f, 718.3006f, 25.46795f)

        });

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
            },
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
            },
            new ResidenceInterior(111874,"Alderney Apartment") {
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
            },

        });

    }
}

