using System;
using System.Collections.Generic;
using System.Linq;

public class DispatchableVehicles_FEJ
{
    public DispatchableVehicles DispatchableVehicles;
    public string PoliceStanier = "police";
    public string PoliceBuffalo = "police2";
    public string PoliceBuffaloS = "buffalo3";
    public string PoliceTorrence = "police3";
    public string PoliceGranger = "sheriff2";
    public string PoliceGresley = "sheriff";
    public string PoliceBison = "ruiner3";
    public string PoliceMerit = "policeold1";
    public string PoliceEsperanto = "policeold2";
    public string PoliceFugitive = "pranger";
    public string PoliceBike = "policeb";
    public string PoliceTransporter = "policet";
    public string StanierUnmarked = "police4";
    public string BuffaloUnmarked = "fbi";
    public string GrangerUnmarked = "fbi2";
    public string SecurityTorrence = "lurcher";
    public string PoliceGauntlet = "polgauntlet";
    public string ServiceDilettante = "dilettante2";
    public string WashingtonUnmarked = "blista3";
    public string PoliceStanierOld = "stalion2";
    public string ServiceStanierOld = "gauntlet2";
    public string PoliceLandstalkerXL = "dominator2";
    public string PoliceSanchez = "sovereign";
    public string PoliceTerminus = "marshall";
    public string PoliceBoxville = "boxville5";
    public string PoliceVindicator = "deathbike";
    public string TaxiMinivan = "dukes2";
    public string SecurityStanier = "massacro2";
    public string PoliceBuffaloSTX = "jester2";
    public string PoliceCaracara = "dune4";
    public string PoliceGranger3600 = "dune5";
    public string PolicePatriot = "wastelander";
    public string PoliceSeminole = "hotring";
    public string PoliceAleutian = "blazer5";
    public string PoliceVSTR = "tampa3";
    public string TaxiVivanite = "caddy3";
    public string PoliceReblaGTS = "oppressor";
    public string PoliceVerus = "shotaro";
    public string PoliceRiata = "zhaba";
    public string PoliceKurumaUnmarked = "zr380";
    public string PoliceOracle = "zr3802";


    //Civilian
    public string CivilianEsperanto = "phantom2";
    public string CivilianInterceptor = "issi2";
    public string CivilianStanierSecondGen = "tornado3";
    public string CivilianMerit = "technical2";
    public List<int> DefaultOptionalColors { get; private set; } = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 40, 45, 61, 62, 63, 64, 66, 69, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 111, 112 };

    public DispatchableVehicles_FEJ(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }
    public DispatchableVehicles_FEJ_2008 DispatchableVehicles_FEJ_2008 { get; private set; }
    public DispatchableVehicles_FEJ_Modern DispatchableVehicles_FEJ_Modern { get; private set; }
    public DispatchableVehicles_FEJ_2015 DispatchableVehicles_FEJ_2015 { get; private set; }

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

        DispatchableVehicles_FEJ_2015 = new DispatchableVehicles_FEJ_2015(this);
        DispatchableVehicles_FEJ_2015.DefaultConfig();


        DispatchableVehicles_FEJ_2008 = new DispatchableVehicles_FEJ_2008(this);
        DispatchableVehicles_FEJ_2008.DefaultConfig();

        DispatchableVehicles_FEJ_Modern = new DispatchableVehicles_FEJ_Modern(this);
        DispatchableVehicles_FEJ_Modern.DefaultConfig();
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
            new DispatchableVehicle("polmav", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 1,50) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 1,50) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 1 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        SheriffHeliVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("frogger2", 1,200) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 4 },MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            new DispatchableVehicle("polmav", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 1 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        ArmyVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            Create_MilitaryArmedHumvee(5,50,-1,false,PoliceVehicleType.Marked,-1,6,10,2,4,"",""),

            //Heavy
            new DispatchableVehicle("rhino", 1, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
    
            //Heli
            new DispatchableVehicle("valkyrie2", 1,20) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("annihilator", 1, 45) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 },RequiredPrimaryColorID = 153,RequiredSecondaryColorID = 153, MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("buzzard",1,20) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},
            new DispatchableVehicle("hunter",1,15) { RequiredPedGroup = "Pilot", RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 2, MaxOccupants = 2, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,50)  } },
        };

        USMCVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            Create_MilitaryArmedHumvee(5,50,-1,false,PoliceVehicleType.Marked,-1,6,10,2,4,"",""),

            //HELI
            new DispatchableVehicle("cargobob",1,15) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},
            
            //Boat
            new DispatchableVehicle("dinghy5", 1, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },
        };

        USAFVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            Create_MilitaryArmedHumvee(5,50,-1,false,PoliceVehicleType.Marked,-1,6,10,2,4,"",""),
            
            //HELI
            new DispatchableVehicle("annihilator", 1, 45) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 4 },RequiredPrimaryColorID = 153,RequiredSecondaryColorID = 153, MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, MinOccupants = 3, MaxOccupants = 4 },

            //JETS
            new DispatchableVehicle("lazer",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150) } },
            new DispatchableVehicle("hydra",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150) } },
            new DispatchableVehicle("strikeforce",1,5) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle, 150) } },
        };

        CoastGuardVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dinghy5", 50, 50) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 1,MaxOccupants = 2, MaxWantedLevelSpawn = 2, },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle("seashark2", 50, 50) { RequiredLiveries = new List<int>() { 2,3 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",1,50) { RequiredLiveries = new List<int>() { 1 },MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
            new DispatchableVehicle("polmav", 1,30) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 4 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
            new DispatchableVehicle("annihilator", 1,100) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
        };

        MarshalsServiceVehicles_FEJ = DispatchableVehicles.MarshalsServiceVehicles.Copy();//for now
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBuffaloS(25, 25, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_Washington(25, 25, -1, true, true, -1, -1, -1, "", ""));//  new DispatchableVehicle(WashingtonUnmarked, 25, 25) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, true, 40) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceInterceptor(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceGresley(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBison(10, 10, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceKuruma(5, 5, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
    }
    private void EMT()
    {
        LSFDEMTVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } }
        };
        LSCOFDEMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } }
        };
        BCFDEMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 3 } }
        };
        SAMSVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 4 } }
        };

        MRHVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };
        LSMCVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };

    }
    private void Fire()
    {
        LSFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("firetruk", 100, 100) { RequiredLiveries = new List<int>() { 0 } ,MinOccupants = 2, MaxOccupants = 4 }
        };
        LSCOFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("firetruk", 100, 100) { RequiredLiveries = new List<int>() { 2 } ,MinOccupants = 2, MaxOccupants = 4 }
        };
        BCFDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("firetruk", 100, 100) { RequiredLiveries = new List<int>() { 1 } ,MinOccupants = 2, MaxOccupants = 4 }
        };
        SanFireVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("firetruk", 100, 100) { RequiredLiveries = new List<int>() { 3 } ,MinOccupants = 2, MaxOccupants = 4 }
        };
    }
    public DispatchableVehicle Create_PoliceOracle(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceOracle, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- LED Siren, 2 - Halogen Siren, 3 - ram bar 4 searchlights, 9 - divider
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 5),
                new DispatchableVehicleExtra(4, true, 35),
                new DispatchableVehicleExtra(9, false, 100),
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
        //Extras 1- Siren, 2 - ram bar, 4 searchlights, 5 top antenna, 6 side antenna,  11 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
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
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 15),
                new DispatchableVehicleExtra(4, true, 35),
                new DispatchableVehicleExtra(5, true, 15),
                new DispatchableVehicleExtra(6, true, 15),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 15),
                new DispatchableVehicleExtra(4, true, 35),
                new DispatchableVehicleExtra(5, true, 15),
                new DispatchableVehicleExtra(6, true, 15),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVerus(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVerus, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //1 = siren, 2 = roof, 3 = brush guard, 4 = searchlight,
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 55),
                new DispatchableVehicleExtra(4, true, 55),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 55),
                new DispatchableVehicleExtra(4, true, 55),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 55),
                new DispatchableVehicleExtra(4, true, 55),
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
    public DispatchableVehicle Create_PoliceVSTR(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVSTR, ambientPercent, wantedPercent);
        //Extras 1 - ram bar, 4 searchlights, 6 antenna,  9 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceAleutian(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceAleutian, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }


        if(requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        //Extras 1- Siren, 3 - ram bar, 4 searchlights, 5 top antenna, 6 big bull bar,  9 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceSeminole(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceSeminole, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- new siren,2 - old siren , 3 ram bar, 4 searchlight, 5 anetnna, 9 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.OlderMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
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
                new DispatchableVehicleExtra(4, true, 5),
                new DispatchableVehicleExtra(5, true, 5),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        if (offroadAdditionalPercent > 0)
        {
            toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>
            {
                new SpawnAdjustmentAmount(eSpawnAdjustment.OffRoad, offroadAdditionalPercent),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PolicePatriot(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PolicePatriot, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren,2 - police ram bar , 4 running boards, 5 brush guard,6 = exhaust  7 antenna, 9 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceEsperanto(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceEsperanto, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Modern Siren, 2 - Old Siren, 3 - driver searchlight, 4 passenger searchlights, 5 top antenna, 6 ram bar,
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.OlderMarked)//only using halogen bar
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 75),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)//NO SIRENS WITH THIS
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 25),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)//NO SIRENS WITH THIS
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 25),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)//NO SIRENS WITH THIS
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_SecurityStanier(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceStanierOldType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(SecurityStanier, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }    
        //Security Stanier (2nd Gen) - 1 = lightbar, 2 = antenna
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
            new DispatchableVehicleExtra(1, true, 45),
            new DispatchableVehicleExtra(2, true, 80),
        };   
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGranger3600(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGranger3600, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 3 - ram bar, 4 searchlights, 5 top antenna, 6 side antenna, 7 running baords, 9 divider, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(3, true, 75),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, true, 75),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceCaracara(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceCaracara, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 2 - bar,3 - running boards, 4 - searchlight,5 - antenna top, 6- antenna rear, 11 - divider, 12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 75),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
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
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(6, true, 65),
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
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 35),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 45),
                new DispatchableVehicleExtra(5, true, 25),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    public DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredDashboardColor)
    {
        DispatchableVehicle intermediate = Create_PoliceBuffaloSTX(ambientPercent,wantedPercent,liveryID,useOptionalColors,policeVehicleType,requiredColor,minWantedLevel,maxWantedLevel,minOccupants,maxOccupants,requiredPedGroup, groupName);
        intermediate.RequiredDashColorID = requiredDashboardColor;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloSTX, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 3 - bar, 4 - searchlight,5 - spoiler, 6- antenna,8 - front radar stuff, 9 - divider, 10 - shotguns, 11 - computer,12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(8, true, 85),
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
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(8, true, 85),
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
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, false, 70),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, true, 25),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, false, 70),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVindicator(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceVindicator, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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
    public DispatchableVehicle Create_PoliceMerit(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceMerit, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Merit - 1 = New Siren, 2 = Old siren, 4 = antenna, 5 = searchlight, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.OlderMarked)
        {
            //Merit - 1 = New Siren, 2 = Old siren, 4 = antenna, 5 = searchlight, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredPrimaryColorID = requiredColor;//base white
            toReturn.RequiredSecondaryColorID = requiredColor;//base black
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 65),
                new DispatchableVehicleExtra(5, false, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 65),
                new DispatchableVehicleExtra(5, false, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBison(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBison, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Bison - 1 = Front Bar, 2 = siren, 4 = antenna, 5 spotlight, 9 = divider
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 70),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 65),
                new DispatchableVehicleExtra(5, false, 65),
                new DispatchableVehicleExtra(9, false, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGresley, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Gresley - 1 = Front Bar, 2 = siren, 4 = searchligh, 8 = antenna, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
        {
            //Gresley - 1 = Front Bar, 2 = siren, 4 = searchligh, 8 = antenna, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredPrimaryColorID = requiredColor;//base white
            toReturn.RequiredSecondaryColorID = requiredColor;//base black
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            //Gresley - 1 = Front Bar, 2 = siren, 4 = searchligh, 8 = antenna, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            //Gresley - 1 = Front Bar, 2 = siren, 4 = searchligh, 8 = antenna, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(8, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            //Gresley - 1 = Front Bar, 2 = siren, 4 = searchligh, 8 = antenna, 9 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 70),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, false, 65),
                new DispatchableVehicleExtra(8, false, 65),
                new DispatchableVehicleExtra(9, false, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_MilitaryUnarmedHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
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
    public DispatchableVehicle Create_MilitaryArmedHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle("insurgent3", ambientPercent, wantedPercent);
        //Squaddie - 1 = Main Gun, 2 = Front Grille, 5 = Rear Antenna, 6 = side antenna
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
        {
            new DispatchableVehicleExtra(1, true, 100),
            new DispatchableVehicleExtra(2, true, 60),
            new DispatchableVehicleExtra(5, true, 60),
            new DispatchableVehicleExtra(6, true, 60),
        };
        toReturn.VehicleMods = new List<DispatchableVehicleMod>()
        {
            new DispatchableVehicleMod(48,20) 
            { 
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>() 
                { 
                    new DispatchableVehicleModValue(0,33),
                    new DispatchableVehicleModValue(1,33),
                    new DispatchableVehicleModValue(2,33),
                }  
            }
        };
        toReturn.FirstPassengerIndex = 7;
        toReturn.ForceStayInSeats = new List<int>() { 7 };
        toReturn.MaxRandomDirtLevel = 15.0f;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBoxville(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBoxville, ambientPercent, wantedPercent);
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
        DispatchableVehicle toReturn = new DispatchableVehicle("scorcher", ambientPercent, wantedPercent);
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
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //1 = roof, 2 = siren, 4 = searchlight, 5 = top antenna, 6 = side antenna, 11 = divider
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(4, true, 55),
                new DispatchableVehicleExtra(5, true, 55),
                new DispatchableVehicleExtra(6, true, 55),
                new DispatchableVehicleExtra(11, true, 85),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 55),
                new DispatchableVehicleExtra(11, true, 85),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(4, true, 55),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(11, false, 100),
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
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }

        //Extras 1 - siren , 2 = ram bar, 4 = searchlights 5 = antenna, 9 = divider, 12 = radio

        if(policeVehicleType == PoliceVehicleType.Marked)
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
    public DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceFugitive, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Fugitive - 1 = Top Light, 2 = Ram Bar,3 = searchlight, 4 = cage,6 = rear chrome, 10 = vanilla, 11 = radio, 12 = vanilla
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 50),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
            toReturn.RequiredPrimaryColorID = 134;//base white
            toReturn.RequiredSecondaryColorID = 134;//base black
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            //Fugitive - 1 = Top Light, 2 = Ram Bar,3 = searchlight, 4 = cage,6 = rear chrome, 10 = vanilla, 11 = radio, 12 = vanilla
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 50),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
            };
            toReturn.RequiredPrimaryColorID = 134;//base white
            toReturn.RequiredSecondaryColorID = 134;//base black
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 50),
                new DispatchableVehicleExtra(3, true, 40),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(11, true, 55),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 20),
                new DispatchableVehicleExtra(3, true, 20),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(11, true, 35),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceLandstalkerXL, ambientPercent, wantedPercent);
        if (modKitliveryID != -1)
        {
            toReturn.RequiredVariation = new VehicleVariation();
            toReturn.RequiredVariation.VehicleMods.Add(new VehicleMod(48, modKitliveryID));
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Landstalker XL - 1 = Top Light, 2 = Ram Bar, 4 = searchlight, 5 = antenna, 11 = partition, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(4, true, 30),
                new DispatchableVehicleExtra(5, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
            toReturn.RequiredPrimaryColorID = 134;//base white
            toReturn.RequiredSecondaryColorID = 0;//base black

            if(toReturn.RequiredVariation != null)
            {
                toReturn.RequiredVariation.PrimaryColor = 134;
                toReturn.RequiredVariation.SecondaryColor = 0;
            }
        }
        if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
        {
            //Landstalker XL - 1 = Top Light, 2 = Ram Bar, 4 = searchlight, 5 = antenna, 11 = partition, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(4, true, 30),
                new DispatchableVehicleExtra(5, true, 70),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
            toReturn.RequiredPrimaryColorID = requiredColor;//base white
            toReturn.RequiredSecondaryColorID = requiredColor;//base black

            if (toReturn.RequiredVariation != null)
            {
                toReturn.RequiredVariation.PrimaryColor = requiredColor;
                toReturn.RequiredVariation.SecondaryColor = requiredColor;
            }
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 55),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, false, 20),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 20),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, false, 20),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceTransporter(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int sirenPercent, bool removeSiren, bool addInteriorExtra, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceTransporter, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (sirenPercent > 0 || addInteriorExtra || removeSiren)
        {
            //Police Transporter - 1 = LED siren, 12 = Cargo Props
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>();
            if(sirenPercent > 0)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(1, true, sirenPercent));
            }
            if (addInteriorExtra)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(12, true, 55));
            }
            if(removeSiren)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(1, false, 100));
            }
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, "");
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceStanierOld, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)
        {
            //Police Stanier - 1 = LED siren, 2 = Halogen Siren, 3 = Ram Bar, 4 = Searchlight, 5 = antenna, 9 = Partition, 11 = Vanilla Cupholder, 12 = Radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 85),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.OlderMarked)
        {
            //Police Stanier - 1 = LED siren, 2 = Halogen Siren, 3 = Ram Bar, 4 = Searchlight, 5 = antenna, 9 = Partition, 11 = Vanilla Cupholder, 12 = Radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, true, 85),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            //Police Stanier - 1 = LED siren, 2 = Halogen Siren, 3 = Ram Bar, 4 = Searchlight, 5 = antenna, 9 = Partition, 11 = Vanilla Cupholder, 12 = Radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 85),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 40),
                new DispatchableVehicleExtra(4, true, 40),
                new DispatchableVehicleExtra(5, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 20),
                new DispatchableVehicleExtra(4, true, 25),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_Washington(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, bool addExtras, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(WashingtonUnmarked, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (addExtras)
        {
            //Washingtion - 1 = Ram Bar, 2 = Emblem, 3 = Searchlight, 11 / 12 = Vanilla Cupholder Stuff, 13 = Radio(Non functional, needs to be changed to lower number)
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 60),
                new DispatchableVehicleExtra(2, true, 20),
                new DispatchableVehicleExtra(3, true, 50),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string groupName, string requiredPedGroup)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceTorrence, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)
        {
            //1 = Searchlught, 2 = ram bar, 3 = siren, 9 partition, 11 = radio, 12 = vanilla
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 70),
                new DispatchableVehicleExtra(2, true, 85),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            //1 = Searchlught, 2 = ram bar, 3 = siren, 9 partition, 11 = radio, 12 = vanilla
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 70),
                new DispatchableVehicleExtra(2, true, 85),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 40),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(11, true, 75),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, true, 20),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(11, true, 20),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGauntlet(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGauntlet, ambientPercent, wantedPercent);
        if (modKitliveryID != -1)
        {
            toReturn.RequiredVariation = new VehicleVariation();
            toReturn.RequiredVariation.VehicleMods.Add(new VehicleMod(48, modKitliveryID));
        }
        toReturn.RequiredPrimaryColorID = 0;//base white
        toReturn.RequiredSecondaryColorID = 111;//base black
        toReturn.ForcedPlateType = 4;
        if(requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            if (toReturn.RequiredVariation != null)
            {
                if (requiredColor == -1)
                {
                    toReturn.RequiredVariation.PrimaryColor = 0;
                    toReturn.RequiredVariation.SecondaryColor = 111;
                }
                else
                {
                    toReturn.RequiredVariation.PrimaryColor = requiredColor;
                    toReturn.RequiredVariation.SecondaryColor = requiredColor;
                }
            }
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
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            if (toReturn.RequiredVariation != null)
            {
                if (requiredColor == -1)
                {
                    toReturn.RequiredVariation.PrimaryColor = 0;
                    toReturn.RequiredVariation.SecondaryColor = 111;
                }
                else
                {
                    toReturn.RequiredVariation.PrimaryColor = requiredColor;
                    toReturn.RequiredVariation.SecondaryColor = requiredColor;
                }
            }
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

        }
        SetDefault(toReturn,useOptionalColors,-1,minWantedLevel,maxWantedLevel,minOccupants,maxOccupants,requiredPedGroup,groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_ServiceDilettante(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType ServiceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(ServiceDilettante, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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
    public DispatchableVehicle Create_ServiceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceStanierOldType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(ServiceStanierOld, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (serviceStanierOldType == ServiceVehicleType.Taxi1)
        {
            //Service Stanier - 1 = Taxi Medallion/Badge, 2 = Antenna, 3 = ram bar, 4 = searchlight, 5-9 = vanilla taxi, 10 = Vanilla Cupholder, 11 = partition, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceStanierOldType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceStanierOldType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceStanierOldType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 45),
                new DispatchableVehicleExtra(4, true, 20),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, true, 100),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceStanierOldType == ServiceVehicleType.Security)
        {
            //Service Stanier - 1 = Taxi Medallion/Badge, 2 = Antenna, 3 = ram bar, 4 = searchlight, 5-9 = vanilla taxi, 10 = Vanilla Cupholder, 11 = partition, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 80),
                new DispatchableVehicleExtra(3, true, 55),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, false, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }
    public DispatchableVehicle Create_ServiceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        //Extras
        //1 = Searchlught, 2 = ram bar, 3 = antenna, 4 = partition 5-9 = regular taxi, 10 = medallion, 11 = radio, 12 = vanilla
        DispatchableVehicle toReturn = new DispatchableVehicle(SecurityTorrence, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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
    public DispatchableVehicle Create_TaxiMinivan(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        //Extras
        //1 = wing,  3 = ram bar, 4 = antenna, 5-9 = regular taxi, 11 = divbider, 12 = radio
        DispatchableVehicle toReturn = new DispatchableVehicle(TaxiMinivan, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (serviceVehicleType == ServiceVehicleType.Taxi1)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(3, true, 30),
                new DispatchableVehicleExtra(4, true, 80),

                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(3, true, 30),
                new DispatchableVehicleExtra(4, true, 80),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(3, true, 30),
                new DispatchableVehicleExtra(4, true, 80),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 20),
                new DispatchableVehicleExtra(3, true, 30),
                new DispatchableVehicleExtra(4, true, 80),

                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, true, 100),
                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
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
            toReturn.RequiredLiveries = new List<int>() { liveryID };
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

