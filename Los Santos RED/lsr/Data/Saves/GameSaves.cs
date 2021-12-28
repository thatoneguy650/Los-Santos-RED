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
    public void Save(ISaveable player, IWeapons weapons)
    {
        GameSave mySave = GetSave(player);
        if(mySave == null)
        {
            mySave = new GameSave();
            GameSaveList.Add(mySave);
        }
        mySave.Save(player, weapons);    
        Serialization.SerializeParams(GameSaveList, ConfigFileName);
    }
    public void Load(GameSave gameSave, IWeapons weapons, IPedSwap pedSwap, IInventoryable player, ISettingsProvideable settings, IEntityProvideable world)
    {       
        gameSave.Load(weapons, pedSwap, player, settings, world);
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
            });
        List<StoredWeapon> AlexisWeapons = new List<StoredWeapon>
        {
            new StoredWeapon(4222310262, Vector3.Zero, new WeaponVariation(), 0),
            new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 60),
            new StoredWeapon(3756226112, Vector3.Zero, new WeaponVariation(), 0),
        };
        List<HeadOverlay> HeadOverlays = new List<HeadOverlay>() {
            new HeadOverlay(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlay(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(4, "Makeup") { Index = 12,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },};
        HeadBlendData HeadBlend = new HeadBlendData(12,31,0,12,31,0,0.8f,0.2f,0.0f);
        VehicleVariation myVehicle = new VehicleVariation("dominator", 30,1, new LSR.Vehicles.LicensePlate("133AD37",0,false));
        GameSave AlexisGameSave = new GameSave("Alexis Davis", 9500, "MP_F_FREEMODE_01", false, AlexisVariation, AlexisWeapons, myVehicle) { CurrentHeadBlendData = HeadBlend, CurrentHeadOverlays = HeadOverlays, CurrentPrimaryHairColor = 57,CurrentSecondaryColor = 5 };


        PedVariation SawyerVariation = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 38, 0, 0) ,
            new PedComponent(3, 14, 0, 0) ,
            new PedComponent(4, 25, 0, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 3, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 11, 5, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 122, 8, 0)
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
            });
        List<StoredWeapon> SawyerWeapons = new List<StoredWeapon>
        {
            new StoredWeapon(0x2B5EF5EC, Vector3.Zero, new WeaponVariation(), 45),
            new StoredWeapon(2508868239, Vector3.Zero, new WeaponVariation(), 0),
        };
        List<HeadOverlay> SawyerHeadOverlays = new List<HeadOverlay>() {
            new HeadOverlay(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlay(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlay(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },};
        HeadBlendData SawyerHeadBlend = new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f);
        VehicleVariation SawyermyVehicle = new VehicleVariation("furoregt", 111, 111, new LSR.Vehicles.LicensePlate("125JK34", 0, false));
        GameSave SawyerGameSave = new GameSave("Sawyer Ward", 45000, "MP_M_FREEMODE_01", true, SawyerVariation, SawyerWeapons, SawyermyVehicle) { CurrentHeadBlendData = SawyerHeadBlend, CurrentHeadOverlays = SawyerHeadOverlays, CurrentPrimaryHairColor = 3, CurrentSecondaryColor = 3 };



        GameSaveList = new List<GameSave>
        {
            AlexisGameSave,
            SawyerGameSave,
        };

        //PedVariation ClaudeVariation = new PedVariation(new List<PedComponent>()
        //{ 
        //    new PedComponent(0, 0, 0, 0),
        //    new PedComponent(1, 0, 0, 0),
        //    new PedComponent(2, 0, 0, 0) , 
        //    new PedComponent(3, 0, 0, 0) , 
        //    new PedComponent(4, 0, 0, 0) , 
        //    new PedComponent(5, 0, 0, 0) , 
        //    new PedComponent(6, 0, 0, 0) , 
        //    new PedComponent(7, 0, 0, 0) , 
        //    new PedComponent(8, 0, 0, 0) , 
        //    new PedComponent(9, 0, 0, 0) , 
        //    new PedComponent(10, 0, 0, 0) , 
        //    new PedComponent(11, 0, 0, 0) 
        //}, 
        //    new List<PedPropComponent>() 
        //    { 
        //         new PedPropComponent(0, -1, -1),
        //         new PedPropComponent(1, -1, -1),
        //         new PedPropComponent(2, -1, -1),
        //         new PedPropComponent(3, -1, -1),
        //         new PedPropComponent(4, -1, -1),
        //         new PedPropComponent(5, -1, -1),
        //         new PedPropComponent(6, -1, -1),
        //         new PedPropComponent(7, -1, -1),
        //    });
        //List<StoredWeapon> ClaudeWeapons = new List<StoredWeapon>
        //{
        //    new StoredWeapon(4222310262, Vector3.Zero, new WeaponVariation(), 0),
        //    new StoredWeapon(453432689, Vector3.Zero, new WeaponVariation(), 60),
        //    new StoredWeapon(3756226112, Vector3.Zero, new WeaponVariation(), 0),
        //};
        //GameSaveList = new List<GameSave>
        //{

        //    new GameSave("Claude Speed",9500,"MP_M_CLAUDE_01",true,0,ClaudeVariation,ClaudeWeapons),
        //};
    }
}