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
        if (File.Exists(ConfigFileName))
        {
            PedGroupList = Serialization.DeserializeParams<PedGroup>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(PedGroupList, ConfigFileName);
        }
    }
    private void DefaultConfig()
    {
        PedGroupList = new List<PedGroup>()
            {
                new PedGroup("Player","PLAYER","Player",true),
                new PedGroup("MaleCivilians","CIVMALE","Man",false),
                new PedGroup("FemaleCivilians","CIVFEMALE","Woman",false),
                new PedGroup("Cop","COP","Cop",false),
                new PedGroup("Security Guard","SECURITY_GUARD","Security Guard",false),
                new PedGroup("Private Security","PRIVATE_SECURITY","Guard",false),
                new PedGroup("Firefigher","FIREMAN","Firefigher",false),
                new PedGroup("GANG_1","GANG_1","Gang 1 Member",true),
                new PedGroup("GANG_2","GANG_2","Gange 2 Member",true),
                new PedGroup("GANG_9","GANG_9","Gange 9 Member",true),
                new PedGroup("GANG_10","GANG_10","Gang 10 Member",true),
                new PedGroup("The Lost MC","AMBIENT_GANG_LOST","Lost Member",true),
                new PedGroup("Los Santos Vagos","AMBIENT_GANG_MEXICAN","Vagos Member",true),
                new PedGroup("The Families","AMBIENT_GANG_FAMILY","Families Member",true),
                new PedGroup("Ballas","AMBIENT_GANG_BALLAS","Ballas Member",true),
                new PedGroup("Marabunta Grande","AMBIENT_GANG_MARABUNTE","Marabunta Grande Member",true),
                new PedGroup("Altruist Cult","AMBIENT_GANG_CULT","Altruist Cult Member",true),
                new PedGroup("Varrios Los Aztecas","AMBIENT_GANG_SALVA","Varrios Los Aztecas Member",true),
                new PedGroup("Los Santos Triads","AMBIENT_GANG_WEICHENG","Triad Member",true),
                new PedGroup("Rednecks","AMBIENT_GANG_HILLBILLY","Redneck",true),
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
                new PedGroup("EMTs","MEDIC","Paramedic",false),
                new PedGroup("Prisoner","PRISONER","Prisoner",false),
                new PedGroup("DOMESTIC_ANIMAL","DOMESTIC_ANIMAL","Pet",false),
                new PedGroup("DEER","DEER","Deer",false),
            };
    }
    public PedGroup GetPedGroup(string internalName)
    {
        return PedGroupList.FirstOrDefault(x => x.InternalName == internalName);
    }
}

