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
            //Greetings
            new SpeechData("GENERIC_HI","Hi (1)","Greetings",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_HI_FEMALE","Hi (2)","Greetings",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_HI_MALE","Hi (3)","Greetings",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_HOWS_IT_GOING","Hows it going","Greetings",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_BYE","Bye","Greetings",false,false) { CanUseInConversation = true }
            ,new SpeechData("KIFFLOM_GREET","Kifflom Greet","Greetings",false,false) { CanUseInConversation = true }
         

            //Insults
            ,new SpeechData("CHALLENGE_ACCEPTED_BUMPED_INTO","Challenge Accepted Bumpped Into","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("CHALLENGE_ACCEPTED_GENERIC","Challenege Accepted Generic","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("CHALLENGE_ACCEPTED_HIT_CAR","Challenege Accepted Hit Car","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("CHALLENGE_THREATEN","Challenge Threaten","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("BUMP","Bump","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_FUCK_YOU","Fuck You","Insults",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_INSULT_HIGH","Insult (High)","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_INSULT_MED","Insult (Med)","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_CURSE_HIGH","Curse (High)","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_CURSE_MED","Curse (Med)","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("WON_DISPUTE","Won Dispute","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("BLOCKED_GENERIC","Blocked Generic","Insults",false,false) { CanUseInConversation = true }
            ,new SpeechData("APOLOGY_NO_TROUBLE","Apology No Trouble","Insults",false,false) { CanUseInConversation = true }

            //Small Talk
            ,new SpeechData("CHAT_RESP","Chat Response","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("CHAT_STATE","Chat State","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_BUY","Buy (1)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_BUY_REPEAT","Buy (2)","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_WHATEVER","Whatever","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_YES","Yes","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_DRINK","Drink","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_EAT","Eat","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_FRUSTRATED_HIGH","Frustrated (High)","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_FRUSTRATED_MED","Frustrated (Med)","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_NO","No","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_OUT_OF_MY_WAY","Out Of My Way","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_THANKS","Thanks","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PED_RANT_01","Rant","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PED_RANT_RESP","Rant Response","General",true,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_SHOCKED_HIGH","Shocked (High)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GENERIC_SHOCKED_MED","Shocked (Med)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("GUN_COOL","Cool Gun","General",false,false) { CanUseInConversation = true }


            ,new SpeechData("PHONE_CONV1_INTRO","Conversation 1 Intro","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV1_CHAT1","Conversation 1 Chat (1)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV1_CHAT2","Conversation 1 Chat (2)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV1_CHAT3","Conversation 1 Chat (3)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV1_OUTRO","Conversation 1 Outro","General",false,false) { CanUseInConversation = true }

            ,new SpeechData("PHONE_CONV2_INTRO","Conversation 2 Intro","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV2_CHAT1","Conversation 2 Chat (1)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV2_CHAT2","Conversation 2 Chat (2)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV2_CHAT3","Conversation 2 Chat (3)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV2_OUTRO","Conversation 2 Outro","General",false,false) { CanUseInConversation = true }

            ,new SpeechData("PHONE_CONV3_INTRO","Conversation 3 Intro","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV3_CHAT1","Conversation 3 Chat (1)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV3_CHAT2","Conversation 3 Chat (2)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV3_CHAT3","Conversation 3 Chat (3)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV3_OUTRO","Conversation 3 Outro","General",false,false) { CanUseInConversation = true }

            ,new SpeechData("PHONE_CONV4_INTRO","Conversation 4 Intro","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV4_CHAT1","Conversation 4 Chat (1)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV4_CHAT2","Conversation 4 Chat (2)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV4_CHAT3","Conversation 4 Chat (3)","General",false,false) { CanUseInConversation = true }
            ,new SpeechData("PHONE_CONV4_OUTRO","Conversation 4 Outro","General",false,false) { CanUseInConversation = true }






            ,new SpeechData("GENERIC_INSULT_OLD","GENERIC_NO","Insults",true,false)
            ,new SpeechData("GENERIC_INSULT_MALE","GENERIC_INSULT_MED","Insults",true,false) 
            ,new SpeechData("GENERIC_INSULT_FEMALE","Insult (","Insults",true,false)
            ,new SpeechData("HOWS_IT_GOING_FEMALE","HOWS_IT_GOING_GENERIC","Greetings",true,false)
            ,new SpeechData("HOWS_IT_GOING_GENERIC","HOWS_IT_GOING_MALE","Greetings",true,false) 
            ,new SpeechData("HOWS_IT_GOING_MALE","HURRY_UP","Greetings",true,false)
            ,new SpeechData("GREET_ATTRACTIVE_F","GREET_BUM","Greetings",true,false) 
            ,new SpeechData("GREET_BUM","GREET_COP","Greetings",true,false)
            ,new SpeechData("GREET_COP","GREET_COP_OUTFIT","Greetings",true,false) 
            ,new SpeechData("GREET_COP_OUTFIT","GREET_CREEPY_OUTFIT","Greetings",true,false) 
            ,new SpeechData("GREET_CREEPY_OUTFIT","GREET_GANG_BALLAS_M","Greetings",true,false) 
            ,new SpeechData("GREET_GANG_BALLAS_M","GREET_GANG_FAMILIES_M","Greetings",true,false) 
            ,new SpeechData("GREET_GANG_FAMILIES_M","GREET_GANG_VAGOS_M","Greetings",true,false)
            ,new SpeechData("GREET_GANG_VAGOS_M","GREET_HILLBILLY_M","Greetings",true,false) 
            ,new SpeechData("GREET_HILLBILLY_M","GREET_HIPPY_F","Greetings",true,false)
            ,new SpeechData("GREET_HIPPY_F","GREET_HIPPY_M","Greetings",true,false) 
            ,new SpeechData("GREET_HIPPY_M","GREET_HIPSTER_F","Greetings",true,false) 
            ,new SpeechData("GREET_HIPSTER_F","GREET_HIPSTER_M","Greetings",true,false) 
            ,new SpeechData("GREET_HIPSTER_M","GREET_JUNKIE","Greetings",true,false) 
            ,new SpeechData("GREET_JUNKIE","GREET_KOREAN","Greetings",true,false) 
            ,new SpeechData("GREET_KOREAN","GREET_STRONG_M","Greetings",true,false) 
            ,new SpeechData("GREET_STRONG_M","GREET_TRANSVESTITE","Greetings",true,false) 
            ,new SpeechData("GREET_TRANSVESTITE","GUN_BEG","Greetings",true,false)
             ,new SpeechData("CANT_LOSE_ENEMY","Cant Lose Enemy","Combat",true,false)
            ,new SpeechData("COMBAT_TAUNT","Combat Taunt","Combat",false,true)
            ,new SpeechData("COVER_ME","Cover Me","Combat",false,true)
            ,new SpeechData("COVER_YOU","Cover You","Combat",false,true)
            ,new SpeechData("DUCK","ENEMY_NOT_SPOTTED","Combat",false,true)
            ,new SpeechData("ENEMY_NOT_SPOTTED","ENEMY_SPOTTED","Combat",true,false)
            ,new SpeechData("ENEMY_SPOTTED","ENEMY_SPOTTED_BEHIND","ENEMY_SPOTTED",true,false)
            ,new SpeechData("ENEMY_SPOTTED_BEHIND","ENEMY_SPOTTED_DOWN","ENEMY_SPOTTED_BEHIND",true,false)
            ,new SpeechData("ENEMY_SPOTTED_DOWN","ENEMY_SPOTTED_FRONT","ENEMY_SPOTTED_DOWN",true,false)
            ,new SpeechData("ENEMY_SPOTTED_FRONT","ENEMY_SPOTTED_LEFT","ENEMY_SPOTTED_FRONT",true,false)
            ,new SpeechData("ENEMY_SPOTTED_LEFT","ENEMY_SPOTTED_RIGHT","ENEMY_SPOTTED_LEFT",true,false)
            ,new SpeechData("ENEMY_SPOTTED_RIGHT","ENEMY_SPOTTED_UP","ENEMY_SPOTTED_RIGHT",true,false)
            ,new SpeechData("ENEMY_SPOTTED_UP","EPSILON_SITE_VISIT_BACKOUT","ENEMY_SPOTTED_UP",true,false)
            ,new SpeechData("GENERIC_WAR_CRY","GENERIC_WHATEVER","GENERIC_WAR_CRY",false,true)
            ,new SpeechData("GENERIC_FRIGHTENED_HIGH","GENERIC_FRIGHTENED_MED","GENERIC_FRIGHTENED_HIGH",false,false)
            ,new SpeechData("GENERIC_FRIGHTENED_MED","GENERIC_FRUSTRATED_HIGH","GENERIC_FRIGHTENED_MED",false,false)
            ,new SpeechData("GUN_BEG","GUN_COOL","GUN_BEG",false,false)
            ,new SpeechData("CAR_HIT_PED","Car Hit Ped","CAR_HIT_PED",true,false)
            ,new SpeechData("CAR_HIT_PED_DRIVEN","Car Hit Ped Driven","CAR_HIT_PED_DRIVEN",true,false)
            ,new SpeechData("CRASH_CAR","Crash Car","CRASH_CAR",false,false)
            ,new SpeechData("CRASH_CAR_BASS","Crash Car BAss","CRASH_CAR_BASS",true,false)
            ,new SpeechData("CRASH_CAR_DRIVEN","Crash Car Driven","CRASH_CAR_DRIVEN",true,false)
            ,new SpeechData("CRASH_GENERIC","Crash Generic","CRASH_GENERIC",false,false)
            ,new SpeechData("CRASH_GENERIC_DRIVEN","Crash Generic Driven","CRASH_GENERIC_DRIVEN",true,false)
            ,new SpeechData("CRASH_GENERIC_INTERRUPT","Crash Generic Interrupt","CRASH_GENERIC_INTERRUPT",true,false)
            ,new SpeechData("ACTIVITY_LEAVING","Leaving Activity","ACTIVITY_LEAVING",true,false)
            ,new SpeechData("AGREE_ACROSS_STREET","Agree Across Street","AGREE_ACROSS_STREET",false,false)
            ,new SpeechData("AIMED_AT_BY_PLAYER","Aimed at by Player","AIMED_AT_BY_PLAYER",true,false)
            ,new SpeechData("ANGRY_WITH_PLAYER_TREVOR","Angry with Trevor","ANGRY_WITH_PLAYER_TREVOR",false,false)
            ,new SpeechData("ARREST_PLAYER","Arrest Player","ARREST_PLAYER",false,true)
            ,new SpeechData("ARRESTED","Arrested","ARRESTED",true,false)
            ,new SpeechData("BASEJUMP_ABOUT_TO_JUMP","About to Basejump","BASEJUMP_ABOUT_TO_JUMP",true,false)
            ,new SpeechData("BASEJUMP_FAIL","Basejump Fail","BASEJUMP_FAIL",true,false)
            ,new SpeechData("BASEJUMP_SUCCESS","Basejump SUccess","BASEJUMP_SUCCESS",true,false)
            ,new SpeechData("BLIND_RAGE","Blind Rage","BLIND_RAGE",true,false)
            ,new SpeechData("BLOCKED_IN_PURSUIT","Blocked In Pursuit","BLOCKED_IN_PURSUIT",false,true)
            ,new SpeechData("BOOTY_FLIRT_RESP","Booty Flirt Response","BOOTY_FLIRT_RESP",true,false)
            ,new SpeechData("BUDDY_DOWN","Buddy Down","BUDDY_DOWN",false,true)
            ,new SpeechData("BUDDY_SEES_FRANKLIN_DEATH","Buddy Sees Franklin Die","BUDDY_SEES_FRANKLIN_DEATH",true,false)
            ,new SpeechData("BUDDY_SEES_TREVOR_DEATH","Buddy Sees Trevor Die","BUDDY_SEES_TREVOR_DEATH",true,false)
            ,new SpeechData("CHASE_VEHICLE_MEGAPHONE","Chase Vehicle Megaphone","CHASE_VEHICLE_MEGAPHONE",false,true)
            ,new SpeechData("CHASED_BY_POLICE","Chased by Police","CHASED_BY_POLICE",true,false)
            ,new SpeechData("CHAT_ACROSS_STREET_RESP","Chat Across Street Response","CHAT_ACROSS_STREET_RESP",false,false)
            ,new SpeechData("CHAT_ACROSS_STREET_STATE","Chat Acress Street State","CHAT_ACROSS_STREET_STATE",false,false)
            ,new SpeechData("CLEAR_AREA_MEGAPHONE","Clear Area Megaphone","CLEAR_AREA_MEGAPHONE",false,true)
            ,new SpeechData("CLEAR_AREA_PANIC_MEGAPHONE","Clear Area Megaphone Panic","CLEAR_AREA_PANIC_MEGAPHONE",false,true)
            ,new SpeechData("COP_ARRIVAL_ANNOUNCE","Arrival Announce","COP_ARRIVAL_ANNOUNCE",false,true)
            ,new SpeechData("COP_ARRIVAL_ANNOUNCE_MEGAPHONE","Arrival Announce Megaphone","COP_ARRIVAL_ANNOUNCE_MEGAPHONE",false,true)
            ,new SpeechData("COP_PEEK","Peek","COP_PEEK",false,true)
            ,new SpeechData("COP_SEES_GRENADE","Sees Grenade","COP_SEES_GRENADE",false,true)
            ,new SpeechData("COP_SEES_GRENADE_LAUNCHER","Sees Grenade Launcher","COP_SEES_GRENADE_LAUNCHER",false,true)
            ,new SpeechData("COP_SEES_GUN","Sees Gun","COP_SEES_GUN",false,true)
            ,new SpeechData("COP_SEES_MINI_GUN","Sees minigun","COP_SEES_MINI_GUN",false,true)
            ,new SpeechData("COP_SEES_ROCKET_LAUNCHER","Sees Rocket Launcher","COP_SEES_ROCKET_LAUNCHER",false,true)
            ,new SpeechData("COP_SEES_WEAPON","Sees Weapon","COP_SEES_WEAPON",false,true)
            ,new SpeechData("CRIMINAL_APPREHENDED","CRIMINAL_GET_IN_CAR","CRIMINAL_APPREHENDED",false,true)
            ,new SpeechData("CRIMINAL_GET_IN_CAR","CRIMINAL_WARNING","CRIMINAL_GET_IN_CAR",false,true)
            ,new SpeechData("CRIMINAL_WARNING","CULT_TALK","CRIMINAL_WARNING",false,true)
            ,new SpeechData("CULT_TALK","DART_PLAYING_POORLY","CULT_TALK",true,false)
            ,new SpeechData("DART_PLAYING_POORLY","DARTS_1_DART_AWAY","DART_PLAYING_POORLY",true,false)
            ,new SpeechData("DARTS_1_DART_AWAY","DARTS_140","DARTS_1_DART_AWAY",true,false)
            ,new SpeechData("DARTS_140","DARTS_180","DARTS_140",true,false)
            ,new SpeechData("DARTS_180","DARTS_BOAST","DARTS_180",true,false)
            ,new SpeechData("DARTS_BOAST","DARTS_BORED","DARTS_BOAST",true,false)
            ,new SpeechData("DARTS_BORED","DARTS_BULLSEYE","DARTS_BORED",true,false)
            ,new SpeechData("DARTS_BULLSEYE","DARTS_BUST","DARTS_BULLSEYE",true,false)
            ,new SpeechData("DARTS_BUST","DARTS_CHAT_WITH_FRANKLIN","DARTS_BUST",true,false)
            ,new SpeechData("DARTS_CHAT_WITH_FRANKLIN","DARTS_CHAT_WITH_TREVOR","DARTS_CHAT_WITH_FRANKLIN",true,false)
            ,new SpeechData("DARTS_CHAT_WITH_TREVOR","DARTS_HAPPY","DARTS_CHAT_WITH_TREVOR",true,false)
            ,new SpeechData("DARTS_HAPPY","DARTS_INVITE","DARTS_HAPPY",true,false)
            ,new SpeechData("DARTS_INVITE","DARTS_LOSE","DARTS_INVITE",true,false)
            ,new SpeechData("DARTS_LOSE","DARTS_LOSING_BADLY","DARTS_LOSE",true,false)
            ,new SpeechData("DARTS_LOSING_BADLY","DARTS_MISS_BOARD","DARTS_LOSING_BADLY",true,false)
            ,new SpeechData("DARTS_MISS_BOARD","DARTS_PLAYING_WELL","DARTS_MISS_BOARD",true,false)
            ,new SpeechData("DARTS_PLAYING_WELL","DARTS_REQUEST_GAME","DARTS_PLAYING_WELL",true,false)
            ,new SpeechData("DARTS_REQUEST_GAME","DARTS_WIN","DARTS_REQUEST_GAME",true,false)
            ,new SpeechData("DARTS_WIN","DODGE","DARTS_WIN",true,false)
            ,new SpeechData("DODGE","DRAW_GUN","DODGE",false,false)
            ,new SpeechData("DRAW_GUN","DRINK_BAD","DRAW_GUN",true,true)
            ,new SpeechData("DRINK_BAD","DRINK_GOOD","DRINK_BAD",true,false)
            ,new SpeechData("DRINK_GOOD","DRIVEN_FAST","DRINK_GOOD",true,false)
            ,new SpeechData("DRIVEN_FAST","DRIVEN_SLOW","DRIVEN_FAST",true,false)
            ,new SpeechData("DRIVEN_SLOW","DUCK","DRIVEN_SLOW",true,false)
            ,new SpeechData("EPSILON_SITE_VISIT_BACKOUT","EXPLOSION_IS_IMMINENT","EPSILON_SITE_VISIT_BACKOUT",true,false)
            ,new SpeechData("EXPLOSION_IS_IMMINENT","EXTREME_GRIEFING","EXPLOSION_IS_IMMINENT",false,true)
            ,new SpeechData("EXTREME_GRIEFING","FALL_BACK","EXTREME_GRIEFING",true,false)
            ,new SpeechData("FALL_BACK","FEMALE_DISTANT_LAUGH","FALL_BACK",false,true)
            ,new SpeechData("FEMALE_DISTANT_LAUGH","FIGHT","FEMALE_DISTANT_LAUGH",false,false)
            ,new SpeechData("FIGHT","FIGHT_ENCOURAGE","FIGHT",false,false)
            ,new SpeechData("FIGHT_ENCOURAGE","FIGHT_RUN","FIGHT_ENCOURAGE",true,false)
            ,new SpeechData("FIGHT_RUN","FLYING_LOW_SKILL","FIGHT_RUN",false,false)
            ,new SpeechData("FLYING_LOW_SKILL","FOOT_CHASE","FLYING_LOW_SKILL",true,false)
            ,new SpeechData("FOOT_CHASE","FOOT_CHASE_AGGRESSIVE","FOOT_CHASE",false,true)
            ,new SpeechData("FOOT_CHASE_AGGRESSIVE","FOOT_CHASE_HEADING_EAST","FOOT_CHASE_AGGRESSIVE",false,true)
            ,new SpeechData("FOOT_CHASE_HEADING_EAST","FOOT_CHASE_HEADING_NORTH","FOOT_CHASE_HEADING_EAST",false,true)
            ,new SpeechData("FOOT_CHASE_HEADING_NORTH","FOOT_CHASE_HEADING_SOUTH","FOOT_CHASE_HEADING_NORTH",false,true)
            ,new SpeechData("FOOT_CHASE_HEADING_SOUTH","FOOT_CHASE_HEADING_WEST","FOOT_CHASE_HEADING_SOUTH",false,true)
            ,new SpeechData("FOOT_CHASE_HEADING_WEST","FOOT_CHASE_LOSING","FOOT_CHASE_HEADING_WEST",false,true)
            ,new SpeechData("FOOT_CHASE_LOSING","FOOT_CHASE_RESPONSE","FOOT_CHASE_LOSING",false,true)
            ,new SpeechData("FOOT_CHASE_RESPONSE","FRIEND_FOLLOWED_BY_PLAYER","FOOT_CHASE_RESPONSE",false,true)
            ,new SpeechData("FRIEND_FOLLOWED_BY_PLAYER","FRIEND_LOOKS_STUPID","FRIEND_FOLLOWED_BY_PLAYER",true,false)
            ,new SpeechData("FRIEND_LOOKS_STUPID","GAME_BAD_OTHER","FRIEND_LOOKS_STUPID",true,false)
            ,new SpeechData("GAME_BAD_OTHER","GAME_BAD_SELF","GAME_BAD_OTHER",true,false)
            ,new SpeechData("GAME_BAD_SELF","GAME_GOOD_OTHER","GAME_BAD_SELF",true,false)
            ,new SpeechData("GAME_GOOD_OTHER","GAME_GOOD_SELF","GAME_GOOD_OTHER",true,false)
            ,new SpeechData("GAME_GOOD_SELF","GAME_HECKLE","GAME_GOOD_SELF",true,false)
            ,new SpeechData("GAME_HECKLE","GAME_LOSE_SELF","GAME_HECKLE",true,false)
            ,new SpeechData("GAME_LOSE_SELF","GAME_QUIT","GAME_LOSE_SELF",true,false)
            ,new SpeechData("GAME_QUIT","GAME_QUIT_EARLY","GAME_QUIT",true,false)
            ,new SpeechData("GAME_QUIT_EARLY","GAME_WAS_FUN","GAME_QUIT_EARLY",true,false)
            ,new SpeechData("GAME_WAS_FUN","GAME_WIN_SELF","GAME_WAS_FUN",true,false)
            ,new SpeechData("GAME_WIN_SELF","GASMASK_CHOKE","GAME_WIN_SELF",true,false)
            ,new SpeechData("GASMASK_CHOKE","GENERIC_BUY","GASMASK_CHOKE",true,false)
            ,new SpeechData("GET_HIM","GET_IN_VEHICLE","GET_HIM",false,true)
            ,new SpeechData("GET_IN_VEHICLE","GET_OUT_OF_HERE","GET_IN_VEHICLE",true,false)
            ,new SpeechData("GET_OUT_OF_HERE","GET_UP_FROM_FALL","GET_OUT_OF_HERE",true,false)
            ,new SpeechData("GET_UP_FROM_FALL","GET_WANTED_LEVEL","GET_UP_FROM_FALL",true,false)
            ,new SpeechData("GET_WANTED_LEVEL","GETTING_OLD","GET_WANTED_LEVEL",true,false)
            ,new SpeechData("GETTING_OLD","GOLF_BAD_LIE_OTHER","GETTING_OLD",true,false)
            ,new SpeechData("GOLF_BAD_LIE_OTHER","GOLF_BAD_LIE_SELF","GOLF_BAD_LIE_OTHER",true,false)
            ,new SpeechData("GOLF_BAD_LIE_SELF","GOLF_BUNKER_OTHER","GOLF_BAD_LIE_SELF",true,false)
            ,new SpeechData("GOLF_BUNKER_OTHER","GOLF_BUNKER_SELF","GOLF_BUNKER_OTHER",true,false)
            ,new SpeechData("GOLF_BUNKER_SELF","GOLF_CHAT","GOLF_BUNKER_SELF",true,false)
            ,new SpeechData("GOLF_CHAT","GOLF_CHAT_WITH_FRANKLIN","GOLF_CHAT",true,false)
            ,new SpeechData("GOLF_CHAT_WITH_FRANKLIN","GOLF_CHAT_WITH_TREVOR","GOLF_CHAT_WITH_FRANKLIN",true,false)
            ,new SpeechData("GOLF_CHAT_WITH_TREVOR","GOLF_FORE_SELF","GOLF_CHAT_WITH_TREVOR",true,false)
            ,new SpeechData("GOLF_FORE_SELF","GOLF_FRUSTRATION","GOLF_FORE_SELF",true,false)
            ,new SpeechData("GOLF_FRUSTRATION","GOLF_GOOD_LIE_OTHER","GOLF_FRUSTRATION",true,false)
            ,new SpeechData("GOLF_GOOD_LIE_OTHER","GOLF_GOOD_LIE_SELF","GOLF_GOOD_LIE_OTHER",true,false)
            ,new SpeechData("GOLF_GOOD_LIE_SELF","GOLF_IDLE_OTHER","GOLF_GOOD_LIE_SELF",true,false)
            ,new SpeechData("GOLF_IDLE_OTHER","GOLF_MISS_PUTT_OTHER","GOLF_IDLE_OTHER",true,false)
            ,new SpeechData("GOLF_MISS_PUTT_OTHER","GOLF_MISS_PUTT_SELF","GOLF_MISS_PUTT_OTHER",true,false)
            ,new SpeechData("GOLF_MISS_PUTT_SELF","GOLF_SHOT_BAD_HIT_SELF","GOLF_MISS_PUTT_SELF",true,false)
            ,new SpeechData("GOLF_SHOT_BAD_HIT_SELF","GOLF_SHOT_GOOD_HIT_SELF","GOLF_SHOT_BAD_HIT_SELF",true,false)
            ,new SpeechData("GOLF_SHOT_GOOD_HIT_SELF","GOLF_SINK_PUTT_OTHER","GOLF_SHOT_GOOD_HIT_SELF",true,false)
            ,new SpeechData("GOLF_SINK_PUTT_OTHER","GOLF_SINK_PUTT_SELF","GOLF_SINK_PUTT_OTHER",true,false)
            ,new SpeechData("GOLF_SINK_PUTT_SELF","GOODBYE_ACROSS_STREET","GOLF_SINK_PUTT_SELF",true,false)
            ,new SpeechData("GOODBYE_ACROSS_STREET","GREET_ACROSS_STREET","GOODBYE_ACROSS_STREET",false,false)
            ,new SpeechData("GREET_ACROSS_STREET","GREET_ATTRACTIVE_F","GREET_ACROSS_STREET",false,false)
            ,new SpeechData("HAIL_CAB","HELI_APPROACHING_DISPATCH","HAIL_CAB",true,false)
            ,new SpeechData("HELI_APPROACHING_DISPATCH","HELI_MAYDAY_DISPATCH","HELI_APPROACHING_DISPATCH",false,true)
            ,new SpeechData("HELI_MAYDAY_DISPATCH","HELI_NO_VISUAL_DISPATCH","HELI_MAYDAY_DISPATCH",false,true)
            ,new SpeechData("HELI_NO_VISUAL_DISPATCH","HELI_VISUAL_HEADING_EAST_DISPATCH","HELI_NO_VISUAL_DISPATCH",false,true)
            ,new SpeechData("HELI_VISUAL_HEADING_EAST_DISPATCH","HELI_VISUAL_HEADING_NORTH_DISPATCH","HELI_VISUAL_HEADING_EAST_DISPATCH",false,true)
            ,new SpeechData("HELI_VISUAL_HEADING_NORTH_DISPATCH","HELI_VISUAL_HEADING_SOUTH_DISPATCH","HELI_VISUAL_HEADING_NORTH_DISPATCH",false,true)
            ,new SpeechData("HELI_VISUAL_HEADING_SOUTH_DISPATCH","HELI_VISUAL_HEADING_WEST_DISPATCH","HELI_VISUAL_HEADING_SOUTH_DISPATCH",false,true)
            ,new SpeechData("HELI_VISUAL_HEADING_WEST_DISPATCH","HELI_VISUAL_ON_FOOT_DISPATCH","HELI_VISUAL_HEADING_WEST_DISPATCH",false,true)
            ,new SpeechData("HELI_VISUAL_ON_FOOT_DISPATCH","HIT_BY_PLAYER","HELI_VISUAL_ON_FOOT_DISPATCH",false,true)
            ,new SpeechData("HIT_BY_PLAYER","HOOKER_CAR_INCORRECT","HIT_BY_PLAYER",true,false)
            ,new SpeechData("HOOKER_CAR_INCORRECT","HOOKER_DECLINE_SERVICE","HOOKER_CAR_INCORRECT",true,false)
            ,new SpeechData("HOOKER_DECLINE_SERVICE","HOOKER_REQUEST","HOOKER_DECLINE_SERVICE",true,false)
            ,new SpeechData("HOOKER_REQUEST","HOOKER_STORY_REVULSION_RESP","HOOKER_REQUEST",true,false)
            ,new SpeechData("HOOKER_STORY_REVULSION_RESP","HOOKER_STORY_SARCASTIC_RESP","HOOKER_STORY_REVULSION_RESP",true,false)
            ,new SpeechData("HOOKER_STORY_SARCASTIC_RESP","HOOKER_STORY_SYMPATHETIC_RESP","HOOKER_STORY_SARCASTIC_RESP",true,false)
            ,new SpeechData("HOOKER_STORY_SYMPATHETIC_RESP","HOWS_IT_GOING_FEMALE","HOOKER_STORY_SYMPATHETIC_RESP",true,false)
            ,new SpeechData("HURRY_UP","I_GOT_THIS","HURRY_UP",true,false)
            ,new SpeechData("I_GOT_THIS","IN_COVER_DODGE_BULLETS","I_GOT_THIS",true,false)
            ,new SpeechData("IN_COVER_DODGE_BULLETS","INSULT_DEAD_PED","IN_COVER_DODGE_BULLETS",true,false)
            ,new SpeechData("INSULT_DEAD_PED","JACK_VEHICLE_BACK","INSULT_DEAD_PED",true,false)
            ,new SpeechData("JACK_VEHICLE_BACK","JACKED_CAR","JACK_VEHICLE_BACK",false,false)
            ,new SpeechData("JACKED_CAR","JACKED_GENERIC","JACKED_CAR",true,false)
            ,new SpeechData("JACKED_GENERIC","JACKING_BIKE","JACKED_GENERIC",false,false)
            ,new SpeechData("JACKING_BIKE","JACKING_CAR_FEMALE","JACKING_BIKE",true,false)
            ,new SpeechData("JACKING_CAR_FEMALE","JACKING_CAR_MALE","JACKING_CAR_FEMALE",true,false)
            ,new SpeechData("JACKING_CAR_MALE","JACKING_DEAD_PED","JACKING_CAR_MALE",true,false)
            ,new SpeechData("JACKING_DEAD_PED","JACKING_GENERIC","JACKING_DEAD_PED",true,false)
            ,new SpeechData("JACKING_GENERIC","JACKING_ORDER","JACKING_GENERIC",true,false)
            ,new SpeechData("JACKING_ORDER","KIFFLOM_GREET","JACKING_ORDER",true,false)
            ,new SpeechData("KIFFLOM_RUNNING","KIFFLOM_SPRINTING","KIFFLOM_RUNNING",true,false)
            ,new SpeechData("KIFFLOM_SPRINTING","KIFFLOM_WALKING","KIFFLOM_SPRINTING",true,false)
            ,new SpeechData("KIFFLOM_WALKING","KILLED_ALL","KIFFLOM_WALKING",true,false)
            ,new SpeechData("KILLED_ALL","KNOCK_OVER_PED","KILLED_ALL",true,false)
            ,new SpeechData("KNOCK_OVER_PED","LET_ME_OUT","KNOCK_OVER_PED",true,false)
            ,new SpeechData("LET_ME_OUT","LISTEN_TO_RADIO","LET_ME_OUT",true,false)
            ,new SpeechData("LISTEN_TO_RADIO","LOCATION_ALAMO_SEA","LISTEN_TO_RADIO",true,false)
            ,new SpeechData("LOCATION_ALAMO_SEA","LOCATION_ALTA","LOCATION_ALAMO_SEA",true,false)
            ,new SpeechData("LOCATION_ALTA","LOCATION_BANHAM_CANYON","LOCATION_ALTA",true,false)
            ,new SpeechData("LOCATION_BANHAM_CANYON","LOCATION_BANNING","LOCATION_BANHAM_CANYON",true,false)
            ,new SpeechData("LOCATION_BANNING","LOCATION_BAYTREE_CANYON","LOCATION_BANNING",true,false)
            ,new SpeechData("LOCATION_BAYTREE_CANYON","LOCATION_BOLINGBROKE_PENITENTIARY","LOCATION_BAYTREE_CANYON",true,false)
            ,new SpeechData("LOCATION_BOLINGBROKE_PENITENTIARY","LOCATION_BRADDOCK_PASS","LOCATION_BOLINGBROKE_PENITENTIARY",true,false)
            ,new SpeechData("LOCATION_BRADDOCK_PASS","LOCATION_BRADDOCK_TUNNEL","LOCATION_BRADDOCK_PASS",true,false)
            ,new SpeechData("LOCATION_BRADDOCK_TUNNEL","LOCATION_BURTON","LOCATION_BRADDOCK_TUNNEL",true,false)
            ,new SpeechData("LOCATION_BURTON","LOCATION_CALAFIA_BRIDGE","LOCATION_BURTON",true,false)
            ,new SpeechData("LOCATION_CALAFIA_BRIDGE","LOCATION_CASSIDY_CREEK","LOCATION_CALAFIA_BRIDGE",true,false)
            ,new SpeechData("LOCATION_CASSIDY_CREEK","LOCATION_CHAMBERLAIN_HILLS","LOCATION_CASSIDY_CREEK",true,false)
            ,new SpeechData("LOCATION_CHAMBERLAIN_HILLS","LOCATION_CHILIAD_MOUNTAIN_STATE_WILDERNESS","LOCATION_CHAMBERLAIN_HILLS",true,false)
            ,new SpeechData("LOCATION_CHILIAD_MOUNTAIN_STATE_WILDERNESS","LOCATION_CHUMASH","LOCATION_CHILIAD_MOUNTAIN_STATE_WILDERNESS",true,false)
            ,new SpeechData("LOCATION_CHUMASH","LOCATION_COUNTRYSIDE","LOCATION_CHUMASH",true,false)
            ,new SpeechData("LOCATION_COUNTRYSIDE","LOCATION_CYPRESS_FLATS","LOCATION_COUNTRYSIDE",true,false)
            ,new SpeechData("LOCATION_CYPRESS_FLATS","LOCATION_DAVIS","LOCATION_CYPRESS_FLATS",true,false)
            ,new SpeechData("LOCATION_DAVIS","LOCATION_DAVIS_QUARTZ","LOCATION_DAVIS",true,false)
            ,new SpeechData("LOCATION_DAVIS_QUARTZ","LOCATION_DEL_PERRO","LOCATION_DAVIS_QUARTZ",true,false)
            ,new SpeechData("LOCATION_DEL_PERRO","LOCATION_DEL_PERRO_BEACH","LOCATION_DEL_PERRO",true,false)
            ,new SpeechData("LOCATION_DEL_PERRO_BEACH","LOCATION_DOWNTOWN","LOCATION_DEL_PERRO_BEACH",true,false)
            ,new SpeechData("LOCATION_DOWNTOWN","LOCATION_DOWNTOWN_VINEWOOD","LOCATION_DOWNTOWN",true,false)
            ,new SpeechData("LOCATION_DOWNTOWN_VINEWOOD","LOCATION_EAST_LOS_SANTOS","LOCATION_DOWNTOWN_VINEWOOD",true,false)
            ,new SpeechData("LOCATION_EAST_LOS_SANTOS","LOCATION_EAST_VINEWOOD","LOCATION_EAST_LOS_SANTOS",true,false)
            ,new SpeechData("LOCATION_EAST_VINEWOOD","LOCATION_ECLIPSE","LOCATION_EAST_VINEWOOD",true,false)
            ,new SpeechData("LOCATION_ECLIPSE","LOCATION_EL_BURRO_HEIGHTS","LOCATION_ECLIPSE",true,false)
            ,new SpeechData("LOCATION_EL_BURRO_HEIGHTS","LOCATION_EL_GORDO_LIGHTHOUSE","LOCATION_EL_BURRO_HEIGHTS",true,false)
            ,new SpeechData("LOCATION_EL_GORDO_LIGHTHOUSE","LOCATION_ELYSIAN_ISLAND","LOCATION_EL_GORDO_LIGHTHOUSE",true,false)
            ,new SpeechData("LOCATION_ELYSIAN_ISLAND","LOCATION_FORT_ZANCUDO","LOCATION_ELYSIAN_ISLAND",true,false)
            ,new SpeechData("LOCATION_FORT_ZANCUDO","LOCATION_GALILEE","LOCATION_FORT_ZANCUDO",true,false)
            ,new SpeechData("LOCATION_GALILEE","LOCATION_GALILEO_OBSERVATORY","LOCATION_GALILEE",true,false)
            ,new SpeechData("LOCATION_GALILEO_OBSERVATORY","LOCATION_GALILEO_PARK","LOCATION_GALILEO_OBSERVATORY",true,false)
            ,new SpeechData("LOCATION_GALILEO_PARK","LOCATION_GRAND_SENORA_DESERT","LOCATION_GALILEO_PARK",true,false)
            ,new SpeechData("LOCATION_GRAND_SENORA_DESERT","LOCATION_GRAPESEED","LOCATION_GRAND_SENORA_DESERT",true,false)
            ,new SpeechData("LOCATION_GRAPESEED","LOCATION_GREAT_CHAPARRAL","LOCATION_GRAPESEED",true,false)
            ,new SpeechData("LOCATION_GREAT_CHAPARRAL","LOCATION_GWC_AND_GOLFING_SOCIETY","LOCATION_GREAT_CHAPARRAL",true,false)
            ,new SpeechData("LOCATION_GWC_AND_GOLFING_SOCIETY","LOCATION_HARMONY","LOCATION_GWC_AND_GOLFING_SOCIETY",true,false)
            ,new SpeechData("LOCATION_HARMONY","LOCATION_HAWICK","LOCATION_HARMONY",true,false)
            ,new SpeechData("LOCATION_HAWICK","LOCATION_HEART_ATTACKS_BEACH","LOCATION_HAWICK",true,false)
            ,new SpeechData("LOCATION_HEART_ATTACKS_BEACH","LOCATION_HUMANE_LABS_AND_RESEARCH","LOCATION_HEART_ATTACKS_BEACH",true,false)
            ,new SpeechData("LOCATION_HUMANE_LABS_AND_RESEARCH","LOCATION_LA_MESA","LOCATION_HUMANE_LABS_AND_RESEARCH",true,false)
            ,new SpeechData("LOCATION_LA_MESA","LOCATION_LA_PUERTA","LOCATION_LA_MESA",true,false)
            ,new SpeechData("LOCATION_LA_PUERTA","LOCATION_LA_PUERTA_FWY","LOCATION_LA_PUERTA",true,false)
            ,new SpeechData("LOCATION_LA_PUERTA_FWY","LOCATION_LAGO_ZANCUDO","LOCATION_LA_PUERTA_FWY",true,false)
            ,new SpeechData("LOCATION_LAGO_ZANCUDO","LOCATION_LAND_ACT_DAM","LOCATION_LAGO_ZANCUDO",true,false)
            ,new SpeechData("LOCATION_LAND_ACT_DAM","LOCATION_LAND_ACT_RESERVOIR","LOCATION_LAND_ACT_DAM",true,false)
            ,new SpeechData("LOCATION_LAND_ACT_RESERVOIR","LOCATION_LITTLE_SEOUL","LOCATION_LAND_ACT_RESERVOIR",true,false)
            ,new SpeechData("LOCATION_LITTLE_SEOUL","LOCATION_LOS_SANTOS_FREEWAY","LOCATION_LITTLE_SEOUL",true,false)
            ,new SpeechData("LOCATION_LOS_SANTOS_FREEWAY","LOCATION_LOS_SANTOS_INTERNATIONAL_AIRPORT","LOCATION_LOS_SANTOS_FREEWAY",true,false)
            ,new SpeechData("LOCATION_LOS_SANTOS_INTERNATIONAL_AIRPORT","LOCATION_MAZE_BANK_ARENA","LOCATION_LOS_SANTOS_INTERNATIONAL_AIRPORT",true,false)
            ,new SpeechData("LOCATION_MAZE_BANK_ARENA","LOCATION_MIRROR_PARK","LOCATION_MAZE_BANK_ARENA",true,false)
            ,new SpeechData("LOCATION_MIRROR_PARK","LOCATION_MISSION_ROW","LOCATION_MIRROR_PARK",true,false)
            ,new SpeechData("LOCATION_MISSION_ROW","LOCATION_MORNINGWOOD","LOCATION_MISSION_ROW",true,false)
            ,new SpeechData("LOCATION_MORNINGWOOD","LOCATION_MOUNT_CHILIAD","LOCATION_MORNINGWOOD",true,false)
            ,new SpeechData("LOCATION_MOUNT_CHILIAD","LOCATION_MOUNT_GORDO","LOCATION_MOUNT_CHILIAD",true,false)
            ,new SpeechData("LOCATION_MOUNT_GORDO","LOCATION_MOUNT_JOSIAH","LOCATION_MOUNT_GORDO",true,false)
            ,new SpeechData("LOCATION_MOUNT_JOSIAH","LOCATION_MURRIETA_HEIGHTS","LOCATION_MOUNT_JOSIAH",true,false)
            ,new SpeechData("LOCATION_MURRIETA_HEIGHTS","LOCATION_NORTH_CHUMASH","LOCATION_MURRIETA_HEIGHTS",true,false)
            ,new SpeechData("LOCATION_NORTH_CHUMASH","LOCATION_NORTH_YANKTON","LOCATION_NORTH_CHUMASH",true,false)
            ,new SpeechData("LOCATION_NORTH_YANKTON","LOCATION_PACIFIC_BLUFFS","LOCATION_NORTH_YANKTON",true,false)
            ,new SpeechData("LOCATION_PACIFIC_BLUFFS","LOCATION_PACIFIC_OCEAN","LOCATION_PACIFIC_BLUFFS",true,false)
            ,new SpeechData("LOCATION_PACIFIC_OCEAN","LOCATION_PALETO_BAY","LOCATION_PACIFIC_OCEAN",true,false)
            ,new SpeechData("LOCATION_PALETO_BAY","LOCATION_PALETO_COVE","LOCATION_PALETO_BAY",true,false)
            ,new SpeechData("LOCATION_PALETO_COVE","LOCATION_PALETO_FOREST","LOCATION_PALETO_COVE",true,false)
            ,new SpeechData("LOCATION_PALETO_FOREST","LOCATION_PALMER-TAYLOR_POWER_STATION","LOCATION_PALETO_FOREST",true,false)
            ,new SpeechData("LOCATION_PALMER-TAYLOR_POWER_STATION","LOCATION_PALOMINO_HIGHLANDS","LOCATION_PALMER-TAYLOR_POWER_STATION",true,false)
            ,new SpeechData("LOCATION_PALOMINO_HIGHLANDS","LOCATION_PILLBOX_HILL","LOCATION_PALOMINO_HIGHLANDS",true,false)
            ,new SpeechData("LOCATION_PILLBOX_HILL","LOCATION_PORT_OF_SOUTH_LOS_SANTOS","LOCATION_PILLBOX_HILL",true,false)
            ,new SpeechData("LOCATION_PORT_OF_SOUTH_LOS_SANTOS","LOCATION_PROCOPIO_BEACH","LOCATION_PORT_OF_SOUTH_LOS_SANTOS",true,false)
            ,new SpeechData("LOCATION_PROCOPIO_BEACH","LOCATION_RANCHO","LOCATION_PROCOPIO_BEACH",true,false)
            ,new SpeechData("LOCATION_RANCHO","LOCATION_RATON_CANYON","LOCATION_RANCHO",true,false)
            ,new SpeechData("LOCATION_RATON_CANYON","LOCATION_REDWOOD_LIGHTS_TRACK","LOCATION_RATON_CANYON",true,false)
            ,new SpeechData("LOCATION_REDWOOD_LIGHTS_TRACK","LOCATION_RICHARDS_MAJESTIC","LOCATION_REDWOOD_LIGHTS_TRACK",true,false)
            ,new SpeechData("LOCATION_RICHARDS_MAJESTIC","LOCATION_RICHMAN","LOCATION_RICHARDS_MAJESTIC",true,false)
            ,new SpeechData("LOCATION_RICHMAN","LOCATION_RICHMAN_GLEN","LOCATION_RICHMAN",true,false)
            ,new SpeechData("LOCATION_RICHMAN_GLEN","LOCATION_ROCKFORD_HILLS","LOCATION_RICHMAN_GLEN",true,false)
            ,new SpeechData("LOCATION_ROCKFORD_HILLS","LOCATION_RON_ALTERNATES_WIND_FARM","LOCATION_ROCKFORD_HILLS",true,false)
            ,new SpeechData("LOCATION_RON_ALTERNATES_WIND_FARM","LOCATION_SAN_ANDREAS","LOCATION_RON_ALTERNATES_WIND_FARM",true,false)
            ,new SpeechData("LOCATION_SAN_ANDREAS","LOCATION_SAN_CHIANSKI_MOUNTAIN_RANGE","LOCATION_SAN_ANDREAS",true,false)
            ,new SpeechData("LOCATION_SAN_CHIANSKI_MOUNTAIN_RANGE","LOCATION_SANDY_SHORES","LOCATION_SAN_CHIANSKI_MOUNTAIN_RANGE",true,false)
            ,new SpeechData("LOCATION_SANDY_SHORES","LOCATION_SENORA_FREEWAY","LOCATION_SANDY_SHORES",true,false)
            ,new SpeechData("LOCATION_SENORA_FREEWAY","LOCATION_SLAB_CITY","LOCATION_SENORA_FREEWAY",true,false)
            ,new SpeechData("LOCATION_SLAB_CITY","LOCATION_SOUTH_LOS_SANTOS","LOCATION_SLAB_CITY",true,false)
            ,new SpeechData("LOCATION_SOUTH_LOS_SANTOS","LOCATION_STRAWBERRY","LOCATION_SOUTH_LOS_SANTOS",true,false)
            ,new SpeechData("LOCATION_STRAWBERRY","LOCATION_TATAVIAM_MOUNTAINS","LOCATION_STRAWBERRY",true,false)
            ,new SpeechData("LOCATION_TATAVIAM_MOUNTAINS","LOCATION_TERMINAL","LOCATION_TATAVIAM_MOUNTAINS",true,false)
            ,new SpeechData("LOCATION_TERMINAL","LOCATION_TEXTILE_CITY","LOCATION_TERMINAL",true,false)
            ,new SpeechData("LOCATION_TEXTILE_CITY","LOCATION_TONGVA_HILLS","LOCATION_TEXTILE_CITY",true,false)
            ,new SpeechData("LOCATION_TONGVA_HILLS","LOCATION_TONGVA_VALLEY","LOCATION_TONGVA_HILLS",true,false)
            ,new SpeechData("LOCATION_TONGVA_VALLEY","LOCATION_UTOPIA_GARDENS","LOCATION_TONGVA_VALLEY",true,false)
            ,new SpeechData("LOCATION_UTOPIA_GARDENS","LOCATION_VENICE","LOCATION_UTOPIA_GARDENS",true,false)
            ,new SpeechData("LOCATION_VENICE","LOCATION_VERNON","LOCATION_VENICE",true,false)
            ,new SpeechData("LOCATION_VERNON","LOCATION_VESPUCCI","LOCATION_VERNON",true,false)
            ,new SpeechData("LOCATION_VESPUCCI","LOCATION_VESPUCCI_BEACH","LOCATION_VESPUCCI",true,false)
            ,new SpeechData("LOCATION_VESPUCCI_BEACH","LOCATION_VESPUCCI_CANALS","LOCATION_VESPUCCI_BEACH",true,false)
            ,new SpeechData("LOCATION_VESPUCCI_CANALS","LOCATION_VINEWOOD","LOCATION_VESPUCCI_CANALS",true,false)
            ,new SpeechData("LOCATION_VINEWOOD","LOCATION_VINEWOOD_HILLS","LOCATION_VINEWOOD",true,false)
            ,new SpeechData("LOCATION_VINEWOOD_HILLS","LOCATION_VINEWOOD_RACETRACK","LOCATION_VINEWOOD_HILLS",true,false)
            ,new SpeechData("LOCATION_VINEWOOD_RACETRACK","LOCATION_W_MIRROR_DRIVE","LOCATION_VINEWOOD_RACETRACK",true,false)
            ,new SpeechData("LOCATION_W_MIRROR_DRIVE","LOCATION_WEST_VINEWOOD","LOCATION_W_MIRROR_DRIVE",true,false)
            ,new SpeechData("LOCATION_WEST_VINEWOOD","LOCATION_ZANCUDO_RIVER","LOCATION_WEST_VINEWOOD",true,false)
            ,new SpeechData("LOCATION_ZANCUDO_RIVER","LOST_SUSPECT_CHOPPER_MEGAPHONE","LOCATION_ZANCUDO_RIVER",true,false)
            ,new SpeechData("LOST_SUSPECT_CHOPPER_MEGAPHONE","MELEE_KNOCK_DOWN","LOST_SUSPECT_CHOPPER_MEGAPHONE",false,true)
            ,new SpeechData("MELEE_KNOCK_DOWN","MOVE_IN","MELEE_KNOCK_DOWN",true,false)
            ,new SpeechData("MOVE_IN","MOVE_IN_PERSONAL","MOVE_IN",false,true)
            ,new SpeechData("MOVE_IN_PERSONAL","NEAR_MISS_VEHICLE","MOVE_IN_PERSONAL",false,true)
            ,new SpeechData("NEAR_MISS_VEHICLE","NEED_A_BIGGER_VEHICLE","NEAR_MISS_VEHICLE",true,false)
            ,new SpeechData("NEED_A_BIGGER_VEHICLE","NEED_A_VEHICLE","NEED_A_BIGGER_VEHICLE",true,false)
            ,new SpeechData("NEED_A_VEHICLE","NEED_SOME_HELP","NEED_A_VEHICLE",true,false)
            ,new SpeechData("NEED_SOME_HELP","NICE_CAR","NEED_SOME_HELP",true,true)
            ,new SpeechData("NICE_CAR","NICE_CAR_SHOUT","NICE_CAR",false,false)
            ,new SpeechData("NICE_CAR_SHOUT","NICE_CAR_THANKS","NICE_CAR_SHOUT",false,false)
            ,new SpeechData("NICE_CAR_THANKS","NICE_SHOT","NICE_CAR_THANKS",false,false)
            ,new SpeechData("NICE_SHOT","NO_CHANGE","NICE_SHOT",true,false)
            ,new SpeechData("NO_CHANGE","NO_LOITERING_MEGAPHONE","NO_CHANGE",true,false)
            ,new SpeechData("NO_LOITERING_MEGAPHONE","OFFICER_DOWN","NO_LOITERING_MEGAPHONE",false,true)
            ,new SpeechData("OFFICER_DOWN","OVER_THERE","OFFICER_DOWN",false,true)
            ,new SpeechData("OVER_THERE","PED_RANT_01","OVER_THERE",false,false)
            ,new SpeechData("PHONE_SURPRISE_EXPLOSION","PHONE_SURPRISE_GUNFIRE","PHONE_SURPRISE_EXPLOSION",false,false)
            ,new SpeechData("PHONE_SURPRISE_GUNFIRE","PHONE_SURPRISE_PLAYER_APPEARANCE","PHONE_SURPRISE_GUNFIRE",false,false)
            ,new SpeechData("PHONE_SURPRISE_PLAYER_APPEARANCE","PINNED_DOWN","PHONE_SURPRISE_PLAYER_APPEARANCE",false,false)
            ,new SpeechData("PINNED_DOWN","PLAYER_BEEN_RUN_OVER","PINNED_DOWN",false,true)
            ,new SpeechData("PLAYER_BEEN_RUN_OVER","PLAYER_KILLED_FRIEND","PLAYER_BEEN_RUN_OVER",true,false)
            ,new SpeechData("PLAYER_KILLED_FRIEND","POLICE_PURSUIT_DRIVEN","PLAYER_KILLED_FRIEND",true,false)
            ,new SpeechData("POLICE_PURSUIT_DRIVEN","POLICE_PURSUIT_FALSE_ALARM","POLICE_PURSUIT_DRIVEN",true,false)
            ,new SpeechData("POLICE_PURSUIT_FALSE_ALARM","POOL_MY_BREAK","POLICE_PURSUIT_FALSE_ALARM",true,false)
            ,new SpeechData("POOL_MY_BREAK","POOL_OFFER_BET","POOL_MY_BREAK",true,false)
            ,new SpeechData("POOL_OFFER_BET","POOL_PLAY_AGAIN","POOL_OFFER_BET",true,false)
            ,new SpeechData("POOL_PLAY_AGAIN","POOL_PLAYER_FOUL","POOL_PLAY_AGAIN",true,false)
            ,new SpeechData("POOL_PLAYER_FOUL","POOL_PLAYER_GOES_FOR_BLACK","POOL_PLAYER_FOUL",true,false)
            ,new SpeechData("POOL_PLAYER_GOES_FOR_BLACK","POOL_PLAYER_HURRY_UP","POOL_PLAYER_GOES_FOR_BLACK",true,false)
            ,new SpeechData("POOL_PLAYER_HURRY_UP","POOL_PLAYER_LOSES","POOL_PLAYER_HURRY_UP",true,false)
            ,new SpeechData("POOL_PLAYER_LOSES","POOL_PLAYER_MISS","POOL_PLAYER_LOSES",true,false)
            ,new SpeechData("POOL_PLAYER_MISS","POOL_PLAYER_POTS","POOL_PLAYER_MISS",true,false)
            ,new SpeechData("POOL_PLAYER_POTS","POOL_PLAYER_POTS_MANY","POOL_PLAYER_POTS",true,false)
            ,new SpeechData("POOL_PLAYER_POTS_MANY","POOL_PLAYER_POTS_ON_BREAK","POOL_PLAYER_POTS_MANY",true,false)
            ,new SpeechData("POOL_PLAYER_POTS_ON_BREAK","POOL_PLAYER_WINS","POOL_PLAYER_POTS_ON_BREAK",true,false)
            ,new SpeechData("POOL_PLAYER_WINS","POOL_YOUR_BREAK","POOL_PLAYER_WINS",true,false)
            ,new SpeechData("POOL_YOUR_BREAK","POST_STONED","POOL_YOUR_BREAK",true,false)
            ,new SpeechData("POST_STONED","PROVOKE_BAR","POST_STONED",true,false)
            ,new SpeechData("PROVOKE_BAR","PROVOKE_BUMPED_INTO","PROVOKE_BAR",false,false)
            ,new SpeechData("PROVOKE_BUMPED_INTO","PROVOKE_FOLLOWING","PROVOKE_BUMPED_INTO",false,false)
            ,new SpeechData("PROVOKE_FOLLOWING","PROVOKE_GENERIC","PROVOKE_FOLLOWING",false,true)
            ,new SpeechData("PROVOKE_GENERIC","PROVOKE_HIT_CAR","PROVOKE_GENERIC",false,false)
            ,new SpeechData("PROVOKE_HIT_CAR","PROVOKE_STARING","PROVOKE_HIT_CAR",false,false)
            ,new SpeechData("PROVOKE_STARING","PROVOKE_TRESPASS","PROVOKE_STARING",false,true)
            ,new SpeechData("PROVOKE_TRESPASS","PURCHASE_ONLINE","PROVOKE_TRESPASS",false,true)
            ,new SpeechData("PURCHASE_ONLINE","RACE_COLLIDE_OUT_OF_BREATH","PURCHASE_ONLINE",true,false)
            ,new SpeechData("RACE_COLLIDE_OUT_OF_BREATH","RACE_CRASH","RACE_COLLIDE_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_CRASH","RACE_FINISHED_OUT_OF_BREATH","RACE_CRASH",true,false)
            ,new SpeechData("RACE_FINISHED_OUT_OF_BREATH","RACE_NEARLY_WIN","RACE_FINISHED_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_NEARLY_WIN","RACE_NEARLY_WIN_OUT_OF_BREATH","RACE_NEARLY_WIN",true,false)
            ,new SpeechData("RACE_NEARLY_WIN_OUT_OF_BREATH","RACE_RANKDOWN","RACE_NEARLY_WIN_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_RANKDOWN","RACE_RANKDOWN_OUT_OF_BREATH","RACE_RANKDOWN",true,false)
            ,new SpeechData("RACE_RANKDOWN_OUT_OF_BREATH","RACE_RANKUP","RACE_RANKDOWN_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_RANKUP","RACE_RANKUP_OUT_OF_BREATH","RACE_RANKUP",true,false)
            ,new SpeechData("RACE_RANKUP_OUT_OF_BREATH","RACE_REACH_START","RACE_RANKUP_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_REACH_START","RACE_REQUEST","RACE_REACH_START",true,false)
            ,new SpeechData("RACE_REQUEST","RACE_STAY_1ST_OUT_OF_BREATH","RACE_REQUEST",true,false)
            ,new SpeechData("RACE_STAY_1ST_OUT_OF_BREATH","RACE_STAY_POSITION_OUT_OF_BREATH","RACE_STAY_1ST_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_STAY_POSITION_OUT_OF_BREATH","RACE_WIN_OUT_OF_BREATH","RACE_STAY_POSITION_OUT_OF_BREATH",true,false)
            ,new SpeechData("RACE_WIN_OUT_OF_BREATH","RADIO_DISLIKE","RACE_WIN_OUT_OF_BREATH",true,false)
            ,new SpeechData("RADIO_DISLIKE","RADIO_LIKE","RADIO_DISLIKE",true,false)
            ,new SpeechData("RADIO_LIKE","RELOADING","RADIO_LIKE",true,false)
            ,new SpeechData("RELOADING","RELOADING_PROFESSIONAL","RELOADING",true,true)
            ,new SpeechData("RELOADING_PROFESSIONAL","REPORT_SUSPECT_CRASHED_VEHICLE","RELOADING_PROFESSIONAL",true,false)
            ,new SpeechData("REPORT_SUSPECT_CRASHED_VEHICLE","REPORT_SUSPECT_ENTERED_FREEWAY","REPORT_SUSPECT_CRASHED_VEHICLE",false,true)
            ,new SpeechData("REPORT_SUSPECT_ENTERED_FREEWAY","REPORT_SUSPECT_ENTERED_METRO","REPORT_SUSPECT_ENTERED_FREEWAY",false,true)
            ,new SpeechData("REPORT_SUSPECT_ENTERED_METRO","REPORT_SUSPECT_IS_IN_CAR","REPORT_SUSPECT_ENTERED_METRO",false,true)
            ,new SpeechData("REPORT_SUSPECT_IS_IN_CAR","REPORT_SUSPECT_IS_ON_FOOT","REPORT_SUSPECT_IS_IN_CAR",false,true)
            ,new SpeechData("REPORT_SUSPECT_IS_ON_FOOT","REPORT_SUSPECT_IS_ON_MOTORCYCLE","REPORT_SUSPECT_IS_ON_FOOT",false,true)
            ,new SpeechData("REPORT_SUSPECT_IS_ON_MOTORCYCLE","REPORT_SUSPECT_LEFT_FREEWAY","REPORT_SUSPECT_IS_ON_MOTORCYCLE",false,true)
            ,new SpeechData("REPORT_SUSPECT_LEFT_FREEWAY","REQUEST_BACKUP","REPORT_SUSPECT_LEFT_FREEWAY",false,true)
            ,new SpeechData("REQUEST_BACKUP","REQUEST_GUIDANCE_DISPATCH","REQUEST_BACKUP",false,true)
            ,new SpeechData("REQUEST_GUIDANCE_DISPATCH","REQUEST_NOOSE","REQUEST_GUIDANCE_DISPATCH",false,true)
            ,new SpeechData("REQUEST_NOOSE","RESCUE_INJURED_COP","REQUEST_NOOSE",false,true)
            ,new SpeechData("RESCUE_INJURED_COP","ROBBERY_FRIEND_WITNESS","RESCUE_INJURED_COP",false,true)
            ,new SpeechData("ROBBERY_FRIEND_WITNESS","ROLLERCOASTER_CHAT_EXCITED","ROBBERY_FRIEND_WITNESS",true,false)
            ,new SpeechData("ROLLERCOASTER_CHAT_EXCITED","ROLLERCOASTER_CHAT_NORMAL","ROLLERCOASTER_CHAT_EXCITED",true,false)
            ,new SpeechData("ROLLERCOASTER_CHAT_NORMAL","SEE_WEIRDO","ROLLERCOASTER_CHAT_NORMAL",true,false)
            ,new SpeechData("SEE_WEIRDO","SEE_WEIRDO_PHONE","SEE_WEIRDO",false,false)
            ,new SpeechData("SEE_WEIRDO_PHONE","SETTLE_DOWN","SEE_WEIRDO_PHONE",false,false)
            ,new SpeechData("SETTLE_DOWN","SEX_CLIMAX","SETTLE_DOWN",false,true)
            ,new SpeechData("SEX_CLIMAX","SEX_GENERIC","SEX_CLIMAX",true,false)
            ,new SpeechData("SEX_GENERIC","SHOOT","SEX_GENERIC",true,false)
            ,new SpeechData("SHOOT","SHOOTOUT_OPEN_FIRE","SHOOT",true,false)
            ,new SpeechData("SHOOTOUT_OPEN_FIRE","SHOOTOUT_READY","SHOOTOUT_OPEN_FIRE",false,true)
            ,new SpeechData("SHOOTOUT_READY","SHOOTOUT_READY_RESP","SHOOTOUT_READY",false,true)
            ,new SpeechData("SHOOTOUT_READY_RESP","SHOP_HOLDUP","SHOOTOUT_READY_RESP",false,true)
            ,new SpeechData("SHOP_HOLDUP","SHOP_HURRY","SHOP_HOLDUP",true,false)
            ,new SpeechData("SHOP_HURRY","SHOT_AT_HELI_MEGAPHONE","SHOP_HURRY",true,false)
            ,new SpeechData("SHOT_AT_HELI_MEGAPHONE","SHOT_BY_PLAYER","SHOT_AT_HELI_MEGAPHONE",false,true)
            ,new SpeechData("SHOT_BY_PLAYER","SHOT_TYRE_CHOPPER_MEGAPHONE","SHOT_BY_PLAYER",true,false)
            ,new SpeechData("SHOT_TYRE_CHOPPER_MEGAPHONE","SHOUT_PERV_AT_WOMAN_PERV","SHOT_TYRE_CHOPPER_MEGAPHONE",false,true)
            ,new SpeechData("SHOUT_PERV_AT_WOMAN_PERV","SPOT_POLICE","SHOUT_PERV_AT_WOMAN_PERV",false,false)
            ,new SpeechData("SPOT_POLICE","SPOT_SUSPECT_CHOPPER_MEGAPHONE","SPOT_POLICE",true,false)
            ,new SpeechData("SPOT_SUSPECT_CHOPPER_MEGAPHONE","START_CAR_PANIC","SPOT_SUSPECT_CHOPPER_MEGAPHONE",false,true)
            ,new SpeechData("START_CAR_PANIC","STAY_DOWN","START_CAR_PANIC",true,false)
            ,new SpeechData("STAY_DOWN","STEALTH_KILL","STAY_DOWN",true,false)
            ,new SpeechData("STEALTH_KILL","STOP_ON_FOOT_CHOPPER_MEGAPHONE","STEALTH_KILL",true,false)
            ,new SpeechData("STOP_ON_FOOT_CHOPPER_MEGAPHONE","STOP_ON_FOOT_MEGAPHONE","STOP_ON_FOOT_CHOPPER_MEGAPHONE",false,true)
            ,new SpeechData("STOP_ON_FOOT_MEGAPHONE","STOP_VEHICLE_BOAT_MEGAPHONE","STOP_ON_FOOT_MEGAPHONE",false,true)
            ,new SpeechData("STOP_VEHICLE_BOAT_MEGAPHONE","STOP_VEHICLE_CAR_MEGAPHONE","STOP_VEHICLE_BOAT_MEGAPHONE",false,true)
            ,new SpeechData("STOP_VEHICLE_CAR_MEGAPHONE","STOP_VEHICLE_CAR_WARNING_MEGAPHONE","STOP_VEHICLE_CAR_MEGAPHONE",false,true)
            ,new SpeechData("STOP_VEHICLE_CAR_WARNING_MEGAPHONE","STOP_VEHICLE_GENERIC_MEGAPHONE","STOP_VEHICLE_CAR_WARNING_MEGAPHONE",false,true)
            ,new SpeechData("STOP_VEHICLE_GENERIC_MEGAPHONE","STOP_VEHICLE_GENERIC_WARNING_MEGAPHONE","STOP_VEHICLE_GENERIC_MEGAPHONE",false,true)
            ,new SpeechData("STOP_VEHICLE_GENERIC_WARNING_MEGAPHONE","STRIP_2ND_DANCE_ACCEPT","STOP_VEHICLE_GENERIC_WARNING_MEGAPHONE",false,true)
            ,new SpeechData("STRIP_2ND_DANCE_ACCEPT","STRIP_2ND_DANCE_DECLINE","STRIP_2ND_DANCE_ACCEPT",true,false)
            ,new SpeechData("STRIP_2ND_DANCE_DECLINE","STRIP_ACCEPTS_DANCE","STRIP_2ND_DANCE_DECLINE",true,false)
            ,new SpeechData("STRIP_ACCEPTS_DANCE","STRIP_ARRIVE_EXPECTED","STRIP_ACCEPTS_DANCE",true,false)
            ,new SpeechData("STRIP_ARRIVE_EXPECTED","STRIP_ARRIVE_UNEXPECTED","STRIP_ARRIVE_EXPECTED",true,false)
            ,new SpeechData("STRIP_ARRIVE_UNEXPECTED","STRIP_COMMENT_DANCE_MULTI","STRIP_ARRIVE_UNEXPECTED",true,false)
            ,new SpeechData("STRIP_COMMENT_DANCE_MULTI","STRIP_COMMENT_DANCE_SINGLE","STRIP_COMMENT_DANCE_MULTI",true,false)
            ,new SpeechData("STRIP_COMMENT_DANCE_SINGLE","STRIP_DANCE_DECLINE_POLITE","STRIP_COMMENT_DANCE_SINGLE",true,false)
            ,new SpeechData("STRIP_DANCE_DECLINE_POLITE","STRIP_DANCE_QUIT","STRIP_DANCE_DECLINE_POLITE",true,false)
            ,new SpeechData("STRIP_DANCE_QUIT","STRIP_DO_OWN_THING","STRIP_DANCE_QUIT",true,false)
            ,new SpeechData("STRIP_DO_OWN_THING","STRIP_DROP_CASH","STRIP_DO_OWN_THING",true,false)
            ,new SpeechData("STRIP_DROP_CASH","STRIP_DUO_REQUEST","STRIP_DROP_CASH",true,false)
            ,new SpeechData("STRIP_DUO_REQUEST","STRIP_ENJOYING_SELF","STRIP_DUO_REQUEST",true,false)
            ,new SpeechData("STRIP_ENJOYING_SELF","STRIP_HOME_REQUEST","STRIP_ENJOYING_SELF",true,false)
            ,new SpeechData("STRIP_HOME_REQUEST","STRIP_INVITE_HOME_ACCEPT","STRIP_HOME_REQUEST",true,false)
            ,new SpeechData("STRIP_INVITE_HOME_ACCEPT","STRIP_INVITE_HOME_DECLINE","STRIP_INVITE_HOME_ACCEPT",true,false)
            ,new SpeechData("STRIP_INVITE_HOME_DECLINE","STRIP_THANKS","STRIP_INVITE_HOME_DECLINE",true,false)
            ,new SpeechData("STRIP_THANKS","STRIP_TOUCH_COMMENT","STRIP_THANKS",true,false)
            ,new SpeechData("STRIP_TOUCH_COMMENT","SURROUNDED","STRIP_TOUCH_COMMENT",true,false)
            ,new SpeechData("SURROUNDED","SUSPECT_KILLED","SURROUNDED",false,true)
            ,new SpeechData("SUSPECT_KILLED","SUSPECT_KILLED_REPORT","SUSPECT_KILLED",false,true)
            ,new SpeechData("SUSPECT_KILLED_REPORT","SUSPECT_LOST","SUSPECT_KILLED_REPORT",false,true)
            ,new SpeechData("SUSPECT_LOST","SUSPECT_SPOTTED","SUSPECT_LOST",false,true)
            ,new SpeechData("SUSPECT_SPOTTED","TAKE_COVER","SUSPECT_SPOTTED",false,true)
            ,new SpeechData("TAKE_COVER","TAXI_CHANGE_DEST","TAKE_COVER",false,true)
            ,new SpeechData("TAXI_CHANGE_DEST","TAXI_HAIL","TAXI_CHANGE_DEST",true,false)
            ,new SpeechData("TAXI_HAIL","TAXI_STEP_ON_IT","TAXI_HAIL",true,false)
            ,new SpeechData("TAXI_STEP_ON_IT","TENNIS_ABOUT_TO_SERVE_MIDGAME_WITH_AMANDA","TAXI_STEP_ON_IT",true,false)
            ,new SpeechData("TENNIS_ABOUT_TO_SERVE_MIDGAME_WITH_AMANDA","TENNIS_ACE","TENNIS_ABOUT_TO_SERVE_MIDGAME_WITH_AMANDA",true,false)
            ,new SpeechData("TENNIS_ACE","TENNIS_CHAT","TENNIS_ACE",true,false)
            ,new SpeechData("TENNIS_CHAT","TENNIS_CHAT_WITH_PLAYER","TENNIS_CHAT",true,false)
            ,new SpeechData("TENNIS_CHAT_WITH_PLAYER","TENNIS_CLOSE_LINE_CALL","TENNIS_CHAT_WITH_PLAYER",true,false)
            ,new SpeechData("TENNIS_CLOSE_LINE_CALL","TENNIS_EXHERT","TENNIS_CLOSE_LINE_CALL",true,false)
            ,new SpeechData("TENNIS_EXHERT","TENNIS_HURRY_UP","TENNIS_EXHERT",true,false)
            ,new SpeechData("TENNIS_HURRY_UP","TENNIS_LONG_RALLY","TENNIS_HURRY_UP",true,false)
            ,new SpeechData("TENNIS_LONG_RALLY","TENNIS_LOSE","TENNIS_LONG_RALLY",true,false)
            ,new SpeechData("TENNIS_LOSE","TENNIS_LUCKY","TENNIS_LOSE",true,false)
            ,new SpeechData("TENNIS_LUCKY","TENNIS_OUT","TENNIS_LUCKY",true,false)
            ,new SpeechData("TENNIS_OUT","TENNIS_PLAYER_HIT_NET","TENNIS_OUT",true,false)
            ,new SpeechData("TENNIS_PLAYER_HIT_NET","TENNIS_START_EARLY_STORY_WITH_AMANDA","TENNIS_PLAYER_HIT_NET",true,false)
            ,new SpeechData("TENNIS_START_EARLY_STORY_WITH_AMANDA","TENNIS_START_END_STORY_WITH_AMANDA","TENNIS_START_EARLY_STORY_WITH_AMANDA",true,false)
            ,new SpeechData("TENNIS_START_END_STORY_WITH_AMANDA","TENNIS_START_WITH_AMANDA","TENNIS_START_END_STORY_WITH_AMANDA",true,false)
            ,new SpeechData("TENNIS_START_WITH_AMANDA","TENNIS_START_WITH_TREVOR","TENNIS_START_WITH_AMANDA",true,false)
            ,new SpeechData("TENNIS_START_WITH_TREVOR","TENNIS_WIN","TENNIS_START_WITH_TREVOR",true,false)
            ,new SpeechData("TENNIS_WIN","TRAPPED","TENNIS_WIN",true,false)
            ,new SpeechData("TRAPPED","TRIATHLON_COMMENT","TRAPPED",false,true)
            ,new SpeechData("TRIATHLON_COMMENT","TURN_RADIO_OFF","TRIATHLON_COMMENT",true,false)
            ,new SpeechData("TURN_RADIO_OFF","TURN_RADIO_ON","TURN_RADIO_OFF",true,false)
            ,new SpeechData("TURN_RADIO_ON","TV_BORED","TURN_RADIO_ON",true,false)
            ,new SpeechData("TV_BORED","UNIT_RESPONDING_DISPATCH","TV_BORED",true,false)
            ,new SpeechData("UNIT_RESPONDING_DISPATCH","UP_THERE","UNIT_RESPONDING_DISPATCH",false,true)
            ,new SpeechData("UP_THERE","VEHICLE_COLLIDE_BIRD_GENERIC","UP_THERE",false,false)
            ,new SpeechData("VEHICLE_COLLIDE_BIRD_GENERIC","VEHICLE_COLLIDE_DEER","VEHICLE_COLLIDE_BIRD_GENERIC",true,false)
            ,new SpeechData("VEHICLE_COLLIDE_DEER","VEHICLE_COLLIDE_DOG","VEHICLE_COLLIDE_DEER",true,false)
            ,new SpeechData("VEHICLE_COLLIDE_DOG","VEHICLE_COLLIDE_HORSE","VEHICLE_COLLIDE_DOG",true,false)
            ,new SpeechData("VEHICLE_COLLIDE_HORSE","VEHICLE_COLLIDE_OFFROAD_ANIMAL_GENERIC","VEHICLE_COLLIDE_HORSE",true,false)
            ,new SpeechData("VEHICLE_COLLIDE_OFFROAD_ANIMAL_GENERIC","VEHICLE_COLLIDE_ONROAD_ANIMAL_GENERIC","VEHICLE_COLLIDE_OFFROAD_ANIMAL_GENERIC",true,false)
            ,new SpeechData("VEHICLE_COLLIDE_ONROAD_ANIMAL_GENERIC","VEHICLE_FLIPPED","VEHICLE_COLLIDE_ONROAD_ANIMAL_GENERIC",true,false)
            ,new SpeechData("VEHICLE_FLIPPED","VEHICLE_JUMP","VEHICLE_FLIPPED",true,false)
            ,new SpeechData("VEHICLE_JUMP","VEHICLE_ON_FIRE","VEHICLE_JUMP",true,false)
            ,new SpeechData("VEHICLE_ON_FIRE","VEND_NO_ITEM","VEHICLE_ON_FIRE",true,false)
            ,new SpeechData("VEND_NO_ITEM","WAIT","VEND_NO_ITEM",true,false)
            ,new SpeechData("WAIT","WAIT_FOR_ME","WAIT",false,true)
            ,new SpeechData("WAIT_FOR_ME","WHISTLES","WAIT_FOR_ME",true,false)
            ,new SpeechData("WHISTLES","WON_DISPUTE","WHISTLES",true,false)
            ,new SpeechData("WORKOUT_FINISHED","YOU_DRIVE","WORKOUT_FINISHED",true,false)
            ,new SpeechData("YOU_DRIVE","","YOU_DRIVE",true,false)

        };
        Serialization.SerializeParams(SpeechLookups, ConfigFileName);
    }

}



//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//public class Speeches : ISpeeches
//{
//    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Speeches.xml";

//    public List<SpeechData> SpeechLookups { get; set; } = new List<SpeechData>();
//    public Speeches()
//    {

//    }
//    public void ReadConfig()
//    {
//        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
//        FileInfo ConfigFile = LSRDirectory.GetFiles("Speeches*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
//        if (ConfigFile != null)
//        {
//            EntryPoint.WriteToConsole($"Loaded Speeches config: {ConfigFile.FullName}", 0);
//            SpeechLookups = Serialization.DeserializeParams<SpeechData>(ConfigFile.FullName);
//        }
//        else if (File.Exists(ConfigFileName))
//        {
//            EntryPoint.WriteToConsole($"Loaded Speeches config  {ConfigFileName}", 0);
//            SpeechLookups = Serialization.DeserializeParams<SpeechData>(ConfigFileName);
//        }
//        else
//        {
//            EntryPoint.WriteToConsole($"No Speeches config found, creating default", 0);
//            DefaultConfig();
//        }
//    }
//    public void DefaultConfig()
//    {
//        SpeechLookups = new List<SpeechData>()
//        {
//            new SpeechData("AGREE_ACROSS_STREET","Agree (Across Street)",false,"Chat","Agree","Shout",false)
//            ,new SpeechData("GENERIC_AGREE","Agree",false,"Chat","Agree","",false)
//            ,new SpeechData("GENERIC_BUY","Buy",false,"Chat","Buy","",false)
//            ,new SpeechData("CHAT_ACROSS_STREET_RESP","Chat Across Street Response",false,"Chat","Chat","Response",true)
//            ,new SpeechData("CHAT_ACROSS_STREET_STATE","Chat Across Street State",false,"Chat","Chat","Shout",false)
//            ,new SpeechData("CHAT_RESP","Chat Reponse",false,"Chat","Chat","Response",true)
//            ,new SpeechData("CHAT_STATE","Chat State",false,"Chat","Chat","",true)
//            ,new SpeechData("GENERIC_CHEER","Cheer",false,"Chat","Cheer","",false)
//            ,new SpeechData("LOOKING_AT_PHONE","Look At Phone",false,"Chat","Look At phone","",false)
//            ,new SpeechData("NICE_CAR_SHOUT","Nice Car (Shout)",false,"Chat","Nice Car","Shout",false)
//            ,new SpeechData("NICE_CAR","Nice Car",false,"Chat","Nice Car","",false)
//            ,new SpeechData("GENERIC_NO","No",false,"Chat","No","",false)
//            ,new SpeechData("PHONE_CONV1_CHAT1","Phone Chat 1-1",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV1_CHAT2","Phone Chat 1-2",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV1_CHAT3","Phone Chat 1-3",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV1_INTRO","Phone Chat 1 Start",false,"Chat","Phone Chat","Start",false)
//            ,new SpeechData("PHONE_CONV1_OUTRO","Phone Chat 1 End",false,"Chat","Phone Chat","End",false)
//            ,new SpeechData("PHONE_CONV2_CHAT1","Phone Chat 2-1",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV2_CHAT2","Phone Chat 2-2",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV2_CHAT3","Phone Chat 2-3",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV2_INTRO","Phone Chat 2 Start",false,"Chat","Phone Chat","Start",false)
//            ,new SpeechData("PHONE_CONV2_OUTRO","Phone Chat 2 End",false,"Chat","Phone Chat","End",false)
//            ,new SpeechData("PHONE_CONV3_CHAT1","Phone Chat 3-1",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV3_CHAT2","Phone Chat 3-2",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV3_CHAT3","Phone Chat 3-3",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV3_INTRO","Phone Chat 3 Start",false,"Chat","Phone Chat","Start",false)
//            ,new SpeechData("PHONE_CONV3_OUTRO","Phone Chat 3 End",false,"Chat","Phone Chat","End",false)
//            ,new SpeechData("PHONE_CONV4_CHAT1","Phone Chat 4-1",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV4_CHAT2","Phone Chat 4-2",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV4_CHAT3","Phone Chat 4-3",false,"Chat","Phone Chat","",true)
//            ,new SpeechData("PHONE_CONV4_INTRO","Phone Chat 4 Start",false,"Chat","Phone Chat","Start",false)
//            ,new SpeechData("PHONE_CONV4_OUTRO","Phone Chat 4 End",false,"Chat","Phone Chat","End",false)
//            ,new SpeechData("PED_RANT_01","Rant",false,"Chat","Rant","",true)
//            ,new SpeechData("PED_RANT","Rant",false,"Chat","Rant","",true)
//            ,new SpeechData("STEPPED_IN_SHIT","Stepped In Shit",false,"Chat","Stepped In Shit","",false)
//            ,new SpeechData("GENERIC_YES","Yes",false,"Chat","Yes","",false)
//            ,new SpeechData("GENERIC_BYE","Bye",false,"Greet","Bye","",false)
//            ,new SpeechData("GOODBYE_ACROSS_STREET_FEMALE","Goodbye (Across Street)",false,"Greet","Goodbye","Shout",false)
//            ,new SpeechData("GOODBYE_ACROSS_STREET","Goodbye (Across Street)",false,"Greet","Goodbye","Shout",false)
//            ,new SpeechData("GREET_ACROSS_SREET_FEMALE","Greet (Across Street)",false,"Greet","Greet","Shout",false)
//            ,new SpeechData("GREET_ACROSS_SREET","Greet (Across Street)",false,"Greet","Greet","Shout",false)
//            ,new SpeechData("GREET_ACROSS_STREET","Greet (Across Street)",false,"Greet","Greet","Shout",false)
//            ,new SpeechData("GENERIC_HI","Hi",false,"Greet","Hi","",false)
//            ,new SpeechData("GENERIC_HOWS_IT_GOING","How's It Going",false,"Greet","How's It Going","",false)
//            ,new SpeechData("HOWS_IT_GOING_GENERIC","How's It Going",false,"Greet","How's It Going","",false)
//            ,new SpeechData("KIFFLOM_GREET","Kifflom Greet",false,"Greet","Kifflom","",false)
//            ,new SpeechData("CHALLENGE_ACCEPTED_HIT_CAR","Challenge Accepted (Hit Car)",false,"React","Chellenge Accepted","Hit Car",false)
//            ,new SpeechData("GUN_COOL","Cool Gun",false,"React","Cool Gun","",false)
//            ,new SpeechData("OVER_THERE","Over There",false,"React","Over There","",false)
//            ,new SpeechData("PROVOKE_BAR","Provoke (Bar)",false,"React","Provoke","Bar",false)
//            ,new SpeechData("PROVOKE_FOLLOWING","Provoke (Weirdo)",false,"React","Provoke","Weirdo",false)
//            ,new SpeechData("PROVOKE_GENERIC","Provoke",false,"React","Provoke","",false)
//            ,new SpeechData("PROVOKE_TRESPASS","Provoke (Trespass)",false,"React","Provoke","Trespass",false)
//            ,new SpeechData("SEE_WEIRDO_PHONE","See Weirdo (Phone)",false,"React","See Weirdo","Phone",false)
//            ,new SpeechData("SEE_WEIRDO","See Weirdo",false,"React","See Weirdo","",false)
//            ,new SpeechData("GENERIC_SHOCKED_HIGH","Shocked (High)",false,"React","Shocked","High",false)
//            ,new SpeechData("GENERIC_SHOCKED_MED","Shocked (Medium)",false,"React","Shocked","Medium",false)
//            ,new SpeechData("GENERIC_THANKS","Thanks",false,"React","Thanks","",false)
//            ,new SpeechData("UP_THERE","Up There",false,"React","Up There","",false)
//            ,new SpeechData("GENERIC_WHATEVER","Whatever",false,"React","Whatever","",false)
//            ,new SpeechData("PHONE_CALL_COPS","Call Cops",false,"Scared","Call Cops","",false)
//            ,new SpeechData("GENERIC_FREIGHTENED_HIGH","Frightened (High)",false,"Scared","Frightened","High",false)
//            ,new SpeechData("GENERIC_FREIGHTENED_MED","Frightened (Medium)",false,"Scared","Frightened","Medium",false)
//            ,new SpeechData("GENERIC_FRIGHTENED_HIGH","Frightened (High)",false,"Scared","Frightened","High",false)
//            ,new SpeechData("GENERIC_FRIGHTENED_MED","Fightened (Medium)",false,"Scared","Frightened","Medium",false)
//            ,new SpeechData("CALL_COPS_COMMIT","Say You Are Calling Cops",false,"Scared","Say You Are Calling the Cops","",false)
//            ,new SpeechData("CALL_COPS_THREAT","Threaten Call Cops",false,"Scared","Threaten To Call the Cops","",false)
//            ,new SpeechData("TRAPPED","Trapped",false,"Scared","Trapped","",false)
//            ,new SpeechData("APOLOGY_NO_TROUBLE","Apologize",false,"Threaten","Apologize","",false)
//            ,new SpeechData("BLOCKED_GENERIC","Blocked",true,"Threaten","Blocked","",false)
//            ,new SpeechData("BUMP","Bump",true,"Threaten","Bump","",false)
//            ,new SpeechData("CHALLENGE_ACCEPTED_BUMPED_INTO","Challenge Accepted (Bumped)",true,"Threaten","Challenge Accepted","Bumped",false)
//            ,new SpeechData("CHALLENGE_ACCEPTED_GENERIC","Challenge Accepted",true,"Threaten","Challenge Accepted","",false)
//            ,new SpeechData("GENERIC_CURSE_HIGH","Curse (High)",true,"Threaten","Curse","High",false)
//            ,new SpeechData("GENERIC_CURSE_MED","Curse (Medium)",true,"Threaten","Curse","Medium",false)
//            ,new SpeechData("FIGHT_RUN","Fight (Run)",true,"Threaten","Fight","Run",false)
//            ,new SpeechData("FIGHT","Fight",true,"Threaten","Fight","",false)
//            ,new SpeechData("GENERIC_INSULT_HIGH","Insult (High)",true,"Threaten","Insult","High",false)
//            ,new SpeechData("GENERIC_INSULT_MED","Insult (Medium)",true,"Threaten","Insult","Medium",false)
//            ,new SpeechData("SHOUT_INSULT","Insult (Shouted)",true,"Threaten","Insult","Shouted",false)
//            ,new SpeechData("CHALLENGE_THREATEN","Challenge Threaten",true,"Threaten","Threaten","",false)
//            ,new SpeechData("WON_DISPUTE","Won Dispute",false,"Threaten","Won Dispute","",false)
//        };
//        Serialization.SerializeParams(SpeechLookups, ConfigFileName);
//    }

//}
