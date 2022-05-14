using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public class Gangs : IGangs
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Gangs.xml";
    private bool UseVanillaConfig = true;
    private List<Gang> GangsList;
    private Gang DefaultGang;
    public Gangs()
    {

    }
    public List<Gang> AllGangs => GangsList;
    public void ReadConfig()
    {
#if DEBUG
        UseVanillaConfig = true;
#else
            UseVanillaConfig = true;
#endif

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Gangs*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Deserializing 1 {ConfigFile.FullName}");
            GangsList = Serialization.DeserializeParams<Gang>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Deserializing 2 {ConfigFileName}");
            GangsList = Serialization.DeserializeParams<Gang>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
        }
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach (Gang gang in GangsList)
        {
            gang.MeleeWeapons = issuableWeapons.GetWeaponData(gang.MeleeWeaponsID);
            gang.LongGuns = issuableWeapons.GetWeaponData(gang.LongGunsID);
            gang.SideArms = issuableWeapons.GetWeaponData(gang.SideArmsID);
            gang.Personnel = dispatchablePeople.GetPersonData(gang.PersonnelID);
            gang.Vehicles = dispatchableVehicles.GetVehicleData(gang.VehiclesID);
            gang.PossibleHeads = heads.GetHeadData(gang.HeadDataGroupID);
        }
    }
    public List<Gang> GetAllGangs()
    {
        return GangsList;
    }
    public Gang GetGang(string GangInitials)
    {
        return GangsList.Where(x => x.ID.ToLower() == GangInitials.ToLower()).FirstOrDefault();
    }
    public Gang GetGangByContact(string contactName)
    {
        return GangsList.Where(x => x.ContactName.ToLower() == contactName.ToLower()).FirstOrDefault();
    }
    public List<Gang> GetGangs(Ped ped)
    {
        return GangsList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Gang> GetGangs(Vehicle vehicle)
    {
        return GangsList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Gang> GetSpawnableGangs(int WantedLevel)
    {
        return GangsList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel)).ToList();
    }
    private void DefaultConfig()
    {
        DefaultGang = new Gang("~s~", "UNK", "Unknown Gang","Unk", "White", null, null, "", null, null,null,"Gang Member") { MaxWantedLevelSpawn = 0 };
        GangsList = new List<Gang>
        {
            new Gang("~w~", "AMBIENT_GANG_LOST", "The Lost MC","LOST MC", "White", "LostMCPEds", "LostMCVehicles", "LOST ","MeleeWeapons","LostSidearms","LostLongGuns", "LOST MC President","CHAR_MP_BIKER_BOSS","LOST Member") { 
                                            DenName = "Clubhouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 5000,EnemyGangs = new List<string>() { "AMBIENT_GANG_MADRAZO", "AMBIENT_GANG_GAMBETTI", "AMBIENT_GANG_ANCELOTTI" }, DealerMenuGroup = "MethamphetamineDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 600, TheftPaymentMin = 1000, TheftPaymentMax = 3000, HitPaymentMin = 10000, HitPaymentMax = 22000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 3000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 40f, PercentageWithSidearms = 30f, PercentageWithLongGuns = 15f } ,//Meth
            new Gang("~o~", "AMBIENT_GANG_MEXICAN", "Vagos","Vagos", "Orange", "VagosPeds", "VagosVehicles", "","MeleeWeapons","VagosSidearms","VagosLongGuns","Vagos O.G.", "CHAR_MP_MEX_BOSS","Vagos Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 500, AmbientMemberMoneyMax = 2000,EnemyGangs = new List<string>() { "AMBIENT_GANG_SALVA" },DealerMenuGroup = "MarijuanaDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1500, DeliveryPaymentMax = 4500
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 5f } ,//marijuana
            new Gang("~g~", "AMBIENT_GANG_FAMILY", "The Families","Families", "Green", "FamiliesPeds", "FamiliesVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Families O.G.","CHAR_MP_FAM_BOSS","Families Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 5000,EnemyGangs = new List<string>() { "AMBIENT_GANG_BALLAS" },DealerMenuGroup = "MarijuanaDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 700, TheftPaymentMin = 1000, TheftPaymentMax = 3000, HitPaymentMin = 10000, HitPaymentMax = 24000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 5f} ,//marijuana
            new Gang("~p~", "AMBIENT_GANG_BALLAS", "Ballas","Ballas", "Purple", "BallasPeds", "BallasVehicles", "","MeleeWeapons","BallasSidearms","BallasLongGuns","Ballas O.G.","CHAR_MP_JULIO","Ballas Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 2000, AmbientMemberMoneyMax = 7000,EnemyGangs = new List<string>() { "AMBIENT_GANG_FAMILY" }, DealerMenuGroup = "CrackDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1200, DeliveryPaymentMax = 4500
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 5f} ,//crack
            new Gang("~b~", "AMBIENT_GANG_MARABUNTE", "Marabunta Grande","Marabunta", "Blue", "MarabuntaPeds", "MarabuntaVehicles", "","MeleeWeapons","MarabuntaSidearms","MarabuntaLongGuns","Marabunta O.G.","CHAR_MP_MEX_LT","Marabunta Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 2000, AmbientMemberMoneyMax = 7000,EnemyGangs = new List<string>() { "AMBIENT_GANG_MADRAZO" },DealerMenuGroup = "MarijuanaDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 5f} ,//marijuana
            new Gang("~w~", "AMBIENT_GANG_CULT", "Altruist Cult","Altruist", "White", "AltruistPeds", "GenericGangVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Altruist Leader","CHAR_PA_MALE","Altruist Member") { 
                                            DenName = "Gathering Location",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_HILLBILLY" }, DealerMenuGroup = "ToiletCleanerDealerMenu",
                                            PickupPaymentMin = 100, PickupPaymentMax = 500, TheftPaymentMin = 500, TheftPaymentMax = 2000, HitPaymentMin = 5000, HitPaymentMax = 10000,DeliveryPaymentMin = 800, DeliveryPaymentMax = 3000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 4500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 30f, PercentageWithLongGuns = 5f} ,
            new Gang("~y~", "AMBIENT_GANG_SALVA", "Varrios Los Aztecas","Varrios", "Yellow", "VarriosPeds", "VarriosVehicles", "","MeleeWeapons","VarriosSidearms","VarriosLongGuns","Varrios O.G.","CHAR_ORTEGA","Varrios Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 5000,EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG","AMBIENT_GANG_MESSINA" }, DealerMenuGroup = "CrackDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 500, TheftPaymentMin = 1000, TheftPaymentMax = 3000, HitPaymentMin = 10000, HitPaymentMax = 27000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 25f, PercentageWithSidearms = 25f, PercentageWithLongGuns = 10f} ,//crack
            new Gang("~r~", "AMBIENT_GANG_WEICHENG", "Triads","Triads", "Red", "TriadsPeds", "TriadVehicles", "","MeleeWeapons","TriadsSidearms","TriadsLongGuns","Triad Leader","CHAR_CHENGSR","Triad Member") { 
                                            DenName = "Meeting Spot",AmbientMemberMoneyMin = 2000, AmbientMemberMoneyMax = 7000,EnemyGangs = new List<string>() { "AMBIENT_GANG_MESSINA","AMBIENT_GANG_SALVA" }, DealerMenuGroup = "HeroinDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 2500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 30f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 2f} ,//heroin
            new Gang("~b~", "AMBIENT_GANG_HILLBILLY", "Rednecks","Rednecks", "Black", "RedneckPeds", "RedneckVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Redneck Leader","CHAR_ONEIL","Redneck") { 
                                            DenName = "Clubhouse",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_CULT" }, DealerMenuGroup = "ToiletCleanerDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 3000, HitPaymentMin = 10000, HitPaymentMax = 15000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1000, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 50f, PercentageWithLongGuns = 20f} ,//TOILET CLEANER
            new Gang("~q~", "AMBIENT_GANG_KKANGPAE", "Kkangpae","Kkangpae", "Pink", "KoreanPeds", "KoreanVehicles", "","MeleeWeapons","KkangpaeSidearms","KkangpaeLongGuns","Kkangpae Leader","CHAR_CHENG","Kkangpae Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 2000, AmbientMemberMoneyMax = 7000,EnemyGangs = new List<string>() { "AMBIENT_GANG_YARDIES" }, DealerMenuGroup = "HeroinDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 2000, TheftPaymentMin = 1000, TheftPaymentMax = 6000, HitPaymentMin = 12000, HitPaymentMax = 32000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 2500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
                                            ,PercentageWithMelee = 30f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 2f} ,//heroin
            new Gang("~g~", "AMBIENT_GANG_GAMBETTI", "Gambetti Crime Family","Gambetti", "Green", "MafiaPeds", "MafiaVehicles", "","MeleeWeapons","MafiaSidearms","MafiaLongGuns","Gambetti Boss","CHAR_TOM","Gambetti Associate") { 
                                            DenName = "Safehouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 10000,EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST" }, DealerMenuGroup = "CokeDealerMenu",
                                            PickupPaymentMin = 1000, PickupPaymentMax = 3000, TheftPaymentMin = 2000, TheftPaymentMax = 7000, HitPaymentMin = 20000, HitPaymentMax = 57000,DeliveryPaymentMin = 3000, DeliveryPaymentMax = 8000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 3500, StartingRep = 0, MaximumRep = 8000, MinimumRep = -8000
                                            ,PercentageWithMelee = 5f, PercentageWithSidearms = 35f, PercentageWithLongGuns = 2f} ,//cocaine
            new Gang("~g~", "AMBIENT_GANG_PAVANO", "Pavano Crime Family","Pavano", "Green", "MafiaPeds", "MafiaVehicles", "","MeleeWeapons","MafiaSidearms","MafiaLongGuns","Pavano Boss","CHAR_DOM","Pavano Assocaite") { 
                                            DenName = "Safehouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 10000,EnemyGangs = new List<string>() { "AMBIENT_GANG_ARMENIAN" }, DealerMenuGroup = "CokeDealerMenu",
                                            PickupPaymentMin = 1000, PickupPaymentMax = 3500, TheftPaymentMin = 2000, TheftPaymentMax = 6000, HitPaymentMin = 20000, HitPaymentMax = 55000,DeliveryPaymentMin = 3000, DeliveryPaymentMax = 8000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 3500, StartingRep = 0, MaximumRep = 8000, MinimumRep = -8000
                                            ,PercentageWithMelee = 5f, PercentageWithSidearms = 35f, PercentageWithLongGuns = 2f} ,//cocaine
            new Gang("~g~", "AMBIENT_GANG_LUPISELLA", "Lupisella Crime Family","Lupisella", "Green", "MafiaPeds", "MafiaVehicles", "","MeleeWeapons","MafiaSidearms","MafiaLongGuns","Lupisella Boss","CHAR_AGENT14","Lupisella Assocaite") { 
                                            DenName = "Safehouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 10000,EnemyGangs = new List<string>() { "AMBIENT_GANG_KKANGPAE" }, DealerMenuGroup = "CokeDealerMenu",
                                            PickupPaymentMin = 1000, PickupPaymentMax = 3200, TheftPaymentMin = 2000, TheftPaymentMax = 8000, HitPaymentMin = 20000, HitPaymentMax = 52000,DeliveryPaymentMin = 3000, DeliveryPaymentMax = 8000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 3500, StartingRep = 0, MaximumRep = 8000, MinimumRep = -8000
                                            ,PercentageWithMelee = 5f, PercentageWithSidearms = 35f, PercentageWithLongGuns = 2f} ,//cocaine
            new Gang("~g~", "AMBIENT_GANG_MESSINA", "Messina Crime Family","Messina", "Green", "MafiaPeds", "MafiaVehicles", "","MeleeWeapons","MafiaSidearms","MafiaLongGuns","Messina Boss","CHAR_BARRY","Messina Assocaite") { 
                                            DenName = "Safehouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 10000,EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG","AMBIENT_GANG_SALVA" }, DealerMenuGroup = "CokeDealerMenu",
                                            PickupPaymentMin = 1000, PickupPaymentMax = 3400, TheftPaymentMin = 2000, TheftPaymentMax = 9000, HitPaymentMin = 20000, HitPaymentMax = 45000,DeliveryPaymentMin = 3000, DeliveryPaymentMax = 8000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 3500, StartingRep = 0, MaximumRep = 8000, MinimumRep = -8000
                                            ,PercentageWithMelee = 5f, PercentageWithSidearms = 35f, PercentageWithLongGuns = 2f} ,//cocaine
            new Gang("~g~", "AMBIENT_GANG_ANCELOTTI", "Ancelotti Crime Family","Ancelotti", "Green", "MafiaPeds", "MafiaVehicles", "","MeleeWeapons","MafiaSidearms","MafiaLongGuns","Ancelotti Boss","CHAR_DREYFUSS","Ancelotti Associate") { 
                                            DenName = "Safehouse",AmbientMemberMoneyMin = 1000, AmbientMemberMoneyMax = 10000,EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST" }, DealerMenuGroup = "CokeDealerMenu",
                                            PickupPaymentMin = 1000, PickupPaymentMax = 3800, TheftPaymentMin = 2000, TheftPaymentMax = 6000, HitPaymentMin = 20000, HitPaymentMax = 44000,DeliveryPaymentMin = 3000, DeliveryPaymentMax = 8000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 3500, StartingRep = 0, MaximumRep = 8000, MinimumRep = -8000
                                            ,PercentageWithMelee = 5f, PercentageWithSidearms = 35f, PercentageWithLongGuns = 2f} ,//cocaine
            new Gang("~r~", "AMBIENT_GANG_MADRAZO", "Madrazo Cartel","Cartel", "Red", "CartelPeds", "CartelVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Madrazo","CHAR_MANUEL","Cartel Member") { 
                                            DenName = "Mansion",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_MARABUNTE" }, DealerMenuGroup = "MethamphetamineDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 3000, MinimumRep = -3000
                                            ,PercentageWithMelee = 25f, PercentageWithSidearms = 20f, PercentageWithLongGuns = 15f} ,//Meth
            new Gang("~b~", "AMBIENT_GANG_ARMENIAN", "Armenian Mob","Armenian", "Black", "ArmenianPeds", "ArmeniaVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Armenian Leader","CHAR_MP_PROF_BOSS","Armenian Member") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_PAVANO" }, DealerMenuGroup = "HeroinDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 3000, MinimumRep = -3000
                                            ,PercentageWithMelee = 20f, PercentageWithSidearms = 25f, PercentageWithLongGuns = 15f} ,//heroin
            new Gang("~g~", "AMBIENT_GANG_YARDIES", "Yardies","Yardies", "Green", "YardiesPeds", "YardieVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Yardie O.G.","CHAR_MP_GERALD","Yardie Member") { 
                                            DenName = "Chill Spot",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_KKANGPAE", "AMBIENT_GANG_DIABLOS" },DealerMenuGroup = "MarijuanaDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 15000, TheftPaymentMin = 1000, TheftPaymentMax = 7000, HitPaymentMin = 10000, HitPaymentMax = 20000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 3000, MinimumRep = -3000
                                            ,PercentageWithMelee = 15f, PercentageWithSidearms = 10f, PercentageWithLongGuns = 2f} ,//marijuana
            new Gang("~r~", "AMBIENT_GANG_DIABLOS", "Diablos","Diablos", "Red", "VagosPeds", "DiablosVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Diablo Leader","CHAR_TW","Diablo Soldier") { 
                                            DenName = "Hangout",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_YARDIES" }, DealerMenuGroup = "SPANKDealerMenu",
                                            PickupPaymentMin = 200, PickupPaymentMax = 1000, TheftPaymentMin = 1000, TheftPaymentMax = 5000, HitPaymentMin = 10000, HitPaymentMax = 30000,DeliveryPaymentMin = 1000, DeliveryPaymentMax = 4000
                                            ,NeutralRepLevel = 0, FriendlyRepLevel = 1500, StartingRep = 0, MaximumRep = 3000, MinimumRep = -3000
                                            ,PercentageWithMelee = 15f, PercentageWithSidearms = 15f, PercentageWithLongGuns = 2f} ,//SPANK

            //DefaultGang
        };
        Serialization.SerializeParams(GangsList, ConfigFileName);
    }
}