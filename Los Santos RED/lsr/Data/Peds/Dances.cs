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
new DanceData("dance_m_default","missfbi3_sniping","dance_m_default"),
new DanceData("priv_dance_p1","mini@strip_club@private_dance@part1","priv_dance_p1"),
new DanceData("mi_dance_facedj_15_v1_male^4","anim@amb@nightclub@dancers@dixon_entourage@","mi_dance_facedj_15_v1_male^4"),
new DanceData("hi_dance_facedj_17_v2_female^2","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_female^2"),
new DanceData("hi_dance_facedj_17_v2_male^5","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_male^5"),
new DanceData("mi_dance_facedj_17_v1_female^1","anim@amb@nightclub@dancers@solomun_entourage@","mi_dance_facedj_17_v1_female^1"),
new DanceData("mi_dance_crowd_13_v2_female^4","anim@amb@nightclub@dancers@tale_of_us_entourage@","mi_dance_crowd_13_v2_female^4"),
new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_a_dance_idle"),
new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_b_dance_idle"),
new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_a_dance_idle"),
new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_b_dance_idle"),
new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_a_dance_idle"),
new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_b_dance_idle"),
new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_a_dance_idle"),
new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_b_dance_idle"),
new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_a_dance_idle"),
new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_b_dance_idle"),
new DanceData("hi_idle_a_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f02"),
new DanceData("hi_idle_a_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f01"),
new DanceData("hi_idle_a_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m01"),
new DanceData("hi_idle_a_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m02"),
new DanceData("hi_idle_a_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m03"),
new DanceData("hi_idle_a_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m04"),
new DanceData("hi_idle_a_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m05"),
new DanceData("hi_idle_b_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f01"),
new DanceData("hi_idle_b_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f02"),
new DanceData("hi_idle_b_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m01"),
new DanceData("hi_idle_b_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m02"),
new DanceData("hi_idle_b_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m03"),
new DanceData("hi_idle_b_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m04"),
new DanceData("hi_idle_b_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m05"),
new DanceData("hi_idle_c_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f01"),
new DanceData("hi_idle_c_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f02"),
new DanceData("hi_idle_c_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m01"),
new DanceData("hi_idle_c_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m02"),
new DanceData("hi_idle_c_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m03"),
new DanceData("hi_idle_c_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m04"),
new DanceData("hi_idle_c_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m05"),
new DanceData("hi_idle_d_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_d_f01"),
new DanceData("hi_idle_a_f01","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f01"),
new DanceData("hi_idle_a_f02","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f02"),
new DanceData("hi_idle_a_f03","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f03"),
new DanceData("hi_idle_a_m01","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m01"),
new DanceData("hi_idle_a_m02","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m02"),
new DanceData("hi_idle_a_m03","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m03"),
new DanceData("hi_idle_b_f01","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f01"),
new DanceData("hi_idle_b_f02","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f02"),

        };


        }

    //List<DanceData> IDances.DanceLookups => throw new System.NotImplementedException();

    public DanceData GetRandomDance()
        {
            return DanceLookups.PickRandom();
        }
    }
