using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedGroups : IPedGroups
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\PedGroups.xml";
    private List<PedGroup> PedGroupList;
    public PedGroups()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("PedGroups*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Ped Groups config: {ConfigFile.FullName}",0);
            PedGroupList = Serialization.DeserializeParams<PedGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Ped Groups config  {ConfigFileName}", 0);
            PedGroupList = Serialization.DeserializeParams<PedGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Ped Groups config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        PedGroupList = new List<PedGroup>()
            {
                new PedGroup("Player","PLAYER","Player",true),
                new PedGroup("MaleCivilians","CIVMALE","Man",false),
                new PedGroup("FemaleCivilians","CIVFEMALE","Woman",false),
                new PedGroup("Security Guard","SECURITY_GUARD","Security Guard",false),
                new PedGroup("Private Security","PRIVATE_SECURITY","Guard",false),
                new PedGroup("Dealer","DEALER","Dealer",true),
                new PedGroup("Hates","HATES_PLAYER","Hates Player",true),
                new PedGroup("Hen","HEN","Hen",false),
                new PedGroup("Wild Animal","WILD_ANIMAL","Wild Animal",false),
                new PedGroup("Shark","SHARK","Shark",false),
                new PedGroup("Cougar","COUGAR","Cougar",false),
                new PedGroup("None","NO_RELATIONSHIP","Unknown",false),
                new PedGroup("Special","SPECIAL","Special",false),
                new PedGroup("MISSION2","MISSION2","MISSION2",false),
                new PedGroup("MISSION3","MISSION3","MISSION3",false),
                new PedGroup("MISSION4","MISSION4","MISSION4",false),
                new PedGroup("MISSION5","MISSION5","MISSION5",false),
                new PedGroup("MISSION6","MISSION6","MISSION6",false),
                new PedGroup("MISSION7","MISSION7","MISSION7",false),
                new PedGroup("MISSION8","MISSION8","MISSION8",false),
                new PedGroup("Army","ARMY","Soldier",false),
                new PedGroup("GUARD_DOG","GUARD_DOG","Guard Dog",false),
                new PedGroup("AGGRESSIVE_INVESTIGATE","AGGRESSIVE_INVESTIGATE","Investigator",false),
                new PedGroup("Prisoner","PRISONER","Prisoner",false),
                new PedGroup("DOMESTIC_ANIMAL","DOMESTIC_ANIMAL","Pet",false),
                new PedGroup("DEER","DEER","Deer",false),
            };
        Serialization.SerializeParams(PedGroupList, ConfigFileName);
    }
    public PedGroup GetPedGroup(string internalName)
    {
        return PedGroupList.FirstOrDefault(x => x.InternalName == internalName);
    }
}

