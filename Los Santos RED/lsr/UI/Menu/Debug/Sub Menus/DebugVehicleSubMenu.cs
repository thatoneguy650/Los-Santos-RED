using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugVehicleSubMenu : DebugSubMenu
{
    private UIMenu vehicleItemsMenu;
    private IPlateTypes PlateTypes;
    private List<VehicleColorLookup> VehicleColors;
    private uint LastVehicleHandle;
    private int selectedPrimaryColor;
    private int selectedSecondaryColor;
    private bool SetPrimaryColor;
    private int PrimaryColor;
    private bool SetSecondaryColor;
    private int SecondaryColor;
    private bool SetPearlescentColor;
    private int PearlescentColor;
    private bool SetWheelColor;
    private int WheelColor;
    private bool SetInteriorColor;
    private int InteriorColor;
    private int selectedInteriorColor;
    private int selectedDashboardColor;

    private int FinalPrimaryColor => PrimaryColor == -1 ? 0 : PrimaryColor;
    private int FinalSecondaryColor => SecondaryColor == -1 ? 0 : SecondaryColor;
    private int FinalPearlColor => PearlescentColor == -1 ? 0 : PearlescentColor;
    private int FinalWheelColor => WheelColor == -1 ? 156 : WheelColor;
    private int FinalInteriorColor => InteriorColor == -1 ? 0 : InteriorColor;
    public DebugVehicleSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IPlateTypes plateTypes) : base(debug, menuPool, player)
    {
        PlateTypes = plateTypes;


        VehicleColors = new List<VehicleColorLookup>()
        {
            new VehicleColorLookup(0,"Metallic Black","Metallic","Black",1) { RGBColor = System.Drawing.Color.FromArgb(13, 17, 22) }
            ,new VehicleColorLookup(1,"Metallic Graphite Black","Metallic","Graphite Black",2) { RGBColor = System.Drawing.Color.FromArgb(28, 29, 33) }
            ,new VehicleColorLookup(2,"Metallic Black Steal","Metallic","Black Steal",3) { RGBColor = System.Drawing.Color.FromArgb(50, 56, 61) }
            ,new VehicleColorLookup(3,"Metallic Dark Silver","Metallic","Dark Silver",4) { RGBColor = System.Drawing.Color.FromArgb(69, 75, 79) }
            ,new VehicleColorLookup(4,"Metallic Silver","Metallic","Silver",5) { RGBColor = System.Drawing.Color.FromArgb(153, 157, 160) }
            ,new VehicleColorLookup(5,"Metallic Blue Silver","Metallic","Blue Silver",6) { RGBColor = System.Drawing.Color.FromArgb(194, 196, 198) }
            ,new VehicleColorLookup(6,"Metallic Steel Gray","Metallic","Steel Gray",7) { RGBColor = System.Drawing.Color.FromArgb(151, 154, 151) }
            ,new VehicleColorLookup(7,"Metallic Shadow Silver","Metallic","Shadow Silver",8) { RGBColor = System.Drawing.Color.FromArgb(99, 115, 128) }
            ,new VehicleColorLookup(8,"Metallic Stone Silver","Metallic","Stone Silver",9) { RGBColor = System.Drawing.Color.FromArgb(99, 98, 92) }
            ,new VehicleColorLookup(9,"Metallic Midnight Silver","Metallic","Midnight Silver",10) { RGBColor = System.Drawing.Color.FromArgb(60, 63, 71) }
            ,new VehicleColorLookup(10,"Metallic Gun Metal","Metallic","Gun Metal",11) { RGBColor = System.Drawing.Color.FromArgb(68, 78, 84) }
            ,new VehicleColorLookup(11,"Metallic Anthracite Grey","Metallic","Anthracite Grey",12) { RGBColor = System.Drawing.Color.FromArgb(29, 33, 41) }
            ,new VehicleColorLookup(27,"Metallic Red","Metallic","Red",13) { RGBColor = System.Drawing.Color.FromArgb(192, 14, 26) }
            ,new VehicleColorLookup(28,"Metallic Torino Red","Metallic","Torino Red",14) { RGBColor = System.Drawing.Color.FromArgb(218, 25, 24) }
            ,new VehicleColorLookup(29,"Metallic Formula Red","Metallic","Formula Red",15) { RGBColor = System.Drawing.Color.FromArgb(182, 17, 27) }
            ,new VehicleColorLookup(30,"Metallic Blaze Red","Metallic","Blaze Red",16) { RGBColor = System.Drawing.Color.FromArgb(165, 30, 35) }
            ,new VehicleColorLookup(31,"Metallic Graceful Red","Metallic","Graceful Red",17) { RGBColor = System.Drawing.Color.FromArgb(123, 26, 34) }
            ,new VehicleColorLookup(32,"Metallic Garnet Red","Metallic","Garnet Red",18) { RGBColor = System.Drawing.Color.FromArgb(142, 27, 31) }
            ,new VehicleColorLookup(33,"Metallic Desert Red","Metallic","Desert Red",19) { RGBColor = System.Drawing.Color.FromArgb(111, 24, 24) }
            ,new VehicleColorLookup(34,"Metallic Cabernet Red","Metallic","Cabernet Red",20) { RGBColor = System.Drawing.Color.FromArgb(73, 17, 29) }
            ,new VehicleColorLookup(35,"Metallic Candy Red","Metallic","Candy Red",21) { RGBColor = System.Drawing.Color.FromArgb(182, 15, 37) }
            ,new VehicleColorLookup(36,"Metallic Sunrise Orange","Metallic","Sunrise Orange",22) { RGBColor = System.Drawing.Color.FromArgb(212, 74, 23) }
            ,new VehicleColorLookup(37,"Metallic Classic Gold","Metallic","Classic Gold",23) { RGBColor = System.Drawing.Color.FromArgb(194, 148, 79) }
            ,new VehicleColorLookup(38,"Metallic Orange","Metallic","Orange",24) { RGBColor = System.Drawing.Color.FromArgb(247, 134, 22) }
            ,new VehicleColorLookup(49,"Metallic Dark Green","Metallic","Dark Green",25) { RGBColor = System.Drawing.Color.FromArgb(19, 36, 40) }
            ,new VehicleColorLookup(50,"Metallic Racing Green","Metallic","Racing Green",26) { RGBColor = System.Drawing.Color.FromArgb(18, 46, 43) }
            ,new VehicleColorLookup(51,"Metallic Sea Green","Metallic","Sea Green",27) { RGBColor = System.Drawing.Color.FromArgb(18, 56, 60) }
            ,new VehicleColorLookup(52,"Metallic Olive Green","Metallic","Olive Green",28) { RGBColor = System.Drawing.Color.FromArgb(49, 66, 63) }
            ,new VehicleColorLookup(53,"Metallic Green","Metallic","Green",29) { RGBColor = System.Drawing.Color.FromArgb(21, 92, 45) }
            ,new VehicleColorLookup(54,"Metallic Gasoline Blue Green","Metallic","Gasoline Blue Green",30) { RGBColor = System.Drawing.Color.FromArgb(27, 103, 112) }
            ,new VehicleColorLookup(61,"Metallic Midnight Blue","Metallic","Midnight Blue",31) { RGBColor = System.Drawing.Color.FromArgb(34, 46, 70) }
            ,new VehicleColorLookup(62,"Metallic Dark Blue","Metallic","Dark Blue",32) { RGBColor = System.Drawing.Color.FromArgb(35, 49, 85) }
            ,new VehicleColorLookup(63,"Metallic Saxony Blue","Metallic","Saxony Blue",33) { RGBColor = System.Drawing.Color.FromArgb(48, 76, 126) }
            ,new VehicleColorLookup(64,"Metallic Blue","Metallic","Blue",34) { RGBColor = System.Drawing.Color.FromArgb(71, 87, 143) }
            ,new VehicleColorLookup(65,"Metallic Mariner Blue","Metallic","Mariner Blue",35) { RGBColor = System.Drawing.Color.FromArgb(99, 123, 167) }
            ,new VehicleColorLookup(66,"Metallic Harbor Blue","Metallic","Harbor Blue",36) { RGBColor = System.Drawing.Color.FromArgb(57, 71, 98) }
            ,new VehicleColorLookup(67,"Metallic Diamond Blue","Metallic","Diamond Blue",37) { RGBColor = System.Drawing.Color.FromArgb(214, 231, 241) }
            ,new VehicleColorLookup(68,"Metallic Surf Blue","Metallic","Surf Blue",38) { RGBColor = System.Drawing.Color.FromArgb(118, 175, 190) }
            ,new VehicleColorLookup(69,"Metallic Nautical Blue","Metallic","Nautical Blue",39) { RGBColor = System.Drawing.Color.FromArgb(52, 94, 114) }
            ,new VehicleColorLookup(70,"Metallic Bright Blue","Metallic","Bright Blue",40) { RGBColor = System.Drawing.Color.FromArgb(11, 156, 241) }
            ,new VehicleColorLookup(71,"Metallic Purple Blue","Metallic","Purple Blue",41) { RGBColor = System.Drawing.Color.FromArgb(47, 45, 82) }
            ,new VehicleColorLookup(72,"Metallic Spinnaker Blue","Metallic","Spinnaker Blue",42) { RGBColor = System.Drawing.Color.FromArgb(40, 44, 77) }
            ,new VehicleColorLookup(73,"Metallic Ultra Blue","Metallic","Ultra Blue",43) { RGBColor = System.Drawing.Color.FromArgb(35, 84, 161) }
            ,new VehicleColorLookup(74,"Metallic Bright Blue","Metallic","Bright Blue",44) { RGBColor = System.Drawing.Color.FromArgb(110, 163, 198) }
            ,new VehicleColorLookup(88,"Metallic Taxi Yellow","Metallic","Taxi Yellow",45) { RGBColor = System.Drawing.Color.FromArgb(255, 207, 32) }
            ,new VehicleColorLookup(89,"Metallic Race Yellow","Metallic","Race Yellow",46) { RGBColor = System.Drawing.Color.FromArgb(251, 226, 18) }
            ,new VehicleColorLookup(90,"Metallic Bronze","Metallic","Bronze",47) { RGBColor = System.Drawing.Color.FromArgb(145, 101, 50) }
            ,new VehicleColorLookup(91,"Metallic Yellow Bird","Metallic","Yellow Bird",48) { RGBColor = System.Drawing.Color.FromArgb(224, 225, 61) }
            ,new VehicleColorLookup(92,"Metallic Lime","Metallic","Lime",49) { RGBColor = System.Drawing.Color.FromArgb(152, 210, 35) }
            ,new VehicleColorLookup(93,"Metallic Champagne","Metallic","Champagne",50) { RGBColor = System.Drawing.Color.FromArgb(155, 140, 120) }
            ,new VehicleColorLookup(94,"Metallic Pueblo Beige","Metallic","Pueblo Beige",51) { RGBColor = System.Drawing.Color.FromArgb(80, 50, 24) }
            ,new VehicleColorLookup(95,"Metallic Dark Ivory","Metallic","Dark Ivory",52) { RGBColor = System.Drawing.Color.FromArgb(71, 63, 43) }
            ,new VehicleColorLookup(96,"Metallic Choco Brown","Metallic","Choco Brown",53) { RGBColor = System.Drawing.Color.FromArgb(34, 27, 25) }
            ,new VehicleColorLookup(97,"Metallic Golden Brown","Metallic","Golden Brown",54) { RGBColor = System.Drawing.Color.FromArgb(101, 63, 35) }
            ,new VehicleColorLookup(98,"Metallic Light Brown","Metallic","Light Brown",55) { RGBColor = System.Drawing.Color.FromArgb(119, 92, 62) }
            ,new VehicleColorLookup(99,"Metallic Straw Beige","Metallic","Straw Beige",56) { RGBColor = System.Drawing.Color.FromArgb(172, 153, 117) }
            ,new VehicleColorLookup(100,"Metallic Moss Brown","Metallic","Moss Brown",57) { RGBColor = System.Drawing.Color.FromArgb(108, 107, 75) }
            ,new VehicleColorLookup(101,"Metallic Biston Brown","Metallic","Biston Brown",58) { RGBColor = System.Drawing.Color.FromArgb(64, 46, 43) }
            ,new VehicleColorLookup(102,"Metallic Beechwood","Metallic","Beechwood",59) { RGBColor = System.Drawing.Color.FromArgb(164, 150, 95) }
            ,new VehicleColorLookup(103,"Metallic Dark Beechwood","Metallic","Dark Beechwood",60) { RGBColor = System.Drawing.Color.FromArgb(70, 35, 26) }
            ,new VehicleColorLookup(104,"Metallic Choco Orange","Metallic","Choco Orange",61) { RGBColor = System.Drawing.Color.FromArgb(117, 43, 25) }
            ,new VehicleColorLookup(105,"Metallic Beach Sand","Metallic","Beach Sand",62) { RGBColor = System.Drawing.Color.FromArgb(191, 174, 123) }
            ,new VehicleColorLookup(106,"Metallic Sun Bleeched Sand","Metallic","Sun Bleeched Sand",63) { RGBColor = System.Drawing.Color.FromArgb(223, 213, 178) }
            ,new VehicleColorLookup(107,"Metallic Cream","Metallic","Cream",64) { RGBColor = System.Drawing.Color.FromArgb(247, 237, 213) }
            ,new VehicleColorLookup(111,"Metallic White","Metallic","White",65) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 246) }
            ,new VehicleColorLookup(112,"Metallic Frost White","Metallic","Frost White",66) { RGBColor = System.Drawing.Color.FromArgb(234, 234, 234) }
            ,new VehicleColorLookup(125,"Metallic Securicor Green","Metallic","Securicor Green",67) { RGBColor = System.Drawing.Color.FromArgb(131, 197, 102) }
            ,new VehicleColorLookup(137,"Metallic Vermillion Pink","Metallic","Vermillion Pink",68) { RGBColor = System.Drawing.Color.FromArgb(223, 88, 145) }
            ,new VehicleColorLookup(141,"Metallic Black Blue","Metallic","Black Blue",69) { RGBColor = System.Drawing.Color.FromArgb(10, 12, 23) }
            ,new VehicleColorLookup(142,"Metallic Black Purple","Metallic","Black Purple",70) { RGBColor = System.Drawing.Color.FromArgb(12, 13, 24) }
            ,new VehicleColorLookup(143,"Metallic Black Red","Metallic","Black Red",71) { RGBColor = System.Drawing.Color.FromArgb(14, 13, 20) }
            ,new VehicleColorLookup(145,"Metallic Purple","Metallic","Purple",72) { RGBColor = System.Drawing.Color.FromArgb(98, 18, 118) }
            ,new VehicleColorLookup(146,"Metallic V Dark Blue","Metallic","V Dark Blue",73) { RGBColor = System.Drawing.Color.FromArgb(11, 20, 33) }
            ,new VehicleColorLookup(150,"Metallic Lava Red","Metallic","Lava Red",74) { RGBColor = System.Drawing.Color.FromArgb(188, 25, 23) }

            ,new VehicleColorLookup(12,"Matte Black","Matte","Black",75) { RGBColor = System.Drawing.Color.FromArgb(19, 24, 31) }
            ,new VehicleColorLookup(13,"Matte Gray","Matte","Gray",76) { RGBColor = System.Drawing.Color.FromArgb(38, 40, 42) }
            ,new VehicleColorLookup(14,"Matte Light Grey","Matte","Light Grey",77) { RGBColor = System.Drawing.Color.FromArgb(81, 85, 84) }
            ,new VehicleColorLookup(39,"Matte Red","Matte","Red",78) { RGBColor = System.Drawing.Color.FromArgb(207, 31, 33) }
            ,new VehicleColorLookup(40,"Matte Dark Red","Matte","Dark Red",79) { RGBColor = System.Drawing.Color.FromArgb(115, 32, 33) }
            ,new VehicleColorLookup(41,"Matte Orange","Matte","Orange",80) { RGBColor = System.Drawing.Color.FromArgb(242, 125, 32) }
            ,new VehicleColorLookup(42,"Matte Yellow","Matte","Yellow",81) { RGBColor = System.Drawing.Color.FromArgb(255, 201, 31) }
            ,new VehicleColorLookup(55,"Matte Lime Green","Matte","Lime Green",82) { RGBColor = System.Drawing.Color.FromArgb(102, 184, 31) }
            ,new VehicleColorLookup(82,"Matte Dark Blue","Matte","Dark Blue",83) { RGBColor = System.Drawing.Color.FromArgb(31, 40, 82) }
            ,new VehicleColorLookup(83,"Matte Blue","Matte","Blue",84) { RGBColor = System.Drawing.Color.FromArgb(37, 58, 167) }
            ,new VehicleColorLookup(84,"Matte Midnight Blue","Matte","Midnight Blue",85) { RGBColor = System.Drawing.Color.FromArgb(28, 53, 81) }
            ,new VehicleColorLookup(128,"Matte Green","Matte","Green",86) { RGBColor = System.Drawing.Color.FromArgb(78, 100, 67) }
            ,new VehicleColorLookup(129,"Matte Brown","Matte","Brown",87) { RGBColor = System.Drawing.Color.FromArgb(188, 172, 143) }
            ,new VehicleColorLookup(148,"Matte Purple","Matte","Purple",88) { RGBColor = System.Drawing.Color.FromArgb(107, 31, 123) }
            ,new VehicleColorLookup(149,"Matte Dark Purple","Matte","Dark Purple",89) { RGBColor = System.Drawing.Color.FromArgb(30, 29, 34) }
            ,new VehicleColorLookup(151,"Matte Forest Green","Matte","Forest Green",90) { RGBColor = System.Drawing.Color.FromArgb(45, 54, 42) }
            ,new VehicleColorLookup(152,"Matte Olive Drab","Matte","Olive Drab",91) { RGBColor = System.Drawing.Color.FromArgb(105, 103, 72) }
            ,new VehicleColorLookup(153,"Matte Desert Brown","Matte","Desert Brown",92) { RGBColor = System.Drawing.Color.FromArgb(122, 108, 85) }
            ,new VehicleColorLookup(154,"Matte Desert Tan","Matte","Desert Tan",93) { RGBColor = System.Drawing.Color.FromArgb(195, 180, 146) }
            ,new VehicleColorLookup(155,"Matte Foilage Green","Matte","Foilage Green",94) { RGBColor = System.Drawing.Color.FromArgb(90, 99, 82) }
            ,new VehicleColorLookup(131,"Matte White","Matte","White",95) { RGBColor = System.Drawing.Color.FromArgb(252, 249, 241) }

            ,new VehicleColorLookup(15,"Util Black","Util","Black",96) { RGBColor = System.Drawing.Color.FromArgb(21, 25, 33) }
            ,new VehicleColorLookup(16,"Util Black Poly","Util","Black Poly",97) { RGBColor = System.Drawing.Color.FromArgb(30, 36, 41) }
            ,new VehicleColorLookup(17,"Util Dark silver","Util","Dark silver",98) { RGBColor = System.Drawing.Color.FromArgb(51, 58, 60) }
            ,new VehicleColorLookup(18,"Util Silver","Util","Silver",99) { RGBColor = System.Drawing.Color.FromArgb(140, 144, 149) }
            ,new VehicleColorLookup(19,"Util Gun Metal","Util","Gun Metal",100) { RGBColor = System.Drawing.Color.FromArgb(57, 67, 77) }
            ,new VehicleColorLookup(20,"Util Shadow Silver","Util","Shadow Silver",101) { RGBColor = System.Drawing.Color.FromArgb(80, 98, 114) }
            ,new VehicleColorLookup(43,"Util Red","Util","Red",102) { RGBColor = System.Drawing.Color.FromArgb(156, 16, 22) }
            ,new VehicleColorLookup(44,"Util Bright Red","Util","Bright Red",103) { RGBColor = System.Drawing.Color.FromArgb(222, 15, 24) }
            ,new VehicleColorLookup(45,"Util Garnet Red","Util","Garnet Red",104) { RGBColor = System.Drawing.Color.FromArgb(143, 30, 23) }
            ,new VehicleColorLookup(56,"Util Dark Green","Util","Dark Green",105) { RGBColor = System.Drawing.Color.FromArgb(34, 56, 62) }
            ,new VehicleColorLookup(57,"Util Green","Util","Green",106) { RGBColor = System.Drawing.Color.FromArgb(29, 90, 63) }
            ,new VehicleColorLookup(75,"Util Dark Blue","Util","Dark Blue",107) { RGBColor = System.Drawing.Color.FromArgb(17, 37, 82) }
            ,new VehicleColorLookup(76,"Util Midnight Blue","Util","Midnight Blue",108) { RGBColor = System.Drawing.Color.FromArgb(27, 32, 62) }
            ,new VehicleColorLookup(77,"Util Blue","Util","Blue",109) { RGBColor = System.Drawing.Color.FromArgb(39, 81, 144) }
            ,new VehicleColorLookup(78,"Util Sea Foam Blue","Util","Sea Foam Blue",110) { RGBColor = System.Drawing.Color.FromArgb(96, 133, 146) }
            ,new VehicleColorLookup(79,"Util Lightning blue","Util","Lightning blue",111) { RGBColor = System.Drawing.Color.FromArgb(36, 70, 168) }
            ,new VehicleColorLookup(80,"Util Maui Blue Poly","Util","Maui Blue Poly",112) { RGBColor = System.Drawing.Color.FromArgb(66, 113, 225) }
            ,new VehicleColorLookup(81,"Util Bright Blue","Util","Bright Blue",113) { RGBColor = System.Drawing.Color.FromArgb(59, 57, 224) }
            ,new VehicleColorLookup(108,"Util Brown","Util","Brown",114) { RGBColor = System.Drawing.Color.FromArgb(58, 42, 27) }
            ,new VehicleColorLookup(109,"Util Medium Brown","Util","Medium Brown",115) { RGBColor = System.Drawing.Color.FromArgb(120, 95, 51) }
            ,new VehicleColorLookup(110,"Util Light Brown","Util","Light Brown",116) { RGBColor = System.Drawing.Color.FromArgb(181, 160, 121) }
            ,new VehicleColorLookup(122,"Util Off White","Util","Off White",117) { RGBColor = System.Drawing.Color.FromArgb(223, 221, 208) }

            ,new VehicleColorLookup(21,"Worn Black","Worn","Black",118) { RGBColor = System.Drawing.Color.FromArgb(30, 35, 47) }
            ,new VehicleColorLookup(22,"Worn Graphite","Worn","Graphite",119) { RGBColor = System.Drawing.Color.FromArgb(54, 58, 63) }
            ,new VehicleColorLookup(23,"Worn Silver Grey","Worn","Silver Grey",120) { RGBColor = System.Drawing.Color.FromArgb(160, 161, 153) }
            ,new VehicleColorLookup(24,"Worn Silver","Worn","Silver",121) { RGBColor = System.Drawing.Color.FromArgb(211, 211, 211) }
            ,new VehicleColorLookup(25,"Worn Blue Silver","Worn","Blue Silver",122) { RGBColor = System.Drawing.Color.FromArgb(183, 191, 202) }
            ,new VehicleColorLookup(26,"Worn Shadow Silver","Worn","Shadow Silver",123) { RGBColor = System.Drawing.Color.FromArgb(119, 135, 148) }
            ,new VehicleColorLookup(46,"Worn Red","Worn","Red",124) { RGBColor = System.Drawing.Color.FromArgb(169, 71, 68) }
            ,new VehicleColorLookup(47,"Worn Golden Red","Worn","Golden Red",125) { RGBColor = System.Drawing.Color.FromArgb(177, 108, 81) }
            ,new VehicleColorLookup(48,"Worn Dark Red","Worn","Dark Red",126) { RGBColor = System.Drawing.Color.FromArgb(55, 28, 37) }
            ,new VehicleColorLookup(58,"Worn Dark Green","Worn","Dark Green",127) { RGBColor = System.Drawing.Color.FromArgb(45, 66, 63) }
            ,new VehicleColorLookup(59,"Worn Green","Worn","Green",128) { RGBColor = System.Drawing.Color.FromArgb(69, 89, 75) }
            ,new VehicleColorLookup(60,"Worn Sea Wash","Worn","Sea Wash",129) { RGBColor = System.Drawing.Color.FromArgb(101, 134, 127) }
            ,new VehicleColorLookup(85,"Worn Dark blue","Worn","Dark blue",130) { RGBColor = System.Drawing.Color.FromArgb(76, 95, 129) }
            ,new VehicleColorLookup(86,"Worn Blue","Worn","Blue",131) { RGBColor = System.Drawing.Color.FromArgb(88, 104, 142) }
            ,new VehicleColorLookup(87,"Worn Light blue","Worn","Light blue",132) { RGBColor = System.Drawing.Color.FromArgb(116, 181, 216) }
            ,new VehicleColorLookup(113,"Worn Honey Beige","Worn","Honey Beige",133) { RGBColor = System.Drawing.Color.FromArgb(176, 171, 148) }
            ,new VehicleColorLookup(114,"Worn Brown","Worn","Brown",134) { RGBColor = System.Drawing.Color.FromArgb(69, 56, 49) }
            ,new VehicleColorLookup(115,"Worn Dark Brown","Worn","Dark Brown",135) { RGBColor = System.Drawing.Color.FromArgb(42, 40, 43) }
            ,new VehicleColorLookup(116,"Worn straw beige","Worn","straw beige",136) { RGBColor = System.Drawing.Color.FromArgb(114, 108, 87) }
            ,new VehicleColorLookup(121,"Worn Off White","Worn","Off White",137) { RGBColor = System.Drawing.Color.FromArgb(234, 230, 222) }
            ,new VehicleColorLookup(123,"Worn Orange","Worn","Orange",138) { RGBColor = System.Drawing.Color.FromArgb(242, 173, 46) }
            ,new VehicleColorLookup(124,"Worn Light Orange","Worn","Light Orange",139) { RGBColor = System.Drawing.Color.FromArgb(249, 164, 88) }
            ,new VehicleColorLookup(126,"Worn Taxi Yellow","Worn","Taxi Yellow",140) { RGBColor = System.Drawing.Color.FromArgb(241, 204, 64) }
            ,new VehicleColorLookup(130,"Worn Orange","Worn","Orange",141) { RGBColor = System.Drawing.Color.FromArgb(248, 182, 88) }
            ,new VehicleColorLookup(132,"Worn White","Worn","White",142) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 251) }
            ,new VehicleColorLookup(133,"Worn Olive Army Green","Worn","Olive Army Green",143) { RGBColor = System.Drawing.Color.FromArgb(129, 132, 76) }

            ,new VehicleColorLookup(134,"Pure White","Standard","Pure White",150) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 255) }
            ,new VehicleColorLookup(135,"Hot Pink","Standard","Hot Pink",150) { RGBColor = System.Drawing.Color.FromArgb(242, 31, 153) }
            ,new VehicleColorLookup(136,"Salmon pink","Standard","Salmon Pink",150) { RGBColor = System.Drawing.Color.FromArgb(253, 214, 205) }
            ,new VehicleColorLookup(138,"Orange","Standard","Orange",150) { RGBColor = System.Drawing.Color.FromArgb(246, 174, 32) }
            ,new VehicleColorLookup(139,"Green","Standard","Green",150) { RGBColor = System.Drawing.Color.FromArgb(176, 238, 110) }
            ,new VehicleColorLookup(140,"Blue","Standard","Blue",150) { RGBColor = System.Drawing.Color.FromArgb(8, 233, 250) }
            ,new VehicleColorLookup(156,"DEFAULT ALLOY COLOR","Standard","DEFAULT ALLOY COLOR",206)

            ,new VehicleColorLookup(120,"Chrome","Chrome","Chrome",199) { RGBColor = System.Drawing.Color.FromArgb(88, 112, 161) }

            ,new VehicleColorLookup(117,"Brushed Steel","Metals","Brushed Steel",200) { RGBColor = System.Drawing.Color.FromArgb(106, 116, 124) }
            ,new VehicleColorLookup(118,"Brushed Black Steel","Metals","Brushed Black Steel",201) { RGBColor = System.Drawing.Color.FromArgb(53, 65, 88) }
            ,new VehicleColorLookup(119,"Brushed Aluminium","Metals","Brushed Aluminium",202) { RGBColor = System.Drawing.Color.FromArgb(155, 160, 168) }
            ,new VehicleColorLookup(158,"Pure Gold","Metals","Pure Gold",203) { RGBColor = System.Drawing.Color.FromArgb(122, 100, 64) }
            ,new VehicleColorLookup(159,"Brushed Gold","Metals","Brushed Gold",204) { RGBColor = System.Drawing.Color.FromArgb(127, 106, 72) }



            //,new ColorLookup(127,"police car blue","Unknown","police car blue",205)
            //,new ColorLookup(156,"DEFAULT ALLOY COLOR","Unknown","DEFAULT ALLOY COLOR",206)
            //,new ColorLookup(157,"Epsilon Blue","Unknown","Epsilon Blue",207)
            //,new ColorLookup(144,"hunter green","Unknown","hunter green",208)
            //,new ColorLookup(147,"MODSHOP BLACK1","Unknown","MODSHOP BLACK1",209)

        };


    }
    public override void AddItems()
    {
        vehicleItemsMenu = MenuPool.AddSubMenu(Debug, "Vehicle Menu");
        vehicleItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various vehicle items.";
        Update();
    }
    public override void Update()
    {

        if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
        {
            vehicleItemsMenu.Clear();
            LastVehicleHandle = 0;
            return;
        }
        if (LastVehicleHandle != Player.InterestedVehicle.Handle)
        {

            vehicleItemsMenu.Clear();
            CreateMenu();
        }
    }
    private void CreateMenu()
    {
        LastVehicleHandle = Player.InterestedVehicle.Handle;
        CreatePlateMenuItem();
        CreateLiveryMenuItem();
        CreateLivery2MenuItem();
        CreateExtraMenuItem();
        CreateColorMenuItem();
        CreateBetterColorMenu();
        CreateInfoMenuItem();
        CreateModificationItem();
        CreateOtherItem();
    }

    private void CreateOtherItem()
    {
        UIMenuNumericScrollerItem<int> SetDoorOpenMenuItem = new UIMenuNumericScrollerItem<int>("Set Door Open", "Set the vehicle door", 0, 5, 1) { Value = 0 };
        SetDoorOpenMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_DOOR_OPEN(Player.InterestedVehicle.Vehicle,SetDoorOpenMenuItem.Value,true,true);
            }
        };
        vehicleItemsMenu.AddItem(SetDoorOpenMenuItem);
    }

    private void CreateModificationItem()
    {

        UIMenuNumericScrollerItem<int> VehicleExtraMenuItem = new UIMenuNumericScrollerItem<int>("Set Extra", "Set the vehicle Extra", 1, 12, 1) { Value = 1 };
        VehicleExtraMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
               if(!NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value))
                {
                    Game.DisplaySubtitle($"EXTRA {VehicleExtraMenuItem.Value} DOES NOT EXIST");
                    return;
                }
                bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value);
                NativeFunction.Natives.SET_VEHICLE_EXTRA(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value, isOn);
                Game.DisplaySubtitle($"SET EXTRA {VehicleExtraMenuItem.Value} Disabled:{isOn}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleExtraMenuItem);




        UIMenuNumericScrollerItem<int> VehicleWheelTypeMenuItem = new UIMenuNumericScrollerItem<int>("Set Wheel Type", "Set the vehicle wheel type", 0, 12, 1) { Value = 1 };
        VehicleWheelTypeMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(Player.InterestedVehicle.Vehicle, VehicleWheelTypeMenuItem.Value);
                Game.DisplaySubtitle($"SET_VEHICLE_WHEEL_TYPE {VehicleWheelTypeMenuItem.Value} ");
            }
        };
        vehicleItemsMenu.AddItem(VehicleWheelTypeMenuItem);



        //NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(vehicleExt.Vehicle, WheelType);


        List<int> possibleToggles = new List<int>() { 17,18,19,20,21,22 };

        UIMenuListScrollerItem<int> VehicleToggleMenuItem = new UIMenuListScrollerItem<int>("Set Toggle", "Set the vehicle toggle", possibleToggles);
        VehicleToggleMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_MOD_KIT(Player.InterestedVehicle.Vehicle, 0);
                bool isOn = NativeFunction.Natives.IS_TOGGLE_MOD_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleToggleMenuItem.SelectedItem);
                NativeFunction.Natives.TOGGLE_VEHICLE_MOD(Player.InterestedVehicle.Vehicle, VehicleToggleMenuItem.SelectedItem, isOn);
                Game.DisplaySubtitle($"SET Toggle {VehicleToggleMenuItem.SelectedItem} Disabled:{isOn}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleToggleMenuItem);

        List<ModKitDescription> ModKitDescriptions = new List<ModKitDescription>()
        {
            new ModKitDescription("Spoilers",0),
            new ModKitDescription("Front Bumper",1),
            new ModKitDescription("Rear Bumper",2),
            new ModKitDescription("Side Skirt",3),
            new ModKitDescription("Exhaust",4),
            new ModKitDescription("Frame",5),
            new ModKitDescription("Grille",6),
            new ModKitDescription("Hood",7),
            new ModKitDescription("Fender",8),
            new ModKitDescription("Right Fender",9),
            new ModKitDescription("Roof",10),
            new ModKitDescription("Engine",11),
            new ModKitDescription("Brakes",12),
            new ModKitDescription("Transmission",13),
            new ModKitDescription("Horns",1),
            new ModKitDescription("Suspension",15),
            new ModKitDescription("Armor",16),
            new ModKitDescription("Turbo",18),
            new ModKitDescription("Xenon",22),
            new ModKitDescription("Front Wheels",23),
            new ModKitDescription("Back Wheels (Motorcycle)",24),
            new ModKitDescription("Plate holders", 25),
            new ModKitDescription("Trim Design", 27),
            new ModKitDescription("Ornaments", 28),
            new ModKitDescription("Dial Design", 30),
            new ModKitDescription("Steering Wheel", 33),
            new ModKitDescription("Shift Lever", 34),
            new ModKitDescription("Plaques", 35),
            new ModKitDescription("Hydraulics", 38),
            new ModKitDescription("Boost", 40),
            new ModKitDescription("Window Tint", 55),
            new ModKitDescription("Livery - 48", 48),
            new ModKitDescription("Plate - 53", 53),




            new ModKitDescription("Pushbar - 42", 42),
            new ModKitDescription("Aerials - 43", 43),
            new ModKitDescription("Searchlights - 44", 44),


        };


        UIMenuListScrollerItem<ModKitDescription> VehicleModMenuItem = new UIMenuListScrollerItem<ModKitDescription>("Set Mod", "Set the vehicle mod", ModKitDescriptions);
        VehicleModMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                if (!int.TryParse(NativeHelper.GetKeyboardInput("-1"), out int modID))
                {
                    return;
                }
                if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                {
                    NativeFunction.Natives.SET_VEHICLE_MOD_KIT(Player.InterestedVehicle.Vehicle, 0);
                    NativeFunction.Natives.SET_VEHICLE_MOD(Player.InterestedVehicle.Vehicle, VehicleModMenuItem.SelectedItem.ID, modID, false);
                    Game.DisplaySubtitle($"SET MOD {VehicleModMenuItem.SelectedItem.ID} modID:{modID}");
                }
            }
        };
        vehicleItemsMenu.AddItem(VehicleModMenuItem);

        /*VehicleExtras = new List<VehicleExtra>()
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,false),
                        new VehicleExtra(2,false),
                        new VehicleExtra(3,false),
                        new VehicleExtra(4,false),
                        new VehicleExtra(5,false),
                        new VehicleExtra(6,false),
                        new VehicleExtra(7,false),
                        new VehicleExtra(8,false),
                        new VehicleExtra(9,false),
                        new VehicleExtra(10,false),
                        new VehicleExtra(11,false),
                        new VehicleExtra(12,false),
                        new VehicleExtra(13,false),
                        new VehicleExtra(14,false),
                        new VehicleExtra(15,false),
                    },
                VehicleToggles = new List<VehicleToggle>()
                    {
                        new VehicleToggle(17,false),
                        new VehicleToggle(18,false),
                        new VehicleToggle(19,false),
                        new VehicleToggle(20,false),
                        new VehicleToggle(21,false),
                        new VehicleToggle(22,false),
                    },
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(0,8),
                        new VehicleMod(1,0),
                        new VehicleMod(2,-1),
                        new VehicleMod(3,0),
                        new VehicleMod(4,-1),
                        new VehicleMod(5,1),
                        new VehicleMod(6,0),
                        new VehicleMod(7,2),
                        new VehicleMod(8,-1),
                        new VehicleMod(9,0),
                        new VehicleMod(10,0),
                        new VehicleMod(11,3),
                        new VehicleMod(12,-1),
                        new VehicleMod(13,-1),
                        new VehicleMod(14,-1),
                        new VehicleMod(15,-1),
                        new VehicleMod(16,-1),
                        new VehicleMod(23,12),
                        new VehicleMod(24,-1),
                        new VehicleMod(25,-1),
                        new VehicleMod(26,-1),
                        new VehicleMod(27,-1),
                        new VehicleMod(28,-1),
                        new VehicleMod(29,-1),
                        new VehicleMod(30,-1),
                        new VehicleMod(31,-1),
                        new VehicleMod(32,-1),
                        new VehicleMod(33,-1),
                        new VehicleMod(34,-1),
                        new VehicleMod(35,-1),
                        new VehicleMod(36,-1),
                        new VehicleMod(37,-1),
                        new VehicleMod(38,-1),
                        new VehicleMod(39,-1),
                        new VehicleMod(40,-1),
                        new VehicleMod(41,-1),
                        new VehicleMod(42,-1),
                        new VehicleMod(43,1),
                        new VehicleMod(44,-1),
                        new VehicleMod(45,-1),
                        new VehicleMod(46,-1),
                        new VehicleMod(47,-1),
                        new VehicleMod(48,-1),
                        new VehicleMod(49,-1),
                        new VehicleMod(50,-1),
                    },*/
    }

    private void CreateExtraMenuItem()
    {
        //UIMenuNumericScrollerItem<int> VehicleExtraMenuItem = new UIMenuNumericScrollerItem<int>("Set Extra", "Set the vehicle Extra", 1, 15, 1) { Value = 1 };
        //VehicleExtraMenuItem.Activated += (menu, item) =>
        //{
        //    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
        //    {
        //        bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value);
        //        NativeFunction.Natives.SET_VEHICLE_EXTRA(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value, isOn);
        //        Game.DisplaySubtitle($"SET EXTRA {VehicleExtraMenuItem.Value} Disabled:{isOn}");
        //    }
        //};
        //vehicleItemsMenu.AddItem(VehicleExtraMenuItem);
    }
    private void CreateColorMenuItem()//CreateColorMenuItem
    {

        UIMenuNumericScrollerItem<float> dirtsetscroller = new UIMenuNumericScrollerItem<float>("Set Dirtyness", "Set how dirty the car is", 0.0f, 15.0f, 1.0f);
        dirtsetscroller.Value = dirtsetscroller.Minimum;
        dirtsetscroller.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(Player.InterestedVehicle.Vehicle, dirtsetscroller.Value);
                Game.DisplaySubtitle($"SET DIRT {dirtsetscroller.Value}");
            }
        };
        vehicleItemsMenu.AddItem(dirtsetscroller);



        UIMenu colorSimpleMenu = MenuPool.AddSubMenu(vehicleItemsMenu, "Simple Colors");
        colorSimpleMenu.SubtitleText = "COLORS";
        vehicleItemsMenu.MenuItems[vehicleItemsMenu.MenuItems.Count() - 1].Description = "Pick Colors";



        UIMenuNumericScrollerItem<int> VehicleColorMenuItem = new UIMenuNumericScrollerItem<int>("Set Both Color", "Set both the vehicle colors the same", 0, 159, 1);
        VehicleColorMenuItem.Value = VehicleColorMenuItem.Minimum;
        VehicleColorMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, VehicleColorMenuItem.Value, VehicleColorMenuItem.Value);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        colorSimpleMenu.AddItem(VehicleColorMenuItem);


        selectedPrimaryColor = 0;
        selectedSecondaryColor = 0;
        UIMenuNumericScrollerItem<int> vehiclePrimaryColorScroller = new UIMenuNumericScrollerItem<int>("Primary Color", "Set the primary color selected", 0, 159, 1);
        vehiclePrimaryColorScroller.Value = vehiclePrimaryColorScroller.Minimum;
        vehiclePrimaryColorScroller.Activated += (menu, item) =>
        {
            selectedPrimaryColor = vehiclePrimaryColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, selectedPrimaryColor, selectedSecondaryColor);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        colorSimpleMenu.AddItem(vehiclePrimaryColorScroller);


        UIMenuNumericScrollerItem<int> vehicleSecondaryColorScroller = new UIMenuNumericScrollerItem<int>("Secondary Color", "Set the secondary color selected", 0, 159, 1);
        vehicleSecondaryColorScroller.Value = vehicleSecondaryColorScroller.Minimum;
        vehicleSecondaryColorScroller.Activated += (sender, selectedItem) =>
        {
            selectedSecondaryColor = vehicleSecondaryColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, selectedPrimaryColor, selectedSecondaryColor);
                Game.DisplaySubtitle($"SET COLOR {selectedSecondaryColor}");
            }
        };
        colorSimpleMenu.AddItem(vehicleSecondaryColorScroller);



        UIMenuNumericScrollerItem<int> vehicleInteriorColorScroller = new UIMenuNumericScrollerItem<int>("Interior Color", "Set the interior color selected", 0, 159, 1);
        vehicleInteriorColorScroller.Value = vehicleInteriorColorScroller.Minimum;
        vehicleInteriorColorScroller.Activated += (sender, selectedItem) =>
        {
            selectedInteriorColor = vehicleInteriorColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(Player.InterestedVehicle.Vehicle, selectedInteriorColor);
                Game.DisplaySubtitle($"SET COLOR {selectedInteriorColor}");
            }
        };
        colorSimpleMenu.AddItem(vehicleInteriorColorScroller);

        UIMenuNumericScrollerItem<int> vehicleDashboardColorScroller = new UIMenuNumericScrollerItem<int>("Dashboard Color", "Set the dashboard color selected", 0, 159, 1);
        vehicleDashboardColorScroller.Value = vehicleDashboardColorScroller.Minimum;
        vehicleDashboardColorScroller.Activated += (sender, selectedItem) =>
        {
            selectedDashboardColor = vehicleDashboardColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(Player.InterestedVehicle.Vehicle, selectedDashboardColor);
                Game.DisplaySubtitle($"SET COLOR {selectedDashboardColor}");
            }
        };
        colorSimpleMenu.AddItem(vehicleDashboardColorScroller);




        UIMenuItem setWHiteMenu = new UIMenuItem("Set Color White", "Set Color White");
        setWHiteMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, 134, 134);
            }
        };
        colorSimpleMenu.AddItem(setWHiteMenu);










    }
    private void CreateBetterColorMenu()
    {
        //Color Stuff Here
        UIMenu colorFullMenu = MenuPool.AddSubMenu(vehicleItemsMenu, "Colors");
        colorFullMenu.SubtitleText = "COLORS";
        vehicleItemsMenu.MenuItems[vehicleItemsMenu.MenuItems.Count() - 1].Description = "Pick Colors";









        UIMenu primaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Primary Color");
        primaryColorMenu.SubtitleText = "PRIMARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Primary Colors";


        UIMenu secondaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Secondary Color");
        secondaryColorMenu.SubtitleText = "SECONDARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Secondary Colors";


        UIMenu pearlescentColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Pearlescent Color");
        pearlescentColorMenu.SubtitleText = "PEARLESCENT COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Pearlescent Colors";

        UIMenu wheelColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Wheel Color");
        wheelColorMenu.SubtitleText = "WHEEL COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Wheel Colors";

        UIMenu interiorColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Interior Color");
        interiorColorMenu.SubtitleText = "INTERIOR COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Interior Colors";

        //Add Color Sub Menu Here
        foreach (string colorGroupString in VehicleColors.GroupBy(x => x.ColorGroup).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            UIMenu primarycolorGroupMenu = MenuPool.AddSubMenu(primaryColorMenu, colorGroupString);
            primarycolorGroupMenu.SubtitleText = "PRIMARY COLORS";
            primaryColorMenu.MenuItems[primaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";


            UIMenu secondarycolorGroupMenu = MenuPool.AddSubMenu(secondaryColorMenu, colorGroupString);
            secondarycolorGroupMenu.SubtitleText = "SECONDARY COLORS";
            secondaryColorMenu.MenuItems[secondaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";


            UIMenu pearlescentcolorGroupMenu = MenuPool.AddSubMenu(pearlescentColorMenu, colorGroupString);
            pearlescentcolorGroupMenu.SubtitleText = "PEARLESCENT COLORS";
            pearlescentColorMenu.MenuItems[pearlescentColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";


            UIMenu wheelcolorGroupMenu = MenuPool.AddSubMenu(wheelColorMenu, colorGroupString);
            wheelcolorGroupMenu.SubtitleText = "WHEEL COLORS";
            wheelColorMenu.MenuItems[wheelColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";

            UIMenu interiorcolorGroupMenu = MenuPool.AddSubMenu(interiorColorMenu, colorGroupString);
            interiorcolorGroupMenu.SubtitleText = "INTERIOR COLORS";
            interiorColorMenu.MenuItems[interiorColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";

            foreach (VehicleColorLookup cl in VehicleColors.Where(x => x.ColorGroup == colorGroupString))
            {
                UIMenuItem actualColorPrimary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorPrimary.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorPrimary.RightBadgeInfo.Color = cl.RGBColor;
                actualColorPrimary.Activated += (sender, selectedItem) =>
                {
                    SetPrimaryColor = true;
                    PrimaryColor = cl.ColorID;
                    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, FinalPrimaryColor, FinalSecondaryColor);
                        DisplayColor();
                    }
                };
                primarycolorGroupMenu.AddItem(actualColorPrimary);
                UIMenuItem actualColorSecondary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorSecondary.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorSecondary.RightBadgeInfo.Color = cl.RGBColor;
                actualColorSecondary.Activated += (sender, selectedItem) =>
                {
                    SetSecondaryColor = true;
                    SecondaryColor = cl.ColorID;
                    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, FinalPrimaryColor, FinalSecondaryColor);
                        DisplayColor();
                    }
                };
                secondarycolorGroupMenu.AddItem(actualColorSecondary);



                UIMenuItem actualColorPearl = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorPearl.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorPearl.RightBadgeInfo.Color = cl.RGBColor;
                actualColorPearl.Activated += (sender, selectedItem) =>
                {
                    SetPearlescentColor = true;
                    PearlescentColor = cl.ColorID;
                    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(Player.InterestedVehicle.Vehicle, FinalPearlColor, FinalWheelColor);
                        DisplayColor();
                    }
                };
                pearlescentcolorGroupMenu.AddItem(actualColorPearl);



                UIMenuItem actualColorWheel = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorWheel.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorWheel.RightBadgeInfo.Color = cl.RGBColor;
                actualColorWheel.Activated += (sender, selectedItem) =>
                {
                    SetWheelColor = true;
                    WheelColor = cl.ColorID;
                    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(Player.InterestedVehicle.Vehicle, FinalPearlColor, FinalWheelColor);
                        DisplayColor();
                    }
                };
                wheelcolorGroupMenu.AddItem(actualColorWheel);


                UIMenuItem actualColorInterior = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorInterior.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorInterior.RightBadgeInfo.Color = cl.RGBColor;
                actualColorInterior.Activated += (sender, selectedItem) =>
                {
                    SetInteriorColor = true;
                    InteriorColor = cl.ColorID;
                    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(Player.InterestedVehicle.Vehicle, FinalInteriorColor);
                        DisplayColor();
                    }
                };
                interiorcolorGroupMenu.AddItem(actualColorInterior);

            }
        }
    }
    private void DisplayColor()
    {
        Game.DisplaySubtitle($"Primary {FinalPrimaryColor} Secondary {FinalSecondaryColor} PearlColor {FinalPearlColor} WheelColor {FinalWheelColor} InteriorColor {FinalInteriorColor}");
    }
    private void CreateLiveryMenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.InterestedVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery", "Set the vehicle Livery", 0, Total - 1, 1) ;
        LogLocationMenu.Value = LogLocationMenu.Minimum;
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle.Vehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.InterestedVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY {LogLocationMenu.Value}");
            }
            
        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }
    private void CreateLivery2MenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY2_COUNT<int>(Player.InterestedVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery 2", "Set the vehicle Livery 2", 0, Total - 1, 1);
        LogLocationMenu.Value = LogLocationMenu.Minimum;
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY2(Player.InterestedVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY 2 {LogLocationMenu.Value}");
            }

        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }
    private void CreatePlateMenuItem()
    {
        UIMenuListScrollerItem<PlateType> plateIndex = new UIMenuListScrollerItem<PlateType>("Plate Type", "Select Plate Type to change", PlateTypes.PlateTypeManager.PlateTypeList);
        plateIndex.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                PlateType NewType = PlateTypes.GetPlateType(plateIndex.SelectedItem.Index);
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Player.InterestedVehicle.Vehicle.LicensePlate = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.InterestedVehicle.Vehicle, NewType.Index);
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index}, Index: {NewType.Index}, State: {NewType.StateID}, Description: {NewType.Description}");
                }
                else
                {
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index} None Found");
                }
            }

        };
        vehicleItemsMenu.AddItem(plateIndex);




        UIMenuItem setRandomPlateType = new UIMenuItem("Set Random Plate Type", "Set random plate type");
        setRandomPlateType.Activated += (menu, item) =>
        {

            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                PlateType NewType = PlateTypes.GetRandomPlateType(Player.InterestedVehicle.IsMotorcycle);
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Player.InterestedVehicle.Vehicle.LicensePlate = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.InterestedVehicle.Vehicle, NewType.Index);
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index}, Index: {NewType.Index}, State: {NewType.StateID}, Description: {NewType.Description}");
                }
                else
                {
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index} None Found");
                }
            }
        };
        vehicleItemsMenu.AddItem(setRandomPlateType);




        UIMenuItem setPlateNumber = new UIMenuItem("Set Plate Number", "Set playe number from input");
        setPlateNumber.Activated += (menu, item) =>
        {

            string newplateText = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(newplateText))
            {
                return;
            }


            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                Player.InterestedVehicle.Vehicle.LicensePlate = newplateText;
            }
        };
        vehicleItemsMenu.AddItem(setPlateNumber);


    }
    private void CreateInfoMenuItem()
    {
        UIMenuItem vehInfoMenu = new UIMenuItem("Get Info", "Print info");
        vehInfoMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                string toDisplay = $"PlateNumber:{Player.InterestedVehicle.CarPlate.PlateNumber} PlateType:{Player.InterestedVehicle.CarPlate.PlateType} IsWanted:{Player.InterestedVehicle.CarPlate.IsWanted} " +
                $"PlateNumber2:{Player.InterestedVehicle.OriginalLicensePlate.PlateNumber} PlateType2:{Player.InterestedVehicle.OriginalLicensePlate.PlateType} IsWanted2:{Player.InterestedVehicle.OriginalLicensePlate.IsWanted}";
                Game.DisplaySubtitle(toDisplay);
                EntryPoint.WriteToConsole(toDisplay);
            }
        };
        vehicleItemsMenu.AddItem(vehInfoMenu);
    }



}


