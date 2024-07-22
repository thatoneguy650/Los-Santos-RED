using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingParameters
{
    public string DealerName { get; set; } = "Dealer";
    public bool HasBlackjack { get; set; } = true;
    public int BlackJackMinBet { get; set; } = 5;
    public int BlackJackMaxBet { get; set; } = 1500;
    public bool BlackJackCanSurrender { get; set; } = true;
    public bool BlackJackCanSplit { get; set; } = true;
    public bool HasPoker { get; set; } = true;
    public bool HasRoulette { get; set; } = true;
    public GamblingParameters()
    {

    }
}

