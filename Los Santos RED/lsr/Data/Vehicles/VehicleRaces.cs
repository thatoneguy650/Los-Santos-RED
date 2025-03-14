using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRaces : IVehicleRaces
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\VehicleRaces.xml";
    public VehicleRaceTypeManager VehicleRaceTypeManager { get; private set; }
    public void ReadConfig(string configName)
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles($"VehicleRaces{configName}.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded VehicleRaces config: {ConfigFile.FullName}", 0);
            VehicleRaceTypeManager = Serialization.DeserializeParam<VehicleRaceTypeManager>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded VehicleRaces config  {ConfigFileName}", 0);
            VehicleRaceTypeManager = Serialization.DeserializeParam<VehicleRaceTypeManager>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No VehicleRaces config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        VehicleRaceTypeManager = new VehicleRaceTypeManager();
        VehicleRaceTypeManager.VehiclesRaces = new List<VehicleRace>();
      
        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions = new List<VehicleRaceStartingPosition>() 
        {
            new VehicleRaceStartingPosition(0, new Vector3(1868.027f, 3226.604f, 44.5677f), 39.27186f),
            new VehicleRaceStartingPosition(1, new Vector3(1876.75f, 3215.834f, 44.83164f), 39.09522f),
            new VehicleRaceStartingPosition(2, new Vector3(1866.469f, 3220.694f, 44.6372f), 43.98711f),
            new VehicleRaceStartingPosition(3, new Vector3(1874.058f, 3211.186f, 44.86393f), 38.87733f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints = new List<VehicleRaceCheckpoint>() 
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1583.795f, 3483.779f, 36.04169f)),
            //new VehicleRaceCheckpoint(2, new Vector3(951.4559f, 3536.375f, 33.44981f)),
            //new VehicleRaceCheckpoint(3,new Vector3(927.9109f, 3630.1f, 31.85251f)),
            //new VehicleRaceCheckpoint(4,new Vector3(1309.275f, 3653.17f, 32.52422f)),
            //new VehicleRaceCheckpoint(5,new Vector3(1663.282f, 3858.711f, 34.23738f)),
            //new VehicleRaceCheckpoint(6,new Vector3(1743.253f, 3758.429f, 33.24446f)),
            //new VehicleRaceCheckpoint(7,new Vector3(1968.987f, 3878.23f, 31.663f)),
            new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            new VehicleRaceCheckpoint(1,new Vector3(1582.85f, 3480.25f, 36.21559f)),
            new VehicleRaceCheckpoint(2,new Vector3(984.0595f, 3535.763f, 33.54565f)),
            new VehicleRaceCheckpoint(3,new Vector3(956.1648f, 3634.594f, 32.08199f)),
            new VehicleRaceCheckpoint(4,new Vector3(1308.131f, 3652.568f, 32.76283f)),
            new VehicleRaceCheckpoint(5,new Vector3(1628.509f, 3824.556f, 34.63056f)),
            new VehicleRaceCheckpoint(6,new Vector3(1740.17f, 3764.768f, 33.48697f)),
            new VehicleRaceCheckpoint(7,new Vector3(1975.885f, 3888.242f, 31.93978f)),
        };
        VehicleRace sandyDebug = new VehicleRace("Sandy Shores Debug Race", vehicleRaceCheckpoints, vehicleRaceStartingPositions);
        VehicleRaceTypeManager.VehiclesRaces.Add(sandyDebug);



        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions2 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints2 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(423.3498f, 295.5466f, 102.4895f)),
            new VehicleRaceCheckpoint(1,new Vector3(283.6923f, -73.34487f, 69.56183f)),
            new VehicleRaceCheckpoint(2,new Vector3(-38.75998f, 32.98601f, 71.58444f)),
            new VehicleRaceCheckpoint(3,new Vector3(-82.37482f, -110.0097f, 57.30961f)),
            new VehicleRaceCheckpoint(4,new Vector3(-111.9359f, -220.4267f, 44.26889f)),
            new VehicleRaceCheckpoint(5,new Vector3(-280.4997f, -172.5392f, 39.44448f)),
            new VehicleRaceCheckpoint(6,new Vector3(-343.272f, -191.9327f, 37.79195f)),
            new VehicleRaceCheckpoint(7,new Vector3(-418.2509f, -72.76836f, 42.19181f)),
            new VehicleRaceCheckpoint(8,new Vector3(-391.8105f, 122.2213f, 65.06055f)),
            new VehicleRaceCheckpoint(9,new Vector3(-643.3143f, 130.383f, 56.60829f)),
            new VehicleRaceCheckpoint(10,new Vector3(-996.1489f, 71.36115f, 51.30039f)),
        };
        VehicleRace vinewoodDebug = new VehicleRace("Vinewood Debug Race", vehicleRaceCheckpoints2, vehicleRaceStartingPositions2);
        VehicleRaceTypeManager.VehiclesRaces.Add(vinewoodDebug);

        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions3 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints3 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(31.48836f, 256.0826f, 109.0203f)),
            new VehicleRaceCheckpoint(1,new Vector3(-2172.701f, -344.3154f, 12.60608f)),

        };
        VehicleRace vinewoodLONGDebug = new VehicleRace("Vinewood LONG Debug Race", vehicleRaceCheckpoints3, vehicleRaceStartingPositions3);
        VehicleRaceTypeManager.VehiclesRaces.Add(vinewoodLONGDebug);
        Serialization.SerializeParam(VehicleRaceTypeManager, ConfigFileName);
    }
}

