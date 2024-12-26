using System.Collections.Generic;


public class DispatchableVehicles_LPP
{
    private DispatchableVehicles DispatchableVehicles;

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


    public List<string> LibertyPlates { get; private set; } = new List<string>() { "Liberty", };
    public List<string> LibertyPolicePlates { get; private set; } = new List<string>() { "Liberty Police", "Liberty", };

    public List<string> AlderneyPlates { get; private set; } = new List<string>() { "Alderney", "Liberty", };
    public List<string> NorthYanktonPlates { get; private set; } = new List<string>() { "North Yankton", };

    public List<string> USGovernmentPlates { get; private set; } = new List<string>() { "US Gov", };
    public DispatchableVehicles_LPP(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }
    public void DefaultConfig()
    {

        LocalSheriff();
        Taxis();
        Fire();
        EMT();
    }
    private void Fire()
    {
        FDLCVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fdlcheavy", 100, 100) { MinOccupants = 2, MaxOccupants = 4 },
            new DispatchableVehicle("fdlcladder", 100, 100) { MinOccupants = 2, MaxOccupants = 4 },
            new DispatchableVehicle("fdlctruck", 100, 100) { MinOccupants = 2, MaxOccupants = 4 },
            new DispatchableVehicle("fdlctruck2", 100, 100) { MinOccupants = 2, MaxOccupants = 4 },
        };
        FDLCVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = LibertyPolicePlates; });
    }
    private void EMT()
    {
        FDLCEMTVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fdlcamb", 100, 100),
            new DispatchableVehicle("fdlcamb2", 100, 100),
        };
        HMSVehicles_FEJ_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("fdlcamb", 100, 100) {  }
        };
        FDLCEMTVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = LibertyPolicePlates; });
        HMSVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = LibertyPolicePlates; });
    }
    private void LocalSheriff()
    {

        //DispatchableVehicle LCPDk9 = DispatchableVehicles_FEJ.Create_PoliceGranger3600LC(0, 50, 5, false, PoliceVehicleType.MarkedValorLightbar, 134, 0, 3, 1, 2, "", "");
        //LCPDk9.CaninePossibleSeats = new List<int> { 1, 2 };
        //LCPDk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 200) };

        LCPDVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lcpdscout",100,100),//scout
            new DispatchableVehicle("lcpd3",75,75),//police3
            new DispatchableVehicle("lcpdspeedo",15,15),//speedo van
            new DispatchableVehicle("lcpdalamo",15,15),//tahoe from 2013
            new DispatchableVehicle("lcpd5",15,15),//crown vic from 2013
            new DispatchableVehicle("lcpd4",15,15),//buffalo from 2013
            //WEird
            new DispatchableVehicle("lcpdsand",1,1),//sandstorm truck thing
            new DispatchableVehicle("lcpdpatriot",1,1),//patriot
            new DispatchableVehicle("lcpdold",1,1),//patriot
            new DispatchableVehicle("lcpd",1,1),//patriot
            new DispatchableVehicle("lcpd2",1,1),//patriot
            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3,RequiredPedGroup = "Detective" },


            new DispatchableVehicle("lcpdb", 10, 5) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop", GroupName = "Motorcycle" },

        };

        foreach (DispatchableVehicle dv in LCPDVehicles)
        {
            dv.RequestedPlateTypes = LibertyPolicePlates;
        }

        //DispatchableVehicle ASPk9 = DispatchableVehicles_FEJ.Create_PoliceAleutianLC(0, 50, 5, false, PoliceVehicleType.Marked, 134, 0, 3, 1, 2, "", "");
        //ASPk9.CaninePossibleSeats = new List<int> { 1, 2 };
        //ASPk9.SpawnAdjustmentAmounts = new List<SpawnAdjustmentAmount>() { new SpawnAdjustmentAmount(eSpawnAdjustment.K9, 200) };

        ASPVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lcpdscout",100,100),//scout
            new DispatchableVehicle("lcpd3",75,75),//police3
            new DispatchableVehicle("lcpdspeedo",15,15),//speedo van
            new DispatchableVehicle("lcpdalamo",15,15),//tahoe from 2013
            new DispatchableVehicle("lcpd5",15,15),//crown vic from 2013
            new DispatchableVehicle("lcpd4",15,15),//buffalo from 2013
            //WEird
            new DispatchableVehicle("lcpdsand",1,1),//sandstorm truck thing
            new DispatchableVehicle("lcpdpatriot",1,1),//patriot
            new DispatchableVehicle("lcpdold",1,1),//patriot
            new DispatchableVehicle("lcpd",1,1),//patriot
            new DispatchableVehicle("lcpd2",1,1),//patriot
            new DispatchableVehicle("police4", 2, 2) { MaxWantedLevelSpawn = 3,RequiredPedGroup = "Detective" },


            new DispatchableVehicle("lcpdb", 10, 5) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop", GroupName = "Motorcycle" },
        };
        foreach (DispatchableVehicle dv in ASPVehicles)
        {
            dv.RequestedPlateTypes = AlderneyPlates;
        }


        LCPDHeliVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("lcpdmav", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
            new DispatchableVehicle("lcpdsparrow", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        ASPHeliVehicles = new List<DispatchableVehicle>()
        {
             new DispatchableVehicle("buzzard2", 1,100) { RequiredPedGroup = "Pilot",RequiredGroupIsDriverOnly = true, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 },
        };

        CoastGuardVehicles_LC = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dinghy5", 50, 50) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 1,MaxOccupants = 2, MaxWantedLevelSpawn = 2, },
            new DispatchableVehicle("dinghy5", 0, 100) { FirstPassengerIndex = 3, RequiredPrimaryColorID = 38, RequiredSecondaryColorID = 0, ForceStayInSeats = new List<int>() { 3 }, MinOccupants = 2,MaxOccupants = 4, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, },
            new DispatchableVehicle("seashark2", 50, 50) { RequiredLiveries = new List<int>() { 2,3 }, MaxOccupants = 1 },
            new DispatchableVehicle("polmavliv", 1,30) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 4 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
            new DispatchableVehicle("annihilatorliv", 1,100) { GroupName = "Helicopter",RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 0,MaxWantedLevelSpawn = 4,MinOccupants = 2,MaxOccupants = 3 },
        };
        CoastGuardVehicles_LC.ForEach(x => { x.RequestedPlateTypes = USGovernmentPlates; });
    }
    private void Taxis()
    {
        LCTaxiVehicles_FEJ_LC = new List<DispatchableVehicle>() {

            new DispatchableVehicle("lctaxi",35,35),
            new DispatchableVehicle("lctaxi2",35,35),
            new DispatchableVehicle("lctaxi4",35,35),
            new DispatchableVehicle("lctaxiold",2,2),
        };
        LCTaxiVehicles_FEJ_LC.ForEach(x => { x.RequestedPlateTypes = LibertyPlates; });
    }

}

