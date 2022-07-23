using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Speeches : ISpeeches
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Speeches.xml";

    public List<SpeechData> SpeechLookups { get; set; } = new List<SpeechData>();
    public Speeches()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Speeches*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Speeches config: {ConfigFile.FullName}", 0);
            SpeechLookups = Serialization.DeserializeParams<SpeechData>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Speeches config  {ConfigFileName}", 0);
            SpeechLookups = Serialization.DeserializeParams<SpeechData>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Speeches config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void DefaultConfig()
    {
        SpeechLookups = new List<SpeechData>()
        {
            new SpeechData("AGREE_ACROSS_STREET","Agree (Across Street)",false,"Chat","Agree","Shout",false)
            ,new SpeechData("GENERIC_AGREE","Agree",false,"Chat","Agree","",false)
            ,new SpeechData("GENERIC_BUY","Buy",false,"Chat","Buy","",false)
            ,new SpeechData("CHAT_ACROSS_STREET_RESP","Chat Across Street Response",false,"Chat","Chat","Response",true)
            ,new SpeechData("CHAT_ACROSS_STREET_STATE","Chat Across Street State",false,"Chat","Chat","Shout",false)
            ,new SpeechData("CHAT_RESP","Chat Reponse",false,"Chat","Chat","Response",true)
            ,new SpeechData("CHAT_STATE","Chat State",false,"Chat","Chat","",true)
            ,new SpeechData("GENERIC_CHEER","Cheer",false,"Chat","Cheer","",false)
            ,new SpeechData("LOOKING_AT_PHONE","Look At Phone",false,"Chat","Look At phone","",false)
            ,new SpeechData("NICE_CAR_SHOUT","Nice Car (Shout)",false,"Chat","Nice Car","Shout",false)
            ,new SpeechData("NICE_CAR","Nice Car",false,"Chat","Nice Car","",false)
            ,new SpeechData("GENERIC_NO","No",false,"Chat","No","",false)
            ,new SpeechData("PHONE_CONV1_CHAT1","Phone Chat 1-1",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV1_CHAT2","Phone Chat 1-2",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV1_CHAT3","Phone Chat 1-3",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV1_INTRO","Phone Chat 1 Start",false,"Chat","Phone Chat","Start",false)
            ,new SpeechData("PHONE_CONV1_OUTRO","Phone Chat 1 End",false,"Chat","Phone Chat","End",false)
            ,new SpeechData("PHONE_CONV2_CHAT1","Phone Chat 2-1",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV2_CHAT2","Phone Chat 2-2",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV2_CHAT3","Phone Chat 2-3",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV2_INTRO","Phone Chat 2 Start",false,"Chat","Phone Chat","Start",false)
            ,new SpeechData("PHONE_CONV2_OUTRO","Phone Chat 2 End",false,"Chat","Phone Chat","End",false)
            ,new SpeechData("PHONE_CONV3_CHAT1","Phone Chat 3-1",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV3_CHAT2","Phone Chat 3-2",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV3_CHAT3","Phone Chat 3-3",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV3_INTRO","Phone Chat 3 Start",false,"Chat","Phone Chat","Start",false)
            ,new SpeechData("PHONE_CONV3_OUTRO","Phone Chat 3 End",false,"Chat","Phone Chat","End",false)
            ,new SpeechData("PHONE_CONV4_CHAT1","Phone Chat 4-1",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV4_CHAT2","Phone Chat 4-2",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV4_CHAT3","Phone Chat 4-3",false,"Chat","Phone Chat","",true)
            ,new SpeechData("PHONE_CONV4_INTRO","Phone Chat 4 Start",false,"Chat","Phone Chat","Start",false)
            ,new SpeechData("PHONE_CONV4_OUTRO","Phone Chat 4 End",false,"Chat","Phone Chat","End",false)
            ,new SpeechData("PED_RANT_01","Rant",false,"Chat","Rant","",true)
            ,new SpeechData("PED_RANT","Rant",false,"Chat","Rant","",true)
            ,new SpeechData("STEPPED_IN_SHIT","Stepped In Shit",false,"Chat","Stepped In Shit","",false)
            ,new SpeechData("GENERIC_YES","Yes",false,"Chat","Yes","",false)
            ,new SpeechData("GENERIC_BYE","Bye",false,"Greet","Bye","",false)
            ,new SpeechData("GOODBYE_ACROSS_STREET_FEMALE","Goodbye (Across Street)",false,"Greet","Goodbye","Shout",false)
            ,new SpeechData("GOODBYE_ACROSS_STREET","Goodbye (Across Street)",false,"Greet","Goodbye","Shout",false)
            ,new SpeechData("GREET_ACROSS_SREET_FEMALE","Greet (Across Street)",false,"Greet","Greet","Shout",false)
            ,new SpeechData("GREET_ACROSS_SREET","Greet (Across Street)",false,"Greet","Greet","Shout",false)
            ,new SpeechData("GREET_ACROSS_STREET","Greet (Across Street)",false,"Greet","Greet","Shout",false)
            ,new SpeechData("GENERIC_HI","Hi",false,"Greet","Hi","",false)
            ,new SpeechData("GENERIC_HOWS_IT_GOING","How's It Going",false,"Greet","How's It Going","",false)
            ,new SpeechData("HOWS_IT_GOING_GENERIC","How's It Going",false,"Greet","How's It Going","",false)
            ,new SpeechData("KIFFLOM_GREET","Kifflom Greet",false,"Greet","Kifflom","",false)
            ,new SpeechData("CHALLENGE_ACCEPTED_HIT_CAR","Challenge Accepted (Hit Car)",false,"React","Chellenge Accepted","Hit Car",false)
            ,new SpeechData("GUN_COOL","Cool Gun",false,"React","Cool Gun","",false)
            ,new SpeechData("OVER_THERE","Over There",false,"React","Over There","",false)
            ,new SpeechData("PROVOKE_BAR","Provoke (Bar)",false,"React","Provoke","Bar",false)
            ,new SpeechData("PROVOKE_FOLLOWING","Provoke (Weirdo)",false,"React","Provoke","Weirdo",false)
            ,new SpeechData("PROVOKE_GENERIC","Provoke",false,"React","Provoke","",false)
            ,new SpeechData("PROVOKE_TRESPASS","Provoke (Trespass)",false,"React","Provoke","Trespass",false)
            ,new SpeechData("SEE_WEIRDO_PHONE","See Weirdo (Phone)",false,"React","See Weirdo","Phone",false)
            ,new SpeechData("SEE_WEIRDO","See Weirdo",false,"React","See Weirdo","",false)
            ,new SpeechData("GENERIC_SHOCKED_HIGH","Shocked (High)",false,"React","Shocked","High",false)
            ,new SpeechData("GENERIC_SHOCKED_MED","Shocked (Medium)",false,"React","Shocked","Medium",false)
            ,new SpeechData("GENERIC_THANKS","Thanks",false,"React","Thanks","",false)
            ,new SpeechData("UP_THERE","Up There",false,"React","Up There","",false)
            ,new SpeechData("GENERIC_WHATEVER","Whatever",false,"React","Whatever","",false)
            ,new SpeechData("PHONE_CALL_COPS","Call Cops",false,"Scared","Call Cops","",false)
            ,new SpeechData("GENERIC_FREIGHTENED_HIGH","Frightened (High)",false,"Scared","Frightened","High",false)
            ,new SpeechData("GENERIC_FREIGHTENED_MED","Frightened (Medium)",false,"Scared","Frightened","Medium",false)
            ,new SpeechData("GENERIC_FRIGHTENED_HIGH","Frightened (High)",false,"Scared","Frightened","High",false)
            ,new SpeechData("GENERIC_FRIGHTENED_MED","Fightened (Medium)",false,"Scared","Frightened","Medium",false)
            ,new SpeechData("CALL_COPS_COMMIT","Say You Are Calling Cops",false,"Scared","Say You Are Calling the Cops","",false)
            ,new SpeechData("CALL_COPS_THREAT","Threaten Call Cops",false,"Scared","Threaten To Call the Cops","",false)
            ,new SpeechData("TRAPPED","Trapped",false,"Scared","Trapped","",false)
            ,new SpeechData("APOLOGY_NO_TROUBLE","Apologize",false,"Threaten","Apologize","",false)
            ,new SpeechData("BLOCKED_GENERIC","Blocked",true,"Threaten","Blocked","",false)
            ,new SpeechData("BUMP","Bump",true,"Threaten","Bump","",false)
            ,new SpeechData("CHALLENGE_ACCEPTED_BUMPED_INTO","Challenge Accepted (Bumped)",true,"Threaten","Challenge Accepted","Bumped",false)
            ,new SpeechData("CHALLENGE_ACCEPTED_GENERIC","Challenge Accepted",true,"Threaten","Challenge Accepted","",false)
            ,new SpeechData("GENERIC_CURSE_HIGH","Curse (High)",true,"Threaten","Curse","High",false)
            ,new SpeechData("GENERIC_CURSE_MED","Curse (Medium)",true,"Threaten","Curse","Medium",false)
            ,new SpeechData("FIGHT_RUN","Fight (Run)",true,"Threaten","Fight","Run",false)
            ,new SpeechData("FIGHT","Fight",true,"Threaten","Fight","",false)
            ,new SpeechData("GENERIC_INSULT_HIGH","Insult (High)",true,"Threaten","Insult","High",false)
            ,new SpeechData("GENERIC_INSULT_MED","Insult (Medium)",true,"Threaten","Insult","Medium",false)
            ,new SpeechData("SHOUT_INSULT","Insult (Shouted)",true,"Threaten","Insult","Shouted",false)
            ,new SpeechData("CHALLENGE_THREATEN","Challenge Threaten",true,"Threaten","Threaten","",false)
            ,new SpeechData("WON_DISPUTE","Won Dispute",false,"Threaten","Won Dispute","",false)
        };
        Serialization.SerializeParams(SpeechLookups, ConfigFileName);
    }

}
