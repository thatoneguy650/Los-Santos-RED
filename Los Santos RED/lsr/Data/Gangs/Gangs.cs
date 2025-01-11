using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
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
    private Gang LOST;
    private Gang Vagos;
    private Gang Families;
    private Gang Ballas;
    private Gang Marabunte;
    private Gang Varrios;
    private Gang Triads;
    private Gang Redneck;
    private Gang Korean;
    private Gang Gambetti;
    private Gang Pavano;
    private Gang Lupisella;
    private Gang Messina;
    private Gang Ancelotti;
    private Gang Cartel;
    private Gang Armenian;
    private Gang Yardies;
    private Gang Diablos;
    private LoanParameters defaultLoanParameters;

    public Gangs()
    {

    }
    public List<Gang> AllGangs => GangsList;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Gangs*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Gangs config: {ConfigFile.FullName}",0);
            GangsList = Serialization.DeserializeParams<Gang>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Gangs config  {ConfigFileName}",0);
            GangsList = Serialization.DeserializeParams<Gang>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Gangs config found, creating default", 0);
            SetupDefaults();
            DefaultConfig_Simple();
            DefaultConfig();
            DefaultConfig_LibertyCity();
        }
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons, IContacts contacts)
    {
        foreach (Gang gang in GangsList)
        {
            gang.MeleeWeapons = issuableWeapons.GetWeaponData(gang.MeleeWeaponsID);
            gang.LongGuns = issuableWeapons.GetWeaponData(gang.LongGunsID);
            gang.SideArms = issuableWeapons.GetWeaponData(gang.SideArmsID);
            gang.Personnel = dispatchablePeople.GetPersonData(gang.PersonnelID);
            gang.Vehicles = dispatchableVehicles.GetVehicleData(gang.VehiclesID);
            gang.PossibleHeads = heads.GetHeadData(gang.HeadDataGroupID);
            gang.Contact = contacts.GetGangContactData(gang.ContactName);
        }
    }
    public List<Gang> GetAllGangs()
    {
        return GangsList;
    }
    public Gang GetGang(string gangID)
    {
        if(string.IsNullOrEmpty(gangID))
        {
            return null;
        }
        return GangsList.Where(x => x.ID.ToLower() == gangID.ToLower()).FirstOrDefault();
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
    private void SetupLoanItems()
    {
        defaultLoanParameters = new LoanParameters();
        defaultLoanParameters.AddParameter(new LoanParameter(GangRespect.Neutral, 0.1f, 4, 500, 5000));
        defaultLoanParameters.AddParameter(new LoanParameter(GangRespect.Friendly, 0.04f, 4, 500, 15000));
        defaultLoanParameters.AddParameter(new LoanParameter(GangRespect.Member, 0.02f, 4, 500, 25000));
    }
    private void SetupDefaults()
    {
        SetupLoanItems();

        DefaultGang = new Gang("~s~", "UNK", "Unknown Gang", "Unk", "White", null, null, "", null, null, null, "Gang Member") { MaxWantedLevelSpawn = 0 };
        LOST = new Gang("~w~", "AMBIENT_GANG_LOST", "The Lost MC", "LOST MC", "White", "LostMCPeds", "LostMCVehicles", "LOST ", "MeleeWeapons", "LostSidearms", "LostLongGuns", "LOST MC", "CHAR_BLANK_ENTRY", "LOST Member")
        {
            DenName = "Clubhouse",
            HeadDataGroupID = "LostMCHeads",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 250,
            DealerMemberMoneyMin = 400,
            DealerMemberMoneyMax = 1500,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_MADRAZO", "AMBIENT_GANG_GAMBETTI" },
            DealerMenuGroup = "MethamphetamineDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 600,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2000,// 3000,
            HitPaymentMin = 2000,//5000,// 10000,
            HitPaymentMax = 3250,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2000,// 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 40f,
            PercentageWithSidearms = 30f,
            PercentageWithLongGuns = 15f,
            MemberKickUpAmount = 2500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "LOS ",
            GangClassification = GangClassification.Biker,
            MembersGetFreeVehicles = true,
            MembersGetFreeWeapons = true,
            LoanParameters = defaultLoanParameters,
        };//Meth
        Vagos = new Gang("~y~", "AMBIENT_GANG_MEXICAN", "Vagos", "Vagos", "Yellow", "VagosPeds", "VagosVehicles", "", "MeleeWeapons", "VagosSidearms", "VagosLongGuns", "Vagos", "CHAR_BLANK_ENTRY", "Vagos Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 400,
            DealerMemberMoneyMin = 400,
            DealerMemberMoneyMax = 1000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_SALVA" },
            DealerMenuGroup = "MarijuanaDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 800,// 1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 3250,//5000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,//4500,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "VAG ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//marijuana
        Varrios = new Gang("~b~", "AMBIENT_GANG_SALVA", "Varrios Los Aztecas", "Varrios", "Blue", "VarriosPeds", "VarriosVehicles", "", "MeleeWeapons", "VarriosSidearms", "VarriosLongGuns", "Varrios", "CHAR_BLANK_ENTRY", "Varrios Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 350,
            DealerMemberMoneyMax = 1100,
            HeadDataGroupID = "VarriosHeads",
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG" },
            DealerMenuGroup = "CrackDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 500,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 3000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 4000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2500,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 25f,
            PercentageWithSidearms = 25f,
            PercentageWithLongGuns = 10f,
            MemberKickUpAmount = 2200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "VAR ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//crack//Varrios are light blue
        Marabunte = new Gang("~b~", "AMBIENT_GANG_MARABUNTE", "Marabunta Grande", "Marabunta", "Blue", "MarabuntaPeds", "MarabuntaVehicles", "", "MeleeWeapons", "MarabuntaSidearms", "MarabuntaLongGuns", "Marabunta", "CHAR_BLANK_ENTRY", "Marabunta Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 250,
            DealerMemberMoneyMin = 250,
            DealerMemberMoneyMax = 850,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_MADRAZO" },
            DealerMenuGroup = "MarijuanaDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 800,//1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 3000,// 5000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3250,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2500,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2000,
            DrugDealerPercentage = 70f,
            LicensePlatePrefix = "MAR ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//marijuana
        Families = new Gang("~g~", "AMBIENT_GANG_FAMILY", "The Families", "Families", "Green", "FamiliesPeds", "FamiliesVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Families", "CHAR_BLANK_ENTRY", "Families Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 350,
            DealerMemberMoneyMax = 1100,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_BALLAS" },
            DealerMenuGroup = "MarijuanaDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 700,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//3000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 1800,//2000,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "FAM ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//marijuana
        Ballas = new Gang("~p~", "AMBIENT_GANG_BALLAS", "Ballas", "Ballas", "Purple", "BallasPeds", "BallasVehicles", "", "MeleeWeapons", "BallasSidearms", "BallasLongGuns", "Ballas", "CHAR_BLANK_ENTRY", "Ballas Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 325,
            DealerMemberMoneyMax = 1200,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_FAMILY" },
            DealerMenuGroup = "CrackDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//5000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1200,
            DeliveryPaymentMax = 2500,//4500,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2700,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "BAL ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//crack
        Triads = new Gang("~r~", "AMBIENT_GANG_WEICHENG", "Triads", "Triads", "Red", "TriadsPeds", "TriadVehicles", "", "MeleeWeapons", "TriadsSidearms", "TriadsLongGuns", "Triads", "CHAR_BLANK_ENTRY", "Triad Member")
        {
            DenName = "Meeting Spot",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 800,
            DealerMemberMoneyMax = 1900,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_SALVA" },
            DealerMenuGroup = "HeroinDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//5000,
            HitPaymentMin = 3000,//5000,// 10000,
            HitPaymentMax = 4000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2500,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 2500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 30f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 3000,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "TRI ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Cartel,
        };//heroin
        Redneck = new Gang("~b~", "AMBIENT_GANG_HILLBILLY", "Rednecks", "Rednecks", "Black", "RedneckPeds", "RedneckVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Rednecks", "CHAR_BLANK_ENTRY", "Redneck")
        {
            DenName = "Clubhouse",
            AmbientMemberMoneyMin = 5,
            AmbientMemberMoneyMax = 100,
            DealerMemberMoneyMin = 200,
            DealerMemberMoneyMax = 500,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG" },
            DealerMenuGroup = "ToiletCleanerDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 600,//1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//3000,
            HitPaymentMin = 1500,//5000,// 10000,
            HitPaymentMax = 3000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 1800,//2500,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1000,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 50f,
            PercentageWithLongGuns = 20f,
            MemberKickUpAmount = 1500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "RED ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Generic,
            MembersGetFreeWeapons = true,
        };//TOILET CLEANER
        Korean = new Gang("~q~", "AMBIENT_GANG_KKANGPAE", "Kkangpae", "Kkangpae", "Pink", "KoreanPeds", "KoreanVehicles", "", "MeleeWeapons", "KkangpaeSidearms", "KkangpaeLongGuns", "Kkangpae", "CHAR_BLANK_ENTRY", "Kkangpae Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 600,
            DealerMemberMoneyMin = 700,
            DealerMemberMoneyMax = 1700,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG" },
            DealerMenuGroup = "HeroinDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 1200,//2000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//6000,
            HitPaymentMin = 2500,// 5000,// 10000,
            HitPaymentMax = 4000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 3200,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 2500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 30f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 3200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "KKA ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//heroin
        float pistolPercentage = 55f;
        float longGunPercentage = 2f;
        Gambetti = new Gang("~g~", "AMBIENT_GANG_GAMBETTI", "Gambetti Crime Family", "Gambetti", "Green", "MafiaPeds", "GambettiVehicles", "", "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns", "Gambetti", "CHAR_BLANK_ENTRY", "Gambetti Associate")
        {
            DenName = "Safehouse",
            AmbientMemberMoneyMin = 200,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 650,
            DealerMemberMoneyMax = 2000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST" },
            DealerMenuGroup = "CokeDealerMenu",
            HeadDataGroupID = "MafiaHeads",

            PickupPaymentMin = 1000,
            PickupPaymentMax = 1400,
            TheftPaymentMin = 1500,
            TheftPaymentMax = 2500,
            HitPaymentMin = 3500,// 10000,
            HitPaymentMax = 5500,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 3500,
            StartingRep = 0,
            MaximumRep = 8000,
            MinimumRep = -8000,
            HitSquadRep = -6500,
            MemberOfferRepLevel = 7500,
            PercentageWithMelee = 5f,
            PercentageWithSidearms = pistolPercentage,
            PercentageWithLongGuns = longGunPercentage,
            MemberKickUpAmount = 6000,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "GAM ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//cocaine
        Pavano = new Gang("~g~", "AMBIENT_GANG_PAVANO", "Pavano Crime Family", "Pavano", "Green", "MafiaPeds", "PavanoVehicles", "", "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns", "Pavano", "CHAR_BLANK_ENTRY", "Pavano Associate")
        {
            DenName = "Safehouse",
            AmbientMemberMoneyMin = 200,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 650,
            DealerMemberMoneyMax = 2000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_ARMENIAN" },
            DealerMenuGroup = "CokeDealerMenu",
            HeadDataGroupID = "MafiaHeads",

            PickupPaymentMin = 1000,
            PickupPaymentMax = 1400,
            TheftPaymentMin = 1500,
            TheftPaymentMax = 2500,
            HitPaymentMin = 3500,// 10000,
            HitPaymentMax = 5500,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 3500,
            StartingRep = 0,
            MaximumRep = 8000,
            MinimumRep = -8000,
            MemberOfferRepLevel = 7500,
            HitSquadRep = -6500,
            PercentageWithMelee = 5f,
            PercentageWithSidearms = 35f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 5500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "PAV ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//cocaine
        Lupisella = new Gang("~g~", "AMBIENT_GANG_LUPISELLA", "Lupisella Crime Family", "Lupisella", "Green", "MafiaPeds", "LupisellaVehicles", "", "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns", "Lupisella", "CHAR_BLANK_ENTRY", "Lupisella Associate")
        {
            DenName = "Safehouse",
            AmbientMemberMoneyMin = 200,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 650,
            DealerMemberMoneyMax = 2000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_KKANGPAE" },
            DealerMenuGroup = "CokeDealerMenu",
            HeadDataGroupID = "MafiaHeads",

            PickupPaymentMin = 1000,
            PickupPaymentMax = 1400,
            TheftPaymentMin = 1500,
            TheftPaymentMax = 2500,
            HitPaymentMin = 3500,// 10000,
            HitPaymentMax = 5500,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 3500,
            StartingRep = 0,
            MaximumRep = 8000,
            MinimumRep = -8000,
            MemberOfferRepLevel = 7500,
            HitSquadRep = -6500,
            PercentageWithMelee = 5f,
            PercentageWithSidearms = 35f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 5700,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "LUP ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//cocaine
        Messina = new Gang("~g~", "AMBIENT_GANG_MESSINA", "Messina Crime Family", "Messina", "Green", "MafiaPeds", "MessinaVehicles", "", "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns", "Messina", "CHAR_BLANK_ENTRY", "Messina Associate")
        {
            DenName = "Safehouse",
            AmbientMemberMoneyMin = 200,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 650,
            DealerMemberMoneyMax = 2000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_SALVA" },
            DealerMenuGroup = "CokeDealerMenu",
            HeadDataGroupID = "MafiaHeads",

            PickupPaymentMin = 1000,
            PickupPaymentMax = 1400,
            TheftPaymentMin = 1500,
            TheftPaymentMax = 2500,
            HitPaymentMin = 3500,// 10000,
            HitPaymentMax = 5500,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 3500,
            StartingRep = 0,
            MaximumRep = 8000,
            MinimumRep = -8000,
            MemberOfferRepLevel = 7500,
            HitSquadRep = -6500,
            PercentageWithMelee = 5f,
            PercentageWithSidearms = 35f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 5200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "MES ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//cocaine
        Ancelotti = new Gang("~g~", "AMBIENT_GANG_ANCELOTTI", "Ancelotti Crime Family", "Ancelotti", "Green", "MafiaPeds", "AncelottiVehicles", "", "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns", "Ancelotti", "CHAR_BLANK_ENTRY", "Ancelotti Associate")
        {
            DenName = "Safehouse",
            AmbientMemberMoneyMin = 200,
            AmbientMemberMoneyMax = 500,
            DealerMemberMoneyMin = 650,
            DealerMemberMoneyMax = 2000,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST" },
            DealerMenuGroup = "CokeDealerMenu",
            HeadDataGroupID = "MafiaHeads",

            PickupPaymentMin = 1000,
            PickupPaymentMax = 1400,
            TheftPaymentMin = 1500,
            TheftPaymentMax = 2500,
            HitPaymentMin = 3500,// 10000,
            HitPaymentMax = 5500,//22000,
            DeliveryPaymentMin = 1500,
            DeliveryPaymentMax = 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 3500,
            StartingRep = 0,
            MaximumRep = 8000,
            MinimumRep = -8000,
            MemberOfferRepLevel = 7500,
            HitSquadRep = -6500,
            PercentageWithMelee = 5f,
            PercentageWithSidearms = 35f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 5500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "ANC ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//cocaine
        Cartel = new Gang("~r~", "AMBIENT_GANG_MADRAZO", "Madrazo Cartel", "Cartel", "Red", "CartelPeds", "CartelVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Cartel", "CHAR_BLANK_ENTRY", "Cartel Member")
        {
            DenName = "Mansion",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 600,
            DealerMemberMoneyMin = 450,
            DealerMemberMoneyMax = 1700,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_MARABUNTE" },
            DealerMenuGroup = "MethamphetamineDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 900,//1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 1900,//5000,
            HitPaymentMin = 2000,//5000,// 10000,
            HitPaymentMax = 3000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2000,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 3000,
            MinimumRep = -3000,
            MemberOfferRepLevel = 2500,
            HitSquadRep = -2500,
            PercentageWithMelee = 25f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 15f,
            MemberKickUpAmount = 2800,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "CAR ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Cartel,
            MembersGetFreeWeapons = true,
        };//Meth
        Armenian = new Gang("~b~", "AMBIENT_GANG_ARMENIAN", "Armenian Mob", "Armenian", "Black", "ArmenianPeds", "ArmeniaVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Armenians", "CHAR_BLANK_ENTRY", "Armenian Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 475,
            DealerMemberMoneyMax = 1500,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_GAMBETTI" },
            DealerMenuGroup = "HeroinDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 800,//1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 1900,//5000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2800,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 3000,
            MinimumRep = -3000,
            MemberOfferRepLevel = 2500,
            HitSquadRep = -2500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 25f,
            PercentageWithLongGuns = 15f,
            MemberKickUpAmount = 3000,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "ARM ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Cartel,
            MembersGetFreeVehicles = true,
        };//heroin
        Yardies = new Gang("~g~", "AMBIENT_GANG_YARDIES", "Yardies", "Yardies", "Green", "YardiesPeds", "YardieVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Yardies", "CHAR_BLANK_ENTRY", "Yardie Member")
        {
            DenName = "Chill Spot",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 250,
            DealerMemberMoneyMax = 900,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_KKANGPAE", "AMBIENT_GANG_DIABLOS" },
            DealerMenuGroup = "MarijuanaDealerMenu",

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 3000,
            MinimumRep = -3000,
            MemberOfferRepLevel = 2500,
            HitSquadRep = -2500,
            PercentageWithMelee = 15f,
            PercentageWithSidearms = 10f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 1500,
            DrugDealerPercentage = 90f,
            LicensePlatePrefix = "YAR ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Mafia,
        };//marijuana
        Diablos = new Gang("~r~", "AMBIENT_GANG_DIABLOS", "Diablos", "Diablos", "Red", "DiablosPeds", "DiablosVehicles", "", "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns", "Diablos", "CHAR_BLANK_ENTRY", "Diablo Soldier")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 20,
            AmbientMemberMoneyMax = 250,
            DealerMemberMoneyMin = 300,
            DealerMemberMoneyMax = 650,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_YARDIES" },
            DealerMenuGroup = "SPANKDealerMenu",
            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 3000,
            MinimumRep = -3000,
            MemberOfferRepLevel = 2500,
            HitSquadRep = -2500,
            PercentageWithMelee = 15f,
            PercentageWithSidearms = 15f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 1500,
            DrugDealerPercentage = 65f,
            HeadDataGroupID = "DiablosHeads",
            LicensePlatePrefix = "DIA ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//SPANK
        //new Gang("~w~", "AMBIENT_GANG_CULT", "Altruist Cult","Altruist", "White", "AltruistPeds", "GenericGangVehicles", "","MeleeWeapons","FamiliesSidearms","FamiliesLongGuns","Altruist Leader","CHAR_PA_MALE","Altruist Member") { 
        //                                DenName = "Gathering Location",AmbientMemberMoneyMin = 200, AmbientMemberMoneyMax = 1000,EnemyGangs = new List<string>() { "AMBIENT_GANG_HILLBILLY" }, DealerMenuGroup = "ToiletCleanerDealerMenu",
        //                                PickupPaymentMin = 100, PickupPaymentMax = 500, TheftPaymentMin = 500, TheftPaymentMax = 2000, HitPaymentMin = 5000, HitPaymentMax = 10000,DeliveryPaymentMin = 800, DeliveryPaymentMax = 3000
        //                                ,NeutralRepLevel = 0, FriendlyRepLevel = 4500, StartingRep = 0, MaximumRep = 5000, MinimumRep = -5000
        //                                ,PercentageWithMelee = 20f, PercentageWithSidearms = 30f, PercentageWithLongGuns = 5f} ,
    }
    private void DefaultConfig()
    {    
        GangsList = new List<Gang>
        {
            LOST,Vagos,Families,Ballas,Marabunte,Varrios,Triads,Redneck,Korean,Gambetti,Pavano,Lupisella,Messina,Ancelotti,Cartel,Armenian,Yardies,Diablos
        };
        Serialization.SerializeParams(GangsList, ConfigFileName);
    }
    private void DefaultConfig_Simple()
    {
        List<Gang>  SimpleGangsList = new List<Gang>
        {
            LOST,Vagos,Families,Ballas,Marabunte,Varrios,Triads,Redneck,Korean,Cartel,Armenian
        };
        Serialization.SerializeParams(SimpleGangsList, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\Gangs_Simple.xml");
    }

    private void DefaultConfig_LibertyCity()
    {
        //North Holland Hustlers
        //Petrovic mafia
        //Spanish Lords - puerto rican
        //ANgels of Death
        //Uptown RIders - black biker gang on crotch rockets
        //Korean Mob

        Gang LOST_LIB = Extensions.DeepCopy(LOST);
        LOST_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_ANGELS", "AMBIENT_GANG_YARDIES", "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_PETROVIC" };

        Gang Triads_LIB = Extensions.DeepCopy(Triads);
        Triads_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST", "AMBIENT_GANG_YARDIES", "AMBIENT_GANG_SPANISH", "AMBIENT_GANG_ANCELOTTI" };

        Gang Yardies_LIB = Extensions.DeepCopy(Yardies);
        Yardies_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST", "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_ANGELS" };

        Gang Gambetti_LIB = Extensions.DeepCopy(Gambetti);
        Gambetti_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_ANCELOTTI", "AMBIENT_GANG_PAVANO", "AMBIENT_GANG_KOREAN", "AMBIENT_GANG_PETROVIC" };

        Gang Pavano_LIB = Extensions.DeepCopy(Pavano);
        Pavano_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_GAMBETTI", };

        Gang Lupisella_LIB = Extensions.DeepCopy(Lupisella);
        Lupisella_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_KOREAN", "AMBIENT_GANG_PETROVIC" };

        Gang Messina_LIB = Extensions.DeepCopy(Messina);
        Messina_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_ANCELOTTI", "AMBIENT_GANG_HOLHUST" };

        Gang Ancelotti_LIB = Extensions.DeepCopy(Ancelotti);
        Ancelotti_LIB.EnemyGangs = new List<string>() { "AMBIENT_GANG_GAMBETTI", "AMBIENT_GANG_MESSINA" };

        Gang SpanishLords = new Gang("~b~", "AMBIENT_GANG_SPANISH", "The Spanish Lords", "Spanish Lords", "Blue",
            "SpanishLordsPeds", "SpanishLordsVehicles", "",
            "MeleeWeapons", "VarriosSidearms", "VarriosLongGuns",
            "Spanish Lords", "CHAR_BLANK_ENTRY", "Lords Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 350,
            DealerMemberMoneyMax = 1100,
            HeadDataGroupID = "VarriosHeads",
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG" },
            DealerMenuGroup = "CrackDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 500,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 3000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 4000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2500,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 25f,
            PercentageWithSidearms = 25f,
            PercentageWithLongGuns = 10f,
            MemberKickUpAmount = 2200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "VAR ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };

        Gang KoreanMob = new Gang("~q~", "AMBIENT_GANG_KOREAN",
            "Korean Mob", "Korean Mob", "Pink", 
            "KoreanPeds", "KoreanVehicles", "",
            "MeleeWeapons", "KkangpaeSidearms", "KkangpaeLongGuns",
            "Korean Mob", "CHAR_BLANK_ENTRY", "Korean Mob Member")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 600,
            DealerMemberMoneyMin = 700,
            DealerMemberMoneyMax = 1700,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_GAMBETTI", "AMBIENT_GANG_PETROVIC" },
            DealerMenuGroup = "HeroinDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 1200,//2000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//6000,
            HitPaymentMin = 2500,// 5000,// 10000,
            HitPaymentMax = 4000,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 3200,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 2500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 30f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 2f,
            MemberKickUpAmount = 3200,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "KKA ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//heroin

        Gang NorthHollandHuslters = new Gang("~r~", "AMBIENT_GANG_HOLHUST", "The North Holland Hustlers", "N Hol Hustlers", "Red",
            "NorthHollandPeds", "NorthHollandVehicles", "", 
            "MeleeWeapons", "FamiliesSidearms", "FamiliesLongGuns",
            "North Holland Hustlers", "CHAR_BLANK_ENTRY", "Huslters Member")
        {
            DenName = "Hangout",
            HeadDataGroupID = "NorthHollandHeads",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 350,
            DealerMemberMoneyMax = 1100,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_ARMENIAN" },
            DealerMenuGroup = "CrackDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 700,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//3000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 1800,//2000,//4000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "NHH ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Street,
        };//crack

        Gang PetrovicMafia = new Gang("~w~", "AMBIENT_GANG_PETROVIC", "Petrovic Crime Syndicate", "Petrovic", "White",
            "PetrovicPeds", "PetrovicVehicles", "PET ",
            "MeleeWeapons", "MafiaSidearms", "MafiaLongGuns",
            "Petrovic", "CHAR_BLANK_ENTRY","Petrovic Associate")
        {
            DenName = "Safehouse",
            HeadDataGroupID = "PetrovicHeads",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 250,
            DealerMemberMoneyMin = 400,
            DealerMemberMoneyMax = 1500,
            
            EnemyGangs = new List<string>() { "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_PAVANO", "AMBIENT_GANG_ANGELS", "AMBIENT_GANG_KOREAN" },
            DealerMenuGroup = "CokeDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 600,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2000,// 3000,
            HitPaymentMin = 2000,//5000,// 10000,
            HitPaymentMax = 3250,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2000,// 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 40f,
            PercentageWithSidearms = 30f,
            PercentageWithLongGuns = 15f,
            MemberKickUpAmount = 2500,
            DrugDealerPercentage = 65f,
            GangClassification = GangClassification.Generic,
            MembersGetFreeVehicles = true,
            MembersGetFreeWeapons = true,
            LoanParameters = defaultLoanParameters,
        };//Coke

        Gang AngelsOfDeath = new Gang("~w~", "AMBIENT_GANG_ANGELS", "The Angels of Death", "AOD MC", "White",
            "AngelsOfDeathPeds", "AngelsOfDeathVehicles", "AOD ", 
            "MeleeWeapons", "LostSidearms", "LostLongGuns",
            "AOD MC", "CHAR_BLANK_ENTRY", "AOD Member")
        {
            DenName = "Clubhouse",
            HeadDataGroupID = "AngelsOfDeathHeads",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 250,
            DealerMemberMoneyMin = 400,
            DealerMemberMoneyMax = 1500,
            EnemyGangs = new List<string>() { "AMBIENT_GANG_LOST", "AMBIENT_GANG_UPTOWN", "AMBIENT_GANG_PETROVIC", "AMBIENT_GANG_YARDIES" },
            DealerMenuGroup = "MethamphetamineDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 600,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2000,// 3000,
            HitPaymentMin = 2000,//5000,// 10000,
            HitPaymentMax = 3250,//12000,//22000,
            DeliveryPaymentMin = 1000,
            DeliveryPaymentMax = 2000,// 3000,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 40f,
            PercentageWithSidearms = 30f,
            PercentageWithLongGuns = 15f,
            MemberKickUpAmount = 2500,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "AOD ",
            GangClassification = GangClassification.Biker,
            MembersGetFreeVehicles = true,
            MembersGetFreeWeapons = true,
            LoanParameters = defaultLoanParameters,
        };//Meth

        Gang UptownRiders = new Gang("~w~", "AMBIENT_GANG_UPTOWN", 
            "Uptown Riders", "Uptown Riders", "White",
            "UptownRidersPeds", "UptownRidersVehicles", "",
            "MeleeWeapons", "BallasSidearms", "BallasLongGuns",
            "Uptown Riders", "CHAR_BLANK_ENTRY", "Uptown Rider")
        {
            DenName = "Hangout",
            AmbientMemberMoneyMin = 100,
            AmbientMemberMoneyMax = 300,
            DealerMemberMoneyMin = 325,
            DealerMemberMoneyMax = 1200,
            HeadDataGroupID = "UptownRidersHeads",
            
            EnemyGangs = new List<string>() { "AMBIENT_GANG_ANGELS" },
            DealerMenuGroup = "MethamphetamineDealerMenu",

            PickupPaymentMin = 200,
            PickupPaymentMax = 1000,
            TheftPaymentMin = 1000,
            TheftPaymentMax = 2500,//5000,
            HitPaymentMin = 2500,//5000,// 10000,
            HitPaymentMax = 3500,//12000,//22000,
            DeliveryPaymentMin = 1200,
            DeliveryPaymentMax = 2500,//4500,

            NeutralRepLevel = 0,
            FriendlyRepLevel = 1500,
            StartingRep = 0,
            MaximumRep = 5000,
            MinimumRep = -5000,
            MemberOfferRepLevel = 4500,
            HitSquadRep = -4500,
            PercentageWithMelee = 20f,
            PercentageWithSidearms = 20f,
            PercentageWithLongGuns = 5f,
            MemberKickUpAmount = 2700,
            DrugDealerPercentage = 65f,
            LicensePlatePrefix = "UT ",
            LoanParameters = defaultLoanParameters,
            GangClassification = GangClassification.Biker,
        };//Meth

        List<Gang> LCGangsList = new List<Gang>
        {
            LOST_LIB,
            Triads_LIB,
            Yardies_LIB,
            Gambetti_LIB,Pavano_LIB,Lupisella_LIB,Messina_LIB,Ancelotti_LIB,

            //New
            PetrovicMafia,
            NorthHollandHuslters,
            SpanishLords,
            KoreanMob,
            AngelsOfDeath,
            UptownRiders
        };
        Serialization.SerializeParams(LCGangsList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Gangs_{StaticStrings.LibertyConfigSuffix}.xml");

        List<Gang> LPPGangsList = new List<Gang>
        {
            LOST_LIB,
            Triads_LIB,
            Yardies_LIB,
            Gambetti_LIB,Pavano_LIB,Lupisella_LIB,Messina_LIB,Ancelotti_LIB,


            Vagos,Families,Ballas,Marabunte,Varrios,Redneck,Korean,Cartel,Armenian,Diablos,


            //New
            PetrovicMafia,
            NorthHollandHuslters,
            SpanishLords,
            KoreanMob,
            AngelsOfDeath,
            UptownRiders
        };
        Serialization.SerializeParams(LPPGangsList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Gangs_{StaticStrings.LPPConfigSuffix}.xml");

    }
    public void CheckTerritory(GangTerritories gangTerritories)
    {
        foreach(Gang gang in GangsList)
        {
            List<ZoneJurisdiction> totalTerritory = gangTerritories.GetGangTerritory(gang.ID);
            if(totalTerritory == null || !totalTerritory.Any())
            {
                EntryPoint.WriteToConsole($"${gang.ID} HAS NO TERRITORY",0);
            }
        }
    }
}