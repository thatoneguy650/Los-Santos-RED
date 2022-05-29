using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RadioStations : IRadioStations
{
    public List<RadioStation> RadioStationList { get; set; }
    public RadioStations()
    {
        
    }
    public void ReadConfig()
    {
        DefaultConfig();
    }
    public void DefaultConfig()
    {
        RadioStationList = new List<RadioStation>()
        {
            new RadioStation("NONE","Random",0) {CanDanceTo  = false },
            new RadioStation("OFF","Off",0) {CanDanceTo  = false },
            new RadioStation("RADIO_01_CLASS_ROCK","Los Santos Rock Radio",0),
            new RadioStation("RADIO_02_POP","Non-Stop-Pop FM",0),
            new RadioStation("RADIO_03_HIPHOP_NEW","Radio Los Santos",0),
            new RadioStation("RADIO_04_PUNK","Channel X",0),
            new RadioStation("RADIO_05_TALK_01","West Coast Talk Radio",0) {CanDanceTo  = false },
            new RadioStation("RADIO_06_COUNTRY","Rebel Radio",0) {CanDanceTo  = false },
            new RadioStation("RADIO_07_DANCE_01","Soulwax FM",0),
            new RadioStation("RADIO_08_MEXICAN","East Los FM",0),
            new RadioStation("RADIO_09_HIPHOP_OLD","West Coast Classics",0),
            new RadioStation("RADIO_11_TALK_02","Blaine County Radio",0),
            new RadioStation("RADIO_12_REGGAE","Blue Ark",0),
            new RadioStation("RADIO_13_JAZZ","Worldwide FM",0),
            new RadioStation("RADIO_14_DANCE_02","FlyLo FM",0),
            new RadioStation("RADIO_15_MOTOWN","The Lowdown 91.1",0),
            new RadioStation("RADIO_16_SILVERLAKE","Radio Mirror Park",0),
            new RadioStation("RADIO_17_FUNK","Space 103.2",0),
            new RadioStation("RADIO_18_90S_ROCK","Vinewood Boulevard Radio",0),
            new RadioStation("RADIO_19_USER","Self Radio",0),
            new RadioStation("RADIO_20_THELAB","The Lab",0),
            new RadioStation("RADIO_21_DLC_XM17","Blonded Los Santos 97.8 FM",0),
            new RadioStation("RADIO_22_DLC_BATTLE_MIX1_RADIO","Los Santos Underground Radio",0),
            new RadioStation("RADIO_23_DLC_XM19_RADIO","iFruit Radio",0),
            //new RadioStation("RADIO_27_DLC_PRHEI4","RADIO_27_DLC_PRHEI4",0),
            //new RadioStation("RADIO_34_DLC_HEI4_KULT","RADIO_34_DLC_HEI4_KULT",0),
            //new RadioStation("RADIO_35_DLC_HEI4_MLR","RADIO_35_DLC_HEI4_MLR",0),
        };
    }
    public RadioStation GetDanceStation()
    {
        return RadioStationList.Where(x => x.CanDanceTo).PickRandom();
    }

}

