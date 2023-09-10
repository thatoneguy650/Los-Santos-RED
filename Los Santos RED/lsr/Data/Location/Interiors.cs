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
                    new InteriorDoor(3082015943, new Vector3(-1226.894f,-903.1218f,12.47039f)),
                }),
            new Interior(19202,"Rob's Liquors",//Route 1
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-2973.535f,390.1414f,15.18735f)),
                   // new InteriorDoor(-1212951353, new Vector3(-2973.535f, 390.1414f, 15.18735f)),
                    //new InteriorDoor(1173348778, new Vector3(-2965.648f, 386.7928f, 15.18735f)),
                    //new InteriorDoor(1173348778, new Vector3(-2961.749f, 390.2573f, 15.19322f)),
                }),
            new Interior(98818,"Rob's Liquors",//Prosperity Street
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1490.411f,-383.8453f,40.30745f)),
                    //new InteriorDoor(-1212951353, new Vector3(-1490.411f, -383.8453f, 40.30745f)),
                    //new InteriorDoor(1173348778, new Vector3(-1482.679f, -380.153f, 40.30745f)),
                    //new InteriorDoor(1173348778, new Vector3(-1482.693f, -374.9365f, 40.31332f)),
                }),
            new Interior(73986,"Rob's Liquors",//El Rancho Boulevard
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(1141.038f,-980.3225f,46.55986f)),
                    //new InteriorDoor(-1212951353, new Vector3(1141.038f, -980.3225f, 46.55986f)),
                    //new InteriorDoor(1173348778, new Vector3(1132.645f, -978.6059f, 46.55986f)),
                    //new InteriorDoor(1173348778, new Vector3(1129.51f, -982.7756f, 46.56573f)),
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
            new Interior(71682,"Fleeca Bank"),
            new Interior(76802,"Fleeca Bank"),
            new Interior(11266,"Fleeca Bank"),
            new Interior(20226,"Fleeca Bank"),
            new Interior(90626,"Fleeca Bank"),
            new Interior(87810,"Fleeca Bank"),
            new Interior(103170,"Pacific Standard Bank") {
                Doors =  new List<InteriorDoor>() {

                } },


            new Interior(42754,"Blaine County Savings") {
                Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(3110375179, new Vector3(-108.9147f,6469.105f,31.91028f)),//teller
                    new InteriorDoor(2628496933, new Vector3(-109.65f,6462.11f,31.98499f)),//FRONT 1
                    new InteriorDoor(3941780146, new Vector3(-111.48f,6463.94f,31.98499f)),//FRONT 2
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

