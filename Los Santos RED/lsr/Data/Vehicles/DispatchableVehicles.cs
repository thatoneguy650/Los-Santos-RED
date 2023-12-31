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
    private List<DispatchableVehicle> GenericGangVehicles;
    private List<DispatchableVehicle> AllGangVehicles;
    private List<DispatchableVehicle> LostMCVehicles;
    private List<DispatchableVehicle> VarriosVehicles;
    private List<DispatchableVehicle> BallasVehicles;
    private List<DispatchableVehicle> VagosVehicles;
    private List<DispatchableVehicle> MarabuntaVehicles;
    private List<DispatchableVehicle> KoreanVehicles;
    private List<DispatchableVehicle> TriadVehicles;
    private List<DispatchableVehicle> YardieVehicles;
    private List<DispatchableVehicle> DiablosVehicles;
    private List<DispatchableVehicle> MafiaVehicles;

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
    private List<DispatchableVehicle> MarshalsServiceVehicles;
    private List<DispatchableVehicle> OffDutyCopVehicles;
    private List<DispatchableVehicle> LCPDVehicles;
    private DispatchableVehicle TaxiBroadWay;
    private DispatchableVehicle TaxiEudora;
    private List<DispatchableVehicle> TaxiVehicles;
    private DispatchableVehicle GauntletUndercoverSAHP;
    private DispatchableVehicle GauntletSAHP;
    private DispatchableVehicle GauntletSAHPSlicktop;
    private DispatchableVehicle SheriffStanierNew;
    private DispatchableVehicle ParkRangerStanierNew;
    private DispatchableVehicle SAHPStanierNew;
    private DispatchableVehicle SAHPStanierSlicktopNew;
    private DispatchableVehicle AleutianSecurityBobCat;
    private DispatchableVehicle AleutianSecurityG6;
    private DispatchableVehicle AleutianSecurityMW;
    private DispatchableVehicle AleutianSecuritySECURO;
    private DispatchableVehicle AsteropeSecurityBobCat;
    private DispatchableVehicle AsteropeSecurityG6;
    private DispatchableVehicle AsteropeSecurityMW;
    private DispatchableVehicle AsteropeSecuritySECURO;

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
        CoastGuardVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) { MaxRandomDirtLevel = 15.0f },
            ParkRangerStanierNew, };
        FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 4, MaxOccupants = 4 }, };
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
        ArmyVehicles = new List<DispatchableVehicle>() {
            //new DispatchableVehicle("crusader", 85,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            //new DispatchableVehicle("barracks", 15,75) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,10) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

            new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("insurgent3", 0,25) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },


            new DispatchableVehicle("rhino", 0, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("valkyrie2", 0,75) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1,0,1,2 },MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 }




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
        GenericGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("tornado", 15, 15),};
        AllGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("buccaneer2", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("chino", 15, 15),
            new DispatchableVehicle("chino2", 15, 15),
            new DispatchableVehicle("faction", 15, 15),
            new DispatchableVehicle("faction2", 15, 15),
            new DispatchableVehicle("primo", 15, 15),
            new DispatchableVehicle("primo2", 15, 15),
            new DispatchableVehicle("voodoo", 15, 15),
            new DispatchableVehicle("voodoo2", 15, 15),
        };
        LostMCVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },};
        VarriosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 20, 20){ RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68},
            //new DispatchableVehicle("buccaneer2", 50, 50){RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },//light?blue
        };
        BallasVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purple
        };
        VagosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("chino", 20, 20){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },
            new DispatchableVehicle("chino2", 20, 20){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },//yellow
        };
        MarabuntaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("faction", 20, 20){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },
            new DispatchableVehicle("faction2", 20, 20){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },//blue
        };
        KoreanVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("feltzer2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("comet2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("dubsta2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        TriadVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 20, 20){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("washington", 20, 20){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
           // new DispatchableVehicle("cavalcade", 33, 33){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
        };
        YardieVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("virgo", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo2", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
        };
        DiablosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("stalion", 25, 25){ RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28, },
        };
        MafiaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("huntley", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        GambettiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        PavanoVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        LupisellaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("huntley", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        MessinaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        AncelottiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("huntley", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };




        ArmeniaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("schafter2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        CartelVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

        };
        RedneckVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sandking2",10,10),
            new DispatchableVehicle("rebel", 20, 20),
            new DispatchableVehicle("bison", 20, 20),
            new DispatchableVehicle("sanchez2",20,20) {MaxOccupants = 1 },
        };
        FamiliesVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("emperor",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
           // new DispatchableVehicle("peyote",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("nemesis",15,15) {MaxOccupants = 1 },
            new DispatchableVehicle("buccaneer",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
           //new DispatchableVehicle("manana",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("tornado",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green       

        };


        DispatchableVehicles_Gangs customGangs = new DispatchableVehicles_Gangs();
        customGangs.DefaultConfig();
        FamiliesVehicles.AddRange(customGangs.FamiliesVehicles);
        VarriosVehicles.AddRange(customGangs.VarriosVehicles);
        BallasVehicles.AddRange(customGangs.BallasVehicles);
        VagosVehicles.AddRange(customGangs.VagosVehicles);
        MarabuntaVehicles.AddRange(customGangs.MarabuntaVehicles);
        KoreanVehicles.AddRange(customGangs.KoreanVehicles);
        TriadVehicles.AddRange(customGangs.TriadVehicles);
        DiablosVehicles.AddRange(customGangs.DiablosVehicles);
        GambettiVehicles.AddRange(customGangs.GambettiVehicles);
        PavanoVehicles.AddRange(customGangs.PavanoVehicles);
        LupisellaVehicles.AddRange(customGangs.LupisellaVehicles);
        MessinaVehicles.AddRange(customGangs.MessinaVehicles);
        AncelottiVehicles.AddRange(customGangs.AncelottiVehicles);
        CartelVehicles.AddRange(customGangs.CartelVehicles);
        RedneckVehicles.AddRange(customGangs.RedneckVehicles);





       

        //SetupDefaultGangSpecialVehicles();

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


            new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles),
            new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles),
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),
            new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles),

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

            //Gang
            new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles),
            new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles),
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles_Old),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles_old),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),
            new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles_Old),

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

            //Gang
            new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles),
            new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles),
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
        //Cops
        List<DispatchableVehicle> UnmarkedVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(StanierUnmarked, 50, 50),
            new DispatchableVehicle(PoliceFugitive, 50,50){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(BuffaloUnmarked, 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(GrangerUnmarked, 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
        };

        //Federal
        List<DispatchableVehicle> CoastGuardVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> ParkRangerVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceGresley, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f, RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceGranger, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceBison, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 } },
        };
        List<DispatchableVehicle> FIBVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(BuffaloUnmarked, 30, 30){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(GrangerUnmarked, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(PoliceFugitive, 30,30){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(BuffaloUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },

            new DispatchableVehicle(PoliceBuffaloS, 0, 70) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) }, },


            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } }, };  
        List<DispatchableVehicle> NOOSEVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 1,1){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 70, 70){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGresley, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },

            new DispatchableVehicle(PoliceBuffalo, 35, 35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, CaninePossibleSeats = new List<int>() { 1,2 } },
            new DispatchableVehicle(PoliceBuffalo, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            new DispatchableVehicle(PoliceTorrence, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};   
        //Police
        List<DispatchableVehicle> LSPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceStanier, 25,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 48,35) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 15, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2,true,80) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceFugitive, 10,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            new DispatchableVehicle(PoliceGauntlet, 5,5)
            {
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
                            new DispatchableVehicleModValue(0,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 }, },
            new DispatchableVehicle(PoliceTransporter, 0, 15) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3},
            new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } }, };
        List<DispatchableVehicle> EastLSPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 35,35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        List<DispatchableVehicle> VWPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 30,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 }, },
            new DispatchableVehicle(PoliceFugitive, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        List<DispatchableVehicle> RHPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle(PoliceGresley, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle(PoliceBison, 5, 5){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle(PoliceGauntlet, 5,5)
            {
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
                            new DispatchableVehicleModValue(7,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceGranger, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle(PoliceFugitive, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> DPPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceGranger, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceBison, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceGauntlet, 3,3)
            {
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
                            new DispatchableVehicleModValue(8,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceFugitive, 15,10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        //Sheriff
        List<DispatchableVehicle> BCSOVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle(PoliceGresley, 20, 20) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle(PoliceGranger, 30, 30) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle(PoliceBison, 15, 15)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 0 }, },
            new DispatchableVehicle(PoliceGauntlet, 1,1)
            {
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
                            new DispatchableVehicleModValue(15,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceFugitive, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 10, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 2 } },};
        List<DispatchableVehicle> LSSDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 }, },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() {7 } },
            new DispatchableVehicle(PoliceFugitive, 2, 2){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceGauntlet, 2,2)
            {
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
                            new DispatchableVehicleModValue(0,33),
                            new DispatchableVehicleModValue(1,33),
                            new DispatchableVehicleModValue(2,33),
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
                            new DispatchableVehicleModValue(4,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};     
        List<DispatchableVehicle> MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGranger, 50, 50) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 }, },
            new DispatchableVehicle(PoliceFugitive, 1, 1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 25, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGresley, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGranger, 20, 20)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceBison, 7, 7)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 }, },
            new DispatchableVehicle(PoliceFugitive, 2, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        List<DispatchableVehicle> DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 55, 55){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGranger, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        //Other State
        List<DispatchableVehicle> LSIAPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceGranger, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceGauntlet, 2,2)
            {
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
                            new DispatchableVehicleModValue(6,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceFugitive, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> LSPPVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle(PoliceGauntlet, 2,2) 
            {          
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
                            new DispatchableVehicleModValue(10,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceGranger, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle(PoliceFugitive, 2, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 10, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 4 } },};
        //State
        List<DispatchableVehicle> SAHPVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(PoliceBuffaloS, 45, 55) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },
            
            //buffalos unmarked too
            new DispatchableVehicle(PoliceBuffaloS, 10, 0) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },

            new DispatchableVehicle(PoliceTorrence, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGresley, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGranger, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceBison, 5, 1) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            GauntletUndercoverSAHP,
            new DispatchableVehicle(PoliceGauntlet, 15,15)
            {
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
            },
            new DispatchableVehicle(PoliceFugitive, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> PrisonVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceTransporter, 0, 25),
            new DispatchableVehicle(PoliceStanier, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceBison, 5, 0) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceFugitive, 1, 0){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> NYSPVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,20){  RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceBison, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },  };
        List<DispatchableVehicle> LCPDVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            new DispatchableVehicle(GrangerUnmarked, 1,1),
            new DispatchableVehicle(PoliceBike, 15, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 5 } },
        };
        List<DispatchableVehicle> BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            new DispatchableVehicle(PoliceGresley, 40, 40){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            new DispatchableVehicle(PoliceGranger, 40, 40){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            new DispatchableVehicle(PoliceMerit, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f, RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceGauntlet, 1,1)
            {
                RequiredPrimaryColorID = 111,
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
                            new DispatchableVehicleModValue(16,100),
                        },
                    },
                },
            },
            new DispatchableVehicle(PoliceBison, 35, 35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
        };
        List<DispatchableVehicle> NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 35, 35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 35, 35) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGresley, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceMerit, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBison, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGauntlet, 1,1)
            {
                RequiredPrimaryColorID = 111,
                RequiredSecondaryColorID = 111,
                ForcedPlateType = 4,
                MaxWantedLevelSpawn = 3,
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
                            new DispatchableVehicleModValue(18,100),
                        },
                    },
                },
            },

            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            new DispatchableVehicle(PoliceBuffaloS, 0, 40) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};
        List<DispatchableVehicle> NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 15,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGresley, 30, 30){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBison, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGauntlet, 1,1)
            {
                RequiredPrimaryColorID = 111,
                RequiredSecondaryColorID = 111,
                ForcedPlateType = 4,
                MaxWantedLevelSpawn = 3,
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
                            new DispatchableVehicleModValue(17,100),
                        },
                    },
                },
            },

            new DispatchableVehicle(PoliceStanier, 0, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { CaninePossibleSeats = new List<int>{ 1,2 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};

        List<DispatchableVehicle> MarshalsServiceVehicles_FEJ = MarshalsServiceVehicles.Copy();//for now

        MarshalsServiceVehicles_FEJ.Add(new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredLiveries = new List<int>() { 16 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });

        //Security
        List<DispatchableVehicle> MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 35, 35) 
            { 
                RequiredLiveries = new List<int>() { 5 }, 
                VehicleExtras = new List<DispatchableVehicleExtra>() 
                { 
                    new DispatchableVehicleExtra(5,false,100), 
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                } 
            },
            new DispatchableVehicle(SecurityTorrence, 35, 35) {  RequiredLiveries = new List<int>() { 1 } },
            AleutianSecurityMW,
            AsteropeSecurityMW,
        };
        List<DispatchableVehicle> BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 8 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 3 } },
            AleutianSecurityBobCat,
            AsteropeSecurityBobCat,
        };
        List<DispatchableVehicle> GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 7 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 0 } },
            AleutianSecurityG6,
            AsteropeSecurityG6,
        };
        List<DispatchableVehicle> SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 6 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 4 } },
            AleutianSecuritySECURO,
            AsteropeSecuritySECURO,
        };

        List<DispatchableVehicle> DowntownTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 0 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            TaxiBroadWay,
            TaxiEudora,
        };
        List<DispatchableVehicle> PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        List<DispatchableVehicle> HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        List<DispatchableVehicle> ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        List<DispatchableVehicle> SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };

        List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>
        {
            new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles_FEJ),
            new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles_FEJ),
            new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles_FEJ),
            new DispatchableVehicleGroup("FIBVehicles", FIBVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles_FEJ),
            new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles_FEJ),
            new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles_FEJ),
            new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles_FEJ),
            new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles_FEJ),
            new DispatchableVehicleGroup("BCSOVehicles", BCSOVehicles_FEJ),
            new DispatchableVehicleGroup("LSIAPDVehicles", LSIAPDVehicles_FEJ),
            new DispatchableVehicleGroup("LSPPVehicles", LSPPVehicles_FEJ),
            new DispatchableVehicleGroup("VWHillsLSSDVehicles", VWHillsLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("DavisLSSDVehicles", DavisLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("MajesticLSSDVehicles", MajesticLSSDVehicles_FEJ),
            new DispatchableVehicleGroup("RHPDVehicles", RHPDVehicles_FEJ),
            new DispatchableVehicleGroup("DPPDVehicles", DPPDVehicles_FEJ),
            new DispatchableVehicleGroup("VWPDVehicles", VWPDVehicles_FEJ),
            new DispatchableVehicleGroup("EastLSPDVehicles", EastLSPDVehicles_FEJ),
            new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles),
            new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles),
            new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles),
            new DispatchableVehicleGroup("Firetrucks", Firetrucks),
            new DispatchableVehicleGroup("Amublance1", Amublance1),
            new DispatchableVehicleGroup("Amublance2", Amublance2),
            new DispatchableVehicleGroup("Amublance3", Amublance3),
            new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles_FEJ),
            new DispatchableVehicleGroup("MerryweatherPatrolVehicles", MerryweatherPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("BobcatSecurityVehicles", BobcatSecurityVehicles_FEJ),
            new DispatchableVehicleGroup("GroupSechsVehicles", GroupSechsVehicles_FEJ),
            new DispatchableVehicleGroup("SecuroservVehicles", SecuroservVehicles_FEJ),
            new DispatchableVehicleGroup("LCPDVehicles", LCPDVehicles_FEJ),

            new DispatchableVehicleGroup("BorderPatrolVehicles", BorderPatrolVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSEPIAVehicles", NOOSEPIAVehicles_FEJ),
            new DispatchableVehicleGroup("NOOSESEPVehicles", NOOSESEPVehicles_FEJ),
            new DispatchableVehicleGroup("MarshalsServiceVehicles", MarshalsServiceVehicles_FEJ),
            new DispatchableVehicleGroup("OffDutyCopVehicles",OffDutyCopVehicles),

            //Gang stuff
            new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles),
            new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles),
            new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles),
            new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles),
            new DispatchableVehicleGroup("BallasVehicles", BallasVehicles),
            new DispatchableVehicleGroup("VagosVehicles", VagosVehicles),
            new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles),
            new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles),
            new DispatchableVehicleGroup("TriadVehicles", TriadVehicles),
            new DispatchableVehicleGroup("YardieVehicles", YardieVehicles),
            new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles),
            new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles),
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
            new DispatchableVehicleGroup("DowntownTaxiVehicles", DowntownTaxiVehicles),
            new DispatchableVehicleGroup("HellTaxiVehicles", HellTaxiVehicles),
            new DispatchableVehicleGroup("PurpleTaxiVehicles", PurpleTaxiVehicles),
            new DispatchableVehicleGroup("ShitiTaxiVehicles", ShitiTaxiVehicles),
            new DispatchableVehicleGroup("SunderedTaxiVehicles",SunderedTaxiVehicles),
            //

        };

        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\DispatchableVehicles_FullExpandedJurisdiction.xml");
    }
}


