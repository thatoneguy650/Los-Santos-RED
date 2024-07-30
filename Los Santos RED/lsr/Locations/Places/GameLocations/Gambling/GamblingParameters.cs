using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingParameters
{
    public List<BlackJackGameRules> BlackJackGameRulesList { get; set; } = new List<BlackJackGameRules>();
    public List<RouletteGameRules> RouletteGameRulesList { get; set; } = new List<RouletteGameRules>();
    public GamblingParameters()
    {

    }
}

