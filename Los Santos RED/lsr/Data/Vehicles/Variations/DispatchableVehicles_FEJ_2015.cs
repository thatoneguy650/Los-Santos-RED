//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class DispatchableVehicles_FEJ_2015
//{
//    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;
//    public List<DispatchableVehicle> UnmarkedVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSLifeguardVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> ParkRangerVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> SADFWParkRangersVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> USNPSParkRangersVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSDPRParkRangersVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> FIBVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> NOOSEVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> EastLSPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> VWPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> RHPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> DPPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> BCSOVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSSDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> MajesticLSSDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> DavisLSSDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSIAPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LSPPVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> SAHPVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> PrisonVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> NYSPVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LCPDVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> BorderPatrolVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> NOOSEPIAVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> NOOSESEPVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> MerryweatherPatrolVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> BobcatSecurityVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> GroupSechsVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> SecuroservVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> LNLVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> CHUFFVehicles_FEJ { get; private set; }
//    public List<DispatchableVehicle> DowntownTaxiVehicles { get; private set; }
//    public List<DispatchableVehicle> PurpleTaxiVehicles { get; private set; }
//    public List<DispatchableVehicle> HellTaxiVehicles { get; private set; }
//    public List<DispatchableVehicle> ShitiTaxiVehicles { get; private set; }
//    public List<DispatchableVehicle> SunderedTaxiVehicles { get; private set; }
//    public DispatchableVehicles_FEJ_2015(DispatchableVehicles_FEJ dispatchableVehicles_FEJ)
//    {
//        DispatchableVehicles_FEJ = dispatchableVehicles_FEJ;
//    }

//    public void DefaultConfig()
//    {
//        LocalPolice();
//        LocalSheriff();
//        StatePolice();
//        FederalPolice();
//        Security();
//        Taxis();
//    }
//    private void LocalPolice()
//    {
//        UnmarkedVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 50, 50),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_Washington(30,30,-1,true,true,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(50, 50, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceKuruma(5,5,-1,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceKuruma(5,5,-1,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//        };
//        LSPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_Washington(2, 2, -1, true, true, -1, 0, 3,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25,20){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(48,35,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(48,35,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 20, 1, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 15, 12){ CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 1,1),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
//        };
//        EastLSPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 35,35){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 3, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 25, 25){ CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,3,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
//        };
//        VWPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 30,25){ RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 25, 2, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(50,50,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 25, 25){CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 2 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,1,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),
//        };
//        RHPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(50, 50, 5, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 15, 15){CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 5 } },
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(20,15,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
//        };
//        DPPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20, 20, 6, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(50,50,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(50,50,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 15, 15){RequiredLiveries = new List<int>() { 6 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(15,15,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(15,10,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
//        };
//        NYSPVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceEsperanto(1,0,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceMerit(20,20,6,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 10, 10){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 25, 25){ RequiredLiveries = new List<int>() { 16 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(25,25,16,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//        };
//        NYSPVehicles_FEJ.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });

//        LCPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,15){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceMerit(25,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(48,35,15,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 25, 20){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 15, 12){ RequiredLiveries = new List<int>() { 15 } },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 1,1),
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 1,1),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 15, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 5 } },
//        };


//    }
//    private void LocalSheriff()
//    {
//        //Sheriff TEST
//        BCSOVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,0,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 20){ RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(7, 7, 0, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20) { RequiredLiveries = new List<int>() {0 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(15,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGauntlet(1,1,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(4,4,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,3,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 10, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 2 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,0,false,100,false,true,134,3,-1,3,4,""),
//            new DispatchableVehicle("dinghy5", 0, 20) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
//            new DispatchableVehicle("polmav", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter", RequiredLiveries = new List<int>() { 10 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//            new DispatchableVehicle("annihilator", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter",RequiredLiveries = new List<int>() { 5 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//        };
//        BCSOVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
//        LSSDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 15){  RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 7, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 25, 25) { CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() {7 } },
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
//            new DispatchableVehicle("dinghy5", 0, 20) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
//        };
//        LSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
//        MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 15) { RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 25, 8, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 50, 50) { RequiredLiveries = new List<int>() { 8 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,8,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
//            new DispatchableVehicle("dinghy5", 0, 20) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
//        };
//        MajesticLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
//        VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 15){ RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 9, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(20,20,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20)  { RequiredLiveries = new List<int>() { 9 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(7,7,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,9,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceVerus(1,0,3,false,PoliceVehicleType.Marked,-1,0,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
//            new DispatchableVehicle("dinghy5", 0, 20) { RequiredPrimaryColorID = 0, RequiredSecondaryColorID = 0,FirstPassengerIndex = 3, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4 },
//        };
//        VWHillsLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
//        DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 55, 55){ RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(10, 10, 10, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 15, 15)  { RequiredLiveries = new List<int>() { 10 }, },
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,10,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),
//        };
//        DavisLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
//    }
//    private void StatePolice()
//    {
//        //Other State
//        LSIAPDVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15, 15, 11, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5)  { RequiredLiveries = new List<int>() { 12 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,12,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//        };
//        LSPPVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,3,false,100,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(3,3,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 12, false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(10,10,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 10, 10){ CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() { 13 } },
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,13,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"Detective",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 10, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 4 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(15,10,4,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,3,false,100,false,true,134,3,-1,3,4,""),
//        };
//        //State
//        SAHPVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_Washington(3,3,-1,true,true,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(5, 5, 16, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(1,1,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 10,10){ RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(45,55,4,false, PoliceVehicleType.Marked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20,20,4,false, PoliceVehicleType.SlicktopMarked, -1, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"","",0),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,4,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,-1,-1,"","",0),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"","",0),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"","",0),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { RequiredLiveries = new List<int>() { 4 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,2,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
//            new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },
//            new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
//            new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
//            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,5,3,false,PoliceVehicleType.Marked,134,3,4,3,4,"Pilot","Helicopter",-1),
//            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(1,1,3,false,PoliceVehicleType.Marked,134,0,2,2,3,"Pilot","Helicopter",-1),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,2,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
//            DispatchableVehicles_FEJ.Create_PoliceVindicator(45,20,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",25),
//        };
//        PrisonVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceMerit(5,2,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 25, 25) { RequiredLiveries = new List<int>() { 14 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,0,14,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//        };
//    }
//    private void FederalPolice()
//    {
        

//        LSLifeguardVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            new DispatchableVehicle("lguard", 50, 50),
//            new DispatchableVehicle("blazer2",50,50)  { RequiredPedGroup = "ATV",GroupName = "ATV" },
//            new DispatchableVehicle("freecrawler",5,5) { RequiredVariation = new VehicleVariation() { VehicleMods = new List<VehicleMod>() {new VehicleMod(48, 7) } } },
//            new DispatchableVehicle("seashark2", 100, 100) { RequiredPedGroup = "Boat",GroupName = "Boat", RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
//            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 6 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
//            new DispatchableVehicle("polmav",2,5) { RequiredLiveries = new List<int>() { 5 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
//        };


//        ParkRangerVehicles_FEJ = new List<DispatchableVehicle>()//San Andreas State Parks
//        {
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,8,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//        };
//        USNPSParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,20,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 40, 40) { RequiredLiveries = new List<int>() { 20 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(40,40,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,5,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//        };
//        SADFWParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,6,false,PoliceVehicleType.Marked,51,-1,2,1,1,"DirtBike","DirtBike",10),
//        };
//        LSDPRParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,7,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
//        };

//        ParkRangerVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
//        FIBVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(10,10,7,false,0,true,true,1,0,4,-1,-1,""),
//            DispatchableVehicles_FEJ.Create_Washington(10,10,-1,false,true,1,0,4,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked,10,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4, },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked,10,10) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(10,10,16,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(5,5,11,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(5,5,11,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(5,5,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBison(1,1,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceKuruma(5,5,-1,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceFrogger, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceMaverick, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,1,6,false,PoliceVehicleType.Marked,1,5,5,3,4,"FIBHET","",-1),

//            //new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,7,false,0,true,true,1,5,5,3,4,"FIBHET"),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(0,35,16,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(0,15,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
//            DispatchableVehicles_FEJ.Create_PoliceKuruma(0,20,-1,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
//            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },
//        };
//        FIBVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 2.0f);
//        BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,6,false,50,false,true,134,-1,-1,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(20, 20, 15, false, PoliceVehicleType.Marked, 134, -1, -1, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(40,40,19,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 40, 40){ RequiredLiveries = new List<int>() { 19 } },
//            DispatchableVehicles_FEJ.Create_PoliceBison(35,35,19,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,6,false,50,false,true,134,4,5,3,4,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceMaverick, 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceAnnihilator, 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//        };
//        BorderPatrolVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
//        NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,4,false,50,false,true,134,0,3,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,14,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(35, 35, 13, false, PoliceVehicleType.Marked, 134, 0, 3, -1, -1, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(70,70,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(70,70,17,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 }, },
//            DispatchableVehicles_FEJ.Create_PoliceBison(10,10,17,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,10){ MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(0, 40, 13, false, PoliceVehicleType.Marked, -1, 4, 5, 3, 4, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,50,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(0,40,17,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,4,false,50,false,true,134,4,5,3,4,""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,15,14,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceMaverick, 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceAnnihilator, 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//        };
//        NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,5,false,50,false,true,134,0,3,-1,-1,""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(15,15,14,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(70,70,16,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(30,30,18,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
//            DispatchableVehicles_FEJ.Create_PoliceBison(5,5,18,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
//            DispatchableVehicles_FEJ.Create_PoliceBuffaloS(25, 20, 14, false, PoliceVehicleType.Marked, 134, 4, 5, 3, 4, "", ""),
//            DispatchableVehicles_FEJ.Create_PoliceInterceptor(0,50,16,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceGresley(0,40,18,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,5,false,50,false,true,134,4,-1,3,4,""),
//            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),
//            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,5,5,false,PoliceVehicleType.Marked,134,4,4,3,4,"","",-1),
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceMaverick, 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceAnnihilator, 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
//        };
//    }

//    private void Security()
//    {
//        MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,5,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,6,false,ServiceVehicleType.Security,-1,-1,-1),
//            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityMW,
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,0,false,ServiceVehicleType.Security,-1,-1,-1),
//        };
//        BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,9,false,ServiceVehicleType.Security,-1,-1,-1),
//            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityBobCat,
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,3,false,ServiceVehicleType.Security,-1,-1,-1),
//        };
//        GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1),
//            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityG6,
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,2,false,ServiceVehicleType.Security,-1,-1,-1),
//        };
//        SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1),
//            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecuritySECURO,
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,1,false,ServiceVehicleType.Security,-1,-1,-1),
//        };

//        LNLVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,5,false,ServiceVehicleType.Security,-1,-1,-1),
//        };

//        CHUFFVehicles_FEJ = new List<DispatchableVehicle>()
//        {
//            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,4,false,ServiceVehicleType.Security,-1,-1,-1),
//        };

//    }
//    private void Taxis()
//    {
//        DowntownTaxiVehicles = new List<DispatchableVehicle>() {
//            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 0 } },

//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),

//            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiBroadWay,
//            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiEudora,
//        };
//        PurpleTaxiVehicles = new List<DispatchableVehicle>() {
//            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 1 } },
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,1,false,ServiceVehicleType.Taxi4,134,-1,-1),

//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi1,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi2,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi3,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi4,-1,-1,-1),
//        };
//        HellTaxiVehicles = new List<DispatchableVehicle>() {
//            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 2 } },
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,2,false,ServiceVehicleType.Taxi4,134,-1,-1),

//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi1,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi2,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi3,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi4,-1,-1,-1),
//        };
//        ShitiTaxiVehicles = new List<DispatchableVehicle>() {
//            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 3 } },
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi1,0,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi2,0,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi3,0,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,3,false,ServiceVehicleType.Taxi4,0,-1,-1),

//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi1,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi2,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi3,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi4,-1,-1,-1),
//        };
//        SunderedTaxiVehicles = new List<DispatchableVehicle>() {
//            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 4 } },
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
//            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
//            DispatchableVehicles_FEJ.Create_ServiceInterceptor(35,35,4,false,ServiceVehicleType.Taxi4,134,-1,-1),

//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi1,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi2,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi3,-1,-1,-1),
//            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi4,-1,-1,-1),
//        };
//    }
//}

