using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

public class DispatchableVehicles_FEJ
{
    private DispatchableVehicles DispatchableVehicles;
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
    private string WashingtonUnmarked = "blista3";

    private string PoliceStanierOld = "stalion2";
    private string ServiceStanierOld = "gauntlet2";
    private string PoliceLandstalkerXL = "dominator2";

    private string PoliceSanchez = "sovereign";
    private int PIBlankLivID = 11;

    private string PoliceTerminus = "marshall";

    private string PoliceBoxville = "boxville5";
    private string PoliceVindicator = "deathbike";
    private string TaxiMinivan = "dukes2";

    private string SecurityStanier = "massacro2";
    private string PoliceBuffaloSTX = "jester2";
    private string PoliceCaracara = "ratloader";

    private List<int> DefaultOptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 };

    public DispatchableVehicles_FEJ(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }

    public List<DispatchableVehicle> UnmarkedVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> ArmyVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USMCVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USAFVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> CoastGuardVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> LSLifeguardVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> ParkRangerVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SADFWParkRangersVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USNPSParkRangersVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSDPRParkRangersVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> PoliceHeliVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SheriffHeliVehicles_FEJ { get; private set; }

    public List<DispatchableVehicle> FIBVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> NOOSEVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> EastLSPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> VWPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> RHPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> DPPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> BCSOVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSSDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> MajesticLSSDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> DavisLSSDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSIAPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSPPVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SAHPVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> PrisonVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> NYSPVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LCPDVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> BorderPatrolVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> NOOSEPIAVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> NOOSESEPVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> MarshalsServiceVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> MerryweatherPatrolVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> BobcatSecurityVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> GroupSechsVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SecuroservVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> DowntownTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> PurpleTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> HellTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> ShitiTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> SunderedTaxiVehicles { get; private set; }
    public void DefaultConfig()
    {
        LocalPolice();
        LocalSheriff();
        StatePolice();
        FederalPolice();
        OtherPolice();
        Security();
        Taxis();
        /*
         * 
         Extras
            Washingtion - 1 = Ram Bar, 2 = Emblem, 3 = Searchlight, 11/12 = Vanilla Cupholder Stuff, 13 = Radio (Non functional, needs to be changed to lower number)
            Police Stanier - 1 = LED siren, 2 = Halogen Siren, 3 = Ram Bar, 4 = Searchlight, 5 = antenna, 9 = Partition, 11 = Vanilla Cupholder, 12 = Radio
            Service Stanier - 1 = Taxi Medallion/Badge, 2 = Antenna, 3 = ram bar, 4 = searchlight, 5-9 = vanilla taxi, 10 = Vanilla Cupholder, 11 = partition, 12 = radio
            Police Transporter - 1 = LED siren, 12 = Cargo Props
            Buffalo S - 1 = LED siren, 2 = Ram Bar
            Dilettante Service = 5-9 = vanilla taxi, Extra 12 = Partiton
         * 
         * 
         * 
         * 
         * 
         * */
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            new DispatchableVehicle(StanierUnmarked, 50, 50),
            Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_Washington(30,30,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceStanierOld(10,10,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(10,10,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(BuffaloUnmarked, 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(GrangerUnmarked, 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            Create_PoliceBuffaloS(50, 50, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(25,25,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(25,25,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(10,10,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(10,10,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            

        };
        LSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_Washington(2, 2, -1, true, true, -1, 0, 3,"",""),
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),      
            Create_PoliceMerit(2,2,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceStanier, 25,20){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,1,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,11,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(48,35,1,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(2,2,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(2,2,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(48,35,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(10,10,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceBuffaloSTX(25,25,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(5,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(25, 20, 1, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            new DispatchableVehicle(PoliceGranger, 15, 12){ CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            Create_PoliceGauntlet(5,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTerminus(1,1,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            Create_PoliceBoxville(1,0,0,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),
            Create_PoliceBoxville(0,5,0,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),

            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        EastLSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 35,35){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,3,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_PoliceMerit(5,5,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(5, 5, 3, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(10,10,3,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceBison(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceTerminus(1,1,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            Create_PoliceBuffaloSTX(15,15,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(5,5,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),

            Create_PoliceBoxville(1,0,0,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),
            Create_PoliceBoxville(0,5,0,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),

            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        VWPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 30,25){ RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,2,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(2,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 20, 20){RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(25, 25, 2, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(50,50,2,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(50,50,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredLiveries = new List<int>() { 2 } },
            Create_PoliceBison(10,10,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceTerminus(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            Create_PoliceBuffaloSTX(20,20,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),

            Create_PoliceBoxville(1,0,0,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),
            Create_PoliceBoxville(0,5,0,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),
            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        RHPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,5,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(2,2,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 25, 15){RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(50, 50, 5, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(25,25,5,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(5,5,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 15, 15){CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 5 } },
            Create_PoliceFugitive(20,15,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
        DPPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,6,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(2,2,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(20, 20, 6, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(50,50,6,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(50,50,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 15, 15){RequiredLiveries = new List<int>() { 6 } },
            Create_PoliceBison(15,15,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(25,25,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(15,15,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(15,10,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceTerminus(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
        NYSPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(20,20,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceGresley(10,10,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredLiveries = new List<int>() { 16 } },
            //new DispatchableVehicle(PoliceBison, 25, 25){ RequiredLiveries = new List<int>() { 16 } },  
            Create_PoliceBison(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LCPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(25,25,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(48,35,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            new DispatchableVehicle(GrangerUnmarked, 1,1),

            new DispatchableVehicle(PoliceBike, 15, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 5 } },
        };

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

    }
    private void LocalSheriff()
    {
        //Sheriff TEST
        BCSOVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,0,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 20){ RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,0,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(20,20,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(10,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(7, 7, 0, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(10,10,0,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 20, 20) { RequiredLiveries = new List<int>() {0 } },
            Create_PoliceBison(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(1,1,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(4,4,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTerminus(3,3,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            Create_PoliceBuffaloSTX(25,25,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(35,35,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceSanchez(1,0,3,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            new DispatchableVehicle(PoliceBike, 10, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 2 } },
            Create_PoliceVindicator(15,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),

            Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            Create_PoliceTransporter(0,35,0,false,100,false,true,134,3,-1,3,4,""),

            new DispatchableVehicle("polmav", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter", RequiredLiveries = new List<int>() { 10 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter",RequiredLiveries = new List<int>() { 5 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },

        };
        BCSOVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20, 15){  RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,7,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(15, 15, 7, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(15,15,7,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(10,10,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 25, 25) { CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() {7 } },
            Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(2,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            Create_PoliceBuffaloSTX(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceTerminus(3,3,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

            Create_PoliceBoxville(1,0,5,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),
            Create_PoliceBoxville(0,5,5,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),
            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        LSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20, 15) { RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,8,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(5,5,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(25, 25, 8, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(25,25,8,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 50, 50) { RequiredLiveries = new List<int>() { 8 } },
            Create_PoliceBison(10,10,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceBuffaloSTX(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceTerminus(3,3,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),
            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        MajesticLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 15){ RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,9,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(10,10,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(15, 15, 9, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(15,15,9,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 20, 20)  { RequiredLiveries = new List<int>() { 9 } },
            Create_PoliceBison(7,7,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceBuffaloSTX(25,25,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(25,25,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceTerminus(3,3,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        VWHillsLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 55, 55){ RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,10,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(15,15,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(10, 10, 10, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(15,15,10,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(10,10,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 15, 15)  { RequiredLiveries = new List<int>() { 10 }, },
            Create_PoliceBison(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(25,25,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceCaracara(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceVindicator(15,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceTerminus(3,3,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",10),

            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        DavisLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
    }
    private void StatePolice()
    {
        //Other State
        LSIAPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,12,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,21,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 5, 5) { RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(15, 15, 11, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(15,15,12,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(10,10,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 5, 5)  { RequiredLiveries = new List<int>() { 12 } },
            Create_PoliceBison(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(2,2,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        LSPPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,3,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,13,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_Washington(3,3,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,22,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(5,5,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 5, 5) { RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(5, 5, 12, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(10,10,13,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(10,10,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(5,5,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 10, 10){ CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 13 } },
            Create_PoliceFugitive(2,2,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 10, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 4 } },
            Create_PoliceVindicator(15,10,4,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),

            Create_PoliceTransporter(0,35,3,false,100,false,true,134,3,-1,3,4,""),
        };
        //State
        SAHPVehicles_FEJ = new List<DispatchableVehicle>()
        {    
            Create_PoliceStanierOld(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceStanierOld(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_Washington(3,3,-1,true,true,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(1,1,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceBuffaloS(5, 5, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"","StandardSAHP"),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            new DispatchableVehicle(PoliceStanier, 10,10){ GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,4,false,PoliceVehicleType.Marked,134,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(15,15,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceMerit(5,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceMerit(5,2,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            Create_PoliceBuffaloS(45,55,4,false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            Create_PoliceBuffaloS(20,20,4,false, PoliceVehicleType.SlicktopMarked, 134, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceGresley(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceGresley(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            new DispatchableVehicle(PoliceGranger, 5, 5) { GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            Create_PoliceBison(5,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceBison(5,2,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceGauntlet(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles.GauntletUndercoverSAHP,

            Create_PoliceBuffaloSTX(35,35,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(35,35,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTX(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },
            
            new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
            new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },


            Create_PoliceSanchez(1,0,2,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            Create_PoliceVindicator(45,20,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
        };
        PrisonVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 25) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(5,2,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceGresley(25,25,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredLiveries = new List<int>() { 14 } },
            //new DispatchableVehicle(PoliceBison, 5, 0) { RequiredLiveries = new List<int>() { 14 } },
            Create_PoliceBison(5,0,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
    }
    private void FederalPolice()
    {
        ArmyVehicles_FEJ = new List<DispatchableVehicle>() 
        {
            //General
            new DispatchableVehicle("crusader", 25,10) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            new DispatchableVehicle("barracks", 25,5) { MaxRandomDirtLevel = 15.0f,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            //new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            //new DispatchableVehicle("insurgent3", 5,50) { ForceStayInSeats = new List<int>(){ 7 },FirstPassengerIndex = 7, MaxRandomDirtLevel = 15.0f, MinOccupants = 2,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryArmedHumvee(5,50,-1,false,PoliceVehicleType.Marked,-1,6,10,2,4,"",""),


            //Heavy
            new DispatchableVehicle("rhino", 1, 15) {  MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 1,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            //new DispatchableVehicle("apc", 0,25) { MaxRandomDirtLevel = 15.0f,ForceStayInSeats = new List<int>() { -1 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },

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
            //new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            //new DispatchableVehicle("insurgent3", 5,50) { ForceStayInSeats = new List<int>(){ 7 },FirstPassengerIndex = 7, MaxRandomDirtLevel = 15.0f, MinOccupants = 2,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
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
            //new DispatchableVehicle("squaddie", 50,50) { MaxRandomDirtLevel = 15.0f, MinOccupants = 1,MaxOccupants = 3,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
            Create_MilitaryUnarmedHumvee(50,50,-1,false,PoliceVehicleType.Marked,-1,6,10,1,4,"",""),
            //new DispatchableVehicle("insurgent3", 5,50) { ForceStayInSeats = new List<int>(){ 7 },FirstPassengerIndex = 7, MaxRandomDirtLevel = 15.0f, MinOccupants = 2,MaxOccupants = 4,MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10 },
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

        LSLifeguardVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50),
            new DispatchableVehicle("freecrawler",5,5) { RequiredVariation = new VehicleVariation() { VehicleMods = new List<VehicleMod>() {new VehicleMod(48, 7) } } },
            new DispatchableVehicle("seashark2", 100, 100) { RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 6 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
            new DispatchableVehicle("polmav",2,5) { RequiredLiveries = new List<int>() { 5 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
        }; 

        ParkRangerVehicles_FEJ = new List<DispatchableVehicle>()//San Andreas State Parks
        {
            Create_PoliceTerminus(20,20,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            Create_PoliceTerminus(20,20,16,false,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"","",20),
            Create_PoliceLandstalkerXL(40,40,29,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceSanchez(10,10,8,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceCaracara(25,25,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        USNPSParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceGresley(25,25,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 40, 40) { RequiredLiveries = new List<int>() { 20 } },
            Create_PoliceBison(40,40,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",5),
            Create_PoliceTerminus(20,20,13,false,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"","",5),
            Create_PoliceLandstalkerXL(40,40,26,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceSanchez(10,10,5,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceCaracara(25,25,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };
        SADFWParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"","",20),
            Create_PoliceTerminus(20,20,14,false,PoliceVehicleType.Unmarked,51,-1,-1,-1,-1,"","",20),
            Create_PoliceLandstalkerXL(40,40,27,false,PoliceVehicleType.MarkedWithColor,51,-1,-1,-1,-1,"",""),
            Create_PoliceSanchez(10,10,6,false,PoliceVehicleType.Marked,51,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceCaracara(25,25,14,false,PoliceVehicleType.Marked,51,-1,-1,-1,-1,"",""),

        };
        LSDPRParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            Create_PoliceTerminus(20,20,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",20),
            Create_PoliceLandstalkerXL(40,40,28,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceSanchez(10,10,7,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            Create_PoliceCaracara(25,25,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
        };

        ParkRangerVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        FIBVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(10,10,7,false,0,true,true,1,0,4,-1,-1,""),
            Create_Washington(10,10,-1,false,true,1,0,4,"",""),
            new DispatchableVehicle(StanierUnmarked,10,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle(BuffaloUnmarked,10,10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle(GrangerUnmarked,10,10) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },
            Create_PoliceBuffaloS(10,10,16,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1, "", ""),
            Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            Create_PoliceInterceptor(5,5,PIBlankLivID,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceInterceptor(5,5,PIBlankLivID,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            Create_PoliceStanierOld(5,5,11,false,PoliceVehicleType.Detective,1,0,4,"",""),
            Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
            Create_PoliceTerminus(5,5,12,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"","",20),
            Create_PoliceBoxville(1,0,4,false,PoliceVehicleType.Unmarked,1,-1,-1,-1,-1,"",""),

            Create_PoliceGresley(5,5,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceMerit(2,2,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceMerit(2,2,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceBuffaloSTX(5,5,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceCaracara(1,1,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceCaracara(1,1,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            new DispatchableVehicle(GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(BuffaloUnmarked, 0, 20) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },

            new DispatchableVehicle("polmav", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },


            Create_PoliceTransporter(0,35,7,false,0,true,true,1,5,5,3,4,"FIBHET"),
            Create_PoliceLandstalkerXL(0,20,-1,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceBuffaloS(0,35,16,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET", ""),
            Create_PoliceInterceptor(15,15,PIBlankLivID,true,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),    
            Create_PoliceBoxville(0,5,4,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceGresley(0,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceBuffaloSTX(0,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },


        };
        FIBVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 2.0f);
        BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,6,false,50,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,19,false,PoliceVehicleType.Marked,134,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(20, 20, 15, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceGresley(40,40,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 40, 40){ RequiredLiveries = new List<int>() { 19 } },
            Create_PoliceMerit(10,10,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(1,1,16,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            Create_PoliceBison(35,35,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(5,5,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,25,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceBoxville(3,3,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTerminus(3,3,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",30),

            Create_PoliceTransporter(0,35,6,false,50,false,true,134,4,5,3,4,""),
            Create_PoliceCaracara(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        BorderPatrolVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,4,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 15,10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,17,false,PoliceVehicleType.Marked,134,0,3,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,0,3,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,0,3,"",""),
            Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
            Create_PoliceFugitive(5,5,14,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 15, 15){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(35, 35, 13, false, PoliceVehicleType.Marked, 134, 0, 3, -1, -1, "", ""),
            Create_PoliceInterceptor(70,70,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceGresley(70,70,17,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            Create_PoliceMerit(10,10,19,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PoliceBison(10,10,17,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PoliceGauntlet(1,1,18,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            Create_PoliceTerminus(1,1,10,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"","",10),
            Create_PoliceBoxville(1,0,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),




            new DispatchableVehicle(PoliceStanier, 15,10){ MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            Create_PoliceBuffaloS(0, 40, 13, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
            Create_PoliceInterceptor(0,50,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            Create_PoliceGresley(0,40,17,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            Create_PoliceLandstalkerXL(0,15,23,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            Create_PoliceTransporter(0,35,4,false,50,false,true,134,4,5,3,4,""),
            Create_PoliceFugitive(0,15,14,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            Create_PoliceBoxville(0,5,1,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,5,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 15,15){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,18,false,PoliceVehicleType.Marked,134,0,3,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,0,3,"",""),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,0,3,"",""),
            Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
            Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 5, 5){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(70,70,16,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceGresley(30,30,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            Create_PoliceMerit(5,5,19,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PoliceBison(5,5,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PoliceGauntlet(1,1,17,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            Create_PoliceTerminus(1,1,11,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"","",10),
            Create_PoliceBoxville(1,0,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceCaracara(5,5,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),


            new DispatchableVehicle(PoliceStanier, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { CaninePossibleSeats = new List<int>{ 1,2 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 5) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            Create_PoliceBuffaloS(25, 20, 14, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
            Create_PoliceInterceptor(0,50,16,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            Create_PoliceGresley(0,40,18,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            Create_PoliceTransporter(0,35,5,false,50,false,true,134,4,-1,3,4,""),
            Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            Create_PoliceBoxville(0,5,2,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),

            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
    }
    private void OtherPolice()
    {
        MarshalsServiceVehicles_FEJ = DispatchableVehicles.MarshalsServiceVehicles.Copy();//for now
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBuffaloS(25, 25, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));  
        MarshalsServiceVehicles_FEJ.Add(Create_Washington(25, 25, -1, true, true, -1, -1, -1, "", ""));//  new DispatchableVehicle(WashingtonUnmarked, 25, 25) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, true, 40) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceStanierOld(20, 20, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, "", ""));// new DispatchableVehicle(PoliceStanierOld, 20, 20) { RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(3, true, 50), new DispatchableVehicleExtra(4, true, 50), new DispatchableVehicleExtra(5, true, 50), new DispatchableVehicleExtra(9, true, 50), }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceInterceptor(15, 15, PIBlankLivID, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceLandstalkerXL(10, 10, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceTerminus(5, 5, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "",20));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceMerit(5, 5, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceGresley(15, 15, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBison(10, 10, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceCaracara(10, 10, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
    }
    private void Security()
    {
        MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,5,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(35,35,6,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecurityMW,
            DispatchableVehicles.AsteropeSecurityMW,
            Create_ServiceStanierOld(20,20,6,false,ServiceVehicleType.Security,134,-1,-1),
            Create_SecurityStanier(20,20,0,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,9,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecurityBobCat,
            DispatchableVehicles.AsteropeSecurityBobCat,
            Create_ServiceStanierOld(20,20,9,false,ServiceVehicleType.Security,134,-1,-1),
            Create_SecurityStanier(20,20,3,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecurityG6,
            DispatchableVehicles.AsteropeSecurityG6,
            Create_ServiceStanierOld(20,20,8,false,ServiceVehicleType.Security,134,-1,-1),
            Create_SecurityStanier(20,20,2,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecuritySECURO,
            DispatchableVehicles.AsteropeSecuritySECURO,
            Create_ServiceStanierOld(20,20,7,false,ServiceVehicleType.Security,134,-1,-1),
            Create_SecurityStanier(20,20,1,false,ServiceVehicleType.Security,-1,-1,-1),
        };
    }
    private void Taxis()
    {
        DowntownTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 0 } },
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),

            DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles.TaxiEudora,
        };
        PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 1 } },
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };
        HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 2 } },
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };
        ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 3 } },
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi1,0,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi2,0,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi3,0,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi4,0,-1,-1),

            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi1,0,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi2,0,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi3,0,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi4,0,-1,-1),

            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };
        SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 4 } },
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };
    }
    private enum PoliceVehicleType
    {
        Marked = 0,
        Unmarked = 1,
        Detective = 2,
        SlicktopMarked = 3,
        MarkedWithColor = 4,
    }
    private enum ServiceVehicleType
    {
        Taxi1 = 0,
        Taxi2 = 1,
        Taxi3 = 2,
        Taxi4 = 3,
        Security = 4,
    }
    private DispatchableVehicle Create_SecurityStanier(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceStanierOldType, int requiredColor, int minWantedLevel, int maxWantedLevel)
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


    private DispatchableVehicle Create_PoliceCaracara(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloSTX, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 3 - bar, 4 - searchlight,5 - spoiler, 6- antenna, 9 - divider, 12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(3, true, 65),
                new DispatchableVehicleExtra(4, true, 65),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
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
                new DispatchableVehicleExtra(5, false, 70),
                new DispatchableVehicleExtra(6, true, 45),
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
                new DispatchableVehicleExtra(5, false, 70),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    private DispatchableVehicle Create_PoliceVindicator(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }

    private DispatchableVehicle Create_PoliceMerit(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
        if (policeVehicleType == PoliceVehicleType.MarkedWithColor)
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
    private DispatchableVehicle Create_PoliceBison(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_MilitaryUnarmedHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_MilitaryArmedHumvee(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceBoxville(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceBicycle(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int bicyclePercent)
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
    private DispatchableVehicle Create_PoliceSanchez(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
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
    private DispatchableVehicle Create_PoliceTerminus(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int offroadAdditionalPercent)
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
    private DispatchableVehicle Create_PoliceBuffaloS(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloS, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if(policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() 
            { 
                new DispatchableVehicleExtra(1, true, 100), 
                new DispatchableVehicleExtra(2, true, 80),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 80),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked || policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 30),
            };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    private DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceTransporter(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, int sirenPercent, bool removeSiren, bool addInteriorExtra, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup)
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
    private DispatchableVehicle Create_PoliceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
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
    private DispatchableVehicle Create_Washington(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, bool addExtras, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
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
    private DispatchableVehicle Create_PoliceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string groupName, string requiredPedGroup)
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
    private DispatchableVehicle Create_PoliceGauntlet(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {

        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {

        }
        SetDefault(toReturn,useOptionalColors,-1,minWantedLevel,maxWantedLevel,minOccupants,maxOccupants,requiredPedGroup,groupName);
        return toReturn;
    }
    private DispatchableVehicle Create_ServiceDilettante(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType ServiceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
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
    private DispatchableVehicle Create_ServiceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceStanierOldType, int requiredColor, int minWantedLevel, int maxWantedLevel)
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
    private DispatchableVehicle Create_ServiceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
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
    private DispatchableVehicle Create_TaxiMinivan(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
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

