using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Seats : ISeats
{
    private List<uint> NonSeatModels;

    public Seats()
    {
    }

    public List<SeatModel> SeatModels { get; private set; }
    public List<TableModel> TableModels { get; private set; }
    public void ReadConfig()
    {

        NonSeatModels = new List<uint>() { 0x272a1260 };

        SeatModels = new List<SeatModel>() {
                new SeatModel(0x6ba514ac,-0.45f) {Name = "Iron Bench" }, //sometime float a bit above it
                new SeatModel(0x7facd66f,-0.2f) {Name = "Bus Bench" },//new SeatModel(0x7facd66f,-0.15f) {Name = "Bus Bench" },
                new SeatModel(0xc0a6cbcd),
                //new SeatModel(0x534bc1bc), actyually garbage
                new SeatModel(0xa55359b8),
                new SeatModel(0xe7ed1a59),

                new SeatModel(0xd3c6d323){Name = "Plastic Chair" },

                new SeatModel("PROP_BENCH_05") {Name ="Bus Bench 05"},
                new SeatModel(0x96ff362a){Name = "Wooden Chair With Table" },



                new SeatModel(0x708bb82d, -0.45f) { Name = "Green Bus Bench" },





                new SeatModel(0xd3c6d323){Name = "Plastic Chair" },
                //new SeatModel(0x3c67ba3f,-0.45f){Name = "Iron Bench" },//actually a bag of chips?



                new SeatModel(0xda867f80,-0.5f){Name = "Iron Bench" },





                new SeatModel(0x643d1f90,-0.25f) {Name = "Maze Bus Bench" },



                new SeatModel("prop_bench_01b"),
                new SeatModel("prop_bench_01c"),
                new SeatModel("prop_bench_02"),
                new SeatModel("prop_bench_03"),
                new SeatModel("prop_bench_04"),
                new SeatModel("prop_bench_05"),
                new SeatModel("prop_bench_06"),
                new SeatModel("prop_bench_05"),
                new SeatModel("prop_bench_08"),
                new SeatModel("prop_bench_09"),
                new SeatModel("prop_bench_10"),
                new SeatModel("prop_bench_11"),
                new SeatModel("prop_fib_3b_bench"),
                new SeatModel("prop_ld_bench01"),
                new SeatModel("prop_wait_bench_01"),
                new SeatModel("hei_prop_heist_off_chair"),
                new SeatModel("hei_prop_hei_skid_chair"),
                new SeatModel("prop_chair_01a"),
                new SeatModel("prop_chair_01b"),
                new SeatModel("prop_chair_02"),
                new SeatModel("prop_chair_03"),
                new SeatModel("prop_chair_04a"),
                new SeatModel("prop_chair_04b"),
                new SeatModel("prop_chair_05"),
                new SeatModel("prop_chair_06"),
                new SeatModel("prop_chair_05"),
                new SeatModel("prop_chair_08"),
                new SeatModel("prop_chair_09"),
                new SeatModel("prop_chair_10"),
                new SeatModel("prop_chateau_chair_01"),
                new SeatModel("prop_clown_chair"),
                new SeatModel("prop_cs_office_chair"),
                new SeatModel("prop_direct_chair_01"),
                new SeatModel("prop_direct_chair_02"),
                new SeatModel("prop_gc_chair02"),
                new SeatModel("prop_off_chair_01"),
                new SeatModel("prop_off_chair_03"),
                new SeatModel("prop_off_chair_04"),
                new SeatModel("prop_off_chair_04b"),
                new SeatModel("prop_off_chair_04_s"),
                new SeatModel("prop_off_chair_05"),
                new SeatModel("prop_old_deck_chair"),
                new SeatModel("prop_old_wood_chair"),
                new SeatModel("prop_rock_chair_01"),
                new SeatModel("prop_skid_chair_01"),
                new SeatModel("prop_skid_chair_02"),
                new SeatModel("prop_skid_chair_03"),
                new SeatModel("prop_sol_chair"),
                new SeatModel("prop_wheelchair_01"),
                new SeatModel("prop_wheelchair_01_s"),
                new SeatModel("p_armchair_01_s"),
                new SeatModel("p_clb_officechair_s"),
                new SeatModel("p_dinechair_01_s"),
                new SeatModel("p_ilev_p_easychair_s"),
                new SeatModel("p_soloffchair_s"),
                new SeatModel("p_yacht_chair_01_s"),
                new SeatModel("v_club_officechair"),
                new SeatModel("v_corp_bk_chair3"),
                new SeatModel("v_corp_cd_chair"),
                new SeatModel("v_corp_offchair"),
                new SeatModel("v_ilev_chair02_ped"),
                new SeatModel("v_ilev_hd_chair"),
                new SeatModel("v_ilev_p_easychair"),
                new SeatModel("v_ret_gc_chair03"),
                new SeatModel("prop_ld_farm_chair01"),
                new SeatModel("prop_table_04_chr"),
                new SeatModel("prop_table_05_chr"),
                new SeatModel("prop_table_06_chr"),
                new SeatModel("v_ilev_leath_chr"),
                new SeatModel("prop_table_01_chr_a"),
                new SeatModel("prop_table_01_chr_b"),
                new SeatModel("prop_table_02_chr"),
                new SeatModel("prop_table_03b_chr"),
                new SeatModel("prop_table_03_chr"),
                new SeatModel("prop_torture_ch_01"),
                new SeatModel("v_ilev_fh_dineeamesa"),
                new SeatModel("v_ilev_fh_kitchenstool"),
                new SeatModel("v_ilev_tort_stool"),
                new SeatModel("v_ilev_fh_kitchenstool"),
                new SeatModel("v_ilev_fh_kitchenstool"),
                new SeatModel("v_ilev_fh_kitchenstool"),
                new SeatModel("v_ilev_fh_kitchenstool"),
                new SeatModel("hei_prop_yah_seat_01"),
                new SeatModel("hei_prop_yah_seat_02"),
                new SeatModel("hei_prop_yah_seat_03"),
                new SeatModel("prop_waiting_seat_01"),
                new SeatModel("prop_yacht_seat_01"),
                new SeatModel("prop_yacht_seat_02"),
                new SeatModel("prop_yacht_seat_03"),
                new SeatModel("prop_hobo_seat_01"),
                new SeatModel("prop_rub_couch01"),
                new SeatModel("miss_rub_couch_01"),
                new SeatModel("prop_ld_farm_couch01"),
                new SeatModel("prop_ld_farm_couch02"),
                new SeatModel("prop_rub_couch02"),
                new SeatModel("prop_rub_couch03"),
                new SeatModel("prop_rub_couch04"),
                new SeatModel("p_lev_sofa_s"),
                new SeatModel("p_res_sofa_l_s"),
                new SeatModel("p_v_med_p_sofa_s"),
                new SeatModel("p_yacht_sofa_01_s"),
                new SeatModel("v_ilev_m_sofa"),
                new SeatModel("v_res_tre_sofa_s"),
                new SeatModel("v_tre_sofa_mess_a_s"),
                new SeatModel("v_tre_sofa_mess_b_s"),
                new SeatModel("v_tre_sofa_mess_c_s"),
                new SeatModel("prop_roller_car_01"),
                new SeatModel("prop_roller_car_02"),









            };



        TableModels = new List<TableModel>() {
                new TableModel(0xf3a90766),
                                             };
    }
    public bool CanSit(Rage.Object obj)
    {
        if (obj.Exists())
        {
            SeatModel seatModel = GetSeatModel(obj);
            string modelName = obj.Model.Name.ToLower();
            if (seatModel != null || NativeHelper.IsSittableModel(modelName))
            {
                return true;
            }
        }
        return false;
    }
    public float GetOffSet(Rage.Object obj)
    {
        SeatModel seatModel = GetSeatModel(obj);
        if(seatModel != null)
        {
            return seatModel.EntryOffsetFront;
        }
        else
        {
            return -0.5f;
        }
    }
    public SeatModel GetSeatModel(Rage.Object obj)
    {
        SeatModel seatModel = null;
        if (obj.Exists())
        {
            string modelName = obj.Model.Name.ToLower();
            uint hash = obj.Model.Hash;
            seatModel = SeatModels.FirstOrDefault(x => x.ModelHash == hash);
            if (seatModel == null)
            {
                seatModel = SeatModels.FirstOrDefault(x => x.ModelName?.ToLower() == modelName);
            }
            if (seatModel == null)
            {
                seatModel = SeatModels.FirstOrDefault(x => Game.GetHashKey(x.ModelName) == hash);
            }
        }
        return seatModel;
    }
}

