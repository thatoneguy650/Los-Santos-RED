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
using System.Windows.Media;


public class Interiors : IInteriors
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Interiors.xml";
    public Interiors()
    {
        PossibleInteriors = new PossibleInteriors();
    }

    public PossibleInteriors PossibleInteriors { get; private set; }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Interiors*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Interiors config  {ConfigFile.FullName}",0);
            PossibleInteriors = Serialization.DeserializeParam<PossibleInteriors>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Interiors config  {ConfigFileName}",0);
            PossibleInteriors = Serialization.DeserializeParam<PossibleInteriors>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Interiors config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        Stores();
        Tunnels();
        Stations();
        Other();
        Residence();
        GangDens();
        Serialization.SerializeParam(PossibleInteriors, ConfigFileName);
    }
    public List<Interior> GetAllPlaces()
    {
        return PossibleInteriors.AllInteriors();
    }
    public Interior GetInteriorByLocalID(int id)
    {
        return PossibleInteriors.AllInteriors().Where(x => x.LocalID == id).FirstOrDefault();
    }
    public Interior GetInteriorByInternalID(int id)
    {
        return PossibleInteriors.AllInteriors().Where(x => x.InternalID == id).FirstOrDefault();
    }

    private void Stores()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Clothes
            new Interior(19458,"Sub Urban") { IsWeaponRestricted = true, },
            new Interior(22786, "BINCO Textile City",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3146141106, new Vector3(418.5713f,-808.674f,29.64108f)),
                    new InteriorDoor(868499217, new Vector3(418.5713f,-806.3979f,29.64108f)) }) { IsWeaponRestricted = true, },//doesntwork?
            new Interior(10754,"Sub Urban") { IsWeaponRestricted = true, },
            new Interior(1282,"Ponsonby") { IsWeaponRestricted = true, },
            new Interior(14338,"Ponsonby") { IsWeaponRestricted = true, },
            new Interior(22786,"Binco") { IsWeaponRestricted = true, },
            new Interior(96266,"Suburban") { IsWeaponRestricted = true, },//suburban harmony
            new Interior(17154,"Binco") { IsWeaponRestricted = true, },
            new Interior(88066,"Discount Store") { IsWeaponRestricted = true, },

            //Ammunations
            new Interior(-555,"Ammunation Vespucci Boulevard",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(842.7685f, -1024.539f, 28.34478f)),
                    new InteriorDoor(97297972, new Vector3(845.3694f, -1024.539f, 28.34478f)),}) { IsWeaponRestricted = true, },
            new Interior(-554,"Ammunation Lindsay Circus",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(-662.6415f, -944.3256f, 21.97915f)),
                    new InteriorDoor(97297972, new Vector3(-665.2424f, -944.3256f, 21.97915f)) }) { IsWeaponRestricted = true, },
            new Interior(29698,"Ammu Nation Vinewood Plaza",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(-8873588, new Vector3(243.8379f, -46.52324f, 70.09098f)),
                    new InteriorDoor(97297972, new Vector3(244.7275f, -44.07911f, 70.09098f)) }) { IsWeaponRestricted = true, },
            new Interior(80386,"Ammunation Cypress Flats"),
            new Interior(48130,"Ammunation Pillbox Hill") { IsWeaponRestricted = true, },
            new Interior(35586,"Ammunation") { IsWeaponRestricted = true, },

            //24/7
            new Interior(41474,"24/7 Route 68",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(545.504f,2672.745f,42.30644f)),//left door
                    new InteriorDoor(997554217, new Vector3(542.9252f,2672.406f,42.30644f))}) { IsWeaponRestricted = true, },//right door
            new Interior(16386,"24/7 Chumash",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3240.128f,1003.157f,12.98064f)),//left door
                    new InteriorDoor(997554217, new Vector3(-3239.905f,1005.749f,12.98064f))}) { IsWeaponRestricted = true, },//right door
            new Interior(62722,"24/7 Palomino Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2559.201f,384.0875f,108.7729f)),
                    new InteriorDoor(997554217, new Vector3(2559.304f,386.6865f,108.7729f)),}) { IsWeaponRestricted = true, },//right door
            new Interior(33282,"24/7 Strawberry",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(27.81761f,-1349.169f,29.64696f)),
                    new InteriorDoor(997554217, new Vector3(30.4186f,-1349.169f,29.64696f)),}) { IsWeaponRestricted = true, },//right door
            new Interior(97538,"24/7 Banham Canyon",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3038.219f,588.2872f,8.058861f)),
                    new InteriorDoor(997554217, new Vector3(-3039.012f,590.7643f,8.058861f)),}) { IsWeaponRestricted = true, },//right door    
            new Interior(46850,"24/7 Downtown Vinewood",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(375.3528f,323.8015f,103.7163f)),
                    new InteriorDoor(997554217, new Vector3(377.8753f,323.1672f,103.7163f)),}) { IsWeaponRestricted = true, },//right door   
            new Interior(36354,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1437777724, new Vector3(1732.245f,6415.377f,34.76194f)),
                    new InteriorDoor(1421582485, new Vector3(1734.097f,6413.048f,34.99545f)),}) { IsWeaponRestricted = true, },//right door   
            new Interior(13826,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2681.292f,3281.427f,55.39108f)),
                    new InteriorDoor(997554217, new Vector3(2682.558f,3283.698f,55.39108f)),}) { IsWeaponRestricted = true, },//right door          
            new Interior(55554,"24/7 Sandy Shores",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(1963.917f,3740.075f,32.49369f)),
                    new InteriorDoor(997554217, new Vector3(1966.17f,3741.376f,32.49369f)),}) { IsWeaponRestricted = true, },//right door 

            //LtD
            new Interior(47874, "Ltd Little Seoul",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-713.0732f,-916.5409f,19.36553f)),
                    new InteriorDoor(2065277225, new Vector3(-710.4722f,-916.5372f,19.36553f)),}),// { IsSPOnly = true},//right door  
            new Interior(45570, "Ltd Grapeseed",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1699.661f,4930.278f,42.21359f)),
                    new InteriorDoor(2065277225, new Vector3(1698.172f,4928.146f,42.21359f)),}),// { IsSPOnly = true},//right door  
            new Interior(80642,"LtD Davis",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-53.96111f,-1755.717f,29.57094f)),
                    new InteriorDoor(2065277225, new Vector3(-51.96669f,-1757.387f,29.57094f)),}) { IsWeaponRestricted = true, },//right door   
            new Interior(2050,"LtD Mirror Park",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1158.364f,-326.8165f,69.35503f)),
                    new InteriorDoor(2065277225, new Vector3(1160.925f,-326.3612f,69.35503f)),}) { IsWeaponRestricted = true, },//right door   
            new Interior(82178,"LtD Richman Glen",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1158.364f,-326.8165f,69.35503f)),
                    new InteriorDoor(2065277225, new Vector3(1160.925f,-326.3612f,69.35503f)),}) { IsWeaponRestricted = true, },//right door  
            new Interior(74874,"LtD Gas") { IsWeaponRestricted = true, },

            //Liquor
            new Interior(33026,"Scoops Liquor Barn"),                
            new Interior(104450,"Liquor Ace"),     
            new Interior(50178,"Rob's Liquors",//San Andreas Ave Del Perro
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1226.894f,-903.1218f,12.47039f)){ LockWhenClosed = true },
                }) { IsWeaponRestricted = true, },
            new Interior(19202,"Rob's Liquors",//Route 1
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-2973.535f,390.1414f,15.18735f)){ LockWhenClosed = true },
                }) { IsWeaponRestricted = true, },
            new Interior(98818,"Rob's Liquors",//Prosperity Street
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1490.411f,-383.8453f,40.30745f)){ LockWhenClosed = true },
                }) { IsWeaponRestricted = true, },
            new Interior(73986,"Rob's Liquors",//El Rancho Boulevard
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(1141.038f,-980.3225f,46.55986f)) { LockWhenClosed = true },
                }) { IsWeaponRestricted = true, },
            new Interior(50178,"Rob's Liquors") { IsWeaponRestricted = true, },

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
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
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
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            new Interior(81666,"LSCFD Fire Station 7"),
            new Interior(-707,"Sandy Shores Sheriff's Station", new List<string>() { "v_sheriff" },new List<string>() { "sheriff_cap" }) 
            { 
                NeedsActivation = true, 
                InternalInteriorCoordinates = new Vector3(1853.66f, 3686.13f, 35.07882f),
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(2529918806, new Vector3(1855.685f, 3683.93f, 34.59282f)),
                },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("ssssStandard1",new Vector3(1854.015f, 3687.735f, 34.26704f), 27.68711f,"Interact With Front Desk")
                    {
                        CameraPosition = new Vector3(1852.45f, 3684.733f, 35.32358f), 
                        CameraDirection = new Vector3(0.2323246f, 0.96091f, -0.1505897f), 
                        CameraRotation = new Rotator(-8.661105f, 2.159055E-07f, -13.59189f)
                    } ,
                },
            },

            new Interior(30978,"Mission Row Police Station")
            {
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("mrpdStandard1",new Vector3(441.8724f, -981.4156f, 30.68966f), 0.7297109f,"Interact With Front Desk")
                    {
                        CameraPosition = new Vector3(437.5189f, -984.9877f, 32.48894f), 
                        CameraDirection = new Vector3(0.6041811f, 0.7704815f, -0.2032817f), 
                        CameraRotation = new Rotator(-11.72893f, -3.923911E-06f, -38.10214f),
                    } ,
                    new StandardInteriorInteract("mrpdStandard2",new Vector3(447.2735f, -980.8964f, 30.68964f), 2.435939f,"Interact With Office"),
                },
            },
            new Interior(58882,"FIB Headquarters",new List<string>() { "FIBlobby" },new List<string>() { "FIBlobbyfake" },new List<InteriorDoor>() { new InteriorDoor(-1517873911, new Vector3(106.3793f, -742.6982f, 46.51962f)),new InteriorDoor(-90456267, new Vector3(105.7607f, -746.646f, 46.18266f))})
            { 
                InteractPoints = new List<InteriorInteract>()
                {
                        new StandardInteriorInteract("fibstandard1",new Vector3(114.9295f, -748.8344f, 45.75159f), 292.533f,"Interact With Front Desk")
                        {
                            CameraPosition = new Vector3(111.1688f, -746.9386f, 47.35395f), 
                            CameraDirection = new Vector3(0.9294057f, -0.2647795f, -0.257093f), 
                            CameraRotation = new Rotator(-14.89764f, -2.208675E-06f, -105.9018f)
                        } ,
                },
            },
            new Interior(-787,"Pill Box Hill Medical Center",new List<string>() { "RC12B_Default" },new List<string>() { "RC12B_Destroyed","RC12B_HospitalInterior","RC12B_Fixed" }),
            new Interior(60418,"Los Santos County Coroner Office",new List<string>() { "Coroner_Int_on","coronertrash" })
            { 
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(253.351f, -1364.622f, 39.53437f), 
                InteriorEgressHeading = 327.1821f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("morgueExit1",new Vector3(253.351f, -1364.622f, 39.53437f),327.1821f,"Exit") ,

                },
            },
            new Interior(3842,"Paleto Bay Sheriff's Office",new List<string>() { "v_sheriff2" },new List<string>() { "cs1_16_sheriff_cap" },new List<InteriorDoor>() { new InteriorDoor(-1501157055, new Vector3(-444.4985f, 6017.06f, 31.86633f)),new InteriorDoor(-1501157055, new Vector3(-442.66f, 6015.222f, 31.86633f))}) 
            {
                InteractPoints = new List<InteriorInteract>()
                {
                        new StandardInteriorInteract("paletopolicestandard1",new Vector3(-447.0432f, 6013.801f, 31.71637f), 130.9942f,"Interact With Front Desk")
                        {
                            CameraPosition = new Vector3(-444.018f, 6013.689f, 32.72884f), 
                            CameraDirection = new Vector3(-0.953248f, -0.2356599f, -0.1891631f), 
                            CameraRotation = new Rotator(-10.90395f, -7.390506E-06f, 103.8861f),
                        },
                },
                DisabledInteriorCoords = new Vector3(-444.89068603515625f, 6013.5869140625f, 30.7164f) 
            },
        });
    }
    private void GangDens()
    {
        PossibleInteriors.GangDenInteriors.AddRange(new List<GangDenInterior>()
        {
            //MadrazoMansion
            new GangDenInterior(-706,"Madrazo Ranch")
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(1391.485f, 1132.229f, 114.3336f),
                InteriorEgressHeading = 269.3185f,
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract("madrazoRanchExit1",new Vector3(1391.485f, 1132.229f, 114.3336f), 89.3185f,"Exit") ,
                    new StandardInteriorInteract("madrazoRanchStandard1",new Vector3(1397.711f, 1145.432f, 114.3336f), 98.00688f,"Interact") ,
                },
            },

            new GangDenInterior(245761,"LOST M.C. Clubhouse")
            {
                RequestIPLs = new List<string>() { "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo" },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("lostmxclubhouseStandard1",new Vector3(988.4098f, -96.65791f, 74.84534f), 43.43883f,"Interact") 
                    {
                        CameraPosition = new Vector3(987.3128f, -99.56942f, 76.22231f), 
                        CameraDirection = new Vector3(-0.09414972f, 0.9557025f, -0.27887f), 
                        CameraRotation = new Rotator(-16.19277f, -1.322452E-05f, 5.626261f)
                    },
                    new ToiletInteract("lostmxclubhouseUrinal1",new Vector3(981.5935f, -98.13846f, 74.97108f), 222.2108f,"Use Urinal") 
                    { 
                        IsStanding = true,
                        CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f), 
                        CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f), 
                        CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    },

                    new ToiletInteract("lostmxclubhouseToiletl1",new Vector3(979.7599f, -98.66194f, 74.845f), 132.1026f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f),
                        CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f),
                        CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    },
                    new ToiletInteract("lostmxclubhouseToiletl2",new Vector3(980.5167f, -99.33208f, 74.84503f), 135.7255f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f),
                        CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f),
                        CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    },
                },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(190770132,new Vector3(981.1505f, -103.2552f, 74.99358f)) 
                    { 
                        LockWhenClosed = true 
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("lostmcclubhouseRest1", new Vector3(989.5287f, -97.59096f, 74.84512f),220.7685f,"Sleep")
                    {
                        CameraPosition = new Vector3(987.0359f, -98.15501f, 75.47958f), 
                        CameraDirection = new Vector3(0.981001f, -0.0696246f, -0.1810784f), 
                        CameraRotation = new Rotator(-10.43258f, 6.510937E-07f, -94.05965f),
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
                    //new Vector3(986.4575f, -92.5943f, 74.84595f), 224.0455f, "Name", "Description"),  //lostmclockerspost
//new Vector3(976.914f, -103.794f, 74.84519f), 222.764f, "Name", "Description"),  //lostmcsafe1
//new Vector3(981.5935f, -98.13846f, 74.97108f), 222.2108f, "Name", "Description"),  //lostmcpiss1
                },
            },
        });

    }
    private void Residence()
    {

        PossibleInteriors.ResidenceInteriors.AddRange(new List<ResidenceInterior>()
        {
            //Apartments
            new ResidenceInterior(60162,"Motel") {//Motel Interior
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(151.817f, -1006.616f, -98.99998f),
                InteriorEgressHeading = 340.2548f,
                ClearPositions = new List<Vector3>() { new Vector3(154.1695f, -1002.792f, -99.00002f),new Vector3(153.8864f, -1006.77f, -98.99998f) },
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
            new ResidenceInterior(21250,"4 Integrity Way, Apt 28") {//Closest: 574422567,new Vector3(-16.84285f, -607.7756f, 100.3467f) //160f//FRONT DOOR OBJECT POS AND HEADING
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-16.20265f, -606.2855f, 100.2328f),
                InteriorEgressHeading = 4.009807f,
                ClearPositions = new List<Vector3>() { new Vector3(-19.51501f, -597.6929f, 94.02557f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("4intapt28Exit1",new Vector3(-16.20265f, -606.2855f, 100.2328f),4.009807f,"Exit") ,
                    new StandardInteriorInteract("4intapt28Standard1",new Vector3(-10.32665f, -591.5042f, 98.83028f),42.31462f,"Interact")
                    {
                        CameraPosition = new Vector3(-15.6437f, -588.6951f, 101.2196f),
                        CameraDirection = new Vector3(0.6824573f, -0.6314822f, -0.368079f),
                        CameraRotation = new Rotator(-21.59719f, -1.010063E-05f, -132.7783f),
                    },
                    new ToiletInteract("4intapt28Toilet1",new Vector3(-19.51501f, -597.6929f, 94.02557f), 243.7846f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-21.57672f, -595.4763f, 94.94376f),
                        CameraDirection = new Vector3(0.7354667f, -0.6025593f, -0.3098564f),
                        CameraRotation = new Rotator(-18.05057f, 1.795937E-06f, -129.3274f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("4intapt28Rest1", new Vector3(-12.56408f, -588.5016f, 94.02548f),249.3776f,"Sleep") 
                    { 
                        InteractDistance = 1.0f,
                        CameraPosition = new Vector3(-14.2581f, -592.8878f, 94.51609f), 
                        CameraDirection = new Vector3(0.6475294f, 0.7548846f, -0.1041872f), 
                        CameraRotation = new Rotator(-5.98034f, 5.579895E-06f, -40.62253f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("4intapt28Outfit1",new Vector3(-17.61602f, -587.5491f, 94.03635f), 156.3727f,"Change Outfit") 
                    {
                        CameraPosition = new Vector3(-19.12787f, -589.4743f, 94.79836f), 
                        CameraDirection = new Vector3(0.629463f, 0.7161801f, -0.301434f), 
                        CameraRotation = new Rotator(-17.54375f, 8.058801E-06f, -41.31279f)
                    }
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("4intapt28Inventory1",new Vector3(-24.43408f, -591.8068f, 98.83029f), 57.30276f,"Access Items") 
                    { 
                        CanAccessCash = false, 
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-19.51676f, -589.384f, 100.0748f),
                        CameraDirection = new Vector3(-0.9070988f, -0.3957494f, -0.1433672f), 
                        CameraRotation = new Rotator(-8.242742f, 1.725371E-06f, 113.5707f)
                    },
                    new InventoryInteract("4intapt28Inventory2",new Vector3(-20.73093f, -579.8246f, 98.83029f), 75.3243f,"Access Cash/Weapons") 
                    { 
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-18.77724f, -583.9211f, 100.1905f), 
                        CameraDirection = new Vector3(-0.3158911f, 0.9080421f, -0.2750861f), 
                        CameraRotation = new Rotator(-15.96715f, -6.21624E-06f, 19.18184f)
                    }
                }
            },
            new ResidenceInterior(47362,"4 Integrity Way, Apt 30") {//Closest: 574422567,new Vector3(-18.01365f, -582.781f, 90.22865f) //250.059f FRONT DOOR
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-19.61186f, -581.8444f, 90.11483f),
                InteriorEgressHeading = 91.7578f,
                ClearPositions = new List<Vector3>() { new Vector3(-28.41244f, -585.5016f, 83.90755f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("4intapt30Exit1",new Vector3(-19.61186f, -581.8444f, 90.11483f),91.7578f,"Exit") ,
                    new StandardInteriorInteract("4intapt30Standard1",new Vector3(-33.56378f, -576.7966f, 88.71226f),276.5922f,"Interact")
                    {
                        CameraPosition = new Vector3(-36.54444f, -579.4444f, 90.46429f),
                        CameraDirection = new Vector3(0.7369682f, 0.5456903f, -0.3988734f),
                        CameraRotation = new Rotator(-23.50777f, -6.517313E-06f, -53.48179f),
                    },
                    new ToiletInteract("4intapt30Toilet1",new Vector3(-28.41244f, -585.5016f, 83.90755f), 312.7336f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-31.14033f, -585.444f, 84.48948f), 
                        CameraDirection = new Vector3(0.9862954f, -0.03383142f, -0.1614832f), 
                        CameraRotation = new Rotator(-9.292995f, -2.838701E-06f, -91.96456f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("4intapt30Outfit1",new Vector3(-37.59638f, -583.7009f, 83.91832f), 248.8541f,"Change Outfit")//new OutfitInteract("4intapt30Outfit1",new Vector3(-17.02242f, -570.6223f, 83.91838f), 248.39576f,"Change Outfit"),
                    {
                         CameraPosition = new Vector3(-33.62995f, -585.479f, 84.64574f), 
                        CameraDirection = new Vector3(-0.8840417f, 0.4389256f, -0.1606693f), 
                        CameraRotation = new Rotator(-9.245749f, -3.027541E-06f, 63.59566f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("4intapt30Rest1", new Vector3(-37.52463f, -578.3497f, 83.90746f), 334.0199f,"Sleep")//new RestInteract("4intapt30Rest1", new Vector3(-37.85818f, -580.4131f, 83.90745f), 340.86966f,"Sleep")//still not working
                    {
                        CameraPosition = new Vector3(-33.62407f, -580.7298f, 85.0428f), 
                        CameraDirection = new Vector3(-0.4075595f, 0.8970569f, -0.170834f),
                        CameraRotation = new Rotator(-9.836313f, -8.231859E-06f, 24.4337f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("4intapt30Inventory1",new Vector3(-33.78065f, -590.0823f, 88.71225f), 161.284f,"Access Items") 
                    { 
                        CanAccessCash = false, 
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-38.09245f, -586.113f, 89.35939f), 
                        CameraDirection = new Vector3(0.6869188f, -0.7261322f, -0.02957254f), 
                        CameraRotation = new Rotator(-1.694629f, 6.406103E-06f, -136.5896f)
                    },
                    new InventoryInteract("4intapt30Inventory2",new Vector3(-45.59173f, -586.4761f, 88.71217f), 157.6343f,"Access Cash/Weapons") 
                    { 
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-41.74444f, -584.4836f, 89.44774f),
                        CameraDirection = new Vector3(-0.9262016f, -0.3567787f, -0.1218992f),
                        CameraRotation = new Rotator(-7.001721f, 2.150471E-06f, 111.067f)
                    }
                }
            },
            new ResidenceInterior(76290,"Dell Perro Heights, Apt 4") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-1458.708f, -522.3859f, 69.55659f),
                InteriorEgressHeading = 151.7898f,
                ClearPositions = new List<Vector3>() { new Vector3(-1460.642f, -530.532f, 63.34938f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("dpheightApt4Exit1",new Vector3(-1458.061f, -520.9923f, 69.5566f), 314.0058f,"Exit") ,
                    new StandardInteriorInteract("dpheightApt4Std1",new Vector3(-1469.23f, -530.2661f, 68.15405f),32.30866f,"Interact")
                    {
                        CameraPosition = new Vector3(-1468.288f, -537.0152f, 70.50282f),
                        CameraDirection = new Vector3(-0.2367817f, 0.938622f, -0.2508448f),
                        CameraRotation = new Rotator(-14.52751f, 6.614796E-07f, 14.15833f),
                    },
                    new ToiletInteract("dpheightApt4Toilet1",new Vector3(-1460.642f, -530.532f, 63.34938f), 2.485797f,"Use Toilet") 
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-1462.01f, -533.6485f, 64.09763f), 
                        CameraDirection = new Vector3(0.3879803f, 0.9071884f, -0.1627286f), 
                        CameraRotation = new Rotator(-9.365308f, 6.489807E-06f, -23.15511f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("dpheightApt4Outfit1",new Vector3(-1467.755f, -537.3607f, 63.36013f), 302.0378f,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-1464.091f, -534.6213f, 64.05119f), 
                        CameraDirection = new Vector3(-0.8109046f, -0.5583638f, -0.17511f), 
                        CameraRotation = new Rotator(-10.08506f, 4.335862E-06f, 124.5501f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("dpheightApt4Inventory1",new Vector3(-1459.885f, -537.7822f, 68.15405f), 216.2637f,"Access Items") 
                    { 
                        CanAccessCash = false, 
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-1462.965f, -538.519f, 69.30391f), 
                        CameraDirection = new Vector3(0.9774104f, 0.1281847f, -0.1680408f),
                        CameraRotation = new Rotator(-9.673925f, 7.470021E-06f, -82.52846f)
                    },
                    new InventoryInteract("dpheightApt4Inventory2",new Vector3(-1469.843f, -545.3965f, 68.15405f), 215.8857f,"Access Cash/Weapons") 
                    { 
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-1468.554f, -541.213f, 68.90868f),
                        CameraDirection = new Vector3(-0.2363213f, -0.9591809f, -0.1553198f),
                        CameraRotation = new Rotator(-8.935347f, 9.939014E-06f, 166.1592f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("dpheightApt4Rest1",new Vector3(-1471.282f, -534.1699f, 63.34928f), 31.54102f,"Sleep")
                    {
                        CameraPosition = new Vector3(-1470.646f, -528.1004f, 64.8106f), 
                        CameraDirection = new Vector3(-0.3486035f, -0.8848822f, -0.3089646f), 
                        CameraRotation = new Rotator(-17.99684f, 1.077233E-05f, 158.4978f)
                    },
                },
            },
            new ResidenceInterior(-673,"Dell Perro Heights, Apt 7") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-1458.221f, -523.283f, 56.92899f),
                InteriorEgressHeading = 144.4565f,
                ClearPositions = new List<Vector3>() { new Vector3(-1460.75f, -530.77f, 50.73058f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("dpheightApt7Exit1",new Vector3(-1457.777f, -520.5983f, 56.92899f), 306.839f,"Exit") ,
                    new StandardInteriorInteract("dpheightApt7Std1",new Vector3(-1469.766f, -529.8615f, 55.52639f),23.87169f,"Interact")
                    {
                        CameraPosition = new Vector3(-1469.822f, -535.2637f, 57.56692f),
                        CameraDirection = new Vector3(-0.08299411f, 0.9490341f, -0.3040497f),
                        CameraRotation = new Rotator(-17.701f, 1.960445E-05f, 4.997866f),
                    },
                    new ToiletInteract("dpheightApt7Toilet1",new Vector3(-1460.75f, -530.77f, 50.73058f), 348.9325f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-1462.437f, -533.1895f, 51.77926f), 
                        CameraDirection = new Vector3(0.5533304f, 0.7600167f, -0.3408814f), 
                        CameraRotation = new Rotator(-19.93059f, 1.0898E-05f, -36.0565f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("dpheightApt7Outfit1",new Vector3(-1467.405f, -537.1845f, 50.7325f), 303.9389f,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-1463.99f, -534.7911f, 52.06883f), 
                        CameraDirection = new Vector3(-0.807931f, -0.5270224f, -0.2636187f),
                        CameraRotation = new Rotator(-15.28489f, 2.301212E-05f, 123.1169f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("dpheightApt7Inventory1",new Vector3(-1459.927f, -537.7508f, 55.5264f), 208.4931f,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-1464.889f, -537.3826f, 57.01154f), 
                        CameraDirection = new Vector3(0.9680428f, -0.1724893f, -0.1820456f), 
                        CameraRotation = new Rotator(-10.48893f, 9.768179E-07f, -100.1031f)
                    },
                    new InventoryInteract("dpheightApt7Inventory2",new Vector3(-1469.73f, -545.197f, 55.5264f), 211.0862f,"Access Cash/Weapons")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-1468.045f, -541.0137f, 56.3699f), 
                        CameraDirection = new Vector3(-0.408142f, -0.9044865f, -0.1237914f), 
                        CameraRotation = new Rotator(-7.110966f, 3.871762E-06f, 155.7131f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("dpheightApt7Rest1",new Vector3(-1471.296f, -534.1364f, 50.72165f), 33.67384f,"Sleep")
                    {
                        CameraPosition = new Vector3(-1467.801f, -532.2233f, 51.9097f), 
                        CameraDirection = new Vector3(-0.9257399f, 0.2948876f, -0.2367424f), 
                        CameraRotation = new Rotator(-13.69435f, -1.098443E-05f, 72.33111f)
                    },
                },



                //RESTnew Vector3(-1471.296f, -534.1364f, 50.72165f), startingHeading: 33.67384f
            },
            new ResidenceInterior(61186, "Eclipse Towers, Apt 3")//SP from PB
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-779.4249f, 339.4756f, 207.6208f),
                InteriorEgressHeading = 113.7695f,
                ClearPositions = new List<Vector3>() { new Vector3(-785.222f, 333.3687f, 201.4136f) },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract()
                    {
                        Name = "ecliApt3Rest1",
                        Position = new Vector3(-796.193f, 336.6858f, 201.4136f),
                        Heading = 4.0855f,
                        InteractDistance = 1f,
                        CameraPosition = new Vector3(-792.7014f, 336.5643f, 203.0321f),
                        CameraDirection = new Vector3(-0.7737222f, 0.5073887f, -0.3793557f),
                        CameraRotation = new Rotator(-22.29378f, -2.122321E-05f, 56.74408f),
                        ButtonPromptText = "Sleep",
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        Name = "ecliApt3Inventory1",
                        Position = new Vector3(-787.1135f, 326.8101f, 206.2184f),
                        Heading = 233.2313f,
                        CameraPosition = new Vector3(-791.9602f, 329.7868f, 207.9795f),
                        CameraDirection = new Vector3(0.8108636f, -0.5609891f, -0.1667076f),
                        CameraRotation = new Rotator(-9.596444f, 1.515308E-05f, -124.6771f),
                        ButtonPromptText = "Access Items",
                    },
                    new InventoryInteract()
                    {
                        CanAccessItems = false,
                        Name = "ecliApt3Inventory2",
                        Position = new Vector3(-801.5412f, 326.3719f, 206.2184f),
                        Heading = 180.9895f,
                        CameraPosition = new Vector3(-799.5909f, 328.0801f, 207.6206f),
                        CameraDirection = new Vector3(-0.5492002f, -0.7414261f, -0.3855729f),
                        CameraRotation = new Rotator(-22.67931f, 2.590902E-05f, 143.4714f),
                        ButtonPromptText = "Access Cash/Weapons",
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract()
                    {
                        Name = "ecliApt3Outfit1",
                        Position = new Vector3(-795.3344f, 331.6995f, 201.4244f),
                        Heading = 270.0498f,
                        CameraPosition = new Vector3(-791.5106f, 331.8639f, 202.628f),
                        CameraDirection = new Vector3(-0.9758344f, 0.01968939f, -0.2176224f),
                        CameraRotation = new Rotator(-12.56942f, -1.448785E-06f, 88.8441f),
                        ButtonPromptText = "Change Outfit",
                    },
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract()
                    {
                        Name = "ecliApt3Exit1",
                        Position = new Vector3(-777.7397f, 340.0569f, 207.6208f),
                        Heading = 271.6255f,
                        ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract()
                    {
                        Name = "ecliApt3Std1",
                        Position = new Vector3(-792.1093f, 338.2278f, 206.2184f),
                        Heading = 11.25924f,
                        CameraPosition = new Vector3(-795.7065f, 332.019f, 208.7504f),
                        CameraDirection = new Vector3(0.3429281f, 0.8894085f, -0.3022463f),
                        CameraRotation = new Rotator(-17.59257f, -4.478318E-07f, -21.08508f),
                    },
                    new ToiletInteract()
                    {
                        Name = "ecliApt3Toilet1",
                        Position = new Vector3(-785.222f, 333.3687f, 201.4136f),
                        Heading = 176.2657f,
                        CameraPosition = new Vector3(-787.2263f, 332.5878f, 202.7737f),
                        CameraDirection = new Vector3(0.7613108f, 0.3867464f, -0.5204163f),
                        CameraRotation = new Rotator(-31.36018f, 6.99886E-06f, -63.0694f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                },
            },
            new ResidenceInterior(-674, "Eclipse Towers, Apt 3")//MP from PB
            {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-779.4249f, 339.4756f, 207.6208f),
                InteriorEgressHeading = 113.7695f,
                ClearPositions = new List<Vector3>() { new Vector3(-785.222f, 333.3687f, 201.4136f) },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract()
                    {
                        Name = "ecliApt3Rest1",
                        Position = new Vector3(-796.193f, 336.6858f, 201.4136f),
                        Heading = 4.0855f,
                        InteractDistance = 1.0f,
                        CameraPosition = new Vector3(-792.7014f, 336.5643f, 203.0321f),
                        CameraDirection = new Vector3(-0.7737222f, 0.5073887f, -0.3793557f),
                        CameraRotation = new Rotator(-22.29378f, -2.122321E-05f, 56.74408f),
                        ButtonPromptText = "Sleep",
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        Name = "ecliApt3Inventory1",
                        Position = new Vector3(-787.1135f, 326.8101f, 206.2184f),
                        Heading = 233.2313f,
                        CameraPosition = new Vector3(-791.9602f, 329.7868f, 207.9795f),
                        CameraDirection = new Vector3(0.8108636f, -0.5609891f, -0.1667076f),
                        CameraRotation = new Rotator(-9.596444f, 1.515308E-05f, -124.6771f),
                        ButtonPromptText = "Access Items",
                    },
                    new InventoryInteract()
                    {
                        CanAccessItems = false,
                        Name = "ecliApt3Inventory2",
                        Position = new Vector3(-784.2221f, 330.7322f, 207.6296f),
                        Heading = 97.70668f,
                        CameraPosition = new Vector3(-781.3085f, 328.6723f, 209.324f),
                        CameraDirection = new Vector3(-0.7612467f, 0.5483233f, -0.3461864f),
                        CameraRotation = new Rotator(-20.25423f, 1.274064E-05f, 54.23493f),
                        ButtonPromptText = "Access Cash/Weapons",
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract()
                    {
                        Name = "ecliApt3Outfit1",
                        Position = new Vector3(-795.3344f, 331.6995f, 201.4244f),
                        Heading = 270.0498f,
                        CameraPosition = new Vector3(-791.5106f, 331.8639f, 202.628f),
                        CameraDirection = new Vector3(-0.9758344f, 0.01968939f, -0.2176224f),
                        CameraRotation = new Rotator(-12.56942f, -1.448785E-06f, 88.8441f),
                        ButtonPromptText = "Change Outfit",
                    },
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract()
                    {
                        Name = "ecliApt3Exit1",
                        Position = new Vector3(-777.7397f, 340.0569f, 207.6208f),
                        Heading = 271.6255f,
                        ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract()
                    {
                        Name = "ecliApt3Std1",
                        Position = new Vector3(-792.1093f, 338.2278f, 206.2184f),
                        Heading = 11.25924f,
                        CameraPosition = new Vector3(-795.7065f, 332.019f, 208.7504f),
                        CameraDirection = new Vector3(0.3429281f, 0.8894085f, -0.3022463f),
                        CameraRotation = new Rotator(-17.59257f, -4.478318E-07f, -21.08508f),
                    },
                    new ToiletInteract()
                    {
                        Name = "ecliApt3Toilet1",
                        Position = new Vector3(-785.222f, 333.3687f, 201.4136f),
                        Heading = 176.2657f,
                        CameraPosition = new Vector3(-787.2263f, 332.5878f, 202.7737f),
                        CameraDirection = new Vector3(0.7613108f, 0.3867464f, -0.5204163f),
                        CameraRotation = new Rotator(-31.36018f, 6.99886E-06f, -63.0694f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                },
            },
            new ResidenceInterior(-675,"Richard Majestic, Apt 2") {
                IsTeleportEntry = true,
                ClearPositions = new List<Vector3>() { new Vector3(-918.4091f, -376.0463f, 103.2419f) },
                InteriorEgressPosition = new Vector3(-915.4658f, -367.6183f, 109.4403f),
                InteriorEgressHeading = 142.9073f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("richMaj2Exit1",new Vector3(-914.0711f, -366.1032f, 109.4403f), 298.5614f,"Exit") ,
                    new StandardInteriorInteract("richMaj2Std1",new Vector3(-926.6827f, -373.9506f, 108.0377f),29.87866f,"Interact")
                    {
                        CameraPosition = new Vector3(-926.1729f, -380.4109f, 110.0456f),
                        CameraDirection = new Vector3(-0.1600053f, 0.9530545f, -0.2570709f),
                        CameraRotation = new Rotator(-14.89633f, -2.208661E-06f, 9.530333f),
                    },
                    new ToiletInteract("richMaj2Toilet1",new Vector3(-918.4091f, -376.0463f, 103.2419f), 344.8122f ,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-920.495f, -377.7718f, 103.935f), 
                        CameraDirection = new Vector3(0.7231579f, 0.6743343f, -0.1493844f), 
                        CameraRotation = new Rotator(-8.591255f, -6.475967E-06f, -47.0009f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("richMaj2Outfit1",new Vector3(-925.7103f, -381.4328f, 103.2438f), 297.0808f,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-922.6008f, -379.8598f, 104.382f), 
                        CameraDirection = new Vector3(-0.8919451f, -0.4158678f, -0.1774484f), 
                        CameraRotation = new Rotator(-10.22117f, 6.072789E-06f, 114.9972f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("richMaj2Inventory1",new Vector3(-918.5358f, -383.0027f, 108.0377f), 206.728f ,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-923.335f, -382.5368f, 109.3175f), 
                        CameraDirection = new Vector3(0.9724852f, -0.1815508f, -0.1459859f), 
                        CameraRotation = new Rotator(-8.394377f, -4.638729E-06f, -100.5747f)
                    },
                    new InventoryInteract("richMaj2Inventory2",new Vector3(-929.5338f, -389.2018f, 108.0377f), 210.6519f,"Access Cash/Weapons")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-927.5273f, -385.4138f, 109.2147f), 
                        CameraDirection = new Vector3(-0.5264284f, -0.8286842f, -0.1901465f), 
                        CameraRotation = new Rotator(-10.96133f, -3.043738E-06f, 147.5739f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("richMaj2Rest1",new Vector3(-929.532f, -377.7678f, 103.2329f), 27.06194f,"Sleep")
                    {
                        CameraPosition = new Vector3(-925.649f, -377.0893f, 104.6415f), 
                        CameraDirection = new Vector3(-0.8747561f, 0.3998851f, -0.2736671f),
                        CameraRotation = new Rotator(-15.8826f, 4.438303E-06f, 65.43301f)
                    },
                },
            },
            new ResidenceInterior(146689,"Tinsel Towers, Apt 42") {
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-601.3342f, 63.16429f, 108.027f),
                InteriorEgressHeading = 102.5944f,
                ClearPositions = new List<Vector3>() { new Vector3(-608.015f, 58.23079f, 101.8285f) },
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("tinselApt42Exit1",new Vector3(-600.2047f, 64.93344f, 108.027f), 271.8336f ,"Exit") ,
                    new StandardInteriorInteract("tinselApt42Std1",new Vector3(-614.5223f, 63.66457f, 106.6245f),350.1266f,"Interact")
                    {
                        CameraPosition = new Vector3(-617.3007f, 59.07885f, 109.0985f),
                        CameraDirection = new Vector3(0.3802197f, 0.8753764f, -0.2985786f),
                        CameraRotation = new Rotator(-17.37225f, 1.744431E-05f, -23.47771f),
                    },
                    new ToiletInteract("tinselApt42Toilet1",new Vector3(-608.015f, 58.23079f, 101.8285f), 313.6327f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-610.6426f, 57.28061f, 102.6193f), 
                        CameraDirection = new Vector3(0.932392f, 0.282665f, -0.2252679f),
                        CameraRotation = new Rotator(-13.01863f, 0f, -73.13474f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("tinselApt42Outfit1",new Vector3(-616.9544f, 56.73778f, 101.8305f), 270.0324f,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-613.0131f, 56.54685f, 102.9228f), 
                        CameraDirection = new Vector3(-0.9800308f, 0.05486051f, -0.1911279f),
                        CameraRotation = new Rotator(-11.01861f, -3.805412E-07f, 86.79602f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("tinselApt42Inventory1",new Vector3(-611.4963f, 51.6665f, 106.6245f), 183.2867f,"Access Items")
                    {
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        CameraPosition = new Vector3(-614.6435f, 54.66705f, 107.5006f), 
                        CameraDirection = new Vector3(0.6689919f, -0.7388232f, -0.0811801f), 
                        CameraRotation = new Rotator(-4.656401f, 7.066957E-06f, -137.8397f)
                    },
                    new InventoryInteract("tinselApt42Inventory2",new Vector3(-623.7113f, 51.4254f, 106.6245f), 180.2425f ,"Access Cash/Weapons")
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(-620.9768f, 53.88909f, 107.36f), 
                        CameraDirection = new Vector3(-0.7933871f, -0.6019743f, -0.09035382f), 
                        CameraRotation = new Rotator(-5.183963f, 4.07208E-06f, 127.189f)
                    }
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("tinselApt42Rest1",new Vector3(-618.5925f, 61.77842f, 101.8197f), 2.551883f,"Sleep")
                    {
                        CameraPosition = new Vector3(-613.8111f, 60.83089f, 102.9666f), 
                        CameraDirection = new Vector3(-0.7403303f, 0.6300179f, -0.2344961f), 
                        CameraRotation = new Rotator(-13.56192f, 2.634786E-06f, 49.60236f)
                    },
                },
            },
            new ResidenceInterior(24578,"7611 Goma St")
            {
                RequestIPLs = new List<string>() {  },
                RemoveIPLs = new List<string>() { "vb_30_crimetape" },
                Doors = new List<InteriorDoor>()
                {
                    new InteriorDoor(-607040053, new Vector3(-1149.709f, -1521.088f, 10.78267f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },
                    new InteriorDoor(3687927243,new Vector3(-1149.709f, -1521.088f, 10.78267f)) { LockWhenClosed = true, NeedsDefaultUnlock = true },
                },
                //DisabledInteriorCoords = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f),
                InteriorSets = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A","swap_mrJam_A" },
                InteractPoints = new List<InteriorInteract>(){
                    new StandardInteriorInteract("gomaStd1",new Vector3(-1156.618f, -1517.657f, 10.63273f), 23.34965f,"Interact")
                    {
                        CameraPosition = new Vector3(-1156.618f, -1520.649f, 11.4059f), 
                        CameraDirection = new Vector3(0.005553481f, 0.9837997f, -0.1791852f), 
                        CameraRotation = new Rotator(-10.3223f, -1.706485E-05f, -0.3234273f)
                    },
                    new ToiletInteract("gomaToilet1",new Vector3(-1148.619f, -1519.637f, 10.63273f), 133.3349f ,"Use Toilet")
                    {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(-1148.25f, -1517.818f, 11.15233f), 
                        CameraDirection = new Vector3(-0.3959475f, -0.8638847f, -0.3113339f), 
                        CameraRotation = new Rotator(-18.13964f, 8.535035E-06f, 155.3764f)
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("gomaOutfit1",new Vector3(-1153.072f, -1516.603f, 10.63273f), 213.7637f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(-1151.871f, -1519.412f, 11.3602f), 
                        CameraDirection = new Vector3(-0.4125309f, 0.8881764f, -0.2023879f), 
                        CameraRotation = new Rotator(-11.67663f, -2.179539E-05f, 24.91343f)
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("gomaInventory1",new Vector3(-1154.125f, -1520.482f, 10.63273f), 304.051f ,"Access Items")
                    {
                        CameraPosition = new Vector3(-1153.794f, -1523.291f, 11.39256f), 
                        CameraDirection = new Vector3(0.09049927f, 0.982206f, -0.1645641f), 
                        CameraRotation = new Rotator(-9.471912f, -1.628362E-05f, -5.2643f)
                    },
                    new InventoryInteract("gomaInventory2",new Vector3(-1155.299f, -1523.785f, 10.63231f), 211.5514f,"Access Items")
                    {
                        CameraPosition = new Vector3(-1158.417f, -1523.426f, 11.5467f), 
                        CameraDirection = new Vector3(0.9296608f, -0.2112845f, -0.3018107f), 
                        CameraRotation = new Rotator(-17.56639f, -1.522408E-05f, -102.8041f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("gomaRest1",new Vector3(-1150.127f, -1513.181f, 10.63273f), 307.6416f,"Sleep")
                    {
                        CameraPosition = new Vector3(-1149.131f, -1515.233f, 11.20237f),
                        CameraDirection = new Vector3(0.1829105f, 0.9701641f, -0.1591396f),
                        CameraRotation = new Rotator(-9.15696f, -1.675539E-05f, -10.67697f)
                    },
                },
            },

            //SP Still?
            new ResidenceInterior(55042,"Nice Medium Apartment") {//needs the blinds closed
                IsTeleportEntry = true,
                NeedsActivation = true,
                NeedsSetDisabled = true,
                InternalInteriorCoordinates = new Vector3(347.2686f,-999.2955f,-99.19622f),
                RequestIPLs = new List<string>() { "Medium End Apartment" },
                InteriorEgressPosition = new Vector3(346.9781f, -1001.959f, -99.19621f),//new Vector3(288.3021f, -998.6495f, -92.79259f),
                InteriorEgressHeading = 1.692819f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("mediumaptExit1",new Vector3(346.538f, -1002.103f, -99.19621f), 175.579f,"Exit") ,
                    new StandardInteriorInteract("mediumaptStandard1",new Vector3(339.6717f, -1000.131f, -99.19621f), 164.4039f ,"Interact")
                    {
                        CameraPosition = new Vector3(341.8449f, -997.2225f, -98.27441f),
                        CameraDirection = new Vector3(-0.5587474f, -0.8046044f, -0.2010301f), 
                        CameraRotation = new Rotator(-11.59721f, 7.844101E-06f, 145.2224f),

                    },
                    new ToiletInteract("mediumaptToilet1",new Vector3(347.1078f, -993.6047f, -99.19623f), 26.89262f,"Use Toilet") {
                        UseNavmesh = false,
                        CameraPosition = new Vector3(347.9928f, -995.7236f, -98.23453f), 
                        CameraDirection = new Vector3(-0.301568f, 0.8751476f, -0.3783827f), 
                        CameraRotation = new Rotator(-22.23354f, -1.567997E-05f, 19.01349f),
                    },
                    new SinkInteract("mediumaptSink1",new Vector3(347.2266f, -994.1828f, -99.19623f), 91.86758f,"Use Sink")
                    {
                        CameraPosition = new Vector3(347.9928f, -995.7236f, -98.23453f), 
                        CameraDirection = new Vector3(-0.301568f, 0.8751476f, -0.3783827f), 
                        CameraRotation = new Rotator(-22.23354f, -1.567997E-05f, 19.01349f),
                        UseNavmesh = false,
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract("mediumaptInventory1",new Vector3(343.5486f, -1001.16f, -99.19621f),264.7867f ,"Open Fridge")//fridge
                    {
                        AllowedItemTypes = new List<ItemType>() { ItemType.Drinks },
                        CanAccessCash = false,
                        CanAccessWeapons = false,
                        Title = "Fridge",
                        Description = "Access drink items",
                        CameraPosition = new Vector3(342.0506f, -998.5658f, -98.08253f), 
                        CameraDirection = new Vector3(0.5944998f, -0.7588475f, -0.2659332f), 
                        CameraRotation = new Rotator(-15.42241f, 4.428326E-06f, -141.924f),
                    },
                    new InventoryInteract("mediumaptInventory2",new Vector3(342.2074f, -1003.222f, -99.19622f), 176.1618f,"Open Pantry")//Pantry
                    {
                        AllowedItemTypes = new List<ItemType>() { ItemType.Food, ItemType.Meals },
                        CanAccessCash = false,
                        CanAccessWeapons = false,                        
                        Title = "Pantry",
                        Description = "Access food items",
                        CameraPosition = new Vector3(343.9472f, -1000.317f, -98.27441f), 
                        CameraDirection = new Vector3(-0.5217595f, -0.8294323f, -0.1995222f), 
                        CameraRotation = new Rotator(-11.50902f, 6.534693E-06f, 147.8278f),
                    },
                    new InventoryInteract("mediumaptInventory3",new Vector3(345.0375f, -996.074f, -99.19621f), 270.716f,"Access Items")//Shelving behind couch
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        DisallowedItemTypes = new List<ItemType>() { ItemType.Drinks,ItemType.Food, ItemType.Meals },
                        CameraPosition = new Vector3(343.0483f, -998.4931f, -98.27441f), 
                        CameraDirection = new Vector3(0.6651655f, 0.7349377f, -0.13199f), 
                        CameraRotation = new Rotator(-7.584604f, 6.890473E-06f, -42.1471f)
                    },
                    new InventoryInteract("mediumaptInventory4",new Vector3(351.2565f, -999.094f, -99.19627f), 179.264f,"Access Weapons/Cash")//dresser in bedroom
                    {
                        CanAccessItems = false,
                        CameraPosition = new Vector3(350.5187f, -996.0263f, -98.22089f), 
                        CameraDirection = new Vector3(0.2043052f, -0.9459162f, -0.2519961f), 
                        CameraRotation = new Rotator(-14.59566f, -1.102806E-06f, -167.8121f),
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract("mediumaptChange1",new Vector3(350.4886f, -993.6788f, -99.19617f), 174.7643f ,"Change Outfit")
                    {
                        CameraPosition = new Vector3(349.9235f, -996.5236f, -98.30812f), 
                        CameraDirection = new Vector3(0.2095511f, 0.922645f, -0.3237507f),
                        CameraRotation = new Rotator(-18.8899f, 9.023732E-07f, -12.79594f)
                    },
                },
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract("mediumaptRest1", new Vector3(349.3769f, -998.2667f, -99.19629f), 357.6585f,"Sleep")
                    {
                        CameraPosition = new Vector3(352.3535f, -997.7198f, -98.33637f), 
                        CameraDirection = new Vector3(-0.8584386f, 0.397912f, -0.3236498f), 
                        CameraRotation = new Rotator(-18.88379f, -9.474573E-06f, 65.13087f)
                    },
                },
            },

            //Houses
            new ResidenceInterior(206081,"2044 North Conker Avenue")
            {
                InternalInteriorCoordinates = new Vector3(340.9412f, 437.1798f, 149.3925f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(341.8584f, 437.655f, 149.3941f),
                InteriorEgressHeading = 116.4628f,
                RestInteracts = new List<RestInteract>()
                {
                        new RestInteract()
                        {
                            Name = "2044NorthConkRest1",
                            Position = new Vector3(333.0005f, 424.0142f, 145.5967f),
                            Heading = 122.1341f,
                            InteractDistance = 1f,
                            CameraPosition = new Vector3(332.1866f, 427.3331f, 147.0565f),
                            CameraDirection = new Vector3( - 0.354439f, -0.8480681f, -0.3938954f),
                            CameraRotation = new Rotator( - 23.1971f, 4.830111E-05f, 157.3181f),
                            ButtonPromptText = "Sleep",
                            UseNavmesh = false,
                        },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() { ItemType.Food, ItemType.Meals },
                        Name = "2044NorthConkInventory1",
                        Position = new Vector3(342.6711f, 430.5975f, 149.3807f),
                        Heading = 294.7958f,
                        CameraPosition = new Vector3(340.1125f, 431.4519f, 150.6194f),
                        CameraDirection = new Vector3(0.895169f, -0.3380183f, -0.2905444f),
                        CameraRotation = new Rotator( - 16.89055f, 5.487425E-05f, -110.6867f),
                        ButtonPromptText = "Open Pantry",
                        Title = "Pantry",
                        Description = "Access food items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract()
                    {
                        CanAccessItems = false,
                        CanAccessCash = false,
                        Name = "2044NorthConkInventory2",
                        Position = new Vector3(336.2231f, 437.8118f, 141.7708f),
                        Heading = 30.97907f,
                        CameraPosition = new Vector3(334.6476f, 435.1272f, 143.2005f),
                        CameraDirection = new Vector3(0.509838f, 0.8177249f, -0.2671912f),
                        CameraRotation = new Rotator( - 15.49719f, 5.758902E-06f, -31.94286f),
                        ButtonPromptText = "Access Weapons",
                        UseNavmesh = false,
                    },
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() { ItemType.Drinks },
                        Name = "2044NorthConkInventory3",
                        Position = new Vector3(341.0717f, 433.2059f, 149.3806f),//new Vector3(341.269f, 433.4402f, 149.3806f),
                        Heading = 297.7491f,
                        AutoCamera = true,
                        Title = "Fridge",
                        Description = "Access drink items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        DisallowedItemTypes = new List<ItemType>() { ItemType.Drinks,ItemType.Food, ItemType.Meals },
                        Name = "2044NorthConkInventory4",
                        Position = new Vector3(337.6338f, 436.5455f, 141.7708f),
                        Heading = 294.9129f,
                        CameraPosition = new Vector3(336.5846f, 432.6438f, 143.1402f), 
                        CameraDirection = new Vector3(0.3455506f, 0.8929839f, -0.2884001f), 
                        CameraRotation = new Rotator(-16.7622f, -3.566641E-06f, -21.15453f),
                        ButtonPromptText = "Access Items/Cash",
                        UseNavmesh = false,
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract() {
                        Name = "2044NorthConkOutfit1",
                        Position = new Vector3(334.3427f, 428.6346f, 145.5709f),
                        Heading = 125.7571f,
                        CameraPosition = new Vector3(332.0534f, 427.342f, 146.4523f),
                        CameraDirection = new Vector3(0.8700684f, 0.4245014f, -0.2505585f),
                        CameraRotation = new Rotator( - 14.51057f, 1.411048E-05f, -63.99251f),
                        ButtonPromptText = "Change Outfit",
                        UseNavmesh = false,
                    },
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract()
                    {
                        Name = "2044NorthConkExit1",
                        Position = new Vector3(341.8584f, 437.655f, 149.3941f),
                        Heading = 295.7379f,
                        ButtonPromptText = "Exit",
                        UseNavmesh = false,
                    },
                    new StandardInteriorInteract()
                    {
                        Name = "2044NorthConkStd1",
                        Position = new Vector3(331.6051f, 429.8193f, 149.1707f),
                        Heading = 290.6066f,
                        CameraPosition = new Vector3(333.559f, 432.0945f, 150.4514f),
                        CameraDirection = new Vector3(-0.6512895f, -0.7182505f, -0.2448228f),
                        CameraRotation = new Rotator(-14.17136f, -1.320857E-06f, 137.7991f),
                        UseNavmesh = false,
                    },
                    new ToiletInteract()
                    {
                        Name = "2044NorthConkToilet1",
                        Position = new Vector3(341.2922f, 427.8438f, 145.5709f),
                        Heading = 206.6838f,
                        CameraPosition = new Vector3(338.8505f, 429.0959f, 147.0373f),
                        CameraDirection = new Vector3(0.8618982f,-0.3906567f,-0.3232939f),
                        CameraRotation = new Rotator(-18.86224f,-4.511121E-07f,-114.3825f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "2044NorthConkSink1",
                        Position = new Vector3(342.154f, 429.6871f, 145.5831f),
                        Heading = 296.63f,
                        CameraPosition = new Vector3(339.2189f, 430.3112f, 146.1448f),
                        CameraDirection = new Vector3(0.9499696f, -0.2650702f, -0.1652133f),
                        CameraRotation = new Rotator(-9.509624f, -3.895513E-06f, -105.5907f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "2044NorthConkSink1",
                        Position = new Vector3(342.656f, 428.3867f, 145.5709f),
                        Heading = 296.7733f,
                        CameraPosition = new Vector3(339.2189f, 430.3112f, 146.1448f), 
                        CameraDirection = new Vector3(0.9499696f, -0.2650702f, -0.1652133f),
                        CameraRotation = new Rotator(-9.509624f, -3.895513E-06f, -105.5907f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                },
            },
            new ResidenceInterior(206337, "2045 North Conker Avenue")
            {
                InternalInteriorCoordinates = new Vector3(373.023f, 416.105f, 145.7006f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(373.59f, 423.5691f, 145.9079f),
                InteriorEgressHeading = 166.8477f,
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract() {
                        StartAnimations = new List < AnimationBundle > () {},
                        LoopAnimations = new List < AnimationBundle > () {},
                        EndAnimations = new List < AnimationBundle > () {},
                        Name = "2045NorthConkRest1",
                        Position = new Vector3(376.9633f, 408.1437f, 142.1256f),
                        Heading = 159.9418f,
                        InteractDistance = 1f,
                        CameraPosition = new Vector3(374.0139f, 409.4142f, 143.3061f),
                        CameraDirection = new Vector3(0.4850231f, -0.8303007f, -0.2745057f),
                        CameraRotation = new Rotator( - 15.93256f, -1.331822E-05f, -149.7085f),
                        ButtonPromptText = "Sleep",
                        UseNavmesh = false,
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Name = "2045NorthConkInventory1",
                        Position = new Vector3(378.208f, 419.2106f, 145.9001f),
                        Heading = 342.1691f,
                        CameraPosition = new Vector3(375.507f, 417.9591f, 147.0778f),
                        CameraDirection = new Vector3(0.879289f, 0.4073337f, -0.24684f),
                        CameraRotation = new Rotator( - 14.2906f, 2.42285E-05f, -65.14391f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract()
                    {
                        CanAccessItems = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Name = "2045NorthConkInventory2",
                        Position = new Vector3(378.9872f, 429.8485f, 138.3001f),
                        Heading = 256.9881f,
                        CameraPosition = new Vector3(377.7497f, 432.1255f, 139.3781f),
                        CameraDirection = new Vector3(0.4488232f, -0.8689867f, -0.2083742f),
                        CameraRotation = new Rotator( - 12.02709f, 3.142567E-05f, -152.6841f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                },
                OutfitInteracts = new List<OutfitInteract>() {
                    new OutfitInteract()
                    {
                        Name = "2045NorthConkOutfit1",
                        Position = new Vector3(374.325f, 411.6066f, 142.1001f),
                        Heading = 166.6548f,
                        CameraPosition = new Vector3(373.709f, 408.8297f, 142.9015f),
                        CameraDirection = new Vector3(0.2510608f, 0.9399703f, -0.2311372f),
                        CameraRotation = new Rotator( - 13.36403f, -1.64538E-05f, -14.9543f),
                        ButtonPromptText = "Change Outfit",
                        UseNavmesh = false,
                    },
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract()
                    {
                        Name = "2045NorthConkExit1",
                        Position = new Vector3(373.59f, 423.5691f, 145.9079f),
                        Heading = 345.1169f,
                        ButtonPromptText = "Exit",
                        UseNavmesh = false,
                    },
                    new StandardInteriorInteract()
                    {
                        Name = "2045NorthConkStd1",
                        Position = new Vector3(371.3817f, 410.5682f, 145.7f),
                        Heading = 342.7884f,
                        CameraPosition = new Vector3(371.0208f, 414.4162f, 146.8516f),
                        CameraDirection = new Vector3(0.05308443f, -0.9791117f, -0.1962712f),
                        CameraRotation = new Rotator( - 11.31899f, -3.26516E-07f, -176.8966f),
                        UseNavmesh = false,
                    },
                    new ToiletInteract()
                    {
                        Name = "2045NorthConkToilet1",
                        Position = new Vector3(379.441f, 416.4362f, 142.1003f),
                        Heading = 259.2629f,
                        CameraPosition = new Vector3(376.517f, 415.0572f, 143.542f),
                        CameraDirection = new Vector3(0.871114f, 0.3520918f, -0.3423329f),
                        CameraRotation = new Rotator( - 20.01907f, -9.08677E-07f, -67.99214f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "2045NorthConkSink1",
                        Position = new Vector3(378.5627f, 418.1747f, 142.1121f),
                        Heading = 345.3333f,
                        CameraPosition = new Vector3(376.1497f, 417.2711f, 143.216f), 
                        CameraDirection = new Vector3(0.9266453f, 0.2660088f, -0.2656461f), 
                        CameraRotation = new Rotator(-15.40535f, -1.284109E-05f, -73.983f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "2045NorthConkSink2",
                        Position = new Vector3(379.9853f, 417.8654f, 142.1003f),
                        Heading = 346.0681f,
                        CameraPosition = new Vector3(376.1497f, 417.2711f, 143.216f),
                        CameraDirection = new Vector3(0.9266453f, 0.2660088f, -0.2656461f),
                        CameraRotation = new Rotator(-15.40535f, -1.284109E-05f, -73.983f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                },
            },
            new ResidenceInterior(207105, "3655 Wild Oats Drive")
            {
                InternalInteriorCoordinates = new Vector3(-169.286f, 486.4938f, 137.4436f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-174.15f, 497.3787f, 137.667f),
                InteriorEgressHeading = 192.5397f,
                RestInteracts = new List<RestInteract>()
                {
                    new RestInteract()
                    {
                        StartAnimations = new List < AnimationBundle > () {},
                        LoopAnimations = new List < AnimationBundle > () {},
                        EndAnimations = new List < AnimationBundle > () {},
                        Name = "3655WildOatRest1",
                        Position = new Vector3( - 163.504f, 485.4048f, 133.8696f),
                        Heading = 191.6281f,
                        InteractDistance = 1f,
                        CameraPosition = new Vector3( - 167.3033f, 485.3243f, 135.2373f),
                        CameraDirection = new Vector3(0.8730214f, -0.3290629f, -0.3599322f),
                        CameraRotation = new Rotator( - 21.09603f, -9.608605E-06f, -110.6526f),
                        ButtonPromptText = "Sleep",
                        UseNavmesh = false,
                    },
                },
                InventoryInteracts = new List<InventoryInteract>()
                {
                    new InventoryInteract()
                    {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Name = "3655WildOatInventory1",
                        Position = new Vector3( - 167.2706f, 496.5005f, 137.6537f),
                        Heading = 13.35964f,
                        CameraPosition = new Vector3( - 170.0928f, 493.4945f, 138.954f),
                        CameraDirection = new Vector3(0.5746922f, 0.773095f, -0.2684271f),
                        CameraRotation = new Rotator( - 15.57069f, -1.595342E-05f, -36.62584f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Name = "3655WildOatInventory2",
                        Position = new Vector3( - 176.0963f, 492.0989f, 130.0437f),
                        Heading = 106.0434f,
                        CameraPosition = new Vector3( - 173.5369f, 489.9279f, 130.9913f),
                        CameraDirection = new Vector3( - 0.7695915f, 0.5710856f, -0.2856399f),
                        CameraRotation = new Rotator( - 16.59711f, -3.385385E-05f, 53.42225f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                },
                OutfitInteracts = new List<OutfitInteract>()
                {
                    new OutfitInteract()
                    {
                        Name = "3655WildOatOutfit1",
                        Position = new Vector3( - 167.4785f, 487.9693f, 133.8437f),
                        Heading = 186.7014f,
                        CameraPosition = new Vector3( - 167.0799f, 485.2438f, 134.6422f),
                        CameraDirection = new Vector3( - 0.1694784f, 0.949171f, -0.2652385f),
                        CameraRotation = new Rotator( - 15.38112f, -7.305287E-06f, 10.12371f),
                        ButtonPromptText = "Change Outfit",
                        UseNavmesh = false,
                    },
                },
                InteractPoints = new List<InteriorInteract>()
                {
                    new ExitInteriorInteract()
                    {
                        Name = "3655WildOatExit1",
                        Position = new Vector3( - 174.15f, 497.3787f, 137.667f),
                        Heading = 15.29212f,
                        ButtonPromptText = "Exit",
                        UseNavmesh = false,
                    },
                    new StandardInteriorInteract()
                    {
                        Name = "3655WildOatStd1",
                        Position = new Vector3( - 169.6604f, 485.4504f, 137.4436f),
                        Heading = 16.84413f,
                        CameraPosition = new Vector3( - 171.98f, 488.6649f, 138.856f),
                        CameraDirection = new Vector3(0.640749f, -0.7213035f, -0.2629869f),
                        CameraRotation = new Rotator( - 15.24737f, 2.65477E-06f, -138.3846f),
                        UseNavmesh = false,
                    },
                    new ToiletInteract()
                    {
                        Name = "3655WildOatToilet1",
                        Position = new Vector3( - 164.9799f, 494.3438f, 133.8438f),
                        Heading = 281.0751f,
                        CameraPosition = new Vector3( - 166.7136f, 491.73f, 135.3105f),
                        CameraDirection = new Vector3(0.5075332f, 0.817638f, -0.2718054f),
                        CameraRotation = new Rotator( - 15.77173f, -6.653801E-06f, -31.82915f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "3655WildOatSink1",
                        Position = new Vector3(-166.4765f, 495.5452f, 133.856f),
                        Heading = 7.877475f,
                        CameraPosition = new Vector3(-168.1566f, 493.3765f, 134.8811f), 
                        CameraDirection = new Vector3(0.6337749f, 0.7336531f, -0.2451171f), 
                        CameraRotation = new Rotator(-14.18876f, 2.641916E-06f, -40.82248f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract()
                    {
                        Name = "3655WildOatSink2",
                        Position = new Vector3(-165.0066f, 495.7455f, 133.8438f),
                        Heading = 9.133607f,
                        CameraPosition = new Vector3(-168.1566f, 493.3765f, 134.8811f),
                        CameraDirection = new Vector3(0.6337749f, 0.7336531f, -0.2451171f),
                        CameraRotation = new Rotator(-14.18876f, 2.641916E-06f, -40.82248f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                },
            },

            //Additional MP Houses
            new ResidenceInterior(206593,"3677 Whispymound Drive") {
                RestInteracts = new List < RestInteract > () {
                    new RestInteract() {
                        StartAnimations = new List < AnimationBundle > () {},
                        LoopAnimations = new List < AnimationBundle > () {},
                        EndAnimations = new List < AnimationBundle > () {},
                        Name = "3677WhispyMDrRest1",
                        Position = new Vector3(125.8886f, 546.3633f, 180.5226f),
                        Heading = 182.4487f,
                        CameraPosition = new Vector3(122.6701f, 546.2838f, 181.7142f),
                        CameraDirection = new Vector3(0.7625598f, -0.5107692f, -0.3970105f),
                        CameraRotation = new Rotator( - 23.39142f, 9.30225E-07f, -123.8145f),
                        ButtonPromptText = "Sleep",
                        UseNavmesh = false,
                    },
                },
                InventoryInteracts = new List < InventoryInteract > () {
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Title = "",
                        Description = "",
                        Name = "3677WhispyMDrInventory1",
                        Position = new Vector3(123.1506f, 557.3663f, 184.2971f),
                        Heading = 4.182598f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(121.4272f, 556.2952f, 185.3616f),
                        CameraDirection = new Vector3(0.7402999f, 0.5385184f, -0.402435f),
                        CameraRotation = new Rotator( - 23.73049f, -2.891149E-05f, -53.96663f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessWeapons = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Title = "",
                        Description = "",
                        Name = "3677WhispyMDrInventory2",
                        Position = new Vector3(118.6933f, 543.4891f, 180.4973f),
                        Heading = 96.76508f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(119.4411f, 545.4094f, 181.4012f),
                        CameraDirection = new Vector3( - 0.5917099f, -0.6976438f, -0.4039463f),
                        CameraRotation = new Rotator( - 23.82512f, -9.333075E-07f, 139.6969f),
                        ButtonPromptText = "Cash Drawer",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List < ItemType > () {},
                        DisallowedItemTypes = new List < ItemType > () {},
                        Title = "",
                        Description = "",
                        Name = "3677WhispyMDrInventory3",
                        Position = new Vector3(120.2305f, 567.5979f, 176.6971f),
                        Heading = 279.2416f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(118.9552f, 569.4901f, 177.5214f),
                        CameraDirection = new Vector3(0.6417705f, -0.6729101f, -0.3678623f),
                        CameraRotation = new Rotator( - 21.58384f, -3.397171E-05f, -136.3569f),
                        ButtonPromptText = "Weapons Locker",
                        UseNavmesh = false,
                    },
                },
                OutfitInteracts = new List < OutfitInteract > () {
                    new OutfitInteract() {
                        Name = "3677WhispyMDrOutfit1",
                        Position = new Vector3(122.0902f, 548.9814f, 180.4972f),
                        Heading = 183.1734f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(122.407f, 546.5842f, 181.1327f),
                        CameraDirection = new Vector3( - 0.09616696f, 0.9643564f, -0.2465129f),
                        CameraRotation = new Rotator( - 14.27126f, -2.202401E-07f, 5.694788f),
                        ButtonPromptText = "Change Outfit",
                        UseNavmesh = false,
                    },
                },
                InternalInteriorCoordinates = new Vector3(120.5f, 549.952f, 184.097f),
                IsTeleportEntry = true,
                Doors = new List <InteriorDoor> () {},
                RequestIPLs = new List <String> () {},
                RemoveIPLs = new List <String> () {},
                InteriorSets = new List <String> () {},
                InteriorEgressPosition = new Vector3(117.3436f, 559.7256f, 184.3049f),
                InteriorEgressHeading = 188.2407f,
                InteractPoints = new List <InteriorInteract> () {
                    new ExitInteriorInteract() {
                        Name = "3677WhispyMDrExit1",
                        Position = new Vector3(117.3436f, 559.7256f, 184.3049f),
                        Heading = 6.260241f,
                        InteractDistance = 2f,
                        ButtonPromptText = "Exit",
                        UseNavmesh = false,
                    },
                    new StandardInteriorInteract() {
                        Name = "3677WhispyMDrStd1",
                        Position = new Vector3(119.7448f, 546.8057f, 184.097f),
                        Heading = 6.364705f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(118.0364f, 548.9323f, 185.1137f),
                        CameraDirection = new Vector3(0.5115602f, -0.8026207f, -0.3067673f),
                        CameraRotation = new Rotator( - 17.86452f, 5.113036E-05f, -147.4881f),
                        UseNavmesh = false,
                    },
                    new ToiletInteract() {
                        Name = "3677WhispyMDrToilet1",
                        Position = new Vector3(125.0282f, 555.0813f, 180.4973f),
                        Heading = 279.1512f,
                        CameraPosition = new Vector3(123.8326f, 553.5462f, 181.3578f),
                        CameraDirection = new Vector3(0.732986f, 0.5595635f, -0.3868077f),
                        CameraRotation = new Rotator( - 22.75601f, -6.480885E-05f, -52.64179f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "3677WhispyMDrWash1",
                        Position = new Vector3(123.7435f, 556.5359f, 180.5091f),
                        Heading = 7.017373f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(122.4933f, 555.8272f, 181.4969f),
                        CameraDirection = new Vector3(0.6746727f, 0.6210913f, -0.3988262f),
                        CameraRotation = new Rotator( - 23.50482f, -2.793072E-05f, -47.3679f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "3677WhispyMDrWash2",
                        Position = new Vector3(125.1659f, 556.5369f, 180.4973f),
                        Heading = 5.326504f,
                        InteractDistance = 2f,
                        CameraPosition = new Vector3(122.4933f, 555.8272f, 181.4969f),
                        CameraDirection = new Vector3(0.6746727f, 0.6210913f, -0.3988262f),
                        CameraRotation = new Rotator( - 23.50482f, -2.793072E-05f, -47.3679f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                },
                ClearPositions = new List <Vector3> () {},
            },
            new ResidenceInterior(207361, "2874 Hillcrest Avenue")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                StartAnimations = new List < AnimationBundle > () {},
                LoopAnimations = new List < AnimationBundle > () {},
                EndAnimations = new List < AnimationBundle > () {},
                Name = "2874HillcrestAveRest1",
                Position = new Vector3( - 851.5009f, 677.3481f, 149.0785f),
                Heading = 186.0841f,
                CameraPosition = new Vector3( - 854.7201f, 677.6414f, 150.2451f),
                CameraDirection = new Vector3(0.6550292f, -0.6153208f, -0.4385398f),
                CameraRotation = new Rotator( - 26.01075f, 9.499971E-07f, -133.2096f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2874HillcrestAveInventory1",
                Position = new Vector3( - 854.1332f, 688.7078f, 152.853f),
                Heading = 7.001656f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 855.8873f, 687.586f, 154.0107f),
                CameraDirection = new Vector3(0.7420815f, 0.5778303f, -0.3397459f),
                CameraRotation = new Rotator( - 19.86139f, -2.632534E-05f, -52.09348f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2874HillcrestAveInventory2",
                Position = new Vector3( - 858.9979f, 674.7934f, 149.0531f),
                Heading = 102.5044f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 857.7955f, 677.0128f, 150.0618f),
                CameraDirection = new Vector3( - 0.6351141f, -0.7019386f, -0.3223543f),
                CameraRotation = new Rotator( - 18.80536f, -1.89403E-05f, 137.8612f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2874HillcrestAveInventory3",
                Position = new Vector3( - 856.6957f, 698.8257f, 145.253f),
                Heading = 277.9463f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 857.8467f, 700.6895f, 146.2396f),
                CameraDirection = new Vector3(0.6652163f, -0.6734213f, -0.3224765f),
                CameraRotation = new Rotator( - 18.81276f, -1.262742E-05f, -135.3512f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "2874HillcrestAveOutfit1",
                Position = new Vector3( - 855.4317f, 680.1762f, 149.053f),
                Heading = 180.8558f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 855.2012f, 677.9338f, 149.7829f),
                CameraDirection = new Vector3( - 0.05468202f, 0.9638299f, -0.260848f),
                CameraRotation = new Rotator( - 15.12039f, -9.396657E-07f, 3.247143f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(-857.798f, 682.563f, 152.6529f),
                IsTeleportEntry = true,
                Doors = new List<InteriorDoor>() { },
                RequestIPLs = new List<String>() { },
                RemoveIPLs = new List<String>() { },
                InteriorSets = new List<String>() { },
                InteriorEgressPosition = new Vector3(-859.9145f, 691.2387f, 152.8607f),
                InteriorEgressHeading = 185.0775f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2874HillcrestAveExit1",
                Position = new Vector3( - 859.9145f, 691.2387f, 152.8607f),
                Heading = 356.9291f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2874HillcrestAveStd1",
                Position = new Vector3( - 857.4491f, 678.1083f, 152.6529f),
                Heading = 0.4769578f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 858.7364f, 680.4945f, 153.5299f),
                CameraDirection = new Vector3(0.4019186f, -0.8688135f, -0.2891791f),
                CameraRotation = new Rotator( - 16.80882f, 2.36348E-05f, -155.1744f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2874HillcrestAveToilet1",
                Position = new Vector3(-852.113f, 686.1462f, 149.0531f),
                Heading = 275.3149f,
                CameraPosition = new Vector3( - 853.4448f, 684.4023f, 150.0508f),
                CameraDirection = new Vector3(0.7114994f, 0.5939562f, -0.3754793f),
                CameraRotation = new Rotator( - 22.05394f, -2.671408E-05f, -50.14504f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2874HillcrestAveWash1",
                Position = new Vector3( - 853.4738f, 687.613f, 149.065f),
                Heading = 5.279492f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 854.678f, 687.0937f, 150.0155f),
                CameraDirection = new Vector3(0.65728f, 0.6045602f, -0.4499889f),
                CameraRotation = new Rotator( - 26.74297f, 0f, -47.39243f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2874HillcrestAveWash2",
                Position = new Vector3(-852.0748f, 687.5978f, 149.0531f),
                Heading = 358.6225f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 854.678f, 687.0937f, 150.0155f),
                CameraDirection = new Vector3(0.65728f, 0.6045602f, -0.4499889f),
                CameraRotation = new Rotator( - 26.74297f, 0f, -47.39243f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
            new ResidenceInterior(207617,"2868 Hillcrest Avenue")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                StartAnimations = new List < AnimationBundle > () {},
                LoopAnimations = new List < AnimationBundle > () {},
                EndAnimations = new List < AnimationBundle > () {},
                Name = "2868HillcrestAveRest1",
                Position = new Vector3( - 769.225f, 606.6739f, 140.3565f),
                Heading = 110.8262f,
                CameraPosition = new Vector3( - 769.3904f, 610.0786f, 141.6123f),
                CameraDirection = new Vector3( - 0.3373653f, -0.8550257f, -0.3938473f),
                CameraRotation = new Rotator( - 23.1941f, -3.250964E-06f, 158.4674f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2868HillcrestAveInventory1",
                Position = new Vector3( - 758.6924f, 611.904f, 144.1406f),
                Heading = 290.364f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 760.3583f, 613.0176f, 145.2854f),
                CameraDirection = new Vector3(0.8517901f, -0.3671363f, -0.3737172f),
                CameraRotation = new Rotator( - 21.94505f, 3.221639E-05f, -113.3169f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2868HillcrestAveInventory2",
                Position = new Vector3( - 764.1301f, 619.9535f, 136.5305f),
                Heading = 21.27656f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 765.7929f, 618.752f, 137.5933f),
                CameraDirection = new Vector3(0.5053546f, 0.7572073f, -0.4138283f),
                CameraRotation = new Rotator( - 24.44555f, 4.032742E-05f, -33.71886f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2868HillcrestAveInventory3",
                Position = new Vector3( - 773.3774f, 612.9773f, 140.3313f),
                Heading = 16.64957f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 771.3787f, 612.6984f, 141.3906f),
                CameraDirection = new Vector3( - 0.847395f, 0.3318431f, -0.4144899f),
                CameraRotation = new Rotator( - 24.4872f, 3.752629E-06f, 68.6145f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "2868HillcrestAveOutfit1",
                Position = new Vector3( - 767.215f, 611.0546f, 140.3307f),
                Heading = 106.0402f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 769.3765f, 610.3242f, 141.1601f),
                CameraDirection = new Vector3(0.897975f, 0.3076635f, -0.3146175f),
                CameraRotation = new Rotator( - 18.33772f, 4.317354E-05f, -71.08755f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(-763.107f, 615.906f, 144.1401f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-758.3497f, 618.9664f, 144.1531f),
                InteriorEgressHeading = 105.945f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2868HillcrestAveExit1",
                Position = new Vector3( - 758.3497f, 618.9664f, 144.1531f),
                Heading = 285.5187f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2868HillcrestAveStd1",
                Position = new Vector3( - 769.757f, 612.5219f, 143.9305f),
                Heading = 283.0699f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 767.4705f, 614.4906f, 144.9521f),
                CameraDirection = new Vector3( - 0.7332091f, -0.6398386f, -0.2302413f),
                CameraRotation = new Rotator( - 13.31128f, 2.632034E-06f, 131.1097f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2868HillcrestAveToilet1",
                Position = new Vector3(-760.5676f, 609.4951f, 140.3307f),
                Heading = 193.8453f,
                CameraPosition = new Vector3( - 762.5089f, 610.7808f, 141.3424f),
                CameraDirection = new Vector3(0.7697082f, -0.523907f, -0.3647887f),
                CameraRotation = new Rotator( - 21.39458f, -1.833922E-06f, -124.2414f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2868HillcrestAveWash1",
                Position = new Vector3( - 759.4769f, 611.0027f, 140.343f),
                Heading = 285.4872f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 759.9645f, 612.1797f, 141.4061f),
                CameraDirection = new Vector3(0.6600884f, -0.5283374f, -0.5339877f),
                CameraRotation = new Rotator( - 32.27528f, -0.0001100676f, -128.6739f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2868HillcrestAveWash2",
                Position = new Vector3(-758.9645f, 609.5732f, 140.3307f),
                Heading = 287.3187f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 759.9645f, 612.1797f, 141.4061f),
                CameraDirection = new Vector3(0.6600884f, -0.5283374f, -0.5339877f),
                CameraRotation = new Rotator( - 32.27528f, -0.0001100676f, -128.6739f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
            new ResidenceInterior(208641,"2866 Hillcrest Avenue")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                StartAnimations = new List < AnimationBundle > () {},
                LoopAnimations = new List < AnimationBundle > () {},
                EndAnimations = new List < AnimationBundle > () {},
                Name = "2866HillcrestAveRest1",
                Position = new Vector3( - 741.7344f, 578.2546f, 142.486f),
                Heading = 146.448f,
                CameraPosition = new Vector3( - 744.536f, 579.7596f, 143.4573f),
                CameraDirection = new Vector3(0.3138163f, -0.8604938f, -0.4013349f),
                CameraRotation = new Rotator( - 23.66166f, 1.444812E-05f, -159.9634f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2866HillcrestAveInventory1",
                Position = new Vector3( - 737.59f, 589.0109f, 146.2604f),
                Heading = 334.0882f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 739.4871f, 588.8943f, 147.0714f),
                CameraDirection = new Vector3(0.9305623f, 0.05484095f, -0.3620033f),
                CameraRotation = new Rotator( - 21.22327f, -4.579462E-07f, -86.62728f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2866HillcrestAveInventory2",
                Position = new Vector3( - 749.4645f, 579.9824f, 142.4606f),
                Heading = 63.62537f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 747.8635f, 581.3597f, 143.2503f),
                CameraDirection = new Vector3( - 0.86753f, -0.3294917f, -0.3725947f),
                CameraRotation = new Rotator( - 21.87573f, -1.472033E-05f, 110.797f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2866HillcrestAveInventory3",
                Position = new Vector3( - 734.3171f, 598.8799f, 138.6604f),
                Heading = 245.9131f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 734.1554f, 601.0134f, 139.4422f),
                CameraDirection = new Vector3( - 0.01822698f, -0.9537277f, -0.3001186f),
                CameraRotation = new Rotator( - 17.46473f, 6.433049E-07f, 178.9051f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "2866HillcrestAveOutfit1",
                Position = new Vector3( - 743.4583f, 582.577f, 142.4605f),
                Heading = 150.176f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 744.5214f, 580.3284f, 143.2913f),
                CameraDirection = new Vector3(0.4879013f, 0.8173583f, -0.3063948f),
                CameraRotation = new Rotator( - 17.8421f, 3.587644E-06f, -30.83402f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(-746.6974f, 576.9874f, 144.86f),
                IsTeleportEntry = true,
                RequestIPLs = new List<string>() {
                        "apa_stilt_ch2_09c_int",
                    },
                InteriorEgressPosition = new Vector3(-741.0357f, 594.1995f, 146.2682f),
                InteriorEgressHeading = 146.5121f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2866HillcrestAveExit1",
                Position = new Vector3( - 741.0357f, 594.1995f, 146.2682f),
                Heading = 331.7428f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2866HillcrestAveStd1",
                Position = new Vector3( - 746.2287f, 582.1323f, 146.0603f),
                Heading = 332.7006f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 746.3914f, 584.8408f, 147.1075f),
                CameraDirection = new Vector3( - 0.08847187f, -0.9578879f, -0.273173f),
                CameraRotation = new Rotator( - 15.85317f, 4.437655E-07f, 174.7231f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2866HillcrestAveToilet1",
                Position = new Vector3(-737.3578f, 585.8151f, 142.4605f),
                Heading = 242.0518f,
                CameraPosition = new Vector3( - 739.1013f, 585.1049f, 143.1959f),
                CameraDirection = new Vector3(0.9369323f, 0.06204982f, -0.3439589f),
                CameraRotation = new Rotator( - 20.11826f, -3.182382E-06f, -86.21103f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2866HillcrestAveWash1",
                Position = new Vector3( - 737.6904f, 587.8032f, 142.4725f),
                Heading = 328.55f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 738.9469f, 587.8602f, 143.2228f),
                CameraDirection = new Vector3(0.8772637f, 0.1506086f, -0.455769f),
                CameraRotation = new Rotator( - 27.11443f, -7.673524E-06f, -80.25843f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2866HillcrestAveWash2",
                Position = new Vector3(-736.4802f, 587.0096f, 142.4606f),
                Heading = 330.0962f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 738.9469f, 587.8602f, 143.2228f),
                CameraDirection = new Vector3(0.8772637f, 0.1506086f, -0.455769f),
                CameraRotation = new Rotator( - 27.11443f, -7.673524E-06f, -80.25843f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
            new ResidenceInterior(208129,"2862 Hillcrest Avenue")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                StartAnimations = new List < AnimationBundle > () {},
                LoopAnimations = new List < AnimationBundle > () {},
                EndAnimations = new List < AnimationBundle > () {},
                Name = "2862HillcrestAveRest1",
                Position = new Vector3( - 666.8728f, 587.166f, 141.5957f),
                Heading = 221.7262f,
                CameraPosition = new Vector3( - 669.5845f, 585.5392f, 143.117f),
                CameraDirection = new Vector3(0.8944548f, 0.07008594f, -0.4416318f),
                CameraRotation = new Rotator( - 26.20804f, -1.665303E-06f, -85.51968f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2862HillcrestAveInventory1",
                Position = new Vector3( - 675.6671f, 594.9548f, 145.3796f),
                Heading = 41.49871f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 676.6571f, 592.7364f, 146.5602f),
                CameraDirection = new Vector3(0.2633265f, 0.8719893f, -0.4126667f),
                CameraRotation = new Rotator( - 24.37247f, -8.435738E-06f, -16.80346f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2862HillcrestAveInventory2",
                Position = new Vector3( - 681.2135f, 586.8436f, 137.7697f),
                Heading = 131.3576f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 678.8915f, 585.951f, 138.7892f),
                CameraDirection = new Vector3( - 0.9406421f, 0.1436474f, -0.3075028f),
                CameraRotation = new Rotator( - 17.9088f, 4.037615E-06f, 81.31732f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2862HillcrestAveInventory3",
                Position = new Vector3( - 671.2198f, 580.9656f, 141.5739f),
                Heading = 131.8276f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 671.6243f, 583.123f, 142.5302f),
                CameraDirection = new Vector3(0.06705433f, -0.9498118f, -0.3055509f),
                CameraRotation = new Rotator( - 17.79131f, -1.120819E-07f, -175.9618f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "2862HillcrestAveOutfit1",
                Position = new Vector3( - 671.5975f, 587.3071f, 141.5698f),
                Heading = 219.6087f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 670.068f, 585.3218f, 142.2223f),
                CameraDirection = new Vector3( - 0.631759f, 0.7347354f, -0.2470715f),
                CameraRotation = new Rotator( - 14.30429f, 0f, 40.69044f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(-676.127f, 588.612f, 145.1698f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-682.1874f, 592.3237f, 145.393f),
                InteriorEgressHeading = 216.9307f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2862HillcrestAveExit1",
                Position = new Vector3( - 682.1874f, 592.3237f, 145.393f),
                Heading = 37.03963f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2862HillcrestAveStd1",
                Position = new Vector3( - 671.9644f, 584.4865f, 145.1697f),
                Heading = 39.51886f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 674.3513f, 585.4233f, 146.0696f),
                CameraDirection = new Vector3(0.8744266f, -0.3722682f, -0.3111181f),
                CameraRotation = new Rotator( - 18.12663f, 7.636043E-06f, -113.0608f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2862HillcrestAveToilet1",
                Position = new Vector3(-672.6783f, 594.1899f, 141.5699f),
                Heading = 315.49f,
                CameraPosition = new Vector3( - 672.995f, 592.0282f, 142.6447f),
                CameraDirection = new Vector3(0.2366741f, 0.8770948f, -0.4179595f),
                CameraRotation = new Rotator( - 24.70583f, -7.988276E-06f, -15.10096f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2862HillcrestAveWash1",
                Position = new Vector3( - 674.5394f, 594.6476f, 141.5821f),
                Heading = 43.3874f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 675.2704f, 593.658f, 142.5043f),
                CameraDirection = new Vector3(0.2019831f, 0.8624508f, -0.4640921f),
                CameraRotation = new Rotator( - 27.65148f, -6.265083E-06f, -13.18092f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2862HillcrestAveWash2",
                Position = new Vector3(-673.4442f, 595.5475f, 141.5699f),
                Heading = 41.78911f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 675.2704f, 593.658f, 142.5043f),
                CameraDirection = new Vector3(0.2019831f, 0.8624508f, -0.4640921f),
                CameraRotation = new Rotator( - 27.65148f, -6.265083E-06f, -13.18092f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
            new ResidenceInterior(207873,"2117 Milton Road")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                Name = "2117MiltonRoadRest1",
                Position = new Vector3( - 568.4893f, 646.1376f, 142.0576f),
                Heading = 167.9836f,
                CameraPosition = new Vector3( - 571.7147f, 647.1907f, 143.1924f),
                CameraDirection = new Vector3(0.5084095f, -0.7622369f, -0.4006428f),
                CameraRotation = new Rotator( - 23.61837f, -3.820498E-05f, -146.2968f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2117MiltonRoadInventory1",
                Position = new Vector3( - 567.3121f, 657.5324f, 145.8321f),
                Heading = 344.1812f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 569.2823f, 656.8463f, 146.7616f),
                CameraDirection = new Vector3(0.8621153f, 0.3152996f, -0.3966652f),
                CameraRotation = new Rotator( - 23.36987f, -4.557362E-05f, -69.91113f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2117MiltonRoadInventory2",
                Position = new Vector3( - 576.2966f, 645.946f, 142.0323f),
                Heading = 78.14932f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 575.0161f, 648.0704f, 142.9277f),
                CameraDirection = new Vector3( - 0.7564366f, -0.5147552f, -0.4035228f),
                CameraRotation = new Rotator( - 23.79859f, -3.359221E-05f, 124.2353f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "2117MiltonRoadInventory3",
                Position = new Vector3( - 566.5204f, 668.0264f, 138.2321f),
                Heading = 257.7583f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 567.1656f, 669.7759f, 139.2798f),
                CameraDirection = new Vector3(0.3714076f, -0.8614651f, -0.3463154f),
                CameraRotation = new Rotator( - 20.26211f, -2.138716E-05f, -156.6774f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "2117MiltonRoadOutfit1",
                Position = new Vector3( - 571.2106f, 650.0455f, 142.0322f),
                Heading = 160.594f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 571.7885f, 647.773f, 142.8578f),
                CameraDirection = new Vector3(0.2578968f, 0.9203916f, -0.2938852f),
                CameraRotation = new Rotator( - 17.0907f, 0f, -15.65305f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(-573.0324f, 643.7613f, 144.4316f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-571.8827f, 661.8361f, 145.8399f),
                InteriorEgressHeading = 165.7903f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2117MiltonRoadExit1",
                Position = new Vector3( - 571.8827f, 661.8361f, 145.8399f),
                Heading = 346.0906f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2117MiltonRoadStd1",
                Position = new Vector3( - 573.8051f, 648.6808f, 145.632f),
                Heading = 348.8827f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 574.5377f, 650.7093f, 146.6002f),
                CameraDirection = new Vector3(0.2257358f, -0.9093344f, -0.3495059f),
                CameraRotation = new Rotator( - 20.45709f, 1.457987E-05f, -166.0585f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2117MiltonRoadToilet1",
                Position = new Vector3(-566.2118f, 654.5648f, 142.0323f),
                Heading = 254.0167f,
                CameraPosition = new Vector3( - 567.6953f, 653.4572f, 143.0564f),
                CameraDirection = new Vector3(0.8589513f, 0.2903567f, -0.4217767f),
                CameraRotation = new Rotator( - 24.94681f, 1.553686E-05f, -71.32288f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2117MiltonRoadWash1",
                Position = new Vector3( - 567.0015f, 656.3623f, 142.0441f),
                Heading = 341.7439f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 568.6034f, 656.2526f, 143.051f),
                CameraDirection = new Vector3(0.8414465f, 0.1797778f, -0.5095565f),
                CameraRotation = new Rotator( - 30.63429f, 2.282188E-05f, -77.9399f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2117MiltonRoadWash2",
                Position = new Vector3(-565.6008f, 656.0184f, 142.0322f),
                Heading = 345.5208f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 568.6034f, 656.2526f, 143.051f),
                CameraDirection = new Vector3(0.8414465f, 0.1797778f, -0.5095565f),
                CameraRotation = new Rotator( - 30.63429f, 2.282188E-05f, -77.9399f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
            new ResidenceInterior(208385,"2113 Mad Wayne Thunder Drive")
            {
                RestInteracts = new List<RestInteract>() {
            new RestInteract() {
                Name = "2113MadWayneRest1",
                Position = new Vector3( - 1282.646f, 435.1075f, 94.12035f),
                Heading = 185.5997f,
                CameraPosition = new Vector3( - 1285.653f, 435.5347f, 95.28134f),
                CameraDirection = new Vector3(0.6979379f, -0.5768532f, -0.4244092f),
                CameraRotation = new Rotator( - 25.11327f, 1.414359E-05f, -129.5741f),
                ButtonPromptText = "Sleep",
                UseNavmesh = false,
            },
        },
                InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "3677WhispyMDrInventory1",
                Position = new Vector3( - 1284.266f, 446.57f, 97.89471f),
                Heading = 2.661436f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1286.199f, 445.6415f, 98.73716f),
                CameraDirection = new Vector3(0.8332375f, 0.4513904f, -0.3193149f),
                CameraRotation = new Rotator( - 18.6215f, -7.207512E-06f, -61.55423f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "3677WhispyMDrInventory2",
                Position = new Vector3( - 1290.167f, 433.09f, 94.09482f),
                Heading = 95.55701f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1289.279f, 435.2166f, 95.02458f),
                CameraDirection = new Vector3( - 0.6014408f, -0.7210578f, -0.3440125f),
                CameraRotation = new Rotator( - 20.12153f, -6.364896E-06f, 140.1682f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
            },
            new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List < ItemType > () {},
                DisallowedItemTypes = new List < ItemType > () {},
                Title = "",
                Description = "",
                Name = "3677WhispyMDrInventory3",
                Position = new Vector3( - 1286.003f, 457.0468f, 90.29469f),
                Heading = 268.169f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1286.948f, 458.8978f, 91.06673f),
                CameraDirection = new Vector3(0.5200119f, -0.7859902f, -0.3343756f),
                CameraRotation = new Rotator( - 19.53458f, -4.529592E-06f, -146.5113f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
            },
        },
                OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
                Name = "3677WhispyMDrOutfit1",
                Position = new Vector3( - 1286.221f, 438.1595f, 94.0948f),
                Heading = 176.0847f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1286.163f, 435.8915f, 94.92171f),
                CameraDirection = new Vector3(0.01443324f, 0.9546465f, -0.2973915f),
                CameraRotation = new Rotator( - 17.301f, 6.147849E-07f, -0.8661853f),
                ButtonPromptText = "Change Outfit",
                UseNavmesh = false,
            },
        },
                InternalInteriorCoordinates = new Vector3(120.5f, 549.952f, 184.097f),
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(-1289.709f, 449.4589f, 97.90252f),
                InteriorEgressHeading = 180.0762f,
                InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
                Name = "2113MadWayneExit1",
                Position = new Vector3( - 1289.709f, 449.4589f, 97.90252f),
                Heading = 1.617516f,
                InteractDistance = 2f,
                ButtonPromptText = "Exit",
                UseNavmesh = false,
            },
            new StandardInteriorInteract() {
                Name = "2113MadWayneStd1",
                Position = new Vector3( - 1288.58f, 436.4243f, 97.69458f),
                Heading = 1.099969f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1290.047f, 438.8174f, 98.54319f),
                CameraDirection = new Vector3(0.4059573f, -0.869975f, -0.2798966f),
                CameraRotation = new Rotator( - 16.25404f, 2.667959E-06f, -154.9848f),
                UseNavmesh = false,
            },
            new ToiletInteract() {
                Name = "2113MadWayneToilet1",
                Position = new Vector3(-1282.475f, 443.9754f, 94.09483f),
                Heading = 270.4488f,
                CameraPosition = new Vector3( - 1283.755f, 442.5715f, 94.99776f),
                CameraDirection = new Vector3(0.7434317f, 0.5413036f, -0.39281f),
                CameraRotation = new Rotator( - 23.12946f, -6.498791E-05f, -53.94109f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2113MadWayneWash1",
                Position = new Vector3( - 1283.7f, 445.5079f, 94.10678f),
                Heading = 4.615284f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1284.896f, 445.025f, 94.89018f),
                CameraDirection = new Vector3(0.7028471f, 0.6098564f, -0.366171f),
                CameraRotation = new Rotator( - 21.47967f, -1.559743E-05f, -49.05201f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
            new SinkInteract() {
                Name = "2113MadWayneWash2",
                Position = new Vector3(-1282.236f, 445.4718f, 94.09483f),
                Heading = 359.5689f,
                InteractDistance = 2f,
                CameraPosition = new Vector3( - 1284.896f, 445.025f, 94.89018f),
                CameraDirection = new Vector3(0.7028471f, 0.6098564f, -0.366171f),
                CameraRotation = new Rotator( - 21.47967f, -1.559743E-05f, -49.05201f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
            },
        },
                ClearPositions = new List<Vector3>() { },
            },
        });
    }
    private void Other()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {


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
            { 
                DisabledInteriorCoords = new Vector3(-1388.0013427734375f, -618.419677734375f, 30.819599151611328f),
                IsTeleportEntry = true,InteriorEgressPosition = new Vector3(-1388.775f, -589.7048f, 30.31956f), InteriorEgressHeading =  139.5796f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("bahamamamaexit1",new Vector3(-1387.947f, -588.3078f, 30.31954f),22.09038f ,"Exit") ,

                },

            },


            new Interior(-999565,"Split Sides Comedy Club",new List<string>() { },new List<string>() { }),





            new Interior(89602,"Yellow Jacket Inn"),
            new Interior(118018,"Vanilla Unicorn"),

            //new Interior(171777,"Apartment"){ RemoveIPLs = new List<string>() { "vb_30_crimetape"}, InteriorSets = new List<string>() { "swap_clean_apt", "layer_debra_pic", "layer_whiskey", "swap_sofa_A","swap_mrJam_A" } },
            //new Interior(92674,"Darnell Bros. Garments",new List<string>() { "id2_14_during1","id2_14_during_door" },new List<string>() {"id2_14_during_door","id2_14_during1","id2_14_during2","id2_14_on_fire","id2_14_post_no_int","id2_14_pre_no_int" }),//top floor works and doors are open, but no interiror>?
            //new Interior(-103,"Rogers Salvage & Scrap",new List<string>() { "sp1_03_interior_v_recycle_milo_","sp1_03","sp1_03_critical_0","sp1_03_grass_0","sp1_03_long_0","sp1_03_strm_0","sp1_03_strm_1" },new List<string>() { }),//doesnt work at all, does nothing
            new Interior(-103,"Rogers Salvage & Scrap",new List<string>() { "v_recycle" },new List<string>() { }) { 

               

                Doors = new List<InteriorDoor>() { 
                    new InteriorDoor(812467272,new Vector3(-589.5225f, -1621.513f, 33.16225f)),//Inside
                    new InteriorDoor(812467272,new Vector3(-590.8179f, -1621.425f, 33.16282f)),//Inside

                    new InteriorDoor(2667367614,new Vector3(-611.32f, -1610.089f, 27.15894f)),//Front Outside R
                    new InteriorDoor(1099436502,new Vector3(-608.7289f, -1610.315f, 27.15894f)),//Front Ouitside L

                    new InteriorDoor(2667367614,new Vector3(-592.7109f, -1628.986f, 27.15931f)),//Rear Outside R
                    new InteriorDoor(1099436502,new Vector3(-592.9376f, -1631.577f, 27.15931f)),//Rear Ouitside L
                    //doors dont stay open

                }, 
                InternalInteriorCoordinates = new Vector3(-611.4f, -1615.7f, 29.2f), 
                NeedsActivation = true,
            },






            //.

            //Old Generic Stuff, i dont think we are loading any of this 
            new Interior(25090,"Mission Carpark"),// { IsSPOnly = true } ,
            new Interior(39682,"Torture Room"),// { IsSPOnly = true } ,
            //new Interior(76290,"Dell Perro Heights, Apt 4") { IsSPOnly = true } ,
            //new Interior(108290,"Low End Apartment") { IsSPOnly = true } ,
            new Interior(69890,"IAA Office"),// { IsSPOnly = true } ,
           // new Interior(25602,"Dell Perro Heights, Apt 7") { IsSPOnly = true } ,
            new Interior(31490,"FIB Floor 47"),// { IsSPOnly = true } ,
            new Interior(135973,"FIB Floor 49"),// { IsSPOnly = true } ,
            //new Interior(60162,"Motel") { IsSPOnly = true } ,
            new Interior(69122,"Lester's House"),// { IsSPOnly = true } ,
            //new Interior(47362,"4 Integrity Way, Apt 30") { IsSPOnly = true } ,
            new Interior(28418,"FIB Top Floor"),// { IsSPOnly = true } ,
            new Interior(70146,"10 Car Garage"),// { IsSPOnly = true } ,
            new Interior(85250,"Omega's Garage"),// { IsSPOnly = true } ,
            //new Interior(61186,"Eclipse Towers, Apt 3") { IsSPOnly = true } ,
            new Interior(94722,"Booking Room")// { IsSPOnly = true }
            ,new Interior(146433, "10 Car")// { IsMPOnly = true }
           // ,new Interior(149761, "Low End Apartment") { IsMPOnly = true }
           // ,new Interior(141313, "4 Integrity Way, Apt 30") { IsMPOnly = true }
           // ,new Interior(145921, "Dell Perro Heights, Apt 4") { IsMPOnly = true }
            //,new Interior(145665, "Dell Perro Heights, Apt 7") { IsMPOnly = true }
           // ,new Interior(146945, "Eclipse Towers, Apt 3") { IsMPOnly = true }
            ,new Interior(94722, "CharCreator")// { IsMPOnly = true }
            ,new Interior(25090, "Mission Carpark")// { IsMPOnly = true }
            ,new Interior(156929, "Torture Room")// { IsMPOnly = true }
            ,new Interior(178433, "Omega's Garage")// { IsMPOnly = true }
           // ,new Interior(149505, "Motel") { IsMPOnly = true }
            ,new Interior(250881, "Lester's House")// { IsMPOnly = true }
            ,new Interior(136449, "FBI Top Floor")// { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 47")// { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 49")// { IsMPOnly = true }
            ,new Interior(135681, "IAA Office")// { IsMPOnly = true }

            ,new Interior(149249, "2 Car")// { IsMPOnly = true }
            ,new Interior(148737, "6 Car")// { IsMPOnly = true }
         //  ,new Interior(148225, "Medium End Apartment") { IsMPOnly = true }
            //,new Interior(147201, "4 Integrity Way, Apt 28") { IsMPOnly = true }
          // ,new Interior(146177, "Richard Majestic, Apt 2") { IsMPOnly = true }
            //,new Interior(146689, "Tinsel Towers, Apt 42") { IsMPOnly = true }
          //  ,new Interior(207105, "3655 Wild Oats Drive") { IsMPOnly = true }
          //  ,new Interior(206081, "2044 North Conker Avenue") { IsMPOnly = true }
          //  ,new Interior(206337, "2045 North Conker Avenue") { IsMPOnly = true }
          //  ,new Interior(208129, "2862 Hillcrest Avenue") { IsMPOnly = true }
           // ,new Interior(207617, "2868 Hillcrest Avenue") { IsMPOnly = true }
           // ,new Interior(207361, "2874 Hillcrest Avenue") { IsMPOnly = true }
           // ,new Interior(206593, "2677 Whispymound Drive") { IsMPOnly = true }
          //  ,new Interior(208385, "2133 Mad Wayne Thunder") { IsMPOnly = true }
            ,new Interior(258561, "Bunker Interior")// { IsMPOnly = true }
            ,new Interior(164865, "Solomon's Office")// { IsMPOnly = true }
            ,new Interior(170497, "Psychiatrist's Office")// { IsMPOnly = true }
            ,new Interior(165889, "Movie Theatre")// { IsMPOnly = true }
            //,new Interior(205825, "Madrazos Ranch") { IsMPOnly = true }
            ,new Interior(260353, "Smuggler's Run Hangar")// { IsMPOnly = true }
            ,new Interior(262145, "Avenger Interior")// { IsMPOnly = true }
            ,new Interior(269313, "Facility")// { IsMPOnly = true }
            ,new Interior(270337, "Server Farm")// { IsMPOnly = true }
            ,new Interior(271105, "Submarine")// { IsMPOnly = true }
            ,new Interior(270081, "IAA Facility")// { IsMPOnly = true }
            ,new Interior(271617, "Nightclub")// { IsMPOnly = true }
            ,new Interior(271873, "Nightclub Warehouse")// { IsMPOnly = true }
            ,new Interior(272129, "Terrorbyte Interior")// { IsMPOnly = true }
        });
    }
}

