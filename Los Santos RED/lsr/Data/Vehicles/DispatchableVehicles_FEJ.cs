using System;
using System.Collections.Generic;
using System.Linq;

public class DispatchableVehicles_FEJ
{
    public DispatchableVehicles DispatchableVehicles;

    public string BuffaloUnmarked = "fbi";
    public string GrangerUnmarked = "fbi2";
    public string WashingtonUnmarked = "polwashingtonunmarked";
    public string PoliceBuffalo = "police2";
    public string PoliceGranger = "sheriff2";
    public string PoliceMerit = "polmeritliv";
    public string PoliceEsperanto = "polesperantoliv";
    public string PoliceTransporter = "poltransporterliv";

    public string PoliceStanier = "polstanier2";//"police";
    public string StanierUnmarked = "police4";
    public string PoliceGauntlet = "polgauntlet";
    public string PoliceTorrence = "polgemini";//"police3";
    public string PoliceBison = "polbisonliv";
    public string PoliceStanierOld = "polstanieroldliv";
    public string PoliceLandstalkerXL = "pollandstalkerxlliv";
    public string PoliceBuffaloS = "polbuffalosliv";
    public string PoliceGresley = "polgresleyliv";
    public string PoliceTerminus = "polterminusliv";
    public string PoliceBuffaloSTX = "polbuffalostxliv";
    public string PoliceCaracara = "polcaracaraliv";
    public string PoliceFugitive = "polfugitiveliv";
    public string PoliceGranger3600 = "polgranger3600liv";
    public string PolicePatriot = "polpatriotliv";
    public string PoliceSeminole = "polseminoleliv";
    public string PoliceAleutian = "polaleutianliv";
    public string PoliceVSTR = "polvstrliv";
    public string PoliceReblaGTS = "polreblagtsliv";
    public string PoliceRiata = "polriataliv";
    public string PoliceKurumaUnmarked = "polkurumaunmarked";
    public string PoliceOracle = "poloracleliv";
    public string PoliceRadius = "polradiusliv";

    public string PolicePurge = "polpurge";
    public string PoliceRaiden = "polraiden";
    public string PoliceScout = "polscoutgresk";
    public string PoliceBeaverRam = "polbeaverram";

    //Bike/Atv
    public string PoliceSanchez = "polsanchezliv";
    public string PoliceVerus = "polverusliv";
    public string PoliceVindicator = "polvindicatorliv";
    public string PoliceThrust = "polthrustliv";
    public string PoliceBike = "policebikeold";
    public string PoliceBicycle = "scorcher";

    //Heli
    public string PoliceMaverick1stGen = "polmaverickoldliv";
    public string PoliceAnnihilator = "annihilatorliv";
    public string PoliceMaverick = "polmavliv";
    public string PoliceFrogger = "polfroggerliv";
    public string PoliceBuzzard = "polbuzzardliv";
    public string PoliceValkrie = "polvalkyrieliv";

    //Military
    public string MilitaryHumvee = "insurgent3";
    public string MilValkyrie = "milvalkyrie2";

    //Service
    public string FireTruck = "firetruckliv";
    public string Ambulance = "ambulanceliv";

    public string SecurityTorrence = "servinterceptor";
    public string ServiceDilettante = "servdilettante";
    public string SecurityStanier = "servstanier2";
    public string TaxiVivanite = "taxvivaniteliv";
    public string TaxiMinivan = "servminivan";
    public string ServiceStanierOld = "servstanierold";

    //Civilian
    public string CivilianEsperanto = "civesperanto";
    public string CivilianInterceptor = "civinterceptor";
    public string CivilianStanierSecondGen = "civstanier2";
    public string CivilianMerit = "civmerit";

    //Modkits
    //981_polpurge_modkit
    //983_polgranger3600liv_modkit
    //984_polcaracaraliv_modkit
    //985_police3_modkit
    //986_polbuffalostxliv_modkit
    //987_polaleutianliv_modkit
    //988_polfugitiveliv_modkit
    //989_pollandstalkerxlliv_modkit
    //990_polgresleyliv_modkit
    //991_polbisonliv_modkit
    //992_polterminusliv_modkit
    //993_police_modkit
    //994_polbuffalosliv_modkit
    //995_firetruckliv_modkit
    //996_polradiusliv_modkit
    //978_ambulanceliv_modkit
    //998_annihilatorliv_modkit
    //979_polvindicatorliv_modkit
    //977_polmavliv_modkit
    //976_polfroggerliv_modkit
    //975_polthrustliv_modkit
    //974_polverusliv_modkit
    //973_polsanchezliv_modkit
    //972_polvstrliv_modkit
    //971_taxivivaniteliv_modkit
    //970_servstanier2_modkit
    //969_servinterceptor_modkit
    //968_servdilettante_modkit
    //967_polriataliv_modkit
    public List<int> DefaultOptionalColors { get; private set; } = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 61, 62, 64, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 112 };

    public List<string> LibertyPlates { get; private set; } = new List<string>() { "Liberty", };
    public List<string> LibertyPolicePlates { get; private set; } = new List<string>() { "Liberty Police", "Liberty", };
    public List<string> AlderneyPlates { get; private set; } = new List<string>() { "Alderney", "Liberty", };
    public List<string> NorthYanktonPlates { get; private set; } = new List<string>() { "North Yankton", };

    public List<string> USGovernmentPlates { get; private set; } = new List<string>() { "US Gov", };
    public DispatchableVehicles_FEJ(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }
    public DispatchableVehicles_FEJ_2008 DispatchableVehicles_FEJ_2008 { get; private set; }
    public DispatchableVehicles_FEJ_Modern DispatchableVehicles_FEJ_Modern { get; private set; }
    public DispatchableVehicles_FEJ_Stanier DispatchableVehicles_FEJ_Stanier { get; private set; }
    public DispatchableVehicles_FEJ_LC DispatchableVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> ArmyVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USMCVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USAFVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> CoastGuardVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> PoliceHeliVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SheriffHeliVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSFDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSCOFDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> BCFDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SanFireVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSFDEMTVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSCOFDEMSVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> BCFDEMSVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SAMSVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> MRHVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSMCVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> MarshalsServiceVehicles_FEJ { get; private set; }
    public void DefaultConfig()
    {
        SetupShared();

        //DispatchableVehicles_FEJ_2015 = new DispatchableVehicles_FEJ_2015(this);
        //DispatchableVehicles_FEJ_2015.DefaultConfig();


        DispatchableVehicles_FEJ_2008 = new DispatchableVehicles_FEJ_2008(this);
        DispatchableVehicles_FEJ_2008.DefaultConfig();

        DispatchableVehicles_FEJ_Modern = new DispatchableVehicles_FEJ_Modern(this);
        DispatchableVehicles_FEJ_Modern.DefaultConfig();

        DispatchableVehicles_FEJ_Stanier = new DispatchableVehicles_FEJ_Stanier(this);
        DispatchableVehicles_FEJ_Stanier.DefaultConfig();

        DispatchableVehicles_FEJ_LC = new DispatchableVehicles_FEJ_LC(this);
        DispatchableVehicles_FEJ_LC.DefaultConfig();
    }
    private void SetupShared()
    {
        Fire();
        EMT();
        PoliceMil();
    }
    private void PoliceMil()
    {
        PoliceHeliVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //new DispatchableVehicle(PoliceMaverick, 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 0 },RequiredPrimaryColorID = 134, RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            //new DispatchableVehicle(PoliceFrogger, 1,50) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 134, RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            //new DispatchableVehicle("annihilator", 1,50) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 1 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            //Create_PoliceMaverick1stGen(1,5,0,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),
            //Create_PoliceMaverick1stGen(1,5,1,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",0),

            Create_PoliceFrogger(1,50,2,false,134,0,0,4,3,4,"Pilot","", true),
            Create_PoliceMaverick(1,100,0,false,134,0,0,4,3,4,"Pilot","", true),
            Create_PoliceBuzzard(1,25,0,false,134,134,0,4,3,4,"Pilot","", true),
        };

        SheriffHeliVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //new DispatchableVehicle(PoliceFrogger, 1,200) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 4 },MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            //new DispatchableVehicle(PoliceMaverick, 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 1 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            //Create_PoliceMaverick1stGen(1,5,4,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),


            Create_PoliceFrogger(1,200,4,false,-1,-1,0,4,3,4,"Pilot","", true),
            Create_PoliceMaverick(1,100,1,false,-1,-1,0,4,3,4,"Pilot","", true),
            Create_PoliceBuzzard(1,25,1,false,134,134,0,4,3,4,"Pilot","", true),
            Create_PoliceValkyrie(1,25,4,false,134,134,0,4,3,4,"Pilot","", true),
        };

        ArmyVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Unarmed,-1,6,10,2,4,"",""),
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Armed,-1,6,10,2,4,"",""),

            //Heavy
            new DispatchableVehicle("rhino", 1, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InArmedMilitaryVehicle, 50) } },
    
            //Heli
            new DispatchableVehicle("valkyrie2", 1,20) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_PoliceAnnihilator(1,45,3,false,153,6,10,3,4,"Pilot","",true),
            Create_PoliceValkyrie(1,25,3,false,153,153,6,10,3,4,"Pilot","", true),


            new DispatchableVehicle("buzzard",1,20) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},
            new DispatchableVehicle("hunter",1,15) { RequiredPedGroup = "Pilot", RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 2, MaxOccupants = 2, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,50)  } },
        };

        USMCVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Unarmed,-1,6,10,2,4,"",""),
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Armed,-1,6,10,2,4,"",""),

            //Heavy
            new DispatchableVehicle("rhino", 1, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InArmedMilitaryVehicle, 50) } },
    
            //HELI
            //new DispatchableVehicle("cargobob",1,15) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},
            Create_MilValkyrie(1,25,-1,false,25,25,0,6,4,6,"Pilot","", true),
            Create_PoliceValkyrie(1,25,1,false,25,25,0,6,3,4,"Pilot","", true),

            //Boat
            new DispatchableVehicle("dinghy5", 1, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },
        };

        USAFVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Unarmed,-1,6,10,2,4,"",""),
            Create_MilitaryHumvee(50,50,-1,false,MilitaryVehicleType.Armed,-1,6,10,2,4,"",""),
            
            //HELI
            Create_PoliceAnnihilator(1,45,4,false,153,6,10,3,4,"Pilot","",true),
            Create_PoliceValkyrie(1,25,2,false,153,153,6,10,3,4,"Pilot","", true),
            //JETS
            new DispatchableVehicle("lazer",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150), new SpawnAdjustmentAmount(eSpawnAdjustment.InArmedMilitaryVehicle, 150) } },
            new DispatchableVehicle("hydra",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150), new SpawnAdjustmentAmount(eSpawnAdjustment.InArmedMilitaryVehicle, 150) } },
            new DispatchableVehicle("strikeforce",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150), new SpawnAdjustmentAmount(eSpawnAdjustment.InArmedMilitaryVehicle, 150) } },
        };

        CoastGuardVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dinghy5", 50, 50) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 1,MaxOccupants = 2, MaxWantedLevelSpawn = 2, },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle("seashark2", 50, 50) { RequiredLiveries = new List<int>() { 2,3 }, MaxOccupants = 1 },
            //new DispatchableVehicle(PoliceFrogger,1,50) { RequiredLiveries = new List<int>() { 1 },MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
            Create_PoliceFrogger(1,50,1,false,-1,-1,0,4,2,3,"","Helicopter",false),
            Create_PoliceAnnihilator(1,100,0,false,-1,0,4,2,3,"","Helicopter",false),
        };

        MarshalsServiceVehicles_FEJ = DispatchableVehicles.MarshalsServiceVehicles.Copy();//for now
        //MarshalsServiceVehicles_FEJ.Add(Create_PoliceBuffaloS(25, 25, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceFugitive(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceInterceptor(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "", -1, 20));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceGresley(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBison(10, 10, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceKuruma(5, 5, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
    }
    private void EMT()
    {
        LSFDEMTVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(1, 100) } } }, }
        };
        LSCOFDEMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(1, 100) } } }, }
        };
        BCFDEMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(3, 100) } } }, }
        };
        SAMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(4, 100) } } }, }
        };
        MRHVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(0, 100) } } }, } };
        LSMCVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(Ambulance, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(2, 100) } } }, } };

    }
    private void Fire()
    {
        LSFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(FireTruck, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(0, 100) } } },  MinOccupants = 2, MaxOccupants = 4 }
        };
        LSCOFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(FireTruck, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(2, 100) } } }, MinOccupants = 2, MaxOccupants = 4 }
        };
        BCFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(FireTruck, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(1, 100) } } },MinOccupants = 2, MaxOccupants = 4 }
        };
        SanFireVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(FireTruck, 100, 100) { VehicleMods = new List<DispatchableVehicleMod>() { new DispatchableVehicleMod(48, 100) { DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() { new DispatchableVehicleModValue(3, 100) } } },MinOccupants = 2, MaxOccupants = 4 }
        };
    }


    public DispatchableVehicle Create_PoliceBeaverRam(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBeaverRam, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredWheelColorID = requiredColor;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    public DispatchableVehicle Create_PoliceFrogger(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor, int requiredSecondaryColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceFrogger, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);

        toReturn.RequiredPrimaryColorID = requiredColor;
        toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceAnnihilator(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceAnnihilator, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceMaverick(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor,int requiredSecondaryColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceMaverick, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        toReturn.RequiredPrimaryColorID = requiredColor;
        toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBuzzard(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor, int requiredSecondaryColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuzzard, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        toReturn.RequiredPrimaryColorID = requiredColor;
        toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceValkyrie(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor, int requiredSecondaryColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceValkrie, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        toReturn.RequiredPrimaryColorID = requiredColor;
        toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        return toReturn;
    }
    public DispatchableVehicle Create_MilValkyrie(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int requiredColor, int requiredSecondaryColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, bool RequiredGroupIsDriverOnly)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(MilValkyrie, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        toReturn.RequiredGroupIsDriverOnly = RequiredGroupIsDriverOnly;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        toReturn.RequiredPrimaryColorID = requiredColor;
        toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        return toReturn;
    }

    public DispatchableVehicle Create_PoliceScout(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceScout(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceScout(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceScout, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked || PoliceVehicleType == PoliceVehicleType.MarkedOriginalLightbar)//2015 syle marked!
        {
            //1-2 extra ram bar stuff 12 = main rambar, 3-5 = siren,6 = top antenna, 7 = rear alprs, 8 front alprs, 9 partition,10 = searchlights, 

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),





                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
      
        else if (PoliceVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),





                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),





                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),


                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),





                new DispatchableVehicleExtra(6, true, 85),
                new DispatchableVehicleExtra(7, true, 55),
                new DispatchableVehicleExtra(8, true, 55),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(12, true, 45),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, true, 45),
                new DispatchableVehicleExtra(12, true, 15),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }



    public DispatchableVehicle Create_PoliceRaiden(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceRaiden(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceRaiden(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceRaiden, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)//2015 syle marked!
        {
            //1 = Searchlught, 2 = ram bar, 3-5 = siren,6 = top antenna alprs, 7 = rear alprs, 8 front alprs, 9 partition,10 = shotguns, 11 = computer,

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),
                new DispatchableVehicleExtra(8, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),
                new DispatchableVehicleExtra(8, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),
                new DispatchableVehicleExtra(8, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),
                new DispatchableVehicleExtra(8, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

   

                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(8, false, 100),

                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PolicePurge(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PolicePurge(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PolicePurge(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PolicePurge, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)//2015 syle marked!
        {
            //1 = Searchlught, 2 = ram bar, 3-5 = siren,6 = top antenna alprs, 7 = rear alprs, 9 partition,10 = shotguns, 11 = computer, 12 = radio

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    public DispatchableVehicle Create_PoliceStanier(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int hubcapWheelsPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceStanier, ambientPercent, wantedPercent);
        toReturn.RequiredDashColorID = requiredDashboardColor;
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //            new ModKitDescription("Pushbar - 42", 42),
       //new ModKitDescription("Aerials - 43", 43),
         //   new ModKitDescription("Searchlights - 44", 44),
        //            //3 = new flat siren,4 = valor siren, 5 = old libertybar, 6 = searchlights, 11 = computer, 9 = DIVIDER
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(3, false, 100), 
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(11, true, 100),
            };

        }
        else if (policeVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked || policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }

        if(policeVehicleType != PoliceVehicleType.Detective)
        {
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(42, 35)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,10),
                            new DispatchableVehicleModValue(1,40),
                            new DispatchableVehicleModValue(2,40),
                            new DispatchableVehicleModValue(3,10),
                            new DispatchableVehicleModValue(4,10),
                        },
            });
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(43, 35)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,20),
                            new DispatchableVehicleModValue(1,20),
                            new DispatchableVehicleModValue(2,20),
                            new DispatchableVehicleModValue(3,20),
                            new DispatchableVehicleModValue(4,20),
                            new DispatchableVehicleModValue(5,20),
                            new DispatchableVehicleModValue(6,20),
                        },
            });
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        //if(useLSPDWheels)
        //{
        //    if( toReturn.RequiredVariation == null)
        //    {
        //        toReturn.RequiredVariation = new VehicleVariation();
        //    }
        //    toReturn.RequiredVariation.WheelType = 0;
        //    toReturn.RequiredVariation.VehicleMods.Add(new VehicleMod(23, 0));
        //    toReturn.RequiredVariation.PrimaryColor = -1;
        //    toReturn.RequiredVariation.SecondaryColor = -1;
        //    toReturn.RequiredVariation.DashboardColor = -1;
        //}
        if (hubcapWheelsPercentage > 0)
        {
            toReturn.WheelType = 0;//sport
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, hubcapWheelsPercentage)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(50,100),
                        },
            });
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVerus(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVerus, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Extras 1 - shovels on front, 2 - front bar
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(4, true, 55),
            };

        if (offroadAdditionalPercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>
            {
                new SpawnAdjustmentAmount(eSpawnAdjustment.OffRoad, offroadAdditionalPercent),
                new SpawnAdjustmentAmount(eSpawnAdjustment.ATV, offroadAdditionalPercent),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceRiata(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        //12 = blank livery
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceRiata, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //1 = siren, 2 = roof, 3 = flat lightbar, 4 = valor lightbar,5 = bar, 6 = searchlights, 10 = shotugns, 11 = computer, 12 =radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);

        if (offroadAdditionalPercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>
            {
                new SpawnAdjustmentAmount(eSpawnAdjustment.OffRoad, offroadAdditionalPercent)
            };
        }

        return toReturn;
    }
    public DispatchableVehicle Create_PoliceKuruma(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceKurumaUnmarked, ambientPercent, wantedPercent);
        //Extras 1 - searchlights, 2 = wing, 3 = front bar, 9 = divider
        if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, false, 99),
                new DispatchableVehicleExtra(3, true, 15),
                new DispatchableVehicleExtra(9, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVSTR(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceVSTR(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceVSTR(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVSTR, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }


        if (requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }

        //Extras 1 - ram bar,2 = spoiler,3 = flat lightbar, 4 = valor lightbar, 5 - old lightbar, 6 antenna, 7 searchlights,8 = front camera  9 divider, 11 computer
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, true, 75),
                new DispatchableVehicleExtra(7, true, 35),
                new DispatchableVehicleExtra(8, true, 35),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 35),
                new DispatchableVehicleExtra(8, true, 35),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 35),
                new DispatchableVehicleExtra(8, true, 35),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 35),
                new DispatchableVehicleExtra(8, true, 35),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 10),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceAleutian(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceAleutian(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceAleutian(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceAleutian(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0);
    }
    public DispatchableVehicle Create_PoliceAleutian(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName,int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceAleutian, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }


        if (requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        //Extras 1- Siren, 2 - searchlights,3 = flat bar, 4 valor bar, 5 top antenna array, 6 big bull bar,, 8 = front camera,  9 divider,10 shotguns, 11 computer, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, false, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 65),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 95),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        //toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        //if (offroadWheelPercentage > 0)
        //{
        //    toReturn.WheelType = 3;//SUV
        //    if (toReturn.VehicleMods == null)
        //    {
        //        toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        //    }
        //    toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, offroadWheelPercentage)
        //    {
        //        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
        //                {
        //                    new DispatchableVehicleModValue(38,100),
        //                },
        //    });
        //}
        //else
        //{
        //    toReturn.WheelType = 3;//SUV
        //    if (toReturn.VehicleMods == null)
        //    {
        //        toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        //    }
        //    toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
        //    {
        //        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
        //                {
        //                    new DispatchableVehicleModValue(39,100),
        //                },
        //    });
        //}




        toReturn.WheelType = 3;//suv
        if (toReturn.VehicleMods == null)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        }
        List<DispatchableVehicleModValue> wheelmodvalues = new List<DispatchableVehicleModValue>()
        {
            new DispatchableVehicleModValue(39,100),//new vapid wheels SUV
        };
        if (offroadWheelPercentage > 0)
        {
            wheelmodvalues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(38,offroadWheelPercentage),
                            new DispatchableVehicleModValue(39,80),
                        };
        }
        toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
        {
            DispatchableVehicleModValues = wheelmodvalues,
        });




        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
   public DispatchableVehicle Create_SecurityStanier(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceStanierOldType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(SecurityStanier, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Security Stanier (2nd Gen) - 1 = lightbar, 2 = antenna
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
            new DispatchableVehicleExtra(1, true, 45),
            new DispatchableVehicleExtra(2, true, 80),
        };   
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGranger3600(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceGranger3600(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceGranger3600(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceGranger3600(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0);
    }
    public DispatchableVehicle Create_PoliceGranger3600(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGranger3600, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Extras 1- Siren,2  = ram bar,  4 valor lightbar, 5 alprs and antenna, 6 side antenna, 7 searchlights,8 interior alpr, 9 divider,10 = shotguns, 11 = computer, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 90),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        //toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        if (offroadWheelPercentage > 0)
        {
            toReturn.WheelType = 3;//SUV
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, offroadWheelPercentage)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(38,100),
                        },
            });
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceCaracara(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceCaracara(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }

    public DispatchableVehicle Create_PoliceCaracara(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceCaracara(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0);
    }

    public DispatchableVehicle Create_PoliceCaracara(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceCaracara, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Extras 1- Siren, 2 - bar,3 - flat lightbar, 4 - valor lightbar,5 - antenna/aprs , 6- searchlights, 7 = rear antenna,  9 = divider, 11 - computer, 12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 35),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 35),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 25),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;


        if(requiredDashboardColor != -1)
        {
            toReturn.RequiredDashColorID = requiredDashboardColor;
        }

        if (offroadWheelPercentage > 0)
        {
            toReturn.WheelType = 3;//SUV
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, offroadWheelPercentage)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(38,100),
                        },
            });
        }

        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    //public DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int vapidStyleWheelsPercentage)
    //{
    //    DispatchableVehicle intermediate = Create_PoliceBuffaloSTX(ambientPercent,wantedPercent,liveryID,useOptionalColors,policeVehicleType,requiredColor,minWantedLevel,maxWantedLevel,minOccupants,maxOccupants,requiredPedGroup, groupName, vapidStyleWheelsPercentage);
    //    intermediate.RequiredDashColorID = requiredDashboardColor;
    //    return intermediate;
    //}
    public DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int vapidStyleWheelsPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloSTX, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Extras 1- Siren, 2 - bar, 3 = flat lightbar, 4 - valor lightbar,5 - spoiler, 6- antenna, 7 = searchlight, 9 - divider, 10 - shotguns, 11 - computer,12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 35),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 95),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
               // new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }

        if (requiredDashboardColor != -1)
        {
            toReturn.RequiredDashColorID = requiredDashboardColor;
        }

        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }


        if (vapidStyleWheelsPercentage > 0)
        {
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            List<DispatchableVehicleModValue> wheelmodvalues = new List<DispatchableVehicleModValue>()
            {
                new DispatchableVehicleModValue(52,vapidStyleWheelsPercentage),
            };
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, vapidStyleWheelsPercentage)
            {
                DispatchableVehicleModValues = wheelmodvalues,
            });
        }



        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVindicator(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVindicator, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Vindicator - 1 = Luggage, 2 = Crash Bar
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 90),
                new DispatchableVehicleExtra(2, true, 95),
            };
        toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.Highway, highwayAdjustmentAmount) };// 25) };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceThrust(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount, int requiredPrimaryColor, int requiredSecondaryColor, int requiredDashcolor)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceThrust, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        //Vindicator -  2 = Crash Bar, 3= side luggae, 4 = top luggage
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(2, true, 95),
                new DispatchableVehicleExtra(3, true, 95),
                new DispatchableVehicleExtra(4, true, 95),
            };
        toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.Highway, highwayAdjustmentAmount) };// 25) };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);

        if (requiredPrimaryColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredPrimaryColor;
        }
        if (requiredSecondaryColor != -1)
        {
            toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        }
        if (requiredDashcolor != -1)
        {
            toReturn.RequiredDashColorID = requiredDashcolor;
            toReturn.MatchDashColorToBaseColor= true;
        }
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBison(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceBison(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceBison(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceBison(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0);
    }
    public DispatchableVehicle Create_PoliceBison(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBison, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)//2015 syle marked!
        {
            // 2 = ram bar, 3-5 = siren,6 = top antenna  9 partition, 11 = computer

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),


                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(9, true, 100),


                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),


                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(6, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),


                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),

                new DispatchableVehicleExtra(9, true, 100),


                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(9, true, 100),


                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 40),

                new DispatchableVehicleExtra(9, true, 100),


                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),

                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        if (offroadWheelPercentage > 0)
        {
            toReturn.WheelType = 3;//SUV
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, offroadWheelPercentage)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(38,100),
                        },
            });
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceGresley(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceGresley(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0); ;
    }
    public DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGresley, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Gresley - 1 = Front Bar, 2 = siren old,3 = flat lightbar, 4 = valor lightar,5 = alprs1, 6 = searchligh,7 = antenna, 9 = divider, 10 = shotguns, 11 = computer, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),

                

                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, true, 65),

                new DispatchableVehicleExtra(6, true, 45),

                
                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 45),

                
                new DispatchableVehicleExtra(7, true, 45),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),

                
                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),


                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),


                new DispatchableVehicleExtra(5, true, 25),

                new DispatchableVehicleExtra(6, true, 65),

                
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, false, 100),


                new DispatchableVehicleExtra(6, false, 65),

                
                new DispatchableVehicleExtra(7, false, 65),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        if (requiredDashboardColor != -1)
        {
            toReturn.RequiredDashColorID = requiredDashboardColor;
        }
 
        toReturn.WheelType = 3;//SUV
        if (toReturn.VehicleMods == null)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        }

        if (offroadWheelPercentage > 0)
        {
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                    {
                    new DispatchableVehicleModValue(38,offroadWheelPercentage),
                        new DispatchableVehicleModValue(39,100-offroadWheelPercentage),
                    },
            });
        }
        else
        {
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                    {
                        new DispatchableVehicleModValue(39,100),
                    },
            });
        }
        
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceRadius(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int hubcapWheelsPercentage)
    {
        DispatchableVehicle intermediate = Create_PoliceRadius(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName, hubcapWheelsPercentage);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceRadius(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int hubcapWheelsPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceRadius, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Gresley - 1 = Front Bar, 2 = siren,3 = flat siren, 4 = valor siren ,5 = alprs1, 6 = alprs 2,7 = searchligh , 9 = divider, 10 = shotguns, 11 = computer, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                 new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                 new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 70),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(7, false, 65),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        toReturn.WheelType = 3;//SUV
        if (toReturn.VehicleMods == null)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        }
        toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
        {
            DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                    {
                        new DispatchableVehicleModValue(39,100),
                    },
        });      
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_MilitaryUnarmedHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        //REMOVED
        DispatchableVehicle toReturn = new DispatchableVehicle("squaddie", ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { 0,1,2,3,4 };
        }
        //Squaddie - 1 = Front Grille, 5 = Rear Antenna, 6 = side antenna
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() 
        {
            new DispatchableVehicleExtra(1, true, 60),
            new DispatchableVehicleExtra(5, true, 60),
            new DispatchableVehicleExtra(6, true, 60),
        };
        toReturn.MaxRandomDirtLevel = 15.0f;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_MilitaryHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, MilitaryVehicleType militaryVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(MilitaryHumvee, ambientPercent, wantedPercent);
        //Squaddie - 1 = Main Gun, 2 = Front Grille, 5 = Rear Antenna, 6 = side antenna


        if (militaryVehicleType == MilitaryVehicleType.Unarmed)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 60),
                new DispatchableVehicleExtra(5, true, 60),
                new DispatchableVehicleExtra(6, true, 60),
            };
        }
        else
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {

                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 60),
                new DispatchableVehicleExtra(5, true, 60),
                new DispatchableVehicleExtra(6, true, 60),
            };
        }

        //toReturn.VehicleMods = new List<DispatchableVehicleMod>()
        //{
        //    new DispatchableVehicleMod(48,20) 
        //    { 
        //        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() 
        //        { 
        //            new DispatchableVehicleModValue(0,33),
        //            new DispatchableVehicleModValue(1,33),
        //            new DispatchableVehicleModValue(2,33),
        //        }  
        //    }
        //};
        if (militaryVehicleType == MilitaryVehicleType.Armed)
        {
            toReturn.FirstPassengerIndex = 7;
            toReturn.ForceStayInSeats = new List<int>() { 7 };
        }
        toReturn.MaxRandomDirtLevel = 15.0f;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBoxville(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        //REMOVED
       
        DispatchableVehicle toReturn = new DispatchableVehicle("NONE", ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Boxville - 1 = Top Light
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
        {
            //Boxville - 1 = Top Light
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
            };
            toReturn.RequiredPrimaryColorID = requiredColor;//base white
            toReturn.RequiredSecondaryColorID = requiredColor;//base black
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBicycle(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int bicyclePercent)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBicycle, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        if (bicyclePercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>
            {
                new SpawnAdjustmentAmount(eSpawnAdjustment.Bicycle, bicyclePercent),
            };
        }
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceSanchez(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceSanchez, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        if (offroadAdditionalPercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>
            {
                new SpawnAdjustmentAmount(eSpawnAdjustment.OffRoad, offroadAdditionalPercent),
                new SpawnAdjustmentAmount(eSpawnAdjustment.DirtBike, offroadAdditionalPercent),
            };
        }
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceTerminus(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        //12 = blank livery
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceTerminus, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //1 = roof, 2 = siren,3 = flat lightbar, 4 = valor lightbar, , 6 = side antenna,7 = searchlight,,9 = divider, 11 = computer
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(1, true, 50),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(1, true, 50),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);

        if(offroadAdditionalPercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>();
            toReturn.SpawnAdjustmentAmounts.Add(new SpawnAdjustmentAmount(eSpawnAdjustment.OffRoad, offroadAdditionalPercent));
        }

        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBuffaloS(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloS, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }

        //Extras 1 - siren , 2 = ram bar, 4 = searchlights 5 = antenna, 9 = divider, 12 = radio

        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() 
            { 
                new DispatchableVehicleExtra(1, true, 100), 
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(4, true, 75),
                new DispatchableVehicleExtra(5, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(4, true, 75),
                new DispatchableVehicleExtra(5, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(4, true, 75),
                new DispatchableVehicleExtra(5, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceFugitive(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceFugitive, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            //Fugitive - 1 = Old Lightbar, 2 = ram bar, 3 = flat lightbar, 4 = valor lightbar, 5 = searchliught, 6 = antenna/alprs front,
            //7 = alprs rear,9 = divider, 11 = computer,12 = raido, 10 = rear divider  
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 35),
                new DispatchableVehicleExtra(6, true, 30),
                new DispatchableVehicleExtra(7, true, 30),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 35),
                new DispatchableVehicleExtra(6, true, 30),
                new DispatchableVehicleExtra(7, true, 30),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 35),
                new DispatchableVehicleExtra(6, true, 30),
                new DispatchableVehicleExtra(7, true, 30),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 35),
                new DispatchableVehicleExtra(6, true, 30),
                new DispatchableVehicleExtra(7, true, 30),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 35),
                new DispatchableVehicleExtra(6, true, 30),
                new DispatchableVehicleExtra(7, true, 30),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceLandstalkerXL(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        return Create_PoliceLandstalkerXL(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName,-1,0);
    }

    public DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int offroadWheelPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceLandstalkerXL, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Landstalker XL - 1 = Top Light, 2 = Ram Bar, 3 = flat new siren, 4 = valor sire, 5 = alprs,6 = side antenna, 7 = searhlights  9 = partition,10 = shotguns, 11 = computer 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 70),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        if (offroadWheelPercentage > 0)
        {
            toReturn.WheelType = 3;//SUV
            if (toReturn.VehicleMods == null)
            {
                toReturn.VehicleMods = new List<DispatchableVehicleMod>();
            }
            toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, offroadWheelPercentage)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(38,100),
                        },
            });
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    } 
    public DispatchableVehicle Create_PoliceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor, int hubcapWheelsPercentage)
    {
        DispatchableVehicle intermediate = Create_PoliceInterceptor(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName, hubcapWheelsPercentage);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int hubcapWheelsPercentage)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceTorrence, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)//2015 syle marked!
        {
            //1 = Searchlught, 2 = ram bar, 3 = siren,6 = top antenna alprs, 7 = rear alprs, 9 partition,10 = shotguns, 11 = computer, 12 = radio

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        toReturn.WheelType = 0;//sport
        if (toReturn.VehicleMods == null)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>();
        }
        List<DispatchableVehicleModValue> wheelmodvalues = new List<DispatchableVehicleModValue>()
        {
            new DispatchableVehicleModValue(52,100),//new vapid wheels
        };
        if(hubcapWheelsPercentage > 0)
        {
            wheelmodvalues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(50,(int)Math.Round(hubcapWheelsPercentage/4.0,0)),
                            new DispatchableVehicleModValue(51,hubcapWheelsPercentage),
                            new DispatchableVehicleModValue(52,80),
                        };
        }
        toReturn.VehicleMods.Add(new DispatchableVehicleMod(23, 100)
        {
            DispatchableVehicleModValues = wheelmodvalues,
        });
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    public DispatchableVehicle Create_PoliceGauntlet(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceGauntlet(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }


    public DispatchableVehicle Create_PoliceGauntlet(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGauntlet, ambientPercent, wantedPercent);
        if (modKitliveryID != -1)
        {
            toReturn.RequiredVariation = new VehicleVariation();
            toReturn.RequiredVariation.VehicleMods.Add(new VehicleMod(48, modKitliveryID));
        }
        //toReturn.RequiredPrimaryColorID = 0;//base white
        //toReturn.RequiredSecondaryColorID = 111;//base black
        toReturn.ForcedPlateType = 4;
        if(requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedFlatLightbar || policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            if (toReturn.RequiredVariation != null)
            {
          
                    toReturn.RequiredVariation.PrimaryColor = -1;
                    toReturn.RequiredVariation.SecondaryColor = -1;
               
            }
            //Gauntlet XL - ?? sirens?
            if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
            {
                toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                };
            }
            else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
            {
                toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,true,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                };
            }
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(42,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
                        },
                    },
                    new DispatchableVehicleMod(43,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,50),
                            new DispatchableVehicleModValue(1,50),
                        },
                    },
                    new DispatchableVehicleMod(44,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                        },
                    },
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(modKitliveryID,100),
                        },
                    },
                };


        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            if (toReturn.RequiredVariation != null)
            {

                toReturn.RequiredVariation.PrimaryColor = -1;
                toReturn.RequiredVariation.SecondaryColor = -1;

            }
            //if (toReturn.RequiredVariation != null)
            //{
            //    if (requiredColor == -1)
            //    {
            //        toReturn.RequiredVariation.PrimaryColor = 0;
            //        toReturn.RequiredVariation.SecondaryColor = 111;
            //    }
            //    else
            //    {
            //        toReturn.RequiredVariation.PrimaryColor = requiredColor;
            //        toReturn.RequiredVariation.SecondaryColor = requiredColor;
            //    }
            //}
            //Gauntlet XL - ?? sirens?
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                };
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(42,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
                        },
                    },
                    new DispatchableVehicleMod(43,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,50),
                            new DispatchableVehicleModValue(1,50),
                        },
                    },
                    new DispatchableVehicleMod(44,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                        },
                    },
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(modKitliveryID,100),
                        },
                    },
                };


        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                };
        }

        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn,useOptionalColors, requiredColor, minWantedLevel,maxWantedLevel,minOccupants,maxOccupants,requiredPedGroup,groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_ServiceDilettante(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType ServiceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(ServiceDilettante, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (ServiceVehicleType == ServiceVehicleType.Taxi1)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(5,false,100),
                new DispatchableVehicleExtra(6,false,100),
                new DispatchableVehicleExtra(7,true,100),
                new DispatchableVehicleExtra(8,false,100),
                new DispatchableVehicleExtra(9,true,100),
                new DispatchableVehicleExtra(12,true,100),
            };
        }
        else if (ServiceVehicleType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(5,false,100),
                new DispatchableVehicleExtra(6,false,100),
                new DispatchableVehicleExtra(7,false,100),
                new DispatchableVehicleExtra(8,true,100),
                new DispatchableVehicleExtra(9,true,100),
                new DispatchableVehicleExtra(12,true,100),
            };
        }
        else if (ServiceVehicleType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(5,true,100),
                new DispatchableVehicleExtra(6,false,100),
                new DispatchableVehicleExtra(7,false,100),
                new DispatchableVehicleExtra(8,false,50),
                new DispatchableVehicleExtra(9,false,100),
                new DispatchableVehicleExtra(12,true,100),
            };

        }
        else if (ServiceVehicleType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(5,false,100),
                new DispatchableVehicleExtra(6,true,100),
                new DispatchableVehicleExtra(7,false,100),
                new DispatchableVehicleExtra(8,false,50),
                new DispatchableVehicleExtra(9,false,100),
                new DispatchableVehicleExtra(12,true,100),
            };
        }
        else if (ServiceVehicleType == ServiceVehicleType.Security)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(5,false,100),
                new DispatchableVehicleExtra(6,false,100),
                new DispatchableVehicleExtra(7,false,100),
                new DispatchableVehicleExtra(8,false,100),
                new DispatchableVehicleExtra(9,false,100),
                new DispatchableVehicleExtra(12,false,100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_ServiceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        //Extras
        //1 = Searchlught, 2 = ram bar, 3 = antenna, 4 = partition 5-9 = regular taxi, 10 = medallion, 11 = radio, 12 = vanilla
        DispatchableVehicle toReturn = new DispatchableVehicle(SecurityTorrence, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (serviceVehicleType == ServiceVehicleType.Taxi1)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 80),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),

                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 80),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),

                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 80),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 80),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, true, 100),
                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Security)
        {
            //Extras
            //1 = Searchlught, 2 = ram bar, 3 = antenna, 4 = partition 5-9 = regular taxi, 10 = medallion, 11 = radio, 12 = vanilla
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(2, true, 30),
                new DispatchableVehicleExtra(3, true, 80),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),

                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }    
    public DispatchableVehicle Create_TaxiVivanite(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        //Extras
        //NO EXTRAS!
        DispatchableVehicle toReturn = new DispatchableVehicle(TaxiVivanite, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(liveryID,100),
                        },
                    },
                };
        }
        if (serviceVehicleType == ServiceVehicleType.Taxi1)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, true, 100),
                new DispatchableVehicleExtra(9, true, 100),


            };
        }

        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }

    private void SetDefault(DispatchableVehicle toSetup, bool useOptionalColors, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, 
        string requiredPedGroup, string groupName)
    {
        if(toSetup == null)
        {
            return;
        }
        if (useOptionalColors)
        {
            toSetup.OptionalColors = DefaultOptionalColors.ToList();
        }
        if (requiredColor != -1)
        {
            toSetup.RequiredPrimaryColorID = requiredColor;
            toSetup.RequiredSecondaryColorID = requiredColor;
        }
        if (minWantedLevel != -1)
        {
            toSetup.MinWantedLevelSpawn = minWantedLevel;
        }
        if (maxWantedLevel != -1)
        {
            toSetup.MaxWantedLevelSpawn = maxWantedLevel;
        }
        if (minOccupants != -1)
        {
            toSetup.MinOccupants = minOccupants;
        }
        if (maxOccupants != -1)
        {
            toSetup.MaxOccupants = maxOccupants;
        }
        if (requiredPedGroup != "")
        {
            toSetup.RequiredPedGroup = requiredPedGroup;
        }
        if (groupName != "")
        {
            toSetup.GroupName = groupName;
        }
    }
}

/*    public DispatchableVehicle Create_PoliceOracle(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceOracle, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- LED Siren, 2 - ram bar, 3 - flat lightbar, 4 = valor lightbar, 5 searchlights,6 = front alprs, 7 = rear alprs,8 = camera, 9 - divider, 10 = shotguns, 11 = computer, 12 = radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 75),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceReblaGTS(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceReblaGTS, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 2 - ram bar,3 = flat lightbar, 4 = balor lightbar, 5 top antenna, 6 side antenna,7 = searchlights, 8 = front camera,  9 divider,11 = computer 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                 new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 10),
                new DispatchableVehicleExtra(6, true, 10),
                new DispatchableVehicleExtra(7, false, 90),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }*/