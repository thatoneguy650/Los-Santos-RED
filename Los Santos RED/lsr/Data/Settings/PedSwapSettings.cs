using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedSwapSettings
{
    public bool AliasPedAsMainCharacter { get; set; } = true;
    public string MainCharacterToAlias { get; set; } = "Michael";
    public bool SetRandomMoney { get; set; } = true;
    public int RandomMoneyMin { get; set; } = 500;
    public int RandomMoneyMax { get; set; } = 5000;
    public int PercentageToGetRandomWeapon { get; set; } = 20;
    public int PercentageToGetCriminalHistory { get; set; } = 5;
    public PedSwapSettings()
    { 
    }
}