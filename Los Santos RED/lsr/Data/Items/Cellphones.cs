using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Cellphones : ICellphones
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Cellphones.xml";
    private List<CellphoneData> CellphoneList;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Cellphones*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Cellphones config: {ConfigFile.FullName}", 0);
            CellphoneList = Serialization.DeserializeParams<CellphoneData>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Cellphones config  {ConfigFileName}", 0);
            CellphoneList = Serialization.DeserializeParams<CellphoneData>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Cellphones config found, creating default", 0);
            DefaultConfig();
            DefaultConfig_LosSantos2008();
        }
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CellphoneList == null ? new List<CellphoneData>() : CellphoneList, ConfigFileName);
    }
    private void DefaultConfig()
    {
        CellphoneList = new List<CellphoneData>
        {
            new CellphoneData("iFruit Cellphone",0,"cellphone_ifruit") { IsDefault = true },
            new CellphoneData("Facade Cellphone",1,"cellphone_facade"),
            new CellphoneData("Badger Cellphone",2,"cellphone_badger"),
            new CellphoneData("Celltowa Cellphone",4,"cellphone_facade") { IsRegular = false, IsHistoric = true, HasFlashlight = false },
        };
        Serialization.SerializeParams(CellphoneList, ConfigFileName);
    }
    private void DefaultConfig_LosSantos2008()
    {
        List<CellphoneData> OldCellphoneList = new List<CellphoneData>
        {
            new CellphoneData("Celltowa Cellphone",4,"cellphone_facade") { IsDefault = true, HasFlashlight = false },
        };
        Serialization.SerializeParams(OldCellphoneList, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\Cellphones_LosSantos2008.xml");
    }
    public CellphoneData Get(string name)
    {
        return CellphoneList.FirstOrDefault(x => x.ModItemName == name);
    }
    public CellphoneData GetDefault()
    {
        return CellphoneList.Where(x => x.IsDefault).FirstOrDefault();
    }
    public CellphoneData GetRandomRegular()
    {
        return CellphoneList.Where(x => x.IsRegular).PickRandom();
    }
    public CellphoneData GetPhone(string name)
    {
        return CellphoneList.Where(x => x.ModItemName == name).PickRandom();
    }
}

