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
using static VehicleLookup;
using static Zones;

internal static class DispatchAudioSystem
{
    private static WaveOutEvent outputDevice;
    private static AudioFileReader audioFile;
    private static Random rnd;
    private static bool ExecutingQueue = false;

    public static bool ReportedOfficerDown { get; set; } = false;
    public static bool ReportedShotsFired { get; set; } = false;
    public static bool ReportedAssaultOnOfficer { get; set; } = false;
    public static bool ReportedCarryingWeapon { get; set; } = false;
    public static bool ReportedLethalForceAuthorized { get; set; } = false;
    public static bool ReportedThreateningWithAFirearm { get; set; } = false;
    public static List<DispatchQueueItem> DispatchQueue { get; set; } = new List<DispatchQueueItem>();

    private static List<DispatchLettersNumber> LettersAndNumbersLookup = new List<DispatchLettersNumber>();
    public static List<VehicleModelNameLookup> ModelNameLookup = new List<VehicleModelNameLookup>();
    public static List<ColorLookup> ColorLookups = new List<ColorLookup>();

    public static List<string> DamagedScannerAliases = new List<string>();
    public static bool IsRunning { get; set; } = true;
    public enum ReportDispatch
    {
        ReportShotsFired = 0,
        ReportCarryingWeapon = 1,
        ReportOfficerDown = 2,
        ReportAssualtOnOfficer = 3,
        ReportThreateningWithFirearm = 4,
        ReportSuspectLastSeen = 5,
        ReportSuspectArrested = 6,
        ReportSuspectWasted = 7,
        ReportLethalForceAuthorized = 8,
        ReportStolenVehicle = 9,
        ReportSuspectLost = 10,
        ReportSpottedStolenCar = 11,
        ReportPedHitAndRun = 12,
        ReportVehicleHitAndRun = 13,
        ReportRecklessDriver = 14,
        ReportFelonySpeeding = 15,
        ReportWeaponsFree = 16,
        ReportSuspiciousActivity = 17,
        ReportSuspiciousVehicle = 18,
        ReportGrandTheftAuto = 19,
    }
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
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', lp_letters_high.Adam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', lp_letters_high.Boy.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', lp_letters_high.Charles.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('D', lp_letters_high.David.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', lp_letters_high.Edward.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', lp_letters_high.Frank.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', lp_letters_high.George.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', lp_letters_high.Henry.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', lp_letters_high.Ita.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', lp_letters_high.John.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', lp_letters_high.King.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', lp_letters_high.Lincoln.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', lp_letters_high.Mary.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', lp_letters_high.Nora.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', lp_letters_high.Ocean.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', lp_letters_high.Paul.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', lp_letters_high.Queen.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', lp_letters_high.Robert.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', lp_letters_high.Sam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', lp_letters_high.Tom.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', lp_letters_high.Union.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', lp_letters_high.Victor.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', lp_letters_high.William.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', lp_letters_high.XRay.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', lp_letters_high.Young.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', lp_letters_high.Zebra.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', lp_letters_high.Adam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', lp_letters_high.Boy1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', lp_letters_high.Charles1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', lp_letters_high.Edward1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', lp_letters_high.Frank1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', lp_letters_high.George1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', lp_letters_high.Henry1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', lp_letters_high.Ita1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', lp_letters_high.John1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', lp_letters_high.King1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', lp_letters_high.Lincoln1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', lp_letters_high.Mary1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', lp_letters_high.Nora1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', lp_letters_high.Ocean1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', lp_letters_high.Paul1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', lp_letters_high.Queen1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', lp_letters_high.Robert1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', lp_letters_high.Sam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', lp_letters_high.Tom1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', lp_letters_high.Union1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', lp_letters_high.Victor1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', lp_letters_high.William1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', lp_letters_high.XRay1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', lp_letters_high.Young1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', lp_letters_high.Zebra1.FileName));



        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Nine.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Niner.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero1.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Niner2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero2.FileName));

        ColorLookups.Add(new ColorLookup(colour.COLORRED01.FileName, Color.Red));
        ColorLookups.Add(new ColorLookup(colour.COLORAQUA01.FileName, Color.Aqua));
        ColorLookups.Add(new ColorLookup(colour.COLORBEIGE01.FileName, Color.Beige));
        ColorLookups.Add(new ColorLookup(colour.COLORBLACK01.FileName, Color.Black));
        ColorLookups.Add(new ColorLookup(colour.COLORBLUE01.FileName, Color.Blue));
        ColorLookups.Add(new ColorLookup(colour.COLORBROWN01.FileName, Color.Brown));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKBLUE01.FileName, Color.DarkBlue));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKGREEN01.FileName, Color.DarkGreen));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKGREY01.FileName, Color.DarkGray));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKORANGE01.FileName, Color.DarkOrange));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKRED01.FileName, Color.DarkRed));
        ColorLookups.Add(new ColorLookup(colour.COLORGOLD01.FileName, Color.Gold));
        ColorLookups.Add(new ColorLookup(colour.COLORGREEN01.FileName, Color.Green));
        ColorLookups.Add(new ColorLookup(colour.COLORGREY01.FileName, Color.Gray));
        ColorLookups.Add(new ColorLookup(colour.COLORGREY02.FileName, Color.Gray));
        ColorLookups.Add(new ColorLookup(colour.COLORLIGHTBLUE01.FileName, Color.LightBlue));
        ColorLookups.Add(new ColorLookup(colour.COLORMAROON01.FileName, Color.Maroon));
        ColorLookups.Add(new ColorLookup(colour.COLORORANGE01.FileName, Color.Orange));
        ColorLookups.Add(new ColorLookup(colour.COLORPINK01.FileName, Color.Pink));
        ColorLookups.Add(new ColorLookup(colour.COLORPURPLE01.FileName, Color.Purple));
        ColorLookups.Add(new ColorLookup(colour.COLORRED01.FileName, Color.Red));
        ColorLookups.Add(new ColorLookup(colour.COLORSILVER01.FileName, Color.Silver));
        ColorLookups.Add(new ColorLookup(colour.COLORWHITE01.FileName, Color.White));
        ColorLookups.Add(new ColorLookup(colour.COLORYELLOW01.FileName, Color.Yellow));



        DamagedScannerAliases.Add(extra_prefix.Battered.FileName);
        DamagedScannerAliases.Add(extra_prefix.Beatup.FileName);
        DamagedScannerAliases.Add(extra_prefix.Damaged.FileName);
        DamagedScannerAliases.Add(extra_prefix.Dented.FileName);
        DamagedScannerAliases.Add(extra_prefix.Distressed.FileName);
        DamagedScannerAliases.Add(extra_prefix.Rundown.FileName);
        DamagedScannerAliases.Add(extra_prefix.Rundown1.FileName);

        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELADDER01.FileName, "ADDER_01", manufacturer.MANUFACTURERTRUFFADE01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELAIRSHIP01.FileName, "AIRSHIP_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELAIRTUG01.FileName, "AIRTUG_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELAKUMA01.FileName, "AKUMA_01", manufacturer.MANUFACTURERDINKA01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELAMBULANCE01.FileName, "AMBULANCE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELANNIHILATOR01.FileName, "ANNIHILATOR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELASEA01.FileName, "ASEA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELASTEROPE01.FileName, "ASTEROPE_01", manufacturer.MANUFACTURERKARIN01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBAGGER01.FileName, "BAGGER_01", manufacturer.MANUFACTURERWESTERNMOTORCYCLECOMPANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBALLER01.FileName, "BALLER_01", manufacturer.MANUFACTURERGALLIVANTER01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBANSHEE01.FileName, "BANSHEE_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBARRACKS01.FileName, "BARRACKS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBATI01.FileName, "BATI_01", manufacturer.MANUFACTURERPEGASI01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBENSON01.FileName, "BENSON_01", manufacturer.MANUFACTURERVAPID01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBFINJECTION01.FileName, "BF_INJECTION_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBIFF01.FileName, "BIFF_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBISON01.FileName, "BISON_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBJXL01.FileName, "BJXL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBLAZER01.FileName, "BLAZER_01", manufacturer.MANUFACTURERNAGASAKI01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBLIMP01.FileName, "BLIMP_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBLISTA01.FileName, "BLISTA_01", manufacturer.MANUFACTURERDINKA01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBMX01.FileName, "BMX_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBOBCAT01.FileName, "BOBCAT_01", manufacturer.MANUFACTURERVAPID01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBOBCATXL01.FileName, "BOBCAT_XL_01", manufacturer.MANUFACTURERVAPID01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBODHI01.FileName, "BODHI_01", manufacturer.MANUFACTURERCANIS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBOXVILLE01.FileName, "BOXVILLE_01", manufacturer.MANUFACTURERBRUTE01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBUCCANEER01.FileName, "BUCCANEER_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBUFFALO01.FileName, "BUFFALO_01", manufacturer.MANUFACTURERBRAVADO01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBULLDOZER01.FileName, "BULLDOZER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBULLET01.FileName, "BULLET_01", manufacturer.MANUFACTURERVAPID01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBULLETGT01.FileName, "BULLET_GT_01", manufacturer.MANUFACTURERVAPID01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBURRITO01.FileName, "BURRITO_01", manufacturer.MANUFACTURERDECLASSE01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBUS01.FileName, "BUS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELBUZZARD01.FileName, "BUZZARD_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCADDY01.FileName, "CADDY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCAMPER01.FileName, "CAMPER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCARBONIZZARE01.FileName, "CARBONIZZARE_01", manufacturer.MANUFACTURERGROTTI01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCARBONRS01.FileName, "CARBON_RS_01", manufacturer.MANUFACTURERNAGASAKI01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCARGOBOB01.FileName, "CARGOBOB_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCARGOPLANE01.FileName, "CARGO_PLANE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCARGOPLANE02.FileName, "CARGO_PLANE_02"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCAVALCADE01.FileName, "CAVALCADE_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCEMENTMIXER01.FileName, "CEMENT_MIXER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCHEETAH01.FileName, "CHEETAH_01", manufacturer.MANUFACTURERGROTTI01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOACH01.FileName, "COACH_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOGNOSCENTI01.FileName, "COGNOSCENTI_01", manufacturer.MANUFACTURERENUS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOG5501.FileName, "COG_55_01", manufacturer.MANUFACTURERENUS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOGCABRIO01.FileName, "COG_CABRIO_01", manufacturer.MANUFACTURERENUS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOMET01.FileName, "COMET_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCOQUETTE01.FileName, "COQUETTE_01", manufacturer.MANUFACTURERINVETERO01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCRUISER01.FileName, "CRUISER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCRUSADER01.FileName, "CRUSADER_01", manufacturer.MANUFACTURERCANIS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCUBAN80001.FileName, "CUBAN_800_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELCUTTER01.FileName, "CUTTER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDAEMON01.FileName, "DAEMON_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDIGGER01.FileName, "DIGGER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDILETTANTE01.FileName, "DILETTANTE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDINGHY01.FileName, "DINGHY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDOCKTUG01.FileName, "DOCK_TUG_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDOMINATOR01.FileName, "DOMINATOR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDOUBLE01.FileName, "DOUBLE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDOUBLET01.FileName, "DOUBLE_T_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUBSTA01.FileName, "DUBSTA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUKES01.FileName, "DUKES_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUMPER01.FileName, "DUMPER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUMP01.FileName, "DUMP_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUNELOADER01.FileName, "DUNELOADER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUNE01.FileName, "DUNE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUNEBUGGY01.FileName, "DUNE_BUGGY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELDUSTER01.FileName, "DUSTER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELELEGY01.FileName, "ELEGY_01", manufacturer.MANUFACTURERANNIS01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELEMPEROR01.FileName, "EMPEROR_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELENTITYXF01.FileName, "ENTITY_XF_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELEOD01.FileName, "EOD_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELEXEMPLAR01.FileName, "EXEMPLAR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELF62001.FileName, "F620_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFACTION01.FileName, "FACTION_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFAGGIO01.FileName, "FAGGIO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFELON01.FileName, "FELON_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFELTZER01.FileName, "FELTZER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFEROCI01.FileName, "FEROCI_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFIELDMASTER01.FileName, "FIELDMASTER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFIRETRUCK01.FileName, "FIRETRUCK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFLATBED01.FileName, "FLATBED_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFORKLIFT01.FileName, "FORKLIFT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFQ201.FileName, "FQ2_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFREIGHT01.FileName, "FREIGHT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFROGGER01.FileName, "FROGGER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFUGITIVE01.FileName, "FUGITIVE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFUSILADE01.FileName, "FUSILADE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELFUTO01.FileName, "FUTO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELGAUNTLET01.FileName, "GAUNTLET_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELGRANGER01.FileName, "GRANGER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELGRESLEY01.FileName, "GRESLEY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHABANERO01.FileName, "HABANERO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHAKUMAI01.FileName, "HAKUMAI_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHANDLER01.FileName, "HANDLER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHAULER01.FileName, "HAULER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHEARSE01.FileName, "HEARSE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHELLFURY01.FileName, "HELLFURY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHEXER01.FileName, "HEXER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHOTKNIFE01.FileName, "HOT_KNIFE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELHUNTER01.FileName, "HUNTER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELINFERNUS01.FileName, "INFERNUS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELINGOT01.FileName, "INGOT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELINTRUDER01.FileName, "INTRUDER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELISSI01.FileName, "ISSI_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELJACKAL01.FileName, "JACKAL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELJB70001.FileName, "JB700_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELJETMAX01.FileName, "JETMAX_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELJET01.FileName, "JET_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELJOURNEY01.FileName, "JOURNEY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELKHAMELION01.FileName, "KHAMELION_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELLANDSTALKER01.FileName, "LANDSTALKER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELLAZER01.FileName, "LAZER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELLIFEGUARDGRANGER01.FileName, "LIFEGUARD_GRANGER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMANANA01.FileName, "MANANA_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMAVERICK01.FileName, "MAVERICK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMESA01.FileName, "MESA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMETROTRAIN01.FileName, "METROTRAIN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMINIVAN01.FileName, "MINIVAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMIXER01.FileName, "MIXER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMONROE01.FileName, "MONROE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMOWER01.FileName, "MOWER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELMULE01.FileName, "MULE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELNEMESIS01.FileName, "NEMESIS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELNINEF01.FileName, "NINE_F_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELORACLE01.FileName, "ORACLE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPACKER01.FileName, "PACKER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPATRIOT01.FileName, "PATRIOT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPCJ60001.FileName, "PCJ_600_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPENUMBRA01.FileName, "PENUMBRA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPEYOTE01.FileName, "PEYOTE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPHANTOM01.FileName, "PHANTOM_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPHOENIX01.FileName, "PHOENIX_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPICADOR01.FileName, "PICADOR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPOLICECAR01.FileName, "POLICE_CAR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPOLICEFUGITIVE01.FileName, "POLICE_FUGITIVE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPOLICEMAVERICK01.FileName, "POLICE_MAVERICK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPOLICETRANSPORT01.FileName, "POLICE_TRANSPORT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPONY01.FileName, "PONY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPOUNDER01.FileName, "POUNDER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPRAIRIE01.FileName, "PRAIRIE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPREDATOR01.FileName, "PREDATOR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPREMIER01.FileName, "PREMIER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELPRIMO01.FileName, "PRIMO_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRADIUS01.FileName, "RADIUS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRADI01.FileName, "RADI_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRANCHERXL01.FileName, "RANCHER_XL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRAPIDGT01.FileName, "RAPID_GT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRATLOADER01.FileName, "RATLOADER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRCBANDITO01.FileName, "RC_BANDITO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELREBEL01.FileName, "REBEL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELREGINA01.FileName, "REGINA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRHINO01.FileName, "RHINO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRIDEONMOWER01.FileName, "RIDE_ON_MOWER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRIOT01.FileName, "RIOT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRIPLEY01.FileName, "RIPLEY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELROCOTO01.FileName, "ROCOTO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRUBBLE01.FileName, "RUBBLE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRUFFIAN01.FileName, "RUFFIAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRUINER01.FileName, "RUINER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELRUMPO01.FileName, "RUMPO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSABERGT01.FileName, "SABER_GT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSABREGT01.FileName, "SABREGT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSADLER01.FileName, "SADLER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSANCHEZ01.FileName, "SANCHEZ_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSANDKING01.FileName, "SANDKING_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSCHAFTER01.FileName, "SCHAFTER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSCHWARZER01.FileName, "SCHWARZER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSCORCHER01.FileName, "SCORCHER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSCRAP01.FileName, "SCRAP_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSEAPLANE01.FileName, "SEAPLANE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSEASHARK01.FileName, "SEASHARK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSEMINOLE01.FileName, "SEMINOLE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSENTINEL01.FileName, "SENTINEL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSERRANO01.FileName, "SERRANO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSKIVVY01.FileName, "SKIVVY_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSKYLIFT01.FileName, "SKYLIFT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSLAMVAN01.FileName, "SLAMVAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSPEEDER01.FileName, "SPEEDER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSPEEDO01.FileName, "SPEEDO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSQUADDIE01.FileName, "SQUADDIE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSQUALO01.FileName, "SQUALO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTANIER01.FileName, "STANIER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTINGER01.FileName, "STINGER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTINGERGT01.FileName, "STINGER_GT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTOCKADE01.FileName, "STOCKADE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTRATUM01.FileName, "STRATUM_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTRETCH01.FileName, "STRETCH_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSTUNT01.FileName, "STUNT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSUBMARINE01.FileName, "SUBMARINE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSUBMERSIBLE01.FileName, "SUBMERSIBLE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSULTAN01.FileName, "SULTAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSUNTRAP01.FileName, "SUNTRAP_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSUPERDIAMOND01.FileName, "SUPER_DIAMOND_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSURANO01.FileName, "SURANO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSURFER01.FileName, "SURFER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSURGE01.FileName, "SURGE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELSXR01.FileName, "SXR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTACO01.FileName, "TACO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTACOVAN01.FileName, "TACO_VAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTAILGATER01.FileName, "TAILGATER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTAMPA01.FileName, "TAMPA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTAXI01.FileName, "TAXI_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTIPPER01.FileName, "TIPPER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTIPTRUCK01.FileName, "TIPTRUCK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTITAN01.FileName, "TITAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTORNADO01.FileName, "TORNADO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTOURBUS01.FileName, "TOUR_BUS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTOWTRUCK01.FileName, "TOWTRUCK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTR301.FileName, "TR3_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTRACTOR01.FileName, "TRACTOR_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTRASH01.FileName, "TRASH_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTRIALS01.FileName, "TRIALS_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTRIBIKE01.FileName, "TRIBIKE_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTROPIC01.FileName, "TROPIC_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELTUGBOAT01.FileName, "TUG_BOAT_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELUTILITYTRUCK01.FileName, "UTILITY_TRUCK_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVACCA01.FileName, "VACCA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVADER01.FileName, "VADER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVIGERO01.FileName, "VIGERO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVOLTIC01.FileName, "VOLTIC_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVOODOO01.FileName, "VOODOO_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELVULKAN01.FileName, "VULKAN_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELWASHINGTON01.FileName, "WASHINGTON_01", manufacturer.MANUFACTURERALBANY01.FileName));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELWAYFARER01.FileName, "WAYFARER_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELWILLARD01.FileName, "WILLARD_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELXL01.FileName, "XL_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELYOUGA01.FileName, "YOUGA_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELZION01.FileName, "ZION_01"));
        //ModelNameLookup.Add(new VehicleModelNameLookup(model.MODELZTYPE01.FileName, "ZTYPE_01"));


    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                PlayDispatchQueue();
                GameFiber.Yield();
            }
        });
    }
    private static void PlayAudio(String _Audio)
    {
        try
        {
            if (_Audio == "")
                return;
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
        catch (Exception e)
        {
            Game.Console.Print(e.Message);
        }
    }
    private static void PlayAudioList(List<String> SoundsToPlay, bool CheckSight)
    {
        //GameFiber.Sleep(rnd.Next(250, 670));
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
    public static void AddDispatchToQueue(DispatchQueueItem _ItemToAdd)
    {
        if (!DispatchQueue.Any(x => x.Type == _ItemToAdd.Type))
            DispatchQueue.Add(_ItemToAdd);
    }
    private static void PlayDispatchQueue()
    {

        if (DispatchQueue.Count > 0 && !ExecutingQueue)
        {
            InstantAction.WriteToLog("PlayDispatchQueue", "Delegate Started");
            ExecutingQueue = true;
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(3000);

                // Remove and order items
                if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                {
                    foreach (DispatchQueueItem Item in DispatchQueue.Where(x => x.Priority > 3 && x.Type != ReportDispatch.ReportSuspectArrested && x.Type != ReportDispatch.ReportSuspectWasted))
                    {
                        InstantAction.WriteToLog("PlayDispatchQueue", string.Format("DeadlyChase: Removed {0}", Item.Type.ToString()));
                    }
                    
                    DispatchQueue.RemoveAll(x => x.Priority > 3 && x.Type != ReportDispatch.ReportSuspectArrested && x.Type != ReportDispatch.ReportSuspectWasted);
                    InstantAction.WriteToLog("PlayDispatchQueue", "DeadlyChase: Removed Some Low priority Items");
                }

                if(DispatchQueue.Any(x => x.Priority <= 1) && DispatchQueue.Any(x => x.Priority > 1 && x.Type == ReportDispatch.ReportLethalForceAuthorized))
                {
                    DispatchQueue.RemoveAll(x => x.Priority > 1);
                    InstantAction.WriteToLog("PlayDispatchQueue", "High Priority Message: Removed Some Low priority Items");
                }

                if (DispatchQueue.Any(x => x.ResultsInLethalForce))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportLethalForceAuthorized);
                    InstantAction.WriteToLog("PlayDispatchQueue", "ResultsInLethalForce: Removed ReportLethalForceAuthorized");
                }


                if (DispatchQueue.Any(x => x.ResultsInStolenCarSpotted && x.Type != ReportDispatch.ReportSpottedStolenCar))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportSpottedStolenCar);
                    InstantAction.WriteToLog("PlayDispatchQueue", "ResultsInStolenCarSpotted: Removed ReportSpottedStolenCar");
                }
                if (DispatchQueue.Any(x => x.ResultsInStolenCarSpotted && x.Type != ReportDispatch.ReportSuspiciousVehicle))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportSuspiciousVehicle);
                    InstantAction.WriteToLog("PlayDispatchQueue", "ResultsInStolenCarSpotted: Removed ReportSuspiciousVehicle");
                }

                if (DispatchQueue.Where(x => x.IsTrafficViolation).Count() > 1)
                {
                    DispatchQueueItem HighestItem = DispatchQueue.Where(x => x.IsTrafficViolation).OrderBy(x => x.Priority).FirstOrDefault();
                    DispatchQueue.RemoveAll(x => x.IsTrafficViolation);
                    if (HighestItem != null)
                    {
                        DispatchQueue.Add(HighestItem);
                    }
                    InstantAction.WriteToLog("PlayDispatchQueue", "IsTrafficViolation: Removed IsTrafficViolation except highest");
                }

                foreach (DispatchQueueItem Item in DispatchQueue)
                {
                    InstantAction.WriteToLog("PlayDispatchQueue", string.Format("Items To Play: {0}", Item.Type.ToString()));
                }


                foreach(DispatchQueueItem Item in DispatchQueue)
                {

                }

                while (DispatchQueue.Count > 0)
                {
                    DispatchQueueItem Item = DispatchQueue[0];
                    if (Item.Type == ReportDispatch.ReportAssualtOnOfficer)
                        ReportAssualtOnOfficer();
                    else if (Item.Type == ReportDispatch.ReportCarryingWeapon)
                        ReportCarryingWeapon(Item.WeaponToReport);
                    else if (Item.Type == ReportDispatch.ReportFelonySpeeding)
                        ReportFelonySpeeding(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportLethalForceAuthorized)
                        ReportLethalForceAuthorized();
                    else if (Item.Type == ReportDispatch.ReportOfficerDown)
                        ReportOfficerDown();
                    else if (Item.Type == ReportDispatch.ReportPedHitAndRun)
                        ReportPedHitAndRun(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportRecklessDriver)
                        ReportRecklessDriver(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportShotsFired)
                        ReportShotsFired();
                    else if (Item.Type == ReportDispatch.ReportSpottedStolenCar)
                        ReportSpottedStolenCar(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportStolenVehicle)
                        ReportStolenVehicle(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportSuspectArrested)
                        ReportSuspectArrested();
                    else if (Item.Type == ReportDispatch.ReportSuspectLastSeen)
                        ReportSuspectLastSeen(false);
                    else if (Item.Type == ReportDispatch.ReportSuspectLost)
                        ReportSuspectLost();
                    else if (Item.Type == ReportDispatch.ReportSuspectWasted)
                        ReportSuspectWasted();
                    else if (Item.Type == ReportDispatch.ReportThreateningWithFirearm)
                        ReportThreateningWithFirearm();
                    else if (Item.Type == ReportDispatch.ReportVehicleHitAndRun)
                        ReportVehicleHitAndRun(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportWeaponsFree)
                        ReportWeaponsFree();
                    else if (Item.Type == ReportDispatch.ReportSuspiciousActivity)
                        ReportSuspiciousActivity();
                    else if (Item.Type == ReportDispatch.ReportSuspiciousVehicle)
                        ReportSuspiciousVehicle(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportGrandTheftAuto)
                        ReportGrandTheftAuto();     
                    else
                        ReportAssualtOnOfficer();
                    DispatchQueue.RemoveAt(0);
                }
                ExecutingQueue = false;
            });
        }
    }
    private static void ReportGenericStart(ref List<string> myList)
    {
        //if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
        //    return;
        myList.Add(ScannerAudio.AudioBeeps.AudioStart());
        myList.Add(ScannerAudio.we_have.OfficersReport());
    }
    private static void ReportGenericEnd(List<string> myList, bool Near)
    {
        if (Near)
        {
            Vector3 Pos = Game.LocalPlayer.Character.Position;
            Zone MyZone = Zones.GetZoneName(Pos);
            if (MyZone != null && MyZone.ScannerValue != "")
            {
                myList.Add(ScannerAudio.conjunctives.NearGenericRandom());
                myList.Add(MyZone.ScannerValue);
            }
        }
        myList.Add(ScannerAudio.AudioBeeps.Radio_End_1.FileName);
    }
    public static void ReportShotsFired()
    {
        InstantAction.WriteToLog("Dispatch", "Got to the proc");
        if (ReportedShotsFired || ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        ReportedShotsFired = true;
        ReportedLethalForceAuthorized = true;

        InstantAction.WriteToLog("Dispatch", "Should have reported shots fired");

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        ScannerList.Add(crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        AddLethalForceAuthorized(ref ScannerList);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, true);

    }
    public static void ReportCarryingWeapon(GTAWeapon CarryingWeapon)
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || ReportedAssaultOnOfficer || InstantAction.isBusted || InstantAction.isDead)
            return;

        ReportedCarryingWeapon = true;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        if (CarryingWeapon == null)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingaweapon.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_rpg")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.ArmedwithanRPG.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_bat")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithabat.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_grenadelauncher" || CarryingWeapon.Name == "weapon_grenadelauncher_smoke" || CarryingWeapon.Name == "weapon_compactlauncher")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithagrenadelauncher.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_dagger" || CarryingWeapon.Name == "weapon_knife" || CarryingWeapon.Name == "weapon_switchblade")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithaknife.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_minigun")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithaminigun.FileName);
        }
        else if (CarryingWeapon.Name == "weapon_sawnoffshotgun")
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithasawedoffshotgun.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.LMG)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithamachinegun.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Pistol)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithafirearm.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Shotgun)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithashotgun.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.SMG)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithasubmachinegun.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.AR)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryinganassaultrifle.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Sniper)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithasniperrifle.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Heavy)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.status_message.HeavilyArmed.FileName);
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Melee)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingaweapon.FileName);
        }
        else
        {
            int Num = rnd.Next(1, 5);
            if (Num == 1)
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
        }

        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, true);
    }
    public static void ReportOfficerDown()
    {
        if (InstantAction.isBusted || InstantAction.isDead || ReportedOfficerDown)
            return;

        ReportedOfficerDown = true;

        bool locReportedLethalForceAuthorized = ReportedLethalForceAuthorized;
        ReportedLethalForceAuthorized = true;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        bool addRespondCode = true;
        int Num = rnd.Next(1, 6);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AcriticalsituationOfficerdown.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AnofferdownpossiblyKIA.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdown.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.custom_wanted_level_line.Officerdownsituationiscode99.FileName);
            addRespondCode = false;
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName);
            addRespondCode = false;
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdownconditionunknown.FileName);
        }

        if (addRespondCode)
        {
            int Num2 = rnd.Next(1, 9);
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
            else if (Num2 == 4)
            {
                ScannerList.Add(ScannerAudio.custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName);
            }
            else if (Num2 == 5)
            {
                ScannerList.Add(ScannerAudio.custom_wanted_level_line.Code99officersrequireimmediateassistance.FileName);
            }
            else if (Num2 == 6)
            {
                ScannerList.Add(ScannerAudio.custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName);
            }
            else if (Num2 == 7)
            {
                ScannerList.Add(ScannerAudio.dispatch_respond_code.Code99allunitsrespond.FileName);
            }
            else
            {
                ScannerList.Add(ScannerAudio.dispatch_respond_code.EmergencyallunitsrespondCode99.FileName);
            }
        }

        if (!locReportedLethalForceAuthorized)
        {
            int Num3 = rnd.Next(1, 5);
            if (Num3 == 1)
            {
                ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceauthorized.FileName);
            }
            else if (Num3 == 2)
            {
                ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized.FileName);
            }
            else if (Num3 == 3)
            {
                ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized1.FileName);
            }
            else if (Num3 == 4)
            {
                ScannerList.Add(ScannerAudio.lethal_force.Useoflethalforceisauthorized.FileName);
            }
            else
            {
                ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforcepermitted1.FileName);
            }
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportAssualtOnOfficer()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        ReportedAssaultOnOfficer = true;
        ReportedLethalForceAuthorized = true;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
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
        AddLethalForceAuthorized(ref ScannerList);
        ReportGenericEnd(ScannerList, false);

        PlayAudioList(ScannerList, true);
    }
    public static void ReportThreateningWithFirearm()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        ReportedThreateningWithAFirearm = true;
        ReportedLethalForceAuthorized = true;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
        ScannerList.Add(ScannerAudio.crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        AddLethalForceAuthorized(ref ScannerList);
        ReportGenericEnd(ScannerList, false);

        PlayAudioList(ScannerList, true);
    }
    public static void ReportSuspectLastSeen(bool Near)
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 100f))
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        int Num = rnd.Next(1, 5);
        bool near = false;
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
            near = true;
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastSeen.FileName);
            near = true;
        }


        ReportGenericEnd(ScannerList, near);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSuspectArrested()
    {
        List<string> ScannerList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart()
        });

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crook_arrested.Asuspectincustody1.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crook_arrested.Asuspectapprehended.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect1.FileName);
        }

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(ScannerList, true);
    }
    public static void ReportSuspectWasted()
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        int Num = rnd.Next(1, 11);
        if (Num == 1)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crook_killed.Acriminaldown.FileName);
        }
        else if (Num == 2)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crook_killed.Asuspectdown.FileName);
        }
        else if (Num == 3)
        {
            //myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crook_killed.Asuspectdown2.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            ScannerList.Add(ScannerAudio.crook_killed.Asuspectdown1.FileName);
        }
        else if (Num == 5)
        {
            ScannerList.Add(ScannerAudio.crook_killed.Criminaldown.FileName);
        }
        else if (Num == 6)
        {
            ScannerList.Add(ScannerAudio.crook_killed.Suspectdown.FileName);
        }
        else if (Num == 7)
        {
            ScannerList.Add(ScannerAudio.crook_killed.Suspectneutralized.FileName);
        }
        else if (Num == 8)
        {
            ScannerList.Add(ScannerAudio.crook_killed.Suspectdownmedicalexaminerenroute.FileName);
        }
        else if (Num == 9)
        {
            ScannerList.Add(ScannerAudio.crook_killed.Suspectdowncoronerenroute.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crook_killed.Officershavepacifiedsuspect.FileName);
        }



        ScannerList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(ScannerList, true);
    }
    public static void ReportLethalForceAuthorized()
    {
        if (InstantAction.isBusted || InstantAction.isDead || ReportedLethalForceAuthorized)
            return;

        ReportedLethalForceAuthorized = true;

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
    public static void ReportStolenVehicle(GTAVehicle stolenVehicle)
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();

        ScannerList.Add(AudioBeeps.AudioStart());
        ScannerList.Add(attention_all_units_gen.Allunits.FileName);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(we_have.CitizensReport_1.FileName);
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(we_have.CitizensReport_2.FileName);
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(we_have.CitizensReport_3.FileName);
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(we_have.CitizensReport_4.FileName);
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }
        else
        {
            ScannerList.Add(we_have.CitizensReport_2.FileName);
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        }

        AddVehicleDescription(stolenVehicle, ref ScannerList,true);
        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
        GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(15000);
            stolenVehicle.WasReportedStolen = true;
            InstantAction.WriteToLog("StolenVehicles", String.Format("Vehicle {0} was just reported stolen", stolenVehicle.VehicleEnt.Handle));
        });



    }
    public static void ReportSuspiciousActivity()
    {
        if (InstantAction.isBusted || InstantAction.isDead || ReportedLethalForceAuthorized)
            return;

        List<string> ScannerList = new List<string>();

        ScannerList.Add(AudioBeeps.AudioStart());
        ScannerList.Add(attention_all_units_gen.Allunits.FileName);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(we_have.CitizensReport_1.FileName);
            ScannerList.Add(crime_suspicious_activity.Suspiciousactivity.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(we_have.CitizensReport_2.FileName);
            ScannerList.Add(crime_theft.Apossibletheft.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(we_have.CitizensReport_3.FileName);
            ScannerList.Add(crime_person_attempting_to_steal_a_car.Apersonattemptingtostealacar.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(we_have.CitizensReport_2.FileName);
            ScannerList.Add(crime_suspicious_activity.Suspiciousactivity.FileName);
        }
        else
        {
            ScannerList.Add(we_have.CitizensReport_3.FileName);
            ScannerList.Add(crime_suspicious_activity.Suspiciousactivity.FileName);
        }

        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);

    }
    public static void ReportGrandTheftAuto()
    {
        if (ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(crime_grand_theft_auto.Agrandtheftauto.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(crime_grand_theft_auto.Agrandtheftautoinprogress.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(crime_grand_theft_auto.AGTAinprogress.FileName);
        }
        else
        {
            ScannerList.Add(crime_grand_theft_auto.AGTAinprogress1.FileName);
        }
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);

    }
    public static void ReportSuspiciousVehicle(GTAVehicle myCar)
    {
        if (ReportedLethalForceAuthorized || InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.crime_suspicious_vehicle.Asuspiciousvehicle.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.crime_suspicious_vehicle.Asuspiciousvehicle.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.crime_suspicious_vehicle.Asuspiciousvehicle.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crime_suspicious_vehicle.Asuspiciousvehicle.FileName);
        }

        AddVehicleDescription(myCar, ref ScannerList, false);
        ScannerList.Add(ScannerAudio.proceed_with_caution.Approachwithcaution.FileName);
        ReportGenericEnd(ScannerList, true);

        PlayAudioList(ScannerList, false);
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
    public static void BeginDispatch()
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;

        List<string> ScannerList = new List<string>();
        // ReportGenericStart(ScannerList);

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());
        ScannerList.Add(attention_all_units_gen.Attentionallunits.FileName);


        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(officer_begin_patrol.Beginpatrol.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(officer_begin_patrol.Beginbeat.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(officer_begin_patrol.Assigntopatrol.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(officer_begin_patrol.Proceedtopatrolarea.FileName);
        }
        else
        {
            ScannerList.Add(officer_begin_patrol.Proceedwithpatrol.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSpottedStolenCar(GTAVehicle vehicle)
    {
        if (InstantAction.isBusted || InstantAction.isDead || ReportedLethalForceAuthorized)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.crime_person_in_a_stolen_car.Apersoninastolencar.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.crime_person_in_a_stolen_car.Apersoninastolencar.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crime_person_in_a_stolen_car.Apersoninastolencar.FileName);
        }

        //AddVehicleDescription(vehicle, ref ScannerList);
        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportPedHitAndRun(GTAVehicle vehicle)
    {

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.crime_ped_struck_by_veh.Apedestrianstruck.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.crime_ped_struck_by_veh.Apedestrianstruck1.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crime_reckless_driver.Arecklessdriver.FileName);
        }
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle);
        }
        AddVehicleDescription(vehicle, ref ScannerList,false);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportVehicleHitAndRun(GTAVehicle vehicle)
    {

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.crime_dangerous_driving.Dangerousdriving.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.crime_dangerous_driving.Dangerousdriving1.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.crime_reckless_driver.Arecklessdriver.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.crime_traffic_felony.Atrafficfelony.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.crime_reckless_driver.Arecklessdriver.FileName);
        }
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle);
        }
        AddVehicleDescription(vehicle, ref ScannerList,false);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportRecklessDriver(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        ScannerList.Add(ScannerAudio.crime_reckless_driver.Arecklessdriver.FileName);
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle);
        }
        AddVehicleDescription(vehicle, ref ScannerList,false);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportWeaponsFree()
    {
        if (InstantAction.isBusted || InstantAction.isDead)
            return;
        List<string> ScannerList = new List<string>();
        ScannerList.Add(AudioBeeps.AudioStart());
        ScannerList.Add(attention_all_units_gen.Attentionallunits.FileName);
        ScannerList.Add(custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName);
        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportFelonySpeeding(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList);
        ScannerList.Add(ScannerAudio.crime_speeding_felony.Aspeedingfelony.FileName);
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle);
        }
        AddVehicleDescription(vehicle, ref ScannerList,false);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    private static void AddLethalForceAuthorized(ref List<string> ScannerList)
    {
        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceauthorized.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized1.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.lethal_force.Useoflethalforceisauthorized.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforcepermitted1.FileName);
        }
    }
    public static void AddVehicleDescription(GTAVehicle VehicleDescription, ref List<string> ScannerList,bool IncludeLicensePlate)
    {
        VehicleInfo VehicleInformation = InstantAction.GetVehicleInfo(VehicleDescription);
        Color BaseColor = GetBaseColor(VehicleDescription.DescriptionColor);
        ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();




        string ManufacturerScannerFile;
        string VehicleClassScannerFile;
        if (VehicleInformation != null)
        {
            InstantAction.WriteToLog("Description", string.Format("VehicleInformation.ModelScannerFile {0}", VehicleInformation.ModelScannerFile.ToString()));
            ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
            VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
            if (LookupColor != null && (VehicleInformation.ModelScannerFile != "" || VehicleInformation.ModelScannerFile != "" || VehicleClassScannerFile != ""))
            {
                ScannerList.Add(conjunctives.A01.FileName);
                if (VehicleDescription.VehicleEnt.IsDamaged())
                {
                    ScannerList.Add(DamagedScannerAliases.PickRandom());
                }

                if(VehicleInformation.VehicleClass != VehicleLookup.VehicleClass.Emergency)
                    ScannerList.Add(LookupColor.ScannerFile);

                if (ManufacturerScannerFile != "")
                    ScannerList.Add(ManufacturerScannerFile);

                if (VehicleInformation.ModelScannerFile != "")
                    ScannerList.Add(VehicleInformation.ModelScannerFile);
                else if (VehicleClassScannerFile != "")
                    ScannerList.Add(VehicleClassScannerFile);
            }
        }
        
        if (IncludeLicensePlate)
        {
            ScannerList.Add(ScannerAudio.suspect_license_plate.SuspectsLicensePlate.FileName);

            foreach (char c in VehicleDescription.VehicleEnt.LicensePlate)
            {
                string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
                ScannerList.Add(DispatchFileName);
            }
        }
        
    }
    public static void AddStolenVehicle(ref List<string> ScannerList,GTAVehicle VehicleDescription)
    {       
        int Num = rnd.Next(1, 3);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.conjunctives.Inuhh2.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.conjunctives.Inuhh2.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.conjunctives.Inuhh3.FileName);
        }
        ScannerList.Add(ScannerAudio.crime_stolen_vehicle.Apossiblestolenvehicle.FileName);

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

        Color MyColor = PrimaryColor;

        int Index = Extensions.closestColor2(BaseColorList, MyColor);

        return BaseColorList[Index];
    }
    public static string GetManufacturerScannerFile(Manufacturer manufacturer)
    {
        if (manufacturer == Manufacturer.Albany)
            return ScannerAudio.manufacturer.ALBANY01.FileName;
        else if (manufacturer == Manufacturer.Annis)
            return ScannerAudio.manufacturer.ANNIS01.FileName;
        else if (manufacturer == Manufacturer.Benefactor)
            return ScannerAudio.manufacturer.BENEFACTOR01.FileName;
        else if (manufacturer == Manufacturer.Bürgerfahrzeug)
            return ScannerAudio.manufacturer.BF01.FileName;
        else if (manufacturer == Manufacturer.Bollokan)
            return ScannerAudio.manufacturer.BOLLOKAN01.FileName;
        else if (manufacturer == Manufacturer.Bravado)
            return ScannerAudio.manufacturer.BRAVADO01.FileName;
        else if (manufacturer == Manufacturer.Brute)
            return ScannerAudio.manufacturer.BRUTE01.FileName;
        else if (manufacturer == Manufacturer.Buckingham)
            return "";
        else if (manufacturer == Manufacturer.Canis)
            return ScannerAudio.manufacturer.CANIS01.FileName;
        else if (manufacturer == Manufacturer.Chariot)
            return ScannerAudio.manufacturer.CHARIOT01.FileName;
        else if (manufacturer == Manufacturer.Cheval)
            return ScannerAudio.manufacturer.CHEVAL01.FileName;
        else if (manufacturer == Manufacturer.Classique)
            return ScannerAudio.manufacturer.CLASSIQUE01.FileName;
        else if (manufacturer == Manufacturer.Coil)
            return ScannerAudio.manufacturer.COIL01.FileName;
        else if (manufacturer == Manufacturer.Declasse)
            return ScannerAudio.manufacturer.DECLASSE01.FileName;
        else if (manufacturer == Manufacturer.Dewbauchee)
            return ScannerAudio.manufacturer.DEWBAUCHEE01.FileName;
        else if (manufacturer == Manufacturer.Dinka)
            return ScannerAudio.manufacturer.DINKA01.FileName;
        else if (manufacturer == Manufacturer.DUDE)
            return "";
        else if (manufacturer == Manufacturer.Dundreary)
            return ScannerAudio.manufacturer.DUNDREARY01.FileName;
        else if (manufacturer == Manufacturer.Emperor)
            return ScannerAudio.manufacturer.EMPEROR01.FileName;
        else if (manufacturer == Manufacturer.Enus)
            return ScannerAudio.manufacturer.ENUS01.FileName;
        else if (manufacturer == Manufacturer.Fathom)
            return ScannerAudio.manufacturer.FATHOM01.FileName;
        else if (manufacturer == Manufacturer.Gallivanter)
            return ScannerAudio.manufacturer.GALLIVANTER01.FileName;
        else if (manufacturer == Manufacturer.Grotti)
            return ScannerAudio.manufacturer.GROTTI01.FileName;
        else if (manufacturer == Manufacturer.HVY)
            return ScannerAudio.manufacturer.HVY01.FileName;
        else if (manufacturer == Manufacturer.Hijak)
            return ScannerAudio.manufacturer.HIJAK01.FileName;
        else if (manufacturer == Manufacturer.Imponte)
            return ScannerAudio.manufacturer.IMPONTE01.FileName;
        else if (manufacturer == Manufacturer.Invetero)
            return ScannerAudio.manufacturer.INVETERO01.FileName;
        else if (manufacturer == Manufacturer.JackSheepe)
            return ScannerAudio.manufacturer.JACKSHEEPE01.FileName;
        else if (manufacturer == Manufacturer.Jobuilt)
            return ScannerAudio.manufacturer.JOEBUILT01.FileName;
        else if (manufacturer == Manufacturer.Karin)
            return ScannerAudio.manufacturer.KARIN01.FileName;
        else if (manufacturer == Manufacturer.KrakenSubmersibles)
            return "";
        else if (manufacturer == Manufacturer.Lampadati)
            return ScannerAudio.manufacturer.LAMPADATI01.FileName;
        else if (manufacturer == Manufacturer.LibertyChopShop)
            return "";
        else if (manufacturer == Manufacturer.LibertyCityCycles)
            return "";
        else if (manufacturer == Manufacturer.MaibatsuCorporation)
            return ScannerAudio.manufacturer.MAIBATSU01.FileName;
        else if (manufacturer == Manufacturer.Mammoth)
            return ScannerAudio.manufacturer.MAMMOTH01.FileName;
        else if (manufacturer == Manufacturer.MTL)
            return ScannerAudio.manufacturer.MTL01.FileName;
        else if (manufacturer == Manufacturer.Nagasaki)
            return ScannerAudio.manufacturer.NAGASAKI01.FileName;
        else if (manufacturer == Manufacturer.Obey)
            return ScannerAudio.manufacturer.OBEY01.FileName;
        else if (manufacturer == Manufacturer.Ocelot)
            return ScannerAudio.manufacturer.OCELOT01.FileName;
        else if (manufacturer == Manufacturer.Överflöd)
            return ScannerAudio.manufacturer.OVERFLOD01.FileName;
        else if (manufacturer == Manufacturer.Pegassi)
            return ScannerAudio.manufacturer.PEGASI01.FileName;
        else if (manufacturer == Manufacturer.Pfister)
            return "";
        else if (manufacturer == Manufacturer.Principe)
            return ScannerAudio.manufacturer.PRINCIPE01.FileName;
        else if (manufacturer == Manufacturer.Progen)
            return ScannerAudio.manufacturer.PROGEN01.FileName;
        else if (manufacturer == Manufacturer.ProLaps)
            return "";
        else if (manufacturer == Manufacturer.RUNE)
            return "";
        else if (manufacturer == Manufacturer.Schyster)
            return ScannerAudio.manufacturer.SCHYSTER01.FileName;
        else if (manufacturer == Manufacturer.Shitzu)
            return ScannerAudio.manufacturer.SHITZU01.FileName;
        else if (manufacturer == Manufacturer.Speedophile)
            return ScannerAudio.manufacturer.SPEEDOPHILE01.FileName;
        else if (manufacturer == Manufacturer.Stanley)
            return ScannerAudio.manufacturer.STANLEY01.FileName;
        else if (manufacturer == Manufacturer.SteelHorse)
            return ScannerAudio.manufacturer.STEELHORSE01.FileName;
        else if (manufacturer == Manufacturer.Truffade)
            return ScannerAudio.manufacturer.TRUFFADE01.FileName;
        else if (manufacturer == Manufacturer.Übermacht)
            return ScannerAudio.manufacturer.UBERMACHT01.FileName;
        else if (manufacturer == Manufacturer.Vapid)
            return ScannerAudio.manufacturer.VAPID01.FileName;
        else if (manufacturer == Manufacturer.Vulcar)
            return ScannerAudio.manufacturer.VULCAR01.FileName;
        else if (manufacturer == Manufacturer.Vysser)
            return "";
        else if (manufacturer == Manufacturer.Weeny)
            return ScannerAudio.manufacturer.WEENY01.FileName;
        else if (manufacturer == Manufacturer.WesternCompany)
            return ScannerAudio.manufacturer.WESTERNCOMPANY01.FileName;
        else if (manufacturer == Manufacturer.WesternMotorcycleCompany)
            return ScannerAudio.manufacturer.WESTERNMOTORCYCLECOMPANY01.FileName;
        else if (manufacturer == Manufacturer.Willard)
            return "";
        else if (manufacturer == Manufacturer.Zirconium)
            return ScannerAudio.manufacturer.ZIRCONIUM01.FileName;
        else
            return "";
    }
    public static string GetVehicleClassScannerFile(VehicleLookup.VehicleClass myVehicleClass)
    {
        if (myVehicleClass == VehicleLookup.VehicleClass.Boat)
            return vehicle_category.Boat01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Commercial)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Compact)
            return vehicle_category.Sedan.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Coupe)
            return vehicle_category.Coupe01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Cycle)
            return vehicle_category.Bicycle01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Emergency)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Helicopter)
            return vehicle_category.Helicopter01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Industrial)
            return vehicle_category.IndustrialVehicle01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Military)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Motorcycle)
            return vehicle_category.Motorcycle01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Muscle)
            return vehicle_category.MuscleCar01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.OffRoad)
            return vehicle_category.OffRoad01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Plane)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Sedan)
            return vehicle_category.Sedan.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Service)
            return vehicle_category.Service01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Sports)
            return vehicle_category.SportsCar01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.SportsClassic)
            return vehicle_category.Classic01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Super)
            return vehicle_category.PerformanceCar01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.SUV)
            return vehicle_category.SUV01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Trailer)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Train)
            return vehicle_category.Train01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Unknown)
            return "";
        else if (myVehicleClass == VehicleLookup.VehicleClass.Utility)
            return vehicle_category.UtilityVehicle01.FileName;
        else if (myVehicleClass == VehicleLookup.VehicleClass.Van)
            return vehicle_category.Van01.FileName;
        else
            return "";
    }

    public class DispatchLettersNumber
    {
        public char AlphaNumeric { get; set; }
        public string ScannerFile { get; set; }

        public DispatchLettersNumber(char _AlphaNumeric, string _ScannerFile)
        {
            AlphaNumeric = _AlphaNumeric;
            ScannerFile = _ScannerFile;
        }

    }
    public class DispatchQueueItem
    {
        public ReportDispatch Type { get; set; }
        public int Priority { get; set; } = 0;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public GTAWeapon WeaponToReport { get; set; }
        public GTAVehicle VehicleToReport { get; set; }
        public DispatchQueueItem(ReportDispatch _Type,int _Priority, bool _ResultsInLethalForce)
        {
            Type = _Type;
            Priority = _Priority;
            ResultsInLethalForce = _ResultsInLethalForce;
        }
        public DispatchQueueItem(ReportDispatch _Type, int _Priority, bool _ResultsInLethalForce,bool _ResultsInStolenCarSpotted)
        {
            Type = _Type;
            Priority = _Priority;
            ResultsInLethalForce = _ResultsInLethalForce;
            ResultsInStolenCarSpotted = _ResultsInStolenCarSpotted;
        }
        public DispatchQueueItem(ReportDispatch _Type, int _Priority, bool _ResultsInLethalForce, bool _ResultsInStolenCarSpotted, GTAVehicle _VehicleToReport)
        {
            Type = _Type;
            Priority = _Priority;
            ResultsInLethalForce = _ResultsInLethalForce;
            ResultsInStolenCarSpotted = _ResultsInStolenCarSpotted;
            VehicleToReport = _VehicleToReport;
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

