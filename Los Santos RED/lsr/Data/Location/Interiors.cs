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
                //Doors = new List<InteriorDoor>() 
                //{
                //    new InteriorDoor(3262795659,new Vector3(1390.666f, 1131.117f, 114.4808f)),
                //},
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
                    new AnimationInteract("lostmxclubhouseUrinal1",new Vector3(981.5935f, -98.13846f, 74.97108f), 222.2108f,"Use Urinal") 
                    { 
                        IsScenario = false,
                        LoopAnimations = new List<AnimationBundle>() 
                        { 
                            new AnimationBundle("missbigscore1switch_trevor_piss", "piss_loop", (int)(eAnimationFlags.AF_LOOPING), 2.0f, -2.0f) { Gender = "U" } 
                        },
                        CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f), 
                        CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f), 
                        CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    },

                    //new AnimationInteract("lostmxclubhouseToiletl1",new Vector3(979.7599f, -98.66194f, 74.845f), 132.1026f,"Use Toilet")
                    //{
                    //    IsScenario = true,
                    //    CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f),
                    //    CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f),
                    //    CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    //},
                    //new AnimationInteract("lostmxclubhouseToiletl2",new Vector3(980.5167f, -99.33208f, 74.84503f), 135.7255f,"Use Toilet")
                    //{
                    //    IsScenario = true,
                    //    CameraPosition = new Vector3(982.1099f, -96.15959f, 75.51414f),
                    //    CameraDirection = new Vector3(-0.4386229f, -0.8754325f, -0.2030466f),
                    //    CameraRotation = new Rotator(-11.71518f, -7.411464E-06f, 153.3875f),
                    //},


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
            new ResidenceInterior(60162,"Apartment") {//Motel Interior
                IsTeleportEntry = true,
                InteriorEgressPosition = new Vector3(151.817f, -1006.616f, -98.99998f),
                InteriorEgressHeading = 340.2548f,
                InteractPoints = new List<InteriorInteract>(){
                    new ExitInteriorInteract("motelExit1",new Vector3(151.47f, -1007.435f, -98.99998f), 168.4321f ,"Exit"),
                    new AnimationInteract("moteltoilet1",new Vector3(154.5553f, -1001.159f, -98.99998f), 267.9796f,"Use Toilet") {
                        UseNavmesh = false,
                        IsScenario = false,
                        LoopAnimations = new List<AnimationBundle>()
                        {
                            new AnimationBundle("missbigscore1switch_trevor_piss", "piss_loop", (int)(eAnimationFlags.AF_LOOPING), 2.0f, -2.0f) { Gender = "U" }
                        },
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
                    new AnimationInteract("mediumaptToilet1",new Vector3(294.9899f, -992.6285f, -98.99982f), 137.3274f,"Use Toilet") {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("4intapt28Toilet1",new Vector3(-19.51501f, -597.6929f, 94.02557f), 243.7846f,"Use Toilet") {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("4intapt30Toilet1",new Vector3(-28.41244f, -585.5016f, 83.90755f), 312.7336f,"Use Toilet") {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("dpheightApt4Toilet1",new Vector3(-1460.642f, -530.532f, 63.34938f), 2.485797f,"Use Toilet") 
                    {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("dpheightApt7Toilet1",new Vector3(-1460.75f, -530.77f, 50.73058f), 348.9325f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        IsScenario = true,
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
                IsSPOnly = true,
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
                    new AnimationInteract()
                    {
                        IsScenario = true,
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
                IsMPOnly = true,
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
                    new AnimationInteract()
                    {
                        IsScenario = true,
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
                    new AnimationInteract("richMaj2Toilet1",new Vector3(-918.4091f, -376.0463f, 103.2419f), 344.8122f ,"Use Toilet")
                    {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("tinselApt42Toilet1",new Vector3(-608.015f, 58.23079f, 101.8285f), 313.6327f,"Use Toilet")
                    {
                        UseNavmesh = false,
                        IsScenario = true,
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
                    new AnimationInteract("gomaToilet1",new Vector3(-1148.619f, -1519.637f, 10.63273f), 133.3349f ,"Use Toilet")
                    {
                        UseNavmesh = false,
                        IsScenario = true,
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
        });




        //new ResidenceInterior(61186,"Eclipse Towers, Apt 3") {
        //    IsTeleportEntry = true,
        //    InteriorEgressPosition = new Vector3(-779.4249f, 339.4756f, 207.6208f),
        //    InteriorEgressHeading = 113.7695f,
        //    InteractPoints = new List<InteriorInteract>(){
        //        new ExitInteriorInteract("ecliApt3Exit1",new Vector3(-777.7397f, 340.0569f, 207.6208f), 271.6255f,"Exit") ,
        //        new StandardInteriorInteract("ecliApt3Std1",new Vector3(-792.1093f, 338.2278f, 206.2184f),11.25924f,"Interact")
        //        {
        //            CameraPosition = new Vector3(-795.7065f, 332.019f, 208.7504f),
        //            CameraDirection = new Vector3(0.3429281f, 0.8894085f, -0.3022463f),
        //            CameraRotation = new Rotator(-17.59257f, -4.478318E-07f, -21.08508f),
        //        },
        //        new AnimationInteract("ecliApt3Toilet1",new Vector3(-785.7243f, 333.1788f, 201.4215f), 317.1673f,"Use Toilet")
        //        {
        //            UseNavmesh = false,
        //            IsScenario = true,
        //            CameraPosition = new Vector3(-788.5071f, 332.3035f, 202.3353f), 
        //            CameraDirection = new Vector3(0.9028393f, 0.3129103f, -0.2949041f), 
        //            CameraRotation = new Rotator(-17.15178f, -1.20624E-05f, -70.88447f)
        //        },
        //    },
        //    OutfitInteracts = new List<OutfitInteract>()
        //    {
        //        new OutfitInteract("ecliApt3Outfit1",new Vector3(-794.5327f, 331.9233f, 201.4244f), 268.0685f ,"Change Outfit")
        //        {
        //            CameraPosition = new Vector3(-790.35f, 331.6266f, 202.6446f), 
        //            CameraDirection = new Vector3(-0.9646187f, 0.0514039f, -0.2585893f), 
        //            CameraRotation = new Rotator(-14.98637f, -1.160034E-06f, 86.94963f)
        //        },
        //    },
        //    InventoryInteracts = new List<InventoryInteract>()
        //    {
        //        new InventoryInteract("ecliApt3Inventory1",new Vector3(-788.9904f, 327.2951f, 206.2184f), 177.0418f ,"Access Items")
        //        {
        //            CanAccessCash = false,
        //            CanAccessWeapons = false,
        //            CameraPosition = new Vector3(-792.4791f, 329.7375f, 207.2037f), 
        //            CameraDirection = new Vector3(0.6938594f, -0.7080619f, -0.1311772f), 
        //            CameraRotation = new Rotator(-7.537621f, -1.464066E-05f, -135.5804f)
        //        },
        //        new InventoryInteract("ecliApt3Inventory2",new Vector3(-801.4796f, 326.6167f, 206.2184f), 178.795f,"Access Cash/Weapons")
        //        {
        //            CanAccessItems = false,
        //            CameraPosition = new Vector3(-797.5353f, 329.2945f, 207.1171f),
        //            CameraDirection = new Vector3(-0.8358095f, -0.5216284f, -0.1712493f),
        //            CameraRotation = new Rotator(-9.860467f, -1.646492E-05f, 121.9683f)
        //        }
        //    },
        //    RestInteracts = new List<RestInteract>()
        //    {
        //        new RestInteract("ecliApt3Rest1",new Vector3(-796.3264f, 336.6161f, 201.4135f), 358.6736f,"Sleep")
        //        {
        //            CameraPosition = new Vector3(-792.834f, 335.8653f, 202.1105f), 
        //            CameraDirection = new Vector3(-0.6058555f, 0.7734321f, -0.1863919f),
        //            CameraRotation = new Rotator(-10.74229f, -9.993527E-06f, 38.0728f)
        //        },
        //    },
        //},



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
            new Interior(25090,"Mission Carpark") { IsSPOnly = true } ,
            new Interior(39682,"Torture Room") { IsSPOnly = true } ,
            //new Interior(76290,"Dell Perro Heights, Apt 4") { IsSPOnly = true } ,
            //new Interior(108290,"Low End Apartment") { IsSPOnly = true } ,
            new Interior(69890,"IAA Office") { IsSPOnly = true } ,
           // new Interior(25602,"Dell Perro Heights, Apt 7") { IsSPOnly = true } ,
            new Interior(31490,"FIB Floor 47") { IsSPOnly = true } ,
            new Interior(135973,"FIB Floor 49") { IsSPOnly = true } ,
            //new Interior(60162,"Motel") { IsSPOnly = true } ,
            new Interior(69122,"Lester's House") { IsSPOnly = true } ,
            //new Interior(47362,"4 Integrity Way, Apt 30") { IsSPOnly = true } ,
            new Interior(28418,"FIB Top Floor") { IsSPOnly = true } ,
            new Interior(70146,"10 Car Garage") { IsSPOnly = true } ,
            new Interior(85250,"Omega's Garage") { IsSPOnly = true } ,
            //new Interior(61186,"Eclipse Towers, Apt 3") { IsSPOnly = true } ,
            new Interior(94722,"Booking Room") { IsSPOnly = true }
            ,new Interior(146433, "10 Car") { IsMPOnly = true }
           // ,new Interior(149761, "Low End Apartment") { IsMPOnly = true }
           // ,new Interior(141313, "4 Integrity Way, Apt 30") { IsMPOnly = true }
           // ,new Interior(145921, "Dell Perro Heights, Apt 4") { IsMPOnly = true }
            //,new Interior(145665, "Dell Perro Heights, Apt 7") { IsMPOnly = true }
           // ,new Interior(146945, "Eclipse Towers, Apt 3") { IsMPOnly = true }
            ,new Interior(94722, "CharCreator") { IsMPOnly = true }
            ,new Interior(25090, "Mission Carpark") { IsMPOnly = true }
            ,new Interior(156929, "Torture Room") { IsMPOnly = true }
            ,new Interior(178433, "Omega's Garage") { IsMPOnly = true }
           // ,new Interior(149505, "Motel") { IsMPOnly = true }
            ,new Interior(250881, "Lester's House") { IsMPOnly = true }
            ,new Interior(136449, "FBI Top Floor") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 47") { IsMPOnly = true }
            ,new Interior(135937, "FBI Floor 49") { IsMPOnly = true }
            ,new Interior(135681, "IAA Office") { IsMPOnly = true }

            ,new Interior(149249, "2 Car") { IsMPOnly = true }
            ,new Interior(148737, "6 Car") { IsMPOnly = true }
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
            ,new Interior(258561, "Bunker Interior") { IsMPOnly = true }
            ,new Interior(164865, "Solomon's Office") { IsMPOnly = true }
            ,new Interior(170497, "Psychiatrist's Office") { IsMPOnly = true }
            ,new Interior(165889, "Movie Theatre") { IsMPOnly = true }
            //,new Interior(205825, "Madrazos Ranch") { IsMPOnly = true }
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

