using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TestAnimations
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\PedAnimList.txt";
    private readonly string ConfigFileName2 = "Plugins\\LosSantosRED\\debug\\PedAnimList.txt";
    public List<TestAnimation> Animations { get; private set; } = new List<TestAnimation>();

    public TestAnimations()
    {

    }
    public void ReadConfig()
    {
        string configToUse = ConfigFileName;
        if (File.Exists(ConfigFileName))
        {
            configToUse = ConfigFileName;
        }
        else if (File.Exists(ConfigFileName2))
        {
            configToUse = ConfigFileName2;
        }
        if (File.Exists(configToUse))
        {
            EntryPoint.WriteToConsole($"Loaded Test Animations {configToUse}", 0);
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(configToUse);
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(' ');
                if(words.Length == 2 )
                {
                    Animations.Add(new TestAnimation(words[0], words[1]));
                }

            }
            file.Close(); 
        }       
    }

}