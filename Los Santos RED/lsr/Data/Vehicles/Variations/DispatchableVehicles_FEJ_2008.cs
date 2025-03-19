using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchableVehicles_FEJ_2008
{
    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;
    public string WashingtonUnmarked = "polwashingtonunmarked";
    public string PoliceBuffalo = "police2";
    public string PoliceMerit = "polmeritliv";
    public string PoliceEsperanto = "polesperantoliv";
    public string PoliceTransporter = "poltransporterliv";
    public string PoliceStanierOld = "polstanieroldliv";
    public string PoliceFugitive = "polfugitiveliv";
    public string PolicePatriot = "polpatriotliv";
    public string PoliceSeminole = "polseminoleliv";
    public string PoliceMaverick1stGen = "polmaverickoldliv";

    public string ServiceDilettante = "servdilettante";
    public string TaxiMinivan = "servminivan";
    public string ServiceStanierOld = "servstanierold";

    public List<int> DefaultOptionalColors { get; private set; } = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 61, 62, 64, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 112 };
    public List<DispatchableVehicle> LSPD2008_FEJ { get; private set; }
    public List<DispatchableVehicle> LSSD2008_FEJ { get; private set; }
    public List<DispatchableVehicle> SAHP2008_FEJ { get; private set; }
    public List<DispatchableVehicle> NYSP2008_FEJ { get; private set; }
    public List<DispatchableVehicle> DowntownTaxi2008_FEJ { get; private set; }
    public List<DispatchableVehicle> PurpleTaxiVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> HellTaxiVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> ShitiTaxiVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> SunderedTaxiVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> MerryweatherSecurity2008_FEJ { get; private set; }
    public List<DispatchableVehicle> GroupSechsSecurity2008_FEJ { get; private set; }
    public List<DispatchableVehicle> BobcatSecurity2008_FEJ { get; private set; }
    public List<DispatchableVehicle> SecuroservSecurity2008_FEJ { get; private set; }
    public List<DispatchableVehicle> UnmarkedVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> LSLifeguardVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> ParkRangerVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> NOOSESEPVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> FIBVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> PrisonVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> PoliceHeliVehicles2008_FEJ { get; private set; }
    public List<DispatchableVehicle> SheriffHeliVehicles2008_FEJ { get; private set; }

    public DispatchableVehicles_FEJ_2008(DispatchableVehicles_FEJ dispatchableVehicles_FEJ)
    {
        DispatchableVehicles_FEJ = dispatchableVehicles_FEJ;
    }
    public void DefaultConfig()
    {
        LSPD2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceStanierOld(65,65,0,false,PoliceVehicleType.OlderMarked,134,-1,-1,"",""),
            Create_PoliceMerit(44,44,0,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceEsperanto(10,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 10, 10){ CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            Create_PolicePatriot(5,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceSeminole(1,1,6,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"","",2),

            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_Washington(2, 2, -1, true, true, -1, 0, 3,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 1,1),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),

            //NO RIDERS
            //new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
        };

        LSSD2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceStanierOld(65,65,2,false,PoliceVehicleType.OlderMarked,134,-1,-1,"",""),
            Create_PoliceMerit(55,55,2,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceEsperanto(20,15,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5) { RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20) { CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() {7 } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 5, 5){  RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PolicePatriot(15,35,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),


            Create_PoliceSeminole(2,1,4,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"","",10),

            Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),

            //NO RIDERS
            //new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
        };
        LSSD2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        SAHP2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceStanierOld(45,45,1,false,PoliceVehicleType.OlderMarked,134,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceStanierOld(45,45,1,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,"StandardSAHP","StandardSAHP"),

            Create_PoliceEsperanto(15,15,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            Create_PoliceMerit(35,35,1,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceMerit(35,35,1,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 10, 10) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },


            Create_PoliceStanierOld(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceStanierOld(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_Washington(3,3,-1,true,true,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            Create_PolicePatriot(10,10,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),


            Create_PoliceMaverick1stGen(0,60,3,false,PoliceVehicleType.Marked,134,3,4,3,4,"Pilot","Helicopter",-1),
            Create_PoliceMaverick1stGen(2,2,3,false,PoliceVehicleType.Marked,134,0,2,2,3,"Pilot","Helicopter",-1),

            new DispatchableVehicle("policebikeold", 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
        };

        PoliceHeliVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceMaverick1stGen(1,200,0,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),
        };

        SheriffHeliVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceMaverick1stGen(1,300,4,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),
        };

        NYSP2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceEsperanto(50,50,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(25,25,6,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 1, 1){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20){ RequiredLiveries = new List<int>() { 16 } },
        };
        NYSP2008_FEJ.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });
        DowntownTaxi2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 0 } },
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),

            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiEudora,
        };



        PurpleTaxiVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,2,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,1,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,1,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };

        HellTaxiVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,0,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,2,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,0,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };

        ShitiTaxiVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,3,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,3,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,2,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };

        SunderedTaxiVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            Create_ServiceDilettante(35,35,4,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi1,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi2,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi3,134,-1,-1),
            Create_ServiceStanierOld(20,20,4,false,ServiceVehicleType.Taxi4,134,-1,-1),

            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            Create_TaxiMinivan(20,20,3,false,ServiceVehicleType.Taxi4,-1,-1,-1),
        };

        MerryweatherSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(35,35,5,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceStanierOld(20,20,6,false,ServiceVehicleType.Security,134,-1,-1),
        };
        BobcatSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceStanierOld(20,20,9,false,ServiceVehicleType.Security,134,-1,-1),
        };
        GroupSechsSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceStanierOld(20,20,8,false,ServiceVehicleType.Security,134,-1,-1),
        };
        SecuroservSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            Create_ServiceStanierOld(20,20,7,false,ServiceVehicleType.Security,134,-1,-1),
        };

        UnmarkedVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceStanierOld(50,50,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            Create_PoliceStanierOld(10,10,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 10, 10),
            Create_Washington(30,30,-1,true,true,-1,-1,-1,"",""),
            Create_PoliceMerit(30,30,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceMerit(10,10,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked, 10, 10){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 10, 10) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            Create_PolicePatriot(5, 5, 3, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "")
        };
        LSLifeguardVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50)  { RequiredPedGroup = "ATV",GroupName = "ATV" },
            new DispatchableVehicle("seashark2", 100, 100) { RequiredPedGroup = "Boat",GroupName = "Boat", RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 0 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
        };
        LSLifeguardVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        ParkRangerVehicles2008_FEJ = new List<DispatchableVehicle>()//San Andreas State Parks
        {
            Create_PoliceEsperanto(75,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PolicePatriot(25,75,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceSeminole(25,25,0,false,PoliceVehicleType.OlderMarked,134,-1,-1,-1,-1,"","",10),
        };
        ParkRangerVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        NOOSESEPVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceTransporter(2,0,5,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 5,5){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceStanierOld(2,2,4,false,PoliceVehicleType.Marked,134,0,3,"",""),
            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,0,3,"",""),
            Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,0,3,"",""),
            Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            Create_PoliceMerit(5,5,8,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PolicePatriot(15,15,4,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),


            Create_PoliceStanierOld(0,45,4,false,PoliceVehicleType.Marked,134,4,5,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(0,35,7,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            Create_PoliceTransporter(0,15,5,false,50,false,true,134,4,-1,3,4,""),
            Create_PolicePatriot(0,45,4,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),


            Create_PoliceMaverick1stGen(0,100,5,false,PoliceVehicleType.Marked,134,4,4,3,4,"","",-1),
        };

        FIBVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            Create_PoliceStanierOld(15,15,3,false,PoliceVehicleType.Detective,1,0,4,"",""),
            Create_PoliceStanierOld(15,15,3,false,PoliceVehicleType.Unmarked,1,0,4,"",""),

            Create_PoliceMerit(12,12,3,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceMerit(12,12,3,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            Create_Washington(10,10,-1,false,true,1,0,4,"",""),

            Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            Create_PolicePatriot(10,25,3,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),

            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked,10,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked,10,10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked,10,10) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },

            Create_PoliceTransporter(10,10,7,false,0,true,true,1,0,4,-1,-1,""),

            Create_PoliceStanierOld(0,15,3,false,PoliceVehicleType.Unmarked,1,5,5,"","FIBHET"),
            Create_PoliceMerit(0,12,3,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_Washington(0,10,-1,false,true,1,5,5,"","FIBHET"),
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked, 0, 20) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },


            Create_PoliceMaverick1stGen(0,90,6,false,PoliceVehicleType.Marked,1,5,5,3,4,"FIBHET","",-1),

            Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PolicePatriot(0,45,3,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            Create_PoliceTransporter(0,35,7,false,0,true,true,1,5,5,3,4,"FIBHET"),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },


        };
        FIBVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 1.0f);
        PrisonVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 20) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            Create_PoliceMerit(40,40,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 2, 2) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { RequiredLiveries = new List<int>() { 14 } },
        };
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
    public DispatchableVehicle Create_ServiceDilettante(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType ServiceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string requiredPedGroup, string groupName)
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
    public DispatchableVehicle Create_Washington(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, bool addExtras, int requiredColor, int minWantedLevel, int maxWantedLevel, string requiredPedGroup, string groupName)
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
            if (sirenPercent > 0)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(1, true, sirenPercent));
            }
            if (addInteriorExtra)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(12, true, 55));
            }
            if (removeSiren)
            {
                toReturn.VehicleExtras.Add(new DispatchableVehicleExtra(1, false, 100));
            }
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, "");
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceStanierOld(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, string requiredPedGroup, string groupName)
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
    public DispatchableVehicle Create_PoliceFugitive(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle("polfugitiveliv", ambientPercent, wantedPercent);
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
    public DispatchableVehicle Create_PoliceMaverick1stGen(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int requiredSecondaryColor)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceMaverick1stGen, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        toReturn.RequiredGroupIsDriverOnly = true;
        if (requiredSecondaryColor != -1)
        {
            toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        }
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
            //toReturn.RequiredLiveries = new List<int>() { 11 };
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
            //toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    private void SetDefault(DispatchableVehicle toSetup, bool useOptionalColors, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants,
        string requiredPedGroup, string groupName)
    {
        if (toSetup == null)
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

