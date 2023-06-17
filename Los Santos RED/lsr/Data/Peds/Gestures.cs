using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Gestures : IGestures
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Gestures.xml";
    public Gestures()
    {
        
    }
    public List<GestureData> GestureLookups { get; set; } = new List<GestureData>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Gestures*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Gestures config: {ConfigFile.FullName}",0);
            GestureLookups = Serialization.DeserializeParams<GestureData>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Gestures config  {ConfigFileName}",0);
            GestureLookups = Serialization.DeserializeParams<GestureData>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Gestures config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void DefaultConfig()
    {
        GestureLookups = new List<GestureData>()
        {
            new GestureData("The Finger Quick","anim@mp_player_intselfiethe_bird","enter") {IsInsulting = true,IsOnActionWheel = true },//left hand middle finger close
            new GestureData("The Finger Alt Quick","mp_player_int_upperfinger","mp_player_int_finger_02_enter") {IsInsulting = true,IsOnActionWheel = true },//Both HAnds middle finger
            new GestureData("Double Finger Quick","mp_player_int_upperfinger","mp_player_int_finger_01_enter") {IsInsulting = true ,IsOnActionWheel = true},//Both HAnds middle finger
            new GestureData("Thumbs Up Quick","anim@mp_player_intselfiethumbs_up","enter") { IsOnActionWheel = true },
            new GestureData("Wank Quick","anim@mp_player_intselfiewank","enter") { IsOnActionWheel = true },
//# if DEBUG
//            new GestureData("Wank Full","anim@mp_player_intselfiewank","idle_a","enter","exit"),
//            new GestureData("Blow Kiss Full","anim@mp_player_intselfieblow_kiss","idle_a","enter","exit"),//looks weird
//            new GestureData("Chicken Full","anim@mp_player_intupperchicken_taunt","idle_a","enter","exit"),
//            new GestureData("Chin Brush Full","anim@mp_player_intupperchin_brush","idle_a","enter","exit"),
//            new GestureData("Gang Signs Full","mp_player_int_uppergang_sign_a","mp_player_int_gang_sign_a","mp_player_int_gang_sign_a_enter","mp_player_int_gang_sign_a_exit"),//Both HAnds middle finger
//#endif
            new GestureData("Bring It On","gesture_bring_it_on") {IsInsulting = true, IsOnActionWheel = true },
            new GestureData("Bye (Hard)","gesture_bye_hard") { IsOnActionWheel = true },
            new GestureData("Bye (Soft)","gesture_bye_soft"),
            new GestureData("Come Here (Hard)","gesture_come_here_hard") { IsOnActionWheel = true },
            new GestureData("Come Here (Soft)","gesture_come_here_soft"),
            new GestureData("Damn","gesture_damn") { IsOnActionWheel = true },
            new GestureData("Displeased","gesture_displeased") { IsOnActionWheel = true },
            new GestureData("Easy Now","gesture_easy_now") { IsOnActionWheel = true },
            new GestureData("Hand Down","gesture_hand_down") { IsOnActionWheel = true },
            new GestureData("Hand Left","gesture_hand_left") { IsOnActionWheel = true },
            new GestureData("Hand Right","gesture_hand_right") { IsOnActionWheel = true },
            new GestureData("Head No","gesture_head_no") { IsOnActionWheel = true },
            new GestureData("Hello","gesture_hello") { IsOnActionWheel = true },
            new GestureData("I Will","gesture_i_will"),
            new GestureData("Me","gesture_me"),
            new GestureData("Me (Hard)","gesture_me_hard") { IsOnActionWheel = true },
            new GestureData("No Way","gesture_no_way"),
            new GestureData("Nod No (Hard)","gesture_nod_no_hard") { IsOnActionWheel = true },
            new GestureData("Nod No (Soft)","gesture_nod_no_soft"),
            new GestureData("Nod Yes (Hard)","gesture_nod_yes_hard") { IsOnActionWheel = true },
            new GestureData("Nod Yes (Soft)","gesture_nod_yes_soft"),
            new GestureData("Pleased","gesture_pleased"),


//#if DEBUG
//            new GestureData("Point","gesture_point") { IsOnActionWheel = true,SetRepeat = true },
//            new GestureData("Shrug (Hard)","gesture_shrug_hard") { IsOnActionWheel = true,IsWholeBody = true,SetRepeat = true },
//             new GestureData("What (Hard)","gesture_what_hard") { IsOnActionWheel = true,IsWholeBody = true },
//#else
//            new GestureData("Point","gesture_point") { IsOnActionWheel = true },
//            new GestureData("Shrug (Hard)","gesture_shrug_hard") { IsOnActionWheel = true },
//             new GestureData("What (Hard)","gesture_what_hard") { IsOnActionWheel = true },
//#endif






            new GestureData("Shrug (Soft)","gesture_shrug_soft"),
            new GestureData("What (Hard)","gesture_what_hard") { IsOnActionWheel = true },
            new GestureData("What (Soft)","gesture_what_soft"),
            new GestureData("Why","gesture_why") { IsOnActionWheel = true },
            new GestureData("Why Left","gesture_why_left"),
            new GestureData("You (Hard)","gesture_you_hard") { IsOnActionWheel = true },
            new GestureData("You (Soft)","gesture_you_soft"),
            new GestureData("Its Mine","getsure_its_mine") { IsOnActionWheel = true },


            //Idles

            new GestureData("Hair Touch","amb@code_human_wander_idles@female@idle_a","idle_a_hairtouch") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Sneeze","amb@code_human_wander_idles@female@idle_a","idle_b_sneeze") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Look Around","amb@code_human_wander_idles@female@idle_a","idle_c_lookaround") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Trip","amb@code_human_wander_idles@female@idle_b","idle_d_trip") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Wipe Forehead","amb@code_human_wander_idles@female@idle_b","idle_e_wipeforehead") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Check Watch","amb@code_human_wander_idles@female@idle_b","idle_f_checkwatch") { IsOnActionWheel = true, Category = "Idles" },


            new GestureData("Wrist Watch","amb@code_human_wander_idles@female@idle_a","idle_a_wristwatch") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Rub Nose","amb@code_human_wander_idles@female@idle_a","idle_b_rubnose") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Back Scratch","amb@code_human_wander_idles@female@idle_a","idle_c_backscratch") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Eye Rub","amb@code_human_wander_idles@male@idle_b","idle_d_eyerub") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Look Around","amb@code_human_wander_idles@male@idle_b","idle_e_lookaround") { IsOnActionWheel = true, Category = "Idles" },
            new GestureData("Knuckle Crack","amb@code_human_wander_idles@male@idle_b","idle_f_knucklecrack") { IsOnActionWheel = true, Category = "Idles" },


        };
        Serialization.SerializeParams(GestureLookups, ConfigFileName);
    }
    public GestureData GetRandomGesture()
    {
        return GestureLookups.PickRandom();
    }

}

