using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class PlacesOfInterest_SunshineDream
{
    private PossibleLocations SunshineDreamLocations;
    public void DefaultConfig()
    {
        SunshineDreamLocations = new PossibleLocations();
        //These are for centered above, remove 200 from height
        DefaultConfig_Other();
        DefaultConfig_PoliceStations();
        DefaultConfig_Hospitals();
        DefaultConfig_Prisons();
        DefaultConfig_GangDens();
        Serialization.SerializeParam(SunshineDreamLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\Locations_SunshineDream.xml");
    }

    private void DefaultConfig_Other()
    {
      
    }
    private void DefaultConfig_GangDens()
    {
        List<GangDen> LCGangDens = new List<GangDen>()
        {
            new GangDen(new Vector3(-1683.321f, 984.329f, 6.509592f), 252.1058f, "Armenian Hangout", "", "ArmenianDenMenu", "AMBIENT_GANG_ARMENIAN")
            {
                IsPrimaryGangDen = true,
                CanInteractWhenWanted = true,
                MapIcon = 541,
                BannerImagePath = "gangs\\armenian.png",
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                StateID = StaticStrings.LeonidaStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1672.252f, 987.6925f, 2.679666f), 254.2377f, 75f){ TaskRequirements = TaskRequirements.Guard },
                    new GangConditionalLocation(new Vector3(-1676.534f, 995.0093f, 2.679703f), 308.6421f, 75f){ TaskRequirements = TaskRequirements.Guard },
                    new GangConditionalLocation(new Vector3(-1700.064f, 978.8469f, 2.679667f), 122.4722f, 75f){ TaskRequirements = TaskRequirements.Guard },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1682.491f, 970.5074f, 2.476591f), 344.0008f, 75f),
                    new GangConditionalLocation(new Vector3(-1689.95f, 972.4074f, 2.477526f), 344.2486f, 75f),
                }
            },
        };
        SunshineDreamLocations.GangDens.AddRange(LCGangDens);
    }
    private void DefaultConfig_Prisons()
    {
        List<Prison> VicePrison = new List<Prison>()
        {
            //new Prison(new Vector3(-903.9021f, 118.7461f, 3.080931f), 91.96844f, "Vice Correctional Facility","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
        };
        SunshineDreamLocations.Prisons.AddRange(VicePrison);
    }
    private void DefaultConfig_Hospitals()
    {
        List<Hospital> Hospitals = new List<Hospital>()
        {
            new Hospital(new Vector3(-1797.433f, -624.8724f, 2.661737f), 210.8405f, "Vice Beach Pharmacy","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
        };
        SunshineDreamLocations.Hospitals.AddRange(Hospitals);
    }
    private void DefaultConfig_PoliceStations()
    {
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            new PoliceStation(new Vector3(-1656.784f, 178.722f, 3.530202f), 57.65264f, "Vice Beach Police Department","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
        };
        SunshineDreamLocations.PoliceStations.AddRange(PoliceStations);
    }
}

