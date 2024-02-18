using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DispatchableVehicles : IDispatchableVehicles
{
    private string PoliceStanier = "police";
    private string PoliceBuffalo = "police2";
    private string PoliceBuffaloS = "buffalo3";
    private string PoliceTorrence = "police3";
    private string PoliceGranger = "sheriff2";
    private string PoliceGresley = "sheriff";

    private string PoliceBison = "policeold2";
    private string PoliceMerit = "policeold1";

    private string PoliceFugitive = "pranger";
    private string PoliceBike = "policeb";
    private string PoliceTransporter = "policet";

    private string StanierUnmarked = "police4";
    private string BuffaloUnmarked = "fbi";
    private string GrangerUnmarked = "fbi2";

    private string SecurityTorrence = "lurcher";

    private string PoliceGauntlet = "polgauntlet";

    private string ServiceDilettante = "dilettante2";


    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\DispatchableVehicles.xml";
    private List<DispatchableVehicleGroup> VehicleGroupLookup = new List<DispatchableVehicleGroup>();


    private DispatchableVehicles_Gangs DispatchableVehicles_Gangs;

    private List<DispatchableVehicle> LostMCVehicles;
    private List<DispatchableVehicle> VarriosVehicles;
    private List<DispatchableVehicle> BallasVehicles;
    private List<DispatchableVehicle> VagosVehicles;
    private List<DispatchableVehicle> MarabuntaVehicles;
    private List<DispatchableVehicle> KoreanVehicles;
    private List<DispatchableVehicle> TriadVehicles;
    private List<DispatchableVehicle> YardieVehicles;
    private List<DispatchableVehicle> DiablosVehicles;

    private List<DispatchableVehicle> GambettiVehicles;
    private List<DispatchableVehicle> PavanoVehicles;
    private List<DispatchableVehicle> LupisellaVehicles;
    private List<DispatchableVehicle> MessinaVehicles;
    private List<DispatchableVehicle> AncelottiVehicles;

    private List<DispatchableVehicle> ArmeniaVehicles;
    private List<DispatchableVehicle> CartelVehicles;
    private List<DispatchableVehicle> RedneckVehicles;
    private List<DispatchableVehicle> FamiliesVehicles;
    private List<DispatchableVehicle> UnmarkedVehicles;
    private List<DispatchableVehicle> CoastGuardVehicles;
    private List<DispatchableVehicle> ParkRangerVehicles;
    private List<DispatchableVehicle> FIBVehicles;
    private List<DispatchableVehicle> NOOSEVehicles;
    private List<DispatchableVehicle> PrisonVehicles;
    private List<DispatchableVehicle> LSPDVehicles;
    private List<DispatchableVehicle> SAHPVehicles;
    private List<DispatchableVehicle> LSSDVehicles;
    private List<DispatchableVehicle> BCSOVehicles;
    private List<DispatchableVehicle> VWHillsLSSDVehicles;
    private List<DispatchableVehicle> DavisLSSDVehicles;
    private List<DispatchableVehicle> RHPDVehicles;
    private List<DispatchableVehicle> DPPDVehicles;
    private List<DispatchableVehicle> EastLSPDVehicles;
    private List<DispatchableVehicle> VWPDVehicles;
    private List<DispatchableVehicle> PoliceHeliVehicles;
    private List<DispatchableVehicle> SheriffHeliVehicles;
    private List<DispatchableVehicle> ArmyVehicles;
    private List<DispatchableVehicle> USMCVehicles;
    private List<DispatchableVehicle> USAFVehicles;
    private List<DispatchableVehicle> LSLifeguardVehicles;
    private List<DispatchableVehicle> Firetrucks;
    private List<DispatchableVehicle> Amublance1;
    private List<DispatchableVehicle> Amublance2;
    private List<DispatchableVehicle> Amublance3;
    private List<DispatchableVehicle> NYSPVehicles;
    private List<DispatchableVehicle> MerryweatherPatrolVehicles;
    private List<DispatchableVehicle> BobcatSecurityVehicles;
    private List<DispatchableVehicle> GroupSechsVehicles;
    private List<DispatchableVehicle> SecuroservVehicles;
    private List<DispatchableVehicle> BorderPatrolVehicles;
    private List<DispatchableVehicle> NOOSEPIAVehicles;
    private List<DispatchableVehicle> NOOSESEPVehicles;
    public List<DispatchableVehicle> MarshalsServiceVehicles;
    private List<DispatchableVehicle> OffDutyCopVehicles;
    private List<DispatchableVehicle> LCPDVehicles;
    private List<DispatchableVehicle> TaxiVehicles;

    public DispatchableVehicle TaxiBroadWay;
    public DispatchableVehicle TaxiEudora;
    public DispatchableVehicle GauntletUndercoverSAHP;
    public DispatchableVehicle GauntletSAHP;
    public DispatchableVehicle GauntletSAHPSlicktop;
    public DispatchableVehicle SheriffStanierNew;
    public DispatchableVehicle ParkRangerStanierNew;
    public DispatchableVehicle SAHPStanierNew;
    public DispatchableVehicle SAHPStanierSlicktopNew;
    public DispatchableVehicle AleutianSecurityBobCat;
    public DispatchableVehicle AleutianSecurityG6;
    public DispatchableVehicle AleutianSecurityMW;
    public DispatchableVehicle AleutianSecuritySECURO;
    public DispatchableVehicle AsteropeSecurityBobCat;
    public DispatchableVehicle AsteropeSecurityG6;
    public DispatchableVehicle AsteropeSecurityMW;
    public DispatchableVehicle AsteropeSecuritySECURO;

    public List<DispatchableVehicleGroup> AllVehicles => VehicleGroupLookup;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("DispatchableVehicles*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable Vehicles config: {ConfigFile.FullName}", 0);
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable Vehicles config  {ConfigFileName}", 0);
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Dispatchable Vehicles config found, creating default", 0);
            SetupDefaults();
            DefaultConfig_Simple();
            DefaultConfig_LosSantos2008();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig();
        }
    }
    public void Setup()
    {
        foreach(DispatchableVehicleGroup dvg in VehicleGroupLookup)
        {
            foreach(DispatchableVehicle dv in dvg.DispatchableVehicles)
            {
                dv.Setup();
            }
        }
    }
    public List<DispatchableVehicle> GetVehicleData(string dispatchableVehicleGroupID)
    {
        return VehicleGroupLookup.FirstOrDefault(x => x.DispatchableVehicleGroupID == dispatchableVehicleGroupID)?.DispatchableVehicles;
    }
    private void SetupDefaults()
    {
        SharedCopCars();
        FEJSecurity();
        //Cops
        UnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100),
            new DispatchableVehicle("fbi", 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle("fbi2", 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
        };
        CoastGuardVehicles = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle("predator", 75, 50),
            //new DispatchableVehicle("dinghy", 0, 25) { MaxWantedLevelSpawn = 2, },

            new DispatchableVehicle("dinghy5", 50, 50) { RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 1,MaxOccupants = 2, MaxWantedLevelSpawn = 2, },
            new DispatchableVehicle("dinghy5", 0, 100) { RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0,ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 4,MaxOccupants = 4, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, },

            new DispatchableVehicle("seasparrow",0,50) { RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 38,MinWantedLevelSpawn = 2,MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle("frogger",0,50) { RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 38,MinWantedLevelSpawn = 2,MaxWantedLevelSpawn = 4 },

            //seasparrow
            //dingy5 seat 3 = gunner seat
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },
            
        };
        ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) { MaxRandomDirtLevel = 15.0f },
            ParkRangerStanierNew, };
        FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },




        };
        NOOSEVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 35) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, CaninePossibleSeats = new List<int>() { 1,2 } },
            new DispatchableVehicle("fbi", 0, 40) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 4,MaxOccupants = 5 }};
        PrisonVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 70, 70),
            new DispatchableVehicle("police4", 30, 30) };
        LSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 48,35) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 48, 35),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4, MinWantedLevelSpawn = 3,CaninePossibleSeats = new List<int>{ 1,2 } }};
        SAHPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 20, 10) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop", GroupName = "Motorcycle" },
            //new DispatchableVehicle("police4", 5, 10) { GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP" },
            GauntletUndercoverSAHP,

            GauntletSAHP,
            GauntletSAHPSlicktop,

            SAHPStanierNew,
            SAHPStanierSlicktopNew
        };






        LSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 10, 10) { MaxRandomDirtLevel = 10.0f,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) { MaxRandomDirtLevel = 10.0f },
            SheriffStanierNew,





        };
        BCSOVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 10, 10) { MaxRandomDirtLevel = 10.0f,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) { MaxRandomDirtLevel = 10.0f },
            SheriffStanierNew,
        };
        VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 70, 70),
            SheriffStanierNew
        };
        DavisLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 30, 30),
            SheriffStanierNew
        };
        RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police2", 100, 85){ VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,25) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        DPPDVehicles = new List<DispatchableVehicle>() {
            //new DispatchableVehicle("police3", 100, 75) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,75) } },
            new DispatchableVehicle("police2", 100, 85){ VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,25) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        VWPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        PoliceHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("polmav", 1,150) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 } };
        SheriffHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 1,150) { MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 },
        };

        ArmyVehicles = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("insurgent3", 5,25) { ForceStayInSeats = new List<int>() { 7 }, FirstPassengerIndex = 7, MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

            //Heavy
            new DispatchableVehicle("rhino", 0, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            //new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

            //Heli
            new DispatchableVehicle("valkyrie2", 0,75) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("buzzard",0,20) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},
            new DispatchableVehicle("hunter",0,20) { RequiredPedGroup = "Pilot", RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 2, MaxOccupants = 2,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,50)  } },
        };

        USMCVehicles = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("insurgent3", 5,25) { ForceStayInSeats = new List<int>() { 7 }, FirstPassengerIndex = 7,MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            
            //HELI
            new DispatchableVehicle("cargobob",0,20) { RequiredPedGroup = "Pilot", RequiredGroupIsDriverOnly = true, RequiredPrimaryColorID = 153, RequiredSecondaryColorID = 153, MinOccupants = 3, MaxOccupants = 4},

            //Boat
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },
        };

        USAFVehicles = new List<DispatchableVehicle>()
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            
            //HELI
           
            //JETS
            new DispatchableVehicle("lazer",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,150)  } },
            new DispatchableVehicle("hydra",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,150)  } },
            new DispatchableVehicle("strikeforce",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1,SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.InAirVehicle,150)  } },
        };

        //ArmyVehicles = new List<DispatchableVehicle>()
        //{
        //    //new DispatchableVehicle("crusader", 85,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    //new DispatchableVehicle("barracks", 15,75) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("insurgent3", 0,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },


        //    new DispatchableVehicle("rhino", 0, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("valkyrie2", 0,75) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },

        //    new DispatchableVehicle("lazer",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("hydra",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("strikeforce",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //};

        //USMCVehicles = new List<DispatchableVehicle>() 
        //{
        //    //new DispatchableVehicle("crusader", 85,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    //new DispatchableVehicle("barracks", 15,75) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("insurgent3", 0,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },


        //    new DispatchableVehicle("rhino", 0, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("valkyrie2", 0,75) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },

        //    new DispatchableVehicle("lazer",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("hydra",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("strikeforce",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //};

        //USAFVehicles = new List<DispatchableVehicle>() 
        //{
        //    //new DispatchableVehicle("crusader", 85,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    //new DispatchableVehicle("barracks", 15,75) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("insurgent3", 0,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },


        //    new DispatchableVehicle("rhino", 0, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
        //    new DispatchableVehicle("valkyrie2", 0,75) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

        //    new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 152, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 6,MaxWantedLevelSpawn = 10, },

        //    new DispatchableVehicle("lazer",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("hydra",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //    new DispatchableVehicle("strikeforce",0,1) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,MaxOccupants = 1 },
        //};

        LSLifeguardVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50),
            new DispatchableVehicle("freecrawler",5,5) { RequiredVariation = new VehicleVariation() { VehicleMods = new List<VehicleMod>() {new VehicleMod(48, 7) } } },
            new DispatchableVehicle("seashark2",100,100),
        };

        LCPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        Firetrucks = new List<DispatchableVehicle>() {
            new DispatchableVehicle("firetruk", 100, 100) { MinOccupants = 2, MaxOccupants = 4 }  };
        Amublance1 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };
        Amublance2 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } } };
        Amublance3 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };
        NYSPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeold1", 50, 50) { MaxRandomDirtLevel = 15.0f },
            new DispatchableVehicle("policeold2", 50, 50) { MaxRandomDirtLevel = 15.0f }, };
        MerryweatherPatrolVehicles = new List<DispatchableVehicle>()
        { 
            new DispatchableVehicle("dilettante2", 100, 100), 
        };
        BobcatSecurityVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dilettante", 100, 100),
        };
        GroupSechsVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("blista", 100, 100),
        };
        SecuroservVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dilettante", 100, 100),
        };
        BorderPatrolVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fbi2", 100, 100) { MaxRandomDirtLevel = 15.0f },
        };
        NOOSEPIAVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fbi", 70, 70),
            new DispatchableVehicle("fbi2", 30, 30),
        };
        NOOSESEPVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fbi", 70, 70),
            new DispatchableVehicle("fbi2", 30, 30),
        };
        MarshalsServiceVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("police4", 50, 50),
            new DispatchableVehicle("fbi", 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle("fbi2", 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
        };

        OffDutyCopVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("buffalo", 20, 0) { MaxOccupants = 1, MaxWantedLevelSpawn = 0,RequiredPedGroup = "OffDuty", },
            new DispatchableVehicle("stanier", 20, 0) { MaxOccupants = 1,MaxWantedLevelSpawn = 0,RequiredPedGroup = "OffDuty", },
            new DispatchableVehicle("granger", 20, 0) { MaxOccupants = 1,MaxWantedLevelSpawn = 0,RequiredPedGroup = "OffDuty", },
            new DispatchableVehicle("fugitive", 20, 0) { MaxOccupants = 1,MaxWantedLevelSpawn = 0,RequiredPedGroup = "OffDuty", },
            new DispatchableVehicle("washington", 20, 0) { MaxOccupants = 1,MaxWantedLevelSpawn = 0,RequiredPedGroup = "OffDuty", },
        };

        //Gangs
        LostMCVehicles = new List<DispatchableVehicle>();
        VarriosVehicles = new List<DispatchableVehicle>();
        BallasVehicles = new List<DispatchableVehicle>();
        VagosVehicles = new List<DispatchableVehicle>();
        MarabuntaVehicles = new List<DispatchableVehicle>();
        KoreanVehicles = new List<DispatchableVehicle>();
        TriadVehicles = new List<DispatchableVehicle>();
        YardieVehicles = new List<DispatchableVehicle>();
        DiablosVehicles = new List<DispatchableVehicle>();
        GambettiVehicles = new List<DispatchableVehicle>();
        PavanoVehicles = new List<DispatchableVehicle>();
        LupisellaVehicles = new List<DispatchableVehicle>();
        MessinaVehicles = new List<DispatchableVehicle>();
        AncelottiVehicles = new List<DispatchableVehicle>();
        ArmeniaVehicles = new List<DispatchableVehicle>();
        CartelVehicles = new List<DispatchableVehicle>();
        RedneckVehicles = new List<DispatchableVehicle>();
        FamiliesVehicles = new List<DispatchableVehicle>();

        DispatchableVehicles_Gangs dispatchableVehiclesGangs = new DispatchableVehicles_Gangs();
        dispatchableVehiclesGangs.DefaultConfig();
        FamiliesVehicles.AddRange(dispatchableVehiclesGangs.FamiliesVehicles);
        VarriosVehicles.AddRange(dispatchableVehiclesGangs.VarriosVehicles);
        BallasVehicles.AddRange(dispatchableVehiclesGangs.BallasVehicles);
        VagosVehicles.AddRange(dispatchableVehiclesGangs.VagosVehicles);
        MarabuntaVehicles.AddRange(dispatchableVehiclesGangs.MarabuntaVehicles);
        KoreanVehicles.AddRange(dispatchableVehiclesGangs.KoreanVehicles);
        TriadVehicles.AddRange(dispatchableVehiclesGangs.TriadVehicles);
        DiablosVehicles.AddRange(dispatchableVehiclesGangs.DiablosVehicles);
        GambettiVehicles.AddRange(dispatchableVehiclesGangs.GambettiVehicles);
        PavanoVehicles.AddRange(dispatchableVehiclesGangs.PavanoVehicles);
        LupisellaVehicles.AddRange(dispatchableVehiclesGangs.LupisellaVehicles);
        MessinaVehicles.AddRange(dispatchableVehiclesGangs.MessinaVehicles);
        AncelottiVehicles.AddRange(dispatchableVehiclesGangs.AncelottiVehicles);
        CartelVehicles.AddRange(dispatchableVehiclesGangs.CartelVehicles);
        RedneckVehicles.AddRange(dispatchableVehiclesGangs.RedneckVehicles);
        ArmeniaVehicles.AddRange(dispatchableVehiclesGangs.ArmenianVehicles);
        YardieVehicles.AddRange(dispatchableVehiclesGangs.YardiesVehicles);
        LostMCVehicles.AddRange(dispatchableVehiclesGangs.LostVehicles);

        //Other
        TaxiBroadWay = new DispatchableVehicle("broadway", 4, 4)
        {
            DebugName = "broadway_taxi_PeterBadoingy_DLCDespawn",
            MaxOccupants = 2,
            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 89,
            RequiresDLC = true,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 89,
                SecondaryColor = 89,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelType = 8,
                VehicleExtras = new List<VehicleExtra>()
                    {
                        new VehicleExtra(1,true),new VehicleExtra(2,true),
                    },
                VehicleToggles = new List<VehicleToggle>()
                    {
                        new VehicleToggle(18,true),
                    },
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,3),new VehicleMod(2,2),new VehicleMod(3,2),new VehicleMod(4,3),new VehicleMod(5,3),new VehicleMod(7,1),new VehicleMod(8,2),new VehicleMod(9,2),new VehicleMod(11,3),new VehicleMod(12,2),new VehicleMod(13,2),new VehicleMod(14,2),
                        new VehicleMod(15,1),new VehicleMod(16,4),new VehicleMod(23,10),new VehicleMod(48,11),new VehicleMod(50,3),
                    },
                InteriorColor = 13,
                DashboardColor = 111,
            },
        };
        TaxiEudora = new DispatchableVehicle("eudora", 4, 4)
        {
            DebugName = "eudora_taxi_PeterBadoingy_DLCDespawn",
            MaxOccupants = 2,
            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 89,
            RequiresDLC = true,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 89,
                SecondaryColor = 89,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelType = 2,
                PearlescentColor = 88,
                VehicleExtras = new List<VehicleExtra>()
                {

                },
                VehicleToggles = new List<VehicleToggle>()
                    {
                        new VehicleToggle(18,true),
                    },
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,2),new VehicleMod(2,2),new VehicleMod(3,0),new VehicleMod(4,2),new VehicleMod(5,0),new VehicleMod(6,1),new VehicleMod(7,5),new VehicleMod(9,7),new VehicleMod(10,0),new VehicleMod(11,3),new VehicleMod(12,2),new VehicleMod(13,2),
                        new VehicleMod(15,0),new VehicleMod(16,4),new VehicleMod(23,10),new VehicleMod(48,10),new VehicleMod(50,3),
                    },
                InteriorColor = 22,
                DashboardColor = 158,
            },
        };
        TaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 92, 92),
            TaxiBroadWay,
            TaxiEudora,
        };
    }

    private DispatchableVehicle CreateDefaultPatriot3Humvee(int ambientPercent, int wantedPercent, int Color)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle("patriot3", ambientPercent, wantedPercent);
        toReturn.MaxRandomDirtLevel = 15.0f;
        toReturn.RequiredVariation = new VehicleVariation()
        {
            PrimaryColor = Color,
            SecondaryColor = Color,
            PearlescentColor = 12,
            WheelColor = 12,
            VehicleMods = new List<VehicleMod>()
            { 
                new VehicleMod(0,1),
                new VehicleMod(3,2),
                new VehicleMod(5,0),
                new VehicleMod(8,1),
                new VehicleMod(28,7),
            },
        };
       // SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    private void SharedCopCars()
    {
        GauntletUndercoverSAHP = new DispatchableVehicle(PoliceGauntlet, 5, 0) //Undercover Gauntlet
        {
            DebugName = "UCgauntlet_PeterBadoingy_DLC",
            MaxWantedLevelSpawn = 0,
            MaxOccupants = 1,
            RequiresDLC = true,
            GroupName = "SAHPUndercover",
            RequiredPedGroup = "StandardSAHP",
            RequiredPrimaryColorID = 28,
            RequiredSecondaryColorID = 28,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 28,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelColor = 0,
                WheelType = 1,
                WindowTint = 0,
                PearlescentColor = 0,
                LicensePlate = new LSR.Vehicles.LicensePlate("",0, false),
                VehicleExtras = new List<VehicleExtra>()
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,false),
                        new VehicleExtra(2,false),
                        new VehicleExtra(3,false),
                        new VehicleExtra(4,false),
                        new VehicleExtra(5,false),
                        new VehicleExtra(6,false),
                    },
                //VehicleToggles = new List<VehicleToggle>()
                //    {
                //        new VehicleToggle(17,false),
                //        new VehicleToggle(18,false),
                //        new VehicleToggle(19,false),
                //        new VehicleToggle(20,false),
                //        new VehicleToggle(21,false),
                //        new VehicleToggle(22,false),
                //    },
                VehicleMods = new List<VehicleMod>()
                    {
                        //new VehicleMod(0,-1),
                        //new VehicleMod(1,-1),
                        //new VehicleMod(2,-1),
                        //new VehicleMod(3,-1),
                        //new VehicleMod(4,-1),
                        //new VehicleMod(5,-1),
                        //new VehicleMod(6,-1),
                        //new VehicleMod(7,-1),
                        //new VehicleMod(8,-1),
                        //new VehicleMod(9,-1),
                        //new VehicleMod(10,-1),
                        //new VehicleMod(11,-1),
                        //new VehicleMod(12,-1),
                        //new VehicleMod(13,-1),
                        //new VehicleMod(14,-1),
                        //new VehicleMod(15,-1),
                        new VehicleMod(16,-1),
                        new VehicleMod(23,12),
                        //new VehicleMod(24,-1),
                        //new VehicleMod(25,-1),
                        //new VehicleMod(26,-1),
                        //new VehicleMod(27,-1),
                        //new VehicleMod(28,-1),
                        //new VehicleMod(29,-1),
                        //new VehicleMod(30,-1),
                        //new VehicleMod(31,-1),
                        //new VehicleMod(32,-1),
                        //new VehicleMod(33,-1),
                        //new VehicleMod(34,-1),
                        //new VehicleMod(35,-1),
                        //new VehicleMod(36,-1),
                        //new VehicleMod(37,-1),
                        //new VehicleMod(38,-1),
                        //new VehicleMod(39,-1),
                        //new VehicleMod(40,-1),
                        //new VehicleMod(41,-1),
                        //new VehicleMod(42,-1),
                        new VehicleMod(43,1),
                        //new VehicleMod(44,-1),
                        //new VehicleMod(45,-1),
                        //new VehicleMod(46,-1),
                        //new VehicleMod(47,-1),
                        //new VehicleMod(48,-1),
                        //new VehicleMod(49,-1),
                        //new VehicleMod(50,-1),
                    },
            }
        };
        GauntletSAHP = new DispatchableVehicle(PoliceGauntlet, 15, 20)
        {
            DebugName = "SAHPgauntlet_PeterBadoingy_DLC",
            MaxOccupants = 2,
            GroupName = "StandardSAHP",
            RequiredPedGroup = "StandardSAHP",
            RequiresDLC = true,
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            ForcedPlateType = 4,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
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
                            new DispatchableVehicleModValue(3,100),
                        },
                    },
                },
        };
        GauntletSAHPSlicktop = new DispatchableVehicle(PoliceGauntlet, 10, 15)
        {
            DebugName = "SAHPgauntlet_PeterBadoingy_DLC",
            MaxOccupants = 2,
            GroupName = "StandardSAHP",
            RequiredPedGroup = "StandardSAHP",
            RequiresDLC = true,
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            ForcedPlateType = 4,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
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
                            new DispatchableVehicleModValue(3,100),
                        },
                    },
                },
        };
        SAHPStanierNew = new DispatchableVehicle("police5", 30, 40)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            GroupName = "StandardSAHP",
            RequiredPedGroup = "StandardSAHP",
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
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
                            new DispatchableVehicleModValue(7,100),
                        },
                    },
                },
        };
        SAHPStanierSlicktopNew = new DispatchableVehicle("police5", 20, 30)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            GroupName = "StandardSAHP",
            RequiredPedGroup = "StandardSAHP",
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
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
                            new DispatchableVehicleModValue(7,100),
                        },
                    },
                },
        };

        SheriffStanierNew = new DispatchableVehicle("police5", 50, 50)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
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
                            new DispatchableVehicleModValue(8,100),
                        },
                    },
                },
        };
        ParkRangerStanierNew = new DispatchableVehicle("police5", 15, 20)
        {
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 15.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,true,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,false,100,4),
                    new DispatchableVehicleExtra(5,false,100,5),
                },
            VehicleMods = new List<DispatchableVehicleMod>()
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
                            new DispatchableVehicleModValue(21,100),
                        },
                    },
                },
        };
    }
    private void FEJSecurity()
    {
        AleutianSecurityBobCat = new DispatchableVehicle("aleutian", 5, 0) //Undercover Gauntlet
        {
            DebugName = "AleutianSecurity_BC_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelColor = 156,
                WheelType = 4,
                WindowTint = 0,
                PearlescentColor = 73,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,5),
                        new VehicleMod(2,1),
                        new VehicleMod(3,1),
                        new VehicleMod(15,3),
                        new VehicleMod(23,9),
                        new VehicleMod(48,4),//5,6,9 BC, G6, MW, SECURO
                    },
            }
        };
        AleutianSecurityG6 = new DispatchableVehicle("aleutian", 20, 20)
        {
            DebugName = "AleutianSecurity_G6_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelColor = 156,
                WheelType = 4,
                WindowTint = 0,
                PearlescentColor = 73,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,5),
                        new VehicleMod(2,1),
                        new VehicleMod(3,1),
                        new VehicleMod(15,3),
                        new VehicleMod(23,9),
                        new VehicleMod(48,5),//5,6,9 BC, G6, MW, SECURO
                    },
            }
        };
        AleutianSecurityMW = new DispatchableVehicle("aleutian", 20, 20)
        {
            DebugName = "AleutianSecurity_MW_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelColor = 156,
                WheelType = 4,
                WindowTint = 0,
                PearlescentColor = 73,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,5),
                        new VehicleMod(2,1),
                        new VehicleMod(3,1),
                        new VehicleMod(15,3),
                        new VehicleMod(23,9),
                        new VehicleMod(48,6),//5,6,9 BC, G6, MW, SECURO
                    },
            }
        };
        AleutianSecuritySECURO = new DispatchableVehicle("aleutian", 20, 20)
        {
            DebugName = "AleutianSecurity_SECURO_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelColor = 156,
                WheelType = 4,
                WindowTint = 0,
                PearlescentColor = 73,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(1,5),
                        new VehicleMod(2,1),
                        new VehicleMod(3,1),
                        new VehicleMod(15,3),
                        new VehicleMod(23,9),
                        new VehicleMod(48,9),//5,6,9 BC, G6, MW, SECURO
                    },
            }
        };

        AsteropeSecuritySECURO = new DispatchableVehicle("asterope2", 20, 20)
        {
            DebugName = "AsteropeSecurity_SECURO_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                WheelColor = 156,
                WheelType = 1,
                WindowTint = 0,
                PearlescentColor = 0,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(23,10),
                        new VehicleMod(48,6),
                    },
            }
        };
        AsteropeSecurityMW = new DispatchableVehicle("asterope2", 20, 20)
        {
            DebugName = "AsteropeSecurity_MW_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                WheelColor = 156,
                WheelType = 1,
                WindowTint = 0,
                PearlescentColor = 0,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(23,10),
                        new VehicleMod(48,7),
                    },
            }
        };
        AsteropeSecurityBobCat = new DispatchableVehicle("asterope2", 20, 20)
        {
            DebugName = "AsteropeSecurity_BobCat_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                WheelColor = 156,
                WheelType = 1,
                WindowTint = 0,
                PearlescentColor = 0,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(23,10),
                        new VehicleMod(48,8),
                    },
            }
        };
        AsteropeSecurityG6 = new DispatchableVehicle("asterope2", 20, 20)
        {
            DebugName = "AsteropeSecurity_G6_DLC",
            RequiresDLC = true,
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                WheelColor = 156,
                WheelType = 1,
                WindowTint = 0,
                PearlescentColor = 0,
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(23,10),
                        new VehicleMod(48,9),
                    },
            }
        };


    }
    private void DefaultConfig()
    {
        VehicleGroupLookup = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles),
            new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles),
            new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles),
            new DispatchableVehicleGroup("SADFWParkRangersVehicles", ParkRangerVehicles),
            new DispatchableVehicleGroup("USNPSParkRangersVehicles", ParkRangerVehicles),
            new DispatchableVehicleGroup("LSDPRParkRangersVehicles", ParkRangerVehicles),


            new DispatchableVehicleGroup("FIBVehicles", FIBVehicles),
            new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles),
            new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles),
            new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles),
            new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles),
            new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles),
            new DispatchableVehicleGroup("BCSOVehicles", BCSOVehicles),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", VWHillsLSSDVehicles),
            new DispatchableVehicleGroup("DavisLSSDVehicles", DavisLSSDVehicles),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", VWHillsLSSDVehicles),
            new DispatchableVehicleGroup("LSPPVehicles", RHPDVehicles),
            new DispatchableVehicleGroup("LSIAPDVehicles", RHPDVehicles),
            new DispatchableVehicleGroup("RHPDVehicles", RHPDVehicles),
            new DispatchableVehicleGroup("DPPDVehicles", DPPDVehicles),
            new DispatchableVehicleGroup("VWPDVehicles", VWPDVehicles),
            new DispatchableVehicleGroup("EastLSPDVehicles", EastLSPDVehicles),
            new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles),
            new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles),
            new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles),
            new DispatchableVehicleGroup("USMCVehicles", USMCVehicles),
            new DispatchableVehicleGroup("USAFVehicles", USAFVehicles),
            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", MerryweatherPatrolVehicles),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", BobcatSecurityVehicles),
            new DispatchableVehicleGroup("GroupSechsVehicles", GroupSechsVehicles),
            new DispatchableVehicleGroup("SecuroservVehicles", SecuroservVehicles),
            new DispatchableVehicleGroup("LCPDVehicles", LCPDVehicles),
            new DispatchableVehicleGroup("BorderPatrolVehicles", BorderPatrolVehicles),
            new DispatchableVehicleGroup("NOOSEPIAVehicles", NOOSEPIAVehicles),
            new DispatchableVehicleGroup("NOOSESEPVehicles", NOOSESEPVehicles),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),
            new DispatchableVehicleGroup("LSLifeguardVehicles",LSLifeguardVehicles),


            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),

            new DispatchableVehicleGroup("GambettiVehicles", GambettiVehicles),
            new DispatchableVehicleGroup("PavanoVehicles", PavanoVehicles),
            new DispatchableVehicleGroup("LupisellaVehicles", LupisellaVehicles),
            new DispatchableVehicleGroup("MessinaVehicles", MessinaVehicles),
            new DispatchableVehicleGroup("AncelottiVehicles", AncelottiVehicles),
         
            new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles),
            new DispatchableVehicleGroup("CartelVehicles", CartelVehicles),
            new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles),
            new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles),

            //Other 
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),
        };
        Serialization.SerializeParams(VehicleGroupLookup, ConfigFileName);
        Serialization.SerializeParams(VehicleGroupLookup, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchableVehicles_EUP.xml");
    }
    public void DefaultConfig_LosSantos2008()
    {
        string PoliceStanierOld = "police8";

        List<DispatchableVehicle> LSPDVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceMerit, 25,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),

            new DispatchableVehicle(PoliceStanierOld, 20,20),


            new DispatchableVehicle(PoliceTransporter, 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3},
            //DONT HAVE A RIDER FOR THIS!//new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
         };
        List<DispatchableVehicle> LSSDVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, true, 100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 25,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 50, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 50, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() {7 } },
            //new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
         };
        List<DispatchableVehicle> SAHPVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { 
                new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(1, true, 70) } },
            new DispatchableVehicle(PoliceMerit, 25,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { 
                new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(2, true, 70) } },
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 45, 45) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(PoliceGranger, 10, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
        };

        List<DispatchableVehicle> BallasVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("patriot", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purp[le
        };
        List<DispatchableVehicle> KoreanVehicles_old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fq2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("prairie", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("oracle", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        List<DispatchableVehicle> MafiaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("washington", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("pmp600", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> ArmeniaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rocoto", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("merit", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> CartelVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("fxt", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };

        //Cop
        List<DispatchableVehicleGroup> OldVehicleLookupGroup = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles),
            new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles),
            new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles),
            new DispatchableVehicleGroup("FIBVehicles", FIBVehicles),
            new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles),
            new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles),
            new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles_Old),
            new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles_Old),
            new DispatchableVehicleGroup("BCSOVehicles", LSSDVehicles_Old),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", LSSDVehicles_Old),
            new DispatchableVehicleGroup("DavisLSSDVehicles", LSSDVehicles_Old),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", LSSDVehicles_Old),
            new DispatchableVehicleGroup("LSPPVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("LSIAPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("RHPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("DPPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("VWPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("EastLSPDVehicles", LSPDVehicles_Old),
            new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles),
            new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles),
            new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles),
            new DispatchableVehicleGroup("USMCVehicles", USMCVehicles),
            new DispatchableVehicleGroup("USAFVehicles", USAFVehicles),
            new DispatchableVehicleGroup("USMCVehicles", USMCVehicles),
            new DispatchableVehicleGroup("USAFVehicles", USAFVehicles),
            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", MerryweatherPatrolVehicles),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", BobcatSecurityVehicles),
            new DispatchableVehicleGroup("GroupSechsVehicles", GroupSechsVehicles),
            new DispatchableVehicleGroup("SecuroservVehicles", SecuroservVehicles),
            new DispatchableVehicleGroup("LCPDVehicles", LCPDVehicles),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),
            new DispatchableVehicleGroup("LSLifeguardVehicles",LSLifeguardVehicles),

            //Gang
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles_Old),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles_old),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),

            new DispatchableVehicleGroup("GambettiVehicles", MafiaVehicles_Old),
            new DispatchableVehicleGroup("PavanoVehicles", MafiaVehicles_Old),
            new DispatchableVehicleGroup("LupisellaVehicles", MafiaVehicles_Old),
            new DispatchableVehicleGroup("MessinaVehicles", MafiaVehicles_Old),
            new DispatchableVehicleGroup("AncelottiVehicles", MafiaVehicles_Old),

            new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles_Old),
            new DispatchableVehicleGroup("CartelVehicles", CartelVehicles_Old),
            new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles),
            new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles),

            //Other
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles)
        };


        Serialization.SerializeParams(OldVehicleLookupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\DispatchableVehicles_LosSantos2008.xml");

    }
    private void DefaultConfig_Simple()
    {
        List<DispatchableVehicleGroup> SimpleVehicleLoopupGroup = new List<DispatchableVehicleGroup>
        {
            //Police
            new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles),
            new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles),
            new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles),
            new DispatchableVehicleGroup("FIBVehicles", FIBVehicles),
            new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles),
            new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles),
            new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles),
            new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles),
            new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles),
            new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles),
            new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles),
            new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles),
            new DispatchableVehicleGroup("USMCVehicles", USMCVehicles),
            new DispatchableVehicleGroup("USAFVehicles", USAFVehicles),
            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", MerryweatherPatrolVehicles),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", BobcatSecurityVehicles),
            new DispatchableVehicleGroup("GroupSechsVehicles", GroupSechsVehicles),
            new DispatchableVehicleGroup("SecuroservVehicles", SecuroservVehicles),
            new DispatchableVehicleGroup("LCPDVehicles", LCPDVehicles),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),
            new DispatchableVehicleGroup("LSLifeguardVehicles",LSLifeguardVehicles),

            //Gang
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles),
            new DispatchableVehicleGroup("CartelVehicles", CartelVehicles),
            new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles),
            new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles),

            //Other
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles)
        };
        Serialization.SerializeParams(SimpleVehicleLoopupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\DispatchableVehicles_Simple.xml");
    }
    private void DefaultConfig_FullExpandedJurisdiction()
    {

        DispatchableVehicles_FEJ dispatchableVehicles_FEJ = new DispatchableVehicles_FEJ(this);
        dispatchableVehicles_FEJ.DefaultConfig();

        List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", dispatchableVehicles_FEJ.UnmarkedVehicles_FEJ),
            new DispatchableVehicleGroup("CoastGuardVehicles", dispatchableVehicles_FEJ.CoastGuardVehicles_FEJ),

            new DispatchableVehicleGroup("ParkRangerVehicles", dispatchableVehicles_FEJ.ParkRangerVehicles_FEJ),//san andreas state parks
            new DispatchableVehicleGroup("SADFWParkRangersVehicles", dispatchableVehicles_FEJ.SADFWParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("USNPSParkRangersVehicles", dispatchableVehicles_FEJ.USNPSParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("LSDPRParkRangersVehicles", dispatchableVehicles_FEJ.LSDPRParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("LSLifeguardVehicles",dispatchableVehicles_FEJ.LSLifeguardVehicles_FEJ),

            new DispatchableVehicleGroup("FIBVehicles", dispatchableVehicles_FEJ.FIBVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEVehicles", dispatchableVehicles_FEJ.NOOSEVehicles_FEJ),
            new DispatchableVehicleGroup("PrisonVehicles", dispatchableVehicles_FEJ.PrisonVehicles_FEJ),
            new DispatchableVehicleGroup("LSPDVehicles", dispatchableVehicles_FEJ.LSPDVehicles_FEJ),
            new DispatchableVehicleGroup("SAHPVehicles", dispatchableVehicles_FEJ.SAHPVehicles_FEJ),
            new DispatchableVehicleGroup("LSSDVehicles", dispatchableVehicles_FEJ.LSSDVehicles_FEJ),
            new DispatchableVehicleGroup("BCSOVehicles", dispatchableVehicles_FEJ.BCSOVehicles_FEJ),
            new DispatchableVehicleGroup("LSIAPDVehicles", dispatchableVehicles_FEJ.LSIAPDVehicles_FEJ),
            new DispatchableVehicleGroup("LSPPVehicles", dispatchableVehicles_FEJ.LSPPVehicles_FEJ),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", dispatchableVehicles_FEJ.VWHillsLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("DavisLSSDVehicles", dispatchableVehicles_FEJ.DavisLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", dispatchableVehicles_FEJ.MajesticLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("RHPDVehicles", dispatchableVehicles_FEJ.RHPDVehicles_FEJ),
            new DispatchableVehicleGroup("DPPDVehicles", dispatchableVehicles_FEJ.DPPDVehicles_FEJ),
            new DispatchableVehicleGroup("VWPDVehicles", dispatchableVehicles_FEJ.VWPDVehicles_FEJ),
            new DispatchableVehicleGroup("EastLSPDVehicles", dispatchableVehicles_FEJ.EastLSPDVehicles_FEJ),
            new DispatchableVehicleGroup("PoliceHeliVehicles", dispatchableVehicles_FEJ.PoliceHeliVehicles_FEJ),
            new DispatchableVehicleGroup("SheriffHeliVehicles", dispatchableVehicles_FEJ.SheriffHeliVehicles_FEJ),


            new DispatchableVehicleGroup("ArmyVehicles", dispatchableVehicles_FEJ.ArmyVehicles_FEJ),
            new DispatchableVehicleGroup("USMCVehicles", dispatchableVehicles_FEJ.USMCVehicles_FEJ),
            new DispatchableVehicleGroup("USAFVehicles", dispatchableVehicles_FEJ.USAFVehicles_FEJ),


            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", dispatchableVehicles_FEJ.NYSPVehicles_FEJ),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", dispatchableVehicles_FEJ.MerryweatherPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", dispatchableVehicles_FEJ.BobcatSecurityVehicles_FEJ),
            new DispatchableVehicleGroup("GroupSechsVehicles", dispatchableVehicles_FEJ.GroupSechsVehicles_FEJ),
            new DispatchableVehicleGroup("SecuroservVehicles", dispatchableVehicles_FEJ.SecuroservVehicles_FEJ),
            new DispatchableVehicleGroup("LCPDVehicles", dispatchableVehicles_FEJ.LCPDVehicles_FEJ),

            new DispatchableVehicleGroup("BorderPatrolVehicles", dispatchableVehicles_FEJ.BorderPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEPIAVehicles", dispatchableVehicles_FEJ.NOOSEPIAVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSESEPVehicles", dispatchableVehicles_FEJ.NOOSESEPVehicles_FEJ),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", dispatchableVehicles_FEJ.MarshalsServiceVehicles_FEJ),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

            //Gang stuff
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),
            new DispatchableVehicleGroup("GambettiVehicles", GambettiVehicles),
            new DispatchableVehicleGroup("PavanoVehicles", PavanoVehicles),
            new DispatchableVehicleGroup("LupisellaVehicles", LupisellaVehicles),
            new DispatchableVehicleGroup("MessinaVehicles", MessinaVehicles),
            new DispatchableVehicleGroup("AncelottiVehicles", AncelottiVehicles),
            new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles),
            new DispatchableVehicleGroup("CartelVehicles", CartelVehicles),
            new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles),
            new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles),

            //Other
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),
            new DispatchableVehicleGroup("DowntownTaxiVehicles", dispatchableVehicles_FEJ.DowntownTaxiVehicles),
            new DispatchableVehicleGroup("HellTaxiVehicles", dispatchableVehicles_FEJ.HellTaxiVehicles),
            new DispatchableVehicleGroup("PurpleTaxiVehicles", dispatchableVehicles_FEJ.PurpleTaxiVehicles),
            new DispatchableVehicleGroup("ShitiTaxiVehicles", dispatchableVehicles_FEJ.ShitiTaxiVehicles),
            new DispatchableVehicleGroup("SunderedTaxiVehicles",dispatchableVehicles_FEJ.SunderedTaxiVehicles),
            //

        };

        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\DispatchableVehicles_FullExpandedJurisdiction.xml");
    }


}


