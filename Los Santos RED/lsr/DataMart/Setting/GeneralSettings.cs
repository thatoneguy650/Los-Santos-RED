using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralSettings
{
    public bool AliasPedAsMainCharacter = true;
    public string MainCharacterToAlias = "Michael";
    public bool Keanu = true;
    public int UndieLimit = 0;
    public bool Debug = false;
    public bool PedTakeoverSetRandomMoney = true;
    public int PedTakeoverRandomMoneyMin = 500;
    public int PedTakeoverRandomMoneyMax = 5000;
    public bool AlwaysShowCash = true;
    public bool Logging = true;
    public bool AllowDeathMenus = true;
    public bool AlwaysShowRadar = true;
    public bool AlwaysShowHUD = true;
    public string MainCharacterToAliasModelName
    {
        get
        {
            if (MainCharacterToAlias == "Michael")
                return "player_zero";
            else if (MainCharacterToAlias == "Franklin")
                return "player_one";
            else if (MainCharacterToAlias == "Trevor")
                return "player_two";
            else
                return "player_zero";
        }
    }
    public GeneralSettings()
    {

    }

}