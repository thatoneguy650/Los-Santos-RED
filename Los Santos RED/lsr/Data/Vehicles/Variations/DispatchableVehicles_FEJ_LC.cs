using System.Collections.Generic;
using System.Linq;


public class DispatchableVehicles_FEJ_LC
{
    

    public string PoliceTorrence = "police3";//VANILLA SOUND
    public string PoliceGresley = "polgresleyliv";//"sheriff";//SOUND
    public string PoliceBuffaloSTX = "polbuffalostxliv";//"jester2";//SOUND
    public string PoliceGranger3600 = "polgranger3600liv";//"dune5";//SOUND
    public string PoliceAleutian = "polaleutianliv";//"blazer5";//SOUND

    //Bike/Atv
    public string PoliceVindicator = "polvindicatorliv";//"deathbike";//SOUND
    public string PoliceThrust = "polthrustliv";//"issi4";//SOUND
    public string PoliceBicycle = "scorcher";//actual bicycle, just vaniulla but we have the peds for it with fej?

    //Heli
    public string PoliceMaverick1stGen = "polmaverickoldliv";//"deathbike2";
    public string PoliceAnnihilator = "annihilatorliv";

    //Service
    public string FireTruck = "firetruckliv";//new
    public string Ambulance = "ambulanceliv";//new
    public string TaxiVivanite = "taxvivaniteliv";//"caddy3";//USING VANILLA ALREADY

    private DispatchableVehicles_FEJ DispatchableVehicles_FEJ;

    public List<DispatchableVehicle> UnmarkedVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> NYSPVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> USNPSParkRangersVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> FIBVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> BorderPatrolVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> NOOSEPIAVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> NOOSESEPVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> MarshalsServiceVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> DOAVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> LCTaxiVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> LCPDVehicles { get; private set; }
    public List<DispatchableVehicle> ASPVehicles { get; private set; }
    public List<DispatchableVehicle> LCPDHeliVehicles { get; private set; }
    public List<DispatchableVehicle> ASPHeliVehicles { get; private set; }
    public List<DispatchableVehicle> CoastGuardVehicles_LC { get; private set; }
    public List<DispatchableVehicle> FDLCVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> FDLCEMTVehicles_FEJ_LC { get; private set; }
    public List<DispatchableVehicle> HMSVehicles_FEJ_LC { get; private set; }
    public DispatchableVehicles_FEJ_LC(DispatchableVehicles_FEJ dispatchableVehicles_FEJ)
    {
        DispatchableVehicles_FEJ = dispatchableVehicles_FEJ;
    }
    public void DefaultConfig()
    {
        LocalPolice();
        StatePolice();
        LocalSheriff();
        ParkRangers();
        FederalPolice();
        Security();
        Taxis();
        Fire();
        EMT();
    }
    private void Fire()
    {
        FDLCVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("firetruckliv", 100, 100) { RequiredLiveries = new List<int>() { 0 } ,MinOccupants = 2, MaxOccupants = 4,ForcedPlateType = 8 }
        };
        FDLCVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates; });
    }
    private void EMT()
    {
        FDLCEMTVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulanceliv", 100, 100) { RequiredLiveries = new List<int>() { 0 },ForcedPlateType = 8 }
        };
        HMSVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ambulanceliv", 100, 100) { RequiredLiveries = new List<int>() { 1 },ForcedPlateType = 8 }
        };
        FDLCEMTVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates; });
        HMSVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates; });
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceGresleyLC(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGresleyLC(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        UnmarkedVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates; });
    }
    private void StatePolice()
    {
        NYSPVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceGresleyLC(50,50,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.NorthYanktonPlates; x.MaxRandomDirtLevel = 15.0f; });
    }
    private void LocalSheriff()
    {

        DispatchableVehicle LCPDk9 = Create_PoliceGranger3600LC(0, 50, 5, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", "");
        LCPDk9.CaninePossibleSeats = new List<int> { 1, 2 };
        LCPDk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 50) };

        LCPDVehicles = new List<DispatchableVehicle>()
        {
            Create_PoliceInterceptorLC(100, 100, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceInterceptorLC(100, 100, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceInterceptorLC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceInterceptorLC(20, 20, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceInterceptorLC(20, 20, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceGresleyLC(100, 100, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGresleyLC(100, 100, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGresleyLC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGresleyLC(20, 20, 2, false, PoliceVehicleType.MarkedNewSlicktop, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGresleyLC(20, 20, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),


            Create_PoliceGranger3600LC(20, 20, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGranger3600LC(20, 20, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGranger3600LC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGranger3600LC(20, 20, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGranger3600LC(2, 2, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceAleutianLC(25, 25, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceAleutianLC(25, 25, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceAleutianLC(10, 10, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceAleutianLC(10, 10, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceAleutianLC(2, 2, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceBuffaloSTXLC(25, 25, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceBuffaloSTXLC(25, 25, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceBuffaloSTXLC(2, 2, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceBuffaloSTXLC(2, 2, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceBuffaloSTXLC(100, 100, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            LCPDk9,

            Create_PoliceInterceptorLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceGresleyLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceGranger3600LC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceAleutianLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceBuffaloSTXLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),


            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3,RequiredPedGroup = "Detective" },

            Create_PoliceVindicatorLC(20,10,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            Create_PoliceThrustLC(20,10,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40,134,134,134),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

        };

        foreach(DispatchableVehicle dv in LCPDVehicles)
        {
            dv.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates;
        }

        DispatchableVehicle ASPk9 = Create_PoliceAleutianLC(0, 50, 5, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", "");
        ASPk9.CaninePossibleSeats = new List<int> { 1, 2 };
        ASPk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9,50) };

        ASPVehicles = new List<DispatchableVehicle>()
        {
            Create_PoliceInterceptorLC(100, 100, 4, false, PoliceVehicleType.MarkedOriginalLightbar, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceInterceptorLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceGresleyLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGresleyLC(100, 100, 4, false, PoliceVehicleType.MarkedNewSlicktop, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceGranger3600LC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceGranger3600LC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            Create_PoliceAleutianLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceAleutianLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            ASPk9,

            //

            Create_PoliceBuffaloSTXLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            Create_PoliceBuffaloSTXLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),


            Create_PoliceInterceptorLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceGresleyLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceGranger3600LC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceAleutianLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            Create_PoliceBuffaloSTXLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3, RequiredPedGroup = "Detective" },


            Create_PoliceVindicatorLC(20,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            Create_PoliceThrustLC(20,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40,134,134,134),

        };

        foreach (DispatchableVehicle dv in ASPVehicles)
        {
            dv.RequestedPlateTypes = DispatchableVehicles_FEJ.AlderneyPlates;
        }


        LCPDHeliVehicles = new List<DispatchableVehicle>()
        {
             new DispatchableVehicle("polmavliv", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 0 },RequiredPrimaryColorID = 134, RequiredSecondaryColorID = 134, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        ASPHeliVehicles = new List<DispatchableVehicle>()
        {
             new DispatchableVehicle("polmavliv", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true,RequiredLiveries = new List<int>() { 1 },RequiredPrimaryColorID = 134, RequiredSecondaryColorID = 134, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        CoastGuardVehicles_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dinghy5", 50, 50) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 1,MaxOccupants = 2, MaxWantedLevelSpawn = 2, },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle("seashark2", 50, 50) { RequiredLiveries = new List<int>() { 2,3 }, MaxOccupants = 1 },
            new DispatchableVehicle("polmavliv", 1,30) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 4 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
            new DispatchableVehicle("annihilatorliv", 1,100) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
        };
        CoastGuardVehicles_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
    }
    private void ParkRangers()
    {

        USNPSParkRangersVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceGresleyLC(25,25,20,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptorLC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),

            Create_PoliceGranger3600LC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            Create_PoliceAleutianLC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
        };
        USNPSParkRangersVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 15.0f; });
    }
    private void FederalPolice()
    {
        FIBVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceGresleyLC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceGresleyLC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),


            Create_PoliceBuffaloSTXLC(25,25,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceBuffaloSTXLC(25,25,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceGranger3600LC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceGranger3600LC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            Create_PoliceAleutianLC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            Create_PoliceAleutianLC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            new DispatchableVehicle("polmavliv", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            Create_PoliceInterceptorLC(0,25,11,true,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            Create_PoliceGresleyLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            Create_PoliceBuffaloSTXLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            Create_PoliceGranger3600LC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            Create_PoliceAleutianLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },
        };
        FIBVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 2.0f; });
        BorderPatrolVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceGresleyLC(25,25,19,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceInterceptorLC(15,15,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            Create_PoliceGranger3600LC(45,45,16,false,PoliceVehicleType.MarkedValorLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceAleutianLC(20,20,16,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTXLC(20,20,13,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        BorderPatrolVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 15.0f; });
        NOOSEPIAVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            Create_PoliceInterceptorLC(70,70,15,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
            Create_PoliceGresleyLC(70,70,17,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
           
            Create_PoliceInterceptorLC(0,50,15,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),
            Create_PoliceGresleyLC(0,40,17,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),

            Create_PoliceGranger3600LC(5,5,14,false,PoliceVehicleType.MarkedValorLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceAleutianLC(5,5,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTXLC(30,30,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            Create_PoliceGranger3600(0,15,14,false,PoliceVehicleType.MarkedValorLightbar,134,3,4,3,4,"",""),
            Create_PoliceAleutian(0,15,14,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        NOOSEPIAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        NOOSESEPVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            
            Create_PoliceInterceptorLC(35,35,16,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
            Create_PoliceGresleyLC(35,35,18,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
           
            Create_PoliceInterceptorLC(0,25,16,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),
            Create_PoliceGresleyLC(0,25,18,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),

            Create_PoliceGranger3600LC(25,25,15,false,PoliceVehicleType.MarkedValorLightbar,134,0,3,-1,-1,"",""),
            Create_PoliceAleutianLC(20,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            Create_PoliceBuffaloSTXLC(20,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),


            Create_PoliceGranger3600LC(0,35,15,false,PoliceVehicleType.MarkedValorLightbar,134,3,4,3,4,"",""),
            Create_PoliceAleutianLC(0,35,15,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),
            Create_PoliceBuffaloSTXLC(0,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        NOOSEPIAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        MarshalsServiceVehicles_FEJ_LC = new List<DispatchableVehicle>
        {
            Create_PoliceInterceptorLC(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceGresleyLC(35, 35, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceBuffaloSTXLC(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGranger3600LC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceAleutianLC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
        };
        MarshalsServiceVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        DOAVehicles_FEJ_LC = new List<DispatchableVehicle>
        {
            Create_PoliceInterceptorLC(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceGresleyLC(35, 35, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceBuffaloSTXLC(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            Create_PoliceGranger3600LC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            Create_PoliceAleutianLC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
        };
        DOAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
    }
    private void Security()
    {
        

    }
    private void Taxis()
    {
        LCTaxiVehicles_FEJ_LC = new List<DispatchableVehicle>() {

            Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi1,88,-1,-1),
            Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi2,88,-1,-1),
            Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi3,88,-1,-1),
            Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi4,88,-1,-1),

            Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi1,92,-1,-1),
            Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi2,92,-1,-1),
            Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi3,92,-1,-1),
            Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi4,92,-1,-1),
        };
        LCTaxiVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPlates; });
    }

    public DispatchableVehicle Create_TaxiVivanite(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        //Extras
        //NO EXTRAS!
        DispatchableVehicle toReturn = new DispatchableVehicle(TaxiVivanite, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (serviceVehicleType == ServiceVehicleType.Taxi1)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi2)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi3)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, true, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, true, 100),


            };
        }
        else if (serviceVehicleType == ServiceVehicleType.Taxi4)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),
                new DispatchableVehicleExtra(8, true, 100),
                new DispatchableVehicleExtra(9, true, 100),


            };
        }

        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, -1, -1, "", "");
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGranger3600(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGranger3600, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren,2  = ram bar,  4 valor lightbar, 5 alprs and antenna, 6 side antenna, 7 searchlights,8 interior alpr, 9 divider,10 = shotguns, 11 = computer, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked || policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 45),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 90),
                new DispatchableVehicleExtra(6, true, 45),
                new DispatchableVehicleExtra(7, true, 64),
                new DispatchableVehicleExtra(8, true, 75),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 95),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(7, true, 25),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 25),
                new DispatchableVehicleExtra(7, true, 45),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        //toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceBuffaloSTX(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceBuffaloSTX, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Extras 1- Siren, 2 - bar, 3 = flat lightbar, 4 - valor lightbar,5 - spoiler, 6- antenna, 7 = searchlight, 9 - divider, 10 - shotguns, 11 - computer,12 - radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 35),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),
                new DispatchableVehicleExtra(6, false, 95),
                new DispatchableVehicleExtra(7, true, 65),
                //new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 65),
                new DispatchableVehicleExtra(7, true, 65),
               // new DispatchableVehicleExtra(8, true, 85),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 75),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceAleutian(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceAleutian, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }


        if (requiredColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredColor;
            toReturn.RequiredSecondaryColorID = requiredColor;
        }
        //Extras 1- Siren, 2 - searchlights,3 = flat bar, 4 valor bar, 5 top antenna array, 6 big bull bar,, 8 = front camera,  9 divider,10 shotguns, 11 computer, 12 radio
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, true, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, true, 65),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, true, 90),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 35),
                new DispatchableVehicleExtra(6, true, 35),
                new DispatchableVehicleExtra(8, false, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, false, 65),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 95),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>()
            {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(8, false, 100),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
        }
        //toReturn.CaninePossibleSeats = new List<int>() { 1, 2 };
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceGresley(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceGresley, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (policeVehicleType == PoliceVehicleType.Marked)
        {
            //Gresley - 1 = Front Bar, 2 = siren old,3 = flat lightbar, 4 = valor lightar,5 = alprs1, 6 = searchligh,7 = antenna, 9 = divider, 10 = shotguns, 11 = computer, 12 = radio
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),



                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),

                new DispatchableVehicleExtra(5, true, 65),

                new DispatchableVehicleExtra(6, true, 45),


                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 45),


                new DispatchableVehicleExtra(7, true, 45),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),


                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 65),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, true, 45),

                new DispatchableVehicleExtra(6, true, 65),


                new DispatchableVehicleExtra(7, true, 65),

                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (policeVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, true, 25),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),


                new DispatchableVehicleExtra(5, true, 25),

                new DispatchableVehicleExtra(6, true, 65),


                new DispatchableVehicleExtra(7, true, 65),
                new DispatchableVehicleExtra(9, true, 100),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        else if (policeVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),

                new DispatchableVehicleExtra(5, false, 100),


                new DispatchableVehicleExtra(6, false, 65),


                new DispatchableVehicleExtra(7, false, 65),
                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, false, 100),
            };
            toReturn.RequiredLiveries = new List<int>() { 11 };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceInterceptor(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType PoliceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceTorrence, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        if (PoliceVehicleType == PoliceVehicleType.Marked)//2015 syle marked!
        {
            //1 = Searchlught, 2 = ram bar, 3 = siren,6 = top antenna alprs, 7 = rear alprs, 9 partition,10 = shotguns, 11 = computer, 12 = radio

            //3 = new flat siren,4 = valor siren, 5 = old libertybar

            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedOriginalLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {


                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, true, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedValorLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 45),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, true, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedFlatLightbar)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),


                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, true, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.SlicktopMarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.MarkedNewSlicktop)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 30),
                new DispatchableVehicleExtra(2, true, 25),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),
                new DispatchableVehicleExtra(6, true, 40),
                new DispatchableVehicleExtra(7, true, 40),

                new DispatchableVehicleExtra(9, true, 100),

                new DispatchableVehicleExtra(10, true, 100),

                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 100),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Unmarked)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),
                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(1, true, 35),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, true, 65),
                new DispatchableVehicleExtra(10, true, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        else if (PoliceVehicleType == PoliceVehicleType.Detective)
        {
            toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {

                new DispatchableVehicleExtra(1, false, 100),
                new DispatchableVehicleExtra(2, false, 100),

                new DispatchableVehicleExtra(1, true, 15),
                new DispatchableVehicleExtra(2, true, 5),
                new DispatchableVehicleExtra(3, false, 100),
                new DispatchableVehicleExtra(4, false, 100),
                new DispatchableVehicleExtra(5, false, 100),

                new DispatchableVehicleExtra(6, false, 100),
                new DispatchableVehicleExtra(7, false, 100),

                new DispatchableVehicleExtra(9, false, 100),
                new DispatchableVehicleExtra(10, false, 100),
                new DispatchableVehicleExtra(11, true, 100),
                new DispatchableVehicleExtra(12, true, 20),
            };
        }
        if (requiredColor != -1)
        {
            toReturn.RequiredDashColorID = requiredColor;
        }
        toReturn.MatchDashColorToBaseColor = true;
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceVindicator(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount)
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
        toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.Highway, highwayAdjustmentAmount) };// 25) };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        return toReturn;
    }
    public DispatchableVehicle Create_PoliceThrust(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount, int requiredPrimaryColor, int requiredSecondaryColor, int requiredDashcolor)
    {
        DispatchableVehicle toReturn = new DispatchableVehicle(PoliceThrust, ambientPercent, wantedPercent);
        if (liveryID != -1)
        {
            toReturn.RequiredLiveries = new List<int>() { liveryID };
        }
        //Vindicator -  2 = Crash Bar, 3= side luggae, 4 = top luggage
        toReturn.VehicleExtras = new List<DispatchableVehicleExtra>() {
                new DispatchableVehicleExtra(2, true, 95),
                new DispatchableVehicleExtra(3, true, 95),
                new DispatchableVehicleExtra(4, true, 95),
            };
        toReturn.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.Highway, highwayAdjustmentAmount) };// 25) };
        SetDefault(toReturn, useOptionalColors, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);

        if (requiredPrimaryColor != -1)
        {
            toReturn.RequiredPrimaryColorID = requiredPrimaryColor;
        }
        if (requiredSecondaryColor != -1)
        {
            toReturn.RequiredSecondaryColorID = requiredSecondaryColor;
        }
        if (requiredDashcolor != -1)
        {
            toReturn.RequiredDashColorID = requiredDashcolor;
            toReturn.MatchDashColorToBaseColor = true;
        }
        return toReturn;
    }


    //LC
    public DispatchableVehicle Create_TaxiVivaniteLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, ServiceVehicleType serviceVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel)
    {
        DispatchableVehicle intermediate = Create_TaxiVivanite(ambientPercent, wantedPercent, liveryID, useOptionalColors, serviceVehicleType, requiredColor, minWantedLevel, maxWantedLevel);
        intermediate.ModelName = "taxvivaniteliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = LibertyPolicePlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceGranger3600LC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle intermediate = Create_PoliceGranger3600(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.ModelName = "polgranger3600liv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceBuffaloSTXLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle intermediate = Create_PoliceBuffaloSTX(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.ModelName = "polbuffalostxliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates; ;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceAleutianLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle intermediate = Create_PoliceAleutian(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.ModelName = "polaleutianliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceGresleyLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle intermediate = Create_PoliceGresley(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.ModelName = "polgresleyliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceInterceptorLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName)
    {
        DispatchableVehicle intermediate = Create_PoliceInterceptor(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName);
        intermediate.ModelName = "police3liv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceVindicatorLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount)
    {
        DispatchableVehicle intermediate = Create_PoliceVindicator(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName, highwayAdjustmentAmount);
        intermediate.ModelName = "polvindicatorliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
    }
    public DispatchableVehicle Create_PoliceThrustLC(int ambientPercent, int wantedPercent, int liveryID, bool useOptionalColors, PoliceVehicleType policeVehicleType, int requiredColor, int minWantedLevel, int maxWantedLevel, int minOccupants, int maxOccupants, string requiredPedGroup, string groupName, int highwayAdjustmentAmount, int requiredPrimaryColor, int requiredSecondaryColor, int requiredDashcolor)
    {
        DispatchableVehicle intermediate = Create_PoliceThrust(ambientPercent, wantedPercent, liveryID, useOptionalColors, policeVehicleType, requiredColor, minWantedLevel, maxWantedLevel, minOccupants, maxOccupants, requiredPedGroup, groupName, highwayAdjustmentAmount, requiredPrimaryColor, requiredSecondaryColor, requiredDashcolor);
        intermediate.ModelName = "polthrustliv";
        //intermediate.ForcedPlateType = 8;
        //intermediate.RequestedPlateTypes = isLC ? LibertyPlates : AlderneyPlates;
        return intermediate;
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
            toSetup.OptionalColors = DispatchableVehicles_FEJ.DefaultOptionalColors.ToList();
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

