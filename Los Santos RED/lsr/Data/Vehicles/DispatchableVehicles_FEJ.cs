using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
    private void LocalPolice()
    {
        UnmarkedVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle(StanierUnmarked, 50, 50),
            new DispatchableVehicle(PoliceFugitive, 50,50){ RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(BuffaloUnmarked, 50, 50){ OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(GrangerUnmarked, 50, 50) { OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(WashingtonUnmarked,30,30) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,40) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, }
        };

        LSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceStanier, 25,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 48,35) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 15, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2,true,80) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,CaninePossibleSeats = new List<int>{ 1 }, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceFugitive, 10,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            new DispatchableVehicle(WashingtonUnmarked,1,1) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,40) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
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
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 15, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } }, };
        EastLSPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 35,35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        VWPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 30,25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGresley, 50, 50){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 }, },
            new DispatchableVehicle(PoliceFugitive, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 0 } },};
        RHPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 50, 50) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
        DPPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 2,2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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


        NYSPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,20){  RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGresley, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceGranger, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },
            new DispatchableVehicle(PoliceBison, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 16 } },  };
        LCPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceTorrence, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceGresley, 48,35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(PoliceBuffalo, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceGranger, 15, 12){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 15 } },
            new DispatchableVehicle(StanierUnmarked, 1,1),
            new DispatchableVehicle(WashingtonUnmarked,1,1) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,40) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(GrangerUnmarked, 1,1),
            new DispatchableVehicle(PoliceBike, 15, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 5 } },
        };
    }
    private void LocalSheriff()
    {
        //Sheriff
        BCSOVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 25, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 10, 10) {GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 2 } },};
        LSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 20, 15){RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 20, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceGranger, 50, 50) {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle(PoliceBison, 10, 10)  {RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 }, },
            new DispatchableVehicle(PoliceFugitive, 1, 1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 25, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGresley, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceGranger, 20, 20)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle(PoliceBison, 7, 7)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 }, },
            new DispatchableVehicle(PoliceFugitive, 2, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 20, 10) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
        DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 55, 55){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 15, 15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f, RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
            new DispatchableVehicle(PoliceTorrence, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGresley, 10, 10)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle(PoliceGranger, 15, 15)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceBison, 5, 5)  { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 }, },
            new DispatchableVehicle(PoliceFugitive, 1,1){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 10.0f,RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 15, 5) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 3 } },};
    }
    private void StatePolice()
    {
        //Other State
        LSIAPDVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5,5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 15, 15) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
        LSPPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceStanier, 25, 25){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 5){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 5, 5) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBike, 10, 5) { GroupName = "Motorcycle",MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 4 } },};
        //State
        SAHPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceStanier, 20,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP", RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(1, true, 50), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBike, 45, 20) { GroupName = "Motorcycle", MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2, RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } ,VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(1, true, 50) } },
            new DispatchableVehicle(PoliceBuffaloS, 45, 55) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111, GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },
            
            //buffalos unmarked too
            new DispatchableVehicle(PoliceBuffaloS, 10, 0) {  GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },

            new DispatchableVehicle(PoliceTorrence, 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGresley, 15, 15) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceGranger, 5, 5) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(PoliceBison, 5, 1) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            DispatchableVehicles.GauntletUndercoverSAHP,
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
            new DispatchableVehicle(PoliceFugitive, 10,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,GroupName = "StandardSAHP",RequiredPedGroup = "StandardSAHP", RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
        PrisonVehicles_FEJ = new List<DispatchableVehicle>()
        {
            //new DispatchableVehicle(PoliceTransporter, 0, 25),
            new DispatchableVehicle(PoliceStanier, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceMerit, 5, 2) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceTorrence, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGresley, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceGranger, 25, 25) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceBison, 5, 0) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle(PoliceFugitive, 1, 0){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },};
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
            new DispatchableVehicle(PoliceTransporter, 5, 10) { RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100) } },
            new DispatchableVehicle(WashingtonUnmarked,5,10) { RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,40) },OptionalColors = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,37,38,54,61,62,63,64,65,66,67,68,69,94,95,96,97,98,99,100,101,201,103,104,105,106,107,111,112 }, },
            new DispatchableVehicle(StanierUnmarked, 5,10) { RequiredPrimaryColorID = 1, RequiredSecondaryColorID = 1,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, },
            new DispatchableVehicle(BuffaloUnmarked, 30, 30){ MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(GrangerUnmarked, 30, 30) { MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle(PoliceFugitive, 30,30){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100), new DispatchableVehicleExtra(10, false, 100), new DispatchableVehicleExtra(12, false, 100) } },

            
            
            new DispatchableVehicle(PoliceTransporter, 0, 35) { RequiredPrimaryColorID = 1,RequiredSecondaryColorID = 1, MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 5,MaxWantedLevelSpawn = 5,  RequiredPedGroup = "FIBHET", RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100) }},
            new DispatchableVehicle(GrangerUnmarked, 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(BuffaloUnmarked, 0, 20) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle(PoliceBuffaloS, 0, 35) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 16 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, false, 100) }, },
            new DispatchableVehicle("frogger2", 0, 30) { MinWantedLevelSpawn = 5, MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHET",MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 0 } }, };
        NOOSEVehicles_FEJ = new List<DispatchableVehicle>()
        {
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

        BorderPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },
            new DispatchableVehicle(PoliceStanier, 20,20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 20, 20){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 20, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 15 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50), new DispatchableVehicleExtra(2, true, 80) } },
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

            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 4, RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },

            new DispatchableVehicle(PoliceBison, 35, 35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MaxRandomDirtLevel = 15.0f,RequiredLiveries = new List<int>() { 19 } },
        };
        NOOSEPIAVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },
            new DispatchableVehicle(PoliceStanier, 15,10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 35, 35){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 17 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 35, 35) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
            new DispatchableVehicle(PoliceBuffaloS, 0, 40) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 17 }, },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 4, RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};
        NOOSESEPVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(PoliceTransporter, 2, 0) { RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },
            new DispatchableVehicle(PoliceStanier, 15,15){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle(PoliceBuffalo, 10, 10){ RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3, RequiredLiveries = new List<int>() { 18 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle(PoliceBuffaloS, 10, 10) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 0, MaxWantedLevelSpawn = 3,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 80) } },
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
            new DispatchableVehicle(PoliceBuffaloS, 25, 20) { RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4,RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle(PoliceTorrence, 0, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceGresley, 0, 40) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0,MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4, RequiredLiveries = new List<int>() { 18 }, },
            new DispatchableVehicle(PoliceTransporter, 0, 35) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 4, RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,50) } },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, MinOccupants = 4, MaxOccupants = 5 }};
    }
    private void OtherPolice()
    {
        MarshalsServiceVehicles_FEJ = DispatchableVehicles.MarshalsServiceVehicles.Copy();//for now
        MarshalsServiceVehicles_FEJ.Add(new DispatchableVehicle(PoliceBuffaloS, 25, 25) { RequiredLiveries = new List<int>() { 16 }, VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, false, 100) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
        MarshalsServiceVehicles_FEJ.Add(new DispatchableVehicle(WashingtonUnmarked, 25, 25) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, true, 40) }, OptionalColors = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 37, 38, 54, 61, 62, 63, 64, 65, 66, 67, 68, 69, 94, 95, 96, 97, 98, 99, 100, 101, 201, 103, 104, 105, 106, 107, 111, 112 }, });
    }
    private void Security()
    {
        MerryweatherPatrolVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 5 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 35, 35) {  RequiredLiveries = new List<int>() { 1 } },
            DispatchableVehicles.AleutianSecurityMW,
            DispatchableVehicles.AsteropeSecurityMW,
        };
        BobcatSecurityVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 8 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 3 } },
            DispatchableVehicles.AleutianSecurityBobCat,
            DispatchableVehicles.AsteropeSecurityBobCat,
        };
        GroupSechsVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 7 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 0 } },
            DispatchableVehicles.AleutianSecurityG6,
            DispatchableVehicles.AsteropeSecurityG6,
        };
        SecuroservVehicles_FEJ = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle(ServiceDilettante, 20, 20)
            {
                RequiredLiveries = new List<int>() { 6 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,false,100),
                }
            },
            new DispatchableVehicle(SecurityTorrence, 20, 20){  RequiredLiveries = new List<int>() { 4 } },
            DispatchableVehicles.AleutianSecuritySECURO,
            DispatchableVehicles.AsteropeSecuritySECURO,
        };
    }
    private void Taxis()
    {
        DowntownTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 0 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 1 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            DispatchableVehicles.TaxiBroadWay,
            DispatchableVehicles.TaxiEudora,
        };
        PurpleTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 2 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        HellTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 0 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        ShitiTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 3 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
        SunderedTaxiVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("taxi", 35, 35){ RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,true,100),
                    new DispatchableVehicleExtra(8,false,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,true,100),
                    new DispatchableVehicleExtra(9,true,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,true,100),
                    new DispatchableVehicleExtra(6,false,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
            new DispatchableVehicle(ServiceDilettante, 35, 35)
            {
                RequiredLiveries = new List<int>() { 4 },
                VehicleExtras = new List<DispatchableVehicleExtra>()
                {
                    new DispatchableVehicleExtra(5,false,100),
                    new DispatchableVehicleExtra(6,true,100),
                    new DispatchableVehicleExtra(7,false,100),
                    new DispatchableVehicleExtra(8,false,50),
                    new DispatchableVehicleExtra(9,false,100),
                    new DispatchableVehicleExtra(12,true,100),
                }
            },
        };
    }
}

