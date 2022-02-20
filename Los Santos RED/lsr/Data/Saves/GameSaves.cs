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
        if (File.Exists(ConfigFileName))
        {
            GameSaveList = Serialization.DeserializeParams<GameSave>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(GameSaveList, ConfigFileName);
        }
    }
    public void Save(ISaveable player, IWeapons weapons, ITimeReportable time)
    {
        GameSave mySave = GetSave(player);
        if(mySave == null)
        {
            mySave = new GameSave();
            GameSaveList.Add(mySave);
        }
        mySave.Save(player, weapons, time);    
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    public void Load(GameSave gameSave, IWeapons weapons, IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time)
    {       
        gameSave.Load(weapons, pedSwap, player, settings, world, gangs, time);
    }
    public GameSave GetSave(ISaveable player)
    {
        GameSaveList = Serialization.DeserializeParams<GameSave>(ConfigFileName);
        return GameSaveList.FirstOrDefault(x => x.PlayerName == player.PlayerName && x.ModelName == player.ModelName);
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
    private void DefaultConfig()
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
            new PedComponent(7, 2, 0, 0) ,
            new PedComponent(8, 8, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 49, 1, 0)
        },
        new List<PedPropComponent>()
        {
            new PedPropComponent(0, -1, -1),
            new PedPropComponent(1, -1, -1),
            new PedPropComponent(2, -1, -1),
            new PedPropComponent(3, -1, -1),
            new PedPropComponent(4, -1, -1),
            new PedPropComponent(5, -1, -1),
            new PedPropComponent(6, -1, -1),
            new PedPropComponent(7, -1, -1),
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
        GameSave AlexisGameSave = new GameSave("Alexis Davis", 15500, "MP_F_FREEMODE_01", false, AlexisVariation, AlexisWeapons, new List<VehicleVariation>() { new VehicleVariation("coquette2", 4, 4, new LSR.Vehicles.LicensePlate("AZZ KIKR", 3, false), new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f)
                                                                                                                                                                ,new VehicleVariation("furoregt", 111, 111, new LSR.Vehicles.LicensePlate("125JK34", 0, false), new Vector3(-382.2991f, -301.4909f, 32.56747f), 287.2352f)});
        AlexisGameSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        AlexisGameSave.PlayerHeading = 45f;
        AlexisGameSave.CurrentDateTime = System.DateTime.Now;


        PedVariation FemalePolice = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 14, 0, 0) ,
            new PedComponent(4, 34, 0, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 55, 0, 0) ,
            new PedComponent(7, 2, 0, 0) ,
            new PedComponent(8, 35, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 48, 0, 0)
        },
        new List<PedPropComponent>()
        {
            new PedPropComponent(0, -1, -1),
            new PedPropComponent(1, -1, -1),
            new PedPropComponent(2, -1, -1),
            new PedPropComponent(3, -1, -1),
            new PedPropComponent(4, -1, -1),
            new PedPropComponent(5, -1, -1),
            new PedPropComponent(6, -1, -1),
            new PedPropComponent(7, -1, -1),
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
        List<StoredWeapon> FemalePoliceWeapons = new List<StoredWeapon>
        {
            new StoredWeapon(0xFAD1F1C9, Vector3.Zero, new WeaponVariation(), 90),
            new StoredWeapon(0xBFE256D4, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(911657153, Vector3.Zero, new WeaponVariation(), 0),//stun gun
        };
        GameSave FemalePoliceSave = new GameSave("Officer Alexis Davis", 120000, "MP_F_FREEMODE_01", false, FemalePolice, FemalePoliceWeapons, new List<VehicleVariation>() { new VehicleVariation("police2", -1, -1, new LSR.Vehicles.LicensePlate("LS 1299", 3, false), new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f)
                                                                                                                                                                });
        FemalePoliceSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        FemalePoliceSave.PlayerHeading = 45f;
        FemalePoliceSave.CurrentDateTime = System.DateTime.Now;

        PedVariation MalePoliceVariation = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 38, 0, 0) ,
            new PedComponent(3, 0, 0, 0) ,
            new PedComponent(4, 35, 0, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 25, 0, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 58, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 55, 0, 0)
        },
        new List<PedPropComponent>()
        {
            new PedPropComponent(0, -1, -1),
            new PedPropComponent(1, -1, -1),
            new PedPropComponent(2, -1, -1),
            new PedPropComponent(3, -1, -1),
            new PedPropComponent(4, -1, -1),
            new PedPropComponent(5, -1, -1),
            new PedPropComponent(6, -1, -1),
            new PedPropComponent(7, -1, -1),
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
        List<StoredWeapon> MalePoliceWeapons = new List<StoredWeapon>
        {
            new StoredWeapon(0xFAD1F1C9, Vector3.Zero, new WeaponVariation(), 90),
            new StoredWeapon(0xBFE256D4, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(911657153, Vector3.Zero, new WeaponVariation(), 0),//stun gun
        };
        GameSave MalePoliceSave = new GameSave("Officer Sawyer Ward", 120000, "MP_M_FREEMODE_01", true, MalePoliceVariation, MalePoliceWeapons, new List<VehicleVariation>() { new VehicleVariation("police2", -1, -1, new LSR.Vehicles.LicensePlate("LS 1299", 3, false), new Vector3(-372.865936f, -308.577576f, 32.1299629f), 280.34967f)
                                                                                                                                                                });
        MalePoliceSave.PlayerPosition = new Vector3(-368.985046f, -305.745453f, 32.7422867f);
        MalePoliceSave.PlayerHeading = 45f;
        MalePoliceSave.CurrentDateTime = System.DateTime.Now;










        //PedVariation SawyerVariation = new PedVariation(new List<PedComponent>()
        //{
        //    new PedComponent(0, 0, 0, 0),
        //    new PedComponent(1, 0, 0, 0),
        //    new PedComponent(2, 38, 0, 0) ,
        //    new PedComponent(3, 14, 0, 0) ,
        //    new PedComponent(4, 25, 0, 0) ,
        //    new PedComponent(5, 0, 0, 0) ,
        //    new PedComponent(6, 3, 2, 0) ,
        //    new PedComponent(7, 0, 0, 0) ,
        //    new PedComponent(8, 11, 5, 0) ,
        //    new PedComponent(9, 0, 0, 0) ,
        //    new PedComponent(10, 0, 0, 0) ,
        //    new PedComponent(11, 122, 8, 0)
        //},
        //new List<PedPropComponent>()
        //{
        //    new PedPropComponent(0, -1, -1),
        //    new PedPropComponent(1, -1, -1),
        //    new PedPropComponent(2, -1, -1),
        //    new PedPropComponent(3, -1, -1),
        //    new PedPropComponent(4, -1, -1),
        //    new PedPropComponent(5, -1, -1),
        //    new PedPropComponent(6, -1, -1),
        //    new PedPropComponent(7, -1, -1),
        //}, 
        //new List<HeadOverlayData>() {
        //    new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
        //    new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        //    new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },},
        //new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f),
        //3,
        //3);
        //List<StoredWeapon> SawyerWeapons = new List<StoredWeapon>
        //{
        //    new StoredWeapon(0x2B5EF5EC, Vector3.Zero, new WeaponVariation(), 45),
        //    new StoredWeapon(2508868239, Vector3.Zero, new WeaponVariation(), 0),
        //};
        //GameSave SawyerGameSave = new GameSave("Sawyer Ward", 45000, "MP_M_FREEMODE_01", true, SawyerVariation, SawyerWeapons, new List<VehicleVariation>() { new VehicleVariation("furoregt", 111, 111, new LSR.Vehicles.LicensePlate("125JK34", 0, false), Vector3.Zero, 0f) });

        PedVariation AlexisVariation2 = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 9, 0, 0) ,
            new PedComponent(4, 9, 11, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 3, 12, 0) ,
            new PedComponent(7, 2, 0, 0) ,
            new PedComponent(8, 0, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 9, 13, 0)
        },
        new List<PedPropComponent>()
        {
                new PedPropComponent(0, -1, -1),
                new PedPropComponent(1, -1, -1),
                new PedPropComponent(2, -1, -1),
                new PedPropComponent(3, -1, -1),
                new PedPropComponent(4, -1, -1),
                new PedPropComponent(5, -1, -1),
                new PedPropComponent(6, -1, -1),
                new PedPropComponent(7, -1, -1),
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
        57,
        5);
        List<StoredWeapon> AlexisWeapons2 = new List<StoredWeapon>
        {
            new StoredWeapon(4222310262, Vector3.Zero, new WeaponVariation(), 0),
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 60),
            new StoredWeapon(3756226112, Vector3.Zero, new WeaponVariation(), 0),
        };
        GameSave JenniferGameSave = new GameSave("Jennifer Lemont", 15500, "MP_F_FREEMODE_01", false, AlexisVariation2, AlexisWeapons2, new List<VehicleVariation>() { new VehicleVariation("sentinel", 0, 0, new LSR.Vehicles.LicensePlate("FG33456A", 0, false), Vector3.Zero, 0f) });
        GameSaveList = new List<GameSave>
        {
            AlexisGameSave,
            FemalePoliceSave,
            MalePoliceSave,
            //SawyerGameSave,
           // JenniferGameSave,
        };
    }
}