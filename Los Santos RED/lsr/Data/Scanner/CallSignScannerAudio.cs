using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class CallsignScannerAudio
{
    private List<DivisionLookup> DivisionsList = new List<DivisionLookup>();
    private List<UnitTypeLookup> UnitTypeList = new List<UnitTypeLookup>();
    private List<BeatNumberLookup> BeatNumberList = new List<BeatNumberLookup>();

    public void ReadConfig()
    {
        DefaultConfig();
    }
    public List<string> GetAudio(int division, string unitType, int beatNumber)
    {
        List<string> toReturn = new List<string>();
        DivisionLookup divisionFound = DivisionsList.Where(x => x.Division == division).PickRandom();
        if (divisionFound != null)
        {
            toReturn.Add(divisionFound.DispatchFile);
            UnitTypeLookup unitTypeFound = UnitTypeList.Where(x => x.UnitType == unitType).PickRandom();
            if (unitTypeFound != null)
            {
                toReturn.Add(unitTypeFound.DispatchFile);
                BeatNumberLookup beatNumberFound = BeatNumberList.Where(x => x.BeatNumber == beatNumber).PickRandom();
                if (beatNumberFound != null)
                {
                    toReturn.Add(beatNumberFound.DispatchFile);
                    return toReturn;
                }
            }
        }
        return null;
    }
    private void DefaultConfig()
    {
        DivisionsList = new List<DivisionLookup>
        {
            new DivisionLookup(1, car_code_beat.one.FileName),
            new DivisionLookup(2, car_code_beat.two.FileName),
            new DivisionLookup(3, car_code_beat.three.FileName),
            new DivisionLookup(4, car_code_beat.four.FileName),
            new DivisionLookup(5, car_code_beat.five.FileName),
            new DivisionLookup(6, car_code_beat.six.FileName),
            new DivisionLookup(7, car_code_beat.seven.FileName),
            new DivisionLookup(8, car_code_beat.eight.FileName),
            new DivisionLookup(9, car_code_beat.nine.FileName),
            new DivisionLookup(10, car_code_beat.ten.FileName),
            new DivisionLookup(1, car_code_division._1.FileName),
            new DivisionLookup(2, car_code_division._2.FileName),
            new DivisionLookup(3, car_code_division._3.FileName),
            new DivisionLookup(4, car_code_division._4.FileName),
            new DivisionLookup(5, car_code_division._5.FileName),
            new DivisionLookup(6, car_code_division._6.FileName),
            new DivisionLookup(7, car_code_division._7.FileName),
            new DivisionLookup(8, car_code_division._8.FileName),
            new DivisionLookup(9, car_code_division._9.FileName),
            new DivisionLookup(10, car_code_division._10.FileName),
            new DivisionLookup(11, car_code_beat.eleven.FileName),
            new DivisionLookup(12, car_code_beat.twelve.FileName),
            new DivisionLookup(13, car_code_beat.thirteen.FileName),
            new DivisionLookup(14, car_code_beat.fourteen.FileName),
            new DivisionLookup(15, car_code_beat.fifteen.FileName),
            new DivisionLookup(16, car_code_beat.sixteen.FileName),
            new DivisionLookup(17, car_code_beat.seventeen.FileName),
            new DivisionLookup(18, car_code_beat.eighteen.FileName),
            new DivisionLookup(19, car_code_beat.nineteen.FileName),
            new DivisionLookup(20, car_code_beat.twenty.FileName),
            new DivisionLookup(21, car_code_beat.twentyone.FileName),
            new DivisionLookup(22, car_code_beat.twentytwo.FileName),
            new DivisionLookup(23, car_code_beat.twentythree.FileName),
            new DivisionLookup(24, car_code_beat.twentyfour.FileName),
        };

        UnitTypeList = new List<UnitTypeLookup>
        {
            new UnitTypeLookup("Adam", car_code_unit_type.Adam.FileName),
            new UnitTypeLookup("Adam", lp_letters_high.Adam.FileName),
            new UnitTypeLookup("Adam", lp_letters_high.Adam1.FileName),
            new UnitTypeLookup("Boy", car_code_unit_type.Boy.FileName),
            new UnitTypeLookup("Boy", lp_letters_high.Boy.FileName),
            new UnitTypeLookup("Boy", lp_letters_high.Boy1.FileName),
            new UnitTypeLookup("Charles", car_code_unit_type.Charles.FileName),
            new UnitTypeLookup("Charles", lp_letters_high.Charles.FileName),
            new UnitTypeLookup("Charles", lp_letters_high.Charles1.FileName),
            new UnitTypeLookup("David", car_code_unit_type.David.FileName),
            new UnitTypeLookup("David", lp_letters_high.David.FileName),
            new UnitTypeLookup("David", lp_letters_high.David.FileName),
            new UnitTypeLookup("Edward", car_code_unit_type.Edward.FileName),
            new UnitTypeLookup("Edward", lp_letters_high.Edward.FileName),
            new UnitTypeLookup("Edward", lp_letters_high.Edward1.FileName),
            new UnitTypeLookup("Frank", car_code_unit_type.Frank.FileName),
            new UnitTypeLookup("Frank", lp_letters_high.Frank.FileName),
            new UnitTypeLookup("Frank", lp_letters_high.Frank1.FileName),
            new UnitTypeLookup("George", car_code_unit_type.George.FileName),
            new UnitTypeLookup("George", lp_letters_high.George.FileName),
            new UnitTypeLookup("George", lp_letters_high.George1.FileName),
            new UnitTypeLookup("Henry", car_code_unit_type.Henry.FileName),
            new UnitTypeLookup("Henry", lp_letters_high.Henry.FileName),
            new UnitTypeLookup("Henry", lp_letters_high.Henry1.FileName),
            new UnitTypeLookup("Ida", car_code_unit_type.Ida.FileName),
            new UnitTypeLookup("Ida", lp_letters_high.Ita.FileName),
            new UnitTypeLookup("Ida", lp_letters_high.Ita1.FileName),
            new UnitTypeLookup("John", car_code_unit_type.John.FileName),
            new UnitTypeLookup("John", lp_letters_high.John.FileName),
            new UnitTypeLookup("John", lp_letters_high.John1.FileName),
            new UnitTypeLookup("King", car_code_unit_type.King.FileName),
            new UnitTypeLookup("King", lp_letters_high.King.FileName),
            new UnitTypeLookup("King", lp_letters_high.King1.FileName),
            new UnitTypeLookup("Lincoln", car_code_unit_type.Lincoln.FileName),
            new UnitTypeLookup("Lincoln", lp_letters_high.Lincoln.FileName),
            new UnitTypeLookup("Lincoln", lp_letters_high.Lincoln1.FileName),
            new UnitTypeLookup("Mary", car_code_unit_type.Mary.FileName),
            new UnitTypeLookup("Mary", lp_letters_high.Mary.FileName),
            new UnitTypeLookup("Mary", lp_letters_high.Mary1.FileName),
            new UnitTypeLookup("Nora", car_code_unit_type.Nora.FileName),
            new UnitTypeLookup("Nora", lp_letters_high.Nora.FileName),
            new UnitTypeLookup("Nora", lp_letters_high.Nora1.FileName),
            new UnitTypeLookup("Ocean", car_code_unit_type.Ocean.FileName),
            new UnitTypeLookup("Ocean", lp_letters_high.Ocean.FileName),
            new UnitTypeLookup("Ocean", lp_letters_high.Ocean1.FileName),
            new UnitTypeLookup("Paul", car_code_unit_type.Paul.FileName),
            new UnitTypeLookup("Paul", lp_letters_high.Paul.FileName),
            new UnitTypeLookup("Paul", lp_letters_high.Paul1.FileName),
            new UnitTypeLookup("Queen", car_code_unit_type.Queen.FileName),
            new UnitTypeLookup("Queen", lp_letters_high.Queen.FileName),
            new UnitTypeLookup("Queen", lp_letters_high.Queen1.FileName),
            new UnitTypeLookup("Robert", car_code_unit_type.Robert.FileName),
            new UnitTypeLookup("Robert", lp_letters_high.Robert.FileName),
            new UnitTypeLookup("Robert", lp_letters_high.Robert1.FileName),
            new UnitTypeLookup("Sam", car_code_unit_type.Sam.FileName),
            new UnitTypeLookup("Sam", lp_letters_high.Sam.FileName),
            new UnitTypeLookup("Sam", lp_letters_high.Sam1.FileName),
            new UnitTypeLookup("Tom", car_code_unit_type.Tom.FileName),
            new UnitTypeLookup("Tom", lp_letters_high.Tom.FileName),
            new UnitTypeLookup("Tom", lp_letters_high.Tom1.FileName),
            new UnitTypeLookup("Union", car_code_unit_type.Union.FileName),
            new UnitTypeLookup("Union", lp_letters_high.Union.FileName),
            new UnitTypeLookup("Union", lp_letters_high.Union1.FileName),
            new UnitTypeLookup("Victor", car_code_unit_type.Victor.FileName),
            new UnitTypeLookup("Victor", lp_letters_high.Victor.FileName),
            new UnitTypeLookup("Victor", lp_letters_high.Victor1.FileName),
            new UnitTypeLookup("William", car_code_unit_type.William.FileName),
            new UnitTypeLookup("William", lp_letters_high.William.FileName),
            new UnitTypeLookup("William", lp_letters_high.William1.FileName),
            new UnitTypeLookup("XRay", car_code_unit_type.XRay.FileName),
            new UnitTypeLookup("XRay", lp_letters_high.XRay.FileName),
            new UnitTypeLookup("XRay", lp_letters_high.XRay1.FileName),
            new UnitTypeLookup("Young", car_code_unit_type.Young.FileName),
            new UnitTypeLookup("Young", lp_letters_high.Young.FileName),
            new UnitTypeLookup("Young", lp_letters_high.Young1.FileName),
            new UnitTypeLookup("Zebra", car_code_unit_type.Zebra.FileName),
            new UnitTypeLookup("Zebra", lp_letters_high.Zebra.FileName),
            new UnitTypeLookup("Zebra", lp_letters_high.Zebra1.FileName),
        };
        BeatNumberList = new List<BeatNumberLookup>
        {

            new BeatNumberLookup(1, car_code_division._1.FileName),
            new BeatNumberLookup(2, car_code_division._2.FileName),
            new BeatNumberLookup(3, car_code_division._3.FileName),
            new BeatNumberLookup(4, car_code_division._4.FileName),
            new BeatNumberLookup(5, car_code_division._5.FileName),
            new BeatNumberLookup(6, car_code_division._6.FileName),
            new BeatNumberLookup(7, car_code_division._7.FileName),
            new BeatNumberLookup(8, car_code_division._8.FileName),
            new BeatNumberLookup(9, car_code_division._9.FileName),
            new BeatNumberLookup(10, car_code_division._10.FileName),


            new BeatNumberLookup(1, car_code_beat.one.FileName),
            new BeatNumberLookup(2, car_code_beat.two.FileName),
            new BeatNumberLookup(3, car_code_beat.three.FileName),
            new BeatNumberLookup(4, car_code_beat.four.FileName),
            new BeatNumberLookup(5, car_code_beat.five.FileName),
            new BeatNumberLookup(6, car_code_beat.six.FileName),
            new BeatNumberLookup(7, car_code_beat.seven.FileName),
            new BeatNumberLookup(8, car_code_beat.eight.FileName),
            new BeatNumberLookup(9, car_code_beat.nine.FileName),
            new BeatNumberLookup(10, car_code_beat.ten.FileName),
            new BeatNumberLookup(11, car_code_beat.eleven.FileName),
            new BeatNumberLookup(12, car_code_beat.twelve.FileName),
            new BeatNumberLookup(13, car_code_beat.thirteen.FileName),
            new BeatNumberLookup(14, car_code_beat.fourteen.FileName),
            new BeatNumberLookup(15, car_code_beat.fifteen.FileName),
            new BeatNumberLookup(16, car_code_beat.sixteen.FileName),
            new BeatNumberLookup(17, car_code_beat.seventeen.FileName),
            new BeatNumberLookup(18, car_code_beat.eighteen.FileName),
            new BeatNumberLookup(19, car_code_beat.nineteen.FileName),
            new BeatNumberLookup(20, car_code_beat.twenty.FileName),
            new BeatNumberLookup(21, car_code_beat.twentyone.FileName),
            new BeatNumberLookup(22, car_code_beat.twentytwo.FileName),
            new BeatNumberLookup(23, car_code_beat.twentythree.FileName),
            new BeatNumberLookup(24, car_code_beat.twentyfour.FileName),
        };

    }
    private class DivisionLookup
    {
        public int Division { get; set; } = -1;
        public string DispatchFile { get; set; } = "";
        public DivisionLookup()
        {

        }
        public DivisionLookup(int division, string _DispatchFile)
        {
            Division = division;
            DispatchFile = _DispatchFile;
        }
    }
    private class UnitTypeLookup
    {
        public string UnitType { get; set; } = "";
        public string DispatchFile { get; set; } = "";
        public UnitTypeLookup()
        {

        }
        public UnitTypeLookup(string _Name, string _DispatchFile)
        {
            UnitType = _Name;
            DispatchFile = _DispatchFile;
        }
    }
    private class BeatNumberLookup
    {
        public int BeatNumber { get; set; } = -1;
        public string DispatchFile { get; set; } = "";
        public BeatNumberLookup()
        {

        }
        public BeatNumberLookup(int beatNumber, string _DispatchFile)
        {
            BeatNumber = beatNumber;
            DispatchFile = _DispatchFile;
        }
    }
}

