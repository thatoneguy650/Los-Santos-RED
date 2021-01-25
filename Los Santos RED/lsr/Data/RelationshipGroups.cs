using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RelationshipGroups : IRelationshipGroups
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\RelationshipGroups.xml";
    private List<RelationshipGroupExt> RelationshipGroupList;
    public RelationshipGroups()
    {

    }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            RelationshipGroupList = Serialization.DeserializeParams<RelationshipGroupExt>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(RelationshipGroupList, ConfigFileName);
        }
    }
    private void DefaultConfig()
    {
        RelationshipGroupList = new List<RelationshipGroupExt>()
            {
                new RelationshipGroupExt("Player","PLAYER","Player"),
                new RelationshipGroupExt("MaleCivilians","CIVMALE","Man"),
                new RelationshipGroupExt("FemaleCivilians","CIVFEMALE","Woman"),
                new RelationshipGroupExt("Cop","COP","Cop"),
                new RelationshipGroupExt("Security Guard","SECURITY_GUARD","Security Guard"),
                new RelationshipGroupExt("Private Security","PRIVATE_SECURITY","Guard"),
                new RelationshipGroupExt("Firefigher","FIREMAN","Firefigher"),
                new RelationshipGroupExt("GANG_1","GANG_1","Gang 1 Member"),
                new RelationshipGroupExt("GANG_2","GANG_2","Gange 2 Member"),
                new RelationshipGroupExt("GANG_9","GANG_9","Gange 9 Member"),
                new RelationshipGroupExt("GANG_10","GANG_10","Gang 10 Member"),
                new RelationshipGroupExt("The Lost MC","AMBIENT_GANG_LOST","Lost Member"),
                new RelationshipGroupExt("Los Santos Vagos","AMBIENT_GANG_MEXICAN","Vagos Member"),
                new RelationshipGroupExt("The Families","AMBIENT_GANG_FAMILY","Families Member"),
                new RelationshipGroupExt("Ballas","AMBIENT_GANG_BALLAS","Ballas Member"),
                new RelationshipGroupExt("Marabunta Grande","AMBIENT_GANG_MARABUNTE","Marabunta Grande Member"),
                new RelationshipGroupExt("Altruist Cult","AMBIENT_GANG_CULT","Altruist Cult Member"),
                new RelationshipGroupExt("Varrios Los Aztecas","AMBIENT_GANG_SALVA","Varrios Los Aztecas Member"),
                new RelationshipGroupExt("Los Santos Triads","AMBIENT_GANG_WEICHENG","Triad Member"),
                new RelationshipGroupExt("Rednecks","AMBIENT_GANG_HILLBILLY","Redneck"),
                new RelationshipGroupExt("Dealer","DEALER","Dealer"),
                new RelationshipGroupExt("Hates","HATES_PLAYER","Hates Player"),
                new RelationshipGroupExt("Hen","HEN","Hen"),
                new RelationshipGroupExt("Wild Animal","WILD_ANIMAL","Wild Animal"),
                new RelationshipGroupExt("Shark","SHARK","Shark"),
                new RelationshipGroupExt("Cougar","COUGAR","Cougar"),
                new RelationshipGroupExt("None","NO_RELATIONSHIP","Unknown"),
                new RelationshipGroupExt("Special","SPECIAL","Special"),
                new RelationshipGroupExt("MISSION2","MISSION2","MISSION2"),
                new RelationshipGroupExt("MISSION3","MISSION3","MISSION3"),
                new RelationshipGroupExt("MISSION4","MISSION4","MISSION4"),
                new RelationshipGroupExt("MISSION5","MISSION5","MISSION5"),
                new RelationshipGroupExt("MISSION6","MISSION6","MISSION6"),
                new RelationshipGroupExt("MISSION7","MISSION7","MISSION7"),
                new RelationshipGroupExt("MISSION8","MISSION8","MISSION8"),
                new RelationshipGroupExt("Army","ARMY","Soldier"),
                new RelationshipGroupExt("GUARD_DOG","GUARD_DOG","Guard Dog"),
                new RelationshipGroupExt("AGGRESSIVE_INVESTIGATE","AGGRESSIVE_INVESTIGATE","Investigator"),
                new RelationshipGroupExt("EMTs","MEDIC","Paramedic"),
                new RelationshipGroupExt("Prisoner","PRISONER","Prisoner"),
                new RelationshipGroupExt("DOMESTIC_ANIMAL","DOMESTIC_ANIMAL","Pet"),
                new RelationshipGroupExt("DEER","DEER","Deer"),
            };
    }
    public RelationshipGroupExt GetRelationshipGroupExt(string internalName)
    {
        return RelationshipGroupList.FirstOrDefault(x => x.InternalName == internalName);
    }
}

