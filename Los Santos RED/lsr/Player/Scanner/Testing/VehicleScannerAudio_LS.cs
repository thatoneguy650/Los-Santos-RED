using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class VehicleScannerAudio_LS : IVehicleScannerAudio
{
    private List<LetterLookup> LettersAndNumbersLookup = new List<LetterLookup>();
    private List<ColorLookup> ColorLookups = new List<ColorLookup>();
    private List<ColorIDLookup> ColorIDLookups = new List<ColorIDLookup>();
    private List<VehicleModelLookup> VehicleModelLookups = new List<VehicleModelLookup>();
    private List<VehicleClassLookup> VehicleClassLookups = new List<VehicleClassLookup>();
    private List<VehicleMakeLookup> VehicleMakeLookups = new List<VehicleMakeLookup>();
    public void ReadConfig()
    {
        DefaultConfig();
    }
    public string GetColorAudio(Color ToLookup)
    {
        ColorLookup VehicleColor = ColorLookups.FirstOrDefault(x => x.BaseColor == ToLookup);
        if (VehicleColor == null)
            return "";
        else
            return VehicleColor.ScannerFile;
    }

    public string GetColorAudioByID(int colorID)
    {
        ColorIDLookup VehicleColor = ColorIDLookups.Where(x => x.PrimaryColor == colorID).PickRandom();
        if (VehicleColor == null)
            return "";
        else
            return VehicleColor.ScannerFile;
    }
    public string GetMakeAudio(string MakeName)
    {
        VehicleMakeLookup VehicleMake = VehicleMakeLookups.FirstOrDefault(x => x.MakeName == MakeName);
        if (VehicleMake == null)
            return "";
        else
            return VehicleMake.ScannerFile;
    }
    public string GetModelAudio(uint VehicleHash)
    {
        VehicleModelLookup VehicleModel = VehicleModelLookups.FirstOrDefault(x => x.Hash == VehicleHash);
        if (VehicleModel == null)
            return "";
        else
            return VehicleModel.ScannerFile;
    }
    public string GetClassAudio(int GameClass)
    {
        VehicleClassLookup VehicleClass = VehicleClassLookups.FirstOrDefault(x => x.GameClass == GameClass);
        if (VehicleClass == null)
            return "";
        else
            return VehicleClass.ScannerFile;
    }
    public List<string> GetPlateAudio(string LicensePlate)
    {
        List<string> AudioFiles = new List<string>();
        foreach (char c in LicensePlate)
        {
            List<LetterLookup> Possible = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).ToList();
            if (Possible.Any())
            {
                string DispatchFileName = Possible.PickRandom().ScannerFile;
                AudioFiles.Add(DispatchFileName);
            }
        }
        return AudioFiles;
    }
    public string ClassName(int GameClass)
    {
        VehicleClassLookup VehicleClass = VehicleClassLookups.FirstOrDefault(x => x.GameClass == GameClass);
        if (VehicleClass == null)
            return "";
        else
            return VehicleClass.Name;
    }
    private void DefaultConfig()
    {
        LettersAndNumbersLookup = new List<LetterLookup>()
        {
            new LetterLookup('A', lp_letters_high.Adam.FileName),
            new LetterLookup('B', lp_letters_high.Boy.FileName),
            new LetterLookup('C', lp_letters_high.Charles.FileName),
            new LetterLookup('D', lp_letters_high.David.FileName),
            new LetterLookup('E', lp_letters_high.Edward.FileName),
            new LetterLookup('F', lp_letters_high.Frank.FileName),
            new LetterLookup('G', lp_letters_high.George.FileName),
            new LetterLookup('H', lp_letters_high.Henry.FileName),
            new LetterLookup('I', lp_letters_high.Ita.FileName),
            new LetterLookup('J', lp_letters_high.John.FileName),
            new LetterLookup('K', lp_letters_high.King.FileName),
            new LetterLookup('L', lp_letters_high.Lincoln.FileName),
            new LetterLookup('M', lp_letters_high.Mary.FileName),
            new LetterLookup('N', lp_letters_high.Nora.FileName),
            new LetterLookup('O', lp_letters_high.Ocean.FileName),
            new LetterLookup('P', lp_letters_high.Paul.FileName),
            new LetterLookup('Q', lp_letters_high.Queen.FileName),
            new LetterLookup('R', lp_letters_high.Robert.FileName),
            new LetterLookup('S', lp_letters_high.Sam.FileName),
            new LetterLookup('T', lp_letters_high.Tom.FileName),
            new LetterLookup('U', lp_letters_high.Union.FileName),
            new LetterLookup('V', lp_letters_high.Victor.FileName),
            new LetterLookup('W', lp_letters_high.William.FileName),
            new LetterLookup('X', lp_letters_high.XRay.FileName),
            new LetterLookup('Y', lp_letters_high.Young.FileName),
            new LetterLookup('Z', lp_letters_high.Zebra.FileName),
            new LetterLookup('A', lp_letters_high.Adam1.FileName),
            new LetterLookup('B', lp_letters_high.Boy1.FileName),
            new LetterLookup('C', lp_letters_high.Charles1.FileName),
            new LetterLookup('E', lp_letters_high.Edward1.FileName),
            new LetterLookup('F', lp_letters_high.Frank1.FileName),
            new LetterLookup('G', lp_letters_high.George1.FileName),
            new LetterLookup('H', lp_letters_high.Henry1.FileName),
            new LetterLookup('I', lp_letters_high.Ita1.FileName),
            new LetterLookup('J', lp_letters_high.John1.FileName),
            new LetterLookup('K', lp_letters_high.King1.FileName),
            new LetterLookup('L', lp_letters_high.Lincoln1.FileName),
            new LetterLookup('M', lp_letters_high.Mary1.FileName),
            new LetterLookup('N', lp_letters_high.Nora1.FileName),
            new LetterLookup('O', lp_letters_high.Ocean1.FileName),
            new LetterLookup('P', lp_letters_high.Paul1.FileName),
            new LetterLookup('Q', lp_letters_high.Queen1.FileName),
            new LetterLookup('R', lp_letters_high.Robert1.FileName),
            new LetterLookup('S', lp_letters_high.Sam1.FileName),
            new LetterLookup('T', lp_letters_high.Tom1.FileName),
            new LetterLookup('U', lp_letters_high.Union1.FileName),
            new LetterLookup('V', lp_letters_high.Victor1.FileName),
            new LetterLookup('W', lp_letters_high.William1.FileName),
            new LetterLookup('X', lp_letters_high.XRay1.FileName),
            new LetterLookup('Y', lp_letters_high.Young1.FileName),
            new LetterLookup('Z', lp_letters_high.Zebra1.FileName),
            new LetterLookup('1', lp_numbers.One.FileName),
            new LetterLookup('2', lp_numbers.Two.FileName),
            new LetterLookup('3', lp_numbers.Three.FileName),
            new LetterLookup('4', lp_numbers.Four.FileName),
            new LetterLookup('5', lp_numbers.Five.FileName),
            new LetterLookup('6', lp_numbers.Six.FileName),
            new LetterLookup('7', lp_numbers.Seven.FileName),
            new LetterLookup('8', lp_numbers.Eight.FileName),
            new LetterLookup('9', lp_numbers.Nine.FileName),
            new LetterLookup('0', lp_numbers.Zero.FileName),
            new LetterLookup('1', lp_numbers.One1.FileName),
            new LetterLookup('2', lp_numbers.Two1.FileName),
            new LetterLookup('3', lp_numbers.Three1.FileName),
            new LetterLookup('4', lp_numbers.Four1.FileName),
            new LetterLookup('5', lp_numbers.Five1.FileName),
            new LetterLookup('6', lp_numbers.Six1.FileName),
            new LetterLookup('7', lp_numbers.Seven1.FileName),
            new LetterLookup('8', lp_numbers.Eight1.FileName),
            new LetterLookup('9', lp_numbers.Niner.FileName),
            new LetterLookup('0', lp_numbers.Zero1.FileName),
            new LetterLookup('1', lp_numbers.One2.FileName),
            new LetterLookup('2', lp_numbers.Two2.FileName),
            new LetterLookup('3', lp_numbers.Three2.FileName),
            new LetterLookup('4', lp_numbers.Four2.FileName),
            new LetterLookup('5', lp_numbers.Five2.FileName),
            new LetterLookup('6', lp_numbers.Six2.FileName),
            new LetterLookup('7', lp_numbers.Seven2.FileName),
            new LetterLookup('8', lp_numbers.Eight2.FileName),
            new LetterLookup('9', lp_numbers.Niner2.FileName),
            new LetterLookup('0', lp_numbers.Zero2.FileName),
        };
        ColorLookups = new List<ColorLookup>()
        {
            new ColorLookup(colour.COLORRED01.FileName, Color.Red),
            new ColorLookup(colour.COLORAQUA01.FileName, Color.Aqua),
            new ColorLookup(colour.COLORBEIGE01.FileName, Color.Beige),
            new ColorLookup(colour.COLORBLACK01.FileName, Color.Black),
            new ColorLookup(colour.COLORBLUE01.FileName, Color.Blue),
            new ColorLookup(colour.COLORBROWN01.FileName, Color.Brown),
            new ColorLookup(colour.COLORDARKBLUE01.FileName, Color.DarkBlue),
            new ColorLookup(colour.COLORDARKGREEN01.FileName, Color.DarkGreen),
            new ColorLookup(colour.COLORDARKGREY01.FileName, Color.DarkGray),
            new ColorLookup(colour.COLORDARKORANGE01.FileName, Color.DarkOrange),
            new ColorLookup(colour.COLORDARKRED01.FileName, Color.DarkRed),
            new ColorLookup(colour.COLORGOLD01.FileName, Color.Gold),
            new ColorLookup(colour.COLORGREEN01.FileName, Color.Green),
            new ColorLookup(colour.COLORGREY01.FileName, Color.Gray),
            new ColorLookup(colour.COLORGREY02.FileName, Color.Gray),
            new ColorLookup(colour.COLORLIGHTBLUE01.FileName, Color.LightBlue),
            new ColorLookup(colour.COLORMAROON01.FileName, Color.Maroon),
            new ColorLookup(colour.COLORORANGE01.FileName, Color.Orange),
            new ColorLookup(colour.COLORPINK01.FileName, Color.Pink),
            new ColorLookup(colour.COLORPURPLE01.FileName, Color.Purple),
            new ColorLookup(colour.COLORRED01.FileName, Color.Red),
            new ColorLookup(colour.COLORSILVER01.FileName, Color.Silver),
            new ColorLookup(colour.COLORWHITE01.FileName, Color.White),
            new ColorLookup(colour.COLORYELLOW01.FileName, Color.Yellow),
         };
        ColorIDLookups = new List<ColorIDLookup>()
        {
            new ColorIDLookup(colour.COLORBLACK01.FileName,0),
            new ColorIDLookup(colour.COLORBLACK01.FileName,1),
            new ColorIDLookup(colour.COLORBLACK01.FileName,2),
            new ColorIDLookup(colour.COLORSILVER01.FileName,3),
            new ColorIDLookup(colour.COLORSILVER01.FileName,4),
            new ColorIDLookup(colour.COLORSILVER01.FileName,5),
            new ColorIDLookup(colour.COLORSILVER01.FileName,6),
            new ColorIDLookup(colour.COLORSILVER01.FileName,7),
            new ColorIDLookup(colour.COLORSILVER01.FileName,8),
            new ColorIDLookup(colour.COLORSILVER01.FileName,9),
            new ColorIDLookup(colour.COLORGREY01.FileName,10),
            new ColorIDLookup(colour.COLORGREY01.FileName,11),
            new ColorIDLookup(colour.COLORBLACK01.FileName,12),
            new ColorIDLookup(colour.COLORGREY01.FileName,13),
            new ColorIDLookup(colour.COLORGREY01.FileName,14),
            new ColorIDLookup(colour.COLORBLACK01.FileName,15),
            new ColorIDLookup(colour.COLORBLACK01.FileName,16),
            new ColorIDLookup(colour.COLORSILVER01.FileName,17),
            new ColorIDLookup(colour.COLORSILVER01.FileName,18),
            new ColorIDLookup(colour.COLORGREY01.FileName,19),
            new ColorIDLookup(colour.COLORSILVER01.FileName,20),
            new ColorIDLookup(colour.COLORBLACK01.FileName,21),
            new ColorIDLookup(colour.COLORGREY01.FileName,22),
            new ColorIDLookup(colour.COLORGREY01.FileName,23),
            new ColorIDLookup(colour.COLORSILVER01.FileName,24),
            new ColorIDLookup(colour.COLORSILVER01.FileName,25),
            new ColorIDLookup(colour.COLORSILVER01.FileName,26),
            new ColorIDLookup(colour.COLORRED01.FileName,27),
            new ColorIDLookup(colour.COLORRED01.FileName,28),
            new ColorIDLookup(colour.COLORRED01.FileName,29),
            new ColorIDLookup(colour.COLORRED01.FileName,30),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,31),
            new ColorIDLookup(colour.COLORRED01.FileName,32),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,33),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,34),
            new ColorIDLookup(colour.COLORRED01.FileName,35),
            new ColorIDLookup(colour.COLORDARKORANGE01.FileName,36),
            new ColorIDLookup(colour.COLORGOLD01.FileName,37),
            new ColorIDLookup(colour.COLORORANGE01.FileName,38),
            new ColorIDLookup(colour.COLORRED01.FileName,39),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,40),
            new ColorIDLookup(colour.COLORORANGE01.FileName,41),
            new ColorIDLookup(colour.COLORYELLOW01.FileName,42),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,43),
            new ColorIDLookup(colour.COLORRED01.FileName,44),
            new ColorIDLookup(colour.COLORRED01.FileName,45),
            new ColorIDLookup(colour.COLORRED01.FileName,46),
            new ColorIDLookup(colour.COLORRED01.FileName,47),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,48),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,49),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,50),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,51),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,52),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,53),
            new ColorIDLookup(colour.COLORGREEN01.FileName,54),
            new ColorIDLookup(colour.COLORGREEN01.FileName,55),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,56),
            new ColorIDLookup(colour.COLORGREEN01.FileName,57),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,58),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,59),
            new ColorIDLookup(colour.COLORGREEN01.FileName,60),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,61),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,62),
            new ColorIDLookup(colour.COLORBLUE01.FileName,63),
            new ColorIDLookup(colour.COLORBLUE01.FileName,64),
            new ColorIDLookup(colour.COLORBLUE01.FileName,65),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,66),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,67),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,68),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,69),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,70),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,71),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,72),
            new ColorIDLookup(colour.COLORBLUE01.FileName,73),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,74),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,75),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,76),
            new ColorIDLookup(colour.COLORBLUE01.FileName,77),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,78),
            new ColorIDLookup(colour.COLORBLUE01.FileName,79),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,80),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,81),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,82),
            new ColorIDLookup(colour.COLORBLUE01.FileName,83),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,84),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,85),
            new ColorIDLookup(colour.COLORBLUE01.FileName,86),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,87),
            new ColorIDLookup(colour.COLORYELLOW01.FileName,88),
            new ColorIDLookup(colour.COLORYELLOW01.FileName,89),
            new ColorIDLookup(colour.COLORSILVER01.FileName,90),
            new ColorIDLookup(colour.COLORYELLOW01.FileName,91),
            new ColorIDLookup(colour.COLORGREEN01.FileName,92),
            new ColorIDLookup(colour.COLORBROWN01.FileName,93),
            new ColorIDLookup(colour.COLORBROWN01.FileName,94),
            new ColorIDLookup(colour.COLORBROWN01.FileName,95),
            new ColorIDLookup(colour.COLORBROWN01.FileName,96),
            new ColorIDLookup(colour.COLORBROWN01.FileName,97),
            new ColorIDLookup(colour.COLORBROWN01.FileName,98),
            new ColorIDLookup(colour.COLORBROWN01.FileName,99),
            new ColorIDLookup(colour.COLORBROWN01.FileName,100),
            new ColorIDLookup(colour.COLORBROWN01.FileName,101),
            new ColorIDLookup(colour.COLORBROWN01.FileName,102),
            new ColorIDLookup(colour.COLORBROWN01.FileName,103),
            new ColorIDLookup(colour.COLORBROWN01.FileName,104),
            new ColorIDLookup(colour.COLORBROWN01.FileName,105),
            new ColorIDLookup(colour.COLORBROWN01.FileName,106),
            new ColorIDLookup(colour.COLORBROWN01.FileName,107),
            new ColorIDLookup(colour.COLORBROWN01.FileName,108),
            new ColorIDLookup(colour.COLORBROWN01.FileName,109),
            new ColorIDLookup(colour.COLORBROWN01.FileName,110),
            new ColorIDLookup(colour.COLORWHITE01.FileName,111),
            new ColorIDLookup(colour.COLORWHITE01.FileName,112),
            new ColorIDLookup(colour.COLORBEIGE01.FileName,113),
            new ColorIDLookup(colour.COLORBROWN01.FileName,114),
            new ColorIDLookup(colour.COLORBROWN01.FileName,115),
            new ColorIDLookup(colour.COLORBEIGE01.FileName,116),
            new ColorIDLookup(colour.COLORSILVER01.FileName,117),
            new ColorIDLookup(colour.COLORSILVER01.FileName,118),
            new ColorIDLookup(colour.COLORSILVER01.FileName,119),
            new ColorIDLookup(colour.COLORSILVER01.FileName,120),
            new ColorIDLookup(colour.COLORWHITE01.FileName,121),
            new ColorIDLookup(colour.COLORWHITE01.FileName,122),
            new ColorIDLookup(colour.COLORORANGE01.FileName,123),
            new ColorIDLookup(colour.COLORORANGE01.FileName,124),
            new ColorIDLookup(colour.COLORGREEN01.FileName,125),
            new ColorIDLookup(colour.COLORYELLOW01.FileName,126),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,127),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,128),
            new ColorIDLookup(colour.COLORBROWN01.FileName,129),
            new ColorIDLookup(colour.COLORORANGE01.FileName,130),
            new ColorIDLookup(colour.COLORWHITE01.FileName,131),
            new ColorIDLookup(colour.COLORWHITE01.FileName,132),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,133),
            new ColorIDLookup(colour.COLORWHITE01.FileName,134),
            new ColorIDLookup(colour.COLORPINK01.FileName,135),
            new ColorIDLookup(colour.COLORPINK01.FileName,136),
            new ColorIDLookup(colour.COLORPINK01.FileName,137),
            new ColorIDLookup(colour.COLORORANGE01.FileName,138),
            new ColorIDLookup(colour.COLORGREEN01.FileName,139),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,140),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,141),
            new ColorIDLookup(colour.COLORPURPLE01.FileName,142),
            new ColorIDLookup(colour.COLORDARKRED01.FileName,143),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,144),
            new ColorIDLookup(colour.COLORPURPLE01.FileName,145),
            new ColorIDLookup(colour.COLORDARKBLUE01.FileName,146),
            new ColorIDLookup(colour.COLORBLACK01.FileName,147),
            new ColorIDLookup(colour.COLORPURPLE01.FileName,148),
            new ColorIDLookup(colour.COLORPURPLE01.FileName,149),
            new ColorIDLookup(colour.COLORRED01.FileName,150),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,151),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,152),
            new ColorIDLookup(colour.COLORBROWN01.FileName,153),
            new ColorIDLookup(colour.COLORBROWN01.FileName,154),
            new ColorIDLookup(colour.COLORDARKGREEN01.FileName,155),
            new ColorIDLookup(colour.COLORSILVER01.FileName,156),
            new ColorIDLookup(colour.COLORLIGHTBLUE01.FileName,157),
            new ColorIDLookup(colour.COLORGOLD01.FileName,158),
            new ColorIDLookup(colour.COLORGOLD01.FileName,159),


         };
        VehicleModelLookups = new List<VehicleModelLookup>
        {
            new VehicleModelLookup("airtug", 0x5D0AAC8F, model.AIRTUG01.FileName),
            new VehicleModelLookup("akuma", 0x63ABADE7, model.AKUMA01.FileName),
            new VehicleModelLookup("asea", 0x94204D89, model.ASEA01.FileName),
            new VehicleModelLookup("asea2", 0x9441D8D5, model.ASEA01.FileName),
            new VehicleModelLookup("asterope", 0x8E9254FB, model.ASTEROPE01.FileName),
            new VehicleModelLookup("bagger", 0x806B9CC3, model.BAGGER01.FileName),
            new VehicleModelLookup("baller", 0xCFCA3668, model.BALLER01.FileName),
            new VehicleModelLookup("baller2", 0x8852855, model.BALLER01.FileName),
            new VehicleModelLookup("baller3", 0x6FF0F727, model.BALLER01.FileName),
            new VehicleModelLookup("baller4", 0x25CBE2E2, model.BALLER01.FileName),
            new VehicleModelLookup("baller5", 0x1C09CF5E, model.BALLER01.FileName),
            new VehicleModelLookup("baller6", 0x27B4E6B0, model.BALLER01.FileName),
            new VehicleModelLookup("banshee", 0xC1E908D2, model.BANSHEE01.FileName),
            new VehicleModelLookup("banshee2", 0x25C5AF13, model.BANSHEE01.FileName),
            new VehicleModelLookup("barracks", 0xCEEA3F4B, model.BARRACKS01.FileName),
            new VehicleModelLookup("barracks2", 0x4008EABB, model.BARRACKS01.FileName),
            new VehicleModelLookup("barracks3", 0x2592B5CF, model.BARRACKS01.FileName),
            new VehicleModelLookup("bati", 0xF9300CC5, model.BATI01.FileName),
            new VehicleModelLookup("bati2", 0xCADD5D2D, model.BATI01.FileName),
            new VehicleModelLookup("benson", 0x7A61B330, model.BENSON01.FileName),
            new VehicleModelLookup("bfinjection", 0x432AA566, model.BFINJECTION01.FileName),
            new VehicleModelLookup("biff", 0x32B91AE8, model.BIFF01.FileName),
            new VehicleModelLookup("bison", 0xFEFD644F, model.BISON01.FileName),
            new VehicleModelLookup("bison2", 0x7B8297C5, model.BISON01.FileName),
            new VehicleModelLookup("bison3", 0x67B3F020, model.BISON01.FileName),
            new VehicleModelLookup("bjxl", 0x32B29A4B, model.BJXL01.FileName),
            new VehicleModelLookup("blazer", 0x8125BCF9, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer2", 0xFD231729, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer3", 0xB44F0582, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer4", 0xE5BA6858, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer5", 0xA1355F67, model.BLAZER01.FileName),
            new VehicleModelLookup("blista", 0xEB70965F, model.BLISTA01.FileName),
            new VehicleModelLookup("blista2", 0x3DEE5EDA, model.BLISTA01.FileName),
            new VehicleModelLookup("blista3", 0xDCBC1C3B, model.BLISTA01.FileName),
            new VehicleModelLookup("bobcatxl", 0x3FC5D440, model.BOBCATXL01.FileName),
            new VehicleModelLookup("bodhi2", 0xAA699BB6, model.BODHI01.FileName),
            new VehicleModelLookup("boxville", 0x898ECCEA, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville2", 0xF21B33BE, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville3", 0x07405E08, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville4", 0x1A79847A, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville5", 0x28AD20E1, model.BOXVILLE01.FileName),
            new VehicleModelLookup("buccaneer", 0xD756460C, model.BUCCANEER01.FileName),
            new VehicleModelLookup("buccaneer2", 0xC397F748, model.BUCCANEER01.FileName),
            new VehicleModelLookup("buffalo", 0xEDD516C6, model.BUFFALO01.FileName),
            new VehicleModelLookup("buffalo2", 0x2BEC3CBE, model.BUFFALO01.FileName),
            new VehicleModelLookup("buffalo3", 0xE2C013E, model.BUFFALO01.FileName),
            new VehicleModelLookup("bullet", 0x9AE6DDA1, model.BULLET01.FileName),
            new VehicleModelLookup("burrito", 0xAFBB2CA4, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito2", 0xC9E8FF76, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito3", 0x98171BD3, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito4", 0x353B561D, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito5", 0x437CF2A0, model.BURRITO01.FileName),
            new VehicleModelLookup("bus", 0xD577C962, model.BUS01.FileName),
            new VehicleModelLookup("caddy", 0x44623884, model.CADDY01.FileName),
            new VehicleModelLookup("caddy2", 0xDFF0594C, model.CADDY01.FileName),
            new VehicleModelLookup("caddy3", 0xD227BDBB, model.CADDY01.FileName),
            new VehicleModelLookup("camper", 0x6FD95F68, model.CAMPER01.FileName),
            new VehicleModelLookup("carbonizzare", 0x7B8AB45F, model.CARBONIZZARE01.FileName),
            new VehicleModelLookup("carbonrs", 0xABB0C0, model.CARBONRS01.FileName),
            new VehicleModelLookup("cavalcade", 0x779F23AA, model.CAVALCADE01.FileName),
            new VehicleModelLookup("cavalcade2", 0xD0EB2BE5, model.CAVALCADE01.FileName),
            new VehicleModelLookup("cheetah", 0xB1D95DA0, model.CHEETAH01.FileName),
            new VehicleModelLookup("cheetah2", 0xD4E5F4D, model.CHEETAH01.FileName),
            new VehicleModelLookup("coach", 0x84718D34, model.COACH01.FileName),
            new VehicleModelLookup("cog55", 0x360A438E, model.COG5501.FileName),
            new VehicleModelLookup("cog552", 0x29FCD3E4, model.COG5501.FileName),
            new VehicleModelLookup("cogcabrio", 0x13B57D8A, model.COGCABRIO01.FileName),
            new VehicleModelLookup("cognoscenti", 0x86FE0B60, model.COGNOSCENTI01.FileName),
            new VehicleModelLookup("cognoscenti2", 0xDBF2D57A, model.COGNOSCENTI01.FileName),
            new VehicleModelLookup("comet2", 0xC1AE4D16, model.COMET01.FileName),
            new VehicleModelLookup("comet3", 0x877358AD, model.COMET01.FileName),
            new VehicleModelLookup("comet4", 0x5D1903F9, model.COMET01.FileName),
            new VehicleModelLookup("coquette", 0x67BC037, model.COQUETTE01.FileName),
            new VehicleModelLookup("coquette2", 0x3C4E2113, model.COQUETTE01.FileName),
            new VehicleModelLookup("coquette3", 0x2EC385FE, model.COQUETTE01.FileName),
            new VehicleModelLookup("daemon", 0x77934CEE, model.DAEMON01.FileName),
            new VehicleModelLookup("daemon2", 0xAC4E93C9, model.DAEMON01.FileName),
            new VehicleModelLookup("dilettante", 0xBC993509, model.DILETTANTE01.FileName),
            new VehicleModelLookup("dilettante2", 0x64430650, model.DILETTANTE01.FileName),
            new VehicleModelLookup("dinghy", 0x3D961290, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy2", 0x107F392C, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy3", 0x1E5E54EA, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy4", 0x33B47F96, model.DINGHY01.FileName),
            new VehicleModelLookup("dominator", 0x4CE68AC, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator2", 0xC96B73D9, model.COQUETTE01.FileName),
            new VehicleModelLookup("dominator3", 0xC52C6B93, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator4", 0xD6FB0F30, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator5", 0xAE0A3D4F, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator6", 0xB2E046FB, model.DOMINATOR01.FileName),
            new VehicleModelLookup("double", 0x9C669788, model.DOUBLE01.FileName),
            new VehicleModelLookup("dubsta", 0x462FE277, model.DUBSTA01.FileName),
            new VehicleModelLookup("dubsta2", 0xE882E5F6, model.DUBSTA01.FileName),
            new VehicleModelLookup("dubsta3", 0xB6410173, model.DUBSTA01.FileName),
            new VehicleModelLookup("dukes", 0x2B26F456, model.DUKES01.FileName),
            new VehicleModelLookup("dukes2", 0xEC8F7094, model.DUKES01.FileName),
            new VehicleModelLookup("elegy", 0xBBA2261, model.ELEGY01.FileName),
            new VehicleModelLookup("elegy2", 0xDE3D9D22, model.ELEGY01.FileName),
            new VehicleModelLookup("emperor", 0xD7278283, model.EMPEROR01.FileName),
            new VehicleModelLookup("emperor2", 0x8FC3AADC, model.EMPEROR01.FileName),
            new VehicleModelLookup("emperor3", 0xB5FCF74E, model.EMPEROR01.FileName),
            new VehicleModelLookup("entityxf", 0xB2FE5CF9, model.ENTITYXF01.FileName),
            new VehicleModelLookup("exemplar", 0xFFB15B5E, model.EXEMPLAR01.FileName),
            new VehicleModelLookup("f620", 0xDCBCBE48, model.F62001.FileName),
            new VehicleModelLookup("faction", 0x81A9CDDF, model.FACTION01.FileName),
            new VehicleModelLookup("faction2", 0x95466BDB, model.FACTION01.FileName),
            new VehicleModelLookup("faction3", 0x866BCE26, model.FACTION01.FileName),
            new VehicleModelLookup("faggio", 0x9229E4EB, model.FAGGIO01.FileName),
            new VehicleModelLookup("faggio2", 0x350D1AB, model.FAGGIO01.FileName),
            new VehicleModelLookup("faggio3", 0xB328B188, model.FAGGIO01.FileName),
            //new VehicleModelLookup("fbi", 0x432EA949, model.POLICECAR01.FileName),
            //new VehicleModelLookup("fbi2", 0x9DC66994, model.POLICECAR01.FileName),
            new VehicleModelLookup("fcr", 0x25676EAF, model.DOMINATOR01.FileName),
            new VehicleModelLookup("felon", 0xE8A8BDA8, model.FELON01.FileName),
            new VehicleModelLookup("felon2", 0xFAAD85EE, model.FELON01.FileName),
            new VehicleModelLookup("feltzer2", 0x8911B9F5, model.FELTZER01.FileName),
            new VehicleModelLookup("feltzer3", 0xA29D6D10, model.FELTZER01.FileName),
            new VehicleModelLookup("firetruk", 0x73920F8E, model.FIRETRUCK01.FileName),
            new VehicleModelLookup("fq2", 0xBC32A33B, model.FQ201.FileName),
            new VehicleModelLookup("fugitive", 0x71CB2FFB, model.FUGITIVE01.FileName),
            new VehicleModelLookup("fusilade", 0x1DC0BA53, model.FUSILADE01.FileName),
            new VehicleModelLookup("futo", 0x7836CE2F, model.FUTO01.FileName),
            new VehicleModelLookup("gauntlet", 0x94B395C5, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet2", 0x14D22159, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet3", 0x2B0C4DCD, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet4", 0x734C5E50, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gburrito", 0x97FA4F36, model.BURRITO01.FileName),
            new VehicleModelLookup("gburrito2", 0x11AA0E14, model.BURRITO01.FileName),
            new VehicleModelLookup("granger", 0x9628879C, model.GRANGER01.FileName),
            new VehicleModelLookup("gresley", 0xA3FC0F4D, model.GRESLEY01.FileName),
            new VehicleModelLookup("habanero", 0x34B7390F, model.HABANERO01.FileName),
            new VehicleModelLookup("hotknife", 0x239E390, model.HOTKNIFE01.FileName),
            new VehicleModelLookup("infernus", 0x18F25AC7, model.INFERNUS01.FileName),
            new VehicleModelLookup("infernus2", 0xAC33179C, model.INFERNUS01.FileName),
            new VehicleModelLookup("ingot", 0xB3206692, model.INGOT01.FileName),
            new VehicleModelLookup("intruder", 0x34DD8AA1, model.INTRUDER01.FileName),
            new VehicleModelLookup("issi2", 0xB9CB3B69, model.ISSI01.FileName),
            new VehicleModelLookup("issi3", 0x378236E1, model.ISSI01.FileName),
            new VehicleModelLookup("issi4", 0x256E92BA, model.ISSI01.FileName),
            new VehicleModelLookup("issi5", 0x5BA0FF1E, model.ISSI01.FileName),
            new VehicleModelLookup("issi6", 0x49E25BA1, model.ISSI01.FileName),
            new VehicleModelLookup("issi7", 0x6E8DA4F7, model.ISSI01.FileName),
            new VehicleModelLookup("jackal", 0xDAC67112, model.JACKAL01.FileName),
            new VehicleModelLookup("jb700", 0x3EAB5555, model.JB70001.FileName),
            new VehicleModelLookup("journey", 0xF8D48E7A, model.JOURNEY01.FileName),
            new VehicleModelLookup("khamelion", 0x206D1B68, model.KHAMELION01.FileName),
            new VehicleModelLookup("landstalker", 0x4BA4E8DC, model.LANDSTALKER01.FileName),
            new VehicleModelLookup("manana", 0x81634188, model.MANANA01.FileName),
            new VehicleModelLookup("mesa", 0x36848602, model.MESA01.FileName),
            new VehicleModelLookup("mesa2", 0xD36A4B44, model.MESA01.FileName),
            new VehicleModelLookup("mesa3", 0x84F42E51, model.MESA01.FileName),
            new VehicleModelLookup("minivan", 0xED7EADA4, model.MINIVAN01.FileName),
            new VehicleModelLookup("minivan2", 0xBCDE91F0, model.MINIVAN01.FileName),
            new VehicleModelLookup("mixer", 0xD138A6BB, model.MIXER01.FileName),
            new VehicleModelLookup("mixer2", 0x1C534995, model.MIXER01.FileName),
            new VehicleModelLookup("monroe", 0xE62B361B, model.MONROE01.FileName),
            new VehicleModelLookup("mule", 0x35ED670B, model.MULE01.FileName),
            new VehicleModelLookup("mule2", 0xC1632BEB, model.MULE01.FileName),
            new VehicleModelLookup("mule3", 0x85A5B471, model.MULE01.FileName),
            new VehicleModelLookup("mule4", 0x73F4110E, model.MULE01.FileName),
            new VehicleModelLookup("nemesis", 0xDA288376, model.NEMESIS01.FileName),
            new VehicleModelLookup("ninef", 0x3D8FA25C, model.NINEF01.FileName),
            new VehicleModelLookup("ninef2", 0xA8E38B01, model.NINEF01.FileName),
            new VehicleModelLookup("oracle", 0x506434F6, model.ORACLE01.FileName),
            new VehicleModelLookup("oracle2", 0xE18195B2, model.ORACLE01.FileName),
            new VehicleModelLookup("packer", 0x21EEE87D, model.PACKER01.FileName),
            new VehicleModelLookup("patriot", 0xCFCFEB3B, model.PATRIOT01.FileName),
            new VehicleModelLookup("patriot2", 0xE6E967F8, model.PATRIOT01.FileName),
            new VehicleModelLookup("pcj", 0xC9CEAF06, model.PCJ60001.FileName),
            new VehicleModelLookup("penumbra", 0xE9805550, model.PENUMBRA01.FileName),
            new VehicleModelLookup("peyote", 0x6D19CCBC, model.PEYOTE01.FileName),
            new VehicleModelLookup("peyote2", 0x9472CD24, model.PEYOTE01.FileName),
            new VehicleModelLookup("phantom", 0x809AA4CB, model.PHANTOM01.FileName),
            new VehicleModelLookup("phantom2", 0x9DAE1398, model.PHANTOM01.FileName),
            new VehicleModelLookup("phantom3", 0xA90ED5C, model.PHANTOM01.FileName),
            new VehicleModelLookup("phoenix", 0x831A21D5, model.PHOENIX01.FileName),
            new VehicleModelLookup("picador", 0x59E0FBF3, model.PICADOR01.FileName),
            //new VehicleModelLookup("police", 0x79FBB0C5, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police2", 0x9F05F101, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police3", 0x71FA16EA, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police4", 0x8A63C7B9, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeb", 0xFDEFAEC3, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeold1", 0xA46462F7, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeold2", 0x95F4C618, model.POLICECAR01.FileName),
            new VehicleModelLookup("policet", 0x1B38E955, model.POLICETRANSPORT01.FileName),
            new VehicleModelLookup("polmav", 0x1517D4D9, model.POLICEMAVERICK01.FileName),
            new VehicleModelLookup("pony", 0xF8DE29A8, model.PONY01.FileName),
            new VehicleModelLookup("pony2", 0x38408341, model.PONY01.FileName),
            new VehicleModelLookup("pounder", 0x7DE35E7D, model.POUNDER01.FileName),
            new VehicleModelLookup("pounder2", 0x6290F15B, model.POUNDER01.FileName),
            new VehicleModelLookup("prairie", 0xA988D3A2, model.PRAIRIE01.FileName),
            new VehicleModelLookup("pranger", 0x2C33B46E, model.GRANGER01.FileName),
            new VehicleModelLookup("predator", 0xE2E7D4AB, model.PREDATOR01.FileName),
            new VehicleModelLookup("premier", 0x8FB66F9B, model.PREMIER01.FileName),
            new VehicleModelLookup("primo", 0xBB6B404F, model.PRIMO01.FileName),
            new VehicleModelLookup("primo2", 0x86618EDA, model.PRIMO01.FileName),
            new VehicleModelLookup("radi", 0x9D96B45B, model.RADI01.FileName),
            new VehicleModelLookup("rancherxl", 0x6210CBB0, model.RANCHERXL01.FileName),
            new VehicleModelLookup("rancherxl2", 0x7341576B, model.RANCHERXL01.FileName),
            new VehicleModelLookup("rapidgt", 0x8CB29A14, model.RAPIDGT01.FileName),
            new VehicleModelLookup("rapidgt2", 0x679450AF, model.RAPIDGT01.FileName),
            new VehicleModelLookup("rapidgt3", 0x7A2EF5E4, model.RAPIDGT01.FileName),
            new VehicleModelLookup("ratloader", 0xD83C13CE, model.RATLOADER01.FileName),
            new VehicleModelLookup("ratloader2", 0xDCE1D9F7, model.RATLOADER01.FileName),
            new VehicleModelLookup("rebel", 0xB802DD46, model.REBEL01.FileName),
            new VehicleModelLookup("rebel2", 0x8612B64B, model.REBEL01.FileName),
            new VehicleModelLookup("regina", 0xFF22D208, model.REGINA01.FileName),
            new VehicleModelLookup("rhino", 0x2EA68690, model.RHINO01.FileName),
            new VehicleModelLookup("riot", 0xB822A1AA, model.RIOT01.FileName),
            new VehicleModelLookup("riot2", 0x9B16A3B4, model.RIOT01.FileName),
            new VehicleModelLookup("rocoto", 0x7F5C91F1, model.ROCOTO01.FileName),
            new VehicleModelLookup("romero", 0x2560B2FC, model.HEARSE01.FileName),
            new VehicleModelLookup("ruiner", 0xF26CEFF9, model.RUINER01.FileName),
            new VehicleModelLookup("ruiner2", 0x381E10BD, model.RUINER01.FileName),
            new VehicleModelLookup("ruiner3", 0x2E5AFD37, model.RUINER01.FileName),
            new VehicleModelLookup("adder", 0xB779A091, model.ADDER01.FileName),
        };
        VehicleClassLookups = new List<VehicleClassLookup>()
        {
            new VehicleClassLookup("Compact",0,vehicle_category.TwoDoor01.FileName),
            new VehicleClassLookup("Sedan",1,vehicle_category.Sedan.FileName),
            new VehicleClassLookup("SUV",2,vehicle_category.SUV01.FileName),
            new VehicleClassLookup("Coupe",3,vehicle_category.Coupe01.FileName),
            new VehicleClassLookup("Muscle",4,vehicle_category.MuscleCar01.FileName),
            new VehicleClassLookup("Sports Classic",5,vehicle_category.SportsCar01.FileName),
            new VehicleClassLookup("Sports Car",6,vehicle_category.SportsCar01.FileName),
            new VehicleClassLookup("Super",7,vehicle_category.PerformanceCar01.FileName),
            new VehicleClassLookup("Motorcycle",8,vehicle_category.Motorcycle01.FileName),
            new VehicleClassLookup("Off Road",9,vehicle_category.OffRoad01.FileName),
            new VehicleClassLookup("Industrial",10,vehicle_category.IndustrialVehicle01.FileName),
            new VehicleClassLookup("Utility",11,vehicle_category.UtilityVehicle01.FileName),
            new VehicleClassLookup("Van",12,vehicle_category.Van01.FileName),
            new VehicleClassLookup("Bicycle",13,vehicle_category.Bicycle01.FileName),
            new VehicleClassLookup("Boat",14,vehicle_category.Boat01.FileName),
            new VehicleClassLookup("Helicopter",15,vehicle_category.Helicopter01.FileName),
            new VehicleClassLookup("Plane",16,vehicle_category.Sedan.FileName),
            new VehicleClassLookup("Service",17,vehicle_category.Service01.FileName),
            new VehicleClassLookup("Emergency",18,vehicle_category.PoliceCar.FileName),
            new VehicleClassLookup("Military",19,vehicle_category.TroopTransport.FileName),
            new VehicleClassLookup("Commercial",20,vehicle_category.UtilityVehicle01.FileName),
            new VehicleClassLookup("Train",21,vehicle_category.Train01.FileName),
        };
        VehicleMakeLookups = new List<VehicleMakeLookup>()
        {
            new VehicleMakeLookup("Albany",manufacturer.ALBANY01.FileName),
            new VehicleMakeLookup("Annis",manufacturer.ANNIS01.FileName),
            new VehicleMakeLookup("Benefactor",manufacturer.BENEFACTOR01.FileName),
            new VehicleMakeLookup("Bollokan",manufacturer.BOLLOKAN01.FileName),
            new VehicleMakeLookup("Bravado",manufacturer.BRAVADO01.FileName),
            new VehicleMakeLookup("Brute",manufacturer.BRUTE01.FileName),
            new VehicleMakeLookup("Buckingham",""),
            new VehicleMakeLookup("Burgerfahrzeug",manufacturer.BF01.FileName),
            new VehicleMakeLookup("Canis",manufacturer.CANIS01.FileName),
            new VehicleMakeLookup("Chariot",manufacturer.CHARIOT01.FileName),
            new VehicleMakeLookup("Cheval",manufacturer.CHEVAL01.FileName),
            new VehicleMakeLookup("Classique",manufacturer.CLASSIQUE01.FileName),
            new VehicleMakeLookup("Coil",manufacturer.COIL01.FileName),
            new VehicleMakeLookup("Declasse",manufacturer.DECLASSE01.FileName),
            new VehicleMakeLookup("Dewbauchee",manufacturer.DEWBAUCHEE01.FileName),
            new VehicleMakeLookup("Dinka",manufacturer.DINKA01.FileName),
            new VehicleMakeLookup("DUDE",""),
            new VehicleMakeLookup("Dundreary",manufacturer.DUNDREARY01.FileName),
            new VehicleMakeLookup("Emperor",manufacturer.EMPEROR01.FileName),
            new VehicleMakeLookup("Enus",manufacturer.ENUS01.FileName),
            new VehicleMakeLookup("Fathom",manufacturer.FATHOM01.FileName),
            new VehicleMakeLookup("Gallivanter",manufacturer.GALLIVANTER01.FileName),
            new VehicleMakeLookup("Grotti",manufacturer.GROTTI01.FileName),
            new VehicleMakeLookup("Hijak",manufacturer.HIJAK01.FileName),
            new VehicleMakeLookup("HVY",manufacturer.HVY01.FileName),
            new VehicleMakeLookup("Imponte",manufacturer.IMPONTE01.FileName),
            new VehicleMakeLookup("Invetero",manufacturer.INVETERO01.FileName),
            new VehicleMakeLookup("JackSheepe",manufacturer.JACKSHEEPE01.FileName),
            new VehicleMakeLookup("Jobuilt",manufacturer.JOEBUILT01.FileName),
            new VehicleMakeLookup("Karin",manufacturer.KARIN01.FileName),
            new VehicleMakeLookup("Kraken Submersibles",""),
            new VehicleMakeLookup("Lampadati",manufacturer.LAMPADATI01.FileName),
            new VehicleMakeLookup("Liberty Chop Shop",""),
            new VehicleMakeLookup("Liberty City Cycles",""),
            new VehicleMakeLookup("Maibatsu Corporation",manufacturer.MAIBATSU01.FileName),
            new VehicleMakeLookup("Mammoth",manufacturer.MAMMOTH01.FileName),
            new VehicleMakeLookup("MTL",manufacturer.MTL01.FileName),
            new VehicleMakeLookup("Nagasaki",manufacturer.NAGASAKI01.FileName),
            new VehicleMakeLookup("Obey",manufacturer.OBEY01.FileName),
            new VehicleMakeLookup("Ocelot",manufacturer.OCELOT01.FileName),
            new VehicleMakeLookup("Overflod",manufacturer.OVERFLOD01.FileName),
            new VehicleMakeLookup("Pegassi",manufacturer.PEGASI01.FileName),
            new VehicleMakeLookup("Pfister",""),
            new VehicleMakeLookup("Principe",manufacturer.PRINCIPE01.FileName),
            new VehicleMakeLookup("Progen",manufacturer.PROGEN01.FileName),
            new VehicleMakeLookup("ProLaps",""),
            new VehicleMakeLookup("RUNE",""),
            new VehicleMakeLookup("Schyster",manufacturer.SCHYSTER01.FileName),
            new VehicleMakeLookup("Shitzu",manufacturer.SHITZU01.FileName),
            new VehicleMakeLookup("Speedophile",manufacturer.SPEEDOPHILE01.FileName),
            new VehicleMakeLookup("Stanley",manufacturer.STANLEY01.FileName),
            new VehicleMakeLookup("SteelHorse",manufacturer.STEELHORSE01.FileName),
            new VehicleMakeLookup("Truffade",manufacturer.TRUFFADE01.FileName),
            new VehicleMakeLookup("Ubermacht",manufacturer.UBERMACHT01.FileName),
            new VehicleMakeLookup("Vapid",manufacturer.VAPID01.FileName),
            new VehicleMakeLookup("Vulcar",manufacturer.VULCAR01.FileName),
            new VehicleMakeLookup("Vysser",""),
            new VehicleMakeLookup("Weeny",manufacturer.WEENY01.FileName),
            new VehicleMakeLookup("Western Company",manufacturer.WESTERNCOMPANY01.FileName),
            new VehicleMakeLookup("Western Motorcycle Company",manufacturer.WESTERNMOTORCYCLECOMPANY01.FileName),
            new VehicleMakeLookup("Willard",""),
            new VehicleMakeLookup("Zirconium",manufacturer.ZIRCONIUM01.FileName),
        };
    }
    private class ColorLookup
    {
        public Color BaseColor { get; set; }
        public string ScannerFile { get; set; }
        public ColorLookup(string _ScannerFile, Color _BaseColor)
        {
            BaseColor = _BaseColor;
            ScannerFile = _ScannerFile;
        }

    }
    private class ColorIDLookup
    {
        public int PrimaryColor { get; set; }
        public string ScannerFile { get; set; }
        public ColorIDLookup(string _ScannerFile, int _PrimaryColor)
        {
            PrimaryColor = _PrimaryColor;
            ScannerFile = _ScannerFile;
        }

    }
    private class LetterLookup
    {
        public char AlphaNumeric { get; set; }
        public string ScannerFile { get; set; }
        public LetterLookup(char _AlphaNumeric, string _ScannerFile)
        {
            AlphaNumeric = _AlphaNumeric;
            ScannerFile = _ScannerFile;
        }

    }
    private class VehicleModelLookup
    {
        public string Name { get; set; }
        public uint Hash { get; set; }
        public string ScannerFile { get; set; } = "";
        public VehicleModelLookup(string _Name, uint _Hash, string _ScannerFile)
        {
            Name = _Name;
            Hash = _Hash;
            ScannerFile = _ScannerFile;
        }
    }
    private class VehicleMakeLookup
    {
        public string MakeName { get; set; }
        public string ScannerFile { get; set; } = "";
        public VehicleMakeLookup(string makeName, string scannerFile)
        {
            MakeName = makeName;
            ScannerFile = scannerFile;
        }
    }
    private class VehicleClassLookup
    {
        public string Name { get; set; }
        public int GameClass { get; set; }
        public string ScannerFile { get; set; } = "";
        public VehicleClassLookup(string name, int gameClass, string scannerFile)
        {
            Name = name;
            GameClass = gameClass;
            ScannerFile = scannerFile;
        }
    }

}
