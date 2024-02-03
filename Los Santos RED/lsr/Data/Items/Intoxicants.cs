using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Intoxicants : IIntoxicants
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Itoxicants.xml";
    private List<Intoxicant> IntoxicantList;
    public List<Intoxicant> Items => IntoxicantList;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Itoxicants*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Intoxicants config: {ConfigFile.FullName}",0);
            IntoxicantList = Serialization.DeserializeParams<Intoxicant>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Intoxicants config  {ConfigFileName}",0);
            IntoxicantList = Serialization.DeserializeParams<Intoxicant>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Intoxicants config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(IntoxicantList == null ? new List<Intoxicant>() : IntoxicantList, ConfigFileName);
    }
    private void DefaultConfig()
    {
        IntoxicantList = new List<Intoxicant>
        {
            new Intoxicant("Marijuana", 60000, 120000, 3.0f, "Barry1_Stoned", IntoxicationEffect.RelaxesMuscles) { NoCameraShake = true, EffectIntoxicationLimit = 0.5f },
            new Intoxicant("Low Proof Alcohol", 25000, 60000, 3.5f, "Drunk",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving),

            new Intoxicant("High Proof Alcohol", 10000, 60000, 4.0f, "Drunk",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving),


            new Intoxicant("Mushrooms", 25000, 60000, 10.0f, "DRUG_gas_huffin",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Nicotine", 120000, 60000, 1.0f, "HeatHaze") { NoCameraShake = true, },





            new Intoxicant("SPANK", 45000, 40000, 5.0f, "BeastIntro01",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Cocaine", 20000, 30000, 5.0f, "BikerFormFlash",IntoxicationEffect.ImparesDriving | IntoxicationEffect.InfiniteStamina | IntoxicationEffect.FastSpeed) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Methamphetamine", 30000, 60000, 5.0f, "BeastIntro02",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving | IntoxicationEffect.InfiniteStamina | IntoxicationEffect.FastSpeed),
            new Intoxicant("Toilet Cleaner", 10000, 60000, 5.0f, "dying",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Heroin", 15000, 60000, 5.0f, "dying",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Crack", 20000, 30000, 5.0f, "BikerFormFlash",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving | IntoxicationEffect.InfiniteStamina) {  EffectIntoxicationLimit = 0.5f },




            new Intoxicant("Diazepam", 60000, 120000, 4.0f, "", IntoxicationEffect.RelaxesMuscles) { NoCameraShake = true, EffectIntoxicationLimit = 0.25f,ContinuesWithoutCurrentUse = true },


            new Intoxicant("Bull Shark Testosterone", 45000, 60000, 2.0f, "drug_wobbly",IntoxicationEffect.InfiniteStamina | IntoxicationEffect.FastSpeed) { NoCameraShake = true, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Alco Patch", 25000, 60000, 4.0f, "Drunk",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Mollis", 15000, 60000, 3.0f, "drug_wobbly",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) {  NoCameraShake = true,EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Chesty", 10000, 60000, 1.0f, "HeatHaze",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { NoCameraShake = true, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Equanox", 30000, 60000, 5.0f, "drug_wobbly",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Zombix", 25000, 60000, 5.0f, "BeastIntro01",IntoxicationEffect.ImparesWalking | IntoxicationEffect.ImparesDriving) {  NoCameraShake = true,EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
        };
        Serialization.SerializeParams(IntoxicantList, ConfigFileName);
    }
    public Intoxicant Get(string name)
    {
        return IntoxicantList.FirstOrDefault(x => x.Name == name);
    }
}

