using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OldDispatchableVehicles : IDispatchableVehicles
{
    private string PoliceStanier = "police";
    private string PoliceBuffalo = "police2";
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
    private DispatchableVehicle AleutianSecurity;

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
        ArmeniaVehicles.AddRange(customGangs.ArmenianVehicles);

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
                LicensePlate = new LSR.Vehicles.LicensePlate("", 0, false),
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
        AleutianSecurity = new DispatchableVehicle("aleutian", 5, 0) //Undercover Gauntlet
        {
            DebugName = "AleutianSecurity_DLC",
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
    }
    private void SetupDefaultGangSpecialVehicles_Gambetti()
    {
        DispatchableVehicle baller4_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "baller4_PeterBadoingy_DLCDespawn",
            ModelName = "baller4",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 2,
                PearlescentColor = 4,
                WheelColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "GAM1 ",
                    PlateType = 53,
                },
                WheelType = 3,
                WindowTint = -1,
                VehicleExtras = new List<VehicleExtra>() {
                    new VehicleExtra() {},
                    new VehicleExtra() {
                        ID = 1,
                    },
                    new VehicleExtra() {
                        ID = 2,
                    },
                    new VehicleExtra() {
                        ID = 3,
                    },
                    new VehicleExtra() {
                        ID = 4,
                    },
                    new VehicleExtra() {
                        ID = 5,
                    },
                    new VehicleExtra() {
                        ID = 6,
                    },
                    new VehicleExtra() {
                        ID = 7,
                    },
                    new VehicleExtra() {
                        ID = 8,
                    },
                    new VehicleExtra() {
                        ID = 9,
                    },
                    new VehicleExtra() {
                        ID = 10,
                        IsTurnedOn = true,
                    },
                    new VehicleExtra() {
                        ID = 11,
                    },
                    new VehicleExtra() {
                        ID = 12,
                    },
                    new VehicleExtra() {
                        ID = 13,
                    },
                    new VehicleExtra() {
                        ID = 14,
                    },
                    new VehicleExtra() {
                        ID = 15,
                    },
                },
                VehicleToggles = new List<VehicleToggle>() {
                    new VehicleToggle() {
                        ID = 17,
                    },
                    new VehicleToggle() {
                        ID = 18,
                        IsTurnedOn = true,
                    },
                    new VehicleToggle() {
                        ID = 19,
                    },
                    new VehicleToggle() {
                        ID = 20,
                    },
                    new VehicleToggle() {
                        ID = 21,
                    },
                    new VehicleToggle() {
                        ID = 22,
                    },
                },
                VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 1,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 2,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 3,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 4,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 5,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 6,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 7,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 8,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 9,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 10,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 11,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 12,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 13,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 14,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 15,
                        Output = 1,
                    },
                    new VehicleMod() {
                        ID = 16,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 23,
                        Output = 11,
                    },
                    new VehicleMod() {
                        ID = 24,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 25,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 26,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 27,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 28,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 29,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 30,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 31,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 32,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 33,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 34,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 35,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 36,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 37,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 38,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 39,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 40,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 41,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 42,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 43,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 44,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 45,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 46,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 47,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 48,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 49,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 50,
                        Output = -1,
                    },
                },
                VehicleNeons = new List<VehicleNeon>() {
                    new VehicleNeon() {},
                    new VehicleNeon() {
                        ID = 1,
                    },
                    new VehicleNeon() {
                        ID = 2,
                    },
                    new VehicleNeon() {
                        ID = 3,
                    },
                },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 8,
                DashboardColor = 156,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle deity_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "deity_PeterBadoingy_DLCDespawn",
            ModelName = "deity",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 2,
                PearlescentColor = 4,
                WheelColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "GAM2 ",
                    PlateType = 53,
                },
                WheelType = 12,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {},
            new VehicleMod() {
                ID = 1,
                Output = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = 4,
            },
            new VehicleMod() {
                ID = 3,
            },
            new VehicleMod() {
                ID = 4,
                Output = 4,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = 2,
            },
            new VehicleMod() {
                ID = 7,
                Output = 2,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = 4,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 17,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                HasInvicibleTires = true,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorG = 255,
                NeonColorB = 255,
                InteriorColor = 16,
                DashboardColor = 134,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle paragon_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "paragon_PeterBadoingy_DLCDespawn",
            ModelName = "paragon",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 2,
                PearlescentColor = 4,
                WheelColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "GAM3 ",
                    PlateType = 53,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {},
            new VehicleMod() {
                ID = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = 1,
            },
            new VehicleMod() {
                ID = 3,
                Output = 1,
            },
            new VehicleMod() {
                ID = 4,
                Output = 1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = 1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 26,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 13,
                DashboardColor = 111,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle baller8_PeterBadoingy_DLC = new DispatchableVehicle("baller8", 75, 75) //Undercover Gauntlet
        {
            DebugName = "baller8_PeterBadoingy_DLC",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            MaxOccupants = 4,
            RequiresDLC = true,
            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 2,
                SecondaryColor = 0,
                PearlescentColor = 4,
                WheelColor = 147,
                Mod1PaintType = 7,
                Mod2PaintType = 7,
                WheelType = 3,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>()
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,false),
                        new VehicleExtra(2,false),
                        new VehicleExtra(3,false),
                        new VehicleExtra(4,false),
                        new VehicleExtra(5,false),
                        new VehicleExtra(6,false),
                        new VehicleExtra(7,false),
                        new VehicleExtra(8,false),
                        new VehicleExtra(9,false),
                        new VehicleExtra(10,false),
                        new VehicleExtra(11,false),
                        new VehicleExtra(12,false),
                        new VehicleExtra(13,false),
                        new VehicleExtra(14,false),
                        new VehicleExtra(15,false),
                    },
                VehicleToggles = new List<VehicleToggle>()
                    {
                        new VehicleToggle(17,false),
                        new VehicleToggle(18,false),
                        new VehicleToggle(19,false),
                        new VehicleToggle(20,false),
                        new VehicleToggle(21,false),
                        new VehicleToggle(22,false),
                    },
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(0,8),
                        new VehicleMod(1,0),
                        new VehicleMod(2,-1),
                        new VehicleMod(3,0),
                        new VehicleMod(4,-1),
                        new VehicleMod(5,1),
                        new VehicleMod(6,0),
                        new VehicleMod(7,2),
                        new VehicleMod(8,-1),
                        new VehicleMod(9,0),
                        new VehicleMod(10,0),
                        new VehicleMod(11,3),
                        new VehicleMod(12,-1),
                        new VehicleMod(13,-1),
                        new VehicleMod(14,-1),
                        new VehicleMod(15,-1),
                        new VehicleMod(16,-1),
                        new VehicleMod(23,12),
                        new VehicleMod(24,-1),
                        new VehicleMod(25,-1),
                        new VehicleMod(26,-1),
                        new VehicleMod(27,-1),
                        new VehicleMod(28,-1),
                        new VehicleMod(29,-1),
                        new VehicleMod(30,-1),
                        new VehicleMod(31,-1),
                        new VehicleMod(32,-1),
                        new VehicleMod(33,-1),
                        new VehicleMod(34,-1),
                        new VehicleMod(35,-1),
                        new VehicleMod(36,-1),
                        new VehicleMod(37,-1),
                        new VehicleMod(38,-1),
                        new VehicleMod(39,-1),
                        new VehicleMod(40,-1),
                        new VehicleMod(41,-1),
                        new VehicleMod(42,-1),
                        new VehicleMod(43,1),
                        new VehicleMod(44,-1),
                        new VehicleMod(45,-1),
                        new VehicleMod(46,-1),
                        new VehicleMod(47,-1),
                        new VehicleMod(48,-1),
                        new VehicleMod(49,-1),
                        new VehicleMod(50,-1),
                    },
            }
        };
        GambettiVehicles.Add(baller4_PeterBadoingy_DLCDespawn);
        GambettiVehicles.Add(deity_PeterBadoingy_DLCDespawn);
        GambettiVehicles.Add(paragon_PeterBadoingy_DLCDespawn);
        GambettiVehicles.Add(baller8_PeterBadoingy_DLC);
    }
    private void SetupDefaultGangSpecialVehicles_Pavano()
    {
        DispatchableVehicle tailgater_PeterBadoingy = new DispatchableVehicle()
        {
            DebugName = "tailgater_PeterBadoingy",
            ModelName = "tailgater",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 7,
                PearlescentColor = 5,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "PAV1 ",
                    PlateType = 36,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = 3,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
            },
            new VehicleMod() {
                ID = 7,
                Output = 5,
            },
            new VehicleMod() {
                ID = 8,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 22,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
        };
        DispatchableVehicle iwagen_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "iwagen_PeterBadoingy_DLCDespawn",
            ModelName = "iwagen",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 7,
                PearlescentColor = 5,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "PAV2 ",
                    PlateType = 36,
                },
                WheelType = 3,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {},
            new VehicleMod() {
                ID = 1,
                Output = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = 1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = 6,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 11,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 93,
                DashboardColor = 134,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle rocoto_PeterBadoingy = new DispatchableVehicle()
        {
            DebugName = "rocoto_PeterBadoingy",
            ModelName = "rocoto",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 7,
                PearlescentColor = 5,
                WheelColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "PAV3 ",
                    PlateType = 36,
                },
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 15,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
        };
        DispatchableVehicle tailgater2_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "tailgater2_PeterBadoingy_DLCDespawn",
            ModelName = "tailgater2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 7,
                PearlescentColor = 5,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "PAV4 ",
                    PlateType = 36,
                },
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = 5,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = 5,
            },
            new VehicleMod() {
                ID = 5,
                Output = 5,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = 3,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 7,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = 2,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = 2,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = 4,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 5,
                DashboardColor = 111,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        PavanoVehicles.Add(tailgater_PeterBadoingy);
        PavanoVehicles.Add(iwagen_PeterBadoingy_DLCDespawn);
        PavanoVehicles.Add(rocoto_PeterBadoingy);
        PavanoVehicles.Add(tailgater2_PeterBadoingy_DLCDespawn);
    }
    private void SetupDefaultGangSpecialVehicles_Lupisella()
    {
        DispatchableVehicle komoda_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "komoda_PeterBadoingy_DLCDespawn",
            ModelName = "komoda",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 96,
                SecondaryColor = 96,
                PearlescentColor = 95,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LUP1 ",
                    PlateType = 25,
                },
                WheelType = 7,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
            },
            new VehicleMod() {
                ID = 4,
                Output = 3,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = 2,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = 3,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 9,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                DirtLevel = 1f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 4,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle cypher_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "cypher_PeterBadoingy_DLCDespawn",
            ModelName = "cypher",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 96,
                SecondaryColor = 96,
                PearlescentColor = 95,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LUP2 ",
                    PlateType = 25,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = 5,
            },
            new VehicleMod() {
                ID = 2,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = 2,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
            },
            new VehicleMod() {
                ID = 7,
                Output = 7,
            },
            new VehicleMod() {
                ID = 8,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = 4,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 16,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = 1,
            },
            new VehicleMod() {
                ID = 27,
                Output = 1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = 1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = 3,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                DirtLevel = 1f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 22,
                DashboardColor = 157,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle rhinehart_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "rhinehart_PeterBadoingy_DLCDespawn",
            ModelName = "rhinehart",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 96,
                SecondaryColor = 96,
                PearlescentColor = 95,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LUP3 ",
                    PlateType = 25,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = 1,
            },
            new VehicleMod() {
                ID = 1,
                Output = 3,
            },
            new VehicleMod() {
                ID = 2,
                Output = 3,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = 3,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = 3,
            },
            new VehicleMod() {
                ID = 7,
                Output = 2,
            },
            new VehicleMod() {
                ID = 8,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = 9,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 22,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 41.92045f,
                DirtLevel = 0.001918551f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 93,
                DashboardColor = 134,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        LupisellaVehicles.Add(komoda_PeterBadoingy_DLCDespawn);
        LupisellaVehicles.Add(cypher_PeterBadoingy_DLCDespawn);
        LupisellaVehicles.Add(rhinehart_PeterBadoingy_DLCDespawn);
    }
    private void SetupDefaultGangSpecialVehicles_Messina()
    {
        DispatchableVehicle superd_PeterBadoingy = new DispatchableVehicle()
        {
            DebugName = "superd_PeterBadoingy",
            ModelName = "superd",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 34,
                SecondaryColor = 147,
                PearlescentColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "MES1 ",
                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 29,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
        };
        DispatchableVehicle windsor_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "windsor_PeterBadoingy_DLCDespawn",
            ModelName = "windsor",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 34,
                SecondaryColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = 0,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "MES2 ",
                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 26,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle windsor2_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "windsor2_PeterBadoingy_DLCDespawn",
            ModelName = "windsor2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 34,
                SecondaryColor = 147,
                PearlescentColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "MES3 ",
                    PlateType = 35,
                },
                WheelType = 12,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 2,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 21,
                DashboardColor = 27,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle jubilee_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "jubilee_PeterBadoingy_DLCDespawn",
            ModelName = "jubilee",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 34,
                SecondaryColor = 147,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "MES4 ",
                    PlateType = 35,
                },
                WheelType = 12,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = 9,
            },
            new VehicleMod() {
                ID = 2,
                Output = 7,
            },
            new VehicleMod() {
                ID = 3,
                Output = 8,
            },
            new VehicleMod() {
                ID = 4,
                Output = 5,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = 14,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 26,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                HasInvicibleTires = true,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 93,
                DashboardColor = 134,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        MessinaVehicles.Add(superd_PeterBadoingy);
        MessinaVehicles.Add(windsor_PeterBadoingy_DLCDespawn);
        MessinaVehicles.Add(windsor2_PeterBadoingy_DLCDespawn);
        MessinaVehicles.Add(jubilee_PeterBadoingy_DLCDespawn);
    }
    private void SetupDefaultGangSpecialVehicles_Ancelotti()
    {
        DispatchableVehicle huntley_PeterBadoingy = new DispatchableVehicle()
        {
            DebugName = "huntley_PeterBadoingy",
            ModelName = "huntley",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 141,
                SecondaryColor = 141,
                PearlescentColor = 73,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "ANC1 ",
                    PlateType = 1,
                },
                WheelType = 3,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
                    new VehicleExtra() {},
                    new VehicleExtra() {
                        ID = 1,
                    },
                    new VehicleExtra() {
                        ID = 2,
                    },
                    new VehicleExtra() {
                        ID = 3,
                    },
                    new VehicleExtra() {
                        ID = 4,
                    },
                    new VehicleExtra() {
                        ID = 5,
                    },
                    new VehicleExtra() {
                        ID = 6,
                    },
                    new VehicleExtra() {
                        ID = 7,
                    },
                    new VehicleExtra() {
                        ID = 8,
                    },
                    new VehicleExtra() {
                        ID = 9,
                    },
                    new VehicleExtra() {
                        ID = 10,
                        IsTurnedOn = true,
                    },
                    new VehicleExtra() {
                        ID = 11,
                    },
                    new VehicleExtra() {
                        ID = 12,
                        IsTurnedOn = true,
                    },
                    new VehicleExtra() {
                        ID = 13,
                    },
                    new VehicleExtra() {
                        ID = 14,
                    },
                    new VehicleExtra() {
                        ID = 15,
                    },
                },
                VehicleToggles = new List<VehicleToggle>() {
                    new VehicleToggle() {
                        ID = 17,
                    },
                    new VehicleToggle() {
                        ID = 18,
                        IsTurnedOn = true,
                    },
                    new VehicleToggle() {
                        ID = 19,
                    },
                    new VehicleToggle() {
                        ID = 20,
                    },
                    new VehicleToggle() {
                        ID = 21,
                    },
                    new VehicleToggle() {
                        ID = 22,
                    },
                },
                VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 1,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 2,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 3,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 4,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 5,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 6,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 7,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 8,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 9,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 10,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 11,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 12,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 13,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 14,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 15,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 16,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 23,
                        Output = 9,
                    },
                    new VehicleMod() {
                        ID = 24,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 25,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 26,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 27,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 28,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 29,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 30,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 31,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 32,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 33,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 34,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 35,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 36,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 37,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 38,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 39,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 40,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 41,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 42,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 43,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 44,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 45,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 46,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 47,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 48,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 49,
                        Output = -1,
                    },
                    new VehicleMod() {
                        ID = 50,
                        Output = -1,
                    },
                },
                VehicleNeons = new List<VehicleNeon>() {
                    new VehicleNeon() {},
                    new VehicleNeon() {
                        ID = 1,
                    },
                    new VehicleNeon() {
                        ID = 2,
                    },
                    new VehicleNeon() {
                        ID = 3,
                    },
                },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
        };
        DispatchableVehicle cogcabrio_PeterBadoingy = new DispatchableVehicle()
        {
            DebugName = "cogcabrio_PeterBadoingy",
            ModelName = "cogcabrio",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 141,
                SecondaryColor = 141,
                PearlescentColor = 73,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "ANC2 ",
                    PlateType = 1,
                },
                WheelType = 12,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 17,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 60f,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                XenonLightColor = 255,
            },
        };
        DispatchableVehicle cog55_PeterBadoingy_DLCDespawn = new DispatchableVehicle()
        {
            DebugName = "cog55_PeterBadoingy_DLCDespawn",
            ModelName = "cog55",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            ForceStayInSeats = new List<Int32>() { },
            CaninePossibleSeats = new List<Int32>() { },
            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,
            RequiredLiveries = new List<Int32>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            OptionalColors = new List<Int32>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 141,
                SecondaryColor = 141,
                PearlescentColor = 73,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "ANC3 ",
                    PlateType = 1,
                },
                WheelType = 11,
                WindowTint = 3,
                VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {},
            new VehicleExtra() {
                ID = 1,
            },
            new VehicleExtra() {
                ID = 2,
            },
            new VehicleExtra() {
                ID = 3,
            },
            new VehicleExtra() {
                ID = 4,
            },
            new VehicleExtra() {
                ID = 5,
            },
            new VehicleExtra() {
                ID = 6,
            },
            new VehicleExtra() {
                ID = 7,
            },
            new VehicleExtra() {
                ID = 8,
            },
            new VehicleExtra() {
                ID = 9,
            },
            new VehicleExtra() {
                ID = 10,
            },
            new VehicleExtra() {
                ID = 11,
            },
            new VehicleExtra() {
                ID = 12,
            },
            new VehicleExtra() {
                ID = 13,
            },
            new VehicleExtra() {
                ID = 14,
            },
            new VehicleExtra() {
                ID = 15,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
                ID = 17,
            },
            new VehicleToggle() {
                ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
                ID = 19,
            },
            new VehicleToggle() {
                ID = 20,
            },
            new VehicleToggle() {
                ID = 21,
            },
            new VehicleToggle() {
                ID = 22,
            },
        },
                VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
                Output = -1,
            },
            new VehicleMod() {
                ID = 1,
                Output = -1,
            },
            new VehicleMod() {
                ID = 2,
                Output = -1,
            },
            new VehicleMod() {
                ID = 3,
                Output = -1,
            },
            new VehicleMod() {
                ID = 4,
                Output = -1,
            },
            new VehicleMod() {
                ID = 5,
                Output = -1,
            },
            new VehicleMod() {
                ID = 6,
                Output = -1,
            },
            new VehicleMod() {
                ID = 7,
                Output = -1,
            },
            new VehicleMod() {
                ID = 8,
                Output = -1,
            },
            new VehicleMod() {
                ID = 9,
                Output = -1,
            },
            new VehicleMod() {
                ID = 10,
                Output = -1,
            },
            new VehicleMod() {
                ID = 11,
                Output = -1,
            },
            new VehicleMod() {
                ID = 12,
                Output = -1,
            },
            new VehicleMod() {
                ID = 13,
                Output = -1,
            },
            new VehicleMod() {
                ID = 14,
                Output = -1,
            },
            new VehicleMod() {
                ID = 15,
                Output = -1,
            },
            new VehicleMod() {
                ID = 16,
                Output = -1,
            },
            new VehicleMod() {
                ID = 23,
                Output = 16,
            },
            new VehicleMod() {
                ID = 24,
                Output = -1,
            },
            new VehicleMod() {
                ID = 25,
                Output = -1,
            },
            new VehicleMod() {
                ID = 26,
                Output = -1,
            },
            new VehicleMod() {
                ID = 27,
                Output = -1,
            },
            new VehicleMod() {
                ID = 28,
                Output = -1,
            },
            new VehicleMod() {
                ID = 29,
                Output = -1,
            },
            new VehicleMod() {
                ID = 30,
                Output = -1,
            },
            new VehicleMod() {
                ID = 31,
                Output = -1,
            },
            new VehicleMod() {
                ID = 32,
                Output = -1,
            },
            new VehicleMod() {
                ID = 33,
                Output = -1,
            },
            new VehicleMod() {
                ID = 34,
                Output = -1,
            },
            new VehicleMod() {
                ID = 35,
                Output = -1,
            },
            new VehicleMod() {
                ID = 36,
                Output = -1,
            },
            new VehicleMod() {
                ID = 37,
                Output = -1,
            },
            new VehicleMod() {
                ID = 38,
                Output = -1,
            },
            new VehicleMod() {
                ID = 39,
                Output = -1,
            },
            new VehicleMod() {
                ID = 40,
                Output = -1,
            },
            new VehicleMod() {
                ID = 41,
                Output = -1,
            },
            new VehicleMod() {
                ID = 42,
                Output = -1,
            },
            new VehicleMod() {
                ID = 43,
                Output = -1,
            },
            new VehicleMod() {
                ID = 44,
                Output = -1,
            },
            new VehicleMod() {
                ID = 45,
                Output = -1,
            },
            new VehicleMod() {
                ID = 46,
                Output = -1,
            },
            new VehicleMod() {
                ID = 47,
                Output = -1,
            },
            new VehicleMod() {
                ID = 48,
                Output = -1,
            },
            new VehicleMod() {
                ID = 49,
                Output = -1,
            },
            new VehicleMod() {
                ID = 50,
                Output = -1,
            },
        },
                VehicleNeons = new List<VehicleNeon>() {
            new VehicleNeon() {},
            new VehicleNeon() {
                ID = 1,
            },
            new VehicleNeon() {
                ID = 2,
            },
            new VehicleNeon() {
                ID = 3,
            },
        },
                FuelLevel = 24.49741f,
                DirtLevel = 0.0277027f,
                HasInvicibleTires = true,
                IsTireSmokeColorCustom = true,
                TireSmokeColorR = 255,
                TireSmokeColorG = 255,
                TireSmokeColorB = 255,
                NeonColorR = 255,
                NeonColorB = 255,
                InteriorColor = 93,
                DashboardColor = 156,
                XenonLightColor = 255,
            },
            RequiresDLC = true,
        };
        AncelottiVehicles.Add(huntley_PeterBadoingy);
        AncelottiVehicles.Add(cogcabrio_PeterBadoingy);
        AncelottiVehicles.Add(cog55_PeterBadoingy_DLCDespawn);
    }
    private void SetupDefaultGangSpecialVehicles_Redneck()
    {
        //Redneck
        DispatchableVehicle RedneckCustom1 = new DispatchableVehicle()
        {
            DebugName = "dukes3_PeterBadoingy_DLCDespawn",
            ModelName = "dukes3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 21,
                SecondaryColor = 21,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 2,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS RED01",

                    PlateType = 8,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 0,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 4,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 3,
            },
            new VehicleMod() {
              ID = 7,
                Output = 1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 0,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 8,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 2,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle RedneckCustom2 = new DispatchableVehicle()
        {
            DebugName = "slamvan3_PeterBadoingy_DLCDespawn",
            ModelName = "slamvan3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 118,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 21,
                SecondaryColor = 118,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 2,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS RED02",

                    PlateType = 8,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 4,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 0,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 1,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 4,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 0,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 7,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle RedneckCustom3 = new DispatchableVehicle()
        {
            DebugName = "slamvan4_PeterBadoingy_DLCDespawn",
            ModelName = "slamvan4",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 118,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 21,
                SecondaryColor = 118,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 2,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS RED03",

                    PlateType = 54,
                },
                WheelType = 1,
                WindowTint = -1,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 2,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = -1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = 2,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = 2,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 2,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle RedneckCustom4 = new DispatchableVehicle()
        {
            DebugName = "yosemite3_PeterBadoingy_DLCDespawn",
            ModelName = "yosemite3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 21,
                SecondaryColor = 21,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 120,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS RED04",

                    PlateType = 8,
                },
                WheelType = 4,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 2,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 10,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 0,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 14,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle RedneckCustom5 = new DispatchableVehicle()
        {
            DebugName = "broadway_PeterBadoingy_DLCDespawn",
            ModelName = "broadway",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 21,
                SecondaryColor = 21,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 120,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS RED05",

                    PlateType = 8,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 3,
            },
            new VehicleMod() {
              ID = 4,
                Output = 1,
            },
            new VehicleMod() {
              ID = 5,
                Output = 3,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = 0,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 7,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 9,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };

        RedneckVehicles.Add(RedneckCustom1);
        RedneckVehicles.Add(RedneckCustom2);
        RedneckVehicles.Add(RedneckCustom3);
        RedneckVehicles.Add(RedneckCustom4);
        RedneckVehicles.Add(RedneckCustom5);
    }
    private void SetupDefaultGangSpecialVehicles()
    {
        SetupDefaultGangSpecialVehicles_Gambetti();
        SetupDefaultGangSpecialVehicles_Pavano();
        SetupDefaultGangSpecialVehicles_Lupisella();
        SetupDefaultGangSpecialVehicles_Messina();
        SetupDefaultGangSpecialVehicles_Ancelotti();
        SetupDefaultGangSpecialVehicles_Redneck();
        //Families
        FamiliesVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "peyote3_PeterBadoingy_DLCDespawn",
            ModelName = "peyote3",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 156,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = 0,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FAMILIES",

                    PlateType = 10,
                },
                WheelType = 2,
                WindowTint = -1,
                HasCustomWheels = true,
                VehicleExtras = new List<VehicleExtra>() {
                      new VehicleExtra() {
                          ID = 0,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 1,
                            IsTurnedOn = true,
                        },
                        new VehicleExtra() {
                          ID = 2,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 3,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 4,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 5,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 6,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 7,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 8,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 9,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 10,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 11,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 12,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 13,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 14,
                            IsTurnedOn = false,
                        },
                        new VehicleExtra() {
                          ID = 15,
                            IsTurnedOn = false,
                        },
                    },
                VehicleToggles = new List<VehicleToggle>() {
                      new VehicleToggle() {
                          ID = 17,
                            IsTurnedOn = true,
                        },
                        new VehicleToggle() {
                          ID = 18,
                            IsTurnedOn = true,
                        },
                        new VehicleToggle() {
                          ID = 19,
                            IsTurnedOn = false,
                        },
                        new VehicleToggle() {
                          ID = 20,
                            IsTurnedOn = false,
                        },
                        new VehicleToggle() {
                          ID = 21,
                            IsTurnedOn = false,
                        },
                        new VehicleToggle() {
                          ID = 22,
                            IsTurnedOn = false,
                        },
                    },
                VehicleMods = new List<VehicleMod>() {
                      new VehicleMod() {
                          ID = 0,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 1,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 2,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 3,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 4,
                            Output = 0,
                        },
                        new VehicleMod() {
                          ID = 5,
                            Output = 0,
                        },
                        new VehicleMod() {
                          ID = 6,
                            Output = 2,
                        },
                        new VehicleMod() {
                          ID = 7,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 8,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 9,
                            Output = 0,
                        },
                        new VehicleMod() {
                          ID = 10,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 11,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 12,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 13,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 14,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 15,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 16,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 23,
                            Output = 2,
                        },
                        new VehicleMod() {
                          ID = 24,
                            Output = 3,
                        },
                        new VehicleMod() {
                          ID = 25,
                            Output = 4,
                        },
                        new VehicleMod() {
                          ID = 26,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 27,
                            Output = 7,
                        },
                        new VehicleMod() {
                          ID = 28,
                            Output = 5,
                        },
                        new VehicleMod() {
                          ID = 29,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 30,
                            Output = 0,
                        },
                        new VehicleMod() {
                          ID = 31,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 32,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 33,
                            Output = 1,
                        },
                        new VehicleMod() {
                          ID = 34,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 35,
                            Output = 2,
                        },
                        new VehicleMod() {
                          ID = 36,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 37,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 38,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 39,
                            Output = 1,
                        },
                        new VehicleMod() {
                          ID = 40,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 41,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 42,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 43,
                            Output = 0,
                        },
                        new VehicleMod() {
                          ID = 44,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 45,
                            Output = 1,
                        },
                        new VehicleMod() {
                          ID = 46,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 47,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 48,
                            Output = 1,
                        },
                        new VehicleMod() {
                          ID = 49,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 50,
                            Output = -1,
                        },
                        new VehicleMod() {
                          ID = 66,
                            Output = 55,
                        },
                        new VehicleMod() {
                          ID = 67,
                            Output = 0,
                        },
                    },
            },
        });

        DispatchableVehicle FamiliesCustom2 = new DispatchableVehicle()
        {
            DebugName = "manana2_PeterBadoingy_DLCDespawn",
            ModelName = "manana2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 53,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 59,
                WheelColor = 3,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FA-FD-01",

                    PlateType = 73,
                },
                WheelType = 9,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = 0,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 39,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = 4,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 0,
            },
            new VehicleMod() {
              ID = 28,
                Output = 5,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 1,
            },
            new VehicleMod() {
              ID = 34,
                Output = 2,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 3,
            },
            new VehicleMod() {
              ID = 38,
                Output = 4,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = 1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 0,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle FamiliesCustom3 = new DispatchableVehicle()
        {
            DebugName = "glendale2_PeterBadoingy_DLCDespawn",
            ModelName = "glendale2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 53,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 3,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FA-FD-02",

                    PlateType = 73,
                },
                WheelType = 2,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 5,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 4,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 6,
            },
            new VehicleMod() {
              ID = 28,
                Output = 5,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 4,
            },
            new VehicleMod() {
              ID = 34,
                Output = 2,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = 3,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 4,
            },
            new VehicleMod() {
              ID = 39,
                Output = 4,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 8,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle FamiliesCustom4 = new DispatchableVehicle()
        {
            DebugName = "minivan2_PeterBadoingy_DLCDespawn",
            ModelName = "minivan2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 53,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 59,
                WheelColor = 3,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FA-FD-03",

                    PlateType = 73,
                },
                WheelType = 0,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 0,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = 5,
            },
            new VehicleMod() {
              ID = 25,
                Output = 4,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 1,
            },
            new VehicleMod() {
              ID = 28,
                Output = 5,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = 3,
            },
            new VehicleMod() {
              ID = 32,
                Output = 0,
            },
            new VehicleMod() {
              ID = 33,
                Output = 6,
            },
            new VehicleMod() {
              ID = 34,
                Output = 2,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 6,
            },
            new VehicleMod() {
              ID = 38,
                Output = 3,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 0,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle FamiliesCustom5 = new DispatchableVehicle()
        {
            DebugName = "manchez_PeterBadoingy_DLCDespawn",
            ModelName = "manchez",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 1,
            AmbientSpawnChance = 10,
            WantedSpawnChance = 10,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 53,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FA-FD-04",

                    PlateType = 73,
                },
                WheelType = 6,
                WindowTint = -1,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 2,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 2,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle FamiliesCustom6 = new DispatchableVehicle()
        {
            DebugName = "peyote3_PeterBadoingy_DLCDespawn",
            ModelName = "peyote3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 53,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 53,
                SecondaryColor = 53,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 156,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = 0,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "FA-FD-05",

                    PlateType = 10,
                },
                WheelType = 2,
                WindowTint = -1,
                HasCustomWheels = true,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = 2,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 0,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 2,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 4,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 7,
            },
            new VehicleMod() {
              ID = 28,
                Output = 5,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = 0,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = 2,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = 1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = 0,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
            new VehicleMod() {
              ID = 66,
                Output = 55,
            },
            new VehicleMod() {
              ID = 67,
                Output = 0,
            },
        },
            },
            RequiresDLC = true,
        };

        FamiliesVehicles.Add(FamiliesCustom2);
        FamiliesVehicles.Add(FamiliesCustom3);
        FamiliesVehicles.Add(FamiliesCustom4);
        FamiliesVehicles.Add(FamiliesCustom5);
        FamiliesVehicles.Add(FamiliesCustom6);

        //Varios
        VarriosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "BUCCANEER2_PeterBadoingy",
            ModelName = "BUCCANEER2",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 63,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 63,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 90,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "V4RRI-O1",

                    PlateType = 22,
                },
                WheelType = 1,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = true,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = 0,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 0,
},
new VehicleMod() {
ID = 7,
Output = 1,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = -1,
},
new VehicleMod() {
ID = 24,
Output = 3,
},
new VehicleMod() {
ID = 25,
Output = 9,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = 5,
},
new VehicleMod() {
ID = 28,
Output = 2,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = 9,
},
new VehicleMod() {
ID = 34,
Output = 9,
},
new VehicleMod() {
ID = 35,
Output = 18,
},
new VehicleMod() {
ID = 36,
Output = 2,
},
new VehicleMod() {
ID = 37,
Output = 6,
},
new VehicleMod() {
ID = 38,
Output = 3,
},
new VehicleMod() {
ID = 39,
Output = 1,
},
new VehicleMod() {
ID = 40,
Output = 1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = 1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 5,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        VarriosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "BUCCANEER2_PeterBadoingy",
            ModelName = "BUCCANEER2",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 63,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 63,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 90,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "V4RRI-O2",

                    PlateType = 22,
                },
                WheelType = 1,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = true,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = 0,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 0,
},
new VehicleMod() {
ID = 7,
Output = 1,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = -1,
},
new VehicleMod() {
ID = 24,
Output = 3,
},
new VehicleMod() {
ID = 25,
Output = 9,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = 5,
},
new VehicleMod() {
ID = 28,
Output = 2,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = 9,
},
new VehicleMod() {
ID = 34,
Output = 9,
},
new VehicleMod() {
ID = 35,
Output = 18,
},
new VehicleMod() {
ID = 36,
Output = 2,
},
new VehicleMod() {
ID = 37,
Output = 6,
},
new VehicleMod() {
ID = 38,
Output = 3,
},
new VehicleMod() {
ID = 39,
Output = 1,
},
new VehicleMod() {
ID = 40,
Output = 1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = 1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 5,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        VarriosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "VAMOS_PeterBadoingy_DLCDespawn",
            ModelName = "VAMOS",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 63,
            RequiredSecondaryColorID = 12,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 63,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "V4RRI-O3",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 0,
},
new VehicleMod() {
ID = 1,
Output = 4,
},
new VehicleMod() {
ID = 2,
Output = 0,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 4,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 2,
},
new VehicleMod() {
ID = 7,
Output = 2,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = 0,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 1,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 3,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        VarriosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "tulip_PeterBadoingy_DLCDespawn",
            ModelName = "tulip",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 63,
            RequiredSecondaryColorID = 12,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 63,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "V4RRI-O4",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 2,
},
new VehicleMod() {
ID = 1,
Output = 1,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = 1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 0,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 3,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });

        //Vagos
        VagosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "SLAMVAN3_PeterBadoingy_DLCDespawn",
            ModelName = "SLAMVAN3",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 88,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 88,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "VAGOS001",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 3,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 4,
},
new VehicleMod() {
ID = 7,
Output = 3,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = 0,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 8,
},
new VehicleMod() {
ID = 24,
Output = 3,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = 1,
},
new VehicleMod() {
ID = 28,
Output = 0,
},
new VehicleMod() {
ID = 29,
Output = 0,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = 0,
},
new VehicleMod() {
ID = 33,
Output = 9,
},
new VehicleMod() {
ID = 34,
Output = 13,
},
new VehicleMod() {
ID = 35,
Output = 17,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = 3,
},
new VehicleMod() {
ID = 39,
Output = 2,
},
new VehicleMod() {
ID = 40,
Output = 1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = 0,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 2,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        VagosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "tulip2_PeterBadoingy_DLCDespawn",
            ModelName = "tulip2",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 88,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 88,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "VAGOS002",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 0,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = 1,
},
new VehicleMod() {
ID = 4,
Output = 4,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 3,
},
new VehicleMod() {
ID = 7,
Output = 5,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 0,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 8,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        VagosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "DEVIANT_PeterBadoingy_DLCDespawn",
            ModelName = "DEVIANT",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 88,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 88,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "VAGOS003",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = true,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 6,
},
new VehicleMod() {
ID = 1,
Output = -1,
},
new VehicleMod() {
ID = 2,
Output = 0,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 1,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 9,
},
new VehicleMod() {
ID = 7,
Output = 3,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = 0,
},
new VehicleMod() {
ID = 11,
Output = 3,
},
new VehicleMod() {
ID = 12,
Output = 2,
},
new VehicleMod() {
ID = 13,
Output = 3,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = 1,
},
new VehicleMod() {
ID = 16,
Output = 4,
},
new VehicleMod() {
ID = 23,
Output = 23,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 4,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = 3,
},
},
            },
        });
        VagosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "IMPALER_PeterBadoingy_DLCDespawn",
            ModelName = "IMPALER",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 88,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 88,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "VAGOS004",

                    PlateType = 22,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 1,
},
new VehicleMod() {
ID = 1,
Output = 1,
},
new VehicleMod() {
ID = 2,
Output = 1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = -1,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 1,
},
new VehicleMod() {
ID = 7,
Output = 4,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 2,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 3,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });


        //Diablos
        DiablosVehicles.Add(new DispatchableVehicle("stalion", 25, 25)//Red Stallion
        {
            DebugName = "stalion_DefaultCustomDiablos_Vanilla",
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 0,
                PearlescentColor = 28,
                WheelColor = 156,
                Mod1PaintType = 0,
                Mod1PearlescentColor = 0,
                Mod1Color = 0,
                Mod2PaintType = 1,
                WheelType = 7,
                HasCustomWheels = true,
                VehicleExtras = new List<VehicleExtra>()
                {
                    new VehicleExtra(0,false),
                    new VehicleExtra(1,false),
                    new VehicleExtra(2,false),
                    new VehicleExtra(3,true),
                    new VehicleExtra(4,false),
                    new VehicleExtra(5,false),
                    new VehicleExtra(6,false),
                    new VehicleExtra(7,false),
                    new VehicleExtra(8,false),
                    new VehicleExtra(9,false),
                    new VehicleExtra(10,false),
                    new VehicleExtra(11,false),
                    new VehicleExtra(12,false),
                    new VehicleExtra(13,false),
                    new VehicleExtra(14,false),
                    new VehicleExtra(15,false),
                },
                VehicleToggles = new List<VehicleToggle>()
                {
                    new VehicleToggle(17,false),
                    new VehicleToggle(18,true),
                    new VehicleToggle(19,false),
                    new VehicleToggle(20,false),
                    new VehicleToggle(21,false),
                    new VehicleToggle(22,true),
                },
                VehicleMods = new List<VehicleMod>()
                {
                    new VehicleMod(0,-1),
                    new VehicleMod(1,-1),
                    new VehicleMod(2,-1),
                    new VehicleMod(3,-1),
                    new VehicleMod(4,-1),
                    new VehicleMod(5,-1),
                    new VehicleMod(6,-1),
                    new VehicleMod(7,-1),
                    new VehicleMod(8,-1),
                    new VehicleMod(9,-1),
                    new VehicleMod(10,-1),
                    new VehicleMod(11,3),
                    new VehicleMod(12,2),
                    new VehicleMod(13,2),
                    new VehicleMod(14,-1),
                    new VehicleMod(15,3),
                    new VehicleMod(16,-1),
                    new VehicleMod(17,-1),
                    new VehicleMod(18,-1),
                    new VehicleMod(19,-1),
                    new VehicleMod(20,-1),
                    new VehicleMod(21,-1),
                    new VehicleMod(22,-1),
                    new VehicleMod(23,2),
                    new VehicleMod(24,-1),
                    new VehicleMod(25,-1),
                    new VehicleMod(26,-1),
                    new VehicleMod(27,-1),
                    new VehicleMod(28,-1),
                    new VehicleMod(29,-1),
                    new VehicleMod(30,-1),
                    new VehicleMod(31,-1),
                    new VehicleMod(32,-1),
                    new VehicleMod(33,-1),
                    new VehicleMod(34,-1),
                    new VehicleMod(35,-1),
                    new VehicleMod(36,-1),
                    new VehicleMod(37,-1),
                    new VehicleMod(38,-1),
                    new VehicleMod(39,-1),
                    new VehicleMod(40,-1),
                    new VehicleMod(41,-1),
                    new VehicleMod(42,-1),
                    new VehicleMod(43,-1),
                    new VehicleMod(44,-1),
                    new VehicleMod(45,-1),
                    new VehicleMod(46,-1),
                    new VehicleMod(47,-1),
                    new VehicleMod(48,-1),
                    new VehicleMod(49,-1),
                    new VehicleMod(50,3),

                },
                LicensePlate = new LSR.Vehicles.LicensePlate("5GNU769", 0, false)
            }
        });
        DiablosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "HERMES_PeterBadoingy_DLCDespawn",
            ModelName = "HERMES",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 28,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "DIABLO01",

                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = true,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 4,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = -1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = 3,
},
new VehicleMod() {
ID = 12,
Output = 2,
},
new VehicleMod() {
ID = 13,
Output = 2,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = 0,
},
new VehicleMod() {
ID = 16,
Output = 4,
},
new VehicleMod() {
ID = 23,
Output = 3,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 2,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = 3,
},
},
            },
        });
        DiablosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "stalion_PeterBadoingy",
            ModelName = "stalion",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 28,
            RequiredSecondaryColorID = 12,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "DIABLO02",

                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = true,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = -1,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = -1,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = -1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 1,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = -1,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
        });
        DiablosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "gauntlet3_PeterBadoingy_DLCDespawn",
            ModelName = "gauntlet3",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 28,
            RequiredSecondaryColorID = 12,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "DIABLO03",

                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 3,
},
new VehicleMod() {
ID = 1,
Output = 3,
},
new VehicleMod() {
ID = 2,
Output = 1,
},
new VehicleMod() {
ID = 3,
Output = 4,
},
new VehicleMod() {
ID = 4,
Output = 3,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 0,
},
new VehicleMod() {
ID = 7,
Output = 4,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = 1,
},
new VehicleMod() {
ID = 10,
Output = 7,
},
new VehicleMod() {
ID = 11,
Output = 3,
},
new VehicleMod() {
ID = 12,
Output = 2,
},
new VehicleMod() {
ID = 13,
Output = 3,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = 4,
},
new VehicleMod() {
ID = 23,
Output = 2,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 0,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = 3,
},
},
            },
        });
        DiablosVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "GAUNTLET5_PeterBadoingy_DLCDespawn",
            ModelName = "GAUNTLET5",
            RequiresDLC = true,
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            RequiredPrimaryColorID = 28,
            RequiredSecondaryColorID = 120,
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 28,
                SecondaryColor = 120,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "DIABLO04",

                    PlateType = 35,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 3,
},
new VehicleMod() {
ID = 1,
Output = 9,
},
new VehicleMod() {
ID = 2,
Output = 4,
},
new VehicleMod() {
ID = 3,
Output = 4,
},
new VehicleMod() {
ID = 4,
Output = 2,
},
new VehicleMod() {
ID = 5,
Output = 1,
},
new VehicleMod() {
ID = 6,
Output = 4,
},
new VehicleMod() {
ID = 7,
Output = 4,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = 1,
},
new VehicleMod() {
ID = 10,
Output = 1,
},
new VehicleMod() {
ID = 11,
Output = 3,
},
new VehicleMod() {
ID = 12,
Output = 2,
},
new VehicleMod() {
ID = 13,
Output = 2,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = 0,
},
new VehicleMod() {
ID = 16,
Output = 4,
},
new VehicleMod() {
ID = 23,
Output = 4,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = 4,
},
new VehicleMod() {
ID = 29,
Output = 0,
},
new VehicleMod() {
ID = 30,
Output = 3,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = 6,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = 5,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = 2,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = 2,
},
new VehicleMod() {
ID = 45,
Output = 1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 2,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = 3,
},
},
            },
        });

        //Korean
        KoreanVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "JESTER3_PeterBadoingy_DLCDespawn",
            ModelName = "JESTER3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "KKANG 01",

                    PlateType = 57,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 1,
},
new VehicleMod() {
ID = 1,
Output = 6,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = 0,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = 4,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 24,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 9,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        KoreanVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "ZR350_PeterBadoingy_DLCDespawn",
            ModelName = "ZR350",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "KKANG 02",

                    PlateType = 57,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 0,
},
new VehicleMod() {
ID = 1,
Output = 1,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = 10,
},
new VehicleMod() {
ID = 4,
Output = 5,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 3,
},
new VehicleMod() {
ID = 7,
Output = 2,
},
new VehicleMod() {
ID = 8,
Output = 2,
},
new VehicleMod() {
ID = 9,
Output = 2,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 14,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 12,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        KoreanVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "RT3000_PeterBadoingy_DLCDespawn",
            ModelName = "RT3000",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "KKANG 03",

                    PlateType = 57,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 0,
},
new VehicleMod() {
ID = 1,
Output = 2,
},
new VehicleMod() {
ID = 2,
Output = 4,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 8,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 4,
},
new VehicleMod() {
ID = 7,
Output = 2,
},
new VehicleMod() {
ID = 8,
Output = 1,
},
new VehicleMod() {
ID = 9,
Output = 0,
},
new VehicleMod() {
ID = 10,
Output = 3,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 6,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = 0,
},
new VehicleMod() {
ID = 27,
Output = 2,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = 0,
},
new VehicleMod() {
ID = 32,
Output = 1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = 0,
},
new VehicleMod() {
ID = 40,
Output = 6,
},
new VehicleMod() {
ID = 41,
Output = 5,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = 1,
},
new VehicleMod() {
ID = 47,
Output = 0,
},
new VehicleMod() {
ID = 48,
Output = 6,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        KoreanVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "SULTAN2_PeterBadoingy_DLCDespawn",
            ModelName = "SULTAN2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "KKANG 04",

                    PlateType = 57,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 3,
},
new VehicleMod() {
ID = 2,
Output = 3,
},
new VehicleMod() {
ID = 3,
Output = 2,
},
new VehicleMod() {
ID = 4,
Output = 8,
},
new VehicleMod() {
ID = 5,
Output = 0,
},
new VehicleMod() {
ID = 6,
Output = 2,
},
new VehicleMod() {
ID = 7,
Output = 7,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = 4,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 16,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 8,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        KoreanVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "DOUBLE_PeterBadoingy",
            ModelName = "DOUBLE",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 7,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "KKANG 05",

                    PlateType = 57,
                },
                WheelType = 6,
                WindowTint = -1,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = -1,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = -1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = -1,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = -1,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = false,
        });

        //Triad
        TriadVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "ELEGY_PeterBadoingy_DLCDespawn",
            ModelName = "ELEGY",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 111,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "TRIAD-01",

                    PlateType = 53,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = true,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 9,
},
new VehicleMod() {
ID = 1,
Output = 2,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = 3,
},
new VehicleMod() {
ID = 4,
Output = 2,
},
new VehicleMod() {
ID = 5,
Output = 1,
},
new VehicleMod() {
ID = 6,
Output = 0,
},
new VehicleMod() {
ID = 7,
Output = 4,
},
new VehicleMod() {
ID = 8,
Output = 2,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = 0,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 19,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = 0,
},
new VehicleMod() {
ID = 27,
Output = 0,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = 0,
},
new VehicleMod() {
ID = 32,
Output = 5,
},
new VehicleMod() {
ID = 33,
Output = 2,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = 2,
},
new VehicleMod() {
ID = 40,
Output = 7,
},
new VehicleMod() {
ID = 41,
Output = 1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = 6,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = 1,
},
new VehicleMod() {
ID = 46,
Output = 1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 2,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        TriadVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "euros_PeterBadoingy_DLCDespawn",
            ModelName = "euros",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "TRIAD-02",

                    PlateType = 53,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 10,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = 0,
},
new VehicleMod() {
ID = 3,
Output = 2,
},
new VehicleMod() {
ID = 4,
Output = 1,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = 12,
},
new VehicleMod() {
ID = 8,
Output = 5,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = 2,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 27,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 12,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        TriadVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "futo2_PeterBadoingy_DLCDespawn",
            ModelName = "futo2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "TRIAD-03",

                    PlateType = 53,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = 1,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = 1,
},
new VehicleMod() {
ID = 3,
Output = 0,
},
new VehicleMod() {
ID = 4,
Output = 2,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = 1,
},
new VehicleMod() {
ID = 7,
Output = 9,
},
new VehicleMod() {
ID = 8,
Output = 0,
},
new VehicleMod() {
ID = 9,
Output = 1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 11,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = 0,
},
new VehicleMod() {
ID = 27,
Output = 0,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = 1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = 0,
},
new VehicleMod() {
ID = 40,
Output = 4,
},
new VehicleMod() {
ID = 41,
Output = 11,
},
new VehicleMod() {
ID = 42,
Output = 2,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = 0,
},
new VehicleMod() {
ID = 45,
Output = 0,
},
new VehicleMod() {
ID = 46,
Output = 2,
},
new VehicleMod() {
ID = 47,
Output = 5,
},
new VehicleMod() {
ID = 48,
Output = 1,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = true,
        });
        TriadVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "KURUMA_PeterBadoingy",
            ModelName = "KURUMA",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "TRIAD-04",

                    PlateType = 53,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = 0,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = 0,
},
new VehicleMod() {
ID = 4,
Output = 0,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = -1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = 16,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = 3,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = false,
        });
        TriadVehicles.Add(new DispatchableVehicle()
        {
            DebugName = "HAKUCHOU_PeterBadoingy",
            ModelName = "HAKUCHOU",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>()
            {
            },
            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>()
            {
            },
            VehicleExtras = new List<DispatchableVehicleExtra>()
            {
            },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 111,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 27,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "TRIAD-05",

                    PlateType = 53,
                },
                WheelType = 6,
                WindowTint = -1,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
new VehicleExtra() {
ID = 0,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 1,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 2,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 3,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 4,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 5,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 6,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 7,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 8,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 9,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 10,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 11,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 12,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 13,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 14,
IsTurnedOn = false,
},
new VehicleExtra() {
ID = 15,
IsTurnedOn = false,
},
},
                VehicleToggles = new List<VehicleToggle>() {
new VehicleToggle() {
ID = 17,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 18,
IsTurnedOn = true,
},
new VehicleToggle() {
ID = 19,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 20,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 21,
IsTurnedOn = false,
},
new VehicleToggle() {
ID = 22,
IsTurnedOn = false,
},
},
                VehicleMods = new List<VehicleMod>() {
new VehicleMod() {
ID = 0,
Output = -1,
},
new VehicleMod() {
ID = 1,
Output = -1,
},
new VehicleMod() {
ID = 2,
Output = -1,
},
new VehicleMod() {
ID = 3,
Output = -1,
},
new VehicleMod() {
ID = 4,
Output = -1,
},
new VehicleMod() {
ID = 5,
Output = -1,
},
new VehicleMod() {
ID = 6,
Output = -1,
},
new VehicleMod() {
ID = 7,
Output = -1,
},
new VehicleMod() {
ID = 8,
Output = -1,
},
new VehicleMod() {
ID = 9,
Output = -1,
},
new VehicleMod() {
ID = 10,
Output = -1,
},
new VehicleMod() {
ID = 11,
Output = -1,
},
new VehicleMod() {
ID = 12,
Output = -1,
},
new VehicleMod() {
ID = 13,
Output = -1,
},
new VehicleMod() {
ID = 14,
Output = -1,
},
new VehicleMod() {
ID = 15,
Output = -1,
},
new VehicleMod() {
ID = 16,
Output = -1,
},
new VehicleMod() {
ID = 23,
Output = -1,
},
new VehicleMod() {
ID = 24,
Output = -1,
},
new VehicleMod() {
ID = 25,
Output = -1,
},
new VehicleMod() {
ID = 26,
Output = -1,
},
new VehicleMod() {
ID = 27,
Output = -1,
},
new VehicleMod() {
ID = 28,
Output = -1,
},
new VehicleMod() {
ID = 29,
Output = -1,
},
new VehicleMod() {
ID = 30,
Output = -1,
},
new VehicleMod() {
ID = 31,
Output = -1,
},
new VehicleMod() {
ID = 32,
Output = -1,
},
new VehicleMod() {
ID = 33,
Output = -1,
},
new VehicleMod() {
ID = 34,
Output = -1,
},
new VehicleMod() {
ID = 35,
Output = -1,
},
new VehicleMod() {
ID = 36,
Output = -1,
},
new VehicleMod() {
ID = 37,
Output = -1,
},
new VehicleMod() {
ID = 38,
Output = -1,
},
new VehicleMod() {
ID = 39,
Output = -1,
},
new VehicleMod() {
ID = 40,
Output = -1,
},
new VehicleMod() {
ID = 41,
Output = -1,
},
new VehicleMod() {
ID = 42,
Output = -1,
},
new VehicleMod() {
ID = 43,
Output = -1,
},
new VehicleMod() {
ID = 44,
Output = -1,
},
new VehicleMod() {
ID = 45,
Output = -1,
},
new VehicleMod() {
ID = 46,
Output = -1,
},
new VehicleMod() {
ID = 47,
Output = -1,
},
new VehicleMod() {
ID = 48,
Output = -1,
},
new VehicleMod() {
ID = 49,
Output = -1,
},
new VehicleMod() {
ID = 50,
Output = -1,
},
},
            },
            RequiresDLC = false,
        });

        //Marabunta
        DispatchableVehicle MarabuntaCustom1 = new DispatchableVehicle()
        {
            DebugName = "faction2_PeterBadoingy_DLCDespawn",
            ModelName = "faction2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 70,
                SecondaryColor = 70,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 38,
                WheelColor = 90,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS MAR01",

                    PlateType = 50,
                },
                WheelType = 9,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = 5,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 4,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
                FuelLevel = 65f,
                DirtLevel = 0f,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle MarabuntaCustom2 = new DispatchableVehicle()
        {
            DebugName = "chino2_PeterBadoingy_DLCDespawn",
            ModelName = "chino2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 70,
                SecondaryColor = 2,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 120,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS MAR02",

                    PlateType = 50,
                },
                WheelType = 1,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 5,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 5,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 4,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 2,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 4,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
                FuelLevel = 65f,
                DirtLevel = 0f,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle MarabuntaCustom3 = new DispatchableVehicle()
        {
            DebugName = "chino2_PeterBadoingy_DLCDespawn",
            ModelName = "chino2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 2,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 70,
                SecondaryColor = 2,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 120,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS MAR03",

                    PlateType = 50,
                },
                WheelType = 1,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 5,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 5,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 4,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 2,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 7,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
                FuelLevel = 65f,
                DirtLevel = 0f,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle MarabuntaCustom4 = new DispatchableVehicle()
        {
            DebugName = "sabregt2_PeterBadoingy_DLCDespawn",
            ModelName = "sabregt2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 70,
                SecondaryColor = 70,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 88,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = 0,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS MAR04",

                    PlateType = 50,
                },
                WheelType = 1,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = -1,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 5,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 7,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = 1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 3,
            },
            new VehicleMod() {
              ID = 39,
                Output = 6,
            },
            new VehicleMod() {
              ID = 40,
                Output = 3,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 7,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
                FuelLevel = 65f,
                DirtLevel = 0f,
            },
            RequiresDLC = true,
        };
        DispatchableVehicle MarabuntaCustom5 = new DispatchableVehicle()
        {
            DebugName = "voodoo_PeterBadoingy_DLCDespawn",
            ModelName = "voodoo",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 70,
                SecondaryColor = 70,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 156,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS MAR05",

                    PlateType = 50,
                },
                WheelType = 9,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 1,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 31,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = 5,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 5,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 5,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 4,
            },
            new VehicleMod() {
              ID = 39,
                Output = 3,
            },
            new VehicleMod() {
              ID = 40,
                Output = 1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = 0,
            },
            new VehicleMod() {
              ID = 43,
                Output = 0,
            },
            new VehicleMod() {
              ID = 44,
                Output = 5,
            },
            new VehicleMod() {
              ID = 45,
                Output = 2,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 6,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
                FuelLevel = 65f,
                DirtLevel = 0f,
            },
            RequiresDLC = true,
        };

        MarabuntaVehicles.Add(MarabuntaCustom1);
        MarabuntaVehicles.Add(MarabuntaCustom2);
        MarabuntaVehicles.Add(MarabuntaCustom3);
        MarabuntaVehicles.Add(MarabuntaCustom4);
        MarabuntaVehicles.Add(MarabuntaCustom5);

        //Cartel
        DispatchableVehicle CartelCustom1 = new DispatchableVehicle()
        {
            DebugName = "patriot3_PeterBadoingy_DLCDespawn",
            ModelName = "patriot3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 0,
                SecondaryColor = 0,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS CAR01",

                    PlateType = 27,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 0,
            },
            new VehicleMod() {
              ID = 1,
                Output = 5,
            },
            new VehicleMod() {
              ID = 2,
                Output = 5,
            },
            new VehicleMod() {
              ID = 3,
                Output = 2,
            },
            new VehicleMod() {
              ID = 4,
                Output = 5,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = 8,
            },
            new VehicleMod() {
              ID = 7,
                Output = 1,
            },
            new VehicleMod() {
              ID = 8,
                Output = 1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 7,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 0,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = 2,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle CartelCustom2 = new DispatchableVehicle()
        {
            DebugName = "kamacho_PeterBadoingy_DLCDespawn",
            ModelName = "kamacho",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 0,
                SecondaryColor = 0,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS CAR02",

                    PlateType = 27,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 2,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 6,
            },
            new VehicleMod() {
              ID = 3,
                Output = 2,
            },
            new VehicleMod() {
              ID = 4,
                Output = 2,
            },
            new VehicleMod() {
              ID = 5,
                Output = 3,
            },
            new VehicleMod() {
              ID = 6,
                Output = 2,
            },
            new VehicleMod() {
              ID = 7,
                Output = 7,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 2,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle CartelCustom3 = new DispatchableVehicle()
        {
            DebugName = "hellion_PeterBadoingy_DLCDespawn",
            ModelName = "hellion",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 0,
                SecondaryColor = 0,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS CAR03",

                    PlateType = 27,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 11,
            },
            new VehicleMod() {
              ID = 2,
                Output = 2,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = -1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 5,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = 8,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 2,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 4,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle CartelCustom4 = new DispatchableVehicle()
        {
            DebugName = "cavalcade2_PeterBadoingy_DLCDespawn",
            ModelName = "cavalcade2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 0,
                SecondaryColor = 0,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS CAR04",

                    PlateType = 27,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = -1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 27,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle CartelCustom5 = new DispatchableVehicle()
        {
            DebugName = "granger2_PeterBadoingy_DLCDespawn",
            ModelName = "granger2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 0,
                SecondaryColor = 0,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS CAR05",

                    PlateType = 27,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 5,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = 1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = 1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 28,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };

        CartelVehicles.Add(CartelCustom1);
        CartelVehicles.Add(CartelCustom2);
        CartelVehicles.Add(CartelCustom3);
        CartelVehicles.Add(CartelCustom4);
        CartelVehicles.Add(CartelCustom5);

        //Ballas
        DispatchableVehicle BallasCustom1 = new DispatchableVehicle()
        {
            DebugName = "primo2_PeterBadoingy_DLCDespawn",
            ModelName = "primo2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 145,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 145,
                WheelColor = 145,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "BA-GS-01",

                    PlateType = 56,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 2,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 25,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = 6,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = 1,
            },
            new VehicleMod() {
              ID = 28,
                Output = 6,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = 4,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 1,
            },
            new VehicleMod() {
              ID = 34,
                Output = 2,
            },
            new VehicleMod() {
              ID = 35,
                Output = 13,
            },
            new VehicleMod() {
              ID = 36,
                Output = 3,
            },
            new VehicleMod() {
              ID = 37,
                Output = 6,
            },
            new VehicleMod() {
              ID = 38,
                Output = 3,
            },
            new VehicleMod() {
              ID = 39,
                Output = 5,
            },
            new VehicleMod() {
              ID = 40,
                Output = 3,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = 2,
            },
            new VehicleMod() {
              ID = 44,
                Output = 1,
            },
            new VehicleMod() {
              ID = 45,
                Output = 2,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle BallasCustom2 = new DispatchableVehicle()
        {
            DebugName = "virgo2_PeterBadoingy_DLCDespawn",
            ModelName = "virgo2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 145,
                SecondaryColor = 15,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 145,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = 0,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "BA-GS-02",

                    PlateType = 56,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = 0,
            },
            new VehicleMod() {
              ID = 6,
                Output = 1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 12,
            },
            new VehicleMod() {
              ID = 24,
                Output = 3,
            },
            new VehicleMod() {
              ID = 25,
                Output = 7,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = 6,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = 0,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = 15,
            },
            new VehicleMod() {
              ID = 34,
                Output = 0,
            },
            new VehicleMod() {
              ID = 35,
                Output = 20,
            },
            new VehicleMod() {
              ID = 36,
                Output = 4,
            },
            new VehicleMod() {
              ID = 37,
                Output = 4,
            },
            new VehicleMod() {
              ID = 38,
                Output = 3,
            },
            new VehicleMod() {
              ID = 39,
                Output = 4,
            },
            new VehicleMod() {
              ID = 40,
                Output = 1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = 1,
            },
            new VehicleMod() {
              ID = 44,
                Output = 0,
            },
            new VehicleMod() {
              ID = 45,
                Output = 3,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = 1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle BallasCustom3 = new DispatchableVehicle()
        {
            DebugName = "baller7_PeterBadoingy_DLCDespawn",
            ModelName = "baller7",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 145,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 74,
                WheelColor = 145,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "BA-GS-03",

                    PlateType = 56,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 1,
            },
            new VehicleMod() {
              ID = 1,
                Output = -1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 1,
            },
            new VehicleMod() {
              ID = 4,
                Output = 1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 2,
            },
            new VehicleMod() {
              ID = 8,
                Output = 0,
            },
            new VehicleMod() {
              ID = 9,
                Output = 0,
            },
            new VehicleMod() {
              ID = 10,
                Output = 2,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 25,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle BallasCustom4 = new DispatchableVehicle()
        {
            DebugName = "baller_PeterBadoingy",
            ModelName = "baller",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 145,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 74,
                WheelColor = 145,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "BA-GS-04",

                    PlateType = 56,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 25,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = false,
        };
        DispatchableVehicle BallasCustom5 = new DispatchableVehicle()
        {
            DebugName = "blazer4_PeterBadoingy_DLCDespawn",
            ModelName = "blazer4",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 1,
            AmbientSpawnChance = 10,
            WantedSpawnChance = 10,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 12,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 145,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 145,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "BA-GS-05",

                    PlateType = 56,
                },
                WheelType = 11,
                WindowTint = -1,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 5,
            },
            new VehicleMod() {
              ID = 2,
                Output = 1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 3,
            },
            new VehicleMod() {
              ID = 4,
                Output = 1,
            },
            new VehicleMod() {
              ID = 5,
                Output = 1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = 2,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 0,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 12,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };

        BallasVehicles.Add(BallasCustom1);
        BallasVehicles.Add(BallasCustom2);
        BallasVehicles.Add(BallasCustom3);
        BallasVehicles.Add(BallasCustom4);
        BallasVehicles.Add(BallasCustom5);

        //Armenians
        DispatchableVehicle ArmenianCustom1 = new DispatchableVehicle()
        {
            DebugName = "dubsta2_PeterBadoingy_DLCDespawn",
            ModelName = "dubsta2",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 147,
                SecondaryColor = 147,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS ARM01",

                    PlateType = 47,
                },
                WheelType = 11,
                WindowTint = 3,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = -1,
            },
            new VehicleMod() {
              ID = 4,
                Output = -1,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = -1,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 27,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle ArmenianCustom2 = new DispatchableVehicle()
        {
            DebugName = "schafter3_PeterBadoingy_DLCDespawn",
            ModelName = "schafter3",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 147,
                SecondaryColor = 147,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 4,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS ARM02",

                    PlateType = 47,
                },
                WheelType = 11,
                WindowTint = 2,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 2,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 3,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 26,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle ArmenianCustom3 = new DispatchableVehicle()
        {
            DebugName = "schafter4_PeterBadoingy_DLCDespawn",
            ModelName = "schafter4",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 147,
                SecondaryColor = 147,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS ARM03",

                    PlateType = 47,
                },
                WheelType = 11,
                WindowTint = 2,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 0,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 0,
            },
            new VehicleMod() {
              ID = 4,
                Output = 0,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = -1,
            },
            new VehicleMod() {
              ID = 7,
                Output = 0,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = -1,
            },
            new VehicleMod() {
              ID = 10,
                Output = -1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 27,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle ArmenianCustom4 = new DispatchableVehicle()
        {
            DebugName = "schwarzer_PeterBadoingy_DLCDespawn",
            ModelName = "schwarzer",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 147,
                SecondaryColor = 12,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS ARM04",

                    PlateType = 47,
                },
                WheelType = 11,
                WindowTint = 2,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = true,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = 0,
            },
            new VehicleMod() {
              ID = 1,
                Output = 0,
            },
            new VehicleMod() {
              ID = 2,
                Output = -1,
            },
            new VehicleMod() {
              ID = 3,
                Output = 2,
            },
            new VehicleMod() {
              ID = 4,
                Output = 4,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 2,
            },
            new VehicleMod() {
              ID = 7,
                Output = 11,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 3,
            },
            new VehicleMod() {
              ID = 10,
                Output = 3,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 17,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        DispatchableVehicle ArmenianCustom5 = new DispatchableVehicle()
        {
            DebugName = "schlagen_PeterBadoingy_DLCDespawn",
            ModelName = "schlagen",
            RequiredPedGroup = "",
            GroupName = "",
            MinOccupants = 1,
            MaxOccupants = 2,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            MinWantedLevelSpawn = 0,
            MaxWantedLevelSpawn = 6,
            ForceStayInSeats = new List<int>() { },
            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,
            RequiredLiveries = new List<int>() { },
            VehicleExtras = new List<DispatchableVehicleExtra>() { },
            RequiredVariation = new VehicleVariation()
            {
                PrimaryColor = 147,
                SecondaryColor = 147,
                IsPrimaryColorCustom = false,
                IsSecondaryColorCustom = false,
                PearlescentColor = 0,
                WheelColor = 0,
                Mod1PaintType = 7,
                Mod1Color = -1,
                Mod1PearlescentColor = -1,
                Mod2PaintType = 7,
                Mod2Color = -1,
                Livery = -1,
                Livery2 = -1,
                LicensePlate = new LSR.Vehicles.LicensePlate()
                {
                    PlateNumber = "LS ARM05",

                    PlateType = 47,
                },
                WheelType = 11,
                WindowTint = 2,
                HasCustomWheels = false,
                VehicleExtras = new List<VehicleExtra>() {
          new VehicleExtra() {
              ID = 0,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 1,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 2,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 3,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 4,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 5,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 6,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 7,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 8,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 9,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 10,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 11,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 12,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 13,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 14,
                IsTurnedOn = false,
            },
            new VehicleExtra() {
              ID = 15,
                IsTurnedOn = false,
            },
        },
                VehicleToggles = new List<VehicleToggle>() {
          new VehicleToggle() {
              ID = 17,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 18,
                IsTurnedOn = true,
            },
            new VehicleToggle() {
              ID = 19,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 20,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 21,
                IsTurnedOn = false,
            },
            new VehicleToggle() {
              ID = 22,
                IsTurnedOn = false,
            },
        },
                VehicleMods = new List<VehicleMod>() {
          new VehicleMod() {
              ID = 0,
                Output = -1,
            },
            new VehicleMod() {
              ID = 1,
                Output = 1,
            },
            new VehicleMod() {
              ID = 2,
                Output = 0,
            },
            new VehicleMod() {
              ID = 3,
                Output = 2,
            },
            new VehicleMod() {
              ID = 4,
                Output = 2,
            },
            new VehicleMod() {
              ID = 5,
                Output = -1,
            },
            new VehicleMod() {
              ID = 6,
                Output = 0,
            },
            new VehicleMod() {
              ID = 7,
                Output = 6,
            },
            new VehicleMod() {
              ID = 8,
                Output = -1,
            },
            new VehicleMod() {
              ID = 9,
                Output = 1,
            },
            new VehicleMod() {
              ID = 10,
                Output = 1,
            },
            new VehicleMod() {
              ID = 11,
                Output = -1,
            },
            new VehicleMod() {
              ID = 12,
                Output = -1,
            },
            new VehicleMod() {
              ID = 13,
                Output = -1,
            },
            new VehicleMod() {
              ID = 14,
                Output = -1,
            },
            new VehicleMod() {
              ID = 15,
                Output = -1,
            },
            new VehicleMod() {
              ID = 16,
                Output = -1,
            },
            new VehicleMod() {
              ID = 23,
                Output = 16,
            },
            new VehicleMod() {
              ID = 24,
                Output = -1,
            },
            new VehicleMod() {
              ID = 25,
                Output = -1,
            },
            new VehicleMod() {
              ID = 26,
                Output = -1,
            },
            new VehicleMod() {
              ID = 27,
                Output = -1,
            },
            new VehicleMod() {
              ID = 28,
                Output = -1,
            },
            new VehicleMod() {
              ID = 29,
                Output = -1,
            },
            new VehicleMod() {
              ID = 30,
                Output = -1,
            },
            new VehicleMod() {
              ID = 31,
                Output = -1,
            },
            new VehicleMod() {
              ID = 32,
                Output = -1,
            },
            new VehicleMod() {
              ID = 33,
                Output = -1,
            },
            new VehicleMod() {
              ID = 34,
                Output = -1,
            },
            new VehicleMod() {
              ID = 35,
                Output = -1,
            },
            new VehicleMod() {
              ID = 36,
                Output = -1,
            },
            new VehicleMod() {
              ID = 37,
                Output = -1,
            },
            new VehicleMod() {
              ID = 38,
                Output = -1,
            },
            new VehicleMod() {
              ID = 39,
                Output = -1,
            },
            new VehicleMod() {
              ID = 40,
                Output = -1,
            },
            new VehicleMod() {
              ID = 41,
                Output = -1,
            },
            new VehicleMod() {
              ID = 42,
                Output = -1,
            },
            new VehicleMod() {
              ID = 43,
                Output = -1,
            },
            new VehicleMod() {
              ID = 44,
                Output = -1,
            },
            new VehicleMod() {
              ID = 45,
                Output = -1,
            },
            new VehicleMod() {
              ID = 46,
                Output = -1,
            },
            new VehicleMod() {
              ID = 47,
                Output = -1,
            },
            new VehicleMod() {
              ID = 48,
                Output = -1,
            },
            new VehicleMod() {
              ID = 49,
                Output = -1,
            },
            new VehicleMod() {
              ID = 50,
                Output = -1,
            },
        },
            },
            RequiresDLC = true,
        };
        ArmeniaVehicles.Add(ArmenianCustom1);
        ArmeniaVehicles.Add(ArmenianCustom2);
        ArmeniaVehicles.Add(ArmenianCustom3);
        ArmeniaVehicles.Add(ArmenianCustom4);
        ArmeniaVehicles.Add(ArmenianCustom5);
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
        };

        //Federal
        List<DispatchableVehicle> CoastGuardVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> ParkRangerVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceGresley, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f, RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceGranger, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceBison, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 } },
        };
        List<DispatchableVehicle> FIBVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(BuffaloUnmarked, 30, 30){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(GrangerUnmarked, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(PoliceFugitive, 30,30){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(BuffaloUnmarked, 0, 70) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } }, };
        List<DispatchableVehicle> NOOSEVehicles_FEJ = new List<DispatchableVehicle>() {
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
        List<DispatchableVehicle> LSPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceStanier, 25,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 48,35) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> EastLSPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 35,35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        List<DispatchableVehicle> VWPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 30,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 }, },
            new DispatchableVehicle(PoliceFugitive, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        List<DispatchableVehicle> RHPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> DPPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> BCSOVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> LSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGranger, 50, 50) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 }, },
            new DispatchableVehicle(PoliceFugitive, 1, 1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 25, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGresley, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGranger, 20, 20)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceBison, 7, 7)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 }, },
            new DispatchableVehicle(PoliceFugitive, 2, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        List<DispatchableVehicle> DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 55, 55){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGranger, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        //Other State
        List<DispatchableVehicle> LSIAPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> LSPPVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
        List<DispatchableVehicle> SAHPVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 45, 45) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGresley, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGranger, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceBison, 5, 0) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
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

            new DispatchableVehicle(PoliceFugitive, 25,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> PrisonVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceTransporter, 0, 25),
            new DispatchableVehicle(PoliceStanier, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceBison, 5, 0) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceFugitive, 1, 0){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        List<DispatchableVehicle> NYSPVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,20){  RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },




            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceBison, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },  };
        List<DispatchableVehicle> LCPDVehicles_FEJ = new List<DispatchableVehicle>() {
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
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};

        List<DispatchableVehicle> NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 15,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
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
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};


        List<DispatchableVehicle> MarshalsServiceVehicles_FEJ = MarshalsServiceVehicles.Copy();//for now

        //Security
        List<DispatchableVehicle> MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>(){
            new DispatchableVehicle("dilettante2", 50, 50),
            new DispatchableVehicle(SecurityTorrence, 50, 50) {  RequiredLiveries = new List<int>() { 1 } },};
        List<DispatchableVehicle> BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>(){
            new DispatchableVehicle(SecurityTorrence, 100, 100){  RequiredLiveries = new List<int>() { 2,3 } },};
        List<DispatchableVehicle> GroupSechsVehicles_FEJ = new List<DispatchableVehicle>(){
            new DispatchableVehicle(SecurityTorrence, 100, 100){  RequiredLiveries = new List<int>() { 0 } },};
        List<DispatchableVehicle> SecuroservVehicles_FEJ = new List<DispatchableVehicle>(){
            new DispatchableVehicle(SecurityTorrence, 100, 100){  RequiredLiveries = new List<int>() { 4 } },};

        List<DispatchableVehicle> DowntownTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 92, 92){ RequiredLiveries = new List<int>() { 0 } },
            TaxiBroadWay,
            TaxiEudora,
        };
        List<DispatchableVehicle> PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 100, 100){ RequiredLiveries = new List<int>() { 1 } },
        };
        List<DispatchableVehicle> HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 100, 100){ RequiredLiveries = new List<int>() { 2 } },
        };
        List<DispatchableVehicle> ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 100, 100){ RequiredLiveries = new List<int>() { 3 } },
        };
        List<DispatchableVehicle> SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 100, 100){ RequiredLiveries = new List<int>() { 4 } },
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


