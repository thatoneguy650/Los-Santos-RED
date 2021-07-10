using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralSettings
{
    public string MainCharacterToAlias { get; set; } = "Michael";

    public bool PedTakeoverSetRandomMoney { get; set; } = true;
    public int PedTakeoverRandomMoneyMin { get; set; } = 500;
    public int PedTakeoverRandomMoneyMax { get; set; } = 5000;

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