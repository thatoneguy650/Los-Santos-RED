using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


public class VehicleColorMenu
{
    private MenuPool MenuPool;
    private UIMenu VehicleHeaderMenu;
    private ILocationInteractable Player;
    private VehicleExt ModdingVehicle;
    private VehicleVariation CurrentVariation;
    private GameLocation GameLocation;
    private ModShopMenu ModShopMenu;

    private List<VehicleColorLookup> VehicleColors;
    private UIMenu primaryColorMenu;
    private UIMenu primarycolorGroupMenu;
    private UIMenu secondarycolorGroupMenu;
    private UIMenu secondaryColorMenu;
    private UIMenu pearlescentColorMenu;
    private UIMenu wheelColorMenu;
    private UIMenu interiorColorMenu;
    private UIMenu dashboardColorMenu;
    private UIMenu pearlescentcolorGroupMenu;
    private UIMenu wheelcolorGroupMenu;
    private UIMenu interiorcolorGroupMenu;
    private UIMenu dashboardcolorGroupMenu;
    private UIMenu colorFullMenu;
    private UIMenu xenonColorMenu;
    private UIMenu neonColorMenu;

    private List<ColorMenuItem> PrimaryColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem> SecondaryColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem> PearlColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem> WheelColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem> InteriorColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem> DashboardColorMenuItems = new List<ColorMenuItem>();
    private List<ColorMenuItem>[] NeonMenuItems = new List<ColorMenuItem>[4];
    public VehicleColorMenu(MenuPool menuPool, UIMenu vehicleHeaderMenu, ILocationInteractable player, VehicleExt moddingVehicle, ModShopMenu modShopMenu, VehicleVariation currentVariation, GameLocation gameLocation)
    {
        MenuPool = menuPool;
        VehicleHeaderMenu = vehicleHeaderMenu;
        Player = player;
        ModdingVehicle = moddingVehicle;
        CurrentVariation = currentVariation;
        GameLocation = gameLocation;
        ModShopMenu = modShopMenu;
    }
    public void Setup()
    {
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
        PrimaryColorMenuItems = new List<ColorMenuItem>();
        CreateBetterColorMenu();
        SetupNeonColorMenu();
        SetupXenonColorMenu();
    }
    private readonly Dictionary<int, string> XenonColors = new Dictionary<int, string>
    {
        { 0, "White" }, { 1, "Blue" }, { 2, "Electric Blue" },
        { 3, "Mint Green" }, { 4, "Lime Green" }, { 5, "Yellow" },
        { 6, "Golden Shower" }, { 7, "Orange" }, { 8, "Red" },
        { 9, "Pony Pink" }, { 10, "Hot Pink" }, { 11, "Purple" },
        { 12, "Blacklight" }
    };

    private List<VehicleColorLookup> NeonColors = new List<VehicleColorLookup>()
    {
        new VehicleColorLookup(0, "White", "Neon", "White", 0) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 255) },
        new VehicleColorLookup(1, "Blue", "Neon", "Blue", 1) { RGBColor = System.Drawing.Color.FromArgb(0, 0, 255) },
        new VehicleColorLookup(2, "Electric Blue", "Neon", "Electric Blue", 2) { RGBColor = System.Drawing.Color.FromArgb(0, 150, 255) },
        new VehicleColorLookup(3, "Mint Green", "Neon", "Mint Green", 3) { RGBColor = System.Drawing.Color.FromArgb(50, 255, 155) },
        new VehicleColorLookup(4, "Lime Green", "Neon", "Lime Green", 4) { RGBColor = System.Drawing.Color.FromArgb(100, 255, 0) },
        new VehicleColorLookup(5, "Yellow", "Neon", "Yellow", 5) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 0) },
        new VehicleColorLookup(6, "Golden Shower", "Neon", "Golden Shower", 6) { RGBColor = System.Drawing.Color.FromArgb(204, 204, 0) },
        new VehicleColorLookup(7, "Orange", "Neon", "Orange", 7) { RGBColor = System.Drawing.Color.FromArgb(255, 128, 0) },
        new VehicleColorLookup(8, "Red", "Neon", "Red", 8) { RGBColor = System.Drawing.Color.FromArgb(255, 0, 0) },
        new VehicleColorLookup(9, "Pony Pink", "Neon", "Pony Pink", 9) { RGBColor = System.Drawing.Color.FromArgb(255, 102, 204) },
        new VehicleColorLookup(10, "Hot Pink", "Neon", "Hot Pink", 10) { RGBColor = System.Drawing.Color.FromArgb(255, 0, 255) },
        new VehicleColorLookup(11, "Purple", "Neon", "Purple", 11) { RGBColor = System.Drawing.Color.FromArgb(153, 0, 153) },
        new VehicleColorLookup(12, "Blacklight", "Neon", "Blacklight", 12){RGBColor = System.Drawing.Color.FromArgb(15, 3, 255) }
    };
    private void CreateBetterColorMenu()
    {
        CreateTypeSubMenus();

        //Add Color Sub Menu Here
        int counter = 0;
        foreach (string colorGroupString in VehicleColors.GroupBy(x => x.ColorGroup).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            SetupPrimaryColors(colorGroupString);
            SetupSecondaryColors(colorGroupString);
            SetupPearlColors(colorGroupString);
            SetupWheelColors(colorGroupString);
            SetupInteriorColors(colorGroupString);
            SetupDashboardColors(colorGroupString);
            foreach (VehicleColorLookup cl in VehicleColors.Where(x => x.ColorGroup == colorGroupString))
            {
                int colorPrice = GetColorPrice(cl.ColorID);
                UIMenuItem actualColorPrimary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                PrimaryColorMenuItems.Add(new ColorMenuItem(actualColorPrimary, cl.ColorID, counter));
                bool isSelectedPrimary = CurrentVariation.PrimaryColor == cl.ColorID;
                if(isSelectedPrimary)
                {
                    actualColorPrimary.RightLabel = "";
                    actualColorPrimary.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorPrimary.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorPrimary.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorPrimary.Activated += (sender, selectedItem) =>
                {
                    if(CurrentVariation != null && CurrentVariation.PrimaryColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Primary");
                        return;
                    }
                    if(!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetPrimaryColor(cl.ColorID, true);
                };
                primarycolorGroupMenu.AddItem(actualColorPrimary);





                UIMenuItem actualColorSecondary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                SecondaryColorMenuItems.Add(new ColorMenuItem(actualColorSecondary, cl.ColorID, counter));
                bool isSelectedSecondary = CurrentVariation.SecondaryColor == cl.ColorID;
                if (isSelectedSecondary)
                {
                    actualColorSecondary.RightLabel = "";
                    actualColorSecondary.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorSecondary.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorSecondary.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorSecondary.Activated += (sender, selectedItem) =>
                {
                    if (CurrentVariation != null && CurrentVariation.SecondaryColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Secondary");
                        return;
                    }
                    if (!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetSecondaryColor(cl.ColorID, true);
                };
                secondarycolorGroupMenu.AddItem(actualColorSecondary);





                UIMenuItem actualColorPearl = new UIMenuItem(cl.ColorName, cl.FullColorName);
                PearlColorMenuItems.Add(new ColorMenuItem(actualColorPearl, cl.ColorID, counter));
                bool isSelectedPearl = CurrentVariation.PearlescentColor == cl.ColorID;
                if (isSelectedPearl)
                {
                    actualColorPearl.RightLabel = "";
                    actualColorPearl.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorPearl.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorPearl.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorPearl.Activated += (sender, selectedItem) =>
                {
                    if (CurrentVariation != null && CurrentVariation.PearlescentColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Pearlescent");
                        return;
                    }
                    if (!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetPearlColor(cl.ColorID, true);
                };
                pearlescentcolorGroupMenu.AddItem(actualColorPearl);





                UIMenuItem actualColorWheel = new UIMenuItem(cl.ColorName, cl.FullColorName);
                WheelColorMenuItems.Add(new ColorMenuItem(actualColorWheel, cl.ColorID, counter));
                bool isSelectedWheel = CurrentVariation.WheelColor == cl.ColorID;
                if (isSelectedWheel)
                {
                    actualColorWheel.RightLabel = "";
                    actualColorWheel.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorWheel.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorWheel.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorWheel.Activated += (sender, selectedItem) =>
                {
                    if (CurrentVariation != null && CurrentVariation.WheelColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Wheel");
                        return;
                    }
                    if (!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetWheelColor(cl.ColorID, true);
                };
                wheelcolorGroupMenu.AddItem(actualColorWheel);




                UIMenuItem actualColorInterior = new UIMenuItem(cl.ColorName, cl.FullColorName);
                InteriorColorMenuItems.Add(new ColorMenuItem(actualColorInterior, cl.ColorID, counter));
                bool isSelectedInterior = CurrentVariation.InteriorColor == cl.ColorID;
                if (isSelectedInterior)
                {
                    actualColorInterior.RightLabel = "";
                    actualColorInterior.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorInterior.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorInterior.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorInterior.Activated += (sender, selectedItem) =>
                {
                    if (CurrentVariation != null && CurrentVariation.InteriorColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Interior");
                        return;
                    }
                    if (!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetInteriorColor(cl.ColorID,true);
                };
                interiorcolorGroupMenu.AddItem(actualColorInterior);





                UIMenuItem actualColorDashboard = new UIMenuItem(cl.ColorName, cl.FullColorName);
                DashboardColorMenuItems.Add(new ColorMenuItem(actualColorDashboard, cl.ColorID, counter));
                bool isSelectedDashboard = CurrentVariation.DashboardColor == cl.ColorID;
                if (isSelectedDashboard)
                {
                    actualColorDashboard.RightLabel = "";
                    actualColorDashboard.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    actualColorDashboard.RightLabel = $"~r~${GetColorPrice(cl.ColorID)}~s~";
                    actualColorDashboard.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                actualColorDashboard.Activated += (sender, selectedItem) =>
                {
                    if (CurrentVariation != null && CurrentVariation.DashboardColor == cl.ColorID)
                    {
                        DisplayMessage("Already Set as Dashboard");
                        return;
                    }
                    if (!ChargeClient(colorPrice))
                    {
                        return;
                    }
                    SetInteriorColor(cl.ColorID, true);
                };
                dashboardcolorGroupMenu.AddItem(actualColorDashboard);


                colorFullMenu.OnMenuOpen += (sender) =>
                {
                    if (CurrentVariation == null)
                        return;

                    // Only build Xenon menu if it hasn't been created yet
                    SetupXenonColorMenu();

                    // Only build Neon menu if it hasn't been created yet
                    SetupNeonColorMenu();
                };




                counter++;

            }
        }
    }

    private void DisplayMessage(string v)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlaySuccessSound();
        GameLocation.DisplayMessage("Information", v);
    }

    private bool ChargeClient(int price)
    {
        if(Player.BankAccounts.GetMoney(true) < price)
        {
            DisplayNotEnoughFunds(price);
            return false;
        }
        DisplayPurchased(price);
        Player.BankAccounts.GiveMoney(-1 * price, true);
        return true;
    }
    private void DisplayNotEnoughFunds(int price)
    {
        if(GameLocation == null)
        {
            return;
        }
        GameLocation.PlayErrorSound();
        GameLocation.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
    }
    private void DisplayPurchased(int price)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlaySuccessSound();
        GameLocation.DisplayMessage("~g~Purchased", $"Thank you for your purchase");
    }
    private void CreateTypeSubMenus()
    {
        //Color Stuff Here
        colorFullMenu = MenuPool.AddSubMenu(VehicleHeaderMenu, "Colors");
        colorFullMenu.SetBannerType(EntryPoint.LSRedColor);
        colorFullMenu.SubtitleText = "COLORS";
        VehicleHeaderMenu.MenuItems[VehicleHeaderMenu.MenuItems.Count() - 1].Description = "Pick Colors";

        primaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Primary Color");
        primaryColorMenu.SetBannerType(EntryPoint.LSRedColor);
        primaryColorMenu.SubtitleText = "PRIMARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Primary Colors";

        secondaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Secondary Color");
        secondaryColorMenu.SetBannerType(EntryPoint.LSRedColor);
        secondaryColorMenu.SubtitleText = "SECONDARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Secondary Colors";

        pearlescentColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Pearlescent Color");
        pearlescentColorMenu.SetBannerType(EntryPoint.LSRedColor);
        pearlescentColorMenu.SubtitleText = "PEARLESCENT COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Pearlescent Colors";

        wheelColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Wheel Color");
        wheelColorMenu.SetBannerType(EntryPoint.LSRedColor);
        wheelColorMenu.SubtitleText = "WHEEL COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Wheel Colors";

        interiorColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Interior Color");
        interiorColorMenu.SetBannerType(EntryPoint.LSRedColor);
        interiorColorMenu.SubtitleText = "INTERIOR COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Interior Colors";

        dashboardColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Dashboard Color");
        dashboardColorMenu.SetBannerType(EntryPoint.LSRedColor);
        dashboardColorMenu.SubtitleText = "DASHBOARD COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Dashboard Colors";
    }



    private void SetupPrimaryColors(string colorGroupString)
    {
        primarycolorGroupMenu = MenuPool.AddSubMenu(primaryColorMenu, colorGroupString);
        primarycolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        primarycolorGroupMenu.SubtitleText = "PRIMARY COLORS";
        primaryColorMenu.MenuItems[primaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
        primarycolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = PrimaryColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if(lookupResult == null)
            {
                return;
            }
            SetPrimaryColor(lookupResult.ColorID, false);
        };
        primarycolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        primarycolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = PrimaryColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetPrimaryColor(lookupResult.ColorID, false);
        };
    }
    private void SetPrimaryColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        int secondaryColor = 0;
        if (CurrentVariation != null)
        {
            secondaryColor = CurrentVariation.SecondaryColor;
            if (setVariation)
            {
                CurrentVariation.PrimaryColor = colorID;
                SyncPrimaryColors(colorID);
            }
        }
        NativeFunction.Natives.SET_VEHICLE_COLOURS(ModdingVehicle.Vehicle, colorID, secondaryColor);
    }
    private void SyncPrimaryColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in PrimaryColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }











    private void SetupSecondaryColors(string colorGroupString)
    {
        secondarycolorGroupMenu = MenuPool.AddSubMenu(secondaryColorMenu, colorGroupString);
        secondarycolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        secondarycolorGroupMenu.SubtitleText = "SECONDARY COLORS";
        secondaryColorMenu.MenuItems[secondaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
        secondarycolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = SecondaryColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetSecondaryColor(lookupResult.ColorID, false);
        };
        secondarycolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        secondarycolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if(newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = SecondaryColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetSecondaryColor(lookupResult.ColorID, false);
        };
    }
    private void SetSecondaryColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        int primaryColor = 0;
        if (CurrentVariation != null)
        {
            primaryColor = CurrentVariation.PrimaryColor;
            if (setVariation)
            {
                CurrentVariation.SecondaryColor = colorID;
                SyncSecondaryColors(colorID);
            }
        }
        NativeFunction.Natives.SET_VEHICLE_COLOURS(ModdingVehicle.Vehicle, primaryColor, colorID);
    }
    private void SyncSecondaryColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in SecondaryColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }


    private void SetupPearlColors(string colorGroupString)
    {
        pearlescentcolorGroupMenu = MenuPool.AddSubMenu(pearlescentColorMenu, colorGroupString);
        pearlescentcolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        pearlescentcolorGroupMenu.SubtitleText = "PEARLESCENT COLORS";
        pearlescentColorMenu.MenuItems[pearlescentColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";

        pearlescentcolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = PearlColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetPearlColor(lookupResult.ColorID, false);
        };
        pearlescentcolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        pearlescentcolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = PearlColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetPearlColor(lookupResult.ColorID, false);
        };

    }
    private void SetPearlColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        int wheelColor = 0;
        if (CurrentVariation != null)
        {
            wheelColor = CurrentVariation.WheelColor;
            if (setVariation)
            {
                CurrentVariation.PearlescentColor = colorID;
                SyncPearlColors(colorID);
            }
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(ModdingVehicle.Vehicle, colorID, wheelColor);
    }
    private void SyncPearlColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in PearlColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }

    private void SetupWheelColors(string colorGroupString)
    {
        wheelcolorGroupMenu = MenuPool.AddSubMenu(wheelColorMenu, colorGroupString);
        wheelcolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        wheelcolorGroupMenu.SubtitleText = "WHEEL COLORS";
        wheelColorMenu.MenuItems[wheelColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
        wheelcolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = WheelColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetWheelColor(lookupResult.ColorID, false);
        };
        wheelcolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        wheelcolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = WheelColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetWheelColor(lookupResult.ColorID, false);
        };
    }
    private void SetWheelColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        int pearlColor = 0;
        if (CurrentVariation != null)
        {
            pearlColor = CurrentVariation.PearlescentColor;
            if (setVariation)
            {
                CurrentVariation.WheelColor = colorID;
                SyncWheelColors(colorID);
            }
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(ModdingVehicle.Vehicle, pearlColor, colorID);
    }
    private void SyncWheelColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in WheelColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }



    private void SetupInteriorColors(string colorGroupString)
    {
        interiorcolorGroupMenu = MenuPool.AddSubMenu(interiorColorMenu, colorGroupString);
        interiorcolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        interiorcolorGroupMenu.SubtitleText = "INTERIOR COLORS";
        interiorColorMenu.MenuItems[interiorColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
        interiorcolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = InteriorColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetInteriorColor(lookupResult.ColorID, false);
        };
        interiorcolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        interiorcolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = InteriorColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetInteriorColor(lookupResult.ColorID, false);
        };
    }
    private void SetInteriorColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        if (CurrentVariation != null && setVariation)
        {
            CurrentVariation.InteriorColor = colorID;
            SyncInteriorColors(colorID);
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(ModdingVehicle.Vehicle, colorID);
    }
    private void SyncInteriorColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in InteriorColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }


    private void SetupDashboardColors(string colorGroupString)
    {
        dashboardcolorGroupMenu = MenuPool.AddSubMenu(dashboardColorMenu, colorGroupString);
        dashboardcolorGroupMenu.SetBannerType(EntryPoint.LSRedColor);
        dashboardcolorGroupMenu.SubtitleText = "DASHBOARD COLORS";
        dashboardColorMenu.MenuItems[dashboardColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
        dashboardcolorGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetColors();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = DashboardColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetDashboardColor(lookupResult.ColorID, false);
        };
        dashboardcolorGroupMenu.OnMenuClose += (sender) =>
        {
            ResetColors();
        };
        dashboardcolorGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            ColorMenuItem lookupResult = DashboardColorMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetDashboardColor(lookupResult.ColorID, false);
        };
    }
    private void SetDashboardColor(int colorID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        if (CurrentVariation != null && setVariation)
        {
            CurrentVariation.DashboardColor = colorID;
            SyncDashboardColors(colorID);
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(ModdingVehicle.Vehicle, colorID);
    }
    private void SyncDashboardColors(int colorID)
    {
        foreach (ColorMenuItem colorMenuItem in DashboardColorMenuItems)
        {
            if (colorMenuItem.ColorID == colorID)
            {
                colorMenuItem.UIMenuItem.RightLabel = "";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                colorMenuItem.UIMenuItem.RightLabel = $"~r~${GetColorPrice(colorMenuItem.ColorID)}~s~";
                colorMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

        }
    }

    private void ResetColors()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists() || CurrentVariation == null)
        {
            return;
        }
        NativeFunction.Natives.SET_VEHICLE_COLOURS(ModdingVehicle.Vehicle, CurrentVariation.PrimaryColor, CurrentVariation.SecondaryColor);
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(ModdingVehicle.Vehicle, CurrentVariation.PearlescentColor, CurrentVariation.WheelColor);
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(ModdingVehicle.Vehicle, CurrentVariation.InteriorColor);
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(ModdingVehicle.Vehicle, CurrentVariation.DashboardColor);
    }
    //private int GetColorPrice(VehicleColorLookup cl)
    //{
    //    return 500;
    //}
    private int GetColorPrice(int colorID)
    {
        if(ModShopMenu == null)
        {
            return 500;
        }
        if(ModShopMenu.VehicleVariationShopMenu == null || ModShopMenu.VehicleVariationShopMenu.VehicleColorShopMenuItems == null)
        {
            return ModShopMenu.DefaultPrice;
        }
        VehicleColorShopMenuItem vsmci = ModShopMenu.VehicleVariationShopMenu.VehicleColorShopMenuItems.Where(x => x.ColorID == colorID).FirstOrDefault();
        if(vsmci != null)
        {
            return vsmci.Price;
        }
        return ModShopMenu.DefaultPrice;
    }

    private class ColorMenuItem
    {
        public ColorMenuItem(UIMenuItem uIMenuItem, int iD, int index)
        {
            UIMenuItem = uIMenuItem;
            ColorID = iD;
            Index = index;
        }

        public UIMenuItem UIMenuItem { get; set; }
        public int ColorID { get; set; }
        public int Index { get; set; }
    }

    // Xenon and Neon Color Menu Setup
    // Xenon Color Menu Setup
    private void SetupXenonColorMenu()
    {
        if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists()) return;

        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);

        bool hasXenonsSaved = CurrentVariation.VehicleMods.Any(m => m.ID == 22 && m.Output == 1);
        bool hasXenonsActual = NativeFunction.Natives.IS_TOGGLE_MOD_ON<bool>(ModdingVehicle.Vehicle, 22);
        bool hasXenons = hasXenonsSaved || hasXenonsActual;

        if (!hasXenonsSaved && hasXenonsActual)
        {
            var mod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == 22);
            if (mod != null)
                mod.Output = 1;
            else
                CurrentVariation.VehicleMods.Add(new VehicleMod(22, 1));
        }

        if (xenonColorMenu == null)
        {
            xenonColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Xenon Headlight Color");
            xenonColorMenu.SetBannerType(EntryPoint.LSRedColor);
            xenonColorMenu.SubtitleText = "XENON COLORS";
        }
        else
        {
            xenonColorMenu.Clear();
        }

        int originalColor = CurrentVariation.XenonLightColor >= 0 ? CurrentVariation.XenonLightColor : 0;

        foreach (var kvp in XenonColors)
        {
            int colorID = kvp.Key;
            string colorName = kvp.Value;

            var item = new UIMenuItem(colorName, hasXenons ? $"Set Xenon headlights to {colorName}" : "You need Xenons installed first");
            item.Enabled = hasXenons;

            bool active = hasXenons && originalColor == colorID;

            item.RightBadge = active ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;
            item.RightLabel = active ? "" : (hasXenons ? "~r~$500~s~" : "");

            item.Activated += (sender, selectedItem) =>
            {
                if (!hasXenons)
                {
                    DisplayMessage("You need Xenons installed first!");
                    return;
                }

                if (originalColor == colorID)
                {
                    DisplayMessage("Already Set");
                    return;
                }

                if (!ChargeClient(500)) return;

                CurrentVariation.XenonLightColor = colorID;
                try { NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(ModdingVehicle.Vehicle, colorID); }
                catch { }

                // Inline sync
                foreach (UIMenuItem menuItem in xenonColorMenu.MenuItems)
                {
                    int menuID = XenonColors.First(x => x.Value == menuItem.Text).Key;
                    menuItem.RightBadge = menuID == colorID ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;
                    menuItem.RightLabel = menuID == colorID ? "" : "~r~$500~s~";
                }

                DisplayMessage($"Xenon headlights set to {colorName}");
            };

            xenonColorMenu.AddItem(item);
        }

        xenonColorMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1 || !hasXenons) return;

            int previewID = XenonColors.First(x => x.Value == sender.MenuItems[newIndex].Text).Key;
            try { NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(ModdingVehicle.Vehicle, previewID); }
            catch { }
        };

        xenonColorMenu.OnMenuClose += (sender) =>
        {
            if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists()) return;
            try { NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(ModdingVehicle.Vehicle, originalColor); }
            catch { }
        };
    }
    // Neon Color Menu Setup
    private void SetupNeonColorMenu()
    {
        if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists()) return;

        bool hasNeons = CurrentVariation.VehicleNeons.Any(n => n.IsEnabled);

        if (neonColorMenu == null)
        {
            neonColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Neon Lights Color");
            neonColorMenu.SetBannerType(EntryPoint.LSRedColor);
            neonColorMenu.SubtitleText = "NEON COLORS";
        }
        else
        {
            neonColorMenu.Clear();
        }

        if (NeonMenuItems[0] == null) NeonMenuItems[0] = new List<ColorMenuItem>();

        int originalR = CurrentVariation.NeonColorR;
        int originalG = CurrentVariation.NeonColorG;
        int originalB = CurrentVariation.NeonColorB;

        foreach (VehicleColorLookup color in NeonColors)
        {
            UIMenuItem item = new UIMenuItem(color.ColorName, hasNeons ? "Set neon color to " + color.ColorName : "You need Neons installed first");
            item.Enabled = hasNeons;

            bool active = hasNeons && CurrentVariation != null &&
                          CurrentVariation.NeonColorR == color.RGBColor.R &&
                          CurrentVariation.NeonColorG == color.RGBColor.G &&
                          CurrentVariation.NeonColorB == color.RGBColor.B;

            item.RightBadge = active ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;
            item.RightLabel = active ? "" : (hasNeons ? "~r~$1500~s~" : "");

            var cmi = new ColorMenuItem(item, color.ColorID, color.ColorID);
            NeonMenuItems[0].Add(cmi);
            neonColorMenu.AddItem(item);

            item.Activated += (sender, selectedItem) =>
            {
                if (!hasNeons)
                {
                    DisplayMessage("You need Neons installed first!");
                    return;
                }

                bool isAlreadySet = CurrentVariation.NeonColorR == color.RGBColor.R &&
                                    CurrentVariation.NeonColorG == color.RGBColor.G &&
                                    CurrentVariation.NeonColorB == color.RGBColor.B;

                if (isAlreadySet)
                {
                    DisplayMessage("Already Set as Neon Color");
                    return;
                }

                if (!ChargeClient(500)) return;

                CurrentVariation.NeonColorR = color.RGBColor.R;
                CurrentVariation.NeonColorG = color.RGBColor.G;
                CurrentVariation.NeonColorB = color.RGBColor.B;

                foreach (VehicleNeon vehicleNeon in CurrentVariation.VehicleNeons)
                {
                    try { NativeFunction.Natives.SET_VEHICLE_NEON_ENABLED(ModdingVehicle.Vehicle, vehicleNeon.ID, vehicleNeon.IsEnabled); }
                    catch { }
                }

                try { NativeFunction.Natives.SET_VEHICLE_NEON_COLOUR(ModdingVehicle.Vehicle, CurrentVariation.NeonColorR, CurrentVariation.NeonColorG, CurrentVariation.NeonColorB); }
                catch { }

                foreach (var menuItem in NeonMenuItems[0])
                {
                    bool isNowActive = menuItem.ColorID == color.ColorID;
                    menuItem.UIMenuItem.RightBadge = isNowActive ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;
                    menuItem.UIMenuItem.RightLabel = isNowActive ? "" : "~r~$1500~s~";
                }

                DisplayMessage("Neon color updated");
            };
        }

        neonColorMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists() || newIndex == -1) return;
            if (!hasNeons) return;

            UIMenuItem selected = sender.MenuItems[newIndex];
            ColorMenuItem lookup = NeonMenuItems[0].FirstOrDefault(x => x.UIMenuItem == selected);
            if (lookup == null) return;

            VehicleColorLookup col = NeonColors.FirstOrDefault(c => c.ColorID == lookup.ColorID);
            if (col == null) return;

            try { NativeFunction.Natives.SET_VEHICLE_NEON_COLOUR(ModdingVehicle.Vehicle, col.RGBColor.R, col.RGBColor.G, col.RGBColor.B); }
            catch { }
        };

        neonColorMenu.OnMenuClose += (sender) =>
        {
            if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists()) return;

            try { NativeFunction.Natives.SET_VEHICLE_NEON_COLOUR(ModdingVehicle.Vehicle, originalR, originalG, originalB); }
            catch { }
        };
    }
}
