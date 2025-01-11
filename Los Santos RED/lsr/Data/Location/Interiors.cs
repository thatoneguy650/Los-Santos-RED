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
            DefaultConfig_LibertyCity();
        }
    }
    private void DefaultConfig_LibertyCity()
    {
        Interiors_Liberty interiors_Liberty = new Interiors_Liberty(this);
        interiors_Liberty.DefaultConfig();
    }
    private void DefaultConfig()
    {
        Stores();
        Tunnels();
        Stations();
        Other();
        Residence();
        GangDens();
        Banks();
        BarberShops();
        ClothingShops();
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
    private void BarberShops()
    {
        PossibleInteriors.BarberShopInteriors.AddRange(
            new List<BarberShopInterior>()
            {
                new BarberShopInterior(-99099,"Barber Shop")
                {
                    IsTeleportEntry = true,

                    ForceAutoInteractName = "genericheaircutinteract1",
                    InteriorEgressPosition = new Vector3(123.7631f, -745.737f, 242.152f),
                    InteriorEgressHeading = 216.5336f,
                    PropSpawns = new List<PropSpawn>() { new PropSpawn("vw_prop_casino_track_chair_01", new SpawnPlace(new Vector3(124.8145f, -747.5364f, 242.152f), 69.98466f)) {PlaceOnGround = true } },
                    VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(122.921f, -748.2304f, 241.652f), 305.9934f) },
                    HaircutInteracts = new List<SalonInteract>() 
                    {
                        new SalonInteract("genericheaircutinteract1",new Vector3(125.0691f, -743.9323f, 242.1519f),216.5336f,"Get Haircut") 
                        {
                            IsAutoInteract = true,
                            //WaitForCameraReturn = false,
                            //ForceIsntantCamera = true,
                            //DoFadeOutAndFadeIn = true,
                            AnimEnterPosition = new Vector3(124.8145f, -747.5364f, 241.152f),
                            AnimEnterRotation = new Vector3(0f, 0f, -388.969f),
                            CameraPosition = new Vector3(125.9692f, -748.0427f, 242.6188f),
                            CameraDirection = new Vector3(-0.8899927f, 0.4485282f, -0.08206987f),
                            CameraRotation = new Rotator(-4.707552f, 4.283317E-06f, 63.25334f),
                        }
                    },
                },



                new BarberShopInterior(13058,"Herr Kutz Paleto")
                { 
                    HaircutInteracts = new List<SalonInteract>() { 
                        new SalonInteract("herrkutzpaletointeract1",new Vector3(-276.8453f, 6225.595f, 31.69551f), 120.1309f,"Get Haircut") {
                            AnimEnterPosition = new Vector3(-278.348f,6225.873f,30.93535f), 
                            AnimEnterRotation = new Vector3(0f, 0f, -138.969f),
                            CameraPosition = new Vector3(-279.3672f, 6225.189f, 32.20325f), 
                            CameraDirection = new Vector3(0.869201f, 0.4895405f, -0.06956866f), 
                            CameraRotation = new Rotator(-3.989213f, -8.558471E-07f, -60.61147f),
                        } 
                    }, 
                    Doors = new List<InteriorDoor>() 
                    { 
                        new InteriorDoor(2450522579, new Vector3(-280.7851f, 6232.782f, 31.84548f)) { NeedsDefaultUnlock = true,LockWhenClosed = true } 
                    } 
                },

                new BarberShopInterior(113922,"Beach Combover Vespucci")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("beachcombvespucciinteract1",new Vector3(-1280.101f, -1117.949f, 6.50118f), 152.0002f,"Get Haircut") 
                        {
                            AnimEnterPosition = new Vector3(-1281.245f,-1118.945f,6.240114f),
                            AnimEnterRotation = new Vector3(0f, 0f, -93.969f),//new Vector3(0f,0f,-453.969f);
                            CameraPosition = new Vector3(-1281.438f, -1120.25f, 7.630449f), 
                            CameraDirection = new Vector3(0.2015185f, 0.9790071f, -0.03058454f), 
                            CameraRotation = new Rotator(-1.752638f, 1.929898E-05f, -11.63129f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2450522579, new Vector3(-1287.857f, -1115.742f, 7.140073f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },
                new BarberShopInterior(37378,"Bob Mulet Rockford Hills")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("bobmuletrockfordinteract1",new Vector3(-817.1225f, -185.375f, 37.56889f), 343.7052f,"Get Haircut")
                        {
                            AnimEnterPosition = new Vector3(-816.22f,-182.97f,36.67f),
                            AnimEnterRotation = new Vector3(0f,0f,-238.969f),
                            CameraPosition = new Vector3(-816.9109f, -181.7749f, 38.01262f), 
                            CameraDirection = new Vector3(0.4810959f, -0.8718764f, -0.09153295f), 
                            CameraRotation = new Rotator(-5.251803f, 2.636421E-05f, -151.1104f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2631455204,new Vector3(-823.2001f, -187.0831f, 37.81895f)) { NeedsDefaultUnlock = true,LockWhenClosed = true },
                        new InteriorDoor(145369505,new Vector3(-822.4442f, -188.3924f, 37.81895f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },
                new BarberShopInterior(102146,"Herr Kutz Davis")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("herrkutzdavisinteract1",new Vector3(139.1199f, -1706.163f, 29.29162f), 212.893f,"Get Haircut") {
                            AnimEnterPosition = new Vector3(139.2859f,-1708.033f,28.48875f),
                            AnimEnterRotation = new Vector3(0f,0f,-43.969f),//new Vector3(0f,0f,-403.969f),
                            CameraPosition = new Vector3(140.1534f, -1709.204f, 29.92947f), 
                            CameraDirection = new Vector3(-0.4719426f, 0.880088f, -0.05210888f), 
                            CameraRotation = new Rotator(-2.986972f, 8.015016E-06f, 28.20221f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2450522579,new Vector3(132.5569f, -1710.996f, 29.44157f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },
                new BarberShopInterior(112642,"Herr Kutz Mirror Park")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("herrkutzmirrorinteract1",new Vector3(1215.068f, -474.1496f, 66.20802f), 135.4196f,"Get Haircut") {
                            AnimEnterPosition = new Vector3(1213.349f,-474.8473f,65.45633f),
                            AnimEnterRotation = new Vector3(0f, 0f, -93.969f),//new Vector3(0f,0f,-453.969f);
                            CameraPosition = new Vector3(1213.009f, -476.0569f, 66.90621f), 
                            CameraDirection = new Vector3(0.3722664f, 0.9256455f, -0.06780995f), 
                            CameraRotation = new Rotator(-3.888207f, -1.465461E-05f, -21.90849f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2450522579,new Vector3(1207.873f, -470.0363f, 66.358f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },
                new BarberShopInterior(10242,"O'Sheas Sandy Shores")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("osheassandyinteract1",new Vector3(1930.365f, 3732.802f, 32.84443f), 264.8277f,"Get Haircut") {
                            AnimEnterPosition = new Vector3(1932.398f,3732.411f,32.08461f),
                            AnimEnterRotation = new Vector3(0f,0f,-318.969f),
                             CameraPosition = new Vector3(1933.77f, 3733.082f, 33.58648f), 
                            CameraDirection = new Vector3(-0.9403489f, -0.3225069f, -0.1083205f), 
                            CameraRotation = new Rotator(-6.218508f, 2.361774E-06f, 108.9302f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2450522579,new Vector3(1932.952f, 3725.154f, 32.9944f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },

                new BarberShopInterior(34306,"Hair on Hawick")
                {
                    HaircutInteracts = new List<SalonInteract>() {
                        new SalonInteract("haironhawickinteract1",new Vector3(-34.39177f, -154.897f, 57.07654f), 38.9985f,"Get Haircut") {
                            AnimEnterPosition = new Vector3(-35.08471f,-153.2957f,56.32422f),
                            AnimEnterRotation = new Vector3(0f,0f,-198.969f),
                            CameraPosition = new Vector3(-36.2537f, -152.7489f, 57.84624f),
                            CameraDirection = new Vector3(0.8691139f, -0.4798547f, -0.1199191f), 
                            CameraRotation = new Rotator(-6.887431f, 6.449846E-06f, -118.9039f),
                        }
                    },
                    Doors = new List<InteriorDoor>()
                    {
                        new InteriorDoor(2450522579,new Vector3(-29.86917f, -148.1571f, 57.22648f)) { NeedsDefaultUnlock = true,LockWhenClosed = true }
                    }
                },
            }
        );;
    }
    private void Stores()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            //Barber
            

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
                    new InteriorDoor(997554217, new Vector3(542.9252f,2672.406f,42.30644f))}) //right door
            { 
                
                IsWeaponRestricted = true, 
                InteractPoints = new List<InteriorInteract>()
                {
                    
                    Generate247Interact1("247rt68itemtheftint1",new Vector3(547.77f, 2669.75f, 42.15649f), 272.6136f),
                    Generate247Interact2("247rt68itemtheftint2",new Vector3(546.6794f, 2668.725f, 42.15649f), 88.89041f),
                    Generate247Interact3("247rt68itemtheftint3",new Vector3(544.7056f, 2668.834f, 42.15654f), 269.0896f),
                    Generate247Interact4("247rt68itemtheftint4",new Vector3(541.7424f, 2668.305f, 42.15653f), 281.1041f),
                    new MoneyTheftInteract("247rt68moneytheft1",new Vector3(546.2946f, 2663.203f, 42.1565f), 189.7516f,"Steal from Safe")//only here for example
                    {
                        CashMinAmount = 250,
                        CashMaxAmount = 2500,
                        IncrementGameTimeMin = 1500,
                        IncrementGameTimeMax = 2000,
                        CashGainedPerIncrement = 250,
                        GameTimeBeforeInitialReward = 3500,
                        ViolatingCrimeID = StaticStrings.TheftCrimeID,
                        CameraPosition = new Vector3(548.8856f, 2664.482f, 43.11457f), CameraDirection = new Vector3(-0.8029377f, -0.4896093f, -0.3399612f), CameraRotation = new Rotator(-19.87451f, 5.447072E-06f, 121.3737f),
                        IntroAnimationDictionary = "anim@amb@business@weed@weed_inspecting_lo_med_hi@",
                        IntroAnimation = "weed_spraybottle_stand_kneeling_01_inspector",
                        LoopAnimationDictionary = "anim@amb@business@weed@weed_inspecting_lo_med_hi@",
                        LoopAnimation = "weed_spraybottle_stand_kneeling_01_inspector",
                    },
                   //new MoneyTheftInteract("247rt68moneytheft2",new Vector3(546.5319f, 2671.772f, 42.15653f), 355.2674f,"Steal Cash")//testing only
                   //{
                   //     CashMinAmount = 250,
                   //     CashMaxAmount = 2500,
                   //     IncrementGameTimeMin = 1500,
                   //     IncrementGameTimeMax = 2000,
                   //     CashGainedPerIncrement = 250,
                   //     ViolatingCrimeID = StaticStrings.TheftCrimeID,
                   //},
                }
            },
            new Interior(16386,"24/7 Chumash",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3240.128f,1003.157f,12.98064f)),//left door
                    new InteriorDoor(997554217, new Vector3(-3239.905f,1005.749f,12.98064f))})//right door 
            { 
                IsWeaponRestricted = true, 
                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247chumashsafe1",new Vector3(-3250.059f,1004.447f,12.83072f),81.94628f),
                    Generate247Interact1("247chumashitemtheftint1",new Vector3(-3243.837f, 1001.816f, 12.83072f), 173.4079f),
                    Generate247Interact2("247chumashitemtheftint2",new Vector3(-3244.236f, 1002.773f, 12.83072f), 349.7097f),
                    Generate247Interact3("247chumashitemtheftint3",new Vector3(-3244.103f, 1004.807f, 12.83072f), 171.435f),
                    Generate247Interact4("247chumashitemtheftint4",new Vector3(-3243.798f, 1007.824f, 12.83071f), 171.7916f),
                },
            },
            new Interior(62722,"24/7 Palomino Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2559.201f,384.0875f,108.7729f)),
                    new InteriorDoor(997554217, new Vector3(2559.304f,386.6865f,108.7729f)),}) { IsWeaponRestricted = true,

                            InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247palominosafe1",new Vector3(2549.198f,384.9543f,108.6229f),90.04411f),
                    Generate247Interact1("247palominoitemtheftint1",new Vector3(2555.472f, 382.472f, 108.623f), 177.8884f),
                    Generate247Interact2("247palominoitemtheftint2",new Vector3(2555.143f, 383.8918f, 108.623f), 354.7713f),
                    Generate247Interact3("247palominoitemtheftint3",new Vector3(2555.162f, 385.2853f, 108.623f), 173.1648f),
                    Generate247Interact4("247palominoitemtheftint4",new Vector3(2555.455f, 388.542f, 108.623f), 177.5899f),
                },

            },//right door
            new Interior(33282,"24/7 Strawberry",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(27.81761f,-1349.169f,29.64696f)),
                    new InteriorDoor(997554217, new Vector3(30.4186f,-1349.169f,29.64696f)),}) { IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247strawberrysafe1",new Vector3(28.21077f,-1339.231f,29.49702f),358.4373f),
                    Generate247Interact1("247strawberryitemtheftint1",new Vector3(26.20215f, -1345.724f, 29.49702f), 84.35592f),
                    Generate247Interact2("247strawberryitemtheftint2",new Vector3(27.30392f, -1345.065f, 29.49702f), 255.6686f),
                    Generate247Interact3("247strawberryitemtheftint3",new Vector3(29.2087f, -1344.919f, 29.49702f), 88.0149f),
                    Generate247Interact4("247strawberryitemtheftint4",new Vector3(31.93349f, -1345.247f, 29.49702f), 86.14433f),
                },
            },//right door
            new Interior(97538,"24/7 Banham Canyon",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(-3038.219f,588.2872f,8.058861f)),
                    new InteriorDoor(997554217, new Vector3(-3039.012f,590.7643f,8.058861f)),}) { IsWeaponRestricted = true,
                    InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247banhamsafe1",new Vector3(-3047.899f,585.6166f,7.908929f),107.6895f),
                    Generate247Interact1("247Banhamitemtheftint1",new Vector3(-3041.139f, 585.6979f, 7.908929f), 196.5549f),
                    Generate247Interact2("247Banhamitemtheftint2",new Vector3(-3041.866f, 586.2217f, 7.908929f), 19.2346f),
                    Generate247Interact3("247Banhamitemtheftint3",new Vector3(-3042.159f, 588.2862f, 7.908929f), 196.0548f),
                    Generate247Interact4("247Banhamitemtheftint4",new Vector3(-3043.282f, 591.1353f, 7.908931f), 181.0563f),
                },

            },//right door    
            new Interior(46850,"24/7 Downtown Vinewood",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(375.3528f,323.8015f,103.7163f)),
                    new InteriorDoor(997554217, new Vector3(377.8753f,323.1672f,103.7163f)),}) { IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247vinewoodsafe1",new Vector3(378.2333f,333.4276f,103.5664f),348.8413f),
                    Generate247Interact1("247vinewooditemtheftint1",new Vector3(374.461f, 327.4398f, 103.5665f), 70.37616f),
                    Generate247Interact2("247vinewooditemtheftint2",new Vector3(375.8289f, 327.4654f, 103.5665f), 255.7188f),
                    Generate247Interact3("247vinewooditemtheftint3",new Vector3(377.3474f, 327.4413f, 103.5665f), 60.62653f),
                    Generate247Interact4("247vinewooditemtheftint4",new Vector3(380.042f, 326.4352f, 103.5665f), 72.08177f),
                },
            },//right door   
            new Interior(36354,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1437777724, new Vector3(1732.245f,6415.377f,34.76194f)),
                    new InteriorDoor(1421582485, new Vector3(1734.097f,6413.048f,34.99545f)),}) { IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247Senorasafe1",new Vector3(2672.808f,3286.673f,55.24113f),59.95816f),
                    Generate247Interact1("247Senoraitemtheftint1",new Vector3(2677.432f, 3281.56f, 55.24113f), 146.3796f),
                    Generate247Interact2("247Senoraitemtheftint2",new Vector3(2677.385f, 3282.917f, 55.24113f), 317.2356f),
                    Generate247Interact3("247Senoraitemtheftint3",new Vector3(2678.458f, 3284.286f, 55.24113f), 144.8814f),
                    Generate247Interact4("247Senoraitemtheftint4",new Vector3(2679.751f, 3286.76f, 55.24113f), 139.366f),
                },
            },//right door   
            new Interior(13826,"24/7 Senora Freeway",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(2681.292f,3281.427f,55.39108f)),
                    new InteriorDoor(997554217, new Vector3(2682.558f,3283.698f,55.39108f)),}) { IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {

                    Generate247Interact1("247Senora2itemtheftint1",new Vector3(1729.814f, 6416.296f, 35.03724f), 54.17762f),
                    Generate247Interact2("247Senora2itemtheftint2",new Vector3(1731.211f, 6415.733f, 35.03724f), 235.11f),
                    Generate247Interact3("247Senora2itemtheftint3",new Vector3(1732.993f, 6414.77f, 35.03724f), 59.2864f),
                    Generate247Interact4("247Senora2itemtheftint4",new Vector3(1735.669f, 6413.408f, 35.03724f), 64.71087f),
                },

            },//right door          
            new Interior(55554,"24/7 Sandy Shores",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(1196685123, new Vector3(1963.917f,3740.075f,32.49369f)),
                    new InteriorDoor(997554217, new Vector3(1966.17f,3741.376f,32.49369f)),}) { IsWeaponRestricted = true,

                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("247sandysafe1",new Vector3(1959.353f,3748.935f,32.34374f),30.7123f),
                    Generate247Interact1("247sandyitemtheftint1",new Vector3(1960.4f, 3742.231f, 32.34375f), 107.836f),
                    Generate247Interact2("247sandyitemtheftint2",new Vector3(1961.382f, 3743.152f, 32.34375f), 287.01f),
                    Generate247Interact3("247sandyitemtheftint3",new Vector3(1962.828f, 3744.041f, 32.34375f), 117.9122f),
                    Generate247Interact4("247sandyitemtheftint4",new Vector3(1965.526f, 3745.193f, 32.34375f), 114.0811f),
                },
            },//right door 

            //LtD
            new Interior(47874, "Ltd Little Seoul",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-713.0732f,-916.5409f,19.36553f)),
                    new InteriorDoor(2065277225, new Vector3(-710.4722f,-916.5372f,19.36553f)),}){
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("ltdlittleseoulitemtheftint1",new Vector3(-707.8715f, -914.1517f, 19.21559f), 270.5069f),
                    Generate247Interact2("ltdlittleseoulitemtheftint2",new Vector3(-714.9484f, -912.2844f, 19.21559f), 47.72692f),
                    Generate247Interact3("ltdlittleseoulitemtheftint3",new Vector3(-714.4394f, -912.6227f, 19.21559f), 221.3954f),
                    Generate247Interact4("ltdlittleseoulitemtheftint4",new Vector3(-713.0032f, -911.1704f, 19.21559f), 223.4646f),
                },



            },// { IsSPOnly = true},//right door  
            new Interior(45570, "Ltd Grapeseed",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1699.661f,4930.278f,42.21359f)),
                    new InteriorDoor(2065277225, new Vector3(1698.172f,4928.146f,42.21359f)),})
            {
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("ltdgrapeseeditemtheftint1",new Vector3(1698.303f, 4924.868f, 42.06368f), 147.1322f),
                    Generate247Interact2("ltdgrapeseeditemtheftint2",new Vector3(1704.167f, 4929.214f, 42.06368f), 284.1616f),
                    Generate247Interact3("ltdgrapeseeditemtheftint3",new Vector3(1703.845f, 4929.148f, 42.06368f), 99.67239f),
                    Generate247Interact4("ltdgrapeseeditemtheftint4",new Vector3(1704.235f, 4927.141f, 42.06368f), 100.0045f),
                },

            },// { IsSPOnly = true},//right door  
            new Interior(80642,"LtD Davis",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-53.96111f,-1755.717f,29.57094f)),
                    new InteriorDoor(2065277225, new Vector3(-51.96669f,-1757.387f,29.57094f)),}) {


                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("ltdgrapeseeditemtheftint1",new Vector3(-48.92481f, -1757.641f, 29.42102f), 232.3135f),
                    Generate247Interact2("ltdgrapeseeditemtheftint2",new Vector3(-52.70221f, -1751.247f, 29.42102f), 10.09468f),
                    Generate247Interact3("ltdgrapeseeditemtheftint3",new Vector3(-52.58833f, -1751.656f, 29.42102f), 190.6157f),
                    Generate247Interact4("ltdgrapeseeditemtheftint4",new Vector3(-50.41463f, -1751.314f, 29.42102f), 184.5213f),
                },




            },//right door   
            new Interior(2050,"LtD Mirror Park",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(1158.364f,-326.8165f,69.35503f)),
                    new InteriorDoor(2065277225, new Vector3(1160.925f,-326.3612f,69.35503f)),}) { IsWeaponRestricted = true,

                                InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("ltdmirroritemtheftint1",new Vector3(1163.169f, -323.9579f, 69.20515f), 281.6205f),
                    Generate247Interact2("ltdmirroritemtheftint2",new Vector3(1155.778f, -323.0717f, 69.20515f), 57.65269f),
                    Generate247Interact3("ltdmirroritemtheftint3",new Vector3(1156.007f, -323.2287f, 69.20515f), 240.3528f),
                    Generate247Interact4("ltdmirroritemtheftint4",new Vector3(1157.43f, -321.1204f, 69.20515f), 233.2851f),
                },



            },//right door   
            new Interior(82178,"LtD Richman Glen",
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3426294393, new Vector3(-1823.285f, 787.3687f, 138.3624f)),
                    new InteriorDoor(2065277225, new Vector3(-1821.369f, 789.1273f, 138.3124f)),}) { IsWeaponRestricted = true, },//right door  
            new Interior(74874,"LtD Gas") { IsWeaponRestricted = true,

            InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("ltdrichamanitemtheftint1",new Vector3(-1821.235f, 792.0287f, 138.1301f), 310.7221f),
                    Generate247Interact2("ltdrichamanitemtheftint2",new Vector3(-1827.494f, 789.2473f, 138.2624f), 86.91869f),
                    Generate247Interact3("ltdrichamanitemtheftint3",new Vector3(-1827.215f, 789.2775f, 138.2572f), 273.1148f),
                    Generate247Interact4("ltdrichamanitemtheftint4",new Vector3(-1827.185f, 791.273f, 138.236f), 268.6499f),
                },




            },

            //Liquor
            new Interior(33026,"Scoops Liquor Barn") { IsWeaponRestricted = true,

            InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("scoopsitemtheft1",new Vector3(1166.253f, 2708.812f, 38.15771f), 359.1691f),
                    Generate247Interact4("scoopsitemtheft4",new Vector3(1166.231f, 2707.29f, 38.15771f), 95.36868f),
                },

            },                
            new Interior(104450,"Liquor Ace"){
            
            IsWeaponRestricted = true,
            InteractPoints = new List<InteriorInteract>()
                {
                    Generate247Interact1("liquoraceitemtheft1",new Vector3(1393.848f, 3604.695f, 34.98093f), 14.72468f),
                    Generate247Interact4("liquoraceitemtheft4",new Vector3(1390.842f, 3601.188f, 34.98093f), 107.446f),
                },

            },     
            new Interior(50178,"Rob's Liquors",//San Andreas Ave Del Perro
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1226.894f,-903.1218f,12.47039f)){ LockWhenClosed = true },
                }) { IsWeaponRestricted = true,

                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("robsdelperrosafe1",new Vector3(-1220.87f,-916.0239f,11.32633f),120.3279f),
                    Generate247Interact1("robsdelperroitemtheftint1",new Vector3(-1821.235f, 792.0287f, 138.1301f), 310.7221f),
                    Generate247Interact4("robsdelperroitemtheftint4",new Vector3(-1827.185f, 791.273f, 138.236f), 268.6499f),
                },




            },
            new Interior(19202,"Rob's Liquors",//Route 1
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-2973.535f,390.1414f,15.18735f)){ LockWhenClosed = true },
                }) { 
                IsWeaponRestricted = true,
                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("robschumashsafe1",new Vector3(-2959.66f,387.1028f,14.04329f),174.1468f),
                    Generate247Interact1("robschumashitemtheftint1",new Vector3(-2968.487f, 390.65f, 15.04331f), 267.1219f),
                    Generate247Interact4("robschumashitemtheftint4",new Vector3(-2970.053f, 390.9432f, 15.04331f), 357.4018f),
                },

            },
            new Interior(98818,"Rob's Liquors",//Prosperity Street
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(-1490.411f,-383.8453f,40.30745f)){ LockWhenClosed = true },
                }) { IsWeaponRestricted = true,

                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("robsprosperitysafe1",new Vector3(-1478.886f,-375.4599f,39.1634f),225.3352f),
                    Generate247Interact1("robsprosperityitemtheftint1",new Vector3(-1487.278f, -379.4213f, 40.16343f), 315.5894f),
                    Generate247Interact4("robsprosperityitemtheftint4",new Vector3(-1488.674f, -380.564f, 40.16343f), 43.9887f),
                },



            },
            new Interior(73986,"Rob's Liquors",//El Rancho Boulevard
                new List<string>() {  },
                new List<string>() {  },
                new List<InteriorDoor>() {
                    new InteriorDoor(3082015943, new Vector3(1141.038f,-980.3225f,46.55986f)) { LockWhenClosed = true },
                }) { IsWeaponRestricted = true,

                InteractPoints = new List<InteriorInteract>()
                {
                    GenerateSafeDrillingInteract("robselranchosafe1",new Vector3(1126.804f,-980.1304f,45.41582f),2.577071f),
                    Generate247Interact1("robselranchoitemtheftint1",new Vector3(1136.005f, -981.7958f, 46.41584f), 104.7439f),
                    Generate247Interact4("robselranchoitemtheftint4",new Vector3(1137.729f, -981.8011f, 46.41584f), 190.8169f),
                },



            },

            //Barber/Tattoo      
            //new Interior(37378,"Bob Mullet Hair & Beauty"),
            //new Interior(113922,"Beachcombover Barber"),
            //new Interior(10242,"Hot Shave Barbers"),
            new Interior(49922,"Los Santos Tattoo"),
            new Interior(93442,"Los Santos Customs"),
            //new Interior(102146,"Herr Kutz Barber"),
            new Interior(35074,"The Pit"),//tattoo
            //Car Dealer
            new Interior(37890,"Los Santos Customs"),
            new Interior(7170, "Premium Deluxe Motorsport",new List<string>() { "shr_int" },new List<string>() { "fakeint" },new List<string>() { "shutter_open","csr_beforeMission" }),

            //Banks
            


            //103170
        });
    }
    private void Tunnels()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
            new Interior(96002,"Zancudo Tunnel") { IsTunnel = true, },
            new Interior(104706,"Zancudo Tunnel"){ IsTunnel = true, },
            new Interior(81154,"Braddock Tunnel"){ IsTunnel = true, },
            new Interior(40450,"Braddock Tunnel"){ IsTunnel = true, },
            new Interior(19714,"Braddock Tunnel"){ IsTunnel = true, },
            new Interior(7682,"Integrity Way Tunnel"){ IsTunnel = true, },
            new Interior(50946,"Integrity Way Tunnel"){ IsTunnel = true, },
            new Interior(75010,"Integrity Way Tunnel"){ IsTunnel = true, },


            new Interior(6146,"Del Perro Tunnel"){ IsTunnel = true, },
            new Interior(96770,"Del Perro Tunnel"){ IsTunnel = true, },
            new Interior(86274,"Del Perro Tunnel"){ IsTunnel = true, },
            new Interior(105218,"Del Perro Tunnel"){ IsTunnel = true, },



            new Interior(38658,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(100866,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(83202,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(110850,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(28674,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(49154,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(27650,"South LS Rail Tunnel"){ IsTunnel = true, },
            new Interior(101122,"South LS Rail Tunnel"){ IsTunnel = true, },


            new Interior(111362,"Del Perro Canal Access"){ IsTunnel = true, },
            new Interior(118530,"Del Perro Canal Access"){ IsTunnel = true, },
            new Interior(97282,"Del Perro Canal Access"){ IsTunnel = true, },
            new Interior(104194,"Del Perro Canal Access"){ IsTunnel = true, },
            new Interior(113410,"Del Perro Canal Access"){ IsTunnel = true, },


            new Interior(108802,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(112898,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(20994,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(29442,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(12034,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(16130,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(117762,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(5890,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(97026,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },
            new Interior(71170,"Raton Canyon Rail Tunnel"){ IsTunnel = true, },



            new Interior(14082,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(42242,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(45826,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(55810,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(57346,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(109826,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(10498,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(71938,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(43266,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(26114,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(9986,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(120322,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(36610,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(104962,"Downtown LS Sewer"){ IsTunnel = true, },
            new Interior(23298,"Downtown LS Sewer"){ IsTunnel = true, },


            new Interior(5634,"Burton Subway Station"){ IsTunnel = true, },
            new Interior(32770,"Burton Subway Station"){ IsTunnel = true, },
            new Interior(99842,"Burton Subway Station"){ IsTunnel = true, },



            new Interior(68866,"Little Seoul Subway Station"){ IsTunnel = true, },
            new Interior(20482,"Little Seoul Subway Station"){ IsTunnel = true, },
            new Interior(16898,"Little Seoul Subway Station"){ IsTunnel = true, },

            new Interior(98306,"Del Perro Subway Station"){ IsTunnel = true, },
            new Interior(5122,"Del Perro Subway Station"){ IsTunnel = true, },
            new Interior(32002,"Del Perro Subway Station"){ IsTunnel = true, },


            new Interior(84482,"Portola Drive Subway Station"){ IsTunnel = true, },
            new Interior(91650,"Portola Drive Subway Station"){ IsTunnel = true, },
            new Interior(50690,"Portola Drive Subway Station"){ IsTunnel = true, },

            new Interior(62466,"LSIA Terminal 4 Subway Station"){ IsTunnel = true, },
            new Interior(108546,"LSIA Terminal 4 Subway Station"){ IsTunnel = true, },
            new Interior(77314,"LSIA Terminal 4 Subway Station"){ IsTunnel = true, },

            new Interior(61698,"LSIA Parking Subway Station"){ IsTunnel = true, },
            new Interior(65538,"LSIA Parking Subway Station"){ IsTunnel = true, },
            new Interior(111106,"LSIA Parking Subway Station"){ IsTunnel = true, },

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
            new ResidenceInterior(76290,"Del Perro Heights, Apt 4") {
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
            new ResidenceInterior(-673,"Del Perro Heights, Apt 7") {
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
            new ResidenceInterior(90882,"Tinsel Towers, Apt 42") {
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
            new ResidenceInterior(55042,"Nice Medium Apartment") {
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

            //Additional MP Apartments
            new ResidenceInterior(146689, "Tinsel Towers, Apt 42") {
              RestInteracts =
                  new List<RestInteract>() {
                    new RestInteract() {
                      StartAnimations = new List<AnimationBundle>() {},
                      LoopAnimations = new List<AnimationBundle>() {},
                      EndAnimations = new List<AnimationBundle>() {},
                      Name = "TinTowApt42Rest1",
                      Position = new Vector3(-593.822f, 50.52116f, 96.99961f),
                      Heading = 176.4254f,
                      CameraPosition = new Vector3(-597.3605f, 50.58469f, 98.08853f),
                      CameraDirection = new Vector3(0.8128961f, -0.4623872f, -0.3541159f),
                      CameraRotation = new Rotator(-20.73927f, -6.39051E-06f, -119.6319f),
                      ButtonPromptText = "Sleep",
                    },
                  },
              InventoryInteracts =
                  new List<InventoryInteract>() {
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Fridge",
                      Description = "Access Drinks",
                      Name = "TinTowApt42Inventory1",
                      Position = new Vector3(-618.2627f, 47.29871f, 97.60003f),
                      Heading = 2.54048f,
                      CameraPosition = new Vector3(-614.1591f, 46.51549f, 98.38596f),
                      CameraDirection = new Vector3(-0.8522642f, 0.4205003f, -0.3111677f),
                      CameraRotation = new Rotator(-18.12962f, 2.919713E-05f, 63.73864f),
                      ButtonPromptText = "Open Fridge",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Pantry",
                      Description = "Access Food",
                      Name = "TinTowApt42Inventory2",
                      Position = new Vector3(-616.2643f, 47.27203f, 97.60003f),
                      Heading = 1.436783f,
                      CameraPosition = new Vector3(-614.1591f, 46.51549f, 98.38596f),
                      CameraDirection = new Vector3(-0.8522642f, 0.4205003f, -0.3111677f),
                      CameraRotation = new Rotator(-18.12962f, 2.919713E-05f, 63.73864f),
                      ButtonPromptText = "Open Pantry",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                          },
                      Title = "",
                      Description = "",
                      Name = "TinTowApt42Inventory3",
                      Position = new Vector3(-617.9466f, 43.27295f, 97.60003f),
                      Heading = 275.2648f,
                      CameraPosition = new Vector3(-618.663f, 41.41837f, 98.25163f),
                      CameraDirection = new Vector3(0.4123664f, 0.8497772f, -0.3283788f),
                      CameraRotation = new Rotator(-19.1704f, 0f, -25.88564f),
                      ButtonPromptText = "Access Items",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessWeapons = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt42Inventory4",
                      Position = new Vector3(-598.0397f, 49.18652f, 97.03484f),
                      Heading = 93.79581f,
                      CameraPosition = new Vector3(-597.3564f, 51.35877f, 97.98212f),
                      CameraDirection = new Vector3(-0.3865968f, -0.8350788f, -0.3913902f),
                      CameraRotation = new Rotator(-23.04103f, 3.432816E-05f, 155.1584f),
                      ButtonPromptText = "Cash Drawer",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt42Inventory5",
                      Position = new Vector3(-622.7729f, 55.91735f, 97.5995f),
                      Heading = 1.475779f,
                      CameraPosition = new Vector3(-624.7068f, 55.14439f, 98.29195f),
                      CameraDirection = new Vector3(0.8347888f, 0.4474139f, -0.3208558f),
                      CameraRotation = new Rotator(-18.71469f, -1.893012E-05f, -61.81045f),
                      ButtonPromptText = "Weapons Locker",
                      UseNavmesh = false,
                    },
                  },
              OutfitInteracts =
                  new List<OutfitInteract>() {
                    new OutfitInteract() {
                      Name = "TinTowApt42Outfit1",
                      Position = new Vector3(-594.6447f, 56.11897f, 96.99954f),
                      Heading = 173.7917f,
                      CameraPosition = new Vector3(-594.6223f, 53.37143f, 97.54902f),
                      CameraDirection = new Vector3(0.02749483f, 0.9808867f, -0.1926273f),
                      CameraRotation = new Rotator(-11.10615f, 8.156891E-08f, -1.605614f),
                      ButtonPromptText = "Change Outfit",
                    },
                  },
              LocalID = 146689,
              InternalInteriorCoordinates = new Vector3(-609.5669f, 51.28212f, 96.60023f),
              Name = "Tinsel Towers, Apt 42",
              IsTeleportEntry = true,
              Doors = new List<InteriorDoor>() {},
              RequestIPLs =
                  new List<String>() {
                    "v_mp_apt_h_01",
                  },
              RemoveIPLs = new List<String>() {},
              InteriorSets = new List<String>() {},
              InteriorEgressPosition = new Vector3(-611.0737f, 58.90728f, 98.20042f),
              InteriorEgressHeading = 89.73334f,
              InteractPoints =
                  new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                      Name = "TinTowApt42Exit1",
                      Position = new Vector3(-611.0737f, 58.90728f, 98.20042f),
                      Heading = 268.3333f,
                      ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                      Name = "TinTowApt42Std1",
                      Position = new Vector3(-608.7299f, 46.93562f, 97.40006f),
                      Heading = 222.9291f,
                      CameraPosition = new Vector3(-611.621f, 48.20256f, 98.48028f),
                      CameraDirection = new Vector3(0.8830497f, -0.4092335f, -0.2296763f),
                      CameraRotation = new Rotator(-13.27802f, 1.052669E-05f, -114.8645f),
                    },
                    new ToiletInteract() {
                      Name = "TinTowApt42Toilet1",
                      Position = new Vector3(-588.1256f, 52.44404f, 96.99961f),
                      Heading = 357.0691f,
                      CameraPosition = new Vector3(-589.4422f, 51.73534f, 98.10249f),
                      CameraDirection = new Vector3(0.6216569f, 0.5307797f, -0.5760344f),
                      CameraRotation = new Rotator(-35.1721f, 7.624605E-05f, -49.50882f),
                      ButtonPromptText = "Use Toilet",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt42Sink1",
                      Position = new Vector3(-590.5774f, 52.45559f, 96.99961f),
                      Heading = 5.400165f,
                      CameraPosition = new Vector3(-591.9615f, 52.16739f, 97.87995f),
                      CameraDirection = new Vector3(0.802425f, 0.3998536f, -0.4429799f),
                      CameraRotation = new Rotator(-26.29416f, -3.428306E-05f, -63.51262f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt42Sink2",
                      Position = new Vector3(-589.184f, 52.41594f, 96.99961f),
                      Heading = 1.058886f,
                      CameraPosition = new Vector3(-591.9615f, 52.16739f, 97.87995f),
                      CameraDirection = new Vector3(0.802425f, 0.3998536f, -0.4429799f),
                      CameraRotation = new Rotator(-26.29416f, -3.428306E-05f, -63.51262f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                  },
              ClearPositions =
                  new List<Vector3>() {
                    new Vector3(-785.222f, 333.3687f, 201.4136f),
                  },
            },
            new ResidenceInterior(144897, "Tinsel Towers, Apt 45") {
              RestInteracts =
                  new List<RestInteract>() {
                    new RestInteract() {
                      StartAnimations = new List<AnimationBundle>() {},
                      LoopAnimations = new List<AnimationBundle>() {},
                      EndAnimations = new List<AnimationBundle>() {},
                      Name = "TinTowApt45Rest1",
                      Position = new Vector3(-618.631f, 61.77807f, 101.8197f),
                      Heading = 354.9522f,
                      CameraPosition = new Vector3(-615.642f, 61.90326f, 102.5927f),
                      CameraDirection = new Vector3(-0.809683f, 0.4772124f,
                                                    -0.3415872f),
                      CameraRotation = new Rotator(-19.9736f, 2.634402E-05f, 59.48568f),
                      ButtonPromptText = "Sleep",
                    },
                  },
              InventoryInteracts =
                  new List<InventoryInteract>() {
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Fridge",
                      Description = "Access Drinks",
                      Name = "TinTowApt45Inventory1",
                      Position = new Vector3(-609.1936f, 51.79979f, 106.6245f),
                      Heading = 271.4582f,
                      CameraPosition = new Vector3(-610.1812f, 53.42981f, 107.4547f),
                      CameraDirection = new Vector3(0.5401275f, -0.7446263f,
                                                    -0.3921657f),
                      CameraRotation = new Rotator(-23.08932f, -3.619673E-05f,
                                                   -144.0441f),
                      ButtonPromptText = "Open Fridge",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Pantry",
                      Description = "Access Food",
                      Name = "TinTowApt45Inventory2",
                      Position = new Vector3(-609.0408f, 54.71547f, 106.6245f),
                      Heading = 273.1998f,
                      CameraPosition = new Vector3(-609.7067f, 52.89948f, 107.438f),
                      CameraDirection = new Vector3(0.4093573f, 0.8242978f,
                                                    -0.3911007f),
                      CameraRotation = new Rotator(-23.02301f, 1.669795E-05f,
                                                   -26.40961f),
                      ButtonPromptText = "Open Pantry",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                          },
                      Title = "",
                      Description = "",
                      Name = "TinTowApt45Inventory3",
                      Position = new Vector3(-613.2388f, 55.32142f, 106.6245f),
                      Heading = 359.7964f,
                      CameraPosition = new Vector3(-611.2772f, 54.53897f, 107.365f),
                      CameraDirection = new Vector3(-0.8417014f, 0.4222438f,
                                                    -0.3365246f),
                      CameraRotation = new Rotator(-19.66528f, 8.159891E-06f,
                                                   63.35913f),
                      ButtonPromptText = "Access Items",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessWeapons = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt45Inventory4",
                      Position = new Vector3(-623.2095f, 51.20193f, 106.6245f),
                      Heading = 182.3948f,
                      CameraPosition = new Vector3(-625.0694f, 51.96946f, 107.3477f),
                      CameraDirection = new Vector3(0.8193148f, -0.4502851f,
                                                    -0.3549178f),
                      CameraRotation = new Rotator(-20.7884f, 7.305816E-06f,
                                                   -118.7926f),
                      ButtonPromptText = "Cash Drawer",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt45Inventory5",
                      Position = new Vector3(-606.4095f, 55.62751f, 108.0357f),
                      Heading = 91.1387f,
                      CameraPosition = new Vector3(-605.6281f, 53.8465f, 108.8756f),
                      CameraDirection = new Vector3(-0.434107f, 0.8149175f, -0.384006f),
                      CameraRotation = new Rotator(-22.58204f, 1.017134E-05f,
                                                   28.04425f),
                      ButtonPromptText = "Weapons Locker",
                      UseNavmesh = false,
                    },
                  },
              OutfitInteracts =
                  new List<OutfitInteract>() {
                    new OutfitInteract() {
                      Name = "TinTowApt45Outfit1",
                      Position = new Vector3(-617.6055f, 56.72815f, 101.8305f),
                      Heading = 265.5468f,
                      CameraPosition = new Vector3(-614.9602f, 56.85302f, 102.3801f),
                      CameraDirection = new Vector3(-0.982409f, 0.04549498f,
                                                    -0.1811153f),
                      CameraRotation = new Rotator(-10.43473f, -4.883237E-07f,
                                                   87.34855f),
                      ButtonPromptText = "Change Outfit",
                    },
                  },
              LocalID = 144897,
              InternalInteriorCoordinates = new Vector3(-613.5405f, 63.04871f,
                                                        100.8196f),
              Name = "Tinsel Towers, Apt 45",
              IsTeleportEntry = true,
              Doors = new List<InteriorDoor>() {},
              RequestIPLs =
                  new List<String>() {
                    "v_mp_apt_h_01",
                  },
              RemoveIPLs = new List<String>() {},
              InteriorSets = new List<String>() {},
              InteriorEgressPosition = new Vector3(-599.7275f, 64.91104f, 108.027f),
              InteriorEgressHeading = 90.39389f,
              InteractPoints =
                  new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                      Name = "TinTowApt45Exit1",
                      Position = new Vector3(-599.7275f, 64.91104f, 108.027f),
                      Heading = 268.7344f,
                      ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                      Name = "TinTowApt45Std1",
                      Position = new Vector3(-615.3904f, 64.36943f, 106.6244f),
                      Heading = 311.2759f,
                      CameraPosition = new Vector3(-615.1647f, 61.61671f, 107.8054f),
                      CameraDirection = new Vector3(-0.008650141f, 0.9485583f,
                                                    -0.3164842f),
                      CameraRotation = new Rotator(-18.45043f, 4.922081E-07f,
                                                   0.5224801f),
                    },
                    new ToiletInteract() {
                      Name = "TinTowApt45Toilet1",
                      Position = new Vector3(-607.4599f, 58.40004f, 101.8197f),
                      Heading = 0.8981894f,
                      CameraPosition = new Vector3(-608.9835f, 57.51441f, 102.9985f),
                      CameraDirection = new Vector3(0.6601922f, 0.5137944f, -0.54787f),
                      CameraRotation = new Rotator(-33.22101f, 3.674061E-05f,
                                                   -52.10817f),
                      ButtonPromptText = "Use Toilet",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt45Sink1",
                      Position = new Vector3(-609.9532f, 58.35662f, 101.8197f),
                      Heading = 356.0494f,
                      CameraPosition = new Vector3(-611.1108f, 58.07933f, 102.7366f),
                      CameraDirection = new Vector3(0.7352808f, 0.4828082f,
                                                    -0.4756662f),
                      CameraRotation = new Rotator(-28.40273f, 4.85305E-06f,
                                                   -56.70985f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt45Sink2",
                      Position = new Vector3(-608.4786f, 58.36885f, 101.8197f),
                      Heading = 0.3202158f,
                      CameraPosition = new Vector3(-611.1108f, 58.07933f, 102.7366f),
                      CameraDirection = new Vector3(0.7352808f, 0.4828082f,
                                                    -0.4756662f),
                      CameraRotation = new Rotator(-28.40273f, 4.85305E-06f,
                                                   -56.70985f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                  },
              ClearPositions =
                  new List<Vector3>() {
                    new Vector3(-607.4599f, 58.40004f, 101.8197f),
                  },
            },
            new ResidenceInterior(145153, "Tinsel Towers, Apt 29") {
              RestInteracts =
                  new List<RestInteract>() {
                    new RestInteract() {
                      StartAnimations = new List<AnimationBundle>() {},
                      LoopAnimations = new List<AnimationBundle>() {},
                      EndAnimations = new List<AnimationBundle>() {},
                      Name = "TinTowApt29Rest1",
                      Position = new Vector3(-582.7964f, 45.45549f, 87.41883f),
                      Heading = 176.289f,
                      CameraPosition = new Vector3(-585.7457f, 45.81867f, 88.34946f),
                      CameraDirection = new Vector3(0.782197f, -0.5455191f,
                                                    -0.3009598f),
                      CameraRotation = new Rotator(-17.51526f, -9.848099E-06f,
                                                   -124.8927f),
                      ButtonPromptText = "Sleep",
                    },
                  },
              InventoryInteracts =
                  new List<InventoryInteract>() {
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Fridge",
                      Description = "Access Drinks",
                      Name = "TinTowApt29Inventory1",
                      Position = new Vector3(-592.2039f, 55.51443f, 92.22356f),
                      Heading = 88.60062f,
                      CameraPosition = new Vector3(-591.4464f, 53.7993f, 92.99853f),
                      CameraDirection = new Vector3(-0.4610449f, 0.8348983f,
                                                    -0.3006366f),
                      CameraRotation = new Rotator(-17.49584f, -4.475931E-07f,
                                                   28.90822f),
                      ButtonPromptText = "Open Fridge",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Pantry",
                      Description = "Access Food",
                      Name = "TinTowApt29Inventory2",
                      Position = new Vector3(-592.2896f, 52.55976f, 92.22356f),
                      Heading = 92.08498f,
                      CameraPosition = new Vector3(-591.6597f, 54.08899f, 92.9202f),
                      CameraDirection = new Vector3(-0.5392722f, -0.8136974f,
                                                    -0.2169838f),
                      CameraRotation = new Rotator(-12.53194f, -1.311917E-06f,
                                                   146.4659f),
                      ButtonPromptText = "Open Pantry",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                          },
                      Title = "",
                      Description = "",
                      Name = "TinTowApt29Inventory3",
                      Position = new Vector3(-588.1188f, 51.98849f, 92.22356f),
                      Heading = 180.6237f,
                      CameraPosition = new Vector3(-590.1172f, 52.6559f, 93.05777f),
                      CameraDirection = new Vector3(0.831331f, -0.4382399f,
                                                    -0.3418106f),
                      CameraRotation = new Rotator(-19.98723f, 8.176439E-06f,
                                                   -117.7962f),
                      ButtonPromptText = "Access Items",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessWeapons = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt29Inventory4",
                      Position = new Vector3(-578.1488f, 56.15914f, 92.22356f),
                      Heading = 358.9412f,
                      CameraPosition = new Vector3(-576.3144f, 55.4312f, 92.99647f),
                      CameraDirection = new Vector3(-0.8150306f, 0.4448217f,
                                                    -0.3712934f),
                      CameraRotation = new Rotator(-21.79541f, 1.011454E-05f,
                                                   61.37548f),
                      ButtonPromptText = "Cash Drawer",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "TinTowApt29Inventory5",
                      Position = new Vector3(-594.9316f, 51.67995f, 93.63477f),
                      Heading = 271.3732f,
                      CameraPosition = new Vector3(-595.7953f, 53.47272f, 94.36034f),
                      CameraDirection = new Vector3(0.4361505f, -0.8330711f,
                                                    -0.340243f),
                      CameraRotation = new Rotator(-19.89168f, 7.26355E-06f,
                                                   -152.3659f),
                      ButtonPromptText = "Weapons Locker",
                      UseNavmesh = false,
                    },
                  },
              OutfitInteracts =
                  new List<OutfitInteract>() {
                    new OutfitInteract() {
                      Name = "TinTowApt29Outfit1",
                      Position = new Vector3(-583.8264f, 50.54099f, 87.42963f),
                      Heading = 88.27164f,
                      CameraPosition = new Vector3(-586.6664f, 50.49601f, 88.19574f),
                      CameraDirection = new Vector3(0.973895f, -0.03747115f,
                                                    -0.2238848f),
                      CameraRotation = new Rotator(-12.93731f, -1.971023E-06f,
                                                   -92.2034f),
                      ButtonPromptText = "Change Outfit",
                    },
                  },
              LocalID = 145153,
              InternalInteriorCoordinates = new Vector3(-587.8259f, 44.2688f, 86.4187f),
              Name = "Tinsel Towers, Apt 29",
              IsTeleportEntry = true,
              Doors = new List<InteriorDoor>() {},
              RequestIPLs =
                  new List<String>() {
                    "hei_dlc_apart_high_new",
                  },
              RemoveIPLs = new List<String>() {},
              InteriorSets = new List<String>() {},
              InteriorEgressPosition = new Vector3(-601.4611f, 42.43423f, 93.62614f),
              InteriorEgressHeading = 266.7249f,
              InteractPoints =
                  new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                      Name = "TinTowApt29Exit1",
                      Position = new Vector3(-601.4611f, 42.43423f, 93.62614f),
                      Heading = 88.60503f,
                      ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                      Name = "TinTowApt29Std1",
                      Position = new Vector3(-585.9499f, 42.85467f, 92.22356f),
                      Heading = 137.2741f,
                      CameraPosition = new Vector3(-586.0474f, 45.82049f, 93.5682f),
                      CameraDirection = new Vector3(-0.02836002f, -0.9373742f,
                                                    -0.3471676f),
                      CameraRotation = new Rotator(-20.31417f, 4.495088E-06f,
                                                   178.2671f),
                    },
                    new ToiletInteract() {
                      Name = "TinTowApt29Toilet1",
                      Position = new Vector3(-593.8964f, 48.92281f, 87.41885f),
                      Heading = 180.212f,
                      CameraPosition = new Vector3(-592.3853f, 50.09855f, 88.35488f),
                      CameraDirection = new Vector3(-0.6919802f, -0.5875334f,
                                                    -0.4194853f),
                      CameraRotation = new Rotator(-24.8021f, -1.786999E-05f,
                                                   130.3333f),
                      ButtonPromptText = "Use Toilet",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt29Sink1",
                      Position = new Vector3(-591.4365f, 48.95616f, 87.41884f),
                      Heading = 178.8959f,
                      CameraPosition = new Vector3(-590.1344f, 49.21557f, 88.30913f),
                      CameraDirection = new Vector3(-0.7833587f, -0.4460181f,
                                                    -0.4329168f),
                      CameraRotation = new Rotator(-25.65281f, 5.966907E-05f,
                                                   119.6557f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "TinTowApt29Sink2",
                      Position = new Vector3(-592.8592f, 48.95021f, 87.41884f),
                      Heading = 177.4461f,
                      CameraPosition = new Vector3(-590.1344f, 49.21557f, 88.30913f),
                      CameraDirection = new Vector3(-0.7833587f, -0.4460181f,
                                                    -0.4329168f),
                      CameraRotation = new Rotator(-25.65281f, 5.966907E-05f,
                                                   119.6557f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                  },
              ClearPositions =
                  new List<Vector3>() {
                    new Vector3(-785.222f, 333.3687f, 201.4136f),
                  },
            },

            //Weasel Plaza MP
            new ResidenceInterior(143617, "Weazel Plaza Apartment 101") {
              RestInteracts =
                  new List<RestInteract>() {
                    new RestInteract() {
                      StartAnimations = new List<AnimationBundle>() {},
                      LoopAnimations = new List<AnimationBundle>() {},
                      EndAnimations = new List<AnimationBundle>() {},
                      Name = "WeaPla101Rest1",
                      Position = new Vector3(-885.4216f, -448.3244f, 120.3271f),
                      Heading = 210.1126f,
                      CameraPosition = new Vector3(-888.0658f, -449.6949f, 121.1193f),
                      CameraDirection = new Vector3(0.9578518f, -0.0512642f, -0.2826515f),
                      CameraRotation = new Rotator(-16.41852f, 3.560273E-06f, -93.06355f),
                      ButtonPromptText = "Sleep",
                    },
                  },
              InventoryInteracts =
                  new List<InventoryInteract>() {
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Fridge",
                      Description = "Access Drinks",
                      Name = "WeaPla101Inventory1",
                      Position = new Vector3(-898.347f, -443.8259f, 125.1319f),
                      Heading = 119.8239f,
                      CameraPosition = new Vector3(-896.6376f, -445.1162f, 125.8658f),
                      CameraDirection = new Vector3(-0.7682052f, 0.552474f, -0.3234707f),
                      CameraRotation = new Rotator(-18.87295f, -6.315973E-06f, 54.27725f),
                      ButtonPromptText = "Open Fridge",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                          },
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "Pantry",
                      Description = "Access Food",
                      Name = "WeaPla101Inventory2",
                      Position = new Vector3(-897.0988f, -446.4196f, 125.1319f),
                      Heading = 120.9279f,
                      CameraPosition = new Vector3(-897.343f, -444.24f, 125.721f),
                      CameraDirection = new Vector3(-0.01847635f, -0.9593403f, -0.2816465f),
                      CameraRotation = new Rotator(-16.3585f, -1.946424E-07f, 178.8967f),
                      ButtonPromptText = "Open Pantry",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessWeapons = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes =
                          new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                          },
                      Title = "",
                      Description = "",
                      Name = "WeaPla101Inventory3",
                      Position = new Vector3(-893.2092f, -445.019f, 125.1319f),
                      Heading = 211.6112f,
                      CameraPosition = new Vector3(-895.4557f, -445.3192f, 125.9113f),
                      CameraDirection = new Vector3(0.9433322f, -0.02818498f, -0.3306508f),
                      CameraRotation = new Rotator(-19.30828f, -2.940138E-06f, -91.71138f),
                      ButtonPromptText = "Access Items",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessWeapons = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "WeaPla101Inventory4",
                      Position = new Vector3(-886.1941f, -436.7825f, 125.1319f),
                      Heading = 27.87741f,
                      CameraPosition = new Vector3(-883.993f, -436.6271f, 125.7194f),
                      CameraDirection = new Vector3(-0.956856f, 0.08392127f, -0.2781797f),
                      CameraRotation = new Rotator(-16.15159f, 9.999646E-07f, 84.98769f),
                      ButtonPromptText = "Cash Drawer",
                      UseNavmesh = false,
                    },
                    new InventoryInteract() {
                      CanAccessItems = false,
                      CanAccessCash = false,
                      AllowedItemTypes = new List<ItemType>() {},
                      DisallowedItemTypes = new List<ItemType>() {},
                      Title = "",
                      Description = "",
                      Name = "WeaPla101Inventory5",
                      Position = new Vector3(-899.0416f, -448.6002f, 126.5431f),
                      Heading = 301.0911f,
                      CameraPosition = new Vector3(-900.9043f, -447.2038f, 127.0556f),
                      CameraDirection = new Vector3(0.8060988f, -0.5487598f, -0.2215115f),
                      CameraRotation = new Rotator(-12.79783f, 1.313285E-06f, -124.2455f),
                      ButtonPromptText = "Weapons Locker",
                      UseNavmesh = false,
                    },
                  },
              OutfitInteracts =
                  new List<OutfitInteract>() {
                    new OutfitInteract() {
                      Name = "WeaPla101Outfit1",
                      Position = new Vector3(-888.5853f, -444.3794f, 120.3379f),
                      Heading = 117.0015f,
                      CameraPosition = new Vector3(-890.9393f, -445.6894f, 121.1364f),
                      CameraDirection = new Vector3(0.8651848f, 0.4256625f, -0.2650787f),
                      CameraRotation = new Rotator(-15.37163f, -3.541795E-06f, -63.80326f),
                      ButtonPromptText = "Change Outfit",
                    },
                  },
              LocalID = 143617,
              InternalInteriorCoordinates = new Vector3(-889.3029f, -451.775f, 119.327f),
              Name = "Weasel Plaza Apartment 101",
              IsTeleportEntry = true,
              Doors = new List<InteriorDoor>() {},
              RequestIPLs =
                  new List<String>() {
                    "hei_dlc_apart_high_new",
                  },
              RemoveIPLs = new List<String>() {},
              InteriorSets = new List<String>() {},
              InteriorEgressPosition = new Vector3(-900.5491f, -459.7449f, 126.5344f),
              InteriorEgressHeading = 302.901f,
              InteractPoints =
                  new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                      Name = "WeaPla101Exit1",
                      Position = new Vector3(-900.5491f, -459.7449f, 126.5344f),
                      Heading = 118.8819f,
                      ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                      Name = "WeaPla101Std1",
                      Position = new Vector3(-887.1334f, -452.0602f, 125.1319f),
                      Heading = 169.9079f,
                      CameraPosition = new Vector3(-889.0356f, -449.3316f, 126.4831f),
                      CameraDirection = new Vector3(0.4523076f, -0.8493323f, -0.2721257f),
                      CameraRotation = new Rotator(-15.7908f, -4.436286E-07f, -151.9627f),
                    },
                    new ToiletInteract() {
                      Name = "WeaPla101Toilet1",
                      Position = new Vector3(-896.852f, -450.4474f, 120.3271f),
                      Heading = 207.3502f,
                      CameraPosition = new Vector3(-895.7465f, -448.8305f, 121.6551f),
                      CameraDirection = new Vector3(-0.3980054f, -0.7530021f, -0.5240034f),
                      CameraRotation = new Rotator(-31.60118f, 4.009662E-06f, 152.1409f),
                      ButtonPromptText = "Use Toilet",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "WeaPla101Sink1",
                      Position = new Vector3(-894.6593f, -449.2914f, 120.3271f),
                      Heading = 206.2848f,
                      CameraPosition = new Vector3(-893.6558f, -448.2444f, 121.203f),
                      CameraDirection = new Vector3(-0.4464947f, -0.8201099f, -0.3578578f),
                      CameraRotation = new Rotator(-20.96869f, 8.228913E-06f, 151.4347f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                    new SinkInteract() {
                      Name = "WeaPla101Sink2",
                      Position = new Vector3(-895.9471f, -449.9453f, 120.3271f),
                      Heading = 205.9453f,
                      CameraPosition = new Vector3(-893.6558f, -448.2444f, 121.203f),
                      CameraDirection = new Vector3(-0.4464947f, -0.8201099f, -0.3578578f),
                      CameraRotation = new Rotator(-20.96869f, 8.228913E-06f, 151.4347f),
                      ButtonPromptText = "Use Sink",
                      UseNavmesh = false,
                    },
                  },
              ClearPositions =
                  new List<Vector3>() {
                    new Vector3(-785.222f, 333.3687f, 201.4136f),
                  },
            },
            new ResidenceInterior(142593, "Weazel Plaza Apartment 26") {
                RestInteracts =
                    new List<RestInteract>() {
                    new RestInteract() {
                        StartAnimations = new List<AnimationBundle>() {},
                        LoopAnimations = new List<AnimationBundle>() {},
                        EndAnimations = new List<AnimationBundle>() {},
                        Name = "WeaPla26Rest1",
                        Position = new Vector3(-895.7525f, -430.5316f, 89.25381f),
                        Heading = 295.5354f,
                        CameraPosition = new Vector3(-894.1095f, -433.2206f, 90.41695f),
                        CameraDirection = new Vector3(-0.01069216f, 0.9200107f,
                                                    -0.3917474f),
                        CameraRotation = new Rotator(-23.06327f, 2.116866E-06f,
                                                    0.6658486f),
                        ButtonPromptText = "Sleep",
                    },
                    },
                InventoryInteracts =
                    new List<InventoryInteract>() {
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            },
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "Fridge",
                        Description = "Access Drinks",
                        Name = "WeaPla26Inventory1",
                        Position = new Vector3(-900.2591f, -443.4739f, 94.05853f),
                        Heading = 209.2944f,
                        CameraPosition = new Vector3(-899.3292f, -442.3584f, 95.03986f),
                        CameraDirection = new Vector3(-0.6049036f, -0.7085445f,
                                                    -0.3633956f),
                        CameraRotation = new Rotator(-21.30888f, 0f, 139.5117f),
                        ButtonPromptText = "Open Fridge",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            },
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "Pantry",
                        Description = "Access Food",
                        Name = "WeaPla26Inventory2",
                        Position = new Vector3(-897.5626f, -441.9761f, 94.05853f),
                        Heading = 212.4414f,
                        CameraPosition = new Vector3(-899.0981f, -442.227f, 94.96915f),
                        CameraDirection = new Vector3(0.9246957f, -0.06614561f,
                                                    -0.3749167f),
                        CameraRotation = new Rotator(-22.01917f, -3.02762E-05f,
                                                    -94.09153f),
                        ButtonPromptText = "Open Pantry",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                            },
                        Title = "",
                        Description = "",
                        Name = "WeaPla26Inventory3",
                        Position = new Vector3(-899.1406f, -438.2918f, 94.05854f),
                        Heading = 298.6662f,
                        CameraPosition = new Vector3(-899.0717f, -439.9467f, 95.07257f),
                        CameraDirection = new Vector3(0.104243f, 0.887586f, -0.4486919f),
                        CameraRotation = new Rotator(-26.65979f, 0f, -6.698448f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessWeapons = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "",
                        Description = "",
                        Name = "WeaPla26Inventory4",
                        Position = new Vector3(-907.1481f, -431.0446f, 94.05853f),
                        Heading = 118.2901f,
                        CameraPosition = new Vector3(-907.2794f, -429.2224f, 95.29121f),
                        CameraDirection = new Vector3(-0.1267877f, -0.8225043f,
                                                    -0.5544471f),
                        CameraRotation = new Rotator(-33.67264f, -3.590651E-06f,
                                                    171.2369f),
                        ButtonPromptText = "Cash Drawer",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "",
                        Description = "",
                        Name = "WeaPla26Inventory5",
                        Position = new Vector3(-895.5907f, -444.1555f, 95.46979f),
                        Heading = 25.91599f,
                        CameraPosition = new Vector3(-896.6422f, -445.6182f, 96.22738f),
                        CameraDirection = new Vector3(0.4902275f, 0.8295374f,
                                                    -0.2674785f),
                        CameraRotation = new Rotator(-15.51428f, -2.215145E-05f,
                                                    -30.5816f),
                        ButtonPromptText = "Weapons Locker",
                        UseNavmesh = false,
                    },
                    },
                OutfitInteracts =
                    new List<OutfitInteract>() {
                    new OutfitInteract() {
                        Name = "WeaPla26Outfit1",
                        Position = new Vector3(-899.7131f, -433.6621f, 89.26461f),
                        Heading = 212.8512f,
                        CameraPosition = new Vector3(-898.4435f, -436.13f, 90.11288f),
                        CameraDirection = new Vector3(-0.3945698f, 0.8891591f,
                                                    -0.2317557f),
                        CameraRotation = new Rotator(-13.40046f, -3.949511E-06f,
                                                    23.92954f),
                        ButtonPromptText = "Change Outfit",
                    },
                    },
                LocalID = 142593,
                InternalInteriorCoordinates = new Vector3(-892.296f, -434.4147f,
                                                        88.25368f),
                Name = "Weasel Plaza Apartment 26",
                IsTeleportEntry = true,
                Doors = new List<InteriorDoor>() {},
                RequestIPLs =
                    new List<String>() {
                    "hei_dlc_apart_high_new",
                    },
                RemoveIPLs = new List<String>() {},
                InteriorSets = new List<String>() {},
                InteriorEgressPosition = new Vector3(-884.6443f, -445.5226f, 95.46111f),
                InteriorEgressHeading = 23.94146f,
                InteractPoints =
                    new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                        Name = "WeaPla26Exit1",
                        Position = new Vector3(-884.6443f, -445.5226f, 95.46111f),
                        Heading = 210.0083f,
                        ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                        Name = "WeaPla26Std1",
                        Position = new Vector3(-891.9187f, -432.0364f, 94.05853f),
                        Heading = 250.5607f,
                        CameraPosition = new Vector3(-895.1946f, -432.6104f, 95.62791f),
                        CameraDirection = new Vector3(0.9622206f, 0.08430437f,
                                                    -0.2588905f),
                        CameraRotation = new Rotator(-15.00424f, -6.629317E-07f,
                                                    -84.99285f),
                    },
                    new ToiletInteract() {
                        Name = "WeaPla26Toilet1",
                        Position = new Vector3(-893.6949f, -441.9278f, 89.25381f),
                        Heading = 298.6301f,
                        CameraPosition = new Vector3(-895.5378f, -441.1341f, 90.65579f),
                        CameraDirection = new Vector3(0.7791226f, -0.2811894f,
                                                    -0.5602682f),
                        CameraRotation = new Rotator(-34.07435f, 0f, -109.8448f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "WeaPla26Sink1",
                        Position = new Vector3(-894.1818f, -441.0596f, 89.25381f),
                        Heading = 297.065f,
                        CameraPosition = new Vector3(-895.7042f, -438.8199f, 90.06606f),
                        CameraDirection = new Vector3(0.771874f, -0.5239697f,
                                                    -0.3600921f),
                        CameraRotation = new Rotator(-21.10585f, -4.575829E-06f,
                                                    -124.1698f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "WeaPla26Sink2",
                        Position = new Vector3(-894.8121f, -439.7979f, 89.25381f),
                        Heading = 297.8102f,
                        CameraPosition = new Vector3(-895.7042f, -438.8199f, 90.06606f),
                        CameraDirection = new Vector3(0.771874f, -0.5239697f,
                                                    -0.3600921f),
                        CameraRotation = new Rotator(-21.10585f, -4.575829E-06f,
                                                    -124.1698f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    },
                ClearPositions =
                    new List<Vector3>() {
                    new Vector3(-785.222f, 333.3687f, 201.4136f),
                    },
            },
            new ResidenceInterior(143105, "Weazel Plaza Apartment 70") {
                RestInteracts =
                    new List<RestInteract>() {
                    new RestInteract() {
                        StartAnimations = new List<AnimationBundle>() {},
                        LoopAnimations = new List<AnimationBundle>() {},
                        EndAnimations = new List<AnimationBundle>() {},
                        Name = "WeaPla70Rest1",
                        Position = new Vector3(-913.0039f, -441.5837f, 115.3998f),
                        Heading = 20.74765f,
                        CameraPosition = new Vector3(-910.5404f, -440.1032f, 116.3236f),
                        CameraDirection = new Vector3(-0.9226856f, 0.04365948f,
                                                    -0.3830732f),
                        CameraRotation = new Rotator(-22.52418f, -4.274793E-06f,
                                                    87.29091f),
                        ButtonPromptText = "Sleep",
                    },
                    },
                InventoryInteracts =
                    new List<InventoryInteract>() {
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            },
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "Fridge",
                        Description = "Access Drinks",
                        Name = "WeaPla70Inventory1",
                        Position = new Vector3(-900.1489f, -446.2936f, 120.2045f),
                        Heading = 302.4942f,
                        CameraPosition = new Vector3(-901.2985f, -445.3068f, 121.3068f),
                        CameraDirection = new Vector3(0.6277661f, -0.5407953f,
                                                    -0.5598662f),
                        CameraRotation = new Rotator(-34.04654f, -5.15201E-06f,
                                                    -130.7436f),
                        ButtonPromptText = "Open Fridge",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            },
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "Pantry",
                        Description = "Access Food",
                        Name = "WeaPla70Inventory2",
                        Position = new Vector3(-901.4078f, -443.6565f, 120.2045f),
                        Heading = 295.9217f,
                        CameraPosition = new Vector3(-901.1397f, -445.0884f, 121.229f),
                        CameraDirection = new Vector3(0.0485396f, 0.8646972f,
                                                    -0.4999427f),
                        CameraRotation = new Rotator(-29.99621f, 2.464538E-07f,
                                                    -3.212915f),
                        ButtonPromptText = "Open Pantry",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessWeapons = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes =
                            new List<ItemType>() {
                            new ItemType() {},
                            new ItemType() {},
                            new ItemType() {},
                            },
                        Title = "",
                        Description = "",
                        Name = "WeaPla70Inventory3",
                        Position = new Vector3(-905.3445f, -445.0342f, 120.2045f),
                        Heading = 27.72766f,
                        CameraPosition = new Vector3(-903.4775f, -445.0414f, 121.2653f),
                        CameraDirection = new Vector3(-0.8626413f, 0.1167328f,
                                                    -0.492162f),
                        CameraRotation = new Rotator(-29.48279f, -1.05434E-05f,
                                                    82.29353f),
                        ButtonPromptText = "Access Items",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessWeapons = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "",
                        Description = "",
                        Name = "WeaPla70Inventory4",
                        Position = new Vector3(-912.4459f, -453.1483f, 120.2045f),
                        Heading = 206.1814f,
                        CameraPosition = new Vector3(-914.5148f, -453.2444f, 121.1589f),
                        CameraDirection = new Vector3(0.9113337f, -0.07373723f,
                                                    -0.4050107f),
                        CameraRotation = new Rotator(-23.8918f, -4.668941E-07f,
                                                    -94.6258f),
                        ButtonPromptText = "Cash Drawer",
                        UseNavmesh = false,
                    },
                    new InventoryInteract() {
                        CanAccessItems = false,
                        CanAccessCash = false,
                        AllowedItemTypes = new List<ItemType>() {},
                        DisallowedItemTypes = new List<ItemType>() {},
                        Title = "",
                        Description = "",
                        Name = "WeaPla70Inventory5",
                        Position = new Vector3(-899.3101f, -441.5693f, 121.6157f),
                        Heading = 118.3303f,
                        CameraPosition = new Vector3(-897.814f, -442.8384f, 122.5912f),
                        CameraDirection = new Vector3(-0.7120011f, 0.5564297f,
                                                    -0.4282994f),
                        CameraRotation = new Rotator(-25.35969f, 0f, 51.99233f),
                        ButtonPromptText = "Weapons Locker",
                        UseNavmesh = false,
                    },
                    },
                OutfitInteracts =
                    new List<OutfitInteract>() {
                    new OutfitInteract() {
                        Name = "WeaPla70Outfit1",
                        Position = new Vector3(-909.7336f, -445.5449f, 115.4106f),
                        Heading = 294.5024f,
                        CameraPosition = new Vector3(-907.3846f, -444.4299f, 115.9478f),
                        CameraDirection = new Vector3(-0.8967538f, -0.3839138f,
                                                    -0.2200972f),
                        CameraRotation = new Rotator(-12.71474f, 3.500945E-06f,
                                                    113.1765f),
                        ButtonPromptText = "Change Outfit",
                    },
                    },
                LocalID = 143105,
                InternalInteriorCoordinates = new Vector3(-909.1017f, -438.1903f,
                                                        114.3997f),
                Name = "Weasel Plaza Apartment 70",
                IsTeleportEntry = true,
                Doors = new List<InteriorDoor>() {},
                RequestIPLs =
                    new List<String>() {
                    "hei_dlc_apart_high_new",
                    },
                RemoveIPLs = new List<String>() {},
                InteriorSets = new List<String>() {},
                InteriorEgressPosition = new Vector3(-897.7975f, -430.5042f, 121.6071f),
                InteriorEgressHeading = 115.802f,
                InteractPoints =
                    new List<InteriorInteract>() {
                    new ExitInteriorInteract() {
                        Name = "WeaPla70Exit1",
                        Position = new Vector3(-897.7975f, -430.5042f, 121.6071f),
                        Heading = 299.5035f,
                        ButtonPromptText = "Exit",
                    },
                    new StandardInteriorInteract() {
                        Name = "WeaPla70Std1",
                        Position = new Vector3(-911.3915f, -437.8884f, 120.2045f),
                        Heading = 339.0608f,
                        CameraPosition = new Vector3(-911.0555f, -440.2426f, 121.4915f),
                        CameraDirection = new Vector3(0.04418589f, 0.9261936f,
                                                    -0.3744503f),
                        CameraRotation = new Rotator(-21.99034f, 1.150952E-06f,
                                                    -2.731337f),
                    },
                    new ToiletInteract() {
                        Name = "WeaPla70Toilet1",
                        Position = new Vector3(-901.5637f, -439.6154f, 115.3998f),
                        Heading = 26.96885f,
                        CameraPosition = new Vector3(-902.3524f, -440.9413f, 116.628f),
                        CameraDirection = new Vector3(0.2879178f, 0.7362373f,
                                                    -0.6124198f),
                        CameraRotation = new Rotator(-37.76468f, 0f, -21.35881f),
                        ButtonPromptText = "Use Toilet",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "WeaPla70Sink1",
                        Position = new Vector3(-903.7506f, -440.7396f, 115.3998f),
                        Heading = 29.37448f,
                        CameraPosition = new Vector3(-904.7712f, -441.5399f, 116.5491f),
                        CameraDirection = new Vector3(0.5104524f, 0.6751515f,
                                                    -0.5325494f),
                        CameraRotation = new Rotator(-32.17787f, -9.481902E-05f,
                                                    -37.09132f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    new SinkInteract() {
                        Name = "WeaPla70Sink2",
                        Position = new Vector3(-902.4836f, -440.102f, 115.3998f),
                        Heading = 25.71767f,
                        CameraPosition = new Vector3(-904.7712f, -441.5399f, 116.5491f),
                        CameraDirection = new Vector3(0.5104524f, 0.6751515f,
                                                    -0.5325494f),
                        CameraRotation = new Rotator(-32.17787f, -9.481902E-05f,
                                                    -37.09132f),
                        ButtonPromptText = "Use Sink",
                        UseNavmesh = false,
                    },
                    },
                ClearPositions =
                    new List<Vector3>() {
                    new Vector3(-785.222f, 333.3687f, 201.4136f),
                    },
            },

            //More MP Apartments
            new ResidenceInterior(300313503,"Eclipse Towers, Apt 3") 
            {
                RestInteracts = new List<RestInteract>() {
                new RestInteract() {
                StartAnimations = new List<AnimationBundle>() {
                },
                LoopAnimations = new List<AnimationBundle>() {
                },
                EndAnimations = new List<AnimationBundle>() {
                },
                Name = "EclTowApt3Rest1",
                Position = new Vector3(-794.2382f,332.1173f,210.7966f),
                Heading = 357.8991f,
                CameraPosition = new Vector3(-790.9478f,332.1293f,211.5446f),
                CameraDirection = new Vector3(-0.834198f,0.46145f,-0.3019564f),
                CameraRotation = new Rotator(-17.57514f,6.269043E-06f,61.05012f),
                ButtonPromptText = "Sleep",
                },
                },
                InventoryInteracts = new List<InventoryInteract>() {
                new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List<ItemType>() {
                new ItemType() {
                },
                },
                DisallowedItemTypes = new List<ItemType>() {
                },
                Title = "Fridge",
                Description = "Access Drinks",
                Name = "EclTowApt3Inventory1",
                Position = new Vector3(-769.7903f,335.2822f,211.397f),
                Heading = 179.8926f,
                CameraPosition = new Vector3(-773.8968f,336.1825f,212.0204f),
                CameraDirection = new Vector3(0.8932383f,-0.3579163f,-0.2720685f),
                CameraRotation = new Rotator(-15.78739f,-3.726418E-05f,-111.8357f),
                ButtonPromptText = "Open Fridge",
                UseNavmesh = false,
                },
                new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List<ItemType>() {
                new ItemType() {
                },
                new ItemType() {
                },
                },
                DisallowedItemTypes = new List<ItemType>() {
                },
                Title = "Pantry",
                Description = "Access Food",
                Name = "EclTowApt3Inventory2",
                Position = new Vector3(-771.836f,335.325f,211.397f),
                Heading = 189.6519f,
                CameraPosition = new Vector3(-773.8968f,336.1825f,212.0204f),
                CameraDirection = new Vector3(0.8932383f,-0.3579163f,-0.2720685f),
                CameraRotation = new Rotator(-15.78739f,-3.726418E-05f,-111.8357f),
                ButtonPromptText = "Open Pantry",
                UseNavmesh = false,
                },
                new InventoryInteract() {
                CanAccessWeapons = false,
                CanAccessCash = false,
                AllowedItemTypes = new List<ItemType>() {
                },
                DisallowedItemTypes = new List<ItemType>() {
                new ItemType() {
                },
                new ItemType() {
                },
                new ItemType() {
                },
                },
                Title = "",
                Description = "",
                Name = "EclTowApt3Inventory3",
                Position = new Vector3(-770.0435f,339.3617f,211.397f),
                Heading = 87.64685f,
                CameraPosition = new Vector3(-768.8466f,341.1152f,212.2362f),
                CameraDirection = new Vector3(-0.5258477f,-0.7788754f,-0.3418148f),
                CameraRotation = new Rotator(-19.98748f,3.633978E-06f,145.9753f),
                ButtonPromptText = "Access Items",
                UseNavmesh = false,
                },
                new InventoryInteract() {
                CanAccessItems = false,
                CanAccessWeapons = false,
                AllowedItemTypes = new List<ItemType>() {
                },
                DisallowedItemTypes = new List<ItemType>() {
                },
                Title = "",
                Description = "",
                Name = "EclTowApt3Inventory4",
                Position = new Vector3(-790.1963f,333.422f,210.8318f),
                Heading = 272.4976f,
                CameraPosition = new Vector3(-791.2614f,331.5301f,211.5391f),
                CameraDirection = new Vector3(0.5184225f,0.7912914f,-0.3241852f),
                CameraRotation = new Rotator(-18.91622f,-9.02515E-06f,-33.23122f),
                ButtonPromptText = "Cash Drawer",
                UseNavmesh = false,
                },
                new InventoryInteract() {
                CanAccessItems = false,
                CanAccessCash = false,
                AllowedItemTypes = new List<ItemType>() {
                },
                DisallowedItemTypes = new List<ItemType>() {
                },
                Title = "",
                Description = "",
                Name = "EclTowApt3Inventory5",
                Position = new Vector3(-765.2819f,326.6274f,211.3965f),
                Heading = 186.2076f,
                CameraPosition = new Vector3(-763.3954f,327.548f,212.0067f),
                CameraDirection = new Vector3(-0.8535704f,-0.4325231f,-0.2904159f),
                CameraRotation = new Rotator(-16.88286f,1.338342E-06f,116.8723f),
                ButtonPromptText = "Weapons Locker",
                UseNavmesh = false,
                },
                },
                OutfitInteracts = new List<OutfitInteract>() {
                new OutfitInteract() {
                Name = "EclTowApt3Outfit1",
                Position = new Vector3(-793.3536f,325.9384f,210.7965f),
                Heading = 355.9424f,
                CameraPosition = new Vector3(-793.3113f,328.365f,211.2841f),
                CameraDirection = new Vector3(-0.02899069f,-0.9774232f,-0.2092928f),
                CameraRotation = new Rotator(-12.08091f,-1.009534E-06f,178.3011f),
                ButtonPromptText = "Change Outfit",
                },
                },
                LocalID = 300313503,
                InternalInteriorCoordinates = new Vector3(-791.294f,338.071f,200.4135f),
                Name = "Eclipse Towers, Apt 3",
                IsTeleportEntry = true,
                Doors = new List<InteriorDoor>() {
                },
                RequestIPLs = new List<String>() {
                "hei_dlc_apart_high_new",
                },
                RemoveIPLs = new List<String>() {
                },
                InteriorSets = new List<String>() {
                },
                InteriorEgressPosition = new Vector3(-776.9362f,323.6867f,211.9975f),
                InteriorEgressHeading = 268.3861f,
                InteractPoints = new List<InteriorInteract>() {
                new ExitInteriorInteract() {
                Name = "EclTowApt3Exit1",
                Position = new Vector3(-776.9362f,323.6867f,211.9975f),
                Heading = 92.63223f,
                ButtonPromptText = "Exit",
                },
                new StandardInteriorInteract() {
                Name = "EclTowApt3Std1",
                Position = new Vector3(-779.4938f,335.7599f,211.1971f),
                Heading = 45.97082f,
                CameraPosition = new Vector3(-777.1794f,335.5338f,211.935f),
                CameraDirection = new Vector3(-0.9467912f,0.1103402f,-0.3023434f),
                CameraRotation = new Rotator(-17.59841f,-6.717697E-07f,83.35267f),
                },
                new ToiletInteract() {
                Name = "EclTowApt3Toilet1",
                Position = new Vector3(-799.9388f,330.1344f,210.7966f),
                Heading = 177.472f,
                CameraPosition = new Vector3(-798.165f,330.9734f,211.4549f),
                CameraDirection = new Vector3(-0.7939164f,-0.5101756f,-0.3307833f),
                CameraRotation = new Rotator(-19.31633f,4.523511E-06f,122.7251f),
                ButtonPromptText = "Use Toilet",
                UseNavmesh = false,
                },
                new SinkInteract() {
                Name = "EclTowApt3Sink1",
                Position = new Vector3(-797.4703f,330.1364f,210.7966f),
                Heading = 178.7707f,
                CameraPosition = new Vector3(-796.1561f,330.3595f,211.6637f),
                CameraDirection = new Vector3(-0.8087228f,-0.4037162f,-0.4277623f),
                CameraRotation = new Rotator(-25.32563f,3.872668E-05f,116.5285f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
                },
                new SinkInteract() {
                Name = "EclTowApt3Sink2",
                Position = new Vector3(-798.9324f,330.1334f,210.7966f),
                Heading = 178.7051f,
                CameraPosition = new Vector3(-796.1561f,330.3595f,211.6637f),
                CameraDirection = new Vector3(-0.8087228f,-0.4037162f,-0.4277623f),
                CameraRotation = new Rotator(-25.32563f,3.872668E-05f,116.5285f),
                ButtonPromptText = "Use Sink",
                UseNavmesh = false,
                },
                },
                ClearPositions = new List<Vector3>() {
                new Vector3(-799.9388f,330.1344f,210.7966f),
                },
                },
            new ResidenceInterior(144385,"Eclipse Towers, Apt 31") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowApt31Rest1",
            Position = new Vector3(-796.7646f,337.2329f,153.7943f),
            Heading = 2.695368f,
            CameraPosition = new Vector3(-793.9561f,337.593f,154.615f),
            CameraDirection = new Vector3(-0.8179816f,0.4361713f,-0.3750476f),
            CameraRotation = new Rotator(-22.02726f,-9.210016E-06f,61.93218f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowApt31Inventory1",
            Position = new Vector3(-787.473f,327.2547f,158.599f),
            Heading = 271.1107f,
            CameraPosition = new Vector3(-788.4828f,329.1906f,159.2095f),
            CameraDirection = new Vector3(0.5769045f,-0.770802f,-0.270269f),
            CameraRotation = new Rotator(-15.68028f,2.349954E-05f,-143.1871f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowApt31Inventory2",
            Position = new Vector3(-787.3885f,330.2854f,158.599f),
            Heading = 275.8958f,
            CameraPosition = new Vector3(-788.1501f,328.3446f,159.3437f),
            CameraDirection = new Vector3(0.3849123f,0.8574076f,-0.3416062f),
            CameraRotation = new Rotator(-19.97476f,1.589738E-05f,-24.17654f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowApt31Inventory3",
            Position = new Vector3(-791.473f,330.6137f,158.599f),
            Heading = 1.556483f,
            CameraPosition = new Vector3(-789.3279f,329.9838f,159.1079f),
            CameraDirection = new Vector3(-0.8895684f,0.3828046f,-0.2492561f),
            CameraRotation = new Rotator(-14.43349f,-4.407995E-06f,66.71652f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt31Inventory4",
            Position = new Vector3(-801.4374f,326.587f,158.599f),
            Heading = 181.1285f,
            CameraPosition = new Vector3(-803.3651f,327.2435f,159.3797f),
            CameraDirection = new Vector3(0.8770649f,-0.3173487f,-0.3606202f),
            CameraRotation = new Rotator(-21.13829f,5.949879E-06f,-109.8917f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt31Inventory5",
            Position = new Vector3(-784.5331f,331.0416f,160.0101f),
            Heading = 90.93836f,
            CameraPosition = new Vector3(-783.7991f,329.1977f,160.8094f),
            CameraDirection = new Vector3(-0.4186683f,0.8321925f,-0.3635553f),
            CameraRotation = new Rotator(-21.3187f,1.191432E-05f,26.70654f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowApt31Outfit1",
            Position = new Vector3(-796.139f,332.1796f,153.805f),
            Heading = 268.4612f,
            CameraPosition = new Vector3(-793.8141f,332.1228f,154.232f),
            CameraDirection = new Vector3(-0.9807909f,0.01977829f,-0.1940569f),
            CameraRotation = new Rotator(-11.18964f,4.161208E-06f,88.84475f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 144385,
            InternalInteriorCoordinates = new Vector3(-791.7613f,338.4633f,152.7941f),
            Name = "Eclipse Towers, Apt 31",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-778.1878f,340.2737f,160.0016f),
            InteriorEgressHeading = 84.02899f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowApt31Exit1",
            Position = new Vector3(-778.1878f,340.2737f,160.0016f),
            Heading = 272.1511f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowApt31Std1",
            Position = new Vector3(-793.6388f,339.7443f,158.599f),
            Heading = 312.999f,
            CameraPosition = new Vector3(-793.589f,337.4945f,159.4298f),
            CameraDirection = new Vector3(0.0420437f,0.9379429f,-0.3442316f),
            CameraRotation = new Rotator(-20.1349f,4.603578E-06f,-2.56659f),
            },
            new ToiletInteract() {
            Name = "EclTowApt31Toilet1",
            Position = new Vector3(-785.6759f,333.7808f,153.7943f),
            Heading = 359.5056f,
            CameraPosition = new Vector3(-787.1832f,332.8556f,154.7406f),
            CameraDirection = new Vector3(0.634066f,0.6208942f,-0.4609236f),
            CameraRotation = new Rotator(-27.44672f,0f,-45.60135f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt31Sink1",
            Position = new Vector3(-788.1632f,333.7371f,153.7943f),
            Heading = 358.2891f,
            CameraPosition = new Vector3(-789.5087f,333.4411f,154.5853f),
            CameraDirection = new Vector3(0.8060546f,0.4705779f,-0.3589324f),
            CameraRotation = new Rotator(-21.03465f,6.403095E-06f,-59.72345f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt31Sink2",
            Position = new Vector3(-786.7275f,333.6949f,153.7943f),
            Heading = 354.8169f,
            CameraPosition = new Vector3(-789.5087f,333.4411f,154.5853f),
            CameraDirection = new Vector3(0.8060546f,0.4705779f,-0.3589324f),
            CameraRotation = new Rotator(-21.03465f,6.403095E-06f,-59.72345f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-785.6759f,333.7808f,153.7943f),
            },
            },
            new ResidenceInterior(144129,"Eclipse Towers, Apt 40") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowApt40Rest1",
            Position = new Vector3(-759.8313f,320.4859f,217.0504f),
            Heading = 178.5156f,
            CameraPosition = new Vector3(-763.0223f,320.148f,218.162f),
            CameraDirection = new Vector3(0.852007f,-0.3778595f,-0.362362f),
            CameraRotation = new Rotator(-21.24533f,2.381676E-05f,-113.917f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowApt40Inventory1",
            Position = new Vector3(-769.0975f,330.4489f,221.8551f),
            Heading = 91.61729f,
            CameraPosition = new Vector3(-768.2119f,328.7206f,222.7053f),
            CameraDirection = new Vector3(-0.562227f,0.7253343f,-0.397229f),
            CameraRotation = new Rotator(-23.40507f,-1.767609E-05f,37.78027f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowApt40Inventory2",
            Position = new Vector3(-769.1849f,327.4959f,221.8551f),
            Heading = 97.14477f,
            CameraPosition = new Vector3(-768.4092f,329.3842f,222.4565f),
            CameraDirection = new Vector3(-0.3925921f,-0.8671825f,-0.3063756f),
            CameraRotation = new Rotator(-17.84094f,2.556179E-05f,155.6427f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowApt40Inventory3",
            Position = new Vector3(-765.1456f,326.9809f,221.8551f),
            Heading = 184.2697f,
            CameraPosition = new Vector3(-767.1672f,327.7366f,222.6002f),
            CameraDirection = new Vector3(0.8634499f,-0.3932828f,-0.3158842f),
            CameraRotation = new Rotator(-18.4142f,1.214795E-05f,-114.4882f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt40Inventory4",
            Position = new Vector3(-755.1428f,331.1204f,221.8551f),
            Heading = 0.3681362f,
            CameraPosition = new Vector3(-753.2482f,330.2609f,222.6234f),
            CameraDirection = new Vector3(-0.8156797f,0.4680566f,-0.3399848f),
            CameraRotation = new Rotator(-19.87595f,-7.262827E-06f,60.15176f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt40Inventory5",
            Position = new Vector3(-772.0205f,326.5904f,223.2664f),
            Heading = 268.113f,
            CameraPosition = new Vector3(-772.7098f,328.4905f,224.1418f),
            CameraDirection = new Vector3(0.4237343f,-0.8133291f,-0.3986791f),
            CameraRotation = new Rotator(-23.49563f,-1.489534E-05f,-152.481f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowApt40Outfit1",
            Position = new Vector3(-760.2075f,325.4823f,217.0611f),
            Heading = 88.33512f,
            CameraPosition = new Vector3(-762.427f,325.4265f,217.575f),
            CameraDirection = new Vector3(0.9732847f,-0.02087267f,-0.2286509f),
            CameraRotation = new Rotator(-13.21766f,-1.425136E-06f,-91.22855f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 144129,
            InternalInteriorCoordinates = new Vector3(-764.8132f,319.1851f,216.0502f),
            Name = "Eclipse Towers, Apt 40",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-778.4022f,317.3115f,223.2576f),
            InteriorEgressHeading = 264.827f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowApt40Exit1",
            Position = new Vector3(-778.4022f,317.3115f,223.2576f),
            Heading = 89.40898f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowApt40Std1",
            Position = new Vector3(-762.9221f,317.8928f,221.8551f),
            Heading = 136.2631f,
            CameraPosition = new Vector3(-762.9666f,320.1385f,222.4028f),
            CameraDirection = new Vector3(-0.006415768f,-0.9667113f,-0.2557892f),
            CameraRotation = new Rotator(-14.82035f,4.415769E-07f,179.6198f),
            },
            new ToiletInteract() {
            Name = "EclTowApt40Toilet1",
            Position = new Vector3(-770.9149f,323.8586f,217.0504f),
            Heading = 176.3236f,
            CameraPosition = new Vector3(-769.2155f,324.5547f,217.9612f),
            CameraDirection = new Vector3(-0.739475f,-0.5053652f,-0.4447277f),
            CameraRotation = new Rotator(-26.40592f,-9.532278E-06f,124.3491f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt40Sink1",
            Position = new Vector3(-768.416f,323.9488f,217.0504f),
            Heading = 179.3142f,
            CameraPosition = new Vector3(-767.1035f,324.1575f,217.8914f),
            CameraDirection = new Vector3(-0.8139243f,-0.4065121f,-0.4150605f),
            CameraRotation = new Rotator(-24.52312f,-2.721434E-05f,116.5397f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt40Sink2",
            Position = new Vector3(-769.8866f,323.9201f,217.0504f),
            Heading = 186.554f,
            CameraPosition = new Vector3(-767.1035f,324.1575f,217.8914f),
            CameraDirection = new Vector3(-0.8139243f,-0.4065121f,-0.4150605f),
            CameraRotation = new Rotator(-24.52312f,-2.721434E-05f,116.5397f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-770.9149f,323.8586f,217.0504f),
            },
            },
            new ResidenceInterior(300313505,"Eclipse Towers, Apt 5") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowApt5Rest1",
            Position = new Vector3(-796.2675f,336.9465f,201.4136f),
            Heading = 359.5772f,
            CameraPosition = new Vector3(-793.4705f,336.9409f,202.1637f),
            CameraDirection = new Vector3(-0.8011839f,0.4945044f,-0.337001f),
            CameraRotation = new Rotator(-19.69426f,-6.34773E-06f,58.31641f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowApt5Inventory1",
            Position = new Vector3(-787.0167f,326.7994f,206.2184f),
            Heading = 270.546f,
            CameraPosition = new Vector3(-787.9161f,328.6867f,206.8889f),
            CameraDirection = new Vector3(0.4781071f,-0.8180887f,-0.3196007f),
            CameraRotation = new Rotator(-18.63878f,2.252576E-05f,-149.6971f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowApt5Inventory2",
            Position = new Vector3(-786.8748f,329.7741f,206.2184f),
            Heading = 269.4378f,
            CameraPosition = new Vector3(-787.7747f,327.7558f,206.8424f),
            CameraDirection = new Vector3(0.4007738f,0.8632554f,-0.306872f),
            CameraRotation = new Rotator(-17.87083f,4.933806E-06f,-24.9035f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowApt5Inventory3",
            Position = new Vector3(-790.9933f,330.2722f,206.2184f),
            Heading = 5.248404f,
            CameraPosition = new Vector3(-789.0598f,329.4584f,206.8278f),
            CameraDirection = new Vector3(-0.8337903f,0.4510108f,-0.3184069f),
            CameraRotation = new Rotator(-18.56661f,4.503243E-06f,61.59032f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt5Inventory4",
            Position = new Vector3(-800.9472f,326.2192f,206.2184f),
            Heading = 180.86f,
            CameraPosition = new Vector3(-802.8814f,327.1565f,206.7833f),
            CameraDirection = new Vector3(0.8668659f,-0.415243f,-0.2758927f),
            CameraRotation = new Rotator(-16.01522f,-3.552992E-06f,-115.5952f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt5Inventory5",
            Position = new Vector3(-784.1207f,330.6962f,207.6296f),
            Heading = 88.24436f,
            CameraPosition = new Vector3(-783.1992f,328.7449f,208.2666f),
            CameraDirection = new Vector3(-0.4636579f,0.8338131f,-0.2996281f),
            CameraRotation = new Rotator(-17.43527f,1.029122E-05f,29.07709f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowApt5Outfit1",
            Position = new Vector3(-795.8353f,331.7423f,201.4244f),
            Heading = 267.5375f,
            CameraPosition = new Vector3(-793.1301f,331.7907f,201.8254f),
            CameraDirection = new Vector3(-0.9852995f,0.04226841f,-0.1655246f),
            CameraRotation = new Rotator(-9.52771f,3.262665E-05f,87.54357f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 300313505,
            InternalInteriorCoordinates = new Vector3(-791.2941f,338.071f,200.4135f),
            Name = "Eclipse Towers, Apt 5",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-778.1547f,339.9178f,207.6209f),
            InteriorEgressHeading = 96.86304f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowApt5Exit1",
            Position = new Vector3(-778.1547f,339.9178f,207.6209f),
            Heading = 274.4365f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowApt5Std1",
            Position = new Vector3(-793.1989f,339.2793f,206.2184f),
            Heading = 318.6518f,
            CameraPosition = new Vector3(-793.2636f,336.8159f,206.8321f),
            CameraDirection = new Vector3(0.02400343f,0.9748843f,-0.2214145f),
            CameraRotation = new Rotator(-12.79213f,-3.009544E-07f,-1.410442f),
            },
            new ToiletInteract() {
            Name = "EclTowApt5Toilet1",
            Position = new Vector3(-785.2192f,333.4179f,201.4136f),
            Heading = 2.141076f,
            CameraPosition = new Vector3(-786.7215f,332.7502f,202.4797f),
            CameraDirection = new Vector3(0.6797885f,0.512443f,-0.5246806f),
            CameraRotation = new Rotator(-31.64674f,1.805232E-05f,-52.99002f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt5Sink1",
            Position = new Vector3(-787.7003f,333.3012f,201.4136f),
            Heading = 3.767585f,
            CameraPosition = new Vector3(-788.9994f,333.1211f,202.281f),
            CameraDirection = new Vector3(0.8231035f,0.3953213f,-0.4077028f),
            CameraRotation = new Rotator(-24.06061f,3.833551E-05f,-64.34589f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt5Sink2",
            Position = new Vector3(-786.2582f,333.3006f,201.4136f),
            Heading = 0.5754824f,
            CameraPosition = new Vector3(-788.9994f,333.1211f,202.281f),
            CameraDirection = new Vector3(0.8231035f,0.3953213f,-0.4077028f),
            CameraRotation = new Rotator(-24.06061f,3.833551E-05f,-64.34589f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-785.2192f,333.4179f,201.4136f),
            },
            },
            new ResidenceInterior(144641,"Eclipse Towers, Apt 9") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowApt9Rest1",
            Position = new Vector3(-759.7751f,320.5169f,170.5965f),
            Heading = 178.3001f,
            CameraPosition = new Vector3(-762.7606f,320.3342f,171.4513f),
            CameraDirection = new Vector3(0.8491555f,-0.4438336f,-0.2862635f),
            CameraRotation = new Rotator(-16.63439f,-8.910638E-06f,-117.595f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowApt9Inventory1",
            Position = new Vector3(-769.0279f,330.4546f,175.4012f),
            Heading = 91.1314f,
            CameraPosition = new Vector3(-767.8176f,328.8437f,176.0703f),
            CameraDirection = new Vector3(-0.6583695f,0.6828285f,-0.3166935f),
            CameraRotation = new Rotator(-18.46308f,3.150363E-05f,43.95523f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowApt9Inventory2",
            Position = new Vector3(-769.1045f,327.4637f,175.4012f),
            Heading = 90.31756f,
            CameraPosition = new Vector3(-768.2756f,329.2923f,176.0744f),
            CameraDirection = new Vector3(-0.4034643f,-0.8529473f,-0.331206f),
            CameraRotation = new Rotator(-19.34199f,9.048442E-07f,154.6847f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowApt9Inventory3",
            Position = new Vector3(-765.0401f,326.9443f,175.4012f),
            Heading = 180.7144f,
            CameraPosition = new Vector3(-766.9792f,327.808f,175.9585f),
            CameraDirection = new Vector3(0.8664972f,-0.4190875f,-0.2711978f),
            CameraRotation = new Rotator(-15.73555f,3.991571E-06f,-115.8111f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt9Inventory4",
            Position = new Vector3(-755.0518f,331.0305f,175.4012f),
            Heading = 7.168064f,
            CameraPosition = new Vector3(-753.1273f,330.3277f,176.0551f),
            CameraDirection = new Vector3(-0.8814247f,0.3464822f,-0.3209994f),
            CameraRotation = new Rotator(-18.72338f,1.307147E-05f,68.54054f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowApt9Inventory5",
            Position = new Vector3(-771.9211f,326.5256f,176.8123f),
            Heading = 272.5923f,
            CameraPosition = new Vector3(-772.8336f,328.4155f,177.5145f),
            CameraDirection = new Vector3(0.5200376f,-0.7893677f,-0.3262812f),
            CameraRotation = new Rotator(-19.04322f,3.703135E-05f,-146.623f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowApt9Outfit1",
            Position = new Vector3(-759.87f,325.5014f,170.6072f),
            Heading = 88.77864f,
            CameraPosition = new Vector3(-762.1398f,325.4697f,171.131f),
            CameraDirection = new Vector3(0.9698167f,-0.008127547f,-0.2436998f),
            CameraRotation = new Rotator(-14.10501f,2.750983E-08f,-90.48016f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 144641,
            InternalInteriorCoordinates = new Vector3(-764.7226f,319.1851f,169.5963f),
            Name = "Eclipse Towers, Apt 9",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-778.2145f,317.3462f,176.8037f),
            InteriorEgressHeading = 259.2614f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowApt9Exit1",
            Position = new Vector3(-778.2145f,317.3462f,176.8037f),
            Heading = 89.75745f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowApt9Std1",
            Position = new Vector3(-762.8351f,317.9705f,175.4012f),
            Heading = 139.311f,
            CameraPosition = new Vector3(-762.8588f,320.4118f,175.993f),
            CameraDirection = new Vector3(-0.007370871f,-0.9690693f,-0.2466787f),
            CameraRotation = new Rotator(-14.28106f,-1.672521E-06f,179.5642f),
            },
            new ToiletInteract() {
            Name = "EclTowApt9Toilet1",
            Position = new Vector3(-770.7679f,323.913f,170.5965f),
            Heading = 172.5276f,
            CameraPosition = new Vector3(-768.9386f,324.5746f,171.2966f),
            CameraDirection = new Vector3(-0.7926278f,-0.4993392f,-0.3498592f),
            CameraRotation = new Rotator(-20.47871f,4.55685E-06f,122.2101f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt9Sink1",
            Position = new Vector3(-768.3495f,323.9526f,170.5964f),
            Heading = 182.1414f,
            CameraPosition = new Vector3(-767.1012f,324.1149f,171.4917f),
            CameraDirection = new Vector3(-0.7833929f,-0.4272278f,-0.4514111f),
            CameraRotation = new Rotator(-26.83426f,2.870419E-05f,118.6061f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowApt9Sink2",
            Position = new Vector3(-769.7753f,323.9225f,170.5965f),
            Heading = 176.7682f,
            CameraPosition = new Vector3(-767.1012f,324.1149f,171.4917f),
            CameraDirection = new Vector3(-0.7833929f,-0.4272278f,-0.4514111f),
            CameraRotation = new Rotator(-26.83426f,2.870419E-05f,118.6061f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-770.7679f,323.913f,170.5965f),
            },
            },
            new ResidenceInterior(227329,"Eclipse Towers Penthouse 1") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowPentHo1Rest1",
            Position = new Vector3(-795.7125f,335.2562f,220.4384f),
            Heading = 91.37115f,
            CameraPosition = new Vector3(-795.9611f,338.7694f,221.2617f),
            CameraDirection = new Vector3(-0.4261923f,-0.8525692f,-0.3024664f),
            CameraRotation = new Rotator(-17.6058f,0f,153.4399f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowPentHo1Inventory1",
            Position = new Vector3(-782.2148f,325.5936f,217.0381f),
            Heading = 181.5688f,
            CameraPosition = new Vector3(-786.2755f,326.5681f,217.7481f),
            CameraDirection = new Vector3(0.8297734f,-0.4694648f,-0.3017928f),
            CameraRotation = new Rotator(-17.56532f,-4.477645E-06f,-119.5001f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowPentHo1Inventory2",
            Position = new Vector3(-784.2417f,325.551f,217.0381f),
            Heading = 184.3132f,
            CameraPosition = new Vector3(-786.2755f,326.5681f,217.7481f),
            CameraDirection = new Vector3(0.8297734f,-0.4694648f,-0.3017928f),
            CameraRotation = new Rotator(-17.56532f,-4.477645E-06f,-119.5001f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo1Inventory3",
            Position = new Vector3(-782.6039f,329.5973f,217.0381f),
            Heading = 90.738f,
            CameraPosition = new Vector3(-781.3889f,331.1578f,217.9198f),
            CameraDirection = new Vector3(-0.6233696f,-0.6689038f,-0.4049421f),
            CameraRotation = new Rotator(-23.8875f,2.61452E-05f,137.018f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo1Inventory4",
            Position = new Vector3(-793.2962f,341.8432f,216.8385f),
            Heading = 97.08846f,
            CameraPosition = new Vector3(-792.295f,343.5441f,217.7679f),
            CameraDirection = new Vector3(-0.585583f,-0.6942225f,-0.4185065f),
            CameraRotation = new Rotator(-24.74033f,0f,139.8521f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo1Inventory5",
            Position = new Vector3(-796.4521f,328.0754f,217.0381f),
            Heading = 1.848104f,
            CameraPosition = new Vector3(-798.3832f,327.1673f,217.7296f),
            CameraDirection = new Vector3(0.8450741f,0.4388361f,-0.3054056f),
            CameraRotation = new Rotator(-17.78257f,0f,-62.55773f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowPentHo1Outfit1",
            Position = new Vector3(-797.7591f,327.6851f,220.4384f),
            Heading = 356.8394f,
            CameraPosition = new Vector3(-797.7917f,329.8751f,220.9311f),
            CameraDirection = new Vector3(-0.0176912f,-0.9712664f,-0.2373365f),
            CameraRotation = new Rotator(-13.72939f,5.218384E-07f,178.9565f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 227329,
            InternalInteriorCoordinates = new Vector3(-787.7805f,334.9232f,215.8384f),
            Name = "Eclipse Towers Penthouse 1",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "apa_v_mp_h_01_a",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-781.8206f,318.1305f,217.6388f),
            InteriorEgressHeading = 0.519394f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowPentHo1Exit1",
            Position = new Vector3(-781.8206f,318.1305f,217.6388f),
            Heading = 180.7443f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowPentHo1Std1",
            Position = new Vector3(-784.41f,337.2825f,216.8385f),
            Heading = 7.334802f,
            CameraPosition = new Vector3(-782.5988f,335.9488f,217.5656f),
            CameraDirection = new Vector3(-0.7117192f,0.6237311f,-0.3231336f),
            CameraRotation = new Rotator(-18.85254f,3.698905E-05f,48.76957f),
            },
            new ToiletInteract() {
            Name = "EclTowPentHo1Toilet1",
            Position = new Vector3(-807.2225f,332.3022f,220.4384f),
            Heading = 180.0627f,
            CameraPosition = new Vector3(-805.597f,333.3304f,221.3964f),
            CameraDirection = new Vector3(-0.7016724f,-0.5626833f,-0.437085f),
            CameraRotation = new Rotator(-25.91804f,4.366545E-05f,128.7267f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo1Sink1",
            Position = new Vector3(-804.8425f,332.4015f,220.4384f),
            Heading = 182.6802f,
            CameraPosition = new Vector3(-803.5584f,332.7104f,221.2338f),
            CameraDirection = new Vector3(-0.8271372f,-0.4430359f,-0.345779f),
            CameraRotation = new Rotator(-20.22936f,7.279199E-06f,118.1746f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo1Sink2",
            Position = new Vector3(-806.2828f,332.4015f,220.4384f),
            Heading = 182.6212f,
            CameraPosition = new Vector3(-803.5584f,332.7104f,221.2338f),
            CameraDirection = new Vector3(-0.8271372f,-0.4430359f,-0.345779f),
            CameraRotation = new Rotator(-20.22936f,7.279199E-06f,118.1746f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-807.2225f,332.3022f,220.4384f),
            },
            },
            new ResidenceInterior(229889,"Eclipse Towers Penthouse 2") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowPentHo2Rest1",
            Position = new Vector3(-765.1819f,322.5501f,199.4886f),
            Heading = 266.0385f,
            CameraPosition = new Vector3(-765.3815f,319.2569f,200.43f),
            CameraDirection = new Vector3(0.531084f,0.7818999f,-0.3264697f),
            CameraRotation = new Rotator(-19.05464f,-9.032658E-06f,-34.18523f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowPentHo2Inventory1",
            Position = new Vector3(-778.8326f,332.2118f,196.0859f),
            Heading = 3.658433f,
            CameraPosition = new Vector3(-774.8258f,331.3178f,196.6861f),
            CameraDirection = new Vector3(-0.8473494f,0.4478109f,-0.2854197f),
            CameraRotation = new Rotator(-16.58394f,-1.158079E-05f,62.14425f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowPentHo2Inventory2",
            Position = new Vector3(-776.7656f,332.1692f,196.0859f),
            Heading = 3.575011f,
            CameraPosition = new Vector3(-774.8258f,331.3178f,196.6861f),
            CameraDirection = new Vector3(-0.8473494f,0.4478109f,-0.2854197f),
            CameraRotation = new Rotator(-16.58394f,-1.158079E-05f,62.14425f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo2Inventory3",
            Position = new Vector3(-778.4882f,328.1311f,196.0859f),
            Heading = 271.208f,
            CameraPosition = new Vector3(-779.8337f,326.3568f,197.0108f),
            CameraDirection = new Vector3(0.644699f,0.7081782f,-0.2878313f),
            CameraRotation = new Rotator(-16.72816f,6.240505E-06f,-42.31356f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo2Inventory4",
            Position = new Vector3(-761.1121f,319.3091f,199.4887f),
            Heading = 273.1691f,
            CameraPosition = new Vector3(-762.4534f,317.5468f,200.1399f),
            CameraDirection = new Vector3(0.6407585f,0.7087319f,-0.2951737f),
            CameraRotation = new Rotator(-17.16795f,-2.680766E-05f,-42.11647f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo2Inventory5",
            Position = new Vector3(-764.5304f,329.6714f,196.086f),
            Heading = 181.1728f,
            CameraPosition = new Vector3(-762.6255f,330.6657f,196.5674f),
            CameraDirection = new Vector3(-0.8278742f,-0.5050099f,-0.2441091f),
            CameraRotation = new Rotator(-14.12919f,3.961835E-06f,121.3835f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowPentHo2Outfit1",
            Position = new Vector3(-763.2099f,329.8693f,199.4886f),
            Heading = 178.8867f,
            CameraPosition = new Vector3(-763.2371f,327.4721f,199.931f),
            CameraDirection = new Vector3(0.02311611f,0.9824839f,-0.184908f),
            CameraRotation = new Rotator(-10.65577f,-1.194538E-06f,-1.34782f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 229889,
            InternalInteriorCoordinates = new Vector3(-773.2258f,322.8252f,194.8862f),
            Name = "Eclipse Towers Penthouse 2",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "apa_v_mp_h_04_b",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-779.1475f,339.6216f,196.6867f),
            InteriorEgressHeading = 180.8226f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowPentHo2Exit1",
            Position = new Vector3(-779.1475f,339.6216f,196.6867f),
            Heading = 5.436052f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowPentHo2Std1",
            Position = new Vector3(-777.1835f,320.5009f,195.8864f),
            Heading = 225.9723f,
            CameraPosition = new Vector3(-779.3177f,320.5703f,196.6826f),
            CameraDirection = new Vector3(0.9286628f,-0.07074887f,-0.3641154f),
            CameraRotation = new Rotator(-21.35316f,-2.05112E-05f,-94.35658f),
            },
            new ToiletInteract() {
            Name = "EclTowPentHo2Toilet1",
            Position = new Vector3(-753.7158f,325.4533f,199.4887f),
            Heading = 358.0818f,
            CameraPosition = new Vector3(-755.5065f,324.4663f,200.1887f),
            CameraDirection = new Vector3(0.7437922f,0.573843f,-0.3427498f),
            CameraRotation = new Rotator(-20.0445f,-5.543828E-05f,-52.34946f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo2Sink1",
            Position = new Vector3(-756.1519f,325.429f,199.4887f),
            Heading = 0.4883329f,
            CameraPosition = new Vector3(-757.4041f,325.0437f,200.3608f),
            CameraDirection = new Vector3(0.7618926f,0.4677287f,-0.4480508f),
            CameraRotation = new Rotator(-26.61869f,-3.724482E-05f,-58.45408f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo2Sink2",
            Position = new Vector3(-754.7568f,325.4339f,199.4887f),
            Heading = 357.7901f,
            CameraPosition = new Vector3(-757.4041f,325.0437f,200.3608f),
            CameraDirection = new Vector3(0.7618926f,0.4677287f,-0.4480508f),
            CameraRotation = new Rotator(-26.61869f,-3.724482E-05f,-58.45408f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-753.7158f,325.4533f,199.4887f),
            },
            },
            new ResidenceInterior(230913,"Eclipse Towers Penthouse 3") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "EclTowPentHo3Rest1",
            Position = new Vector3(-795.6975f,335.1445f,190.7135f),
            Heading = 89.64941f,
            CameraPosition = new Vector3(-795.7505f,338.4409f,191.6141f),
            CameraDirection = new Vector3(-0.5260338f,-0.776157f,-0.347662f),
            CameraRotation = new Rotator(-20.34438f,1.274806E-05f,145.8729f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "EclTowPentHo3Inventory1",
            Position = new Vector3(-782.1838f,325.5741f,187.3131f),
            Heading = 178.1269f,
            CameraPosition = new Vector3(-786.0485f,326.5223f,187.989f),
            CameraDirection = new Vector3(0.8242152f,-0.467704f,-0.3192526f),
            CameraRotation = new Rotator(-18.61773f,1.711746E-05f,-119.5729f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "EclTowPentHo3Inventory2",
            Position = new Vector3(-784.17f,325.5173f,187.3131f),
            Heading = 184.9404f,
            CameraPosition = new Vector3(-786.0485f,326.5223f,187.989f),
            CameraDirection = new Vector3(0.8242152f,-0.467704f,-0.3192526f),
            CameraRotation = new Rotator(-18.61773f,1.711746E-05f,-119.5729f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo3Inventory3",
            Position = new Vector3(-782.474f,329.582f,187.3131f),
            Heading = 91.93946f,
            CameraPosition = new Vector3(-781.5139f,331.5724f,188.1786f),
            CameraDirection = new Vector3(-0.4784784f,-0.8034166f,-0.354373f),
            CameraRotation = new Rotator(-20.75502f,-6.391176E-06f,149.2239f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo3Inventory4",
            Position = new Vector3(-800.0526f,338.4295f,190.7136f),
            Heading = 92.91203f,
            CameraPosition = new Vector3(-798.8828f,340.2088f,191.3691f),
            CameraDirection = new Vector3(-0.5138673f,-0.7969967f,-0.3173903f),
            CameraRotation = new Rotator(-18.50518f,2.700975E-06f,147.1879f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "EclTowPentHo3Inventory5",
            Position = new Vector3(-796.3978f,328.1177f,187.3131f),
            Heading = 359.2587f,
            CameraPosition = new Vector3(-798.2268f,327.1035f,187.9133f),
            CameraDirection = new Vector3(0.7887396f,0.5419886f,-0.2900658f),
            CameraRotation = new Rotator(-16.86189f,6.244904E-06f,-55.50479f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "EclTowPentHo3Outfit1",
            Position = new Vector3(-797.8123f,327.7688f,190.7135f),
            Heading = 2.032324f,
            CameraPosition = new Vector3(-797.8375f,330.1159f,191.1557f),
            CameraDirection = new Vector3(-0.02868303f,-0.9799965f,-0.1969368f),
            CameraRotation = new Rotator(-11.35789f,6.259074E-06f,178.3235f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 230913,
            InternalInteriorCoordinates = new Vector3(-787.7805f,334.9232f,186.1134f),
            Name = "Eclipse Towers Penthouse 3",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "apa_v_mp_h_05_c",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-781.8493f,318.2509f,187.9175f),
            InteriorEgressHeading = 359.4961f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "EclTowPentHo3Exit1",
            Position = new Vector3(-781.8493f,318.2509f,187.9175f),
            Heading = 182.963f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "EclTowPentHo3Std1",
            Position = new Vector3(-784.3889f,337.233f,187.1136f),
            Heading = 1.341392f,
            CameraPosition = new Vector3(-782.1384f,335.8456f,187.9009f),
            CameraDirection = new Vector3(-0.7954648f,0.5279641f,-0.2974721f),
            CameraRotation = new Rotator(-17.30583f,-1.967363E-05f,56.42706f),
            },
            new ToiletInteract() {
            Name = "EclTowPentHo3Toilet1",
            Position = new Vector3(-807.2319f,332.2801f,190.7134f),
            Heading = 171.9976f,
            CameraPosition = new Vector3(-805.4277f,333.2918f,191.3657f),
            CameraDirection = new Vector3(-0.7593325f,-0.5652958f,-0.3222652f),
            CameraRotation = new Rotator(-18.79997f,9.0189E-06f,126.6664f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo3Sink1",
            Position = new Vector3(-804.8542f,332.3999f,190.7134f),
            Heading = 181.0351f,
            CameraPosition = new Vector3(-803.6426f,332.6292f,191.4212f),
            CameraDirection = new Vector3(-0.83117f,-0.4568655f,-0.3169074f),
            CameraRotation = new Rotator(-18.476f,-1.800343E-05f,118.7961f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "EclTowPentHo3Sink2",
            Position = new Vector3(-806.2471f,332.4004f,190.7134f),
            Heading = 176.9023f,
            CameraPosition = new Vector3(-803.6426f,332.6292f,191.4212f),
            CameraDirection = new Vector3(-0.83117f,-0.4568655f,-0.3169074f),
            CameraRotation = new Rotator(-18.476f,-1.800343E-05f,118.7961f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-807.2319f,332.2801f,190.7134f),
            },
            },


            new ResidenceInterior(145409,"Del Perro Heights Apt 20") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            Name = "DelPerHeiApt20Rest1",
            Position = new Vector3(-1471.518f,-533.7202f,63.34928f),
            Heading = 33.97481f,
            CameraPosition = new Vector3(-1469.199f,-531.8126f,64.17513f),
            CameraDirection = new Vector3(-0.9461892f,-0.1475707f,-0.2880087f),
            CameraRotation = new Rotator(-16.73878f,1.783101E-06f,98.86462f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "DelPerHeiApt20Inventory1",
            Position = new Vector3(-1458.109f,-536.7103f,68.15404f),
            Heading = 308.1834f,
            CameraPosition = new Vector3(-1459.933f,-535.8007f,68.74513f),
            CameraDirection = new Vector3(0.9105906f,-0.2885745f,-0.2958874f),
            CameraRotation = new Rotator(-17.21076f,-2.949524E-05f,-107.5839f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "DelPerHeiApt20Inventory2",
            Position = new Vector3(-1459.699f,-534.2094f,68.15404f),
            Heading = 310.7587f,
            CameraPosition = new Vector3(-1459.29f,-536.2368f,68.74316f),
            CameraDirection = new Vector3(-0.1565849f,0.9455993f,-0.285172f),
            CameraRotation = new Rotator(-16.56913f,-2.226903E-07f,9.402474f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt20Inventory3",
            Position = new Vector3(-1463.234f,-536.1485f,68.15404f),
            Heading = 39.61137f,
            CameraPosition = new Vector3(-1461.175f,-535.5031f,68.72215f),
            CameraDirection = new Vector3(-0.9405042f,-0.2023761f,-0.2729391f),
            CameraRotation = new Rotator(-15.83924f,1.774939E-06f,102.1436f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt20Inventory4",
            Position = new Vector3(-1469.179f,-545.1423f,68.15405f),
            Heading = 216.4861f,
            CameraPosition = new Vector3(-1471.185f,-545.7788f,68.7453f),
            CameraDirection = new Vector3(0.9238101f,0.2527578f,-0.2875561f),
            CameraRotation = new Rotator(-16.7117f,-1.025138E-05f,-74.69816f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt20Inventory5",
            Position = new Vector3(-1457.918f,-531.8682f,69.56528f),
            Heading = 133.5022f,
            CameraPosition = new Vector3(-1456.132f,-532.9701f,70.13641f),
            CameraDirection = new Vector3(-0.8907775f,0.3580257f,-0.2798806f),
            CameraRotation = new Rotator(-16.25308f,5.780549E-06f,68.10358f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "DelPerHeiApt20Outfit1",
            Position = new Vector3(-1468.438f,-537.8539f,63.36014f),
            Heading = 305.0954f,
            CameraPosition = new Vector3(-1466.177f,-536.2948f,63.88441f),
            CameraDirection = new Vector3(-0.8245944f,-0.5423697f,-0.1608701f),
            CameraRotation = new Rotator(-9.257405f,2.941137E-05f,123.3346f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 145409,
            InternalInteriorCoordinates = new Vector3(-1468.021f,-529.9437f,62.34918f),
            Name = "Del Perro Heights Apt 20",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-1457.749f,-520.4504f,69.5566f),
            InteriorEgressHeading = 120.3658f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "DelPerHeiApt20Exit1",
            Position = new Vector3(-1457.749f,-520.4504f,69.5566f),
            Heading = 302.6746f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "DelPerHeiApt20Std1",
            Position = new Vector3(-1470.321f,-529.9076f,68.15404f),
            Heading = 352.6274f,
            CameraPosition = new Vector3(-1468.997f,-531.7629f,68.85372f),
            CameraDirection = new Vector3(-0.5096513f,0.8277681f,-0.234639f),
            CameraRotation = new Rotator(-13.57035f,1.888331E-05f,31.62033f),
            },
            new ToiletInteract() {
            Name = "DelPerHeiApt20Toilet1",
            Position = new Vector3(-1460.363f,-530.2695f,63.3493f),
            Heading = 39.44948f,
            CameraPosition = new Vector3(-1461.164f,-531.9329f,64.19548f),
            CameraDirection = new Vector3(0.2502472f,0.8762599f,-0.4117583f),
            CameraRotation = new Rotator(-24.31534f,-1.686387E-05f,-15.93858f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "DelPerHeiApt20Sink1",
            Position = new Vector3(-1462.352f,-531.7161f,63.34929f),
            Heading = 34.19762f,
            CameraPosition = new Vector3(-1463.299f,-532.6681f,64.3065f),
            CameraDirection = new Vector3(0.3813351f,0.7931452f,-0.4748729f),
            CameraRotation = new Rotator(-28.35108f,-2.910412E-06f,-25.67771f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "DelPerHeiApt20Sink2",
            Position = new Vector3(-1461.168f,-530.909f,63.3493f),
            Heading = 36.16143f,
            CameraPosition = new Vector3(-1463.299f,-532.6681f,64.3065f),
            CameraDirection = new Vector3(0.3813351f,0.7931452f,-0.4748729f),
            CameraRotation = new Rotator(-28.35108f,-2.910412E-06f,-25.67771f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-1460.363f,-530.2695f,63.3493f),
            },
            },
            new ResidenceInterior(145921,"Del Perro Heights Apt 4") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "DelPerHeiApt04Rest1",
            Position = new Vector3(-1453.933f,-552.9336f,72.84373f),
            Heading = 125.5231f,
            CameraPosition = new Vector3(-1455.808f,-550.3955f,73.87406f),
            CameraDirection = new Vector3(0.1167331f,-0.9165349f,-0.3825404f),
            CameraRotation = new Rotator(-22.49113f,3.003191E-06f,-172.7417f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "DelPerHeiApt04Inventory1",
            Position = new Vector3(-1470.57f,-534.8392f,73.44415f),
            Heading = 310.4349f,
            CameraPosition = new Vector3(-1468.777f,-538.4162f,74.18135f),
            CameraDirection = new Vector3(-0.3192191f,0.8851675f,-0.338493f),
            CameraRotation = new Rotator(-19.78509f,-4.083005E-06f,19.83089f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "DelPerHeiApt04Inventory2",
            Position = new Vector3(-1469.52f,-536.4717f,73.44415f),
            Heading = 296.7298f,
            CameraPosition = new Vector3(-1468.777f,-538.4162f,74.18135f),
            CameraDirection = new Vector3(-0.3192191f,0.8851675f,-0.338493f),
            CameraRotation = new Rotator(-19.78509f,-4.083005E-06f,19.83089f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt04Inventory3",
            Position = new Vector3(-1473.714f,-537.3282f,73.44415f),
            Heading = 215.1499f,
            CameraPosition = new Vector3(-1475.618f,-537.8273f,74.24632f),
            CameraDirection = new Vector3(0.9160337f,0.1088544f,-0.3860478f),
            CameraRotation = new Rotator(-22.7088f,6.710026E-06f,-83.22319f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt04Inventory4",
            Position = new Vector3(-1457.401f,-550.2145f,72.87895f),
            Heading = 42.39359f,
            CameraPosition = new Vector3(-1455.524f,-549.3076f,73.67564f),
            CameraDirection = new Vector3(-0.8724361f,-0.3309723f,-0.3596006f),
            CameraRotation = new Rotator(-21.07567f,-5.261135E-05f,110.775f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "DelPerHeiApt04Inventory5",
            Position = new Vector3(-1466.103f,-526.2086f,73.44367f),
            Heading = 303.4736f,
            CameraPosition = new Vector3(-1467.838f,-524.9519f,74.05177f),
            CameraDirection = new Vector3(0.829695f,-0.4729565f,-0.2965242f),
            CameraRotation = new Rotator(-17.24895f,-2.860736E-05f,-119.6849f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "DelPerHeiApt04Outfit1",
            Position = new Vector3(-1449.405f,-548.8419f,72.84367f),
            Heading = 126.3673f,
            CameraPosition = new Vector3(-1451.419f,-550.3232f,73.18575f),
            CameraDirection = new Vector3(0.815858f,0.5551128f,-0.1619431f),
            CameraRotation = new Rotator(-9.319701f,-2.292764E-05f,-55.76849f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 145921,
            InternalInteriorCoordinates = new Vector3(-1468.021f,-529.9437f,62.34918f),
            Name = "Del Perro Heights Apt 4",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-1457.08f,-533.8014f,74.04426f),
            InteriorEgressHeading = 34.77639f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "DelPerHeiApt04Exit1",
            Position = new Vector3(-1457.08f,-533.8014f,74.04426f),
            Heading = 213.1863f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "DelPerHeiApt04Std1",
            Position = new Vector3(-1465.288f,-542.9439f,73.24418f),
            Heading = 167.6722f,
            CameraPosition = new Vector3(-1466.032f,-540.5422f,73.94208f),
            CameraDirection = new Vector3(0.3508471f,-0.8908623f,-0.2885666f),
            CameraRotation = new Rotator(-16.77216f,-1.426731E-05f,-158.5041f),
            },
            new ToiletInteract() {
            Name = "DelPerHeiApt04Toilet1",
            Position = new Vector3(-1449.02f,-556.4895f,72.84373f),
            Heading = 303.0388f,
            CameraPosition = new Vector3(-1450.695f,-555.5477f,73.65031f),
            CameraDirection = new Vector3(0.8672153f,-0.312066f,-0.3880109f),
            CameraRotation = new Rotator(-22.83079f,3.936982E-05f,-109.7912f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "DelPerHeiApt04Sink1",
            Position = new Vector3(-1450.446f,-554.4987f,72.84373f),
            Heading = 305.3961f,
            CameraPosition = new Vector3(-1451.35f,-553.5603f,73.75994f),
            CameraDirection = new Vector3(0.7972178f,-0.4071854f,-0.4456948f),
            CameraRotation = new Rotator(-26.4678f,2.670472E-05f,-117.056f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "DelPerHeiApt04Sink2",
            Position = new Vector3(-1449.611f,-555.6869f,72.84374f),
            Heading = 303.0836f,
            CameraPosition = new Vector3(-1451.35f,-553.5603f,73.75994f),
            CameraDirection = new Vector3(0.7972178f,-0.4071854f,-0.4456948f),
            CameraRotation = new Rotator(-26.4678f,2.670472E-05f,-117.056f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-1449.02f,-556.4895f,72.84373f),
            },
            },
            new ResidenceInterior(141825,"3 Alta Street Apt 10") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "AltaStApt10Rest1",
            Position = new Vector3(-260.272f,-948.3198f,71.02402f),
            Heading = 248.9149f,
            CameraPosition = new Vector3(-261.0232f,-951.3234f,71.90832f),
            CameraDirection = new Vector3(0.6727957f,0.6331171f,-0.3827645f),
            CameraRotation = new Rotator(-22.50503f,-1.478642E-05f,-46.74033f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "AltaStApt10Inventory1",
            Position = new Vector3(-272.9524f,-953.8384f,75.82875f),
            Heading = 162.5488f,
            CameraPosition = new Vector3(-270.9013f,-953.5771f,76.5802f),
            CameraDirection = new Vector3(-0.9160573f,-0.2196722f,-0.3355341f),
            CameraRotation = new Rotator(-19.60502f,-5.891043E-06f,103.485f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "AltaStApt10Inventory2",
            Position = new Vector3(-270.1936f,-954.8537f,75.82875f),
            Heading = 165.3992f,
            CameraPosition = new Vector3(-271.6598f,-953.4677f,76.61515f),
            CameraDirection = new Vector3(0.6660597f,-0.6531989f,-0.3601329f),
            CameraRotation = new Rotator(-21.10836f,4.209834E-05f,-134.4415f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "AltaStApt10Inventory3",
            Position = new Vector3(-268.308f,-951.2095f,75.82875f),
            Heading = 254.6531f,
            CameraPosition = new Vector3(-269.8145f,-952.7606f,76.51395f),
            CameraDirection = new Vector3(0.7135417f,0.6348705f,-0.2963068f),
            CameraRotation = new Rotator(-17.23591f,-2.324184E-05f,-48.33906f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "AltaStApt10Inventory4",
            Position = new Vector3(-268.795f,-940.3889f,75.82876f),
            Heading = 72.81547f,
            CameraPosition = new Vector3(-267.5614f,-938.7665f,76.47857f),
            CameraDirection = new Vector3(-0.6052395f,-0.7264922f,-0.325414f),
            CameraRotation = new Rotator(-18.99066f,6.320427E-06f,140.2024f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "AltaStApt10Inventory5",
            Position = new Vector3(-270.292f,-957.8326f,77.23998f),
            Heading = 338.7403f,
            CameraPosition = new Vector3(-272.4098f,-957.9104f,77.90544f),
            CameraDirection = new Vector3(0.9445699f,0.130385f,-0.3013096f),
            CameraRotation = new Rotator(-17.53628f,-1.566924E-06f,-82.14076f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "AltaStApt10Outfit1",
            Position = new Vector3(-265.1576f,-947.0001f,71.03483f),
            Heading = 161.5125f,
            CameraPosition = new Vector3(-265.9648f,-949.1346f,71.56094f),
            CameraDirection = new Vector3(0.3397493f,0.9073833f,-0.2474387f),
            CameraRotation = new Rotator(-14.326f,6.168225E-06f,-20.52728f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 141825,
            InternalInteriorCoordinates = new Vector3(-260.8821f,-953.5573f,70.0239f),
            Name = "3 Alta Street Apt 10",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-263.5225f,-966.6315f,77.23132f),
            InteriorEgressHeading = 336.3881f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "AltaStApt10Exit1",
            Position = new Vector3(-263.5225f,-966.6315f,77.23132f),
            Heading = 158.2155f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "AltaStApt10Std1",
            Position = new Vector3(-258.9587f,-952.3098f,75.82875f),
            Heading = 203.2703f,
            CameraPosition = new Vector3(-261.3362f,-951.5068f,76.64555f),
            CameraDirection = new Vector3(0.8947953f,-0.3557883f,-0.2697333f),
            CameraRotation = new Rotator(-15.6484f,5.763139E-06f,-111.6837f),
            },
            new ToiletInteract() {
            Name = "AltaStApt10Toilet1",
            Position = new Vector3(-267.334f,-957.6657f,71.02405f),
            Heading = 249.348f,
            CameraPosition = new Vector3(-267.5888f,-955.8449f,71.89442f),
            CameraDirection = new Vector3(0.2757764f,-0.8619617f,-0.4254049f),
            CameraRotation = new Rotator(-25.1763f,2.311313E-05f,-162.2584f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "AltaStApt10Sink1",
            Position = new Vector3(-266.6086f,-955.3453f,71.02402f),
            Heading = 250.4029f,
            CameraPosition = new Vector3(-266.3515f,-953.941f,71.92012f),
            CameraDirection = new Vector3(0.07506654f,-0.9050447f,-0.4186395f),
            CameraRotation = new Rotator(-24.74872f,2.115273E-06f,-175.2586f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "AltaStApt10Sink2",
            Position = new Vector3(-267.0625f,-956.6949f,71.02402f),
            Heading = 249.1579f,
            CameraPosition = new Vector3(-266.3515f,-953.941f,71.92012f),
            CameraDirection = new Vector3(0.07506654f,-0.9050447f,-0.4186395f),
            CameraRotation = new Rotator(-24.74872f,2.115273E-06f,-175.2586f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-267.334f,-957.6657f,71.02405f),
            },
            },
            new ResidenceInterior(141569,"3 Alta Street Apt 57") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "AltaStApt57Rest1",
            Position = new Vector3(-282.7987f,-959.9765f,86.30359f),
            Heading = 68.19323f,
            CameraPosition = new Vector3(-281.9579f,-956.9681f,87.32534f),
            CameraDirection = new Vector3(-0.6839957f,-0.640223f,-0.3496631f),
            CameraRotation = new Rotator(-20.46671f,-1.002429E-05f,133.1068f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "AltaStApt57Inventory1",
            Position = new Vector3(-270.2449f,-954.5553f,91.10832f),
            Heading = 341.2601f,
            CameraPosition = new Vector3(-272.2531f,-954.9421f,91.84678f),
            CameraDirection = new Vector3(0.8959323f,0.2847437f,-0.3409198f),
            CameraRotation = new Rotator(-19.93292f,-4.540903E-06f,-72.36879f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "AltaStApt57Inventory2",
            Position = new Vector3(-272.9691f,-953.4411f,91.10832f),
            Heading = 343.3071f,
            CameraPosition = new Vector3(-271.4101f,-954.7846f,91.89536f),
            CameraDirection = new Vector3(-0.6832214f,0.6302322f,-0.3688034f),
            CameraRotation = new Rotator(-21.64184f,1.837045E-06f,47.31026f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "AltaStApt57Inventory3",
            Position = new Vector3(-274.9763f,-957.1364f,91.10832f),
            Heading = 72.44791f,
            CameraPosition = new Vector3(-273.5804f,-955.5012f,91.73455f),
            CameraDirection = new Vector3(-0.6609916f,-0.6916662f,-0.291012f),
            CameraRotation = new Rotator(-16.91855f,-1.517075E-05f,136.2991f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "AltaStApt57Inventory4",
            Position = new Vector3(-274.5255f,-967.9532f,91.10832f),
            Heading = 258.0547f,
            CameraPosition = new Vector3(-275.7726f,-969.6298f,91.87809f),
            CameraDirection = new Vector3(0.6074609f,0.7129701f,-0.3502356f),
            CameraRotation = new Rotator(-20.50172f,4.557534E-06f,-40.43148f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "AltaStApt57Inventory5",
            Position = new Vector3(-272.9438f,-950.5449f,92.51954f),
            Heading = 163.3322f,
            CameraPosition = new Vector3(-270.8748f,-950.4706f,93.14882f),
            CameraDirection = new Vector3(-0.9432389f,-0.1356819f,-0.3031351f),
            CameraRotation = new Rotator(-17.646f,-4.479646E-07f,98.18566f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "AltaStApt57Outfit1",
            Position = new Vector3(-277.9756f,-961.2343f,86.31441f),
            Heading = 338.8039f,
            CameraPosition = new Vector3(-277.157f,-959.0605f,86.75431f),
            CameraDirection = new Vector3(-0.3547875f,-0.9130298f,-0.201252f),
            CameraRotation = new Rotator(-11.61018f,-4.358036E-06f,158.7647f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 141569,
            InternalInteriorCoordinates = new Vector3(-282.3039f,-954.7815f,85.30347f),
            Name = "3 Alta Street Apt 57",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-279.4127f,-941.5223f,92.51087f),
            InteriorEgressHeading = 160.1609f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "AltaStApt57Exit1",
            Position = new Vector3(-279.4127f,-941.5223f,92.51087f),
            Heading = 343.3716f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "AltaStApt57Std1",
            Position = new Vector3(-284.29f,-956.007f,91.10833f),
            Heading = 26.82014f,
            CameraPosition = new Vector3(-281.9352f,-956.7161f,91.78551f),
            CameraDirection = new Vector3(-0.9130197f,0.3314864f,-0.2377223f),
            CameraRotation = new Rotator(-13.75215f,1.977685E-05f,70.04576f),
            },
            new ToiletInteract() {
            Name = "AltaStApt57Toilet1",
            Position = new Vector3(-275.8356f,-950.6735f,86.3036f),
            Heading = 69.67938f,
            CameraPosition = new Vector3(-275.6149f,-952.4642f,87.25557f),
            CameraDirection = new Vector3(-0.2584846f,0.8483504f,-0.4620469f),
            CameraRotation = new Rotator(-27.51927f,-1.203372E-05f,16.9455f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "AltaStApt57Sink1",
            Position = new Vector3(-276.6593f,-952.9739f,86.30361f),
            Heading = 74.64212f,
            CameraPosition = new Vector3(-276.9427f,-954.3682f,87.20724f),
            CameraDirection = new Vector3(-0.1236235f,0.8908282f,-0.4371983f),
            CameraRotation = new Rotator(-25.92526f,4.746534E-06f,7.900684f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "AltaStApt57Sink2",
            Position = new Vector3(-276.1429f,-951.614f,86.30361f),
            Heading = 70.81513f,
            CameraPosition = new Vector3(-276.9427f,-954.3682f,87.20724f),
            CameraDirection = new Vector3(-0.1236235f,0.8908282f,-0.4371983f),
            CameraRotation = new Rotator(-25.92526f,4.746534E-06f,7.900684f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-275.8356f,-950.6735f,86.3036f),
            },
            },
            new ResidenceInterior(147969,"4 Intergrity Way Apt 35") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "IntWayApt35Rest1",
            Position = new Vector3(-12.49595f,-588.4779f,94.02556f),
            Heading = 247.4245f,
            CameraPosition = new Vector3(-13.27576f,-591.3399f,94.78937f),
            CameraDirection = new Vector3(0.7112711f,0.6325878f,-0.3064736f),
            CameraRotation = new Rotator(-17.84684f,1.79387E-06f,-48.35086f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "IntWayApt35Inventory1",
            Position = new Vector3(-25.11045f,-593.8321f,98.83028f),
            Heading = 155.8004f,
            CameraPosition = new Vector3(-23.09112f,-593.5209f,99.59695f),
            CameraDirection = new Vector3(-0.9008622f,-0.2665759f,-0.3426145f),
            CameraRotation = new Rotator(-20.03625f,-2.953523E-05f,106.4841f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "IntWayApt35Inventory2",
            Position = new Vector3(-22.27052f,-594.9371f,98.83028f),
            Heading = 165.2931f,
            CameraPosition = new Vector3(-23.91305f,-593.504f,99.43718f),
            CameraDirection = new Vector3(0.6932386f,-0.6610042f,-0.2872174f),
            CameraRotation = new Rotator(-16.69144f,-7.130635E-06f,-133.6365f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "IntWayApt35Inventory3",
            Position = new Vector3(-20.53627f,-591.2621f,98.83028f),
            Heading = 253.8847f,
            CameraPosition = new Vector3(-21.85833f,-592.9427f,99.56026f),
            CameraDirection = new Vector3(0.6431546f,0.6923689f,-0.327074f),
            CameraRotation = new Rotator(-19.09127f,2.258663E-05f,-42.8896f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "IntWayApt35Inventory4",
            Position = new Vector3(-20.9125f,-580.4717f,98.83028f),
            Heading = 70.72016f,
            CameraPosition = new Vector3(-19.60998f,-578.7156f,99.54924f),
            CameraDirection = new Vector3(-0.6098498f,-0.7188955f,-0.3335753f),
            CameraRotation = new Rotator(-19.48593f,-1.720727E-05f,139.6916f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "IntWayApt35Inventory5",
            Position = new Vector3(-22.43726f,-597.8624f,100.2415f),
            Heading = 343.3521f,
            CameraPosition = new Vector3(-24.57026f,-598.0402f,100.9485f),
            CameraDirection = new Vector3(0.933969f,0.1375803f,-0.3298084f),
            CameraRotation = new Rotator(-19.25714f,2.034844E-06f,-81.62019f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "IntWayApt35Outfit1",
            Position = new Vector3(-17.37087f,-587.0785f,94.03636f),
            Heading = 157.3442f,
            CameraPosition = new Vector3(-18.14923f,-589.2803f,94.54393f),
            CameraDirection = new Vector3(0.3406675f,0.9122009f,-0.2276734f),
            CameraRotation = new Rotator(-13.16013f,6.137604E-06f,-20.47848f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 147969,
            InternalInteriorCoordinates = new Vector3(-13.08014f,-593.6168f,93.02542f),
            Name = "4 Intergrity Way Apt 35",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            "hei_dlc_apart_high_new",
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-15.9621f,-607.2884f,100.2328f),
            InteriorEgressHeading = 339.7805f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "IntWayApt35Exit1",
            Position = new Vector3(-15.9621f,-607.2884f,100.2328f),
            Heading = 163.1726f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "IntWayApt35Std1",
            Position = new Vector3(-11.25586f,-592.3395f,98.83028f),
            Heading = 208.9811f,
            CameraPosition = new Vector3(-13.40703f,-591.6384f,99.31755f),
            CameraDirection = new Vector3(0.9205942f,-0.3063007f,-0.2422522f),
            CameraRotation = new Rotator(-14.0195f,-1.011983E-05f,-108.4034f),
            },
            new ToiletInteract() {
            Name = "IntWayApt35Toilet1",
            Position = new Vector3(-19.5546f,-597.7441f,94.02555f),
            Heading = 252.0069f,
            CameraPosition = new Vector3(-19.79958f,-596.0133f,95.08201f),
            CameraDirection = new Vector3(0.2493351f,-0.8306972f,-0.4977691f),
            CameraRotation = new Rotator(-29.85251f,1.476589E-06f,-163.2928f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "IntWayApt35Sink1",
            Position = new Vector3(-18.71841f,-595.4225f,94.02555f),
            Heading = 252.5508f,
            CameraPosition = new Vector3(-18.51176f,-594.0811f,94.90929f),
            CameraDirection = new Vector3(0.1182782f,-0.8918899f,-0.4365118f),
            CameraRotation = new Rotator(-25.88153f,-1.423433E-06f,-172.4458f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "IntWayApt35Sink2",
            Position = new Vector3(-19.2072f,-596.7625f,94.02555f),
            Heading = 249.0675f,
            CameraPosition = new Vector3(-18.51176f,-594.0811f,94.90929f),
            CameraDirection = new Vector3(0.1182782f,-0.8918899f,-0.4365118f),
            CameraRotation = new Rotator(-25.88153f,-1.423433E-06f,-172.4458f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-18.88803f,-597.8907f,93.52498f),
            },
            },
            new ResidenceInterior(147201,"4 Intergrity Way Apt 28") {
            RestInteracts = new List<RestInteract>() {
            new RestInteract() {
            StartAnimations = new List<AnimationBundle>() {
            },
            LoopAnimations = new List<AnimationBundle>() {
            },
            EndAnimations = new List<AnimationBundle>() {
            },
            Name = "IntWayApt28Rest1",
            Position = new Vector3(-37.16466f,-583.97f,78.8303f),
            Heading = 339.9454f,
            CameraPosition = new Vector3(-33.99035f,-585.1405f,79.79942f),
            CameraDirection = new Vector3(-0.5844311f,0.7171674f,-0.3796198f),
            CameraRotation = new Rotator(-22.31013f,9.228564E-06f,39.17713f),
            ButtonPromptText = "Sleep",
            },
            },
            InventoryInteracts = new List<InventoryInteract>() {
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Fridge",
            Description = "Access Drinks",
            Name = "IntWayApt28Inventory1",
            Position = new Vector3(-13.06198f,-589.3143f,79.43072f),
            Heading = 167.0125f,
            CameraPosition = new Vector3(-16.69204f,-587.3567f,80.19315f),
            CameraDirection = new Vector3(0.7653685f,-0.546774f,-0.3394839f),
            CameraRotation = new Rotator(-19.84543f,1.633822E-05f,-125.5418f),
            ButtonPromptText = "Open Fridge",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "Pantry",
            Description = "Access Food",
            Name = "IntWayApt28Inventory2",
            Position = new Vector3(-14.93999f,-588.6166f,79.43072f),
            Heading = 163.9242f,
            CameraPosition = new Vector3(-16.69204f,-587.3567f,80.19315f),
            CameraDirection = new Vector3(0.7653685f,-0.546774f,-0.3394839f),
            CameraRotation = new Rotator(-19.84543f,1.633822E-05f,-125.5418f),
            ButtonPromptText = "Open Pantry",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessWeapons = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            new ItemType() {
            },
            new ItemType() {
            },
            new ItemType() {
            },
            },
            Title = "",
            Description = "",
            Name = "IntWayApt28Inventory3",
            Position = new Vector3(-11.94945f,-585.4624f,79.43072f),
            Heading = 66.99971f,
            CameraPosition = new Vector3(-10.47478f,-583.9545f,80.17472f),
            CameraDirection = new Vector3(-0.7169776f,-0.6106874f,-0.3361607f),
            CameraRotation = new Rotator(-19.64314f,-5.439177E-06f,130.4228f),
            ButtonPromptText = "Access Items",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessWeapons = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "IntWayApt28Inventory4",
            Position = new Vector3(-32.57137f,-584.2276f,78.86552f),
            Heading = 249.758f,
            CameraPosition = new Vector3(-33.86394f,-585.8267f,79.60912f),
            CameraDirection = new Vector3(0.6186898f,0.703401f,-0.3499286f),
            CameraRotation = new Rotator(-20.48295f,1.549372E-05f,-41.33386f),
            ButtonPromptText = "Cash Drawer",
            UseNavmesh = false,
            },
            new InventoryInteract() {
            CanAccessItems = false,
            CanAccessCash = false,
            AllowedItemTypes = new List<ItemType>() {
            },
            DisallowedItemTypes = new List<ItemType>() {
            },
            Title = "",
            Description = "",
            Name = "IntWayApt28Inventory5",
            Position = new Vector3(-11.75585f,-598.9f,79.43021f),
            Heading = 159.8243f,
            CameraPosition = new Vector3(-9.712107f,-598.9153f,80.15553f),
            CameraDirection = new Vector3(-0.9368092f,-0.1051063f,-0.3336782f),
            CameraRotation = new Rotator(-19.49218f,-4.528405E-07f,96.40159f),
            ButtonPromptText = "Weapons Locker",
            UseNavmesh = false,
            },
            },
            OutfitInteracts = new List<OutfitInteract>() {
            new OutfitInteract() {
            Name = "IntWayApt28Outfit1",
            Position = new Vector3(-38.30136f,-589.6819f,78.83025f),
            Heading = 332.1915f,
            CameraPosition = new Vector3(-37.52896f,-587.5145f,79.43327f),
            CameraDirection = new Vector3(-0.3625692f,-0.8973691f,-0.2515399f),
            CameraRotation = new Rotator(-14.56865f,2.205343E-06f,157.9995f),
            ButtonPromptText = "Change Outfit",
            },
            },
            LocalID = 147201,
            InternalInteriorCoordinates = new Vector3(-22.61353f,-590.1432f,78.43091f),
            Name = "4 Intergrity Way Apt 28",
            IsTeleportEntry = true,
            Doors = new List<InteriorDoor>() {
            },
            RequestIPLs = new List<String>() {
            },
            RemoveIPLs = new List<String>() {
            },
            InteriorSets = new List<String>() {
            },
            InteriorEgressPosition = new Vector3(-23.78597f,-597.8651f,80.03111f),
            InteriorEgressHeading = 246.097f,
            InteractPoints = new List<InteriorInteract>() {
            new ExitInteriorInteract() {
            Name = "IntWayApt28Exit1",
            Position = new Vector3(-23.78597f,-597.8651f,80.03111f),
            Heading = 30.90518f,
            ButtonPromptText = "Exit",
            },
            new StandardInteriorInteract() {
            Name = "IntWayApt28Std1",
            Position = new Vector3(-22.03025f,-585.5857f,79.23075f),
            Heading = 30.90518f,
            CameraPosition = new Vector3(-20.13322f,-586.7794f,79.96764f),
            CameraDirection = new Vector3(-0.8326086f,0.4589401f,-0.3100595f),
            CameraRotation = new Rotator(-18.06281f,-3.592124E-06f,61.13612f),
            },
            new ToiletInteract() {
            Name = "IntWayApt28Toilet1",
            Position = new Vector3(-43.14818f,-583.9168f,78.83031f),
            Heading = 156.5145f,
            CameraPosition = new Vector3(-41.25925f,-583.7449f,79.72494f),
            CameraDirection = new Vector3(-0.8764492f,-0.2165945f,-0.4300276f),
            CameraRotation = new Rotator(-25.46931f,-6.619751E-06f,103.8812f),
            ButtonPromptText = "Use Toilet",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "IntWayApt28Sink1",
            Position = new Vector3(-40.81684f,-584.7047f,78.8303f),
            Heading = 160.8843f,
            CameraPosition = new Vector3(-39.37873f,-584.9852f,79.57347f),
            CameraDirection = new Vector3(-0.9313787f,-0.121525f,-0.3431697f),
            CameraRotation = new Rotator(-20.07011f,4.544862E-06f,97.43388f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            new SinkInteract() {
            Name = "IntWayApt28Sink2",
            Position = new Vector3(-42.15263f,-584.1649f,78.83798f),
            Heading = 159.9368f,
            CameraPosition = new Vector3(-39.37873f,-584.9852f,79.57347f),
            CameraDirection = new Vector3(-0.9313787f,-0.121525f,-0.3431697f),
            CameraRotation = new Rotator(-20.07011f,4.544862E-06f,97.43388f),
            ButtonPromptText = "Use Sink",
            UseNavmesh = false,
            },
            },
            ClearPositions = new List<Vector3>() {
            new Vector3(-43.14818f,-583.9168f,78.83031f),
            },
            },
        });
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

        PossibleInteriors.BankInteriors.AddRange(new List<BankInterior>()
        {
            new BankInterior(71682,"Fleeca Bank") {

                SearchLocations = new List<Vector3>() { new Vector3(-355.2124f, -47.33231f, 49.03636f) },

               IsWeaponRestricted = true, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(2121050683,new Vector3(-353.2158f,-53.87801f,49.03653f)) { ForceRotateOpen = true },//unknown door1
                   new InteriorDoor(73386408,new Vector3(-348.8109f, -47.26213f, 49.38759f)) { LockWhenClosed = true },//Front Door1
                   new InteriorDoor(3142793112,new Vector3(-351.2598f, -46.41221f, 49.38765f)) { LockWhenClosed = true },//Front Door1
                   new InteriorDoor(4163212883, new Vector3(-355.3892f, -51.06768f, 49.31105f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>() 
                {
                    new BankDrawerInteract("fleeca1Drawer1",new Vector3(-351.3789f, -51.64762f, 49.03649f), 336.6109f,"Steal from Drawer") { AutoCamera = false },
                },


                InteractPoints = new List<InteriorInteract> () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca1vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 350.4557f, -58.93706f, 49.01488f),360f-251.1714f,-.4f),
                        Heading = 251.1714f,
                        CameraPosition = new Vector3(-350.751f, -60.13727f, 50.7495f),
                        CameraDirection = new Vector3(-0.3681353f, 0.6738781f, -0.6405972f),
                        CameraRotation = new Rotator(-39.83636f, 1.223048E-05f, 28.6475f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca1vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 353.7186f, -57.80367f, 49.0148f),360f-72.45023f,-.4f),
                        Heading = 72.45023f,
                        CameraPosition = new Vector3( - 350.751f, -60.13727f, 50.7495f),
                        CameraDirection = new Vector3( - 0.3681353f, 0.6738781f, -0.6405972f),
                        CameraRotation = new Rotator( - 39.83636f, 1.223048E-05f, 28.6475f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca1vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 352.4845f, -59.79395f, 49.01487f),360f-162.354f,-.4f),
                        Heading = 162.354f,
                        CameraPosition = new Vector3( - 350.751f, -60.13727f, 50.7495f),
                        CameraDirection = new Vector3( - 0.3681353f, 0.6738781f, -0.6405972f),
                        CameraRotation = new Rotator( - 39.83636f, 1.223048E-05f, 28.6475f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },

            },
            new BankInterior(76802,"Fleeca Bank"){
               IsWeaponRestricted = true
               ,SearchLocations = new List<Vector3>() { new Vector3(145.943f, -1037.929f, 29.36783f) }//,new Vector3(150.2974f, -1046.151f, 29.34631f) }
               , Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(2121050683,new Vector3(148.2597f,-1045.38f,29.34628f)) { ForceRotateOpen = true, },
                    new InteriorDoor(3142793112,new Vector3(149.6298f, -1037.231f, 29.71915f)){ LockWhenClosed = true, } ,//Front Door1
                    new InteriorDoor(73386408,new Vector3(152.0632f, -1038.124f, 29.71909f)) { LockWhenClosed = true, } ,//Front Door2
                    new InteriorDoor(4163212883, new Vector3(145.4186f,-1041.813f,29.64255f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("fleeca2Drawer1",new Vector3(147.8368f, -1041.57f, 29.36793f), 338.927f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("fleeca2Drawer2",new Vector3(149.4326f, -1042.337f, 29.368f), 340.193f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca2vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(150.1338f, -1049.841f, 29.3464f),360f-250.8819f,-.4f),
                        Heading = 250.8819f,
                        CameraPosition = new Vector3(149.8349f, -1051.176f, 31.3536f),
                        CameraDirection = new Vector3( - 0.5062351f, 0.7241401f, -0.468345f),
                        CameraRotation = new Rotator( - 27.92692f, 1.642717E-05f, 34.9568f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca2vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(146.9373f, -1048.566f, 29.3463f),360f-70.71257f,-.4f),
                        Heading = 70.71257f,
                        CameraPosition = new Vector3(146.7075f, -1049.683f, 31.4293f),
                        CameraDirection = new Vector3(0.7448676f, 0.3106417f, -0.5904863f),
                        CameraRotation = new Rotator( - 36.19152f, 1.375267E-05f, -67.36177f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca2vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(148.2065f, -1050.615f, 29.34638f),360f-159.0868f,-.4f),
                        Heading = 159.0868f,
                        CameraPosition = new Vector3(150.5642f, -1044.893f, 31.26001f),
                        CameraDirection = new Vector3( - 0.3296868f, -0.8698641f, -0.3669374f),
                        CameraRotation = new Rotator( - 21.52686f, 1.835587E-06f, 159.2428f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
            new BankInterior(11266,"Fleeca Bank") {
                IsWeaponRestricted = true,
                SearchLocations = new List<Vector3>() { new Vector3(310.2834f, -276.4164f, 54.16457f) },
                Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(2121050683,new Vector3(311.8455f, -283.0915f, 54.16475f)) { ForceRotateOpen = true, },
                    new InteriorDoor(73386408,new Vector3(316.3925f, -276.4888f, 54.5158f)) { LockWhenClosed = true }, //Front Door1
                    new InteriorDoor(3142793112,new Vector3(313.9587f, -275.5965f, 54.51586f)) { LockWhenClosed = true }, //Front Door2
                    new InteriorDoor(4163212883, new Vector3(309.7491f, -280.1797f, 54.43926f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("fleeca3Drawer1",new Vector3(313.6212f, -280.8588f, 54.1647f), 335.8324f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("fleeca3Drawer2",new Vector3(312.5256f, -280.4068f, 54.1647f), 338.1261f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca3vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(314.826f, -288.2838f, 54.1431f),360f-250.2773f,-.4f),
                        Heading = 250.2773f,
                        CameraPosition = new Vector3(314.3101f, -289.4625f, 56.25073f),
                        CameraDirection = new Vector3( - 0.2966053f, 0.6950718f, -0.6549049f),
                        CameraRotation = new Rotator( - 40.91244f, 6.778568E-06f, 23.10921f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca3vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(311.3073f, -286.9115f, 54.14302f),360f-71.48337f,-.4f),
                        Heading = 71.48337f,
                        CameraPosition = new Vector3(314.3101f, -289.4625f, 56.25073f),
                        CameraDirection = new Vector3( - 0.2966053f, 0.6950718f, -0.6549049f),
                        CameraRotation = new Rotator( - 40.91244f, 6.778568E-06f, 23.10921f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca3vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(312.443f, -289.0419f, 54.14309f),360f-161.7028f,-.4f),
                        Heading = 161.7028f,
                        CameraPosition = new Vector3(314.3101f, -289.4625f, 56.25073f),
                        CameraDirection = new Vector3( - 0.2966053f, 0.6950718f, -0.6549049f),
                        CameraRotation = new Rotator( - 40.91244f, 6.778568E-06f, 23.10921f),
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
            new BankInterior(20226,"Fleeca Bank") {
               IsWeaponRestricted = true,
                SearchLocations = new List<Vector3>() { new Vector3(-2963.338f, 477.7827f, 15.69686f) },
                Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(2121050683,new Vector3(-2957.66f, 482.8094f, 15.67528f)) { ForceRotateOpen = true, },
                    new InteriorDoor(3142793112,new Vector3(-2965.821f, 481.6297f, 16.04816f)) { LockWhenClosed = true }, //Front Door1
                    new InteriorDoor(73386408,new Vector3(-2965.71f, 484.2195f, 16.0481f)) { LockWhenClosed = true }, //Front Door2
                    new InteriorDoor(4163212883, new Vector3(-2960.176f, 479.0105f, 15.97156f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("fleeca4Drawer1",new Vector3(-2960.644f, 482.839f, 15.69701f), 81.83675f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca4vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 2954.013f, 486.0489f, 15.67541f),360f-358.9159f,-.4f),
                        Heading = 358.9159f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca4vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 2954.152f, 482.4714f, 15.67532f),360f-171.9128f,-.4f),
                        Heading = 171.9128f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca4vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 2952.514f, 484.314f, 15.67538f),360f-264.5385f,-.4f),
                        Heading = 264.5385f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
            new BankInterior(90626,"Fleeca Bank") {
               IsWeaponRestricted = true, SearchLocations = new List<Vector3>() {new Vector3(1180.423f, 2705.902f, 38.08785f) }, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(2121050683,new Vector3(1174.963f, 2711.711f, 38.06625f)) { ForceRotateOpen = true, },
                   new InteriorDoor(3142793112,new Vector3(1176.495f, 2703.613f, 38.43911f)) { LockWhenClosed = true },
                   new InteriorDoor(73386408,new Vector3(1173.903f, 2703.613f, 38.43904f)) { LockWhenClosed = true },
                    new InteriorDoor(4163212883, new Vector3(1178.87f, 2709.365f, 38.36251f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("fleeca5Drawer1",new Vector3(1175.087f, 2708.431f, 38.08793f), 177.2366f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca5vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(1171.215f, 2715.287f, 38.06635f),360f-90.35101f,-.4f),
                        Heading = 90.35101f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca5vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(1175.178f, 2715.24f, 38.06626f),360f-271.4401f,-.4f),
                        Heading = 271.4401f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca5vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(1173.201f, 2716.751f, 38.06634f),360f-357.0772f,-.4f),
                        Heading = 357.0772f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
            new BankInterior(87810,"Fleeca Bank") {
               IsWeaponRestricted = true, SearchLocations = new List<Vector3>() {new Vector3(-1217.313f, -331.7081f, 37.7808f) }, Doors =  new List<InteriorDoor>() {
                   new InteriorDoor(2121050683,new Vector3( - 1210.374f, -335.0283f, 37.75924f)) { ForceRotateOpen = true, },
                   new InteriorDoor(3142793112,new Vector3(-1215.386f, -328.5237f, 38.13211f)) { LockWhenClosed = true },
                   new InteriorDoor(73386408,new Vector3(-1213.074f, -327.3524f, 38.13205f)) { LockWhenClosed = true },
                    new InteriorDoor(4163212883, new Vector3(-1214.906f, -334.7281f, 38.05551f)) { ForceRotateOpen = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("fleeca6Drawer1",new Vector3(-1211.815f, -332.2156f, 37.78094f), 25.86222f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("fleeca6Drawer2",new Vector3(-1213.225f, -333.1036f, 37.78089f), 22.51491f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca6vaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 1205.487f, -336.3931f, 37.75935f),360f-296.6526f,-.4f),
                        Heading = 296.6526f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca6vaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 1208.928f, -338.272f, 37.75927f),360f-116.7019f,-.4f),
                        Heading = 116.7019f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxStealItems,
                        MinItems = SafetyDepositBoxStealMinItems,
                        MaxItems = SafetyDepositBoxStealMaxItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "fleeca6vaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 1206.709f, -338.908f, 37.75932f),360f-207.8481f,-.4f),
                        Heading = 207.8481f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },

            new BankInterior(103170,"Pacific Standard Bank") {
                SearchLocations = new List<Vector3>(){new Vector3(257.4755f, 223.8576f, 106.2864f),new Vector3(252.2917f, 218.485f, 101.6834f),new Vector3(249.9998f, 209.9364f, 110.2829f)},
                IsWeaponRestricted = true,Doors =  new List<InteriorDoor>() {


                    new InteriorDoor(961976194,new Vector3(255.2283f, 223.976f, 102.3932f)) { ForceRotateOpen = true },//?
                    new InteriorDoor(1956494919,new Vector3(266.3624f, 217.5697f, 110.4328f)) { ForceRotateOpen = true },//?
                    new InteriorDoor(4072696575,new Vector3(256.3116f, 220.6579f, 106.4296f)) { ForceRotateOpen = true },//?

                    new InteriorDoor(2253282288,new Vector3(232.6054f, 214.1584f, 106.4049f)) { LockWhenClosed = true },//FRONT LEFT
                    new InteriorDoor(2253282288,new Vector3(231.5075f, 216.5148f, 106.4049f)) { LockWhenClosed = true },//FRONT RIGHT


                    new InteriorDoor(1335309163,new Vector3(260.6518f, 203.2292f, 106.4328f)) { LockWhenClosed = true },//BACK LEFT
                    new InteriorDoor(1335309163,new Vector3(258.2093f, 204.119f, 106.4328f)) { LockWhenClosed = true },//BACK RIGHT
                    new InteriorDoor(4072696575, new Vector3(256.3116f,220.6579f,106.4296f)) { LockWhenClosed = true },//teller door
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("pacstdDrawer1",new Vector3(249.3135f, 224.6261f, 106.287f), 147.2702f,"Steal from Drawer") { AutoCamera = false },
                    new BankDrawerInteract("pacstdDrawer2",new Vector3(253.0091f, 223.5203f, 106.2868f), 151.6559f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "pacstdOuterVaultL1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(258.57f, 218.4534f, 101.6834f),360f-340.07f,-.4f),
                        Heading = 340.07f,
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
                        Name = "pacstdOuterVaultL2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(260.6405f, 217.6998f, 101.6834f),360f-340.7092f,-.4f),
                        Heading = 340.7092f,
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
                        Name = "pacstdOuterVaultR1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(257.0691f, 214.541f, 101.6834f),360f-160.8612f,-.4f),
                        Heading = 160.8612f,
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
                        Name = "pacstdOuterVaultR2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(259.4067f, 213.6903f, 101.6834f),360f-159.7794f,-.4f),
                        Heading = 159.7794f,
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
                        Name = "pacstdInnerVaultL1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(263.6108f, 216.4729f, 101.6834f),360f-342.1166f,-.4f),
                        Heading = 342.1166f,
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
                        Name = "pacstdInnerVaultL2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(265.6564f, 215.8741f, 101.6834f),360f-342.004f,-.4f),
                        Heading = 342.004f,
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
                        Name = "pacstdInnerVaultT1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(266.4569f, 214.4251f, 101.6834f),360f-250.5221f,-.4f),
                        Heading = 250.5221f,
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
                        Name = "pacstdInnerVaultT2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(265.6734f, 212.6927f, 101.6834f),360f-249.6621f,-.4f),
                        Heading = 249.6621f,
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
                        Name = "pacstdInnerVaultR1",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(262.3621f, 213.0617f, 101.6834f),360f-167.3309f,-.4f),
                        Heading = 167.3309f,
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
                        Name = "pacstdInnerVaultR2",
                        Position = NativeHelper.GetOffsetPosition(new Vector3(264.2992f, 212.1149f, 101.6834f),360f-161.562f,-.4f),
                        Heading = 161.562f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },

            new BankInterior(42754,"Blaine County Savings") {
                IsWeaponRestricted = true,SearchLocations = new List<Vector3>() {new Vector3(-106.8916f, 6474.261f, 31.62672f) }, Doors =  new List<InteriorDoor>() {
                    new InteriorDoor(3110375179, new Vector3(-108.9147f,6469.105f,31.91028f)) { LockWhenClosed = true },//teller
                    new InteriorDoor(2628496933, new Vector3(-109.65f,6462.11f,31.98499f)) { LockWhenClosed = true },//FRONT 1
                    new InteriorDoor(3941780146, new Vector3(-111.48f,6463.94f,31.98499f)) { LockWhenClosed = true },//FRONT 2
                },
                BankDrawerInteracts = new List<BankDrawerInteract>()
                {
                    new BankDrawerInteract("bcsDrawer1",new Vector3(-111.1494f, 6470.298f, 31.6267f), 133.0098f,"Steal from Drawer") { AutoCamera = false },
                },
                InteractPoints = new List < InteriorInteract > () {
                    new ItemTheftInteract() {
                        PossibleItems = SafetyDepositBoxLargeStealItems,
                        MinItems = SafetyDepositBoxStealMinLargeItems,
                        MaxItems = SafetyDepositBoxStealMaxLargeItems,
                        ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
                        Name = "bcsvaultleft",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 105.9199f, 6478.416f, 31.62671f),360f-47.10064f,-.4f),
                        Heading = 47.10064f,
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
                        Name = "bcsvaultright",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 102.9088f, 6475.624f, 31.62673f),360f-226.068f,-.4f),
                        Heading = 226.068f,
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
                        Name = "bcsvaulttop",
                        Position = NativeHelper.GetOffsetPosition(new Vector3( - 103.1819f, 6478.17f, 31.62672f),360f-315.1491f,-.4f),
                        Heading = 315.1491f,
                        ButtonPromptText = "Rob",
                        UseNavmesh = false,
                        HasPreInteractRequirement = true,
                        ItemUsePreInteract = new DrillUsePreInteract(),
                    },
                },
            },
        });
    }
    private void ClothingShops()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {
                new Interior(82690, "Vangelico")
                {
                    LocalID = 82690,
                    Name = "Vangelico",
                    Doors = new List<InteriorDoor>() 
                    {
                        new InteriorDoor() 
                        {
                            Position = new Vector3(-631.1723f, -236.7114f, 38.06244f),
                            LockWhenClosed = true,
                        },
                        new InteriorDoor() 
                        {
                            Position = new Vector3(-630.4095f, -237.7712f, 38.0971f),
                            LockWhenClosed = true,
                        },
                    },
                    IsWeaponRestricted = true,
                    InteractPoints = new List<InteriorInteract>() 
                    {
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngFrontR1",
                            Position = new Vector3( - 626.624f, -238.5478f, 38.05702f),
                            Heading = 211.9446f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngFrontR2",
                            Position = new Vector3( - 625.6374f, -237.7162f, 38.05702f),
                            Heading = 207.4052f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngMidL1",
                            Position = new Vector3( - 627.9446f, -233.8257f, 38.05702f),
                            Heading = 212.1647f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngMidL2",
                            Position = new Vector3( - 626.9693f, -233.037f, 38.05702f),
                            Heading = 212.0368f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngMidR1",
                            Position = new Vector3( - 626.7f, -235.4685f, 38.05702f),
                            Heading = 33.27945f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngMidR2",
                            Position = new Vector3( - 625.7751f, -234.5709f, 38.05702f),
                            Heading = 32.57174f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter1",
                            Position = new Vector3( - 623.116f, -233.0139f, 38.05702f),
                            Heading = 306.5918f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter2",
                            Position = new Vector3( - 620.101f, -233.5278f, 38.05702f),
                            Heading = 37.02949f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter3",
                            Position = new Vector3( - 619.5614f, -230.3965f, 38.05702f),
                            Heading = 126.2357f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter4",
                            Position = new Vector3( - 621.0455f, -228.5942f, 38.05702f),
                            Heading = 122.6166f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter5",
                            Position = new Vector3( - 624.0377f, -228.1052f, 38.05702f),
                            Heading = 217.7205f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngCenter6",
                            Position = new Vector3( - 624.5729f, -231.036f, 38.05702f),
                            Heading = 307.6254f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter1",
                            Position = new Vector3( - 624.9713f, -227.9358f, 38.05702f),
                            Heading = 35.42755f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter2",
                            Position = new Vector3( - 623.9496f, -227.0906f, 38.05702f),
                            Heading = 34.9143f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter3",
                            Position = new Vector3( - 620.6199f, -226.6056f, 38.05702f),
                            Heading = 306.7109f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter4",
                            Position = new Vector3( - 619.5996f, -227.6832f, 38.05702f),
                            Heading = 297.4792f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter5",
                            Position = new Vector3( - 618.4504f, -229.4612f, 38.05702f),
                            Heading = 296.6827f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter6",
                            Position = new Vector3( - 617.5781f, -230.695f, 38.05702f),
                            Heading = 296.4518f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter7",
                            Position = new Vector3( - 619.2208f, -233.6226f, 38.05702f),
                            Heading = 216.0172f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
                        new ItemTheftInteract() {
                            PossibleItems = new List < TheftInteractItem > () {
                                new TheftInteractItem() {
                                    ModItemName = "Fake Gold Ring",
                                    MinItems = 6,
                                    MaxItems = 6,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Gold Ring",
                                    MinItems = 8,
                                    MaxItems = 8,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Fake Silver Ring",
                                    MinItems = 3,
                                    MaxItems = 3,
                                    Percentage = 100,
                                },
                                new TheftInteractItem() {
                                    ModItemName = "Silver Ring",
                                    MinItems = 5,
                                    MaxItems = 5,
                                    Percentage = 100,
                                },
                            },
                            MinItems = 4,
                            MaxItems = 5,
                            ViolatingCrimeID = "JewelRobbery",
                            Name = "vngOuter8",
                            Position = new Vector3( - 620.0914f, -234.3648f, 38.05702f),
                            Heading = 213.1084f,
                            ButtonPromptText = "Rob",
                            UseNavmesh = false,
                        },
            },
            }
        });
    }
    private void Other()
    {
        PossibleInteriors.GeneralInteriors.AddRange(new List<Interior>()
        {


            new Interior(78338,"Maze Bank Arena",new List<string>() { "sp1_10_real_interior" },new List<string>() { "sp1_10_fake_interior" }),   
            new Interior(31746,"O'Neil Ranch",
                new List<string>() { "farm", "farmint", "farm_lod", "farm_props","des_farmhs_startimap","des_farmhs_start_occl" },
                new List<string>() { "farm_burnt", "farm_burnt_lod", "farm_burnt_props", "farmint_cap", "farmint_cap_lod", "des_farmhouse", "des_farmhs_endimap", "des_farmhs_end_occl"})

            { SearchLocations = new List<Vector3>()
            {
                new Vector3(2452.9f, 4973.177f, 46.81016f),
                new Vector3(2438.356f, 4963.151f, 46.8106f),
                new Vector3(2432.671f, 4966.289f, 42.34761f),
                new Vector3(2440.33f, 4976.595f, 46.8106f),
                new Vector3(2448.677f, 4983.571f, 46.84687f),
                new Vector3(2442.995f, 4971.742f, 51.56486f),
                new Vector3(2447.533f, 4985.781f, 51.56487f),
            }, }
            
            
            ,
            new Interior(3330,"Lifeinvader",new List<string>() { "facelobby","facelobby_lod" },new List<string>() { "facelobbyfake","facelobbyfake_lod" }) { SearchLocations = new List<Vector3>() { 
                new Vector3(-1075.071f, -250.3029f, 37.76332f),
new Vector3(-1077.251f, -251.8201f, 44.02116f),
new Vector3(-1051.115f, -237.8116f, 44.02106f), } } ,
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


            new Interior(104450,"Liquor Ace") { SearchLocations = new List<Vector3>() { new Vector3(1391.579f, 3608.259f, 34.98093f), new Vector3(1390.555f, 3613.684f, 38.94193f), new Vector3(1392.507f, 3602.898f, 38.94189f) } },


            new Interior(89602,"Yellow Jacket Inn") { SearchLocations = new List<Vector3>() { new Vector3(1993.187f, 3045.516f, 47.21509f) } },
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
            //new Interior(76290,"Del Perro Heights, Apt 4") { IsSPOnly = true } ,
            //new Interior(108290,"Low End Apartment") { IsSPOnly = true } ,
            new Interior(69890,"IAA Office"),// { IsSPOnly = true } ,
           // new Interior(25602,"Del Perro Heights, Apt 7") { IsSPOnly = true } ,
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
           // ,new Interior(145921, "Del Perro Heights, Apt 4") { IsMPOnly = true }
            //,new Interior(145665, "Del Perro Heights, Apt 7") { IsMPOnly = true }
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



    private ItemTheftInteract GenerateSafeDrillingInteract(string name, Vector3 position, float heading)
    {
        int minItems = RandomItems.GetRandomNumberInt(10, 25);
        int maxItems = minItems + RandomItems.GetRandomNumberInt(2, 10);
        return new ItemTheftInteract(name, position, heading, "Drill Safe")//Looking at the front counter, mostly candy bars and chips
        {
            MinItems = 1,
            MaxItems = 3,
            SpawnPercent = 100,
            ViolatingCrimeID = StaticStrings.ArmedRobberyCrimeID,
            UseNavmesh = false,
            HasPreInteractRequirement = true,
            ItemUsePreInteract = new DrillUsePreInteract(),
            PossibleItems = new List<TheftInteractItem>()
                        {
                            new TheftInteractItem("Marked Cash Stack",1,1,100),
                            new TheftInteractItem("Marked Cash Stack",1,1,65),
                            new TheftInteractItem("Marked Cash Stack",1,1,40),
                        }
        };
    }


    private ItemTheftInteract Generate247Interact1(string name,Vector3 position,float heading)
    {
        int minItems = RandomItems.GetRandomNumberInt(10, 25);
        int maxItems = minItems + RandomItems.GetRandomNumberInt(2, 10);
        return new ItemTheftInteract(name, position, heading, "Shoplift")//Looking at the front counter, mostly candy bars and chips
        {
            MinItems = minItems,
            MaxItems = maxItems,
            ViolatingCrimeID = StaticStrings.ShopliftingCrimeID,
            PossibleItems = new List<TheftInteractItem>()
                        {
                            new TheftInteractItem("Sticky Rib Phat Chips",1,1,100),
                            new TheftInteractItem("Habanero Phat Chips",1,1,100),
                            new TheftInteractItem("Supersalt Phat Chips",1,1,100),
                            new TheftInteractItem("Big Cheese Phat Chips",1,1,100),
                            new TheftInteractItem("Ego Chaser Energy Bar",1,1,100),
                            new TheftInteractItem("P's & Q's",1,1,100),
                            new TheftInteractItem("Meteorite Bar",1,1,100),

                            new TheftInteractItem("Redwood Regular",20,20,100),
                            new TheftInteractItem("Redwood Mild",20,20,100),
                            new TheftInteractItem("Debonaire",20,20,100),
                            new TheftInteractItem("Debonaire Menthol",20,20,100),
                            new TheftInteractItem("Caradique",20,20,100),
                            new TheftInteractItem("69 Brand",20,20,100),
                            new TheftInteractItem("Estancia Cigar",10,10,100),
                        }
        };
    }
    private ItemTheftInteract Generate247Interact2(string name, Vector3 position, float heading)
    {
        int minItems = RandomItems.GetRandomNumberInt(10, 25);
        int maxItems = minItems + RandomItems.GetRandomNumberInt(2, 10);
        return new ItemTheftInteract(name, position, heading, "Shoplift")//Front of first stand
        {
            MinItems = minItems,
            MaxItems = maxItems,
            ViolatingCrimeID = StaticStrings.ShopliftingCrimeID,
            PossibleItems = new List<TheftInteractItem>()
                        {
                            new TheftInteractItem("Can of eCola",1,1,20),
                            new TheftInteractItem("Can of Sprunk",1,1,20),
                            new TheftInteractItem("Can of Orang-O-Tang",1,1,45),
                        },
            //CameraPosition = new Vector3(546.7435f, 2670.579f, 43.03606f),
            //CameraDirection = new Vector3(-0.3920216f, -0.8322931f, -0.3919277f),
            //CameraRotation = new Rotator(-23.0745f, 1.113623E-05f, 154.7789f),
        };
    }
    private ItemTheftInteract Generate247Interact3(string name, Vector3 position, float heading)
    {
        int minItems = RandomItems.GetRandomNumberInt(10, 25);
        int maxItems = minItems + RandomItems.GetRandomNumberInt(2, 10);
        return new ItemTheftInteract(name, position, heading, "Shoplift")///back of first
        {
            MinItems = minItems,
            MaxItems = maxItems,
            ViolatingCrimeID = StaticStrings.ShopliftingCrimeID,
            PossibleItems = new List<TheftInteractItem>()
                        {
                            new TheftInteractItem("Strawberry Rails Cereal",1,1,20),
                            new TheftInteractItem("Crackles O' Dawn Cereal",1,1,20),
                            new TheftInteractItem("White Bread",1,1,45),
                        }
        };
    }
    private ItemTheftInteract Generate247Interact4(string name, Vector3 position, float heading)
    {
        int minItems = RandomItems.GetRandomNumberInt(10, 25);
        int maxItems = minItems + RandomItems.GetRandomNumberInt(2, 10);
        return new ItemTheftInteract(name, position, heading, "Shoplift")//Looking at the front counter, mostly candy bars and chips
        {
            MinItems = minItems,
            MaxItems = maxItems,
            IncrementGameTimeMin = 1500,
            IncrementGameTimeMax = 2000,
            ViolatingCrimeID = StaticStrings.ShopliftingCrimeID,
            IntroAnimationDictionary = "anim@scripted@heist@ig1_table_grab@cash@male@",
            IntroAnimation = "enter",
            LoopAnimationDictionary = "anim@scripted@heist@ig1_table_grab@cash@male@",
            LoopAnimation = "grab",
            RightHandAnimationPoints = new List<AnimationPoint>() {
                            new AnimationPoint(0, 0.05f,true),
                            new AnimationPoint(1, 0.08f, false),
                            new AnimationPoint(2, 0.15f, true),
                            new AnimationPoint(3, 0.2f, false),
                            new AnimationPoint(4, 0.25f, true),
                            new AnimationPoint(5, 0.3f, false),
                            new AnimationPoint(6, 0.35f, true),
                            new AnimationPoint(7, 0.39f, false),
                            new AnimationPoint(8, 0.43f, true),
                            new AnimationPoint(9, 0.5f, false),
                            new AnimationPoint(10, 0.55f, true),
                            new AnimationPoint(11, 0.6f, false),
                            new AnimationPoint(12, 0.65f, true),
                            new AnimationPoint(13, 0.7f, false),
                            new AnimationPoint(14, 0.75f, true),
                            new AnimationPoint(15, 0.8f, false),
                            new AnimationPoint(16, 0.85f, true),
                            new AnimationPoint(17, 0.9f, false),
                            new AnimationPoint(18, 0.95f, true),
                            new AnimationPoint(19, 0.98f, false),
                        },
            PossibleItems = new List<TheftInteractItem>()
                        {
                            new TheftInteractItem("Bottle of Pride",1,1,100),
                            new TheftInteractItem("Bottle of Barracho",1,1,100),
                            new TheftInteractItem("Bottle of A.M.",1,1,100),
                            new TheftInteractItem("Bottle of Dusche",1,1,100),
                        }
        };
    }
}

