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
        if (File.Exists(ConfigFileName))
        {
            IntoxicantList = Serialization.DeserializeParams<Intoxicant>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(IntoxicantList, ConfigFileName);
        }
    }
    private void DefaultConfig()
    {
        IntoxicantList = new List<Intoxicant>
        {
            new Intoxicant("Marijuana", 60000, 120000, 3.0f, "Barry1_Stoned") {  EffectIntoxicationLimit = 0.5f },
            new Intoxicant("Alcohol", 25000, 60000, 3.5f, "Drunk",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving),
            new Intoxicant("Mushrooms", 25000, 60000, 10.0f, "DRUG_gas_huffin",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Nicotine", 120000, 60000, 1.0f, "HeatHaze"),


            new Intoxicant("SPANK", 45000, 40000, 5.0f, "BeastIntro01",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Cocaine", 20000, 30000, 5.0f, "BikerFormFlash",IntoxicationEffect.CausesSwerving | IntoxicationEffect.InfiniteStamina) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Methamphetamine", 30000, 60000, 5.0f, "BeastIntro02",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving | IntoxicationEffect.InfiniteStamina),
            new Intoxicant("Toilet Cleaner", 10000, 60000, 5.0f, "dying",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Heroin", 15000, 60000, 5.0f, "dying",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Crack", 20000, 30000, 5.0f, "BikerFormFlash",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving | IntoxicationEffect.InfiniteStamina) {  EffectIntoxicationLimit = 0.5f },


            new Intoxicant("Bull Shark Testosterone", 45000, 60000, 2.0f, "drug_wobbly") { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Alco Patch", 25000, 60000, 4.0f, "Drunk",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Mollis", 15000, 60000, 3.0f, "drug_wobbly",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
            new Intoxicant("Chesty", 10000, 60000, 1.0f, "HeatHaze",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Equanox", 30000, 60000, 5.0f, "drug_wobbly",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) { ContinuesWithoutCurrentUse = true },
            new Intoxicant("Zombix", 25000, 60000, 5.0f, "BeastIntro01",IntoxicationEffect.CausesStumbling | IntoxicationEffect.CausesSwerving) {  EffectIntoxicationLimit = 0.5f, ContinuesWithoutCurrentUse = true },
        };
    }
    public Intoxicant Get(string name)
    {
        return IntoxicantList.FirstOrDefault(x => x.Name == name);
    }
}

