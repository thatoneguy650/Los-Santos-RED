using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedSwapSettings : ISettingsDefaultable
{
    [Description("Uses memory editing to set the model name of whatever ped you are playing as to be the of one of the three main characters. Lets you do things only main characters are allowed to do (like spend money, shop, have random encounters, own property, etc.).")]
    public bool AliasPedAsMainCharacter { get; set; }
    [Description("Determines the model name to alias your current ped as. Will only work with the options Michael, Franklin, or Trevor. Anything else will be ignored and it will not alias. The actual model that will be returned to the game/mods will be player_zero (Michael) player_one (Franklin) or player_two (Trevor).")]
    public string MainCharacterToAlias { get; set; }
    [Description("Randomizes money upon takeover")]
    public bool SetRandomMoney { get; set; }
    [Description("Minimum amount of money to be given upon takeover")]
    public int RandomMoneyMin { get; set; }
    [Description("Maximum amount of money to be given upon takeover")]
    public int RandomMoneyMax { get; set; }
    [Description("Percentage of time you will get a random low end weapon when taking over a random ped")]
    public int PercentageToGetRandomWeapon { get; set; }
    [Description("Percentage of time you will get random criminal history when taking over a random ped")]
    public int PercentageToGetCriminalHistory { get; set; }
    [Description("Percentage of time you will get random items when taking over a random ped (weapons and vehicles excluded)")]
    public float PercentageToGetRandomItems { get; set; }
    [Description("Max number of random items to get when taking over a random ped (weapons and vehicles excluded). Requires PercentageToGetRandomItems > 0")]
    public int MaxRandomItemsToGet { get; set; }
    [Description("Max amount to get for each random item when taking over a random ped (weapons and vehicles excluded). Requires PercentageToGetRandomItems > 0")]
    public int MaxRandomItemsAmount { get; set; }

    public PedSwapSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

#if DEBUG
        AliasPedAsMainCharacter = false;
#else
        AliasPedAsMainCharacter = true;
#endif

        MainCharacterToAlias = "Michael";
        SetRandomMoney = true;
        RandomMoneyMin = 500;
        RandomMoneyMax = 5000;
        PercentageToGetRandomWeapon = 20;
        PercentageToGetCriminalHistory = 5;
        PercentageToGetRandomItems = 80;
        MaxRandomItemsToGet = 5;
        MaxRandomItemsAmount = 7;
    }
}