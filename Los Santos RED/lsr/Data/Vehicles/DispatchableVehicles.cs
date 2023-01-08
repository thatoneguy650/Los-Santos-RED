﻿using LosSantosRED.lsr.Helper;
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
    private List<DispatchableVehicle> GenericGangVehicles;
    private List<DispatchableVehicle> AllGangVehicles;
    private List<DispatchableVehicle> LostMCVehicles;
    private List<DispatchableVehicle> VarriosVehicles;
    private List<DispatchableVehicle> BallasVehicles;
    private List<DispatchableVehicle> VagosVehicles;
    private List<DispatchableVehicle> MarabuntaVehicles;
    private List<DispatchableVehicle> KoreanVehicles;
    private List<DispatchableVehicle> TriadVehicles;
    private List<DispatchableVehicle> YardieVehicles;
    private List<DispatchableVehicle> DiablosVehicles;
    private List<DispatchableVehicle> MafiaVehicles;
    private List<DispatchableVehicle> ArmeniaVehicles;
    private List<DispatchableVehicle> CartelVehicles;
    private List<DispatchableVehicle> RedneckVehicles;
    private List<DispatchableVehicle> FamiliesVehicles;
    private List<DispatchableVehicle> UnmarkedVehicles;
    private List<DispatchableVehicle> CoastGuardVehicles;
    private List<DispatchableVehicle> ParkRangerVehicles;
    private List<DispatchableVehicle> FIBVehicles;
    private List<DispatchableVehicle> NOOSEVehicles;
    private List<DispatchableVehicle> PrisonVehicles;
    private List<DispatchableVehicle> LSPDVehicles;
    private List<DispatchableVehicle> SAHPVehicles;
    private List<DispatchableVehicle> LSSDVehicles;
    private List<DispatchableVehicle> BCSOVehicles;
    private List<DispatchableVehicle> VWHillsLSSDVehicles;
    private List<DispatchableVehicle> DavisLSSDVehicles;
    private List<DispatchableVehicle> RHPDVehicles;
    private List<DispatchableVehicle> DPPDVehicles;
    private List<DispatchableVehicle> EastLSPDVehicles;
    private List<DispatchableVehicle> VWPDVehicles;
    private List<DispatchableVehicle> PoliceHeliVehicles;
    private List<DispatchableVehicle> SheriffHeliVehicles;
    private List<DispatchableVehicle> ArmyVehicles;
    private List<DispatchableVehicle> Firetrucks;
    private List<DispatchableVehicle> Amublance1;
    private List<DispatchableVehicle> Amublance2;
    private List<DispatchableVehicle> Amublance3;
    private List<DispatchableVehicle> NYSPVehicles;

    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("DispatchableVehicles*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable Vehicles config: {ConfigFile.FullName}", 0);
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Dispatchable Vehicles config  {ConfigFileName}", 0);
            VehicleGroupLookup = Serialization.DeserializeParams<DispatchableVehicleGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Dispatchable Vehicles config found, creating default", 0);
            SetupDefaults();
            DefaultConfig_Simple();
            DefaultConfig_LosSantos2008();
            DefaultConfig_FullExpandedJurisdiction();
            DefaultConfig();
        }
    }
    public List<DispatchableVehicle> GetVehicleData(string dispatchableVehicleGroupID)
    {
        return VehicleGroupLookup.FirstOrDefault(x => x.DispatchableVehicleGroupID == dispatchableVehicleGroupID)?.DispatchableVehicles;
    }
    private void SetupDefaults()
    {
        //Cops
        UnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        CoastGuardVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) };
        FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 4, MaxOccupants = 4 }, };
        NOOSEVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 35) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 40) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 4,MaxOccupants = 5 }};
        PrisonVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 70, 70),
            new DispatchableVehicle("police4", 30, 30) };
        LSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 48,35) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 20),
            new DispatchableVehicle("police4", 1,1) { RequiredPedGroup = "Detectives" },
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4, MinWantedLevelSpawn = 3}};
        SAHPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 70, 70) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop" },
            new DispatchableVehicle("police4", 30, 30) {RequiredPedGroup = "StandardSAHP" }  };
        LSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 50, 50) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) };
        BCSOVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 50, 50) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("sheriff2", 50, 50) };
        VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) };
        DavisLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) };
        RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police2", 100, 75){ VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,25) } },
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        DPPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police3", 100, 75) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,75) } },
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        VWPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        PoliceHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("polmav", 0,100) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 } };
        SheriffHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 }, //};
         new DispatchableVehicle("valkyrie2", 0,50) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 4,MaxOccupants = 4 } };
        ArmyVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("crusader", 85,90) { MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("barracks", 15,10) { MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("rhino", 0, 25) { MinOccupants = 2,MaxOccupants = 2,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },
            new DispatchableVehicle("valkyrie2", 0,50) { MinOccupants = 4,MaxOccupants = 4,MinWantedLevelSpawn = 6 },};
        Firetrucks = new List<DispatchableVehicle>() {
            new DispatchableVehicle("firetruk", 100, 100) };
        Amublance1 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };
        Amublance2 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } } };
        Amublance3 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };
        NYSPVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeold1", 50, 50),
            new DispatchableVehicle("policeold2", 50, 50), };

        //Gangs
        GenericGangVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 15, 15),
            new DispatchableVehicle("manana", 15, 15),
            new DispatchableVehicle("tornado", 15, 15),};
        AllGangVehicles = new List<DispatchableVehicle>() {
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
        LostMCVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },};
        VarriosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buccaneer", 50, 50){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42},
            new DispatchableVehicle("buccaneer2", 50, 50){RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },//yellow
        };
        BallasVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purp[le
        };
        VagosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("chino", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },
            new DispatchableVehicle("chino2", 50, 50){ RequiredPrimaryColorID = 38,RequiredSecondaryColorID = 38 },//orange
        };
        MarabuntaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("faction", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },
            new DispatchableVehicle("faction2", 50, 50){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },//blue
        };
        KoreanVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("feltzer2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("comet2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("dubsta2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        TriadVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("washington", 50, 50){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
           // new DispatchableVehicle("cavalcade", 33, 33){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
        };
        YardieVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("virgo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo2", 33, 33){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
        };
        DiablosVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("stalion", 100, 100){ RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28 },//red
        };
        MafiaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sentinel", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        ArmeniaVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("schafter2", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        CartelVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade2", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

        };
        RedneckVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rumpo", 33, 33),
            new DispatchableVehicle("bison", 33, 33),
            new DispatchableVehicle("sanchez2",33,33) {MaxOccupants = 1 },
        };
        FamiliesVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("emperor",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("peyote",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("nemesis",15,15) {MaxOccupants = 1 },
            new DispatchableVehicle("buccaneer",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("manana",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("tornado",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
        };
    }
    private void DefaultConfig()
    {
        VehicleGroupLookup = new List<DispatchableVehicleGroup>();
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
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("DavisLSSDVehicles", DavisLSSDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("MajesticLSSDVehicles", VWHillsLSSDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("LSPPVehicles", RHPDVehicles));
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("LSIAPDVehicles", RHPDVehicles));
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
        VehicleGroupLookup.Add(new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles));

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
        Serialization.SerializeParams(VehicleGroupLookup, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\DispatchableVehicles_EUP.xml");
    }
    public void DefaultConfig_LosSantos2008()
    {
        List<DispatchableVehicleGroup> OldVehicleLookupGroup = new List<DispatchableVehicleGroup>();
        List<DispatchableVehicle> LSPDVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 85,85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1, false, 100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("police2", 15, 15),
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> LSSDVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 85, 85) { VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,false,100), new DispatchableVehicleExtra(2, true, 100) } },
            new DispatchableVehicle("sheriff2", 15, 15), };
        List<DispatchableVehicle> BallasVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("baller", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("patriot", 50, 50){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purp[le
        };
        List<DispatchableVehicle> KoreanVehicles_old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fq2", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("prairie", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("oracle", 33, 33){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
        };
        List<DispatchableVehicle> MafiaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fugitive", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("washington", 50, 50) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> ArmeniaVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("rocoto", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
        };
        List<DispatchableVehicle> CartelVehicles_Old = new List<DispatchableVehicle>() {
            new DispatchableVehicle("cavalcade", 100, 100) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

        };

        //Cop
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("BCSOVehicles", LSSDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("VWHillsLSSDVehicles", LSSDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("DavisLSSDVehicles", LSSDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("MajesticLSSDVehicles", LSSDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("LSPPVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("LSIAPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("RHPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("DPPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("VWPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("EastLSPDVehicles", LSPDVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("Firetrucks", Firetrucks));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("Amublance1", Amublance1));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("Amublance2", Amublance2));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("Amublance3", Amublance3));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles));

        //Gang
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("BallasVehicles", BallasVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("VagosVehicles", VagosVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles_old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("TriadVehicles", TriadVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("YardieVehicles", YardieVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("CartelVehicles", CartelVehicles_Old));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles));
        OldVehicleLookupGroup.Add(new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles));
        Serialization.SerializeParams(OldVehicleLookupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\DispatchableVehicles_LosSantos2008.xml");

    }
    private void DefaultConfig_Simple()
    {
        List<DispatchableVehicleGroup> SimpleVehicleLoopupGroup = new List<DispatchableVehicleGroup>();

        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("Firetrucks", Firetrucks));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("Amublance1", Amublance1));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("Amublance2", Amublance2));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("Amublance3", Amublance3));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles));

        //Gang
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("BallasVehicles", BallasVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("VagosVehicles", VagosVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("TriadVehicles", TriadVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("CartelVehicles", CartelVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles));
        SimpleVehicleLoopupGroup.Add(new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles));
        Serialization.SerializeParams(SimpleVehicleLoopupGroup, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\DispatchableVehicles_Simple.xml");
    }
    private void DefaultConfig_FullExpandedJurisdiction()
    {
        List<DispatchableVehicleGroup> VehicleGroupLookupFEJ = new List<DispatchableVehicleGroup>();

        //Cops
        List<DispatchableVehicle> UnmarkedVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        List<DispatchableVehicle> CoastGuardVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> ParkRangerVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) };
        List<DispatchableVehicle> FIBVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("fbi", 0, 70) { MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("frogger2", 0, 30) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 5 ,MaxWantedLevelSpawn = 5, RequiredPedGroup = "FIBHRT",MinOccupants = 3, MaxOccupants = 4 }, };
        List<DispatchableVehicle> NOOSEVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police3", 70, 70){ RequiredLiveries = new List<int>() { 11 }, MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            //new DispatchableVehicle("police5", 70, 70){ RequiredLiveries = new List<int>() { 11 }, MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("sheriff2", 30, 30) { RequiredLiveries = new List<int>() { 11 },MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("police2", 0, 35) { RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("riot", 0, 25) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("police2", 0, 40) { RequiredLiveries = new List<int>() { 11 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("police3", 0, 40) { RequiredLiveries = new List<int>() { 11 },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },
            new DispatchableVehicle("police5", 0, 40) { RequiredLiveries = new List<int>() { 11 },MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 3, MaxOccupants = 4 },


            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5,MinOccupants = 4,MaxOccupants = 5 }};
        List<DispatchableVehicle> PrisonVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 0, 25),
            new DispatchableVehicle("police", 25, 25) {RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 14 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() { 14 } },
            //new DispatchableVehicle("police5", 25, 25) {RequiredLiveries = new List<int>() { 14 } },
            new DispatchableVehicle("sheriff2", 25, 25) {RequiredLiveries = new List<int>() { 14 } },


        };
        List<DispatchableVehicle> LSPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police3", 48,35) { RequiredLiveries = new List<int>() { 1 } },
            //new DispatchableVehicle("police5", 48,35) { RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle("police2", 25, 20){ RequiredLiveries = new List<int>() { 1 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("sheriff2", 15, 10){ RequiredLiveries = new List<int>() { 1 } },
            new DispatchableVehicle("police4", 1,1),
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 25) { MinOccupants = 3, MaxOccupants = 4,MinWantedLevelSpawn = 3}};
        List<DispatchableVehicle> SAHPVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("policeb", 25, 15) { MaxOccupants = 1, RequiredPedGroup = "MotorcycleCop",MaxWantedLevelSpawn = 2 },
            new DispatchableVehicle("police2", 30, 45) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle("police3", 30, 30) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            //new DispatchableVehicle("police5", 30, 30) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } },
            new DispatchableVehicle("sheriff2", 15, 10) {RequiredPedGroup = "StandardSAHP",RequiredLiveries = new List<int>() { 4 } }
        };
        List<DispatchableVehicle> LSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50) {RequiredLiveries = new List<int>() { 7 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50) {RequiredLiveries = new List<int>() { 7 } },
            //new DispatchableVehicle("police5", 50, 50) {RequiredLiveries = new List<int>() { 7 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() {7 } },};
        List<DispatchableVehicle> BCSOVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() {0 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() {0 } },
            //new DispatchableVehicle("police5", 25, 25) {RequiredLiveries = new List<int>() {0 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() {0 } }, };
        List<DispatchableVehicle> MajesticLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 8 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() { 8 } },
            //new DispatchableVehicle("police5", 25, 25) {RequiredLiveries = new List<int>() { 8 } },
            new DispatchableVehicle("sheriff2", 50, 50) {RequiredLiveries = new List<int>() { 8 } }, };
        List<DispatchableVehicle> VWHillsLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 9 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25) {RequiredLiveries = new List<int>() { 9 } },
             new DispatchableVehicle("police5", 25, 25) {RequiredLiveries = new List<int>() { 9 } },
            new DispatchableVehicle("sheriff2", 50, 50)  {RequiredLiveries = new List<int>() { 9 } }, };
        List<DispatchableVehicle> DavisLSSDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 10 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25)  {RequiredLiveries = new List<int>() { 10 } },
            //new DispatchableVehicle("police5", 25, 25)  {RequiredLiveries = new List<int>() { 10 } },
            new DispatchableVehicle("sheriff2", 50, 50)  {RequiredLiveries = new List<int>() { 10 } },};
        List<DispatchableVehicle> LSIAPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25) {RequiredLiveries = new List<int>() { 12 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50)  {RequiredLiveries = new List<int>() { 12 } },
            //new DispatchableVehicle("police5", 50, 50)  {RequiredLiveries = new List<int>() { 12 } },
            new DispatchableVehicle("sheriff2", 25, 25)  {RequiredLiveries = new List<int>() { 12 } },};
        List<DispatchableVehicle> RHPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50){RequiredLiveries = new List<int>() { 5 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 25, 25){RequiredLiveries = new List<int>() { 5 } },
            //new DispatchableVehicle("police5", 25, 25){RequiredLiveries = new List<int>() { 5 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 5 } } };
        List<DispatchableVehicle> DPPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25){RequiredLiveries = new List<int>() { 6 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50){RequiredLiveries = new List<int>() { 6 } },
            //new DispatchableVehicle("police5", 50, 50){RequiredLiveries = new List<int>() { 6 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 6 } } };
        List<DispatchableVehicle> EastLSPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 33, 33){RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 33, 33){RequiredLiveries = new List<int>() { 3 } },
            //new DispatchableVehicle("police5", 33, 33){RequiredLiveries = new List<int>() { 3 } },
            new DispatchableVehicle("sheriff2", 33, 33){RequiredLiveries = new List<int>() { 3 } } };
        List<DispatchableVehicle> VWPDVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 3 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 25, 25){RequiredLiveries = new List<int>() { 2 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50){RequiredLiveries = new List<int>() { 2 } },
            //new DispatchableVehicle("police5", 50, 50){RequiredLiveries = new List<int>() { 2 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 2 } }, };
        List<DispatchableVehicle> LSPPVehicles_FEJ = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 15,10){ RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100), new DispatchableVehicleExtra(2, false, 100) } },
            new DispatchableVehicle("police2", 50, 50) {RequiredLiveries = new List<int>() { 13 },VehicleExtras = new List<DispatchableVehicleExtra>() { new DispatchableVehicleExtra(1,true,100) } },
            new DispatchableVehicle("police3", 50, 50)  {RequiredLiveries = new List<int>() { 13 } },
            //new DispatchableVehicle("police5", 50, 50)  {RequiredLiveries = new List<int>() { 13 } },
            new DispatchableVehicle("sheriff2", 25, 25){RequiredLiveries = new List<int>() { 13 } }, };


        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("UnmarkedVehicles", UnmarkedVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("CoastGuardVehicles", CoastGuardVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("ParkRangerVehicles", ParkRangerVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("FIBVehicles", FIBVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("NOOSEVehicles", NOOSEVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("PrisonVehicles", PrisonVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("LSPDVehicles", LSPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("SAHPVehicles", SAHPVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("LSSDVehicles", LSSDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("BCSOVehicles", BCSOVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("LSIAPDVehicles", LSIAPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("LSPPVehicles", LSPPVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("VWHillsLSSDVehicles", VWHillsLSSDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("DavisLSSDVehicles", DavisLSSDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("MajesticLSSDVehicles", MajesticLSSDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("RHPDVehicles", RHPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("DPPDVehicles", DPPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("VWPDVehicles", VWPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("EastLSPDVehicles", EastLSPDVehicles_FEJ));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("PoliceHeliVehicles", PoliceHeliVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("SheriffHeliVehicles", SheriffHeliVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("ArmyVehicles", ArmyVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("Firetrucks", Firetrucks));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("Amublance1", Amublance1));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("Amublance2", Amublance2));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("Amublance3", Amublance3));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("NYSPVehicles", NYSPVehicles));

        //Gang stuff
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("GenericGangVehicles", GenericGangVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("AllGangVehicles", AllGangVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("LostMCVehicles", LostMCVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("VarriosVehicles", VarriosVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("BallasVehicles", BallasVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("VagosVehicles", VagosVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("MarabuntaVehicles", MarabuntaVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("KoreanVehicles", KoreanVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("TriadVehicles", TriadVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("YardieVehicles", YardieVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("DiablosVehicles", DiablosVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("MafiaVehicles", MafiaVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("ArmeniaVehicles", ArmeniaVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("CartelVehicles", CartelVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("RedneckVehicles", RedneckVehicles));
        VehicleGroupLookupFEJ.Add(new DispatchableVehicleGroup("FamiliesVehicles", FamiliesVehicles));

        Serialization.SerializeParams(VehicleGroupLookupFEJ, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\DispatchableVehicles_FullExpandedJurisdiction.xml");
    }
}


