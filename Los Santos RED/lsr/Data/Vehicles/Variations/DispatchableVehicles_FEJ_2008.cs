using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchableVehicles_FEJ_2008
{
    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;
    public List<DispatchableVehicle> LSPD2008_FEJ { get; private set; }
    public List<DispatchableVehicle> LSSD2008_FEJ { get; private set; }
    public List<DispatchableVehicle> SAHP2008_FEJ { get; private set; }
    public List<DispatchableVehicle> NYSP2008_FEJ { get; private set; }
    public List<DispatchableVehicle> DowntownTaxi2008_FEJ { get; private set; }
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
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(65,65,0,false,PoliceVehicleType.OlderMarked,134,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(44,44,0,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceEsperanto(10,5,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 10, 10){ CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            DispatchableVehicles_FEJ.Create_PolicePatriot(5,15,0,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,5,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSeminole(1,1,6,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"","",2),

            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_Washington(2, 2, -1, true, true, -1, 0, 3,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 1,1),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,1,false,100,false,true,134,-1,-1,-1,-1,""),
            //DispatchableVehicles_FEJ.Create_PoliceBoxville(1,0,0,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),

            //DispatchableVehicles_FEJ.Create_PoliceBoxville(0,5,0,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,1,false,100,false,true,134,3,-1,3,4,""),

            //NO RIDERS
            //new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },
            //DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,3,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            //DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,0,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
        };

        LSSD2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(65,65,2,false,PoliceVehicleType.OlderMarked,134,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(55,55,2,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceEsperanto(20,15,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5) { RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20) { CaninePossibleSeats = new List<int>{ 1 },RequiredLiveries = new List<int>() {7 } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 5, 5){  RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            DispatchableVehicles_FEJ.Create_PolicePatriot(15,35,2,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(2,2,7,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceSeminole(2,1,4,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"","",10),

            DispatchableVehicles_FEJ.Create_Washington(1,1,-1,true,true,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,2,false,100,false,true,134,-1,-1,-1,-1,""),
            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,2,false,100,false,true,134,3,-1,3,4,""),

           // DispatchableVehicles_FEJ.Create_PoliceBoxville(1,0,5,false,PoliceVehicleType.Marked,0,-1,-1,-1,-1,"",""),
           // DispatchableVehicles_FEJ.Create_PoliceBoxville(0,5,5,false,PoliceVehicleType.Marked,0,3,4,3,4,"",""),



            //new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },
            //DispatchableVehicles_FEJ.Create_PoliceVindicator(20,10,5,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
            //DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,1,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
            //DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),
        };
        LSSD2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        SAHP2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(45,45,1,false,PoliceVehicleType.OlderMarked,134,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(45,45,1,false,PoliceVehicleType.SlicktopMarked,134,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceEsperanto(15,15,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceMerit(35,35,1,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceMerit(35,35,1,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 10, 10) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },


            DispatchableVehicles_FEJ.Create_PoliceStanierOld(1,1,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(1,1,3,true,PoliceVehicleType.Detective,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_Washington(3,3,-1,true,true,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PolicePatriot(10,10,1,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(10,10,4,false,PoliceVehicleType.SlicktopMarked,-1,-1,-1,-1,-1,"StandardSAHP","StandardSAHP"),

            //new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
            //new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },

            //new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
            //new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },


            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,60,3,false,PoliceVehicleType.Marked,134,3,4,3,4,"Pilot","Helicopter",-1),
            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(2,2,3,false,PoliceVehicleType.Marked,134,0,2,2,3,"Pilot","Helicopter",-1),

            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
           // DispatchableVehicles_FEJ.Create_PoliceVindicator(45,20,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle"),
        };

        PoliceHeliVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(1,200,0,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),
        };

        SheriffHeliVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(1,300,4,false,PoliceVehicleType.Marked,134,0,4,3,4,"Pilot","",-1),
        };

        NYSP2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceEsperanto(50,50,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            DispatchableVehicles_FEJ.Create_PoliceMerit(25,25,6,false,PoliceVehicleType.OlderMarked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 1, 1){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 20, 20){ RequiredLiveries = new List<int>() { 16 } },
        };
        NYSP2008_FEJ.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });
        DowntownTaxi2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 0 } },
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi2,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi3,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,1,false,ServiceVehicleType.Taxi4,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi1,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi2,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi3,134,-1,-1),
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,0,false,ServiceVehicleType.Taxi4,134,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi1,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi2,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi3,-1,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiMinivan(20,20,4,false,ServiceVehicleType.Taxi4,-1,-1,-1),

            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles_FEJ.DispatchableVehicles.TaxiEudora,
        };

        MerryweatherSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(35,35,5,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityMW,
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,6,false,ServiceVehicleType.Security,134,-1,-1),
        };
        BobcatSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,8,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityBobCat,
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,9,false,ServiceVehicleType.Security,134,-1,-1),
        };
        GroupSechsSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,7,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecurityG6,
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,8,false,ServiceVehicleType.Security,134,-1,-1),
        };
        SecuroservSecurity2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_ServiceDilettante(20,20,6,false,ServiceVehicleType.Security,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.DispatchableVehicles.AsteropeSecuritySECURO,
            DispatchableVehicles_FEJ.Create_ServiceStanierOld(20,20,7,false,ServiceVehicleType.Security,134,-1,-1),
        };

        UnmarkedVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(50,50,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(10,10,3,true,PoliceVehicleType.Detective,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 10, 10),
            DispatchableVehicles_FEJ.Create_Washington(30,30,-1,true,true,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(30,30,3,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(10,10,3,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked, 10, 10){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 10, 10) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            DispatchableVehicles_FEJ.Create_PolicePatriot(5, 5, 3, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", "")
        };
        LSLifeguardVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50)  { RequiredPedGroup = "ATV",GroupName = "ATV" },
            new DispatchableVehicle("seashark2", 100, 100) { RequiredPedGroup = "Boat",GroupName = "Boat", RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 6 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
            new DispatchableVehicle("polmav",2,5) { RequiredLiveries = new List<int>() { 5 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
        };
        LSLifeguardVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        ParkRangerVehicles2008_FEJ = new List<DispatchableVehicle>()//San Andreas State Parks
        {
            DispatchableVehicles_FEJ.Create_PoliceEsperanto(75,25,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PolicePatriot(25,75,5,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSeminole(25,25,0,false,PoliceVehicleType.OlderMarked,134,-1,-1,-1,-1,"","",10),
        };
        ParkRangerVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        NOOSESEPVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,5,false,50,false,true,134,0,3,-1,-1,""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 5,5){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,4,false,PoliceVehicleType.Marked,134,0,3,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Unmarked,-1,0,3,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(2,2,3,true,PoliceVehicleType.Detective,-1,0,3,"",""),
            DispatchableVehicles_FEJ.Create_Washington(2,2,-1,true,true,-1,0,3,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 5, 5){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 }, },
            DispatchableVehicles_FEJ.Create_PoliceMerit(5,5,8,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            //DispatchableVehicles_FEJ.Create_PoliceBoxville(1,0,2,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PolicePatriot(15,15,4,false,PoliceVehicleType.Marked,-1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,15,false,PoliceVehicleType.Marked,134,0,3,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceStanierOld(0,45,4,false,PoliceVehicleType.Marked,134,4,5,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            //new DispatchableVehicle("riot", 0, 25) { CaninePossibleSeats = new List<int>{ 1,2 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            DispatchableVehicles_FEJ.Create_PoliceMerit(0,35,7,false,PoliceVehicleType.Marked,-1,4,5,3,4,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) }, },
            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,15,5,false,50,false,true,134,4,-1,3,4,""),
            //DispatchableVehicles_FEJ.Create_PoliceBoxville(0,5,2,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PolicePatriot(0,45,4,false,PoliceVehicleType.Marked,-1,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,15,15,false,PoliceVehicleType.Marked,134,4,5,3,4,"",""),


            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,100,5,false,PoliceVehicleType.Marked,134,4,4,3,4,"","",-1),
            //new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };

        FIBVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(15,15,3,false,PoliceVehicleType.Detective,1,0,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceStanierOld(15,15,3,false,PoliceVehicleType.Unmarked,1,0,4,"",""),

            DispatchableVehicles_FEJ.Create_PoliceMerit(12,12,3,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceMerit(12,12,3,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_Washington(10,10,-1,false,true,1,0,4,"",""),

            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceFugitive(5,5,11,false,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PolicePatriot(10,25,3,false,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),

            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked,10,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked,10,10){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked,10,10) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4 },

            DispatchableVehicles_FEJ.Create_PoliceTransporter(10,10,7,false,0,true,true,1,0,4,-1,-1,""),
           // DispatchableVehicles_FEJ.Create_PoliceBoxville(2,2,4,false,PoliceVehicleType.Unmarked,1,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceStanierOld(0,15,3,false,PoliceVehicleType.Unmarked,1,5,5,"","FIBHET"),
            DispatchableVehicles_FEJ.Create_PoliceMerit(0,12,3,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_Washington(0,10,-1,false,true,1,5,5,"","FIBHET"),
            new DispatchableVehicle(DispatchableVehicles_FEJ.GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(DispatchableVehicles_FEJ.BuffaloUnmarked, 0, 20) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },


            DispatchableVehicles_FEJ.Create_PoliceMaverick1stGen(0,90,6,false,PoliceVehicleType.Marked,1,5,5,3,4,"FIBHET","",-1),

            //new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },
            //new DispatchableVehicle("polmav", 0, 30) { MinWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            //new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },

            DispatchableVehicles_FEJ.Create_PoliceFugitive(0,10,11,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PolicePatriot(0,45,3,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceTransporter(0,35,7,false,0,true,true,1,5,5,3,4,"FIBHET"),
            //DispatchableVehicles_FEJ.Create_PoliceBoxville(0,5,4,false,PoliceVehicleType.Unmarked,1,5,5,3,4,"FIBHET",""),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },


        };
        FIBVehicles2008_FEJ.ForEach(x => x.MaxRandomDirtLevel = 1.0f);
        PrisonVehicles2008_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 20) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            DispatchableVehicles_FEJ.Create_PoliceMerit(40,40,4,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBuffalo, 2, 2) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 5, 5) { RequiredLiveries = new List<int>() { 14 } },
        };
    }

}

