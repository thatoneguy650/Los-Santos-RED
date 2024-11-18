using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Heads : IHeads
{

    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Heads.xml";
    private List<HeadDataGroup> RandomHeadDataLookup;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Heads*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Heads config: {ConfigFile.FullName}", 0);
            RandomHeadDataLookup = Serialization.DeserializeParams<HeadDataGroup>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Heads config  {ConfigFileName}", 0);
            RandomHeadDataLookup = Serialization.DeserializeParams<HeadDataGroup>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Heads config found, creating default", 0);
            DefaultConfig();
        }
    }
    private void DefaultConfig()
    {
        RandomHeadDataLookup = new List<HeadDataGroup>();

        //HEADS
        List<int> WhiteHairStyles_Male = new List<int>() { 2, 3, 4, 5, 7, 9, 10, 11, 12, 18, 19, 31, 40, 41, 43, 45, 46, 54, 55, 66, 80 };
        List<int> BrownHairStyles_Male = new List<int>() { 2, 3, 4, 9, 10, 11, 12, 18, 19, 31, 40, 41, 43, 45, 46, 54, 55, 66, 80 };
        List<int> AsianHairStyles_Male = new List<int>() { 2, 3, 4, 9, 10, 11, 12, 18, 19, 31, 40, 41, 43, 45, 46, 54, 55, 66, 80 };
        List<int> BlackHairStyles_Male = new List<int>() { 0, 1, 8, 14, 24, 25, 30, 72 };

        List<int> WhiteHairColors_Male = new List<int>() { 0, 2, 3, 4, 7, 8, 9, 10, 11, 12, 13 };
        List<int> BrownHairColors_Male = new List<int>() { 0, 2, 3 };
        List<int> AsianHairColors_Male = new List<int>() { 0, 2, 3 };
        List<int> BlackHairColors_Male = new List<int>() { 0, 2, 3 };

        List<int> WhiteHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
        List<int> BrownHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
        List<int> AsianHairStyles_Female = new List<int>() { 1, 2, 4, 10, 11, 14, 15, 16, 17, 21, 48 };
        List<int> BlackHairStyles_Female = new List<int>() { 6, 2, 4, 10, 11, 20, 22, 25, 54, 58 };

        List<int> WhiteHairColors_Female = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        List<int> BrownHairColors_Female = new List<int>() { 0, 1, 2, 3 };
        List<int> AsianHairColors_Female = new List<int>() { 0, 1, 2, 3 };
        List<int> BlackHairColors_Female = new List<int>() { 0, 1, 2, 3 };

        List<int> WhiteEyeColors = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        List<int> BrownEyeColors = new List<int>() { 1, 4, 5, 6 };
        List<int> AsianEyeColors = new List<int>() { 1, 4, 5, 6 };
        List<int> BlackEyeColors = new List<int>() { 1, 4, 5, 6 };

        List<int> MafiaHairColors_Male = new List<int>() { 0, 1,2,3,4 };
        List<int> MafiaHairStyles_Male = new List<int>() { 4, 5, 7, 9, 10, 12, 18, 19, 31,40,41,43,45,46,54,55,66,80 };

        List<int> VariosHairStyles_Male = new List<int>() { 1,12,31,35 };
        List<int> VarriosHairStyles_Female = new List<int>() { 1, 3, 4, 10, 11, 14, 16, 31,36 };

        List<RandomHeadData> AllHeadsList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(1,"Daniel",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(5,"Juan",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(12,"Diego",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(13,"Adrian",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(42,"Claude",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(43,"Niko",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),///white male
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(8,"Evan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true),//brown male
                        new RandomHeadData(7,"Isaac",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true),//asian male
                        new RandomHeadData(17,"Kevin",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true),//asian male
                        new RandomHeadData(18,"Louis",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true),//asian male            

                        new RandomHeadData(6,"Alex",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(27,"Zoe",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(28,"Ava",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(39,"Elizabeth",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(33,"Nicole",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(34,"Ashley",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(21,"Hannah",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(22,"Audrey",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(40,"Charlotte",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(45,"Misty",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(25,"Amelia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(26,"Isabella",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(29,"Camila",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(30,"Violet",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(31,"Sophia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(32,"Evelyn",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(37,"Natalie",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(38,"Olivia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female     
                        new RandomHeadData(41,"Emma",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female            
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("AllHeads", AllHeadsList));
        List<RandomHeadData> LostMCHeadsList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 55f },//white male
                        new RandomHeadData(1,"Daniel",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(5,"Juan",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(12,"Diego",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(13,"Adrian",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(42,"Claude",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(43,"Niko",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },//white male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 55f },///white male
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true){ FacialHairPercentage = 55f },//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true){ FacialHairPercentage = 55f },//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true){ FacialHairPercentage = 55f },//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true){ FacialHairPercentage = 55f },//black male
                        new RandomHeadData(8,"Evan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 55f },//brown male
                        new RandomHeadData(7,"Isaac",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true){ FacialHairPercentage = 55f },//asian male
                        new RandomHeadData(17,"Kevin",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true){ FacialHairPercentage = 55f },//asian male
                        new RandomHeadData(18,"Louis",AsianHairColors_Male,AsianHairStyles_Male,AsianEyeColors, true){ FacialHairPercentage = 55f },//asian male            

                        new RandomHeadData(6,"Alex",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(27,"Zoe",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(28,"Ava",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(39,"Elizabeth",AsianHairColors_Female,AsianHairStyles_Female,AsianEyeColors,false),//asian female
                        new RandomHeadData(33,"Nicole",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(34,"Ashley",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(21,"Hannah",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(22,"Audrey",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(40,"Charlotte",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(45,"Misty",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(25,"Amelia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(26,"Isabella",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(29,"Camila",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(30,"Violet",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(31,"Sophia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(32,"Evelyn",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(37,"Natalie",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female
                        new RandomHeadData(38,"Olivia",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female     
                        new RandomHeadData(41,"Emma",BrownHairColors_Female,BrownHairStyles_Female,BrownEyeColors,false),//brown female            
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("LostMCHeads", LostMCHeadsList));
        List<RandomHeadData> VarriosHeadsList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(8,"Evan",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,VariosHairStyles_Male,BrownEyeColors, true){ FacialHairPercentage = 65f },//brown male        

                        new RandomHeadData(25,"Amelia",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false) { LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(26,"Isabella",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(29,"Camila",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(30,"Violet",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(31,"Sophia",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(32,"Evelyn",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(37,"Natalie",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female
                        new RandomHeadData(38,"Olivia",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female     
                        new RandomHeadData(41,"Emma",BrownHairColors_Female,VarriosHairStyles_Female,BrownEyeColors,false){ LipstickAndMakeupPercentage = 95f, },//brown female            
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("VarriosHeads", VarriosHeadsList));
        List<RandomHeadData> MafiaHeadList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(1,"Daniel",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(4,"Andrew",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(5,"Juan",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(12,"Diego",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(13,"Adrian",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(42,"Claude",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(43,"Niko",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(44,"John",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("MafiaHeads", MafiaHeadList));
        List<RandomHeadData> DiablosHeadList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 45f, },//white male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f, },//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 45f },///white male
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("DiablosHeads", DiablosHeadList));

        List<RandomHeadData> PetrovicHeadsList = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(1,"Daniel",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(4,"Andrew",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(5,"Juan",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(12,"Diego",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(13,"Adrian",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(42,"Claude",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(43,"Niko",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(44,"John",MafiaHairColors_Male,MafiaHairStyles_Male,WhiteEyeColors, true){ FacialHairPercentage = 0.0f,AgingPercentage = 40f },//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 45f, },//white male
                        new RandomHeadData(9,"Ethan",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f, },//brown male
                        new RandomHeadData(10,"Vincent",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(11,"Angel",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(16,"Santiago",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(20,"Anthony",BrownHairColors_Male,BrownHairStyles_Male,BrownEyeColors, true) { FacialHairPercentage = 45f },//brown male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true) { FacialHairPercentage = 45f },///white male
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("PetrovicHeads", PetrovicHeadsList));


        List<RandomHeadData> UptownRidersHeads = new List<RandomHeadData>()
                    {
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
          
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
       
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("UptownRidersHeads", UptownRidersHeads));


        List<RandomHeadData> NorthHollandHeads = new List<RandomHeadData>()
                    {
                        new RandomHeadData(2,"Joshua",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(3,"Noah",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(15,"Michael",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
                        new RandomHeadData(19,"Samuel",BlackHairColors_Male,BlackHairStyles_Male,BlackEyeColors, true),//black male
          
                        new RandomHeadData(14,"Gabriel",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(23,"Jasmine",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(24,"Giselle",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(35,"Grace",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
                        new RandomHeadData(36,"Brianna",BlackHairColors_Female,BlackHairStyles_Female,BlackEyeColors,false),//black female
       
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("NorthHollandHeads", NorthHollandHeads));


        List<RandomHeadData> AngelsOfDeathHeads = new List<RandomHeadData>()
                    {
                        new RandomHeadData(0,"Benjamin",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(1,"Daniel",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(4,"Andrew",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(5,"Juan",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(12,"Diego",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(13,"Adrian",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(42,"Claude",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(43,"Niko",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),//white male
                        new RandomHeadData(44,"John",WhiteHairColors_Male,WhiteHairStyles_Male,WhiteEyeColors, true),///white male      


                        new RandomHeadData(33,"Nicole",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(34,"Ashley",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(21,"Hannah",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(22,"Audrey",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(40,"Charlotte",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
                        new RandomHeadData(45,"Misty",WhiteHairColors_Female,WhiteHairStyles_Female,WhiteEyeColors,false),//white female
        
                    };
        RandomHeadDataLookup.Add(new HeadDataGroup("AngelsOfDeathHeads", AngelsOfDeathHeads));


        Serialization.SerializeParams(RandomHeadDataLookup, ConfigFileName);






    }
    public List<RandomHeadData> GetHeadData(string headDataGroupID)
    {
        return RandomHeadDataLookup.FirstOrDefault(x => x.HeadDataGroupID == headDataGroupID)?.HeadList;
    }
}


