using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Interiors : IInteriors
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Interiors.xml";
    private List<Interior> LocationsList;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Interiors*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Interiors config  {ConfigFile.FullName}",0);
            LocationsList = Serialization.DeserializeParams<Interior>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Interiors config  {ConfigFileName}",0);
            LocationsList = Serialization.DeserializeParams<Interior>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Interiors config found, creating default", 0);
            DefaultConfig();
        }
    }
    public List<Interior> GetAllPlaces()
    {
        return LocationsList;
    }
    public Interior GetInteriorByLocalID(int id)
    {
        return LocationsList.Where(x => x.LocalID == id).FirstOrDefault();
    }
    public Interior GetInteriorByInternalID(int id)
    {
        return LocationsList.Where(x => x.InternalID == id).FirstOrDefault();
    }


    public Interior GetInterior(string name)
    {
        return LocationsList.Where(x => x.Name == name).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        LocationsList = new List<Interior>();
        Stores();
        Tunnels();
        Stations();
        Other();
        Serialization.SerializeParams(LocationsList, ConfigFileName);
    }
    private void Stores()
    {
        LocationsList.AddRange(new List<Interior>()
        {
            //Clothes
            new Interior(19458,"Sub Urban"),
            new Interior(22786, "BINCO Textile City",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3146141106, new Vector3(418.5713f,-808.674f,29.64108f)),
                    new InteriorDoor(868499217, new Vector3(418.5713f,-806.3979f,29.64108f)) }),//doesntwork?
            new Interior(10754,"Sub Urban"),
            new Interior(1282,"Ponsonby"),
            new Interior(14338,"Ponsonby"),
            new Interior(22786,"Binco"),
            new Interior(96266,"Suburban"),//suburban harmony
            new Interior(17154,"Binco"),
            new Interior(88066,"Discount Store"),

            //Ammunations
            new Interior(-555,"Ammunation Vespucci Boulevard",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(842.7685f, -1024.539f, 28.34478f)),
                    new InteriorDoor(97297972, new Vector3(845.3694f, -1024.539f, 28.34478f)),}),
            new Interior(-554,"Ammunation Lindsay Circus",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(-662.6415f, -944.3256f, 21.97915f)),
                    new InteriorDoor(97297972, new Vector3(-665.2424f, -944.3256f, 21.97915f)) }),
            new Interior(29698,"Ammu Nation Vinewood Plaza",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(243.8379f, -46.52324f, 70.09098f)),
                    new InteriorDoor(97297972, new Vector3(244.7275f, -44.07911f, 70.09098f)) }),
            new Interior(80386,"Ammunation Cypress Flats"),
            new Interior(48130,"Ammunation Pillbox Hill"),
            new Interior(35586,"Ammunation"),

            //24/7
            new Interior(41474,"24/7 Route 68",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(545.504f,2672.745f,42.30644f)),//left door
                    new InteriorDoor(997554217, new Vector3(542.9252f,2672.406f,42.30644f))}),//right door
            new Interior(16386,"24/7 Chumash",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3240.128f,1003.157f,12.98064f)),//left door
                    new InteriorDoor(997554217, new Vector3(-3239.905f,1005.749f,12.98064f))}),//right door
            new Interior(62722,"24/7 Palomino Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2559.201f,384.0875f,108.7729f)),
                    new InteriorDoor(997554217, new Vector3(2559.304f,386.6865f,108.7729f)),}),//right door
            new Interior(33282,"24/7 Strawberry",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(27.81761f,-1349.169f,29.64696f)),
                    new InteriorDoor(997554217, new Vector3(30.4186f,-1349.169f,29.64696f)),}),//right door
            new Interior(97538,"24/7 Banham Canyon",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3038.219f,588.2872f,8.058861f)),
                    new InteriorDoor(997554217, new Vector3(-3039.012f,590.7643f,8.058861f)),}),//right door    
            new Interior(46850,"24/7 Downtown Vinewood",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(375.3528f,323.8015f,103.7163f)),
                    new InteriorDoor(997554217, new Vector3(377.8753f,323.1672f,103.7163f)),}),//right door   
            new Interior(36354,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1437777724, new Vector3(1732.245f,6415.377f,34.76194f)),
                    new InteriorDoor(1421582485, new Vector3(1734.097f,6413.048f,34.99545f)),}),//right door   
            new Interior(13826,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2681.292f,3281.427f,55.39108f)),
                    new InteriorDoor(997554217, new Vector3(2682.558f,3283.698f,55.39108f)),}),//right door          
            new Interior(55554,"24/7 Sandy Shores",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(1963.917f,3740.075f,32.49369f)),
                    new InteriorDoor(997554217, new Vector3(1966.17f,3741.376f,32.49369f)),}),//right door 

            //LtD
            new Interior(47874, "Ltd Little Seoul",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-713.0732f,-916.5409f,19.36553f)),
                    new InteriorDoor(2065277225, new Vector3(-710.4722f,-916.5372f,19.36553f)),}) { IsSPOnly = true},//right door  
            new Interior(45570, "Ltd Grapeseed",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1699.661f,4930.278f,42.21359f)),
                    new InteriorDoor(2065277225, new Vector3(1698.172f,4928.146f,42.21359f)),}) { IsSPOnly = true},//right door  
            new Interior(80642,"LtD Davis",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-53.96111f,-1755.717f,29.57094f)),
                    new InteriorDoor(2065277225, new Vector3(-51.96669f,-1757.387f,29.57094f)),}),//right door   
            new Interior(2050,"LtD Mirror Park",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1158.364f,-326.8165f,69.35503f)),
                    new InteriorDoor(2065277225, new Vector3(1160.925f,-326.3612f,69.35503f)),}),//right door   
            new Interior(82178,"LtD Richman Glen",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1158.364f,-326.8165f,69.35503f)),
                    new InteriorDoor(2065277225, new Vector3(1160.925f,-326.3612f,69.35503f)),}),//right door  
            new Interior(74874,"LtD Gas"),

            //Liquor
            new Interior(33026,"Scoops Liquor Barn"),                
            new Interior(104450,"Liquor Ace"),     
            new Interior(50178,"Rob's Liquors",//San Andreas Ave Del Perro
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1226.894f,-903.1218f,12.47039f)){ LockWhenClosed = true },
                }),
            new Interior(19202,"Rob's Liquors",//Route 1
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-2973.535f,390.1414f,15.18735f)){ LockWhenClosed = true },
                }),
            new Interior(98818,"Rob's Liquors",//Prosperity Street
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1490.411f,-383.8453f,40.30745f)){ LockWhenClosed = true },
                }),
            new Interior(73986,"Rob's Liquors",//El Rancho Boulevard
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(1141.038f,-980.3225f,46.55986f)) { LockWhenClosed = true },
                }),
            new Interior(50178,"Rob's Liquors"),

            //Barber/Tattoo      
            new Interior(37378,"Bob Mullet Hair & Beauty"),
            new Interior(113922,"Beachcombover Barber"),
            new Interior(10242,"Hot Shave Barbers"),
            new Interior(49922,"Los Santos Tattoo"),
            new Interior(93442,"Los Santos Customs"),
            new Interior(102146,"Herr Kutz Barber"),
            new Interior(35074,"The Pit"),//tattoo
            //Car Dealer
            new Interior(37890,"Los Santos Customs"),
            new Interior(7170, "Premium Deluxe Motorsport",new List<string>() { "shr_int" },new List<string>() { "fakeint" },new List<string>() { "shutter_open","csr_beforeMission" }),

            //Banks
            new Interior(71682,"Fleeca Bank") {
               IsWeaponRestricted = true, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(73386408,new Vector3(-348.8109f, -47.26213f, 49.38759f)) { LockWhenClosed = true },//Front Door1
                   new InteriorDoor(3142793112,new Vector3(-351.2598f, -46.41221f, 49.38765f)) { LockWhenClosed = true },//Front Door1
                    new InteriorDoor(4163212883, new Vector3(-355.3892f, -51.06768f, 49.31105f)) { ForceRotateOpen = true },//teller door
                } },
            new Interior(76802,"Fleeca Bank"){
               IsWeaponRestricted = true
               , Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(3142793112,new Vector3(149.6298f, -1037.231f, 29.71915f)){ LockWhenClosed = true, } ,//Front Door1
                    new InteriorDoor(73386408,new Vector3(152.0632f, -1038.124f, 29.71909f)) { LockWhenClosed = true, } ,//Front Door2
                    new InteriorDoor(4163212883, new Vector3(145.4186f,-1041.813f,29.64255f)) { ForceRotateOpen = true },//teller door
                } },
            new Interior(11266,"Fleeca Bank") { 
                IsWeaponRestricted = true,
                Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(73386408,new Vector3(316.3925f, -276.4888f, 54.5158f)) { LockWhenClosed = true }, //Front Door1
                    new InteriorDoor(3142793112,new Vector3(313.9587f, -275.5965f, 54.51586f)) { LockWhenClosed = true }, //Front Door2
                    new InteriorDoor(4163212883, new Vector3(309.7491f, -280.1797f, 54.43926f)) { ForceRotateOpen = true },//teller door
                } },
            new Interior(20226,"Fleeca Bank") {
               IsWeaponRestricted = true, Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(3142793112,new Vector3(-2965.821f, 481.6297f, 16.04816f)) { LockWhenClosed = true }, //Front Door1
                    new InteriorDoor(73386408,new Vector3(-2965.71f, 484.2195f, 16.0481f)) { LockWhenClosed = true }, //Front Door2
                    new InteriorDoor(4163212883, new Vector3(-2960.176f, 479.0105f, 15.97156f)) { ForceRotateOpen = true },//teller door
                } },
            new Interior(90626,"Fleeca Bank") {
               IsWeaponRestricted = true, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(3142793112,new Vector3(1176.495f, 2703.613f, 38.43911f)) { LockWhenClosed = true },
                   new InteriorDoor(73386408,new Vector3(1173.903f, 2703.613f, 38.43904f)) { LockWhenClosed = true },
                    new InteriorDoor(4163212883, new Vector3(1178.87f, 2709.365f, 38.36251f)) { ForceRotateOpen = true },//teller door
                } },
            new Interior(87810,"Fleeca Bank") {
               IsWeaponRestricted = true, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(3142793112,new Vector3(-1215.386f, -328.5237f, 38.13211f)) { LockWhenClosed = true },
                   new InteriorDoor(73386408,new Vector3(-1213.074f, -327.3524f, 38.13205f)) { LockWhenClosed = true },
                    new InteriorDoor(4163212883, new Vector3(-1214.906f, -334.7281f, 38.05551f)) { ForceRotateOpen = true },//teller door
                } },



            //4163212883

            new Interior(103170,"Pacific Standard Bank") {
                IsWeaponRestricted = true,Doors =  new List<InteriorDoor>() {

                    new InteriorDoor(2253282288,new Vector3(232.6054f, 214.1584f, 106.4049f)) { LockWhenClosed = true },//FRONT LEFT
                    new InteriorDoor(2253282288,new Vector3(231.5075f, 216.5148f, 106.4049f)) { LockWhenClosed = true },//FRONT RIGHT


                    new InteriorDoor(1335309163,new Vector3(260.6518f, 203.2292f, 106.4328f)) { LockWhenClosed = true },//BACK LEFT
                    new InteriorDoor(1335309163,new Vector3(258.2093f, 204.119f, 106.4328f)) { LockWhenClosed = true },//BACK RIGHT
                    new InteriorDoor(4072696575, new Vector3(256.3116f,220.6579f,106.4296f)) { LockWhenClosed = true },//teller door
                } },


            new Interior(42754,"Blaine County Savings") {
                IsWeaponRestricted = true,Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(3110375179, new Vector3(-108.9147f,6469.105f,31.91028f)) { LockWhenClosed = true },//teller
                    new InteriorDoor(2628496933, new Vector3(-109.65f,6462.11f,31.98499f)) { LockWhenClosed = true },//FRONT 1
                    new InteriorDoor(3941780146, new Vector3(-111.48f,6463.94f,31.98499f)) { LockWhenClosed = true },//FRONT 2
                    //new InteriorDoor(-1184592117, new Vector3(-108.9147f,6469.105f,31.91028f)),//teller
                    //new InteriorDoor(-1666470363, new Vector3(-109.65f,6462.11f,31.98499f)),//FRONT 1
                    //new InteriorDoor(-353187150, new Vector3(-111.48f,6463.94f,31.98499f)),//FRONT 2
                } },


            //103170
        });
    }
    private void Tunnels()
    {
        LocationsList.AddRange(new List<Interior>()
        {
            new Interior(96002,"Zancudo Tunnel"),
            new Interior(104706,"Zancudo Tunnel"),
            new Interior(81154,"Braddock Tunnel"),
            new Interior(40450,"Braddock Tunnel"),
            new Interior(19714,"Braddock Tunnel"),
            new Interior(7682,"Integrity Way Tunnel"),
            new Interior(50946,"Integrity Way Tunnel"),
            new Interior(75010,"Integrity Way Tunnel"),


            new Interior(6146,"Del Perro Tunnel"),
            new Interior(96770,"Del Perro Tunnel"),
            new Interior(86274,"Del Perro Tunnel"),
            new Interior(105218,"Del Perro Tunnel"),



            new Interior(38658,"South LS Rail Tunnel"),
            new Interior(100866,"South LS Rail Tunnel"),
            new Interior(83202,"South LS Rail Tunnel"),
            new Interior(110850,"South LS Rail Tunnel"),
            new Interior(28674,"South LS Rail Tunnel"),
            new Interior(49154,"South LS Rail Tunnel"),
            new Interior(27650,"South LS Rail Tunnel"),
            new Interior(101122,"South LS Rail Tunnel"),


            new Interior(111362,"Del Perro Canal Access"),
            new Interior(118530,"Del Perro Canal Access"),
            new Interior(97282,"Del Perro Canal Access"),
            new Interior(104194,"Del Perro Canal Access"),
            new Interior(113410,"Del Perro Canal Access"),


            new Interior(108802,"Raton Canyon Rail Tunnel"),
            new Interior(112898,"Raton Canyon Rail Tunnel"),
            new Interior(20994,"Raton Canyon Rail Tunnel"),
            new Interior(29442,"Raton Canyon Rail Tunnel"),
            new Interior(12034,"Raton Canyon Rail Tunnel"),
            new Interior(16130,"Raton Canyon Rail Tunnel"),
            new Interior(117762,"Raton Canyon Rail Tunnel"),
            new Interior(5890,"Raton Canyon Rail Tunnel"),
            new Interior(97026,"Raton Canyon Rail Tunnel"),
            new Interior(71170,"Raton Canyon Rail Tunnel"),



            new Interior(14082,"Downtown LS Sewer"),
            new Interior(42242,"Downtown LS Sewer"),
            new Interior(45826,"Downtown LS Sewer"),
            new Interior(55810,"Downtown LS Sewer"),
            new Interior(57346,"Downtown LS Sewer"),
            new Interior(109826,"Downtown LS Sewer"),
            new Interior(10498,"Downtown LS Sewer"),
            new Interior(71938,"Downtown LS Sewer"),
            new Interior(43266,"Downtown LS Sewer"),
            new Interior(26114,"Downtown LS Sewer"),
            new Interior(9986,"Downtown LS Sewer"),
            new Interior(120322,"Downtown LS Sewer"),
            new Interior(36610,"Downtown LS Sewer"),
            new Interior(104962,"Downtown LS Sewer"),
            new Interior(23298,"Downtown LS Sewer"),


            new Interior(5634,"Burton Subway Station"),
            new Interior(32770,"Burton Subway Station"),
            new Interior(99842,"Burton Subway Station"),



            new Interior(68866,"Little Seoul Subway Station"),
            new Interior(20482,"Little Seoul Subway Station"),
            new Interior(16898,"Little Seoul Subway Station"),

            new Interior(98306,"Del Perro Subway Station"),
            new Interior(5122,"Del Perro Subway Station"),
            new Interior(32002,"Del Perro Subway Station"),


            new Interior(84482,"Portola Drive Subway Station"),
            new Interior(91650,"Portola Drive Subway Station"),
            new Interior(50690,"Portola Drive Subway Station"),

            new Interior(62466,"LSIA Terminal 4 Subway Station"),
            new Interior(108546,"LSIA Terminal 4 Subway Station"),
            new Interior(77314,"LSIA Terminal 4 Subway Station"),

            new Interior(61698,"LSIA Parking Subway Station"),
            new Interior(65538,"LSIA Parking Subway Station"),
            new Interior(111106,"LSIA Parking Subway Station"),

            //"LSIA Parking Subway Station"
        });
    }
    private void Stations()
    {
        LocationsList.AddRange(new List<Interior>()
        {
            new Interior(81666,"LSCFD Fire Station 7"),
            new Interior(30978,"Mission Row Police Station"),
            new Interior(58882,"FIB Headquarters",new List<string>() { "FIBlobby" },new List<string>() { "FIBlobbyfake" },new List<InteriorDoor>() { new InteriorDoor(-1517873911, new Vector3(106.3793f, -742.6982f, 46.51962f)),new InteriorDoor(-90456267, new Vector3(105.7607f, -746.646f, 46.18266f))}),
            new Interior(-787,"Pill Box Hill Medical Center",new List<string>() { "RC12B_Default" },new List<string>() { "RC12B_Destroyed","RC12B_HospitalInterior","RC12B_Fixed" }),
            new Interior(60418,"Los Santos County Coroner Office",new List<string>() { "Coroner_Int_on","coronertrash" }) { IsTeleportEntry = true,InteriorEgressPosition = new Vector3(253.351f, -1364.622f, 39.53437f), InteriorEgressHeading = 327.1821f },
            new Interior(3842,"Paleto Bay Sheriff's Office",new List<string>() { "v_sheriff2" },new List<string>() { "cs1_16_sheriff_cap" },new List<InteriorDoor>() { new InteriorDoor(-1501157055, new Vector3(-444.4985f, 6017.06f, 31.86633f)),new InteriorDoor(-1501157055, new Vector3(-442.66f, 6015.222f, 31.86633f))}) { DisabledInteriorCoords = new Vector3(-444.89068603515625f, 6013.5869140625f, 30.7164f) },
        });
    }
    private void Other()
    {
        LocationsList.AddRange(new List<Interior>()
        {


            new Interior(-667,"Motel Inside 1",new List<string>() {  },new List<string>() {  }) { 
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(151.817f, -1006.616f, -98.99998f),//new Vector3(151.817f, -1006.616f, -98.99998f),
                InteriorEgressHeading = 340.2548f,
                StandardInteractLocation = new Vector3(152.0395f, -1001.007f, -98.99998f), 
               // StandardInteractHeading = 89.01726f,
               // BathroomLocation = new Vector3(154.3581f, -1001.021f, -98.99998f),
               // BathroomHeading = 2.820992f,
              //  BedLocation = new Vector3(154.2265f, -1006.057f, -98.99998f),
               // BedHeading = 273.5557f,
                StandardInteractCameraPosition = new Vector3(154.3757f, -1006.959f, -97.54375f), 
                StandardInteractCameraDirection = new Vector3(-0.3906728f, 0.8876943f, -0.2436668f), 
                StandardInteractCameraRotation = new Rotator(-14.10306f, -3.961381E-06f, 23.75422f),

            },
            new Interior(-668,"Low End Apartment") {                 
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(266.3692f, -1004.84f, -99.41235f),//new Vector3(261.4586f, -998.8196f, -99.00863f),
                InteriorEgressHeading = 5.047247f,
                StandardInteractLocation = new Vector3(260.0411f, -1003.866f, -99.00858f),
                StandardInteractCameraPosition = new Vector3(259.6914f, -1001.466f, -98.10947f), 
                StandardInteractCameraDirection = new Vector3(0.6112908f, -0.737038f, -0.2882683f), 
                StandardInteractCameraRotation = new Rotator(-16.75431f, 6.241364E-06f, -140.3281f),//DONE AND DONE
            },
            new Interior(-669,"Medium Apartment") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(289.1848f,-997.1297f,-92.79259f),//new Vector3(288.3021f, -998.6495f, -92.79259f),
                InteriorEgressHeading = 288.6531f,
                StandardInteractLocation = new Vector3(302.9109f,-999.1298f,-94.19514f), //new Vector3(303.1596f, -999.4008f, -94.19513f),
                StandardInteractHeading = 128.1528f,
                StandardInteractCameraPosition = new Vector3(304.4328f, -993.7673f, -91.80285f),
                StandardInteractCameraDirection = new Vector3(-0.4074028f, -0.8461376f, -0.343619f),
                StandardInteractCameraRotation = new Rotator(-20.09752f, -1.181871E-05f, 154.2899f)
                //StandardInteractCameraPosition = new Vector3(305.1868f, -992.3067f, -92.05402f),
                //StandardInteractCameraDirection = new Vector3(-0.3786451f, -0.8935453f, -0.2412563f),
                //StandardInteractCameraRotation = new Rotator(-13.9607f, 4.398803E-06f, 157.0349f)

            },
            new Interior(-670,"4 Integrity Way, Apt 28") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-16.20265f, -606.2855f, 100.2328f),//new Vector3(-18.07856f, -583.6725f, 79.46569f),
                InteriorEgressHeading = 4.009807f,
                StandardInteractLocation = new Vector3(-10.32665f, -591.5042f, 98.83028f),
                StandardInteractHeading = 42.31462f,
                StandardInteractCameraPosition = new Vector3(-15.6437f, -588.6951f, 101.2196f),
                StandardInteractCameraDirection = new Vector3(0.6824573f, -0.6314822f, -0.368079f),
                StandardInteractCameraRotation = new Rotator(-21.59719f, -1.010063E-05f, -132.7783f)


                //StandardInteractCameraPosition = new Vector3(-36.37276f, -579.1055f, 90.60618f), 
                //StandardInteractCameraDirection = new Vector3(0.8701445f, 0.3863149f, -0.3059565f), 
                //StandardInteractCameraRotation = new Rotator(-17.81572f, -1.838395E-05f, -66.06039f)
            },



            new Interior(-671,"4 Integrity Way, Apt 30") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-19.61186f, -581.8444f, 90.11483f), //new Vector3(-35.31277f, -580.4199f, 88.71221f),
                InteriorEgressHeading = 91.7578f,
                StandardInteractLocation = new Vector3(-33.56378f, -576.7966f, 88.71226f),//new Vector3(-35.31277f, -580.4199f, 88.71221f),
                StandardInteractHeading = 276.5922f,
                //StandardInteractCameraPosition = new Vector3(-14.33171f, -589.8526f, 100.7934f), 
                //StandardInteractCameraDirection = new Vector3(0.6419793f, -0.680028f, -0.3541532f), 
                //StandardInteractCameraRotation = new Rotator(-20.74156f, -3.651775E-06f, -136.6486f),

                StandardInteractCameraPosition = new Vector3(-36.54444f, -579.4444f, 90.46429f),
                StandardInteractCameraDirection = new Vector3(0.7369682f, 0.5456903f, -0.3988734f),
                StandardInteractCameraRotation = new Rotator(-23.50777f, -6.517313E-06f, -53.48179f)



            },






            new Interior(-672,"Dell Perro Heights, Apt 4") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-1458.708f, -522.3859f, 69.55659f),
                InteriorEgressHeading = 151.7898f,
                StandardInteractLocation = new Vector3(-1469.23f, -530.2661f, 68.15405f),
                StandardInteractHeading = 32.30866f,
                StandardInteractCameraPosition = new Vector3(-1468.288f, -537.0152f, 70.50282f),
                StandardInteractCameraDirection = new Vector3(-0.2367817f, 0.938622f, -0.2508448f),
                StandardInteractCameraRotation = new Rotator(-14.52751f, 6.614796E-07f, 14.15833f)
            },
            new Interior(-673,"Dell Perro Heights, Apt 7") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-1458.221f, -523.283f, 56.92899f),
                InteriorEgressHeading = 144.4565f,
                StandardInteractLocation = new Vector3(-1469.766f, -529.8615f, 55.52639f),
                StandardInteractHeading = 23.87169f,
                StandardInteractCameraPosition = new Vector3(-1469.822f, -535.2637f, 57.56692f), 
                StandardInteractCameraDirection = new Vector3(-0.08299411f, 0.9490341f, -0.3040497f), 
                StandardInteractCameraRotation = new Rotator(-17.701f, 1.960445E-05f, 4.997866f)
            },
            new Interior(-674,"Eclipse Towers, Apt 3") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-779.4249f, 339.4756f, 207.6208f),
                InteriorEgressHeading = 113.7695f,
                StandardInteractLocation = new Vector3(-792.1093f, 338.2278f, 206.2184f),
                StandardInteractHeading = 11.25924f,
                StandardInteractCameraPosition = new Vector3(-795.7065f, 332.019f, 208.7504f), 
                StandardInteractCameraDirection = new Vector3(0.3429281f, 0.8894085f, -0.3022463f), 
                StandardInteractCameraRotation = new Rotator(-17.59257f, -4.478318E-07f, -21.08508f)
            },


            new Interior(-675,"Richard Majestic, Apt 2") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-915.4658f, -367.6183f, 109.4403f),
                InteriorEgressHeading = 142.9073f,
                StandardInteractLocation = new Vector3(-926.6827f, -373.9506f, 108.0377f),
                StandardInteractHeading = 29.87866f,
                StandardInteractCameraPosition = new Vector3(-926.1729f, -380.4109f, 110.0456f), 
                StandardInteractCameraDirection = new Vector3(-0.1600053f, 0.9530545f, -0.2570709f), 
                StandardInteractCameraRotation = new Rotator(-14.89633f, -2.208661E-06f, 9.530333f)
            },

            new Interior(-676,"Tinsel Towers, Apt 42") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-601.3342f, 63.16429f, 108.027f),
                InteriorEgressHeading = 102.5944f,
                StandardInteractLocation = new Vector3(-614.5223f, 63.66457f, 106.6245f),
                StandardInteractHeading = 350.1266f,
                StandardInteractCameraPosition = new Vector3(-617.3007f, 59.07885f, 109.0985f), 
                StandardInteractCameraDirection = new Vector3(0.3802197f, 0.8753764f, -0.2985786f), 
                StandardInteractCameraRotation = new Rotator(-17.37225f, 1.744431E-05f, -23.47771f)
            },

            //

            //
            /*            ,new InteriorPosition("4 Integrity Way, Apt 30",new Vector3(-35.31277f, -580.4199f, 88.71221f))//works
            ,new InteriorPosition("Dell Perro Heights, Apt 4",new Vector3(-1468.14f, -541.815f, 73.4442f))//works
            ,new InteriorPosition("Dell Perro Heights, Apt 7",new Vector3(-1477.14f, -538.7499f, 55.5264f))//works
            ,new InteriorPosition("Eclipse Towers, Apt 3",new Vector3(-773.407f, 341.766f, 211.397f))//works*/


            new Interior(78338,"Maze Bank Arena",new List<string>() { "sp1_10_real_interior" },new List<string>() { "sp1_10_fake_interior" }),   
            new Interior(31746,"O'Neil Ranch",
                new List<string>() { "farm", "farmint", "farm_lod", "farm_props","des_farmhs_startimap","des_farmhs_start_occl" },
                new List<string>() { "farm_burnt", "farm_burnt_lod", "farm_burnt_props", "farmint_cap", "farmint_cap_lod", "des_farmhouse", "des_farmhs_endimap", "des_farmhs_end_occl"}),
            new Interior(3330,"Lifeinvader",new List<string>() { "facelobby","facelobby_lod" },new List<string>() { "facelobbyfake","facelobbyfake_lod" }),
            new Interior(119042,"Union Depository",new List<string>() { "FINBANK" },new List<string>() { }){ IsRestricted = true },
            new Interior(28162,"Clucking Bell Farms",new List<string>() { "CS1_02_cf_onmission1","CS1_02_cf_onmission2","CS1_02_cf_onmission3","CS1_02_cf_onmission4" },new List<string>() { "CS1_02_cf_offmission" }){ IsRestricted = true },
            new Interior(35330,"Clucking Bell Farms",new List<string>() {  },new List<string>() {  }){ IsRestricted = true },
            new Interior(67074,"Clucking Bell Farms",new List<string>() {  },new List<string>() {  }){ IsRestricted = true },
            new Interior(75778,"Clucking Bell Farms",new List<string>() { },new List<string>() { }) { IsRestricted = true },
            new Interior(72706,"Tequila-La",
                new List<string>() { "v_rockclub" },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(993120320, new Vector3(-565.1712f, 276.6259f, 83.28626f)),
                    new InteriorDoor(993120320, new Vector3(-561.2866f, 293.5044f, 87.77851f))})
            { DisabledInteriorCoords = new Vector3(-556.5089111328125f, 286.318115234375f, 81.1763f) },
            new Interior(107778,"Bahama Mama's",
                new List<string>() { "v_bahama" },
                new List<string>() { },
                new List<InteriorDoor>() { })
            { DisabledInteriorCoords = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f) },

            new Interior(24578,"7611 Goma St",
                new List<string>() {  },
                new List<string>() { "vb_30_crimetape" },
                new List<InteriorDoor>() {
                    new InteriorDoor(-607040053, new Vector3(-1149.709f, -1521.088f, 10.78267f)),})
            { DisabledInteriorCoords = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f),InteriorSets = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A","swap_mrJam_A" } },
            new Interior(89602,"Yellow Jacket Inn"),
            new Interior(118018,"Vanilla Unicorn"),

            //new Interior(171777,"Apartment"){ RemoveIPLs = new List<string>() { "vb_30_crimetape"}, InteriorSets = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A","swap_mrJam_A" } },
            //new Interior(92674,"Darnell Bros. Garments",new List<string>() { "id2_14_during1","id2_14_during_door" },new List<string>() {"id2_14_during_door","id2_14_during1","id2_14_during2","id2_14_on_fire","id2_14_post_no_int","id2_14_pre_no_int" }),//top floor works and doors are open, but no interiror>?
            //new Interior(-103,"Rogers Salvage & Scrap",new List<string>() { "sp1_03_interior_v_recycle_milo_","sp1_03","sp1_03_critical_0","sp1_03_grass_0","sp1_03_long_0","sp1_03_strm_0","sp1_03_strm_1" },new List<string>() { }),//doesnt work at all, does nothing
            new Interior(-103,"Rogers Salvage & Scrap",new List<string>() {  },new List<string>() { }) { 
                Doors = new List<InteriorDoor>() { 
                    new InteriorDoor(812467272,new Vector3(-589.5225f, -1621.513f, 33.16225f)),//Inside
                    new InteriorDoor(812467272,new Vector3(-590.8179f, -1621.425f, 33.16282f)),//Inside

                    new InteriorDoor(2667367614,new Vector3(-611.32f, -1610.089f, 27.15894f)),//Front Outside R
                    new InteriorDoor(1099436502,new Vector3(-608.7289f, -1610.315f, 27.15894f)),//Front Ouitside L

                    new InteriorDoor(2667367614,new Vector3(-592.7109f, -1628.986f, 27.15931f)),//Rear Outside R
                    new InteriorDoor(1099436502,new Vector3(-592.9376f, -1631.577f, 27.15931f)),//Rear Ouitside L
                    //doors dont stay open

                }, 
                InternalInteriorCoordinates = new Vector3(-609.962f, -1612.49f, 27.0105f), 
                NeedsActivation = true },






            //.

            //Old Generic Stuff, i dont think we are loading any of this 
            new Interior(25090,"Mission Carpark") { IsSPOnly = true } ,
            new Interior(39682,"Torture Room") { IsSPOnly = true } ,
            new Interior(76290,"Dell Perro Heights, Apt 4") { IsSPOnly = true } ,
            new Interior(108290,"Low End Apartment") { IsSPOnly = true } ,
            new Interior(69890,"IAA Office") { IsSPOnly = true } ,
            new Interior(25602,"Dell Perro Heights, Apt 7") { IsSPOnly = true } ,
            new Interior(31490,"FIB Floor 47") { IsSPOnly = true } ,
            new Interior(135973,"FIB Floor 49") { IsSPOnly = true } ,
            new Interior(60162,"Motel") { IsSPOnly = true } ,
            new Interior(69122,"Lester's House") { IsSPOnly = true } ,
            new Interior(47362,"4 Integrity Way, Apt 30") { IsSPOnly = true } ,
            new Interior(28418,"FIB Top Floor") { IsSPOnly = true } ,
            new Interior(70146,"10 Car Garage") { IsSPOnly = true } ,
            new Interior(85250,"Omega's Garage") { IsSPOnly = true } ,
            new Interior(61186,"Eclipse Towers, Apt 3") { IsSPOnly = true } ,
            new Interior(94722,"Booking Room") { IsSPOnly = true }
            ,new Interior(146433, "10 Car") { IsMPOnly = true }
            ,new Interior(149761, "Low End Apartment") { IsMPOnly = true }
            ,new Interior(141313, "4 Integrity Way, Apt 30") { IsMPOnly = true }
            ,new Interior(145921, "Dell Perro Heights, Apt 4") { IsMPOnly = true }
            ,new Interior(145665, "Dell Perro Heights, Apt 7") { IsMPOnly = true }
            ,new Interior(146945, "Eclipse Towers, Apt 3") { IsMPOnly = true }
            ,new Interior(94722, "CharCreator") { IsMPOnly = true }
            ,new Interior(25090, "Mission Carpark") { IsMPOnly = true }
            ,new Interior(156929, "Torture Room") { IsMPOnly = true }
            ,new Interior(178433, "Omega's Garage") { IsMPOnly = true }
            ,new Interior(149505, "Motel") { IsMPOnly = true }
            ,new Interior(250881, "Lester's House") { IsMPOnly = true }
            ,new Interior(136449, "FBI Top Floor") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 47") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 49") { IsMPOnly = true }
            ,new Interior(135681, "IAA Office") { IsMPOnly = true }

            ,new Interior(149249, "2 Car") { IsMPOnly = true }
            ,new Interior(148737, "6 Car") { IsMPOnly = true }
            ,new Interior(148225, "Medium End Apartment") { IsMPOnly = true }
            ,new Interior(147201, "4 Integrity Way, Apt 28") { IsMPOnly = true }
            ,new Interior(146177, "Richard Majestic, Apt 2") { IsMPOnly = true }
            ,new Interior(146689, "Tinsel Towers, Apt 42") { IsMPOnly = true }
            ,new Interior(207105, "3655 Wild Oats Drive") { IsMPOnly = true }
            ,new Interior(206081, "2044 North Conker Avenue") { IsMPOnly = true }
            ,new Interior(206337, "2045 North Conker Avenue") { IsMPOnly = true }
            ,new Interior(208129, "2862 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(207617, "2868 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(207361, "2874 Hillcrest Avenue") { IsMPOnly = true }
            ,new Interior(206593, "2677 Whispymound Drive") { IsMPOnly = true }
            ,new Interior(208385, "2133 Mad Wayne Thunder") { IsMPOnly = true }
            ,new Interior(258561, "Bunker Interior") { IsMPOnly = true }
            ,new Interior(164865, "Solomon's Office") { IsMPOnly = true }
            ,new Interior(170497, "Psychiatrist's Office") { IsMPOnly = true }
            ,new Interior(165889, "Movie Theatre") { IsMPOnly = true }
            ,new Interior(205825, "Madrazos Ranch") { IsMPOnly = true }
            ,new Interior(260353, "Smuggler's Run Hangar") { IsMPOnly = true }
            ,new Interior(262145, "Avenger Interior") { IsMPOnly = true }
            ,new Interior(269313, "Facility") { IsMPOnly = true }
            ,new Interior(270337, "Server Farm") { IsMPOnly = true }
            ,new Interior(271105, "Submarine") { IsMPOnly = true }
            ,new Interior(270081, "IAA Facility") { IsMPOnly = true }
            ,new Interior(271617, "Nightclub") { IsMPOnly = true }
            ,new Interior(271873, "Nightclub Warehouse") { IsMPOnly = true }
            ,new Interior(272129, "Terrorbyte Interior") { IsMPOnly = true }
        });
    }
}

