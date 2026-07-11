using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Dances : IDances
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Dances.xml";

    public List<DanceData> DanceLookups { get; set; } = new List<DanceData>();
    public Dances()
    {

    }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "Dances*.xml" : $"Dances_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded Dances config: {ConfigFile.FullName}", 0);
            DanceLookups = Serialization.DeserializeParams<DanceData>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Dances config  {ConfigFileName}", 0);
            DanceLookups = Serialization.DeserializeParams<DanceData>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Dances config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void DefaultConfig()
    {
        DanceLookups = new List<DanceData>()
        {
            new DanceData("Default","missfbi3_sniping","dance_m_default") { IsOnActionWheel = true },

            new DanceData("Nightclub A-A","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { IsOnActionWheel = true, FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },
            new DanceData("Nightclub A-B","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_b_dance_idle","ped_b_dance_intro","ped_b_dance_exit") { IsOnActionWheel = true, FacialAnimationEnter = "ped_b_dance_intro_facial", FacialAnimationIdle = "ped_b_dance_idle_facial", FacialAnimationExit = "ped_b_dance_exit_facial" },
            new DanceData("Nightclub B-A","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { IsOnActionWheel = true, FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },
            new DanceData("Nightclub B-B","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_b_dance_idle","ped_b_dance_intro","ped_b_dance_exit"){ IsOnActionWheel = true, FacialAnimationEnter = "ped_b_dance_intro_facial", FacialAnimationIdle = "ped_b_dance_idle_facial", FacialAnimationExit = "ped_b_dance_exit_facial" },
            new DanceData("Nightclub D-A","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { IsOnActionWheel = true, FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },

            new DanceData("Nightclub D-B","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_b_dance_idle") { IsOnActionWheel = true },
            new DanceData("Nightclub E-A","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_a_dance_idle"),
            new DanceData("Nightclub E-B","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_b_dance_idle") { IsOnActionWheel = true },
            new DanceData("Nightclub F-A","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_a_dance_idle"),
            new DanceData("Nightclub F-B","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_b_dance_idle"),

            new DanceData("Nighclub Beach (M) A-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m01") { IsOnActionWheel = true },
            new DanceData("Nighclub Beach (M) A-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m02"),
            new DanceData("Nighclub Beach (M) A-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m03"),
            new DanceData("Nighclub Beach (M) A-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m04"),
            new DanceData("Nighclub Beach (M) A-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m05"),
            new DanceData("Nighclub Beach (M) B-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m01") { IsOnActionWheel = true },
            new DanceData("Nighclub Beach (M) B-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m02"),
            new DanceData("Nighclub Beach (M) B-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m03"),
            new DanceData("Nighclub Beach (M) B-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m04"),
            new DanceData("Nighclub Beach (M) B-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m05"),

            new DanceData("Nighclub Beach (F) A-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f01") { IsOnActionWheel = true },
            new DanceData("Nighclub Beach (F) A-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f02"),
            new DanceData("Nighclub Beach (F) B-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f01"),
            new DanceData("Nighclub Beach (F) B-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f02") { IsOnActionWheel = true },
            new DanceData("Nighclub Beach (F) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f01"),
            new DanceData("Nighclub Beach (F) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f02"),
            new DanceData("Nighclub Beach (F) F-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_d_f01"),

            new DanceData("Nighclub Beach (M) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m01") { IsOnActionWheel = true },
            new DanceData("Nighclub Beach (M) C-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m02"),
            new DanceData("Nighclub Beach (M) C-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m03"),
            new DanceData("Nighclub Beach (M) C-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m04"),
            new DanceData("Nighclub Beach (M) C-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m05") { IsOnActionWheel = true },

            new DanceData("Nightclub Club (F) A-1","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f01") { IsOnActionWheel = true },
            new DanceData("Nightclub Club (F) A-2","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f02"),
            new DanceData("Nightclub Club (F) A-3","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f03"),
            new DanceData("Nightclub Club (F) B-1","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f01"),
            new DanceData("Nightclub Club (F) B-2","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f02") { IsOnActionWheel = true },

            new DanceData("Nightclub Club (M) A-1","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m01"),
            new DanceData("Nightclub Club (M) A-2","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m02") { IsOnActionWheel = true },
            new DanceData("Nightclub Club (M) A-3","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m03"),

            new DanceData("Mountain Dancer Base","special_ped@mountain_dancer@base","base") { IsOnActionWheel = true },
            new DanceData("Mountain Dancer Verse","special_ped@mountain_dancer@monologue_4@monologue_4a","mnt_dnc_verse") { IsOnActionWheel = true },
            new DanceData("Mountain Dancer Butt Wag","special_ped@mountain_dancer@monologue_3@monologue_3a","mnt_dnc_buttwag") { IsOnActionWheel = true },
            new DanceData("Mountain Dancer Angel","special_ped@mountain_dancer@monologue_2@monologue_2a","mnt_dnc_angel") { IsOnActionWheel = true },
            new DanceData("Mountain Dancer Heaven","special_ped@mountain_dancer@monologue_1@monologue_1a","mtn_dnc_if_you_want_to_get_to_heaven") { IsOnActionWheel = true },

            new DanceData("NME Away Dance B-26","anim@scripted@nightclub@dj@kmuzk@andme@","nme_away_dance_b_26") { IsOnActionWheel = true },
            new DanceData("NME Away Dance A-03v2","anim@scripted@nightclub@dj@kmuzk@andme@","nme_away_dance_a_03_v2"),
            new DanceData("NME Away Dance C-16","anim@scripted@nightclub@dj@kmuzk@andme@","nme_away_dance_c_16"),
            new DanceData("NME Away Dance D-30","anim@scripted@nightclub@dj@kmuzk@andme@","nme_away_dance_d_30"),

            new DanceData("Dance In Car (Driver)","anim@mp_player_intincardancestd@ds@","idle_a","enter","exit") { IsVehicle = true },
            new DanceData("Dance In Car (Passenger)","anim@mp_player_intincardancestd@ps@","idle_a","enter","exit") { IsVehicle = true },

            new DanceData("Hi Dance Crown (F) 09-1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v1_female^1") { IsOnActionWheel = true },
            new DanceData("Hi Dance Crown (F) 09-2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v1_female^2"),
            new DanceData("hi_dance_crowd_09_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v1_male^1") { IsOnActionWheel = true },
            new DanceData("hi_dance_crowd_09_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v2_female^1"),
            new DanceData("hi_dance_crowd_09_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v2_female^2"),
            new DanceData("hi_dance_crowd_09_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_09_v2_male^1"),
            new DanceData("hi_dance_crowd_11_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v1_female^2"),
            new DanceData("hi_dance_crowd_11_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v1_female^1"),
            new DanceData("hi_dance_crowd_11_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v1_male^1"),
            new DanceData("hi_dance_crowd_11_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v2_female^1"),
            new DanceData("hi_dance_crowd_11_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v2_female^2"),
            new DanceData("hi_dance_crowd_11_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_11_v2_male^1"),
            new DanceData("hi_dance_crowd_13_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v1_female^1"),
            new DanceData("hi_dance_crowd_13_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v1_female^2"),
            new DanceData("hi_dance_crowd_13_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v1_male^1"),
            new DanceData("hi_dance_crowd_13_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v2_female^1"),
            new DanceData("hi_dance_crowd_13_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v2_female^2"),
            new DanceData("hi_dance_crowd_13_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_13_v2_male^1"),
            new DanceData("hi_dance_crowd_15_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v1_female^1"),
            new DanceData("hi_dance_crowd_15_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v1_female^2"),
            new DanceData("hi_dance_crowd_15_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v1_male^1") { IsOnActionWheel = true },
            new DanceData("hi_dance_crowd_15_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v2_female^1") { IsOnActionWheel = true },
            new DanceData("hi_dance_crowd_15_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v2_female^2") { IsOnActionWheel = true },
            new DanceData("hi_dance_crowd_15_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_15_v2_male^1"),
            new DanceData("hi_dance_crowd_17_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v1_female^1"),
            new DanceData("hi_dance_crowd_17_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v1_female^2"),
            new DanceData("hi_dance_crowd_17_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v1_male^1"),
            new DanceData("hi_dance_crowd_17_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v2_female^1"),
            new DanceData("hi_dance_crowd_17_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v2_female^2"),
            new DanceData("hi_dance_crowd_17_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","hi_dance_crowd_17_v2_male^1"),
            new DanceData("li_dance_crowd_09_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v1_female^1"),
            new DanceData("li_dance_crowd_09_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v1_female^2"),
            new DanceData("li_dance_crowd_09_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v1_male^1"),
            new DanceData("li_dance_crowd_09_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v2_female^1"),
            new DanceData("li_dance_crowd_09_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v2_female^2"),
            new DanceData("li_dance_crowd_09_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_09_v2_male^1"),
            new DanceData("li_dance_crowd_11_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v1_female^1"),
            new DanceData("li_dance_crowd_11_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v1_female^2"),
            new DanceData("li_dance_crowd_11_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v1_male^1"),
            new DanceData("li_dance_crowd_11_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v2_female^1"),
            new DanceData("li_dance_crowd_11_v2_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v2_female^2"),
            new DanceData("li_dance_crowd_11_v2_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_11_v2_male^1"),
            new DanceData("li_dance_crowd_13_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_13_v1_female^1"),
            new DanceData("li_dance_crowd_13_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_13_v1_female^2"),
            new DanceData("li_dance_crowd_13_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_13_v1_male^1"),
            new DanceData("li_dance_crowd_13_v2_female^1","anim@amb@nightclub_island@dancers@crowddance_groups@groupe@","li_dance_crowd_13_v2_female^1"),

            new DanceData("hi_dance_facedj_09_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^1") { IsOnActionWheel = true },
            new DanceData("hi_dance_facedj_09_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^2"),
            new DanceData("hi_dance_facedj_09_v1_female^3","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^3"),
            new DanceData("hi_dance_facedj_09_v1_female^4","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^4") { IsOnActionWheel = true },
            new DanceData("hi_dance_facedj_09_v1_female^5","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^5"),
            new DanceData("hi_dance_facedj_09_v1_female^6","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_female^6"),
            new DanceData("hi_dance_facedj_09_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^1"),
            new DanceData("hi_dance_facedj_09_v1_male^2","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^2"),
            new DanceData("hi_dance_facedj_09_v1_male^3","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^3"),
            new DanceData("hi_dance_facedj_09_v1_male^4","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^4"),
            new DanceData("hi_dance_facedj_09_v1_male^5","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^5"),
            new DanceData("hi_dance_facedj_09_v1_male^6","anim@amb@nightclub_island@dancers@crowddance_facedj@hi_intensity","hi_dance_facedj_09_v1_male^6"),

            new DanceData("]mi_dance_facedj_11_v1_female^1","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^1") { IsOnActionWheel = true },
            new DanceData("]mi_dance_facedj_11_v1_female^2","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^2"),
            new DanceData("]mi_dance_facedj_11_v1_female^3","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^3"),
            new DanceData("]mi_dance_facedj_11_v1_female^4","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^4") { IsOnActionWheel = true },
            new DanceData("]mi_dance_facedj_11_v1_female^5","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^5"),
            new DanceData("]mi_dance_facedj_11_v1_female^6","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_female^6"),
            new DanceData("]mi_dance_facedj_11_v1_male^1","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^1"),
            new DanceData("]mi_dance_facedj_11_v1_male^2","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^2"),
            new DanceData("]mi_dance_facedj_11_v1_male^3","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^3"),
            new DanceData("]mi_dance_facedj_11_v1_male^4","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^4"),
            new DanceData("]mi_dance_facedj_11_v1_male^5","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^5"),
            new DanceData("]mi_dance_facedj_11_v1_male^6","anim@amb@nightclub_island@dancers@crowddance_facedj@med_intensity","]mi_dance_facedj_11_v1_male^6"),

            new DanceData("Dixon Entourage (M) 01","anim@amb@nightclub@dancers@dixon_entourage@","mi_dance_facedj_15_v1_male^4") { IsOnActionWheel = true },
            new DanceData("Podium Dancers (M) 01","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_male^5") { IsOnActionWheel = true },

            new DanceData("Solomun Entourage (F) 01","anim@amb@nightclub@dancers@solomun_entourage@","mi_dance_facedj_17_v1_female^1"),
            new DanceData("Tale of Us Entourage (F) 04","anim@amb@nightclub@dancers@tale_of_us_entourage@","mi_dance_crowd_13_v2_female^4"),
            new DanceData("Podium Dancers (F) 01","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_female^2"),

            new DanceData("Strip Club Private Dance (F)","mini@strip_club@private_dance@part1","priv_dance_p1"),
            new DanceData("Lap Dance 01","mp_safehouse","lap_dance_girl"),
            new DanceData("Private Dance 01","mini@strip_club@private_dance@idle","priv_dance_idle") { IsOnActionWheel = true },
            


            new DanceData("Cheer Female 1","amb@world_human_cheering@female_a","base") { IsCheer = true },
            new DanceData("Cheer Female 2","amb@world_human_cheering@female_b","base"){ IsCheer = true },
            new DanceData("Cheer Female 3","amb@world_human_cheering@female_c","base") { IsCheer = true },
            new DanceData("Cheer Female 4","amb@world_human_cheering@female_d","base") { IsCheer = true },
            new DanceData("Cheer Male 1","amb@world_human_cheering@male_a","base"){ IsCheer = true },
            new DanceData("Cheer Male 2","amb@world_human_cheering@male_b","base") { IsCheer = true },
            new DanceData("Cheer Male 3","amb@world_human_cheering@male_d","base") { IsCheer = true },
            new DanceData("Cheer Male 4","amb@world_human_cheering@male_e","base") { IsCheer = true },


            new DanceData("Cheer Podium 1","anim@arena@celeb@podium@no_prop@","clapping_a_1st") { IsCheer = true },
            new DanceData("Cheer Podium 2","anim@arena@celeb@podium@no_prop@","cheer_a_2nd") { IsCheer = true },
            new DanceData("Cheer Podium 3","anim@arena@celeb@podium@no_prop@","cheer_a_1nd") { IsCheer = true },

            new DanceData("Slow Clap 1","anim@mp_player_intcelebrationfemale@slow_clap","slow_clap") { IsCheer = true },
            new DanceData("Slow Clap 2","anim@mp_player_intcelebrationmale@slow_clap","slow_clap") { IsCheer = true },


            new DanceData("Angry Clap 1","anim@arena@celeb@flat@solo@no_props@","angry_clap_b_player_b") { IsCheer = true },
            new DanceData("Angry Clap 2","anim@arena@celeb@flat@solo@no_props@","angry_clap_a_player_a") { IsCheer = true },
            new DanceData("Angry Clap 3","anim@arena@celeb@flat@solo@no_props@","angry_clap_b_player_a") { IsCheer = true },
        };
        Serialization.SerializeParams(DanceLookups, ConfigFileName);
    }
    public DanceData GetRandomDance()
    {
        return DanceLookups.Where(x=> !x.IsCheer).PickRandom();
    }
    public DanceData GetRandomCheer()
    {
        return DanceLookups.Where(x => x.IsCheer).PickRandom();
    }
}
