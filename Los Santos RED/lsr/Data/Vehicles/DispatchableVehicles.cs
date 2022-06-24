using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DispatchableVehicles : IDispatchableVehicles
{

    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\DispatchableVehicles.xml";
    private List<DispatchableVehicleGroup> VehicleGroupLookup = new List<DispatchableVehicleGroup>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("DispatchableVehicles*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFileName);
        }
        else
        {
            DefaultConfig_Gresk();
            DefaultConfig();
        }
    }

    private void DefaultConfig_Gresk()
    {
        List<DispatchableVehicleGroup> VehicleGroupLookupGresk = new List<DispatchableVehicleGroup>();

        //Cops
        List<DispatchableVehicle> UnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        List<DispatchableVehicle> CoastGuardVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) };
        List<DispatchableVehicle> FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 }, };
        List<DispatchableVehicle> NOOSEVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police3", 70, 70){ RequiredLiveries = new List<int>() { 11 }, MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("sheriff2", 30, 30) { RequiredLiveries = new List<int>() { 11 },MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("police2", 0, 35) { RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("police2", 0, 40) { RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("police3", 0, 40) { RequiredLiveries = new List<int>() { 11 },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },



            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 4,MaxOccupants = 5 }};
        List<DispatchableVehicle> PrisonVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 70, 70),
            new DispatchableVehicle("police4", 30, 30) };
        List<DispatchableVehicle> LSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police3", 48,35) { RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle("police2", 25, 20){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("sheriff2", 15, 10){ RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle("police4", 1,1),
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3}};




        List<DispatchableVehicle> SAHPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("policeb", 25, 15) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("police2", 30, 45) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle("police3", 30, 30) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle("sheriff2", 15, 10) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } }
        };



        List<DispatchableVehicle> LSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50) {RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50) {RequiredLiveries = new List<int>() { 7 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() {7 } },};
        List<DispatchableVehicle> BCSOVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() {0 } }, };
        List<DispatchableVehicle> MajesticLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() { 8 } }, };
        List<DispatchableVehicle> VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle("sheriff2", 50, 50)  {RequiredLiveries = new List<int>() { 9 } }, };
        List<DispatchableVehicle> ChumashLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25)  {RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle("sheriff2", 50, 50)  {RequiredLiveries = new List<int>() { 10 } },};
        List<DispatchableVehicle> LSIAPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50)  {RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle("sheriff2", 25, 25)  {RequiredLiveries = new List<int>() { 12 } },};
        List<DispatchableVehicle> RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50){RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25){RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 5 } } };
        List<DispatchableVehicle> DPPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25){RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50){RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 6 } } };
        List<DispatchableVehicle> EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 33, 33){RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 33, 33){RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("sheriff2", 33, 33){RequiredLiveries = new List<int>() { 3 } } };
        List<DispatchableVehicle> VWPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25){RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50){RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 2 } }, };


        List<DispatchableVehicle> LSPPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50) {RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50)  {RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 13 } }, };



        List<DispatchableVehicle> PoliceHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("polmav", 0,100) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 } };
        List<DispatchableVehicle> SheriffHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 }, //};
         new DispatchableVehicle("valkyrie2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 } };
        List<DispatchableVehicle> ArmyVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("crusader", 85,90) { MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("barracks", 15,10) { MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("rhino", 0, 25) { MinOccupants = 2,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie2", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },};
        List<DispatchableVehicle> Firetrucks = new List<DispatchableVehicle>() {
            new DispatchableVehicle("firetruk", 100, 100) };
        List<DispatchableVehicle> Amublance1 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };
        List<DispatchableVehicle> Amublance2 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } } };
        List<DispatchableVehicle> Amublance3 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };

        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("BCSOVehicles", BCSOVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("LSIAPDVehicles", LSIAPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("LSPPVehicles", LSPPVehicles));



        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("VWHillsLSSDVehicles", VWHillsLSSDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("ChumashLSSDVehicles", ChumashLSSDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("MajesticLSSDVehicles", MajesticLSSDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("RHPDVehicles", RHPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("DPPDVehicles", DPPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("VWPDVehicles", VWPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("EastLSPDVehicles", EastLSPDVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("Firetrucks", Firetrucks));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("Amublance1", Amublance1));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("Amublance2", Amublance2));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("Amublance3", Amublance3));

        //Gangs
        List<DispatchableVehicle> GenericGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("tornado", 15, 15),};
        List<DispatchableVehicle> AllGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("buccaneer2", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("chino", 15, 15),
            new DispatchableVehicle("chino2", 15, 15),
            new DispatchableVehicle("faction", 15, 15),
            new DispatchableVehicle("faction2", 15, 15),
            new DispatchableVehicle("primo", 15, 15),
            new DispatchableVehicle("primo2", 15, 15),
            new DispatchableVehicle("voodoo", 15, 15),
            new DispatchableVehicle("voodoo2", 15, 15),
        };
        List<DispatchableVehicle> LostMCVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },};
        List<DispatchableVehicle> VarriosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 50, 50){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42},
            new DispatchableVehicle("buccaneer2", 50, 50){RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },//yellow
        };
        List<DispatchableVehicle> BallasVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purp[le
        };
        List<DispatchableVehicle> VagosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("chino", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },
            new DispatchableVehicle("chino2", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },//orange
        };
        List<DispatchableVehicle> MarabuntaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("faction", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },
            new DispatchableVehicle("faction2", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },//blue
        };
        List<DispatchableVehicle> KoreanVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("feltzer2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("comet2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("dubsta2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        List<DispatchableVehicle> TriadVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("oracle", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
           // new DispatchableVehicle("cavalcade", 33, 33){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
        };
        List<DispatchableVehicle> YardieVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("virgo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo2", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
        };
        List<DispatchableVehicle> DiablosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("stalion", 100, 100){ RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28 },//red
        };
        List<DispatchableVehicle> MafiaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> ArmeniaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("schafter2", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> CartelVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

        };
        List<DispatchableVehicle> RedneckVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rumpo", 33, 33),
            new DispatchableVehicle("bison", 33, 33),
            new DispatchableVehicle("sanchez2",33,33) {MaxOccupants = 1 },
        };
        List<DispatchableVehicle> FamiliesVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("emperor ",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("peyote ",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("nemesis",15,15) {MaxOccupants = 1 },
            new DispatchableVehicle("buccaneer",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("manana",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("tornado",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
        };

        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles));

        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("BallasVehicles", BallasVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("VagosVehicles", VagosVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("TriadVehicles", TriadVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("YardieVehicles", YardieVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("CartelVehicles", CartelVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles));
        VehicleGroupLookupGresk.Add(new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles));


        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
        Serialization.SerializeParams(VehicleGroupLookupGresk, "Plugins\\LosSantosRED\\AlternateConfigs\\DispatchableVehicles_Gresk.xml");
    }
    private void DefaultConfig()
    {
        VehicleGroupLookup = new List<DispatchableVehicleGroup>();

        //Cops
        List<DispatchableVehicle> UnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        List<DispatchableVehicle> CoastGuardVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) };
        List<DispatchableVehicle> FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 }, };
        List<DispatchableVehicle> NOOSEVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 35) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 40) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 4,MaxOccupants = 5 }};
        List<DispatchableVehicle> HighwayPatrolVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 70, 70) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop" },
            new DispatchableVehicle("police4", 30, 30) };
        List<DispatchableVehicle> PrisonVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 70, 70),
            new DispatchableVehicle("police4", 30, 30) };
        List<DispatchableVehicle> LSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 48,35) { RequiredLiveries = new List<int>() { 0,1,2,3,4,5 } },
            new DispatchableVehicle("police2", 25, 20),
            new DispatchableVehicle("police4", 1,1),
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3}};
        List<DispatchableVehicle> SAHPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 70, 70) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop" },
            new DispatchableVehicle("police4", 30, 30) {RequiredPedGroup = "StandardSAHP" }  };
        List<DispatchableVehicle> LSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 50, 50),
            new DispatchableVehicle("sheriff2", 50, 50) };
        List<DispatchableVehicle> BCSOVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 50, 50),
            new DispatchableVehicle("sheriff2", 50, 50) };
        List<DispatchableVehicle> VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) };
        List<DispatchableVehicle> ChumashLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) };
        List<DispatchableVehicle> RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police2", 100, 75),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> DPPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police3", 100, 75) ,
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> VWPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> PoliceHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("polmav", 0,100) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 4 } };
        List<DispatchableVehicle> SheriffHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 }, //};
         new DispatchableVehicle("valkyrie2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 } };
        List<DispatchableVehicle> ArmyVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("crusader", 85,90) { MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("barracks", 15,10) { MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("rhino", 0, 25) { MinOccupants = 2,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie2", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },};
        List<DispatchableVehicle> Firetrucks = new List<DispatchableVehicle>() {
            new DispatchableVehicle("firetruk", 100, 100) };
        List<DispatchableVehicle> Amublance1 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };
        List<DispatchableVehicle> Amublance2 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } } };
        List<DispatchableVehicle> Amublance3 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };

        VehicleGroupLookup.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("BCSOVehicles", BCSOVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("VWHillsLSSDVehicles", VWHillsLSSDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("ChumashLSSDVehicles", ChumashLSSDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("RHPDVehicles", RHPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("DPPDVehicles", DPPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("VWPDVehicles", VWPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("EastLSPDVehicles", EastLSPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("Firetrucks", Firetrucks));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("Amublance1", Amublance1));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("Amublance2", Amublance2));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("Amublance3", Amublance3));

        //Gangs
        List<DispatchableVehicle> GenericGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("tornado", 15, 15),};
        List<DispatchableVehicle> AllGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("buccaneer2", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("chino", 15, 15),
            new DispatchableVehicle("chino2", 15, 15),
            new DispatchableVehicle("faction", 15, 15),
            new DispatchableVehicle("faction2", 15, 15),
            new DispatchableVehicle("primo", 15, 15),
            new DispatchableVehicle("primo2", 15, 15),
            new DispatchableVehicle("voodoo", 15, 15),
            new DispatchableVehicle("voodoo2", 15, 15),
        };
        List<DispatchableVehicle> LostMCVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },};
        List<DispatchableVehicle> VarriosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 50, 50){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42},
            new DispatchableVehicle("buccaneer2", 50, 50){RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },//yellow
        };
        List<DispatchableVehicle> BallasVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purp[le
        };
        List<DispatchableVehicle> VagosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("chino", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },
            new DispatchableVehicle("chino2", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },//orange
        };
        List<DispatchableVehicle> MarabuntaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("faction", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },
            new DispatchableVehicle("faction2", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },//blue
        };
        List<DispatchableVehicle> KoreanVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("feltzer2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("comet2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("dubsta2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        List<DispatchableVehicle> TriadVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("oracle", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
           // new DispatchableVehicle("cavalcade", 33, 33){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
        };
        List<DispatchableVehicle> YardieVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("virgo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo2", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
        };
        List<DispatchableVehicle> DiablosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("stalion", 100, 100){ RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28 },//red
        };
        List<DispatchableVehicle> MafiaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> ArmeniaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("schafter2", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> CartelVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

        };
        List<DispatchableVehicle> RedneckVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rumpo", 33, 33),
            new DispatchableVehicle("bison", 33, 33),
            new DispatchableVehicle("sanchez2",33,33) {MaxOccupants = 1 },
        };
        List<DispatchableVehicle> FamiliesVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("emperor ",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("peyote ",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("nemesis",15,15) {MaxOccupants = 1 },
            new DispatchableVehicle("buccaneer",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("manana",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("tornado",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
        };

        VehicleGroupLookup.Add(new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles));

        VehicleGroupLookup.Add(new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("BallasVehicles", BallasVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("VagosVehicles", VagosVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("TriadVehicles", TriadVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("YardieVehicles", YardieVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("CartelVehicles", CartelVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles));

        Serialization.SerializeParams(VehicleGroupLookup, ConfigFileName);

    }

    public List<DispatchableVehicle> GetVehicleData(string dispatchableVehicleGroupID)
    {
        return VehicleGroupLookup.FirstOrDefault(x => x.DispatchableVehicleGroupID == dispatchableVehicleGroupID)?.DispatchableVehicles;
    }
}


