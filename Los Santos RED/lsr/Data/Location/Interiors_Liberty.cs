using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Interiors_Liberty
{
    private PossibleInteriors LibertyCityInteriors;

    public void DefaultConfig()
    {
        LibertyCityInteriors = new PossibleInteriors();
        GeneralInteriors();
        

        Serialization.SerializeParam(LibertyCityInteriors, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Interiors_{StaticStrings.LibertyConfigSuffix}.xml");
    }
    private void GeneralInteriors()
    {
        LibertyCityInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            new Interior(76290,"Perestroika") { NeedsActivation = true, },
            new Interior(138498, "Laundromat"){ NeedsActivation = true, },
            new Interior(160514,"Broker Fire Station"){ NeedsActivation = true, },
            new Interior(176898, "Bohan Fire Station"){ NeedsActivation = true, },


            new Interior(27394, "Memory Lanes"){ NeedsActivation = true, },
            new Interior(151554, "Beechwood Apts 2"),
            new Interior(119042, "Beechwood Apts 1"),
            new Interior(35330, "Beechwood Apts 3"),
            new Interior(118786, "Homebrew Cafe"){ NeedsActivation = true, },
            new Interior(37634, "Beechwood Apts 5"),
            new Interior(50178, "tw@"){ NeedsActivation = true, },
            new Interior(24578, "JJ China Limited"),
            new Interior(124418, "Burger Shot"){ NeedsActivation = true, },


        });

        LibertyCityInteriors.ResidenceInteriors.AddRange(new List<ResidenceInterior>() 
        { 
             //Apartments
            new ResidenceInterior(60162,"Motel") {//Motel Interior
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(151.817f, -1006.616f, -98.99998f),
                InteriorEgressHeading = 340.2548f,
                ClearPositions = new List<Vector3>() { new Vector3(154.1695f, -1002.792f, -99.00002f),new Vector3(153.8864f, -1006.77f, -98.99998f),new Vector3(153.409f, -1007.084f, -98.99998f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("motelExit1",new Vector3(151.47f, -1007.435f, -98.99998f), 168.4321f ,"Exit"),
                    new ToiletInteract("moteltoilet1",new Vector3(154.5553f, -1001.159f, -98.99998f), 267.9796f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(151.8545f, -1001.544f, -98.08442f),
                        CameraDirection = new Vector3(0.9478149f, 0.1316178f, -0.2903854f),
                        CameraRotation = new Rotator(-16.88103f, -2.230548E-07f, -82.09421f)
                    },
                    new SinkInteract("motelSink1",new Vector3(154.1283f, -1000.606f, -99f), 359.4646f,"Use Sink")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(151.8545f, -1001.544f, -98.08442f),
                        CameraDirection = new Vector3(0.9478149f, 0.1316178f, -0.2903854f),
                        CameraRotation = new Rotator(-16.88103f, -2.230548E-07f, -82.09421f)
                    },
                    new StandardInteriorInteract("motelstandard1",new Vector3(153.8864f, -1006.77f, -98.99998f), 226.7879f,"Interact") {
                        CameraPosition = new Vector3(151.5212f, -1005f, -97.66277f),
                        CameraDirection = new Vector3(0.7067831f, -0.6425714f, -0.2959048f),
                        CameraRotation = new Rotator(-17.2118f, 1.7876E-06f, -132.2755f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("motelInventory1",new Vector3(151.6077f, -1003.203f, -99.00002f), 86.54763f,"Access Cash/Weapons/Items")
                    {
                        CameraPosition = new Vector3(152.989f, -1005.717f, -98.17555f),
                        CameraDirection = new Vector3(-0.4973816f, 0.846449f, -0.1900941f),
                        CameraRotation = new Rotator(-10.95827f, 1.174001E-05f, 30.43891f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("motelChange1",new Vector3(152.0395f, -1001.111f, -98.99998f), 208.9005f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(154.3757f, -1006.959f, -97.54375f),
                        CameraDirection = new Vector3(-0.3906728f, 0.8876943f, -0.2436668f),
                        CameraRotation = new Rotator(-14.10306f, -3.961381E-06f, 23.75422f)
                    },
                },
                RestInteracts = new List<RestInteract>(){
                    new RestInteract("motelRest1",new Vector3(154.1695f, -1002.792f, -99.00002f), 189.1073f,"Rest")//new Vector3(152.0395f, -1001.007f, -98.99998f),89.01726f,"Rest")
                    {
                        CameraPosition = new Vector3(152.0579f, -1006.497f, -97.8805f),
                        CameraDirection = new Vector3(0.718467f, 0.6200207f, -0.3152452f),
                        CameraRotation = new Rotator(-18.37562f, 6.297525E-06f, -49.20655f)
                    } ,
                },

            },
            new ResidenceInterior(108290,"Low End Apartment") {//Low End Apartment
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(266.3692f, -1004.84f, -99.41235f),
                InteriorEgressHeading = 5.047247f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("lowEndExit1",new Vector3(266.3692f, -1004.84f, -99.41235f),176.2754f,"Exit"),
                    new StandardInteriorInteract("lowEndStandard1",new Vector3(262.6654f, -999.8923f, -99.00858f), 181.6553f,"Interact")
                    {
                        CameraPosition = new Vector3(260.6436f, -998.1468f, -97.62578f),
                        CameraDirection = new Vector3(0.5112366f, -0.7773319f, -0.3665953f),
                        CameraRotation = new Rotator(-21.50579f, -3.670642E-06f, -146.6678f)
                    },
                    new ToiletInteract("lowEndToilet1",new Vector3(256.2933f, -1000.674f, -99.00987f), 359.5767f,"Use Toilet")
                    {
                        CameraPosition = new Vector3(254.3052f, -1001.035f, -97.88249f),
                        CameraDirection = new Vector3(0.7697585f, 0.3008637f, -0.5629858f),
                        CameraRotation = new Rotator(-34.26254f, 4.442075E-05f, -68.65173f)
                    },
                    new SinkInteract("lowEndSink1",new Vector3(255.6285f, -1000.62f, -99.00987f), 358.1648f,"Use Sink")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(254.3052f, -1001.035f, -97.88249f),
                        CameraDirection = new Vector3(0.7697585f, 0.3008637f, -0.5629858f),
                        CameraRotation = new Rotator(-34.26254f, 4.442075E-05f, -68.65173f)
                    },
                },
                RestInteracts = new List<RestInteract>(){
                    new RestInteract("lowEndRest1",new Vector3(262.5934f,-1002.507f,-99.0086f),182.0201f,"Sleep") {
                        InteractDistance = 1.0f,
                        CameraPosition = new Vector3(260.0531f, -1002.673f, -98.05583f),
                        CameraDirection = new Vector3(0.8871264f, -0.3528852f, -0.2974538f),
                        CameraRotation = new Rotator(-17.30473f, -1.341376E-05f, -111.6919f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("lowEndInventory1",new Vector3(265.4756f, -997.2742f, -99.00861f), 274.2195f,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(263.4088f, -999.621f, -97.33189f),
                        CameraDirection = new Vector3(0.6594798f, 0.6394753f, -0.395168f),
                        CameraRotation = new Rotator(-23.27645f, 3.717681E-06f, -45.88231f)
                    },
                    new InventoryInteract("lowEndInventory2",new Vector3(265.8927f, -999.3979f, -99.00861f), 269.6088f,"Access Cash/Weapons")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(263.7372f, -1000.824f, -97.25036f),
                        CameraDirection = new Vector3(0.763785f, 0.4243863f, -0.4863422f),
                        CameraRotation = new Rotator(-29.10045f, -6.839815E-06f, -60.94188f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("lowEndChange1",new Vector3(260.1581f, -1004.046f, -99.0086f), 328.1687f,"Change Outfit")
                    {
                        CameraPosition = new Vector3(261.702f, -1001.972f, -98.1841f),
                        CameraDirection = new Vector3(-0.6467782f, -0.7220481f, -0.2456104f),
                        CameraRotation = new Rotator(-14.21791f, -5.284513E-06f, 138.1474f)
                    },
                },
            },
            new ResidenceInterior(-669,"Medium Apartment") {//needs the blinds closed
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(289.1848f,-997.1297f,-92.79259f),//new Vector3(288.3021f, -998.6495f, -92.79259f),
                InteriorEgressHeading = 288.6531f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("mediumaptExit1",new Vector3(287.0905f, -999.1916f, -92.79259f), 92.78439f ,"Exit") ,
                    new StandardInteriorInteract("mediumaptStandard1",new Vector3(302.9109f,-999.1298f,-94.19514f),128.1528f,"Interact")
                    {
                        CameraPosition = new Vector3(304.4328f, -993.7673f, -91.80285f),
                        CameraDirection = new Vector3(-0.4074028f, -0.8461376f, -0.343619f),
                        CameraRotation = new Rotator(-20.09752f, -1.181871E-05f, 154.2899f),
                    },
                    new ToiletInteract("mediumaptToilet1",new Vector3(294.9899f, -992.6285f, -98.99982f), 137.3274f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(297.0419f, -992.011f, -98.30881f),
                        CameraDirection = new Vector3(-0.9224917f, -0.2608043f, -0.2845877f),
                        CameraRotation = new Rotator(-16.53421f, 6.679501E-06f, 105.7865f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("mediumaptInventory1",new Vector3(298.5356f, -986.5438f, -94.19513f), 2.748475f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(301.618f, -988.8294f, -93.4222f),
                        CameraDirection = new Vector3(-0.6374181f, 0.769813f, -0.03295543f),
                        CameraRotation = new Rotator(-1.888549f, 7.26102E-06f, 39.62533f)
    },
                    new InventoryInteract("mediumaptInventory2",new Vector3(310.6211f, -985.8007f, -94.19513f), 357.7761f ,"Access Weapons/Cash")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(307.6822f, -988.2132f, -93.57951f),
                        CameraDirection = new Vector3(0.7854234f, 0.6117197f, -0.09438821f),
                        CameraRotation = new Rotator(-5.416109f, 2.572807E-06f, -52.08709f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("mediumaptChange1",new Vector3(304.4411f, -991.0422f, -98.98907f), 86.91703f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(300.5961f, -991.0005f, -97.69125f),
                        CameraDirection = new Vector3(0.979211f, -0.05827192f, -0.1942943f),
                        CameraRotation = new Rotator(-11.2035f, 2.393489E-06f, -93.4056f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("mediumaptRest1", new Vector3(305.5909f,-996.2227f,-98.99995f),180.8388f,"Sleep")
                    {
                        CameraPosition = new Vector3(301.9485f, -996.23f, -98.27151f),
                        CameraDirection = new Vector3(0.8657399f, -0.425304f, -0.2638389f),
                        CameraRotation = new Rotator(-15.29797f, 3.540547E-06f, -116.1631f)
                    },
                },
            },
        });

    }
}

