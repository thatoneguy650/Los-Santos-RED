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
    public void Load(GameSave gameSave, IWeapons weapons, IPedSwap pedSwap)
    {
        gameSave.Load(weapons, pedSwap);
    }
    public GameSave GetSave(ISaveable player)
    {
        return GameSaveList.FirstOrDefault(x => x.PlayerName == player.PlayerName && x.ModelName == player.ModelName);
    }
    public void DeleteSave(string playerName, string modelName)
    {
        GameSave toDelete = GameSaveList.FirstOrDefault(x => x.PlayerName == playerName && x.ModelName == modelName);
        if(toDelete != null)
        {
            GameSaveList.Remove(toDelete);
        }
    }
    private void DefaultConfig()
    {
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