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


    private int PIBlankLivID = 11;

    private List<int> DefaultOptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 };

    public DispatchableVehicles_FEJ(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }

    public List<DispatchableVehicle> UnmarkedVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> CoastGuardVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> ParkRangerVehicles_FEJ { get; private set; }
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
            Create_Washington(30,30,-1,true,true,-1,-1,-1),
            Create_PoliceStanierOld(10,10,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(10,10,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(15,15,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(BuffaloUnmarked, 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(GrangerUnmarked, 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            //new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            Create_PoliceBuffaloS(50, 50, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceInterceptor(25,25,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(25,25,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


        };
        LSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_Washington(2, 2, -1, true, true, -1, 0, 3),
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceStanier, 25,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,1,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,11,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(48,35,1,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(2,2,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(2,2,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            //new DispatchableVehicle(PoliceTorrence, 48,35) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2,true,80) } },
            Create_PoliceBuffaloS(25, 20, 1, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            Create_PoliceGauntlet(5,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 }, },
            
            new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            
            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        EastLSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 35,35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,3,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(5, 5, 3, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            //new DispatchableVehicle(PoliceTorrence, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceInterceptor(10,10,3,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),


            new DispatchableVehicle(PoliceGresley, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 }, },     

            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            
            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        VWPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 30,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,2,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 2,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(25, 25, 2, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            //new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            Create_PoliceInterceptor(50,50,2,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 }, },
            Create_PoliceFugitive(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            
            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
        };
        RHPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,5,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(50, 50, 5, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            Create_PoliceInterceptor(25,25,5,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle(PoliceBison, 5, 5){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 } },
            Create_PoliceGauntlet(5,5,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 5 } },
            Create_PoliceFugitive(20,15,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        DPPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,6,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(20, 20, 6, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            Create_PoliceInterceptor(50,50,6,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceGranger, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle(PoliceBison, 15, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 } },
            Create_PoliceGauntlet(3,3,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(15,10,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,20){  RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceTorrence, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceBison, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },  
        };
        LCPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            //new DispatchableVehicle(PoliceTorrence, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            new DispatchableVehicle(GrangerUnmarked, 1,1),

            new DispatchableVehicle(PoliceBike, 15, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 5 } },
        };
    }
    private void LocalSheriff()
    {
        //Sheriff
        BCSOVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,0,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,0,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 10, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(10, 10, 0, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            Create_PoliceInterceptor(10,10,0,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 20, 20) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle(PoliceGranger, 30, 30) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle(PoliceBison, 15, 15)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 0 }, },
            Create_PoliceGauntlet(2,2,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 10, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 2 } },

            Create_PoliceTransporter(0,35,0,false,100,false,true,134,3,-1,3,4,""),
        };
        LSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,7,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(15, 15, 7, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            //new DispatchableVehicle(PoliceTorrence, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 } },
            Create_PoliceInterceptor(15,15,7,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 }, },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() {7 } },
            Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGauntlet(2,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },

            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,8,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(25, 25, 8, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            Create_PoliceInterceptor(25,25,8,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGranger, 50, 50) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 }, },
            Create_PoliceFugitive(2,2,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },

            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,9,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,18,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(15, 15, 9, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            Create_PoliceInterceptor(15,15,9,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGranger, 20, 20)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceBison, 7, 7)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 }, },
            Create_PoliceFugitive(2,2,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            
            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
        DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 55, 55){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,10,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(15,15,17,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(10, 10, 10, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            Create_PoliceInterceptor(15,15,10,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGranger, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            Create_PoliceFugitive(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },

            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
        };
    }
    private void StatePolice()
    {
        //Other State
        LSIAPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,12,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(1,1,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(5,5,21,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(15, 15, 11, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            Create_PoliceInterceptor(15,15,12,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceGranger, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 } },
            Create_PoliceGauntlet(2,2,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        LSPPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,3,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,13,false,PoliceVehicleType.Marked,134,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1),
            Create_Washington(3,3,-1,true,true,-1,-1,-1),
            Create_PoliceLandstalkerXL(5,5,22,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(5, 5, 12, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            Create_PoliceInterceptor(10,10,13,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 } },
            Create_PoliceGauntlet(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGranger, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 13 } },
            Create_PoliceFugitive(2,2,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceBike, 10, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 4 } },

            Create_PoliceTransporter(0,35,3,false,100,false,true,134,3,-1,3,4,""),
        };
        //State
        SAHPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,4,false,PoliceVehicleType.Marked,134,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_Washington(3,3,-1,true,true,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(15,15,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceLandstalkerXL(2,2,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            new DispatchableVehicle(PoliceMerit, 5, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            //new DispatchableVehicle(PoliceBuffaloS, 45, 55) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },      
            //buffalos unmarked too
            //new DispatchableVehicle(PoliceBuffaloS, 10, 0) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            Create_PoliceBuffaloS(45, 55, 4, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            Create_PoliceBuffaloS(20, 20, 4, false, PoliceVehicleType.SlicktopMarked, 111, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            Create_PoliceBuffaloS(10, 0, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "StandardSAHP", "StandardSAHP"),
            //new DispatchableVehicle(PoliceTorrence, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },     
            Create_PoliceInterceptor(20,20,4,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceInterceptor(20,20,4,false,PoliceVehicleType.SlicktopMarked,111,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceInterceptor(1,1,PIBlankLivID,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"","StandardSAHP"),
            new DispatchableVehicle(PoliceGresley, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGranger, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceBison, 5, 1) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            DispatchableVehicles.GauntletUndercoverSAHP,
            Create_PoliceGauntlet(1,1,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
        };
        PrisonVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceTorrence, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            Create_PoliceInterceptor(25,25,14,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceBison, 5, 0) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
        };
    }
    private void FederalPolice()
    {
        CoastGuardVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        ParkRangerVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceGresley, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f, RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceGranger, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 20 } },
            new DispatchableVehicle(PoliceBison, 40, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 } },
        };


        FIBVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(5,10,7,false,0,true,true,1,0,3,-1,-1,""),
            Create_Washington(5, 10, -1, false, true,1,0,3),
            new DispatchableVehicle(StanierUnmarked, 5,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, },
            new DispatchableVehicle(BuffaloUnmarked, 10, 10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            //new DispatchableVehicle(PoliceBuffaloS, 30, 30){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) }, },
            Create_PoliceBuffaloS(30, 30, 16, false, PoliceVehicleType.Unmarked, 0, -1, 3, -1, -1, "", ""),
            new DispatchableVehicle(GrangerUnmarked, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            Create_PoliceFugitive(15,15,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceFugitive(15,15,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),
            Create_PoliceInterceptor(15,15,PIBlankLivID,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceInterceptor(15,15,PIBlankLivID,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),
            Create_PoliceStanierOld(5,5,11,false,PoliceVehicleType.Detective,1,0,3),
            Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,-1,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            new DispatchableVehicle(GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(BuffaloUnmarked, 0, 20) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            //new DispatchableVehicle(PoliceBuffaloS, 0, 35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) }, },
            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },     
            Create_PoliceTransporter(0,35,7,false,0,true,true,1,5,5,3,4,"FIBHET"),
            Create_PoliceLandstalkerXL(0,20,-1,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceBuffaloS(0, 35, 16, false, PoliceVehicleType.Unmarked, 0, 5, 5, 3, 4, "FIBHET", ""),
            Create_PoliceInterceptor(15,15,PIBlankLivID,true,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
        };
        NOOSEVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 1,1){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            //new DispatchableVehicle(PoliceTorrence, 70, 70){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGresley, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 }, },
            //new DispatchableVehicle(PoliceFugitive, 1,1){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },

            new DispatchableVehicle(PoliceBuffalo, 35, 35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, CaninePossibleSeats = new List<int>() { 1,2 } },
            new DispatchableVehicle(PoliceBuffalo, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
           // new DispatchableVehicle(PoliceTorrence, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 11 }, },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };

        BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,6,false,50,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,19,false,PoliceVehicleType.Marked,134,-1,-1),
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(20, 20, 15, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            Create_PoliceInterceptor(15,15,14,false,PoliceVehicleType.Marked,111,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 40, 40){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            new DispatchableVehicle(PoliceGranger, 40, 40){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
            new DispatchableVehicle(PoliceMerit, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f, RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceGauntlet(1,1,16,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceBison, 35, 35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 }, },
            Create_PoliceFugitive(5,5,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,25,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTransporter(0,35,6,false,50,false,true,134,4,5,3,4,""),
        };
        NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,4,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,17,false,PoliceVehicleType.Marked,134,0,3),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,0,3),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,0,3),
            Create_Washington(2,2,-1,true,true,-1,0,3),
            Create_PoliceFugitive(5,5,14,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PoliceBuffaloS(35, 35, 13, false, PoliceVehicleType.Marked, 111, 0, 3, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceBuffaloS, 35, 35) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceInterceptor(70,70,15,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            //new DispatchableVehicle(PoliceTorrence, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGresley, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceMerit, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBison, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
            Create_PoliceGauntlet(1,1,18,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),

            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            Create_PoliceBuffaloS(0, 40, 13, false, PoliceVehicleType.Marked, 111, 4, 5, 3, 4, "", ""),
            //new DispatchableVehicle(PoliceBuffaloS, 0, 40) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            //new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            Create_PoliceInterceptor(0,50,15,false,PoliceVehicleType.Marked,111,4,5,3,4,"",""),
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            Create_PoliceLandstalkerXL(0,15,23,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            Create_PoliceTransporter(0,35,4,false,50,false,true,134,4,5,3,4,""),
            Create_PoliceFugitive(0,15,14,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
        NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,5,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(PoliceStanier, 15,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,18,false,PoliceVehicleType.Marked,134,0,3),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Unmarked,-1,0,3),
            Create_PoliceStanierOld(2,2,11,true,PoliceVehicleType.Detective,-1,0,3),
            Create_Washington(2,2,-1,true,true,-1,0,3),
            Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
            Create_PoliceLandstalkerXL(5,5,23,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceBuffalo, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            //new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            Create_PoliceBuffaloS(15, 15, 14, false, PoliceVehicleType.Marked, 111, -1, -1, -1, -1, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 70, 70){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            Create_PoliceInterceptor(70,70,16,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),
            new DispatchableVehicle(PoliceGresley, 30, 30){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGranger, 30, 30) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBison, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            Create_PoliceGauntlet(1,1,17,false,PoliceVehicleType.Marked,111,0,3,-1,-1,"",""),          
           
            new DispatchableVehicle(PoliceStanier, 0, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("riot", 0, 25) { CaninePossibleSeats = new List<int>{ 1,2 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffalo, 0, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            //new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            Create_PoliceBuffaloS(25, 20, 14, false, PoliceVehicleType.Marked, 111, 4, 5, 3, 4, "", ""),
            //new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            Create_PoliceInterceptor(0,50,16,false,PoliceVehicleType.Marked,111,4,5,3,4,"",""),
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
           
            Create_PoliceTransporter(0,35,5,false,50,false,true,134,4,-1,3,4,""),
            Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
        };
    }
    private void OtherPolice()
    {
        MarshalsServiceVehicles_FEJ = DispatchableVehicles.MarshalsServiceVehicles.Copy();//for now
        //MarshalsServiceVehicles_FEJ.Add(new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredLiveries = new List<int>() { 16 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceBuffaloS(25, 25, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));  
        MarshalsServiceVehicles_FEJ.Add(Create_Washington(25, 25, -1, true, true, -1, -1, -1));//  new DispatchableVehicle(WashingtonUnmarked, 25, 25) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, true, 40) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceStanierOld(20, 20, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1));// new DispatchableVehicle(PoliceStanierOld, 20, 20) { RequiredLiveries = new List<int>() { 11 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(3, true, 50), new DispatchableVehicleExtra(4, true, 50), new DispatchableVehicleExtra(5, true, 50), new DispatchableVehicleExtra(9, true, 50), }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
        MarshalsServiceVehicles_FEJ.Add(Create_PoliceInterceptor(15, 15, PIBlankLivID, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""));
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
        };
        BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,9,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecurityBobCat,
            DispatchableVehicles.AsteropeSecurityBobCat,
            Create_ServiceStanierOld(20,20,9,false,ServiceVehicleType.Security,134,-1,-1),
        };
        GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecurityG6,
            DispatchableVehicles.AsteropeSecurityG6,
            Create_ServiceStanierOld(20,20,8,false,ServiceVehicleType.Security,134,-1,-1),
        };
        SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceInterceptor(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1),
            DispatchableVehicles.AleutianSecuritySECURO,
            DispatchableVehicles.AsteropeSecuritySECURO,
            Create_ServiceStanierOld(20,20,7,false,ServiceVehicleType.Security,134,-1,-1),
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

            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi1,111,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi2,111,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi3,111,-1,-1),
            Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi4,111,-1,-1),

            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi4,134,-1,-1),
            DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles.TaxiEudora,
        };
        PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 1 } },
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi1,111,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi2,111,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi3,111,-1,-1),
            Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi4,111,-1,-1),

            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi4,134,-1,-1),
        };
        HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 2 } },
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi1,111,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi2,111,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi3,111,-1,-1),
            Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi4,111,-1,-1),

            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi4,134,-1,-1),
        };
        ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 3 } },
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi1,111,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi2,111,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi3,111,-1,-1),
            Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi4,111,-1,-1),

            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi4,134,-1,-1),
        };
        SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 4 } },
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),

            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi4,134,-1,-1),
        };
    }

    private enum PoliceVehicleType
    {
        Marked = 0,
        Unmarked = 1,
        Detective = 2,
        SlicktopMarked = 3,
    }

    private enum ServiceVehicleType
    {
        Taxi1 = 0,
        Taxi2 = 1,
        Taxi3 = 2,
        Taxi4 = 3,
        Security = 4,
    }
    private DispatchableVehicle Create_PoliceBuffaloS(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,
    int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
        else if(ServiceVehicleType == ServiceVehicleType.Taxi3)
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
    private DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,
        int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
    private DispatchableVehicle Create_PoliceLandstalkerXL(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, 
        int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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
        DispatchableVehicle dispatchableVehicle = Create_PoliceStanierOld(ambientPercent,wantedPercent,liveryID,useOptionalColors,PoliceVehicleType,requiredColor,minWantedLevel,maxWantedLevel);
        dispatchableVehicle.GroupName = groupName;
        dispatchableVehicle.RequiredPedGroup = requiredPedGroup;
        return dispatchableVehicle;
    }
    private DispatchableVehicle Create_PoliceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
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
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }
    private DispatchableVehicle Create_Washington(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, bool addExtras, int requiredColor, int minWantedLevel, int maxWantedLevel, string groupName, string requiredPedGroup)
    {
        DispatchableVehicle dispatchableVehicle = Create_Washington(ambientPercent, wantedPercent, liveryID, useOptionalColors, addExtras, requiredColor, minWantedLevel, maxWantedLevel);
        dispatchableVehicle.GroupName = groupName;
        dispatchableVehicle.RequiredPedGroup = requiredPedGroup;
        return dispatchableVehicle;
    }
    private DispatchableVehicle Create_Washington(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, bool addExtras, int requiredColor, int minWantedLevel, int maxWantedLevel)
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
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
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
    private DispatchableVehicle Create_PoliceGauntlet(int ambientPercent, int wantedPercent, int modKitliveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel,
    int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
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

