using ExtensionsMethods;
using Instant_Action_RAGE.Systems;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScannerAudio;
using static Zones;

internal static class DispatchAudioSystem
    {
        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;
        private static Random rnd;

    public static bool ReportedOfficerDown { get; set; } = false;
    public static bool ReportedShotsFired { get; set; } = false;
    public static bool ReportedAssaultOnOfficer { get; set; } = false;
    public static bool ReportedCarryingWeapon { get; set; } = false;
    public static bool ReportedLethalForceAuthorized { get; set; } = false;
    public static bool ReportedThreateningWithAFirearm { get; set; } = false;

    private static List<DispatchLettersNumber> LettersAndNumbersLookup = new List<DispatchLettersNumber>();
    public static List<VehicleModelNameLookup> ModelNameLookup = new List<VehicleModelNameLookup>();
    public static List<ColorLookup> ColorLookups = new List<ColorLookup>();
    public static bool IsRunning { get; set; } = true;
    static DispatchAudioSystem()
        {
            rnd = new Random();
        }
    public static void Initialize()
    {
        SetupLists();
        MainLoop();
    }

    private static void SetupLists()
    {
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', ScannerAudio.lp_letters_high.Adam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', ScannerAudio.lp_letters_high.Boy.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', ScannerAudio.lp_letters_high.Charles.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('D', ScannerAudio.lp_letters_high.David.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', ScannerAudio.lp_letters_high.Edward.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', ScannerAudio.lp_letters_high.Frank.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', ScannerAudio.lp_letters_high.George.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', ScannerAudio.lp_letters_high.Henry.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', ScannerAudio.lp_letters_high.Ita.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', ScannerAudio.lp_letters_high.John.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', ScannerAudio.lp_letters_high.King.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', ScannerAudio.lp_letters_high.Lincoln.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', ScannerAudio.lp_letters_high.Mary.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', ScannerAudio.lp_letters_high.Nora.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', ScannerAudio.lp_letters_high.Ocean.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', ScannerAudio.lp_letters_high.Paul.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', ScannerAudio.lp_letters_high.Queen.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', ScannerAudio.lp_letters_high.Robert.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', ScannerAudio.lp_letters_high.Sam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', ScannerAudio.lp_letters_high.Tom.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', ScannerAudio.lp_letters_high.Union.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', ScannerAudio.lp_letters_high.Victor.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', ScannerAudio.lp_letters_high.William.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', ScannerAudio.lp_letters_high.XRay.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', ScannerAudio.lp_letters_high.Young.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', ScannerAudio.lp_letters_high.Zebra.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', ScannerAudio.lp_letters_high.Adam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', ScannerAudio.lp_letters_high.Boy1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', ScannerAudio.lp_letters_high.Charles1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', ScannerAudio.lp_letters_high.Edward1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', ScannerAudio.lp_letters_high.Frank1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', ScannerAudio.lp_letters_high.George1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', ScannerAudio.lp_letters_high.Henry1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', ScannerAudio.lp_letters_high.Ita1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', ScannerAudio.lp_letters_high.John1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', ScannerAudio.lp_letters_high.King1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', ScannerAudio.lp_letters_high.Lincoln1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', ScannerAudio.lp_letters_high.Mary1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', ScannerAudio.lp_letters_high.Nora1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', ScannerAudio.lp_letters_high.Ocean1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', ScannerAudio.lp_letters_high.Paul1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', ScannerAudio.lp_letters_high.Queen1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', ScannerAudio.lp_letters_high.Robert1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', ScannerAudio.lp_letters_high.Sam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', ScannerAudio.lp_letters_high.Tom1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', ScannerAudio.lp_letters_high.Union1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', ScannerAudio.lp_letters_high.Victor1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', ScannerAudio.lp_letters_high.William1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', ScannerAudio.lp_letters_high.XRay1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', ScannerAudio.lp_letters_high.Young1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', ScannerAudio.lp_letters_high.Zebra1.FileName));



        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', ScannerAudio.lp_numbers.One.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', ScannerAudio.lp_numbers.Two.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', ScannerAudio.lp_numbers.Three.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', ScannerAudio.lp_numbers.Four.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', ScannerAudio.lp_numbers.Five.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', ScannerAudio.lp_numbers.Six.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', ScannerAudio.lp_numbers.Seven.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', ScannerAudio.lp_numbers.Eight.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', ScannerAudio.lp_numbers.Nine.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', ScannerAudio.lp_numbers.Zero.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', ScannerAudio.lp_numbers.One1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', ScannerAudio.lp_numbers.Two1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', ScannerAudio.lp_numbers.Three1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', ScannerAudio.lp_numbers.Four1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', ScannerAudio.lp_numbers.Five1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', ScannerAudio.lp_numbers.Six1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', ScannerAudio.lp_numbers.Seven1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', ScannerAudio.lp_numbers.Eight1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', ScannerAudio.lp_numbers.Niner.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', ScannerAudio.lp_numbers.Zero1.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', ScannerAudio.lp_numbers.One2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', ScannerAudio.lp_numbers.Two2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', ScannerAudio.lp_numbers.Three2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', ScannerAudio.lp_numbers.Four2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', ScannerAudio.lp_numbers.Five2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', ScannerAudio.lp_numbers.Six2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', ScannerAudio.lp_numbers.Seven2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', ScannerAudio.lp_numbers.Eight2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', ScannerAudio.lp_numbers.Niner2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', ScannerAudio.lp_numbers.Zero2.FileName));

        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORRED01.FileName, Color.Red));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORAQUA01.FileName, Color.Aqua));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORBEIGE01.FileName, Color.Beige));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORBLACK01.FileName, Color.Black));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORBLUE01.FileName, Color.Blue));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORBROWN01.FileName, Color.Brown));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORDARKBLUE01.FileName, Color.DarkBlue));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORDARKGREEN01.FileName, Color.DarkGreen));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORDARKGREY01.FileName, Color.DarkGray));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORDARKORANGE01.FileName, Color.DarkOrange));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORDARKRED01.FileName, Color.DarkRed));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORGOLD01.FileName, Color.Gold));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORGREEN01.FileName, Color.Green));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORGREY01.FileName, Color.Gray));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORGREY02.FileName, Color.Gray));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORLIGHTBLUE01.FileName, Color.LightBlue));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORMAROON01.FileName,Color.Maroon ));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORORANGE01.FileName, Color.Orange));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORPINK01.FileName, Color.Pink));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORPURPLE01.FileName, Color.Purple));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORRED01.FileName, Color.Red));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORSILVER01.FileName, Color.Silver));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORWHITE01.FileName, Color.White));
        ColorLookups.Add(new ColorLookup(ScannerAudio.colour.COLORYELLOW01.FileName, Color.Yellow));

        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELADDER01.FileName, "ADDER_01",manufacturer.MANUFACTURERTRUFFADE01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELAIRSHIP01.FileName, "AIRSHIP_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELAIRTUG01.FileName, "AIRTUG_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELAKUMA01.FileName, "AKUMA_01", manufacturer.MANUFACTURERDINKA01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELAMBULANCE01.FileName, "AMBULANCE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELANNIHILATOR01.FileName, "ANNIHILATOR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELASEA01.FileName, "ASEA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELASTEROPE01.FileName, "ASTEROPE_01", manufacturer.MANUFACTURERKARIN01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBAGGER01.FileName, "BAGGER_01", manufacturer.MANUFACTURERWESTERNMOTORCYCLECOMPANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBALLER01.FileName, "BALLER_01", manufacturer.MANUFACTURERGALLIVANTER01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBANSHEE01.FileName, "BANSHEE_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBARRACKS01.FileName, "BARRACKS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBATI01.FileName, "BATI_01", manufacturer.MANUFACTURERPEGASI01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBENSON01.FileName, "BENSON_01", manufacturer.MANUFACTURERVAPID01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBFINJECTION01.FileName, "BF_INJECTION_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBIFF01.FileName, "BIFF_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBISON01.FileName, "BISON_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBJXL01.FileName, "BJXL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBLAZER01.FileName, "BLAZER_01", manufacturer.MANUFACTURERNAGASAKI01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBLIMP01.FileName, "BLIMP_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBLISTA01.FileName, "BLISTA_01", manufacturer.MANUFACTURERDINKA01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBMX01.FileName, "BMX_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBOBCAT01.FileName, "BOBCAT_01", manufacturer.MANUFACTURERVAPID01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBOBCATXL01.FileName, "BOBCAT_XL_01", manufacturer.MANUFACTURERVAPID01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBODHI01.FileName, "BODHI_01", manufacturer.MANUFACTURERCANIS01.FileName) );
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBOXVILLE01.FileName, "BOXVILLE_01", manufacturer.MANUFACTURERBRUTE01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBUCCANEER01.FileName, "BUCCANEER_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBUFFALO01.FileName, "BUFFALO_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBULLDOZER01.FileName, "BULLDOZER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBULLET01.FileName, "BULLET_01", manufacturer.MANUFACTURERVAPID01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBULLETGT01.FileName, "BULLET_GT_01", manufacturer.MANUFACTURERVAPID01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBURRITO01.FileName, "BURRITO_01", manufacturer.MANUFACTURERDECLASSE01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBUS01.FileName, "BUS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELBUZZARD01.FileName, "BUZZARD_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCADDY01.FileName, "CADDY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCAMPER01.FileName, "CAMPER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCARBONIZZARE01.FileName, "CARBONIZZARE_01", manufacturer.MANUFACTURERGROTTI01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCARBONRS01.FileName, "CARBON_RS_01", manufacturer.MANUFACTURERNAGASAKI01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCARGOBOB01.FileName, "CARGOBOB_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCARGOPLANE01.FileName, "CARGO_PLANE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCARGOPLANE02.FileName, "CARGO_PLANE_02"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCAVALCADE01.FileName, "CAVALCADE_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCEMENTMIXER01.FileName, "CEMENT_MIXER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCHEETAH01.FileName, "CHEETAH_01", manufacturer.MANUFACTURERGROTTI01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOACH01.FileName, "COACH_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOGNOSCENTI01.FileName, "COGNOSCENTI_01", manufacturer.MANUFACTURERENUS01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOG5501.FileName, "COG_55_01", manufacturer.MANUFACTURERENUS01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOGCABRIO01.FileName, "COG_CABRIO_01", manufacturer.MANUFACTURERENUS01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOMET01.FileName, "COMET_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCOQUETTE01.FileName, "COQUETTE_01", manufacturer.MANUFACTURERINVETERO01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCRUISER01.FileName, "CRUISER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCRUSADER01.FileName, "CRUSADER_01", manufacturer.MANUFACTURERCANIS01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCUBAN80001.FileName, "CUBAN_800_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELCUTTER01.FileName, "CUTTER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDAEMON01.FileName, "DAEMON_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDIGGER01.FileName, "DIGGER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDILETTANTE01.FileName, "DILETTANTE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDINGHY01.FileName, "DINGHY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDOCKTUG01.FileName, "DOCK_TUG_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDOMINATOR01.FileName, "DOMINATOR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDOUBLE01.FileName, "DOUBLE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDOUBLET01.FileName, "DOUBLE_T_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUBSTA01.FileName, "DUBSTA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUKES01.FileName, "DUKES_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUMPER01.FileName, "DUMPER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUMP01.FileName, "DUMP_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUNELOADER01.FileName, "DUNELOADER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUNE01.FileName, "DUNE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUNEBUGGY01.FileName, "DUNE_BUGGY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELDUSTER01.FileName, "DUSTER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELELEGY01.FileName, "ELEGY_01",manufacturer.MANUFACTURERANNIS01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELEMPEROR01.FileName, "EMPEROR_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELENTITYXF01.FileName, "ENTITY_XF_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELEOD01.FileName, "EOD_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELEXEMPLAR01.FileName, "EXEMPLAR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELF62001.FileName, "F620_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFACTION01.FileName, "FACTION_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFAGGIO01.FileName, "FAGGIO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFELON01.FileName, "FELON_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFELTZER01.FileName, "FELTZER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFEROCI01.FileName, "FEROCI_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFIELDMASTER01.FileName, "FIELDMASTER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFIRETRUCK01.FileName, "FIRETRUCK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFLATBED01.FileName, "FLATBED_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFORKLIFT01.FileName, "FORKLIFT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFQ201.FileName, "FQ2_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFREIGHT01.FileName, "FREIGHT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFROGGER01.FileName, "FROGGER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFUGITIVE01.FileName, "FUGITIVE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFUSILADE01.FileName, "FUSILADE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELFUTO01.FileName, "FUTO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELGAUNTLET01.FileName, "GAUNTLET_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELGRANGER01.FileName, "GRANGER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELGRESLEY01.FileName, "GRESLEY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHABANERO01.FileName, "HABANERO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHAKUMAI01.FileName, "HAKUMAI_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHANDLER01.FileName, "HANDLER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHAULER01.FileName, "HAULER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHEARSE01.FileName, "HEARSE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHELLFURY01.FileName, "HELLFURY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHEXER01.FileName, "HEXER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHOTKNIFE01.FileName, "HOT_KNIFE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELHUNTER01.FileName, "HUNTER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELINFERNUS01.FileName, "INFERNUS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELINGOT01.FileName, "INGOT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELINTRUDER01.FileName, "INTRUDER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELISSI01.FileName, "ISSI_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELJACKAL01.FileName, "JACKAL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELJB70001.FileName, "JB700_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELJETMAX01.FileName, "JETMAX_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELJET01.FileName, "JET_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELJOURNEY01.FileName, "JOURNEY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELKHAMELION01.FileName, "KHAMELION_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELLANDSTALKER01.FileName, "LANDSTALKER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELLAZER01.FileName, "LAZER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELLIFEGUARDGRANGER01.FileName, "LIFEGUARD_GRANGER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMANANA01.FileName, "MANANA_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMAVERICK01.FileName, "MAVERICK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMESA01.FileName, "MESA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMETROTRAIN01.FileName, "METROTRAIN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMINIVAN01.FileName, "MINIVAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMIXER01.FileName, "MIXER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMONROE01.FileName, "MONROE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMOWER01.FileName, "MOWER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELMULE01.FileName, "MULE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELNEMESIS01.FileName, "NEMESIS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELNINEF01.FileName, "NINE_F_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELORACLE01.FileName, "ORACLE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPACKER01.FileName, "PACKER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPATRIOT01.FileName, "PATRIOT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPCJ60001.FileName, "PCJ_600_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPENUMBRA01.FileName, "PENUMBRA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPEYOTE01.FileName, "PEYOTE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPHANTOM01.FileName, "PHANTOM_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPHOENIX01.FileName, "PHOENIX_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPICADOR01.FileName, "PICADOR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPOLICECAR01.FileName, "POLICE_CAR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPOLICEFUGITIVE01.FileName, "POLICE_FUGITIVE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPOLICEMAVERICK01.FileName, "POLICE_MAVERICK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPOLICETRANSPORT01.FileName, "POLICE_TRANSPORT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPONY01.FileName, "PONY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPOUNDER01.FileName, "POUNDER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPRAIRIE01.FileName, "PRAIRIE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPREDATOR01.FileName, "PREDATOR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPREMIER01.FileName, "PREMIER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELPRIMO01.FileName, "PRIMO_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRADIUS01.FileName, "RADIUS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRADI01.FileName, "RADI_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRANCHERXL01.FileName, "RANCHER_XL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRAPIDGT01.FileName, "RAPID_GT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRATLOADER01.FileName, "RATLOADER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRCBANDITO01.FileName, "RC_BANDITO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELREBEL01.FileName, "REBEL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELREGINA01.FileName, "REGINA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRHINO01.FileName, "RHINO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRIDEONMOWER01.FileName, "RIDE_ON_MOWER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRIOT01.FileName, "RIOT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRIPLEY01.FileName, "RIPLEY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELROCOTO01.FileName, "ROCOTO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRUBBLE01.FileName, "RUBBLE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRUFFIAN01.FileName, "RUFFIAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRUINER01.FileName, "RUINER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELRUMPO01.FileName, "RUMPO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSABERGT01.FileName, "SABER_GT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSABREGT01.FileName, "SABREGT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSADLER01.FileName, "SADLER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSANCHEZ01.FileName, "SANCHEZ_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSANDKING01.FileName, "SANDKING_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSCHAFTER01.FileName, "SCHAFTER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSCHWARZER01.FileName, "SCHWARZER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSCORCHER01.FileName, "SCORCHER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSCRAP01.FileName, "SCRAP_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSEAPLANE01.FileName, "SEAPLANE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSEASHARK01.FileName, "SEASHARK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSEMINOLE01.FileName, "SEMINOLE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSENTINEL01.FileName, "SENTINEL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSERRANO01.FileName, "SERRANO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSKIVVY01.FileName, "SKIVVY_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSKYLIFT01.FileName, "SKYLIFT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSLAMVAN01.FileName, "SLAMVAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSPEEDER01.FileName, "SPEEDER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSPEEDO01.FileName, "SPEEDO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSQUADDIE01.FileName, "SQUADDIE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSQUALO01.FileName, "SQUALO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTANIER01.FileName, "STANIER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTINGER01.FileName, "STINGER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTINGERGT01.FileName, "STINGER_GT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTOCKADE01.FileName, "STOCKADE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTRATUM01.FileName, "STRATUM_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTRETCH01.FileName, "STRETCH_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSTUNT01.FileName, "STUNT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSUBMARINE01.FileName, "SUBMARINE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSUBMERSIBLE01.FileName, "SUBMERSIBLE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSULTAN01.FileName, "SULTAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSUNTRAP01.FileName, "SUNTRAP_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSUPERDIAMOND01.FileName, "SUPER_DIAMOND_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSURANO01.FileName, "SURANO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSURFER01.FileName, "SURFER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSURGE01.FileName, "SURGE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELSXR01.FileName, "SXR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTACO01.FileName, "TACO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTACOVAN01.FileName, "TACO_VAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTAILGATER01.FileName, "TAILGATER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTAMPA01.FileName, "TAMPA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTAXI01.FileName, "TAXI_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTIPPER01.FileName, "TIPPER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTIPTRUCK01.FileName, "TIPTRUCK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTITAN01.FileName, "TITAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTORNADO01.FileName, "TORNADO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTOURBUS01.FileName, "TOUR_BUS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTOWTRUCK01.FileName, "TOWTRUCK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTR301.FileName, "TR3_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTRACTOR01.FileName, "TRACTOR_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTRASH01.FileName, "TRASH_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTRIALS01.FileName, "TRIALS_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTRIBIKE01.FileName, "TRIBIKE_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTROPIC01.FileName, "TROPIC_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELTUGBOAT01.FileName, "TUG_BOAT_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELUTILITYTRUCK01.FileName, "UTILITY_TRUCK_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVACCA01.FileName, "VACCA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVADER01.FileName, "VADER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVIGERO01.FileName, "VIGERO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVOLTIC01.FileName, "VOLTIC_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVOODOO01.FileName, "VOODOO_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELVULKAN01.FileName, "VULKAN_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELWASHINGTON01.FileName, "WASHINGTON_01", manufacturer.MANUFACTURERALBANY01.FileName));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELWAYFARER01.FileName, "WAYFARER_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELWILLARD01.FileName, "WILLARD_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELXL01.FileName, "XL_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELYOUGA01.FileName, "YOUGA_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELZION01.FileName, "ZION_01"));
        ModelNameLookup.Add(new VehicleModelNameLookup(ScannerAudio.model.MODELZTYPE01.FileName, "ZTYPE_01"));


}

    public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {
                    GameFiber.Yield();
                }
            });
        }
    private static void PlayAudio(String _Audio)
    {
        try
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\scanner\\{0}", _Audio));
                audioFile.Volume = Settings.DispatchAudioVolume;
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
        catch(Exception e)
        {
            Game.Console.Print(e.Message);
        }
    }
    private static void PlayAudioList(List<String> SoundsToPlay,bool CheckSight)
    {
        GameFiber.Sleep(rnd.Next(250, 670));
        if (CheckSight && !PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;

        GameFiber.StartNew(delegate
        {            
            while (outputDevice != null)
                GameFiber.Yield();
            foreach (String audioname in SoundsToPlay)
            {
                PlayAudio(audioname);
                while (outputDevice != null)
                    GameFiber.Yield();
            }
        });
    }
    private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        audioFile.Dispose();
        audioFile = null;
    }
    private static void ReportGenericStart(List<string> myList)
    {
        if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;
        myList.Add(ScannerAudio.AudioBeeps.AudioStart());
        myList.Add(ScannerAudio.we_have.OfficersReport());
    }
    private static void ReportGenericEnd(List<string> myList,bool Near)
    {
        if(Near)
        {
            Vector3 Pos = Game.LocalPlayer.Character.Position;
            Zone MyZone = Zones.GetZoneName(Pos);
            if (MyZone != null)
            {
                myList.Add(ScannerAudio.conjunctives.NearGenericRandom());
                myList.Add(MyZone.ScannerValue);
            }
        }
        myList.Add(ScannerAudio.AudioBeeps.Radio_End_1.FileName);
    }
    public static void ReportShotsFired()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        ReportGenericEnd(ScannerList, true);
        ReportedShotsFired = true;
        PlayAudioList(ScannerList, true);
        
    }
    public static void ReportCarryingWeapon()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || ReportedAssaultOnOfficer || InstantAction.isBusted || InstantAction.isDead)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 5);
        if(Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithafirearm.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithagat.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingafirearm.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingagat.FileName);
        }
        
        ReportGenericEnd(ScannerList, true);
        ReportedCarryingWeapon = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportOfficerDown()
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AcriticalsituationOfficerdown.FileName);
        }
        else if (Num == 2)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AnofferdownpossiblyKIA.FileName);
        }
        else if (Num == 3)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdown.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdownconditionunknown.FileName);
        }

        int Num2 = rnd.Next(1, 5);
        if (Num2 == 1)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.AllunitsrespondCode99.FileName);
        }
        else if (Num2 == 2)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.AllunitsrespondCode99emergency.FileName);
        }
        else if (Num2 == 3)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.Code99allunitsrespond.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.EmergencyallunitsrespondCode99.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        ReportedOfficerDown = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportAssualtOnOfficer()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault.Apossibleassault1.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault.Apossibleassault.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault_on_an_officer.Anassaultonanofficer.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault_on_an_officer.Anofficerassault.FileName);
        }
        ReportGenericEnd(ScannerList, true);
        ReportedAssaultOnOfficer = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportThreateningWithFirearm()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
        ScannerList.Add(ScannerAudio.crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        ReportGenericEnd(ScannerList, true);
        ReportedThreateningWithAFirearm = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportSuspectLastSeen(bool Near)
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastReported.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastSeen.FileName);
        }
        
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSuspectArrested()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart()
        }) ;

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_arrested.Asuspectincustody1.FileName);
        }
        else if (Num == 2)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_arrested.Asuspectapprehended.FileName);
        }
        else if (Num == 3)
        {
            myList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect.FileName);
        }
        else
        {
            myList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect1.FileName);
        }

        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList,true);
    }
    public static void ReportSuspectWasted()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart()
            
        });

        int Num = rnd.Next(1, 11);
        if (Num == 1)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Acriminaldown.FileName);
        }
        else if (Num == 2)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown.FileName);
        }
        else if (Num == 3)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown2.FileName);
        }
        else if (Num == 4)
        {
            myList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown1.FileName);
        }
        else if (Num == 5)
        {
            myList.Add(ScannerAudio.crook_killed.Criminaldown.FileName);
        }
        else if (Num == 6)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdown.FileName);
        }
        else if (Num == 7)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectneutralized.FileName);
        }
        else if (Num == 8)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdownmedicalexaminerenroute.FileName);
        }
        else if (Num == 9)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdowncoronerenroute.FileName);
        }
        else
        {
            myList.Add(ScannerAudio.crook_killed.Officershavepacifiedsuspect.FileName);
        }



        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList, true);
    }
    public static void ReportLethalForceAuthorized()
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        // ReportGenericStart(ScannerList);

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());
        ScannerList.Add(ScannerAudio.attention_all_units_gen.Allunits.FileName);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Allunits.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceauthorized.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits1.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized1.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits2.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useoflethalforceisauthorized.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits3.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforcepermitted1.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportStolenVehicle(StolenVehicle stolenVehicle)
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());
        ScannerList.Add(ScannerAudio.attention_all_units_gen.Allunits.FileName);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.we_have.CitizensReport_1.FileName);
            ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.we_have.CitizensReport_2.FileName);
            ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.we_have.CitizensReport_3.FileName);
            ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.we_have.CitizensReport_4.FileName);
            ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.CitizensReport_2.FileName);
            ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }


        var output = Regex.Replace(stolenVehicle.VehicleEnt.Model.Name.ToUpper(), @"[\d-]", string.Empty);
        VehicleModelNameLookup LookupModel = ModelNameLookup.Where(x => x.ModelName.Contains(output)).PickRandom();

        Color BaseColor = GetBaseColor(stolenVehicle.VehicleEnt.PrimaryColor);
        ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();

        if (LookupModel != null && LookupColor != null)
        {
            ScannerList.Add(conjunctives.A01.FileName);
            ScannerList.Add(LookupColor.ScannerFile);
            if(LookupModel.ManufacturerScannerFile != "")
                ScannerList.Add(LookupModel.ManufacturerScannerFile);
            ScannerList.Add(LookupModel.ScannerFile);
        }

        ScannerList.Add(ScannerAudio.suspect_license_plate.SuspectsLicensePlate.FileName); //

        foreach (char c in stolenVehicle.VehicleEnt.LicensePlate)
        {
            string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
            ScannerList.Add(DispatchFileName);
        }




        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
        //GameFiber.Sleep(2500);
        GameFiber.Sleep(20000);

        stolenVehicle.WasReportedStolen = true;
        InstantAction.WriteToLog("StolenVehicles", String.Format("Vehicle {0} was just reported stolen",stolenVehicle.VehicleEnt.Handle));
    }
    public static void ReportSuspectLost()
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        // ReportGenericStart(ScannerList);

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStandby.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ResetReportedItems()
    {
        ReportedAssaultOnOfficer = false;
        ReportedCarryingWeapon = false;
        ReportedLethalForceAuthorized = false;
        ReportedOfficerDown = false;
        ReportedShotsFired = false;
        ReportedThreateningWithAFirearm = false;
    }
    public static Color GetBaseColor(Color PrimaryColor)
    {
        List<Color> BaseColorList = new List<Color>();
        BaseColorList.Add(Color.Red);
        BaseColorList.Add(Color.Aqua);
        BaseColorList.Add(Color.Beige);
        BaseColorList.Add(Color.Black);
        BaseColorList.Add(Color.Blue);
        BaseColorList.Add(Color.Brown);
        BaseColorList.Add(Color.DarkBlue);
        BaseColorList.Add(Color.DarkGreen);
        BaseColorList.Add(Color.DarkGray);
        BaseColorList.Add(Color.DarkOrange);
        BaseColorList.Add(Color.DarkRed);
        BaseColorList.Add(Color.Gold);
        BaseColorList.Add(Color.Green);
        BaseColorList.Add(Color.Gray);
        BaseColorList.Add(Color.LightBlue);
        BaseColorList.Add(Color.Maroon);
        BaseColorList.Add(Color.Orange);
        BaseColorList.Add(Color.Pink);
        BaseColorList.Add(Color.Purple);
        BaseColorList.Add(Color.Silver);
        BaseColorList.Add(Color.White);
        BaseColorList.Add(Color.Yellow);

        Color MyColor = Game.LocalPlayer.Character.CurrentVehicle.PrimaryColor;

        int Index = Extensions.closestColor2(BaseColorList, MyColor);

        return BaseColorList[Index];
    }

    public class DispatchLettersNumber
    {
        public char AlphaNumeric { get; set; }
        public string ScannerFile { get; set; }

        public DispatchLettersNumber(char _AlphaNumeric,string _ScannerFile)
        {
            AlphaNumeric = _AlphaNumeric;
            ScannerFile = _ScannerFile;
        }

    }

    public class VehicleModelNameLookup
    {
        public string ModelName { get; set; }
        public string ScannerFile { get; set; }
        public string ManufacturerScannerFile { get; set; } = "";

        public VehicleModelNameLookup(string _ScannerFile, string _ModelName)
        {
            ModelName = _ModelName;
            ScannerFile = _ScannerFile;
        }
        public VehicleModelNameLookup(string _ScannerFile, string _ModelName, string _ManufacturerScannerFile)
        {
            ModelName = _ModelName;
            ScannerFile = _ScannerFile;
            ManufacturerScannerFile = _ManufacturerScannerFile;
        }

    }
    public class ColorLookup
    {
        public Color BaseColor { get; set; }
        public string ScannerFile { get; set; }

        public ColorLookup(string _ScannerFile, Color _BaseColor)
        {
            BaseColor = _BaseColor;
            ScannerFile = _ScannerFile;
        }

    }
}

