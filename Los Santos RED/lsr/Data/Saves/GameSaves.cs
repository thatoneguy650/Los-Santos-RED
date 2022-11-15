using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GameSaves : IGameSaves
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\SaveGames.xml";
    public GameSaves()
    {
    }
    public List<GameSave> GameSaveList { get; private set; } = new List<GameSave>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("SaveGames*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Games Saves config: {ConfigFile.FullName}", 0);
            GameSaveList = Serialization.DeserializeParams<GameSave>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Game Saves config  {ConfigFileName}", 0);
            GameSaveList = Serialization.DeserializeParams<GameSave>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Game Saves config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void Save(ISaveable player, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
    {
        GameSave mySave = new GameSave();
        GameSaveList.Add(mySave);     
        mySave.Save(player, weapons, time, placesOfInterest, modItems);
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    public void SaveSamePlayer(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
    {
        GameSave mySave = GetSave(player);
        if(mySave == null)
        {
            mySave = new GameSave();
            GameSaveList.Add(mySave);
        }
        mySave.Save(player, weapons, time, placesOfInterest, modItems);    
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    public void Load(GameSave gameSave, IWeapons weapons, IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
    {       
        gameSave.Load(weapons, pedSwap, player, settings, world, gangs, time, placesOfInterest, modItems);
    }
    public GameSave GetSave(ISaveable player)
    {
        GameSaveList = Serialization.DeserializeParams<GameSave>(ConfigFileName);
        return GameSaveList.FirstOrDefault(x => x.PlayerName == player.PlayerName && x.ModelName == player.ModelName);
    }
    public void UpdateSave(GameSave toUpdate)
    {
        if(toUpdate != null)
        {
            Serialization.SerializeParams(GameSaveList, ConfigFileName);
        }
    }
    public void DeleteSave(string playerName, string modelName)
    {
        GameSave toDelete = GameSaveList.FirstOrDefault(x => x.PlayerName == playerName && x.ModelName == modelName);
        if(toDelete != null)
        {
            GameSaveList.Remove(toDelete);
        }
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }

    public void DeleteSave(GameSave toDelete)
    {
        if (toDelete != null)
        {
            GameSaveList.Remove(toDelete);
        }
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    private void DefaultConfig()
    {
        GameSaveList = new List<GameSave>();
        AddAlexis();
        AddClaude();
        //AddMichaelJones();
        AddLamar();
        AddBrad();
#if DEBUG
        AddDaveNorton();
        AddKarenDaniels();
        AddMaleMPCop();
        AddFemaleMPCop();
#endif
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    private void AddAlexis()
    {
        PedVariation AlexisVariation = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 14, 0, 0) ,
            new PedComponent(4, 11, 8, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 11, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 8, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 49, 1, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>()
        {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 12,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        11,
        12);
        List<StoredWeapon> AlexisWeapons = new List<StoredWeapon>
        {
            new StoredWeapon(4222310262, Vector3.Zero, new WeaponVariation(), 0),
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 50),

            new StoredWeapon(324215364, Vector3.Zero, new WeaponVariation(new List<WeaponComponent>() { new WeaponComponent("Extended Clip") } ), 120),

            new StoredWeapon(3756226112, Vector3.Zero, new WeaponVariation(), 0),
        };
        GameSave AlexisGameSave = new GameSave("Alexis Davis", 15500, "MP_F_FREEMODE_01", false, AlexisVariation, AlexisWeapons, new List<VehicleSaveStatus>() { 
            new VehicleSaveStatus("furoregt", new Vector3(-365.8749f, -179.3706f, 36.62038f), 206.9494f){ VehicleVariation = new VehicleVariation() { PrimaryColor =  111, SecondaryColor = 111, LicensePlate = new LSR.Vehicles.LicensePlate("125JK34", 0, false) } } });


        AlexisGameSave.PlayerPosition = new Vector3(-380.8266f, -192.3129f, 36.84449f);
        AlexisGameSave.PlayerHeading = 281.9426f;
        AlexisGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);

        AlexisGameSave.InventoryItems.Add(new InventorySave("Marijuana", 4.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("DIC Lighter", 1.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Can of eCola", 2.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Redwood Regular", 15.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Marijuana", 4.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Hot Dog", 1.0f));

        //

        AlexisGameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Flint Flathead Screwdriver", 1.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("Flint Pliers", 1.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("GASH Black Umbrella", 1.0f));
       // AlexisGameSave.InventoryItems.Add(new InventorySave("GASH Blue Umbrella", 1.0f));
       // AlexisGameSave.InventoryItems.Add(new InventorySave("SCHEISS DS Binoculars", 1.0f));
        AlexisGameSave.InventoryItems.Add(new InventorySave("SCHEISS RP Binoculars", 1.0f));
        




        AlexisGameSave.HungerValue = 75.0f;
        AlexisGameSave.ThirstValue = 75.0f;
        AlexisGameSave.SleepValue = 75.0f;
        AlexisGameSave.SpeechSkill = 80;


        AlexisGameSave.Contacts.Add(new SavedContact(EntryPoint.UndergroundGunsContactName, 30, "CHAR_BLANK_ENTRY"));
        AlexisGameSave.DriversLicense = new DriversLicense() { IssueDate = AlexisGameSave.CurrentDateTime, ExpirationDate = AlexisGameSave.CurrentDateTime.AddMonths(12) };
        AlexisGameSave.CCWLicense = new CCWLicense() { IssueDate = AlexisGameSave.CurrentDateTime, ExpirationDate = AlexisGameSave.CurrentDateTime.AddMonths(12) };
        AlexisGameSave.SavedResidences.Add(new SavedResidence("70W Carcer Way Apt 343", false, true) { RentalPaymentDate = AlexisGameSave.CurrentDateTime.AddDays(28), DateOfLastRentalPayment = AlexisGameSave.CurrentDateTime });
        AlexisGameSave.GangReputationsSave = new List<GangRepSave>() { new GangRepSave("Gambetti", 4000, 0, 0, 0, 0, 0, 0, 0, false, false) };

        GameSaveList.Add(AlexisGameSave);
    }
    private void AddClaude()
    {
        PedVariation Variation = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 38, 0, 0) ,
            new PedComponent(3, 0, 0, 0) ,
            new PedComponent(4, 129, 2, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 31, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 15, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 0, 1, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },},
        new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f),
        3,
        3);
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(0x2B5EF5EC, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(2508868239, Vector3.Zero, new WeaponVariation(), 0),
        };
        GameSave ClaudeGameSave = new GameSave("Claude Speed", 9500, "MP_M_FREEMODE_01", true, Variation, Weapons, new List<VehicleSaveStatus>() {
            new VehicleSaveStatus("stalion", new Vector3(866.8428f, -1594.666f, 30.80709f), 84.34846f) { VehicleVariation = new VehicleVariation() {



                PrimaryColor = 28, SecondaryColor = 120
                ,WheelColor = 156
                ,Mod1PaintType = 1
                ,Mod1Color = 0
                ,Mod2PaintType = 1
                ,WheelType = 7
                ,HasCustomWheels = true
                ,VehicleExtras = new List<VehicleExtra>()
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
                }
                ,VehicleToggles =  new List<VehicleToggle>()
                {
                    new VehicleToggle(17,false),
                    new VehicleToggle(18,true),
                    new VehicleToggle(19,false),
                    new VehicleToggle(20,false),
                    new VehicleToggle(21,false),
                    new VehicleToggle(22,true),
                }
                , VehicleMods = new List<VehicleMod>()
                {
                    new VehicleMod(0,-1),
                    new VehicleMod(1,-1),
                    new VehicleMod(2,-1),
                    new VehicleMod(3,-1),
                    new VehicleMod(4,-1),
                    new VehicleMod(5,-1),
                    new VehicleMod(6,-1),
                    new VehicleMod(7,-1),
                    new VehicleMod(8,-1),
                    new VehicleMod(9,-1),
                    new VehicleMod(10,-1),
                    new VehicleMod(11,3),
                    new VehicleMod(12,2),
                    new VehicleMod(13,2),
                    new VehicleMod(14,-1),
                    new VehicleMod(15,3),
                    new VehicleMod(16,-1),
                    new VehicleMod(17,-1),
                    new VehicleMod(18,-1),
                    new VehicleMod(19,-1),
                    new VehicleMod(20,-1),
                    new VehicleMod(21,-1),
                    new VehicleMod(22,-1),
                    new VehicleMod(23,2),
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
                    new VehicleMod(43,-1),
                    new VehicleMod(44,-1),
                    new VehicleMod(45,-1),
                    new VehicleMod(46,-1),
                    new VehicleMod(47,-1),
                    new VehicleMod(48,-1),
                    new VehicleMod(49,-1),
                    new VehicleMod(50,3),

                }
                , LicensePlate = new LSR.Vehicles.LicensePlate("5GNU769", 0, false)





            } }
                                                                                                                                                                 }); ;

        ClaudeGameSave.PlayerPosition = new Vector3(860.6456f, -1591.222f, 31.7381f);
        ClaudeGameSave.PlayerHeading = 107.1845f;
        ClaudeGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);
        ClaudeGameSave.Contacts.Add(new SavedContact(EntryPoint.UndergroundGunsContactName, 30, "CHAR_BLANK_ENTRY"));
        ClaudeGameSave.SavedResidences.Add(new SavedResidence("310S Popular Street", false, true) { RentalPaymentDate = ClaudeGameSave.CurrentDateTime.AddDays(28), DateOfLastRentalPayment = ClaudeGameSave.CurrentDateTime });
        ClaudeGameSave.GangReputationsSave = new List<GangRepSave>() { 
            
            
            
            new GangRepSave("AMBIENT_GANG_MARABUNTE", 3000, 0, 0, 0, 0, 0, 0, 0, false, false),
        new GangRepSave("AMBIENT_GANG_LOST", 3000, 0, 0, 0, 0, 0, 0, 0, false, false),
        new GangRepSave("AMBIENT_GANG_ARMENIAN", 3000, 0, 0, 0, 0, 0, 0, 0, false, false),
        new GangRepSave("AMBIENT_GANG_YARDIES", -3000, 3, 1, 0, 3, 1, 0, 0, false, false),
        new GangRepSave("AMBIENT_GANG_DIABLOS", -3000, 5, 3, 3, 5, 3, 3, 0, false, false),


        };

        ClaudeGameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        ClaudeGameSave.InventoryItems.Add(new InventorySave("Flint Flathead Screwdriver", 1.0f));
        ClaudeGameSave.InventoryItems.Add(new InventorySave("Flint Pliers", 1.0f));
        ClaudeGameSave.InventoryItems.Add(new InventorySave("SCHEISS DS Binoculars", 1.0f));


        ClaudeGameSave.HungerValue = 85.0f;
        ClaudeGameSave.ThirstValue = 85.0f;
        ClaudeGameSave.SleepValue = 85.0f;
        ClaudeGameSave.SpeechSkill = 0;

        GameSaveList.Add(ClaudeGameSave);
    }
    private void AddMichaelJones()
    {
        List<StoredWeapon> weapons = new List<StoredWeapon>
        {
            new StoredWeapon(0x2B5EF5EC, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(2508868239, Vector3.Zero, new WeaponVariation(), 0),
        };
        PedVariation TestVar1 = new PedVariation(new List<PedComponent>() { new PedComponent(3, 4, 0, 0), new PedComponent(4, 10, 0, 0), new PedComponent(6, 10, 0, 0), new PedComponent(7, 21, 2, 0), new PedComponent(8, 10, 0, 0), new PedComponent(11, 4, 0, 0) }, new List<PedPropComponent>() { });

        GameSave TestGameSave = new GameSave("Michael Jones", 950000, "MP_M_FREEMODE_01", true, TestVar1, weapons, new List<VehicleSaveStatus>() { new VehicleSaveStatus("sentinel", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f) { VehicleVariation = new VehicleVariation() { PrimaryColor = 0, SecondaryColor = 0, LicensePlate = new LSR.Vehicles.LicensePlate("JG234F1", 0, false) } }
                                                                                                                                                                });

        TestGameSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        TestGameSave.PlayerHeading = 45f;
        TestGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);

        TestGameSave.Contacts.Add(new SavedContact(EntryPoint.UndergroundGunsContactName, 30, "CHAR_BLANK_ENTRY"));
        TestGameSave.DriversLicense = new DriversLicense() { IssueDate = TestGameSave.CurrentDateTime, ExpirationDate = TestGameSave.CurrentDateTime.AddMonths(12) };
        TestGameSave.CCWLicense = new CCWLicense() { IssueDate = TestGameSave.CurrentDateTime, ExpirationDate = TestGameSave.CurrentDateTime.AddMonths(12) };
        TestGameSave.SavedResidences.Add(new SavedResidence("566 Ineseno Road", false, true) { RentalPaymentDate = TestGameSave.CurrentDateTime.AddDays(28), DateOfLastRentalPayment = TestGameSave.CurrentDateTime });
        TestGameSave.SavedResidences.Add(new SavedResidence("805 Ineseno Road", true, false) { });
        TestGameSave.GangReputationsSave = new List<GangRepSave>() { new GangRepSave("AMBIENT_GANG_LOST", 3000, 0, 0, 0, 0, 0, 0, 0, false, false) };
        TestGameSave.HungerValue = 75.0f;
        TestGameSave.ThirstValue = 75.0f;
        TestGameSave.SleepValue = 75.0f;
        TestGameSave.SpeechSkill = 80;

        GameSaveList.Add(TestGameSave);
    }
    private void AddGenericMale()
    {
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {

            new StoredWeapon(0x2B5EF5EC, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(2508868239, Vector3.Zero, new WeaponVariation(), 0),
        };
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>() {
            new VehicleSaveStatus("sentinel", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f) {
                VehicleVariation = new VehicleVariation() { PrimaryColor = 0, SecondaryColor = 0, LicensePlate = new LSR.Vehicles.LicensePlate("JG234F1", 0, false) } }
        };
        PedVariation Variation = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 10, 0, 0),
                new PedComponent(7, 21, 2, 0),
                new PedComponent(8, 10, 0, 0),
                new PedComponent(11, 4, 0, 0) },
            new List<PedPropComponent>() { });

        GameSave ExampleGameSave = new GameSave("John Doe", 950000, "MP_M_FREEMODE_01", true, Variation, Weapons, Vehicles);
        //Position
        ExampleGameSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        ExampleGameSave.PlayerHeading = 45f;
        //Date
        ExampleGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);
        //Contacts
        ExampleGameSave.Contacts.Add(new SavedContact(EntryPoint.UndergroundGunsContactName, 30, "CHAR_BLANK_ENTRY"));
        //Licenses
        ExampleGameSave.DriversLicense = new DriversLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        ExampleGameSave.CCWLicense = new CCWLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        //Residences
        ExampleGameSave.SavedResidences.Add(new SavedResidence("566 Ineseno Road", false, true) { RentalPaymentDate = ExampleGameSave.CurrentDateTime.AddDays(28), DateOfLastRentalPayment = ExampleGameSave.CurrentDateTime });
        ExampleGameSave.SavedResidences.Add(new SavedResidence("805 Ineseno Road", true, false) { });
        //Gang Items
        ExampleGameSave.GangReputationsSave = new List<GangRepSave>() { new GangRepSave("AMBIENT_GANG_LOST", 3000, 0, 0, 0, 0, 0, 0, 0, false, false) };
        //Needs
        ExampleGameSave.HungerValue = 75.0f;
        ExampleGameSave.ThirstValue = 75.0f;
        ExampleGameSave.SleepValue = 75.0f;
        //Speech
        ExampleGameSave.SpeechSkill = 80;


        GameSaveList.Add(ExampleGameSave);
    }
    private void AddLamar()
    {
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(324215364, Vector3.Zero, new WeaponVariation(new List<WeaponComponent>() { new WeaponComponent("Extended Clip") } ), 120),
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(3756226112, Vector3.Zero, new WeaponVariation(), 0),
        };
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>() { 
            new VehicleSaveStatus("emperor", new Vector3(-76.17985f, -1457.356f, 31.50146f), 207.2386f) { 
                VehicleVariation = new VehicleVariation() { PrimaryColor = 0, SecondaryColor = 0, LicensePlate = new LSR.Vehicles.LicensePlate("LD2", 0, false) } }
        };
        PedVariation Variation = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(1, 2, 0, 0),
                new PedComponent(2, 2, 0, 0),
                new PedComponent(3, 2, 2, 0),
                new PedComponent(4, 5, 0, 0),
                new PedComponent(5, 0, 0, 0),
                new PedComponent(6, 1, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 0, 0, 0),
                new PedComponent(9, 0, 0, 0),
                new PedComponent(10, 1, 0, 0),
                new PedComponent(11, 0, 0, 0),
            },
            new List<PedPropComponent>() { });

        GameSave ExampleGameSave = new GameSave("Lamar Davis", 78534, "ig_lamardavis", true, Variation, Weapons, Vehicles);
        //Position
        ExampleGameSave.PlayerPosition = new Vector3(-61.21735f, -1457.369f, 32.09263f);
        ExampleGameSave.PlayerHeading = 148.086f;
        //Date
        ExampleGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);
        //Contacts
        ExampleGameSave.Contacts.Add(new SavedContact(EntryPoint.UndergroundGunsContactName, 30, "CHAR_BLANK_ENTRY"));
        //Licenses
        ExampleGameSave.DriversLicense = new DriversLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        ExampleGameSave.CCWLicense = new CCWLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        //Residences
        ExampleGameSave.SavedResidences.Add(new SavedResidence("280S Forum Dr No 1", true, false) { });
        //Gang Items
        ExampleGameSave.GangReputationsSave = new List<GangRepSave>() { 
            new GangRepSave("AMBIENT_GANG_FAMILY", 5000, 0, 0, 0, 0, 0, 0, 0, false, false), 
            new GangRepSave("AMBIENT_GANG_BALLAS", -5000, 5, 1, 0, 0, 0, 0, 0, false, false),
            new GangRepSave("AMBIENT_GANG_WEICHENG", 5000, 0, 0, 0, 0, 0, 0, 0, false, false),


        };
        ExampleGameSave.InventoryItems.Add(new InventorySave("Flint Flathead Screwdriver", 1.0f));
        ExampleGameSave.InventoryItems.Add(new InventorySave("SCHEISS BS Binoculars", 1.0f));

        //Needs
        ExampleGameSave.HungerValue = 95.0f;
        ExampleGameSave.ThirstValue = 95.0f;
        ExampleGameSave.SleepValue = 95.0f;
        //Speech
        ExampleGameSave.SpeechSkill = 85;


        GameSaveList.Add(ExampleGameSave);
    }
    private void AddBrad()
    {
        GameSave gameSave = new GameSave("Bradley Snider", 250, "ig_brad", true, new PedVariation(new List<PedComponent>() { },new List<PedPropComponent>() { }), new List<StoredWeapon>{ }, new List<VehicleSaveStatus>() { });
        SetDefault(gameSave);
        gameSave.InventoryItems.Add(new InventorySave("Flint Flathead Screwdriver", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("SCHEISS BS Binoculars", 1.0f));
        GameSaveList.Add(gameSave);
    }
    private void AddDaveNorton()
    {
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>() 
        {
            new VehicleSaveStatus("oracle2", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f){ VehicleVariation = new VehicleVariation() { PrimaryColor =  61, SecondaryColor = 61, LicensePlate = new LSR.Vehicles.LicensePlate("7CVJ356", 0, false) } } 
        };
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 48),
        };
        GameSave gameSave = new GameSave("Dave Norton", 455000, "ig_davenorton", true, new PedVariation(new List<PedComponent>() { }, new List<PedPropComponent>() { }), Weapons, Vehicles);
        SetDefault(gameSave);

        gameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("GASH Black Umbrella", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("SCHEISS RP Binoculars", 1.0f));

        gameSave.IsCop = true;
        GameSaveList.Add(gameSave);
    }
    private void AddKarenDaniels()
    {
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>() 
        {
            new VehicleSaveStatus("zion", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f){ VehicleVariation = new VehicleVariation() { PrimaryColor =  0, SecondaryColor = 0, LicensePlate = new LSR.Vehicles.LicensePlate("1RCT244", 0, false) } } 
        };
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(1593441988, Vector3.Zero, new WeaponVariation(), 36),//combat pistol
        };
        GameSave gameSave = new GameSave("Karen Daniels", 867000, "ig_karen_daniels", true, new PedVariation(new List<PedComponent>() { }, new List<PedPropComponent>() { }), Weapons, Vehicles);
        SetDefault(gameSave);
        gameSave.IsCop = true;

        gameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("GASH Black Umbrella", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("SCHEISS RP Binoculars", 1.0f));

        GameSaveList.Add(gameSave);
    }

    private void AddMaleMPCop()
    {
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>()
        {
            new VehicleSaveStatus("police2", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f){ VehicleVariation = new VehicleVariation() { PrimaryColor =  61, SecondaryColor = 61, LicensePlate = new LSR.Vehicles.LicensePlate("7CVJ356", 0, false) } }
        };
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 48),
        };
        GameSave gameSave = new GameSave("Officer Speed", 455000, "mp_m_freemode_01", true, new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 38, 0, 0), new PedComponent(4, 35, 0, 0), new PedComponent(6, 25, 0, 0), new PedComponent(8, 58, 0, 0), new PedComponent(10, 8, 2, 0), new PedComponent(11, 55, 0, 0) },
                    new List<PedPropComponent>() { }, new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },},
        new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f),
        3,
        3), Weapons, Vehicles);
        SetDefault(gameSave);

        gameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("GASH Black Umbrella", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("SCHEISS RP Binoculars", 1.0f));

        gameSave.IsCop = true;
        GameSaveList.Add(gameSave);
    }
    private void AddFemaleMPCop()
    {
        List<VehicleSaveStatus> Vehicles = new List<VehicleSaveStatus>()
        {
            new VehicleSaveStatus("police", new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f){ VehicleVariation = new VehicleVariation() { PrimaryColor =  61, SecondaryColor = 61, LicensePlate = new LSR.Vehicles.LicensePlate("7CVJ356", 0, false) } }
        };
        List<StoredWeapon> Weapons = new List<StoredWeapon>
        {
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 48),
        };
        GameSave gameSave = new GameSave("Officer Daniels", 455000, "mp_f_freemode_01", true, new PedVariation(
                    new List<PedComponent>() { new PedComponent(2, 42, 0, 0), new PedComponent(3, 14, 0, 0), new PedComponent(4, 34, 0, 0), new PedComponent(6, 55, 0, 0), new PedComponent(8, 35, 0, 0), new PedComponent(10, 7, 1, 0), new PedComponent(11, 48, 0, 0) },
                    new List<PedPropComponent>() { }, new List<HeadOverlayData>()
        {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 12,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        11,
        12), Weapons, Vehicles);
        SetDefault(gameSave);

        gameSave.InventoryItems.Add(new InventorySave("TAG-HARD Flashlight", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("GASH Black Umbrella", 1.0f));
        gameSave.InventoryItems.Add(new InventorySave("SCHEISS RP Binoculars", 1.0f));

        gameSave.IsCop = true;
        GameSaveList.Add(gameSave);
    }

    private void SetDefault(GameSave ExampleGameSave)
    {
        //Position
        ExampleGameSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        ExampleGameSave.PlayerHeading = 45f;
        //Date
        ExampleGameSave.CurrentDateTime = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 13, 30, 0);
        //Licenses
        ExampleGameSave.DriversLicense = new DriversLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        ExampleGameSave.CCWLicense = new CCWLicense() { IssueDate = ExampleGameSave.CurrentDateTime, ExpirationDate = ExampleGameSave.CurrentDateTime.AddMonths(12) };
        //Needs
        ExampleGameSave.HungerValue = 95.0f;
        ExampleGameSave.ThirstValue = 95.0f;
        ExampleGameSave.SleepValue = 95.0f;
        //Speech
        ExampleGameSave.SpeechSkill = 35;
    }

    //public void Save(ISaveable saveable, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems)
    //{
    //    throw new System.NotImplementedException();
    //}
}

