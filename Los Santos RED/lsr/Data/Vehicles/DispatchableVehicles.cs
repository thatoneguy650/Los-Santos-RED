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


    private List<DispatchableVehicle> NorthHollandVehicles;
    private List<DispatchableVehicle> PetrovicVehicles;
    private List<DispatchableVehicle> SpanishLordsVehicles;
    private List<DispatchableVehicle> UptownRidersVehicles;
    private List<DispatchableVehicle> AngelsOfDeathVehicles;

    private List<DispatchableVehicle> ArmeniaVehicles;
    private List<DispatchableVehicle> CartelVehicles;
    private List<DispatchableVehicle> RedneckVehicles;
    private List<DispatchableVehicle> FamiliesVehicles;
    public List<DispatchableVehicle> UnmarkedVehicles;
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
    private List<DispatchableVehicle> LSPPVehicles;
    private List<DispatchableVehicle> LSIAPDVehicles;
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
    public List<DispatchableVehicle> DOAVehicles;

    private List<DispatchableVehicle> OffDutyCopVehicles;
    private List<DispatchableVehicle> LCPDVehicles;
    private List<DispatchableVehicle> TaxiVehicles;
    private List<DispatchableVehicle> RideshareVehicles;

    public DispatchableVehicle TaxiBroadWay;
    public DispatchableVehicle TaxiEudora;
    public DispatchableVehicle GauntletUndercoverSAHP;
    public DispatchableVehicle LSPDStanierNew;
    private DispatchableVehicle LSIAPDStanierNew;
    public DispatchableVehicle GauntletSAHP;
    public DispatchableVehicle GauntletSAHPSlicktop;
    private DispatchableVehicle LSPPStanierNew;
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
    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;

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
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig_SunshineDream();
            DefaultConfig();
            DefaultConfig_LibertyCity();
            DefaultConfig_LPP();
        }

//#if DEBUG
//        foreach (DispatchableVehicleGroup dispatchableVehicleGroup in VehicleGroupLookup)
//        {
//            dispatchableVehicleGroup.DispatchableVehicles.RemoveAll(x => x.ModelName == "jester2" || x.ModelName == "dune5" || x.ModelName == "blazer5");
//        }
//#endif
    }
    private void DefaultConfig_FullExpandedJurisdiction()
    {
        DispatchableVehicles_FEJ = new DispatchableVehicles_FEJ(this);
        DispatchableVehicles_FEJ.DefaultConfig();
        //DefaultConfig_FullExpandedJurisdiction_2015();
        DefaultConfig_FullExpandedJurisdiction_Modern();
        DefaultConfig_LosSantos_2008();
        //DefaultConfig_FullExpandedJurisdiction_Stanier();
    }
    public void Setup(IPlateTypes plateTypes)
    {
        foreach(DispatchableVehicleGroup dvg in VehicleGroupLookup)
        {
            foreach(DispatchableVehicle dv in dvg.DispatchableVehicles)
            {
                dv.Setup(plateTypes);
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
            Create_PoliceTerminusVanilla(50,50,"SAPR"),
            Create_PoliceCaracaraVanilla(25,25,"SAPR"),
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
            //new DispatchableVehicle("police", 48,35) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            LSPDStanierNew,
            Create_PoliceTerminusVanilla(10,10,"LSPD"),
            Create_PoliceCaracaraVanilla(5,5,"LSPD"),
            new DispatchableVehicle("policeb", 10, 5) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop", GroupName = "Motorcycle" },
            new DispatchableVehicle("police2", 48, 35),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4, MinWantedLevelSpawn = 3,CaninePossibleSeats = new List<int>{ 1,2 }, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) } }};
        SAHPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 20, 10) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop", GroupName = "Motorcycle" },
            GauntletUndercoverSAHP,

            GauntletSAHP,
            GauntletSAHPSlicktop,

            SAHPStanierNew,
            SAHPStanierSlicktopNew,

            Create_PoliceTerminusVanilla(5,5,"SAHP"),
            Create_PoliceCaracaraVanilla(1,1,"SAHP"),
        };

        LSPPVehicles = new List<DispatchableVehicle>() {
            LSPPStanierNew,
            Create_PoliceTerminusVanilla(5,5,"LSPP"),
            Create_PoliceCaracaraVanilla(1,1,"LSPP"),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),};

        LSIAPDVehicles = new List<DispatchableVehicle>() {
            LSIAPDStanierNew,
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),};

        LSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 10, 10) { MaxRandomDirtLevel = 10.0f,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) { MaxRandomDirtLevel = 10.0f,CaninePossibleSeats = new List<int>{ 1,2 }, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) } },
            SheriffStanierNew,
            Create_PoliceTerminusVanilla(15,15,"LSSD"),
            Create_PoliceCaracaraVanilla(5,5,"LSSD"),
            new DispatchableVehicle("dinghy5", 0, 0) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
        };
        BCSOVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 10, 10) { MaxRandomDirtLevel = 10.0f,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) { MaxRandomDirtLevel = 10.0f,CaninePossibleSeats = new List<int>{ 1,2 }, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) } },
            SheriffStanierNew,
            Create_PoliceTerminusVanilla(15,15,"LSSD"),
            Create_PoliceCaracaraVanilla(5,5,"LSSD"),
            new DispatchableVehicle("dinghy5", 0, 0) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
        };
        VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 70, 70),
            SheriffStanierNew,
            Create_PoliceTerminusVanilla(15,15,"LSSD"),
            Create_PoliceCaracaraVanilla(5,5,"LSSD"),
            new DispatchableVehicle("dinghy5", 0, 0) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
        };
        DavisLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 30, 30) { CaninePossibleSeats = new List<int>{ 1,2 }, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) } },
            SheriffStanierNew,
        };
        RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police2", 100, 85){ VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,25) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        DPPDVehicles = new List<DispatchableVehicle>() {

            Create_PoliceTerminusVanilla(15,15,"DPPD"),
            new DispatchableVehicle("police2", 100, 85){ VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,25) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3,CaninePossibleSeats = new List<int>{ 1,2 }, SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) } } };
        VWPDVehicles = new List<DispatchableVehicle>() {
            LSPDStanierNew,
            //new DispatchableVehicle("police", 100,85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
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

        LSLifeguardVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50),
            new DispatchableVehicle("freecrawler",5,5) { RequiredVariation = new VehicleVariation() { VehicleMods = new List<VehicleMod>() {new VehicleMod(48, 7) } } },
            new DispatchableVehicle("seashark2",100,100),
        };

        LCPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police3", 100, 100),
            new DispatchableVehicle("police4", 2, 2)
        };
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
        DOAVehicles = new List<DispatchableVehicle>()
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


        NorthHollandVehicles = new List<DispatchableVehicle>();
        PetrovicVehicles = new List<DispatchableVehicle>();
        SpanishLordsVehicles = new List<DispatchableVehicle>();
        UptownRidersVehicles = new List<DispatchableVehicle>();
        AngelsOfDeathVehicles = new List<DispatchableVehicle>();

        NorthHollandVehicles.AddRange(dispatchableVehiclesGangs.NorthHollandVehicles);
        PetrovicVehicles.AddRange(dispatchableVehiclesGangs.PetrovicVehicles);
        SpanishLordsVehicles.AddRange(dispatchableVehiclesGangs.SpanishLordsVehicles);
        UptownRidersVehicles.AddRange(dispatchableVehiclesGangs.UptownRidersVehicles);
        AngelsOfDeathVehicles.AddRange(dispatchableVehiclesGangs.AngelsOfDeathVehicles);

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

        RideshareVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("granger2", 100, 100),
            new DispatchableVehicle("castigator", 100, 100),
            new DispatchableVehicle("cavalcade3", 100, 100),
            new DispatchableVehicle("vivanite", 100, 100),
            new DispatchableVehicle("aleutian", 100, 100),
            new DispatchableVehicle("asterope", 100, 100),
            new DispatchableVehicle("radi", 100, 100),
            new DispatchableVehicle("gresley", 100, 100),
        };

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
        LSPDStanierNew = new DispatchableVehicle("police5", 100, 100)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,true,100,4),
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
                            new DispatchableVehicleModValue(0,100),
                            new DispatchableVehicleModValue(1,100),
                            new DispatchableVehicleModValue(2,100),
                            new DispatchableVehicleModValue(4,100),
                        },
                    },
                },
        };
        LSIAPDStanierNew = new DispatchableVehicle("police5", 100, 100)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,true,100,4),
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
                            new DispatchableVehicleModValue(10,100),
                        },
                    },
                },
        };
        LSPPStanierNew = new DispatchableVehicle("police5", 100, 100)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,true,100,4),
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
                            new DispatchableVehicleModValue(14,100),
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
                    new DispatchableVehicleExtra(2,false,100,2),
                    new DispatchableVehicleExtra(3,false,100,3),
                    new DispatchableVehicleExtra(4,true,100,4),
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
            new DispatchableVehicleGroup("LSPPVehicles", LSPPVehicles),
            new DispatchableVehicleGroup("LSIAPDVehicles", LSIAPDVehicles),
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
            new DispatchableVehicleGroup("DOAVehicles",DOAVehicles),
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

            new DispatchableVehicleGroup("NorthHollandVehicles", NorthHollandVehicles),
            new DispatchableVehicleGroup("PetrovicVehicles", PetrovicVehicles),
            new DispatchableVehicleGroup("SpanishLordsVehicles", SpanishLordsVehicles),
            new DispatchableVehicleGroup("UptownRidersVehicles", UptownRidersVehicles),
            new DispatchableVehicleGroup("AngelsOfDeathVehicles", AngelsOfDeathVehicles),

            //Other 
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),


            new DispatchableVehicleGroup("RideshareVehicles", RideshareVehicles),

        };
        Serialization.SerializeParams(VehicleGroupLookup, ConfigFileName);
        Serialization.SerializeParams(VehicleGroupLookup, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchableVehicles_EUP.xml");
    }
    private void DefaultConfig_LibertyCity()
    {
        List<DispatchableVehicleGroup> LibertyVehicleGroupLookup = ExtensionsMethods.Extensions.DeepCopy(VehicleGroupLookup);
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "ASPVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "LCPDVehicles");


        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "UnmarkedVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "CoastGuardVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "USNPSParkRangersVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "BorderPatrolVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "NOOSEPIAVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "NOOSESEPVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "FIBVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "MarshalsServiceVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "DOAVehicles");


        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "DowntownTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "HellTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "PurpleTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "ShitiTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "SunderedTaxiVehicles");

        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.LCPDVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCPDHeliVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.LCPDHeliVehicles));

        
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("ASPHeliVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.ASPHeliVehicles));

        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("ASPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.ASPVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("UnmarkedVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.UnmarkedVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("CoastGuardVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.CoastGuardVehicles_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("USNPSParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.USNPSParkRangersVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("BorderPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.BorderPatrolVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("NOOSEPIAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.NOOSEPIAVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("NOOSESEPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.NOOSESEPVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FIBVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.FIBVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("MarshalsServiceVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.MarshalsServiceVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("DOAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.MarshalsServiceVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.LCTaxiVehicles_FEJ_LC));

        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FDNYFireVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.FDLCVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FDNYEMTVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.FDLCEMTVehicles_FEJ_LC));


        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("HMSVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_LC.HMSVehicles_FEJ_LC));


        Serialization.SerializeParams(LibertyVehicleGroupLookup, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\DispatchableVehicles_{StaticStrings.LibertyConfigSuffix}.xml");
    }
    private void DefaultConfig_LPP()
    {
        DispatchableVehicles_LPP dispatchableVehicles_LPP = new DispatchableVehicles_LPP(this);
        dispatchableVehicles_LPP.DefaultConfig();

        List<DispatchableVehicleGroup> LibertyVehicleGroupLookup = ExtensionsMethods.Extensions.DeepCopy(VehicleGroupLookup);
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "ASPVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "LCPDVehicles");


        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "UnmarkedVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "CoastGuardVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "USNPSParkRangersVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "BorderPatrolVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "NOOSEPIAVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "NOOSESEPVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "FIBVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "MarshalsServiceVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "DOAVehicles");


        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "DowntownTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "HellTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "PurpleTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "ShitiTaxiVehicles");
        LibertyVehicleGroupLookup.RemoveAll(x => x.DispatchableVehicleGroupID == "SunderedTaxiVehicles");






        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCPDVehicles", dispatchableVehicles_LPP.LCPDVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCPDHeliVehicles", dispatchableVehicles_LPP.LCPDHeliVehicles));


        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("ASPHeliVehicles", dispatchableVehicles_LPP.ASPHeliVehicles));

        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("ASPVehicles", dispatchableVehicles_LPP.ASPVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("USNPSParkRangersVehicles", ParkRangerVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("BorderPatrolVehicles", NOOSEVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("NOOSEPIAVehicles", NOOSEVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("NOOSESEPVehicles", NOOSEVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("DOAVehicles", DOAVehicles));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("LCTaxiVehicles", dispatchableVehicles_LPP.LCTaxiVehicles_FEJ_LC));

        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FDNYFireVehicles", dispatchableVehicles_LPP.FDLCVehicles_FEJ_LC));
        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("FDNYEMTVehicles", dispatchableVehicles_LPP.FDLCEMTVehicles_FEJ_LC));


        LibertyVehicleGroupLookup.Add(new DispatchableVehicleGroup("HMSVehicles", dispatchableVehicles_LPP.HMSVehicles_FEJ_LC));


        Serialization.SerializeParams(LibertyVehicleGroupLookup, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\DispatchableVehicles_{StaticStrings.LPPConfigSuffix}.xml");
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
            new DispatchableVehicleGroup("DOAVehicles", DOAVehicles),
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
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),
            new DispatchableVehicleGroup("RideshareVehicles", RideshareVehicles),
        };
        Serialization.SerializeParams(SimpleVehicleLoopupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\DispatchableVehicles_Simple.xml");
        //SunshineDream
    }
    private void DefaultConfig_SunshineDream()
    {

        List<DispatchableVehicle> ArmeniaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("washington", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            //new DispatchableVehicle("pmp600", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };

        //DispatchableVehicle vcpdpolice3 = DispatchableVehicles_FEJ.Create_PoliceInterceptor(100, 100, 0, false, PoliceVehicleType.MarkedFlatLightbar, 134, 0, 3, 1, 4, "", "");
        //vcpdpolice3.ModelName = "police3liv";
        List<DispatchableVehicle> VCPDVehicles = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle("police3", 100, 100),
            //new DispatchableVehicle("police", 48,35) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            //new DispatchableVehicle("police2", 48, 35),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),
            //new DispatchableVehicle("policet", 0, 15) { MinOccupants = 3, MaxOccupants = 4, MinWantedLevelSpawn = 3,CaninePossibleSeats = new List<int>{ 1,2 } } 
        };
        //DispatchableVehicle vdpdpolice3 = DispatchableVehicles_FEJ.Create_PoliceInterceptor(100, 100, 1, false, PoliceVehicleType.MarkedFlatLightbar, 134, 0, 3, 1, 4, "", "");
        //vdpdpolice3.ModelName = "police3liv";
        List<DispatchableVehicle> VDPDVehicles = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle("police3", 100, 100),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives", GroupName = "Unmarked" },
            new DispatchableVehicle("fbi2", 1,1),

        };

        List<DispatchableVehicle> VCPDHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 1,150) { MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 },
        };

        List<DispatchableVehicleGroup> SunshineDreamVehicleLoopupGroup = new List<DispatchableVehicleGroup>
        {
            //Police
            new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles),
            new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles),
            new DispatchableVehicleGroup("FIBVehicles", FIBVehicles),
            new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles),
            new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles),
            new DispatchableVehicleGroup("VCPDVehicles", VCPDVehicles),
            new DispatchableVehicleGroup("VDPDVehicles", VDPDVehicles),


            new DispatchableVehicleGroup("VCPDHeliVehicles", VCPDHeliVehicles),
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
            new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles),
            new DispatchableVehicleGroup("DOAVehicles",DOAVehicles),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

            //Gang
            new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles_Old),

            //Other
            new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),
            new DispatchableVehicleGroup("RideshareVehicles", RideshareVehicles),
        };
        Serialization.SerializeParams(SunshineDreamVehicleLoopupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\DispatchableVehicles_SunshineDream.xml");
    }
    private void DefaultConfig_LosSantos_2008()
    {
        List<DispatchableVehicle> KoreanVehicles_old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fq2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("prairie", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("oracle", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        List<DispatchableVehicle> MafiaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("washington", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            //new DispatchableVehicle("pmp600", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> ArmeniaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rocoto", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("merit", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> CartelVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            //new DispatchableVehicle("fxt", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };

        List<DispatchableVehicle> TriadVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 20, 20) { RequiredPrimaryColorID = 111, RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("washington", 20, 20) { RequiredPrimaryColorID = 111, RequiredSecondaryColorID = 111 },//white
        };
        //Base
        List<string> toRemove = new List<string>() { "baller7", "aleutian","bison" };

        List<DispatchableVehicle> FamiliesVehicles2008 = ExtensionsMethods.Extensions.DeepCopy(FamiliesVehicles);
        FamiliesVehicles2008.RemoveAll(x => toRemove.Contains(x.ModelName.ToLower()));

        List<DispatchableVehicle> BallasVehicles2008 = ExtensionsMethods.Extensions.DeepCopy(BallasVehicles);
        BallasVehicles2008.RemoveAll(x => toRemove.Contains(x.ModelName.ToLower()));

        //Cop
        List<DispatchableVehicleGroup> OldVehicleLookupGroup = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.UnmarkedVehicles2008_FEJ),
            new DispatchableVehicleGroup("CoastGuardVehicles", DispatchableVehicles_FEJ.CoastGuardVehicles_FEJ),
            new DispatchableVehicleGroup("ParkRangerVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.ParkRangerVehicles2008_FEJ),
            new DispatchableVehicleGroup("FIBVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.FIBVehicles2008_FEJ),
            new DispatchableVehicleGroup("NOOSEVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.NOOSESEPVehicles2008_FEJ),
            new DispatchableVehicleGroup("PrisonVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.PrisonVehicles2008_FEJ),
            new DispatchableVehicleGroup("LSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("SAHPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.SAHP2008_FEJ),
            new DispatchableVehicleGroup("LSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSSD2008_FEJ),
            new DispatchableVehicleGroup("BCSOVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSSD2008_FEJ),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSSD2008_FEJ),
            new DispatchableVehicleGroup("DavisLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSSD2008_FEJ),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSSD2008_FEJ),
            new DispatchableVehicleGroup("LSPPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("LSIAPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("RHPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("DPPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("VWPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("EastLSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSPD2008_FEJ),
            new DispatchableVehicleGroup("PoliceHeliVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.PoliceHeliVehicles2008_FEJ),
            new DispatchableVehicleGroup("SheriffHeliVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.SheriffHeliVehicles2008_FEJ),
            new DispatchableVehicleGroup("ArmyVehicles", DispatchableVehicles_FEJ.ArmyVehicles_FEJ),
            new DispatchableVehicleGroup("USMCVehicles", DispatchableVehicles_FEJ.USMCVehicles_FEJ),
            new DispatchableVehicleGroup("USAFVehicles", DispatchableVehicles_FEJ.USAFVehicles_FEJ),

            //EMT
            new DispatchableVehicleGroup("LSFDEMTVehicles", DispatchableVehicles_FEJ.LSFDEMTVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDEMSVehicles", DispatchableVehicles_FEJ.LSCOFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDEMSVehicles", DispatchableVehicles_FEJ.BCFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("SAMSVehicles", DispatchableVehicles_FEJ.SAMSVehicles_FEJ),


            new DispatchableVehicleGroup("LSMCVehicles", DispatchableVehicles_FEJ.LSMCVehicles_FEJ),
            new DispatchableVehicleGroup("MRHVehicles", DispatchableVehicles_FEJ.MRHVehicles_FEJ),


            //Fire
            new DispatchableVehicleGroup("LSFDVehicles", DispatchableVehicles_FEJ.LSFDVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDVehicles", DispatchableVehicles_FEJ.LSCOFDVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDVehicles", DispatchableVehicles_FEJ.BCFDVehicles_FEJ),
            new DispatchableVehicleGroup("SanFireVehicles", DispatchableVehicles_FEJ.SanFireVehicles_FEJ),


            new DispatchableVehicleGroup("NYSPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.NYSP2008_FEJ),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.MerryweatherSecurity2008_FEJ),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.BobcatSecurity2008_FEJ),
            new DispatchableVehicleGroup("GroupSechsVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.GroupSechsSecurity2008_FEJ),
            new DispatchableVehicleGroup("SecuroservVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.SecuroservSecurity2008_FEJ),
            new DispatchableVehicleGroup("LCPDVehicles", LCPDVehicles),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.UnmarkedVehicles2008_FEJ),
            new DispatchableVehicleGroup("DOAVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.UnmarkedVehicles2008_FEJ),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),
            new DispatchableVehicleGroup("LSLifeguardVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.LSLifeguardVehicles2008_FEJ),

            //Gang
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles2008),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles_old),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles_Old),
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
            new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles2008),

            //Other
            new DispatchableVehicleGroup("TaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.DowntownTaxi2008_FEJ),
            new DispatchableVehicleGroup("DowntownTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.DowntownTaxi2008_FEJ),
            new DispatchableVehicleGroup("HellTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.HellTaxiVehicles2008_FEJ),
            new DispatchableVehicleGroup("PurpleTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.PurpleTaxiVehicles2008_FEJ),
            new DispatchableVehicleGroup("ShitiTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.ShitiTaxiVehicles2008_FEJ),
            new DispatchableVehicleGroup("SunderedTaxiVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2008.SunderedTaxiVehicles2008_FEJ),
        };


        Serialization.SerializeParams(OldVehicleLookupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\DispatchableVehicles_LosSantos2008.xml");

    }
    private void DefaultConfig_FullExpandedJurisdiction_2015()
    {
        //disabled
        //List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>
        //{
        //    new DispatchableVehicleGroup("UnmarkedVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.UnmarkedVehicles_FEJ),
        //    new DispatchableVehicleGroup("CoastGuardVehicles", DispatchableVehicles_FEJ.CoastGuardVehicles_FEJ),

        //    new DispatchableVehicleGroup("ParkRangerVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.ParkRangerVehicles_FEJ),//san andreas state parks
        //    new DispatchableVehicleGroup("SADFWParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.SADFWParkRangersVehicles_FEJ),
        //    new DispatchableVehicleGroup("USNPSParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.USNPSParkRangersVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSDPRParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSDPRParkRangersVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSLifeguardVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSLifeguardVehicles_FEJ),

        //    new DispatchableVehicleGroup("FIBVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.FIBVehicles_FEJ),
        //    new DispatchableVehicleGroup("NOOSEVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.NOOSEVehicles_FEJ),
        //    new DispatchableVehicleGroup("PrisonVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.PrisonVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("SAHPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.SAHPVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSSDVehicles_FEJ),
        //    new DispatchableVehicleGroup("BCSOVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.BCSOVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSIAPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSIAPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSPPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LSPPVehicles_FEJ),
        //    new DispatchableVehicleGroup("VWHillsLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.VWHillsLSSDVehicles_FEJ),
        //    new DispatchableVehicleGroup("DavisLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.DavisLSSDVehicles_FEJ),
        //    new DispatchableVehicleGroup("MajesticLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.MajesticLSSDVehicles_FEJ),
        //    new DispatchableVehicleGroup("RHPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.RHPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("DPPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.DPPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("VWPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.VWPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("EastLSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.EastLSPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("PoliceHeliVehicles", DispatchableVehicles_FEJ.PoliceHeliVehicles_FEJ),
        //    new DispatchableVehicleGroup("SheriffHeliVehicles", DispatchableVehicles_FEJ.SheriffHeliVehicles_FEJ),
        //    new DispatchableVehicleGroup("ArmyVehicles", DispatchableVehicles_FEJ.ArmyVehicles_FEJ),
        //    new DispatchableVehicleGroup("USMCVehicles", DispatchableVehicles_FEJ.USMCVehicles_FEJ),
        //    new DispatchableVehicleGroup("USAFVehicles", DispatchableVehicles_FEJ.USAFVehicles_FEJ),
        //    new DispatchableVehicleGroup("Firetrucks", Firetrucks),
        //    new DispatchableVehicleGroup("Amublance1", Amublance1),
        //    new DispatchableVehicleGroup("Amublance2", Amublance2),
        //    new DispatchableVehicleGroup("Amublance3", Amublance3),
        //    new DispatchableVehicleGroup("NYSPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.NYSPVehicles_FEJ),
        //    new DispatchableVehicleGroup("LCPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LCPDVehicles_FEJ),
        //    new DispatchableVehicleGroup("BorderPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.BorderPatrolVehicles_FEJ),
        //    new DispatchableVehicleGroup("NOOSEPIAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.NOOSEPIAVehicles_FEJ),
        //    new DispatchableVehicleGroup("NOOSESEPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.NOOSESEPVehicles_FEJ),
        //    new DispatchableVehicleGroup("MarshalsServiceVehicles", DispatchableVehicles_FEJ.MarshalsServiceVehicles_FEJ),
        //    new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

        //    //EMT
        //    new DispatchableVehicleGroup("LSFDEMTVehicles", DispatchableVehicles_FEJ.LSFDEMTVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSCOFDEMSVehicles", DispatchableVehicles_FEJ.LSCOFDEMSVehicles_FEJ),
        //    new DispatchableVehicleGroup("BCFDEMSVehicles", DispatchableVehicles_FEJ.BCFDEMSVehicles_FEJ),
        //    new DispatchableVehicleGroup("SAMSVehicles", DispatchableVehicles_FEJ.SAMSVehicles_FEJ),


        //    new DispatchableVehicleGroup("LSMCVehicles", DispatchableVehicles_FEJ.LSMCVehicles_FEJ),
        //    new DispatchableVehicleGroup("MRHVehicles", DispatchableVehicles_FEJ.MRHVehicles_FEJ),


        //    //Fire
        //    new DispatchableVehicleGroup("LSFDVehicles", DispatchableVehicles_FEJ.LSFDVehicles_FEJ),
        //    new DispatchableVehicleGroup("LSCOFDVehicles", DispatchableVehicles_FEJ.LSCOFDVehicles_FEJ),
        //    new DispatchableVehicleGroup("BCFDVehicles", DispatchableVehicles_FEJ.BCFDVehicles_FEJ),
        //    new DispatchableVehicleGroup("SanFireVehicles", DispatchableVehicles_FEJ.SanFireVehicles_FEJ),


        //    //Security

        //    new DispatchableVehicleGroup("MerryweatherPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.MerryweatherPatrolVehicles_FEJ),
        //    new DispatchableVehicleGroup("BobcatSecurityVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.BobcatSecurityVehicles_FEJ),
        //    new DispatchableVehicleGroup("GroupSechsVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.GroupSechsVehicles_FEJ),
        //    new DispatchableVehicleGroup("SecuroservVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.SecuroservVehicles_FEJ),
        //    new DispatchableVehicleGroup("LNLVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.LNLVehicles_FEJ),
        //    new DispatchableVehicleGroup("ChuffVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.CHUFFVehicles_FEJ),

        //    //Gang stuff
        //    new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
        //    new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
        //    new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
        //    new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
        //    new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
        //    new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
        //    new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
        //    new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
        //    new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),
        //    new DispatchableVehicleGroup("GambettiVehicles", GambettiVehicles),
        //    new DispatchableVehicleGroup("PavanoVehicles", PavanoVehicles),
        //    new DispatchableVehicleGroup("LupisellaVehicles", LupisellaVehicles),
        //    new DispatchableVehicleGroup("MessinaVehicles", MessinaVehicles),
        //    new DispatchableVehicleGroup("AncelottiVehicles", AncelottiVehicles),
        //    new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles),
        //    new DispatchableVehicleGroup("CartelVehicles", CartelVehicles),
        //    new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles),
        //    new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles),

        //    //Other
        //    new DispatchableVehicleGroup("TaxiVehicles", TaxiVehicles),
        //    new DispatchableVehicleGroup("DowntownTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.DowntownTaxiVehicles),
        //    new DispatchableVehicleGroup("HellTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.HellTaxiVehicles),
        //    new DispatchableVehicleGroup("PurpleTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.PurpleTaxiVehicles),
        //    new DispatchableVehicleGroup("ShitiTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.ShitiTaxiVehicles),
        //    new DispatchableVehicleGroup("SunderedTaxiVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_2015.SunderedTaxiVehicles),
        //};
        //Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\DispatchableVehicles_FullExpandedJurisdiction2015.xml");
    }
    private void DefaultConfig_FullExpandedJurisdiction_Modern()
    {

        List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.UnmarkedVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("CoastGuardVehicles", DispatchableVehicles_FEJ.CoastGuardVehicles_FEJ),

            new DispatchableVehicleGroup("ParkRangerVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.ParkRangerVehicles_FEJ_Modern),//san andreas state parks
            new DispatchableVehicleGroup("SADFWParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.SADFWParkRangersVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("USNPSParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.USNPSParkRangersVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSDPRParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSDPRParkRangersVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSLifeguardVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSLifeguardVehicles_FEJ_Modern),

            new DispatchableVehicleGroup("FIBVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.FIBVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("NOOSEVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.NOOSESEPVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("PrisonVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.PrisonVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("SAHPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.SAHPVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSSDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("BCSOVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.BCSOVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSIAPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSIAPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LSPPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LSPPVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.VWHillsLSSDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("DavisLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.DavisLSSDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.MajesticLSSDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("RHPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.RHPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("DPPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.DPPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("VWPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.VWPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("EastLSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.EastLSPDVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("PoliceHeliVehicles", DispatchableVehicles_FEJ.PoliceHeliVehicles_FEJ),
            new DispatchableVehicleGroup("SheriffHeliVehicles", DispatchableVehicles_FEJ.SheriffHeliVehicles_FEJ),
            new DispatchableVehicleGroup("ArmyVehicles", DispatchableVehicles_FEJ.ArmyVehicles_FEJ),
            new DispatchableVehicleGroup("USMCVehicles", DispatchableVehicles_FEJ.USMCVehicles_FEJ),
            new DispatchableVehicleGroup("USAFVehicles", DispatchableVehicles_FEJ.USAFVehicles_FEJ),
            new DispatchableVehicleGroup("Firetrucks",  DispatchableVehicles_FEJ.LSFDVehicles_FEJ),
            new DispatchableVehicleGroup("Amublance1", DispatchableVehicles_FEJ.LSMCVehicles_FEJ),
            new DispatchableVehicleGroup("Amublance2", DispatchableVehicles_FEJ.MRHVehicles_FEJ),
            new DispatchableVehicleGroup("Amublance3", DispatchableVehicles_FEJ.LSFDEMTVehicles_FEJ),
            new DispatchableVehicleGroup("NYSPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.NYSPVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("BorderPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.BorderPatrolVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("NOOSEPIAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.NOOSEPIAVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("NOOSESEPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.NOOSESEPVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.MarshalsServiceVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("DOAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.DOAVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

            //EMT
            new DispatchableVehicleGroup("LSFDEMTVehicles", DispatchableVehicles_FEJ.LSFDEMTVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDEMSVehicles", DispatchableVehicles_FEJ.LSCOFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDEMSVehicles", DispatchableVehicles_FEJ.BCFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("SAMSVehicles", DispatchableVehicles_FEJ.SAMSVehicles_FEJ),


            new DispatchableVehicleGroup("LSMCVehicles", DispatchableVehicles_FEJ.LSMCVehicles_FEJ),
            new DispatchableVehicleGroup("MRHVehicles", DispatchableVehicles_FEJ.MRHVehicles_FEJ),


            //Fire
            new DispatchableVehicleGroup("LSFDVehicles", DispatchableVehicles_FEJ.LSFDVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDVehicles", DispatchableVehicles_FEJ.LSCOFDVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDVehicles", DispatchableVehicles_FEJ.BCFDVehicles_FEJ),
            new DispatchableVehicleGroup("SanFireVehicles", DispatchableVehicles_FEJ.SanFireVehicles_FEJ),


            //Security

            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.MerryweatherPatrolVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.BobcatSecurityVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("GroupSechsVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.GroupSechsVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("SecuroservVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.SecuroservVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("LNLVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.LNLVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("ChuffVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.CHUFFVehicles_FEJ_Modern),

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
            new DispatchableVehicleGroup("RideshareVehicles", RideshareVehicles),
            new DispatchableVehicleGroup("DowntownTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.DowntownTaxiVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("HellTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.HellTaxiVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("PurpleTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.PurpleTaxiVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("ShitiTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.ShitiTaxiVehicles_FEJ_Modern),
            new DispatchableVehicleGroup("SunderedTaxiVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Modern.SunderedTaxiVehicles_FEJ_Modern),
        };

        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\DispatchableVehicles_FullExpandedJurisdiction.xml");
        //Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Vanilla Peds\\DispatchableVehicles_FullExpandedJurisdiction.xml");

        //List<string> ModelsToRemove = new List<string>()
        //{
        //    DispatchableVehicles_FEJ.PoliceRiata,
        //    DispatchableVehicles_FEJ.PoliceReblaGTS,
        //    DispatchableVehicles_FEJ.PoliceOracle,
        //    DispatchableVehicles_FEJ.PoliceLandstalkerXL,
        //};

        //foreach(DispatchableVehicleGroup dvg in VehicleGroupLookupFEJ)
        //{
        //    dvg.DispatchableVehicles.RemoveAll(x => ModelsToRemove.Contains(x.ModelName));
        //}
        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Vanilla Peds\\DispatchableVehicles_FullExpandedJurisdiction.xml");
    }
    private void DefaultConfig_FullExpandedJurisdiction_Stanier()
    {

        List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.UnmarkedVehicles_FEJ),
            new DispatchableVehicleGroup("CoastGuardVehicles", DispatchableVehicles_FEJ.CoastGuardVehicles_FEJ),

            new DispatchableVehicleGroup("ParkRangerVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.ParkRangerVehicles_FEJ),//san andreas state parks
            new DispatchableVehicleGroup("SADFWParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.SADFWParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("USNPSParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.USNPSParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("LSDPRParkRangersVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSDPRParkRangersVehicles_FEJ),
            new DispatchableVehicleGroup("LSLifeguardVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSLifeguardVehicles_FEJ),

            new DispatchableVehicleGroup("FIBVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.FIBVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.NOOSESEPVehicles_FEJ),
            new DispatchableVehicleGroup("PrisonVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.PrisonVehicles_FEJ),
            new DispatchableVehicleGroup("LSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSPDVehicles_FEJ),
            new DispatchableVehicleGroup("SAHPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.SAHPVehicles_FEJ),
            new DispatchableVehicleGroup("LSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSSDVehicles_FEJ),
            new DispatchableVehicleGroup("BCSOVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.BCSOVehicles_FEJ),
            new DispatchableVehicleGroup("LSIAPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSIAPDVehicles_FEJ),
            new DispatchableVehicleGroup("LSPPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LSPPVehicles_FEJ),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.VWHillsLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("DavisLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.DavisLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.MajesticLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("RHPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.RHPDVehicles_FEJ),
            new DispatchableVehicleGroup("DPPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.DPPDVehicles_FEJ),
            new DispatchableVehicleGroup("VWPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.VWPDVehicles_FEJ),
            new DispatchableVehicleGroup("EastLSPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.EastLSPDVehicles_FEJ),
            new DispatchableVehicleGroup("PoliceHeliVehicles", DispatchableVehicles_FEJ.PoliceHeliVehicles_FEJ),
            new DispatchableVehicleGroup("SheriffHeliVehicles", DispatchableVehicles_FEJ.SheriffHeliVehicles_FEJ),
            new DispatchableVehicleGroup("ArmyVehicles", DispatchableVehicles_FEJ.ArmyVehicles_FEJ),
            new DispatchableVehicleGroup("USMCVehicles", DispatchableVehicles_FEJ.USMCVehicles_FEJ),
            new DispatchableVehicleGroup("USAFVehicles", DispatchableVehicles_FEJ.USAFVehicles_FEJ),
            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.NYSPVehicles_FEJ),
            new DispatchableVehicleGroup("LCPDVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LCPDVehicles_FEJ),
            new DispatchableVehicleGroup("BorderPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.BorderPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEPIAVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.NOOSEPIAVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSESEPVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.NOOSESEPVehicles_FEJ),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.MarshalsServiceVehicles_FEJ),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

            //EMT
            new DispatchableVehicleGroup("LSFDEMTVehicles", DispatchableVehicles_FEJ.LSFDEMTVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDEMSVehicles", DispatchableVehicles_FEJ.LSCOFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDEMSVehicles", DispatchableVehicles_FEJ.BCFDEMSVehicles_FEJ),
            new DispatchableVehicleGroup("SAMSVehicles", DispatchableVehicles_FEJ.SAMSVehicles_FEJ),


            new DispatchableVehicleGroup("LSMCVehicles", DispatchableVehicles_FEJ.LSMCVehicles_FEJ),
            new DispatchableVehicleGroup("MRHVehicles", DispatchableVehicles_FEJ.MRHVehicles_FEJ),


            //Fire
            new DispatchableVehicleGroup("LSFDVehicles", DispatchableVehicles_FEJ.LSFDVehicles_FEJ),
            new DispatchableVehicleGroup("LSCOFDVehicles", DispatchableVehicles_FEJ.LSCOFDVehicles_FEJ),
            new DispatchableVehicleGroup("BCFDVehicles", DispatchableVehicles_FEJ.BCFDVehicles_FEJ),
            new DispatchableVehicleGroup("SanFireVehicles", DispatchableVehicles_FEJ.SanFireVehicles_FEJ),


            //Security

            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.MerryweatherPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.BobcatSecurityVehicles_FEJ),
            new DispatchableVehicleGroup("GroupSechsVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.GroupSechsVehicles_FEJ),
            new DispatchableVehicleGroup("SecuroservVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.SecuroservVehicles_FEJ),
            new DispatchableVehicleGroup("LNLVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.LNLVehicles_FEJ),
            new DispatchableVehicleGroup("ChuffVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.CHUFFVehicles_FEJ),

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
            new DispatchableVehicleGroup("RideshareVehicles", RideshareVehicles),
            new DispatchableVehicleGroup("DowntownTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.DowntownTaxiVehicles),
            new DispatchableVehicleGroup("HellTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.HellTaxiVehicles),
            new DispatchableVehicleGroup("PurpleTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.PurpleTaxiVehicles),
            new DispatchableVehicleGroup("ShitiTaxiVehicles", DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.ShitiTaxiVehicles),
            new DispatchableVehicleGroup("SunderedTaxiVehicles",DispatchableVehicles_FEJ.DispatchableVehicles_FEJ_Stanier.SunderedTaxiVehicles),
        };

        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\DispatchableVehicles_FullExpandedJurisdiction_AttackOfTheStaniers.xml");
    }




    private DispatchableVehicle Create_PoliceTerminusVanilla(int ambientSpawnChance, int wantedSpawnChance, string agencyID)
    {
        DispatchableVehicle policeTerminus = new DispatchableVehicle("polterminus", ambientSpawnChance, wantedSpawnChance)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),//weight bubble lights
                    new DispatchableVehicleExtra(2,false,100,2),//red and blue regular
                    new DispatchableVehicleExtra(3,false,100,3),//red and blue vector
                    new DispatchableVehicleExtra(4,true,100,4),//clear vector
                    new DispatchableVehicleExtra(5,true,100,5),//roof
                },
            VehicleMods = new List<DispatchableVehicleMod>()
                {
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



                            new DispatchableVehicleModValue(0,100),//LSPD
                            new DispatchableVehicleModValue(1,100),//LSPD Medical examiner
                            new DispatchableVehicleModValue(2,100),//LSPD CRIME SCENE
                            new DispatchableVehicleModValue(3,100),//LSPD K9
                            new DispatchableVehicleModValue(4,100),//LSPD Prisoner Transport
                            new DispatchableVehicleModValue(5,100),//LSPD
                            new DispatchableVehicleModValue(20,100),//LSPD

                            //new DispatchableVehicleModValue(10,100),//SAHP
                            //new DispatchableVehicleModValue(11,100),//LSSD
                            //new DispatchableVehicleModValue(13,100),//LSIAPD
                            //new DispatchableVehicleModValue(15,100),//DPPD
                            //new DispatchableVehicleModValue(17,100),//LSPP

                            //new DispatchableVehicleModValue(22,100),//SAHP
                            //new DispatchableVehicleModValue(23,100),//LSSD
                            //new DispatchableVehicleModValue(27,100),//PARK RANGER
                            //new DispatchableVehicleModValue(28,100),//PARK RANGER K9
                        },
                    },
                },
        };


        if(agencyID == "LSPD")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,100),//LSPD
                            new DispatchableVehicleModValue(1,100),//LSPD Medical examiner
                            new DispatchableVehicleModValue(2,100),//LSPD CRIME SCENE
                            new DispatchableVehicleModValue(3,100),//LSPD K9
                            new DispatchableVehicleModValue(4,100),//LSPD Prisoner Transport
                            new DispatchableVehicleModValue(5,100),//LSPD
                            new DispatchableVehicleModValue(20,100),//LSPD

                            //new DispatchableVehicleModValue(10,100),//SAHP
                            //new DispatchableVehicleModValue(11,100),//LSSD
                            //new DispatchableVehicleModValue(13,100),//LSIAPD
                            //new DispatchableVehicleModValue(15,100),//DPPD
                            //new DispatchableVehicleModValue(17,100),//LSPP

                            //new DispatchableVehicleModValue(22,100),//SAHP
                            //new DispatchableVehicleModValue(23,100),//LSSD
                            //new DispatchableVehicleModValue(27,100),//PARK RANGER
                            //new DispatchableVehicleModValue(28,100),//PARK RANGER K9
                        },
            });
        }
        else if (agencyID == "SAHP")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(10,100),//SAHP
                            new DispatchableVehicleModValue(22,100),//SAHP
                        },
            });
        }
        else if (agencyID == "LSSD")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(11,100),//LSSD
                            new DispatchableVehicleModValue(23,100),//LSSD
                        },
            });
        }
        else if (agencyID == "LSPP")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(17,100),
                        },
            });
        }
        else if (agencyID == "DPPD")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(15,100),
                        },
            });
        }
        else if (agencyID == "SAPR")
        {
            policeTerminus.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(27,100),
                        },
            });
        }
        return policeTerminus;
    }
    private DispatchableVehicle Create_PoliceCaracaraVanilla(int ambientSpawnChance, int wantedSpawnChance, string agencyID)
    {
        DispatchableVehicle policeCaracara = new DispatchableVehicle("polcaracara", ambientSpawnChance, wantedSpawnChance)
        {
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 111,
            MaxRandomDirtLevel = 10.0f,
            VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(1,false,100,1),//weight bubble lights
                    new DispatchableVehicleExtra(2,false,100,2),//red and blue regular
                    new DispatchableVehicleExtra(3,false,100,3),//red and blue vector
                    new DispatchableVehicleExtra(4,true,100,4),//clear vector
                },
            VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(1,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,100),
                        },
                    },
                    new DispatchableVehicleMod(42,50)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(2,15),
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
                },
        };

        if (agencyID == "LSPD")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,100),//LSPD
                            new DispatchableVehicleModValue(1,100),//LSPD Medical examiner
                            new DispatchableVehicleModValue(2,100),//LSPD CRIME SCENE
                            new DispatchableVehicleModValue(3,100),//LSPD K9
                            new DispatchableVehicleModValue(4,100),//LSPD Prisoner Transport
                            new DispatchableVehicleModValue(5,100),//LSPD
                            new DispatchableVehicleModValue(20,100),//LSPD

                            //new DispatchableVehicleModValue(10,100),//SAHP
                            //new DispatchableVehicleModValue(11,100),//LSSD
                            //new DispatchableVehicleModValue(13,100),//LSIAPD
                            //new DispatchableVehicleModValue(15,100),//DPPD
                            //new DispatchableVehicleModValue(17,100),//LSPP

                            //new DispatchableVehicleModValue(22,100),//SAHP
                            //new DispatchableVehicleModValue(23,100),//LSSD
                            //new DispatchableVehicleModValue(27,100),//PARK RANGER
                            //new DispatchableVehicleModValue(28,100),//PARK RANGER K9
                        },
            });
        }
        else if (agencyID == "SAHP")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(10,100),//SAHP
                            new DispatchableVehicleModValue(22,100),//SAHP
                        },
            });
        }
        else if (agencyID == "LSSD")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(11,100),//LSSD
                            new DispatchableVehicleModValue(23,100),//LSSD
                        },
            });
        }
        else if (agencyID == "LSPP")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(17,100),
                        },
            });
        }
        else if (agencyID == "DPPD")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(15,100),
                        },
            });
        }
        else if (agencyID == "SAPR")
        {
            policeCaracara.VehicleMods.Add(new DispatchableVehicleMod(48, 100)
            {
                DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(27,100),
                        },
            });
        }
        return policeCaracara;
    }
}


/*
 *             new ModKitDescription("Spoilers",0),
            new ModKitDescription("Front Bumper",1),
            new ModKitDescription("Rear Bumper",2),
            new ModKitDescription("Side Skirt",3),
            new ModKitDescription("Exhaust",4),
            new ModKitDescription("Frame",5),
            new ModKitDescription("Grille",6),
            new ModKitDescription("Hood",7),
            new ModKitDescription("Fender",8),
            new ModKitDescription("Right Fender",9),
            new ModKitDescription("Roof",10),
            new ModKitDescription("Engine",11),
            new ModKitDescription("Brakes",12),
            new ModKitDescription("Transmission",13),
            new ModKitDescription("Horns",1),
            new ModKitDescription("Suspension",15),
            new ModKitDescription("Armor",16),
            new ModKitDescription("Turbo",18),
            new ModKitDescription("Xenon",22),
            new ModKitDescription("Front Wheels",23),
            new ModKitDescription("Back Wheels (Motorcycle)",24),
            new ModKitDescription("Plate holders", 25),
            new ModKitDescription("Trim Design", 27),
            new ModKitDescription("Ornaments", 28),
            new ModKitDescription("Dial Design", 30),
            new ModKitDescription("Steering Wheel", 33),
            new ModKitDescription("Shift Lever", 34),
            new ModKitDescription("Plaques", 35),
            new ModKitDescription("Hydraulics", 38),
            new ModKitDescription("Boost", 40),
            new ModKitDescription("Window Tint", 55),
            new ModKitDescription("Livery", 48),
            new ModKitDescription("Plate", 53),*/