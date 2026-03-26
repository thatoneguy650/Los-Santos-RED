using Rage;
using System;
using System.Collections.Generic;


public class ShopMenus_Clothing
{
    private PedClothingShopMenuItem MaskHockey_MPC;
    private PedClothingShopMenuItem MaskMonkey_MPC;
    private PedClothingShopMenuItem MaskLuchador_MPC;
    private PedClothingShopMenuItem MaskGargoyle1_MPC;
    private PedClothingShopMenuItem MaskBallistic_MPC;
    private PedClothingShopMenuItem MaskCombatMask_MPC;
    private PedClothingShopMenuItem MaskPainted_MPC;
    private PedClothingShopMenuItem MaskNightVision_MPC;
    private PedClothingShopMenuItem MaskSnakeSkull_MPC;
    private PedClothingShopMenuItem MaskSpecOps_MPC;
    private PedClothingShopMenuItem MaskFilter_MPC;
    private PedClothingShopMenuItem MaskVent_MPC;
    private PedClothingShopMenuItem MaskScruffyBalaclava_MPC;
    private PedClothingShopMenuItem MaskSkullScruffyBalaclava_MPC;
    private PedClothingShopMenuItem MaskBlackKnitBalaclava_MPC;
    private PedClothingShopMenuItem MaskBanditKnitBalaclava_MPC;
    private PedClothingShopMenuItem MaskBrightStripeKnitBalaclava_MPC;
    private PedClothingShopMenuItem MaskLooseBalaclava_MPC;
    private PedClothingShopMenuItem MaskBear_MPC;
    private PedClothingShopMenuItem MaskBison_MPC;
    private PedClothingShopMenuItem MaskBull_MPC;
    private PedClothingShopMenuItem MaskCat_MPC;
    private PedClothingShopMenuItem MaskCrazedApe_MPC;
    private PedClothingShopMenuItem MaskDino_MPC;
    private PedClothingShopMenuItem MaskEagle_MPC;
    private PedClothingShopMenuItem MaskFox_MPC;
    private PedClothingShopMenuItem MaskHorse_MPC;
    private PedClothingShopMenuItem MaskHyena_MPC;
    private PedClothingShopMenuItem MaskMouse_MPC;
    private PedClothingShopMenuItem MaskOwl_MPC;
    private PedClothingShopMenuItem MaskPig_MPC;
    private PedClothingShopMenuItem MaskPug_MPC;
    private PedClothingShopMenuItem MaskRaccoon_MPC;
    private PedClothingShopMenuItem MaskTurtle_MPC;
    private PedClothingShopMenuItem MaskWolf_MPC;
    private PedClothingShopMenuItem MaskUnicorn_MPC;
    private PedClothingShopMenuItem MaskVulture_MPC;
    private PedClothingShopMenuItem MaskClown_MPC;
    private PedClothingShopMenuItem MaskDeath_MPM;
    private PedClothingShopMenuItem MaskDeath_MPF;
    private PedClothingShopMenuItem MaskFalseFace_MPC;
    private PedClothingShopMenuItem MaskFamine_MPM;
    private PedClothingShopMenuItem MaskFamine_MPF;
    private PedClothingShopMenuItem MaskImpotentRage_MPC;
    private PedClothingShopMenuItem MaskMoorehead_MPC;
    private PedClothingShopMenuItem MaskPogo_MPC;
    private PedClothingShopMenuItem MaskPrincessRobotBubblegum_MPC;
    private PedClothingShopMenuItem MaskHockeyAlt_MPC;
    private PedClothingShopMenuItem MaskMandible_MPC;
    private PedClothingShopMenuItem MaskRobo_MPC;
    private PedClothingShopMenuItem MaskWarrior_MPC;
    private PedClothingShopMenuItem MaskCrimeSceneTape_MPC;
    private PedClothingShopMenuItem MaskDuctTape_MPC;
    private PedClothingShopMenuItem MaskFaceBandana_MPC;
    private PedClothingShopMenuItem MaskTShirtMask_MPC;

    private PedClothingShopMenuItem Aviators1_MPF;
    private PedClothingShopMenuItem GoldWatch1_MPF;
    private PedClothingShopMenuItem GoldWatch2_MPF;
    private PedClothingShopMenuItem DangleEarring1_MPF;
    private PedClothingShopMenuItem BaseballHatProLaps1_MPF;
    private PedClothingShopMenuItem BaseballHatRegular1_MPF;
    private PedClothingShopMenuItem BaseballHatRegular1Reverse_MPF;
    private PedClothingShopMenuItem StrawHat1_MPF;
    private PedClothingShopMenuItem Necklace1_MPF;
    private PedClothingShopMenuItem Necklace1_MPM;
    private PedClothingShopMenuItem DangleEarring1_MPM;
    private PedClothingShopMenuItem BaseballHatProLaps1_MPM;
    private PedClothingShopMenuItem BaseballHatProLaps1Reverse_MPM;
    private PedClothingShopMenuItem BaseballHatRegular1_MPM;
    private PedClothingShopMenuItem BaseballHatRegular1Reverse_MPM;
    private PedClothingShopMenuItem BrimmedHat1_MPM;
    private PedClothingShopMenuItem BrimmedHat2_MPM;
    private PedClothingShopMenuItem BrimmedHat3_MPM;
    private PedClothingShopMenuItem BowlerHat1_MPM;
    private PedClothingShopMenuItem StovetopHat1_MPM;
    private PedClothingShopMenuItem CowboyHat1_MPM;
    private PedClothingShopMenuItem Beanie1_MPM;
    private PedClothingShopMenuItem Beanie2_MPM;
    private PedClothingShopMenuItem Beanie3_MPM;
    private PedClothingShopMenuItem StrawHat1_MPM;
    private PedClothingShopMenuItem Aviators1_MPM;
    private PedClothingShopMenuItem Watch1_MPM;
    private PedClothingShopMenuItem Watch2_MPM;
    private PedClothingShopMenuItem Watch3_MPM;
    private PedClothingShopMenuItem DirtHelmet1_MPF;
    private PedClothingShopMenuItem DirtHelmet2_MPF;
    private PedClothingShopMenuItem HalfHelmet1_MPF;
    private PedClothingShopMenuItem FullHelmet1_MPF;
    private PedClothingShopMenuItem FullHelmet2_MPF;
    private PedClothingShopMenuItem FullHelmet3_MPF;
    private PedClothingShopMenuItem FullHelmet4_MPF;
    private PedClothingShopMenuItem FullHelmet5_MPF;
    private PedClothingShopMenuItem OpenFullHelmet1_MPF;
    private PedClothingShopMenuItem PilotHelmet1_MPF;
    private PedClothingShopMenuItem DirtHelmet1_MPM;
    private PedClothingShopMenuItem DirtHelmet2_MPM;
    private PedClothingShopMenuItem HalfHelmet1_MPM;
    private PedClothingShopMenuItem FullHelmet1_MPM;
    private PedClothingShopMenuItem FullHelmet2_MPM;
    private PedClothingShopMenuItem FullHelmet3_MPM;
    private PedClothingShopMenuItem FullHelmet4_MPM;
    private PedClothingShopMenuItem FullHelmet5_MPM;
    private PedClothingShopMenuItem OpenFullHelmet1_MPM;
    private PedClothingShopMenuItem PilotHelmet1_MPM;
    private PedClothingShopMenuItem BaseballHatBroker_MPM;
    private PedClothingShopMenuItem BaseballHatBrokerReverse_MPM;
    private PedClothingShopMenuItem BaseballHatEnus_MPM;
    private PedClothingShopMenuItem BaseballHatEnusReverse_MPM;
    private PedClothingShopMenuItem BaseballHatFastFood_MPM;
    private PedClothingShopMenuItem BaseballHatFastFoodReverse_MPM;
    private PedClothingShopMenuItem BaseballHatGuns_MPM;
    private PedClothingShopMenuItem BaseballHatGunsReverse_MPM;


    // Male Outfits

    //Perseus & Deider
    private PedClothingShopMenuItem BusinessOutfit1_MPM;
    private PedClothingShopMenuItem BusinessOutfit2_MPM;
    private PedClothingShopMenuItem BusinessOutfit3_MPM;
    private PedClothingShopMenuItem BusinessOutfit4_MPM;
    private PedClothingShopMenuItem BusinessOutfit5_MPM;
    //Binco
    private PedClothingShopMenuItem BincoOutfit1_MPM;
    private PedClothingShopMenuItem BincoOutfit2_MPM;
    private PedClothingShopMenuItem BincoOutfit3_MPM;
    //Discount
    private PedClothingShopMenuItem DiscoOutfit1_MPM;
    private PedClothingShopMenuItem DiscoOutfit2_MPM;
    private PedClothingShopMenuItem DiscoOutfit3_MPM;

    private PedClothingShopMenuItem SuburbOutfit1_MPM;
    private PedClothingShopMenuItem SuburbOutfit2_MPM;


    // Mp Male Accessories
    private PedClothingShopMenuItem RemoveAccessories1_MPM;
    //Added this for the player to remove accessories added from Outfits



    //Upper Body Male
    private PedClothingShopMenuItem UnbuttonedCasual1_MPM;
    private PedClothingShopMenuItem LostSupported_MPM;


    // Jumpers / Sweaters
    private PedClothingShopMenuItem CombatJumper1_MPM;
    private PedClothingShopMenuItem DesignJumper1_MPM;
    private PedClothingShopMenuItem PlainJumper1_MPM;
    private PedClothingShopMenuItem DesignJumper2_MPM;
    private PedClothingShopMenuItem DesignJumper3_MPM;
    private PedClothingShopMenuItem DesignJumper4_MPM;
    private PedClothingShopMenuItem TurtleJumper1_MPM;

    // Sportswear
    private PedClothingShopMenuItem PounderJersey1_MPM;
    private PedClothingShopMenuItem LeatherJersey1_MPM;

    private PedClothingShopMenuItem SportsJersey1_MPM;
    private PedClothingShopMenuItem SportsJersey2_MPM;
    private PedClothingShopMenuItem SportsJersey3_MPM;

    // Tshirts
    private PedClothingShopMenuItem PlainTshirt1_MPM;
    private PedClothingShopMenuItem VNeckTshirt1_MPM;
    private PedClothingShopMenuItem Tankshirt1_MPM;
    private PedClothingShopMenuItem PlainTshirt2_MPM;
    private PedClothingShopMenuItem PoloTshirt1_MPM;
    private PedClothingShopMenuItem Tankshirt2_MPM;
    private PedClothingShopMenuItem TeeTshirt1_MPM;
    private PedClothingShopMenuItem DesignerTshirt1_MPM;
    private PedClothingShopMenuItem BaggyTshirt1_MPM;
    private PedClothingShopMenuItem WorkShirt1_MPM;
    private PedClothingShopMenuItem DesignerTshirt2_MPM;

    // Hoodies
    private PedClothingShopMenuItem Hoodie1_MPM;
    private PedClothingShopMenuItem Hoodie2_MPM;
    private PedClothingShopMenuItem Hoodie3_MPM;
    private PedClothingShopMenuItem Hoodie4_MPM;
    private PedClothingShopMenuItem Hoodie5_MPM;
    private PedClothingShopMenuItem Hoodie6_MPM;
    private PedClothingShopMenuItem Hoodie7_MPM;
    private PedClothingShopMenuItem Hoodie8_MPM;
    private PedClothingShopMenuItem Hoodie9_MPM;

    // Leathers
    private PedClothingShopMenuItem LeatherJacket1_MPM;
    private PedClothingShopMenuItem LeatherJacket2_MPM;
    private PedClothingShopMenuItem LeatherJacket3_MPM;
    private PedClothingShopMenuItem LeatherJacket4_MPM;
    private PedClothingShopMenuItem LeatherJacket5_MPM;
    private PedClothingShopMenuItem LeatherJacket6_MPM;
    private PedClothingShopMenuItem LeatherJacket7_MPM;

    // Puffers
    private PedClothingShopMenuItem PufferJacket1_MPM;
    private PedClothingShopMenuItem PufferJacket2_MPM;
    private PedClothingShopMenuItem PufferJacket3_MPM;
    private PedClothingShopMenuItem PufferJacket4_MPM;

    // Shirts
    private PedClothingShopMenuItem LooseShirt1_MPM;
    private PedClothingShopMenuItem TuckedShirt1_MPM;
    private PedClothingShopMenuItem CheckShirt1_MPM;
    private PedClothingShopMenuItem TuckedShirt2_MPM;
    private PedClothingShopMenuItem BraceShirt1_MPM;
    private PedClothingShopMenuItem BraceShirt2_MPM;
    private PedClothingShopMenuItem FloralShirt1_MPM;
    private PedClothingShopMenuItem DesignerShirt1_MPM;
    private PedClothingShopMenuItem OfficeShirt1_MPM;
    private PedClothingShopMenuItem OfficeShirt2_MPM;

    // Sports Jackets/ Windbreakers/ Track Jackets
    private PedClothingShopMenuItem TrackJacket1_MPM;
    private PedClothingShopMenuItem TrackJacket2_MPM;
    private PedClothingShopMenuItem TrackJacket3_MPM;
    private PedClothingShopMenuItem TrackJacket4_MPM;

    // Suit Jackets/ Blazers/ Sports Coats
    private PedClothingShopMenuItem CoatJacket1_MPM;
    private PedClothingShopMenuItem CoatJacket2_MPM;
    private PedClothingShopMenuItem CoatJacket3_MPM;
    private PedClothingShopMenuItem CoatJacket4_MPM;
    private PedClothingShopMenuItem CoatJacket5_MPM;
    private PedClothingShopMenuItem CoatJacket6_MPM;

    private PedClothingShopMenuItem SuitJacket1_MPM;
    private PedClothingShopMenuItem SuitJacket2_MPM;
    private PedClothingShopMenuItem SuitJacket3_MPM;
    private PedClothingShopMenuItem SuitJacket4_MPM;
    private PedClothingShopMenuItem SuitJacket5_MPM;

    // SweaterVests
    private PedClothingShopMenuItem Sweat1Vest1_MPM;
    private PedClothingShopMenuItem Sweat1Vest2_MPM;
    private PedClothingShopMenuItem Sweat1Vest3_MPM;
    private PedClothingShopMenuItem Sweat1Vest4_MPM;
    private PedClothingShopMenuItem Sweat1Vest5_MPM;

    private PedClothingShopMenuItem Sweat2Vest1_MPM;
    private PedClothingShopMenuItem Sweat2Vest2_MPM;
    private PedClothingShopMenuItem Sweat2Vest3_MPM;
    private PedClothingShopMenuItem Sweat2Vest4_MPM;
    private PedClothingShopMenuItem Sweat2Vest5_MPM;

    // Vests
    private PedClothingShopMenuItem FormalVest1_MPM;
    private PedClothingShopMenuItem FormalVest2_MPM;
    private PedClothingShopMenuItem FormalVest3_MPM;
    private PedClothingShopMenuItem FormalVest4_MPM;
    private PedClothingShopMenuItem FormalVest5_MPM;

    private PedClothingShopMenuItem WatchVest1_MPM;
    private PedClothingShopMenuItem WatchVest2_MPM;
    private PedClothingShopMenuItem WatchVest3_MPM;
    private PedClothingShopMenuItem WatchVest4_MPM;
    private PedClothingShopMenuItem WatchVest5_MPM;

    //Lower Body Male

    //Jeans/ Casual Pants
    private PedClothingShopMenuItem RegularJeans1_MPM;
    private PedClothingShopMenuItem WashedOutJeans1_MPM;
    private PedClothingShopMenuItem SlimFitJeans1_MPM;
    private PedClothingShopMenuItem BaggyChinos1_MPM;
    private PedClothingShopMenuItem SlimFitJeans2_MPM;
    private PedClothingShopMenuItem LooseJeans2_MPM;
    private PedClothingShopMenuItem ClassicBlueJeans1_MPM;
    private PedClothingShopMenuItem RibbedJeans1_MPM;
    private PedClothingShopMenuItem RoadWornJeans1_MPM;
    private PedClothingShopMenuItem DenimOveralls1_MPM;
    private PedClothingShopMenuItem StraightChinoPants1_MPM;
    private PedClothingShopMenuItem StraightChinoPants2_MPM;
    private PedClothingShopMenuItem BeachChinoPants1_MPM;
    //Leather Pants
    private PedClothingShopMenuItem LeatherPants1_MPM;
    private PedClothingShopMenuItem LeatherPants2_MPM;
    private PedClothingShopMenuItem PaddedLeatherPants1_MPM;
    private PedClothingShopMenuItem PaddedLeatherPants2_MPM;
    private PedClothingShopMenuItem StitchLeatherPants1_MPM;
    //Tracksuit Pants
    private PedClothingShopMenuItem TrackPants1_MPM;
    private PedClothingShopMenuItem SweatPants1_MPM;
    private PedClothingShopMenuItem TrackPants2_MPM;
    private PedClothingShopMenuItem TrackPants3_MPM;
    private PedClothingShopMenuItem MusclePants1_MPM;
    private PedClothingShopMenuItem TieDyePants1_MPM;
    private PedClothingShopMenuItem CuffedPants1_MPM;
    private PedClothingShopMenuItem CuffedPants2_MPM;
    //Formal Trousers/ Suit Trousers/ Slacks
    private PedClothingShopMenuItem BeltedTrousers_MPM;
    private PedClothingShopMenuItem BaggyBeltedTrousers_MPM;
    private PedClothingShopMenuItem SlimSuitTrousers_MPM;
    private PedClothingShopMenuItem ClassicSuitTrousers_MPM;
    private PedClothingShopMenuItem TuxedoPants_MPM;
    private PedClothingShopMenuItem RegularSuitTrousers_MPM;
    private PedClothingShopMenuItem RegularSuitPants_MPM;
    private PedClothingShopMenuItem BaggySuitTrousers_MPM;
    private PedClothingShopMenuItem SlimFitTrousers_MPM;
    private PedClothingShopMenuItem ScruffySuitPants_MPM;
    private PedClothingShopMenuItem ContinentalPants_MPM;
    private PedClothingShopMenuItem ContinentalSlimPants_MPM;
    private PedClothingShopMenuItem ShinyPants_MPM;
    private PedClothingShopMenuItem GoldPrintPants_MPM;
    private PedClothingShopMenuItem ShinyFittedPants_MPM;
    private PedClothingShopMenuItem GoldPrintFittedPants_MPM;
    private PedClothingShopMenuItem ClassicSuitTrousers2_MPM;
    private PedClothingShopMenuItem HighRollerTrousers_MPM;
    private PedClothingShopMenuItem WideTrousers_MPM;
    //Shorts
    private PedClothingShopMenuItem TwoToneShorts1_MPM;
    private PedClothingShopMenuItem BoardShorts1_MPM;
    private PedClothingShopMenuItem ChinoShorts1_MPM;
    private PedClothingShopMenuItem RunningShorts1_MPM;
    private PedClothingShopMenuItem CargoShorts1_MPM;
    private PedClothingShopMenuItem BoardShorts2_MPM;
    private PedClothingShopMenuItem ChinoShorts2_MPM;
    private PedClothingShopMenuItem JoggingShorts1_MPM;
    private PedClothingShopMenuItem SwimShorts1_MPM;
    private PedClothingShopMenuItem WorkShorts1_MPM;
    private PedClothingShopMenuItem ChainShorts1_MPM;
    private PedClothingShopMenuItem KneeShorts1_MPM;
    private PedClothingShopMenuItem BasketballShorts1_MPM;
    private PedClothingShopMenuItem JeanShorts1_MPM;
    //Work/Utility/Cargo Pants
    private PedClothingShopMenuItem WorkPants1_MPM;
    private PedClothingShopMenuItem CargoPants1_MPM;
    private PedClothingShopMenuItem CombatPants1_MPM;
    private PedClothingShopMenuItem UtilityPants1_MPM;
    private PedClothingShopMenuItem ChainPants1_MPM;
    private PedClothingShopMenuItem CargoPants2_MPM;
    private PedClothingShopMenuItem CargoPants3_MPM;
    //Underwear
    private PedClothingShopMenuItem LoveHeartUnderwear1_MPM;
    private PedClothingShopMenuItem LongJohnsUnderwear1_MPM;
    private PedClothingShopMenuItem BoxersUnderwear1_MPM;
    private PedClothingShopMenuItem UFOBoxersUnderwear1_MPM;


    //Feet Male

    private PedClothingShopMenuItem Chucks1_MPM;
    private PedClothingShopMenuItem FlipFlops1_MPM;

    // Formal/Dress Shoes
    private PedClothingShopMenuItem LuxuryBoatShoes_MPM;
    private PedClothingShopMenuItem BlackOxfords_MPM;
    private PedClothingShopMenuItem ChocolateOxfords_MPM;
    private PedClothingShopMenuItem LeatherLoafers_MPM;
    private PedClothingShopMenuItem LeatherLoafers2_MPM;
    private PedClothingShopMenuItem SlipOnLoafers_MPM;
    private PedClothingShopMenuItem WingTips_MPM;
    private PedClothingShopMenuItem DrivingLoafers_MPM;
    private PedClothingShopMenuItem TipOxfords_MPM;
    private PedClothingShopMenuItem SmartOxfords_MPM;
    private PedClothingShopMenuItem BuckledLoafers_MPM;
    private PedClothingShopMenuItem SmartOxfords2_MPM;
    // Running Shoes/ Sneakers/ Casual Shoes
    private PedClothingShopMenuItem SkateShoes_MPM;
    private PedClothingShopMenuItem RunningShoes_MPM;
    private PedClothingShopMenuItem SportsShoes_MPM;
    private PedClothingShopMenuItem AthleticShoes_MPM;
    private PedClothingShopMenuItem KickShoes_MPM;
    private PedClothingShopMenuItem Chucks2_MPM;
    private PedClothingShopMenuItem StuddedSneakers_MPM;
    private PedClothingShopMenuItem GoldenHiTops_MPM;
    private PedClothingShopMenuItem Runners_MPM;
    private PedClothingShopMenuItem HighTopSneakers_MPM;
    private PedClothingShopMenuItem HighTopSneakers2_MPM;
    private PedClothingShopMenuItem CanvasSlipons_MPM;
    private PedClothingShopMenuItem PlainHiTops_MPM;
    private PedClothingShopMenuItem RetroRunners_MPM;
    private PedClothingShopMenuItem LightUps_MPM;
    private PedClothingShopMenuItem RetroSneakers_MPM;
    private PedClothingShopMenuItem KnitSneakers_MPM;
    private PedClothingShopMenuItem KnitSneakers2_MPM;
    private PedClothingShopMenuItem KnitSneakers3_MPM;
    // Boots
    private PedClothingShopMenuItem CopperWorkBoots_MPM;// Hinterlands, work boots, whatever you want to call them, they are a staple of the suburban dad look. < lol a true auto-gen description.
    private PedClothingShopMenuItem CasualBoots_MPM;
    private PedClothingShopMenuItem ChelseaBoots_MPM;
    private PedClothingShopMenuItem FlightBoots_MPM;
    private PedClothingShopMenuItem TacticalBoots_MPM;
    private PedClothingShopMenuItem WalkingBoots_MPM;
    private PedClothingShopMenuItem CowboyBoots_MPM;
    private PedClothingShopMenuItem CowboyBoots2_MPM;
    private PedClothingShopMenuItem AnkleBoots_MPM;
    private PedClothingShopMenuItem LaceUpBoots_MPM;
    private PedClothingShopMenuItem SlackBoots_MPM;
    private PedClothingShopMenuItem TacticalBoots2_MPM;
    private PedClothingShopMenuItem MocToeBoots_MPM;
    private PedClothingShopMenuItem RubberizedBoots_MPM;
    private PedClothingShopMenuItem TrailBoots_MPM;
    private PedClothingShopMenuItem FlamingBoots_MPM;
    private PedClothingShopMenuItem HarnessBoots_MPM;
    private PedClothingShopMenuItem UniformBoots_MPM;
    private PedClothingShopMenuItem RoadBoots_MPM;


    // Female

    //OutFits
    private PedClothingShopMenuItem BusinessOutfit1_MPF;
    private PedClothingShopMenuItem BusinessOutfit2_MPF;

    //Accessories
    private PedClothingShopMenuItem RemoveAccessories1_MPF;



    // Female Upper Body
    private PedClothingShopMenuItem Polo1_MPF;
    private PedClothingShopMenuItem FittedT_MPF;
    private PedClothingShopMenuItem LooseButton_MPF;
    private PedClothingShopMenuItem LostSupported_MPF;





    //Female Lower Body

    //Bikini
    private PedClothingShopMenuItem Bikini1_MPF;
    private PedClothingShopMenuItem Bikini2_MPF;

    //Formal Trousers/ Suit Trousers/ Slacks
    private PedClothingShopMenuItem SuitPants1_MPF;
    private PedClothingShopMenuItem SuitPants2_MPF;
    private PedClothingShopMenuItem SuitPants3_MPF;
    private PedClothingShopMenuItem SuitPants4_MPF;
    private PedClothingShopMenuItem ContinentalPants_MPF;
    private PedClothingShopMenuItem ContinentalSlimPants_MPF;
    private PedClothingShopMenuItem GoldPrintFittedPants_MPF;
    private PedClothingShopMenuItem GoldPrintPants_MPF;
    private PedClothingShopMenuItem ShinyFittedPants_MPF;
    private PedClothingShopMenuItem ShinyPants_MPF;
    private PedClothingShopMenuItem WideTrousers_MPF;
    private PedClothingShopMenuItem SlackPants_MPF;
    //Jeans/ Chinos Casual Pants
    private PedClothingShopMenuItem SkinnyJeans1_MPF;
    private PedClothingShopMenuItem RegularJeans1_MPF;
    private PedClothingShopMenuItem ChinosPants1_MPF;
    private PedClothingShopMenuItem CroppedJeans1_MPF;
    private PedClothingShopMenuItem ChinosPants2_MPF;
    private PedClothingShopMenuItem RibbedJeans1_MPF;
    private PedClothingShopMenuItem RoadWornJeans1_MPF;
    private PedClothingShopMenuItem StraightChinoPants1_MPF;
    private PedClothingShopMenuItem StraightChinoPants2_MPF;
    private PedClothingShopMenuItem BeachChinoPants1_MPF;

    //Leathers
    private PedClothingShopMenuItem LeatherPants1_MPF;
    private PedClothingShopMenuItem LeatherPants2_MPF;
    private PedClothingShopMenuItem LeatherPants3_MPF;
    private PedClothingShopMenuItem LeatherPants4_MPF;
    private PedClothingShopMenuItem LeatherPants5_MPF;
    private PedClothingShopMenuItem LeatherPants6_MPF;
    //Leggings
    private PedClothingShopMenuItem LeggingsPants1_MPF;
    private PedClothingShopMenuItem LeggingsPants2_MPF;
    private PedClothingShopMenuItem LeggingsPants3_MPF;
    private PedClothingShopMenuItem LeggingsPants4_MPF;
    // Shorts
    private PedClothingShopMenuItem eShorts1_MPF;
    private PedClothingShopMenuItem PlainShorts1_MPF;
    private PedClothingShopMenuItem ClassicShorts1_MPF;
    private PedClothingShopMenuItem CargoShorts1_MPF;
    private PedClothingShopMenuItem DenimShorts1_MPF;
    private PedClothingShopMenuItem DenimShorts2_MPF;
    private PedClothingShopMenuItem BeachShorts1_MPF;
    private PedClothingShopMenuItem ChainShorts1_MPF;
    private PedClothingShopMenuItem KneeShorts1_MPF;
    private PedClothingShopMenuItem ChinoShorts1_MPF;
    private PedClothingShopMenuItem BasketballShorts1_MPF;
    private PedClothingShopMenuItem JeanShorts1_MPF;
    //Skirts
    private PedClothingShopMenuItem PencilSkirt1_MPF;
    private PedClothingShopMenuItem MiniSkirt1_MPF;
    private PedClothingShopMenuItem MiniSkirt2_MPF;
    private PedClothingShopMenuItem PleatedSkirt1_MPF;
    private PedClothingShopMenuItem PencilSkirt2_MPF;
    private PedClothingShopMenuItem MiniSkirt3_MPF;
    private PedClothingShopMenuItem PencilSkirt3_MPF;
    private PedClothingShopMenuItem MiniSkirt4_MPF;
    private PedClothingShopMenuItem DenimSkirt1_MPF;
    private PedClothingShopMenuItem PleatedSkirt2_MPF;
    //Sportswear
    private PedClothingShopMenuItem RolledUpPants1_MPF;
    private PedClothingShopMenuItem TrackPants1_MPF;
    private PedClothingShopMenuItem TrackPants2_MPF;
    private PedClothingShopMenuItem MusclePants1_MPF;
    private PedClothingShopMenuItem SportsPants1_MPF;
    private PedClothingShopMenuItem TieDyePants1_MPF;
    private PedClothingShopMenuItem CuffedPants1_MPF;
    private PedClothingShopMenuItem CuffedPants2_MPF;
    private PedClothingShopMenuItem TrackPants3_MPF;
    //Underwear
    private PedClothingShopMenuItem LaceUnderwear1_MPF;
    private PedClothingShopMenuItem StockingsUnderwear1_MPF;
    private PedClothingShopMenuItem LaceUnderwear2_MPF;
    private PedClothingShopMenuItem StockingsUnderwear2_MPF;
    private PedClothingShopMenuItem UFOBoxersUnderwear1_MPF;
    //Utility
    private PedClothingShopMenuItem CargoPants1_MPF;
    private PedClothingShopMenuItem CombatPants1_MPF;
    private PedClothingShopMenuItem CargoPants2_MPF;
    private PedClothingShopMenuItem UtilityPants1_MPF;
    private PedClothingShopMenuItem DenimOveralls1_MPF;
    private PedClothingShopMenuItem ChainPants1_MPF;
    private PedClothingShopMenuItem CargoPants3_MPF;
    private PedClothingShopMenuItem CargoPants4_MPF;

    //Feet Female

    // Running Shoes / Skate Shoes etc..
    private PedClothingShopMenuItem SkateShoes1_MPF;
    private PedClothingShopMenuItem RunningShoes1_MPF;
    private PedClothingShopMenuItem RunningShoes2_MPF;
    private PedClothingShopMenuItem Chucks1_MPF;
    private PedClothingShopMenuItem FlipFlops1_MPF;
    private PedClothingShopMenuItem FlipFlops2_MPF;
    private PedClothingShopMenuItem FlipFlops3_MPF;
    private PedClothingShopMenuItem HighTopsShoes1_MPF;
    private PedClothingShopMenuItem RoundToeShoes1_MPF;
    private PedClothingShopMenuItem SandalShoes1_MPF;
    private PedClothingShopMenuItem GoldenHiTops_MPF;
    private PedClothingShopMenuItem Runners_MPF;
    private PedClothingShopMenuItem SneakerWedges_MPF;
    private PedClothingShopMenuItem PlainHiTops_MPF;
    private PedClothingShopMenuItem RetroRunners_MPF;
    private PedClothingShopMenuItem LightUps_MPF;
    private PedClothingShopMenuItem RetroSneakers_MPF;
    private PedClothingShopMenuItem KnitSneakers_MPF;
    private PedClothingShopMenuItem KnitSneakers2_MPF;
    private PedClothingShopMenuItem KnitSneakers3_MPF;
    // High Heels
    private PedClothingShopMenuItem Heels1_MPF;
    private PedClothingShopMenuItem Heels2_MPF;
    private PedClothingShopMenuItem Heels3_MPF;
    private PedClothingShopMenuItem Heels4_MPF;
    private PedClothingShopMenuItem Heels5_MPF;
    private PedClothingShopMenuItem Heels6_MPF;
    private PedClothingShopMenuItem Heels7_MPF;
    private PedClothingShopMenuItem Heels8_MPF;
    private PedClothingShopMenuItem Heels9_MPF;
    //Boots
    private PedClothingShopMenuItem FUGGsBoots1_MPF;
    private PedClothingShopMenuItem HeelsBoots1_MPF;
    private PedClothingShopMenuItem HeelsBoots2_MPF;
    private PedClothingShopMenuItem KneeHighBoots1_MPF;
    private PedClothingShopMenuItem KneeHighBoots2_MPF;
    private PedClothingShopMenuItem FoldedBoots1_MPF;
    private PedClothingShopMenuItem FlightBoots1_MPF;
    private PedClothingShopMenuItem TacticalBoots1_MPF;
    private PedClothingShopMenuItem StuddedBoots1_MPF;
    private PedClothingShopMenuItem WalkingBoots_MPF;
    private PedClothingShopMenuItem CowboyBoots_MPF;
    private PedClothingShopMenuItem CowboyBoots2_MPF;
    private PedClothingShopMenuItem LaceUpBoots_MPF;
    private PedClothingShopMenuItem SlackBoots_MPF;
    private PedClothingShopMenuItem CalfBoots_MPF;
    private PedClothingShopMenuItem TacticalBoots2_MPF;
    private PedClothingShopMenuItem MocToeBoots_MPF;
    private PedClothingShopMenuItem RubberizedBoots_MPF;
    private PedClothingShopMenuItem TrailBoots_MPF;
    private PedClothingShopMenuItem FlamingBoots_MPF;
    private PedClothingShopMenuItem HarnessBoots_MPF;
    private PedClothingShopMenuItem UniformBoots_MPF;


    public PedClothingShopMenu SuburbanClothesMenu { get; private set; }
    public PedClothingShopMenu BincoClothesMenu { get; private set; }
    public PedClothingShopMenu DiscountStoreClothesMenu { get; private set; }
    public PedClothingShopMenu PoisonbysClothesMenu { get; private set; }
    public PedClothingShopMenu DidierSachsClothesMenu { get; private set; }
    public PedClothingShopMenu HelmetShopMenu { get; private set; }
    public PedClothingShopMenu GenericClothesShopMenu { get; private set; }
    public PedClothingShopMenu MaskShopMenu { get; private set; }
    public PedClothingShopMenu FreakMaskShopMenu { get; private set; }
    public void Setup()
    {
        MPFemale();
        MPMale();
        MPCombined();
        
        SetMenus();
    }
    public void SetMenus()
    {
        //Suburban
        //Binco
        //Discount Store
        //Poisonbys
        //Didier Sachs
        DiscountStoreClothesMenu = new PedClothingShopMenu();
        DiscountStoreClothesMenu.ID = "DiscountStoreClothesMenu";
        DiscountStoreClothesMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            //Male

            //OutFits
            DiscoOutfit1_MPM,
            DiscoOutfit2_MPM,
            DiscoOutfit3_MPM,

            //Accessories
            RemoveAccessories1_MPM,

            StrawHat1_MPM,
            UnbuttonedCasual1_MPM,
            LostSupported_MPM,
            Beanie1_MPM,
            Beanie2_MPM,
            Beanie3_MPM,
            CowboyHat1_MPM,
            BaseballHatFastFood_MPM,
            BaseballHatFastFoodReverse_MPM,




            //Hoodies
            Hoodie2_MPM,
            Hoodie4_MPM,
            Hoodie5_MPM,
            Hoodie7_MPM,
       

            //Shirts
            BraceShirt1_MPM,
            BraceShirt2_MPM,

            //T-Shirts
            PlainTshirt1_MPM,
            VNeckTshirt1_MPM,
            Tankshirt1_MPM,
            PlainTshirt2_MPM,
            TuckedShirt2_MPM,
            BaggyTshirt1_MPM,



            //Jackets/ Blazers/ Sports Coats
            TrackJacket1_MPM,
            TrackJacket2_MPM,
            TrackJacket3_MPM,



            //Leathers
            LeatherJacket1_MPM,
            LeatherJacket3_MPM,
            LeatherJacket4_MPM,
            LeatherJacket7_MPM,

            //Puffers
            PufferJacket1_MPM,

            //Shirts
            CheckShirt1_MPM,
            LooseShirt1_MPM,
            TuckedShirt1_MPM,

            //Sweaters
            PlainJumper1_MPM,


            //Lower

            //Jeans/ casual pants
            BaggyChinos1_MPM,
            ClassicBlueJeans1_MPM,
            LooseJeans2_MPM,
            RegularJeans1_MPM,
            RibbedJeans1_MPM,
            RoadWornJeans1_MPM,
            WashedOutJeans1_MPM,

            //Leathers
            LeatherPants1_MPM,
            LeatherPants2_MPM,
            PaddedLeatherPants1_MPM,
            PaddedLeatherPants2_MPM,
            StitchLeatherPants1_MPM,

            //Shorts
            BoardShorts1_MPM,
            BoardShorts2_MPM,
            CargoShorts1_MPM,
            ChinoShorts1_MPM,
            TwoToneShorts1_MPM,
            JeanShorts1_MPM,
            SwimShorts1_MPM,
            WorkShorts1_MPM,

            //Formal
            ScruffySuitPants_MPM,

            //Track Pants
            CuffedPants1_MPM,
            SweatPants1_MPM,
            TrackPants1_MPM,
            TrackPants2_MPM,

            //Utilty
            CargoPants2_MPM,
            CargoPants3_MPM,
            UtilityPants1_MPM,
            WorkPants1_MPM,

            //Underwear
            BoxersUnderwear1_MPM,
            UFOBoxersUnderwear1_MPM,


            //Shoes
            FlipFlops1_MPM,
            HighTopSneakers2_MPM,
            KickShoes_MPM,
            KnitSneakers2_MPM,
            PlainHiTops_MPM,
            RetroRunners_MPM,
            RetroSneakers_MPM,
            //Boots
            CowboyBoots_MPM,
            FlamingBoots_MPM,
            LaceUpBoots_MPM,
            RoadBoots_MPM,
            SlackBoots_MPM,
            TacticalBoots2_MPM,

         
            //Female

            //Accessories
            RemoveAccessories1_MPF,

            LooseButton_MPF,
            StrawHat1_MPF,
            LostSupported_MPF,


            //Lower

            //Formal Trousers/ Suit Trousers/ Slacks
            SuitPants4_MPF,

            //Leathers
            LeatherPants1_MPF,
            LeatherPants2_MPF,
            LeatherPants3_MPF,
            LeatherPants4_MPF,
            LeatherPants5_MPF,

            //Leggings
            LeggingsPants3_MPF,

            // Jeans/ Casual Pants

            RegularJeans1_MPF,
            RibbedJeans1_MPF,
            RoadWornJeans1_MPF,

            //Shorts
            CargoShorts1_MPF,
            DenimShorts2_MPF,
            JeanShorts1_MPF,
            PlainShorts1_MPF,

            //Skirts
            DenimSkirt1_MPF,
            MiniSkirt3_MPF,
            MiniSkirt1_MPF,
            PencilSkirt1_MPF,
            PencilSkirt3_MPF,
                       
            //Sportswear
            CuffedPants1_MPF,
            RolledUpPants1_MPF,
            SportsPants1_MPF,
            TrackPants1_MPF,
            TrackPants2_MPF,

            //Bikini - Underwear
            Bikini1_MPF,
            UFOBoxersUnderwear1_MPF,

            //Utility
            CargoPants2_MPF,

            //Shoes
            FlipFlops1_MPF,
            FlipFlops2_MPF,
            HighTopsShoes1_MPF,
            KnitSneakers2_MPF,
            RetroRunners_MPF,
            RetroSneakers_MPF,

            //Boots
            CalfBoots_MPF,
            CowboyBoots_MPF,
            FlamingBoots_MPF,
            KneeHighBoots1_MPF,
            LaceUpBoots_MPF,
            SlackBoots_MPF,
            TacticalBoots2_MPF,


        };
        BincoClothesMenu = new PedClothingShopMenu();
        BincoClothesMenu.ID = "BincoClothesMenu";
        BincoClothesMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            //Male

            //Outfits
            BincoOutfit1_MPM,
            BincoOutfit2_MPM,
            BincoOutfit3_MPM,

            //Accessories
            RemoveAccessories1_MPM,


            Aviators1_MPM,
            BaseballHatGuns_MPM,
            BaseballHatGunsReverse_MPM,



            //Tops

            //Hoodies
            Hoodie2_MPM,

            //Jumper
            CombatJumper1_MPM,
            PlainJumper1_MPM,

            //Sportswear
            PounderJersey1_MPM,
            LeatherJersey1_MPM,

            SportsJersey1_MPM,
            SportsJersey2_MPM,
            SportsJersey3_MPM,

            //Tshirts
            PlainTshirt1_MPM,
            VNeckTshirt1_MPM,
            Tankshirt1_MPM,

            //Jackets/ Blazers/ Sports Coats
            TrackJacket1_MPM,


            //Shirt
            LooseShirt1_MPM,
            TuckedShirt1_MPM,
            WorkShirt1_MPM,

            //Lower

            //Jeans & casual
            ClassicBlueJeans1_MPM,
            DenimOveralls1_MPM,
            LooseJeans2_MPM,
            RegularJeans1_MPM,

            //Track Pants
            SweatPants1_MPM,
            TrackPants2_MPM,

            //Shorts
            BasketballShorts1_MPM,
            BoardShorts1_MPM,
            CargoShorts1_MPM,
            JoggingShorts1_MPM,
            RunningShorts1_MPM,
            SwimShorts1_MPM,
            WorkShorts1_MPM,

            //Utility
            CargoPants1_MPM,
            CargoPants2_MPM,
            CargoPants3_MPM,
            CombatPants1_MPM,
            UtilityPants1_MPM,
            WorkPants1_MPM,

            //Underwear
            BoxersUnderwear1_MPM,

            // Shoes
            CanvasSlipons_MPM,
            Chucks2_MPM,
            FlipFlops1_MPM,
            KickShoes_MPM,
            RunningShoes_MPM,
            //Boots
            FlightBoots_MPM,
            RubberizedBoots_MPM,
            TacticalBoots_MPM,
            TrailBoots_MPM,
            WalkingBoots_MPM,


            //Female

            //Accessories
            RemoveAccessories1_MPF,

            Aviators1_MPF,
            Polo1_MPF,
            FittedT_MPF,



            //Lower
            //Formal Trousers/ Suit Trousers/ Slacks
            SuitPants3_MPF,
            SuitPants4_MPF,
            SlackPants_MPF,

            //Skirts
            PencilSkirt1_MPF,
            // Shorts
            BasketballShorts1_MPF,
            DenimShorts1_MPF,
            CargoShorts1_MPF,
            // Sportswear
            TrackPants2_MPF,
            TrackPants3_MPF,
            //Utility
            CargoPants1_MPF,
            CargoPants3_MPF,
            CargoPants4_MPF,
            CombatPants1_MPF,
            DenimOveralls1_MPF,
            UtilityPants1_MPF,
            //Shoes
            FlipFlops1_MPF,
            FlipFlops2_MPF,
            RunningShoes1_MPF,
            //Boots
            FlightBoots1_MPF,
            RubberizedBoots_MPF,
            TacticalBoots1_MPF,
            TrailBoots_MPF,
            WalkingBoots_MPF,

        };
        SuburbanClothesMenu = new PedClothingShopMenu();
        SuburbanClothesMenu.ID = "SuburbanClothesMenu";
        SuburbanClothesMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            // MP Male

            //Outfits
            SuburbOutfit1_MPM,
            SuburbOutfit2_MPM,

            //Accessories
            RemoveAccessories1_MPM,

            //Hats
            BaseballHatProLaps1_MPM,
            BaseballHatProLaps1Reverse_MPM,
            BaseballHatRegular1_MPM,
            BaseballHatRegular1Reverse_MPM,   
            BaseballHatBroker_MPM,
            BaseballHatBrokerReverse_MPM,
            BaseballHatEnus_MPM,
            BaseballHatEnusReverse_MPM,


            //Tops

            //Hoodies
            Hoodie1_MPM,
            Hoodie3_MPM,
            Hoodie6_MPM,
            Hoodie7_MPM,
            Hoodie8_MPM,
            Hoodie9_MPM,

            //Jumpers
            DesignJumper1_MPM,
            DesignJumper2_MPM,
            DesignJumper4_MPM,
            DesignJumper3_MPM,

            //Sportswear
            PounderJersey1_MPM,
            SportsJersey1_MPM,
            SportsJersey2_MPM,
            SportsJersey3_MPM,


            //Tshirts
            PlainTshirt1_MPM,
            VNeckTshirt1_MPM,
            PlainTshirt2_MPM,
            Tankshirt2_MPM,
            TeeTshirt1_MPM,
            BaggyTshirt1_MPM,
            DesignerTshirt2_MPM,

            //Jackets/ Blazers/ Sports Coats
            TrackJacket4_MPM,

            PufferJacket2_MPM,
            PufferJacket3_MPM,
            PufferJacket4_MPM,


            //Shirts
            CheckShirt1_MPM,
            LooseShirt1_MPM,
            TuckedShirt1_MPM,
            TuckedShirt2_MPM,

            //Vests/ Leathers

            //Lower

            //Jeans / Casual Pants
            BaggyChinos1_MPM,
            BeachChinoPants1_MPM,
            LooseJeans2_MPM,
            RegularJeans1_MPM,
            SlimFitJeans1_MPM,
            SlimFitJeans2_MPM,
            StraightChinoPants1_MPM,
            StraightChinoPants2_MPM,
            WashedOutJeans1_MPM,

            //Shorts
            BasketballShorts1_MPM,
            BoardShorts1_MPM,
            BoardShorts2_MPM,
            CargoShorts1_MPM,
            ChainShorts1_MPM,
            ChinoShorts1_MPM,
            JeanShorts1_MPM,
            JoggingShorts1_MPM,
            RunningShorts1_MPM,
            SwimShorts1_MPM,
            TwoToneShorts1_MPM,

            //Track Pants
            CuffedPants2_MPM,
            MusclePants1_MPM,
            TieDyePants1_MPM,
            TrackPants1_MPM,
            TrackPants2_MPM,
            TrackPants3_MPM,


            //Utility
            CargoPants1_MPM,
            CombatPants1_MPM,
            ChainPants1_MPM,

            //Underwear
            LoveHeartUnderwear1_MPM,
            UFOBoxersUnderwear1_MPM,

            //Feet

            //Shoes
            AthleticShoes_MPM,
            Chucks1_MPM,           
            HighTopSneakers_MPM,
            KnitSneakers_MPM,
            KnitSneakers3_MPM,
            LightUps_MPM,
            PlainHiTops_MPM,
            Runners_MPM,
            SkateShoes_MPM,
            SportsShoes_MPM,
            StuddedSneakers_MPM,
            //Boots
            CasualBoots_MPM,
            CopperWorkBoots_MPM,
            CowboyBoots2_MPM,
            HarnessBoots_MPM,
            UniformBoots_MPM,



            // MP Female

            //Accessories
            RemoveAccessories1_MPF,

            //Hats
            BaseballHatProLaps1_MPF,
            BaseballHatRegular1_MPF,
            BaseballHatRegular1Reverse_MPF,



            //Lower

            //Jeans / Casual Pants
            BeachChinoPants1_MPF,
            ChainPants1_MPF,
            ChinosPants1_MPF,
            CroppedJeans1_MPF,
            ChinosPants2_MPF,
            RegularJeans1_MPF,
            SkinnyJeans1_MPF,
            StraightChinoPants1_MPF,
            StraightChinoPants2_MPF,

            //leathers
            LeatherPants6_MPF,


            //Leggings
            LeggingsPants1_MPF,
            LeggingsPants2_MPF,
            LeggingsPants4_MPF,



            // Shorts
            BasketballShorts1_MPF,
            BeachShorts1_MPF,
            ChainShorts1_MPF,
            DenimShorts2_MPF,
            eShorts1_MPF,
            JeanShorts1_MPF,



            //Skirts

            MiniSkirt1_MPF,
            PleatedSkirt1_MPF,
            PleatedSkirt2_MPF,


            // Sportswear
            CuffedPants2_MPF,
            MusclePants1_MPF,
            RolledUpPants1_MPF,
            SportsPants1_MPF,
            TieDyePants1_MPF,

            //Bikini - Underwear
            Bikini2_MPF,
            UFOBoxersUnderwear1_MPF,

            //Utility
            CargoPants2_MPF,
            CargoPants3_MPF,
            CargoPants4_MPF,


            //Shoes
            Chucks1_MPF,
            FlipFlops2_MPF,
            FlipFlops3_MPF,
            KnitSneakers_MPF,
            KnitSneakers3_MPF,
            LightUps_MPF,
            RunningShoes2_MPF,
            Runners_MPF,
            SkateShoes1_MPF,
            SneakerWedges_MPF,
            PlainHiTops_MPF,
            //Heels
            Heels3_MPF,
            Heels5_MPF,
            Heels8_MPF,
            Heels9_MPF,
            //Boots
            CowboyBoots2_MPF,
            FUGGsBoots1_MPF,
            FoldedBoots1_MPF,
            HarnessBoots_MPF,
            StuddedBoots1_MPF,
            UniformBoots_MPF,


        };
        

        PoisonbysClothesMenu = new PedClothingShopMenu();
        PoisonbysClothesMenu.ID = "PoisonbysClothesMenu";
        PoisonbysClothesMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            //Male

            //Outfits
            BusinessOutfit1_MPM,
            BusinessOutfit2_MPM,
            BusinessOutfit3_MPM,

            //Accessories
            RemoveAccessories1_MPM,

            Necklace1_MPM,
            Watch2_MPM,
            Watch3_MPM,
            DangleEarring1_MPM,





            //Tops

            //Shirts
            TuckedShirt1_MPM,
            FloralShirt1_MPM,
            DesignerShirt1_MPM,
            OfficeShirt1_MPM,
            OfficeShirt2_MPM,

            // Suit Jackets & Coats
            SuitJacket1_MPM,
            SuitJacket2_MPM,
            SuitJacket3_MPM,

            LeatherJacket5_MPM,
            LeatherJacket6_MPM,

            CoatJacket1_MPM,
            CoatJacket2_MPM,
            CoatJacket3_MPM,
            CoatJacket4_MPM,
            CoatJacket5_MPM,
            CoatJacket6_MPM,

            //SweaterVests
            Sweat1Vest1_MPM,
            Sweat1Vest2_MPM,
            Sweat1Vest3_MPM,
            Sweat1Vest4_MPM,
            Sweat1Vest5_MPM,
            Sweat2Vest1_MPM,
            Sweat2Vest2_MPM,
            Sweat2Vest3_MPM,
            Sweat2Vest4_MPM,
            Sweat2Vest5_MPM,

            //Vests
            FormalVest1_MPM,
            FormalVest2_MPM,
            FormalVest3_MPM,
            FormalVest4_MPM,
            FormalVest5_MPM,
            WatchVest1_MPM,
            WatchVest2_MPM,
            WatchVest3_MPM,
            WatchVest4_MPM,
            WatchVest5_MPM,

            //T - Shirt
            PoloTshirt1_MPM,
            DesignerTshirt1_MPM,

            //Sweater
            TurtleJumper1_MPM,


            //Lower

            // Suit Trousers & Slacks
            BaggyBeltedTrousers_MPM,
            BaggySuitTrousers_MPM,
            BeltedTrousers_MPM,
            ClassicSuitTrousers_MPM,
            ContinentalPants_MPM,
            ContinentalSlimPants_MPM,
            HighRollerTrousers_MPM,
            RegularSuitTrousers_MPM,
            RegularSuitPants_MPM,
            ShinyFittedPants_MPM,
            ShinyPants_MPM,
            SlimFitTrousers_MPM,
            SlimSuitTrousers_MPM,
            TuxedoPants_MPM,
            WideTrousers_MPM,

            //Shorts
            ChinoShorts2_MPM,
            KneeShorts1_MPM,
            RunningShorts1_MPM,

            //Underwear
            LongJohnsUnderwear1_MPM,
            LoveHeartUnderwear1_MPM,



            // Shoes
            BlackOxfords_MPM,            
            BuckledLoafers_MPM,
            ChocolateOxfords_MPM,
            LeatherLoafers_MPM,
            LuxuryBoatShoes_MPM,
            SlipOnLoafers_MPM,
            SmartOxfords2_MPM,
            //Boots
            AnkleBoots_MPM,
            ChelseaBoots_MPM,
            MocToeBoots_MPM,

            //Female

            //Accessories
            RemoveAccessories1_MPF,

            Necklace1_MPF,
            GoldWatch2_MPF,
            BusinessOutfit2_MPF,



            //Lower

            //Bikini
            Bikini2_MPF,

            //Formal Trousers/ Suit Trousers/ Slacks
            ContinentalPants_MPF,
            ContinentalSlimPants_MPF,
            GoldPrintFittedPants_MPF,
            GoldPrintPants_MPF,
            ShinyFittedPants_MPF,
            ShinyPants_MPF,
            SuitPants1_MPF,
            SuitPants2_MPF,
            WideTrousers_MPF,

            //Shorts
            ChinoShorts1_MPF,
            KneeShorts1_MPF,

            //Skirts
            MiniSkirt2_MPF,
            MiniSkirt4_MPF,
            PencilSkirt2_MPF,

            //Underwear
            LaceUnderwear1_MPF,
            LaceUnderwear2_MPF,
            StockingsUnderwear1_MPF,
            StockingsUnderwear2_MPF,


            //Shoes
            RoundToeShoes1_MPF,

            //Heels
            Heels1_MPF,
            Heels4_MPF,
            Heels7_MPF,

            //Boots
            FoldedBoots1_MPF,
            HeelsBoots1_MPF,
            MocToeBoots_MPF,


        };
        DidierSachsClothesMenu = new PedClothingShopMenu();
        DidierSachsClothesMenu.ID = "DidierSachsClothesMenu";
        DidierSachsClothesMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            //Male

            //Outfits
            BusinessOutfit1_MPM,
            BusinessOutfit4_MPM,
            BusinessOutfit5_MPM,


            //Accessories
            RemoveAccessories1_MPM,


            //Accessories
            Watch1_MPM,

            //Hats
            StovetopHat1_MPM,
            BowlerHat1_MPM,
            BrimmedHat1_MPM,
            BrimmedHat2_MPM,
            BrimmedHat3_MPM,


            //Tops



            //Shirts
            TuckedShirt1_MPM,
            FloralShirt1_MPM,
            DesignerShirt1_MPM,
            OfficeShirt1_MPM,
            OfficeShirt2_MPM,


            //Jackets
            LeatherJacket2_MPM, // Suede Jacket
            LeatherJacket5_MPM, // Leather Fur
            LeatherJacket6_MPM, // Bright Leather Fur

            // Suit Jackets & Coats

            CoatJacket1_MPM,
            CoatJacket2_MPM,
            CoatJacket3_MPM,
            CoatJacket4_MPM,
            CoatJacket5_MPM,
            CoatJacket6_MPM,

            SuitJacket1_MPM,
            SuitJacket2_MPM,
            SuitJacket3_MPM,
            SuitJacket4_MPM,
            SuitJacket5_MPM,

            //SweaterVests
            Sweat1Vest1_MPM,
            Sweat1Vest2_MPM,
            Sweat1Vest3_MPM,
            Sweat1Vest4_MPM,
            Sweat1Vest5_MPM,
            Sweat2Vest1_MPM,
            Sweat2Vest2_MPM,
            Sweat2Vest3_MPM,
            Sweat2Vest4_MPM,
            Sweat2Vest5_MPM,

            //Vests
            FormalVest1_MPM,
            FormalVest2_MPM,
            FormalVest3_MPM,
            FormalVest4_MPM,
            FormalVest5_MPM,
            WatchVest1_MPM,
            WatchVest2_MPM,
            WatchVest3_MPM,
            WatchVest4_MPM,
            WatchVest5_MPM,

            //Tshirts
            PoloTshirt1_MPM,
            DesignerTshirt1_MPM,

            //Sweaters
            TurtleJumper1_MPM,



            //Lower
            BaggyBeltedTrousers_MPM,
            BaggySuitTrousers_MPM,
            BeltedTrousers_MPM,
            ClassicSuitTrousers_MPM,
            ClassicSuitTrousers2_MPM,
            ContinentalPants_MPM,
            ContinentalSlimPants_MPM,
            GoldPrintFittedPants_MPM,
            GoldPrintPants_MPM,
            HighRollerTrousers_MPM,
            RegularSuitTrousers_MPM,
            ShinyFittedPants_MPM,
            ShinyPants_MPM,
            SlimFitTrousers_MPM,

            //Shorts
            ChinoShorts2_MPM,
            KneeShorts1_MPM,
            RunningShorts1_MPM,

            //Underwear
            LongJohnsUnderwear1_MPM,


            //Shoes
            BuckledLoafers_MPM,
            DrivingLoafers_MPM,
            GoldenHiTops_MPM,
            LeatherLoafers2_MPM,
            SmartOxfords_MPM,
            TipOxfords_MPM,
            WingTips_MPM,

            //Boots
            AnkleBoots_MPM,
            ChelseaBoots_MPM,
            MocToeBoots_MPM,





            //Female

            //OutFits
            BusinessOutfit1_MPF,
            
            //Accessories
            RemoveAccessories1_MPF,

            GoldWatch1_MPF,
            DangleEarring1_MPF,



            //Lower

            //Formal Trousers/ Suit Trousers/ Slacks
            ContinentalPants_MPF,
            ContinentalSlimPants_MPF,
            GoldPrintFittedPants_MPF,
            GoldPrintPants_MPF,
            ShinyFittedPants_MPF,
            ShinyPants_MPF,
            SuitPants1_MPF,
            SuitPants2_MPF,
            WideTrousers_MPF,



            // Shorts
            ClassicShorts1_MPF,
            ChinoShorts1_MPF,
            KneeShorts1_MPF,

            //Skirts
            MiniSkirt2_MPF,
            MiniSkirt4_MPF,
            PencilSkirt2_MPF,

            //underwear
            LaceUnderwear1_MPF,
            LaceUnderwear2_MPF,
            StockingsUnderwear1_MPF,
            StockingsUnderwear2_MPF,

            //Shoes
            GoldenHiTops_MPF,
            SandalShoes1_MPF,

            //Heels
            Heels2_MPF,
            Heels6_MPF,

            //Boots
            HeelsBoots2_MPF,
            KneeHighBoots2_MPF,

        };


        MaskShopMenu = new PedClothingShopMenu();
        MaskShopMenu.ID = "MaskShopMenu";
        MaskShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            MaskHockey_MPC,
            MaskBallistic_MPC,                 
            MaskCombatMask_MPC,                            
            MaskPainted_MPC,                    
            MaskNightVision_MPC,               
            MaskSnakeSkull_MPC,                 
            MaskSpecOps_MPC,                    
            MaskFilter_MPC,                     
            MaskVent_MPC,
            MaskSkullScruffyBalaclava_MPC,
            MaskScruffyBalaclava_MPC,
            MaskBlackKnitBalaclava_MPC,
            MaskBanditKnitBalaclava_MPC,
            MaskBrightStripeKnitBalaclava_MPC,
            MaskLooseBalaclava_MPC,
            MaskClown_MPC,
            MaskHockeyAlt_MPC,
            MaskMandible_MPC,                   
            MaskRobo_MPC,                       
            MaskWarrior_MPC,
            MaskCrimeSceneTape_MPC,      
            MaskDuctTape_MPC,              
            MaskFaceBandana_MPC,           
            MaskTShirtMask_MPC,                     
        };

        FreakMaskShopMenu = new PedClothingShopMenu();
        FreakMaskShopMenu.ID = "FreakMaskShopMenu";
        FreakMaskShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            MaskPig_MPC,          
            MaskCat_MPC,          
            MaskDino_MPC,          
            MaskFox_MPC,           
            MaskOwl_MPC,            
            MaskRaccoon_MPC,          
            MaskBear_MPC,               
            MaskBison_MPC,              
            MaskBull_MPC,                    
            MaskEagle_MPC,                   
            MaskHorse_MPC,
            MaskMonkey_MPC,
            MaskPug_MPC,        
            MaskWolf_MPC,             
            MaskUnicorn_MPC,          
            MaskVulture_MPC,        
            MaskHyena_MPC,        
            MaskMouse_MPC,       
            MaskCrazedApe_MPC,            
            MaskTurtle_MPC,             
            MaskDeath_MPM,
            MaskDeath_MPF,
            MaskFalseFace_MPC,              
            MaskFamine_MPM,
            MaskFamine_MPF,
            MaskImpotentRage_MPC,            
            MaskMoorehead_MPC,                 
            MaskPogo_MPC,
            MaskLuchador_MPC,
            MaskGargoyle1_MPC,
            MaskPrincessRobotBubblegum_MPC,     
        };

        HelmetShopMenu = new PedClothingShopMenu();
        HelmetShopMenu.ID = "HelmetShopMenu";
        HelmetShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            DirtHelmet1_MPF,
            DirtHelmet2_MPF,
            HalfHelmet1_MPF,
            FullHelmet1_MPF,
            FullHelmet2_MPF,
            FullHelmet3_MPF,
            FullHelmet4_MPF,
            FullHelmet5_MPF,
            OpenFullHelmet1_MPF,
            PilotHelmet1_MPF,

            DirtHelmet1_MPM,
            DirtHelmet2_MPM,
            HalfHelmet1_MPM,
            FullHelmet1_MPM,
            FullHelmet2_MPM,
            FullHelmet3_MPM,
            FullHelmet4_MPM,
            FullHelmet5_MPM,
            OpenFullHelmet1_MPM,
            PilotHelmet1_MPM,
        };

        GenericClothesShopMenu = new PedClothingShopMenu();
        GenericClothesShopMenu.ID = "GenericClothesShop";
        GenericClothesShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            Polo1_MPF,UnbuttonedCasual1_MPM,
            FittedT_MPF,
            LooseButton_MPF,
            LostSupported_MPF,LostSupported_MPM,
            Aviators1_MPF,Aviators1_MPM,
            Heels1_MPF,
            Chucks1_MPF,Chucks1_MPM,
            FlipFlops1_MPF,FlipFlops1_MPM,
            GoldWatch1_MPF,Watch1_MPM,
            GoldWatch2_MPF,Watch2_MPM,
            Watch3_MPM,
            DangleEarring1_MPF,DangleEarring1_MPM,
            BaseballHatProLaps1_MPF,BaseballHatProLaps1_MPM,
            BaseballHatRegular1Reverse_MPF,BaseballHatRegular1Reverse_MPM,
            BaseballHatRegular1_MPF,BaseballHatRegular1_MPM,
            StrawHat1_MPF,StrawHat1_MPM,
            Necklace1_MPF,Necklace1_MPM,
            BusinessOutfit1_MPF,BusinessOutfit1_MPM,
            BusinessOutfit2_MPF,
        };

    }
    private void MPCombined()
    {
        MPCombined_Masks();
    }
    private void MPFemale()
    {
        //Components Mostly
        MPFemale_Tops();
        MPFemale_Lower();
        MPFemale_Shoes();   
        
        MPFemale_Accessories();

        //Props?
        MPFemale_Hats();
        MPFemale_Glasses();
        MPFemale_Ears();
        MPFemale_Watches();
        MPFemale_Bracelets();

        MPFemale_Outfits();
        MPFemale_Helmets();     
    }
    private void MPMale()
    {
        MPMale_Accessories();
        MPMale_Hats();
        MPMale_Glasses();
        MPMale_Shoes();
        MPMale_Tops();
        MPMale_Lower();
        MPMale_Outfits();
        MPMale_Watches();
        MPMale_Bracelets();
        MPMale_Ears();
        MPMale_Helmets();
    }
    private void MPFemale_Accessories()
    {
        RemoveAccessories1_MPF = new PedClothingShopMenuItem("Remove Accessory", "Remove Unwanted Accessories - Ties etc..", 10, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 0, new List<int>() { 0 }), })
        {
            Category = "Accessories",
            SubCategory = "Removal",
            PedFocusZone = ePedFocusZone.Neck,
            IsAccessory = true,
        };

        Necklace1_MPF = new PedClothingShopMenuItem("Necklace 1", "", 100, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 7, new List<int>() { 0 }), })
        {
            Category = "Necklaces",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Neck,
            IsAccessory = true,
        };
    }
    private void MPMale_Accessories()
    {
        RemoveAccessories1_MPM = new PedClothingShopMenuItem("Remove Accessory", "Remove Unwanted Accessories - Ties etc..", 10, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 0, new List<int>() { 0 }), })
        {
            Category = "Accessories",
            SubCategory = "Removal",
            PedFocusZone = ePedFocusZone.Neck,
            IsAccessory = true,
        };

        Necklace1_MPM = new PedClothingShopMenuItem("Chain 1", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 17, new List<int>() { 0 }), })
        {
            Category = "Chains",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Neck,
            IsAccessory = true,
        };
    }
    private void MPFemale_Outfits()
    {
        BusinessOutfit1_MPF = new PedClothingShopMenuItem("Business Outfit 1", "", 1200, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 0, new List<int>() { 0 }),
            new PedClothingComponent(4, 24, new List<int>() { 0,1,2,3,4,5 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 19, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        //new PedClothingComponent(7, 13, new List<int>() { 0 }),
        new PedClothingComponent(8, 15, new List<int>() { 0 }){  },
        new PedClothingComponent(11, 27, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Business",
            PedFocusZone = ePedFocusZone.Body,
        };
        BusinessOutfit2_MPF = new PedClothingShopMenuItem("Business Outfit 2", "", 1400, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 5, new List<int>() { 0 }),
        new PedClothingComponent(4, 6, new List<int>() { 2 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 20, new List<int>() { 7 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 40, new List<int>() { 6 }) { AllowAllTextureVariations = true },//new PedClothingComponent(8, 23, new List<int>() { 6 }),
        new PedClothingComponent(11, 6, new List<int>() { 2 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Business",
            PedFocusZone = ePedFocusZone.Body,
        };
    }
    private void MPMale_Outfits()
    {
        BusinessOutfit1_MPM = new PedClothingShopMenuItem("The Business Man", "", 1200, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 10, new List<int>() { 0, 1, 2 }){ AllowAllTextureVariations = false },
        new PedClothingComponent(6, 10, new List<int>() { 0, 7, 12, 14  }){ AllowAllTextureVariations = false },
        new PedClothingComponent(7, 21, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 10, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 4, new List<int>() { 0, 2, 3, 11, 14 }){ AllowAllTextureVariations = false },
        })
        {
            Category = "Outfits",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Body,
        };

        /*new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 10, 0, 0),
                new PedComponent(7, 21, 2, 0),
                new PedComponent(8, 10, 0, 0),
                new PedComponent(11, 4, 0, 0)*/

        //Perseus & Deidier
        BusinessOutfit2_MPM = new PedClothingShopMenuItem("The Vested Capo", "", 1000, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 10, new List<int>() { 0, 7, 12, 14 }){ AllowAllTextureVariations = false },
        new PedClothingComponent(7, 20, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 26, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 24, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Body,
        };
        BusinessOutfit3_MPM = new PedClothingShopMenuItem("The Cashmere Don", "", 1300, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 13, new List<int>() { 0, 1 ,2 }){ AllowAllTextureVariations = false },
        new PedClothingComponent(6, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(7, 21, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 178, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 142, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Body,
        };
        BusinessOutfit4_MPM = new PedClothingShopMenuItem("The Classic Gangster", "", 1500, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 20, new List<int>() { 0, 1 ,2 }){ AllowAllTextureVariations = false },
        new PedClothingComponent(6, 105, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(7, 38, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 20, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Body,
        };
        BusinessOutfit5_MPM = new PedClothingShopMenuItem("The Continental", "", 1800, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 48, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 105, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(7, 28, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 31, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 99, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Body,
        };

        //Binco
        BincoOutfit1_MPM = new PedClothingShopMenuItem("The Worker", "Hinterland Work Outfit", 400, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 0, new List<int>() { 0 }),
        new PedClothingComponent(4, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 12, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 123, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Body,
        };
        BincoOutfit2_MPM = new PedClothingShopMenuItem("The Weekend Warrior", "Combat Ready!", 800, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 15, new List<int>() { 0 }),
        new PedClothingComponent(4, 31, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 62, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 50, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Body,
        };
        BincoOutfit3_MPM = new PedClothingShopMenuItem("The Garbage Man", "Get Down and Dirty", 250, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 0, new List<int>() { 0 }),
        new PedClothingComponent(4, 36, new List<int>() { 0 }),
        new PedClothingComponent(6, 27, new List<int>() { 0 }),
        new PedClothingComponent(8, 59, new List<int>() { 0 }),
        new PedClothingComponent(11, 56, new List<int>() { 0 }),
        })
        {
            Category = "Outfits",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Body,
        };


        //Discount
        DiscoOutfit1_MPM = new PedClothingShopMenuItem("Discount Outfit 1", "Streetwear", 170, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 0, new List<int>() { 0 }),
        new PedClothingComponent(4, 0, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 76, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 167, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Body,
        };
        DiscoOutfit2_MPM = new PedClothingShopMenuItem("Discount Outfit 2", "Full Tracksuit", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 6, new List<int>() { 0 }),
        new PedClothingComponent(4, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 8, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Body,
        };
        DiscoOutfit3_MPM = new PedClothingShopMenuItem("Discount Outfit 3", "Beach Outfit", 80, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 5, new List<int>() { 0 }),
        new PedClothingComponent(4, 16, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 1, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Body,
        };

        //Suburban
        SuburbOutfit1_MPM = new PedClothingShopMenuItem("Street Outfit 1", "", 300, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 6, new List<int>() { 0 }),
        new PedClothingComponent(4, 0, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 32, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 135, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 167, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Body,
        };
        SuburbOutfit2_MPM = new PedClothingShopMenuItem("Street Outfit 2", "", 180, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 6, new List<int>() { 0 }),
        new PedClothingComponent(4, 4, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 12, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Body,
        };

    }
    private void MPFemale_Ears()
    {
        DangleEarring1_MPF = new PedClothingShopMenuItem("Dangle Earring", "", 67, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(2, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Earrings",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftEar
        };
    }
    private void MPMale_Ears()
    {
        DangleEarring1_MPM = new PedClothingShopMenuItem("Simple Earring", "", 67, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(2, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true }, })
        {
            Category = "Earrings",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.LeftEar
        };
    }
    private void MPFemale_Tops()
    {
        Polo1_MPF = new PedClothingShopMenuItem("Branded Polo Shirt", "Show you are worth it.", 50, new List<string>() { "mp_f_freemode_01" }, 
            new List<PedClothingComponent>() { 
                new PedClothingComponent(11, 14, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }) { AllowAllTextureVariations = true, }, 
                new PedClothingComponent(8, 15, new List<int>() { 0 }), 
                new PedClothingComponent(3, 14, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "Polo",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        FittedT_MPF = new PedClothingShopMenuItem("Fitted T Shirt", "Test Description", 50, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(11, 49, new List<int>() { 1, 0 }) { AllowAllTextureVariations = true, }, 
            new PedClothingComponent(8, 15, new List<int>() { 0 }), 
            new PedClothingComponent(3, 14, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LooseButton_MPF = new PedClothingShopMenuItem("Loose Button-Up", "Test Description 2", 60, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(11, 9, new List<int>() { 13 }) {AllowAllTextureVariations = true, }, 
            new PedClothingComponent(8, 15, new List<int>() { 0 }), 
            new PedClothingComponent(3, 9, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "Button Up",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        LostSupported_MPF = new PedClothingShopMenuItem("LOST Supporter", "Test Description 3", 60, new List<string>() { "mp_f_freemode_01" },
        new List<PedClothingComponent>() {
                new PedClothingComponent(11, 385, new List<int>() { 0 }),
                new PedClothingComponent(8, 86, new List<int>() { 22,23,24 }),
                new PedClothingComponent(3, 0, new List<int>() { 0 }) , })
        {
            Category = "Shirts",
            SubCategory = "Cuts",
            PedFocusZone = ePedFocusZone.Chest,
            ForceSetOverlays = new List<AppliedOverlay>() {
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_028_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_029_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_030_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_031_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_034_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_035_F","ZONE_TORSO"),
                },
            RemoveTorsoDecals = true,
        };
    }
    private void MPMale_Tops()
    {
        UnbuttonedCasual1_MPM = new PedClothingShopMenuItem("Unbuttoned Casual Shirt", "Show you are worth it.", 50, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
                new PedClothingComponent(11, 346, new List<int>() { 0 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(8, 0, new List<int>() { 0 }),
                new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        

        LostSupported_MPM = new PedClothingShopMenuItem("LOST Supporter", "Test Description 3", 60, new List<string>() { "mp_m_freemode_01" },
        new List<PedClothingComponent>() {
                new PedClothingComponent(11, 366, new List<int>() { 0 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(8, 81, new List<int>() { 22,23,24 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(3, 0, new List<int>() { 0 }) , })
        {
            Category = "Shirts",
            SubCategory = "Cuts",
            PedFocusZone = ePedFocusZone.Chest,
            ForceSetOverlays = new List<AppliedOverlay>() {
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_028_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_029_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_030_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_031_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_034_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_035_M","ZONE_TORSO"),
                },
            RemoveTorsoDecals = true,
        };

        // Torso ID's

        // new PedClothingComponent(3, 0, new List<int>() { 0 }) , }) // T- Shirts & Cuts
        // new PedClothingComponent(3, 1, new List<int>() { 0 }) , }) // Shirts & Jumpers & Jackets
        // new PedClothingComponent(3, 2, new List<int>() { 0 }) , }) // 
        // new PedClothingComponent(3, 5, new List<int>() { 0 }) , }) // Arms - Chest - Sleeveless Shirts Vests & Tank Tops
        // new PedClothingComponent(3, 8, new List<int>() { 0 }) , }) // Arms below elbow


        // new PedClothingComponent(3, 11, new List<int>() { 0 }) , }) // Long Sleeve Shirts & T -Shirts 
        // new PedClothingComponent(3, 14, new List<int>() { 0 }) , }) // Torso with hands
        // new PedClothingComponent(3, 15, new List<int>() { 0 }) , }) // Full Torso

        //Undershirt ID's
        // new PedClothingComponent(8, 0, new List<int>() { 0 }) , }) // UnderShirt - Plain White T-Shirt
        // new PedClothingComponent(8, 15, new List<int>() { 0 }) , }) // No Undershirt

        //Jumpers
        CombatJumper1_MPM = new PedClothingShopMenuItem("Combat Jumper", "", 85, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 50, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignJumper1_MPM = new PedClothingShopMenuItem("Designer Sweater", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 78, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PlainJumper1_MPM = new PedClothingShopMenuItem("Plain Sweater", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 89, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TurtleJumper1_MPM = new PedClothingShopMenuItem("Turtleneck Sweater", "", 50, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 139, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 75, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignJumper2_MPM = new PedClothingShopMenuItem("Designer Sweater 2", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 190, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignJumper3_MPM = new PedClothingShopMenuItem("Designer Sweater 3", "", 95, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 307, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignJumper4_MPM = new PedClothingShopMenuItem("Designer Sweater 4", "", 130, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 358, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweaters",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        // TShirts / Tank tops
        PlainTshirt1_MPM = new PedClothingShopMenuItem("Plain T-Shirt", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 0, new List<int>() { 0, 1, 2, 3, 4, 5, 7, 8, 11 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 0, new List<int>() { 0, 1, 2, 3, 4, 5, 7, 8, 11 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        VNeckTshirt1_MPM = new PedClothingShopMenuItem("Plain V-Neck", "", 28, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 1, new List<int>() { 0, 1, 3, 4, 5, 6, 7, 8, 11, 12, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 1, new List<int>() { 0, 1, 3, 4, 5, 6, 7, 8, 11, 12, 14 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Tankshirt1_MPM = new PedClothingShopMenuItem("Tank Top", "", 20, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 5, new List<int>() { 0, 1, 2, 7 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 5, new List<int>() { 0, 1, 2, 7 }),
            new PedClothingComponent(3, 5, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PlainTshirt2_MPM = new PedClothingShopMenuItem("Two Tone Tee-Shirt", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 8, new List<int>() { 0, 10, 13, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 8, new List<int>() { 0, 10, 13, 14 }),
            new PedClothingComponent(3, 8, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PoloTshirt1_MPM = new PedClothingShopMenuItem("Polo T-Shirt", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 9, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 9, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Tankshirt2_MPM = new PedClothingShopMenuItem("Beach Tank Top", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 5, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TeeTshirt1_MPM = new PedClothingShopMenuItem("Tee-Shirt", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 38, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 41, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 8, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignerTshirt1_MPM = new PedClothingShopMenuItem("Designer T-Shirt", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 73, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 65, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        BaggyTshirt1_MPM = new PedClothingShopMenuItem("Baggy T-Shirt", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 80, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        WorkShirt1_MPM = new PedClothingShopMenuItem("Hinterland Work Shirt", "", 35, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 123, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignerTshirt2_MPM = new PedClothingShopMenuItem("Designer Long T-Shirt", "", 50, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 193, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        //Hoodies
        Hoodie1_MPM = new PedClothingShopMenuItem("Open Hoodie", "", 35, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 14, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie2_MPM = new PedClothingShopMenuItem("Grey Baggy Hoodie", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 57, new List<int>() { 0 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie3_MPM = new PedClothingShopMenuItem("Sports Hoodie", "", 50, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 86, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie4_MPM = new PedClothingShopMenuItem("Hippy Hoodie", "", 25, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 121, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie5_MPM = new PedClothingShopMenuItem("Leather Hoodie", "", 65, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 168, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie6_MPM = new PedClothingShopMenuItem("Longline Hoodie", "", 75, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 187, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie7_MPM = new PedClothingShopMenuItem("Sleeveless Hoodie", "", 35, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 205, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 5, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie8_MPM = new PedClothingShopMenuItem("Guffy Hoodie", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 200, new List<int>() { 5, 6, 7, 8, 16, 17 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Hoodie9_MPM = new PedClothingShopMenuItem("Bigness Hoodie", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 200, new List<int>() { 9, 10, 11, 12, 13, 21 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Hoodies",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };




        // Jackets & Coats  
        LeatherJacket1_MPM = new PedClothingShopMenuItem("Striped Leather Jacket", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 6, new List<int>() { 0, 1, 3, 4, 5, 8, 9, }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        //Suede Version of the above Jacket,
        LeatherJacket2_MPM = new PedClothingShopMenuItem("Suede Jacket", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 6, new List<int>() { 11 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJacket3_MPM = new PedClothingShopMenuItem("Black Fitted Leather Jacket", "", 80, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 62, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJacket4_MPM = new PedClothingShopMenuItem("Black Full Leather Jacket", "", 90, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 64, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJacket5_MPM = new PedClothingShopMenuItem("Leather Fur Jacket", "", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 70, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJacket6_MPM = new PedClothingShopMenuItem("Bright Leather Fur Jacket", "", 280, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 240, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJacket7_MPM = new PedClothingShopMenuItem(" Leather Field Coat ", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 138, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Leather",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        CoatJacket1_MPM = new PedClothingShopMenuItem("Wool Coat", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 72, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CoatJacket2_MPM = new PedClothingShopMenuItem("Trench Coat", "", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 76, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CoatJacket3_MPM = new PedClothingShopMenuItem("Over-Coat", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 77, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CoatJacket4_MPM = new PedClothingShopMenuItem("Grey Cashmere Coat", "", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 115, new List<int>() { 0 }),
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CoatJacket5_MPM = new PedClothingShopMenuItem("Cashmere Coat", "", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 142, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CoatJacket6_MPM = new PedClothingShopMenuItem("Designer Cashmere Coat", "", 280, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 192, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Coat",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        PufferJacket1_MPM = new PedClothingShopMenuItem("Plain Puffer Jacket", "", 80, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 167, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Puffer",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PufferJacket2_MPM = new PedClothingShopMenuItem("Designer Puffer Jacket", "", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 191, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Puffer",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PufferJacket3_MPM = new PedClothingShopMenuItem("Vibrant Puffer Jacket ", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 269, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Puffer",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        PufferJacket4_MPM = new PedClothingShopMenuItem("Designer Puffer Jacket 2", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 309, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Puffer",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        //Shirts
        LooseShirt1_MPM = new PedClothingShopMenuItem("Untucked Shirt", "", 55, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 12, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 12, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TuckedShirt1_MPM = new PedClothingShopMenuItem("Rolled-Up Sleeve Shirt", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 13, new List<int>() { 0, 1, 2, 3, 5 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 13, new List<int>() { 0, 1, 2, 3, 5}),
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        CheckShirt1_MPM = new PedClothingShopMenuItem("Check  Shirt", "Shirt with Undershirt", 70, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 14, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 29, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TuckedShirt2_MPM = new PedClothingShopMenuItem("Tucked Shirt", "", 55, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 26, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 27, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        BraceShirt1_MPM = new PedClothingShopMenuItem("Denim Shirt & Braces", "", 30, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 42, new List<int>() { 0 }),
            new PedClothingComponent(8, 45, new List<int>() { 0 }),
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        BraceShirt2_MPM = new PedClothingShopMenuItem("Denim Shirt & Braces", "Un-Buttoned Collar", 30, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 43, new List<int>() { 0 }),
            new PedClothingComponent(8, 46, new List<int>() { 0 }),
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        FloralShirt1_MPM = new PedClothingShopMenuItem("Floral Loose Shirt", "", 65, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 105, new List<int>() { 0 }),
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        DesignerShirt1_MPM = new PedClothingShopMenuItem("Designer Loose Shirt", "", 75, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 135, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        OfficeShirt1_MPM = new PedClothingShopMenuItem("Office Un-Buttoned Shirt", "Un-Buttoned Collar", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 348, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 179, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        OfficeShirt2_MPM = new PedClothingShopMenuItem("Office Shirt", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 349, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 178, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };



        //Sportswear Jerseys and Jackets
        PounderJersey1_MPM = new PedClothingShopMenuItem("LS Pounders Jersey", "", 85, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 2, new List<int>() { 9 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 5, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TrackJacket1_MPM = new PedClothingShopMenuItem("Track Jacket", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LeatherJersey1_MPM = new PedClothingShopMenuItem("Leather Sports Jersey", "", 85, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 164, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        SportsJersey1_MPM = new PedClothingShopMenuItem("Football Shirt", "", 55, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 81, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        SportsJersey2_MPM = new PedClothingShopMenuItem("Baseball Shirt", "", 55, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 83, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        SportsJersey3_MPM = new PedClothingShopMenuItem("Wind Shirt", "", 65, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 84, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 0, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TrackJacket2_MPM = new PedClothingShopMenuItem("Tracksuit Jacket", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 113, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TrackJacket3_MPM = new PedClothingShopMenuItem("Tracksuit Jacket 2", "", 45, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 141, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        TrackJacket4_MPM = new PedClothingShopMenuItem("Designer Tracksuit Jacket", "", 75, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 257, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 15, new List<int>() { 0 }),
            new PedClothingComponent(3, 6, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Sportswear",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        // Suit Jackets
        SuitJacket1_MPM = new PedClothingShopMenuItem("Sports Coat", "A classic business essential.", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 4, new List<int>() { 0, 2, 3, 11, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Suits",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        SuitJacket2_MPM = new PedClothingShopMenuItem("Buttoned-Up Jacket", "Premium formal evening wear.", 300, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 10, new List<int>() { 0, 1 , 2 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Suits",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        SuitJacket3_MPM = new PedClothingShopMenuItem("Tailored Jacket", "Semi-formal designer blazer.", 260, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 28, new List<int>() { 0, 1 , 2 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Suits",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        SuitJacket4_MPM = new PedClothingShopMenuItem("Pin Stripe Jacket", "", 350, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 20, new List<int>() { 1, 2, 3 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Suits",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        SuitJacket5_MPM = new PedClothingShopMenuItem("Classic Ivory Jacket", "", 320, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 20, new List<int>() { 0 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(3, 4, new List<int>() { 0 }), })
        {
            Category = "Jackets",
            SubCategory = "Suits",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        //Sweatervests with shirts

        Sweat1Vest1_MPM = new PedClothingShopMenuItem("Tailored Vest with Short Sleeve Shirt", "", 140, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 109, new List<int>() { 0 }),
            new PedClothingComponent(8, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat1Vest2_MPM = new PedClothingShopMenuItem("Designer Sweater Vest with Short Sleeve Shirt", "Un-Buttoned Collar", 140, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 109, new List<int>() { 0 }),
            new PedClothingComponent(8, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat1Vest3_MPM = new PedClothingShopMenuItem("Designer Sweater Vest with Band Vest Shirt", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 109, new List < int >() { 0 }),
            new PedClothingComponent(8, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat1Vest4_MPM = new PedClothingShopMenuItem("Designer Sweater Vest with White Shirt", "Un-Buttoned Collar", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 109, new List < int >() { 0 }),
            new PedClothingComponent(8, 157, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat1Vest5_MPM = new PedClothingShopMenuItem("White Sweater Vest with White Shirt", "", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 109, new List < int >() { 0 }),
            new PedClothingComponent(8, 158, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        Sweat2Vest1_MPM = new PedClothingShopMenuItem("Designer Sweatervest with Short Sleeve Shirt", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat2Vest2_MPM = new PedClothingShopMenuItem("Designer Sweatervest with Short Sleeve Shirt", "Un-Buttoned Collar", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat2Vest3_MPM = new PedClothingShopMenuItem("Designer Sweatervest with Band Vest Shirt", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat2Vest4_MPM = new PedClothingShopMenuItem("Designer Sweatervest with White Shirt", "Un-Buttoned Collar", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 157, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        Sweat2Vest5_MPM = new PedClothingShopMenuItem("Designer Sweatervest with White Shirt", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 158, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Sweatervests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        //Vests with Shirts

        FormalVest1_MPM = new PedClothingShopMenuItem("Tailored Vest with Short Sleeve Shirt", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 11, new List<int>() { 0, 1, 7, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        FormalVest2_MPM = new PedClothingShopMenuItem("Tailored Vest with Short Sleeve Shirt", "Un-Buttoned Collar", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 11, new List<int>() { 0, 1, 7, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        FormalVest3_MPM = new PedClothingShopMenuItem("Tailored Vest with Band Vest Shirt", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 11, new List<int>() { 0, 1, 7, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        FormalVest4_MPM = new PedClothingShopMenuItem("Tailored Vest with White Shirt", "Un-Buttoned Collar", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 11, new List<int>() { 0, 1, 7, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 157, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        FormalVest5_MPM = new PedClothingShopMenuItem("Tailored Vest with White Shirt", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 11, new List<int>() { 0, 1, 7, 14 }){ AllowAllTextureVariations = false },
            new PedClothingComponent(8, 158, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        WatchVest1_MPM = new PedClothingShopMenuItem("Pocket-Watch Vest with Short Sleeve Shirt", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        WatchVest2_MPM = new PedClothingShopMenuItem("Pocket-Watch Vest with Short Sleeve Shirt", "Un-Buttoned Collar", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        WatchVest3_MPM = new PedClothingShopMenuItem("Pocket-Watch Vest with Band Vest Shirt", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        WatchVest4_MPM = new PedClothingShopMenuItem("Pocket-Watch Vest with White Shirt", "Un-Buttoned Collar", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 157, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        WatchVest5_MPM = new PedClothingShopMenuItem("Pocket-Watch Vest with White Shirt", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(11, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true },
            new PedClothingComponent(8, 158, new List<int>() { 0 }),
            new PedClothingComponent(3, 1, new List<int>() { 0 }), })
        {
            Category = "Tops",
            SubCategory = "Vests",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };



    }
    private void MPMale_Lower()
    {

        // Jeans & Casual Pants
        RegularJeans1_MPM = new PedClothingShopMenuItem("Regular Fit Jeans", "", 80, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 0, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        WashedOutJeans1_MPM = new PedClothingShopMenuItem("Faded-Out Jeans", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 1, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SlimFitJeans1_MPM = new PedClothingShopMenuItem("Slim Fit Jeans", "", 175, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 4, new List<int>() { 0, 1, 2, 4 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BaggyChinos1_MPM = new PedClothingShopMenuItem("Baggy Chinos", "", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 8, new List<int>() { 0, 3, 4, 14 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SlimFitJeans2_MPM = new PedClothingShopMenuItem("Slim Fitted Stylized Jeans", "", 200, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 26, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LooseJeans2_MPM = new PedClothingShopMenuItem("Loose Jeans", "", 130, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 43, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ClassicBlueJeans1_MPM = new PedClothingShopMenuItem("Classic Blue Jeans", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 63, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RibbedJeans1_MPM = new PedClothingShopMenuItem("Ribbed Jeans", "", 70, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 75, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RoadWornJeans1_MPM = new PedClothingShopMenuItem("Roadworn Jeans", "", 55, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 76, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        DenimOveralls1_MPM = new PedClothingShopMenuItem("Denim Overalls", "", 85, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 90, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Overalls",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StraightChinoPants1_MPM = new PedClothingShopMenuItem("Straight Chinos ", "", 250, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 141, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StraightChinoPants2_MPM = new PedClothingShopMenuItem("Stylized Straight Chinos ", "", 280, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 142, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BeachChinoPants1_MPM = new PedClothingShopMenuItem("Beach Chinos ", "", 240, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 143, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };


        //Leather Pants & Chaps - Biker Boys
        LeatherPants1_MPM = new PedClothingShopMenuItem("Plain Leather Pants", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 71, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants2_MPM = new PedClothingShopMenuItem("Tucked-In Plain Leather Pants", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 72, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PaddedLeatherPants1_MPM = new PedClothingShopMenuItem("Padded Leather Pants", "", 250, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 73, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PaddedLeatherPants2_MPM = new PedClothingShopMenuItem("Tucked-In Padded Leather Pants", "", 250, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 74, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StitchLeatherPants1_MPM = new PedClothingShopMenuItem("Leather Stitch Pants", "", 180, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 105, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Sports Pants & Joggers
        TrackPants1_MPM = new PedClothingShopMenuItem("Track Pants", "", 50, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SweatPants1_MPM = new PedClothingShopMenuItem("Sweat Pants", "", 35, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 5, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TrackPants2_MPM = new PedClothingShopMenuItem("ProLaps Tracksuit Pants", "", 35, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 55, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TrackPants3_MPM = new PedClothingShopMenuItem("ProLaps 2 Stripe Tracksuit Pants", "", 55, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 64, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MusclePants1_MPM = new PedClothingShopMenuItem("Muscle Pants", "", 85, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 100, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TieDyePants1_MPM = new PedClothingShopMenuItem("Tie-Dye Sports Pants", "", 185, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 131, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CuffedPants1_MPM = new PedClothingShopMenuItem("Cuffed Sports Pants", "", 55, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 138, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CuffedPants2_MPM = new PedClothingShopMenuItem("Cuffed Stylized Sports Pants", "", 155, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 139, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };

        // Male Suit Trousers & Slacks
        BeltedTrousers_MPM = new PedClothingShopMenuItem("Belted Trousers", "", 180, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 10, new List<int>() { 0, 1 ,2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BaggyBeltedTrousers_MPM = new PedClothingShopMenuItem("Baggy Belted Trousers", "", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 13, new List<int>() { 0, 1 ,2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ClassicSuitTrousers_MPM = new PedClothingShopMenuItem("Classic Suit Trousers", "Traditional pinstripe luxury trousers.", 110, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 20, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RegularSuitTrousers_MPM = new PedClothingShopMenuItem("Regular Trousers", "", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 22, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BaggySuitTrousers_MPM = new PedClothingShopMenuItem("Baggy Trousers", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 23, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SlimSuitTrousers_MPM = new PedClothingShopMenuItem("Slim Fit Suit Trousers", "Modern tailored trousers for a sleek profile.", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 24, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RegularSuitPants_MPM = new PedClothingShopMenuItem("Regular Suit Trousers", "Casual trousers for a relaxed look.", 95, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 25, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SlimFitTrousers_MPM = new PedClothingShopMenuItem("Slim Fit  Trousers", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 28, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TuxedoPants_MPM = new PedClothingShopMenuItem("Tuxedo Trousers", "Formal trousers with a silk side-stripe.", 250, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 35, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ScruffySuitPants_MPM = new PedClothingShopMenuItem("Scruffy Suit Pants", "", 120, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 37, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ContinentalPants_MPM = new PedClothingShopMenuItem("Continental Pants", "", 280, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 48, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ContinentalSlimPants_MPM = new PedClothingShopMenuItem("Continental Slim Pants", "", 320, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 49, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ShinyPants_MPM = new PedClothingShopMenuItem("Shiny Pants", "", 350, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 50, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        GoldPrintPants_MPM = new PedClothingShopMenuItem("Gold Print Pants", "", 350, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 51, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ShinyFittedPants_MPM = new PedClothingShopMenuItem("Shiny Slim Pants", "", 400, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 52, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        GoldPrintFittedPants_MPM = new PedClothingShopMenuItem("Gold Print Slim Pants", "", 400, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 53, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ClassicSuitTrousers2_MPM = new PedClothingShopMenuItem("Classic Suit Trousers", "", 175, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 60, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        HighRollerTrousers_MPM = new PedClothingShopMenuItem("High Roller Trousers", "", 375, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 116, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        WideTrousers_MPM = new PedClothingShopMenuItem("Wide Suit Trousers", "", 325, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 118, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };

        // Shorts
        TwoToneShorts1_MPM = new PedClothingShopMenuItem("Grey Two-Tone Shorts", "", 40, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 2, new List<int>() { 11 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BoardShorts1_MPM = new PedClothingShopMenuItem("Sqaure Boards Shorts", "", 45, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 6, new List<int>() { 0, 1, 2, 10 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChinoShorts1_MPM = new PedClothingShopMenuItem("Chino Shorts", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 12, new List<int>() { 0, 4, 5, 7, 12 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RunningShorts1_MPM = new PedClothingShopMenuItem("Running Shorts", "", 50, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 14, new List<int>() { 0, 1, 3, 12 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoShorts1_MPM = new PedClothingShopMenuItem("Cargo Shorts", "", 80, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BoardShorts2_MPM = new PedClothingShopMenuItem("Beach Boards Shorts", "", 55, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 16, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChinoShorts2_MPM = new PedClothingShopMenuItem("Smart Chino Shorts", "", 75, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RunningShorts1_MPM = new PedClothingShopMenuItem("Beach Running Shorts", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 18, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        JoggingShorts1_MPM = new PedClothingShopMenuItem("Jogging Shorts", "", 45, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 42, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SwimShorts1_MPM = new PedClothingShopMenuItem("Swim Shorts", "", 50, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 54, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        WorkShorts1_MPM = new PedClothingShopMenuItem("Work Shorts", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 62, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChainShorts1_MPM = new PedClothingShopMenuItem("Chain Cargo Shorts", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 103, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        KneeShorts1_MPM = new PedClothingShopMenuItem("Knee Shorts", "", 75, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 117, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BasketballShorts1_MPM = new PedClothingShopMenuItem("Basketball Shorts", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 132, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        JeanShorts1_MPM = new PedClothingShopMenuItem("Denim Shorts", "", 50, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 145, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Underwear
        LoveHeartUnderwear1_MPM = new PedClothingShopMenuItem("Love Hear Boxers", "", 35, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 21, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LongJohnsUnderwear1_MPM = new PedClothingShopMenuItem("Long Johns", "", 55, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 32, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BoxersUnderwear1_MPM = new PedClothingShopMenuItem("Boxers", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 61, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        UFOBoxersUnderwear1_MPM = new PedClothingShopMenuItem("UFO Boxers", "", 45, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 147, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Utility & Cargo Pants
        WorkPants1_MPM = new PedClothingShopMenuItem("Work Pants", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants1_MPM = new PedClothingShopMenuItem("Cargo Pants", "", 145, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 9, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CombatPants1_MPM = new PedClothingShopMenuItem("Combat Pants", "", 265, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 31, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        UtilityPants1_MPM = new PedClothingShopMenuItem("Utility Pants", "", 125, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 47, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChainPants1_MPM = new PedClothingShopMenuItem("Chain Cargo Pants", "", 125, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 102, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants2_MPM = new PedClothingShopMenuItem("Large Cargo Pants", "", 185, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 129, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants3_MPM = new PedClothingShopMenuItem("Tucked-In Large Cargo Pants", "", 185, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 130, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };


    }
    private void MPFemale_Lower()
    {

        //Formal Suits Pants  Trousers
        SuitPants1_MPF = new PedClothingShopMenuItem("Suit Pants", "", 165, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 6, new List<int>() { 0, 1, 2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SuitPants2_MPF = new PedClothingShopMenuItem("Tailored Suit Pants", "", 185, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 23, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SuitPants3_MPF = new PedClothingShopMenuItem("Regular Suit Pants", "", 135, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 37, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SuitPants4_MPF = new PedClothingShopMenuItem("Scruffy Suit Pants", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 41, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ContinentalPants_MPF = new PedClothingShopMenuItem("Continental Pants", "", 280, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 50, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ContinentalSlimPants_MPF = new PedClothingShopMenuItem("Continental Slim Pants", "", 320, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 51, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ShinyPants_MPF = new PedClothingShopMenuItem("Shiny Pants", "", 350, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 52, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        GoldPrintPants_MPF = new PedClothingShopMenuItem("Gold Print Pants", "", 350, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 53, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ShinyFittedPants_MPF = new PedClothingShopMenuItem("Shiny Slim Pants", "", 400, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 54, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        GoldPrintFittedPants_MPF = new PedClothingShopMenuItem("Gold Print Slim Pants", "", 400, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 55, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        WideTrousers_MPF = new PedClothingShopMenuItem("Wide Suit Trousers", "", 325, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 124, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SlackPants_MPF = new PedClothingShopMenuItem("Slacks", "", 125, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 133, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Legs,
        };

        // Jeans & Casual Pants
        SkinnyJeans1_MPF = new PedClothingShopMenuItem("Skinny Jeans", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 0, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RegularJeans1_MPF = new PedClothingShopMenuItem("Regular Fit Jeans", "", 80, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 1, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChinosPants1_MPF = new PedClothingShopMenuItem("Chino Trousers", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CroppedJeans1_MPF = new PedClothingShopMenuItem("Cropped Jeans", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 4, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChinosPants2_MPF = new PedClothingShopMenuItem("Fitted Chinos", "", 130, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 64, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RibbedJeans1_MPF = new PedClothingShopMenuItem("Ribbed Jeans", "", 70, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 73, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        RoadWornJeans1_MPF = new PedClothingShopMenuItem("Roadworn Jeans", "", 55, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 74, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Jeans",
            PedFocusZone = ePedFocusZone.Legs,
        };
        DenimOveralls1_MPF = new PedClothingShopMenuItem("Denim Overalls", "", 85, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 93, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Overalls",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StraightChinoPants1_MPF = new PedClothingShopMenuItem("Straight Chinos ", "", 250, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 148, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StraightChinoPants2_MPF = new PedClothingShopMenuItem("Stylized Straight Chinos ", "", 280, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 149, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BeachChinoPants1_MPF = new PedClothingShopMenuItem("Beach Chinos ", "", 240, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 150, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Chinos",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Leathers
        LeatherPants1_MPF = new PedClothingShopMenuItem("Leather Zipper Pants", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 43, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants2_MPF = new PedClothingShopMenuItem("Leather Skinny-Cut Pants", "", 135, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 44, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants3_MPF = new PedClothingShopMenuItem("Plain Leather Leggings", "", 100, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 75, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants4_MPF = new PedClothingShopMenuItem("Quilted Leather Leggings", "", 170, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 76, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants5_MPF = new PedClothingShopMenuItem("Ribbed Leather Leggings", "", 150, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 77, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeatherPants6_MPF = new PedClothingShopMenuItem("Colorful Leather Zipper Pants", "", 130, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 106, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leathers",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Leggings
        CargoPants1_MPF = new PedClothingShopMenuItem("Cargo Leggings", "", 135, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 11, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leggings",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeggingsPants1_MPF = new PedClothingShopMenuItem("Stylized Leggings", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 27, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leggings",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeggingsPants2_MPF = new PedClothingShopMenuItem("Camo Leggings", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 87, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leggings",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeggingsPants3_MPF = new PedClothingShopMenuItem("Denim Leggings", "", 80, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 160, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leggings",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LeggingsPants4_MPF = new PedClothingShopMenuItem("Stylized Denim Leggings", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 166, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Leggings",
            PedFocusZone = ePedFocusZone.Legs,
        };

        // Shorts
        eShorts1_MPF = new PedClothingShopMenuItem("eShorts", "", 40, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 10, new List<int>() { 0, 1, 2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PlainShorts1_MPF = new PedClothingShopMenuItem("Plain Shorts", "", 35, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 14, new List<int>() { 0, 1, 8, 9 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ClassicShorts1_MPF = new PedClothingShopMenuItem("Classic Shorts", "", 45, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 16, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        DenimShorts1_MPF = new PedClothingShopMenuItem("Denim Shorts", "", 50, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 25, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        DenimShorts2_MPF = new PedClothingShopMenuItem("Denim Shorts and Stockings", "", 80, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 78, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoShorts1_MPF = new PedClothingShopMenuItem("Cargo Shorts", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 91, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BeachShorts1_MPF = new PedClothingShopMenuItem("Beach Shorts", "", 65, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 107, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChainShorts1_MPF = new PedClothingShopMenuItem("Chain Cargo Shorts", "", 85, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 110, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        KneeShorts1_MPF = new PedClothingShopMenuItem("Knee Shorts", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 123, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChinoShorts1_MPF = new PedClothingShopMenuItem("Chino Shorts", "", 65, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 137, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        BasketballShorts1_MPF = new PedClothingShopMenuItem("Basketball Shorts", "", 100, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 139, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        JeanShorts1_MPF = new PedClothingShopMenuItem("Long Denim Shorts", "", 50, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 154, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Shorts",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Skirts
        PencilSkirt1_MPF = new PedClothingShopMenuItem("Pencil Skirt", "", 85, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 7, new List<int>() { 0, 1 , 2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MiniSkirt1_MPF = new PedClothingShopMenuItem("Mini-Skirt", "", 125, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 8, new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,12,14,15 }){ AllowAllTextureVariations = false },})  // No texture for id 13 the rest work - set like this to hide it.
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MiniSkirt2_MPF = new PedClothingShopMenuItem("Sequin Mini-Skirt", "", 175, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 9, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PleatedSkirt1_MPF = new PedClothingShopMenuItem("Pleated Skirt", "", 135, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 12, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PencilSkirt2_MPF = new PedClothingShopMenuItem("Stylized Pencil Skirt", "", 200, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 24, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MiniSkirt3_MPF = new PedClothingShopMenuItem("Leopard Print Mini-Skirt", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 26, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PencilSkirt3_MPF = new PedClothingShopMenuItem("Cheap Pencil Skirt", "", 70, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 36, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MiniSkirt4_MPF = new PedClothingShopMenuItem("Luxury Sequin Mini-Skirt", "", 250, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 108, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        DenimSkirt1_MPF = new PedClothingShopMenuItem("Mini-Skirt", "", 60, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 155, new List<int>() { 0}){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };
        PleatedSkirt2_MPF = new PedClothingShopMenuItem("Colorful Pleated Skirt", "", 155, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 159, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Skirts",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Sports Pants 
        RolledUpPants1_MPF = new PedClothingShopMenuItem("Rolled Up Pants", "", 55, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 2, new List<int>() { 0, 1 , 2 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TrackPants1_MPF = new PedClothingShopMenuItem("Tracksuit Pants", "", 35, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 58, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TrackPants2_MPF = new PedClothingShopMenuItem("Colorful Tracksuit Pants", "", 55, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 66, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        MusclePants1_MPF = new PedClothingShopMenuItem("Muscle Pants", "", 85, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 104, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        SportsPants1_MPF = new PedClothingShopMenuItem("Sports Track Pants", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 134, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TieDyePants1_MPF = new PedClothingShopMenuItem("Tie-Dye Sports Pants", "", 185, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 138, new List<int>() { 0 }){ AllowAllTextureVariations = false },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CuffedPants1_MPF = new PedClothingShopMenuItem("Cuffed Sports Pants", "", 55, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 145, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CuffedPants2_MPF = new PedClothingShopMenuItem("Cuffed Stylized Sports Pants", "", 155, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 146, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };
        TrackPants3_MPF = new PedClothingShopMenuItem("Button-Up Track Pants", "", 65, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 185, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Sports",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Underwear/ Bikini Bottoms
        Bikini1_MPF = new PedClothingShopMenuItem("Bikini Bottoms", "", 35, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 17, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Bikinis",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LaceUnderwear1_MPF = new PedClothingShopMenuItem("Lace Panties", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 19, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StockingsUnderwear1_MPF = new PedClothingShopMenuItem("Stockings", "", 125, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 20, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        Bikini2_MPF = new PedClothingShopMenuItem("Stylized Bikini Bottoms", "", 90, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 56, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Bikinis",
            PedFocusZone = ePedFocusZone.Legs,
        };
        LaceUnderwear2_MPF = new PedClothingShopMenuItem("Luxury Lace Panties", "", 120, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 62, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        StockingsUnderwear2_MPF = new PedClothingShopMenuItem("Luxury Stockings", "", 140, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 63, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };
        UFOBoxersUnderwear1_MPF = new PedClothingShopMenuItem("UFO Boxers", "", 45, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 152, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Underwear",
            PedFocusZone = ePedFocusZone.Legs,
        };

        //Utility Pants
        CombatPants1_MPF = new PedClothingShopMenuItem("Combat Pants", "", 150, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 30, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants2_MPF = new PedClothingShopMenuItem("Baggy Cargo Pants", "", 85, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 45, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        UtilityPants1_MPF = new PedClothingShopMenuItem("Utility Pants", "", 145, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 49, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        ChainPants1_MPF = new PedClothingShopMenuItem("Chain Cargo Pants", "", 125, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 109, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants3_MPF = new PedClothingShopMenuItem("Large Cargo Pants", "", 185, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 135, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };
        CargoPants4_MPF = new PedClothingShopMenuItem("Tucked-In Large Cargo Pants", "", 185, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(4, 136, new List<int>() { 0 }){ AllowAllTextureVariations = true },})
        {
            Category = "Bottoms",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Legs,
        };

    }
    private void MPFemale_Hats()
    {
        BaseballHatProLaps1_MPF = new PedClothingShopMenuItem("Prolaps Golf", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 158, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1_MPF = new PedClothingShopMenuItem("Baseball Cap 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 153, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1Reverse_MPF = new PedClothingShopMenuItem("Backwards Flat Cap", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 154, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        StrawHat1_MPF = new PedClothingShopMenuItem("Straw Hat", "", 25, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 20, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPMale_Hats()
    {
        //Formal
        BrimmedHat1_MPM = new PedClothingShopMenuItem("Brimmed Hat 1", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 25, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BrimmedHat2_MPM = new PedClothingShopMenuItem("Brimmed Hat 2", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 30, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BrimmedHat3_MPM = new PedClothingShopMenuItem("Brimmed Hat 3", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 64, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BowlerHat1_MPM = new PedClothingShopMenuItem("Bowler 1", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 26, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        StovetopHat1_MPM = new PedClothingShopMenuItem("Stovetop Hat 1", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 27, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        //Cowboy
        CowboyHat1_MPM = new PedClothingShopMenuItem("Cowboy Hat", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {//EUP OVERWRITE
        new PedClothingComponent(0, 13, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        //Bandana
        Beanie1_MPM = new PedClothingShopMenuItem("Tied Bandana", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 14, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        //Beanies
        Beanie1_MPM = new PedClothingShopMenuItem("Loose Beanie", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 2, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        Beanie2_MPM = new PedClothingShopMenuItem("Tight Beanie", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 5, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        Beanie3_MPM = new PedClothingShopMenuItem("Tight Beanie 2", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {//EUP OVERWRITE
        new PedClothingComponent(0, 28, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        //Brimmed
        StrawHat1_MPM = new PedClothingShopMenuItem("Small Brimmed Hat", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 21, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        //Baseball
        BaseballHatGuns_MPM = new PedClothingShopMenuItem("Arms Dealer Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 109, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatGunsReverse_MPM = new PedClothingShopMenuItem("Backwards Arms Dealer", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 110, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatFastFood_MPM = new PedClothingShopMenuItem("Fast Food Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 130, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatFastFoodReverse_MPM = new PedClothingShopMenuItem("Backwards Fast Food Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 131, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatEnus_MPM = new PedClothingShopMenuItem("Bugstars Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 139, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatEnusReverse_MPM = new PedClothingShopMenuItem("Backwards Bugstars Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 140, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatEnus_MPM = new PedClothingShopMenuItem("Gaming Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 151, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatEnusReverse_MPM = new PedClothingShopMenuItem("Backwards Gaming Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 152, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatBroker_MPM = new PedClothingShopMenuItem("Broker Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 162, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatBrokerReverse_MPM = new PedClothingShopMenuItem("Backwards Broker Hat", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 163, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatProLaps1_MPM = new PedClothingShopMenuItem("Prolaps Golf", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 159, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatProLaps1Reverse_MPM = new PedClothingShopMenuItem("Backwards Prolaps Golf", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 161, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1_MPM = new PedClothingShopMenuItem("Baseball Cap 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 154, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1Reverse_MPM = new PedClothingShopMenuItem("Backwards Baseball Cap 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 155, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPFemale_Glasses()
    {
        Aviators1_MPF = new PedClothingShopMenuItem("Aviators 1", "", 150, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(1, 11, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Glasses",
            SubCategory = "Sunglasses",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPMale_Glasses()
    {
        Aviators1_MPM = new PedClothingShopMenuItem("Aviators 1", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(1, 12, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Glasses",
            SubCategory = "Sunglasses",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPFemale_Shoes()
    {
        // Boots
        FUGGsBoots1_MPF = new PedClothingShopMenuItem("FUGGS", "Make sure your feet are FUGG-ly", 195, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 2, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        HeelsBoots1_MPF = new PedClothingShopMenuItem("Heel Boots", "", 225, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 7, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        HeelsBoots2_MPF = new PedClothingShopMenuItem("Low Heel Boots", "", 220, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 8, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        KneeHighBoots1_MPF = new PedClothingShopMenuItem("Knee High Boots", "", 180, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 9, new List<int>() { 0, 1, 2, 3, 11, 12 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        KneeHighBoots2_MPF = new PedClothingShopMenuItem("Knee High Boots", "", 280, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 21, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        FoldedBoots1_MPF = new PedClothingShopMenuItem("Folded Boots", "", 180, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 22, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlightBoots1_MPF = new PedClothingShopMenuItem("Flight Boots", "", 250, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 24, new List<int>() { 0 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        TacticalBoots1_MPF = new PedClothingShopMenuItem("Tactical Boots", "", 260, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 25, new List<int>() { 0 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        StuddedBoots1_MPF = new PedClothingShopMenuItem("Studded Boots", "", 240, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 30, new List<int>() { 0 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        WalkingBoots_MPF = new PedClothingShopMenuItem("Walking Boots", "", 100, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 36, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        CowboyBoots_MPF = new PedClothingShopMenuItem("Cowboy Boots", "Howdy Partner!", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 38, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Cowboy",
            PedFocusZone = ePedFocusZone.Feet
        };
        CowboyBoots2_MPF = new PedClothingShopMenuItem("Cowboy Boots", "Howdy Partner!", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 45, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Cowboy",
            PedFocusZone = ePedFocusZone.Feet
        };
        LaceUpBoots_MPF = new PedClothingShopMenuItem("Lace-Up Boots", "", 100, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 51, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        SlackBoots_MPF = new PedClothingShopMenuItem("Slack Boots", "", 180, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 54, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        CalfBoots_MPF = new PedClothingShopMenuItem("Buckle-Up Boots", "", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 56, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        TacticalBoots2_MPF = new PedClothingShopMenuItem("Tactical Boots", "", 150, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 65, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        MocToeBoots_MPF = new PedClothingShopMenuItem("Moc Toe Boots", "", 180, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 68, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RubberizedBoots_MPF = new PedClothingShopMenuItem("Rubberized Boots", "", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 73, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        TrailBoots_MPF = new PedClothingShopMenuItem("Trail Boots", "", 150, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 75, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlamingBoots_MPF = new PedClothingShopMenuItem("Flaming Skull Boots", "", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 83, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        HarnessBoots_MPF = new PedClothingShopMenuItem("Skull Harness Boots", "", 200, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 85, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        UniformBoots_MPF = new PedClothingShopMenuItem("Uniform Boots", "", 220, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 100, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };

        //HighHeels
        Heels1_MPF = new PedClothingShopMenuItem("Rounder Toe Heels", "", 185, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 0, new List<int>() { 0, 1 , 2, 3 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels2_MPF = new PedClothingShopMenuItem("Pointed Toe Heels", "", 195, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 6, new List<int>() { 0, 1 , 2 , 3 }) { AllowAllTextureVariations = false }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels3_MPF = new PedClothingShopMenuItem("Strapped High Heels", "", 200, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 14, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels4_MPF = new PedClothingShopMenuItem("Platform High Heels", "", 210, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 19, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels5_MPF = new PedClothingShopMenuItem("Patent High Heels", "", 180, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 20, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels6_MPF = new PedClothingShopMenuItem("Sparkly High Heels", "", 260, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 23, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels7_MPF = new PedClothingShopMenuItem("Rounded High Heels", "", 240, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 42, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels8_MPF = new PedClothingShopMenuItem("Sneaker High Heels", "", 200, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 44, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        Heels9_MPF = new PedClothingShopMenuItem("Cat suit High Heels", "", 260, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 77, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Heels",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };


        //Shoes / Running Shoes / Trainers
        SkateShoes1_MPF = new PedClothingShopMenuItem("Skaters", "", 155, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 1, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        Chucks1_MPF = new PedClothingShopMenuItem("Blaines", "", 125, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RunningShoes1_MPF = new PedClothingShopMenuItem("Aeris Running", "", 175, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 4, new List<int>() { 0, 1 , 2, 3 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlipFlops1_MPF = new PedClothingShopMenuItem("Flip Flops", "", 55, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 5, new List<int>() { 0, 1, 10, 13 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RunningShoes2_MPF = new PedClothingShopMenuItem("Running Shoes", "", 175, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 10, new List<int>() { 0, 1 , 2, 3 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        HighTopsShoes1_MPF = new PedClothingShopMenuItem("Aeris Hi-Top Running", "", 195, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 11, new List<int>() { 0, 1 , 2, 3 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RoundToeShoes1_MPF = new PedClothingShopMenuItem("Round Toe Shoes", "", 220, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 13, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        SandalShoes1_MPF = new PedClothingShopMenuItem("Sandals", "", 220, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(6, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlipFlops2_MPF = new PedClothingShopMenuItem("Flip Flops", "", 60, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 16, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlipFlops3_MPF = new PedClothingShopMenuItem("Flip Flops", "", 65, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 110, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        GoldenHiTops_MPF = new PedClothingShopMenuItem("Golden Hi-Tops", "", 300, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 31, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        Runners_MPF = new PedClothingShopMenuItem("ProLaps Runners", "", 125, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 32, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        SneakerWedges_MPF = new PedClothingShopMenuItem("Sneaker Wedges", "", 225, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 43, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        PlainHiTops_MPF = new PedClothingShopMenuItem("Plain Hi-Tops", "", 80, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 60, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RetroRunners_MPF = new PedClothingShopMenuItem("Retro Runners", "", 160, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 79, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        LightUps_MPF = new PedClothingShopMenuItem("LightUp Sneakers", "", 120, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 81, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RetroSneakers_MPF = new PedClothingShopMenuItem("Retro Sneakers", "", 120, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 96, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers_MPF = new PedClothingShopMenuItem("Knitted Trainers", "", 250, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 103, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers2_MPF = new PedClothingShopMenuItem("Plain Cross Knitted Trainers", "", 150, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 132, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers3_MPF = new PedClothingShopMenuItem("Styled Cross Knitted Trainers", "", 350, new List<string>() { "mp_f_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 156, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
    }
    private void MPMale_Shoes()
    {
        // Boots
        CopperWorkBoots_MPM = new PedClothingShopMenuItem("Hinterlands", "The staple of the suburban dad look.", 170,
            new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 12, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        CasualBoots_MPM = new PedClothingShopMenuItem("Loafer Boots", "", 130, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 14, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        ChelseaBoots_MPM = new PedClothingShopMenuItem("Chelsea Boots", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 15, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlightBoots_MPM = new PedClothingShopMenuItem("Black Flight Boots", "", 140, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 24, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        TacticalBoots_MPM = new PedClothingShopMenuItem("Black Tactical Boots", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 25, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        WalkingBoots_MPM = new PedClothingShopMenuItem("Walking Boots", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 35, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        CowboyBoots_MPM = new PedClothingShopMenuItem("Cowboy Boots", "Howdy Partner!", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 37, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Cowboy",
            PedFocusZone = ePedFocusZone.Feet
        };
        CowboyBoots2_MPM = new PedClothingShopMenuItem("Cowboy Boots", "Howdy Partner!", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 44, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Cowboy",
            PedFocusZone = ePedFocusZone.Feet
        };
        AnkleBoots_MPM = new PedClothingShopMenuItem("Ankle Boots", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 43, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        LaceUpBoots_MPM = new PedClothingShopMenuItem("Lace-Up Boots", "", 100, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 50, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        SlackBoots_MPM = new PedClothingShopMenuItem("Slack Boots", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 53, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        TacticalBoots2_MPM = new PedClothingShopMenuItem("Tactical Boots", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 62, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        MocToeBoots_MPM = new PedClothingShopMenuItem("Moc Toe Boots", "", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 65, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RubberizedBoots_MPM = new PedClothingShopMenuItem("Rubberized Boots", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 70, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        TrailBoots_MPM = new PedClothingShopMenuItem("Trail Boots", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 72, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlamingBoots_MPM = new PedClothingShopMenuItem("Flaming Skull Boots", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 79, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        HarnessBoots_MPM = new PedClothingShopMenuItem("Skull Harness Boots", "", 200, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 81, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };
        UniformBoots_MPM = new PedClothingShopMenuItem("Uniform Boots", "", 220, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 96, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Boots",
            SubCategory = "Utility",
            PedFocusZone = ePedFocusZone.Feet
        };
        RoadBoots_MPM = new PedClothingShopMenuItem("Road Boots", "", 220, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 103, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Boots",
            SubCategory = "Motorcycle",
            PedFocusZone = ePedFocusZone.Feet
        };

        //Casual & Running

        SkateShoes_MPM = new PedClothingShopMenuItem("Skate Shoes", "", 95, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 1, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };

        RunningShoes_MPM = new PedClothingShopMenuItem("Running Shoes", "", 85, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 2, new List<int>() { 6, 13 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };

        Chucks1_MPM = new PedClothingShopMenuItem("Blaines", "", 125, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 4, new List<int>() { 0 ,1 ,2, 4  }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };

        FlipFlops1_MPM = new PedClothingShopMenuItem("Flip Flops", "", 55, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 5, new List<int>() { 0, 1, 2, 3 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        SportsShoes_MPM = new PedClothingShopMenuItem("Sports Shoes", "", 100, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 7, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        AthleticShoes_MPM = new PedClothingShopMenuItem("Athletic Shoes", "", 120, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 8, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        KickShoes_MPM = new PedClothingShopMenuItem("Kicks", "", 90, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 9, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        Chucks2_MPM = new PedClothingShopMenuItem("Stanks", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 26, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        StuddedSneakers_MPM = new PedClothingShopMenuItem("Studded Sneakers", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 28, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        GoldenHiTops_MPM = new PedClothingShopMenuItem("Golden Hi-Tops", "", 300, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 29, new List<int>() { 0 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        Runners_MPM = new PedClothingShopMenuItem("ProLaps Runners", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 31, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        HighTopSneakers_MPM = new PedClothingShopMenuItem("Hi-Top Sneakers", "", 125, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 32, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        CanvasSlipons_MPM = new PedClothingShopMenuItem("Canvas Slip-On", "", 80, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 42, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        HighTopSneakers2_MPM = new PedClothingShopMenuItem("Hi-Top Sneakers", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 46, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        PlainHiTops_MPM = new PedClothingShopMenuItem("Plain Hi-Tops", "", 80, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 57, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RetroRunners_MPM = new PedClothingShopMenuItem("Retro Runners", "", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 75, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        LightUps_MPM = new PedClothingShopMenuItem("LightUp Sneakers", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 77, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        RetroSneakers_MPM = new PedClothingShopMenuItem("Retro Sneakers", "", 120, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 93, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers_MPM = new PedClothingShopMenuItem("Knitted Trainers", "", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 99, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers2_MPM = new PedClothingShopMenuItem("Plain Cross Knitted Trainers", "", 150, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 128, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };
        KnitSneakers3_MPM = new PedClothingShopMenuItem("Styled Cross Knitted Trainers", "", 350, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 147, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Running",
            PedFocusZone = ePedFocusZone.Feet
        };


        // --- Male Dress/Formal Shoes ---

        LuxuryBoatShoes_MPM = new PedClothingShopMenuItem("Luxury Loafers", "", 150, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        BlackOxfords_MPM = new PedClothingShopMenuItem("Black Oxfords", "Polished calfskin leather.", 250, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 10, new List<int>() { 0, 7, 12, 14 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        SlipOnLoafers_MPM = new PedClothingShopMenuItem("Slip On loafers", "Crocodile skin embossed leather.", 150, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 11, new List<int>() { 9, 12, 14, 15 }){ AllowAllTextureVariations = false }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        ChocolateOxfords_MPM = new PedClothingShopMenuItem("Chocolate Oxfords", "Polished calfskin leather.", 250, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 20, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        LeatherLoafers_MPM = new PedClothingShopMenuItem("Leather Loafers", "Luxury slip-on dress shoes.", 200, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 21, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        WingTips_MPM = new PedClothingShopMenuItem("Wing Tips", "", 220, new List<string>() { "mp_m_freemode_01" }, 
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 23, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        DrivingLoafers_MPM = new PedClothingShopMenuItem("Driving Loafers", "Drive in Comfort.", 180, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 30, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        LeatherLoafers2_MPM = new PedClothingShopMenuItem("Leather Loafers", "Luxury Loafers.", 160, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 36, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        TipOxfords_MPM = new PedClothingShopMenuItem("Oxford Tips", "", 250, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 40, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        SmartOxfords_MPM = new PedClothingShopMenuItem("Smart Oxford", "", 350, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 105, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        BuckledLoafers_MPM = new PedClothingShopMenuItem("Buckled Loafers", "", 230, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 107, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        SmartOxfords2_MPM = new PedClothingShopMenuItem("Smart Oxford", "", 350, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
            new PedClothingComponent(6, 111, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };

    }
    private void MPFemale_Watches()
    {
        GoldWatch1_MPF = new PedClothingShopMenuItem("Small Watch", "", 660, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(6, 2, new List<int>() { 0 }) { IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        GoldWatch2_MPF = new PedClothingShopMenuItem("Large Watch", "", 860, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 5, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
    }
    private void MPMale_Watches()
    {
        Watch1_MPM = new PedClothingShopMenuItem("Watch 1", "", 85, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 1, new List<int>() { 0 }) { IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        Watch2_MPM = new PedClothingShopMenuItem("Watch 2", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 3, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        Watch3_MPM = new PedClothingShopMenuItem("Watch 3", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
    }
    private void MPFemale_Bracelets()
    {

    }
    private void MPMale_Bracelets()
    {

    }

    //WHEW
    private void MPFemale_Helmets()
    {
        DirtHelmet1_MPF = new PedClothingShopMenuItem("Dirt Bike Helmet", "", 245, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 16, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        DirtHelmet2_MPF = new PedClothingShopMenuItem("Blacked Out Dirt Bike Helmet", "", 350, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 48, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        HalfHelmet1_MPF = new PedClothingShopMenuItem("Half Helmet", "Look like a dork, I dare you", 150, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 17, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Half",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet1_MPF = new PedClothingShopMenuItem("Full Face Basic", "", 345, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 18, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet2_MPF = new PedClothingShopMenuItem("Blacked Out Full Face", "", 385, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 49, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet3_MPF = new PedClothingShopMenuItem("Mirror Finish Full Face", "", 425, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 50, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet4_MPF = new PedClothingShopMenuItem("Backed Out Full Face Alt", "", 410, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 51, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet5_MPF = new PedClothingShopMenuItem("Backed Out Full Face Alt 2", "", 410, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 52, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        OpenFullHelmet1_MPF = new PedClothingShopMenuItem("Lifted Full Face", "", 345, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 66, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };

        PilotHelmet1_MPF = new PedClothingShopMenuItem("Jet Pilot Helmet", "", 4500, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 37, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Military",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
    }
    private void MPMale_Helmets()
    {
        DirtHelmet1_MPM = new PedClothingShopMenuItem("Dirt Bike Helmet", "", 245, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 16, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        DirtHelmet2_MPM = new PedClothingShopMenuItem("Blacked Out Dirt Bike Helmet", "", 350, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 48, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        HalfHelmet1_MPM = new PedClothingShopMenuItem("Half Helmet", "Look like a dork, I dare you", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 17, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Half",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet1_MPM = new PedClothingShopMenuItem("Full Face Basic", "", 345, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 18, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet2_MPM = new PedClothingShopMenuItem("Blacked Out Full Face", "", 385, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 50, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet3_MPM = new PedClothingShopMenuItem("Mirror Finish Full Face", "", 425, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 51, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet4_MPM = new PedClothingShopMenuItem("Backed Out Full Face Alt", "", 410, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 52, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet5_MPM = new PedClothingShopMenuItem("Backed Out Full Face Alt 2", "", 410, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 53, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        OpenFullHelmet1_MPM = new PedClothingShopMenuItem("Lifted Full Face", "", 345, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 67, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };

        PilotHelmet1_MPM = new PedClothingShopMenuItem("Jet Pilot Helmet", "", 4500, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 38, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Military",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
    }
    private void MPCombined_Masks()
    {
        MaskHockey_MPC = new PedClothingShopMenuItem("Hockey Mask", "", 175, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 4, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Horror",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskMonkey_MPC = new PedClothingShopMenuItem("Monkey Mask", "No monkeying around!", 190, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 5, new List<int>() { 0 }){ AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Other",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskLuchador_MPC = new PedClothingShopMenuItem("Luchador Mask", "", 190, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Other",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskGargoyle1_MPC = new PedClothingShopMenuItem("Gargoyle Mask", "", 250, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 7, new List<int>() { 0 }){AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Horror",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBallistic_MPC = new PedClothingShopMenuItem("Ballistic Mask", "", 950, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 125, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskCombatMask_MPC = new PedClothingShopMenuItem("Combat Mask", "", 450, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 28, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskPainted_MPC = new PedClothingShopMenuItem("Painted Mask", "", 190, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 188, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskNightVision_MPC = new PedClothingShopMenuItem("Night Vision", "", 2500, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 132, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskSnakeSkull_MPC = new PedClothingShopMenuItem("Snake Skull", "", 190, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 106, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskSpecOps_MPC = new PedClothingShopMenuItem("Spec Ops", "", 150, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 126, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Tactical",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskSkullScruffyBalaclava_MPC = new PedClothingShopMenuItem("Scruffy Balaclava", "", 70, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 119, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskScruffyBalaclava_MPC = new PedClothingShopMenuItem("Black Scruffy Balaclava", "", 75, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 37, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBlackKnitBalaclava_MPC = new PedClothingShopMenuItem("Knit Balaclava", "", 75, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 57, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBanditKnitBalaclava_MPC = new PedClothingShopMenuItem("Bandit Knit Balaclava", "", 65, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 58, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBrightStripeKnitBalaclava_MPC = new PedClothingShopMenuItem("Bright Stripe Knit Balaclava", "", 60, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 117, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskLooseBalaclava_MPC = new PedClothingShopMenuItem("Loose Balaclava", "", 45, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 56, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Balaclava",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFilter_MPC = new PedClothingShopMenuItem("Filter Mask", "", 400, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 90, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Biker",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskVent_MPC = new PedClothingShopMenuItem("Vent Mask", "", 345, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 107, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Biker",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBear_MPC = new PedClothingShopMenuItem("Bear Mask", "", 120, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 21, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBison_MPC = new PedClothingShopMenuItem("Bison Mask", "", 200, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 22, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskBull_MPC = new PedClothingShopMenuItem("Bull Mask", "", 130, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 23, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskCat_MPC = new PedClothingShopMenuItem("Cat Mask", "", 245, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 17, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskCrazedApe_MPC = new PedClothingShopMenuItem("Crazed Ape Mask", "", 260, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 96, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskDino_MPC = new PedClothingShopMenuItem("Dino Mask", "", 160, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 17, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskEagle_MPC = new PedClothingShopMenuItem("Eagle Mask", "", 130, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 24, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFox_MPC = new PedClothingShopMenuItem("Fox Mask", "", 120, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 18, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskHorse_MPC = new PedClothingShopMenuItem("Horse Mask", "", 125, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 97, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskHyena_MPC = new PedClothingShopMenuItem("Hyena Mask", "", 75, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 184, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskMouse_MPC = new PedClothingShopMenuItem("Mouse Mask", "", 85, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 182, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskOwl_MPC = new PedClothingShopMenuItem("Owl Mask", "", 90, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 19, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskPig_MPC = new PedClothingShopMenuItem("Pig Mask", "", 95, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 1, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskPug_MPC = new PedClothingShopMenuItem("Pug Mask", "", 75, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 100, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskRaccoon_MPC = new PedClothingShopMenuItem("Raccoon Mask", "", 90, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 20, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskTurtle_MPC = new PedClothingShopMenuItem("Turtle Mask", "", 80, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 181, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskWolf_MPC = new PedClothingShopMenuItem("Wolf Mask", "", 120, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 26, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskUnicorn_MPC = new PedClothingShopMenuItem("Unicorn Mask", "", 110, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 98, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskVulture_MPC = new PedClothingShopMenuItem("Vulture Mask", "", 110, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 24, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Animal",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskClown_MPC = new PedClothingShopMenuItem("Clown Mask", "", 120, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 95, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Clowns",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskDeath_MPM = new PedClothingShopMenuItem("Death Mask", "", 175, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 202, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskDeath_MPF = new PedClothingShopMenuItem("Death Mask", "", 175, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 203, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFalseFace_MPC = new PedClothingShopMenuItem("False Face Mask", "", 175, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 128, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFamine_MPM = new PedClothingShopMenuItem("Famine Mask", "", 160, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 198, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFamine_MPF = new PedClothingShopMenuItem("Famine Mask", "", 160, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 199, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskImpotentRage_MPC = new PedClothingShopMenuItem("Impotent Rage Mask", "", 345, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 43, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskMoorehead_MPC = new PedClothingShopMenuItem("Moorehead Mask", "", 200, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 45, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskPogo_MPC = new PedClothingShopMenuItem("Pogo Mask", "", 225, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 3, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskPrincessRobotBubblegum_MPC = new PedClothingShopMenuItem("Princess Robot Bubblegum Mask", "", 355, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 44, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Characters",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskHockeyAlt_MPC = new PedClothingShopMenuItem("Hockey Mask", "", 165, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 14, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Intimidation",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskMandible_MPC = new PedClothingShopMenuItem("Mandible Mask", "", 185, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 112, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Intimidation",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskRobo_MPC = new PedClothingShopMenuItem("Robo Mask", "", 225, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 110, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Intimidation",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskWarrior_MPC = new PedClothingShopMenuItem("Warrior Mask", "", 100, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 16, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Intimidation",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskCrimeSceneTape_MPC = new PedClothingShopMenuItem("Crime Scene Tape Mask", "", 125, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 47, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Crime",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskDuctTape_MPC = new PedClothingShopMenuItem("Duct Tape Mask", "", 145, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 48, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Crime",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskFaceBandana_MPC = new PedClothingShopMenuItem("Face Bandana", "", 160, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 51, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Crime",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskTShirtMask_MPC = new PedClothingShopMenuItem("T-Shirt Mask", "", 175, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 54, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Crime",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };

    }

}