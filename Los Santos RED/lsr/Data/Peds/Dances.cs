using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.Linq;

    public class Dances : IDances
    {

        public List<DanceData> DanceLookups { get; set; } = new List<DanceData>();
        public Dances()
        {
        DanceLookups = new List<DanceData>()
        {
            new DanceData("Default","missfbi3_sniping","dance_m_default"),



            new DanceData("Strip Club Private Dance (F)","mini@strip_club@private_dance@part1","priv_dance_p1"),



            new DanceData("Dixon Entourage (M) 01","anim@amb@nightclub@dancers@dixon_entourage@","mi_dance_facedj_15_v1_male^4"),
            new DanceData("Podium Dancers (M) 01","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_male^5"),




            new DanceData("Solomun Entourage (F) 01","anim@amb@nightclub@dancers@solomun_entourage@","mi_dance_facedj_17_v1_female^1"),
            new DanceData("Tale of Us Entourage (F) 04","anim@amb@nightclub@dancers@tale_of_us_entourage@","mi_dance_crowd_13_v2_female^4"),
            new DanceData("Podium Dancers (F) 01","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_female^2"),



            new DanceData("Nightclub A-A","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },
            new DanceData("Nightclub A-B","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_b_dance_idle","ped_b_dance_intro","ped_b_dance_exit") { FacialAnimationEnter = "ped_b_dance_intro_facial", FacialAnimationIdle = "ped_b_dance_idle_facial", FacialAnimationExit = "ped_b_dance_exit_facial" },
            new DanceData("Nightclub B-A","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },
            new DanceData("Nightclub B-B","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_b_dance_idle","ped_b_dance_intro","ped_b_dance_exit"){ FacialAnimationEnter = "ped_b_dance_intro_facial", FacialAnimationIdle = "ped_b_dance_idle_facial", FacialAnimationExit = "ped_b_dance_exit_facial" },
            new DanceData("Nightclub D-A","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_a_dance_idle","ped_a_dance_intro","ped_a_dance_exit") { FacialAnimationEnter = "ped_a_dance_intro_facial", FacialAnimationIdle = "ped_a_dance_idle_facial", FacialAnimationExit = "ped_a_dance_exit_facial" },



            new DanceData("Nightclub D-B","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_b_dance_idle"),
            new DanceData("Nightclub E-A","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_a_dance_idle"),
            new DanceData("Nightclub E-B","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_b_dance_idle"),
            new DanceData("Nightclub F-A","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_a_dance_idle"),
            new DanceData("Nightclub F-B","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_b_dance_idle"),



            

            new DanceData("Nighclub Beach (M) A-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m01"),
            new DanceData("Nighclub Beach (M) A-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m02"),
            new DanceData("Nighclub Beach (M) A-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m03"),
            new DanceData("Nighclub Beach (M) A-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m04"),
            new DanceData("Nighclub Beach (M) A-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m05"),
            new DanceData("Nighclub Beach (M) B-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m01"),
            new DanceData("Nighclub Beach (M) B-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m02"),
            new DanceData("Nighclub Beach (M) B-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m03"),
            new DanceData("Nighclub Beach (M) B-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m04"),
            new DanceData("Nighclub Beach (M) B-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m05"),

            new DanceData("Nighclub Beach (F) A-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f01"),
            new DanceData("Nighclub Beach (F) A-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f02"),
            new DanceData("Nighclub Beach (F) B-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f01"),
            new DanceData("Nighclub Beach (F) B-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f02"),
            new DanceData("Nighclub Beach (F) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f01"),
            new DanceData("Nighclub Beach (F) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f02"),
            new DanceData("Nighclub Beach (F) F-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_d_f01"),






            new DanceData("Nighclub Beach (M) C-1","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m01"),
            new DanceData("Nighclub Beach (M) C-2","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m02"),
            new DanceData("Nighclub Beach (M) C-3","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m03"),
            new DanceData("Nighclub Beach (M) C-4","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m04"),
            new DanceData("Nighclub Beach (M) C-5","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m05"),

            


            new DanceData("Nightclub Club (F) A-1","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f01"),
            new DanceData("Nightclub Club (F) A-2","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f02"),
            new DanceData("Nightclub Club (F) A-3","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f03"),
            new DanceData("Nightclub Club (F) B-1","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f01"),
            new DanceData("Nightclub Club (F) B-2","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f02"),


            new DanceData("Nightclub Club (M) A-1","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m01"),
            new DanceData("Nightclub Club (M) A-2","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m02"),
            new DanceData("Nightclub Club (M) A-3","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m03"),


        };


        }

    //List<DanceData> IDances.DanceLookups => throw new System.NotImplementedException();

        public DanceData GetRandomDance()
        {
            return DanceLookups.PickRandom();
        }
    }
