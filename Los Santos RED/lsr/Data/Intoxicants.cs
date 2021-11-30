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


public class Itoxicants : IIntoxicants
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ModItems.xml";
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
            new Intoxicant("Marijuana", 60000, 120000, 3.0f, "drug_wobbly"),
            new Intoxicant("Alcohol", 25000, 60000, 3.5f, "Drunk"),
            new Intoxicant("Meth", 25000, 60000, 5.0f, "drug_wobbly"),
            new Intoxicant("Cocaine", 25000, 25000, 5.0f, "drug_wobbly"),
            new Intoxicant("Mushrooms", 25000, 60000, 10.0f, "drug_wobbly"),
            new Intoxicant("Nicotine", 120000, 60000, 1.0f, "drug_wobbly"),
        };
    }
    public Intoxicant Get(string name)
    {
        return IntoxicantList.FirstOrDefault(x => x.Name == name);
    }
}

