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
    public List<TestAnimation> Animations { get; private set; } = new List<TestAnimation>();

    public TestAnimations()
    {

    }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Test Animations {ConfigFileName}", 0);
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(ConfigFileName);
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