using System.Collections.Generic;


public class DispatchableVehicles_FEJ_LC
{
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
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Detective,-1,-1,-1,-1,-1,"",""),
        };
        UnmarkedVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates; });
    }
    private void StatePolice()
    {
        NYSPVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(50,50,16,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
        };
        NYSPVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.NorthYanktonPlates; x.MaxRandomDirtLevel = 15.0f; });
    }
    private void LocalSheriff()
    {

        DispatchableVehicle LCPDk9 = DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(0, 50, 5, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", "");
        LCPDk9.CaninePossibleSeats = new List<int> { 1, 2 };
        LCPDk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 200) };

        LCPDVehicles = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(100, 100, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(100, 100, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(20, 20, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(20, 20, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(100, 100, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(100, 100, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(20, 20, 2, false, PoliceVehicleType.MarkedNewSlicktop, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(20, 20, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),


            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(20, 20, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(20, 20, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(20, 20, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(20, 20, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(2, 2, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(25, 25, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(25, 25, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(10, 10, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(10, 10, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(2, 2, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(25, 25, 0, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(25, 25, 1, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(2, 2, 2, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(2, 2, 2, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(100, 100, 3, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", ""),

            LCPDk9,

            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),


            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3,RequiredPedGroup = "Detective" },

            DispatchableVehicles_FEJ.Create_PoliceVindicatorLC(20,10,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceThrustLC(20,10,0,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40,134,134,134),
            DispatchableVehicles_FEJ.Create_PoliceBicycle(0,0,-1,false,PoliceVehicleType.Unmarked,0,-1,2,1,1,"Bicycle","Bicycle",50),

        };

        foreach(DispatchableVehicle dv in LCPDVehicles)
        {
            dv.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPolicePlates;
        }

        DispatchableVehicle ASPk9 = DispatchableVehicles_FEJ.Create_PoliceAleutianLC(0, 50, 5, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", "");
        ASPk9.CaninePossibleSeats = new List<int> { 1, 2 };
        ASPk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9,200) };

        ASPVehicles = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(100, 100, 4, false, PoliceVehicleType.MarkedOriginalLightbar, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(100, 100, 4, false, PoliceVehicleType.MarkedNewSlicktop, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),

            ASPk9,

            //

            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(100, 100, 4, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(100, 100, 4, false, PoliceVehicleType.SlicktopMarked, 134, 0, 3, 1, 2, "", ""),


            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(2, 2, 11, true, PoliceVehicleType.Detective, -1, 0, 3, 1, 2, "Detective", ""),
            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3, RequiredPedGroup = "Detective" },


            DispatchableVehicles_FEJ.Create_PoliceVindicatorLC(20,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40),
            DispatchableVehicles_FEJ.Create_PoliceThrustLC(20,10,1,false,PoliceVehicleType.Marked,-1,-1,2,1,1,"MotorcycleCop","Motorcycle",40,134,134,134),

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
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,20,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(20,20,17,false,PoliceVehicleType.Marked,134,-1,-1,-1,-1,"",""),
        };
        USNPSParkRangersVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 15.0f; });
    }
    private void FederalPolice()
    {
        FIBVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Unmarked,1,0,4,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(25,25,11,true,PoliceVehicleType.Detective,1,0,4,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(25,25,11,true,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(25,25,11,true,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(25,25,11,false,PoliceVehicleType.Unmarked,1,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(25,25,11,false,PoliceVehicleType.Detective,1,0,3,-1,-1,"",""),


            new DispatchableVehicle("polmavliv", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5,RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 3 } },
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(0,25,11,true,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(0,25,11,false,PoliceVehicleType.Detective,1,5,5,3,4,"FIBHET",""),

            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 0, RequiredPedGroup = "FIBHET", ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 6, },
        };
        FIBVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 2.0f; });
        BorderPatrolVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(25,25,19,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(15,15,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(45,45,16,false,PoliceVehicleType.MarkedValorLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(20,20,16,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(20,20,13,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        BorderPatrolVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; x.MaxRandomDirtLevel = 15.0f; });
        NOOSEPIAVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(70,70,15,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(70,70,17,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
           
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(0,50,15,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(0,40,17,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(5,5,14,false,PoliceVehicleType.MarkedValorLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(5,5,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(30,30,14,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600(0,15,14,false,PoliceVehicleType.MarkedValorLightbar,134,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutian(0,15,14,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 8 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 7 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        NOOSEPIAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        NOOSESEPVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(35,35,16,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(35,35,18,false,PoliceVehicleType.MarkedFlatLightbar,134,0,3,-1,-1,"",""),
           
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(0,25,16,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(0,25,18,false,PoliceVehicleType.MarkedFlatLightbar,134,4,5,3,4,"",""),

            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(25,25,15,false,PoliceVehicleType.MarkedValorLightbar,134,0,3,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(20,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(20,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,-1,-1,-1,-1,"",""),


            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(0,35,15,false,PoliceVehicleType.MarkedValorLightbar,134,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(0,35,15,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(0,20,15,false,PoliceVehicleType.MarkedFlatLightbar,134,3,4,3,4,"",""),

            new DispatchableVehicle("polmavliv", 0, 100) { RequiredLiveries = new List<int>() { 9 }, MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
            new DispatchableVehicle("annihilatorliv", 0, 100) { RequiredLiveries = new List<int>() { 6 },MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 4 },
        };
        NOOSEPIAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        MarshalsServiceVehicles_FEJ_LC = new List<DispatchableVehicle>
        {
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(35, 35, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
        };
        MarshalsServiceVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
        DOAVehicles_FEJ_LC = new List<DispatchableVehicle>
        {
            DispatchableVehicles_FEJ.Create_PoliceInterceptorLC(35, 35, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceGresleyLC(35, 35, -1, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceBuffaloSTXLC(20,20,11,true,PoliceVehicleType.Unmarked,-1,-1,-1,-1,-1,"",""),
            DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
            DispatchableVehicles_FEJ.Create_PoliceAleutianLC(15, 15, 11, true, PoliceVehicleType.Unmarked, -1, -1, -1, -1, -1, "", ""),
        };
        DOAVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.USGovernmentPlates; });
    }
    private void Security()
    {
        

    }
    private void Taxis()
    {
        LCTaxiVehicles_FEJ_LC = new List<DispatchableVehicle>() {

            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi1,88,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi2,88,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi3,88,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,0,false,ServiceVehicleType.Taxi4,88,-1,-1),

            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi1,92,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi2,92,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi3,92,-1,-1),
            DispatchableVehicles_FEJ.Create_TaxiVivaniteLC(35,35,1,false,ServiceVehicleType.Taxi4,92,-1,-1),
        };
        LCTaxiVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = DispatchableVehicles_FEJ.LibertyPlates; });
    }

}

