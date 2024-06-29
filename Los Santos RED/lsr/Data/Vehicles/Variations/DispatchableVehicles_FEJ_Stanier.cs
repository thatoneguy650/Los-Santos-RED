using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchableVehicles_FEJ_Stanier
{
    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;
    public List<DispatchableVehicle> UnmarkedVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSLifeguardVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> ParkRangerVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> SADFWParkRangersVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> USNPSParkRangersVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> LSDPRParkRangersVehicles_FEJ { get; private set; }
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
    public List<DispatchableVehicle> LNLVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> CHUFFVehicles_FEJ { get; private set; }
    public List<DispatchableVehicle> DowntownTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> PurpleTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> HellTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> ShitiTaxiVehicles { get; private set; }
    public List<DispatchableVehicle> SunderedTaxiVehicles { get; private set; }
    public DispatchableVehicles_FEJ_Stanier(DispatchableVehicles_FEJ dispatchableVehicles_FEJ)
    {
        DispatchableVehicles_FEJ = dispatchableVehicles_FEJ;
    }

    public void DefaultConfig()
    {
        LocalPolice();
        LocalSheriff();
        StatePolice();
        FederalPolice();
        Security();
        Taxis();
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 100, 100),
        };
        LSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25,20){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked, 1,1),      
        };
        EastLSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 35,35){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        VWPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 30,25){ RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        RHPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        DPPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,10){ RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        NYSPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } }, 
        };
        NYSPVehicles_FEJ.ForEach(x => { x.ForcedPlateType = 5; x.MaxRandomDirtLevel = 15.0f; });

        LCPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,15){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
    }
    private void LocalSheriff()
    {
        //Sheriff TEST
        BCSOVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceTransporter(2,0,0,false,100,false,true,134,-1,-1,-1,-1,""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 20){ RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("polmav", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter", RequiredLiveries = new List<int>() { 10 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 1, 150) { RequiredGroupIsDriverOnly = true, RequiredPedGroup = "Pilot",GroupName = "Helicopter",RequiredLiveries = new List<int>() { 5 }, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        BCSOVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        LSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 15){  RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        LSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20, 15) { RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        MajesticLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 15){ RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        VWHillsLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
        DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 55, 55){ RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } }, 
        };
        DavisLSSDVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 10.0f);
    }
    private void StatePolice()
    {
        //Other State
        LSIAPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        LSPPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25){ RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
        //State
        SAHPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 100,100){ GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("frogger2",1,1) { RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("frogger2",0,30) { RequiredGroupIsDriverOnly = true, RequiredLiveries = new List<int>() { 3 },MinOccupants = 3,MaxOccupants = 4, GroupName = "Helicopter",RequiredPedGroup = "Pilot",MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle("polmav", 1,1) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MaxWantedLevelSpawn = 2,MinOccupants = 2,MaxOccupants = 4 },
            new DispatchableVehicle("polmav", 0,30) { RequiredPedGroup = "Pilot", GroupName = "Helicopter",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            DispatchableVehicles_FEJ.Create_PoliceSanchez(1,0,2,false,PoliceVehicleType.Marked,0,-1,2,1,1,"DirtBike","DirtBike",10),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
        };
        PrisonVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 25, 25) { RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
        };
    }
    private void FederalPolice()
    {
        LSLifeguardVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lguard", 50, 50),
            new DispatchableVehicle("blazer2",50,50)  { RequiredPedGroup = "ATV",GroupName = "ATV" },
            new DispatchableVehicle("seashark2", 100, 100) { RequiredPedGroup = "Boat",GroupName = "Boat", RequiredLiveries = new List<int>() { 0,1 }, MaxOccupants = 1 },
            new DispatchableVehicle("frogger2",2,5) { RequiredLiveries = new List<int>() { 6 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
            new DispatchableVehicle("polmav",2,5) { RequiredLiveries = new List<int>() { 5 }, MinOccupants = 2,MaxOccupants = 3, GroupName = "Helicopter" },
        };
        ParkRangerVehicles_FEJ = new List<DispatchableVehicle>()//San Andreas State Parks
        {
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,8,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
        };
        USNPSParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresley(25,25,20,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceGranger, 40, 40) { RequiredLiveries = new List<int>() { 20 } },
            DispatchableVehicles_FEJ.Create_PoliceBison(40,40,15,false,PoliceVehicleType.Marked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,5,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
        };
        SADFWParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,6,false,PoliceVehicleType.Marked,51,-1,2,1,1,"DirtBike","DirtBike",10),
        };
        LSDPRParkRangersVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceSanchez(10,10,7,false,PoliceVehicleType.Marked,134,-1,2,1,1,"DirtBike","DirtBike",10),
        };

        ParkRangerVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        FIBVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked,100,100) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle(DispatchableVehicles_FEJ.StanierUnmarked,0,100) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1, MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, },
            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } },
            new DispatchableVehicle("polmav", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("annihilator", 0, 30) { RequiredLiveries = new List<int>() { 2 },RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET", MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },
        };
        FIBVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 2.0f);
        BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 20,20){ RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        BorderPatrolVehicles_FEJ.ForEach(x => x.MaxRandomDirtLevel = 15.0f);
        NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,10){ RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 15,15){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(DispatchableVehicles_FEJ.PoliceStanier, 0, 15) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("polmav", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        MarshalsServiceVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("police4", 50, 50),
        };
    }

    private void Security()
    {
        MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,0,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,3,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,2,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,1,false,ServiceVehicleType.Security,-1,-1,-1),
        };
        LNLVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,5,false,ServiceVehicleType.Security,-1,-1,-1),
        };

        CHUFFVehicles_FEJ = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_SecurityStanier(20,20,4,false,ServiceVehicleType.Security,-1,-1,-1),
        };
    }
    private void Taxis()
    {
        DowntownTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 0 } },
        };
        PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 1 } },
        };
        HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 2 } },
        };
        ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 3 } },
        };
        SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 85, 85){ RequiredLiveries = new List<int>() { 4 } },
        };
    }
}

